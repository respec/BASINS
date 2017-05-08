VERSION 5.00
Begin VB.Form frmSWMMInterface 
   Caption         =   "SWMM Interface"
   ClientHeight    =   6435
   ClientLeft      =   6090
   ClientTop       =   720
   ClientWidth     =   5520
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
   Icon            =   "GenSWM.frx":0000
   LinkTopic       =   "Form1"
   ScaleHeight     =   6435
   ScaleWidth      =   5520
   Begin VB.TextBox txtSWOut 
      Height          =   288
      Left            =   2400
      TabIndex        =   3
      Top             =   480
      Width           =   2892
   End
   Begin MSComDlg.CommonDialog cdlSwFile 
      Left            =   4920
      Top             =   240
      _ExtentX        =   688
      _ExtentY        =   688
      _Version        =   393216
      DialogTitle     =   "Select SWMM Transport File"
   End
   Begin VB.ComboBox cboHSPSen 
      Height          =   288
      Left            =   2400
      Style           =   2  'Dropdown List
      TabIndex        =   5
      Top             =   840
      Width           =   1692
   End
   Begin VB.CommandButton cmdSave 
      Caption         =   "&Get Specs"
      Height          =   300
      Index           =   1
      Left            =   2880
      TabIndex        =   17
      Top             =   5640
      Width           =   1212
   End
   Begin VB.CommandButton cmdSave 
      Caption         =   "Sa&ve Specs"
      Height          =   300
      Index           =   0
      Left            =   1440
      TabIndex        =   16
      Top             =   5640
      Width           =   1212
   End
   Begin VB.CommandButton cmdSWMM 
      Cancel          =   -1  'True
      Caption         =   "Cancel"
      Height          =   300
      Index           =   1
      Left            =   2880
      TabIndex        =   19
      Top             =   6000
      Width           =   1212
   End
   Begin VB.CommandButton cmdSWMM 
      Caption         =   "&Simulate"
      Height          =   300
      Index           =   0
      Left            =   1440
      TabIndex        =   18
      Top             =   6000
      Width           =   1212
   End
   Begin VB.CommandButton cmdSwFile 
      Caption         =   "Select SWMM &File"
      Height          =   300
      Left            =   240
      TabIndex        =   0
      Top             =   120
      Width           =   2052
   End
   Begin ATCoCtl.ATCoGrid agdCon 
      Height          =   972
      Left            =   240
      TabIndex        =   9
      Top             =   2640
      Width           =   5052
      _ExtentX        =   8916
      _ExtentY        =   1720
      SelectionToggle =   0   'False
      AllowBigSelection=   0   'False
      AllowEditHeader =   0   'False
      AllowLoad       =   0   'False
      AllowSorting    =   -1  'True
      Rows            =   2
      Cols            =   3
      ColWidthMinimum =   300
      gridFontBold    =   0   'False
      gridFontItalic  =   0   'False
      gridFontName    =   "MS Sans Serif"
      gridFontSize    =   8
      gridFontUnderline=   0   'False
      gridFontWeight  =   400
      gridFontWidth   =   0
      Header          =   "Assign HSPF Constituents to SWMM Pollutants"
      FixedRows       =   1
      FixedCols       =   0
      ScrollBars      =   3
      SelectionMode   =   0
      BackColor       =   -2147483643
      ForeColor       =   -2147483640
      BackColorBkg    =   -2147483633
      BackColorSel    =   -2147483634
      ForeColorSel    =   16777215
      BackColorFixed  =   -2147483633
      ForeColorFixed  =   -2147483630
      InsideLimitsBackground=   -2147483643
      OutsideHardLimitBackground=   8421631
      OutsideSoftLimitBackground=   8454143
      ComboCheckValidValues=   -1  'True
   End
   Begin VB.Frame fraLoc 
      Caption         =   "&Location Connections"
      Height          =   1815
      Left            =   240
      TabIndex        =   10
      Top             =   3720
      Width           =   5052
      Begin VB.CommandButton cmdLoc 
         Caption         =   "Clea&r"
         Height          =   300
         Index           =   3
         Left            =   3960
         TabIndex        =   15
         Top             =   1320
         Width           =   852
      End
      Begin VB.CommandButton cmdLoc 
         Caption         =   "D&elete"
         Height          =   300
         Index           =   2
         Left            =   2880
         TabIndex        =   14
         Top             =   1320
         Width           =   852
      End
      Begin VB.CommandButton cmdLoc 
         Caption         =   "&Add"
         Height          =   300
         Index           =   1
         Left            =   1800
         TabIndex        =   13
         Top             =   1320
         Width           =   852
      End
      Begin VB.CommandButton cmdLoc 
         Caption         =   "Co&nnect All"
         Height          =   300
         Index           =   0
         Left            =   240
         TabIndex        =   12
         Top             =   1320
         Width           =   1332
      End
      Begin ATCoCtl.ATCoGrid agdLoc 
         Height          =   972
         Left            =   120
         TabIndex        =   11
         Top             =   240
         Width           =   4836
         _ExtentX        =   8520
         _ExtentY        =   1720
         SelectionToggle =   0   'False
         AllowBigSelection=   0   'False
         AllowEditHeader =   0   'False
         AllowLoad       =   0   'False
         AllowSorting    =   -1  'True
         Rows            =   2
         Cols            =   3
         ColWidthMinimum =   300
         gridFontBold    =   0   'False
         gridFontItalic  =   0   'False
         gridFontName    =   "MS Sans Serif"
         gridFontSize    =   8
         gridFontUnderline=   0   'False
         gridFontWeight  =   400
         gridFontWidth   =   0
         Header          =   "Assign HSPF Locations to SWMM Elements"
         FixedRows       =   1
         FixedCols       =   0
         ScrollBars      =   3
         SelectionMode   =   0
         BackColor       =   -2147483643
         ForeColor       =   -2147483640
         BackColorBkg    =   -2147483633
         BackColorSel    =   -2147483634
         ForeColorSel    =   16777215
         BackColorFixed  =   -2147483633
         ForeColorFixed  =   -2147483630
         InsideLimitsBackground=   -2147483643
         OutsideHardLimitBackground=   8421631
         OutsideSoftLimitBackground=   8454143
         ComboCheckValidValues=   -1  'True
      End
   End
   Begin VB.Frame fraDumDates 
      Caption         =   "Dates"
      Height          =   1332
      Left            =   240
      TabIndex        =   6
      Top             =   1200
      Width           =   5052
      Begin VB.Label lblDumDates 
         Caption         =   "No common period available for specified SWMM and HSPF simulations."
         Height          =   492
         Left            =   120
         TabIndex        =   7
         Top             =   360
         Width           =   4332
      End
   End
   Begin ATCoCtl.ATCoDate adtSWMM 
      Height          =   1356
      Left            =   240
      TabIndex        =   8
      Top             =   1200
      Width           =   5124
      _ExtentX        =   9049
      _ExtentY        =   2381
      TUnit           =   4
      TAggr           =   1
      TStep           =   1
      CurrE           =   35399
      CurrS           =   35399
      LimtE           =   35399
      LimtS           =   35399
      DispL           =   1
      LabelCurrentRange=   "Simulate"
      TstepVisible    =   0   'False
   End
   Begin VB.Label lblSWOut 
      Caption         =   "SWMM &Output File:"
      Height          =   252
      Left            =   240
      TabIndex        =   2
      Top             =   480
      Width           =   2052
   End
   Begin VB.Label lblHSPSen 
      Caption         =   "Select &HSPF Scenario:"
      Height          =   252
      Left            =   240
      TabIndex        =   4
      Top             =   840
      Width           =   2052
   End
   Begin VB.Label lblSwFile 
      Caption         =   "<none>"
      Height          =   252
      Left            =   2400
      TabIndex        =   1
      Top             =   160
      Width           =   2892
   End
