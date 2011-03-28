Imports System.Drawing

Imports atcData
Imports atcUtility
Imports ZedGraph

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
                        If lOldCurve.Tag = lTs.Serial Then
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
        Dim lPaneMain As GraphPane = aZgc.MasterPane.PaneList(aZgc.MasterPane.PaneList.Count - 1)
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

        For Each lTimeseries As atcTimeseries In aDataGroup
            Dim lCurve As ZedGraph.CurveItem = AddTimeseriesCurve(lTimeseries, aZgc, lYaxisNames.ItemByKey(lTimeseries.Serial))
            lCurve.Label.Text = TSCurveLabel(lTimeseries, lCommonTimeUnitName, lCommonScenario, lCommonConstituent, lCommonLocation, lCommonUnits)
        Next

        If lLeftDataSets.Count > 0 Then
            ScaleAxis(lLeftDataSets, lPaneMain.YAxis)
        End If
        If lRightDataSets.Count > 0 Then
            ScaleAxis(lRightDataSets, lPaneMain.Y2Axis)
            lMain.Y2Axis.MinSpace = 80
        Else
            lMain.Y2Axis.MinSpace = 20
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

        AxisTitlesFromCommonAttributes(lPaneMain, lCommonTimeUnitName, lCommonScenario, lCommonConstituent, lCommonLocation, lCommonUnits)
    End Sub

    <CLSCompliant(False)> _
    Sub AxisTitlesFromCommonAttributes(ByVal aPane As GraphPane, _
                              Optional ByVal aCommonTimeUnitName As String = Nothing, _
                              Optional ByVal aCommonScenario As String = Nothing, _
                              Optional ByVal aCommonConstituent As String = Nothing, _
                              Optional ByVal aCommonLocation As String = Nothing, _
                              Optional ByVal aCommonUnits As String = Nothing)
        If Not aCommonTimeUnitName Is Nothing AndAlso aCommonTimeUnitName.Length > 0 _
           AndAlso aCommonTimeUnitName <> "<unk>" _
           AndAlso Not aPane.XAxis.Title.Text.Contains(aCommonTimeUnitName) Then
            aPane.XAxis.Title.Text &= " " & aCommonTimeUnitName
        End If

        If aCommonScenario IsNot Nothing AndAlso aCommonScenario.Length > 0 AndAlso aCommonScenario <> "<unk>" Then
            If aCommonConstituent.Length > 0 _
               AndAlso Not aPane.YAxis.Title.Text.Contains(aCommonScenario) Then
                aPane.YAxis.Title.Text &= " " & aCommonScenario
            ElseIf Not aPane.XAxis.Title.Text.Contains(aCommonScenario) Then
                aPane.XAxis.Title.Text &= " " & aCommonScenario
            End If
        End If

        If aCommonConstituent IsNot Nothing AndAlso aCommonConstituent.Length > 0 _
           AndAlso aCommonConstituent <> "<unk>" _
           AndAlso Not aPane.YAxis.Title.Text.Contains(aCommonConstituent) Then
            aPane.YAxis.Title.Text &= " " & aCommonConstituent
        End If

        If aCommonLocation IsNot Nothing AndAlso aCommonLocation.Length > 0 _
           AndAlso aCommonLocation <> "<unk>" _
           AndAlso Not aPane.XAxis.Title.Text.Contains(aCommonLocation) Then
            If aPane.XAxis.Title.Text.Length > 0 Then aPane.XAxis.Title.Text &= " at "
            aPane.XAxis.Title.Text &= aCommonLocation
        End If

        If aCommonUnits IsNot Nothing AndAlso aCommonUnits.Length > 0 _
           AndAlso aCommonUnits <> "<unk>" _
           AndAlso Not aPane.YAxis.Title.Text.Contains(aCommonUnits) Then
            If aPane.YAxis.Title.Text.Length > 0 Then
                aPane.YAxis.Title.Text &= " (" & aCommonUnits & ")"
            Else
                aPane.YAxis.Title.Text = aCommonUnits
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

    ''' <summary>
    ''' Create a new curve from the given atcTimeseries and add it to the ZedGraphControl
    ''' </summary>
    ''' <param name="aTimeseries">Timeseries data to turn into a curve</param>
    ''' <param name="aZgc">ZedGraphControl to add the curve to</param>
    ''' <param name="aYAxisName">Y axis to use (LEFT, RIGHT, or AUX)</param>
    ''' <remarks></remarks>
    <CLSCompliant(False)> _
    Function AddTimeseriesCurve(ByVal aTimeseries As atcTimeseries, ByVal aZgc As ZedGraphControl, ByVal aYAxisName As String, _
                        Optional ByVal aCommonTimeUnitName As String = Nothing, _
                        Optional ByVal aCommonScenario As String = Nothing, _
                        Optional ByVal aCommonConstituent As String = Nothing, _
                        Optional ByVal aCommonLocation As String = Nothing, _
                        Optional ByVal aCommonUnits As String = Nothing) As CurveItem
        Dim lScen As String = aTimeseries.Attributes.GetValue("scenario")
        Dim lLoc As String = aTimeseries.Attributes.GetValue("location")
        Dim lCons As String = aTimeseries.Attributes.GetValue("constituent")
        Dim lCurveLabel As String = TSCurveLabel(aTimeseries, aCommonTimeUnitName, aCommonScenario, aCommonConstituent, aCommonLocation, aCommonUnits)
        Dim lCurveColor As Color = GetMatchingColor(lScen & ":" & lLoc & ":" & lCons)

        Dim lPane As GraphPane = aZgc.MasterPane.PaneList(aZgc.MasterPane.PaneList.Count - 1)
        Dim lYAxis As Axis = lPane.YAxis
        Dim lCurve As LineItem = Nothing

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
            If .XAxis.Type <> AxisType.DateDual Then
                .XAxis.Type = AxisType.DateDual
            End If
            If aTimeseries.Attributes.GetValue("point", False) Then
                lCurve = .AddCurve(lCurveLabel, New atcTimeseriesPointList(aTimeseries), lCurveColor, SymbolType.Plus)
                lCurve.Line.IsVisible = False
            Else
                lCurve = .AddCurve(lCurveLabel, New atcTimeseriesPointList(aTimeseries), lCurveColor, SymbolType.None)
                lCurve.Line.Width = 1
                Select Case aTimeseries.Attributes.GetValue("StepType", "rearwardstep").ToString.ToLower
                    Case "rearwardstep" : lCurve.Line.StepType = StepType.RearwardStep
                    Case "forwardsegment" : lCurve.Line.StepType = StepType.ForwardSegment
                    Case "forwardstep" : lCurve.Line.StepType = StepType.ForwardStep
                    Case "nonstep" : lCurve.Line.StepType = StepType.NonStep
                    Case "rearwardsegment" : lCurve.Line.StepType = StepType.RearwardSegment
                End Select
            End If

            lCurve.Tag = aTimeseries.Serial 'Make this easy to find again even if label changes

            If aYAxisName.ToUpper.Equals("RIGHT") Then lCurve.IsY2Axis = True

            'Use units as Y axis title (if this data has units and Y axis title is not set)
            If aTimeseries.Attributes.ContainsAttribute("Units") AndAlso _
               (lYAxis.Title Is Nothing OrElse lYAxis.Title.Text Is Nothing OrElse lYAxis.Title.Text.Length = 0) Then
                lYAxis.Title.Text = aTimeseries.Attributes.GetValue("Units")
                lYAxis.Title.IsVisible = True
            End If

            Dim lSJDay As Double = aTimeseries.Attributes.GetValue("SJDay")
            Dim lEJDay As Double = aTimeseries.Attributes.GetValue("EJDay")
            If .CurveList.Count = 1 Then
                If aTimeseries.numValues > 0 Then 'Set X axis to contain this date range
                    .XAxis.Scale.Min = lSJDay
                    .XAxis.Scale.Max = lEJDay
                End If
            ElseIf .CurveList.Count > 1 AndAlso Not lCurve Is Nothing Then
                'Expand time scale if needed to include all dates in new curve
                If aTimeseries.numValues > 0 Then
                    If lSJDay < .XAxis.Scale.Min Then
                        .XAxis.Scale.Min = lSJDay
                    End If
                    If lEJDay > .XAxis.Scale.Max Then
                        .XAxis.Scale.Max = lEJDay
                    End If
                End If
            End If
        End With
        Return lCurve
    End Function

    Public Function TSCurveLabel(ByVal aTimeseries As atcTimeseries, _
                        Optional ByVal aCommonTimeUnitName As String = Nothing, _
                        Optional ByVal aCommonScenario As String = Nothing, _
                        Optional ByVal aCommonConstituent As String = Nothing, _
                        Optional ByVal aCommonLocation As String = Nothing, _
                        Optional ByVal aCommonUnits As String = Nothing) As String
        With aTimeseries.Attributes
            Dim lCurveLabel As String = ""

            If (aCommonTimeUnitName Is Nothing OrElse aCommonTimeUnitName.Length = 0) _
              AndAlso aTimeseries.Attributes.ContainsAttribute("Time Unit") Then
                lCurveLabel &= TimeUnitName(aTimeseries.Attributes.GetValue("Time Unit"), _
                                            aTimeseries.Attributes.GetValue("Time Step", 1)) & " "
            End If

            If aCommonScenario Is Nothing OrElse aCommonScenario.Length = 0 Then
                lCurveLabel &= .GetValue("Scenario", "") & " "
            End If
            If aCommonConstituent Is Nothing OrElse aCommonConstituent.Length = 0 Then
                lCurveLabel &= .GetValue("Constituent", "") & " "
            End If
            If aCommonLocation Is Nothing OrElse aCommonLocation.Length = 0 Then
                Dim lLocation As String = .GetValue("Location", "")
                If lLocation.Length = 0 OrElse lLocation = "<unk>" Then
                    lLocation = .GetValue("STAID", "")
                End If
                If lLocation.Length > 0 AndAlso lLocation <> "<unk>" Then
                    If lCurveLabel.Length > 0 Then lCurveLabel &= "at "
                    lCurveLabel &= lLocation
                End If
            End If
            If (aCommonUnits Is Nothing OrElse aCommonUnits.Length = 0) AndAlso .ContainsAttribute("Units") Then
                lCurveLabel &= " (" & .GetValue("Units", "") & ")"
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
        Dim lGraphics As Graphics = lDummyForm.CreateGraphics()
        aMasterPane.PaneList.Clear()
        If aEnable Then
            ' Main pane already exists, just needs to be shifted
            With lPaneMain
                .Margin.All = 0
                .Margin.Top = 10
                .Margin.Bottom = 10
            End With
            ' Create, format, position aux pane
            If lPaneAux Is Nothing Then lPaneAux = New ZedGraph.GraphPane
            FormatPaneWithDefaults(lPaneAux)
            With lPaneAux
                .Margin.All = 0
                .Margin.Top = 10
                With .XAxis
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
        Return lPaneAux
    End Function

    <CLSCompliant(False)> _
    Public Function CreateZgc(Optional ByVal aZgc As ZedGraphControl = Nothing, Optional ByVal aWidth As Integer = 600, Optional ByVal aHeight As Integer = 500) As ZedGraphControl
        InitMatchingColors(FindFile("", "GraphColors.txt"))

        If Not aZgc Is Nothing AndAlso Not aZgc.IsDisposed Then
            aZgc.Dispose()
        End If

        aZgc = New ZedGraphControl

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
    Public Sub Scalit(ByVal aDataMin As Double, ByVal aDataMax As Double, ByVal aLogScale As Boolean, _
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
End Module
