Imports atcData
Imports atcData.atcDataGroup
Imports atcUtility
Imports atcUCI
Imports MapWindow.Interfaces
Imports MapWinUtility

Imports Microsoft.VisualBasic
Imports System

Public Module ScriptStatTest
    Private Const pTestPath As String = "C:\test\WatershedSummary\"
    Private Const pSummaryType As String = "TotalN"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.Dbg("Start")
        ChDriveDir(pTestPath)
        Logger.Dbg(" CurDir:" & CurDir)

        Dim lScenario As String = "base"

        Dim lPrimaryConstituent As String = ""
        Select Case pSummaryType
            Case "Water"
            Case "TotalN"
                'Total N is a combination of NH4, No3, OrganicN
                lPrimaryConstituent = "NITROGEN - ALL FORMS"

        End Select

        Dim lString As New Text.StringBuilder
        lString.AppendLine(pSummaryType & " Watershed Summary Report For " & lScenario)

        Dim lArgs As Object() = Nothing
        Dim lErr As String = ""
        Dim lDataManager As New atcDataManager(aMapWin) ' = Scripting.Run("vb", "", "subFindDataManager.vb", lErr, False, aMapWin, aMapWin)

        'open hbn file
        Dim lHspfBinFile As atcDataSource = New atcHspfBinOut.atcTimeseriesFileHspfBinOut
        Dim lHspfBinFileName As String = "Input\" & lScenario & ".hbn"
        Logger.Dbg(" AboutToOpen " & lHspfBinFileName)
        lDataManager.OpenDataSource(lHspfBinFile, lHspfBinFileName, Nothing)
        Logger.Dbg(" DataSetCount " & lHspfBinFile.Datasets.Count)
        Dim lFileDetails As System.IO.FileInfo = New System.IO.FileInfo(lHspfBinFileName)
        lString.AppendLine("   Run Made " & lFileDetails.CreationTime & vbCrLf)

        'make uci available 
        Dim lMsg As New atcUCI.HspfMsg
        lMsg.Open("hspfmsg.mdb")
        Dim lHspfUci As New atcUCI.HspfUci
        lHspfUci.FastReadUciForStarter(lMsg, "Input\" & lScenario & ".uci")

        Dim lOper As atcUCI.HspfOperation
        Dim lLuName As String
        Dim lLuArea As Single
        Dim lValue As Single
        Dim lTempDataSet As atcDataSet
        Dim lOperTypes As New Collection
        lOperTypes.Add("PERLND")
        lOperTypes.Add("IMPLND")
        For Each lOperType As String In lOperTypes
            For Each lOper In lHspfUci.OpnBlks(lOperType).ids
                'for each operation, get land use name, number of acres, and load/acre
                lLuName = lOper.Tables("GEN-INFO").parms(1).value
                lLuArea = LandArea(lOper.Name, lOper.Id, lHspfUci)

                Dim lTempDataGroup As atcDataGroup = lHspfBinFile.DataSets.FindData("Location", Left(lOperType, 1) & ":" & lOper.Id)
                lTempDataSet = lTempDataGroup.FindData("Constituent", lPrimaryConstituent).Item(0)
                If Not lTempDataSet Is Nothing Then
                    lValue = lTempDataSet.Attributes.GetDefinedValue("SumAnnual").Value
                Else
                    If lOperType = "PERLND" Then
                        lTempDataSet = lTempDataGroup.FindData("Constituent", "POQUAL-NH4").Item(0)
                        'lValue = lTempDataSet.Attributes.GetDefinedValue("SumAnnual").Value
                        lTempDataSet = lTempDataGroup.FindData("Constituent", "POQUAL-NO3").Item(0)
                        'lValue = lValue + lTempDataSet.Attributes.GetDefinedValue("SumAnnual").Value
                        lTempDataSet = lTempDataGroup.FindData("Constituent", "POQUAL-BOD").Item(0)
                        'lValue = lValue + lTempDataSet.Attributes.GetDefinedValue("SumAnnual").Value * 0.048
                    Else
                        lTempDataSet = lTempDataGroup.FindData("Constituent", "SOQUAL-NH4").Item(0)
                        'lValue = lTempDataSet.Attributes.GetDefinedValue("SumAnnual").Value
                        lTempDataSet = lTempDataGroup.FindData("Constituent", "SOQUAL-NO3").Item(0)
                        'lValue = lValue + lTempDataSet.Attributes.GetDefinedValue("SumAnnual").Value
                        lTempDataSet = lTempDataGroup.FindData("Constituent", "SOQUAL-BOD").Item(0)
                        'lValue = lValue + lTempDataSet.Attributes.GetDefinedValue("SumAnnual").Value * 0.048
                    End If
                End If
                lString.AppendLine(lLuName & lLuArea & lValue)
            Next
        Next

        Dim lOutFileName As String = lScenario & "_" & pSummaryType & "_" & "WatershedSummary.txt"
        Logger.Dbg("  WriteReportTo " & lOutFileName)
        SaveFileString(lOutFileName, lString.ToString)

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
