Public Class atcTimeseriesPointList
    Implements ZedGraph.IPointList

    Private pValues As atcData.atcTimeseries
    Private pDates As atcData.atcTimeseries
    Private pPoint As Boolean = False

    Private pPointPairs() As ZedGraph.PointPair

    Sub New(ByVal aTimeseries As atcData.atcTimeseries)
        pValues = aTimeseries
        pDates = aTimeseries.Dates
        pPoint = aTimeseries.Attributes.GetValue("point", False)
        ReDim pPointPairs(Count)
    End Sub

    Public Function Clone() As Object Implements ZedGraph.IPointList.Clone
        Return New atcTimeseriesPointList(pValues)
    End Function

    Public ReadOnly Property Count() As Integer Implements ZedGraph.IPointList.Count
        Get
            If pPoint Then
                Return pDates.numValues
            Else
                Return pDates.numValues + 1
            End If
        End Get
    End Property

    <CLSCompliant(False)> _
    Default Public ReadOnly Property Item(ByVal aIndex As Integer) As ZedGraph.PointPair Implements ZedGraph.IPointList.Item
        Get
            If pPointPairs(aIndex) Is Nothing Then
                Dim dIndex As Integer = aIndex
                Dim vIndex As Integer
                If pPoint Then
                    dIndex += 1
                    vIndex = dIndex
                ElseIf dIndex = 0 Then
                    vIndex = dIndex + 1
                End If
                If Not pPoint AndAlso aIndex = 0 AndAlso pValues.numValues > 0 Then
                    pPointPairs(aIndex) = New ZedGraph.PointPair(pDates.Value(dIndex), pValues.Value(1))
                Else
                    pPointPairs(aIndex) = New ZedGraph.PointPair(pDates.Value(dIndex), pValues.Value(dIndex))
                End If
            End If
            Return pPointPairs(aIndex)
        End Get
    End Property
End Class
