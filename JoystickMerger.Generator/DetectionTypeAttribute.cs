using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoystickMerger.Generator
{
    class DetectionTypeAttribute : Attribute
    {
        public DetectionTypeAttribute(DetectionType type)
        {
            this.DetectionType = type;
        }
        public DetectionType DetectionType { get; set; }
    }
}
