Public Class atcTimeseriesSource
    Inherits atcDataSource

    ''' <summary>Create a new timeseries source</summary>
    Public Sub New()
        pData = New atcTimeseriesGroup
    End Sub

    Public Shadows ReadOnly Property DataSets() As atcTimeseriesGroup
        Get
            Return CType(pData, atcTimeseriesGroup)
        End Get
    End Property

    'Public Overridable Shadows Function AddDatasets(ByVal aTimeseriesGroup As atcTimeseriesGroup) As Boolean
    '    Return MyBase.AddDataSets(aTimeseriesGroup)
    'End Function
End Class
