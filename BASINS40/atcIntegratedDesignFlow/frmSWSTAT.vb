Imports atcData
Imports atcUtility
Imports DFLOWAnalysis
Imports MapWinUtility
Imports System.Windows.Forms

Public Class frmSWSTAT
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()
        InitializeComponent() 'required by Windows Form Designer
    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents MainMenu1 As System.Windows.Forms.MainMenu
    Friend WithEvents mnuAnalysis As System.Windows.Forms.MenuItem
    Friend WithEvents mnuFile As System.Windows.Forms.MenuItem
    Friend WithEvents mnuFileSelectData As System.Windows.Forms.MenuItem
    Friend WithEvents tabMain As System.Windows.Forms.TabControl
    Friend WithEvents tabSelectDates As System.Windows.Forms.TabPage
    Friend WithEvents tabNDay As System.Windows.Forms.TabPage
    Friend WithEvents grpDates As System.Windows.Forms.GroupBox
    Friend WithEvents cboStartMonth As System.Windows.Forms.ComboBox
    Friend WithEvents lblYearStart As System.Windows.Forms.Label
    Friend WithEvents txtEndDay As System.Windows.Forms.TextBox
    Friend WithEvents txtStartDay As System.Windows.Forms.TextBox
    Friend WithEvents cboEndMonth As System.Windows.Forms.ComboBox
    Friend WithEvents lblYearEnd As System.Windows.Forms.Label
    Friend WithEvents grpYears As System.Windows.Forms.GroupBox
    Friend WithEvents lblDataStart As System.Windows.Forms.Label
    Friend WithEvents lblDataEnd As System.Windows.Forms.Label
    Friend WithEvents lblOmitBefore As System.Windows.Forms.Label
    Friend WithEvents lblOmitAfter As System.Windows.Forms.Label
    Friend WithEvents txtOmitAfterYear As System.Windows.Forms.TextBox
    Friend WithEvents txtOmitBeforeYear As System.Windows.Forms.TextBox
    Friend WithEvents cboYears As System.Windows.Forms.ComboBox
    Friend WithEvents btnNDayList As System.Windows.Forms.Button
    Friend WithEvents btnScreeningTests As System.Windows.Forms.Button
    Friend WithEvents btnDisplayBasic As System.Windows.Forms.Button
    Friend WithEvents btnFrequencyGrid As System.Windows.Forms.Button
    Friend WithEvents panelNdayTrendFrequency As System.Windows.Forms.Panel
    Friend WithEvents grpRecurrence As System.Windows.Forms.GroupBox
    Friend WithEvents btnRecurrenceDefault As System.Windows.Forms.Button
    Friend WithEvents btnRecurrenceRemove As System.Windows.Forms.Button
    Friend WithEvents lstRecurrence As System.Windows.Forms.ListBox
    Friend WithEvents btnRecurrenceAdd As System.Windows.Forms.Button
    Friend WithEvents txtRecurrenceAdd As System.Windows.Forms.TextBox
    Friend WithEvents btnRecurrenceNone As System.Windows.Forms.Button
    Friend WithEvents btnRecurrenceAll As System.Windows.Forms.Button
    Friend WithEvents Splitter1 As System.Windows.Forms.Splitter
    Friend WithEvents grpNday As System.Windows.Forms.GroupBox
    Friend WithEvents btnNdayDefault As System.Windows.Forms.Button
    Friend WithEvents btnNdayRemove As System.Windows.Forms.Button
    Friend WithEvents btnNdayAdd As System.Windows.Forms.Button
    Friend WithEvents txtNdayAdd As System.Windows.Forms.TextBox
    Friend WithEvents btnNdayNone As System.Windows.Forms.Button
    Friend WithEvents btnNdayAll As System.Windows.Forms.Button
    Friend WithEvents lstNday As System.Windows.Forms.ListBox
    Friend WithEvents chkLog As System.Windows.Forms.CheckBox
    Friend WithEvents grpHighLow As System.Windows.Forms.GroupBox
    Friend WithEvents radioHigh As System.Windows.Forms.RadioButton
    Friend WithEvents radioLow As System.Windows.Forms.RadioButton
    Friend WithEvents btnFrequencyGraph As System.Windows.Forms.Button
    Friend WithEvents chkMultipleStationPlots As System.Windows.Forms.CheckBox
    Friend WithEvents chkMultipleNDayPlots As System.Windows.Forms.CheckBox
    Friend WithEvents gbTextOutput As System.Windows.Forms.GroupBox
    Friend WithEvents txtOutputDir As System.Windows.Forms.TextBox
    Friend WithEvents lblOutputDir As System.Windows.Forms.Label
    Friend WithEvents txtOutputRootName As System.Windows.Forms.TextBox
    Friend WithEvents lblBaseFilename As System.Windows.Forms.Label
    Friend WithEvents tabDFLOW As System.Windows.Forms.TabPage
    Friend WithEvents groupNonBio As System.Windows.Forms.GroupBox
    Friend WithEvents txtNonBioExplicitFlow As System.Windows.Forms.TextBox
    Friend WithEvents txtNonBioFlowPercentile As System.Windows.Forms.TextBox
    Friend WithEvents tbNonBio2 As System.Windows.Forms.TextBox
    Friend WithEvents lblNonBioExplicitFlowUnits As System.Windows.Forms.Label
    Friend WithEvents lblNonBioFlowPercentileUnits As System.Windows.Forms.Label
    Friend WithEvents txtNonBioCustomNday1 As System.Windows.Forms.TextBox
    Friend WithEvents groupBio As System.Windows.Forms.GroupBox
    Friend WithEvents txtBioExcursionDays As System.Windows.Forms.TextBox
    Friend WithEvents txtBioExcursionsPerCluster As System.Windows.Forms.TextBox
    Friend WithEvents txtBioYearsBetween As System.Windows.Forms.TextBox
    Friend WithEvents lblBioExcursionsPerCluster As System.Windows.Forms.Label
    Friend WithEvents lblBioExcursionDays As System.Windows.Forms.Label
    Friend WithEvents lblBioYearsBetween As System.Windows.Forms.Label
    Friend WithEvents txtBioFlowDays As System.Windows.Forms.TextBox
    Friend WithEvents lblBioFlowDays As System.Windows.Forms.Label
    Friend WithEvents chkBioCustom As System.Windows.Forms.CheckBox
    Friend WithEvents btnCalculate As System.Windows.Forms.Button
    Friend WithEvents chkBioChronic As System.Windows.Forms.CheckBox
    Friend WithEvents chkBioAcute As System.Windows.Forms.CheckBox
    Friend WithEvents chkBioAmmonia As System.Windows.Forms.CheckBox
    Friend WithEvents chkNonBioCustom1 As System.Windows.Forms.CheckBox
    Friend WithEvents chkNonBioChronic As System.Windows.Forms.CheckBox
    Friend WithEvents chkNonBioAcute As System.Windows.Forms.CheckBox
    Friend WithEvents txtNonBioCustomReturn1 As System.Windows.Forms.TextBox
    Friend WithEvents lblNonBioReturn As System.Windows.Forms.Label
    Friend WithEvents lblNonBioNday As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents chkNonBioFlowPercentile As System.Windows.Forms.CheckBox
    Friend WithEvents chkNonBioExplicitFlow As System.Windows.Forms.CheckBox
    Friend WithEvents chkNonBioCustom2 As System.Windows.Forms.CheckBox
    Friend WithEvents txtNonBioCustomNday2 As System.Windows.Forms.TextBox
    Friend WithEvents txtNonBioCustomReturn2 As System.Windows.Forms.TextBox
    Friend WithEvents btnNDayGraph As System.Windows.Forms.Button
    Friend WithEvents btnFrequencyReport As System.Windows.Forms.Button
    Friend WithEvents groupGraph As System.Windows.Forms.GroupBox
    Friend WithEvents chkNonBioHarmonicMean As System.Windows.Forms.CheckBox
    Friend WithEvents mnuHelp As System.Windows.Forms.MenuItem
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSWSTAT))
        Me.MainMenu1 = New System.Windows.Forms.MainMenu(Me.components)
        Me.mnuFile = New System.Windows.Forms.MenuItem()
        Me.mnuFileSelectData = New System.Windows.Forms.MenuItem()
        Me.mnuAnalysis = New System.Windows.Forms.MenuItem()
        Me.mnuHelp = New System.Windows.Forms.MenuItem()
        Me.tabMain = New System.Windows.Forms.TabControl()
        Me.tabSelectDates = New System.Windows.Forms.TabPage()
        Me.gbTextOutput = New System.Windows.Forms.GroupBox()
        Me.txtOutputDir = New System.Windows.Forms.TextBox()
        Me.lblOutputDir = New System.Windows.Forms.Label()
        Me.txtOutputRootName = New System.Windows.Forms.TextBox()
        Me.lblBaseFilename = New System.Windows.Forms.Label()
        Me.grpHighLow = New System.Windows.Forms.GroupBox()
        Me.radioHigh = New System.Windows.Forms.RadioButton()
        Me.radioLow = New System.Windows.Forms.RadioButton()
        Me.btnDisplayBasic = New System.Windows.Forms.Button()
        Me.grpDates = New System.Windows.Forms.GroupBox()
        Me.cboStartMonth = New System.Windows.Forms.ComboBox()
        Me.lblYearStart = New System.Windows.Forms.Label()
        Me.txtEndDay = New System.Windows.Forms.TextBox()
        Me.txtStartDay = New System.Windows.Forms.TextBox()
        Me.cboEndMonth = New System.Windows.Forms.ComboBox()
        Me.lblYearEnd = New System.Windows.Forms.Label()
        Me.grpYears = New System.Windows.Forms.GroupBox()
        Me.lblDataStart = New System.Windows.Forms.Label()
        Me.lblDataEnd = New System.Windows.Forms.Label()
        Me.lblOmitBefore = New System.Windows.Forms.Label()
        Me.lblOmitAfter = New System.Windows.Forms.Label()
        Me.txtOmitAfterYear = New System.Windows.Forms.TextBox()
        Me.txtOmitBeforeYear = New System.Windows.Forms.TextBox()
        Me.cboYears = New System.Windows.Forms.ComboBox()
        Me.tabNDay = New System.Windows.Forms.TabPage()
        Me.groupGraph = New System.Windows.Forms.GroupBox()
        Me.btnNDayGraph = New System.Windows.Forms.Button()
        Me.btnFrequencyGraph = New System.Windows.Forms.Button()
        Me.chkMultipleNDayPlots = New System.Windows.Forms.CheckBox()
        Me.chkMultipleStationPlots = New System.Windows.Forms.CheckBox()
        Me.btnFrequencyReport = New System.Windows.Forms.Button()
        Me.chkLog = New System.Windows.Forms.CheckBox()
        Me.panelNdayTrendFrequency = New System.Windows.Forms.Panel()
        Me.grpRecurrence = New System.Windows.Forms.GroupBox()
        Me.btnRecurrenceDefault = New System.Windows.Forms.Button()
        Me.btnRecurrenceRemove = New System.Windows.Forms.Button()
        Me.lstRecurrence = New System.Windows.Forms.ListBox()
        Me.btnRecurrenceAdd = New System.Windows.Forms.Button()
        Me.txtRecurrenceAdd = New System.Windows.Forms.TextBox()
        Me.btnRecurrenceNone = New System.Windows.Forms.Button()
        Me.btnRecurrenceAll = New System.Windows.Forms.Button()
        Me.Splitter1 = New System.Windows.Forms.Splitter()
        Me.grpNday = New System.Windows.Forms.GroupBox()
        Me.btnNdayDefault = New System.Windows.Forms.Button()
        Me.btnNdayRemove = New System.Windows.Forms.Button()
        Me.btnNdayAdd = New System.Windows.Forms.Button()
        Me.txtNdayAdd = New System.Windows.Forms.TextBox()
        Me.btnNdayNone = New System.Windows.Forms.Button()
        Me.btnNdayAll = New System.Windows.Forms.Button()
        Me.lstNday = New System.Windows.Forms.ListBox()
        Me.btnFrequencyGrid = New System.Windows.Forms.Button()
        Me.btnScreeningTests = New System.Windows.Forms.Button()
        Me.btnNDayList = New System.Windows.Forms.Button()
        Me.tabDFLOW = New System.Windows.Forms.TabPage()
        Me.btnCalculate = New System.Windows.Forms.Button()
        Me.groupNonBio = New System.Windows.Forms.GroupBox()
        Me.chkNonBioHarmonicMean = New System.Windows.Forms.CheckBox()
        Me.chkNonBioCustom2 = New System.Windows.Forms.CheckBox()
        Me.txtNonBioCustomNday2 = New System.Windows.Forms.TextBox()
        Me.txtNonBioCustomReturn2 = New System.Windows.Forms.TextBox()
        Me.chkNonBioFlowPercentile = New System.Windows.Forms.CheckBox()
        Me.chkNonBioExplicitFlow = New System.Windows.Forms.CheckBox()
        Me.chkNonBioCustom1 = New System.Windows.Forms.CheckBox()
        Me.chkNonBioChronic = New System.Windows.Forms.CheckBox()
        Me.chkNonBioAcute = New System.Windows.Forms.CheckBox()
        Me.lblNonBioReturn = New System.Windows.Forms.Label()
        Me.txtNonBioCustomNday1 = New System.Windows.Forms.TextBox()
        Me.txtNonBioCustomReturn1 = New System.Windows.Forms.TextBox()
        Me.lblNonBioNday = New System.Windows.Forms.Label()
        Me.txtNonBioExplicitFlow = New System.Windows.Forms.TextBox()
        Me.txtNonBioFlowPercentile = New System.Windows.Forms.TextBox()
        Me.lblNonBioExplicitFlowUnits = New System.Windows.Forms.Label()
        Me.lblNonBioFlowPercentileUnits = New System.Windows.Forms.Label()
        Me.groupBio = New System.Windows.Forms.GroupBox()
        Me.chkBioAmmonia = New System.Windows.Forms.CheckBox()
        Me.chkBioAcute = New System.Windows.Forms.CheckBox()
        Me.txtBioExcursionDays = New System.Windows.Forms.TextBox()
        Me.txtBioExcursionsPerCluster = New System.Windows.Forms.TextBox()
        Me.txtBioYearsBetween = New System.Windows.Forms.TextBox()
        Me.lblBioExcursionsPerCluster = New System.Windows.Forms.Label()
        Me.lblBioExcursionDays = New System.Windows.Forms.Label()
        Me.lblBioYearsBetween = New System.Windows.Forms.Label()
        Me.txtBioFlowDays = New System.Windows.Forms.TextBox()
        Me.lblBioFlowDays = New System.Windows.Forms.Label()
        Me.chkBioCustom = New System.Windows.Forms.CheckBox()
        Me.chkBioChronic = New System.Windows.Forms.CheckBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.tbNonBio2 = New System.Windows.Forms.TextBox()
        Me.tabMain.SuspendLayout()
        Me.tabSelectDates.SuspendLayout()
        Me.gbTextOutput.SuspendLayout()
        Me.grpHighLow.SuspendLayout()
        Me.grpDates.SuspendLayout()
        Me.grpYears.SuspendLayout()
        Me.tabNDay.SuspendLayout()
        Me.groupGraph.SuspendLayout()
        Me.panelNdayTrendFrequency.SuspendLayout()
        Me.grpRecurrence.SuspendLayout()
        Me.grpNday.SuspendLayout()
        Me.tabDFLOW.SuspendLayout()
        Me.groupNonBio.SuspendLayout()
        Me.groupBio.SuspendLayout()
        Me.SuspendLayout()
        '
        'MainMenu1
        '
        Me.MainMenu1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuFile, Me.mnuAnalysis, Me.mnuHelp})
        '
        'mnuFile
        '
        Me.mnuFile.Index = 0
        Me.mnuFile.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuFileSelectData})
        Me.mnuFile.Text = "File"
        '
        'mnuFileSelectData
        '
        Me.mnuFileSelectData.Index = 0
        Me.mnuFileSelectData.Text = "Select &Data"
        '
        'mnuAnalysis
        '
        Me.mnuAnalysis.Index = 1
        Me.mnuAnalysis.Text = "Analysis"
        '
        'mnuHelp
        '
        Me.mnuHelp.Index = 2
        Me.mnuHelp.Shortcut = System.Windows.Forms.Shortcut.F1
        Me.mnuHelp.Text = "Help"
        '
        'tabMain
        '
        Me.tabMain.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tabMain.Controls.Add(Me.tabSelectDates)
        Me.tabMain.Controls.Add(Me.tabNDay)
        Me.tabMain.Controls.Add(Me.tabDFLOW)
        Me.tabMain.Location = New System.Drawing.Point(0, 2)
        Me.tabMain.Name = "tabMain"
        Me.tabMain.SelectedIndex = 0
        Me.tabMain.Size = New System.Drawing.Size(523, 523)
        Me.tabMain.TabIndex = 1
        '
        'tabSelectDates
        '
        Me.tabSelectDates.BackColor = System.Drawing.Color.Transparent
        Me.tabSelectDates.Controls.Add(Me.gbTextOutput)
        Me.tabSelectDates.Controls.Add(Me.grpHighLow)
        Me.tabSelectDates.Controls.Add(Me.btnDisplayBasic)
        Me.tabSelectDates.Controls.Add(Me.grpDates)
        Me.tabSelectDates.Controls.Add(Me.grpYears)
        Me.tabSelectDates.Location = New System.Drawing.Point(4, 22)
        Me.tabSelectDates.Name = "tabSelectDates"
        Me.tabSelectDates.Padding = New System.Windows.Forms.Padding(3)
        Me.tabSelectDates.Size = New System.Drawing.Size(515, 497)
        Me.tabSelectDates.TabIndex = 0
        Me.tabSelectDates.Text = "Select Dates"
        Me.tabSelectDates.UseVisualStyleBackColor = True
        '
        'gbTextOutput
        '
        Me.gbTextOutput.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbTextOutput.Controls.Add(Me.txtOutputDir)
        Me.gbTextOutput.Controls.Add(Me.lblOutputDir)
        Me.gbTextOutput.Controls.Add(Me.txtOutputRootName)
        Me.gbTextOutput.Controls.Add(Me.lblBaseFilename)
        Me.gbTextOutput.Location = New System.Drawing.Point(214, 6)
        Me.gbTextOutput.Name = "gbTextOutput"
        Me.gbTextOutput.Size = New System.Drawing.Size(292, 144)
        Me.gbTextOutput.TabIndex = 73
        Me.gbTextOutput.TabStop = False
        Me.gbTextOutput.Text = "Output"
        Me.gbTextOutput.Visible = False
        '
        'txtOutputDir
        '
        Me.txtOutputDir.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtOutputDir.Location = New System.Drawing.Point(6, 45)
        Me.txtOutputDir.Name = "txtOutputDir"
        Me.txtOutputDir.Size = New System.Drawing.Size(280, 20)
        Me.txtOutputDir.TabIndex = 12
        '
        'lblOutputDir
        '
        Me.lblOutputDir.AutoSize = True
        Me.lblOutputDir.Location = New System.Drawing.Point(6, 29)
        Me.lblOutputDir.Name = "lblOutputDir"
        Me.lblOutputDir.Size = New System.Drawing.Size(29, 13)
        Me.lblOutputDir.TabIndex = 31
        Me.lblOutputDir.Text = "Path"
        '
        'txtOutputRootName
        '
        Me.txtOutputRootName.Location = New System.Drawing.Point(6, 102)
        Me.txtOutputRootName.Name = "txtOutputRootName"
        Me.txtOutputRootName.Size = New System.Drawing.Size(201, 20)
        Me.txtOutputRootName.TabIndex = 13
        '
        'lblBaseFilename
        '
        Me.lblBaseFilename.AutoSize = True
        Me.lblBaseFilename.Location = New System.Drawing.Point(6, 86)
        Me.lblBaseFilename.Name = "lblBaseFilename"
        Me.lblBaseFilename.Size = New System.Drawing.Size(52, 13)
        Me.lblBaseFilename.TabIndex = 30
        Me.lblBaseFilename.Text = "File Prefix"
        '
        'grpHighLow
        '
        Me.grpHighLow.Controls.Add(Me.radioHigh)
        Me.grpHighLow.Controls.Add(Me.radioLow)
        Me.grpHighLow.ForeColor = System.Drawing.SystemColors.ControlText
        Me.grpHighLow.Location = New System.Drawing.Point(8, 6)
        Me.grpHighLow.Name = "grpHighLow"
        Me.grpHighLow.Size = New System.Drawing.Size(200, 65)
        Me.grpHighLow.TabIndex = 72
        Me.grpHighLow.TabStop = False
        Me.grpHighLow.Text = "Flow Condition"
        '
        'radioHigh
        '
        Me.radioHigh.AutoSize = True
        Me.radioHigh.Checked = True
        Me.radioHigh.Location = New System.Drawing.Point(6, 19)
        Me.radioHigh.Name = "radioHigh"
        Me.radioHigh.Size = New System.Drawing.Size(47, 17)
        Me.radioHigh.TabIndex = 1
        Me.radioHigh.TabStop = True
        Me.radioHigh.Text = "High"
        Me.radioHigh.UseVisualStyleBackColor = True
        '
        'radioLow
        '
        Me.radioLow.AutoSize = True
        Me.radioLow.Location = New System.Drawing.Point(6, 42)
        Me.radioLow.Name = "radioLow"
        Me.radioLow.Size = New System.Drawing.Size(45, 17)
        Me.radioLow.TabIndex = 2
        Me.radioLow.Text = "Low"
        Me.radioLow.UseVisualStyleBackColor = True
        '
        'btnDisplayBasic
        '
        Me.btnDisplayBasic.Location = New System.Drawing.Point(8, 270)
        Me.btnDisplayBasic.Name = "btnDisplayBasic"
        Me.btnDisplayBasic.Size = New System.Drawing.Size(157, 23)
        Me.btnDisplayBasic.TabIndex = 10
        Me.btnDisplayBasic.Text = "Display Basic Statistics"
        Me.btnDisplayBasic.UseVisualStyleBackColor = True
        '
        'grpDates
        '
        Me.grpDates.BackColor = System.Drawing.SystemColors.Control
        Me.grpDates.Controls.Add(Me.cboStartMonth)
        Me.grpDates.Controls.Add(Me.lblYearStart)
        Me.grpDates.Controls.Add(Me.txtEndDay)
        Me.grpDates.Controls.Add(Me.txtStartDay)
        Me.grpDates.Controls.Add(Me.cboEndMonth)
        Me.grpDates.Controls.Add(Me.lblYearEnd)
        Me.grpDates.ForeColor = System.Drawing.SystemColors.ControlText
        Me.grpDates.Location = New System.Drawing.Point(8, 77)
        Me.grpDates.Name = "grpDates"
        Me.grpDates.Size = New System.Drawing.Size(200, 73)
        Me.grpDates.TabIndex = 67
        Me.grpDates.TabStop = False
        Me.grpDates.Text = "Year / Season Boundaries"
        '
        'cboStartMonth
        '
        Me.cboStartMonth.Items.AddRange(New Object() {"January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"})
        Me.cboStartMonth.Location = New System.Drawing.Point(41, 19)
        Me.cboStartMonth.MaxDropDownItems = 12
        Me.cboStartMonth.Name = "cboStartMonth"
        Me.cboStartMonth.Size = New System.Drawing.Size(88, 21)
        Me.cboStartMonth.TabIndex = 3
        Me.cboStartMonth.Text = "January"
        '
        'lblYearStart
        '
        Me.lblYearStart.AutoSize = True
        Me.lblYearStart.Location = New System.Drawing.Point(6, 22)
        Me.lblYearStart.Name = "lblYearStart"
        Me.lblYearStart.Size = New System.Drawing.Size(29, 13)
        Me.lblYearStart.TabIndex = 23
        Me.lblYearStart.Text = "Start"
        '
        'txtEndDay
        '
        Me.txtEndDay.Location = New System.Drawing.Point(135, 46)
        Me.txtEndDay.Name = "txtEndDay"
        Me.txtEndDay.Size = New System.Drawing.Size(24, 20)
        Me.txtEndDay.TabIndex = 6
        Me.txtEndDay.Text = "31"
        Me.txtEndDay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtStartDay
        '
        Me.txtStartDay.Location = New System.Drawing.Point(135, 19)
        Me.txtStartDay.Name = "txtStartDay"
        Me.txtStartDay.Size = New System.Drawing.Size(24, 20)
        Me.txtStartDay.TabIndex = 4
        Me.txtStartDay.Text = "1"
        Me.txtStartDay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'cboEndMonth
        '
        Me.cboEndMonth.Items.AddRange(New Object() {"January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"})
        Me.cboEndMonth.Location = New System.Drawing.Point(41, 46)
        Me.cboEndMonth.MaxDropDownItems = 12
        Me.cboEndMonth.Name = "cboEndMonth"
        Me.cboEndMonth.Size = New System.Drawing.Size(88, 21)
        Me.cboEndMonth.TabIndex = 5
        Me.cboEndMonth.Text = "December"
        '
        'lblYearEnd
        '
        Me.lblYearEnd.AutoSize = True
        Me.lblYearEnd.Location = New System.Drawing.Point(6, 49)
        Me.lblYearEnd.Name = "lblYearEnd"
        Me.lblYearEnd.Size = New System.Drawing.Size(26, 13)
        Me.lblYearEnd.TabIndex = 61
        Me.lblYearEnd.Text = "End"
        '
        'grpYears
        '
        Me.grpYears.BackColor = System.Drawing.SystemColors.Control
        Me.grpYears.Controls.Add(Me.lblDataStart)
        Me.grpYears.Controls.Add(Me.lblDataEnd)
        Me.grpYears.Controls.Add(Me.lblOmitBefore)
        Me.grpYears.Controls.Add(Me.lblOmitAfter)
        Me.grpYears.Controls.Add(Me.txtOmitAfterYear)
        Me.grpYears.Controls.Add(Me.txtOmitBeforeYear)
        Me.grpYears.Controls.Add(Me.cboYears)
        Me.grpYears.ForeColor = System.Drawing.SystemColors.ControlText
        Me.grpYears.Location = New System.Drawing.Point(8, 156)
        Me.grpYears.Name = "grpYears"
        Me.grpYears.Size = New System.Drawing.Size(250, 102)
        Me.grpYears.TabIndex = 66
        Me.grpYears.TabStop = False
        Me.grpYears.Text = "Years to Include in Analysis"
        '
        'lblDataStart
        '
        Me.lblDataStart.AutoSize = True
        Me.lblDataStart.Enabled = False
        Me.lblDataStart.Location = New System.Drawing.Point(123, 49)
        Me.lblDataStart.Name = "lblDataStart"
        Me.lblDataStart.Size = New System.Drawing.Size(119, 13)
        Me.lblDataStart.TabIndex = 45
        Me.lblDataStart.Tag = "Data Starts"
        Me.lblDataStart.Text = "Start Date: 11/22/1934"
        '
        'lblDataEnd
        '
        Me.lblDataEnd.AutoSize = True
        Me.lblDataEnd.Enabled = False
        Me.lblDataEnd.Location = New System.Drawing.Point(123, 75)
        Me.lblDataEnd.Name = "lblDataEnd"
        Me.lblDataEnd.Size = New System.Drawing.Size(116, 13)
        Me.lblDataEnd.TabIndex = 1
        Me.lblDataEnd.Tag = "Data Ends"
        Me.lblDataEnd.Text = "End Date: 11/22/1934"
        '
        'lblOmitBefore
        '
        Me.lblOmitBefore.AutoSize = True
        Me.lblOmitBefore.Enabled = False
        Me.lblOmitBefore.Location = New System.Drawing.Point(6, 49)
        Me.lblOmitBefore.Name = "lblOmitBefore"
        Me.lblOmitBefore.Size = New System.Drawing.Size(54, 13)
        Me.lblOmitBefore.TabIndex = 40
        Me.lblOmitBefore.Text = "Start Year"
        '
        'lblOmitAfter
        '
        Me.lblOmitAfter.AutoSize = True
        Me.lblOmitAfter.Enabled = False
        Me.lblOmitAfter.Location = New System.Drawing.Point(6, 75)
        Me.lblOmitAfter.Name = "lblOmitAfter"
        Me.lblOmitAfter.Size = New System.Drawing.Size(51, 13)
        Me.lblOmitAfter.TabIndex = 43
        Me.lblOmitAfter.Text = "End Year"
        '
        'txtOmitAfterYear
        '
        Me.txtOmitAfterYear.Enabled = False
        Me.txtOmitAfterYear.Location = New System.Drawing.Point(66, 72)
        Me.txtOmitAfterYear.Name = "txtOmitAfterYear"
        Me.txtOmitAfterYear.Size = New System.Drawing.Size(37, 20)
        Me.txtOmitAfterYear.TabIndex = 9
        Me.txtOmitAfterYear.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtOmitBeforeYear
        '
        Me.txtOmitBeforeYear.Enabled = False
        Me.txtOmitBeforeYear.Location = New System.Drawing.Point(66, 46)
        Me.txtOmitBeforeYear.Name = "txtOmitBeforeYear"
        Me.txtOmitBeforeYear.Size = New System.Drawing.Size(37, 20)
        Me.txtOmitBeforeYear.TabIndex = 8
        Me.txtOmitBeforeYear.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'cboYears
        '
        Me.cboYears.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboYears.FormattingEnabled = True
        Me.cboYears.Location = New System.Drawing.Point(6, 18)
        Me.cboYears.Name = "cboYears"
        Me.cboYears.Size = New System.Drawing.Size(233, 21)
        Me.cboYears.TabIndex = 7
        '
        'tabNDay
        '
        Me.tabNDay.BackColor = System.Drawing.Color.Transparent
        Me.tabNDay.Controls.Add(Me.groupGraph)
        Me.tabNDay.Controls.Add(Me.btnFrequencyReport)
        Me.tabNDay.Controls.Add(Me.chkLog)
        Me.tabNDay.Controls.Add(Me.panelNdayTrendFrequency)
        Me.tabNDay.Controls.Add(Me.btnFrequencyGrid)
        Me.tabNDay.Controls.Add(Me.btnScreeningTests)
        Me.tabNDay.Controls.Add(Me.btnNDayList)
        Me.tabNDay.Location = New System.Drawing.Point(4, 22)
        Me.tabNDay.Name = "tabNDay"
        Me.tabNDay.Padding = New System.Windows.Forms.Padding(3)
        Me.tabNDay.Size = New System.Drawing.Size(515, 497)
        Me.tabNDay.TabIndex = 2
        Me.tabNDay.Text = "N-Day, Trend, Frequency"
        Me.tabNDay.UseVisualStyleBackColor = True
        '
        'groupGraph
        '
        Me.groupGraph.Controls.Add(Me.btnNDayGraph)
        Me.groupGraph.Controls.Add(Me.btnFrequencyGraph)
        Me.groupGraph.Controls.Add(Me.chkMultipleNDayPlots)
        Me.groupGraph.Controls.Add(Me.chkMultipleStationPlots)
        Me.groupGraph.Location = New System.Drawing.Point(164, 362)
        Me.groupGraph.Name = "groupGraph"
        Me.groupGraph.Size = New System.Drawing.Size(160, 127)
        Me.groupGraph.TabIndex = 43
        Me.groupGraph.TabStop = False
        Me.groupGraph.Text = "Graph"
        '
        'btnNDayGraph
        '
        Me.btnNDayGraph.Location = New System.Drawing.Point(6, 19)
        Me.btnNDayGraph.Name = "btnNDayGraph"
        Me.btnNDayGraph.Size = New System.Drawing.Size(140, 23)
        Me.btnNDayGraph.TabIndex = 41
        Me.btnNDayGraph.Text = "N-Day Timeseries Graph"
        Me.btnNDayGraph.UseVisualStyleBackColor = True
        '
        'btnFrequencyGraph
        '
        Me.btnFrequencyGraph.Location = New System.Drawing.Point(6, 48)
        Me.btnFrequencyGraph.Name = "btnFrequencyGraph"
        Me.btnFrequencyGraph.Size = New System.Drawing.Size(140, 23)
        Me.btnFrequencyGraph.TabIndex = 38
        Me.btnFrequencyGraph.Text = "Frequency Graph"
        Me.btnFrequencyGraph.UseVisualStyleBackColor = True
        '
        'chkMultipleNDayPlots
        '
        Me.chkMultipleNDayPlots.AutoSize = True
        Me.chkMultipleNDayPlots.Location = New System.Drawing.Point(15, 77)
        Me.chkMultipleNDayPlots.Name = "chkMultipleNDayPlots"
        Me.chkMultipleNDayPlots.Size = New System.Drawing.Size(121, 17)
        Me.chkMultipleNDayPlots.TabIndex = 39
        Me.chkMultipleNDayPlots.Text = "Multiple N-Day Plots"
        Me.chkMultipleNDayPlots.UseVisualStyleBackColor = True
        '
        'chkMultipleStationPlots
        '
        Me.chkMultipleStationPlots.AutoSize = True
        Me.chkMultipleStationPlots.Location = New System.Drawing.Point(15, 100)
        Me.chkMultipleStationPlots.Name = "chkMultipleStationPlots"
        Me.chkMultipleStationPlots.Size = New System.Drawing.Size(124, 17)
        Me.chkMultipleStationPlots.TabIndex = 40
        Me.chkMultipleStationPlots.Text = "Multiple Station Plots"
        Me.chkMultipleStationPlots.UseVisualStyleBackColor = True
        '
        'btnFrequencyReport
        '
        Me.btnFrequencyReport.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnFrequencyReport.Location = New System.Drawing.Point(6, 468)
        Me.btnFrequencyReport.Name = "btnFrequencyReport"
        Me.btnFrequencyReport.Size = New System.Drawing.Size(140, 23)
        Me.btnFrequencyReport.TabIndex = 42
        Me.btnFrequencyReport.Text = "Frequency Report"
        Me.btnFrequencyReport.UseVisualStyleBackColor = True
        '
        'chkLog
        '
        Me.chkLog.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkLog.AutoSize = True
        Me.chkLog.Checked = True
        Me.chkLog.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkLog.Location = New System.Drawing.Point(6, 353)
        Me.chkLog.Name = "chkLog"
        Me.chkLog.Size = New System.Drawing.Size(80, 17)
        Me.chkLog.TabIndex = 36
        Me.chkLog.Text = "Logarithmic"
        Me.chkLog.UseVisualStyleBackColor = True
        '
        'panelNdayTrendFrequency
        '
        Me.panelNdayTrendFrequency.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.panelNdayTrendFrequency.Controls.Add(Me.grpRecurrence)
        Me.panelNdayTrendFrequency.Controls.Add(Me.Splitter1)
        Me.panelNdayTrendFrequency.Controls.Add(Me.grpNday)
        Me.panelNdayTrendFrequency.Location = New System.Drawing.Point(0, 0)
        Me.panelNdayTrendFrequency.Name = "panelNdayTrendFrequency"
        Me.panelNdayTrendFrequency.Size = New System.Drawing.Size(515, 347)
        Me.panelNdayTrendFrequency.TabIndex = 34
        '
        'grpRecurrence
        '
        Me.grpRecurrence.BackColor = System.Drawing.SystemColors.Control
        Me.grpRecurrence.Controls.Add(Me.btnRecurrenceDefault)
        Me.grpRecurrence.Controls.Add(Me.btnRecurrenceRemove)
        Me.grpRecurrence.Controls.Add(Me.lstRecurrence)
        Me.grpRecurrence.Controls.Add(Me.btnRecurrenceAdd)
        Me.grpRecurrence.Controls.Add(Me.txtRecurrenceAdd)
        Me.grpRecurrence.Controls.Add(Me.btnRecurrenceNone)
        Me.grpRecurrence.Controls.Add(Me.btnRecurrenceAll)
        Me.grpRecurrence.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grpRecurrence.ForeColor = System.Drawing.SystemColors.ControlText
        Me.grpRecurrence.Location = New System.Drawing.Point(208, 0)
        Me.grpRecurrence.Name = "grpRecurrence"
        Me.grpRecurrence.Size = New System.Drawing.Size(307, 347)
        Me.grpRecurrence.TabIndex = 7
        Me.grpRecurrence.TabStop = False
        Me.grpRecurrence.Text = "Recurrence Interval, Years"
        '
        'btnRecurrenceDefault
        '
        Me.btnRecurrenceDefault.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRecurrenceDefault.Location = New System.Drawing.Point(245, 288)
        Me.btnRecurrenceDefault.Name = "btnRecurrenceDefault"
        Me.btnRecurrenceDefault.Size = New System.Drawing.Size(56, 20)
        Me.btnRecurrenceDefault.TabIndex = 31
        Me.btnRecurrenceDefault.Text = "Default"
        '
        'btnRecurrenceRemove
        '
        Me.btnRecurrenceRemove.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRecurrenceRemove.Location = New System.Drawing.Point(211, 288)
        Me.btnRecurrenceRemove.Name = "btnRecurrenceRemove"
        Me.btnRecurrenceRemove.Size = New System.Drawing.Size(28, 20)
        Me.btnRecurrenceRemove.TabIndex = 30
        Me.btnRecurrenceRemove.Text = "-"
        '
        'lstRecurrence
        '
        Me.lstRecurrence.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstRecurrence.IntegralHeight = False
        Me.lstRecurrence.Location = New System.Drawing.Point(6, 19)
        Me.lstRecurrence.Name = "lstRecurrence"
        Me.lstRecurrence.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple
        Me.lstRecurrence.Size = New System.Drawing.Size(293, 263)
        Me.lstRecurrence.TabIndex = 27
        Me.lstRecurrence.Tag = "Return Period"
        '
        'btnRecurrenceAdd
        '
        Me.btnRecurrenceAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRecurrenceAdd.Location = New System.Drawing.Point(179, 288)
        Me.btnRecurrenceAdd.Name = "btnRecurrenceAdd"
        Me.btnRecurrenceAdd.Size = New System.Drawing.Size(27, 20)
        Me.btnRecurrenceAdd.TabIndex = 29
        Me.btnRecurrenceAdd.Text = "+"
        '
        'txtRecurrenceAdd
        '
        Me.txtRecurrenceAdd.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtRecurrenceAdd.Location = New System.Drawing.Point(6, 288)
        Me.txtRecurrenceAdd.Name = "txtRecurrenceAdd"
        Me.txtRecurrenceAdd.Size = New System.Drawing.Size(167, 20)
        Me.txtRecurrenceAdd.TabIndex = 28
        '
        'btnRecurrenceNone
        '
        Me.btnRecurrenceNone.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRecurrenceNone.Location = New System.Drawing.Point(236, 317)
        Me.btnRecurrenceNone.Name = "btnRecurrenceNone"
        Me.btnRecurrenceNone.Size = New System.Drawing.Size(65, 24)
        Me.btnRecurrenceNone.TabIndex = 33
        Me.btnRecurrenceNone.Text = "None"
        '
        'btnRecurrenceAll
        '
        Me.btnRecurrenceAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnRecurrenceAll.Location = New System.Drawing.Point(6, 317)
        Me.btnRecurrenceAll.Name = "btnRecurrenceAll"
        Me.btnRecurrenceAll.Size = New System.Drawing.Size(64, 24)
        Me.btnRecurrenceAll.TabIndex = 32
        Me.btnRecurrenceAll.Text = "All"
        '
        'Splitter1
        '
        Me.Splitter1.BackColor = System.Drawing.SystemColors.Control
        Me.Splitter1.Location = New System.Drawing.Point(200, 0)
        Me.Splitter1.Name = "Splitter1"
        Me.Splitter1.Size = New System.Drawing.Size(8, 347)
        Me.Splitter1.TabIndex = 13
        Me.Splitter1.TabStop = False
        '
        'grpNday
        '
        Me.grpNday.BackColor = System.Drawing.SystemColors.Control
        Me.grpNday.Controls.Add(Me.btnNdayDefault)
        Me.grpNday.Controls.Add(Me.btnNdayRemove)
        Me.grpNday.Controls.Add(Me.btnNdayAdd)
        Me.grpNday.Controls.Add(Me.txtNdayAdd)
        Me.grpNday.Controls.Add(Me.btnNdayNone)
        Me.grpNday.Controls.Add(Me.btnNdayAll)
        Me.grpNday.Controls.Add(Me.lstNday)
        Me.grpNday.Dock = System.Windows.Forms.DockStyle.Left
        Me.grpNday.ForeColor = System.Drawing.SystemColors.ControlText
        Me.grpNday.Location = New System.Drawing.Point(0, 0)
        Me.grpNday.Name = "grpNday"
        Me.grpNday.Size = New System.Drawing.Size(200, 347)
        Me.grpNday.TabIndex = 1
        Me.grpNday.TabStop = False
        Me.grpNday.Text = "Number of Days"
        '
        'btnNdayDefault
        '
        Me.btnNdayDefault.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnNdayDefault.Location = New System.Drawing.Point(138, 288)
        Me.btnNdayDefault.Name = "btnNdayDefault"
        Me.btnNdayDefault.Size = New System.Drawing.Size(56, 20)
        Me.btnNdayDefault.TabIndex = 24
        Me.btnNdayDefault.Text = "Default"
        '
        'btnNdayRemove
        '
        Me.btnNdayRemove.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnNdayRemove.Location = New System.Drawing.Point(105, 288)
        Me.btnNdayRemove.Name = "btnNdayRemove"
        Me.btnNdayRemove.Size = New System.Drawing.Size(27, 20)
        Me.btnNdayRemove.TabIndex = 23
        Me.btnNdayRemove.Text = "-"
        '
        'btnNdayAdd
        '
        Me.btnNdayAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnNdayAdd.Location = New System.Drawing.Point(72, 288)
        Me.btnNdayAdd.Name = "btnNdayAdd"
        Me.btnNdayAdd.Size = New System.Drawing.Size(27, 20)
        Me.btnNdayAdd.TabIndex = 22
        Me.btnNdayAdd.Text = "+"
        '
        'txtNdayAdd
        '
        Me.txtNdayAdd.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtNdayAdd.Location = New System.Drawing.Point(6, 288)
        Me.txtNdayAdd.Name = "txtNdayAdd"
        Me.txtNdayAdd.Size = New System.Drawing.Size(54, 20)
        Me.txtNdayAdd.TabIndex = 21
        '
        'btnNdayNone
        '
        Me.btnNdayNone.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnNdayNone.Location = New System.Drawing.Point(130, 317)
        Me.btnNdayNone.Name = "btnNdayNone"
        Me.btnNdayNone.Size = New System.Drawing.Size(64, 23)
        Me.btnNdayNone.TabIndex = 26
        Me.btnNdayNone.Text = "None"
        '
        'btnNdayAll
        '
        Me.btnNdayAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnNdayAll.Location = New System.Drawing.Point(6, 317)
        Me.btnNdayAll.Name = "btnNdayAll"
        Me.btnNdayAll.Size = New System.Drawing.Size(64, 24)
        Me.btnNdayAll.TabIndex = 25
        Me.btnNdayAll.Text = "All"
        '
        'lstNday
        '
        Me.lstNday.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstNday.IntegralHeight = False
        Me.lstNday.Location = New System.Drawing.Point(6, 19)
        Me.lstNday.Name = "lstNday"
        Me.lstNday.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple
        Me.lstNday.Size = New System.Drawing.Size(188, 263)
        Me.lstNday.TabIndex = 20
        Me.lstNday.Tag = "NDay"
        '
        'btnFrequencyGrid
        '
        Me.btnFrequencyGrid.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnFrequencyGrid.Location = New System.Drawing.Point(6, 410)
        Me.btnFrequencyGrid.Name = "btnFrequencyGrid"
        Me.btnFrequencyGrid.Size = New System.Drawing.Size(140, 23)
        Me.btnFrequencyGrid.TabIndex = 37
        Me.btnFrequencyGrid.Text = "Frequency Grid"
        Me.btnFrequencyGrid.UseVisualStyleBackColor = True
        '
        'btnScreeningTests
        '
        Me.btnScreeningTests.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnScreeningTests.Location = New System.Drawing.Point(6, 439)
        Me.btnScreeningTests.Name = "btnScreeningTests"
        Me.btnScreeningTests.Size = New System.Drawing.Size(140, 23)
        Me.btnScreeningTests.TabIndex = 35
        Me.btnScreeningTests.Text = "Screening Tests"
        Me.btnScreeningTests.UseVisualStyleBackColor = True
        '
        'btnNDayList
        '
        Me.btnNDayList.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnNDayList.Location = New System.Drawing.Point(6, 381)
        Me.btnNDayList.Name = "btnNDayList"
        Me.btnNDayList.Size = New System.Drawing.Size(140, 23)
        Me.btnNDayList.TabIndex = 34
        Me.btnNDayList.Text = "N-Day Timeseries List"
        Me.btnNDayList.UseVisualStyleBackColor = True
        '
        'tabDFLOW
        '
        Me.tabDFLOW.Controls.Add(Me.btnCalculate)
        Me.tabDFLOW.Controls.Add(Me.groupNonBio)
        Me.tabDFLOW.Controls.Add(Me.groupBio)
        Me.tabDFLOW.Controls.Add(Me.Label8)
        Me.tabDFLOW.Controls.Add(Me.tbNonBio2)
        Me.tabDFLOW.Location = New System.Drawing.Point(4, 22)
        Me.tabDFLOW.Name = "tabDFLOW"
        Me.tabDFLOW.Padding = New System.Windows.Forms.Padding(3)
        Me.tabDFLOW.Size = New System.Drawing.Size(515, 497)
        Me.tabDFLOW.TabIndex = 3
        Me.tabDFLOW.Text = "Design Flow"
        Me.tabDFLOW.UseVisualStyleBackColor = True
        '
        'btnCalculate
        '
        Me.btnCalculate.Location = New System.Drawing.Point(8, 466)
        Me.btnCalculate.Name = "btnCalculate"
        Me.btnCalculate.Size = New System.Drawing.Size(75, 23)
        Me.btnCalculate.TabIndex = 7
        Me.btnCalculate.Text = "Calculate"
        Me.btnCalculate.UseVisualStyleBackColor = True
        '
        'groupNonBio
        '
        Me.groupNonBio.Controls.Add(Me.chkNonBioHarmonicMean)
        Me.groupNonBio.Controls.Add(Me.chkNonBioCustom2)
        Me.groupNonBio.Controls.Add(Me.txtNonBioCustomNday2)
        Me.groupNonBio.Controls.Add(Me.txtNonBioCustomReturn2)
        Me.groupNonBio.Controls.Add(Me.chkNonBioFlowPercentile)
        Me.groupNonBio.Controls.Add(Me.chkNonBioExplicitFlow)
        Me.groupNonBio.Controls.Add(Me.chkNonBioCustom1)
        Me.groupNonBio.Controls.Add(Me.chkNonBioChronic)
        Me.groupNonBio.Controls.Add(Me.chkNonBioAcute)
        Me.groupNonBio.Controls.Add(Me.lblNonBioReturn)
        Me.groupNonBio.Controls.Add(Me.txtNonBioCustomNday1)
        Me.groupNonBio.Controls.Add(Me.txtNonBioCustomReturn1)
        Me.groupNonBio.Controls.Add(Me.lblNonBioNday)
        Me.groupNonBio.Controls.Add(Me.txtNonBioExplicitFlow)
        Me.groupNonBio.Controls.Add(Me.txtNonBioFlowPercentile)
        Me.groupNonBio.Controls.Add(Me.lblNonBioExplicitFlowUnits)
        Me.groupNonBio.Controls.Add(Me.lblNonBioFlowPercentileUnits)
        Me.groupNonBio.Location = New System.Drawing.Point(8, 225)
        Me.groupNonBio.Name = "groupNonBio"
        Me.groupNonBio.Size = New System.Drawing.Size(332, 220)
        Me.groupNonBio.TabIndex = 6
        Me.groupNonBio.TabStop = False
        Me.groupNonBio.Text = "Non-Biological Design Flow Parameters"
        '
        'chkNonBioHarmonicMean
        '
        Me.chkNonBioHarmonicMean.AutoSize = True
        Me.chkNonBioHarmonicMean.Checked = True
        Me.chkNonBioHarmonicMean.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkNonBioHarmonicMean.Location = New System.Drawing.Point(6, 193)
        Me.chkNonBioHarmonicMean.Name = "chkNonBioHarmonicMean"
        Me.chkNonBioHarmonicMean.Size = New System.Drawing.Size(101, 17)
        Me.chkNonBioHarmonicMean.TabIndex = 28
        Me.chkNonBioHarmonicMean.Text = "Harmonic Mean"
        Me.chkNonBioHarmonicMean.UseVisualStyleBackColor = True
        '
        'chkNonBioCustom2
        '
        Me.chkNonBioCustom2.AutoSize = True
        Me.chkNonBioCustom2.Location = New System.Drawing.Point(6, 118)
        Me.chkNonBioCustom2.Name = "chkNonBioCustom2"
        Me.chkNonBioCustom2.Size = New System.Drawing.Size(61, 17)
        Me.chkNonBioCustom2.TabIndex = 27
        Me.chkNonBioCustom2.Text = "Custom"
        Me.chkNonBioCustom2.UseVisualStyleBackColor = True
        '
        'txtNonBioCustomNday2
        '
        Me.txtNonBioCustomNday2.Enabled = False
        Me.txtNonBioCustomNday2.Location = New System.Drawing.Point(120, 116)
        Me.txtNonBioCustomNday2.Name = "txtNonBioCustomNday2"
        Me.txtNonBioCustomNday2.Size = New System.Drawing.Size(49, 20)
        Me.txtNonBioCustomNday2.TabIndex = 25
        Me.txtNonBioCustomNday2.Text = "30"
        Me.txtNonBioCustomNday2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtNonBioCustomReturn2
        '
        Me.txtNonBioCustomReturn2.Enabled = False
        Me.txtNonBioCustomReturn2.Location = New System.Drawing.Point(175, 116)
        Me.txtNonBioCustomReturn2.Name = "txtNonBioCustomReturn2"
        Me.txtNonBioCustomReturn2.Size = New System.Drawing.Size(49, 20)
        Me.txtNonBioCustomReturn2.TabIndex = 26
        Me.txtNonBioCustomReturn2.Text = "5"
        Me.txtNonBioCustomReturn2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'chkNonBioFlowPercentile
        '
        Me.chkNonBioFlowPercentile.AutoSize = True
        Me.chkNonBioFlowPercentile.Location = New System.Drawing.Point(6, 170)
        Me.chkNonBioFlowPercentile.Name = "chkNonBioFlowPercentile"
        Me.chkNonBioFlowPercentile.Size = New System.Drawing.Size(97, 17)
        Me.chkNonBioFlowPercentile.TabIndex = 24
        Me.chkNonBioFlowPercentile.Text = "Flow percentile"
        Me.chkNonBioFlowPercentile.UseVisualStyleBackColor = True
        '
        'chkNonBioExplicitFlow
        '
        Me.chkNonBioExplicitFlow.AutoSize = True
        Me.chkNonBioExplicitFlow.Location = New System.Drawing.Point(6, 147)
        Me.chkNonBioExplicitFlow.Name = "chkNonBioExplicitFlow"
        Me.chkNonBioExplicitFlow.Size = New System.Drawing.Size(110, 17)
        Me.chkNonBioExplicitFlow.TabIndex = 23
        Me.chkNonBioExplicitFlow.Text = "Explicit flow value"
        Me.chkNonBioExplicitFlow.UseVisualStyleBackColor = True
        '
        'chkNonBioCustom1
        '
        Me.chkNonBioCustom1.AutoSize = True
        Me.chkNonBioCustom1.Location = New System.Drawing.Point(6, 92)
        Me.chkNonBioCustom1.Name = "chkNonBioCustom1"
        Me.chkNonBioCustom1.Size = New System.Drawing.Size(61, 17)
        Me.chkNonBioCustom1.TabIndex = 19
        Me.chkNonBioCustom1.Text = "Custom"
        Me.chkNonBioCustom1.UseVisualStyleBackColor = True
        '
        'chkNonBioChronic
        '
        Me.chkNonBioChronic.AutoSize = True
        Me.chkNonBioChronic.Location = New System.Drawing.Point(6, 48)
        Me.chkNonBioChronic.Name = "chkNonBioChronic"
        Me.chkNonBioChronic.Size = New System.Drawing.Size(263, 17)
        Me.chkNonBioChronic.TabIndex = 18
        Me.chkNonBioChronic.Text = "Criterion continuous concentration (chronic, 7Q10)"
        Me.chkNonBioChronic.UseVisualStyleBackColor = True
        '
        'chkNonBioAcute
        '
        Me.chkNonBioAcute.AutoSize = True
        Me.chkNonBioAcute.Location = New System.Drawing.Point(6, 25)
        Me.chkNonBioAcute.Name = "chkNonBioAcute"
        Me.chkNonBioAcute.Size = New System.Drawing.Size(246, 17)
        Me.chkNonBioAcute.TabIndex = 17
        Me.chkNonBioAcute.Text = "Criterion maximum concentration (acute, 1Q10)"
        Me.chkNonBioAcute.UseVisualStyleBackColor = True
        '
        'lblNonBioReturn
        '
        Me.lblNonBioReturn.AccessibleDescription = "Return period on years with excursions (years)"
        Me.lblNonBioReturn.AutoSize = True
        Me.lblNonBioReturn.Location = New System.Drawing.Point(172, 73)
        Me.lblNonBioReturn.Name = "lblNonBioReturn"
        Me.lblNonBioReturn.Size = New System.Drawing.Size(39, 13)
        Me.lblNonBioReturn.TabIndex = 13
        Me.lblNonBioReturn.Text = "Return"
        '
        'txtNonBioCustomNday1
        '
        Me.txtNonBioCustomNday1.Enabled = False
        Me.txtNonBioCustomNday1.Location = New System.Drawing.Point(120, 90)
        Me.txtNonBioCustomNday1.Name = "txtNonBioCustomNday1"
        Me.txtNonBioCustomNday1.Size = New System.Drawing.Size(49, 20)
        Me.txtNonBioCustomNday1.TabIndex = 5
        Me.txtNonBioCustomNday1.Text = "30"
        Me.txtNonBioCustomNday1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtNonBioCustomReturn1
        '
        Me.txtNonBioCustomReturn1.Enabled = False
        Me.txtNonBioCustomReturn1.Location = New System.Drawing.Point(175, 90)
        Me.txtNonBioCustomReturn1.Name = "txtNonBioCustomReturn1"
        Me.txtNonBioCustomReturn1.Size = New System.Drawing.Size(49, 20)
        Me.txtNonBioCustomReturn1.TabIndex = 16
        Me.txtNonBioCustomReturn1.Text = "5"
        Me.txtNonBioCustomReturn1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblNonBioNday
        '
        Me.lblNonBioNday.AutoSize = True
        Me.lblNonBioNday.Location = New System.Drawing.Point(117, 73)
        Me.lblNonBioNday.Name = "lblNonBioNday"
        Me.lblNonBioNday.Size = New System.Drawing.Size(37, 13)
        Me.lblNonBioNday.TabIndex = 12
        Me.lblNonBioNday.Text = "N-Day"
        '
        'txtNonBioExplicitFlow
        '
        Me.txtNonBioExplicitFlow.Enabled = False
        Me.txtNonBioExplicitFlow.Location = New System.Drawing.Point(120, 142)
        Me.txtNonBioExplicitFlow.Name = "txtNonBioExplicitFlow"
        Me.txtNonBioExplicitFlow.Size = New System.Drawing.Size(49, 20)
        Me.txtNonBioExplicitFlow.TabIndex = 11
        Me.txtNonBioExplicitFlow.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtNonBioFlowPercentile
        '
        Me.txtNonBioFlowPercentile.Enabled = False
        Me.txtNonBioFlowPercentile.Location = New System.Drawing.Point(120, 168)
        Me.txtNonBioFlowPercentile.Name = "txtNonBioFlowPercentile"
        Me.txtNonBioFlowPercentile.Size = New System.Drawing.Size(49, 20)
        Me.txtNonBioFlowPercentile.TabIndex = 10
        Me.txtNonBioFlowPercentile.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblNonBioExplicitFlowUnits
        '
        Me.lblNonBioExplicitFlowUnits.AutoSize = True
        Me.lblNonBioExplicitFlowUnits.Enabled = False
        Me.lblNonBioExplicitFlowUnits.Location = New System.Drawing.Point(172, 145)
        Me.lblNonBioExplicitFlowUnits.Name = "lblNonBioExplicitFlowUnits"
        Me.lblNonBioExplicitFlowUnits.Size = New System.Drawing.Size(21, 13)
        Me.lblNonBioExplicitFlowUnits.TabIndex = 8
        Me.lblNonBioExplicitFlowUnits.Text = "cfs"
        '
        'lblNonBioFlowPercentileUnits
        '
        Me.lblNonBioFlowPercentileUnits.AutoSize = True
        Me.lblNonBioFlowPercentileUnits.Enabled = False
        Me.lblNonBioFlowPercentileUnits.Location = New System.Drawing.Point(175, 171)
        Me.lblNonBioFlowPercentileUnits.Name = "lblNonBioFlowPercentileUnits"
        Me.lblNonBioFlowPercentileUnits.Size = New System.Drawing.Size(15, 13)
        Me.lblNonBioFlowPercentileUnits.TabIndex = 7
        Me.lblNonBioFlowPercentileUnits.Text = "%"
        '
        'groupBio
        '
        Me.groupBio.Controls.Add(Me.chkBioAmmonia)
        Me.groupBio.Controls.Add(Me.chkBioAcute)
        Me.groupBio.Controls.Add(Me.txtBioExcursionDays)
        Me.groupBio.Controls.Add(Me.txtBioExcursionsPerCluster)
        Me.groupBio.Controls.Add(Me.txtBioYearsBetween)
        Me.groupBio.Controls.Add(Me.lblBioExcursionsPerCluster)
        Me.groupBio.Controls.Add(Me.lblBioExcursionDays)
        Me.groupBio.Controls.Add(Me.lblBioYearsBetween)
        Me.groupBio.Controls.Add(Me.txtBioFlowDays)
        Me.groupBio.Controls.Add(Me.lblBioFlowDays)
        Me.groupBio.Controls.Add(Me.chkBioCustom)
        Me.groupBio.Controls.Add(Me.chkBioChronic)
        Me.groupBio.Location = New System.Drawing.Point(8, 7)
        Me.groupBio.Name = "groupBio"
        Me.groupBio.Size = New System.Drawing.Size(332, 212)
        Me.groupBio.TabIndex = 5
        Me.groupBio.TabStop = False
        Me.groupBio.Text = "Biological Design Flow Parameters"
        '
        'chkBioAmmonia
        '
        Me.chkBioAmmonia.AutoSize = True
        Me.chkBioAmmonia.Location = New System.Drawing.Point(6, 65)
        Me.chkBioAmmonia.Name = "chkBioAmmonia"
        Me.chkBioAmmonia.Size = New System.Drawing.Size(103, 17)
        Me.chkBioAmmonia.TabIndex = 14
        Me.chkBioAmmonia.Text = "Ammonia (30B3)"
        Me.chkBioAmmonia.UseVisualStyleBackColor = True
        '
        'chkBioAcute
        '
        Me.chkBioAcute.AutoSize = True
        Me.chkBioAcute.Location = New System.Drawing.Point(6, 19)
        Me.chkBioAcute.Name = "chkBioAcute"
        Me.chkBioAcute.Size = New System.Drawing.Size(239, 17)
        Me.chkBioAcute.TabIndex = 12
        Me.chkBioAcute.Text = "Criterion maximum concentration (acute, 1B3)"
        Me.chkBioAcute.UseVisualStyleBackColor = True
        '
        'txtBioExcursionDays
        '
        Me.txtBioExcursionDays.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtBioExcursionDays.Enabled = False
        Me.txtBioExcursionDays.Location = New System.Drawing.Point(280, 157)
        Me.txtBioExcursionDays.Name = "txtBioExcursionDays"
        Me.txtBioExcursionDays.Size = New System.Drawing.Size(46, 20)
        Me.txtBioExcursionDays.TabIndex = 11
        Me.txtBioExcursionDays.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtBioExcursionsPerCluster
        '
        Me.txtBioExcursionsPerCluster.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtBioExcursionsPerCluster.Enabled = False
        Me.txtBioExcursionsPerCluster.Location = New System.Drawing.Point(280, 183)
        Me.txtBioExcursionsPerCluster.Name = "txtBioExcursionsPerCluster"
        Me.txtBioExcursionsPerCluster.Size = New System.Drawing.Size(46, 20)
        Me.txtBioExcursionsPerCluster.TabIndex = 10
        Me.txtBioExcursionsPerCluster.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtBioYearsBetween
        '
        Me.txtBioYearsBetween.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtBioYearsBetween.Enabled = False
        Me.txtBioYearsBetween.Location = New System.Drawing.Point(280, 132)
        Me.txtBioYearsBetween.Name = "txtBioYearsBetween"
        Me.txtBioYearsBetween.Size = New System.Drawing.Size(46, 20)
        Me.txtBioYearsBetween.TabIndex = 9
        Me.txtBioYearsBetween.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblBioExcursionsPerCluster
        '
        Me.lblBioExcursionsPerCluster.AutoSize = True
        Me.lblBioExcursionsPerCluster.Enabled = False
        Me.lblBioExcursionsPerCluster.Location = New System.Drawing.Point(27, 186)
        Me.lblBioExcursionsPerCluster.Name = "lblBioExcursionsPerCluster"
        Me.lblBioExcursionsPerCluster.Size = New System.Drawing.Size(247, 13)
        Me.lblBioExcursionsPerCluster.TabIndex = 8
        Me.lblBioExcursionsPerCluster.Text = "Average number of excursions counted per cluster:"
        '
        'lblBioExcursionDays
        '
        Me.lblBioExcursionDays.AutoSize = True
        Me.lblBioExcursionDays.Enabled = False
        Me.lblBioExcursionDays.Location = New System.Drawing.Point(27, 161)
        Me.lblBioExcursionDays.Name = "lblBioExcursionDays"
        Me.lblBioExcursionDays.Size = New System.Drawing.Size(214, 13)
        Me.lblBioExcursionDays.TabIndex = 7
        Me.lblBioExcursionDays.Text = "Length of excursion clustering period (days):"
        '
        'lblBioYearsBetween
        '
        Me.lblBioYearsBetween.AutoSize = True
        Me.lblBioYearsBetween.Enabled = False
        Me.lblBioYearsBetween.Location = New System.Drawing.Point(27, 136)
        Me.lblBioYearsBetween.Name = "lblBioYearsBetween"
        Me.lblBioYearsBetween.Size = New System.Drawing.Size(225, 13)
        Me.lblBioYearsBetween.TabIndex = 6
        Me.lblBioYearsBetween.Text = "Average number of years between excursions:"
        '
        'txtBioFlowDays
        '
        Me.txtBioFlowDays.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtBioFlowDays.Enabled = False
        Me.txtBioFlowDays.Location = New System.Drawing.Point(280, 108)
        Me.txtBioFlowDays.Name = "txtBioFlowDays"
        Me.txtBioFlowDays.Size = New System.Drawing.Size(46, 20)
        Me.txtBioFlowDays.TabIndex = 5
        Me.txtBioFlowDays.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblBioFlowDays
        '
        Me.lblBioFlowDays.AutoSize = True
        Me.lblBioFlowDays.Enabled = False
        Me.lblBioFlowDays.Location = New System.Drawing.Point(27, 111)
        Me.lblBioFlowDays.Name = "lblBioFlowDays"
        Me.lblBioFlowDays.Size = New System.Drawing.Size(145, 13)
        Me.lblBioFlowDays.TabIndex = 4
        Me.lblBioFlowDays.Text = "Flow averaging period (days):"
        '
        'chkBioCustom
        '
        Me.chkBioCustom.AutoSize = True
        Me.chkBioCustom.Checked = True
        Me.chkBioCustom.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkBioCustom.Location = New System.Drawing.Point(6, 88)
        Me.chkBioCustom.Name = "chkBioCustom"
        Me.chkBioCustom.Size = New System.Drawing.Size(61, 17)
        Me.chkBioCustom.TabIndex = 0
        Me.chkBioCustom.Text = "Custom"
        Me.chkBioCustom.UseVisualStyleBackColor = True
        '
        'chkBioChronic
        '
        Me.chkBioChronic.AutoSize = True
        Me.chkBioChronic.Location = New System.Drawing.Point(6, 42)
        Me.chkBioChronic.Name = "chkBioChronic"
        Me.chkBioChronic.Size = New System.Drawing.Size(256, 17)
        Me.chkBioChronic.TabIndex = 13
        Me.chkBioChronic.Text = "Criterion continuous concentration (chronic, 4B3)"
        Me.chkBioChronic.UseVisualStyleBackColor = True
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Enabled = False
        Me.Label8.Location = New System.Drawing.Point(177, 286)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(145, 13)
        Me.Label8.TabIndex = 4
        Me.Label8.Text = "Flow averaging period (days):"
        '
        'tbNonBio2
        '
        Me.tbNonBio2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbNonBio2.Enabled = False
        Me.tbNonBio2.Location = New System.Drawing.Point(518, 283)
        Me.tbNonBio2.Name = "tbNonBio2"
        Me.tbNonBio2.Size = New System.Drawing.Size(46, 20)
        Me.tbNonBio2.TabIndex = 9
        Me.tbNonBio2.Text = "10"
        Me.tbNonBio2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'frmSWSTAT
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(523, 525)
        Me.Controls.Add(Me.tabMain)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Menu = Me.MainMenu1
        Me.Name = "frmSWSTAT"
        Me.Text = "Integrated Design Flow"
        Me.tabMain.ResumeLayout(False)
        Me.tabSelectDates.ResumeLayout(False)
        Me.gbTextOutput.ResumeLayout(False)
        Me.gbTextOutput.PerformLayout()
        Me.grpHighLow.ResumeLayout(False)
        Me.grpHighLow.PerformLayout()
        Me.grpDates.ResumeLayout(False)
        Me.grpDates.PerformLayout()
        Me.grpYears.ResumeLayout(False)
        Me.grpYears.PerformLayout()
        Me.tabNDay.ResumeLayout(False)
        Me.tabNDay.PerformLayout()
        Me.groupGraph.ResumeLayout(False)
        Me.groupGraph.PerformLayout()
        Me.panelNdayTrendFrequency.ResumeLayout(False)
        Me.grpRecurrence.ResumeLayout(False)
        Me.grpRecurrence.PerformLayout()
        Me.grpNday.ResumeLayout(False)
        Me.grpNday.PerformLayout()
        Me.tabDFLOW.ResumeLayout(False)
        Me.tabDFLOW.PerformLayout()
        Me.groupNonBio.ResumeLayout(False)
        Me.groupNonBio.PerformLayout()
        Me.groupBio.ResumeLayout(False)
        Me.groupBio.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

