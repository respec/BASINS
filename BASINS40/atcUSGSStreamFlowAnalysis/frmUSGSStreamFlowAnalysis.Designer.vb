<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUSGSStreamFlowAnalysis
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUSGSStreamFlowAnalysis))
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip
        Me.mnuFile = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuFileSelectData = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuOutput = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuOutputASCII = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuGraphBF = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuGraphTimeseries = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuGraphProbability = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuAnalysis = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuHelp = New System.Windows.Forms.ToolStripMenuItem
        Me.gbDates = New System.Windows.Forms.GroupBox
        Me.txtEndDateUser = New System.Windows.Forms.TextBox
        Me.txtStartDateUser = New System.Windows.Forms.TextBox
        Me.btnExamineData = New System.Windows.Forms.Button
        Me.txtDataEnd = New System.Windows.Forms.TextBox
        Me.txtDataStart = New System.Windows.Forms.TextBox
        Me.lblDataEnd = New System.Windows.Forms.Label
        Me.lblDataStart = New System.Windows.Forms.Label
        Me.gbMonthSeason = New System.Windows.Forms.GroupBox
        Me.rdoNoSeason = New System.Windows.Forms.RadioButton
        Me.rdoWinter = New System.Windows.Forms.RadioButton
        Me.rdoFall = New System.Windows.Forms.RadioButton
        Me.rdoSummer = New System.Windows.Forms.RadioButton
        Me.rdoSpring = New System.Windows.Forms.RadioButton
        Me.lstMonths = New System.Windows.Forms.ListBox
        Me.lblRecessionDays = New System.Windows.Forms.Label
        Me.txtMinRecessionDays = New System.Windows.Forms.TextBox
        Me.gbToolBar = New System.Windows.Forms.GroupBox
        Me.btnSummary = New System.Windows.Forms.Button
        Me.btnAnalyse = New System.Windows.Forms.Button
        Me.btnNextRecession = New System.Windows.Forms.Button
        Me.btnUndoRefine = New System.Windows.Forms.Button
        Me.btnRefineRecessionDuration = New System.Windows.Forms.Button
        Me.btnGraph = New System.Windows.Forms.Button
        Me.btnTable = New System.Windows.Forms.Button
        Me.lblOutpuDir = New System.Windows.Forms.Label
        Me.TextBox1 = New System.Windows.Forms.TextBox
        Me.scDisplay = New System.Windows.Forms.SplitContainer
        Me.tabRefine = New System.Windows.Forms.TabControl
        Me.tabRefineDays = New System.Windows.Forms.TabPage
        Me.btnSetDays = New System.Windows.Forms.Button
        Me.txtAskUserLastDayofSegment = New System.Windows.Forms.TextBox
        Me.txtAskUserFirstDayofSegment = New System.Windows.Forms.TextBox
        Me.lblLastDay = New System.Windows.Forms.Label
        Me.lblFirstDay = New System.Windows.Forms.Label
        Me.txtDisplayText = New System.Windows.Forms.TextBox
        Me.gbRnages = New System.Windows.Forms.GroupBox
        Me.Button1 = New System.Windows.Forms.Button
        Me.TextBox4 = New System.Windows.Forms.TextBox
        Me.TextBox3 = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.TextBox2 = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.MenuStrip1.SuspendLayout()
        Me.gbDates.SuspendLayout()
        Me.gbMonthSeason.SuspendLayout()
        Me.gbToolBar.SuspendLayout()
        Me.scDisplay.Panel1.SuspendLayout()
        Me.scDisplay.SuspendLayout()
        Me.tabRefine.SuspendLayout()
        Me.tabRefineDays.SuspendLayout()
        Me.gbRnages.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.BackColor = System.Drawing.SystemColors.Window
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuFile, Me.mnuAnalysis, Me.mnuHelp})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(614, 24)
        Me.MenuStrip1.TabIndex = 29
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
        Me.mnuGraphBF.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuGraphTimeseries, Me.mnuGraphProbability})
        Me.mnuGraphBF.Name = "mnuGraphBF"
        Me.mnuGraphBF.Size = New System.Drawing.Size(132, 22)
        Me.mnuGraphBF.Text = "Graph"
        '
        'mnuGraphTimeseries
        '
        Me.mnuGraphTimeseries.Name = "mnuGraphTimeseries"
        Me.mnuGraphTimeseries.Size = New System.Drawing.Size(131, 22)
        Me.mnuGraphTimeseries.Text = "TimeSeries"
        '
        'mnuGraphProbability
        '
        Me.mnuGraphProbability.Name = "mnuGraphProbability"
        Me.mnuGraphProbability.Size = New System.Drawing.Size(131, 22)
        Me.mnuGraphProbability.Text = "Probability"
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
        'gbDates
        '
        Me.gbDates.Controls.Add(Me.txtEndDateUser)
        Me.gbDates.Controls.Add(Me.txtStartDateUser)
        Me.gbDates.Controls.Add(Me.btnExamineData)
        Me.gbDates.Controls.Add(Me.txtDataEnd)
        Me.gbDates.Controls.Add(Me.txtDataStart)
        Me.gbDates.Controls.Add(Me.lblDataEnd)
        Me.gbDates.Controls.Add(Me.lblDataStart)
        Me.gbDates.Location = New System.Drawing.Point(12, 27)
        Me.gbDates.Name = "gbDates"
        Me.gbDates.Size = New System.Drawing.Size(301, 109)
        Me.gbDates.TabIndex = 30
        Me.gbDates.TabStop = False
        Me.gbDates.Text = "Streamflow Analysis Dates"
        '
        'txtEndDateUser
        '
        Me.txtEndDateUser.Location = New System.Drawing.Point(162, 52)
        Me.txtEndDateUser.Name = "txtEndDateUser"
        Me.txtEndDateUser.Size = New System.Drawing.Size(88, 20)
        Me.txtEndDateUser.TabIndex = 1
        '
        'txtStartDateUser
        '
        Me.txtStartDateUser.Location = New System.Drawing.Point(162, 26)
        Me.txtStartDateUser.Name = "txtStartDateUser"
        Me.txtStartDateUser.Size = New System.Drawing.Size(88, 20)
        Me.txtStartDateUser.TabIndex = 0
        '
        'btnExamineData
        '
        Me.btnExamineData.Location = New System.Drawing.Point(156, 78)
        Me.btnExamineData.Name = "btnExamineData"
        Me.btnExamineData.Size = New System.Drawing.Size(94, 23)
        Me.btnExamineData.TabIndex = 2
        Me.btnExamineData.Text = "Examine Data"
        Me.btnExamineData.UseVisualStyleBackColor = True
        '
        'txtDataEnd
        '
        Me.txtDataEnd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtDataEnd.Location = New System.Drawing.Point(68, 53)
        Me.txtDataEnd.Name = "txtDataEnd"
        Me.txtDataEnd.ReadOnly = True
        Me.txtDataEnd.Size = New System.Drawing.Size(88, 20)
        Me.txtDataEnd.TabIndex = 3
        '
        'txtDataStart
        '
        Me.txtDataStart.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtDataStart.Location = New System.Drawing.Point(68, 26)
        Me.txtDataStart.Name = "txtDataStart"
        Me.txtDataStart.ReadOnly = True
        Me.txtDataStart.Size = New System.Drawing.Size(88, 20)
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
        'gbMonthSeason
        '
        Me.gbMonthSeason.Controls.Add(Me.rdoNoSeason)
        Me.gbMonthSeason.Controls.Add(Me.rdoWinter)
        Me.gbMonthSeason.Controls.Add(Me.rdoFall)
        Me.gbMonthSeason.Controls.Add(Me.rdoSummer)
        Me.gbMonthSeason.Controls.Add(Me.rdoSpring)
        Me.gbMonthSeason.Controls.Add(Me.lstMonths)
        Me.gbMonthSeason.Location = New System.Drawing.Point(319, 27)
        Me.gbMonthSeason.Name = "gbMonthSeason"
        Me.gbMonthSeason.Size = New System.Drawing.Size(275, 155)
        Me.gbMonthSeason.TabIndex = 33
        Me.gbMonthSeason.TabStop = False
        Me.gbMonthSeason.Text = "Months and Season"
        '
        'rdoNoSeason
        '
        Me.rdoNoSeason.AutoSize = True
        Me.rdoNoSeason.Location = New System.Drawing.Point(6, 131)
        Me.rdoNoSeason.Name = "rdoNoSeason"
        Me.rdoNoSeason.Size = New System.Drawing.Size(122, 17)
        Me.rdoNoSeason.TabIndex = 8
        Me.rdoNoSeason.TabStop = True
        Me.rdoNoSeason.Text = "No particular season"
        Me.rdoNoSeason.UseVisualStyleBackColor = True
        '
        'rdoWinter
        '
        Me.rdoWinter.AutoSize = True
        Me.rdoWinter.Location = New System.Drawing.Point(183, 108)
        Me.rdoWinter.Name = "rdoWinter"
        Me.rdoWinter.Size = New System.Drawing.Size(56, 17)
        Me.rdoWinter.TabIndex = 7
        Me.rdoWinter.TabStop = True
        Me.rdoWinter.Text = "Winter"
        Me.rdoWinter.UseVisualStyleBackColor = True
        '
        'rdoFall
        '
        Me.rdoFall.AutoSize = True
        Me.rdoFall.Location = New System.Drawing.Point(136, 108)
        Me.rdoFall.Name = "rdoFall"
        Me.rdoFall.Size = New System.Drawing.Size(41, 17)
        Me.rdoFall.TabIndex = 6
        Me.rdoFall.TabStop = True
        Me.rdoFall.Text = "Fall"
        Me.rdoFall.UseVisualStyleBackColor = True
        '
        'rdoSummer
        '
        Me.rdoSummer.AutoSize = True
        Me.rdoSummer.Location = New System.Drawing.Point(67, 108)
        Me.rdoSummer.Name = "rdoSummer"
        Me.rdoSummer.Size = New System.Drawing.Size(63, 17)
        Me.rdoSummer.TabIndex = 5
        Me.rdoSummer.TabStop = True
        Me.rdoSummer.Text = "Summer"
        Me.rdoSummer.UseVisualStyleBackColor = True
        '
        'rdoSpring
        '
        Me.rdoSpring.AutoSize = True
        Me.rdoSpring.Location = New System.Drawing.Point(6, 108)
        Me.rdoSpring.Name = "rdoSpring"
        Me.rdoSpring.Size = New System.Drawing.Size(55, 17)
        Me.rdoSpring.TabIndex = 4
        Me.rdoSpring.TabStop = True
        Me.rdoSpring.Text = "Spring"
        Me.rdoSpring.UseVisualStyleBackColor = True
        '
        'lstMonths
        '
        Me.lstMonths.FormattingEnabled = True
        Me.lstMonths.Items.AddRange(New Object() {"January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"})
        Me.lstMonths.Location = New System.Drawing.Point(7, 20)
        Me.lstMonths.MultiColumn = True
        Me.lstMonths.Name = "lstMonths"
        Me.lstMonths.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple
        Me.lstMonths.Size = New System.Drawing.Size(256, 82)
        Me.lstMonths.TabIndex = 3
        '
        'lblRecessionDays
        '
        Me.lblRecessionDays.AutoSize = True
        Me.lblRecessionDays.Location = New System.Drawing.Point(9, 145)
        Me.lblRecessionDays.Name = "lblRecessionDays"
        Me.lblRecessionDays.Size = New System.Drawing.Size(223, 13)
        Me.lblRecessionDays.TabIndex = 34
        Me.lblRecessionDays.Text = "Specify minimum flow recession length in days"
        '
        'txtMinRecessionDays
        '
        Me.txtMinRecessionDays.Location = New System.Drawing.Point(238, 142)
        Me.txtMinRecessionDays.Name = "txtMinRecessionDays"
        Me.txtMinRecessionDays.Size = New System.Drawing.Size(75, 20)
        Me.txtMinRecessionDays.TabIndex = 9
        '
        'gbToolBar
        '
        Me.gbToolBar.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbToolBar.Controls.Add(Me.btnSummary)
        Me.gbToolBar.Controls.Add(Me.btnAnalyse)
        Me.gbToolBar.Controls.Add(Me.btnNextRecession)
        Me.gbToolBar.Controls.Add(Me.btnUndoRefine)
        Me.gbToolBar.Controls.Add(Me.btnRefineRecessionDuration)
        Me.gbToolBar.Controls.Add(Me.btnGraph)
        Me.gbToolBar.Controls.Add(Me.btnTable)
        Me.gbToolBar.Location = New System.Drawing.Point(12, 284)
        Me.gbToolBar.Name = "gbToolBar"
        Me.gbToolBar.Size = New System.Drawing.Size(590, 52)
        Me.gbToolBar.TabIndex = 36
        Me.gbToolBar.TabStop = False
        Me.gbToolBar.Text = "Recession Analysis Tool Bar"
        '
        'btnSummary
        '
        Me.btnSummary.Location = New System.Drawing.Point(507, 19)
        Me.btnSummary.Name = "btnSummary"
        Me.btnSummary.Size = New System.Drawing.Size(75, 23)
        Me.btnSummary.TabIndex = 17
        Me.btnSummary.Text = "Summary"
        Me.btnSummary.UseVisualStyleBackColor = True
        '
        'btnAnalyse
        '
        Me.btnAnalyse.Location = New System.Drawing.Point(426, 19)
        Me.btnAnalyse.Name = "btnAnalyse"
        Me.btnAnalyse.Size = New System.Drawing.Size(75, 23)
        Me.btnAnalyse.TabIndex = 16
        Me.btnAnalyse.Text = "Analyse"
        Me.btnAnalyse.UseVisualStyleBackColor = True
        '
        'btnNextRecession
        '
        Me.btnNextRecession.Location = New System.Drawing.Point(345, 19)
        Me.btnNextRecession.Name = "btnNextRecession"
        Me.btnNextRecession.Size = New System.Drawing.Size(75, 23)
        Me.btnNextRecession.TabIndex = 15
        Me.btnNextRecession.Text = "Next"
        Me.btnNextRecession.UseVisualStyleBackColor = True
        '
        'btnUndoRefine
        '
        Me.btnUndoRefine.Location = New System.Drawing.Point(264, 19)
        Me.btnUndoRefine.Name = "btnUndoRefine"
        Me.btnUndoRefine.Size = New System.Drawing.Size(75, 23)
        Me.btnUndoRefine.TabIndex = 14
        Me.btnUndoRefine.Text = "Undo Refine"
        Me.btnUndoRefine.UseVisualStyleBackColor = True
        '
        'btnRefineRecessionDuration
        '
        Me.btnRefineRecessionDuration.Location = New System.Drawing.Point(183, 19)
        Me.btnRefineRecessionDuration.Name = "btnRefineRecessionDuration"
        Me.btnRefineRecessionDuration.Size = New System.Drawing.Size(75, 23)
        Me.btnRefineRecessionDuration.TabIndex = 13
        Me.btnRefineRecessionDuration.Text = "Refine"
        Me.btnRefineRecessionDuration.UseVisualStyleBackColor = True
        '
        'btnGraph
        '
        Me.btnGraph.Location = New System.Drawing.Point(64, 20)
        Me.btnGraph.Name = "btnGraph"
        Me.btnGraph.Size = New System.Drawing.Size(47, 23)
        Me.btnGraph.TabIndex = 12
        Me.btnGraph.Text = "Graph"
        Me.btnGraph.UseVisualStyleBackColor = True
        '
        'btnTable
        '
        Me.btnTable.Location = New System.Drawing.Point(7, 20)
        Me.btnTable.Name = "btnTable"
        Me.btnTable.Size = New System.Drawing.Size(50, 23)
        Me.btnTable.TabIndex = 11
        Me.btnTable.Text = "Table"
        Me.btnTable.UseVisualStyleBackColor = True
        '
        'lblOutpuDir
        '
        Me.lblOutpuDir.AutoSize = True
        Me.lblOutpuDir.Location = New System.Drawing.Point(9, 242)
        Me.lblOutpuDir.Name = "lblOutpuDir"
        Me.lblOutpuDir.Size = New System.Drawing.Size(118, 13)
        Me.lblOutpuDir.TabIndex = 37
        Me.lblOutpuDir.Text = "Specify output directory"
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(12, 258)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(581, 20)
        Me.TextBox1.TabIndex = 10
        '
        'scDisplay
        '
        Me.scDisplay.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.scDisplay.Location = New System.Drawing.Point(12, 342)
        Me.scDisplay.Name = "scDisplay"
        '
        'scDisplay.Panel1
        '
        Me.scDisplay.Panel1.Controls.Add(Me.tabRefine)
        Me.scDisplay.Panel1.Controls.Add(Me.txtDisplayText)
        Me.scDisplay.Panel1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        '
        'scDisplay.Panel2
        '
        Me.scDisplay.Panel2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.scDisplay.Size = New System.Drawing.Size(589, 301)
        Me.scDisplay.SplitterDistance = 294
        Me.scDisplay.TabIndex = 38
        '
        'tabRefine
        '
        Me.tabRefine.Controls.Add(Me.tabRefineDays)
        Me.tabRefine.Location = New System.Drawing.Point(183, 3)
        Me.tabRefine.Name = "tabRefine"
        Me.tabRefine.SelectedIndex = 0
        Me.tabRefine.Size = New System.Drawing.Size(200, 100)
        Me.tabRefine.TabIndex = 1
        Me.tabRefine.Visible = False
        '
        'tabRefineDays
        '
        Me.tabRefineDays.Controls.Add(Me.btnSetDays)
        Me.tabRefineDays.Controls.Add(Me.txtAskUserLastDayofSegment)
        Me.tabRefineDays.Controls.Add(Me.txtAskUserFirstDayofSegment)
        Me.tabRefineDays.Controls.Add(Me.lblLastDay)
        Me.tabRefineDays.Controls.Add(Me.lblFirstDay)
        Me.tabRefineDays.Location = New System.Drawing.Point(4, 22)
        Me.tabRefineDays.Name = "tabRefineDays"
        Me.tabRefineDays.Padding = New System.Windows.Forms.Padding(3)
        Me.tabRefineDays.Size = New System.Drawing.Size(192, 74)
        Me.tabRefineDays.TabIndex = 0
        Me.tabRefineDays.Text = "Refine recession segment duration"
        Me.tabRefineDays.UseVisualStyleBackColor = True
        '
        'btnSetDays
        '
        Me.btnSetDays.Location = New System.Drawing.Point(125, 32)
        Me.btnSetDays.Name = "btnSetDays"
        Me.btnSetDays.Size = New System.Drawing.Size(61, 23)
        Me.btnSetDays.TabIndex = 4
        Me.btnSetDays.Text = "Set"
        Me.btnSetDays.UseVisualStyleBackColor = True
        '
        'txtAskUserLastDayofSegment
        '
        Me.txtAskUserLastDayofSegment.Location = New System.Drawing.Point(62, 34)
        Me.txtAskUserLastDayofSegment.Name = "txtAskUserLastDayofSegment"
        Me.txtAskUserLastDayofSegment.Size = New System.Drawing.Size(54, 20)
        Me.txtAskUserLastDayofSegment.TabIndex = 3
        '
        'txtAskUserFirstDayofSegment
        '
        Me.txtAskUserFirstDayofSegment.Location = New System.Drawing.Point(62, 7)
        Me.txtAskUserFirstDayofSegment.Name = "txtAskUserFirstDayofSegment"
        Me.txtAskUserFirstDayofSegment.Size = New System.Drawing.Size(54, 20)
        Me.txtAskUserFirstDayofSegment.TabIndex = 2
        '
        'lblLastDay
        '
        Me.lblLastDay.AutoSize = True
        Me.lblLastDay.Location = New System.Drawing.Point(6, 37)
        Me.lblLastDay.Name = "lblLastDay"
        Me.lblLastDay.Size = New System.Drawing.Size(49, 13)
        Me.lblLastDay.TabIndex = 1
        Me.lblLastDay.Text = "Last Day"
        '
        'lblFirstDay
        '
        Me.lblFirstDay.AutoSize = True
        Me.lblFirstDay.Location = New System.Drawing.Point(8, 10)
        Me.lblFirstDay.Name = "lblFirstDay"
        Me.lblFirstDay.Size = New System.Drawing.Size(48, 13)
        Me.lblFirstDay.TabIndex = 0
        Me.lblFirstDay.Text = "First Day"
        '
        'txtDisplayText
        '
        Me.txtDisplayText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtDisplayText.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtDisplayText.Font = New System.Drawing.Font("Consolas", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDisplayText.Location = New System.Drawing.Point(0, 0)
        Me.txtDisplayText.Multiline = True
        Me.txtDisplayText.Name = "txtDisplayText"
        Me.txtDisplayText.ReadOnly = True
        Me.txtDisplayText.Size = New System.Drawing.Size(294, 301)
        Me.txtDisplayText.TabIndex = 0
        '
        'gbRnages
        '
        Me.gbRnages.Controls.Add(Me.Button1)
        Me.gbRnages.Controls.Add(Me.TextBox4)
        Me.gbRnages.Controls.Add(Me.TextBox3)
        Me.gbRnages.Controls.Add(Me.Label3)
        Me.gbRnages.Controls.Add(Me.Label2)
        Me.gbRnages.Controls.Add(Me.TextBox2)
        Me.gbRnages.Controls.Add(Me.Label1)
        Me.gbRnages.Location = New System.Drawing.Point(12, 181)
        Me.gbRnages.Name = "gbRnages"
        Me.gbRnages.Size = New System.Drawing.Size(582, 58)
        Me.gbRnages.TabIndex = 39
        Me.gbRnages.TabStop = False
        Me.gbRnages.Text = "Recession Data Display Parameters"
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(453, 29)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(117, 23)
        Me.Button1.TabIndex = 6
        Me.Button1.Text = "Browse for range file"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'TextBox4
        '
        Me.TextBox4.Location = New System.Drawing.Point(300, 32)
        Me.TextBox4.Name = "TextBox4"
        Me.TextBox4.Size = New System.Drawing.Size(100, 20)
        Me.TextBox4.TabIndex = 5
        '
        'TextBox3
        '
        Me.TextBox3.Location = New System.Drawing.Point(156, 33)
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.Size = New System.Drawing.Size(100, 20)
        Me.TextBox3.TabIndex = 4
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(297, 16)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(100, 13)
        Me.Label3.TabIndex = 3
        Me.Label3.Text = "Maximum Log(Flow)"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(153, 16)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(97, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Minimum Log(Flow)"
        '
        'TextBox2
        '
        Me.TextBox2.Location = New System.Drawing.Point(9, 32)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(100, 20)
        Me.TextBox2.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(49, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Last Day"
        '
        'frmUSGSStreamFlowAnalysis
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(614, 656)
        Me.Controls.Add(Me.gbRnages)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.lblOutpuDir)
        Me.Controls.Add(Me.gbToolBar)
        Me.Controls.Add(Me.txtMinRecessionDays)
        Me.Controls.Add(Me.lblRecessionDays)
        Me.Controls.Add(Me.gbMonthSeason)
        Me.Controls.Add(Me.gbDates)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Controls.Add(Me.scDisplay)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmUSGSStreamFlowAnalysis"
        Me.Text = "USGS Streamflow Analysis"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.gbDates.ResumeLayout(False)
        Me.gbDates.PerformLayout()
        Me.gbMonthSeason.ResumeLayout(False)
        Me.gbMonthSeason.PerformLayout()
        Me.gbToolBar.ResumeLayout(False)
        Me.scDisplay.Panel1.ResumeLayout(False)
        Me.scDisplay.Panel1.PerformLayout()
        Me.scDisplay.ResumeLayout(False)
        Me.tabRefine.ResumeLayout(False)
        Me.tabRefineDays.ResumeLayout(False)
        Me.tabRefineDays.PerformLayout()
        Me.gbRnages.ResumeLayout(False)
        Me.gbRnages.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents mnuFile As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuFileSelectData As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuOutput As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuOutputASCII As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuGraphBF As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuGraphTimeseries As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuGraphProbability As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAnalysis As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuHelp As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gbDates As System.Windows.Forms.GroupBox
    Friend WithEvents txtEndDateUser As System.Windows.Forms.TextBox
    Friend WithEvents txtStartDateUser As System.Windows.Forms.TextBox
    Friend WithEvents btnExamineData As System.Windows.Forms.Button
    Friend WithEvents txtDataEnd As System.Windows.Forms.TextBox
    Friend WithEvents txtDataStart As System.Windows.Forms.TextBox
    Friend WithEvents lblDataEnd As System.Windows.Forms.Label
    Friend WithEvents lblDataStart As System.Windows.Forms.Label
    Friend WithEvents gbMonthSeason As System.Windows.Forms.GroupBox
    Friend WithEvents lstMonths As System.Windows.Forms.ListBox
    Friend WithEvents rdoWinter As System.Windows.Forms.RadioButton
    Friend WithEvents rdoFall As System.Windows.Forms.RadioButton
    Friend WithEvents rdoSummer As System.Windows.Forms.RadioButton
    Friend WithEvents rdoSpring As System.Windows.Forms.RadioButton
    Friend WithEvents rdoNoSeason As System.Windows.Forms.RadioButton
    Friend WithEvents lblRecessionDays As System.Windows.Forms.Label
    Friend WithEvents txtMinRecessionDays As System.Windows.Forms.TextBox
    Friend WithEvents gbToolBar As System.Windows.Forms.GroupBox
    Friend WithEvents btnGraph As System.Windows.Forms.Button
    Friend WithEvents btnTable As System.Windows.Forms.Button
    Friend WithEvents btnRefineRecessionDuration As System.Windows.Forms.Button
    Friend WithEvents btnNextRecession As System.Windows.Forms.Button
    Friend WithEvents btnUndoRefine As System.Windows.Forms.Button
    Friend WithEvents btnAnalyse As System.Windows.Forms.Button
    Friend WithEvents btnSummary As System.Windows.Forms.Button
    Friend WithEvents lblOutpuDir As System.Windows.Forms.Label
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents scDisplay As System.Windows.Forms.SplitContainer
    Friend WithEvents txtDisplayText As System.Windows.Forms.TextBox
    Friend WithEvents gbRnages As System.Windows.Forms.GroupBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents TextBox4 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox3 As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents tabRefine As System.Windows.Forms.TabControl
    Friend WithEvents tabRefineDays As System.Windows.Forms.TabPage
    Friend WithEvents btnSetDays As System.Windows.Forms.Button
    Friend WithEvents txtAskUserLastDayofSegment As System.Windows.Forms.TextBox
    Friend WithEvents txtAskUserFirstDayofSegment As System.Windows.Forms.TextBox
    Friend WithEvents lblLastDay As System.Windows.Forms.Label
    Friend WithEvents lblFirstDay As System.Windows.Forms.Label
End Class
