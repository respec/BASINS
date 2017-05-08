VERSION 5.00
Begin VB.Form frmBasins 
   Caption         =   "GenScn Initialization from BASINS"
   ClientHeight    =   3360
   ClientLeft      =   60
   ClientTop       =   345
   ClientWidth     =   8085
   HelpContextID   =   142
   Icon            =   "frmBasins.frx":0000
   LinkTopic       =   "Form1"
   ScaleHeight     =   3360
   ScaleWidth      =   8085
   StartUpPosition =   3  'Windows Default
   Begin VB.CommandButton cmdLoad 
      Caption         =   "&Load Existing"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Left            =   120
      TabIndex        =   1
      Top             =   2880
      Width           =   1815
   End
   Begin VB.CommandButton cmdCancel 
      Cancel          =   -1  'True
      Caption         =   "&Cancel"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Left            =   4080
      TabIndex        =   3
      Top             =   2880
      Width           =   1095
   End
   Begin VB.CommandButton cmdOK 
      Caption         =   "&OK"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Left            =   2880
      TabIndex        =   2
      Top             =   2880
      Width           =   1095
   End
   Begin TabDlg.SSTab tabData 
      Height          =   2655
      Left            =   120
      TabIndex        =   0
      Top             =   120
      Width           =   7815
      _ExtentX        =   13785
      _ExtentY        =   4683
      _Version        =   393216
      Tabs            =   6
      TabsPerRow      =   6
      TabHeight       =   520
      TabCaption(0)   =   "Overview"
      TabPicture(0)   =   "frmBasins.frx":0442
      Tab(0).ControlEnabled=   -1  'True
      Tab(0).Control(0)=   "fraOverview"
      Tab(0).Control(0).Enabled=   0   'False
      Tab(0).ControlCount=   1
      TabCaption(1)   =   "Met Data"
      TabPicture(1)   =   "frmBasins.frx":045E
      Tab(1).ControlEnabled=   0   'False
      Tab(1).Control(0)=   "cklObserved"
      Tab(1).Control(0).Enabled=   0   'False
      Tab(1).ControlCount=   1
      TabCaption(2)   =   "PLTGEN"
      TabPicture(2)   =   "frmBasins.frx":047A
      Tab(2).ControlEnabled=   0   'False
      Tab(2).Control(0)=   "cklPoint"
      Tab(2).ControlCount=   1
      TabCaption(3)   =   "SWAT Output"
      TabPicture(3)   =   "frmBasins.frx":0496
      Tab(3).ControlEnabled=   0   'False
      Tab(3).Control(0)=   "cklSwat"
      Tab(3).ControlCount=   1
      TabCaption(4)   =   "HSPF Output"
      TabPicture(4)   =   "frmBasins.frx":04B2
      Tab(4).ControlEnabled=   0   'False
      Tab(4).Control(0)=   "cklHspf"
      Tab(4).ControlCount=   1
      TabCaption(5)   =   "Obs WQ Data"
      TabPicture(5)   =   "frmBasins.frx":04CE
      Tab(5).ControlEnabled=   0   'False
      Tab(5).Control(0)=   "cklObswq"
      Tab(5).Control(0).Enabled=   0   'False
      Tab(5).ControlCount=   1
      Begin GenScn.ctlChecklist cklPoint 
         Height          =   1572
         Left            =   -74640
         TabIndex        =   14
         Top             =   600
         Width           =   7092
         _ExtentX        =   12515
         _ExtentY        =   2778
      End
      Begin GenScn.ctlChecklist cklObserved 
         Height          =   1572
         Left            =   -74640
         TabIndex        =   13
         Top             =   600
         Width           =   7092
         _ExtentX        =   12515
         _ExtentY        =   2778
      End
      Begin VB.Frame fraOverview 
         BorderStyle     =   0  'None
         Height          =   1695
         Left            =   360
         TabIndex        =   4
         Top             =   600
         Width           =   7215
         Begin VB.CommandButton cmdMapBrowse 
            Caption         =   "&Browse"
            BeginProperty Font 
               Name            =   "MS Sans Serif"
               Size            =   8.25
               Charset         =   0
               Weight          =   700
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Left            =   5880
            TabIndex        =   18
            Top             =   1320
            Width           =   855
         End
         Begin VB.ComboBox cboWatershed 
            Height          =   315
            Left            =   2280
            TabIndex        =   9
            Top             =   120
            Width           =   1455
         End
         Begin VB.ComboBox cboType 
            Height          =   315
            Left            =   2280
            TabIndex        =   10
            Top             =   600
            Width           =   1455
         End
         Begin MSComDlg.CommonDialog cdMapName 
            Left            =   6720
            Top             =   1200
            _ExtentX        =   688
            _ExtentY        =   688
            _Version        =   393216
            FontSize        =   4.873e-37
         End
         Begin VB.Label lblLocName 
            Caption         =   "<none>"
            BeginProperty Font 
               Name            =   "MS Sans Serif"
               Size            =   8.25
               Charset         =   0
               Weight          =   700
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Left            =   2280
            TabIndex        =   12
            Top             =   960
            Visible         =   0   'False
            Width           =   4095
         End
         Begin VB.Label lblMapName 
            Caption         =   "<none>"
            BeginProperty Font 
               Name            =   "MS Sans Serif"
               Size            =   8.25
               Charset         =   0
               Weight          =   700
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Left            =   2280
            TabIndex        =   11
            Top             =   1320
            Width           =   3615
         End
         Begin VB.Label lblWatershed 
            Caption         =   "BASINS &Project"
            BeginProperty Font 
               Name            =   "MS Sans Serif"
               Size            =   8.25
               Charset         =   0
               Weight          =   700
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Left            =   360
            TabIndex        =   8
            Top             =   120
            Width           =   1455
         End
         Begin VB.Label lblLoc 
            Caption         =   "BASINS Location"
            BeginProperty Font 
               Name            =   "MS Sans Serif"
               Size            =   8.25
               Charset         =   0
               Weight          =   700
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Left            =   360
            TabIndex        =   7
            Top             =   960
            Visible         =   0   'False
            Width           =   1695
         End
         Begin VB.Label lblMap 
            Caption         =   "Map File Name"
            BeginProperty Font 
               Name            =   "MS Sans Serif"
               Size            =   8.25
               Charset         =   0
               Weight          =   700
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Left            =   360
            TabIndex        =   6
            Top             =   1320
            Width           =   1335
         End
         Begin VB.Label lblType 
            Caption         =   "Project &Type"
            BeginProperty Font 
               Name            =   "MS Sans Serif"
               Size            =   8.25
               Charset         =   0
               Weight          =   700
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Left            =   360
            TabIndex        =   5
            Top             =   600
            Width           =   1215
         End
      End
      Begin GenScn.ctlChecklist cklSwat 
         Height          =   1572
         Left            =   -74640
         TabIndex        =   15
         Top             =   600
         Width           =   7092
         _ExtentX        =   12515
         _ExtentY        =   2778
      End
      Begin GenScn.ctlChecklist cklHspf 
         Height          =   1572
         Left            =   -74640
         TabIndex        =   16
         Top             =   600
         Width           =   7092
         _ExtentX        =   12515
         _ExtentY        =   2778
      End
      Begin GenScn.ctlChecklist cklObswq 
         Height          =   1572
         Left            =   -74640
         TabIndex        =   17
         Top             =   600
         Width           =   7092
         _ExtentX        =   12515
         _ExtentY        =   2778
      End
   End
   Begin MSComDlg.CommonDialog cdl 
      Left            =   7440
      Top             =   2880
      _ExtentX        =   688
      _ExtentY        =   688
      _Version        =   393216
      FontSize        =   4.873e-37
   End
