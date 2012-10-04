Imports atcUtility
Imports MapWindow.Interfaces
Imports MapWinUtility
Imports System.IO
Imports System.Text
Imports System.Collections.Specialized
Imports atcData

Public Enum BFMethods
    HySEPFixed = 1
    HySEPLocMin = 2
    HySEPSlide = 3
    PART = 4
    BFIStandard = 5
    BFIModified = 6
End Enum

Public Interface IclsBaseflow
    Function DoBaseFlowSeparation() As atcTimeseriesGroup
    Sub Clear()
End Interface

Public MustInherit Class clsBaseflow
    Implements IclsBaseflow

    Private pDataSource As atcDataSource = Nothing
    Private pDataGroup As atcTimeseriesGroup = Nothing

    Private pTargetTS As atcTimeseries
    Public Property TargetTS() As atcTimeseries
        Get
            Return pTargetTS
        End Get
        Set(ByVal value As atcTimeseries)
            pTargetTS = value
        End Set
    End Property

    Public Sub New()
    End Sub

    Public Sub New(ByVal aDataGroup As atcTimeseriesGroup)
        pDataGroup = aDataGroup
        'TargetTS = GetTargetTS()
    End Sub

    Public Sub New(ByVal aDataSource As atcDataSource)
        pDataSource = aDataSource
        If pDataSource.Open(pDataSource.Specification) Then
            pDataGroup = pDataSource.DataSets
        End If
    End Sub

    'if metric unit (UnitFlag=2), then convert drainage area from square mile to square kilometer by a factor of 2.59
    Private pDrainageArea As Double = 0.0
    Public Property DrainageArea() As Double
        Get
            Return pDrainageArea
        End Get
        Set(ByVal value As Double)
            pDrainageArea = value
        End Set
    End Property

    Private pStartDate As Double = 0.0
    Public Property StartDate() As Double
        Get
            Return pStartDate
        End Get
        Set(ByVal value As Double)
            pStartDate = value
        End Set
    End Property

    Private pEndDate As Double = 0.0
    Public Property EndDate() As Double
        Get
            Return pEndDate
        End Get
        Set(ByVal value As Double)
            pEndDate = value
        End Set
    End Property

    'unit flag 1 for English, 2 for metric
    'if metric, sq mi => sq km
    'if metric, cfs => cms
    Private pUnitFlag As Integer
    Public Property UnitFlag() As Integer
        Get
            Return pUnitFlag
        End Get
        Set(ByVal value As Integer)
            pUnitFlag = value
        End Set
    End Property

    Public MustOverride Function DoBaseFlowSeparation() As atcData.atcTimeseriesGroup Implements IclsBaseflow.DoBaseFlowSeparation
    Public MustOverride Sub Clear() Implements IclsBaseflow.Clear
End Class
