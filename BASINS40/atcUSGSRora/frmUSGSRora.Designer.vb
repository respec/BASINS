<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUSGSRora
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUSGSRora))
        Me.gbDates = New System.Windows.Forms.GroupBox
        Me.lblAnalysisDates = New System.Windows.Forms.Label
        Me.lblPeriodOfRecord = New System.Windows.Forms.Label
        Me.txtEndDateUser = New System.Windows.Forms.TextBox
        Me.txtStartDateUser = New System.Windows.Forms.TextBox
        Me.btnExamineData = New System.Windows.Forms.Button
        Me.txtDataEnd = New System.Windows.Forms.TextBox
        Me.txtDataStart = New System.Windows.Forms.TextBox
        Me.lblDataEnd = New System.Windows.Forms.Label
        Me.lblDataStart = New System.Windows.Forms.Label
        Me.gbOutputFileSpecs = New System.Windows.Forms.GroupBox
        Me.btnGraphRecharge = New System.Windows.Forms.Button
        Me.btnWriteASCIIOutput = New System.Windows.Forms.Button
        Me.txtOutputDir = New System.Windows.Forms.TextBox
        Me.lblOutputDir = New System.Windows.Forms.Label
        Me.txtOutputRootName = New System.Windows.Forms.TextBox
        Me.lblBaseFilename = New System.Windows.Forms.Label
        Me.gbParameters = New System.Windows.Forms.GroupBox
        Me.cboAnteRecess = New System.Windows.Forms.ComboBox
        Me.btnRecessionIndex = New System.Windows.Forms.Button
        Me.lblUnit3 = New System.Windows.Forms.Label
        Me.lblUnit2 = New System.Windows.Forms.Label
        Me.lblAnteRecess = New System.Windows.Forms.Label
        Me.txtRecessionIndex = New System.Windows.Forms.TextBox
        Me.lblRecessionIndex = New System.Windows.Forms.Label
        Me.lblDrainageArea = New System.Windows.Forms.Label
        Me.txtDrainageArea = New System.Windows.Forms.TextBox
        Me.lblDrainageAreaUnits = New System.Windows.Forms.Label
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip
        Me.mnuFile = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuFileSelectData = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuAnalysis = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuHelp = New System.Windows.Forms.ToolStripMenuItem
        Me.lblNewMinFlow = New System.Windows.Forms.Label
        Me.txtNewMinFlow = New System.Windows.Forms.TextBox
        Me.lblUnit4 = New System.Windows.Forms.Label
        Me.gbDates.SuspendLayout()
        Me.gbOutputFileSpecs.SuspendLayout()
        Me.gbParameters.SuspendLayout()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
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
        Me.gbDates.Location = New System.Drawing.Point(16, 33)
        Me.gbDates.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.gbDates.Name = "gbDates"
        Me.gbDates.Padding = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.gbDates.Size = New System.Drawing.Size(459, 140)
        Me.gbDates.TabIndex = 8
        Me.gbDates.TabStop = False
        Me.gbDates.Text = "Analysis Dates"
        '
        'lblAnalysisDates
        '
        Me.lblAnalysisDates.AutoSize = True
        Me.lblAnalysisDates.Location = New System.Drawing.Point(254, 18)
        Me.lblAnalysisDates.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblAnalysisDates.Name = "lblAnalysisDates"
        Me.lblAnalysisDates.Size = New System.Drawing.Size(101, 17)
        Me.lblAnalysisDates.TabIndex = 14
        Me.lblAnalysisDates.Text = "Analysis Dates"
        '
        'lblPeriodOfRecord
        '
        Me.lblPeriodOfRecord.AutoSize = True
        Me.lblPeriodOfRecord.Location = New System.Drawing.Point(87, 20)
        Me.lblPeriodOfRecord.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblPeriodOfRecord.Name = "lblPeriodOfRecord"
        Me.lblPeriodOfRecord.Size = New System.Drawing.Size(115, 17)
        Me.lblPeriodOfRecord.TabIndex = 13
        Me.lblPeriodOfRecord.Text = "Period of Record"
        '
        'txtEndDateUser
        '
        Me.txtEndDateUser.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtEndDateUser.Location = New System.Drawing.Point(257, 73)
        Me.txtEndDateUser.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txtEndDateUser.Name = "txtEndDateUser"
        Me.txtEndDateUser.Size = New System.Drawing.Size(191, 22)
        Me.txtEndDateUser.TabIndex = 2
        '
        'txtStartDateUser
        '
        Me.txtStartDateUser.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtStartDateUser.Location = New System.Drawing.Point(257, 39)
        Me.txtStartDateUser.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txtStartDateUser.Name = "txtStartDateUser"
        Me.txtStartDateUser.Size = New System.Drawing.Size(191, 22)
        Me.txtStartDateUser.TabIndex = 1
        '
        'btnExamineData
        '
        Me.btnExamineData.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnExamineData.Location = New System.Drawing.Point(91, 105)
        Me.btnExamineData.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnExamineData.Name = "btnExamineData"
        Me.btnExamineData.Size = New System.Drawing.Size(125, 28)
        Me.btnExamineData.TabIndex = 3
        Me.btnExamineData.Text = "Examine Data"
        Me.btnExamineData.UseVisualStyleBackColor = True
        '
        'txtDataEnd
        '
        Me.txtDataEnd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtDataEnd.Location = New System.Drawing.Point(91, 73)
        Me.txtDataEnd.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txtDataEnd.Name = "txtDataEnd"
        Me.txtDataEnd.ReadOnly = True
        Me.txtDataEnd.Size = New System.Drawing.Size(158, 22)
        Me.txtDataEnd.TabIndex = 12
        '
        'txtDataStart
        '
        Me.txtDataStart.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtDataStart.Location = New System.Drawing.Point(91, 39)
        Me.txtDataStart.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txtDataStart.Name = "txtDataStart"
        Me.txtDataStart.ReadOnly = True
        Me.txtDataStart.Size = New System.Drawing.Size(158, 22)
        Me.txtDataStart.TabIndex = 11
        '
        'lblDataEnd
        '
        Me.lblDataEnd.AutoSize = True
        Me.lblDataEnd.Location = New System.Drawing.Point(8, 71)
        Me.lblDataEnd.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblDataEnd.Name = "lblDataEnd"
        Me.lblDataEnd.Size = New System.Drawing.Size(67, 17)
        Me.lblDataEnd.TabIndex = 1
        Me.lblDataEnd.Text = "Data End"
        '
        'lblDataStart
        '
        Me.lblDataStart.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblDataStart.AutoSize = True
        Me.lblDataStart.Location = New System.Drawing.Point(8, 39)
        Me.lblDataStart.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblDataStart.Name = "lblDataStart"
        Me.lblDataStart.Size = New System.Drawing.Size(72, 17)
        Me.lblDataStart.TabIndex = 0
        Me.lblDataStart.Text = "Data Start"
        '
        'gbOutputFileSpecs
        '
        Me.gbOutputFileSpecs.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbOutputFileSpecs.Controls.Add(Me.btnGraphRecharge)
        Me.gbOutputFileSpecs.Controls.Add(Me.btnWriteASCIIOutput)
        Me.gbOutputFileSpecs.Controls.Add(Me.txtOutputDir)
        Me.gbOutputFileSpecs.Controls.Add(Me.lblOutputDir)
        Me.gbOutputFileSpecs.Controls.Add(Me.txtOutputRootName)
        Me.gbOutputFileSpecs.Controls.Add(Me.lblBaseFilename)
        Me.gbOutputFileSpecs.Location = New System.Drawing.Point(16, 397)
        Me.gbOutputFileSpecs.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.gbOutputFileSpecs.Name = "gbOutputFileSpecs"
        Me.gbOutputFileSpecs.Padding = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.gbOutputFileSpecs.Size = New System.Drawing.Size(459, 137)
        Me.gbOutputFileSpecs.TabIndex = 12
        Me.gbOutputFileSpecs.TabStop = False
        Me.gbOutputFileSpecs.Text = "Text Output"
        '
        'btnGraphRecharge
        '
        Me.btnGraphRecharge.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGraphRecharge.Location = New System.Drawing.Point(104, 101)
        Me.btnGraphRecharge.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnGraphRecharge.Name = "btnGraphRecharge"
        Me.btnGraphRecharge.Size = New System.Drawing.Size(168, 28)
        Me.btnGraphRecharge.TabIndex = 32
        Me.btnGraphRecharge.Text = "Plot Monthly Recharge"
        Me.btnGraphRecharge.UseVisualStyleBackColor = True
        '
        'btnWriteASCIIOutput
        '
        Me.btnWriteASCIIOutput.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnWriteASCIIOutput.Location = New System.Drawing.Point(280, 101)
        Me.btnWriteASCIIOutput.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnWriteASCIIOutput.Name = "btnWriteASCIIOutput"
        Me.btnWriteASCIIOutput.Size = New System.Drawing.Size(171, 28)
        Me.btnWriteASCIIOutput.TabIndex = 10
        Me.btnWriteASCIIOutput.Text = "Write ASCII Outputs"
        Me.btnWriteASCIIOutput.UseVisualStyleBackColor = True
        '
        'txtOutputDir
        '
        Me.txtOutputDir.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtOutputDir.Location = New System.Drawing.Point(104, 25)
        Me.txtOutputDir.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txtOutputDir.Name = "txtOutputDir"
        Me.txtOutputDir.Size = New System.Drawing.Size(345, 22)
        Me.txtOutputDir.TabIndex = 8
        '
        'lblOutputDir
        '
        Me.lblOutputDir.AutoSize = True
        Me.lblOutputDir.Location = New System.Drawing.Point(11, 28)
        Me.lblOutputDir.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblOutputDir.Name = "lblOutputDir"
        Me.lblOutputDir.Size = New System.Drawing.Size(91, 17)
        Me.lblOutputDir.TabIndex = 31
        Me.lblOutputDir.Text = "Output folder"
        '
        'txtOutputRootName
        '
        Me.txtOutputRootName.Location = New System.Drawing.Point(152, 60)
        Me.txtOutputRootName.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txtOutputRootName.Name = "txtOutputRootName"
        Me.txtOutputRootName.Size = New System.Drawing.Size(160, 22)
        Me.txtOutputRootName.TabIndex = 9
        '
        'lblBaseFilename
        '
        Me.lblBaseFilename.AutoSize = True
        Me.lblBaseFilename.Location = New System.Drawing.Point(8, 64)
        Me.lblBaseFilename.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblBaseFilename.Name = "lblBaseFilename"
        Me.lblBaseFilename.Size = New System.Drawing.Size(141, 17)
        Me.lblBaseFilename.TabIndex = 30
        Me.lblBaseFilename.Text = "Base output filename"
        '
        'gbParameters
        '
        Me.gbParameters.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbParameters.Controls.Add(Me.lblUnit4)
        Me.gbParameters.Controls.Add(Me.txtNewMinFlow)
        Me.gbParameters.Controls.Add(Me.lblNewMinFlow)
        Me.gbParameters.Controls.Add(Me.cboAnteRecess)
        Me.gbParameters.Controls.Add(Me.btnRecessionIndex)
        Me.gbParameters.Controls.Add(Me.lblUnit3)
        Me.gbParameters.Controls.Add(Me.lblUnit2)
        Me.gbParameters.Controls.Add(Me.lblAnteRecess)
        Me.gbParameters.Controls.Add(Me.txtRecessionIndex)
        Me.gbParameters.Controls.Add(Me.lblRecessionIndex)
        Me.gbParameters.Controls.Add(Me.lblDrainageArea)
        Me.gbParameters.Controls.Add(Me.txtDrainageArea)
        Me.gbParameters.Controls.Add(Me.lblDrainageAreaUnits)
        Me.gbParameters.Location = New System.Drawing.Point(16, 181)
        Me.gbParameters.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.gbParameters.Name = "gbParameters"
        Me.gbParameters.Padding = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.gbParameters.Size = New System.Drawing.Size(459, 208)
        Me.gbParameters.TabIndex = 13
        Me.gbParameters.TabStop = False
        Me.gbParameters.Text = "Parameters"
        '
        'cboAnteRecess
        '
        Me.cboAnteRecess.FormattingEnabled = True
        Me.cboAnteRecess.Location = New System.Drawing.Point(172, 127)
        Me.cboAnteRecess.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.cboAnteRecess.Name = "cboAnteRecess"
        Me.cboAnteRecess.Size = New System.Drawing.Size(120, 24)
        Me.cboAnteRecess.TabIndex = 7
        '
        'btnRecessionIndex
        '
        Me.btnRecessionIndex.Location = New System.Drawing.Point(172, 87)
        Me.btnRecessionIndex.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnRecessionIndex.Name = "btnRecessionIndex"
        Me.btnRecessionIndex.Size = New System.Drawing.Size(228, 28)
        Me.btnRecessionIndex.TabIndex = 6
        Me.btnRecessionIndex.Text = "Browse Recession Index"
        Me.btnRecessionIndex.UseVisualStyleBackColor = True
        '
        'lblUnit3
        '
        Me.lblUnit3.AutoSize = True
        Me.lblUnit3.Location = New System.Drawing.Point(300, 130)
        Me.lblUnit3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblUnit3.Name = "lblUnit3"
        Me.lblUnit3.Size = New System.Drawing.Size(38, 17)
        Me.lblUnit3.TabIndex = 12
        Me.lblUnit3.Text = "days"
        '
        'lblUnit2
        '
        Me.lblUnit2.AutoSize = True
        Me.lblUnit2.Location = New System.Drawing.Point(300, 60)
        Me.lblUnit2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblUnit2.Name = "lblUnit2"
        Me.lblUnit2.Size = New System.Drawing.Size(108, 17)
        Me.lblUnit2.TabIndex = 11
        Me.lblUnit2.Text = "days/logQ cycle"
        '
        'lblAnteRecess
        '
        Me.lblAnteRecess.AutoSize = True
        Me.lblAnteRecess.Location = New System.Drawing.Point(8, 117)
        Me.lblAnteRecess.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblAnteRecess.Name = "lblAnteRecess"
        Me.lblAnteRecess.Size = New System.Drawing.Size(150, 34)
        Me.lblAnteRecess.TabIndex = 10
        Me.lblAnteRecess.Text = "Requirement of" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Antecedent Recession"
        '
        'txtRecessionIndex
        '
        Me.txtRecessionIndex.Location = New System.Drawing.Point(172, 57)
        Me.txtRecessionIndex.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txtRecessionIndex.Name = "txtRecessionIndex"
        Me.txtRecessionIndex.Size = New System.Drawing.Size(120, 22)
        Me.txtRecessionIndex.TabIndex = 5
        '
        'lblRecessionIndex
        '
        Me.lblRecessionIndex.AutoSize = True
        Me.lblRecessionIndex.Location = New System.Drawing.Point(8, 57)
        Me.lblRecessionIndex.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblRecessionIndex.Name = "lblRecessionIndex"
        Me.lblRecessionIndex.Size = New System.Drawing.Size(111, 17)
        Me.lblRecessionIndex.TabIndex = 8
        Me.lblRecessionIndex.Text = "Recession Index"
        '
        'lblDrainageArea
        '
        Me.lblDrainageArea.AutoSize = True
        Me.lblDrainageArea.Location = New System.Drawing.Point(8, 26)
        Me.lblDrainageArea.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblDrainageArea.Name = "lblDrainageArea"
        Me.lblDrainageArea.Size = New System.Drawing.Size(100, 17)
        Me.lblDrainageArea.TabIndex = 7
        Me.lblDrainageArea.Text = "Drainage Area"
        '
        'txtDrainageArea
        '
        Me.txtDrainageArea.Location = New System.Drawing.Point(172, 27)
        Me.txtDrainageArea.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txtDrainageArea.Name = "txtDrainageArea"
        Me.txtDrainageArea.Size = New System.Drawing.Size(120, 22)
        Me.txtDrainageArea.TabIndex = 4
        '
        'lblDrainageAreaUnits
        '
        Me.lblDrainageAreaUnits.AutoSize = True
        Me.lblDrainageAreaUnits.Location = New System.Drawing.Point(300, 30)
        Me.lblDrainageAreaUnits.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblDrainageAreaUnits.Name = "lblDrainageAreaUnits"
        Me.lblDrainageAreaUnits.Size = New System.Drawing.Size(41, 17)
        Me.lblDrainageAreaUnits.TabIndex = 4
        Me.lblDrainageAreaUnits.Text = "sq mi"
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuFile, Me.mnuAnalysis, Me.mnuHelp})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Padding = New System.Windows.Forms.Padding(8, 2, 0, 2)
        Me.MenuStrip1.Size = New System.Drawing.Size(489, 28)
        Me.MenuStrip1.TabIndex = 14
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'mnuFile
        '
        Me.mnuFile.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuFileSelectData})
        Me.mnuFile.Name = "mnuFile"
        Me.mnuFile.Size = New System.Drawing.Size(44, 24)
        Me.mnuFile.Text = "File"
        '
        'mnuFileSelectData
        '
        Me.mnuFileSelectData.Name = "mnuFileSelectData"
        Me.mnuFileSelectData.Size = New System.Drawing.Size(154, 24)
        Me.mnuFileSelectData.Text = "Select Data"
        '
        'mnuAnalysis
        '
        Me.mnuAnalysis.Name = "mnuAnalysis"
        Me.mnuAnalysis.Size = New System.Drawing.Size(74, 24)
        Me.mnuAnalysis.Text = "Analysis"
        '
        'mnuHelp
        '
        Me.mnuHelp.Name = "mnuHelp"
        Me.mnuHelp.Size = New System.Drawing.Size(53, 24)
        Me.mnuHelp.Text = "Help"
        '
        'lblNewMinFlow
        '
        Me.lblNewMinFlow.AutoSize = True
        Me.lblNewMinFlow.Location = New System.Drawing.Point(8, 171)
        Me.lblNewMinFlow.Name = "lblNewMinFlow"
        Me.lblNewMinFlow.Size = New System.Drawing.Size(158, 17)
        Me.lblNewMinFlow.TabIndex = 13
        Me.lblNewMinFlow.Text = "Replace Zero Flow With"
        '
        'txtNewMinFlow
        '
        Me.txtNewMinFlow.Location = New System.Drawing.Point(172, 168)
        Me.txtNewMinFlow.Name = "txtNewMinFlow"
        Me.txtNewMinFlow.Size = New System.Drawing.Size(120, 22)
        Me.txtNewMinFlow.TabIndex = 14
        Me.txtNewMinFlow.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblUnit4
        '
        Me.lblUnit4.AutoSize = True
        Me.lblUnit4.Location = New System.Drawing.Point(298, 171)
        Me.lblUnit4.Name = "lblUnit4"
        Me.lblUnit4.Size = New System.Drawing.Size(26, 17)
        Me.lblUnit4.TabIndex = 15
        Me.lblUnit4.Text = "cfs"
        '
        'frmUSGSRora
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(489, 547)
        Me.Controls.Add(Me.gbParameters)
        Me.Controls.Add(Me.gbOutputFileSpecs)
        Me.Controls.Add(Me.gbDates)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Name = "frmUSGSRora"
        Me.Text = "USGS RORA"
        Me.gbDates.ResumeLayout(False)
        Me.gbDates.PerformLayout()
        Me.gbOutputFileSpecs.ResumeLayout(False)
        Me.gbOutputFileSpecs.PerformLayout()
        Me.gbParameters.ResumeLayout(False)
        Me.gbParameters.PerformLayout()
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents gbDates As System.Windows.Forms.GroupBox
    Friend WithEvents txtEndDateUser As System.Windows.Forms.TextBox
    Friend WithEvents txtStartDateUser As System.Windows.Forms.TextBox
    Friend WithEvents btnExamineData As System.Windows.Forms.Button
    Friend WithEvents txtDataEnd As System.Windows.Forms.TextBox
    Friend WithEvents txtDataStart As System.Windows.Forms.TextBox
    Friend WithEvents lblDataEnd As System.Windows.Forms.Label
    Friend WithEvents lblDataStart As System.Windows.Forms.Label
    Friend WithEvents gbOutputFileSpecs As System.Windows.Forms.GroupBox
    Friend WithEvents btnWriteASCIIOutput As System.Windows.Forms.Button
    Friend WithEvents txtOutputDir As System.Windows.Forms.TextBox
    Friend WithEvents lblOutputDir As System.Windows.Forms.Label
    Friend WithEvents txtOutputRootName As System.Windows.Forms.TextBox
    Friend WithEvents lblBaseFilename As System.Windows.Forms.Label
    Friend WithEvents gbParameters As System.Windows.Forms.GroupBox
    Friend WithEvents txtDrainageArea As System.Windows.Forms.TextBox
    Friend WithEvents lblDrainageAreaUnits As System.Windows.Forms.Label
    Friend WithEvents lblDrainageArea As System.Windows.Forms.Label
    Friend WithEvents lblUnit2 As System.Windows.Forms.Label
    Friend WithEvents lblAnteRecess As System.Windows.Forms.Label
    Friend WithEvents txtRecessionIndex As System.Windows.Forms.TextBox
    Friend WithEvents lblRecessionIndex As System.Windows.Forms.Label
    Friend WithEvents lblUnit3 As System.Windows.Forms.Label
    Friend WithEvents btnRecessionIndex As System.Windows.Forms.Button
    Friend WithEvents cboAnteRecess As System.Windows.Forms.ComboBox
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents mnuFile As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuFileSelectData As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAnalysis As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuHelp As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnGraphRecharge As System.Windows.Forms.Button
    Friend WithEvents lblAnalysisDates As System.Windows.Forms.Label
    Friend WithEvents lblPeriodOfRecord As System.Windows.Forms.Label
    Friend WithEvents txtNewMinFlow As System.Windows.Forms.TextBox
    Friend WithEvents lblNewMinFlow As System.Windows.Forms.Label
    Friend WithEvents lblUnit4 As System.Windows.Forms.Label
End Class
