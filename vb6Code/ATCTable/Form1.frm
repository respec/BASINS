VERSION 5.00
Object = "{F9043C88-F6F2-101A-A3C9-08002B2F49FB}#1.2#0"; "COMDLG32.OCX"
Object = "{872F11D5-3322-11D4-9D23-00A0C9768F70}#1.10#0"; "ATCoCtl.ocx"
Begin VB.Form frmMain 
   Caption         =   "ATCTable Utility"
   ClientHeight    =   7575
   ClientLeft      =   60
   ClientTop       =   345
   ClientWidth     =   10200
   LinkTopic       =   "Form1"
   OLEDropMode     =   1  'Manual
   ScaleHeight     =   7575
   ScaleWidth      =   10200
   StartUpPosition =   3  'Windows Default
   Begin VB.ComboBox cboDisplay 
      Height          =   315
      Left            =   1440
      TabIndex        =   8
      Text            =   "cboDisplay"
      Top             =   120
      Width           =   1935
   End
   Begin ATCoCtl.ATCoGrid agd 
      Height          =   5775
      Left            =   0
      TabIndex        =   7
      Top             =   600
      Width           =   6855
      _ExtentX        =   12091
      _ExtentY        =   10186
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
   Begin VB.CommandButton cmdWriteSHP 
      Caption         =   "Write SHP"
      Height          =   375
      Left            =   7080
      TabIndex        =   6
      Top             =   120
      Width           =   975
   End
   Begin VB.CommandButton cmdWriteDBF 
      Caption         =   "Save as DBF"
      Height          =   375
      Left            =   5640
      TabIndex        =   5
      Top             =   120
      Width           =   1215
   End
   Begin VB.CommandButton cmdWriteBin 
      Caption         =   "Write bin"
      Height          =   375
      Left            =   8280
      TabIndex        =   4
      Top             =   120
      Visible         =   0   'False
      Width           =   855
   End
   Begin VB.CommandButton cmdRewrite 
      Caption         =   "RewriteDBF"
      Height          =   375
      Left            =   7080
      TabIndex        =   3
      Top             =   120
      Visible         =   0   'False
      Width           =   1215
   End
   Begin VB.CommandButton cmdCopy 
      Caption         =   "Copy to clipboard"
      Height          =   375
      Left            =   3720
      TabIndex        =   2
      Top             =   120
      Width           =   1695
   End
   Begin VB.TextBox txtSummary 
      Height          =   6975
      Left            =   0
      MultiLine       =   -1  'True
      OLEDropMode     =   1  'Manual
      TabIndex        =   1
      Top             =   600
      Visible         =   0   'False
      Width           =   10215
   End
   Begin MSComDlg.CommonDialog cdlg 
      Left            =   120
      Top             =   720
      _ExtentX        =   847
      _ExtentY        =   847
      _Version        =   393216
      CancelError     =   -1  'True
   End
   Begin VB.CommandButton cmdOpen 
      Caption         =   "Open"
      Height          =   375
      Left            =   120
      TabIndex        =   0
      Top             =   120
      Width           =   1095
   End
End
Attribute VB_Name = "frmMain"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
  
Dim pTable As clsATCTable

Private Sub cmdCopy_Click()
  If txtSummary.Visible Then
    Clipboard.SetText txtSummary.Text
  Else
    agd.copyAll
  End If
End Sub

Private Sub cmdOpen_Click()
  On Error GoTo ErrHand
  cdlg.Filter = "*.dbf|*.dbf|*.RT1|*.RT1|*.RT2|*.RT2|All Files (*.*)|*.*"
  cdlg.FilterIndex = 1
  cdlg.ShowOpen
  OpenTable cdlg.filename
ErrHand:
End Sub

Public Sub OpenTable(aFilename As String)
  Caption = aFilename
  
  Set pTable = New clsATCTable
  Set pTable = pTable.OpenFile(aFilename)
  RefreshDisplay
End Sub

