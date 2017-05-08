VERSION 5.00
Begin VB.Form frmGenScnManageProject 
   Caption         =   "GenScn Edit Project"
   ClientHeight    =   5370
   ClientLeft      =   345
   ClientTop       =   1470
   ClientWidth     =   8415
   ControlBox      =   0   'False
   BeginProperty Font 
      Name            =   "MS Sans Serif"
      Size            =   8.25
      Charset         =   0
      Weight          =   700
      Underline       =   0   'False
      Italic          =   0   'False
      Strikethrough   =   0   'False
   EndProperty
   HelpContextID   =   35
   Icon            =   "GenProj.frx":0000
   LinkTopic       =   "Form1"
   PaletteMode     =   1  'UseZOrder
   ScaleHeight     =   5370
   ScaleWidth      =   8415
   WhatsThisButton =   -1  'True
   WhatsThisHelp   =   -1  'True
   Begin VB.Frame fraButtons 
      BorderStyle     =   0  'None
      Height          =   375
      Left            =   3240
      TabIndex        =   15
      Top             =   4920
      Width           =   1935
      Begin VB.CommandButton cmdOk 
         Caption         =   "&OK"
         Default         =   -1  'True
         Height          =   375
         Left            =   0
         TabIndex        =   17
         Top             =   0
         Width           =   852
      End
      Begin VB.CommandButton cmdCancel 
         Cancel          =   -1  'True
         Caption         =   "Cancel"
         Height          =   375
         Left            =   1080
         TabIndex        =   16
         Top             =   0
         Width           =   852
      End
   End
   Begin VB.CommandButton cmdAdd 
      Caption         =   "Add from Web"
      Height          =   252
      Index           =   1
      Left            =   240
      TabIndex        =   14
      Top             =   2040
      Width           =   1932
   End
   Begin VB.Frame fraProperties 
      Caption         =   "File Properties"
      Height          =   1332
      Left            =   120
      TabIndex        =   13
      Top             =   3480
      Width           =   8172
      Begin VB.TextBox txtInfo 
         BorderStyle     =   0  'None
         ForeColor       =   &H00000000&
         Height          =   972
         Left            =   120
         MultiLine       =   -1  'True
         ScrollBars      =   2  'Vertical
         TabIndex        =   8
         Top             =   240
         Width           =   7932
      End
   End
   Begin VB.CommandButton cmdFile 
      Caption         =   "New"
      Height          =   252
      Index           =   0
      Left            =   120
      TabIndex        =   0
      Top             =   120
      Width           =   735
   End
   Begin VB.ComboBox cmbFileType 
      Height          =   315
      Left            =   240
      TabIndex        =   4
      Text            =   "Combo1"
      Top             =   1200
      Width           =   1935
   End
   Begin ATCoCtl.ATCoGrid grdTSer 
      Height          =   2532
      Left            =   2280
      TabIndex        =   7
      Top             =   960
      Width           =   6012
      _ExtentX        =   10610
      _ExtentY        =   4471
      SelectionToggle =   0   'False
      AllowBigSelection=   -1  'True
      AllowEditHeader =   0   'False
      AllowLoad       =   0   'False
      AllowSorting    =   0   'False
      Rows            =   2
      Cols            =   2
      ColWidthMinimum =   300
      gridFontBold    =   0   'False
      gridFontItalic  =   0   'False
      gridFontName    =   "MS Sans Serif"
      gridFontSize    =   8
      gridFontUnderline=   0   'False
      gridFontWeight  =   400
      gridFontWidth   =   0
      Header          =   "Open Files"
      FixedRows       =   1
      FixedCols       =   0
      ScrollBars      =   3
      SelectionMode   =   4
      BackColor       =   -2147483643
      ForeColor       =   -2147483640
      BackColorBkg    =   -2147483633
      BackColorSel    =   -2147483635
      ForeColorSel    =   -2147483634
      BackColorFixed  =   -2147483633
      ForeColorFixed  =   -2147483630
      InsideLimitsBackground=   -2147483643
      OutsideHardLimitBackground=   8421631
      OutsideSoftLimitBackground=   8454143
      ComboCheckValidValues=   0   'False
   End
   Begin VB.CommandButton cmdRem 
      Caption         =   "Remove"
      Height          =   252
      Left            =   240
      TabIndex        =   6
      Top             =   2400
      Width           =   1932
   End
   Begin VB.CommandButton cmdAdd 
      Caption         =   "Add from File"
      Height          =   252
      Index           =   0
      Left            =   240
      TabIndex        =   5
      Top             =   1680
      Width           =   1932
   End
   Begin VB.CommandButton cmdFile 
      Caption         =   "Select"
      Height          =   252
      Index           =   2
      Left            =   960
      TabIndex        =   1
      ToolTipText     =   "A Map File must be selected"
      Top             =   120
      Width           =   852
   End
   Begin VB.CommandButton cmdFile 
      Caption         =   "Select"
      Height          =   252
      Index           =   1
      Left            =   960
      TabIndex        =   2
      ToolTipText     =   "The HSPF Message File is distributed with GenScn"
      Top             =   480
      Width           =   852
   End
   Begin MSComDlg.CommonDialog CDFile 
      Left            =   7920
      Top             =   4800
      _ExtentX        =   688
      _ExtentY        =   688
      _Version        =   393216
      FontSize        =   4.09255e-38
   End
   Begin VB.Label lblFileType 
      Caption         =   "File&Type:"
      Height          =   255
      Left            =   120
      TabIndex        =   3
      Top             =   960
      Width           =   855
   End
   Begin VB.Label lblName 
      Caption         =   "Map File"
      Height          =   252
      Index           =   0
      Left            =   1920
      TabIndex        =   12
      Top             =   120
      Width           =   1932
   End
   Begin VB.Label lblFile 
      Caption         =   "<none>"
      Height          =   252
      Index           =   2
      Left            =   4080
      TabIndex        =   11
      Top             =   120
      Width           =   4212
   End
   Begin VB.Label lblName 
      Caption         =   "HSPF Message File"
      Height          =   252
      Index           =   1
      Left            =   1920
      TabIndex        =   10
      Top             =   480
      Width           =   1932
   End
   Begin VB.Label lblFile 
      Caption         =   "<none>"
      Height          =   252
      Index           =   1
      Left            =   4080
      TabIndex        =   9
      Top             =   480
      Width           =   4212
   End
