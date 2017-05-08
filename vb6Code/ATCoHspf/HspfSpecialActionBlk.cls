VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "HspfSpecialActionBlk"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'Copyright 2002 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Dim pActions As Collection 'of HspfSpecialAction
Dim pDistributes As Collection 'of HspfSpecialDistribute
Dim pUserDefineNames As Collection 'of HspfSpecialUserDefineName
Dim pUserDefineQuans As Collection 'of HspfSpecialUserDefineQuans
Dim pConditions As Collection 'of HspfSpecialCondition
Dim pRecords As Collection 'of HspfSpecialRecord
Dim pComment As String
Dim pUci As HspfUci

Friend Property Get Uci() As HspfUci
  Set Uci = pUci
End Property
Friend Property Set Uci(newUci As HspfUci)
  Set pUci = newUci
End Property

Public Property Get Caption() As String
  Caption = "Special Actions Block"
End Property

Public Property Get Comment() As String
  Comment = pComment
End Property

Public Property Let Comment(newComment As String)
  pComment = newComment
End Property

Public Property Get EditControlName() As String
  EditControlName = "ATCoHspf.ctlSpecialActionEdit"
End Property

Property Get Actions() As Collection
  Set Actions = pActions
End Property

Property Get Distributes() As Collection
  Set Distributes = pDistributes
End Property

Property Get UserDefineNames() As Collection
  Set UserDefineNames = pUserDefineNames
End Property

Property Get UserDefineQuans() As Collection
  Set UserDefineQuans = pUserDefineQuans
End Property

Property Get Conditions() As Collection
  Set Conditions = pConditions
End Property

Property Get Records() As Collection
  Set Records = pRecords
End Property

Public Sub Edit()
  editInit Me, Me.Uci.icon, True
End Sub

Public Sub ReadUciFile()
  Dim done As Boolean, init As Long, OmCode As Long
  Dim retkey As Long, cbuff As String, retcod As Long
  Dim i&, val As Single, rectyp&
  Dim mySpecialRecord As HspfSpecialRecord
  Dim moreUvnames As Long
  
  If pUci.FastFlag Then
    GetCommentBeforeBlock "SPEC-ACTIONS", pComment
  End If
  
  moreUvnames = 0
  OmCode = HspfOmCode("SPEC-ACTIONS")
  init = 1
  done = False
  retkey = -1
  Do Until done
    If pUci.FastFlag Then
      GetNextRecordFromBlock "SPEC-ACTIONS", retkey, cbuff, rectyp, retcod
    Else
      retkey = -2 'force return of comments/blanks
      Call REM_XBLOCKEX(Me.Uci, OmCode, init, retkey, cbuff, rectyp, retcod)
    End If
    init = 0
    If retcod = 2 Then 'normal record
      Set mySpecialRecord = New HspfSpecialRecord
      With mySpecialRecord
        .Text = cbuff
        If Len(cbuff) = 0 Or InStr(cbuff, "***") > 0 Then
          .SpecType = hComment
        ElseIf Left(Trim(cbuff), 3) = "IF " Or _
               Left(Trim(cbuff), 4) = "ELSE" Or _
               Left(Trim(cbuff), 6) = "END IF" Then
          .SpecType = hCondition
        ElseIf Mid(cbuff, 3, 6) = "DISTRB" Then
          .SpecType = hDistribute
        ElseIf Mid(cbuff, 3, 6) = "UVNAME" Then
          .SpecType = hUserDefineName
          'look at how many uvnames to come
          moreUvnames = CInt(Mid(cbuff, 17, 3))
          moreUvnames = Int((moreUvnames - 1) / 2) 'lines to come
        ElseIf Mid(cbuff, 3, 6) = "UVQUAN" Then
          .SpecType = hUserDefineQuan
        Else
          If moreUvnames > 0 Then
            .SpecType = hUserDefineName
            If Left(.Text, 5) <> "     " Then  'see if record needs padding
              .Text = "                  " & .Text
            End If
            moreUvnames = moreUvnames - 1
          Else
            .SpecType = hAction
          End If
        End If
      End With
      pRecords.Add mySpecialRecord
    Else
      done = True
    End If
  Loop
End Sub

Public Sub WriteUciFile(f As Long)
  Dim i&
  
  If pRecords.Count > 0 Then
    If Len(pComment) > 0 Then
      Print #f, pComment
    End If
    Print #f, " "
    Print #f, "SPEC-ACTIONS"
    With pRecords
      For i = 1 To .Count
        Print #f, .Item(i).Text
      Next i
    End With
    Print #f, "END SPEC-ACTIONS"
  End If
End Sub

Private Sub Class_Initialize()
  Set pRecords = New Collection
  Set pActions = New Collection
  Set pDistributes = New Collection
  Set pUserDefineNames = New Collection
  Set pUserDefineQuans = New Collection
  Set pConditions = New Collection
End Sub
