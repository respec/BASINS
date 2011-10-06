Imports atcData
Imports atcUtility
Imports MapWinUtility
Imports System.Windows.Forms

Public Class frmMain
    Private pName As String = "Unnamed"
    Private WithEvents pDataGroup As atcTimeseriesGroup
    Private pBasicAttributes As Generic.List(Of String)
    Private pDateFormat As atcDateFormat

    Private pYearStartMonth As Integer = 0
    Private pYearStartDay As Integer = 0
    Private pYearEndMonth As Integer = 0
    Private pYearEndDay As Integer = 0
    Private pFirstYear As Integer = 0
    Private pLastYear As Integer = 0

    Private pCommonStart As Double = GetMinValue()
    Private pCommonEnd As Double = GetMaxValue()
    Private Const pNoDatesInCommon As String = ": No dates in common"

    Private pModel As String = ""
    Private pClsBaseFlow As atcTimeseriesBaseflow.atcTimeseriesBaseflow

    Public Sub Initialize(Optional ByVal aTimeseriesGroup As atcData.atcTimeseriesGroup = Nothing, _
                      Optional ByVal aBasicAttributes As Generic.List(Of String) = Nothing, _
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

        If aShowForm Then
            Dim DisplayPlugins As ICollection = atcDataManager.GetPlugins(GetType(atcDataDisplay))
            For Each lDisp As atcDataDisplay In DisplayPlugins
                Dim lMenuText As String = lDisp.Name
                If lMenuText.StartsWith("Analysis::") Then lMenuText = lMenuText.Substring(10)
                mnuAnalysis.DropDownItems.Add(lMenuText, Nothing, New EventHandler(AddressOf mnuAnalysis_Click))
            Next
        End If

        If pDataGroup.Count = 0 Then 'ask user to specify some timeseries
            pDataGroup = atcDataManager.UserSelectData("Select Daily Streamflow Data for Baseflow Separation", _
                                                       pDataGroup, Nothing, True, True, Me.Icon)
        End If

        If pDataGroup.Count > 0 Then
            PopulateForm()
            If aShowForm Then Me.Show()
        Else 'user declined to specify timeseries
            Me.Close()
        End If

    End Sub

    Private Sub PopulateForm()
        pDateFormat = New atcDateFormat
        With pDateFormat
            .IncludeHours = False
            .IncludeMinutes = False
            .IncludeSeconds = False
        End With

        pModel = GetSetting("atcUSGSBaseflow", "Defaults", "Model", "HySep-Fixed")
        StationInfoFile = GetSetting("atcUSGSBaseflow", "Defaults", "Stations", "Station.txt")

        RepopulateForm()
    End Sub

    Private Sub RepopulateForm()
        Dim lFirstDate As Double = GetMaxValue()
        Dim lLastDate As Double = GetMinValue()

        pCommonStart = GetMinValue()
        pCommonEnd = GetMaxValue()

        Dim lAllText As String = "All"
        Dim lCommonText As String = "Common"

        For Each lDataset As atcData.atcTimeseries In pDataGroup
            If lDataset.Dates.numValues > 0 Then
                Dim lThisDate As Double = lDataset.Dates.Value(0)
                If lThisDate < lFirstDate Then lFirstDate = lThisDate
                If lThisDate > pCommonStart Then pCommonStart = lThisDate
                lThisDate = lDataset.Dates.Value(lDataset.Dates.numValues)
                If lThisDate > lLastDate Then lLastDate = lThisDate
                If lThisDate < pCommonEnd Then pCommonEnd = lThisDate
            End If
        Next

        If lFirstDate < GetMaxValue() AndAlso lLastDate > GetMinValue() Then
            txtDataStart.Text = txtDataStart.Tag & " " & pDateFormat.JDateToString(lFirstDate)
            txtDataEnd.Text = txtDataEnd.Tag & " " & pDateFormat.JDateToString(lLastDate)
            lAllText &= ": " & pDateFormat.JDateToString(lFirstDate) & " to " & pDateFormat.JDateToString(lLastDate)
        End If

        If pCommonStart > GetMinValue() AndAlso pCommonEnd < GetMaxValue() AndAlso pCommonStart < pCommonEnd Then
            lCommonText &= ": " & pDateFormat.JDateToString(pCommonStart) & " to " & pDateFormat.JDateToString(pCommonEnd)
        Else
            lCommonText &= pNoDatesInCommon
        End If

    End Sub

    Public Function AskUser(ByVal aName As String, _
                        ByVal aDataGroup As atcData.atcTimeseriesGroup, _
                        ByRef aStartMonth As Integer, _
                        ByRef aStartDay As Integer, _
                        ByRef aEndMonth As Integer, _
                        ByRef aEndDay As Integer, _
                        ByRef aFirstYear As Integer, _
                        ByRef aLastYear As Integer) As Boolean

        'pName = aName

        'txtStartDay.Text = aStartDay
        'cboStartMonth.SelectedIndex = aStartMonth - 1

        'txtEndDay.Text = aEndDay
        'cboEndMonth.SelectedIndex = aEndMonth - 1

        'If aFirstYear <> 0 Then txtOmitBeforeYear.Text = aFirstYear
        'If aLastYear <> 0 Then txtOmitAfterYear.Text = aLastYear

        'Dim lFirstDate As Double = GetMaxValue()
        'Dim lLastDate As Double = GetMinValue()
        'For Each lDataset As atcData.atcTimeseries In aDataGroup
        '    If lDataset.Dates.numValues > 0 Then
        '        Dim lThisDate As Double = lDataset.Dates.Value(1)
        '        If lThisDate < lFirstDate Then lFirstDate = lThisDate
        '        lThisDate = lDataset.Dates.Value(lDataset.Dates.numValues)
        '        If lThisDate > lLastDate Then lLastDate = lThisDate
        '    End If
        'Next
        'If lFirstDate < GetMaxValue() Then
        '    'Dim lDate As Date = Date.FromOADate(lFirstDate)
        '    'txtOmitBeforeYear.Text = lDate.Year
        '    lblDataStart.Text = lblDataStart.Tag & " " & pDateFormat.JDateToString(lFirstDate)
        'End If
        'If lLastDate > GetMinValue() Then
        '    'Dim lDate As Date = Date.FromOADate(lLastDate)
        '    'txtOmitAfterYear.Text = lDate.Year
        '    lblDataEnd.Text = lblDataEnd.Tag & " " & pDateFormat.JDateToString(lLastDate)
        'End If

        'If Me.ShowDialog() = Windows.Forms.DialogResult.OK Then
        '    aStartMonth = cboStartMonth.SelectedIndex + 1
        '    If IsNumeric(txtStartDay.Text) Then
        '        aStartDay = txtStartDay.Text
        '    Else
        '        aStartDay = 1
        '    End If
        '    aEndMonth = cboEndMonth.SelectedIndex + 1
        '    If IsNumeric(txtEndDay.Text) Then
        '        aEndDay = txtEndDay.Text
        '    Else
        '        aEndDay = pLastDayOfMonth(aEndMonth)
        '    End If
        '    If IsNumeric(txtOmitBeforeYear.Text) Then
        '        aFirstYear = CInt(txtOmitBeforeYear.Text)
        '    Else
        '        aFirstYear = 0
        '    End If
        '    If IsNumeric(txtOmitAfterYear.Text) Then
        '        aLastYear = CInt(txtOmitAfterYear.Text)
        '    Else
        '        aLastYear = 0
        '    End If
        '    Return True
        'Else
        '    Return False
        'End If
    End Function

    Private Sub mnuAnalysis_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAnalysis.Click
        atcDataManager.ShowDisplay(sender.Text, pDataGroup)
    End Sub

    Private Sub btnFindStations_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindStations.Click
        StationInfoFile = FindFile("Locate Station File", StationInfoFile, "txt")
        SaveSetting("atcUSGSBaseflow", "Defaults", "Stations", StationInfoFile)
        GetStations()
        Dim lfrmStaion As New frmStations(Stations)
        lfrmStaion.Show()
    End Sub

    Private Sub btnExamineData_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExamineData.Click
        For Each lTs As atcTimeseries In pDataGroup
            Dim lfrmDataSummary As New frmDataSummary(PrintDataSummary(lTs))
            lfrmDataSummary.txtDataSummary.SelectionStart = 0
            lfrmDataSummary.Show()
        Next
    End Sub
End Class