End
Attribute VB_Name = "frmGenScnManageProject"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants

Private Const MemoryLabel = "Script Import"

Private Sub editMade()
  If Not p.EditFlg Then
    If Right(Me.Caption, 1) <> ">" Then
      Me.Caption = Me.Caption & " Edited"
    End If
    p.EditFlg = True
  End If
End Sub

Private Sub cmdAdd_Click(index As Integer)
  Dim FileName As String, ext As String
  Dim i%, j%, s$, e$, extpos&, endextpos&
  Dim TserIndex&
  Dim curAvail As ATCclsTserFile, curactive As ATCclsTserFile
  Dim addedFile As Boolean

  s = cmbFileType.List(cmbFileType.ListIndex)
  If s = MemoryLabel Then s = "Memory"
  TserIndex = TserFiles.AvailIndexByName("clsTSer" & s)
  If TserIndex = 0 Then
    MsgBox s & " Error: " & TserFiles.ErrorDescription & vbCrLf, vbExclamation
    Exit Sub
  End If
  Set curAvail = TserFiles.Avail(TserIndex).obj
  If index = 0 Then 'Get from file
    ext = curAvail.FileExtension
    CDFile.filter = ext
    extpos = InStr(1, ext, "|*.")
    If extpos > 0 Then
      endextpos = InStr(extpos + 3, ext, "|") - 1
      If endextpos < extpos Then endextpos = Len(ext)
      CDFile.FileName = Mid(ext, extpos + 1, endextpos - extpos) '"*." & curAvail.ThreeCharExtension
    End If
    CDFile.flags = cdlOFNPathMustExist + cdlOFNHideReadOnly + cdlOFNOverwritePrompt
    CDFile.DialogTitle = "GenScn Add " & s
    On Error GoTo 10
    CDFile.CancelError = True
    CDFile.ShowOpen
    FileName = LCase(CDFile.FileName)
    'If Len(p.StatusFilePath) > 0 Then
    '  If Left(FileName, Len(p.StatusFilePath)) = LCase(p.StatusFilePath) Then
    '    FileName = Mid(FileName, Len(p.StatusFilePath) + 1)
    '    If Left(FileName, 1) = "\" Then FileName = Mid(FileName, 2)
    '    ChDriveDir p.StatusFilePath
    '  End If
    'End If
  Else 'Get from web
    FileName = frmGetDataFromWeb.GetFileName
    If FileName = "Error" Then Exit Sub
  End If
  If grdTSer.RowContaining(LCase(FileName), 1) > -1 Then
    MsgBox s & " file " & FileName & " is already part of the current project", _
           vbExclamation, Me.Caption & " Problem"
    GoTo 10
  Else
    Call TserFiles.Create(TserIndex)
    addedFile = True
    Set curactive = TserFiles.CurrentActive.obj
    'Set curactive.Monitor = launch
    Set curactive.Monitor = IPC
    curactive.HelpFileName = App.HelpFile
    
    If s = "WDM" Then TserFiles.CurrentActive.obj.msgUnit = p.HSPFMsg.Unit
    
    curactive.FileName = FileName
    
    If s = "WDM" Then p.WDMFiles.Add curactive, "FU" & curactive.FileUnit
    
    e = curactive.ErrorDescription
    If Len(e) > 0 Then
      If Right(e, 9) = "not found" Then
        If MsgBox(e & vbCr & vbCr & "Create it?", vbYesNo, Me.Caption & " Problem") = vbYes Then
          curactive.SaveAs FileName
        Else
          addedFile = False
        End If
      Else
        MsgBox e, vbExclamation, Me.Caption & " Problem"
      End If
    ElseIf curactive.DataCount < 1 Then 'Did not actually find any datasets
      MsgBox "No datasets added from '" & FileName & "'", vbOKOnly, Me.Caption
    End If
    If addedFile Then
      grdTSer.Rows = grdTSer.Rows + 1
      grdTSer.TextMatrix(grdTSer.Rows, 0) = s
      grdTSer.TextMatrix(grdTSer.Rows, 1) = curactive.FileUnit
      grdTSer.TextMatrix(grdTSer.Rows, 2) = FileName
      editMade
    Else
      TserFiles.Delete TserFiles.CurrentActiveIndex
      Debug.Print "after aborted add, count of files is " & TserFiles.Active.Count
    End If
  End If
