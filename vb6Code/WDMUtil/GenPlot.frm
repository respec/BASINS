VERSION 5.00
Begin VB.Form frmGenScnPlot 
   Caption         =   "Graph"
   ClientHeight    =   4515
   ClientLeft      =   2385
   ClientTop       =   2070
   ClientWidth     =   5130
   HelpContextID   =   601
   Icon            =   "GenPlot.frx":0000
   LinkTopic       =   "Form1"
   PaletteMode     =   1  'UseZOrder
   ScaleHeight     =   4515
   ScaleWidth      =   5130
   Begin VB.CheckBox chkPlot 
      Caption         =   "&Bar Chart Comparison of Multiple Time Series"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   252
      HelpContextID   =   66
      Index           =   3
      Left            =   240
      TabIndex        =   3
      Top             =   1320
      Width           =   4692
   End
   Begin VB.CheckBox chkXYLine 
      Caption         =   "45 deg/regress lines"
      Enabled         =   0   'False
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   252
      HelpContextID   =   70
      Left            =   2880
      TabIndex        =   9
      Top             =   3120
      Width           =   2292
   End
   Begin VB.OptionButton optStorm 
      Caption         =   "Current Period"
      Enabled         =   0   'False
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   252
      HelpContextID   =   92
      Index           =   1
      Left            =   240
      TabIndex        =   10
      Top             =   3360
      Value           =   -1  'True
      Visible         =   0   'False
      Width           =   1572
   End
   Begin VB.OptionButton optStorm 
      Caption         =   "Storms"
      Enabled         =   0   'False
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   252
      HelpContextID   =   92
      Index           =   0
      Left            =   1920
      TabIndex        =   11
      Top             =   3360
      Visible         =   0   'False
      Width           =   972
   End
   Begin VB.CommandButton cmdSelect 
      Caption         =   "&None"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   252
      Index           =   0
      Left            =   1320
      TabIndex        =   1
      Top             =   360
      Width           =   852
   End
   Begin VB.CommandButton cmdSelect 
      Caption         =   "&All"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   252
      Index           =   1
      Left            =   240
      TabIndex        =   0
      Top             =   360
      Width           =   852
   End
   Begin VB.CommandButton cmdCancel 
      Cancel          =   -1  'True
      Caption         =   "Cancel"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   495
      Left            =   1920
      TabIndex        =   13
      Top             =   3840
      Width           =   972
   End
   Begin VB.CommandButton cmdGen 
      Caption         =   "&Generate"
      Default         =   -1  'True
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   495
      Left            =   240
      TabIndex        =   12
      Top             =   3840
      Width           =   1095
   End
   Begin VB.CheckBox chkPlot 
      Caption         =   "Sca&tter (TS #2  vs. TS #1)"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   252
      HelpContextID   =   70
      Index           =   6
      Left            =   240
      TabIndex        =   8
      Top             =   3120
      Width           =   2772
   End
   Begin VB.CheckBox chkPlot 
      Caption         =   "&Difference (TS #2 - TS #1) vs. TS #1"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   252
      HelpContextID   =   69
      Index           =   5
      Left            =   240
      TabIndex        =   7
      Top             =   2760
      Width           =   3972
   End
   Begin VB.CheckBox chkPlot 
      Caption         =   "&Flow/Duration"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   252
      HelpContextID   =   90
      Index           =   4
      Left            =   3120
      TabIndex        =   6
      Top             =   2400
      Visible         =   0   'False
      Width           =   1692
   End
   Begin VB.CheckBox chkPlot 
      Caption         =   "&Cumulative Difference vs. time"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   252
      HelpContextID   =   68
      Index           =   2
      Left            =   240
      TabIndex        =   5
      Top             =   2400
      Width           =   3612
   End
   Begin VB.CheckBox chkPlot 
      Caption         =   "&Residual (TS #2 - TS #1 vs. time)"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   252
      HelpContextID   =   67
      Index           =   1
      Left            =   240
      TabIndex        =   4
      Top             =   2040
      Width           =   3732
   End
   Begin VB.CheckBox chkPlot 
      Caption         =   "&Standard"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   252
      HelpContextID   =   65
      Index           =   0
      Left            =   240
      TabIndex        =   2
      Top             =   960
      Width           =   1692
   End
   Begin VB.Label lblGraph 
      Caption         =   "Comparison Plots (For 2 Time Series [TS] Only)"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   252
      Index           =   1
      Left            =   240
      TabIndex        =   16
      Top             =   1800
      Width           =   4692
   End
   Begin VB.Label lblGraph 
      Caption         =   "Single/Multiple Time Series Plots"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   252
      Index           =   0
      Left            =   240
      TabIndex        =   15
      Top             =   720
      Width           =   4332
   End
   Begin VB.Label lblSelect 
      Caption         =   "Select desired plots, then press Generate."
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   252
      Left            =   240
      TabIndex        =   14
      Top             =   120
      Width           =   4332
   End
