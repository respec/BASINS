Imports System.Drawing

Imports atcData
Imports atcUtility
Imports ZedGraph

Public Class clsGraphFrequency
    Inherits clsGraphBase

    <CLSCompliant(False)> _
    Public Sub New(ByVal aDataGroup As atcDataGroup, ByVal aZedGraphControl As ZedGraphControl)
        MyBase.New(aDataGroup, aZedGraphControl)
    End Sub

    Public Overrides Property Datasets() As atcDataGroup
        Get
            Return MyBase.Datasets
        End Get
        Set(ByVal aDataGroup As atcDataGroup)
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

        'TODO: check to see if this is an NDay timseries, if not then compute one or throw an exception

        Dim lDataCount As Integer = aTimeseries.Values.GetUpperBound(0)
        Dim lX(lDataCount) As Double
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
            End If
        End With

        Dim lXFracExceed(lDataCount - 1) As Double
        Dim lY(lDataCount - 1) As Double
        aTimeseries.Attributes.SetValue("Bins", MakeBins(aTimeseries))
        Dim lDataIndex As Integer = 0
        For Each lBin As ArrayList In aTimeseries.Attributes.GetValue("Bins")
            For Each lValue As Double In lBin
                lY(lDataIndex) = lValue
                lXFracExceed(lDataIndex) = (lDataIndex + 1) / CDbl(lDataCount + 1)
                lDataIndex += 1
            Next
        Next
        If lDataIndex <> lDataCount Then
            Debug.Print("big problem with bins")
        End If

        With lPane.XAxis
            '.Scale.BaseTic = lXFracExceed(0)
            .Title.Text = "Percent chance exceeded"
            'TODO - adjust scale to show return periods, also need to reverse labels
        End With
        With lPane.YAxis
            .Type = AxisType.Log
            .Scale.IsUseTenPower = False
            If aTimeseries.Attributes.ContainsAttribute("Units") Then
                .Title.Text = aTimeseries.Attributes.GetValue("Units")
                .Title.IsVisible = True
            End If
        End With

        Dim lCurve As LineItem = lPane.AddCurve(lCurveLabel, lXFracExceed, lY, lCurveColor, SymbolType.None)
        lCurve.Symbol.Type = SymbolType.Circle
        lCurve.Line.IsVisible = False

        'TODO - additional curves for fitted frequency and adjusted fitted frequency and confidence limits
        'TODO -   get points from a call to atcTimeseriesNdayHighLow ComputeFreq

        'TODO - if additional curve points are stored as attributes - process them to make the curves here
        Dim lAttributes As SortedList = aTimeseries.Attributes.ValuesSortedByName
        Dim lAttributeName As String
        Dim lAttributeValue As String
        For lAttributeIndex As Integer = 0 To lAttributes.Count - 1
            lAttributeName = lAttributes.GetKey(lAttributeIndex)
            lAttributeValue = aTimeseries.Attributes.GetFormattedValue(lAttributeName)
        Next

        'TODO - add USGS labeling
        Dim lUSGSLabel As TextObj = Nothing
        Dim lStr As String
        With lPane
            .XAxis.Title.FontSpec.IsBold = False
            .YAxis.Title.FontSpec.IsBold = False

            .XAxis.Title.Text = "ANNUAL EXCEEDANCE PROBABILITY, PERCENT" & vbCrLf & "Station - " & aTimeseries.ToString()
            .YAxis.Title.Text = "ANNUAL PEAK DISCHARGE" & vbCrLf & "CUBIC FEET PER SECOND"


            lStr = "Peakfq 5 run " & Date.Today.ToString() & vbCrLf
            lStr &= "NOTE - Preliminary computation" & vbCrLf & "User is reponsible for assessment and interpretation."
            'lUSGSLabel = New TextObj(lStr, .Rect.Right - 5.0, .Rect.Bottom - 5.0, CoordType.PaneFraction)
            lUSGSLabel = New TextObj(lStr, 0.5, 0.6, CoordType.ChartFraction)
            lUSGSLabel.ZOrder = ZOrder.A_InFront
            .GraphObjList.Add(lUSGSLabel)
            lUSGSLabel.IsVisible = True
            '.GraphObjList.Draw(System.Drawing.Graphics(lUSGSLabel), lPane, 1.0, ZOrder.A_InFront)

        End With


        SetYRange(lPane) 'TODO: does this do anything?
    End Sub

    Private Sub SetYRange(ByVal aPane As ZedGraph.GraphPane)
        Dim lYMax As Double = 0.0001
        Dim lYMin As Double = 1.0E+30
        For Each lCurve As ZedGraph.CurveItem In aPane.CurveList
            For lPointIndex As Integer = 0 To lCurve.NPts - 1
                Dim lY As Double = lCurve.Points(lPointIndex).Y
                lYMax = Math.Max(lYMax, lY)
                lYMin = Math.Min(lYMin, lY)
            Next
        Next
        aPane.YAxis.Scale.MaxAuto = False
        aPane.YAxis.Scale.Max = Math.Pow(10, Math.Ceiling(Log10(lYMax)))
        aPane.YAxis.Scale.Min = Math.Pow(10, Math.Floor(Log10(lYMin)))
    End Sub
End Class
