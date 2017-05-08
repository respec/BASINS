VERSION 5.00
Object = "{6B7E6392-850A-101B-AFC0-4210102A8DA7}#1.3#0"; "COMCTL32.OCX"
Object = "{F9043C88-F6F2-101A-A3C9-08002B2F49FB}#1.2#0"; "COMDLG32.OCX"
Object = "{872F11D5-3322-11D4-9D23-00A0C9768F70}#1.9#0"; "ATCoCtl.ocx"
Begin VB.Form frmMain 
   Caption         =   "Data Browser"
   ClientHeight    =   5700
   ClientLeft      =   165
   ClientTop       =   735
   ClientWidth     =   9660
   Icon            =   "frmMainMap.frx":0000
   LinkTopic       =   "Form1"
   ScaleHeight     =   5700
   ScaleWidth      =   9660
   StartUpPosition =   3  'Windows Default
   Begin ATCoCtl.ATCoGrid agdDetails 
      Height          =   4332
      Left            =   3960
      TabIndex        =   5
      Top             =   0
      Width           =   3132
      _ExtentX        =   5530
      _ExtentY        =   7646
      SelectionToggle =   0   'False
      AllowBigSelection=   -1  'True
      AllowEditHeader =   0   'False
      AllowLoad       =   0   'False
      AllowSorting    =   0   'False
      Rows            =   1
      Cols            =   2
      ColWidthMinimum =   300
      gridFontBold    =   0   'False
      gridFontItalic  =   0   'False
      gridFontName    =   "MS Sans Serif"
      gridFontSize    =   8
      gridFontUnderline=   0   'False
      gridFontWeight  =   400
      gridFontWidth   =   0
      Header          =   "lblHeader"
      FixedRows       =   1
      FixedCols       =   0
      ScrollBars      =   3
      SelectionMode   =   0
      BackColor       =   -2147483643
      ForeColor       =   -2147483640
      BackColorBkg    =   -2147483632
      BackColorSel    =   -2147483635
      ForeColorSel    =   -2147483634
      BackColorFixed  =   -2147483633
      ForeColorFixed  =   -2147483630
      InsideLimitsBackground=   -2147483643
      OutsideHardLimitBackground=   8421631
      OutsideSoftLimitBackground=   8454143
      ComboCheckValidValues=   0   'False
   End
   Begin VB.TextBox txtMetadata 
      Height          =   2415
      Left            =   5520
      MultiLine       =   -1  'True
      ScrollBars      =   3  'Both
      TabIndex        =   4
      Text            =   "frmMainMap.frx":0442
      Top             =   0
      Visible         =   0   'False
      Width           =   3255
   End
   Begin VB.PictureBox pctMap 
      AutoRedraw      =   -1  'True
      Height          =   3855
      Left            =   7200
      ScaleHeight     =   3795
      ScaleWidth      =   2235
      TabIndex        =   2
      Top             =   -60
      Visible         =   0   'False
      Width           =   2295
   End
   Begin VB.Frame sash 
      BorderStyle     =   0  'None
      Caption         =   "Frame1"
      Height          =   5535
      Left            =   3540
      MousePointer    =   9  'Size W E
      TabIndex        =   0
      Top             =   0
      Width           =   65
   End
   Begin MSComDlg.CommonDialog cdlg 
      Left            =   4440
      Top             =   4320
      _ExtentX        =   847
      _ExtentY        =   847
      _Version        =   393216
   End
   Begin ComctlLib.TreeView treeDirectory 
      Height          =   5535
      Left            =   0
      TabIndex        =   1
      Top             =   0
      Width           =   3555
      _ExtentX        =   6271
      _ExtentY        =   9763
      _Version        =   327682
      HideSelection   =   0   'False
      Indentation     =   706
      LabelEdit       =   1
      Style           =   7
      Appearance      =   1
   End
   Begin ComctlLib.TreeView treeMetadata 
      Height          =   1095
      Left            =   3600
      TabIndex        =   3
      Top             =   0
      Visible         =   0   'False
      Width           =   3555
      _ExtentX        =   6271
      _ExtentY        =   1931
      _Version        =   327682
      HideSelection   =   0   'False
      Indentation     =   706
      LabelEdit       =   1
      Style           =   7
      Appearance      =   1
   End
   Begin ATCoCtl.ATCoGrid agd2 
      Height          =   4332
      Left            =   0
      TabIndex        =   6
      Top             =   0
      Width           =   3132
      _ExtentX        =   5530
      _ExtentY        =   7646
      SelectionToggle =   0   'False
      AllowBigSelection=   -1  'True
      AllowEditHeader =   0   'False
      AllowLoad       =   0   'False
      AllowSorting    =   0   'False
      Rows            =   1
      Cols            =   2
      ColWidthMinimum =   300
      gridFontBold    =   0   'False
      gridFontItalic  =   0   'False
      gridFontName    =   "MS Sans Serif"
      gridFontSize    =   8
      gridFontUnderline=   0   'False
      gridFontWeight  =   400
      gridFontWidth   =   0
      Header          =   "lblHeader"
      FixedRows       =   1
      FixedCols       =   0
      ScrollBars      =   3
      SelectionMode   =   0
      BackColor       =   -2147483643
      ForeColor       =   -2147483640
      BackColorBkg    =   -2147483632
      BackColorSel    =   -2147483635
      ForeColorSel    =   -2147483634
      BackColorFixed  =   -2147483633
      ForeColorFixed  =   -2147483630
      InsideLimitsBackground=   -2147483643
      OutsideHardLimitBackground=   8421631
      OutsideSoftLimitBackground=   8454143
      ComboCheckValidValues=   0   'False
   End
   Begin ComctlLib.ImageList lstIcons 
      Left            =   3840
      Top             =   4320
      _ExtentX        =   1005
      _ExtentY        =   1005
      BackColor       =   -2147483643
      ImageWidth      =   16
      ImageHeight     =   16
      MaskColor       =   12632256
      _Version        =   327682
      BeginProperty Images {0713E8C2-850A-101B-AFC0-4210102A8DA7} 
         NumListImages   =   8
         BeginProperty ListImage1 {0713E8C3-850A-101B-AFC0-4210102A8DA7} 
            Picture         =   "frmMainMap.frx":044E
            Key             =   "Folder"
         EndProperty
         BeginProperty ListImage2 {0713E8C3-850A-101B-AFC0-4210102A8DA7} 
            Picture         =   "frmMainMap.frx":0768
            Key             =   "Null"
         EndProperty
         BeginProperty ListImage3 {0713E8C3-850A-101B-AFC0-4210102A8DA7} 
            Picture         =   "frmMainMap.frx":0A82
            Key             =   "Point"
         EndProperty
         BeginProperty ListImage4 {0713E8C3-850A-101B-AFC0-4210102A8DA7} 
            Picture         =   "frmMainMap.frx":0D9C
            Key             =   "MultiPoint"
         EndProperty
         BeginProperty ListImage5 {0713E8C3-850A-101B-AFC0-4210102A8DA7} 
            Picture         =   "frmMainMap.frx":10B6
            Key             =   "PolyLine"
         EndProperty
         BeginProperty ListImage6 {0713E8C3-850A-101B-AFC0-4210102A8DA7} 
            Picture         =   "frmMainMap.frx":13D0
            Key             =   "Polygon"
         EndProperty
         BeginProperty ListImage7 {0713E8C3-850A-101B-AFC0-4210102A8DA7} 
            Picture         =   "frmMainMap.frx":16EA
            Key             =   "WDM"
         EndProperty
         BeginProperty ListImage8 {0713E8C3-850A-101B-AFC0-4210102A8DA7} 
            Picture         =   "frmMainMap.frx":1A04
            Key             =   "UCI"
         EndProperty
      EndProperty
   End
   Begin VB.Menu mnuTop 
      Caption         =   "&File"
      Index           =   0
      Begin VB.Menu mnuOpen 
         Caption         =   "&Open Directory"
         Shortcut        =   ^O
      End
      Begin VB.Menu mnuOpenMetadata 
         Caption         =   "Open &Metadata"
      End
      Begin VB.Menu mnuSaveProject 
         Caption         =   "&Save Project"
      End
   End
   Begin VB.Menu mnuTop 
      Caption         =   "&View"
      Index           =   2
      Begin VB.Menu mnuViewHeaders 
         Caption         =   "&Headers"
         Checked         =   -1  'True
      End
      Begin VB.Menu mnuViewData 
         Caption         =   "&Data"
      End
      Begin VB.Menu mnuViewMetadata 
         Caption         =   "&Metadata"
      End
      Begin VB.Menu mnuViewText 
         Caption         =   "&Text Details"
      End
      Begin VB.Menu mnuSep1 
         Caption         =   "-"
      End
      Begin VB.Menu mnuColumns 
         Caption         =   "&Edit Columns"
      End
      Begin VB.Menu mnuSep2 
         Caption         =   "-"
      End
      Begin VB.Menu mnuViewMap 
         Caption         =   "&Map"
         Shortcut        =   ^M
      End
      Begin VB.Menu mnuViewLockBoundingBox 
         Caption         =   "Lock &Bounding Box"
         Shortcut        =   ^B
      End
      Begin VB.Menu mnuViewLock 
         Caption         =   "&Lock Layer"
         Shortcut        =   ^L
      End
      Begin VB.Menu mnuViewUnlock 
         Caption         =   "&Unlock Layers"
         Shortcut        =   ^U
      End
      Begin VB.Menu mnusep3 
         Caption         =   "-"
      End
      Begin VB.Menu mnuViewOptions 
         Caption         =   "&Options..."
      End
   End
   Begin VB.Menu mnuTop 
      Caption         =   "&Report"
      Index           =   3
      Begin VB.Menu mnuReportStart 
         Caption         =   "&Start Report"
      End
      Begin VB.Menu mnuReportAddTable 
         Caption         =   "Add Current &Table"
      End
      Begin VB.Menu mnuReportAddFolderTables 
         Caption         =   "Add All &Folder Tables"
      End
      Begin VB.Menu mnuReportAddHeaderTables 
         Caption         =   "Add All &Header Tables"
      End
      Begin VB.Menu mnuReportAddDataTables 
         Caption         =   "Add All &Data Tables"
         Enabled         =   0   'False
      End
   End
   Begin VB.Menu mnuTop 
      Caption         =   "&Convert"
      Index           =   4
      Begin VB.Menu mnuConvertFromTo 
         Caption         =   "Albers to Lat/Long"
         Index           =   1
      End
      Begin VB.Menu mnuConvertFromTo 
         Caption         =   "Lat/Long to Albers"
         Index           =   2
      End
      Begin VB.Menu mnuConvertFromTo 
         Caption         =   "Mass State Plane to Lat/Long"
         Index           =   3
      End
      Begin VB.Menu mnuConvertFromTo 
         Caption         =   "Lat/Long to Mass State Plane"
         Index           =   4
      End
      Begin VB.Menu mnuConvertToPoints 
         Caption         =   "Line/Poly to Points"
         Index           =   1
      End
      Begin VB.Menu mnuConvertToPoints 
         Caption         =   "Line/Poly to Endpoints"
         Index           =   2
      End
   End