10:
End Sub

Private Sub cmdCancel_Click()
  'Dim lp As GenScnProject
  If Len(p.StatusFileName) = 0 Then 'new, quick cancel makes sense
    'p = lp
    Unload Me
  ElseIf p.EditFlg Then 'edits have taken place, verify
    If MsgBox("Cancel will discard any changes to your original status file. " & vbCrLf & _
               "Are you sure?", vbYesNo, Me.Caption & " Confirmation") = vbYes Then
      'refresh from orig status file
      p.EditFlg = False 'Don't ask whether to save changes
      frmGenScn.OpenStatusFile (p.StatusFileName)
      Unload Me
    End If
  Else
    Unload Me
  End If
End Sub

Private Sub cmdFile_Click(index As Integer)
  Dim iwdm&, i&, s$
  
  On Error GoTo Cancelled
  CDFile.flags = cdlOFNHideReadOnly + cdlOFNOverwritePrompt + cdlOFNPathMustExist + cdlOFNFileMustExist
  Select Case index
    Case 1 'message wdm
      CDFile.filter = "WDM files (*.wdm)|*.wdm"
      CDFile.FileName = "hspfmsg.wdm"
      CDFile.DialogTitle = "GenScn Select HSPF Message File"
      CDFile.CancelError = True
      CDFile.ShowOpen
      On Error GoTo OtherError
      lblFile(index).Caption = CDFile.FileName
      If p.HSPFMsg.Unit > 0 Then
        'message file already open
        i = F90_WDFLCL(p.HSPFMsg.Unit)
        If i <> -87 Then
          'closed properly
          p.HSPFMsg.Unit = 0
        End If
      End If
      p.HSPFMsg.Name = lblFile(index).Caption
      If p.HSPFMsg.Unit = 0 Then
        p.HSPFMsg.Unit = F90_WDBOPN(i, p.HSPFMsg.Name, Len(p.HSPFMsg.Name))
      End If
      If p.HSPFMsg.Unit = 0 Then
        s = p.HSPFMsg.Name
        MsgBox "Could not find WDM file " & s, vbExclamation, Me.Caption & " Problem"
        lblFile(index).Caption = "<none>"
        p.HSPFMsg.Name = ""
      End If
    Case 0, 2 'map
      CDFile.filter = "Map files (*.map)|*.map"
      CDFile.FileName = "*.map"
      CDFile.DialogTitle = "GenScn Map File"
      CDFile.CancelError = True
      If index = 0 Then
        CDFile.ShowSave
        On Error GoTo OtherError
        SaveFileString CDFile.FileName, ""
      Else
        CDFile.ShowOpen
        On Error GoTo OtherError
      End If
      lblFile(2).Caption = CDFile.FileName
      'frmGenScn.Map1.MapFileName = CDFile.filename
      'get reach info from database
      s = ""
      editMade
      'frmGenScn.Map1.SetMapData p.StatusFilePath, CDFile.Filename, s
      frmGenScn.Map1.SetMapData PathNameOnly(CDFile.FileName), CDFile.FileName, s
      'frmGenScn.sstLocation.TabCaption(1) = frmGenScn.Map1.Layers(0).Name
      frmGenScn.Map1.Visible = False
  End Select
