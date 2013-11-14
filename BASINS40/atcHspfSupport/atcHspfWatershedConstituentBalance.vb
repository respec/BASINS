Imports atcUtility
Imports atcData
Imports MapWinUtility
Imports System.Collections.ObjectModel

''' <summary>
''' Build watershed constituent balance reports
''' </summary>
''' <remarks></remarks>


Public Module WatershedConstituentBalance
    ''' <summary>
    ''' Build watershed constituent balance reports for watershed above specified output locations
    ''' </summary>
    ''' <param name="aUci">HSPF UCI object</param>
    ''' <param name="aBalanceType">Constituent name - see ConstituentsToOutput in modUtility for valid values</param>
    ''' <param name="aOperationTypes"></param>
    ''' <param name="aScenario"></param>
    ''' <param name="aScenarioResults"></param>
    ''' <param name="aOutletLocations"></param>
    ''' <param name="aRunMade"></param>
    ''' <param name="aOutFilePrefix"></param>
    ''' <param name="aOutletDetails"></param>
    ''' <param name="aSegmentRows">true if pivoted report desired</param>
    ''' <param name="aDecimalPlaces"></param>
    ''' <param name="aSignificantDigits"></param>
    ''' <param name="aFieldWidth"></param>
    ''' <param name="aSkipZeroOrNoValue"></param>
    ''' <remarks>see HSPFOutputReports script for sample usage</remarks>
    Public Sub ReportsToFiles(ByVal aUci As atcUCI.HspfUci, _
                              ByVal aBalanceType As String, _
                              ByVal aOperationTypes As atcCollection, _
                              ByVal aScenario As String, _
                              ByVal aScenarioResults As atcTimeseriesSource, _
                              ByVal aOutletLocations As atcCollection, _
                              ByVal aRunMade As String, _
                     Optional ByVal aOutFilePrefix As String = "", _
                     Optional ByVal aOutletDetails As Boolean = False, _
                     Optional ByVal aSegmentRows As Boolean = False, _
                     Optional ByVal aDecimalPlaces As Integer = 3, _
                     Optional ByVal aSignificantDigits As Integer = 5, _
                     Optional ByVal aFieldWidth As Integer = 12, _
                     Optional ByVal aSkipZeroOrNoValue As Boolean = True)



        For Each lOutletLocation As String In aOutletLocations
            Dim lReport As atcReport.ReportText = Report(aUci, aBalanceType, _
                                               aOperationTypes, _
                                               aScenario, aScenarioResults, _
                                               aRunMade, lOutletLocation, _
                                               aOutFilePrefix, True, _
                                               aSegmentRows, aDecimalPlaces, aSignificantDigits, aFieldWidth, aSkipZeroOrNoValue)

            Dim lPivotString As String = ""
            If aSegmentRows Then
                lPivotString = "Pivot"
            End If
            Dim lOutFileName As String = aOutFilePrefix & SafeFilename(ShortConstituentName(aBalanceType) & "_" & aScenario & "_" & lOutletLocation & "_Grp_By_OPN_LU_Ann_Avg" & lPivotString & ".txt")
            '_Balance.txt
            Logger.Dbg("  WriteReportTo " & lOutFileName)
            SaveFileString(lOutFileName, lReport.ToString)
        Next lOutletLocation

        If aOutletDetails Then 'final summary for all locations
            Try
                Dim lGroups As New atcCollection ' of Constituents keyed by Group
                Dim lConstituents As New atcCollection   ' of SummaryDetails keyed by Constituent
                Dim lSummaryDetails As atcCollection
                Dim lSummaryDetail As SummaryDetail
                For Each lOutletLocation As String In aOutletLocations
                    Dim lSummaryFileName As String = aOutFilePrefix & SafeFilename(ShortConstituentName(aBalanceType) & "_" & aScenario & "_" & lOutletLocation & "_Oall_Avgd.txt")
                    'BalanceSummary.txt
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
                            ElseIf aBalanceType = "TotalN" Then
                                If lConstituent = "Surface" Then
                                    lConstituent &= "-" & lCurrentOperation.Substring(0, 3)
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
                                    If lBaseETIndex = -1 Then
                                        Logger.Dbg("ReportsToFiles:Impervious:NoBaseETIndex")
                                    Else
                                        lConstituents.Insert(lBaseETIndex, lConstituent, lSummaryDetails)
                                    End If
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
                                lSurfaceIndex = -1
                                lConstituents = New atcCollection ' of Constituents
                                lGroups.Add(lCurrentGroup, lConstituents)
                            Else
                                lConstituents = lGroups.ItemByKey(lCurrentGroup)
                            End If
                        End If
                    Next lString
                Next lOutletLocation

                Dim lReport As New atcReport.ReportText
                lReport.AppendLine(aBalanceType & " Balance Report For " & aScenario)
                lReport.AppendLine(Header(aBalanceType, aScenario, aRunMade, aUci))
                lReport.Append("Location".PadLeft(12))
                For Each lLocation As String In aOutletLocations
                    lReport.Append(vbTab & lLocation.PadLeft(12) & vbTab & Space(12))
                Next lLocation
                lReport.AppendLine()
                lReport.Append(Space(12))
                For Each lLocation As String In aOutletLocations
                    If aBalanceType = "Water" Then
                        lReport.Append(vbTab & "in".PadLeft(12) & vbTab & "ac-ft".PadLeft(12))
                    End If
                Next lLocation
                lReport.AppendLine()
                For Each lGroup As String In lGroups.Keys
                    lReport.AppendLine()
                    lReport.AppendLine(lGroup)
                    lConstituents = lGroups.ItemByKey(lGroup)
                    For Each lConstituent As String In lConstituents.Keys
                        lSummaryDetails = lConstituents.ItemByKey(lConstituent)
                        lReport.Append("  " & lConstituent.PadRight(12))
                        For Each lSummaryDetail In lSummaryDetails
                            With lSummaryDetail
                                lReport.Append(vbTab & DecimalAlign(.UnitValue) _
                                               & vbTab & DecimalAlign(.Mass, , 1))
                            End With
                        Next
                        lReport.AppendLine()
                    Next
                Next lGroup
                SaveFileString(aOutFilePrefix & SafeFilename(ShortConstituentName(aBalanceType) & "_" & aScenario & "_OvAll_Avg_By_Mult_Locs.txt"), lReport.ToString)
                'Mult_BalanceBasin.txt
            Catch lEx As Exception
                Logger.Dbg("Whoops!")
            End Try
        End If
    End Sub

    Private Function ShortConstituentName(ByVal aBalanceType As String) As String
        Select Case aBalanceType
            Case "Water"
                Return "WAT"
            Case "Sediment"
                Return "SED"
            Case "N-PQUAL"
                Return "N"
            Case "P-PQUAL"
                Return "P"
            Case "TotalN"
                Return "TN"
            Case "TotalP"
                Return "TP"
            Case "BOD"
                Return "BOD"
        End Select
        Return aBalanceType
    End Function

    Friend Class SummaryDetail
        Friend UnitValue As Double = 0.0
        Friend Mass As Double = 0.0
    End Class

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="aUci"></param>
    ''' <param name="aBalanceType"></param>
    ''' <param name="aOperationTypes"></param>
    ''' <param name="aScenario"></param>
    ''' <param name="aScenarioResults"></param>
    ''' <param name="aRunMade"></param>
    ''' <param name="aOutletLocation"></param>
    ''' <param name="aOutFilePrefix"></param>
    ''' <param name="aOutletDetails"></param>
    ''' <param name="aSegmentRows"></param>
    ''' <param name="aDecimalPlaces"></param>
    ''' <param name="aSignificantDigits"></param>
    ''' <param name="aFieldWidth"></param>
    ''' <param name="aSkipZeroOrNoValue"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Report(ByVal aUci As atcUCI.HspfUci, _
                           ByVal aBalanceType As String, _
                           ByVal aOperationTypes As atcCollection, _
                           ByVal aScenario As String, _
                           ByVal aScenarioResults As atcTimeseriesSource, _
                           ByVal aRunMade As String, _
                  Optional ByVal aOutletLocation As String = "", _
                  Optional ByVal aOutFilePrefix As String = "", _
                  Optional ByVal aOutletDetails As Boolean = False, _
                  Optional ByVal aSegmentRows As Boolean = False, _
                  Optional ByVal aDecimalPlaces As Integer = 3, _
                  Optional ByVal aSignificantDigits As Integer = 5, _
                  Optional ByVal aFieldWidth As Integer = 12, _
                  Optional ByVal aSkipZeroOrNoValue As Boolean = True) As atcReport.IReport

        Dim lOutletReport As Boolean = False
        If aOutletLocation.Length > 0 Then
            lOutletReport = True
        End If

        Dim lConstituentsToOutput As atcCollection = ConstituentsToOutput(aBalanceType)
        Logger.Dbg("ConstituentCount:" & lConstituentsToOutput.Count)
        Dim lConstituentTotals As New atcCollection
        Dim lConstituentLandUseTotals As New atcCollection

        Dim lLandUses As atcCollection = LandUses(aUci, aOperationTypes, aOutletLocation)
        Logger.Dbg("LandUseCount:" & lLandUses.Count)

        Dim lReport As New atcReport.ReportText
        lReport.AppendLine(aBalanceType & " Watershed Balance Report For " & aScenario)
        lReport.AppendLine(Header(aBalanceType, aScenario, aRunMade, aUci))
        If aBalanceType = "Water" Then
            If aUci.GlobalBlock.EmFg = 1 Then
                lReport.AppendLine("   (Units:Inches)")
            Else
                lReport.AppendLine("   (Units:mm)")
            End If
        End If
        lReport.AppendLine()
        lReport.AppendLine()

        Dim lConstituentDataGroup As atcTimeseriesGroup
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
            Dim lConstituentKey As String
            For Each lLandUse As String In lLandUses.Keys
               
                If lOperationType.StartsWith(lLandUse.Substring(0, 1)) Then
                    Try
                        Dim lNeedHeader As Boolean = True
                        Dim lLandUseOperations As atcCollection = lLandUses.ItemByKey(lLandUse)
                        Dim lOutputTable As New atcTableDelimited
                        Dim lLoadTotals As New Loads
                        Dim lLoadUnit() As Double = Nothing
                        With lOutputTable
                            'For Each lConstituentKey As String In lConstituentsToOutput.Keys
                            For lConstituentIndex As Integer = 0 To lConstituentsToOutput.Count - 1 'Anurag Changed the way For loop works to enable easy skipping when there are no AgCHEM constituents. Basically if the HBN file has no data for ("P:NO3+NO2-N - SURFACE LAYER OUTFLOW") or ("P:PO4-P IN SOLUTION - SURFACE LAYER - OUTFLOW") the code assumes that there are no AGCHEM constituents for that specific PERLND and looks for the PQUAL constituents
                                'For Each lConstituentKey In lConstituentsToOutput.Keys
                                lConstituentKey = lConstituentsToOutput.Keys(lConstituentIndex)
                                If lConstituentKey.StartsWith(lOperationType.Substring(0, 1)) Then
                                    Dim lConstituentName As String = lConstituentsToOutput.ItemByKey(lConstituentKey)
                                    Dim lConstituentDataName As String = lConstituentKey
                                    Dim lMultipleIndex As Integer = 0
                                    If Not lConstituentKey.ToLower.Contains("header") AndAlso Not lConstituentKey.ToLower.Contains("total") Then
                                        If lConstituentDataName.EndsWith("1") Or lConstituentDataName.EndsWith("2") Then
                                            lMultipleIndex = lConstituentDataName.Substring(lConstituentDataName.Length - 1)
                                            lConstituentDataName = lConstituentDataName.Substring(0, lConstituentDataName.Length - 1)
                                        End If
                                    End If
                                    lConstituentKey = lConstituentKey.Remove(0, 2)
                                    lConstituentDataName = lConstituentDataName.Remove(0, 2)
                                    lConstituentDataGroup = aScenarioResults.DataSets.FindData("Constituent", lConstituentDataName)
                                    If lConstituentDataGroup.Count > 0 Then
                                        If lNeedHeader Then
                                            .Delimiter = vbTab
                                            .Header = aBalanceType & " Balance Report For " & lLandUse & vbCrLf & " "
                                            .NumHeaderRows = 2
                                            If lOutletReport And lOperationType <> "RCHRES" Then
                                                .NumFields = lLandUseOperations.Count + 2
                                            Else
                                                .NumFields = lLandUseOperations.Count + 1
                                            End If
                                            'get operation description for header
                                            lFieldIndex = 1
                                            .FieldLength(lFieldIndex) = 12
                                            .FieldName(lFieldIndex) = (lOperationType & ":").PadRight(.FieldLength(1))
                                            For lOperationIndex As Integer = 0 To lLandUseOperations.Count - 1
                                                Dim lOperationName As String = lLandUseOperations.Keys(lOperationIndex)
                                                lFieldIndex += 1
                                                .FieldLength(lFieldIndex) = aFieldWidth
                                                .FieldName(lFieldIndex) = (lOperationName & "  ").PadLeft(.FieldLength(lFieldIndex))
                                            Next
                                            If lOutletReport And lOperationType <> "RCHRES" Then
                                                lFieldIndex += 1
                                                .FieldLength(lFieldIndex) = aFieldWidth
                                                .FieldName(lFieldIndex) = ("WtdAvg  ").PadLeft(.FieldLength(lFieldIndex))
                                            End If
                                            .CurrentRecord = 1
                                            If lOutletReport And lOperationType <> "RCHRES" Then
                                                .Value(1) = "Area".PadRight(aFieldWidth)
                                                lFieldIndex = 1
                                                For Each lOperationKey As String In lLandUseOperations.Keys
                                                    Dim lOperationArea As Double = lLandUseOperations.ItemByKey(lOperationKey)
                                                    lFieldIndex += 1
                                                    .Value(lFieldIndex) = DecimalAlign(lOperationArea, aFieldWidth, aDecimalPlaces, aSignificantDigits)
                                                    lOperationAreas.Increment(lOperationKey, lOperationArea)
                                                    lLandUseAreas.Increment(lLandUse, lOperationArea)
                                                Next
                                                lFieldIndex += 1
                                                .Value(lFieldIndex) = DecimalAlign(lLandUseAreas.ItemByKey(lLandUse), aFieldWidth, aDecimalPlaces, aSignificantDigits)
                                                lOperationTypeAreas.Increment(lOperationType, lLandUseAreas.ItemByKey(lLandUse))
                                                .CurrentRecord += 1
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
                                        .Value(1) = lConstituentName.PadRight(12)
                                        Dim lWeightAccum As Double = 0.0
                                        Dim lValue As Double = 0.0
                                        lFieldIndex = 1
                                        Dim lSomeLocationHasData As Boolean = False

                                        For Each lLocation As String In lLandUseOperations.Keys
                                            'lLocation e.g. R:69
                                            Dim lLocationDataGroup As atcTimeseriesGroup = lConstituentDataGroup.FindData("Location", lLocation)
                                            If lLocationDataGroup.Count > 0 Then
                                                lSomeLocationHasData = True
                                                lTempDataSet = lLocationDataGroup.Item(0)
                                                Dim lMult As Double = 1.0
                                                Select Case lConstituentDataName
                                                    Case "POQUAL-BOD", "SOQUAL-BOD", "IOQUAL-BOD", "AOQUAL-BOD"
                                                        'might need another multiplier for bod
                                                        If aBalanceType = "BOD" Then
                                                            lMult = 0.4
                                                        ElseIf aBalanceType = "OrganicN" Or aBalanceType = "TotalN" Then
                                                            If lMultipleIndex = 1 Then
                                                                lMult = 0.048
                                                            ElseIf lMultipleIndex = 2 Then
                                                                lMult = 0.05294
                                                            End If
                                                        ElseIf aBalanceType = "OrganicP" Or aBalanceType = "TotalP" Then
                                                            If lMultipleIndex = 1 Then
                                                                lMult = 0.0023
                                                            ElseIf lMultipleIndex = 2 Then
                                                                lMult = 0.007326
                                                            End If
                                                        End If
                                                End Select
                                                If aBalanceType = "FColi" Then
                                                    lMult = 1 / 1000000000.0 '10^9
                                                End If
                                                Dim lAttribute As atcDefinedValue
                                                If ConstituentsThatUseLast.Contains(lConstituentDataName) Then
                                                    lAttribute = lTempDataSet.Attributes.GetDefinedValue("Last")
                                                Else
                                                    lAttribute = lTempDataSet.Attributes.GetDefinedValue("SumAnnual")
                                                End If
                                                If lAttribute Is Nothing Then
                                                    lValue = GetNaN()
                                                Else
                                                    lValue = lMult * lAttribute.Value
                                                End If
                                            Else
                                                lValue = 0.0

                                                'Logger.Dbg("SkipLocation:" & lLocation & ":WithNo:" & lConstituentKey & ":Data")
                                            End If
                                            
                                            ReDim Preserve lLoadUnit(lFieldIndex - 1)
                                            lLoadUnit(lFieldIndex - 1) = lValue
                                            lFieldIndex += 1
                                            .Value(lFieldIndex) = DecimalAlign(lValue, aFieldWidth, aDecimalPlaces, aSignificantDigits)
                                            If lOutletReport Then
                                                If lOperationType <> "RCHRES" Then
                                                    lWeightAccum += lValue * lOperationAreas.ItemByKey(lLocation)
                                                ElseIf lLocation = aOutletLocation Then
                                                    If aBalanceType = "Water" Then
                                                        lValueOutlet = lValue * 12  'feet to inches
                                                    Else
                                                        lValueOutlet = lValue
                                                    End If
                                                End If
                                            End If
                                        Next lLocation
                                        'At this place output will have Null values for first constituent 
                                        'If all of this line is Null then lPendingOutput could be made zero and skipped to k = 123
                                        If Not lSomeLocationHasData Then
                                            Dim lSkipTo As String = Nothing
                                            Select Case lConstituentKey
                                                Case "NO3+NO2-N - SURFACE LAYER OUTFLOW" : lSkipTo = "NO3 (PQUAL)"
                                                Case "PO4-P IN SOLUTION - SURFACE LAYER - OUTFLOW" : lSkipTo = "Ortho P (PQUAL)"
                                                Case "SOQUAL-Ortho P" : lSkipTo = "Ortho P (IQUAL)"
                                                Case "SOQUAL-NO3" : lSkipTo = "NO3 (IQUAL)"
                                            End Select
                                            If lSkipTo IsNot Nothing Then
                                                'At this point the specific data for AGCHEM N constitunet
                                                'in HBN file is missing and code skips to PQUAL constituents                                                
                                                For lClearFieldIndex As Integer = 1 To .NumFields
                                                    .Value(lClearFieldIndex) = ""
                                                Next
                                                .CurrentRecord -= 1
                                                .Value(1) = ""
                                                .CurrentRecord -= 1

                                                Dim lSkipToIndex As Integer = lConstituentsToOutput.IndexOf(lSkipTo)
                                                If lSkipToIndex > lConstituentIndex Then
                                                    lConstituentIndex = lSkipToIndex - 1
                                                End If
                                            Else
                                                lSomeLocationHasData = True
                                            End If
                                        End If
                                        If lSomeLocationHasData Then
                                            If aBalanceType <> "Water" Then
                                                SumLoads(lLoadTotals, lConstituentName, lLoadUnit, 0, 0)
                                            End If

                                            If lOutletReport Then
                                                Dim lConstituentTotalKey As String = lOperationType.Substring(0, 1) & ":" & lConstituentKey
                                                If lOperationType <> "RCHRES" Then
                                                    lLandUseConstituentTotals.Increment(lConstituentTotalKey & "-" & lLandUse, lWeightAccum)
                                                    lConstituentTotals.Increment(lConstituentTotalKey, lWeightAccum)
                                                    lFieldIndex += 1
                                                    .Value(lFieldIndex) = DecimalAlign(lWeightAccum / lLandUseAreas.ItemByKey(lLandUse), aFieldWidth, aDecimalPlaces, aSignificantDigits)
                                                ElseIf Math.Abs(lValueOutlet) > 0.00001 Then
                                                    Dim lConstituentTotalKeyIndex As Integer = lConstituentTotals.IndexFromKey(lConstituentTotalKey)
                                                    lConstituentTotals.Increment(lConstituentTotalKey, lValueOutlet)
                                                    lValueOutlet = 0.0
                                                End If
                                            End If
                                            .CurrentRecord += 1
                                        End If

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
                                        .Value(1) = lConstituentName.PadRight(aFieldWidth)
                                        For lFieldIndex = 2 To lCurFieldValues.GetUpperBound(0)
                                            .Value(lFieldIndex) = DecimalAlign(lCurFieldValues(lFieldIndex), aFieldWidth, aDecimalPlaces, aSignificantDigits)
                                        Next
                                        .CurrentRecord += 1
                                    Else
                                        If lConstituentKey.StartsWith("Header") Then
                                            lPendingOutput &= vbCr
                                        End If
                                        lPendingOutput &= lConstituentName

                                        If Not lConstituentKey.StartsWith("Header") Then
                                            For lOperationIndex As Integer = 0 To lLandUseOperations.Count - 1
                                                lPendingOutput &= vbTab & DecimalAlign(0.0, aFieldWidth, aDecimalPlaces, aSignificantDigits)
                                            Next
                                            If lOutletReport And lOperationType <> "RCHRES" Then
                                                lPendingOutput &= vbTab & DecimalAlign(0.0, aFieldWidth, aDecimalPlaces, aSignificantDigits)
                                            End If
                                            lPendingOutput &= vbCr

                                        End If
                                    End If
                                End If
                            Next
                            If lOutputTable.NumFields > 0 Then
                                If aSegmentRows Then
                                    lReport.AppendLine(.ToStringPivoted)
                                Else
                                    lReport.AppendLine(.ToString)
                                End If
                            End If
                        End With
                        'need totals?
                        Dim lNeedTotal As Boolean = False
                        For Each lLoad As Load In lLoadTotals
                            If lLoad.Count > 1 Then
                                lNeedTotal = True
                                Exit For
                            End If
                        Next
                        If lNeedTotal Then
                            lReport.Body.Remove(lReport.Body.Length - 2, 2)
                            lReport.AppendLine(aBalanceType)
                            For Each lLoad As Load In lLoadTotals
                                If lLoad.Count > 1 Then
                                    Dim lStr As String = ""
                                    For lIndex As Integer = 0 To lLoad.Unit.GetUpperBound(0)
                                        lStr &= DecimalAlign(lLoad.Unit(lIndex), aFieldWidth, aDecimalPlaces, aSignificantDigits) & vbTab
                                    Next
                                    lReport.AppendLine(lLoad.Name.PadRight(12) & vbTab & lStr)
                                End If
                            Next
                            lReport.AppendLine()
                        End If
                    Catch lEx As Exception
                        Logger.Dbg(lEx.Message)
                    End Try
                End If
            Next lLandUse
        Next lOperationType

        'For Debug:
        'lLandUses.Keys -> lLandUses.ItemByKey(aKey).Keys -> R:69

        If lOutletReport Then 'watershed summary report at specified output
            If aOutletDetails Then
                Dim lDetailsReport As New atcReport.ReportText
                Try
                    lDetailsReport.AppendLine(aBalanceType & " Balance by Land Use Category for " & aScenario & " at " & aOutletLocation)
                    lDetailsReport.AppendLine(Header(aBalanceType, aScenario, aRunMade, aUci))
                    lDetailsReport.AppendLine()
                    For Each lOperationType As String In aOperationTypes
                        Dim lOutputTable As New atcTableDelimited
                        With lOutputTable
                            If Not lOperationType.StartsWith("R") Then
                                .Delimiter = vbTab
                                .Header = lOperationType & vbCrLf
                                .NumHeaderRows = 2
                                Dim lNumFields As Integer = 0
                                For Each lLandUse As String In lLandUses.Keys
                                    If lLandUse.StartsWith(lOperationType.Substring(0, 1)) Then
                                        lNumFields += 1
                                    End If
                                Next
                                .NumFields = lNumFields + 2

                                lFieldIndex = 1
                                .FieldLength(lFieldIndex) = 12
                                .FieldName(lFieldIndex) = "Land Use".PadRight(12)
                                For Each lLandUse As String In lLandUses.Keys
                                    If lLandUse.StartsWith(lOperationType.Substring(0, 1)) Then
                                        lFieldIndex += 1
                                        .FieldLength(lFieldIndex) = aFieldWidth
                                        .FieldName(lFieldIndex) = lLandUse.Substring(2).PadLeft(aFieldWidth)
                                    End If
                                Next
                                lFieldIndex += 1
                                .FieldLength(lFieldIndex) = aFieldWidth
                                .FieldName(lFieldIndex) = "WtdAvg".PadLeft(aFieldWidth)

                                .CurrentRecord = 1
                                .Value(1) = "Area".PadRight(aFieldWidth)
                                lFieldIndex = 1
                                Dim lAreaTotal As Double = 0.0
                                For Each lLandUse As String In lLandUses.Keys
                                    If lLandUse.StartsWith(lOperationType.Substring(0, 1)) Then
                                        Dim lArea As Double = lLandUseAreas.ItemByKey(lLandUse)
                                        lAreaTotal += lArea
                                        lFieldIndex += 1
                                        .Value(lFieldIndex) = DecimalAlign(lArea, aFieldWidth, aDecimalPlaces, 8)
                                    End If
                                Next
                                lFieldIndex += 1
                                .Value(lFieldIndex) = DecimalAlign(lAreaTotal, aFieldWidth, aDecimalPlaces, 8)

                                For Each lConstituentKey As String In lConstituentsToOutput.Keys
                                    Dim lConstituentName As String = lConstituentsToOutput.ItemByKey(lConstituentKey)
                                    If lConstituentKey.StartsWith(lOperationType.Substring(0, 1)) Then
                                        .CurrentRecord += 1
                                        If lConstituentKey.Substring(2).StartsWith("Header") Then
                                            .CurrentRecord += 1
                                            .Value(1) = lConstituentName.PadRight(12)
                                        Else
                                            .Value(1) = lConstituentName.PadRight(12)
                                            'fill in values for each land use
                                            Dim lValueTotal As Double = 0.0
                                            lFieldIndex = 1
                                            For Each lLandUse As String In lLandUses.Keys
                                                If lLandUse.StartsWith(lOperationType.Substring(0, 1)) Then
                                                    Dim lValue As Double = lLandUseConstituentTotals.ItemByKey(lConstituentKey & "-" & lLandUse)
                                                    lValueTotal += lValue
                                                    lFieldIndex += 1
                                                    .Value(lFieldIndex) = DecimalAlign(lValue / lLandUseAreas.ItemByKey(lLandUse), aFieldWidth, aDecimalPlaces, aSignificantDigits)
                                                End If
                                            Next
                                            lFieldIndex += 1
                                            .Value(lFieldIndex) = DecimalAlign(lValueTotal / lAreaTotal, aFieldWidth, aDecimalPlaces, aSignificantDigits)
                                        End If
                                    End If
                                Next lConstituentKey
                                If aSegmentRows Then
                                    lDetailsReport.AppendLine(.ToStringPivoted)
                                Else
                                    lDetailsReport.AppendLine(.ToString)
                                End If
                            End If
                        End With
                    Next
                    Dim lPivotString As String = ""
                    If aSegmentRows Then
                        lPivotString = "Pivot"
                    End If
                    Dim lDetailsFileName As String = aOutFilePrefix & SafeFilename(ShortConstituentName(aBalanceType) & "_" & aScenario & "_" & aOutletLocation & "_LU_AnnAvgs" & lPivotString & ".txt")
                    SaveFileString(lDetailsFileName, lDetailsReport.ToString)
                    lDetailsReport = Nothing
                Catch lEx As Exception
                    Logger.Dbg("Whoops " & lEx.Message)
                End Try
            End If

            ' simple report - PERLND, IMPLND, RCHRES summary
            Dim lSummaryReport As New atcReport.ReportText
            lSummaryReport.AppendLine(Left(aBalanceType, 3) & " Balance for Subbasin " & aOutletLocation)
            lSummaryReport.AppendLine(Header(aBalanceType, aScenario, aRunMade, aUci))
            lSummaryReport.AppendLine()
            lSummaryReport.AppendLine("Area Summary (acres)")
            Dim lTotalArea As Double = 0.0
            For Each lOperationType As String In lOperationTypeAreas.Keys
                Dim lArea As Double = lOperationTypeAreas.ItemByKey(lOperationType)
                lTotalArea += lArea
                lSummaryReport.AppendLine(("  " & lOperationType).PadRight(12) & DecimalAlign(lArea, , , 8))
            Next

            lSummaryReport.AppendLine()
            lSummaryReport.AppendLine("  RCHRES".PadRight(12) & DecimalAlign(lTotalArea, , , 8))

            Dim lRowIdLength As Integer = 20
            lSummaryReport.AppendLine()
            lSummaryReport.AppendLine(Space(lRowIdLength) & vbTab & "OverOperType".PadLeft(12) & _
                                                        vbTab & "Land".PadLeft(12) & _
                                                        vbTab & "OverAll".PadLeft(12))
            If aBalanceType = "Water" Then
                lSummaryReport.AppendLine(Space(lRowIdLength) & vbTab & "Inches".PadLeft(12) & _
                                                            vbTab & "Ac-Ft".PadLeft(12) & _
                                                            vbTab & "Inches".PadLeft(12))
            ElseIf aBalanceType = "Sediment" Then
                lSummaryReport.AppendLine(Space(lRowIdLength) & vbTab & "tons/ac".PadLeft(12) & _
                                                            vbTab & "tons".PadLeft(12) & _
                                                            vbTab & "tons/ac".PadLeft(12))
            ElseIf aBalanceType = "FColi" Then
                lSummaryReport.AppendLine(Space(lRowIdLength) & vbTab & "10^9/ac".PadLeft(12) & _
                                                            vbTab & "10^9".PadLeft(12) & _
                                                            vbTab & "10^9/ac".PadLeft(12))
            Else
                lSummaryReport.AppendLine(Space(lRowIdLength) & vbTab & "lbs/ac".PadLeft(12) & _
                                                            vbTab & "lbs".PadLeft(12) & _
                                                            vbTab & "lbs/ac".PadLeft(12))
            End If

            For Each lOperationType As String In aOperationTypes
                Dim lNeedHeader As String = True
                Dim lOperationTypeArea As Double = lOperationTypeAreas.ItemByKey(lOperationType)
                Dim lLoadTotals As New Loads
                For Each lConstituentKey As String In lConstituentsToOutput.Keys
                    If lConstituentKey.StartsWith(lOperationType.Substring(0, 1)) Then
                        If lNeedHeader Then
                            lSummaryReport.AppendLine()
                            If lOperationType = "RCHRES" Then
                                lSummaryReport.AppendLine(Space(lRowIdLength) & vbTab & "Reach".PadLeft(12) & _
                                                                            vbTab & "Outlets".PadLeft(12))
                                If aBalanceType = "Water" Then
                                    lSummaryReport.AppendLine(Space(lRowIdLength) & vbTab & "Inches".PadLeft(12) & _
                                                                                vbTab & "Ac-Ft".PadLeft(12))
                                ElseIf aBalanceType = "Sediment" Then
                                    lSummaryReport.AppendLine(Space(lRowIdLength) & vbTab & "tons".PadLeft(12) & _
                                                                                vbTab & "tons/ac".PadLeft(12))
                                ElseIf aBalanceType = "FColi" Then
                                    lSummaryReport.AppendLine(Space(lRowIdLength) & vbTab & "10^9/ac".PadLeft(12) & _
                                                                                vbTab & "10^9".PadLeft(12))
                                Else
                                    lSummaryReport.AppendLine(Space(lRowIdLength) & vbTab & "lbs/ac".PadLeft(12) & _
                                                                                vbTab & "lbs".PadLeft(12))
                                End If
                                lSummaryReport.AppendLine("RCHRES") ' & vbTab & vbTab & "Area" & vbTab & DecimalAlign(lTotalArea))
                            Else
                                lSummaryReport.AppendLine(lOperationType) '& vbTab & vbTab & "Area" & vbTab & DecimalAlign(lOperationTypeArea))
                            End If
                            lNeedHeader = False
                        End If
                        Dim lConstituentName As String = lConstituentsToOutput.ItemByKey(lConstituentKey)
                        If lConstituentName.Length > lRowIdLength Then
                            lConstituentName = lConstituentName.Substring(0, lRowIdLength)
                        End If
                        Dim lConstituentTotalIndex As Integer = lConstituentTotals.IndexFromKey(lConstituentKey)
                        If lConstituentTotalIndex >= 0 Then
                            Dim lValue As Double = lConstituentTotals.Item(lConstituentTotalIndex)
                            Dim lUnitsAdjust As Double = 1.0
                            If aBalanceType = "Water" Then
                                lUnitsAdjust = 12
                            End If
                            If Math.Abs(lValue) > 0.00001 OrElse Not aSkipZeroOrNoValue Then
                                If lOperationType = "RCHRES" Then
                                    lSummaryReport.Append(lConstituentName.PadRight(lRowIdLength) & vbTab & _
                                               DecimalAlign(lValue / lTotalArea) & vbTab & _
                                               DecimalAlign(lValue / lUnitsAdjust))
                                Else
                                    Dim lLoadUnit(0) As Double
                                    lLoadUnit(0) = lValue / lOperationTypeArea
                                    Dim lLoadTotal As Double = lValue / lUnitsAdjust
                                    Dim lLoadOverall As Double = lValue / lTotalArea
                                    lSummaryReport.Append(lConstituentName.PadRight(lRowIdLength) & vbTab & _
                                               DecimalAlign(lLoadUnit(0)) & vbTab & _
                                               DecimalAlign(lLoadTotal) & vbTab & _
                                               DecimalAlign(lLoadOverall))
                                    If aBalanceType <> "Water" Then
                                        SumLoads(lLoadTotals, lConstituentName, lLoadUnit, lLoadTotal, lLoadOverall)
                                    End If
                                End If
                                lSummaryReport.AppendLine()
                            Else
                                'Logger.Dbg("SkipNoData:" & lConstituentKey)
                            End If
                        ElseIf Not lConstituentKey.Substring(2).StartsWith("Header") Then
                            lSummaryReport.AppendLine(lConstituentName.PadRight(lRowIdLength) & vbTab & _
                                               DecimalAlign(0.0) & vbTab & _
                                               DecimalAlign(0.0))
                        Else
                            lSummaryReport.AppendLine()
                            lSummaryReport.AppendLine(lConstituentName.PadRight(lRowIdLength))
                        End If
                    End If
                Next
                'need totals?
                Dim lNeedTotal As Boolean = False
                For Each lLoad As Load In lLoadTotals
                    If lLoad.Count > 1 Then
                        lNeedTotal = True
                        Exit For
                    End If
                Next
                If lNeedTotal Then
                    lSummaryReport.AppendLine()
                    lSummaryReport.AppendLine(aBalanceType)
                    For Each lLoad As Load In lLoadTotals
                        If lLoad.Count > 1 Then
                            lSummaryReport.AppendLine(lLoad.Name.PadRight(lRowIdLength) & vbTab & _
                                                  DecimalAlign(lLoad.Unit(0)) & vbTab & _
                                                  DecimalAlign(lLoad.Total) & vbTab & _
                                                  DecimalAlign(lLoad.Overall))
                        End If
                    Next
                End If
            Next lOperationType
            Dim lSummaryFileName As String = aOutFilePrefix & SafeFilename(ShortConstituentName(aBalanceType) & "_" & aScenario & "_" & aOutletLocation & "_Oall_Avgd.txt")
            SaveFileString(lSummaryFileName, lSummaryReport.ToString)
            lSummaryReport = Nothing
        End If
        Return lReport
    End Function

    Private Class Loads
        Inherits KeyedCollection(Of String, Load)
        Protected Overrides Function GetKeyForItem(ByVal aLoad As Load) As String
            Return aLoad.Name
        End Function
    End Class
    Private Class Load
        Friend Name As String
        Friend Count As Integer
        Friend Unit() As Double
        Friend Total As Double
        Friend Overall As Double
    End Class

    Private Sub SumLoads(ByVal aLoadTotals As Loads, _
                         ByVal aConstituentName As String, _
                         ByVal aLoadUnit() As Double, _
                         ByVal aLoadTotal As Double, _
                         ByVal aLoadOverall As Double)
        If aLoadTotals.Contains(aConstituentName) Then
            With aLoadTotals.Item(aConstituentName)
                ReDim Preserve .Unit(aLoadUnit.GetUpperBound(0))
                For lIndex As Integer = 0 To aLoadUnit.GetUpperBound(0)
                    .Unit(lIndex) += aLoadUnit(lIndex)
                Next
                .Total += aLoadTotal
                .Overall += aLoadOverall
                .Count += 1
            End With
        Else
            Dim lLoad As New Load
            With lLoad
                .Name = aConstituentName
                ReDim .Unit(aLoadUnit.GetUpperBound(0))
                For lIndex As Integer = 0 To aLoadUnit.GetUpperBound(0)
                    .Unit = aLoadUnit
                Next
                .Total = aLoadTotal
                .Overall = aLoadOverall
                .Count = 1
            End With
            aLoadTotals.Add(lLoad)
        End If
    End Sub

    Private Function Header(ByVal aBalanceType As String, ByVal aScenario As String, ByVal aRunMade As Date, ByVal auci As atcUCI.HspfUci) As String
        Dim lString As String = "   Run Made " & aRunMade & vbCrLf
        lString &= "   " & auci.GlobalBlock.RunInf.Value & vbCrLf
        lString &= "   " & auci.GlobalBlock.RunPeriod
        Return lString
    End Function
End Module