End
Attribute VB_Name = "frmMain"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Private SashDragging As Boolean
Private layers() As Layer
Private scaleLayer As Layer
Private metaDataPath$, metaDataExt$
Private reportFilename$, writingReportType& '0 = none, 1=RDB, 2=RTF

Public Sub RefreshView()
  treeDirectory_NodeClick treeDirectory.SelectedItem
End Sub

Private Sub agdDetails_Click()
  Debug.Print "(" & agdDetails.row & ", " & agdDetails.col & ")"
End Sub

Private Sub Form_Load()
  gridClear agdDetails
  ReDim layers(0)
  ReDim dispTypes(2)
  dispTypes(0) = "shp"
  dispTypes(1) = "uci"
  dispTypes(2) = "wdm"
  InitFileTypeName
  InitDataTypeName
  InitColName
  treeDirectory.ImageList = lstIcons
  txtMetadata.Text = ""
End Sub

Private Sub Form_Resize()
  Dim newWidth&
  If Height > 800 Then sash.Height = Height - 753 'menu height
  treeDirectory.Height = sash.Height
  treeMetadata.Height = sash.Height
  txtMetadata.Height = sash.Height
  agdDetails.Height = sash.Height
  pctMap.Height = sash.Height
  
  agdDetails.Left = sash.Left + sash.Width
  newWidth = Width - agdDetails.Left - 100
  If newWidth > 0 Then
    If agdDetails.Visible And pctMap.Visible Then
      agdDetails.Width = newWidth / 2
      pctMap.Width = newWidth / 2
      pctMap.Left = agdDetails.Left + agdDetails.Width
    Else
      agdDetails.Width = newWidth
      pctMap.Width = newWidth
      pctMap.Left = agdDetails.Left
    End If
    'If treeMetadata.Visible And txtMetadata.Visible Then
      treeMetadata.Left = sash.Left + sash.Width
      treeMetadata.Width = newWidth / 2
      txtMetadata.Width = newWidth / 2
      txtMetadata.Left = treeMetadata.Left + treeMetadata.Width
    'End If
  End If
  agd2.Left = agdDetails.Left
  agd2.Width = agdDetails.Width
  If agdDetails.Height > agd2.Top Then agd2.Height = agdDetails.Height - agd2.Top
