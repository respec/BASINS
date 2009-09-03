Imports atcUCI
Imports atcUtility
Imports MapWinUtility

Module modHSPFTableBuilder
    Private g_Project As String = "WILL"
    Private g_BaseFolder As String

    Sub Initialize()
        Select Case g_Project
            Case "WILL"
                g_BaseFolder = "g:\Projects\TT_GCRP\ProjectsTT\Willamette\"
        End Select

    End Sub

    Sub main()
        Initialize()
        My.Computer.FileSystem.CurrentDirectory = g_BaseFolder
        Logger.StartToFile("parms\HSPFTableBuilderLog.txt", , False)

        Dim lPrmUpdTable As New atcTableDelimited
        lPrmUpdTable.Delimiter = vbTab
        lPrmUpdTable.OpenFile("parms\parms.txt")

        Dim lSlpRecTable As New atcTableDelimited
        lSlpRecTable.Delimiter = vbTab
        lSlpRecTable.OpenFile("parms\HruSummarizeSubBasin.txt")

        Dim lMsg As New HspfMsg("hspfmsg.mdb")
        Dim lUci As New HspfUci

        Try
            lUci.FastReadUciForStarter(lMsg, "parms\" & g_Project & ".uci")
            Dim lError As String = lUci.ErrorDescription
            If lError.Length > 0 Then
                Logger.Dbg("Error " & lError)
            Else
                Logger.Dbg("UCI " & lUci.Name & " Opened")
                Dim lSlpRecFieldNumber() As Integer = {2, 3}
                Dim lSlpRecFieldOperation() As String = {"=", "="}
                Dim lSlpRecFieldValue(1) As String
                Dim lPrmUpdFieldNumber() As Integer = {3, 4}
                Dim lPrmUpdFieldOperation() As String = {"=", "="}
                Dim lPrmUpdFieldValue(1) As String
                For Each lOperation As atcUCI.HspfOperation In lUci.OpnSeqBlock.Opns
                    If lOperation.Name = "PERLND" Then
                        Dim lMetSegmentComment As String = lOperation.MetSeg.Comment
                        Dim lMetSegmentName As String = lMetSegmentComment.Substring(lMetSegmentComment.Length - 8)
                        Dim lLandUseName As String = lOperation.Tables("GEN-INFO").Parms(0).Value
                        Dim lSlopeReclassValue As Integer = 1
                        lSlpRecFieldValue(0) = lMetSegmentName
                        lSlpRecFieldValue(1) = lLandUseName
                        If lSlpRecTable.FindMatch(lSlpRecFieldNumber, lSlpRecFieldOperation, lSlpRecFieldValue) Then
                            lSlopeReclassValue = lSlpRecTable.Value(4)
                            Logger.Dbg("Met,LU,SlopeReclass:" & lMetSegmentName & ":" & lLandUseName & ":" & lSlopeReclassValue)
                            lPrmUpdFieldValue(0) = lLandUseName
                            lPrmUpdFieldValue(1) = lSlopeReclassValue
                            Dim lRecordStart As Integer = 1
                            Dim lRecordsFound As Integer = 0
                            While lPrmUpdTable.FindMatch(lPrmUpdFieldNumber, lPrmUpdFieldOperation, lPrmUpdFieldValue, , lRecordStart)
                                lRecordsFound += 1
                                Dim lTableName As String = lPrmUpdTable.Value(1)
                                Dim lParmName As String = lPrmUpdTable.Value(2)
                                Dim lParmValue As String = lPrmUpdTable.Value(5)
                                lRecordStart = lPrmUpdTable.CurrentRecord + 1
                                For Each lTable As HspfTable In lOperation.Tables
                                    If lTable.Name = lTableName Then
                                        For Each lParm As HspfParm In lTable.Parms
                                            If lParmName = lParm.Name Then
                                                Logger.Dbg("Update:" & lTableName & ":" & lParmName & ":" & lParm.Value & ":" & lParmValue)
                                                lParm.Value = lParmValue
                                                Exit For
                                            End If
                                        Next
                                        Exit For
                                    End If
                                Next
                            End While
                            If lRecordsFound = 0 Then
                                Logger.Dbg("NoParmUpdatesFor " & lMetSegmentName & ":" & lLandUseName)
                            End If
                        Else
                            Logger.Dbg("NoSlopeReclassFor " & lMetSegmentName & ":" & lLandUseName)
                        End If
                    End If
                Next lOperation
                lUci.Name = lUci.Name.ToUpper.Replace(g_Project, g_Project.ToLower & "Rev")
                lUci.Save()
            End If
        Catch
        End Try

    End Sub
End Module
