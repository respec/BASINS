VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "HspfMetSeg"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'Copyright 2002 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Dim pMetSegRecs(7) As HspfMetSegRecord
Dim pId As Long
Dim pName As String
Dim pUci As HspfUci
Dim pAirType As Long '1-GATMP, 2-AIRTMP

Public Property Get MetSegRecs() As HspfMetSegRecord()
  MetSegRecs = pMetSegRecs
End Property
Public Property Get MetSegRec(msr As MetSegRecordType) As HspfMetSegRecord
Attribute MetSegRec.VB_MemberFlags = "200"
  Set MetSegRec = pMetSegRecs(msr)
End Property
Public Property Let MetSegRecs(newMetSegRecs() As HspfMetSegRecord)
  Dim itype&
  For itype = 1 To 7
    Set pMetSegRecs(itype) = newMetSegRecs(itype)
  Next
End Property
Public Property Get Id() As Long
  Id = pId
End Property
Public Property Let Id(newId As Long)
  pId = newId
End Property
Public Property Get AirType() As Long
  AirType = pAirType
End Property
Public Property Let AirType(newAirType As Long)
  pAirType = newAirType
End Property
Public Property Get Name() As String
  Name = pName
End Property
Public Property Let Name(newName As String)
  pName = newName
End Property
Public Function Add(newConn As HspfConnection) As Boolean
  Dim lMetSegRec As HspfMetSegRecord
  Dim MetSegRecs As Collection
  Dim itype&
  
  Set lMetSegRec = New HspfMetSegRecord
  If newConn.Target.VolName = "RCHRES" Then
    lMetSegRec.MFactR = newConn.MFact
  ElseIf newConn.Target.VolName = "PERLND" Or _
         newConn.Target.VolName = "IMPLND" Then
    lMetSegRec.MFactP = newConn.MFact
  End If
  Set lMetSegRec.Source = newConn.Source
  lMetSegRec.Tran = newConn.Tran
  lMetSegRec.Sgapstrg = newConn.Sgapstrg
  lMetSegRec.Ssystem = newConn.Ssystem
  lMetSegRec.Typ = Str2Type(newConn.Target.Member)
  
  Add = True
  
  If newConn.Target.VolName = "PERLND" Or _
     newConn.Target.VolName = "IMPLND" Or _
     newConn.Target.VolName = "RCHRES" Then
    itype = lMetSegRec.Typ
    If itype <> msrUNK Then
      If Len(pMetSegRecs(itype).Source.VolName) > 0 Then
        'dont add if already have this type of record
        Add = False
      Else
        Set pMetSegRecs(itype) = Nothing
        Set pMetSegRecs(itype) = lMetSegRec
        If newConn.Target.Member = "GATMP" Then
          pAirType = 1
        ElseIf newConn.Target.Member = "AIRTMP" Then
          pAirType = 2
        End If
      End If
    Else ' not needed
      Add = False
    End If
  Else
    Add = False
  End If
  
  If Not Add Then
    Set lMetSegRec = Nothing
  End If
  
End Function
Private Function Str2Type(str$) As MetSegRecordType
  Select Case str
    Case "PREC":   Str2Type = msrPREC
    Case "GATMP":  Str2Type = msrGATMP
    Case "AIRTMP": Str2Type = msrGATMP
    Case "DTMPG":  Str2Type = msrDTMPG
    Case "DEWTMP": Str2Type = msrDTMPG
    Case "WINMOV": Str2Type = msrWINMOV
    Case "WIND":   Str2Type = msrWINMOV
    Case "SOLRAD": Str2Type = msrSOLRAD
    Case "CLOUD":  Str2Type = msrCLOUD
    Case "PETINP": Str2Type = msrPETINP
    Case "POTEV":  Str2Type = msrPETINP
    Case Else:     Str2Type = msrUNK
  End Select
End Function

