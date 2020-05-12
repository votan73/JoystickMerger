using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX;
using SharpDX.DirectInput;
using System.Windows.Forms;
using vJoyInterfaceWrap;


namespace JoystickMerger.DualT16000M
{
    public class GameDevPoller
    {
        Joystick joystickDevice1;
        Joystick joystickDevice2;

        MainForm mainForm;

        vJoy joystick;
        vJoy.JoystickState iReport;
        bool vjoyEnabled;
        uint id = 1;
        double axisScale;
        DirectInput input;

        public void Init(MainForm parent)
        {
            mainForm = parent;
            ClearDevices();
            // Create one joystick object and a position structure.
            input = new DirectInput();
            joystick = new vJoy();
            iReport = new vJoy.JoystickState();
            vjoyEnabled = false;


            // Get the driver attributes (Vendor ID, Product ID, Version Number)
            if (!joystick.vJoyEnabled())
            {
                MessageBox.Show("vJoy driver not enabled: Failed Getting vJoy attributes.\n", "Error");
                return;
            }

            // Get the state of the requested device
            VjdStat status = joystick.GetVJDStatus(id);
            switch (status)
            {
                case VjdStat.VJD_STAT_OWN:
                case VjdStat.VJD_STAT_FREE:
                    break;
                case VjdStat.VJD_STAT_BUSY:
                    MessageBox.Show("vJoy Device is already owned by another feeder. Cannot continue", "Error");
                    return;
                case VjdStat.VJD_STAT_MISS:
                    MessageBox.Show("vJoy Device is not installed or disabled. Cannot continue", "Error");
                    return;
                default:
                    MessageBox.Show("vJoy Device general error. Cannot continue", "Error");
                    return;
            };

            // Make sure all needed axes and buttons are supported
            bool AxisX = joystick.GetVJDAxisExist(id, HID_USAGES.HID_USAGE_X);
            bool AxisY = joystick.GetVJDAxisExist(id, HID_USAGES.HID_USAGE_Y);
            bool AxisRX = joystick.GetVJDAxisExist(id, HID_USAGES.HID_USAGE_RX);
            bool AxisRZ = joystick.GetVJDAxisExist(id, HID_USAGES.HID_USAGE_RZ);
            int nButtons = joystick.GetVJDButtonNumber(id);
            int cont = joystick.GetVJDContPovNumber(id);
            if (!AxisX || !AxisY || !AxisRX || !AxisRZ || nButtons < 32 || cont < 3)
            {
                MessageBox.Show("vJoy Device is not configured correctly. Must have X,Y,Rx,Ry analog axis, 12 buttons and 2 Analog POVs. Cannot continue\n", "Error");
                return;
            }

            if ((status == VjdStat.VJD_STAT_OWN) || ((status == VjdStat.VJD_STAT_FREE) && (!joystick.AcquireVJD(id))))
            {
                MessageBox.Show("Failed to acquire vJoy device number", "Error");
                return;
            }

            long maxval = 0;
            joystick.GetVJDAxisMax(id, HID_USAGES.HID_USAGE_X, ref maxval);
            axisScale = (double)maxval / 65536.0;
            mainForm.LblVjoyStat.Text = "Found. Ver: " + joystick.GetvJoySerialNumberString();
            vjoyEnabled = true;
        }

        void ClearDevices()
        {
            if (joystickDevice1 != null && !joystickDevice1.IsDisposed)
                joystickDevice1.Dispose();
            if (joystickDevice2 != null && !joystickDevice2.IsDisposed)
                joystickDevice2.Dispose();
            mainForm.LblJoystick2Stat.Text = "Waiting...";
            mainForm.LblJoystick1Stat.Text = "Waiting...";
        }

        public bool ValidateDeviceExistance()
        {
            // Find all the GameControl devices that are attached.

            var gameControllerList = input.GetDevices(DeviceClass.GameControl, DeviceEnumerationFlags.AttachedOnly);

            // check for devices existance
            var preferJoy1 = Properties.Settings.Default.PreferredJoy1Guid;
            var preferJoy2 = Properties.Settings.Default.PreferredJoy2Guid;
            if (preferJoy1 == preferJoy2)
                preferJoy2 = Guid.Empty;
            var newJoy1 = Guid.Empty;
            var newJoy2 = Guid.Empty;
            foreach (var deviceInstance in gameControllerList)
            {
                // Move to the first device
                //if (DetectDevice(deviceInstance, "vjoy", ref vjoyDevice, mainForm.LblVjoyStat))
                //continue;

                //T.1600M
                if (DetectDevice(deviceInstance, ref joystickDevice1, ref preferJoy1, mainForm.LblJoystick1Stat))
                    continue;
                if (DetectDevice(deviceInstance, ref joystickDevice2, ref preferJoy2, mainForm.LblJoystick2Stat))
                    continue;

                if (DetectDevice(deviceInstance, ref joystickDevice1, ref newJoy1, mainForm.LblJoystick1Stat))
                {
                    preferJoy1 = newJoy1;
                    continue;
                }
                if (DetectDevice(deviceInstance, ref joystickDevice2, ref newJoy2, mainForm.LblJoystick2Stat))
                {
                    preferJoy2 = newJoy2;
                    continue;
                }
            }
            bool changed = false;
            if (Properties.Settings.Default.PreferredJoy1Guid != preferJoy1)
            {
                Properties.Settings.Default.PreferredJoy1Guid = preferJoy1;
                changed = true;
            }
            if (Properties.Settings.Default.PreferredJoy2Guid != preferJoy2)
            {
                Properties.Settings.Default.PreferredJoy2Guid = preferJoy2;
                changed = true;
            }
            if (changed)
                Properties.Settings.Default.Save();
            return joystickDevice1 != null && joystickDevice2 != null && vjoyEnabled;
        }

