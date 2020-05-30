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
    partial class MapItemButtonToggle : MapItemConditionBase, IMapItem
    {
        public static string TagName = "ButtonToggle";
        public static string DisplayText = "Button Toggle";

        public MapItemButtonToggle()
        {
            InitializeComponent();
        }

        public void ToXml(System.Xml.XmlNode parentNode)
        {
            var node = parentNode.AddElement(TagName);
            node.SetAttribute("joystick", Joystick);
            node.SetAttribute("button", (Button - 1).ToString());
            node.SetAttribute("deactivatedText", IfFalseSayText);
            IfFalseMappings.ToXml(node.AddElement("Deactivated"));
            node.SetAttribute("activatedText", IfTrueSayText);
            IfTrueMappings.ToXml(node.AddElement("Activated"));
        }


        public void FromXml(System.Xml.XmlNode node)
        {
            Joystick = node.GetAttribute("joystick");
            int value;
            if (Int32.TryParse(node.GetAttribute("button"), out value))
                Button = value + 1;

            IfFalseSayText = node.GetAttribute("deactivatedText");
            IfFalseMappings.FromXml(node.SelectSingleNode("Deactivated"));
            IfTrueSayText = node.GetAttribute("activatedText");
            IfTrueMappings.FromXml(node.SelectSingleNode("Activated"));
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
            buttonName = "buttonToggle" + info.RegisterIndex(this);

            file.Write("        ButtonToggle ");
            file.Write(buttonName);
            file.Write(" = new ButtonToggle()");
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
    }
}
