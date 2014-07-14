namespace HSPFftable
{
    partial class frmChoice
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmChoice));
            this.btnContinue = new System.Windows.Forms.Button();
            this.rdoToolGreen = new System.Windows.Forms.RadioButton();
            this.rdoToolGray = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // btnContinue
            // 
            this.btnContinue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnContinue.Location = new System.Drawing.Point(274, 117);
            this.btnContinue.Name = "btnContinue";
            this.btnContinue.Size = new System.Drawing.Size(75, 23);
            this.btnContinue.TabIndex = 2;
            this.btnContinue.Text = "Continue";
            this.btnContinue.UseVisualStyleBackColor = true;
            this.btnContinue.Click += new System.EventHandler(this.btnContinue_Click);
            // 
            // rdoToolGreen
            // 
            this.rdoToolGreen.AutoSize = true;
            this.rdoToolGreen.Location = new System.Drawing.Point(50, 31);
            this.rdoToolGreen.Name = "rdoToolGreen";
            this.rdoToolGreen.Size = new System.Drawing.Size(268, 17);
            this.rdoToolGreen.TabIndex = 3;
            this.rdoToolGreen.TabStop = true;
            this.rdoToolGreen.Text = "HSPF Low Impact Development (LID) Controls Tool";
            this.rdoToolGreen.UseVisualStyleBackColor = true;
            this.rdoToolGreen.CheckedChanged += new System.EventHandler(this.rdoToolGreen_CheckedChanged);
            // 
            // rdoToolGray
            // 
            this.rdoToolGray.AutoSize = true;
            this.rdoToolGray.Location = new System.Drawing.Point(50, 69);
            this.rdoToolGray.Name = "rdoToolGray";
            this.rdoToolGray.Size = new System.Drawing.Size(238, 17);
            this.rdoToolGray.TabIndex = 4;
            this.rdoToolGray.TabStop = true;
            this.rdoToolGray.Text = "HSPF Sewer and Open Channel Design Tool";
            this.rdoToolGray.UseVisualStyleBackColor = true;
            this.rdoToolGray.CheckedChanged += new System.EventHandler(this.rdoToolGray_CheckedChanged);
            // 
            // frmChoice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(361, 152);
            this.Controls.Add(this.rdoToolGray);
            this.Controls.Add(this.rdoToolGreen);
            this.Controls.Add(this.btnContinue);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmChoice";
            this.Text = "Choose a HSPF Tool";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnContinue;
        private System.Windows.Forms.RadioButton rdoToolGreen;
        private System.Windows.Forms.RadioButton rdoToolGray;
    }
}