End Sub

Private Sub mnuColumns_Click()
  Call EditColumns(agdDetails, treeDirectory)
End Sub

Public Sub OpenDir()
  mnuOpen_Click
End Sub


Private Sub mnuConvertFromTo_Click(Index As Integer)
  Dim nod As Node, count&, msg$
  count = 0
  On Error GoTo Refresh
  If treeDirectory.SelectedItem.Image <> "Folder" Then
    ConvertFromTo treeDirectory.SelectedItem, Index
    count = 1
  Else
    Set nod = treeDirectory.SelectedItem.Child
    While Not IsNull(nod)
      ConvertFromTo nod, Index
      count = count + 1
      Set nod = nod.Next
    Wend
  End If
Refresh:
  If count = 1 Then
    msg = "Converted one shape file from "
  Else
    msg = "Converted " & count & " shape files from "
  End If
  Select Case Index
    Case 1: MsgBox msg & "Albers to Lat/Long"
    Case 2: MsgBox msg & "Lat/Long to Albers"
    Case 3: MsgBox msg & "Mass. State Plane to Lat/Long"
    Case 4: MsgBox msg & "Lat/Long to Mass. State Plane"
  End Select
  If treeDirectory.Nodes.count > 0 Then Call OpenDirectory(treeDirectory.Nodes(1).Text, treeDirectory)
