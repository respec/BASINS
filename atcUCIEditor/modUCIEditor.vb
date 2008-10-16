
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

    Sub Initialize()
        Select Case pScenario
            Case "mono"
                pBaseDrive = "d:\"
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

        Dim lMsg As New atcUCI.HspfMsg("hspfmsg.mdb")
        Dim lUci As New atcUCI.HspfUci
        lUci.FastReadUciForStarter(lMsg, pScenarioName & ".uci")
        Dim lError As String = lUci.ErrorDescription
        If lError.Length > 0 Then
            Logger.Dbg("Error " & lError)
        Else
            Logger.Dbg("UCI " & lUci.Name & " Opened")

            'add changes to scematic block here
            Logger.Dbg("CollectionCount " & lUci.Connections.Count)
            For Each lConnection As atcUCI.HspfConnection In lUci.Connections
                Logger.Dbg(lConnection.Source.VolName & " " & lConnection.MFact & " " & lConnection.Target.VolName)
            Next

            lUci.Name = pScenarioNameNew & ".uci"
            lUci.Save()
            Logger.Dbg("UCI " & lUci.Name & " Saved")
        End If
    End Sub
End Module
