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
            node.SetAttribute("button", (Button - 1).ToString());
            node.SetAttribute("deactivatedText", IfFalseSayText);
            IfFalseMappings.ToXml(node.AddElement("Deactivated"));
            node.SetAttribute("activatedText", IfTrueSayText);
            IfTrueMappings.ToXml(node.AddElement("Activated"));
        }


        public void FromXml(System.Xml.XmlNode node)
        {
            int value;
            if (Int32.TryParse(node.GetAttribute("button"), out value))
                Button = value + 1;

            IfFalseSayText = node.GetAttribute("deactivatedText");
            IfFalseMappings.FromXml(node.SelectSingleNode("Deactivated"));
            IfTrueSayText = node.GetAttribute("activatedText");
            IfTrueMappings.FromXml(node.SelectSingleNode("Activated"));
        }
    }
}
