Imports System
Imports atcUtility
Imports atcData
Imports atcWDM
Imports atcHspfBinOut
Imports MapWinUtility
Imports atcGraph
Imports ZedGraph

Imports MapWindow.Interfaces
Imports System.Collections.Specialized

Module HSPFWatershedSummaryWQReport 
    Private pTestPath As String
    Private pBaseName As String
    Private pDrive As String = "D:" 'C: in CA
    Private pSummaryType As String
    Private pOperationTypes As New atcCollection

    Private Sub Initialize()
        Dim lTestName As String = "upatoi"
        'Dim lTestName As String = "addtrail"
        Select Case lTestName
            Case "upatoi"
                pTestPath = pDrive & "\Basins\data\20710-01\Upatoi"
                pBaseName = "upatoi"
            Case "addtrail"
                pTestPath = pDrive & "\Basins\modelout\UpatoiScen"
                pBaseName = "addtrail"
        End Select

        pSummaryType = "Sediment"
        'pSummaryType = "Water"
        'pSummaryType = "BOD"
        'pSummaryType = "DO"
        'pSummaryType = "FColi"
        'pSummaryType = "Lead"
        'pSummaryType = "NH3"
        'pSummaryType = "NH4"
        'pSummaryType = "NO3"
        'pSummaryType = "OrganicN"
        'pSummaryType = "OrganicP"
        'pSummaryType = "PO4"
        'pSummaryType = "TotalN"
        'pSummaryType = "TotalP"
        'pSummaryType = "WaterTemp"
        'pSummaryType = "Zinc"
        Select Case pSummaryType
            Case "Water"
                pOperationTypes.Add("P:", "PERLND")
                pOperationTypes.Add("I:", "IMPLND")
                pOperationTypes.Add("R:", "RCHRES")
            Case "Sediment"
                pOperationTypes.Add("R:", "RCHRES")
        End Select
    End Sub

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Initialize()
        ChDriveDir(pTestPath)

        'open uci file
        Dim lMsg As New atcUCI.HspfMsg("hspfmsg.mdb")
        Dim lHspfUci As New atcUCI.HspfUci
        lHspfUci.FastReadUciForStarter(lMsg, pBaseName & ".uci")

        'open HBN file
        Dim lHspfBinFileName As String = pTestPath & "\" & pBaseName & ".hbn"
        Dim lHspfBinDataSource As New atcTimeseriesFileHspfBinOut()
        lHspfBinDataSource.Open(lHspfBinFileName)
        Dim lHspfBinFileInfo As System.IO.FileInfo = New System.IO.FileInfo(lHspfBinFileName)

        Dim lLocations As atcCollection = lHspfBinDataSource.DataSets.SortedAttributeValues("Location")

        'constituent balance
        Dim lReport As atcReport.ReportText = HspfSupport.ConstituentBalance.Report _
                (lHspfUci, pSummaryType, pOperationTypes, pBaseName, _
                 lHspfBinDataSource, lLocations, lHspfBinFileInfo.LastWriteTime)
        Dim lOutFileName As String = "outfiles\" & pSummaryType & "_" & "ConstituentBalance.txt"
        SaveFileString(lOutFileName, lReport.ToString)

        'watershed summary
        lReport = HspfSupport.WatershedSummary.Report(lHspfUci, lHspfBinDataSource, lHspfBinFileInfo.LastWriteTime, pSummaryType)
        lOutFileName = "outfiles\" & pSummaryType & "_" & "WatershedSummary.txt"
        SaveFileString(lOutFileName, lReport.ToString)
        lReport = Nothing
    End Sub
End Module