End
Attribute VB_Name = "frmSWMMInterface"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants

Dim s As SwmmData
Dim SWPath As String, SWInFile As String
'Dim lc&, lts() As Timser
Dim lTs As Collection
Dim ssdate&(5), sedate&(5)
Dim nHSPCons&, HSPCons$()
Dim nHSPLocs&, HSPLocs$()

Private Sub agdCon_RowColChange()

  Dim i&

  With agdCon
    .ClearValues
    If .Col = 1 Then 'put HSPF constituents in list
      For i = 0 To nHSPCons - 1
        .addValue HSPCons(i)
      Next i
    End If
  End With

End Sub

Private Sub agdLoc_CommitChange(ChangeFromRow As Long, ChangeToRow As Long, ChangeFromCol As Long, ChangeToCol As Long)

  Dim i&, j&, failfg As Boolean, failstr$

  If ChangeFromCol = 1 Then
    'check entered location availability of selected constituents
    failfg = False
    failstr = "The following selected HSPF constituents are not available for the location entered:" & vbCrLf
    For i = 1 To agdCon.Rows
      If Len(agdCon.TextMatrix(i, 1)) > 0 Then
        j = 0
        Do While j < lTs.Count
          If lTs(j).loc = agdLoc.TextMatrix(ChangeFromRow, ChangeFromCol) And _
             lTs(j).con = agdCon.TextMatrix(i, 1) Then
            'location and constituent match
            j = lTs.Count
          End If
          j = j + 1
        Loop
        If j = lTs.Count Then 'didn't find match,
          'entered location does not have specified constituent
          failfg = True
          failstr = failstr & agdCon.TextMatrix(i, 1) & ", "
        End If
      End If
    Next i
    If failfg Then 'let user know about problems
      failstr = Left(failstr, Len(failstr) - 2)
      MsgBox failstr, vbInformation, "GenScn SWMM Interface Problem"
    End If
  End If

