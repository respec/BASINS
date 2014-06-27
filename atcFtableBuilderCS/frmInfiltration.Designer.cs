namespace atcFtableBuilder
{
    partial class frmInfiltration
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmInfiltration));
            this.gbStep1 = new System.Windows.Forms.GroupBox();
            this.cboSoilMenu = new System.Windows.Forms.ComboBox();
            this.gbStep2 = new System.Windows.Forms.GroupBox();
            this.frameGAmp = new System.Windows.Forms.Panel();
            this.btnCalcInfil = new System.Windows.Forms.Button();
            this.txtGAmpSoilDepth = new System.Windows.Forms.TextBox();
            this.lblGAmpSoilDepth = new System.Windows.Forms.Label();
            this.txtGAmpInitWater = new System.Windows.Forms.TextBox();
            this.lblGAmpInitWater = new System.Windows.Forms.Label();
            this.txtGAmpSatHydCond = new System.Windows.Forms.TextBox();
            this.lblGAmpSatHydCond = new System.Windows.Forms.Label();
            this.txtGAmpSuction = new System.Windows.Forms.TextBox();
            this.lblGAmpSuction = new System.Windows.Forms.Label();
            this.txtGAmpResWater = new System.Windows.Forms.TextBox();
            this.lblGAmpResWater = new System.Windows.Forms.Label();
            this.txtGAmpEffPorosity = new System.Windows.Forms.TextBox();
            this.lblGAmpEffPorosity = new System.Windows.Forms.Label();
            this.txtGAmpPorosity = new System.Windows.Forms.TextBox();
            this.lblGAmpPorosity = new System.Windows.Forms.Label();
            this.rdoInfilMethodGAmp = new System.Windows.Forms.RadioButton();
            this.rdoInfilMethodMaryland = new System.Windows.Forms.RadioButton();
            this.gbStep1.SuspendLayout();
            this.gbStep2.SuspendLayout();
            this.frameGAmp.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbStep1
            // 
            this.gbStep1.Controls.Add(this.cboSoilMenu);
            this.gbStep1.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbStep1.Location = new System.Drawing.Point(0, 0);
            this.gbStep1.Name = "gbStep1";
            this.gbStep1.Size = new System.Drawing.Size(337, 51);
            this.gbStep1.TabIndex = 0;
            this.gbStep1.TabStop = false;
            this.gbStep1.Text = "Step 1: Select Soil Type";
            // 
            // cboSoilMenu
            // 
            this.cboSoilMenu.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSoilMenu.FormattingEnabled = true;
            this.cboSoilMenu.Location = new System.Drawing.Point(13, 20);
            this.cboSoilMenu.Name = "cboSoilMenu";
            this.cboSoilMenu.Size = new System.Drawing.Size(121, 21);
            this.cboSoilMenu.TabIndex = 0;
            this.cboSoilMenu.SelectedIndexChanged += new System.EventHandler(this.cboSoilMenu_SelectedIndexChanged);
            // 
            // gbStep2
            // 
            this.gbStep2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbStep2.Controls.Add(this.frameGAmp);
            this.gbStep2.Controls.Add(this.rdoInfilMethodGAmp);
            this.gbStep2.Controls.Add(this.rdoInfilMethodMaryland);
            this.gbStep2.Location = new System.Drawing.Point(0, 57);
            this.gbStep2.Name = "gbStep2";
            this.gbStep2.Size = new System.Drawing.Size(337, 325);
            this.gbStep2.TabIndex = 1;
            this.gbStep2.TabStop = false;
            this.gbStep2.Text = "Step 2: Select Method";
            // 
            // frameGAmp
            // 
            this.frameGAmp.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.frameGAmp.Controls.Add(this.btnCalcInfil);
            this.frameGAmp.Controls.Add(this.txtGAmpSoilDepth);
            this.frameGAmp.Controls.Add(this.lblGAmpSoilDepth);
            this.frameGAmp.Controls.Add(this.txtGAmpInitWater);
            this.frameGAmp.Controls.Add(this.lblGAmpInitWater);
            this.frameGAmp.Controls.Add(this.txtGAmpSatHydCond);
            this.frameGAmp.Controls.Add(this.lblGAmpSatHydCond);
            this.frameGAmp.Controls.Add(this.txtGAmpSuction);
            this.frameGAmp.Controls.Add(this.lblGAmpSuction);
            this.frameGAmp.Controls.Add(this.txtGAmpResWater);
            this.frameGAmp.Controls.Add(this.lblGAmpResWater);
            this.frameGAmp.Controls.Add(this.txtGAmpEffPorosity);
            this.frameGAmp.Controls.Add(this.lblGAmpEffPorosity);
            this.frameGAmp.Controls.Add(this.txtGAmpPorosity);
            this.frameGAmp.Controls.Add(this.lblGAmpPorosity);
            this.frameGAmp.Location = new System.Drawing.Point(13, 44);
            this.frameGAmp.Name = "frameGAmp";
            this.frameGAmp.Size = new System.Drawing.Size(312, 268);
            this.frameGAmp.TabIndex = 2;
            // 
            // btnCalcInfil
            // 
            this.btnCalcInfil.Location = new System.Drawing.Point(162, 233);
            this.btnCalcInfil.Name = "btnCalcInfil";
            this.btnCalcInfil.Size = new System.Drawing.Size(141, 23);
            this.btnCalcInfil.TabIndex = 3;
            this.btnCalcInfil.Text = "Calculate Infiltration Rate";
            this.btnCalcInfil.UseVisualStyleBackColor = true;
            this.btnCalcInfil.Click += new System.EventHandler(this.btnCalcInfil_Click);
            // 
            // txtGAmpSoilDepth
            // 
            this.txtGAmpSoilDepth.Location = new System.Drawing.Point(187, 191);
            this.txtGAmpSoilDepth.Name = "txtGAmpSoilDepth";
            this.txtGAmpSoilDepth.Size = new System.Drawing.Size(100, 20);
            this.txtGAmpSoilDepth.TabIndex = 13;
            // 
            // lblGAmpSoilDepth
            // 
            this.lblGAmpSoilDepth.AutoSize = true;
            this.lblGAmpSoilDepth.Location = new System.Drawing.Point(66, 194);
            this.lblGAmpSoilDepth.Name = "lblGAmpSoilDepth";
            this.lblGAmpSoilDepth.Size = new System.Drawing.Size(115, 26);
            this.lblGAmpSoilDepth.TabIndex = 12;
            this.lblGAmpSoilDepth.Text = "Underlaying Soil Depth\r\n(m or ft)";
            // 
            // txtGAmpInitWater
            // 
            this.txtGAmpInitWater.Location = new System.Drawing.Point(187, 165);
            this.txtGAmpInitWater.Name = "txtGAmpInitWater";
            this.txtGAmpInitWater.Size = new System.Drawing.Size(100, 20);
            this.txtGAmpInitWater.TabIndex = 11;
            // 
            // lblGAmpInitWater
            // 
            this.lblGAmpInitWater.AutoSize = true;
            this.lblGAmpInitWater.Location = new System.Drawing.Point(78, 168);
            this.lblGAmpInitWater.Name = "lblGAmpInitWater";
            this.lblGAmpInitWater.Size = new System.Drawing.Size(103, 13);
            this.lblGAmpInitWater.TabIndex = 10;
            this.lblGAmpInitWater.Text = "Initial Water Content";
            // 
            // txtGAmpSatHydCond
            // 
            this.txtGAmpSatHydCond.Location = new System.Drawing.Point(187, 117);
            this.txtGAmpSatHydCond.Name = "txtGAmpSatHydCond";
            this.txtGAmpSatHydCond.Size = new System.Drawing.Size(100, 20);
            this.txtGAmpSatHydCond.TabIndex = 9;
            // 
            // lblGAmpSatHydCond
            // 
            this.lblGAmpSatHydCond.AutoSize = true;
            this.lblGAmpSatHydCond.Location = new System.Drawing.Point(20, 120);
            this.lblGAmpSatHydCond.Name = "lblGAmpSatHydCond";
            this.lblGAmpSatHydCond.Size = new System.Drawing.Size(161, 26);
            this.lblGAmpSatHydCond.TabIndex = 8;
            this.lblGAmpSatHydCond.Text = "Saturated Hydraulic Conductivity\r\n(in/hr or cm/hr)";
            // 
            // txtGAmpSuction
            // 
            this.txtGAmpSuction.Location = new System.Drawing.Point(187, 91);
            this.txtGAmpSuction.Name = "txtGAmpSuction";
            this.txtGAmpSuction.Size = new System.Drawing.Size(100, 20);
            this.txtGAmpSuction.TabIndex = 7;
            // 
            // lblGAmpSuction
            // 
            this.lblGAmpSuction.AutoSize = true;
            this.lblGAmpSuction.Location = new System.Drawing.Point(63, 94);
            this.lblGAmpSuction.Name = "lblGAmpSuction";
            this.lblGAmpSuction.Size = new System.Drawing.Size(118, 13);
            this.lblGAmpSuction.TabIndex = 6;
            this.lblGAmpSuction.Text = "Suction Head (in or cm)";
            // 
            // txtGAmpResWater
            // 
            this.txtGAmpResWater.Location = new System.Drawing.Point(187, 65);
            this.txtGAmpResWater.Name = "txtGAmpResWater";
            this.txtGAmpResWater.Size = new System.Drawing.Size(100, 20);
            this.txtGAmpResWater.TabIndex = 5;
            // 
            // lblGAmpResWater
            // 
            this.lblGAmpResWater.AutoSize = true;
            this.lblGAmpResWater.Location = new System.Drawing.Point(61, 68);
            this.lblGAmpResWater.Name = "lblGAmpResWater";
            this.lblGAmpResWater.Size = new System.Drawing.Size(120, 13);
            this.lblGAmpResWater.TabIndex = 4;
            this.lblGAmpResWater.Text = "Residual Water Content";
            // 
            // txtGAmpEffPorosity
            // 
            this.txtGAmpEffPorosity.Location = new System.Drawing.Point(187, 39);
            this.txtGAmpEffPorosity.Name = "txtGAmpEffPorosity";
            this.txtGAmpEffPorosity.Size = new System.Drawing.Size(100, 20);
            this.txtGAmpEffPorosity.TabIndex = 3;
            // 
            // lblGAmpEffPorosity
            // 
            this.lblGAmpEffPorosity.AutoSize = true;
            this.lblGAmpEffPorosity.Location = new System.Drawing.Point(92, 42);
            this.lblGAmpEffPorosity.Name = "lblGAmpEffPorosity";
            this.lblGAmpEffPorosity.Size = new System.Drawing.Size(89, 13);
            this.lblGAmpEffPorosity.TabIndex = 2;
            this.lblGAmpEffPorosity.Text = "Effective Porosity";
            // 
            // txtGAmpPorosity
            // 
            this.txtGAmpPorosity.Location = new System.Drawing.Point(187, 13);
            this.txtGAmpPorosity.Name = "txtGAmpPorosity";
            this.txtGAmpPorosity.Size = new System.Drawing.Size(100, 20);
            this.txtGAmpPorosity.TabIndex = 1;
            // 
            // lblGAmpPorosity
            // 
            this.lblGAmpPorosity.AutoSize = true;
            this.lblGAmpPorosity.Location = new System.Drawing.Point(137, 16);
            this.lblGAmpPorosity.Name = "lblGAmpPorosity";
            this.lblGAmpPorosity.Size = new System.Drawing.Size(44, 13);
            this.lblGAmpPorosity.TabIndex = 0;
            this.lblGAmpPorosity.Text = "Porosity";
            // 
            // rdoInfilMethodGAmp
            // 
            this.rdoInfilMethodGAmp.AutoSize = true;
            this.rdoInfilMethodGAmp.Location = new System.Drawing.Point(175, 20);
            this.rdoInfilMethodGAmp.Name = "rdoInfilMethodGAmp";
            this.rdoInfilMethodGAmp.Size = new System.Drawing.Size(141, 17);
            this.rdoInfilMethodGAmp.TabIndex = 1;
            this.rdoInfilMethodGAmp.TabStop = true;
            this.rdoInfilMethodGAmp.Text = "Green and Ampt Method";
            this.rdoInfilMethodGAmp.UseVisualStyleBackColor = true;
            // 
            // rdoInfilMethodMaryland
            // 
            this.rdoInfilMethodMaryland.AutoSize = true;
            this.rdoInfilMethodMaryland.Location = new System.Drawing.Point(13, 20);
            this.rdoInfilMethodMaryland.Name = "rdoInfilMethodMaryland";
            this.rdoInfilMethodMaryland.Size = new System.Drawing.Size(155, 17);
            this.rdoInfilMethodMaryland.TabIndex = 0;
            this.rdoInfilMethodMaryland.TabStop = true;
            this.rdoInfilMethodMaryland.Text = "Maryland Method (Look-up)";
            this.rdoInfilMethodMaryland.UseVisualStyleBackColor = true;
            // 
            // frmInfiltration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(337, 381);
            this.Controls.Add(this.gbStep2);
            this.Controls.Add(this.gbStep1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmInfiltration";
            this.Text = "Infiltration Calculator";
            this.Load += new System.EventHandler(this.frmInfiltration_Load);
            this.gbStep1.ResumeLayout(false);
            this.gbStep2.ResumeLayout(false);
            this.gbStep2.PerformLayout();
            this.frameGAmp.ResumeLayout(false);
            this.frameGAmp.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbStep1;
        private System.Windows.Forms.ComboBox cboSoilMenu;
        private System.Windows.Forms.GroupBox gbStep2;
        private System.Windows.Forms.RadioButton rdoInfilMethodGAmp;
        private System.Windows.Forms.RadioButton rdoInfilMethodMaryland;
        private System.Windows.Forms.Panel frameGAmp;
        private System.Windows.Forms.TextBox txtGAmpSatHydCond;
        private System.Windows.Forms.Label lblGAmpSatHydCond;
        private System.Windows.Forms.TextBox txtGAmpSuction;
        private System.Windows.Forms.Label lblGAmpSuction;
        private System.Windows.Forms.TextBox txtGAmpResWater;
        private System.Windows.Forms.Label lblGAmpResWater;
        private System.Windows.Forms.TextBox txtGAmpEffPorosity;
        private System.Windows.Forms.Label lblGAmpEffPorosity;
        private System.Windows.Forms.TextBox txtGAmpPorosity;
        private System.Windows.Forms.Label lblGAmpPorosity;
        private System.Windows.Forms.TextBox txtGAmpSoilDepth;
        private System.Windows.Forms.Label lblGAmpSoilDepth;
        private System.Windows.Forms.TextBox txtGAmpInitWater;
        private System.Windows.Forms.Label lblGAmpInitWater;
        private System.Windows.Forms.Button btnCalcInfil;
    }
}