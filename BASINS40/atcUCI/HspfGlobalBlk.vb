'Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license
Option Strict Off
Option Explicit On
Imports atcUtility
Imports System.Text

Public Class HspfGlobalBlk
    Private pRunInf As HSPFParm ' run information
    Private pSDate(5) As Integer 'starting date
    Private pEDate(5) As Integer 'ending date
    Private pOutLev As HSPFParm 'run interp output level
    Private pSpOut As Integer 'special action output level
    Private pRunFg As Integer 'interp only(0) or interp and run(1)
    Private pEmFg As Integer 'english(1), metric(2) flag
    Private pIhmFg As Integer 'ihm flag (normal-0,IHM control-1)
    Private pComment As String = ""
    Private pUci As HspfUci
    Private pEdited As Boolean

    Public Property Edited() As Boolean
        Get
            Return pEdited
        End Get
        Set(ByVal Value As Boolean)
            pEdited = Value
            If Value Then pUci.Edited = True
        End Set
    End Property

    Public Property Uci() As HspfUci
        Get
            Return pUci
        End Get
        Set(ByVal Value As HspfUci)
            pUci = Value
        End Set
    End Property

    Public ReadOnly Property Caption() As String
        Get
            Return "Global Block"
        End Get
    End Property

    Public Property Comment() As String
        Get
            Return pComment
        End Get
        Set(ByVal Value As String)
            pComment = Value
        End Set
    End Property

    Public ReadOnly Property EditControlName() As String
        Get
            Return "ATCoHspf.ctlGlobalBlkEdit"
        End Get
    End Property

    Public Property RunInf() As HSPFParm
        Get
            Return pRunInf
        End Get
        Set(ByVal Value As HSPFParm)
            pRunInf = Value
            Update()
        End Set
    End Property

    Public ReadOnly Property RunPeriod() As String
        Get
            Dim lYrCnt As Double = timdifJ(SDateJ, EdateJ, 6, 1)
            Dim lStr As String = "Simulation Period: " & lYrCnt & " years"
            lStr &= " from " & Format(Date.FromOADate(SDateJ), "yyyy/MM/dd")
            lStr &= " to " & Format(Date.FromOADate(EdateJ), "yyyy/MM/dd") & vbCrLf
            Return lStr
        End Get
    End Property

    Public Property SDate(ByVal aIndex As Integer) As Integer
        Get
            If aIndex >= 0 And aIndex <= pSDate.GetUpperBound(0) Then
                Return pSDate(aIndex)
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal Value As Integer)
            pSDate(aIndex) = Value
            Update()
        End Set
    End Property

    Public ReadOnly Property SDateJ() As Integer
        Get
            Return atcUtility.modDate.Date2J(pSDate)
        End Get
    End Property

    Public Property EDate(ByVal aIndex As Integer) As Integer
        Get
            If aIndex >= 0 And aIndex <= pEDate.GetUpperBound(0) Then
                EDate = pEDate(aIndex)
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal Value As Integer)
            pEDate(aIndex) = Value
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
            Return pOutLev
        End Get
        Set(ByVal Value As HSPFParm)
            pOutLev = Value
            Update()
        End Set
    End Property

    Public Property spout() As Integer
        Get
            Return pSpOut
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
            Return pEmFg
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
        Dim cbuff As String = Nothing

        If pUci.FastFlag Then
            retkey = -1
            pComment = GetCommentBeforeBlock("GLOBAL")
            GetNextRecordFromBlock("GLOBAL", retkey, cbuff, rectyp, retcod)
            If Mid(cbuff, 1, 7) <> "START" Then
                pRunInf.Value = cbuff.TrimEnd
                GetNextRecordFromBlock("GLOBAL", retkey, cbuff, rectyp, retcod)
            Else
                pRunInf.Value = ""
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
            Call REM_GLOBLK((Me.Uci), pSDate, pEDate, lOutLev, pSpOut, pRunFg, pEmFg, pRunInf.Value)
            Call REM_GLOPRMI((Me.Uci), pIhmFg, "IHMFG")
        End If

        If pSDate(1) = 0 Then pSDate(1) = 1
        If pSDate(2) = 0 Then pSDate(2) = 1
        If pEDate(1) = 0 Then pEDate(1) = 12
        If pEDate(2) = 0 Then pEDate(2) = 31

        pOutLev.Value = CStr(lOutLev)
    End Sub

    Public Overrides Function ToString() As String
        Dim lSB As New StringBuilder

        If pComment.Length > 0 Then
            lSB.AppendLine(pComment.TrimEnd)
        End If
        lSB.AppendLine("GLOBAL")
        lSB.AppendLine("  " & pRunInf.Value.Trim)
        lSB.Append("  START       " & Format(SDate(0), "0000") & "/" & Format(SDate(1), "00") & "/" & Format(SDate(2), "00") & " " & Format(SDate(3), "00") & ":" & Format(SDate(4), "00"))
        lSB.AppendLine("  END    " & Format(EDate(0), "0000") & "/" & Format(EDate(1), "00") & "/" & Format(EDate(2), "00") & " " & Format(EDate(3), "00") & ":" & Format(EDate(4), "00"))
        lSB.AppendLine("  RUN INTERP OUTPT LEVELS" & myFormatI(CInt(pOutLev.Value), 5) & myFormatI(pSpOut, 5))
        lSB.Append("  RESUME     0 RUN " & myFormatI(pRunFg, 5) & Space(26) & "UNITS" & myFormatI(pEmFg, 5))
        If pIhmFg <> 0 And pIhmFg <> -999 Then
            lSB.Append("     " & myFormatI(pIhmFg, 5))
        End If
        lSB.AppendLine("")
        lSB.AppendLine("END GLOBAL")
        Return lSB.ToString
    End Function

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
        pRunInf = New HspfParm
        pRunInf.Parent = Me
        pRunInf.Def = readParmDef("RunInf")
    End Sub
End Class