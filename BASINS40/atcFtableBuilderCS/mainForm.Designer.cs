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
            this.bmpSketch1 = new atcFtableBuilder.BMPSketch();
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
            this.frameTop.Name = "frameTop";
            this.frameTop.Size = new System.Drawing.Size(793, 41);
            this.frameTop.TabIndex = 1;
            // 
            // lblBMPMsg
            // 
            this.lblBMPMsg.AutoSize = true;
            this.lblBMPMsg.Location = new System.Drawing.Point(537, 11);
            this.lblBMPMsg.Name = "lblBMPMsg";
            this.lblBMPMsg.Size = new System.Drawing.Size(231, 26);
            this.lblBMPMsg.TabIndex = 5;
            this.lblBMPMsg.Text = "A suggested channel geometry will be selected.\r\nYou can change this shape.";
            // 
            // cboBMPTypes
            // 
            this.cboBMPTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboBMPTypes.FormattingEnabled = true;
            this.cboBMPTypes.Location = new System.Drawing.Point(384, 10);
            this.cboBMPTypes.Name = "cboBMPTypes";
            this.cboBMPTypes.Size = new System.Drawing.Size(147, 21);
            this.cboBMPTypes.TabIndex = 4;
            this.cboBMPTypes.SelectedIndexChanged += new System.EventHandler(this.cboBMPTypes_SelectedIndexChanged);
            // 
            // lblBMPList
            // 
            this.lblBMPList.AutoSize = true;
            this.lblBMPList.Location = new System.Drawing.Point(274, 13);
            this.lblBMPList.Name = "lblBMPList";
            this.lblBMPList.Size = new System.Drawing.Size(104, 13);
            this.lblBMPList.TabIndex = 3;
            this.lblBMPList.Text = "Choose a BMP type:";
            // 
            // rdoUnitSI
            // 
            this.rdoUnitSI.AutoSize = true;
            this.rdoUnitSI.Location = new System.Drawing.Point(177, 11);
            this.rdoUnitSI.Name = "rdoUnitSI";
            this.rdoUnitSI.Size = new System.Drawing.Size(65, 17);
            this.rdoUnitSI.TabIndex = 2;
            this.rdoUnitSI.TabStop = true;
            this.rdoUnitSI.Text = "S.I.Units";
            this.rdoUnitSI.UseVisualStyleBackColor = true;
            // 
            // rdoUnitUS
            // 
            this.rdoUnitUS.AutoSize = true;
            this.rdoUnitUS.Location = new System.Drawing.Point(86, 11);
            this.rdoUnitUS.Name = "rdoUnitUS";
            this.rdoUnitUS.Size = new System.Drawing.Size(70, 17);
            this.rdoUnitUS.TabIndex = 1;
            this.rdoUnitUS.TabStop = true;
            this.rdoUnitUS.Text = "U.S.Units";
            this.rdoUnitUS.UseVisualStyleBackColor = true;
            // 
            // lblUnit
            // 
            this.lblUnit.AutoSize = true;
            this.lblUnit.Location = new System.Drawing.Point(13, 13);
            this.lblUnit.Name = "lblUnit";
            this.lblUnit.Size = new System.Drawing.Size(67, 13);
            this.lblUnit.TabIndex = 0;
            this.lblUnit.Text = "Specify Unit:";
            // 
            // btnShowOptControls
            // 
            this.btnShowOptControls.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnShowOptControls.Location = new System.Drawing.Point(12, 300);
            this.btnShowOptControls.Name = "btnShowOptControls";
            this.btnShowOptControls.Size = new System.Drawing.Size(165, 23);
            this.btnShowOptControls.TabIndex = 2;
            this.btnShowOptControls.Text = "Show optional control devices";
            this.btnShowOptControls.UseVisualStyleBackColor = true;
            this.btnShowOptControls.Click += new System.EventHandler(this.btnShowOptControls_Click);
            // 
            // btnShowInfilCalc
            // 
            this.btnShowInfilCalc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnShowInfilCalc.Location = new System.Drawing.Point(183, 300);
            this.btnShowInfilCalc.Name = "btnShowInfilCalc";
            this.btnShowInfilCalc.Size = new System.Drawing.Size(151, 23);
            this.btnShowInfilCalc.TabIndex = 1;
            this.btnShowInfilCalc.Text = "Show infiltration calculator";
            this.btnShowInfilCalc.UseVisualStyleBackColor = true;
            this.btnShowInfilCalc.Click += new System.EventHandler(this.btnShowInfilCalc_Click);
            // 
            // btnCalcFtable
            // 
            this.btnCalcFtable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCalcFtable.Location = new System.Drawing.Point(646, 503);
            this.btnCalcFtable.Name = "btnCalcFtable";
            this.btnCalcFtable.Size = new System.Drawing.Size(135, 23);
            this.btnCalcFtable.TabIndex = 3;
            this.btnCalcFtable.Text = "Calculate FTable";
            this.btnCalcFtable.UseVisualStyleBackColor = true;
            this.btnCalcFtable.Click += new System.EventHandler(this.btnCalcFtable_Click);
            // 
            // btnHelp
            // 
            this.btnHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnHelp.Location = new System.Drawing.Point(565, 503);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(75, 23);
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
            this.gbChannel.Location = new System.Drawing.Point(0, 47);
            this.gbChannel.Name = "gbChannel";
            this.gbChannel.Size = new System.Drawing.Size(793, 122);
            this.gbChannel.TabIndex = 3;
            this.gbChannel.TabStop = false;
            this.gbChannel.Text = "Choose Channel Type";
            // 
            // rdoChNaturalFP
            // 
            this.rdoChNaturalFP.AutoSize = true;
            this.rdoChNaturalFP.Location = new System.Drawing.Point(587, 19);
            this.rdoChNaturalFP.Name = "rdoChNaturalFP";
            this.rdoChNaturalFP.Size = new System.Drawing.Size(155, 17);
            this.rdoChNaturalFP.TabIndex = 7;
            this.rdoChNaturalFP.TabStop = true;
            this.rdoChNaturalFP.Text = "NATURAL (with flood plain)";
            this.rdoChNaturalFP.UseVisualStyleBackColor = true;
            // 
            // rdoChNatural
            // 
            this.rdoChNatural.AutoSize = true;
            this.rdoChNatural.Location = new System.Drawing.Point(504, 19);
            this.rdoChNatural.Name = "rdoChNatural";
            this.rdoChNatural.Size = new System.Drawing.Size(76, 17);
            this.rdoChNatural.TabIndex = 5;
            this.rdoChNatural.TabStop = true;
            this.rdoChNatural.Text = "NATURAL";
            this.rdoChNatural.UseVisualStyleBackColor = true;
            // 
            // rdoChPara
            // 
            this.rdoChPara.AutoSize = true;
            this.rdoChPara.Location = new System.Drawing.Point(413, 19);
            this.rdoChPara.Name = "rdoChPara";
            this.rdoChPara.Size = new System.Drawing.Size(85, 17);
            this.rdoChPara.TabIndex = 4;
            this.rdoChPara.TabStop = true;
            this.rdoChPara.Text = "PARABOLIC";
            this.rdoChPara.UseVisualStyleBackColor = true;
            // 
            // rdoChTrape
            // 
            this.rdoChTrape.AutoSize = true;
            this.rdoChTrape.Location = new System.Drawing.Point(307, 19);
            this.rdoChTrape.Name = "rdoChTrape";
            this.rdoChTrape.Size = new System.Drawing.Size(100, 17);
            this.rdoChTrape.TabIndex = 3;
            this.rdoChTrape.TabStop = true;
            this.rdoChTrape.Text = "TRAPEZOIDAL";
            this.rdoChTrape.UseVisualStyleBackColor = true;
            // 
            // rdoChTri
            // 
            this.rdoChTri.AutoSize = true;
            this.rdoChTri.Location = new System.Drawing.Point(206, 19);
            this.rdoChTri.Name = "rdoChTri";
            this.rdoChTri.Size = new System.Drawing.Size(95, 17);
            this.rdoChTri.TabIndex = 2;
            this.rdoChTri.TabStop = true;
            this.rdoChTri.Text = "TRIANGULAR";
            this.rdoChTri.UseVisualStyleBackColor = true;
            // 
            // rdoChRect
            // 
            this.rdoChRect.AutoSize = true;
            this.rdoChRect.Location = new System.Drawing.Point(94, 19);
            this.rdoChRect.Name = "rdoChRect";
            this.rdoChRect.Size = new System.Drawing.Size(106, 17);
            this.rdoChRect.TabIndex = 1;
            this.rdoChRect.TabStop = true;
            this.rdoChRect.Text = "RECTANGULAR";
            this.rdoChRect.UseVisualStyleBackColor = true;
            // 
            // rdoChCirc
            // 
            this.rdoChCirc.AutoSize = true;
            this.rdoChCirc.Location = new System.Drawing.Point(9, 19);
            this.rdoChCirc.Name = "rdoChCirc";
            this.rdoChCirc.Size = new System.Drawing.Size(79, 17);
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
            this.gbInputData.Location = new System.Drawing.Point(0, 168);
            this.gbInputData.Name = "gbInputData";
            this.gbInputData.Size = new System.Drawing.Size(793, 329);
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
            this.frameNaturalChFP.Location = new System.Drawing.Point(9, 19);
            this.frameNaturalChFP.Name = "frameNaturalChFP";
            this.frameNaturalChFP.Size = new System.Drawing.Size(442, 275);
            this.frameNaturalChFP.TabIndex = 7;
            // 
            // txtGeomNFP_LOBX
            // 
            this.txtGeomNFP_LOBX.Location = new System.Drawing.Point(174, 65);
            this.txtGeomNFP_LOBX.Name = "txtGeomNFP_LOBX";
            this.txtGeomNFP_LOBX.Size = new System.Drawing.Size(100, 20);
            this.txtGeomNFP_LOBX.TabIndex = 16;
            // 
            // txtGeomNFP_ROBX
            // 
            this.txtGeomNFP_ROBX.Location = new System.Drawing.Point(174, 91);
            this.txtGeomNFP_ROBX.Name = "txtGeomNFP_ROBX";
            this.txtGeomNFP_ROBX.Size = new System.Drawing.Size(100, 20);
            this.txtGeomNFP_ROBX.TabIndex = 15;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(8, 94);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(160, 13);
            this.label8.TabIndex = 14;
            this.label8.Text = "Right Bank Ending X Coordinate";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(13, 68);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(156, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "Left Bank Starting X Coordinate";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(52, 180);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(66, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Manning\'s N";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 159);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(102, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Downstream Length";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(246, 137);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Channel";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(334, 137);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(92, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Right Bank (ROB)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(139, 137);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Left Bank (LOB)";
            // 
            // txtGeomNFP_ROBMannN
            // 
            this.txtGeomNFP_ROBMannN.Location = new System.Drawing.Point(326, 177);
            this.txtGeomNFP_ROBMannN.Name = "txtGeomNFP_ROBMannN";
            this.txtGeomNFP_ROBMannN.Size = new System.Drawing.Size(100, 20);
            this.txtGeomNFP_ROBMannN.TabIndex = 7;
            // 
            // txtGeomNFP_ChMannN
            // 
            this.txtGeomNFP_ChMannN.Location = new System.Drawing.Point(225, 177);
            this.txtGeomNFP_ChMannN.Name = "txtGeomNFP_ChMannN";
            this.txtGeomNFP_ChMannN.Size = new System.Drawing.Size(100, 20);
            this.txtGeomNFP_ChMannN.TabIndex = 6;
            // 
            // txtGeomNFP_LOBMannN
            // 
            this.txtGeomNFP_LOBMannN.Location = new System.Drawing.Point(124, 177);
            this.txtGeomNFP_LOBMannN.Name = "txtGeomNFP_LOBMannN";
            this.txtGeomNFP_LOBMannN.Size = new System.Drawing.Size(100, 20);
            this.txtGeomNFP_LOBMannN.TabIndex = 5;
            // 
            // txtGeomNFP_ROBLength
            // 
            this.txtGeomNFP_ROBLength.Location = new System.Drawing.Point(326, 156);
            this.txtGeomNFP_ROBLength.Name = "txtGeomNFP_ROBLength";
            this.txtGeomNFP_ROBLength.Size = new System.Drawing.Size(100, 20);
            this.txtGeomNFP_ROBLength.TabIndex = 4;
            // 
            // txtGeomNFP_ChLength
            // 
            this.txtGeomNFP_ChLength.Location = new System.Drawing.Point(225, 156);
            this.txtGeomNFP_ChLength.Name = "txtGeomNFP_ChLength";
            this.txtGeomNFP_ChLength.Size = new System.Drawing.Size(100, 20);
            this.txtGeomNFP_ChLength.TabIndex = 3;
            // 
            // txtGeomNFP_LOBLength
            // 
            this.txtGeomNFP_LOBLength.Location = new System.Drawing.Point(124, 156);
            this.txtGeomNFP_LOBLength.Name = "txtGeomNFP_LOBLength";
            this.txtGeomNFP_LOBLength.Size = new System.Drawing.Size(100, 20);
            this.txtGeomNFP_LOBLength.TabIndex = 2;
            // 
            // txtGeomNFP_ChLSlope
            // 
            this.txtGeomNFP_ChLSlope.Location = new System.Drawing.Point(174, 18);
            this.txtGeomNFP_ChLSlope.Name = "txtGeomNFP_ChLSlope";
            this.txtGeomNFP_ChLSlope.Size = new System.Drawing.Size(100, 20);
            this.txtGeomNFP_ChLSlope.TabIndex = 1;
            // 
            // lblNFP_ChSlope
            // 
            this.lblNFP_ChSlope.AutoSize = true;
            this.lblNFP_ChSlope.Location = new System.Drawing.Point(35, 21);
            this.lblNFP_ChSlope.Name = "lblNFP_ChSlope";
            this.lblNFP_ChSlope.Size = new System.Drawing.Size(136, 13);
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
            this.frameNaturalXsectGrid.Location = new System.Drawing.Point(457, 7);
            this.frameNaturalXsectGrid.Name = "frameNaturalXsectGrid";
            this.frameNaturalXsectGrid.Size = new System.Drawing.Size(330, 287);
            this.frameNaturalXsectGrid.TabIndex = 6;
            // 
            // btnUndoClear
            // 
            this.btnUndoClear.Location = new System.Drawing.Point(248, 4);
            this.btnUndoClear.Name = "btnUndoClear";
            this.btnUndoClear.Size = new System.Drawing.Size(75, 23);
            this.btnUndoClear.TabIndex = 9;
            this.btnUndoClear.Text = "Undo Clear";
            this.btnUndoClear.UseVisualStyleBackColor = true;
            this.btnUndoClear.Click += new System.EventHandler(this.btnUndoClear_Click);
            // 
            // btnClearProfile
            // 
            this.btnClearProfile.Location = new System.Drawing.Point(167, 4);
            this.btnClearProfile.Name = "btnClearProfile";
            this.btnClearProfile.Size = new System.Drawing.Size(75, 23);
            this.btnClearProfile.TabIndex = 8;
            this.btnClearProfile.Text = "Clear Profile";
            this.btnClearProfile.UseVisualStyleBackColor = true;
            this.btnClearProfile.Click += new System.EventHandler(this.btnClearProfile_Click);
            // 
            // btnImportChProfile
            // 
            this.btnImportChProfile.Location = new System.Drawing.Point(4, 4);
            this.btnImportChProfile.Name = "btnImportChProfile";
            this.btnImportChProfile.Size = new System.Drawing.Size(157, 23);
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
            this.grdChProfile.Location = new System.Drawing.Point(3, 33);
            this.grdChProfile.Name = "grdChProfile";
            this.grdChProfile.Size = new System.Drawing.Size(324, 251);
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
            this.gbInputInfil.Location = new System.Drawing.Point(229, 19);
            this.gbInputInfil.Name = "gbInputInfil";
            this.gbInputInfil.Size = new System.Drawing.Size(221, 275);
            this.gbInputInfil.TabIndex = 4;
            this.gbInputInfil.TabStop = false;
            this.gbInputInfil.Text = "Infiltration Related";
            // 
            // txtInfilDrainTime
            // 
            this.txtInfilDrainTime.Location = new System.Drawing.Point(107, 198);
            this.txtInfilDrainTime.Name = "txtInfilDrainTime";
            this.txtInfilDrainTime.Size = new System.Drawing.Size(98, 20);
            this.txtInfilDrainTime.TabIndex = 14;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(22, 201);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(79, 13);
            this.label16.TabIndex = 13;
            this.label16.Text = "Drain Time (hr):";
            // 
            // txtInfilDepth
            // 
            this.txtInfilDepth.Location = new System.Drawing.Point(107, 162);
            this.txtInfilDepth.Name = "txtInfilDepth";
            this.txtInfilDepth.Size = new System.Drawing.Size(98, 20);
            this.txtInfilDepth.TabIndex = 12;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(25, 156);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(76, 26);
            this.label15.TabIndex = 11;
            this.label15.Text = "Infiltration\r\nDepth (in, cm):";
            // 
            // txtInfilRate
            // 
            this.txtInfilRate.Location = new System.Drawing.Point(107, 127);
            this.txtInfilRate.Name = "txtInfilRate";
            this.txtInfilRate.Size = new System.Drawing.Size(98, 20);
            this.txtInfilRate.TabIndex = 10;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(6, 121);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(98, 26);
            this.label14.TabIndex = 9;
            this.label14.Text = "Infiltration\r\nRate (in/hr, cm/hr):";
            // 
            // txtBackfillPore
            // 
            this.txtBackfillPore.Location = new System.Drawing.Point(107, 69);
            this.txtBackfillPore.Name = "txtBackfillPore";
            this.txtBackfillPore.Size = new System.Drawing.Size(98, 20);
            this.txtBackfillPore.TabIndex = 8;
            // 
            // lblBackfillPore
            // 
            this.lblBackfillPore.AutoSize = true;
            this.lblBackfillPore.Location = new System.Drawing.Point(6, 72);
            this.lblBackfillPore.Name = "lblBackfillPore";
            this.lblBackfillPore.Size = new System.Drawing.Size(95, 13);
            this.lblBackfillPore.TabIndex = 7;
            this.lblBackfillPore.Text = "Porosity of backfill:";
            // 
            // txtBackfillDepth
            // 
            this.txtBackfillDepth.Location = new System.Drawing.Point(107, 43);
            this.txtBackfillDepth.Name = "txtBackfillDepth";
            this.txtBackfillDepth.Size = new System.Drawing.Size(98, 20);
            this.txtBackfillDepth.TabIndex = 6;
            // 
            // lblBackfillDepth
            // 
            this.lblBackfillDepth.AutoSize = true;
            this.lblBackfillDepth.Location = new System.Drawing.Point(14, 46);
            this.lblBackfillDepth.Name = "lblBackfillDepth";
            this.lblBackfillDepth.Size = new System.Drawing.Size(87, 13);
            this.lblBackfillDepth.TabIndex = 5;
            this.lblBackfillDepth.Text = "Depth of backfill:";
            // 
            // chkBackfill
            // 
            this.chkBackfill.AutoSize = true;
            this.chkBackfill.Location = new System.Drawing.Point(7, 20);
            this.chkBackfill.Name = "chkBackfill";
            this.chkBackfill.Size = new System.Drawing.Size(169, 17);
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
            this.gbInputGeometry.Location = new System.Drawing.Point(12, 19);
            this.gbInputGeometry.Name = "gbInputGeometry";
            this.gbInputGeometry.Size = new System.Drawing.Size(211, 275);
            this.gbInputGeometry.TabIndex = 3;
            this.gbInputGeometry.TabStop = false;
            this.gbInputGeometry.Text = "Structure Geometry";
            // 
            // txtGeomDepth
            // 
            this.txtGeomDepth.Location = new System.Drawing.Point(135, 247);
            this.txtGeomDepth.Name = "txtGeomDepth";
            this.txtGeomDepth.Size = new System.Drawing.Size(70, 20);
            this.txtGeomDepth.TabIndex = 19;
            // 
            // txtGeomWidth
            // 
            this.txtGeomWidth.Location = new System.Drawing.Point(135, 221);
            this.txtGeomWidth.Name = "txtGeomWidth";
            this.txtGeomWidth.Size = new System.Drawing.Size(70, 20);
            this.txtGeomWidth.TabIndex = 18;
            // 
            // txtGeomSideSlope
            // 
            this.txtGeomSideSlope.Location = new System.Drawing.Point(135, 195);
            this.txtGeomSideSlope.Name = "txtGeomSideSlope";
            this.txtGeomSideSlope.Size = new System.Drawing.Size(70, 20);
            this.txtGeomSideSlope.TabIndex = 17;
            // 
            // txtGeomTopWidth
            // 
            this.txtGeomTopWidth.Location = new System.Drawing.Point(135, 169);
            this.txtGeomTopWidth.Name = "txtGeomTopWidth";
            this.txtGeomTopWidth.Size = new System.Drawing.Size(70, 20);
            this.txtGeomTopWidth.TabIndex = 16;
            // 
            // txtGeomMaxDepth
            // 
            this.txtGeomMaxDepth.Location = new System.Drawing.Point(135, 143);
            this.txtGeomMaxDepth.Name = "txtGeomMaxDepth";
            this.txtGeomMaxDepth.Size = new System.Drawing.Size(70, 20);
            this.txtGeomMaxDepth.TabIndex = 15;
            // 
            // lblGeomDepth
            // 
            this.lblGeomDepth.AutoSize = true;
            this.lblGeomDepth.Location = new System.Drawing.Point(90, 250);
            this.lblGeomDepth.Name = "lblGeomDepth";
            this.lblGeomDepth.Size = new System.Drawing.Size(39, 13);
            this.lblGeomDepth.TabIndex = 14;
            this.lblGeomDepth.Text = "Depth:";
            // 
            // lblGeomWidth
            // 
            this.lblGeomWidth.AutoSize = true;
            this.lblGeomWidth.Location = new System.Drawing.Point(91, 224);
            this.lblGeomWidth.Name = "lblGeomWidth";
            this.lblGeomWidth.Size = new System.Drawing.Size(38, 13);
            this.lblGeomWidth.TabIndex = 13;
            this.lblGeomWidth.Text = "Width:";
            // 
            // lblGeomSideSlope
            // 
            this.lblGeomSideSlope.AutoSize = true;
            this.lblGeomSideSlope.Location = new System.Drawing.Point(41, 198);
            this.lblGeomSideSlope.Name = "lblGeomSideSlope";
            this.lblGeomSideSlope.Size = new System.Drawing.Size(88, 13);
            this.lblGeomSideSlope.TabIndex = 12;
            this.lblGeomSideSlope.Text = "Side Slope (H:V):";
            // 
            // lblGeomTopWidth
            // 
            this.lblGeomTopWidth.AutoSize = true;
            this.lblGeomTopWidth.Location = new System.Drawing.Point(69, 172);
            this.lblGeomTopWidth.Name = "lblGeomTopWidth";
            this.lblGeomTopWidth.Size = new System.Drawing.Size(60, 13);
            this.lblGeomTopWidth.TabIndex = 11;
            this.lblGeomTopWidth.Text = "Top Width:";
            // 
            // lblGeomMaxDepth
            // 
            this.lblGeomMaxDepth.AutoSize = true;
            this.lblGeomMaxDepth.Location = new System.Drawing.Point(43, 146);
            this.lblGeomMaxDepth.Name = "lblGeomMaxDepth";
            this.lblGeomMaxDepth.Size = new System.Drawing.Size(86, 13);
            this.lblGeomMaxDepth.TabIndex = 10;
            this.lblGeomMaxDepth.Text = "Maximum Depth:";
            // 
            // txtGeomDiam
            // 
            this.txtGeomDiam.Location = new System.Drawing.Point(135, 117);
            this.txtGeomDiam.Name = "txtGeomDiam";
            this.txtGeomDiam.Size = new System.Drawing.Size(70, 20);
            this.txtGeomDiam.TabIndex = 9;
            // 
            // lblGeomDiam
            // 
            this.lblGeomDiam.AutoSize = true;
            this.lblGeomDiam.Location = new System.Drawing.Point(77, 120);
            this.lblGeomDiam.Name = "lblGeomDiam";
            this.lblGeomDiam.Size = new System.Drawing.Size(52, 13);
            this.lblGeomDiam.TabIndex = 8;
            this.lblGeomDiam.Text = "Diameter:";
            // 
            // txtGeomHInc
            // 
            this.txtGeomHInc.Location = new System.Drawing.Point(135, 91);
            this.txtGeomHInc.Name = "txtGeomHInc";
            this.txtGeomHInc.Size = new System.Drawing.Size(70, 20);
            this.txtGeomHInc.TabIndex = 7;
            // 
            // txtGeomMannN
            // 
            this.txtGeomMannN.Location = new System.Drawing.Point(135, 39);
            this.txtGeomMannN.Name = "txtGeomMannN";
            this.txtGeomMannN.Size = new System.Drawing.Size(70, 20);
            this.txtGeomMannN.TabIndex = 6;
            // 
            // txtGeomLSlope
            // 
            this.txtGeomLSlope.Location = new System.Drawing.Point(135, 65);
            this.txtGeomLSlope.Name = "txtGeomLSlope";
            this.txtGeomLSlope.Size = new System.Drawing.Size(70, 20);
            this.txtGeomLSlope.TabIndex = 5;
            // 
            // txtGeomLength
            // 
            this.txtGeomLength.Location = new System.Drawing.Point(135, 13);
            this.txtGeomLength.Name = "txtGeomLength";
            this.txtGeomLength.Size = new System.Drawing.Size(70, 20);
            this.txtGeomLength.TabIndex = 4;
            // 
            // lblGeomHInc
            // 
            this.lblGeomHInc.AutoSize = true;
            this.lblGeomHInc.Location = new System.Drawing.Point(38, 94);
            this.lblGeomHInc.Name = "lblGeomHInc";
            this.lblGeomHInc.Size = new System.Drawing.Size(91, 13);
            this.lblGeomHInc.TabIndex = 3;
            this.lblGeomHInc.Text = "Height Increment:";
            // 
            // lblGeomLSlope
            // 
            this.lblGeomLSlope.AutoSize = true;
            this.lblGeomLSlope.Location = new System.Drawing.Point(6, 68);
            this.lblGeomLSlope.Name = "lblGeomLSlope";
            this.lblGeomLSlope.Size = new System.Drawing.Size(123, 13);
            this.lblGeomLSlope.TabIndex = 2;
            this.lblGeomLSlope.Text = "Longitudinal Slope (ft/ft):";
            // 
            // lblGeomMannN
            // 
            this.lblGeomMannN.AutoSize = true;
            this.lblGeomMannN.Location = new System.Drawing.Point(60, 42);
            this.lblGeomMannN.Name = "lblGeomMannN";
            this.lblGeomMannN.Size = new System.Drawing.Size(69, 13);
            this.lblGeomMannN.TabIndex = 1;
            this.lblGeomMannN.Text = "Manning\'s N:";
            // 
            // lblGeomLength
            // 
            this.lblGeomLength.AutoSize = true;
            this.lblGeomLength.Location = new System.Drawing.Point(86, 16);
            this.lblGeomLength.Name = "lblGeomLength";
            this.lblGeomLength.Size = new System.Drawing.Size(43, 13);
            this.lblGeomLength.TabIndex = 0;
            this.lblGeomLength.Text = "Length:";
            // 
            // bmpSketch1
            // 
            this.bmpSketch1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bmpSketch1.Location = new System.Drawing.Point(3, 42);
            this.bmpSketch1.Name = "bmpSketch1";
            this.bmpSketch1.Size = new System.Drawing.Size(787, 77);
            this.bmpSketch1.TabIndex = 6;
            this.bmpSketch1.MouseHover += new System.EventHandler(this.bmpSketch1_MouseHover);
            // 
            // mainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(793, 530);
            this.Controls.Add(this.gbInputData);
            this.Controls.Add(this.gbChannel);
            this.Controls.Add(this.btnHelp);
            this.Controls.Add(this.btnCalcFtable);
            this.Controls.Add(this.frameTop);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
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