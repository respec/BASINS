VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "HspfMonthData"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'Copyright 2002 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Dim pMonthDataTables As Collection 'of HspfMonthDataTable
Dim pUci As HspfUci
Dim pComment As String

Property Get Caption() As String
  Caption = "Month Data"
End Property

Public Property Get Comment() As String
  Comment = pComment
End Property

Public Property Let Comment(newComment As String)
  pComment = newComment
End Property

Property Get Uci() As HspfUci
  Set Uci = pUci
End Property
Property Set Uci(newUci As HspfUci)
  Set pUci = newUci
End Property

Public Property Get MonthDataTables() As Collection
  Set MonthDataTables = pMonthDataTables
End Property

Public Sub Edit()
  editInit Me, Me.Uci.icon, True
End Sub

Private Sub Class_Initialize()
  Set pMonthDataTables = Nothing
  Set pMonthDataTables = New Collection
End Sub

Public Property Get EditControlName() As String
  EditControlName = "ATCoHspf.ctlMonthDataEdit"
End Property

Public Sub ReadUciFile()
  Dim done As Boolean, init As Long, OmCode As Long
  Dim retkey As Long, cbuff As String, retcod As Long
  Dim i&, val As Single, rectyp&
  Dim myMonthDataTable As HspfMonthDataTable
  
  If pUci.FastFlag Then
    GetCommentBeforeBlock "MONTH-DATA", pComment
  End If
  
  OmCode = HspfOmCode("MONTH-DATA")
  init = 1
  done = False
  retkey = -1
  Do Until done
    If pUci.FastFlag Then
      GetNextRecordFromBlock "MONTH-DATA", retkey, cbuff, rectyp, retcod
    Else
      Call REM_XBLOCK(Me.Uci, OmCode, init, retkey, cbuff, retcod)
    End If
    init = 0
    If InStr(cbuff, "END") Then 'skip this
    ElseIf InStr(cbuff, "MONTH-DATA") > 0 Then 'another one
      Set myMonthDataTable = New HspfMonthDataTable
      myMonthDataTable.Id = Right(cbuff, 3)
      Set myMonthDataTable.Block = Me
      If pUci.FastFlag Then
        GetNextRecordFromBlock "MONTH-DATA", retkey, cbuff, rectyp, retcod
      Else
        Call REM_XBLOCK(Me.Uci, OmCode, init, retkey, cbuff, retcod)
      End If
      If rectyp = -1 Then
        'this is a comment
      Else
        'this is a regular record
        For i = 1 To 12
          val = Mid(cbuff, 1 + (i - 1) * 6, 6)
          myMonthDataTable.MonthValue(i) = val
        Next i
        pMonthDataTables.Add myMonthDataTable
      End If
    End If
    If retcod <> 2 Then
      done = True
    End If
  Loop
End Sub

Public Sub WriteUciFile(f As Long)
  Dim i&, j&, s$, t$, lMonthDataTable As HspfMonthDataTable
  
  With pMonthDataTables
    If .Count > 0 Then 'something to write
      If Len(pComment) > 0 Then
        Print #f, pComment
      End If
      Print #f, " "
      Print #f, "MONTH-DATA"
      Print #f, " "
      For i = 1 To .Count
        Set lMonthDataTable = .Item(i)
        Print #f, "  MONTH-DATA     " & myFormatI(lMonthDataTable.Id, 3)
        s = ""
        For j = 1 To 12
          t = Space(6)
          RSet t = CStr(lMonthDataTable.MonthValue(j))
          s = s & t
        Next j
        Print #f, s
        Print #f, "  END MONTH-DATA " & myFormatI(lMonthDataTable.Id, 3)
        Print #f, " "
      Next i
      Print #f, "END MONTH-DATA"
    End If
  End With
End Sub
