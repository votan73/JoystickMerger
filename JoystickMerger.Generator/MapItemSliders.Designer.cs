namespace JoystickMerger.Generator
{
    partial class MapItemSliders
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.DropDownJoystick = new JoystickMerger.Generator.DropDownJoystick();
            this.NumJoystickPos = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.DropDownVJoy = new JoystickMerger.Generator.DropDownVJoyAxis();
            this.ItemsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumJoystickPos)).BeginInit();
            this.SuspendLayout();
            // 
            // ItemsPanel
            // 
            this.ItemsPanel.Controls.Add(this.DropDownVJoy);
            this.ItemsPanel.Controls.Add(this.NumJoystickPos);
            this.ItemsPanel.Controls.Add(this.label1);
            this.ItemsPanel.Controls.Add(this.DropDownJoystick);
            // 
            // DropDownJoystick
            // 
            this.DropDownJoystick.Location = new System.Drawing.Point(3, 6);
            this.DropDownJoystick.Name = "DropDownJoystick";
            this.DropDownJoystick.SelectedJoystick = "";
            this.DropDownJoystick.Size = new System.Drawing.Size(226, 21);
            this.DropDownJoystick.TabIndex = 0;
            // 
            // NumJoystickFrom
            // 
            this.NumJoystickPos.Location = new System.Drawing.Point(284, 7);
            this.NumJoystickPos.Maximum = new decimal(new int[] {
            128,
            0,
            0,
            0});
            this.NumJoystickPos.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NumJoystickPos.Name = "NumJoystickFrom";
            this.NumJoystickPos.Size = new System.Drawing.Size(45, 20);
            this.NumJoystickPos.TabIndex = 2;
            this.NumJoystickPos.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.NumJoystickPos.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(235, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Slider #";
            // 
            // DropDownVJoySliders
            // 
            this.DropDownVJoy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DropDownVJoy.Location = new System.Drawing.Point(370, 6);
            this.DropDownVJoy.Name = "DropDownVJoySliders";
            this.DropDownVJoy.SelectedKey = "AxisSL0";
            this.DropDownVJoy.Size = new System.Drawing.Size(121, 21);
            this.DropDownVJoy.TabIndex = 3;
            // 
            // MapItemSliders
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "MapItemSliders";
            this.Size = new System.Drawing.Size(554, 32);
            this.ItemsPanel.ResumeLayout(false);
            this.ItemsPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumJoystickPos)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DropDownJoystick DropDownJoystick;
        private System.Windows.Forms.NumericUpDown NumJoystickPos;
        private System.Windows.Forms.Label label1;
        private DropDownVJoyAxis DropDownVJoy;
    }
}
