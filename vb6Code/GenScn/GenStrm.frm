VERSION 5.00
Begin VB.Form frmStrmDeplInterface 
   Caption         =   "StrmDepl Interface"
   ClientHeight    =   5460
   ClientLeft      =   252
   ClientTop       =   1296
   ClientWidth     =   11556
   BeginProperty Font 
      Name            =   "MS Sans Serif"
      Size            =   7.8
      Charset         =   0
      Weight          =   700
      Underline       =   0   'False
      Italic          =   0   'False
      Strikethrough   =   0   'False
   EndProperty
   Icon            =   "GenStrm.frx":0000
   LinkTopic       =   "Form1"
   ScaleHeight     =   5460
   ScaleWidth      =   11556
   Begin VB.CommandButton cmdEdit 
      Caption         =   "&Edit Multipliers"
      Height          =   375
      Left            =   120
      TabIndex        =   1
      Top             =   4800
      Width           =   1455
   End
   Begin VB.CommandButton cmdLoc 
      Caption         =   "&Delete"
      Height          =   300
      Index           =   2
      Left            =   4440
      TabIndex        =   3
      Top             =   4560
      Width           =   852
   End
   Begin VB.CommandButton cmdLoc 
      Caption         =   "&Add"
      Height          =   300
      Index           =   1
      Left            =   3360
      TabIndex        =   2
      Top             =   4560
      Width           =   852
   End
   Begin VB.FileListBox sdilist 
      Height          =   648
      Left            =   0
      Pattern         =   "*.sdi"
      TabIndex        =   8
      Top             =   4680
      Visible         =   0   'False
      Width           =   1095
   End
   Begin VB.CommandButton cmdSave 
      Caption         =   "&Get Specs"
      Height          =   300
      Index           =   1
      Left            =   7440
      TabIndex        =   5
      Top             =   4920
      Visible         =   0   'False
      Width           =   1212
   End
   Begin VB.CommandButton cmdSave 
      Caption         =   "Sa&ve Specs"
      Height          =   300
      Index           =   0
      Left            =   7440
      TabIndex        =   4
      Top             =   4560
      Visible         =   0   'False
      Width           =   1212
   End
   Begin VB.CommandButton cmdStrm 
      Cancel          =   -1  'True
      Caption         =   "&Cancel"
      Height          =   300
      Index           =   1
      Left            =   4440
      TabIndex        =   7
      Top             =   5040
      Width           =   1212
   End
   Begin VB.CommandButton cmdStrm 
      Caption         =   "&Simulate"
      Height          =   300
      Index           =   0
      Left            =   3000
      TabIndex        =   6
      Top             =   5040
      Width           =   1212
   End
   Begin ATCoCtl.ATCoGrid agdLoc 
      Height          =   1575
      Left            =   120
      TabIndex        =   0
      Top             =   120
      Width           =   8535
      _ExtentX        =   15050
      _ExtentY        =   2773
      SelectionToggle =   0   'False
      AllowBigSelection=   -1  'True
      AllowEditHeader =   0   'False
      AllowLoad       =   0   'False
      AllowSorting    =   0   'False
      Rows            =   1
      Cols            =   11
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
      FixedCols       =   2
      ScrollBars      =   3
      SelectionMode   =   0
      BackColor       =   -2147483643
      ForeColor       =   -2147483640
      BackColorBkg    =   -2147483633
      BackColorSel    =   -2147483635
      ForeColorSel    =   16777215
      BackColorFixed  =   -2147483633
      ForeColorFixed  =   -2147483630
      InsideLimitsBackground=   -2147483643
      OutsideHardLimitBackground=   8421631
      OutsideSoftLimitBackground=   8454143
      ComboCheckValidValues=   -1  'True
   End
End
Attribute VB_Name = "frmStrmDeplInterface"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants
'Dim s As SwmmData
Dim SWPath As String, SWInFile As String
'Dim lc&, lts() As Timser
Dim lTs As Collection
Dim ssdate&(5), sedate&(5)
Dim nHSPCons&, HSPCons$()
Dim nHSPScen&, HSPScen$()
Dim ntempScen&, tempScen$()
Dim cdata$(), ndata&
Dim f$, Scen$
Dim initing As Boolean, movedfromrow As Long

Private Type WellData
    WellID As String
    Reach As String
    PumpRateDsn As Long
    DepletionDsn As Long
    CombDepDsn As Long
    StreamDist As Single
    Diffusivity As Single
    StreamSemi As Boolean
    Leakance As Single
    PriorPumpDays As Long
    PriorPumpRate As Single
    NumDays As Long
    Description As String
End Type
Dim WellRec() As WellData
Dim WellRecCnt&
Dim MonthMult!(12)
Dim rval!(12), i&

