VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "HspfOpnSeqBlk"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'Copyright 2002 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Dim pDelt As Long
Dim pOpns As Collection 'of HspfOperation
Dim pUci As HspfUci
Dim pComment As String

Public Property Get Uci() As HspfUci
  Set Uci = pUci
End Property
Public Property Set Uci(newUci As HspfUci)
  Dim vOpn As Variant
  
  Set pUci = newUci
  For Each vOpn In pOpns
    Set vOpn.Uci = pUci
  Next vOpn
End Property

Public Property Get Caption() As String
  Caption = "Opn Sequence Block"
End Property

Public Property Get Comment() As String
  Comment = pComment
End Property

Public Property Let Comment(newComment As String)
  pComment = newComment
End Property

Public Property Get EditControlName() As String
  EditControlName = "ATCoHspf.ctlOpnSeqBlkEdit"
End Property

Public Property Get Opns()
  Set Opns = pOpns
End Property

Public Property Get Opn(Index&) As HspfOperation
  If Index > 0 And Index <= pOpns.Count Then
    Set Opn = pOpns(Index)
  Else
    Set Opn = Nothing
  End If
End Property

Public Property Get Delt() As Long
  Delt = pDelt
End Property
Public Property Let Delt(newDelt As Long)
  pDelt = newDelt
End Property

Public Sub Add(newOpn As HspfOperation) 'to end
  pOpns.Add newOpn
End Sub

Public Sub Delete(Index As Long)
  pOpns.Remove Index
End Sub

Public Sub AddAfter(newOpn As HspfOperation, afterid As Long)
  pOpns.Add newOpn, after:=afterid
End Sub

Public Sub AddBefore(newOpn As HspfOperation, beforeid As Long)
  pOpns.Add newOpn, before:=beforeid
End Sub

Public Sub Edit()
  editInit Me, Me.Uci.icon, True, True
End Sub

Private Sub Class_Initialize()
  Set pOpns = New Collection
End Sub

Public Sub ReadUciFile()
  Dim OmCode&, init&, retkey&, cbuff$, retcod&, l&
  Dim lOpn As HspfOperation, rectyp&, c$
  
  init = 1
  OmCode = HspfOmCode("OPN SEQUENCE")
  retcod = 0
  ' first call gets delt
  If pUci.FastFlag Then
    retkey = -1
    GetCommentBeforeBlock "OPN SEQUENCE", pComment
    GetNextRecordFromBlock "OPN SEQUENCE", retkey, cbuff, rectyp, retcod
  Else
    Call REM_XBLOCK(Me.Uci, OmCode, init, retkey, cbuff, retcod)
  End If
  If retcod >= 0 Then
    pDelt = Mid(cbuff, 31, 2) * 60
    If Len(cbuff) > 33 Then
      pDelt = pDelt + Mid(cbuff, 34, 2)
    End If
    init = 0
    c = ""
    While retcod = 2
      If pUci.FastFlag Then
        GetNextRecordFromBlock "OPN SEQUENCE", retkey, cbuff, rectyp, retcod
      Else
        retkey = -1
        Call REM_XBLOCKEX(Me.Uci, OmCode, init, retkey, cbuff, rectyp, retcod)
      End If
      If InStr(cbuff, "INGRP") = 0 And retcod = 2 And rectyp = 0 Then
        Set lOpn = New HspfOperation
        lOpn.Name = Trim(StrRetRem(cbuff))
        If IsNumeric(cbuff) Then
          lOpn.Id = cbuff
        Else
          lOpn.Id = StrRetRem(cbuff)
        End If
        Set lOpn.Uci = pUci
        lOpn.Comment = c
        If lOpn.Name <> "UNKNOWN" Then
          pOpns.Add lOpn
        End If
        c = ""
      ElseIf retcod = 2 And rectyp = -1 Then
        'save comment
        If Len(c) = 0 Then
          c = cbuff
        Else
          c = c & vbCrLf & cbuff
        End If
      ElseIf retcod = 2 And rectyp = -2 Then
        'save blank line
        If Len(c) = 0 Then
          c = " "
        Else
          c = c & vbCrLf & " "
        End If
      End If
    Wend
  End If
End Sub

Public Sub WriteUciFile(f As Integer)
  Dim vOpn As Variant, lOpn As HspfOperation
  Dim h As Long, M As Long
  
  If Len(pComment) > 0 Then
    Print #f, pComment
  End If
  Print #f, " "
  Print #f, "OPN SEQUENCE"
  h = pDelt / 60
  M = pDelt - h * 60
  Print #f, "    INGRP" & Space(14) & "INDELT " & Format(h, "00") & ":" & Format(M, "00")
  For Each vOpn In pOpns
    Set lOpn = vOpn
    If Len(lOpn.Comment) > 0 Then
      Print #f, lOpn.Comment
    End If
    Print #f, Space(6) & lOpn.Name & Space(10 - Len(lOpn.Name)) & myFormatI(lOpn.Id, 4)
  Next vOpn
  Print #f, "    END INGRP"
  Print #f, "END OPN SEQUENCE"
End Sub


