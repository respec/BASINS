Imports atcUtility
Imports System.ComponentModel

Public Class atcCommonDataGroupDates
    Private Const pNoDatesInCommon As String = "none"

    Private WithEvents pDataGroup As atcTimeseriesGroup

    Private pDateFormatStart As atcDateFormat
    Private pDateFormatEnd As atcDateFormat

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

        Dim lFirstStart As Double = GetNaN()
        Dim lLastEnd As Double = GetNaN()
        Dim lCommonStart As Double = GetNaN()
        Dim lCommonEnd As Double = GetNaN()

        If CommonDates(pDataGroup, lFirstStart, lLastEnd, lCommonStart, lCommonEnd) Then
            lblCommonStart.Text = pDateFormatStart.JDateToString(lCommonStart)
            lblCommonEnd.Text = pDateFormatEnd.JDateToString(lCommonEnd)
        Else
            lblCommonStart.Text = pNoDatesInCommon
            lblCommonEnd.Text = pNoDatesInCommon
        End If

        If lFirstStart <= lLastEnd Then
            lblDataStart.Text = pDateFormatStart.JDateToString(lFirstStart)
            lblDataEnd.Text = pDateFormatEnd.JDateToString(lLastEnd)
        Else
            lblDataStart.Text = pNoDatesInCommon
            lblDataEnd.Text = pNoDatesInCommon
        End If
    End Sub

End Class
