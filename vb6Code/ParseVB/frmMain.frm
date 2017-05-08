VERSION 5.00
Object = "{6B7E6392-850A-101B-AFC0-4210102A8DA7}#1.3#0"; "comctl32.ocx"
Object = "{F9043C88-F6F2-101A-A3C9-08002B2F49FB}#1.2#0"; "COMDLG32.OCX"
Object = "{3B7C8863-D78F-101B-B9B5-04021C009402}#1.2#0"; "richtx32.ocx"
Begin VB.Form frmMain 
   Caption         =   "Parse VB"
   ClientHeight    =   5700
   ClientLeft      =   165
   ClientTop       =   735
   ClientWidth     =   6825
   LinkTopic       =   "Form1"
   ScaleHeight     =   5700
   ScaleWidth      =   6825
   StartUpPosition =   3  'Windows Default
   Begin VB.Frame fraFind 
      BorderStyle     =   0  'None
      Caption         =   "Frame1"
      Height          =   492
      Left            =   1920
      TabIndex        =   3
      Top             =   0
      Width           =   4692
      Begin VB.CommandButton cmdReplace 
         Caption         =   "Replace:"
         Height          =   252
         Left            =   2280
         TabIndex        =   7
         Top             =   120
         Width           =   972
      End
      Begin VB.CommandButton cmdFind 
         Caption         =   "Find:"
         Default         =   -1  'True
         Height          =   252
         Left            =   0
         TabIndex        =   6
         Top             =   120
         Width           =   732
      End
      Begin VB.TextBox txtReplace 
         Height          =   288
         Left            =   3360
         MultiLine       =   -1  'True
         TabIndex        =   5
         Top             =   120
         Width           =   1332
      End
      Begin VB.TextBox txtFind 
         Height          =   288
         Left            =   840
         MultiLine       =   -1  'True
         TabIndex        =   4
         Top             =   120
         Width           =   1332
      End
   End
   Begin VB.Timer Timer1 
      Enabled         =   0   'False
      Interval        =   100
      Left            =   6360
      Top             =   0
   End
   Begin VB.Frame sash 
      BorderStyle     =   0  'None
      Caption         =   "Frame1"
      Height          =   5535
      Left            =   1800
      MousePointer    =   9  'Size W E
      TabIndex        =   0
      Top             =   0
      Width           =   65
   End
   Begin RichTextLib.RichTextBox txtMain 
      Height          =   5052
      Left            =   1920
      TabIndex        =   2
      Top             =   480
      Width           =   4692
      _ExtentX        =   8281
      _ExtentY        =   8916
      _Version        =   393217
      Enabled         =   -1  'True
      HideSelection   =   0   'False
      ScrollBars      =   2
      TextRTF         =   $"frmMain.frx":0000
      BeginProperty Font {0BE35203-8F91-11CE-9DE3-00AA004BB851} 
         Name            =   "Courier New"
         Size            =   9.75
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
   End
   Begin MSComDlg.CommonDialog cdlg 
      Left            =   120
      Top             =   1080
      _ExtentX        =   847
      _ExtentY        =   847
      _Version        =   393216
   End
   Begin ComctlLib.TreeView tree1 
      Height          =   5535
      Left            =   0
      TabIndex        =   1
      Top             =   0
      Width           =   1815
      _ExtentX        =   3201
      _ExtentY        =   9763
      _Version        =   327682
      HideSelection   =   0   'False
      Indentation     =   529
      Style           =   7
      ImageList       =   "ilTree"
      Appearance      =   1
   End
   Begin MSComDlg.CommonDialog cdlgImage 
      Left            =   0
      Top             =   0
      _ExtentX        =   847
      _ExtentY        =   847
      _Version        =   393216
   End
   Begin ComctlLib.ImageList ilTree 
      Left            =   6360
      Top             =   360
      _ExtentX        =   794
      _ExtentY        =   794
      BackColor       =   -2147483643
      ImageWidth      =   16
      ImageHeight     =   16
      MaskColor       =   12632256
      UseMaskColor    =   0   'False
      _Version        =   327682
      BeginProperty Images {0713E8C2-850A-101B-AFC0-4210102A8DA7} 
         NumListImages   =   9
         BeginProperty ListImage1 {0713E8C3-850A-101B-AFC0-4210102A8DA7} 
            Picture         =   "frmMain.frx":0074
            Key             =   "cls"
         EndProperty
         BeginProperty ListImage2 {0713E8C3-850A-101B-AFC0-4210102A8DA7} 
            Picture         =   "frmMain.frx":024E
            Key             =   "ctl"
         EndProperty
         BeginProperty ListImage3 {0713E8C3-850A-101B-AFC0-4210102A8DA7} 
            Picture         =   "frmMain.frx":0428
            Key             =   "frm"
         EndProperty
         BeginProperty ListImage4 {0713E8C3-850A-101B-AFC0-4210102A8DA7} 
            Picture         =   "frmMain.frx":0602
            Key             =   "MDIchild"
         EndProperty
         BeginProperty ListImage5 {0713E8C3-850A-101B-AFC0-4210102A8DA7} 
            Picture         =   "frmMain.frx":07DC
            Key             =   "MDIparent"
         EndProperty
         BeginProperty ListImage6 {0713E8C3-850A-101B-AFC0-4210102A8DA7} 
            Picture         =   "frmMain.frx":09B6
            Key             =   "misc"
         EndProperty
         BeginProperty ListImage7 {0713E8C3-850A-101B-AFC0-4210102A8DA7} 
            Picture         =   "frmMain.frx":0B90
            Key             =   "bas"
         EndProperty
         BeginProperty ListImage8 {0713E8C3-850A-101B-AFC0-4210102A8DA7} 
            Picture         =   "frmMain.frx":0D6A
            Key             =   "vbg"
         EndProperty
         BeginProperty ListImage9 {0713E8C3-850A-101B-AFC0-4210102A8DA7} 
            Picture         =   "frmMain.frx":0F44
            Key             =   "vbp"
         EndProperty
      EndProperty
   End
   Begin VB.Menu mnuTop 
      Caption         =   "&File"
      Index           =   0
      Begin VB.Menu mnuOpen 
         Caption         =   "&Open"
      End
      Begin VB.Menu sep1 
         Caption         =   "-"
      End
      Begin VB.Menu mnuSaveFile 
         Caption         =   "&Save File"
         Enabled         =   0   'False
      End
      Begin VB.Menu mnuSaveAs 
         Caption         =   "Save As..."
      End
      Begin VB.Menu mnuRevert 
         Caption         =   "&Revert to Saved"
      End
      Begin VB.Menu mnuAutoSave 
         Caption         =   "&Auto-Save"
      End
      Begin VB.Menu mnuRecent 
         Caption         =   "-"
         Index           =   0
         Visible         =   0   'False
      End
      Begin VB.Menu sep3 
         Caption         =   "-"
      End
      Begin VB.Menu mnuExit 
         Caption         =   "E&xit"
      End
   End
   Begin VB.Menu mnuTop 
      Caption         =   "&Edit"
      Index           =   1
      Begin VB.Menu mnuCut 
         Caption         =   "Cut"
      End
      Begin VB.Menu mnuCopy 
         Caption         =   "Copy"
      End
      Begin VB.Menu mnuPaste 
         Caption         =   "Paste"
      End
      Begin VB.Menu mnuUndo 
         Caption         =   "Undo"
         Shortcut        =   ^Z
      End
      Begin VB.Menu sep5 
         Caption         =   "-"
      End
      Begin VB.Menu mnuFindSelection 
         Caption         =   "Find Selection"
         Shortcut        =   ^F
      End
      Begin VB.Menu mnuFind 
         Caption         =   "Find"
         Shortcut        =   {F3}
      End
      Begin VB.Menu sep6 
         Caption         =   "-"
      End
      Begin VB.Menu mnuOptionExplicit 
         Caption         =   "Enforce Option Explicit"
      End
      Begin VB.Menu mnuComment 
         Caption         =   "Insert Standard Comment"
      End
      Begin VB.Menu mnuRemoveObjRef 
         Caption         =   "Remove Object References"
      End
   End
   Begin VB.Menu mnuTop 
      Caption         =   "&View"
      Index           =   2
      Begin VB.Menu mnuFormatting 
         Caption         =   "&Formatting"
      End
      Begin VB.Menu mnuOptions 
         Caption         =   "&Options"
      End
   End
   Begin VB.Menu mnuTop 
      Caption         =   "&Analyze"
      Index           =   3
      Begin VB.Menu mnuCount 
         Caption         =   "&Count"
      End
   End
   Begin VB.Menu mnuTop 
      Caption         =   "Package"
      Index           =   4
      Begin VB.Menu mnuCompare 
         Caption         =   "&Compare"
      End
      Begin VB.Menu mnuPack 
         Caption         =   "&Pack"
         Enabled         =   0   'False
      End
      Begin VB.Menu mnuUnpack 
         Caption         =   "&Unpack"
         Enabled         =   0   'False
      End
   End
