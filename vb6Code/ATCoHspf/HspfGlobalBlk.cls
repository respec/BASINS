VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "HspfGlobalBlk"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'Copyright 2002 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Dim pRunInf As HSPFParm  ' run information
Dim pSDate(5) As Long   'starting date
Dim pEDate(5) As Long   'ending date
Dim pOutLev As HSPFParm  'run interp output level
Dim pSpOut As Long      'special action output level
Dim pRunFg As Long      'interp only(0) or interp and run(1)
Dim pEmFg As Long       'english(1), metric(2) flag
Dim pIhmFg As Long      'ihm flag (normal-0,IHM control-1)
Dim pComment As String
Dim pUci As HspfUci
Dim pEdited As Boolean

Public Property Get Edited() As Boolean
  Edited = pEdited
End Property
Public Property Let Edited(newEdited As Boolean)
  pEdited = newEdited
  If newEdited Then pUci.Edited = True
End Property

Public Property Get Uci() As HspfUci
  Set Uci = pUci
End Property
Public Property Set Uci(newUci As HspfUci)
  Set pUci = newUci
End Property

Public Property Get Caption() As String
  Caption = "Global Block"
End Property

Public Property Get Comment() As String
  Comment = pComment
End Property

Public Property Let Comment(newComment As String)
  pComment = newComment
End Property

Public Property Get EditControlName() As String
  EditControlName = "ATCoHspf.ctlGlobalBlkEdit"
End Property

Public Property Get RunInf() As HSPFParm
  Set RunInf = pRunInf
End Property
Public Property Let RunInf(newRunInf As HSPFParm)
  Set pRunInf = newRunInf
  Update
End Property

Public Property Get SDate(Index&) As Long
  SDate = pSDate(Index)
End Property
Public Property Let SDate(Index&, newSDate As Long)
  pSDate(Index) = newSDate
  Update
End Property

Public Property Get EDate(Index&) As Long
  EDate = pEDate(Index)
End Property
Public Property Let EDate(Index&, newEDate As Long)
  pEDate(Index) = newEDate
  Update
End Property

Public Property Get outlev() As HSPFParm
  Set outlev = pOutLev
End Property
Public Property Let outlev(newOutLev As HSPFParm)
  Set pOutLev = newOutLev
  Update
End Property

Public Property Get spout() As Long
  spout = pSpOut
End Property
Public Property Let spout(newSpOut As Long)
  pSpOut = newSpOut
  Update
End Property

Public Property Get runfg() As Long
  runfg = pRunFg
End Property
Public Property Let runfg(newRunFg As Long)
  pRunFg = newRunFg
  Update
End Property

Public Property Get emfg() As Long
  emfg = pEmFg
End Property
Public Property Let emfg(newEmFg As Long)
  pEmFg = newEmFg
  Update
End Property

Public Sub Edit()
  editInit Me, Me.Uci.icon
End Sub

