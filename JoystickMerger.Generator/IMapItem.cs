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

        void Apply(DetectionResult result);

        void Initialize(CompileInfo info);
        void Declaration(CompileInfo info, System.IO.StreamWriter file);
        void PreFeed(CompileInfo info, System.IO.StreamWriter file);
        void Feed(CompileInfo info, System.IO.StreamWriter file);
        void PostFeed(CompileInfo info, System.IO.StreamWriter file);
    }
}
