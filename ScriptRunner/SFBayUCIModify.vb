Imports System.Collections.ObjectModel
Imports MapWindow.Interfaces
Imports atcUCI
Imports atcUtility

Module SFBayUCIModify

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Dim lOrigFolder As String = "F:\SFBay\Scenario-mid\"
        Dim lNewFolder As String = "F:\SFBay\Task1\"
        ChDriveDir(lNewFolder)

        Dim lMsg As New atcUCI.HspfMsg
        lMsg.Open("hspfmsg.mdb")

        'build collection of folders/names
        Dim lUciNames As ReadOnlyCollection(Of String) = My.Computer.FileSystem.GetDirectories(lOrigFolder)

        Dim lHspfUci As New atcUCI.HspfUci

        For Each lUciName As String In lUciNames
            'make a copy of the file to begin working with
            FileCopy(lUciName & "\" & FilenameNoPath(lUciName) & ".uci", lNewFolder & FilenameNoPath(lUciName) & "\" & FilenameNoPath(lUciName) & ".uci")

            'open this uci
            lHspfUci.FastReadUciForStarter(lMsg, lNewFolder & FilenameNoPath(lUciName) & "\" & FilenameNoPath(lUciName) & ".uci")
            lHspfUci.Msg = lMsg

            'look through perlnds for Developed/Landscape perlnds
            Dim lDevPerlnds As New Collection(Of HspfOperation)
            For Each lOper As HspfOperation In lHspfUci.OpnBlks("PERLND").Ids
                If Trim(lOper.Description) = "Developed/Landscape" Then
                    'found one
                    lDevPerlnds.Add(lOper)
                End If
            Next

            'for each dev perlnd, add a new 'Road Buffer/Developed' perlnd
            For Each lDevPerlnd As HspfOperation In lDevPerlnds
                'add a new perlnd for 'Road Buffer/Developed'
                lHspfUci.AddOperation(lDevPerlnd.Name, lDevPerlnd.Id)
            Next

            'Dim lNewOper As New HspfOperation
            'lNewOper = lOper
            'lNewOper.Id = CInt(Mid(lOper.Id.ToString, 1, 2) & "6")
            'lNewOper.Description = "Road Buffer/Developed"
            'lHspfUci.OpnBlks(lOper.Name).Ids.Add(lNewOper)
            'lHspfUci.OpnSeqBlock.AddAfter(lNewOper, lOper.Id)

            'add appropriate connections

            'add special action?

            'save the uci
            lHspfUci.Save()
        Next
    End Sub
End Module
