using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JoystickMerger.Generator
{
    public partial class MapItemAxis : MapItemBase, IMapItem
    {
        public static string TagName = "Axis";
        public static string DisplayText = "Axis";

        public MapItemAxis()
        {
            InitializeComponent();
        }

        public string JoystickAxis { get { return dropDownJoystick.SelectedKey; } set { dropDownJoystick.SelectedKey = value; } }
        public bool Inverted { get { return CbxInvert.Checked; } set { CbxInvert.Checked = value; } }
        public string VJoyAxis { get { return dropDownVJoy.SelectedKey; } set { dropDownVJoy.SelectedKey = value; } }

        public void ToXml(System.Xml.XmlNode parentNode)
        {
            var node = parentNode.AddElement(TagName);
            node.SetAttribute("joystick", JoystickAxis);
            node.SetAttribute("inverted", Inverted.ToXml());
            node.SetAttribute("vjoy", VJoyAxis);
        }
        
        public void FromXml(System.Xml.XmlNode node)
        {
            JoystickAxis = node.GetAttribute("joystick");
            Inverted = node.GetAttribute("inverted") == "true";
            VJoyAxis = node.GetAttribute("vjoy");
        }
    }
}
