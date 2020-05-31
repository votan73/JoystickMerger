﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JoystickMerger.Generator
{
    class DropDownVJoySliders : JoyKeyValueDropDown
    {
        public DropDownVJoySliders()
        {
            base.DataList = new BindingList<JoyKeyValue>(DataSources.VJoySliders);
        }
    }
}
