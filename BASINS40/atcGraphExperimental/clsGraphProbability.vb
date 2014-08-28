Imports System.Drawing

Imports atcData
Imports atcUtility
Imports ZedGraph

Public Class clsGraphProbability
    Inherits clsGraphBase

    ''' <summary>
    ''' True  for Non-Exceedance graph
    ''' False for Exceedance     graph
    ''' Default is Exceedance
    ''' </summary>
    ''' <remarks>Because New runs before setting private member, 
    ''' must set nonExceedance to false to default to exceedance graph
    ''' </remarks>
    Private pNonExceedance As Boolean = False

    Private Const pNumProbabilityPoints As Integer = 200

    <CLSCompliant(False)> _
    Public Sub New(ByVal aDataGroup As atcTimeseriesGroup, ByVal aZedGraphControl As ZedGraphControl)
        MyBase.New(aDataGroup, aZedGraphControl)
    End Sub

    Public Property Exceedance() As Boolean
        Get
            Return Not pNonExceedance
            'Try
            '    Dim lProbScale As ProbabilityScale = pZgc.MasterPane.PaneList(0).XAxis.Scale
            '    Return lProbScale.Exceedance
            'Catch e As Exception
            '    MapWinUtility.Logger.Dbg("Could not get scale exceedance: " & e.ToString)
            '    Return False
            'End Try
        End Get
        Set(ByVal value As Boolean)
            Try
                If value = pNonExceedance Then
                    pNonExceedance = Not value
                    Dim lCurves As New Generic.List(Of CurveItem)
                    lCurves.AddRange(MyBase.pZgc.MasterPane.PaneList.Item(0).CurveList)
                    If MyBase.pZgc.MasterPane.PaneList.Count > 1 Then
                        lCurves.AddRange(MyBase.pZgc.MasterPane.PaneList.Item(1).CurveList)
                    End If

                    For Each lCurve As CurveItem In lCurves
                        If lCurve.IsLine Then
                            Dim lLine As LineItem = lCurve
                            If pNonExceedance OrElse Not lLine.Line.IsVisible Then
                                For lPointIndex As Integer = 0 To lCurve.NPts - 1
                                    lCurve.Points(lPointIndex).X = 1 - lCurve.Points(lPointIndex).X
                                Next
                            End If
                        End If
                    Next

                    With pZgc.MasterPane.PaneList(0).XAxis.Title
                        If pNonExceedance Then
                            .Text = .Text.Replace(" Exceeded", " Not-Exceeded")
                        Else
                            .Text = .Text.Replace(" Not-Exceeded", " Exceeded")
                        End If
                    End With
                    pZgc.Refresh()
                End If
                '    Dim lProbScale As ProbabilityScale = pZgc.MasterPane.PaneList(0).XAxis.Scale
                '    If value <> lProbScale.Exceedance Then
                '        lProbScale.Exceedance = value
                '        With pZgc.MasterPane.PaneList(0).XAxis.Title
                '            If value Then
                '                .Text = .Text.Replace(" Not-Exceeded", " Exceeded")
                '            Else
                '                .Text = .Text.Replace(" Exceeded", " Not-Exceeded")
                '            End If
                '        End With
                '        pZgc.Refresh()
                '    End If
            Catch e As Exception
                MapWinUtility.Logger.Dbg("Could not set scale exceedance: " & e.ToString)
            End Try
        End Set
    End Property

    Public Overrides Property Datasets() As atcTimeseriesGroup
        Get
            Return MyBase.Datasets
        End Get
        Set(ByVal aDataGroup As atcTimeseriesGroup)
            MyBase.Datasets = aDataGroup
            Dim lCommonTimeUnit As Integer = aDataGroup.CommonAttributeValue("Time Unit", -1)
            Dim lCommonTimeStep As Integer = aDataGroup.CommonAttributeValue("Time Step", -1)
            Dim lCommonScenario As String = aDataGroup.CommonAttributeValue("Scenario", "")
            Dim lCommonLocation As String = aDataGroup.CommonAttributeValue("Location", "")
            Dim lCommonConstituent As String = aDataGroup.CommonAttributeValue("Constituent", "")
            Dim lCommonUnits As String = aDataGroup.CommonAttributeValue("Units", "")
            Dim lCommonTimeUnitName As String = TimeUnitName(lCommonTimeUnit, lCommonTimeStep)
            For Each lTimeseries As atcTimeseries In aDataGroup
                AddDatasetCurve(lTimeseries, lCommonTimeUnitName, lCommonScenario, lCommonConstituent, lCommonLocation, lCommonUnits)
            Next
            AxisTitlesFromCommonAttributes(pZgc.MasterPane.PaneList(0), lCommonTimeUnitName, lCommonScenario, lCommonConstituent, lCommonLocation, lCommonUnits)
            If lCommonConstituent = "GW LEVEL" Then
                ScaleAxis(Datasets, pZgc.MasterPane.PaneList(0).YAxis)
            End If
            pZgc.Refresh()
        End Set
    End Property

    Overrides Sub pDataGroup_Added(ByVal aAdded As atcUtility.atcCollection)
        For Each lTimeseries As atcTimeseries In aAdded
            AddDatasetCurve(lTimeseries)
        Next
        pZgc.Refresh()
    End Sub

    Overrides Sub pDataGroup_Removed(ByVal aRemoved As atcUtility.atcCollection)
        If aRemoved IsNot Nothing Then
            Dim lCurveList As ZedGraph.CurveList = MyBase.pZgc.MasterPane.PaneList.Item(0).CurveList

            Dim lCurveListAux As ZedGraph.CurveList = Nothing
            If MyBase.pZgc.MasterPane.PaneList.Count > 1 Then
                lCurveListAux = MyBase.pZgc.MasterPane.PaneList.Item(1).CurveList
            End If
            Dim lCurve As ZedGraph.CurveItem
            For Each lTs As atcTimeseries In aRemoved
                lCurve = lCurveList.Item(TSCurveLabel(lTs))
                If lCurve Is Nothing Then
                    If Not lCurveListAux Is Nothing Then
                        lCurve = lCurveListAux.Item(TSCurveLabel(lTs))
                        If Not lCurve Is Nothing Then
                            lCurveListAux.Remove(lCurve)
                        End If
                    End If
                Else
                    lCurveList.Remove(lCurve)
                End If
            Next
            pZgc.Refresh()
        End If
    End Sub

    Private Sub AddDatasetCurve(ByVal aTimeseries As atcTimeseries, _
                        Optional ByVal aCommonTimeUnitName As String = Nothing, _
                        Optional ByVal aCommonScenario As String = Nothing, _
                        Optional ByVal aCommonConstituent As String = Nothing, _
                        Optional ByVal aCommonLocation As String = Nothing, _
                        Optional ByVal aCommonUnits As String = Nothing)
        Dim lScen As String = aTimeseries.Attributes.GetValue("scenario")
        Dim lLoc As String = aTimeseries.Attributes.GetValue("location")
        Dim lCons As String = aTimeseries.Attributes.GetValue("constituent")
        Dim lCurveLabel As String = TSCurveLabel(aTimeseries, aCommonTimeUnitName, aCommonScenario, aCommonConstituent, aCommonLocation, aCommonUnits)
        Dim lCurveColor As Color = GetMatchingColor(lScen & ":" & lLoc & ":" & lCons)

        Dim lCurve As LineItem = Nothing

        Dim lX(pNumProbabilityPoints) As Double
        Dim lLastIndex As Integer = lX.GetUpperBound(0)
        Dim lPane As ZedGraph.GraphPane = pZgc.MasterPane.PaneList(0)
        Dim lXScale As ProbabilityScale
        With lPane.XAxis
            If .Type <> AxisType.Probability Then
                .Type = AxisType.Probability
                With .MajorTic
                    .IsInside = True
                    .IsCrossInside = True
                    .IsOutside = False
                    .IsCrossOutside = False
                End With
                lXScale = .Scale
                lXScale.standardDeviations = 3
                'lXScale.IsReverse = True
            End If

            For lXindex As Integer = 0 To lLastIndex
                lX(lXindex) = 100 * .Scale.DeLinearize(lXindex / CDbl(lLastIndex))
            Next
        End With
        Dim lAttributeName As String
        Dim lIndex As Integer
        Dim lXFracExceed() As Double
        Dim lY() As Double

        ReDim lY(lLastIndex)
        ReDim lXFracExceed(lLastIndex)

        For lIndex = 0 To lLastIndex
            If Exceedance Then
                lXFracExceed(lIndex) = (100 - lX(lIndex)) / 100
            Else
                lXFracExceed(lIndex) = lX(lIndex) / 100
            End If
            lAttributeName = "%" & Format(lX(lIndex), "00.####")
            lY(lIndex) = aTimeseries.Attributes.GetValue(lAttributeName)
            'Logger.Dbg(lAttributeName & " = " & lY(lIndex) & _
            '                            " : " & lX(lIndex) & _
            '                            " : " & lXFracExceed(lIndex))
        Next
        lXScale = lPane.XAxis.Scale
        lXScale.BaseTic = lXFracExceed(0)
        If Exceedance Then
            lPane.XAxis.Title.Text = "Percent Exceeded"
        Else
            lPane.XAxis.Title.Text = "Percent Not-Exceeded"
        End If
        With lPane.YAxis
            .Type = AxisType.Log
            .Scale.IsUseTenPower = False
            '.Scale.Min = 10
            If aTimeseries.Attributes.ContainsAttribute("Units") Then
                .Title.Text = aTimeseries.Attributes.GetValue("Units")
                .Title.IsVisible = True
            End If
        End With

        'Upper right corner of chart is better for this graph type
        lPane.Legend.Location = New Location(0.95, 0.05, CoordType.ChartFraction, AlignH.Right, AlignV.Top)
        lPane.Legend.FontSpec.Size += 2
        lPane.Legend.FontSpec.IsBold = True

        lCurve = lPane.AddCurve(lCurveLabel, lXFracExceed, lY, lCurveColor, SymbolType.None)
        lCurve.Line.Width = 2

        lCurve.Line.StepType = StepType.NonStep
        SetYMax(lPane)
    End Sub

    Private Sub SetYMax(ByVal aPane As ZedGraph.GraphPane)
        Dim lYMax As Double = 0.0001
        For Each lCurve As ZedGraph.CurveItem In aPane.CurveList
            For lPointIndex As Integer = 0 To lCurve.NPts - 1
                lYMax = Math.Max(lYMax, lCurve.Points(lPointIndex).Y)
            Next
        Next
        aPane.YAxis.Scale.MaxAuto = False
        aPane.YAxis.Scale.Max = Math.Pow(10, Math.Ceiling(Math.Log10(lYMax)))
    End Sub
End Class
