Imports atcData
Imports atcData.atcDataGroup
Imports atcUtility
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

        Dim lPerConstituents2Output As New atcCollection
        Dim lImpConstituents2Output As New atcCollection
        Select Case pSummaryType
            Case "Water"
            Case "TotalN"
                'Total N is a combination of NH4, No3, OrganicN
                lPerConstituents2Output.Add("NITROGEN - ALL FORMS")
                lPerConstituents2Output.Add("POQUAL-NH4")
                lPerConstituents2Output.Add("POQUAL-NO3")
                lPerConstituents2Output.Add("POQUAL-BOD")  'to be mult by 0.048
                lImpConstituents2Output.Add("SOQUAL-NH4")
                lImpConstituents2Output.Add("SOQUAL-NO3")
                lImpConstituents2Output.Add("SOQUAL-BOD")  'to be mult by 0.048
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
        For Each lOper In lHspfUci.OpnBlks("PERLND").ids
            'for each operation, get land use name, number of acres, and load/acre
            lLuName = lOper.Tables("GEN-INFO").parms(1).value
            lLuArea = LandArea(lOper.Name, lOper.Id, lHspfUci)

            Dim lTempDataGroup As atcDataGroup = lHspfBinFile.DataSets.FindData("Location", "P:" & lOper.Id)
            For Each lCon As String In lPerConstituents2Output
                'Dim lTempDataSet As atcDataSet = lTempDataGroup.FindData("Constituent", lCon).Item(0)
                'lValue = lTempDataSet.Attributes.GetDefinedValue("SumAnnual").Value
            Next
            lString.AppendLine(lLuName & lLuArea & lValue)
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
