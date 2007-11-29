'Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license
Option Strict Off
Option Explicit On

Imports System.Text

Public Class HspfConnection
    Private pMFact As Double
    Private pMFactAsRead As String
    Private pTyp As Integer '1-ExtSource,2-Network,3-Schematic,4-ExtTarget
    Private pTran As String
    Private pSgapstrg As String
    Private pAmdstrg As String
    Private pSsystem As String
    Private pSource As HspfSrcTar
    Private pTarget As HspfSrcTar
    Private pMassLink As Integer
    Private pUci As HspfUci
    Private DesiredType As String
    Private pComment As String

    Public Property MFact() As Double
        Get
            MFact = pMFact
        End Get
        Set(ByVal Value As Double)
            pMFact = Value
        End Set
    End Property

    Public Property MFactAsRead() As String
        Get
            MFactAsRead = pMFactAsRead
        End Get
        Set(ByVal Value As String)
            pMFactAsRead = Value
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

    Public Property Source() As HspfSrcTar
        Get
            Source = pSource
        End Get
        Set(ByVal Value As HspfSrcTar)
            pSource = Value
        End Set
    End Property

    Public Property Target() As HspfSrcTar
        Get
            Target = pTarget
        End Get
        Set(ByVal Value As HspfSrcTar)
            pTarget = Value
        End Set
    End Property

    Public Property Tran() As String
        Get
            Tran = pTran
        End Get
        Set(ByVal Value As String)
            pTran = Value
        End Set
    End Property


    Public Property Comment() As String
        Get
            Comment = pComment
        End Get
        Set(ByVal Value As String)
            pComment = Value
        End Set
    End Property

    Public Property Ssystem() As String
        Get
            Ssystem = pSsystem
        End Get
        Set(ByVal Value As String)
            pSsystem = Value
        End Set
    End Property

    Public Property Sgapstrg() As String
        Get
            Sgapstrg = pSgapstrg
        End Get
        Set(ByVal Value As String)
            pSgapstrg = Value
        End Set
    End Property

    Public Property Amdstrg() As String
        Get
            Amdstrg = pAmdstrg
        End Get
        Set(ByVal Value As String)
            pAmdstrg = Value
        End Set
    End Property
    Public Property Typ() As Integer
        Get
            Typ = pTyp
        End Get
        Set(ByVal Value As Integer)
            pTyp = Value
        End Set
    End Property

    Public Property MassLink() As Integer
        Get
            MassLink = pMassLink
        End Get
        Set(ByVal Value As Integer)
            pMassLink = Value
        End Set
    End Property
    Public ReadOnly Property EditControlName() As String
        Get
            EditControlName = "ATCoHspf.ctlConnectionEdit"
        End Get
    End Property
    Public ReadOnly Property DesiredRecordType() As String
        Get
            DesiredRecordType = DesiredType
        End Get
    End Property
    Public ReadOnly Property Caption() As String
        Get
            Caption = DesiredType & " Block"
        End Get
    End Property
    Public Sub readTimSer(ByRef myUci As HspfUci)
        Dim retcod, OmCode, init, retkey, rectyp As Integer
        Dim cbuff As String = Nothing
        Dim lConnection As HspfConnection
        Dim s, c As String
        Dim pastHeader As Boolean
        Dim t As String

        pUci = myUci
        OmCode = HspfOmCode("EXT SOURCES")
        init = 1
        c = ""
        pastHeader = False
        retkey = -1
        Do
            If myUci.FastFlag Then
                GetNextRecordFromBlock("EXT SOURCES", retkey, cbuff, rectyp, retcod)
            Else
                retkey = -1
                Call REM_XBLOCKEX(myUci, OmCode, init, retkey, cbuff, rectyp, retcod)
            End If
            If retcod <> 2 Then Exit Do
            init = 0
            If rectyp = 0 Then
                lConnection = New HspfConnection
                lConnection.Uci = myUci
                lConnection.Typ = 1
                lConnection.Source.VolName = Trim(Left(cbuff, 6))
                lConnection.Source.VolId = CInt(Mid(cbuff, 7, 4))
                lConnection.Source.Member = Trim(Mid(cbuff, 12, 6))
                s = Trim(Mid(cbuff, 18, 2))
                If Len(s) > 0 Then lConnection.Source.MemSub1 = CInt(s)
                lConnection.Ssystem = Mid(cbuff, 21, 4)
                lConnection.Sgapstrg = Mid(cbuff, 25, 4)
                s = Trim(Mid(cbuff, 29, 10))
                lConnection.MFactAsRead = Mid(cbuff, 29, 10)
                If Len(s) > 0 Then lConnection.MFact = CDbl(s)
                s = Mid(cbuff, 39, 4)
                If Len(s) > 0 Then lConnection.Tran = s
                lConnection.Target.VolName = Trim(Mid(cbuff, 44, 6))
                lConnection.Target.VolId = CInt(Mid(cbuff, 51, 3))
                s = Trim(Mid(cbuff, 55, 3))
                If Len(s) > 0 Then lConnection.Target.VolIdL = CInt(s)
                lConnection.Target.Group = Trim(Mid(cbuff, 59, 6))
                lConnection.Target.Member = Trim(Mid(cbuff, 66, 6))
                s = Trim(Mid(cbuff, 72, 2))
                If Len(s) > 0 And IsNumeric(s) Then
                    lConnection.Target.MemSub1 = CInt(s)
                Else
                    If Len(s) > 0 Then lConnection.Target.MemSub1 = myUci.CatAsInt(s)
                End If
                s = Trim(Mid(cbuff, 74, 2))
                If Len(s) > 0 And IsNumeric(s) Then
                    lConnection.Target.MemSub2 = CInt(s)
                Else
                    If Len(s) > 0 Then lConnection.Target.MemSub2 = myUci.CatAsInt(s)
                End If
                lConnection.Comment = c
                myUci.Connections.Add(lConnection)
                c = ""
            ElseIf rectyp = -1 And retcod <> 1 Then
                'save comment
                t = Left(cbuff, 6)
                If t = "<Name>" Then 'a cheap rule to identify the last header line
                    pastHeader = True
                ElseIf pastHeader Then
                    If Len(c) = 0 Then
                        c = cbuff
                    Else
                        c = c & vbCrLf & cbuff
                    End If
                End If
            End If
        Loop

        OmCode = HspfOmCode("NETWORK")
        init = 1
        c = ""
        pastHeader = False
        retkey = -1
        Do
            If myUci.FastFlag Then
                GetNextRecordFromBlock("NETWORK", retkey, cbuff, rectyp, retcod)
            Else
                retkey = -1
                Call REM_XBLOCKEX(myUci, OmCode, init, retkey, cbuff, rectyp, retcod)
            End If
            If retcod <> 2 Then Exit Do
            init = 0
            If rectyp = 0 Then
                lConnection = New HspfConnection
                lConnection.Uci = myUci
                lConnection.Typ = 2
                lConnection.Source.VolName = Trim(Left(cbuff, 6))
                lConnection.Source.VolId = CInt(Mid(cbuff, 7, 4))
                lConnection.Source.Group = Trim(Mid(cbuff, 12, 6))
                lConnection.Source.Member = Trim(Mid(cbuff, 19, 6))
                s = Trim(Mid(cbuff, 25, 2))
                If Len(s) > 0 And IsNumeric(s) Then
                    lConnection.Source.MemSub1 = CInt(s)
                Else
                    If Len(s) > 0 Then lConnection.Source.MemSub1 = myUci.CatAsInt(s)
                End If
                s = Trim(Mid(cbuff, 27, 2))
                If Len(s) > 0 And IsNumeric(s) Then
                    lConnection.Source.MemSub2 = CInt(s)
                Else
                    If Len(s) > 0 Then lConnection.Source.MemSub2 = myUci.CatAsInt(s)
                End If
                s = Trim(Mid(cbuff, 29, 10))
                lConnection.MFactAsRead = Mid(cbuff, 29, 10)
                If Len(s) > 0 Then lConnection.MFact = CDbl(s)
                lConnection.Tran = Trim(Mid(cbuff, 39, 4))
                lConnection.Target.VolName = Trim(Mid(cbuff, 44, 6))
                lConnection.Target.VolId = CInt(Mid(cbuff, 51, 3))
                s = Trim(Mid(cbuff, 55, 3))
                If Len(s) > 0 Then lConnection.Target.VolIdL = CInt(s)
                lConnection.Target.Group = Trim(Mid(cbuff, 59, 6))
                lConnection.Target.Member = Trim(Mid(cbuff, 66, 6))
                s = Trim(Mid(cbuff, 72, 2))
                If Len(s) > 0 And IsNumeric(s) Then
                    lConnection.Target.MemSub1 = CInt(s)
                Else
                    If Len(s) > 0 Then lConnection.Target.MemSub1 = myUci.CatAsInt(s)
                End If
                s = Trim(Mid(cbuff, 74, 2))
                If Len(s) > 0 And IsNumeric(s) Then
                    lConnection.Target.MemSub2 = CInt(s)
                Else
                    If Len(s) > 0 Then lConnection.Target.MemSub2 = myUci.CatAsInt(s)
                End If
                lConnection.Comment = c
                myUci.Connections.Add(lConnection)
                c = ""
            ElseIf rectyp = -1 And retcod <> 1 Then
                'save comment
                t = Left(cbuff, 6)
                If t = "<Name>" Then 'a cheap rule to identify the last header line
                    pastHeader = True
                ElseIf pastHeader Then
                    If Len(c) = 0 Then
                        c = cbuff
                    Else
                        c = c & vbCrLf & cbuff
                    End If
                End If
            End If
        Loop

        OmCode = HspfOmCode("SCHEMATIC")
        init = 1
        c = ""
        pastHeader = False
        retkey = -1
        Do
            If myUci.FastFlag Then
                GetNextRecordFromBlock("SCHEMATIC", retkey, cbuff, rectyp, retcod)
            Else
                retkey = -1
                Call REM_XBLOCKEX(myUci, OmCode, init, retkey, cbuff, rectyp, retcod)
            End If
            If retcod <> 2 Then Exit Do
            init = 0
            If rectyp = 0 Then
                lConnection = New HspfConnection
                lConnection.Uci = myUci
                lConnection.Typ = 3
                lConnection.Source.VolName = Trim(Left(cbuff, 6))
                lConnection.Source.VolId = CInt(Mid(cbuff, 7, 4))
                s = Trim(Mid(cbuff, 29, 10))
                lConnection.MFactAsRead = Mid(cbuff, 29, 10)
                If Len(s) > 0 Then lConnection.MFact = CDbl(s)
                lConnection.Target.VolName = Trim(Mid(cbuff, 44, 6))
                lConnection.Target.VolId = CInt(Mid(cbuff, 50, 4))
                lConnection.MassLink = CInt(Mid(cbuff, 57, 4))
                s = Trim(Mid(cbuff, 72, 2))
                If Len(s) > 0 And IsNumeric(s) Then
                    lConnection.Target.MemSub1 = CInt(s)
                Else
                    If Len(s) > 0 Then lConnection.Target.MemSub1 = myUci.CatAsInt(s)
                End If
                s = Trim(Mid(cbuff, 74, 2))
                If Len(s) > 0 And IsNumeric(s) Then
                    lConnection.Target.MemSub2 = CInt(s)
                Else
                    If Len(s) > 0 Then lConnection.Target.MemSub2 = myUci.CatAsInt(s)
                End If
                lConnection.Comment = c
                myUci.Connections.Add(lConnection)
                c = ""
            ElseIf rectyp = -1 And retcod <> 1 Then
                'save comment
                t = Left(cbuff, 6)
                If t = "<Name>" Then 'a cheap rule to identify the last header line
                    pastHeader = True
                ElseIf pastHeader Then
                    If Len(c) = 0 Then
                        c = cbuff
                    Else
                        c = c & vbCrLf & cbuff
                    End If
                End If
            End If
        Loop

        OmCode = HspfOmCode("EXT TARGETS")
        init = 1
        c = ""
        pastHeader = False
        retkey = -1
        Do
            If myUci.FastFlag Then
                GetNextRecordFromBlock("EXT TARGETS", retkey, cbuff, rectyp, retcod)
            Else
                retkey = -1
                Call REM_XBLOCKEX(myUci, OmCode, init, retkey, cbuff, rectyp, retcod)
            End If
            If retcod <> 2 Then Exit Do
            init = 0
            If rectyp = 0 Then
                lConnection = New HspfConnection
                lConnection.Uci = myUci
                lConnection.Typ = 4
                lConnection.Source.VolName = Trim(Left(cbuff, 6))
                lConnection.Source.VolId = CInt(Mid(cbuff, 7, 4))
                lConnection.Source.Group = Trim(Mid(cbuff, 12, 6))
                lConnection.Source.Member = Trim(Mid(cbuff, 19, 6))
                s = Trim(Mid(cbuff, 25, 2))
                If Len(s) > 0 And IsNumeric(s) Then
                    lConnection.Source.MemSub1 = CInt(s)
                Else
                    If Len(s) > 0 Then lConnection.Source.MemSub1 = myUci.CatAsInt(s)
                End If
                s = Trim(Mid(cbuff, 27, 2))
                If Len(s) > 0 And IsNumeric(s) Then
                    lConnection.Source.MemSub2 = CInt(s)
                Else
                    If Len(s) > 0 Then lConnection.Source.MemSub1 = myUci.CatAsInt(s)
                End If
                s = Trim(Mid(cbuff, 29, 10))
                lConnection.MFactAsRead = Mid(cbuff, 29, 10)
                If Len(s) > 0 Then lConnection.MFact = CDbl(s)
                s = Trim(Mid(cbuff, 39, 4))
                If Len(s) > 0 Then lConnection.Tran = s
                lConnection.Target.VolName = Trim(Mid(cbuff, 44, 6))
                lConnection.Target.VolId = CInt(Mid(cbuff, 50, 4))
                lConnection.Target.Member = Trim(Mid(cbuff, 55, 6))
                s = Trim(Mid(cbuff, 61, 2))
                If Len(s) > 0 Then lConnection.Target.MemSub1 = CInt(s)
                lConnection.Ssystem = Trim(Mid(cbuff, 64, 4))
                lConnection.Sgapstrg = Trim(Mid(cbuff, 69, 4))
                lConnection.Amdstrg = Trim(Mid(cbuff, 74, 4))
                lConnection.Comment = c
                myUci.Connections.Add(lConnection)
                c = ""
            ElseIf rectyp = -1 And retcod <> 1 Then
                'save comment
                t = Left(cbuff, 6)
                If t = "<Name>" Then 'a cheap rule to identify the last header line
                    pastHeader = True
                ElseIf pastHeader Then
                    If Len(c) = 0 Then
                        c = cbuff
                    Else
                        c = c & vbCrLf & cbuff
                    End If
                End If
            End If
        Loop
    End Sub

    Public Sub New()
        MyBase.New()
        pSource = New HspfSrcTar
        pTarget = New HspfSrcTar
        pTyp = 0
        pMFact = 1.0#
    End Sub
    Public Sub EditExtSrc()
        DesiredType = "EXT SOURCES"
        editInit(Me, Me.Uci.icon, True) 'add remove ok
    End Sub
    Public Sub EditExtTar()
        DesiredType = "EXT TARGETS"
        editInit(Me, Me.Uci.icon, True) 'add remove ok
    End Sub
    Public Sub EditNetwork()
        DesiredType = "NETWORK"
        editInit(Me, Me.Uci.icon, True) 'add remove ok
    End Sub
    Public Sub EditSchematic()
        DesiredType = "SCHEMATIC"
        editInit(Me, Me.Uci.icon, True) 'add remove ok
    End Sub

    Public Overrides Function ToString() As String
        Dim lSB As New StringBuilder
        Dim s As String
        Dim lBlockDef As HspfBlockDef
        Dim lTableDef As HspfTableDef
        Dim j, i, k As Integer
        Dim typeexists(4) As Boolean
        Dim iCol(15) As Integer
        Dim iLen(15) As Integer
        Dim lOper As HspfOperation
        Dim lConn As HspfConnection
        Dim lOpnSeqBlock As HspfOpnSeqBlk
        Dim lParmDef As HSPFParmDef
        Dim t As String
        Dim lMetSeg As HspfMetSeg
        Dim lPtSrc As HspfPoint
        Static lOpTypes() As String = {"PERLND", "IMPLND", "RCHRES"} 'operations with assoc met segs

        typeexists(0) = False 'ext sou
        typeexists(1) = False 'network
        typeexists(2) = False 'schematic
        typeexists(3) = False 'ext tar

        If pUci.MetSegs.Count() > 0 Then
            typeexists(0) = True
        End If
        If pUci.PointSources.Count() > 0 Then
            typeexists(0) = True
        End If

        lOpnSeqBlock = pUci.OpnSeqBlock
        For i = 1 To lOpnSeqBlock.Opns.Count
            lOper = lOpnSeqBlock.Opn(i)
            For j = 1 To lOper.Targets.Count()
                lConn = lOper.Targets.Item(j)
                typeexists(lConn.Typ - 1) = True
            Next j
            For j = 1 To lOper.Sources.Count()
                lConn = lOper.Sources.Item(j)
                typeexists(lConn.Typ - 1) = True
            Next j
        Next i

        For i = 1 To 4
            If typeexists(i - 1) Then
                Select Case i
                    Case 1 : s = "EXT SOURCES"
                    Case 2 : s = "NETWORK"
                    Case 3 : s = "SCHEMATIC"
                    Case 4 : s = "EXT TARGETS"
                    Case Else : s = ""
                End Select
                lBlockDef = pUci.Msg.BlockDefs.Item(s)
                lTableDef = lBlockDef.TableDefs.Item(1)
                'get lengths and starting positions
                j = 0
                For Each lParmDef In lTableDef.ParmDefs
                    icol(j) = lParmDef.StartCol
                    ilen(j) = lParmDef.Length
                    j = j + 1
                Next lParmDef
                lSB.AppendLine(" ")
                lSB.AppendLine(s)
                'now start building the records
                Select Case i
                    Case 1 'ext srcs
                        lSB.AppendLine("<-Volume-> <Member> SsysSgap<--Mult-->Tran <-Target vols> <-Grp> <-Member-> ***")
                        lSB.AppendLine("<Name>   x <Name> x tem strg<-factor->strg <Name>   x   x        <Name> x x ***")
                        'do met segs
                        For Each OpTyp As String In lOpTypes
                            For Each lMetSeg In pUci.MetSegs
                                lSB.AppendLine(lMetSeg.ToStringFromSpecs(OpTyp, iCol, iLen))
                            Next
                        Next
                        'If pUci.PointSources.Count() > 0 And pUci.MetSegs.Count() > 0 Then
                        '    lSB.AppendLine("") 'write a blank line between met segs and pt srcs
                        'End If
                        'do point sources
                        For Each lPtSrc In pUci.PointSources
                            lSB.AppendLine(lPtSrc.ToStringFromSpecs(iCol, iLen))
                        Next
                        'now do everything else
                        For k = 1 To lOpnSeqBlock.Opns.Count
                            lOper = lOpnSeqBlock.Opn(k)
                            For j = 1 To lOper.Sources.Count()
                                lConn = lOper.Sources.Item(j)
                                If lConn.Typ = i Then
                                    If lConn.Comment.Length > 0 Then
                                        lSB.Append(lConn.Comment)
                                    End If
                                    Dim lStr As New StringBuilder
                                    lStr.Append(lConn.Source.VolName.Trim)
                                    lStr.Append(Space(icol(1) - lStr.Length - 1)) 'pad prev field
                                    t = Space(ilen(1)) 'right justify numbers
                                    t = RSet(CStr(lConn.Source.VolId), Len(t))
                                    lStr.Append(t)
                                    lStr.Append(Space(icol(2) - lStr.Length - 1))
                                    lStr.Append(lConn.Source.Member)
                                    lStr.Append(Space(icol(3) - lStr.Length - 1))
                                    If lConn.Source.MemSub1 <> 0 Then
                                        t = Space(ilen(3))
                                        t = RSet(CStr(lConn.Source.MemSub1), Len(t))
                                        If lConn.Source.VolName = "RCHRES" Then t = pUci.IntAsCat(lConn.Source.Member, 1, t)
                                        lStr.Append(t)
                                    End If
                                    lStr.Append(Space(icol(4) - lStr.Length - 1))
                                    lStr.Append(lConn.Ssystem)
                                    lStr.Append(Space(icol(5) - lStr.Length - 1))
                                    lStr.Append(lConn.Sgapstrg)
                                    lStr.Append(Space(icol(6) - lStr.Length - 1))
                                    If NumericallyTheSame((lConn.MFactAsRead), (lConn.MFact)) Then
                                        lStr.Append(lConn.MFactAsRead)
                                    ElseIf lConn.MFact <> 1 Then
                                        lStr.Append(CStr(lConn.MFact).PadLeft(ilen(6)))
                                    End If
                                    lStr.Append(Space(icol(7) - lStr.Length - 1))
                                    lStr.Append(lConn.Tran)
                                    lStr.Append(Space(icol(8) - lStr.Length - 1))
                                    lStr.Append(lOper.Name)
                                    lStr.Append(Space(icol(9) - lStr.Length - 1))
                                    lStr.Append(CStr(lOper.Id).PadLeft(ilen(9)))
                                    lStr.Append(Space(icol(11) - lStr.Length - 1))
                                    lStr.Append(lConn.Target.Group)
                                    lStr.Append(Space(icol(12) - lStr.Length - 1))
                                    lStr.Append(lConn.Target.Member)
                                    lStr.Append(Space(icol(13) - lStr.Length - 1))
                                    If lConn.Target.MemSub1 <> 0 Then
                                        t = CStr(lConn.Target.MemSub1).PadLeft(ilen(13))
                                        If lConn.Target.VolName = "RCHRES" Then t = pUci.IntAsCat(lConn.Target.Member, 1, t)
                                        lStr.Append(t)
                                    End If
                                    lStr.Append(Space(icol(14) - lStr.Length - 1))
                                    If lConn.Target.MemSub2 <> 0 Then
                                        t = CStr(lConn.Target.MemSub2).PadLeft(ilen(14))
                                        If lConn.Target.VolName = "RCHRES" Then t = pUci.IntAsCat(lConn.Target.Member, 2, t)
                                        lStr.Append(t)
                                    End If
                                    lSB.AppendLine(lStr.ToString)
                                End If
                            Next j
                        Next k
                    Case 2 'network
                        lSB.AppendLine("<-Volume-> <-Grp> <-Member-><--Mult-->Tran <-Target vols> <-Grp> <-Member->  ***")
                        lSB.AppendLine("<Name>   x        <Name> x x<-factor->strg <Name>   x   x        <Name> x x  ***")
                        For k = 1 To lOpnSeqBlock.Opns.Count
                            lOper = lOpnSeqBlock.Opn(k)
                            For j = 1 To lOper.Sources.Count() 'used to go thru targets, misses range
                                lConn = lOper.Sources.Item(j)
                                If lConn.Typ = i Then
                                    If lConn.Comment.Length > 0 Then
                                        lSB.AppendLine(lConn.Comment)
                                    End If
                                    Dim lStr As New StringBuilder
                                    lStr.Append(lConn.Source.VolName.Trim)
                                    lStr.Append(Space(icol(1) - lStr.Length - 1)) 'pad prev field
                                    lStr.Append(CStr(lConn.Source.VolId).PadLeft(ilen(1)))
                                    lStr.Append(Space(icol(2) - lStr.Length - 1))
                                    lStr.Append(lConn.Source.Group)
                                    lStr.Append(Space(icol(3) - lStr.Length - 1))
                                    lStr.Append(lConn.Source.Member)
                                    lStr.Append(Space(icol(4) - lStr.Length - 1))
                                    If lConn.Source.MemSub1 <> 0 Then
                                        t = CStr(lConn.Source.MemSub1).PadLeft(ilen(4))
                                        If lConn.Source.VolName = "RCHRES" Then t = pUci.IntAsCat(lConn.Source.Member, 1, t)
                                        lStr.Append(t)
                                    End If
                                    lStr.Append(Space(icol(5) - lStr.Length - 1))
                                    If lConn.Source.MemSub2 <> 0 Then
                                        t = CStr(lConn.Source.MemSub2).PadLeft(ilen(5))
                                        If lConn.Source.VolName = "RCHRES" Then t = pUci.IntAsCat(lConn.Source.Member, 2, t)
                                        lStr.Append(t)
                                    End If
                                    lStr.Append(Space(icol(6) - lStr.Length - 1))
                                    If NumericallyTheSame((lConn.MFactAsRead), (lConn.MFact)) Then
                                        lStr.Append(lConn.MFactAsRead)
                                    ElseIf lConn.MFact <> 1 Then
                                        lStr.Append(CStr(lConn.MFact).PadLeft(ilen(6)))
                                    End If
                                    lStr.Append(Space(icol(7) - lStr.Length - 1))
                                    lStr.Append(lConn.Tran)
                                    lStr.Append(Space(icol(8) - lStr.Length - 1))
                                    lStr.Append(lOper.Name)
                                    lStr.Append(Space(icol(9) - lStr.Length - 1))
                                    lStr.Append(CStr(lOper.Id).PadLeft(ilen(9)))
                                    lStr.Append(Space(icol(11) - lStr.Length - 1))
                                    lStr.Append(lConn.Target.Group)
                                    lStr.Append(Space(icol(12) - lStr.Length - 1))
                                    lStr.Append(lConn.Target.Member)
                                    lStr.Append(Space(icol(13) - lStr.Length - 1))
                                    If lConn.Target.MemSub1 <> 0 Then
                                        t = CStr(lConn.Target.MemSub1).PadLeft(ilen(13))
                                        If lConn.Target.VolName = "RCHRES" Then t = pUci.IntAsCat(lConn.Target.Member, 1, t)
                                        lStr.Append(t)
                                    End If
                                    lStr.Append(Space(icol(14) - lStr.Length - 1))
                                    If lConn.Target.MemSub2 <> 0 Then
                                        t = CStr(lConn.Target.MemSub2).PadLeft(ilen(14))
                                        If lConn.Target.VolName = "RCHRES" Then t = pUci.IntAsCat(lConn.Target.Member, 2, t)
                                        lStr.Append(t)
                                    End If
                                    lSB.AppendLine(lStr.ToString)
                                End If
                            Next j
                        Next k
                    Case 3 'schematic
                        lSB.AppendLine("<-Volume->                  <--Area-->     <-Volume->  <ML#> ***       <sb>")
                        lSB.AppendLine("<Name>   x                  <-factor->     <Name>   x        ***        x x")
                        For k = 1 To lOpnSeqBlock.Opns.Count
                            lOper = lOpnSeqBlock.Opn(k)
                            For j = 1 To lOper.Sources.Count()
                                lConn = lOper.Sources.Item(j)
                                If lConn.Typ = i Then
                                    If lConn.Comment.Length > 0 Then
                                        lSB.AppendLine(lConn.Comment)
                                    End If
                                    Dim lStr As New StringBuilder
                                    lStr.Append(lConn.Source.VolName.Trim)
                                    lStr.Append(Space(icol(1) - lStr.Length - 1)) 'pad prev field
                                    lStr.Append(CStr(lConn.Source.VolId).PadLeft(ilen(1)))
                                    lStr.Append(Space(icol(2) - lStr.Length - 1))
                                    If NumericallyTheSame((lConn.MFactAsRead), (lConn.MFact)) Then
                                        lStr.Append(lConn.MFactAsRead)
                                    ElseIf lConn.MFact <> 1 Then
                                        lConn.MFact = System.Math.Round(lConn.MFact, 2)
                                        lStr.Append(CStr(lConn.MFact).PadLeft(ilen(2)))
                                    End If
                                    lStr.Append(Space(icol(3) - lStr.Length - 1))
                                    lStr.Append(lConn.Target.VolName)
                                    lStr.Append(Space(icol(4) - lStr.Length - 1))
                                    lStr.Append(CStr(lConn.Target.VolId).PadLeft(ilen(5)))
                                    lStr.Append(Space(icol(5) - lStr.Length - 1))
                                    lStr.Append(CStr(lConn.MassLink).PadLeft(ilen(5)))
                                    If lConn.Target.MemSub1 > 0 Then
                                        lStr.Append(Space(icol(6) - lStr.Length - 1))
                                        t = CStr(lConn.Target.MemSub1).PadLeft(ilen(6))
                                        If lConn.Target.VolName = "RCHRES" Then t = pUci.IntAsCat(lConn.Target.Member, 1, t)
                                        lStr.Append(t)
                                    End If
                                    If lConn.Target.MemSub2 > 0 Then
                                        lStr.Append(Space(icol(7) - lStr.Length - 1))
                                        t = CStr(lConn.Target.MemSub2).PadLeft(ilen(7))
                                        If lConn.Target.VolName = "RCHRES" Then t = pUci.IntAsCat(lConn.Target.Member, 2, t)
                                        lStr.Append(t)
                                    End If
                                    lSB.AppendLine(lStr.ToString)
                                End If
                            Next j
                        Next k
                    Case 4 'ext targ
                        lSB.AppendLine("<-Volume-> <-Grp> <-Member-><--Mult-->Tran <-Volume-> <Member> Tsys Aggr Amd ***")
                        lSB.AppendLine("<Name>   x        <Name> x x<-factor->strg <Name>   x <Name>qf  tem strg strg***")
                        For k = 1 To lOpnSeqBlock.Opns.Count
                            lOper = lOpnSeqBlock.Opn(k)
                            For j = 1 To lOper.Targets.Count()
                                lConn = lOper.Targets.Item(j)
                                If lConn.Typ = i Then
                                    If lConn.Comment.Length > 0 Then
                                        lSB.AppendLine(lConn.Comment)
                                    End If
                                    Dim lStr As New StringBuilder
                                    lStr.Append(lConn.Source.VolName.Trim)
                                    lStr.Append(Space(icol(1) - lStr.Length - 1)) 'pad prev field
                                    lStr.Append(CStr(lConn.Source.VolId).PadLeft(ilen(1)))
                                    lStr.Append(Space(icol(2) - lStr.Length - 1))
                                    lStr.Append(lConn.Source.Group)
                                    lStr.Append(Space(icol(3) - lStr.Length - 1))
                                    lStr.Append(lConn.Source.Member)
                                    lStr.Append(Space(icol(4) - lStr.Length - 1))
                                    If lConn.Source.MemSub1 <> 0 Then
                                        t = CStr(lConn.Source.MemSub1).PadLeft(ilen(4))
                                        If lConn.Source.VolName = "RCHRES" Then t = pUci.IntAsCat(lConn.Source.Member, 1, t)
                                        lStr.Append(t)
                                    End If
                                    lStr.Append(Space(icol(5) - lStr.Length - 1))
                                    If lConn.Source.MemSub2 <> 0 Then
                                        t = CStr(lConn.Source.MemSub2).PadLeft(ilen(5))
                                        If lConn.Source.VolName = "RCHRES" Then t = pUci.IntAsCat(lConn.Source.Member, 2, t)
                                        lStr.Append(t)
                                    End If
                                    lStr.Append(Space(icol(6) - lStr.Length - 1))
                                    If NumericallyTheSame((lConn.MFactAsRead), (lConn.MFact)) Then
                                        lStr.Append(lConn.MFactAsRead)
                                    ElseIf lConn.MFact <> 1 Then
                                        'lConn.MFact = Format(lConn.MFact, "0.#######")
                                        'RSet t = CStr(lConn.MFact)
                                        t = RSet(HspfTable.NumFmtRE(lConn.MFact, ilen(6) - 1), ilen(6) - 1)
                                        lConn.MFact = CDbl(t)
                                        lStr.Append(" " & t)
                                    End If
                                    lStr.Append(Space(icol(7) - lStr.Length - 1))
                                    lStr.Append(lConn.Tran)
                                    lStr.Append(Space(icol(8) - lStr.Length - 1))
                                    lStr.Append(lConn.Target.VolName)
                                    lStr.Append(Space(icol(9) - lStr.Length - 1))
                                    lStr.Append(CStr(lConn.Target.VolId).PadLeft(ilen(9)))
                                    lStr.Append(Space(icol(10) - lStr.Length - 1))
                                    If Len(lConn.Target.Member) > ilen(10) Then 'dont write more chars than there is room for
                                        lConn.Target.Member = Mid(lConn.Target.Member, 1, ilen(10))
                                    End If
                                    lStr.Append(lConn.Target.Member.TrimEnd)
                                    If (icol(11) - lStr.Length - 1 > 0) Then 'check to make sure not spacing zero or fewer characters
                                        lStr.Append(Space(icol(11) - lStr.Length - 1))
                                    End If
                                    If lConn.Target.MemSub1 <> 0 Then
                                        t = CStr(lConn.Target.MemSub1).PadLeft(ilen(11))
                                        If lConn.Target.VolName = "RCHRES" Then t = pUci.IntAsCat(lConn.Target.Member, 1, t)
                                        lStr.Append(t)
                                    End If
                                    lStr.Append(Space(icol(12) - lStr.Length - 1))
                                    lStr.Append(lConn.Ssystem)
                                    lStr.Append(Space(icol(13) - lStr.Length - 1))
                                    lStr.Append(lConn.Sgapstrg)
                                    lStr.Append(Space(icol(14) - lStr.Length - 1))
                                    lStr.Append(lConn.Amdstrg)
                                    lSB.AppendLine(lStr.ToString)
                                End If
                            Next j
                        Next k
                End Select
                lSB.AppendLine("END " & s)
            End If
        Next i
        Return lSB.ToString
    End Function

    Private Function NumericallyTheSame(ByRef ValueAsRead As String, ByRef ValueStored As Single) As Boolean
        'see if the current mfact value is the same as the value as read from the uci
        '4. is the same as 4.0
        Dim rtemp1 As Single

        NumericallyTheSame = False
        If IsNumeric(ValueStored) Then
            If IsNumeric(ValueAsRead) Then
                'simple case
                rtemp1 = CSng(ValueAsRead)
                If rtemp1 = ValueStored Then
                    NumericallyTheSame = True
                End If
            End If
        End If
    End Function
End Class