Private Sub RefreshDisplay()
  If pTable Is Nothing Then
    txtSummary.Text = "(no table is open)"
    txtSummary.Visible = True
    agd.Visible = False
  Else
    Dim iField As Long
    Dim iRecord As Long
  
    Select Case cboDisplay.Text
      Case "Data"
        txtSummary.Text = ""
        txtSummary.Visible = False
        agd.Visible = True
      Case Else
        txtSummary.Visible = True
        agd.Visible = False
    End Select
    
    Select Case cboDisplay.Text
      Case "Summary":       txtSummary.Text = pTable.Summary
      Case "Creation Code": txtSummary.Text = pTable.CreationCode
      Case "Data"
        For iField = 1 To pTable.NumFields
          agd.ColTitle(iField - 1) = pTable.FieldName(iField)
          For iRecord = 1 To pTable.NumRecords
            pTable.CurrentRecord = iRecord
            agd.TextMatrix(iRecord, iField - 1) = pTable.Value(iField)
          Next
          'DoEvents
        Next
        agd.ColsSizeByContents
        If agd.DesiredWidth + 2000 + (Me.Width - Me.ScaleWidth) < Screen.Width Then
          Me.Width = agd.DesiredWidth + 1850 + (Me.Width - Me.ScaleWidth)
        End If
        If Me.Left + Me.Width > Screen.Width Then Me.Left = Screen.Width - Me.Width
        If agd.DesiredHeight + agd.Top + 50 + (Me.Height - Me.ScaleHeight) < Screen.Height Then
          Me.Height = agd.DesiredHeight + agd.Top + 50 + (Me.Height - Me.ScaleHeight)
        End If
        If Me.Top + Me.Height > Screen.Height Then Me.Top = Screen.Height - Me.Height
      Case "Data Widths"
        Dim maxLength As Long
        txtSummary.Text = ""
        For iField = 1 To pTable.NumFields
          maxLength = 0
          For iRecord = 1 To pTable.NumRecords
            pTable.CurrentRecord = iRecord
            If Len(Trim(pTable.Value(iField))) > maxLength Then maxLength = Len(Trim(pTable.Value(iField)))
          Next
          txtSummary.Text = txtSummary.Text & pTable.FieldName(iField) & " max length = " & maxLength & vbCrLf
          txtSummary.Refresh
        Next
    End Select
  End If
End Sub

're-write DBF tables using field definitions from the currently open table
Private Sub cmdRewrite_Click()
  Dim fromTable As clsATCTable
  Dim toTable As clsATCTable
  Dim iRecord As Long
  Dim TableFolder As String
  Dim TableBaseName As String
  Dim curHUC As String
  
  If pTable Is Nothing Then
    MsgBox "First open a table with the correct fields"
    Exit Sub
  End If
  
  cdlg.Filter = "*.dbf|*.dbf|All Files (*.*)|*.*"
  cdlg.FilterIndex = 1
  cdlg.DialogTitle = "Open a Table to convert"
  cdlg.ShowOpen
  
  TableBaseName = LCase(FilenameOnly(cdlg.filename))
  TableFolder = PathNameOnly(PathNameOnly(cdlg.filename))
  
  ChDriveDir TableFolder
  
  curHUC = Dir("*", vbDirectory)
