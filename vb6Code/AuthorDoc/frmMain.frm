VERSION 5.00
Object = "{6B7E6392-850A-101B-AFC0-4210102A8DA7}#1.3#0"; "COMCTL32.OCX"
Object = "{F9043C88-F6F2-101A-A3C9-08002B2F49FB}#1.2#0"; "COMDLG32.OCX"
Object = "{3B7C8863-D78F-101B-B9B5-04021C009402}#1.2#0"; "RICHTX32.OCX"
Begin VB.Form frmMain 
   Caption         =   "AuthorDoc"
   ClientHeight    =   5700
   ClientLeft      =   165
   ClientTop       =   735
   ClientWidth     =   8865
   Icon            =   "frmMain.frx":0000
   LinkTopic       =   "Form1"
   ScaleHeight     =   5700
   ScaleWidth      =   8865
   StartUpPosition =   3  'Windows Default
   Begin VB.Timer TimerSlowAction 
      Enabled         =   0   'False
      Interval        =   1000
      Left            =   0
      Top             =   0
   End
   Begin VB.Frame fraFind 
      BorderStyle     =   0  'None
      Caption         =   "Frame1"
      Height          =   492
      Left            =   1920
      TabIndex        =   3
      Top             =   0
      Width           =   4692
      Begin VB.TextBox txtFind 
         Height          =   288
         Left            =   840
         TabIndex        =   7
         Top             =   120
         Width           =   1332
      End
      Begin VB.TextBox txtReplace 
         Height          =   288
         Left            =   3360
         TabIndex        =   6
         Top             =   120
         Width           =   1332
      End
      Begin VB.CommandButton cmdFind 
         Caption         =   "Find:"
         Height          =   252
         Left            =   0
         TabIndex        =   5
         Top             =   120
         Width           =   732
      End
      Begin VB.CommandButton cmdReplace 
         Caption         =   "Replace:"
         Height          =   252
         Left            =   2280
         TabIndex        =   4
         Top             =   120
         Width           =   972
      End
   End
   Begin VB.Timer Timer1 
      Enabled         =   0   'False
      Interval        =   100
      Left            =   120
      Top             =   1440
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
      TextRTF         =   $"frmMain.frx":0442
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
      Appearance      =   1
   End
   Begin MSComDlg.CommonDialog cdlgImage 
      Left            =   0
      Top             =   0
      _ExtentX        =   847
      _ExtentY        =   847
      _Version        =   393216
   End
   Begin VB.Menu mnuTop 
      Caption         =   "&File"
      Index           =   0
      Begin VB.Menu mnuOpenProject 
         Caption         =   "&Open Project"
      End
      Begin VB.Menu mnuSaveProject 
         Caption         =   "Save Project As"
      End
      Begin VB.Menu mnuNewProject 
         Caption         =   "New Project"
      End
      Begin VB.Menu sep1 
         Caption         =   "-"
      End
      Begin VB.Menu mnuNewSection 
         Caption         =   "&New Section"
      End
      Begin VB.Menu mnuSaveFile 
         Caption         =   "&Save Section"
         Enabled         =   0   'False
      End
      Begin VB.Menu mnuRevert 
         Caption         =   "&Revert to Saved"
      End
      Begin VB.Menu mnuAutoSave 
         Caption         =   "&Auto-Save"
      End
      Begin VB.Menu sep2 
         Caption         =   "-"
      End
      Begin VB.Menu mnuConvert 
         Caption         =   "&Convert"
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
   End
   Begin VB.Menu mnuTop 
      Caption         =   "&Tags"
      Index           =   2
      Begin VB.Menu mnuUnderline 
         Caption         =   "&Underline <u>...</u>"
      End
      Begin VB.Menu mnuBold 
         Caption         =   "&Bold <b>...</b>"
      End
      Begin VB.Menu mnuItalic 
         Caption         =   "&Italic <i>...</i>"
         Shortcut        =   ^I
      End
      Begin VB.Menu mnuLink 
         Caption         =   "&Link <a href=""..."">...</a>"
         Shortcut        =   ^L
      End
      Begin VB.Menu mnuLinkSection 
         Caption         =   "Link &Section"
      End
      Begin VB.Menu mnuImage 
         Caption         =   "I&mage <img src=""..."">"
         Shortcut        =   ^M
      End
      Begin VB.Menu mnuIndexword 
         Caption         =   "Inde&x word <indexword=...>"
      End
      Begin VB.Menu mnuKeyword 
         Caption         =   "&Keyword <keyword=...>"
      End
      Begin VB.Menu mnuOL 
         Caption         =   "&Numbered List <ol><li>...</ol>"
      End
      Begin VB.Menu mnuUL 
         Caption         =   "Bulle&ts <ul><li>...</ul>"
      End
      Begin VB.Menu mnuPRE 
         Caption         =   "&Preformatted <pre>...</pre>"
         Shortcut        =   ^P
      End
      Begin VB.Menu mnuFigure 
         Caption         =   "&Figure <figure>...</figure>"
      End
      Begin VB.Menu sep4 
         Caption         =   "-"
      End
      Begin VB.Menu mnuAutoParagraph 
         Caption         =   "Automatic Paragraphs <p>"
         Checked         =   -1  'True
      End
   End
   Begin VB.Menu mnuTop 
      Caption         =   "&View"
      Index           =   3
      Begin VB.Menu mnuFormatting 
         Caption         =   "&Formatting"
         Checked         =   -1  'True
      End
      Begin VB.Menu mnuFormatWhileTyping 
         Caption         =   "Format While Typing"
      End
      Begin VB.Menu mnuOptions 
         Caption         =   "&Options"
      End
      Begin VB.Menu mnuTextImage 
         Caption         =   "Test TextImage"
      End
   End
   Begin VB.Menu mnuContextTop 
      Caption         =   "Context"
      Begin VB.Menu mnuContext 
         Caption         =   "Delete"
         Index           =   0
      End
   End
   Begin VB.Menu mnuTopHelp 
      Caption         =   "&Help"
      Begin VB.Menu mnuHelpContents 
         Caption         =   "&Contents"
      End
      Begin VB.Menu mnuHelpAbout 
         Caption         =   "&About"
      End
      Begin VB.Menu mnuHelpWebsite 
         Caption         =   "&Web Site"
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
Dim CurrentFileContents$                'What was last saved or retrieved from CurrentFilename
Dim Undos$()
Dim UndoCursor&()
Dim UndoPos&
Dim UndosAvail&
Dim MaxUndo&
Dim Undoing As Boolean
Dim Changed As Boolean                  'True if txtMain.Text has been edited
Dim ProjectChanged As Boolean
Dim ViewFormatting As Boolean
Dim FormatWhileTyping As Boolean
Dim txtMainButton As Long
Dim AbortAction As Boolean

