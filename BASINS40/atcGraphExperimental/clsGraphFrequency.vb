Imports System.Drawing

Imports atcData
Imports atcUtility
Imports MapWinUtility
Imports ZedGraph

Public Class clsGraphFrequency
    Inherits clsGraphBase

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
            'AxisTitlesFromCommonAttributes(pZgc.MasterPane.PaneList(0), lCommonTimeUnitName, lCommonScenario, lCommonConstituent, lCommonLocation, lCommonUnits)
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
        Dim lPane As ZedGraph.GraphPane = pZgc.MasterPane.PaneList(0)
        Dim lScen As String = aTimeseries.Attributes.GetValue("scenario")
        Dim lLoc As String = aTimeseries.Attributes.GetValue("location")
        Dim lCons As String = aTimeseries.Attributes.GetValue("constituent")
        Dim lCurveLabel As String = TSCurveLabel(aTimeseries, aCommonTimeUnitName, aCommonScenario, aCommonConstituent, aCommonLocation, aCommonUnits)
        Dim lCurveColor As Color = GetMatchingColor(lScen & ":" & lLoc & ":" & lCons)

        With lPane
            If .GraphObjList.Count = 0 Then

                'Add USGS Peakfq label
                Dim lUSGSLabel As TextObj = Nothing
                Dim lStr As String = "run " & Date.Today.ToString() & vbCrLf _
                                   & "NOTE - Preliminary computation" & vbCrLf _
                                   & "User is reponsible for assessment and interpretation."
                lUSGSLabel = New TextObj(lStr, 0.7, 0.7, CoordType.ChartFraction)
                lUSGSLabel.ZOrder = ZOrder.A_InFront
                .GraphObjList.Add(lUSGSLabel)
                lUSGSLabel.IsVisible = True

                With .XAxis
                    If .Type <> AxisType.Probability Then .Type = AxisType.Probability
                    With .MajorTic
                        .IsInside = True
                        .IsCrossInside = True
                        .IsOutside = False
                        .IsCrossOutside = False
                    End With
                    .Title.FontSpec.IsBold = False
                    '.Title.Text = "Percent Exceeded"
                    .Title.Text = "ANNUAL EXCEEDANCE PROBABILITY, PERCENT" ' & vbCrLf & "Station - " & aTimeseries.ToString()
                    .Scale.Format = "0.####"
                    Dim lProbScale As ProbabilityScale = .Scale
                    lProbScale.LabelStyle = ProbabilityScale.ProbabilityLabelStyle.Percent
                    lProbScale.IsReverse = True
                End With

                With .YAxis
                    .Type = AxisType.Log
                    .Scale.IsUseTenPower = False
                    .Title.FontSpec.IsBold = False
                    .Title.Text = "ANNUAL PEAK DISCHARGE" & vbCrLf
                    If aTimeseries.Attributes.ContainsAttribute("Units") Then
                        .Title.Text &= aTimeseries.Attributes.GetValue("Units")
                        .Title.IsVisible = True
                    Else
                        .Title.Text &= "CUBIC FEET PER SECOND"
                    End If
                End With
            End If
        End With

        'check to see if this is an annual timseries
        If aTimeseries.Attributes.GetValue("Time Units") <> atcTimeUnit.TUYear Then
            Dim lAnnualTS As atcTimeseries
            Dim lAllAnnual As New atcTimeseriesGroup
            'check to see if any annual timeseries have already been computed by atcTimeseriesNdayHighLow
            For Each lAttribute As atcDefinedValue In aTimeseries.Attributes
                If lAttribute.Arguments IsNot Nothing Then
                    For Each lArgument As atcData.atcDefinedValue In lAttribute.Arguments
                        If lArgument.Value.GetType.Name = "atcTimeseries" Then
                            lAnnualTS = lArgument.Value
                            If lAnnualTS.Attributes.GetValue("Time Units") = atcTimeUnit.TUYear Then
                                If Not lAllAnnual.Contains(lAnnualTS) Then
                                    lAllAnnual.Add(lAnnualTS)
                                End If
                            End If
                        End If
                    Next
                End If
            Next

            'TODO: compute an annual timeseries or throw an exception
            If lAllAnnual.Count = 0 Then

            End If

            For Each lAnnualTS In lAllAnnual
                AddDatasetCurve(lAnnualTS)
            Next
        Else
            Dim lNdays() As Double = Nothing
            AddPercentileCurve(aTimeseries, lPane, lCurveLabel, lCurveColor)
            AddAttributeCurves(aTimeseries, lPane, lCurveColor, lNdays)
        End If

        SetYRange(lPane) 'TODO: does this do anything?

    End Sub

    Private Sub AddAttributeCurves(ByVal aTimeseries As atcTimeseries, _
                                   ByVal aPane As ZedGraph.GraphPane, _
                                   ByVal aCurveColor As Color, _
                                   ByVal aNdays() As Double)
        Dim pNdays As New SortedList
        Dim lNdays As String

        For Each lData As atcDataSet In pDataGroup
            For Each lAttribute As atcDefinedValue In lData.Attributes
                If Not lAttribute.Arguments Is Nothing Then
                    Dim lKey As String = ""
                    If lAttribute.Arguments.ContainsAttribute("Nday") Then
                        lNdays = lAttribute.Arguments.GetFormattedValue("Nday")
                        lKey = Format(lAttribute.Arguments.GetValue("Nday"), "00000.0000")
                        If Not pNdays.ContainsKey(lKey) AndAlso IsIn(lNdays, aNdays) Then
                            pNdays.Add(lKey, lNdays)
                        End If
                    End If
                End If
            Next ' lAttribute
        Next ' lData

        For Each lNdays In pNdays.Values
            AddAttributeCurve(aTimeseries, aPane, aCurveColor, lNdays & "Low", "")
            AddAttributeCurve(aTimeseries, aPane, aCurveColor, lNdays & "High", "")

            'Only add confidence intervals when we have one dataset and one n-day
            If Datasets.Count = 1 AndAlso pNdays.Count = 1 Then
                AddAttributeCurve(aTimeseries, aPane, aCurveColor, lNdays & "Low", " CI Lower")
                AddAttributeCurve(aTimeseries, aPane, aCurveColor, lNdays & "High", " CI Lower")

                AddAttributeCurve(aTimeseries, aPane, aCurveColor, lNdays & "Low", " CI Upper")
                AddAttributeCurve(aTimeseries, aPane, aCurveColor, lNdays & "High", " CI Upper")
            End If
        Next
    End Sub

    ''' <summary>
    ''' Return True if aArray is Nothing or else it contains a number within 1e-30 of aNumber
    ''' </summary>
    Private Function IsIn(ByVal aNumber As Double, ByVal aArray() As Double) As Boolean
        If aArray Is Nothing Then
            Return True
        Else
            For Each lCheck As Double In aArray
                If Math.Abs(lCheck - aNumber) < 1.0E+30 Then
                    Return True
                End If
            Next
        End If
        Return False
    End Function

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

    Private Function AddPercentileCurve(ByVal aTimeseries As atcTimeseries, _
                                        ByVal aPane As ZedGraph.GraphPane, _
                                        ByVal aCurveLabel As String, _
                                        ByVal aCurveColor As Color) As LineItem
        Dim lDataCount As Integer = aTimeseries.Values.GetUpperBound(0)

        Dim lXFracExceed(lDataCount - 1) As Double
        Dim lY(lDataCount - 1) As Double
        aTimeseries.Attributes.SetValue("Bins", MakeBins(aTimeseries))
        Dim lDataIndex As Integer = 0
        For Each lBin As ArrayList In aTimeseries.Attributes.GetValue("Bins")
            For Each lValue As Double In lBin
                lY(lDataIndex) = lValue
                lXFracExceed(lDataIndex) = 1 - ((lDataIndex + 1) / CDbl(lDataCount + 1))
                lDataIndex += 1
            Next
        Next
        If lDataIndex <> lDataCount Then
            Debug.Print("big problem with bins")
        End If

        Dim lCurve As LineItem = aPane.AddCurve(aCurveLabel, lXFracExceed, lY, aCurveColor, SymbolType.None)
        lCurve.Symbol.Type = SymbolType.Circle
        lCurve.Line.IsVisible = False
        Return lCurve
    End Function

    Private Function AddAttributeCurve(ByVal aTimeseries As atcTimeseries, _
                                       ByVal aPane As ZedGraph.GraphPane, _
                                       ByVal aCurveColor As Color, _
                                       ByVal aAttributePrefix As String, _
                                       ByVal aAttributeSuffix As String) As LineItem
        Dim lPreLen As Integer = aAttributePrefix.Length
        Dim lSufLen As Integer = aAttributeSuffix.Length
        Dim lReturnStr As String
        Dim lReturnDbl As Double
        Dim lPoints As New SortedList
        For Each lAttribute As atcDefinedValue In aTimeseries.Attributes
            Dim lName As String = lAttribute.Definition.Name
            If lName.StartsWith(aAttributePrefix) AndAlso _
               lName.EndsWith(aAttributeSuffix) Then
                lReturnStr = lName.Substring(lPreLen, lName.Length - lPreLen - lSufLen)
                If Double.TryParse(lReturnStr, lReturnDbl) Then
                    'Logger.Dbg("Found Attribute " & lName)
                    lPoints.Add(lReturnDbl, lAttribute.Value)
                End If
            End If
        Next

        If lPoints.Count > 0 Then
            Dim lZedGraphPoints As New ZedGraph.PointPairList
            For Each lPoint As DictionaryEntry In lPoints
                Dim lX As Double = 1 - 1 / lPoint.Key
                'Logger.Dbg("Add Point " & lPoint.Key & " = " & lPoint.Value)
                lZedGraphPoints.Add(New ZedGraph.PointPair(lX, lPoint.Value, CStr(lPoint.Key)))
            Next
            Return aPane.AddCurve(aAttributePrefix & aAttributeSuffix, lZedGraphPoints, aCurveColor, SymbolType.None)
        Else
            Return Nothing
        End If
    End Function

End Class
