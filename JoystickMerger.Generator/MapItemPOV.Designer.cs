namespace JoystickMerger.Generator
{
    partial class MapItemPOV
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
            this.dropDownJoystick = new JoystickMerger.Generator.DropDownJoystickPOV();
            this.ItemsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ItemsPanel
            // 
            this.ItemsPanel.Controls.Add(this.dropDownVJoy);
            this.ItemsPanel.Controls.Add(this.dropDownJoystick);
            // 
            // dropDownVJoy
            // 
            this.dropDownVJoy.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.dropDownVJoy.FormattingEnabled = true;
            this.dropDownVJoy.Location = new System.Drawing.Point(374, 5);
            this.dropDownVJoy.Name = "dropDownVJoy";
            this.dropDownVJoy.Size = new System.Drawing.Size(75, 21);
            this.dropDownVJoy.TabIndex = 1;
            // 
            // dropDownJoystick
            // 
            this.dropDownJoystick.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.dropDownJoystick.FormattingEnabled = true;
            this.dropDownJoystick.Location = new System.Drawing.Point(3, 5);
            this.dropDownJoystick.Name = "dropDownJoystick";
            this.dropDownJoystick.Size = new System.Drawing.Size(226, 21);
            this.dropDownJoystick.TabIndex = 0;
            // 
            // MapItemPOV
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "MapItemPOV";
            this.ItemsPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DropDownVJoyPOV dropDownVJoy;
        private DropDownJoystickPOV dropDownJoystick;
    }
}
