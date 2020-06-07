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
    [DetectionType(DetectionType.Button)]
    partial class MapItemSwitch : MapItemConditionBase, IMapItem
    {
        public static string TagName = "Switch";
        public static string DisplayText = "Switch";

        public MapItemSwitch()
        {
            InitializeComponent();
        }

        public void ToXml(System.Xml.XmlNode parentNode)
        {
            var node = parentNode.AddElement("Switch");
            node.SetAttribute("button", (Button - 1).ToString());
            node.SetAttribute("offText", IfFalseSayText);
            IfFalseMappings.ToXml(node.AddElement("Off"));
            node.SetAttribute("onText", IfTrueSayText);
            IfTrueMappings.ToXml(node.AddElement("On"));
        }

        public void FromXml(System.Xml.XmlNode node)
        {
            int value;
            if (Int32.TryParse(node.GetAttribute("button"), out value))
                Button = value + 1;

            IfFalseSayText = node.GetAttribute("offText");
            IfFalseMappings.FromXml(node.SelectSingleNode("Off"));
            IfTrueSayText = node.GetAttribute("onText");
            IfTrueMappings.FromXml(node.SelectSingleNode("On"));
        }

        public void Initialize(CompileInfo info)
        {
            buttonName = null;
            info.RegisterVJoyButton(Button);
            IfFalseMappings.Initialize(info);
            IfTrueMappings.Initialize(info);
        }

        string buttonName;
        public void Declaration(CompileInfo info, System.IO.StreamWriter file)
        {
            buttonName = "switch" + info.RegisterIndex(this);

            file.Write("        Switch ");
            file.Write(buttonName);
            file.Write(" = new Switch()");
            if (String.IsNullOrEmpty(IfFalseSayText) && String.IsNullOrEmpty(IfTrueSayText))
                file.WriteLine(";");
            else
            {
                file.WriteLine();
                file.WriteLine("        {");
                file.WriteLine("            OnStateChanged = (button) =>");
                file.WriteLine("            {");
                file.WriteLine("                if (button.State)");
                if (String.IsNullOrEmpty(IfTrueSayText))
                    file.WriteLine("                    { }");
                else
                {
                    file.Write("                    Say(\"");
                    file.Write(IfTrueSayText);
                    file.WriteLine("\");");
                }
                file.WriteLine("                else");
                if (String.IsNullOrEmpty(IfFalseSayText))
                    file.WriteLine("                    { }");
                else
                {
                    file.Write("                    Say(\"");
                    file.Write(IfFalseSayText);
                    file.WriteLine("\");");
                }
                file.WriteLine("            }");
                file.WriteLine("        };");
            }
        }

        public void PreFeed(CompileInfo info, System.IO.StreamWriter file)
        {
        }

        public void Feed(CompileInfo info, System.IO.StreamWriter file)
        {
            if (String.IsNullOrEmpty(Joystick))
                return;

            file.Write(new string(' ', info.IndentLevel * 4));
            file.Write("if ("); file.Write(buttonName); file.Write(".Check("); file.Write(Joystick.Replace("joystick", "state")); file.Write(".Buttons["); file.Write(Button - 1); file.WriteLine("]))");
            file.Write(new string(' ', info.IndentLevel * 4));
            file.WriteLine("{");

            info.IndentLevel++;
            IfFalseMappings.Feed(info, file);
            info.IndentLevel--;
            file.Write(new string(' ', info.IndentLevel * 4));
            file.WriteLine("}");
            file.Write(new string(' ', info.IndentLevel * 4));
            file.WriteLine("else");
            file.Write(new string(' ', info.IndentLevel * 4));
            file.WriteLine("{");

            info.IndentLevel++;
            IfTrueMappings.Feed(info, file);
            info.IndentLevel--;
            file.Write(new string(' ', info.IndentLevel * 4));
            file.WriteLine("}");
        }

        public void PostFeed(CompileInfo info, System.IO.StreamWriter file)
        {
            buttonName = null;
        }


        public void Apply(DetectionResult result)
        {
            Joystick = result.Joystick;
            Button = result.From;
        }
    }
}