Public Function Compare(newMetSeg As HspfMetSeg, opname As String) As Boolean
  Compare = True 'assume the best
  Dim itype&
  Dim newMetSegRecs() As HspfMetSegRecord
  
  newMetSegRecs = newMetSeg.MetSegRecs
  For itype = 1 To 7
    If Not (pMetSegRecs(itype).Compare(newMetSegRecs(itype), opname)) Then
      Compare = False
    End If
  Next itype
End Function

Public Sub UpdateMetSeg(newMetSeg As HspfMetSeg)
  Dim itype&
  Dim newMetSegRecs() As HspfMetSegRecord
  
  newMetSegRecs = newMetSeg.MetSegRecs
  For itype = 1 To 7
    If pMetSegRecs(itype).MFactR = -999# And newMetSegRecs(itype).MFactR <> -999# Then
      pMetSegRecs(itype).MFactR = newMetSegRecs(itype).MFactR
      pMetSegRecs(itype).Sgapstrg = newMetSegRecs(itype).Sgapstrg
      Set pMetSegRecs(itype).Source = newMetSegRecs(itype).Source
      pMetSegRecs(itype).Ssystem = newMetSegRecs(itype).Ssystem
      pMetSegRecs(itype).Tran = newMetSegRecs(itype).Tran
      pMetSegRecs(itype).Typ = newMetSegRecs(itype).Typ
    End If
  Next itype
End Sub

Private Sub Class_Initialize()
  Dim itype&
  
  For itype = 1 To 7
    Set pMetSegRecs(itype) = New HspfMetSegRecord
  Next itype
  pAirType = 0
End Sub

Public Sub ExpandMetSegName(wdmid$, idsn&)
  Dim itype&, addstr$, Con$
  Dim newMetSegRecs() As HspfMetSegRecord
  
  Me.Name = Me.Uci.GetWDMAttr(wdmid, idsn, "LOC")
  
  newMetSegRecs = Me.MetSegRecs
  For itype = 1 To 7
    Select Case itype
      Case 1: Con = "PREC"
      Case 2: Con = "GATMP"
      Case 3: Con = "DTMPG"
      Case 4: Con = "WINMOV"
      Case 5: Con = "SOLRAD"
      Case 6: Con = "CLOUD"
      Case 7: Con = "PETINP"
    End Select
    If itype = 2 And Me.AirType = 2 Then
      Con = "AIRTMP"
    End If
    If pMetSegRecs(itype).MFactP <> 1 And pMetSegRecs(itype).MFactP <> 0 And pMetSegRecs(itype).MFactP <> -999 Then
      addstr = addstr & ",PI:" & Con & "=" & CStr(pMetSegRecs(itype).MFactP)
    End If
    If pMetSegRecs(itype).MFactR <> 1 And pMetSegRecs(itype).MFactR <> 0 And pMetSegRecs(itype).MFactR <> -999 Then
      addstr = addstr & ",R:" & Con & "=" & CStr(pMetSegRecs(itype).MFactR)
    End If
  Next itype
  
  If Len(addstr) > 0 Then
    Me.Name = Me.Name & addstr
  End If
  
End Sub

Public Property Set Uci(newUci As HspfUci)
  Set pUci = newUci
End Property
Public Property Get Uci()
  Set Uci = pUci
End Property

Public Sub WriteUciFile(optyp$, icol&(), ilen&(), f)
  Dim lOpn As HspfOperation
  Dim j&, i&, str$, t$
  Dim segRec As Long, tmember$
  Dim firstid&, lastid&
 
  firstid = 0
  lastid = 0
  For j = 1 To pUci.OpnBlks(optyp).Ids.Count
    Set lOpn = pUci.OpnBlks(optyp).NthOper(j)
    If lOpn.MetSeg.Id = Me.Id Then
      If firstid = 0 Then
        firstid = lOpn.Id
      Else
        lastid = lOpn.Id
      End If
    ElseIf firstid > 0 Then
      WriteRecs optyp, firstid, lastid, icol, ilen, f
      firstid = 0
      lastid = 0
    End If
  Next j
  If firstid > 0 Then WriteRecs optyp, firstid, lastid, icol, ilen, f
  
