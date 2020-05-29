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
    public partial class MapItemFakePOV : MapItemBase, IMapItem
    {
        public static string TagName = "FakePOV";
        public static string DisplayText = "Fake POV";

        public MapItemFakePOV()
        {
            InitializeComponent();
        }

        public string FromJoystick { get { return dropDownJoystick.SelectedKey; } set { dropDownJoystick.SelectedKey = value; } }
        public bool IsXDirection { get { return rbAxisX.Checked; } set { rbAxisX.Checked = value; } }
        public string VJoyPOV { get { return dropDownVJoy.SelectedKey; } set { dropDownVJoy.SelectedKey = value; } }

        public void ToXml(System.Xml.XmlNode parentNode)
        {
            var node = parentNode.AddElement(TagName);
            node.SetAttribute("joystick", dropDownJoystick.SelectedKey);
            node.SetAttribute("direction", IsXDirection ? "x" : "y");
            node.SetAttribute("vjoy", dropDownVJoy.SelectedKey);
        }

        public void FromXml(System.Xml.XmlNode node)
        {
            dropDownJoystick.SelectedKey = node.GetAttribute("joystick");
            IsXDirection = node.GetAttribute("direction") != "y";
            dropDownVJoy.SelectedKey = node.GetAttribute("vjoy");
        }
    }
}
