Imports atcData
Imports MapWinUtility

Public Class atcGraphPlugin
    Inherits atcData.atcDataDisplay

    Private pGraphTypeNames() As String = {"Timeseries", _
                                           "Residual (TS2 - TS1)", _
                                           "Cumulative Difference", _
                                           "Flow/Duration", _
                                           "Scatter (TS2 vs TS1)"}

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Analysis::Graph"
        End Get
    End Property

    Public Overrides Sub Save(ByVal aDataGroup As atcData.atcDataGroup, _
                              ByVal aFileName As String, _
                              ByVal ParamArray aOption() As String)

        If Not aDataGroup Is Nothing AndAlso aDataGroup.Count > 0 Then
            'Dim lForm As New atcGraphForm()
            'TODO: set up a graph to save
            'lForm.SaveBitmapToFile(aFileName)
            'lForm.Dispose()
        End If
    End Sub

    Public Overrides Function Show(Optional ByVal aDataGroup As atcDataGroup = Nothing) As Object
        Show = Nothing

        Dim lDataGroup As atcDataGroup = aDataGroup
        If lDataGroup Is Nothing Then lDataGroup = New atcDataGroup
        If lDataGroup.Count = 0 Then 'ask user to specify some Data
            atcDataManager.UserSelectData("Select Data To Graph", lDataGroup, True)
        End If

        If lDataGroup.Count > 0 Then
            Dim lChooseForm As New frmChooseGraphs
            lChooseForm.lstChooseGraphs.Items.AddRange(pGraphTypeNames)
            lChooseForm.ShowDialog()

            For Each lGraphTypeName As String In lChooseForm.lstChooseGraphs.CheckedItems
                Dim lForm As New atcGraphForm()
                Dim lGrapher As clsGraphBase = GetGraphType(lGraphTypeName, lDataGroup, lForm.ZedGraphCtrl)
                If lGrapher Is Nothing Then
                    lForm.Dispose()
                Else
                    lForm.Grapher = lGrapher
                    lForm.DataSets = lDataGroup
                    lForm.Show()
                End If
            Next
        End If
    End Function

    <CLSCompliant(False)> _
    Public Function GetGraphType(ByVal aGraphTypeName As String, ByVal aDataGroup As atcDataGroup, ByVal aZedGraphCtrl As ZedGraph.ZedGraphControl) As clsGraphBase
        Dim lIndex As Integer = Array.IndexOf(pGraphTypeNames, aGraphTypeName)
        Select Case lIndex
            Case -1 : Throw New ApplicationException("Unknown graph type requested: " & aGraphTypeName)
            Case 0 : Return New clsGraphTime(aDataGroup, aZedGraphCtrl)
            Case 1 'Residual (TS2 - TS1)
            Case 2 'Cumulative Difference
            Case 3 : Return New clsGraphProbability(aDataGroup, aZedGraphCtrl)
            Case 4 : Return New clsGraphScatter(aDataGroup, aZedGraphCtrl)
        End Select
        Throw New ApplicationException("Unable to create graph: " & aGraphTypeName)
    End Function

End Class
