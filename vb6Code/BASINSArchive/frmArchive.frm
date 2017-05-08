VERSION 5.00
Object = "{BDC217C8-ED16-11CD-956C-0000C04E4C0A}#1.1#0"; "Tabctl32.ocx"
Object = "{F9043C88-F6F2-101A-A3C9-08002B2F49FB}#1.2#0"; "Comdlg32.ocx"
Object = "{872F11D5-3322-11D4-9D23-00A0C9768F70}#1.10#0"; "ATCoCtl.ocx"
Begin VB.Form frmArchive 
   Caption         =   "BASINS Archive"
   ClientHeight    =   5796
   ClientLeft      =   132
   ClientTop       =   708
   ClientWidth     =   8028
   HelpContextID   =   26
   Icon            =   "frmArchive.frx":0000
   LinkTopic       =   "Form1"
   ScaleHeight     =   5796
   ScaleWidth      =   8028
   StartUpPosition =   3  'Windows Default
   Begin VB.ComboBox cmbDrives 
      Height          =   315
      Left            =   240
      Style           =   2  'Dropdown List
      TabIndex        =   20
      Top             =   480
      Width           =   1695
   End
   Begin MSComDlg.CommonDialog cdlg 
      Left            =   6000
      Top             =   240
      _ExtentX        =   847
      _ExtentY        =   847
      _Version        =   393216
   End
   Begin TabDlg.SSTab tabMain 
      Height          =   4695
      Left            =   120
      TabIndex        =   1
      Top             =   960
      Width           =   7815
      _ExtentX        =   13780
      _ExtentY        =   8276
      _Version        =   393216
      Tabs            =   4
      TabsPerRow      =   4
      TabHeight       =   520
      TabCaption(0)   =   "&Archive"
      TabPicture(0)   =   "frmArchive.frx":08CA
      Tab(0).ControlEnabled=   -1  'True
      Tab(0).Control(0)=   "Label1"
      Tab(0).Control(0).Enabled=   0   'False
      Tab(0).Control(1)=   "fraInclude"
      Tab(0).Control(1).Enabled=   0   'False
      Tab(0).Control(2)=   "lstBasinsProject"
      Tab(0).Control(2).Enabled=   0   'False
      Tab(0).Control(3)=   "fraButtons(0)"
      Tab(0).Control(3).Enabled=   0   'False
      Tab(0).ControlCount=   4
      TabCaption(1)   =   "&Restore"
      TabPicture(1)   =   "frmArchive.frx":08E6
      Tab(1).ControlEnabled=   0   'False
      Tab(1).Control(0)=   "txtRestoreFile"
      Tab(1).Control(0).Enabled=   0   'False
      Tab(1).Control(1)=   "cboBasinsProject"
      Tab(1).Control(1).Enabled=   0   'False
      Tab(1).Control(2)=   "fraManifest"
      Tab(1).Control(2).Enabled=   0   'False
      Tab(1).Control(3)=   "fraButtons(1)"
      Tab(1).Control(3).Enabled=   0   'False
      Tab(1).Control(4)=   "Label2"
      Tab(1).Control(4).Enabled=   0   'False
      Tab(1).Control(5)=   "lblRestoreFile"
      Tab(1).Control(5).Enabled=   0   'False
      Tab(1).Control(6)=   "lblBasinsProjectR"
      Tab(1).Control(6).Enabled=   0   'False
      Tab(1).ControlCount=   7
      TabCaption(2)   =   "&View/Compare"
      TabPicture(2)   =   "frmArchive.frx":0902
      Tab(2).ControlEnabled=   0   'False
      Tab(2).Control(0)=   "agdCompare"
      Tab(2).Control(1)=   "cmdCompare"
      Tab(2).Control(2)=   "cmdUncompare"
      Tab(2).Control(3)=   "chkDetails(0)"
      Tab(2).Control(4)=   "chkDetails(1)"
      Tab(2).Control(5)=   "cmdDetails"
      Tab(2).ControlCount=   6
      TabCaption(3)   =   "&Build"
      TabPicture(3)   =   "frmArchive.frx":091E
      Tab(3).ControlEnabled=   0   'False
      Tab(3).Control(0)=   "atxProjectName"
      Tab(3).Control(0).Enabled=   0   'False
      Tab(3).Control(1)=   "lstFolders"
      Tab(3).Control(1).Enabled=   0   'False
      Tab(3).Control(2)=   "fraButtons(2)"
      Tab(3).Control(2).Enabled=   0   'False
      Tab(3).Control(3)=   "lblNewBASINSprojectName"
      Tab(3).Control(3).Enabled=   0   'False
      Tab(3).Control(4)=   "Label3"
      Tab(3).Control(4).Enabled=   0   'False
      Tab(3).ControlCount=   5
      Begin VB.Frame fraButtons 
         BorderStyle     =   0  'None
         Height          =   615
         Index           =   0
         Left            =   120
         TabIndex        =   23
         Top             =   3840
         Width           =   6255
         Begin VB.CheckBox chkCompress 
            Caption         =   "Compress archive with &gzip"
            Height          =   255
            Left            =   240
            TabIndex        =   25
            ToolTipText     =   "Create a smaller archive.tar.gz rather than an uncompressed archive.tar"
            Top             =   120
            Value           =   1  'Checked
            Visible         =   0   'False
            Width           =   2655
         End
         Begin VB.CommandButton cmdArchive 
            Caption         =   "Archive"
            BeginProperty Font 
               Name            =   "MS Sans Serif"
               Size            =   7.8
               Charset         =   0
               Weight          =   700
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   615
            Left            =   3360
            TabIndex        =   24
            Top             =   0
            Width           =   1215
         End
      End
      Begin ATCoCtl.ATCoText atxProjectName 
         Height          =   255
         Left            =   -71520
         TabIndex        =   19
         Top             =   3360
         Width           =   4095
         _ExtentX        =   7218
         _ExtentY        =   445
         InsideLimitsBackground=   16777215
         OutsideHardLimitBackground=   8421631
         OutsideSoftLimitBackground=   8454143
         HardMax         =   -999
         HardMin         =   -999
         SoftMax         =   -999
         SoftMin         =   -999
         MaxWidth        =   -999
         Alignment       =   0
         DataType        =   0
         DefaultValue    =   ""
         Value           =   ""
         Enabled         =   -1  'True
      End
      Begin VB.ListBox lstFolders 
         Height          =   2580
         IntegralHeight  =   0   'False
         ItemData        =   "frmArchive.frx":093A
         Left            =   -71520
         List            =   "frmArchive.frx":093C
         TabIndex        =   17
         Top             =   600
         Width           =   4095
      End
      Begin VB.CommandButton cmdDetails 
         Caption         =   "Details"
         Enabled         =   0   'False
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   7.8
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   375
         Left            =   -72120
         TabIndex        =   15
         Top             =   480
         Width           =   975
      End
      Begin VB.CheckBox chkDetails 
         Caption         =   "Date"
         Height          =   375
         Index           =   1
         Left            =   -70200
         TabIndex        =   14
         Top             =   480
         Value           =   1  'Checked
         Width           =   735
      End
      Begin VB.CheckBox chkDetails 
         Caption         =   "Size"
         Height          =   375
         Index           =   0
         Left            =   -70920
         TabIndex        =   13
         Top             =   480
         Width           =   735
      End
      Begin VB.CommandButton cmdUncompare 
         Caption         =   "Remove"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   7.8
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   375
         Left            =   -73680
         TabIndex        =   12
         Top             =   480
         Width           =   1335
      End
      Begin VB.CommandButton cmdCompare 
         Caption         =   "Add..."
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   7.8
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   375
         Left            =   -74880
         TabIndex        =   11
         Top             =   480
         Width           =   975
      End
      Begin ATCoCtl.ATCoGrid agdCompare 
         Height          =   3615
         Left            =   -74880
         TabIndex        =   10
         Top             =   960
         Width           =   7575
         _ExtentX        =   13356
         _ExtentY        =   6371
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
         Header          =   ""
         FixedRows       =   1
         FixedCols       =   1
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
      Begin VB.TextBox txtRestoreFile 
         Height          =   285
         Left            =   -71520
         TabIndex        =   5
         Top             =   600
         Width           =   4215
      End
      Begin VB.ComboBox cboBasinsProject 
         Height          =   2655
         IntegralHeight  =   0   'False
         Left            =   -71520
         Style           =   1  'Simple Combo
         TabIndex        =   8
         Top             =   960
         Width           =   4215
      End
      Begin VB.ListBox lstBasinsProject 
         Height          =   1380
         IntegralHeight  =   0   'False
         ItemData        =   "frmArchive.frx":093E
         Left            =   3480
         List            =   "frmArchive.frx":0940
         TabIndex        =   3
         Top             =   600
         Width           =   4215
      End
      Begin VB.Frame fraManifest 
         Caption         =   "Archive File Details"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   7.8
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   1815
         Left            =   -74880
         TabIndex        =   21
         Top             =   1800
         Width           =   3255
         Begin VB.Label lblManifest 
            Caption         =   "No Archive File Selected"
            Height          =   1335
            Left            =   120
            TabIndex        =   22
            Top             =   360
            Width           =   3000
         End
      End
      Begin VB.Frame fraButtons 
         BorderStyle     =   0  'None
         Height          =   615
         Index           =   1
         Left            =   -74880
         TabIndex        =   26
         Top             =   3840
         Width           =   4695
         Begin VB.CommandButton cmdRestore 
            Caption         =   "Restore"
            BeginProperty Font 
               Name            =   "MS Sans Serif"
               Size            =   7.8
               Charset         =   0
               Weight          =   700
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   615
            Left            =   3360
            TabIndex        =   30
            Top             =   0
            Width           =   1215
         End
         Begin VB.CommandButton cmdView 
            Caption         =   "View Archive"
            BeginProperty Font 
               Name            =   "MS Sans Serif"
               Size            =   7.8
               Charset         =   0
               Weight          =   700
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   615
            Left            =   1320
            TabIndex        =   29
            Top             =   0
            Visible         =   0   'False
            Width           =   1695
         End
      End
      Begin VB.Frame fraButtons 
         BorderStyle     =   0  'None
         Height          =   615
         Index           =   2
         Left            =   -74880
         TabIndex        =   27
         Top             =   3840
         Width           =   4695
         Begin VB.CommandButton cmdBuild 
            Caption         =   "Build"
            BeginProperty Font 
               Name            =   "MS Sans Serif"
               Size            =   7.8
               Charset         =   0
               Weight          =   700
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   615
            Left            =   3360
            TabIndex        =   28
            Top             =   0
            Width           =   1215
         End
      End
      Begin VB.Frame fraInclude 
         BorderStyle     =   0  'None
         Height          =   1575
         Left            =   120
         TabIndex        =   31
         Top             =   2040
         Width           =   6255
         Begin VB.CheckBox chkDataProject 
            Caption         =   "data\{BASINSProject}"
            Height          =   255
            Left            =   3360
            TabIndex        =   36
            Tag             =   "data\"
            Top             =   360
            Width           =   2775
         End
         Begin VB.CheckBox chkModelout 
            Caption         =   "modelout"
            Height          =   255
            Left            =   3360
            TabIndex        =   35
            Tag             =   "modelout\"
            Top             =   960
            Width           =   2775
         End
         Begin VB.CheckBox chkExtensions 
            Caption         =   "etc\extensions"
            Height          =   255
            Left            =   3360
            TabIndex        =   34
            Top             =   0
            Width           =   2295
         End
         Begin VB.CheckBox chkSubdirs 
            Caption         =   "include subdirectories"
            Height          =   255
            Index           =   1
            Left            =   3600
            TabIndex        =   33
            Top             =   1200
            Width           =   2415
         End
         Begin VB.CheckBox chkSubdirs 
            Caption         =   "include subdirectories"
            Height          =   255
            Index           =   0
            Left            =   3600
            TabIndex        =   32
            Top             =   600
            Width           =   2415
         End
         Begin VB.Label lblAllFiles 
            Alignment       =   1  'Right Justify
            Caption         =   "Include all files from:"
            BeginProperty Font 
               Name            =   "MS Sans Serif"
               Size            =   7.8
               Charset         =   0
               Weight          =   700
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Left            =   1080
            TabIndex        =   38
            Top             =   360
            Width           =   2055
         End
         Begin VB.Label lblExtensions 
            Alignment       =   1  'Right Justify
            Caption         =   "Include referenced &extensions from:"
            BeginProperty Font 
               Name            =   "MS Sans Serif"
               Size            =   7.8
               Charset         =   0
               Weight          =   700
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Left            =   0
            TabIndex        =   37
            Top             =   0
            Width           =   3135
         End
      End
      Begin VB.Label lblNewBASINSprojectName 
         Alignment       =   1  'Right Justify
         Caption         =   "New BASINS Project Name:"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   7.8
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   375
         Left            =   -74880
         TabIndex        =   18
         Top             =   3360
         Width           =   3015
      End
      Begin VB.Label Label3 
         Alignment       =   1  'Right Justify
         Caption         =   "Select BASINS Project Folder from which to Build the New Project:"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   7.8
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   492
         Left            =   -74880
         TabIndex        =   16
         Top             =   600
         Width           =   3012
      End
      Begin VB.Label Label2 
         Alignment       =   1  'Right Justify
         Caption         =   "Or write to existing BASINS Project:"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   7.8
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   375
         Left            =   -74880
         TabIndex        =   7
         Top             =   1440
         Width           =   3135
      End
      Begin VB.Label Label1 
         Alignment       =   1  'Right Justify
         Caption         =   "Select BASINS &Project to Archive:"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   7.8
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   255
         Left            =   120
         TabIndex        =   2
         Top             =   600
         Width           =   3015
      End
      Begin VB.Label lblRestoreFile 
         Alignment       =   1  'Right Justify
         Caption         =   "Archive File to Restore &From:"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   7.8
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   255
         Left            =   -74760
         TabIndex        =   4
         Top             =   600
         Width           =   3015
      End
      Begin VB.Label lblBasinsProjectR 
         Alignment       =   1  'Right Justify
         Caption         =   "Restore as new BASINS &Project:"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   7.8
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   255
         Left            =   -74760
         TabIndex        =   6
         Top             =   1005
         Width           =   3015
      End
   End
   Begin MSComDlg.CommonDialog cdlFile 
      Left            =   1440
      Top             =   360
      _ExtentX        =   847
      _ExtentY        =   847
      _Version        =   393216
   End
   Begin VB.Label lblAbout 
      Caption         =   "This tool creates, restores, and compares archives of BASINS projects"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   735
      Left            =   2160
      TabIndex        =   9
      Top             =   240
      Width           =   3495
   End
   Begin VB.Label lblDrive 
      Caption         =   "BASINS &Drive:"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   255
      Left            =   240
      TabIndex        =   0
      Top             =   240
      Width           =   1575
   End
   Begin VB.Menu mnuFile 
      Caption         =   "&File"
      Begin VB.Menu mnuExit 
         Caption         =   "E&xit"
      End
   End
   Begin VB.Menu mnuHelp 
      Caption         =   "&Help"
      Begin VB.Menu mnuContents 
         Caption         =   "&Contents and Index"
      End
      Begin VB.Menu mnuAbout 
         Caption         =   "&About"
      End
      Begin VB.Menu mnuFeedback 
         Caption         =   "Send &Feedback"
      End
   End
