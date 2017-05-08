VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "HspfFilesBlk"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'Copyright 2002 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Dim pFiles As Collection 'of HspfFile
Dim pUci As HspfUci
Dim pComment As String

Friend Property Get Uci() As HspfUci
  Set Uci = pUci
End Property
Friend Property Set Uci(newUci As HspfUci)
  Set pUci = newUci
End Property

Public Property Get Caption() As String
  Caption = "Files Block"
End Property

Public Property Get Comment() As String
  Comment = pComment
End Property

Public Property Let Comment(newComment As String)
  pComment = newComment
End Property

Public Property Get EditControlName() As String
  EditControlName = "ATCoHspf.ctlFilesBlkEdit"
End Property

Public Sub Clear()
  Set pFiles = Nothing
  Set pFiles = New Collection
End Sub

Public Property Get Count() As Long
  Count = pFiles.Count
End Property

Public Sub Add(newValue As HspfFile) 'to end, how about in between
  pFiles.Add newValue
End Sub

Public Sub AddFromSpecs(newName$, wid$)
  Dim iunit&, vfile As Variant, ifound As Boolean
  Dim newFile As HspfFile
  newFile.Name = newName
  newFile.Typ = wid
  'find available unit
  iunit = 25
  ifound = True
  Do Until ifound = False
    iunit = iunit + 1
    ifound = False
    For Each vfile In pFiles
      If iunit = vfile.Unit Then
        ifound = True
      End If
    Next vfile
  Loop
  newFile.Unit = iunit
  pFiles.Add newFile
End Sub

Public Sub AddFromSpecsExt(newName$, wid$, Unit&)
  Dim iunit&, vfile As Variant, ifound As Boolean
  Dim newFile As HspfFile
  newFile.Name = newName
  newFile.Typ = wid
  'find available unit
  iunit = Unit - 1
  ifound = True
  Do Until ifound = False
    iunit = iunit + 1
    ifound = False
    For Each vfile In pFiles
      If iunit = vfile.Unit Then
        ifound = True
      End If
    Next vfile
  Loop
  newFile.Unit = iunit
  pFiles.Add newFile
End Sub

Public Sub Remove(Index&)
  If Index > 0 And Index <= pFiles.Count Then
    pFiles.Remove (Index)
  End If
End Sub

Public Sub SetTyp(Index&, wid$)
  Dim lFile As HspfFile
  If Index > 0 And Index <= pFiles.Count Then
    lFile.Typ = wid
    lFile.Comment = pFiles(Index).Comment
    lFile.Name = pFiles(Index).Name
    lFile.Unit = pFiles(Index).Unit
    pFiles.Remove Index
    pFiles.Add lFile
  End If
End Sub

Public Property Get Value(Index&) As HspfFile
  If Index > 0 And Index <= pFiles.Count Then
    Value = pFiles(Index)
  Else
    Value.Name = ""
    Value.Typ = ""
    Value.Unit = 0
  End If
End Property
Public Property Let Value(Index&, newValue As HspfFile) '????
  If Index <= pFiles.Count Then
    pFiles.Remove Index
    pFiles.Add newValue, , Index
    'Set pFiles(Index) = newValue
  ElseIf Index = pFiles.Count + 1 Then
    pFiles.Add newValue
  Else 'error?
  End If
End Property

Public Sub Edit()
  editInit Me, Me.Uci.icon, True
End Sub

Private Sub Class_Initialize()
  Set pFiles = New Collection
End Sub

Private Sub Update()
  pUci.Edited = True
End Sub

Public Function Check() As String
  'verify values are correct in relation to each other and other tables
  
End Function

