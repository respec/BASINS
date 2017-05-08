VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "HspfConnection"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'Copyright 2002 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Dim pMFact As Double
Dim pMFactAsRead As String
Dim pTyp As Long '1-ExtSource,2-Network,3-Schematic,4-ExtTarget
Dim pTran As String
Dim pSgapstrg As String
Dim pAmdstrg As String
Dim pSsystem As String
Dim pSource As HspfSrcTar
Dim pTarget As HspfSrcTar
Dim pMassLink As Long
Dim pUci As HspfUci
Dim DesiredType As String
Dim pComment As String

Public Property Get MFact() As Double
  MFact = pMFact
End Property
Public Property Let MFact(newMFact As Double)
  pMFact = newMFact
End Property

Public Property Get MFactAsRead() As String
  MFactAsRead = pMFactAsRead
End Property
Public Property Let MFactAsRead(newMFactAsRead As String)
  pMFactAsRead = newMFactAsRead
End Property

Public Property Get Uci() As HspfUci
  Set Uci = pUci
End Property
Public Property Set Uci(newUci As HspfUci)
  Set pUci = newUci
End Property

Public Property Get Source() As HspfSrcTar
  Set Source = pSource
End Property
Public Property Set Source(newSource As HspfSrcTar)
  Set pSource = newSource
End Property

Public Property Get Target() As HspfSrcTar
  Set Target = pTarget
End Property
Public Property Set Target(newTarget As HspfSrcTar)
  Set pTarget = newTarget
End Property

Public Property Get Tran() As String
  Tran = pTran
End Property
Public Property Let Tran(newTran As String)
  pTran = newTran
End Property

Public Property Get Comment() As String
  Comment = pComment
End Property

Public Property Let Comment(newComment As String)
  pComment = newComment
End Property

Public Property Get Ssystem() As String
  Ssystem = pSsystem
End Property
Public Property Let Ssystem(newSsystem As String)
  pSsystem = newSsystem
End Property

Public Property Get Sgapstrg() As String
  Sgapstrg = pSgapstrg
End Property
Public Property Let Sgapstrg(newSgapstrg As String)
  pSgapstrg = newSgapstrg
End Property

Public Property Get Amdstrg() As String
  Amdstrg = pAmdstrg
End Property
Public Property Let Amdstrg(newAmdstrg As String)
  pAmdstrg = newAmdstrg
End Property
Public Property Get Typ() As Long
  Typ = pTyp
End Property
Public Property Let Typ(newTyp As Long)
  pTyp = newTyp
End Property

Public Property Get MassLink() As Long
  MassLink = pMassLink
End Property
Public Property Let MassLink(newMassLink As Long)
  pMassLink = newMassLink