End
Attribute VB_Name = "frmGenScnPlot"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants
Private Sub chkPlot_Click(Index As Integer)

    Dim i%, pflg%

    pflg = 0
    i = 0
    While i <= 6
      If chkPlot(i).Value = 1 Then
        pflg = 1
        i = 7
      Else
        i = i + 1
      End If
    Wend
    If pflg = 1 Then
      cmdGen.Enabled = True
    Else
      cmdGen.Enabled = False
    End If
    If Index = 6 Then
      If chkPlot(6).Value = 1 Then
        chkXYLine.Enabled = True
'        optStorm(0).Enabled = True
'        optStorm(1).Enabled = True
      Else
        chkXYLine.Enabled = False
'        optStorm(0).Enabled = False
'        optStorm(1).Enabled = False
      End If
    End If

End Sub


'Dim g As New HGrph
Private Sub cmdCancel_Click()

    Unload frmGenScnPlot

End Sub

Private Sub cmdGen_Click()

  Dim i&, j&, ipos&, lnts&, ilen&
  Dim mxpts&, ConstInt As Boolean
  Dim CScenm$(), clocnm$(), cconnm$()
  Dim ltu&(), which&(), typind&(), Tran&
  Dim Lcntcon&, Lcntsen&, Lcntloc&
  Dim Calab$, Cyrlab$, Cyllab$, Ctitl$, capt$
  Dim ctran$, ChrTUnit$, clab$(), ltitl$, VLab$()
  Dim rtmp!(), ACoef!, BCoef!, RSquar!
  Dim lTser As Collection  '
  Dim PLTser As Collection '
  Dim XYD() As xyplotdata
  Dim g As Object
  Dim n&, nplt&, nvalsOrig&, NVALS&
  Dim ltmp!, lValues!(), lFlags&(), lJday#()
  Dim lTs As ATCclsTserData, lds As ATCclsTserDate
  Dim STSer As Collection
  Set STSer = frmGenScn.TSerSelected

  MousePointer = ccHourglass
  If STSer.Count > 18 Then 'not doing more than 18 plots
    lnts = 18
  Else
    lnts = STSer.Count
  End If
  'get the data values for selected data sets
  ConstInt = True
  Set lTser = FillTimSerExt(STSer, ConstInt)

  'labels
  ReDim CScenm(lnts - 1)
  ReDim clocnm(lnts - 1)
  ReDim cconnm(lnts - 1)
  ReDim which(lnts)
  ReDim typind(lnts)
  ReDim clab(lnts)
  ReDim VLab(lnts)
  For i = 1 To lnts
    CScenm(i - 1) = lTser(i).Header.Sen
    clocnm(i - 1) = lTser(i).Header.Loc
    cconnm(i - 1) = lTser(i).Header.Con
  Next i
  If CDTran = 4 Then Tran = 1 Else Tran = CDTran