Cancelled:
  Exit Sub
OtherError:
  MsgBox err.Description, vbExclamation, "GenScn Manage Project"
End Sub

Private Sub cmdOk_Click()
  frmGenScn.Map1.Visible = True
  MousePointer = vbDefault
  'okay to create new
  'If lblFile(0).Caption = "<none>" Then
    'no ts wdm specified
  '  MsgBox "A Timeseries WDM file must be specified.", _
  '    vbExclamation, Me.Caption & " Problem"
  'ElseIf lblFile(1).Caption = "<none>" Then
    'no HSPF msg wdm specified
  '  MsgBox "A HSPF message file must be specified.", _
  '    vbExclamation, Me.Caption & " Problem"
  'Else
  If p.EditFlg Then 'changes have been made
    'successful opening, (re)initialize genscn
    InitSLCCollections
    'ReDim p.Scen(0)
    'p.ScenCount = 0
    'ReDim p.Cons(0)
    'p.ConsCount = 0
    'ReDim p.locn(0)
    'p.LocnCount = 0
    MousePointer = vbHourglass
'    If p.HSPFMsg.Unit > 0 Then
'      Call F90_WDIINI
'      'If p.WDMData.Unit > 0 Then
'      '  Call F90_WIDADD(p.WDMData.Unit, 10000, "WDM1", Len("WDM1"))
'      '  On Error Resume Next
'      '  Unload Me
'      '  Call F90_TSDRRE(p.WDMData.Unit, 0&, 0&)
'      'Else
'        Unload Me
'      'End If
'    Else
'      Unload Me
'    End If
    'p.StatusFilePath = CurDir
'    frmGenScn.SetFocus
    On Error GoTo 0
    Call RefreshSLC
    'Call frmGenScn.RefreshMain
    If Len(p.StatusFileName) = 0 Then
      frmGenScn.Caption = "GenScn: <new project>"
    ElseIf Right(frmGenScn.Caption, 6) <> "Edited" Then
      frmGenScn.Caption = frmGenScn.Caption & " Edited"
    End If
'        MapPopulateGrid frmGenScn.Map1, frmGenScn.agdMapLocationDetails
'        frmGenScn.lblLocationsSelected.Caption = PtSelCount & " of " & frmGenScn.agdMapLocationDetails.Rows
  End If
  Unload Me