End Sub

Private Sub mnuConvertToPoints_Click(Index As Integer)
  Dim nod As Node, count&, justEndpoints As Boolean
  count = 0
  If Index = 2 Then justEndpoints = True Else justEndpoints = False
  On Error GoTo Refresh
  If treeDirectory.SelectedItem.Image <> "Folder" Then
    ConvertToPoints treeDirectory.SelectedItem, justEndpoints
    count = 1
  Else
    Set nod = treeDirectory.SelectedItem.Child
    While Not IsNull(nod)
      ConvertToPoints nod, justEndpoints
      count = count + 1
      Set nod = nod.Next
    Wend
  End If
Refresh:
  If count = 1 Then
    MsgBox "Converted one shape file from Line or Polygon to Points"
  Else
    MsgBox "Converted " & count & " shape files from Line or Polygon to Points"
  End If
  If treeDirectory.Nodes.count > 0 Then Call OpenDirectory(treeDirectory.Nodes(1).Text, treeDirectory)

End Sub

Private Sub mnuOpen_Click()
  cdlg.filename = "(all files)"
  cdlg.ShowOpen
  If Len(cdlg.filename) > 0 Then
    Dim fileBaseDir$
    fileBaseDir = PathNameOnly(cdlg.filename)
    If Len(fileBaseDir) > 0 Then
      Me.MousePointer = vbHourglass
      Call OpenDirectory(fileBaseDir, treeDirectory)
      Me.MousePointer = vbDefault
    End If
  End If
