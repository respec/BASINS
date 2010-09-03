Imports atcData
Imports atcUtility
Imports MapWinUtility

Public Class atcGraphPlugin
    Inherits atcData.atcDataDisplay

    Private pGraphTypeNames() As String = {"Timeseries", _
                                           "Flow/Duration", _
                                           "Frequency", _
                                           "Running Sum", _
                                           "Residual (TS2 - TS1)", _
                                           "Cumulative Difference", _
                                           "Scatter (TS2 vs TS1)"}

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Analysis::Graph"
        End Get
    End Property

    Public Overrides Sub Save(ByVal aTimeseriesGroup As atcData.atcDataGroup, _
                              ByVal aFileName As String, _
                              ByVal ParamArray aOption() As String)

        If Not aTimeseriesGroup Is Nothing AndAlso aTimeseriesGroup.Count > 0 Then
            'Dim lForm As New atcGraphForm()
            'TODO: set up a graph to save
            'lForm.SaveBitmapToFile(aFileName)
            'lForm.Dispose()
        End If
    End Sub

    Public Overrides Function Show(ByVal aTimeseriesGroup As atcDataGroup) As Object
        Show = Nothing

        Dim lTimeseriesGroup As atcTimeseriesGroup = aTimeseriesGroup
        If lTimeseriesGroup Is Nothing Then lTimeseriesGroup = New atcTimeseriesGroup
        If lTimeseriesGroup.Count = 0 Then 'ask user to specify some Data
            lTimeseriesGroup = atcDataManager.UserSelectData("Select Data To Graph", lTimeseriesGroup)
        End If

        If lTimeseriesGroup.Count > 0 Then
            Dim lChooseForm As New frmChooseGraphs
            With lChooseForm.lstChooseGraphs
                Dim lItemIndex As Integer
                '.Items.AddRange(pGraphTypeNames)
                If lTimeseriesGroup.Count < 1 Then
                    .Items.Add("No data selected, cannot graph")
                    lChooseForm.btnGenerate.Visible = False
                Else
                    .Items.Add(pGraphTypeNames(0))
                    .Items.Add(pGraphTypeNames(1))
                    .Items.Add(pGraphTypeNames(2))
                    .Items.Add(pGraphTypeNames(3))
                    Dim lNeededTwoSuffix As String
                    If lTimeseriesGroup.Count = 2 Then
                        lNeededTwoSuffix = ""
                    Else
                        lNeededTwoSuffix = " (two datasets needed but " & lTimeseriesGroup.Count & " datasets selected)"
                    End If
                    .Items.Add(pGraphTypeNames(4) & lNeededTwoSuffix)
                    .Items.Add(pGraphTypeNames(5) & lNeededTwoSuffix)
                    .Items.Add(pGraphTypeNames(6) & lNeededTwoSuffix)
                End If

                For Each lCheckedName As String In GetSetting("BASINS4", "Graph", "ChooseGraphs", "").Split(","c)
                    lItemIndex = .Items.IndexOf(lCheckedName)
                    If lItemIndex > -1 Then .SetItemChecked(lItemIndex, True)
                Next

                'lChooseForm.atcDataGroupDates.DataGroup = lDataGroup

                If lChooseForm.ShowDialog() = Windows.Forms.DialogResult.OK Then
                    'lDataGroup = lChooseForm.atcDataGroupDates.CreateSelectedDataGroupSubset

                    Dim lTimeseriesGroups As New Collection
                    If lChooseForm.cbxMultiple.Checked Then
                        'need multiple wq plots, split ltimeseriesgroup into groups by constituent
                        Dim lCons As New atcCollection
                        For Each lTimSer As atcTimeseries In lTimeseriesGroup
                            lCons.Add(lTimSer.Attributes.GetValue("Constituent"))
                        Next
                        For Each lCon As String In lCons
                            Dim lGroup As New atcTimeseriesGroup
                            For Each lTimSer As atcTimeseries In lTimeseriesGroup
                                If lTimSer.Attributes.GetValue("Constituent") = lCon Then
                                    lGroup.Add(lTimSer)
                                End If
                            Next
                            lTimeseriesGroups.Add(lGroup)
                        Next
                    Else
                        'regular case
                        lTimeseriesGroups.Add(lTimeseriesGroup)
                    End If

                    Dim lAllCheckedItemNames As String = ""
                    For Each lGraphTypeName As String In .CheckedItems
                        lAllCheckedItemNames &= lGraphTypeName & ","
                        For Each lGroup As atcTimeseriesGroup In lTimeseriesGroups
                            Me.Show(lGroup, lGraphTypeName)
                        Next
                    Next
                    SaveSetting("BASINS4", "Graph", "ChooseGraphs", lAllCheckedItemNames)
                End If
            End With
        End If
    End Function

    Public Overloads Function Show(ByVal aTimeseriesGroup As atcDataGroup, ByVal aGraphTypeName As String) As Object
        Dim lForm As New atcGraphForm()
        Dim lGrapher As clsGraphBase = GetGraphType(aGraphTypeName, aTimeseriesGroup, lForm)
        If lGrapher Is Nothing Then
            lForm.Dispose()
            Return Nothing
        Else
            lForm.Grapher = lGrapher
            lForm.Show()
            Return lForm
        End If
    End Function

    <CLSCompliant(False)> _
    Private Function GetGraphType(ByVal aGraphTypeName As String, _
                                 ByVal aDataGroup As atcTimeseriesGroup, _
                                 ByVal aGraphForm As atcGraphForm) As clsGraphBase
        Select Case aGraphTypeName
            Case "Timeseries" : aGraphForm.Text = "Timeseries Graph"
                Return New clsGraphTime(aDataGroup, aGraphForm.ZedGraphCtrl)
            Case "Flow/Duration" : aGraphForm.Text = "Flow/Duration Graph"
                Return New clsGraphProbability(aDataGroup, aGraphForm.ZedGraphCtrl)
            Case "Frequency" : aGraphForm.Text = "Frequency Graph"
                Return New clsGraphFrequency(aDataGroup, aGraphForm.ZedGraphCtrl)
            Case "Running Sum" : aGraphForm.Text = "Running Sum Graph"
                Return New clsGraphRunningSum(aDataGroup, aGraphForm.ZedGraphCtrl)
            Case "Residual (TS2 - TS1)" : aGraphForm.Text = "Residual Graph"
                Return New clsGraphResidual(aDataGroup, aGraphForm.ZedGraphCtrl)
            Case "Cumulative Difference" : aGraphForm.Text = "Cumulative Difference Graph"
                Return New clsGraphCumulativeDifference(aDataGroup, aGraphForm.ZedGraphCtrl)
            Case "Scatter (TS2 vs TS1)" : aGraphForm.Text = "Scatter Plot"
                Return New clsGraphScatter(aDataGroup, aGraphForm.ZedGraphCtrl)
        End Select
        Logger.Dbg("Unable to create graph: " & aGraphTypeName)
        Return Nothing
    End Function

End Class
