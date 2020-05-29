using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX;
using SharpDX.DirectInput;
using System.Windows.Forms;
using vJoyInterfaceWrap;

namespace JoystickMerger.Feeder
{
    public partial class GameDevPoller
    {
        MainForm mainForm;

        vJoy joystick;
        vJoy.JoystickState iReport;
        bool vjoyEnabled;
        double axisScale;
        DirectInput input;
        readonly static System.Speech.Synthesis.SpeechSynthesizer speech = new System.Speech.Synthesis.SpeechSynthesizer() { Volume = 70, Rate = 1 };
        private static void Say(System.Globalization.CultureInfo culture, string text)
        {
            var prompt = new System.Speech.Synthesis.PromptBuilder(culture);
            prompt.StartSentence();
            prompt.AppendText(text);
            prompt.EndSentence();
            speech.SpeakAsync(prompt);
        }
        private static void Say(string text)
        {
            if (speech.State == System.Speech.Synthesis.SynthesizerState.Speaking)
                speech.SpeakAsyncCancelAll();
            var prompt = new System.Speech.Synthesis.PromptBuilder();
            prompt.StartSentence();
            prompt.AppendText(text);
            prompt.EndSentence();
            speech.SpeakAsync(prompt);
        }

        public void Init(MainForm parent)
        {
            mainForm = parent;
            mainForm.SetDeviceNames(deviceNames);

            ClearDevices();
            // Create one joystick object and a position structure.
            joystick = new vJoy();
            vjoyEnabled = false;

            // Get the driver attributes (Vendor ID, Product ID, Version Number)
            if (!joystick.vJoyEnabled())
            {
                mainForm.ReportError("vJoy driver not enabled: Failed Getting vJoy attributes.\n");
                return;
            }

            input = new DirectInput();
            iReport = new vJoy.JoystickState();

            // Get the state of the requested device
            VjdStat status = joystick.GetVJDStatus(id);
            switch (status)
            {
                case VjdStat.VJD_STAT_OWN:
                case VjdStat.VJD_STAT_FREE:
                    break;
                case VjdStat.VJD_STAT_BUSY:
                    mainForm.ReportError("vJoy Device is already owned by another feeder. Cannot continue");
                    return;
                case VjdStat.VJD_STAT_MISS:
                    mainForm.ReportError("vJoy Device is not installed or disabled. Cannot continue");
                    return;
                default:
                    mainForm.ReportError("vJoy Device general error. Cannot continue");
                    return;
            };

            if (!ValidateVJoyConfiguration())
                return;

            if ((status == VjdStat.VJD_STAT_OWN) || ((status == VjdStat.VJD_STAT_FREE) && (!joystick.AcquireVJD(id))))
            {
                mainForm.ReportError("Failed to acquire vJoy device number");
                return;
            }

            long maxval = 0;
            joystick.GetVJDAxisMax(id, HID_USAGES.HID_USAGE_X, ref maxval);
            axisScale = (double)maxval / 65536.0;
            mainForm.ReportVJoyVersion(joystick.GetvJoySerialNumberString());
            vjoyEnabled = true;
        }

        void ClearDevices()
        {
            ReleaseDevices();
            var guid = Guid.Empty;
            for (int i = 0; i < numDevices; i++)
                mainForm.SetDevicesState(i, false, ref guid);
        }

        public bool ValidateDeviceExistance()
        {
            // Find all the GameControl devices that are attached.

            var gameControllerList = input.GetDevices(DeviceClass.GameControl, DeviceEnumerationFlags.AttachedOnly);

            // check for devices existance
            var guids = new IdentifierList(Properties.Settings.Default.PreferredJoyGuids);
            for (int i = 0; i < guids.Count; i++)
                for (int t = i + 1; t < guids.Count; t++)
                    if (guids[t] == guids[i])
                        guids[t] = Guid.Empty;
            while (guids.Count > numDevices) guids.RemoveAt(guids.Count - 1);
            while (guids.Count < numDevices) guids.Add(Guid.Empty);

            FindDevices(gameControllerList, guids);

            var newPreferred = guids.ToString();
            if (Properties.Settings.Default.PreferredJoyGuids != newPreferred)
            {
                Properties.Settings.Default.PreferredJoyGuids = newPreferred;
                Properties.Settings.Default.Save();
            }
            return AllDevicesReady() && vjoyEnabled;
        }

        bool DetectDevice(DeviceInstance deviceInstance, ref Joystick dev, string name, ref Guid preferGuid, Action<Guid> feedback)
        {
            if (deviceInstance.ProductName.StartsWith(name, StringComparison.OrdinalIgnoreCase) &&
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
                feedback(deviceInstance.InstanceGuid);
                return true;
            }
            return false;
        }

        public bool Poll()
        {
            if (!AllDevicesReady())
                return false;
            if (!vjoyEnabled)
            {
                vjoyEnabled = joystick.AcquireVJD(id);
                if (!vjoyEnabled) return false;
            }

            try
            {
                iReport.bDevice = (byte)id;
                iReport.Buttons = 0;
                iReport.bHats = iReport.bHatsEx1 = iReport.bHatsEx2 = iReport.bHatsEx3 = uint.MaxValue;

                Feed();

                /*** Feed the driver with the position packet - is fails then wait for input then try to re-acquire device ***/
                if (!joystick.UpdateVJD(id, ref iReport))
                {
                    vjoyEnabled = false;
                    mainForm.ReportVJoyDisconnect();
                }
            }
            catch (System.Exception)
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

        private uint FakePOV_X(uint prev, int rot)
        {
            unchecked
            {
                if (prev == uint.MaxValue)
                {
                    if (rot < 20000)
                        return 27000;
                    if (rot > 45536)
                        return 9000;

                    return uint.MaxValue;
                }
                else
                {
                    if (rot < 20000)
                        return (uint)(prev < 18000 ? 27000 + 4500 : 27000 - 4500);
                    if (rot > 45536)
                        return (uint)(prev < 18000 ? 9000 - 4500 : 9000 + 4500);
                    return prev;
                }
            }
        }

    }
}
