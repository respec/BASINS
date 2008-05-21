Imports MapWindow.Interfaces
Imports MapWinUtility

Imports atcUtility
Imports atcData
Imports atcWDM
Imports atcTimeseriesBinary
Imports SwatObject

Module SWATRunner
    Private Const pRefreshDB As Boolean = False
    Private Const pOutputOnly As Boolean = True
    'Private Const pBasePath As String = "D:\Basins\data\SWATOutput\UM\baseline90"
    Private Const pBasePath As String = "C:\Project\UMRB\baseline90"
    'Private Const pInputPath As String = "D:\Basins\data\SWATOutput\UM\baseline90jack"
    Private Const pInputPath As String = "C:\Project\UMRB\baseline90\Scenarios\Test"
    'Private Const pSWATGDB As String = "SWAT2005.mdb"
    Private Const pSWATGDB As String = "C:\Program Files\SWAT\ArcSWAT\Databases\SWAT2005.mdb"
    Private Const pOutGDBPath As String = pInputPath & "\TablesIn"
    Private Const pOutGDB As String = "baseline90.mdb"
    Private Const pOutputFolder = pInputPath & "\TxtInOut"
    Private Const pSWATExe As String = pOutputFolder & "\swat2005.exe" 'local copy with input data
    'Private Const pSWATExe As String = "C:\Program Files\SWAT 2005 Editor\swat2005.exe"
    Private pSwatOutput As Text.StringBuilder
    Private pSwatError As Text.StringBuilder

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        ChDriveDir(pInputPath)
        Dim lLogFileName As String = Logger.FileName

        'log for swat runner
        Logger.StartToFile(pInputPath & "\logs\SWATRunner.log", , , True)

        Dim lOutGDB = pOutGDBPath & "\" & pOutGDB

        If Not pOutputOnly Then
            If pRefreshDB Then 'copy the entire input parameter database for this new scenario
                If IO.File.Exists(lOutGDB) Then
                    Logger.Dbg("DeleteExisting " & lOutGDB)
                    IO.File.Delete(lOutGDB)
                End If
                IO.File.Copy(pBasePath & "\" & pOutGDB, lOutGDB)
            End If

            Logger.Dbg("InitializeSwatInput")
            SwatInput.Initialize(pSWATGDB, lOutGDB, pOutputFolder)

            SwatInput.Hru.TableCreate()

            Logger.Dbg("SWATPreprocess-UpdateParametersAsRequested")
            For Each lString As String In LinesInFile("SWATParmChanges.txt")
                Dim lParms() As String = lString.Split(";")
                SwatInput.UpdateInputDB(lParms(0).Trim, lParms(1).Trim, lParms(2).Trim, lParms(3).Trim, lParms(4).Trim)
            Next

            Logger.Dbg("SWATSummarizeInput")
            Dim lStreamWriter As New IO.StreamWriter(pInputPath & "\logs\LandUses.txt")
            Dim lUniqueLandUses As DataTable = SwatInput.Hru.UniqueValues("LandUse")
            For Each lLandUse As DataRow In lUniqueLandUses.Rows
                lStreamWriter.WriteLine(lLandUse.Item(0).ToString)
            Next
            lStreamWriter.Close()

            Dim lLandUSeTable As DataTable = AggregateCrops(SwatInput.SubBasin.TableWithArea("LandUse"))
            SaveFileString(pInputPath & "\logs\AreaLandUseReport.txt", _
                           SWATArea.Report(lLandUSeTable))
            SaveFileString(pInputPath & "\logs\AreaSoilReport.txt", _
                           SWATArea.Report(SwatInput.SubBasin.TableWithArea("Soil")))
            SaveFileString(pInputPath & "\logs\AreaSlopeCodeReport.txt", _
                           SWATArea.Report(SwatInput.SubBasin.TableWithArea("Slope_Cd")))

            LaunchProgram(pSWATExe, pOutputFolder)
        End If

        Dim lOutputRch As New atcTimeseriesSWAT.atcTimeseriesSWAT
        With lOutputRch
            .Open(pOutputFolder & "\output.rch")
            Logger.Dbg("OutputRchTimserCount " & .DataSets.Count)
            WriteDatasets(pOutputFolder & "\rch", .DataSets)
        End With

        Dim lOutputSub As New atcTimeseriesSWAT.atcTimeseriesSWAT
        With lOutputSub
            .Open(pOutputFolder & "\output.sub")
            Logger.Dbg("OutputSubTimserCount " & .DataSets.Count)
            WriteDatasets(pOutputFolder & "\sub", .DataSets)
        End With

        Dim lOutputFields As New atcData.atcDataAttributes
        lOutputFields.SetValue("FieldName", "AREAkm2;YLDt/ha")
        Dim lOutputHru As New atcTimeseriesSWAT.atcTimeseriesSWAT
        With lOutputHru
            .Open(pOutputFolder & "\output.hru", lOutputFields)
            Logger.Dbg("OutputHruTimserCount " & .DataSets.Count)
            WriteDatasets(pOutputFolder & "\hru", .DataSets)
        End With

        Logger.Dbg("SwatPostProcessingDone")

        'back to basins log
        Logger.StartToFile(lLogFileName, True, False, True)
    End Sub

    Private Sub WriteDatasets(ByVal aFileName As String, ByVal aDatasets As atcDataGroup)
        Dim lDataTarget As New atcDataSourceTimeseriesBinary ' atcDataSourceWDM
        Dim lFileName As String = aFileName & ".tsbin" 'lDataTarget.Filter.?) Then
        TryDelete(lFileName)
        If lDataTarget.Open(lFileName) Then
            lDataTarget.AddDatasets(aDatasets)
        End If
    End Sub
End Module

Module SWATArea
    Public Function Report(ByVal aReportTable As DataTable) As String
        Dim lFormat As String = "###,##0.00"

        With aReportTable
            Dim lAreaTotals(.Columns.Count) As Double
            Dim lSb As New Text.StringBuilder
            Dim lStr As String = ""
            For lColumnIndex As Integer = 0 To .Columns.Count - 1
                lStr &= .Columns(lColumnIndex).ColumnName.PadLeft(12) & vbTab
            Next
            lSb.AppendLine(lStr.TrimEnd(vbTab))
            For lRowIndex As Integer = 0 To .Rows.Count - 1
                Dim lReportRow As DataRow = .Rows(lRowIndex)
                With lReportRow
                    lStr = .Item(0).ToString.PadLeft(12) & vbTab
                    For lColumnIndex As Integer = 1 To .ItemArray.GetUpperBound(0)
                        lStr &= DoubleToString(.Item(lColumnIndex), 12, lFormat, , , 10).PadLeft(12) & vbTab
                        lAreaTotals(lColumnIndex) += .Item(lColumnIndex)
                    Next
                End With
                lSb.AppendLine(lStr.TrimEnd(vbTab))
            Next
            lStr = "Totals".PadLeft(12) & vbTab
            For lColumnIndex As Integer = 1 To .Columns.Count - 1
                lStr &= DoubleToString(lAreaTotals(lColumnIndex), 12, lFormat, , , 10).PadLeft(12) & vbTab
            Next
            lSb.AppendLine(lStr.TrimEnd(vbTab))
            Logger.Dbg("AreaTotalReportComplete " & lAreaTotals(1))
            Return lSb.ToString
        End With
    End Function

    Public Function AggregateCrops(ByVal aInputTable As DataTable) As DataTable
        Dim lArea As Double = 0.0
        Dim lCornFraction As New atcCollection
        lCornFraction.Add("CCCC", 1.0)
        lCornFraction.Add("CCS1", 0.66667)
        lCornFraction.Add("CSC1", 0.5)
        lCornFraction.Add("CSS1", 0.33333)
        lCornFraction.Add("SCC1", 0.66667)
        lCornFraction.Add("SCS1", 0.5)
        lCornFraction.Add("SSC1", 0.33333)
        lCornFraction.Add("SSSC", 0.0)

        Dim lOutputTable As DataTable = aInputTable.Copy
        Dim lCornColumnIndex = lOutputTable.Columns.Count
        lOutputTable.Columns.Add("CORN")
        Dim lSoybColumnIndex = lOutputTable.Columns.Count
        lOutputTable.Columns.Add("SOYB")

        For Each lRow As DataRow In lOutputTable.Rows
            lRow(lCornColumnIndex) = 0.0
            lRow(lSoybColumnIndex) = 0.0
            For lColumnIndex As Integer = 2 To lOutputTable.Columns.Count - 2
                Dim lColumnName As String = lOutputTable.Columns(lColumnIndex).ColumnName
                Dim lColumnKeyIndex As Integer = lCornFraction.IndexFromKey(lColumnName)
                If lColumnKeyIndex >= 0 Then
                    lArea = lRow(lColumnIndex)
                    lRow(lCornColumnIndex) += lArea * lCornFraction(lColumnKeyIndex)
                    lRow(lSoybColumnIndex) += lArea * (1 - lCornFraction(lColumnKeyIndex))
                End If
            Next
        Next
        Return lOutputTable
    End Function
End Module
