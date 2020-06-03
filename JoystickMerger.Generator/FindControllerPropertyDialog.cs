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

namespace JoystickMerger.Generator
{
    partial class FindControllerPropertyDialog : Form
    {
        public FindControllerPropertyDialog()
        {
            InitializeComponent();
            label1.Font = MainForm.BiggerFont;
        }
        protected override void OnLoad(EventArgs e)
        {
            if (!DesignMode)
            {
                button1.Enabled = false;
                timer1.Enabled = true;
            }
            base.OnLoad(e);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            var highestItem = DeviceList.RecentTouchedDevice();
            if (highestItem != null)
                label1.Text = String.Concat(highestItem.Item.Name, " ", highestItem.ChangeType, " ", highestItem.ChangeValue);
            else
            {
                label1.Text = "-";
            }
            button1.Enabled = highestItem != null;
        }

        public DeviceList DeviceList;
    }
}