End Sub

Private Sub mnuOpenMetadata_Click()
  Dim dotpos&, extPos&
  If Not mnuViewMetadata.Checked Then mnuViewMetadata_Click
  'mnuViewMetadata_Click
  Me.MousePointer = vbHourglass
  Call DisplayMetadata(treeMetadata, "", cdlg)
  If Len(cdlg.filename) > 0 Then
    metaDataPath = PathNameOnly(cdlg.filename) & "\"
    dotpos = InStr(cdlg.filename, ".")
    While dotpos > 0
      extPos = dotpos
      dotpos = InStr(dotpos + 1, cdlg.filename, ".")
    Wend
    If extPos > 0 Then metaDataExt = Mid(cdlg.filename, extPos)
  End If
  Me.MousePointer = vbDefault
End Sub

Private Sub mnuReportAddFolderTables_Click()
  Dim ni As Long, cnt As Long
  
  cnt = 0
  If mnuViewMap.Checked Then
    mnuViewMap.Checked = False
    pctMap.Visible = False
  End If
  mnuViewHeaders.Checked = True
  With treeDirectory.Nodes
    For ni = 1 To .count
      If .Item(ni).Children > 0 Then
        treeDirectory_NodeClick .Item(ni)
        mnuReportAddTable_Click
        cnt = cnt + 1
      End If
    Next ni
  End With
  treeDirectory.Nodes(1).EnsureVisible
  Debug.Print "Added " & cnt & " Folder Tables to " & reportFilename
End Sub

Private Sub mnuReportAddHeaderTables_Click()
  Dim ni&, cnt&
  cnt = 0
  If mnuViewMap.Checked Then
    mnuViewMap.Checked = False
    pctMap.Visible = False
  End If
  mnuViewHeaders.Checked = True
  With treeDirectory.Nodes
    For ni = 1 To .count
      If .Item(ni).Image <> "Folder" Then
        treeDirectory_NodeClick .Item(ni)
        mnuReportAddTable_Click
        cnt = cnt + 1
      End If
    Next ni
  End With
  treeDirectory.Nodes(1).EnsureVisible
  Debug.Print "Added " & cnt & " Header Tables to " & reportFilename
End Sub

