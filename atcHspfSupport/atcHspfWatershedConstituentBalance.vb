Imports atcUtility
Imports atcData
Imports MapWinUtility
Imports System.Collections.ObjectModel
Imports atcUCI

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
    Public Sub ReportsToFiles(ByVal aUci As atcUCI.HspfUci,
                              ByVal aBalanceType As String,
                              ByVal aOperationTypes As atcCollection,
                              ByVal aScenario As String,
                              ByVal aScenarioResults As atcDataSource,
                              ByVal aOutletLocations As atcCollection,
                              ByVal aRunMade As String,
                              ByVal aSDateJ As String,
                              ByVal aEDateJ As String,
                              ByVal aConstProperties As List(Of ConstituentProperties),
                     Optional ByVal aOutFilePrefix As String = "",
                     Optional ByVal aOutletDetails As Boolean = False,
                     Optional ByVal aSegmentRows As Boolean = False,
                     Optional ByVal aDecimalPlaces As Integer = 3,
                     Optional ByVal aSignificantDigits As Integer = 5,
                     Optional ByVal aFieldWidth As Integer = 12,
                     Optional ByVal aSkipZeroOrNoValue As Boolean = True)



        For Each lOutletLocation As String In aOutletLocations
            Dim lReport As atcReport.ReportText = Report(aUci, aBalanceType,
                                               aOperationTypes,
                                               aScenario, aScenarioResults,
                                               aRunMade, aSDateJ, aEDateJ, aConstProperties,
                                                         lOutletLocation,
                                               aOutFilePrefix, True,
                                               aSegmentRows, aDecimalPlaces, aSignificantDigits, aFieldWidth, aSkipZeroOrNoValue)

            Dim lPivotString As String = ""
            If aSegmentRows Then
                lPivotString = "Pivot"
            End If
            Dim lOutFileName As String = aOutFilePrefix & SafeFilename(ShortConstituentName(aBalanceType) & "_" & aScenario & "_" & lOutletLocation & "_Grp_By_OPN_LU_Ann_Avg" & lPivotString & ".txt")

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
                        If (lFields.GetUpperBound(0) = 3 OrElse
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
                            ElseIf aBalanceType = "TN" Then
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
                lReport.AppendLine(Header(aBalanceType, aScenario, aRunMade, aUci, aSDateJ, aEDateJ))
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
            Case "BOD-Labile"
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
    Public Function Report(ByVal aUci As atcUCI.HspfUci,
                           ByVal aBalanceType As String,
                           ByVal aOperationTypes As atcCollection,
                           ByVal aScenario As String,
                           ByVal aScenarioResults As atcDataSource,
                           ByVal aRunMade As String,
                           ByVal aSDateJ As Double,
                           ByVal aEDateJ As Double,
                           ByVal aConstProperties As List(Of ConstituentProperties),
                  Optional ByVal aOutletLocation As String = "",
                  Optional ByVal aOutFilePrefix As String = "",
                  Optional ByVal aOutletDetails As Boolean = False,
                  Optional ByVal aSegmentRows As Boolean = False,
                  Optional ByVal aDecimalPlaces As Integer = 3,
                  Optional ByVal aSignificantDigits As Integer = 5,
                  Optional ByVal aFieldWidth As Integer = 12,
                  Optional ByVal aSkipZeroOrNoValue As Boolean = True) As atcReport.IReport

        Dim lOutletReport As Boolean = False
        If aOutletLocation.Length > 0 Then
            lOutletReport = True
        End If

        Dim lUnits As String = GQualUnits(aUci, aBalanceType)   'if not a gqual, will return a blank string
        Dim lConstituentsToOutput As atcCollection = ConstituentsToOutput(aBalanceType, aConstProperties, , lUnits)
        Logger.Dbg("ConstituentCount:" & lConstituentsToOutput.Count)
        Dim lConstituentTotals As New atcCollection
        Dim lConstituentLandUseTotals As New atcCollection

        Dim lLandUses As atcCollection = LandUses(aUci, aOperationTypes, aOutletLocation)
        Logger.Dbg("LandUseCount:" & lLandUses.Count)

        Dim lReport As New atcReport.ReportText
        lReport.AppendLine(aScenario & " Annual Average Watershed Balance Report of " & aBalanceType & " For Each PERLND, IMPLND, and RCHRES.")
        lReport.AppendLine(Header(aBalanceType, aScenario, aRunMade, aUci, aSDateJ, aEDateJ))
        If aBalanceType = "Water" Then
            If aUci.GlobalBlock.EmFg = 1 Then
                lReport.AppendLine("   (Units:Inches)")
            Else
                lReport.AppendLine("   (Units:mm)")
            End If
        End If
        lReport.AppendLine()
        lReport.AppendLine()


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
                                Dim lConstituentDataGroup As New atcTimeseriesGroup
                                'For Each lConstituentKey In lConstituentsToOutput.Keys
                                lConstituentKey = lConstituentsToOutput.Keys(lConstituentIndex)
                                If lConstituentKey.StartsWith(lOperationType.Substring(0, 1)) Then
                                    Dim lConstituentName As String = lConstituentsToOutput.ItemByKey(lConstituentKey)
                                    Dim lConstituentDataName As String = lConstituentKey
                                    Dim lMultipleIndex As Integer = 0
                                    If Not lConstituentKey.ToLower.Contains("header") Then ' AndAlso Not lConstituentKey.ToLower.Contains("total") Then
                                        'Anurag commented out latter part of this condition as it was causing problem in ORGN - TOTAL OUTFLOW
                                        If lConstituentDataName.EndsWith("1") Or lConstituentDataName.EndsWith("2") Then
                                            lMultipleIndex = lConstituentDataName.Substring(lConstituentDataName.Length - 1)
                                            lConstituentDataName = lConstituentDataName.Substring(0, lConstituentDataName.Length - 1)
                                        End If
                                    End If
                                    lConstituentKey = lConstituentKey.Remove(0, 2)
                                    lConstituentDataName = lConstituentDataName.Remove(0, 2)
                                    lConstituentDataGroup.Add(aScenarioResults.DataSets.FindData("Constituent", lConstituentDataName))
                                    Dim lSomeConstituentsHaveData As Boolean = False
                                    Dim lWeightAccum As Double = 0.0
                                    If lConstituentDataGroup.Count > 0 Then
                                        lSomeConstituentsHaveData = True
                                        If lNeedHeader Then
                                            .Delimiter = vbTab

                                            Select Case aBalanceType & "_" & lOperationType
                                                Case "Water_PERLND", "Water_IMPLND"
                                                    .Header = aBalanceType & " Balance Report For " & lLandUse & "  (Inches)" & vbCrLf
                                                Case "Water_RCHRES"
                                                    .Header = aBalanceType & " Balance Report For " & lLandUse & "  (ac-ft)" & vbCrLf
                                                Case "Sediment_PERLND", "Sediment_IMPLND"
                                                    .Header = aBalanceType & " Balance Report For " & lLandUse & "  (tons/ac)" & vbCrLf
                                                Case "Sediment_RCHRES"
                                                    .Header = aBalanceType & " Balance Report For " & lLandUse & "  (tons)" & vbCrLf
                                                Case "TN_PERLND", "TN_IMPLND", "TP_PERLND", "TP_IMPLND", "BOD-Labile_PERLND", "BOD-Labile_IMPLND"
                                                    .Header = aBalanceType & " Balance Report For " & lLandUse & "  (lbs/ac)" & vbCrLf
                                                Case "TN_RCHRES", "TP_RCHRES", "BOD-Labile_RCHRES"
                                                    .Header = aBalanceType & " Balance Report For " & lLandUse & "  (lbs)" & vbCrLf
                                                Case Else
                                                    Dim lPrefix As String = ""
                                                    If aBalanceType.ToUpper.Contains("F.COLIFORM") Or aBalanceType.ToUpper.StartsWith("FCOLI") Or aBalanceType.ToUpper.StartsWith("BACT") Then 'Assuming this is f.coli or bacteria
                                                        lPrefix = "10^9 "
                                                    End If
                                                    If lOperationType = "PERLND" Or lOperationType = "IMPLND" Then
                                                        .Header = aBalanceType & " Balance Report For " & lLandUse & "  (" & lPrefix & lUnits & "/ac)" & vbCrLf
                                                    ElseIf lOperationType = "RCHRES" Then
                                                        .Header = aBalanceType & " Balance Report For " & lLandUse & "  (" & lPrefix & lUnits & ")" & vbCrLf
                                                    End If
                                            End Select

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

                                        Dim lValue As Double = 0.0
                                        lFieldIndex = 1
                                        Dim lSomeLocationHasData As Boolean = False


                                        For Each lLocation As String In lLandUseOperations.Keys
                                            'lLocation e.g. R:69
                                            Dim lLocationDataGroup As atcTimeseriesGroup = lConstituentDataGroup.FindData("Location", lLocation)
                                            If lLocationDataGroup.Count > 0 Then
                                                lSomeLocationHasData = True
                                                lTempDataSet = lLocationDataGroup.Item(0)
                                                Dim lOperation As HspfOperation = aUci.OpnBlks(lOperationType).OperFromID(lLocation.Substring(2))
                                                Dim lMult As Double = 1.0
                                                Dim lMassLinkID As Integer = 0
                                                Dim lMassLinkFactor As Double = 0
                                                Dim lTotalLoad As Double = 0.0
                                                Dim lTotalArea As Double = 0.0
                                                Dim MassLinkExists As Boolean = True

                                                If ConstituentsThatNeedMassLink.Contains(lConstituentDataName.ToUpper) Then
                                                    For Each lConnection As HspfConnection In lOperation.Targets

                                                        If lConnection.Target.VolName = "RCHRES" Then
                                                            Dim aReach As HspfOperation = aUci.OpnBlks("RCHRES").OperFromID(lConnection.Target.VolId)
                                                            Dim aConversionFactor As Double = 0.0
                                                            If aBalanceType = "TN" Or aBalanceType = "TP" Then
                                                                aConversionFactor = ConversionFactorfromOxygen(aUci, aBalanceType, aReach)
                                                            End If
                                                            lMassLinkID = lConnection.MassLink

                                                            If Not lMassLinkID = 0 Then
                                                                Dim ConstNameMassLink As String = lConstituentDataName
                                                                If Not (aConstProperties.Count = 0 OrElse (lConnection.Source.Opn.Name = "PERLND" AndAlso
                                                                        lConnection.Source.Opn.Tables("ACTIVITY").Parms("PQALFG").Value = "0")) Then
                                                                    ConstNameMassLink = Split(lConstituentDataName.ToUpper, "-", 2)(1)
                                                                    Dim ConstNameEXP As String = ""
                                                                    For Each constt As ConstituentProperties In aConstProperties
                                                                        If constt.ConstituentNameInUCI = ConstNameMassLink Then
                                                                            ConstNameEXP = constt.ConstNameForEXPPlus
                                                                            If ConstNameEXP = "TAM" Then ConstNameEXP = "NH3+NH4"
                                                                            ConstNameMassLink = Split(lConstituentDataName.ToUpper, "-", 2)(0) & "-" & ConstNameEXP
                                                                        End If
                                                                    Next
                                                                End If


                                                                lMassLinkFactor = FindMassLinkFactor(aUci, lMassLinkID, ConstNameMassLink, aBalanceType,
                                                                                               aConversionFactor, lMultipleIndex)
                                                            Else
                                                                MassLinkExists = False
                                                            End If
                                                            Dim lArea As Double = lConnection.MFact
                                                            If lArea = 0 Then
                                                                lArea = 0.0000000001
                                                            End If
                                                            If aBalanceType = "Water" Then 'Water is simulated in inches and when it goes to RCHRES, it goes as feet. 
                                                                'This factor takes care of that conversion that happened in MASS-LINK
                                                                lMassLinkFactor *= 12
                                                            End If
                                                            lTotalLoad += lArea * lMassLinkFactor
                                                            lTotalArea += lArea
                                                        End If

                                                    Next

                                                    lMult = lTotalLoad / lTotalArea


                                                End If


                                                If aBalanceType.ToUpper.Contains("F.COLIFORM") Or aBalanceType.ToUpper.StartsWith("FCOLI") Or aBalanceType.ToUpper.StartsWith("BACT") Then 'Assuming this is f.coli or bacteria
                                                    lMult = 1 / 1000000000.0 '10^9
                                                End If

                                                Dim lAttribute As atcDefinedValue = Nothing
                                                If ConstituentsThatUseLast.Contains(lConstituentDataName) Then
                                                    'lAttribute = lTempDataSet.Attributes.GetDefinedValue("Last")

                                                    'Dim lTempDataSet As atcDataSet = lConstituentDataGroup.Item(0)
                                                    Dim lSeasons As atcSeasonBase
                                                    If aUci.GlobalBlock.SDate(1) = 10 Then 'month Oct
                                                        lSeasons = New atcSeasonsWaterYear
                                                    Else
                                                        lSeasons = New atcSeasonsCalendarYear
                                                    End If
                                                    Dim lSeasonalAttributes As New atcDataAttributes

                                                    lSeasonalAttributes.SetValue("Last", 0)

                                                    Dim ltest As Double = 0
                                                    Dim lYearlyAttributes As New atcDataAttributes
                                                    lSeasons.SetSeasonalAttributes(lTempDataSet, lSeasonalAttributes, lYearlyAttributes)
                                                    For Each lseasonalAttribute As atcDefinedValue In lYearlyAttributes
                                                        ltest += lseasonalAttribute.Value
                                                    Next
                                                    lSeasonalAttributes(0).Value = ltest / lYearlyAttributes.Count
                                                    lAttribute = lSeasonalAttributes(0)
                                                    'Mark added this option to calculate the average of end of the year values for state variables.
                                                    'Earlier we were getting end of the simulation period value.
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
                                            Dim lSkipTo As String = FindSkipTo(lConstituentKey)
                                            If lSkipTo IsNot Nothing Then
                                                'At this point the specific data for AGCHEM N constitunet
                                                'in HBN file is missing and code skips to PQUAL constituents                                                
                                                For lClearFieldIndex As Integer = 1 To .NumFields
                                                    .Value(lClearFieldIndex) = ""
                                                Next
                                                .CurrentRecord -= 1
                                                .Value(1) = ""
                                                .CurrentRecord -= 1

                                                Dim lSkipToindex2 As Integer = 2000

                                                If lSkipTo.StartsWith("NO3+NO2") Then
                                                    lSkipTo = "NO3+NO2 (PQUAL)"
                                                    Dim lSkip2 As String = "NH3+NH4 (PQUAL)"
                                                    lSkipToindex2 = lConstituentsToOutput.IndexOf(lSkip2)
                                                End If
                                                Dim lSkipToindex As Integer = lConstituentsToOutput.IndexOf(lSkipTo)

                                                If lSkipToindex > lSkipToindex2 Then lSkipToindex = lSkipToindex2
                                                If lSkipToindex > lConstituentIndex Then
                                                    lConstituentIndex = lSkipToindex - 1
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


                                    ElseIf lConstituentKey.StartsWith("Total") AndAlso
                                         lConstituentKey.Length > 5 AndAlso
                                         IsNumeric(lConstituentKey.Substring(5, 1)) Then
                                        Dim lTotalCount As Integer = lConstituentKey.Substring(5, 1)
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
                                        'If lOutletReport Then
                                        '    Dim lConstituentTotalKey As String = lOperationType.Substring(0, 1) & ":" & lConstituentKey
                                        '    If lOperationType <> "RCHRES" Then
                                        '        lLandUseConstituentTotals.Increment(lConstituentTotalKey & "-" & lLandUse, lWeightAccum)
                                        '        lConstituentTotals.Increment(lConstituentTotalKey, lWeightAccum)
                                        '        lFieldIndex += 1
                                        '        .Value(lFieldIndex) = DecimalAlign(lWeightAccum / lLandUseAreas.ItemByKey(lLandUse), aFieldWidth, aDecimalPlaces, aSignificantDigits)
                                        '    ElseIf Math.Abs(lValueOutlet) > 0.00001 Then
                                        '        Dim lConstituentTotalKeyIndex As Integer = lConstituentTotals.IndexFromKey(lConstituentTotalKey)
                                        '        lConstituentTotals.Increment(lConstituentTotalKey, lValueOutlet)
                                        '        lValueOutlet = 0.0
                                        '    End If
                                        'End If
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
                                        If Not lSomeConstituentsHaveData Then
                                            Dim lSkipTo As String = FindSkipTo(lConstituentKey)
                                            If lSkipTo IsNot Nothing Then
                                                'At this point the specific data for AGCHEM N constitunet
                                                'in HBN file is missing and code skips to PQUAL constituents                                                
                                                For lClearFieldIndex As Integer = 1 To .NumFields
                                                    .Value(lClearFieldIndex) = ""
                                                Next
                                                '.CurrentRecord -= 1
                                                '.Value(1) = ""
                                                '.CurrentRecord -= 1

                                                Dim lSkipToindex2 As Integer = 2000

                                                If lSkipTo.StartsWith("NO3+NO2") Then
                                                    lSkipTo = "NO3+NO2 (PQUAL)"
                                                    Dim lSkip2 As String = "NH3+NH4 (PQUAL)"
                                                    lSkipToindex2 = lConstituentsToOutput.IndexOf(lSkip2)
                                                End If
                                                Dim lSkipToindex As Integer = lConstituentsToOutput.IndexOf(lSkipTo)

                                                If lSkipToindex > lSkipToindex2 Then lSkipToindex = lSkipToindex2
                                                If lSkipToindex > lConstituentIndex Then
                                                    lConstituentIndex = lSkipToindex - 1
                                                End If
                                                lPendingOutput = ""
                                            End If
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
                        If lLoadTotals.Count < 6 Then
                            For Each lLoad As Load In lLoadTotals
                                If lLoad.Count > 1 Then
                                    'Anurag added this condition on 2/22/2014, so that we do not get sum values for AGCHEM Constituents, as we were 
                                    'seeing junk values for AGCHEM constituents. There might be a more elegant way to do it, but this 
                                    'might for now.
                                    lNeedTotal = True
                                    Exit For
                                End If
                            Next
                        End If
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
                        Else
                            lReport.Body.Remove(lReport.Body.Length - 5, 5)
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
                    lDetailsReport.AppendLine(Header(aBalanceType, aScenario, aRunMade, aUci, aSDateJ, aEDateJ))
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

                                For lConstituentIndex As Integer = 0 To lConstituentsToOutput.Count - 1 'Anurag Changed the way For loop works to enable easy skipping when there are no AgCHEM constituents. 
                                    'Basically if the HBN file has no data for ("P:NO3+NO2-N - SURFACE LAYER OUTFLOW") or 
                                    '("P:PO4-P IN SOLUTION - SURFACE LAYER - OUTFLOW") the code assumes that there are no AGCHEM constituents for that 
                                    'specific PERLND and looks for the PQUAL constituents
                                    'For Each lConstituentKey In lConstituentsToOutput.Keys
                                    Dim lConstituentKey As String = lConstituentsToOutput.Keys(lConstituentIndex)

                                    Dim lConstituentName As String = lConstituentsToOutput.ItemByKey(lConstituentKey)
                                    If lConstituentKey.StartsWith(lOperationType.Substring(0, 1)) Then
                                        .CurrentRecord += 1
                                        If lConstituentKey.Substring(2).StartsWith("Header") Then
                                            .CurrentRecord += 1
                                            .Value(1) = lConstituentName.PadRight(12)

                                        ElseIf lConstituentKey.Substring(2).StartsWith("Total") AndAlso
                                                lConstituentKey.Substring(2).Length > 5 AndAlso
                                                IsNumeric(SafeSubstring(lConstituentKey, 7, 1)) Then
                                            Dim lTotalCount As Integer = lConstituentKey.Substring(7, 1)
                                            Dim lCurFieldValues(.NumFields) As Double
                                            Dim lCurrentRecordSave As Integer = .CurrentRecord
                                            For lCount As Integer = 1 To lTotalCount
                                                .CurrentRecord -= 1
                                                For lFieldPos As Integer = 2 To lCurFieldValues.GetUpperBound(0)
                                                    If IsNumeric(.Value(lFieldPos)) Then
                                                        lCurFieldValues(lFieldPos) += .Value(lFieldPos)
                                                    Else
                                                        Logger.Dbg("Why")
                                                    End If
                                                Next
                                            Next
                                            .CurrentRecord += lTotalCount

                                            .Value(1) = lConstituentName.PadRight(aFieldWidth)
                                            For lFieldPos As Integer = 2 To lCurFieldValues.GetUpperBound(0)
                                                .Value(lFieldPos) = DecimalAlign(lCurFieldValues(lFieldPos), aFieldWidth, aDecimalPlaces, aSignificantDigits)
                                            Next
                                        Else
                                            '.CurrentRecord += 1

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
                                            If lValueTotal = 0 Then
                                                Dim lSkipTo As String = FindSkipTo(lConstituentKey)
                                                If lSkipTo IsNot Nothing Then
                                                    'At this point the specific data for AGCHEM N constitunet
                                                    'in HBN file is missing and code skips to PQUAL constituents                                                
                                                    For lClearFieldIndex As Integer = 1 To .NumFields
                                                        .Value(lClearFieldIndex) = ""
                                                    Next
                                                    .CurrentRecord -= 1
                                                    .Value(1) = ""
                                                    .CurrentRecord -= 1

                                                    Dim lSkipToindex2 As Integer = 2000

                                                    If lSkipTo.StartsWith("NO3+NO2") Then
                                                        lSkipTo = "NO3+NO2 (PQUAL)"
                                                        Dim lSkip2 As String = "NH3+NH4 (PQUAL)"
                                                        lSkipToindex2 = lConstituentsToOutput.IndexOf(lSkip2)
                                                    End If
                                                    Dim lSkipToindex As Integer = lConstituentsToOutput.IndexOf(lSkipTo)

                                                    If lSkipToindex > lSkipToindex2 Then lSkipToindex = lSkipToindex2
                                                    If lSkipToindex > lConstituentIndex Then
                                                        lConstituentIndex = lSkipToindex - 1
                                                    End If
                                                    lPendingOutput = ""
                                                End If
                                            End If

                                        End If
                                        '.CurrentRecord += 1

                                    End If


                                Next
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
            lSummaryReport.AppendLine(Header(aBalanceType, aScenario, aRunMade, aUci, aSDateJ, aEDateJ))
            lSummaryReport.AppendLine()
            lSummaryReport.AppendLine("Area Summary (acres)")
            Dim lTotalArea As Double = 0.0
            For Each lOperationType As String In lOperationTypeAreas.Keys
                Dim lArea As Double = lOperationTypeAreas.ItemByKey(lOperationType)
                lTotalArea += lArea
                lSummaryReport.AppendLine(("  " & lOperationType).PadRight(12) & vbTab & DecimalAlign(lArea, , , 8))
            Next

            lSummaryReport.AppendLine()
            lSummaryReport.AppendLine("  RCHRES".PadRight(12) & vbTab & DecimalAlign(lTotalArea, , , 8))

            Dim lRowIdLength As Integer = 20
            lSummaryReport.AppendLine()
            lSummaryReport.AppendLine(Space(lRowIdLength) & vbTab & "OverOperType".PadLeft(12) &
                                                        vbTab & "Land".PadLeft(12) &
                                                        vbTab & "OverAll".PadLeft(12))
            If aBalanceType = "Water" Then
                lSummaryReport.AppendLine(Space(lRowIdLength) & vbTab & "Inches".PadLeft(12) &
                                                            vbTab & "Ac-Ft".PadLeft(12) &
                                                            vbTab & "Inches".PadLeft(12))
            ElseIf aBalanceType = "Sediment" Then
                lSummaryReport.AppendLine(Space(lRowIdLength) & vbTab & "tons/ac".PadLeft(12) &
                                                            vbTab & "tons".PadLeft(12) &
                                                            vbTab & "tons/ac".PadLeft(12))
            ElseIf aBalanceType.ToUpper.Contains("F.COLIFORM") Or aBalanceType.ToUpper.StartsWith("FCOLI") Or aBalanceType.ToUpper.StartsWith("BACT") Then 'Assuming this is f.coli or bacteria
                lSummaryReport.AppendLine(Space(lRowIdLength) & vbTab & "10^9/ac".PadLeft(12) &
                                                            vbTab & "10^9".PadLeft(12) &
                                                            vbTab & "10^9/ac".PadLeft(12))
            Else
                lSummaryReport.AppendLine(Space(lRowIdLength) & vbTab & "lbs/ac".PadLeft(12) &
                                                            vbTab & "lbs".PadLeft(12) &
                                                            vbTab & "lbs/ac".PadLeft(12))
            End If
            Dim lSubtotals(3) As Double
            For Each lOperationType As String In aOperationTypes
                Dim lNeedHeader As String = True
                Dim lOperationTypeArea As Double = lOperationTypeAreas.ItemByKey(lOperationType)
                Dim lLoadTotals As New Loads
                Dim lHeaderStart As Integer
                For lConstituentIndex As Integer = 0 To lConstituentsToOutput.Count - 1 'Anurag Changed the way For loop works to enable easy skipping when there are no AgCHEM constituents. Basically if the HBN file has no data for ("P:NO3+NO2-N - SURFACE LAYER OUTFLOW") or ("P:PO4-P IN SOLUTION - SURFACE LAYER - OUTFLOW") the code assumes that there are no AGCHEM constituents for that specific PERLND and looks for the PQUAL constituents
                    'For Each lConstituentKey In lConstituentsToOutput.Keys
                    Dim lConstituentKey As String = lConstituentsToOutput.Keys(lConstituentIndex)
                    Dim lOutputTable As New atcTableDelimited

                    If lConstituentKey.StartsWith(lOperationType.Substring(0, 1)) Then
                        If lNeedHeader Then
                            lHeaderStart = lSummaryReport.Body.Length
                            lSummaryReport.AppendLine()
                            If lOperationType = "RCHRES" Then
                                lSummaryReport.AppendLine(Space(lRowIdLength) & vbTab & "Reach".PadLeft(12) &
                                                                            vbTab & "Outlets".PadLeft(12))
                                If aBalanceType = "Water" Then
                                    lSummaryReport.AppendLine(Space(lRowIdLength) & vbTab & "Inches".PadLeft(12) &
                                                                                vbTab & "Ac-Ft".PadLeft(12))
                                ElseIf aBalanceType = "Sediment" Then
                                    lSummaryReport.AppendLine(Space(lRowIdLength) & vbTab & "tons".PadLeft(12) &
                                                                                vbTab & "tons/ac".PadLeft(12))
                                ElseIf aBalanceType.ToUpper.Contains("F.COLIFORM") Or aBalanceType.ToUpper.StartsWith("FCOLI") Or aBalanceType.ToUpper.StartsWith("BACT") Then 'Assuming this is f.coli or bacteria
                                    lSummaryReport.AppendLine(Space(lRowIdLength) & vbTab & "10^9/ac".PadLeft(12) &
                                                                                vbTab & "10^9".PadLeft(12))
                                Else
                                    lSummaryReport.AppendLine(Space(lRowIdLength) & vbTab & "lbs/ac".PadLeft(12) &
                                                                                vbTab & "lbs".PadLeft(12))
                                End If
                                lSummaryReport.AppendLine("RCHRES") ' & vbTab & vbTab & "Area" & vbTab & DecimalAlign(lTotalArea))
                            Else
                                lSummaryReport.AppendLine(lOperationType) '& vbTab & vbTab & "Area" & vbTab & DecimalAlign(lOperationTypeArea))
                            End If
                            lNeedHeader = False
                        End If

                        'With lOutputTable
                        Dim lNumFields As Integer = 3
                        lFieldIndex = 1
                        Dim lConstituentName As String = lConstituentsToOutput.ItemByKey(lConstituentKey)

                        If lConstituentName.Length > lRowIdLength Then
                            lConstituentName = lConstituentName.Substring(0, lRowIdLength)
                        End If
                        Dim lConstituentTotalIndex As Integer = lConstituentTotals.IndexFromKey(lConstituentKey)

                        '.FieldLength(lFieldIndex) = 20
                        '.FieldName(lFieldIndex) = lConstituentName.PadLeft(.FieldLength(lFieldIndex))
                        '.CurrentRecord() = 1
                        If lConstituentTotalIndex >= 0 Then
                            Dim lValue As Double = lConstituentTotals.Item(lConstituentTotalIndex)
                            Dim lUnitsAdjust As Double = 1.0
                            If aBalanceType = "Water" Then
                                lUnitsAdjust = 12
                                aSkipZeroOrNoValue = False
                            End If
                            If Math.Abs(lValue) > 0.00001 OrElse Not aSkipZeroOrNoValue Then
                                If lOperationType = "RCHRES" Then
                                    lFieldIndex += 1
                                    '.Value(1) = DecimalAlign(lValue / lTotalArea)
                                    '.Value(2) = DecimalAlign(lValue / lUnitsAdjust)


                                    lSummaryReport.Append(lConstituentName.PadRight(lRowIdLength) & vbTab &
                                               DecimalAlign(lValue / lTotalArea) & vbTab &
                                               DecimalAlign(lValue / lUnitsAdjust))
                                Else
                                    Dim lLoadUnit(0) As Double
                                    lLoadUnit(0) = lValue / lOperationTypeArea
                                    Dim lLoadTotal As Double = lValue / lUnitsAdjust
                                    Dim lLoadOverall As Double = lValue / lTotalArea
                                    '.Value(1) = (lLoadUnit(0))
                                    '.Value(2) = (lLoadTotal)
                                    '.Value(3) = (lLoadOverall)

                                    lSummaryReport.Append(lConstituentName.PadRight(lRowIdLength) & vbTab &
                                               DecimalAlign(lLoadUnit(0)) & vbTab &
                                               DecimalAlign(lLoadTotal) & vbTab &
                                               DecimalAlign(lLoadOverall))
                                    If Not lConstituentName.Contains("Rainfall") Then 'Anurag added this case as total rainfall was being added to the runoff
                                        lSubtotals(0) += lLoadUnit(0)
                                        lSubtotals(1) += lLoadTotal
                                        lSubtotals(2) += lLoadOverall
                                    End If

                                    If aBalanceType <> "Water" Then
                                        SumLoads(lLoadTotals, lConstituentName, lLoadUnit, lLoadTotal, lLoadOverall)
                                    End If
                                End If
                                lSummaryReport.AppendLine()
                            Else
                                'Logger.Dbg("SkipNoData:" & lConstituentKey)
                            End If
                            '.CurrentRecord += 1
                        ElseIf lConstituentKey.Substring(2).StartsWith("Total") AndAlso
                                 lConstituentKey.Substring(2).Length > 5 AndAlso
                                 IsNumeric(lConstituentKey.Substring(7, 1)) Then

                            lSummaryReport.AppendLine("Total".PadRight(lRowIdLength) & vbTab &
                                                      DecimalAlign(lSubtotals(0)) & vbTab &
                                                      DecimalAlign(lSubtotals(1)) & vbTab &
                                                      DecimalAlign(lSubtotals(2)))
                            lSubtotals(0) = 0
                            lSubtotals(1) = 0
                            lSubtotals(2) = 0

                        ElseIf Not lConstituentKey.Substring(2).StartsWith("Header") Then
                            Dim lSkipTo As String = FindSkipTo(lConstituentKey)
                            If lSkipTo IsNot Nothing Then
                                'At this point the specific data for AGCHEM N constitunet
                                'in HBN file is missing and code skips to PQUAL constituents                                                
                                Dim lSkipToindex2 As Integer = 2000

                                If lSkipTo.StartsWith("NO3+NO2") Then
                                    lSkipTo = "NO3+NO2 (PQUAL)"
                                    Dim lSkip2 As String = "NH3+NH4 (PQUAL)"
                                    lSkipToindex2 = lConstituentsToOutput.IndexOf(lSkip2)
                                End If
                                Dim lSkipToindex As Integer = lConstituentsToOutput.IndexOf(lSkipTo)

                                If lSkipToindex > lSkipToindex2 Then lSkipToindex = lSkipToindex2
                                If lSkipToindex > lConstituentIndex Then
                                    lConstituentIndex = lSkipToindex - 1
                                End If
                                lSummaryReport.Body.Remove(lHeaderStart + 10, lSummaryReport.Body.Length - lHeaderStart - 10)

                            Else

                                lSummaryReport.AppendLine(lConstituentName.PadRight(lRowIdLength) & vbTab &
                                                           DecimalAlign(0.0) & vbTab &
                                                           DecimalAlign(0.0))
                            End If

                        Else
                            lSummaryReport.AppendLine()
                            lSummaryReport.AppendLine(lConstituentName.PadRight(lRowIdLength))
                        End If
                        'End With
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
                            lSummaryReport.AppendLine(lLoad.Name.PadRight(lRowIdLength) & vbTab &
                                                  DecimalAlign(lLoad.Unit(0)) & vbTab &
                                                  DecimalAlign(lLoad.Total) & vbTab &
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

    Private Function Header(ByVal aBalanceType As String, ByVal aScenario As String, ByVal aRunMade As Date, ByVal auci As atcUCI.HspfUci,
                            ByVal aSDateJ As Double, ByVal aEDateJ As Double) As String
        Dim lString As String = "   Run Made " & aRunMade & vbCrLf
        lString &= "   " & auci.GlobalBlock.RunInf.Value & vbCrLf
        lString &= "   " & TimeSpanAsString(aSDateJ, aEDateJ, "Analysis Period: ")
        Return lString
    End Function

    Public Function FindSkipTo(ByVal aConstituentKey As String) As String
        Dim lSkipTo As String = Nothing
        Select Case aConstituentKey
            Case "P:NO3+NO2-N - SURFACE LAYER OUTFLOW", "NO3+NO2-N - SURFACE LAYER OUTFLOW" : lSkipTo = "NO3+NO2 (PQUAL)"
            Case "P:PO4-P IN SOLUTION - SURFACE LAYER - OUTFLOW", "PO4-P IN SOLUTION - SURFACE LAYER - OUTFLOW" : lSkipTo = "ORTHO P (PQUAL)"
            Case "P:SOQUAL-ORTHO P", "SOQUAL-ORTHO P" : lSkipTo = "ORTHO P (IQUAL)"
            Case "P:SOQUAL-NO3", "SOQUAL-NO3" : lSkipTo = "NO3 (IQUAL)"
            Case "P:WASHQS-BOD", "WASHQS-BOD" : lSkipTo = "  BOD from OrganicN"
            Case "P:ORGN - TOTAL OUTFLOW", "ORGN - TOTAL OUTFLOW" : lSkipTo = "BOD"
            Case "OVOL1" : lSkipTo = "OVOL2"
            Case "OVOL2" : lSkipTo = "OVOL3"
            Case "OVOL3" : lSkipTo = "OVOL4"
            Case "OVOL4" : lSkipTo = "OVOL5"
            Case "OVOL5" : lSkipTo = "IVOL"
            Case "N-TOT-OUT1" : lSkipTo = "N-TOT-OUT2"
            Case "N-TOT-OUT2" : lSkipTo = "N-TOT-OUT3"
            Case "N-TOT-OUT3" : lSkipTo = "N-TOT-OUT4"
            Case "N-TOT-OUT4" : lSkipTo = "N-TOT-OUT5"
            Case "N-TOT-OUT5" : lSkipTo = "END"
            Case "P-TOT-OUT1" : lSkipTo = "P-TOT-OUT2"
            Case "P-TOT-OUT2" : lSkipTo = "P-TOT-OUT3"
            Case "P-TOT-OUT3" : lSkipTo = "P-TOT-OUT4"
            Case "P-TOT-OUT4" : lSkipTo = "P-TOT-OUT5"
            Case "P-TOT-OUT5" : lSkipTo = "END"


        End Select
        Return lSkipTo
    End Function
End Module
