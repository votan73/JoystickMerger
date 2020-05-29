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
    }
}
