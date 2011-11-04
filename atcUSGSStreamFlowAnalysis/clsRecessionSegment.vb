Imports atcData

Public Class clsRecessionSegment
    Implements IComparable

    Public Shared RecessionCount As Integer
    Public Shared StreamFlowTS As atcTimeseries
    Public Shared MaxSegmentLengthInDays As Integer = 60
    Public PeakDayIndex As Integer
    Public PeakDayDate As Double
    Public SegmentLength As Integer
    Public Coefficient1 As Double
    Public Coefficient2 As Double
    Public DaysLogCycle As Double = -1 * Coefficient1
    Public MinDayOrdinal As Integer
    Public MaxDayOrdinal As Integer
    Public IsExcluded As Boolean = False
    Public NeedtoReadData As Boolean = True
    Public MeanLogQ As Double
    Public MeanOrdinals As Double

    'for display
    Public ReadOnly Property BestFitEquation() As String
        Get
            Return " T = ( " & String.Format("{0:0.0000}", Coefficient1).PadLeft(12, " ") & "* LOGQ )  +  " & String.Format("{0:0.0000}", Coefficient2).PadLeft(12, " ")
        End Get
    End Property

    Public Flow() As Double
    Public QLog() As Double
    Public Dates() As Double

    Public Sub New()
        'ReDim Flow(MaxSegmentLengthInDays)
        'ReDim QLog(MaxSegmentLengthInDays)
        'ReDim Dates(MaxSegmentLengthInDays)
    End Sub

    Public Sub GetData()
        ReDim Flow(SegmentLength)
        ReDim QLog(SegmentLength)
        ReDim Dates(SegmentLength)

        For I As Integer = 1 To SegmentLength 'loop 215
            Flow(I) = 0.0
            QLog(I) = -99.9
        Next 'loop 215
        For I As Integer = 1 To SegmentLength 'loop 220
            Flow(I) = StreamFlowTS.Value(I + PeakDayIndex)
            If Flow(I) = 0.0 Then
                QLog(I) = -88.8
            Else
                QLog(I) = Math.Log10(Flow(I))
            End If
            Dates(I) = StreamFlowTS.Dates.Value(I + PeakDayIndex - 1)
        Next 'loop 220
        NeedtoReadData = False
    End Sub

    Public Sub GetDataSubset()
        If MinDayOrdinal = MaxDayOrdinal AndAlso MaxDayOrdinal = 0 Then
            Exit Sub
        End If

        Dim lnewLength As Integer = MaxDayOrdinal - MinDayOrdinal + 1

        Dim lFlow(lnewLength) As Double
        Dim lQLog(lnewLength) As Double
        Dim lDates(lnewLength) As Double
        For I As Integer = 1 To lnewLength 'loop 215
            lFlow(I) = 0.0
            lQLog(I) = -99.9
        Next 'loop 215

        Dim lNewIndex As Integer = 0
        For I As Integer = 1 To SegmentLength 'loop 220
            If I < MinDayOrdinal OrElse I > MaxDayOrdinal Then
                Continue For
            Else
                lFlow(lNewIndex) = Flow(I)
                lQLog(lNewIndex) = QLog(I)
                lDates(lNewIndex) = Dates(I)
            End If
        Next 'loop 220

        ReDim Flow(lnewLength)
        ReDim QLog(lnewLength)
        ReDim Dates(lnewLength)

        For I As Integer = 1 To lnewLength
            Flow(lNewIndex) = lFlow(I)
            QLog(lNewIndex) = lQLog(I)
            Dates(lNewIndex) = lDates(I)
        Next

        ReDim lFlow(0)
        ReDim lQLog(0)
        ReDim lDates(0)
    End Sub

    Public Sub ReadData()

    End Sub
    Public Sub Clear()
        ReDim Flow(0)
        ReDim QLog(0)
        ReDim Dates(0)
    End Sub

    Public Function CompareTo(ByVal obj As Object) As Integer Implements System.IComparable.CompareTo
        If TypeOf obj Is clsRecessionSegment Then
            Dim aSeg As clsRecessionSegment = CType(obj, clsRecessionSegment)
            Return MeanLogQ.CompareTo(aSeg.MeanLogQ)
        End If
        Throw New ArgumentException("object is not a clsRecessionSegment")
    End Function
End Class
