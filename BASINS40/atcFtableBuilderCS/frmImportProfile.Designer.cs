namespace atcFtableBuilder
{
    partial class frmImportProfile
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmImportProfile));
            this.lblNotice = new System.Windows.Forms.Label();
            this.gbContent = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.rdoDataDepth = new System.Windows.Forms.RadioButton();
            this.rdoDataElev = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rdoImportDataUnitUS = new System.Windows.Forms.RadioButton();
            this.rdoImportDataUnitSI = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdoImportDataCSV = new System.Windows.Forms.RadioButton();
            this.rdoImportDataExcel = new System.Windows.Forms.RadioButton();
            this.btnImport = new System.Windows.Forms.Button();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.txtChProfileDatafile = new System.Windows.Forms.TextBox();
            this.lblChDatafile = new System.Windows.Forms.Label();
            this.gbContent.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblNotice
            // 
            this.lblNotice.AutoSize = true;
            this.lblNotice.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblNotice.Location = new System.Drawing.Point(0, 0);
            this.lblNotice.Name = "lblNotice";
            this.lblNotice.Size = new System.Drawing.Size(35, 13);
            this.lblNotice.TabIndex = 1;
            this.lblNotice.Text = "label1";
            // 
            // gbContent
            // 
            this.gbContent.Controls.Add(this.groupBox3);
            this.gbContent.Controls.Add(this.groupBox2);
            this.gbContent.Controls.Add(this.groupBox1);
            this.gbContent.Controls.Add(this.btnImport);
            this.gbContent.Controls.Add(this.btnBrowse);
            this.gbContent.Controls.Add(this.txtChProfileDatafile);
            this.gbContent.Controls.Add(this.lblChDatafile);
            this.gbContent.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gbContent.Location = new System.Drawing.Point(0, 118);
            this.gbContent.Name = "gbContent";
            this.gbContent.Size = new System.Drawing.Size(578, 248);
            this.gbContent.TabIndex = 2;
            this.gbContent.TabStop = false;
            this.gbContent.Text = "Import Controls";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.rdoDataDepth);
            this.groupBox3.Controls.Add(this.rdoDataElev);
            this.groupBox3.Location = new System.Drawing.Point(12, 123);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(548, 45);
            this.groupBox3.TabIndex = 15;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Profile Data Type";
            // 
            // rdoDataDepth
            // 
            this.rdoDataDepth.AutoSize = true;
            this.rdoDataDepth.Location = new System.Drawing.Point(218, 19);
            this.rdoDataDepth.Name = "rdoDataDepth";
            this.rdoDataDepth.Size = new System.Drawing.Size(201, 17);
            this.rdoDataDepth.TabIndex = 12;
            this.rdoDataDepth.TabStop = true;
            this.rdoDataDepth.Text = "Depth to lowest point in cross-section";
            this.rdoDataDepth.UseVisualStyleBackColor = true;
            // 
            // rdoDataElev
            // 
            this.rdoDataElev.AutoSize = true;
            this.rdoDataElev.Location = new System.Drawing.Point(86, 19);
            this.rdoDataElev.Name = "rdoDataElev";
            this.rdoDataElev.Size = new System.Drawing.Size(69, 17);
            this.rdoDataElev.TabIndex = 11;
            this.rdoDataElev.TabStop = true;
            this.rdoDataElev.Text = "Elevation";
            this.rdoDataElev.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.rdoImportDataUnitUS);
            this.groupBox2.Controls.Add(this.rdoImportDataUnitSI);
            this.groupBox2.Location = new System.Drawing.Point(12, 70);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(548, 47);
            this.groupBox2.TabIndex = 14;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Profile Data Unit";
            // 
            // rdoImportDataUnitUS
            // 
            this.rdoImportDataUnitUS.AutoSize = true;
            this.rdoImportDataUnitUS.Location = new System.Drawing.Point(218, 19);
            this.rdoImportDataUnitUS.Name = "rdoImportDataUnitUS";
            this.rdoImportDataUnitUS.Size = new System.Drawing.Size(95, 17);
            this.rdoImportDataUnitUS.TabIndex = 9;
            this.rdoImportDataUnitUS.TabStop = true;
            this.rdoImportDataUnitUS.Text = "U.S. Unit (feet)";
            this.rdoImportDataUnitUS.UseVisualStyleBackColor = true;
            // 
            // rdoImportDataUnitSI
            // 
            this.rdoImportDataUnitSI.AutoSize = true;
            this.rdoImportDataUnitSI.Location = new System.Drawing.Point(86, 19);
            this.rdoImportDataUnitSI.Name = "rdoImportDataUnitSI";
            this.rdoImportDataUnitSI.Size = new System.Drawing.Size(103, 17);
            this.rdoImportDataUnitSI.TabIndex = 8;
            this.rdoImportDataUnitSI.TabStop = true;
            this.rdoImportDataUnitSI.Text = "S.I. Unit (meters)";
            this.rdoImportDataUnitSI.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.rdoImportDataCSV);
            this.groupBox1.Controls.Add(this.rdoImportDataExcel);
            this.groupBox1.Location = new System.Drawing.Point(12, 19);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(548, 45);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Import File Type";
            // 
            // rdoImportDataCSV
            // 
            this.rdoImportDataCSV.AutoSize = true;
            this.rdoImportDataCSV.Location = new System.Drawing.Point(218, 19);
            this.rdoImportDataCSV.Name = "rdoImportDataCSV";
            this.rdoImportDataCSV.Size = new System.Drawing.Size(134, 17);
            this.rdoImportDataCSV.TabIndex = 2;
            this.rdoImportDataCSV.TabStop = true;
            this.rdoImportDataCSV.Text = "Comma-delimited (CSV)";
            this.rdoImportDataCSV.UseVisualStyleBackColor = true;
            // 
            // rdoImportDataExcel
            // 
            this.rdoImportDataExcel.AutoSize = true;
            this.rdoImportDataExcel.Location = new System.Drawing.Point(86, 19);
            this.rdoImportDataExcel.Name = "rdoImportDataExcel";
            this.rdoImportDataExcel.Size = new System.Drawing.Size(118, 17);
            this.rdoImportDataExcel.TabIndex = 1;
            this.rdoImportDataExcel.TabStop = true;
            this.rdoImportDataExcel.Text = "Tab-delimited (TXT)";
            this.rdoImportDataExcel.UseVisualStyleBackColor = true;
            // 
            // btnImport
            // 
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImport.Location = new System.Drawing.Point(445, 219);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(116, 23);
            this.btnImport.TabIndex = 6;
            this.btnImport.Text = "Import Profile Data";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse.Location = new System.Drawing.Point(489, 187);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(72, 23);
            this.btnBrowse.TabIndex = 5;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // txtChProfileDatafile
            // 
            this.txtChProfileDatafile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtChProfileDatafile.Location = new System.Drawing.Point(149, 189);
            this.txtChProfileDatafile.Name = "txtChProfileDatafile";
            this.txtChProfileDatafile.Size = new System.Drawing.Size(331, 20);
            this.txtChProfileDatafile.TabIndex = 4;
            // 
            // lblChDatafile
            // 
            this.lblChDatafile.AutoSize = true;
            this.lblChDatafile.Location = new System.Drawing.Point(12, 192);
            this.lblChDatafile.Name = "lblChDatafile";
            this.lblChDatafile.Size = new System.Drawing.Size(131, 13);
            this.lblChDatafile.TabIndex = 3;
            this.lblChDatafile.Text = "Natural Channel Data File:";
            // 
            // frmImportProfile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(578, 366);
            this.Controls.Add(this.gbContent);
            this.Controls.Add(this.lblNotice);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmImportProfile";
            this.Text = "Import Natural Channel Profile";
            this.Load += new System.EventHandler(this.frmImportProfile_Load);
            this.gbContent.ResumeLayout(false);
            this.gbContent.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblNotice;
        private System.Windows.Forms.GroupBox gbContent;
        private System.Windows.Forms.RadioButton rdoImportDataCSV;
        private System.Windows.Forms.RadioButton rdoImportDataExcel;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox txtChProfileDatafile;
        private System.Windows.Forms.Label lblChDatafile;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.RadioButton rdoImportDataUnitUS;
        private System.Windows.Forms.RadioButton rdoImportDataUnitSI;
        private System.Windows.Forms.RadioButton rdoDataDepth;
        private System.Windows.Forms.RadioButton rdoDataElev;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}