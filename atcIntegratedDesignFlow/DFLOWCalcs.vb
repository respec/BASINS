Imports atcData
Imports System.Windows.Forms
Imports System.IO
Imports atcUtility
Imports MapWinUtility

Public Class DFLOWCalcs
    Friend Structure stExcursion
        Public Start As Integer
        Public Finish As Integer
        Public SumLength As Integer
        Public Count As Integer
        Public SumMag As Double
    End Structure
    Friend Structure stCluster
        Public Start As Integer
        Public Finish As Integer
        Public Excursions As Integer
        Public Events As Integer
    End Structure

    Public Shared eps = 0.005

    Public Shared fBioDefault As Boolean
    Public Shared fBioType As Integer
    Public Shared fBioPeriod As Integer
    Public Shared fBioYears As Integer
    Public Shared fBioCluster As Integer
    Public Shared fBioExcursions As Integer
    Public Shared fBioFPArray(,) As Integer = New Integer(3, 3) {{1, 3, 120, 5}, {4, 3, 120, 5}, {30, 3, 120, 5}, {-1, -1, -1, -1}}
    Public Shared fNonBioType As Integer
    Public Shared fAveragingPeriod As Integer
    Public Shared fReturnPeriod As Integer
    Public Shared fExplicitFlow As Double
    Public Shared fPercentile As Double

    Public Shared fStartDay As Integer = 1
    Public Shared fStartMonth As Integer = 4
    Public Shared fEndDay As Integer = 31
    Public Shared fEndMonth As Integer = 3
    Public Shared fFirstYear As Integer = -1
    Public Shared fLastYear As Integer = -1

    Public Shared fLastDay() As Integer = {31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31}
    Public Shared fMonth3 As String = "JanFebMarAprMayJunJulAugSepOctNovDec"

    Public Shared DFLOWMessage As Text.StringBuilder

    Public Shared LabelYears As String = ""
    Public Shared LabelSeasons As String = ""

    Public Shared Function Sig2(ByVal x As Double) As String
        Return DoubleToString(x)
        'If x >= 100 Then
        '    Sig2 = Format(x, "Scientific")
        'ElseIf x >= 10 Then
        '    Sig2 = Format(x, "00.0")
        'Else
        '    Sig2 = Format(x, "0.00")
        'End If
    End Function

    Public Shared Sub Initialize(Optional ByVal aChoice As atcDataAttributes = Nothing)
        If aChoice Is Nothing Then
            ' This sets the initial values for DFLOW calculations - CMC, 7Q10
            fBioDefault = True
            fBioType = 0
            fBioPeriod = 1
            fBioYears = 3
            fBioCluster = 120
            fBioExcursions = 5

            fNonBioType = 0
            fAveragingPeriod = 7
            fReturnPeriod = 10
            fExplicitFlow = 1.0
            fPercentile = 0.1

            aChoice = New atcDataAttributes()
            With aChoice
                .SetValue(InputNamesDFLOW.EBioDFlowType.Acute_maximum_concentration.ToString(), GetBioDefaultParams(InputNamesDFLOW.EBioDFlowType.Acute_maximum_concentration))
                .SetValue(InputNamesDFLOW.EBioDFlowType.Chronic_continuous_concentration.ToString(), GetBioDefaultParams(InputNamesDFLOW.EBioDFlowType.Chronic_continuous_concentration))
                .SetValue(InputNamesDFLOW.EBioDFlowType.Ammonia.ToString(), GetBioDefaultParams(InputNamesDFLOW.EBioDFlowType.Ammonia))
                .SetValue(InputNamesDFLOW.EDFlowType.Hydrological.ToString(), New Integer() {7, 10})
                .SetValue(InputNamesDFLOW.EDFlowType.Explicit_Flow_Value.ToString(), 1.0)
                .SetValue(InputNamesDFLOW.EDFlowType.Flow_Percentile.ToString(), 0.1)
            End With
        Else
            With aChoice
                fBioDefault = .GetValue(InputNamesDFLOW.BioUseDefault, True)
                fBioType = .GetValue(InputNamesDFLOW.BioSelectedMethod, InputNamesDFLOW.EBioDFlowType.Acute_maximum_concentration)
                Dim lBio4Params() As Integer '= GetBioDefaultParams(fBioType)
                If fBioDefault Then
                    lBio4Params = GetBioDefaultParams(fBioType)
                Else
                    Select Case fBioType
                        Case InputNamesDFLOW.EBioDFlowType.Acute_maximum_concentration
                            lBio4Params = .GetValue(InputNamesDFLOW.EBioDFlowType.Acute_maximum_concentration.ToString(), GetBioDefaultParams(fBioType))
                        Case InputNamesDFLOW.EBioDFlowType.Chronic_continuous_concentration
                            lBio4Params = .GetValue(InputNamesDFLOW.EBioDFlowType.Chronic_continuous_concentration.ToString(), GetBioDefaultParams(fBioType))
                        Case InputNamesDFLOW.EBioDFlowType.Ammonia
                            lBio4Params = .GetValue(InputNamesDFLOW.EBioDFlowType.Ammonia.ToString(), GetBioDefaultParams(fBioType))
                        Case Else
                            Throw New ApplicationException("Could not set parameters for Bio Type " & InputNamesDFLOW.BioSelectedMethod & " " & fBioType)
                    End Select
                End If
                fBioPeriod = lBio4Params(0)
                fBioYears = lBio4Params(1)
                fBioCluster = lBio4Params(2)
                fBioExcursions = lBio4Params(3)

                fNonBioType = .GetValue(InputNamesDFLOW.NBioSelectedMethod, InputNamesDFLOW.EDFlowType.Hydrological)
                Dim lHydro2Params() As Integer = .GetValue(InputNamesDFLOW.EDFlowType.Hydrological.ToString(), New Integer() {7, 10})
                fAveragingPeriod = lHydro2Params(0)
                fReturnPeriod = lHydro2Params(1)

                fExplicitFlow = .GetValue(InputNamesDFLOW.EDFlowType.Explicit_Flow_Value.ToString(), 1.0)
                fPercentile = .GetValue(InputNamesDFLOW.EDFlowType.Flow_Percentile.ToString(), 0.1)
            End With
        End If
    End Sub

    Public Shared Function GetBioDefaultParams(ByVal aBioType As Integer) As Integer()
        Dim lBioParam(3) As Integer
        For I As Integer = 0 To UBound(fBioFPArray, 1)
            If I = aBioType Then
                For p As Integer = 0 To 3
                    lBioParam(p) = fBioFPArray(I, p)
                Next
            End If
        Next
        Return lBioParam
    End Function

    Public Shared Function xQy(ByVal aDays As Integer,
                        ByVal aYears As Double,
                        ByVal aDataSet As atcTimeseries,
                        Optional ByVal aInputs As atcDataAttributes = Nothing) As Double
        Dim lResult As Double = GetNaN()
        Dim lAttrName As String
        lAttrName = aDays & "Low" & aYears
        'If Not aDataSet.Attributes.ContainsAttribute(lAttrName) Then
        Try
            Dim lLogFlag As Boolean = True
            Dim lHigh As Boolean = False
            Dim lOperationName As String = "n-day low value"

            Dim lBoundaryMonth As Integer = fStartMonth '4
            Dim lBoundaryDay As Integer = fStartDay '1
            Dim lEndMonth As Integer = fEndMonth '3
            Dim lEndDay As Integer = fEndDay '31
            Dim lBatchMode As Boolean = False
            If aInputs IsNot Nothing Then
                lBatchMode = True
                With aInputs
                    If .ContainsAttribute(InputNamesDFLOW.StartMonth) Then lBoundaryMonth = .GetValue(InputNamesDFLOW.StartMonth)
                    If .ContainsAttribute(InputNamesDFLOW.StartDay) Then lBoundaryDay = .GetValue(InputNamesDFLOW.StartDay)
                    If .ContainsAttribute(InputNamesDFLOW.EndMonth) Then lEndMonth = .GetValue(InputNamesDFLOW.EndMonth)
                    If .ContainsAttribute(InputNamesDFLOW.EndDay) Then lEndDay = .GetValue(InputNamesDFLOW.EndDay)
                End With
            End If

            Dim lArgs As New atcDataAttributes
            With lArgs
                .SetValue("Timeseries", aDataSet)
                .SetValue("LogFlg", lLogFlag)
                .SetValue("HighFlag", lHigh)
                .SetValue("BoundaryMonth", lBoundaryMonth)
                .SetValue("BoundaryDay", lBoundaryDay)
                .SetValue("EndMonth", lEndMonth)
                .SetValue("EndDay", lEndDay)
                Dim lNdays(1) As Double
                lNdays(0) = aDays
                .SetValue("NDay", lNdays)
                Dim lReturns(1) As Double
                lReturns(0) = aYears
                .SetValue("Return Period", lReturns)
            End With

            Dim lCalculator As New atcTimeseriesNdayHighLow.atcTimeseriesNdayHighLow
            If lCalculator.Open(lOperationName, lArgs) AndAlso lCalculator.DataSets.Count = 1 Then
                lResult = lCalculator.DataSets(0).Attributes.GetValue(lAttrName, lResult)
            Else
                If lBatchMode Then
                    If DFLOWMessage Is Nothing Then
                        DFLOWMessage = New Text.StringBuilder()
                    End If
                    DFLOWMessage.AppendLine(aDataSet.ToString & vbCrLf _
                               & "LogFlg=" & lLogFlag & vbCrLf _
                               & "HighFlag=" & lHigh & vbCrLf _
                               & "BoundaryMonth=" & lBoundaryMonth & vbCrLf _
                               & "BoundaryDay=" & lBoundaryDay & vbCrLf _
                               & "Return Period=" & aYears & vbCrLf _
                               & "Could not create " & aDays & "-day timeseries" & vbCrLf)
                Else
                    Logger.Msg(aDataSet.ToString & vbCrLf _
                               & "LogFlg=" & lLogFlag & vbCrLf _
                               & "HighFlag=" & lHigh & vbCrLf _
                               & "BoundaryMonth=" & lBoundaryMonth & vbCrLf _
                               & "BoundaryDay=" & lBoundaryDay & vbCrLf _
                               & "Return Period=" & aYears,
                               "Could not create " & aDays & "-day timeseries")

                End If
            End If
        Catch e As Exception
            MessageBox.Show("Could not calculate value for " & lAttrName & ". " & e.ToString)
        End Try
        'End If
        Return lResult
    End Function

    Public Shared Function xBy(ByVal aDesignFlow As Double,
                         ByVal aDays As Integer,
                         ByVal aYears As Integer,
                         ByVal aMaxDays As Integer,
                         ByVal aMaxExcursions As Double,
                         ByVal aFlowRecord As Double(),
                         ByRef aExcursionCount As Integer,
                         ByRef aExcursions As ArrayList,
                         ByRef aClusters As ArrayList,
                         ByRef afrmProgress As frmDFLOWProgress)

        Dim lMaxExcursionsAllowed As Double = UBound(aFlowRecord, 1) / 365 / aYears
        ''MessageBox.Show("Entered xBy", UBound(aFlowRecord, 1) & " " & aFlowRecord(1))
        'Using sw As StreamWriter = New StreamWriter("c:\TestFile.txt")
        '    ' Add some text to the file.
        '    Dim li As Integer
        '    For li = 0 To UBound(aFlowRecord) - 1
        '        sw.WriteLine(aFlowRecord(li))

        '    Next
        '    sw.Close()
        'End Using


        Dim lFL As Double
        Dim lFU As Double
        Dim lExcL As Double
        Dim lExcU As Double

        Dim lExcursions As New ArrayList
        Dim lClusters As New ArrayList
        Dim lIters As Integer = 0

        If aDesignFlow > 0 And Not Double.IsNaN(aDesignFlow) Then

            lFL = 0
            lExcL = 0

            lFU = aDesignFlow
            lExcU = CountExcursions(lFU, aDays, aMaxDays, aMaxExcursions, aFlowRecord, lExcursions, lClusters)
            Do While (lExcU <= lMaxExcursionsAllowed)
                lFU = lFU * 2 + 1
                lExcU = CountExcursions(lFU, aDays, aMaxDays, aMaxExcursions, aFlowRecord, lExcursions, lClusters)
            Loop
            Do While ((lFU - lFL) >= eps * lFU) And (Math.Abs(lFU - lFL) >= 0.1) And (Math.Abs(lMaxExcursionsAllowed - lExcL) >= 0.005)
                If (Math.Abs(lMaxExcursionsAllowed - lExcU) < 0.005) Then
                    lFL = lFU
                Else
                    Dim lFt As Double
                    Dim lExcT As Double
                    lFt = lFL + (lFU - lFL) * (lMaxExcursionsAllowed - lExcL) / (lExcU - lExcL)
                    lExcT = CountExcursions(lFt, aDays, aMaxDays, aMaxExcursions, aFlowRecord, lExcursions, lClusters)
                    lIters = lIters + 1
                    'Logger.Status(aDays & "B" & aYears & ": " & Format(lFt, "Fixed") & " (" & lIters & ")")
                    If afrmProgress IsNot Nothing Then
                        afrmProgress.Label1.Text = afrmProgress.Label1.Text.Substring(0, afrmProgress.Label1.Text.IndexOf("-") + 2) & aDays & "B" & aYears & ": " & Format(lFt, "Fixed") & " (" & lIters & ")"
                    End If

                    Application.DoEvents()

                    If (lExcT <= lMaxExcursionsAllowed) Then
                        lFL = lFt
                        lExcL = lExcT

                        Dim lexc As stExcursion
                        aExcursions.Clear()
                        For Each lexc In lExcursions
                            aExcursions.Add(lexc)
                        Next
                        Dim lclu As stCluster
                        aClusters.Clear()
                        For Each lclu In lClusters
                            aClusters.Add(lclu)
                        Next
                    Else
                        lFU = lFt
                        lExcU = lExcT

                    End If
                End If
            Loop

            aExcursionCount = lExcL
            Return lFL
        Else
            Return GetNaN()
        End If
    End Function


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="aDesignFlow"></param>
    ''' <param name="aDays"></param>
    ''' <param name="aMaxDays"></param>
    ''' <param name="aFlowRecord"></param>
    ''' <param name="aExcursions"></param>
    ''' <remarks></remarks>
    Public Shared Function CountExcursions(ByVal aDesignFlow As Double,
                               ByVal aDays As Integer,
                               ByVal aMaxDays As Integer,
                               ByVal aMaxExc As Integer,
                               ByVal aFlowRecord As Array,
                               ByRef aExcursions As ArrayList,
                               ByRef aClusters As ArrayList) As Integer



        ' Returns number of biologically-based excursions of design flow in
        ' flow record. Excursion information is stored in aExcursions array
        Dim lExcursion As stExcursion
        With lExcursion
            .Start = 0
            .Finish = -1
            .SumLength = 0.0
            .Count = 0
            .SumMag = 0.0
        End With

        aExcursions.Clear()

        Dim lDay As Integer
        For lDay = 0 To UBound(aFlowRecord) - 1

            If aFlowRecord(lDay) < aDesignFlow And Not Double.IsNaN(aFlowRecord(lDay)) Then

                ' ----- N-day averaged flow is below design flow - append to excursions !! CHECK USE of lExcursion below

                If lDay > lExcursion.Finish + 1 Or (lExcursion.Finish - lExcursion.Start) >= aMaxDays Then

                    ' ----- It's a new excursion if it's not connected to last excursion, 
                    '       or if the last excursion is too long.

                    aExcursions.Add(lExcursion)

                    With lExcursion
                        .Start = lDay
                        .Finish = lDay - 1
                        .SumLength = 0.0
                        .Count = 0
                        .SumMag = 0.0
                    End With

                End If

                ' ----- Incorporate "today" into current excursion

                Dim lLength As Integer = lDay + aDays - 1 - lExcursion.Finish

                With lExcursion

                    If lLength > 0 Then
                        .SumLength = .SumLength + lLength / aDays
                        .Finish = .Finish + lLength
                    End If

                    .Count = .Count + 1
                    If aFlowRecord(lDay) = 0 Then
                        .SumMag = .SumMag + aDesignFlow / 0.001
                    Else
                        .SumMag = .SumMag + aDesignFlow / aFlowRecord(lDay)
                    End If

                End With

            End If

        Next

        ' ----- Store last excursion if active at end of period

        If lExcursion.Count > 0 Then
            aExcursions.Add(lExcursion)
        End If


        ' ----- Process clusters


        Dim lCluster As stCluster
        With lCluster
            .Start = -aMaxDays
            .Finish = 1
            .Excursions = 0
        End With

        aClusters.Clear()

        Dim lEx As Integer
        For lEx = 2 To aExcursions.Count
            ' Loop through excursions
            lExcursion = aExcursions.Item(lEx - 1)
            If lExcursion.Finish - lCluster.Start > aMaxDays Then
                aClusters.Add(lCluster)
                With lCluster
                    .Start = lExcursion.Start
                    .Finish = lEx
                    .Excursions = 0
                    .Events = 0
                End With
            End If
            ' add excursion period to cluster
            With lCluster
                .Finish = lEx
                .Events = .Events + 1
                .Excursions = .Excursions + lExcursion.SumLength
                If .Excursions > aMaxExc Then
                    .Excursions = aMaxExc
                End If
            End With
        Next

        aClusters.Add(lCluster)

        Dim lExcursionCount As Integer = 0
        For Each lCluster In aClusters
            lExcursionCount = lExcursionCount + lCluster.Excursions
        Next
        ' MessageBox.Show("CountExcursions done - " & aDesignFlow, lExcursionCount.ToString)

        Return lExcursionCount

    End Function

    Public Shared Function DFLOWToTable(ByVal aDataGroup As atcTimeseriesGroup,
                                 ByVal aBioParam As atcCollection,
                                 ByVal aNBioParam As atcCollection,
                                 Optional ByVal aInputs As atcDataAttributes = Nothing,
                                 Optional ByVal aShowProgress As Boolean = False) As String
        Dim lResultTable As String = ""
        Dim ladsResults As atcControls.atcGridSource = DFLOWToGrid(aDataGroup, aBioParam, aNBioParam, aInputs, aShowProgress)
        If ladsResults IsNot Nothing Then
            lResultTable = ladsResults.ToString()
        End If
        Return lResultTable
    End Function

    '''{
    Public Shared Function DFLOWToGrid(ByVal aDataGroup As atcTimeseriesGroup,
                                 ByVal aBioParam As atcCollection,
                                 ByVal aNBioParam As atcCollection,
                                 Optional ByVal aInputs As atcDataAttributes = Nothing,
                                 Optional ByVal aShowProgress As Boolean = False) As atcControls.atcGridSource

        If aDataGroup Is Nothing OrElse aDataGroup.Count = 0 Then
            Return Nothing
        End If
        ' ----- Advisory labels

        Dim lNaN As Double = GetNaN()

        Dim lFirstYear As Integer = fFirstYear
        Dim lStartMonth As Integer = fStartMonth
        Dim lStartDay As Integer = fStartDay
        Dim lLastYear As Integer = fLastYear
        Dim lEndMonth As Integer = fEndMonth
        Dim lEndDay As Integer = fEndDay

        Dim lBioPeriod As Integer = fBioPeriod
        Dim lBioYears As Integer = fBioYears
        Dim lBioCluster As Integer = fBioCluster
        Dim lBioExcursions As Integer = fBioExcursions

        Dim lAveragingPeriod As Integer = fAveragingPeriod
        Dim lReturnPeriod As Integer = fReturnPeriod
        Dim lExplicitFlow As Double = fExplicitFlow
        Dim lPercentile As Double = fPercentile

        If aInputs IsNot Nothing Then
            With aInputs
                lFirstYear = .GetValue(InputNamesDFLOW.StartYear, 0)
                lStartMonth = .GetValue(InputNamesDFLOW.StartMonth, 4)
                lStartDay = .GetValue(InputNamesDFLOW.StartDay, 1)
                lLastYear = .GetValue(InputNamesDFLOW.EndYear, 0)
                lEndMonth = .GetValue(InputNamesDFLOW.EndMonth, 3)
                lEndDay = .GetValue(InputNamesDFLOW.EndDay, 31)
            End With
        End If
        Dim lBioType As Integer = fBioType
        If aBioParam IsNot Nothing Then
            With aBioParam
                If .Keys.Contains(InputNamesDFLOW.BioAvgPeriod) Then
                    lBioPeriod = .ItemByKey(InputNamesDFLOW.BioAvgPeriod)
                End If
                If .Keys.Contains(InputNamesDFLOW.BioReturnYears) Then
                    lBioYears = .ItemByKey(InputNamesDFLOW.BioReturnYears)
                End If
                If .Keys.Contains(InputNamesDFLOW.BioClusterDays) Then
                    lBioCluster = .ItemByKey(InputNamesDFLOW.BioClusterDays)
                End If
                If .Keys.Contains(InputNamesDFLOW.BioNumExcrsnPerCluster) Then
                    lBioExcursions = .ItemByKey(InputNamesDFLOW.BioNumExcrsnPerCluster)
                End If
                For Each lKey As String In .Keys
                    If lKey.ToLower().StartsWith("type_") Then
                        Dim lBioTypeName As String = lKey.Substring("type_".Length)
                        If lBioTypeName.Contains(InputNamesDFLOW.EBioDFlowType.Acute_maximum_concentration.ToString()) Then
                            lBioType = InputNamesDFLOW.EBioDFlowType.Acute_maximum_concentration
                            Exit For
                        ElseIf lBioTypeName.Contains(InputNamesDFLOW.EBioDFlowType.Chronic_continuous_concentration.ToString()) Then
                            lBioType = InputNamesDFLOW.EBioDFlowType.Chronic_continuous_concentration
                            Exit For
                        ElseIf lBioTypeName.Contains(InputNamesDFLOW.EBioDFlowType.Ammonia.ToString()) Then
                            lBioType = InputNamesDFLOW.EBioDFlowType.Ammonia
                            Exit For
                        ElseIf lBioTypeName.Contains(InputNamesDFLOW.EBioDFlowType.Custom.ToString()) Then
                            lBioType = InputNamesDFLOW.EBioDFlowType.Custom
                            Exit For
                        End If
                    End If
                Next
            End With
        End If

        Dim lNonBioType As Integer = fNonBioType
        If aNBioParam IsNot Nothing Then
            With aNBioParam
                For Each lKey As String In .Keys
                    If lKey.ToLower().StartsWith("type_") Then
                        Dim lNonBioTypeName As String = lKey.Substring("type_".Length)
                        If lNonBioTypeName.Contains(InputNamesDFLOW.EDFlowType.Hydrological.ToString()) Then
                            If .Keys.Contains(InputNamesDFLOW.NBioAveragingPeriod) Then
                                lAveragingPeriod = .ItemByKey(InputNamesDFLOW.NBioAveragingPeriod)
                            End If
                            If .Keys.Contains(InputNamesDFLOW.NBioReturnPeriod) Then
                                lReturnPeriod = .ItemByKey(InputNamesDFLOW.NBioReturnPeriod)
                            End If
                            lNonBioType = InputNamesDFLOW.EDFlowType.Hydrological
                            Exit For
                        ElseIf lNonBioTypeName.Contains(InputNamesDFLOW.EDFlowType.Explicit_Flow_Value.ToString()) Then
                            If .Keys.Contains(InputNamesDFLOW.NBioExplicitFlow) Then
                                lExplicitFlow = .ItemByKey(InputNamesDFLOW.NBioExplicitFlow)
                            End If
                            lNonBioType = InputNamesDFLOW.EDFlowType.Explicit_Flow_Value
                            Exit For
                        ElseIf lNonBioTypeName.Contains(InputNamesDFLOW.EDFlowType.Flow_Percentile.ToString()) Then
                            If .Keys.Contains(InputNamesDFLOW.NBioFlowPercentile) Then
                                lPercentile = .ItemByKey(InputNamesDFLOW.NBioFlowPercentile)
                            End If
                            lNonBioType = InputNamesDFLOW.EDFlowType.Flow_Percentile
                            Exit For
                        ElseIf lNonBioTypeName.ToLower().Contains("custom") Then
                            If .Keys.Contains(InputNamesDFLOW.NBioAveragingPeriod) Then
                                lAveragingPeriod = .ItemByKey(InputNamesDFLOW.NBioAveragingPeriod)
                            End If
                            If .Keys.Contains(InputNamesDFLOW.NBioReturnPeriod) Then
                                lReturnPeriod = .ItemByKey(InputNamesDFLOW.NBioReturnPeriod)
                            End If
                            lNonBioType = InputNamesDFLOW.EDFlowType.Hydrological
                            Exit For
                        End If
                    End If
                Next
            End With
        End If

        Dim lFirstyearDFLOW = 1700
        If lFirstYear > 0 Then lFirstyearDFLOW = lFirstYear

        Dim lLastYearDFLOW = 2100
        If lLastYear > 0 Then lLastYearDFLOW = lLastYear

        Dim lEndDayDFLOW = lEndDay + 1
        Dim lEndMonthDFLOW = lEndMonth
        If lEndDay > fLastDay(lEndMonthDFLOW - 1) Then
            lEndDayDFLOW = 1
            If lEndMonthDFLOW = 12 Then
                lEndMonthDFLOW = 1
                lLastYearDFLOW = lLastYearDFLOW + 1
            End If
        End If

        Dim lTextSeasons As String = ""
        If (lStartMonth = lEndMonth And lStartDay = lEndDay + 1) Or
           ((lStartMonth - lEndMonth) Mod 12 = 1 And lStartDay = 1 And lEndDay = fLastDay(lEndMonth - 1)) Then
            lTextSeasons = "Climatic year defined as " & fMonth3.Substring(3 * lStartMonth - 3, 3) & " " & lStartDay & " - " & fMonth3.Substring(3 * lEndMonth - 3, 3) & " " & lEndDay & "."
        Else
            lTextSeasons = "Season defined as " & fMonth3.Substring(3 * lStartMonth - 3, 3) & " " & lStartDay & " - " & fMonth3.Substring(3 * lEndMonth - 3, 3) & " " & lEndDay &
                          ". Biological flow is calculated for full climatic year starting at " & fMonth3.Substring(3 * lStartMonth - 3, 3) & " " & lStartDay & "."
        End If
        LabelSeasons = lTextSeasons

        Dim lTextYear As String = ""
        If lFirstYear <= 0 And lLastYear <= 0 Then
            lTextYear = "All available years of data are included in analysis."
        ElseIf lFirstYear <= 0 Then
            lTextYear = "All available data through " & fMonth3.Substring(3 * lEndMonth - 3, 3) & " " & lEndDay & ", " & lLastYear & " are included in analysis."
        ElseIf lLastYear <= 0 Then
            If (lStartMonth < lEndMonth) Or (lStartMonth = lEndMonth And lStartDay < lEndDay) Then
                lTextYear = "All available data from " & fMonth3.Substring(3 * lStartMonth - 3, 3) & " " & lStartDay & ", " & lFirstYear - 1 & " are included in analysis."
            Else
                lTextYear = "All available data from " & fMonth3.Substring(3 * lStartMonth - 3, 3) & " " & lStartDay & ", " & lFirstYear & " are included in analysis."
            End If
        Else
            If (lStartMonth < lEndMonth) Or (lStartMonth = lEndMonth And lStartDay < lEndDay) Then
                lTextYear = "All available data from " & fMonth3.Substring(3 * lStartMonth - 3, 3) & " " & lStartDay & ", " & lFirstYear - 1 &
                                " through " & fMonth3.Substring(3 * lEndMonth - 3, 3) & " " & lEndDay & ", " & lLastYear & " are included in analysis."
            Else
                lTextYear = "All available data from " & fMonth3.Substring(3 * lStartMonth - 3, 3) & " " & lStartDay & ", " & lFirstYear &
                                " through " & fMonth3.Substring(3 * lEndMonth - 3, 3) & " " & lEndDay & ", " & lLastYear & " are included in analysis."
            End If
        End If
        LabelYears = lTextYear

        ' ----- Count number of items checked in the listbox/ Selected number of datasets
        Dim lTotalItems As Integer = aDataGroup.Count
        Dim lAddSeason As Boolean = False
        For Each lTsData As atcTimeseries In aDataGroup
            If lTsData.Attributes.ContainsAttribute("seasonname") Then
                lAddSeason = True
                Exit For
            End If
        Next

        ' ----- Grid to store results
        Dim ladsResults As New atcControls.atcGridSource
        With ladsResults
            If lAddSeason Then
                .Columns = 16
            Else
                .Columns = 15
            End If
            .FixedColumns = 4
            .Rows = lTotalItems + 1
            .FixedRows = 1

            .CellValue(0, 0) = "Gage"
            .CellValue(0, 1) = "Period"
            .CellValue(0, 2) = "Days in Record"
            .CellValue(0, 3) = "Zero/Missing"
            .CellValue(0, 4) = lBioPeriod & "B" & lBioYears
            .CellValue(0, 5) = "Percentile"
            .CellValue(0, 6) = "Excur per 3 yr"

            Select Case lNonBioType
                Case 0 : .CellValue(0, 7) = lAveragingPeriod & "Q" & lReturnPeriod
                Case 1 : .CellValue(0, 7) = "Explicit Q"
                Case 2 : .CellValue(0, 7) = "Percentile Q"
            End Select

            .CellValue(0, 8) = "Percentile"
            .CellValue(0, 9) = "Excur per 3 yr"
            .CellValue(0, 10) = lAveragingPeriod & "Qy Type"
            .CellValue(0, 11) = "xQy"
            .CellValue(0, 12) = "Percentile"
            .CellValue(0, 13) = "Harmonic"
            .CellValue(0, 14) = "Harmonic Adj" 'newly added
            .CellValue(0, 15) = "Percentile" 'originally 14
            If lAddSeason Then .CellValue(0, 16) = "SeasonName"

            Dim lColumn As Integer
            For lColumn = 0 To 15
                .CellColor(0, lColumn) = Drawing.Color.White 'Me.BackColor
                .Alignment(0, lColumn) = atcControls.atcAlignment.HAlignCenter
            Next
            Dim lRow As Integer
            For lRow = 1 To lTotalItems
                For lColumn = 0 To 3
                    .CellColor(lRow, lColumn) = Drawing.Color.White 'Me.BackColor
                Next
            Next
            .Rows = lTotalItems + 1
        End With

        ' ----- Loop over the items in the listbox
        Dim lItemIdx As Integer = 0
        Dim lfrmProgress As frmDFLOWProgress = Nothing
        If aShowProgress Then
            lfrmProgress = New frmDFLOWProgress()
            lfrmProgress.ProgressBar1.Value = 0
            lfrmProgress.Label1.Text = ""
            lfrmProgress.Show()
        End If

        'Dim lExcursionCountArray As New ArrayList
        'Dim lExcursionsArray As New ArrayList
        'Dim lClustersArray As New ArrayList
        'lExcursionCountArray.Clear()
        'lExcursionsArray.Clear()
        'lClustersArray.Clear()
        Dim lExcursionCountArray As atcCollection = aInputs.GetValue("ExcursionCountArray")
        Dim lExcursionsArray As atcCollection = aInputs.GetValue("ExcursionsArray")
        Dim lClustersArray As atcCollection = aInputs.GetValue("ClustersArray")

        Dim lDateFormat As New atcDateFormat
        With lDateFormat
            .IncludeHours = False
            .IncludeMinutes = False
            .IncludeSeconds = False
        End With

        '{
        For lDSIndex As Integer = 0 To aDataGroup.Count - 1
            ' ===== Quick trim 
            Dim lHydrologicTS As atcTimeseries = aDataGroup(lDSIndex)
            Dim lHydrologicTS2 As atcTimeseries = SubsetByDateBoundary(lHydrologicTS, lStartMonth, lStartDay, Nothing, lFirstyearDFLOW, lLastYearDFLOW, lEndMonthDFLOW, lEndDayDFLOW)
            Dim lFirstDate As Double = lHydrologicTS2.Attributes.GetValue("start date")
            Dim lHydrologicDS As atcDataSet = lHydrologicTS2
            Dim lSYear As Integer = (lDateFormat.JDateToString(lHydrologicDS.Attributes.GetValue("start date"))).Substring(0, 4)
            Dim lEYear As Integer = (lDateFormat.JDateToString(lHydrologicDS.Attributes.GetValue("end date"))).Substring(0, 4)
            Dim lYears As Integer = lEYear - lSYear
            lHydrologicTS.Attributes.SetValue("xBy start date", lFirstDate)

            ' ===== Calculate hydrologic design flow lxQy
            Dim lxQy As Double
            If aShowProgress Then lfrmProgress.ProgressBar1.Value = Int(100 * (3 * lItemIdx + 1) / (3 * lTotalItems))
            'If aShowProgress Then
            'Logger.Progress(Int(100 * (3 * lItemIdx + 1) / (3 * lTotalItems)), lTotalItems)
            'End If
            Select Case lNonBioType
                Case InputNamesDFLOW.EDFlowType.Hydrological '0
                    If aShowProgress Then lfrmProgress.Label1.Text = (1 + lItemIdx) & " of " & lTotalItems & " - " & fAveragingPeriod & "Q" & fReturnPeriod
                    Application.DoEvents()
                    If lYears >= lReturnPeriod Then
                        lxQy = xQy(lAveragingPeriod, lReturnPeriod, lHydrologicDS, aInputs)
                    Else
                        lxQy = lNaN
                    End If
                Case InputNamesDFLOW.EDFlowType.Explicit_Flow_Value '1
                    If aShowProgress Then lfrmProgress.Label1.Text = (1 + lItemIdx) & " of " & lTotalItems & " - explicit flow"
                    Application.DoEvents()
                    lxQy = lExplicitFlow
                Case InputNamesDFLOW.EDFlowType.Flow_Percentile '2
                    If aShowProgress Then lfrmProgress.Label1.Text = (1 + lItemIdx) & " of " & lTotalItems & " - " & lPercentile & "th percentile"
                    Application.DoEvents()
                    lxQy = modTimeseriesMath.ComputePercentile(lHydrologicTS2, lPercentile)
            End Select

            ' ===== Create 4-day running average for start of xBy excursion analysis - 
            Dim lTimeSeries As atcTimeseries = aDataGroup(lDSIndex)
            Dim lTimeSeries2 As atcTimeseries = SubsetByDateBoundary(lTimeSeries, lStartMonth, lStartDay, Nothing, lFirstyearDFLOW, lLastYearDFLOW, lEndMonthDFLOW, lEndDayDFLOW)

            Dim lTS As Double() = lTimeSeries2.Values
            lTS(0) = lNaN

            Dim lTSN As Double()
            ReDim lTSN(UBound(lTS))

            Dim lSum As Double = 0
            Dim lN As Integer = 0

            Dim lI As Integer
            For lI = 0 To UBound(lTS) - 1
                If Double.IsNaN(lTS(lI)) Then
                    lSum = 0
                    lN = 0
                Else
                    lSum = lSum + lTS(lI)
                    lN = lN + 1
                    If lN > 4 Then
                        lSum = lSum - lTS(lI - 4)
                    End If
                End If

                If lN >= 4 Then
                    lTSN(lI) = lSum / 4
                Else
                    lTSN(lI) = lNaN
                End If
            Next

            Dim lExcursions As New ArrayList
            Dim lClusters As New ArrayList
            'Dim lExcQ As Integer = CountExcursions(lxQy, 1, 120, 5, lTSN, lExcursions, lClusters)
            Dim lExcQ As Integer = CountExcursions(lxQy, lBioPeriod, lBioCluster, lBioExcursions, lTSN, lExcursions, lClusters)
            lExcursions.Clear()
            lClusters.Clear()

            ' ===== Calculate xBy (only defined for full-year)
            If aShowProgress Then
                lfrmProgress.ProgressBar1.Value = Int(100 * (3 * lItemIdx + 2) / (3 * lTotalItems))
                lfrmProgress.Label1.Text = (1 + lItemIdx) & " of " & lTotalItems & " - " & lBioPeriod & "B" & lBioYears
            End If
            Application.DoEvents()

            ' ----- 1. Create n-day running average from current time series
            lSum = 0
            lN = 0
            For lI = 0 To UBound(lTS) - 1
                If Double.IsNaN(lTS(lI)) Then
                    lSum = 0
                    lN = 0
                Else
                    lSum = lSum + lTS(lI)
                    lN = lN + 1
                    If lN > lBioPeriod Then
                        lSum = lSum - lTS(lI - lBioPeriod)
                    End If
                End If
                If lN >= lBioPeriod Then
                    lTSN(lI) = lSum / lBioPeriod
                Else
                    lTSN(lI) = lNaN
                End If
            Next

            ' ----- 2. Get initial guess
            Dim lxBy As Double = xQy(lBioPeriod, lBioYears, aDataGroup(lDSIndex), aInputs)

            ' ----- 3. Do xBy calculation
            Dim lExcursionCount As Integer
            lxBy = xBy(lxBy, lBioPeriod, lBioYears, lBioCluster, lBioExcursions, lTSN, lExcursionCount, lExcursions, lClusters, lfrmProgress)
            Dim lAttrName As String = lBioPeriod & "B" & lBioYears
            lHydrologicTS.Attributes.SetValue(lAttrName, lxBy)
            Dim loc As String = aDataGroup(lDSIndex).Attributes.GetValue("Location", "")
            Dim lSn As String = aDataGroup(lDSIndex).Attributes.GetValue("seasonname", "")
            lExcursionCountArray.Add(loc & "-" & lSn, lExcursionCount)
            lExcursionsArray.Add(loc & "-" & lSn, lExcursions)
            lClustersArray.Add(loc & "-" & lSn, lClusters)

            ' ===== If appropriate, calculate equivalent xQy for this xBy
            Dim lEquivalentxQy As Double = 0
            Dim lReturnPeriodTry As Integer '= lReturnPeriod
            If lNonBioType > 0 Then
                lEquivalentxQy = lNaN
            Else
                If aShowProgress Then
                    lfrmProgress.ProgressBar1.Value = Int(100 * (3 * lItemIdx + 3) / (3 * lTotalItems))
                    lfrmProgress.Label1.Text = (1 + lItemIdx) & " of " & lTotalItems & " - xQy"
                End If
                Application.DoEvents()

                lEquivalentxQy = xQy(lAveragingPeriod, lYears, aDataGroup(lDSIndex), aInputs)
                If lEquivalentxQy > lxBy Then
                    lReturnPeriodTry = lYears
                Else
                    If lxQy > lxBy Then
                        lReturnPeriodTry = lReturnPeriod
                    Else
                        lReturnPeriodTry = 1
                    End If
                    lEquivalentxQy = lxBy
                    While lEquivalentxQy >= lxBy And lReturnPeriodTry < lYears
                        lReturnPeriodTry += 1
                        lEquivalentxQy = xQy(lAveragingPeriod, lReturnPeriodTry, aDataGroup(lDSIndex), aInputs)
                        If aShowProgress Then
                            lfrmProgress.Label1.Text = (1 + lItemIdx) & " of " & lTotalItems & " - xQy (" & lReturnPeriodTry & " of up to " & lYears & ")"
                        End If
                        Application.DoEvents()
                    End While
                End If
            End If

            ' ===== Harmonic mean of flows
            Dim lHFlow As Double = 0
            Dim lNH As Integer = 0
            Dim lHFlowAdj As Double = 0
            For lI = 1 To UBound(lTS) '0 To UBound(lTS) - 1
                If (Not Double.IsNaN(lTS(lI))) AndAlso (lTS(lI) > 0) Then 'And (lTS(lI) <> 0)
                    lNH = lNH + 1
                    lHFlow = lHFlow + 1 / lTS(lI)
                End If
            Next
            If lHFlow <> 0 Then
                lHFlow = lNH / lHFlow
                If UBound(lTS) > 0 Then lHFlowAdj = lHFlow * ((1.0 * lNH) / (1.0 * UBound(lTS)))
            End If
            'Dim lHM As Double = lHydrologicTS2.Attributes.GetValue("Harmonic Mean")
            'Dim lHMAdj As Double = lHydrologicTS2.Attributes.GetValue("Harmonic Mean Adj")

            ' ===== Calculate percentiles
            Dim lNMiss As Integer = 0
            Dim lNZero As Integer = 0
            Dim lNExc As Integer = 0
            Dim lNExcB As Integer = 0
            Dim lNExcBQ As Integer = 0
            Dim lNExcHF As Integer = 0
            For lI = 0 To UBound(lTS) - 1
                If Double.IsNaN(lTS(lI)) Then
                    lNMiss = lNMiss + 1
                Else
                    If lTS(lI) = 0 Then
                        lNZero = lNZero + 1
                    End If
                    If lTS(lI) < lxQy Then
                        lNExc = lNExc + 1
                    End If
                    If lTS(lI) < lxBy Then
                        lNExcB = lNExcB + 1
                    End If
                    If lTS(lI) < lEquivalentxQy Then
                        lNExcBQ = lNExcBQ + 1
                    End If
                    If lTS(lI) < lHFlow Then
                        lNExcHF = lNExcHF + 1
                    End If
                End If
            Next

            ' ===== Store results
            ladsResults.CellValue(lItemIdx + 1, 0) = lHydrologicDS.Attributes.GetFormattedValue("Location") & " - " & lHydrologicDS.Attributes.GetFormattedValue("STANAM")

            ' ----- Next three values are corrected by one day to account for current (3/2008) behavior of time series trimming

            ladsResults.CellValue(lItemIdx + 1, 1) = lDateFormat.JDateToString(1 + lHydrologicDS.Attributes.GetValue("start date")) & " - " &
                                                     lDateFormat.JDateToString(lHydrologicDS.Attributes.GetValue("end date"))
            ladsResults.CellValue(lItemIdx + 1, 2) = Format(UBound(lTS) - 1, "#,##0") & " "
            ladsResults.Alignment(lItemIdx + 1, 2) = atcControls.atcAlignment.HAlignRight
            ladsResults.CellValue(lItemIdx + 1, 3) = Format(lNZero, "#,##0") & "/" & Format(lNMiss - 1, "#,##0") & " "
            ladsResults.Alignment(lItemIdx + 1, 3) = atcControls.atcAlignment.HAlignRight

            ladsResults.CellValue(lItemIdx + 1, 4) = Sig2(lxBy)
            If Sig2(lxBy) < 100 Then ladsResults.Alignment(lItemIdx + 1, 4) = atcControls.atcAlignment.HAlignDecimal
            ladsResults.CellValue(lItemIdx + 1, 5) = Format(lNExcB / (UBound(lTS) - lNMiss), "percent")
            ladsResults.Alignment(lItemIdx + 1, 5) = atcControls.atcAlignment.HAlignDecimal
            ladsResults.CellValue(lItemIdx + 1, 6) = Sig2(lExcursionCountArray(lItemIdx) * 3 / lYears)
            ladsResults.Alignment(lItemIdx + 1, 6) = atcControls.atcAlignment.HAlignDecimal

            ladsResults.CellValue(lItemIdx + 1, 7) = Sig2(lxQy)
            If Sig2(lxQy) < 100 Then ladsResults.Alignment(lItemIdx + 1, 7) = atcControls.atcAlignment.HAlignDecimal
            ladsResults.CellValue(lItemIdx + 1, 8) = Format(lNExc / (UBound(lTS) - lNMiss), "percent")
            ladsResults.Alignment(lItemIdx + 1, 8) = atcControls.atcAlignment.HAlignDecimal
            ladsResults.CellValue(lItemIdx + 1, 9) = Sig2(lExcQ * 3 / lYears)
            ladsResults.Alignment(lItemIdx + 1, 9) = atcControls.atcAlignment.HAlignDecimal

            ladsResults.Alignment(lItemIdx + 1, 10) = atcControls.atcAlignment.HAlignCenter
            ladsResults.Alignment(lItemIdx + 1, 11) = atcControls.atcAlignment.HAlignCenter
            ladsResults.Alignment(lItemIdx + 1, 12) = atcControls.atcAlignment.HAlignCenter

            If lNonBioType = InputNamesDFLOW.EDFlowType.Hydrological Then 'DFLOWCalcs.fNonBioType = 0 
                If lEquivalentxQy > lxBy Then
                    ladsResults.CellValue(lItemIdx + 1, 10) = "> " & lReturnPeriodTry & " years"
                    ladsResults.CellValue(lItemIdx + 1, 11) = "N/A"
                    ladsResults.CellValue(lItemIdx + 1, 12) = "N/A"
                Else
                    ladsResults.CellValue(lItemIdx + 1, 10) = lAveragingPeriod & "Q" & lReturnPeriodTry
                    ladsResults.CellValue(lItemIdx + 1, 11) = Sig2(lEquivalentxQy)
                    If Sig2(lEquivalentxQy) < 100 Then ladsResults.Alignment(lItemIdx + 1, 11) = atcControls.atcAlignment.HAlignDecimal
                    ladsResults.CellValue(lItemIdx + 1, 12) = Format(lNExcBQ / (UBound(lTS) - lNMiss), "percent")
                    ladsResults.Alignment(lItemIdx + 1, 12) = atcControls.atcAlignment.HAlignDecimal
                End If
            Else
                ladsResults.CellValue(lItemIdx + 1, 10) = "N/A"
                ladsResults.CellValue(lItemIdx + 1, 11) = "N/A"
                ladsResults.CellValue(lItemIdx + 1, 12) = "N/A"
            End If

            ladsResults.CellValue(lItemIdx + 1, 13) = Sig2(lHFlow)
            If Sig2(lHFlow) < 100 Then ladsResults.Alignment(lItemIdx + 1, 13) = atcControls.atcAlignment.HAlignDecimal
            ladsResults.CellValue(lItemIdx + 1, 14) = Sig2(lHFlowAdj)
            If Sig2(lHFlowAdj) < 100 Then ladsResults.Alignment(lItemIdx + 1, 14) = atcControls.atcAlignment.HAlignDecimal

            ladsResults.CellValue(lItemIdx + 1, 15) = Format(lNExcHF / (UBound(lTS) - lNMiss), "percent")
            ladsResults.Alignment(lItemIdx + 1, 15) = atcControls.atcAlignment.HAlignDecimal

            If lAddSeason Then
                ladsResults.CellValue(lItemIdx + 1, 16) = lHydrologicTS.Attributes.GetValue("seasonname")
            End If
            lItemIdx = lItemIdx + 1
            lHydrologicTS2.Clear() : lHydrologicTS2 = Nothing
            lTimeSeries2.Clear() : lTimeSeries2 = Nothing
            GC.Collect()
        Next 'lDSIndex}

        If aShowProgress Then
            lfrmProgress.Close()
            lfrmProgress.Dispose()
        End If

        'agrResults.Initialize(ladsResults)
        'If Not pIsBatch Then
        '    Me.Refresh()
        'End If
        Return ladsResults
    End Function '}


    'if aHelpTopic is a file, set the file to display instead of opening help
    Public Shared Sub ShowDFLOWHelp(ByVal aHelpTopic As String)
        Static lHelpFilename As String = ""
        Static lHelpProcess As Process = Nothing

        If aHelpTopic.ToLower.EndsWith(".chm") Then
            If IO.File.Exists(aHelpTopic) Then
                lHelpFilename = aHelpTopic
                Logger.Dbg("Set new help file '" & lHelpFilename & "'")
            Else
                Logger.Dbg("New help file not found at '" & lHelpFilename & "'")
            End If
        Else
            If lHelpProcess IsNot Nothing Then
                If Not lHelpProcess.HasExited Then
                    Try
                        Logger.Dbg("Killing old help process")
                        lHelpProcess.Kill()
                    Catch e As Exception
                        Logger.Dbg("Error killing old help process: " & e.Message)
                    End Try
                Else
                    Logger.Dbg("Old help process already exited")
                End If
                lHelpProcess.Close()
                lHelpProcess = Nothing
            End If

            If Not IO.File.Exists(lHelpFilename) Then
                lHelpFilename = atcUtility.FindFile("Please locate DFLOW help file", "dflow4.chm")
            End If

            If IO.File.Exists(lHelpFilename) Then
                If aHelpTopic.Length < 1 Then
                    Logger.Dbg("Showing help file '" & lHelpFilename & "'")
                    lHelpProcess = Process.Start("hh.exe", lHelpFilename)
                ElseIf Not aHelpTopic.Equals("CLOSE") Then
                    Logger.Dbg("Showing help file '" & lHelpFilename & "' topic '" & aHelpTopic & "'")
                    lHelpProcess = Process.Start("hh.exe", "mk:@MSITStore:" & lHelpFilename & "::/" & aHelpTopic)
                End If
            End If
        End If
    End Sub

End Class
