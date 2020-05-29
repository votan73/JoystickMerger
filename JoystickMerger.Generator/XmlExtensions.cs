using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace JoystickMerger.Generator
{
    static class XmlExtensions
    {
        public static XmlNode AddElement(this XmlNode parentNode, string name)
        {
            var node = parentNode.OwnerDocument.CreateElement(name);
            parentNode.AppendChild(node);
            return node;
        }
        public static XmlNode AddElement(this XmlDocument root, string name)
        {
            var node = root.CreateElement(name);
            root.AppendChild(node);
            return node;
        }
        public static string GetAttribute(this XmlNode node, string name)
        {
            var attr = node.Attributes[name];
            return attr != null ? attr.Value : String.Empty;
        }
        public static void SetAttribute(this XmlNode node, string name, string value)
        {
            (node.Attributes[name] ?? NewAttribute(node, name)).Value = value;
        }

        private static XmlAttribute NewAttribute(XmlNode node, string name)
        {
            var attr = node.OwnerDocument.CreateAttribute(name);
            node.Attributes.Append(attr);
            return attr;
        }

        public static string ToXml(this bool value)
        {
            return value ? "true" : "false";
        }
    }
}
