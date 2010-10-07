Imports atcData
Imports atcUtility
Imports MapWinUtility
Imports System.Windows.Forms

Friend Class frmTrend
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()
        InitializeComponent() 'required by Windows Form Designer
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

        LoadListSettingsOrDefaults(lstNday)
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

        For Each lDataset As atcData.atcTimeseries In pDataGroup
            If lDataset.Dates.numValues > 0 Then
                Dim lThisDate As Double = lDataset.Dates.Value(1)
                If lThisDate < lFirstDate Then lFirstDate = lThisDate
                If lThisDate > pCommonStart Then pCommonStart = lThisDate
                lThisDate = lDataset.Dates.Value(lDataset.Dates.numValues)
                If lThisDate > lLastDate Then lLastDate = lThisDate
                If lThisDate < pCommonEnd Then pCommonEnd = lThisDate
            End If
        Next
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
                MapWinUtility.Logger.Dbg("Error retrieving saved settings: " & e.Message)
            End Try
        Else
            LoadListDefaults(lst)
        End If
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
    Friend WithEvents btnNDay As System.Windows.Forms.Button
    Friend WithEvents btnDisplayBasic As System.Windows.Forms.Button
    Friend WithEvents grpNday As System.Windows.Forms.GroupBox
    Friend WithEvents btnNdayDefault As System.Windows.Forms.Button
    Friend WithEvents btnNdayRemove As System.Windows.Forms.Button
    Friend WithEvents btnNdayAdd As System.Windows.Forms.Button
    Friend WithEvents txtNdayAdd As System.Windows.Forms.TextBox
    Friend WithEvents btnNdayNone As System.Windows.Forms.Button
    Friend WithEvents btnNdayAll As System.Windows.Forms.Button
    Friend WithEvents lstNday As System.Windows.Forms.ListBox
    Friend WithEvents grpHighLow As System.Windows.Forms.GroupBox
    Friend WithEvents radioHigh As System.Windows.Forms.RadioButton
    Friend WithEvents radioLow As System.Windows.Forms.RadioButton
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
    Friend WithEvents btnDisplayTrend As System.Windows.Forms.Button
    Friend WithEvents chkLowValue As System.Windows.Forms.CheckBox
    Friend WithEvents grpLimits As System.Windows.Forms.GroupBox
    Friend WithEvents txtHighValue As System.Windows.Forms.TextBox
    Friend WithEvents chkHighValue As System.Windows.Forms.CheckBox
    Friend WithEvents txtLowValue As System.Windows.Forms.TextBox
    Friend WithEvents mnuHelp As System.Windows.Forms.MenuItem
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmTrend))
        Me.MainMenu1 = New System.Windows.Forms.MainMenu(Me.components)
        Me.mnuFile = New System.Windows.Forms.MenuItem
        Me.mnuFileSelectData = New System.Windows.Forms.MenuItem
        Me.mnuAnalysis = New System.Windows.Forms.MenuItem
        Me.mnuHelp = New System.Windows.Forms.MenuItem
        Me.btnDisplayBasic = New System.Windows.Forms.Button
        Me.btnNDay = New System.Windows.Forms.Button
        Me.grpNday = New System.Windows.Forms.GroupBox
        Me.btnNdayDefault = New System.Windows.Forms.Button
        Me.btnNdayRemove = New System.Windows.Forms.Button
        Me.btnNdayAdd = New System.Windows.Forms.Button
        Me.txtNdayAdd = New System.Windows.Forms.TextBox
        Me.btnNdayNone = New System.Windows.Forms.Button
        Me.btnNdayAll = New System.Windows.Forms.Button
        Me.lstNday = New System.Windows.Forms.ListBox
        Me.grpHighLow = New System.Windows.Forms.GroupBox
        Me.radioHigh = New System.Windows.Forms.RadioButton
        Me.radioLow = New System.Windows.Forms.RadioButton
        Me.grpDates = New System.Windows.Forms.GroupBox
        Me.cboStartMonth = New System.Windows.Forms.ComboBox
        Me.lblYearStart = New System.Windows.Forms.Label
        Me.txtEndDay = New System.Windows.Forms.TextBox
        Me.txtStartDay = New System.Windows.Forms.TextBox
        Me.cboEndMonth = New System.Windows.Forms.ComboBox
        Me.lblYearEnd = New System.Windows.Forms.Label
        Me.grpYears = New System.Windows.Forms.GroupBox
        Me.lblDataStart = New System.Windows.Forms.Label
        Me.lblDataEnd = New System.Windows.Forms.Label
        Me.lblOmitBefore = New System.Windows.Forms.Label
        Me.lblOmitAfter = New System.Windows.Forms.Label
        Me.txtOmitAfterYear = New System.Windows.Forms.TextBox
        Me.txtOmitBeforeYear = New System.Windows.Forms.TextBox
        Me.cboYears = New System.Windows.Forms.ComboBox
        Me.btnDisplayTrend = New System.Windows.Forms.Button
        Me.chkLowValue = New System.Windows.Forms.CheckBox
        Me.grpLimits = New System.Windows.Forms.GroupBox
        Me.txtHighValue = New System.Windows.Forms.TextBox
        Me.chkHighValue = New System.Windows.Forms.CheckBox
        Me.txtLowValue = New System.Windows.Forms.TextBox
        Me.grpNday.SuspendLayout()
        Me.grpHighLow.SuspendLayout()
        Me.grpDates.SuspendLayout()
        Me.grpYears.SuspendLayout()
        Me.grpLimits.SuspendLayout()
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
        'btnDisplayBasic
        '
        Me.btnDisplayBasic.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnDisplayBasic.Location = New System.Drawing.Point(12, 347)
        Me.btnDisplayBasic.Name = "btnDisplayBasic"
        Me.btnDisplayBasic.Size = New System.Drawing.Size(157, 23)
        Me.btnDisplayBasic.TabIndex = 10
        Me.btnDisplayBasic.Text = "Display Basic Statistics"
        Me.btnDisplayBasic.UseVisualStyleBackColor = True
        '
        'btnNDay
        '
        Me.btnNDay.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnNDay.Location = New System.Drawing.Point(175, 347)
        Me.btnNDay.Name = "btnNDay"
        Me.btnNDay.Size = New System.Drawing.Size(124, 23)
        Me.btnNDay.TabIndex = 34
        Me.btnNDay.Text = "N-Day Timeseries List"
        Me.btnNDay.UseVisualStyleBackColor = True
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
        Me.grpNday.ForeColor = System.Drawing.SystemColors.ControlText
        Me.grpNday.Location = New System.Drawing.Point(268, 12)
        Me.grpNday.Name = "grpNday"
        Me.grpNday.Size = New System.Drawing.Size(200, 329)
        Me.grpNday.TabIndex = 77
        Me.grpNday.TabStop = False
        Me.grpNday.Text = "Number of Days"
        '
        'btnNdayDefault
        '
        Me.btnNdayDefault.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnNdayDefault.Location = New System.Drawing.Point(138, 270)
        Me.btnNdayDefault.Name = "btnNdayDefault"
        Me.btnNdayDefault.Size = New System.Drawing.Size(56, 20)
        Me.btnNdayDefault.TabIndex = 24
        Me.btnNdayDefault.Text = "Default"
        '
        'btnNdayRemove
        '
        Me.btnNdayRemove.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnNdayRemove.Location = New System.Drawing.Point(105, 270)
        Me.btnNdayRemove.Name = "btnNdayRemove"
        Me.btnNdayRemove.Size = New System.Drawing.Size(27, 20)
        Me.btnNdayRemove.TabIndex = 23
        Me.btnNdayRemove.Text = "-"
        '
        'btnNdayAdd
        '
        Me.btnNdayAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnNdayAdd.Location = New System.Drawing.Point(72, 270)
        Me.btnNdayAdd.Name = "btnNdayAdd"
        Me.btnNdayAdd.Size = New System.Drawing.Size(27, 20)
        Me.btnNdayAdd.TabIndex = 22
        Me.btnNdayAdd.Text = "+"
        '
        'txtNdayAdd
        '
        Me.txtNdayAdd.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtNdayAdd.Location = New System.Drawing.Point(6, 270)
        Me.txtNdayAdd.Name = "txtNdayAdd"
        Me.txtNdayAdd.Size = New System.Drawing.Size(54, 20)
        Me.txtNdayAdd.TabIndex = 21
        '
        'btnNdayNone
        '
        Me.btnNdayNone.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnNdayNone.Location = New System.Drawing.Point(130, 299)
        Me.btnNdayNone.Name = "btnNdayNone"
        Me.btnNdayNone.Size = New System.Drawing.Size(64, 23)
        Me.btnNdayNone.TabIndex = 26
        Me.btnNdayNone.Text = "None"
        '
        'btnNdayAll
        '
        Me.btnNdayAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnNdayAll.Location = New System.Drawing.Point(6, 299)
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
        Me.lstNday.Size = New System.Drawing.Size(188, 245)
        Me.lstNday.TabIndex = 20
        Me.lstNday.Tag = "NDay"
        '
        'grpHighLow
        '
        Me.grpHighLow.Controls.Add(Me.radioHigh)
        Me.grpHighLow.Controls.Add(Me.radioLow)
        Me.grpHighLow.ForeColor = System.Drawing.SystemColors.ControlText
        Me.grpHighLow.Location = New System.Drawing.Point(12, 12)
        Me.grpHighLow.Name = "grpHighLow"
        Me.grpHighLow.Size = New System.Drawing.Size(250, 65)
        Me.grpHighLow.TabIndex = 76
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
        Me.grpDates.Location = New System.Drawing.Point(12, 83)
        Me.grpDates.Name = "grpDates"
        Me.grpDates.Size = New System.Drawing.Size(250, 73)
        Me.grpDates.TabIndex = 75
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
        Me.grpYears.Location = New System.Drawing.Point(12, 162)
        Me.grpYears.Name = "grpYears"
        Me.grpYears.Size = New System.Drawing.Size(250, 102)
        Me.grpYears.TabIndex = 74
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
        'btnDisplayTrend
        '
        Me.btnDisplayTrend.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnDisplayTrend.Location = New System.Drawing.Point(305, 347)
        Me.btnDisplayTrend.Name = "btnDisplayTrend"
        Me.btnDisplayTrend.Size = New System.Drawing.Size(124, 23)
        Me.btnDisplayTrend.TabIndex = 78
        Me.btnDisplayTrend.Text = "Trend List"
        Me.btnDisplayTrend.UseVisualStyleBackColor = True
        '
        'chkLowValue
        '
        Me.chkLowValue.AutoSize = True
        Me.chkLowValue.Location = New System.Drawing.Point(6, 19)
        Me.chkLowValue.Name = "chkLowValue"
        Me.chkLowValue.Size = New System.Drawing.Size(123, 17)
        Me.chkLowValue.TabIndex = 79
        Me.chkLowValue.Text = "Ignore Values Below"
        Me.chkLowValue.UseVisualStyleBackColor = True
        '
        'grpLimits
        '
        Me.grpLimits.Controls.Add(Me.txtHighValue)
        Me.grpLimits.Controls.Add(Me.chkHighValue)
        Me.grpLimits.Controls.Add(Me.txtLowValue)
        Me.grpLimits.Controls.Add(Me.chkLowValue)
        Me.grpLimits.Location = New System.Drawing.Point(12, 270)
        Me.grpLimits.Name = "grpLimits"
        Me.grpLimits.Size = New System.Drawing.Size(250, 71)
        Me.grpLimits.TabIndex = 80
        Me.grpLimits.TabStop = False
        Me.grpLimits.Text = "Annual Value Limits"
        '
        'txtHighValue
        '
        Me.txtHighValue.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtHighValue.Location = New System.Drawing.Point(183, 43)
        Me.txtHighValue.Name = "txtHighValue"
        Me.txtHighValue.Size = New System.Drawing.Size(61, 20)
        Me.txtHighValue.TabIndex = 82
        Me.txtHighValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'chkHighValue
        '
        Me.chkHighValue.AutoSize = True
        Me.chkHighValue.Location = New System.Drawing.Point(6, 45)
        Me.chkHighValue.Name = "chkHighValue"
        Me.chkHighValue.Size = New System.Drawing.Size(125, 17)
        Me.chkHighValue.TabIndex = 81
        Me.chkHighValue.Text = "Ignore Values Above"
        Me.chkHighValue.UseVisualStyleBackColor = True
        '
        'txtLowValue
        '
        Me.txtLowValue.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtLowValue.Location = New System.Drawing.Point(183, 17)
        Me.txtLowValue.Name = "txtLowValue"
        Me.txtLowValue.Size = New System.Drawing.Size(61, 20)
        Me.txtLowValue.TabIndex = 80
        Me.txtLowValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'frmTrend
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(474, 382)
        Me.Controls.Add(Me.grpLimits)
        Me.Controls.Add(Me.btnDisplayBasic)
        Me.Controls.Add(Me.btnDisplayTrend)
        Me.Controls.Add(Me.grpNday)
        Me.Controls.Add(Me.grpHighLow)
        Me.Controls.Add(Me.btnNDay)
        Me.Controls.Add(Me.grpDates)
        Me.Controls.Add(Me.grpYears)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Menu = Me.MainMenu1
        Me.Name = "frmTrend"
        Me.Text = "Trend"
        Me.grpNday.ResumeLayout(False)
        Me.grpNday.PerformLayout()
        Me.grpHighLow.ResumeLayout(False)
        Me.grpHighLow.PerformLayout()
        Me.grpDates.ResumeLayout(False)
        Me.grpDates.PerformLayout()
        Me.grpYears.ResumeLayout(False)
        Me.grpYears.PerformLayout()
        Me.grpLimits.ResumeLayout(False)
        Me.grpLimits.PerformLayout()
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

    Private pDataAttributes As ArrayList
    Private pBasicAttributes As ArrayList
    Private pNDayAttributes As ArrayList
    Private pTrendAttributes As ArrayList

    Private pYearStartMonth As Integer = 0
    Private pYearStartDay As Integer = 0
    Private pYearEndMonth As Integer = 0
    Private pYearEndDay As Integer = 0
    Private pFirstYear As Integer = 0
    Private pLastYear As Integer = 0

    Private pCommonStart As Double = GetMinValue()
    Private pCommonEnd As Double = GetMaxValue()

    Private Const pNoDatesInCommon As String = ": No dates in common"

    'TODO: Get correct help location
    Private pHelpLocation As String = "BASINS Details\Analysis.html"
    Private Sub mnuHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuHelp.Click
        ShowHelp(pHelpLocation)
    End Sub

    Private Sub frmTrend_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyValue = Windows.Forms.Keys.F1 Then
            ShowHelp(pHelpLocation)
        End If
    End Sub

    Public Sub Initialize(Optional ByVal aTimeseriesGroup As atcData.atcTimeseriesGroup = Nothing, _
                          Optional ByVal aBasicAttributes As ArrayList = Nothing, _
                          Optional ByVal aNDayAttributes As ArrayList = Nothing, _
                          Optional ByVal aTrendAttributes As ArrayList = Nothing, _
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
                If lMenuText.StartsWith("Analysis::") Then lMenuText = lMenuText.Substring(10)
                mnuAnalysis.MenuItems.Add(lMenuText, New EventHandler(AddressOf mnuAnalysis_Click))
            Next
        End If

        If pDataGroup.Count = 0 Then 'ask user to specify some timeseries
            pDataGroup = atcDataManager.UserSelectData("Select Data for Trend Analysis", pDataGroup)
        End If

        If pDataGroup.Count > 0 Then
            PopulateForm()
            If aShowForm Then Me.Show()
        Else 'user declined to specify timeseries
            Me.Close()
        End If

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
        atcDataManager.ShowDisplay(sender.Text, pDataGroup)
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
        atcDataManager.UserSelectData("Select Data for Trend Analysis", pDataGroup, , False)
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

    Private Sub SeasonsYearsFromForm()
        pYearStartMonth = cboStartMonth.SelectedIndex + 1
        pYearEndMonth = cboEndMonth.SelectedIndex + 1
        If IsNumeric(txtStartDay.Text) Then
            pYearStartDay = txtStartDay.Text
        Else
            pYearStartDay = 0
        End If
        If IsNumeric(txtEndDay.Text) Then
            pYearEndDay = txtEndDay.Text
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
    Private Function ListToArray(ByVal aList As System.Windows.Forms.ListBox) As Double()
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
        End With

    End Sub

    Private Function TSStats(ByVal aTS As atcTimeseries) As ArrayList

        Dim lmean As Double
        Dim lmin As Double
        Dim lmax As Double
        Dim lstdev As Double
        Dim lUsed As Integer
        Dim lTotalCount As Integer
        Dim lSum As Double
        Dim lUnused As Integer
        Dim lSS As Double

        Dim lusedVals As New ArrayList()
        Dim lStats As New ArrayList()

        lmin = Double.MaxValue
        lmax = Double.MinValue
        For Each lVal As Double In aTS.Values
            lTotalCount += 1
            If lVal > 0 Then
                lUsed += 1
                lSum += lVal
                lusedVals.Add(lVal)
                If lVal > lmax Then
                    lmax = lVal
                End If
                If lVal < lmin Then
                    lmin = lVal
                End If
            End If
        Next

        lmean = lSum / lUsed
        lUnused = lTotalCount - lUsed

        For Each lVal As Double In lusedVals
            lSS += (lVal - lmean) ^ 2
        Next

        lstdev = Math.Sqrt(lSS / lUsed)

        lStats.Add(lmin)
        lStats.Add(lmax)
        lStats.Add(lmean)
        lStats.Add(lstdev)
        lStats.Add(lUsed)
        lStats.Add(lUnused)

        Return lStats

    End Function

    Private Sub btnNDay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNDay.Click
        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
        Dim lSelectedData As atcTimeseriesGroup = SelectedData()
        If lSelectedData.Count > 0 Then
            If lstNday.SelectedIndices.Count > 0 Then
                Dim lRankedAnnual As atcTimeseriesGroup = _
                   clsSWSTATPlugin.ComputeRankedAnnualTimeseries(aTimeseriesGroup:=lSelectedData, _
                                                                 aNDay:=ListToArray(lstNday), aHighFlag:=radioHigh.Checked, _
                                                                 aFirstYear:=pFirstYear, aLastYear:=pLastYear, _
                                                                 aBoundaryMonth:=pYearStartMonth, aBoundaryDay:=pYearStartDay, _
                                                                 aEndMonth:=pYearEndMonth, aEndDay:=pYearEndDay)
                If lRankedAnnual.Count > 0 Then
                    Dim lDisplayThese As atcTimeseriesGroup = GroupWithinLimits(lRankedAnnual)
                    Dim lList As New atcList.atcListForm
                    With lList.DateFormat
                        .IncludeDays = False
                        .IncludeHours = False
                        .IncludeMinutes = False
                        .IncludeMonths = False
                    End With
                    lList.Text = "N-Day " & HighOrLowString() & " Annual Time Series and Ranking"
                    lList.Initialize(lDisplayThese, pNDayAttributes, True, , )
                    lList.DisplayValueAttributes = True
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
        If IsNumeric(txtNdayAdd.Text) Then
            Try
                Dim lIndex As Integer = 0
                Dim lNewValue As Double = CDbl(txtNdayAdd.Text)
                While lIndex < lstNday.Items.Count AndAlso CDbl(lstNday.Items(lIndex)) < lNewValue
                    lIndex += 1
                End While
                lstNday.Items.Insert(lIndex, txtNdayAdd.Text)
                lstNday.SetSelected(lIndex, True)
            Catch ex As Exception
                Logger.Dbg("Exception adding N-day '" & txtNdayAdd.Text & "': " & ex.Message)
            End Try
        Else
            Logger.Msg("Type a number of days to add in the blank, then press the add button again")
        End If
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

    Private Sub LoadListDefaults(ByVal lst As Windows.Forms.ListBox)
        Dim lCalculator As New atcTimeseriesNdayHighLow.atcTimeseriesNdayHighLow
        Dim lNDayHi As atcDefinedValue = lCalculator.AvailableOperations.GetDefinedValue("n-day high value")
        Dim lArgs As atcDataAttributes = lNDayHi.Arguments
        Dim lArgName As String = lst.Tag
        Dim lDefault As Object = lArgs.GetDefinedValue(lArgName).Definition.DefaultValue
        If Not lDefault Is Nothing AndAlso IsArray(lDefault) Then
            lst.Items.Clear()
            For Each lNumber As Double In lDefault
                Dim lLabel As String = Format(lNumber, "0.####")
                lst.Items.Add(lLabel)
            Next
        End If
    End Sub

    Private Sub SaveSettings()
        Dim lName As String = HighOrLowString()
        SaveSetting("atcFrequencyGrid", "StartMonth", lName, pYearStartMonth)
        SaveSetting("atcFrequencyGrid", "StartDay", lName, pYearStartDay)
        SaveSetting("atcFrequencyGrid", "EndMonth", lName, pYearEndMonth)
        SaveSetting("atcFrequencyGrid", "EndDay", lName, pYearEndDay)
        SaveSetting("atcFrequencyGrid", "Defaults", "HighOrLow", lName)
        SaveList(lstNday)
    End Sub

    Private Sub SaveList(ByVal lst As Windows.Forms.ListBox)
        SaveSetting("atcFrequencyGrid", "List." & lst.Tag, "dummy", "")
        DeleteSetting("atcFrequencyGrid", "List." & lst.Tag)
        For lIndex As Integer = 0 To lst.Items.Count - 1
            SaveSetting("atcFrequencyGrid", "List." & lst.Tag, lst.Items(lIndex), lst.SelectedIndices.Contains(lIndex))
        Next
    End Sub

    Private Sub ClearAttributes()
        Dim lRemoveThese As New atcCollection
        For Each lData As atcDataSet In pDataGroup
            For Each lAttribute As atcDefinedValue In lData.Attributes
                If Not lAttribute.Arguments Is Nothing Then
                    If lAttribute.Arguments.ContainsAttribute("Nday") OrElse _
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

    Private Sub btnDisplayTrend_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplayTrend.Click
        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim lHiLow As New atcTimeseriesNdayHighLow.atcTimeseriesNdayHighLow
        Dim lArgs As New atcDataAttributes

        Dim lSelectedData As atcTimeseriesGroup = SelectedData()

        If lSelectedData.Count > 0 Then
            lArgs.Add("Timeseries", lSelectedData)

            If lstNday.SelectedIndices.Count > 0 Then
                lArgs.SetValue("NDay", ListToArray(lstNday))
                If HighOrLowString() = "High" Then
                    lArgs.SetValue("HighFlag", radioHigh.Checked)
                    lArgs.SetValue("LowFlag", False)
                ElseIf HighOrLowString() = "Low" Then
                    lArgs.SetValue("LowFlag", radioLow.Checked)
                    lArgs.SetValue("HighFlag", False)
                End If

                If lHiLow.Open("n-day high timeseries", lArgs) Then
                    Dim lDisplayThese As atcTimeseriesGroup = GroupWithinLimits(lHiLow.DataSets)
                    If lDisplayThese IsNot Nothing AndAlso lDisplayThese.Count > 0 Then
                        For Each lTS As atcTimeseries In lDisplayThese
                            With lTS.Attributes
                                .SetValue("Original ID", lTS.OriginalParent.Attributes.GetValue("ID"))
                                .SetValue("From", pDateFormat.JDateToString(lTS.Dates.Value(1)))
                                .SetValue("To", pDateFormat.JDateToString(lTS.Dates.Value(lTS.numValues)))
                                .SetValue("Not Used", .GetValue("Count Missing"))
                            End With
                        Next

                        Dim lList As New atcList.atcListForm
                        With lList.DateFormat
                            .IncludeDays = False
                            .IncludeHours = False
                            .IncludeMinutes = False
                            .IncludeMonths = False
                        End With
                        lList.Text = "Trend of " & HighOrLowString() & " Annual Time Series and Statistics"

                        If chkLowValue.Checked AndAlso Not pTrendAttributes.Contains("Limit Low") Then pTrendAttributes.Add("Limit Low")
                        If chkHighValue.Checked AndAlso Not pTrendAttributes.Contains("Limit High") Then pTrendAttributes.Add("Limit High")
                        lList.Initialize(lDisplayThese, pTrendAttributes, False)
                        lList.SwapRowsColumns = True
                    End If
                End If
            Else
                Logger.Msg("Select at least one number of days from the N-Day List")
            End If
        Else
            Logger.Msg("Select at least one time series")
        End If

ExitSub:
        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub

    Private Function GroupWithinLimits(ByVal aGroup As atcTimeseriesGroup) As atcTimeseriesGroup
        Dim lGroupWithinLimits As New atcTimeseriesGroup
        'Remove annual values above and/or below limits before displaying
        If chkHighValue.Checked OrElse chkLowValue.Checked Then
            Dim lHighValue As Double = GetMaxValue()
            Dim lLowValue As Double = GetMinValue()
            Dim lHaveLowLimit As Boolean = chkLowValue.Checked
            Dim lHaveHighLimit As Boolean = chkHighValue.Checked
            If lHaveLowLimit Then
                If Not Double.TryParse(txtLowValue.Text, lLowValue) Then
                    Logger.Msg("Non-numeric " & chkLowValue.Text & " = '" & txtLowValue.Text & "'")
                    Return Nothing
                End If
            End If
            If lHaveHighLimit Then
                If Not Double.TryParse(txtHighValue.Text, lHighValue) Then
                    Logger.Msg("Non-numeric " & chkHighValue.Text & " = '" & txtHighValue.Text & "'")
                    Return Nothing
                End If
            End If

            For Each lTS As atcTimeseries In aGroup
                Dim lNumNotUsed As Integer = 0
                Dim lTsKeptValues As atcTimeseries = lTS.Clone
                Dim lNaN As Double = GetNaN()
                For lIndex As Integer = 1 To lTS.numValues
                    If (lHaveHighLimit AndAlso lTsKeptValues.Value(lIndex) > lHighValue) OrElse _
                       (lHaveLowLimit AndAlso lTsKeptValues.Value(lIndex) < lLowValue) Then
                        lTsKeptValues.Value(lIndex) = lNaN
                        lNumNotUsed += 1
                    End If
                Next
                lTsKeptValues.Attributes.DiscardCalculated()
                If lHaveLowLimit Then lTsKeptValues.Attributes.SetValue("Limit Low", lLowValue)
                If lHaveHighLimit Then lTsKeptValues.Attributes.SetValue("Limit High", lHighValue)
                lGroupWithinLimits.Add(lTsKeptValues)
            Next
        Else 'no limits to enforce, return original group
            lGroupWithinLimits = aGroup
        End If
        Return lGroupWithinLimits
    End Function

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

    Private Sub txtHighValue_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtHighValue.TextChanged
        chkHighValue.Checked = IsNumeric(txtHighValue.Text)
    End Sub
    Private Sub txtLowValue_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtLowValue.TextChanged
        chkLowValue.Checked = IsNumeric(txtLowValue.Text)
    End Sub
End Class