#End Region

    'The group of atcTimeseries displayed
    Private WithEvents pDataGroup As atcTimeseriesGroup
    Private WithEvents pNDayGroup As atcTimeseriesGroup

    Private pDateFormat As New atcDateFormat

    'Value formatting options, can be overridden by timeseries attributes
    Private pMaxWidth As Integer = 10
    Private pFormat As String = "#,##0.########"
    Private pExpFormat As String = "#.#e#"
    Private pCantFit As String = "#"
    Private pSignificantDigits As Integer = 5

    'Private pDataAttributes As ArrayList
    Private pBasicAttributes As Generic.List(Of String)
    Private pNDayAttributes As Generic.List(Of String)
    Private pTrendAttributes As Generic.List(Of String)

    Private pYearStartMonth As Integer = 0
    Private pYearStartDay As Integer = 0
    Private pYearEndMonth As Integer = 0
    Private pYearEndDay As Integer = 0
    Private pFirstYear As Integer = 0
    Private pLastYear As Integer = 0

    Private pCommonStart As Double = GetMinValue()
    Private pCommonEnd As Double = GetMaxValue()

    Private Const pNoDatesInCommon As String = ": No dates in common"

    Public Event ParametersSet(ByVal aArgs As atcDataAttributes)
    Private pAttributes As atcDataAttributes
    Private pSetGlobal As Boolean = False
    Private pBatch As Boolean = False

    Private Shared pLastDayOfMonth() As Integer = {99, 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31}

    Private pHelpLocation As String = "BASINS Details\Analysis\USGS Surface Water Statistics.html"
    Private Sub mnuHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuHelp.Click
        ShowHelp(pHelpLocation)
    End Sub

    Private Sub PopulateForm()
        pDateFormat = New atcDateFormat
        With pDateFormat
            .IncludeHours = False
            .IncludeMinutes = False
            .IncludeSeconds = False
        End With

        If GetSetting("atcFrequencyGrid", "Defaults", "HighOrLow", "High") = "High" Then
            radioHigh.Checked = True
        Else
            radioLow.Checked = True
        End If

        chkLog.Checked = (GetSetting("atcFrequencyGrid", "Defaults", "Logarithmic", "True") = "True")

        LoadListSettingsOrDefaults(lstNday)
        LoadListSettingsOrDefaults(lstRecurrence)
        RepopulateForm()
    End Sub

    Private Sub PopulateForm(ByVal attribs As atcDataAttributes)
        pDateFormat = New atcDateFormat
        With pDateFormat
            .IncludeHours = False
            .IncludeMinutes = False
            .IncludeSeconds = False
        End With

        Dim lHighLow As String = attribs.GetValue(InputNames.HighLow, "")
        If lHighLow.ToLower = "high" Then
            radioHigh.Checked = True
        Else
            radioLow.Checked = True
        End If

        chkLog.Checked = attribs.GetValue(InputNames.Logarithmic, True)
        chkMultipleNDayPlots.Checked = attribs.GetValue(InputNames.MultiNDayPlot, False)
        chkMultipleStationPlots.Checked = attribs.GetValue(InputNames.MultiStationPlot, False)
        Dim lOutDir As String = attribs.GetValue(InputNames.OutputDir, "")
        If IO.Directory.Exists(lOutDir) Then
            txtOutputDir.Text = lOutDir
        End If
        Dim lPrefix As String = attribs.GetValue(InputNames.OutputPrefix, "")
        txtOutputRootName.Text = lPrefix

        LoadListSettingsOrDefaults(lstNday, attribs)
        LoadListSettingsOrDefaults(lstRecurrence, attribs)
        RepopulateForm()
    End Sub

    Private Sub RepopulateForm()
        SeasonsYearsToForm()

        Dim lFirstDate As Double = GetMaxValue()
        Dim lLastDate As Double = GetMinValue()

        pCommonStart = GetMinValue()
        pCommonEnd = GetMaxValue()

        Dim lAllText As String = "All"
        Dim lCommonText As String = "Common"

        Dim lHaveAnnual As Boolean = False
        Dim lAnnualIsHigh As Boolean = False
        Dim lAnnualIsLow As Boolean = False
        Dim lAnnualNday As Integer = 0

        For Each lDataset As atcData.atcTimeseries In pDataGroup
            If lDataset.Dates.numValues > 0 Then
                Dim lThisDate As Double = lDataset.Dates.Value(1)
                If lThisDate < lFirstDate Then lFirstDate = lThisDate
                If lThisDate > pCommonStart Then pCommonStart = lThisDate
                lThisDate = lDataset.Dates.Value(lDataset.Dates.numValues)
                If lThisDate > lLastDate Then lLastDate = lThisDate
                If lThisDate < pCommonEnd Then pCommonEnd = lThisDate

                If lDataset.Attributes.GetValue("Tu", atcTimeUnit.TUDay) = atcTimeUnit.TUYear Then
                    lHaveAnnual = True
                    Dim lConstituent As String = lDataset.Attributes.GetValue("Constituent", "")
                    If lConstituent.Length < 4 Then
                        Logger.Msg("Dataset #" & lDataset.Attributes.GetValue("ID", "") & " " & lDataset.ToString,
                                   "Annual Dataset Missing Constituent")
                    Else
                        Select Case lConstituent.Substring(0, 1).ToUpper
                            Case "L" : lAnnualIsLow = True
                            Case "H" : lAnnualIsHigh = True
                            Case Else
                                Logger.Msg("Dataset #" & lDataset.Attributes.GetValue("ID", "") & " " & lDataset.ToString,
                                           "Annual Dataset Constituent does not start with L or H")
                        End Select
                        Dim lNday As Integer
                        If Integer.TryParse(lConstituent.Substring(1), lNday) Then
                            If lAnnualNday = 0 Then
                                lAnnualNday = lNday
                            ElseIf lAnnualNday <> lNday Then
                                Logger.Msg("Annual datasets with different N-Day cannot be analyzed at the same time." & vbCrLf _
                                           & "N-Day values of both " & lAnnualNday & " and " & lNday & " were found.",
                                           "Incompatible Annual Data Selected")
                            End If
                        End If
                    End If
                End If
            End If
        Next

        grpHighLow.Enabled = True
        grpNday.Enabled = True
        If lHaveAnnual Then
            If lAnnualIsLow <> lAnnualIsHigh Then
                radioHigh.Checked = lAnnualIsHigh
                radioLow.Checked = Not lAnnualIsHigh
                grpHighLow.Enabled = False
            Else
                If lAnnualIsLow Then
                    Logger.Msg("Low and high annual datasets cannot be analyzed at the same time",
                               "Incompatible Annual Data Selected")
                End If
            End If

            If lAnnualNday > 0 Then
                lstNday.ClearSelected()
                SelectNday(lAnnualNday)
                grpNday.Enabled = False
            End If
        End If

        If lFirstDate < GetMaxValue() AndAlso lLastDate > GetMinValue() Then
            lblDataStart.Text = lblDataStart.Tag & " " & pDateFormat.JDateToString(lFirstDate)
            lblDataEnd.Text = lblDataEnd.Tag & " " & pDateFormat.JDateToString(lLastDate)
            lAllText &= ": " & pDateFormat.JDateToString(lFirstDate) & " to " & pDateFormat.JDateToString(lLastDate)
        End If

        If pCommonStart > GetMinValue() AndAlso pCommonEnd < GetMaxValue() AndAlso pCommonStart < pCommonEnd Then
            lCommonText &= ": " & pDateFormat.JDateToString(pCommonStart) & " to " & pDateFormat.JDateToString(pCommonEnd)
        Else
            lCommonText &= pNoDatesInCommon
        End If

        Dim lLastSelectedIndex As Integer = cboYears.SelectedIndex
        If lLastSelectedIndex < 0 Then lLastSelectedIndex = 0
        With cboYears.Items
            .Clear()
            .Add(lAllText)
            .Add(lCommonText)
            .Add("Custom")
        End With
        cboYears.SelectedIndex = lLastSelectedIndex


    End Sub

    Private Sub LoadListSettingsOrDefaults(ByVal lst As Windows.Forms.ListBox)
        Dim lArgName As String = lst.Tag
        Dim lAvailableArray As String(,) = GetAllSettings("atcFrequencyGrid", "List." & lArgName)
        Dim lSelected As New ArrayList
        lst.Items.Clear()

        If Not lAvailableArray Is Nothing AndAlso lAvailableArray.Length > 0 Then
            Try
                For lIndex As Integer = 0 To lAvailableArray.GetUpperBound(0)
                    lst.Items.Add(lAvailableArray(lIndex, 0))
                    If lAvailableArray(lIndex, 1) = "True" Then
                        lst.SetSelected(lst.Items.Count - 1, True)
                    End If
                Next
            Catch e As Exception
                Logger.Dbg("Error retrieving saved settings: " & e.Message)
            End Try
        Else
            LoadListDefaults(lst)
        End If
    End Sub

    Private Sub LoadListSettingsOrDefaults(ByVal lst As Windows.Forms.ListBox, ByVal attribs As atcDataAttributes)
        Dim lArgName As String = lst.Tag & "s"
        Dim listing As atcCollection = attribs.GetValue(lArgName, Nothing)
        If listing Is Nothing Then
            Exit Sub
        End If

        Dim lSelected As New ArrayList
        lst.Items.Clear()

        If listing.Count > 0 Then
            Try
                For lIndex As Integer = 0 To listing.Count - 1
                    lst.Items.Add(listing.Keys(lIndex))
                    If listing.ItemByIndex(lIndex) Then
                        lst.SetSelected(lst.Items.Count - 1, True)
                    End If
                Next
            Catch e As Exception
                Logger.Dbg("Error retrieving " & lArgName & " settings: " & e.Message)
            End Try
        Else
            LoadListDefaults(lst)
        End If
    End Sub

    Private Sub frmSWSTAT_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyValue = Windows.Forms.Keys.F1 Then
            ShowHelp(pHelpLocation)
        End If
    End Sub

    Public Sub Initialize(Optional ByVal aTimeseriesGroup As atcData.atcTimeseriesGroup = Nothing,
                          Optional ByVal aBasicAttributes As Generic.List(Of String) = Nothing,
                          Optional ByVal aNDayAttributes As Generic.List(Of String) = Nothing,
                          Optional ByVal aTrendAttributes As Generic.List(Of String) = Nothing,
                          Optional ByVal aShowForm As Boolean = True)

        If aTimeseriesGroup Is Nothing Then
            pDataGroup = New atcTimeseriesGroup
        Else
            pDataGroup = aTimeseriesGroup
        End If

        If aBasicAttributes Is Nothing Then
            pBasicAttributes = atcDataManager.DisplayAttributes
        Else
            pBasicAttributes = aBasicAttributes
        End If

        If aNDayAttributes Is Nothing Then
            pNDayAttributes = atcDataManager.DisplayAttributes
        Else
            pNDayAttributes = aNDayAttributes
        End If

        If aTrendAttributes Is Nothing Then
            pTrendAttributes = atcDataManager.DisplayAttributes
        Else
            pTrendAttributes = aTrendAttributes
        End If

        If aShowForm Then
            Dim DisplayPlugins As ICollection = atcDataManager.GetPlugins(GetType(atcDataDisplay))
            For Each lDisp As atcDataDisplay In DisplayPlugins
                Dim lMenuText As String = lDisp.Name
                If lMenuText <> "Analysis::USGS Surface Water Statistics (SWSTAT)::Integrated Frequency Analysis" Then
                    If lMenuText.StartsWith("Analysis::") Then lMenuText = lMenuText.Substring(10)
                    mnuAnalysis.MenuItems.Add(lMenuText, New EventHandler(AddressOf mnuAnalysis_Click))
                End If
            Next
        End If

        If pDataGroup.Count = 0 Then 'ask user to specify some timeseries
            pDataGroup = atcDataManager.UserSelectData("Select Data for Integrated Frequency Analysis",
                                                       pDataGroup, Nothing, True, True, Me.Icon)
        End If
        DFLOWCalcs_Initialize()
        If pDataGroup.Count > 0 Then
            PopulateForm()
            PopulateDFLOW(pDataGroup)
            If aShowForm Then Me.Show()
        Else 'user declined to specify timeseries
            Me.Close()
        End If

    End Sub

    Public Sub Initialize(ByVal aTimeseriesGroup As atcData.atcTimeseriesGroup,
                          ByVal attributes As atcDataAttributes)

        'Optional ByVal aBasicAttributes As Generic.List(Of String) = Nothing, _
        'Optional ByVal aNDayAttributes As Generic.List(Of String) = Nothing, _
        'Optional ByVal aTrendAttributes As Generic.List(Of String) = Nothing, _
        'Optional ByVal aShowForm As Boolean = True
        DFLOWCalcs_Initialize(attributes)
        If aTimeseriesGroup Is Nothing Then
            pDataGroup = New atcTimeseriesGroup
        Else
            pDataGroup = aTimeseriesGroup
        End If

        pAttributes = attributes

        pBatch = True
        Dim loperation As String = attributes.GetValue("Operation", "")
        If loperation.ToLower = "globalsetparm" Then
            pSetGlobal = True
            btnFrequencyGrid.Text = "Set Global Parameters"
            Me.Text &= " -Batch Run Global Default Parameter Setup"

        Else
            pSetGlobal = False
            btnFrequencyGrid.Text = "Set Group Parameters"
            Dim lGroupname As String = attributes.GetValue("group", "")
            Me.Text &= " -Batch Run Group (" & lGroupname & ") Parameter Setup"
        End If
        btnNDayList.Visible = False
        btnScreeningTests.Visible = False
        btnDisplayBasic.Visible = False
        btnFrequencyGraph.Visible = False
        gbTextOutput.Visible = True

        PopulateForm(attributes)
        Me.Show()
    End Sub

    Public Property DateFormat() As atcDateFormat
        Get
            Return pDateFormat
        End Get
        Set(ByVal newValue As atcDateFormat)
            pDateFormat = newValue
        End Set
    End Property

    Private Sub mnuAnalysis_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAnalysis.Click
        atcDataManager.ShowDisplay(sender.Text, pDataGroup, Me.Icon)
    End Sub

    Private Sub pDataGroup_Added(ByVal aAdded As atcCollection) Handles pDataGroup.Added
        If Me.Visible Then RepopulateForm()
        'TODO: could efficiently insert newly added item(s)
    End Sub

    Private Sub pDataGroup_Removed(ByVal aRemoved As atcCollection) Handles pDataGroup.Removed
        If Me.Visible Then RepopulateForm()
        'TODO: could efficiently remove by serial number
    End Sub

    Protected Overrides Sub OnClosing(ByVal e As System.ComponentModel.CancelEventArgs)
        pDataGroup = Nothing
    End Sub

    Private Sub mnuFileSelectData_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFileSelectData.Click
        atcDataManager.UserSelectData("Select Data for Integrated Frequency Analysis",
                                      pDataGroup, Nothing, False, True, Me.Icon)
    End Sub

    Private Sub SeasonsYearsToForm()
        If cboStartMonth.Items.Count > 0 Then
            cboStartMonth.SelectedIndex = pYearStartMonth - 1
            cboEndMonth.SelectedIndex = pYearEndMonth - 1
            If pYearStartDay > 0 Then txtStartDay.Text = pYearStartDay Else txtStartDay.Text = ""
            If pYearEndDay > 0 Then txtEndDay.Text = pYearEndDay Else txtEndDay.Text = ""
            If pFirstYear > 0 Then txtOmitBeforeYear.Text = pFirstYear Else txtOmitBeforeYear.Text = ""
            If pLastYear > 0 Then txtOmitAfterYear.Text = pLastYear Else txtOmitAfterYear.Text = ""
            If pFirstYear > 0 OrElse pLastYear > 0 Then
                ShowCustomYears(True)
            End If
        End If
    End Sub

    Private Function SeasonsYearsFromFormBatch() As String
        pYearStartMonth = cboStartMonth.SelectedIndex + 1
        pYearEndMonth = cboEndMonth.SelectedIndex + 1
        If IsNumeric(txtStartDay.Text) Then
            pYearStartDay = Math.Min(CInt(txtStartDay.Text), DayMon(1901, pYearStartMonth))
            txtStartDay.Text = pYearStartDay
        Else
            pYearStartDay = 0
        End If
        If IsNumeric(txtEndDay.Text) Then
            pYearEndDay = Math.Min(CInt(txtEndDay.Text), DayMon(1901, pYearEndMonth))
            txtEndDay.Text = pYearEndDay
        Else
            pYearEndDay = 0
        End If
        If IsNumeric(txtOmitBeforeYear.Text) Then
            pFirstYear = CInt(txtOmitBeforeYear.Text)
        Else
            pFirstYear = 0
        End If
        If IsNumeric(txtOmitAfterYear.Text) Then
            pLastYear = CInt(txtOmitAfterYear.Text)
        Else
            pLastYear = 0
        End If
        Dim lMsg As String = SaveSettingsBatch()
        Return lMsg
    End Function

    Private Sub SeasonsYearsFromForm()
        pYearStartMonth = cboStartMonth.SelectedIndex + 1
        pYearEndMonth = cboEndMonth.SelectedIndex + 1
        If IsNumeric(txtStartDay.Text) Then
            pYearStartDay = Math.Min(CInt(txtStartDay.Text), DayMon(1901, pYearStartMonth))
            txtStartDay.Text = pYearStartDay
        Else
            pYearStartDay = 0
        End If
        If IsNumeric(txtEndDay.Text) Then
            pYearEndDay = Math.Min(CInt(txtEndDay.Text), DayMon(1901, pYearEndMonth))
            txtEndDay.Text = pYearEndDay
        Else
            pYearEndDay = 0
        End If
        If IsNumeric(txtOmitBeforeYear.Text) Then
            pFirstYear = CInt(txtOmitBeforeYear.Text)
        Else
            pFirstYear = 0
        End If
        If IsNumeric(txtOmitAfterYear.Text) Then
            pLastYear = CInt(txtOmitAfterYear.Text)
        Else
            pLastYear = 0
        End If
        SaveSettings()
    End Sub

    Private Sub ShowCustomYears(ByVal aShowCustom As Boolean)
        'cboYears.Visible = Not aShowCustom
        txtOmitBeforeYear.Enabled = aShowCustom
        txtOmitAfterYear.Enabled = aShowCustom
        lblDataStart.Enabled = aShowCustom
        lblDataEnd.Enabled = aShowCustom
        lblOmitBefore.Enabled = aShowCustom
        lblOmitAfter.Enabled = aShowCustom
    End Sub

    'Return all selected items, or if none are selected then all items
    Private Function ListToArray(ByVal aList As Windows.Forms.ListBox) As Double()
        Dim lArray() As Double
        Dim lCollection As New ArrayList
        If aList.SelectedItems.Count > 0 Then
            For lIndex As Integer = 0 To aList.SelectedItems.Count - 1
                If IsNumeric(aList.SelectedItems(lIndex)) Then
                    lCollection.Add(CDbl(aList.SelectedItems(lIndex)))
                End If
            Next
        Else
            For lIndex As Integer = 0 To aList.Items.Count - 1
                If IsNumeric(aList.Items(lIndex)) Then
                    lCollection.Add(CDbl(aList.Items(lIndex)))
                End If
            Next
        End If
        ReDim lArray(lCollection.Count - 1)
        For lIndex As Integer = 0 To lCollection.Count - 1
            lArray(lIndex) = lCollection.Item(lIndex)
        Next
        Return lArray
    End Function

    Private Function SelectedData() As atcTimeseriesGroup
        Dim lDataGroupB As New atcTimeseriesGroup
        Dim lTsB As atcTimeseries
        SeasonsYearsFromForm()

        For Each lTs As atcTimeseries In pDataGroup
            If lTs.Attributes.GetValue("Time Unit") = atcTimeUnit.TUYear Then
                lTsB = lTs
            Else
                If pFirstYear > 0 AndAlso pLastYear > 0 Then
                    lTsB = SubsetByDateBoundary(lTs, pYearStartMonth, pYearStartDay, Nothing, pFirstYear, pLastYear, pYearEndMonth, pYearEndDay)
                Else
                    lTsB = lTs
                End If

                Dim lSeasons As New atcSeasonsYearSubset(pYearStartMonth, pYearStartDay, pYearEndMonth, pYearEndDay)
                lSeasons.SeasonSelected(0) = True
                lTsB = lSeasons.SplitBySelected(lTsB, Nothing).ItemByIndex(1)
                lTsB.Attributes.SetValue("ID", lTs.OriginalParent.Attributes.GetValue("ID"))
            End If
            lDataGroupB.Add(lTsB)
        Next

        Return lDataGroupB
    End Function

    Private Sub btnDisplayBasic_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplayBasic.Click
        Dim lList As New atcList.atcListForm

        With lList
            .Text = "Basic Statistics"
            .Initialize(SelectedData(), pBasicAttributes, False, , )
            .Width = 600
            .SwapRowsColumns = True
            .Icon = Me.Icon
        End With

        'Code section is to print out Text format of Basic Statistics, leave to remind how formatting is working to get 
        '     at the precise formatting in a SWSTAT format table
        '
        'Dim lselectedData As atcDataGroup = SelectedData()
        'Dim lfrmReport As New frmTextReport
        'lfrmReport.Title = "Basic Flow Statistics"

        'lfrmReport.ReportBody = "  DATA-                                                     NUMBER      NON-ZERO" & vbCrLf
        'lfrmReport.ReportBody &= "   SET                                        STANDARD   OF DATA VALUES RETURNS" & vbCrLf
        'lfrmReport.ReportBody &= " NUMBER    MINIMUM     MAXIMUM        MEAN   DEVIATION     USED  UNUSED CODE  NO." & vbCrLf
        'lfrmReport.ReportBody &= " ------ ---------- ----------- ----------- -----------  ------- ------- ---- ---" & vbCrLf

        'Dim lTemp As Double = 0.0
        'Dim lTSstats As New ArrayList()
        'If lselectedData.Count > 0 Then
        '    For Each aTS As atcTimeseries In lselectedData

        '        lTSstats = TSStats(aTS)

        '        lfrmReport.ReportBody &= CStr(aTS.Attributes.GetValue("ID")).PadLeft(6)
        '        lfrmReport.ReportBody &= DecimalAlignM(lTSstats(0), , 3, 6, False).PadLeft(13) 'min
        '        lfrmReport.ReportBody &= DecimalAlignM(lTSstats(1), , 3, 6, False).PadLeft(12) 'max
        '        lfrmReport.ReportBody &= DecimalAlignM(lTSstats(2), , 3, 6, False).PadLeft(12) 'mean
        '        lfrmReport.ReportBody &= DecimalAlignM(lTSstats(3), , 3, 6, False).PadLeft(12) 'lstdev
        '        lfrmReport.ReportBody &= CStr(lTSstats(4)).PadLeft(8) 'used
        '        lfrmReport.ReportBody &= CStr(lTSstats(5)).PadLeft(8) 'unused
        '        lfrmReport.ReportBody &= vbCrLf

        '    Next
        'Else
        'Logger.Msg("Select at least one time series")
        'End If

        'lfrmReport.displayReport()

    End Sub

    'Private Function TSStats(ByVal aTS As atcTimeseries) As ArrayList

    '    Dim lmean As Double
    '    Dim lmin As Double
    '    Dim lmax As Double
    '    Dim lstdev As Double
    '    Dim lUsed As Integer
    '    Dim lTotalCount As Integer
    '    Dim lSum As Double
    '    Dim lUnused As Integer
    '    Dim lSS As Double

    '    Dim lusedVals As New ArrayList()
    '    Dim lStats As New ArrayList()

    '    lmin = Double.MaxValue
    '    lmax = Double.MinValue
    '    For Each lVal As Double In aTS.Values
    '        lTotalCount += 1
    '        If lVal > 0 Then
    '            lUsed += 1
    '            lSum += lVal
    '            lusedVals.Add(lVal)
    '            If lVal > lmax Then
    '                lmax = lVal
    '            End If
    '            If lVal < lmin Then
    '                lmin = lVal
    '            End If
    '        End If
    '    Next

    '    lmean = lSum / lUsed
    '    lUnused = lTotalCount - lUsed

    '    For Each lVal As Double In lusedVals
    '        lSS += (lVal - lmean) ^ 2
    '    Next

    '    lstdev = Math.Sqrt(lSS / lUsed)

    '    lStats.Add(lmin)
    '    lStats.Add(lmax)
    '    lStats.Add(lmean)
    '    lStats.Add(lstdev)
    '    lStats.Add(lUsed)
    '    lStats.Add(lUnused)

    '    Return lStats

    'End Function

    Private Sub btnNDay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNDayList.Click
        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
        Dim lSelectedData As atcTimeseriesGroup = SelectedData()
        If lSelectedData.Count > 0 Then
            If lstNday.SelectedIndices.Count > 0 Then
                Dim lRankedAnnual As atcTimeseriesGroup =
                   clsIDFPlugin.ComputeRankedAnnualTimeseries(aTimeseriesGroup:=lSelectedData,
                                                                 aNDay:=ListToArray(lstNday), aHighFlag:=radioHigh.Checked,
                                                                 aFirstYear:=pFirstYear, aLastYear:=pLastYear,
                                                                 aBoundaryMonth:=pYearStartMonth, aBoundaryDay:=pYearStartDay,
                                                                 aEndMonth:=pYearEndMonth, aEndDay:=pYearEndDay)
                If lRankedAnnual.Count > 0 Then
                    For Each lTS As atcTimeseries In lRankedAnnual
                        lTS.Attributes.SetValue("Units", "Common")
                    Next

                    Dim lList As New atcList.atcListForm
                    With lList
                        With .DateFormat
                            .IncludeDays = False
                            .IncludeHours = False
                            .IncludeMinutes = False
                            .IncludeMonths = False
                        End With
                        .Text = "N-Day " & HighOrLowString() & " Annual Time Series and Ranking"
                        .Initialize(lRankedAnnual.Clone, pNDayAttributes, True, , )
                        .DisplayValueAttributes = True
                        .Icon = Me.Icon
                    End With
                End If
            Else
                Logger.Msg("Select at least one number of days")
            End If
        Else
            Logger.Msg("Select at least one time series")
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub

    Private Sub btnNdayAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNdayAdd.Click
        AddNday(txtNdayAdd.Text)
    End Sub

    Private Sub AddNday(ByVal aNday As String)
        If IsNumeric(aNday) Then
            Try
                Dim lIndex As Integer = 0
                Dim lNewValue As Double = CDbl(aNday)
                While lIndex < lstNday.Items.Count AndAlso CDbl(lstNday.Items(lIndex)) < lNewValue
                    lIndex += 1
                End While
                lstNday.Items.Insert(lIndex, aNday)
                lstNday.SetSelected(lIndex, True)
            Catch ex As Exception
                Logger.Dbg("Exception adding N-day '" & aNday & "': " & ex.Message)
            End Try
        Else
            Logger.Msg("Non-numeric value '" & aNday & "' could not be added to N-Day list")
        End If
    End Sub

    Private Sub SelectNday(ByVal aNday As String)
        If Not lstNday.Items.Contains(aNday) Then
            AddNday(aNday)
        End If
        For lNdayIndex As Integer = 0 To lstNday.Items.Count - 1
            If CInt(lstNday.Items(lNdayIndex)) = aNday Then
                lstNday.SelectedIndex = lNdayIndex
                Exit For
            End If
        Next
    End Sub

    Private Sub btnNdayRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNdayRemove.Click
        Dim lRemoveThese As New ArrayList
        Dim lIndex As Integer
        If lstNday.SelectedIndices.Count > 0 Then
            For lIndex = lstNday.SelectedIndices.Count - 1 To 0 Step -1
                lRemoveThese.Add(lstNday.SelectedIndices.Item(lIndex))
            Next
        Else
            For lIndex = lstNday.Items.Count - 1 To 0 Step -1
                If lstNday.Items(lIndex) = txtNdayAdd.Text Then
                    lRemoveThese.Add(lIndex)
                End If
            Next
        End If

        For Each lIndex In lRemoveThese
            lstNday.Items.RemoveAt(lIndex)
        Next
    End Sub

    Private Sub btnNdayDefault_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNdayDefault.Click
        LoadListDefaults(lstNday)
    End Sub

    Private Sub LoadListDefaults(ByVal aList As Windows.Forms.ListBox)
        Dim lDefault() As Double = clsIDFPlugin.ListDefaultArray(aList.Tag)
        If Not lDefault Is Nothing Then
            aList.Items.Clear()
            For Each lNumber As Double In lDefault
                Dim lLabel As String = Format(lNumber, "0.####")
                aList.Items.Add(lLabel)
            Next
        End If
    End Sub

    ''' <summary>Calculate(aOperationName: = "n-day "  HighOrLowString()  " value", _
    ''' aReturnPeriods() = ListToArray(lstRecurrence))
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CalculateBatch()
        If Not pSetGlobal AndAlso pDataGroup IsNot Nothing Then ClearAttributes()
        Dim lMsg As String = SeasonsYearsFromFormBatch()
        If Not String.IsNullOrEmpty(lMsg) Then
            Logger.Msg("Please address the following issues before proceeding:" & vbCrLf & vbCrLf & lMsg, MsgBoxStyle.Information, "Input Needs Correction")
            Exit Sub
        Else
            If pSetGlobal Then
                Logger.MsgCustomOwned("Parameters are set for Global Defaults.", "USGS Batch Processing", Me, New String() {"OK"})
                Me.Close()
            Else
                Dim lGroup As String = pAttributes.GetValue("Group", "")
                Logger.MsgCustomOwned("Group Parameters are set for " & lGroup, "USGS Batch Processing", Me, New String() {"OK"})
                Me.Close()
            End If
        End If
        RaiseEvent ParametersSet(pAttributes)
    End Sub

    Private Sub Calculate(ByVal aOperationName As String, ByVal aReturnPeriods() As Double)
        ClearAttributes()
        SeasonsYearsFromForm() 'setup all inputs from form
        Dim lCalculator As New atcTimeseriesNdayHighLow.atcTimeseriesNdayHighLow
        For Each lTs As atcTimeseries In pDataGroup
            lTs.Attributes.SetValueIfMissing("CalcEMA", True)
        Next
        Dim lArgs As New atcDataAttributes
        lArgs.SetValue("Timeseries", pDataGroup)
        lArgs.SetValue("NDay", ListToArray(lstNday))
        lArgs.SetValue("Return Period", aReturnPeriods)
        lArgs.SetValue("LogFlg", chkLog.Checked)
        If pYearStartMonth > 0 Then lArgs.SetValue("BoundaryMonth", pYearStartMonth)
        If pYearStartDay > 0 Then lArgs.SetValue("BoundaryDay", pYearStartDay)
        If pYearEndMonth > 0 Then lArgs.SetValue("EndMonth", pYearEndMonth)
        If pYearEndDay > 0 Then lArgs.SetValue("EndDay", pYearEndDay)
        If pFirstYear > 0 Then lArgs.SetValue("FirstYear", pFirstYear)
        If pLastYear > 0 Then lArgs.SetValue("LastYear", pLastYear)

        lCalculator.Open(aOperationName, lArgs)
        lCalculator.DataSets.Clear()
    End Sub

    Private Function SaveSettingsBatch() As String
        Dim lMsg As String = ""
        Dim lName As String = HighOrLowString()
        If pYearStartMonth = 0 OrElse pYearStartDay = 0 OrElse pYearEndMonth = 0 OrElse pYearEndDay = 0 Then
            lMsg &= "- Problem with start and/or end dates." & vbCrLf
        Else
            If pAttributes Is Nothing Then
                pAttributes = New atcDataAttributes()
            End If
            With pAttributes
                .SetValue(InputNames.StartMonth, pYearStartMonth)
                .SetValue(InputNames.StartDay, pYearStartDay)
                .SetValue(InputNames.EndMonth, pYearEndMonth)
                .SetValue(InputNames.EndDay, pYearEndDay)
                'For All years, txtOmitBeforeYear and txtOmitAfterYear are empty
                .SetValue(InputNames.IncludeYears, cboYears.SelectedItem.ToString())
                .SetValue(InputNames.StartYear, txtOmitBeforeYear.Text)
                .SetValue(InputNames.EndYear, txtOmitAfterYear.Text)
                .SetValue(InputNames.HighLow, lName)
                .SetValue(InputNames.Logarithmic, chkLog.Checked)
                .SetValue(InputNames.MultiNDayPlot, chkMultipleNDayPlots.Checked)
                .SetValue(InputNames.MultiStationPlot, chkMultipleStationPlots.Checked)
                If IO.Directory.Exists(txtOutputDir.Text) Then
                    .SetValue(InputNames.OutputDir, txtOutputDir.Text)
                Else
                    lMsg &= "- Need to specify a default output directory."
                End If
                If Not String.IsNullOrEmpty(txtOutputRootName.Text.Trim()) Then
                    .SetValue(InputNames.OutputPrefix, txtOutputRootName.Text.Trim())
                End If
            End With
        End If

        If Not pSetGlobal Then
            Dim lStationsInfo As atcCollection = InputNames.BuildStationsInfo(pDataGroup)
            pAttributes.SetValue(InputNames.StationsInfo, lStationsInfo)
        End If

        lMsg &= SaveListBatch(lstNday)
        lMsg &= SaveListBatch(lstRecurrence)

        Return lMsg
    End Function

    Private Sub SaveSettings()
        Dim lName As String = HighOrLowString()
        SaveSetting("atcFrequencyGrid", "StartMonth", lName, pYearStartMonth)
        SaveSetting("atcFrequencyGrid", "StartDay", lName, pYearStartDay)
        SaveSetting("atcFrequencyGrid", "EndMonth", lName, pYearEndMonth)
        SaveSetting("atcFrequencyGrid", "EndDay", lName, pYearEndDay)
        SaveSetting("atcFrequencyGrid", "Defaults", "HighOrLow", lName)
        SaveSetting("atcFrequencyGrid", "Defaults", "Logarithmic", chkLog.Checked.ToString)
        SaveList(lstNday)
        SaveList(lstRecurrence)
    End Sub

    Private Sub SaveList(ByVal lst As Windows.Forms.ListBox)
        SaveSetting("atcFrequencyGrid", "List." & lst.Tag, "dummy", "")
        DeleteSetting("atcFrequencyGrid", "List." & lst.Tag)
        For lIndex As Integer = 0 To lst.Items.Count - 1
            SaveSetting("atcFrequencyGrid", "List." & lst.Tag, lst.Items(lIndex), lst.SelectedIndices.Contains(lIndex))
        Next
    End Sub

    Private Function SaveListBatch(ByVal lst As Windows.Forms.ListBox) As String
        Dim lMsg As String = ""

        'Dim listing0() As Double = pAttributes.GetValue(lst.Tag, Nothing)
        'If listing0 IsNot Nothing Then
        '    ReDim listing0(0)
        '    listing0 = Nothing
        'End If
        Dim lCollection0 As atcCollection = pAttributes.GetValue(lst.Tag & "s", Nothing)
        If lCollection0 IsNot Nothing Then
            lCollection0.Clear()
            lCollection0 = Nothing
        End If
        pAttributes.RemoveByKey(lst.Tag)
        pAttributes.RemoveByKey(lst.Tag & "s")
        If lst.SelectedIndices.Count = 0 Then
            lMsg &= "- Need to select at least one " & lst.Tag & vbCrLf
            Return lMsg
        Else
            Dim listing(lst.Items.Count - 1) As Double
            Dim lCollection As New atcCollection()

            Dim lItemValue As Double
            For lIndex As Integer = 0 To lst.Items.Count - 1
                lItemValue = lst.Items(lIndex)
                listing(lIndex) = lItemValue
                lCollection.Add(lItemValue, lst.SelectedIndices.Contains(lIndex))
            Next
            pAttributes.SetValue(lst.Tag, listing)
            pAttributes.SetValue(lst.Tag & "s", lCollection)
        End If

        Return lMsg
    End Function

    Private Sub ClearAttributes()
        Dim lRemoveThese As New atcCollection
        For Each lData As atcDataSet In pDataGroup
            For Each lAttribute As atcDefinedValue In lData.Attributes
                If Not lAttribute.Arguments Is Nothing Then
                    If lAttribute.Arguments.ContainsAttribute("Nday") OrElse
                       lAttribute.Arguments.ContainsAttribute("Return Period") Then
                        lRemoveThese.Add(lAttribute)
                    End If
                End If
            Next
            For Each lAttribute As atcDefinedValue In lRemoveThese
                lData.Attributes.Remove(lAttribute)
            Next
        Next
    End Sub

    Private Sub btnNdayAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNdayAll.Click
        For index As Integer = 0 To lstNday.Items.Count - 1
            lstNday.SetSelected(index, True)
        Next
    End Sub

    Private Sub btnNdayNone_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNdayNone.Click
        For index As Integer = 0 To lstNday.Items.Count - 1
            lstNday.SetSelected(index, False)
        Next
    End Sub

    Private Sub radioHigh_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles radioHigh.CheckedChanged
        GetDefaultYearStartEnd()
    End Sub

    Private Sub radioLow_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles radioLow.CheckedChanged
        GetDefaultYearStartEnd()
    End Sub

    Private Sub GetDefaultYearStartEnd()
        If radioHigh.Checked Then
            pYearStartMonth = 10
            pYearStartDay = 1
            pYearEndMonth = 9
            pYearEndDay = 30
        Else
            pYearStartMonth = 4
            pYearStartDay = 1
            pYearEndMonth = 3
            pYearEndDay = 31
        End If

        Dim lName As String = HighOrLowString()
        pYearStartMonth = GetSetting("atcFrequencyGrid", "StartMonth", lName, pYearStartMonth)
        pYearStartDay = GetSetting("atcFrequencyGrid", "StartDay", lName, pYearStartDay)
        pYearEndMonth = GetSetting("atcFrequencyGrid", "EndMonth", lName, pYearEndMonth)
        pYearEndDay = GetSetting("atcFrequencyGrid", "EndDay", lName, pYearEndDay)
        SeasonsYearsToForm()
    End Sub

    Private Function HighOrLowString() As String
        If radioHigh.Checked Then
            Return "High"
        Else
            Return "Low"
        End If
    End Function

    Private Function MultipleNDayPlots() As Boolean
        If chkMultipleNDayPlots.Checked Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Function MultipleStationPlots() As Boolean
        If chkMultipleStationPlots.Checked Then
            Return True
        Else
            Return False
        End If

    End Function

    Private Function RunSpearmanTest(ByVal aAnnualTS As atcTimeseries) As String
        Dim lRunR As String
        Dim lPrompt As Boolean = False
        Do
            lRunR = FindFile("Locate RunR executable", "RunR.exe", aUserVerifyFileName:=lPrompt)
            If lRunR.Length = 0 Then Return "RunR.exe not found"
            lPrompt = True
        Loop While Not lRunR.ToLower.EndsWith("runr.exe") OrElse Not IO.File.Exists(lRunR)

        lPrompt = False
        Dim lRcodeFilename As String
        Do
            lRcodeFilename = FindFile("Locate R script for computing Spearman Rho Test", "fnGetTrend.R", aUserVerifyFileName:=lPrompt)
            If lRcodeFilename.Length = 0 Then Return "R script not found"
            lPrompt = True
        Loop While Not lRcodeFilename.ToUpper.EndsWith(".R") OrElse Not IO.File.Exists(lRcodeFilename)

        If IO.File.Exists(lRcodeFilename) AndAlso aAnnualTS.numValues > 0 Then
            Dim lYearsString As String = ""
            Dim lValuesString As String = ""
            Dim lDateArray(6) As Integer
            For lValueIndex As Integer = 1 To aAnnualTS.numValues
                If Not Double.IsNaN(aAnnualTS.Value(lValueIndex)) Then
                    J2Date(aAnnualTS.Dates.Value(lValueIndex), lDateArray)
                    lYearsString &= lDateArray(0) & ", "
                    lValuesString &= DoubleToString(aAnnualTS.Value(lValueIndex), 15, "0.##########", aSignificantDigits:=8) & ", "
                End If
            Next
            'Trim extra comma and space
            lYearsString = lYearsString.Substring(0, lYearsString.Length - 2)
            lValuesString = lValuesString.Substring(0, lValuesString.Length - 2)

            Dim lArgsFilename As String = GetTemporaryFileName("RunR", ".R")
            Logger.Dbg("Writing trend R function/arguments to " & lArgsFilename)
            TryCopy(lRcodeFilename, lArgsFilename)
            Dim lArgWriter As New System.IO.StreamWriter(lArgsFilename, True)
            lArgWriter.WriteLine()
            lArgWriter.WriteLine("# Arguments for this run")
            lArgWriter.WriteLine("InputYears <- c(" & lYearsString & ")")
            lArgWriter.WriteLine("InputValues <- c(" & lValuesString & ")")
            lArgWriter.WriteLine("fnGetTrend(InputYears, InputValues, dPercentConfidenceInterval = 95, intWhat = 0)")
            lArgWriter.Close()

            Dim lResultsFilename As String = GetTemporaryFileName("Rresults", ".txt")
            Logger.Dbg("Writing trend R results to " & lResultsFilename)
            LaunchProgram(lRunR, IO.Path.GetDirectoryName(lRcodeFilename), lResultsFilename & " " & lArgsFilename)
            Return IO.File.ReadAllText(lResultsFilename).TrimEnd(vbLf).TrimEnd(vbCr)
        End If
        Return "R code not found"
    End Function

    Private Sub btnDisplayTrend_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnScreeningTests.Click
        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
        Dim lSelectedData As atcTimeseriesGroup = SelectedData()
        If lSelectedData.Count > 0 Then
            If lstNday.SelectedIndices.Count > 0 Then
                Dim lRankedAnnual As atcTimeseriesGroup =
                   clsIDFPlugin.ComputeRankedAnnualTimeseries(aTimeseriesGroup:=lSelectedData,
                                                                 aNDay:=ListToArray(lstNday),
                                                                 aHighFlag:=radioHigh.Checked,
                                                                 aFirstYear:=pFirstYear,
                                                                 aLastYear:=pLastYear,
                                                                 aBoundaryMonth:=pYearStartMonth,
                                                                 aBoundaryDay:=pYearStartDay,
                                                                 aEndMonth:=pYearEndMonth,
                                                                 aEndDay:=pYearEndDay)
                If lRankedAnnual.Count > 0 Then
                    For Each lTS As atcTimeseries In lRankedAnnual
                        With lTS.Attributes
                            .SetValue("Original ID", lTS.OriginalParent.Attributes.GetValue("ID"))
                            .SetValue("From", pDateFormat.JDateToString(lTS.Dates.Value(1)))
                            .SetValue("To", pDateFormat.JDateToString(lTS.Dates.Value(lTS.numValues)))
                            .SetValue("Not Used", .GetValue("Count Missing"))
                            .SetValueIfMissing("SpearmanTest", RunSpearmanTest(lTS))
                        End With
                    Next
                    Dim lList As New atcList.atcListForm
                    With lList
                        With .DateFormat
                            .IncludeDays = False
                            .IncludeHours = False
                            .IncludeMinutes = False
                            .IncludeMonths = False
                        End With
                        .Text = "Trend of " & HighOrLowString() & " Annual Time Series and Statistics"
                        .Initialize(lRankedAnnual, pTrendAttributes, False)
                        .SwapRowsColumns = True
                        .Icon = Me.Icon
                    End With
                End If
            Else
                Logger.Msg("Select at least one number of days")
            End If
        Else
            Logger.Msg("Select at least one time series")
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default
    End Sub

    Private Sub btnDoFrequencyGrid_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFrequencyGrid.Click
        If pBatch Then
            CalculateBatch() 'setting params for batch run
        Else
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
            Calculate("n-day " & HighOrLowString() & " value", ListToArray(lstRecurrence))

            Dim lFreqForm As New frmDisplayFrequencyGrid(aDataGroup:=pDataGroup,
                                                         aHigh:=radioHigh.Checked,
                                                         aNday:=ListToArray(lstNday),
                                                         aReturns:=ListToArray(lstRecurrence))
            lFreqForm.SWSTATform = Me

            Me.Cursor = System.Windows.Forms.Cursors.Default
        End If
    End Sub

    Private Sub btnFrequencyReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFrequencyReport.Click
        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim lSaveDialog As New System.Windows.Forms.SaveFileDialog
        With lSaveDialog
            .Title = "Save Frequency Report As"
            .DefaultExt = ".txt"
            .FileName = ReplaceString(Me.Text, " ", "_") & "_report.txt"
            If FileExists(IO.Path.GetDirectoryName(.FileName), True, False) Then
                .InitialDirectory = IO.Path.GetDirectoryName(.FileName)
            End If
            If .ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                Calculate("n-day " & HighOrLowString() & " value", ListToArray(lstRecurrence))

                Dim lFreqForm As New frmDisplayFrequencyGrid(aDataGroup:=pDataGroup,
                                                             aHigh:=radioHigh.Checked,
                                                             aNday:=ListToArray(lstNday),
                                                             aReturns:=ListToArray(lstRecurrence))
                lFreqForm.Visible = False

                SaveFileString(.FileName, lFreqForm.CreateReport)
                OpenFile(.FileName)
            End If
        End With
        Me.Cursor = System.Windows.Forms.Cursors.Default
    End Sub

    Private Sub btnDoFrequencyGraph_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFrequencyGraph.Click
        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
        DoFrequencyGraph()
        Me.Cursor = System.Windows.Forms.Cursors.Default
    End Sub

    Private Function STAID(ByVal aTs As atcTimeseries) As String
        Dim lSTAID As String = aTs.Attributes.GetValue("STAID", Nothing)
        If lSTAID Is Nothing Then
            lSTAID = aTs.Attributes.GetValue("Location", Nothing)
            If lSTAID Is Nothing Then
                lSTAID = aTs.Attributes.GetValue("STANAM", Nothing)
                If lSTAID Is Nothing Then
                    lSTAID = "N/A"
                End If
            End If
            aTs.Attributes.SetValue("STAID", lSTAID)
        End If
        Return lSTAID
    End Function

    Public Sub DoFrequencyGraph(Optional ByVal aDataGroup As atcData.atcTimeseriesGroup = Nothing)
        Calculate("n-day " & HighOrLowString() & " value", clsIDFPlugin.ListDefaultArray("Return Period"))
        Dim lGraphPlugin As New atcGraph.atcGraphPlugin
        Dim lGraphForm As atcGraph.atcGraphForm
        'Dim lSeparateGraphs As Boolean = False
        'Select Case pDataGroup.Count
        '    Case 0 : Return
        '    Case 1 : lSeparateGraphs = False
        '    Case Else
        '        lSeparateGraphs = (Logger.MsgCustomCheckbox("Create separate graphs or all on one graph?", _
        '                                                    pDataGroup.Count & " datasets selected", _
        '                                                    "Do not ask again", "BASINS", "SWSTAT", "SeparateFreqGraphs", _
        '                                                    "Separate", "One Graph") = "Separate")
        'End Select

        'Get station list
        Dim lStnList As New Generic.List(Of String)
        For Each lDataSet As atcTimeseries In pDataGroup
            If Not lStnList.Contains(STAID(lDataSet)) Then
                lStnList.Add(STAID(lDataSet))
            End If
        Next
        'Get Nday list
        Dim lNDayList As New Generic.List(Of String)
        For Each lDataSet As atcTimeseries In pDataGroup
            For Each lAtt As atcData.atcDefinedValue In lDataSet.Attributes
                If lAtt.Definition.Name.ToLower.Contains("daytimeseries") Then
                    If Not lNDayList.Contains(lAtt.Definition.Name) Then
                        lNDayList.Add(lAtt.Definition.Name)
                    End If
                End If
            Next
        Next
        If MultipleNDayPlots() And MultipleStationPlots() Then
            For Each lStaId As String In lStnList
                For Each lNDayTimeseriesName As String In lNDayList
                    Dim lDataGroup As New atcData.atcTimeseriesGroup
                    For Each lDataset As atcTimeseries In pDataGroup
                        If STAID(lDataset) = lStaId Then
                            Dim lTs As atcTimeseries = lDataset.Attributes.GetValue(lNDayTimeseriesName)
                            If lTs IsNot Nothing Then
                                lDataGroup.Add(lTs)
                            End If
                        End If
                    Next
                    If lDataGroup.Count > 0 Then
                        lGraphForm = lGraphPlugin.Show(lDataGroup, "Frequency")
                        lGraphForm.Icon = Me.Icon
                    End If
                    lDataGroup.Clear()
                    lDataGroup = Nothing
                Next
            Next

            'For Each lDataSet As atcTimeseries In pDataGroup
            '    lGraphForm = lGraphPlugin.Show(New atcTimeseriesGroup(lDataSet), "Frequency")
            '    lGraphForm.Icon = Me.Icon
            'Next
        ElseIf MultipleNDayPlots() Then
            For Each lNDayTimeseriesName As String In lNDayList
                Dim lDataGroup As New atcData.atcTimeseriesGroup
                For Each lDataset As atcTimeseries In pDataGroup
                    Dim lTs As atcTimeseries = lDataset.Attributes.GetValue(lNDayTimeseriesName)
                    If lTs IsNot Nothing Then
                        lDataGroup.Add(lTs)
                    End If
                Next
                If lDataGroup IsNot Nothing AndAlso lDataGroup.Count > 0 Then
                    lGraphForm = lGraphPlugin.Show(lDataGroup, "Frequency")
                    lGraphForm.Icon = Me.Icon
                End If
                lDataGroup.Clear()
                lDataGroup = Nothing
            Next
        ElseIf MultipleStationPlots() Then
            Dim lDataGroup As atcData.atcTimeseriesGroup = Nothing
            For Each lStaId As String In lStnList
                lDataGroup = pDataGroup.FindData("STAID", lStaId)
                If lDataGroup IsNot Nothing AndAlso lDataGroup.Count > 0 Then
                    lGraphForm = lGraphPlugin.Show(lDataGroup, "Frequency")
                    lGraphForm.Icon = Me.Icon
                End If
                lDataGroup.Clear()
                lDataGroup = Nothing
            Next
        Else
            lGraphForm = lGraphPlugin.Show(pDataGroup, "Frequency")
            lGraphForm.Icon = Me.Icon
        End If

        lNDayList.Clear()
        lNDayList = Nothing
        lStnList.Clear()
        lStnList = Nothing
    End Sub

    Private Sub cboYears_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboYears.SelectedIndexChanged
        Select Case cboYears.SelectedIndex
            Case 0 'All
                ShowCustomYears(False)
                txtOmitBeforeYear.Text = ""
                txtOmitAfterYear.Text = ""
            Case 1 'Common
                ShowCustomYears(False)
                If cboYears.Text.EndsWith(pNoDatesInCommon) Then
                    cboYears.SelectedIndex = 0
                Else
                    Dim lCurDate(5) As Integer
                    J2Date(pCommonStart, lCurDate)
                    txtOmitBeforeYear.Text = Format(lCurDate(0), "0000")
                    J2Date(pCommonEnd, lCurDate)
                    txtOmitAfterYear.Text = Format(lCurDate(0), "0000")
                End If
            Case 2 'Custom
                ShowCustomYears(True)
        End Select
    End Sub

    Private Sub btnRecurrenceAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRecurrenceAdd.Click
        If IsNumeric(txtRecurrenceAdd.Text) Then
            Try
                Dim lIndex As Integer = 0
                Dim lNewValue As Double = CDbl(txtRecurrenceAdd.Text)
                While lIndex < lstRecurrence.Items.Count AndAlso CDbl(lstRecurrence.Items(lIndex)) < lNewValue
                    lIndex += 1
                End While
                lstRecurrence.Items.Insert(lIndex, txtRecurrenceAdd.Text)
                lstRecurrence.SetSelected(lIndex, True)
            Catch ex As Exception
                Logger.Dbg("Exception adding Recurrence '" & txtRecurrenceAdd.Text & "': " & ex.Message)
            End Try
        Else
            Logger.Msg("Type a return period to add in the blank, then press the add button again")
        End If
    End Sub

    Private Sub btnRecurrenceRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRecurrenceRemove.Click
        Dim lRemoveThese As New ArrayList
        Dim lIndex As Integer
        If lstRecurrence.SelectedIndices.Count > 0 Then
            For lIndex = lstRecurrence.SelectedIndices.Count - 1 To 0 Step -1
                lRemoveThese.Add(lstRecurrence.SelectedIndices.Item(lIndex))
            Next
        Else
            For lIndex = lstRecurrence.Items.Count - 1 To 0 Step -1
                If lstRecurrence.Items(lIndex) = txtRecurrenceAdd.Text Then
                    lRemoveThese.Add(lIndex)
                End If
            Next
        End If

        For Each lIndex In lRemoveThese
            lstRecurrence.Items.RemoveAt(lIndex)
        Next

    End Sub

    Private Sub btnRecurrenceDefault_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRecurrenceDefault.Click
        LoadListDefaults(lstRecurrence)
    End Sub

    Private Sub btnRecurrenceAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRecurrenceAll.Click
        For index As Integer = 0 To lstRecurrence.Items.Count - 1
            lstRecurrence.SetSelected(index, True)
        Next
    End Sub

    Private Sub btnRecurrenceNone_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRecurrenceNone.Click
        For index As Integer = 0 To lstRecurrence.Items.Count - 1
            lstRecurrence.SetSelected(index, False)
        Next
    End Sub

