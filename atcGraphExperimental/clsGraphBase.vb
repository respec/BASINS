Imports atcData
Imports ZedGraph

Public Class clsGraphBase

    Friend WithEvents pDataGroup As atcDataGroup 'The group of atcData displayed
    Friend WithEvents pZgc As ZedGraphControl    'The control used to display the data

    <CLSCompliant(False)> _
    Public Sub New(ByVal aDataGroup As atcDataGroup, ByVal aZedgraphControl As ZedGraphControl)
        Me.ZedGraphCtrl = aZedgraphControl
        Me.Datasets = aDataGroup
    End Sub

    Overridable Property Datasets() As atcDataGroup
        Get
            Return pDataGroup
        End Get
        Set(ByVal newValue As atcDataGroup)
            pDataGroup = newValue
            If Not pZgc Is Nothing Then
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
End Class
