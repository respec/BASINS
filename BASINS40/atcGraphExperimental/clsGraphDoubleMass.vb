Imports System.Drawing

Imports atcData
Imports atcUtility
Imports MapWinUtility
Imports ZedGraph

Public Class clsGraphDoubleMass
    Inherits clsGraphBase
    <CLSCompliant(False)> _
    Public Sub New(ByVal aDataGroup As atcTimeseriesGroup, ByVal aZedGraphControl As ZedGraphControl)
        MyBase.New(aDataGroup, aZedGraphControl)
    End Sub

    Public Overrides Property Datasets() As atcTimeseriesGroup
        Get
            Return MyBase.Datasets
        End Get
        Set(ByVal newValue As atcTimeseriesGroup)
            If newValue.Count > 1 Then
                Dim lTimeseriesX As atcTimeseries = newValue.ItemByIndex(0)
                Dim lTimeseriesY As atcTimeseries = newValue.ItemByIndex(1)
                If lTimeseriesX.Attributes.GetValue("timeunit") <> lTimeseriesY.Attributes.GetValue("timeunit") Then
                    Logger.Msg("Double-Mass curve requires two timeseries to be of same time unit.")
                    Exit Property
                End If
                'find common start and end dates
                Dim lSJDay As Double
                Dim lEJDay As Double
                If lTimeseriesX.Dates.Value(0) < lTimeseriesY.Dates.Value(0) Then
                    'y starts after x, use y start date
                    lSJDay = lTimeseriesY.Dates.Value(0)
                Else 'use x start date
                    lSJDay = lTimeseriesX.Dates.Value(0)
                End If
                If lTimeseriesX.Dates.Value(lTimeseriesX.Dates.numValues) < lTimeseriesY.Dates.Value(lTimeseriesY.Dates.numValues) Then
                    'x ends before y, use x end date
                    lEJDay = lTimeseriesX.Dates.Value(lTimeseriesX.Dates.numValues)
                Else 'use y end date
                    lEJDay = lTimeseriesY.Dates.Value(lTimeseriesY.Dates.numValues)
                End If

                Dim lSubsetTimeseriesX As atcTimeseries = SubsetByDate(lTimeseriesX, lSJDay, lEJDay, Nothing)
                Dim lSubsetTimeseriesY As atcTimeseries = SubsetByDate(lTimeseriesY, lSJDay, lEJDay, Nothing)
                Dim lSubsetGroup As New atcTimeseriesGroup
                lSubsetGroup.Add(lSubsetTimeseriesX)
                lSubsetGroup.Add(lSubsetTimeseriesY)

                Dim lRunningSums As New atcTimeseriesGroup
                Dim lTsMath As atcTimeseriesSource = New atcTimeseriesMath.atcTimeseriesMath
                For Each lTimeseries As atcTimeseries In lSubsetGroup
                    Dim lArgsMath As New atcDataAttributes
                    lArgsMath.SetValue("timeseries", lTimeseries)
                    If lTsMath.Open("running sum", lArgsMath) Then
                        lRunningSums.Add(lTsMath.DataSets.ItemByIndex(lTsMath.DataSets.Count - 1))
                    Else
                        Logger.Msg("Running Sum Calculation Failed: " & lTimeseries.ToString())
                    End If
                Next

                If lRunningSums Is Nothing OrElse lRunningSums.Count < 2 Then
                    Exit Property
                End If

                Dim lTserXRunSum As atcTimeseries = lRunningSums(0)
                Dim lTserYRunSum As atcTimeseries = lRunningSums(1)
                Dim DataDiv As Double = 1
                Dim DataScaleUnit As String = ""
                Dim lXMax As Double = lTserXRunSum.Attributes.GetValue("Max")
                Dim lYMax As Double = lTserYRunSum.Attributes.GetValue("Max")
                Dim lMax As Double = lXMax
                If lMax < lYMax Then lMax = lYMax
                Dim lUseLog As Boolean = False
                If lMax / 1000000000.0# > 1.5 Then
                    DataScaleUnit = "Billion"
                    DataDiv = 1000000000
                    lUseLog = True
                ElseIf lMax / 1000000.0# > 1.5 Then
                    DataScaleUnit = "Million"
                    DataDiv = 1000000
                    lUseLog = True
                ElseIf lMax / 1000.0# > 1.5 Then
                    DataScaleUnit = "Thousand"
                    DataDiv = 1000
                End If
                For I As Integer = 0 To lTserXRunSum.numValues
                    lTserXRunSum.Value(I) /= DataDiv
                    lTserYRunSum.Value(I) /= DataDiv
                Next
                MyBase.Datasets = lRunningSums

                lUseLog = False 'double-mass uses plain grid not log
                Dim lPane As GraphPane = MyBase.pZgc.MasterPane.PaneList(0)
                lPane.Legend.IsVisible = False
                With lPane.XAxis
                    If lUseLog Then
                        .Type = AxisType.Log
                    Else
                        .Type = AxisType.Linear
                    End If
                    .Scale.MaxAuto = False
                    Dim lTitle As String = ""
                    With lTimeseriesX.Attributes
                        Dim lScen As String = .GetValue("scenario")
                        Dim lLoc As String = .GetValue("location")
                        Dim lCons As String = .GetValue("constituent")
                        Dim lUnit As String = .GetValue("Units") & ", " & DataScaleUnit
                        Dim lTimeUnit As String = TimeUnitText(lTimeseriesX)
                        'Dim lCurveColor As Color = GetMatchingColor(lScen & ":" & lLoc & ":" & lCons)
                        lTitle = lTimeUnit & " Cummulative " & lCons & " at " & lLoc & vbCrLf & "(" & lUnit & ")"
                    End With
                    .Title.Text = lTitle 'lTimeseriesX.ToString
                End With

                With lPane.YAxis
                    If lUseLog Then
                        .Type = AxisType.Log
                    Else
                        .Type = AxisType.Linear
                    End If
                    .Scale.MaxAuto = False
                    Dim lTitle As String = ""
                    With lTimeseriesY.Attributes
                        Dim lScen As String = .GetValue("scenario")
                        Dim lLoc As String = .GetValue("location")
                        Dim lCons As String = .GetValue("constituent")
                        Dim lUnit As String = .GetValue("Units") & ", " & DataScaleUnit
                        Dim lTimeUnit As String = TimeUnitText(lTimeseriesY)
                        'Dim lCurveColor As Color = GetMatchingColor(lScen & ":" & lLoc & ":" & lCons)
                        lTitle = lTimeUnit & " Cummulative " & lCons & " at " & lLoc & vbCrLf & "(" & lUnit & ")"
                    End With
                    .Title.Text = lTitle 'lTimeseriesY.ToString
                End With

                Dim lCurveColor As Color = Color.Blue
                Dim lCurve As LineItem = Nothing
                
                Dim lXValues() As Double = lTserXRunSum.Values
                Dim lYValues() As Double = lTserYRunSum.Values
                Dim lSymbol As SymbolType
                Dim lNPts As Integer = lXValues.GetUpperBound(0)
                If lNPts < 100 Then
                    lSymbol = SymbolType.Star
                Else
                    lSymbol = SymbolType.Circle
                End If
                lCurve = lPane.AddCurve("", lXValues, lYValues, lCurveColor, lSymbol)
                If lNPts >= 1000 Then
                    lCurve.Symbol.Size = 1
                ElseIf lNPts >= 100 Then
                    lCurve.Symbol.Size = 2
                Else
                    lCurve.Symbol.Size = 3
                End If
                lCurve.Line.IsVisible = False
                lCurve.Tag = lTimeseriesY.ToString & " vs " & lTimeseriesX.ToString
                Dim lmin As Double = lRunningSums(0).Attributes.GetValue("Min")
                With lRunningSums(0).Attributes
                    .DiscardCalculated()
                    '.RemoveByKey("Min")
                    '.RemoveByKey("Minimum")
                    '.RemoveByKey("Max")
                    '.RemoveByKey("Maximum")
                End With
                With lRunningSums(1).Attributes
                    .DiscardCalculated()
                    '.RemoveByKey("Min")
                    '.RemoveByKey("Minimum")
                    '.RemoveByKey("Max")
                    '.RemoveByKey("Maximum")
                End With
                ScaleAxis(lRunningSums, lPane.YAxis)
                lPane.XAxis.Scale.Min = lPane.YAxis.Scale.Min
                lPane.XAxis.Scale.Max = lPane.YAxis.Scale.Max
            End If
            pZgc.Refresh()
        End Set
    End Property

    Private Function TimeUnitText(ByVal aTser As atcTimeseries) As String
        Dim lTimeUnit As String = ""
        With aTser.Attributes
            Select Case CType(.GetValue("timeunit"), atcTimeUnit)
                Case atcTimeUnit.TUYear : lTimeUnit = "Annual"
                Case atcTimeUnit.TUMonth : lTimeUnit = "Monthly"
                Case atcTimeUnit.TUDay : lTimeUnit = "Daily"
                Case atcTimeUnit.TUHour : lTimeUnit = "Hourly"
                Case atcTimeUnit.TUMinute : lTimeUnit = "Minute"
                Case atcTimeUnit.TUSecond : lTimeUnit = "Second"
                Case atcTimeUnit.TUCentury : lTimeUnit = "Century"
            End Select
        End With
        Return lTimeUnit
    End Function
    Public Sub AddFitLine()
        '45 degree line
        Dim lPane As ZedGraph.GraphPane = pZgc.MasterPane.PaneList(0)
        Dim lLine As ZedGraph.LineItem = AddLine(lPane, 1, 0, Drawing.Drawing2D.DashStyle.Dot, "45DegLine")
        lLine.Color = Color.Black

        'regression line 
        Dim lACoef As Double
        Dim lBCoef As Double
        Dim lRSquare As Double

        'use common period from the two datasets
        Dim lSJDay As Double
        Dim lEJDay As Double
        Dim lTimeseriesX As atcTimeseries = Datasets(1)
        Dim lTimeseriesY As atcTimeseries = Datasets(0)
        If lTimeseriesX.Dates.Value(0) < lTimeseriesY.Dates.Value(0) Then
            'y starts after x, use y start date
            lSJDay = lTimeseriesY.Dates.Value(0)
        Else 'use x start date
            lSJDay = lTimeseriesX.Dates.Value(0)
        End If
        If lTimeseriesX.Dates.Value(lTimeseriesX.Dates.numValues) < lTimeseriesY.Dates.Value(lTimeseriesY.Dates.numValues) Then
            'x ends before y, use x end date
            lEJDay = lTimeseriesX.Dates.Value(lTimeseriesX.Dates.numValues)
        Else 'use y end date
            lEJDay = lTimeseriesY.Dates.Value(lTimeseriesY.Dates.numValues)
        End If

        Dim lSubsetTimeseriesX As atcTimeseries = SubsetByDate(lTimeseriesX, lSJDay, lEJDay, Nothing)
        Dim lSubsetTimeseriesY As atcTimeseries = SubsetByDate(lTimeseriesY, lSJDay, lEJDay, Nothing)
        Dim lNote As String = ""

        FitLine(lSubsetTimeseriesX, lSubsetTimeseriesY, lACoef, lBCoef, lRSquare, lNote)
        AddLine(lPane, lACoef, lBCoef, Drawing.Drawing2D.DashStyle.Solid, "RegLine")
        Dim lText As New TextObj
        Dim lFmt As String = "###,##0.###"
        Dim lBstr As String = DoubleToString(lBCoef, , lFmt)
        If lBCoef >= 0 Then lBstr = "+ " & lBstr 'If it was negative, already have "-" prefix
        lText.Text = "Y = " & DoubleToString(lACoef, , lFmt) & " X " & lBstr & Environment.NewLine & _
                     "R = " & DoubleToString(Math.Sqrt(lRSquare), , lFmt) & vbCrLf & _
                     "R Squared = " & DoubleToString(lRSquare, , lFmt)
        If lNote.Length > 0 Then lText.Text &= vbCrLf & lNote
        lText.FontSpec.StringAlignment = Drawing.StringAlignment.Near
        lText.Location = New Location(0.05, 0.05, CoordType.ChartFraction, AlignH.Left, AlignV.Top)
        lText.FontSpec.Border.IsVisible = False
        lPane.GraphObjList.Add(lText)
        lPane.XAxis.Title.Text &= vbCrLf & vbCrLf & "Scatter Plot"
    End Sub

End Class
