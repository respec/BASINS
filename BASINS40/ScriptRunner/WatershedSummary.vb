Imports atcData
Imports atcData.atcDataGroup
Imports atcUtility
Imports atcUCI
Imports MapWindow.Interfaces
Imports MapWinUtility

Imports Microsoft.VisualBasic
Imports System

Public Module ScriptWatershedSummary
    Private Const pFieldWidth As Integer = 12
    Private Const pTestPath As String = "C:\test\ScriptReports\"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)

        'For each UCI file in the specified (pTestPath) folder, 
        'do a Watershed Summary (Loading) Report for the specified constituents;
        'ie total n summary

        'Requirements:
        'UCI and HBN must reside in the same folder,
        'base name of UCI = base name of HBN file

        'Output:
        'writes to the pTestPath folder, for each UCI and constituent,
        'a file named as follows:
        'UCI base name & "_" & constituent name & "_" & "WatershedSummary.txt"

        Logger.Dbg("Start")

        'change to the directory of the current project
        ChDriveDir(pTestPath)
        Logger.Dbg(" CurDir:" & CurDir())

        'build collection of constituents to report
        Dim lConstituents As New atcCollection
        'lConstituents.Add("Water")
        'lConstituents.Add("BOD")
        'lConstituents.Add("DO")
        'lConstituents.Add("FColi")
        'lConstituents.Add("Lead")
        'lConstituents.Add("NH3")
        'lConstituents.Add("NH4")
        lConstituents.Add("NO3")
        'lConstituents.Add("OrganicN")
        'lConstituents.Add("OrganicP")
        'lConstituents.Add("PO4")
        'lConstituents.Add("Sed")
        lConstituents.Add("TotalN")
        'lConstituents.Add("TotalP")
        'lConstituents.Add("WaterTemp")
        'lConstituents.Add("Zinc")

        'build collection of scenarios (uci base names) to report
        Dim lUcis As New System.Collections.Specialized.NameValueCollection
        AddFilesInDir(lUcis, pTestPath, False, "*.uci")
        Dim lScenarios As New atcCollection
        For Each lUci As String In lUcis
            lScenarios.Add(FilenameNoPath(FilenameNoExt(lUci)))
        Next

        'declare a new data manager to manage the hbn files
        Dim lDataManager As New atcDataManager(aMapWin) ' = Scripting.Run("vb", "", "subFindDataManager.vb", lErr, False, aMapWin, aMapWin)

        'loop thru each scenario (uci name)
        For Each lScenario As String In lScenarios

            'open the corresponding hbn file
            Dim lHspfBinFile As atcDataSource = New atcHspfBinOut.atcTimeseriesFileHspfBinOut
            Dim lHspfBinFileName As String = lScenario & ".hbn"
            Logger.Dbg(" AboutToOpen " & lHspfBinFileName)
            If Not FileExists(lHspfBinFileName) Then
                'if hbn doesnt exist, make a guess at what the name might be
                lHspfBinFileName = lHspfBinFileName.Replace(".hbn", ".base.hbn")
                Logger.Dbg("  NameUpdated " & lHspfBinFileName)
            End If
            Dim lHspfBinFileInfo As System.IO.FileInfo = New System.IO.FileInfo(lHspfBinFileName)
            lDataManager.OpenDataSource(lHspfBinFile, lHspfBinFileName, Nothing)
            Logger.Dbg(" DataSetCount " & lHspfBinFile.DataSets.Count)

            'call main watershed summary routine
            DoWatershedSummary(lConstituents, lScenario, lHspfBinFile, lHspfBinFileInfo.LastWriteTime)

            'clean up 
            lDataManager.DataSources.Remove(lHspfBinFile)
            lHspfBinFile.DataSets.Clear()
            lHspfBinFile = Nothing

        Next

    End Sub

    Friend Sub DoWatershedSummary(ByVal aSummaryTypes As atcCollection, _
                                  ByVal aScenario As String, _
                                  ByVal aScenarioResults As atcDataSource, _
                                  ByVal aRunMade As String)

        'make uci available 
        Dim lMsg As New atcUCI.HspfMsg
        lMsg.Open("hspfmsg.mdb")
        Dim lHspfUci As New atcUCI.HspfUci
        lHspfUci.FastReadUciForStarter(lMsg, aScenario & ".uci")

        For Each lSummaryType As String In aSummaryTypes
            Dim lAgchemConstituent As String = ""
            Dim lUnits As String = "lbs"
            Dim lTotalUnits As String = lUnits
            Dim lPerlndConstituents As New Collection
            Dim lImplndConstituents As New Collection
            Select Case lSummaryType
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
                Case "Sed"
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
            lString.AppendLine(lSummaryType & " Watershed Summary Report For " & aScenario)
            lString.AppendLine("   Run Made " & aRunMade)
            lString.AppendLine("   Average Annual Rates and Totals")
            lString.AppendLine("   " & lHspfUci.GlobalBlock.RunInf.Value)
            lString.AppendLine(vbCrLf)
            lString.AppendLine("Land Use  " & vbTab & _
                               "    Area    " & vbTab & _
                               "    Load    " & vbTab & _
                               "Total Load  " & vbTab & _
                               "Total Load")
            lString.AppendLine("            " & vbTab & _
                               "   (acres)  " & vbTab & _
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
                For Each lOper In lHspfUci.OpnBlks(lOperType).ids
                    'for each operation, get land use name, number of acres, and load/acre
                    lLuName = lOper.Tables("GEN-INFO").parms(1).value
                    lLuArea = LandArea(lOper.Name, lOper.Id, lHspfUci)

                    Dim lTempDataGroup As atcDataGroup = aScenarioResults.DataSets.FindData("Location", Left(lOperType, 1) & ":" & lOper.Id)
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
                                    If lSummaryType = "BOD" Then
                                        lMult = 0.4
                                    ElseIf lSummaryType = "OrganicN" Or lSummaryType = "TotalN" Then
                                        lMult = 0.048
                                    ElseIf lSummaryType = "OrganicP" Or lSummaryType = "TotalP" Then
                                        lMult = 0.0023
                                    End If
                                End If
                                lValue = lValue + (lTempDataSet.Attributes.GetDefinedValue("SumAnnual").Value * lMult)
                            End If
                        Next
                    End If
                    lTotal = lLuArea * lValue
                    If lSummaryType = "Water" Then
                        lTotal = lTotal * 8687.6  'convert in to cfs
                    End If
                    lLandUses.Add(Left(lOperType, 1) & lOper.Id & ":" & lLuName.Trim)
                    lAreas.Add(lLuArea)
                    lLoads.Add(lValue)
                    lTotalLoads.Add(lTotal)
                    lSum = lSum + lTotal
                Next
            Next

            For lIndex As Integer = 1 To lLandUses.Count
                lString.AppendLine(lLandUses(lIndex) & vbTab & _
                                   DF(lAreas(lIndex)) & vbTab & _
                                   DF(lLoads(lIndex)) & vbTab & _
                                   DF(lTotalLoads(lIndex)) & vbTab & _
                                   DF((lTotalLoads(lIndex) / lSum * 100), 2))
            Next
            lString.AppendLine("")
            lString.AppendLine(vbTab & vbTab & vbTab & vbTab & "Total Load = " & vbTab & DF(lSum) & vbTab & " " & lTotalUnits)

            Dim lOutFileName As String = aScenario & "_" & lSummaryType & "_" & "WatershedSummary.txt"
            Logger.Dbg("  WriteReportTo " & lOutFileName)
            SaveFileString(lOutFileName, lString.ToString)
        Next lSummaryType
    End Sub

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

    Private Function DF(ByVal aValue As Double, Optional ByVal aDecimalPlaces As Integer = 3) As String
        Dim lFormat As String
        If aDecimalPlaces > 1 Then
            lFormat = "###,##0.0" & StrDup(aDecimalPlaces - 1, "#")
        Else
            lFormat = "###,##0.0"
        End If
        Dim lString As String = DoubleToString(aValue, , lFormat, , , 5)
        Dim dp As Integer = lString.IndexOf("."c)
        If dp >= 0 Then
            Dim laddLeft As Integer = pFieldWidth - 5 - dp
            If laddLeft > 0 Then lString = Space(laddLeft) & lString
        End If
        Return lString.PadRight(pFieldWidth)
        'Return Trim(Format(aValue, "##########0." & StrDup(aDecimalPlaces, "0")))
    End Function

End Module
