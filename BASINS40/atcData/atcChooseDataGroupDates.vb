Imports atcUtility

Public Class atcChooseDataGroupDates

    Private Const pNoDatesInCommon As String = "none"

    Private WithEvents pDataGroup As atcDataGroup

    Private pDateFormat As atcDateFormat

    Private pFirstStart As Double = GetNaN()
    Private pLastEnd As Double = GetNaN()
    Private pCommonStart As Double = GetNaN()
    Private pCommonEnd As Double = GetNaN()

    Public Property OmitBefore() As Double
        Get
            Return GetTextboxJdate(txtOmitBefore)
        End Get
        Set(ByVal aJdate As Double)
            If Math.Abs(aJdate) > 5 Then txtOmitBefore.Text = pDateFormat.JDateToString(aJdate)
        End Set
    End Property

    Public Property OmitAfter() As Double
        Get
            Return GetTextboxJdate(txtOmitAfter)
        End Get
        Set(ByVal aJdate As Double)
            If Math.Abs(aJdate) > 5 Then txtOmitAfter.Text = pDateFormat.JDateToString(aJdate)
        End Set
    End Property

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

    Public Property FirstStart() As Double
        Get
            Return pFirstStart
        End Get
        Set(ByVal aValue As Double)
            pFirstStart = aValue
        End Set
    End Property

    Public Property LastEnd() As Double
        Get
            Return pLastEnd
        End Get
        Set(ByVal aValue As Double)
            pLastEnd = aValue
        End Set
    End Property

    Private Function GetTextboxJdate(ByVal aTextbox As Windows.Forms.TextBox) As Double
        Try
            Dim lDate As Date = Date.Parse(aTextbox.Text)
            Return lDate.ToOADate
        Catch e As Exception
            Return 0
        End Try
    End Function

    Public Property DataGroup() As atcDataGroup
        Get
            Return pDataGroup
        End Get
        Set(ByVal aGroup As atcDataGroup)
            pDataGroup = aGroup
            Reset()
        End Set
    End Property

    Private Sub pDataGroup_Added(ByVal aAdded As atcUtility.atcCollection) Handles pDataGroup.Added
        Reset()
    End Sub

    Private Sub pDataGroup_Removed(ByVal aRemoved As atcUtility.atcCollection) Handles pDataGroup.Removed
        Reset()
    End Sub

    ''' <summary>
    ''' Set all the controls to default values from current DataGroup
    ''' </summary>
    Public Sub Reset()
        If pDateFormat Is Nothing Then
            pDateFormat = New atcDateFormat
            With pDateFormat
                .IncludeHours = False
                .IncludeMinutes = False
                .IncludeSeconds = False
            End With
        End If

        pFirstStart = GetMaxValue()
        pLastEnd = GetMinValue()

        pCommonStart = GetMinValue()
        pCommonEnd = GetMaxValue()

        If Not pDataGroup Is Nothing Then
            For Each lTs As atcData.atcTimeseries In pDataGroup
                If lTs.Dates.numValues > 0 Then
                    Dim lThisDate As Double = lTs.Dates.Value(1)
                    If lThisDate < pFirstStart Then pFirstStart = lThisDate
                    If lThisDate > pCommonStart Then pCommonStart = lThisDate
                    lThisDate = lTs.Dates.Value(lTs.Dates.numValues)
                    If lThisDate > pLastEnd Then pLastEnd = lThisDate
                    If lThisDate < pCommonEnd Then pCommonEnd = lThisDate
                End If
            Next
        End If
        If pFirstStart <= pLastEnd Then
            lblDataStart.Text = pDateFormat.JDateToString(pFirstStart)
            lblDataEnd.Text = pDateFormat.JDateToString(pLastEnd)
            btnAll.Enabled = True
            btnAll_Click(Nothing, Nothing)
        Else
            lblDataStart.Text = ""
            lblDataEnd.Text = ""
            btnAll.Enabled = False
        End If

        If pCommonStart > GetMinValue() AndAlso pCommonEnd < GetMaxValue() AndAlso pCommonStart < pCommonEnd Then
            lblCommonStart.Text = pDateFormat.JDateToString(pCommonStart)
            lblCommonEnd.Text = pDateFormat.JDateToString(pCommonEnd)
            btnCommon.Enabled = True
        Else
            lblCommonStart.Text = pNoDatesInCommon
            lblCommonEnd.Text = pNoDatesInCommon
            btnCommon.Enabled = False
        End If
    End Sub

    Public ReadOnly Property SelectedAll() As Boolean
        Get
            Return txtOmitBefore.Text = lblDataStart.Text AndAlso txtOmitAfter.Text = lblDataEnd.Text
        End Get
    End Property

    Public Function CreateSelectedDataGroupSubset() As atcDataGroup
        If pDataGroup Is Nothing Then
            'nothing to subset, return empty group
            Return New atcDataGroup
        ElseIf SelectedAll Then
            'No need to subset
            Return pDataGroup
        Else
            Dim lSubsetGroup As New atcDataGroup
            Dim lStartDate As Double = OmitBefore
            Dim lEndDate As Double = OmitAfter
            If lStartDate <= lEndDate Then
                For Each lTs As atcData.atcTimeseries In pDataGroup
                    If lTs.Dates.numValues > 0 Then
                        lSubsetGroup.Add(SubsetByDate(lTs, lStartDate, lEndDate, Nothing))
                    End If
                Next
            End If
            Return lSubsetGroup
        End If
    End Function

    Private Sub btnAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAll.Click
        txtOmitBefore.Text = lblDataStart.Text
        txtOmitAfter.Text = lblDataEnd.Text
    End Sub

    Private Sub btnCommon_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCommon.Click
        txtOmitBefore.Text = lblCommonStart.Text
        txtOmitAfter.Text = lblCommonEnd.Text
    End Sub
End Class
