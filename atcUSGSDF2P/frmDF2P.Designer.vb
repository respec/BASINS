<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDF2P
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDF2P))
        Me.chkSaveInterimToFile = New System.Windows.Forms.CheckBox()
        Me.txtOutputDir = New System.Windows.Forms.TextBox()
        Me.lblOutpuDir = New System.Windows.Forms.Label()
        Me.txtMinRecessionDays = New System.Windows.Forms.TextBox()
        Me.lblRecessionDays = New System.Windows.Forms.Label()
        Me.gbMonthSeason = New System.Windows.Forms.GroupBox()
        Me.btnClear = New System.Windows.Forms.Button()
        Me.btnAllMonths = New System.Windows.Forms.Button()
        Me.rdoNoSeason = New System.Windows.Forms.RadioButton()
        Me.rdoWinter = New System.Windows.Forms.RadioButton()
        Me.rdoFall = New System.Windows.Forms.RadioButton()
        Me.rdoSummer = New System.Windows.Forms.RadioButton()
        Me.rdoSpring = New System.Windows.Forms.RadioButton()
        Me.lstMonths = New System.Windows.Forms.ListBox()
        Me.gbDates = New System.Windows.Forms.GroupBox()
        Me.lblAnalysisDates = New System.Windows.Forms.Label()
        Me.lblPeriodOfRecord = New System.Windows.Forms.Label()
        Me.txtEndDateUser = New System.Windows.Forms.TextBox()
        Me.txtStartDateUser = New System.Windows.Forms.TextBox()
        Me.btnExamineData = New System.Windows.Forms.Button()
        Me.txtDataEnd = New System.Windows.Forms.TextBox()
        Me.txtDataStart = New System.Windows.Forms.TextBox()
        Me.lblDataEnd = New System.Windows.Forms.Label()
        Me.lblDataStart = New System.Windows.Forms.Label()
        Me.lstRecessSegments = New System.Windows.Forms.CheckedListBox()
        Me.scDisplay = New System.Windows.Forms.SplitContainer()
        Me.txtAnalysisResults = New System.Windows.Forms.TextBox()
        Me.lstTable = New System.Windows.Forms.ListBox()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnAnalyse = New System.Windows.Forms.Button()
        Me.btnGetAllSegments = New System.Windows.Forms.Button()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.mnuFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuFileSelectData = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuHelp = New System.Windows.Forms.ToolStripMenuItem()
        Me.panelConfiguration = New System.Windows.Forms.Panel()
        Me.lblMinRecessDays = New System.Windows.Forms.Label()
        Me.txtBackDays = New System.Windows.Forms.TextBox()
        Me.lblRC = New System.Windows.Forms.Label()
        Me.txtRC = New System.Windows.Forms.TextBox()
        Me.btnEstRC = New System.Windows.Forms.Button()
        Me.gbDataType = New System.Windows.Forms.GroupBox()
        Me.txtDataInfo = New System.Windows.Forms.TextBox()
        Me.panelAnalysis = New System.Windows.Forms.Panel()
        Me.txtBFImax = New System.Windows.Forms.TextBox()
        Me.lblEstimatedBFImax = New System.Windows.Forms.Label()
        Me.txtRC2 = New System.Windows.Forms.TextBox()
        Me.lblRC2 = New System.Windows.Forms.Label()
        Me.btnConfiguration = New System.Windows.Forms.Button()
        Me.gbMonthSeason.SuspendLayout()
        Me.gbDates.SuspendLayout()
        Me.scDisplay.Panel1.SuspendLayout()
        Me.scDisplay.SuspendLayout()
        Me.MenuStrip1.SuspendLayout()
        Me.panelConfiguration.SuspendLayout()
        Me.gbDataType.SuspendLayout()
        Me.panelAnalysis.SuspendLayout()
        Me.SuspendLayout()
        '
        'chkSaveInterimToFile
        '
        Me.chkSaveInterimToFile.AutoSize = True
        Me.chkSaveInterimToFile.Location = New System.Drawing.Point(6, 210)
        Me.chkSaveInterimToFile.Name = "chkSaveInterimToFile"
        Me.chkSaveInterimToFile.Size = New System.Drawing.Size(150, 17)
        Me.chkSaveInterimToFile.TabIndex = 11
        Me.chkSaveInterimToFile.Text = "Save Intermediate Results"
        Me.chkSaveInterimToFile.UseVisualStyleBackColor = True
        Me.chkSaveInterimToFile.Visible = False
        '
        'txtOutputDir
        '
        Me.txtOutputDir.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtOutputDir.Location = New System.Drawing.Point(6, 189)
        Me.txtOutputDir.Name = "txtOutputDir"
        Me.txtOutputDir.Size = New System.Drawing.Size(764, 20)
        Me.txtOutputDir.TabIndex = 10
        '
        'lblOutpuDir
        '
        Me.lblOutpuDir.AutoSize = True
        Me.lblOutpuDir.Location = New System.Drawing.Point(6, 170)
        Me.lblOutpuDir.Name = "lblOutpuDir"
        Me.lblOutpuDir.Size = New System.Drawing.Size(118, 13)
        Me.lblOutpuDir.TabIndex = 45
        Me.lblOutpuDir.Text = "Specify output directory"
        '
        'txtMinRecessionDays
        '
        Me.txtMinRecessionDays.Location = New System.Drawing.Point(581, 227)
        Me.txtMinRecessionDays.Name = "txtMinRecessionDays"
        Me.txtMinRecessionDays.Size = New System.Drawing.Size(54, 20)
        Me.txtMinRecessionDays.TabIndex = 9
        Me.txtMinRecessionDays.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblRecessionDays
        '
        Me.lblRecessionDays.AutoSize = True
        Me.lblRecessionDays.Location = New System.Drawing.Point(440, 256)
        Me.lblRecessionDays.Name = "lblRecessionDays"
        Me.lblRecessionDays.Size = New System.Drawing.Size(135, 13)
        Me.lblRecessionDays.TabIndex = 44
        Me.lblRecessionDays.Text = "Estimation duration in days:"
        '
        'gbMonthSeason
        '
        Me.gbMonthSeason.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbMonthSeason.Controls.Add(Me.btnClear)
        Me.gbMonthSeason.Controls.Add(Me.btnAllMonths)
        Me.gbMonthSeason.Controls.Add(Me.rdoNoSeason)
        Me.gbMonthSeason.Controls.Add(Me.rdoWinter)
        Me.gbMonthSeason.Controls.Add(Me.rdoFall)
        Me.gbMonthSeason.Controls.Add(Me.rdoSummer)
        Me.gbMonthSeason.Controls.Add(Me.rdoSpring)
        Me.gbMonthSeason.Controls.Add(Me.lstMonths)
        Me.gbMonthSeason.Location = New System.Drawing.Point(323, 8)
        Me.gbMonthSeason.Name = "gbMonthSeason"
        Me.gbMonthSeason.Size = New System.Drawing.Size(275, 175)
        Me.gbMonthSeason.TabIndex = 43
        Me.gbMonthSeason.TabStop = False
        Me.gbMonthSeason.Text = "Months and Season"
        '
        'btnClear
        '
        Me.btnClear.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClear.Location = New System.Drawing.Point(218, 146)
        Me.btnClear.Name = "btnClear"
        Me.btnClear.Size = New System.Drawing.Size(45, 23)
        Me.btnClear.TabIndex = 10
        Me.btnClear.Text = "Clear"
        Me.btnClear.UseVisualStyleBackColor = True
        '
        'btnAllMonths
        '
        Me.btnAllMonths.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnAllMonths.Location = New System.Drawing.Point(167, 146)
        Me.btnAllMonths.Name = "btnAllMonths"
        Me.btnAllMonths.Size = New System.Drawing.Size(45, 23)
        Me.btnAllMonths.TabIndex = 9
        Me.btnAllMonths.Text = "All"
        Me.btnAllMonths.UseVisualStyleBackColor = True
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
        Me.gbDates.Location = New System.Drawing.Point(6, 8)
        Me.gbDates.Name = "gbDates"
        Me.gbDates.Size = New System.Drawing.Size(311, 120)
        Me.gbDates.TabIndex = 42
        Me.gbDates.TabStop = False
        Me.gbDates.Text = "Streamflow Analysis Dates"
        '
        'lblAnalysisDates
        '
        Me.lblAnalysisDates.AutoSize = True
        Me.lblAnalysisDates.Location = New System.Drawing.Point(184, 22)
        Me.lblAnalysisDates.Name = "lblAnalysisDates"
        Me.lblAnalysisDates.Size = New System.Drawing.Size(76, 13)
        Me.lblAnalysisDates.TabIndex = 16
        Me.lblAnalysisDates.Text = "Analysis Dates"
        '
        'lblPeriodOfRecord
        '
        Me.lblPeriodOfRecord.AutoSize = True
        Me.lblPeriodOfRecord.Location = New System.Drawing.Point(68, 22)
        Me.lblPeriodOfRecord.Name = "lblPeriodOfRecord"
        Me.lblPeriodOfRecord.Size = New System.Drawing.Size(87, 13)
        Me.lblPeriodOfRecord.TabIndex = 15
        Me.lblPeriodOfRecord.Text = "Period of Record"
        '
        'txtEndDateUser
        '
        Me.txtEndDateUser.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtEndDateUser.Location = New System.Drawing.Point(187, 64)
        Me.txtEndDateUser.Name = "txtEndDateUser"
        Me.txtEndDateUser.Size = New System.Drawing.Size(118, 20)
        Me.txtEndDateUser.TabIndex = 1
        '
        'txtStartDateUser
        '
        Me.txtStartDateUser.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtStartDateUser.Location = New System.Drawing.Point(187, 38)
        Me.txtStartDateUser.Name = "txtStartDateUser"
        Me.txtStartDateUser.Size = New System.Drawing.Size(118, 20)
        Me.txtStartDateUser.TabIndex = 0
        '
        'btnExamineData
        '
        Me.btnExamineData.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnExamineData.Location = New System.Drawing.Point(71, 91)
        Me.btnExamineData.Name = "btnExamineData"
        Me.btnExamineData.Size = New System.Drawing.Size(88, 23)
        Me.btnExamineData.TabIndex = 2
        Me.btnExamineData.Text = "Examine Data"
        Me.btnExamineData.UseVisualStyleBackColor = True
        '
        'txtDataEnd
        '
        Me.txtDataEnd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtDataEnd.Location = New System.Drawing.Point(71, 65)
        Me.txtDataEnd.Name = "txtDataEnd"
        Me.txtDataEnd.ReadOnly = True
        Me.txtDataEnd.Size = New System.Drawing.Size(110, 20)
        Me.txtDataEnd.TabIndex = 3
        '
        'txtDataStart
        '
        Me.txtDataStart.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtDataStart.Location = New System.Drawing.Point(71, 38)
        Me.txtDataStart.Name = "txtDataStart"
        Me.txtDataStart.ReadOnly = True
        Me.txtDataStart.Size = New System.Drawing.Size(110, 20)
        Me.txtDataStart.TabIndex = 2
        '
        'lblDataEnd
        '
        Me.lblDataEnd.AutoSize = True
        Me.lblDataEnd.Location = New System.Drawing.Point(9, 64)
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
        Me.lblDataStart.Location = New System.Drawing.Point(9, 38)
        Me.lblDataStart.Name = "lblDataStart"
        Me.lblDataStart.Size = New System.Drawing.Size(55, 13)
        Me.lblDataStart.TabIndex = 0
        Me.lblDataStart.Text = "Data Start"
        '
        'lstRecessSegments
        '
        Me.lstRecessSegments.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lstRecessSegments.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lstRecessSegments.FormattingEnabled = True
        Me.lstRecessSegments.IntegralHeight = False
        Me.lstRecessSegments.Location = New System.Drawing.Point(3, 3)
        Me.lstRecessSegments.Name = "lstRecessSegments"
        Me.lstRecessSegments.Size = New System.Drawing.Size(122, 228)
        Me.lstRecessSegments.TabIndex = 13
        '
        'scDisplay
        '
        Me.scDisplay.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.scDisplay.Location = New System.Drawing.Point(132, 3)
        Me.scDisplay.Name = "scDisplay"
        '
        'scDisplay.Panel1
        '
        Me.scDisplay.Panel1.Controls.Add(Me.txtAnalysisResults)
        Me.scDisplay.Panel1.Controls.Add(Me.lstTable)
        Me.scDisplay.Panel1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        '
        'scDisplay.Panel2
        '
        Me.scDisplay.Panel2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.scDisplay.Size = New System.Drawing.Size(636, 228)
        Me.scDisplay.SplitterDistance = 26
        Me.scDisplay.TabIndex = 39
        '
        'txtAnalysisResults
        '
        Me.txtAnalysisResults.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtAnalysisResults.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAnalysisResults.Location = New System.Drawing.Point(3, 19)
        Me.txtAnalysisResults.Multiline = True
        Me.txtAnalysisResults.Name = "txtAnalysisResults"
        Me.txtAnalysisResults.ReadOnly = True
        Me.txtAnalysisResults.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtAnalysisResults.Size = New System.Drawing.Size(272, 206)
        Me.txtAnalysisResults.TabIndex = 15
        Me.txtAnalysisResults.Visible = False
        Me.txtAnalysisResults.WordWrap = False
        '
        'lstTable
        '
        Me.lstTable.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lstTable.FormattingEnabled = True
        Me.lstTable.IntegralHeight = False
        Me.lstTable.ItemHeight = 14
        Me.lstTable.Location = New System.Drawing.Point(3, 3)
        Me.lstTable.Name = "lstTable"
        Me.lstTable.Size = New System.Drawing.Size(246, 173)
        Me.lstTable.TabIndex = 0
        Me.lstTable.Visible = False
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.Location = New System.Drawing.Point(675, 233)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(93, 27)
        Me.btnSave.TabIndex = 18
        Me.btnSave.Text = "Save"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnAnalyse
        '
        Me.btnAnalyse.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnAnalyse.Location = New System.Drawing.Point(135, 235)
        Me.btnAnalyse.Name = "btnAnalyse"
        Me.btnAnalyse.Size = New System.Drawing.Size(75, 23)
        Me.btnAnalyse.TabIndex = 17
        Me.btnAnalyse.Text = "Replot"
        Me.btnAnalyse.UseVisualStyleBackColor = True
        Me.btnAnalyse.Visible = False
        '
        'btnGetAllSegments
        '
        Me.btnGetAllSegments.AutoSize = True
        Me.btnGetAllSegments.Location = New System.Drawing.Point(641, 246)
        Me.btnGetAllSegments.Name = "btnGetAllSegments"
        Me.btnGetAllSegments.Size = New System.Drawing.Size(132, 27)
        Me.btnGetAllSegments.TabIndex = 12
        Me.btnGetAllSegments.Text = "Step 2: Estimate BFImax"
        Me.btnGetAllSegments.UseVisualStyleBackColor = True
        '
        'MenuStrip1
        '
        Me.MenuStrip1.BackColor = System.Drawing.SystemColors.Window
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuFile, Me.mnuHelp})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(789, 24)
        Me.MenuStrip1.TabIndex = 30
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'mnuFile
        '
        Me.mnuFile.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuFileSelectData})
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
        'mnuHelp
        '
        Me.mnuHelp.Name = "mnuHelp"
        Me.mnuHelp.Size = New System.Drawing.Size(44, 20)
        Me.mnuHelp.Text = "Help"
        '
        'panelConfiguration
        '
        Me.panelConfiguration.Controls.Add(Me.lblMinRecessDays)
        Me.panelConfiguration.Controls.Add(Me.lblRecessionDays)
        Me.panelConfiguration.Controls.Add(Me.txtBackDays)
        Me.panelConfiguration.Controls.Add(Me.lblRC)
        Me.panelConfiguration.Controls.Add(Me.txtMinRecessionDays)
        Me.panelConfiguration.Controls.Add(Me.txtRC)
        Me.panelConfiguration.Controls.Add(Me.btnEstRC)
        Me.panelConfiguration.Controls.Add(Me.gbDataType)
        Me.panelConfiguration.Controls.Add(Me.chkSaveInterimToFile)
        Me.panelConfiguration.Controls.Add(Me.gbDates)
        Me.panelConfiguration.Controls.Add(Me.btnGetAllSegments)
        Me.panelConfiguration.Controls.Add(Me.txtOutputDir)
        Me.panelConfiguration.Controls.Add(Me.gbMonthSeason)
        Me.panelConfiguration.Controls.Add(Me.lblOutpuDir)
        Me.panelConfiguration.Location = New System.Drawing.Point(0, 27)
        Me.panelConfiguration.Name = "panelConfiguration"
        Me.panelConfiguration.Size = New System.Drawing.Size(777, 278)
        Me.panelConfiguration.TabIndex = 31
        '
        'lblMinRecessDays
        '
        Me.lblMinRecessDays.AutoSize = True
        Me.lblMinRecessDays.Location = New System.Drawing.Point(386, 230)
        Me.lblMinRecessDays.Name = "lblMinRecessDays"
        Me.lblMinRecessDays.Size = New System.Drawing.Size(189, 13)
        Me.lblMinRecessDays.TabIndex = 53
        Me.lblMinRecessDays.Text = "Minimum flow recession length in days:"
        '
        'txtBackDays
        '
        Me.txtBackDays.Location = New System.Drawing.Point(581, 253)
        Me.txtBackDays.Name = "txtBackDays"
        Me.txtBackDays.Size = New System.Drawing.Size(54, 20)
        Me.txtBackDays.TabIndex = 45
        Me.txtBackDays.Text = "365"
        Me.txtBackDays.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblRC
        '
        Me.lblRC.AutoSize = True
        Me.lblRC.Location = New System.Drawing.Point(203, 234)
        Me.lblRC.Name = "lblRC"
        Me.lblRC.Size = New System.Drawing.Size(117, 13)
        Me.lblRC.TabIndex = 51
        Me.lblRC.Text = "Recession Constant (a)"
        '
        'txtRC
        '
        Me.txtRC.Location = New System.Drawing.Point(206, 250)
        Me.txtRC.Name = "txtRC"
        Me.txtRC.Size = New System.Drawing.Size(114, 20)
        Me.txtRC.TabIndex = 50
        Me.txtRC.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'btnEstRC
        '
        Me.btnEstRC.Location = New System.Drawing.Point(6, 246)
        Me.btnEstRC.Name = "btnEstRC"
        Me.btnEstRC.Size = New System.Drawing.Size(194, 27)
        Me.btnEstRC.TabIndex = 48
        Me.btnEstRC.Text = "Step 1: Estimate Recession Constant"
        Me.btnEstRC.UseVisualStyleBackColor = True
        '
        'gbDataType
        '
        Me.gbDataType.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbDataType.Controls.Add(Me.txtDataInfo)
        Me.gbDataType.Location = New System.Drawing.Point(604, 8)
        Me.gbDataType.Name = "gbDataType"
        Me.gbDataType.Size = New System.Drawing.Size(172, 175)
        Me.gbDataType.TabIndex = 46
        Me.gbDataType.TabStop = False
        Me.gbDataType.Text = "Data Info."
        '
        'txtDataInfo
        '
        Me.txtDataInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtDataInfo.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtDataInfo.Location = New System.Drawing.Point(3, 16)
        Me.txtDataInfo.Multiline = True
        Me.txtDataInfo.Name = "txtDataInfo"
        Me.txtDataInfo.ReadOnly = True
        Me.txtDataInfo.Size = New System.Drawing.Size(166, 156)
        Me.txtDataInfo.TabIndex = 0
        '
        'panelAnalysis
        '
        Me.panelAnalysis.Controls.Add(Me.txtBFImax)
        Me.panelAnalysis.Controls.Add(Me.lblEstimatedBFImax)
        Me.panelAnalysis.Controls.Add(Me.txtRC2)
        Me.panelAnalysis.Controls.Add(Me.lblRC2)
        Me.panelAnalysis.Controls.Add(Me.btnConfiguration)
        Me.panelAnalysis.Controls.Add(Me.lstRecessSegments)
        Me.panelAnalysis.Controls.Add(Me.btnSave)
        Me.panelAnalysis.Controls.Add(Me.btnAnalyse)
        Me.panelAnalysis.Controls.Add(Me.scDisplay)
        Me.panelAnalysis.Location = New System.Drawing.Point(6, 322)
        Me.panelAnalysis.Name = "panelAnalysis"
        Me.panelAnalysis.Size = New System.Drawing.Size(771, 263)
        Me.panelAnalysis.TabIndex = 32
        Me.panelAnalysis.Visible = False
        '
        'txtBFImax
        '
        Me.txtBFImax.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtBFImax.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtBFImax.Location = New System.Drawing.Point(612, 238)
        Me.txtBFImax.Name = "txtBFImax"
        Me.txtBFImax.ReadOnly = True
        Me.txtBFImax.Size = New System.Drawing.Size(57, 20)
        Me.txtBFImax.TabIndex = 45
        '
        'lblEstimatedBFImax
        '
        Me.lblEstimatedBFImax.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblEstimatedBFImax.AutoSize = True
        Me.lblEstimatedBFImax.Location = New System.Drawing.Point(321, 241)
        Me.lblEstimatedBFImax.Name = "lblEstimatedBFImax"
        Me.lblEstimatedBFImax.Size = New System.Drawing.Size(72, 13)
        Me.lblEstimatedBFImax.TabIndex = 44
        Me.lblEstimatedBFImax.Text = "Estimated BFI"
        '
        'txtRC2
        '
        Me.txtRC2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtRC2.Location = New System.Drawing.Point(263, 237)
        Me.txtRC2.Name = "txtRC2"
        Me.txtRC2.Size = New System.Drawing.Size(45, 20)
        Me.txtRC2.TabIndex = 43
        Me.txtRC2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtRC2.Visible = False
        '
        'lblRC2
        '
        Me.lblRC2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblRC2.AutoSize = True
        Me.lblRC2.Location = New System.Drawing.Point(137, 240)
        Me.lblRC2.Name = "lblRC2"
        Me.lblRC2.Size = New System.Drawing.Size(120, 13)
        Me.lblRC2.TabIndex = 42
        Me.lblRC2.Text = "Recession Constant (a):"
        Me.lblRC2.Visible = False
        '
        'btnConfiguration
        '
        Me.btnConfiguration.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnConfiguration.AutoSize = True
        Me.btnConfiguration.Location = New System.Drawing.Point(3, 233)
        Me.btnConfiguration.Name = "btnConfiguration"
        Me.btnConfiguration.Size = New System.Drawing.Size(122, 27)
        Me.btnConfiguration.TabIndex = 16
        Me.btnConfiguration.Text = "< Back"
        Me.btnConfiguration.UseVisualStyleBackColor = True
        '
        'frmDF2P
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(789, 314)
        Me.Controls.Add(Me.panelConfiguration)
        Me.Controls.Add(Me.panelAnalysis)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Name = "frmDF2P"
        Me.Text = "Two-Parameter Digital Filter"
        Me.gbMonthSeason.ResumeLayout(False)
        Me.gbMonthSeason.PerformLayout()
        Me.gbDates.ResumeLayout(False)
        Me.gbDates.PerformLayout()
        Me.scDisplay.Panel1.ResumeLayout(False)
        Me.scDisplay.Panel1.PerformLayout()
        Me.scDisplay.ResumeLayout(False)
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.panelConfiguration.ResumeLayout(False)
        Me.panelConfiguration.PerformLayout()
        Me.gbDataType.ResumeLayout(False)
        Me.gbDataType.PerformLayout()
        Me.panelAnalysis.ResumeLayout(False)
        Me.panelAnalysis.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents mnuFile As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuFileSelectData As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuHelp As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents txtOutputDir As System.Windows.Forms.TextBox
    Friend WithEvents lblOutpuDir As System.Windows.Forms.Label
    Friend WithEvents txtMinRecessionDays As System.Windows.Forms.TextBox
    Friend WithEvents lblRecessionDays As System.Windows.Forms.Label
    Friend WithEvents gbMonthSeason As System.Windows.Forms.GroupBox
    Friend WithEvents rdoNoSeason As System.Windows.Forms.RadioButton
    Friend WithEvents rdoWinter As System.Windows.Forms.RadioButton
    Friend WithEvents rdoFall As System.Windows.Forms.RadioButton
    Friend WithEvents rdoSummer As System.Windows.Forms.RadioButton
    Friend WithEvents rdoSpring As System.Windows.Forms.RadioButton
    Friend WithEvents lstMonths As System.Windows.Forms.ListBox
    Friend WithEvents gbDates As System.Windows.Forms.GroupBox
    Friend WithEvents txtEndDateUser As System.Windows.Forms.TextBox
    Friend WithEvents txtStartDateUser As System.Windows.Forms.TextBox
    Friend WithEvents btnExamineData As System.Windows.Forms.Button
    Friend WithEvents txtDataEnd As System.Windows.Forms.TextBox
    Friend WithEvents txtDataStart As System.Windows.Forms.TextBox
    Friend WithEvents lblDataEnd As System.Windows.Forms.Label
    Friend WithEvents lblDataStart As System.Windows.Forms.Label
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents btnAnalyse As System.Windows.Forms.Button
    Friend WithEvents btnGetAllSegments As System.Windows.Forms.Button
    Friend WithEvents scDisplay As System.Windows.Forms.SplitContainer
    Friend WithEvents lstTable As System.Windows.Forms.ListBox
    Friend WithEvents lstRecessSegments As System.Windows.Forms.CheckedListBox
    Friend WithEvents txtAnalysisResults As System.Windows.Forms.TextBox
    Friend WithEvents chkSaveInterimToFile As System.Windows.Forms.CheckBox
    Friend WithEvents panelConfiguration As System.Windows.Forms.Panel
    Friend WithEvents panelAnalysis As System.Windows.Forms.Panel
    Friend WithEvents btnConfiguration As System.Windows.Forms.Button
    Friend WithEvents lblAnalysisDates As System.Windows.Forms.Label
    Friend WithEvents lblPeriodOfRecord As System.Windows.Forms.Label
    Friend WithEvents gbDataType As System.Windows.Forms.GroupBox
    Friend WithEvents txtDataInfo As System.Windows.Forms.TextBox
    Friend WithEvents btnEstRC As Windows.Forms.Button
    Friend WithEvents lblRC As Windows.Forms.Label
    Friend WithEvents txtRC As Windows.Forms.TextBox
    Friend WithEvents txtRC2 As Windows.Forms.TextBox
    Friend WithEvents lblRC2 As Windows.Forms.Label
    Friend WithEvents txtBackDays As Windows.Forms.TextBox
    Friend WithEvents lblMinRecessDays As Windows.Forms.Label
    Friend WithEvents btnClear As Windows.Forms.Button
    Friend WithEvents btnAllMonths As Windows.Forms.Button
    Friend WithEvents gbStep2Inputs As Windows.Forms.GroupBox
    Friend WithEvents lblEstimatedBFImax As Windows.Forms.Label
    Friend WithEvents txtBFImax As Windows.Forms.TextBox
End Class
