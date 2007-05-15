Imports atcData
Imports atcTimeseriesNdayHighLow
Imports atcUtility
Imports MapWinUtility

Public Class frmSpecifyFrequency
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

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
    Friend WithEvents panelTop As System.Windows.Forms.Panel
    Friend WithEvents panelBottom As System.Windows.Forms.Panel
    Friend WithEvents Splitter1 As System.Windows.Forms.Splitter
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents grpRecurrence As System.Windows.Forms.GroupBox
    Friend WithEvents btnRecurrenceNone As System.Windows.Forms.Button
    Friend WithEvents btnRecurrenceAll As System.Windows.Forms.Button
    Friend WithEvents lstRecurrence As System.Windows.Forms.ListBox
    Friend WithEvents grpNday As System.Windows.Forms.GroupBox
    Friend WithEvents btnNdayNone As System.Windows.Forms.Button
    Friend WithEvents btnNdayAll As System.Windows.Forms.Button
    Friend WithEvents lstNday As System.Windows.Forms.ListBox
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents txtNdayAdd As System.Windows.Forms.TextBox
    Friend WithEvents btnNdayAdd As System.Windows.Forms.Button
    Friend WithEvents btnRecurrenceAdd As System.Windows.Forms.Button
    Friend WithEvents btnSelectYearsSeasons As System.Windows.Forms.Button
    Friend WithEvents radioLow As System.Windows.Forms.RadioButton
    Friend WithEvents radioHigh As System.Windows.Forms.RadioButton
    Friend WithEvents chkKeepNDayTSers As System.Windows.Forms.CheckBox
    Friend WithEvents chkLog As System.Windows.Forms.CheckBox
    Friend WithEvents lblYearsSeasons As System.Windows.Forms.Label
    Friend WithEvents txtRecurrenceAdd As System.Windows.Forms.TextBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSpecifyFrequency))
        Me.panelTop = New System.Windows.Forms.Panel
        Me.grpRecurrence = New System.Windows.Forms.GroupBox
        Me.lstRecurrence = New System.Windows.Forms.ListBox
        Me.btnRecurrenceAdd = New System.Windows.Forms.Button
        Me.txtRecurrenceAdd = New System.Windows.Forms.TextBox
        Me.btnRecurrenceNone = New System.Windows.Forms.Button
        Me.btnRecurrenceAll = New System.Windows.Forms.Button
        Me.Splitter1 = New System.Windows.Forms.Splitter
        Me.grpNday = New System.Windows.Forms.GroupBox
        Me.btnNdayAdd = New System.Windows.Forms.Button
        Me.txtNdayAdd = New System.Windows.Forms.TextBox
        Me.btnNdayNone = New System.Windows.Forms.Button
        Me.btnNdayAll = New System.Windows.Forms.Button
        Me.lstNday = New System.Windows.Forms.ListBox
        Me.panelBottom = New System.Windows.Forms.Panel
        Me.chkKeepNDayTSers = New System.Windows.Forms.CheckBox
        Me.radioLow = New System.Windows.Forms.RadioButton
        Me.radioHigh = New System.Windows.Forms.RadioButton
        Me.btnSelectYearsSeasons = New System.Windows.Forms.Button
        Me.btnOk = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.chkLog = New System.Windows.Forms.CheckBox
        Me.lblYearsSeasons = New System.Windows.Forms.Label
        Me.panelTop.SuspendLayout()
        Me.grpRecurrence.SuspendLayout()
        Me.grpNday.SuspendLayout()
        Me.panelBottom.SuspendLayout()
        Me.SuspendLayout()
        '
        'panelTop
        '
        Me.panelTop.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.panelTop.Controls.Add(Me.grpRecurrence)
        Me.panelTop.Controls.Add(Me.Splitter1)
        Me.panelTop.Controls.Add(Me.grpNday)
        Me.panelTop.Location = New System.Drawing.Point(0, 0)
        Me.panelTop.Name = "panelTop"
        Me.panelTop.Size = New System.Drawing.Size(447, 502)
        Me.panelTop.TabIndex = 14
        '
        'grpRecurrence
        '
        Me.grpRecurrence.Controls.Add(Me.lstRecurrence)
        Me.grpRecurrence.Controls.Add(Me.btnRecurrenceAdd)
        Me.grpRecurrence.Controls.Add(Me.txtRecurrenceAdd)
        Me.grpRecurrence.Controls.Add(Me.btnRecurrenceNone)
        Me.grpRecurrence.Controls.Add(Me.btnRecurrenceAll)
        Me.grpRecurrence.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grpRecurrence.Location = New System.Drawing.Point(208, 0)
        Me.grpRecurrence.Name = "grpRecurrence"
        Me.grpRecurrence.Size = New System.Drawing.Size(239, 502)
        Me.grpRecurrence.TabIndex = 7
        Me.grpRecurrence.TabStop = False
        Me.grpRecurrence.Text = "Recurrence Interval"
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
        Me.lstRecurrence.Size = New System.Drawing.Size(220, 418)
        Me.lstRecurrence.TabIndex = 8
        Me.lstRecurrence.Tag = "Return Period"
        '
        'btnRecurrenceAdd
        '
        Me.btnRecurrenceAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRecurrenceAdd.Location = New System.Drawing.Point(162, 443)
        Me.btnRecurrenceAdd.Name = "btnRecurrenceAdd"
        Me.btnRecurrenceAdd.Size = New System.Drawing.Size(64, 24)
        Me.btnRecurrenceAdd.TabIndex = 10
        Me.btnRecurrenceAdd.Text = "Add"
        '
        'txtRecurrenceAdd
        '
        Me.txtRecurrenceAdd.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtRecurrenceAdd.Location = New System.Drawing.Point(8, 446)
        Me.txtRecurrenceAdd.Name = "txtRecurrenceAdd"
        Me.txtRecurrenceAdd.Size = New System.Drawing.Size(146, 20)
        Me.txtRecurrenceAdd.TabIndex = 9
        '
        'btnRecurrenceNone
        '
        Me.btnRecurrenceNone.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRecurrenceNone.Location = New System.Drawing.Point(162, 473)
        Me.btnRecurrenceNone.Name = "btnRecurrenceNone"
        Me.btnRecurrenceNone.Size = New System.Drawing.Size(64, 24)
        Me.btnRecurrenceNone.TabIndex = 12
        Me.btnRecurrenceNone.Text = "None"
        '
        'btnRecurrenceAll
        '
        Me.btnRecurrenceAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnRecurrenceAll.Location = New System.Drawing.Point(8, 472)
        Me.btnRecurrenceAll.Name = "btnRecurrenceAll"
        Me.btnRecurrenceAll.Size = New System.Drawing.Size(64, 24)
        Me.btnRecurrenceAll.TabIndex = 11
        Me.btnRecurrenceAll.Text = "All"
        '
        'Splitter1
        '
        Me.Splitter1.BackColor = System.Drawing.SystemColors.Control
        Me.Splitter1.Location = New System.Drawing.Point(200, 0)
        Me.Splitter1.Name = "Splitter1"
        Me.Splitter1.Size = New System.Drawing.Size(8, 502)
        Me.Splitter1.TabIndex = 13
        Me.Splitter1.TabStop = False
        '
        'grpNday
        '
        Me.grpNday.Controls.Add(Me.btnNdayAdd)
        Me.grpNday.Controls.Add(Me.txtNdayAdd)
        Me.grpNday.Controls.Add(Me.btnNdayNone)
        Me.grpNday.Controls.Add(Me.btnNdayAll)
        Me.grpNday.Controls.Add(Me.lstNday)
        Me.grpNday.Dock = System.Windows.Forms.DockStyle.Left
        Me.grpNday.Location = New System.Drawing.Point(0, 0)
        Me.grpNday.Name = "grpNday"
        Me.grpNday.Size = New System.Drawing.Size(200, 502)
        Me.grpNday.TabIndex = 1
        Me.grpNday.TabStop = False
        Me.grpNday.Text = "Number of Days"
        '
        'btnNdayAdd
        '
        Me.btnNdayAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnNdayAdd.Location = New System.Drawing.Point(130, 443)
        Me.btnNdayAdd.Name = "btnNdayAdd"
        Me.btnNdayAdd.Size = New System.Drawing.Size(64, 24)
        Me.btnNdayAdd.TabIndex = 4
        Me.btnNdayAdd.Text = "Add"
        '
        'txtNdayAdd
        '
        Me.txtNdayAdd.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtNdayAdd.Location = New System.Drawing.Point(12, 446)
        Me.txtNdayAdd.Name = "txtNdayAdd"
        Me.txtNdayAdd.Size = New System.Drawing.Size(112, 20)
        Me.txtNdayAdd.TabIndex = 3
        '
        'btnNdayNone
        '
        Me.btnNdayNone.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnNdayNone.Location = New System.Drawing.Point(130, 472)
        Me.btnNdayNone.Name = "btnNdayNone"
        Me.btnNdayNone.Size = New System.Drawing.Size(64, 23)
        Me.btnNdayNone.TabIndex = 6
        Me.btnNdayNone.Text = "None"
        '
        'btnNdayAll
        '
        Me.btnNdayAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnNdayAll.Location = New System.Drawing.Point(12, 472)
        Me.btnNdayAll.Name = "btnNdayAll"
        Me.btnNdayAll.Size = New System.Drawing.Size(64, 24)
        Me.btnNdayAll.TabIndex = 5
        Me.btnNdayAll.Text = "All"
        '
        'lstNday
        '
        Me.lstNday.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstNday.IntegralHeight = False
        Me.lstNday.Location = New System.Drawing.Point(12, 19)
        Me.lstNday.Name = "lstNday"
        Me.lstNday.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple
        Me.lstNday.Size = New System.Drawing.Size(182, 418)
        Me.lstNday.TabIndex = 2
        Me.lstNday.Tag = "NDay"
        '
        'panelBottom
        '
        Me.panelBottom.Controls.Add(Me.lblYearsSeasons)
        Me.panelBottom.Controls.Add(Me.chkLog)
        Me.panelBottom.Controls.Add(Me.chkKeepNDayTSers)
        Me.panelBottom.Controls.Add(Me.radioLow)
        Me.panelBottom.Controls.Add(Me.radioHigh)
        Me.panelBottom.Controls.Add(Me.btnSelectYearsSeasons)
        Me.panelBottom.Controls.Add(Me.btnOk)
        Me.panelBottom.Controls.Add(Me.btnCancel)
        Me.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.panelBottom.Location = New System.Drawing.Point(0, 508)
        Me.panelBottom.Name = "panelBottom"
        Me.panelBottom.Size = New System.Drawing.Size(446, 100)
        Me.panelBottom.TabIndex = 15
        '
        'chkKeepNDayTSers
        '
        Me.chkKeepNDayTSers.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkKeepNDayTSers.AutoSize = True
        Me.chkKeepNDayTSers.Location = New System.Drawing.Point(12, 73)
        Me.chkKeepNDayTSers.Name = "chkKeepNDayTSers"
        Me.chkKeepNDayTSers.Size = New System.Drawing.Size(137, 17)
        Me.chkKeepNDayTSers.TabIndex = 19
        Me.chkKeepNDayTSers.Text = "Keep N-Day Timeseries"
        Me.chkKeepNDayTSers.UseVisualStyleBackColor = True
        '
        'radioLow
        '
        Me.radioLow.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.radioLow.AutoSize = True
        Me.radioLow.Location = New System.Drawing.Point(65, 27)
        Me.radioLow.Name = "radioLow"
        Me.radioLow.Size = New System.Drawing.Size(45, 17)
        Me.radioLow.TabIndex = 18
        Me.radioLow.Text = "Low"
        Me.radioLow.UseVisualStyleBackColor = True
        '
        'radioHigh
        '
        Me.radioHigh.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.radioHigh.AutoSize = True
        Me.radioHigh.Checked = True
        Me.radioHigh.Location = New System.Drawing.Point(12, 27)
        Me.radioHigh.Name = "radioHigh"
        Me.radioHigh.Size = New System.Drawing.Size(47, 17)
        Me.radioHigh.TabIndex = 17
        Me.radioHigh.TabStop = True
        Me.radioHigh.Text = "High"
        Me.radioHigh.UseVisualStyleBackColor = True
        '
        'btnSelectYearsSeasons
        '
        Me.btnSelectYearsSeasons.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSelectYearsSeasons.Location = New System.Drawing.Point(286, 0)
        Me.btnSelectYearsSeasons.Name = "btnSelectYearsSeasons"
        Me.btnSelectYearsSeasons.Size = New System.Drawing.Size(148, 24)
        Me.btnSelectYearsSeasons.TabIndex = 16
        Me.btnSelectYearsSeasons.Text = "Select Years / Seasons..."
        '
        'btnOk
        '
        Me.btnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOk.Location = New System.Drawing.Point(268, 68)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(96, 24)
        Me.btnOk.TabIndex = 13
        Me.btnOk.Text = "Compute"
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(370, 68)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(64, 24)
        Me.btnCancel.TabIndex = 15
        Me.btnCancel.Text = "Cancel"
        '
        'chkLog
        '
        Me.chkLog.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkLog.AutoSize = True
        Me.chkLog.Checked = True
        Me.chkLog.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkLog.Location = New System.Drawing.Point(12, 50)
        Me.chkLog.Name = "chkLog"
        Me.chkLog.Size = New System.Drawing.Size(80, 17)
        Me.chkLog.TabIndex = 20
        Me.chkLog.Text = "Logarithmic"
        Me.chkLog.UseVisualStyleBackColor = True
        '
        'lblYearsSeasons
        '
        Me.lblYearsSeasons.AutoSize = True
        Me.lblYearsSeasons.Location = New System.Drawing.Point(9, 6)
        Me.lblYearsSeasons.Name = "lblYearsSeasons"
        Me.lblYearsSeasons.Size = New System.Drawing.Size(99, 13)
        Me.lblYearsSeasons.TabIndex = 21
        Me.lblYearsSeasons.Text = "Years and Seasons"
        '
        'frmSpecifyFrequency
        '
        Me.AcceptButton = Me.btnOk
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(446, 608)
        Me.Controls.Add(Me.panelTop)
        Me.Controls.Add(Me.panelBottom)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Name = "frmSpecifyFrequency"
        Me.Text = "Select Numbers of Days and Recurrence Intervals"
        Me.panelTop.ResumeLayout(False)
        Me.grpRecurrence.ResumeLayout(False)
        Me.grpRecurrence.PerformLayout()
        Me.grpNday.ResumeLayout(False)
        Me.grpNday.PerformLayout()
        Me.panelBottom.ResumeLayout(False)
        Me.panelBottom.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private WithEvents pDataGroup As atcDataGroup
    Private pDataManager As atcDataManager
    Private pOk As Boolean = False
    Private pYearStartMonth As Integer = 0
    Private pYearStartDay As Integer = 0
    Private pYearEndMonth As Integer = 0
    Private pYearEndDay As Integer = 0
    Private pFirstYear As Integer = 0
    Private pLastYear As Integer = 0

    Public Function AskUser(ByVal aDataManager As atcDataManager, ByVal aGroup As atcDataGroup, ByRef aChoseHigh As Boolean) As Boolean
        pDataManager = aDataManager
        pDataGroup = aGroup
        Clear()
        Me.ShowDialog()
        If pOk Then aChoseHigh = radioHigh.Checked
        pDataGroup = Nothing
        Return pOk
    End Function

    Private Sub Clear()
        If GetSetting("atcFrequencyGrid", "Defaults", "HighOrLow", "High") = "High" Then
            radioHigh.Checked = True
        Else
            radioLow.Checked = True
        End If

        chkKeepNDayTSers.Checked = (GetSetting("atcFrequencyGrid", "Defaults", "KeepNDayTSers", "False") = "True")
        chkLog.Checked = (GetSetting("atcFrequencyGrid", "Defaults", "Logarithmic", "True") = "True")

        pOk = False
        Dim lCalculator As New atcTimeseriesNdayHighLow.atcTimeseriesNdayHighLow
        Dim lNDayHi As atcDefinedValue = lCalculator.AvailableOperations.GetDefinedValue("n-day high value")

        LoadList(lstNday, lNDayHi.Arguments)
        LoadList(lstRecurrence, lNDayHi.Arguments)

        RefreshSeasonsYearsLabel()
    End Sub

    Private Sub LoadList(ByVal lst As Windows.Forms.ListBox, ByVal aArgs As atcDataAttributes)
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
            Dim lDefault As Object = aArgs.GetDefinedValue(lArgName).Definition.DefaultValue
            If Not lDefault Is Nothing AndAlso IsArray(lDefault) Then
                For Each lNumber As Double In lDefault
                    Dim lLabel As String = Format(lNumber, "0.####")
                    lst.Items.Add(lLabel)
                Next
            End If
        End If
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

    Private Sub RefreshSeasonsYearsLabel()
        Dim lLabel As String = ""
        If pYearStartMonth > 0 Then lLabel &= pYearStartMonth & "/"
        If pYearStartDay > 0 Then lLabel &= pYearStartDay & " to "
        If pYearEndMonth > 0 Then lLabel &= pYearEndMonth & "/"
        If pYearEndDay > 0 Then lLabel &= pYearEndDay
        If pFirstYear > 0 Then lLabel &= " " & pFirstYear & " - "
        If pLastYear > 0 Then lLabel &= pLastYear

        lblYearsSeasons.Text = lLabel
    End Sub

    Private Sub Calculate(ByVal aOperationName As String)
        ClearAttributes()
        Dim lCalculator As New atcTimeseriesNdayHighLow.atcTimeseriesNdayHighLow
        Dim lArgs As New atcDataAttributes
        lArgs.SetValue("Timeseries", pDataGroup)
        lArgs.SetValue("NDay", ListToArray(lstNday))
        lArgs.SetValue("Return Period", ListToArray(lstRecurrence))
        lArgs.SetValue("LogFlg", chkLog.Checked)
        If pYearStartMonth > 0 Then lArgs.SetValue("BoundaryMonth", pYearStartMonth)
        If pYearStartDay > 0 Then lArgs.SetValue("BoundaryDay", pYearStartDay)
        If pYearEndMonth > 0 Then lArgs.SetValue("EndMonth", pYearEndMonth)
        If pYearEndDay > 0 Then lArgs.SetValue("EndDay", pYearEndDay)
        If pFirstYear > 0 Then lArgs.SetValue("FirstYear", pFirstYear)
        If pLastYear > 0 Then lArgs.SetValue("LastYear", pLastYear)

        lCalculator.Open(aOperationName, lArgs)
        If chkKeepNDayTSers.Checked Then 'add NDay Tsers to data manager
            pDataManager.DataSources.Add(lCalculator)
        End If
        SaveSetting("atcFrequencyGrid", "Defaults", "HighOrLow", HighOrLowString)
        SaveSetting("atcFrequencyGrid", "Defaults", "KeepNDayTSers", chkKeepNDayTSers.Checked.ToString)
        SaveSetting("atcFrequencyGrid", "Defaults", "Logarithmic", chkLog.Checked.ToString)
        SaveList(lstNday)
        SaveList(lstRecurrence)
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

    Private Sub btnNdayAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNdayAdd.Click
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
    End Sub

    Private Sub btnRecurrenceAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRecurrenceAdd.Click
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

    Private Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
        Calculate("n-day " & HighOrLowString() & " value")
        pOk = True
        Close()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Close()
    End Sub

    Private Sub frmSpecifyFrequency_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyValue = Windows.Forms.Keys.F1 Then
            ShowHelp("BASINS Details\Analysis\Time Series Functions\Frequency Grid.html")
        End If
    End Sub

    Private Sub btnSelectYearsSeasons_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelectYearsSeasons.Click

        Dim lForm As New frmSpecifyYearsSeasons
        If lForm.AskUser("FrequencyGrid" & Me.Text, pDataGroup, pYearStartMonth, pYearStartDay, pYearEndMonth, pYearEndDay, pFirstYear, pLastYear) Then
            'lSeasons = New atcSeasonsYearSubset(aStartMonth, aStartDay, aEndMonth, aEndDay)
            Dim lName As String = HighOrLowString()
            SaveSetting("atcFrequencyGrid", "StartMonth", lName, pYearStartMonth)
            SaveSetting("atcFrequencyGrid", "StartDay", lName, pYearStartDay)
            SaveSetting("atcFrequencyGrid", "EndMonth", lName, pYearEndMonth)
            SaveSetting("atcFrequencyGrid", "EndDay", lName, pYearEndDay)
            RefreshSeasonsYearsLabel()
        End If

    End Sub

    Private Sub radioHigh_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles radioHigh.CheckedChanged
        GetDefaultYearStartEnd()
    End Sub

    Private Sub radioLow_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles radioLow.CheckedChanged
        GetDefaultYearStartEnd()
    End Sub

    Private Sub GetDefaultYearStartEnd()
        Dim lName As String = HighOrLowString()
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
        pYearStartMonth = GetSetting("atcFrequencyGrid", "StartMonth", lName, pYearStartMonth)
        pYearStartDay = GetSetting("atcFrequencyGrid", "StartDay", lName, pYearStartDay)
        pYearEndMonth = GetSetting("atcFrequencyGrid", "EndMonth", lName, pYearEndMonth)
        pYearEndDay = GetSetting("atcFrequencyGrid", "EndDay", lName, pYearEndDay)
        RefreshSeasonsYearsLabel()
    End Sub

    Private Function HighOrLowString() As String
        If radioHigh.Checked Then
            Return "High"
        Else
            Return "Low"
        End If
    End Function

End Class
