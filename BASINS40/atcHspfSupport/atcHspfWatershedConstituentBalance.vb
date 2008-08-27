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
                     Optional ByVal aOutFilePrefix As String = "", _
                     Optional ByVal aOutletDetails As Boolean = False)
        For Each lOutletLocation As String In aOutletLocations
            Dim lString As Text.StringBuilder = Report(aUci, aBalanceType, _
                                                       aOperationTypes, _
                                                       aScenario, aScenarioResults, _
                                                       aRunMade, lOutletLocation, True)
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
                  Optional ByVal aOutletLocation As String = "", _
                  Optional ByVal aOutletDetails As Boolean = False) As Text.StringBuilder

        Dim lOutletReport As Boolean = False
        If aOutletLocation.Length > 0 Then
            lOutletReport = True
        End If

        Dim lConstituentsToOutput As atcCollection = ConstituentsToOutput(aBalanceType)
        Logger.Dbg("ConstituentCount:" & lConstituentsToOutput.Count)
        Dim lConstituentTotals As New atcCollection
        Dim lConstituentLandUseTotals As New atcCollection

        Dim lLandUses As atcCollection = LandUses(aUci, aOperationTypes, aOutletLocation)
        Logger.Dbg("LandUsecount:" & lLandUses.Count)

        Dim lString As New Text.StringBuilder
        lString.AppendLine(aBalanceType & " Watershed Balance Report For " & aScenario)
        lString.AppendLine(Header(aBalanceType, aScenario, aRunMade, aUci))
        If aBalanceType = "Water" Then
            If aUci.GlobalBlock.EmFg = 1 Then
                lString.AppendLine("   (Units:Inches)")
            Else
                lString.AppendLine("   (Units:mm)")
            End If
        End If

        Dim lConstituentDataGroup As atcDataGroup
        Dim lTempDataSet As atcDataSet
        Dim lPendingOutput As String = ""
        Dim lOperationTypeAreas As New atcCollection
        Dim lOperationAreas As New atcCollection
        Dim lLandUseAreas As New atcCollection
        Dim lLandUseConstituentTotals As New atcCollection

        Logger.Dbg("OperationTypesCount:" & aOperationTypes.Count)
        For Each lOperationType As String In aOperationTypes
            Logger.Dbg("ProcessType:" & lOperationType)
            Dim lValueOutlet As Double = 0.0
            For Each lLandUse As String In lLandUses.Keys
                If lOperationType.StartsWith(lLandUse.Substring(0, 1)) Then
                    Dim lNeedHeader As Boolean = True
                    Dim lLandUseOperations As atcCollection = lLandUses.ItemByKey(lLandUse)
                    For Each lConstituentKey As String In lConstituentsToOutput.Keys
                        If lConstituentKey.StartsWith(lOperationType.Substring(0, 1)) Then
                            Dim lConstituentName As String = lConstituentsToOutput.ItemByKey(lConstituentKey)
                            lConstituentKey = lConstituentKey.Remove(0, 2)
                            lConstituentDataGroup = aScenarioResults.DataSets.FindData("Constituent", lConstituentKey)
                            If lConstituentDataGroup.Count > 0 Then
                                If lNeedHeader Then
                                    lString.AppendLine(vbCrLf)
                                    lString.AppendLine(aBalanceType & " Balance Report For " & lLandUse & vbCrLf)
                                    'get operation description for header
                                    lString.Append((lOperationType & ":").PadRight(12))
                                    For lOperationIndex As Integer = 0 To lLandUseOperations.Count - 1
                                        Dim lOperationName As String = lLandUseOperations.Keys(lOperationIndex)
                                        lString.Append(vbTab & (lOperationName & "  ").PadLeft(12))
                                    Next
                                    If lOutletReport And lOperationType <> "RCHRES" Then
                                        lString.Append(vbTab & "WtdAvg  ".PadLeft(12))
                                    End If
                                    lString.AppendLine()
                                    If lOutletReport And lOperationType <> "RCHRES" Then
                                        lString.AppendLine()
                                        lString.Append("Area".PadRight(12))
                                        For Each lOperationKey As String In lLandUseOperations.Keys
                                            Dim lOperationArea As Double = lLandUseOperations.ItemByKey(lOperationKey)
                                            lString.Append(vbTab & DecimalAlign(lOperationArea))
                                            lOperationAreas.Increment(lOperationKey, lOperationArea)
                                            lLandUseAreas.Increment(lLandUse, lOperationArea)
                                        Next
                                        lString.Append(vbTab & DecimalAlign(lLandUseAreas.ItemByKey(lLandUse)))
                                        lOperationTypeAreas.Increment(lOperationType, lLandUseAreas.ItemByKey(lLandUse))
                                        lString.AppendLine(vbTab & "(Sum)")
                                    End If
                                    lNeedHeader = False
                                End If
                                If lPendingOutput.Length > 0 Then
                                    lString.AppendLine(lPendingOutput)
                                    lPendingOutput = ""
                                End If
                                lString.Append(lConstituentName.PadRight(12))
                                Dim lWeightAccum As Double = 0.0
                                Dim lValue As Double = 0.0
                                For Each lLocation As String In lLandUseOperations.Keys
                                    Dim lLocationDataGroup As atcDataGroup = lConstituentDataGroup.FindData("Location", lLocation)
                                    If lLocationDataGroup.Count > 0 Then
                                        lTempDataSet = lLocationDataGroup.Item(0)
                                        Dim lAttribute As atcDefinedValue = lTempDataSet.Attributes.GetDefinedValue("SumAnnual")
                                        If lAttribute Is Nothing Then
                                            lValue = GetNaN()
                                        Else
                                            lValue = lAttribute.Value
                                        End If
                                    Else
                                        lValue = 0.0
                                        'Logger.Dbg("SkipLocation:" & lLocation & ":WithNo:" & lConstituentKey & ":Data")
                                    End If
                                    lString.Append(vbTab & DecimalAlign(lValue))
                                    If lOutletReport Then
                                        If lOperationType <> "RCHRES" Then
                                            lWeightAccum += lValue * lOperationAreas.ItemByKey(lLocation)
                                        ElseIf lLocation = aOutletLocation Then
                                            lValueOutlet = lValue * 12  'feet to inches
                                        End If
                                    End If
                                Next lLocation

                                If lOutletReport Then
                                    Dim lConstituentTotalKey As String = lOperationType.Substring(0, 1) & ":" & lConstituentKey
                                    If lOperationType <> "RCHRES" Then
                                        lLandUseConstituentTotals.Increment(lConstituentTotalKey & "-" & lLandUse, lWeightAccum)
                                        lConstituentTotals.Increment(lConstituentTotalKey, lWeightAccum)
                                        lString.Append(vbTab & DecimalAlign(lWeightAccum / lLandUseAreas.ItemByKey(lLandUse)))
                                    ElseIf Math.Abs(lValueOutlet) > 0.00001 Then
                                        Dim lConstituentTotalKeyIndex As Integer = lConstituentTotals.IndexFromKey(lConstituentTotalKey)
                                        lConstituentTotals.Increment(lConstituentTotalKey, lValueOutlet)
                                        lValueOutlet = 0.0
                                    End If
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
                                    For lOperationIndex As Integer = 0 To lLandUseOperations.Count - 1
                                        lPendingOutput &= vbTab & DecimalAlign(0.0)
                                    Next
                                    If lOutletReport And lOperationType <> "RCHRES" Then
                                        lPendingOutput &= vbTab & DecimalAlign(0.0)
                                    End If
                                End If
                            End If
                        End If
                    Next lConstituentKey
                End If
            Next lLandUse
        Next lOperationType

        If lOutletReport Then 'watershed summary report at specified output
            If aOutletDetails Then
                Try
                    lString.AppendLine()
                    lString.AppendLine()
                    lString.AppendLine(aBalanceType & " Balance by Land Use Category")
                    lString.AppendLine(Header(aBalanceType, aScenario, aRunMade, aUci))
                    lString.AppendLine()
                    For Each lOperationType As String In aOperationTypes
                        If Not lOperationType.StartsWith("R") Then
                            lString.AppendLine(lOperationType)
                            lString.AppendLine()
                            lString.Append("LandUse".PadRight(12))
                            For Each lLandUse As String In lLandUses.Keys
                                If lLandUse.StartsWith(lOperationType.Substring(0, 1)) Then
                                    lString.Append(lLandUse.Substring(2))
                                End If
                            Next
                            lString.AppendLine(vbTab & "WtdAvg".PadLeft(12))
                            lString.AppendLine()
                            lString.Append("Area".PadRight(12))
                            Dim lAreaTotal As Double = 0.0
                            For Each lLandUse As String In lLandUses.Keys
                                If lLandUse.StartsWith(lOperationType.Substring(0, 1)) Then
                                    Dim lArea As Double = lLandUseAreas.ItemByKey(lLandUse)
                                    lAreaTotal += lArea
                                    lString.Append(vbTab & DecimalAlign(lArea, , , 8))
                                End If
                            Next
                            lString.AppendLine(vbTab & DecimalAlign(lAreaTotal, , , 8) & vbTab & "(Sum)")

                            For Each lConstituentKey As String In lConstituentsToOutput.Keys
                                Dim lConstituentName As String = lConstituentsToOutput.ItemByKey(lConstituentKey)
                                If lConstituentKey.StartsWith(lOperationType.Substring(0, 1)) Then
                                    If lConstituentKey.Substring(2).StartsWith("Header") Then
                                        lString.AppendLine()
                                        lString.AppendLine(lConstituentName.PadRight(12))
                                    Else
                                        lString.Append(lConstituentName.PadRight(12))
                                        'fill in values for each land use
                                        Dim lValueTotal As Double = 0.0
                                        For Each lLandUse As String In lLandUses.Keys
                                            If lLandUse.StartsWith(lOperationType.Substring(0, 1)) Then
                                                Dim lValue As Double = lLandUseConstituentTotals.ItemByKey(lConstituentKey & "-" & lLandUse)
                                                lValueTotal += lValue
                                                lString.Append(vbTab & DecimalAlign(lValue / lLandUseAreas.ItemByKey(lLandUse)))
                                            End If
                                        Next
                                        lString.AppendLine(vbTab & DecimalAlign(lValueTotal / lAreaTotal))
                                    End If
                                End If
                            Next
                            lString.AppendLine()
                        End If
                    Next
                Catch lEx As Exception
                    Logger.Dbg("Whoops " & lEx.Message)
                End Try
            End If

            ' simple report - PERLND, IMPLND, RCHRES summary
            lString.AppendLine()
            lString.AppendLine()
            lString.AppendLine(aBalanceType & " Balance for Subbasin " & aOutletLocation)
            lString.AppendLine(Header(aBalanceType, aScenario, aRunMade, aUci))
            lString.AppendLine()
            lString.AppendLine("Area Summary (acres)")
            Dim lTotalArea As Double = 0.0
            For Each lOperationType As String In lOperationTypeAreas.Keys
                Dim lArea As Double = lOperationTypeAreas.ItemByKey(lOperationType)
                lTotalArea += lArea
                lString.AppendLine(("  " & lOperationType).PadRight(12) & DecimalAlign(lArea, , , 8))
            Next
            lString.AppendLine()
            lString.AppendLine("  RCHRES".PadRight(12) & DecimalAlign(lTotalArea, , , 8))
            lString.AppendLine()
            If aBalanceType = "Water" Then
                lString.AppendLine(Space(12) & vbTab & "OverOperType".PadLeft(12) & _
                                                   vbTab & "Land".PadRight(12) & _
                                                   vbTab & "OverAll".PadLeft(12))
                lString.AppendLine(Space(12) & vbTab & "Inches".PadLeft(12) & _
                                               vbTab & "Ac-Ft".PadRight(12) & _
                                               vbTab & "Inches".PadLeft(12))
            End If

            For Each lOperationType As String In aOperationTypes
                Dim lNeedHeader As String = True
                Dim lOperationTypeArea As Double = lOperationTypeAreas.ItemByKey(lOperationType)
                For Each lConstituentKey As String In lConstituentsToOutput.Keys
                    If lConstituentKey.StartsWith(lOperationType.Substring(0, 1)) Then
                        If lNeedHeader Then
                            lString.AppendLine()
                            If lOperationType <> "RCHRES" Then
                                lString.AppendLine(lOperationType) '& vbTab & vbTab & "Area" & vbTab & DecimalAlign(lOperationTypeArea))
                            Else
                                lString.AppendLine(Space(12) & vbTab & "Reach".PadLeft(12) & _
                                                               vbTab & "Outlets".PadRight(12))
                                lString.AppendLine(Space(12) & vbTab & "Inches".PadLeft(12) & _
                                                               vbTab & "Ac-Ft".PadRight(12))
                                lString.AppendLine("RCHRES") ' & vbTab & vbTab & "Area" & vbTab & DecimalAlign(lTotalArea))
                            End If
                            lNeedHeader = False
                        End If
                        Dim lConstituentName As String = lConstituentsToOutput.ItemByKey(lConstituentKey)
                        Dim lConstituentTotalIndex As Integer = lConstituentTotals.IndexFromKey(lConstituentKey)
                        If lConstituentTotalIndex >= 0 Then
                            Dim lValue As Double = lConstituentTotals.Item(lConstituentTotalIndex)
                            If Math.Abs(lValue) > 0.00001 Then
                                If lOperationType = "RCHRES" Then
                                    lString.Append(lConstituentName.PadRight(12) & vbTab & _
                                               DecimalAlign(lValue / lTotalArea) & vbTab & _
                                               DecimalAlign(lValue / 12))
                                Else
                                    lString.Append(lConstituentName.PadRight(12) & vbTab & _
                                               DecimalAlign(lValue / lOperationTypeArea) & vbTab & _
                                               DecimalAlign(lValue / 12) & vbTab & _
                                               DecimalAlign(lValue / lTotalArea))
                                End If
                                lString.AppendLine()
                            Else
                                'Logger.Dbg("SkipNoData:" & lConstituentKey)
                            End If
                        ElseIf Not lConstituentKey.Substring(2).StartsWith("Header") Then
                            lString.AppendLine(lConstituentName.PadRight(12) & vbTab & _
                                               DecimalAlign(0.0) & vbTab & _
                                               DecimalAlign(0.0))
                        Else
                            lString.AppendLine()
                            lString.AppendLine(lConstituentName.PadRight(12))
                        End If
                    End If
                Next
            Next
        End If
        Return lString
    End Function

    Private Function Header(ByVal aBalanceType As String, ByVal aScenario As String, ByVal aRunMade As Date, ByVal auci As atcUCI.HspfUci) As String
        Dim lString As String = "   Run Made " & aRunMade & vbCrLf
        lString &= "   " & auci.GlobalBlock.RunInf.Value & vbCrLf
        lString &= "   " & auci.GlobalBlock.RunPeriod
        Return lString
    End Function
End Module
