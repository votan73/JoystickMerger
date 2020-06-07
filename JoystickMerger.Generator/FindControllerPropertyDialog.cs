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
                BtnOk.Enabled = false;
                timer1.Enabled = true;
                pictureBox1.Image = new Bitmap(320, 64, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                Reset();
            }
            base.OnLoad(e);
        }
        DateTime last;
        public int ButtonFrom { get; private set; }
        public int ButtonTo { get; private set; }

        private void timer1_Tick(object sender, EventArgs e)
        {
            var highestItem = DeviceList.RecentTouchedDevice();
            if (highestItem != null)
            {
                if (last != highestItem.LastChangeDectection)
                {
                    last = highestItem.LastChangeDectection;
                    label1.Text = String.Concat(highestItem.Item.Name, " ", highestItem.DetectedType, " ", highestItem.DetectedValue);
                    if (highestItem.DetectedType == DetectionType.Button)
                    {
                        int button = Int32.Parse(highestItem.DetectedValue);
                        ButtonFrom = Math.Min(ButtonFrom, button);
                        ButtonTo = Math.Max(ButtonTo, button);

                        DrawGrid(highestItem.ButtonCount, ButtonFrom, ButtonTo);
                        pictureBox1.Visible = true;
                    }
                    else
                        pictureBox1.Visible = false;
                }
                BtnOk.Focus();
            }
            else
            {
                label1.Text = "-";
                pictureBox1.Visible = false;
            }
            BtnReset.Visible = pictureBox1.Visible;
            BtnOk.Enabled = highestItem != null;
        }

        private void DrawGrid(int numButtons, int min, int max)
        {
            try
            {
                using (var g = Graphics.FromImage(pictureBox1.Image))
                {
                    g.Clear(Color.Transparent);
                    if (numButtons <= 0)
                        return;

                    int rowCount = Convert.ToInt32(Math.Ceiling(64.0 / Math.Sqrt(pictureBox1.Image.Width * pictureBox1.Image.Height / numButtons)));
                    int colCount = Convert.ToInt32(Math.Ceiling(numButtons * 1.0 / rowCount));
                    int d = 64 / rowCount;
                    if (max < min)
                        max = min;
                    min--;
                    max--;
                    int offsetX = (pictureBox1.Image.Width - colCount * d) / 2;
                    for (int i = 0; i < numButtons; i++)
                    {
                        int col = i % colCount;
                        int row = i / colCount;
                        if (i >= min && i <= max)
                            g.FillRectangle(Brushes.SteelBlue, new Rectangle(offsetX + col * d, row * d, d - 1, d - 1));
                        else
                            g.FillRectangle(Brushes.LightGray, new Rectangle(offsetX + col * d, row * d, d - 1, d - 1));
                    }
                }
            }
            finally
            {
                pictureBox1.Refresh();
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DeviceList DeviceList;

        private void Reset()
        {
            ButtonFrom = Int32.MaxValue;
            ButtonTo = 0;
        }
        private void BtnReset_Click(object sender, EventArgs e)
        {
            Reset();
            last = DateTime.MinValue;
        }
    }
}