        bool DetectDevice(DeviceInstance deviceInstance, ref Joystick dev, ref Guid preferGuid, Label lbl)
        {
            if (deviceInstance.ProductName.StartsWith("T.16000M", StringComparison.OrdinalIgnoreCase) &&
                (preferGuid == Guid.Empty || deviceInstance.InstanceGuid == preferGuid))
            {
                preferGuid = deviceInstance.InstanceGuid;

                // create a device from this controller.
                dev = new Joystick(input, deviceInstance.InstanceGuid);
                dev.SetCooperativeLevel(mainForm.Handle, CooperativeLevel.Background | CooperativeLevel.NonExclusive);
                // Tell DirectX that this is a Joystick.
                //dev.SetDataFormat(DeviceDataFormat.Joystick);
                // Finally, acquire the device.
                dev.Acquire();
                lbl.Text = String.Concat("Found", " ", deviceInstance.InstanceGuid);
                return true;
            }
            return false;
        }
        public bool Poll()
        {
            JoystickState state;
            if (joystickDevice1 == null || joystickDevice2 == null)
                return false;
            if (!vjoyEnabled)
            {
                vjoyEnabled = joystick.AcquireVJD(id);
                if (!vjoyEnabled) return false;
            }

            try
            {
                int[] povs;

                // poll the joystick
                joystickDevice2.Poll();
                // update the joystick state field
                state = joystickDevice2.GetCurrentState();
                iReport.bDevice = (byte)id;
                iReport.AxisXRot = (int)(DeadZone((65536 - state.Y), 2768) * axisScale);
                iReport.AxisZ = (int)(DeadZone((double)state.X, 2768) * axisScale);
                iReport.AxisYRot = (int)(DeadZone(state.RotationZ, 2768) * axisScale);


                // Set throttle buttons one by one
                var buttons = state.Buttons;
                iReport.Buttons = 0;

                for (int i = 0; i < buttons.Length; i++)
                    if (buttons[i])
                        iReport.Buttons |= (uint)1 << (i + 16);

                // joystick povs
                povs = state.PointOfViewControllers;
                iReport.bHatsEx2 = (uint)povs[0]; // Neutral state

                //mainForm.LblJoystick2Stat.Text = String.Join("", String.Join(" ", state.RotationZ, iReport.AxisYRot));

                joystickDevice1.Poll();
                // update the joystick state field
                state = joystickDevice1.GetCurrentState();
                iReport.AxisX = (int)(DeadZone(state.X, 2768) * axisScale);
                iReport.AxisY = (int)(DeadZone((65536 - state.Y), 2768) * axisScale);
                //iReport.AxisZRot = (int)(state.RotationZ * axisScale);
                iReport.bHatsEx1 = FakePOV(state);

                // Set joystick buttons one by one
                buttons = state.Buttons;
                for (int i = 0; i < buttons.Length; i++)
                    if (buttons[i])
                        iReport.Buttons |= (uint)1 << i;

                // joystick povs
                povs = state.PointOfViewControllers;
                iReport.bHats = (uint)povs[0]; // Neutral state

                //mainForm.LblJoystick1Stat.Text = String.Join("", String.Join(" ", state.RotationZ));

                /*** Feed the driver with the position packet - is fails then wait for input then try to re-acquire device ***/
                if (!joystick.UpdateVJD(id, ref iReport))
                {
                    vjoyEnabled = false;
                    mainForm.ReportVJoyDisconnect();
                }
            }
            catch (Exception)
            {
                ClearDevices();
                return false;
            }
            return true;
        }

        private double DeadZone(double value, double range)
        {
            if (value > (32768 - range) && value < (32768 + range))
                return 32768;
            return value;
        }

        private uint FakePOV(JoystickState state)
        {
            var rot = state.RotationZ;
            if (rot > 30000 && rot < 35536)
                return uint.MaxValue;
            if (rot < 20000)
                return 27000;
            if (rot > 45536)
                return 9000;

            return uint.MaxValue;
        }

        public void SwapJoysticks()
        {
            var help1 = joystickDevice2;
            joystickDevice2 = joystickDevice1;
            joystickDevice1 = help1;
            var help2 = Properties.Settings.Default.PreferredJoy2Guid;
            Properties.Settings.Default.PreferredJoy2Guid = Properties.Settings.Default.PreferredJoy1Guid;
            Properties.Settings.Default.PreferredJoy1Guid = help2;

            mainForm.LblJoystick1Stat.Text = String.Concat("Found", " ", joystickDevice1.Information.InstanceGuid);
            mainForm.LblJoystick2Stat.Text = String.Concat("Found", " ", joystickDevice2.Information.InstanceGuid);
        }
    }
}
