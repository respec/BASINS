Imports atcData
Imports atcUtility

Public Class atcSeasonYears

    Private Const pNoDatesInCommon As String = ": No dates in common"

    Private WithEvents pDataGroup As atcDataGroup

    Private pShowBoundaries As Boolean = True
    Private pDateFormat As atcDateFormat
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

    Public Property OmitBeforeYear() As Integer
        Get
            Return GetTextbox(txtOmitBeforeYear)
        End Get
        Set(ByVal aYear As Integer)
            SetTextbox(txtOmitBeforeYear, aYear)
            If aYear > 0 Then ShowCustomYears(True)
        End Set
    End Property

    Public Property OmitAfterYear() As Integer
        Get
            Return GetTextbox(txtOmitAfterYear)
        End Get
        Set(ByVal aYear As Integer)
            SetTextbox(txtOmitAfterYear, aYear)
            If aYear > 0 Then ShowCustomYears(True)
        End Set
    End Property

    Private Function GetTextbox(ByVal aTextbox As Windows.Forms.TextBox) As Integer
        If IsNumeric(aTextbox.Text) Then
            Return CInt(aTextbox.Text)
        Else
            Return 0
        End If
    End Function

    Private Sub SetTextbox(ByVal aTextbox As Windows.Forms.TextBox, ByVal aValue As Integer)
        If aValue = 0 Then
            aTextbox.Text = ""
        Else
            aTextbox.Text = aValue
        End If
    End Sub

    Public Property DataGroup() As atcDataGroup
        Get
            Return pDataGroup
        End Get
        Set(ByVal aGroup As atcDataGroup)
            pDataGroup = aGroup
            Reset()
        End Set
    End Property

    ''' <summary>
    ''' Set all the controls to default values from current DataGroup
    ''' </summary>
    Public Sub Reset()
        pDateFormat = New atcDateFormat
        With pDateFormat
            .IncludeHours = False
            .IncludeMinutes = False
            .IncludeSeconds = False
        End With

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

        With cboYears.Items
            .Clear()
            .Add(lAllText)
            .Add(lCommonText)
            .Add("Custom")
        End With
        cboYears.SelectedIndex = 0
    End Sub

    Public Property CommonStart() As Double
        Get
            Return pCommonStart
        End Get
        Set(ByVal aValue As Double)
            pCommonStart = aValue
        End Set
    End Property

    Public Property CommonEnd() As Double
        Get
            Return pCommonEnd
        End Get
        Set(ByVal aValue As Double)
            pCommonEnd = aValue
        End Set
    End Property

    Private Sub cboYears_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboYears.SelectedIndexChanged
        Select Case cboYears.SelectedIndex
            Case 0 'All
                txtOmitBeforeYear.Text = ""
                txtOmitAfterYear.Text = ""
            Case 1 'Common
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

    Private Sub ShowCustomYears(ByVal aShowCustom As Boolean)
        cboYears.Visible = Not aShowCustom
        txtOmitBeforeYear.Visible = aShowCustom
        txtOmitAfterYear.Visible = aShowCustom
        lblDataStart.Visible = aShowCustom
        lblDataEnd.Visible = aShowCustom
        lblOmitBefore.Visible = aShowCustom
        lblOmitAfter.Visible = aShowCustom
    End Sub

End Class
