using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JoystickMerger.Generator
{
    class DropDownVJoyAxis : JoyKeyValueDropDown
    {
        public DropDownVJoyAxis()
        {
            base.DataList = new BindingList<JoyKeyValue>(DataSources.VJoyAxis);
        }
    }
}
