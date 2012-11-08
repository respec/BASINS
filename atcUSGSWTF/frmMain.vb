Imports atcData
Imports atcUtility
Imports atcGraph
Imports ZedGraph
Imports MapWinUtility
Imports atcUSGSRecess
Imports System.Windows.Forms

Public Class frmMain
    Public FallObj As clsFall
    Public RiseObj As clsFall
    Private pBasicAttributes As Generic.List(Of String)
    Private pDataGroup As atcTimeseriesGroup
    Private pWTFAttributes As atcDataAttributes

    Private pGraphTsHPeak As atcTimeseries
    Private pGraphTsGroupH2 As atcTimeseriesGroup
    Private pGraphTsGroupRecharge As atcTimeseriesGroup

    Private pWTF As clsWTF

    Friend WithEvents FfrmParam As frmParams

    Public Sub Initialize(Optional ByVal aTimeseriesGroup As atcData.atcTimeseriesGroup = Nothing, _
                          Optional ByVal aBasicAttributes As Generic.List(Of String) = Nothing, _
                      Optional ByVal aShowForm As Boolean = True)
        If aBasicAttributes Is Nothing Then
            pBasicAttributes = atcDataManager.DisplayAttributes
        Else
            pBasicAttributes = aBasicAttributes
        End If

        pDataGroup = aTimeseriesGroup

        pWTFAttributes = New atcDataAttributes()

        If FallObj Is Nothing Then
            FallObj = New clsFall()
        End If
        If RiseObj Is Nothing Then
            RiseObj = New clsFall()
        End If

        SetStyle(ControlStyles.DoubleBuffer Or ControlStyles.UserPaint Or ControlStyles.AllPaintingInWmPaint, True)
        Me.Show()
    End Sub

    Private Sub btnAntMethodSpecifyParm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAntMethodSpecifyParm.Click
        Dim lResponse As MsgBoxResult = MsgBox("User Specify Parameters?", MsgBoxStyle.YesNoCancel, "Parameters for Estimating Ant. GWL")
        If rdoAntMethodFall.Checked Then
            If pWTF Is Nothing Then
                pWTF = New clsWTFFall()
            ElseIf Not pWTF.GetType.FullName.ToLower.Contains("fall") Then
                pWTF.Clear()
                pWTF = Nothing
                pWTF = New clsWTFFall()
            End If
            If lResponse = MsgBoxResult.Yes Then
                If FfrmParam Is Nothing OrElse FfrmParam.IsDisposed Then
                    FfrmParam = New frmParams()
                End If
                pWTFAttributes.SetValue("FallD", CType(pWTF, clsWTFFall).GWLAsymptote)
                pWTFAttributes.SetValue("FallKgw", CType(pWTF, clsWTFFall).KGWL)

                FfrmParam.Initialize(AntecedentGWLMethod.FALL, pWTFAttributes)
            ElseIf lResponse = MsgBoxResult.No Then
                Dim lfrmFall As New frmRecess()
                lfrmFall.Initialize(pDataGroup, pBasicAttributes, , , FallObj) 'here you have to write it on a piece of paper
            End If
        ElseIf rdoAntMethodLinear.Checked Then
            If pWTF Is Nothing Then
                pWTF = New clsWTFLinear()
            ElseIf Not pWTF.GetType.FullName.ToLower.Contains("linear") Then
                pWTF.Clear()
                pWTF = Nothing
                pWTF = New clsWTFLinear()
            End If
            If lResponse = MsgBoxResult.Yes Then
                If FfrmParam Is Nothing OrElse FfrmParam.IsDisposed Then
                    FfrmParam = New frmParams()
                End If
                pWTFAttributes.SetValue("LinearA", CType(pWTF, clsWTFLinear).LinearSlope)
                pWTFAttributes.SetValue("LinearB", CType(pWTF, clsWTFLinear).LinearIntercept)
                FfrmParam.Initialize(AntecedentGWLMethod.Linear, pWTFAttributes)
            ElseIf lResponse = MsgBoxResult.No Then
                Dim lfrmFall As New frmRecess()
                lfrmFall.Initialize(pDataGroup, pBasicAttributes, , , FallObj, True)
                pWTF.EstimateParameters(FallObj)
            End If
        ElseIf rdoAntMethodPower.Checked Then
            If pWTF Is Nothing Then
                pWTF = New clsWTFPower()
            ElseIf Not pWTF.GetType.FullName.ToLower.Contains("power") Then
                pWTF.Clear()
                pWTF = Nothing
                pWTF = New clsWTFPower()
            End If
            If lResponse = MsgBoxResult.Yes Then
                If FfrmParam Is Nothing OrElse FfrmParam.IsDisposed Then
                    FfrmParam = New frmParams()
                End If
                pWTFAttributes.SetValue("PowerCIntercept", CType(pWTF, clsWTFPower).ParamCIntercept)
                pWTFAttributes.SetValue("PowerDMultiplier", CType(pWTF, clsWTFPower).ParamDMultiplier)
                pWTFAttributes.SetValue("PowerEDatum", CType(pWTF, clsWTFPower).ParamEDatum)
                pWTFAttributes.SetValue("PowerFExp", CType(pWTF, clsWTFPower).ParamFExp)

                FfrmParam.Initialize(AntecedentGWLMethod.Power, pWTFAttributes)

            ElseIf lResponse = MsgBoxResult.No Then
                Dim lfrmFall As New frmRecess()
                lfrmFall.Initialize(pDataGroup, pBasicAttributes, , , FallObj, True)
                pWTF.EstimateParameters(FallObj)
            End If
        End If
    End Sub

    Private Sub ParameterChanged(ByVal aArgs As atcDataAttributes) Handles FfrmParam.ParameterChanged
        If rdoAntMethodFall.Checked Then
            With CType(pWTF, clsWTFFall)
                Dim lV1 As Double
                If Not Double.IsNaN(aArgs.GetValue("FallD")) AndAlso Double.TryParse(aArgs.GetValue("FallD"), lV1) AndAlso _
                   Not Double.IsNaN(aArgs.GetValue("FallKgw")) AndAlso Double.TryParse(aArgs.GetValue("FallKgw"), lV1) Then
                    .GWLAsymptote = aArgs.GetValue("FallD")
                    .KGWL = aArgs.GetValue("FallKgw")
                    .ParametersSet = True
                Else
                    .ParametersSet = False
                End If
            End With
        ElseIf rdoAntMethodLinear.Checked Then
            With CType(pWTF, clsWTFLinear)
                Dim lV1 As Double
                If Not Double.IsNaN(aArgs.GetValue("LinearA")) AndAlso Double.TryParse(aArgs.GetValue("LinearA"), lV1) AndAlso _
                   Not Double.IsNaN(aArgs.GetValue("LinearB")) AndAlso Double.TryParse(aArgs.GetValue("LinearB"), lV1) Then
                    .LinearSlope = aArgs.GetValue("LinearA")
                    .LinearIntercept = aArgs.GetValue("LinearB")
                    CType(pWTF, clsWTFLinear).ParametersSet = True
                Else
                    CType(pWTF, clsWTFLinear).ParametersSet = False
                End If
            End With
        ElseIf rdoAntMethodPower.Checked Then
            With CType(pWTF, clsWTFPower)
                Dim lV1 As Double
                If Not Double.IsNaN(aArgs.GetValue("PowerCIntercept")) AndAlso Double.TryParse(aArgs.GetValue("PowerCIntercept"), lV1) AndAlso _
                   Not Double.IsNaN(aArgs.GetValue("PowerDMultiplier")) AndAlso Double.TryParse(aArgs.GetValue("PowerDMultiplier"), lV1) AndAlso _
                   Not Double.IsNaN(aArgs.GetValue("PowerEDatum")) AndAlso Double.TryParse(aArgs.GetValue("PowerEDatum"), lV1) AndAlso _
                   Not Double.IsNaN(aArgs.GetValue("PowerFExp")) AndAlso Double.TryParse(aArgs.GetValue("PowerFExp"), lV1) Then
                    .ParamCIntercept = aArgs.GetValue("PowerCIntercept")
                    .ParamDMultiplier = aArgs.GetValue("PowerDMultiplier")
                    .ParamEDatum = aArgs.GetValue("PowerEDatum")
                    .ParamFExp = aArgs.GetValue("PowerFExp")
                    CType(pWTF, clsWTFPower).ParametersSet = True
                Else
                    CType(pWTF, clsWTFPower).ParametersSet = False
                End If
            End With
        End If
    End Sub

    Private Sub btnCalcRecharge_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindRechargeEvents.Click
        Dim lfrmFall As New frmRecess()
        With lfrmFall
            .Text = "Find Recharge Events (all rising limbs); Close form when done."
            .SetFindingRises(True)
            .Initialize(pDataGroup, pBasicAttributes, True, Nothing, RiseObj, True)
        End With

        'Dim lNowSeeifFallObjIsStillThere As String = ""
    End Sub

    Private Sub btnWriteOutput_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnWriteOutput.Click
        If RiseObj Is Nothing OrElse RiseObj.listOfSegments Is Nothing OrElse RiseObj.listOfSegments.Count = 0 Then
            'Is not ready to do the recharge caculation as no recharge events have been found
            Logger.Msg("Must find all recharge events before calculating recharge volume.", "Need to find recharge events")
            Exit Sub
        End If

        If Not pWTF.Recharge(RiseObj) Then
            Logger.Msg("Unable to calculate recharge.")
            Exit Sub
        End If

        Dim lOutputDirectory As String = IO.Path.GetDirectoryName(pDataGroup(0).Attributes.GetValue("History 1").ToString.Replace("Read from", "").Trim())
        Dim lOutputFilename As String = FindFile("WTF Output File", IO.Path.Combine(lOutputDirectory, "WTFPrintOut"), "txt")

        Dim lDelim As String = vbTab
        Dim lWTFOutputDailyTitle As New Text.StringBuilder()
        Dim lWTFOutput As New Text.StringBuilder()

        'Individual recharge event output

        'Monthly and Yearly Recharge output
        'At the same time construct two separate timeseries (point type) for H2 and Hpeak
        
        Dim lSegCtr As Integer = 1
        Dim lTsDailyRecharge As atcTimeseries = pDataGroup(0).Clone()
        Dim lTsDailyH2 As atcTimeseries = pDataGroup(0).Clone()
        pGraphTsHPeak = pDataGroup(0).Clone()
        If pGraphTsGroupH2 IsNot Nothing Then
            pGraphTsGroupH2.Clear()
        Else
            pGraphTsGroupH2 = New atcTimeseriesGroup()
        End If
        If pGraphTsGroupRecharge IsNot Nothing Then
            pGraphTsGroupRecharge.Clear()
        Else
            pGraphTsGroupRecharge = New atcTimeseriesGroup()
        End If

        For I As Integer = 1 To pGraphTsHPeak.numValues
            lTsDailyRecharge.Value(I) = 0.0
            lTsDailyH2.Value(I) = 0.0
            pGraphTsHPeak.Value(I) = 0.0
        Next

        'Title line
        lWTFOutputDailyTitle.Append("Count" & lDelim & _
                          "TsDate" & lDelim & _
                          "Ts Duration" & lDelim & _
                          "Ts GWL" & lDelim & _
                          "GWLpeak" & lDelim & _
                          "H0Date" & lDelim & _
                          "H0 GWL" & lDelim)
        If RiseObj.listOfSegments(0).AntecedentGWLs.Keys.Contains(AntecedentGWLMethod.FALL) Then
            lWTFOutputDailyTitle.Append("FALLH2" & lDelim & "(GWLpeak-FALLH2)")
        End If
        If RiseObj.listOfSegments(0).AntecedentGWLs.Keys.Contains(AntecedentGWLMethod.Linear) Then
            lWTFOutputDailyTitle.Append("LinearH2" & lDelim & "(GWLpeak-LinearH2)")
        End If
        If RiseObj.listOfSegments(0).AntecedentGWLs.Keys.Contains(AntecedentGWLMethod.Power) Then
            lWTFOutputDailyTitle.Append("PowerH2" & lDelim & "(GWLpeak-PowerH2)")
        End If
        lWTFOutputDailyTitle.AppendLine("")

        Dim lHasFall As Boolean = False
        Dim lHasLinear As Boolean = False
        Dim lHasPower As Boolean = False

        Dim lStartDateForGraph As Double = 0
        With CType(RiseObj.listOfSegments(0), clsRecessionSegment)
            lStartDateForGraph = .PeakDayDate - JulianHour * 24 * 7
        End With
        Dim lEndDataForGraph As Double = 0
        With CType(RiseObj.listOfSegments(RiseObj.listOfSegments.Count - 1), clsRecessionSegment)
            lEndDataForGraph = .PeakDayDate + JulianHour * 24 * (.Flow.Length + 7)
        End With
        lWTFOutput.Append(lWTFOutputDailyTitle.ToString)
        For Each lSeg As clsRecessionSegment In RiseObj.listOfSegments
            With lSeg
                'Ctr-ts-Length-GWLts-GWLpeak-H0Date-H0-FALLH2-(GWLpeak-FALLH2)-LinearH2-(GWLpeak-LinearH2)-PowerH2-(GWLpeak-PowerH2)
                Dim lGWLpeak As Double = .Flow(.Flow.Length - 1)
                lWTFOutput.Append(lSegCtr & lDelim & _
                                  .PeakDayDateToString & lDelim & _
                                  .Flow.Length & lDelim & _
                                  .Flow(1) & lDelim & _
                                  lGWLpeak & lDelim & _
                                  .HzeroDayDateToString & lDelim & _
                                  .HzeroDayValue & lDelim)
                Dim lIndexOfHpeak As Double = .PeakDayIndex + .Flow.Length - 1
                pGraphTsHPeak.Value(lIndexOfHpeak) = lGWLpeak
                If .AntecedentGWLs.Keys.Contains(AntecedentGWLMethod.FALL) Then
                    Dim lH2 As Double = Math.Round(.AntecedentGWLs.ItemByKey(AntecedentGWLMethod.FALL), 3)
                    lWTFOutput.Append(lH2 & lDelim & Math.Round(lGWLpeak - lH2, 3))

                    'Construct proper timeseries
                    If pGraphTsGroupH2.Keys.Contains(AntecedentGWLMethod.FALL) Then
                        pGraphTsGroupH2.ItemByKey(AntecedentGWLMethod.FALL).Value(lIndexOfHpeak) = lH2
                    Else
                        Dim lTsDailyH2New As atcTimeseries = lTsDailyH2.Clone()
                        lTsDailyH2New.Attributes.SetValue("Scenario", "FALL method")
                        lTsDailyH2New.Attributes.SetValue("Constituent", "H2")
                        lTsDailyH2New.Value(lIndexOfHpeak) = lH2
                        pGraphTsGroupH2.Add(AntecedentGWLMethod.FALL, lTsDailyH2New)
                    End If

                    If pGraphTsGroupRecharge.Keys.Contains(AntecedentGWLMethod.FALL) Then
                        pGraphTsGroupRecharge.ItemByKey(AntecedentGWLMethod.FALL).Value(lIndexOfHpeak) = .Recharges.ItemByKey(AntecedentGWLMethod.FALL)
                    Else
                        Dim lTsDailyRechargeNew As atcTimeseries = lTsDailyRecharge.Clone()
                        lTsDailyRechargeNew.Attributes.SetValue("Scenario", "FALL method")
                        lTsDailyRechargeNew.Attributes.SetValue("Constituent", "Recharge")
                        lTsDailyRechargeNew.Value(lIndexOfHpeak) = .Recharges.ItemByKey(AntecedentGWLMethod.FALL)
                        pGraphTsGroupRecharge.Add(AntecedentGWLMethod.FALL, lTsDailyRechargeNew)
                    End If
                    lHasFall = True
                End If
                If .AntecedentGWLs.Keys.Contains(AntecedentGWLMethod.Linear) Then
                    Dim lH2 As Double = .AntecedentGWLs.ItemByKey(AntecedentGWLMethod.Linear)
                    lWTFOutput.Append(lH2 & lDelim & lGWLpeak - lH2)

                    'Construct proper timeseries
                    If pGraphTsGroupH2.Keys.Contains(AntecedentGWLMethod.Linear) Then
                        pGraphTsGroupH2.ItemByKey(AntecedentGWLMethod.Linear).Value(lIndexOfHpeak) = lH2
                    Else
                        Dim lTsDailyH2New As atcTimeseries = lTsDailyH2.Clone()
                        lTsDailyH2New.Attributes.SetValue("Scenario", "Linear method")
                        lTsDailyH2New.Attributes.SetValue("Constituent", "H2")
                        lTsDailyH2New.Value(lIndexOfHpeak) = lH2
                        pGraphTsGroupH2.Add(AntecedentGWLMethod.Linear, lTsDailyH2New)
                    End If

                    If pGraphTsGroupRecharge.Keys.Contains(AntecedentGWLMethod.Linear) Then
                        pGraphTsGroupRecharge.ItemByKey(AntecedentGWLMethod.Linear).Value(lIndexOfHpeak) = .Recharges.ItemByKey(AntecedentGWLMethod.Linear)
                    Else
                        Dim lTsDailyRechargeNew As atcTimeseries = lTsDailyRecharge.Clone()
                        lTsDailyRechargeNew.Attributes.SetValue("Scenario", "Linear method")
                        lTsDailyRechargeNew.Attributes.SetValue("Constituent", "Recharge")
                        lTsDailyRechargeNew.Value(lIndexOfHpeak) = .Recharges.ItemByKey(AntecedentGWLMethod.Linear)
                        pGraphTsGroupRecharge.Add(AntecedentGWLMethod.Linear, lTsDailyRechargeNew)
                    End If
                    lHasLinear = True
                End If
                If .AntecedentGWLs.Keys.Contains(AntecedentGWLMethod.Power) Then
                    Dim lH2 As Double = .AntecedentGWLs.ItemByKey(AntecedentGWLMethod.Power)
                    lWTFOutput.Append(lH2 & lDelim & lGWLpeak - lH2)

                    'Construct proper timeseries
                    If pGraphTsGroupH2.Keys.Contains(AntecedentGWLMethod.Power) Then
                        pGraphTsGroupH2.ItemByKey(AntecedentGWLMethod.Power).Value(lIndexOfHpeak) = lH2
                    Else
                        Dim lTsDailyH2New As atcTimeseries = lTsDailyH2.Clone()
                        lTsDailyH2New.Attributes.SetValue("Scenario", "Power method")
                        lTsDailyH2New.Attributes.SetValue("Constituent", "H2")
                        lTsDailyH2New.Value(lIndexOfHpeak) = lH2
                        pGraphTsGroupH2.Add(AntecedentGWLMethod.Power, lTsDailyH2New)
                    End If

                    If pGraphTsGroupRecharge.Keys.Contains(AntecedentGWLMethod.Power) Then
                        pGraphTsGroupRecharge.ItemByKey(AntecedentGWLMethod.Power).Value(lIndexOfHpeak) = .Recharges.ItemByKey(AntecedentGWLMethod.Power)
                    Else
                        Dim lTsDailyRechargeNew As atcTimeseries = lTsDailyRecharge.Clone()
                        lTsDailyRechargeNew.Attributes.SetValue("Scenario", "Power method")
                        lTsDailyRechargeNew.Attributes.SetValue("Constituent", "Recharge")
                        lTsDailyRechargeNew.Value(lIndexOfHpeak) = .Recharges.ItemByKey(AntecedentGWLMethod.Power)
                        pGraphTsGroupRecharge.Add(AntecedentGWLMethod.Power, lTsDailyRechargeNew)
                    End If
                    lHasPower = True
                End If
                lWTFOutput.AppendLine("")
            End With 'lSeg
        Next 'lSeg

        If lHasFall Then
            lWTFOutputDailyTitle.Append("FALLH2" & lDelim & "(GWLpeak-FALLH2)" & lDelim)
        End If
        If lHasLinear Then
            lWTFOutputDailyTitle.Append("LinearH2" & lDelim & "(GWLpeak-LinearH2)" & lDelim)
        End If
        If lHasPower Then
            lWTFOutputDailyTitle.Append("PowerH2" & lDelim & "(GWLpeak-PowerH2)")
        End If
        lWTFOutputDailyTitle.AppendLine("")
        lWTFOutputDailyTitle.AppendLine("")

        'Write output monthly and yearly output in one file
        If lHasFall Then
            WriteMonthlyYearlyOutput(AntecedentGWLMethod.FALL, atcTimeUnit.TUMonth, lWTFOutput, lDelim)
        End If
        If lHasLinear Then
            WriteMonthlyYearlyOutput(AntecedentGWLMethod.Linear, atcTimeUnit.TUMonth, lWTFOutput, lDelim)
        End If
        If lHasPower Then
            WriteMonthlyYearlyOutput(AntecedentGWLMethod.Power, atcTimeUnit.TUMonth, lWTFOutput, lDelim)
        End If

        If lHasFall Then
            WriteMonthlyYearlyOutput(AntecedentGWLMethod.FALL, atcTimeUnit.TUYear, lWTFOutput, lDelim)
        End If
        If lHasLinear Then
            WriteMonthlyYearlyOutput(AntecedentGWLMethod.Linear, atcTimeUnit.TUYear, lWTFOutput, lDelim)
        End If
        If lHasPower Then
            WriteMonthlyYearlyOutput(AntecedentGWLMethod.Power, atcTimeUnit.TUYear, lWTFOutput, lDelim)
        End If

        Dim lSW As New IO.StreamWriter(lOutputFilename, False)
        lSW.WriteLine(lWTFOutput.ToString())
        lSW.Flush()
        lSW.Close()
        lSW = Nothing
        lWTFOutput.Length = 0
        lWTFOutput = Nothing

        Dim lGraphTsGroup As New atcTimeseriesGroup()
        lGraphTsGroup.Add(RiseObj.FlowData) 'original GWL data

        'processing peak Tser before graphing
        For I As Integer = 1 To pGraphTsHPeak.numValues
            If pGraphTsHPeak.Value(I) = 0 Then
                pGraphTsHPeak.Value(I) = GetNaN()
            End If
        Next
        pGraphTsHPeak.Attributes.SetValue("Scenario", "Identified")
        pGraphTsHPeak.Attributes.SetValue("Constituent", "Hpeak")
        pGraphTsHPeak.Attributes.SetValue("point", True)
        lGraphTsGroup.Add(pGraphTsHPeak) 'only the peaks

        'processing H2 Tsers before graphing
        For I As Integer = 0 To pGraphTsGroupH2.Count - 1
            For J As Integer = 1 To pGraphTsGroupH2(I).numValues
                If pGraphTsGroupH2(I).Value(J) = 0 Then
                    pGraphTsGroupH2(I).Value(J) = GetNaN()
                End If
            Next
            pGraphTsGroupH2(I).Attributes.SetValue("point", True)
        Next
        lGraphTsGroup.AddRange(pGraphTsGroupH2) 'up to 3 projected H2 Tsers

        For I As Integer = 0 To lGraphTsGroup.Count - 1
            lGraphTsGroup(I) = SubsetByDate(lGraphTsGroup(I), lStartDateForGraph, lEndDataForGraph, Nothing)
        Next
        DoWTFGraphTimeseries("Timeseries", False, lGraphTsGroup)

        Logger.Msg("Writing output is done." & vbCrLf & lOutputFilename, "WTF Output")
    End Sub

    Private Sub WriteMonthlyYearlyOutput(ByVal aH2Method As AntecedentGWLMethod, _
                                         ByVal aTimeUnit As atcTimeUnit, _
                                         ByRef aWTFOutput As Text.StringBuilder, _
                                         ByVal aDelim As String)
        Dim lWTFOutputMonthlyTitle As String
        lWTFOutputMonthlyTitle = "*********************************" & vbNewLine
        lWTFOutputMonthlyTitle &= "Month" & aDelim & "Recharge" & vbNewLine
        lWTFOutputMonthlyTitle &= "*********************************" & vbNewLine

        Dim lWTFOutputYearlyTitle As String
        lWTFOutputYearlyTitle = "*********************************" & vbNewLine
        lWTFOutputYearlyTitle &= "Year" & aDelim & "Recharge" & vbNewLine
        lWTFOutputYearlyTitle &= "*********************************" & vbNewLine
        Dim lDate(5) As Integer
        Dim lTsRecharge As atcTimeseries = pGraphTsGroupRecharge.ItemByKey(aH2Method)
        Dim lTsRechargeAggregate As atcTimeseries = Nothing
        If lTsRecharge IsNot Nothing Then
            'debug
            'For Z As Integer = 1 To lTsRecharge.numValues
            '    If Double.IsNaN(lTsRecharge.Value(Z)) Then
            '        lTsRecharge.Value(Z) = 0.0
            '    End If
            'Next
            lTsRechargeAggregate = Aggregate(lTsRecharge, aTimeUnit, 1, atcTran.TranSumDiv)
            'debug
            'Dim lSW As New System.IO.StreamWriter("C:\Temp\Zhai.txt", False)
            'For Z As Integer = 1 To lTsRecharge.numValues
            '    If lTsRecharge.Value(Z) > 0 Then
            '        lSW.WriteLine(DumpDate(lTsRecharge.Dates.Value(Z - 1)) & vbTab & lTsRecharge.Value(Z))
            '    End If
            'Next
            'lSW.Flush() : lSW.Close() : lSW = Nothing
            If lTsRechargeAggregate IsNot Nothing Then
                Dim label As String = ""
                Select Case aH2Method
                    Case AntecedentGWLMethod.FALL : label = vbNewLine & "FALL Method "
                    Case AntecedentGWLMethod.Linear : label = vbNewLine & "Linear Model "
                    Case AntecedentGWLMethod.Power : label = vbNewLine & "Power Model "
                End Select
                Select Case aTimeUnit
                    Case atcTimeUnit.TUMonth : label &= "Monthly Summary"
                    Case atcTimeUnit.TUYear : label &= "Yearly Summary"
                End Select
                aWTFOutput.AppendLine(label)
                Select Case aTimeUnit
                    Case atcTimeUnit.TUMonth : aWTFOutput.AppendLine(lWTFOutputMonthlyTitle)
                    Case atcTimeUnit.TUYear : aWTFOutput.AppendLine(lWTFOutputYearlyTitle)
                End Select

                For I As Integer = 0 To lTsRechargeAggregate.numValues - 1
                    J2Date(lTsRechargeAggregate.Dates.Value(I), lDate)
                    Dim lTime As String = ""
                    Select Case aTimeUnit
                        Case atcTimeUnit.TUMonth : lTime = lDate(0).ToString & "-" & lDate(1).ToString
                        Case atcTimeUnit.TUYear : lTime = lDate(0).ToString
                    End Select
                    Dim lRechargeVal As Double = lTsRechargeAggregate.Value(I + 1)
                    aWTFOutput.AppendLine(lTime & aDelim & Math.Round(lRechargeVal, 2))
                Next
            End If
        End If
    End Sub
    ''' <summary>
    ''' In this version, must provide the graph timeseries group constructed from writing the outputs
    ''' btnWriteOutput_Click
    ''' </summary>
    ''' <param name="aGraphType"></param>
    ''' <param name="aPerUnitArea"></param>
    ''' <param name="aDataGroup"></param>
    ''' <remarks></remarks>
    Private Sub DoWTFGraphTimeseries(ByVal aGraphType As String, Optional ByVal aPerUnitArea As Boolean = False, Optional ByVal aDataGroup As atcData.atcTimeseriesGroup = Nothing)
        If aDataGroup Is Nothing OrElse aDataGroup.Count = 0 Then
            Exit Sub
        End If

        Dim lYAxisTitleText As String = "GW LEVEL"
        If aPerUnitArea Then lYAxisTitleText &= " (per unit square mile)"
        'If aGraphType = "CDist" Then lYAxisTitleText = "Flow (in)"
        aDataGroup(0).Attributes.SetValue("Units", "ft")
        aDataGroup(0).Attributes.SetValue("YAxis", "LEFT")

        If aGraphType = "Timeseries" Then
            DisplayTsGraph(aDataGroup)
            'ElseIf aGraphType = "Duration" Then
            '    DisplayDurGraph(lDataGroup, aPerUnitArea)
            'ElseIf aGraphType = "CDist" Then
            '    DisplayCDistGraph(lDataGroup)
        End If
    End Sub

    Private Sub DisplayTsGraph(ByVal aDataGroup As atcTimeseriesGroup)
        Dim lGraphForm As New atcGraph.atcGraphForm()
        lGraphForm.Icon = Me.Icon
        Dim lZgc As ZedGraphControl = lGraphForm.ZedGraphCtrl
        Dim lMax As Double = aDataGroup(0).Attributes.GetValue("Max")
        Dim lMin As Double = aDataGroup(0).Attributes.GetValue("Min")
        For I As Integer = 0 To aDataGroup.Count - 1
            If aDataGroup(I).Attributes.GetValue("Constituent") = "H2" Then
                lMin = aDataGroup(I).Attributes.GetValue("Min") - 2.0
                Exit For
            End If
        Next
        Dim lGraphTS As New clsGraphTime(aDataGroup, lZgc)
        lGraphForm.Grapher = lGraphTS
        With lGraphForm.Grapher.ZedGraphCtrl.GraphPane
            '.YAxis.Type = AxisType.Log
            'Dim lScaleMin As Double = 1
            
            .YAxis.Scale.MaxAuto = False
            .YAxis.Scale.MinAuto = False
            .YAxis.Scale.Min = lMin
            .YAxis.Scale.Max = lMax

            .CurveList.Item(0).Color = Drawing.Color.Red 'original GWL data
            .CurveList.Item(1).Color = Drawing.Color.DarkMagenta 'only the peaks
            If aDataGroup.Count > 2 Then
                CType(.CurveList.Item(0), LineItem).Line.Width = 1
                For I As Integer = 1 To aDataGroup.Count - 1
                    .CurveList.Item(I).Color = GetCurveColor(aDataGroup(I))
                Next
            Else
                .CurveList.Item(1).Color = Drawing.Color.DarkBlue
                CType(.CurveList.Item(1), LineItem).Line.Width = 2
            End If

            .AxisChange()
        End With
        lGraphForm.Show()
    End Sub

    Private Function GetCurveColor(ByVal aTs As atcTimeseries) As System.Drawing.Color
        Dim lCons As String = aTs.Attributes.GetValue("Constituent")
        Dim lMethod As String = aTs.Attributes.GetValue("Scenario")

        Select Case lMethod
            Case "FALL method"
                If lCons.StartsWith("H2") Then
                    Return Drawing.Color.Blue
                ElseIf lCons.StartsWith("Recharge") Then
                    Return Drawing.Color.LightBlue
                End If
            Case "Linear method"
                If lCons.StartsWith("H2") Then
                    Return Drawing.Color.DarkGreen
                ElseIf lCons.StartsWith("Recharge") Then
                    Return Drawing.Color.LightGreen
                End If
            Case "Power method"
                If lCons.StartsWith("H2") Then
                    Return Drawing.Color.DarkOrange
                ElseIf lCons.StartsWith("Recharge") Then
                    Return Drawing.Color.Orange
                End If
            Case "Identified" 'hpeak
                If lCons.StartsWith("Hpeak") Then
                    Return Drawing.Color.Cyan
                End If
        End Select
    End Function

    Private Sub txtSy_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSy.TextChanged
        Dim lV As Double
        If Not Double.IsNaN(txtSy.Text) AndAlso Double.TryParse(txtSy.Text, lV) Then
            pWTF.SpecificYield = lV
        End If
    End Sub
End Class