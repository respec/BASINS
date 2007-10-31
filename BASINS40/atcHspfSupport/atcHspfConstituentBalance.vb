Imports atcUtility
Imports atcData
Imports atcSeasons
Imports MapWinUtility

Public Module ConstituentBalance
    Public Sub ReportsToFiles(ByVal aOperations As atcCollection, _
                              ByVal aBalanceTypes As atcCollection, _
                              ByVal aScenario As String, _
                              ByVal aScenarioResults As atcDataSource, _
                              ByVal aLocations As atcCollection, _
                              ByVal aRunMade As String)

        'make uci available 
        Dim lMsg As New atcUCI.HspfMsg
        lMsg.Open("hspfmsg.mdb")
        Dim lHspfUci As New atcUCI.HspfUci
        lHspfUci.FastReadUciForStarter(lMsg, aScenario & ".uci")

        For Each lBalanceType As String In aBalanceTypes
            Dim lString As Text.StringBuilder = Report(lHspfUci, lBalanceType, aOperations, _
                                                       aScenario, aScenarioResults, aLocations, _
                                                       aRunMade)
            Dim lOutFileName As String = aScenario & "_" & lBalanceType & "_" & "Balance.txt"
            Logger.Dbg("  WriteReportTo " & lOutFileName)
            SaveFileString(lOutFileName, lString.ToString)
        Next lBalanceType
    End Sub

    Public Function Report(ByVal aUci As atcUCI.HspfUci, _
                           ByVal aBalanceType As String, _
                           ByVal aOperations As atcCollection, _
                           ByVal aScenario As String, _
                           ByVal aScenarioResults As atcDataSource, _
                           ByVal aLocations As atcCollection, _
                           ByVal aRunMade As String) As Text.StringBuilder
        Dim lConstituents2Output As atcCollection = ConstituentsToOutput(aBalanceType)

        Dim lString As New Text.StringBuilder
        lString.AppendLine(aBalanceType & " Balance Report For " & aScenario)
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
        lString.AppendLine(vbCrLf)

        Dim lMatchConstituentGroup As atcDataGroup
        Dim lTempDataSet As atcDataSet

        For lOperationIndex As Integer = 0 To aOperations.Count - 1
            Dim lOperationKey As String = aOperations.Keys(lOperationIndex)
            For Each lLocation As String In aLocations
                If lLocation.StartsWith(lOperationKey) Then
                    'Logger.Dbg(aOperations(lOperationIndex) & " " & lLocation)
                    Dim lTempDataGroup As atcDataGroup = aScenarioResults.DataSets.FindData("Location", lLocation)
                    'Logger.Dbg("     MatchingDatasetCount " & lTempDataGroup.Count)
                    Dim lNeedHeader As Boolean = True
                    Dim lPendingOutput As String = ""
                    For lIndex As Integer = 0 To lConstituents2Output.Count - 1
                        Dim lConstituentKey As String = lConstituents2Output.Keys(lIndex)
                        If lConstituentKey.StartsWith(lOperationKey) Then
                            lConstituentKey = lConstituentKey.Remove(0, 2)
                            Dim lConstituentName As String = lConstituents2Output(lIndex)
                            lMatchConstituentGroup = lTempDataGroup.FindData("Constituent", lConstituentKey)
                            If lMatchConstituentGroup.Count > 0 Then
                                lTempDataSet = lMatchConstituentGroup.Item(0)
                                Dim lSeasons As atcSeasonBase
                                If aUci.GlobalBlock.SDate(1) = 10 Then 'month Oct
                                    lSeasons = New atcSeasonsWaterYear
                                Else
                                    lSeasons = New atcSeasonsCalendarYear
                                End If
                                Dim lSeasonalAttributes As New atcDataAttributes
                                Dim lCalculatedAttributes As New atcDataAttributes
                                lSeasonalAttributes.SetValue("Sum", 0) 'fluxes are summed from daily, monthly or annual to annual
                                lSeasons.SetSeasonalAttributes(lTempDataSet, lSeasonalAttributes, lCalculatedAttributes)

                                If lNeedHeader Then
                                    'get operation description for header
                                    Dim lDesc As String = ""
                                    Dim lOperName As String = ""
                                    If lLocation.Substring(0, 1) = "P" Then
                                        lOperName = "PERLND"
                                    ElseIf lLocation.Substring(0, 1) = "I" Then
                                        lOperName = "IMPLND"
                                    ElseIf lLocation.Substring(0, 1) = "R" Then
                                        lOperName = "RCHRES"
                                    End If
                                    If lOperName.Length > 0 Then
                                        lDesc = aUci.OpnBlks(lOperName).operfromid(lLocation.Substring(2)).description
                                    End If

                                    lString.AppendLine(aBalanceType & " Balance Report For " & lLocation & " (" & lDesc & ")" & vbCrLf)
                                    lString.Append("Date    " & vbTab & "      Mean")
                                    For Each lAttribute As atcDefinedValue In lCalculatedAttributes
                                        Dim s As String = lAttribute.Arguments(1).Value
                                        lString.Append(vbTab & s.PadLeft(10))
                                    Next
                                    lString.AppendLine()
                                    lNeedHeader = False
                                End If
                                If lPendingOutput.Length > 0 Then
                                    lString.AppendLine(lPendingOutput)
                                    lPendingOutput = ""
                                End If

                                lString.Append(lConstituentName & vbTab & DecimalAlign(lTempDataSet.Attributes.GetDefinedValue("SumAnnual").Value))
                                For Each lAttribute As atcDefinedValue In lCalculatedAttributes
                                    lString.Append(vbTab & DecimalAlign(lAttribute.Value))
                                Next
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
                                        lCurFieldValues(lFieldPos) += lCurFields(lFieldPos)
                                    Next
                                    lRecEndPos = lRecStartPos
                                Next
                                lString.Append(lConstituentName)
                                For lFieldPos As Integer = 1 To lCurFieldValues.GetUpperBound(0)
                                    lString.Append(vbTab & DecimalAlign(lCurFieldValues(lFieldPos)))
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
                            End If
                        End If
                    Next

                    If lConstituents2Output.Count = 0 Then
                        Logger.Dbg(" BalanceType " & aBalanceType & " at " & lLocation & " has no timeseries to output in script!")
                    Else
                        If lPendingOutput.Length > 0 Then
                            If lNeedHeader Then
                                lString.AppendLine("No data for " & aBalanceType & " balance report at " & lLocation & "!")
                            Else
                                Logger.Dbg("  No data for " & lPendingOutput)
                            End If
                        End If
                    End If
                    lTempDataGroup = Nothing
                    lString.AppendLine(vbCrLf)
                Else
                    'Logger.Dbg("   SKIP " & lLocation)
                End If
            Next lLocation
        Next lOperationIndex

        Return lString
    End Function
End Module
