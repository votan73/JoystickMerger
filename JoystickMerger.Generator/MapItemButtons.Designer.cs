namespace JoystickMerger.Generator
{
    partial class MapItemButtons
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
            this.NumJoystickFrom = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.NumJoystickTo = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.NumVJoyEndAt = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.NumVJoyStartAt = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.ItemsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumJoystickFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumJoystickTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumVJoyEndAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumVJoyStartAt)).BeginInit();
            this.SuspendLayout();
            // 
            // ItemsPanel
            // 
            this.ItemsPanel.Controls.Add(this.NumVJoyEndAt);
            this.ItemsPanel.Controls.Add(this.label3);
            this.ItemsPanel.Controls.Add(this.NumVJoyStartAt);
            this.ItemsPanel.Controls.Add(this.label4);
            this.ItemsPanel.Controls.Add(this.NumJoystickTo);
            this.ItemsPanel.Controls.Add(this.label2);
            this.ItemsPanel.Controls.Add(this.NumJoystickFrom);
            this.ItemsPanel.Controls.Add(this.label1);
            this.ItemsPanel.Controls.Add(this.DropDownJoystick);
            // 
            // Joystick
            // 
            this.DropDownJoystick.FormattingEnabled = true;
            this.DropDownJoystick.Location = new System.Drawing.Point(3, 2);
            this.DropDownJoystick.Name = "Joystick";
            this.DropDownJoystick.Size = new System.Drawing.Size(226, 21);
            this.DropDownJoystick.TabIndex = 0;
            // 
            // JoystickFrom
            // 
            this.NumJoystickFrom.Location = new System.Drawing.Point(269, 3);
            this.NumJoystickFrom.Maximum = new decimal(new int[] {
            32,
            0,
            0,
            0});
            this.NumJoystickFrom.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NumJoystickFrom.Name = "JoystickFrom";
            this.NumJoystickFrom.Size = new System.Drawing.Size(45, 20);
            this.NumJoystickFrom.TabIndex = 2;
            this.NumJoystickFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.NumJoystickFrom.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NumJoystickFrom.ValueChanged += new System.EventHandler(this.JoystickFrom_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(237, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "From";
            // 
            // JoystickTo
            // 
            this.NumJoystickTo.Location = new System.Drawing.Point(269, 26);
            this.NumJoystickTo.Maximum = new decimal(new int[] {
            32,
            0,
            0,
            0});
            this.NumJoystickTo.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NumJoystickTo.Name = "JoystickTo";
            this.NumJoystickTo.Size = new System.Drawing.Size(45, 20);
            this.NumJoystickTo.TabIndex = 4;
            this.NumJoystickTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.NumJoystickTo.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NumJoystickTo.ValueChanged += new System.EventHandler(this.JoystickTo_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(248, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(20, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "To";
            // 
            // VJoyEndAt
            // 
            this.NumVJoyEndAt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.NumVJoyEndAt.Location = new System.Drawing.Point(489, 26);
            this.NumVJoyEndAt.Maximum = new decimal(new int[] {
            32,
            0,
            0,
            0});
            this.NumVJoyEndAt.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NumVJoyEndAt.Name = "VJoyEndAt";
            this.NumVJoyEndAt.ReadOnly = true;
            this.NumVJoyEndAt.Size = new System.Drawing.Size(45, 20);
            this.NumVJoyEndAt.TabIndex = 8;
            this.NumVJoyEndAt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.NumVJoyEndAt.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(468, 29);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(20, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "To";
            // 
            // VJoyStartAt
            // 
            this.NumVJoyStartAt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.NumVJoyStartAt.Location = new System.Drawing.Point(489, 3);
            this.NumVJoyStartAt.Maximum = new decimal(new int[] {
            32,
            0,
            0,
            0});
            this.NumVJoyStartAt.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NumVJoyStartAt.Name = "VJoyStartAt";
            this.NumVJoyStartAt.Size = new System.Drawing.Size(45, 20);
            this.NumVJoyStartAt.TabIndex = 6;
            this.NumVJoyStartAt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.NumVJoyStartAt.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NumVJoyStartAt.ValueChanged += new System.EventHandler(this.VJoyStartAt_ValueChanged);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(400, 6);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Map at vJoy from";
            // 
            // MapItemButtons
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "MapItemButtons";
            this.Size = new System.Drawing.Size(600, 48);
            this.ItemsPanel.ResumeLayout(false);
            this.ItemsPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumJoystickFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumJoystickTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumVJoyEndAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumVJoyStartAt)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DropDownJoystick DropDownJoystick;
        private System.Windows.Forms.NumericUpDown NumJoystickTo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown NumJoystickFrom;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown NumVJoyEndAt;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown NumVJoyStartAt;
        private System.Windows.Forms.Label label4;
    }
}