End Sub

Public Sub WriteRecs(optyp$, firstid&, lastid&, icol&(), ilen&(), f)
  Dim j&, i&, str$, t$
  Dim segRec As Long, tmember$
 
  For segRec = 1 To 7
    With pMetSegRecs(segRec)
      If .Typ <> 0 Then 'type exists
        If (optyp = "RCHRES" And .MFactR > 0#) Or _
           (optyp = "PERLND" And .MFactP > 0#) Or _
           (optyp = "IMPLND" And .MFactP > 0#) Then
          'have this type of met seg record
          str = Trim(.Source.VolName)
          str = str & Space(icol(1) - Len(str) - 1) 'pad prev field
          t = Space(ilen(1)) 'right justify numbers
          RSet t = CStr(.Source.VolId)
          str = str & t
          str = str & Space(icol(2) - Len(str) - 1)
          str = str & .Source.Member
          str = str & Space(icol(3) - Len(str) - 1)
          If .Source.MemSub1 <> 0 Then
            t = Space(ilen(3))
            RSet t = CStr(.Source.MemSub1)
            str = str & t
          End If
          str = str & Space(icol(4) - Len(str) - 1)
          str = str & .Ssystem
          str = str & Space(icol(5) - Len(str) - 1)
          str = str & .Sgapstrg
          str = str & Space(icol(6) - Len(str) - 1)
          If optyp = "RCHRES" Then
            If .MFactR <> 1 Then
              t = Space(ilen(6))
              RSet t = CStr(.MFactR)
              str = str & t
            End If
          Else
            If .MFactP <> 1 Then
              t = Space(ilen(6))
              RSet t = CStr(.MFactP)
              str = str & t
            End If
          End If
          str = str & Space(icol(7) - Len(str) - 1)
          str = str & .Tran
          str = str & Space(icol(8) - Len(str) - 1)
          str = str & optyp
          str = str & Space(icol(9) - Len(str) - 1)
          t = Space(ilen(9))
          RSet t = CStr(firstid)
          str = str & t
          If lastid > 0 Then
            str = str & Space(icol(10) - Len(str) - 1)
            t = Space(ilen(9))
            RSet t = CStr(lastid)
            str = str & t
          End If
          str = str & Space(icol(11) - Len(str) - 1)
          If optyp <> "RCHRES" And pAirType = 2 And .Typ = 2 Then
            str = str & "ATEMP"
          Else
            str = str & "EXTNL"
          End If
          str = str & Space(icol(12) - Len(str) - 1)
          If optyp = "RCHRES" Then
            Select Case .Typ
              Case 1:   tmember = "PREC"
              Case 2:   tmember = "GATMP"
              Case 3:   tmember = "DEWTMP"
              Case 4:   tmember = "WIND"
              Case 5:   tmember = "SOLRAD"
              Case 6:   tmember = "CLOUD"
              Case 7:   tmember = "POTEV"
            End Select
          Else
            Select Case .Typ
              Case 1:   tmember = "PREC"
              Case 2:   tmember = "GATMP"
              Case 3:   tmember = "DTMPG"
              Case 4:   tmember = "WINMOV"
              Case 5:   tmember = "SOLRAD"
              Case 6:   tmember = "CLOUD"
              Case 7:   tmember = "PETINP"
            End Select
            If .Typ = 2 Then
              'get right air temp member name
              If pAirType = 1 Then
                tmember = "GATMP"
              ElseIf pAirType = 2 Then
                tmember = "AIRTMP"
              End If
            End If
          End If
          str = str & tmember
          str = str & Space(icol(13) - Len(str) - 1)
          If segRec = 1 Then
            Print #f, "*** Met Seg " & pName
          End If
          Print #f, str
        End If
      End If
    End With
  Next segRec
  
End Sub