Private Sub agdLoc_CommitChange(ChangeFromRow As Long, ChangeToRow As Long, ChangeFromCol As Long, ChangeToCol As Long)
  Dim col&, row&
  With agdLoc
    'put value to data structure
    For col = ChangeFromCol To ChangeToCol
      For row = ChangeFromRow To ChangeToRow
        Select Case col
          Case 6: WellRec(row - 1).StreamDist = .TextMatrix(row, 6)
          Case 7: WellRec(row - 1).Diffusivity = .TextMatrix(row, 7)
          Case 8
            If .TextMatrix(row, col) = "True" Then
              WellRec(row - 1).StreamSemi = True
            Else
              WellRec(row - 1).StreamSemi = False
            End If
          Case 9: WellRec(row - 1).Leakance = .TextMatrix(row, 9)
          Case 10: WellRec(row - 1).PriorPumpDays = .TextMatrix(row, 10)
          Case 11: WellRec(row - 1).PriorPumpRate = .TextMatrix(row, 11)
          Case 12: WellRec(row - 1).Description = .TextMatrix(row, 12)
        End Select
      Next row
    Next col
  End With
End Sub

Private Sub agdLoc_RowColChange()
  Static changing As Boolean
  Dim i&, r&, c&
  If Not changing Then
    changing = True
    With agdLoc
      'put value to data structure
      .ClearValues
      If .col = 8 Then
        .addValue "True"
        .addValue "False"
        If .TextMatrix(.row, .col) = "True" Then
          WellRec(.row - 1).StreamSemi = True
        Else
          WellRec(.row - 1).StreamSemi = False
        End If
      ElseIf .col = 2 Then
        .addValue "Yes"
        .addValue "No"
      End If
      If initing = False Then
        'were not initializing, color row on this event
        If movedfromrow <> .row And movedfromrow <> -1 Then
          'we changed rows
          Call UnYellowRow(movedfromrow)
          Call YellowRow
          movedfromrow = .row
        End If
        If movedfromrow = -1 Then
          movedfromrow = 0
        End If
      End If
      changing = False
    End With
    
  End If
End Sub

Public Sub AddWell(tWellID$, tReach$, tPumpRateDsn&, tDepletionDsn&, tCombDepDsn&, tStreamDist As Single, tDiffusivity As Single, tStreamSemi As Boolean, tLeakance As Single, tPriorPumpDays&, tPriorPumpRate As Single, tNumDays&, tDescription$)
  Dim i&, j&
  ReDim Preserve WellRec(WellRecCnt + 1)
  WellRecCnt = WellRecCnt + 1
  i = WellRecCnt - 1
  WellRec(i).WellID = tWellID
  WellRec(i).Reach = tReach
  WellRec(i).PumpRateDsn = tPumpRateDsn
  WellRec(i).DepletionDsn = tDepletionDsn
  WellRec(i).CombDepDsn = tCombDepDsn
  WellRec(i).StreamDist = tStreamDist
  WellRec(i).Diffusivity = tDiffusivity
  WellRec(i).StreamSemi = tStreamSemi
  WellRec(i).Leakance = tLeakance
  WellRec(i).PriorPumpDays = tPriorPumpDays
  WellRec(i).PriorPumpRate = tPriorPumpRate
  WellRec(i).Description = tDescription
  WellRec(i).NumDays = tNumDays
  With agdLoc
    .Rows = .Rows + 1
    j = .Rows
    .TextMatrix(j, 0) = WellRec(i).WellID
    .TextMatrix(j, 1) = WellRec(i).Reach
    .TextMatrix(j, 1) = "No"
    .TextMatrix(j, 3) = WellRec(i).PumpRateDsn
    .TextMatrix(j, 4) = WellRec(i).DepletionDsn
    .TextMatrix(j, 5) = WellRec(i).CombDepDsn
    .TextMatrix(j, 6) = WellRec(i).StreamDist
    .TextMatrix(j, 7) = WellRec(i).Diffusivity
    .TextMatrix(j, 8) = WellRec(i).StreamSemi
    .TextMatrix(j, 9) = WellRec(i).Leakance
    .TextMatrix(j, 10) = WellRec(i).PriorPumpDays
    .TextMatrix(j, 11) = WellRec(i).PriorPumpRate
    .TextMatrix(j, 12) = WellRec(i).Description
  End With
End Sub

Private Sub cmdEdit_Click()
  frmStrmDeplMult.Show 1
End Sub

