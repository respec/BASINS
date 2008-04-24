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
