Public Class atcTimeseriesSource
    Inherits atcDataSource

    ''' <summary>Create a new timeseries source</summary>
    Public Sub New()
        pData = New atcTimeseriesGroup
    End Sub

    Public Shadows ReadOnly Property DataSets() As atcTimeseriesGroup
        Get
            Return pData
        End Get
    End Property
End Class
