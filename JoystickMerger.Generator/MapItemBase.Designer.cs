namespace JoystickMerger.Generator
{
    partial class MapItemBase
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
            this.BtnDelete = new JoystickMerger.Generator.MapItemBase.LocationFreeButton();
            this.BtnUp = new JoystickMerger.Generator.MapItemBase.LocationFreeButton();
            this.BtnDown = new JoystickMerger.Generator.MapItemBase.LocationFreeButton();
            this.ItemsPanel = new JoystickMerger.Generator.MapItemBase.LocationFreePanel();
            this.SuspendLayout();
            // 
            // BtnDelete
            // 
            this.BtnDelete.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.BtnDelete.BackColor = System.Drawing.Color.OrangeRed;
            this.BtnDelete.FlatAppearance.BorderSize = 0;
            this.BtnDelete.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Maroon;
            this.BtnDelete.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Red;
            this.BtnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnDelete.ForeColor = System.Drawing.Color.White;
            this.BtnDelete.Location = new System.Drawing.Point(0, 1);
            this.BtnDelete.Name = "BtnDelete";
            this.BtnDelete.Size = new System.Drawing.Size(24, 24);
            this.BtnDelete.TabIndex = 3;
            this.BtnDelete.TabStop = false;
            this.BtnDelete.Text = "❌";
            this.BtnDelete.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.BtnDelete.UseVisualStyleBackColor = false;
            // 
            // BtnUp
            // 
            this.BtnUp.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.BtnUp.FlatAppearance.BorderSize = 0;
            this.BtnUp.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Silver;
            this.BtnUp.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.BtnUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnUp.Location = new System.Drawing.Point(487, -8);
            this.BtnUp.Name = "BtnUp";
            this.BtnUp.Size = new System.Drawing.Size(24, 24);
            this.BtnUp.TabIndex = 1;
            this.BtnUp.Text = "▲";
            this.BtnUp.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.BtnUp.UseVisualStyleBackColor = true;
            this.BtnUp.Click += new System.EventHandler(this.BtnUp_Click);
            // 
            // BtnDown
            // 
            this.BtnDown.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.BtnDown.FlatAppearance.BorderSize = 0;
            this.BtnDown.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Silver;
            this.BtnDown.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.BtnDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnDown.Location = new System.Drawing.Point(487, 16);
            this.BtnDown.Name = "BtnDown";
            this.BtnDown.Size = new System.Drawing.Size(24, 24);
            this.BtnDown.TabIndex = 2;
            this.BtnDown.Text = "▼";
            this.BtnDown.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.BtnDown.UseVisualStyleBackColor = true;
            this.BtnDown.Click += new System.EventHandler(this.BtnDown_Click);
            // 
            // ItemsPanel
            // 
            this.ItemsPanel.Name = "ItemsPanel";
            this.ItemsPanel.TabIndex = 0;
            // 
            // MapItemBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ItemsPanel);
            this.Controls.Add(this.BtnDown);
            this.Controls.Add(this.BtnUp);
            this.Controls.Add(this.BtnDelete);
            this.Name = "MapItemBase";
            this.Size = new System.Drawing.Size(512, 32);
            this.ResumeLayout(false);

        }

        #endregion

        private LocationFreeButton BtnDelete;
        private LocationFreeButton BtnUp;
        private LocationFreeButton BtnDown;
        protected LocationFreePanel ItemsPanel;
    }
}
