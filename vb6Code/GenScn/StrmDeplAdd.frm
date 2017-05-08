VERSION 5.00
Begin VB.Form frmStrmDeplAdd 
   BorderStyle     =   1  'Fixed Single
   Caption         =   "GenScn Stream Depletion Add"
   ClientHeight    =   1980
   ClientLeft      =   48
   ClientTop       =   336
   ClientWidth     =   7608
   Icon            =   "StrmDeplAdd.frx":0000
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   1980
   ScaleWidth      =   7608
   Begin ATCoCtl.ATCoText ATxComb 
      Height          =   255
      Left            =   5280
      TabIndex        =   9
      Top             =   960
      Width           =   1335
      _ExtentX        =   2350
      _ExtentY        =   445
      InsideLimitsBackground=   16777215
      OutsideHardLimitBackground=   8421631
      OutsideSoftLimitBackground=   8454143
      HardMax         =   -999
      HardMin         =   0
      SoftMax         =   -999
      SoftMin         =   -999
      MaxWidth        =   5
      Alignment       =   1
      DataType        =   0
      DefaultValue    =   "0"
      Value           =   "0"
      Enabled         =   -1  'True
   End
   Begin ATCoCtl.ATCoText ATxDepl 
      Height          =   255
      Left            =   3600
      TabIndex        =   7
      Top             =   960
      Width           =   1335
      _ExtentX        =   2350
      _ExtentY        =   445
      InsideLimitsBackground=   16777215
      OutsideHardLimitBackground=   8421631
      OutsideSoftLimitBackground=   8454143
      HardMax         =   -999
      HardMin         =   0
      SoftMax         =   -999
      SoftMin         =   -999
      MaxWidth        =   5
      Alignment       =   1
      DataType        =   0
      DefaultValue    =   "0"
      Value           =   "0"
      Enabled         =   -1  'True
   End
   Begin ATCoCtl.ATCoText ATxPump 
      Height          =   255
      Left            =   1920
      TabIndex        =   5
      Top             =   960
      Width           =   1335
      _ExtentX        =   2350
      _ExtentY        =   445
      InsideLimitsBackground=   16777215
      OutsideHardLimitBackground=   8421631
      OutsideSoftLimitBackground=   8454143
      HardMax         =   -999
      HardMin         =   0
      SoftMax         =   -999
      SoftMin         =   -999
      MaxWidth        =   5
      Alignment       =   1
      DataType        =   0
      DefaultValue    =   "0"
      Value           =   "0"
      Enabled         =   -1  'True
   End
   Begin VB.ComboBox cmbLoc 
      Height          =   315
      Left            =   240
      Style           =   2  'Dropdown List
      TabIndex        =   3
      Top             =   960
      Width           =   1335
   End
   Begin VB.CommandButton cmdCancel 
      Cancel          =   -1  'True
      Caption         =   "&Cancel"
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
      Left            =   3960
      TabIndex        =   11
      Top             =   1440
      Width           =   1215
   End
   Begin VB.CommandButton cmdAdd 
      Caption         =   "&Add"
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
      Left            =   2400
      TabIndex        =   10
      Top             =   1440
      Width           =   1215
   End
   Begin VB.CommandButton cmdFile 
      Caption         =   "&Input File"
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
      Width           =   1335
   End
   Begin MSComDlg.CommonDialog CDFile 
      Index           =   0
      Left            =   360
      Top             =   1440
      _ExtentX        =   699
      _ExtentY        =   699
      _Version        =   393216
      FontSize        =   3.90888e-38
   End
   Begin VB.Label lblCompDepl 
      Caption         =   "C&ombined Depletion Dsn:"
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
      Left            =   5280
      TabIndex        =   8
      Top             =   720
      Width           =   2415
   End
   Begin VB.Label lblDeplDsn 
      Caption         =   "&Depletion Dsn:"
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
      Left            =   3600
      TabIndex        =   6
      Top             =   720
      Width           =   1455
   End
   Begin VB.Label lblPumpDsn 
      Caption         =   "&Pump Rate Dsn:"
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
      Left            =   1920
      TabIndex        =   4
      Top             =   720
      Width           =   1455
   End
   Begin VB.Label Label2 
      Caption         =   "&Location:"
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
      TabIndex        =   2
      Top             =   720
      Width           =   1335
   End
   Begin VB.Label lblFile 
      Caption         =   "<none>"
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
      Left            =   1800
      TabIndex        =   1
      Top             =   240
      Width           =   5535
   End
