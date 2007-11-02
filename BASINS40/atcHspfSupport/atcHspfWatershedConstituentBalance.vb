Imports atcUtility
Imports atcData
Imports atcSeasons
Imports MapWinUtility

Public Module WatershedConstituentBalance
    Public Sub ReportsToFiles(ByVal aUci As atcUCI.HspfUci, _
                              ByVal aBalanceType As String, _
                              ByVal aOperationTypes As atcCollection, _
                              ByVal aScenario As String, _
                              ByVal aScenarioResults As atcDataSource, _
                              ByVal aOutletLocations As atcCollection, _
                              ByVal aRunMade As String, _
                              Optional ByVal aOutFilePrefix As String = "")

        For Each lOutletLocation As String In aOutletLocations
            Dim lString As Text.StringBuilder = Report(aUci, aBalanceType, aOperationTypes, _
                                                       aScenario, aScenarioResults, _
                                                       aRunMade, lOutletLocation)
            Dim lOutFileName As String = aOutFilePrefix & SafeFilename(aScenario & "_" & lOutletLocation & "_" & aBalanceType & "_" & "Balance.txt")
            Logger.Dbg("  WriteReportTo " & lOutFileName)
            SaveFileString(lOutFileName, lString.ToString)
        Next lOutletLocation
    End Sub

    Public Function Report(ByVal aUci As atcUCI.HspfUci, _
                           ByVal aBalanceType As String, _
                           ByVal aOperationTypes As atcCollection, _
                           ByVal aScenario As String, _
                           ByVal aScenarioResults As atcDataSource, _
                           ByVal aRunMade As String, _
                           Optional ByVal aOutletLocation As String = "") As Text.StringBuilder

        Dim lWeight As Boolean = False
        If aOutletLocation.Length > 0 Then
            lWeight = True
        End If

        Dim lConstituentsToOutput As atcCollection = ConstituentsToOutput(aBalanceType)
        Dim lConstituentTotals As New atcCollection
        Dim lLandUses As atcCollection = HspfSupport.Utility.LandUses(aUci, aOperationTypes, aOutletLocation)

        Dim lString As New Text.StringBuilder
        lString.AppendLine(aBalanceType & " Watershed Balance Report For " & aScenario)
        lString.AppendLine("   Run Made " & aRunMade)
        lString.AppendLine("   " & aUci.GlobalBlock.RunInf.Value)
        lString.AppendLine("   " & aUci.GlobalBlock.RunPeriod)
        If aBalanceType = "Water" Then
            If aUci.GlobalBlock.emfg = 1 Then
                lString.AppendLine("   (Units:Inches)")
            Else
                lString.AppendLine("   (Units:mm)")
            End If
        End If

        Dim lConstituentDataGroup As atcDataGroup
        Dim lTempDataSet As atcDataSet
        Dim lPendingOutput As String = ""
        Dim lAreaTotal As Double = 0.0

        For lOperationTypeIndex As Integer = 0 To aOperationTypes.Count - 1
            Dim lOperationKey As String = aOperationTypes.Keys(lOperationTypeIndex)
            For lLanduseIndex As Integer = 0 To lLandUses.Count - 1
                Dim lLandUse As String = lLandUses.Keys(lLanduseIndex)
                If lOperationKey.StartsWith(lLandUse.Substring(0, 1)) Then
                    Dim lNeedHeader As Boolean = True
                    Dim lAreaLanduse As Double = 0.0
                    Dim lOperations As atcCollection = lLandUses.ItemByIndex(lLanduseIndex)
                    Dim lAreaOperation(lOperations.Count - 1) As Double
                    For lIndex As Integer = 0 To lConstituentsToOutput.Count - 1
                        Dim lConstituentKey As String = lConstituentsToOutput.Keys(lIndex)
                        If lConstituentKey.StartsWith(lOperationKey) Then
                            lConstituentKey = lConstituentKey.Remove(0, 2)
                            Dim lConstituentName As String = lConstituentsToOutput(lIndex)
                            lConstituentDataGroup = aScenarioResults.DataSets.FindData("Constituent", lConstituentKey)
                            If lConstituentDataGroup.Count > 0 Then
                                If lNeedHeader Then
                                    lString.AppendLine(vbCrLf)
                                    lString.AppendLine(aBalanceType & " Balance Report For " & lLandUse & vbCrLf)
                                    'get operation description for header
                                    Dim lDesc As String = ""
                                    Dim lOperName As String = ""
                                    If lOperationKey.Substring(0, 1) = "P" Then
                                        lOperName = "PERLND"
                                    ElseIf lOperationKey.Substring(0, 1) = "I" Then
                                        lOperName = "IMPLND"
                                    ElseIf lOperationKey.Substring(0, 1) = "R" Then
                                        lOperName = "RCHRES"
                                    End If
                                    If lOperName.Length > 0 Then
                                        lDesc = lOperName & ":"
                                    End If
                                    lString.Append(lDesc.PadRight(12))
                                    For lOperationIndex As Integer = 0 To lOperations.Count - 1
                                        Dim lOperationName As String = lOperations.Keys(lOperationIndex)
                                        lString.Append(vbTab & (lOperationName & "  ").PadLeft(12))
                                    Next
                                    If lWeight Then
                                        lString.Append(vbTab & "WtdAvg  ".PadLeft(12))
                                    End If
                                    lString.AppendLine()
                                    If lWeight Then
                                        lString.AppendLine()
                                        lString.Append("Area".PadRight(12))
                                        For lOperationIndex As Integer = 0 To lOperations.Count - 1
                                            lAreaOperation(lOperationIndex) = lOperations.ItemByIndex(lOperationIndex)
                                            lString.Append(vbTab & DecimalAlign(lAreaOperation(lOperationIndex)))
                                            lAreaLanduse += lAreaOperation(lOperationIndex)
                                        Next
                                        lString.Append(vbTab & DecimalAlign(lAreaLanduse))
                                        lAreaTotal += lAreaLanduse
                                        lString.AppendLine()
                                    End If
                                    lNeedHeader = False
                                End If
                                If lPendingOutput.Length > 0 Then
                                    lString.AppendLine(lPendingOutput)
                                    lPendingOutput = ""
                                End If
                                lString.Append(lConstituentName.PadRight(12))
                                Dim lWeightAccum As Double = 0.0
                                For lOperationIndex As Integer = 0 To lOperations.Count - 1
                                    Dim lLocation As String = lOperations.Keys(lOperationIndex)
                                    Dim lLocationDataGroup As atcDataGroup = lConstituentDataGroup.FindData("Location", lLocation)
                                    Dim lValue As Double = 0.0
                                    If lLocationDataGroup.Count > 0 Then
                                        lTempDataSet = lLocationDataGroup.Item(0)
                                        lValue = lTempDataSet.Attributes.GetDefinedValue("SumAnnual").Value
                                    End If
                                    lString.Append(vbTab & DecimalAlign(lValue))
                                    If lWeight Then
                                        lWeightAccum += lValue * lAreaOperation(lOperationIndex)
                                    End If
                                Next lOperationIndex
                                If lWeight Then
                                    IncrementCollectionValue(lOperationKey & lConstituentKey, lWeightAccum, lConstituentTotals)
                                    lString.Append(vbTab & DecimalAlign(lWeightAccum / lAreaLanduse))
                                End If
                                lString.AppendLine()
                            ElseIf lConstituentKey.StartsWith("Total") AndAlso _
                                    lConstituentKey.Length > 5 AndAlso _
                                   IsNumeric(lConstituentKey.Substring(5)) Then
                                Dim lTotalCount As Integer = lConstituentKey.Substring(5)
                                Dim lStr As String = lString.ToString
                                Dim lCurFields() As String
                                Dim lCurFieldValues(1) As Double
                                Dim lRecStartPos As Integer
                                Dim lRecEndPos As Integer = lStr.LastIndexOf(vbCr)
                                For lCount As Integer = 1 To lTotalCount
                                    lRecStartPos = lStr.LastIndexOf(vbCr, lRecEndPos - 1)
                                    lCurFields = lStr.Substring(lRecStartPos, lRecEndPos - lRecStartPos).Split(vbTab)
                                    If lCount = 1 Then
                                        ReDim lCurFieldValues(lCurFields.GetUpperBound(0))
                                        lCurFieldValues.Initialize()
                                    End If
                                    For lFieldPos As Integer = 1 To lCurFieldValues.GetUpperBound(0)
                                        If lCurFields(lFieldPos).Trim.Length > 0 Then
                                            lCurFieldValues(lFieldPos) += lCurFields(lFieldPos)
                                        End If
                                    Next
                                    lRecEndPos = lRecStartPos
                                Next
                                lString.Append(lConstituentName.PadRight(12))
                                For lFieldPos As Integer = 1 To lCurFieldValues.GetUpperBound(0)
                                    Dim lValue As Double = lCurFieldValues(lFieldPos)
                                    lString.Append(vbTab & DecimalAlign(lValue))
                                Next
                                lString.AppendLine()
                            Else
                                If lPendingOutput.Length > 0 Then
                                    lPendingOutput &= vbCrLf
                                End If
                                If lConstituentKey.StartsWith("Header") Then
                                    lPendingOutput &= vbCrLf
                                End If
                                lPendingOutput &= lConstituentName
                                If Not lConstituentKey.StartsWith("Header") Then
                                    For lOperationIndex As Integer = 0 To lOperations.Count - 1
                                        lPendingOutput &= vbTab & DecimalAlign(0.0)
                                    Next
                                    If lWeight Then
                                        lPendingOutput &= vbTab & DecimalAlign(0.0)
                                    End If
                                End If
                            End If
                        End If
                    Next lIndex
                End If
            Next lLanduseIndex
        Next lOperationTypeIndex

        If lWeight Then
            lString.AppendLine()
            lString.AppendLine()
            lString.AppendLine("Overall Weighted " & aBalanceType & " Balance Averages")
            lString.AppendLine()
            If aBalanceType = "Water" Then
                lString.AppendLine(Space(12) & vbTab & "Over".PadLeft(12) & _
                                               vbTab & "Land".PadRight(12) & _
                                               vbTab & "Reach".PadLeft(12) & _
                                               vbTab & "Outlets".PadRight(12) & _
                                               vbTab & "Plant".PadLeft(12) & _
                                               vbTab & "Efflnt".PadRight(12) & _
                                               vbTab & "GW".PadLeft(12) & _
                                               vbTab & "Pumping".PadRight(12) & _
                                               vbTab & "Channel".PadLeft(12) & _
                                               vbTab & "Losses".PadRight(12))
                lString.AppendLine(Space(12) & vbTab & "Inches".PadLeft(12) & _
                                               vbTab & "Ac-Ft".PadRight(12) & _
                                               vbTab & "Inches".PadLeft(12) & _
                                               vbTab & "Ac-Ft".PadRight(12) & _
                                               vbTab & "Inches".PadLeft(12) & _
                                               vbTab & "Ac-Ft".PadRight(12) & _
                                               vbTab & "Inches".PadLeft(12) & _
                                               vbTab & "Ac-Ft".PadRight(12) & _
                                               vbTab & "Inches".PadLeft(12) & _
                                               vbTab & "Ac-Ft".PadRight(12))
            End If
            Dim lOperType As String = " "
            For lIndex As Integer = 0 To lConstituentsToOutput.Count - 1
                Dim lConstituentKey As String = lConstituentsToOutput.Keys(lIndex)
                If Not lConstituentKey.StartsWith(lOperType) Then
                    lOperType = lConstituentKey.Substring(0, 1)
                    lString.AppendLine()
                    Select Case lOperType.ToLower
                        Case "p" : lString.AppendLine("PERLND:")
                        Case "i" : lString.AppendLine("IMPLND:")
                    End Select
                End If
                Dim lIndexTotal As Integer = lConstituentTotals.IndexFromKey(lConstituentKey)
                If lIndexTotal >= 0 Then
                    Dim lValue As Double = lConstituentTotals.Item(lIndexTotal)
                    lString.AppendLine(lConstituentsToOutput(lIndex).PadRight(12) & vbTab & _
                                       DecimalAlign(lValue / lAreaTotal) & vbTab & _
                                       DecimalAlign(lValue / 12))
                ElseIf Not lConstituentKey.Substring(2).StartsWith("Header") Then
                    lString.AppendLine(lConstituentsToOutput(lIndex).PadRight(12) & vbTab & _
                                       DecimalAlign(0.0) & vbTab & _
                                       DecimalAlign(0.0))
                Else
                    lString.AppendLine()
                    lString.AppendLine(lConstituentsToOutput(lIndex).PadRight(12))
                End If
            Next
        End If
        Return lString
    End Function
End Module
