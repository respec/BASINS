Public Class atcTimeseriesGroup
    Inherits atcDataGroup
    Sub New()
        MyBase.New()
    End Sub

    ''' <summary>Create a new timeseries group and add a timeseries
    ''' to the group with the default key of its serial number</summary>
    Public Sub New(ByVal aTimeseries As atcTimeseries)
        MyBase.New()
        Add(aTimeseries.Serial, aTimeseries)
    End Sub

    ''' <summary>Create a new timeseries group and add timeseries
    ''' to the group with the default key of its serial number</summary>
    Public Sub New(ByVal ParamArray aTimeseries() As atcTimeseries)
        MyBase.New()
        For Each lTimeseries As atcTimeseries In aTimeseries
            Add(lTimeseries.Serial, lTimeseries)
        Next
    End Sub

    ''' <summary>atcDataSet by index</summary>
    Default Public Shadows Property Item(ByVal aIndex As Integer) As atcTimeseries
        Get
            Return MyBase.Item(aIndex)
        End Get
        Set(ByVal newValue As atcTimeseries)
            MyBase.Item(aIndex) = newValue
        End Set
    End Property

    ''' <summary>atcDataSet by index</summary>
    Public Shadows Property ItemByIndex(ByVal aIndex As Integer) As atcTimeseries
        Get
            Return MyBase.Item(aIndex)
        End Get
        Set(ByVal newValue As atcTimeseries)
            MyBase.Item(aIndex) = newValue
        End Set
    End Property

    ''' <summary>atcDataSet by key</summary>
    Public Shadows Property ItemByKey(ByVal aKey As Object) As atcTimeseries
        Get
            Return MyBase.ItemByKey(aKey)
        End Get
        Set(ByVal newValue As atcTimeseries)
            MyBase.ItemByKey(aKey) = newValue
        End Set
    End Property
End Class
