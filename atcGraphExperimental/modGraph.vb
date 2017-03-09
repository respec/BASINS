Imports System.Drawing

Imports atcData
Imports atcUtility
Imports ZedGraph
Imports System.IO
Imports System.Xml
Imports System.Xml.Linq
Imports System.Runtime.Serialization.Json

Public Module modGraph
    Friend Const DefaultAxisLabelFormat As String = "#,##0.###"
    Friend DefaultMajorGridColor As Color = Color.FromArgb(255, 225, 225, 225)
    Friend DefaultMinorGridColor As Color = Color.FromArgb(255, 245, 245, 245)

    <CLSCompliant(False)> _
    Public Function AddLine(ByRef aPane As ZedGraph.GraphPane, _
                            ByVal aACoef As Double, _
                            ByVal aBCoef As Double, _
                   Optional ByVal aLineStyle As Drawing.Drawing2D.DashStyle = Drawing.Drawing2D.DashStyle.Solid, _
                   Optional ByVal aTag As String = Nothing) As LineItem
        With aPane
            'Dim lXValues(1) As Double
            'Dim lYValues(1) As Double
            'If aBCoef > 0 Then
            '    lXValues(0) = .XAxis.Scale.Min
            '    lYValues(0) = (aACoef * lXValues(0)) + aBCoef
            'Else
            '    lYValues(0) = .YAxis.Scale.Min
            '    lXValues(0) = (lYValues(0) - aBCoef) / aACoef
            'End If
            'lXValues(1) = .XAxis.Scale.Max
            'lYValues(1) = (aACoef * lXValues(1)) + aBCoef
            Dim lXValues(1000) As Double
            Dim lYValues(1000) As Double
            Dim lStep As Double = (.XAxis.Scale.Max - .XAxis.Scale.Min) / lXValues.GetUpperBound(0)
            For lIndex As Integer = 0 To lXValues.GetUpperBound(0)
                lXValues(lIndex) = .XAxis.Scale.Min + (lStep * lIndex)
                lYValues(lIndex) = (aACoef * lXValues(lIndex)) + aBCoef
            Next
            Dim lCurve As LineItem = .AddCurve("", lXValues, lYValues, Drawing.Color.Blue, SymbolType.None)
            lCurve.Line.Style = aLineStyle
            lCurve.Tag = aTag
            Return lCurve
        End With
    End Function

    ''' <summary>
    ''' Find a Y axis (LEFT, RIGHT, or AUX) for the given atcTimeseries
    ''' </summary>
    ''' <param name="aTimeseries">Timeseries data to turn into a curve</param>
    ''' <param name="aZgc">ZedGraphControl to add the curve to</param>
    ''' <param name="aExistingCurves">Group of other data on this graph, a hint for which Y axis to use</param>
    <CLSCompliant(False)> _
    Function FindYAxis(ByVal aTimeseries As atcTimeseries, ByVal aZgc As ZedGraphControl, ByVal aExistingCurves As atcTimeseriesGroup) As String
        Dim lCons As String = aTimeseries.Attributes.GetValue("constituent")
        Dim lPane As GraphPane = aZgc.MasterPane.PaneList(0)
        Dim lYAxisName As String = aTimeseries.Attributes.GetValue("YAxis", "")
        If lYAxisName.Length = 0 Then 'Does not have a pre-assigned axis
            lYAxisName = "LEFT" 'Default to left Y axis
            'Use the same Y axis as existing curve with this constituent
            Dim lFoundMatchingCons As Boolean = False
            Dim lOldCons As String
            Dim lOldCurve As LineItem
            If Not aExistingCurves Is Nothing Then
                For Each lTs As atcTimeseries In aExistingCurves
                    For Each lOldCurve In lPane.CurveList
                        If lOldCurve.Tag = lTs.Serial & "|" & lTs.Attributes.GetValue("ID") & "|" & lTs.Attributes.GetValue("Data Source") Then
                            lOldCons = lTs.Attributes.GetValue("constituent")
                            If lOldCons = lCons Then
                                If lOldCurve.IsY2Axis Then lYAxisName = "RIGHT" Else lYAxisName = "LEFT"
                                lFoundMatchingCons = True
                                Exit For
                            End If
                        End If
                    Next
                Next
            End If
            If Not lFoundMatchingCons AndAlso lPane.CurveList.Count > 0 Then
                'Put new curve on right axis if we already have a non-matching curve
                lYAxisName = "RIGHT"
            End If
        End If
        Return lYAxisName
    End Function

    ''' <summary>
    ''' Create a new curve for each atcTimeseries in aDataGroup and add them to aZgc
    ''' </summary>
    ''' <param name="aDataGroup">Group of timeseries to make into curves</param>
    ''' <param name="aZgc">graph control to add curves to</param>
    ''' <remarks></remarks>
    <CLSCompliant(False)> _
    Sub AddTimeseriesCurves(ByVal aDataGroup As atcTimeseriesGroup, ByVal aZgc As ZedGraphControl)
        If aZgc.IsDisposed Then Exit Sub
        Dim lCommonTimeUnits As Integer = aDataGroup.CommonAttributeValue("Time Unit", -1)
        Dim lCommonTimeStep As Integer = aDataGroup.CommonAttributeValue("Time Step", -1)

        Dim lCommonScenario As String = aDataGroup.CommonAttributeValue("Scenario", "")
        Dim lCommonLocation As String = aDataGroup.CommonAttributeValue("Location", "")
        Dim lCommonConstituent As String = aDataGroup.CommonAttributeValue("Constituent", "")
        Dim lCommonUnits As String = aDataGroup.CommonAttributeValue("Units", "")

        Dim lConstituent As String
        Dim lUnits As String

        Dim lYaxisNames As New atcCollection 'name for each item in aDataGroup

        Dim lLeftDataSets As New atcTimeseriesGroup
        Dim lRightDataSets As New atcTimeseriesGroup
        Dim lAuxDataSets As New atcTimeseriesGroup

        Dim lCommonTimeUnitName As String = TimeUnitName(lCommonTimeUnits, lCommonTimeStep)

        For Each lTimeseries As atcTimeseries In aDataGroup
            lConstituent = lTimeseries.Attributes.GetValue("Constituent", "").ToString.ToUpper
            lUnits = lTimeseries.Attributes.GetValue("Units", "").ToString.ToUpper
            Dim lYAxisName As String = lTimeseries.Attributes.GetValue("YAxis", "")
            If lYAxisName.Length = 0 Then 'Does not have a pre-assigned axis
                lYAxisName = "LEFT" 'Default to left Y axis

                'Look for existing curve with same constituent and use the same Y axis
                If GroupContainsAttribute(lLeftDataSets, "Constituent", lConstituent) OrElse _
                   GroupContainsAttribute(lLeftDataSets, "Units", lUnits) Then
                    GoTo FoundMatch
                End If
                If GroupContainsAttribute(lRightDataSets, "Constituent", lConstituent) OrElse _
                   GroupContainsAttribute(lRightDataSets, "Units", lUnits) Then
                    lYAxisName = "RIGHT"
                    GoTo FoundMatch
                End If
                If GroupContainsAttribute(lAuxDataSets, "Constituent", lConstituent) OrElse _
                   GroupContainsAttribute(lAuxDataSets, "Units", lUnits) Then
                    lYAxisName = "AUX"
                    GoTo FoundMatch
                End If

                'Precip defaults to aux when there is other data
                If lCommonConstituent.Length = 0 AndAlso lConstituent.Contains("PREC") Then
                    lYAxisName = "AUX"
                    GoTo FoundMatch
                End If

                If lYaxisNames.Contains("LEFT") Then 'Put new curve on right axis if we already have a non-matching curve on the left
                    lYAxisName = "RIGHT"
                    GoTo FoundMatch
                End If
            End If

