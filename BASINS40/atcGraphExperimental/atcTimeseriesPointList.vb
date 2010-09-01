Public Class atcTimeseriesPointList
    Implements ZedGraph.IPointList

    Private pValues As atcData.atcTimeseries
    Private pDates As atcData.atcTimeseries
    Private pPoint As Boolean = False

    Sub New(ByVal aTimeseries As atcData.atcTimeseries)
        pPoint = aTimeseries.Attributes.GetValue("point", False)
        If pPoint OrElse aTimeseries.Attributes.GetValue("Count") = aTimeseries.numValues Then
            pValues = aTimeseries
            pDates = aTimeseries.Dates
        Else
            Dim lNumValuesNew As Integer = (2 * aTimeseries.numValues) - aTimeseries.Attributes.GetValue("Count")
            pValues = New atcData.atcTimeseries(Nothing)
            pDates = New atcData.atcTimeseries(Nothing)
            pValues.Dates = pDates
            pValues.numValues = lNumValuesNew
            Dim lIndexNew As Integer = 0
            For lIndex As Integer = 0 To aTimeseries.numValues
                Dim lValue As Double = aTimeseries.Value(lIndex)
                pValues.Value(lIndexNew) = lValue
                Dim lDate As Double = aTimeseries.Dates.Value(lIndex)
                pDates.Value(lIndexNew) = lDate
                If lIndex > 0 AndAlso lIndex < aTimeseries.numValues AndAlso Double.IsNaN(lValue) Then
                    lIndexNew += 1
                    pValues.Value(lIndexNew) = aTimeseries.Value(lIndex + 1)
                    pDates.Value(lIndexNew) = lDate
                End If
                lIndexNew += 1
            Next
        End If
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
            Dim dIndex As Integer = aIndex
            Dim vIndex As Integer
            If pPoint Then
                dIndex += 1
                vIndex = dIndex
            ElseIf dIndex = 0 Then
                vIndex = dIndex + 1
            End If
            If Not pPoint AndAlso aIndex = 0 AndAlso pValues.numValues > 0 Then
                Return New ZedGraph.PointPair(pDates.Value(dIndex), pValues.Value(1))
            Else
                Return New ZedGraph.PointPair(pDates.Value(dIndex), pValues.Value(dIndex))
            End If
        End Get
    End Property
End Class