#Region "DFLOW"

    Private Sub chkBioCustom_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkBioCustom.CheckedChanged
        Dim i As Integer

        DFLOWCalcs.fBioDefault = chkBioCustom.Checked

        txtBioFlowDays.Enabled = Not DFLOWCalcs.fBioDefault
        txtBioYearsBetween.Enabled = Not DFLOWCalcs.fBioDefault
        txtBioExcursionDays.Enabled = Not DFLOWCalcs.fBioDefault
        txtBioExcursionsPerCluster.Enabled = Not DFLOWCalcs.fBioDefault
        lblBioFlowDays.Enabled = Not DFLOWCalcs.fBioDefault
        lblBioYearsBetween.Enabled = Not DFLOWCalcs.fBioDefault
        lblBioExcursionDays.Enabled = Not DFLOWCalcs.fBioDefault
        lblBioExcursionsPerCluster.Enabled = Not DFLOWCalcs.fBioDefault

        If DFLOWCalcs.fBioDefault Then
            txtBioFlowDays.Text = DFLOWCalcs.fBioFPArray(DFLOWCalcs.fBioType, 0)
            txtBioYearsBetween.Text = DFLOWCalcs.fBioFPArray(DFLOWCalcs.fBioType, 1)
            txtBioExcursionDays.Text = DFLOWCalcs.fBioFPArray(DFLOWCalcs.fBioType, 2)
            txtBioExcursionsPerCluster.Text = DFLOWCalcs.fBioFPArray(DFLOWCalcs.fBioType, 3)
        Else
            For i = 0 To 3
                If DFLOWCalcs.fBioFPArray(3, i) < 0 Then
                    DFLOWCalcs.fBioFPArray(3, i) = DFLOWCalcs.fBioFPArray(DFLOWCalcs.fBioType, i)
                End If
            Next
            txtBioFlowDays.Text = DFLOWCalcs.fBioFPArray(3, 0)
            txtBioYearsBetween.Text = DFLOWCalcs.fBioFPArray(3, 1)
            txtBioExcursionDays.Text = DFLOWCalcs.fBioFPArray(3, 2)
            txtBioExcursionsPerCluster.Text = DFLOWCalcs.fBioFPArray(3, 3)
        End If

        'If pBatch AndAlso pAttributes IsNot Nothing Then
        '    If DFLOWCalcs.fBioDefault Then
        '        Dim lParams() As Integer = Nothing
        '        If rbBio1.Checked Then
        '            lParams = pAttributes.GetValue(DFLOWAnalysis.InputNames.EBioDFlowType.Acute_maximum_concentration.ToString, Nothing)
        '        ElseIf rbBio2.Checked Then
        '            lParams = pAttributes.GetValue(DFLOWAnalysis.InputNames.EBioDFlowType.Chronic_continuous_concentration.ToString, Nothing)
        '        ElseIf rbBio3.Checked Then
        '            lParams = pAttributes.GetValue(DFLOWAnalysis.InputNames.EBioDFlowType.Ammonia.ToString, Nothing)
        '        End If
        '        If lParams IsNot Nothing Then
        '            lParams(0) = DFLOWCalcs.fBioPeriod
        '            lParams(1) = DFLOWCalcs.fBioYears
        '            lParams(2) = DFLOWCalcs.fBioCluster
        '            lParams(3) = DFLOWCalcs.fBioExcursions
        '        End If
        '    End If
        'End If
    End Sub

    Private Sub chkBio_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _
            chkBioAcute.CheckedChanged, chkBioChronic.CheckedChanged, chkBioAmmonia.CheckedChanged
        If chkBioAcute.Checked Then
            DFLOWCalcs.fBioType = 0
        ElseIf chkBioChronic.Checked Then
            DFLOWCalcs.fBioType = 1
        ElseIf chkBioAmmonia.Checked Then
            DFLOWCalcs.fBioType = 2
        Else
            Exit Sub
        End If

        If pBatch AndAlso pAttributes IsNot Nothing Then
            Dim lParams() As Integer = Nothing
            If chkBioAcute.Checked Then
                lParams = pAttributes.GetValue(DFLOWAnalysis.InputNames.EBioDFlowType.Acute_maximum_concentration.ToString())
                pAttributes.SetValue(DFLOWAnalysis.InputNames.BioSelectedMethod, DFLOWAnalysis.InputNames.EBioDFlowType.Acute_maximum_concentration)
            ElseIf chkBioChronic.Checked Then
                lParams = pAttributes.GetValue(DFLOWAnalysis.InputNames.EBioDFlowType.Chronic_continuous_concentration.ToString())
                pAttributes.SetValue(DFLOWAnalysis.InputNames.BioSelectedMethod, DFLOWAnalysis.InputNames.EBioDFlowType.Chronic_continuous_concentration)
            ElseIf chkBioAmmonia.Checked Then
                lParams = pAttributes.GetValue(DFLOWAnalysis.InputNames.EBioDFlowType.Ammonia.ToString())
                pAttributes.SetValue(DFLOWAnalysis.InputNames.BioSelectedMethod, DFLOWAnalysis.InputNames.EBioDFlowType.Ammonia)
            End If
            If lParams IsNot Nothing Then
                txtBioFlowDays.Text = lParams(0).ToString()
                txtBioYearsBetween.Text = lParams(1).ToString()
                txtBioExcursionDays.Text = lParams(2).ToString()
                txtBioExcursionsPerCluster.Text = lParams(3).ToString()
            End If
        Else
            txtBioFlowDays.Text = DFLOWCalcs.fBioFPArray(DFLOWCalcs.fBioType, 0)
            txtBioYearsBetween.Text = DFLOWCalcs.fBioFPArray(DFLOWCalcs.fBioType, 1)
            txtBioExcursionDays.Text = DFLOWCalcs.fBioFPArray(DFLOWCalcs.fBioType, 2)
            txtBioExcursionsPerCluster.Text = DFLOWCalcs.fBioFPArray(DFLOWCalcs.fBioType, 3)
        End If
    End Sub

    ' Boolean flag used to determine when a character other than a number is entered.
    Private nonNumberEntered As Boolean = False

    ' Handle the KeyDown event to determine the type of character entered into the control.
    Private Sub textBox_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) _
         Handles txtBioFlowDays.KeyDown, txtBioYearsBetween.KeyDown, txtBioExcursionDays.KeyDown, txtBioExcursionsPerCluster.KeyDown, txtNonBioCustomNday1.KeyDown, tbNonBio2.KeyDown, txtNonBioFlowPercentile.KeyDown
        ' Initialize the flag to false.
        nonNumberEntered = False

        ' Determine whether the keystroke is a number from the top of the keyboard.
        If e.KeyCode < Keys.D0 OrElse e.KeyCode > Keys.D9 Then
            ' Determine whether the keystroke is a number from the keypad.
            If e.KeyCode < Keys.NumPad0 OrElse e.KeyCode > Keys.NumPad9 Then
                ' Determine whether the keystroke is a backspace.
                If e.KeyCode <> Keys.Back Then
                    ' A non-numerical keystroke was pressed. 
                    ' Set the flag to true and evaluate in KeyPress event.
                    nonNumberEntered = True
                End If
            End If
        End If
    End Sub 'textBox1_KeyDown
    Private Sub textBox2_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtNonBioExplicitFlow.KeyDown
        ' Initialize the flag to false.
        nonNumberEntered = False

        ' Determine whether the keystroke is a number from the top of the keyboard.
        If e.KeyCode = Keys.OemPeriod And InStr(sender.text, ".") = 1 Then
            If e.KeyCode < Keys.D0 OrElse e.KeyCode > Keys.D9 Then
                ' Determine whether the keystroke is a number from the keypad.
                If e.KeyCode < Keys.NumPad0 OrElse e.KeyCode > Keys.NumPad9 Then
                    ' Determine whether the keystroke is a backspace.
                    If e.KeyCode <> Keys.Back Then
                        ' A non-numerical keystroke was pressed. 
                        ' Set the flag to true and evaluate in KeyPress event.
                        nonNumberEntered = True
                    End If
                End If
            End If
        End If
    End Sub

    ' This event occurs after the KeyDown event and can be used 
    ' to prevent characters from entering the control.
    Private Sub textBox_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) _
        Handles txtBioFlowDays.KeyPress, txtBioYearsBetween.KeyPress, txtBioExcursionDays.KeyPress, txtBioExcursionsPerCluster.KeyPress, txtNonBioCustomNday1.KeyPress, tbNonBio2.KeyPress, txtNonBioFlowPercentile.KeyPress, txtNonBioExplicitFlow.KeyPress

        ' Check for the flag being set in the KeyDown event.
        If nonNumberEntered = True Then
            ' Stop the character from being entered into the control since it is non-numerical.
            e.Handled = True
        End If
    End Sub 'textBox1_KeyPress

    Private Sub chkNonBioAcute_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkNonBioAcute.CheckedChanged, chkNonBioChronic.CheckedChanged

        DFLOWCalcs.fNonBioType.Clear()

        If chkNonBioAcute.Checked Then
            DFLOWCalcs.fNonBioType.Add(0)
        End If
        If chkNonBioChronic.Checked Then
            DFLOWCalcs.fNonBioType.Add(1)
        End If
        If chkNonBioExplicitFlow.Checked Then
            DFLOWCalcs.fNonBioType.Add(2)
        End If
        'Label7.Enabled = chkNonBioAcute.Checked
        Label8.Enabled = chkNonBioAcute.Checked
        txtNonBioCustomNday1.Enabled = chkNonBioAcute.Checked
        tbNonBio2.Enabled = chkNonBioAcute.Checked

        lblNonBioExplicitFlowUnits.Enabled = chkNonBioChronic.Checked
        txtNonBioExplicitFlow.Enabled = chkNonBioChronic.Checked

        lblNonBioFlowPercentileUnits.Enabled = chkNonBioFlowPercentile.Checked
        txtNonBioFlowPercentile.Enabled = chkNonBioFlowPercentile.Checked

        If pBatch AndAlso pAttributes IsNot Nothing Then
            If chkNonBioAcute.Checked Then
                pAttributes.SetValue(DFLOWAnalysis.InputNames.NBioSelectedMethod, DFLOWAnalysis.InputNames.EDFlowType.Hydrological)
                'Dim lParams() As Integer = pAttributes.GetValue(InputNames.EDFlowType.Hydrological.ToString(), Nothing)
                'If lParams IsNot Nothing AndAlso lParams.Length = 2 Then
                '    tbNonBio1.Text = lParams(0).ToString()
                '    tbNonBio2.Text = lParams(1).ToString()
                'End If
            ElseIf chkNonBioChronic.Checked Then
                pAttributes.SetValue(DFLOWAnalysis.InputNames.NBioSelectedMethod, DFLOWAnalysis.InputNames.EDFlowType.Explicit_Flow_Value)
                'Dim lflowval As Double = pAttributes.GetValue(InputNames.EDFlowType.Explicit_Flow_Value.ToString(), 1.0)
                'tbNonBio3.Text = lflowval.ToString()
            ElseIf chkNonBioFlowPercentile.Checked Then
                pAttributes.SetValue(DFLOWAnalysis.InputNames.NBioSelectedMethod, DFLOWAnalysis.InputNames.EDFlowType.Flow_Percentile)
                'Dim lflowPct As Double = pAttributes.GetValue(InputNames.EDFlowType.Flow_Percentile.ToString(), 0.1)
                'tbNonBio4.Text = lflowPct.ToString()
            End If
        Else
            If chkNonBioAcute.Checked Then
                If pAttributes IsNot Nothing Then pAttributes.SetValue(DFLOWAnalysis.InputNames.NBioSelectedMethod, DFLOWAnalysis.InputNames.EDFlowType.Hydrological)
                Dim lAvgPeriod As Integer
                Dim lReturnPeriod As Integer
                If pAttributes IsNot Nothing AndAlso pAttributes.ContainsAttribute(DFLOWAnalysis.InputNames.EDFlowType.Hydrological.ToString()) Then
                    Dim lParams() As Integer = pAttributes.GetValue(DFLOWAnalysis.InputNames.EDFlowType.Hydrological.ToString(), Nothing)
                    If lParams IsNot Nothing AndAlso lParams.Length = 2 Then
                        lAvgPeriod = lParams(0)
                        lReturnPeriod = lParams(1)
                    Else
                        lAvgPeriod = DFLOWAnalysis.DFLOWCalcs.fAveragingPeriod
                        lReturnPeriod = DFLOWAnalysis.DFLOWCalcs.fReturnPeriod
                    End If
                Else
                    lAvgPeriod = DFLOWAnalysis.DFLOWCalcs.fAveragingPeriod
                    lReturnPeriod = DFLOWAnalysis.DFLOWCalcs.fReturnPeriod
                End If
                txtNonBioCustomNday1.Text = lAvgPeriod.ToString()
                tbNonBio2.Text = lReturnPeriod.ToString()
            ElseIf chkNonBioChronic.Checked Then
                If pAttributes IsNot Nothing Then pAttributes.SetValue(DFLOWAnalysis.InputNames.NBioSelectedMethod, DFLOWAnalysis.InputNames.EDFlowType.Explicit_Flow_Value)
                Dim lflowval As Double
                If pAttributes IsNot Nothing AndAlso pAttributes.ContainsAttribute(DFLOWAnalysis.InputNames.EDFlowType.Explicit_Flow_Value.ToString()) Then
                    lflowval = pAttributes.GetValue(DFLOWAnalysis.InputNames.EDFlowType.Explicit_Flow_Value.ToString())
                Else
                    lflowval = DFLOWAnalysis.DFLOWCalcs.fExplicitFlow
                End If
                txtNonBioExplicitFlow.Text = lflowval.ToString()
            ElseIf chkNonBioFlowPercentile.Checked Then
                If pAttributes IsNot Nothing Then pAttributes.SetValue(DFLOWAnalysis.InputNames.NBioSelectedMethod, DFLOWAnalysis.InputNames.EDFlowType.Flow_Percentile)
                Dim lflowPct As Double
                If pAttributes IsNot Nothing AndAlso pAttributes.ContainsAttribute(DFLOWAnalysis.InputNames.EDFlowType.Flow_Percentile.ToString()) Then
                    lflowPct = pAttributes.GetValue(DFLOWAnalysis.InputNames.EDFlowType.Flow_Percentile.ToString())
                Else
                    lflowPct = DFLOWAnalysis.DFLOWCalcs.fPercentile
                End If
                txtNonBioFlowPercentile.Text = lflowPct.ToString()
            End If
        End If
    End Sub

    Private Sub tbNonBio1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtNonBioCustomNday1.TextChanged
        If IsNumeric(txtNonBioCustomNday1.Text) AndAlso Integer.TryParse(txtNonBioCustomNday1.Text, DFLOWCalcs.fAveragingPeriod) Then
            Dim lParams() As Integer = GetSavedParams(False)
            If lParams IsNot Nothing AndAlso lParams.Length = 2 Then
                lParams(0) = DFLOWCalcs.fAveragingPeriod
            End If
        End If
    End Sub
    Private Sub tbNonBio2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbNonBio2.TextChanged
        If IsNumeric(tbNonBio2.Text) AndAlso Integer.TryParse(tbNonBio2.Text, DFLOWCalcs.fReturnPeriod) Then
            Dim lParams() As Integer = GetSavedParams(False)
            If lParams IsNot Nothing AndAlso lParams.Length = 2 Then
                lParams(1) = DFLOWCalcs.fReturnPeriod
            End If
        End If
    End Sub
    Private Sub tbNonBio3_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtNonBioExplicitFlow.TextChanged
        If IsNumeric(txtNonBioExplicitFlow.Text) AndAlso Double.TryParse(txtNonBioExplicitFlow.Text, DFLOWCalcs.fExplicitFlow) AndAlso DFLOWCalcs.fExplicitFlow > 0 Then
            Dim lNewValue As Double = GetSavedParams(False, DFLOWCalcs.fExplicitFlow)
        End If
    End Sub
    Private Sub tbNonBio4_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtNonBioFlowPercentile.TextChanged
        If IsNumeric(txtNonBioFlowPercentile.Text) AndAlso Double.TryParse(txtNonBioFlowPercentile.Text, DFLOWCalcs.fPercentile) AndAlso DFLOWCalcs.fPercentile > 0 Then
            Dim lNewValue As Double = GetSavedParams(False, DFLOWCalcs.fPercentile)
        End If
    End Sub

    Private Sub tbBio1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBioFlowDays.TextChanged
        If IsNumeric(txtBioFlowDays.Text) AndAlso Integer.TryParse(txtBioFlowDays.Text, DFLOWCalcs.fBioFPArray(3, 0)) Then
            'fBioFPArray(3, 0) = tbBio1.Text
            Dim lParams() As Integer = GetSavedParams()
            If lParams IsNot Nothing AndAlso lParams.Length = 4 Then
                lParams(DFLOWAnalysis.InputNames.EBioDFlowParamIndex.P0FlowAveragingPeriodDays) = Integer.Parse(txtBioFlowDays.Text)
                pAttributes.SetValue(DFLOWAnalysis.InputNames.BioUseDefault, False)
            End If
        End If
    End Sub
    Private Sub tbBio2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBioYearsBetween.TextChanged
        If IsNumeric(txtBioYearsBetween.Text) AndAlso Integer.TryParse(txtBioYearsBetween.Text, DFLOWCalcs.fBioFPArray(3, 1)) Then
            'fBioFPArray(3, 1) = tbBio2.Text
            Dim lParams() As Integer = GetSavedParams()
            If lParams IsNot Nothing AndAlso lParams.Length = 4 Then
                lParams(DFLOWAnalysis.InputNames.EBioDFlowParamIndex.P1AverageYearsBetweenExcursions) = Integer.Parse(txtBioYearsBetween.Text)
                pAttributes.SetValue(DFLOWAnalysis.InputNames.BioUseDefault, False)
            End If
        End If
    End Sub
    Private Sub tbBio3_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBioExcursionDays.TextChanged
        If IsNumeric(txtBioExcursionDays.Text) AndAlso Integer.TryParse(txtBioExcursionDays.Text, DFLOWCalcs.fBioFPArray(3, 2)) Then
            'fBioFPArray(3, 2) = tbBio3.Text
            Dim lParams() As Integer = GetSavedParams()
            If lParams IsNot Nothing AndAlso lParams.Length = 4 Then
                lParams(DFLOWAnalysis.InputNames.EBioDFlowParamIndex.P2ExcursionClusterPeriodDays) = Integer.Parse(txtBioExcursionDays.Text)
                pAttributes.SetValue(DFLOWAnalysis.InputNames.BioUseDefault, False)
            End If
        End If
    End Sub
    Private Sub tbBio4_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBioExcursionsPerCluster.TextChanged
        If IsNumeric(txtBioExcursionsPerCluster.Text) AndAlso Integer.TryParse(txtBioExcursionsPerCluster.Text, DFLOWCalcs.fBioFPArray(3, 3)) Then
            'fBioFPArray(3, 3) = tbBio4.Text
            Dim lParams() As Integer = GetSavedParams()
            If lParams IsNot Nothing AndAlso lParams.Length = 4 Then
                lParams(DFLOWAnalysis.InputNames.EBioDFlowParamIndex.P3AverageExcursionsPerCluster) = Integer.Parse(txtBioExcursionsPerCluster.Text)
                pAttributes.SetValue(DFLOWAnalysis.InputNames.BioUseDefault, False)
            End If
        End If
    End Sub

    Private Function GetSavedParams(Optional ByVal aBio As Boolean = True, Optional ByVal aValue As Double = -99) As Object
        If pAttributes IsNot Nothing Then
            If aBio Then
                If chkBioAcute.Checked Then
                    Return pAttributes.GetValue(DFLOWAnalysis.InputNames.EBioDFlowType.Acute_maximum_concentration.ToString(), Nothing)
                ElseIf chkBioChronic.Checked Then
                    Return pAttributes.GetValue(DFLOWAnalysis.InputNames.EBioDFlowType.Chronic_continuous_concentration.ToString(), Nothing)
                ElseIf chkBioAmmonia.Checked Then
                    Return pAttributes.GetValue(DFLOWAnalysis.InputNames.EBioDFlowType.Ammonia.ToString(), Nothing)
                End If
            Else
                If chkNonBioAcute.Checked Then
                    Return pAttributes.GetValue(DFLOWAnalysis.InputNames.EDFlowType.Hydrological.ToString(), Nothing)
                ElseIf chkNonBioChronic.Checked Then
                    If aValue > 0 Then
                        pAttributes.SetValue(DFLOWAnalysis.InputNames.EDFlowType.Explicit_Flow_Value.ToString(), aValue)
                    End If
                    Return pAttributes.GetValue(DFLOWAnalysis.InputNames.EDFlowType.Explicit_Flow_Value.ToString(), Nothing)
                ElseIf chkNonBioFlowPercentile.Checked Then
                    If aValue > 0 Then
                        pAttributes.SetValue(DFLOWAnalysis.InputNames.EDFlowType.Flow_Percentile.ToString(), aValue)
                    End If
                    Return pAttributes.GetValue(DFLOWAnalysis.InputNames.EDFlowType.Flow_Percentile.ToString(), Nothing)
                End If
            End If
        Else
            Return Nothing
        End If
        Return Nothing
    End Function

    Friend Sub DFLOWCalcs_Initialize(Optional ByVal aChoice As atcDataAttributes = Nothing)
        If aChoice Is Nothing Then
            ' This sets the initial values for DFLOW calculations - CMC, 7Q10
            DFLOWCalcs.fBioDefault = True
            DFLOWCalcs.fBioType = 0
            DFLOWCalcs.fBioPeriod = 1
            DFLOWCalcs.fBioYears = 3
            DFLOWCalcs.fBioCluster = 120
            DFLOWCalcs.fBioExcursions = 5

            DFLOWCalcs.fNonBioType.Clear()
            DFLOWCalcs.fNonBioType.Add(0)
            DFLOWCalcs.fAveragingPeriod = 7
            DFLOWCalcs.fReturnPeriod = 10
            DFLOWCalcs.fExplicitFlow = 1.0
            DFLOWCalcs.fPercentile = 0.1

            aChoice = New atcDataAttributes()
            With aChoice
                .SetValue(DFLOWAnalysis.InputNames.EBioDFlowType.Acute_maximum_concentration.ToString(), DFLOWCalcs.GetBioDefaultParams(DFLOWAnalysis.InputNames.EBioDFlowType.Acute_maximum_concentration))
                .SetValue(DFLOWAnalysis.InputNames.EBioDFlowType.Chronic_continuous_concentration.ToString(), DFLOWCalcs.GetBioDefaultParams(DFLOWAnalysis.InputNames.EBioDFlowType.Chronic_continuous_concentration))
                .SetValue(DFLOWAnalysis.InputNames.EBioDFlowType.Ammonia.ToString(), DFLOWCalcs.GetBioDefaultParams(DFLOWAnalysis.InputNames.EBioDFlowType.Ammonia))
                .SetValue(DFLOWAnalysis.InputNames.EDFlowType.Hydrological.ToString(), New Integer() {7, 10})
                .SetValue(DFLOWAnalysis.InputNames.EDFlowType.Explicit_Flow_Value.ToString(), 1.0)
                .SetValue(DFLOWAnalysis.InputNames.EDFlowType.Flow_Percentile.ToString(), 0.1)
            End With
        Else
            With aChoice
                DFLOWCalcs.fBioDefault = .GetValue(DFLOWAnalysis.InputNames.BioUseDefault, True)
                DFLOWCalcs.fBioType = .GetValue(DFLOWAnalysis.InputNames.BioSelectedMethod, DFLOWAnalysis.InputNames.EBioDFlowType.Acute_maximum_concentration)
                Dim lBio4Params() As Integer '= DFLOWCalcs.GetBioDefaultParams(DFLOWCalcs.fBioType)
                If DFLOWCalcs.fBioDefault Then
                    lBio4Params = DFLOWCalcs.GetBioDefaultParams(DFLOWCalcs.fBioType)
                Else
                    Select Case DFLOWCalcs.fBioType
                        Case DFLOWAnalysis.InputNames.EBioDFlowType.Acute_maximum_concentration
                            lBio4Params = .GetValue(DFLOWAnalysis.InputNames.EBioDFlowType.Acute_maximum_concentration.ToString(), DFLOWCalcs.GetBioDefaultParams(DFLOWCalcs.fBioType))
                        Case DFLOWAnalysis.InputNames.EBioDFlowType.Chronic_continuous_concentration
                            lBio4Params = .GetValue(DFLOWAnalysis.InputNames.EBioDFlowType.Chronic_continuous_concentration.ToString(), DFLOWCalcs.GetBioDefaultParams(DFLOWCalcs.fBioType))
                        Case DFLOWAnalysis.InputNames.EBioDFlowType.Ammonia
                            lBio4Params = .GetValue(DFLOWAnalysis.InputNames.EBioDFlowType.Ammonia.ToString(), DFLOWCalcs.GetBioDefaultParams(DFLOWCalcs.fBioType))
                    End Select
                End If
                DFLOWCalcs.fBioPeriod = lBio4Params(0)
                DFLOWCalcs.fBioYears = lBio4Params(1)
                DFLOWCalcs.fBioCluster = lBio4Params(2)
                DFLOWCalcs.fBioExcursions = lBio4Params(3)

                DFLOWCalcs.fNonBioType = .GetValue(DFLOWAnalysis.InputNames.NBioSelectedMethod, DFLOWAnalysis.InputNames.EDFlowType.Hydrological)
                Dim lHydro2Params() As Integer = .GetValue(DFLOWAnalysis.InputNames.EDFlowType.Hydrological.ToString(), New Integer() {7, 10})
                DFLOWCalcs.fAveragingPeriod = lHydro2Params(0)
                DFLOWCalcs.fReturnPeriod = lHydro2Params(1)

                DFLOWCalcs.fExplicitFlow = .GetValue(DFLOWAnalysis.InputNames.EDFlowType.Explicit_Flow_Value.ToString(), 1.0)
                DFLOWCalcs.fPercentile = .GetValue(DFLOWAnalysis.InputNames.EDFlowType.Flow_Percentile.ToString(), 0.1)
            End With
        End If
    End Sub

    Private Sub PopulateDFLOW(ByVal aTimeseriesGroup As atcTimeseriesGroup)
        ' This populates the DFLOW input form according to the DFLOW calculation values
        chkBioCustom.Checked = DFLOWCalcs.fBioDefault
        'rbBio1.Enabled = DFLOWCalcs.fBioDefault
        'rbBio2.Enabled = DFLOWCalcs.fBioDefault
        'rbBio3.Enabled = DFLOWCalcs.fBioDefault

        Select Case DFLOWCalcs.fBioType
            Case 0
                chkBioAcute.Checked = True
            Case 1
                chkBioChronic.Checked = True
            Case 2
                chkBioAmmonia.Checked = True
        End Select

        txtBioFlowDays.Enabled = Not DFLOWCalcs.fBioDefault
        txtBioYearsBetween.Enabled = Not DFLOWCalcs.fBioDefault
        txtBioExcursionDays.Enabled = Not DFLOWCalcs.fBioDefault

        Dim lBioIdx As Integer
        If DFLOWCalcs.fBioDefault Then
            lBioIdx = DFLOWCalcs.fBioType
        Else
            lBioIdx = 3
        End If

        RemoveHandler txtBioFlowDays.TextChanged, AddressOf tbBio1_TextChanged
        RemoveHandler txtBioYearsBetween.TextChanged, AddressOf tbBio2_TextChanged
        RemoveHandler txtBioExcursionDays.TextChanged, AddressOf tbBio3_TextChanged
        RemoveHandler txtBioExcursionsPerCluster.TextChanged, AddressOf tbBio4_TextChanged

        RemoveHandler txtNonBioCustomNday1.TextChanged, AddressOf tbNonBio1_TextChanged
        RemoveHandler tbNonBio2.TextChanged, AddressOf tbNonBio2_TextChanged
        RemoveHandler txtNonBioExplicitFlow.TextChanged, AddressOf tbNonBio3_TextChanged
        RemoveHandler txtNonBioFlowPercentile.TextChanged, AddressOf tbNonBio4_TextChanged


        If DFLOWCalcs.fBioFPArray(lBioIdx, 0) < 0 Then
            If pBatch Then
                txtBioFlowDays.Text = DFLOWCalcs.fBioPeriod.ToString()
            Else
                txtBioFlowDays.Text = ""
            End If
        Else
            If pBatch Then
                txtBioFlowDays.Text = DFLOWCalcs.fBioPeriod.ToString()
            Else
                txtBioFlowDays.Text = DFLOWCalcs.fBioFPArray(lBioIdx, 0)
            End If
        End If

        If DFLOWCalcs.fBioFPArray(lBioIdx, 1) < 0 Then
            If pBatch Then
                txtBioYearsBetween.Text = DFLOWCalcs.fBioYears.ToString()
            Else
                txtBioYearsBetween.Text = ""
            End If
        Else
            If pBatch Then
                txtBioYearsBetween.Text = DFLOWCalcs.fBioYears.ToString()
            Else
                txtBioYearsBetween.Text = DFLOWCalcs.fBioFPArray(lBioIdx, 1)
            End If
        End If

        If DFLOWCalcs.fBioFPArray(lBioIdx, 2) < 0 Then
            If pBatch Then
                txtBioExcursionDays.Text = DFLOWCalcs.fBioExcursions.ToString()
            Else
                txtBioExcursionDays.Text = ""
            End If
        Else
            If pBatch Then
                txtBioExcursionDays.Text = DFLOWCalcs.fBioExcursions.ToString()
            Else
                txtBioExcursionDays.Text = DFLOWCalcs.fBioFPArray(lBioIdx, 2)
            End If
        End If

        If DFLOWCalcs.fBioFPArray(lBioIdx, 3) < 0 Then
            If pBatch Then
                txtBioExcursionsPerCluster.Text = DFLOWCalcs.fBioCluster.ToString()
            Else
                txtBioExcursionsPerCluster.Text = ""
            End If
        Else
            If pBatch Then
                txtBioExcursionsPerCluster.Text = DFLOWCalcs.fBioCluster.ToString()
            Else
                txtBioExcursionsPerCluster.Text = DFLOWCalcs.fBioFPArray(lBioIdx, 3)
            End If
        End If

        For Each lNonBioType As Integer In DFLOWCalcs.fNonBioType
            Select Case lNonBioType
                Case 0
                    chkNonBioAcute.Checked = True
                Case 1
                    chkNonBioChronic.Checked = True
                Case 2
                    chkNonBioFlowPercentile.Checked = True
            End Select
        Next

        txtNonBioCustomNday1.Text = DFLOWCalcs.fAveragingPeriod
        tbNonBio2.Text = DFLOWCalcs.fReturnPeriod
        txtNonBioExplicitFlow.Text = DFLOWCalcs.fExplicitFlow
        txtNonBioFlowPercentile.Text = DFLOWCalcs.fPercentile

        AddHandler txtBioFlowDays.TextChanged, AddressOf tbBio1_TextChanged
        AddHandler txtBioYearsBetween.TextChanged, AddressOf tbBio2_TextChanged
        AddHandler txtBioExcursionDays.TextChanged, AddressOf tbBio3_TextChanged
        AddHandler txtBioExcursionsPerCluster.TextChanged, AddressOf tbBio4_TextChanged

        AddHandler txtNonBioCustomNday1.TextChanged, AddressOf tbNonBio1_TextChanged
        AddHandler tbNonBio2.TextChanged, AddressOf tbNonBio2_TextChanged
        AddHandler txtNonBioExplicitFlow.TextChanged, AddressOf tbNonBio3_TextChanged
        AddHandler txtNonBioFlowPercentile.TextChanged, AddressOf tbNonBio4_TextChanged

        'clbDataSets.Items.Clear()

        Dim lDateFormat As atcDateFormat
        lDateFormat = New atcDateFormat
        With lDateFormat
            .IncludeHours = False
            .IncludeMinutes = False
            .IncludeSeconds = False
        End With

        If aTimeseriesGroup IsNot Nothing AndAlso aTimeseriesGroup.Count > 0 Then
            'Dim lDataSet As atcDataSet
            'For Each lDataSet In aTimeseriesGroup
            '    Dim lString As String
            '    lString = lDataSet.Attributes.GetFormattedValue("Location") & " (" & _
            '              lDateFormat.JDateToString(lDataSet.Attributes.GetValue("start date")) & " - " & _
            '              lDateFormat.JDateToString(lDataSet.Attributes.GetValue("end date")) & ")"

            '    clbDataSets.Items.Add(lString, True)
            'Next
        End If
    End Sub

    Private Sub btnCalculate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCalculate.Click

        DFLOWCalcs.fBioPeriod = DFLOWCalcs.fBioFPArray(DFLOWCalcs.fBioType, 0)
        DFLOWCalcs.fBioYears = DFLOWCalcs.fBioFPArray(DFLOWCalcs.fBioType, 1)
        DFLOWCalcs.fBioCluster = DFLOWCalcs.fBioFPArray(DFLOWCalcs.fBioType, 2)
        DFLOWCalcs.fBioExcursions = DFLOWCalcs.fBioFPArray(DFLOWCalcs.fBioType, 3)
        Dim lfrmResult As New DFLOWAnalysis.frmDFLOWResults(pDataGroup, , True)
    End Sub

#End Region '"DFLOW"
End Class