FoundMatch:
            Select Case lYAxisName.ToUpper
                Case "AUX" : lAuxDataSets.Add(lTimeseries)
                Case "RIGHT" : lRightDataSets.Add(lTimeseries)
                Case Else : lLeftDataSets.Add(lTimeseries)
            End Select
            lYaxisNames.Add(lTimeseries.Serial, lYAxisName)
        Next

        Dim lMain As ZedGraph.GraphPane = Nothing
        Dim lAux As ZedGraph.GraphPane = Nothing
        If lAuxDataSets.Count > 0 Then
            EnableAuxAxis(aZgc.MasterPane, True, 0.2)
            lAux = aZgc.MasterPane.PaneList(0)
            lMain = aZgc.MasterPane.PaneList(1)
        Else
            lMain = aZgc.MasterPane.PaneList(0)
        End If

        Dim lLeftAndRightDataSets As New atcTimeseriesGroup(lLeftDataSets)
        lLeftAndRightDataSets.AddRange(lRightDataSets)
        Dim lElevation As Double = GetNaN()
        If lCommonConstituent = "GW LEVEL" AndAlso Double.TryParse(lLeftAndRightDataSets.CommonAttributeValue("Elevation", Nothing), lElevation) Then
            MakeDepthAndElevationYAxes(aDataGroup, aZgc, lElevation, lYaxisNames, lCommonTimeUnitName, lCommonScenario, lCommonConstituent)
        Else
            For Each lTimeseries As atcTimeseries In aDataGroup
                Dim lCurveDict As Generic.Dictionary(Of String, ZedGraph.CurveItem) = AddTimeseriesCurve(lTimeseries, aZgc, lYaxisNames.ItemByKey(lTimeseries.Serial))
                Dim lMiscText As String = ""
                For Each lKey As String In lCurveDict.Keys
                    If lKey = "provisional" Then
                        lMiscText = "Provisional"
                    ElseIf lKey = "nonprovisional" Then
                        lMiscText = ""
                    End If
                    lCurveDict.Item(lKey).Label.Text = TSCurveLabel(lTimeseries, lCommonTimeUnitName, lCommonScenario, lCommonConstituent, lCommonLocation, lCommonUnits, lMiscText)
                Next
                lCurveDict.Clear()
                lCurveDict = Nothing
            Next

            If lLeftDataSets.Count > 0 Then
                If lCommonConstituent = "GW LEVEL" Then
                    With lMain.YAxis
                        .Scale.IsReverse = True
                        .MajorTic.IsOpposite = False
                        .MinorTic.IsOpposite = False
                    End With
                End If
                ScaleAxis(lLeftDataSets, lMain.YAxis)
            End If
            If lRightDataSets.Count > 0 Then
                ScaleAxis(lRightDataSets, lMain.Y2Axis)
                lMain.Y2Axis.MinSpace = 80
            Else
                lMain.Y2Axis.MinSpace = 20
            End If
            AxisTitlesFromCommonAttributes(lMain, lCommonTimeUnitName, lCommonScenario, lCommonConstituent, lCommonLocation, lCommonUnits)
            If lLeftDataSets.Count > 0 Then
                If lCommonConstituent = "GW LEVEL" Then
                    With lMain.YAxis
                        Select Case aDataGroup(0).Attributes.GetValue("parm_cd", "").ToString
                            Case "61055", "72019", "lev_va" 'Data is depth below lElevation
                                .Title.Text = "Depth to water level, feet below land surface"
                        End Select
                    End With
                End If
            End If
        End If
        If lAuxDataSets.Count > 0 Then
            lAux.YAxis.MinSpace = lMain.YAxis.MinSpace
            lAux.Y2Axis.MinSpace = lMain.Y2Axis.MinSpace

            ScaleAxis(lAuxDataSets, lAux.YAxis)
            lAux.XAxis.Scale.Min = lMain.XAxis.Scale.Min
            lAux.XAxis.Scale.Max = lMain.XAxis.Scale.Max

            'Make sure both graphs line up horizontally
            'Dim lMaxX As Single = Math.Max(lAux.Rect.X, lMain.Rect.X)
            'Dim lMinRight As Single = Math.Max(lAux.Rect.Right, lMain.Rect.Right)
            'lAux.Rect = New RectangleF(lMaxX, lAux.Rect.Y, lMinRight - lMaxX, lAux.Rect.Height)
            'lMain.Rect = New RectangleF(lMaxX, lMain.Rect.Y, lMinRight - lMaxX, lMain.Rect.Height)
        End If
    End Sub

    Private Sub MakeDepthAndElevationYAxes(ByVal aDataGroup As atcTimeseriesGroup, ByVal aZgc As ZedGraphControl, _
                                           ByVal aElevation As Double, ByVal aYaxisNames As atcCollection, _
                                           ByVal aCommonTimeUnitName As String, ByVal aCommonScenario As String, ByVal aCommonConstituent As String)
        Dim lMain As GraphPane = aZgc.MasterPane.PaneList(aZgc.MasterPane.PaneList.Count - 1)
        Dim lMinElev As Double = 1.0E+30
        Dim lMaxElev As Double = -1.0E+30
        Dim lLeftAxisTitle As String = Nothing
        Dim lRightAxisTitle As String = Nothing
        For Each lTimeseries As atcTimeseries In aDataGroup
            Dim lYAxisName As String = aYaxisNames.ItemByKey(lTimeseries.Serial)
            If lYAxisName <> "AUX" Then
                Select Case lTimeseries.Attributes.GetValue("parm_cd", "").ToString
                    Case "61055", "72019", "lev_va" 'Data is depth below lElevation
                        lYAxisName = "LEFT"
                        lMinElev = Math.Min(lMinElev, aElevation - lTimeseries.Attributes.GetValue("Maximum"))
                        lMaxElev = Math.Max(lMaxElev, aElevation - lTimeseries.Attributes.GetValue("Minimum"))
                        'lLeftAxisTitle = lTimeseries.Attributes.GetValue("Description")
                    Case Else 'Data is specified as Elevation
                        lYAxisName = "RIGHT"
                        lMinElev = Math.Min(lMinElev, lTimeseries.Attributes.GetValue("Minimum"))
                        lMaxElev = Math.Max(lMaxElev, lTimeseries.Attributes.GetValue("Maximum"))
                        'lRightAxisTitle = lTimeseries.Attributes.GetValue("Description")
                End Select
                If lRightAxisTitle Is Nothing AndAlso lTimeseries.Attributes.ContainsAttribute("alt_datum_cd") Then
                    lRightAxisTitle = "Groundwater elevation, feet, " & lTimeseries.Attributes.GetValue("alt_datum_cd")
                End If
            End If
            'Dim lCurve As ZedGraph.CurveItem = AddTimeseriesCurve(lTimeseries, aZgc, lYAxisName)
            'lCurve.Label.Text = TSCurveLabel(lTimeseries, aCommonTimeUnitName, aCommonScenario, aCommonConstituent, "", "feet")
            Dim lCurveDict As Generic.Dictionary(Of String, ZedGraph.CurveItem) = AddTimeseriesCurve(lTimeseries, aZgc, lYAxisName)
            Dim lMiscText As String = ""
            For Each lKey As String In lCurveDict.Keys
                If lKey = "provisional" Then
                    lMiscText = "Provisional"
                ElseIf lKey = "nonprovisional" Then
                    lMiscText = ""
                End If
                lCurveDict.Item(lKey).Label.Text = TSCurveLabel(lTimeseries, aCommonTimeUnitName, aCommonScenario, aCommonConstituent, "", "feet", lMiscText)
            Next
            lCurveDict.Clear()
            lCurveDict = Nothing
        Next
        With lMain.YAxis
            If lLeftAxisTitle Is Nothing Then
                .Title.Text = "Depth to water level, feet below land surface"
            Else
                .Title.Text = lLeftAxisTitle
            End If
            .Scale.IsReverse = True
            .MajorTic.IsOpposite = False
            .MinorTic.IsOpposite = False
            Scalit(aElevation - lMaxElev, aElevation - lMinElev, .Scale.IsLog, .Scale.Min, .Scale.Max)
        End With
        With lMain.Y2Axis
            If lRightAxisTitle Is Nothing Then
                .Title.Text = "Groundwater elevation, feet"
            Else
                .Title.Text = lRightAxisTitle
            End If
            .MinSpace = 80
            .IsVisible = True
            .Scale.IsVisible = True
            .MajorTic.IsOpposite = False
            .MinorTic.IsOpposite = False
            .MajorGrid.IsVisible = False
            .MinorGrid.IsVisible = False
            'Scale with elevation Scalit(lMinElev, lMaxElev, .Scale.IsLog, .Scale.Min, .Scale.Max)
            .Scale.Min = aElevation - lMain.YAxis.Scale.Max
            .Scale.Max = aElevation - lMain.YAxis.Scale.Min
            'TODO: make sure Y2Axis stays in sync with YAxis
        End With
    End Sub

    <CLSCompliant(False)> _
    Sub AxisTitlesFromCommonAttributes(ByVal aPane As GraphPane, _
                              Optional ByVal aCommonTimeUnitName As String = Nothing, _
                              Optional ByVal aCommonScenario As String = Nothing, _
                              Optional ByVal aCommonConstituent As String = Nothing, _
                              Optional ByVal aCommonLocation As String = Nothing, _
                              Optional ByVal aCommonUnits As String = Nothing)
        If Not String.IsNullOrEmpty(aCommonTimeUnitName) Then
            aCommonTimeUnitName = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(aCommonTimeUnitName.ToLower)
            If aCommonTimeUnitName = "<Unk>" Then aCommonTimeUnitName = "<unk>"
        End If
        If Not String.IsNullOrEmpty(aCommonScenario) Then
            aCommonScenario = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(aCommonScenario.ToLower)
            If aCommonScenario = "<Unk>" Then aCommonScenario = "<unk>"
        End If
        If Not String.IsNullOrEmpty(aCommonConstituent) Then
            aCommonConstituent = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(aCommonConstituent.ToLower)
            If aCommonConstituent = "<Unk>" Then aCommonConstituent = "<unk>"
        End If
        If Not String.IsNullOrEmpty(aCommonLocation) Then
            aCommonLocation = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(aCommonLocation.ToLower)
            If aCommonLocation = "<Unk>" Then aCommonLocation = "<unk>"
        End If

        If Not String.IsNullOrEmpty(aCommonTimeUnitName) AndAlso aCommonTimeUnitName <> "<unk>" _
           AndAlso Not aPane.XAxis.Title.Text.Contains(aCommonTimeUnitName) Then
            aPane.XAxis.Title.Text &= " " & aCommonTimeUnitName
        End If

        If Not String.IsNullOrEmpty(aCommonScenario) AndAlso aCommonScenario <> "<unk>" Then
            If aCommonConstituent.Length > 0 _
               AndAlso Not aPane.YAxis.Title.Text.Contains(aCommonScenario) Then
                aPane.YAxis.Title.Text &= " " & aCommonScenario
            ElseIf Not aPane.XAxis.Title.Text.Contains(aCommonScenario) Then
                aPane.XAxis.Title.Text &= " " & aCommonScenario
            End If
        End If

        If Not String.IsNullOrEmpty(aCommonConstituent) AndAlso aCommonConstituent <> "<unk>" _
           AndAlso Not aPane.YAxis.Title.Text.Contains(aCommonConstituent) Then
            aPane.YAxis.Title.Text &= " " & aCommonConstituent
        End If

        If Not String.IsNullOrEmpty(aCommonLocation) AndAlso aCommonLocation <> "<unk>" _
           AndAlso Not aPane.XAxis.Title.Text.Contains(aCommonLocation) Then
            If aPane.XAxis.Title.Text.Length > 0 Then aPane.XAxis.Title.Text &= " at "
            aPane.XAxis.Title.Text &= aCommonLocation
        End If

        If Not String.IsNullOrEmpty(aCommonUnits) AndAlso aCommonUnits <> "<unk>" Then
            If aPane.YAxis.Title.Text.StartsWith(aCommonUnits) Then
                aPane.YAxis.Title.Text = aPane.YAxis.Title.Text.Substring(aCommonUnits.Length).Trim
            End If
            If Not aPane.YAxis.Title.Text.Contains(aCommonUnits) Then
                If aPane.YAxis.Title.Text.Length > 0 Then
                    aPane.YAxis.Title.Text &= ", " & aCommonUnits
                Else
                    aPane.YAxis.Title.Text = aCommonUnits
                End If
            End If
        End If
    End Sub

    Private Function GroupContainsAttribute(ByVal aGroup As atcTimeseriesGroup, ByVal aAttribute As String, ByVal aValue As String) As Boolean
        For Each lTs As atcTimeseries In aGroup
            If String.Compare(lTs.Attributes.GetValue(aAttribute), aValue, True) = 0 Then
                Return True
            End If
        Next
        Return False
    End Function

    Public Function TimeUnitName(ByVal aTimeUnit As Integer, Optional ByVal aTimeStep As Integer = 1) As String
        Dim lName As String = ""
        Select Case aTimeStep
            Case Is > 1 : lName = aTimeStep & "-"
            Case Is < 1 : aTimeUnit = 0 'aTimeStep <= 0 means bad time step, ignore time units and return ""
        End Select
        Select Case aTimeUnit
            Case 1 : lName &= "SECOND"
            Case 2 : lName &= "MINUTE"
            Case 3 : lName &= "HOURLY"
            Case 4 : lName &= "DAILY"
            Case 5 : lName &= "MONTHLY"
            Case 6 : lName &= "YEARLY"
        End Select
        If lName = "7-DAILY" Then lName = "WEEKLY"
        Return lName
    End Function

    Private Function AxisTypeFromName(ByVal aAxisTypeName As String) As ZedGraph.AxisType
        Select Case aAxisTypeName
            Case "Date" : Return AxisType.Date
            Case "DateAsOrdinal" : Return AxisType.DateAsOrdinal
            Case "DateDual" : Return AxisType.DateDual
            Case "Exponent" : Return AxisType.Exponent
            Case "Linear" : Return AxisType.Linear
            Case "LinearAsOrdinal" : Return AxisType.LinearAsOrdinal
            Case "Log" : Return AxisType.Log
            Case "Ordinal" : Return AxisType.Ordinal
            Case "Probability" : Return AxisType.Probability
            Case "Text" : Return AxisType.Text
            Case Else : Return AxisType.DateDual
        End Select
    End Function

    ''' <summary>
    ''' Return a new group of TS with dates shifted if needed for all TS to start in aStartYear
    ''' </summary>
    ''' <param name="aTimeseriesGroup">Data to shift as needed to start in aStartYear</param>
    ''' <param name="aStartYear">Year that resulting data will be shifted to start in, or zero to start in same year as first TS in aTimeseriesGroup</param>
    ''' <returns></returns>
    Public Function MakeCommonStartYear(ByVal aTimeseriesGroup As atcTimeseriesGroup, ByVal aStartYear As Integer) As atcTimeseriesGroup
        Dim lCommon As New atcTimeseriesGroup
        Dim lTsStartDate(6) As Integer
        Dim lDelta As Double
        For Each lTs As atcTimeseries In aTimeseriesGroup
            If lTs.numValues > 0 Then
                modDate.J2Date(lTs.Dates.Value(1), lTsStartDate)
                If aStartYear = 0 Then 'First timeseries gets to keep its dates
                    aStartYear = lTsStartDate(0)
                ElseIf lTsStartDate(0) <> aStartYear Then 'Move other timeseries to start in same year
                    lDelta = lTs.Dates.Value(1) - Date2J(aStartYear, lTsStartDate(1), lTsStartDate(2), _
                                                         lTsStartDate(3), lTsStartDate(4), lTsStartDate(5))
                    Dim lZTs As New atcTimeseries(Nothing)
                    lZTs.Values = lTs.Values
                    lZTs.Dates = lTs.Dates - lDelta
                    With lZTs.Attributes
                        .ChangeTo(lTs.Attributes)
                        .RemoveByKey("Start Date")
                        .RemoveByKey("End Date")
                    End With
                    If lTs.ValueAttributesExist Then
                        For lIndex As Integer = 1 To lTs.numValues
                            If lTs.ValueAttributesExist(lIndex) Then
                                lZTs.ValueAttributes(lIndex) = lTs.ValueAttributes(lIndex)
                            End If
                        Next
                    End If
                    lTs = lZTs
                End If
                lCommon.Add(lTs)
            End If
        Next
        Return lCommon
    End Function

    ''' <summary>
    ''' Return a new group of TS with dates shifted if needed for all TS to start on the same day
    ''' </summary>
    ''' <param name="aTimeseriesGroup">Data to shift as needed</param>
    ''' <param name="aStartYear">Year that resulting data will be shifted to start in, or zero to start in same year as first TS in aTimeseriesGroup</param>
    ''' <param name="aStartMonth">Month that resulting data will be shifted to start in, or zero to start in same month as first TS in aTimeseriesGroup</param>
    ''' <param name="aStartDay">Day of month that resulting data will be shifted to start in, or zero to start in same day as first TS in aTimeseriesGroup</param>
    ''' <returns></returns>
    Public Function MakeCommonStartDay(ByVal aTimeseriesGroup As atcTimeseriesGroup, _
                                       ByVal aStartYear As Integer, _
                                       ByVal aStartMonth As Integer, _
                                       ByVal aStartDay As Integer) As atcTimeseriesGroup
        Dim lCommon As New atcTimeseriesGroup
        Dim lTsStartDate(6) As Integer
        Dim lDelta As Double
        For Each lTs As atcTimeseries In aTimeseriesGroup
            If lTs.numValues > 0 Then
                modDate.J2Date(lTs.Dates.Value(1), lTsStartDate)
                If aStartYear = 0 Then 'First timeseries gets to keep its dates
                    aStartYear = lTsStartDate(0)
                    aStartMonth = lTsStartDate(1)
                    aStartDay = lTsStartDate(2)
                ElseIf lTsStartDate(0) <> aStartYear _
                  OrElse lTsStartDate(1) <> aStartMonth _
                  OrElse lTsStartDate(2) <> aStartDay Then 'Move timeseries to start in same year
                    lDelta = lTs.Dates.Value(1) - Date2J(aStartYear, aStartMonth, aStartDay, _
                                                         lTsStartDate(3), lTsStartDate(4), lTsStartDate(5))
                    Dim lZTs As New atcTimeseries(Nothing)
                    lZTs.Values = lTs.Values
                    lZTs.Dates = lTs.Dates - lDelta
                    lZTs.Attributes.ChangeTo(lTs.Attributes)
                    lZTs.Attributes.RemoveByKey("SJDay")
                    lZTs.Attributes.RemoveByKey("EJDay")
                    lTs = lZTs
                End If
                lCommon.Add(lTs)
            End If
        Next
        Return lCommon
    End Function

    ''' <summary>
    ''' Create a new curve from the given atcTimeseries and add it to the ZedGraphControl
    ''' </summary>
    ''' <param name="aTimeseries">Timeseries data to turn into a curve</param>
    ''' <param name="aZgc">ZedGraphControl to add the curve to</param>
    ''' <param name="aYAxisName">Y axis to use (LEFT, RIGHT, or AUX)</param>
    ''' <return>A dictionary of ZedGraph CurveItem Objects, i.e. non-provisional and/or provisional data curves</return>
    ''' <remarks>The returned collection of CurveItem is keyed on provisional or non-provisional</remarks>
    <CLSCompliant(False)>
    Function AddTimeseriesCurve(ByVal aTimeseries As atcTimeseries, ByVal aZgc As ZedGraphControl, ByVal aYAxisName As String,
                       Optional ByVal aCommonTimeUnitName As String = Nothing,
                       Optional ByVal aCommonScenario As String = Nothing,
                       Optional ByVal aCommonConstituent As String = Nothing,
                       Optional ByVal aCommonLocation As String = Nothing,
                       Optional ByVal aCommonUnits As String = Nothing) As Generic.Dictionary(Of String, CurveItem)

        Dim lCurve As LineItem = Nothing
        Dim lCurveDict As New Generic.Dictionary(Of String, CurveItem)()

        'Graph provisional data separately
        Dim lGraphThese As New atcTimeseriesGroup
        Dim lProvisionalTS As atcTimeseries = Nothing
        Dim lNonProvisionalTS As atcTimeseries = Nothing
        If HasProvisionalValues(aTimeseries) Then
            SplitProvisional(aTimeseries, lProvisionalTS, lNonProvisionalTS)
            If lNonProvisionalTS IsNot Nothing AndAlso lNonProvisionalTS.Attributes.GetValue("Count", 0) > 0 Then
                lGraphThese.Add(lNonProvisionalTS)
            End If
            If lProvisionalTS IsNot Nothing AndAlso lProvisionalTS.Attributes.GetValue("Count", 0) > 0 Then
                lGraphThese.Add(lProvisionalTS)
            End If
        Else
            lGraphThese.Add(aTimeseries)
        End If

        For Each lTimeseries As atcTimeseries In lGraphThese
            Dim lScen As String = lTimeseries.Attributes.GetValue("scenario")
            Dim lLoc As String = lTimeseries.Attributes.GetValue("location")
            Dim lCons As String = lTimeseries.Attributes.GetValue("constituent")
            Dim lCurveLabel As String = TSCurveLabel(lTimeseries, aCommonTimeUnitName, aCommonScenario, aCommonConstituent, aCommonLocation, aCommonUnits)
            Dim lCurveColor As Color
            If lTimeseries = lProvisionalTS Then
                lCurveColor = Color.Red
                lCurveLabel &= " Provisional"
            Else
                lCurveColor = GetMatchingColor(lScen & ":" & lLoc & ":" & lCons)
            End If

            Dim lPane As GraphPane = aZgc.MasterPane.PaneList(aZgc.MasterPane.PaneList.Count - 1)
            Dim lYAxis As Axis = lPane.YAxis
            Dim lXAxisType As ZedGraph.AxisType = AxisTypeFromName(lTimeseries.Attributes.GetValue("GraphXAxisType", "DateDual"))
            If lPane.XAxis.Type <> lXAxisType Then lPane.XAxis.Type = lXAxisType

            Select Case aYAxisName.ToUpper
                Case "AUX"
                    EnableAuxAxis(aZgc.MasterPane, True, 0.2)
                    lPane = aZgc.MasterPane.PaneList(0)
                    lYAxis = lPane.YAxis
                Case "RIGHT"
                    With lPane.YAxis
                        .MajorTic.IsOpposite = False
                        .MinorTic.IsOpposite = False
                    End With
                    With lPane.Y2Axis
                        .MajorTic.IsOpposite = False
                        .MinorTic.IsOpposite = False
                        .MinSpace = 80
                        aZgc.MasterPane.PaneList(0).Y2Axis.MinSpace = .MinSpace 'align right space on aux graph if present
                    End With
                    lYAxis = lPane.Y2Axis
            End Select

            lYAxis.IsVisible = True
            lYAxis.Scale.IsVisible = True

            With lPane
                If .XAxis.Type <> lXAxisType Then .XAxis.Type = lXAxisType

                If lTimeseries.Attributes.GetValue("point", False) Then
                    lCurve = .AddCurve(lCurveLabel, New atcTimeseriesPointList(lTimeseries), lCurveColor, SymbolType.Plus)
                    lCurve.Line.IsVisible = False
                Else
                    lCurve = .AddCurve(lCurveLabel, New atcTimeseriesPointList(lTimeseries), lCurveColor, SymbolType.None)
                    lCurve.Line.Width = 1
                    Select Case lTimeseries.Attributes.GetValue("StepType", "rearwardstep").ToString.ToLower
                        Case "rearwardstep" : lCurve.Line.StepType = StepType.RearwardStep
                        Case "forwardsegment" : lCurve.Line.StepType = StepType.ForwardSegment
                        Case "forwardstep" : lCurve.Line.StepType = StepType.ForwardStep
                        Case "nonstep" : lCurve.Line.StepType = StepType.NonStep
                        Case "rearwardsegment" : lCurve.Line.StepType = StepType.RearwardSegment
                    End Select
                End If
                If lCurveLabel.Contains("Provisional") Then
                    If Not lCurveDict.ContainsKey("provisional") Then
                        lCurveDict.Add("provisional", lCurve)
                    End If
                Else
                    If Not lCurveDict.ContainsKey("nonprovisional") Then
                        lCurveDict.Add("nonprovisional", lCurve)
                    End If
                End If

                Dim lSourceFile As String = ""
                If lTimeseries.Attributes.ContainsAttribute("Data Source") Then
                    lSourceFile = lTimeseries.Attributes.GetValue("Data Source")
                End If
                If Not IO.File.Exists(lSourceFile) And (lTimeseries.Attributes.ContainsAttribute("History 1") AndAlso lTimeseries.Attributes.GetValue("History 1").Length > 9) Then
                    'see if the history attribute contains a file name
                    lSourceFile = lTimeseries.Attributes.GetValue("History 1").Substring(10)
                End If
                lCurve.Tag = lTimeseries.Serial & "|" & lTimeseries.Attributes.GetValue("ID") & "|" & lSourceFile   'Make this easy to find again even if label changes

                If aYAxisName.ToUpper.Equals("RIGHT") Then lCurve.IsY2Axis = True

                'Use units as Y axis title (if this data has units and Y axis title is not set)
                If lTimeseries.Attributes.ContainsAttribute("Units") AndAlso
                   (lYAxis.Title Is Nothing OrElse lYAxis.Title.Text Is Nothing OrElse lYAxis.Title.Text.Length = 0) Then
                    lYAxis.Title.Text = lTimeseries.Attributes.GetValue("Units")
                    lYAxis.Title.IsVisible = True
                End If

                Dim lSJDay As Double = lTimeseries.Attributes.GetValue("SJDay")
                Dim lEJDay As Double = lTimeseries.Attributes.GetValue("EJDay")
                If .CurveList.Count = 1 Then
                    If lTimeseries.numValues > 0 Then 'Set X axis to contain this date range
                        .XAxis.Scale.Min = lSJDay
                        .XAxis.Scale.Max = lEJDay
                    End If
                ElseIf .CurveList.Count > 1 AndAlso Not lCurve Is Nothing Then
                    'Expand time scale if needed to include all dates in new curve
                    If lTimeseries.numValues > 0 Then
                        If lSJDay < .XAxis.Scale.Min Then
                            .XAxis.Scale.Min = lSJDay
                        End If
                        If lEJDay > .XAxis.Scale.Max Then
                            .XAxis.Scale.Max = lEJDay
                        End If
                    End If
                End If

            End With
        Next
        If lProvisionalTS IsNot Nothing Then
            'Dim lProvisionalCurve As CurveItem = AddTimeseriesCurve( _
            '    lProvisionalTS, aZgc, aYAxisName, _
            '    aCommonTimeUnitName, aCommonScenario, aCommonConstituent, _
            '    aCommonLocation, aCommonUnits)
            'lProvisionalCurve.Label.Text = "Provisional"
            'lProvisionalCurve.Color = Color.Red
            If Not lProvisionalTS.Attributes.GetValue("point", False) AndAlso lProvisionalTS.Attributes.GetValue("Count") <> lProvisionalTS.numValues Then
                'atcTimeseriesPointList has its own copy of the values, discard the temporary timeseries
                lProvisionalTS.Clear()
            End If
        End If
        If lNonProvisionalTS IsNot Nothing Then
            If Not lNonProvisionalTS.Attributes.GetValue("point", False) AndAlso lNonProvisionalTS.Attributes.GetValue("Count") <> lNonProvisionalTS.numValues Then
                'atcTimeseriesPointList has its own copy of the values, discard the temporary timeseries
                lNonProvisionalTS.Clear()
            End If
        End If

        Return lCurveDict
    End Function

    Public Function TSCurveLabel(ByVal aTimeseries As atcTimeseries,
                        Optional ByVal aCommonTimeUnitName As String = Nothing,
                        Optional ByVal aCommonScenario As String = Nothing,
                        Optional ByVal aCommonConstituent As String = Nothing,
                        Optional ByVal aCommonLocation As String = Nothing,
                        Optional ByVal aCommonUnits As String = Nothing,
                        Optional ByVal aMiscText As String = "") As String
        With aTimeseries.Attributes
            Dim lCurveLabel As String = ""

            If String.IsNullOrEmpty(aCommonTimeUnitName) _
              AndAlso aTimeseries.Attributes.ContainsAttribute("Time Unit") Then
                lCurveLabel &= TimeUnitName(aTimeseries.Attributes.GetValue("Time Unit"),
                                            aTimeseries.Attributes.GetValue("Time Step", 1)) & " "
            Else
                lCurveLabel &= aCommonTimeUnitName & " "
            End If

            If String.IsNullOrEmpty(aCommonScenario) Then
                lCurveLabel &= .GetValue("Scenario", "") & " "
            ElseIf Not aCommonScenario.ToLower.Contains("<unk>") Then
                lCurveLabel &= aCommonScenario & " "
            End If
            If String.IsNullOrEmpty(aCommonConstituent) Then
                lCurveLabel &= .GetValue("Constituent", "") & " "
            Else
                lCurveLabel &= aCommonConstituent & " "
            End If
            If String.IsNullOrEmpty(aCommonLocation) OrElse aCommonLocation.ToLower.Contains("<unk>") Then
                Dim lLocation As String = .GetValue("Location", "")
                If lLocation.Length = 0 OrElse lLocation = "<unk>" Then
                    lLocation = .GetValue("STAID", "")
                End If
                If lLocation.Length > 0 AndAlso lLocation <> "<unk>" Then
                    If lCurveLabel.Length > 0 Then lCurveLabel &= "at "
                    lCurveLabel &= lLocation
                End If
            End If
            If lCurveLabel.Length > 0 Then
                lCurveLabel = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(lCurveLabel.ToLower())
            End If

            If .ContainsAttribute("SeasonName") Then
                lCurveLabel &= " " & .GetFormattedValue("SeasonName")
            End If

            If String.IsNullOrEmpty(aCommonUnits) AndAlso .ContainsAttribute("Units") Then
                lCurveLabel &= " (" & .GetValue("Units", "") & ")"
            End If

            If Not String.IsNullOrEmpty(aMiscText) Then
                lCurveLabel &= " (" & aMiscText & ")"
            End If
            Return lCurveLabel.Replace("<unk>", "").Trim '.GetValue("scenario") & " " & .GetValue("constituent") & " at " & .GetValue("location")
        End With
    End Function

    <CLSCompliant(False)> _
    Public Function EnableAuxAxis(ByVal aMasterPane As ZedGraph.MasterPane, ByVal aEnable As Boolean, ByVal aAuxFraction As Single) As GraphPane
        Dim lPaneMain As GraphPane = aMasterPane.PaneList(aMasterPane.PaneList.Count - 1)
        Dim lPaneAux As GraphPane = Nothing
        If aMasterPane.PaneList.Count > 1 Then lPaneAux = aMasterPane.PaneList(0)
        Dim lDummyForm As New Windows.Forms.Form
        Dim lGraphics As Graphics
        Try
            lGraphics = lDummyForm.CreateGraphics()
        Catch ex As Exception
            lDummyForm.Show()
            lDummyForm.Hide()
            lGraphics = lDummyForm.CreateGraphics()
        End Try
        aMasterPane.PaneList.Clear()
        If aEnable Then
            ' Main pane already exists, just needs to be shifted
            With lPaneMain
                .Margin.All = 0
                .Margin.Top = 10
                .Margin.Bottom = 10
            End With
            ' Create, format, position aux pane
            If lPaneAux Is Nothing Then
                lPaneAux = New ZedGraph.GraphPane
                FormatPaneWithDefaults(lPaneAux)
                With lPaneAux
                    .Margin.All = 0
                    .Margin.Top = 10
                    With .XAxis
                        .Type = lPaneMain.XAxis.Type
                        .Title.IsVisible = False
                        .Scale.IsVisible = False
                        .Scale.Max = lPaneMain.XAxis.Scale.Max
                        .Scale.Min = lPaneMain.XAxis.Scale.Min
                    End With
                    .X2Axis.IsVisible = False
                    With .YAxis
                        .Type = AxisType.Linear
                        .MinSpace = lPaneMain.YAxis.MinSpace
                    End With
                    .Y2Axis.MinSpace = lPaneMain.Y2Axis.MinSpace
                End With
            End If

            With aMasterPane
                .PaneList.Add(lPaneAux)
                .PaneList.Add(lPaneMain)
                .SetLayout(lGraphics, PaneLayout.SingleColumn)
                .IsCommonScaleFactor = True
                Dim lOrigAuxHeight As Single = lPaneAux.Rect.Height
                Dim lTotalPaneHeight As Single = lPaneMain.Rect.Height + lOrigAuxHeight
                Dim lPaneX As Single = Math.Max(lPaneAux.Rect.X, lPaneMain.Rect.X)
                Dim lPaneWidth As Single = Math.Min(lPaneAux.Rect.Width, lPaneMain.Rect.Width)
                lPaneAux.Rect = New System.Drawing.Rectangle( _
                        lPaneX, lPaneAux.Rect.Y, _
                        lPaneWidth, lTotalPaneHeight * aAuxFraction)
                lPaneMain.Rect = New System.Drawing.Rectangle( _
                        lPaneX, lPaneMain.Rect.Y - lOrigAuxHeight + lPaneAux.Rect.Height, _
                        lPaneWidth, lTotalPaneHeight - lPaneAux.Rect.Height)
            End With
        Else
            aMasterPane.PaneList.Add(lPaneMain)
            aMasterPane.SetLayout(lGraphics, PaneLayout.SingleColumn)
        End If
        aMasterPane.AxisChange()
        lGraphics.Dispose()
        lDummyForm.Dispose()
        Return lPaneAux
    End Function

    <CLSCompliant(False)> _
    Public Function CreateZgc(Optional ByVal aZgc As ZedGraphControl = Nothing, Optional ByVal aWidth As Integer = 600, Optional ByVal aHeight As Integer = 500) As ZedGraphControl
        InitMatchingColors(FindFile("", "GraphColors.txt")) 'Becky commented this out because now we're calling
        'InitMatchingColors ONCE for the entire program in the HSPFSupport version of modGraph.  The only thing
        'we need from InitMatchingColors is to clear the colors used list
        'ClearColors() 'added by Becky to clear pColorsUsed since we don't run InitMatchingColors anymore

        If Not aZgc Is Nothing AndAlso Not aZgc.IsDisposed Then
            aZgc.Dispose()
        End If
        Try
            aZgc = New ZedGraphControl
        Catch e As Exception
            Throw New ApplicationException("modGraph: Could not create ZedGraphControl", e)
        End Try
        Dim lPaneMain As New GraphPane
        FormatPaneWithDefaults(lPaneMain)

        With aZgc
            .Visible = True
            .IsSynchronizeXAxes = True
            .Width = aWidth
            .Height = aHeight
            With .MasterPane
                .PaneList.Clear() 'remove default GraphPane
                .Border.IsVisible = False
                .Legend.IsVisible = False
                .Margin.All = 10
                .InnerPaneGap = 5
                .IsCommonScaleFactor = True
                .PaneList.Add(lPaneMain)
            End With
            EnableAuxAxis(.MasterPane, False, 0)
        End With
        Return aZgc
    End Function

    <CLSCompliant(False)> _
    Public Sub SetGraphSpecs(ByRef aZgc As ZedGraphControl, _
                             Optional ByRef aLabel1 As String = "Simulated", _
                             Optional ByRef aLabel2 As String = "Observed")
        With aZgc.MasterPane.PaneList(aZgc.MasterPane.PaneList.Count - 1)
            .YAxis.MajorGrid.IsVisible = True
            .YAxis.MinorGrid.IsVisible = False
            .XAxis.MajorGrid.IsVisible = True
            With .CurveList(0)
                .Label.Text = aLabel1
                .Color = System.Drawing.Color.Red
            End With
            With .CurveList(1)
                .Label.Text = aLabel2
                .Color = System.Drawing.Color.Blue
            End With
        End With
        Windows.Forms.Application.DoEvents()
    End Sub

    <CLSCompliant(False)> _
    Public Sub FormatPaneWithDefaults(ByVal aPane As ZedGraph.GraphPane)
        With aPane
            .IsAlignGrids = True
            .IsFontsScaled = False
            .IsPenWidthScaled = False
            With .XAxis
                .Scale.FontSpec.Size = 14
                .Scale.FontSpec.IsBold = True
                .Scale.IsUseTenPower = False
                .Title.IsOmitMag = True
                .Scale.Mag = 0
                .MajorTic.IsOutside = False
                .MajorTic.IsInside = True
                .MajorTic.IsOpposite = True
                .MinorTic.IsOutside = False
                .MinorTic.IsInside = True
                .MinorTic.IsOpposite = True
                .Scale.Format = DefaultAxisLabelFormat
                With .MajorGrid
                    .Color = DefaultMajorGridColor
                    .DashOn = 0
                    .DashOff = 0
                    .IsVisible = True
                End With
                With .MinorGrid
                    .Color = DefaultMinorGridColor
                    .DashOn = 0
                    .DashOff = 0
                    .IsVisible = True
                End With
            End With
            With .X2Axis
                .IsVisible = False
            End With
            SetYaxisDefaults(.YAxis)
            SetYaxisDefaults(.Y2Axis)
            .YAxis.MinSpace = 80
            .Y2Axis.MinSpace = 20
            .Y2Axis.Scale.IsVisible = False 'Default to not labeling on Y2, will be turned on later if different from Y
            With .Legend
                .Position = LegendPos.Float
                .Location = New Location(0.05, 0.05, CoordType.ChartFraction, AlignH.Left, AlignV.Top)
                .IsHStack = False
                .Border.IsVisible = False
                .Fill.IsVisible = False
            End With
            .Border.IsVisible = False
        End With
    End Sub

    Private Sub SetYaxisDefaults(ByVal aYaxis As Axis)
        With aYaxis
            .Title.IsOmitMag = True
            .MajorGrid.IsVisible = True
            .MajorTic.IsOutside = False
            .MajorTic.IsInside = True
            .MinorTic.IsOutside = False
            .MinorTic.IsInside = True
            .Scale.IsUseTenPower = False
            .Scale.FontSpec.Size = 14
            .Scale.FontSpec.IsBold = True
            .Scale.Mag = 0
            .Scale.Format = DefaultAxisLabelFormat
            .Scale.Align = AlignP.Inside
            With .MajorGrid
                .Color = DefaultMajorGridColor
                .DashOn = 0
                .DashOff = 0
                .IsVisible = True
            End With
            With .MinorGrid
                .Color = DefaultMinorGridColor
                .DashOn = 0
                .DashOff = 0
                .IsVisible = True
            End With
        End With
    End Sub

    <CLSCompliant(False)> _
    Public Sub ScaleAxis(ByVal aDataGroup As atcTimeseriesGroup, ByVal aAxis As Axis)
        Dim lDataMin As Double = 1.0E+30
        Dim lDataMax As Double = -1.0E+30
        Dim lLogFlag As Boolean = False
        If aAxis.Type = ZedGraph.AxisType.Log Then
            lLogFlag = True
        End If

        For Each lTimeseries As atcTimeseries In aDataGroup
            Try
                Dim lValue As Double = lTimeseries.Attributes.GetValue("Minimum")
                If lValue < lDataMin Then lDataMin = lValue
                lValue = lTimeseries.Attributes.GetValue("Maximum")
                If lValue > lDataMax Then lDataMax = lValue
            Catch
                'Could not get good Minimum or Maximum value
            End Try
        Next

        If lDataMin < -1.0E+20 Then
            'assume there is a bad value in here
            lDataMin = 0
        End If

        If lLogFlag AndAlso lDataMin <= 0 Then
            lLogFlag = False
            aAxis.Type = AxisType.Linear
            If aAxis.Title.Text.Contains("GW LEVEL") Then
                aAxis.Scale.IsReverse = True
            End If
            MapWinUtility.Logger.Dbg("Change axis type from log to linear to fit less than or equal to zero data.")
        End If
        Scalit(lDataMin, lDataMax, lLogFlag, aAxis.Scale.Min, aAxis.Scale.Max)
    End Sub

    ''' <summary>
    ''' Determines an appropriate scale based on the minimum and maximum values and 
    ''' whether an arithmetic or logarithmic scale is requested. 
    ''' For log scales, the minimum and maximum must not be transformed.
    ''' </summary>
    ''' <param name="aDataMin"></param>
    ''' <param name="aDataMax"></param>
    ''' <param name="aLogScale"></param>
    ''' <param name="aScaleMin"></param>
    ''' <param name="aScaleMax"></param>
    ''' <remarks></remarks>
    Public Sub Scalit(ByVal aDataMin As Double, ByVal aDataMax As Double, ByVal aLogScale As Boolean,
                      ByRef aScaleMin As Double, ByRef aScaleMax As Double)
        'TODO: should existing ScaleMin and ScaleMax be respected?
        If Not aLogScale Then 'arithmetic scale
            'get next lowest mult of 10
            Static lRange(15) As Double
            If lRange(1) < 0.09 Then 'need to initialze
                lRange(1) = 0.1
                lRange(2) = 0.15
                lRange(3) = 0.2
                lRange(4) = 0.4
                lRange(5) = 0.5
                lRange(6) = 0.6
                lRange(7) = 0.8
                lRange(8) = 1.0#
                lRange(9) = 1.5
                lRange(10) = 2.0#
                lRange(11) = 4.0#
                lRange(12) = 5.0#
                lRange(13) = 6.0#
                lRange(14) = 8.0#
                lRange(15) = 10.0#
            End If

            Dim lRangeIndex As Integer
            Dim lRangeInc As Integer
            Dim lDataRndlow As Double = Rndlow(aDataMax)
            If lDataRndlow > 0.0# Then
                lRangeInc = 1
                lRangeIndex = 1
            Else
                lRangeInc = -1
                lRangeIndex = 15
            End If
            Do
                aScaleMax = lRange(lRangeIndex) * lDataRndlow
                lRangeIndex += lRangeInc
            Loop While aDataMax > aScaleMax And lRangeIndex <= 15 And lRangeIndex >= 1

            If aDataMin < 0.5 * aDataMax And aDataMin >= 0.0# And aDataMin = 1 Then
                aScaleMin = 0.0#
            Else 'get next lowest mult of 10
                lDataRndlow = Rndlow(aDataMin)
                If lDataRndlow >= 0.0# Then
                    lRangeInc = -1
                    lRangeIndex = 15
                Else
                    lRangeInc = 1
                    lRangeIndex = 1
                End If
                Do
                    aScaleMin = lRange(lRangeIndex) * lDataRndlow
                    lRangeIndex += lRangeInc
                Loop While aDataMin < aScaleMin And lRangeIndex >= 1 And lRangeIndex <= 15
            End If
        Else 'logarithmic scale
            Dim lLogMin As Integer
            If aDataMin > 0.000000001 Then
                lLogMin = Fix(Math.Log10(aDataMin))
            Else
                'too small or neg value, set to -9
                lLogMin = -9
            End If
            If aDataMin < 1.0# Then
                lLogMin -= 1
            End If
            aScaleMin = 10.0# ^ lLogMin

            Dim lLogMax As Integer
            If aDataMax > 0.000000001 Then
                lLogMax = Fix(Math.Log10(aDataMax))
            Else
                'too small or neg value, set to -8
                lLogMax = -8
            End If
            If aDataMax > 1.0# Then
                lLogMax += 1
            End If
            aScaleMax = 10.0# ^ lLogMax

            If aScaleMin * 10000000.0# < aScaleMax Then
                'limit range to 7 cycles
                aScaleMin = aScaleMax / 10000000.0
            End If
        End If
    End Sub

    Public Sub GraphJsonToFile(ByVal aJsonFileName As String, ByVal aOutputFileName As String)

        'open json to find timeseries needed and plot type
        Dim lGraphType As String = ""
        Dim lCurveContents As New atcCollection
        atcGraph.ReadGraphBasicsFromJSON(aJsonFileName, lGraphType, lCurveContents)

        'put data in data group
        Dim lTimeseriesGroup As New atcTimeseriesGroup
        For Each lCurve As String In lCurveContents
            'get timeseries
            Dim lIndex As String = MapWinUtility.Strings.StrSplit(lCurve, "|", """")
            Dim lID As String = MapWinUtility.Strings.StrSplit(lCurve, "|", """")
            Dim lDataFileName As String = lCurve

            Dim lTimeSource As atcTimeseriesSource = atcDataManager.DataSourceBySpecification(lDataFileName)
            If lTimeSource Is Nothing Then 'not already open
                atcDataManager.OpenDataSource(lDataFileName)
            End If

            For Each lDataSource As atcDataSource In atcDataManager.DataSources
                If lDataSource.Specification = lDataFileName Then
                    For Each lTimSer As atcTimeseries In lDataSource.DataSets
                        If lTimSer.Attributes.GetValue("ID").ToString = lID Then
                            lTimeseriesGroup.Add(lTimSer)
                            Exit For
                        End If
                    Next
                    Exit For
                End If
            Next
        Next

        'do subset by date for residual and cummulative diff graphs
        Dim lTimeseriesGroupSubset As New atcTimeseriesGroup
        If lTimeseriesGroup.Count > 1 Then
            'find common dates
            Dim lFirstStart As Double
            Dim lLastEnd As Double
            Dim lCommonStart As Double
            Dim lCommonEnd As Double
            CommonDates(lTimeseriesGroup, lFirstStart, lLastEnd, lCommonStart, lCommonEnd)
            For Each lTs As atcTimeseries In lTimeseriesGroup
                lTimeseriesGroupSubset.Add(SubsetByDate(lTs, lCommonStart, lCommonEnd, Nothing))
            Next
        End If

        'create basic plot
        Dim lZgc As ZedGraphControl = CreateZgc()
        Select Case lGraphType
            Case "Timeseries", "Time-Series", "Time Series"
                Dim lGrapher As New clsGraphTime(lTimeseriesGroup, lZgc)
            Case "Shared Start Year"
                Dim lGrapher As New clsGraphTime(MakeCommonStartYear(lTimeseriesGroup, 0), lZgc)
            Case "Flow/Duration"
                Dim lGrapher As New clsGraphProbability(lTimeseriesGroup, lZgc)
            Case "Frequency"
                Dim lGrapher As New clsGraphFrequency(lTimeseriesGroup, lZgc)
            Case "Running Sum"
                Dim lGrapher As New clsGraphRunningSum(lTimeseriesGroup, lZgc)
            Case "Double-Mass Curve"
                Dim lGrapher As New clsGraphDoubleMass(lTimeseriesGroup, lZgc)
            Case "Box Whisker"
                Dim lGrapher As New clsGraphBoxWhisker(lTimeseriesGroup, lZgc)
            Case "Residual (TS2 - TS1)"
                Dim lGrapher As New clsGraphResidual(lTimeseriesGroupSubset, lZgc)
            Case "Cumulative Difference"
                Dim lGrapher As New clsGraphCumulativeDifference(lTimeseriesGroupSubset, lZgc)
            Case "Scatter (TS2 vs TS1)"
                Dim lGrapher As New clsGraphScatter(lTimeseriesGroup, lZgc)
        End Select

        'apply specs from json
        atcGraph.ApplySpecsFromJSON(aJsonFileName, lZgc)

        'save it to a file
        lZgc.SaveIn(aOutputFileName)
        lZgc.Dispose()

    End Sub

    Private Sub ReadGraphBasicsFromJSON(ByVal aFilename As String, ByRef aGraphType As String, ByRef aCurveContents As atcCollection)
        Try
            Dim lBuffer() As Byte = File.ReadAllBytes(aFilename)
            Dim lReader As XmlDictionaryReader = JsonReaderWriterFactory.CreateJsonReader(lBuffer, New XmlDictionaryReaderQuotas())
            Dim lXml As XElement = XElement.Load(lReader)
            Dim lDoc As New XmlDocument
            lDoc.LoadXml(lXml.ToString())

            Dim lTagList As XmlNodeList = lDoc.GetElementsByTagName("Tag")
            For Each lNode As XmlNode In lDoc.ChildNodes(0).ChildNodes
                If lNode.Name = "Tag" Then
                    aGraphType = lNode.InnerText
                End If
            Next

            Dim lPaneList As XmlNodeList = lDoc.GetElementsByTagName("PaneList")
            For Each lPane As XmlNode In lPaneList
                For Each lItemNode As XmlNode In lPane.ChildNodes
                    For Each lItemNode2 As XmlNode In lItemNode.ChildNodes
                        If lItemNode2.Name = "CurveList" Then
                            For Each lChildItem As XmlNode In lItemNode2.ChildNodes
                                For Each lChildNode As XmlNode In lChildItem.ChildNodes
                                    If lChildNode.Name = "Tag" And lChildNode.InnerText.Length > 0 Then
                                        Dim lDelims() As String = lChildNode.InnerText.Split({"||"}, 2, StringSplitOptions.None)
                                        If lDelims.Length > 1 Then
                                            'like a scatter plot where there are 2 timeseries on a curve
                                            aCurveContents.Add(lDelims(0))
                                            aCurveContents.Add(lDelims(1))
                                        Else
                                            aCurveContents.Add(lChildNode.InnerText)
                                        End If
                                    End If
                                Next
                            Next
                        End If
                    Next
                Next
            Next

        Catch ex As Exception

        End Try
    End Sub

    <CLSCompliant(False)>
    Friend Function ApplySpecsFromJSON(ByVal aFilename As String, ByRef aZgc As ZedGraph.ZedGraphControl) As Boolean
        'Dim JsonString As String = File.ReadAllText(.FileName)
        'Dim ser As JavaScriptSerializer = New JavaScriptSerializer()
        Try
            'pZgc.MasterPane = ser.Deserialize(Of ZedGraph.MasterPane)(JsonString)   'does not work

            Dim lBuffer() As Byte = File.ReadAllBytes(aFilename)
            Dim lReader As XmlDictionaryReader = JsonReaderWriterFactory.CreateJsonReader(lBuffer, New XmlDictionaryReaderQuotas())
            Dim lXml As XElement = XElement.Load(lReader)
            Dim lDoc As New XmlDocument
            lDoc.LoadXml(lXml.ToString())

            'set up collection of curves for easy reference
            Dim lCurves As New atcCollection
            Dim lCurveIndex As Integer = -1
            For Each lPane As GraphPane In aZgc.MasterPane.PaneList
                For Each lCurve As CurveItem In lPane.CurveList
                    lCurveIndex += 1
                    lCurves.Add(lCurveIndex, lCurve)
                Next
            Next

            Dim lPaneList As XmlNodeList = lDoc.GetElementsByTagName("PaneList")
            Dim lTag As String
            lCurveIndex = -1
            For Each lPane As XmlNode In lPaneList
                For Each lItemNode As XmlNode In lPane.ChildNodes
                    For Each lItemNode2 As XmlNode In lItemNode.ChildNodes
                        If lItemNode2.Name = "CurveList" Then
                            For Each lChildItem As XmlNode In lItemNode2.ChildNodes
                                lCurveIndex += 1
                                For Each lChildNode As XmlNode In lChildItem.ChildNodes
                                    If lChildNode.Name = "Tag" Then
                                        lTag = lChildNode.InnerText
                                    ElseIf lChildNode.Name = "Label" Then
                                        If lChildNode.InnerText.EndsWith("true") Then
                                            lChildNode.InnerText = lChildNode.InnerText.Remove(lChildNode.InnerText.Length - 4, 4) 'work around for a strange bug
                                        End If
                                        lCurves(lCurveIndex).Label.Text = lChildNode.InnerText
                                    ElseIf lChildNode.Name = "Color" Then
                                        Dim lA As Integer = 0
                                        Dim lR As Integer = 0
                                        Dim lG As Integer = 0
                                        Dim lB As Integer = 0
                                        For Each lColorNode As XmlNode In lChildNode.ChildNodes
                                            If lColorNode.Name = "R" Then
                                                lR = lColorNode.InnerText
                                            ElseIf lColorNode.Name = "G" Then
                                                lG = lColorNode.InnerText
                                            ElseIf lColorNode.Name = "B" Then
                                                lB = lColorNode.InnerText
                                            ElseIf lColorNode.Name = "A" Then
                                                lA = lColorNode.InnerText
                                            End If
                                        Next
                                        lCurves(lCurveIndex).color = Color.FromArgb(lA, lR, lG, lB)
                                    ElseIf lChildNode.Name = "Symbol" Then
                                        For Each lSymbolNode As XmlNode In lChildNode.ChildNodes
                                            If lSymbolNode.Name = "Size" Then
                                                lCurves(lCurveIndex).Symbol.Size = lSymbolNode.InnerText
                                            End If
                                        Next
                                    End If
                                Next
                            Next
                        ElseIf lItemNode2.Name = "XAxis" Then
                            For Each lChildItem As XmlNode In lItemNode2.ChildNodes
                                If lChildItem.Name = "Title" Then
                                    For Each lScaleNode As XmlNode In lChildItem.ChildNodes
                                        If lScaleNode.Name = "Text" Then
                                            aZgc.MasterPane.PaneList(0).XAxis.Title.Text = lScaleNode.InnerText
                                        End If
                                    Next
                                ElseIf lChildItem.Name = "Scale"
                                    For Each lScaleNode As XmlNode In lChildItem.ChildNodes
                                        If lScaleNode.Name = "Min" Then
                                            aZgc.MasterPane.PaneList(0).XAxis.Scale.Min = lScaleNode.InnerText
                                        ElseIf lScaleNode.Name = "Max" Then
                                            aZgc.MasterPane.PaneList(0).XAxis.Scale.Max = lScaleNode.InnerText
                                        End If
                                    Next
                                End If
                            Next
                        ElseIf lItemNode2.Name = "YAxis" Then
                            For Each lChildItem As XmlNode In lItemNode2.ChildNodes
                                If lChildItem.Name = "Title" Then
                                    For Each lScaleNode As XmlNode In lChildItem.ChildNodes
                                        If lScaleNode.Name = "Text" Then
                                            aZgc.MasterPane.PaneList(0).YAxis.Title.Text = lScaleNode.InnerText
                                        End If
                                    Next
                                ElseIf lChildItem.Name = "Scale"
                                    For Each lScaleNode As XmlNode In lChildItem.ChildNodes
                                        If lScaleNode.Name = "Min" Then
                                            aZgc.MasterPane.PaneList(0).YAxis.Scale.Min = lScaleNode.InnerText
                                        ElseIf lScaleNode.Name = "Max" Then
                                            aZgc.MasterPane.PaneList(0).YAxis.Scale.Max = lScaleNode.InnerText
                                        End If
                                    Next
                                End If
                            Next
                        ElseIf lItemNode2.Name = "Y2Axis" Then
                            For Each lChildItem As XmlNode In lItemNode2.ChildNodes
                                If lChildItem.Name = "Title" Then
                                    For Each lScaleNode As XmlNode In lChildItem.ChildNodes
                                        If lScaleNode.Name = "Text" Then
                                            aZgc.MasterPane.PaneList(0).Y2Axis.Title.Text = lScaleNode.InnerText
                                        End If
                                    Next
                                ElseIf lChildItem.Name = "Scale"
                                    For Each lScaleNode As XmlNode In lChildItem.ChildNodes
                                        If lScaleNode.Name = "Min" Then
                                            aZgc.MasterPane.PaneList(0).Y2Axis.Scale.Min = lScaleNode.InnerText
                                        ElseIf lScaleNode.Name = "Max" Then
                                            aZgc.MasterPane.PaneList(0).Y2Axis.Scale.Max = lScaleNode.InnerText
                                        End If
                                    Next
                                End If
                            Next
                        End If
                    Next
                Next
            Next

        Catch ex As Exception
            Dim s As String = ex.ToString
        End Try
    End Function
End Module
