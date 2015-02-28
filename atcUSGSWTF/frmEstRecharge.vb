Imports atcData
Imports atcUtility
Imports atcUSGSUtility
Imports atcUSGSRecess
Imports atcGraph
Imports ZedGraph
Imports MapWinUtility

Public Class frmEstRecharge

    Private pFall As clsWTFFall
    Private pLinear As clsWTFLinear
    Private pPower As clsWTFPower

    Private RiseObj As clsFall
    Private pBasicAttributes As Generic.List(Of String)
    Private pDataGroup As atcTimeseriesGroup
    Private pWTFAttributes As atcDataAttributes

    Private pGraphTsHPeak As atcTimeseries
    Private pGraphTsGroupH2 As atcTimeseriesGroup
    Private pGraphTsGroupRecharge As atcTimeseriesGroup

    Private pDateFormat As atcDateFormat

    Private pYearStartMonth As Integer = 0
    Private pYearStartDay As Integer = 0
    Private pYearEndMonth As Integer = 0
    Private pYearEndDay As Integer = 0
    Private pFirstYear As Integer = 0
    Private pLastYear As Integer = 0

    Private pCommonStart As Double = GetMinValue()
    Private pCommonEnd As Double = GetMaxValue()
    Private Const pNoDatesInCommon As String = ": No dates in common"
    Private pAnalysisOverCommonDuration As Boolean = True

    Private pMethods As ArrayList = Nothing
    Private pOutputDir As String = ""
    'Private pBaseOutputFilename As String = ""
    Private pMethodLastDone As String = ""
    'Private pMethodsLastDone As ArrayList = Nothing
    Private pDALastUsed As Double = 0.0
    Public OutputFilenameRoot As String

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        pFall = New clsWTFFall()
        pLinear = New clsWTFLinear()
        pPower = New clsWTFPower()
        gbFall.Visible = False
        gbLinear.Visible = False
        gbPower.Visible = False
    End Sub

    Public Sub Initialize(ByVal aDataGroup As atcTimeseriesGroup, _
                          ByVal aRiseObj As clsFall, _
                          Optional ByVal aShowForm As Boolean = True)
        pDataGroup = aDataGroup
        RiseObj = aRiseObj
        If pDataGroup IsNot Nothing AndAlso pDataGroup.Count > 0 Then
            PopulateForm()
            If aShowForm Then Me.Show()
        Else
            Logger.Msg("Need to select groundwater elevation data first.")
            Me.Close()
        End If
    End Sub
    Private Sub PopulateForm()
        pDateFormat = New atcDateFormat
        With pDateFormat
            .IncludeHours = False
            .IncludeMinutes = False
            .IncludeSeconds = False
        End With

        pOutputDir = GetSetting("atcWTF", "Defaults", "OutputDir", "")
        If String.IsNullOrEmpty(pOutputDir) OrElse Not IO.Directory.Exists(pOutputDir) Then
            Dim lOutputDirectory As String = IO.Path.GetDirectoryName(pDataGroup(0).Attributes.GetValue("History 1").ToString.Replace("Read from", "").Trim())
            pOutputDir = lOutputDirectory
        End If
        OutputFilenameRoot = GetSetting("atcWTF", "Defaults", "BaseOutputFilename", "")

        'atcUSGSStations.StationInfoFile = GetSetting("atcUSGSBaseflow", "Defaults", "Stations", "Station.txt")

        RepopulateForm()
    End Sub

    Private Sub RepopulateForm()
        Dim lFirstDate As Double = GetMaxValue()
        Dim lLastDate As Double = GetMinValue()

        pCommonStart = GetMinValue()
        pCommonEnd = GetMaxValue()

        For Each lDataset As atcData.atcTimeseries In pDataGroup
            If lDataset.Dates.numValues > 0 Then
                Dim lThisDate As Double = lDataset.Dates.Value(0)
                If lThisDate < lFirstDate Then lFirstDate = lThisDate
                If lThisDate > pCommonStart Then pCommonStart = lThisDate
                lThisDate = lDataset.Dates.Value(lDataset.Dates.numValues)
                If lThisDate > lLastDate Then lLastDate = lThisDate
                If lThisDate < pCommonEnd Then pCommonEnd = lThisDate
            End If
            
        Next
        Dim lFallD As Double = pDataGroup(0).Attributes.GetValue("FallD", -99)
        Dim lFallKgw As Double = pDataGroup(0).Attributes.GetValue("FallKgw", -99)
        If lFallD > 0 AndAlso lFallKgw > 0 Then
            txtFallD.Text = lFallD.ToString()
            Dim lIndex As Double = -1 * (1 / lFallKgw)
            txtFallKgw.Text = DoubleToString(lIndex)
        End If
        Dim lSpecificYield As String = GetSetting("atcWTF", "Defaults", "SpecificYield", "")
        txtSy.Text = lSpecificYield

        If lFirstDate < GetMaxValue() AndAlso lLastDate > GetMinValue() Then
            If txtDataStart.Tag IsNot Nothing AndAlso txtDataEnd.Tag IsNot Nothing Then
                txtDataStart.Text = txtDataStart.Tag & " " & pDateFormat.JDateToString(lFirstDate + 1)
                txtDataEnd.Text = txtDataEnd.Tag & " " & pDateFormat.JDateToString(lLastDate)
            Else
                txtDataStart.Text = pDateFormat.JDateToString(lFirstDate + 1)
                txtDataEnd.Text = pDateFormat.JDateToString(lLastDate)
            End If
            If txtStartDateUser.Text.Length = 0 Then
                txtStartDateUser.Text = txtDataStart.Text
            End If
            If txtEndDateUser.Text.Length = 0 Then
                txtEndDateUser.Text = txtDataEnd.Text
            End If
        End If

        txtOutputDir.Text = pOutputDir
        txtOutputRootName.Text = OutputFilenameRoot
    End Sub

    Private Function UpdateGWLParametersFromForm() As String
        Dim lParamErrMsg As String = ""
        If chkAntMethodFall.Checked Then
            With pFall
                Dim lFallD As Double
                Dim lFallKgw As Double
                If Double.TryParse(txtFallD.Text, lFallD) AndAlso _
                   Double.TryParse(txtFallKgw.Text, lFallKgw) Then
                    .GWLAsymptote = lFallD
                    .KGWL = lFallKgw
                    .ParametersSet = True
                Else
                    .ParametersSet = False
                    lParamErrMsg &= "- Semi-log method parameters invalid." & vbCrLf
                End If
            End With
        End If
        If chkAntMethodLinear.Checked Then
            With pLinear
                Dim lA As Double
                Dim lB As Double
                If Double.TryParse(txtLinearA.Text, lA) AndAlso _
                   Double.TryParse(txtLinearB.Text, lB) Then
                    .LinearSlope = lA
                    .LinearIntercept = lB
                    .ParametersSet = True
                Else
                    .ParametersSet = False
                    lParamErrMsg &= "- Linear method parameters invalid." & vbCrLf
                End If
            End With
        End If
        If chkAntMethodPower.Checked Then
            With pPower
                Dim lPowerCIntercept As Double
                Dim lPowerEDatum As Double
                Dim lPowerFExp As Double
                Dim lPowerDMultiplier As Double

                If Double.TryParse(txtPowerIntercept.Text, lPowerCIntercept) AndAlso _
                   Double.TryParse(txtPowerMultiplier.Text, lPowerDMultiplier) AndAlso _
                   Double.TryParse(txtPowerDatum.Text, lPowerEDatum) AndAlso _
                    Double.TryParse(txtPowerExp.Text, lPowerFExp) Then
                    .ParamCIntercept = lPowerCIntercept
                    .ParamDMultiplier = lPowerDMultiplier
                    .ParamEDatum = lPowerEDatum
                    .ParamFExp = lPowerFExp
                    pPower.ParametersSet = True
                Else
                    pPower.ParametersSet = False
                    lParamErrMsg &= "- Power method parameters invalid." & vbCrLf
                End If
            End With
        End If
        Dim lSy As Double
        If Double.TryParse(txtSy.Text, lSy) Then
            If lSy <= 1 AndAlso lSy > 0 Then
                If pFall IsNot Nothing Then pFall.SpecificYield = lSy
                If pLinear IsNot Nothing Then pLinear.SpecificYield = lSy
                If pPower IsNot Nothing Then pPower.SpecificYield = lSy
            Else
                lParamErrMsg &= "- Specific yield value is outside valid range (0 ~ 1)." & vbCrLf
            End If
        Else
            lParamErrMsg &= "- Specific yield value is invalid." & vbCrLf
        End If
        Return lParamErrMsg
    End Function

    Private Sub txtSy_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim lV As Double
        Dim lSyText As String = txtSy.Text.Trim()
        If Not Double.IsNaN(lSyText) AndAlso Double.TryParse(lSyText, lV) Then
            pFall.SpecificYield = lV
        End If
    End Sub

    Private Sub chkAntMethod_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkAntMethodFall.Click, _
    chkAntMethodLinear.Click, _
    chkAntMethodPower.Click
        gbFall.Visible = False
        gbLinear.Visible = False
        gbPower.Visible = False
        Dim lchkBoxName As String = CType(sender, Windows.Forms.CheckBox).Name
        Dim lMethod As AntecedentGWLMethod = -9
        If String.Compare(lchkBoxName, "chkAntMethodFall", True) = 0 Then
            gbFall.Dock = Windows.Forms.DockStyle.Fill
            gbFall.Visible = True
            lMethod = AntecedentGWLMethod.FALL
        ElseIf String.Compare(lchkBoxName, "chkAntMethodLinear", True) = 0 Then
            gbLinear.Dock = Windows.Forms.DockStyle.Fill
            gbLinear.Visible = True
            lMethod = AntecedentGWLMethod.Linear
        ElseIf String.Compare(lchkBoxName, "chkAntMethodPower", True) = 0 Then
            gbPower.Dock = Windows.Forms.DockStyle.Fill
            gbPower.Visible = True
            lMethod = AntecedentGWLMethod.Power
        End If
        Select Case lMethod
            Case AntecedentGWLMethod.FALL
                If pFall Is Nothing Then pFall = New clsWTFFall()
                Dim lFallDVal As Double
                Dim lFallKgwVal As Double
                Dim lReAssign As Boolean = True
                If Double.TryParse(txtFallD.Text, lFallDVal) AndAlso Double.TryParse(txtFallKgw.Text, lFallKgwVal) Then
                    If Not Double.IsNaN(lFallDVal) AndAlso Not Double.IsNaN(lFallKgwVal) Then
                        lReAssign = False
                    End If
                End If
                If lReAssign Then
                    Dim lFallD As Double = pDataGroup(0).Attributes.GetValue("FallD", -99)
                    Dim lFallKgw As Double = pDataGroup(0).Attributes.GetValue("FallKgw", -99)
                    If lFallD > 0 AndAlso lFallKgw > 0 Then
                        pFall.GWLAsymptote = lFallD
                        txtFallD.Text = lFallD.ToString()
                        Dim lKgwIndex As Double = -1 * (1 / lFallKgw)
                        pFall.KGWL = lKgwIndex
                        txtFallKgw.Text = DoubleToString(lKgwIndex)
                    End If
                End If
            Case AntecedentGWLMethod.Linear
                If pLinear Is Nothing Then pLinear = New clsWTFLinear()
                txtLinearA.Text = pLinear.LinearIntercept.ToString
                txtLinearB.Text = pLinear.LinearSlope.ToString
            Case AntecedentGWLMethod.Power
                If pPower Is Nothing Then pPower = New clsWTFPower()
                txtPowerDatum.Text = pPower.ParamEDatum.ToString
                txtPowerExp.Text = pPower.ParamFExp.ToString
                txtPowerIntercept.Text = pPower.ParamCIntercept.ToString
                txtPowerMultiplier.Text = pPower.ParamDMultiplier.ToString
        End Select
    End Sub

    Private Sub btnExamineData_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExamineData.Click
        For Each lTs As atcTimeseries In pDataGroup
            Dim lfrmDataSummary As New atcUSGSUtility.frmDataSummary(atcUSGSScreen.PrintDataSummary(lTs))
            lfrmDataSummary.ClearSelection()
            lfrmDataSummary.Show()
        Next
    End Sub

    Private Sub btnWriteASCIIOutput_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnWriteASCIIOutput.Click
        If RiseObj Is Nothing OrElse RiseObj.listOfSegments Is Nothing OrElse RiseObj.listOfSegments.Count = 0 Then
            'Is not ready to do the recharge caculation as no recharge events have been found
            Logger.Msg("Must find all recharge events before calculating recharge volume.", "Need to find recharge events")
            Exit Sub
        End If

        Dim lParamErrMsg As String = UpdateGWLParametersFromForm()
        If Not String.IsNullOrEmpty(lParamErrMsg) Then
            Logger.Msg("Please address the following issues before proceed." & vbCrLf & _
                       lParamErrMsg, MsgBoxStyle.Exclamation, "WTF Estimate Recharge")
            Exit Sub
        End If

        Dim lRechargeEstFallOk As Boolean = True
        Dim lRechargeEstLinearOk As Boolean = True
        Dim lRechargeEstPowerOk As Boolean = True
        If chkAntMethodFall.Checked Then
            If Not pFall.Recharge(RiseObj) Then
                Logger.Msg("Semi-log recharge esimation failed")
                lRechargeEstFallOk = False
            End If
        End If
        If chkAntMethodLinear.Checked Then
            If Not pLinear.Recharge(RiseObj) Then
                Logger.Msg("Linear method recharge esimation failed")
                lRechargeEstLinearOk = False
            End If
        End If
        If chkAntMethodPower.Checked Then
            If Not pPower.Recharge(RiseObj) Then
                Logger.Msg("Power method recharge esimation failed")
                lRechargeEstPowerOk = False
            End If
        End If

        Dim lOutputDirectory As String = IO.Path.GetDirectoryName(pDataGroup(0).Attributes.GetValue("History 1").ToString.Replace("Read from", "").Trim())
        Dim lOutputFilename As String = ""
        Dim lFD As New System.Windows.Forms.SaveFileDialog()
        With lFD
            .InitialDirectory = lOutputDirectory
            .Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*"
            .FilterIndex = 1
            .Title = "WTF Output File"
            .FileName = txtOutputRootName.Text & ".txt"
            If .ShowDialog() = System.Windows.Forms.DialogResult.OK Then
                lOutputFilename = .FileName
            End If
        End With

        If String.IsNullOrEmpty(lOutputFilename) Then
            Logger.Msg("Please specify output filename.")
            Exit Sub
        End If

        Dim lDelim As String = vbTab
        Dim lWTFOutputDailyTitle As New Text.StringBuilder()
        Dim lWTFOutput As New Text.StringBuilder()

        'Individual recharge event output

        'Monthly and Yearly Recharge output
        'At the same time construct two separate timeseries (point type) for H2 and Hpeak

        'Dim lNP As atcTimeseries
        'Dim lP As atcTimeseries
        'SplitProvisional(pDataGroup(0), lP, lNP)
        Dim lSegCtr As Integer = 1
        Dim lTsDailyRecharge As atcTimeseries = pDataGroup(0).Clone()
        Dim lTsDailyH2 As atcTimeseries = pDataGroup(0).Clone()
        pGraphTsHPeak = pDataGroup(0).Clone()
        If pGraphTsGroupH2 Is Nothing Then
            pGraphTsGroupH2 = New atcTimeseriesGroup()
        Else
            pGraphTsGroupH2.Clear()
        End If
        If pGraphTsGroupRecharge Is Nothing Then
            pGraphTsGroupRecharge = New atcTimeseriesGroup()
        Else
            pGraphTsGroupRecharge.Clear()
        End If

        For I As Integer = 1 To pGraphTsHPeak.numValues
            lTsDailyRecharge.Value(I) = Double.NaN
            lTsDailyH2.Value(I) = Double.NaN
            pGraphTsHPeak.Value(I) = Double.NaN
        Next

        'Title line
        lWTFOutputDailyTitle.Append("Count" & lDelim & _
                          "TsDate" & lDelim & _
                          "Ts Duration" & lDelim & _
                          "Ts GWL" & lDelim & _
                          "GWLpeak" & lDelim & _
                          "H0Date" & lDelim & _
                          "H0 GWL" & lDelim)
        If RiseObj.listOfSegments(0).AntecedentGWLMethods.Keys.Contains(AntecedentGWLMethod.FALL) Then
            lWTFOutputDailyTitle.Append("FALLH2" & lDelim & "(GWLpeak-FALLH2)")
        End If
        If RiseObj.listOfSegments(0).AntecedentGWLMethods.Keys.Contains(AntecedentGWLMethod.Linear) Then
            lWTFOutputDailyTitle.Append("LinearH2" & lDelim & "(GWLpeak-LinearH2)")
        End If
        If RiseObj.listOfSegments(0).AntecedentGWLMethods.Keys.Contains(AntecedentGWLMethod.Power) Then
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
                Dim lIndexOfHpeak As Double = .PeakDayIndex + .Flow.Length - 1 - 1
                pGraphTsHPeak.Value(lIndexOfHpeak) = lGWLpeak
                If .AntecedentGWLMethods.Keys.Contains(AntecedentGWLMethod.FALL) Then
                    Dim lH2 As Double = Math.Round(.AntecedentGWLMethods.ItemByKey(AntecedentGWLMethod.FALL), 3)
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
                If .AntecedentGWLMethods.Keys.Contains(AntecedentGWLMethod.Linear) Then
                    Dim lH2 As Double = .AntecedentGWLMethods.ItemByKey(AntecedentGWLMethod.Linear)
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
                If .AntecedentGWLMethods.Keys.Contains(AntecedentGWLMethod.Power) Then
                    Dim lH2 As Double = .AntecedentGWLMethods.ItemByKey(AntecedentGWLMethod.Power)
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

        ''processing peak Tser before graphing
        'For I As Integer = 1 To pGraphTsHPeak.numValues
        '    If pGraphTsHPeak.Value(I) = 0 Then
        '        pGraphTsHPeak.Value(I) = GetNaN()
        '    End If
        'Next
        pGraphTsHPeak.Attributes.SetValue("Scenario", "Identified")
        pGraphTsHPeak.Attributes.SetValue("Constituent", "Hpeak")
        pGraphTsHPeak.Attributes.SetValue("point", False) 'change it later
        lGraphTsGroup.Add(pGraphTsHPeak) 'only the peaks

        'processing H2 Tsers before graphing
        For I As Integer = 0 To pGraphTsGroupH2.Count - 1
            'For J As Integer = 1 To pGraphTsGroupH2(I).numValues
            '    If pGraphTsGroupH2(I).Value(J) = 0 Then
            '        pGraphTsGroupH2(I).Value(J) = GetNaN()
            '    End If
            'Next
            pGraphTsGroupH2(I).Attributes.SetValue("point", False) 'change it later
        Next
        lGraphTsGroup.AddRange(pGraphTsGroupH2) 'up to 3 projected H2 Tsers

        Dim lGraphTsGroupSubsetDates As New atcTimeseriesGroup()
        For I As Integer = 0 To lGraphTsGroup.Count - 1
            Dim lTs As atcTimeseries = SubsetByDate(lGraphTsGroup(I), lStartDateForGraph, lEndDataForGraph, Nothing)
            Dim lcons As String = lTs.Attributes.GetValue("Constituent")
            Select Case lcons.ToUpper()
                Case "H2", "HPEAK"
                    lTs.Attributes.SetValue("Point", True)
            End Select
            lGraphTsGroupSubsetDates.Add(lTs)
        Next
        'Dim lSWdbg As New IO.StreamWriter("C:\temp\GraphTs.txt", False)
        'For I As Integer = 1 To lGraphTsGroupSubsetDates(0).numValues
        '    Dim lDateS As Double = lGraphTsGroupSubsetDates(0).Dates.Value(I - 1)
        '    Dim lGWL As Double = lGraphTsGroupSubsetDates(0).Value(I)
        '    Dim lPeak As Double = lGraphTsGroupSubsetDates(1).Value(I)
        '    Dim lH2 As Double = lGraphTsGroupSubsetDates(2).Value(I)
        '    If Double.IsNaN(lGWL) Then lGWL = 0
        '    If Double.IsNaN(lPeak) Then lPeak = 0
        '    If Double.IsNaN(lH2) Then lH2 = 0
        '    lSWdbg.WriteLine(lDateS & vbTab & lGWL & vbTab & lPeak & vbTab & lH2)
        'Next
        'lSWdbg.Flush()
        'lSWdbg.Close()
        'lSWdbg = Nothing

        DoWTFGraphTimeseries("Timeseries", False, lGraphTsGroupSubsetDates)

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
        For Each lTs As atcTimeseries In aDataGroup
            lTs.Attributes.SetValue("Units", "ft")
            lTs.Attributes.SetValue("YAxis", "LEFT")
        Next

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
            .CurveList.Item(1).Color = Drawing.Color.DarkBlue 'only the peaks
            If aDataGroup.Count > 2 Then
                CType(.CurveList.Item(0), LineItem).Line.Width = 1
                For I As Integer = 1 To aDataGroup.Count - 1
                    .CurveList.Item(I).Color = GetCurveColor(aDataGroup(I))
                Next
            Else
                .CurveList.Item(1).Color = Drawing.Color.DarkBlue
                CType(.CurveList.Item(1), LineItem).Line.Width = 2
            End If
            .YAxis.Scale.MaxAuto = True
            .YAxis.Scale.MinAuto = True
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
                    Return Drawing.Color.Magenta
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
                    Return Drawing.Color.Blue
                End If
        End Select
    End Function

    Private Sub btnPlot_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPlot.Click
        'DoWTFGraphTimeseries("")
    End Sub

    Private Sub txtParamValues_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtFallD.TextChanged, _
    txtFallKgw.TextChanged, txtLinearA.TextChanged, txtLinearB.TextChanged, txtPowerDatum.TextChanged, _
    txtPowerExp.TextChanged, txtPowerIntercept.TextChanged, txtPowerMultiplier.TextChanged

        Dim ltxtBoxName As String = CType(sender, Windows.Forms.TextBox).Name
        Select Case ltxtBoxName
            Case "txtFallD"
                Double.TryParse(txtFallD.Text, pFall.GWLAsymptote)
            Case "txtFallKgw"
                Double.TryParse(txtFallKgw.Text, pFall.KGWL)
            Case "txtLinearA"
                Double.TryParse(txtLinearA.Text, pLinear.LinearSlope)
            Case "txtLinearB"
                Double.TryParse(txtLinearB.Text, pLinear.LinearIntercept)
            Case "txtPowerDatum"
                Double.TryParse(txtPowerDatum.Text, pPower.ParamEDatum)
            Case "txtPowerExp"
                Double.TryParse(txtPowerExp.Text, pPower.ParamFExp)
            Case "txtPowerIntercept"
                Double.TryParse(txtPowerIntercept.Text, pPower.ParamCIntercept)
            Case "txtPowerMultiplier"
                Double.TryParse(txtPowerMultiplier.Text, pPower.ParamDMultiplier)
        End Select
    End Sub
End Class