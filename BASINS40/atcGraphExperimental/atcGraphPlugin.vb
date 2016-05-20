Imports atcData
Imports atcUtility
Imports MapWinUtility

Public Class atcGraphPlugin
    Inherits atcData.atcDataDisplay
    Private pIcon As System.Drawing.Icon = Nothing

    Private pGraphTypeNames() As String = {"Time Series", _
                                           "Flow/Duration", _
                                           "Frequency", _
                                           "Running Sum", _
                                           "Residual (TS2 - TS1)", _
                                           "Cumulative Difference", _
                                           "Scatter (TS2 vs TS1)", _
                                           "Double-Mass Curve", _
                                           "Shared Start Year"}

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
        Dim lIcon As System.Drawing.Icon = Nothing
        Return Show(aTimeseriesGroup, lIcon)
    End Function

    Private Function HasAnnual(ByVal aTimeseriesGroup As atcDataGroup) As Boolean
        'check to see if any annual timeseries have already been computed by atcTimeseriesNdayHighLow
        For Each lTimeseries As atcTimeseries In aTimeseriesGroup
            If lTimeseries.Attributes.GetValue("Time Unit", atcTimeUnit.TUDay) = atcTimeUnit.TUYear Then
                Return True
            End If
            For Each lAttribute As atcDefinedValue In lTimeseries.Attributes
                Try
                    If lAttribute.Arguments IsNot Nothing Then
                        For Each lArgument As atcData.atcDefinedValue In lAttribute.Arguments
                            If lArgument.Value.GetType.Name = "atcTimeseries" Then
                                Dim lAnnualTS As atcTimeseries = lArgument.Value
                                If lAnnualTS.Attributes.GetValue("Time Unit") = atcTimeUnit.TUYear Then
                                    Return True
                                End If
                            End If
                        Next
                    End If
                Catch
                End Try
            Next
        Next
        Return False
    End Function

    Public Overrides Function Show(ByVal aTimeseriesGroup As atcDataGroup, ByVal aIcon As System.Drawing.Icon) As Object
        pIcon = aIcon

        Show = Nothing

        Dim lTimeseriesGroup As atcTimeseriesGroup = aTimeseriesGroup
        If lTimeseriesGroup Is Nothing Then lTimeseriesGroup = New atcTimeseriesGroup
        If lTimeseriesGroup.Count = 0 Then 'ask user to specify some Data
            lTimeseriesGroup = atcDataManager.UserSelectData("Select Data To Graph", lTimeseriesGroup, Nothing, True, True, pIcon)
        End If

        If lTimeseriesGroup.Count > 0 Then
            Dim lChooseForm As New frmChooseGraphs
            If pIcon IsNot Nothing Then lChooseForm.Icon = aIcon
            With lChooseForm.lstChooseGraphs
                Dim lItemIndex As Integer
                '.Items.AddRange(pGraphTypeNames)
                If lTimeseriesGroup.Count < 1 Then
                    .Items.Add("No data selected, cannot graph")
                    lChooseForm.btnGenerate.Visible = False
                Else
                    .Items.Add(pGraphTypeNames(0))
                    .Items.Add(pGraphTypeNames(1))
                    If HasAnnual(lTimeseriesGroup) Then
                        .Items.Add(pGraphTypeNames(2))
                    Else
                        .Items.Add(pGraphTypeNames(2) & " (needs annual time series)")
                    End If
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
                    .Items.Add(pGraphTypeNames(7))
                End If

                For Each lCheckedName As String In GetSetting("BASINS41", "Graph", "ChooseGraphs", "").Split(","c)
                    lItemIndex = .Items.IndexOf(lCheckedName)
                    If lItemIndex > -1 Then .SetItemChecked(lItemIndex, True)
                Next

                If lChooseForm.ShowDialog() = Windows.Forms.DialogResult.OK Then
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
                            Show = Me.Show(lGroup, lGraphTypeName)
                        Next
                    Next
                    SaveSetting("BASINS41", "Graph", "ChooseGraphs", lAllCheckedItemNames)
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
            If pIcon IsNot Nothing Then lForm.Icon = pIcon
            lForm.Show()
            Return lForm
        End If
    End Function

    <CLSCompliant(False)> _
    Private Function GetGraphType(ByVal aGraphTypeName As String, _
                                 ByVal aDataGroup As atcTimeseriesGroup, _
                                 ByVal aGraphForm As atcGraphForm) As clsGraphBase
        aGraphForm.Text = aGraphTypeName & " Graph"
        Select Case aGraphTypeName
            Case "Timeseries", "Time-Series", "Time Series"
                aGraphForm.Text = "Time-Series Graph"
                Return New clsGraphTime(aDataGroup, aGraphForm.ZedGraphCtrl)
            Case "Shared Start Year"
                Return New clsGraphTime(MakeCommonStartYear(aDataGroup, 0), aGraphForm.ZedGraphCtrl)
            Case "Flow/Duration"
                Return New clsGraphProbability(aDataGroup, aGraphForm.ZedGraphCtrl)
            Case "Frequency"
                Return New clsGraphFrequency(aDataGroup, aGraphForm.ZedGraphCtrl)
            Case "Running Sum"
                Return New clsGraphRunningSum(aDataGroup, aGraphForm.ZedGraphCtrl)
            Case "Double-Mass Curve"
                Return New clsGraphDoubleMass(aDataGroup, aGraphForm.ZedGraphCtrl)
            Case "Residual (TS2 - TS1)"
                aGraphForm.Text = "Residual Graph"
                Return New clsGraphResidual(aDataGroup, aGraphForm.ZedGraphCtrl)
            Case "Cumulative Difference"
                Return New clsGraphCumulativeDifference(aDataGroup, aGraphForm.ZedGraphCtrl)
            Case "Scatter (TS2 vs TS1)"
                aGraphForm.Text = "Scatter Plot"
                Return New clsGraphScatter(aDataGroup, aGraphForm.ZedGraphCtrl)
        End Select
        Logger.Dbg("Unable to create graph: " & aGraphTypeName)
        Return Nothing
    End Function

End Class