Private Sub mnuReportAddTable_Click()
  Dim doColTitles As Boolean
  
  'On Error GoTo Problem
  If writingReportType = 0 Then mnuReportStart_Click
  If writingReportType > 0 Then
    Dim f%        'file handle
    Dim FileLength&, srchPos&, findClose$, foundClose As Boolean
    f = FreeFile(0)
    Open reportFilename For Binary Access Read Write As #f
    FileLength = LOF(f)
    
    'A hack - we guess that there is only a title written so far and we still need col headers
    If FileLength < 100 Then doColTitles = True
    
    If writingReportType = 1 Then 'RDB file
      Seek #f, FileLength + 1
      If agd2.Visible Then
        If agd2.ColTitle(0) <> noDetailsMsg Then
          Call WriteRDBgrid(agd2, f, "Path", treeDirectory.SelectedItem.FullPath, doColTitles)
          'Put #f, , vbCrLf
        End If
      ElseIf agdDetails.Visible Then
        'Put #f, , treeDirectory.SelectedItem.FullPath & vbCrLf
        Call WriteRDBgrid(agdDetails, f, "Path", treeDirectory.SelectedItem.FullPath, doColTitles)
        'Put #f, , vbCrLf
      End If
      If txtMetadata.Visible Then Put #f, , txtMetadata.Text & vbCrLf
    ElseIf writingReportType = 2 Then 'RTF file
      If FileLength > 3 Then
        'Search for closing brace so we can append content inside braces
        srchPos = FileLength
        Seek #f, srchPos
        Get #f, , findClose
        While Mid(findClose, 1, 1) <> "}" And srchPos > 1
          srchPos = srchPos - 1
          Seek #f, srchPos
          Input #f, findClose
        Wend
        If Mid(findClose, 1, 1) = "}" Then
          foundClose = True
          Seek #f, srchPos
        End If
      End If
      If Not foundClose Then
        Seek #f, 1
        Put #f, , "{\rtf1\ansi \fs40 Report\par\pard\plain "
      End If
      If agdDetails.Visible Then
        Put #f, , "\fs30 " & RTFescape(treeDirectory.SelectedItem.FullPath) & "\par\pard\plain"
        Call WriteRTFgrid(agdDetails, f)
      End If
      If agd2.Visible Then Call WriteRTFgrid(agd2, f)
      If txtMetadata.Visible Then
        Put #f, , "\fs30 Metadata " & RTFescape(treeMetadata.SelectedItem.FullPath) & "\par\pard\plain"
        Put #f, , txtMetadata.Text
      End If
      Put #f, , "}"
    End If
    Close f
  End If
  Exit Sub
Problem:
  MsgBox "Error writing to file '" & reportFilename & "'" & vbCrLf & Err.Description, vbOKOnly
End Sub

Private Sub mnuReportStart_Click()
  On Error GoTo NeverMind
  reportFilename = ""
  writingReportType = 0
  cdlg.Filter = "Richtext files (*.rtf)|*.rtf|RDB files (*.rdb)|*.rdb|All files (*.*)|*.*"
  If InStr(cdlg.filename, "(all files)") > 0 Or Len(cdlg.filename) = 0 Then
    cdlg.filename = "Report"
  End If
  cdlg.ShowSave
  If Len(cdlg.filename) > 3 Then
    Dim f%, user$, machine$, header$
    reportFilename = cdlg.filename
    user = Environ("USERNAME")
    machine = Environ("COMPUTERNAME")
    f = FreeFile(0)
    header = "Report created"
    If Len(user) > 0 Then header = header & " by " & user
    If Len(machine) > 0 Then header = header & "@" & machine
    header = header & " on " & Date
    Open reportFilename For Binary Access Read Write As #f
    Select Case UCase(Right(reportFilename, 3))
      Case "RDB"
        writingReportType = 1
        Put #f, , "# RDB " & header & vbCrLf
      Case "RTF"
        writingReportType = 2
        Put #f, , "{\rtf1\ansi \fs40 " & RTFescape(header) & " \par\pard\plain }"
    End Select
    Close f
  End If
  Exit Sub
NeverMind:
  MsgBox "Error writing to file '" & cdlg.filename & "'" & vbCrLf & Err.Description, vbOKOnly
End Sub

Private Sub mnuViewData_Click()
  mnuViewData.Checked = Not mnuViewData.Checked
  If mnuViewData.Checked Then
    mnuViewHeaders.Checked = False
    mnuViewMetadata.Checked = False
  End If
  RefreshView
End Sub

Private Sub mnuViewHeaders_Click()
  mnuViewHeaders.Checked = Not mnuViewHeaders.Checked
  If mnuViewHeaders.Checked Then
    mnuViewData.Checked = False
    mnuViewMetadata.Checked = False
  End If
  RefreshView
End Sub

Private Sub mnuViewLock_Click()
  If Not layers(0) Is Nothing Then
    ReDim Preserve layers(UBound(layers) + 1)
    Set layers(UBound(layers)) = layers(0)
    Set layers(0) = Nothing
    mnuViewLock.Checked = True
  End If
End Sub

