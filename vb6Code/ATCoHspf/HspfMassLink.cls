VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "HspfMassLink"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'Copyright 2002 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Dim pSource As HspfSrcTar
Dim pTarget As HspfSrcTar
Dim pTran As String
Dim pMFact As Double
Dim pUci As HspfUci
Dim pMassLinkId As Long
Dim pComment As String

Public Property Get MFact() As Double
  MFact = pMFact
End Property
Public Property Let MFact(newMFact As Double)
  pMFact = newMFact
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
Public Property Get MassLinkID() As Long
  MassLinkID = pMassLinkId
End Property
Public Property Let MassLinkID(newMassLinkID As Long)
  pMassLinkId = newMassLinkID
End Property
Public Sub readMassLinks(myUci As HspfUci)
  Dim init&, OmCode&, retkey&, cbuff$, retcod&
  Dim opnam$, opf&, opl&, lId&, i&, curml&
  Dim lMassLink As HspfMassLink, s$, mlcnt&
  Dim kwd$, kflg&, contfg&, retid&, mlno&(), mlkeys&()
  Dim c$, pastHeader As Boolean, rectyp&, t$

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
        mlno(mlcnt - 1) = kwd
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
        GetNextRecordFromBlock "MASS-LINK", retkey, cbuff, rectyp, retcod
        If Left(cbuff, 11) = "  MASS-LINK" Then
          'start of a new mass link
          curml = CInt(Mid(cbuff, 16, 5))
          pastHeader = False
          GetNextRecordFromBlock "MASS-LINK", retkey, cbuff, rectyp, retcod
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
        Set lMassLink = New HspfMassLink
        Set lMassLink.Uci = myUci
        lMassLink.MassLinkID = curml
        lMassLink.Source.VolName = Trim(Left(cbuff, 6))
        lMassLink.Source.VolId = 0
        lMassLink.Source.Group = Trim(Mid(cbuff, 12, 6))
        lMassLink.Source.member = Trim(Mid(cbuff, 19, 6))
        s = Trim(Mid(cbuff, 25, 2))
        If Len(s) > 0 And IsNumeric(s) Then
          lMassLink.Source.MemSub1 = s
        Else
          If Len(s) > 0 Then lMassLink.Source.MemSub1 = myUci.CatAsInt(s)
        End If
        s = Trim(Mid(cbuff, 27, 2))
        If Len(s) > 0 And IsNumeric(s) Then
          lMassLink.Source.MemSub2 = s
        Else
          If Len(s) > 0 Then lMassLink.Source.MemSub2 = myUci.CatAsInt(s)
        End If
        s = Trim(Mid(cbuff, 29, 10))
        If Len(s) > 0 Then lMassLink.MFact = s
        lMassLink.Tran = Trim(Mid(cbuff, 39, 4))
        lMassLink.Target.VolName = Trim(Mid(cbuff, 44, 6))
        lMassLink.Target.VolId = 0
        s = Trim(Mid(cbuff, 55, 3))
        If Len(s) > 0 Then lMassLink.Target.VolIdL = s
        lMassLink.Target.Group = Trim(Mid(cbuff, 59, 6))
        lMassLink.Target.member = Trim(Mid(cbuff, 66, 6))
        s = Trim(Mid(cbuff, 72, 2))
        If Len(s) > 0 And IsNumeric(s) Then
          lMassLink.Target.MemSub1 = s
        Else
          If Len(s) > 0 Then lMassLink.Target.MemSub1 = myUci.CatAsInt(s)
        End If
        s = Trim(Mid(cbuff, 74, 2))
        If Len(s) > 0 And IsNumeric(s) Then
          lMassLink.Target.MemSub2 = s
        Else
          If Len(s) > 0 Then lMassLink.Target.MemSub2 = myUci.CatAsInt(s)
        End If
        lMassLink.Comment = c
        myUci.MassLinks.Add lMassLink
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

Private Sub Class_Initialize()
  Set pSource = New HspfSrcTar
  Set pTarget = New HspfSrcTar
  pMFact = 1
End Sub
Public Property Get EditControlName() As String
  EditControlName = "ATCoHspf.ctlMassLinkEdit"
End Property

Public Sub Edit()
  editInit Me, Me.Uci.icon, True
End Sub
Public Property Get Caption() As String
  Caption = "Mass-Link Block"
End Property