Public Sub ReadUciFile()
  Dim lOutLev&, lRunInf$, retkey&, cbuff$, rectyp&, retcod&
  
  If pUci.FastFlag Then
    retkey = -1
    GetCommentBeforeBlock "GLOBAL", pComment
    GetNextRecordFromBlock "GLOBAL", retkey, cbuff, rectyp, retcod
    If Mid(cbuff, 1, 7) <> "START" Then
      lRunInf = Trim(cbuff)
      GetNextRecordFromBlock "GLOBAL", retkey, cbuff, rectyp, retcod
    Else
      lRunInf = ""
    End If
    'Allow room for comments
    While rectyp < 0 And retkey < 50 '(50 is arbitrary to prevent an endless loop)
      GetNextRecordFromBlock "GLOBAL", retkey, cbuff, rectyp, retcod
    Wend

    pSDate(0) = Mid(cbuff, 15, 4)
    If Len(Trim(Mid(cbuff, 20, 2))) > 0 Then
      pSDate(1) = Mid(cbuff, 20, 2)
    Else
      pSDate(1) = 1
    End If
    If Len(Trim(Mid(cbuff, 23, 2))) > 0 Then
      pSDate(2) = Mid(cbuff, 23, 2)
    Else
      pSDate(2) = 1
    End If
    If Len(Trim(Mid(cbuff, 26, 2))) > 0 Then
      pSDate(3) = Mid(cbuff, 26, 2)
      pSDate(4) = Mid(cbuff, 29, 2)
    Else
      pSDate(3) = 0
      pSDate(4) = 0
    End If
    pEDate(0) = Mid(cbuff, 40, 4)
    If Len(Trim(Mid(cbuff, 45, 2))) > 0 Then
      pEDate(1) = Mid(cbuff, 45, 2)
    Else
      pEDate(1) = 12
    End If
    If Len(Trim(Mid(cbuff, 48, 2))) > 0 Then
      pEDate(2) = Mid(cbuff, 48, 2)
    Else
      pEDate(2) = 31
    End If
    If Len(Trim(Mid(cbuff, 51, 2))) > 0 Then
      pEDate(3) = Mid(cbuff, 51, 2)
      pEDate(4) = Mid(cbuff, 54, 2)
    Else
      pEDate(3) = 24
      pEDate(4) = 0
    End If
    GetNextRecordFromBlock "GLOBAL", retkey, cbuff, rectyp, retcod
    lOutLev = Mid(cbuff, 26, 5)
    If Len(Trim(Mid(cbuff, 31, 5))) > 0 Then
      pSpOut = Mid(cbuff, 31, 5)
    Else
      pSpOut = 2
    End If
    GetNextRecordFromBlock "GLOBAL", retkey, cbuff, rectyp, retcod
    pRunFg = Mid(cbuff, 20, 5)
    If Len(Trim(Mid(cbuff, 58, 5))) > 0 Then
      pEmFg = Mid(cbuff, 58, 5)
    Else
      pEmFg = 1
    End If
    If Len(Trim(Mid(cbuff, 68, 5))) > 0 Then
      pIhmFg = Mid(cbuff, 68, 5)
    Else
      pIhmFg = 0
    End If
  Else
    Call REM_GLOBLK(Me.Uci, pSDate, pEDate, lOutLev, pSpOut, pRunFg, pEmFg, lRunInf)
    Call REM_GLOPRMI(Me.Uci, pIhmFg, "IHMFG")
  End If
  
  If pSDate(1) = 0 Then pSDate(1) = 1
  If pSDate(2) = 0 Then pSDate(2) = 1
  If pEDate(1) = 0 Then pEDate(1) = 12
  If pEDate(2) = 0 Then pEDate(2) = 31
  
  pOutLev.Value = lOutLev
  pRunInf.Value = lRunInf
End Sub

Public Sub WriteUciFile(f As Integer)
  Dim s$, e$
  
  If Len(pComment) > 0 Then
    Print #f, pComment
  End If
  Print #f, " "
  Print #f, "GLOBAL"
  Print #f, "  " & Trim(pRunInf)
  s = "  START       " & Format(SDate(0), "0000") & "/" & Format(SDate(1), "00") & "/" & Format(SDate(2), "00") & _
                   " " & Format(SDate(3), "00") & ":" & Format(SDate(4), "00")
  e = "  END    " & Format(EDate(0), "0000") & "/" & Format(EDate(1), "00") & "/" & Format(EDate(2), "00") & _
                   " " & Format(EDate(3), "00") & ":" & Format(EDate(4), "00")
  Print #f, s & e
  Print #f, "  RUN INTERP OUTPT LEVELS" & myFormatI((pOutLev), 5) & myFormatI((pSpOut), 5)
  s = "  RESUME     0 RUN " & myFormatI((pRunFg), 5) & Space(26) & "UNITS" & myFormatI((pEmFg), 5)
  If pIhmFg <> 0 And pIhmFg <> -999 Then
    s = s & "     " & myFormatI((pIhmFg), 5)
  End If
  Print #f, s
  Print #f, "END GLOBAL"
End Sub

Private Sub Update()
  'Call F90_PUTGLO(pSDate(0), pEDate(0), pOutLev, pSpOut, pRunFg, pEmFg, pRunInf, Len(pRunInf))
  pUci.Edited = True
End Sub

Public Function Check() As String
  'verify values are correct in relation to each other and other tables
  
End Function

Private Sub Class_Initialize()
  Set pOutLev = New HSPFParm
  Set pOutLev.Parent = Me
  Set pOutLev.Def = readParmDef("OutLev")
  Set pRunInf = New HSPFParm
  Set pRunInf.Parent = Me
  Set pRunInf.Def = readParmDef("RunInf")
End Sub