Private Sub mnuViewLockBoundingBox_Click()
  With mnuViewLockBoundingBox
    If .Checked Then
      .Checked = False
      Set scaleLayer = Nothing
    ElseIf Not layers(0) Is Nothing Then
      .Checked = True
      Set scaleLayer = layers(0)
      RefreshView
    End If
  End With
End Sub

Private Sub mnuViewMap_Click()
  mnuViewMap.Checked = Not mnuViewMap.Checked
  pctMap.Visible = mnuViewMap.Checked
  If pctMap.Visible Then
    RefreshView
  Else
    Form_Resize
  End If
End Sub

Private Sub mnuViewMetadata_Click()
  mnuViewMetadata.Checked = Not mnuViewMetadata.Checked
  If mnuViewMetadata.Checked Then
    mnuViewHeaders.Checked = False
    mnuViewData.Checked = False
  End If
  RefreshView
End Sub

Private Sub mnuViewOptions_Click()
  frmOptions.Show
End Sub

Private Sub mnuViewText_Click()
  mnuViewText.Checked = Not mnuViewText.Checked
  frmText.Visible = mnuViewText.Checked
  If mnuViewText.Checked Then
    RefreshView
  End If
End Sub

Private Sub mnuViewUnlock_Click()
  ReDim layers(0)
  mnuViewLock.Checked = False
  RefreshView
End Sub

Private Sub sash_MouseDown(Button As Integer, Shift As Integer, x As Single, y As Single)
  SashDragging = True
End Sub

Private Sub sash_MouseMove(Button As Integer, Shift As Integer, x As Single, y As Single)
  If SashDragging And (sash.Left + x) > 1000 And (sash.Left + x < Width - 1000) Then
    sash.Left = sash.Left + x
    treeDirectory.Width = sash.Left
    Form_Resize
  End If
End Sub

Private Sub sash_MouseUp(Button As Integer, Shift As Integer, x As Single, y As Single)
  SashDragging = False
End Sub

Private Sub treeDirectory_NodeClick(ByVal Node As ComctlLib.Node)
  Dim li As MapLayerInfo
  On Error GoTo NeverMind
  Me.MousePointer = vbHourglass
  gridClear agdDetails
  gridClear agd2
  If mnuViewMetadata.Checked Then
    txtMetadata.Visible = True
    treeMetadata.Visible = True
    agdDetails.Visible = False
    agd2.Visible = False
    pctMap.Visible = False
    If Len(metaDataPath) = 0 Then
      mnuOpenMetadata_Click
      Me.Caption = Node.FullPath
    Else
      Me.Caption = Node.FullPath
      Call DisplayMetadata(treeMetadata, metaDataPath & Node.Text & metaDataExt, cdlg)
    End If
  Else
    txtMetadata.Visible = False
    treeMetadata.Visible = False
    If Node.Children > 0 Then 'GetAttr(Node.FullPath) = vbDirectory Then=
      agdDetails.Visible = True
      agd2.Visible = False
      pctMap.Visible = False
      Form_Resize
      Me.Caption = Node.FullPath
      Call DisplayDirectoryDetails(treeDirectory, Node, agdDetails)
    ElseIf Node.Image <> "Folder" Then 'they clicked on a file
      'If mnuViewMetadata.Checked And Not treeMetadata.Visible Then treeMetadata.Visible = True
      If mnuViewMap.Checked And Not pctMap.Visible Then pctMap.Visible = True
      agdDetails.Visible = mnuViewHeaders.Checked
      agd2.Visible = mnuViewData.Checked
      Me.Caption = Node.FullPath
      If agdDetails.Visible Then
        agdDetails.rows = 1
        Call SetDirectoryHeaders(agdDetails)
        Call DisplayDirectoryRow(1, Node, agdDetails)
      End If
      If agd2.Visible Then
        If mnuViewHeaders.Checked Then
          Call DisplayDataHeaders(Node, agd2)
        Else
          Call DisplayData(Node, agd2, li, "<null>")
        End If
        agd2.ColsSizeByContents
      End If
      Form_Resize
      If mnuViewText.Checked Then
        Call DisplayText(Node)
      End If
      If pctMap.Visible Then
        Dim lyr&
        Set layers(0) = Nothing
        Set layers(0) = New Layer
        pctMap.Cls
        layers(0).ShapeFile = Node.FullPath
        
        'Set map scale from all layers or from designated scale layer
        If scaleLayer Is Nothing Then
          layers(0).SetMaxScale pctMap, False  'was layer 1
          For lyr = 1 To UBound(layers) 'added loop
            layers(lyr).SetMaxScale pctMap, True
          Next lyr
        Else
          scaleLayer.SetMaxScale pctMap, False
        End If
        
        'Draw all layers with most recently clicked layer last (on top)
        For lyr = UBound(layers) To 0 Step -1
          layers(lyr).Render pctMap
        Next lyr
      End If
    End If
  End If