End
Attribute VB_Name = "frmArchive"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Private Const PROJECT_PLACEHOLDER = "~"
Private Const NoArchiveLabel = "No Archive File Selected"

Private pBaseCaption As String
Private pOpenArchives As FastCollection
Private pModelOutDirs As FastCollection

'Private RestoreTar As clsTar

Private Sub Status(newValue As String)
  If Len(newValue) = 0 Then
    Me.Caption = pBaseCaption
  Else
    Me.Caption = pBaseCaption & " - " & newValue
  End If
  pLogger.Log "Status:" & Me.Caption
  Me.Refresh
End Sub

Private Sub agdCompare_RowColChange()
'  Dim extension As String
'  extension = LCase(FileExt(agdCompare.TextMatrix(agdCompare.row, 0)))
'  Select Case extension
'    Case "shp", "shx": cmdMap.Enabled = True
'    Case Else:         cmdMap.Enabled = False
'  End Select
End Sub

Private Sub chkDetails_Click(Index As Integer)
  Dim col As Long
  Me.MousePointer = vbHourglass
  For col = 1 To agdCompare.cols - 1
    AddCompare agdCompare.ColTitle(col)
  Next
  SetCompareColors
  Me.MousePointer = vbDefault
End Sub

Private Sub chkDataProject_Click()
  If chkDataProject.Value = vbUnchecked Then
    chkSubdirs(0).Value = vbUnchecked
  End If