End
Attribute VB_Name = "frmMain"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants

Dim path$
Dim CurrentFilename$                    'current file in txtMain
Dim CurrentFileContents$                'What was last saved or retrieved from file
Dim Undos$()
Dim UndoCursor&()
Dim UndoPos&
Dim UndosAvail&
Dim MaxUndo&
Dim Undoing As Boolean
Dim Changed As Boolean                  'True if txtMain.Text has been edited
Dim ProjectChanged As Boolean
Dim ViewFormatting As Boolean
Dim txtMainButton As Long

Dim tagName$, openTagPos&, closeTagPos& 'current tag being edited
Dim NodeLinking&                        'Index in tree of file containing link being edited

Private SashDragging As Boolean
Private pShowAllItems As Boolean

Private Const lenCheckComment = 10
Private Const DefaultSourceComment = "'Copyright 2001 by AQUA TERRA Consultants"

Private Const SectionMainWin = "Main Window"
Private Const SectionRecentFiles = "Recent Files"
Private Const MaxRecentFiles = 6

Public Property Get ShowAllItems() As Boolean
  ShowAllItems = pShowAllItems
End Property

Public Property Let ShowAllItems(newValue As Boolean)
  Dim nod As Node, nodnum As Long
  If newValue <> pShowAllItems Then
    pShowAllItems = newValue
    If Not pShowAllItems Then
      For nodnum = 1 To tree1.Nodes.Count
        If tree1.Nodes(nodnum).Image = "misc" Then tree1.Nodes.Remove nodnum
      Next
    End If
  End If
End Property

Private Sub cmdFind_MouseUp(Button As Integer, Shift As Integer, x As Single, y As Single)
  If Button = vbRightButton Then
    fraFind.Visible = False
    Form_Resize
  Else
    Dim searchThrough$, SearchFor$, SelStart&, searchPos&, startNodeIndex&
    searchThrough = txtMain.text
    If txtFind.text = "" And txtMain.SelLength > 0 Then txtFind.text = txtMain.SelText
    If txtFind.text <> "" Then
      SearchFor = txtFind.text
      SelStart = txtMain.SelStart
      searchPos = txtMain.SelStart + txtMain.SelLength
      searchPos = txtMain.Find(SearchFor, searchPos)
      startNodeIndex = tree1.SelectedItem.index
      If searchPos < 0 Then
        If QuerySave <> vbCancel Then
NextNode:
          If tree1.SelectedItem.index < tree1.Nodes.Count Then
            tree1.SelectedItem = tree1.Nodes(tree1.SelectedItem.index + 1)
          Else
            tree1.SelectedItem = tree1.Nodes(1)
          End If
          tree1_NodeClick tree1.SelectedItem
          searchPos = txtMain.Find(SearchFor, 0)
          If searchPos < 0 And tree1.SelectedItem.index <> startNodeIndex Then GoTo NextNode
        End If
      End If
    End If
  End If
End Sub

Private Sub cmdReplace_MouseUp(Button As Integer, Shift As Integer, x As Single, y As Single)
  Dim startNodeIndex&, searchedBeyondStart As Boolean
  If Button = vbRightButton Then
    fraFind.Visible = False
    Form_Resize
  Else
    startNodeIndex = tree1.SelectedItem.index
    searchedBeyondStart = False
    If LCase(txtMain.SelText) = LCase(txtFind.text) Then
NextReplace:
      txtMain.SelText = txtReplace.text
    End If
    cmdFind_MouseUp Button, Shift, x, y
    If startNodeIndex <> tree1.SelectedItem.index Then searchedBeyondStart = True
    If Shift > 0 Then
      If Not searchedBeyondStart Or startNodeIndex <> tree1.SelectedItem.index Then
        If LCase(txtMain.SelText) = LCase(txtFind.text) Then GoTo NextReplace
      End If
    End If
  End If
End Sub

Private Sub Form_Load()
  SourceComment = GetSetting(App.Title, "Defaults", "SourceComment", DefaultSourceComment)
  MaxUndo = 10
  ReDim Undos(MaxUndo)
  ReDim UndoCursor(MaxUndo)
  txtMain.text = ""
  RetrieveWindowSettings
  'Path = GetSetting(App.Title, "Defaults", "Path", CurDir)
  'cdlgImage.Filename = Path
  'If Dir(Path & "\") <> "" Then ChDir Path
  ViewFormatting = mnuFormatting.Checked
End Sub

Private Sub Form_Resize()
  Dim newWidth&
  If Height > 800 Then sash.Height = Height - 753 'menu height
  tree1.Height = sash.Height
  If fraFind.Visible Then
    txtMain.Top = fraFind.Top + fraFind.Height
  Else
    txtMain.Top = fraFind.Top
  End If
  
  If sash.Height > txtMain.Top Then txtMain.Height = sash.Height - txtMain.Top
  
  txtMain.Left = sash.Left + sash.Width
  fraFind.Left = txtMain.Left
  newWidth = Width - txtMain.Left - 100
  If newWidth > 0 Then
    txtMain.Width = newWidth
    If fraFind.Visible Then
      fraFind.Width = newWidth
      If (newWidth - 324 - cmdFind.Width - cmdReplace.Width) > 100 Then
        txtFind.Width = (newWidth - cmdFind.Width - cmdReplace.Width - 324) / 2
        cmdReplace.Left = txtFind.Left + txtFind.Width + 108
        txtReplace.Left = cmdReplace.Left + cmdReplace.Width + 108
        txtReplace.Width = txtFind.Width
      End If
    End If
  End If
End Sub

Private Sub Form_Unload(Cancel As Integer)
  If QuerySave = vbCancel Then
    Cancel = 1
  ElseIf QuerySaveProject = vbCancel Then
    Cancel = 1
  Else
    'If Len(BaseName) > 0 Then SaveSetting App.Title, "Defaults", "BaseName", BaseName
    'If Len(Path) > 0 Then SaveSetting App.Title, "Defaults", "Path", Path
    SaveSetting App.Title, "Defaults", "SourceComment", SourceComment
    SaveWindowSettings
    Dim frm As Object
    For Each frm In Forms
      Unload frm
    Next frm
    End
  End If
End Sub

Private Sub mnuAutoSave_Click()
  mnuAutoSave.Checked = Not mnuAutoSave.Checked
End Sub

Private Sub mnuContext_Click(index As Integer)
  'ContextAction mnuContext(Index).Caption
