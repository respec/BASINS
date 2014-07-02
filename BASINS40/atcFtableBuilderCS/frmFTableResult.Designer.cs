namespace atcFtableBuilder
{
    partial class frmFTableResult
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmFTableResult));
            this.frameCommands = new System.Windows.Forms.Panel();
            this.btnClearFTable = new System.Windows.Forms.Button();
            this.btnCopyToUCI = new System.Windows.Forms.Button();
            this.btnCopyToSpreadsheet = new System.Windows.Forms.Button();
            this.grdFtableResult = new atcControls.atcGrid();
            this.frameStatus = new System.Windows.Forms.Panel();
            this.lblMessage = new System.Windows.Forms.Label();
            this.frameCommands.SuspendLayout();
            this.frameStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // frameCommands
            // 
            this.frameCommands.Controls.Add(this.btnClearFTable);
            this.frameCommands.Controls.Add(this.btnCopyToUCI);
            this.frameCommands.Controls.Add(this.btnCopyToSpreadsheet);
            this.frameCommands.Dock = System.Windows.Forms.DockStyle.Top;
            this.frameCommands.Location = new System.Drawing.Point(0, 0);
            this.frameCommands.Name = "frameCommands";
            this.frameCommands.Size = new System.Drawing.Size(619, 33);
            this.frameCommands.TabIndex = 0;
            // 
            // btnClearFTable
            // 
            this.btnClearFTable.Location = new System.Drawing.Point(287, 4);
            this.btnClearFTable.Name = "btnClearFTable";
            this.btnClearFTable.Size = new System.Drawing.Size(135, 23);
            this.btnClearFTable.TabIndex = 2;
            this.btnClearFTable.Text = "Clear FTable";
            this.btnClearFTable.UseVisualStyleBackColor = true;
            // 
            // btnCopyToUCI
            // 
            this.btnCopyToUCI.Location = new System.Drawing.Point(146, 4);
            this.btnCopyToUCI.Name = "btnCopyToUCI";
            this.btnCopyToUCI.Size = new System.Drawing.Size(135, 23);
            this.btnCopyToUCI.TabIndex = 1;
            this.btnCopyToUCI.Text = "Copy to UCI";
            this.btnCopyToUCI.UseVisualStyleBackColor = true;
            // 
            // btnCopyToSpreadsheet
            // 
            this.btnCopyToSpreadsheet.Location = new System.Drawing.Point(4, 4);
            this.btnCopyToSpreadsheet.Name = "btnCopyToSpreadsheet";
            this.btnCopyToSpreadsheet.Size = new System.Drawing.Size(135, 23);
            this.btnCopyToSpreadsheet.TabIndex = 0;
            this.btnCopyToSpreadsheet.Text = "Copy to Spreadsheet";
            this.btnCopyToSpreadsheet.UseVisualStyleBackColor = true;
            // 
            // grdFtableResult
            // 
            this.grdFtableResult.AllowHorizontalScrolling = true;
            this.grdFtableResult.AllowNewValidValues = false;
            this.grdFtableResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdFtableResult.CellBackColor = System.Drawing.SystemColors.Window;
            this.grdFtableResult.Fixed3D = false;
            this.grdFtableResult.LineColor = System.Drawing.SystemColors.Control;
            this.grdFtableResult.LineWidth = 1F;
            this.grdFtableResult.Location = new System.Drawing.Point(0, 39);
            this.grdFtableResult.Name = "grdFtableResult";
            this.grdFtableResult.Size = new System.Drawing.Size(619, 356);
            this.grdFtableResult.Source = null;
            this.grdFtableResult.TabIndex = 1;
            // 
            // frameStatus
            // 
            this.frameStatus.Controls.Add(this.lblMessage);
            this.frameStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.frameStatus.Location = new System.Drawing.Point(0, 401);
            this.frameStatus.Name = "frameStatus";
            this.frameStatus.Size = new System.Drawing.Size(619, 36);
            this.frameStatus.TabIndex = 2;
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Location = new System.Drawing.Point(4, 4);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(33, 13);
            this.lblMessage.TabIndex = 0;
            this.lblMessage.Text = "Note:";
            // 
            // frmFTableResult
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(619, 437);
            this.Controls.Add(this.frameStatus);
            this.Controls.Add(this.grdFtableResult);
            this.Controls.Add(this.frameCommands);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmFTableResult";
            this.Text = "FTable Result";
            this.Load += new System.EventHandler(this.frmFTableResult_Load);
            this.frameCommands.ResumeLayout(false);
            this.frameStatus.ResumeLayout(false);
            this.frameStatus.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel frameCommands;
        private System.Windows.Forms.Button btnCopyToSpreadsheet;
        private System.Windows.Forms.Button btnCopyToUCI;
        private System.Windows.Forms.Button btnClearFTable;
        private atcControls.atcGrid grdFtableResult;
        private System.Windows.Forms.Panel frameStatus;
        private System.Windows.Forms.Label lblMessage;
    }
}