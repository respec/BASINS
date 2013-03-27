Option Strict Off
Option Explicit On
Module HassLibs
    '##MODULE_REMARKS Copyright 2001-3 AQUA TERRA Consultants - Royalty-free use permitted under open source license
    Declare Sub F90_W99OPN Lib "hass_ent.dll" ()
    Declare Sub F90_WDBFIN Lib "hass_ent.dll" ()
    Declare Function F90_WDBOPN Lib "hass_ent.dll" (ByRef l As Integer, ByVal s As String, ByVal i As Short) As Integer
    Declare Sub F90_WMSGTW_XX Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer)
    Declare Sub F90_WMSGTT_XX Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer)
    Declare Sub F90_WMSGTH Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer)
    Declare Sub F90_GTNXKW_XX Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer)
    Declare Sub F90_XTINFO_XX Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer)
    Declare Sub F90_PUTOLV Lib "hass_ent.dll" (ByRef l As Integer)
    Declare Sub F90_MSGUNIT Lib "hass_ent.dll" (ByRef l As Integer)
    Declare Function F90_WDFLCL Lib "hass_ent.dll" (ByRef aWdmUnit As Integer) As Integer
    Declare Function F90_INQNAM Lib "hass_ent.dll" (ByVal aName As String, ByVal aNameLen As Short) As Integer

    Public Sub F90_GTNXKW(ByRef Init As Integer, ByRef id As Integer, ByRef ckwd As String, ByRef kwdfg As Integer, ByRef contfg As Integer, ByRef retid As Integer)
        Dim ikwd(12) As Integer

        Call F90_GTNXKW_XX(Init, id, ikwd(0), kwdfg, contfg, retid)
        Call NumChr(12, ikwd, ckwd)

    End Sub

    Private Sub NumChr(ByRef ilen As Integer, ByRef inam() As Integer, ByRef outstr As String)

        Dim i As Integer

        outstr = ""
        For i = 0 To ilen - 1 'added "- 1" 8/16/2002 Mark Gray
            If inam(i) > 0 Then
                outstr = outstr & Chr(inam(i))
            End If
        Next i
        outstr = RTrim(outstr)

    End Sub
    Private Sub NumChrA(ByRef icnt As Integer, ByRef ilen As Integer, ByRef inam() As Integer, ByRef outstr() As String)

        Dim j, i, p As Integer
        Dim s As String = ""
        Dim jnam() As Integer

        ReDim jnam(ilen)
        p = 0
        For i = 0 To icnt - 1
            For j = 0 To ilen - 1
                jnam(j) = inam(p + j)
            Next j
            Call NumChr(ilen, jnam, s)
            outstr(i) = s
            p = p + ilen
        Next i

    End Sub

    Sub F90_WMSGTW(ByRef id As Integer, ByRef s As String)
        Dim i(48) As Integer

        Call F90_WMSGTW_XX(id, i(0))
        Call NumChr(48, i, s)

    End Sub
    Sub F90_WMSGTT(ByRef wdmsfl As Integer, ByRef dsn As Integer, ByRef gnum As Integer, ByRef initfg As Integer, ByRef olen As Integer, ByRef cont As Integer, ByRef obuff As String)
        Dim lobuff(256) As Integer

        Call F90_WMSGTT_XX(wdmsfl, dsn, gnum, initfg, olen, cont, lobuff(0))

        Call NumChr(olen, lobuff, obuff)
    End Sub

    Sub F90_XTINFO(ByRef omcode As Integer, ByRef tnum As Integer, ByRef uunits As Integer, ByRef estflg As Integer, ByRef lnflds As Integer, ByRef lscol() As Integer, ByRef lflen() As Integer, ByRef lftyp As String, ByRef lapos() As Integer, ByRef limin() As Integer, ByRef limax() As Integer, ByRef lidef() As Integer, ByRef lrmin() As Single, ByRef lrmax() As Single, ByRef lrdef() As Single, ByRef lnmhdr As Integer, ByRef hdrbuf() As String, ByRef lfdnam() As String, ByRef isect As Integer, ByRef irept As Integer, ByRef retcod As Integer)
        Dim ihdrbuf(780) As Integer
        Dim ilftyp(30) As Integer
        Dim ifdnam(360) As Integer

        Call F90_XTINFO_XX(omcode, tnum, uunits, estflg, lnflds, lscol(0), lflen(0), ilftyp(0), lapos(0), limin(0), limax(0), lidef(0), lrmin(0), lrmax(0), lrdef(0), lnmhdr, ihdrbuf(0), ifdnam(0), isect, irept, retcod)

        Call NumChr(30, ilftyp, lftyp)
        Call NumChrA(10, 78, ihdrbuf, hdrbuf)
        Call NumChrA(30, 12, ifdnam, lfdnam)

    End Sub
End Module