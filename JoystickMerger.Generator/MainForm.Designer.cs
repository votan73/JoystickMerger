namespace JoystickMerger.Generator
{
    partial class MainForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.RootLevel = new System.Windows.Forms.Panel();
            this.mapLevel1 = new JoystickMerger.Generator.MapLevel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.BtnGenerate = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.BtnLoad = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.saveXmlFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.openXmlFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.deviceList1 = new JoystickMerger.Generator.DeviceList();
            this.saveExeFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.RootLevel.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // RootLevel
            // 
            this.RootLevel.AutoScroll = true;
            this.RootLevel.BackColor = System.Drawing.Color.Transparent;
            this.RootLevel.Controls.Add(this.mapLevel1);
            this.RootLevel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RootLevel.Location = new System.Drawing.Point(0, 66);
            this.RootLevel.Name = "RootLevel";
            this.RootLevel.Size = new System.Drawing.Size(624, 535);
            this.RootLevel.TabIndex = 1;
            // 
            // mapLevel1
            // 
            this.mapLevel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.mapLevel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.mapLevel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.mapLevel1.Location = new System.Drawing.Point(0, 0);
            this.mapLevel1.Name = "mapLevel1";
            this.mapLevel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.mapLevel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.mapLevel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.mapLevel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.mapLevel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.mapLevel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.mapLevel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.mapLevel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.mapLevel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.mapLevel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.mapLevel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.mapLevel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.mapLevel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.mapLevel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.mapLevel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.mapLevel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.mapLevel1.TabIndex = 1;
            this.mapLevel1.Resize += new System.EventHandler(this.mapLevel1_Resize);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.BtnGenerate);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.BtnLoad);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(624, 32);
            this.panel1.TabIndex = 5;
            // 
            // BtnGenerate
            // 
            this.BtnGenerate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnGenerate.Location = new System.Drawing.Point(512, 3);
            this.BtnGenerate.Name = "BtnGenerate";
            this.BtnGenerate.Size = new System.Drawing.Size(100, 23);
            this.BtnGenerate.TabIndex = 2;
            this.BtnGenerate.Text = "Generate";
            this.BtnGenerate.UseVisualStyleBackColor = true;
            this.BtnGenerate.Click += new System.EventHandler(this.BtnGenerate_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(118, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 23);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // BtnLoad
            // 
            this.BtnLoad.Location = new System.Drawing.Point(12, 3);
            this.BtnLoad.Name = "BtnLoad";
            this.BtnLoad.Size = new System.Drawing.Size(100, 23);
            this.BtnLoad.TabIndex = 0;
            this.BtnLoad.Text = "Load";
            this.BtnLoad.UseVisualStyleBackColor = true;
            this.BtnLoad.Click += new System.EventHandler(this.BtnLoad_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(123, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Select controllers to use:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Location = new System.Drawing.Point(0, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(146, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Mapping: (Click plus for more)";
            // 
            // openXmlFileDialog
            // 
            this.openXmlFileDialog.FileName = "openFileDialog1";
            // 
            // deviceList1
            // 
            this.deviceList1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.deviceList1.Dock = System.Windows.Forms.DockStyle.Top;
            this.deviceList1.Location = new System.Drawing.Point(0, 45);
            this.deviceList1.Name = "deviceList1";
            this.deviceList1.Padding = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.deviceList1.TabIndex = 3;
            this.deviceList1.Validated += new System.EventHandler(this.checkedListBox1_Validated);
            // 
            // saveExeFileDialog
            // 
            this.saveExeFileDialog.Filter = "Feeder Executable|*.exe";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 601);
            this.Controls.Add(this.RootLevel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.deviceList1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.Name = "MainForm";
            this.Text = "Joystick Merger Generator";
            this.RootLevel.ResumeLayout(false);
            this.RootLevel.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel RootLevel;
        private MapLevel mapLevel1;
        private DeviceList deviceList1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button BtnGenerate;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button BtnLoad;
        private System.Windows.Forms.SaveFileDialog saveXmlFileDialog;
        private System.Windows.Forms.OpenFileDialog openXmlFileDialog;
        private System.Windows.Forms.SaveFileDialog saveExeFileDialog;

    }
}