End
Attribute VB_Name = "frmBasins"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants
Dim staname As String
Dim stapath As String
Dim DataType$, BasinsLoc$, WatershedName$, MapName$, InForm As Boolean
Dim UciName As String

Public Sub Init(ctype$, cbloc$, cwshed$, cmap$)
  Dim datapath$, cname$, ctemp$
  
  InForm = False
  DataType = ctype
  BasinsLoc = cbloc
  If cwshed = FilenameOnly(cwshed) Then
    WatershedName = cwshed
    UciName = ""
  Else
    UciName = cwshed
    cwshed = FilenameOnly(cwshed)
    WatershedName = cwshed
  End If
  MapName = cmap
  stapath = PathNameOnly(UciName)
  If Len(stapath) = 0 Then
    stapath = PathNameOnly(MapName)
    If Len(stapath) = 0 Then
      stapath = BasinsLoc & "\modelout\" & WatershedName
    End If
  End If
  
  cboType.clear
  cboType.AddItem "HSPF"
  cboType.AddItem "SWAT"
  cboType.AddItem "General"
  If ctype = "HSPF" Then
    cboType.ListIndex = 0
  ElseIf ctype = "SWAT" Then
    cboType.ListIndex = 1
  Else
    cboType.ListIndex = 2
  End If
  
  lblMapName = cmap
  lblLocName = cbloc
  
  cboWatershed.clear
  cboWatershed.AddItem "ALL"
  cboWatershed.ListIndex = 0
  datapath = BasinsLoc & "\apr\"
  cname = Dir(datapath & "*.apr")
  If Len(cname) > 0 Then
    'add this project name
    While Len(cname) > 0
      ctemp = Mid(cname, 1, Len(cname) - 4)
      cboWatershed.AddItem ctemp
      If ctemp = cwshed Then
        cboWatershed.ListIndex = cboWatershed.ListCount - 1
      End If
      cname = Dir()
    Wend
  End If
  InForm = True
  
  Call RefreshLists
  
  If ctype = "HSPF" And Len(UciName) > 0 Then
    'scan uci file to see which files to check 'on'
    ScanFilesBlock
  End If
