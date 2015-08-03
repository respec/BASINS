﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUSGSBaseflowBatch
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUSGSBaseflowBatch))
        Me.btnExamineData = New System.Windows.Forms.Button
        Me.gbDates = New System.Windows.Forms.GroupBox
        Me.btnPlotDur = New System.Windows.Forms.Button
        Me.lblAnalysisDates = New System.Windows.Forms.Label
        Me.lblPeriodOfRecord = New System.Windows.Forms.Label
        Me.txtEndDateUser = New System.Windows.Forms.TextBox
        Me.txtStartDateUser = New System.Windows.Forms.TextBox
        Me.txtDataEnd = New System.Windows.Forms.TextBox
        Me.txtDataStart = New System.Windows.Forms.TextBox
        Me.lblDataEnd = New System.Windows.Forms.Label
        Me.lblDataStart = New System.Windows.Forms.Label
        Me.toolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.chkMethodBFIStandard = New System.Windows.Forms.CheckBox
        Me.chkMethodBFIModified = New System.Windows.Forms.CheckBox
        Me.chkBFISymbols = New System.Windows.Forms.CheckBox
        Me.txtOutputRootName = New System.Windows.Forms.TextBox
        Me.lblBaseFilename = New System.Windows.Forms.Label
        Me.gbTextOutput = New System.Windows.Forms.GroupBox
        Me.txtOutputDir = New System.Windows.Forms.TextBox
        Me.lblOutputDir = New System.Windows.Forms.Label
        Me.btnWriteASCIIOutput = New System.Windows.Forms.Button
        Me.gbBFMethods = New System.Windows.Forms.GroupBox
        Me.chkMethodPART = New System.Windows.Forms.CheckBox
        Me.chkMethodHySEPSlide = New System.Windows.Forms.CheckBox
        Me.chkMethodHySEPLocMin = New System.Windows.Forms.CheckBox
        Me.chkMethodHySEPFixed = New System.Windows.Forms.CheckBox
        Me.gbBFI = New System.Windows.Forms.GroupBox
        Me.rdoBFIReportbyWaterYear = New System.Windows.Forms.RadioButton
        Me.rdoBFIReportbyCalendarYear = New System.Windows.Forms.RadioButton
        Me.Label2 = New System.Windows.Forms.Label
        Me.txtK = New System.Windows.Forms.TextBox
        Me.txtF = New System.Windows.Forms.TextBox
        Me.txtN = New System.Windows.Forms.TextBox
        Me.lblK = New System.Windows.Forms.Label
        Me.lblF = New System.Windows.Forms.Label
        Me.lblN = New System.Windows.Forms.Label
        Me.colDataPath = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.colDA = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.colStationID = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.grdStations = New System.Windows.Forms.DataGridView
        Me.gbDates.SuspendLayout()
        Me.gbTextOutput.SuspendLayout()
        Me.gbBFMethods.SuspendLayout()
        Me.gbBFI.SuspendLayout()
        CType(Me.grdStations, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
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
        Me.gbDates.Controls.Add(Me.btnPlotDur)
        Me.gbDates.Controls.Add(Me.lblAnalysisDates)
        Me.gbDates.Controls.Add(Me.lblPeriodOfRecord)
        Me.gbDates.Controls.Add(Me.txtEndDateUser)
        Me.gbDates.Controls.Add(Me.txtStartDateUser)
        Me.gbDates.Controls.Add(Me.btnExamineData)
        Me.gbDates.Controls.Add(Me.txtDataEnd)
        Me.gbDates.Controls.Add(Me.txtDataStart)
        Me.gbDates.Controls.Add(Me.lblDataEnd)
        Me.gbDates.Controls.Add(Me.lblDataStart)
        Me.gbDates.Location = New System.Drawing.Point(9, 296)
        Me.gbDates.Name = "gbDates"
        Me.gbDates.Size = New System.Drawing.Size(378, 114)
        Me.gbDates.TabIndex = 7
        Me.gbDates.TabStop = False
        Me.gbDates.Text = "Define Analysis Dates"
        '
        'btnPlotDur
        '
        Me.btnPlotDur.Location = New System.Drawing.Point(168, 85)
        Me.btnPlotDur.Name = "btnPlotDur"
        Me.btnPlotDur.Size = New System.Drawing.Size(85, 23)
        Me.btnPlotDur.TabIndex = 13
        Me.btnPlotDur.Text = "Plot Durations"
        Me.btnPlotDur.UseVisualStyleBackColor = True
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
        Me.txtEndDateUser.Size = New System.Drawing.Size(178, 20)
        Me.txtEndDateUser.TabIndex = 9
        Me.toolTip1.SetToolTip(Me.txtEndDateUser, "User-specified ending date: yyyy/mm/dd")
        '
        'txtStartDateUser
        '
        Me.txtStartDateUser.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtStartDateUser.Location = New System.Drawing.Point(194, 31)
        Me.txtStartDateUser.Name = "txtStartDateUser"
        Me.txtStartDateUser.Size = New System.Drawing.Size(178, 20)
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
        'chkBFISymbols
        '
        Me.chkBFISymbols.AutoSize = True
        Me.chkBFISymbols.Location = New System.Drawing.Point(6, 105)
        Me.chkBFISymbols.Name = "chkBFISymbols"
        Me.chkBFISymbols.Size = New System.Drawing.Size(65, 17)
        Me.chkBFISymbols.TabIndex = 8
        Me.chkBFISymbols.Text = "Symbols"
        Me.toolTip1.SetToolTip(Me.chkBFISymbols, "Use symbols in output file")
        Me.chkBFISymbols.UseVisualStyleBackColor = True
        Me.chkBFISymbols.Visible = False
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
        Me.gbTextOutput.Controls.Add(Me.txtOutputDir)
        Me.gbTextOutput.Controls.Add(Me.lblOutputDir)
        Me.gbTextOutput.Controls.Add(Me.txtOutputRootName)
        Me.gbTextOutput.Controls.Add(Me.lblBaseFilename)
        Me.gbTextOutput.Location = New System.Drawing.Point(9, 416)
        Me.gbTextOutput.Name = "gbTextOutput"
        Me.gbTextOutput.Size = New System.Drawing.Size(378, 80)
        Me.gbTextOutput.TabIndex = 11
        Me.gbTextOutput.TabStop = False
        Me.gbTextOutput.Text = "Text Output"
        '
        'txtOutputDir
        '
        Me.txtOutputDir.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtOutputDir.Location = New System.Drawing.Point(78, 20)
        Me.txtOutputDir.Name = "txtOutputDir"
        Me.txtOutputDir.Size = New System.Drawing.Size(294, 20)
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
        'btnWriteASCIIOutput
        '
        Me.btnWriteASCIIOutput.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnWriteASCIIOutput.Location = New System.Drawing.Point(9, 502)
        Me.btnWriteASCIIOutput.Name = "btnWriteASCIIOutput"
        Me.btnWriteASCIIOutput.Size = New System.Drawing.Size(378, 23)
        Me.btnWriteASCIIOutput.TabIndex = 15
        Me.btnWriteASCIIOutput.Text = "Set Parameters"
        Me.btnWriteASCIIOutput.UseVisualStyleBackColor = True
        '
        'gbBFMethods
        '
        Me.gbBFMethods.Controls.Add(Me.chkMethodPART)
        Me.gbBFMethods.Controls.Add(Me.chkMethodBFIModified)
        Me.gbBFMethods.Controls.Add(Me.chkMethodHySEPSlide)
        Me.gbBFMethods.Controls.Add(Me.chkMethodBFIStandard)
        Me.gbBFMethods.Controls.Add(Me.chkMethodHySEPLocMin)
        Me.gbBFMethods.Controls.Add(Me.chkMethodHySEPFixed)
        Me.gbBFMethods.Location = New System.Drawing.Point(9, 130)
        Me.gbBFMethods.Name = "gbBFMethods"
        Me.gbBFMethods.Size = New System.Drawing.Size(112, 160)
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
        'gbBFI
        '
        Me.gbBFI.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbBFI.Controls.Add(Me.rdoBFIReportbyWaterYear)
        Me.gbBFI.Controls.Add(Me.rdoBFIReportbyCalendarYear)
        Me.gbBFI.Controls.Add(Me.Label2)
        Me.gbBFI.Controls.Add(Me.chkBFISymbols)
        Me.gbBFI.Controls.Add(Me.txtK)
        Me.gbBFI.Controls.Add(Me.txtF)
        Me.gbBFI.Controls.Add(Me.txtN)
        Me.gbBFI.Controls.Add(Me.lblK)
        Me.gbBFI.Controls.Add(Me.lblF)
        Me.gbBFI.Controls.Add(Me.lblN)
        Me.gbBFI.Location = New System.Drawing.Point(133, 130)
        Me.gbBFI.Name = "gbBFI"
        Me.gbBFI.Size = New System.Drawing.Size(254, 160)
        Me.gbBFI.TabIndex = 30
        Me.gbBFI.TabStop = False
        Me.gbBFI.Text = "BFI Parameters"
        '
        'rdoBFIReportbyWaterYear
        '
        Me.rdoBFIReportbyWaterYear.AutoSize = True
        Me.rdoBFIReportbyWaterYear.Location = New System.Drawing.Point(169, 16)
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
        Me.rdoBFIReportbyCalendarYear.Location = New System.Drawing.Point(71, 16)
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
        Me.Label2.Location = New System.Drawing.Point(6, 16)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(56, 13)
        Me.Label2.TabIndex = 9
        Me.Label2.Text = "Report by:"
        '
        'txtK
        '
        Me.txtK.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtK.Location = New System.Drawing.Point(170, 86)
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
        Me.txtF.Location = New System.Drawing.Point(170, 62)
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
        Me.txtN.Location = New System.Drawing.Point(170, 37)
        Me.txtN.Name = "txtN"
        Me.txtN.Size = New System.Drawing.Size(79, 20)
        Me.txtN.TabIndex = 3
        Me.txtN.Text = "5"
        Me.txtN.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblK
        '
        Me.lblK.AutoSize = True
        Me.lblK.Location = New System.Drawing.Point(45, 89)
        Me.lblK.Name = "lblK"
        Me.lblK.Size = New System.Drawing.Size(118, 13)
        Me.lblK.TabIndex = 2
        Me.lblK.Text = "Recession Constant (K)"
        '
        'lblF
        '
        Me.lblF.AutoSize = True
        Me.lblF.Location = New System.Drawing.Point(24, 64)
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
        'colDataPath
        '
        Me.colDataPath.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells
        Me.colDataPath.HeaderText = "Data Path"
        Me.colDataPath.Name = "colDataPath"
        Me.colDataPath.Width = 80
        '
        'colDA
        '
        Me.colDA.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader
        Me.colDA.HeaderText = "Drainage Area"
        Me.colDA.MaxInputLength = 16
        Me.colDA.Name = "colDA"
        Me.colDA.Width = 21
        '
        'colStationID
        '
        Me.colStationID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader
        Me.colStationID.HeaderText = "Station ID"
        Me.colStationID.MaxInputLength = 16
        Me.colStationID.MinimumWidth = 8
        Me.colStationID.Name = "colStationID"
        Me.colStationID.ReadOnly = True
        Me.colStationID.Width = 21
        '
        'grdStations
        '
        Me.grdStations.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grdStations.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.grdStations.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colStationID, Me.colDA, Me.colDataPath})
        Me.grdStations.Location = New System.Drawing.Point(9, 3)
        Me.grdStations.Name = "grdStations"
        Me.grdStations.Size = New System.Drawing.Size(378, 121)
        Me.grdStations.TabIndex = 31
        '
        'frmUSGSBaseflowBatch
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(399, 538)
        Me.Controls.Add(Me.grdStations)
        Me.Controls.Add(Me.btnWriteASCIIOutput)
        Me.Controls.Add(Me.gbBFI)
        Me.Controls.Add(Me.gbBFMethods)
        Me.Controls.Add(Me.gbDates)
        Me.Controls.Add(Me.gbTextOutput)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmUSGSBaseflowBatch"
        Me.Text = "Base-Flow Separation Batch Parameters"
        Me.gbDates.ResumeLayout(False)
        Me.gbDates.PerformLayout()
        Me.gbTextOutput.ResumeLayout(False)
        Me.gbTextOutput.PerformLayout()
        Me.gbBFMethods.ResumeLayout(False)
        Me.gbBFMethods.PerformLayout()
        Me.gbBFI.ResumeLayout(False)
        Me.gbBFI.PerformLayout()
        CType(Me.grdStations, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnExamineData As System.Windows.Forms.Button
    Friend WithEvents gbDates As System.Windows.Forms.GroupBox
    Friend WithEvents lblDataEnd As System.Windows.Forms.Label
    Friend WithEvents lblDataStart As System.Windows.Forms.Label
    Friend WithEvents txtDataEnd As System.Windows.Forms.TextBox
    Friend WithEvents txtDataStart As System.Windows.Forms.TextBox
    Friend WithEvents toolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents txtStartDateUser As System.Windows.Forms.TextBox
    Friend WithEvents txtEndDateUser As System.Windows.Forms.TextBox
    Friend WithEvents txtOutputRootName As System.Windows.Forms.TextBox
    Friend WithEvents lblBaseFilename As System.Windows.Forms.Label
    Friend WithEvents gbTextOutput As System.Windows.Forms.GroupBox
    Friend WithEvents txtOutputDir As System.Windows.Forms.TextBox
    Friend WithEvents lblOutputDir As System.Windows.Forms.Label
    Friend WithEvents btnWriteASCIIOutput As System.Windows.Forms.Button
    Friend WithEvents gbBFMethods As System.Windows.Forms.GroupBox
    Friend WithEvents chkMethodHySEPSlide As System.Windows.Forms.CheckBox
    Friend WithEvents chkMethodHySEPLocMin As System.Windows.Forms.CheckBox
    Friend WithEvents chkMethodHySEPFixed As System.Windows.Forms.CheckBox
    Friend WithEvents chkMethodPART As System.Windows.Forms.CheckBox
    Friend WithEvents lblPeriodOfRecord As System.Windows.Forms.Label
    Friend WithEvents lblAnalysisDates As System.Windows.Forms.Label
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
    Friend WithEvents rdoBFIReportbyWaterYear As System.Windows.Forms.RadioButton
    Friend WithEvents rdoBFIReportbyCalendarYear As System.Windows.Forms.RadioButton
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents btnPlotDur As System.Windows.Forms.Button
    Friend WithEvents colDataPath As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colDA As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colStationID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents grdStations As System.Windows.Forms.DataGridView
End Class
