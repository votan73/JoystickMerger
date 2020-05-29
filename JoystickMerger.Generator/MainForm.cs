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

            saveFileDialog1.SupportMultiDottedExtensions = openFileDialog1.SupportMultiDottedExtensions = true;
            saveFileDialog1.DefaultExt = openFileDialog1.DefaultExt = "joystickmerger.xml";
            saveFileDialog1.Filter = openFileDialog1.Filter = "Joystick Merger|*.joystickmerger.xml";
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
                    deviceList1.Controls.Add(new DeviceListItem() { Item = item, Checked = true });
            }
            else
                deviceList1.Controls.Add(new Label() { Text = "No Controller found.", Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right, });
            checkedListBox1_Validated(null, null);

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

        private void checkedListBox1_Validated(object sender, EventArgs e)
        {
            DataSources.CreateDataSources(deviceList1.Controls.OfType<DeviceListItem>().Where<DeviceListItem>(x => x.Checked).Select<DeviceListItem, DeviceItem>(x => x.Item));
        }

        private void mapLevel1_Resize(object sender, EventArgs e)
        {
            var screen = Screen.FromControl(this);

            this.Height = Math.Min(screen.WorkingArea.Height - 32 - this.Top, (Height - ClientSize.Height) + RootLevel.Top + mapLevel1.Height);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var xml = new System.Xml.XmlDocument();
                var root = xml.AddElement("JoystickMerger");
                root.SetAttribute("version", "1");
                var joysticks = root.AddElement("Joysticks");
                deviceList1.ToXml(joysticks);
                var rootLevel = root.AddElement("Mapping");
                mapLevel1.ToXml(rootLevel);
                xml.Save(saveFileDialog1.FileName);
            }
        }

        private void BtnLoad_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var xml = new System.Xml.XmlDocument();
                xml.Load(openFileDialog1.FileName);
                RealXml(xml);
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

        private void BtnGenerate_Click(object sender, EventArgs e)
        {
            var location = System.IO.Path.GetDirectoryName(typeof(MainForm).Assembly.Location);
            var parent = Path.GetDirectoryName(location);
            var commonPath = Path.Combine(parent, "Common");
            var playerPath = Path.Combine(parent, "JoystickMerger.Feeder");
            var packagesPath = Path.Combine(parent, "packages");

            if (!Directory.Exists(commonPath))
            {
                MessageBox.Show(String.Concat("Path \"", commonPath, "\" missing."), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!Directory.Exists(playerPath))
            {
                MessageBox.Show(String.Concat("Path \"", playerPath, "\" missing."), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!Directory.Exists(packagesPath))
            {
                MessageBox.Show(String.Concat("Path \"", packagesPath, "\" missing."), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (var temp = new System.CodeDom.Compiler.TempFileCollection(System.IO.Path.Combine(location, "Build"), true))
            {
                CopyFolder(commonPath, temp.TempDir);
                CopyFolder(playerPath, temp.TempDir);
                CopyFolder(packagesPath, temp.TempDir);
                var process = new System.Diagnostics.Process();
                process.StartInfo.FileName = Path.Combine(Path.GetDirectoryName(typeof(GC).Assembly.Location), "MSBuild.exe");
                process.StartInfo.WorkingDirectory = Path.Combine(temp.TempDir, "JoystickMerger.Feeder");
                process.StartInfo.Arguments = "JoystickMerger.Feeder.csproj /p:\"Configuration=Release\" /p:\"Platform=x64\"";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.Start();

                var output = process.StandardOutput.ReadToEnd();
                var err = process.StandardError.ReadToEnd();
                process.WaitForExit();

                if (process.ExitCode == 0)
                    MessageBox.Show("Build successfully completed.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show("Build failed.", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);

                //%windir%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe DualT16000M.csproj /p:"Configuration=Release" /p:"Platform=x64"
            }
        }
        private void CopyFolder(string SourcePath, string DestinationPath)
        {
            if (SourcePath == DestinationPath)
                return;

            int indexSrc = Path.GetDirectoryName(SourcePath).Length + 1;

            foreach (var path in Directory.GetFiles(SourcePath, "*.*", SearchOption.AllDirectories))
            {
                var dest = System.IO.Path.Combine(DestinationPath, path.Substring(indexSrc));
                Directory.CreateDirectory(Path.GetDirectoryName(dest));
                File.Copy(path, dest, true);
            }
        }
    }
}