End Sub

Private Sub mnuComment_Click()
  frmTextBox.text = frmTextBox.text & vbCrLf & "Inserting Comment--------------------------------------"
  frmTextBox.Visible = True

  EnforceOptionExplicit ItemFromNodeKey(tree1.Nodes(tree1.SelectedItem.index).Key), True
End Sub

Private Sub mnuCompare_Click()
  MsgBox "Compare not yet implemented - try the program BeyondCompare"
'  If Len(Path) > 0 And Len(Dir(Path & "\")) > 0 Then ChDir Path
'  cdlg.Filename = ""
'  cdlg.DefaultExt = ""
'  cdlg.Filter = "Group/Project Files|*.vbg;*.vbp|Setup List|*.lst|Tar files (*.tar)|*.tar|All Files (*.*)|*.*"
'  cdlg.FilterIndex = 1
'  cdlg.ShowOpen
'  If Len(cdlg.Filename) > 0 Then
    
End Sub

Private Sub mnuCopy_Click()
  Clipboard.SetText txtMain.SelText
End Sub

Private Sub mnuCount_Click()
  CountAll
End Sub

Private Sub mnuCut_Click()
  Clipboard.SetText txtMain.SelText
  txtMain.SelText = ""
End Sub

Private Sub mnuExit_Click()
  Form_Unload 0
End Sub

Private Sub mnuFind_Click()
  If fraFind.Visible Then
    cmdFind_MouseUp vbLeftButton, 0, 0, 0
  Else
    fraFind.Visible = True
    Form_Resize
  End If
End Sub

Private Sub mnuFindSelection_Click()
'    Case 6 'Control-F = find
  If Not fraFind.Visible Then
    fraFind.Visible = True
    Form_Resize
  End If
  If Len(txtMain.SelText) < 1 Then
    Dim SelStart As Long, SelEnd As Long, txtLen As Long
    txtLen = Len(txtMain.text)
    SelEnd = txtMain.SelStart
    SelStart = txtMain.SelStart
    Do While SelStart > 0
      If IsAlphaNumeric(Mid(txtMain.text, SelStart, 1)) Then
        SelStart = SelStart - 1
      Else
        Exit Do
      End If
    Loop
    Do While SelEnd <= txtLen
      If IsAlphaNumeric(Mid(txtMain.text, SelEnd + 1, 1)) Then
        SelEnd = SelEnd + 1
      Else
        Exit Do
      End If
    Loop
    txtMain.SelStart = SelStart
    txtMain.SelLength = SelEnd - SelStart
  End If
  txtFind.text = txtMain.SelText
  cmdFind.SetFocus
End Sub

Private Sub mnuFormatting_Click()
  mnuFormatting.Checked = Not mnuFormatting.Checked
  ViewFormatting = mnuFormatting.Checked
  If ViewFormatting Then
    FormatText txtMain
  Else
    txtMain.text = txtMain.text
    txtMain.Refresh
  End If
End Sub

