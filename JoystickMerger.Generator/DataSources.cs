using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoystickMerger.Generator
{
    static class DataSources
    {
        static DataSources()
        {
            VJoyAxis = new JoyKeyValue[] {
                new JoyKeyValue("AxisX", "AxisX"),
                new JoyKeyValue("AxisY", "AxisY"),
                new JoyKeyValue("AxisZ", "AxisZ"),
                new JoyKeyValue("AxisXRot", "AxisXRot"),
                new JoyKeyValue("AxisYRot", "AxisYRot"),
                new JoyKeyValue("AxisZRot", "AxisZRot"),
                //new JoyKeyValue("Buttons", "Buttons"),
                new JoyKeyValue("AxisSL0", "Slider"),
                new JoyKeyValue("AxisSL1", "Dial"),
                new JoyKeyValue("AxisWHL", "Wheel"),
            };

            VJoyPOVs = new JoyKeyValue[] {
                new JoyKeyValue("bHats", "POV 1"),
                new JoyKeyValue("bHatsEx1", "POV 2"),
                new JoyKeyValue("bHatsEx2", "POV 3"),
                new JoyKeyValue("bHatsEx3", "POV 4"),
            };

            JoystickPOVTemplate = new JoyKeyValue[] {
                new JoyKeyValue("POV1", "POV 1"),
                new JoyKeyValue("POV2", "POV 2"),
                new JoyKeyValue("POV3", "POV 3"),
                new JoyKeyValue("POV4", "POV 4"), };
            JoystickAxisTemplate = new JoyKeyValue[] {
                new JoyKeyValue("X", "X"),
                new JoyKeyValue("Y", "Y"),
                new JoyKeyValue("Z", "Z"), 
                new JoyKeyValue("RotationX", "RotationX"),
                new JoyKeyValue("RotationY", "RotationY"),
                new JoyKeyValue("RotationZ", "RotationZ"),
                new JoyKeyValue("Slider1", "Slider1"),
                new JoyKeyValue("Slider2", "Slider2"), 
                new JoyKeyValue("TorqueX", "TorqueX"),
                new JoyKeyValue("TorqueY", "TorqueY"),
                new JoyKeyValue("TorqueZ", "TorqueZ"),
                new JoyKeyValue("VelocitySlider1", "VelocitySlider1"),
                new JoyKeyValue("VelocitySlider2", "VelocitySlider2"),
                new JoyKeyValue("VelocityX", "VelocityX"),
                new JoyKeyValue("VelocityY", "VelocityY"),
                new JoyKeyValue("VelocityZ", "VelocityZ"),
            };
        }
        private static JoyKeyValue[] JoystickPOVTemplate;
        private static JoyKeyValue[] JoystickAxisTemplate;

        public static JoyKeyValue[] VJoyAxis;
        public static JoyKeyValue[] VJoyPOVs;
        public static readonly BindingList<JoyKeyValue> JoystickPOV = new BindingList<JoyKeyValue>();
        public static readonly BindingList<JoyKeyValue> JoystickAxis = new BindingList<JoyKeyValue>();
        public static readonly BindingList<DeviceItem> Joysticks = new BindingList<DeviceItem>();

        public static void CreateDataSources(IEnumerable<DeviceItem> items)
        {
            int count = 1;
            var listPOV = new List<JoyKeyValue>();
            var listAxis = new List<JoyKeyValue>();
            Joysticks.Clear();
            foreach (var item in items)
            {
                item.Key = "joystick" + count;
                Joysticks.Add(item);

                foreach (var prop in JoystickPOVTemplate)
                    listPOV.Add(new JoyKeyValue(String.Concat("joystick", count, ".", prop.Key), String.Concat(prop.Value, " - ", item.Name)));
                foreach (var prop in JoystickAxisTemplate)
                    listAxis.Add(new JoyKeyValue(String.Concat("joystick", count, ".", prop.Key), String.Concat(prop.Value, " - ", item.Name)));
                count++;
            }
            JoystickPOV.Clear();
            listPOV.ForEach(x => JoystickPOV.Add(x));

            JoystickAxis.Clear();
            listAxis.ForEach(x => JoystickAxis.Add(x));
        }
    }
}