End Sub

Public Sub ScanFilesBlock()
  Dim i&, j&, s$, tname$, InList As Boolean
  
  On Error GoTo x:
  
  i = FreeFile(0)
  Open UciName For Input As #i
  Do
    Line Input #i, s
    If Left(s, Len("FILES")) = "FILES" Then 'at files block
      While Left(s, Len("END FILES")) <> "END FILES"
        Line Input #i, s
        If InStr(1, s, "***") = 0 Then
          If Left(s, 3) = "WDM" Then
            'found wdm file
            tname = Mid(s, 17, Len(s) - 16)
            'make absolute path
            tname = AbsolutePath(tname, PathNameOnly(UciName))
            InList = False
            For j = 1 To cklHspf.Count
              If UCase(tname) = UCase(cklHspf.FileName(j - 1)) Then
                'set to 'on'
                'cklHspf.SetFileOnOff j - 1, 1
                cklHspf.FileValue(j - 1) = 1
                InList = True
                Exit For
              End If
            Next j
            For j = 1 To cklObserved.Count
              If UCase(tname) = UCase(cklObserved.FileName(j - 1)) Then
                'set to 'on'
                'cklObserved.SetFileOnOff j - 1, 1
                cklObserved.FileValue(j - 1) = 1
                InList = True
                Exit For
              End If
            Next j
            If Not InList Then
              'add to hspf output list
              cklHspf.AddItem tname, 1
            End If
          End If
        End If
      Wend
      Exit Do
    End If
  Loop
x:
  Close #i
End Sub

Private Sub cboType_Click()
  If InForm Then
    Call RefreshLists
  End If
End Sub

Private Sub cboWatershed_Click()
  If InForm Then
    Call RefreshLists
  End If
End Sub

Private Sub cmdCancel_Click()
  Unload Me
  staname = ""
End Sub

Private Sub cmdLoad_Click()
  On Error Resume Next
  staname = ""
  ChDriveDir stapath
  cdl.DialogTitle = "Load Existing GenScn Project"
  cdl.CancelError = True
  cdl.FLAGS = &H1804& 'not read only
  cdl.filter = "Project files (*.sta)|*.sta"
  cdl.ShowOpen
  staname = cdl.FileName
  If staname <> "" Then
    Unload Me
  End If
End Sub

Private Sub cmdMapBrowse_Click()
  On Error GoTo errhandler
  cdMapName.DialogTitle = "Browse for GenScn Map File"
  cdMapName.CancelError = True
  cdMapName.FLAGS = &H1804& 'not read only
  cdMapName.filter = "Map files (*.map)|*.map"
  cdMapName.ShowOpen
  MapName = cdMapName.FileName
  lblMapName = MapName
