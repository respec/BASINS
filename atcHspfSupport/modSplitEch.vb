Imports System
Imports System.Windows.Forms
Imports System.Collections.Specialized
Imports Microsoft.VisualBasic
Imports MapWinGIS
Imports MapWindow.Interfaces
Imports MapWinUtility
Imports atcUtility
Imports atcData

'SplitEch.vb 
'Created by Jack Kittle (jlkittle@aquaterra.com)
'Date 10 Oct 2006

Public Module SplitEch
    Private Const pDirPath As String = "d:\cbp_working\output\combined"
    Private pUciName As String = "base"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.Dbg("SplitEch:Start")
        Logger.Dbg(" DirPath '" & pDirPath & "'")
        ChDriveDir(pDirPath)
        Logger.Dbg(" CurDir:" & CurDir)

        Dim lEchString As String = WholeFileString(pUciName & ".ech")
        Logger.dbg("Length " & lEchString.Length)
        MkDirPath("split")
        ChDriveDir("split")

	  Try
            Kill(pUciName & "*.ech")
        Catch
        End Try

        Dim lStrPos As Integer
        Dim lStrBeg As String

        'errors/warnings anywhere
        lStrPos = 0
        ProcessBlockLoop(lEchString, lStrPos, _
                         "ERROR/WARNING ID:", "*****" & vbCrLf & " " & vbCrLf, _
                         pUciName & ".Error.", ".ech", 6)

        lStrPos = 0
        Dim lStrEnd As String = "FINISHED PROCESSING OPN SEQUENCE BLOCK"
        SaveFileString(pUciName & ".RiIntroduction.ech", GetBlock(lEchString, lStrPos, lStrEnd))

        lStrBeg = "PROCESSING FTABLES BLOCK"
        GetBlock(lEchString, lStrPos, lStrBeg)
        lStrPos -= lStrBeg.Length
        lStrEnd = "FINISHED PROCESSING FTABLES BLOCK"
        SaveFileString(pUciName & ".RiFtables.ech", GetBlock(lEchString, lStrPos, lStrEnd))

        lStrBeg = "PROCESSING MASS LINK BLOCK"
        GetBlock(lEchString, lStrPos, lStrBeg)
        lStrPos -= lStrBeg.Length
        lStrEnd = "FINISHED PROCESSING MASS LINK BLOCK"
        SaveFileString(pUciName & ".RiMassLink.ech", GetBlock(lEchString, lStrPos, lStrEnd))

        ProcessBlockLoop(lEchString, lStrPos, _
                         "PROCESSING PERVIOUS LAND-SEGMENT NO:", _
                         "FINISHED PROCESSING PERVIOUS LAND-SEGMENT NO.", _
                         pUciName & ".PERLND.", ".RiParms.ech")
        ProcessBlockLoop(lEchString, lStrPos, _
                         "PROCESSING IMPERVIOUS LAND-SEGMENT NO:", _
                         "FINISHED PROCESSING IMPERVIOUS LAND-SEGMENT NO.", _
                         pUciName & ".IMPLND.", ".RiParms.ech")
        ProcessBlockLoop(lEchString, lStrPos, _
                         "PROCESSING RCHRES NO:", _
                         "FINISHED PROCESSING RCHRES NO.", _
                         pUciName & ".RCHRES.", ".RiParms.ech")
        ProcessBlockLoop(lEchString, lStrPos, _
                         "PROCESSING PLTGEN OPERATION NO.", _
                         "FINISHED PROCESSING PLTGEN OPERATION NO.", _
                         pUciName & ".PLTGEN.", ".RiParms.ech")

        lStrBeg = "PROCESSING SPEC-ACTIONS BLOCK"
        GetBlock(lEchString, lStrPos, lStrBeg)
        lStrPos -= lStrBeg.Length
        lStrEnd = "FINISHED PROCESSING SPEC-ACTIONS BLOCK"
        SaveFileString(pUciName & ".RiSpecialActions.ech", GetBlock(lEchString, lStrPos, lStrEnd))

        lStrBeg = "PROCESSING BLOCKS CONTAINING TIME SERIES LINKAGES"
        GetBlock(lEchString, lStrPos, lStrBeg)
        lStrPos -= lStrBeg.Length
        lStrEnd = "FINISHED ALLOCATING INPAD ROWS AND GENERATING TSGET/TSPUT INSTRUCTIONS"
        SaveFileString(pUciName & ".RiTimser.ech", GetBlock(lEchString, lStrPos, lStrEnd))

        ProcessBlockLoop(lEchString, lStrPos, _
                         "TIMESERIES USED BY OPERATION   PERLND", _
                         "++++++++++++++++++++++", _
                         pUciName & ".PERLND.", ".RiTimser.ech")
        ProcessBlockLoop(lEchString, lStrPos, _
                         "TIMESERIES USED BY OPERATION   IMPLND", _
                         "++++++++++++++++++++++", _
                         pUciName & ".IMPLND.", ".RiTimser.ech")
        ProcessBlockLoop(lEchString, lStrPos, _
                         "TIMESERIES USED BY OPERATION   RCHRES", _
                         "++++++++++++++++++++++", _
                         pUciName & ".RCHRES.", ".RiTimser.ech")
        ProcessBlockLoop(lEchString, lStrPos, _
                         "TIMESERIES USED BY OPERATION   PLTGEN", _
                         "++++++++++++++++++++++", _
                         pUciName & ".PLTGEN.", ".RiTimser.ech")
        'may miss last PLTGEN

        lStrBeg = "BEGIN GENERATION OF TSGET/TSPUT INSTRUCTIONS FOR OPERATIONS"
        GetBlock(lEchString, lStrPos, lStrBeg)
        lStrPos -= lStrBeg.Length
        lStrEnd = "INTERPRETATION OF RUN DATA SET COMPLETE"
        SaveFileString(pUciName & ".RiDone.ech", GetBlock(lEchString, lStrPos, lStrEnd))

        Dim lExecStartPos As Integer = lStrPos
        'special actions during run
        ProcessBlockLoop(lEchString, lStrPos, _
                         "SPEC-ACT:", _
                         vbCrLf & vbCrLf, _
                         pUciName & ".", ".ExSpecAct.ech", _
                         12)

        Logger.Dbg("AllDone")
    End Sub

    Private Sub ProcessBlockLoop(ByRef aStr As String, _
                                 ByVal aStrPos As Integer, _
                                 ByRef aStrBeg As String, _
                                 ByRef aStrEnd As String, _
                                 ByRef aFilePrefix As String, _
                                 ByVal aFileSuffix As String, _
                                 Optional ByVal aIdLen As Integer = 5)
        Logger.Dbg("ProcessBlockLoop " & aStrPos & " of " & aStr.Length & " Begin '" & aStrBeg & "' End '" & aStrEnd & "'")
        Dim lStr As String = GetBlock(aStr, aStrPos, aStrBeg)
        While lStr.Length > 0
            Dim lId As String = aStr.Substring(aStrPos, aIdLen)
            Dim lFileName As String
            If aFilePrefix = pUciName & "." Then 'special action, get operation name & number
                lFileName = aFilePrefix & lId.Substring(1, 6) & "." & lId.Substring(7, 5).Trim & aFileSuffix
            Else
                lFileName = aFilePrefix & lId.Trim & aFileSuffix
            End If
            'Logger.Dbg("Id " & lId & " Start " & aStrPos & " FileName '" & lFileName & "'")
            Dim lSafeFileName As String = SafeFilename(lFileName)
            If lSafeFileName <> lFileName Then
                'Logger.Dbg(" SafeFileName '" & lSafeFileName & "'")
            End If
            Dim lStrPos As Integer
            If aStrBeg.Substring(0, 5) = "ERROR" Then
                lStrPos = aStrPos - aStrBeg.Length - 178
            Else
                lStrPos = aStrPos - aStrBeg.Length
            End If
            AppendFileString(lSafeFileName, GetBlock(aStr, lStrPos, aStrEnd))
            lStr = GetBlock(aStr, aStrPos, aStrBeg)
        End While
    End Sub

    Private Function GetBlock(ByRef aStr As String, _
                              ByRef aStrPos As Integer, _
                              ByVal aStrEnd As String) As String
        Dim lStrPosEnd As Integer = aStr.IndexOf(aStrEnd, aStrPos)
        If lStrPosEnd > 0 Then
            Dim lStrPos As Integer = aStrPos
            aStrPos = lStrPosEnd + aStrEnd.Length
            'Logger.Dbg("Found '" & aStrEnd & "' Start " & lStrPos & " End " & aStrPos)
            Return aStr.Substring(lStrPos, aStrPos - lStrPos)
        Else
            Logger.Dbg("StringNotFound '" & aStrEnd & "' Starting at " & aStrPos)
            Return ""
        End If
    End Function
End Module