Private Sub cmdLoc_Click(Index As Integer)

  Dim i&, j&, irw&, tempwell$, temprch$, istr&

  With agdLoc
    If Index = 1 Then 'add a row for new well
      Call frmStrmDeplAdd.GetScen(Scen)
      frmStrmDeplAdd.Show 1
    ElseIf Index = 2 Then 'delete an existing well
      If .row > 0 Then
        tempwell = .TextMatrix(.row, 0)
        temprch = .TextMatrix(.row, 1)
        'delete from grid
        For i = .row To .Rows - 1
          For j = 0 To 11
            .TextMatrix(i, j) = .TextMatrix(i + 1, j)
          Next j
        Next i
        .Rows = .Rows - 1
        'find position in data structure
        istr = -1
        For i = 0 To WellRecCnt - 1
          If WellRec(i).WellID = tempwell And WellRec(i).Reach = temprch Then
            'found in data structure
            istr = i
          End If
        Next i
        If istr > -1 Then
          'delete from data structure
          For i = istr To WellRecCnt - 1
            WellRec(i).WellID = WellRec(i + 1).WellID
            WellRec(i).Reach = WellRec(i + 1).Reach
            WellRec(i).PumpRateDsn = WellRec(i + 1).PumpRateDsn
            WellRec(i).DepletionDsn = WellRec(i + 1).DepletionDsn
            WellRec(i).CombDepDsn = WellRec(i + 1).CombDepDsn
            WellRec(i).StreamDist = WellRec(i + 1).StreamDist
            WellRec(i).Diffusivity = WellRec(i + 1).Diffusivity
            WellRec(i).StreamSemi = WellRec(i + 1).StreamSemi
            WellRec(i).Leakance = WellRec(i + 1).Leakance
            WellRec(i).PriorPumpDays = WellRec(i + 1).PriorPumpDays
            WellRec(i).PriorPumpRate = WellRec(i + 1).PriorPumpRate
            WellRec(i).Description = WellRec(i + 1).Description
            WellRec(i).NumDays = WellRec(i + 1).NumDays
          Next i
          WellRecCnt = WellRecCnt - 1
        End If
      End If
    End If
  End With
  Call SizeForm
End Sub

Private Sub cmdSave_Click(Index As Integer)
  
  'On Error GoTo 10
  'cdlSwFile.CancelError = True
  'cdlSwFile.filter = "GenScn SWMM Specs Files (*.sws)|*.sws"
  'cdlSwFile.filename = "*.sws"
  'If Index = 0 Then 'save specs file
  '  cdlSwFile.Flags = &H8806&
  '  cdlSwFile.DialogTitle = "GenScn SWMM Save Specifications File"
  '  cdlSwFile.Action = 2
  '  Call SaveSpecs(cdlSwFile.filename)
  'ElseIf Index = 1 Then 'get specs file
  '  cdlSwFile.Flags = &H1806&
  '  cdlSwFile.DialogTitle = "GenScn SWMM Get Specifications File"
  '  cdlSwFile.Action = 1
  '  Call GetSpecs(cdlSwFile.filename)
  'End If
'10 'continue here on cancel

End Sub

Private Sub cmdSwFile_Click()

  'Dim f$, ierr%, i&, j&

  'cdlSwFile.DialogTitle = "Select SWMM Transport File"
  'cdlSwFile.filter = "SWMM files (*.doc)|*.doc|All files (*.*)|*.*"
  'cdlSwFile.filename = "*.doc"
  'ierr = 0
  'On Error GoTo errhandler
  'cdlSwFile.CancelError = True
  'cdlSwFile.Action = 1
  'ierr = 1
  'update swmm file and path names, parse info from input file
  'Call UpdateSWInFile(cdlSwFile.filename)

'errhandler:
 ' If ierr = 1 Then 'problem parsing file
  
  'End If
End Sub

'Private Sub cmdSaveProps_Click(Index As Integer)
'  Call SaveProps
'  sdilist.Refresh
'  agdLoc.TextMatrix(agdLoc.row, 0) = txtName.Text
'  agdLoc.Col = 1
'  agdLoc.Col = 0
'End Sub