End Sub

Private Sub agdLoc_RowColChange()

  Dim i&

  With agdLoc
    .ClearValues
    If .Col = 0 Then 'put SWMM element locations in list
      For i = 0 To s.Locats - 1
        .addValue s.l(i).Name
      Next i
    ElseIf .Col = 1 Then 'put HSPF locations in list
      For i = 0 To nHSPLocs - 1
        .addValue HSPLocs(i)
      Next i
    End If
  End With

End Sub

Private Sub cboHSPSen_Click()

  Dim i&, j&

  MousePointer = vbHourglass
  Call FindTimSer(cboHSPSen.text, "", "", lTs)
  nHSPCons = 0
  nHSPLocs = 0
  For i = 1 To lTs.Count
    'add selected scenario's constituents to list
    j = 0
    Do While j < nHSPCons
      If lTs(i).con = HSPCons(j) Then 'already in list
        j = nHSPCons
      End If
      j = j + 1
    Loop
    If j = nHSPCons Then 'not in list, add it
      ReDim Preserve HSPCons(nHSPCons)
      HSPCons(nHSPCons) = lTs(i).con
      nHSPCons = nHSPCons + 1
    End If
    'set valid constituent values in grid
    agdCon.Col = 1
    Call agdCon_RowColChange
    'add selected scenario's locations to list
    j = 0
    Do While j < nHSPLocs
      If lTs(i).loc = HSPLocs(j) Then 'already in list
        j = nHSPLocs
      End If
      j = j + 1
    Loop
    If j = nHSPLocs Then 'not in list, add it
      ReDim Preserve HSPLocs(nHSPLocs)
      HSPLocs(nHSPLocs) = lTs(i).loc
      nHSPLocs = nHSPLocs + 1
    End If
  Next i
  agdLoc.Col = 0 'forces RowColChange event to get locations put in list
  Call SetCommonDates 'try to find common dates
  MousePointer = vbDefault

End Sub

Private Sub cmdLoc_Click(index As Integer)

  Dim i&, j&, irw&

  With agdLoc
    If index = 0 Then 'connect all locations to each other
      .Rows = s.Locats * nHSPLocs
      irw = 1
      For i = 0 To s.Locats - 1
        For j = 0 To nHSPLocs - 1
          .TextMatrix(irw, 0) = s.l(i).Name
          .TextMatrix(irw, 1) = HSPLocs(j)
          .TextMatrix(irw, 2) = "1.0"
          irw = irw + 1
        Next j
      Next i
    ElseIf index = 1 Then 'add a row for new connection
      .Rows = .Rows + 1
    ElseIf index = 2 Then 'delete an existing connection
      If .row > 0 Then
        For i = .row To .Rows - 1
          For j = 0 To 2
            .TextMatrix(i, j) = .TextMatrix(i + 1, j)
          Next j
        Next i
        .Rows = .Rows - 1
      End If
    ElseIf index = 3 Then 'clear all rows
      .Rows = 1
      For j = 0 To 2
        .TextMatrix(1, j) = ""
      Next j
    End If
  End With
  Call fraLocResize