End
Attribute VB_Name = "frmStrmDeplAdd"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants
Dim nHSPLocs&, HSPLocs$()
Dim Scen$
'Dim lc&, lts() As Timser
Dim lTs As Collection
Dim WellID As String
Dim Reach As String
Dim PumpRateDsn As Long
Dim DepletionDsn As Long
Dim CombDepDsn As Long
Dim StreamDist As Single
Dim Diffusivity As Single
Dim StreamSemi As Boolean
Dim Leakance As Single
Dim PriorPumpDays As Long
Dim PriorPumpRate As Single
Dim NumDays As Long
Dim Description As String

Private Sub ATxDepl_Change()
  If lblFile.Caption <> "<none>" And ATxPump.value > 0 And ATxDepl.value > 0 Then
    cmdAdd.Enabled = True
  Else
    cmdAdd.Enabled = False
  End If
End Sub

Private Sub ATxPump_Change()
  If lblFile.Caption <> "<none>" And ATxPump.value > 0 And ATxDepl.value > 0 Then
    cmdAdd.Enabled = True
  Else
    cmdAdd.Enabled = False
  End If
End Sub

Private Sub cmbLoc_Change()
  'default combined depletion dsn if exists
  Call FindTimSer(Scen, cmbLoc.List(cmbLoc.ListIndex), "ExDemand", lTs)
  If lTs.Count > 0 Then
    ATxComb.value = lTs(0).id
  Else
    ATxComb.value = 0
  End If
End Sub
Public Sub GetScen(n As String)
  Scen = n
End Sub

Private Sub cmbLoc_Click()
  'default combined depletion dsn if exists
  Call FindTimSer(Scen, cmbLoc.List(cmbLoc.ListIndex), "ExDemand", lTs)
  If lTs.Count > 0 Then
    ATxComb.value = lTs(0).id
  Else
    ATxComb.value = 0
  End If
End Sub

Private Sub cmbLoc_KeyUp(KeyCode As Integer, Shift As Integer)
  'default combined depletion dsn if exists
  Call FindTimSer(Scen, cmbLoc.List(cmbLoc.ListIndex), "ExDemand", lTs)
  If lTs.Count > 0 Then
    ATxComb.value = lTs(0).id
  Else
    ATxComb.value = 0
  End If
End Sub
Private Sub CheckDsns(ok As Boolean)
  Dim oscen$, oloc$, ocons$, otu&, ots&, osdat&(6), oedat&(6), dtype&
  ok = True
  If ATxPump.value > 0 Then
    dtype = F90_WDCKDT(p.WDMFiles(1).fileUnit, ATxPump.value)
    If dtype <> 0 Then
      'already exists, dont allow
      ok = False
      MsgBox "Data set " & ATxPump.value & " already exists.  Choose a new data set number.", vbExclamation, "GenScn Stream Depletion Add Problem"
    End If
  ElseIf ATxPump.value = 0 Then
    ok = False
    MsgBox "Choose a Pump Rate data set number.", vbExclamation, "GenScn Stream Depletion Add Problem"
  End If
  If ATxDepl.value > 0 Then
    dtype = F90_WDCKDT(p.WDMFiles(1).fileUnit, ATxDepl.value)
    If dtype <> 0 Then
      'already exists, dont allow
      ok = False
      MsgBox "Data set " & ATxDepl.value & " already exists.  Choose a new data set number.", vbExclamation, "GenScn Stream Depletion Add Problem"
    End If
  ElseIf ATxPump.value = 0 Then
    ok = False
    MsgBox "Choose a Depletion data set number.", vbExclamation, "GenScn Stream Depletion Add Problem"
  End If
  If ATxComb.value > 0 Then
    dtype = F90_WDCKDT(p.WDMFiles(1).fileUnit, ATxComb.value)
    If dtype <> 0 Then
      'already exists, do more checking
      'Call F90_TSDSPC(ATxComb.value, oscen, oloc, ocons, otu, ots, osdat, oedat, ogsiz)
      Call GetInfoFromWDMTSer(ATxComb.value, oscen, oloc, ocons, otu, ots, osdat, oedat)
      If oscen <> Scen Or oloc <> cmbLoc.List(cmbLoc.ListIndex) Or ocons <> "ExDemand" Then
        'this is not a valid data set
        ok = False
        MsgBox "Data set " & ATxComb.value & " does not have the necessary attributes to be a combined depletion data set.", vbExclamation, "GenScn Stream Depletion Add Problem"
      End If
    End If
  End If
