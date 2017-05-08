VERSION 5.00
Begin VB.Form frmGenScnPlot 
   Caption         =   "Graph"
   ClientHeight    =   4515
   ClientLeft      =   2385
   ClientTop       =   2070
   ClientWidth     =   4785
   HelpContextID   =   601
   Icon            =   "GenPlot.frx":0000
   LinkTopic       =   "Form1"
   PaletteMode     =   1  'UseZOrder
   ScaleHeight     =   4515
   ScaleWidth      =   4785
   Begin VB.CheckBox chkUniqueCons 
      Caption         =   "Multiple WQ Plots"
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
      Left            =   2640
      TabIndex        =   15
      Top             =   360
      Width           =   2055
   End
   Begin VB.CheckBox chkPlot 
      Caption         =   "&Bar Chart"
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
      HelpContextID   =   89
      Index           =   3
      Left            =   240
      TabIndex        =   5
      Top             =   1800
      Width           =   3252
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
      HelpContextID   =   92
      Left            =   2640
      TabIndex        =   9
      Top             =   2880
      Width           =   2172
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
      Caption         =   "Sca&tter (TS2  vs. TS1)"
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
      Index           =   6
      Left            =   240
      TabIndex        =   8
      Top             =   2880
      Width           =   2292
   End
   Begin VB.CheckBox chkPlot 
      Caption         =   "&Difference (TS2 - TS1) vs. TS1"
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
      HelpContextID   =   91
      Index           =   5
      Left            =   240
      TabIndex        =   7
      Top             =   2520
      Width           =   3252
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
      Left            =   240
      TabIndex        =   6
      Top             =   2160
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
      HelpContextID   =   88
      Index           =   2
      Left            =   240
      TabIndex        =   4
      Top             =   1440
      Width           =   3252
   End
   Begin VB.CheckBox chkPlot 
      Caption         =   "&Residual (TS2 - TS1 vs. time)"
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
      HelpContextID   =   87
      Index           =   1
      Left            =   240
      TabIndex        =   3
      Top             =   1080
      Width           =   3372
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
      HelpContextID   =   86
      Index           =   0
      Left            =   240
      TabIndex        =   2
      Top             =   720
      Width           =   1455
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

'Public l As ATCoDisp.ATCoList
'Public g As ATCoDisp.ATCoGraph

Private Sub chkPlot_Click(index As Integer)

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
  If index = 6 Then
    If chkPlot(6).Value = 1 Then
      chkXYLine.Enabled = True
    Else
      chkXYLine.Enabled = False
    End If
  End If

End Sub

Private Sub cmdCancel_Click()

  Unload frmGenScnPlot

End Sub

Private Sub cmdGen_Click()

  Dim i&, j&, ipos&, lnts&, ilen&
  Dim mxpts&, ConstInt As Boolean
  Dim CScenm$(), clocnm$(), cconnm$()
  Dim ltu&(), which&(), typind&()
  Dim Lcntcon&, Lcntsen&, Lcntloc&
  Dim Calab$, Cyrlab$, Cyllab$, Ctitl$, capt$
  Dim ctran$, ChrTUnit$, clab$(), ltitl$, VLab$()
  Dim rtmp!(), ACoef!, BCoef!, RSquar!
  Dim lTser As Collection  '
  Dim PLTser As Collection '
  Dim lTserCnt&
  Dim XYD() As xyplotdata
  Dim g As Object
  Dim n&, nplt&, nvalsOrig&, NVALS&
  Dim ltmp!, lValues!(), lFlags&(), lJday#()
  Dim uniCon$(), capExt$
  Dim lts As ATCclsTserData, lds As ATCclsTserDate
  Dim unitsName As String
  'Dim lSummary As ATTimSerDataSummary

  MousePointer = vbHourglass
