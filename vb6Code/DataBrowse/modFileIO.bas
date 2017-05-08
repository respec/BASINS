Attribute VB_Name = "modFileIO"
Option Explicit

Sub OpenProject(Filename$)
  Dim f%        'file handle
  Dim buf$      'input buffer, contains current line
  Dim ThisName$ 'file name of current source file, minus extension
  Dim key$      'unique ID for tree control
  Dim SectionName$(50) 'Array of current section names for each level
  Dim SectionLevel&    'Level of current source file, according to indentation
  Dim lvl&             'Level in loop that constructs keys
  Dim nod As Node      'Node inserted into tree control
  
  'On Error GoTo OpenError
  
  f = FreeFile(0)
  Open Filename For Input As #f
  frmMain.tree1.Nodes.Clear
  While Not EOF(f)  ' Loop until end of file.
    Line Input #f, buf
    ThisName = LTrim(buf)
    SectionLevel = (Len(buf) - Len(ThisName)) / 2 + 1 '2 spaces indentation per level
    ThisName = RTrim(ThisName)
    key = ThisName
    SectionName(SectionLevel) = ThisName
    If SectionLevel = 1 Then
      Set nod = frmMain.tree1.Nodes.Add(, , key, ThisName)
    Else
      For lvl = SectionLevel - 1 To 1 Step -1
        key = SectionName(lvl) & "\" & key
      Next lvl
      Set nod = frmMain.tree1.Nodes.Add(Left(key, Len(key) - Len(ThisName) - 1), tvwChild, key, ThisName)
      nod.Parent.Expanded = True
    End If
  Wend
  Close f
  Exit Sub
OpenError:
  MsgBox "Error reading project file '" & Filename & "'" & vbCr & Err.Description
End Sub

Function PathNameOnly(istr$)
  'determine the path part of a string
  'containing path, file name, and extension
  Dim ilen&, i&, slashpos&
  
  ilen = Len(istr)
  
  slashpos = 0
  i = ilen + 1
  While i > 0 And slashpos = 0
    If Mid(istr, i, 1) = "\" Then
      slashpos = i
    Else
      i = i - 1
    End If
  Wend
  If slashpos > 0 Then
    PathNameOnly = Mid(istr, 1, slashpos - 1)
  Else
    PathNameOnly = ""
  End If
End Function

