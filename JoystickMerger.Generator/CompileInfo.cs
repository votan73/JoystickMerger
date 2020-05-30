using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoystickMerger.Generator
{
    class CompileInfo
    {
        readonly HashSet<string> joysticks = new HashSet<string>();
        public void RegisterJoystick(string joystick)
        {
            if (String.IsNullOrEmpty(joystick))
                return;

            if (joystick.Contains('.'))
                joysticks.Add(joystick.Substring(0, joystick.IndexOf('.')));
            else
                joysticks.Add(joystick);
        }

        readonly HashSet<string> vJoyAxis = new HashSet<string>();
        public void RegisterVJoyAxis(string axis)
        {
            vJoyAxis.Add(axis);
        }
        public IEnumerable<string> GetVJoyAxis()
        {
            var list = new List<string>(DataSources.VJoyAxis.Select<JoyKeyValue, string>(x => x.Key));
            return list.Intersect<string>(vJoyAxis);
        }

        int maxPOVs;
        public void RegisterPOV(string vJoyPOV)
        {
            for (int index = 0; index < DataSources.VJoyPOVs.Length; index++)
                if (DataSources.VJoyPOVs[index].Key == vJoyPOV)
                {
                    maxPOVs = Math.Max(maxPOVs, index + 1);
                    break;
                }
        }
        public int GetMaxPOVs()
        {
            return maxPOVs;
        }

        int maxButtons;
        public void RegisterVJoyButton(int index)
        {
            maxButtons = Math.Max(maxButtons, index);
        }
        public int GetMaxButtons()
        {
            return maxButtons;
        }

        readonly Dictionary<Type, int> registeredIndex = new Dictionary<Type, int>();
        public int RegisterIndex(MapItemBase item)
        {
            if (item == null)
                throw new ArgumentNullException("item");
            int index;
            registeredIndex.TryGetValue(item.GetType(), out index);
            index++;
            registeredIndex[item.GetType()] = index;
            return index;
        }

        public readonly List<string> Joysticks = new List<string>();
        public void EndInitialization()
        {
            IndentLevel = 3;
            Joysticks.Clear();
            Joysticks.AddRange(joysticks);
            Joysticks.Sort();
        }

        public int IndentLevel;
    }
}