Friend Sub ReadUciFile()
  Dim lFile As HspfFile, c$
  Dim retcod&, init&, OmCode&, cbuff$, retkey&, rectyp&
  
  On Error GoTo ErrHand
  
  If pUci.FastFlag Then
    GetCommentBeforeBlock "FILES", pComment
  End If
  
  retcod = 0
  init = 1
  OmCode = HspfOmCode("FILES")
  c = ""
  retkey = -1
  Do
    If pUci.FastFlag Then
      GetNextRecordFromBlock "FILES", retkey, cbuff, rectyp, retcod
    Else
      retkey = -1
      Call REM_XBLOCKEX(Me.Uci, OmCode, init, retkey, cbuff, rectyp, retcod)
    End If
    If retcod = 10 Then Exit Do
    If rectyp = 0 Then
      If Len(Trim(Left(cbuff, 6))) > 0 Then
        lFile.Typ = StrRetRem(cbuff)
      Else
        lFile.Typ = ""
      End If
      lFile.Unit = StrRetRem(cbuff)
      lFile.Name = cbuff
      lFile.Comment = c
      pFiles.Add lFile
      c = ""
    ElseIf rectyp = -1 Then
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
    init = 0
  Loop

  Exit Sub

ErrHand:
  MsgBox err.Description & vbCr & vbCr & cbuff, vbCritical, "Error in ReadUciFile"

End Sub
 
Friend Sub WriteUciFile(f As Integer)
  Dim vfile As Variant, lFile As HspfFile, tname As String
  Dim tpath As String
  
  If Len(pComment) > 0 Then
    Print #f, pComment
  End If
  Print #f, " "
  Print #f, "FILES"
  If pFiles.Count > 0 Then
    If Len(pFiles(1).Comment) = 0 Then
      'need to add header
      Print #f, "<FILE>  <UN#>***<----FILE NAME------------------------------------------------->"
    End If
  End If
  For Each vfile In pFiles
    lFile = vfile
    tname = lFile.Name
    If InStr(1, tname, ":") Then
      'this is the absolute path name, make relative
      'tpath = CurDir
      tpath = PathNameOnly(Me.Uci.Name)
      tname = RelativeFilename(tname, tpath)
      lFile.Name = tname
    End If
    If Len(lFile.Comment) > 0 Then
      Print #f, lFile.Comment
    End If
    Print #f, lFile.Typ & Space(10 - Len(lFile.Typ)) & myFormatI(lFile.Unit, 3), Space(2) & tname
  Next vfile
  Print #f, "END FILES"
End Sub

Public Sub newName(oldn$, newn$)
  Dim i&, j&, k&, tempn$, islash&, l&, itmp&
  Dim lHspfFile As HspfFile, lFiles As Collection, vHspfFile As Variant
  
  Set lFiles = New Collection
  For Each vHspfFile In pFiles
    lHspfFile = vHspfFile
    If Trim(lHspfFile.Typ) = "MESSU" Or Trim(lHspfFile.Typ) = "" Or Trim(lHspfFile.Typ) = "BINO" Then
      'Close lFile.Unit
      'replace file name
      tempn = lHspfFile.Name
      l = Len(oldn)
      itmp = InStr(1, UCase(tempn), UCase(oldn))
      j = Len(tempn)
      islash = 0
      For i = 1 To j
        'check for a path in the name
        If (Mid(tempn, i, 1) = "\") Then
          islash = i
        End If
      Next i
      If ((itmp > 0 And islash > 0 And itmp > islash) Or _
        (itmp > 0 And islash = 0)) Then
        'found the old name in this string, replace it
        j = Len(newn)
        lHspfFile.Name = Mid(tempn, 1, itmp - 1) & newn & Mid(tempn, itmp + l)
      Else
        'just add the new scen name
        k = Len(newn)
        If islash = 0 Then
          'no path
          lHspfFile.Name = newn & "." & tempn
        Else
          'have a path name, insert after slash
          lHspfFile.Name = Mid(tempn, 1, islash) & newn & _
             "." & Mid(tempn, islash + 1, j)
        End If
      End If
    End If
    lFiles.Add lHspfFile
  Next vHspfFile
  
  Set pFiles = Nothing
  Set pFiles = New Collection
  Set pFiles = lFiles
End Sub