Private Sub cmdStrm_Click(Index As Integer)

  Dim i&, str$, lstr$, written&, buf() As Byte, Msg$, rsp&, j&, utimser As ATCclsTserData
  Dim Init&, k&, l&, retcod&, rdat#()
  Dim tempts As Collection, temptsFilled As Collection
  Dim maxvals&
  Dim curDate&(6), simed&
  
  If Index = 0 Then 'perform StrmDepl simulation
    simed = 0
    Me.MousePointer = vbHourglass
    IPC.SendMonitorMessage "(OPEN GenScn StrmDepl Simulation)"
    IPC.SendMonitorMessage "(BUTTOFF OUTPUT)"
    IPC.SendMonitorMessage "(BUTTOFF PAUSE)"
    IPC.SendMonitorMessage "(BUTTOFF CANCEL)"
    For i = 1 To agdLoc.Rows
      'check to see if this row has valid values for simulation
      If WellRec(i - 1).PriorPumpDays > 0 Then
        'build sdi file for streamdepl input
        Call SaveProps(i)
        If agdLoc.TextMatrix(i, 2) = "Yes" Then
          Msg = "Running StrmDepl for Well " & WellRec(i - 1).WellID & " and Reach " & WellRec(i - 1).Reach
          IPC.SendMonitorMessage Msg
          lstr = SDExeName & " " & WellRec(i - 1).WellID & "." & WellRec(i - 1).Reach & ".sdi " & WellRec(i - 1).WellID & "." & WellRec(i - 1).Reach & ".sdo" & vbNullString
          IPC.StartProcess "StrmDepl", lstr, 10
          simed = simed + 1
        End If
        'ShowWin SDExeName, SW_MINIMIZE, 0
      ElseIf agdLoc.TextMatrix(i, 2) = "Yes" Then
        rsp = MsgBox("Simulation not run for Well " & WellRec(i - 1).WellID & " and Reach " & WellRec(i - 1).Reach & "." & vbCrLf & "'Prior Pump Days' must be greater than zero.", vbOKOnly, "StrmDepl Problem")
      End If
    Next i
    IPC.SendMonitorMessage "(CLOSE)"
    IPC.SendMonitorMessage "(BUTTON OUTPUT)"
    IPC.SendMonitorMessage "(BUTTON PAUSE)"
    IPC.SendMonitorMessage "(BUTTON CANCEL)"
    Me.MousePointer = vbDefault
    
    If simed > 0 Then
      rsp = vbYes
      rsp = MsgBox("Do you want to update the WDM data sets for these depletions?", _
                   4, "GenScn Stream Depletion Query")
      If rsp = vbYes Then
        'update the wdm data sets for depletions
        For i = 1 To WellRecCnt
          'check to see if this row has valid values for simulation
          If WellRec(i - 1).NumDays > 0 Then
            'read sdo file for streamdepl output
            lstr = WellRec(i - 1).WellID & "." & WellRec(i - 1).Reach & ".sdo"
            dsn = WellRec(i - 1).DepletionDsn
            'get this timser for updating
            Call FindTimSer(Scen, WellRec(i - 1).Reach, "DEPL", lTs)
            For j = 1 To lTs.Count
              If lTs(j).ID = dsn Then
                utimser = lTs(j)
              End If
            Next j
            Call UpdateDeplDsn(lstr, dsn, WellRec(i - 1).StreamSemi, utimser)
          End If
        Next i
      End If
    End If
    'now see if the user would like to update the combined dsns
    rsp = vbYes
    rsp = MsgBox("Do you want to update the WDM data sets for the combined depletions?", _
                 4, "GenScn Stream Depletion Query")
    If rsp = vbYes Then
      'update the wdm data sets for combined depletions
      Call FindTimSer(Scen, "", "ExDemand", lTs)
      For j = 1 To lTs.Count
        'loop through each combined demand time series
        Init = 1
        ReDim rdat(lTs(j).NVal)
        For i = 1 To WellRecCnt
          'loop through each well record
          If lTs(j).ID = WellRec(i - 1).CombDepDsn Then
            'will need to include this depl, find its timser
            Call FindTimSer(Scen, WellRec(i - 1).Reach, "DEPL", tempts)
            Set temptsFilled = FillTimSer(tempts)
            For l = 1 To temptsFilled.Count
              If temptsFilled(l).ID = WellRec(i - 1).DepletionDsn Then
                'found the timser for this depl, add these values to array
                Init = 0
                If lTs(j).NVal < temptsFilled(l).NVal Then
                  maxvals = lTs(j).NVal
                Else
                  maxvals = temptsFilled(l).NVal
                End If
                For k = 1 To maxvals
                  'sum the values
                  'Call F90_TIMADD(tempts(l - 1).sdat(0), tempts(l - 1).Tu, tempts(l - 1).ts, k - 1, curDate(0))
                  rdat(k - 1) = rdat(k - 1) + (temptsFilled(l).Vals(k - 1)) ' * MonthMult(curDate(1) - 1))
                Next k
              End If
            Next l
          End If
        Next i
        If Init = 0 Then
          'have something to update
          '---------------FIXME-----------------------Call UpdateWDMTs(lts(j).sdat, lts(j).edat, rdat, lts(j), retcod)
        End If
      Next j
    End If
  ElseIf Index = 1 Then 'cancel
    Unload Me
    ChDriveDir p.StatusFilePath
  End If

End Sub
Private Sub Form_Load()

  Dim i&, j&, sdpath$
  initing = True
  frmStrmDeplInterface.Caption = "GenScn Stream Depletion for Scenario " & Scen
  nHSPScen = 0
  sdpath = p.StatusFilePath & "strmdepl"
  On Error GoTo 10
  ChDriveDir sdpath
  GoTo 20
10  MkDirPath sdpath
    ChDriveDir sdpath