Dim tagName$, openTagPos&, closeTagPos& 'current tag being edited
Dim NodeLinking&                        'Index in tree of file containing link being edited

Private SashDragging As Boolean
Private Const SectionMainWin = "Main Window"
Private Const SectionRecentFiles = "Recent Files"
Private Const MaxRecentFiles = 6

Private Sub cmdFind_KeyPress(KeyAscii As Integer)
  If KeyAscii >= 32 And KeyAscii < 127 Then
    txtFind.SetFocus
    txtFind.Text = Chr(KeyAscii)
    txtFind.selStart = 1
  End If
End Sub

Private Sub cmdFind_MouseUp(Button As Integer, Shift As Integer, x As Single, y As Single)
  Static Finding As Boolean
  If Button = vbRightButton Then
    fraFind.Visible = False
    Form_Resize
  ElseIf cmdFind.caption = "Stop" Then
    Finding = False
  Else
    Dim searchThrough$, SearchFor$, selStart&, searchPos&, startNodeIndex&
    'Dim StartTime As Single
    Finding = True
    cmdFind.caption = "Stop"
    'StartTime = Timer
    searchThrough = txtMain.Text
    If txtFind.Text = "" And txtMain.SelLength > 0 Then txtFind.Text = txtMain.SelText
    If txtFind.Text <> "" Then
      SearchFor = UnEscape(txtFind.Text)
      selStart = txtMain.selStart
      searchPos = txtMain.selStart + txtMain.SelLength
      searchPos = txtMain.Find(SearchFor, searchPos)
      startNodeIndex = tree1.SelectedItem.Index
      If searchPos < 0 And Finding Then
        If QuerySave <> vbCancel Then
NextNode:
          If tree1.SelectedItem Is Nothing Then
            tree1_NodeClick tree1.Nodes(1)
          ElseIf tree1.SelectedItem.Index < tree1.Nodes.Count Then
            tree1_NodeClick tree1.Nodes(tree1.SelectedItem.Index + 1)
          Else
            tree1_NodeClick tree1.Nodes(1)
          End If
          searchPos = txtMain.Find(SearchFor, 0)
          If searchPos < 0 And tree1.SelectedItem.Index <> startNodeIndex Then
            'If Timer - StartTime < FindTimeout Then
            DoEvents
            If Finding Then GoTo NextNode
          End If
        End If
      End If
    End If
  End If
  cmdFind.caption = "Find"
End Sub

Private Function UnEscape(ByVal Source As String) As String
  Dim retval As String
  Dim ch As String
  Dim chpos As Long, lastchpos As Long
  chpos = 1
  lastchpos = Len(Source)
  While chpos <= lastchpos
    ch = Mid(Source, chpos, 1)
    If ch = "\" Then
      chpos = chpos + 1
      If chpos > lastchpos Then
        retval = retval & ch
      Else
        ch = Mid(Source, chpos, 1)
        Select Case LCase(ch)
          Case "c": retval = retval & vbCrLf
          Case "n": retval = retval & vbLf
          Case "r": retval = retval & vbCr
          Case "t": retval = retval & vbTab
          Case "\": retval = retval & ch
          Case Else: retval = retval & "^" & ch
        End Select
      End If
    Else
      retval = retval & ch
    End If
    chpos = chpos + 1
  Wend
  UnEscape = retval
End Function

Private Sub cmdReplace_KeyPress(KeyAscii As Integer)
  If KeyAscii >= 32 And KeyAscii < 127 Then
    txtReplace.SetFocus
    txtReplace.Text = Chr(KeyAscii)
    txtReplace.selStart = 1
  End If
End Sub

Private Sub cmdReplace_MouseUp(Button As Integer, Shift As Integer, x As Single, y As Single)
  Dim startNodeIndex&, searchedBeyondStart As Boolean
  Dim FindText As String, ReplaceText As String
  If Button = vbRightButton Then
    fraFind.Visible = False
    Form_Resize
  Else
    FindText = LCase(UnEscape(txtFind.Text))
    ReplaceText = UnEscape(txtReplace.Text)
    startNodeIndex = tree1.SelectedItem.Index
    searchedBeyondStart = False
    If LCase(txtMain.SelText) = FindText Then
NextReplace:
      txtMain.SelText = ReplaceText
    End If
    cmdFind_MouseUp Button, Shift, x, y
    If startNodeIndex <> tree1.SelectedItem.Index Then searchedBeyondStart = True
    If Shift > 0 Then
      If Not searchedBeyondStart Or startNodeIndex <> tree1.SelectedItem.Index Then
        If LCase(txtMain.SelText) = FindText Then GoTo NextReplace
      End If
    End If
  End If
End Sub