'  Call sglabl(lnts, CScenm(), clocnm(), cconnm(), ctunit, CDTran, which(), typind(), Lcntcon, Lcntsen, Lcntloc, Calab, Cyrlab, Cyllab, Ctitl, clab(), ctran, ChrTUnit)
  Call DefaultLabels(CDTran, ConstInt, _
                 lnts, CScenm(), clocnm(), cconnm(), ctunit, Tran, _
                 which(), typind(), _
                 Lcntcon, Lcntsen, Lcntloc, _
                 Calab, Cyrlab, Cyllab, Ctitl, clab(), ctran, ChrTUnit)

  If Not ConstInt Then
    'adjust variable labels
    ReDim Preserve clab(2 * lnts)
    ReDim VLab(2 * lnts)
    For i = lnts - 1 To 1 Step -1
      VLab(2 * i) = clab(i)
      VLab(2 * i + 1) = "Time"
    Next i
    VLab(1) = "Time"
'    'adjust title (remove time units)
'    ilen = 10
'    ipos = InStr(Ctitl, "SECONDLY")
'    If ipos = 0 Then
'      ipos = InStr(Ctitl, "HOURLY")
'      ilen = 8
'    End If
'    If ipos = 0 Then
'      ipos = InStr(Ctitl, "DAILY")
'      ilen = 7
'    End If
'    If ipos = 0 Then
'      ipos = InStr(Ctitl, "MONTHLY")
'      ilen = 9
'    End If
'    If ipos > 0 Then
'      If ipos > 1 Then 'imbedded
'        Ctitl = Left(Ctitl, ipos - 1) & Mid(Ctitl, ipos + ilen - 1)
'      Else 'at the beginning
'        Ctitl = Mid(Ctitl, ilen)
'      End If
'    End If
'    'remove "MEAN" also
'    ipos = InStr(Ctitl, "MEAN")
'    If ipos > 0 Then
'      If ipos > 1 Then 'imbedded
'        Ctitl = Left(Ctitl, ipos - 1) & Mid(Ctitl, ipos + 5)
'      Else 'at the beginning
'        Ctitl = Mid(Ctitl, 6)
'      End If
'    End If
  End If

  If chkPlot(0).Value = 1 Then 'standard timeseries plot
    capt = "WDMUtil Standard Plot"
    If ConstInt Then 'constant interval
      Call GLInit(1, g, lnts, lnts)
    Else
      Call GLInit(1, g, lnts, 2 * lnts)