20  'continue
  With agdLoc
    .ColTitle(0) = "Well ID"
    .ColTitle(1) = "Reach"
    .ColTitle(2) = "Simulate?"
    .ColType(2) = ATCoTxt
    .ColTitle(3) = "Dsn: Pumping Rate"
    .ColTitle(4) = "Dsn: Depletion"
    .ColTitle(5) = "Dsn: Combined Depletion"
    .ColTitle(6) = "Distance to Stream(ft)"
    .ColType(6) = ATCoSng
    .colCharWidth(6) = 6
    .ColMin(6) = 0#
    .ColTitle(7) = "Diffusivity(ft^2/s)"
    .ColType(7) = ATCoSng
    .colCharWidth(7) = 6
    .ColMin(7) = 0#
    .ColTitle(8) = "Streambank Semipervious"
    .ColType(8) = ATCoTxt
    .ColTitle(9) = "Leakance(ft)"
    .ColType(9) = ATCoSng
    .ColMin(9) = 0#
    .colCharWidth(9) = 6
    .ColTitle(10) = "Prior Pumping Days"
    .ColType(10) = ATCoInt
    .ColMin(10) = 0
    .colCharWidth(10) = 6
    .ColTitle(11) = "Prior Pumping Rate(cfs)"
    .ColType(11) = ATCoSng
    .ColMin(11) = 0#
    .colCharWidth(11) = 6
    .ColTitle(12) = "Description"
    .ColType(12) = ATCoTxt
    For i = 0 To 1
      .ColEditable(i) = False
    Next i
    .ColEditable(2) = True
    For i = 3 To 5
      .ColEditable(i) = False
    Next i
    For i = 6 To 12
      .ColEditable(i) = True
    Next i
    .Rows = 0
    
  End With
  Me.Height = 6200
  Call FillWellData
  With agdLoc
    For j = 1 To WellRecCnt
      .Rows = .Rows + 1
      .TextMatrix(j, 0) = WellRec(j - 1).WellID
      .TextMatrix(j, 1) = WellRec(j - 1).Reach
      .TextMatrix(j, 2) = "No"
      .TextMatrix(j, 3) = WellRec(j - 1).PumpRateDsn
      .TextMatrix(j, 4) = WellRec(j - 1).DepletionDsn
      .TextMatrix(j, 5) = WellRec(j - 1).CombDepDsn
      .TextMatrix(j, 6) = WellRec(j - 1).StreamDist
      .TextMatrix(j, 7) = WellRec(j - 1).Diffusivity
      .TextMatrix(j, 8) = WellRec(j - 1).StreamSemi
      .TextMatrix(j, 9) = WellRec(j - 1).Leakance
      .TextMatrix(j, 10) = WellRec(j - 1).PriorPumpDays
      .TextMatrix(j, 11) = WellRec(j - 1).PriorPumpRate
      .TextMatrix(j, 12) = WellRec(j - 1).Description
    Next j
    .ColsSizeByContents
  End With
  For j = 1 To 12
    MonthMult(j - 1) = 1#
  Next j
  movedfromrow = -1
  initing = False
End Sub
Private Sub FillWellData()
  Dim s$, i&, j&
  
  Call FindTimSer(Scen, "", "PUMP", lTs)
  WellRecCnt = lTs.Count
  ReDim Preserve WellRec(WellRecCnt)
  For i = 1 To lTs.Count
    WellRec(i - 1).PumpRateDsn = lTs(i).ID
    WellRec(i - 1).Reach = lTs(i).loc
    dsn = lTs(i).ID
    Call F90_WDBSGC(p.WDMFiles(1).FileUnit, dsn, CLng(2), CLng(16), s)
    WellRec(i - 1).WellID = s
  Next i
  'now find depletion data sets
  For i = 1 To WellRecCnt
    Call FindTimSer(Scen, WellRec(i - 1).Reach, "DEPL", lTs)
    For j = 1 To lTs.Count
      dsn = lTs(j).ID
      Call F90_WDBSGC(p.WDMFiles(1).FileUnit, dsn, CLng(2), CLng(16), s)
      If s = WellRec(i - 1).WellID Then
        WellRec(i - 1).DepletionDsn = lTs(j).ID
      End If
    Next j
    'fill in props for this well
    WellRec(i - 1).CombDepDsn = 0
    WellRec(i - 1).StreamDist = 0#
    WellRec(i - 1).Diffusivity = 0#
    WellRec(i - 1).StreamSemi = False
    WellRec(i - 1).Leakance = 0#
    WellRec(i - 1).PriorPumpDays = 0
    WellRec(i - 1).PriorPumpRate = 0#
    WellRec(i - 1).NumDays = 0
    WellRec(i - 1).Description = ""
    Call GetProps(i - 1)
  Next i
  For i = 1 To WellRecCnt
    'find combined depletion data set number
    Call FindTimSer(Scen, WellRec(i - 1).Reach, "ExDemand", lTs)
    If lTs.Count > 0 Then
      WellRec(i - 1).CombDepDsn = lTs(1).ID
    End If
  Next i
End Sub

