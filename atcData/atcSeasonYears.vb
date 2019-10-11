Imports atcData
Imports atcUtility
Imports System.ComponentModel

Public Class atcSeasonYears

    Private WithEvents pDataGroup As atcTimeseriesGroup

    Private pShowBoundaries As Boolean = True

    Private pDateFormatStart As atcDateFormat
    Private pDateFormatEnd As atcDateFormat

    Private pFirstDate As Double = GetMaxValue()
    Private pLastDate As Double = GetMinValue()

    Private pCommonStart As Double = GetNaN()
    Private pCommonEnd As Double = GetNaN()

    Public Property ShowBoundaries() As Boolean
        Get
            Return pShowBoundaries
        End Get
        Set(ByVal aShowBoundaries As Boolean)
            If pShowBoundaries <> aShowBoundaries Then
                pShowBoundaries = aShowBoundaries
                grpBoundaries.Visible = pShowBoundaries
                Splitter1.Visible = pShowBoundaries
                If pShowBoundaries Then
                Else
                    grpYears.Left = 0
                    grpYears.Width = Me.Width
                End If
            End If
        End Set
    End Property

    Public Property StartMonth() As Integer
        Get
            Return cboStartMonth.SelectedIndex + 1
        End Get
        Set(ByVal aMonth As Integer)
            If aMonth > 0 AndAlso aMonth <= cboStartMonth.Items.Count Then
                cboStartMonth.SelectedIndex = aMonth - 1
            End If
        End Set
    End Property

    Public Property EndMonth() As Integer
        Get
            Return cboEndMonth.SelectedIndex + 1
        End Get
        Set(ByVal aMonth As Integer)
            If aMonth > 0 AndAlso aMonth <= cboEndMonth.Items.Count Then
                cboEndMonth.SelectedIndex = aMonth - 1
            End If
        End Set
    End Property

    Public Property StartDay() As Integer
        Get
            Return GetTextbox(txtStartDay)
        End Get
        Set(ByVal aDay As Integer)
            SetTextbox(txtStartDay, aDay)
        End Set
    End Property

    Public Property EndDay() As Integer
        Get
            Return GetTextbox(txtEndDay)
        End Get
        Set(ByVal aDay As Integer)
            SetTextbox(txtEndDay, aDay)
        End Set
    End Property

    <Browsable(False)> _
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Public Property OmitBeforeYear() As Integer
        Get
            Return GetTextbox(txtOmitBeforeYear)
        End Get
        Set(ByVal aYear As Integer)
            SetTextbox(txtOmitBeforeYear, aYear)
        End Set
    End Property

    <Browsable(False)> _
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Public Property OmitAfterYear() As Integer
        Get
            Return GetTextbox(txtOmitAfterYear)
        End Get
        Set(ByVal aYear As Integer)
            SetTextbox(txtOmitAfterYear, aYear)
        End Set
    End Property

    Private Function GetTextbox(ByVal aTextbox As System.Windows.Forms.TextBox) As Integer
        If IsNumeric(aTextbox.Text) Then
            Return CInt(aTextbox.Text)
        Else
            Return 0
        End If
    End Function

    Private Sub SetTextbox(ByVal aTextbox As System.Windows.Forms.TextBox, ByVal aValue As Integer)
        If aValue = 0 Then
            aTextbox.Text = ""
        Else
            aTextbox.Text = aValue
        End If
    End Sub

    <Browsable(False)> _
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Public Property DataGroup() As atcTimeseriesGroup
        Get
            Return pDataGroup
        End Get
        Set(ByVal aGroup As atcTimeseriesGroup)
            pDataGroup = aGroup
            Reset()
        End Set
    End Property

    ''' <summary>
    ''' Set all the controls to default values from current DataGroup
    ''' </summary>
    Public Sub Reset()
        If pDateFormatStart Is Nothing Then
            pDateFormatStart = New atcDateFormat
            With pDateFormatStart
                .IncludeHours = False
                .IncludeMinutes = False
                .IncludeSeconds = False
                .Midnight24 = False
            End With
        End If

        If pDateFormatEnd Is Nothing Then
            pDateFormatEnd = New atcDateFormat
            With pDateFormatEnd
                .IncludeHours = False
                .IncludeMinutes = False
                .IncludeSeconds = False
            End With
        End If

        pCommonStart = GetMinValue()
        pCommonEnd = GetMaxValue()

        For Each lDataset As atcData.atcTimeseries In pDataGroup
            If lDataset.Dates.numValues > 0 Then
                Dim lThisDate As Double = lDataset.Dates.Value(1)
                If lThisDate < pFirstDate Then pFirstDate = lThisDate
                If lThisDate > pCommonStart Then pCommonStart = lThisDate
                lThisDate = lDataset.Dates.Value(lDataset.Dates.numValues)
                If lThisDate > pLastDate Then pLastDate = lThisDate
                If lThisDate < pCommonEnd Then pCommonEnd = lThisDate
            End If
        Next
        If pFirstDate < pLastDate Then
            lblDataStart.Text = lblDataStart.Tag & " " & pDateFormatStart.JDateToString(pFirstDate)
            lblDataEnd.Text = lblDataEnd.Tag & " " & pDateFormatEnd.JDateToString(pLastDate)
            btnAll.Enabled = True
        Else
            btnAll.Enabled = False
        End If

        If pCommonStart > GetMinValue() AndAlso pCommonEnd < GetMaxValue() AndAlso pCommonStart < pCommonEnd Then
            btnCommon.Enabled = True
        Else
            btnCommon.Text = "No common dates"
            btnCommon.Enabled = False
        End If

    End Sub

    <Browsable(False)> _
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Public Property CommonStart() As Double
        Get
            Return pCommonStart
        End Get
        Set(ByVal aValue As Double)
            pCommonStart = aValue
        End Set
    End Property

    <Browsable(False)> _
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Public Property CommonEnd() As Double
        Get
            Return pCommonEnd
        End Get
        Set(ByVal aValue As Double)
            pCommonEnd = aValue
        End Set
    End Property

    Private Sub btnCalendarYear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCalendarYear.Click
        cboStartMonth.SelectedIndex = 0
        txtStartDay.Text = "1"
        cboEndMonth.SelectedIndex = 11
        txtEndDay.Text = "31"
    End Sub

    Private Sub btnWaterYear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnWaterYear.Click
        cboStartMonth.SelectedIndex = 9
        txtStartDay.Text = "1"
        cboEndMonth.SelectedIndex = 8
        txtEndDay.Text = "30"
    End Sub

    'Private Sub btnCommonYears_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Dim lCurDate(5) As Integer
    '    J2Date(pCommonStart, lCurDate)
    '    'TODO: shift by one year when required to follow water/drought year convention
    '    txtOmitBeforeYear.Text = Format(lCurDate(0), "0000")
    '    J2Date(pCommonEnd, lCurDate)
    '    txtOmitAfterYear.Text = Format(lCurDate(0), "0000")
    'End Sub

    'Private Sub btnAllYears_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Dim lCurDate(5) As Integer
    '    J2Date(pFirstDate, lCurDate)
    '    'TODO: shift by one year when required to follow water/drought year convention
    '    txtOmitBeforeYear.Text = Format(lCurDate(0), "0000")
    '    J2Date(pLastDate, lCurDate)
    '    txtOmitAfterYear.Text = Format(lCurDate(0), "0000")
    'End Sub


    Private Sub btnAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAll.Click
        txtOmitBeforeYear.Text = lblDataStart.Text
        txtOmitAfterYear.Text = lblDataEnd.Text
    End Sub

    Private Sub btnCommon_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCommon.Click
        txtOmitBeforeYear.Text = lblCommonStart.Text
        txtOmitAfterYear.Text = lblCommonEnd.Text
    End Sub
End Class
