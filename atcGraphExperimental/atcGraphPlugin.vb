Imports atcData
Imports MapWinUtility

Public Class atcGraphPlugin
    Inherits atcData.atcDataDisplay

    Private pGraphTypeNames() As String = {"Timeseries", _
                                           "Flow/Duration", _
                                           "Running Sum", _
                                           "Residual (TS2 - TS1)", _
                                           "Cumulative Difference", _
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
            With lChooseForm.lstChooseGraphs
                Dim lItemIndex As Integer
                '.Items.AddRange(pGraphTypeNames)
                If lDataGroup.Count < 1 Then
                    .Items.Add("No data selected, cannot graph")
                    lChooseForm.btnGenerate.Visible = False
                Else
                    .Items.Add(pGraphTypeNames(0))
                    .Items.Add(pGraphTypeNames(1))
                    .Items.Add(pGraphTypeNames(2))
                    If lDataGroup.Count = 2 Then
                        .Items.Add(pGraphTypeNames(3))
                        .Items.Add(pGraphTypeNames(4))
                        .Items.Add(pGraphTypeNames(5))
                    End If
                End If

                For Each lCheckedName As String In GetSetting("BASINS4", "Graph", "ChooseGraphs", "").Split(","c)
                    lItemIndex = .Items.IndexOf(lCheckedName)
                    If lItemIndex > -1 Then .SetItemChecked(lItemIndex, True)
                Next

                lChooseForm.atcDataGroupDates.DataGroup = lDataGroup

                If lChooseForm.ShowDialog() = Windows.Forms.DialogResult.OK Then
                    lDataGroup = lChooseForm.atcDataGroupDates.CreateSelectedDataGroupSubset
                    Dim lAllCheckedItemNames As String = ""
                    For Each lGraphTypeName As String In .CheckedItems
                        lAllCheckedItemNames &= lGraphTypeName & ","
                        Dim lForm As New atcGraphForm()
                        Dim lGrapher As clsGraphBase = GetGraphType(lGraphTypeName, lDataGroup, lForm.ZedGraphCtrl)
                        If lGrapher Is Nothing Then
                            lForm.Dispose()
                        Else
                            lForm.Grapher = lGrapher
                            lForm.Show()
                        End If
                    Next
                    SaveSetting("BASINS4", "Graph", "ChooseGraphs", lAllCheckedItemNames)
                End If
            End With
        End If
    End Function

    <CLSCompliant(False)> _
    Public Function GetGraphType(ByVal aGraphTypeName As String, _
                                 ByVal aDataGroup As atcDataGroup, _
                                 ByVal aZedGraphCtrl As ZedGraph.ZedGraphControl) As clsGraphBase
        Dim lIndex As Integer = Array.IndexOf(pGraphTypeNames, aGraphTypeName)
        Select Case lIndex
            Case -1 : Throw New ApplicationException("Unknown graph type requested: " & aGraphTypeName)
            Case 0 'timeseries
                Return New clsGraphTime(aDataGroup, aZedGraphCtrl)
            Case 1 'Flow/Duration
                Return New clsGraphProbability(aDataGroup, aZedGraphCtrl)
            Case 2 'Running Sum
                Return New clsGraphRunningSum(aDataGroup, aZedGraphCtrl)
            Case 3 'Residual
                Return New clsGraphResidual(aDataGroup, aZedGraphCtrl)
            Case 4 'Cumulative Difference
                Return New clsGraphCumulativeDifference(aDataGroup, aZedGraphCtrl)
            Case 5 'Scatter
                If aDataGroup.Count = 2 Then
                    Return New clsGraphScatter(aDataGroup, aZedGraphCtrl)
                Else
                    Logger.Msg("ScatterGraph requires 2 timeseries, " & aDataGroup.Count & " specified")
                End If
        End Select
        Throw New ApplicationException("Unable to create graph: " & aGraphTypeName)
    End Function

End Class
