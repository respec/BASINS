Imports MapWindow.Interfaces
Imports MapWinUtility

Imports atcUtility
Imports SwatObject

Module SWATRunner
    Private Const pInputPath As String = "D:\Basins\data\SWATOutput\UM\baseline30jack"
    Private Const pSWATGDB As String = "SWAT2005.mdb"
    Private Const pOutGDB As String = "baseline90.mdb"
    Private Const pOutputFolder = pInputPath & "\Scenarios\Default\TxtInOut"
    Private Const pSWATExe As String = pOutputFolder & "\swat2005.exe" 'local copy with input data
    'Private Const pSWATExe As String = "C:\Program Files\SWAT 2005 Editor\swat2005.exe"
    Private pSwatOutput As Text.StringBuilder
    Private pSwatError As Text.StringBuilder

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        ChDriveDir(pInputPath)
        Dim lLogFileName As String = Logger.FileName

        'log for swat runner
        Logger.StartToFile(pInputPath & "\logs\SWATRunner.log", , , True)

        Logger.Dbg("SWATPreprocess-UpdateParametersAsRequested")
        'With SwatInput - does not work???
        SwatInput.Initialize(pSWATGDB, pOutGDB, pOutputFolder)
        Logger.Dbg("HRU RowCount:" & SwatObject.SwatInput.Hru.Table.Rows.Count)
        'End With

        Logger.Dbg("SWATSummarizeInput")
        SaveFileString(pInputPath & "\logs\AreaReport.txt", SWATArea.Report)

        Logger.Dbg("SwatModelRunStartIn " & pOutputFolder)
        Try 'to run swat
            Dim lSwatProcess As New System.Diagnostics.Process
            With lSwatProcess.StartInfo
                .FileName = pSWATExe
                .WorkingDirectory = pOutputFolder
                .CreateNoWindow = True
                .UseShellExecute = False
                .RedirectStandardOutput = True
                pSwatOutput = New Text.StringBuilder
                AddHandler lSwatProcess.OutputDataReceived, AddressOf OutputDataHandler
                .RedirectStandardError = True
                pSwatError = New Text.StringBuilder
                AddHandler lSwatProcess.ErrorDataReceived, AddressOf ErrorDataHandler
            End With
            lSwatProcess.Start()
            lSwatProcess.BeginErrorReadLine()
            lSwatProcess.BeginOutputReadLine()
            lSwatProcess.WaitForExit()
            SaveFileString(pInputPath & "\logs\SWATError.txt", pSwatError.ToString)
            SaveFileString(pInputPath & "\logs\SWATOutput.txt", pSwatOutput.ToString)
        Catch lEx As ApplicationException
            Logger.Dbg("SwatRunProblem " & lEx.Message)
        End Try
        Logger.Dbg("SwatModelRunDone")

        Logger.Dbg("SwatPostProcessingDone")

        'back to basins log
        Logger.StartToFile(lLogFileName, True, False, True)
    End Sub

    Private Sub OutputDataHandler(ByVal aSendingProcess As Object, _
                                  ByVal aOutLine As DataReceivedEventArgs)
        If Not String.IsNullOrEmpty(aOutLine.Data) Then
            pSwatOutput.Append(Environment.NewLine + "  " + aOutLine.Data)
        End If
    End Sub

    Private Sub ErrorDataHandler(ByVal aSendingProcess As Object, _
                                 ByVal aErrLine As DataReceivedEventArgs)
        If Not String.IsNullOrEmpty(aErrLine.Data) Then
            pSwatError.Append(Environment.NewLine + "  " + aErrLine.Data)
        End If
    End Sub
End Module

Module SWATArea
    Public Function Report() As String
        Dim lFormat As String = "###,##0.00"
        'get SubBasin info from database
        Dim lSubBasins As New atcCollection
        For Each lRow As DataRow In SwatObject.SwatInput.SubBasin.Table.Rows
            lSubBasins.Add(lRow.Item(1).ToString, lRow.Item(2)) 'id,area
        Next

        Dim lLandUses As New atcCollection
        For Each lRow As DataRow In SwatObject.SwatInput.Hru.LandUses.Rows
            lLandUses.Add(lRow.Item(0))
        Next

        Dim lArea As Double = 0.0
        Dim lReportTable As New DataTable
        With lReportTable
            .Columns.Add("SubBasinId", "".GetType)
            .Columns.Add("Area", lArea.GetType)
            For lLandUseIndex As Integer = 0 To lLandUses.Count - 1
                .Columns.Add(lLandUses.ItemByIndex(lLandUseIndex), lArea.GetType)
            Next
            'summarize areas by subbasin
            Dim lAreaTotal As Double = 0.0
            For lSubBasinIndex As Integer = 0 To lSubBasins.Count - 1
                lArea = lSubBasins.Item(lSubBasinIndex)
                lAreaTotal += lArea
                Dim lReportRow As DataRow = .NewRow
                With lReportRow
                    .Item(0) = lSubBasinIndex + 1
                    .Item(1) = lArea
                    For lLanduseindex As Integer = 0 To lLandUses.Count - 1
                        .Item(lLanduseindex + 2) = 0.0
                    Next
                End With
                .Rows.Add(lReportRow)
            Next

            'summarize areas by subbasin/landuse
            For Each lHruRow As DataRow In SwatObject.SwatInput.Hru.Table.Rows
                Dim lSubBasin As Integer = lHruRow.Item(1)
                Dim lLandUse As String = lHruRow.Item(3)
                Dim lReportRow As DataRow = .Rows(lSubBasin - 1)
                lReportRow.Item(lHruRow.Item(3)) += lHruRow.Item(6) * lReportRow.Item(1)
            Next

            Dim lAreaTotals(.Columns.Count)
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
            Logger.Dbg("AreaTotalReportComplete " & lAreaTotals(1) & " " & lAreaTotal)
            Return lSb.ToString
        End With
    End Function
End Module
