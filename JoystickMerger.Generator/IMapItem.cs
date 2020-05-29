using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoystickMerger.Generator
{
    interface IMapItem
    {
        void ToXml(System.Xml.XmlNode parentNode);
        void FromXml(System.Xml.XmlNode node);
    }
}
