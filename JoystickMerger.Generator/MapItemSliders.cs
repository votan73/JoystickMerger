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
    partial class MapItemSliders : MapItemBase, IMapItem
    {
        public static string TagName = "Sliders";
        public static string DisplayText = "Sliders";

        public MapItemSliders()
        {
            InitializeComponent();
        }

        public string Joystick
        {
            get { return DropDownJoystick.SelectedJoystick; }
            set { DropDownJoystick.SelectedJoystick = value; }
        }
        public int Pos
        {
            get { return Convert.ToInt32(NumJoystickPos.Value); }
            set { NumJoystickPos.Value = Math.Min(128, Math.Max(1, value)); }
        }
        public string VJoyAxis { get { return DropDownVJoy.SelectedKey; } set { DropDownVJoy.SelectedKey = value; } }

        public void ToXml(System.Xml.XmlNode parentNode)
        {
            var node = parentNode.AddElement(TagName);
            node.SetAttribute("joystick", Joystick);
            node.SetAttribute("pos", (Pos - 1).ToString());
            node.SetAttribute("vjoy", VJoyAxis);
        }

        public void FromXml(System.Xml.XmlNode node)
        {
            Joystick = node.GetAttribute("joystick");
            int value;
            if (Int32.TryParse(node.GetAttribute("pos"), out value))
                Pos = value + 1;
            VJoyAxis = node.GetAttribute("vjoy");
        }

        public void Initialize(CompileInfo info)
        {
            info.RegisterJoystick(Joystick);
            //info.RegisterVJoyAxis(Convert.ToInt32(NumVJoyEndAt.Value));
        }

        public void Declaration(CompileInfo info, System.IO.StreamWriter file)
        {
        }

        public void PreFeed(CompileInfo info, System.IO.StreamWriter file)
        {
        }

        public void Feed(CompileInfo info, System.IO.StreamWriter file)
        {
            if (String.IsNullOrEmpty(Joystick))
                return;

            file.Write(new string(' ', info.IndentLevel * 4));
            file.Write("iReport."); file.Write(VJoyAxis);
            file.Write(" = (int)");
            file.Write(Joystick.Replace("joystick", "state"));
            file.Write(".Sliders[");
            file.Write(Pos - 1);
            file.Write("];");
        }

        public void PostFeed(CompileInfo info, System.IO.StreamWriter file)
        {
        }
    }
}
