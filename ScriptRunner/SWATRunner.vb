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
        Dim lFormat As String = "###,##0.0"
        'get SubBasin info from database
        Dim lSubBasins As New atcCollection
        For Each lRow As DataRow In SwatObject.SwatInput.SubBasin.Table.Rows
            lSubBasins.Add(lRow.Item(1).ToString, lRow.Item(2)) 'id,area
        Next

        'summarize areas by subbasin
        Dim lAreaTotal As Double = 0.0
        Dim lArea As Double = 0.0
        Dim lSb As New Text.StringBuilder
        lSb.AppendLine("SubBasinId" & vbTab & "Area")
        For lSubBasinIndex As Integer = 0 To lSubBasins.Count - 1
            lArea = lSubBasins.Item(lSubBasinIndex)
            lAreaTotal += lArea
            lSb.AppendLine(lSubBasins.Keys(lSubBasinIndex) & vbTab & DoubleToString(lArea, , lFormat, , , 8).PadLeft(12))
        Next
        lSb.AppendLine("Total" & vbTab & DoubleToString(lAreaTotal, , lFormat, , , 8).PadLeft(12))
        lSb.AppendLine("")

        'summarize areas by subbasin/landuse
        Dim lLandUsePrev As String = ""
        Dim lSubBasinPrev As Integer = 0
        Dim lAreaLandUse As Double = 0.0
        lAreaTotal = 0.0

        lSb.AppendLine("SubBasinId" & vbTab & "LandUse" & vbTab & "Area")
        For Each lHruRow As DataRow In SwatObject.SwatInput.Hru.Table.Rows
            Dim lSubBasin As Integer = lHruRow.Item(1)
            Dim lLandUse As String = lHruRow.Item(3)
            If lSubBasinPrev > 0 AndAlso _
               (lSubBasin <> lSubBasinPrev OrElse _
                lLandUse <> lLandUsePrev) Then
                lSb.AppendLine(lSubBasinPrev & vbTab & lLandUsePrev & vbTab & DoubleToString(lAreaLandUse, , lFormat, , , 8).PadLeft(12))
                lAreaTotal += lAreaLandUse
                lAreaLandUse = 0.0
            End If
            lSubBasinPrev = lSubBasin
            lLandUsePrev = lLandUse
            lAreaLandUse += lHruRow(6) * lSubBasins.ItemByKey(lSubBasin.ToString)
        Next
        lSb.AppendLine("Total" & vbTab & "All" & vbTab & DoubleToString(lAreaTotal, , lFormat, , , 8).PadLeft(12))

        Return lSb.ToString
    End Function
End Module
