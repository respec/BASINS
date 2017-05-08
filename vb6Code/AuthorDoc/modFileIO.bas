Attribute VB_Name = "modFileIO"
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants

Private Declare Function WaitForSingleObject Lib "kernel32" _
   (ByVal hHandle As Long, ByVal dwMilliseconds As Long) As Long

Private Declare Function CloseHandle Lib "kernel32" _
   (ByVal hObject As Long) As Long
   
Private Declare Function OpenProcess Lib "kernel32" _
   (ByVal dwDesiredAccess As Long, ByVal bInheritHandle As Long, _
    ByVal dwProcessId As Long) As Long


Private Const INFINITE = -1&
Private Const SYNCHRONIZE = &H100000
Private NconvertPath As String

Sub RunNconvert(cmdline As String)
  Dim iTask As Long, ret As Long, pHandle As Long
  
  If NconvertPath = "" Then FindNconvert
  
  iTask = Shell(NconvertPath & " " & cmdline, vbHide)
  pHandle = OpenProcess(SYNCHRONIZE, False, iTask)
  ret = WaitForSingleObject(pHandle, INFINITE)
  ret = CloseHandle(pHandle)

End Sub

Sub FindNconvert()
  NconvertPath = GetSetting("Nconvert", "Paths", "ExePath", "")
  If NconvertPath = "" Then GoTo OpenDialog
  If Len(Dir(NconvertPath)) = 0 Then GoTo OpenDialog
  Exit Sub
OpenDialog:
  With frmSample.cdlg
    .DialogTitle = "Find Nconvert.exe to perform conversion"
    .filename = "Nconvert.exe"
    .ShowOpen
    NconvertPath = .filename
  End With
  If Len(Dir(NconvertPath)) > 0 Then
    SaveSetting "Nconvert", "Paths", "ExePath", NconvertPath
  End If
End Sub

Sub OpenProject(filename$, t As TreeView)
  Dim f%        'file handle
  Dim buf$      'input buffer, contains current line
  Dim ThisName$ 'file name of current source file, minus extension
  Dim key$      'unique ID for tree control
  Dim SectionName$(50) 'Array of current section names for each level
  Dim SectionLevel&    'Level of current source file, according to indentation
  Dim lvl&             'Level in loop that constructs keys
  Dim nod As Node      'Node inserted into tree control
  Dim dotpos&   'position of . in filename
  Dim StartTime As Long
  StartTime = Timer
  
  On Error GoTo OpenError
  
  f = FreeFile(0)
  frmMain.MousePointer = vbHourglass
  t.Visible = False
  If Len(Dir(filename)) = 0 Then
    If MsgBox("File not found. Create new project file '" & filename & "'?", vbYesNo) = vbYes Then
      GoSub SetProjectName
      Open filename For Output As #f
      Close f
    End If
  Else
    GoSub SetProjectName
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
          Set nod = t.Nodes.Add("N" & BaseName, tvwChild, "N" & key, ThisName)
        Else
          For lvl = SectionLevel - 1 To 1 Step -1
            key = SectionName(lvl) & "\" & key
          Next lvl
          On Error GoTo skip:
          Set nod = t.Nodes.Add("N" & Left(key, Len(key) - Len(ThisName) - 1), tvwChild, "N" & key, ThisName)
          If Not nod.Parent.Expanded Then nod.Parent.Expanded = True
        End If
      End If
    Wend
    Close f
  End If
  t.Visible = True
  frmMain.MousePointer = vbDefault
  If t.Nodes.Count > 0 Then t.Nodes(1).EnsureVisible
  Exit Sub

OpenError:
  MsgBox "Error reading project file '" & filename & "'" & vbCr & Err.Description
  On Error Resume Next
  Close f
skip:
  Debug.Print "Duplicate key in tree: " & key
  Resume Next
SetProjectName:
  ProjectFileName = filename
  BaseName = FilenameOnly(filename)
  t.Nodes.Clear
  t.Nodes.Add , , "N" & BaseName, BaseName
  t.Nodes(1).Expanded = True
  Return
End Sub

Public Sub SaveProject(filename$, t As TreeView)
  Dim outfile%    'file handle
  Dim nodNum&     'Node number (we go sequentially through nodes)
  Dim nod As Node 'Node of the tree being written
  
  'Mark all as need to be saved
  For nodNum = 1 To t.Nodes.Count
    t.Nodes.Item(nodNum).tag = True
  Next
  t.Nodes.Item(1).tag = False

  outfile = FreeFile(0)
  Open filename For Output As #outfile
  Set nod = t.Nodes.Item(1).Child
  While Not nod Is Nothing
    WriteProjectSection nod, outfile
    Set nod = nod.Next
  Wend
  Close outfile
End Sub

Private Sub WriteProjectSection(nod As Node, outfile As Integer)
  Dim ThisName$        'file name of current source file, minus extension
  Dim pos&             'position of directory delimiter '\' in node key for counting levels
  Dim kid As Node      'nod's child
  
  If nod.tag Then
    If Not nod.Parent Is Nothing Then
      If nod.Parent.tag Then
        WriteProjectSection nod.Parent, outfile 'Write parent first
        Exit Sub 'Writing parent will lead to doing this node, so we are done
      End If
    End If
    nod.tag = False
    ThisName = ""
    pos = InStr(nod.key, "\")
    While pos > 0 And pos < Len(nod.key)
      ThisName = ThisName & "  "
      pos = InStr(pos + 1, nod.key, "\")
    Wend
  
    ThisName = ThisName & nod.Text
    Print #outfile, ThisName
    Set kid = nod.Child
    For pos = 1 To nod.Children
      WriteProjectSection kid, outfile
      Set kid = kid.Next
    Next
    Set kid = Nothing
  End If
End Sub