'        Call GLVarLab(vlab)
    End If
    Call GLVarLab(clab)
    Call GLDate(CSDat(), CEDat(), ConstInt)
    Call GLTitl(Ctitl, capt)
    Call GLLegend(clab())
    Call GLAxLab("", Cyllab, Cyrlab, Calab)
    Call GLIcon(frmGenScn.cmdAna(1).Picture)
    Call GLDoTS(g, 1, lTser)
  End If
  If chkPlot(1).Value = 1 Then 'residual plot
    If lTser(1).dates.Summary.NVALS = lTser(2).dates.Summary.NVALS Then
      'same number of values, ok to do plot
      Set PLTser = Nothing
      Set PLTser = New Collection
      Set lTs = lTser(1).DoMath(ATCSubtract, lTser(2), 0)
      PLTser.Add lTs
      'make same pos and neg scale on Y-axis
      With PLTser(1)
        If .Max > Abs(.Min) And .Max > 0 Then 'adjust min
          .Min = -1 * .Max
        ElseIf .Max < Abs(.Min) And .Max > 0 Then 'adjust max
          .Max = -1 * .Min
        ElseIf .Max < 0 Then ' adjust max
          .Max = -1 * .Min
        End If
      End With
      VLab(0) = Trim(clab(1)) & "-" & Trim(clab(0))
      If ConstInt Then
        ltitl = "Residual Time Series Plot (" & VLab(0) _
          & ")&for " & Trim(ctran) & " " & Trim(ChrTUnit)
      Else
        ltitl = "Residual Time Series Plot (" & VLab(0)
        If Lcntsen = 1 Or Lcntcon = 1 Or Lcntloc = 1 Then
          ltitl = ltitl & ")&for "
        End If
      End If
      If Lcntsen = 1 Then 'add scenario to title
        ltitl = ltitl & " " & CScenm(0)
      End If
      If Lcntcon = 1 Then 'add constituent to title
        ltitl = ltitl & " " & cconnm(0)
      End If
      If Lcntloc = 1 Then 'add location to title
        ltitl = ltitl & " at " & clocnm(0)
      End If
      If ConstInt Then
        Call GLInit(1, g, 1, 1)
      Else
        Call GLInit(1, g, 1, 2)
        VLab(1) = "Time"
      End If
      capt = "WDMUtil Residual Plot"
      Call GLTitl(ltitl, capt)
      Call GLLegend(clab())
      Call GLDate(CSDat(), CEDat(), ConstInt)
      Call GLAxLab("", Cyllab, Cyrlab, Calab)
      Call GLVarLab(VLab())
      Call GLZLine(1, 0)
      Call GLDoTS(g, 1, PLTser)
    Else 'can't do plot, incompatible timeseries
      MsgBox "Unable to perform Residual plot as the two timeseries being used do not contain the same number of values.", 48, "WDMUtil Plot Problem"
    End If
  End If
  If chkPlot(2).Value = 1 Then 'cummulative difference plot
    If lTser(1).dates.Summary.NVALS = lTser(2).dates.Summary.NVALS Then
      'same number of values, ok to do plot
      Set PLTser = Nothing
      Set PLTser = New Collection
      Set lTs = lTser(1).DoMath(ATCCummDiff, lTser(2), 0)
      PLTser.Add lTs
      'make same pos and neg scale on Y-axis
      With PLTser(1)
        If .Max > Abs(.Min) And .Max > 0 Then 'adjust min
          .Min = -1 * .Max
        ElseIf .Max < Abs(.Min) And .Max > 0 Then 'adjust max
          .Max = -1 * .Min
        ElseIf .Max < 0 Then ' adjust max
          .Max = -1 * .Min
        End If
      End With
      If ConstInt Then
        Call GLInit(1, g, 1, 1)
      Else
        Call GLInit(1, g, 1, 2)
        VLab(1) = "Time"
      End If
      VLab(0) = Trim(clab(1)) & "-" & Trim(clab(0))
      If ConstInt Then
        ltitl = "Cumulative Differences Time Series Plot (" _
          & VLab(0) & ")&for " & Trim(ctran) _
          & " " & Trim(ChrTUnit)
      Else
        ltitl = "Cumulative Differences Time Series Plot (" & VLab(0)
        If Lcntsen = 1 Or Lcntcon = 1 Or Lcntloc = 1 Then
          ltitl = ltitl & ")&for "
        End If
      End If
      If Lcntsen = 1 Then 'add scenario to title
        ltitl = ltitl & " " & CScenm(0)
      End If
      If Lcntcon = 1 Then 'add constituent to title
        ltitl = ltitl & " " & cconnm(0)
      End If
      If Lcntloc = 1 Then 'add location to title
        ltitl = ltitl & " at " & clocnm(0)
      End If
      capt = "WDMUtil Cumulative Differences Plot"
      Call GLTitl(ltitl, capt)
      Call GLLegend(clab())
      Call GLDate(CSDat(), CEDat(), ConstInt)
      Call GLAxLab("", Cyllab, Cyrlab, Calab)
      Call GLVarLab(VLab())
      Call GLZLine(1, 0)
      Call GLDoTS(g, 1, PLTser)
    Else 'can't do plot, incompatible timeseries
      MsgBox "Unable to perform Cumulative Difference plot as the two timeseries being used do not contain the same number of values.", 48, "WDMUtil Plot Problem"
    End If
  End If
  If chkPlot(3).Value = 1 Then 'timeseries bar plot
    If ConstInt Then
        nvalsOrig = lTser(1).dates.Summary.NVALS
        NVALS = nvalsOrig * (lnts + 1)
        'build dummy date array
        Set lds = New ATCclsTserDate
        ReDim lJday(NVALS) ' init to 0
        ReDim lFlags(NVALS)
        lds.flags = lFlags
        For i = 1 To NVALS
          lJday(i) = lTser(1).dates.Value(1 + (i / (lnts + 1)))
        Next i
        lds.Values = lJday
        lds.calcSummary
        Set PLTser = Nothing
        Set PLTser = New Collection
        For i = 1 To lnts
          Set lTs = New ATCclsTserData
          Set lTs.dates = lds
          lTs.flags = lFlags
          ReDim lValues(NVALS)
          'lts.dates.summary.nvals = nvals * (lnts + 1)
          For j = 1 To nvalsOrig
            ipos = i + ((j - 1) * (lnts + 1))
            ltmp = lTser(i).Value(j)
            lValues(ipos) = ltmp
          Next j
          lTs.Values = lValues
          lTs.calcSummary
          PLTser.Add lTs
        Next i
