Imports System.Drawing

Imports atcData
Imports atcUtility
Imports ZedGraph

Public Class clsGraphProbability
    Inherits clsGraphBase

    Private Const pNumProbabilityPoints As Integer = 200

    <CLSCompliant(False)> _
    Public Sub New(ByVal aDataGroup As atcTimeseriesGroup, ByVal aZedGraphControl As ZedGraphControl)
        MyBase.New(aDataGroup, aZedGraphControl)
    End Sub

    Public Overrides Property Datasets() As atcTimeseriesGroup
        Get
            Return MyBase.Datasets
        End Get
        Set(ByVal aDataGroup As atcTimeseriesGroup)
            MyBase.Datasets = aDataGroup
            Dim lCommonTimeUnits As Integer = aDataGroup.CommonAttributeValue("Time Units", -1)
            Dim lCommonTimeStep As Integer = aDataGroup.CommonAttributeValue("Time Step", -1)
            Dim lCommonScenario As String = aDataGroup.CommonAttributeValue("Scenario", "")
            Dim lCommonLocation As String = aDataGroup.CommonAttributeValue("Location", "")
            Dim lCommonConstituent As String = aDataGroup.CommonAttributeValue("Constituent", "")
            Dim lCommonUnits As String = aDataGroup.CommonAttributeValue("Units", "")
            Dim lCommonTimeUnitName As String = TimeUnitName(lCommonTimeUnits, lCommonTimeStep)
            For Each lTimeseries As atcTimeseries In aDataGroup
                AddDatasetCurve(lTimeseries, lCommonTimeUnitName, lCommonScenario, lCommonConstituent, lCommonLocation, lCommonUnits)
            Next
            AxisTitlesFromCommonAttributes(pZgc.MasterPane.PaneList(0), lCommonTimeUnitName, lCommonScenario, lCommonConstituent, lCommonLocation, lCommonUnits)
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
        With lPane.XAxis
            If .Type <> AxisType.Probability Then
                .Type = AxisType.Probability
                With .MajorTic
                    .IsInside = True
                    .IsCrossInside = True
                    .IsOutside = False
                    .IsCrossOutside = False
                End With                
                Dim lProbScale As ProbabilityScale = .Scale
                lProbScale.standardDeviations = 3
                'lProbScale.LabelStyle = ProbabilityScale.ProbabilityLabelStyle.ReturnInterval
                'lProbScale.IsReverse = True
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
        'Dim lXSd() As Double
        'ReDim lXSd(lLastIndex)
        ReDim lXFracExceed(lLastIndex)

        For lIndex = 0 To lLastIndex
            'lXSd(lIndex) = Gausex(lX(lIndex) / 100)
            lXFracExceed(lIndex) = (100 - lX(lIndex)) / 100
            lAttributeName = "%" & Format(lX(lIndex), "00.####")
            lY(lIndex) = aTimeseries.Attributes.GetValue(lAttributeName)
            'Logger.Dbg(lAttributeName & " = " & lY(lIndex) & _
            '                            " : " & lX(lIndex) & _
            '                            " : " & lXFracExceed(lIndex))
        Next
        With lPane.XAxis
            '.Scale.Max = lXFracExceed(0)
            '.Scale.Min = lXFracExceed(lLastIndex)
            .Scale.BaseTic = lXFracExceed(0)
            .Title.Text = "Percent Exceeded"
        End With
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