End Sub

Private Sub chkModelout_Click()
  If chkModelout.Value = vbUnchecked Then
    chkSubdirs(1).Value = vbUnchecked
  End If
End Sub

Private Sub chkSubdirs_Click(Index As Integer)
  If chkSubdirs(Index).Value = vbChecked Then
    If Index = 0 Then chkDataProject.Value = vbChecked
    If Index = 1 Then chkModelout.Value = vbChecked
  End If
End Sub

Private Function OpenArchive(filename As String) As clsTar
  Dim i As Long
  Status "Opening Archive: " & filename
  i = pOpenArchives.IndexFromKey(LCase(filename))
  If i > 0 Then
    Set OpenArchive = pOpenArchives.ItemByIndex(i)
  Else
    Set OpenArchive = New clsTar
    OpenArchive.TarFilename = filename
    pOpenArchives.Add OpenArchive, LCase(filename)
  End If
  Status "Opened Archive: " & filename
  pLogger.Log "Opened Archive: " & filename & " (now have " & pOpenArchives.Count & " open)"
End Function

Private Sub cmbDrives_Click()
  refreshBasinsProjectList 0
  refreshBasinsProjectList 1
  refreshBasinsProjectList 3
End Sub

Private Sub cmdArchive_Click()
  Dim mytar As clsTar
  Dim txtArchiveFile As String
  Dim fName As String
  Dim i As Long
  Dim manifest As String
  Dim AllFiles As FastCollection
  Dim vFilename As Variant
  Dim vOutDir As Variant
  Dim BasinsProjectName As String
  
  BasinsProjectName = lstBasinsProject.List(lstBasinsProject.ListIndex)

  On Error GoTo ErrHandler

  With cdlFile
    .InitDir = BasinsDrive & "basins\apr"
    .DialogTitle = "Select BASINS Archive File"
    .Filter = "BASINS Archives (*.tar,*.tar.gz)|*.tar;*.tar.gz|Tar (*.tar)|*.tar|Tar GNU Zip (*.tar.gz)|*.tar.gz"
    .CancelError = True
    .FilterIndex = 1
'    If chkCompress.Value = vbChecked Then
      .filename = BasinsProjectName & ".tar.gz"
      .DefaultExt = ".tar.gz"
'    Else
'      .filename = BasinsProjectName & ".tar"
'      .DefaultExt = ".tar"
'    End If
    On Error GoTo NoAction
    .ShowSave
    On Error GoTo ErrHandler
'    If chkCompress.Value = vbChecked Then
'      If LCase(Right(.filename, 3)) <> ".gz" Then .filename = .filename & ".gz"
'    End If
    If FileExists(.filename) Then
      If MsgBox("BASINS Archive " & .filename & " already exists, overwrite it?", _
        vbYesNo, "Archive") = vbNo Then GoTo NoAction
    End If
    
    If LCase(Right(.filename, 3)) = ".gz" Then
      txtArchiveFile = Left(.filename, Len(.filename) - 3)
      If FileExists(txtArchiveFile) Then
        If MsgBox("BASINS Archive " & txtArchiveFile & " already exists, overwrite it?", _
          vbYesNo, "Archive") = vbNo Then GoTo NoAction
      End If
    Else
      If FileExists(txtArchiveFile & ".gz") Then
        If MsgBox("BASINS Archive " & txtArchiveFile & ".gz" & " already exists, overwrite it?", _
          vbYesNo, "Archive") = vbNo Then GoTo NoAction
      End If
    End If
    txtArchiveFile = .filename
  End With
  
  If Len(txtArchiveFile) = 0 Then
    pLogger.LogMsg "Need to specify an Archive File", "Archive"
  Else
    Me.MousePointer = vbHourglass
    DoEvents
    Status "Removing existing archives"
    On Error Resume Next
    Kill txtArchiveFile
    If LCase(Right(txtArchiveFile, 3)) = ".gz" Then
      Kill Left(txtArchiveFile, Len(txtArchiveFile) - 3)
'      txtArchiveFile = Left(txtArchiveFile, Len(txtArchiveFile) - 3)
'      Kill txtArchiveFile
    Else
      Kill txtArchiveFile & ".gz"
    End If
    On Error GoTo ErrHandler
    
    Status "Opening " & txtArchiveFile
    Set mytar = OpenArchive(txtArchiveFile)
    manifest = "ProjectName: " & BasinsProjectName & vbCrLf
    manifest = manifest & "FromDrive: " & Left(CurDir, 2) & vbCrLf
    manifest = manifest & "CreatedAt: " & Now & vbCrLf
    manifest = manifest & "CreatedBy: " & APIUserName & vbCrLf
    manifest = manifest & "CreatedOn: " & APIComputerName & vbCrLf
    
    'Write to a temporary file to get a current time stamp
    fName = GetTmpFileName
    SaveFileString fName, manifest
    mytar.AppendFileFromDisk fName, "manifest.txt"
    Kill fName
    
    fName = BasinsDrive & "\basins\apr\" & BasinsProjectName & ".apr"
    Status "Reading " & FilenameNoPath(fName)
    'myTar.AppendFileFromDisk fname, "apr\" & fname
    
    Set AllFiles = AllFilesInAPR(fName)
    
    If chkDataProject.Value = vbChecked Then
      If chkSubdirs(0).Value = vbChecked Then
        AddFilesInDir AllFiles, cmbDrives.Text & "basins\" & chkDataProject.Caption, True
      Else
        AddFilesInDir AllFiles, cmbDrives.Text & "basins\" & chkDataProject.Caption, False
      End If
    End If
    
    If chkModelout.Value = vbChecked Then
      For Each vOutDir In pModelOutDirs
        If chkSubdirs(1).Value = vbChecked Then
          AddFilesInDir AllFiles, (vOutDir), True
        Else
          AddFilesInDir AllFiles, (vOutDir), False
        End If
      Next vOutDir
    End If
    For Each vFilename In AllFiles
      fName = vFilename
      If FileExists(fName) Then
        Status "Appending " & vFilename
        If mytar.AppendFileFromDisk(fName, Mid(vFilename, 11)) Then 'Save with "X:\Basins\" trimmed
          'pLogger.Log "Appended " & fname 'status produces message too
        Else
          pLogger.Log "Did not append " & fName & " - " & mytar.ErrorDescription
        End If
      Else
        pLogger.Log "File not found: " & fName
      End If
    Next
    Status "Compressing archive"
    mytar.Flush
    Set mytar = Nothing
    Status "Finished Archiving"
  End If
