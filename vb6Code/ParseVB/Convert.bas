Attribute VB_Name = "modConvert"
Option Explicit

Dim ProjectFile As Integer
Dim Visio As VisioObjects

Private Type PropertyStruct
  ThisName As String
  ReturnType As String
  Readable As Boolean
  Writable As Boolean
End Type
Private Properties() As PropertyStruct

Public Sub Main()
  Dim fn$, BaseName$, Path$, SourceExtension$
  'BaseName = GetSetting(App.Title, "Defaults", "BaseName", "")
  'Path = GetSetting(App.Title, "Defaults", "Path", CurDir)
  'frmMain.cdlg.filename = Path & "\" & BaseName & SourceExtension
  frmMain.cdlg.ShowOpen
  fn = frmMain.cdlg.filename
  If Len(Dir(fn)) > 0 Then
    BaseName = Mid(fn, InStrRev(fn, "\") + 1)
    Path = PathNameOnly(fn)
    Convert fn
  End If
  If Len(BaseName) > 0 Then SaveSetting App.Title, "Defaults", "BaseName", BaseName
  If Len(Path) > 0 Then SaveSetting App.Title, "Defaults", "Path", Path

End Sub

Public Sub Convert(filename$)
  Dim buf$
  Dim Prop As Long
  Set Visio = New VisioObjects
  ReDim Properties(0)
  ProjectFile = FreeFile(0)
  'Open Path & "\" & BaseName For Input As ProjectFile
  Open filename For Input As ProjectFile
  While Not EOF(ProjectFile)  ' Loop until end of file.
    Line Input #ProjectFile, buf
    If Left(buf, 20) = "Attribute VB_Name = " Then
      Visio.addClass Mid(buf, 22, Len(buf) - 1), True
    ElseIf Left(buf, 7) = "Public " Then
      buf = Mid(buf, 8)
      Debug.Print buf
      If Left(buf, 4) = "Sub " Then
        DoSubroutine Mid(buf, 5)
      ElseIf Left(buf, 13) = "Property Set " Then
        DoPropertySet Mid(buf, 14)
      ElseIf Left(buf, 13) = "Property Let " Then
        DoPropertySet Mid(buf, 14)
      ElseIf Left(buf, 13) = "Property Get " Then
        DoPropertyGet Mid(buf, 14)
      ElseIf Left(buf, 9) = "Function " Then
        DoFunction Mid(buf, 10)
      ElseIf Left(buf, 6) = "Event " Then
        DoEvent Mid(buf, 7)
      End If
    End If
  Wend
  For Prop = 1 To UBound(Properties)
    With Properties(Prop)
      buf = ""
      If .Readable Then buf = "R"
      If .Writable Then buf = buf & "W"
      Visio.addProperty .ThisName & " " & .ReturnType, buf
    End With
  Next Prop
  Close ProjectFile
End Sub

Private Sub DoPropertySet(buf$)
  Dim found As Boolean, Prop As Long
  Dim ThisName$, Args$, ReturnType$
  FindNameArgsAndType buf, ThisName, Args, ReturnType
  found = False
  For Prop = 1 To UBound(Properties)
    If Properties(Prop).ThisName = ThisName Then
      Properties(Prop).Writable = True
      If Len(ReturnType) > Len(Properties(Prop).ReturnType) Then Properties(Prop).ReturnType = ReturnType
      found = True
    End If
  Next Prop
  If Not found Then
    ReDim Preserve Properties(UBound(Properties) + 1)
    With Properties(UBound(Properties))
      .ThisName = ThisName
      .Writable = True
      .Readable = False
      .ReturnType = ReturnType
    End With
  End If
End Sub

Private Sub DoPropertyGet(buf$)
  Dim found As Boolean, Prop As Long
  Dim ThisName$, Args$, ReturnType$
  FindNameArgsAndType buf, ThisName, Args, ReturnType
  found = False
  For Prop = 1 To UBound(Properties)
    If Properties(Prop).ThisName = ThisName Then
      Properties(Prop).Readable = True
      If Len(ReturnType) > Len(Properties(Prop).ReturnType) Then Properties(Prop).ReturnType = ReturnType
      found = True
    End If
  Next Prop
  If Not found Then
    ReDim Preserve Properties(UBound(Properties) + 1)
    With Properties(UBound(Properties))
      .ThisName = ThisName
      .Readable = True
      .Writable = False
      .ReturnType = ReturnType
    End With
  End If
End Sub

Private Sub DoSubroutine(buf$)
  DoFunction buf
End Sub

Private Sub DoFunction(buf$)
  Dim ThisName$, Args$, ReturnType$
  FindNameArgsAndType buf, ThisName, Args, ReturnType
  Visio.addMethod ThisName, Args, ReturnType
End Sub

Private Sub DoEvent(buf$)
  Dim ThisName$, Args$, ReturnType$
  FindNameArgsAndType buf, ThisName, Args, ReturnType
  Visio.addEvent ThisName, Args
End Sub

Private Sub FindNameArgsAndType(buf$, ByRef ThisName$, ByRef Args$, ByRef ReturnType$)
  Dim openParen&
  Dim closeParen&
  Dim StartComment&
  Dim buf2$
  ThisName = ""
  Args = ""
  ReturnType = ""
  openParen = InStr(buf, "(")
  While openParen < 1 And Not EOF(ProjectFile)
    GoSub ReadAnotherLine
    openParen = InStr(buf, "(")
  Wend
  If openParen < 1 Then
    ThisName = buf
  Else
    ThisName = Left(buf, openParen - 2)
    buf2 = Mid(buf, openParen - 1, 1)
    buf = Mid(buf, openParen + 1)
    Select Case buf2
      Case "%": ReturnType = "As Integer"
      Case "&": ReturnType = "As Long"
      Case "!": ReturnType = "As Single"
      Case "#": ReturnType = "As Double"
      Case "@": ReturnType = "As Currency"
      Case "$": ReturnType = "As String"
      Case Else: ThisName = ThisName & buf2
    End Select
    closeParen = InStr(buf, ")")
    While closeParen < 1 And Not EOF(ProjectFile)
      GoSub ReadAnotherLine
      closeParen = InStr(buf, ")")
    Wend
    If closeParen < 1 Then
      Args = buf
    Else
      If closeParen > 1 Then
        While Mid(buf, closeParen - 1, 1) = "("
          closeParen = InStr(closeParen + 1, buf, ")")
          While closeParen = 0 And Not EOF(ProjectFile)
            GoSub ReadAnotherLine
            closeParen = InStr(closeParen + 1, buf, ")")
          Wend
        Wend
      End If
      Args = Left(buf, closeParen - 1)
      If ReturnType = "" Then ReturnType = Trim(Mid(buf, closeParen + 1))
    End If
  End If
  Exit Sub
ReadAnotherLine:
  Line Input #ProjectFile, buf2
  StartComment = InStr(buf2, "'")
  If StartComment > 0 Then buf2 = Left(buf2, StartComment - 1)
  buf = buf & buf2

End Sub