NextHUC:
  If Len(curHUC) = 8 Then
    Set fromTable = New clsATCTable
    Debug.Print curHUC & "\" & TableBaseName
    Set fromTable = fromTable.OpenFile(curHUC & "\" & TableBaseName & ".dbf")
    
    Set toTable = pTable.Cousin
    toTable.NumRecords = fromTable.NumRecords
    Debug.Print toTable.NumRecords
    'toDBF.InitData
    
    For iRecord = 1 To fromTable.NumRecords
      fromTable.CurrentRecord = iRecord
      toTable.CurrentRecord = iRecord
      Select Case TableBaseName
        Case "cat"
          toTable.Value(5) = fromTable.Value(1)
          toTable.Value(7) = fromTable.Value(2)
          toTable.Value(8) = fromTable.Value(3)
        Case "st"
          toTable.Value(5) = FIPS2Abbrev(fromTable.Value(1))
          If toTable.Value(5) = "AK" Then
            toTable.Value(6) = 10
          ElseIf toTable.Value(5) = "PR" Or toTable.Value(5) = "VI" Then
            toTable.Value(6) = 2
          ElseIf toTable.Value(5) = "HI" Then
            toTable.Value(6) = 9
          Else
            Stop
          End If
        Case "cnty"
          toTable.Value(5) = CLng(fromTable.Value(1))
          toTable.Value(10) = toTable.Value(5)
          toTable.Value(6) = fromTable.Value(2)
          toTable.Value(7) = ReplaceString(fromTable.Value(3), " County", "")
        Case Else
          Stop
      End Select
    Next
    toTable.WriteFile curHUC & "\" & TableBaseName & ".dbf"
  End If
  curHUC = Dir
  If Len(curHUC) > 0 Then GoTo NextHUC
  Exit Sub
ErrHand:
  
End Sub

Private Sub cmdWriteBin_Click()
  SaveAsNewBin pTable
End Sub

Private Sub cmdWriteDBF_Click()
  SaveAsNewDBF pTable
End Sub



Private Sub cmdWriteTigerSHP_Click()
  Dim rt1 As clsATCTable
  Dim rt2 As clsATCTable
  Dim filename As String
  
  If Not pTable Is Nothing Then
    filename = pTable.filename
    Select Case UCase(FileExt(filename))
      Case "RT1": Set rt1 = pTable
      Case "RT2": Set rt2 = pTable
    End Select
    filename = FilenameNoExt(filename)
  End If
    
  If rt1 Is Nothing Then
    Set rt1 = New clsATCTableRT1
    If FileExists(filename & ".RT1") Then
      rt1.OpenFile filename & ".RT1"
    Else
      cdlg.Filter = "*.RT1|*.RT1|All Files (*.*)|*.*"
      cdlg.FilterIndex = 1
      cdlg.DialogTitle = "Open an RT1 file"
      cdlg.DefaultExt = "RT1"
      cdlg.ShowOpen
      rt1.OpenFile cdlg.filename
      filename = FilenameNoExt(cdlg.filename)
    End If
  End If
    
  If rt2 Is Nothing Then
    Set rt2 = New clsATCTableRT2
    If FileExists(filename & ".RT2") Then
      rt2.OpenFile filename & ".RT2"
    Else
      cdlg.Filter = "*.RT2|*.RT2|All Files (*.*)|*.*"
      cdlg.FilterIndex = 1
      cdlg.DefaultExt = "RT2"
      cdlg.DialogTitle = "Open an RT2 file"
      cdlg.ShowOpen
      rt2.OpenFile cdlg.filename
    End If
  End If
  
  cdlg.Filter = "*.shp|*.shp|All Files (*.*)|*.*"
  cdlg.FilterIndex = 1
  cdlg.DefaultExt = "shp"
  cdlg.DialogTitle = "Save as shape file"
  cdlg.ShowSave
  
  WriteShpFromTiger rt1, rt2, cdlg.filename
End Sub

Private Sub cmdWriteSHP_Click()
  Dim latField As Long
  Dim lonField As Long
  Dim fieldNames() As Variant
  Dim m As New ATCoMessage
  
  ReDim fieldNames(pTable.NumFields)
  
  latField = m.ShowArray("Which field contains Latitude or North/South coordinate?", "Select Latitude", fieldNames)
  lonField = m.Show("Which field contains Longitude or East/West coordinate?", "Select Longitude", fieldNames)
  
  WriteShapePointsFromDBF pTable, latField, lonField
End Sub

Private Sub Form_Load()
  With cboDisplay
    .Clear
    .AddItem "Data"
    .AddItem "Summary"
    .AddItem "Creation Code"
    .AddItem "Data Widths"
    .ListIndex = 0
  End With
End Sub

Private Sub Form_OLEDragDrop(Data As DataObject, Effect As Long, Button As Integer, Shift As Integer, X As Single, Y As Single)
  OpenTable Data.Files.Item(1)
End Sub

Private Sub Form_OLEDragOver(Data As DataObject, Effect As Long, Button As Integer, Shift As Integer, X As Single, Y As Single, State As Integer)
  If Data.Files.count = 1 Then
    If FileExt(Data.Files.Item(1)) = "dbf" Then Effect = vbDropEffectCopy
  End If
End Sub

Private Sub txtSummary_OLEDragDrop(Data As DataObject, Effect As Long, Button As Integer, Shift As Integer, X As Single, Y As Single)
  Form_OLEDragDrop Data, Effect, Button, Shift, X, Y
End Sub

Private Sub txtSummary_OLEDragOver(Data As DataObject, Effect As Long, Button As Integer, Shift As Integer, X As Single, Y As Single, State As Integer)
  Form_OLEDragOver Data, Effect, Button, Shift, X, Y, State
End Sub

Private Sub Form_Resize()
  If Me.ScaleHeight > 700 And Me.ScaleWidth > 0 Then
    txtSummary.Height = Me.ScaleHeight - 600
    txtSummary.Width = Me.ScaleWidth
    agd.Height = txtSummary.Height
    agd.Width = txtSummary.Width
  End If
End Sub

Private Sub cboDisplay_Click()
  RefreshDisplay
End Sub

