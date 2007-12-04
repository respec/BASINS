'Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license
Option Strict Off
Option Explicit On

Imports System.Text

Public Class HspfMassLink
    Private pSource As HspfSrcTar
    Private pTarget As HspfSrcTar
    Private pTran As String
    Private pMFact As Double
    Private pUci As HspfUci
    Private pMassLinkId As Integer
    Private pComment As String = ""

    Public Property MFact() As Double
        Get
            MFact = pMFact
        End Get
        Set(ByVal Value As Double)
            pMFact = Value
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
    Public Property MassLinkID() As Integer
        Get
            MassLinkID = pMassLinkId
        End Get
        Set(ByVal Value As Integer)
            pMassLinkId = Value
        End Set
    End Property

    Public ReadOnly Property EditControlName() As String
        Get
            EditControlName = "ATCoHspf.ctlMassLinkEdit"
        End Get
    End Property

    Public ReadOnly Property Caption() As String
        Get
            Caption = "Mass-Link Block"
        End Get
    End Property

    Public Sub readMassLinks(ByRef myUci As HspfUci)
        Dim retkey, init, OmCode, retcod As Integer
        Dim cbuff As String = Nothing
        Dim i, lId, curml As Integer
        Dim lMassLink As HspfMassLink
        Dim s As String
        Dim mlcnt As Integer
        Dim kwd As String = Nothing
        Dim contfg, kflg, retid As Integer
        Dim mlno(-1) As Integer
        Dim mlkeys(-1) As Integer
        Dim c, t As String
        Dim pastHeader As Boolean
        Dim rectyp As Integer

        If myUci.FastFlag Then
            mlcnt = 1
            ReDim Preserve mlkeys(mlcnt)
        Else
            OmCode = HspfOmCode("MASS-LINKS")
            lId = -101
            init = 1
            mlcnt = 0
            Do
                Call REM_GTNXKW(myUci, init, lId, kwd, kflg, contfg, retid)
                If retid <> 0 Then
                    mlcnt = mlcnt + 1
                    ReDim Preserve mlno(mlcnt)
                    ReDim Preserve mlkeys(mlcnt)
                    mlno(mlcnt - 1) = CInt(kwd)
                    mlkeys(mlcnt - 1) = retid
                End If
                init = 0
            Loop While contfg = 1
        End If

        For i = 0 To mlcnt - 1
            'loop through each mass link
            init = mlkeys(i)
            c = ""
            pastHeader = False
            retkey = -1
            Do
                If myUci.FastFlag Then
                    GetNextRecordFromBlock("MASS-LINK", retkey, cbuff, rectyp, retcod)
                    If Left(cbuff, 11) = "  MASS-LINK" Then
                        'start of a new mass link
                        curml = CShort(Mid(cbuff, 16, 5))
                        pastHeader = False
                        GetNextRecordFromBlock("MASS-LINK", retkey, cbuff, rectyp, retcod)
                    ElseIf Left(cbuff, 15) = "  END MASS-LINK" Then
                        'end of a mass link
                        rectyp = -2
                    End If
                Else
                    retkey = -1
                    Call REM_XBLOCKEX(myUci, OmCode, init, retkey, cbuff, rectyp, retcod)
                    curml = mlno(i)
                End If
                init = 0
                If rectyp = 0 Then
                    lMassLink = New HspfMassLink
                    lMassLink.Uci = myUci
                    lMassLink.MassLinkID = curml
                    lMassLink.Source.VolName = Trim(Left(cbuff, 6))
                    lMassLink.Source.VolId = 0
                    lMassLink.Source.Group = Trim(Mid(cbuff, 12, 6))
                    lMassLink.Source.Member = Trim(Mid(cbuff, 19, 6))
                    s = Trim(Mid(cbuff, 25, 2))
                    If Len(s) > 0 And IsNumeric(s) Then
                        lMassLink.Source.MemSub1 = CInt(s)
                    Else
                        If Len(s) > 0 Then lMassLink.Source.MemSub1 = myUci.CatAsInt(s)
                    End If
                    s = Trim(Mid(cbuff, 27, 2))
                    If Len(s) > 0 And IsNumeric(s) Then
                        lMassLink.Source.MemSub2 = CInt(s)
                    Else
                        If Len(s) > 0 Then lMassLink.Source.MemSub2 = myUci.CatAsInt(s)
                    End If
                    s = Trim(Mid(cbuff, 29, 10))
                    If Len(s) > 0 Then lMassLink.MFact = CDbl(s)
                    lMassLink.Tran = Trim(Mid(cbuff, 39, 4))
                    lMassLink.Target.VolName = Trim(Mid(cbuff, 44, 6))
                    lMassLink.Target.VolId = 0
                    s = Trim(Mid(cbuff, 55, 3))
                    If Len(s) > 0 Then lMassLink.Target.VolIdL = CInt(s)
                    lMassLink.Target.Group = Trim(Mid(cbuff, 59, 6))
                    lMassLink.Target.Member = Trim(Mid(cbuff, 66, 6))
                    s = Trim(Mid(cbuff, 72, 2))
                    If Len(s) > 0 And IsNumeric(s) Then
                        lMassLink.Target.MemSub1 = CInt(s)
                    Else
                        If Len(s) > 0 Then lMassLink.Target.MemSub1 = myUci.CatAsInt(s)
                    End If
                    s = Trim(Mid(cbuff, 74, 2))
                    If Len(s) > 0 And IsNumeric(s) Then
                        lMassLink.Target.MemSub2 = CInt(s)
                    Else
                        If Len(s) > 0 Then lMassLink.Target.MemSub2 = myUci.CatAsInt(s)
                    End If
                    lMassLink.Comment = c
                    myUci.MassLinks.Add(lMassLink)
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
                If retcod <> 2 Then Exit Do
            Loop
        Next i
    End Sub

    Public Sub New()
        MyBase.New()
        pSource = New HspfSrcTar
        pTarget = New HspfSrcTar
        pMFact = 1
    End Sub

    Public Sub Edit()
        editInit(Me, Me.Uci.icon, True)
    End Sub

    Public Overrides Function ToString() As String
        Dim s As String
        Dim lSB As New StringBuilder
        Dim lBlockDef As HspfBlockDef
        Dim lTableDef As HspfTableDef
        Dim j, i, k As Integer
        Dim typeexists(4) As Boolean
        Dim icol(15) As Integer
        Dim ilen(15) As Integer
        Dim lParmDef As HSPFParmDef
        Dim t As String
        Dim mlno(-1) As Integer
        Dim mlcnt As Integer = 0
        Dim found As Boolean

        For Each lML As HspfMassLink In Uci.MassLinks
            found = False
            For k = 0 To mlcnt - 1
                If lML.MassLinkID = mlno(k) Then
                    found = True
                End If
            Next k
            If found = False Then
                mlcnt = mlcnt + 1
                ReDim Preserve mlno(mlcnt)
                mlno(mlcnt - 1) = lML.MassLinkID
            End If
        Next lML

        s = "MASS-LINK"
        lBlockDef = pUci.Msg.BlockDefs.Item(s)
        lTableDef = lBlockDef.TableDefs.Item(1)
        'get lengths and starting positions
        j = 0
        For Each lParmDef In lTableDef.ParmDefs
            icol(j) = lParmDef.StartCol
            ilen(j) = lParmDef.Length
            j = j + 1
        Next lParmDef
        lSB.AppendLine(s)

        For i = 1 To mlcnt
            lSB.AppendLine(" ")
            lSB.AppendLine("  MASS-LINK    " & CStr(mlno(i - 1)).PadLeft(5))
            'now start building the records
            lSB.AppendLine("<-Volume-> <-Grp> <-Member-><--Mult-->     <-Target vols> <-Grp> <-Member->  ***")
            lSB.AppendLine("<Name>            <Name> x x<-factor->     <Name>                <Name> x x  ***")
            For Each lML As HspfMassLink In Uci.MassLinks
                If lML.MassLinkID = mlno(i - 1) Then
                    If lML.Comment.Length > 0 Then
                        lSB.AppendLine(lML.Comment)
                    End If
                    Dim lStr As New StringBuilder
                    lStr.Append(lML.Source.VolName.Trim)
                    lStr.Append(Space(icol(1) - lStr.Length - 1)) 'pad prev field
                    lStr.Append(lML.Source.Group)
                    lStr.Append(Space(icol(2) - lStr.Length - 1))
                    lStr.Append(lML.Source.Member)
                    lStr.Append(Space(icol(3) - lStr.Length - 1))
                    If lML.Source.MemSub1 <> 0 Then
                        t = CStr(lML.Source.MemSub1).PadLeft(ilen(3))
                        If lML.Source.VolName = "RCHRES" Then t = pUci.IntAsCat(lML.Source.Member, 1, t)
                        lStr.Append(t)
                    End If
                    lStr.Append(Space(icol(4) - lStr.Length - 1))
                    If lML.Source.MemSub2 <> 0 Then
                        t = CStr(lML.Source.MemSub2).PadLeft(ilen(4))
                        If lML.Source.VolName = "RCHRES" Then t = pUci.IntAsCat(lML.Source.Member, 2, t)
                        lStr.Append(t)
                    End If
                    lStr.Append(Space(icol(5) - lStr.Length - 1))
                    If lML.MFact <> 1 Then
                        lStr.Append(CStr(lML.MFact).PadLeft(ilen(5)))
                    End If
                    lStr.Append(Space(icol(6) - lStr.Length - 1))
                    'str = str & lML.Tran
                    'str = str & Space(icol(7) - Len(str) - 1)
                    lStr.Append(lML.Target.VolName)
                    lStr.Append(Space(icol(7) - lStr.Length - 1))
                    lStr.Append(lML.Target.Group)
                    lStr.Append(Space(icol(8) - lStr.Length - 1))
                    lStr.Append(lML.Target.Member)
                    lStr.Append(Space(icol(9) - lStr.Length - 1))
                    If lML.Target.MemSub1 <> 0 Then
                        t = CStr(lML.Target.MemSub1).PadLeft(ilen(9))
                        If lML.Target.VolName = "RCHRES" Then t = pUci.IntAsCat(lML.Target.Member, 1, t)
                        lStr.Append(t)
                    End If
                    lStr.Append(Space(icol(10) - lStr.Length - 1))
                    If lML.Target.MemSub2 <> 0 Then
                        t = Space(ilen(10))
                        t = RSet(CStr(lML.Target.MemSub2), Len(t))
                        If lML.Target.VolName = "RCHRES" Then t = pUci.IntAsCat(lML.Target.Member, 2, t)
                        lStr.Append(t)
                    End If
                    lSB.AppendLine(lStr.ToString)
                End If
            Next lML
            lSB.AppendLine("  END MASS-LINK" & CStr(mlno(i - 1)).PadLeft(5))
        Next i
        lSB.AppendLine("END " & s)
        Return lSB.ToString
    End Function

    Public Function FindMassLinkID(ByRef sname As String, ByRef tname As String) As Integer
        For Each lMassLink As HspfMassLink In pUci.MassLinks
            If lMassLink.Source.VolName = sname And lMassLink.Target.VolName = tname Then
                Return lMassLink.MassLinkID
            End If
        Next lMassLink
        Return -1
    End Function
End Class