using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JoystickMerger.Generator
{
    partial class DeviceListItem : UserControl
    {
        public DeviceListItem()
        {
            InitializeComponent();
            DeadZone = 8.4f;
        }
        protected override void OnLoad(EventArgs e)
        {
            if (!DesignMode)
            {
                base.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
                Text = Item.Name;
            }
            base.OnLoad(e);
        }

        public override string Text { get { return checkBox1.Text; } set { checkBox1.Text = value; } }

        public DeviceItem Item;
        public override string ToString()
        {
            return String.Concat(Item.Name, " (", Item.Device.InstanceGuid, ")");
        }

        private void SliderDeadzone_ValueChanged(object sender, EventArgs e)
        {
            label2.Text = (SliderDeadzone.Value / 1000f).ToString("p01");
        }

        public bool Checked { get { return checkBox1.Checked; } set { checkBox1.Checked = value; } }
        public float DeadZone { get { return SliderDeadzone.Value / 10f; } set { SliderDeadzone.Value = Math.Min(500, Math.Max(0, Convert.ToInt32(value * 10f))); } }
    }
}