errhandler:
  On Error Resume Next
End Sub

Private Sub cmdOk_Click()
  'write sta file
  staname = ""
  If Len(stapath) = 0 Then
    staname = WatershedName & ".sta"
  Else
    MkDirPath stapath
    If Len(Dir(stapath, vbDirectory)) > 0 Then
      staname = stapath & "\" & WatershedName & ".sta"
    End If
  End If
  If Len(staname) > 0 Then Call WriteSTA
  Unload Me
End Sub

Public Sub GetStaName(tname As String)
  tname = staname
End Sub

Private Sub RefreshLists()
  Dim cname$, datapath$, i&, watname$
  Dim swatScenCnt&, swatScenName$()
  
  tabData.Tab = 0
  
  'find met data candidates
  cklObserved.clear
  datapath = BasinsLoc & "\data\met_data\"
  cklObserved.DefaultDirectory = datapath
  cname = Dir(datapath & "*.wdm")
  If Len(cname) > 0 Then
    cklObserved.AddItem datapath & cname, 0
    cname = Dir()
    While Len(cname) > 0
      cklObserved.AddItem datapath & cname, 0
      cname = Dir()
    Wend
  End If
  cklObserved.FilterString = "Met WDM files (*.wdm)|*.wdm"
  
  cklPoint.clear
  If cboType.ListIndex = 0 Or cboType.ListIndex = 2 Then 'hspf type
    'find point load candidates
    datapath = BasinsLoc & "\models\npsm\" 'used to be stored here
    cname = Dir(datapath & "*.mut")
    If Len(cname) > 0 Then
      cklPoint.AddItem datapath & cname, 0
      cname = Dir()
      While Len(cname) > 0
        cklPoint.AddItem datapath & cname, 0
        cname = Dir()
      Wend
    End If
    If cboWatershed.ListIndex = 0 Then 'also need to check in modelout project folder
      'fill in for all watersheds
      For i = 1 To cboWatershed.ListCount - 1
        watname = cboWatershed.List(i)
        Call AddPointFiles(watname)
      Next i
    ElseIf cboWatershed.ListIndex > 0 Then
      'fill in for this watershed
      watname = cboWatershed.List(cboWatershed.ListIndex)
      Call AddPointFiles(watname)
    End If
  End If
  cklPoint.FilterString = "MUTSIN files (*.mut)|*.mut|PLTGEN files (*.plt)|*.plt|all files (*.*)|*.*"
  
  'find swat candidates
  cklSwat.clear
  If cboType.ListIndex = 1 Or cboType.ListIndex = 2 Then  'swat type
    If cboWatershed.ListIndex = 0 Then
      'need to check under all watersheds
      For i = 0 To cboWatershed.ListCount - 1
        watname = cboWatershed.List(i)
        Call AddSWATFiles(watname)
      Next i
    Else
      'need only to check current watershed
      watname = cboWatershed.List(cboWatershed.ListIndex)
      Call AddSWATFiles(watname)
    End If
  End If
  cklSwat.FilterString = "SWAT Dbf files (*.dbf)|*.dbf"
  
  'find project wdm candidates for hspf
  cklHspf.clear
  If cboType.ListIndex = 0 Or cboType.ListIndex = 2 Then 'hspf type
    If cboWatershed.ListIndex = 0 Then
      'need to check under all watersheds
      For i = 0 To cboWatershed.ListCount - 1
        watname = cboWatershed.List(i)
        Call AddHSPFFiles(watname)
      Next i
    Else
      'need only to check current watershed
      watname = cboWatershed.List(cboWatershed.ListIndex)
      Call AddHSPFFiles(watname)
    End If
    Call AddHSPFFiles("")
  End If
  cklHspf.FilterString = "Project WDM files (*.wdm)|*.wdm"
  
  'find observed wq candidates
  cklObswq.clear
  Call AddWQFiles
  cklObswq.FilterString = "Observed WQ files (*.dbf)|*.dbf"
  
  cklObserved.Resize
  cklHspf.Resize
  cklSwat.Resize
  cklPoint.Resize
  cklObswq.Resize
End Sub