Private Sub GetSpecs(f$)

  'Dim i&, ifl&, lstr$, lstr1$, lstr2$, Strt&(5), Stp&(5)

  'ifl = FreeFile(0)
  'Open f For Input As #ifl
  'Input #ifl, lstr
  'update swmm input file and path names, parse info from input file
  'Call UpdateSWInFile(lstr)
  'Input #ifl, lstr
  'txtSWOut.Text = lstr
  'Input #ifl, lstr
  'cboHSPSen.Text = lstr
  'Input #ifl, Strt(0), Strt(1), Strt(2), Strt(3), Strt(4), Strt(5)
  'Input #ifl, Stp(0), Stp(1), Stp(2), Stp(3), Stp(4), Stp(5)
  'adtSWMM.CurrS = DateSerial(Strt(0), Strt(1), Strt(2)) + TimeSerial(Strt(3), Strt(4), Strt(5))
  'adtSWMM.CurrE = DateSerial(Stp(0), Stp(1), Stp(2)) + TimeSerial(Stp(3), Stp(4), Stp(5))
  'With agdCon
  '  Input #ifl, i
  '  .Rows = i
  '  For i = 1 To .Rows
  '    Input #ifl, lstr, lstr1, lstr2
  '    .TextMatrix(i, 0) = lstr
  '    .TextMatrix(i, 1) = lstr1
  '    .TextMatrix(i, 2) = lstr2
  '  Next i
  'End With
  'With agdLoc
  '  Input #ifl, i
  '  .Rows = i
  '  For i = 1 To .Rows
  '    Input #ifl, lstr, lstr1, lstr2
  '    .TextMatrix(i, 0) = lstr
  '    .TextMatrix(i, 1) = lstr1
  '    .TextMatrix(i, 2) = lstr2
  '  Next i
  'End With
  'Call fraLocResize
  'Close #ifl

End Sub
Private Sub GetProps(i&)

  Dim ifl&, lstr$, fname$, r1!, r2!, r3!, i1&

  ifl = FreeFile(0)
  fname = WellRec(i).WellID & "." & WellRec(i).Reach & ".sdi"
  On Error Resume Next
  Open fname For Input As #ifl
  Input #ifl, lstr
  WellRec(i).Description = Left(lstr, 70)
  Input #ifl, lstr
  Line Input #ifl, lstr
  r1 = StrRetRem(lstr)
  r2 = StrRetRem(lstr)
  i1 = StrRetRem(lstr)
  r3 = StrRetRem(lstr)
  WellRec(i).StreamDist = r1
  WellRec(i).Diffusivity = r2
  WellRec(i).Leakance = r3
  If i1 = 0 Then
    WellRec(i).StreamSemi = False
  Else
    WellRec(i).StreamSemi = True
  End If
  Line Input #ifl, lstr
  i1 = StrRetRem(lstr)
  r1 = StrRetRem(lstr)
  WellRec(i).PriorPumpDays = i1
  WellRec(i).PriorPumpRate = r1
  Line Input #ifl, lstr
  i1 = StrRetRem(lstr)
  WellRec(i).NumDays = 0
  'ndata = 0
  'ReDim cdata(0)
  'While Not EOF(ifl)
  '  Line Input #ifl, lstr
  '  ndata = ndata + 1
  '  ReDim Preserve cdata(ndata)
  '  cdata(ndata - 1) = lstr
  'Wend
  Close #ifl
End Sub
Private Sub UpdateDeplDsn(fname$, dsn&, leak As Boolean, utimser As ATCclsTserData)

  Dim ifl&, lstr$, r!, r2!, r3!, i1&, icnt&, i&
  Dim SDate&(6), EDate&(6), retcod&, rdat#()
  
  ifl = FreeFile(0)
  On Error Resume Next
  Open fname For Input As #ifl
  'read header lines
  If leak Then
    icnt = 48
  Else
    icnt = 47
  End If
  For i = 1 To icnt
    Input #ifl, lstr
  Next i
  'now read output data
  Line Input #ifl, lstr
  SDate(0) = CInt(Mid(lstr, 4, 4))
  SDate(1) = CInt(Mid(lstr, 8, 2))
  SDate(2) = CInt(Mid(lstr, 10, 2))
  SDate(3) = 0
  SDate(4) = 0
  SDate(5) = 0
  ndata = 1
  ReDim rdat(ndata)
  rdat(0) = CSng(Mid(lstr, 37, 10))
  While Not EOF(ifl)
    Line Input #ifl, lstr
    ndata = ndata + 1
    ReDim Preserve rdat(ndata)
    rdat(ndata - 1) = CSng(Mid(lstr, 37, 10))
  Wend
  EDate(0) = CInt(Mid(lstr, 4, 4))
  EDate(1) = CInt(Mid(lstr, 8, 2))
  EDate(2) = CInt(Mid(lstr, 10, 2))
  EDate(3) = 24
  EDate(4) = 0
  EDate(5) = 0
  Close #ifl
  'now put to wdm
  '-------------FIXME--------------Call UpdateWDMTs(SDate, EDate, rdat, utimser, retcod)
