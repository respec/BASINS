Imports atcUtility
Imports atcData
Imports atcUCI
Imports HspfSupport
Imports MapWinUtility
Imports MapWindow.Interfaces
Imports System.Collections.Specialized

Module HSPFAreaSummary
    Private pTestPath As String = "G:\Projects\Mono"
    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        ChDriveDir(pTestPath)

        Dim lMsg As New atcUCI.HspfMsg
        lMsg.Open("hspfmsg.mdb")


        Dim lUciNames As NameValueCollection = Nothing
        AddFilesInDir(lUciNames, pTestPath, True, "base.uci")

        Dim lSB As New Text.StringBuilder
        Try
            For Each lUciName As String In lUciNames
                Logger.Dbg("UCI " & lUciName)
                Dim lLandUseArea As New atcCollection
                Dim lHspfUci As New atcUCI.HspfUci
                lHspfUci.FastReadUciForStarter(lMsg, lUciName)
                For Each lRchresOpn As HspfOperation In lHspfUci.OpnBlks("RCHRES").Ids
                    For Each lConnection As HspfConnection In lRchresOpn.Sources
                        If lConnection.Source.Opn IsNot Nothing Then
                            Dim lLandUse As String = lConnection.Source.Opn.Description
                            If Not lLandUse.StartsWith("PM") Then
                                lLandUseArea.Increment(lLandUse, lConnection.MFact)
                            End If
                        End If
                    Next
                Next
                For lLandUseIndex As Integer = 0 To lLandUseArea.Count - 1
                    Logger.Dbg(lLandUseArea.Keys(lLandUseIndex) & " " & lLandUseArea.ItemByIndex(lLandUseIndex))
                    Dim lKey As String = lLandUseArea.Keys(lLandUseIndex)
                    lSB.AppendLine(lUciName.Replace(pTestPath.ToLower, "") & vbTab & lKey.Substring(0, 6) & vbTab & lKey.Substring(6).Trim & vbTab & lLandUseArea.ItemByIndex(lLandUseIndex))
                Next
                lHspfUci = Nothing
            Next
            SaveFileString("AreaSummary.txt", lSB.ToString)
        Catch lEx As Exception
            Logger.Dbg(lEx.ToString)
        End Try
    End Sub
End Module
