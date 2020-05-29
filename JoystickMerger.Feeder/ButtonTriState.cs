using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoystickMerger.Feeder
{
    class ButtonTriState
    {
        DateTime lastOnState;
        bool lastState;
        int buttonState;
        public int Check(bool state)
        {
            if (lastState != state)
            {
                if (!lastState)
                    lastOnState = DateTime.Now;
                else if (lastOnState.Subtract(DateTime.Now).TotalMilliseconds >= 70)
                {
                    buttonState = (buttonState + 1) % 3;
                    if (OnStateChanged != null)
                        OnStateChanged(this);
                }
                lastState = state;
            }
            return buttonState;
        }
        public Action<ButtonTriState> OnStateChanged;
    }
}