End Sub
Private Sub cmdAdd_Click()
  Dim ok As Boolean
  Dim ifl&, lstr$, fname$, r1!, r2!, r3!, i1&, SDate&(6), EDate&(6)
  Dim ndata&, rdat#(), utimser As ATCclsTserData, retcod&, i&, j&, dtype&
  
  Call CheckDsns(ok)
  If ok Then
    'read pumping rate file
    ifl = FreeFile(0)
    fname = lblFile.Caption
    On Error Resume Next
    Open fname For Input As #ifl
    Input #ifl, lstr
    Description = Left(lstr, 60)
    Input #ifl, lstr
    WellID = Left(lstr, 20)
    Line Input #ifl, lstr
    r1 = StrRetRem(lstr)
    r2 = StrRetRem(lstr)
    i1 = StrRetRem(lstr)
    r3 = StrRetRem(lstr)
    StreamDist = r1
    Diffusivity = r2
    Leakance = r3
    If i1 = 0 Then
      StreamSemi = False
    Else
      StreamSemi = True
    End If
    Line Input #ifl, lstr
    i1 = StrRetRem(lstr)
    r1 = StrRetRem(lstr)
    PriorPumpDays = i1
    PriorPumpRate = r1
    Line Input #ifl, lstr
    i1 = StrRetRem(lstr)
    NumDays = i1
    
    'now read pump data
    Line Input #ifl, lstr
    SDate(0) = CInt(Mid(lstr, 1, 4))
    SDate(1) = CInt(Mid(lstr, 5, 2))
    SDate(2) = CInt(Mid(lstr, 7, 2))
    SDate(3) = 0
    SDate(4) = 0
    SDate(5) = 0
    ndata = 1
    ReDim rdat(ndata)
    rdat(0) = CSng(Mid(lstr, 10, 10))
    While Not EOF(ifl)
      Line Input #ifl, lstr
      ndata = ndata + 1
      ReDim Preserve rdat(ndata)
      rdat(ndata - 1) = CSng(Mid(lstr, 10, 10))
    Wend
    EDate(0) = CInt(Mid(lstr, 1, 4))
    EDate(1) = CInt(Mid(lstr, 5, 2))
    EDate(2) = CInt(Mid(lstr, 7, 2))
    EDate(3) = 24
    EDate(4) = 0
    EDate(5) = 0
    Close #ifl
    NumDays = ndata
    
    'build data sets
    Reach = cmbLoc.List(cmbLoc.ListIndex)
    PumpRateDsn = ATxPump.value
    DepletionDsn = ATxDepl.value
    CombDepDsn = ATxComb.value
    'build PumpRate data set
    Call F90_WDLBAD(p.WDMFiles(1).fileUnit, PumpRateDsn, 1, i)
    Call AddAtts(PumpRateDsn, "PUMP", "PUMP")
    'build Depletion data set
    Call F90_WDLBAD(p.WDMFiles(1).fileUnit, DepletionDsn, 1, i)
    Call AddAtts(DepletionDsn, "DEPL", "DEPL")
    'build combined depletion data set
    If CombDepDsn > 0 Then
      dtype = F90_WDCKDT(p.WDMFiles(1).fileUnit, CombDepDsn)
      If dtype = 0 Then
        'does not exist
        Call F90_WDLBAD(p.WDMFiles(1).fileUnit, CombDepDsn, 1, i)
        Call AddAtts(CombDepDsn, "ExDemand", "WSPD")
      End If
    End If
    'now add data to the pump rate timser
    'Call WDMDir.TSDRRE(p.WDMFiles(1).fileUnit, 0&, 0&)
    Call FindTimSer(Scen, Reach, "PUMP", lTs)
    For j = 1 To lTs.Count
      If lTs(j).id = PumpRateDsn Then
        utimser = lTs(j)
      End If
    Next j
    '-----------FIXME----------Call UpdateWDMTs(SDate, EDate, rdat, utimser, retcod)
    ' now add to data structure and grid
    Call frmStrmDeplInterface.AddWell(WellID, Reach, PumpRateDsn, DepletionDsn, CombDepDsn, StreamDist, Diffusivity, StreamSemi, Leakance, PriorPumpDays, PriorPumpRate, NumDays, Description)
    Unload Me
    'refresh slc lists
    Call RefreshSLC
    'Call frmGenScn.RefreshMain
  End If