End Sub
Private Sub SaveProps(irow&)

  Dim i&, ofl&, lstr$, f$, i1&, ifound&, j&, s$
  Dim sday$, smon$, ndat&(6), tdat&(6), rtmp!

  ofl = FreeFile(0)
  f = WellRec(irow - 1).WellID & "." & WellRec(irow - 1).Reach & ".sdi"
  Open f For Output As #ofl
  If Len(WellRec(irow - 1).Description) = 0 Then
    WellRec(irow - 1).Description = "Stream Depletion Input File Created by GenScn from DSN " & WellRec(irow - 1).PumpRateDsn & " on " & Date
  End If
  Print #ofl, WellRec(irow - 1).Description
  Print #ofl, WellRec(irow - 1).WellID
  If WellRec(irow - 1).StreamSemi = "False" Then
    i1 = 0
  Else
    i1 = 1
  End If
  lstr = WellRec(irow - 1).StreamDist & " " & WellRec(irow - 1).Diffusivity & " " & i1 & " " & WellRec(irow - 1).Leakance
  Print #ofl, lstr
  lstr = WellRec(irow - 1).PriorPumpDays & " " & WellRec(irow - 1).PriorPumpRate
  Print #ofl, lstr
  
  Call FindTimSer(Scen, WellRec(irow - 1).Reach, "PUMP", lTs)
  'Call FillTimSer(lc, lts())
  
  ifound = 0
  For j = 1 To lTs.Count
    dsn = lTs(j).ID
    Call F90_WDBSGC(p.WDMFiles(1).FileUnit, dsn, CLng(2), CLng(16), s)
    If s = WellRec(irow - 1).WellID Then
      ifound = j
    End If
  Next j
  'write actual number of days in timser
  Print #ofl, lTs(ifound).NVal
  WellRec(irow - 1).NumDays = lTs(ifound).NVal  'bug fix 3/6/00
  CSDat(0) = lTs(ifound).sdat(0)
  CSDat(1) = lTs(ifound).sdat(1)
  CSDat(2) = lTs(ifound).sdat(2)
  CSDat(3) = lTs(ifound).sdat(3)
  CSDat(4) = lTs(ifound).sdat(4)
  CSDat(5) = lTs(ifound).sdat(5)
  CEDat(0) = lTs(ifound).edat(0)
  CEDat(1) = lTs(ifound).edat(1)
  CEDat(2) = lTs(ifound).edat(2)
  CEDat(3) = lTs(ifound).edat(3)
  CEDat(4) = lTs(ifound).edat(4)
  CEDat(5) = lTs(ifound).edat(5)
  Call FillTimSer(lTs)
  
  'develop first day's data
  smon = Trim(str(lTs(ifound).sdat(1)))
  sday = Trim(str(lTs(ifound).sdat(2)))
  If Len(smon) = 1 Then
    smon = "0" & smon
  End If
  If Len(sday) = 1 Then
    sday = "0" & sday
  End If
  rtmp = lTs(ifound).Vals(0) * MonthMult(lTs(ifound).sdat(1) - 1)
  lstr = lTs(ifound).sdat(0) & smon & sday & "  " & rtmp
  Print #ofl, lstr
  tdat(0) = lTs(ifound).sdat(0)
  tdat(1) = lTs(ifound).sdat(1)
  tdat(2) = lTs(ifound).sdat(2)
  tdat(3) = lTs(ifound).sdat(3)
  tdat(4) = lTs(ifound).sdat(4)
  tdat(5) = lTs(ifound).sdat(5)
  For i = 1 To lTs(ifound).NVal - 1
    'now do subsequent day's data
    Call F90_TIMADD(tdat(0), 4, 1, 1, ndat(0))
    smon = Trim(str(ndat(1)))
    sday = Trim(str(ndat(2)))
    If Len(smon) = 1 Then
      smon = "0" & smon
    End If
    If Len(sday) = 1 Then
      sday = "0" & sday
    End If
    rtmp = lTs(ifound).Vals(i) * MonthMult(ndat(1) - 1)
    lstr = ndat(0) & smon & sday & "  " & rtmp
    tdat(0) = ndat(0)
    tdat(1) = ndat(1)
    tdat(2) = ndat(2)
    tdat(3) = ndat(3)
    tdat(4) = ndat(4)
    tdat(5) = ndat(5)
    Print #ofl, lstr
  Next i
  Close #ofl
