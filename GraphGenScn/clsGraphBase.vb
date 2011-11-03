Imports atcData
Imports ZedGraph

Public Class clsGraphBase
    Implements IDisposable

    Friend WithEvents pDataGroup As atcTimeseriesGroup 'The group of atcData displayed
    Friend WithEvents pZgc As ZedGraphControl    'The control used to display the data

    <CLSCompliant(False)> _
    Public Sub New(ByVal aDataGroup As atcTimeseriesGroup, ByVal aZedgraphControl As ZedGraphControl)
        Me.ZedGraphCtrl = aZedgraphControl
        Me.Datasets = aDataGroup
    End Sub

    Overridable Property Datasets() As atcTimeseriesGroup
        Get
            Return pDataGroup
        End Get
        Set(ByVal newValue As atcTimeseriesGroup)
            pDataGroup = newValue
            If Not pZgc Is Nothing AndAlso Not pZgc.IsDisposed AndAlso Not pZgc.MasterPane Is Nothing Then
                For Each lPane As ZedGraph.GraphPane In pZgc.MasterPane.PaneList
                    lPane.CurveList.Clear()
                Next
            End If
        End Set
    End Property

    <CLSCompliant(False)> _
    Public Property ZedGraphCtrl() As ZedGraphControl
        Get
            Return pZgc
        End Get
        Set(ByVal newValue As ZedGraphControl)
            pZgc = newValue
        End Set
    End Property

    Overridable Sub pDataGroup_Added(ByVal aAdded As atcUtility.atcCollection) Handles pDataGroup.Added
        Datasets = pDataGroup
    End Sub

    Overridable Sub pDataGroup_Removed(ByVal aRemoved As atcUtility.atcCollection) Handles pDataGroup.Removed
        Datasets = pDataGroup
    End Sub

    Protected Overrides Sub Finalize()
        pDataGroup = Nothing
        pZgc = Nothing
    End Sub

#Region " IDisposable Support "
    Private pDisposed As Boolean = False        ' To detect redundant calls

    Public Sub Dispose() Implements IDisposable.Dispose
        If Not Me.pDisposed Then
            Me.pDisposed = True
            Me.Finalize()
            GC.SuppressFinalize(Me)
        End If
    End Sub
#End Region

End Class
