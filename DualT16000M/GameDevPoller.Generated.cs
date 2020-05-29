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
    partial class GameDevPoller
    {
        const uint id = 1;
        const int numDevices = 2;

        const int deadzone1 = 2768;
        const int deadzone2 = 2768;

        readonly string[] deviceNames = new string[]{
            "T.16000M 1",
            "T.16000M 2",
        };

        Joystick joystickDevice1;
        Joystick joystickDevice2;
        ButtonToggle buttonToggle1 = new ButtonToggle()
        {
            OnStateChanged = (button) =>
            {
                if (button.State)
                    Say("Fahrzeugsteuerrung");
                else
                    Say("Flugzeugsteuerrung");
            }
        };

        private bool ValidateVJoyConfiguration()
        {
            // Make sure all needed axes and buttons are supported
            bool AxisX = joystick.GetVJDAxisExist(id, HID_USAGES.HID_USAGE_X);
            bool AxisY = joystick.GetVJDAxisExist(id, HID_USAGES.HID_USAGE_Y);
            bool AxisRX = joystick.GetVJDAxisExist(id, HID_USAGES.HID_USAGE_RX);
            bool AxisRZ = joystick.GetVJDAxisExist(id, HID_USAGES.HID_USAGE_RZ);
            int nButtons = joystick.GetVJDButtonNumber(id);
            int cont = joystick.GetVJDContPovNumber(id);
            if (!AxisX || !AxisY || !AxisRX || !AxisRZ || nButtons < 32 || cont < 3)
            {
                mainForm.ReportError("vJoy Device is not configured correctly. Must have X,Y,Rx,Ry analog axis, 32 buttons and 3 Analog POVs. Cannot continue\n");
                return false;
            }
            return true;
        }

        private bool AllDevicesReady()
        {
            return joystickDevice1 != null && joystickDevice2 != null;
        }

        private void FindDevices(IList<DeviceInstance> gameControllerList, IdentifierList preferred)
        {
            var newJoy1 = Guid.Empty;
            var preferJoy1 = preferred[0];
            var newJoy2 = Guid.Empty;
            var preferJoy2 = preferred[1];

            foreach (var deviceInstance in gameControllerList)
            {
                if (DetectDevice(deviceInstance, ref joystickDevice1, "T.16000M", ref preferJoy1, (guid) => mainForm.SetDevicesState(0, true, ref guid)))
                    continue;
                if (DetectDevice(deviceInstance, ref joystickDevice2, "T.16000M", ref preferJoy2, (guid) => mainForm.SetDevicesState(1, true, ref guid)))
                    continue;

                if (DetectDevice(deviceInstance, ref joystickDevice1, "T.16000M", ref newJoy1, (guid) => mainForm.SetDevicesState(0, true, ref guid)))
                {
                    preferJoy1 = newJoy1;
                    continue;
                }
                if (DetectDevice(deviceInstance, ref joystickDevice2, "T.16000M", ref newJoy2, (guid) => mainForm.SetDevicesState(1, true, ref guid)))
                {
                    preferJoy2 = newJoy2;
                    continue;
                }
            }
            preferred[0] = preferJoy1;
            preferred[1] = preferJoy2;
        }

        private void Feed()
        {
            joystickDevice1.Poll();
            // update the joystick state1 field
            var state1 = joystickDevice1.GetCurrentState();
            // joystick povs1
            var povs1 = state1.PointOfViewControllers;

            // poll the joystick
            joystickDevice2.Poll();
            // update the joystick state2 field
            var state2 = joystickDevice2.GetCurrentState();
            // joystick povs2
            var povs2 = state2.PointOfViewControllers;

            //
            //iReport.AxisZRot = (int)(state1.RotationZ * axisScale);
            iReport.bHats = (uint)povs1[0]; // Neutral state1
            iReport.bHatsEx1 = FakePOV_X(iReport.bHatsEx1, state1.RotationZ);

            iReport.Buttons |= ButtonMapper.From(state1.Buttons, 16);

            iReport.AxisXRot = (int)(DeadZone((65536 - state2.Y), deadzone2) * axisScale);
            iReport.AxisYRot = (int)(DeadZone(state2.RotationZ, deadzone2) * axisScale);

            iReport.bHatsEx2 = (uint)povs2[0]; // Neutral state2

            iReport.Buttons |= ButtonMapper.From(state2.Buttons, 16, 16);

            if (buttonToggle1.Check(state1.Buttons[15]))
            {
                iReport.AxisX = (int)(DeadZone(state1.X, deadzone1) * axisScale);
                iReport.AxisZ = (int)(DeadZone((double)state1.X, deadzone1) * axisScale);
                iReport.AxisY = (int)(DeadZone(state1.Y, deadzone1) * axisScale);
            }
            else
            {
                iReport.AxisX = (int)(DeadZone(state1.X, deadzone1) * axisScale);
                iReport.AxisZ = (int)(DeadZone((double)state2.X, deadzone1) * axisScale);
                iReport.AxisY = (int)(DeadZone((65536 - state1.Y), deadzone1) * axisScale);
            }

        }

        private void ReleaseDevices()
        {
            if (joystickDevice1 != null && !joystickDevice1.IsDisposed)
                joystickDevice1.Dispose();
            if (joystickDevice2 != null && !joystickDevice2.IsDisposed)
                joystickDevice2.Dispose();
        }

    }
}
