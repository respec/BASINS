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
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUSGSBaseflow))
        Me.txtDrainageArea = New System.Windows.Forms.TextBox
        Me.lblDrainageAreaUnits = New System.Windows.Forms.Label
        Me.btnExamineData = New System.Windows.Forms.Button
        Me.gbDates = New System.Windows.Forms.GroupBox
        Me.lblAnalysisDates = New System.Windows.Forms.Label
        Me.lblPeriodOfRecord = New System.Windows.Forms.Label
        Me.txtEndDateUser = New System.Windows.Forms.TextBox
        Me.txtStartDateUser = New System.Windows.Forms.TextBox
        Me.txtDataEnd = New System.Windows.Forms.TextBox
        Me.txtDataStart = New System.Windows.Forms.TextBox
        Me.lblDataEnd = New System.Windows.Forms.Label
        Me.lblDataStart = New System.Windows.Forms.Label
        Me.toolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.chkMethodBFI = New System.Windows.Forms.CheckBox
        Me.chkMethodBFIStandard = New System.Windows.Forms.CheckBox
        Me.chkMethodBFIModified = New System.Windows.Forms.CheckBox
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip
        Me.mnuFile = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuFileSelectData = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuOutput = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuOutputASCII = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuGraphBF = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuGraphTimeseries = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuGraphDuration = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuGraphCDistPlot = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuAnalysis = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuHelp = New System.Windows.Forms.ToolStripMenuItem
        Me.txtOutputRootName = New System.Windows.Forms.TextBox
        Me.lblBaseFilename = New System.Windows.Forms.Label
        Me.gbTextOutput = New System.Windows.Forms.GroupBox
        Me.btnWriteASCIIOutput = New System.Windows.Forms.Button
        Me.chkTabDelimited = New System.Windows.Forms.CheckBox
        Me.txtOutputDir = New System.Windows.Forms.TextBox
        Me.lblOutputDir = New System.Windows.Forms.Label
        Me.btnGraphCDist = New System.Windows.Forms.Button
        Me.btnGraphDurationPUA = New System.Windows.Forms.Button
        Me.btnGraphDuration = New System.Windows.Forms.Button
        Me.btnGraphTimeseries = New System.Windows.Forms.Button
        Me.gbBFMethods = New System.Windows.Forms.GroupBox
        Me.chkMethodPART = New System.Windows.Forms.CheckBox
        Me.chkMethodHySEPSlide = New System.Windows.Forms.CheckBox
        Me.chkMethodHySEPLocMin = New System.Windows.Forms.CheckBox
        Me.chkMethodHySEPFixed = New System.Windows.Forms.CheckBox
        Me.gbGraph = New System.Windows.Forms.GroupBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.gbBFI = New System.Windows.Forms.GroupBox
        Me.txtK = New System.Windows.Forms.TextBox
        Me.txtF = New System.Windows.Forms.TextBox
        Me.txtN = New System.Windows.Forms.TextBox
        Me.lblK = New System.Windows.Forms.Label
        Me.lblF = New System.Windows.Forms.Label
        Me.lblN = New System.Windows.Forms.Label
        Me.chkBFISymbols = New System.Windows.Forms.CheckBox
        Me.gbDates.SuspendLayout()
        Me.MenuStrip1.SuspendLayout()
        Me.gbTextOutput.SuspendLayout()
        Me.gbBFMethods.SuspendLayout()
        Me.gbGraph.SuspendLayout()
        Me.gbBFI.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtDrainageArea
        '
        Me.txtDrainageArea.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDrainageArea.Location = New System.Drawing.Point(211, 27)
        Me.txtDrainageArea.Name = "txtDrainageArea"
        Me.txtDrainageArea.Size = New System.Drawing.Size(124, 20)
        Me.txtDrainageArea.TabIndex = 6
        Me.txtDrainageArea.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblDrainageAreaUnits
        '
        Me.lblDrainageAreaUnits.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblDrainageAreaUnits.AutoSize = True
        Me.lblDrainageAreaUnits.Location = New System.Drawing.Point(341, 30)
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
        Me.gbDates.Location = New System.Drawing.Point(12, 174)
        Me.gbDates.Name = "gbDates"
        Me.gbDates.Size = New System.Drawing.Size(361, 114)
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
        Me.txtEndDateUser.Size = New System.Drawing.Size(161, 20)
        Me.txtEndDateUser.TabIndex = 9
        Me.toolTip1.SetToolTip(Me.txtEndDateUser, "User-specified ending date: yyyy/mm/dd")
        '
        'txtStartDateUser
        '
        Me.txtStartDateUser.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtStartDateUser.Location = New System.Drawing.Point(194, 31)
        Me.txtStartDateUser.Name = "txtStartDateUser"
        Me.txtStartDateUser.Size = New System.Drawing.Size(161, 20)
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
        'chkMethodBFI
        '
        Me.chkMethodBFI.AutoSize = True
        Me.chkMethodBFI.Location = New System.Drawing.Point(6, 112)
        Me.chkMethodBFI.Name = "chkMethodBFI"
        Me.chkMethodBFI.Size = New System.Drawing.Size(42, 17)
        Me.chkMethodBFI.TabIndex = 5
        Me.chkMethodBFI.Text = "BFI"
        Me.toolTip1.SetToolTip(Me.chkMethodBFI, "Annual baseflow index")
        Me.chkMethodBFI.UseVisualStyleBackColor = True
        '
        'chkMethodBFIStandard
        '
        Me.chkMethodBFIStandard.AutoSize = True
        Me.chkMethodBFIStandard.Location = New System.Drawing.Point(7, 20)
        Me.chkMethodBFIStandard.Name = "chkMethodBFIStandard"
        Me.chkMethodBFIStandard.Size = New System.Drawing.Size(69, 17)
        Me.chkMethodBFIStandard.TabIndex = 6
        Me.chkMethodBFIStandard.Text = "Standard"
        Me.toolTip1.SetToolTip(Me.chkMethodBFIStandard, "Institute of Hydrology Method (Standard)")
        Me.chkMethodBFIStandard.UseVisualStyleBackColor = True
        '
        'chkMethodBFIModified
        '
        Me.chkMethodBFIModified.AutoSize = True
        Me.chkMethodBFIModified.Location = New System.Drawing.Point(82, 20)
        Me.chkMethodBFIModified.Name = "chkMethodBFIModified"
        Me.chkMethodBFIModified.Size = New System.Drawing.Size(66, 17)
        Me.chkMethodBFIModified.TabIndex = 7
        Me.chkMethodBFIModified.Text = "Modified"
        Me.toolTip1.SetToolTip(Me.chkMethodBFIModified, "Modified Method")
        Me.chkMethodBFIModified.UseVisualStyleBackColor = True
        '
        'MenuStrip1
        '
        Me.MenuStrip1.BackColor = System.Drawing.SystemColors.Window
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuFile, Me.mnuAnalysis, Me.mnuHelp})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(385, 24)
        Me.MenuStrip1.TabIndex = 28
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'mnuFile
        '
        Me.mnuFile.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuFileSelectData, Me.mnuOutput, Me.mnuGraphBF})
        Me.mnuFile.Name = "mnuFile"
        Me.mnuFile.Size = New System.Drawing.Size(35, 20)
        Me.mnuFile.Text = "File"
        '
        'mnuFileSelectData
        '
        Me.mnuFileSelectData.Name = "mnuFileSelectData"
        Me.mnuFileSelectData.Size = New System.Drawing.Size(140, 22)
        Me.mnuFileSelectData.Text = "Select Data"
        '
        'mnuOutput
        '
        Me.mnuOutput.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuOutputASCII})
        Me.mnuOutput.Name = "mnuOutput"
        Me.mnuOutput.Size = New System.Drawing.Size(140, 22)
        Me.mnuOutput.Text = "Output"
        '
        'mnuOutputASCII
        '
        Me.mnuOutputASCII.Name = "mnuOutputASCII"
        Me.mnuOutputASCII.Size = New System.Drawing.Size(113, 22)
        Me.mnuOutputASCII.Text = "ASCII"
        '
        'mnuGraphBF
        '
        Me.mnuGraphBF.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuGraphTimeseries, Me.mnuGraphDuration, Me.mnuGraphCDistPlot})
        Me.mnuGraphBF.Name = "mnuGraphBF"
        Me.mnuGraphBF.Size = New System.Drawing.Size(140, 22)
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
        Me.mnuAnalysis.Size = New System.Drawing.Size(58, 20)
        Me.mnuAnalysis.Text = "Analysis"
        '
        'mnuHelp
        '
        Me.mnuHelp.Name = "mnuHelp"
        Me.mnuHelp.Size = New System.Drawing.Size(40, 20)
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
        Me.gbTextOutput.Controls.Add(Me.btnWriteASCIIOutput)
        Me.gbTextOutput.Controls.Add(Me.chkTabDelimited)
        Me.gbTextOutput.Controls.Add(Me.txtOutputDir)
        Me.gbTextOutput.Controls.Add(Me.lblOutputDir)
        Me.gbTextOutput.Controls.Add(Me.txtOutputRootName)
        Me.gbTextOutput.Controls.Add(Me.lblBaseFilename)
        Me.gbTextOutput.Location = New System.Drawing.Point(12, 294)
        Me.gbTextOutput.Name = "gbTextOutput"
        Me.gbTextOutput.Size = New System.Drawing.Size(361, 111)
        Me.gbTextOutput.TabIndex = 11
        Me.gbTextOutput.TabStop = False
        Me.gbTextOutput.Text = "Text Output"
        '
        'btnWriteASCIIOutput
        '
        Me.btnWriteASCIIOutput.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnWriteASCIIOutput.Location = New System.Drawing.Point(227, 82)
        Me.btnWriteASCIIOutput.Name = "btnWriteASCIIOutput"
        Me.btnWriteASCIIOutput.Size = New System.Drawing.Size(128, 23)
        Me.btnWriteASCIIOutput.TabIndex = 15
        Me.btnWriteASCIIOutput.Text = "Write Text Output"
        Me.btnWriteASCIIOutput.UseVisualStyleBackColor = True
        '
        'chkTabDelimited
        '
        Me.chkTabDelimited.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkTabDelimited.AutoSize = True
        Me.chkTabDelimited.Location = New System.Drawing.Point(130, 86)
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
        Me.txtOutputDir.Size = New System.Drawing.Size(277, 20)
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
        Me.btnGraphTimeseries.Text = "Timeseries"
        Me.btnGraphTimeseries.UseVisualStyleBackColor = True
        '
        'gbBFMethods
        '
        Me.gbBFMethods.Controls.Add(Me.chkMethodBFI)
        Me.gbBFMethods.Controls.Add(Me.chkMethodPART)
        Me.gbBFMethods.Controls.Add(Me.chkMethodHySEPSlide)
        Me.gbBFMethods.Controls.Add(Me.chkMethodHySEPLocMin)
        Me.gbBFMethods.Controls.Add(Me.chkMethodHySEPFixed)
        Me.gbBFMethods.Location = New System.Drawing.Point(12, 30)
        Me.gbBFMethods.Name = "gbBFMethods"
        Me.gbBFMethods.Size = New System.Drawing.Size(112, 138)
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
        Me.gbGraph.Location = New System.Drawing.Point(12, 411)
        Me.gbGraph.Name = "gbGraph"
        Me.gbGraph.Size = New System.Drawing.Size(361, 90)
        Me.gbGraph.TabIndex = 16
        Me.gbGraph.TabStop = False
        Me.gbGraph.Text = "Display Graph"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(130, 30)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(75, 13)
        Me.Label1.TabIndex = 29
        Me.Label1.Text = "Drainage Area"
        '
        'gbBFI
        '
        Me.gbBFI.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbBFI.Controls.Add(Me.chkBFISymbols)
        Me.gbBFI.Controls.Add(Me.chkMethodBFIModified)
        Me.gbBFI.Controls.Add(Me.chkMethodBFIStandard)
        Me.gbBFI.Controls.Add(Me.txtK)
        Me.gbBFI.Controls.Add(Me.txtF)
        Me.gbBFI.Controls.Add(Me.txtN)
        Me.gbBFI.Controls.Add(Me.lblK)
        Me.gbBFI.Controls.Add(Me.lblF)
        Me.gbBFI.Controls.Add(Me.lblN)
        Me.gbBFI.Location = New System.Drawing.Point(133, 53)
        Me.gbBFI.Name = "gbBFI"
        Me.gbBFI.Size = New System.Drawing.Size(240, 115)
        Me.gbBFI.TabIndex = 30
        Me.gbBFI.TabStop = False
        Me.gbBFI.Text = "BFI"
        '
        'txtK
        '
        Me.txtK.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtK.Location = New System.Drawing.Point(169, 89)
        Me.txtK.Name = "txtK"
        Me.txtK.Size = New System.Drawing.Size(65, 20)
        Me.txtK.TabIndex = 5
        Me.txtK.Text = "0.97915"
        Me.txtK.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtF
        '
        Me.txtF.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtF.Location = New System.Drawing.Point(169, 63)
        Me.txtF.Name = "txtF"
        Me.txtF.Size = New System.Drawing.Size(65, 20)
        Me.txtF.TabIndex = 4
        Me.txtF.Text = "0.9"
        Me.txtF.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtN
        '
        Me.txtN.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtN.Location = New System.Drawing.Point(169, 37)
        Me.txtN.Name = "txtN"
        Me.txtN.Size = New System.Drawing.Size(65, 20)
        Me.txtN.TabIndex = 3
        Me.txtN.Text = "5"
        Me.txtN.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblK
        '
        Me.lblK.AutoSize = True
        Me.lblK.Location = New System.Drawing.Point(6, 92)
        Me.lblK.Name = "lblK"
        Me.lblK.Size = New System.Drawing.Size(160, 13)
        Me.lblK.TabIndex = 2
        Me.lblK.Text = "One-Day Recession Constant(K)"
        '
        'lblF
        '
        Me.lblF.AutoSize = True
        Me.lblF.Location = New System.Drawing.Point(24, 67)
        Me.lblF.Name = "lblF"
        Me.lblF.Size = New System.Drawing.Size(139, 13)
        Me.lblF.TabIndex = 1
        Me.lblF.Text = "Turning Point Test Factor(F)"
        '
        'lblN
        '
        Me.lblN.AutoSize = True
        Me.lblN.Location = New System.Drawing.Point(37, 40)
        Me.lblN.Name = "lblN"
        Me.lblN.Size = New System.Drawing.Size(126, 13)
        Me.lblN.TabIndex = 0
        Me.lblN.Text = "Partition Length (N, days)"
        '
        'chkBFISymbols
        '
        Me.chkBFISymbols.AutoSize = True
        Me.chkBFISymbols.Location = New System.Drawing.Point(169, 20)
        Me.chkBFISymbols.Name = "chkBFISymbols"
        Me.chkBFISymbols.Size = New System.Drawing.Size(65, 17)
        Me.chkBFISymbols.TabIndex = 8
        Me.chkBFISymbols.Text = "Symbols"
        Me.toolTip1.SetToolTip(Me.chkBFISymbols, "Use symbols in output file")
        Me.chkBFISymbols.UseVisualStyleBackColor = True
        '
        'frmUSGSBaseflow
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(385, 508)
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
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "frmUSGSBaseflow"
        Me.Text = "Baseflow Separation"
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
    Friend WithEvents chkMethodBFI As System.Windows.Forms.CheckBox
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
    Friend WithEvents chkBFISymbols As System.Windows.Forms.CheckBox
End Class