NoAction:
  Me.MousePointer = vbNormal
  Exit Sub
ErrHandler:
  pLogger.LogMsg Err.Description, "Archive"
  Me.MousePointer = vbNormal
End Sub

Private Function AllFilesInAPR(APRfilename As String) As FastCollection
  Const fileStart As String = ":/basins/"
  Const fileStartO As String = """/basins/" 'an original basins project - no disk letter
  Dim retval As New FastCollection
  Dim apr As String  'contents of .apr file
  Dim lapr As String 'Lcase(apr)
  Dim auxShape As String
  Dim nextStart As Long, nextStartO As Long, nextStop As Long, nextStopQ As Long
  Dim fName As String
  Dim path As String
  Dim BasinsFolderName As String
  Dim i As Long
  Dim ilen As Long
  
  Status "In AllFilesInAPR: Reading " & APRfilename
  apr = WholeFileString(APRfilename)
  lapr = LCase(apr) 'dont want file names to be case sensitive
  
  retval.Add APRfilename, LCase(APRfilename)
  
  BasinsFolderName = FilenameOnly(APRfilename)
  i = InStr(1, lapr, "/basins/data/") + Len("/basins/data/")
  ilen = InStr(i + 1, lapr, "/") - i
  If i > 0 Then
    BasinsFolderName = Mid(lapr, i, ilen)
  End If
  Status "In AllFilesInAPR: BasinsFolderName " & BasinsFolderName

  fName = BasinsDrive & "basins\data\" & BasinsFolderName & "\prj.odb"
  If FileExists(fName) Then
    retval.Add fName, LCase(fName)
    Status "In AllFilesInAPR: Adding " & fName
  End If
  
  nextStop = 1
  GoSub FindNextStart
  While nextStart > 0
    nextStop = InStr(nextStart, lapr, "\n")
    nextStopQ = InStr(nextStart, lapr, """")
    If (nextStop > nextStopQ And nextStopQ > 0) Or nextStop = 0 Then
      nextStop = nextStopQ
    End If
      
    fName = Mid(apr, nextStart, nextStop - nextStart)
    fName = ReplaceString(fName, "/", "\")
    If Mid(fName, 2, 1) <> ":" Then
      fName = BasinsDrive & fName
    End If
    path = PathNameOnly(fName)
    If FileExists(fName) Then
      If FileExt(fName) = "shp" Then
        auxShape = Dir(FilenameNoExt(fName) & ".sh*")
      Else
        auxShape = Dir(fName)
      End If
      
      While Len(auxShape) > 0
        fName = path & "\" & auxShape
        If chkExtensions = 0 And InStr(LCase(fName), "extensions") > 0 Then
          'skip
        ElseIf retval.KeyExists(LCase(fName)) Then
          'skip, already added
        Else
          retval.Add fName, LCase(fName)
          Status "In AllFilesInAPR: Adding " & fName
        End If
        auxShape = Dir 'more???
      Wend
    ElseIf (FileExt(fName) = "vat") Or ((FileExists(fName, True)) And FileExists(fName & "\hdr.adf")) Then
      'special case for arcinfo grids, get all files in 'path'
      If (FileExt(fName) <> "vat") Then
        path = fName
      End If
      auxShape = Dir(path & "/*.*")
      While Len(auxShape) > 0
        fName = path & "\" & auxShape
        If retval.KeyExists(LCase(fName)) Then
          'skip, already added
        Else
          retval.Add fName, LCase(fName)
          Status "In AllFilesInAPR: Adding " & fName
        End If
        auxShape = Dir 'more???
      Wend
    End If
    GoSub FindNextStart
  Wend
  Set AllFilesInAPR = retval
  Status "In AllFilesInAPR: Exiting"
  Exit Function

FindNextStart:
  nextStart = InStr(nextStop, lapr, fileStart) - 1
  nextStartO = InStr(nextStop, lapr, fileStartO) + 1
  If nextStartO < nextStart And nextStartO > 1 Then
    nextStart = nextStartO
  End If
  Return
End Function

Private Sub cmdBuild_Click()
  Dim FolderName$, ProjectName$
  
  FolderName = cmbDrives.Text & "BASINS\data\" & lstFolders.List(lstFolders.ListIndex)
  ProjectName = atxProjectName.Value
  Status ""
  If BuildNewBasinsApr(FolderName, ProjectName) Then
    Status "Build Completed"
    refreshBasinsProjectList 0, True
    refreshBasinsProjectList 1, True
  End If
  'open apr?
End Sub

Public Function BuildNewBasinsApr(PathName$, ProjectName$) As Boolean
  
  Dim basins_dir As String
  Dim f As String
  Dim s As String
  Dim ipath As Long
  Dim ilen As Long
  Dim ts As String
  Dim istr As Long
  Dim iend As Long
  Dim ProjectFolder As String
  Dim fName As String
  Dim ipos As Long
  Dim shapetype As String
  Dim iresp As Long
  
  BuildNewBasinsApr = False
  If Len(Trim(ProjectName)) > 0 Then
    'build new apr
    basins_dir = Left(PathName, 9)
    ProjectFolder = Mid(PathName, Len("c:\basins\data\") + 1)
    If Mid(ProjectFolder, Len(ProjectFolder)) = "\" Then
      ProjectFolder = Mid(ProjectFolder, 1, Len(ProjectFolder) - 1)
    End If
    'copy base apr
    f = basins_dir & "\apr\" & ProjectName & ".apr"
    If FileExists(f) Then
      iresp = MsgBox("The BASINS project '" & f & "' already exists." & vbCrLf & "Do you want to overwrite it?", vbOKCancel, "BASINS Project Build")
    End If
    If iresp <> 2 Then
      FileCopy basins_dir & "\etc\build.dat", f
      'replace file paths
      s = WholeFileString(f)
      If LCase(ProjectFolder) <> "tutorial" Then
        s = ReplaceStringNoCase(s, "/tutorial/", "/" & ProjectFolder & "/")
      End If
      SaveFileString f, s
      'make sure all files exist, use dummys if not
      ipath = 1
      Do While ipath > 0
        ipath = InStr(ipath, s, "Path:")
        If ipath > 0 Then
          istr = InStr(ipath, s, """")
          iend = InStr(istr + 1, s, """")
          ts = Mid(s, istr + 1, iend - istr - 1)
          If Not FileExists(ts) Then
            If LCase(Right(ts, 3)) = "dbf" Then
              FileCopy basins_dir & "\data\dummy.dbf", ts
            ElseIf LCase(Right(ts, 3)) = "shp" Then
              'need to figure out if point, line, or polygon
              fName = FilenameNoPath(ReplaceString(ts, "/", "\"))
              ipos = InStr(1, s, fName)
              ipos = InStr(ipos, s, "SubName:")
              istr = InStr(ipos, s, """")
              iend = InStr(istr + 1, s, """")
              shapetype = Mid(s, istr + 1, iend - istr - 1)
              If shapetype = "Point" Then
                FileCopy basins_dir & "\data\dumpoint.shp", ts
                FileCopy basins_dir & "\data\dumpoint.dbf", Left(ts, Len(ts) - 3) & "dbf"
                FileCopy basins_dir & "\data\dumpoint.shx", Left(ts, Len(ts) - 3) & "shx"
              ElseIf shapetype = "Line" Or shapetype = "Arc" Then
                FileCopy basins_dir & "\data\dumline.shp", ts
                FileCopy basins_dir & "\data\dumline.dbf", Left(ts, Len(ts) - 3) & "dbf"
                FileCopy basins_dir & "\data\dumline.shx", Left(ts, Len(ts) - 3) & "shx"
              ElseIf shapetype = "Polygon" Then
                FileCopy basins_dir & "\data\dumpoly.shp", ts
                FileCopy basins_dir & "\data\dumpoly.dbf", Left(ts, Len(ts) - 3) & "dbf"
                FileCopy basins_dir & "\data\dumpoly.shx", Left(ts, Len(ts) - 3) & "shx"
              End If
            End If
          End If
          ipath = ipath + 1
        End If
      Loop
      BuildNewBasinsApr = True
    End If
  End If

End Function

Private Sub cmdDetails_Click()
  Dim row As Long
  Dim col As Long
  Dim ProjectName As String
  Dim projectType() As String
  Dim filename As String
  Dim realFilenames As FastCollection
  Dim label As String
  Dim newMap As New frmMap
  Dim mytar As clsTar
  Dim tarProjectName As String
  Dim tarIndex As Long
  Dim tmpIndex As Long
  
  With agdCompare
    If .cols > 1 Then
      ReDim projectType(1 To .cols - 1)
      For col = 1 To .cols - 1
        projectType(col) = LCase(FileExt(.ColTitle(col)))
      Next
      For row = 1 To .Rows
        label = .TextMatrix(row, 0)
        Select Case LCase(FileExt(label))
          Case "shx"     'Never add shx files to detail window
            GoTo NextRow
          Case "dbf"     'Don't add DBF if it is part of a shape file
            If agdCompare.RowContaining(ReplaceString(label, "dbf", "shp"), 0) > 0 Then GoTo NextRow
        End Select
        
        Set realFilenames = New FastCollection
        For col = 1 To .cols - 1
          ProjectName = .ColTitle(col)
          filename = ""
          Select Case projectType(col)
            Case "apr"
              filename = Left(ProjectName, 10) & ReplaceStringNoCase(label, PROJECT_PLACEHOLDER, FilenameOnly(ProjectName))
            Case "tar", "gz"
              Set mytar = OpenArchive(ProjectName)
              tarProjectName = mytar.ArchiveFilenames.ItemByIndex(1)
              If LCase(FileExt(tarProjectName)) = "apr" Then
                ProjectName = tarProjectName
              Else
                tarProjectName = mytar.ArchiveFilenames.ItemByIndex(2)
                If LCase(FileExt(tarProjectName)) = "apr" Then
                  ProjectName = tarProjectName
                End If
              End If
              ProjectName = ReplaceString(ProjectName, "/", "\")
              tarIndex = mytar.FileIndexByName(ReplaceString(ReplaceStringNoCase(label, PROJECT_PLACEHOLDER, FilenameOnly(ProjectName)), "\", "/"))
              If tarIndex > 0 Then
                filename = GetTmpPath & "\" & TEMP_PREFIX & "0_" & FilenameNoPath(label)
                For tmpIndex = 1 To 9
                  If FileExists(filename) Then Exit For
                  filename = GetTmpPath & "\" & TEMP_PREFIX & tmpIndex & "_" & FilenameNoPath(label)
                Next
                pLogger.Log "Restoring " & label & " to " & filename
                TempFiles.Add filename
                mytar.ExtractFile tarIndex, PathNameOnly(filename), FilenameNoPath(filename)
              End If
          End Select
          If Not FileExists(filename) Then filename = ""
          realFilenames.Add filename
        Next
        If .row = row And row < .Rows Then
          newMap.Add label, realFilenames, True
        Else
          newMap.Add label, realFilenames, False
        End If
NextRow:
      Next
      newMap.Show
    End If
  End With
End Sub

Private Sub cmdRestore_Click()
  Dim i As Long, f As String, s As String
  Dim mytar As clsTar
  Dim oldProject As String, oldDrive As String
  Dim ArchiveFilenames As FastCollection
  Dim ConflictingFiles As New FastCollection
  Dim NonConflictingFiles As New FastCollection
  Dim wasCompressed As Boolean
  Dim extension As String
  Dim manifest As String
  Dim BasinsProjectName As String
  Dim AllNone As String
  Dim addslash As String
  
  Me.MousePointer = vbHourglass
  DoEvents
  On Error GoTo ErrHandler
  
  If Not FileExists(txtRestoreFile) Then
    If Len(txtRestoreFile.Text) = 0 Then
      pLogger.LogMsg "Archive File to Restore From was not specified", "Restore"
    Else
      pLogger.LogMsg "Archive File to Restore From: " & vbCr & "'" & txtRestoreFile & "' not found.", "Restore"
    End If
  Else
    BasinsProjectName = cboBasinsProject.Text
    
    If Not FileExists(BasinsDrive & "\basins\apr\" & BasinsProjectName & ".apr") Then
      If MsgBox("BASINS Project " & BasinsProjectName & " does not exist, create it?", _
                                                                    vbYesNo) = vbNo Then
        Status "Restore Cancelled"
        GoTo SkipIt:
      End If
    End If
    'do the restore
    DoEvents
    
'    If LCase(Right(txtRestoreFile, 3)) = ".gz" Then
'      wasCompressed = True
'      Status "Uncompressing " & txtRestoreFile
'      Shell GzipPath & " -d """ & txtRestoreFile & """"
'      While FileExists(txtRestoreFile)
'        DoEvents
'        Sleep 50
'      Wend
'      txtRestoreFile = Left(txtRestoreFile, Len(txtRestoreFile) - 3)
'    End If
    
    Set mytar = OpenArchive(txtRestoreFile)
    Status "Reading Archive Filenames from " & txtRestoreFile
    Set ArchiveFilenames = mytar.ArchiveFilenames
    If ArchiveFilenames(1) = "manifest.txt" Then
      manifest = mytar.FileAsString(1)
      oldProject = StrSplit(Mid(manifest, InStr(manifest, "ProjectName: ") + 13), vbCr, "")
      oldDrive = Mid(manifest, InStr(manifest, "FromDrive: ") + 11, 2)
      lblManifest.Caption = manifest
    Else
      oldProject = FilenameOnly(ReplaceString(ArchiveFilenames(1), "/", "\"))
      lblManifest.Caption = NoArchiveLabel
    End If
    Status "Checking for conflicting files in archive"
    For i = 1 To ArchiveFilenames.Count
      If ArchiveFilenames(i) <> "manifest.txt" Then
        If Left(ArchiveFilenames(i), 1) <> "\" Then
          addslash = "\"
        Else
          addslash = ""
        End If
        f = BasinsDrive & "\basins" & addslash & ReplaceString(ArchiveFilenames(i), "/", "\")
        f = ReplaceStringNoCase(f, "\" & oldProject & ".apr", "\" & BasinsProjectName & ".apr")
        f = ReplaceStringNoCase(f, "\" & oldProject & "\", "\" & BasinsProjectName & "\")
        If FileExists(f) Then
          ConflictingFiles.Add f, (i)
        Else
          NonConflictingFiles.Add f, (i)
        End If
      End If
    Next
    If ConflictingFiles.Count > 0 Then
      Set ConflictingFiles = frmConflicts.SelectFiles(ConflictingFiles)
      If frmConflicts.Cancelled Then
        Status "Restore Cancelled"
        Unload frmConflicts
        GoTo CloseTar
      End If
      Unload frmConflicts
    End If
    For i = 1 To ArchiveFilenames.Count
      DoEvents
      If ArchiveFilenames(i) <> "manifest.txt" Then
        f = BasinsDrive & "\basins\" & ReplaceString(ArchiveFilenames(i), "/", "\")
        f = ReplaceStringNoCase(f, "\" & oldProject & ".apr", "\" & BasinsProjectName & ".apr")
        f = ReplaceStringNoCase(f, "\" & oldProject & "\", "\" & BasinsProjectName & "\")
        If NonConflictingFiles.KeyExists((i)) Or ConflictingFiles.KeyExists((i)) Then
          MkDirPath (PathNameOnly(f))
          Status "Restoring " & f
          mytar.ExtractFile i, "", f, AllNone
          Select Case AllNone
            Case "none": Status "Restore Cancelled"
                         GoTo CloseTar
            Case "skip": pLogger.Log "Skipped file: " & f
            Case Else: 'file was restored as expected (possibly overwriting an existing file)
              extension = LCase(FileExt(f))
              Select Case extension
                Case "apr", "odb"
                  s = WholeFileString(f)
                  If extension = "apr" And Len(oldDrive) = 0 Then oldDrive = Mid(s, InStr(LCase(s), ":/basins") - 1, 2)
                  If LCase(BasinsDrive) <> LCase(oldDrive) Then
                    s = ReplaceStringNoCase(s, oldDrive & "/", BasinsDrive & "/") 'does this do too much?
                  End If
                  If LCase(BasinsProjectName) <> LCase(oldProject) Then
                    s = ReplaceStringNoCase(s, "/" & oldProject & "/", "/" & BasinsProjectName & "/")
                  End If
                  SaveFileString f, s
                Case "map", "sta", "src" 'various files may need file names listed inside edited
                  s = WholeFileString(f)
                  s = ReplaceStringNoCase(s, oldDrive & "\", BasinsDrive & "\")
                  s = ReplaceStringNoCase(s, "\" & oldProject & "\", "\" & BasinsProjectName & "\")
                  SaveFileString f, s
              End Select
              pLogger.Log "Restored file: " & f
          End Select
        Else
          pLogger.Log "Skipped file: " & f
        End If
      End If
    Next i
    Status "Finished Restoring"
CloseTar:
    Set mytar = Nothing
'    If wasCompressed Then
'      'Launch gzip, which may take some time to finish compressing if the tar file is large
'      Status "Re-compressing archive with gzip"
'      Shell GzipPath & " -6 """ & txtRestoreFile & """"
'      While FileExists(txtRestoreFile)
'        DoEvents
'        Sleep 50
'      Wend
'      txtRestoreFile = txtRestoreFile & ".gz"
'    End If
  End If
SkipIt:
  Me.MousePointer = vbNormal
  Exit Sub
  
ErrHandler:
  pLogger.Log Err.Description, "Restore"
End Sub

Private Sub cmdCompare_Click()
  On Error GoTo ErrHand
  Me.MousePointer = vbHourglass
  With cdlFile
    .InitDir = BasinsDrive & "\basins\apr"
    .DialogTitle = "Select BASINS Archive File or Project"
    .DefaultExt = ".apr"
    .Filter = "Projects and Archives (*.tar,*.tar.gz,*.apr)|*.tar;*.tar.gz;*.apr|Tar Archives (*.tar)|*.tar|Tar GNU Zip (*.tar.gz)|*.tar.gz|BASINS Projects (*.apr)|*.apr"
    .FilterIndex = 1
    .CancelError = True
    On Error GoTo SkipIt
    .ShowOpen
    On Error GoTo ErrHand
    If Len(.filename) > 0 Then
      If FileExists(.filename) Then
        AddCompare .filename
        SetCompareColors
      End If
    End If
  End With
SkipIt:
  Me.MousePointer = vbDefault
  Exit Sub
ErrHand:
  pLogger.Log Err.Description, "cmdCompare Error"
End Sub
  
Private Sub AddCompare(filename As String)
  Dim fIndex As Long
  Dim row As Long 'row in agdCompare
  Dim col As Long 'column in agdCompare
  Dim tmpFilenames As FastCollection
  Dim curFilenames As New FastCollection
  Dim details As New FastCollection
  Dim thisFileDetails As String
  Dim ProjectName As String
  
  On Error GoTo ErrHand
  
  For col = 1 To agdCompare.cols - 1
    If LCase(agdCompare.ColTitle(col)) = LCase(filename) Or _
         Len(agdCompare.ColTitle(col)) = 0 Then Exit For
  Next
  
  agdCompare.ColTitle(0) = "File"
  agdCompare.ColTitle(col) = filename
  agdCompare.ColAlignment(col) = 7

  Select Case LCase(FileExt(filename))
    Case "apr"
      ProjectName = FilenameOnly(filename)
      Set tmpFilenames = AllFilesInAPR(filename)
      For fIndex = 1 To tmpFilenames.Count
        filename = tmpFilenames.ItemByIndex(fIndex)
        thisFileDetails = ""
        If FileExists(filename) Then
          If chkDetails(0).Value = vbChecked Then thisFileDetails = Format(FileLen(filename), "###,###,###,###")
          If chkDetails(1).Value = vbChecked Then thisFileDetails = thisFileDetails & "  " & FileDateTime(filename)
          details.Add thisFileDetails
          'details.Add Format(FileLen(filename), "###,###,###,###")
        Else
          details.Add "File not found"
        End If
        filename = Mid(filename, 11)
        If Len(ProjectName) > 0 Then filename = ReplaceStringNoCase(filename, ProjectName, PROJECT_PLACEHOLDER)
        curFilenames.Add filename, LCase(filename)
      Next
    Case "tar", "gz"
      Dim mytar As clsTar
      Set mytar = OpenArchive(filename)
      Set tmpFilenames = mytar.ArchiveFilenames
      For fIndex = 1 To tmpFilenames.Count
        thisFileDetails = ""
        If chkDetails(0).Value = vbChecked Then thisFileDetails = Format(mytar.FileSize(fIndex), "###,###,###,###")
        If chkDetails(1).Value = vbChecked Then thisFileDetails = thisFileDetails & "  " & mytar.FileDate(fIndex)
        details.Add thisFileDetails
        
        filename = ReplaceString(tmpFilenames.ItemByIndex(fIndex), "/", "\")
        If LCase(FileExt(filename)) = "apr" Then ProjectName = FilenameOnly(filename)
        If Len(ProjectName) > 0 Then filename = ReplaceStringNoCase(filename, ProjectName, PROJECT_PLACEHOLDER)
        curFilenames.Add filename, LCase(filename)
      Next
  End Select
  
  With agdCompare
    For fIndex = 1 To curFilenames.Count
      filename = curFilenames.key(fIndex)
      For row = 1 To agdCompare.Rows
        If LCase(.TextMatrix(row, 0)) = filename Then GoTo SetDetails
      Next
      'Add a new row if file was not found in grid
      If row = 2 And agdCompare.TextMatrix(1, 0) = "" Then row = 1
      .TextMatrix(row, 0) = curFilenames.ItemByIndex(fIndex)
SetDetails:
      .TextMatrix(row, col) = details.ItemByIndex(fIndex)
    Next
    .ColsSizeByContents
  End With
  Exit Sub
ErrHand:
  pLogger.LogMsg Err.Description, "Compare Error"
End Sub

Private Sub SetCompareColors()
  Dim foundMismatch As Boolean
  Dim row As Long
  Dim col As Long
  
  With agdCompare
    For row = 1 To .Rows
      .row = row
      foundMismatch = False
      For col = 2 To .cols - 1
        If .TextMatrix(row, col) <> .TextMatrix(row, 1) Then foundMismatch = True: Exit For
      Next
      For col = 0 To .cols - 1
        .col = col
        If foundMismatch Then
          .CellForeColor = vbRed
        Else
          .CellForeColor = .ForeColor
        End If
      Next
    Next
    If .cols > 1 Then cmdDetails.Enabled = True Else cmdDetails.Enabled = False
  End With
End Sub

Private Sub cmdUncompare_Click()
  Dim r As Long
  Dim c As Long
  Dim rememberCols As New FastCollection
  With agdCompare
    If .cols > 1 And .col > 0 Then
      For c = 1 To .cols - 1
        If c <> .col Then rememberCols.Add .ColTitle(c)
      Next
      .cols = 1
      .Rows = 0
      For c = 1 To rememberCols.Count
        AddCompare rememberCols.ItemByIndex(c)
      Next
      
'      If .col < .cols - 1 Then
'        'move contents of farther right column(s) left one
'        For c = .col To .cols - 1
'          For r = 0 To .Rows
'            .TextMatrix(r, c) = .TextMatrix(r, c + 1)
'          Next
'        Next
'      End If
'      .cols = .cols - 1
'      If .cols = 1 Then 'just files left
'        .Rows = 0
'      End If
    End If
  End With
  SetCompareColors
End Sub

Private Sub cmdView_Click()
  Dim mytar As clsTar
  Dim ArchiveFilenames As FastCollection
  Dim Summary As String
  Dim i As Long
  
  If FileExists(txtRestoreFile) Then
    Set mytar = OpenArchive(txtRestoreFile)
    Status "Reading Archive Filenames from " & txtRestoreFile
    Set ArchiveFilenames = mytar.ArchiveFilenames
  
    For i = 1 To ArchiveFilenames.Count
      Summary = Summary & ArchiveFilenames.ItemByIndex(i) & vbTab & Format(mytar.FileSize(i), "###,###,### bytes") & vbTab & mytar.FileDate(i) & vbCrLf
    Next
    Set mytar = Nothing
    Set ArchiveFilenames = Nothing
  Else
    Summary = lblRestoreFile.Caption & " '" & txtRestoreFile & "' not found"
  End If
  frmTextBox.Caption = "Contents of '" & txtRestoreFile & "'"
  frmTextBox.txt = Summary
  frmTextBox.Show
End Sub

Private Sub Form_Load()
  Dim curDrive As Variant
  Dim allDrives As Collection
  
  On Error GoTo ErrHand
  
  Set allDrives = GetLogicalDriveStringsAsCollection
  lblManifest.Caption = NoArchiveLabel
  
  For Each curDrive In allDrives
    If (GetDriveType(CStr(curDrive))) = DRIVE_FIXED Then
      If FileExists(curDrive & "\Basins", True, False) Then
        cmbDrives.AddItem curDrive
        If Not FileExists(App.HelpFile) Then
          If FileExists(curDrive & "\Basins\Docs\BASINS3.1.chm") Then
            App.HelpFile = curDrive & "\Basins\Docs\BASINS3.1.chm"
          End If
        End If
      End If
    End If
  Next

  If cmbDrives.ListCount = 0 Then
    pLogger.LogMsg "BASINS is not installed on this computer", "Cannot run without a BASINS folder"
    cmbDrives.Enabled = False
    tabMain.Enabled = False
    cmdArchive.Enabled = False
    chkExtensions.Enabled = False
    chkDataProject.Enabled = False
    chkModelout.Enabled = False
    chkSubdirs(0).Enabled = False
    chkSubdirs(1).Enabled = False
    chkCompress.Enabled = False
    cmdView.Enabled = False
    cmdRestore.Enabled = False
    cmdCompare.Enabled = False
    cmdUncompare.Enabled = False
    cmdDetails.Enabled = False
    cmdBuild.Enabled = False
  Else
    cmbDrives.ListIndex = 0
    refreshBasinsProjectList 0, True
    refreshBasinsProjectList 1, True
    refreshBasinsProjectList 3, True
    tabMain.Tab = 0
  End If
  
  pBaseCaption = Me.Caption
  
  Set pOpenArchives = New FastCollection
  
  Exit Sub

ErrHand:
  pLogger.LogMsg Err.Description, "Archive Form Load"
End Sub

Private Sub Form_Resize()
  Dim w As Long
  Dim h As Long
  
  w = ScaleWidth
  h = ScaleHeight
  If w >= 8025 And h >= 5790 Then
    tabMain.Width = w - 240
    tabMain.Height = h - tabMain.Top - 120
    fraButtons(0).Top = tabMain.Height - 855
    fraButtons(1).Top = tabMain.Height - 855
    fraButtons(2).Top = tabMain.Height - 855
    Select Case tabMain.Tab
      Case 0
        If tabMain.Width > lstBasinsProject.Left + 240 Then
          lstBasinsProject.Width = tabMain.Width - lstBasinsProject.Left - 120
        End If
        fraInclude.Top = fraButtons(0).Top - fraInclude.Height - 120
        lstBasinsProject.Height = fraInclude.Top - lstBasinsProject.Top - 120
      Case 1
        If tabMain.Width > txtRestoreFile.Left + 240 Then
          txtRestoreFile.Width = tabMain.Width - txtRestoreFile.Left - 120
          cboBasinsProject.Width = txtRestoreFile.Width
        End If
        cboBasinsProject.Height = tabMain.Height - 2037
        fraManifest.Height = tabMain.Height - 2880
        lblManifest.Height = fraManifest.Height - 480
      Case 2
        If w > 1000 And h - tabMain.Top > agdCompare.Top + 240 Then
          agdCompare.Width = tabMain.Width - 240
          agdCompare.Height = tabMain.Height - agdCompare.Top - 120
        End If
      Case 3
        If tabMain.Width > txtRestoreFile.Left + 240 Then
          atxProjectName.Width = tabMain.Width - atxProjectName.Left - 120
          lstFolders.Width = atxProjectName.Width
        End If
        lblNewBASINSprojectName.Top = tabMain.Height - 1332
        atxProjectName.Top = lblNewBASINSprojectName.Top
        lstFolders.Height = atxProjectName.Top - lstFolders.Top - 120
    End Select
  ElseIf Me.WindowState = vbNormal Then
    If Me.Width < 8145 Then Me.Width = 8145
    If Me.Height < 6480 Then Me.Height = 6480
  End If
End Sub

Private Sub Form_Unload(Cancel As Integer)
  Dim vFilename As Variant
  Dim vForm As Variant
  On Error Resume Next
  pOpenArchives.Clear
  For Each vFilename In TempFiles
    Kill vFilename
  Next
  For Each vForm In Forms
    Unload vForm
  Next
End Sub

Private Sub lstBasinsProject_Click()
  Dim i As Long
  Dim ilen As Long
  Dim BasinsProjectName As String
  Dim BasinsFolderName As String
  Dim s As String
  Dim f As String
  
  'find folder name for this project
  BasinsProjectName = lstBasinsProject.List(lstBasinsProject.ListIndex)
  BasinsFolderName = BasinsProjectName
  f = cmbDrives.Text & "basins\apr\" & BasinsProjectName & ".apr"
  If FileExists(f) Then
    s = WholeFileString(f)
    i = InStr(1, s, "/basins/data/") + Len("/basins/data/")
    ilen = InStr(i + 1, s, "/") - i
    If i > 0 Then
      BasinsFolderName = Mid(s, i, ilen)
    End If
  End If
  
  chkDataProject.Enabled = True
  chkSubdirs(0).Enabled = True
  chkDataProject.Caption = chkDataProject.Tag & BasinsFolderName
  If Not FileExists("\basins\data\" & BasinsFolderName, True, False) Then
    chkDataProject.Enabled = False
    chkSubdirs(0).Enabled = False
  End If
  
  chkModelout.Enabled = True
  chkSubdirs(1).Enabled = True
  Set pModelOutDirs = New FastCollection
  AddFilesInDir pModelOutDirs, "\basins\modelout", True, BasinsFolderName, vbDirectory
  If BasinsFolderName <> BasinsProjectName Then
    AddFilesInDir pModelOutDirs, "\basins\modelout", True, BasinsProjectName, vbDirectory
  End If
  
  If pModelOutDirs.Count = 0 Then 'no directories, try just filename
    chkSubdirs(1).Enabled = False
    'AddFilesInDir pModelOutDirs, "\basins\modelout", True, BasinsFolderName
    'If BasinsFolderName <> BasinsProjectName Then
    '  AddFilesInDir pModelOutDirs, "\basins\modelout", True, BasinsProjectName
    'End If
    'If pModelOutDirs.Count = 0 Then
      chkModelout.Enabled = False
    'End If
  End If
End Sub

Private Sub refreshBasinsProjectList(Index As Integer, Optional Init As Boolean = False)
  Dim prj As Object 'gets set to either lstBasinsProject or cboBasinsProject
  Dim P As String   'name of each *.apr in \Basins\apr
  Dim BasinsProjectPath As String

  If Index = 0 Then
    Set prj = lstBasinsProject
    BasinsProjectPath = "Basins\apr"
  ElseIf Index = 3 Then
    Set prj = lstFolders
    BasinsProjectPath = "Basins\data"
  Else
    Set prj = cboBasinsProject
    BasinsProjectPath = "Basins\apr"
  End If
  BasinsProjectPath = cmbDrives.Text & BasinsProjectPath
  If FileExists(BasinsProjectPath, True, False) Then
    ChDriveDir BasinsProjectPath
    BasinsDrive = Left(cmbDrives.Text, 2)

    prj.Clear
    If Index = 3 Then
      P = Dir("*.", vbDirectory)
      While Len(P) > 0
        If P <> "." And P <> ".." And P <> "met_data" And P <> "national" Then
          If (GetAttr(BasinsProjectPath & "\" & P) And vbDirectory) = vbDirectory Then
            'add only the directories
            prj.AddItem P
          End If
        End If
        P = Dir
      Wend
    Else
      P = Dir("*.apr")
      While Len(P) > 0
        prj.AddItem FilenameNoExt(P)
        P = Dir
      Wend
    End If
    If prj.ListCount > 0 Then
      prj.ListIndex = 0
      pLogger.Log "  refreshBasinsProjectList:Number of projects found is " & prj.ListCount
    Else
      pLogger.Log "  refreshBasinsProjectList:No projects found."
    End If
  ElseIf Not Init Then
    'pLogger.LogMsg "Not found: " & BasinsProjectPath, "Refresh Basins Project List Problem"
    pLogger.Log "Not found: " & BasinsProjectPath
  End If
End Sub

Private Sub lstFolders_Click()
  atxProjectName.Value = lstFolders.List(lstFolders.ListIndex)
End Sub

Private Sub mnuContents_Click()
  SendKeys "{F1}"
'  Dim filename As String
'  filename = OpenFile(App.HelpFile, Me.cdlg)
'  If FileExists(filename) Then
'    If filename <> App.HelpFile Then
'      App.HelpFile = filename
'      SaveSetting "BasinsArchive", "files", "BasinsArchive.chm", filename
'    End If
'  End If
End Sub

Private Sub mnuAbout_Click()
  frmAbout.Caption = "About " & pBaseCaption
  frmAbout.ShowVersions AboutString(True)
End Sub

Private Sub mnuExit_Click()
  Unload Me
End Sub

Private Sub mnuFeedback_Click()
  Dim stepname As String
  On Error GoTo errmsg
  stepname = "1: Dim feedback As clsATCoFeedback"
  Dim feedback As clsATCoFeedback
  stepname = "2: Set feedback = New clsATCoFeedback"
  Set feedback = New clsATCoFeedback
  stepname = "3: feedback.AddFile"
  feedback.AddFile Left(App.path, InStr(4, App.path, "\")) & "unins000.dat"
  stepname = "4: feedback.AddText"
  feedback.AddText AboutString(False)
  stepname = "5: feedback.AddLogFile"
  Set feedback.Logger = pLogger
  stepname = "6: feedback.Show"
  feedback.Show App, Me.Icon
  
  Exit Sub
  
errmsg:
  pLogger.LogMsg "Error opening feedback in step " & stepname & vbCr _
                 & Err.Description, _
                 "Archive Feedback"
End Sub

Private Sub tabMain_Click(PreviousTab As Integer)
  Form_Resize
End Sub

Private Sub txtRestoreFile_KeyPress(KeyAscii As Integer)
  txtRestoreFile_MouseDown 0, 0, 0, 0
End Sub

Private Sub txtRestoreFile_MouseDown(Button As Integer, Shift As Integer, x As Single, y As Single)
  Dim mytar As clsTar
  Me.MousePointer = vbHourglass
  With cdlFile
    .InitDir = BasinsDrive & "basins\apr"
    .DialogTitle = "Select BASINS Archive File"
    .DefaultExt = ".tar"
    .Filter = "BASINS Archives (*.tar,*.tar.gz)|*.tar;*.tar.gz|Tar (*.tar)|*.tar|Tar GNU Zip (*.tar.gz)|*.tar.gz"
    .FilterIndex = 1
    .CancelError = True
    On Error GoTo SkipIt
    .ShowOpen
    On Error GoTo ErrHandler
    txtRestoreFile = .filename
    cboBasinsProject = FilenameOnly(FilenameOnly(txtRestoreFile))
  End With
  Set mytar = OpenArchive(txtRestoreFile)
  If mytar.ArchiveFilenames.Count > 0 Then
    If mytar.ArchiveFilenames(1) = "manifest.txt" Then
      lblManifest.Caption = mytar.FileAsString(1)
    Else
      lblManifest.Caption = "no manifest"
    End If
  Else
    lblManifest.Caption = "file not found"
  End If
SkipIt:
  Status "Ready to restore " & FilenameNoPath(txtRestoreFile)
  Me.MousePointer = vbDefault
  Exit Sub

ErrHandler:
  pLogger.LogMsg Err.Description, "Archive Restore"
End Sub

