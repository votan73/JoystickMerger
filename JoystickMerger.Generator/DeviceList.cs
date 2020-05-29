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
            foreach (var item in this.Controls.OfType<DeviceListItem>().Where<DeviceListItem>(x => x.Checked))
            {
                var joystick = joysticks.AddElement("Joystick");
                joystick.SetAttribute("name", item.Item.Name);
                joystick.SetAttribute("preferred", item.Item.Device.InstanceGuid.ToString());
                joystick.SetAttribute("deadzone", item.DeadZone.ToString(System.Globalization.CultureInfo.GetCultureInfo(9)));
            }
        }

        public void FromXml(System.Xml.XmlNode joysticks)
        {
            Action<DeviceListItem, System.Xml.XmlNode> setDeadzone = (item, joystick) =>
            {
                float value;
                if (Single.TryParse(joystick.GetAttribute("deadzone"), System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.CultureInfo.GetCultureInfo(9), out value))
                    item.DeadZone = value;
            };

            var list = new HashSet<System.Xml.XmlNode>(joysticks.ChildNodes.Cast<System.Xml.XmlNode>().Where<System.Xml.XmlNode>(x => x.Name == "Joystick"));
            var items = new List<DeviceListItem>(this.Controls.OfType<DeviceListItem>());
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

    }
}
