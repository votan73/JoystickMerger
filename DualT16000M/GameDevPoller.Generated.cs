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
        uint id = 1;

        Joystick joystickDevice1;
        Joystick joystickDevice2;

        private void Feed()
        {
            JoystickState state;
            int[] povs;

            // poll the joystick
            joystickDevice2.Poll();
            // update the joystick state field
            state = joystickDevice2.GetCurrentState();
            iReport.AxisXRot = (int)(DeadZone((65536 - state.Y), 2768) * axisScale);
            iReport.AxisZ = (int)(DeadZone((double)state.X, 2768) * axisScale);
            iReport.AxisYRot = (int)(DeadZone(state.RotationZ, 2768) * axisScale);


            iReport.Buttons = ButtonMapper.From(state.Buttons, 16, 16);
            // Set joystick buttons one by one
            //for (int i = 0; i < 16; i++)
            //    if (buttons[i])
            //        iReport.Buttons |= (uint)1 << (i + 16);

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
            iReport.bHatsEx1 = FakePOV_X(state.RotationZ);

            iReport.Buttons |= ButtonMapper.From(state.Buttons, 16);
            // Set joystick buttons one by one
            //buttons = state.Buttons;
            //for (int i = 0; i < 16; i++)
            //    if (buttons[i])
            //        iReport.Buttons |= (uint)1 << i;

            // joystick povs
            povs = state.PointOfViewControllers;
            iReport.bHats = (uint)povs[0]; // Neutral state

            //mainForm.LblJoystick1Stat.Text = String.Join("", String.Join(" ", state.RotationZ));
        }
    }
}
