
Imports MapWinUtility
Imports atcUtility
Imports atcData
Imports atcWDM
Imports System.Collections.Specialized

Module modUCIEditor
    Private pBaseDrive As String
    'Private pScenario As String = "mono"
    Private pScenario As String = "hspf10"
    Private pBaseDir As String
    Private pOutputDir As String
    Private pScenarioName As String
    Private pScenarioNameNew As String
    Private pUpdateFileName As String = "C:\test\HSPF\Updates.txt"

    Sub Initialize()
        Select Case pScenario
            Case "mono"
                pBaseDrive = "c:\"
                pBaseDir = pBaseDrive & "mono_luChange\"
                pOutputDir = pBaseDir & "output\"
                pScenarioName = "base"
                pScenarioNameNew = "LU_Ch"
            Case "hspf10"
                pBaseDrive = "c:\"
                pBaseDir = pBaseDrive & "test\HSPF\"
                pOutputDir = pBaseDir & "output\"
                pScenarioName = "test10"
                pScenarioNameNew = "test10Rev"
                pUpdateFileName = "updates.txt"
            Case "hspf12"
                pBaseDrive = "c:\"
                pBaseDir = pBaseDrive & "test\HSPF\"
                pOutputDir = pBaseDir & "output\"
                pScenarioName = "test12"
                pScenarioNameNew = "test12Rev"
        End Select
    End Sub

    Sub main()
        Initialize()
        Logger.StartToFile(pOutputDir & "UCIEditor.log")
        ChDriveDir(pBaseDir)
        Logger.Dbg("BaseDir " & CurDir())

        Dim lUpdateTable As New atcTableDelimited
        If pUpdateFileName.Length > 0 Then
            lUpdateTable.Delimiter = ";"
            lUpdateTable.NumHeaderRows = 3
            lUpdateTable.OpenFile(pUpdateFileName)
        End If

        'Set number of lines in the header as 3

        Dim lstrtmp As String = ""

        With lUpdateTable

            For i As Integer = 1 To .NumRecords
                .CurrentRecord = i
                For j As Integer = 1 To .NumFields
                    lstrtmp &= .Value(j) & "-"
                Next
                'MsgBox("Record " & i & " = " & lstrtmp.Trim(("-")))
                lstrtmp = ""
            Next
            Try
                System.IO.File.Open(pUpdateFileName, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.None)
                MsgBox("File " & pUpdateFileName & " is closed.")
            Catch ex As Exception
                If ex.Message.Contains("file in use") Then
                    MsgBox("Ex: " & ex.ToString)
                End If
                MsgBox("File " & pUpdateFileName & " is still open.")
            End Try
        End With

        Dim lMsg As New atcUCI.HspfMsg("hspfmsg.mdb")
        Dim lUci As New atcUCI.HspfUci
        lUci.FastReadUciForStarter(lMsg, pScenarioName & ".uci")
        Dim lError As String = lUci.ErrorDescription
        If lError.Length > 0 Then
            Logger.Dbg("Error " & lError)
        Else
            Logger.Dbg("UCI " & lUci.Name & " Opened")

            'add changes to scematic block here
            Logger.Dbg("ConnectionCount " & lUci.Connections.Count)
            For Each lConnection As atcUCI.HspfConnection In lUci.Connections
                Logger.Dbg(lConnection.Source.VolName & " " & lConnection.Source.VolId & " " & _
                           lConnection.MFact & " " & _
                           lConnection.Target.VolName & " " & lConnection.Target.VolId)
            Next

            lUci.Name = pScenarioNameNew & ".uci"
            lUci.Save()
            Logger.Dbg("UCI " & lUci.Name & " Saved")
        End If
        Logger.Flush()
    End Sub
End Module
