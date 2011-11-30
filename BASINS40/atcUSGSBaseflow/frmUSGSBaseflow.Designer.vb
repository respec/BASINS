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
        Me.Label1 = New System.Windows.Forms.Label
        Me.cboBFMothod = New System.Windows.Forms.ComboBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.txtDrainageArea = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.btnOK = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnExamineData = New System.Windows.Forms.Button
        Me.btnFindStations = New System.Windows.Forms.Button
        Me.gbDates = New System.Windows.Forms.GroupBox
        Me.txtEndDateUser = New System.Windows.Forms.TextBox
        Me.txtStartDateUser = New System.Windows.Forms.TextBox
        Me.txtDataEnd = New System.Windows.Forms.TextBox
        Me.txtDataStart = New System.Windows.Forms.TextBox
        Me.lblDataEnd = New System.Windows.Forms.Label
        Me.lblDataStart = New System.Windows.Forms.Label
        Me.toolTip1 = New System.Windows.Forms.ToolTip(Me.components)
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
        Me.gbOutputFileSpecs = New System.Windows.Forms.GroupBox
        Me.btnGraphCDist = New System.Windows.Forms.Button
        Me.btnGraphDurationPUA = New System.Windows.Forms.Button
        Me.btnGraphDuration = New System.Windows.Forms.Button
        Me.btnGraphTimeseries = New System.Windows.Forms.Button
        Me.btnWriteASCIIOutput = New System.Windows.Forms.Button
        Me.chkTabDelimited = New System.Windows.Forms.CheckBox
        Me.txtOutputDir = New System.Windows.Forms.TextBox
        Me.lblOutputDir = New System.Windows.Forms.Label
        Me.gbDates.SuspendLayout()
        Me.MenuStrip1.SuspendLayout()
        Me.gbOutputFileSpecs.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(14, 39)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(76, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Select Method"
        '
        'cboBFMothod
        '
        Me.cboBFMothod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboBFMothod.FormattingEnabled = True
        Me.cboBFMothod.Items.AddRange(New Object() {"HySEP-Fixed", "HySEP-LocMin", "HySEP-Slide", "PART"})
        Me.cboBFMothod.Location = New System.Drawing.Point(96, 36)
        Me.cboBFMothod.Name = "cboBFMothod"
        Me.cboBFMothod.Size = New System.Drawing.Size(121, 21)
        Me.cboBFMothod.TabIndex = 1
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(15, 66)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(75, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Drainage Area"
        '
        'txtDrainageArea
        '
        Me.txtDrainageArea.Location = New System.Drawing.Point(96, 63)
        Me.txtDrainageArea.Name = "txtDrainageArea"
        Me.txtDrainageArea.Size = New System.Drawing.Size(121, 20)
        Me.txtDrainageArea.TabIndex = 3
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(223, 66)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(31, 13)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "sq mi"
        '
        'btnOK
        '
        Me.btnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOK.Location = New System.Drawing.Point(200, 437)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(75, 23)
        Me.btnOK.TabIndex = 23
        Me.btnOK.Text = "Calculate"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.Location = New System.Drawing.Point(281, 437)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 24
        Me.btnCancel.Text = "Close"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnExamineData
        '
        Me.btnExamineData.Location = New System.Drawing.Point(241, 78)
        Me.btnExamineData.Name = "btnExamineData"
        Me.btnExamineData.Size = New System.Drawing.Size(94, 23)
        Me.btnExamineData.TabIndex = 25
        Me.btnExamineData.Text = "Examine Data"
        Me.btnExamineData.UseVisualStyleBackColor = True
        '
        'btnFindStations
        '
        Me.btnFindStations.Location = New System.Drawing.Point(261, 61)
        Me.btnFindStations.Name = "btnFindStations"
        Me.btnFindStations.Size = New System.Drawing.Size(98, 23)
        Me.btnFindStations.TabIndex = 26
        Me.btnFindStations.Text = "Browse Stations"
        Me.btnFindStations.UseVisualStyleBackColor = True
        Me.btnFindStations.Visible = False
        '
        'gbDates
        '
        Me.gbDates.Controls.Add(Me.txtEndDateUser)
        Me.gbDates.Controls.Add(Me.txtStartDateUser)
        Me.gbDates.Controls.Add(Me.btnExamineData)
        Me.gbDates.Controls.Add(Me.txtDataEnd)
        Me.gbDates.Controls.Add(Me.txtDataStart)
        Me.gbDates.Controls.Add(Me.lblDataEnd)
        Me.gbDates.Controls.Add(Me.lblDataStart)
        Me.gbDates.Location = New System.Drawing.Point(18, 92)
        Me.gbDates.Name = "gbDates"
        Me.gbDates.Size = New System.Drawing.Size(341, 109)
        Me.gbDates.TabIndex = 27
        Me.gbDates.TabStop = False
        Me.gbDates.Text = "Baseflow Analysis Dates"
        '
        'txtEndDateUser
        '
        Me.txtEndDateUser.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtEndDateUser.Location = New System.Drawing.Point(194, 52)
        Me.txtEndDateUser.Name = "txtEndDateUser"
        Me.txtEndDateUser.Size = New System.Drawing.Size(141, 20)
        Me.txtEndDateUser.TabIndex = 27
        Me.toolTip1.SetToolTip(Me.txtEndDateUser, "User-specified ending date: yyyy/mm/dd")
        '
        'txtStartDateUser
        '
        Me.txtStartDateUser.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtStartDateUser.Location = New System.Drawing.Point(194, 25)
        Me.txtStartDateUser.Name = "txtStartDateUser"
        Me.txtStartDateUser.Size = New System.Drawing.Size(141, 20)
        Me.txtStartDateUser.TabIndex = 26
        Me.toolTip1.SetToolTip(Me.txtStartDateUser, "User-specified starting date: yyyy/mm/dd")
        '
        'txtDataEnd
        '
        Me.txtDataEnd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtDataEnd.Location = New System.Drawing.Point(68, 53)
        Me.txtDataEnd.Name = "txtDataEnd"
        Me.txtDataEnd.ReadOnly = True
        Me.txtDataEnd.Size = New System.Drawing.Size(119, 20)
        Me.txtDataEnd.TabIndex = 3
        '
        'txtDataStart
        '
        Me.txtDataStart.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtDataStart.Location = New System.Drawing.Point(68, 26)
        Me.txtDataStart.Name = "txtDataStart"
        Me.txtDataStart.ReadOnly = True
        Me.txtDataStart.Size = New System.Drawing.Size(119, 20)
        Me.txtDataStart.TabIndex = 2
        '
        'lblDataEnd
        '
        Me.lblDataEnd.AutoSize = True
        Me.lblDataEnd.Location = New System.Drawing.Point(6, 52)
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
        Me.lblDataStart.Location = New System.Drawing.Point(6, 26)
        Me.lblDataStart.Name = "lblDataStart"
        Me.lblDataStart.Size = New System.Drawing.Size(55, 13)
        Me.lblDataStart.TabIndex = 0
        Me.lblDataStart.Text = "Data Start"
        '
        'MenuStrip1
        '
        Me.MenuStrip1.BackColor = System.Drawing.SystemColors.Window
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuFile, Me.mnuAnalysis, Me.mnuHelp})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(368, 24)
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
        Me.txtOutputRootName.TabIndex = 29
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
        'gbOutputFileSpecs
        '
        Me.gbOutputFileSpecs.Controls.Add(Me.btnGraphCDist)
        Me.gbOutputFileSpecs.Controls.Add(Me.btnGraphDurationPUA)
        Me.gbOutputFileSpecs.Controls.Add(Me.btnGraphDuration)
        Me.gbOutputFileSpecs.Controls.Add(Me.btnGraphTimeseries)
        Me.gbOutputFileSpecs.Controls.Add(Me.btnWriteASCIIOutput)
        Me.gbOutputFileSpecs.Controls.Add(Me.chkTabDelimited)
        Me.gbOutputFileSpecs.Controls.Add(Me.txtOutputDir)
        Me.gbOutputFileSpecs.Controls.Add(Me.lblOutputDir)
        Me.gbOutputFileSpecs.Controls.Add(Me.txtOutputRootName)
        Me.gbOutputFileSpecs.Controls.Add(Me.lblBaseFilename)
        Me.gbOutputFileSpecs.Location = New System.Drawing.Point(18, 208)
        Me.gbOutputFileSpecs.Name = "gbOutputFileSpecs"
        Me.gbOutputFileSpecs.Size = New System.Drawing.Size(342, 223)
        Me.gbOutputFileSpecs.TabIndex = 31
        Me.gbOutputFileSpecs.TabStop = False
        Me.gbOutputFileSpecs.Text = "Output Specifications"
        '
        'btnGraphCDist
        '
        Me.btnGraphCDist.Location = New System.Drawing.Point(9, 193)
        Me.btnGraphCDist.Name = "btnGraphCDist"
        Me.btnGraphCDist.Size = New System.Drawing.Size(323, 23)
        Me.btnGraphCDist.TabIndex = 38
        Me.btnGraphCDist.Text = "Graph: Cummulative Distribution"
        Me.btnGraphCDist.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnGraphCDist.UseVisualStyleBackColor = True
        '
        'btnGraphDurationPUA
        '
        Me.btnGraphDurationPUA.Location = New System.Drawing.Point(9, 164)
        Me.btnGraphDurationPUA.Name = "btnGraphDurationPUA"
        Me.btnGraphDurationPUA.Size = New System.Drawing.Size(323, 23)
        Me.btnGraphDurationPUA.TabIndex = 37
        Me.btnGraphDurationPUA.Text = "Graph: Flow Duration per unit area"
        Me.btnGraphDurationPUA.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnGraphDurationPUA.UseVisualStyleBackColor = True
        '
        'btnGraphDuration
        '
        Me.btnGraphDuration.Location = New System.Drawing.Point(9, 135)
        Me.btnGraphDuration.Name = "btnGraphDuration"
        Me.btnGraphDuration.Size = New System.Drawing.Size(323, 23)
        Me.btnGraphDuration.TabIndex = 36
        Me.btnGraphDuration.Text = "Graph: Flow Duration"
        Me.btnGraphDuration.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnGraphDuration.UseVisualStyleBackColor = True
        '
        'btnGraphTimeseries
        '
        Me.btnGraphTimeseries.Location = New System.Drawing.Point(9, 105)
        Me.btnGraphTimeseries.Name = "btnGraphTimeseries"
        Me.btnGraphTimeseries.Size = New System.Drawing.Size(323, 23)
        Me.btnGraphTimeseries.TabIndex = 35
        Me.btnGraphTimeseries.Text = "Graph: Timeseries"
        Me.btnGraphTimeseries.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnGraphTimeseries.UseVisualStyleBackColor = True
        '
        'btnWriteASCIIOutput
        '
        Me.btnWriteASCIIOutput.Location = New System.Drawing.Point(9, 75)
        Me.btnWriteASCIIOutput.Name = "btnWriteASCIIOutput"
        Me.btnWriteASCIIOutput.Size = New System.Drawing.Size(323, 23)
        Me.btnWriteASCIIOutput.TabIndex = 34
        Me.btnWriteASCIIOutput.Text = "Write ASCII Outputs"
        Me.btnWriteASCIIOutput.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnWriteASCIIOutput.UseVisualStyleBackColor = True
        '
        'chkTabDelimited
        '
        Me.chkTabDelimited.AutoSize = True
        Me.chkTabDelimited.Location = New System.Drawing.Point(241, 51)
        Me.chkTabDelimited.Name = "chkTabDelimited"
        Me.chkTabDelimited.Size = New System.Drawing.Size(91, 17)
        Me.chkTabDelimited.TabIndex = 33
        Me.chkTabDelimited.Text = "Tab-Delimited"
        Me.chkTabDelimited.UseVisualStyleBackColor = True
        '
        'txtOutputDir
        '
        Me.txtOutputDir.Location = New System.Drawing.Point(78, 20)
        Me.txtOutputDir.Name = "txtOutputDir"
        Me.txtOutputDir.Size = New System.Drawing.Size(257, 20)
        Me.txtOutputDir.TabIndex = 32
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
        'frmUSGSBaseflow
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(368, 472)
        Me.Controls.Add(Me.gbDates)
        Me.Controls.Add(Me.btnFindStations)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.txtDrainageArea)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.cboBFMothod)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Controls.Add(Me.gbOutputFileSpecs)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "frmUSGSBaseflow"
        Me.Text = "Baseflow Separation"
        Me.gbDates.ResumeLayout(False)
        Me.gbDates.PerformLayout()
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.gbOutputFileSpecs.ResumeLayout(False)
        Me.gbOutputFileSpecs.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cboBFMothod As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtDrainageArea As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnExamineData As System.Windows.Forms.Button
    Friend WithEvents btnFindStations As System.Windows.Forms.Button
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
    Friend WithEvents gbOutputFileSpecs As System.Windows.Forms.GroupBox
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
End Class
