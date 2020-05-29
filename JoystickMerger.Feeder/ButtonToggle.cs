using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoystickMerger.Feeder
{
    class ButtonToggle
    {
        DateTime lastOnState;
        bool lastState;
        bool buttonState;
        public bool Check(bool state)
        {
            if (lastState != state)
            {
                if (!lastState)
                    lastOnState = DateTime.Now;
                else if (DateTime.Now.Subtract(lastOnState).TotalMilliseconds >= 70)
                {
                    buttonState = !buttonState;
                    if (OnStateChanged != null)
                        OnStateChanged(this);
                }
                lastState = state;
            }
            return buttonState;
        }
        public bool State { get { return buttonState; } }
        public Action<ButtonToggle> OnStateChanged;
    }
}
