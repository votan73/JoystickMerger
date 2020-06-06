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
    public partial class MapItemBase : UserControl, IDataObject
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
                BtnDelete.Click += BtnDelete_Click;
                MouseDown += MapItemBase_MouseDown;
                MouseMove += MapItemBase_MouseMove;
                MouseUp += MapItemBase_MouseUp;
                GiveFeedback += MapItemBase_GiveFeedback;
                ItemsPanel.MouseDown += MapItemBase_MouseDown;
                ItemsPanel.MouseMove += MapItemBase_MouseMove;
                ItemsPanel.MouseUp += MapItemBase_MouseUp;
                ItemsPanel.GiveFeedback += MapItemBase_GiveFeedback;
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

        #region Drag'n'Drop
        object IDataObject.GetData(Type format)
        {
            System.Diagnostics.Debug.WriteLine("object IDataObject.GetData(Type format) " + format);
            if (format == typeof(MapItemBase))
            {
                System.Diagnostics.Debug.WriteLine("jo object IDataObject.GetData(Type format) " + format);
                return this;
            }
            return null;
        }

        object IDataObject.GetData(string format)
        {
            System.Diagnostics.Debug.WriteLine("object IDataObject.GetData(string format) " + format);
            if (format == typeof(MapItemBase).FullName)
            {
                System.Diagnostics.Debug.WriteLine("jo object IDataObject.GetData(string format) " + format);
                return this;
            }
            return null;
        }

        object IDataObject.GetData(string format, bool autoConvert)
        {
            System.Diagnostics.Debug.WriteLine("object IDataObject.GetData(string format, bool autoConvert) " + format);
            System.Diagnostics.Debug.WriteLine(format);
            if (format == typeof(MapItemBase).FullName)
            {
                System.Diagnostics.Debug.WriteLine("jo object IDataObject.GetData(string format, bool autoConvert) " + format);
                return this;
            }
            return null;
        }

        bool IDataObject.GetDataPresent(Type format)
        {
            if (format == this.GetType())
                return true;
            if (format == typeof(MapItemBase))
                return true;
            return false;
        }

        bool IDataObject.GetDataPresent(string format)
        {
            if (format == this.GetType().FullName)
                return true;
            if (format == typeof(MapItemBase).FullName)
                return true;
            return false;
        }

        bool IDataObject.GetDataPresent(string format, bool autoConvert)
        {
            if (format == this.GetType().FullName)
                return true;
            if (format == typeof(MapItemBase).FullName)
                return true;
            return false;
        }

        string[] IDataObject.GetFormats()
        {
            return new string[] { this.GetType().FullName };
        }

        string[] IDataObject.GetFormats(bool autoConvert)
        {
            return new string[] { this.GetType().FullName };
        }

        void IDataObject.SetData(object data)
        {
        }

        void IDataObject.SetData(Type format, object data)
        {
        }

        void IDataObject.SetData(string format, object data)
        {
        }

        void IDataObject.SetData(string format, bool autoConvert, object data)
        {
        }

        Rectangle dragBoxFromMouseDown;
        bool inDragMode;
        void MapItemBase_MouseDown(object sender, MouseEventArgs e)
        {
            // Remember the point where the mouse down occurred. The DragSize indicates
            // the size that the mouse can move before a drag event should be started.
            var dragSize = SystemInformation.DragSize;

            // Create a rectangle using the DragSize, with the mouse position being
            // at the center of the rectangle.
            dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2), e.Y - (dragSize.Height / 2)), dragSize);
            inDragMode = true;
        }

        void MapItemBase_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left && inDragMode)
            {
                // If the mouse moves outside the rectangle, start the drag.
                if (dragBoxFromMouseDown != Rectangle.Empty && !dragBoxFromMouseDown.Contains(e.X, e.Y))
                {
                    try
                    {
                        // Proceed with the drag and drop, passing in the list item.
                        var args = new DragEventArgs(this as IDataObject, 0, e.X, e.Y, DragDropEffects.Move, DragDropEffects.None);
                        BeginDrag(this, args);
                    }
                    finally
                    {
                        inDragMode = false;
                    }
                }
            }
        }
        void MapItemBase_MouseUp(object sender, MouseEventArgs e)
        {
            // Reset the drag rectangle when the mouse button is raised.
            dragBoxFromMouseDown = Rectangle.Empty;
            inDragMode = false;
        }
        Cursor cur;
        protected virtual void BeginDrag(Control Source, DragEventArgs e)
        {
            var old = Cursor.Current;
            try
            {
                using (var bmp = new Bitmap(this.Width, this.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
                {
                    this.DrawToBitmap(bmp, new Rectangle(Point.Empty, bmp.Size));
                    using (cur = new Cursor(bmp.GetHicon()))
                    {
                        Cursor.Current = cur;

                        this.DoDragDrop(this, e.AllowedEffect);
                    }
                }
            }
            finally
            {
                Cursor.Current = old;
                cur = null;
            }
        }
        void MapItemBase_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            e.UseDefaultCursors = false;
            Cursor.Current = cur;
        }
        #endregion

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
