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
    [DetectionType(DetectionType.Axis)]
    partial class MapItemFakePOV : MapItemBase, IMapItem
    {
        public static string TagName = "FakePOV";
        public static string DisplayText = "Fake POV";

        public MapItemFakePOV()
        {
            InitializeComponent();
        }

        public string JoystickAxis { get { return dropDownJoystick.SelectedKey; } set { dropDownJoystick.SelectedKey = value; } }
        public bool Inverted { get { return CbxInvert.Checked; } set { CbxInvert.Checked = value; } }
        public bool IsXDirection { get { return rbAxisX.Checked; } set { rbAxisX.Checked = value; } }
        public string VJoyPOV { get { return dropDownVJoy.SelectedKey; } set { dropDownVJoy.SelectedKey = value; } }

        public void ToXml(System.Xml.XmlNode parentNode)
        {
            var node = parentNode.AddElement(TagName);
            node.SetAttribute("joystick", dropDownJoystick.SelectedKey);
            node.SetAttribute("inverted", Inverted.ToXml());
            node.SetAttribute("direction", IsXDirection ? "x" : "y");
            node.SetAttribute("vjoy", dropDownVJoy.SelectedKey);
        }

        public void FromXml(System.Xml.XmlNode node)
        {
            dropDownJoystick.SelectedKey = node.GetAttribute("joystick");
            Inverted = node.GetAttribute("inverted") == "true";
            IsXDirection = node.GetAttribute("direction") != "y";
            dropDownVJoy.SelectedKey = node.GetAttribute("vjoy");
        }

        public void Initialize(CompileInfo info)
        {
            info.RegisterJoystick(JoystickAxis);
            info.RegisterPOV(VJoyPOV);
        }

        public void Declaration(CompileInfo info, System.IO.StreamWriter file)
        {
        }

        public void PreFeed(CompileInfo info, System.IO.StreamWriter file)
        {
        }

        public void Feed(CompileInfo info, System.IO.StreamWriter file)
        {
            if (String.IsNullOrEmpty(JoystickAxis))
                return;

            file.Write(new string(' ', info.IndentLevel * 4));
            var parts = JoystickAxis.Split('.');
            file.Write("iReport."); file.Write(VJoyPOV); file.Write(" = FakePOV_"); file.Write(IsXDirection ? "X" : "Y"); file.Write("(");
            file.Write("iReport."); file.Write(VJoyPOV);
            file.Write(", ");
            if (Inverted)
            {
                file.Write("(65536 - "); file.Write(JoystickAxis.Replace("joystick", "state")); file.Write(")");
            }
            else
            {
                file.Write(JoystickAxis.Replace("joystick", "state"));
            }
            file.WriteLine(");");
        }

        public void PostFeed(CompileInfo info, System.IO.StreamWriter file)
        {
        }


        public void Apply(DeviceListItem item)
        {
            JoystickAxis = item.Item.Key + "." + item.DetectedValue;
        }
    }
}
