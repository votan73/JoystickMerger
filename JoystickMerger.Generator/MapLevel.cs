using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JoystickMerger.Generator
{
    class MapLevel : TableLayoutPanel, IMapItem
    {
        readonly Button addButton = new Button();
        readonly ContextMenuStrip availableItems = new ContextMenuStrip();

        readonly static Dictionary<string, Type> mappings = new Dictionary<string, Type>();
        readonly static Dictionary<Type, string> mappingsDisplay = new Dictionary<Type, string>();

        static MapLevel()
        {
            var mapItem = typeof(MapItemBase);
            foreach (var type in typeof(MapLevel).Assembly.GetTypes().Where<Type>(x => mapItem.IsAssignableFrom(x)))
            {
                var tagNameField = type.GetField("TagName");
                if (tagNameField == null)
                    continue;
                var displayTextField = type.GetField("DisplayText");
                if (displayTextField == null)
                    continue;
                mappings[tagNameField.GetValue(null) as string] = type;
                mappingsDisplay[type] = displayTextField.GetValue(null) as string;
            }
        }

        public MapLevel()
        {
            base.ColumnCount = 1;
            base.RowCount = 1;
            base.AutoSize = true;
            addButton.Name = "Add";
            addButton.Text = "+";
            addButton.Width = addButton.Height = 32;
            addButton.Anchor = AnchorStyles.Top;
            addButton.FlatStyle = FlatStyle.Flat;
            addButton.FlatAppearance.BorderSize = 0;
            addButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Silver;
            addButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(224, 224, 224);
            addButton.Font = MainForm.BoldFont;
            addButton.BackColor = System.Drawing.Color.SteelBlue;
            Controls.Add(addButton);

            this.SetColumn(addButton, 0);
            this.SetRow(addButton, 1);
            this.Height = 32;
            base.Dock = DockStyle.Top;
            this.SetAutoSizeMode(System.Windows.Forms.AutoSizeMode.GrowAndShrink);
            this.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            base.AutoSize = true;
            this.DoubleBuffered = true;

            availableItems.RenderMode = ToolStripRenderMode.System;

            //availableItems.Items.Add(new ToolStripMenuItem("Axis", null, addItem_Click) { Tag = typeof(MapItemAxis) });
            //availableItems.Items.Add(new ToolStripMenuItem("POV", null, addItem_Click) { Tag = typeof(MapItemPOV) });
            //availableItems.Items.Add(new ToolStripMenuItem("Buttons", null, addItem_Click) { Tag = typeof(MapItemButtons) });
            //availableItems.Items.Add(new ToolStripMenuItem("Fake POV", null, addItem_Click) { Tag = typeof(MapItemFakePOV) });
            //availableItems.Items.Add(new ToolStripMenuItem("Button Toggle", null, addItem_Click) { Tag = typeof(MapItemButtonToggle) });
            //availableItems.Items.Add(new ToolStripMenuItem("Switch", null, addItem_Click) { Tag = typeof(MapItemSwitch) });
            foreach (var mapping in mappings)
                availableItems.Items.Add(new ToolStripMenuItem(mappingsDisplay[mapping.Value], null, addItem_Click) { Tag = mapping.Value });
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [DefaultValue(true), EditorBrowsable(EditorBrowsableState.Never)]
        public new bool AutoSize { get { return base.AutoSize; } set { } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [DefaultValue(1), EditorBrowsable(EditorBrowsableState.Never)]
        public new int RowCount { get { return base.RowCount; } set { base.RowCount = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [DefaultValue(1), EditorBrowsable(EditorBrowsableState.Never)]
        public new int ColumnCount { get { return base.ColumnCount; } set { base.ColumnCount = 1; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
        public new Size Size { get { return base.Size; } set { base.Size = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
        public new DockStyle Dock { get { return base.Dock; } set { base.Dock = value; } }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            if (!DesignMode)
            {
                addButton.Font = MainForm.BiggerFont;
                addButton.Click += addButton_Click;
            }
        }

        protected override void OnControlAdded(ControlEventArgs e)
        {
            RowCount = Controls.Count;
            SetRow(e.Control, RowCount - 2);
            base.OnControlAdded(e);
            SetRow(addButton, RowCount - 1);
            for (int i = 0; i < RowCount; i++)
                Controls.SetChildIndex(GetControlFromPosition(0, i), i);
        }

        protected override void OnControlRemoved(ControlEventArgs e)
        {
            RowCount = Controls.Count;
            base.OnControlRemoved(e);
            SetRow(addButton, RowCount - 1);
        }

        void addButton_Click(object sender, EventArgs e)
        {
            availableItems.Show(addButton, new Point(0, addButton.Height));
        }

        private void addItem_Click(object sender, EventArgs e)
        {
            var type = (sender as ToolStripItem).Tag as Type;
            if (type == null)
                return;

            AddMapItem(type);
        }

        private IMapItem AddMapItem(Type type)
        {
            SuspendLayout();
            try
            {
                RowCount++;
                var ctl = Activator.CreateInstance(type) as MapItemBase;
                Controls.Add(ctl);
                SetRow(ctl, RowCount - 2);
                return ctl as IMapItem;
            }
            finally
            {
                ResumeLayout(true);
            }
        }

        public bool RemoveItem(MapItemBase item)
        {
            var row = GetRow(item);
            if (row < 0)
                return false;
            Controls.Remove(item);

            return true;
        }

        public void MoveItemUp(MapItemBase item)
        {
            var index = GetRow(item);
            if (index <= 0 || RowCount < 3)
                return;
            SuspendLayout();
            try
            {
                var other = GetControlFromPosition(0, index - 1);
                SetRow(item, index - 1);
                SetRow(other, index);
                Controls.SetChildIndex(item, index - 1);
                Controls.SetChildIndex(other, index);
            }
            finally
            {
                ResumeLayout(true);
            }
        }

        public void MoveItemDown(MapItemBase item)
        {
            var index = GetRow(item);
            if (index >= RowCount - 1 || RowCount < 3)
                return;
            SuspendLayout();
            try
            {
                var other = GetControlFromPosition(0, index + 1);
                SetRow(other, index);
                SetRow(item, index + 1);
                Controls.SetChildIndex(item, index + 1);
                Controls.SetChildIndex(other, index);
            }
            finally
            {
                ResumeLayout(true);
            }
        }

        public IEnumerable<MapItemBase> Items { get { return Controls.OfType<MapItemBase>(); } }

        public void ToXml(System.Xml.XmlNode parentNode)
        {
            foreach (var map in Controls.OfType<IMapItem>())
                map.ToXml(parentNode);
        }


        public void FromXml(System.Xml.XmlNode node)
        {
            SuspendLayout();
            try
            {
                var list = new List<MapItemBase>(Controls.OfType<MapItemBase>());
                foreach (var item in list)
                    Controls.Remove(item);

                if (node == null) return;
                foreach (System.Xml.XmlNode child in node.ChildNodes)
                {
                    Type type;
                    if (mappings.TryGetValue(child.Name, out type))
                    {
                        var mapping = AddMapItem(type);
                        if (mapping != null)
                            mapping.FromXml(child);
                    }
                }
            }
            finally
            {
                ResumeLayout(true);
            }
        }
    }
}
