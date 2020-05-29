using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoystickMerger.DualT16000M
{
    class HardwareSwitch
    {
        bool lastState;
        public bool Check(bool state)
        {
            if (lastState != state)
            {
                lastState = state;
                if (OnStateChanged != null)
                    OnStateChanged(this);
            }
            return lastState;
        }
        public Action<HardwareSwitch> OnStateChanged;
    }
}
