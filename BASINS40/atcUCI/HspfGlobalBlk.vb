'Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license
Option Strict Off
Option Explicit On

Imports atcUtility
Imports MapWinUtility

Public Class HspfGlobalBlk
    Private pRunInf As HSPFParm ' run information
    Private pSDate(5) As Integer 'starting date
    Private pEDate(5) As Integer 'ending date
    Private pOutLev As HSPFParm 'run interp output level
    Private pSpOut As Integer 'special action output level
    Private pRunFg As Integer 'interp only(0) or interp and run(1)
    Private pEmFg As Integer 'english(1), metric(2) flag
    Private pIhmFg As Integer 'ihm flag (normal-0,IHM control-1)
    Public Comment As String = ""
    Public Uci As HspfUci
    Private pEdited As Boolean

    Public ReadOnly Caption As String = "Global Block"
    Public ReadOnly EditControlName As String = "ATCoHspf.ctlGlobalBlkEdit"

    Public Property Edited() As Boolean
        Get
            Return pEdited
        End Get
        Set(ByVal Value As Boolean)
            pEdited = Value
            If Value Then Me.Uci.Edited = True
        End Set
    End Property


    Public Property RunInf() As HspfParm
        Get
            Return pRunInf
        End Get
        Set(ByVal Value As HspfParm)
            pRunInf = Value
            Update()
        End Set
    End Property

    Public ReadOnly Property RunPeriod() As String
        Get
            Return TimeSpanAsString(SDateJ, EdateJ)
        End Get
    End Property
    Public ReadOnly Property YearCount() As Integer
        Get
            Return modDate.YearCount(SDateJ, EdateJ)
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

    Public Property OutLev() As HspfParm
        Get
            Return pOutLev
        End Get
        Set(ByVal Value As HspfParm)
            pOutLev = Value
            Update()
        End Set
    End Property

    Public Property SpOut() As Integer
        Get
            Return pSpOut
        End Get
        Set(ByVal Value As Integer)
            pSpOut = Value
            Update()
        End Set
    End Property

    Public Property RunFg() As Integer
        Get
            RunFg = pRunFg
        End Get
        Set(ByVal Value As Integer)
            pRunFg = Value
            Update()
        End Set
    End Property

    Public Property EmFg() As Integer
        Get
            Return pEmFg
        End Get
        Set(ByVal Value As Integer)
            pEmFg = Value
            Update()
        End Set
    End Property

    Public Sub ReadUciFile()
        Dim lRecordType, lRecordIndex, lReturnCode As Integer
        Dim lRecord As String = Nothing
        Dim lOutLev As Integer

        lRecordIndex = -1
        Me.Comment = GetCommentBeforeBlock("GLOBAL")
        GetNextRecordFromBlock("GLOBAL", lRecordIndex, lRecord, lRecordType, lReturnCode)
        If Not lRecord.StartsWith("  START") Then
            pRunInf.Value = lRecord.TrimEnd
            GetNextRecordFromBlock("GLOBAL", lRecordIndex, lRecord, lRecordType, lReturnCode)
        Else
            pRunInf.Value = ""
        End If
        'Allow room for comments
        While lRecordType < 0 And lRecordIndex < 50 '(50 is arbitrary to prevent an endless loop)
            GetNextRecordFromBlock("GLOBAL", lRecordIndex, lRecord, lRecordType, lReturnCode)
        End While

        Dim lField As String = lRecord.Substring(14, 4)
        If IsInteger(lField) Then
            pSDate(0) = lField
        Else
            Logger.Dbg("StartYearParseFailed:" & lField)
            pSDate(0) = 1996 'better than nothing?
        End If
        lField = lRecord.Substring(19, 2)
        If lField.Length > 0 AndAlso IsInteger(lField) Then
            pSDate(1) = lField
        Else
            pSDate(1) = 1
        End If
        lField = lRecord.Substring(22, 2).TrimEnd
        If lField.Length > 0 AndAlso IsInteger(lField) Then
            pSDate(2) = lField
        Else
            pSDate(2) = 1
        End If
        lField = lRecord.Substring(25, 2).TrimEnd
        If lField.Length > 0 AndAlso IsInteger(lField) Then
            pSDate(3) = lField
            pSDate(4) = lRecord.Substring(28, 2)
        Else
            pSDate(3) = 0
            pSDate(4) = 0
        End If

        lField = lRecord.Substring(39, 4)
        If IsInteger(lField) Then
            pEDate(0) = lField
        Else
            Logger.Dbg("EndYearParseFailed:" & lField)
            pEDate(0) = 2007 'better than nothing?
        End If
        lField = lRecord.Substring(44, 2).TrimEnd
        If lField.Length > 0 AndAlso IsInteger(lField) Then
            pEDate(1) = lField
        Else
            pEDate(1) = 12
        End If
        lField = lRecord.Substring(47, 2).TrimEnd
        If lField.Length > 0 AndAlso IsInteger(lField) Then
            pEDate(2) = lField
        Else
            pEDate(2) = 31
        End If
        lField = lRecord.Substring(50, 2).TrimEnd
        If lField.Length > 0 AndAlso IsInteger(lField) Then
            pEDate(3) = lField
            pEDate(4) = lRecord.Substring(53, 2)
        Else
            pEDate(3) = 24
            pEDate(4) = 0
        End If

        GetNextRecordFromBlock("GLOBAL", lRecordIndex, lRecord, lRecordType, lReturnCode)
        lField = lRecord.Substring(25, 5)
        If lField.Length > 0 AndAlso IsInteger(lField) Then
            lOutLev = lField
        Else
            lOutLev = 3
        End If
        lField = lRecord.Substring(30, 5)
        If lField.Length > 0 AndAlso IsInteger(lField) Then
            pSpOut = lField
        Else
            pSpOut = 2
        End If

        GetNextRecordFromBlock("GLOBAL", lRecordIndex, lRecord, lRecordType, lReturnCode)
        lField = lRecord.Substring(19, 5)
        If lField.Length > 0 AndAlso IsInteger(lField) Then
            pRunFg = lField
        Else
            pRunFg = 0
        End If
        lField = lRecord.Substring(57, 5)
        If lField.Length > 0 AndAlso IsInteger(lField) Then
            pEmFg = lField
        Else
            pEmFg = 1
        End If
        lField = lRecord.Substring(67, 5)
        If lField.Length > 0 AndAlso IsInteger(lField) Then
            pIhmFg = lField
        Else
            pIhmFg = 0
        End If

        If pSDate(1) = 0 Then pSDate(1) = 1
        If pSDate(2) = 0 Then pSDate(2) = 1
        If pEDate(1) = 0 Then pEDate(1) = 12
        If pEDate(2) = 0 Then pEDate(2) = 31

        pOutLev.Value = CStr(lOutLev)
    End Sub

    Public Overrides Function ToString() As String
        Dim lSB As New System.Text.StringBuilder

        If Me.Comment.Length > 0 Then
            lSB.AppendLine(Me.Comment.TrimEnd)
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
        Me.Uci.Edited = True
    End Sub

    'Public Function Check() As String
    '    'verify values are correct in relation to each other and other tables
    'End Function

    Public Sub New()
        MyBase.New()
        pOutLev = New HspfParm
        pOutLev.Parent = Me
        pOutLev.Def = readParmDef("OutLev")
        pRunInf = New HspfParm
        pRunInf.Parent = Me
        pRunInf.Def = readParmDef("RunInf")
    End Sub

    Private Function readParmDef(ByRef aNameParm As String) As HSPFParmDef
        Dim lParmDef As New HSPFParmDef

        If aNameParm = "OutLev" Then
            lParmDef.Define = "Run Interpreter Output Level"
        ElseIf aNameParm = "RunInf" Then
            lParmDef.Define = "Run Information"
        End If

        Return lParmDef
    End Function
End Class