Public Sub writeMassLinks(f As Integer, M As HspfMsg)
  Dim s$, e$, str$
  Dim lBlockDef As HspfBlockDef
  Dim vTableDef As Variant, lTableDef As HspfTableDef
  Dim firstTable As Boolean
  Dim i&, j&, k&, typeexists(4) As Boolean, icol&(15), ilen&(15)
  Dim lOper As HspfOperation
  Dim lML As HspfMassLink
  Dim lOpnSeqBlock As HspfOpnSeqBlk
  Dim lParmDef As Variant, t$
  Dim mlno&(), mlkeys&(), mlcnt&, found As Boolean

  
  mlcnt = 0
  For j = 1 To Uci.MassLinks.Count
    Set lML = Uci.MassLinks(j)
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
  Next j
  
  s = "MASS-LINK"
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
    
  For i = 1 To mlcnt
    Print #f, " "
    t = Space(5)
    RSet t = CStr(mlno(i - 1))
    Print #f, "  MASS-LINK    " & t
    'now start building the records
    Print #f, "<-Volume-> <-Grp> <-Member-><--Mult-->     <-Target vols> <-Grp> <-Member->  ***"
    Print #f, "<Name>            <Name> x x<-factor->     <Name>                <Name> x x  ***"
    For j = 1 To Uci.MassLinks.Count
      Set lML = Uci.MassLinks(j)
      If lML.MassLinkID = mlno(i - 1) Then
        str = Trim(lML.Source.VolName)
        str = str & Space(icol(1) - Len(str) - 1) 'pad prev field
        str = str & lML.Source.Group
        str = str & Space(icol(2) - Len(str) - 1)
        str = str & lML.Source.member
        str = str & Space(icol(3) - Len(str) - 1)
        If lML.Source.MemSub1 <> 0 Then
          t = Space(ilen(3))
          RSet t = CStr(lML.Source.MemSub1)
          If lML.Source.VolName = "RCHRES" Then t = pUci.IntAsCat(lML.Source.member, 1, t)
          str = str & t
        End If
        str = str & Space(icol(4) - Len(str) - 1)
        If lML.Source.MemSub2 <> 0 Then
          t = Space(ilen(4))
          RSet t = CStr(lML.Source.MemSub2)
          If lML.Source.VolName = "RCHRES" Then t = pUci.IntAsCat(lML.Source.member, 2, t)
          str = str & t
        End If
        str = str & Space(icol(5) - Len(str) - 1)
        If lML.MFact <> 1 Then
          t = Space(ilen(5))
          RSet t = CStr(lML.MFact)
          str = str & t
        End If
        str = str & Space(icol(6) - Len(str) - 1)
        'str = str & lML.Tran
        'str = str & Space(icol(7) - Len(str) - 1)
        str = str & lML.Target.VolName
        str = str & Space(icol(7) - Len(str) - 1)
        str = str & lML.Target.Group
        str = str & Space(icol(8) - Len(str) - 1)
        str = str & lML.Target.member
        str = str & Space(icol(9) - Len(str) - 1)
        If lML.Target.MemSub1 <> 0 Then
          t = Space(ilen(9))
          RSet t = CStr(lML.Target.MemSub1)
          If lML.Target.VolName = "RCHRES" Then t = pUci.IntAsCat(lML.Target.member, 1, t)
          str = str & t
        End If
        str = str & Space(icol(10) - Len(str) - 1)
        If lML.Target.MemSub2 <> 0 Then
          t = Space(ilen(10))
          RSet t = CStr(lML.Target.MemSub2)
          If lML.Target.VolName = "RCHRES" Then t = pUci.IntAsCat(lML.Target.member, 2, t)
          str = str & t
        End If
        If Len(lML.Comment) > 0 Then
          Print #f, lML.Comment
        End If
        Print #f, str
      End If
    Next j
    t = Space(5)
    RSet t = CStr(mlno(i - 1))
    Print #f, "  END MASS-LINK" & t
  Next i
  Print #f, "END " & s
End Sub

Public Function FindMassLinkID(sname$, tname$)
  Dim lML As HspfMassLink
  Dim j&
  
  FindMassLinkID = 0
  For j = 1 To pUci.MassLinks.Count
    Set lML = pUci.MassLinks(j)
    If lML.Source.VolName = sname And lML.Target.VolName = tname Then
      FindMassLinkID = lML.MassLinkID
    End If
  Next j
End Function