End Sub
Private Sub AddAtts(dsn&, Cons$, tstype$)

    Dim saind&, salen&, lstr$, retcod&

    saind = 288
    salen = 8
    lstr = Scen
    Call F90_WDBSAC(p.WDMFiles(1).fileUnit, dsn, p.HSPFMsg.Unit, saind, salen, retcod, lstr, Len(lstr))
    saind = 289
    salen = 8
    lstr = Cons
    Call F90_WDBSAC(p.WDMFiles(1).fileUnit, dsn, p.HSPFMsg.Unit, saind, salen, retcod, lstr, Len(lstr))
    saind = 290
    salen = 8
    lstr = Reach
    Call F90_WDBSAC(p.WDMFiles(1).fileUnit, dsn, p.HSPFMsg.Unit, saind, salen, retcod, lstr, Len(lstr))
    saind = 1
    salen = 4
    lstr = tstype
    Call F90_WDBSAC(p.WDMFiles(1).fileUnit, dsn, p.HSPFMsg.Unit, saind, salen, retcod, lstr, Len(lstr))
    saind = 2
    salen = 16
    lstr = WellID
    Call F90_WDBSAC(p.WDMFiles(1).fileUnit, dsn, p.HSPFMsg.Unit, saind, salen, retcod, lstr, Len(lstr))
    saind = 45
    salen = 48
    lstr = Description
    Call F90_WDBSAC(p.WDMFiles(1).fileUnit, dsn, p.HSPFMsg.Unit, saind, salen, retcod, lstr, Len(lstr))
End Sub


Private Sub cmdCancel_Click()
  Unload Me
End Sub

Private Sub cmdFile_Click()
  CDFile(0).flags = &H8806&
  CDFile(0).filter = "Stream Depletion Input Files (*.sdi)|*.sdi|All files (*.*)|*.*"
  CDFile(0).DialogTitle = "GenScn Stream Depletion Input File"
  On Error GoTo 10
  CDFile(0).CancelError = True
  CDFile(0).Action = 1
  lblFile.Caption = CDFile(0).Filename
10      'continue here on cancel
  If lblFile.Caption <> "<none>" And ATxPump.value > 0 And ATxDepl.value > 0 Then
    cmdAdd.Enabled = True
  Else
    cmdAdd.Enabled = False
  End If
End Sub
Private Sub GetLocs()

  Dim i&, j&
  
  Call FindTimSer("", "", "", lTs)
  nHSPLocs = 0
  For i = 0 To lTs.Count - 1
    'add locations to list
    j = 0
    Do While j < nHSPLocs
      If lTs(i).loc = HSPLocs(j) Then 'already in list
        j = nHSPLocs
      End If
      j = j + 1
    Loop
    If j = nHSPLocs Then 'not in list, add it
      If lTs(i).loc <> "<unk>" Then
        ReDim Preserve HSPLocs(nHSPLocs)
        HSPLocs(nHSPLocs) = lTs(i).loc
        nHSPLocs = nHSPLocs + 1
      End If
    End If
  Next i
End Sub

Private Sub Form_Load()
  Dim i&
  Call GetLocs
  For i = 0 To nHSPLocs - 1
    cmbLoc.AddItem HSPLocs(i)
  Next i
  cmbLoc.ListIndex = 0
End Sub