End Sub

'Private Sub cmdOkayCancel_Click(Index As Integer)
'    Dim i&, s$, lp As GenScnProject
'
'    frmGenScn.Map1.Visible = True
'    MousePointer = vbDefault
'    If Index = 0 Then
'      'okay to create new
'      'If lblFile(0).Caption = "<none>" Then
'        'no ts wdm specified
'      '  MsgBox "A Timeseries WDM file must be specified.", _
'      '    vbExclamation, Me.Caption & " Problem"
'      'ElseIf lblFile(1).Caption = "<none>" Then
'        'no HSPF msg wdm specified
'      '  MsgBox "A HSPF message file must be specified.", _
'      '    vbExclamation, Me.Caption & " Problem"
'      'Else
'      If p.EditFlg Then 'changes have been made
'        'successful opening, (re)initialize genscn
'        ReDim p.Scen(0)
'        p.ScenCount = 0
'        ReDim p.Cons(0)
'        p.ConsCount = 0
'        ReDim p.Locn(0)
'        p.LocnCount = 0
'        MousePointer = vbHourglass
'        If p.HSPFMsg.Unit > 0 Then
'          Call F90_WDIINI
'          'If p.WDMData.Unit > 0 Then
'          '  Call F90_WIDADD(p.WDMData.Unit, 10000, "WDM1", Len("WDM1"))
'          '  On Error Resume Next
'          '  Unload Me
'          '  Call F90_TSDRRE(p.WDMData.Unit, 0&, 0&)
'          'Else
'            Unload Me
'          'End If
'        Else
'          Unload Me
'        End If
'        'p.StatusFilePath = CurDir
'        frmGenScn.SetFocus
'        On Error GoTo 0
'        Call RefreshSLC
'        Call frmGenScn.RefreshMain
'        If Len(p.StatusFileName) = 0 Then
'          frmGenScn.Caption = "GenScn: <new project>"
'        ElseIf Right(frmGenScn.Caption, 6) <> "Edited" Then
'          frmGenScn.Caption = frmGenScn.Caption & " Edited"
'        End If
''        MapPopulateGrid frmGenScn.Map1, frmGenScn.agdMapLocationDetails
''        frmGenScn.lblLocationsSelected.Caption = PtSelCount & " of " & frmGenScn.agdMapLocationDetails.Rows
'      Else 'no changes
'        Unload Me
'      End If
'    ElseIf Index = 1 Then 'cancel
'      If Len(p.StatusFileName) = 0 Then 'new, quick cancel makes sense
'        p = lp
'        Unload Me
'      ElseIf p.EditFlg Then 'edits have taken place, verify
'        i = MsgBox("Cancel will discard the changes just made. " & vbCrLf & _
'                   "Are you sure?", vbYesNo, Me.Caption & " Confirmation")
'        If i = vbYes Then
'          'refresh from orig status file
'          frmGenScn.OpenStatusFile (p.StatusFileName)
'          Unload Me
'        End If
'      Else
'        Unload Me
'      End If
'    End If
'End Sub

Private Sub cmdRem_Click()
  Dim curactive As ATCclsTserFile
  If grdTSer.SelCount = 0 Then    'no file selected to remove
    MsgBox "No file selected to remove", vbExclamation, Me.Caption & " Problem"
  ElseIf grdTSer.row = 1 Then
    If InMemFile.DataCount > 0 Then
      If MsgBox("Discard In-Memory data?", vbYesNo, "Removing In-Memory Data") = vbYes Then
        InMemFile.clear
        refreshGrdTSer
        editMade
        txtInfo.text = TserFileProperties(InMemFile)
      End If
    Else
      MsgBox "In-Memory data is already empty.", vbOKOnly, "Removing In-Memory Data"
    End If
  Else
    If TserFiles.Active(grdTSer.row).label = "WDM" Then
      'TserFiles.CurrentActiveIndex = grdTSer.row
      Set curactive = TserFiles.Active(grdTSer.row).obj
      p.WDMFiles.Remove "FU" & curactive.FileUnit
    End If
    Call TserFiles.Delete(grdTSer.row)
    refreshGrdTSer
    Call grdTSer_SelChange(grdTSer.row, 0)
    editMade
    txtInfo.text = ""
  End If
