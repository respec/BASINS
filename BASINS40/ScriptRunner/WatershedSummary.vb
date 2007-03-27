Imports atcData
Imports atcData.atcDataGroup
Imports atcUtility
Imports atcUCI
Imports MapWindow.Interfaces
Imports MapWinUtility

Imports Microsoft.VisualBasic
Imports System

Public Module ScriptStatTestX
    Private Const pTestPath As String = "C:\test\WatershedSummary\"

    Public Sub ScriptMainX(ByRef aMapWin As IMapWin)
        Logger.Dbg("Start")
        ChDriveDir(pTestPath)
        Logger.Dbg(" CurDir:" & CurDir())

        Dim lScenario As String = "base"

        Dim lSummaryTypes As New atcCollection
        lSummaryTypes.Add("TotalN")

        Dim lArgs As Object() = Nothing
        Dim lErr As String = ""
        Dim lDataManager As New atcDataManager(aMapWin) ' = Scripting.Run("vb", "", "subFindDataManager.vb", lErr, False, aMapWin, aMapWin)

        'open hbn file
        Dim lHspfBinFile As atcDataSource = New atcHspfBinOut.atcTimeseriesFileHspfBinOut
        Dim lHspfBinFileName As String = "Input\" & lScenario & ".hbn"
        Logger.Dbg(" AboutToOpen " & lHspfBinFileName)
        lDataManager.OpenDataSource(lHspfBinFile, lHspfBinFileName, Nothing)
        Logger.Dbg(" DataSetCount " & lHspfBinFile.DataSets.Count)
        Dim lFileDetails As System.IO.FileInfo = New System.IO.FileInfo(lHspfBinFileName)

        DoWatershedSummary(lSummaryTypes, lScenario, lHspfBinFile, lFileDetails.CreationTime)

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

End Module
