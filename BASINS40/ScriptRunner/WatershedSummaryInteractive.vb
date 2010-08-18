Imports atcData
Imports atcData.atcTimeseriesGroup
Imports atcUtility
Imports atcUCI
Imports MapWindow.Interfaces
Imports MapWinUtility

Public Module WatershedSummaryInteractive
    Private pWorkingDirectory As String = "F:\BASINS\modelout\nutrient\"
    Private pConstituent As String = "NO3"  'Water,Sediment,NH3,NH4,PO4,TotalN,TotalP,OrganicN,OrganicP,BOD,DO,FColi,Lead,WaterTemp,Zinc
    Private pScenario As String = "nutrien2"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)

        'For each UCI file in the specified (pTestPath) folder, 
        'do a Watershed Summary (Loading) Report for the specified constituents;
        'ie total n summary

        'Requirements:
        'UCI and HBN must reside in the same folder,
        'base name of UCI = base name of HBN file

        'Output:
        'writes to the pWorkingDirectory folder, for each UCI and constituent,
        'a file named as follows:
        'UCI base name & "_" & constituent name & "_" & "WatershedSummary.txt"

        Dim lAttributes As New atcCollection
        With lAttributes
            .Add("Scenario", pScenario)
            .Add("Constituent", pConstituent)
            .Add("Working Directory", pWorkingDirectory)
        End With
        Dim lAsk As New frmArgs
        If lAsk.AskUser("Watershed Summary Script Arguments", lAttributes) Then
            pConstituent = lAttributes.ItemByKey("Constituent")
            pScenario = lAttributes.ItemByKey("Scenario")
            pWorkingDirectory = lAttributes.ItemByKey("Working Directory")

            Logger.Dbg("Start")

            'change to the directory of the current project
            ChDriveDir(pWorkingDirectory)
            'Logger.Dbg(" CurDir:" & CurDir())

            'build collection of constituents to report
            Dim lConstituents As New atcCollection
            lConstituents.Add(pConstituent)

            'build collection of scenarios (uci base names) to report
            Dim lUcis As New System.Collections.Specialized.NameValueCollection
            Dim lScenarios As New atcCollection
            lScenarios.Add(pScenario)

            'loop thru each scenario (uci name)
            For Each lScenario As String In lScenarios
                'open the corresponding hbn file
                Dim lHspfBinFile As atcTimeseriesSource = New atcHspfBinOut.atcTimeseriesFileHspfBinOut
                Dim lHspfBinFileName As String = lScenario & ".hbn"
                Logger.Dbg(" AboutToOpen " & lHspfBinFileName)
                If Not FileExists(lHspfBinFileName) Then
                    'if hbn doesnt exist, make a guess at what the name might be
                    lHspfBinFileName = lHspfBinFileName.Replace(".hbn", ".base.hbn")
                    Logger.Dbg("  NameUpdated " & lHspfBinFileName)
                End If
                atcDataManager.OpenDataSource(lHspfBinFile, lHspfBinFileName, Nothing)
                Logger.Dbg(" DataSetCount " & lHspfBinFile.DataSets.Count)

                'make uci available 
                Dim lMsg As New atcUCI.HspfMsg
                lMsg.Open("hspfmsg.mdb")
                Dim lHspfUci As New atcUCI.HspfUci
                lHspfUci.FastReadUciForStarter(lMsg, lScenario & ".uci")

                'call main watershed summary routine
                Dim lHspfBinFileInfo As System.IO.FileInfo = New System.IO.FileInfo(lHspfBinFileName)
                HspfSupport.WatershedSummary.ReportsToFiles(lConstituents, lHspfUci, lHspfBinFile, lHspfBinFileInfo.LastWriteTime)

                'clean up 
                atcDataManager.DataSources.Remove(lHspfBinFile)
                lHspfBinFile.DataSets.Clear()
                lHspfBinFile = Nothing

                'open output files
                For Each lReportType As String In lConstituents
                    Dim lOutFileName As String = lScenario & "_" & lReportType & "_WatershedSummary.txt"
                    OpenFile(lOutFileName)
                Next lReportType
            Next
        Else
            Logger.Dbg("WatershedSummaryCancelled")
        End If
    End Sub
End Module
