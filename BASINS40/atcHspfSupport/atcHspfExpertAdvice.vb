Module ExpertAdvice
    'this module contains expert advice from the original HSPEXP user manual appendix - the comments after each advice indicate which rule
    'each is from that appendix; if there is no commented rule number, it is one Becky added based on her experience
    Public Function WaterBalance(ByVal aCriteria As List(Of Double), ByVal aError As List(Of Double)) As String 'contains advice on water balance - first priority
        WaterBalance = "Total Water Balance Error" & vbCrLf & _
                       "=========================" & vbCrLf & vbCrLf & vbCrLf & vbCrLf
        If aError(1) < 0 Then 'simulated volume much less than observed
            WaterBalance &= "Total runoff volume error (" & Strings.Format(aError(1), "00.00") & "%) is less than -" & aCriteria(1) & "%." & vbCrLf & vbCrLf & _
                "To correct this problem:" & vbCrLf & _
                "1 - Compare average annual potential ET to the value from the U.S." & vbCrLf & _
                "    National Weather Service map of annual lake evaporation for the" & vbCrLf & _
                "    study area and decrease the multiplier on the EXTERNAL SOURCES" & vbCrLf & _
                "    block of your UCI file if appropriate." & vbCrLf & _
                "2 - Compare total precipitation with surrounding rain gages and" & vbCrLf & _
                "    increase the multiplier on the EXTERNAL SOURCES block of your" & vbCrLf & _
                "    UCI file if appropriate." & vbCrLf & _
                "3 - Check to see if there are flow diversions into the watershed" & vbCrLf & _
                "    and include in the model input if appropriate." & vbCrLf & vbCrLf & _
                "Explanation: Model parameters cannot or should not account for " & vbCrLf & _
                "             major errors in input and output time series." & vbCrLf & vbCrLf & vbCrLf & vbCrLf 'TRNOF1.2
            WaterBalance &= "Total runoff volume error (" & Strings.Format(aError(1), "00.00") & "%) is less than -" & aCriteria(1) & "%." & vbCrLf & vbCrLf & _
                "It may be possible to correct this problem using LZETP (or MON-LZETPARM if input monthly).  " & vbCrLf & _
                "Consider decreasing LZETP if any of the following are true:" & vbCrLf & _
                "         LZETP for forest is greater than 0.6" & vbCrLf & _
                "         LZETP for bush/grass/pasture is greater than 0.3" & vbCrLf & _
                "         LZETP for crops is greater than 0.4" & vbCrLf & _
                "         LZETP for sparse land is greater than 0.1" & vbCrLf & _
                "         LZETP for bare land is greater than 0" & vbCrLf & vbCrLf & _
                "Explanation: Precipitation can only leave the watershed as runoff," & vbCrLf & _
                "             underflow, evapotranspiration, or diversions. If underflow and" & vbCrLf & _
                "             diversions are negligible, the only way to increase simulated runoff" & vbCrLf & _
                "             is to decrease simulated evapotranspiration. LZETP is the parameter" & vbCrLf & _
                "             that adjusts for the vigor with which vegetation transpires and" & vbCrLf & _
                "             the current value for the landuses may be high." & vbCrLf & vbCrLf & vbCrLf & vbCrLf 'LZETP2.1, 2.2, 2.3, 2.4, 2.5, 2.6
            WaterBalance &= "Total runoff volume error (" & Strings.Format(aError(1), "00.00") & "%) is less than -" & aCriteria(1) & "%." & vbCrLf & _
                "and the simulated recharge to deeper aquifers could be too high.  " & vbCrLf & vbCrLf & _
                "IF AND ONLY IF any PERLND has a value for DEEPFR greater than 0.0, consider decreasing DEEPFR." & vbCrLf & vbCrLf & _
                "Explanation: Water budgets for watersheds must consider subsurface" & vbCrLf & _
                "             losses in addition to surface-water losses at the outlet and" & vbCrLf & _
                "             evapotranspiration losses. DEEPFR is the only parameter used to" & vbCrLf & _
                "             roughly estimate those losses and should be based on a ground-water" & vbCrLf & _
                "             study of the area." & vbCrLf & vbCrLf & vbCrLf & vbCrLf 'DEPFR2.1
            WaterBalance &= "Total runoff volume error (" & Strings.Format(aError(1), "00.00") & "%) is less than -" & aCriteria(1) & "%." & vbCrLf & vbCrLf & _
                "To correct this problem: decrease LZSN" & vbCrLf & vbCrLf & _
                "Explanation: If potential evapotranspiration and the transpiration" & vbCrLf & _
                "             factor for vegetal cover (LZETP) are sufficiently low and the" & vbCrLf & _
                "             subsurface losses are appropriate, then the only way to increase" & vbCrLf & _
                "             flow is to decrease the storage capacity (LZSN) to provide less" & vbCrLf & _
                "             opportunity for evapotranspiration." & vbCrLf & vbCrLf & vbCrLf & vbCrLf 'LZSN2.1

        Else 'simulated volume much greater than observed
            WaterBalance &= "Total runoff volume error (" & Strings.Format(aError(1), "00.00") & "%) is greater than " & aCriteria(1) & "%." & vbCrLf & vbCrLf & _
                "To correct this problem: " & vbCrLf & _
                "1 - Compare average annual potential ET to the value from the U.S. " & vbCrLf & _
                "    National Weather Service map of annual lake evaporation for the" & vbCrLf & _
                "    study area and increase the multiplier on the EXTERNAL SOURCES " & vbCrLf & _
                "    block of your UCI file if appropriate." & vbCrLf & _
                "2 - Compare total precipitation with surrounding rain gages and " & vbCrLf & _
                "    decrease the multiplier on the EXTERNAL SOURCES block of your " & vbCrLf & _
                "    UCI file if appropriate." & vbCrLf & _
                "3 - Check to see if there are flow diversions out of the watershed " & vbCrLf & _
                "    and include in the model input if appropriate." & vbCrLf & vbCrLf & _
                "Explanation: Model parameters cannot or should not account for " & vbCrLf & _
                "             major errors in input and output time series." & vbCrLf & vbCrLf & vbCrLf & vbCrLf 'TRNOF1.1
            WaterBalance &= "Total runoff volume error (" & Strings.Format(aError(1), "00.00") & "%) is greater than " & aCriteria(1) & "%. " & vbCrLf & vbCrLf & _
                "It may be possible to correct this problem using LZETP (or MON-LZETPARM if input monthly).  " & vbCrLf & _
                "Consider increasing LZETP if any of the following are true:" & vbCrLf & _
                "         LZETP for forest is below 0.6" & vbCrLf & _
                "         LZETP for bush/grass/pasture is below 0.3" & vbCrLf & _
                "         LZETP for crops is below 0.4" & vbCrLf & _
                "         LZETP for sparse land is below 0.1" & vbCrLf & _
                "         LZETP for bare land is equal to 0" & vbCrLf & vbCrLf & _
                "Explanation: Precipitation can only leave the watershed as runoff," & vbCrLf & _
                "             underflow, evapotranspiration, or diversions. If underflow and" & vbCrLf & _
                "             diversions are negligible, the only way to decrease simulated runoff" & vbCrLf & _
                "             is to increase simulated evapotranspiration. LZETP is the parameter" & vbCrLf & _
                "             that adjusts for the vigor with which vegetation transpires and" & vbCrLf & _
                "             the current value for these land uses may be low." & vbCrLf & vbCrLf & vbCrLf & vbCrLf 'LZETP1.1, 1.2, 1.3, 1.4, 1.5, 1.6
            WaterBalance &= "Total runoff volume error (" & Strings.Format(aError(1), "00.00") & "%) is greater than " & aCriteria(1) & "%." & vbCrLf & _
                "and there could be recharge to deeper aquifers." & vbCrLf & vbCrLf & _
                "To correct this problem: increase DEEPFR" & vbCrLf & vbCrLf & _
                "Explanation: Water budgets for watersheds must consider subsurface" & vbCrLf & _
                "             losses in addition to surface-water losses at the outlet and" & vbCrLf & _
                "             evapotranspiration losses. DEEPFR is the only parameter used to" & vbCrLf & _
                "             roughly estimate those losses and should be based on a ground-water" & vbCrLf & _
                "             study of the area." & vbCrLf & vbCrLf & vbCrLf & vbCrLf 'DEPFR1.2
            WaterBalance &= "Total runoff volume error (" & Strings.Format(aError(1), "00.00") & "%) is greater than " & aCriteria(1) & "%." & vbCrLf & _
                "If the difference between simulated and potential ET is greater" & vbCrLf & _
                "than the difference between simulated and observed flow, or if LZSN * 1.5 " & vbCrLf & _
                "is likely below the available water capacity of the soil for the estimated" & vbCrLf & _
                "rooting depth (check this, this has not been checked for you):" & vbCrLf & vbCrLf & _
                "To correct this problem: increase LZSN" & vbCrLf & vbCrLf & _
                "Explanation: If potential evapotranspiration and the transpiration" & vbCrLf & _
                "             factor for vegetal cover (LZETP) are sufficiently high and the" & vbCrLf & _
                "             subsurface losses are appropriate, then the only way to decrease" & vbCrLf & _
                "             flow is to increase the storage capacity (LZSN) to provide greater" & vbCrLf & _
                "             opportunity for evapotranspiration." & vbCrLf & vbCrLf & vbCrLf & vbCrLf 'LZSN1.1, 1.2
            If aError(4) > (aCriteria(9) * aCriteria(4)) And aError(3) < 0 Then 'see message for explanation
                WaterBalance &= "Total runoff volume error (" & Strings.Format(aError(1), "00.00") & "%) is greater than " & aCriteria(1) & "%, and" & vbCrLf & _
                    "the highest 10% of simulated daily mean flows exceeds observed by" & vbCrLf & _
                    "more than " & aCriteria(9) & " times " & aCriteria(4) & "%, and the lowest 50% of simulated" & vbCrLf & _
                    "daily flows is lower than observed." & vbCrLf & vbCrLf & _
                    "To correct this problem: increase INFILT" & vbCrLf & vbCrLf & _
                    "Explanation: The major effect of increased infiltration (INFILT) is" & vbCrLf & _
                    "             to shift drainage from rapid response (surface runoff and interflow)" & vbCrLf & _
                    "             to delayed response (base flow). Although INFILT does not have a strong" & vbCrLf & _
                    "             direct influence on water balance, it may be too far off for the other" & vbCrLf & _
                    "             parameters to correct for water-balance errors." 'INFILT3.1
            ElseIf aError(4) > 0 And (aError(3) < -(aCriteria(9) * aCriteria(3))) Then
                WaterBalance &= "Total runoff volume error (" & Strings.Format(aError(1), "00.00") & "%) is greater than " & aCriteria(1) & "%, and" & vbCrLf & _
                    "the highest 10% of simulated daily mean flows exceeds" & vbCrLf & _
                    "observed, and the lowest 50% of simulated daily mean flows" & vbCrLf & _
                    "is less than observed by more than " & aCriteria(9) & " times " & aCriteria(3) & "%." & vbCrLf & vbCrLf & _
                    "To correct this problem: increase INFILT" & vbCrLf & vbCrLf & _
                    "Explanation: The major effect of increased infiltration (INFILT) is" & vbCrLf & _
                    "             to shift drainage from rapid response (surface runoff and interflow)" & vbCrLf & _
                    "             to delayed response (base flow). Although INFILT does not have a strong" & vbCrLf & _
                    "             direct influence on water balance, it may be too far off for the other" & vbCrLf & _
                    "             parameters to correct for water-balance errors." 'INFILT3.2
            ElseIf (aError(4) < -(aCriteria(9) * aCriteria(4))) And aError(3) > 0 Then
                WaterBalance &= "Total runoff volume error (" & Strings.Format(aError(1), "00.00") & "%) is greater than " & aCriteria(1) & "%, and" & vbCrLf & _
                    "the highest 10% of simulated daily mean flows is below observed" & vbCrLf & _
                    "by more than " & aCriteria(9) & " times " & aCriteria(4) & "%, and the lowest 50% simulated" & vbCrLf & _
                    "flows is greater than observed." & vbCrLf & vbCrLf & _
                    "To correct this problem: decrease INFILT" & vbCrLf & vbCrLf & _
                    "Explanation: The major effect of decreased infiltration (INFILT) is" & vbCrLf & _
                    "             to shift drainage from delayed response (base flow) to rapid response" & vbCrLf & _
                    "             (surface runoff and interflow). Although INFILT does not have a strong" & vbCrLf & _
                    "             direct influence on water balance, it may be too far off for the other" & vbCrLf & _
                    "             parameters to correct for water-balance errors." 'INFILT4.1
            ElseIf aError(4) < 0 And (aError(3) > (aCriteria(9) * aCriteria(3))) Then
                WaterBalance &= "Total runoff volume error (" & Strings.Format(aError(1), "00.00") & "%) is greater than " & aCriteria(1) & "%, and" & vbCrLf & _
                    "the highest 10% of simulated daily mean flows is lower than" & vbCrLf & _
                    "observed, and the lowest 50% of simulated daily mean flows" & vbCrLf & _
                    "exceeds the observed by more than " & aCriteria(9) & " times " & aCriteria(3) & "%." & vbCrLf & vbCrLf & _
                    "To correct this problem: decrease INFILT" & vbCrLf & vbCrLf & _
                    "Explanation: The major effect of decreased infiltration (INFILT) is" & vbCrLf & _
                    "             to shift drainage from delayed response (base flow) to rapid response" & vbCrLf & _
                    "             (surface runoff and interflow). Although INFILT does not have a strong" & vbCrLf & _
                    "             direct influence on water balance, it may be too far off for the other" & vbCrLf & _
                    "             parameters to correct for water-balance errors." 'INFLT4.2
            End If
        End If
    End Function

    Public Function LowFlowRecession(ByVal aCriteria As Double, ByVal aError As Double) As String
        LowFlowRecession = "Low Flow Recession Error" & vbCrLf & _
                           "========================" & vbCrLf & vbCrLf & vbCrLf & vbCrLf
        If aError < 0 Then 'low flow recession underpredicted
            LowFlowRecession &= "The difference (" & Strings.Format(aError, "0.000") & ") between the average value of the daily recession" & vbCrLf & _
                "rate Q(t)/Q(t-1) during the base-flow period for the simulated flow" & vbCrLf & _
                "and that for the observed flow is lower than -" & aCriteria & "." & vbCrLf & vbCrLf & _
                "To correct this problem: increase AGWRC" & vbCrLf & vbCrLf & _
                "Explanation: AGWRC is the fraction of yesterday's base flow that runs" & vbCrLf & _
                "             off today. An increase will 'flatten' the base-flow recession." & vbCrLf & vbCrLf & vbCrLf & vbCrLf 'AGWRC1.1
        Else 'low flow recession overpredicted
            LowFlowRecession &= "The difference (" & Strings.Format(aError, "0.000") & ") between the average value of the daily recession" & vbCrLf & _
                "rate Q(t)/Q(t-1) during the base-flow period for the simulated flow" & vbCrLf & _
                "and that for the observed flow is greater than " & aCriteria & "." & vbCrLf & vbCrLf & _
                "To correct this problem: decrease AGWRC for PERLNDs with AGWRC >= 0.88" & vbCrLf & vbCrLf & _
                "Explanation: AGWRC is the fraction of yesterday's base flow that runs" & vbCrLf & _
                "             off today. A decrease will 'steepen' the base-flow recession. The" & vbCrLf & _
                "             value for AGWRC is not too low." & vbCrLf & vbCrLf & vbCrLf & vbCrLf 'AGWRC2.1
            LowFlowRecession &= "The difference (" & Strings.Format(aError, "0.000") & ") between the average value of the daily recession" & vbCrLf & _
                "rate Q(t)/Q(t-1) during the base-flow period for the simulated flow" & vbCrLf & _
                "and that for the observed flow is greater than " & aCriteria & "." & vbCrLf & vbCrLf & _
                "To correct this problem: increase BASETP for a given PERLND if AGWRC is already less than 0.88 for a given PERLND" & vbCrLf & vbCrLf & _
                "Explanation: If AGWRC is less than 0.88, the base-flow recession constant is so low that decreasing" & vbCrLf & _
                "             it more would make the flow like interflow. You should consider" & vbCrLf & _
                "             increasing the effect of transpiration along the channels." & vbCrLf & vbCrLf & vbCrLf & vbCrLf 'BSETP3.1
            LowFlowRecession &= "The difference (" & Strings.Format(aError, "0.000") & ") between the average value of the daily recession" & vbCrLf & _
                "rate Q(t)/Q(t-1) during the base-flow period for the simulated flow" & vbCrLf & _
                "and that for the observed flow is greater than " & aCriteria & "." & vbCrLf & vbCrLf & _
                "To correct this problem: increase DEEPFR for a given PERLND if AGWRC is already less than 0.88 for a given PERLND" & vbCrLf & vbCrLf & _
                "Explanation: The base-flow recession constant is so low that decreasing" & vbCrLf & _
                "             it more would make the flow like interflow. You should consider sending" & vbCrLf & _
                "             drainage from the soil profile to deeper aquifers instead of base flow." 'DEPFR3.1
        End If
    End Function
    Public Function HighLowFlows(ByVal aCriteria As List(Of Double), ByVal aError As List(Of Double), ByVal aItem As Integer) As String
        HighLowFlows = "Flow Distribution Error" & vbCrLf & _
                       "=======================" & vbCrLf & vbCrLf & vbCrLf & vbCrLf
        'this needs to know whether high flows or low flows (or both) are off and which directions
        'high flow error is stored in aItem 4, low flow in aItem 3
        Select Case aItem
            Case 3 'low flow triggered
                If Math.Abs(aError(4)) > aCriteria(4) Then 'high flow also out of whack
                    If Math.Sign(aError(4)) = Math.Sign(aError(3)) Then 'they're out of whack the same direction
                        If aError(3) < 0 Then 'underpredicted
                            HighLowFlows &= "Although this message was triggered by low flows with an error unacceptably lower than -" & aCriteria(3) & "%," & vbCrLf & _
                                "because your high (" & Strings.Format(aError(4), "0.00") & "%) and low (" & Strings.Format(aError(3), "0.00") & "%) flow errors are BOTH lower than the acceptable range" & vbCrLf & _
                                "consider addressing your total flow volume by decreasing LZETP, decreasing DEEPFR, decreasing INFILT, or decreasing LZSN."
                        Else 'overpredicted
                            HighLowFlows &= "Although this message was triggered by low flows with an error unacceptably greater than " & aCriteria(3) & "%," & vbCrLf & _
                                "because your high (" & Strings.Format(aError(4), "0.00") & "%) and low (" & Strings.Format(aError(3), "0.00") & "%) flow errors are BOTH higher than the acceptable range" & vbCrLf & _
                                "consider addressing your total flow volume by increasing LZETP, increasing DEEPFR, increasing INFILT, or increasing LZSN."
                        End If
                    Else 'they're out of whack in opposite directions
                        If aError(3) < 0 Then 'low flows underpredicted, high flows overpredicted
                            HighLowFlows &= "The highest 10% of simulated daily mean flows is more than " & aCriteria(4) & "%" & vbCrLf & _
                                "greater than observed, and the lowest 50% of simulated daily mean flows" & vbCrLf & _
                                "is more than " & aCriteria(3) & "% less than observed." & vbCrLf & vbCrLf & _
                                "To correct this problem: increase INFILT" & vbCrLf & vbCrLf & _
                                "Explanation: The major effect of increased infiltration (INFILT) is" & vbCrLf & _
                                "             to shift drainage from rapid response (surface runoff and interflow)" & vbCrLf & _
                                "             to delayed response (base flow)." & vbCrLf & vbCrLf & vbCrLf & vbCrLf 'INFILT1.2 (modified)
                        Else 'low flows overpredicted, high flows underpredicted
                            HighLowFlows &= "The highest 10% of simulated daily mean flows is more than " & aCriteria(4) & "% less than" & vbCrLf & _
                                "observed, and the lowest 50% of simulated daily mean flows" & vbCrLf & _
                                "is more than " & aCriteria(3) & "% greater than observed." & vbCrLf & vbCrLf & _
                                "To correct this problem: decrease INFILT" & vbCrLf & vbCrLf & _
                                "Explanation: The major effect of decreased infiltration (INFILT) is" & vbCrLf & _
                                "             to shift drainage from delayed response (base flow) to rapid response" & vbCrLf & _
                                "             (surface runoff and interflow)." & vbCrLf & vbCrLf & vbCrLf & vbCrLf 'INFLT2.2
                        End If
                    End If
                Else 'high flows are within criteria
                    If Math.Sign(aError(4)) = Math.Sign(aError(3)) Then 'high and low flow errors are in same direction
                        If aError(3) < 0 Then 'underpredicted
                            HighLowFlows &= "Although this message was triggered by low flows with an error unacceptably lower than -" & aCriteria(3) & "%," & vbCrLf & _
                                "because your high (" & Strings.Format(aError(4), "0.00") & "%) and low (" & Strings.Format(aError(3), "0.00") & "%) flow errors are both negative," & vbCrLf & _
                                "consider first addressing your total flow volume by decreasing LZETP, decreasing DEEPFR, decreasing INFILT, or decreasing LZSN."
                        Else 'overpredicted
                            HighLowFlows &= "Although this message was triggered by low flows with an error unacceptably greater than " & aCriteria(3) & "%," & vbCrLf & _
                                "because your high (" & Strings.Format(aError(4), "0.00") & "%) and low (" & Strings.Format(aError(3), "0.00") & "%) flow errors are both positive," & vbCrLf & _
                                "consider first addressing your total flow volume by increasing LZETP, increasing DEEPFR, increasing INFILT, or increasing LZSN."
                        End If
                    Else 'high and low flow errors in opposite directions
                        If aError(3) < 0 Then 'low flows underpredicted and exceed criteria, high flows overpredicted but in criteria
                            HighLowFlows &= "The highest 10% of simulated daily mean flows is greater than" & vbCrLf & _
                                "observed, and the lowest 50% of simulated daily mean flows" & vbCrLf & _
                                "is more than " & aCriteria(3) & "% less than observed." & vbCrLf & vbCrLf & _
                                "To correct this problem: increase INFILT" & vbCrLf & vbCrLf & _
                                "Explanation: The major effect of increased infiltration (INFILT) is" & vbCrLf & _
                                "             to shift drainage from rapid response (surface runoff and interflow)" & vbCrLf & _
                                "             to delayed response (base flow)." & vbCrLf & vbCrLf & vbCrLf & vbCrLf 'INFILT1.2
                            HighLowFlows &= "The highest 10% of simulated daily mean flows" & vbCrLf & _
                                "is less than " & aCriteria(4) & "% greater than observed, and" & vbCrLf & _
                                "the lowest 50% of simulated daily mean flows" & vbCrLf & _
                                "is more than " & aCriteria(3) & "% less than observed." & vbCrLf & vbCrLf & _
                                "To correct this problem: decrease LZSN" & vbCrLf & vbCrLf & _
                                "Explanation: If potential evapotranspiration and the transpiration factor" & vbCrLf & _
                                "             for vegetal cover (LZETP) are sufficiently low and the subsurface losses" & vbCrLf & _
                                "             are appropriate, then the only way to increase flow is to decrease the" & vbCrLf & _
                                "             storage capacity (LZSN) to provide less opportunity for evapotranspiration." & vbCrLf 'LZSN4.1
                        Else 'low flows overpredicted, high flows underpredicted
                            HighLowFlows &= "The highest 10% of simulated daily mean flows is less than" & vbCrLf & _
                                "observed, and the lowest 50% of simulated daily mean flows" & vbCrLf & _
                                "is more than " & aCriteria(3) & "% greater than observed." & vbCrLf & vbCrLf & _
                                "To correct this problem: decrease INFILT" & vbCrLf & vbCrLf & _
                                "Explanation: The major effect of decreased infiltration (INFILT) is" & vbCrLf & _
                                "             to shift drainage from delayed response (base flow) to rapid response" & vbCrLf & _
                                "             (surface runoff and interflow)." & vbCrLf & vbCrLf & vbCrLf & vbCrLf 'INFLT2.2
                            HighLowFlows &= "The highest 10% of simulated daily mean flows" & vbCrLf & _
                                "is less than " & aCriteria(4) & "% less than observed, and" & vbCrLf & _
                                "the lowest 50% of simulated daily mean flows" & vbCrLf & _
                                "is more than " & aCriteria(3) & "% greater than observed." & vbCrLf & vbCrLf & _
                                "To correct this problem: increase LZSN" & vbCrLf & vbCrLf & _
                                "Explanation: If potential evapotranspiration and the transpiration factor" & vbCrLf & _
                                "             for vegetal cover (LZETP) are sufficiently high and the subsurface losses are" & vbCrLf & _
                                "             appropriate, then the only way to decrease flow is to increase the storage" & vbCrLf & _
                                "             capacity (LZSN) to provide greater opportunity for evapotranspiration." 'LZSN3.1
                        End If
                    End If
                End If
            Case 4 'high flow triggered
                If Math.Abs(aError(3)) > aCriteria(3) Then 'low flow also out of whack
                    If Math.Sign(aError(4)) = Math.Sign(aError(3)) Then 'they're out of whack the same direction
                        If aError(4) < 0 Then 'underpredicted
                            HighLowFlows &= "Although this message was triggered by high flows with an error unacceptably lower than -" & aCriteria(4) & "%," & vbCrLf & _
                                "because your high (" & Strings.Format(aError(4), "0.00") & "%) and low (" & Strings.Format(aError(3), "0.00") & "%) flow errors are BOTH lower than the acceptable range," & vbCrLf & _
                                "consider first addressing your total flow volume by decreasing LZETP, decreasing DEEPFR, decreasing INFILT, or decreasing LZSN."
                        Else 'overpredicted
                            HighLowFlows &= "Although this message was triggered by high flows with an error unacceptably greater than " & aCriteria(4) & "%," & vbCrLf & _
                                "because your high (" & Strings.Format(aError(4), "0.00") & "%) and low (" & Strings.Format(aError(3), "0.00") & "%) flow errors are BOTH higher than the acceptable range," & vbCrLf & _
                                "consider first addressing your total flow volume by increasing LZETP, increasing DEEPFR, increasing INFILT, or increasing LZSN.."
                        End If
                    Else 'they're out of whack in opposite directions
                        If aError(3) < 0 Then 'low flows underpredicted, high flows overpredicted
                            HighLowFlows &= "The highest 10% of simulated daily mean flows is more than " & aCriteria(4) & "%" & vbCrLf & _
                                "greater than observed, and the lowest 50% of simulated daily" & vbCrLf & _
                                "flows is more than " & aCriteria(3) & "% less than observed." & vbCrLf & vbCrLf & _
                                "To correct this problem: increase INFILT" & vbCrLf & vbCrLf & _
                                "Explanation: The major effect of increased infiltration (INFILT) is" & vbCrLf & _
                                "             to shift drainage from rapid response (surface runoff and interflow)" & vbCrLf & _
                                "             to delayed response (base flow)." & vbCrLf & vbCrLf & vbCrLf & vbCrLf 'INFILT1.1 (modified)
                        Else 'low flows overpredicted, high flows underpredicted
                            HighLowFlows &= "The highest 10% of simulated daily mean flows is" & vbCrLf & _
                                "more than " & aCriteria(4) & "% less than observed, and the lowest 50%" & vbCrLf & _
                                "of simulated mean daily flows is more than " & aCriteria(3) & "% greater than observed." & vbCrLf & vbCrLf & _
                                "To correct this problem: decrease INFILT" & vbCrLf & vbCrLf & _
                                "Explanation: The major effect of decreased infiltration (INFILT) is" & vbCrLf & _
                                "             to shift drainage from delayed response (base flow) to rapid response" & vbCrLf & _
                                "             (surface runoff and interflow)." & vbCrLf & vbCrLf & vbCrLf & vbCrLf 'INFLT2.1 (modified)
                        End If
                    End If
                Else 'low flows are within criteria
                    If Math.Sign(aError(4)) = Math.Sign(aError(3)) Then 'their errors are in the same direction
                        If aError(4) < 0 Then 'high and low flows underpredicted
                            HighLowFlows &= "Although this message was triggered by high flows with an error unacceptably lower than -" & aCriteria(4) & "%," & vbCrLf & _
                                "because your high (" & Strings.Format(aError(4), "0.00") & "%) and low (" & Strings.Format(aError(3), "0.00") & "%) flow errors are both negative," & vbCrLf & _
                                "consider first addressing your total flow volume by decreasing LZETP, decreasing DEEPFR, decreasing INFILT, or decreasing LZSN."
                        Else 'overpredicted
                            HighLowFlows &= "Although this message was triggered by high flows with an error unacceptably greater than " & aCriteria(4) & "%," & vbCrLf & _
                                "because your high (" & Strings.Format(aError(4), "0.00") & "%) and low (" & Strings.Format(aError(3), "0.00") & "%) flow errors are both positive," & vbCrLf & _
                                "consider first addressing your total flow volume by increasing LZETP, increasing DEEPFR, increasing INFILT, or increasing LZSN.."
                        End If
                    Else 'high and low flow errors are in opposite directions
                        If aError(4) < 0 Then 'high flows underpredicted and exceed criteria, low flows overpredicted but within criteria
                            HighLowFlows &= "The highest 10% of simulated daily mean flows is" & vbCrLf & _
                                "more than " & aCriteria(4) & "% less than observed, and the lowest 50%" & vbCrLf & _
                                "of simulated mean daily flows is greater than observed." & vbCrLf & vbCrLf & _
                                "To correct this problem: decrease INFILT" & vbCrLf & vbCrLf & _
                                "Explanation: The major effect of decreased infiltration (INFILT) is" & vbCrLf & _
                                "             to shift drainage from delayed response (base flow) to rapid response" & vbCrLf & _
                                "             (surface runoff and interflow)." & vbCrLf & vbCrLf & vbCrLf & vbCrLf 'INFLT2.1
                            HighLowFlows &= "The highest 10% of simulated daily mean flows" & vbCrLf & _
                                "is more than " & aCriteria(4) & "% less than observed, and" & vbCrLf & _
                                "the lowest 50% of simulated daily mean flows" & vbCrLf & _
                                "is less than " & aCriteria(3) & "% greater than observed." & vbCrLf & vbCrLf & _
                                "To correct this problem: decrease LZSN" & vbCrLf & vbCrLf & _
                                "Explanation: If potential evapotranspiration and the transpiration factor" & vbCrLf & _
                                "             for vegetal cover (LZETP) are sufficiently low and the subsurface losses" & vbCrLf & _
                                "             are appropriate, then the only way to increase flow is to decrease the" & vbCrLf & _
                                "             storage capacity (LZSN) to provide less opportunity for evapotranspiration." 'LZSN4.2
                        Else 'high flows overpredicted and exceed criteria, low flows underpredicted but within criteria
                            HighLowFlows &= "The highest 10% of simulated daily mean flows is more than " & aCriteria(4) & "%" & vbCrLf & _
                                "greater than observed, and the lowest 50% of simulated daily" & vbCrLf & _
                                "flows is less than observed." & vbCrLf & vbCrLf & _
                                "To correct this problem: increase INFILT" & vbCrLf & vbCrLf & _
                                "Explanation: The major effect of increased infiltration (INFILT) is" & vbCrLf & _
                                "             to shift drainage from rapid response (surface runoff and interflow)" & vbCrLf & _
                                "             to delayed response (base flow)." & vbCrLf & vbCrLf & vbCrLf & vbCrLf 'INFILT1.1
                            HighLowFlows &= "The highest 10% of simulated daily mean flows" & vbCrLf & _
                                "is more than " & aCriteria(4) & "% greater than observed, and" & vbCrLf & _
                                "the lowest 50% of simulated daily mean flows" & vbCrLf & _
                                "is less than " & aCriteria(3) & "% less than observed." & vbCrLf & vbCrLf & _
                                "To correct this problem: increase LZSN" & vbCrLf & vbCrLf & _
                                "Explanation: If potential evapotranspiration and the transpiration factor" & vbCrLf & _
                                "             for vegetal cover (LZETP) are sufficiently high and the subsurface losses are" & vbCrLf & _
                                "             appropriate, then the only way to decrease flow is to increase the storage" & vbCrLf & _
                                "             capacity (LZSN) to provide greater opportunity for evapotranspiration." 'LZSN3.2
                        End If
                    End If
                End If
        End Select
    End Function
    Public Function Stormflows(ByVal aCriteria As List(Of Double), ByVal aError As List(Of Double), ByVal aItem As Integer) As String
        'average storm peaks error in aItem 16, total storm volume error in aItem 5
        Stormflows = "Storm Error" & vbCrLf & _
                     "===========" & vbCrLf & vbCrLf
        Stormflows &= "Note that the total storm volume error and the peak stormflow error are completely dependent upon the " & vbCrLf & _
                "   storms you specified in your .exs file.  If there are errors in your storm specifications (easily " & vbCrLf & _
                "   detected by viewing storm graphs), if your storm choices are not evenly distributed throughout the " & vbCrLf & _
                "   year, or if you happen to have picked a hurricane (see storm graphs), this will significantly affect" & vbCrLf & _
                "   your storm volume and storm peaks errors." & vbCrLf & vbCrLf & vbCrLf & vbCrLf
        If (aItem = 5 And Math.Abs(aError(16)) > aCriteria(16)) Or aItem = 16 Then 'storm peaks are a problem
            If aError(16) < 0 Then 'storm peaks are underpredicted
                If Math.Abs(aError(5)) > aCriteria(5) Then 'storm volumes are exceeding their criteria
                    If aError(5) < 0 Then 'storm volumes are underpredicted
                        Stormflows &= "Peak stormflow error (" & Strings.Format(aError(16), "0.00") & "%) is less than -" & aCriteria(16) & "%" & vbCrLf & _
                                "and absolute value of the storm volume error (" & Strings.Format(aError(5), "0.00") & "%)" & vbCrLf & _
                                "is greater than " & aCriteria(5) & "%, and the simulated storm volume " & vbCrLf & _
                                "is less than the observed." & vbCrLf & vbCrLf & _
                                "To correct this problem: decrease INFILT" & vbCrLf & vbCrLf & _
                                "Explanation: The major effect of decreased infiltration (INFILT) is" & vbCrLf & _
                                "             to shift drainage from delayed response (base flow) to rapid response" & vbCrLf & _
                                "             (surface runoff and interflow), thus increasing both simulated storm" & vbCrLf & _
                                "             volumes and simulated storm peaks." & vbCrLf & vbCrLf & vbCrLf & vbCrLf 'INFLT6.1
                        'note that the original advice used aCriteria(5) for both criteria
                        Stormflows &= "You may want to decrease INTFW if IRC is less than 0.4 and INTFW is greater than 1.0." & vbCrLf & vbCrLf & _
                            "Explanation: Runoff needs to be shifted from interflow to surface" & vbCrLf & _
                            "             runoff to put the water in a faster drainage category. If IRC" & vbCrLf & _
                            "             is too low, however, interflow will respond like surface runoff" & vbCrLf & _
                            "             and the decrease in INTFW will have little effect on the peak." & vbCrLf & vbCrLf & vbCrLf & vbCrLf 'from INTFW2.4, INTFW2.5
                    Else 'storm volumes are overpredicted
                        Stormflows &= "Peak stormflow error (" & Strings.Format(aError(16), "0.00") & "%) is less than -" & aCriteria(16) & "%" & vbCrLf & _
                            "and the absolute value of the storm volume error (" & Strings.Format(aError(5), "0.00") & "%)" & vbCrLf & _
                            "is greater than " & aCriteria(5) & "%, and the simulated storm " & vbCrLf & _
                            "volume is greater than observed." & vbCrLf & vbCrLf & _
                            "USER PLEASE CHECK: is interflow less than " & aCriteria(6) & " times the surface runoff?" & vbCrLf & _
                            "To correct this problem: decrease INTFW" & vbCrLf & vbCrLf & _
                            "Explanation: Runoff needs to be shifted from interflow to surface" & vbCrLf & _
                            "             runoff to put the water in a faster drainage category. If IRC" & vbCrLf & _
                            "             is too low, however, interflow will respond like surface runoff" & vbCrLf & _
                            "             and the decrease in INTFW will have little effect on the peak." & vbCrLf & vbCrLf 'INTFW2.2
                        Stormflows &= "You may want to decrease INTFW whether or not interflow is more than " & aCriteria(6) & vbCrLf & _
                            "times the surface runoff if IRC is less than 0.4 and INTFW is greater than 1.0." & vbCrLf & vbCrLf & vbCrLf & vbCrLf 'from INTFW2.4
                    End If
                    Stormflows &= "If (IRC > 0.4) OR (IRC > 0.3 and INTFW < 1.0), consider decreasing IRC." & vbCrLf & vbCrLf  'IRC1.2 
                    Stormflows &= "Explanation: Simulated peak flows for the storms are too low and most" & vbCrLf & _
                        "             of the storm runoff is interflow. Because IRC values are not too low" & vbCrLf & _
                        "             and the time required for interflow to drain following a storm is" & vbCrLf & _
                        "             regulated with the parameter IRC, a decrease in IRC should increase peak flows." & vbCrLf & vbCrLf & vbCrLf & vbCrLf
                Else 'storm volumes are within their criteria
                    Stormflows &= "Peak stormflow error (" & Strings.Format(aError(16), "0.00") & "%) is less than -" & aCriteria(16) & "%" & vbCrLf & _
                        "and the absolute value of the storm volume error (" & Strings.Format(aError(5), "0.00") & "%) is less than " & aCriteria(5) & "%" & vbCrLf & vbCrLf & _
                        "USER PLEASE CHECK: is interflow less than " & aCriteria(6) & " times the surface runoff?" & vbCrLf & _
                        "To correct this problem: decrease INTFW" & vbCrLf & vbCrLf & _
                        "Explanation: Runoff needs to be shifted from interflow to surface" & vbCrLf & _
                        "             runoff to put the water in a faster drainage category. If IRC" & vbCrLf & _
                        "             is too low, however, interflow will respond like surface runoff" & vbCrLf & _
                        "             and the decrease in INTFW will have little effect on the peak." & vbCrLf & vbCrLf  'INTFW2.1
                    Stormflows &= "You may still want to decrease INTFW if interflow is more than " & aCriteria(6) & "times the surface runoff" & vbCrLf & _
                        "             if IRC is less than 0.4 and INTFW is greater than 1.0." & vbCrLf & vbCrLf & vbCrLf & vbCrLf 'from INTFW 2.3
                    Stormflows &= "If (IRC is greater than 0.4) OR (IRC is greater than 0.3 and INTFW is less than 1.0), consider decreasing IRC." & vbCrLf & vbCrLf 'IRC1.1
                    Stormflows &= "Explanation: Simulated peak flows for the storms are too low and most" & vbCrLf & _
                        "             of the storm runoff is interflow. Because IRC values are not too low" & vbCrLf & _
                        "             and the time required for interflow to drain following a storm is" & vbCrLf & _
                        "             regulated with the parameter IRC, a decrease in IRC should increase peak flows." & vbCrLf & vbCrLf & vbCrLf & vbCrLf
                End If
            Else 'storm peaks are overpredicted
                If Math.Abs(aError(5)) > aCriteria(5) Then 'storm volumes are also exceeding error
                    If aError(5) < 0 Then 'storm volumes are underpredicted
                        Stormflows &= "Peak stormflow error (" & Strings.Format(aError(16), "0.00") & "%) is greater than " & aCriteria(16) & "%" & vbCrLf & _
                            "and absolute value of the storm volume error (" & Strings.Format(aError(5), "0.00") & "%)" & vbCrLf & _
                            "is greater than " & aCriteria(5) & "%, and the simulated storm " & vbCrLf & _
                            "volume is less than observed." & vbCrLf & vbCrLf & _
                            "USER PLEASE CHECK: is interflow less than " & aCriteria(6) & " times the surface runoff?" & vbCrLf & _
                            "To correct this problem: increase INTFW" & vbCrLf & vbCrLf & _
                            "Explanation: Runoff needs to be shifted from surface runoff to" & vbCrLf & _
                            "             interflow to put the water in a slower drainage category. If IRC" & vbCrLf & _
                            "             is too low, however, interflow will respond like surface runoff" & vbCrLf & _
                            "             and the increase in INTFW will have little effect." & vbCrLf & vbCrLf  'INTFW1.2
                        Stormflows &= "Note that you may want to increase INTFW (whether or not interflow is more than " & aCriteria(6) & vbCrLf & _
                            "times the surface runoff) if IRC is above 0.6 and INTFW is less than 7.5." & vbCrLf & vbCrLf & vbCrLf & vbCrLf  'from INTFW1.4
                    Else 'storm volumes are overpredicted
                        Stormflows &= "Peak stormflow error (" & Strings.Format(aError(16), "0.00") & "%) is greater than " & aCriteria(16) & "%" & vbCrLf & _
                                "and absolute value of the storm volume error (" & Strings.Format(aError(5), "0.00") & "%)" & vbCrLf & _
                                "is greater than " & aCriteria(5) & "%, and the simulated storm " & vbCrLf & _
                                "volume is greater than the observed." & vbCrLf & vbCrLf & _
                                "To correct this problem: increase INFILT" & vbCrLf & vbCrLf & _
                                "Explanation: The major effect of increased infiltration (INFILT) is" & vbCrLf & _
                                "             to shift drainage from rapid response (surface runoff and interflow)" & vbCrLf & _
                                "             to delayed response (base flow), thus decreasing both simulated storm" & vbCrLf & _
                                "             volumes and simulated storm peaks." & vbCrLf & vbCrLf & vbCrLf & vbCrLf  'INFLT5.1
                        'note that the original advice used aCriteria(5) for both criteria
                        Stormflows &= "Note that you may want to increase INTFW if IRC is above 0.6 and INTFW is less than 7.5." & vbCrLf & vbCrLf
                        Stormflows &= "Explanation: Runoff needs to be shifted from surface runoff to" & vbCrLf & _
                            "             interflow to put the water in a slower drainage category. If IRC" & vbCrLf & _
                            "             is too low, however, interflow will respond like surface runoff" & vbCrLf & _
                            "             and the increase in INTFW will have little effect." & vbCrLf & vbCrLf & vbCrLf & vbCrLf 'from INTFW1.4
                    End If
                    Stormflows &= "If interflow is more than " & aCriteria(6) & " times the surface runoff and" & vbCrLf & _
                        "(IRC < 0.6) OR (IRC < 0.7 and INTFW > 7.5), you may want to increase IRC instead." & vbCrLf & vbCrLf & _
                        "Explanation: Simulated peak flows for the storms are too high and most" & vbCrLf & _
                        "             of the storm runoff is interflow. Because IRC values are not too high" & vbCrLf & _
                        "             and the time required for interflow to drain following a storm is" & vbCrLf & _
                        "             regulated with the parameter IRC, an increase in IRC should decrease peak flows." & vbCrLf & vbCrLf & vbCrLf & vbCrLf 'IRC2.2
                Else 'storm volumes are in range
                    Stormflows &= "Peak stormflow error (" & Strings.Format(aError(16), "0.00") & "%) is greater than " & aCriteria(16) & "%" & vbCrLf & _
                        "and absolute value of the storm volume error (" & Strings.Format(aError(5), "0.00") & "%) is less than " & aCriteria(5) & "%." & vbCrLf & vbCrLf & _
                        "USER, PLEASE CHECK: is your interflow less than " & aCriteria(6) & " times the surface runoff?" & vbCrLf & _
                        "If yes, to correct this problem: increase INTFW" & vbCrLf & vbCrLf & _
                        "Explanation: Runoff needs to be shifted from surface runoff to" & vbCrLf & _
                        "             interflow to put the water in a slower drainage category. If IRC" & vbCrLf & _
                        "             is too low, however, interflow will respond like surface runoff" & vbCrLf & _
                        "             and the increase in INTFW will have little effect." & vbCrLf & vbCrLf 'INFTW1.1
                    Stormflows &= "You may still want to increase INTFW if interflow is more than " & aCriteria(6) & "times the surface runoff" & vbCrLf & _
                        "if IRC is greater than 0.6 and INTFW is less than 7.5." & vbCrLf & vbCrLf & vbCrLf & vbCrLf 'from INTFW 1.3
                    Stormflows &= "If interflow is more than " & aCriteria(6) & "times the surface runoff and" & vbCrLf & _
                        "IRC is less than 0.6 OR (IRC is less than 0.7 and INTFW is greater than 7.5), you may want to increase IRC instead." & vbCrLf & _
                        "Explanation: Simulated peak flows for the storms are too high and most" & vbCrLf & _
                        "             of the storm runoff is interflow. Because IRC values are not too high" & vbCrLf & _
                        "             and the time required for interflow to drain following a storm is" & vbCrLf & _
                        "             regulated with the parameter IRC, an increase in IRC should decrease peak flows." & vbCrLf & vbCrLf & vbCrLf & vbCrLf 'IRC2.1
                End If
            End If
        Else 'storm peaks are not a problem, just storm volumes
            'HSPEXP does not have advice to address this situation
            If aError(5) > 0 Then 'storm volumes are overpredicted
                Stormflows &= "Although your storm peaks are predicted within the acceptable range, your simulated storm volumes are" & vbCrLf & _
                    "   greater than " & aCriteria(5) & "% greater than observed.  The experts have not created advice to address this situation, " & vbCrLf & _
                    "   but you might try increasing INFILT to shift flow from rapid response to delayed reponse.  You can balance this" & vbCrLf & _
                    "   later with adjustments to IRC and INTFW if the storm peaks become too low." & vbCrLf & vbCrLf & _
                    "   Additionally, consider whether CEPSC may be estimated too low - if interception is too low, the total storm volume may be too high." & vbCrLf & vbCrLf & vbCrLf & vbCrLf
            Else 'storm volumes are underpredicted
                Stormflows &= "Although your storm peaks are predicted within the acceptable range, your simulated storm volumes are" & vbCrLf & _
                    "   less than - " & aCriteria(5) & "% less than observed.  The experts have not created advice to address this situation, " & vbCrLf & _
                    "   but you might try decreasing INFILT to shift flow from delayed reponse to rapid response.  You can balance this" & vbCrLf & _
                    "   later with adjustments to IRC and INTFW if the storm peaks become too high." & vbCrLf & vbCrLf & _
                    "   Additionally, consider whether CEPSC may be estimated too high - if interception is too high, the total storm volume may be too low." & vbCrLf & vbCrLf & vbCrLf & vbCrLf
            End If
        End If
    End Function
    Function SeasonalAdvice(ByVal aCriteria As List(Of Double), ByVal aError As List(Of Double), ByVal aItem As Integer) As String
        'aItem = 7 is seasonal error; aItem = 8 is seasonal storm error; aItem = 17 is summer error; aItem = 18 is winter error; 
        'aItem = 20 is summer storm volume error; aItem = 19 is winter storm volume error
        SeasonalAdvice = "Seasonal Error" & vbCrLf & _
                         "==============" & vbCrLf & vbCrLf
        SeasonalAdvice &= "Before dismissing seasonal errors as impossible to correct, keep in mind the purpose of these error criteria:" & vbCrLf & _
            "   To make sure you are not achieving okay overall errors by overestimating one season and underestimating another.  The" & vbCrLf & _
            "   seasonal volume error and the seasonal storm error help ensure you are doing an adequate job predicting flow during" & vbCrLf & _
            "   all seasons of the year." & vbCrLf & vbCrLf & _
            "If you are having trouble with seasonal errors, the first thing to do is input CEPSC " & vbCrLf & _
            "   and LZETP as monthly values, if you are not doing so already." & vbCrLf & vbCrLf 'LZCEP1.1
        If Math.Abs(aError(17)) > aCriteria(17) Then SeasonalAdvice &= "Ensure your values for BASETP and " & vbCrLf & _
            "   AGWETP are appropriate, as they have a significant effect on summer volumes" & vbCrLf & vbCrLf
        If Math.Abs(aError(7)) > aCriteria(7) Then SeasonalAdvice &= "Additionally, note that changes to UZSN in an indicated direction will only affect seasonal volumes as" & vbCrLf & _
            "   as described below if it is input as a constant value (NOT monthly).  If UZSN is input monthly, then use good" & vbCrLf & _
            "   judgment on which monthly values to change which direction based on the explanation for the advice." & vbCrLf & vbCrLf & vbCrLf & vbCrLf
        If Math.Abs(aError(7)) > aCriteria(7) Then
            If aError(7) < 0 Then 'seasonal error is negative (summer flow error - winter flow error is negative)
                SeasonalAdvice &= "The average summer flow error (" & Strings.Format(aError(17), "0.00") & "%) minus the average" & vbCrLf & _
                    "winter flow error (" & Strings.Format(aError(18), "0.00") & "%) is less than -" & aCriteria(7) & "%." & vbCrLf & vbCrLf & _
                    "To correct this problem: decrease UZSN" & vbCrLf & vbCrLf & _
                    "Explanation: Surface storage in depressions and the upper few" & vbCrLf & _
                    "             inches of soil or forest litter remain near capacity in winter" & vbCrLf & _
                    "             so that the value of this storage (UZSN) has minimal effect in" & vbCrLf & _
                    "             winter but a much larger influence in summer where losses for" & vbCrLf & _
                    "             UZS are at or near the potential evapotranspiration." & vbCrLf & vbCrLf & vbCrLf & vbCrLf 'UZSN1.1
            Else 'seasonal error is positive (summer flow error - winter flow error is positive)
                SeasonalAdvice &= "The average summer flow error (" & Strings.Format(aError(17), "0.00") & "%) minus" & vbCrLf & _
                    "the average winter flow error (" & Strings.Format(aError(18), "0.00") & "%) is more than " & aCriteria(7) & "%." & vbCrLf & vbCrLf & _
                    "To correct this problem: increase UZSN" & vbCrLf & vbCrLf & _
                    "Explanation: Surface storage in depressions and the upper few" & vbCrLf & _
                    "             inches of soil or forest litter remain near capacity in winter" & vbCrLf & _
                    "             so that the value of this storage (UZSN) has minimal effect in" & vbCrLf & _
                    "             winter but a much larger influence in summer where losses for" & vbCrLf & _
                    "             UZS are at or near the potential evapotranspiration." & vbCrLf & vbCrLf & vbCrLf & vbCrLf 'UZSN2.1
            End If
            If aError(18) < 0 And aError(17) > 0 Then 'winter underpredicted, summer overpredicted
                SeasonalAdvice &= "The simulated total winter flow" & vbCrLf & _
                    "is the same or lower than observed, and" & vbCrLf & _
                    "the simulated total summer flow is greater than observed." & vbCrLf & vbCrLf & _
                    "To correct this problem: decrease LZSN" & vbCrLf & vbCrLf & _
                    "Explanation: If potential evapotranspiration and the transpiration factor" & vbCrLf & _
                    "for vegetal cover (LZETP) are sufficiently low and the subsurface losses" & vbCrLf & _
                    "are appropriate, then the only way to increase flow is to decrease the" & vbCrLf & _
                    "storage capacity (LZSN) to provide less opportunity for evapotranspiration." & vbCrLf & vbCrLf & vbCrLf & vbCrLf 'LZSN4.3
            End If
        End If
        If aError(17) > aCriteria(17) Then 'summer flow is greater than observed by an unacceptable amount
            SeasonalAdvice &= "Your summer flow error (" & Strings.Format(aError(17), "0.00") & ") is greater than " & aCriteria(17) & "%." & vbCrLf & _
                "When you look at your graph, does observed base flow increase in the fall when the simulated base flow" & vbCrLf & _
                "does not, without substantial rainfall?" & vbCrLf & vbCrLf & _
                "If this is the case: increase BASETP" & vbCrLf & vbCrLf & _
                "Explanation: Although subsurface drainage to the channels takes" & vbCrLf & _
                "             place during the summer, the vegetation around the channels is" & vbCrLf & _
                "             transpiring much of the water; as transpiration drops off in the" & vbCrLf & _
                "             fall, the flows will increase with no additional rainfall." & vbCrLf & vbCrLf & vbCrLf & vbCrLf 'BSETP1.1
        ElseIf aError(17) < -aCriteria(17) Then 'summer flow is less than observed by an unacceptable amount
            SeasonalAdvice &= "Your summer flow error (" & Strings.Format(aError(17), "0.00") & ") is less than -" & aCriteria(17) & "%." & vbCrLf & _
                "When you look at your graph, does observed base flow decrease in the fall when the simulated base" & vbCrLf & _
                "flow does not, without substantial rainfall, and" & vbCrLf & _
                "does BASETP have a value greater than 0.0?" & vbCrLf & vbCrLf & _
                "To correct this problem: decrease BASETP" & vbCrLf & vbCrLf & _
                "Explanation: Although subsurface drainage to the channels takes" & vbCrLf & _
                "             place during the summer, the vegetation around the channels is" & vbCrLf & _
                "             transpiring much of the water; as transpiration drops off in" & vbCrLf & _
                "             the fall, the flows will increase with no additional rainfall." & vbCrLf & vbCrLf & vbCrLf & vbCrLf 'BSETP2.2
        End If

        If Math.Abs(aError(8)) > aCriteria(8) Then 'seasonal storm volumes error is out of allowable range
            SeasonalAdvice &= "Your seasonal storm error (" & Strings.Format(aError(8), "0.00") & "%) - which compares summer and total storm volumes -" & vbCrLf & _
                "   is exceeding the criteria set for it (" & aCriteria(8) & "%)."
            If Math.Abs(aError(20)) < Math.Abs(aError(5)) Then
                SeasonalAdvice &= "  If your summer storm volume error (" & Strings.Format(aError(20), "0.00") & "%) is less significant than your" & vbCrLf & _
                "   total storm volume error (" & Strings.Format(aError(5), "0.00") & "%), consider previous advice you may have seen regarding stormflows in addition to this advice." & vbCrLf & vbCrLf
            Else
                SeasonalAdvice &= vbCrLf & vbCrLf
            End If
            SeasonalAdvice &= "Note that the value for CEPSC will greatly affect storm volumes, and if it is input " & vbCrLf & _
                "   monthly you can adjust its effect on summer storm volume and total storm volume " & vbCrLf & _
                "   separately (the latter statistic is driven primarily by winter storms)." & vbCrLf & vbCrLf & _
                "Additionally note that the seasonal storm error, summer storm error, and total storm " & vbCrLf & _
                "   error are completely dependent upon the storms you specified in your .exs file.  " & vbCrLf & _
                "   If there are errors in your storm specifications (easily detected by viewing storm graphs)," & vbCrLf & _
                "   if your storm choices are not evenly distributed throughout the year, or if you happen to " & vbCrLf & _
                "   have picked a hurricane (see storm graphs), this will significantly affect your storm volume errors." & vbCrLf & vbCrLf & vbCrLf & vbCrLf
            If aError(20) > aCriteria(20) Then 'summer storms overpredicted
                SeasonalAdvice &= "The difference (" & Strings.Format(aError(20), "0.00") & "%) between simulated and observed" & vbCrLf & _
                    "high flow during the summer months is greater than " & aCriteria(20) & "%." & vbCrLf & vbCrLf & _
                    "To correct this problem: increase PERLND area and" & vbCrLf & _
                    "   decrease IMPLND area by a corresponding amount." & vbCrLf & vbCrLf & _
                    "Explanation: The most detectable effect from urban area" & vbCrLf & _
                    "             (impervious area) on streamflow is an increase in stormflows" & vbCrLf & _
                    "             during hot, dry periods usually in the summer." & vbCrLf & vbCrLf & vbCrLf & vbCrLf 'PRIMP1.1
            ElseIf aError(20) < -aCriteria(20) Then 'summer storms underpredicted
                SeasonalAdvice &= "The difference (" & Strings.Format(aError(20), "0.00") & "%) between simulated and observed" & vbCrLf & _
                    "high flow during the summer months is less than -" & aCriteria(20) & "%." & vbCrLf & vbCrLf & _
                    "To correct this problem: decrease PERLND area and" & vbCrLf & _
                    "increase IMPLND area by a corresponding amount." & vbCrLf & vbCrLf & _
                    "Explanation: The most detectable effect from urban area" & vbCrLf & _
                    "             (impervious area) on streamflow is an increase in stormflows" & vbCrLf & _
                    "             during hot, dry periods usually in the summer." & vbCrLf & vbCrLf & vbCrLf & vbCrLf 'PRIMP2.1
            ElseIf aError(20) < 0 Then 'summer storms underpredicted but in range
                SeasonalAdvice &= "The simulated high flow during summer months is lower than observed, and" & vbCrLf & _
                    "there are urban areas in the watershed; is it possible that" & vbCrLf & _
                    "the amount of IMPLND area might have been estimated low?" & vbCrLf & vbCrLf & _
                    "To correct this problem: decrease PERLND area and" & vbCrLf & _
                    "increase IMPLND area by a corresponding amount." & vbCrLf & vbCrLf & _
                    "Explanation: The most detectable effect from urban area" & vbCrLf & _
                    "             (impervious area) on streamflow is an increase in stormflows" & vbCrLf & _
                    "             during hot, dry periods usually in the summer." & vbCrLf & vbCrLf & vbCrLf & vbCrLf 'PRIMP2.2
            ElseIf aError(20) > 0 Then 'summer storms overpredicted but in range
                SeasonalAdvice &= "The simulated high flow during summer months is greater than observed, and" & vbCrLf & _
                    "there are urban areas in the watershed; is it possible that" & vbCrLf & _
                    "the amount of IMPLND area might have been estimated high?" & vbCrLf & vbCrLf & _
                    "To correct this problem: increase PERLND area and" & vbCrLf & _
                    "decrease IMPLND area by a corresponding amount." & vbCrLf & vbCrLf & _
                    "Explanation: The most detectable effect from urban area" & vbCrLf & _
                    "             (impervious area) on streamflow is an increase in stormflows" & vbCrLf & _
                    "             during hot, dry periods usually in the summer." & vbCrLf & vbCrLf & vbCrLf & vbCrLf 'PRIMP1.2
            End If
        End If

        SeasonalAdvice &= "A note on KVARY - USE WITH CAUTION:" & vbCrLf & _
            "**IF** simulated base flow during wet periods recedes more slowly than the" & vbCrLf & _
            "observed base flow, or simulated winter base flow recedes more slowly" & vbCrLf & _
            "than the observed winter base flow; and simulated summer base flow" & vbCrLf & _
            "recedes faster than the observed summer base flow, or simulated base" & vbCrLf & _
            "flow during dry periods recedes faster than the observed base flow" & vbCrLf & _
            "during dry periods. - THIS HAS NOT BEEN CHECKED, YOU NEED TO LOOK AT YOUR" & vbCrLf & _
            "GRAPHS AND EVALUATE FOR YOURSELF." & vbCrLf & vbCrLf & _
            "If you find this to be a problem, to correct it: increase KVARY, decrease BASETP if" & vbCrLf & _
            "   there is channel or flood-plain vegetation, or decrease AGWETP if" & vbCrLf & _
            "   vegetation roots reach the ground-water table." & vbCrLf & vbCrLf & _
            "Explanation: KVARY has the effect of shifting base-flow drainage from drier" & vbCrLf & _
            "             periods (no recharge) to shortly after wet periods (recent recharge). It" & vbCrLf & _
            "             accounts for greater contributing area immediately following a storm period." & vbCrLf & _
            "             Evapotranspiration from the ground-water storage (AGWETP) or from the base" & vbCrLf & _
            "             flow at the channel (BASETP) has a greater impact during the summer than" & vbCrLf & _
            "             the winter, which gives the effect of a steeper recession in the summer." 'KVARY1.1
    End Function
End Module