End Sub

Private Sub cmdSave_Click(index As Integer)
  
  On Error GoTo 10
  cdlSwFile.CancelError = True
  cdlSwFile.filter = "GenScn SWMM Specs Files (*.sws)|*.sws"
  cdlSwFile.Filename = "*.sws"
  If index = 0 Then 'save specs file
    cdlSwFile.flags = &H8806&
    cdlSwFile.DialogTitle = "GenScn SWMM Save Specifications File"
    cdlSwFile.Action = 2
    Call SaveSpecs(cdlSwFile.Filename)
  ElseIf index = 1 Then 'get specs file
    cdlSwFile.flags = &H1806&
    cdlSwFile.DialogTitle = "GenScn SWMM Get Specifications File"
    cdlSwFile.Action = 1
    Call GetSpecs(cdlSwFile.Filename)
  End If
10 'continue here on cancel

End Sub

Private Sub cmdSwFile_Click()

  Dim f$, ierr%, i&, j&

  cdlSwFile.DialogTitle = "Select SWMM Transport File"
  cdlSwFile.filter = "SWMM files (*.doc)|*.doc|All files (*.*)|*.*"
  cdlSwFile.Filename = "*.doc"
  ierr = 0
  On Error GoTo errhandler
  cdlSwFile.CancelError = True
  cdlSwFile.Action = 1
  ierr = 1
  'update swmm file and path names, parse info from input file
  Call UpdateSWInFile(cdlSwFile.Filename)

errhandler:
  If ierr = 1 Then 'problem parsing file
  
  End If
End Sub

Private Sub cmdSWMM_Click(index As Integer)

  Dim i&, str$, lstr$, written&, buf() As Byte

  If index = 0 Then 'perform SWMM simulation
    ChDriveDir SWPath 'set path same as input swmm file
    If InputOK Then
      Me.MousePointer = vbHourglass
      Call WDM2SWMM(i) 'write hspf data to swmm binary file
      If i = 0 Then 'swmm binary file ok
        
        IPC.SendMonitorMessage "(OPEN GenScn HSPF/SWMM Simulation)"
        IPC.SendMonitorMessage "(BUTTOFF CANCEL)"
        IPC.SendMonitorMessage "(BUTTOFF PAUSE)"
        
        lstr = SWExeName & " " & SWInFile & " " & txtSWOut.text & vbNullString  'FileNameOnly(txtSWOut.Text) & ".out " & vbNullString
        IPC.StartProcess "SWMM", lstr, 600
        IPC.SendMonitorMessage "(CLOSE)"
        IPC.SendMonitorMessage "(BUTTON CANCEL)"
        IPC.SendMonitorMessage "(BUTTON PAUSE)"
      End If
      Me.MousePointer = vbDefault
    End If
  ElseIf index = 1 Then 'cancel
    s.Locats = 0
    s.Npoll = 0
    s.Idatez = 0
    Unload Me
  End If

End Sub

Private Sub Form_Load()

  Dim i&, vName As Variant

  cboHSPSen.clear
  For Each vName In p.ScenName
    cboHSPSen.AddItem vName
  Next
  With agdCon
    .ColTitle(0) = "SWMM Constituent"
    .TextMatrix(1, 0) = "Flow"
    .ColTitle(1) = "HSPF Constituent"
    .ColTitle(2) = "Units Conversion Factor"
    For i = 1 To 2
      .ColEditable(i) = True
    Next i
    .ColType(2) = ATCoCtl.ATCoSng
    .ColMin(2) = 0
    .ColMax(2) = -999
  End With
  With agdLoc
    .ColTitle(0) = "SWMM Element"
    .ColTitle(1) = "HSPF Location"
    .ColTitle(2) = "Area Factor"
    For i = 0 To 2
      .ColEditable(i) = True
    Next i
    .ColType(2) = ATCoCtl.ATCoSng
    .ColMin(2) = 0
    .ColMax(2) = -999
  End With
  Call RefreshSWMMInfo
  
