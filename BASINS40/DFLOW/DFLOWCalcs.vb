Imports atcData
Imports System.Windows.Forms
Imports System.IO
Imports atcUtility
Imports MapWinUtility

Module DFLOWCalcs
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

    Const eps = 0.005

    Friend fBioDefault As Boolean
    Friend fBioType As Integer
    Friend fBioPeriod As Integer
    Friend fBioYears As Integer
    Friend fBioCluster As Integer
    Friend fBioExcursions As Integer
    Friend fBioFPArray(,) As Integer = New Integer(3, 3) {{1, 3, 120, 5}, {4, 3, 120, 5}, {30, 3, 120, 5}, {-1, -1, -1, -1}}
    Friend fNonBioType As Integer
    Friend fAveragingPeriod As Integer
    Friend fReturnPeriod As Integer
    Friend fExplicitFlow As Double
    Friend fPercentile As Double

    Friend fStartDay As Integer = 1
    Friend fStartMonth As Integer = 4
    Friend fEndDay As Integer = 31
    Friend fEndMonth As Integer = 3
    Friend fFirstYear As Integer = -1
    Friend fLastYear As Integer = -1


    Friend Function Sig2(ByVal x As Double) As String
        If x >= 100 Then
            Sig2 = Format(x, "Scientific")
        ElseIf x >= 10 Then
            Sig2 = Format(x, "00.0")
        Else
            Sig2 = Format(x, "0.00")
        End If
    End Function


    Friend Sub Initialize()

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

    End Sub

    Friend Function xQy(ByVal aDays As Integer, ByVal aYears As Double, ByVal aDataSet As atcTimeseries) As Double
        Dim lResult As Double = GetNaN()
        Dim lAttrName As String
        lAttrName = aDays & "Low" & aYears
        'If Not aDataSet.Attributes.ContainsAttribute(lAttrName) Then
        Try
            Dim lArgs As New atcDataAttributes

            lArgs.SetValue("Timeseries", aDataSet)

            Dim lLogFlag As Boolean = True
            lArgs.SetValue("LogFlg", lLogFlag)

            Dim lHigh As Boolean = False
            lArgs.SetValue("HighFlag", lHigh)

            Dim lBoundaryMonth As Integer = 4
            lArgs.SetValue("BoundaryMonth", lBoundaryMonth)

            Dim lBoundaryDay As Integer = 1
            lArgs.SetValue("BoundaryDay", lBoundaryDay)

            Dim lNdays(1) As Double
            lNdays(0) = aDays
            lArgs.SetValue("NDay", lNdays)

            Dim lReturns(1) As Double
            lReturns(0) = aYears
            lArgs.SetValue("Return Period", lReturns)

            Dim lOperationName As String
            lOperationName = "n-day low value"

            Dim lCalculator As New atcTimeseriesNdayHighLow.atcTimeseriesNdayHighLow
            If lCalculator.Open(lOperationName, lArgs) AndAlso lCalculator.DataSets.Count = 1 Then
                lResult = lCalculator.DataSets(0).Attributes.GetValue(lAttrName, lResult)
            Else
                Logger.Msg(aDataSet.ToString & vbCrLf _
                           & "LogFlg=" & lLogFlag & vbCrLf _
                           & "HighFlag=" & lHigh & vbCrLf _
                           & "BoundaryMonth=" & lBoundaryMonth & vbCrLf _
                           & "BoundaryDay=" & lBoundaryDay & vbCrLf _
                           & "Return Period=" & aYears, _
                           "Could not create " & aDays & "-day timeseries")
            End If

        Catch e As Exception
            MessageBox.Show("Could not calculate value for " & lAttrName & ". " & e.ToString)
        End Try
        'End If
        Return lResult
    End Function

    Friend Function xBy(ByVal aDesignFlow As Double, _
                         ByVal aDays As Integer, _
                         ByVal aYears As Integer, _
                         ByVal aMaxDays As Integer, _
                         ByVal aMaxExcursions As Double, _
                         ByVal aFlowRecord As Double(), _
                         ByRef aExcursionCount As Integer, _
                         ByRef aExcursions As ArrayList, _
                         ByRef aClusters As ArrayList, _
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
                    afrmProgress.Label1.Text = afrmProgress.Label1.Text.Substring(0, afrmProgress.Label1.Text.IndexOf("-") + 2) & aDays & "B" & aYears & ": " & Format(lFt, "Fixed") & " (" & lIters & ")"
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
            Return GetNaN
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
    Friend Function CountExcursions(ByVal aDesignFlow As Double, _
                               ByVal aDays As Integer, _
                               ByVal aMaxDays As Integer, _
                               ByVal aMaxExc As Integer, _
                               ByVal aFlowRecord As Array, _
                               ByRef aExcursions As ArrayList, _
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
                    .events = 0
                End With
            End If
            ' add excursion period to cluster
            With lCluster
                .Finish = lEx
                .events = .events + 1
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

    'if aHelpTopic is a file, set the file to display instead of opening help
    Public Sub ShowDFLOWHelp(ByVal aHelpTopic As String)
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

End Module
