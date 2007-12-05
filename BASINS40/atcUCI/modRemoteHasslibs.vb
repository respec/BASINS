Option Strict Off
Option Explicit On

Imports atcUtility

Module modRemoteHasslibs
    Public Sub REM_GLOBLK(ByRef aUci As HspfUci, _
                          ByRef aSDatim() As Integer, ByRef aEDatim() As Integer, _
                          ByRef aOutLev As Integer, ByRef aSpOut As Integer, _
                          ByRef aRunFg As Integer, ByRef aEmFg As Integer, _
                          ByRef aRunInf As String)
        aUci.SendHspfMessage("GLOBLK")
        Dim lMsg As String = aUci.WaitForChildMessage
        For i As Integer = 0 To 5
            aSDatim(i) = CInt(StrRetRem(lMsg))
        Next
        For i As Integer = 0 To 5
            aEDatim(i) = CInt(StrRetRem(lMsg))
        Next
        aOutLev = CInt(StrRetRem(lMsg))
        aSpOut = CInt(StrRetRem(lMsg))
        aRunFg = CInt(StrRetRem(lMsg))
        aEmFg = CInt(StrRetRem(lMsg))
        aRunInf = lMsg
    End Sub

    Public Sub REM_GLOPRMI(ByRef myUci As HspfUci, ByRef ival As Integer, ByRef parmname As String)
        Dim M As String

        myUci.SendHspfMessage("GLOPRMI " & parmname)
        M = myUci.WaitForChildMessage
        ival = -999
        If Len(Trim(M)) > 0 Then
            If IsNumeric(M) Then
                ival = CShort(M)
            End If
        End If
    End Sub

    Public Sub REM_XBLOCK(ByRef myUci As HspfUci, ByRef blkno As Integer, ByRef init As Integer, ByRef retkey As Integer, ByRef cbuff As String, ByRef retcod As Integer)
        Dim M As String
        Dim i As Integer

        myUci.SendHspfMessage("XBLOCK " & blkno & " " & init)
        M = myUci.WaitForChildMessage
        'Debug.Print blkno & "=" & M
        blkno = CInt(StrRetRem(M))
        init = CInt(StrRetRem(M))
        retkey = CInt(StrRetRem(M))
        i = InStr(M, " ")
        retcod = CInt(Left(M, i - 1))
        cbuff = Right(M, Len(M) - i)
    End Sub

    Public Sub REM_XBLOCKEX(ByRef myUci As HspfUci, ByRef blkno As Integer, ByRef init As Integer, ByRef retkey As Integer, ByRef cbuff As String, ByRef rectyp As Integer, ByRef retcod As Integer)
        Dim M As String
        Dim i As Integer

        myUci.SendHspfMessage("XBLOCKEX " & blkno & " " & init & " " & retkey)
        M = myUci.WaitForChildMessage
        Debug.Print(blkno & "=" & M)
        blkno = CInt(StrRetRem(M))
        init = CInt(StrRetRem(M))
        retkey = CInt(StrRetRem(M))
        rectyp = CInt(StrRetRem(M))
        i = InStr(M, " ")
        retcod = CInt(Left(M, i - 1))
        cbuff = Right(M, Len(M) - i)
    End Sub

    Public Sub REM_GTNXKW(ByRef myUci As HspfUci, ByRef init As Integer, ByRef Id As Integer, ByRef ckwd As String, ByRef kwdfg As Integer, ByRef contfg As Integer, ByRef retid As Integer)
        Dim M As String

        myUci.SendHspfMessage("GTNXKW " & init & " " & Id)
        M = myUci.WaitForChildMessage
        init = CInt(StrRetRem(M))
        Id = CInt(StrRetRem(M))
        kwdfg = CInt(StrRetRem(M))
        contfg = CInt(StrRetRem(M))
        retid = CInt(StrRetRem(M))
        ckwd = M
    End Sub

    Public Sub REM_GETOCR(ByRef myUci As HspfUci, ByRef itype As Integer, ByRef noccur As Integer)
        Dim M As String

        myUci.SendHspfMessage("GETOCR " & itype)
        M = myUci.WaitForChildMessage
        itype = CInt(StrRetRem(M))
        noccur = CInt(M)
    End Sub

    Public Sub REM_XTABLEEX(ByRef myUci As HspfUci, ByRef OmCode As Integer, ByRef tabno As Integer, ByRef uunits As Integer, ByRef init As Integer, ByRef addfg As Integer, ByRef Occur As Integer, ByRef retkey As Integer, ByRef cbuff As String, ByRef rectyp As Integer, ByRef retcod As Integer)
        Dim M As String
        Dim i As Integer

        myUci.SendHspfMessage("XTABLEEX " & OmCode & " " & tabno & " " & uunits & " " & init & " " & addfg & " " & Occur & " " & retkey)
        M = myUci.WaitForChildMessage
        retkey = CInt(StrRetRem(M))
        rectyp = CInt(StrRetRem(M))
        i = InStr(M, " ")
        retcod = CInt(Left(M, i - 1))
        cbuff = Right(M, Len(M) - i)

    End Sub
End Module