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
                     Optional ByVal aOutletDetails As Boolean = False, _
                     Optional ByVal aSegmentRows As Boolean = False, _
                     Optional ByVal aDecimalPlaces As Integer = 3, _
                     Optional ByVal aSignificantDigits As Integer = 5, _
                     Optional ByVal aFieldWidth As Integer = 12)
        For Each lOutletLocation As String In aOutletLocations
            Dim lString As Text.StringBuilder = Report(aUci, aBalanceType, _
                                                       aOperationTypes, _
                                                       aScenario, aScenarioResults, _
                                                       aRunMade, lOutletLocation, _
                                                       aOutFilePrefix, True, _
                                                       aSegmentRows, aDecimalPlaces, aSignificantDigits, aFieldWidth)
            Dim lPivotString As String = ""
            If aSegmentRows Then
                lPivotString = "Pivot"
            End If
            Dim lOutFileName As String = aOutFilePrefix & SafeFilename(aScenario & "_" & lOutletLocation & "_" & aBalanceType & "_" & "Balance" & lPivotString & ".txt")
            Logger.Dbg("  WriteReportTo " & lOutFileName)
            SaveFileString(lOutFileName, lString.ToString)
        Next lOutletLocation

        If aOutletDetails Then 'final summary for all locations
            Try
                Dim lGroups As New atcCollection ' of Constituents keyed by Group
                Dim lConstituents As New atcCollection   ' of SummaryDetails keyed by Constituent
                Dim lSummaryDetails As atcCollection
                Dim lSummaryDetail As SummaryDetail
                For Each lOutletLocation As String In aOutletLocations
                    Dim lSummaryFileName As String = aOutFilePrefix & SafeFilename(aScenario & "_" & lOutletLocation & "_" & aBalanceType & "_" & "BalanceSummary.txt")
                    Dim lSurfaceIndex As Integer = -1
                    Dim lBaseETIndex As Integer = -1
                    Dim lCurrentOperation As String = ""
                    Dim lCurrentGroup As String = ""
                    For Each lString As String In LinesInFile(lSummaryFileName)
                        Dim lFields() As Object = lString.Split(vbTab)
                        If (lFields.GetUpperBound(0) = 3 OrElse _
                            lFields.GetUpperBound(0) = 2 AndAlso lCurrentOperation = "RCHRES") _
                            AndAlso lFields(0).ToString.Trim.Length > 0 Then
                            Dim lConstituent As String = lFields(0).ToString.Trim
                            If aBalanceType = "Water" Then
                                If lConstituent = "Surface" Then
                                    lConstituent &= "-" & lCurrentOperation.Substring(0, 3)
                                ElseIf lConstituent = "Actual" Then
                                    lConstituent = "Impervious"
                                ElseIf lConstituent = "Baseflow" AndAlso lCurrentGroup = "Evaporation" AndAlso lBaseETIndex = -1 Then
                                    lBaseETIndex = lConstituents.Count + 1
                                End If
                            End If

                            If lConstituents.IndexFromKey(lConstituent) = -1 Then
                                lSummaryDetails = New atcCollection ' of summary details
                                If lConstituent.Contains("Surface") Then
                                    If lSurfaceIndex > -1 Then
                                        lConstituents.Insert(lSurfaceIndex, lConstituent, lSummaryDetails)
                                    Else
                                        lConstituents.Add(lConstituent, lSummaryDetails)
                                        lSurfaceIndex = lConstituents.Count
                                    End If
                                ElseIf lConstituent = "Impervious" Then
                                    lConstituents.Insert(lBaseETIndex, lConstituent, lSummaryDetails)
                                Else
                                    lConstituents.Add(lConstituent, lSummaryDetails)
                                End If
                            Else
                                lSummaryDetails = lConstituents.ItemByKey(lConstituent)
                            End If

                            If lSummaryDetails.IndexFromKey(lOutletLocation) = -1 Then
                                lSummaryDetail = New SummaryDetail
                                lSummaryDetails.Add(lOutletLocation, lSummaryDetail)
                            Else
                                lSummaryDetail = lSummaryDetails.ItemByKey(lOutletLocation)
                            End If
                            With lSummaryDetail
                                .Mass += lFields(2)
                                If lCurrentOperation = "RCHRES" Then
                                    .UnitValue += lFields(1)
                                Else
                                    .UnitValue += lFields(3)
                                    If lConstituent = "Impervious" OrElse lConstituent = "Surface-IMP" Then
                                        Dim lSummaryDetailTotal As SummaryDetail = lConstituents.ItemByKey("Total").ItemByKey(lOutletLocation)
                                        lSummaryDetailTotal.UnitValue += .UnitValue
                                        lSummaryDetailTotal.Mass += .Mass
                                    End If
                                End If
                            End With
                        ElseIf lString.Trim.Length = 6 AndAlso aOperationTypes.Contains(lString.Trim) Then
                            lCurrentOperation = lString.Trim
                        ElseIf lString.Trim.Length > 0 AndAlso lFields.GetUpperBound(0) = 0 AndAlso lCurrentOperation.Length > 0 Then
                            lCurrentGroup = lString.Trim
                            If lGroups.IndexFromKey(lCurrentGroup) = -1 Then
                                lConstituents = New atcCollection ' of Constituents
                                lGroups.Add(lCurrentGroup, lConstituents)
                            Else
                                lConstituents = lGroups.ItemByKey(lCurrentGroup)
                            End If
                        End If
                    Next
                Next

                Dim lStringSB As New Text.StringBuilder
                lStringSB.AppendLine(aBalanceType & " Balance Report For " & aScenario)
                lStringSB.AppendLine(Header(aBalanceType, aScenario, aRunMade, aUci))
                lStringSB.Append("Location".PadLeft(12))
                For Each lLocation As String In aOutletLocations
                    lStringSB.Append(vbTab & lLocation.PadLeft(12) & vbTab & Space(12))
                Next
                lStringSB.AppendLine()
                lStringSB.Append(Space(12))
                For Each lLocation As String In aOutletLocations
                    If aBalanceType = "Water" Then
                        lStringSB.Append(vbTab & "in".PadLeft(12) & vbTab & "ac-ft".PadLeft(12))
                    End If
                Next
                lStringSB.AppendLine()
                For Each lGroup As String In lGroups.Keys
                    lStringSB.AppendLine()
                    lStringSB.AppendLine(lGroup)
                    lConstituents = lGroups.ItemByKey(lGroup)
                    For Each lConstituent As String In lConstituents.Keys
                        lSummaryDetails = lConstituents.ItemByKey(lConstituent)
                        lStringSB.Append("  " & lConstituent.PadRight(12))
                        For Each lSummaryDetail In lSummaryDetails
                            With lSummaryDetail
                                lStringSB.Append(vbTab & DecimalAlign(.UnitValue) _
                                               & vbTab & DecimalAlign(.Mass, , 1))
                            End With
                        Next
                        lStringSB.AppendLine()
                    Next
                Next
                SaveFileString(aOutFilePrefix & SafeFilename(aScenario & "_" & aBalanceType & "_BalanceBasin.txt"), lStringSB.ToString)
            Catch lEx As Exception
                Logger.Dbg("Whoops!")
            End Try
        End If
    End Sub

    Friend Class SummaryDetail
        Friend UnitValue As Double = 0.0
        Friend Mass As Double = 0.0
    End Class

    Public Function Report(ByVal aUci As atcUCI.HspfUci, _
                           ByVal aBalanceType As String, _
                           ByVal aOperationTypes As atcCollection, _
                           ByVal aScenario As String, _
                           ByVal aScenarioResults As atcDataSource, _
                           ByVal aRunMade As String, _
                  Optional ByVal aOutletLocation As String = "", _
                  Optional ByVal aOutFilePrefix As String = "", _
                  Optional ByVal aOutletDetails As Boolean = False, _
                  Optional ByVal aSegmentRows As Boolean = False, _
                  Optional ByVal aDecimalPlaces As Integer = 3, _
                  Optional ByVal aSignificantDigits As Integer = 5, _
                  Optional ByVal aFieldWidth As Integer = 12) As Text.StringBuilder

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
        lString.AppendLine()
        lString.AppendLine()

        Dim lConstituentDataGroup As atcDataGroup
        Dim lTempDataSet As atcDataSet
        Dim lPendingOutput As String = ""
        Dim lOperationTypeAreas As New atcCollection
        Dim lOperationAreas As New atcCollection
        Dim lLandUseAreas As New atcCollection
        Dim lLandUseConstituentTotals As New atcCollection
        Dim lFieldIndex As Integer

        Logger.Dbg("OperationTypesCount:" & aOperationTypes.Count)
        For Each lOperationType As String In aOperationTypes
            Logger.Dbg("ProcessType:" & lOperationType)
            Dim lValueOutlet As Double = 0.0
            For Each lLandUse As String In lLandUses.Keys
                If lOperationType.StartsWith(lLandUse.Substring(0, 1)) Then
                    Try
                        Dim lNeedHeader As Boolean = True
                        Dim lLandUseOperations As atcCollection = lLandUses.ItemByKey(lLandUse)
                        Dim lOutputTable As New atcTableDelimited
                        With lOutputTable
                            For Each lConstituentKey As String In lConstituentsToOutput.Keys
                                If lConstituentKey.StartsWith(lOperationType.Substring(0, 1)) Then
                                    Dim lConstituentName As String = lConstituentsToOutput.ItemByKey(lConstituentKey)
                                    lConstituentKey = lConstituentKey.Remove(0, 2)
                                    lConstituentDataGroup = aScenarioResults.DataSets.FindData("Constituent", lConstituentKey)
                                    If lConstituentDataGroup.Count > 0 Then
                                        If lNeedHeader Then
                                            'lString.AppendLine(vbCrLf)
                                            'lString.AppendLine(aBalanceType & " Balance Report For " & lLandUse & vbCrLf)
                                            .Delimiter = vbTab
                                            .Header = aBalanceType & " Balance Report For " & lLandUse & vbCrLf & " "
                                            .NumHeaderRows = 2
                                            If lOutletReport And lOperationType <> "RCHRES" Then
                                                .NumFields = lLandUseOperations.Count + 2
                                            Else
                                                .NumFields = lLandUseOperations.Count + 1
                                            End If
                                            'get operation description for header
                                            'lString.Append((lOperationType & ":").PadRight(12))
                                            .FieldLength(1) = 12
                                            .FieldName(1) = (lOperationType & ":").PadRight(.FieldLength(1))
                                            lFieldIndex = 1
                                            For lOperationIndex As Integer = 0 To lLandUseOperations.Count - 1
                                                Dim lOperationName As String = lLandUseOperations.Keys(lOperationIndex)
                                                'lString.Append(vbTab & (lOperationName & "  ").PadLeft(12))
                                                lFieldIndex += 1
                                                .FieldLength(lFieldIndex) = 12
                                                .FieldName(lFieldIndex) = (lOperationName & "  ").PadLeft(.FieldLength(lFieldIndex))
                                            Next
                                            If lOutletReport And lOperationType <> "RCHRES" Then
                                                'lString.Append(vbTab & "WtdAvg  ".PadLeft(12))
                                                lFieldIndex += 1
                                                .FieldLength(lFieldIndex) = 12
                                                .FieldName(lFieldIndex) = ("WtdAvg  ").PadLeft(.FieldLength(lFieldIndex))
                                            End If
                                            .CurrentRecord = 1
                                            'lString.AppendLine()
                                            If lOutletReport And lOperationType <> "RCHRES" Then
                                                'lString.AppendLine()
                                                'lString.Append("Area".PadRight(12))
                                                .Value(1) = "Area".PadRight(12)
                                                lFieldIndex = 1
                                                For Each lOperationKey As String In lLandUseOperations.Keys
                                                    Dim lOperationArea As Double = lLandUseOperations.ItemByKey(lOperationKey)
                                                    'lString.Append(vbTab & DecimalAlign(lOperationArea))
                                                    lFieldIndex += 1
                                                    .Value(lFieldIndex) = DecimalAlign(lOperationArea)
                                                    lOperationAreas.Increment(lOperationKey, lOperationArea)
                                                    lLandUseAreas.Increment(lLandUse, lOperationArea)
                                                Next
                                                'lString.Append(vbTab & DecimalAlign(lLandUseAreas.ItemByKey(lLandUse)))
                                                lFieldIndex += 1
                                                .Value(lFieldIndex) = DecimalAlign(lLandUseAreas.ItemByKey(lLandUse))
                                                lOperationTypeAreas.Increment(lOperationType, lLandUseAreas.ItemByKey(lLandUse))
                                                .CurrentRecord += 1
                                                'lString.AppendLine(vbTab & "(Sum)")
                                            End If
                                            lNeedHeader = False
                                        End If
                                        If lPendingOutput.Length > 0 Then
                                            Dim lPendingRecords() As String = lPendingOutput.Split(vbCr)
                                            For Each lPendingRecord As String In lPendingRecords
                                                .Value(1) = lPendingRecord
                                                .CurrentRecord += 1
                                            Next
                                            lPendingOutput = ""
                                        End If
                                        'lString.Append(lConstituentName.PadRight(12))
                                        .Value(1) = lConstituentName.PadRight(12)
                                        Dim lWeightAccum As Double = 0.0
                                        Dim lValue As Double = 0.0
                                        lFieldIndex = 1

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
                                            lFieldIndex += 1
                                            .Value(lFieldIndex) = DecimalAlign(lValue)
                                            'lString.Append(vbTab & DecimalAlign(lValue))
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
                                                'lString.Append(vbTab & DecimalAlign(lWeightAccum / lLandUseAreas.ItemByKey(lLandUse)))
                                                lFieldIndex += 1
                                                .Value(lFieldIndex) = DecimalAlign(lWeightAccum / lLandUseAreas.ItemByKey(lLandUse))
                                            ElseIf Math.Abs(lValueOutlet) > 0.00001 Then
                                                Dim lConstituentTotalKeyIndex As Integer = lConstituentTotals.IndexFromKey(lConstituentTotalKey)
                                                lConstituentTotals.Increment(lConstituentTotalKey, lValueOutlet)
                                                lValueOutlet = 0.0
                                            End If
                                        End If
                                        'lString.AppendLine()
                                        .CurrentRecord += 1
                                    ElseIf lConstituentKey.StartsWith("Total") AndAlso _
                                           lConstituentKey.Length > 5 AndAlso _
                                           IsNumeric(lConstituentKey.Substring(5)) Then
                                        Dim lTotalCount As Integer = lConstituentKey.Substring(5)
                                        Dim lCurFieldValues(.NumFields) As Double
                                        Dim lCurrentRecordSave As Integer = .CurrentRecord
                                        For lCount As Integer = 1 To lTotalCount
                                            .CurrentRecord -= 1
                                            For lFieldIndex = 2 To lCurFieldValues.GetUpperBound(0)
                                                If IsNumeric(.Value(lFieldIndex)) Then
                                                    lCurFieldValues(lFieldIndex) += .Value(lFieldIndex)
                                                Else
                                                    Logger.Dbg("Why")
                                                End If
                                            Next
                                        Next
                                        .CurrentRecord = lCurrentRecordSave
                                        .Value(1) = lConstituentName.PadRight(12)
                                        For lFieldIndex = 2 To lCurFieldValues.GetUpperBound(0)
                                            .Value(lFieldIndex) = DecimalAlign(lCurFieldValues(lFieldIndex))
                                        Next
                                        .CurrentRecord += 1
                                    Else
                                        If lPendingOutput.Length > 0 Then
                                            lPendingOutput &= vbCrLf
                                        ElseIf lConstituentKey.StartsWith("Header") Then
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
                            If aSegmentRows Then
                                lString.AppendLine(.ToStringPivoted)
                            Else
                                lString.AppendLine(.ToString)
                            End If
                        End With
                    Catch lEx As Exception
                        Logger.Dbg(lEx.Message)
                    End Try
                End If
            Next lLandUse
        Next lOperationType

        If lOutletReport Then 'watershed summary report at specified output
            If aOutletDetails Then
                Dim lDetailsSB As New Text.StringBuilder
                Try
                    lDetailsSB.AppendLine(aBalanceType & " Balance by Land Use Category")
                    lDetailsSB.AppendLine(Header(aBalanceType, aScenario, aRunMade, aUci))
                    lDetailsSB.AppendLine()
                    For Each lOperationType As String In aOperationTypes
                        If Not lOperationType.StartsWith("R") Then
                            lDetailsSB.AppendLine(lOperationType)
                            lDetailsSB.AppendLine()
                            lDetailsSB.Append("LandUse".PadRight(12))
                            For Each lLandUse As String In lLandUses.Keys
                                If lLandUse.StartsWith(lOperationType.Substring(0, 1)) Then
                                    lDetailsSB.Append(lLandUse.Substring(2))
                                End If
                            Next
                            lDetailsSB.AppendLine(vbTab & "WtdAvg".PadLeft(12))
                            lDetailsSB.AppendLine()
                            lDetailsSB.Append("Area".PadRight(12))
                            Dim lAreaTotal As Double = 0.0
                            For Each lLandUse As String In lLandUses.Keys
                                If lLandUse.StartsWith(lOperationType.Substring(0, 1)) Then
                                    Dim lArea As Double = lLandUseAreas.ItemByKey(lLandUse)
                                    lAreaTotal += lArea
                                    lDetailsSB.Append(vbTab & DecimalAlign(lArea, , , 8))
                                End If
                            Next
                            lDetailsSB.AppendLine(vbTab & DecimalAlign(lAreaTotal, , , 8) & vbTab & "(Sum)")

                            For Each lConstituentKey As String In lConstituentsToOutput.Keys
                                Dim lConstituentName As String = lConstituentsToOutput.ItemByKey(lConstituentKey)
                                If lConstituentKey.StartsWith(lOperationType.Substring(0, 1)) Then
                                    If lConstituentKey.Substring(2).StartsWith("Header") Then
                                        lDetailsSB.AppendLine()
                                        lDetailsSB.AppendLine(lConstituentName.PadRight(12))
                                    Else
                                        lDetailsSB.Append(lConstituentName.PadRight(12))
                                        'fill in values for each land use
                                        Dim lValueTotal As Double = 0.0
                                        For Each lLandUse As String In lLandUses.Keys
                                            If lLandUse.StartsWith(lOperationType.Substring(0, 1)) Then
                                                Dim lValue As Double = lLandUseConstituentTotals.ItemByKey(lConstituentKey & "-" & lLandUse)
                                                lValueTotal += lValue
                                                lDetailsSB.Append(vbTab & DecimalAlign(lValue / lLandUseAreas.ItemByKey(lLandUse)))
                                            End If
                                        Next
                                        lDetailsSB.AppendLine(vbTab & DecimalAlign(lValueTotal / lAreaTotal))
                                    End If
                                End If
                            Next
                            lDetailsSB.AppendLine()
                        End If
                    Next
                    Dim lDetailsFileName As String = aOutFilePrefix & SafeFilename(aScenario & "_" & aOutletLocation & "_" & aBalanceType & "_" & "BalanceDetails.txt")
                    SaveFileString(lDetailsFileName, lDetailsSB.ToString)
                    lDetailsSB = Nothing
                Catch lEx As Exception
                    Logger.Dbg("Whoops " & lEx.Message)
                End Try
            End If

            ' simple report - PERLND, IMPLND, RCHRES summary
            Dim lSummarySB As New Text.StringBuilder
            lSummarySB.AppendLine(aBalanceType & " Balance for Subbasin " & aOutletLocation)
            lSummarySB.AppendLine(Header(aBalanceType, aScenario, aRunMade, aUci))
            lSummarySB.AppendLine()
            lSummarySB.AppendLine("Area Summary (acres)")
            Dim lTotalArea As Double = 0.0
            For Each lOperationType As String In lOperationTypeAreas.Keys
                Dim lArea As Double = lOperationTypeAreas.ItemByKey(lOperationType)
                lTotalArea += lArea
                lSummarySB.AppendLine(("  " & lOperationType).PadRight(12) & DecimalAlign(lArea, , , 8))
            Next
            lSummarySB.AppendLine()
            lSummarySB.AppendLine("  RCHRES".PadRight(12) & DecimalAlign(lTotalArea, , , 8))
            lSummarySB.AppendLine()
            If aBalanceType = "Water" Then
                lSummarySB.AppendLine(Space(12) & vbTab & "OverOperType".PadLeft(12) & _
                                                  vbTab & "Land".PadRight(12) & _
                                                  vbTab & "OverAll".PadLeft(12))
                lSummarySB.AppendLine(Space(12) & vbTab & "Inches".PadLeft(12) & _
                                                  vbTab & "Ac-Ft".PadRight(12) & _
                                                  vbTab & "Inches".PadLeft(12))
            End If

            For Each lOperationType As String In aOperationTypes
                Dim lNeedHeader As String = True
                Dim lOperationTypeArea As Double = lOperationTypeAreas.ItemByKey(lOperationType)
                For Each lConstituentKey As String In lConstituentsToOutput.Keys
                    If lConstituentKey.StartsWith(lOperationType.Substring(0, 1)) Then
                        If lNeedHeader Then
                            lSummarySB.AppendLine()
                            If lOperationType <> "RCHRES" Then
                                lSummarySB.AppendLine(lOperationType) '& vbTab & vbTab & "Area" & vbTab & DecimalAlign(lOperationTypeArea))
                            Else
                                lSummarySB.AppendLine(Space(12) & vbTab & "Reach".PadLeft(12) & _
                                                                  vbTab & "Outlets".PadRight(12))
                                lSummarySB.AppendLine(Space(12) & vbTab & "Inches".PadLeft(12) & _
                                                                  vbTab & "Ac-Ft".PadRight(12))
                                lSummarySB.AppendLine("RCHRES") ' & vbTab & vbTab & "Area" & vbTab & DecimalAlign(lTotalArea))
                            End If
                            lNeedHeader = False
                        End If
                        Dim lConstituentName As String = lConstituentsToOutput.ItemByKey(lConstituentKey)
                        Dim lConstituentTotalIndex As Integer = lConstituentTotals.IndexFromKey(lConstituentKey)
                        If lConstituentTotalIndex >= 0 Then
                            Dim lValue As Double = lConstituentTotals.Item(lConstituentTotalIndex)
                            If Math.Abs(lValue) > 0.00001 Then
                                If lOperationType = "RCHRES" Then
                                    lSummarySB.Append(lConstituentName.PadRight(12) & vbTab & _
                                               DecimalAlign(lValue / lTotalArea) & vbTab & _
                                               DecimalAlign(lValue / 12))
                                Else
                                    lSummarySB.Append(lConstituentName.PadRight(12) & vbTab & _
                                               DecimalAlign(lValue / lOperationTypeArea) & vbTab & _
                                               DecimalAlign(lValue / 12) & vbTab & _
                                               DecimalAlign(lValue / lTotalArea))
                                End If
                                lSummarySB.AppendLine()
                            Else
                                'Logger.Dbg("SkipNoData:" & lConstituentKey)
                            End If
                        ElseIf Not lConstituentKey.Substring(2).StartsWith("Header") Then
                            lSummarySB.AppendLine(lConstituentName.PadRight(12) & vbTab & _
                                               DecimalAlign(0.0) & vbTab & _
                                               DecimalAlign(0.0))
                        Else
                            lSummarySB.AppendLine()
                            lSummarySB.AppendLine(lConstituentName.PadRight(12))
                        End If
                    End If
                Next
            Next
            Dim lSummaryFileName As String = aOutFilePrefix & SafeFilename(aScenario & "_" & aOutletLocation & "_" & aBalanceType & "_" & "BalanceSummary.txt")
            SaveFileString(lSummaryFileName, lSummarySB.ToString)
            lSummarySB = Nothing
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
