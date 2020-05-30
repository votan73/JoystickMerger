using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JoystickMerger.Generator
{
    class DropDownJoystick : DataListDropDown
    {
        public DropDownJoystick()
        {
            this.DataList = new BindingList<DeviceItem>(DataSources.Joysticks);
        }
        protected override System.Drawing.Size DefaultSize
        {
            get { return new System.Drawing.Size(226, base.DefaultSize.Height); }
        }

        readonly Dictionary<string, DeviceItem> lookup = new Dictionary<string, DeviceItem>();

        protected override System.Collections.IList DataList
        {
            get
            {
                return base.DataList;
            }
            set
            {
                base.DataList = null;
                lookup.Clear();
                if (value != null)
                    foreach (DeviceItem item in value)
                        lookup[item.Key] = item;
                base.DataList = value;
            }
        }

        public String SelectedJoystick
        {
            get { return SelectedItem != null ? (SelectedItem as DeviceItem).Key : ""; }
            set
            {
                DeviceItem selected;
                if (lookup.TryGetValue(value, out selected))
                    SelectedItem = selected;
                else
                    SelectedItem = null;
            }
        }
    }
}
