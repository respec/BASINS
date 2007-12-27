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
                    lForm.DataSets = lGrapher.Datasets
                    lForm.Show()
                End If
            Next
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
            Case 1 'Residual
                If aDataGroup.Count = 2 Then
                    Dim lArgsMath As New atcDataAttributes
                    Dim lTsMath As atcDataSource = New atcTimeseriesMath.atcTimeseriesMath
                    lTsMath.DataSets.Clear()
                    lArgsMath.Clear()
                    lArgsMath.SetValue("timeseries", aDataGroup)
                    lArgsMath.SetValue("number", Double.NaN)  'TODO: kludge, find a better way!
                    If lTsMath.Open("subtract", lArgsMath) Then
                        Dim lGraphTime As New clsGraphTime(lTsMath.DataSets, aZedGraphCtrl)
                        lGraphTime.ZedGraphCtrl.MasterPane(0).CurveList(0).Label.Text = "Residual"
                        Return lGraphTime
                    Else
                        Logger.Msg("ResidualGraph Calculation Failed")
                    End If
                Else
                    Logger.Msg("ResidualGraph requires 2 timeseries, " & aDataGroup.Count & " specified")
                End If
            Case 2 'Cumulative Difference
                If aDataGroup.Count = 2 Then
                    Dim lArgsMath As New atcDataAttributes
                    Dim lTsMath As atcDataSource = New atcTimeseriesMath.atcTimeseriesMath
                    lTsMath.DataSets.Clear()
                    lArgsMath.Clear()
                    lArgsMath.SetValue("timeseries", aDataGroup)
                    lArgsMath.SetValue("number", Double.NaN)  'TODO: kludge, find a better way!
                    If lTsMath.Open("subtract", lArgsMath) Then
                        lArgsMath.Clear()
                        lArgsMath.SetValue("timeseries", lTsMath.DataSets)
                        Dim lTsRunSum As atcDataSource = New atcTimeseriesMath.atcTimeseriesMath
                        If lTsRunSum.Open("running sum", lArgsMath) Then
                            Dim lGraphTime As New clsGraphTime(lTsRunSum.DataSets, aZedGraphCtrl)
                            lGraphTime.ZedGraphCtrl.MasterPane(0).CurveList(0).Label.Text = "Cummulative Difference"
                            Return lGraphTime
                        Else
                            Logger.Msg("CumulativeDifferenceGraph Accumulation Calculation Failed")
                        End If
                    Else
                        Logger.Msg("CumulativeDifferenceGraph Difference Calculation Failed")
                    End If
                Else
                    Logger.Msg("Cumulative Difference requires 2 timeseries, " & aDataGroup.Count & " specified")
                End If
            Case 3 'Flow Duration
                Return New clsGraphProbability(aDataGroup, aZedGraphCtrl)
            Case 4 'Scatter
                    If aDataGroup.Count = 2 Then
                        Return New clsGraphScatter(aDataGroup, aZedGraphCtrl)
                    Else
                        Logger.Msg("ScatterGraph requires 2 timeseries, " & aDataGroup.Count & " specified")
                    End If
        End Select
        Throw New ApplicationException("Unable to create graph: " & aGraphTypeName)
    End Function

End Class
