Attribute VB_Name = "modFileIO"
Option Explicit

Sub OpenProject(filename$, t As TreeView)
  Dim f%        'file handle
  Dim buf$      'input buffer, contains current line
  Dim ThisType$ 'Type of thing current line in project references
  Dim ThisName$ 'file name of current source file, minus extension
  Dim key$      'unique ID for tree control
  Dim SectionName$(50) 'Array of current section names for each level
  Dim SectionLevel&    'Level of current source file, according to indentation
  Dim lvl&             'Level in loop that constructs keys
  Dim nod As Node      'Node inserted into tree control
  Dim dotpos&   'position of . in filename
  
  On Error GoTo OpenError
  
  f = FreeFile(0)
  If Len(Dir(filename)) = 0 Then
    If MsgBox("File not found. Create new project file '" & filename & "'?", vbYesNo) = vbYes Then
      ProjectFileName = filename
      t.Nodes.Clear
      Open filename For Output As #f
      Close f
    End If
  Else
    t.Nodes.Clear
    ProjectFileName = filename
    Open filename For Input As #f
    While Not EOF(f)  ' Loop until end of file.
      Line Input #f, buf
      
      ThisName = LTrim(buf)
      If ThisName <> "" Then
        SectionLevel = (Len(buf) - Len(ThisName)) / 2 + 1 '2 spaces indentation per level
        ThisName = RTrim(ThisName)
        key = ThisName
        SectionName(SectionLevel) = ThisName
        If SectionLevel = 1 Then
          Set nod = t.Nodes.add(, , key, ThisName)
        Else
          For lvl = SectionLevel - 1 To 1 Step -1
            key = SectionName(lvl) & "\" & key
          Next lvl
          Set nod = t.Nodes.add(Left(key, Len(key) - Len(ThisName) - 1), tvwChild, key, ThisName)
          nod.Parent.Expanded = True
        End If
      End If
    Wend
    Close f
  End If
  BaseName = FileNameOnly(filename)
  'dotpos = InStr(BaseName, ".")
  'If dotpos > 1 Then BaseName = Left(BaseName, dotpos - 1) '".foo" -> ".foo", "foo.txt" -> "foo"
  If t.Nodes.Count > 0 Then t.Nodes(1).EnsureVisible
  Exit Sub
OpenError:
  MsgBox "Error reading project file '" & filename & "'" & vbCr & Err.Description
  On Error Resume Next
  Close f
End Sub

Public Sub SaveProject(filename$, t As TreeView)
  Dim f%               'file handle
  Dim ThisName$        'file name of current source file, minus extension
  Dim nod As Node      'Node in tree control that we are writing to file
  'Dim par As Node      'Current node's (grand)parent as we find this node's level
  Dim nodNum&          'Node number (we go sequentially through nodes)
  Dim pos&             'position of directory delimiter '\' in node key for counting levels
  
  f = FreeFile(0)
  Open filename For Output As #f
  For nodNum = 1 To t.Nodes.Count
    Set nod = t.Nodes.Item(nodNum)
    ThisName = ""
    pos = InStr(nod.key, "\")
    While pos > 0 And pos < Len(nod.key)
      ThisName = ThisName & "  "
      pos = InStr(pos + 1, nod.key, "\")
    Wend
    'Set par = nod.Parent
    'On Error GoTo AppendText
    'While Not IsNull(par)
    '  par = par.Parent
    '  ThisName = ThisName & "  "
    'Wend
    'AppendText:
    'On Error GoTo 0
    ThisName = ThisName & nod.Text
    Print #f, ThisName
  Next nodNum
  Set nod = Nothing
  'Set par = Nothing
  Close f
End Sub

