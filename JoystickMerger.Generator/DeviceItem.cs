using SharpDX.DirectInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoystickMerger.Generator
{
    class DeviceItem
    {
        public DeviceItem(DeviceInstance device, string name)
        {
            this.Device = device;
            this.Name = name;
        }
        public DeviceInstance Device;
        public string Key;

        public string Name;
        public override string ToString()
        {
            return Name;
        }
    }
}