'      'dummy values for spacing timeseries
'      PLTser(lnts).NVal = PLTser(0).NVal * (lnts + 1)
'      ReDim PLTser(lnts).Vals(PLTser(lnts).NVal * (lnts + 1))
'      PLTser(lnts).Tu = STSer(0).Tu
'      PLTser(lnts).ts = STSer(0).ts
'      PLTser(lnts).Min = 0
'      PLTser(lnts).Max = 0
      Call GLInit(1, g, lnts, lnts)
      Call GLDate(CSDat(), CEDat(), ConstInt)
      capt = "WDMUtil Bar Chart"
      Call GLTitl(Ctitl, capt)
      clab(lnts) = ""
      Call GLLegend(clab())
      Call GLAxLab("", Cyllab, Cyrlab, Calab)
      Call GLVarLab(clab)
      Call GLDoTS(g, 1, PLTser)
    Else
      MsgBox "Unable to perform Bar Chart for non-constant interval timeseries.", 48, "WDMUtil Plot Problem"
    End If
  End If
'   no duration plots in WDMUtil
'    If chkPlot(4).Value = 1 Then 'flow duration plot
'      Call GLInit(1, g, lnts, lnts + 1)
'      capt = "WDMUtil " & Cyllab & "/Duration Plot"
'      Call GLTitl(Ctitl, capt)
'      Call GLLegend(CLab())
'      Call GLAxLab("Percent chance " & Cyllab & " exceeded", Cyllab, Cyrlab, Calab)
'      vlab(0) = "Interval"
'      For j = 0 To lnts - 1
'        vlab(j + 1) = CLab(j)
'      Next j
'      Call GLVarLab(vlab)
'      Call GLDoFD(g, 1, STSer())
'    End If
  If chkPlot(5).Value = 1 Then 'XY Difference plot
    If lTser(1).dates.Summary.NVALS = lTser(2).dates.Summary.NVALS Then
      'same number of values, ok to do plot
      ReDim XYD(0)
      XYD(0).NVal = lTser(1).dates.Summary.NVALS
      ReDim XYD(0).Var(0).Vals(XYD(0).NVal)
      ReDim XYD(0).Var(1).Vals(XYD(0).NVal)
      XYD(0).Var(0).Trans = 1
      XYD(0).Var(1).Trans = 1
      For j = 0 To 1
        XYD(0).Var(j).Min = 1E+30
        XYD(0).Var(j).Max = -1E+30
      Next j
      For i = 0 To XYD(0).NVal - 1
        XYD(0).Var(0).Vals(i) = lTser(2).Value(i + 1) - lTser(1).Value(i + 1)
        XYD(0).Var(1).Vals(i) = lTser(1).Value(i + 1)
        For j = 0 To 1
          If XYD(0).Var(j).Vals(i) < XYD(0).Var(j).Min Then
            XYD(0).Var(j).Min = XYD(0).Var(j).Vals(i)
          End If
          If XYD(0).Var(j).Vals(i) > XYD(0).Var(j).Max Then
            XYD(0).Var(j).Max = XYD(0).Var(j).Vals(i)
          End If
        Next j
      Next i
      'make same pos and neg scale on Y-axis
      If XYD(0).Var(0).Max > Abs(XYD(0).Var(0).Min) And _
         XYD(0).Var(0).Max > 0 Then 'adjust min
        XYD(0).Var(0).Min = -1 * XYD(0).Var(0).Max
      ElseIf XYD(0).Var(0).Max < Abs(XYD(0).Var(0).Min) And _
             XYD(0).Var(0).Max > 0 Then 'adjust max
        XYD(0).Var(0).Max = -1 * XYD(0).Var(0).Min
      ElseIf XYD(0).Var(0).Max < 0 Then ' adjust max
        XYD(0).Var(0).Max = -1 * XYD(0).Var(0).Min
      End If
      Call GLInit(1, g, 1, 2)
      VLab(0) = Trim(clab(1)) & "-" & Trim(clab(0))
      VLab(1) = Trim(clab(0))
      If ConstInt Then
        ltitl = "Difference Plot (" & VLab(0) _
          & " vs " & VLab(1) & ")&for " _
          & Trim(ctran) & " " & Trim(ChrTUnit)
      Else
        ltitl = "Difference Plot (" & VLab(0) & " vs " & VLab(1)
        If Lcntsen = 1 Or Lcntcon = 1 Or Lcntloc = 1 Then
          ltitl = ltitl & ")&for "
        End If
      End If
      If Lcntsen = 1 Then 'add scenario to title
        ltitl = ltitl & " " & CScenm(0)
      End If
      If Lcntcon = 1 Then 'add constituent to title
        ltitl = ltitl & " " & cconnm(0)
      End If
      If Lcntloc = 1 Then 'add location to title
        ltitl = ltitl & " at " & clocnm(0)
      End If
      capt = "WDMUtil Difference Plot"
      Call GLTitl(ltitl, capt)
      Call GLIcon(frmGenScn.cmdAna(3).Picture)
      Call GLAxLab(clab(0), "Difference", Cyrlab, Calab)
      Call GLVarLab(VLab())
      Call GLDoXY(g, 1, XYD(), 1)
    Else 'can't do plot, incompatible timeseries
      MsgBox "Unable to perform Differences plot as the two timeseries being used do not contain the same number of values.", 48, "WDMUtil Plot Problem"
    End If
  End If
  If chkPlot(6).Value = 1 Then 'XY - Dsn2 v Dsn1
    If lTser(1).dates.Summary.NVALS = lTser(2).dates.Summary.NVALS Then
      'same number of values, ok to do plot
      ReDim XYD(0)
      XYD(0).NVal = lTser(1).dates.Summary.NVALS
      ReDim XYD(0).Var(0).Vals(XYD(0).NVal)
      ReDim XYD(0).Var(1).Vals(XYD(0).NVal)
      ReDim rtmp(2 * XYD(0).NVal)
      XYD(0).Var(0).Trans = 1
      XYD(0).Var(1).Trans = 1
      For j = 0 To 1
        XYD(0).Var(j).Min = 1E+30
        XYD(0).Var(j).Max = -1E+30
      Next j
      For i = 0 To XYD(0).NVal - 1
        XYD(0).Var(0).Vals(i) = lTser(2).Value(i + 1)
        XYD(0).Var(1).Vals(i) = STSer(1).Value(i + 1)
        rtmp(i) = lTser(2).Value(i + 1)
        rtmp(i + XYD(0).NVal) = lTser(1).Value(i + 1)
        For j = 0 To 1
          If XYD(0).Var(j).Vals(i) < XYD(0).Var(j).Min Then
            XYD(0).Var(j).Min = XYD(0).Var(j).Vals(i)
          End If
          If XYD(0).Var(j).Vals(i) > XYD(0).Var(j).Max Then
            XYD(0).Var(j).Max = XYD(0).Var(j).Vals(i)
          End If
        Next j
      Next i
      Call GLInit(1, g, 1, 2)
      If ConstInt Then
        ltitl = "Scatter Plot (" & Trim(clab(0)) _
          & " vs " & Trim(clab(1)) & ")&for " _
          & Trim(ctran) & " " & Trim(ChrTUnit)
      Else
        ltitl = "Scatter Plot (" & Trim(clab(0)) & " vs " & Trim(clab(1))
        If Lcntsen = 1 Or Lcntcon = 1 Or Lcntloc = 1 Then
          ltitl = ltitl & ")&for "
        End If
      End If
      If Lcntsen = 1 Then 'add scenario to title
        ltitl = ltitl & " " & CScenm(0)
      End If
      If Lcntcon = 1 Then 'add constituent to title
        ltitl = ltitl & " " & cconnm(0)
      End If
      If Lcntloc = 1 Then 'add location to title
        ltitl = ltitl & " at " & clocnm(0)
      End If
      capt = "WDMUtil Scatter Plot"
      Call GLTitl(ltitl, capt)
      'reverse labels so TS2 is on Y and TS1 is on X
      clab(2) = clab(0)
      clab(0) = clab(1)
      clab(1) = clab(2)
      Call GLAxLab(clab(1), clab(0), Cyrlab, Calab)
      Call GLVarLab(clab())
      If chkXYLine.Value = 1 Then
        Call F90_FITLIN(XYD(0).NVal, 2 * XYD(0).NVal, rtmp(0), ACoef, BCoef, RSquar)
        Call GLRegLines(1, ACoef, BCoef, RSquar)
        Call GLLegLoc(-2, -2)
      End If
      Call GLDoXY(g, 1, XYD(), 1)
    Else 'can't do plot, incompatible timeseries
      MsgBox "Unable to perform Scatter plot as the two timeseries being used do not contain the same number of values.", 48, "WDMUtil Plot Problem"
    End If
  End If
  MousePointer = ccDefault