NeverMind:
  Me.MousePointer = vbDefault
  treeDirectory.SelectedItem = Node
End Sub

Private Sub gridClear(g As ATCoGrid)
  g.rows = 0
  'g.Cols = 1
  g.Clear
End Sub

Private Sub treeMetadata_Expand(ByVal Node As ComctlLib.Node)
  treeMetadata_NodeClick Node
End Sub

Private Sub treeMetadata_NodeClick(ByVal Node As ComctlLib.Node)
  Dim txt$, lenTxt&, nod As Node, lastIndex&, Index&
  On Error GoTo SetText
  txt = metaDataString(Node.Index)
  If Node.Children > 0 Then
    Index = Node.Index + 1
    Set nod = Node
    While nod = nod.LastSibling
      Set nod = nod.Parent
    Wend
    lastIndex = nod.Next.Index - 1
    While lenTxt < 10000 And Index <= lastIndex
      txt = txt & vbCrLf & metaDataString(Index)
      lenTxt = Len(txt)
      If lenTxt = 2000 Then txtMetadata.Text = txt
      Index = Index + 1
    Wend
    If Index <= lastIndex Then txt = txt & vbCrLf & "[truncated]"
  End If
SetText:
  txtMetadata.Text = txt
End Sub

'Private Sub PopulateGrid(g As ATCoGrid, li As MapLayerInfo)
'  Dim pt&, prop As Variant, o&, i&, MapRS As Recordset, val
'  pt = 1
'  Set MapRS = OpenDBF(li.Path & li.baseFilename & ".shp", True)
'  MapRS.MoveFirst
'  'Debug.Print MapRS.Name
'  g.ClearData
'
'  'g.rows could be set to zero here, but this saves us adding rows later. RecordCount need not be exact, but should be <= actual number
'  g.Rows = MapRS.RecordCount
'  g.Cols = li.nFields 'MapRS.Fields.Count
'  For i = 0 To g.Cols - 1 '0 To MapRS.Fields.Count - 1
'    g.ColTitle(i) = li.Fields(i).Caption
'    If li.Fields(i).Name = li.KeyField Then
'      g.ColEditable(i) = False 'g.ColSelectable(i) = True
'    Else
'      g.ColEditable(i) = True  'g.ColSelectable(i) = False
'    End If
'    g.ColSelectable(i) = True
'  Next i
'  While Not MapRS.EOF
'    If g.Rows < pt Then
'      g.Rows = pt + 100 'over-allocate so we don't have to for each element.
'      ReDim Preserve li.Selected(pt + 101)
'    End If
'    For i = 0 To g.Cols - 1
'      val = MapRS.Fields(li.Fields(i).Name).Value
'      If Not IsNull(val) Then
'        g.TextMatrix(pt, i) = val
'      Else
'        g.TextMatrix(pt, i) = ""
'      End If
'    Next i
'    pt = pt + 1
'    MapRS.MoveNext
'  Wend
'  MapRS.Close
'  g.Rows = pt - 1
'  ReDim Preserve li.Selected(pt)
'End Sub

