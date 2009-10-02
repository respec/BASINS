Imports atcUtility

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

    ''' <summary>Create a new group containing the same timeseries as aDataGroup</summary>
    Public Sub New(ByVal aDataGroup As atcDataGroup)
        MyBase.New()
        For lIndex As Integer = 0 To aDataGroup.Count - 1
            Add(aDataGroup.Keys(lIndex), aDataGroup.ItemByIndex(lIndex))
        Next
    End Sub

    ''' <summary>Create a copy of this data group containing references to the same group of atcTimeseries</summary>
    ''' <remarks>Does not create copies of each atcTimeseries, clone refers to same objects as original</remarks>
    Public Shadows Function Clone() As atcTimeseriesGroup
        Dim lClone As New atcTimeseriesGroup
        For lIndex As Integer = 0 To MyBase.Count - 1
            lClone.Add(MyBase.Keys(lIndex), MyBase.Item(lIndex))
        Next
        Return lClone
    End Function

    ''' <summary>atcTimeseries by index</summary>
    Default Public Shadows Property Item(ByVal aIndex As Integer) As atcTimeseries
        Get
            Return MyBase.Item(aIndex)
        End Get
        Set(ByVal newValue As atcTimeseries)
            MyBase.Item(aIndex) = newValue
        End Set
    End Property

    ''' <summary>atcTimeseries by index</summary>
    Public Shadows Property ItemByIndex(ByVal aIndex As Integer) As atcTimeseries
        Get
            Return MyBase.Item(aIndex)
        End Get
        Set(ByVal newValue As atcTimeseries)
            MyBase.Item(aIndex) = newValue
        End Set
    End Property

    ''' <summary>atcTimeseries by key</summary>
    Public Shadows Property ItemByKey(ByVal aKey As Object) As atcTimeseries
        Get
            Return MyBase.ItemByKey(aKey)
        End Get
        Set(ByVal newValue As atcTimeseries)
            MyBase.ItemByKey(aKey) = newValue
        End Set
    End Property

    Public Shadows Function FindData(ByVal aAttributeName As String, ByVal aValue As String, Optional ByVal aLimit As Integer = 0) As atcTimeseriesGroup
        Return New atcTimeseriesGroup(MyBase.FindData(aAttributeName, aValue, aLimit))
    End Function

    Public Shadows Function FindData(ByVal aAttributeName As String, ByVal aValues As atcCollection, Optional ByVal aLimit As Integer = 0) As atcTimeseriesGroup
        Return New atcTimeseriesGroup(MyBase.FindData(aAttributeName, aValues, aLimit))
    End Function
End Class