End Sub

Private Sub cmdSelect_Click(Index As Integer)
    'select all (Index=1) or none (Index=0)
    'of the available time-series plots

    Dim i%

    For i = 0 To 6
      If chkPlot(i).Enabled Then
        chkPlot(i).Value = Index
      End If
    Next i

End Sub


Private Sub Form_Load()

  If Not (HelpCheck) Then
    If frmGenScn.TSerSelected.Count < 2 Then 'disable multi-dsn plots
      chkPlot(1).Enabled = False
      chkPlot(2).Enabled = False
      chkPlot(3).Enabled = False
      chkPlot(5).Enabled = False
      chkPlot(6).Enabled = False
    End If
    If chkPlot(6).Value = 0 Then
      chkXYLine.Enabled = False
      optStorm(0).Enabled = False
      optStorm(1).Enabled = False
    End If
    chkPlot(0).Value = 1
  End If
    
End Sub

Private Sub l_DataChanged(inum As Long)

  Dim i&, lnval&, NewVals#(), lVals!(), lsdate&(5), ioff&
  Dim lTs&(0), ltc&(0), nsdate&(5), nedate&(5), ldtyp&(0)
  Dim lSJDy#, lEJDy#
  Dim STSer As Collection
  Set STSer = frmGenScn.TSerSelected

