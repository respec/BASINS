namespace ZedGraph
{
    partial class frmEdit
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
            this.components = new System.ComponentModel.Container();
            this.grpAxis = new System.Windows.Forms.GroupBox();
            this.txtAxisMajorGridColor = new System.Windows.Forms.TextBox();
            this.lblAxisMajorGridColor = new System.Windows.Forms.Label();
            this.lblAxisMajorGrid = new System.Windows.Forms.Label();
            this.chkAxisMajorGridVisible = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtAxisDisplayMaximum = new System.Windows.Forms.TextBox();
            this.txtAxisDisplayMinimum = new System.Windows.Forms.TextBox();
            this.lblAxisRange = new System.Windows.Forms.Label();
            this.lblAxisTo = new System.Windows.Forms.Label();
            this.txtAxisMaximum = new System.Windows.Forms.TextBox();
            this.txtAxisMinimum = new System.Windows.Forms.TextBox();
            this.lblAxisDataRange = new System.Windows.Forms.Label();
            this.txtAxisLabel = new System.Windows.Forms.TextBox();
            this.lblAxisLabel = new System.Windows.Forms.Label();
            this.lblAxisType = new System.Windows.Forms.Label();
            this.lblWhichAxis = new System.Windows.Forms.Label();
            this.cboAxisType = new System.Windows.Forms.ComboBox();
            this.cboWhichAxis = new System.Windows.Forms.ComboBox();
            this.grpCurve = new System.Windows.Forms.GroupBox();
            this.lblCurveWidth = new System.Windows.Forms.Label();
            this.txtCurveWidth = new System.Windows.Forms.TextBox();
            this.txtCurveColor = new System.Windows.Forms.TextBox();
            this.lblCurveColor = new System.Windows.Forms.Label();
            this.txtCurveLabel = new System.Windows.Forms.TextBox();
            this.lblCurveLabel = new System.Windows.Forms.Label();
            this.lblCurveYAxis = new System.Windows.Forms.Label();
            this.lblCurve = new System.Windows.Forms.Label();
            this.cboCurveAxis = new System.Windows.Forms.ComboBox();
            this.cboWhichCurve = new System.Windows.Forms.ComboBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnApply = new System.Windows.Forms.Button();
            this.txtAxisMinorGridColor = new System.Windows.Forms.TextBox();
            this.lblAxisMinorGrid = new System.Windows.Forms.Label();
            this.chkAxisMinorGridVisible = new System.Windows.Forms.CheckBox();
            this.lblAxisMinorGridColor = new System.Windows.Forms.Label();
            this.grpAxis.SuspendLayout();
            this.grpCurve.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpAxis
            // 
            this.grpAxis.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grpAxis.Controls.Add(this.lblAxisMinorGridColor);
            this.grpAxis.Controls.Add(this.txtAxisMinorGridColor);
            this.grpAxis.Controls.Add(this.lblAxisMinorGrid);
            this.grpAxis.Controls.Add(this.chkAxisMinorGridVisible);
            this.grpAxis.Controls.Add(this.txtAxisMajorGridColor);
            this.grpAxis.Controls.Add(this.lblAxisMajorGridColor);
            this.grpAxis.Controls.Add(this.lblAxisMajorGrid);
            this.grpAxis.Controls.Add(this.chkAxisMajorGridVisible);
            this.grpAxis.Controls.Add(this.label2);
            this.grpAxis.Controls.Add(this.txtAxisDisplayMaximum);
            this.grpAxis.Controls.Add(this.txtAxisDisplayMinimum);
            this.grpAxis.Controls.Add(this.lblAxisRange);
            this.grpAxis.Controls.Add(this.lblAxisTo);
            this.grpAxis.Controls.Add(this.txtAxisMaximum);
            this.grpAxis.Controls.Add(this.txtAxisMinimum);
            this.grpAxis.Controls.Add(this.lblAxisDataRange);
            this.grpAxis.Controls.Add(this.txtAxisLabel);
            this.grpAxis.Controls.Add(this.lblAxisLabel);
            this.grpAxis.Controls.Add(this.lblAxisType);
            this.grpAxis.Controls.Add(this.lblWhichAxis);
            this.grpAxis.Controls.Add(this.cboAxisType);
            this.grpAxis.Controls.Add(this.cboWhichAxis);
            this.grpAxis.Location = new System.Drawing.Point(6, 6);
            this.grpAxis.Name = "grpAxis";
            this.grpAxis.Size = new System.Drawing.Size(298, 165);
            this.grpAxis.TabIndex = 5;
            this.grpAxis.TabStop = false;
            this.grpAxis.Text = "Axis Properties";
            // 
            // txtAxisMajorGridColor
            // 
            this.txtAxisMajorGridColor.BackColor = System.Drawing.Color.LightGray;
            this.txtAxisMajorGridColor.Location = new System.Drawing.Point(201, 116);
            this.txtAxisMajorGridColor.Name = "txtAxisMajorGridColor";
            this.txtAxisMajorGridColor.Size = new System.Drawing.Size(91, 20);
            this.txtAxisMajorGridColor.TabIndex = 26;
            this.toolTip1.SetToolTip(this.txtAxisMajorGridColor, "Color of Major Grid");
            this.txtAxisMajorGridColor.Click += new System.EventHandler(this.txtAxisMajorGridColor_Click);
            // 
            // lblAxisMajorGridColor
            // 
            this.lblAxisMajorGridColor.AutoSize = true;
            this.lblAxisMajorGridColor.Location = new System.Drawing.Point(164, 119);
            this.lblAxisMajorGridColor.Name = "lblAxisMajorGridColor";
            this.lblAxisMajorGridColor.Size = new System.Drawing.Size(31, 13);
            this.lblAxisMajorGridColor.TabIndex = 25;
            this.lblAxisMajorGridColor.Text = "Color";
            this.toolTip1.SetToolTip(this.lblAxisMajorGridColor, "Range of data currently displayed");
            // 
            // lblAxisMajorGrid
            // 
            this.lblAxisMajorGrid.AutoSize = true;
            this.lblAxisMajorGrid.Location = new System.Drawing.Point(6, 119);
            this.lblAxisMajorGrid.Name = "lblAxisMajorGrid";
            this.lblAxisMajorGrid.Size = new System.Drawing.Size(55, 13);
            this.lblAxisMajorGrid.TabIndex = 24;
            this.lblAxisMajorGrid.Text = "Major Grid";
            this.toolTip1.SetToolTip(this.lblAxisMajorGrid, "Range of data currently displayed");
            // 
            // chkAxisMajorGridVisible
            // 
            this.chkAxisMajorGridVisible.AutoSize = true;
            this.chkAxisMajorGridVisible.Location = new System.Drawing.Point(81, 118);
            this.chkAxisMajorGridVisible.Name = "chkAxisMajorGridVisible";
            this.chkAxisMajorGridVisible.Size = new System.Drawing.Size(55, 17);
            this.chkAxisMajorGridVisible.TabIndex = 23;
            this.chkAxisMajorGridVisible.Text = "visible";
            this.chkAxisMajorGridVisible.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(179, 95);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(16, 13);
            this.label2.TabIndex = 22;
            this.label2.Text = "to";
            // 
            // txtAxisDisplayMaximum
            // 
            this.txtAxisDisplayMaximum.Location = new System.Drawing.Point(201, 92);
            this.txtAxisDisplayMaximum.Name = "txtAxisDisplayMaximum";
            this.txtAxisDisplayMaximum.Size = new System.Drawing.Size(91, 20);
            this.txtAxisDisplayMaximum.TabIndex = 21;
            this.toolTip1.SetToolTip(this.txtAxisDisplayMaximum, "Maximum value currently displayed on this axis");
            // 
            // txtAxisDisplayMinimum
            // 
            this.txtAxisDisplayMinimum.Location = new System.Drawing.Point(81, 92);
            this.txtAxisDisplayMinimum.Name = "txtAxisDisplayMinimum";
            this.txtAxisDisplayMinimum.Size = new System.Drawing.Size(91, 20);
            this.txtAxisDisplayMinimum.TabIndex = 20;
            this.toolTip1.SetToolTip(this.txtAxisDisplayMinimum, "Minimum value currently displayed on this axis");
            // 
            // lblAxisRange
            // 
            this.lblAxisRange.AutoSize = true;
            this.lblAxisRange.Location = new System.Drawing.Point(6, 95);
            this.lblAxisRange.Name = "lblAxisRange";
            this.lblAxisRange.Size = new System.Drawing.Size(69, 13);
            this.lblAxisRange.TabIndex = 19;
            this.lblAxisRange.Text = "Zoom Range";
            this.toolTip1.SetToolTip(this.lblAxisRange, "Range of data currently displayed");
            // 
            // lblAxisTo
            // 
            this.lblAxisTo.AutoSize = true;
            this.lblAxisTo.Location = new System.Drawing.Point(179, 75);
            this.lblAxisTo.Name = "lblAxisTo";
            this.lblAxisTo.Size = new System.Drawing.Size(16, 13);
            this.lblAxisTo.TabIndex = 18;
            this.lblAxisTo.Text = "to";
            // 
            // txtAxisMaximum
            // 
            this.txtAxisMaximum.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.txtAxisMaximum.Enabled = false;
            this.txtAxisMaximum.Location = new System.Drawing.Point(201, 72);
            this.txtAxisMaximum.Name = "txtAxisMaximum";
            this.txtAxisMaximum.Size = new System.Drawing.Size(91, 20);
            this.txtAxisMaximum.TabIndex = 17;
            this.toolTip1.SetToolTip(this.txtAxisMaximum, "Maximum data value in any dataset using this axis");
            // 
            // txtAxisMinimum
            // 
            this.txtAxisMinimum.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.txtAxisMinimum.Enabled = false;
            this.txtAxisMinimum.Location = new System.Drawing.Point(81, 72);
            this.txtAxisMinimum.Name = "txtAxisMinimum";
            this.txtAxisMinimum.Size = new System.Drawing.Size(91, 20);
            this.txtAxisMinimum.TabIndex = 16;
            this.toolTip1.SetToolTip(this.txtAxisMinimum, "Minimum data value in any dataset using this axis");
            // 
            // lblAxisDataRange
            // 
            this.lblAxisDataRange.AutoSize = true;
            this.lblAxisDataRange.Location = new System.Drawing.Point(6, 75);
            this.lblAxisDataRange.Name = "lblAxisDataRange";
            this.lblAxisDataRange.Size = new System.Drawing.Size(65, 13);
            this.lblAxisDataRange.TabIndex = 15;
            this.lblAxisDataRange.Text = "Data Range";
            this.toolTip1.SetToolTip(this.lblAxisDataRange, "Total range of all data using this axis");
            // 
            // txtAxisLabel
            // 
            this.txtAxisLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAxisLabel.Location = new System.Drawing.Point(81, 46);
            this.txtAxisLabel.Name = "txtAxisLabel";
            this.txtAxisLabel.Size = new System.Drawing.Size(211, 20);
            this.txtAxisLabel.TabIndex = 14;
            // 
            // lblAxisLabel
            // 
            this.lblAxisLabel.AutoSize = true;
            this.lblAxisLabel.Location = new System.Drawing.Point(6, 49);
            this.lblAxisLabel.Name = "lblAxisLabel";
            this.lblAxisLabel.Size = new System.Drawing.Size(27, 13);
            this.lblAxisLabel.TabIndex = 13;
            this.lblAxisLabel.Text = "Title";
            // 
            // lblAxisType
            // 
            this.lblAxisType.AutoSize = true;
            this.lblAxisType.Location = new System.Drawing.Point(164, 22);
            this.lblAxisType.Name = "lblAxisType";
            this.lblAxisType.Size = new System.Drawing.Size(31, 13);
            this.lblAxisType.TabIndex = 12;
            this.lblAxisType.Text = "Type";
            // 
            // lblWhichAxis
            // 
            this.lblWhichAxis.AutoSize = true;
            this.lblWhichAxis.Location = new System.Drawing.Point(6, 22);
            this.lblWhichAxis.Name = "lblWhichAxis";
            this.lblWhichAxis.Size = new System.Drawing.Size(26, 13);
            this.lblWhichAxis.TabIndex = 11;
            this.lblWhichAxis.Text = "Axis";
            // 
            // cboAxisType
            // 
            this.cboAxisType.FormattingEnabled = true;
            this.cboAxisType.Items.AddRange(new object[] {
            "Time",
            "Arithmetic",
            "Logarithmic",
            "Probability"});
            this.cboAxisType.Location = new System.Drawing.Point(201, 19);
            this.cboAxisType.Name = "cboAxisType";
            this.cboAxisType.Size = new System.Drawing.Size(91, 21);
            this.cboAxisType.TabIndex = 10;
            this.cboAxisType.Text = "Logarithmic";
            // 
            // cboWhichAxis
            // 
            this.cboWhichAxis.FormattingEnabled = true;
            this.cboWhichAxis.Items.AddRange(new object[] {
            "X Bottom",
            "Y Left",
            "Y Right",
            "Auxiliary"});
            this.cboWhichAxis.Location = new System.Drawing.Point(81, 19);
            this.cboWhichAxis.Name = "cboWhichAxis";
            this.cboWhichAxis.Size = new System.Drawing.Size(77, 21);
            this.cboWhichAxis.TabIndex = 9;
            this.cboWhichAxis.Text = "X Bottom";
            this.toolTip1.SetToolTip(this.cboWhichAxis, "Select which axis to edit");
            this.cboWhichAxis.SelectedIndexChanged += new System.EventHandler(this.cboWhichAxis_SelectedIndexChanged);
            // 
            // grpCurve
            // 
            this.grpCurve.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grpCurve.Controls.Add(this.lblCurveWidth);
            this.grpCurve.Controls.Add(this.txtCurveWidth);
            this.grpCurve.Controls.Add(this.txtCurveColor);
            this.grpCurve.Controls.Add(this.lblCurveColor);
            this.grpCurve.Controls.Add(this.txtCurveLabel);
            this.grpCurve.Controls.Add(this.lblCurveLabel);
            this.grpCurve.Controls.Add(this.lblCurveYAxis);
            this.grpCurve.Controls.Add(this.lblCurve);
            this.grpCurve.Controls.Add(this.cboCurveAxis);
            this.grpCurve.Controls.Add(this.cboWhichCurve);
            this.grpCurve.Location = new System.Drawing.Point(6, 177);
            this.grpCurve.Name = "grpCurve";
            this.grpCurve.Size = new System.Drawing.Size(298, 126);
            this.grpCurve.TabIndex = 6;
            this.grpCurve.TabStop = false;
            this.grpCurve.Text = "Curve Properties";
            // 
            // lblCurveWidth
            // 
            this.lblCurveWidth.AutoSize = true;
            this.lblCurveWidth.Location = new System.Drawing.Point(6, 102);
            this.lblCurveWidth.Name = "lblCurveWidth";
            this.lblCurveWidth.Size = new System.Drawing.Size(35, 13);
            this.lblCurveWidth.TabIndex = 30;
            this.lblCurveWidth.Text = "Width";
            // 
            // txtCurveWidth
            // 
            this.txtCurveWidth.Location = new System.Drawing.Point(81, 99);
            this.txtCurveWidth.Name = "txtCurveWidth";
            this.txtCurveWidth.Size = new System.Drawing.Size(55, 20);
            this.txtCurveWidth.TabIndex = 29;
            this.toolTip1.SetToolTip(this.txtCurveWidth, "Width of curve");
            // 
            // txtCurveColor
            // 
            this.txtCurveColor.BackColor = System.Drawing.Color.LightGray;
            this.txtCurveColor.Location = new System.Drawing.Point(201, 99);
            this.txtCurveColor.Name = "txtCurveColor";
            this.txtCurveColor.Size = new System.Drawing.Size(91, 20);
            this.txtCurveColor.TabIndex = 28;
            this.toolTip1.SetToolTip(this.txtCurveColor, "Color of curve");
            this.txtCurveColor.Click += new System.EventHandler(this.txtCurveColor_Click);
            // 
            // lblCurveColor
            // 
            this.lblCurveColor.AutoSize = true;
            this.lblCurveColor.Location = new System.Drawing.Point(164, 102);
            this.lblCurveColor.Name = "lblCurveColor";
            this.lblCurveColor.Size = new System.Drawing.Size(31, 13);
            this.lblCurveColor.TabIndex = 27;
            this.lblCurveColor.Text = "Color";
            this.toolTip1.SetToolTip(this.lblCurveColor, "Range of data currently displayed");
            // 
            // txtCurveLabel
            // 
            this.txtCurveLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCurveLabel.Location = new System.Drawing.Point(81, 46);
            this.txtCurveLabel.Name = "txtCurveLabel";
            this.txtCurveLabel.Size = new System.Drawing.Size(211, 20);
            this.txtCurveLabel.TabIndex = 16;
            // 
            // lblCurveLabel
            // 
            this.lblCurveLabel.AutoSize = true;
            this.lblCurveLabel.Location = new System.Drawing.Point(6, 49);
            this.lblCurveLabel.Name = "lblCurveLabel";
            this.lblCurveLabel.Size = new System.Drawing.Size(33, 13);
            this.lblCurveLabel.TabIndex = 15;
            this.lblCurveLabel.Text = "Label";
            // 
            // lblCurveYAxis
            // 
            this.lblCurveYAxis.AutoSize = true;
            this.lblCurveYAxis.Location = new System.Drawing.Point(6, 75);
            this.lblCurveYAxis.Name = "lblCurveYAxis";
            this.lblCurveYAxis.Size = new System.Drawing.Size(36, 13);
            this.lblCurveYAxis.TabIndex = 14;
            this.lblCurveYAxis.Text = "Y Axis";
            // 
            // lblCurve
            // 
            this.lblCurve.AutoSize = true;
            this.lblCurve.Location = new System.Drawing.Point(6, 22);
            this.lblCurve.Name = "lblCurve";
            this.lblCurve.Size = new System.Drawing.Size(35, 13);
            this.lblCurve.TabIndex = 12;
            this.lblCurve.Text = "Curve";
            // 
            // cboCurveAxis
            // 
            this.cboCurveAxis.FormattingEnabled = true;
            this.cboCurveAxis.Items.AddRange(new object[] {
            "Left",
            "Right"});
            this.cboCurveAxis.Location = new System.Drawing.Point(81, 72);
            this.cboCurveAxis.Name = "cboCurveAxis";
            this.cboCurveAxis.Size = new System.Drawing.Size(94, 21);
            this.cboCurveAxis.TabIndex = 13;
            this.cboCurveAxis.Text = "Left";
            this.toolTip1.SetToolTip(this.cboCurveAxis, "Which Y axis this curve is measured on");
            // 
            // cboWhichCurve
            // 
            this.cboWhichCurve.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cboWhichCurve.FormattingEnabled = true;
            this.cboWhichCurve.Location = new System.Drawing.Point(81, 19);
            this.cboWhichCurve.Name = "cboWhichCurve";
            this.cboWhichCurve.Size = new System.Drawing.Size(211, 21);
            this.cboWhichCurve.TabIndex = 0;
            this.toolTip1.SetToolTip(this.cboWhichCurve, "Select which curve to edit");
            this.cboWhichCurve.SelectedIndexChanged += new System.EventHandler(this.cboWhichDataset_SelectedIndexChanged);
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(6, 309);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(75, 23);
            this.btnApply.TabIndex = 7;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // txtAxisMinorGridColor
            // 
            this.txtAxisMinorGridColor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtAxisMinorGridColor.Location = new System.Drawing.Point(201, 138);
            this.txtAxisMinorGridColor.Name = "txtAxisMinorGridColor";
            this.txtAxisMinorGridColor.Size = new System.Drawing.Size(91, 20);
            this.txtAxisMinorGridColor.TabIndex = 29;
            this.toolTip1.SetToolTip(this.txtAxisMinorGridColor, "Color of Major Grid");
            this.txtAxisMinorGridColor.Click += new System.EventHandler(this.txtAxisMinorGridColor_Click);
            // 
            // lblAxisMinorGrid
            // 
            this.lblAxisMinorGrid.AutoSize = true;
            this.lblAxisMinorGrid.Location = new System.Drawing.Point(6, 141);
            this.lblAxisMinorGrid.Name = "lblAxisMinorGrid";
            this.lblAxisMinorGrid.Size = new System.Drawing.Size(55, 13);
            this.lblAxisMinorGrid.TabIndex = 28;
            this.lblAxisMinorGrid.Text = "Minor Grid";
            this.toolTip1.SetToolTip(this.lblAxisMinorGrid, "Range of data currently displayed");
            // 
            // chkAxisMinorGridVisible
            // 
            this.chkAxisMinorGridVisible.AutoSize = true;
            this.chkAxisMinorGridVisible.Location = new System.Drawing.Point(81, 140);
            this.chkAxisMinorGridVisible.Name = "chkAxisMinorGridVisible";
            this.chkAxisMinorGridVisible.Size = new System.Drawing.Size(55, 17);
            this.chkAxisMinorGridVisible.TabIndex = 27;
            this.chkAxisMinorGridVisible.Text = "visible";
            this.chkAxisMinorGridVisible.UseVisualStyleBackColor = true;
            // 
            // lblAxisMinorGridColor
            // 
            this.lblAxisMinorGridColor.AutoSize = true;
            this.lblAxisMinorGridColor.Location = new System.Drawing.Point(164, 141);
            this.lblAxisMinorGridColor.Name = "lblAxisMinorGridColor";
            this.lblAxisMinorGridColor.Size = new System.Drawing.Size(31, 13);
            this.lblAxisMinorGridColor.TabIndex = 30;
            this.lblAxisMinorGridColor.Text = "Color";
            this.toolTip1.SetToolTip(this.lblAxisMinorGridColor, "Range of data currently displayed");
            // 
            // frmEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(310, 339);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.grpCurve);
            this.Controls.Add(this.grpAxis);
            this.Name = "frmEdit";
            this.Text = "Edit Graph";
            this.grpAxis.ResumeLayout(false);
            this.grpAxis.PerformLayout();
            this.grpCurve.ResumeLayout(false);
            this.grpCurve.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpAxis;
        private System.Windows.Forms.ComboBox cboWhichAxis;
        private System.Windows.Forms.Label lblAxisLabel;
        private System.Windows.Forms.Label lblAxisType;
        private System.Windows.Forms.Label lblWhichAxis;
        private System.Windows.Forms.ComboBox cboAxisType;
        private System.Windows.Forms.TextBox txtAxisMinimum;
        private System.Windows.Forms.Label lblAxisDataRange;
        private System.Windows.Forms.TextBox txtAxisLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtAxisDisplayMaximum;
        private System.Windows.Forms.TextBox txtAxisDisplayMinimum;
        private System.Windows.Forms.Label lblAxisRange;
        private System.Windows.Forms.Label lblAxisTo;
        private System.Windows.Forms.TextBox txtAxisMaximum;
        private System.Windows.Forms.GroupBox grpCurve;
        private System.Windows.Forms.Label lblCurve;
        private System.Windows.Forms.ComboBox cboWhichCurve;
        private System.Windows.Forms.Label lblCurveYAxis;
        private System.Windows.Forms.ComboBox cboCurveAxis;
        private System.Windows.Forms.TextBox txtCurveLabel;
        private System.Windows.Forms.Label lblCurveLabel;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TextBox txtAxisMajorGridColor;
        private System.Windows.Forms.Label lblAxisMajorGridColor;
        private System.Windows.Forms.Label lblAxisMajorGrid;
        private System.Windows.Forms.CheckBox chkAxisMajorGridVisible;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.TextBox txtCurveColor;
        private System.Windows.Forms.Label lblCurveColor;
        private System.Windows.Forms.Label lblCurveWidth;
        private System.Windows.Forms.TextBox txtCurveWidth;
        private System.Windows.Forms.Label lblAxisMinorGridColor;
        private System.Windows.Forms.TextBox txtAxisMinorGridColor;
        private System.Windows.Forms.Label lblAxisMinorGrid;
        private System.Windows.Forms.CheckBox chkAxisMinorGridVisible;

    }
}