Imports System.Drawing

Imports atcData
Imports atcUtility
Imports ZedGraph

Public Class clsGraphProbability
    Inherits clsGraphBase

    Private pNumProbabilityPoints As Integer = 200

    <CLSCompliant(False)> _
    Public Sub New(ByVal aDataGroup As atcDataGroup, ByVal aZedGraphControl As ZedGraphControl)
        MyBase.New(aDataGroup, aZedGraphControl)
    End Sub

    Public Overrides Property Datasets() As atcDataGroup
        Get
            Return MyBase.Datasets
        End Get
        Set(ByVal newValue As atcDataGroup)
            MyBase.Datasets = newValue
            For Each lTimeseries As atcTimeseries In newValue
                AddDatasetCurve(lTimeseries)
            Next
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
        Dim lCurveList As ZedGraph.CurveList = MyBase.pZgc.MasterPane.PaneList.Item(0).CurveList
        Dim lCurveListAux As ZedGraph.CurveList = Nothing
        If MyBase.pZgc.MasterPane.PaneList.Count > 1 Then
            lCurveListAux = MyBase.pZgc.MasterPane.PaneList.Item(1).CurveList
        End If
        Dim lCurve As ZedGraph.CurveItem
        For Each lTs As atcTimeseries In aRemoved
            lCurve = lCurveList.Item(clsGraphTime.TSCurveLabel(lTs))
            If lCurve Is Nothing Then
                If Not lCurveListAux Is Nothing Then
                    lCurve = lCurveListAux.Item(clsGraphTime.TSCurveLabel(lTs))
                    If Not lCurve Is Nothing Then
                        lCurveListAux.Remove(lCurve)
                    End If
                End If
            Else
                lCurveList.Remove(lCurve)
            End If
        Next
        pZgc.Refresh()
    End Sub

    Private Sub AddDatasetCurve(ByVal aTimeseries As atcTimeseries)
        Dim lScen As String = aTimeseries.Attributes.GetValue("scenario")
        Dim lLoc As String = aTimeseries.Attributes.GetValue("location")
        Dim lCons As String = aTimeseries.Attributes.GetValue("constituent")
        Dim lCurveLabel As String = clsGraphTime.TSCurveLabel(aTimeseries)
        Dim lCurveColor As Color = GetMatchingColor(lScen & ":" & lLoc & ":" & lCons)
        Dim lCurve As LineItem = Nothing
        If pNumProbabilityPoints = 0 Then
            'TODO: why is this needed?
            pNumProbabilityPoints = 200
        End If
        Dim lX(pNumProbabilityPoints) As Double
        Dim lLastIndex As Integer = lX.GetUpperBound(0)
        Dim lProbScale As ZedGraph.ProbabilityScale
        Dim lPane As ZedGraph.GraphPane = MyBase.pZgc.MasterPane.PaneList(0)
        With lPane.XAxis
            If .Type <> AxisType.Probability Then
                .Type = AxisType.Probability
                With .MajorTic
                    .IsInside = True
                    .IsCrossInside = True
                    .IsOutside = False
                    .IsCrossOutside = False
                End With
            End If
            lProbScale = .Scale
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
            .Scale.Min = lXFracExceed(0)
            .Scale.Max = lXFracExceed(lLastIndex)
            .Scale.BaseTic = lXFracExceed(0)
            .Title.Text = "Percent chance exceeded"
        End With
        With lPane.YAxis
            .Type = AxisType.Log
            .Scale.IsUseTenPower = False
            If aTimeseries.Attributes.ContainsAttribute("Units") Then
                .Title.Text = aTimeseries.Attributes.GetValue("Units")
                .Title.IsVisible = True
            End If
        End With

        'Upper right corner of chart is better for this graph type
        lPane.Legend.Location = New Location(0.95, 0.05, CoordType.ChartFraction, AlignH.Right, AlignV.Top)

        lCurve = lPane.AddCurve(lCurveLabel, lXFracExceed, lY, lCurveColor, SymbolType.None)
        lCurve.Line.Width = 1
        lCurve.Line.StepType = StepType.NonStep
    End Sub

    'Private Function Gausex(ByVal aExprob As Double) As Double
    '    'GAUSSIAN PROBABILITY FUNCTIONS   W.KIRBY  JUNE 71
    '    ' GAUSEX=VALUE EXCEEDED WITH PROB EXPROB
    '    ' rev 8/96 by PRH for VB
    '    ' rev 11/2006 by MHG for c#
    '    ' rev 11/2007 by JLK for VB.NET
    '    Static c0 As Double = 2.515517
    '    Static c1 As Double = 0.802853
    '    Static c2 As Double = 0.010328
    '    Static d1 As Double = 1.432788
    '    Static d2 As Double = 0.189269
    '    Static d3 As Double = 0.001308
    '    Static StandardDeviations As Integer = 3
    '    Dim pr, rtmp, p, t, numerat, Denom As Double

    '    Try
    '        p = aExprob
    '        If (p >= 1) Then
    '            rtmp = -StandardDeviations 'set to minimum
    '        ElseIf (p <= 0) Then
    '            rtmp = StandardDeviations 'set at maximum
    '        Else          'compute value
    '            pr = p
    '            If (p > 0.5) Then pr = 1 - pr
    '            t = Math.Sqrt(-2 * Math.Log(pr))
    '            numerat = (c0 + t * (c1 + t * c2))
    '            Denom = (1 + t * (d1 + t * (d2 + t * d3)))
    '            rtmp = t - numerat / Denom
    '            If (p > 0.5) Then rtmp = -rtmp
    '            If (rtmp > StandardDeviations) Then rtmp = StandardDeviations
    '            If (rtmp < -StandardDeviations) Then rtmp = -StandardDeviations
    '            Return rtmp
    '        End If
    '    Catch e As Exception
    '        Return 0
    '    End Try
    'End Function
End Class