End Sub

Private Sub RefreshSWMMInfo()

  Dim i&, j&

  Call SetCommonDates 'set available dates
  With agdCon
    .Rows = s.Npoll + 1
    For i = 0 To s.Npoll - 1
      .TextMatrix(i + 2, 0) = s.p(i).Pname
    Next i
    If .Rows = 0 Then 'show one row
      .Height = 732
    ElseIf .Rows < 10 Then 'size to show all rows
      .Height = .Rows * 240 + 492
    Else 'size to show max of 10 rows
      .Height = 2892
    End If
    .Col = 1
  End With
  fraLoc.Top = agdCon.Top + agdCon.Height + 108
  agdLoc.Rows = s.Locats
  For i = 1 To s.Locats
    agdLoc.TextMatrix(i, 0) = s.l(i - 1).Name
    agdLoc.TextMatrix(i, 2) = "1"
  Next i
  Call fraLocResize

End Sub

Private Sub fraLocResize()

  Dim i&

  With agdLoc
    If .Rows = 0 Then 'show one row
      .Height = 732
    ElseIf .Rows < 10 Then 'size to show all rows
      .Height = .Rows * 240 + 492
    Else 'size to show max of 10 rows
      .Height = 2892
    End If
  End With
  cmdLoc(0).Top = agdLoc.Top + agdLoc.Height + 108
  For i = 1 To 3
    cmdLoc(i).Top = cmdLoc(0).Top
  Next i
  fraLoc.Height = cmdLoc(0).Top + cmdLoc(0).Height + 60
  cmdSave(0).Top = fraLoc.Top + fraLoc.Height + 108
  For i = 1 To cmdSave.Count - 1
    cmdSave(i).Top = cmdSave(0).Top
  Next i
  cmdSWMM(0).Top = cmdSave(0).Top + cmdSave(0).Height + 110
  For i = 1 To cmdSave.Count - 1
    cmdSWMM(i).Top = cmdSWMM(0).Top
  Next i
  If Me.WindowState = vbNormal Then
    frmSWMMInterface.Height = cmdSWMM(0).Top + cmdSWMM(0).Height + 480
  End If

End Sub

