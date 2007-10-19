Option Strict Off
Option Explicit On
<System.Runtime.InteropServices.ProgId("HspfGlobalBlk_NET.HspfGlobalBlk")> Public Class HspfGlobalBlk
    'Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license

    Private pRunInf As HSPFParm ' run information
    Private pSDate(5) As Integer 'starting date
    Private pEDate(5) As Integer 'ending date
    Private pOutLev As HSPFParm 'run interp output level
    Private pSpOut As Integer 'special action output level
    Private pRunFg As Integer 'interp only(0) or interp and run(1)
    Private pEmFg As Integer 'english(1), metric(2) flag
    Private pIhmFg As Integer 'ihm flag (normal-0,IHM control-1)
    Private pComment As String
    Private pUci As HspfUci
    Private pEdited As Boolean

    Public Property Edited() As Boolean
        Get
            Edited = pEdited
        End Get
        Set(ByVal Value As Boolean)
            pEdited = Value
            If Value Then pUci.Edited = True
        End Set
    End Property

    Public Property Uci() As HspfUci
        Get
            Uci = pUci
        End Get
        Set(ByVal Value As HspfUci)
            pUci = Value
        End Set
    End Property

    Public ReadOnly Property Caption() As String
        Get
            Caption = "Global Block"
        End Get
    End Property


    Public Property Comment() As String
        Get
            Comment = pComment
        End Get
        Set(ByVal Value As String)
            pComment = Value
        End Set
    End Property

    Public ReadOnly Property EditControlName() As String
        Get
            EditControlName = "ATCoHspf.ctlGlobalBlkEdit"
        End Get
    End Property

    Public Property RunInf() As HSPFParm
        Get
            RunInf = pRunInf
        End Get
        Set(ByVal Value As HSPFParm)
            pRunInf = Value
            Update()
        End Set
    End Property

    Public Property SDate(ByVal Index As Integer) As Integer
        Get
            SDate = pSDate(Index)
        End Get
        Set(ByVal Value As Integer)
            pSDate(Index) = Value
            Update()
        End Set
    End Property

    Public ReadOnly Property SDateJ() As Integer
        Get
            Return atcUtility.modDate.Date2J(pSDate)
        End Get
    End Property

    Public Property EDate(ByVal Index As Integer) As Integer
        Get
            EDate = pEDate(Index)
        End Get
        Set(ByVal Value As Integer)
            pEDate(Index) = Value
            Update()
        End Set
    End Property

    Public ReadOnly Property EdateJ() As Integer
        Get
            Return atcUtility.modDate.Date2J(pEDate)
        End Get
    End Property

    Public Property outlev() As HSPFParm
        Get
            outlev = pOutLev
        End Get
        Set(ByVal Value As HSPFParm)
            pOutLev = Value
            Update()
        End Set
    End Property

    Public Property spout() As Integer
        Get
            spout = pSpOut
        End Get
        Set(ByVal Value As Integer)
            pSpOut = Value
            Update()
        End Set
    End Property

    Public Property runfg() As Integer
        Get
            runfg = pRunFg
        End Get
        Set(ByVal Value As Integer)
            pRunFg = Value
            Update()
        End Set
    End Property

    Public Property emfg() As Integer
        Get
            emfg = pEmFg
        End Get
        Set(ByVal Value As Integer)
            pEmFg = Value
            Update()
        End Set
    End Property

    Public Sub Edit()
        editInit(Me, Me.Uci.icon)
    End Sub

    Public Sub ReadUciFile()
        Dim rectyp, lOutLev, retkey, retcod As Integer
        Dim lRunInf As String = Nothing
        Dim cbuff As String = Nothing

        If pUci.FastFlag Then
            retkey = -1
            GetCommentBeforeBlock("GLOBAL", pComment)
            GetNextRecordFromBlock("GLOBAL", retkey, cbuff, rectyp, retcod)
            If Mid(cbuff, 1, 7) <> "START" Then
                lRunInf = Trim(cbuff)
                GetNextRecordFromBlock("GLOBAL", retkey, cbuff, rectyp, retcod)
            Else
                lRunInf = ""
            End If
            'Allow room for comments
            While rectyp < 0 And retkey < 50 '(50 is arbitrary to prevent an endless loop)
                GetNextRecordFromBlock("GLOBAL", retkey, cbuff, rectyp, retcod)
            End While

            pSDate(0) = CInt(Mid(cbuff, 15, 4))
            If Len(Trim(Mid(cbuff, 20, 2))) > 0 Then
                pSDate(1) = CInt(Mid(cbuff, 20, 2))
            Else
                pSDate(1) = 1
            End If
            If Len(Trim(Mid(cbuff, 23, 2))) > 0 Then
                pSDate(2) = CInt(Mid(cbuff, 23, 2))
            Else
                pSDate(2) = 1
            End If
            If Len(Trim(Mid(cbuff, 26, 2))) > 0 Then
                pSDate(3) = CInt(Mid(cbuff, 26, 2))
                pSDate(4) = CInt(Mid(cbuff, 29, 2))
            Else
                pSDate(3) = 0
                pSDate(4) = 0
            End If
            pEDate(0) = CInt(Mid(cbuff, 40, 4))
            If Len(Trim(Mid(cbuff, 45, 2))) > 0 Then
                pEDate(1) = CInt(Mid(cbuff, 45, 2))
            Else
                pEDate(1) = 12
            End If
            If Len(Trim(Mid(cbuff, 48, 2))) > 0 Then
                pEDate(2) = CInt(Mid(cbuff, 48, 2))
            Else
                pEDate(2) = 31
            End If
            If Len(Trim(Mid(cbuff, 51, 2))) > 0 Then
                pEDate(3) = CInt(Mid(cbuff, 51, 2))
                pEDate(4) = CInt(Mid(cbuff, 54, 2))
            Else
                pEDate(3) = 24
                pEDate(4) = 0
            End If
            GetNextRecordFromBlock("GLOBAL", retkey, cbuff, rectyp, retcod)
            lOutLev = CInt(Mid(cbuff, 26, 5))
            If Len(Trim(Mid(cbuff, 31, 5))) > 0 Then
                pSpOut = CInt(Mid(cbuff, 31, 5))
            Else
                pSpOut = 2
            End If
            GetNextRecordFromBlock("GLOBAL", retkey, cbuff, rectyp, retcod)
            pRunFg = CInt(Mid(cbuff, 20, 5))
            If Len(Trim(Mid(cbuff, 58, 5))) > 0 Then
                pEmFg = CInt(Mid(cbuff, 58, 5))
            Else
                pEmFg = 1
            End If
            If Len(Trim(Mid(cbuff, 68, 5))) > 0 Then
                pIhmFg = CInt(Mid(cbuff, 68, 5))
            Else
                pIhmFg = 0
            End If
        Else
            Call REM_GLOBLK((Me.Uci), pSDate, pEDate, lOutLev, pSpOut, pRunFg, pEmFg, lRunInf)
            Call REM_GLOPRMI((Me.Uci), pIhmFg, "IHMFG")
        End If

        If pSDate(1) = 0 Then pSDate(1) = 1
        If pSDate(2) = 0 Then pSDate(2) = 1
        If pEDate(1) = 0 Then pEDate(1) = 12
        If pEDate(2) = 0 Then pEDate(2) = 31

        pOutLev.Value = CStr(lOutLev)
        pRunInf.Value = lRunInf
    End Sub

    Public Sub WriteUciFile(ByRef f As Short)
        Dim s, e As String

        If Len(pComment) > 0 Then
            PrintLine(f, pComment)
        End If
        PrintLine(f, " ")
        PrintLine(f, "GLOBAL")
        PrintLine(f, "  " & Trim(pRunInf.Value))
        s = "  START       " & Format(SDate(0), "0000") & "/" & Format(SDate(1), "00") & "/" & Format(SDate(2), "00") & " " & Format(SDate(3), "00") & ":" & Format(SDate(4), "00")
        e = "  END    " & Format(EDate(0), "0000") & "/" & Format(EDate(1), "00") & "/" & Format(EDate(2), "00") & " " & Format(EDate(3), "00") & ":" & Format(EDate(4), "00")
        PrintLine(f, s & e)
        PrintLine(f, "  RUN INTERP OUTPT LEVELS" & myFormatI(CInt(pOutLev.Value), 5) & myFormatI(pSpOut, 5))
        s = "  RESUME     0 RUN " & myFormatI(pRunFg, 5) & Space(26) & "UNITS" & myFormatI(pEmFg, 5)
        If pIhmFg <> 0 And pIhmFg <> -999 Then
            s = s & "     " & myFormatI(pIhmFg, 5)
        End If
        PrintLine(f, s)
        PrintLine(f, "END GLOBAL")
    End Sub

    Private Sub Update()
        'Call F90_PUTGLO(pSDate(0), pEDate(0), pOutLev, pSpOut, pRunFg, pEmFg, pRunInf, Len(pRunInf))
        pUci.Edited = True
    End Sub

    'Public Function Check() As String
    '    'verify values are correct in relation to each other and other tables
    'End Function

    Public Sub New()
        MyBase.New()
        pOutLev = New HSPFParm
        pOutLev.Parent = Me
        pOutLev.Def = readParmDef("OutLev")
        pRunInf = New HSPFParm
        pRunInf.Parent = Me
        pRunInf.Def = readParmDef("RunInf")
    End Sub
End Class