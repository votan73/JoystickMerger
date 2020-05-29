using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace JoystickMerger.DualT16000M
{
    public partial class MainForm : Form
    {
        System.Threading.Timer pollTimer;
        GameDevPoller gamePoller;
        bool joyReady;

        public MainForm()
        {
            InitializeComponent();
            gamePoller = new GameDevPoller();
            joyReady = false;
            notifyIcon1.Text = Text;
            Icon = Properties.Resources.t16000m;
            SetNotifierState(false);
        }

        private void SetNotifierState(bool on)
        {
            notifyIcon1.Icon = Icon.FromHandle((on ? Properties.Resources.gamepad_on : Properties.Resources.gamepad_off).GetHicon());
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            gamePoller.Init(this);
            pollTimer = new System.Threading.Timer(pollTimer_Tick, null, 0, 500);
        }

        void pollTimer_Tick(object state)
        {
            if (joyReady)
            {
                joyReady = gamePoller.Poll();
                if (!joyReady)
                {
                    SetNotifierState(joyReady);
                    pollTimer.Change(500, 500);
                    if (WindowState == FormWindowState.Minimized)
                        notifyIcon1.ShowBalloonTip(3000, Text, "One of the controllers was disconnected", ToolTipIcon.None);
                }
            }
            else if (gamePoller.ValidateDeviceExistance())
            {
                pollTimer.Change(0, 10);
                joyReady = true;
                SetNotifierState(joyReady);
                if (WindowState != FormWindowState.Minimized)
                    WindowState = FormWindowState.Minimized;
                else
                    notifyIcon1.ShowBalloonTip(3000, Text, "Controllers back on-line", ToolTipIcon.None);
            }
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                ShowInTaskbar = false;
                notifyIcon1.Visible = true;
                notifyIcon1.ShowBalloonTip(1000);
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Clicks > 1 || e.Button.HasFlag(MouseButtons.Right))
            {
                ShowInTaskbar = true;
                notifyIcon1.Visible = false;
                WindowState = FormWindowState.Normal;
            }
        }

        public void SetDeviceNames(IList<string> names)
        {
            int height = this.Height - tableLayoutPanel1.Height;
            tableLayoutPanel1.RowCount = names.Count + 1;
            for (int i = 0; i < names.Count; i++)
            {
                var lblName = new Label() { Name = "lblJoystickName" + i, Text = names[i], AutoSize = true, Padding = new Padding(0, 5, 5, 0) };
                tableLayoutPanel1.Controls.Add(lblName);
                tableLayoutPanel1.SetColumn(lblName, 0);
                tableLayoutPanel1.SetRow(lblName, i + 1);
                toolTip1.SetToolTip(lblName, lblName.Text);
                var lblStat = new Label() { Name = "lblJoystickStat" + i, AutoSize = true, Padding = new Padding(0, 5, 5, 0) };
                tableLayoutPanel1.Controls.Add(lblStat);
                tableLayoutPanel1.SetColumn(lblStat, 1);
                tableLayoutPanel1.SetRow(lblStat, i + 1);
            }
            tableLayoutPanel1.AutoSize = true;
            this.Height = height + tableLayoutPanel1.Height;
        }

        public void ReportVJoyDisconnect()
        {
            if (this.InvokeRequired)
            {
                MethodInvoker call = ReportVJoyDisconnect;
                BeginInvoke(call);
            }
            else
            {
                MessageBox.Show("Feeding vJoy device failed - try to enable device then press OK", "Error");
            }
        }
        public void ReportError(string text)
        {
            if (this.InvokeRequired)
            {
                var txt = text;
                MethodInvoker call = () => ReportError(txt);
                BeginInvoke(call);
            }
            else
                MessageBox.Show(text, "Error");
        }
        public void ReportVJoyVersion(string version)
        {
            lblVjoyStat.Text = "Found. Ver: " + version;
        }
        public void SetDevicesState(int num, bool ready, ref Guid id)
        {
            var lbl = tableLayoutPanel1.GetControlFromPosition(1, num + 1) as Label;
            if (ready)
            {
                lbl.Text = "Found.";
                toolTip1.SetToolTip(lbl, "ID: " + id);
            }
            else
            {
                lbl.Text = "Waiting...";
                toolTip1.SetToolTip(lbl, lbl.Text);
            }
        }
    }
}