End Sub

Private Sub Form_Load()
  Dim i&
  Dim vTserFile As Variant
  Dim LabelString As String
  Dim cmbWDM As Long
  
  If Len(p.StatusFileName) > 0 Then
    Me.Caption = Me.Caption & ": " & p.StatusFileName
  Else
    Me.Caption = Me.Caption & ": <new project>"
  End If
  'If Len(p.StatusFilePath) = 0 Then
  '   p.StatusFilePath = CurDir & "\"
  'End If
  If p.HSPFMsg.Unit <= 0 Then
    Batch.OpenHspfMsg ExePath & "bin\hspfmsg.wdm"
  End If
  lblFile(1).Caption = p.HSPFMsg.Name
  If Len(frmGenScn.Map1.MapFileName) > 0 Then
    lblFile(2).Caption = frmGenScn.Map1.MapFileName
  Else
    lblFile(2).Caption = "<none>"
  End If
       
  cmbFileType.clear
  For Each vTserFile In TserFiles.Avail
    'If vTserFile.PluginType = "ATCclsTserFile" Then 'We will assume this for this collection
      LabelString = vTserFile.label
      If LabelString = "Memory" Then LabelString = MemoryLabel
      If LabelString <> "Dummy" Then
        If LabelString = "WDM" Then cmbWDM = cmbFileType.ListCount
        cmbFileType.AddItem LabelString
      End If
    'End If
  Next vTserFile
  cmbFileType.ListIndex = cmbWDM
  
  refreshGrdTSer
  grdTSer.colWidth(0) = 1000
  grdTSer.colWidth(1) = 500
  grdTSer.colWidth(2) = grdTSer.Width - 1700
    
End Sub

Private Sub refreshGrdTSer()
  Dim i&, vTserFile As Variant
  Dim tserTmp As ATCclsTserFile
  
    grdTSer.clear
    grdTSer.cols = 3
    grdTSer.ColTitle(0) = "Type"
    grdTSer.ColTitle(1) = "Unit"
    grdTSer.ColTitle(2) = "Name"
    grdTSer.Rows = TserFiles.Active.Count
    i = 0
    For Each vTserFile In TserFiles.Active
      i = i + 1
      grdTSer.TextMatrix(i, 0) = vTserFile.label
      Set tserTmp = vTserFile.obj
      grdTSer.TextMatrix(i, 1) = tserTmp.FileUnit
      grdTSer.TextMatrix(i, 2) = tserTmp.FileName
    Next vTserFile
End Sub

Private Sub Form_Resize()
  Dim w As Long
  Dim h As Long
  w = Me.ScaleWidth
  h = Me.ScaleHeight
  If w > lblFile(1).Left + 500 And h > cmdRem.Top + fraProperties.Height + fraButtons.Height + 120 * 3 Then
    lblFile(1).Width = w - lblFile(1).Left
    lblFile(2).Width = lblFile(1).Width
    grdTSer.Width = w - grdTSer.Left - 120
    fraProperties.Width = w - fraProperties.Left - 120
    txtInfo.Width = fraProperties.Width - 240
    fraButtons.Left = (w - fraButtons.Width) / 2
    
    fraButtons.Top = h - fraButtons.Height - 120
    fraProperties.Top = fraButtons.Top - fraProperties.Height - 120
    grdTSer.Height = fraProperties.Top - grdTSer.Top - 120
  End If
End Sub

Private Sub Form_Unload(Cancel As Integer)
  frmGenScn.Enabled = True
End Sub

Private Sub grdTSer_SelChange(row As Long, col As Long)
  Dim r&
  If col = 0 Then
    If grdTSer.Selected(row, col) Then
      If row > 0 And row <= TserFiles.Active.Count Then
        txtInfo.text = TserFileProperties(TserFiles.Active(row).obj)
      End If
    End If
  End If
End Sub
