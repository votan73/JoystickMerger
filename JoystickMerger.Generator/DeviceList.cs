using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JoystickMerger.Generator
{
    class DeviceList : TableLayoutPanel, IMapItem
    {
        public DeviceList()
        {
            base.ColumnCount = 1;
            base.RowCount = 0;
            base.AutoSize = true;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [DefaultValue(true), EditorBrowsable(EditorBrowsableState.Never)]
        public new bool AutoSize { get { return base.AutoSize; } set { } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [DefaultValue(1), EditorBrowsable(EditorBrowsableState.Never)]
        public new int RowCount { get { return base.RowCount; } set { base.RowCount = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [DefaultValue(1), EditorBrowsable(EditorBrowsableState.Never)]
        public new int ColumnCount { get { return base.ColumnCount; } set { base.ColumnCount = 1; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
        public new Size Size { get { return base.Size; } set { base.Size = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
        public new TableLayoutRowStyleCollection RowStyles { get { return base.RowStyles; } }

        protected override void OnControlAdded(ControlEventArgs e)
        {
            RowCount = Controls.Count;
            SetRow(e.Control, RowCount - 1);
            base.OnControlAdded(e);
            for (int i = 0; i < RowCount; i++)
                Controls.SetChildIndex(GetControlFromPosition(0, i), i);
        }

        protected override void OnControlRemoved(ControlEventArgs e)
        {
            RowCount = Controls.Count;
            base.OnControlRemoved(e);
        }

        public void ToXml(System.Xml.XmlNode joysticks)
        {
            foreach (var item in Items)
            {
                var joystick = joysticks.AddElement("Joystick");
                joystick.SetAttribute("name", item.Item.Name);
                joystick.SetAttribute("preferred", item.Item.Device.InstanceGuid.ToString());
                joystick.SetAttribute("deadzone", item.DeadZone.ToString(System.Globalization.CultureInfo.GetCultureInfo(9)));
            }
        }

        public IEnumerable<DeviceListItem> Items { get { return this.Controls.OfType<DeviceListItem>(); } }

        public void FromXml(System.Xml.XmlNode joysticks)
        {
            Action<DeviceListItem, System.Xml.XmlNode> setDeadzone = (item, joystick) =>
            {
                float value;
                if (Single.TryParse(joystick.GetAttribute("deadzone"), System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.CultureInfo.GetCultureInfo(9), out value))
                    item.DeadZone = value;
            };

            var list = new HashSet<System.Xml.XmlNode>(joysticks.ChildNodes.Cast<System.Xml.XmlNode>().Where<System.Xml.XmlNode>(x => x.Name == "Joystick"));
            var items = new List<DeviceListItem>(Items);
            for (int i = items.Count - 1; i >= 0; i--)
            {
                var item = items[i];
                var id = item.Item.Device.InstanceGuid.ToString();
                foreach (var joystick in list)
                {
                    if (joystick.GetAttribute("preferred") == id)
                    {
                        setDeadzone(item, joystick);
                        list.Remove(joystick);
                        items.RemoveAt(i);
                        break;
                    }
                }
            }
            for (int i = items.Count - 1; i >= 0; i--)
            {
                var item = items[i];
                var id = item.Item.Name;
                foreach (var joystick in list)
                {
                    if (joystick.GetAttribute("name") == id)
                    {
                        setDeadzone(item, joystick);
                        list.Remove(joystick);
                        break;
                    }
                }
            }
        }

        public DeviceListItem RecentTouchedDevice()
        {
            var highest = DateTime.MinValue;
            DeviceListItem highestItem = null;
            foreach (var item in this.Items)
            {
                if (item.LastChangeDectection > highest)
                {
                    highest = item.LastChangeDectection;
                    highestItem = item;
                }
            }
            return highestItem;
        }

        public void Initialize(CompileInfo info)
        {
        }

        public void Declaration(CompileInfo info, System.IO.StreamWriter file)
        {
            file.Write("        const int numDevices = ");
            file.Write(info.Joysticks.Count);
            file.WriteLine(";");

            var nameToDevice = new Dictionary<string, DeviceListItem>();
            foreach (var device in Items)
                nameToDevice[device.Item.Key] = device;
            file.WriteLine();

            int index = 0;
            foreach (var joystick in info.Joysticks)
            {
                file.Write("        const int ");
                file.Write(joystick.Replace("joystick", "deadzone"));
                file.Write(" = ");
                file.Write(Convert.ToInt32(nameToDevice[joystick].DeadZone * 327.68f));
                file.WriteLine(";");
            }
            file.WriteLine();

            file.WriteLine("        readonly string[] deviceNames = new string[]{");
            foreach (var joystick in info.Joysticks)
            {
                file.Write("            \"");
                file.Write(nameToDevice[joystick].Text);
                file.WriteLine("\",");
            }
            file.WriteLine("        };");
            file.WriteLine();

            index = 0;
            foreach (var joystick in info.Joysticks)
            {
                file.Write("        Joystick ");
                file.Write(joystick.Replace("joystick", "joystickDevice"));
                file.WriteLine(";");
            }
        }

        private void FindDevices(CompileInfo info, System.IO.StreamWriter file)
        {
            var nameToDevice = new Dictionary<string, DeviceListItem>();
            foreach (var device in Items)
                nameToDevice[device.Item.Key] = device;

            file.WriteLine("        private void FindDevices(IList<DeviceInstance> gameControllerList, IdentifierList preferred)");
            file.WriteLine("        {");
            int index = 0;
            foreach (var joystick in info.Joysticks)
            {
                file.Write("            var newJoy"); file.Write(index + 1); file.WriteLine(" = Guid.Empty;");
                file.Write("            var preferJoy"); file.Write(index + 1); file.Write(" = preferred["); file.Write(index); file.WriteLine("];");
                index++;
            }
            file.WriteLine();

            file.WriteLine("            foreach (var deviceInstance in gameControllerList)");
            file.WriteLine("            {");

            index = 0;
            foreach (var joystick in info.Joysticks)
            {
                file.Write("                if (DetectDevice(deviceInstance, ref ");
                file.Write(joystick.Replace("joystick", "joystickDevice"));
                file.Write(", \"");
                file.Write(nameToDevice[joystick].Item.Device.InstanceName);
                file.Write("\", ref preferJoy");
                file.Write(index + 1);
                file.Write(", (guid) => mainForm.SetDevicesState(");
                file.Write(index);
                file.WriteLine(", true, ref guid)))");
                file.WriteLine("                    continue;");

                index++;
            }
            file.WriteLine();

            index = 0;
            foreach (var joystick in info.Joysticks)
            {
                file.Write("                if (DetectDevice(deviceInstance, ref ");
                file.Write(joystick.Replace("joystick", "joystickDevice"));
                file.Write(", \"");
                file.Write(nameToDevice[joystick].Item.Device.InstanceName);
                file.Write("\", ref newJoy");
                file.Write(index + 1);
                file.Write(", (guid) => mainForm.SetDevicesState(");
                file.Write(index);
                file.WriteLine(", true, ref guid)))");
                file.WriteLine("                {");
                file.Write("                    preferJoy");
                file.Write(index + 1);
                file.Write(" = newJoy");
                file.Write(index + 1);
                file.WriteLine(";");
                file.WriteLine("                    continue;");
                file.WriteLine("                }");
                index++;
            }
            file.WriteLine("            }");


            index = 0;
            foreach (var joystick in info.Joysticks)
            {
                file.Write("            preferred["); file.Write(index); file.Write("] = preferJoy"); file.Write(index + 1); file.WriteLine(";");
                index++;
            }

            file.WriteLine("        }");
            file.WriteLine();
        }

        private void AllDevicesReady(CompileInfo info, System.IO.StreamWriter file)
        {
            file.WriteLine("        private bool AllDevicesReady()");
            file.WriteLine("        {");
            file.Write("            return ");

            var list = new List<string>();
            foreach (var joystick in info.Joysticks)
                list.Add(String.Concat(joystick.Replace("joystick", "joystickDevice"), " != null"));
            if (list.Count > 0)
                file.Write(String.Join(" && ", list));
            else
                file.Write("false");
            file.WriteLine(";");
            file.WriteLine("        }");
            file.WriteLine();
        }

        private void ReleaseDevices(CompileInfo info, System.IO.StreamWriter file)
        {
            file.WriteLine("        private void ReleaseDevices()");
            file.WriteLine("        {");

            foreach (var joystick in info.Joysticks)
            {
                var deviceName = joystick.Replace("joystick", "joystickDevice");
                file.Write("            if ("); file.Write(deviceName); file.Write(" != null && !"); file.Write(deviceName); file.WriteLine(".IsDisposed)");
                file.Write("                "); file.Write(deviceName); file.WriteLine(".Dispose();");
            }
            file.WriteLine("        }");
            file.WriteLine();
        }

        public void PreFeed(CompileInfo info, System.IO.StreamWriter file)
        {
            AllDevicesReady(info, file);
            FindDevices(info, file);
        }

        public void Feed(CompileInfo info, System.IO.StreamWriter file)
        {
            foreach (var joystick in info.Joysticks)
            {
                file.Write("            "); file.Write(joystick.Replace("joystick", "joystickDevice")); file.WriteLine(".Poll();");
                file.Write("            // update the joystick "); file.Write(joystick.Replace("joystick", "state")); file.WriteLine(" field");
                file.Write("            var "); file.Write(joystick.Replace("joystick", "state")); file.Write(" = "); file.Write(joystick.Replace("joystick", "joystickDevice")); file.WriteLine(".GetCurrentState();");
                file.Write("            // joystick "); file.WriteLine(joystick.Replace("joystick", "povs"));
                file.Write("            var "); file.Write(joystick.Replace("joystick", "povs")); file.Write(" = "); file.Write(joystick.Replace("joystick", "state")); file.WriteLine(".PointOfViewControllers;");
                file.WriteLine();
            }
        }

        public void PostFeed(CompileInfo info, System.IO.StreamWriter file)
        {
            ReleaseDevices(info, file);
        }


        public void Apply(DeviceListItem item)
        {
            throw new NotSupportedException();
        }
    }
}
