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
    partial class MapItemConditionBase : MapItemBase
    {
        public MapItemConditionBase()
        {
            InitializeComponent();
        }

        private void tableLayoutPanel1_Resize(object sender, EventArgs e)
        {
            this.Height = tableLayoutPanel1.Height;
        }

        public string Joystick
        {
            get { return dropDownJoystick1.SelectedJoystick; }
            set { dropDownJoystick1.SelectedJoystick = value; }
        }

        protected string IfTrueLabelText
        {
            get { return label4.Text; }
            set { label4.Text = value; }
        }

        protected string IfFalseLabelText
        {
            get { return label1.Text; }
            set { label1.Text = value; }
        }

        protected string IfTrueSayText
        {
            get { return textBox2.Text; }
            set { textBox2.Text = value; }
        }

        protected string IfFalseSayText
        {
            get { return textBox1.Text; }
            set { textBox1.Text = value; }
        }

        public int Button { get { return Convert.ToInt32(numericUpDown1.Value); } set { numericUpDown1.Value = Math.Min(32, Math.Max(1, value)); } }
        public MapLevel IfFalseMappings { get { return mapLevel1; } }
        public MapLevel IfTrueMappings { get { return mapLevel2; } }
    }
}
