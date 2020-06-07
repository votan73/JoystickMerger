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
    [DetectionType(DetectionType.PointOfView)]
    partial class MapItemPOV : MapItemBase, IMapItem
    {
        public static string TagName = "POV";
        public static string DisplayText = "POV";

        public MapItemPOV()
        {
            InitializeComponent();
        }

        public string JoystickPOV { get { return dropDownJoystick.SelectedKey; } set { dropDownJoystick.SelectedKey = value; } }
        public string VJoyPOV { get { return dropDownVJoy.SelectedKey; } set { dropDownVJoy.SelectedKey = value; } }

        public void ToXml(System.Xml.XmlNode parentNode)
        {
            var node = parentNode.AddElement(TagName);
            node.SetAttribute("joystick", JoystickPOV);
            //node.SetAttribute("inverted", Inverted.ToXml());
            node.SetAttribute("vjoy", VJoyPOV);
        }

        public void FromXml(System.Xml.XmlNode node)
        {
            JoystickPOV = node.GetAttribute("joystick");
            //Inverted = node.GetAttribute("inverted")!="false";
            VJoyPOV = node.GetAttribute("vjoy");
        }

        public void Initialize(CompileInfo info)
        {
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
            if (String.IsNullOrEmpty(JoystickPOV))
                return;

            file.Write(new string(' ', info.IndentLevel * 4));
            var parts = JoystickPOV.Split('.');
            file.Write("iReport."); file.Write(VJoyPOV); file.Write(" = (uint)"); file.Write(parts[0].Replace("joystick", "povs"));
            file.Write("["); file.Write(Int32.Parse(parts[1].Replace("POV", "")) - 1); file.WriteLine("];");
        }

        public void PostFeed(CompileInfo info, System.IO.StreamWriter file)
        {
        }


        public void Apply(DetectionResult result)
        {
            JoystickPOV = result.Joystick + ".POV" + result.Value;
        }
    }
}
