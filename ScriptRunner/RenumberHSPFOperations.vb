Imports System.Collections.ObjectModel
Imports MapWindow.Interfaces
Imports atcUCI
Imports atcUtility

Module RenumberHSPFOperations

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)

        'set current working directory
        Dim lFolder As String = "C:\BASINS\modelout\Upatoi\"
        ChDriveDir(lFolder)

        'set hspf message mdb (needs to be set to use HspfUci object
        Dim lMsg As New atcUCI.HspfMsg
        lMsg.Open("hspfmsg.mdb")

        'create a new HspfUci object and set the message mdb in it
        Dim lHspfUci As New atcUCI.HspfUci
        lHspfUci.Msg = lMsg

        'read the UCI and populate the object
        lHspfUci.FastReadUciForStarter(lMsg, "test.uci")

        RenumberOperation(lHspfUci, "RCHRES", 46, 100)   'find RCHRES 46 and assign it the operation number 100 

        'save the UCI with the renumbered operations
        lHspfUci.Save()
    End Sub

    Private Sub RenumberOperation(ByVal aHspfUci As HspfUci, ByVal aSourceName As String, ByVal aSourceId As Integer, ByVal aTargetId As Integer)
        Dim lOperation As HspfOperation
        lOperation = aHspfUci.OpnBlks(aSourceName).OperFromID(aSourceId)
        lOperation.Id = aTargetId
        lOperation.FTable.Id = aTargetId
        lOperation.Tables("HYDR-PARM2").Parms("FTBUCI").Value = aTargetId
    End Sub

End Module