End Sub
Private Sub SaveSpecs(f$)
  
  'Dim i&, ofl&

  'ofl = FreeFile(0)
  'Open f For Output As #ofl
  'Write #ofl, lblSwFile.Caption
  'Write #ofl, txtSWOut.Text
  'Write #ofl, cboHSPSen.Text
  'Write #ofl, Year(adtSWMM.CurrS), Month(adtSWMM.CurrS), Day(adtSWMM.CurrS), Hour(adtSWMM.CurrS), Minute(adtSWMM.CurrS), Second(adtSWMM.CurrS)
  'Write #ofl, Year(adtSWMM.CurrE), Month(adtSWMM.CurrE), Day(adtSWMM.CurrE), Hour(adtSWMM.CurrE), Minute(adtSWMM.CurrE), Second(adtSWMM.CurrE)
  'With agdCon
  '  Write #ofl, .Rows
  '  For i = 1 To .Rows
  '    Write #ofl, .TextMatrix(i, 0), .TextMatrix(i, 1), .TextMatrix(i, 2)
  '  Next i
  'End With
  'With agdLoc
  '  Write #ofl, .Rows
  '  For i = 1 To .Rows
  '    Write #ofl, .TextMatrix(i, 0), .TextMatrix(i, 1), .TextMatrix(i, 2)
  '  Next i
  'End With
  'Close #ofl

End Sub

Private Sub UpdateSWInFile(fname$)
  
  'SWPath = PathNameOnly(fname)
  'If Len(SWPath) > 0 Then 'extract just file name from full path
  '  SWInFile = Mid(fname, Len(SWPath) + 2)
  'Else 'no path name
  '  SWInFile = fname
  'End If
  'Call SwTransInRead(fname, s)
  'If Len(s.fname) = 0 Then 'problem, no interface file on input file
  '  MsgBox "The specified Transport input file does not contain a SWMM interface file." & vbCrLf & _
  '         "HSPF runoff data is transferred to SWMM via the interface file." & vbCrLf & _
  '         "Please modify the Transport input file to reference an interface file (even if it does not exist).", vbExclamation, "GenScn SWMM Problem"
  'End If
  'txtSWOut.Text = SWPath & "\" & FileNameOnly(fname) & ".out"
  'lblSwFile.Caption = fname
  'Call RefreshSWMMInfo
  'agdLoc.Col = 1 'forces RowColChange event to get locations put in list

End Sub

Private Sub SizeForm()
  If frmStrmDeplInterface.WindowState = vbNormal Then
    'size vertical
    agdLoc.Height = Me.Height - 1500
    cmdLoc(1).Top = Me.Height - 1300
    cmdLoc(2).Top = Me.Height - 1300
    cmdStrm(0).Top = Me.Height - 850
    cmdStrm(1).Top = Me.Height - 850
    cmdEdit.Top = Me.Height - 950
    'size horizontal
    agdLoc.Width = Me.Width - 400
    cmdLoc(1).Left = (Me.Width / 2) - 100 - cmdLoc(1).Width
    cmdLoc(2).Left = (Me.Width / 2) + 100
    cmdStrm(0).Left = (Me.Width / 2) - 100 - cmdStrm(0).Width
    cmdStrm(1).Left = (Me.Width / 2) + 100
  End If
  agdLoc.ColsSizeToWidth
  agdLoc.colWidth(0) = 1100
  agdLoc.colWidth(1) = 700
  agdLoc.colWidth(2) = 800
End Sub
Public Sub LoadStrmDeplData(n As String)
  Scen = n
  f = Scen & ".sdd"
  On Error GoTo nofile
  Open f For Input As #1
  Close #1
  frmStrmDeplInterface.Show
  Exit Sub
nofile:
  If err.Number <> 55 Then
    'MsgBox "Stream Depletion Data file " & f & " not found", _
        vbExclamation, "GenScn Stream Depletion"
    f = ""
    frmStrmDeplInterface.Show
    Exit Sub
  Else
    Resume Next
  End If
End Sub
Private Sub Form_Resize()
  Call SizeForm
End Sub
Private Sub YellowRow()
  Dim i&, savecol&
  With agdLoc
    savecol = .col
    For i = 0 To .cols
      .col = i
      .CellBackColor = vbYellow
    Next i
    .col = savecol
  End With
  '.Selected(.row, .Col) = True
End Sub
Private Sub UnYellowRow(irow&)
  Dim i&, savecol&, saverow&, j&
  With agdLoc
    savecol = .col
    saverow = .row
    .row = irow
    .col = 0
    If .CellBackColor = vbYellow Then
      'this row is yellow, change it back
      For i = 0 To 1
        .col = i
        .CellBackColor = vbButtonFace
      Next i
      For i = 2 To .cols
        .col = i
        .CellBackColor = vbWindowBackground
      Next i
    End If
    .col = savecol
    .row = saverow
  End With
End Sub
Public Sub GetMult(rval)
  For i = 0 To 11
    rval(i) = MonthMult(i)
  Next i
End Sub
Public Sub PutMult(rval)
  For i = 0 To 11
    MonthMult(i) = rval(i)
  Next i
End Sub