Private Sub SetCommonDates()

  Dim i&
  Dim Strt&(11), Stp&(11), TUnits&(1), TSTEP&(1)
  Dim ComStrt&(5), ComStp&(5), retcod&, Its&, nd&

  If s.Idatez > 0 Then 'try to find common dates
    Strt(0) = Int(s.Idatez / 10000)
    Strt(1) = Int((s.Idatez - (10000 * Strt(0))) / 100)
    Strt(2) = s.Idatez Mod 100
    Strt(3) = s.Tzero
    If Strt(0) < 30 Then 'assume > year 2000
      Strt(0) = 2000 + Strt(0)
    Else 'assume < year 2000
      Strt(0) = 1900 + Strt(0)
    End If
    TSTEP(0) = s.t(0).Delta
    TUnits(0) = 1
    Call F90_TIMADD(Strt(0), TUnits(0), TSTEP(0), s.Tcount, Stp(0))
    nd = 1
  Else
    nd = 0
  End If
  If Len(cboHSPSen.text) > 0 Then 'look for common period with HSPF scenario
    'try to find a flow timeseries
    Its = 0
    For i = 1 To lTs.Count
      If lTs(i).con = "FLOW" Then
        Its = i
        i = lTs.Count
      End If
    Next
    'if flow time series not found, 1st will be used
    For i = 0 To 5
      Strt(i + 6 * nd) = lTs(Its).sdat(i)
      Stp(i + 6 * nd) = lTs(Its).edat(i)
    Next i
    TSTEP(nd) = lTs(Its).ts
    TUnits(nd) = lTs(Its).Tu
    nd = nd + 1
  End If
  If nd = 0 Then 'nothing yet specified
    fraDumDates.Visible = True
    lblDumDates.Caption = "No Dates yet available."
  ElseIf nd = 1 Then 'just show SWMM or HSPF period
    fraDumDates.Visible = False
    adtSWMM.Visible = True
    adtSWMM.LimtS = DateSerial(Strt(0), Strt(1), Strt(2)) + TimeSerial(Strt(3), Strt(4), Strt(5))
    adtSWMM.LimtE = DateSerial(Stp(0), Stp(1), Stp(2)) + TimeSerial(Stp(3), Stp(4), Stp(5))
    adtSWMM.CurrS = DateSerial(Strt(0), Strt(1), Strt(2)) + TimeSerial(Strt(3), Strt(4), Strt(5))
    adtSWMM.CurrE = DateSerial(Stp(0), Stp(1), Stp(2)) + TimeSerial(Stp(3), Stp(4), Stp(5))
  ElseIf nd = 2 Then 'try to find common period
    Call F90_DTMCMN(nd, Strt(0), Stp(0), TSTEP(0), TUnits(0), ComStrt(0), ComStp(0), TSTEP(0), TUnits(0), retcod)
    If retcod = 0 Then 'valid common period
      fraDumDates.Visible = False
      adtSWMM.Visible = True
      adtSWMM.LimtS = DateSerial(ComStrt(0), ComStrt(1), ComStrt(2)) + TimeSerial(ComStrt(3), ComStrt(4), ComStrt(5))
      adtSWMM.LimtE = DateSerial(ComStp(0), ComStp(1), ComStp(2)) + TimeSerial(ComStp(3), ComStp(4), ComStp(5))
      adtSWMM.CurrS = DateSerial(ComStrt(0), ComStrt(1), ComStrt(2)) + TimeSerial(ComStrt(3), ComStrt(4), ComStrt(5))
      adtSWMM.CurrE = DateSerial(ComStp(0), ComStp(1), ComStp(2)) + TimeSerial(ComStp(3), ComStp(4), ComStp(5))
    Else 'no common period, show dummy dates frame
      fraDumDates.Visible = True
      lblDumDates.Caption = "No common period available for specified SWMM and HSPF simulations."
      adtSWMM.Visible = False
    End If
  End If

End Sub

Private Function InputOK() As Boolean

  'check validity of HSPF and SWMM specs
  Dim i&, j&, failstr$, failfg As Boolean

  failfg = False
  failstr = ""
  If Len(SWInFile) > 0 And Len(txtSWOut.text) > 0 Then 'swmm input/output files ok
    If Len(s.fname) > 0 Then 'interface file ok
      If Len(cboHSPSen.text) > 0 Then 'hspf scenario ok
        For i = 1 To agdCon.Rows 'scan through constituent assignments
          If Len(agdCon.TextMatrix(i, 1)) = 0 Then 'no HSPF constituent entered
            failstr = failstr & "No HSPF constituent entered for SWMM constituent " & agdCon.TextMatrix(i, 0) & vbCrLf
            failfg = True
          End If
        Next i
      Else
        failfg = True
        failstr = "No HSPF scenario has been specified."
      End If
    Else 'no interface file on input file
      failfg = True
      failstr = "The specified Transport input file does not contain a SWMM interface file." & vbCrLf & _
             "HSPF runoff data is transferred to SWMM via the interface file." & vbCrLf & _
             "Please modify the Transport input file to reference an interface file (even if it does not exist)."
    End If
  Else
    failfg = True
    failstr = "SWMM input and/or output files have NOT been specified."
  End If
  If failfg Then 'display constituent problem message
    MsgBox failstr, vbExclamation, "GenScn SWMM Problem"
    InputOK = False
  Else
    InputOK = True
  End If

End Function

