using SharpDX.DirectInput;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JoystickMerger.Generator
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            MainForm.BoldFont = new Font(Font, FontStyle.Bold);
            MainForm.BiggerFont = new Font(Font.FontFamily, Font.SizeInPoints * 1.75f);
        }
        protected override void OnLoad(EventArgs e)
        {
            input = new DirectInput();

            saveXmlFileDialog.SupportMultiDottedExtensions = openXmlFileDialog.SupportMultiDottedExtensions = true;
            saveXmlFileDialog.DefaultExt = openXmlFileDialog.DefaultExt = "joystickmerger.xml";
            saveXmlFileDialog.Filter = openXmlFileDialog.Filter = "Joystick Merger|*.joystickmerger.xml";
            //openFileDialog1.Filter += ";Joystick Merger Executable|*.exe";

            base.OnLoad(e);
        }
        protected async override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            var list = new List<DeviceItem>();
            await System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                var gameControllerList = input.GetDevices(DeviceClass.GameControl, DeviceEnumerationFlags.AttachedOnly);
                foreach (var deviceInstance in gameControllerList)
                {
                    if (!deviceInstance.InstanceName.StartsWith("vJoy", StringComparison.OrdinalIgnoreCase))
                        list.Add(new DeviceItem(deviceInstance, deviceInstance.InstanceName));
                }
                list.Sort((a, b) => (a.Name.CompareTo(b.Name) << 1) + a.Device.InstanceGuid.CompareTo(b.Device.InstanceGuid));

                var unique = new HashSet<string>();
                for (int i = 0; i < list.Count; i++)
                {
                    if (!unique.Add(list[i].Name))
                    {
                        var prefix = list[i].Name;
                        var num = 1;
                        for (int t = 0; t < list.Count; t++)
                        {
                            if (list[t].Name == prefix)
                                list[t].Name = String.Concat(prefix, " ", num++);
                        }
                    }
                }
            });
            if (list.Count > 0)
            {
                foreach (var item in list)
                    deviceList1.Controls.Add(new DeviceListItem() { Item = item });
            }
            else
                deviceList1.Controls.Add(new Label() { Text = "No Controller found.", Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right, });
            UpdateDeviceDropDown(null, null);

            mapLevel1.SuspendLayout();
            try
            {
                int index = 0;
                var joysticks = DataSources.JoystickAxis;
                foreach (var axis in DataSources.VJoyAxis)
                {
                    var item = new MapItemAxis();
                    mapLevel1.Controls.Add(item);
                    item.JoystickAxis = joysticks.Count > index ? joysticks[index++].Key : null;
                    item.VJoyAxis = axis.Key;
                }
                index = 0;
                var joysickPOVs = DataSources.JoystickPOV;
                foreach (var pov in DataSources.VJoyPOVs)
                {
                    var item = new MapItemPOV();
                    mapLevel1.Controls.Add(item);
                    item.JoystickPOV = joysickPOVs.Count > index ? joysickPOVs[index].Key : null;
                    index += 2;
                    item.VJoyPOV = pov.Key;
                }
                int numButtonSections = Math.Max(1, deviceList1.Controls.Count);
                int rangePerSection = 32 / numButtonSections;
                var devices = new List<DeviceListItem>(deviceList1.Controls.OfType<DeviceListItem>());
                for (int i = 0; i < numButtonSections; i++)
                {
                    var item = new MapItemButtons();
                    mapLevel1.Controls.Add(item);
                    item.Joystick = i < devices.Count ? (devices[i] as DeviceListItem).Item.Name : "";
                    item.From = 1;
                    item.Range = rangePerSection;
                    item.MapTo = i * rangePerSection + 1;
                }
            }
            finally
            {
                mapLevel1.ResumeLayout(true);
            }
            mapLevel1.Focus();
        }

        public static Font BoldFont;
        public static Font BiggerFont;
        DirectInput input;

        private void UpdateDeviceDropDown(object sender, EventArgs e)
        {
            DataSources.CreateDataSources(deviceList1.Controls.OfType<DeviceListItem>().Select<DeviceListItem, DeviceItem>(x => x.Item));
        }

        private void mapLevel1_Resize(object sender, EventArgs e)
        {
            var screen = Screen.FromControl(this);

            this.Height = Math.Min(screen.WorkingArea.Height - 32 - this.Top, (Height - ClientSize.Height) + RootLevel.Top + mapLevel1.Height);
        }

        private void BtnLoad_Click(object sender, EventArgs e)
        {
            if (openXmlFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var xml = new System.Xml.XmlDocument();
                xml.Load(openXmlFileDialog.FileName);
                RealXml(xml);
                saveXmlFileDialog.FileName = Path.GetFileName(openXmlFileDialog.FileName);
                saveXmlFileDialog.InitialDirectory = Path.GetDirectoryName(openXmlFileDialog.FileName);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (saveXmlFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var xml = WriteXml();
                xml.Save(saveXmlFileDialog.FileName);
            }
        }

        private void RealXml(System.Xml.XmlDocument xml)
        {
            var root = xml.DocumentElement;
            if (root.Name != "JoystickMerger" || root.GetAttribute("version") != "1")
            {
                MessageBox.Show("Unknown XML", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            deviceList1.FromXml(root.SelectSingleNode("Joysticks"));
            mapLevel1.FromXml(root.SelectSingleNode("Mapping"));
        }

        private System.Xml.XmlDocument WriteXml()
        {
            var xml = new System.Xml.XmlDocument();
            var root = xml.AddElement("JoystickMerger");
            root.SetAttribute("version", "1");
            var joysticks = root.AddElement("Joysticks");
            deviceList1.ToXml(joysticks);
            var rootLevel = root.AddElement("Mapping");
            mapLevel1.ToXml(rootLevel);
            return xml;
        }

        private void BtnGenerate_Click(object sender, EventArgs e)
        {
            if (saveExeFileDialog.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;

            using (var generator = new Generator())
            {
                try
                {
                    if (generator.Build(saveExeFileDialog.FileName, deviceList1, mapLevel1))
                        MessageBox.Show("Build successfully completed.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        MessageBox.Show("Build failed.\n" + generator.Output, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            var highest = DateTime.MinValue;
            DeviceListItem highestItem = null;
            foreach (var item in deviceList1.Items)
            {
                if (item.LastChangeDectection > highest)
                {
                    highest = item.LastChangeDectection;
                    highestItem = item;
                }
            }
            if (highestItem != null)
                label3.Text = String.Concat(highestItem.Item.Name, " ", highestItem.ChangeType, " ", highestItem.ChangeValue);
            else
                label3.Text = "-";
        }
    }
}