Private Sub Form_Resize()
  If frmBasins.WindowState <> vbMinimized Then
    With frmBasins
      If .Width > 1500 Then
        tabData.Width = .Width - 400
        cmdOk.Left = (.Width / 2) - cmdOk.Width - 100
        cmdCancel.Left = (.Width / 2) + 100
        cmdMapBrowse.Left = fraOverview.Width - cmdMapBrowse.Width - 500
        lblMapName.Width = cmdMapBrowse.Left - lblMapName.Left
        fraOverview.Width = tabData.Width - 500
        cklObserved.Width = tabData.Width - 700
        cklHspf.Width = tabData.Width - 700
        cklSwat.Width = tabData.Width - 700
        cklObswq.Width = tabData.Width - 700
        cklPoint.Width = tabData.Width - 700
      End If
      If .Height > 2500 Then
        tabData.Height = .Height - cmdOk.Height - 750
        cmdOk.Top = .Height - 850
        cmdCancel.Top = .Height - 850
        cmdLoad.Top = .Height - 850
        fraOverview.Height = tabData.Height - 700
        cklObserved.Height = tabData.Height - 900
        cklHspf.Height = tabData.Height - 900
        cklSwat.Height = tabData.Height - 900
        cklObswq.Height = tabData.Height - 900
        cklPoint.Height = tabData.Height - 900
      End If
    End With
  End If
End Sub

Private Sub AddSWATFiles(watname$)
  Dim cname$, datapath$, i&, j&
  Dim swatScenCnt&, swatScenName$()
  
  swatScenCnt = 0
  'look for swat scenarios within this project
  datapath = BasinsLoc & "\modelout\swat\" & watname & "\scen\"
  cklSwat.DefaultDirectory = BasinsLoc & "\modelout\swat\"
  cname = Dir(datapath & "*.", vbDirectory)
  If Len(cname) > 0 Then
    If cname <> "." And cname <> ".." Then
      swatScenCnt = swatScenCnt + 1
      ReDim Preserve swatScenName(swatScenCnt)
      swatScenName(swatScenCnt - 1) = cname
    End If
    cname = Dir()
    While Len(cname) > 0
      If cname <> "." And cname <> ".." Then
        swatScenCnt = swatScenCnt + 1
        ReDim Preserve swatScenName(swatScenCnt)
        swatScenName(swatScenCnt - 1) = cname
      End If
      cname = Dir()
    Wend
  End If
  'now look for files within these scenarios
  For j = 1 To swatScenCnt
    datapath = BasinsLoc & "\modelout\swat\" & _
       watname & "\scen\" & swatScenName(j - 1) & "\"
    cname = Dir(datapath & "*.dbf")
    If Len(cname) > 0 Then
      cklSwat.AddItem datapath & cname, 0
      cname = Dir()
      While Len(cname) > 0
        cklSwat.AddItem datapath & cname, 0
        cname = Dir()
      Wend
    End If
  Next j
  'also look for files above the scenario level
  datapath = BasinsLoc & "\modelout\swat\" & _
       watname & "\tablesout\"
  cname = Dir(datapath & "*.dbf")
  If Len(cname) > 0 Then
    cklSwat.AddItem datapath & cname, 0
    cname = Dir()
    While Len(cname) > 0
      cklSwat.AddItem datapath & cname, 0
      cname = Dir()
    Wend
  End If
End Sub

Private Sub AddPointFiles(watname$)
  Dim cname$, datapath$
  
  datapath = BasinsLoc & "\modelout\" & watname & "\"
  cklPoint.DefaultDirectory = BasinsLoc & "\modelout\"
  cname = Dir(datapath & "*.mut")
  If Len(cname) > 0 Then
    cklPoint.AddItem datapath & cname, 0
    cname = Dir()
    While Len(cname) > 0
      cklPoint.AddItem datapath & cname, 0
      cname = Dir()
    Wend
  End If
  cname = Dir(datapath & "*.plt")
  If Len(cname) > 0 Then
    cklPoint.AddItem datapath & cname, 0
    cname = Dir()
    While Len(cname) > 0
      cklPoint.AddItem datapath & cname, 0
      cname = Dir()
    Wend
  End If
End Sub