Private Sub Form_Load()
  Dim setting As Variant, rf As Long
  MaxUndo = 10
  ReDim Undos(MaxUndo)
  ReDim UndoCursor(MaxUndo)
  CaptureNew = "Capture New Image"
  CaptureReplace = "Capture Replacement Image"
  BrowseImage = "Use Other Image (File)"
  ViewImage = "View image"
  SelectLink = "Link to Page (select)"
  DeleteTag = "Delete"
  mnuContext(0).caption = DeleteTag
  txtMain.Text = ""
  
  App.HelpFile = GetSetting(App.Title, "Files", "Help", App.path & "\AuthorDoc.chm")
  BaseName = GetSetting(App.Title, "Defaults", "BaseName", "")
  path = GetSetting(App.Title, "Defaults", "Path", CurDir)
  ViewFormatting = GetSetting(App.Title, "Defaults", "ViewFormatting", True)
  FormatWhileTyping = GetSetting(App.Title, "Defaults", "FormatWhileTyping", False)
  mnuAutoParagraph.Checked = GetSetting(App.Title, "Defaults", "AutoParagraph", False)
  setting = GetSetting(App.Title, "Defaults", "FindTimeout", 2): If IsNumeric(setting) Then FindTimeout = setting
  setting = GetSetting(App.Title, SectionMainWin, "Width"):      If IsNumeric(setting) Then Width = setting
  setting = GetSetting(App.Title, SectionMainWin, "Height"):     If IsNumeric(setting) Then Height = setting
  setting = GetSetting(App.Title, SectionMainWin, "Left"):       If IsNumeric(setting) Then Left = setting
  setting = GetSetting(App.Title, SectionMainWin, "Top"):        If IsNumeric(setting) Then Top = setting
  setting = GetSetting(App.Title, SectionMainWin, "TreeWidth")
  If IsNumeric(setting) Then
    sash.Left = setting
    SashDragging = True
    sash_MouseMove 1, 0, 0, 0
    SashDragging = False
  End If
  For rf = MaxRecentFiles To 1 Step -1
    setting = GetSetting(App.Title, SectionRecentFiles, rf, "")
    If setting <> "" Then AddRecentFile CStr(setting)
  Next rf
  
  mnuFormatting.Checked = ViewFormatting
  mnuFormatWhileTyping.Checked = FormatWhileTyping
  cdlg.filename = path & "\" & BaseName & SourceExtension
  cdlgImage.filename = path
  If Dir(path & "\") <> "" Then ChDir path
  If Len(Dir(cdlg.filename)) > 0 Then
    Me.Show
    Me.MousePointer = vbHourglass
    OpenProject cdlg.filename, tree1
    If tree1.Nodes.Count > 0 Then tree1_NodeClick tree1.Nodes(1)
    Me.MousePointer = vbDefault
  End If
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
  Dim rf As Long
  If QuerySave = vbCancel Then
    Cancel = 1
  ElseIf QuerySaveProject = vbCancel Then
    Cancel = 1
  Else
    SaveSetting App.Title, "Files", "Help", App.HelpFile
    
    SaveSetting App.Title, "Defaults", "BaseName", BaseName
    SaveSetting App.Title, "Defaults", "Path", path
    SaveSetting App.Title, "Defaults", "FindTimeout", FindTimeout
    SaveSetting App.Title, "Defaults", "ViewFormatting", ViewFormatting
    SaveSetting App.Title, "Defaults", "FormatWhileTyping", FormatWhileTyping
    SaveSetting App.Title, "Defaults", "AutoParagraph", mnuAutoParagraph.Checked

    SaveSetting App.Title, SectionMainWin, "Width", Width
    SaveSetting App.Title, SectionMainWin, "Height", Height
    SaveSetting App.Title, SectionMainWin, "Left", Left
    SaveSetting App.Title, SectionMainWin, "Top", Top
    SaveSetting App.Title, SectionMainWin, "TreeWidth", sash.Left
    For rf = mnuRecent.Count - 1 To 1 Step -1
      SaveSetting App.Title, SectionRecentFiles, CStr(rf), mnuRecent(rf).tag
    Next rf
    While GetSetting(App.Title, SectionRecentFiles, CStr(rf)) <> ""
      SaveSetting App.Title, SectionRecentFiles, CStr(rf), ""
      rf = rf + 1
    Wend

    Dim frm As Object
    For Each frm In Forms
      Unload frm
    Next frm
    End
  End If
End Sub

Private Sub fraFind_MouseDown(Button As Integer, Shift As Integer, x As Single, y As Single)
  If Button = vbRightButton Or Shift = vbKeyShift Then
    fraFind.Visible = False
    Form_Resize
  End If
End Sub

Private Sub mnuAutoSave_Click()
  mnuAutoSave.Checked = Not mnuAutoSave.Checked
End Sub

Private Sub mnuContext_Click(Index As Integer)
  ContextAction mnuContext(Index).caption
End Sub

Public Sub ContextAction(cmd$)
  Dim filename$, PathName$
  Select Case cmd
    Case CaptureReplace
      filename = ReplaceString(SubTagValue("src"), "/", "\")
      filename = PathNameOnly(path & "\" & NodeFile) & "\" & filename
      frmCapture.filename = filename
      frmCapture.Show
    Case CaptureNew, BrowseImage
      cdlg.ShowOpen
      filename = cdlg.filename
      If Len(filename) > 0 Then
        PathName = PathNameOnly(path & "\" & NodeFile)
        filename = HTMLRelativeFilename(filename, PathName)
      End If
      If closeTagPos > openTagPos + 4 Then
        EditSubTag "src", filename
      Else
        txtMain.Text = Left(txtMain.Text, txtMain.selStart) & "<img src=""" & filename & """>" & Mid(txtMain.Text, txtMain.selStart + 1)
      End If
      If cmd = CaptureNew Then
        frmCapture.filename = filename
        frmCapture.Show
      End If
    Case ViewImage
      filename = ReplaceString(SubTagValue("src"), "/", "\")
      filename = PathNameOnly(path & "\" & NodeFile) & "\" & filename
      If Len(Dir(filename)) > 0 Then OpenFile filename
    Case DeleteTag
      If closeTagPos > openTagPos + 4 Then txtMain.Text = Left(txtMain.Text, openTagPos - 1) & Mid(txtMain.Text, closeTagPos + 1)
    Case SelectLink
      NodeLinking = tree1.SelectedItem.Index
      Me.MousePointer = vbUpArrow
    Case Else: MsgBox "Unrecognized menu item: " & cmd, vbOKOnly, "AuthorDoc"
  End Select
End Sub

Private Sub mnuConvert_Click()
  If QuerySave <> vbCancel Then
    If QuerySaveProject <> vbCancel Then frmConvert.Show
  End If
End Sub

Private Sub mnuCopy_Click()
  Clipboard.SetText txtMain.SelText
End Sub

Private Sub mnuCut_Click()
  Clipboard.SetText txtMain.SelText
  txtMain.SelText = ""
End Sub

Private Sub mnuEditProject_Click()
  If tree1.Visible Then
    If QuerySaveProject <> vbCancel Then
      LoadTextboxFromFile PathNameOnly(ProjectFileName), _
                          FilenameOnly(ProjectFileName), _
                          "." & FileExt(ProjectFileName), txtMain
      tree1.Visible = False
    End If
  Else
    If QuerySave <> vbCancel Then
      tree1.Visible = True
      mnuRecent_Click 1
    End If
  End If
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
    Dim selStart As Long, SelEnd As Long, txtLen As Long
    txtLen = Len(txtMain.Text)
    SelEnd = txtMain.selStart
    selStart = txtMain.selStart
    Do While selStart > 0
      If IsAlphaNumeric(Mid(txtMain.Text, selStart, 1)) Then
        selStart = selStart - 1
      Else
        Exit Do
      End If
    Loop
    Do While SelEnd <= txtLen
      If IsAlphaNumeric(Mid(txtMain.Text, SelEnd + 1, 1)) Then
        SelEnd = SelEnd + 1
      Else
        Exit Do
      End If
    Loop
    txtMain.selStart = selStart
    txtMain.SelLength = SelEnd - selStart
  End If
  txtFind.Text = txtMain.SelText
  cmdFind.SetFocus
End Sub

Private Sub mnuFormatting_Click()
  mnuFormatting.Checked = Not mnuFormatting.Checked
  ViewFormatting = mnuFormatting.Checked
  If ViewFormatting Then
    FormatText txtMain
  Else
    txtMain.Text = txtMain.Text
    txtMain.Refresh
  End If
End Sub

Private Sub mnuFormatWhileTyping_Click()
  mnuFormatWhileTyping.Checked = Not mnuFormatWhileTyping.Checked
  FormatWhileTyping = mnuFormatWhileTyping.Checked
  If FormatWhileTyping Then
    If Not ViewFormatting Then
      mnuFormatting_Click
    Else
      FormatText txtMain
    End If
  End If
End Sub

Private Sub mnuHelpAbout_Click()
  MsgBox "AuthorDoc" & vbCr & "Version " & App.Major & "." & App.Minor & vbCr & "Aqua Terra Consultants", vbOKOnly, "About AuthorDoc"
End Sub

Private Sub mnuHelpContents_Click()
  Dim newHelpfile As String
  newHelpfile = OpenFile(App.HelpFile, cdlg)
  If newHelpfile <> App.HelpFile Then
    If Len(Dir(newHelpfile)) > 0 Then
      App.HelpFile = newHelpfile
      SaveSetting App.Title, "Files", "Help", App.HelpFile
    End If
  End If
End Sub

Private Sub mnuHelpWebsite_Click()
  ShellExecute Me.hwnd, "Open", "http://hspf.com/pub/authordoc", 0&, 0&, 0&
End Sub

Private Sub mnuLinkSection_Click()
  GetCurrentTag
  If tagName = "a" Then
    txtMain.selStart = openTagPos + 9
  Else
    mnuLink_Click
    GetCurrentTag
  End If
  ContextAction SelectLink
End Sub

Private Sub mnuNewProject_Click()
  Dim f%
  
  If QuerySave = vbCancel Then Exit Sub
  On Error GoTo ErrNew
  cdlg.ShowSave
  If Len(cdlg.filename) > 0 Then
    path = PathNameOnly(cdlg.filename)
    ChDir path
    If Len(Dir(path, vbDirectory)) < 1 Then MkDir path
    f = FreeFile(0)
    Open cdlg.filename For Output As #f
    Close #f
    OpenProject cdlg.filename, tree1
    mnuNewSection.Enabled = True
    ProjectChanged = False
    If tree1.Nodes.Count > 0 Then tree1_NodeClick tree1.Nodes(1)
  End If
  Exit Sub
ErrNew:
  MsgBox "Error creating new project:" & vbCr & Err.Description
End Sub

Private Sub mnuNewSection_Click()
  Dim found As Boolean, nodNum&
  Dim ThisName$, key$, keypath$
  Dim filename As String
  Dim f%
  
  cdlg.ShowOpen
  filename = cdlg.filename
  If Len(filename) > Len(path) Then
    If UCase(Left(filename, Len(path))) <> UCase(path) Then
      MsgBox "Files must be in the same directory as or a subdirectory of the project file's directory.", vbOKOnly
    Else
      If UCase(Right(filename, Len(SourceExtension))) <> UCase(SourceExtension) Then
        filename = filename & SourceExtension
      End If

      If Len(Dir(filename)) = 0 Then
        keypath = PathNameOnly(filename)
        If Len(Dir(keypath, vbDirectory)) = 0 Then MkDir keypath
        f = FreeFile(0)
        Open filename For Output As #f
        Close f
      End If
      
      ThisName = FilenameOnly(filename)
      key = Left(filename, Len(filename) - Len(SourceExtension))  'trim extension .txt
      key = "N" & Mid(key, Len(path) + 2)
      keypath = PathNameOnly(Mid(key, 2))
      If tree1.Nodes.Count = 0 Then 'This is the first node
        tree1.Nodes.Add , , key, ThisName
      ElseIf keypath = PathNameOnly(NodeFile) Then 'place after selected sibling
        tree1.Nodes.Add tree1.SelectedItem, tvwNext, key, ThisName
      Else
        nodNum = tree1.Nodes.Count
        found = False
        While nodNum >= 1 And Not found 'Look for last sibling
          If PathNameOnly(NodeFile(nodNum)) = keypath Then
            tree1.Nodes.Add tree1.Nodes(nodNum).key, tvwNext, key, ThisName
            found = True
          End If
          nodNum = nodNum - 1
        Wend
        If Not found Then tree1.Nodes.Add tree1.SelectedItem, tvwChild, key, ThisName
      End If
      CurrentFilename = cdlg.filename
      ProjectChanged = True
      tree1.Nodes(key).EnsureVisible
    End If
  End If
End Sub

Private Function NodeFile(Optional nodNum As Long) As String
  If IsMissing(nodNum) Then nodNum = tree1.SelectedItem.Index
  If nodNum = 0 Then nodNum = tree1.SelectedItem.Index
  If nodNum < 1 Then nodNum = 1
  NodeFile = Mid(tree1.Nodes(nodNum).key, 2)
End Function

Private Sub mnuOpenProject_Click()
  If Len(path) > 0 And Len(Dir(path, vbDirectory)) > 0 Then ChDir path
  
  cdlg.filename = "" 'BaseName
  cdlg.ShowOpen
  If Len(cdlg.filename) > 0 Then
    AddRecentFile cdlg.filename
    mnuRecent_Click 1
  End If
End Sub

Private Sub mnuOptions_Click()
  frmOptions.Show
End Sub

Private Sub mnuPaste_Click()
  txtMain.SelText = Clipboard.GetText
End Sub

Private Sub mnuRecent_Click(Index As Integer)
  Dim newFilePath$
  If Index > 0 Then
    If QuerySaveProject <> vbCancel Then
      If QuerySave <> vbCancel Then
        newFilePath = mnuRecent(Index).tag
        path = PathNameOnly(newFilePath)
        Me.MousePointer = vbHourglass
        OpenProject newFilePath, tree1
        mnuNewSection.Enabled = True
        ProjectChanged = False
        If tree1.Nodes.Count > 0 Then tree1_NodeClick tree1.Nodes(1)
        Me.MousePointer = vbDefault
      End If
    End If
  End If
End Sub

Private Sub mnuRevert_Click()
  txtMain.Text = CurrentFileContents
End Sub

Private Sub mnuSaveFile_Click()
  Dim f%        'file handle
  Dim FileLength&
  
  f = FreeFile(0)
  'Kill CurrentFilename
  Open CurrentFilename For Output As #f
  Print #f, txtMain.Text
  Close f
  SetFileChanged False
  If CurrentFilename = ProjectFileName Then
    Me.MousePointer = vbHourglass
    OpenProject ProjectFileName, tree1
    Me.MousePointer = vbDefault
  End If
End Sub

Private Sub mnuSaveProject_Click()
  cdlg.filename = ProjectFileName
  cdlg.ShowSave
  If Len(cdlg.filename) > 0 Then SaveProject cdlg.filename, tree1
End Sub

Private Sub mnuImage_Click()
  Dim startPos&
  Dim filename$, PathName$
  startPos = txtMain.selStart
  txtMain.SelLength = 0
  txtMain.SelText = "<img src="""">"
  txtMain.selStart = startPos + 10
  cdlgImage.ShowOpen
  filename = cdlgImage.filename
  If Len(filename) > 0 Then
    PathName = PathNameOnly(path & "\" & NodeFile)
    filename = HTMLRelativeFilename(filename, PathName)
  End If
  txtMain.SelText = filename
End Sub

Private Sub mnuBold_Click()
  InsertTag "b"
End Sub

Private Sub mnuFigure_Click()
 InsertTag "figure"
End Sub

Private Sub mnuItalic_Click()
  InsertTag "i"
End Sub

Private Sub mnuPRE_Click()
  InsertTag "pre"
End Sub

Private Sub mnuOL_Click()
  ListTag "ol"
End Sub
  
Private Sub testTextImage()
  Dim formatTxt As String
  Dim FormatStart As Long, FormatEnd As Long
  FormatStart = InStr(txtMain.Text, Asterisks80)
  FormatEnd = InStrRev(txtMain.Text, Asterisks80)
  If FormatEnd > FormatStart Then CardImage (Mid(txtMain.Text, FormatStart, FormatEnd - FormatStart))
End Sub

Private Sub mnuTextImage_Click()
  mnuTextImage.Checked = Not mnuTextImage.Checked
  If mnuTextImage.Checked Then
    testTextImage
  Else
    frmSample.Visible = False
  End If
End Sub

Private Sub mnuUL_Click()
  ListTag "ul"
End Sub

Private Sub mnuUnderline_Click()
  InsertTag "u"
End Sub

Private Sub mnuLink_Click()
  Dim startPos&
  startPos = txtMain.selStart
  InsertTag "a"
  txtMain.selStart = startPos + 2
  txtMain.SelText = " href=""#"""
  txtMain.selStart = startPos + 9
End Sub

Private Sub ListTag(tag$)
  Dim startPos&, endPos&
  With txtMain
    startPos = .selStart
    endPos = startPos + .SelLength
    InsertTag tag
    .selStart = startPos + 4
    .SelText = vbCrLf & "<li>"
    If endPos = startPos Then
      .selStart = startPos + 10
      .SelText = vbCrLf
      .selStart = startPos + 10
    Else
      startPos = .selStart
      endPos = endPos + 9
      While startPos < endPos
        startPos = InStr(startPos + 1, .Text, vbCrLf)
        If startPos = 0 Or startPos >= endPos Then
          startPos = endPos
        Else
          .selStart = startPos + 1
          .SelText = "<li>"
          endPos = endPos + 4
        End If
      Wend
    End If
  End With
End Sub

Private Sub InsertTag(tag$)
  Dim startTag$, endtag$
  Dim startPos&, endPos&
  With txtMain
    startPos = .selStart
    endPos = startPos + .SelLength
    
    Select Case LCase(tag)
      Case "keyword", "indexword"
        startTag = "<" & tag & "="""
        endtag = """>"
      Case Else
        startTag = "<" & tag & ">"
        endtag = "</" & tag & ">"
    End Select
    
    If .SelLength = 0 Then
      .SelText = startTag & endtag
      .selStart = startPos + Len(startTag)
    Else
      .selStart = endPos
      .SelText = endtag
      .selStart = startPos
      .SelText = startTag
      .selStart = endPos + Len(startTag & endtag)
    End If
  End With
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
  If IsNumeric(Timer1.tag) Then
    tree1.SelectedItem = tree1.Nodes(CInt(Timer1.tag))
    If txtMain.Text <> WholeFileString(CurrentFilename) Then SetFileChanged True
  End If
  Timer1.Enabled = False
End Sub

Private Sub TimerSlowAction_Timer()
  TimerSlowAction.Enabled = False
  AbortAction = True
End Sub

Private Sub tree1_AfterLabelEdit(Cancel As Integer, NewString As String)
  Dim OldFilePath As String
  With tree1.SelectedItem
    OldFilePath = path & "\" & Mid(.key, 2) & SourceExtension
    If Len(Dir(OldFilePath)) > 0 Then
      Select Case MsgBox("Rename file '" & OldFilePath & "' to '" & NewString & "?", vbYesNoCancel)
        Case vbNo:  .Text = NewString: .key = "N" & .fullpath
        Case vbYes: .Text = NewString: .key = "N" & .fullpath
          'Name OldFilePath As path & "\" & .fullpath & SourceExtension
          Name OldFilePath As PathNameOnly(OldFilePath) & "\" & .Text & SourceExtension
        Case vbCancel:
          Cancel = True
      End Select
    End If
  End With
End Sub

Private Sub tree1_KeyDown(KeyCode As Integer, Shift As Integer)
  Dim nod As Node
  Select Case KeyCode
    Case vbKeyDelete: tree1.Nodes.Remove tree1.SelectedItem.Index
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
  Dim filename$, fullpath$
  Dim inClick As Boolean
  If Not inClick And Not Timer1.Enabled Then
    inClick = True
    If NodeLinking > 0 Then
      fullpath = "c:\" & PathNameOnly(NodeFile(NodeLinking))
      filename = HTMLRelativeFilename("c:\" & Mid(Node.key, 2), fullpath)
      EditSubTag "href", filename
      DelaySetNode NodeLinking
      NodeLinking = 0
      Me.MousePointer = vbDefault
    Else
      filename = Mid(Node.key, 2)
      If QuerySave = vbCancel Then 'Should move focus back to old node here
        DelaySetNode 1
      Else
        LoadTextboxFromFile path, filename, SourceExtension, txtMain
        If tree1.SelectedItem Is Nothing Then
          tree1.SelectedItem = Node
        ElseIf tree1.SelectedItem.Index <> Node.Index Then
          tree1.SelectedItem = Node
        End If
      End If
    End If
  End If
  inClick = False
End Sub

Public Sub LoadTextboxFromFile(fullpath$, filename$, ext$, txtBox As RichTextBox)
  Static LastAnswer As VbMsgBoxResult
  Dim altExt$, altpath$, thisAnswer As VbMsgBoxResult
  If Len(Dir(fullpath & "\" & filename & ext)) = 0 Then 'Check for files named .html or SourceExtension
    If LCase(ext) = LCase(SourceExtension) Then altExt = ".html" Else altExt = SourceExtension
    altpath = fullpath & "\" & filename & altExt
    If Len(Dir(altpath)) > 0 Then
      If altExt = SourceExtension Then
        ext = SourceExtension
      Else
        If LastAnswer = 0 Then
          thisAnswer = MsgBox("File " & filename & SourceExtension & " was not found, use " & filename & ".html instead?", vbYesNoCancel, path)
          If thisAnswer = vbCancel Then Exit Sub
          LastAnswer = MsgBox("Treat other missing files the same way?", vbYesNo)
          If LastAnswer = vbYes Then LastAnswer = thisAnswer Else LastAnswer = 0
        Else
          thisAnswer = LastAnswer
        End If
        If thisAnswer = vbYes Then FileCopy altpath, fullpath & "\" & filename & ext
      End If
    End If
  End If
  ReadFile fullpath & "\" & filename & ext, txtBox
End Sub

Private Sub ReadFile(filename$, txtBox As RichTextBox)
  Dim f%        'file handle
  Dim FileLength&
  f = FreeFile(0)
  On Error GoTo nofile
OpenFile:
  Open filename For Input As #f
  On Error GoTo 0
  FileLength = LOF(f)
  If txtBox.Name = "txtMain" Then
    CurrentFilename = filename
    caption = CurrentFilename
    CurrentFileContents = Input(FileLength, f)
    txtBox.Text = CurrentFileContents
    If ViewFormatting Then FormatText txtBox
  Else
    txtBox.Text = Input(FileLength, f)
  End If
  Close f
  SetFileChanged False
  Exit Sub
nofile:
  txtBox.Text = "(no file)"
  If MsgBox("File '" & filename & "' does not exist. Create it?", vbYesNo, "Missing file") = vbYes Then
    Err.Clear
    On Error Resume Next
    If Len(Dir(PathNameOnly(filename), vbDirectory)) = 0 Then MkDir PathNameOnly(filename)
    On Error GoTo errCreate
    Open filename For Output As #f
    Print #f, ""
    Close f
    GoTo OpenFile
  Else
    If txtBox.Name = "txtMain" Then
      CurrentFilename = filename
      caption = CurrentFilename
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
      caption = CurrentFilename & " (edited)"
    Else
      caption = CurrentFilename
    End If
  End If
End Sub

Private Function RTF_START(txtBox As RichTextBox)
  'RTF_START = "{\rtf1\ansi\deff0{\fonttbl{\f0\fswiss MS Sans Serif;}}\pard\plain\fs17 "
  RTF_START = "{\rtf1\ansi\deff0{\fonttbl{\f0\fswiss MS Sans Serif;}{\f1\froman\fcharset2 Symbol;}{\f2\f" & txtBox.Font.Name & ";}{\f3\fmodern Courier New;}}"
End Function

Private Sub FormatTextSelection(txtBox As RichTextBox, startPos As Long, endPos As Long, command As String)
  txtBox.selStart = startPos
  txtBox.SelLength = endPos - startPos
  Select Case command
    Case "bold":      txtBox.SelBold = True
    Case "italic":    txtBox.SelItalic = True
    Case "underline": txtBox.SelUnderline = True
    Case "bullet":    txtBox.SelBullet = True
    Case Else:        txtBox.SelFontName = command
  End Select
End Sub

Private Function FormatText(txtBox As RichTextBox)
  Dim txt$, tag As String
  Dim nextch&, maxch&
  Dim openTag&, closeTag&, parenlevel&, spacepos&
  Dim selStart&, SelLength&
  txtBox.Visible = False
  Me.MousePointer = vbHourglass
  selStart = txtBox.selStart
  SelLength = txtBox.SelLength
  AbortAction = False
  TimerSlowAction.Enabled = True
  txt = txtBox.Text
   
  'clear formatting
  txtBox.selStart = 0
  txtBox.SelLength = Len(txt)
  txtBox.SelBold = False
  txtBox.SelItalic = False
  txtBox.SelUnderline = False
  'txtBox.SelBullet = False
  
  maxch = Len(txt)
  
  While nextch < maxch And Not AbortAction
    nextch = InStr(nextch + 1, txt, "<")
    If nextch = 0 Then
      nextch = maxch
    Else
      tag = Mid(txt, nextch + 1, 2)
      closeTag = InStr(nextch + 1, txt, "</" & tag)
      If closeTag > 0 Then
        Select Case LCase(tag)
          Case "h>", "b>": FormatTextSelection txtBox, nextch + 2, closeTag - 1, "bold"
          Case "i>":       FormatTextSelection txtBox, nextch + 2, closeTag - 1, "italic"
          Case "u>":       FormatTextSelection txtBox, nextch + 2, closeTag - 1, "underline"
          Case "pr":       FormatTextSelection txtBox, nextch + 4, closeTag - 1, "Courier New"
        End Select
      'Else
      '  If LCase(tag) = "li" Then FormatTextSelection txtBox, nextch + 2, nextch + 3, "bullet"
      End If
    End If
  Wend
  If AbortAction Then
    If InStr(caption, "(formatting aborted)") = 0 Then caption = caption & " (formatting aborted)"
  End If
  txtBox.selStart = selStart
  txtBox.SelLength = SelLength
  TimerSlowAction.Enabled = False
  txtBox.Visible = True
  txtBox.SetFocus
  Me.MousePointer = vbDefault
End Function

'Private Function FormatTextOld(txtBox As RichTextBox)
'  Dim rtf$
'  Dim nextch&, maxch&
'  Dim openTag&, closeTag&, parenlevel&, spacepos&
'  AbortAction = False
'  TimerSlowAction.Enabled = True
'  rtf = ReplaceString(txtBox.Text, "\", "\\")
'  rtf = ReplaceString(txtBox.Text, "{", "\{")
'  rtf = ReplaceString(txtBox.Text, "}", "\}")
'
'  If Not AbortAction Then rtf = ReplaceString(rtf, "<h", RTF_BOLD & "<h")
'  If Not AbortAction Then rtf = ReplaceString(rtf, "</h", RTF_BOLD_END & "</h")
'
'  If Not AbortAction Then rtf = ReplaceString(rtf, "<u>", "<u>" & RTF_UNDERLINE)
'  If Not AbortAction Then rtf = ReplaceString(rtf, "<b>", "<b>" & RTF_BOLD)
'  If Not AbortAction Then rtf = ReplaceString(rtf, "<i>", "<i>" & RTF_ITALIC)
'
'  If Not AbortAction Then rtf = ReplaceString(rtf, "</u>", RTF_UNDERLINE_END & "</u>")
'  If Not AbortAction Then rtf = ReplaceString(rtf, "</b>", RTF_BOLD_END & "</b>")
'  If Not AbortAction Then rtf = ReplaceString(rtf, "</i>", RTF_ITALIC_END & "</i>")
'
'  If Not AbortAction Then rtf = ReplaceString(rtf, vbCrLf, RTF_PARAGRAPH)
'
'  'make sure text ends with a newline
'  If Right(rtf, 2 * Len(RTF_PARAGRAPH)) <> RTF_PARAGRAPH & RTF_PARAGRAPH Then
'    rtf = rtf & RTF_PARAGRAPH & RTF_PARAGRAPH
'  End If
'  If AbortAction And InStr(caption, "(formatting aborted)") = 0 Then
'    caption = caption & " (formatting aborted)"
'  End If
'  rtf = RTF_START(txtBox) & rtf & RTF_END
'
'  If rtf <> txtBox.TextRTF Then
'    Dim selStart&, SelLength&
'    selStart = txtBox.selStart
'    SelLength = txtBox.SelLength
'    txtBox.TextRTF = rtf
'    txtBox.selStart = selStart
'    txtBox.SelLength = SelLength
'  End If
'  TimerSlowAction.Enabled = False
'End Function

Private Function QuerySaveProject() As VbMsgBoxResult
  Dim retval As VbMsgBoxResult
  retval = vbYes
  If ProjectChanged Then
    retval = MsgBox("Save changes to " & ProjectFileName & "?", vbYesNoCancel)
    If retval = vbYes Then SaveProject ProjectFileName, tree1
    ProjectChanged = False
  End If
  QuerySaveProject = retval
End Function

Private Function QuerySave() As VbMsgBoxResult
  Dim retval As VbMsgBoxResult
  retval = vbYes
  If Changed Then
    If Not mnuAutoSave.Checked Then
      retval = MsgBox("Save changes to " & CurrentFilename & "?", vbYesNoCancel)
    End If
    If retval = vbYes Then mnuSaveFile_Click
  End If
  QuerySave = retval
End Function

Private Sub txtFind_KeyDown(KeyCode As Integer, Shift As Integer)
  If KeyCode = vbKeyReturn Then cmdFind_MouseUp vbLeftButton, Shift, 0, 0
End Sub

Private Sub txtMain_KeyUp(KeyCode As Integer, Shift As Integer)
  If FormatWhileTyping Then FormatText txtMain
End Sub

Private Sub txtReplace_KeyDown(KeyCode As Integer, Shift As Integer)
  If KeyCode = vbKeyReturn Then cmdReplace_MouseUp vbLeftButton, Shift, 0, 0
End Sub

Private Sub txtMain_Change()
  Static InChange As Boolean
  If Not InChange And Not Undoing Then
    InChange = True
    
    Undos(UndoPos) = txtMain.Text
    UndoCursor(UndoPos) = txtMain.selStart
    UndoPos = UndoPos + 1
    If UndoPos > MaxUndo Then UndoPos = 0
    If UndosAvail < MaxUndo Then UndosAvail = UndosAvail + 1
    
    If CurrentFileContents <> txtMain.Text Then
      If Not Changed Then SetFileChanged True
    Else
      If Changed Then SetFileChanged False
    End If
    mnuSaveFile.Enabled = Changed
    If mnuTextImage.Checked Then testTextImage
    InChange = False
  End If
End Sub

Private Sub txtMain_Click()
  Dim mnuItem&
  Dim subtagName$, filename$, PathName$
  Dim txt$
  txt = txtMain.Text
  DoEvents
  For mnuItem = mnuContext.Count - 1 To 1 Step -1
    Unload mnuContext(mnuItem)
  Next mnuItem
  GetCurrentTag 'txt, txtMain.SelStart, tagName, openTagPos, closeTagPos
  If openTagPos < closeTagPos Then
    Select Case tagName
      Case "img"
        AddContextMenuItem CaptureReplace
        AddContextMenuItem CaptureNew
        AddContextMenuItem BrowseImage
        AddContextMenuItem ViewImage
        'filename = SubTagValue("src")
        'filename = ReplaceString(filename, "/", "\")
        'pathname = AbsolutePath(filename, PathNameOnly(path & "\" & NodeFile))
        'If Len(Dir(pathname)) > 0 Then frmSample.SetImage pathname
      Case "a"
        Dim hashPos&
        AddContextMenuItem SelectLink
        filename = SubTagValue("href")
        hashPos = InStr(filename, "#")
        If hashPos > 0 Then filename = Left(filename, hashPos - 1)
        If Len(filename) > 0 Then
          filename = ReplaceString(filename, "/", "\")
          If Left(filename, 1) = "\" Then
            PathName = path & filename
          Else
            PathName = PathNameOnly(path & "\" & NodeFile) & "\" & filename
          End If
          If FileExists(PathName) Then
            frmSample.SetText PathName
          ElseIf FileExists(PathName & SourceExtension) Then
            frmSample.SetText PathName & SourceExtension
          End If
        End If
    End Select
    'If txtMainButton = vbRightButton Then PopupMenu mnuContextTop
  End If
End Sub

Private Sub txtMain_KeyPress(KeyAscii As Integer)
  Dim oldStart As Long
  Select Case KeyAscii
    Case 26 'Control-Z = undo
      If UndosAvail > 0 Then
        Undoing = True
        UndoPos = UndoPos - 1
        If UndoPos < 0 Then UndoPos = MaxUndo
        txtMain.Text = Undos(UndoPos)
        txtMain.selStart = UndoCursor(UndoPos)
        UndosAvail = UndosAvail - 1
        Undoing = False
      End If
    Case 13
      If mnuAutoParagraph.Checked Then
        oldStart = txtMain.selStart
        txtMain.Text = Left(txtMain.Text, oldStart) & "<p>" & Mid(txtMain.Text, oldStart + 1)
        txtMain.selStart = oldStart + 3
      End If
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
  txt = txtMain.Text
  start = txtMain.selStart
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
  Dim valueStart&, valueEnd&, subtagStart&, selStart&, retval$
  Dim txt$, tag$
  txt = txtMain.Text
  selStart = txtMain.selStart
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
    If valueEnd > valueStart Then retval = Mid(tag, valueStart, valueEnd - valueStart)
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
  txt = txtMain.Text
  tag = LCase(Mid(txt, openTagPos, closeTagPos - openTagPos + 1))
  subtagStart = InStr(1, tag, LCase(subtagName))
  If subtagStart = 0 Then
    txtMain.Text = Left(txt, closeTagPos - 1) & " " & LCase(subtagName) & "=" & newValue & Mid(txt, closeTagPos)
  Else
    'subtagStart = subtagStart + openTagPos
    valueStart = subtagStart + Len(subtagName) + 1
    If Mid(tag, valueStart, 1) = """" Then
      valueEnd = InStr(valueStart + 1, tag, """")
    Else
      valueEnd = InStr(valueStart + 1, tag, " ")
      If valueEnd = 0 Then valueEnd = Len(tag)
    End If
    txtMain.Text = Left(txt, openTagPos + valueStart - 1) & newValue & Mid(txt, openTagPos + valueEnd - 1)
    closeTagPos = InStr(openTagPos + 1, txtMain.Text, ">")
  End If
  txtMain.selStart = openTagPos + 1
  txtMain_Click
  txtMain.selStart = closeTagPos + 1
End Sub

Private Sub AddContextMenuItem(newItem$)
  Dim mnuItem&
  mnuItem = mnuContext.Count
  Load mnuContext(mnuItem)
  mnuContext(mnuItem).caption = newItem
End Sub

Private Sub txtMain_SelChange()
  Dim lastSelStart&
  If txtMain.selStart <> lastSelStart Then
    
    lastSelStart = txtMain.selStart
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
      mnuRecent(rfMove).caption = "&" & rfMove & " " & FilenameOnly(mnuRecent(rfMove).tag)
    Next rfMove
  Else 'Add file to list
    mnuRecent(0).Visible = True
    If mnuRecent.Count <= MaxRecentFiles Then Load mnuRecent(mnuRecent.Count)
    For rfMove = mnuRecent.Count - 1 To 2 Step -1
      mnuRecent(rfMove).tag = mnuRecent(rfMove - 1).tag
      mnuRecent(rfMove).caption = "&" & rfMove & " " & FilenameOnly(mnuRecent(rfMove).tag)
    Next rfMove
  End If
  mnuRecent(1).Visible = True
  mnuRecent(1).tag = FilePath
  mnuRecent(1).caption = "&1 " & FilenameOnly(mnuRecent(rfMove).tag)
End Sub