'  If TSer.Count > 36 Then  'not doing more than 36 plots
'    lnts = 36
'  Else
'    lnts = TSer.Count
'  End If
  'get the data values for selected data sets
    
  
  Set lTser = FillTimSerExt(frmGenScn.TSerSelected, ConstInt)
  
  'Turning off ConstInt helps avoid having data that does not cover the whole
  'span be stretched to cover the whole span
  If CDTran = 4 Then ConstInt = False Else ConstInt = True
  
  lnts = lTser.Count
  If lnts < 1 Then
    MsgBox "The selected data has no values in the selected time", vbOKOnly, "Graph"
  Else
    'labels
    ReDim CScenm(lnts - 1)
    ReDim clocnm(lnts - 1)
    ReDim cconnm(lnts - 1)
    For i = 1 To lnts
      CScenm(i - 1) = lTser(i).Header.sen
      clocnm(i - 1) = lTser(i).Header.loc
      cconnm(i - 1) = lTser(i).Header.con
    Next i
    
    Dim Tran&
    If CDTran = 4 Then Tran = 1 Else Tran = CDTran
    If chkUniqueCons Then
      Call strUni(cconnm, nplt, uniCon)
    Else
      nplt = 1
    End If
    
    For n = 0 To nplt - 1
      Set PLTser = Nothing
      Set PLTser = New Collection
      
      If chkUniqueCons Then 'just the cons we are on
        lTserCnt = 0
        For i = 1 To lnts
          Set lts = lTser(i)
          If lts.Header.con = uniCon(n) Then
            PLTser.Add lts
            CScenm(lTserCnt) = lts.Header.sen
            clocnm(lTserCnt) = lts.Header.loc
            cconnm(lTserCnt) = lts.Header.con
            lTserCnt = lTserCnt + 1
            ReDim Preserve cconnm(lTserCnt)
            ReDim Preserve CScenm(lTserCnt)
            ReDim Preserve clocnm(lTserCnt)
          End If
        Next i
        capExt = " for " & uniCon(n)
      Else
        For i = 1 To lnts
          PLTser.Add lTser(i)
        Next i
        capExt = ""
      End If
      Set PLTser = ConvertCompatibleUnits(PLTser, p.PreferredUnits, p.UnitsRequired)
      lTserCnt = PLTser.Count
      For i = 1 To lTserCnt
        unitsName = PLTser(i).Attrib("Units")
        If Len(unitsName) > 0 Then
          If LCase(unitsName) <> "unknown" Then cconnm(i - 1) = cconnm(i - 1) & " (" & unitsName & ")"
        End If
      Next i
      ReDim which(lTserCnt)
      ReDim typind(lTserCnt)
      ReDim clab(lTserCnt)
      ReDim VLab(lTserCnt)
      'Call F90_SGLABL(lnts, CSCENM(), CLOCNM(), CCONNM(), CTUNIT, Tran, WHICH(), TYPIND(), Lcntcon, Lcntsen, Lcntloc, Calab, Cyrlab, Cyllab, Ctitl, CLAB(), CTRAN, ChrTUnit)
      Call DefaultLabels(CDTran, ConstInt, _
                     lTserCnt, CScenm(), clocnm(), cconnm(), ctunit, Tran, _
                     which(), typind(), _
                     Lcntcon, Lcntsen, Lcntloc, _
                     Calab, Cyrlab, Cyllab, Ctitl, clab(), ctran, ChrTUnit)
  
      If Not ConstInt Or CDTran = 4 Then 'Remove interval from labels
        'what is this doing????
        ReDim Preserve clab(2 * lTserCnt)
        ReDim VLab(2 * lTserCnt)
        For i = lTserCnt - 1 To 1 Step -1
          VLab(2 * i) = clab(i)
          VLab(2 * i + 1) = "Time"
        Next i
        VLab(1) = "Time"
      End If
      
      If chkPlot(0).Value = 1 Then 'standard timeseries plot
        capt = "Genscn Standard Plot" & capExt
        If ConstInt Then 'constant interval
          Call GLInit(1, g, lTserCnt, lTserCnt)
        Else
          Call GLInit(1, g, lTserCnt, 2 * lTserCnt)
        End If
        InitMatchingColors ""
        For i = 1 To PLTser.Count
          With PLTser(i).Header
            Call GLSetColor(i - 1, .sen & ":" & .loc & ":" & .con)
          End With
        Next
        Call GLVarLab(clab)
        Call GLDate(CSDat(), CEDat(), ConstInt)
        Call GLTitl(Ctitl, capt)
        Call GLLegend(clab())
        Call GLAxLab("", Cyllab, Cyrlab, Calab)
        Call GLDoTS(g, 1, PLTser)
      End If
      If chkPlot(1).Value = 1 Then 'residual plot
        If lTser(1).dates.Summary.NVALS = lTser(2).dates.Summary.NVALS Then
          'same number of values, ok to do plot
          Set PLTser = Nothing
          Set PLTser = New Collection
          Set lts = lTser(2).DoMath(ATCSubtract, lTser(1), 0)
          PLTser.Add lts
          'make same pos and neg scale on Y-axis
          'PLTser(1).Summary
          With PLTser(1)
            If .Max > Abs(.Min) And .Max > 0 Then 'adjust min
              .Min = -1 * .Max
            ElseIf .Max < Abs(.Min) And .Max > 0 Then 'adjust max
              .Max = -1 * .Min
            ElseIf .Max < 0 Then ' adjust max
              .Max = -1 * .Min
            End If
          End With
          'PLTser(1).Summary = lSummary
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
          capt = "Genscn Residual Plot" & capExt
          
          InitMatchingColors ""
          Call GLSetColor(0, "residual")
          
          Call GLTitl(ltitl, capt)
          Call GLLegend(clab())
          Call GLDate(CSDat(), CEDat(), ConstInt)
          Call GLAxLab("", Cyllab, Cyrlab, Calab)
          Call GLVarLab(VLab())
          Call GLZLine(1, 0)
          Call GLDoTS(g, 1, PLTser)
        Else 'can't do plot, incompatible timeseries
          MsgBox "Unable to perform Residual plot as the two timeseries being used do not contain the same number of values.", vbExclamation, "GenScn Plot Problem"
        End If
      End If
      If chkPlot(2).Value = 1 Then 'cummulative difference plot
        If lTser(1).dates.Summary.NVALS = lTser(2).dates.Summary.NVALS Then
          'same number of values, ok to do plot
          Set PLTser = Nothing
          Set PLTser = New Collection
          Set lts = lTser(2).DoMath(ATCCummDiff, lTser(1), 0)
          PLTser.Add lts
  
          'lSummary = PLTser(1).Summary
          With PLTser(1)
            If .Max > Abs(.Min) And .Max > 0 Then 'adjust min
              .Min = -1 * .Max
            ElseIf .Max < Abs(.Min) And .Max > 0 Then 'adjust max
              .Max = -1 * .Min
            ElseIf .Max < 0 Then ' adjust max
              .Max = -1 * .Min
            End If
          End With
          'PLTser(1).Summary = lSummary
          
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
          InitMatchingColors ""
          Call GLSetColor(0, "difference")
          capt = "Genscn Cumulative Differences Plot" & capExt
          Call GLTitl(ltitl, capt)
          Call GLLegend(clab())
          Call GLDate(CSDat(), CEDat(), ConstInt)
          Call GLAxLab("", Cyllab, Cyrlab, Calab)
          Call GLVarLab(VLab())
          Call GLZLine(1, 0)
          Call GLDoTS(g, 1, PLTser)
        Else 'can't do plot, incompatible timeseries
          MsgBox "Unable to perform Cumulative Difference plot as the two timeseries being used do not contain the same number of values.", vbExclamation, "GenScn Plot Problem"
        End If
      End If
      If chkPlot(3).Value = 1 Then 'timeseries bar plot
        If ConstInt Then
          nvalsOrig = lTser(1).dates.Summary.NVALS
          NVALS = nvalsOrig * (lTserCnt + 1)
          'build dummy date array
          Set lds = New ATCclsTserDate
          ReDim lJday(NVALS) ' init to 0
          ReDim lFlags(NVALS)
          lds.flags = lFlags
          For i = 1 To NVALS
            lJday(i) = lTser(1).dates.Value(1 + (i / (lTserCnt + 1)))
          Next i
          lds.Values = lJday
          lds.calcSummary
          
          Set PLTser = Nothing
          Set PLTser = New Collection
          For i = 1 To lTserCnt
            Set lts = New ATCclsTserData
            Set lts.dates = lds
            lts.flags = lFlags
            ReDim lValues(NVALS)
            For j = 1 To nvalsOrig
              ipos = i + ((j - 1) * (lTserCnt + 1))
              ltmp = lTser(i).Value(j)
              lValues(ipos) = ltmp
            Next j
            lts.Values = lValues
            lts.calcSummary
            PLTser.Add lts
          Next i
          Call GLInit(1, g, lTserCnt, lTserCnt)
          InitMatchingColors ""
          For i = 1 To PLTser.Count
            With PLTser(i).Header
              Call GLSetColor(i - 1, .sen & ":" & .loc & ":" & .con)
            End With
          Next
          Call GLDate(CSDat(), CEDat(), ConstInt)
          capt = "GenScn Bar Chart" & capExt
          Call GLTitl(Ctitl, capt)
          clab(lTserCnt) = ""
          Call GLLegend(clab())
          Call GLAxLab("", Cyllab, Cyrlab, Calab)
          Call GLVarLab(clab)
          Call GLDoTS(g, 1, PLTser)
        Else
          MsgBox "Unable to perform Bar Chart for non-constant interval timeseries.", vbExclamation, "GenScn Plot Problem"
        End If
      End If
      If chkPlot(4).Value = 1 Then 'flow duration plot
        Call GLInit(1, g, lTserCnt, lTserCnt + 1)
        capt = "Genscn " & Cyllab & "/Duration Plot" & capExt
        InitMatchingColors ""
        For i = 1 To lTser.Count
          With lTser(i).Header
            Call GLSetColor(i - 1, .sen & ":" & .loc & ":" & .con)
          End With
        Next
        Call GLTitl(Ctitl, capt)
        Call GLLegend(clab())
        Call GLAxLab("Percent chance " & Cyllab & " exceeded", Cyllab, Cyrlab, Calab)
        VLab(0) = "Interval"
        For j = 0 To lTserCnt - 1
          VLab(j + 1) = clab(j)
        Next j
        Call GLVarLab(VLab)
        Call GLDoFD(g, 1, lTser)
      End If
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
          capt = "Genscn Difference Plot" & capExt
          InitMatchingColors ""
          Call GLSetColor(0, "difference")
          Call GLTitl(ltitl, capt)
          Call GLAxLab(clab(0), "Difference", Cyrlab, Calab)
          Call GLVarLab(VLab())
          Call GLDoXY(g, 1, XYD(), 1)
        Else 'can't do plot, incompatible timeseries
          MsgBox "Unable to perform Differences plot as the two timeseries being used do not contain the same number of values.", vbExclamation, "GenScn Plot Problem"
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
            XYD(0).Var(1).Vals(i) = lTser(1).Value(i + 1)
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
          capt = "Genscn Scatter Plot" & capExt
          InitMatchingColors ""
          Call GLSetColor(0, "scatter")
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
          MsgBox "Unable to perform Scatter plot as the two timeseries being used do not contain the same number of values.", vbExclamation, "GenScn Plot Problem"
        End If
      End If
    Next n
  End If
  MousePointer = vbDefault

End Sub

Private Sub cmdSelect_Click(index As Integer)
  'select all (Index=1) or none (Index=0)
  'of the available time-series plots

  Dim i%

  For i = 0 To 6
    If chkPlot(i).Enabled Then
      chkPlot(i).Value = index
    End If
  Next i

End Sub


Private Sub Form_Load()
  'Set l = New ATCoDisp.ATCoList
  'Set g = New ATCoDisp.ATCoGraph

  If frmGenScn.TSerSelected.Count < 2 Then  'disable multi-dsn plots
    chkPlot(1).Enabled = False
    chkPlot(2).Enabled = False
    'chkPlot(3).Enabled = False
    chkPlot(5).Enabled = False
    chkPlot(6).Enabled = False
  End If
  If chkPlot(6).Value = 0 Then
    chkXYLine.Enabled = False
    optStorm(0).Enabled = False
    optStorm(1).Enabled = False
  End If
  chkPlot(0).Value = 1
  'disable bar charts for Native mode since that seems to be broken
  If frmGenScn.ctlGenDate.TAggr = 4 Then chkPlot(3).Enabled = False
End Sub
