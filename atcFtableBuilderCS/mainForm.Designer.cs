namespace atcFtableBuilder
{
    partial class mainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(mainForm));
            this.frameTop = new System.Windows.Forms.Panel();
            this.lblBMPMsg = new System.Windows.Forms.Label();
            this.cboBMPTypes = new System.Windows.Forms.ComboBox();
            this.lblBMPList = new System.Windows.Forms.Label();
            this.rdoUnitSI = new System.Windows.Forms.RadioButton();
            this.rdoUnitUS = new System.Windows.Forms.RadioButton();
            this.lblUnit = new System.Windows.Forms.Label();
            this.btnShowOptControls = new System.Windows.Forms.Button();
            this.btnShowInfilCalc = new System.Windows.Forms.Button();
            this.btnCalcFtable = new System.Windows.Forms.Button();
            this.btnHelp = new System.Windows.Forms.Button();
            this.gbChannel = new System.Windows.Forms.GroupBox();
            this.rdoChNaturalFP = new System.Windows.Forms.RadioButton();
            this.bmpSketch1 = new atcFtableBuilder.BMPSketch();
            this.rdoChNatural = new System.Windows.Forms.RadioButton();
            this.rdoChPara = new System.Windows.Forms.RadioButton();
            this.rdoChTrape = new System.Windows.Forms.RadioButton();
            this.rdoChTri = new System.Windows.Forms.RadioButton();
            this.rdoChRect = new System.Windows.Forms.RadioButton();
            this.rdoChCirc = new System.Windows.Forms.RadioButton();
            this.gbInputData = new System.Windows.Forms.GroupBox();
            this.frameNaturalChFP = new System.Windows.Forms.Panel();
            this.txtGeomNFP_LOBX = new System.Windows.Forms.TextBox();
            this.txtGeomNFP_ROBX = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtGeomNFP_ROBMannN = new System.Windows.Forms.TextBox();
            this.txtGeomNFP_ChMannN = new System.Windows.Forms.TextBox();
            this.txtGeomNFP_LOBMannN = new System.Windows.Forms.TextBox();
            this.txtGeomNFP_ROBLength = new System.Windows.Forms.TextBox();
            this.txtGeomNFP_ChLength = new System.Windows.Forms.TextBox();
            this.txtGeomNFP_LOBLength = new System.Windows.Forms.TextBox();
            this.txtGeomNFP_ChLSlope = new System.Windows.Forms.TextBox();
            this.lblNFP_ChSlope = new System.Windows.Forms.Label();
            this.frameNaturalXsectGrid = new System.Windows.Forms.Panel();
            this.btnUndoClear = new System.Windows.Forms.Button();
            this.btnClearProfile = new System.Windows.Forms.Button();
            this.btnImportChProfile = new System.Windows.Forms.Button();
            this.grdChProfile = new atcControls.atcGrid();
            this.gbInputInfil = new System.Windows.Forms.GroupBox();
            this.txtInfilDrainTime = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.txtInfilDepth = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.txtInfilRate = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.txtBackfillPore = new System.Windows.Forms.TextBox();
            this.lblBackfillPore = new System.Windows.Forms.Label();
            this.txtBackfillDepth = new System.Windows.Forms.TextBox();
            this.lblBackfillDepth = new System.Windows.Forms.Label();
            this.chkBackfill = new System.Windows.Forms.CheckBox();
            this.gbInputGeometry = new System.Windows.Forms.GroupBox();
            this.txtGeomDepth = new System.Windows.Forms.TextBox();
            this.txtGeomWidth = new System.Windows.Forms.TextBox();
            this.txtGeomSideSlope = new System.Windows.Forms.TextBox();
            this.txtGeomTopWidth = new System.Windows.Forms.TextBox();
            this.txtGeomMaxDepth = new System.Windows.Forms.TextBox();
            this.lblGeomDepth = new System.Windows.Forms.Label();
            this.lblGeomWidth = new System.Windows.Forms.Label();
            this.lblGeomSideSlope = new System.Windows.Forms.Label();
            this.lblGeomTopWidth = new System.Windows.Forms.Label();
            this.lblGeomMaxDepth = new System.Windows.Forms.Label();
            this.txtGeomDiam = new System.Windows.Forms.TextBox();
            this.lblGeomDiam = new System.Windows.Forms.Label();
            this.txtGeomHInc = new System.Windows.Forms.TextBox();
            this.txtGeomMannN = new System.Windows.Forms.TextBox();
            this.txtGeomLSlope = new System.Windows.Forms.TextBox();
            this.txtGeomLength = new System.Windows.Forms.TextBox();
            this.lblGeomHInc = new System.Windows.Forms.Label();
            this.lblGeomLSlope = new System.Windows.Forms.Label();
            this.lblGeomMannN = new System.Windows.Forms.Label();
            this.lblGeomLength = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.frameTop.SuspendLayout();
            this.gbChannel.SuspendLayout();
            this.gbInputData.SuspendLayout();
            this.frameNaturalChFP.SuspendLayout();
            this.frameNaturalXsectGrid.SuspendLayout();
            this.gbInputInfil.SuspendLayout();
            this.gbInputGeometry.SuspendLayout();
            this.SuspendLayout();
            // 
            // frameTop
            // 
            this.frameTop.Controls.Add(this.lblBMPMsg);
            this.frameTop.Controls.Add(this.cboBMPTypes);
            this.frameTop.Controls.Add(this.lblBMPList);
            this.frameTop.Controls.Add(this.rdoUnitSI);
            this.frameTop.Controls.Add(this.rdoUnitUS);
            this.frameTop.Controls.Add(this.lblUnit);
            this.frameTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.frameTop.Location = new System.Drawing.Point(0, 0);
            this.frameTop.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.frameTop.Name = "frameTop";
            this.frameTop.Size = new System.Drawing.Size(1057, 50);
            this.frameTop.TabIndex = 1;
            // 
            // lblBMPMsg
            // 
            this.lblBMPMsg.AutoSize = true;
            this.lblBMPMsg.Location = new System.Drawing.Point(716, 14);
            this.lblBMPMsg.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblBMPMsg.Name = "lblBMPMsg";
            this.lblBMPMsg.Size = new System.Drawing.Size(307, 34);
            this.lblBMPMsg.TabIndex = 5;
            this.lblBMPMsg.Text = "A suggested channel geometry will be selected.\r\nYou can change this shape.";
            // 
            // cboBMPTypes
            // 
            this.cboBMPTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboBMPTypes.FormattingEnabled = true;
            this.cboBMPTypes.Location = new System.Drawing.Point(512, 12);
            this.cboBMPTypes.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cboBMPTypes.Name = "cboBMPTypes";
            this.cboBMPTypes.Size = new System.Drawing.Size(195, 24);
            this.cboBMPTypes.TabIndex = 4;
            this.cboBMPTypes.SelectedIndexChanged += new System.EventHandler(this.cboBMPTypes_SelectedIndexChanged);
            // 
            // lblBMPList
            // 
            this.lblBMPList.AutoSize = true;
            this.lblBMPList.Location = new System.Drawing.Point(365, 16);
            this.lblBMPList.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblBMPList.Name = "lblBMPList";
            this.lblBMPList.Size = new System.Drawing.Size(136, 17);
            this.lblBMPList.TabIndex = 3;
            this.lblBMPList.Text = "Choose a BMP type:";
            // 
            // rdoUnitSI
            // 
            this.rdoUnitSI.AutoSize = true;
            this.rdoUnitSI.Location = new System.Drawing.Point(236, 14);
            this.rdoUnitSI.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rdoUnitSI.Name = "rdoUnitSI";
            this.rdoUnitSI.Size = new System.Drawing.Size(81, 21);
            this.rdoUnitSI.TabIndex = 2;
            this.rdoUnitSI.Text = "S.I.Units";
            this.rdoUnitSI.UseVisualStyleBackColor = true;
            // 
            // rdoUnitUS
            // 
            this.rdoUnitUS.AutoSize = true;
            this.rdoUnitUS.Checked = true;
            this.rdoUnitUS.Location = new System.Drawing.Point(115, 14);
            this.rdoUnitUS.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rdoUnitUS.Name = "rdoUnitUS";
            this.rdoUnitUS.Size = new System.Drawing.Size(88, 21);
            this.rdoUnitUS.TabIndex = 1;
            this.rdoUnitUS.TabStop = true;
            this.rdoUnitUS.Text = "U.S.Units";
            this.rdoUnitUS.UseVisualStyleBackColor = true;
            // 
            // lblUnit
            // 
            this.lblUnit.AutoSize = true;
            this.lblUnit.Location = new System.Drawing.Point(17, 16);
            this.lblUnit.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUnit.Name = "lblUnit";
            this.lblUnit.Size = new System.Drawing.Size(87, 17);
            this.lblUnit.TabIndex = 0;
            this.lblUnit.Text = "Specify Unit:";
            // 
            // btnShowOptControls
            // 
            this.btnShowOptControls.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnShowOptControls.Location = new System.Drawing.Point(16, 369);
            this.btnShowOptControls.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnShowOptControls.Name = "btnShowOptControls";
            this.btnShowOptControls.Size = new System.Drawing.Size(272, 28);
            this.btnShowOptControls.TabIndex = 2;
            this.btnShowOptControls.Text = "Show optional control devices";
            this.btnShowOptControls.UseVisualStyleBackColor = true;
            this.btnShowOptControls.Click += new System.EventHandler(this.btnShowOptControls_Click);
            // 
            // btnShowInfilCalc
            // 
            this.btnShowInfilCalc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnShowInfilCalc.Location = new System.Drawing.Point(334, 369);
            this.btnShowInfilCalc.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnShowInfilCalc.Name = "btnShowInfilCalc";
            this.btnShowInfilCalc.Size = new System.Drawing.Size(201, 28);
            this.btnShowInfilCalc.TabIndex = 1;
            this.btnShowInfilCalc.Text = "Show infiltration calculator";
            this.btnShowInfilCalc.UseVisualStyleBackColor = true;
            this.btnShowInfilCalc.Click += new System.EventHandler(this.btnShowInfilCalc_Click);
            // 
            // btnCalcFtable
            // 
            this.btnCalcFtable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCalcFtable.Location = new System.Drawing.Point(861, 619);
            this.btnCalcFtable.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCalcFtable.Name = "btnCalcFtable";
            this.btnCalcFtable.Size = new System.Drawing.Size(180, 28);
            this.btnCalcFtable.TabIndex = 3;
            this.btnCalcFtable.Text = "Calculate FTable";
            this.btnCalcFtable.UseVisualStyleBackColor = true;
            this.btnCalcFtable.Click += new System.EventHandler(this.btnCalcFtable_Click);
            // 
            // btnHelp
            // 
            this.btnHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnHelp.Location = new System.Drawing.Point(753, 619);
            this.btnHelp.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(100, 28);
            this.btnHelp.TabIndex = 4;
            this.btnHelp.Text = "Help";
            this.btnHelp.UseVisualStyleBackColor = true;
            // 
            // gbChannel
            // 
            this.gbChannel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbChannel.Controls.Add(this.rdoChNaturalFP);
            this.gbChannel.Controls.Add(this.bmpSketch1);
            this.gbChannel.Controls.Add(this.rdoChNatural);
            this.gbChannel.Controls.Add(this.rdoChPara);
            this.gbChannel.Controls.Add(this.rdoChTrape);
            this.gbChannel.Controls.Add(this.rdoChTri);
            this.gbChannel.Controls.Add(this.rdoChRect);
            this.gbChannel.Controls.Add(this.rdoChCirc);
            this.gbChannel.Location = new System.Drawing.Point(0, 58);
            this.gbChannel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gbChannel.Name = "gbChannel";
            this.gbChannel.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gbChannel.Size = new System.Drawing.Size(1057, 150);
            this.gbChannel.TabIndex = 3;
            this.gbChannel.TabStop = false;
            this.gbChannel.Text = "Choose Channel Type";
            // 
            // rdoChNaturalFP
            // 
            this.rdoChNaturalFP.AutoSize = true;
            this.rdoChNaturalFP.Location = new System.Drawing.Point(783, 23);
            this.rdoChNaturalFP.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rdoChNaturalFP.Name = "rdoChNaturalFP";
            this.rdoChNaturalFP.Size = new System.Drawing.Size(201, 21);
            this.rdoChNaturalFP.TabIndex = 7;
            this.rdoChNaturalFP.TabStop = true;
            this.rdoChNaturalFP.Text = "NATURAL (with flood plain)";
            this.rdoChNaturalFP.UseVisualStyleBackColor = true;
            // 
            // bmpSketch1
            // 
            this.bmpSketch1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bmpSketch1.Location = new System.Drawing.Point(4, 51);
            this.bmpSketch1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.bmpSketch1.Name = "bmpSketch1";
            this.bmpSketch1.Size = new System.Drawing.Size(1049, 95);
            this.bmpSketch1.TabIndex = 6;
            this.bmpSketch1.MouseHover += new System.EventHandler(this.bmpSketch1_MouseHover);
            // 
            // rdoChNatural
            // 
            this.rdoChNatural.AutoSize = true;
            this.rdoChNatural.Location = new System.Drawing.Point(672, 23);
            this.rdoChNatural.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rdoChNatural.Name = "rdoChNatural";
            this.rdoChNatural.Size = new System.Drawing.Size(94, 21);
            this.rdoChNatural.TabIndex = 5;
            this.rdoChNatural.TabStop = true;
            this.rdoChNatural.Text = "NATURAL";
            this.rdoChNatural.UseVisualStyleBackColor = true;
            // 
            // rdoChPara
            // 
            this.rdoChPara.AutoSize = true;
            this.rdoChPara.Location = new System.Drawing.Point(551, 23);
            this.rdoChPara.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rdoChPara.Name = "rdoChPara";
            this.rdoChPara.Size = new System.Drawing.Size(106, 21);
            this.rdoChPara.TabIndex = 4;
            this.rdoChPara.TabStop = true;
            this.rdoChPara.Text = "PARABOLIC";
            this.rdoChPara.UseVisualStyleBackColor = true;
            // 
            // rdoChTrape
            // 
            this.rdoChTrape.AutoSize = true;
            this.rdoChTrape.Location = new System.Drawing.Point(409, 23);
            this.rdoChTrape.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rdoChTrape.Name = "rdoChTrape";
            this.rdoChTrape.Size = new System.Drawing.Size(125, 21);
            this.rdoChTrape.TabIndex = 3;
            this.rdoChTrape.TabStop = true;
            this.rdoChTrape.Text = "TRAPEZOIDAL";
            this.rdoChTrape.UseVisualStyleBackColor = true;
            // 
            // rdoChTri
            // 
            this.rdoChTri.AutoSize = true;
            this.rdoChTri.Location = new System.Drawing.Point(275, 23);
            this.rdoChTri.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rdoChTri.Name = "rdoChTri";
            this.rdoChTri.Size = new System.Drawing.Size(118, 21);
            this.rdoChTri.TabIndex = 2;
            this.rdoChTri.TabStop = true;
            this.rdoChTri.Text = "TRIANGULAR";
            this.rdoChTri.UseVisualStyleBackColor = true;
            // 
            // rdoChRect
            // 
            this.rdoChRect.AutoSize = true;
            this.rdoChRect.Location = new System.Drawing.Point(125, 23);
            this.rdoChRect.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rdoChRect.Name = "rdoChRect";
            this.rdoChRect.Size = new System.Drawing.Size(133, 21);
            this.rdoChRect.TabIndex = 1;
            this.rdoChRect.TabStop = true;
            this.rdoChRect.Text = "RECTANGULAR";
            this.rdoChRect.UseVisualStyleBackColor = true;
            // 
            // rdoChCirc
            // 
            this.rdoChCirc.AutoSize = true;
            this.rdoChCirc.Location = new System.Drawing.Point(12, 23);
            this.rdoChCirc.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rdoChCirc.Name = "rdoChCirc";
            this.rdoChCirc.Size = new System.Drawing.Size(97, 21);
            this.rdoChCirc.TabIndex = 0;
            this.rdoChCirc.TabStop = true;
            this.rdoChCirc.Text = "CIRCULAR";
            this.rdoChCirc.UseVisualStyleBackColor = true;
            // 
            // gbInputData
            // 
            this.gbInputData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbInputData.Controls.Add(this.frameNaturalChFP);
            this.gbInputData.Controls.Add(this.frameNaturalXsectGrid);
            this.gbInputData.Controls.Add(this.gbInputInfil);
            this.gbInputData.Controls.Add(this.gbInputGeometry);
            this.gbInputData.Controls.Add(this.btnShowInfilCalc);
            this.gbInputData.Controls.Add(this.btnShowOptControls);
            this.gbInputData.Location = new System.Drawing.Point(0, 207);
            this.gbInputData.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gbInputData.Name = "gbInputData";
            this.gbInputData.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gbInputData.Size = new System.Drawing.Size(1057, 405);
            this.gbInputData.TabIndex = 5;
            this.gbInputData.TabStop = false;
            this.gbInputData.Text = "Input Data";
            // 
            // frameNaturalChFP
            // 
            this.frameNaturalChFP.Controls.Add(this.txtGeomNFP_LOBX);
            this.frameNaturalChFP.Controls.Add(this.txtGeomNFP_ROBX);
            this.frameNaturalChFP.Controls.Add(this.label8);
            this.frameNaturalChFP.Controls.Add(this.label7);
            this.frameNaturalChFP.Controls.Add(this.label6);
            this.frameNaturalChFP.Controls.Add(this.label5);
            this.frameNaturalChFP.Controls.Add(this.label4);
            this.frameNaturalChFP.Controls.Add(this.label3);
            this.frameNaturalChFP.Controls.Add(this.label2);
            this.frameNaturalChFP.Controls.Add(this.txtGeomNFP_ROBMannN);
            this.frameNaturalChFP.Controls.Add(this.txtGeomNFP_ChMannN);
            this.frameNaturalChFP.Controls.Add(this.txtGeomNFP_LOBMannN);
            this.frameNaturalChFP.Controls.Add(this.txtGeomNFP_ROBLength);
            this.frameNaturalChFP.Controls.Add(this.txtGeomNFP_ChLength);
            this.frameNaturalChFP.Controls.Add(this.txtGeomNFP_LOBLength);
            this.frameNaturalChFP.Controls.Add(this.txtGeomNFP_ChLSlope);
            this.frameNaturalChFP.Controls.Add(this.lblNFP_ChSlope);
            this.frameNaturalChFP.Location = new System.Drawing.Point(12, 23);
            this.frameNaturalChFP.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.frameNaturalChFP.Name = "frameNaturalChFP";
            this.frameNaturalChFP.Size = new System.Drawing.Size(589, 338);
            this.frameNaturalChFP.TabIndex = 7;
            // 
            // txtGeomNFP_LOBX
            // 
            this.txtGeomNFP_LOBX.Location = new System.Drawing.Point(232, 80);
            this.txtGeomNFP_LOBX.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtGeomNFP_LOBX.Name = "txtGeomNFP_LOBX";
            this.txtGeomNFP_LOBX.Size = new System.Drawing.Size(132, 22);
            this.txtGeomNFP_LOBX.TabIndex = 16;
            // 
            // txtGeomNFP_ROBX
            // 
            this.txtGeomNFP_ROBX.Location = new System.Drawing.Point(232, 112);
            this.txtGeomNFP_ROBX.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtGeomNFP_ROBX.Name = "txtGeomNFP_ROBX";
            this.txtGeomNFP_ROBX.Size = new System.Drawing.Size(132, 22);
            this.txtGeomNFP_ROBX.TabIndex = 15;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(11, 116);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(211, 17);
            this.label8.TabIndex = 14;
            this.label8.Text = "Right Bank Ending X Coordinate";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(17, 84);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(207, 17);
            this.label7.TabIndex = 13;
            this.label7.Text = "Left Bank Starting X Coordinate";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(69, 222);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(86, 17);
            this.label6.TabIndex = 12;
            this.label6.Text = "Manning\'s N";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(21, 196);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(134, 17);
            this.label5.TabIndex = 11;
            this.label5.Text = "Downstream Length";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(328, 169);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 17);
            this.label4.TabIndex = 10;
            this.label4.Text = "Channel";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(445, 169);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(121, 17);
            this.label3.TabIndex = 9;
            this.label3.Text = "Right Bank (ROB)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(185, 169);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(110, 17);
            this.label2.TabIndex = 8;
            this.label2.Text = "Left Bank (LOB)";
            // 
            // txtGeomNFP_ROBMannN
            // 
            this.txtGeomNFP_ROBMannN.Location = new System.Drawing.Point(435, 218);
            this.txtGeomNFP_ROBMannN.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtGeomNFP_ROBMannN.Name = "txtGeomNFP_ROBMannN";
            this.txtGeomNFP_ROBMannN.Size = new System.Drawing.Size(132, 22);
            this.txtGeomNFP_ROBMannN.TabIndex = 7;
            // 
            // txtGeomNFP_ChMannN
            // 
            this.txtGeomNFP_ChMannN.Location = new System.Drawing.Point(300, 218);
            this.txtGeomNFP_ChMannN.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtGeomNFP_ChMannN.Name = "txtGeomNFP_ChMannN";
            this.txtGeomNFP_ChMannN.Size = new System.Drawing.Size(132, 22);
            this.txtGeomNFP_ChMannN.TabIndex = 6;
            // 
            // txtGeomNFP_LOBMannN
            // 
            this.txtGeomNFP_LOBMannN.Location = new System.Drawing.Point(165, 218);
            this.txtGeomNFP_LOBMannN.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtGeomNFP_LOBMannN.Name = "txtGeomNFP_LOBMannN";
            this.txtGeomNFP_LOBMannN.Size = new System.Drawing.Size(132, 22);
            this.txtGeomNFP_LOBMannN.TabIndex = 5;
            // 
            // txtGeomNFP_ROBLength
            // 
            this.txtGeomNFP_ROBLength.Location = new System.Drawing.Point(435, 192);
            this.txtGeomNFP_ROBLength.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtGeomNFP_ROBLength.Name = "txtGeomNFP_ROBLength";
            this.txtGeomNFP_ROBLength.Size = new System.Drawing.Size(132, 22);
            this.txtGeomNFP_ROBLength.TabIndex = 4;
            // 
            // txtGeomNFP_ChLength
            // 
            this.txtGeomNFP_ChLength.Location = new System.Drawing.Point(300, 192);
            this.txtGeomNFP_ChLength.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtGeomNFP_ChLength.Name = "txtGeomNFP_ChLength";
            this.txtGeomNFP_ChLength.Size = new System.Drawing.Size(132, 22);
            this.txtGeomNFP_ChLength.TabIndex = 3;
            // 
            // txtGeomNFP_LOBLength
            // 
            this.txtGeomNFP_LOBLength.Location = new System.Drawing.Point(165, 192);
            this.txtGeomNFP_LOBLength.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtGeomNFP_LOBLength.Name = "txtGeomNFP_LOBLength";
            this.txtGeomNFP_LOBLength.Size = new System.Drawing.Size(132, 22);
            this.txtGeomNFP_LOBLength.TabIndex = 2;
            // 
            // txtGeomNFP_ChLSlope
            // 
            this.txtGeomNFP_ChLSlope.Location = new System.Drawing.Point(232, 22);
            this.txtGeomNFP_ChLSlope.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtGeomNFP_ChLSlope.Name = "txtGeomNFP_ChLSlope";
            this.txtGeomNFP_ChLSlope.Size = new System.Drawing.Size(132, 22);
            this.txtGeomNFP_ChLSlope.TabIndex = 1;
            // 
            // lblNFP_ChSlope
            // 
            this.lblNFP_ChSlope.AutoSize = true;
            this.lblNFP_ChSlope.Location = new System.Drawing.Point(47, 26);
            this.lblNFP_ChSlope.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblNFP_ChSlope.Name = "lblNFP_ChSlope";
            this.lblNFP_ChSlope.Size = new System.Drawing.Size(181, 17);
            this.lblNFP_ChSlope.TabIndex = 0;
            this.lblNFP_ChSlope.Text = "Channel Longitudinal Slope";
            // 
            // frameNaturalXsectGrid
            // 
            this.frameNaturalXsectGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.frameNaturalXsectGrid.Controls.Add(this.btnUndoClear);
            this.frameNaturalXsectGrid.Controls.Add(this.btnClearProfile);
            this.frameNaturalXsectGrid.Controls.Add(this.btnImportChProfile);
            this.frameNaturalXsectGrid.Controls.Add(this.grdChProfile);
            this.frameNaturalXsectGrid.Location = new System.Drawing.Point(609, 9);
            this.frameNaturalXsectGrid.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.frameNaturalXsectGrid.Name = "frameNaturalXsectGrid";
            this.frameNaturalXsectGrid.Size = new System.Drawing.Size(440, 353);
            this.frameNaturalXsectGrid.TabIndex = 6;
            // 
            // btnUndoClear
            // 
            this.btnUndoClear.Location = new System.Drawing.Point(331, 5);
            this.btnUndoClear.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnUndoClear.Name = "btnUndoClear";
            this.btnUndoClear.Size = new System.Drawing.Size(100, 28);
            this.btnUndoClear.TabIndex = 9;
            this.btnUndoClear.Text = "Undo Clear";
            this.btnUndoClear.UseVisualStyleBackColor = true;
            this.btnUndoClear.Click += new System.EventHandler(this.btnUndoClear_Click);
            // 
            // btnClearProfile
            // 
            this.btnClearProfile.Location = new System.Drawing.Point(223, 5);
            this.btnClearProfile.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnClearProfile.Name = "btnClearProfile";
            this.btnClearProfile.Size = new System.Drawing.Size(100, 28);
            this.btnClearProfile.TabIndex = 8;
            this.btnClearProfile.Text = "Clear Profile";
            this.btnClearProfile.UseVisualStyleBackColor = true;
            this.btnClearProfile.Click += new System.EventHandler(this.btnClearProfile_Click);
            // 
            // btnImportChProfile
            // 
            this.btnImportChProfile.Location = new System.Drawing.Point(5, 5);
            this.btnImportChProfile.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnImportChProfile.Name = "btnImportChProfile";
            this.btnImportChProfile.Size = new System.Drawing.Size(209, 28);
            this.btnImportChProfile.TabIndex = 6;
            this.btnImportChProfile.Text = "Import Channel Profile";
            this.btnImportChProfile.UseVisualStyleBackColor = true;
            this.btnImportChProfile.Click += new System.EventHandler(this.btnImportChProfile_Click);
            // 
            // grdChProfile
            // 
            this.grdChProfile.AllowHorizontalScrolling = true;
            this.grdChProfile.AllowNewValidValues = false;
            this.grdChProfile.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdChProfile.CellBackColor = System.Drawing.SystemColors.Window;
            this.grdChProfile.Fixed3D = false;
            this.grdChProfile.LineColor = System.Drawing.SystemColors.Control;
            this.grdChProfile.LineWidth = 1F;
            this.grdChProfile.Location = new System.Drawing.Point(4, 41);
            this.grdChProfile.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.grdChProfile.Name = "grdChProfile";
            this.grdChProfile.Size = new System.Drawing.Size(432, 309);
            this.grdChProfile.Source = null;
            this.grdChProfile.TabIndex = 5;
            // 
            // gbInputInfil
            // 
            this.gbInputInfil.Controls.Add(this.txtInfilDrainTime);
            this.gbInputInfil.Controls.Add(this.label16);
            this.gbInputInfil.Controls.Add(this.txtInfilDepth);
            this.gbInputInfil.Controls.Add(this.label15);
            this.gbInputInfil.Controls.Add(this.txtInfilRate);
            this.gbInputInfil.Controls.Add(this.label14);
            this.gbInputInfil.Controls.Add(this.txtBackfillPore);
            this.gbInputInfil.Controls.Add(this.lblBackfillPore);
            this.gbInputInfil.Controls.Add(this.txtBackfillDepth);
            this.gbInputInfil.Controls.Add(this.lblBackfillDepth);
            this.gbInputInfil.Controls.Add(this.chkBackfill);
            this.gbInputInfil.Location = new System.Drawing.Point(305, 23);
            this.gbInputInfil.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gbInputInfil.Name = "gbInputInfil";
            this.gbInputInfil.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gbInputInfil.Size = new System.Drawing.Size(295, 338);
            this.gbInputInfil.TabIndex = 4;
            this.gbInputInfil.TabStop = false;
            this.gbInputInfil.Text = "Infiltration Related";
            // 
            // txtInfilDrainTime
            // 
            this.txtInfilDrainTime.Location = new System.Drawing.Point(143, 244);
            this.txtInfilDrainTime.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtInfilDrainTime.Name = "txtInfilDrainTime";
            this.txtInfilDrainTime.Size = new System.Drawing.Size(129, 22);
            this.txtInfilDrainTime.TabIndex = 14;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(29, 247);
            this.label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(108, 17);
            this.label16.TabIndex = 13;
            this.label16.Text = "Drain Time (hr):";
            // 
            // txtInfilDepth
            // 
            this.txtInfilDepth.Location = new System.Drawing.Point(143, 199);
            this.txtInfilDepth.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtInfilDepth.Name = "txtInfilDepth";
            this.txtInfilDepth.Size = new System.Drawing.Size(129, 22);
            this.txtInfilDepth.TabIndex = 12;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(33, 192);
            this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(101, 34);
            this.label15.TabIndex = 11;
            this.label15.Text = "Infiltration\r\nDepth (in, cm):";
            // 
            // txtInfilRate
            // 
            this.txtInfilRate.Location = new System.Drawing.Point(143, 156);
            this.txtInfilRate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtInfilRate.Name = "txtInfilRate";
            this.txtInfilRate.Size = new System.Drawing.Size(129, 22);
            this.txtInfilRate.TabIndex = 10;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(8, 149);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(127, 34);
            this.label14.TabIndex = 9;
            this.label14.Text = "Infiltration\r\nRate (in/hr, cm/hr):";
            // 
            // txtBackfillPore
            // 
            this.txtBackfillPore.Location = new System.Drawing.Point(143, 85);
            this.txtBackfillPore.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtBackfillPore.Name = "txtBackfillPore";
            this.txtBackfillPore.Size = new System.Drawing.Size(129, 22);
            this.txtBackfillPore.TabIndex = 8;
            // 
            // lblBackfillPore
            // 
            this.lblBackfillPore.AutoSize = true;
            this.lblBackfillPore.Location = new System.Drawing.Point(8, 89);
            this.lblBackfillPore.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblBackfillPore.Name = "lblBackfillPore";
            this.lblBackfillPore.Size = new System.Drawing.Size(126, 17);
            this.lblBackfillPore.TabIndex = 7;
            this.lblBackfillPore.Text = "Porosity of backfill:";
            // 
            // txtBackfillDepth
            // 
            this.txtBackfillDepth.Location = new System.Drawing.Point(143, 53);
            this.txtBackfillDepth.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtBackfillDepth.Name = "txtBackfillDepth";
            this.txtBackfillDepth.Size = new System.Drawing.Size(129, 22);
            this.txtBackfillDepth.TabIndex = 6;
            // 
            // lblBackfillDepth
            // 
            this.lblBackfillDepth.AutoSize = true;
            this.lblBackfillDepth.Location = new System.Drawing.Point(19, 57);
            this.lblBackfillDepth.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblBackfillDepth.Name = "lblBackfillDepth";
            this.lblBackfillDepth.Size = new System.Drawing.Size(113, 17);
            this.lblBackfillDepth.TabIndex = 5;
            this.lblBackfillDepth.Text = "Depth of backfill:";
            // 
            // chkBackfill
            // 
            this.chkBackfill.AutoSize = true;
            this.chkBackfill.Location = new System.Drawing.Point(9, 25);
            this.chkBackfill.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkBackfill.Name = "chkBackfill";
            this.chkBackfill.Size = new System.Drawing.Size(221, 21);
            this.chkBackfill.TabIndex = 0;
            this.chkBackfill.Text = "This structure contains backfill";
            this.chkBackfill.UseVisualStyleBackColor = true;
            this.chkBackfill.CheckedChanged += new System.EventHandler(this.chkBackfill_CheckedChanged);
            // 
            // gbInputGeometry
            // 
            this.gbInputGeometry.Controls.Add(this.txtGeomDepth);
            this.gbInputGeometry.Controls.Add(this.txtGeomWidth);
            this.gbInputGeometry.Controls.Add(this.txtGeomSideSlope);
            this.gbInputGeometry.Controls.Add(this.txtGeomTopWidth);
            this.gbInputGeometry.Controls.Add(this.txtGeomMaxDepth);
            this.gbInputGeometry.Controls.Add(this.lblGeomDepth);
            this.gbInputGeometry.Controls.Add(this.lblGeomWidth);
            this.gbInputGeometry.Controls.Add(this.lblGeomSideSlope);
            this.gbInputGeometry.Controls.Add(this.lblGeomTopWidth);
            this.gbInputGeometry.Controls.Add(this.lblGeomMaxDepth);
            this.gbInputGeometry.Controls.Add(this.txtGeomDiam);
            this.gbInputGeometry.Controls.Add(this.lblGeomDiam);
            this.gbInputGeometry.Controls.Add(this.txtGeomHInc);
            this.gbInputGeometry.Controls.Add(this.txtGeomMannN);
            this.gbInputGeometry.Controls.Add(this.txtGeomLSlope);
            this.gbInputGeometry.Controls.Add(this.txtGeomLength);
            this.gbInputGeometry.Controls.Add(this.lblGeomHInc);
            this.gbInputGeometry.Controls.Add(this.lblGeomLSlope);
            this.gbInputGeometry.Controls.Add(this.lblGeomMannN);
            this.gbInputGeometry.Controls.Add(this.lblGeomLength);
            this.gbInputGeometry.Location = new System.Drawing.Point(16, 23);
            this.gbInputGeometry.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gbInputGeometry.Name = "gbInputGeometry";
            this.gbInputGeometry.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gbInputGeometry.Size = new System.Drawing.Size(281, 338);
            this.gbInputGeometry.TabIndex = 3;
            this.gbInputGeometry.TabStop = false;
            this.gbInputGeometry.Text = "Structure Geometry";
            // 
            // txtGeomDepth
            // 
            this.txtGeomDepth.Location = new System.Drawing.Point(180, 304);
            this.txtGeomDepth.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtGeomDepth.Name = "txtGeomDepth";
            this.txtGeomDepth.Size = new System.Drawing.Size(92, 22);
            this.txtGeomDepth.TabIndex = 19;
            // 
            // txtGeomWidth
            // 
            this.txtGeomWidth.Location = new System.Drawing.Point(180, 272);
            this.txtGeomWidth.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtGeomWidth.Name = "txtGeomWidth";
            this.txtGeomWidth.Size = new System.Drawing.Size(92, 22);
            this.txtGeomWidth.TabIndex = 18;
            // 
            // txtGeomSideSlope
            // 
            this.txtGeomSideSlope.Location = new System.Drawing.Point(180, 240);
            this.txtGeomSideSlope.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtGeomSideSlope.Name = "txtGeomSideSlope";
            this.txtGeomSideSlope.Size = new System.Drawing.Size(92, 22);
            this.txtGeomSideSlope.TabIndex = 17;
            // 
            // txtGeomTopWidth
            // 
            this.txtGeomTopWidth.Location = new System.Drawing.Point(180, 208);
            this.txtGeomTopWidth.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtGeomTopWidth.Name = "txtGeomTopWidth";
            this.txtGeomTopWidth.Size = new System.Drawing.Size(92, 22);
            this.txtGeomTopWidth.TabIndex = 16;
            // 
            // txtGeomMaxDepth
            // 
            this.txtGeomMaxDepth.Location = new System.Drawing.Point(180, 176);
            this.txtGeomMaxDepth.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtGeomMaxDepth.Name = "txtGeomMaxDepth";
            this.txtGeomMaxDepth.Size = new System.Drawing.Size(92, 22);
            this.txtGeomMaxDepth.TabIndex = 15;
            // 
            // lblGeomDepth
            // 
            this.lblGeomDepth.AutoSize = true;
            this.lblGeomDepth.Location = new System.Drawing.Point(120, 308);
            this.lblGeomDepth.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblGeomDepth.Name = "lblGeomDepth";
            this.lblGeomDepth.Size = new System.Drawing.Size(50, 17);
            this.lblGeomDepth.TabIndex = 14;
            this.lblGeomDepth.Text = "Depth:";
            // 
            // lblGeomWidth
            // 
            this.lblGeomWidth.AutoSize = true;
            this.lblGeomWidth.Location = new System.Drawing.Point(121, 276);
            this.lblGeomWidth.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblGeomWidth.Name = "lblGeomWidth";
            this.lblGeomWidth.Size = new System.Drawing.Size(48, 17);
            this.lblGeomWidth.TabIndex = 13;
            this.lblGeomWidth.Text = "Width:";
            // 
            // lblGeomSideSlope
            // 
            this.lblGeomSideSlope.AutoSize = true;
            this.lblGeomSideSlope.Location = new System.Drawing.Point(55, 244);
            this.lblGeomSideSlope.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblGeomSideSlope.Name = "lblGeomSideSlope";
            this.lblGeomSideSlope.Size = new System.Drawing.Size(117, 17);
            this.lblGeomSideSlope.TabIndex = 12;
            this.lblGeomSideSlope.Text = "Side Slope (H:V):";
            // 
            // lblGeomTopWidth
            // 
            this.lblGeomTopWidth.AutoSize = true;
            this.lblGeomTopWidth.Location = new System.Drawing.Point(92, 212);
            this.lblGeomTopWidth.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblGeomTopWidth.Name = "lblGeomTopWidth";
            this.lblGeomTopWidth.Size = new System.Drawing.Size(77, 17);
            this.lblGeomTopWidth.TabIndex = 11;
            this.lblGeomTopWidth.Text = "Top Width:";
            // 
            // lblGeomMaxDepth
            // 
            this.lblGeomMaxDepth.AutoSize = true;
            this.lblGeomMaxDepth.Location = new System.Drawing.Point(57, 180);
            this.lblGeomMaxDepth.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblGeomMaxDepth.Name = "lblGeomMaxDepth";
            this.lblGeomMaxDepth.Size = new System.Drawing.Size(112, 17);
            this.lblGeomMaxDepth.TabIndex = 10;
            this.lblGeomMaxDepth.Text = "Maximum Depth:";
            // 
            // txtGeomDiam
            // 
            this.txtGeomDiam.Location = new System.Drawing.Point(180, 144);
            this.txtGeomDiam.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtGeomDiam.Name = "txtGeomDiam";
            this.txtGeomDiam.Size = new System.Drawing.Size(92, 22);
            this.txtGeomDiam.TabIndex = 9;
            // 
            // lblGeomDiam
            // 
            this.lblGeomDiam.AutoSize = true;
            this.lblGeomDiam.Location = new System.Drawing.Point(103, 148);
            this.lblGeomDiam.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblGeomDiam.Name = "lblGeomDiam";
            this.lblGeomDiam.Size = new System.Drawing.Size(69, 17);
            this.lblGeomDiam.TabIndex = 8;
            this.lblGeomDiam.Text = "Diameter:";
            // 
            // txtGeomHInc
            // 
            this.txtGeomHInc.Location = new System.Drawing.Point(180, 112);
            this.txtGeomHInc.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtGeomHInc.Name = "txtGeomHInc";
            this.txtGeomHInc.Size = new System.Drawing.Size(92, 22);
            this.txtGeomHInc.TabIndex = 7;
            // 
            // txtGeomMannN
            // 
            this.txtGeomMannN.Location = new System.Drawing.Point(180, 48);
            this.txtGeomMannN.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtGeomMannN.Name = "txtGeomMannN";
            this.txtGeomMannN.Size = new System.Drawing.Size(92, 22);
            this.txtGeomMannN.TabIndex = 6;
            // 
            // txtGeomLSlope
            // 
            this.txtGeomLSlope.Location = new System.Drawing.Point(180, 80);
            this.txtGeomLSlope.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtGeomLSlope.Name = "txtGeomLSlope";
            this.txtGeomLSlope.Size = new System.Drawing.Size(92, 22);
            this.txtGeomLSlope.TabIndex = 5;
            // 
            // txtGeomLength
            // 
            this.txtGeomLength.Location = new System.Drawing.Point(180, 16);
            this.txtGeomLength.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtGeomLength.Name = "txtGeomLength";
            this.txtGeomLength.Size = new System.Drawing.Size(92, 22);
            this.txtGeomLength.TabIndex = 4;
            // 
            // lblGeomHInc
            // 
            this.lblGeomHInc.AutoSize = true;
            this.lblGeomHInc.Location = new System.Drawing.Point(51, 116);
            this.lblGeomHInc.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblGeomHInc.Name = "lblGeomHInc";
            this.lblGeomHInc.Size = new System.Drawing.Size(119, 17);
            this.lblGeomHInc.TabIndex = 3;
            this.lblGeomHInc.Text = "Height Increment:";
            // 
            // lblGeomLSlope
            // 
            this.lblGeomLSlope.AutoSize = true;
            this.lblGeomLSlope.Location = new System.Drawing.Point(8, 84);
            this.lblGeomLSlope.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblGeomLSlope.Name = "lblGeomLSlope";
            this.lblGeomLSlope.Size = new System.Drawing.Size(163, 17);
            this.lblGeomLSlope.TabIndex = 2;
            this.lblGeomLSlope.Text = "Longitudinal Slope (ft/ft):";
            // 
            // lblGeomMannN
            // 
            this.lblGeomMannN.AutoSize = true;
            this.lblGeomMannN.Location = new System.Drawing.Point(80, 52);
            this.lblGeomMannN.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblGeomMannN.Name = "lblGeomMannN";
            this.lblGeomMannN.Size = new System.Drawing.Size(90, 17);
            this.lblGeomMannN.TabIndex = 1;
            this.lblGeomMannN.Text = "Manning\'s N:";
            // 
            // lblGeomLength
            // 
            this.lblGeomLength.AutoSize = true;
            this.lblGeomLength.Location = new System.Drawing.Point(115, 20);
            this.lblGeomLength.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblGeomLength.Name = "lblGeomLength";
            this.lblGeomLength.Size = new System.Drawing.Size(56, 17);
            this.lblGeomLength.TabIndex = 0;
            this.lblGeomLength.Text = "Length:";
            // 
            // mainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1057, 652);
            this.Controls.Add(this.gbInputData);
            this.Controls.Add(this.gbChannel);
            this.Controls.Add(this.btnHelp);
            this.Controls.Add(this.btnCalcFtable);
            this.Controls.Add(this.frameTop);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "mainForm";
            this.Text = "HSPF FTable Builder";
            this.Load += new System.EventHandler(this.mainForm_Load);
            this.frameTop.ResumeLayout(false);
            this.frameTop.PerformLayout();
            this.gbChannel.ResumeLayout(false);
            this.gbChannel.PerformLayout();
            this.gbInputData.ResumeLayout(false);
            this.frameNaturalChFP.ResumeLayout(false);
            this.frameNaturalChFP.PerformLayout();
            this.frameNaturalXsectGrid.ResumeLayout(false);
            this.gbInputInfil.ResumeLayout(false);
            this.gbInputInfil.PerformLayout();
            this.gbInputGeometry.ResumeLayout(false);
            this.gbInputGeometry.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel frameTop;
        private System.Windows.Forms.Label lblUnit;
        private System.Windows.Forms.RadioButton rdoUnitSI;
        private System.Windows.Forms.RadioButton rdoUnitUS;
        private System.Windows.Forms.ComboBox cboBMPTypes;
        private System.Windows.Forms.Label lblBMPList;
        private System.Windows.Forms.Label lblBMPMsg;
        private System.Windows.Forms.Button btnShowOptControls;
        private System.Windows.Forms.Button btnShowInfilCalc;
        private System.Windows.Forms.Button btnCalcFtable;
        private System.Windows.Forms.Button btnHelp;
        private System.Windows.Forms.GroupBox gbChannel;
        private System.Windows.Forms.RadioButton rdoChNatural;
        private System.Windows.Forms.RadioButton rdoChPara;
        private System.Windows.Forms.RadioButton rdoChTrape;
        private System.Windows.Forms.RadioButton rdoChTri;
        private System.Windows.Forms.RadioButton rdoChRect;
        private System.Windows.Forms.RadioButton rdoChCirc;
        private System.Windows.Forms.GroupBox gbInputData;
        private System.Windows.Forms.GroupBox gbInputInfil;
        private System.Windows.Forms.GroupBox gbInputGeometry;
        private System.Windows.Forms.TextBox txtGeomHInc;
        private System.Windows.Forms.TextBox txtGeomMannN;
        private System.Windows.Forms.TextBox txtGeomLSlope;
        private System.Windows.Forms.TextBox txtGeomLength;
        private System.Windows.Forms.Label lblGeomHInc;
        private System.Windows.Forms.Label lblGeomLSlope;
        private System.Windows.Forms.Label lblGeomMannN;
        private System.Windows.Forms.Label lblGeomLength;
        private System.Windows.Forms.TextBox txtGeomDepth;
        private System.Windows.Forms.TextBox txtGeomWidth;
        private System.Windows.Forms.TextBox txtGeomSideSlope;
        private System.Windows.Forms.TextBox txtGeomTopWidth;
        private System.Windows.Forms.TextBox txtGeomMaxDepth;
        private System.Windows.Forms.Label lblGeomDepth;
        private System.Windows.Forms.Label lblGeomWidth;
        private System.Windows.Forms.Label lblGeomSideSlope;
        private System.Windows.Forms.Label lblGeomTopWidth;
        private System.Windows.Forms.Label lblGeomMaxDepth;
        private System.Windows.Forms.TextBox txtGeomDiam;
        private System.Windows.Forms.Label lblGeomDiam;
        private System.Windows.Forms.TextBox txtBackfillPore;
        private System.Windows.Forms.Label lblBackfillPore;
        private System.Windows.Forms.TextBox txtBackfillDepth;
        private System.Windows.Forms.Label lblBackfillDepth;
        private System.Windows.Forms.CheckBox chkBackfill;
        private System.Windows.Forms.TextBox txtInfilDrainTime;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox txtInfilDepth;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtInfilRate;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Panel frameNaturalXsectGrid;
        private System.Windows.Forms.Button btnUndoClear;
        private System.Windows.Forms.Button btnClearProfile;
        private System.Windows.Forms.Button btnImportChProfile;
        private atcControls.atcGrid grdChProfile;
        private BMPSketch bmpSketch1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.RadioButton rdoChNaturalFP;
        private System.Windows.Forms.Panel frameNaturalChFP;
        private System.Windows.Forms.TextBox txtGeomNFP_ChLSlope;
        private System.Windows.Forms.Label lblNFP_ChSlope;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtGeomNFP_ROBMannN;
        private System.Windows.Forms.TextBox txtGeomNFP_ChMannN;
        private System.Windows.Forms.TextBox txtGeomNFP_LOBMannN;
        private System.Windows.Forms.TextBox txtGeomNFP_ROBLength;
        private System.Windows.Forms.TextBox txtGeomNFP_ChLength;
        private System.Windows.Forms.TextBox txtGeomNFP_LOBLength;
        private System.Windows.Forms.TextBox txtGeomNFP_LOBX;
        private System.Windows.Forms.TextBox txtGeomNFP_ROBX;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
    }
}