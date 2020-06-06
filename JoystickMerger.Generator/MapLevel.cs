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
        ContextMenuStrip availableItems;

        readonly static Dictionary<string, Type> mappings = new Dictionary<string, Type>();
        readonly static Dictionary<Type, string> mappingsDisplay = new Dictionary<Type, string>();
        readonly static Dictionary<DetectionType, List<Type>> mappingsDetectionType = new Dictionary<DetectionType, List<Type>>();

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
                foreach (DetectionTypeAttribute attribute in type.GetCustomAttributes(typeof(DetectionTypeAttribute), false))
                {
                    var detectType = attribute.DetectionType;
                    List<Type> list;
                    if (!mappingsDetectionType.TryGetValue(detectType, out list))
                        mappingsDetectionType[detectType] = list = new List<Type>();
                    list.Add(type);
                }
            }
        }

        public MapLevel()
        {
            base.ColumnCount = 1;
            base.RowCount = 1;
            base.AutoSize = true;
            addButton.Name = "Add";
            addButton.Text = "➕";
            addButton.Width = addButton.Height = 32;
            addButton.Anchor = AnchorStyles.Top;
            addButton.UseVisualStyleBackColor = false;
            addButton.FlatStyle = FlatStyle.Flat;
            addButton.FlatAppearance.BorderSize = 0;
            addButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Silver;
            addButton.FlatAppearance.MouseOverBackColor = SystemColors.Highlight;
            addButton.TextAlign = ContentAlignment.MiddleCenter;
            addButton.TextImageRelation = TextImageRelation.Overlay;
            addButton.BackColor = System.Drawing.Color.SteelBlue;
            addButton.UseMnemonic = false;
            Controls.Add(addButton);

            this.SetColumn(addButton, 0);
            this.SetRow(addButton, 1);
            this.Height = 32;
            base.Dock = DockStyle.Top;
            this.SetAutoSizeMode(System.Windows.Forms.AutoSizeMode.GrowAndShrink);
            this.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            base.AutoSize = true;
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
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
        public new TableLayoutRowStyleCollection RowStyles { get { return base.RowStyles; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
        public new TableLayoutColumnStyleCollection ColumnStyles { get { return base.ColumnStyles; } }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            if (!DesignMode)
            {
                if (availableItems == null)
                {
                    availableItems = new ContextMenuStrip();
                    availableItems.Items.Add(new ToolStripMenuItem("Auto Select...", null, autoSelect_Click));
                    foreach (var mapping in mappings)
                        availableItems.Items.Add(new ToolStripMenuItem(mappingsDisplay[mapping.Value], null, addItem_Click) { Tag = mapping.Value });
                    availableItems.RenderMode = ToolStripRenderMode.System;
                }
                addButton.Font = MainForm.BiggerFont;
                addButton.Click += addButton_Click;
                DragEnter += MapLevel_DragEnter;
                DragOver += MapLevel_DragEnter;
                DragDrop += MapLevel_DragDrop;
                AllowDrop = true;
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
            availableItems.Tag = null;
            foreach (ToolStripItem item in availableItems.Items)
                item.Visible = true;
            availableItems.Show(addButton, new Point(0, addButton.Height));
        }

        private void autoSelect_Click(object sender, EventArgs e)
        {
            using (var dialog = new FindControllerPropertyDialog())
            {
                var deviceList = (FindForm() as MainForm).DeviceList;
                dialog.DeviceList = deviceList;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    var item = deviceList.RecentTouchedDevice();
                    List<Type> list;
                    if (!mappingsDetectionType.TryGetValue(item.DetectedType, out list))
                        return;
                    ToolStripItem onlyOne = null;
                    int count = 0;
                    foreach (ToolStripItem menu in availableItems.Items)
                    {
                        var type = menu.Tag as Type;
                        if (list.Contains(type))
                        {
                            menu.Visible = true;
                            onlyOne = menu;
                            count++;
                        }
                        else
                            menu.Visible = false;
                    }
                    if (count == 0)
                        return;
                    availableItems.Tag = item;
                    if (count == 1)
                        onlyOne.PerformClick();
                    else
                        availableItems.Show(MousePosition);
                }
            }
        }

        private void addItem_Click(object sender, EventArgs e)
        {
            var type = (sender as ToolStripItem).Tag as Type;
            if (type == null)
                return;

            var item = AddMapItem(type);
            if (availableItems.Tag != null)
            {
                var deviceItem = availableItems.Tag as DeviceListItem;
                item.Apply(deviceItem);
                availableItems.Tag = null;
            }
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

        #region Drag'n'Drop
        void MapLevel_DragDrop(object sender, DragEventArgs e)
        {
            var control = e.Data.GetData(typeof(MapItemBase)) as MapItemBase;
            if (Controls.Contains(control))
            {
                var oldRow = this.GetRow(control);
                var other = this.GetChildAtPoint(PointToClient(new Point(e.X, e.Y)));
                if (other != null)
                {
                    var newRow = this.GetRow(other);
                    if (newRow > oldRow)
                        Controls.SetChildIndex(control, newRow);
                    else if (newRow > 0)
                        Controls.SetChildIndex(control, newRow - 1);
                    else
                        Controls.SetChildIndex(control, newRow);
                }
                else
                {
                    var newRow = RowCount - 2;
                    Controls.SetChildIndex(control, newRow);
                }
                int index = 0;
                foreach (Control ctl in Controls)
                    SetRow(ctl, index++);
            }
            else
                Controls.Add(control);
        }

        void MapLevel_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(MapItemBase)))
                e.Effect = DragDropEffects.Move & e.AllowedEffect;
            else
                e.Effect = DragDropEffects.None;
        }
        #endregion
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

        private IEnumerable<IMapItem> GetMapItems()
        {
            for (int i = 0; i < RowCount; i++)
            {
                var item = GetControlFromPosition(0, i) as IMapItem;
                if (item != null)
                    yield return item;
            }
        }
        public void Initialize(CompileInfo info)
        {
            foreach (var item in GetMapItems())
                item.Initialize(info);
        }

        public void Declaration(CompileInfo info, System.IO.StreamWriter file)
        {
            foreach (var item in GetMapItems())
                item.Declaration(info, file);
        }


        public void PreFeed(CompileInfo info, System.IO.StreamWriter file)
        {
            foreach (var item in GetMapItems())
                item.PreFeed(info, file);
        }

        public void Feed(CompileInfo info, System.IO.StreamWriter file)
        {
            foreach (var item in GetMapItems())
                item.Feed(info, file);
        }

        public void PostFeed(CompileInfo info, System.IO.StreamWriter file)
        {
            foreach (var item in GetMapItems())
                item.PostFeed(info, file);
        }

        public void Apply(DeviceListItem item)
        {
            throw new NotSupportedException();
        }
    }
}
