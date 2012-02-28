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
        Me.lblAnalysisDates = New System.Windows.Forms.Label
        Me.lblPeriodOfRecord = New System.Windows.Forms.Label
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
        Me.gbDates.Location = New System.Drawing.Point(12, 27)
        Me.gbDates.Name = "gbDates"
        Me.gbDates.Size = New System.Drawing.Size(344, 114)
        Me.gbDates.TabIndex = 8
        Me.gbDates.TabStop = False
        Me.gbDates.Text = "Analysis Dates"
        '
        'txtEndDateUser
        '
        Me.txtEndDateUser.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtEndDateUser.Location = New System.Drawing.Point(194, 58)
        Me.txtEndDateUser.Name = "txtEndDateUser"
        Me.txtEndDateUser.Size = New System.Drawing.Size(144, 20)
        Me.txtEndDateUser.TabIndex = 2
        '
        'txtStartDateUser
        '
        Me.txtStartDateUser.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtStartDateUser.Location = New System.Drawing.Point(194, 31)
        Me.txtStartDateUser.Name = "txtStartDateUser"
        Me.txtStartDateUser.Size = New System.Drawing.Size(144, 20)
        Me.txtStartDateUser.TabIndex = 1
        '
        'btnExamineData
        '
        Me.btnExamineData.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnExamineData.Location = New System.Drawing.Point(68, 85)
        Me.btnExamineData.Name = "btnExamineData"
        Me.btnExamineData.Size = New System.Drawing.Size(94, 23)
        Me.btnExamineData.TabIndex = 3
        Me.btnExamineData.Text = "Examine Data"
        Me.btnExamineData.UseVisualStyleBackColor = True
        '
        'txtDataEnd
        '
        Me.txtDataEnd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtDataEnd.Location = New System.Drawing.Point(68, 59)
        Me.txtDataEnd.Name = "txtDataEnd"
        Me.txtDataEnd.ReadOnly = True
        Me.txtDataEnd.Size = New System.Drawing.Size(119, 20)
        Me.txtDataEnd.TabIndex = 12
        '
        'txtDataStart
        '
        Me.txtDataStart.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtDataStart.Location = New System.Drawing.Point(68, 32)
        Me.txtDataStart.Name = "txtDataStart"
        Me.txtDataStart.ReadOnly = True
        Me.txtDataStart.Size = New System.Drawing.Size(119, 20)
        Me.txtDataStart.TabIndex = 11
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
        Me.gbOutputFileSpecs.Location = New System.Drawing.Point(12, 291)
        Me.gbOutputFileSpecs.Name = "gbOutputFileSpecs"
        Me.gbOutputFileSpecs.Size = New System.Drawing.Size(344, 111)
        Me.gbOutputFileSpecs.TabIndex = 12
        Me.gbOutputFileSpecs.TabStop = False
        Me.gbOutputFileSpecs.Text = "Text Output"
        '
        'btnGraphRecharge
        '
        Me.btnGraphRecharge.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGraphRecharge.Location = New System.Drawing.Point(78, 82)
        Me.btnGraphRecharge.Name = "btnGraphRecharge"
        Me.btnGraphRecharge.Size = New System.Drawing.Size(126, 23)
        Me.btnGraphRecharge.TabIndex = 32
        Me.btnGraphRecharge.Text = "Plot Monthly Recharge"
        Me.btnGraphRecharge.UseVisualStyleBackColor = True
        '
        'btnWriteASCIIOutput
        '
        Me.btnWriteASCIIOutput.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnWriteASCIIOutput.Location = New System.Drawing.Point(210, 82)
        Me.btnWriteASCIIOutput.Name = "btnWriteASCIIOutput"
        Me.btnWriteASCIIOutput.Size = New System.Drawing.Size(128, 23)
        Me.btnWriteASCIIOutput.TabIndex = 10
        Me.btnWriteASCIIOutput.Text = "Write ASCII Outputs"
        Me.btnWriteASCIIOutput.UseVisualStyleBackColor = True
        '
        'txtOutputDir
        '
        Me.txtOutputDir.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtOutputDir.Location = New System.Drawing.Point(78, 20)
        Me.txtOutputDir.Name = "txtOutputDir"
        Me.txtOutputDir.Size = New System.Drawing.Size(260, 20)
        Me.txtOutputDir.TabIndex = 8
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
        'txtOutputRootName
        '
        Me.txtOutputRootName.Location = New System.Drawing.Point(114, 49)
        Me.txtOutputRootName.Name = "txtOutputRootName"
        Me.txtOutputRootName.Size = New System.Drawing.Size(121, 20)
        Me.txtOutputRootName.TabIndex = 9
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
        'gbParameters
        '
        Me.gbParameters.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
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
        Me.gbParameters.Location = New System.Drawing.Point(12, 147)
        Me.gbParameters.Name = "gbParameters"
        Me.gbParameters.Size = New System.Drawing.Size(344, 138)
        Me.gbParameters.TabIndex = 13
        Me.gbParameters.TabStop = False
        Me.gbParameters.Text = "Parameters"
        '
        'cboAnteRecess
        '
        Me.cboAnteRecess.FormattingEnabled = True
        Me.cboAnteRecess.Location = New System.Drawing.Point(129, 106)
        Me.cboAnteRecess.Name = "cboAnteRecess"
        Me.cboAnteRecess.Size = New System.Drawing.Size(86, 21)
        Me.cboAnteRecess.TabIndex = 7
        '
        'btnRecessionIndex
        '
        Me.btnRecessionIndex.Location = New System.Drawing.Point(126, 69)
        Me.btnRecessionIndex.Name = "btnRecessionIndex"
        Me.btnRecessionIndex.Size = New System.Drawing.Size(171, 23)
        Me.btnRecessionIndex.TabIndex = 6
        Me.btnRecessionIndex.Text = "Browse Recession Index"
        Me.btnRecessionIndex.UseVisualStyleBackColor = True
        '
        'lblUnit3
        '
        Me.lblUnit3.AutoSize = True
        Me.lblUnit3.Location = New System.Drawing.Point(221, 109)
        Me.lblUnit3.Name = "lblUnit3"
        Me.lblUnit3.Size = New System.Drawing.Size(29, 13)
        Me.lblUnit3.TabIndex = 12
        Me.lblUnit3.Text = "days"
        '
        'lblUnit2
        '
        Me.lblUnit2.AutoSize = True
        Me.lblUnit2.Location = New System.Drawing.Point(221, 46)
        Me.lblUnit2.Name = "lblUnit2"
        Me.lblUnit2.Size = New System.Drawing.Size(84, 13)
        Me.lblUnit2.TabIndex = 11
        Me.lblUnit2.Text = "days/logQ cycle"
        '
        'lblAnteRecess
        '
        Me.lblAnteRecess.AutoSize = True
        Me.lblAnteRecess.Location = New System.Drawing.Point(7, 102)
        Me.lblAnteRecess.Name = "lblAnteRecess"
        Me.lblAnteRecess.Size = New System.Drawing.Size(115, 26)
        Me.lblAnteRecess.TabIndex = 10
        Me.lblAnteRecess.Text = "Requirement of" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Antecedent Recession"
        '
        'txtRecessionIndex
        '
        Me.txtRecessionIndex.Location = New System.Drawing.Point(126, 43)
        Me.txtRecessionIndex.Name = "txtRecessionIndex"
        Me.txtRecessionIndex.Size = New System.Drawing.Size(89, 20)
        Me.txtRecessionIndex.TabIndex = 5
        '
        'lblRecessionIndex
        '
        Me.lblRecessionIndex.AutoSize = True
        Me.lblRecessionIndex.Location = New System.Drawing.Point(7, 46)
        Me.lblRecessionIndex.Name = "lblRecessionIndex"
        Me.lblRecessionIndex.Size = New System.Drawing.Size(86, 13)
        Me.lblRecessionIndex.TabIndex = 8
        Me.lblRecessionIndex.Text = "Recession Index"
        '
        'lblDrainageArea
        '
        Me.lblDrainageArea.AutoSize = True
        Me.lblDrainageArea.Location = New System.Drawing.Point(7, 20)
        Me.lblDrainageArea.Name = "lblDrainageArea"
        Me.lblDrainageArea.Size = New System.Drawing.Size(75, 13)
        Me.lblDrainageArea.TabIndex = 7
        Me.lblDrainageArea.Text = "Drainage Area"
        '
        'txtDrainageArea
        '
        Me.txtDrainageArea.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDrainageArea.Location = New System.Drawing.Point(126, 17)
        Me.txtDrainageArea.Name = "txtDrainageArea"
        Me.txtDrainageArea.Size = New System.Drawing.Size(89, 20)
        Me.txtDrainageArea.TabIndex = 4
        '
        'lblDrainageAreaUnits
        '
        Me.lblDrainageAreaUnits.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblDrainageAreaUnits.AutoSize = True
        Me.lblDrainageAreaUnits.Location = New System.Drawing.Point(221, 20)
        Me.lblDrainageAreaUnits.Name = "lblDrainageAreaUnits"
        Me.lblDrainageAreaUnits.Size = New System.Drawing.Size(31, 13)
        Me.lblDrainageAreaUnits.TabIndex = 4
        Me.lblDrainageAreaUnits.Text = "sq mi"
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuFile, Me.mnuAnalysis, Me.mnuHelp})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(367, 27)
        Me.MenuStrip1.TabIndex = 14
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'mnuFile
        '
        Me.mnuFile.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuFileSelectData})
        Me.mnuFile.Name = "mnuFile"
        Me.mnuFile.Size = New System.Drawing.Size(45, 23)
        Me.mnuFile.Text = "File"
        '
        'mnuFileSelectData
        '
        Me.mnuFileSelectData.Name = "mnuFileSelectData"
        Me.mnuFileSelectData.Size = New System.Drawing.Size(172, 24)
        Me.mnuFileSelectData.Text = "Select Data"
        '
        'mnuAnalysis
        '
        Me.mnuAnalysis.Name = "mnuAnalysis"
        Me.mnuAnalysis.Size = New System.Drawing.Size(79, 23)
        Me.mnuAnalysis.Text = "Analysis"
        '
        'mnuHelp
        '
        Me.mnuHelp.Name = "mnuHelp"
        Me.mnuHelp.Size = New System.Drawing.Size(53, 23)
        Me.mnuHelp.Text = "Help"
        '
        'lblAnalysisDates
        '
        Me.lblAnalysisDates.AutoSize = True
        Me.lblAnalysisDates.Location = New System.Drawing.Point(191, 16)
        Me.lblAnalysisDates.Name = "lblAnalysisDates"
        Me.lblAnalysisDates.Size = New System.Drawing.Size(76, 13)
        Me.lblAnalysisDates.TabIndex = 14
        Me.lblAnalysisDates.Text = "Analysis Dates"
        '
        'lblPeriodOfRecord
        '
        Me.lblPeriodOfRecord.AutoSize = True
        Me.lblPeriodOfRecord.Location = New System.Drawing.Point(65, 16)
        Me.lblPeriodOfRecord.Name = "lblPeriodOfRecord"
        Me.lblPeriodOfRecord.Size = New System.Drawing.Size(87, 13)
        Me.lblPeriodOfRecord.TabIndex = 13
        Me.lblPeriodOfRecord.Text = "Period of Record"
        '
        'frmUSGSRora
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(367, 411)
        Me.Controls.Add(Me.gbParameters)
        Me.Controls.Add(Me.gbOutputFileSpecs)
        Me.Controls.Add(Me.gbDates)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.MenuStrip1
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
End Class
