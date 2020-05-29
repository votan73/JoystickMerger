using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoystickMerger
{
    static class ButtonMapper
    {
        static ButtonMapper()
        {
            for (int i = 0; i < 32; i++)
                bitfield[i] = (uint)(1 << i);
        }
        private static readonly uint[] bitfield = new uint[32];

        public static uint From(IList<bool> buttons, int count)
        {
            count = Math.Min(count, 32);
            uint result = 0;
            for (int i = 0; i < count; i++)
                if (buttons[i])
                    result |= bitfield[i];
            return result;
        }
        public static uint From(IList<bool> buttons, int count, int bitOffset)
        {
            uint result = 0;
            for (int i = 0; i < count; i++)
                if (buttons[i])
                    result |= bitfield[i + bitOffset];
            return result;
        }
        public static uint From(IList<bool> buttons, int start, int count, int bitOffset)
        {
            count = Math.Min(count + start, 32);
            bitOffset -= start;
            uint result = 0;
            for (int i = start; i < count; i++)
                if (buttons[i])
                    result |= bitfield[i + bitOffset];
            return result;
        }
    }
}
