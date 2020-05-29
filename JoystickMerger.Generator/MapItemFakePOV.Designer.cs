namespace JoystickMerger.Generator
{
    partial class MapItemFakePOV
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
            this.dropDownVJoy = new JoystickMerger.Generator.DropDownVJoyPOV();
            this.dropDownJoystick = new JoystickMerger.Generator.DropDownJoystickAxis();
            this.rbAxisX = new System.Windows.Forms.RadioButton();
            this.rbAxisY = new System.Windows.Forms.RadioButton();
            this.ItemsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ItemsPanel
            // 
            this.ItemsPanel.Controls.Add(this.rbAxisY);
            this.ItemsPanel.Controls.Add(this.rbAxisX);
            this.ItemsPanel.Controls.Add(this.dropDownVJoy);
            this.ItemsPanel.Controls.Add(this.dropDownJoystick);
            // 
            // dropDownVJoy
            // 
            this.dropDownVJoy.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.dropDownVJoy.Location = new System.Drawing.Point(374, 6);
            this.dropDownVJoy.Name = "dropDownVJoy";
            this.dropDownVJoy.SelectedKey = "bHats";
            this.dropDownVJoy.Size = new System.Drawing.Size(75, 21);
            this.dropDownVJoy.TabIndex = 3;
            // 
            // dropDownJoystick
            // 
            this.dropDownJoystick.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.dropDownJoystick.Location = new System.Drawing.Point(3, 6);
            this.dropDownJoystick.Name = "dropDownJoystick";
            this.dropDownJoystick.SelectedKey = "";
            this.dropDownJoystick.Size = new System.Drawing.Size(226, 21);
            this.dropDownJoystick.TabIndex = 0;
            // 
            // rbAxisX
            // 
            this.rbAxisX.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rbAxisX.AutoSize = true;
            this.rbAxisX.Checked = true;
            this.rbAxisX.Location = new System.Drawing.Point(298, 7);
            this.rbAxisX.Name = "rbAxisX";
            this.rbAxisX.Size = new System.Drawing.Size(32, 17);
            this.rbAxisX.TabIndex = 1;
            this.rbAxisX.TabStop = true;
            this.rbAxisX.Text = "X";
            this.rbAxisX.UseVisualStyleBackColor = true;
            // 
            // rbAxisY
            // 
            this.rbAxisY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rbAxisY.AutoSize = true;
            this.rbAxisY.Location = new System.Drawing.Point(336, 7);
            this.rbAxisY.Name = "rbAxisY";
            this.rbAxisY.Size = new System.Drawing.Size(32, 17);
            this.rbAxisY.TabIndex = 2;
            this.rbAxisY.Text = "Y";
            this.rbAxisY.UseVisualStyleBackColor = true;
            // 
            // MapItemFakePOV
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "MapItemFakePOV";
            this.ItemsPanel.ResumeLayout(false);
            this.ItemsPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DropDownVJoyPOV dropDownVJoy;
        private DropDownJoystickAxis dropDownJoystick;
        private System.Windows.Forms.RadioButton rbAxisY;
        private System.Windows.Forms.RadioButton rbAxisX;
    }
}
