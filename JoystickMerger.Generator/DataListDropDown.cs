using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JoystickMerger.Generator
{
    class DataListDropDown : ComboBox
    {
        public DataListDropDown()
        {
            base.ValueMember = "Key";
            base.DisplayMember = "Value";
            base.DropDownStyle = ComboBoxStyle.DropDownList;
            MaxDropDownItems = 12;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [DefaultValue(true)]
        public new bool FormattingEnabled { get { return base.FormattingEnabled; } set { base.FormattingEnabled = value; } }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [DefaultValue(null), EditorBrowsable(EditorBrowsableState.Never)]
        protected virtual IList DataList { get { return base.DataSource as IList; } set { base.DataSource = value; } }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [DefaultValue(null), EditorBrowsable(EditorBrowsableState.Never)]
        public new object DataSource { get { return base.DataSource; } set { } }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [DefaultValue(ComboBoxStyle.DropDownList), EditorBrowsable(EditorBrowsableState.Never)]
        public new ComboBoxStyle DropDownStyle { get { return base.DropDownStyle; } set { } }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new ComboBox.ObjectCollection Items { get { return base.Items; } }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [DefaultValue("Key"), EditorBrowsable(EditorBrowsableState.Never)]
        public new string ValueMember { get { return base.ValueMember; } set { base.ValueMember = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [DefaultValue("Value"), EditorBrowsable(EditorBrowsableState.Never)]
        public new string DisplayMember { get { return base.DisplayMember; } set { base.DisplayMember = value; } }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [DefaultValue(12), EditorBrowsable(EditorBrowsableState.Never)]
        public new int MaxDropDownItems { get { return base.MaxDropDownItems; } set { base.MaxDropDownItems = value; } }
    }
}
