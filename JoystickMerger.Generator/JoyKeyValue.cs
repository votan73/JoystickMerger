using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoystickMerger.Generator
{
    class JoyKeyValue
    {
        public JoyKeyValue(string key, string value)
        {
            this.Key = key;
            this.Value = value;
        }
        public string Key;
        public string Value;

        public override string ToString()
        {
            return Value;
        }
    }
}
