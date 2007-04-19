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
    Private Const pTestPath As String = "C:\test\SegmentBalance\"

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
        lConstituents.Add("TotalN")
        'lConstituents.Add("Water")    'not yet implemented

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
            DoWatershedSummary(lConstituents, lScenario, lHspfBinFile, lHspfBinFileInfo.CreationTime)

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
            Dim lPrimaryConstituent As String = ""
            Select Case lSummaryType
                Case "Water"
                Case "TotalN"
                    'Total N is a combination of NH4, No3, OrganicN
                    lPrimaryConstituent = "NITROGEN - TOTAL OUTFLOW"
            End Select

            Dim lString As New Text.StringBuilder
            lString.AppendLine(lSummaryType & " Watershed Summary Report For " & aScenario)
            lString.AppendLine("   Run Made " & aRunMade & vbCrLf)
            lString.AppendLine("Land Use  " & vbTab & _
                               "    Area    " & vbTab & _
                               "    Load    " & vbTab & _
                               "Total Load  " & vbTab & _
                               "Total Load")
            lString.AppendLine("            " & vbTab & _
                               "   (acres)  " & vbTab & _
                               " (lbs/acre) " & vbTab & _
                               "  (lbs)     " & vbTab & _
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
                    If lTempDataGroup.FindData("Constituent", lPrimaryConstituent).Count > 0 Then
                        lTempDataSet = lTempDataGroup.FindData("Constituent", lPrimaryConstituent).Item(0)
                        lValue = lTempDataSet.Attributes.GetDefinedValue("SumAnnual").Value
                    Else
                        If lOperType = "PERLND" Then
                            lTempDataSet = lTempDataGroup.FindData("Constituent", "POQUAL-NH4").Item(0)
                            lValue = lTempDataSet.Attributes.GetDefinedValue("SumAnnual").Value
                            lTempDataSet = lTempDataGroup.FindData("Constituent", "POQUAL-NO3").Item(0)
                            lValue = lValue + lTempDataSet.Attributes.GetDefinedValue("SumAnnual").Value
                            lTempDataSet = lTempDataGroup.FindData("Constituent", "POQUAL-BOD").Item(0)
                            lValue = lValue + lTempDataSet.Attributes.GetDefinedValue("SumAnnual").Value * 0.048
                        Else
                            lTempDataSet = lTempDataGroup.FindData("Constituent", "SOQUAL-NH4").Item(0)
                            lValue = lTempDataSet.Attributes.GetDefinedValue("SumAnnual").Value
                            lTempDataSet = lTempDataGroup.FindData("Constituent", "SOQUAL-NO3").Item(0)
                            lValue = lValue + lTempDataSet.Attributes.GetDefinedValue("SumAnnual").Value
                            lTempDataSet = lTempDataGroup.FindData("Constituent", "SOQUAL-BOD").Item(0)
                            lValue = lValue + lTempDataSet.Attributes.GetDefinedValue("SumAnnual").Value * 0.048
                        End If
                    End If
                    lTotal = lLuArea * lValue
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
            lString.AppendLine(vbTab & vbTab & vbTab & vbTab & "Total Load = " & vbTab & DF(lSum) & vbTab & " lbs.")

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
