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
    public partial class MapItemBase : UserControl
    {
        public MapItemBase()
        {
            InitializeComponent();
            ArrangeControls();
        }
        protected override void OnLoad(EventArgs e)
        {
            if (!DesignMode)
            {
                base.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
                BtnDelete.Font = MainForm.BoldFont;
                BtnDelete.Click += BtnDelete_Click;
            }
            base.OnLoad(e);
        }
        protected override Size DefaultSize
        {
            get { return new System.Drawing.Size(512, 32); }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [DefaultValue(DockStyle.None), EditorBrowsable(EditorBrowsableState.Never)]
        public new DockStyle Dock { get { return base.Dock; } set { base.Dock = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [DefaultValue(AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right), EditorBrowsable(EditorBrowsableState.Never)]
        public new AnchorStyles Anchor { get { return base.Anchor; } set { base.Anchor = value; } }

        void BtnDelete_Click(object sender, EventArgs e)
        {
            var parent = this.Parent as MapLevel;
            if (parent == null) return;
            parent.RemoveItem(this);
        }
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            ArrangeControls();
        }

        private void ArrangeControls()
        {
            BtnDelete.Location = new Point(3, (Height - BtnDelete.Height) / 2);
            BtnUp.Location = new Point(Width - BtnUp.Width - 3, Height / 2 - BtnUp.Height);
            BtnDown.Location = new Point(Width - BtnDown.Width - 3, Height / 2);

            var left = BtnDelete.Right + 3;
            ItemsPanel.SetBounds(left, 0, BtnUp.Left - left - 3, Height);
        }

        protected class LocationFreeButton : Button
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
            [EditorBrowsable(EditorBrowsableState.Never)]
            public new Point Location { get { return base.Location; } set { base.Location = value; } }
        }
        protected class LocationFreePanel : Panel
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
            [EditorBrowsable(EditorBrowsableState.Never)]
            public new Size Size { get { return base.Size; } set { base.Size = value; } }
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
            [EditorBrowsable(EditorBrowsableState.Never)]
            public new Point Location { get { return base.Location; } set { base.Location = value; } }
        }

        private void BtnUp_Click(object sender, EventArgs e)
        {
            var parent = Parent as MapLevel;
            if (parent == null)
                return;
            parent.MoveItemUp(this);
        }

        private void BtnDown_Click(object sender, EventArgs e)
        {
            var parent = Parent as MapLevel;
            if (parent == null)
                return;
            parent.MoveItemDown(this);
        }
    }
}