Private Sub WDM2SWMM(retcod&)

  Dim i&, j&, k&, il&, it&, Tu&, dt&, qfg&
  Dim lsdat&(5), cdat&(5), failstr$, failfg As Boolean
  Dim unconv!, arconv!, lval!

  failfg = False
  'set WDM Data retrieval parameters
  Tu = 1 'time units always seconds
  dt = 0 'ave/same data transformation
  qfg = 31
  lsdat(0) = Year(adtSWMM.CurrS)
  lsdat(1) = Month(adtSWMM.CurrS)
  lsdat(2) = Day(adtSWMM.CurrS)
  lsdat(3) = Hour(adtSWMM.CurrS)
  lsdat(4) = Minute(adtSWMM.CurrS)
  lsdat(5) = Second(adtSWMM.CurrS)
  
  'build SWMM start date/time values
  Call Date2SWMMDatTim(lsdat(), s.Idatez, s.Tzero)
  For i = 0 To s.Tcount - 1
    Call F90_TIMADD(lsdat(0), Tu, CLng(s.t(0).Delta), i, cdat(0))
    s.t(i).JDay = Date2J(cdat())
    s.t(i).Delta = s.t(0).Delta
  Next i
  s.Source = "Runoff Block"
  s.Title3 = s.Title1
  s.Title4 = s.Title2
  s.Qconv = 1#
  s.Jce = 0 'assume names are numeric
  s.Locats = 0 'will rebuild locations structure based on elements selected for use
  For i = 1 To agdLoc.Rows
    If Len(agdLoc.TextMatrix(i, 0)) > 0 And _
       Len(agdLoc.TextMatrix(i, 1)) > 0 Then
      'valid SWMM and HSPF locations selected
      j = 0
      il = -1
      Do While j < s.Locats
        If s.l(j).Name = agdLoc.TextMatrix(i, 0) Then
          'this SWMM location exists
          il = j
          j = s.Locats
        End If
        j = j + 1
      Loop
      If il = -1 Then 'need new location
        il = s.Locats
        ReDim Preserve s.l(il)
        s.l(il).Name = agdLoc.TextMatrix(i, 0)
        If Not IsNumeric(s.l(il).Name) Then 'alphanumeric names
          s.Jce = 1
        End If
        ReDim s.l(il).q.v(s.Tcount)
        For k = 1 To s.Npoll
          ReDim s.l(il).Poll(k).v(s.Tcount)
        Next k
        s.Locats = s.Locats + 1
      End If
      For k = 1 To agdCon.Rows
        'find and retrieve data from HSPF time series
        If Len(agdCon.TextMatrix(k, 1)) > 0 Then
          it = -1
          For j = 1 To lTs.Count 'look for HSPF time series
            If lTs(j).loc = agdLoc.TextMatrix(i, 1) And _
               lTs(j).con = agdCon.TextMatrix(k, 1) Then
              'use this time series
              it = j
              j = lTs.Count
            End If
          Next
          If it > -1 Then 'flow data set found
            ReDim lTs(it).Vals(s.Tcount)
            Call F90_WDTGET(p.WDMFiles(1).fileUnit, CLng(lTs(it).id), CLng(s.t(0).Delta), lsdat(0), s.Tcount, dt, qfg, Tu, lTs(it).Vals(0), retcod)
            If Len(agdCon.TextMatrix(k, 2)) > 0 Then
              unconv = CSng(agdCon.TextMatrix(k, 2))
            Else 'no conversion entered
              unconv = 1#
            End If
            If Len(agdLoc.TextMatrix(i, 2)) > 0 Then
              arconv = CSng(agdLoc.TextMatrix(i, 2))
            Else 'no area conversion entered
              arconv = 1#
            End If
            If k = 2 Then 'make space for pollutant values in location data
              ReDim Preserve s.l(il).Poll(s.Npoll)
            End If
            For j = 0 To s.Tcount - 1
              lval = lTs(it).Vals(j) * unconv * arconv
              If k = 1 Then 'fill flow data
                s.l(il).q.v(j) = s.l(il).q.v(j) + lval
              Else 'fill pollutant data
                s.l(il).Poll(k - 2).v(j) = s.l(il).Poll(k - 2).v(j) + lval
              End If
            Next j
          Else 'no data set found
            failfg = True
            failstr = failstr & "Time series could not be found for HSPF constituent " & agdCon.TextMatrix(k, 1) & " at location " & agdLoc.TextMatrix(i, 1) & vbCrLf
          End If
        End If
      Next k
    End If
  Next i

  If Not failfg Then 'ok to do simulation, write out interface file
    Call SwmmWrite(s)
    retcod = 0
  Else 'problem, don't do simulation
    failstr = failstr & "Will not be able to perform simulation."
    MsgBox failstr, vbExclamation, "GenScn SWMM Problem"
    retcod = 1
  End If

