Imports atcUtility
Imports atcData
Imports atcUCI
Imports MapWinUtility

Public Module WatershedSummary
    Public Sub ReportsToFiles(ByVal aSummaryTypes As atcCollection, _
                              ByVal aUci As atcUCI.HspfUci, _
                              ByVal aScenarioResults As atcDataSource, _
                              ByVal aRunMade As String)

        For Each lSummaryType As String In aSummaryTypes
            Dim lString As Text.StringBuilder = Report(aUci, aScenarioResults, aRunMade, lSummaryType)
            Dim lOutFileName As String = FilenameOnly(aUci.Name) & "_" & lSummaryType & "_" & "WatershedSummary.txt"
            Logger.Dbg("  WriteReportTo " & lOutFileName)
            SaveFileString(lOutFileName, lString.ToString)
        Next lSummaryType
    End Sub

    Public Function Report(ByVal aUci As HspfUci, ByVal aScenarioResults As atcDataSource, ByVal aRunMade As String, ByVal aSummaryType As String) As System.Text.StringBuilder
        Dim lAgchemConstituent As String = ""
        Dim lUnits As String = "lbs"
        Dim lTotalUnits As String = lUnits
        Dim lPerlndConstituents As New Collection
        Dim lImplndConstituents As New Collection
        Select Case aSummaryType
            Case "Water"
                lPerlndConstituents.Add("PERO")
                lImplndConstituents.Add("SURO")
                lUnits = "in"
                lTotalUnits = "cfs"
            Case "BOD"
                lPerlndConstituents.Add("POQUAL-BOD")
                lImplndConstituents.Add("SOQUAL-BOD")
            Case "DO"
                lPerlndConstituents.Add("PODOXM")
                lImplndConstituents.Add("SODOXM")
            Case "FColi"
                lPerlndConstituents.Add("POQUAL-F.Coliform")
                lImplndConstituents.Add("SOQUAL-F.Coliform")
                lUnits = "10^9"
                lTotalUnits = lUnits
            Case "Lead"
                lPerlndConstituents.Add("POQUAL-LEAD")
                lImplndConstituents.Add("SOQUAL-LEAD")
            Case "NH3"
                lPerlndConstituents.Add("POQUAL-NH3")
                lImplndConstituents.Add("SOQUAL-NH3")
            Case "NH4"
                lAgchemConstituent = "NH4-N - TOTAL OUTFLOW"
                lPerlndConstituents.Add("POQUAL-NH4")
                lImplndConstituents.Add("SOQUAL-NH4")
            Case "NO3"
                lAgchemConstituent = "N03-N - TOTAL OUTFLOW"
                lPerlndConstituents.Add("POQUAL-NO3")
                lImplndConstituents.Add("SOQUAL-NO3")
            Case "OrganicN"
                lAgchemConstituent = "ORGN - TOTAL OUTFLOW"
                lPerlndConstituents.Add("POQUAL-BOD")
                lImplndConstituents.Add("SOQUAL-BOD")
            Case "OrganicP"
                lPerlndConstituents.Add("POQUAL-BOD")
                lImplndConstituents.Add("SOQUAL-BOD")
            Case "PO4"
                lPerlndConstituents.Add("POQUAL-ORTHO P")
                lImplndConstituents.Add("SOQUAL-ORTHO P")
            Case "Sediment"
                lPerlndConstituents.Add("SOSED")
                lImplndConstituents.Add("SOSLD")
                lUnits = "tons"
                lTotalUnits = lUnits
            Case "TotalN"
                lAgchemConstituent = "NITROGEN - TOTAL OUTFLOW"
                'Total N is a combination of NH4, No3, OrganicN
                lPerlndConstituents.Add("POQUAL-NH4")
                lPerlndConstituents.Add("POQUAL-NO3")
                lPerlndConstituents.Add("POQUAL-BOD")
                lImplndConstituents.Add("SOQUAL-NH4")
                lImplndConstituents.Add("SOQUAL-NO3")
                lImplndConstituents.Add("SOQUAL-BOD")
            Case "TotalP"
                'Total P is a combination of PO4 and OrganicP
                lPerlndConstituents.Add("POQUAL-ORTHO P")
                lPerlndConstituents.Add("POQUAL-BOD")
                lImplndConstituents.Add("SOQUAL-ORTHO P")
                lImplndConstituents.Add("SOQUAL-BOD")
            Case "WaterTemp"
                lPerlndConstituents.Add("POHT")
                lPerlndConstituents.Add("SOHT")
                lUnits = "btu"
                lTotalUnits = lUnits
            Case "Zinc"
                lPerlndConstituents.Add("POQUAL-ZINC")
                lImplndConstituents.Add("SOQUAL-ZINC")
        End Select

        Dim lString As New Text.StringBuilder
        lString.AppendLine(aSummaryType & " Watershed Summary Report For " & FilenameOnly(aUci.Name))
        lString.AppendLine("   Run Made " & aRunMade)
        lString.AppendLine("   Average Annual Rates and Totals")
        lString.AppendLine("   " & aUci.GlobalBlock.RunInf.Value)
        lString.AppendLine("   " & aUci.GlobalBlock.RunPeriod)
        lString.AppendLine("Land Use".PadLeft(25) & vbTab & _
                           "   Area    " & vbTab & _
                           "    Load    " & vbTab & _
                           "Total Load  " & vbTab & _
                           "Total Load")
        lString.AppendLine("                         " & vbTab & _
                           "  (acres)  " & vbTab & _
                           " (" & lUnits & "/acre) " & vbTab & _
                           "  (" & lTotalUnits & ")     " & vbTab & _
                           "    (%)     ")
        lString.AppendLine("")

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
                    lLuName = lOper.Tables("GEN-INFO").Parms(1).Value
                    lLuArea = LandArea(lOper.Name, lOper.Id, aUci)

                    Dim lTempDataGroup As atcDataGroup = aScenarioResults.DataSets.FindData("Location", Left(lOperType, 1) & ":" & lOper.Id)
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

        Dim lStr As String
        For lIndex As Integer = 1 To lLandUses.Count
            lStr = lLandUses(lIndex)
            lStr = lStr.PadLeft(25)
            lString.AppendLine(lStr & vbTab & _
                               DecimalAlign(lAreas(lIndex)) & vbTab & _
                               DecimalAlign(lLoads(lIndex)) & vbTab & _
                               DecimalAlign(lTotalLoads(lIndex)) & vbTab & _
                               DecimalAlign((lTotalLoads(lIndex) / lSum * 100), , 2))
        Next
        lString.AppendLine("")
        lStr = ""
        lStr = lStr.PadRight(25)
        lString.AppendLine(lStr & vbTab & vbTab & vbTab & "Total Load = " & vbTab & DecimalAlign(lSum) & vbTab & " " & lTotalUnits)
        Return lString
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
