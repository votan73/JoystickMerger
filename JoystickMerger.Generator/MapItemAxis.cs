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
    partial class MapItemAxis : MapItemBase, IMapItem
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

        public void Initialize(CompileInfo info)
        {
            info.RegisterJoystick(JoystickAxis);
            info.RegisterVJoyAxis(VJoyAxis);
        }

        public void Declaration(CompileInfo info, System.IO.StreamWriter file)
        {
        }

        public void PreFeed(CompileInfo info, System.IO.StreamWriter file)
        {
        }

        public void Feed(CompileInfo info, System.IO.StreamWriter file)
        {
            var parts = JoystickAxis.Split('.');

            file.Write(new string(' ', info.IndentLevel * 4));
            file.Write("iReport."); file.Write(VJoyAxis);
            file.Write(" = (int)(DeadZone(");
            if (Inverted)
            {
                file.Write("(65536 - "); file.Write(JoystickAxis.Replace("joystick", "state")); file.Write(")");
            }
            else
            {
                file.Write(JoystickAxis.Replace("joystick", "state"));
            }
            file.Write(", "); file.Write(parts[0].Replace("joystick", "deadzone")); file.WriteLine(") * axisScale);");
        }

        public void PostFeed(CompileInfo info, System.IO.StreamWriter file)
        {
        }
    }
}
