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
    partial class MapItemButtons : MapItemBase, IMapItem
    {
        public static string TagName = "Buttons";
        public static string DisplayText = "Buttons";

        public MapItemButtons()
        {
            InitializeComponent();
        }

        bool isInUpdate;
        private void JoystickFrom_ValueChanged(object sender, EventArgs e)
        {
            if (isInUpdate)
                return;
            isInUpdate = true;
            NumJoystickTo.Value = Math.Max(NumJoystickFrom.Value, NumJoystickTo.Value);
            UpdateEndAt();
            isInUpdate = false;
        }

        private void JoystickTo_ValueChanged(object sender, EventArgs e)
        {
            if (isInUpdate)
                return;
            isInUpdate = true;
            NumJoystickFrom.Value = Math.Min(NumJoystickFrom.Value, NumJoystickTo.Value);
            UpdateEndAt();
            isInUpdate = false;
        }

        private void VJoyStartAt_ValueChanged(object sender, EventArgs e)
        {
            if (isInUpdate)
                return;
            isInUpdate = true;
            UpdateEndAt();
            isInUpdate = false;
        }

        private void UpdateEndAt()
        {
            var max = Math.Min(32, 32 - (NumJoystickTo.Value - NumJoystickFrom.Value));
            NumVJoyStartAt.Value = Math.Min(NumVJoyStartAt.Value, max);
            NumVJoyStartAt.Maximum = max;
            NumVJoyEndAt.Value = NumVJoyStartAt.Value + (NumJoystickTo.Value - NumJoystickFrom.Value);
        }

        public string Joystick
        {
            get { return DropDownJoystick.SelectedJoystick; }
            set { DropDownJoystick.SelectedJoystick = value; }
        }
        public int From
        {
            get { return Convert.ToInt32(NumJoystickFrom.Value); }
            set { NumJoystickFrom.Value = Math.Min(32, Math.Max(1, value)); }
        }
        public int Range
        {
            get { return Convert.ToInt32(1 + NumJoystickTo.Value - NumJoystickFrom.Value); }
            set { NumJoystickTo.Value = Math.Min(32, Math.Max(1, NumJoystickFrom.Value + value - 1)); }
        }
        public int MapTo
        {
            get { return Convert.ToInt32(NumVJoyStartAt.Value); }
            set { NumVJoyStartAt.Value = Math.Min(32, Math.Max(1, value)); }
        }

        public void ToXml(System.Xml.XmlNode parentNode)
        {
            var node = parentNode.AddElement(TagName);
            node.SetAttribute("joystick", Joystick);
            node.SetAttribute("from", (From - 1).ToString());
            node.SetAttribute("range", Range.ToString());
            node.SetAttribute("mapTo", (MapTo - 1).ToString());
        }

        public void FromXml(System.Xml.XmlNode node)
        {
            isInUpdate = true;
            Joystick = node.GetAttribute("joystick");
            int value;
            if (Int32.TryParse(node.GetAttribute("from"), out value))
                From = value + 1;
            if (Int32.TryParse(node.GetAttribute("range"), out value))
                Range = value;
            if (Int32.TryParse(node.GetAttribute("mapTo"), out value))
                MapTo = value + 1;
            UpdateEndAt();
            isInUpdate = false;
        }

        public void Initialize(CompileInfo info)
        {
            info.RegisterJoystick(Joystick);
            info.RegisterVJoyButton(Convert.ToInt32(NumVJoyEndAt.Value));
        }

        public void Declaration(CompileInfo info, System.IO.StreamWriter file)
        {
        }

        public void PreFeed(CompileInfo info, System.IO.StreamWriter file)
        {
        }

        public void Feed(CompileInfo info, System.IO.StreamWriter file)
        {
            file.Write(new string(' ', info.IndentLevel * 4));
            file.Write("iReport.Buttons |= ButtonMapper.From(");
            file.Write(Joystick.Replace("joystick", "state"));
            file.Write(".Buttons, ");
            if (From > 1)
            {
                file.Write(From - 1);
                file.Write(", ");
                file.Write(Range);
                file.Write(", ");
                file.Write(MapTo - 1);
            }
            else
            {
                if (MapTo > 1)
                {
                    file.Write(Range);
                    file.Write(", ");
                    file.Write(MapTo - 1);
                }
                else
                    file.Write(Range);
            }
            file.WriteLine(");");
        }

        public void PostFeed(CompileInfo info, System.IO.StreamWriter file)
        {
        }
    }
}