End Sub

Private Sub GetSpecs(f$)

  Dim i&, ifl&, lstr$, lstr1$, lstr2$, Strt&(5), Stp&(5)

  ifl = FreeFile(0)
  Open f For Input As #ifl
  Input #ifl, lstr
  'update swmm input file and path names, parse info from input file
  Call UpdateSWInFile(lstr)
  Input #ifl, lstr
  txtSWOut.text = lstr
  Input #ifl, lstr
  cboHSPSen.text = lstr
  Input #ifl, Strt(0), Strt(1), Strt(2), Strt(3), Strt(4), Strt(5)
  Input #ifl, Stp(0), Stp(1), Stp(2), Stp(3), Stp(4), Stp(5)
  adtSWMM.CurrS = DateSerial(Strt(0), Strt(1), Strt(2)) + TimeSerial(Strt(3), Strt(4), Strt(5))
  adtSWMM.CurrE = DateSerial(Stp(0), Stp(1), Stp(2)) + TimeSerial(Stp(3), Stp(4), Stp(5))
  With agdCon
    Input #ifl, i
    .Rows = i
    For i = 1 To .Rows
      Input #ifl, lstr, lstr1, lstr2
      .TextMatrix(i, 0) = lstr
      .TextMatrix(i, 1) = lstr1
      .TextMatrix(i, 2) = lstr2
    Next i
  End With
  With agdLoc
    Input #ifl, i
    .Rows = i
    For i = 1 To .Rows
      Input #ifl, lstr, lstr1, lstr2
      .TextMatrix(i, 0) = lstr
      .TextMatrix(i, 1) = lstr1
      .TextMatrix(i, 2) = lstr2
    Next i
  End With
  Call fraLocResize
  Close #ifl

End Sub
Private Sub SaveSpecs(f$)
  
  Dim i&, ofl&

  ofl = FreeFile(0)
  Open f For Output As #ofl
  Write #ofl, lblSwFile.Caption
  Write #ofl, txtSWOut.text
  Write #ofl, cboHSPSen.text
  Write #ofl, Year(adtSWMM.CurrS), Month(adtSWMM.CurrS), Day(adtSWMM.CurrS), Hour(adtSWMM.CurrS), Minute(adtSWMM.CurrS), Second(adtSWMM.CurrS)
  Write #ofl, Year(adtSWMM.CurrE), Month(adtSWMM.CurrE), Day(adtSWMM.CurrE), Hour(adtSWMM.CurrE), Minute(adtSWMM.CurrE), Second(adtSWMM.CurrE)
  With agdCon
    Write #ofl, .Rows
    For i = 1 To .Rows
      Write #ofl, .TextMatrix(i, 0), .TextMatrix(i, 1), .TextMatrix(i, 2)
    Next i
  End With
  With agdLoc
    Write #ofl, .Rows
    For i = 1 To .Rows
      Write #ofl, .TextMatrix(i, 0), .TextMatrix(i, 1), .TextMatrix(i, 2)
    Next i
  End With
  Close #ofl

End Sub

Private Sub UpdateSWInFile(fname$)
  
  SWPath = PathNameOnly(fname)
  If Len(SWPath) > 0 Then 'extract just file name from full path
    SWInFile = Mid(fname, Len(SWPath) + 2)
  Else 'no path name
    SWInFile = fname
  End If
  Call SwTransInRead(fname, s)
  If Len(s.fname) = 0 Then 'problem, no interface file on input file
    MsgBox "The specified Transport input file does not contain a SWMM interface file." & vbCrLf & _
           "HSPF runoff data is transferred to SWMM via the interface file." & vbCrLf & _
           "Please modify the Transport input file to reference an interface file (even if it does not exist).", vbExclamation, "GenScn SWMM Problem"
  End If
  txtSWOut.text = SWPath & "\" & FilenameOnly(fname) & ".out"
  lblSwFile.Caption = fname
  Call RefreshSWMMInfo
  agdLoc.Col = 1 'forces RowColChange event to get locations put in list

End Sub
