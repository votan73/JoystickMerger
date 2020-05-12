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

        public Label LblVjoyStat
        {
            get { return lblVjoyStat; }
        }
        public Label LblJoystick1Stat
        {
            get { return lblJoystickStat; }
        }
        public Label LblJoystick2Stat
        {
            get { return lblThrottleStat; }
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
                        notifyIcon1.ShowBalloonTip(3000, "SuncomControllerMerge", "One of the controllers was disconnected", ToolTipIcon.None);
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
                    notifyIcon1.ShowBalloonTip(3000, "SuncomControllerMerge", "SuncomControllerMerge is back on-line", ToolTipIcon.None);
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
            ShowInTaskbar = true;
            notifyIcon1.Visible = false;
            WindowState = FormWindowState.Normal;
        }

        private void ButtonSwapJoysticks_Click(object sender, EventArgs e)
        {
            gamePoller.SwapJoysticks();
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
    }
}
