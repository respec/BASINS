Imports atcData
Imports atcUtility
Imports MapWinUtility

Module modFreq
    Private pWarned As Boolean = False 'Flag to prevent duplicate warnings during session
    Private pNan As Double = GetNaN()

    Private Declare Sub LGPSTX Lib "usgs_swstats.dll" (ByRef N As Integer, _
                                               ByRef NZI As Integer, _
                                               ByRef NUMONS As Integer, _
                                               ByRef NQS As Integer, _
                                               ByRef XBAR As Single, _
                                               ByRef STD As Single, _
                                               ByRef SKEW As Single, _
                                               ByRef LOGARH As Integer, _
                                               ByRef ILH As Integer, _
                                               ByRef DBG As Boolean, _
                                               ByVal SE() As Single, _
                                               ByVal C() As Single, _
                                               ByVal CCPA() As Single, _
                                               ByVal P() As Single, _
                                               ByVal Q() As Single, _
                                               ByVal ADP() As Single, _
                                               ByVal QNEW() As Single, _
                                               ByVal RI() As Single, _
                                               ByVal RSOUT() As Single, _
                                               ByRef RETCOD As Integer)
    Private Declare Sub KENT Lib "usgs_swstats.dll" (ByVal X() As Single, _
                                              ByRef N As Integer, _
                                              ByRef TAU As Single, _
                                              ByRef PLEVEL As Single, _
                                              ByRef SLOPE As Single)
    Private Declare Sub EMAFITB Lib "usgs_swstats.dll" (ByRef N As Integer, _
                                               ByVal QL_IN() As Single, _
                                               ByVal QU_IN() As Single, _
                                               ByVal TL_IN() As Single, _
                                               ByVal TU_IN() As Single, _
                                               ByRef REG_SKEW As Single, _
                                               ByRef REG_MSE As Single, _
                                               ByRef NEPS As Integer, _
                                               ByVal EPS() As Single, _
                                               ByRef GBTHRSH0 As Single, _
                                               ByRef PQ As Single, _
                                               ByVal CMOMS(,) As Single, _
                                               ByRef YP As Single, _
                                               ByVal CI_LOW() As Single, _
                                               ByVal CI_HIGH() As Single, _
                                               ByVal VAR_EST() As Single)
    Private Declare Sub VAR_EMA Lib "usgs_swstats.dll" (ByRef NT As Integer, _
                                               ByVal NOBS() As Double, _
                                               ByVal TL_IN() As Double, _
                                               ByVal TU_IN() As Double, _
                                               ByVal CMOMS() As Double, _
                                               ByRef PQ As Double, _
                                               ByRef REG_MSE As Double, _
                                               ByRef YP As Double, _
                                               ByVal VAR_EST(,) As Double)
    Private Declare Sub CI_EMA_M3 Lib "usgs_swstats.dll" (ByRef YP As Double, _
                                               ByVal VAR_EST(,) As Double, _
                                               ByRef NEPS As Integer, _
                                               ByVal EPS() As Double, _
                                               ByVal CI_LOW() As Double, _
                                               ByVal CI_HIGH() As Double)


    'Kendall Tau Calculation
    Friend Sub KendallTau(ByVal aTs As atcTimeseries, _
                          ByRef aTau As Double, _
                          ByRef aLevel As Double, _
                          ByRef aSlope As Double)
        Dim lTau As Single = pNan
        Dim lLevel As Single = pNan
        Dim lSlope As Single = pNan
        Dim lN As Integer = aTs.numValues

        If lN > 0 Then
            Dim lQ(lN - 1) As Single
            For i As Integer = 1 To aTs.numValues
                If Double.IsNaN(aTs.Values(i)) Then
                    lQ(i - 1) = 0
                Else
                    lQ(i - 1) = aTs.Values(i)
                End If
            Next
            Try
                KENT(lQ, lN, lTau, lLevel, lSlope)
            Catch ex As Exception
                If pWarned Then
                    Logger.Dbg("Could not compute Kendall Tau: " & ex.Message)
                Else
                    Dim lExpectedDLL As String = IO.Path.Combine(IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly.Location), "usgs_swstats.dll")
                    If IO.File.Exists(lExpectedDLL) Then
                        Logger.Msg("Required library found: " & vbCrLf & lExpectedDLL & vbCrLf & "Error message: " & ex.Message, "Could not compute Kendall Tau")
                    Else
                        Logger.Msg("Required library not found: " & vbCrLf & lExpectedDLL & vbCrLf & "Error message: " & ex.Message, "Could not compute Kendall Tau")
                    End If
                    pWarned = True
                End If
            End Try
        End If
        aTau = lTau
        aLevel = lLevel
        aSlope = lSlope
    End Sub

    'frequency analysis for specified recurrence interval or probability
    Friend Sub PearsonType3(ByVal aTs As atcTimeseries, _
                            ByVal aRecurOrProbs() As Double, _
                            ByVal aHigh As Boolean, _
                            ByVal aLogFg As Boolean, _
                            ByVal aDataSource As atcTimeseriesSource, _
                            ByRef aAttributesStorage As atcDataAttributes, _
                            ByVal aNumZero As Integer)

        Dim lNonLogTS As atcTimeseries = aTs.Attributes.GetValue("NDayTimeseries", aTs)

        'Dim lN As Integer = aTs.Attributes.GetValue("Count")
        Dim lN As Integer = lNonLogTS.Attributes.GetValue("count positive")
        Dim lMean As Double = aTs.Attributes.GetValue("Mean")
        Dim lStd As Double = aTs.Attributes.GetValue("Standard Deviation")
        Dim lSkew As Double = aTs.Attributes.GetValue("Skew")

        If lN = 0 OrElse Double.IsNaN(lMean) Then ' <= 0 Then 'no data or problem data
            Throw New ApplicationException("Count = 0 or Mean = NaN")
        Else
            Dim lNumons As Integer = 1

            Dim lLogarh As Integer
            If aLogFg Then
                lLogarh = 1 'log trans flag 1-yes 
            Else
                lLogarh = 2 'no trans desired
            End If

            Dim lIntervalMax As Integer = aRecurOrProbs.GetUpperBound(0) 'number of recurrence intervals to calculate-1
            Dim lProbs(lIntervalMax) As Double
            Dim lSe(lIntervalMax) As Single
            'Turn recurrence into probability
            For lIntervalIndex As Integer = 0 To lIntervalMax
                If aRecurOrProbs(lIntervalIndex) <= 0 Then
                    Throw New ApplicationException("Bad RecurOrProb=" & aRecurOrProbs(lIntervalIndex))
                ElseIf aRecurOrProbs(lIntervalIndex) > 1 Then
                    lProbs(lIntervalIndex) = 1.0 / aRecurOrProbs(lIntervalIndex)
                Else
                    lProbs(lIntervalIndex) = aRecurOrProbs(lIntervalIndex)
                End If
                If aHigh Then
                    lSe(lIntervalIndex) = lProbs(lIntervalIndex)
                Else
                    lSe(lIntervalIndex) = 1 - lProbs(lIntervalIndex)
                End If
            Next

            Dim lC(lIntervalMax) As Single
            Dim lCcpa(lIntervalMax) As Single
            Dim lP(lIntervalMax) As Single
            Dim lQ(lIntervalMax) As Single
            Dim lTL(lN) As Double
            Dim lTU(lN) As Double
            Dim lmse As Double = 100000000000.0
            Dim lCILow(lIntervalMax) As Single
            Dim lCIHigh(lIntervalMax) As Single
            Dim lCILowVal(1) As Double
            Dim lCIHighVal(1) As Double
            Dim lVarEst(lIntervalMax) As Single
            Dim lVarEstArray(1, 1) As Double
            Dim lCMoms(2) As Double
            Dim lneps As Integer = 1
            Dim leps(1) As Double
            Dim lNobs(1) As Double
            Dim lyp(lIntervalMax) As Single
            Dim lAdp(lIntervalMax) As Single
            Dim lQnew(lIntervalMax) As Single
            Dim lRi(lIntervalMax) As Single
            Dim lRsout(1 + (2 * lIntervalMax)) As Single
            Dim lRetcod As Integer

            Dim lIlh As Integer  'stats option 1-hi, 2-low,3-month
            If aHigh Then lIlh = 1 Else lIlh = 2

            Try
                LGPSTX(lN, aNumZero, lNumons, (lIntervalMax + 1), lMean, lStd, lSkew, lLogarh, lIlh, True, lSe, _
                       lC, lCcpa, lP, lQ, lAdp, lQnew, lRi, lRsout, lRetcod)
                lNobs(0) = 1
                lTL(0) = -1.0E+21
                lTU(0) = 1.0E+21
                leps(0) = 0.95
                lCMoms(0) = lMean
                lCMoms(1) = lStd
                lCMoms(2) = lSkew
                For i As Integer = 0 To lIntervalMax
                    VAR_EMA(1, lNobs, lTL, lTU, lCMoms, CDbl(lP(i)), lmse, lyp(i), lVarEstArray)
                    CI_EMA_M3(CDbl(lyp(i)), lVarEstArray, 1, leps, lCILowVal, lCIHighVal)
                    lCILow(i) = lCILowVal(0)
                    lCIHigh(i) = lCIHighVal(0)
                    lVarEst(i) = lVarEstArray(0, 0)
                Next i
                Dim lMsg As String = ""

                Dim lNday As Integer = aTs.Attributes.GetValue("NDay")

                For lIndex As Integer = 0 To lIntervalMax
                    If lQ(lIndex) = 0 Or Double.IsNaN(lQ(lIndex)) Then
                        If lMsg.Length = 0 Then
                            lMsg = "ComputeFreq:ZeroOrNan:" & lQ(lIndex) & ":"
                            lQ(lIndex) = GetNaN()
                        End If

                        lMsg &= lNday & ":" & aRecurOrProbs(lIndex) & ":" & aHigh & ":" & lN
                        Logger.Dbg(lMsg)
                        lMsg = ""
                    End If

                    'this is now done in USGS fortran code
                    'If aLogFg Then 'remove log10 transform 
                    '    lQ = 10 ^ lQ
                    'End If

                    Dim lS As String
                    If aHigh Then
                        lS = lNday & "High" & DoubleToString(aRecurOrProbs(lIndex), , "#0.####")
                    Else
                        lS = lNday & "Low" & DoubleToString(aRecurOrProbs(lIndex), , "#0.####")
                    End If

                    Dim lNewAttribute As atcAttributeDefinition = atcDataAttributes.GetDefinition(lS)

                    Dim lArguments As New atcDataAttributes
                    lArguments.SetValue("Nday", lNday)
                    lArguments.SetValue("Return Period", aRecurOrProbs(lIndex))
                    lArguments.SetValue("NDayTimeseries", lNonLogTS)

                    aAttributesStorage.SetValue(lNewAttribute, lQ(lIndex), lArguments)

                    Dim lNewAttVarEst As atcAttributeDefinition = atcDataAttributes.GetDefinition(lS & " Variance of Estimate")
                    aAttributesStorage.SetValue(lNewAttVarEst, lVarEst(lIndex), lArguments)

                    Dim lNewAttCILower As atcAttributeDefinition = atcDataAttributes.GetDefinition(lS & " CI Lower")
                    aAttributesStorage.SetValue(lNewAttCILower, lCILow(lIndex), lArguments)
                    Dim lNewAttCIUpper As atcAttributeDefinition = atcDataAttributes.GetDefinition(lS & " CI Upper")
                    aAttributesStorage.SetValue(lNewAttCIUpper, lCIHigh(lIndex), lArguments)

                    If aNumZero > 0 Then
                        lNewAttribute = atcDataAttributes.GetDefinition(lS & "Adj")
                        If lNewAttribute Is Nothing Then
                            lNewAttribute = New atcAttributeDefinition
                            With lNewAttribute
                                .Name = lS & "Adj"
                                .TypeString = "Double"
                                .Description = "Adjusted Result"
                                .Calculator = aDataSource
                                .DefaultValue = GetNaN()
                            End With
                        End If
                        aAttributesStorage.SetValue(lNewAttribute, lQnew(lIndex), lArguments)

                        lNewAttribute = atcDataAttributes.GetDefinition(lS & "AdjProb")
                        If lNewAttribute Is Nothing Then
                            lNewAttribute = New atcAttributeDefinition
                            With lNewAttribute
                                .Name = lS & "AdjProb"
                                .TypeString = "Double"
                                .Description = "Adjusted Probability"
                                .Calculator = aDataSource
                                .DefaultValue = GetNaN()
                            End With
                        End If
                        aAttributesStorage.SetValue(lNewAttribute, lAdp(lIndex), lArguments)

                    End If
                Next
            Catch lEx As Exception
                If pWarned Then
                    Logger.Dbg("Could not compute Pearson Type3 Frequency: " & lEx.Message)
                Else
                    Dim lExpectedDLL As String = IO.Path.Combine(IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly.Location), "usgs_swstats.dll")
                    If IO.File.Exists(lExpectedDLL) Then
                        Logger.Msg("Required library found: " & vbCrLf & lExpectedDLL & vbCrLf & "Error message: " & lEx.Message, "Could not compute PearsonType3")
                    Else
                        Logger.Msg("Required library not found: " & vbCrLf & lExpectedDLL & vbCrLf & "Error message: " & lEx.Message, "Could not compute PearsonType3")
                    End If
                    pWarned = True
                End If
            End Try
        End If
    End Sub
End Module