End Property
Public Sub readTimSer(myUci As HspfUci)
  Dim init&, OmCode&, retkey&, cbuff$, retcod&, rectyp&
  Dim opnam$, opf&, opl&
  Dim lConnection As HspfConnection, s$, c$
  Dim pastHeader As Boolean, t$

  Set pUci = myUci
  OmCode = HspfOmCode("EXT SOURCES")
  init = 1
  c = ""
  pastHeader = False
  retkey = -1
  Do
    If myUci.FastFlag Then
      GetNextRecordFromBlock "EXT SOURCES", retkey, cbuff, rectyp, retcod
    Else
      retkey = -1
      Call REM_XBLOCKEX(myUci, OmCode, init, retkey, cbuff, rectyp, retcod)
    End If
    If retcod <> 2 Then Exit Do
    init = 0
    If rectyp = 0 Then
      Set lConnection = New HspfConnection
      Set lConnection.Uci = myUci
      lConnection.Typ = 1
      lConnection.Source.VolName = Trim(Left(cbuff, 6))
      lConnection.Source.VolId = Mid(cbuff, 7, 4)
      lConnection.Source.Member = Trim(Mid(cbuff, 12, 6))
      s = Trim(Mid(cbuff, 18, 2))
      If Len(s) > 0 Then lConnection.Source.MemSub1 = s
      lConnection.Ssystem = Mid(cbuff, 21, 4)
      lConnection.Sgapstrg = Mid(cbuff, 25, 4)
      s = Trim(Mid(cbuff, 29, 10))
      lConnection.MFactAsRead = Mid(cbuff, 29, 10)
      If Len(s) > 0 Then lConnection.MFact = s
      s = Mid(cbuff, 39, 4)
      If Len(s) > 0 Then lConnection.Tran = s
      lConnection.Target.VolName = Trim(Mid(cbuff, 44, 6))
      lConnection.Target.VolId = Mid(cbuff, 51, 3)
      s = Trim(Mid(cbuff, 55, 3))
      If Len(s) > 0 Then lConnection.Target.VolIdL = s
      lConnection.Target.Group = Trim(Mid(cbuff, 59, 6))
      lConnection.Target.Member = Trim(Mid(cbuff, 66, 6))
      s = Trim(Mid(cbuff, 72, 2))
      If Len(s) > 0 And IsNumeric(s) Then
        lConnection.Target.MemSub1 = s
      Else
        If Len(s) > 0 Then lConnection.Target.MemSub1 = myUci.CatAsInt(s)
      End If
      s = Trim(Mid(cbuff, 74, 2))
      If Len(s) > 0 And IsNumeric(s) Then
        lConnection.Target.MemSub2 = s
      Else
        If Len(s) > 0 Then lConnection.Target.MemSub2 = myUci.CatAsInt(s)
      End If
      lConnection.Comment = c
      myUci.Connections.Add lConnection
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
      GetNextRecordFromBlock "NETWORK", retkey, cbuff, rectyp, retcod
    Else
      retkey = -1
      Call REM_XBLOCKEX(myUci, OmCode, init, retkey, cbuff, rectyp, retcod)
    End If
    If retcod <> 2 Then Exit Do
    init = 0
    If rectyp = 0 Then
      Set lConnection = New HspfConnection
      Set lConnection.Uci = myUci
      lConnection.Typ = 2
      lConnection.Source.VolName = Trim(Left(cbuff, 6))
      lConnection.Source.VolId = Mid(cbuff, 7, 4)
      lConnection.Source.Group = Trim(Mid(cbuff, 12, 6))
      lConnection.Source.Member = Trim(Mid(cbuff, 19, 6))
      s = Trim(Mid(cbuff, 25, 2))
      If Len(s) > 0 And IsNumeric(s) Then
        lConnection.Source.MemSub1 = s
      Else
        If Len(s) > 0 Then lConnection.Source.MemSub1 = myUci.CatAsInt(s)
      End If
      s = Trim(Mid(cbuff, 27, 2))
      If Len(s) > 0 And IsNumeric(s) Then
        lConnection.Source.MemSub2 = s
      Else
        If Len(s) > 0 Then lConnection.Source.MemSub2 = myUci.CatAsInt(s)
      End If
      s = Trim(Mid(cbuff, 29, 10))
      lConnection.MFactAsRead = Mid(cbuff, 29, 10)
      If Len(s) > 0 Then lConnection.MFact = s
      lConnection.Tran = Trim(Mid(cbuff, 39, 4))
      lConnection.Target.VolName = Trim(Mid(cbuff, 44, 6))
      lConnection.Target.VolId = Mid(cbuff, 51, 3)
      s = Trim(Mid(cbuff, 55, 3))
      If Len(s) > 0 Then lConnection.Target.VolIdL = s
      lConnection.Target.Group = Trim(Mid(cbuff, 59, 6))
      lConnection.Target.Member = Trim(Mid(cbuff, 66, 6))
      s = Trim(Mid(cbuff, 72, 2))
      If Len(s) > 0 And IsNumeric(s) Then
        lConnection.Target.MemSub1 = s
      Else
        If Len(s) > 0 Then lConnection.Target.MemSub1 = myUci.CatAsInt(s)
      End If
      s = Trim(Mid(cbuff, 74, 2))
      If Len(s) > 0 And IsNumeric(s) Then
        lConnection.Target.MemSub2 = s
      Else
        If Len(s) > 0 Then lConnection.Target.MemSub2 = myUci.CatAsInt(s)
      End If
      lConnection.Comment = c
      myUci.Connections.Add lConnection
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
      GetNextRecordFromBlock "SCHEMATIC", retkey, cbuff, rectyp, retcod
    Else
      retkey = -1
      Call REM_XBLOCKEX(myUci, OmCode, init, retkey, cbuff, rectyp, retcod)
    End If
    If retcod <> 2 Then Exit Do
    init = 0
    If rectyp = 0 Then
      Set lConnection = New HspfConnection
      Set lConnection.Uci = myUci
      lConnection.Typ = 3
      lConnection.Source.VolName = Trim(Left(cbuff, 6))
      lConnection.Source.VolId = Mid(cbuff, 7, 4)
      s = Trim(Mid(cbuff, 29, 10))
      lConnection.MFactAsRead = Mid(cbuff, 29, 10)
      If Len(s) > 0 Then lConnection.MFact = s
      lConnection.Target.VolName = Trim(Mid(cbuff, 44, 6))
      lConnection.Target.VolId = Mid(cbuff, 50, 4)
      lConnection.MassLink = Mid(cbuff, 57, 4)
      s = Trim(Mid(cbuff, 72, 2))
      If Len(s) > 0 And IsNumeric(s) Then
        lConnection.Target.MemSub1 = s
      Else
        If Len(s) > 0 Then lConnection.Target.MemSub1 = myUci.CatAsInt(s)
      End If
      s = Trim(Mid(cbuff, 74, 2))
      If Len(s) > 0 And IsNumeric(s) Then
        lConnection.Target.MemSub2 = s
      Else
        If Len(s) > 0 Then lConnection.Target.MemSub2 = myUci.CatAsInt(s)
      End If
      lConnection.Comment = c
      myUci.Connections.Add lConnection
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
      GetNextRecordFromBlock "EXT TARGETS", retkey, cbuff, rectyp, retcod
    Else
      retkey = -1
      Call REM_XBLOCKEX(myUci, OmCode, init, retkey, cbuff, rectyp, retcod)
    End If
    If retcod <> 2 Then Exit Do
    init = 0
    If rectyp = 0 Then
      Set lConnection = New HspfConnection
      Set lConnection.Uci = myUci
      lConnection.Typ = 4
      lConnection.Source.VolName = Trim(Left(cbuff, 6))
      lConnection.Source.VolId = Mid(cbuff, 7, 4)
      lConnection.Source.Group = Trim(Mid(cbuff, 12, 6))
      lConnection.Source.Member = Trim(Mid(cbuff, 19, 6))
      s = Trim(Mid(cbuff, 25, 2))
      If Len(s) > 0 And IsNumeric(s) Then
        lConnection.Source.MemSub1 = s
      Else
        If Len(s) > 0 Then lConnection.Source.MemSub1 = myUci.CatAsInt(s)
      End If
      s = Trim(Mid(cbuff, 27, 2))
      If Len(s) > 0 And IsNumeric(s) Then
        lConnection.Source.MemSub2 = s
      Else
        If Len(s) > 0 Then lConnection.Source.MemSub1 = myUci.CatAsInt(s)
      End If
      s = Trim(Mid(cbuff, 29, 10))
      lConnection.MFactAsRead = Mid(cbuff, 29, 10)
      If Len(s) > 0 Then lConnection.MFact = s
      s = Trim(Mid(cbuff, 39, 4))
      If Len(s) > 0 Then lConnection.Tran = s
      lConnection.Target.VolName = Trim(Mid(cbuff, 44, 6))
      lConnection.Target.VolId = Mid(cbuff, 50, 4)
      lConnection.Target.Member = Trim(Mid(cbuff, 55, 6))
      s = Trim(Mid(cbuff, 61, 2))
      If Len(s) > 0 Then lConnection.Target.MemSub1 = s
      lConnection.Ssystem = Trim(Mid(cbuff, 64, 4))
      lConnection.Sgapstrg = Trim(Mid(cbuff, 69, 4))
      lConnection.Amdstrg = Trim(Mid(cbuff, 74, 4))
      lConnection.Comment = c
      myUci.Connections.Add lConnection
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

