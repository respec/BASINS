Imports atcUtility
Imports atcData
Imports atcUCI
Imports MapWinUtility

Public Module WatershedSummary
    Public Sub ReportsToFiles(ByVal aSummaryTypes As atcCollection, _
                              ByVal aUci As atcUCI.HspfUci, _
                              ByVal aScenarioResults As atcTimeseriesSource, _
                              ByVal aRunMade As String)

        For Each lSummaryType As String In aSummaryTypes
            Dim lReport As atcReport.ReportText = Report(aUci, aScenarioResults, aRunMade, lSummaryType)
            Dim lOutFileName As String = IO.Path.GetFileNameWithoutExtension(aUci.Name) & "_" & lSummaryType & "_WatershedSummary.txt"
            Logger.Dbg("  WriteReportTo " & lOutFileName)
            SaveFileString(lOutFileName, lReport.ToString)
        Next lSummaryType
    End Sub

    Public Function Report(ByVal aUci As HspfUci, ByVal aScenarioResults As atcTimeseriesSource, ByVal aRunMade As String, ByVal aSummaryType As String) As atcReport.IReport
        Dim lAgchemConstituent As String = ""
        Dim lUnits As String = "lbs"
        Dim lTotalUnits As String = lUnits
        Dim lPerlndConstituents As New Collection
        Dim lImplndConstituents As New Collection

        Dim lRchresConstituents As New Collection
        Dim lRchresConversion As String = "1.0"

        Select Case aSummaryType
            Case "Water"
                lPerlndConstituents.Add("PERO")
                lImplndConstituents.Add("SURO")
                lUnits = "in"
                lTotalUnits = "cfs"
                lRchresConstituents.Add("ROVOL")
                lRchresConversion = "723.97"
            Case "BOD"
                lPerlndConstituents.Add("POQUAL-BOD")
                lImplndConstituents.Add("SOQUAL-BOD")
                lRchresConstituents.Add("BODOUTTOT")
            Case "DO"
                lPerlndConstituents.Add("PODOXM")
                lImplndConstituents.Add("SODOXM")
                lRchresConstituents.Add("DOXOUTTOT")
            Case "FColi"
                lPerlndConstituents.Add("POQUAL-F.Coliform")
                lImplndConstituents.Add("SOQUAL-F.Coliform")
                lUnits = "10^9"
                lTotalUnits = lUnits
                lRchresConstituents.Add("F.Coliform-TROQAL")
            Case "Lead"
                lPerlndConstituents.Add("POQUAL-LEAD")
                lImplndConstituents.Add("SOQUAL-LEAD")
                lRchresConstituents.Add("LEAD-TROQAL")
            Case "NH3"
                lPerlndConstituents.Add("POQUAL-NH3")
                lImplndConstituents.Add("SOQUAL-NH3")
                lRchresConstituents.Add("TAM-OUTTOT")
            Case "NH4"
                lAgchemConstituent = "NH4-N - TOTAL OUTFLOW"
                lPerlndConstituents.Add("POQUAL-NH4")
                lImplndConstituents.Add("SOQUAL-NH4")
                lRchresConstituents.Add("TAM-OUTTOT")
            Case "NO3"
                lAgchemConstituent = "N03-N - TOTAL OUTFLOW"
                lPerlndConstituents.Add("POQUAL-NO3")
                lImplndConstituents.Add("SOQUAL-NO3")
                lRchresConstituents.Add("NO3-OUTTOT")
            Case "OrganicN"
                lAgchemConstituent = "ORGN - TOTAL OUTFLOW"
                lPerlndConstituents.Add("POQUAL-BOD")
                lImplndConstituents.Add("SOQUAL-BOD")
                lRchresConstituents.Add("BODOUTTOT")
            Case "OrganicP"
                lPerlndConstituents.Add("POQUAL-BOD")
                lImplndConstituents.Add("SOQUAL-BOD")
                lRchresConstituents.Add("BODOUTTOT")
            Case "PO4"
                lPerlndConstituents.Add("POQUAL-ORTHO P")
                lImplndConstituents.Add("SOQUAL-ORTHO P")
                lRchresConstituents.Add("PO4-OUTTOT")
            Case "Sediment"
                lPerlndConstituents.Add("SOSED")
                lImplndConstituents.Add("SOSLD")
                lUnits = "tons"
                lTotalUnits = lUnits
                lRchresConstituents.Add("ROSED-TOT")
            Case "TotalN"
                lAgchemConstituent = "NITROGEN - TOTAL OUTFLOW"
                'Total N is a combination of NH4, No3, OrganicN
                lPerlndConstituents.Add("POQUAL-NH4")
                lPerlndConstituents.Add("POQUAL-NO3")
                lPerlndConstituents.Add("POQUAL-BOD")
                lImplndConstituents.Add("SOQUAL-NH4")
                lImplndConstituents.Add("SOQUAL-NO3")
                lImplndConstituents.Add("SOQUAL-BOD")
                lRchresConstituents.Add("TAM-OUTTOT")
                lRchresConstituents.Add("NO3-OUTTOT")
                lRchresConstituents.Add("BODOUTTOT")
            Case "TotalP"
                'Total P is a combination of PO4 and OrganicP
                lPerlndConstituents.Add("POQUAL-ORTHO P")
                lPerlndConstituents.Add("POQUAL-BOD")
                lImplndConstituents.Add("SOQUAL-ORTHO P")
                lImplndConstituents.Add("SOQUAL-BOD")
                lRchresConstituents.Add("PO4-OUTTOT")
                lRchresConstituents.Add("BODOUTTOT")
            Case "WaterTemp"
                lPerlndConstituents.Add("POHT")
                lPerlndConstituents.Add("SOHT")
                lUnits = "btu"
                lTotalUnits = lUnits
                lRchresConstituents.Add("ROHEAT")
            Case "Zinc"
                lPerlndConstituents.Add("POQUAL-ZINC")
                lImplndConstituents.Add("SOQUAL-ZINC")
                lRchresConstituents.Add("ZINC-TROQAL")
        End Select

        Dim lReport As New atcReport.ReportText
        lReport.AppendLine(aSummaryType & " Watershed Summary Report For " & IO.Path.GetFileNameWithoutExtension(aUci.Name))
        lReport.AppendLine("   Run Made " & aRunMade)
        lReport.AppendLine("   Average Annual Rates and Totals")
        lReport.AppendLine("   " & aUci.GlobalBlock.RunInf.Value)
        lReport.AppendLine("   " & aUci.GlobalBlock.RunPeriod)

        Dim lOper As atcUCI.HspfOperation
        Dim lLuName As String
        Dim lLuArea As Single
        Dim lValue As Single
        Dim lTotal As Single
        Dim lTempDataSet As atcDataSet
        Dim lOperTypes As New Collection
        lOperTypes.Add("PERLND")
        lOperTypes.Add("IMPLND")

        Dim lLandUses As New Collection
        Dim lAreas As New Collection
        Dim lLoads As New Collection
        Dim lTotalLoads As New Collection
        Dim lSum As Double = 0.0

        For Each lOperType As String In lOperTypes
            For Each lOper In aUci.OpnBlks(lOperType).Ids
                'for each operation, get land use name, number of acres, and load/acre
                If Not lOper.TableExists("GEN-INFO") Then
                    Logger.Dbg("Missing:GEN-INFO:" & lOper.Name & ":" & lOper.Id)
                Else
                    lLuName = lOper.Tables("GEN-INFO").Parms(0).Value
                    lLuArea = LandArea(lOper.Name, lOper.Id, aUci)

                    Dim lTempDataGroup As atcTimeseriesGroup = aScenarioResults.DataSets.FindData("Location", Left(lOperType, 1) & ":" & lOper.Id)
                    If lTempDataGroup Is Nothing Then
                        Logger.Dbg("No Data") 'TODO: for?
                    End If
                    If lTempDataGroup.FindData("Constituent", lAgchemConstituent).Count > 0 Then
                        'if you find the agchem constituent, use it
                        lTempDataSet = lTempDataGroup.FindData("Constituent", lAgchemConstituent).Item(0)
                        lValue = lTempDataSet.Attributes.GetDefinedValue("SumAnnual").Value
                    Else
                        'normal case -- don't have the agchem constituent
                        Dim lConstituents As New Collection
                        If lOperType = "PERLND" Then
                            lConstituents = lPerlndConstituents
                        Else
                            lConstituents = lImplndConstituents
                        End If
                        lValue = 0.0
                        For Each lConstituent As String In lConstituents
                            If lTempDataGroup.FindData("Constituent", lConstituent).Count > 0 Then
                                lTempDataSet = lTempDataGroup.FindData("Constituent", lConstituent).Item(0)
                                Dim lMult As Single = 1.0
                                If lConstituent = "POQUAL-BOD" Or lConstituent = "SOQUAL-BOD" Then
                                    'might need another multiplier for bod
                                    If aSummaryType = "BOD" Then
                                        lMult = 0.4
                                    ElseIf aSummaryType = "OrganicN" Or aSummaryType = "TotalN" Then
                                        lMult = 0.048
                                    ElseIf aSummaryType = "OrganicP" Or aSummaryType = "TotalP" Then
                                        lMult = 0.0023
                                    End If
                                ElseIf lConstituent = "POQUAL-F.Coliform" Or lConstituent = "SOQUAL-F.Coliform" Then
                                    lMult = 1 / 1000000000.0 '10^9
                                End If
                                lValue += (lTempDataSet.Attributes.GetDefinedValue("SumAnnual").Value * lMult)
                            End If
                        Next
                    End If
                    lTotal = lLuArea * lValue
                    If aSummaryType = "Water" Then
                        lTotal /= 8687.6  'convert in to cfs
                    End If
                    lLandUses.Add(Left(lOperType, 1) & lOper.Id & ":" & lLuName.Trim)
                    lAreas.Add(lLuArea)
                    lLoads.Add(lValue)
                    lTotalLoads.Add(lTotal)
                    lSum += lTotal
                End If
            Next
        Next

        'begin reach section
        Dim lReaches As New Collection
        Dim lReachLoads As New Collection
        Dim lReachName As String = ""
        Dim lRchOperType As String = "RCHRES"
        For Each lOper In aUci.OpnBlks(lRchOperType).Ids
            'for each rchres, get reach name
            If Not lOper.TableExists("GEN-INFO") Then
                Logger.Dbg("Missing:GEN-INFO:" & lOper.Name & ":" & lOper.Id)
            Else
                lReachName = lOper.Tables("GEN-INFO").Parms(0).Value
                Dim lTempDataGroup As atcTimeseriesGroup = aScenarioResults.DataSets.FindData("Location", Left(lRchOperType, 1) & ":" & lOper.Id)
                If lTempDataGroup Is Nothing Then
                    Logger.Dbg("No Data") 'TODO: for?
                End If
                lValue = 0.0
                For Each lConstituent As String In lRchresConstituents
                    If lTempDataGroup.FindData("Constituent", lConstituent).Count > 0 Then
                        lTempDataSet = lTempDataGroup.FindData("Constituent", lConstituent).Item(0)
                        Dim lMult As Single = 1.0
                        If lConstituent = "BODOUTTOT" Then
                            'might need another multiplier for bod
                            If aSummaryType = "BOD" Then
                                lMult = 0.4
                            ElseIf aSummaryType = "OrganicN" Or aSummaryType = "TotalN" Then
                                lMult = 0.048
                            ElseIf aSummaryType = "OrganicP" Or aSummaryType = "TotalP" Then
                                lMult = 0.0023
                            End If
                        ElseIf lConstituent = "F.Coliform-TROQAL" Then
                            lMult = 1 / 1000000000.0 '10^9
                        End If
                        lValue += (lTempDataSet.Attributes.GetDefinedValue("SumAnnual").Value * lMult / lRchresConversion)
                    End If
                Next
                lReaches.Add(lReachName.Trim)
                lReachLoads.Add(lValue)
            End If
        Next

        'now build table for output
        Dim lOutputTable As New atcTableDelimited
        lOutputTable.TrimValues = False
        Dim lFieldWidth As Integer = 12

        With lOutputTable
            .NumHeaderRows = 0
            .Delimiter = vbTab
            .NumFields = 5

            .FieldLength(1) = 25
            .FieldName(1) = "Land Use".PadLeft(25)
            .FieldLength(2) = lFieldWidth
            .FieldName(2) = Center("Area", lFieldWidth)
            .FieldLength(3) = lFieldWidth
            .FieldName(3) = Center("Load", lFieldWidth)
            .FieldLength(4) = lFieldWidth
            .FieldName(4) = Center("Total Load", lFieldWidth)
            .FieldLength(5) = lFieldWidth
            .FieldName(5) = Center("Total Load", lFieldWidth)

            .CurrentRecord += 1
            .Value(1) = "".PadLeft(25)
            .Value(2) = Center("(acres)", lFieldWidth)
            .Value(3) = Center("(" & lUnits & "/acre)", lFieldWidth)
            .Value(4) = Center("(" & lTotalUnits & ")", lFieldWidth)
            .Value(5) = Center("(%)", lFieldWidth)

            .CurrentRecord += 1
            For lIndex As Integer = 1 To lLandUses.Count
                .CurrentRecord += 1
                .Value(1) = lLandUses(lIndex).PadLeft(25)
                .Value(2) = DecimalAlign(lAreas(lIndex))
                .Value(3) = DecimalAlign(lLoads(lIndex))
                .Value(4) = DecimalAlign(lTotalLoads(lIndex))
                .Value(5) = DecimalAlign((lTotalLoads(lIndex) / lSum * 100), 10, 2)
            Next

            .CurrentRecord += 1
            .CurrentRecord += 1

            .Value(1) = "".PadLeft(25)
            .Value(2) = "".PadLeft(lFieldWidth)
            .Value(3) = "Total Load = "
            .Value(4) = DecimalAlign(DecimalAlign(lSum))
            .Value(5) = lTotalUnits

            .CurrentRecord += 1
            .CurrentRecord += 1
            .Value(1) = "Reach Output".PadLeft(25)
            .CurrentRecord += 1

            For lIndex As Integer = 1 To lReaches.Count
                .CurrentRecord += 1
                .Value(1) = lReaches(lIndex).ToString.PadLeft(25)
                .Value(2) = "".PadLeft(lFieldWidth)
                .Value(3) = "".PadLeft(lFieldWidth)
                .Value(4) = DecimalAlign(lReachLoads(lIndex))
            Next

            lReport.Append(.ToString)
        End With

        Return lReport
    End Function

    Private Function LandArea(ByVal aOperName As String, ByVal aOperID As Integer, ByVal aUci As atcUCI.HspfUci) As Single
        Dim lOpnBlk As atcUCI.HspfOpnBlk
        lOpnBlk = aUci.OpnBlks(aOperName)
        Dim lOper As atcUCI.HspfOperation = lOpnBlk.OperFromID(aOperID)
        LandArea = 0
        For Each lConn As atcUCI.HspfConnection In lOper.Targets
            If lConn.Target.VolName = "RCHRES" Then
                LandArea = LandArea + lConn.MFact
            End If
        Next
    End Function
End Module