Private Sub AddHSPFFiles(watname$)
  Dim cname$, datapath$
  
  If Len(watname) > 0 Then
    datapath = BasinsLoc & "\modelout\" & watname & "\"
  Else
    datapath = BasinsLoc & "\modelout\"
  End If
  cklHspf.DefaultDirectory = BasinsLoc & "\modelout\"
  cname = Dir(datapath & "*.wdm")
  If Len(cname) > 0 Then
    cklHspf.AddItem datapath & cname, 0
    cname = Dir()
    While Len(cname) > 0
      cklHspf.AddItem datapath & cname, 0
      cname = Dir()
    Wend
  End If
End Sub

Private Sub AddWQFiles()
  Dim cname$, datapath$, i&, j&
  Dim watdataCnt&, watdataName$()
  
  watdataCnt = 0
  'look for watershed data folders
  datapath = BasinsLoc & "\data\"
  cklObswq.DefaultDirectory = datapath
  cname = Dir(datapath & "*.", vbDirectory)
  If Len(cname) > 0 Then
    If cname <> "." And cname <> ".." And cname <> "met_data" Then
      watdataCnt = watdataCnt + 1
      ReDim Preserve watdataName(watdataCnt)
      watdataName(watdataCnt - 1) = cname
    End If
    cname = Dir()
    While Len(cname) > 0
      If cname <> "." And cname <> ".." And cname <> "met_data" Then
        watdataCnt = watdataCnt + 1
        ReDim Preserve watdataName(watdataCnt)
        watdataName(watdataCnt - 1) = cname
      End If
      cname = Dir()
    Wend
  End If
  'now look for files within these watershed folders
  For j = 1 To watdataCnt
    datapath = BasinsLoc & "\data\" & _
       watdataName(j - 1) & "\wqobs\"
    cname = Dir(datapath & "*.dbf")
    If Len(cname) > 0 Then
      cklObswq.AddItem datapath & cname, 0
      cname = Dir()
      While Len(cname) > 0
        cklObswq.AddItem datapath & cname, 0
        cname = Dir()
      Wend
    End If
  Next j
End Sub
  
Private Sub WriteSTA()
  Dim fl&, sp$, i&, j&
  
  On Error GoTo errHand
  
  fl = FreeFile(0)
  Open staname For Output As #fl
  sp = PathNameOnly(staname)
  
  'message file name
  Print #fl, "MES " & RelativeFilename(ExePath & "hspfmsg.wdm", sp)
  Print #fl, "HID SCENARIOMODIFY"
  
  'map file name
  If Len(MapName) > 0 Then
    Print #fl, "MAP " & RelativeFilename(MapName, sp)
  End If
  
  'met wdm names
  j = 0
  For i = 1 To cklObserved.Count
    If cklObserved.FileValue(i - 1) = 1 Then
      j = j + 1
      If j < 4 Then
        Print #fl, "WDM WDM" & j & " " & _
                 RelativeFilename(cklObserved.FileName(i - 1), sp)
      End If
    End If
  Next i
  
  'project wdm names
  For i = 1 To cklHspf.Count
    If cklHspf.FileValue(i - 1) = 1 Then
      j = j + 1
      If j < 5 Then
        Print #fl, "WDM WDM" & j & " " & _
                 RelativeFilename(cklHspf.FileName(i - 1), sp)
      End If
    End If
  Next i
  
  'swat dbf names
  For i = 1 To cklSwat.Count
    If cklSwat.FileValue(i - 1) = 1 Then
      Print #fl, "SWATDBF " & _
                 RelativeFilename(cklSwat.FileName(i - 1), sp)
    End If
  Next i
  
  'point load names
  For i = 1 To cklPoint.Count
    If cklPoint.FileValue(i - 1) = 1 Then
      Print #fl, "PLT " & _
                 RelativeFilename(cklPoint.FileName(i - 1), sp)
    End If
  Next i
  
  'obs wq names
  For i = 1 To cklObswq.Count
    If cklObswq.FileValue(i - 1) = 1 Then
      Print #fl, "BasObsWQ " & _
                 RelativeFilename(cklObswq.FileName(i - 1), sp)
    End If
  Next i
  
  Close #fl
  
  Exit Sub
errHand:
  MsgBox "Could not write '" & staname & "'" & vbCr & err.Description, vbOKOnly, "Error in WriteSTA"
End Sub


