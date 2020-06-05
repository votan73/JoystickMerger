using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoystickMerger.Generator
{
    class JoyKeyValueDropDown : DataListDropDown
    {
        readonly Dictionary<string, JoyKeyValue> lookup = new Dictionary<string, JoyKeyValue>();

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
                    foreach (JoyKeyValue item in value)
                        lookup[item.Key] = item;
                base.DataList = value;
            }
        }
        protected override void CreateHandle()
        {
            base.CreateHandle();
            SelectedKey = selectedKey;
        }

        private string selectedKey;
        public String SelectedKey
        {
            get { return IsHandleCreated ? (SelectedItem != null ? (SelectedItem as JoyKeyValue).Key : "") : selectedKey; }
            set
            {
                selectedKey = value;
                if (IsHandleCreated)
                {
                    JoyKeyValue selected;
                    if (lookup.TryGetValue(value ?? "", out selected))
                        SelectedItem = selected;
                    else
                        SelectedItem = null;
                }
            }
        }
    }
}
