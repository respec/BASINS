Imports atcData
Imports atcUtility
Imports atcSeasons
Imports MapWindow.Interfaces
Imports MapWinUtility

Imports Microsoft.VisualBasic
Imports System.Collections
Imports System.IO
Imports System.Windows.Forms
Imports System.Array

Public Module ScriptSynop
    Private Const pTestPath As String = "C:\BASINSmet\Logs\"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        ChDriveDir(pTestPath)
        Logger.Dbg(" CurDir:" & CurDir)

        Dim lOutFile As String = "SKYCOND.txt"
        SaveFileString(lOutFile, "Entry" & vbCrLf)

        Dim lPos As Integer
        Dim lSkyLog As String = WholeFileString("SkyCond2Tenths.log")
        While lSkyLog.Length > 0
            lPos = lSkyLog.IndexOf("Found timeseries OBSERVED")
            If lPos > 0 Then
                AppendFileString(lOutFile, lSkyLog.Substring(lPos + 26, 6) & vbCrLf)
                lSkyLog = lSkyLog.Substring(lPos + 33)
            Else
                Exit While
            End If
        End While
        AppendFileString(lOutFile, " Done" & vbCrLf)
        Application.Exit()
    End Sub
End Module