Private Sub mnuOpen_Click()
  
  If Len(path) > 0 And Len(Dir(path & "\")) > 0 Then ChDir path
  cdlg.filename = ""
  cdlg.DefaultExt = ""
  cdlg.Filter = "Group/Project Files|*.vbg;*.vbp|Setup List|*.lst|Tar files (*.tar)|*.tar|All Files (*.*)|*.*"
  cdlg.FilterIndex = 1
  cdlg.ShowOpen
  If Len(cdlg.filename) > 0 Then OpenFile TopItem, cdlg.filename
  
End Sub

Private Sub OpenFile(newTopItem As clsVBitem, filename As String)
  
  Me.MousePointer = vbHourglass
  
  Set AllItems = Nothing
  Set AllItems = New Collection
  
  AddRecentFile filename
  path = PathNameOnly(filename)
  GetFromTar = False
  Set newTopItem = Nothing
  Select Case LCase(Right(filename, 3))
    Case "vbg": Set newTopItem = New clsGroup
    Case "vbp": Set newTopItem = New clsProject
    Case "cls": Set newTopItem = New clsModule
    Case "bas": Set newTopItem = New clsModule
    Case "frm": Set newTopItem = New clsForm
    Case "ctl": Set newTopItem = New clsForm
    Case "tar":  GetFromTar = True
                 OpenTar filename
                 GoTo ExitSub
'      Case "zip": OpenZip filename
'                  Exit Sub
    Case "lst": Set newTopItem = New clsLst
    Case Else:  MsgBox "Cannot open this type of file: " & vbCr & filename, vbOKOnly, "Open File"
                GoTo ExitSub
  End Select
  newTopItem.path = filename

  tree1.Visible = False
  RefreshTree TopItem
  tree1.Visible = True
  tree1.SelectedItem = tree1.Nodes(1)
  
  Me.MousePointer = vbDefault

ExitSub:
  mnuPack.Enabled = True
End Sub

Private Sub RefreshTree(vbi As clsVBitem, Optional parent As String = "")
  Dim child As Long, children As Long, thisName As String, thisKey As String
  thisName = vbi.Name
  thisKey = vbi.path 'parent & "|" & thisName
  On Error GoTo HaveUniqueKey 'Try to find a unique key if there is more than one item in a file
  If tree1.Nodes(thisKey).Key = thisKey Then thisKey = vbi.path & "|" & thisName
  While tree1.Nodes(thisKey).Key = thisKey
    thisKey = thisKey & "."
  Wend
HaveUniqueKey:
  Err.clear
  On Error GoTo 0
  If parent = "" Then
    tree1.Nodes.clear
    tree1.Nodes.Add , , thisKey, thisName
  Else
    tree1.Nodes.Add parent, tvwChild, thisKey, thisName
  End If
  tree1.Nodes(thisKey).tag = vbi.Body
  On Error GoTo Nevermind 'AllItems.Add may fail if another item in this file has already been added
  Select Case vbi.VBItype
    Case vbi_Class:       tree1.Nodes(thisKey).Image = "cls": AllItems.Add vbi, thisKey
    Case vbi_Form:        tree1.Nodes(thisKey).Image = "frm": AllItems.Add vbi, thisKey
    Case vbi_Group:       tree1.Nodes(thisKey).Image = "vbg": AllItems.Add vbi, thisKey
    Case vbi_Module:      tree1.Nodes(thisKey).Image = "bas": AllItems.Add vbi, thisKey
    Case vbi_Project:     tree1.Nodes(thisKey).Image = "vbp": AllItems.Add vbi, thisKey
    Case vbi_UserControl: tree1.Nodes(thisKey).Image = "ctl": AllItems.Add vbi, thisKey
    Case vbi_List:        tree1.Nodes(thisKey).Image = "misc": AllItems.Add vbi, thisKey
    Case Else:            tree1.Nodes(thisKey).Image = "misc"
      If Not pShowAllItems Then tree1.Nodes.Remove thisKey
      'tree1.Nodes(thisKey).Visible = pShowAllItems
  End Select
NevermindNevermind:
  children = vbi.nItems
  For child = 1 To children
    RefreshTree vbi.item(child), thisKey
  Next child
  If children > 0 Then tree1.Nodes(thisKey).Expanded = True
  Exit Sub
Nevermind:
  Err.clear
  On Error GoTo 0
  Resume NevermindNevermind
End Sub

Private Sub OpenTar(TarFilename As String)
  Dim txt As String, index As Long
  Dim parentName As String
  parentName = FilenameOnly(TarFilename) & ".tar"
  Set TarFile = New clsTar
  TarFile.TarFilename = TarFilename
  Set TopItem = Nothing
  txt = TarFile.ArchiveFilename(1)
  Select Case LCase(Right(txt, 3))
    Case "vbg": Set TopItem = New clsGroup
    Case "vbp": Set TopItem = New clsProject
    Case "cls": Set TopItem = New clsModule
    Case "bas": Set TopItem = New clsModule
    Case "frm": Set TopItem = New clsForm
    Case "ctl": Set TopItem = New clsForm
    Case Else:  MsgBox "Cannot open this type of file: " & vbCr & txt, vbOKOnly, "Open File"
                Exit Sub
  End Select
  txtMain.text = ""
  TopItem.Name = txt
  RefreshTree TopItem
End Sub

Private Sub mnuOptionExplicit_Click()
  frmTextBox.text = frmTextBox.text & vbCrLf & "Enforcing Option Explicit--------------------------------------"
  frmTextBox.Visible = True

  EnforceOptionExplicit ItemFromNodeKey(tree1.Nodes(tree1.SelectedItem.index).Key), False
End Sub

Private Sub EnforceOptionExplicit(vbi As clsVBitem, InsertComment As Boolean)
  Dim child As Long, children As Long
  Dim inFile As Integer, OutFile As Integer, buf As String
  Dim ChangedSomething As Boolean, StartedAttributes As Boolean
  
  Select Case vbi.VBItype
    Case vbi_Class, vbi_Form, vbi_UserControl, vbi_Module
      
      ChangedSomething = False
      inFile = FreeFile
      Open vbi.path For Input As inFile
      OutFile = FreeFile
      Open vbi.path & ".ParseTemp" For Output As OutFile
      While Not EOF(inFile)
        Line Input #inFile, buf
        If Left(buf, 13) = "Attribute VB_" Then
          StartedAttributes = True
          Print #OutFile, buf
        ElseIf Not StartedAttributes Then
          Print #OutFile, buf
        Else
          If Left(buf, 15) = "Option Explicit" Then
            If EOF(inFile) Then buf = "" Else Line Input #inFile, buf
          Else
            ChangedSomething = True
            frmTextBox.text = frmTextBox.text & vbCrLf & "++" & vbi.path & ": Option Explicit"
          End If
          Print #OutFile, "Option Explicit"
          If InsertComment Then
            If Left(buf, lenCheckComment) <> Left(SourceComment, lenCheckComment) Then
              Print #OutFile, SourceComment
              ChangedSomething = True
              frmTextBox.text = frmTextBox.text & vbCrLf & "++" & vbi.path & ": " & SourceComment
            End If
          End If
          Print #OutFile, buf
          StartedAttributes = False
        End If
      Wend
      Close inFile
      Close OutFile
      If ChangedSomething Then
        OutFile = 1
        buf = vbi.path & ".Backup" & OutFile
        While Len(Dir(buf)) > 0
          OutFile = OutFile + 1
          buf = vbi.path & ".Backup" & OutFile
        Wend
        Name vbi.path As buf
        Name vbi.path & ".ParseTemp" As vbi.path
      Else
        Kill vbi.path & ".ParseTemp"
      End If
      
  End Select
  children = vbi.nItems
  For child = 1 To children
    EnforceOptionExplicit vbi.item(child), InsertComment
  Next child
End Sub

'Private Sub OpenZip(ZipFilename As String)
'  Dim ListBuf As String
'  VBUnzipToBuffer ZipFilename, FileListFilename, ListBuf
'  txtMain.Text = ListBuf
'End Sub

Private Sub mnuOptions_Click()
  frmOptions.Show
End Sub

Private Sub mnuRecent_Click(index As Integer)
  Dim newFilePath$
  If index > 0 Then
    newFilePath = mnuRecent(index).tag
    OpenFile TopItem, newFilePath
  End If
End Sub

Private Sub mnuRemoveObjRef_Click()
  Dim nod As Node, SelItm As clsVBitem, GrpItm As clsVBitem
  Set nod = tree1.Nodes(tree1.SelectedItem.index)
  Set SelItm = ItemFromNodeKey(nod.Key)
  Set GrpItm = SelItm
  While GrpItm.VBItype <> vbi_Group
    Set nod = nod.parent
    If nod Is Nothing Then
      If MsgBox("No group was found to search for related projects." & vbCr _
           & "Project references cannot be fixed without a group." & vbCr _
           & "Continue anyway?", vbOKCancel, "No Group") = vbOK Then
        RemoveProjectObjRef "VBG"
        GoTo SkipGroup
      End If
    End If
    Set GrpItm = ItemFromNodeKey(nod.Key)
    If GrpItm.VBItype = vbi_Group Then RemoveProjectObjRef GrpItm.path
  Wend
SkipGroup:
  frmTextBox.text = frmTextBox.text & vbCrLf & "Removing Object References--------------------------------------"
  frmTextBox.Visible = True
  RemoveAllObjRef SelItm
End Sub

Private Sub RemoveAllObjRef(vbi As clsVBitem)
  Dim child As Long, children As Long
  
  Select Case vbi.VBItype
    Case vbi_Class, vbi_Form, vbi_UserControl: RemoveFileObjRef vbi.path
    Case vbi_Group:                            RemoveProjectObjRef vbi.path
    Case vbi_Project:                          'ChDir Path
                                               RemoveProjectObjRef vbi.path
                                               'If InStr(vbi.Name, "\") > 0 Then ChDir PathNameOnly(vbi.Path)
  End Select
  
  children = vbi.nItems
  For child = 1 To children
    RemoveAllObjRef vbi.item(child)
  Next child
End Sub

Private Sub RemoveProjectObjRef(filename As String)
  Static ProjectFileName(255) As String
  Static ProjectName(255) As String
  Static ProjectNames As Long
  Static GroupPath As String
  Dim DoingProjectFile As Boolean
  Dim ParsePos As Long, ProjectNum As Long
  Dim inFile As Integer, OutFile As Integer
  Dim buf As String, newBuf As String, ChangedSomething As Boolean
  Dim Ubuf As String
  Dim BackupFilename As String
  
  If UCase(Right(filename, 3)) = "VBP" Then
    DoingProjectFile = True
    OutFile = FreeFile
    Open filename & ".ParseTemp" For Output As OutFile
  Else
    DoingProjectFile = False
    ProjectNames = 0
    GroupPath = PathNameOnly(filename) & "\"
    If filename = "VBG" Then Exit Sub
  End If
  inFile = FreeFile
  Open filename For Input As inFile
  While Not EOF(inFile)
    Line Input #inFile, buf
    ChangedSomething = False 'Different from RemoveFileObjRef
    Ubuf = UCase(buf)
    If DoingProjectFile Then
      If Left(buf, 8) = "Object={" Then
        For ProjectNum = 1 To ProjectNames
          ParsePos = InStr(Ubuf, ProjectFileName(ProjectNum) & ".OCX")
          If ParsePos > 0 Then
            Select Case Asc(Mid(Ubuf, ParsePos - 1, 1))
              Case Is < 65, Is > 90 'Looks like whole base filename matches a project in this group
                newBuf = "Object=*\A" & RelativeFilename(GroupPath & ProjectName(ProjectNum), PathNameOnly(filename))
                frmTextBox.text = frmTextBox.text & vbCrLf & "--" & filename & ": " & buf
                frmTextBox.text = frmTextBox.text & vbCrLf & "++" & filename & ": " & newBuf
                Print #OutFile, newBuf
                ChangedSomething = True
                Exit For
            End Select
          End If
        Next
      ElseIf Left(buf, 14) = "Reference=*\G{" Then
        ParsePos = InStr(Ubuf, ".OCA")
        If ParsePos > 0 Then 'Remove reference to OCA - usually MSCOMCTL.OCX Object gets mangled into OCA
          frmTextBox.text = frmTextBox.text & vbCrLf & "--" & filename & ": " & buf
        Else
          For ProjectNum = 1 To ProjectNames
            ParsePos = InStr(Ubuf, ProjectFileName(ProjectNum) & ".DLL")
            If ParsePos > 0 Then
              Select Case Asc(Mid(Ubuf, ParsePos - 1, 1))
                Case Is < 65, Is > 90 'Looks like whole base filename matches a project in this group
                  newBuf = "Reference=*\A" & RelativeFilename(GroupPath & ProjectName(ProjectNum), PathNameOnly(filename))
                  frmTextBox.text = frmTextBox.text & vbCrLf & "--" & filename & ": " & buf
                  frmTextBox.text = frmTextBox.text & vbCrLf & "++" & filename & ": " & newBuf
                  Print #OutFile, newBuf
                  ChangedSomething = True
                  Exit For
              End Select
            End If
          Next
        End If
      End If
      If Not ChangedSomething Then Print #OutFile, buf
    Else
      ParsePos = InStr(buf, "Project=")
      If ParsePos > 0 Then
        ProjectNames = ProjectNames + 1
        ProjectName(ProjectNames) = Mid(buf, ParsePos + 8)
        ProjectFileName(ProjectNames) = UCase(FilenameOnly(ProjectName(ProjectNames)))
      End If
    End If
  Wend
  Close inFile
  If DoingProjectFile Then
    Close OutFile
    ParsePos = 1
    BackupFilename = filename & ".Backup" & ParsePos
    While Len(Dir(BackupFilename)) > 0
      ParsePos = ParsePos + 1
      BackupFilename = filename & ".Backup" & ParsePos
    Wend
    Name filename As BackupFilename
    Name filename & ".ParseTemp" As filename
  End If
End Sub

Private Sub RemoveFileObjRef(filename As String)
  Dim inFile As Integer, OutFile As Integer, buf As String
  Dim ChangedSomething As Boolean, StartedAttributes As Boolean
  ChangedSomething = False
  inFile = FreeFile
  Open filename For Input As inFile
  OutFile = FreeFile
  Open filename & ".ParseTemp" For Output As OutFile
  While Not EOF(inFile)
    Line Input #inFile, buf
    If Left(buf, 8) = "Object =" Then
      ChangedSomething = True
      frmTextBox.text = frmTextBox.text & vbCrLf & "--" & filename & ": " & buf
    ElseIf Left(buf, 13) = "Attribute VB_" Then
      StartedAttributes = True
      Print #OutFile, buf
    ElseIf Not StartedAttributes Then
      Print #OutFile, buf
    Else
      Print #OutFile, buf
      StartedAttributes = False
    End If
  Wend
  Close inFile
  Close OutFile
  If ChangedSomething Then
    OutFile = 1
    buf = filename & ".Backup" & OutFile
    While Len(Dir(buf)) > 0
      OutFile = OutFile + 1
      buf = filename & ".Backup" & OutFile
    Wend
    Name filename As buf
    Name filename & ".ParseTemp" As filename
  Else
    Kill filename & ".ParseTemp"
  End If
End Sub

Private Sub mnuPaste_Click()
  txtMain.SelText = Clipboard.GetText
End Sub

Private Sub mnuRevert_Click()
  txtMain.text = CurrentFileContents
End Sub

Private Sub mnuSaveAs_Click()
  Dim f%        'file handle
  Dim nod As Node, SelItm As clsVBitem, searchItm As clsVBitem
  Dim buf As String, buf2 As String, oldFilename As String, newFilename As String
  Set nod = tree1.Nodes(tree1.SelectedItem.index)
  Set SelItm = ItemFromNodeKey(nod.Key)
  oldFilename = SelItm.Name
  If Len(SelItm.path) > 3 Then
    ChDir PathNameOnly(SelItm.path)
    cdlg.DefaultExt = Right(SelItm.path, 3)
    cdlg.Filter = "*." & cdlg.DefaultExt
  End If
  cdlg.ShowSave
  If cdlg.filename <> "" Then
    CurrentFilename = cdlg.filename
    newFilename = cdlg.filename
    mnuSaveFile_Click
    If SelItm.VBItype = vbi_Form Or SelItm.VBItype = vbi_UserControl Then
      buf = Left(SelItm.path, Len(SelItm.path) - 1) & "x"
      If Len(Dir(buf)) > 0 Then
        FileCopy buf, PathNameOnly(newFilename)
      End If
    End If
    Select Case SelItm.VBItype
      Case vbi_Class, vbi_Form, vbi_Module, vbi_UserControl
        If MsgBox("Change the name in all open projects that include this file?", vbYesNo, "Save As...") = vbYes Then
          For Each searchItm In AllItems
            If searchItm.VBItype = vbi_Project Then
              buf = searchItm.Body
              buf2 = ReplaceString(buf, oldFilename, newFilename)
              If buf2 <> buf Then SaveFileString newFilename, buf2
            End If
          Next
        End If
        If MsgBox("Change the public name to match the new filename?", vbYesNo, "Save As...") = vbYes Then
          buf = SelItm.Body
          buf2 = ReplaceString(buf, FilenameOnly(oldFilename), FilenameOnly(newFilename))
          If buf2 <> buf Then SaveFileString newFilename, buf2
        End If
      Case vbi_Group
      Case vbi_Project
    End Select
    Set searchItm = SelItm
    While searchItm.VBItype <> vbi_Group
      Set nod = nod.parent
      If nod Is Nothing Then
        MsgBox "No group was found to search for related projects." & vbCr _
             & ""
        Exit Sub
      End If
      Set searchItm = ItemFromNodeKey(nod.Key)
      If searchItm.VBItype = vbi_Group Then RemoveProjectObjRef searchItm.path
    Wend
    RemoveAllObjRef SelItm
  End If
End Sub

Private Sub mnuSaveFile_Click()
  SaveFileString CurrentFilename, txtMain.text
  SetFileChanged False
End Sub

Private Function FullPathFromNodeKey(nodeKey As String) As String
  Dim FullPath$, EndOfPath As Long
  FullPath = nodeKey
  EndOfPath = InStr(FullPath, "|")
  If EndOfPath > 0 Then FullPath = Left(FullPath, EndOfPath - 1)
  FullPathFromNodeKey = FullPath
End Function

Private Function ItemFromNodeKey(nodeKey As String) As clsVBitem
  Set ItemFromNodeKey = AllItems(FullPathFromNodeKey(nodeKey))
End Function

'Private Sub mnuZip_Click()
'  Dim zf As ZipFile, filelist As String, savelist As Integer
'  Set zf = New ZipFile
'  AddFiles TopItem, zf, filelist
'  If MsgBox(filelist, vbYesNo, "Create this zip file?") = vbYes Then
'    cdlg.DefaultExt = "zip"
'    cdlg.Filter = "Zip files (*.zip)|*.zip|All files (*.*)|*.*"
'    cdlg.FilterIndex = 1
'    cdlg.ShowSave
'    If cdlg.filename <> "" Then
'      savelist = FreeFile
'      Open FileListFilename For Output As savelist
'      Print #savelist, filelist
'      Close savelist
'      zf.AddFile FileListFilename
'      zf.RootDirectory = CurDir
'      zf.ZipFilename = cdlg.filename
'      zf.MakeZipFile
'      Kill FileListFilename
'    End If
'  End If
'End Sub
'
'Private Sub AddFiles(vbi As clsVBitem, zf As ZipFile, ByRef filelist As String)
'  Dim child As Long, children As Long, thisName As String, thisKey As String
'  thisName = vbi.name
'  Select Case vbi.VBItype
'    Case vbi_Class, vbi_Form, vbi_Group, vbi_Module, vbi_Project, vbi_UserControl
'      If InStr(filelist, vbi.name & vbCrLf) = 0 Then
'        zf.AddFile vbi.name
'        filelist = filelist & vbi.name & vbCrLf
'      End If
'  End Select
'  children = vbi.nItems
'  For child = 1 To children
'    AddFiles vbi.item(child), zf, filelist
'  Next child
'End Sub

Private Sub mnuPack_Click()
  Dim tf As clsTar, filelist As String, savelist As Integer
  Set tf = New clsTar
  cdlg.filename = ""
  cdlg.DefaultExt = "tar"
  cdlg.Filter = "Tar files (*.tar)|*.tar|All files (*.*)|*.*"
  cdlg.FilterIndex = 1
  cdlg.ShowSave
  filelist = vbCrLf
  If cdlg.filename <> "" Then
    tf.TarFilename = cdlg.filename
    'Recursively add files in this group/project
    AddTarFiles TopItem, tf, filelist
    Debug.Print "Added: " & filelist
    'Add list of files as a new file
    'tf.AppendFile FileListFilename, filelist, Now
  End If
End Sub

Private Sub AddTarFiles(vbi As clsVBitem, tf As clsTar, ByRef filelist As String)
  Static Aborting As Boolean
  Dim child As Long, children As Long, thisName As String, thisKey As String, xFilename As String
  Dim i As Long, BinaryBytes() As Byte, BinaryString As String
  Aborting = False
  'thisName = vbi.Name
  thisName = vbi.path
  If UCase(Left(thisName, Len(path))) = UCase(path) Then
    thisName = Mid(thisName, Len(path) + 1)
  Else
    Select Case MsgBox("File in project is outside start path:" & vbCr & thisName & vbCr & "Include anyway?", vbYesNoCancel, "Packing Problem")
      Case vbYes: If Mid(thisName, 2, 1) = ":" Then thisName = Mid(thisName, 3)
      Case vbNo: Exit Sub
      Case vbCancel: Aborting = True: Exit Sub
    End Select
  End If
  If Left(thisName, 1) = "\" Then thisName = Mid(thisName, 2)
  Select Case vbi.VBItype
    Case vbi_Class, vbi_Form, vbi_Group, vbi_Module, vbi_Project, vbi_UserControl
      If InStr(filelist, vbCrLf & thisName & vbCrLf) = 0 Then
        tf.AppendFile thisName, WholeFileString(vbi.path), FileDateTime(vbi.path)
        filelist = filelist & thisName & vbCrLf
        If vbi.VBItype = vbi_UserControl Or vbi.VBItype = vbi_Form Then
          xFilename = Left(vbi.path, Len(vbi.path) - 1) & "x"
          If Len(Dir(xFilename)) > 0 Then
            BinaryBytes = WholeFileBytes(xFilename)
            BinaryString = ""
            For i = LBound(BinaryBytes) To UBound(BinaryBytes)
              BinaryString = BinaryString & Chr(BinaryBytes(i))
            Next
            tf.AppendFile Left(thisName, Len(thisName) - 1) & "x", BinaryString, FileDateTime(xFilename)
          End If
        End If
      End If
  End Select
  children = vbi.nItems
  For child = 1 To children
    AddTarFiles vbi.item(child), tf, filelist
    If Aborting Then Exit Sub
  Next child
End Sub

Private Sub mnuUndo_Click()
'    Case 26 'Control-Z = undo
  If UndosAvail > 0 Then
    Undoing = True
    UndoPos = UndoPos - 1
    If UndoPos < 0 Then UndoPos = MaxUndo
    txtMain.text = Undos(UndoPos)
    txtMain.SelStart = UndoCursor(UndoPos)
    UndosAvail = UndosAvail - 1
    Undoing = False
  End If
End Sub

Private Sub mnuUnpack_Click()
'  Dim txt As String, ext As String
'  txt = TarFile.ArchiveFilename(1)
'  ext = LCase(Right(txt, 3))
'
'  cdlg.DefaultExt = ext
'  cdlg.Filter = "Files|*." & ext & "|All Files (*.*)|*.*"
'  cdlg.FilterIndex = 1
'  cdlg.Filename = txt
'  cdlg.ShowSave
'  If Len(cdlg.Filename) > 0 Then
'    Path = PathNameOnly(cdlg.Filename)
'
'
'  End If
End Sub

Private Sub sash_MouseDown(Button As Integer, Shift As Integer, x As Single, y As Single)
  SashDragging = True
End Sub

Private Sub sash_MouseMove(Button As Integer, Shift As Integer, x As Single, y As Single)
  If SashDragging And (sash.Left + x) > 100 And (sash.Left + x < Width - 100) Then
    Dim newLeftWidth&
    sash.Left = sash.Left + x
    newLeftWidth = sash.Left
    If newLeftWidth > 1000 Then tree1.Width = newLeftWidth
    Form_Resize
  End If
End Sub

Private Sub sash_MouseUp(Button As Integer, Shift As Integer, x As Single, y As Single)
  SashDragging = False
End Sub

Private Sub Timer1_Timer()
  If IsNumeric(Timer1.tag) Then tree1.SelectedItem = tree1.Nodes(CInt(Timer1.tag))
  Timer1.Enabled = False
End Sub

Private Sub tree1_AfterLabelEdit(Cancel As Integer, NewString As String)
  With tree1.SelectedItem
    Select Case MsgBox("Rename file?", vbYesNoCancel)
      Case vbNo:
        .text = NewString
        .Key = .FullPath
      Case vbYes:
        Dim oldKey$
        oldKey = .Key
        .text = NewString
        .Key = .FullPath
        'Name Path & "\" & oldKey & SourceExtension As Path & "\" & .Key & SourceExtension
      Case vbCancel:
        Cancel = True
    End Select
  End With
End Sub

Private Sub tree1_KeyDown(KeyCode As Integer, Shift As Integer)
  Dim nod As Node
  Select Case KeyCode
    Case vbKeyDelete: tree1.Nodes.Remove tree1.SelectedItem.index
    'Case vbKeyInsert:
    '  Set nod = tree1.Nodes.add(tree1.SelectedItem, tvwPrevious, "NewFile", "NewFile")
    
  End Select
End Sub

'A horrible hack to get around the tree control's penchant for changing
'the selected node after we have lost control
Private Sub DelaySetNode(nodeNum&)
  Timer1.tag = nodeNum
  Timer1.Enabled = True
End Sub

Private Sub tree1_NodeClick(ByVal Node As ComctlLib.Node)
  Dim filename$
  Static inClick As Boolean
  Static LastNode As Long
  If Not inClick And Not Timer1.Enabled Then
    inClick = True
'    If NodeLinking > 0 Then
'      fullpath = PathNameOnly(tree1.Nodes(NodeLinking).Key)
'      Filename = RelativeFilename(Node.Key & SourceExtension, fullpath)
'      EditSubTag "href", Filename
'      DelaySetNode NodeLinking
'      NodeLinking = 0
'      Me.MousePointer = vbDefault
''    Else
    If QuerySave = vbCancel Then 'Should move focus back to old node here
      DelaySetNode 1
    Else
      filename = FullPathFromNodeKey(Node.Key)
      ReadFile filename, txtMain 'txtMain.Text = GetFileString(Filename)
      LastNode = tree1.SelectedItem.index
    End If
    inClick = False
  End If
End Sub

Private Sub ReadFile(filename$, txtBox As RichTextBox)
  Dim fileContents As String
  txtBox.text = "(no file)"
  On Error GoTo nofile
OpenFile:
  fileContents = GetFileString(filename)
  On Error GoTo 0
  If txtBox.Name = "txtMain" Then
    CurrentFilename = filename
    Caption = CurrentFilename
    CurrentFileContents = fileContents
    txtBox.textRTF = CurrentFileContents
    If Changed Then SetFileChanged False
  Else
    txtBox.text = fileContents
  End If
  Exit Sub
nofile:
  If MsgBox("File '" & filename & "' does not exist. Create it?", vbYesNo, "Missing file") = vbYes Then
    Dim f As Integer
    f = FreeFile
    Err.clear
    On Error GoTo errCreate
    Open filename For Output As #f
    Print #f, ""
    Close f
    GoTo OpenFile
  Else
    If txtBox.Name = "txtMain" Then
      CurrentFilename = filename
      Caption = CurrentFilename
    End If
    GoTo endsub
  End If
errCreate:
  MsgBox "Could not create file '" & filename & "'" & vbCr & Err.Description
endsub:
  SetFileChanged False
End Sub

Private Sub SetFileChanged(newValue As Boolean)
  If Changed <> newValue Then
    Changed = newValue
    mnuSaveFile.Enabled = Changed
    If Changed Then
      Caption = CurrentFilename & " (edited)"
    Else
      Caption = CurrentFilename
    End If
  End If
End Sub

Private Function FormatText(txtBox As RichTextBox)
  Dim rtf$
  Dim nextch&, maxch&
  Dim openTag&, closeTag&, parenlevel&, spacepos&
  rtf = txtBox.text
  
  rtf = ReplaceString(rtf, "<h", RTF_BOLD & "<h")
  rtf = ReplaceString(rtf, "</h", RTF_PLAIN & "</h")
  
  rtf = ReplaceString(rtf, "<u>", "<u>" & RTF_UNDERLINE)
  rtf = ReplaceString(rtf, "<b>", "<b>" & RTF_BOLD)
  rtf = ReplaceString(rtf, "<i>", "<i>" & RTF_ITALIC)
  
  rtf = ReplaceString(rtf, "</u>", RTF_PLAIN & "</u>")
  rtf = ReplaceString(rtf, "</b>", RTF_PLAIN & "</b>")
  rtf = ReplaceString(rtf, "</i>", RTF_PLAIN & "</i>")
  
  rtf = ReplaceString(rtf, vbCrLf, RTF_PARAGRAPH)
  
  If Len(txtFind.text) > 0 Then
    rtf = ReplaceString(rtf, txtFind.text, RTF_BOLD & "\cf1 " & txtFind.text & RTF_PLAIN & "\cf0 ")
  End If

  rtf = RTF_START & rtf & RTF_PARAGRAPH & RTF_END
    
  If rtf <> txtBox.textRTF Then
    Dim SelStart&, SelLength&
    SelStart = txtBox.SelStart
    SelLength = txtBox.SelLength
    txtBox.textRTF = rtf
    txtBox.SelStart = SelStart
    txtBox.SelLength = SelLength
  End If
End Function

Private Function QuerySaveProject() As VbMsgBoxResult
  QuerySaveProject = vbYes
'  If ProjectChanged Then
'    QuerySaveProject = MsgBox("Save changes to " & ProjectFileName & "?", vbYesNoCancel)
'    If QuerySaveProject = vbYes Then
'      SaveProject ProjectFileName, tree1
'    End If
'  End If
End Function

Private Function QuerySave() As VbMsgBoxResult
  QuerySave = vbYes
  If Changed Then
    If Not mnuAutoSave.Checked Then
      QuerySave = MsgBox("Save changes to " & CurrentFilename & "?", vbYesNoCancel)
    End If
    If QuerySave = vbYes Then mnuSaveFile_Click
  End If
End Function

Private Sub txtMain_Change()
  Static InChange As Boolean
  If Not InChange And Not Undoing Then
    InChange = True
    
    Undos(UndoPos) = txtMain.text
    UndoCursor(UndoPos) = txtMain.SelStart
    UndoPos = UndoPos + 1
    If UndoPos > MaxUndo Then UndoPos = 0
    If UndosAvail < MaxUndo Then UndosAvail = UndosAvail + 1
    
    If CurrentFileContents <> txtMain.text Then
      If Not Changed Then SetFileChanged True
    Else
      If Changed Then SetFileChanged False
    End If
    mnuSaveFile.Enabled = Changed
    If ViewFormatting Then FormatText txtMain
    InChange = False
  End If
End Sub

Private Sub txtMain_Click()
  Dim mnuItem&
  Dim subtagName$, filename$, PathName$
  Dim txt$
  txt = txtMain.text
  DoEvents
'  GetCurrentTag 'txt, txtMain.SelStart, tagName, openTagPos, closeTagPos
'  If openTagPos < closeTagPos Then
'    For mnuItem = mnuContext.Count - 1 To 1 Step -1
'      Unload mnuContext(mnuItem)
'    Next mnuItem
'    Select Case tagName
'      Case "img"
'        AddContextMenuItem CaptureReplace
'        AddContextMenuItem CaptureNew
'        AddContextMenuItem BrowseImage
'        filename = SubTagValue("src")
'        filename = ReplaceString(filename, "/", "\")
'        pathname = PathNameOnly(path & "\" & tree1.Nodes(tree1.SelectedItem.Index).key) & "\" & filename
'        If Len(Dir(pathname)) > 0 Then frmSample.SetImage pathname
'      Case "a"
'        Dim hashPos&
'        AddContextMenuItem SelectLink
'        filename = SubTagValue("href")
'        hashPos = InStr(filename, "#")
'        If hashPos > 0 Then filename = Left(filename, hashPos - 1)
'        filename = ReplaceString(filename, "/", "\")
'        pathname = PathNameOnly(path & "\" & tree1.Nodes(tree1.SelectedItem.Index).key) & "\" & filename
'        If Len(Dir(pathname)) > 0 Then frmSample.SetText pathname
'    End Select
'    'If txtMainButton = vbRightButton Then PopupMenu mnuContextTop
'  End If
End Sub

Private Sub txtMain_KeyPress(KeyAscii As Integer)
  Select Case KeyAscii
  End Select
End Sub

Private Sub txtMain_MouseDown(Button As Integer, Shift As Integer, x As Single, y As Single)
  'txtMainButton = Button
End Sub

'Search in string txt for a tag that encloses start character position
'Sets tagName to lowercase of first word in tag
'Sets openTagPos, closeTagPos to string index of < and > of tag in txt
Private Sub GetCurrentTag() 'txt$, start&, tagName$, openTagPos&, closeTagPos&)
  Dim txt$, start&
  txt = txtMain.text
  start = txtMain.SelStart
  If start < 1 Then Exit Sub
  openTagPos = InStrRev(txt, "<", start)
  If openTagPos > 0 Then
    closeTagPos = InStrRev(txt, ">", start)
    If closeTagPos < openTagPos Then 'we are in a tag
      closeTagPos = InStr(start, txt, ">")
    End If
  End If
  If openTagPos > 0 And openTagPos <= start And closeTagPos >= start Then
    Dim endNamePos&
    endNamePos = InStr(openTagPos, txt, " ")
    If endNamePos = 0 Or endNamePos > closeTagPos Then endNamePos = closeTagPos
    tagName = LCase(Mid(txt, openTagPos + 1, endNamePos - openTagPos - 1))
  Else
    openTagPos = 0
    closeTagPos = 0
    tagName = ""
  End If
End Sub

'Uses current tag delimited by openTagPos and closeTagPos
'Sets subtagName$, value$
'QuotedStringFromTag( st, v, 1 ) when the current tag is <img src="foo.png">
'will result in st="src", v="foo.png"
'Private Sub QuotedStringFromTag(subtagName$, value$, Optional stringNum& = 1)
'  Dim valueStart&, valueEnd&, subtagStart&, num&
'  Dim txt$
'  txt = txtMain.Text
'
'  valueStart = InStr(openTagPos, txt, """") + 1
'  num = 1
'  While num < stringNum And valueStart > 0
'    valueStart = InStr(valueStart, txt, """") + 1 'find close quote
'    If valueStart > 0 Then
'      valueStart = InStr(valueStart, txt, """") + 1 'find next open quote
'    End If
'    num = num + 1
'  Wend
'  valueEnd = InStr(valueStart, txt, """")
'  If valueStart > 0 And valueEnd > valueStart And valueEnd < closeTagPos Then
'    value = Mid(txt, valueStart, valueEnd - valueStart)
'    subtagStart = InStrRev(txt, " ", valueStart)
'    If subtagStart < openTagPos Then subtagStart = openTagPos
'    subtagName = Mid(txt, subtagStart + 1, valueStart - subtagStart - 3)
'  Else
'    subtagName = ""
'    value = ""
'  End If
'
'End Sub

'Uses current tag delimited by openTagPos and closeTagPos
'If subtagName does not exist in the current tag, "" is returned.
'SubTagValue( "src" ) when the current tag is <img src="foo.png">
'will return foo.png
Private Function SubTagValue(subtagName$) As String
  Dim valueStart&, valueEnd&, subtagStart&, SelStart&, retval$
  Dim txt$, tag$
  txt = txtMain.text
  SelStart = txtMain.SelStart
  tag = LCase(Mid(txt, openTagPos, closeTagPos - openTagPos + 1))
  subtagStart = InStr(1, tag, LCase(subtagName))
  If subtagStart = 0 Then
    retval = ""
  Else
    valueStart = subtagStart + Len(subtagName) + 1
    If Mid(tag, valueStart, 1) = """" Then
      valueStart = valueStart + 1
      valueEnd = InStr(valueStart, tag, """")
    Else
      valueEnd = InStr(valueStart + 1, tag, " ")
      If valueEnd = 0 Then valueEnd = Len(tag)
    End If
    retval = Mid(tag, valueStart, valueEnd - valueStart)
  End If
  SubTagValue = retval
End Function

'Uses current tag delimited by openTagPos and closeTagPos
'Modifies txtMain.Text, replacing current value of subtagName with NewValue
'If subtagName does not exist in the current tag, it is added at the end
'EditSubTag( "src", "bar.gif" ) when the current tag is <img src="foo.png">
'will result in <img src="bar.gif">
Private Sub EditSubTag(subtagName$, newValue$)
  Dim valueStart&, valueEnd&, subtagStart&
  Dim txt$, tag$
  txt = txtMain.text
  tag = LCase(Mid(txt, openTagPos, closeTagPos - openTagPos + 1))
  subtagStart = InStr(1, tag, LCase(subtagName))
  If subtagStart = 0 Then
    txtMain.text = Left(txt, closeTagPos - 1) & " " & LCase(subtagName) & "=" & newValue & Mid(txt, closeTagPos)
  Else
    'subtagStart = subtagStart + openTagPos
    valueStart = subtagStart + Len(subtagName) + 1
    If Mid(tag, valueStart, 1) = """" Then
      valueEnd = InStr(valueStart + 1, tag, """")
    Else
      valueEnd = InStr(valueStart + 1, tag, " ")
      If valueEnd = 0 Then valueEnd = Len(tag)
    End If
    txtMain.text = Left(txt, openTagPos + valueStart - 1) & newValue & Mid(txt, openTagPos + valueEnd - 1)
    closeTagPos = InStr(openTagPos + 1, txtMain.text, ">")
  End If
  txtMain.SelStart = openTagPos + 1
  txtMain_Click
  txtMain.SelStart = closeTagPos + 1
End Sub

Private Sub AddContextMenuItem(newItem$)
'  Dim mnuItem&
'  mnuItem = mnuContext.Count
'  Load mnuContext(mnuItem)
'  mnuContext(mnuItem).Caption = newItem
End Sub

Private Sub txtMain_SelChange()
  Dim lastSelStart&
  If txtMain.SelStart <> lastSelStart Then
    
    lastSelStart = txtMain.SelStart
  End If
End Sub

Private Sub AddRecentFile(FilePath As String)
  Dim rf&, rfMove&, newPath$, match As Boolean
  rf = 0
  While Not match And rf <= mnuRecent.Count - 2
    rf = rf + 1
    If UCase(mnuRecent(rf).tag) = UCase(FilePath) Then match = True
  Wend
  If match Then 'move file to top of list
    For rfMove = rf To 2 Step -1
      mnuRecent(rfMove).tag = mnuRecent(rfMove - 1).tag
      mnuRecent(rfMove).Caption = "&" & rfMove & " " & FilenameOnly(mnuRecent(rfMove).tag)
    Next rfMove
  Else 'Add file to list
    mnuRecent(0).Visible = True
    If mnuRecent.Count <= MaxRecentFiles Then Load mnuRecent(mnuRecent.Count)
    For rfMove = mnuRecent.Count - 1 To 2 Step -1
      mnuRecent(rfMove).tag = mnuRecent(rfMove - 1).tag
      mnuRecent(rfMove).Caption = "&" & rfMove & " " & FilenameOnly(mnuRecent(rfMove).tag)
    Next rfMove
  End If
  mnuRecent(1).Visible = True
  mnuRecent(1).tag = FilePath
  mnuRecent(1).Caption = "&1 " & FilenameOnly(mnuRecent(rfMove).tag)
End Sub


Private Sub SaveWindowSettings()
  Dim rf&
  If Height > 800 And Left < Screen.Width And Top < Screen.Height Then
    SaveSetting App.Title, SectionMainWin, "Width", Width
    SaveSetting App.Title, SectionMainWin, "Height", Height
    SaveSetting App.Title, SectionMainWin, "Left", Left
    SaveSetting App.Title, SectionMainWin, "Top", Top
    SaveSetting App.Title, SectionMainWin, "MapWidth", sash.Left
  End If
  For rf = mnuRecent.Count - 1 To 1 Step -1
    SaveSetting App.Title, SectionRecentFiles, CStr(rf), mnuRecent(rf).tag
  Next rf
  While GetSetting(App.Title, SectionRecentFiles, CStr(rf), "") <> ""
    DeleteSetting App.Title, SectionRecentFiles, CStr(rf)
    rf = rf + 1
  Wend
End Sub

Private Sub RetrieveWindowSettings()
  Dim setting As Variant, rf&
  setting = GetSetting(App.Title, SectionMainWin, "Left")
  If IsNumeric(setting) Then
    If setting < Screen.Width Then Left = setting
  End If
  setting = GetSetting(App.Title, SectionMainWin, "Top")
  If IsNumeric(setting) Then
    If setting >= 0 And setting < Screen.Height * 0.9 Then Top = setting
  End If
  setting = GetSetting(App.Title, SectionMainWin, "Width")
  If IsNumeric(setting) Then
    If setting > 200 And setting <= Screen.Width Then Width = setting
  End If
  setting = GetSetting(App.Title, SectionMainWin, "Height")
  If IsNumeric(setting) Then
    If setting > 200 And setting <= Screen.Height Then Height = setting
  End If
  setting = GetSetting(App.Title, SectionMainWin, "MapWidth")
  If IsNumeric(setting) Then
    If setting > 50 And setting < Me.ScaleWidth * 0.9 Then sash.Left = setting
  End If
  SashDragging = True
  sash_MouseMove 1, 0, 0, 0
  SashDragging = False
  For rf = MaxRecentFiles To 1 Step -1
    setting = GetSetting(App.Title, SectionRecentFiles, CStr(rf))
    If setting <> "" Then AddRecentFile CStr(setting)
  Next rf
End Sub