Private Sub Class_Initialize()
  Set pSource = New HspfSrcTar
  Set pTarget = New HspfSrcTar
  pTyp = 0
  pMFact = 1#
End Sub
Public Property Get EditControlName() As String
  EditControlName = "ATCoHspf.ctlConnectionEdit"
End Property
Public Sub EditExtSrc()
  DesiredType = "EXT SOURCES"
  editInit Me, Me.Uci.icon, True 'add remove ok
End Sub
Public Sub EditExtTar()
  DesiredType = "EXT TARGETS"
  editInit Me, Me.Uci.icon, True 'add remove ok
End Sub
Public Sub EditNetwork()
  DesiredType = "NETWORK"
  editInit Me, Me.Uci.icon, True 'add remove ok
End Sub
Public Sub EditSchematic()
  DesiredType = "SCHEMATIC"
  editInit Me, Me.Uci.icon, True 'add remove ok
End Sub
Public Property Get DesiredRecordType() As String
  DesiredRecordType = DesiredType
End Property
Public Property Get Caption() As String
  Caption = DesiredType & " Block"
End Property

Public Sub WriteUciFile(f As Integer, M As HspfMsg)
  Dim s$, e$, str$, optyp$
  Dim lBlockDef As HspfBlockDef
  Dim vTableDef As Variant, lTableDef As HspfTableDef
  Dim firstTable As Boolean
  Dim i&, j&, k&, typeexists(4) As Boolean, icol&(15), ilen&(15)
  Dim lOper As HspfOperation
  Dim lConn As HspfConnection
  Dim lOpnSeqBlock As HspfOpnSeqBlk
  Dim lParmDef As Variant, t$
  Dim vMetSeg As Variant, lMetSeg As HspfMetSeg
  Dim vOpTyps As Variant, vOpTyp As Variant, vPtSrc As Variant
  Dim lPtSrc As HspfPoint
  
  typeexists(0) = False 'ext sou
  typeexists(1) = False 'network
  typeexists(2) = False 'schematic
  typeexists(3) = False 'ext tar
  
  If pUci.MetSegs.Count > 0 Then
    typeexists(0) = True
  End If
  If pUci.PointSources.Count > 0 Then
    typeexists(0) = True
  End If

  Set lOpnSeqBlock = pUci.OpnSeqBlock
  For i = 1 To lOpnSeqBlock.Opns.Count
    Set lOper = lOpnSeqBlock.Opn(i)
    For j = 1 To lOper.Targets.Count
      Set lConn = lOper.Targets(j)
      typeexists(lConn.Typ - 1) = True
    Next j
    For j = 1 To lOper.Sources.Count
      Set lConn = lOper.Sources(j)
      typeexists(lConn.Typ - 1) = True
    Next j
  Next i
  
  For i = 1 To 4
    If typeexists(i - 1) Then
      Select Case i
        Case 1: s = "EXT SOURCES"
        Case 2: s = "NETWORK"
        Case 3: s = "SCHEMATIC"
        Case 4: s = "EXT TARGETS"
      End Select
      Set lBlockDef = M.BlockDefs(s)
      Set lTableDef = lBlockDef.TableDefs(1)
      'get lengths and starting positions
      j = 0
      For Each lParmDef In lTableDef.ParmDefs
        icol(j) = lParmDef.StartCol
        ilen(j) = lParmDef.Length
        j = j + 1
      Next lParmDef
      Print #f, " "
      Print #f, s
      'now start building the records
      Select Case i
        Case 1:  'ext srcs
          Print #f, "<-Volume-> <Member> SsysSgap<--Mult-->Tran <-Target vols> <-Grp> <-Member-> ***"
          Print #f, "<Name>   x <Name> x tem strg<-factor->strg <Name>   x   x        <Name> x x ***"
          'do met segs
          vOpTyps = Array("PERLND", "IMPLND", "RCHRES") 'operations with assoc met segs
          For Each vOpTyp In vOpTyps
            For Each vMetSeg In pUci.MetSegs
              Set lMetSeg = vMetSeg
              optyp = vOpTyp
              lMetSeg.WriteUciFile optyp, icol, ilen, f
            Next vMetSeg
          Next vOpTyp
          'do point sources
          If pUci.PointSources.Count > 0 And pUci.MetSegs.Count > 0 Then
            Print #f, ""  'write a blank line between met segs and pt srcs
          End If
          For Each vPtSrc In pUci.PointSources
            Set lPtSrc = vPtSrc
            lPtSrc.WriteUciFile icol, ilen, f
          Next vPtSrc
          'now do everything else
          For k = 1 To lOpnSeqBlock.Opns.Count
            Set lOper = lOpnSeqBlock.Opn(k)
            For j = 1 To lOper.Sources.Count
              Set lConn = lOper.Sources(j)
              If lConn.Typ = i Then
                str = Trim(lConn.Source.VolName)
                str = str & Space(icol(1) - Len(str) - 1) 'pad prev field
                t = Space(ilen(1)) 'right justify numbers
                RSet t = CStr(lConn.Source.VolId)
                str = str & t
                str = str & Space(icol(2) - Len(str) - 1)
                str = str & lConn.Source.Member
                str = str & Space(icol(3) - Len(str) - 1)
                If lConn.Source.MemSub1 <> 0 Then
                  t = Space(ilen(3))
                  RSet t = CStr(lConn.Source.MemSub1)
                  If lConn.Source.VolName = "RCHRES" Then t = pUci.IntAsCat(lConn.Source.Member, 1, t)
                  str = str & t
                End If
                str = str & Space(icol(4) - Len(str) - 1)
                str = str & lConn.Ssystem
                str = str & Space(icol(5) - Len(str) - 1)
                str = str & lConn.Sgapstrg
                str = str & Space(icol(6) - Len(str) - 1)
                If NumericallyTheSame(lConn.MFactAsRead, lConn.MFact) Then
                  t = lConn.MFactAsRead
                  str = str & t
                ElseIf lConn.MFact <> 1 Then
                  t = Space(ilen(6))
                  RSet t = CStr(lConn.MFact)
                  str = str & t
                End If
                str = str & Space(icol(7) - Len(str) - 1)
                str = str & lConn.Tran
                str = str & Space(icol(8) - Len(str) - 1)
                str = str & lOper.Name
                str = str & Space(icol(9) - Len(str) - 1)
                t = Space(ilen(9))
                RSet t = CStr(lOper.Id)
                str = str & t
                str = str & Space(icol(11) - Len(str) - 1)
                str = str & lConn.Target.Group
                str = str & Space(icol(12) - Len(str) - 1)
                str = str & lConn.Target.Member
                str = str & Space(icol(13) - Len(str) - 1)
                If lConn.Target.MemSub1 <> 0 Then
                  t = Space(ilen(13))
                  RSet t = CStr(lConn.Target.MemSub1)
                  If lConn.Target.VolName = "RCHRES" Then t = pUci.IntAsCat(lConn.Target.Member, 1, t)
                  str = str & t
                End If
                str = str & Space(icol(14) - Len(str) - 1)
                If lConn.Target.MemSub2 <> 0 Then
                  t = Space(ilen(14))
                  RSet t = CStr(lConn.Target.MemSub2)
                  If lConn.Target.VolName = "RCHRES" Then t = pUci.IntAsCat(lConn.Target.Member, 2, t)
                  str = str & t
                End If
                If Len(lConn.Comment) > 0 Then
                  Print #f, lConn.Comment
                End If
                Print #f, str
              End If
            Next j
          Next k
        Case 2:  'network
          Print #f, "<-Volume-> <-Grp> <-Member-><--Mult-->Tran <-Target vols> <-Grp> <-Member->  ***"
          Print #f, "<Name>   x        <Name> x x<-factor->strg <Name>   x   x        <Name> x x  ***"
          For k = 1 To lOpnSeqBlock.Opns.Count
            Set lOper = lOpnSeqBlock.Opn(k)
            For j = 1 To lOper.Sources.Count   'used to go thru targets, misses range
              Set lConn = lOper.Sources(j)
              If lConn.Typ = i Then
                str = Trim(lConn.Source.VolName)
                str = str & Space(icol(1) - Len(str) - 1) 'pad prev field
                t = Space(ilen(1)) 'right justify numbers
                RSet t = CStr(lConn.Source.VolId)
                str = str & t
                str = str & Space(icol(2) - Len(str) - 1)
                str = str & lConn.Source.Group
                str = str & Space(icol(3) - Len(str) - 1)
                str = str & lConn.Source.Member
                str = str & Space(icol(4) - Len(str) - 1)
                If lConn.Source.MemSub1 <> 0 Then
                  t = Space(ilen(4))
                  RSet t = CStr(lConn.Source.MemSub1)
                  If lConn.Source.VolName = "RCHRES" Then t = pUci.IntAsCat(lConn.Source.Member, 1, t)
                  str = str & t
                End If
                str = str & Space(icol(5) - Len(str) - 1)
                If lConn.Source.MemSub2 <> 0 Then
                  t = Space(ilen(5))
                  RSet t = CStr(lConn.Source.MemSub2)
                  If lConn.Source.VolName = "RCHRES" Then t = pUci.IntAsCat(lConn.Source.Member, 2, t)
                  str = str & t
                End If
                str = str & Space(icol(6) - Len(str) - 1)
                If NumericallyTheSame(lConn.MFactAsRead, lConn.MFact) Then
                  t = lConn.MFactAsRead
                  str = str & t
                ElseIf lConn.MFact <> 1 Then
                  t = Space(ilen(6))
                  RSet t = CStr(lConn.MFact)
                  str = str & t
                End If
                str = str & Space(icol(7) - Len(str) - 1)
                str = str & lConn.Tran
                str = str & Space(icol(8) - Len(str) - 1)
                str = str & lOper.Name
                str = str & Space(icol(9) - Len(str) - 1)
                t = Space(ilen(9))
                RSet t = CStr(lOper.Id)
                str = str & t
                str = str & Space(icol(11) - Len(str) - 1)
                str = str & lConn.Target.Group
                str = str & Space(icol(12) - Len(str) - 1)
                str = str & lConn.Target.Member
                str = str & Space(icol(13) - Len(str) - 1)
                If lConn.Target.MemSub1 <> 0 Then
                  t = Space(ilen(13))
                  RSet t = CStr(lConn.Target.MemSub1)
                  If lConn.Target.VolName = "RCHRES" Then t = pUci.IntAsCat(lConn.Target.Member, 1, t)
                  str = str & t
                End If
                str = str & Space(icol(14) - Len(str) - 1)
                If lConn.Target.MemSub2 <> 0 Then
                  t = Space(ilen(14))
                  RSet t = CStr(lConn.Target.MemSub2)
                  If lConn.Target.VolName = "RCHRES" Then t = pUci.IntAsCat(lConn.Target.Member, 2, t)
                  str = str & t
                End If
                If Len(lConn.Comment) > 0 Then
                  Print #f, lConn.Comment
                End If
                Print #f, str
              End If
            Next j
          Next k
        Case 3:  'schematic
          Print #f, "<-Volume->                  <--Area-->     <-Volume->  <ML#> ***       <sb>"
          Print #f, "<Name>   x                  <-factor->     <Name>   x        ***        x x"
          For k = 1 To lOpnSeqBlock.Opns.Count
            Set lOper = lOpnSeqBlock.Opn(k)
            For j = 1 To lOper.Sources.Count
              Set lConn = lOper.Sources(j)
              If lConn.Typ = i Then
                str = Trim(lConn.Source.VolName)
                str = str & Space(icol(1) - Len(str) - 1) 'pad prev field
                t = Space(ilen(1)) 'right justify numbers
                RSet t = CStr(lConn.Source.VolId)
                str = str & t
                str = str & Space(icol(2) - Len(str) - 1)
                If NumericallyTheSame(lConn.MFactAsRead, lConn.MFact) Then
                  t = lConn.MFactAsRead
                  str = str & t
                ElseIf lConn.MFact <> 1 Then
                  lConn.MFact = Round(lConn.MFact, 2)
                  t = Space(ilen(2))
                  RSet t = CStr(lConn.MFact)
                  str = str & t
                End If
                str = str & Space(icol(3) - Len(str) - 1)
                str = str & lConn.Target.VolName
                str = str & Space(icol(4) - Len(str) - 1)
                t = Space(ilen(5))
                RSet t = CStr(lConn.Target.VolId)
                str = str & t
                str = str & Space(icol(5) - Len(str) - 1)
                t = Space(ilen(5))
                RSet t = CStr(lConn.MassLink)
                str = str & t
                If lConn.Target.MemSub1 > 0 Then
                  str = str & Space(icol(6) - Len(str) - 1)
                  t = Space(ilen(6))
                  RSet t = CStr(lConn.Target.MemSub1)
                  If lConn.Target.VolName = "RCHRES" Then t = pUci.IntAsCat(lConn.Target.Member, 1, t)
                  str = str & t
                End If
                If lConn.Target.MemSub2 > 0 Then
                  str = str & Space(icol(7) - Len(str) - 1)
                  t = Space(ilen(7))
                  RSet t = CStr(lConn.Target.MemSub2)
                  If lConn.Target.VolName = "RCHRES" Then t = pUci.IntAsCat(lConn.Target.Member, 2, t)
                  str = str & t
                End If
                If Len(lConn.Comment) > 0 Then
                  Print #f, lConn.Comment
                End If
                Print #f, str
              End If
            Next j
          Next k
        Case 4:  'ext targ
          Print #f, "<-Volume-> <-Grp> <-Member-><--Mult-->Tran <-Volume-> <Member> Tsys Aggr Amd ***"
          Print #f, "<Name>   x        <Name> x x<-factor->strg <Name>   x <Name>qf  tem strg strg***"
          For k = 1 To lOpnSeqBlock.Opns.Count
            Set lOper = lOpnSeqBlock.Opn(k)
            For j = 1 To lOper.Targets.Count
              Set lConn = lOper.Targets(j)
              If lConn.Typ = i Then
                str = Trim(lConn.Source.VolName)
                str = str & Space(icol(1) - Len(str) - 1) 'pad prev field
                t = Space(ilen(1)) 'right justify numbers
                RSet t = CStr(lConn.Source.VolId)
                str = str & t
                str = str & Space(icol(2) - Len(str) - 1)
                str = str & lConn.Source.Group
                str = str & Space(icol(3) - Len(str) - 1)
                str = str & lConn.Source.Member
                str = str & Space(icol(4) - Len(str) - 1)
                If lConn.Source.MemSub1 <> 0 Then
                  t = Space(ilen(4))
                  RSet t = CStr(lConn.Source.MemSub1)
                  If lConn.Source.VolName = "RCHRES" Then t = pUci.IntAsCat(lConn.Source.Member, 1, t)
                  str = str & t
                End If
                str = str & Space(icol(5) - Len(str) - 1)
                If lConn.Source.MemSub2 <> 0 Then
                  t = Space(ilen(5))
                  RSet t = CStr(lConn.Source.MemSub2)
                  If lConn.Source.VolName = "RCHRES" Then t = pUci.IntAsCat(lConn.Source.Member, 2, t)
                  str = str & t
                End If
                str = str & Space(icol(6) - Len(str) - 1)
                If NumericallyTheSame(lConn.MFactAsRead, lConn.MFact) Then
                  t = lConn.MFactAsRead
                  str = str & t
                ElseIf lConn.MFact <> 1 Then
                  t = Space(ilen(6) - 1)
                  'lConn.MFact = Format(lConn.MFact, "0.#######")
                  'RSet t = CStr(lConn.MFact)
                  RSet t = NumFmtRE(lConn.MFact, ilen(6) - 1)
                  lConn.MFact = t
                  str = str & " " & t
                End If
                str = str & Space(icol(7) - Len(str) - 1)
                str = str & lConn.Tran
                str = str & Space(icol(8) - Len(str) - 1)
                str = str & lConn.Target.VolName
                str = str & Space(icol(9) - Len(str) - 1)
                t = Space(ilen(9))
                RSet t = CStr(lConn.Target.VolId)
                str = str & t
                str = str & Space(icol(10) - Len(str) - 1)
                If Len(lConn.Target.Member) > ilen(10) Then 'dont write more chars than there is room for
                  lConn.Target.Member = Mid(lConn.Target.Member, 1, ilen(10))
                End If
                str = Trim(str & lConn.Target.Member)
                If (icol(11) - Len(str) - 1 > 0) Then 'check to make sure not spacing zero or fewer characters
                  str = str & Space(icol(11) - Len(str) - 1)
                End If
                If lConn.Target.MemSub1 <> 0 Then
                  t = Space(ilen(11))
                  RSet t = CStr(lConn.Target.MemSub1)
                  If lConn.Target.VolName = "RCHRES" Then t = pUci.IntAsCat(lConn.Target.Member, 1, t)
                  str = str & t
                End If
                str = str & Space(icol(12) - Len(str) - 1)
                str = str & lConn.Ssystem
                str = str & Space(icol(13) - Len(str) - 1)
                str = str & lConn.Sgapstrg
                str = str & Space(icol(14) - Len(str) - 1)
                str = str & lConn.Amdstrg
                If Len(lConn.Comment) > 0 Then
                  Print #f, lConn.Comment
                End If
                Print #f, str
              End If
            Next j
          Next k
      End Select
      Print #f, "END " & s
    End If
  Next i
End Sub

Private Function NumericallyTheSame(ValueAsRead$, ValueStored!) As Boolean
  'see if the current mfact value is the same as the value as read from the uci
  '4. is the same as 4.0
  Dim rtemp1!
  
  NumericallyTheSame = False
  If IsNumeric(ValueStored) Then
    If IsNumeric(ValueAsRead) Then
      'simple case
      rtemp1 = ValueAsRead
      If rtemp1 = ValueStored Then
        NumericallyTheSame = True
      End If
    End If
  End If
End Function