'    Call l.GetTime(lts(), ltc(), nsdate(), nedate(), ldtyp())
    If STSer(inum).dates.Summary.ts = lTs(0) And STSer(inum).dates.Summary.Tu = ltc(0) Then
      'time units of listing match time units of edited time series
      lVals = STSer(inum).Values
      lSJDy = Date2J(nsdate)
      lEJDy = Date2J(nedate)
      lnval = lEJDy - lSJDy + 1
      ReDim NewVals(lnval)
'      Call l.GetData(inum, NewVals())
      'update data on this data set
      'find offset of start of values and start of timeseries
      Call F90_TIMDIF(lsdate(0), nsdate(0), ltc(0), ltc(0), ioff)
      'put updated time series in value buffer
      For i = 1 To lnval
        lVals(ioff + i) = CSng(NewVals(i))
      Next i
      STSer(inum).Values = lVals
      If Not STSer(inum).File.addtimser(STSer(inum), TsIdRepl) Then
        'problem updating data values
        MsgBox "Problem updating data values on data set.", vbExclamation, "WDMUtil View/Edit Problem"
      End If
    Else 'time units of listing not the same as time units of edited time series
      MsgBox "Time units of listed values does not match that of the WDM data set." & vbCrLf & "Updates to this data set will not be made.", vbInformation, "WDMUtil View/Edit Problem"
    End If

End Sub
