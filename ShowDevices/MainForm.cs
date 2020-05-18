using SharpDX.DirectInput;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace xJoyMerger.ShowDevices
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }
        protected override void OnLoad(EventArgs e)
        {
            input = new DirectInput();

            base.OnLoad(e);
        }
        protected async override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            textBox1.Text = "Loading...";
            var sb = new StringBuilder();
            await System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                var gameControllerList = input.GetDevices(DeviceClass.GameControl, DeviceEnumerationFlags.AttachedOnly);
                foreach (var deviceInstance in gameControllerList)
                {
                    sb.Append("Name: ");
                    sb.Append(deviceInstance.InstanceName);
                    sb.Append(" ID: ");
                    sb.Append(deviceInstance.InstanceGuid);
                    sb.AppendLine();
                }
            });
            textBox1.Text = sb.ToString();
        }
        DirectInput input;

    }
}
