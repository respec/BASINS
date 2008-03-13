Imports System
Imports atcUtility
Imports atcData
Imports atcWDM
Imports atcHspfBinOut
Imports HspfSupport
Imports MapWinUtility
Imports atcGraph
Imports ZedGraph

Imports MapWindow.Interfaces
Imports System.Collections.Specialized

Module HSPFWatershedSummaryWQReport 
    Private pTestPath As String
    Private pBaseName As String

    Private Sub Initialize()

        Dim lTestName As String = "upatoi"
        pTestPath = "c:\Basins\modelout\Upatoi"
        pBaseName = "upatoi"
        'Dim lTestName As String = "addtrail"
        'pTestPath = "c:\Basins\modelout\UpatoiScen"
        'pBaseName = "addtrail"

    End Sub

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Initialize()
        ChDriveDir(pTestPath)

        'open uci file
        Dim lMsg As New atcUCI.HspfMsg
        lMsg.Open("hspfmsg.mdb")
        Dim lHspfUci As New atcUCI.HspfUci
        lHspfUci.FastReadUciForStarter(lMsg, pBaseName & ".uci")

        'open WDM file
        'Dim lWdmFileName As String = pTestPath & "\" & pBaseName & ".wdm"
        'Dim lWdmDataSource As New atcDataSourceWDM()
        'lWdmDataSource.Open(lWdmFileName)

        'open HBN file
        Dim lHspfBinFileName As String = pTestPath & "\" & pBaseName & ".hbn"
        Dim lHspfBinDataSource As New atcTimeseriesFileHspfBinOut()
        lHspfBinDataSource.Open(lHspfBinFileName)

        Dim lSummaryType As String = "Sediment"
        'Dim lSummaryType As String = "Water"
        'Dim lSummaryType As String = "BOD"
        'Dim lSummaryType As String = "DO"
        'Dim lSummaryType As String = "FColi"
        'Dim lSummaryType As String = "Lead"
        'Dim lSummaryType As String = "NH3"
        'Dim lSummaryType As String = "NH4"
        'Dim lSummaryType As String = "NO3"
        'Dim lSummaryType As String = "OrganicN"
        'Dim lSummaryType As String = "OrganicP"
        'Dim lSummaryType As String = "PO4"
        'Dim lSummaryType As String = "Sediment"
        'Dim lSummaryType As String = "TotalN"
        'Dim lSummaryType As String = "TotalP"
        'Dim lSummaryType As String = "WaterTemp"
        'Dim lSummaryType As String = "Zinc"

        'watershed summary
        Dim lHspfBinFileInfo As System.IO.FileInfo = New System.IO.FileInfo(lHspfBinFileName)
        Dim lString As Text.StringBuilder = HspfSupport.WatershedSummary.Report(lHspfUci, lHspfBinDataSource, lHspfBinFileInfo.LastWriteTime, lSummaryType)
        Dim lOutFileName As String = "outfiles\" & lSummaryType & "_" & "WatershedSummary.txt"
        SaveFileString(lOutFileName, lString.ToString)
        lString = Nothing
        
    End Sub

End Module
