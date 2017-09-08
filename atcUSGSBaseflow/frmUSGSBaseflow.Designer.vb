<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUSGSBaseflow
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUSGSBaseflow))
        Me.txtDrainageArea = New System.Windows.Forms.TextBox()
        Me.lblDrainageAreaUnits = New System.Windows.Forms.Label()
        Me.btnExamineData = New System.Windows.Forms.Button()
        Me.gbDates = New System.Windows.Forms.GroupBox()
        Me.lblAnalysisDates = New System.Windows.Forms.Label()
        Me.lblPeriodOfRecord = New System.Windows.Forms.Label()
        Me.txtEndDateUser = New System.Windows.Forms.TextBox()
        Me.txtStartDateUser = New System.Windows.Forms.TextBox()
        Me.txtDataEnd = New System.Windows.Forms.TextBox()
        Me.txtDataStart = New System.Windows.Forms.TextBox()
        Me.lblDataEnd = New System.Windows.Forms.Label()
        Me.lblDataStart = New System.Windows.Forms.Label()
        Me.toolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.chkMethodBFIStandard = New System.Windows.Forms.CheckBox()
        Me.chkMethodBFIModified = New System.Windows.Forms.CheckBox()
        Me.chkMethodBFLOW = New System.Windows.Forms.CheckBox()
        Me.chkMethodTwoPRDF = New System.Windows.Forms.CheckBox()
        Me.txtDFParamBFImax = New System.Windows.Forms.TextBox()
        Me.txtDFParamRC = New System.Windows.Forms.TextBox()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.mnuFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuFileSelectData = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuOutput = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuOutputASCII = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuGraphBF = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuGraphTimeseries = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuGraphDuration = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuGraphCDistPlot = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAnalysis = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuHelp = New System.Windows.Forms.ToolStripMenuItem()
        Me.txtOutputRootName = New System.Windows.Forms.TextBox()
        Me.lblBaseFilename = New System.Windows.Forms.Label()
        Me.gbTextOutput = New System.Windows.Forms.GroupBox()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnWriteASCIIOutput = New System.Windows.Forms.Button()
        Me.chkTabDelimited = New System.Windows.Forms.CheckBox()
        Me.txtOutputDir = New System.Windows.Forms.TextBox()
        Me.lblOutputDir = New System.Windows.Forms.Label()
        Me.btnGraphCDist = New System.Windows.Forms.Button()
        Me.btnGraphDurationPUA = New System.Windows.Forms.Button()
        Me.btnGraphDuration = New System.Windows.Forms.Button()
        Me.btnGraphTimeseries = New System.Windows.Forms.Button()
        Me.gbBFMethods = New System.Windows.Forms.GroupBox()
        Me.chkMethodPART = New System.Windows.Forms.CheckBox()
        Me.chkMethodHySEPSlide = New System.Windows.Forms.CheckBox()
        Me.chkMethodHySEPLocMin = New System.Windows.Forms.CheckBox()
        Me.chkMethodHySEPFixed = New System.Windows.Forms.CheckBox()
        Me.gbGraph = New System.Windows.Forms.GroupBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.gbBFI = New System.Windows.Forms.GroupBox()
        Me.txtK = New System.Windows.Forms.TextBox()
        Me.txtF = New System.Windows.Forms.TextBox()
        Me.txtN = New System.Windows.Forms.TextBox()
        Me.lblK = New System.Windows.Forms.Label()
        Me.lblF = New System.Windows.Forms.Label()
        Me.lblN = New System.Windows.Forms.Label()
        Me.rdoBFIReportbyWaterYear = New System.Windows.Forms.RadioButton()
        Me.rdoBFIReportbyCalendarYear = New System.Windows.Forms.RadioButton()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.gbDFParam = New System.Windows.Forms.GroupBox()
        Me.txt2PDefaultNotice = New System.Windows.Forms.TextBox()
        Me.rdo2PDefault = New System.Windows.Forms.RadioButton()
        Me.rdo2PSpecify = New System.Windows.Forms.RadioButton()
        Me.lblTwoParam = New System.Windows.Forms.Label()
        Me.lblBFImax = New System.Windows.Forms.Label()
        Me.txtDFParamBeta = New System.Windows.Forms.TextBox()
        Me.lblBeta = New System.Windows.Forms.Label()
        Me.lblRC = New System.Windows.Forms.Label()
        Me.gbduration = New System.Windows.Forms.GroupBox()
        Me.rdoDurationYes = New System.Windows.Forms.RadioButton()
        Me.rdoDurationNo = New System.Windows.Forms.RadioButton()
        Me.gbNotes = New System.Windows.Forms.GroupBox()
        Me.txtDurationNote = New System.Windows.Forms.TextBox()
        Me.txtNotes = New System.Windows.Forms.TextBox()
        Me.gbDates.SuspendLayout()
        Me.MenuStrip1.SuspendLayout()
        Me.gbTextOutput.SuspendLayout()
        Me.gbBFMethods.SuspendLayout()
        Me.gbGraph.SuspendLayout()
        Me.gbBFI.SuspendLayout()
        Me.gbDFParam.SuspendLayout()
        Me.gbduration.SuspendLayout()
        Me.gbNotes.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtDrainageArea
        '
        Me.txtDrainageArea.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDrainageArea.Location = New System.Drawing.Point(211, 37)
        Me.txtDrainageArea.Name = "txtDrainageArea"
        Me.txtDrainageArea.Size = New System.Drawing.Size(138, 20)
        Me.txtDrainageArea.TabIndex = 6
        Me.txtDrainageArea.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblDrainageAreaUnits
        '
        Me.lblDrainageAreaUnits.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblDrainageAreaUnits.AutoSize = True
        Me.lblDrainageAreaUnits.Location = New System.Drawing.Point(354, 39)
        Me.lblDrainageAreaUnits.Name = "lblDrainageAreaUnits"
        Me.lblDrainageAreaUnits.Size = New System.Drawing.Size(31, 13)
        Me.lblDrainageAreaUnits.TabIndex = 4
        Me.lblDrainageAreaUnits.Text = "sq mi"
        '
        'btnExamineData
        '
        Me.btnExamineData.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnExamineData.Location = New System.Drawing.Point(68, 85)
        Me.btnExamineData.Name = "btnExamineData"
        Me.btnExamineData.Size = New System.Drawing.Size(94, 23)
        Me.btnExamineData.TabIndex = 10
        Me.btnExamineData.Text = "Examine Data"
        Me.btnExamineData.UseVisualStyleBackColor = True
        '
        'gbDates
        '
        Me.gbDates.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbDates.Controls.Add(Me.lblAnalysisDates)
        Me.gbDates.Controls.Add(Me.lblPeriodOfRecord)
        Me.gbDates.Controls.Add(Me.txtEndDateUser)
        Me.gbDates.Controls.Add(Me.txtStartDateUser)
        Me.gbDates.Controls.Add(Me.btnExamineData)
        Me.gbDates.Controls.Add(Me.txtDataEnd)
        Me.gbDates.Controls.Add(Me.txtDataStart)
        Me.gbDates.Controls.Add(Me.lblDataEnd)
        Me.gbDates.Controls.Add(Me.lblDataStart)
        Me.gbDates.Location = New System.Drawing.Point(12, 363)
        Me.gbDates.Name = "gbDates"
        Me.gbDates.Size = New System.Drawing.Size(375, 114)
        Me.gbDates.TabIndex = 7
        Me.gbDates.TabStop = False
        Me.gbDates.Text = "Define Analysis Dates"
        '
        'lblAnalysisDates
        '
        Me.lblAnalysisDates.AutoSize = True
        Me.lblAnalysisDates.Location = New System.Drawing.Point(191, 15)
        Me.lblAnalysisDates.Name = "lblAnalysisDates"
        Me.lblAnalysisDates.Size = New System.Drawing.Size(76, 13)
        Me.lblAnalysisDates.TabIndex = 12
        Me.lblAnalysisDates.Text = "Analysis Dates"
        '
        'lblPeriodOfRecord
        '
        Me.lblPeriodOfRecord.AutoSize = True
        Me.lblPeriodOfRecord.Location = New System.Drawing.Point(65, 15)
        Me.lblPeriodOfRecord.Name = "lblPeriodOfRecord"
        Me.lblPeriodOfRecord.Size = New System.Drawing.Size(87, 13)
        Me.lblPeriodOfRecord.TabIndex = 11
        Me.lblPeriodOfRecord.Text = "Period of Record"
        '
        'txtEndDateUser
        '
        Me.txtEndDateUser.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtEndDateUser.Location = New System.Drawing.Point(194, 58)
        Me.txtEndDateUser.Name = "txtEndDateUser"
        Me.txtEndDateUser.Size = New System.Drawing.Size(175, 20)
        Me.txtEndDateUser.TabIndex = 9
        Me.toolTip1.SetToolTip(Me.txtEndDateUser, "User-specified ending date: yyyy/mm/dd")
        '
        'txtStartDateUser
        '
        Me.txtStartDateUser.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtStartDateUser.Location = New System.Drawing.Point(194, 31)
        Me.txtStartDateUser.Name = "txtStartDateUser"
        Me.txtStartDateUser.Size = New System.Drawing.Size(175, 20)
        Me.txtStartDateUser.TabIndex = 8
        Me.toolTip1.SetToolTip(Me.txtStartDateUser, "User-specified starting date: yyyy/mm/dd")
        '
        'txtDataEnd
        '
        Me.txtDataEnd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtDataEnd.Location = New System.Drawing.Point(68, 59)
        Me.txtDataEnd.Name = "txtDataEnd"
        Me.txtDataEnd.ReadOnly = True
        Me.txtDataEnd.Size = New System.Drawing.Size(119, 20)
        Me.txtDataEnd.TabIndex = 3
        '
        'txtDataStart
        '
        Me.txtDataStart.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtDataStart.Location = New System.Drawing.Point(68, 32)
        Me.txtDataStart.Name = "txtDataStart"
        Me.txtDataStart.ReadOnly = True
        Me.txtDataStart.Size = New System.Drawing.Size(119, 20)
        Me.txtDataStart.TabIndex = 2
        '
        'lblDataEnd
        '
        Me.lblDataEnd.AutoSize = True
        Me.lblDataEnd.Location = New System.Drawing.Point(6, 58)
        Me.lblDataEnd.Name = "lblDataEnd"
        Me.lblDataEnd.Size = New System.Drawing.Size(52, 13)
        Me.lblDataEnd.TabIndex = 1
        Me.lblDataEnd.Text = "Data End"
        '
        'lblDataStart
        '
        Me.lblDataStart.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblDataStart.AutoSize = True
        Me.lblDataStart.Location = New System.Drawing.Point(6, 32)
        Me.lblDataStart.Name = "lblDataStart"
        Me.lblDataStart.Size = New System.Drawing.Size(55, 13)
        Me.lblDataStart.TabIndex = 0
        Me.lblDataStart.Text = "Data Start"
        '
        'chkMethodBFIStandard
        '
        Me.chkMethodBFIStandard.AutoSize = True
        Me.chkMethodBFIStandard.Location = New System.Drawing.Point(6, 113)
        Me.chkMethodBFIStandard.Name = "chkMethodBFIStandard"
        Me.chkMethodBFIStandard.Size = New System.Drawing.Size(88, 17)
        Me.chkMethodBFIStandard.TabIndex = 6
        Me.chkMethodBFIStandard.Text = "BFI-Standard"
        Me.toolTip1.SetToolTip(Me.chkMethodBFIStandard, "Institute of Hydrology Method (Standard)")
        Me.chkMethodBFIStandard.UseVisualStyleBackColor = True
        '
        'chkMethodBFIModified
        '
        Me.chkMethodBFIModified.AutoSize = True
        Me.chkMethodBFIModified.Location = New System.Drawing.Point(6, 136)
        Me.chkMethodBFIModified.Name = "chkMethodBFIModified"
        Me.chkMethodBFIModified.Size = New System.Drawing.Size(85, 17)
        Me.chkMethodBFIModified.TabIndex = 7
        Me.chkMethodBFIModified.Text = "BFI-Modified"
        Me.toolTip1.SetToolTip(Me.chkMethodBFIModified, "Modified Method")
        Me.chkMethodBFIModified.UseVisualStyleBackColor = True
        '
        'chkMethodBFLOW
        '
        Me.chkMethodBFLOW.AutoSize = True
        Me.chkMethodBFLOW.Location = New System.Drawing.Point(6, 159)
        Me.chkMethodBFLOW.Name = "chkMethodBFLOW"
        Me.chkMethodBFLOW.Size = New System.Drawing.Size(96, 17)
        Me.chkMethodBFLOW.TabIndex = 8
        Me.chkMethodBFLOW.Text = "DF-One Param"
        Me.toolTip1.SetToolTip(Me.chkMethodBFLOW, "One Parameter Digital Filter (BFLOW method)")
        Me.chkMethodBFLOW.UseVisualStyleBackColor = True
        '
        'chkMethodTwoPRDF
        '
        Me.chkMethodTwoPRDF.AutoSize = True
        Me.chkMethodTwoPRDF.Location = New System.Drawing.Point(6, 182)
        Me.chkMethodTwoPRDF.Name = "chkMethodTwoPRDF"
        Me.chkMethodTwoPRDF.Size = New System.Drawing.Size(97, 17)
        Me.chkMethodTwoPRDF.TabIndex = 9
        Me.chkMethodTwoPRDF.Text = "DF-Two Param"
        Me.toolTip1.SetToolTip(Me.chkMethodTwoPRDF, "Two Parameter Digital Filter (Eckhardt method)")
        Me.chkMethodTwoPRDF.UseVisualStyleBackColor = True
        '
        'txtDFParamBFImax
        '
        Me.txtDFParamBFImax.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDFParamBFImax.Location = New System.Drawing.Point(195, 99)
        Me.txtDFParamBFImax.Name = "txtDFParamBFImax"
        Me.txtDFParamBFImax.Size = New System.Drawing.Size(53, 20)
        Me.txtDFParamBFImax.TabIndex = 11
        Me.txtDFParamBFImax.Text = "0.8"
        Me.txtDFParamBFImax.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.toolTip1.SetToolTip(Me.txtDFParamBFImax, "a common value is 0.8")
        '
        'txtDFParamRC
        '
        Me.txtDFParamRC.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDFParamRC.Location = New System.Drawing.Point(195, 73)
        Me.txtDFParamRC.Name = "txtDFParamRC"
        Me.txtDFParamRC.Size = New System.Drawing.Size(54, 20)
        Me.txtDFParamRC.TabIndex = 10
        Me.txtDFParamRC.Text = "0.978"
        Me.txtDFParamRC.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.toolTip1.SetToolTip(Me.txtDFParamRC, "a common value is 0.925")
        '
        'MenuStrip1
        '
        Me.MenuStrip1.BackColor = System.Drawing.SystemColors.Window
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuFile, Me.mnuAnalysis, Me.mnuHelp})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(399, 24)
        Me.MenuStrip1.TabIndex = 28
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'mnuFile
        '
        Me.mnuFile.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuFileSelectData, Me.mnuOutput, Me.mnuGraphBF})
        Me.mnuFile.Name = "mnuFile"
        Me.mnuFile.Size = New System.Drawing.Size(37, 20)
        Me.mnuFile.Text = "File"
        '
        'mnuFileSelectData
        '
        Me.mnuFileSelectData.Name = "mnuFileSelectData"
        Me.mnuFileSelectData.Size = New System.Drawing.Size(132, 22)
        Me.mnuFileSelectData.Text = "Select Data"
        '
        'mnuOutput
        '
        Me.mnuOutput.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuOutputASCII})
        Me.mnuOutput.Name = "mnuOutput"
        Me.mnuOutput.Size = New System.Drawing.Size(132, 22)
        Me.mnuOutput.Text = "Output"
        '
        'mnuOutputASCII
        '
        Me.mnuOutputASCII.Name = "mnuOutputASCII"
        Me.mnuOutputASCII.Size = New System.Drawing.Size(102, 22)
        Me.mnuOutputASCII.Text = "ASCII"
        '
        'mnuGraphBF
        '
        Me.mnuGraphBF.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuGraphTimeseries, Me.mnuGraphDuration, Me.mnuGraphCDistPlot})
        Me.mnuGraphBF.Name = "mnuGraphBF"
        Me.mnuGraphBF.Size = New System.Drawing.Size(132, 22)
        Me.mnuGraphBF.Text = "Graph"
        '
        'mnuGraphTimeseries
        '
        Me.mnuGraphTimeseries.Name = "mnuGraphTimeseries"
        Me.mnuGraphTimeseries.Size = New System.Drawing.Size(146, 22)
        Me.mnuGraphTimeseries.Text = "TimeSeries"
        '
        'mnuGraphDuration
        '
        Me.mnuGraphDuration.Name = "mnuGraphDuration"
        Me.mnuGraphDuration.Size = New System.Drawing.Size(146, 22)
        Me.mnuGraphDuration.Text = "Duration"
        '
        'mnuGraphCDistPlot
        '
        Me.mnuGraphCDistPlot.Name = "mnuGraphCDistPlot"
        Me.mnuGraphCDistPlot.Size = New System.Drawing.Size(146, 22)
        Me.mnuGraphCDistPlot.Text = "Cummulative"
        '
        'mnuAnalysis
        '
        Me.mnuAnalysis.Name = "mnuAnalysis"
        Me.mnuAnalysis.Size = New System.Drawing.Size(62, 20)
        Me.mnuAnalysis.Text = "Analysis"
        '
        'mnuHelp
        '
        Me.mnuHelp.Name = "mnuHelp"
        Me.mnuHelp.Size = New System.Drawing.Size(44, 20)
        Me.mnuHelp.Text = "Help"
        '
        'txtOutputRootName
        '
        Me.txtOutputRootName.Location = New System.Drawing.Point(114, 49)
        Me.txtOutputRootName.Name = "txtOutputRootName"
        Me.txtOutputRootName.Size = New System.Drawing.Size(121, 20)
        Me.txtOutputRootName.TabIndex = 13
        '
        'lblBaseFilename
        '
        Me.lblBaseFilename.AutoSize = True
        Me.lblBaseFilename.Location = New System.Drawing.Point(6, 52)
        Me.lblBaseFilename.Name = "lblBaseFilename"
        Me.lblBaseFilename.Size = New System.Drawing.Size(106, 13)
        Me.lblBaseFilename.TabIndex = 30
        Me.lblBaseFilename.Text = "Base output filename"
        '
        'gbTextOutput
        '
        Me.gbTextOutput.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbTextOutput.Controls.Add(Me.btnSave)
        Me.gbTextOutput.Controls.Add(Me.btnWriteASCIIOutput)
        Me.gbTextOutput.Controls.Add(Me.chkTabDelimited)
        Me.gbTextOutput.Controls.Add(Me.txtOutputDir)
        Me.gbTextOutput.Controls.Add(Me.lblOutputDir)
        Me.gbTextOutput.Controls.Add(Me.txtOutputRootName)
        Me.gbTextOutput.Controls.Add(Me.lblBaseFilename)
        Me.gbTextOutput.Location = New System.Drawing.Point(12, 483)
        Me.gbTextOutput.Name = "gbTextOutput"
        Me.gbTextOutput.Size = New System.Drawing.Size(375, 111)
        Me.gbTextOutput.TabIndex = 11
        Me.gbTextOutput.TabStop = False
        Me.gbTextOutput.Text = "Text Output"
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(241, 47)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(75, 23)
        Me.btnSave.TabIndex = 32
        Me.btnSave.Text = "SaveRDB"
        Me.btnSave.UseVisualStyleBackColor = True
        Me.btnSave.Visible = False
        '
        'btnWriteASCIIOutput
        '
        Me.btnWriteASCIIOutput.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnWriteASCIIOutput.Location = New System.Drawing.Point(161, 82)
        Me.btnWriteASCIIOutput.Name = "btnWriteASCIIOutput"
        Me.btnWriteASCIIOutput.Size = New System.Drawing.Size(208, 23)
        Me.btnWriteASCIIOutput.TabIndex = 15
        Me.btnWriteASCIIOutput.Text = "Run Base-Flow Separation Program(s)"
        Me.btnWriteASCIIOutput.UseVisualStyleBackColor = True
        '
        'chkTabDelimited
        '
        Me.chkTabDelimited.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkTabDelimited.AutoSize = True
        Me.chkTabDelimited.Location = New System.Drawing.Point(22, 88)
        Me.chkTabDelimited.Name = "chkTabDelimited"
        Me.chkTabDelimited.Size = New System.Drawing.Size(91, 17)
        Me.chkTabDelimited.TabIndex = 14
        Me.chkTabDelimited.Text = "Tab-Delimited"
        Me.chkTabDelimited.UseVisualStyleBackColor = True
        Me.chkTabDelimited.Visible = False
        '
        'txtOutputDir
        '
        Me.txtOutputDir.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtOutputDir.Location = New System.Drawing.Point(78, 20)
        Me.txtOutputDir.Name = "txtOutputDir"
        Me.txtOutputDir.Size = New System.Drawing.Size(291, 20)
        Me.txtOutputDir.TabIndex = 12
        '
        'lblOutputDir
        '
        Me.lblOutputDir.AutoSize = True
        Me.lblOutputDir.Location = New System.Drawing.Point(8, 23)
        Me.lblOutputDir.Name = "lblOutputDir"
        Me.lblOutputDir.Size = New System.Drawing.Size(68, 13)
        Me.lblOutputDir.TabIndex = 31
        Me.lblOutputDir.Text = "Output folder"
        '
        'btnGraphCDist
        '
        Me.btnGraphCDist.Location = New System.Drawing.Point(115, 19)
        Me.btnGraphCDist.Name = "btnGraphCDist"
        Me.btnGraphCDist.Size = New System.Drawing.Size(163, 23)
        Me.btnGraphCDist.TabIndex = 18
        Me.btnGraphCDist.Text = "Cummulative Distribution"
        Me.btnGraphCDist.UseVisualStyleBackColor = True
        '
        'btnGraphDurationPUA
        '
        Me.btnGraphDurationPUA.Location = New System.Drawing.Point(115, 48)
        Me.btnGraphDurationPUA.Name = "btnGraphDurationPUA"
        Me.btnGraphDurationPUA.Size = New System.Drawing.Size(163, 23)
        Me.btnGraphDurationPUA.TabIndex = 20
        Me.btnGraphDurationPUA.Text = "Flow Duration per unit area"
        Me.btnGraphDurationPUA.UseVisualStyleBackColor = True
        '
        'btnGraphDuration
        '
        Me.btnGraphDuration.Location = New System.Drawing.Point(6, 48)
        Me.btnGraphDuration.Name = "btnGraphDuration"
        Me.btnGraphDuration.Size = New System.Drawing.Size(103, 23)
        Me.btnGraphDuration.TabIndex = 19
        Me.btnGraphDuration.Text = "Flow Duration"
        Me.btnGraphDuration.UseVisualStyleBackColor = True
        '
        'btnGraphTimeseries
        '
        Me.btnGraphTimeseries.Location = New System.Drawing.Point(6, 19)
        Me.btnGraphTimeseries.Name = "btnGraphTimeseries"
        Me.btnGraphTimeseries.Size = New System.Drawing.Size(103, 23)
        Me.btnGraphTimeseries.TabIndex = 17
        Me.btnGraphTimeseries.Text = "Time Series"
        Me.btnGraphTimeseries.UseVisualStyleBackColor = True
        '
        'gbBFMethods
        '
        Me.gbBFMethods.Controls.Add(Me.chkMethodTwoPRDF)
        Me.gbBFMethods.Controls.Add(Me.chkMethodBFLOW)
        Me.gbBFMethods.Controls.Add(Me.chkMethodPART)
        Me.gbBFMethods.Controls.Add(Me.chkMethodBFIModified)
        Me.gbBFMethods.Controls.Add(Me.chkMethodHySEPSlide)
        Me.gbBFMethods.Controls.Add(Me.chkMethodBFIStandard)
        Me.gbBFMethods.Controls.Add(Me.chkMethodHySEPLocMin)
        Me.gbBFMethods.Controls.Add(Me.chkMethodHySEPFixed)
        Me.gbBFMethods.Location = New System.Drawing.Point(12, 30)
        Me.gbBFMethods.Name = "gbBFMethods"
        Me.gbBFMethods.Size = New System.Drawing.Size(112, 327)
        Me.gbBFMethods.TabIndex = 0
        Me.gbBFMethods.TabStop = False
        Me.gbBFMethods.Text = "Select Method(s)"
        '
        'chkMethodPART
        '
        Me.chkMethodPART.AutoSize = True
        Me.chkMethodPART.Location = New System.Drawing.Point(6, 89)
        Me.chkMethodPART.Name = "chkMethodPART"
        Me.chkMethodPART.Size = New System.Drawing.Size(55, 17)
        Me.chkMethodPART.TabIndex = 4
        Me.chkMethodPART.Text = "PART"
        Me.chkMethodPART.UseVisualStyleBackColor = True
        '
        'chkMethodHySEPSlide
        '
        Me.chkMethodHySEPSlide.AutoSize = True
        Me.chkMethodHySEPSlide.Location = New System.Drawing.Point(6, 66)
        Me.chkMethodHySEPSlide.Name = "chkMethodHySEPSlide"
        Me.chkMethodHySEPSlide.Size = New System.Drawing.Size(86, 17)
        Me.chkMethodHySEPSlide.TabIndex = 3
        Me.chkMethodHySEPSlide.Text = "HySEP-Slide"
        Me.chkMethodHySEPSlide.UseVisualStyleBackColor = True
        '
        'chkMethodHySEPLocMin
        '
        Me.chkMethodHySEPLocMin.AutoSize = True
        Me.chkMethodHySEPLocMin.Location = New System.Drawing.Point(6, 43)
        Me.chkMethodHySEPLocMin.Name = "chkMethodHySEPLocMin"
        Me.chkMethodHySEPLocMin.Size = New System.Drawing.Size(98, 17)
        Me.chkMethodHySEPLocMin.TabIndex = 2
        Me.chkMethodHySEPLocMin.Text = "HySEP-LocMin"
        Me.chkMethodHySEPLocMin.UseVisualStyleBackColor = True
        '
        'chkMethodHySEPFixed
        '
        Me.chkMethodHySEPFixed.AutoSize = True
        Me.chkMethodHySEPFixed.Location = New System.Drawing.Point(6, 20)
        Me.chkMethodHySEPFixed.Name = "chkMethodHySEPFixed"
        Me.chkMethodHySEPFixed.Size = New System.Drawing.Size(88, 17)
        Me.chkMethodHySEPFixed.TabIndex = 1
        Me.chkMethodHySEPFixed.Text = "HySEP-Fixed"
        Me.chkMethodHySEPFixed.UseVisualStyleBackColor = True
        '
        'gbGraph
        '
        Me.gbGraph.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbGraph.Controls.Add(Me.btnGraphDurationPUA)
        Me.gbGraph.Controls.Add(Me.btnGraphCDist)
        Me.gbGraph.Controls.Add(Me.btnGraphTimeseries)
        Me.gbGraph.Controls.Add(Me.btnGraphDuration)
        Me.gbGraph.Location = New System.Drawing.Point(12, 600)
        Me.gbGraph.Name = "gbGraph"
        Me.gbGraph.Size = New System.Drawing.Size(375, 77)
        Me.gbGraph.TabIndex = 16
        Me.gbGraph.TabStop = False
        Me.gbGraph.Text = "Display Graph"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(130, 39)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(75, 13)
        Me.Label1.TabIndex = 29
        Me.Label1.Text = "Drainage Area"
        '
        'gbBFI
        '
        Me.gbBFI.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbBFI.Controls.Add(Me.txtK)
        Me.gbBFI.Controls.Add(Me.txtF)
        Me.gbBFI.Controls.Add(Me.txtN)
        Me.gbBFI.Controls.Add(Me.lblK)
        Me.gbBFI.Controls.Add(Me.lblF)
        Me.gbBFI.Controls.Add(Me.lblN)
        Me.gbBFI.Location = New System.Drawing.Point(131, 132)
        Me.gbBFI.Name = "gbBFI"
        Me.gbBFI.Size = New System.Drawing.Size(254, 91)
        Me.gbBFI.TabIndex = 30
        Me.gbBFI.TabStop = False
        Me.gbBFI.Text = "BFI Parameters"
        '
        'txtK
        '
        Me.txtK.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtK.Location = New System.Drawing.Point(170, 66)
        Me.txtK.Name = "txtK"
        Me.txtK.Size = New System.Drawing.Size(79, 20)
        Me.txtK.TabIndex = 5
        Me.txtK.Text = "0.97915"
        Me.txtK.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtF
        '
        Me.txtF.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtF.Location = New System.Drawing.Point(170, 42)
        Me.txtF.Name = "txtF"
        Me.txtF.Size = New System.Drawing.Size(79, 20)
        Me.txtF.TabIndex = 4
        Me.txtF.Text = "0.9"
        Me.txtF.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtN
        '
        Me.txtN.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtN.Location = New System.Drawing.Point(170, 17)
        Me.txtN.Name = "txtN"
        Me.txtN.Size = New System.Drawing.Size(79, 20)
        Me.txtN.TabIndex = 3
        Me.txtN.Text = "5"
        Me.txtN.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblK
        '
        Me.lblK.AutoSize = True
        Me.lblK.Location = New System.Drawing.Point(33, 69)
        Me.lblK.Name = "lblK"
        Me.lblK.Size = New System.Drawing.Size(131, 13)
        Me.lblK.TabIndex = 2
        Me.lblK.Text = "Daily Recession Index (K’)"
        '
        'lblF
        '
        Me.lblF.AutoSize = True
        Me.lblF.Location = New System.Drawing.Point(24, 44)
        Me.lblF.Name = "lblF"
        Me.lblF.Size = New System.Drawing.Size(139, 13)
        Me.lblF.TabIndex = 1
        Me.lblF.Text = "Turning Point Test Factor(F)"
        '
        'lblN
        '
        Me.lblN.AutoSize = True
        Me.lblN.Location = New System.Drawing.Point(37, 20)
        Me.lblN.Name = "lblN"
        Me.lblN.Size = New System.Drawing.Size(126, 13)
        Me.lblN.TabIndex = 0
        Me.lblN.Text = "Partition Length (N, days)"
        '
        'rdoBFIReportbyWaterYear
        '
        Me.rdoBFIReportbyWaterYear.AutoSize = True
        Me.rdoBFIReportbyWaterYear.Location = New System.Drawing.Point(299, 63)
        Me.rdoBFIReportbyWaterYear.Name = "rdoBFIReportbyWaterYear"
        Me.rdoBFIReportbyWaterYear.Size = New System.Drawing.Size(79, 17)
        Me.rdoBFIReportbyWaterYear.TabIndex = 11
        Me.rdoBFIReportbyWaterYear.Text = "Water Year"
        Me.rdoBFIReportbyWaterYear.UseVisualStyleBackColor = True
        '
        'rdoBFIReportbyCalendarYear
        '
        Me.rdoBFIReportbyCalendarYear.AutoSize = True
        Me.rdoBFIReportbyCalendarYear.Checked = True
        Me.rdoBFIReportbyCalendarYear.Location = New System.Drawing.Point(201, 63)
        Me.rdoBFIReportbyCalendarYear.Name = "rdoBFIReportbyCalendarYear"
        Me.rdoBFIReportbyCalendarYear.Size = New System.Drawing.Size(92, 17)
        Me.rdoBFIReportbyCalendarYear.TabIndex = 10
        Me.rdoBFIReportbyCalendarYear.TabStop = True
        Me.rdoBFIReportbyCalendarYear.Text = "Calendar Year"
        Me.rdoBFIReportbyCalendarYear.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(130, 65)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(56, 13)
        Me.Label2.TabIndex = 9
        Me.Label2.Text = "Report by:"
        '
        'gbDFParam
        '
        Me.gbDFParam.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbDFParam.Controls.Add(Me.txt2PDefaultNotice)
        Me.gbDFParam.Controls.Add(Me.rdo2PDefault)
        Me.gbDFParam.Controls.Add(Me.rdo2PSpecify)
        Me.gbDFParam.Controls.Add(Me.lblTwoParam)
        Me.gbDFParam.Controls.Add(Me.txtDFParamBFImax)
        Me.gbDFParam.Controls.Add(Me.lblBFImax)
        Me.gbDFParam.Controls.Add(Me.txtDFParamBeta)
        Me.gbDFParam.Controls.Add(Me.lblBeta)
        Me.gbDFParam.Controls.Add(Me.txtDFParamRC)
        Me.gbDFParam.Controls.Add(Me.lblRC)
        Me.gbDFParam.Location = New System.Drawing.Point(131, 229)
        Me.gbDFParam.Name = "gbDFParam"
        Me.gbDFParam.Size = New System.Drawing.Size(254, 128)
        Me.gbDFParam.TabIndex = 31
        Me.gbDFParam.TabStop = False
        Me.gbDFParam.Text = "Digital Filter (DF) Parameters"
        '
        'txt2PDefaultNotice
        '
        Me.txt2PDefaultNotice.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txt2PDefaultNotice.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txt2PDefaultNotice.Location = New System.Drawing.Point(6, 61)
        Me.txt2PDefaultNotice.Multiline = True
        Me.txt2PDefaultNotice.Name = "txt2PDefaultNotice"
        Me.txt2PDefaultNotice.ReadOnly = True
        Me.txt2PDefaultNotice.Size = New System.Drawing.Size(242, 61)
        Me.txt2PDefaultNotice.TabIndex = 15
        Me.txt2PDefaultNotice.Text = "Recession Constant (a) and BFImax are calculated by the program"
        Me.txt2PDefaultNotice.Visible = False
        '
        'rdo2PDefault
        '
        Me.rdo2PDefault.AutoSize = True
        Me.rdo2PDefault.Location = New System.Drawing.Point(160, 38)
        Me.rdo2PDefault.Name = "rdo2PDefault"
        Me.rdo2PDefault.Size = New System.Drawing.Size(59, 17)
        Me.rdo2PDefault.TabIndex = 14
        Me.rdo2PDefault.Text = "Default"
        Me.rdo2PDefault.UseVisualStyleBackColor = True
        '
        'rdo2PSpecify
        '
        Me.rdo2PSpecify.AutoSize = True
        Me.rdo2PSpecify.Checked = True
        Me.rdo2PSpecify.Location = New System.Drawing.Point(94, 38)
        Me.rdo2PSpecify.Name = "rdo2PSpecify"
        Me.rdo2PSpecify.Size = New System.Drawing.Size(60, 17)
        Me.rdo2PSpecify.TabIndex = 13
        Me.rdo2PSpecify.TabStop = True
        Me.rdo2PSpecify.Text = "Specify"
        Me.rdo2PSpecify.UseVisualStyleBackColor = True
        '
        'lblTwoParam
        '
        Me.lblTwoParam.AutoSize = True
        Me.lblTwoParam.Location = New System.Drawing.Point(6, 40)
        Me.lblTwoParam.Name = "lblTwoParam"
        Me.lblTwoParam.Size = New System.Drawing.Size(82, 13)
        Me.lblTwoParam.TabIndex = 12
        Me.lblTwoParam.Text = "Two Parameter:"
        '
        'lblBFImax
        '
        Me.lblBFImax.AutoSize = True
        Me.lblBFImax.Location = New System.Drawing.Point(122, 102)
        Me.lblBFImax.Name = "lblBFImax"
        Me.lblBFImax.Size = New System.Drawing.Size(42, 13)
        Me.lblBFImax.TabIndex = 8
        Me.lblBFImax.Text = "BFImax"
        '
        'txtDFParamBeta
        '
        Me.txtDFParamBeta.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDFParamBeta.Location = New System.Drawing.Point(195, 12)
        Me.txtDFParamBeta.Name = "txtDFParamBeta"
        Me.txtDFParamBeta.Size = New System.Drawing.Size(53, 20)
        Me.txtDFParamBeta.TabIndex = 9
        Me.txtDFParamBeta.Text = "0.925"
        Me.txtDFParamBeta.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblBeta
        '
        Me.lblBeta.AutoSize = True
        Me.lblBeta.Location = New System.Drawing.Point(6, 15)
        Me.lblBeta.Name = "lblBeta"
        Me.lblBeta.Size = New System.Drawing.Size(183, 13)
        Me.lblBeta.TabIndex = 6
        Me.lblBeta.Text = "One Parameter Filter Constant (alpha)"
        '
        'lblRC
        '
        Me.lblRC.AutoSize = True
        Me.lblRC.Location = New System.Drawing.Point(47, 76)
        Me.lblRC.Name = "lblRC"
        Me.lblRC.Size = New System.Drawing.Size(117, 13)
        Me.lblRC.TabIndex = 7
        Me.lblRC.Text = "Recession Constant (a)"
        '
        'gbduration
        '
        Me.gbduration.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbduration.Controls.Add(Me.txtDurationNote)
        Me.gbduration.Controls.Add(Me.rdoDurationNo)
        Me.gbduration.Controls.Add(Me.rdoDurationYes)
        Me.gbduration.Location = New System.Drawing.Point(133, 86)
        Me.gbduration.Name = "gbduration"
        Me.gbduration.Size = New System.Drawing.Size(254, 40)
        Me.gbduration.TabIndex = 32
        Me.gbduration.TabStop = False
        Me.gbduration.Text = "Write flow duration curve for full span result:"
        '
        'rdoDurationYes
        '
        Me.rdoDurationYes.AutoSize = True
        Me.rdoDurationYes.Location = New System.Drawing.Point(9, 19)
        Me.rdoDurationYes.Name = "rdoDurationYes"
        Me.rdoDurationYes.Size = New System.Drawing.Size(43, 17)
        Me.rdoDurationYes.TabIndex = 0
        Me.rdoDurationYes.TabStop = True
        Me.rdoDurationYes.Text = "Yes"
        Me.rdoDurationYes.UseVisualStyleBackColor = True
        '
        'rdoDurationNo
        '
        Me.rdoDurationNo.AutoSize = True
        Me.rdoDurationNo.Location = New System.Drawing.Point(58, 19)
        Me.rdoDurationNo.Name = "rdoDurationNo"
        Me.rdoDurationNo.Size = New System.Drawing.Size(39, 17)
        Me.rdoDurationNo.TabIndex = 1
        Me.rdoDurationNo.TabStop = True
        Me.rdoDurationNo.Text = "No"
        Me.rdoDurationNo.UseVisualStyleBackColor = True
        '
        'gbNotes
        '
        Me.gbNotes.Controls.Add(Me.txtNotes)
        Me.gbNotes.Location = New System.Drawing.Point(12, 683)
        Me.gbNotes.Name = "gbNotes"
        Me.gbNotes.Size = New System.Drawing.Size(375, 73)
        Me.gbNotes.TabIndex = 33
        Me.gbNotes.TabStop = False
        Me.gbNotes.Text = "Notes"
        '
        'txtDurationNote
        '
        Me.txtDurationNote.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDurationNote.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtDurationNote.Location = New System.Drawing.Point(104, 19)
        Me.txtDurationNote.Name = "txtDurationNote"
        Me.txtDurationNote.ReadOnly = True
        Me.txtDurationNote.Size = New System.Drawing.Size(144, 13)
        Me.txtDurationNote.TabIndex = 2
        '
        'txtNotes
        '
        Me.txtNotes.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtNotes.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtNotes.Location = New System.Drawing.Point(6, 19)
        Me.txtNotes.Multiline = True
        Me.txtNotes.Name = "txtNotes"
        Me.txtNotes.ReadOnly = True
        Me.txtNotes.Size = New System.Drawing.Size(363, 48)
        Me.txtNotes.TabIndex = 0
        '
        'frmUSGSBaseflow
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(399, 768)
        Me.Controls.Add(Me.gbNotes)
        Me.Controls.Add(Me.gbduration)
        Me.Controls.Add(Me.rdoBFIReportbyWaterYear)
        Me.Controls.Add(Me.rdoBFIReportbyCalendarYear)
        Me.Controls.Add(Me.gbDFParam)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.gbBFI)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lblDrainageAreaUnits)
        Me.Controls.Add(Me.txtDrainageArea)
        Me.Controls.Add(Me.gbGraph)
        Me.Controls.Add(Me.gbBFMethods)
        Me.Controls.Add(Me.gbDates)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Controls.Add(Me.gbTextOutput)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "frmUSGSBaseflow"
        Me.Text = "Base-Flow Separation"
        Me.gbDates.ResumeLayout(False)
        Me.gbDates.PerformLayout()
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.gbTextOutput.ResumeLayout(False)
        Me.gbTextOutput.PerformLayout()
        Me.gbBFMethods.ResumeLayout(False)
        Me.gbBFMethods.PerformLayout()
        Me.gbGraph.ResumeLayout(False)
        Me.gbBFI.ResumeLayout(False)
        Me.gbBFI.PerformLayout()
        Me.gbDFParam.ResumeLayout(False)
        Me.gbDFParam.PerformLayout()
        Me.gbduration.ResumeLayout(False)
        Me.gbduration.PerformLayout()
        Me.gbNotes.ResumeLayout(False)
        Me.gbNotes.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtDrainageArea As System.Windows.Forms.TextBox
    Friend WithEvents lblDrainageAreaUnits As System.Windows.Forms.Label
    Friend WithEvents btnExamineData As System.Windows.Forms.Button
    Friend WithEvents gbDates As System.Windows.Forms.GroupBox
    Friend WithEvents lblDataEnd As System.Windows.Forms.Label
    Friend WithEvents lblDataStart As System.Windows.Forms.Label
    Friend WithEvents txtDataEnd As System.Windows.Forms.TextBox
    Friend WithEvents txtDataStart As System.Windows.Forms.TextBox
    Friend WithEvents toolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents txtStartDateUser As System.Windows.Forms.TextBox
    Friend WithEvents txtEndDateUser As System.Windows.Forms.TextBox
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents mnuFile As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuFileSelectData As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAnalysis As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuHelp As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuOutput As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuOutputASCII As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents txtOutputRootName As System.Windows.Forms.TextBox
    Friend WithEvents lblBaseFilename As System.Windows.Forms.Label
    Friend WithEvents mnuGraphBF As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gbTextOutput As System.Windows.Forms.GroupBox
    Friend WithEvents txtOutputDir As System.Windows.Forms.TextBox
    Friend WithEvents lblOutputDir As System.Windows.Forms.Label
    Friend WithEvents chkTabDelimited As System.Windows.Forms.CheckBox
    Friend WithEvents mnuGraphTimeseries As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuGraphDuration As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuGraphCDistPlot As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnGraphDurationPUA As System.Windows.Forms.Button
    Friend WithEvents btnGraphDuration As System.Windows.Forms.Button
    Friend WithEvents btnGraphTimeseries As System.Windows.Forms.Button
    Friend WithEvents btnWriteASCIIOutput As System.Windows.Forms.Button
    Friend WithEvents btnGraphCDist As System.Windows.Forms.Button
    Friend WithEvents gbBFMethods As System.Windows.Forms.GroupBox
    Friend WithEvents chkMethodHySEPSlide As System.Windows.Forms.CheckBox
    Friend WithEvents chkMethodHySEPLocMin As System.Windows.Forms.CheckBox
    Friend WithEvents chkMethodHySEPFixed As System.Windows.Forms.CheckBox
    Friend WithEvents chkMethodPART As System.Windows.Forms.CheckBox
    Friend WithEvents gbGraph As System.Windows.Forms.GroupBox
    Friend WithEvents lblPeriodOfRecord As System.Windows.Forms.Label
    Friend WithEvents lblAnalysisDates As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents gbBFI As System.Windows.Forms.GroupBox
    Friend WithEvents lblF As System.Windows.Forms.Label
    Friend WithEvents lblN As System.Windows.Forms.Label
    Friend WithEvents txtK As System.Windows.Forms.TextBox
    Friend WithEvents txtF As System.Windows.Forms.TextBox
    Friend WithEvents txtN As System.Windows.Forms.TextBox
    Friend WithEvents lblK As System.Windows.Forms.Label
    Friend WithEvents chkMethodBFIModified As System.Windows.Forms.CheckBox
    Friend WithEvents chkMethodBFIStandard As System.Windows.Forms.CheckBox
    Friend WithEvents rdoBFIReportbyWaterYear As System.Windows.Forms.RadioButton
    Friend WithEvents rdoBFIReportbyCalendarYear As System.Windows.Forms.RadioButton
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents chkMethodTwoPRDF As Windows.Forms.CheckBox
    Friend WithEvents chkMethodBFLOW As Windows.Forms.CheckBox
    Friend WithEvents gbDFParam As Windows.Forms.GroupBox
    Friend WithEvents txtDFParamBFImax As Windows.Forms.TextBox
    Friend WithEvents txtDFParamRC As Windows.Forms.TextBox
    Friend WithEvents txtDFParamBeta As Windows.Forms.TextBox
    Friend WithEvents lblBFImax As Windows.Forms.Label
    Friend WithEvents lblRC As Windows.Forms.Label
    Friend WithEvents lblBeta As Windows.Forms.Label
    Friend WithEvents rdo2PDefault As Windows.Forms.RadioButton
    Friend WithEvents rdo2PSpecify As Windows.Forms.RadioButton
    Friend WithEvents lblTwoParam As Windows.Forms.Label
    Friend WithEvents txt2PDefaultNotice As Windows.Forms.TextBox
    Friend WithEvents gbduration As Windows.Forms.GroupBox
    Friend WithEvents rdoDurationNo As Windows.Forms.RadioButton
    Friend WithEvents rdoDurationYes As Windows.Forms.RadioButton
    Friend WithEvents gbNotes As Windows.Forms.GroupBox
    Friend WithEvents txtDurationNote As Windows.Forms.TextBox
    Friend WithEvents txtNotes As Windows.Forms.TextBox
End Class
