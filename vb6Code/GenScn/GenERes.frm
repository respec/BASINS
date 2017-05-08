VERSION 5.00
Begin VB.Form frmGenEstRes 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "GenScn Estimator Results"
   ClientHeight    =   4452
   ClientLeft      =   3096
   ClientTop       =   2388
   ClientWidth     =   4212
   BeginProperty Font 
      Name            =   "MS Sans Serif"
      Size            =   7.8
      Charset         =   0
      Weight          =   700
      Underline       =   0   'False
      Italic          =   0   'False
      Strikethrough   =   0   'False
   EndProperty
   Icon            =   "GenERes.frx":0000
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   PaletteMode     =   1  'UseZOrder
   ScaleHeight     =   4452
   ScaleWidth      =   4212
   ShowInTaskbar   =   0   'False
   Begin VB.Frame frmOutput 
      Caption         =   "Output Files"
      Height          =   852
      Left            =   120
      TabIndex        =   14
      Top             =   3000
      Width           =   3975
      Begin VB.CommandButton cmdDaily 
         Caption         =   "&Daily Loads"
         Enabled         =   0   'False
         Height          =   375
         Left            =   2640
         TabIndex        =   11
         Top             =   360
         Width           =   1212
      End
      Begin VB.CommandButton cmdCompare 
         Caption         =   "&Comparison"
         Enabled         =   0   'False
         Height          =   375
         Left            =   1200
         TabIndex        =   10
         Top             =   360
         Width           =   1335
      End
      Begin VB.CommandButton cmdMain 
         Caption         =   "&Main"
         Enabled         =   0   'False
         Height          =   375
         Left            =   120
         TabIndex        =   9
         Top             =   360
         Width           =   972
      End
   End
   Begin VB.CommandButton cmdCancel 
      Cancel          =   -1  'True
      Caption         =   "Cancel"
      Height          =   375
      Left            =   1560
      TabIndex        =   12
      Top             =   3960
      Width           =   972
   End
   Begin VB.Frame frmPlot 
      Caption         =   "Plots"
      Height          =   2772
      Left            =   120
      TabIndex        =   13
      Top             =   120
      Width           =   3975
      Begin VB.CommandButton cmdAll 
         Caption         =   "&All"
         Height          =   252
         Left            =   120
         TabIndex        =   0
         Top             =   360
         Width           =   732
      End
      Begin VB.CommandButton CmdNone 
         Caption         =   "&None"
         Height          =   252
         Left            =   1080
         TabIndex        =   1
         Top             =   360
         Width           =   732
      End
      Begin VB.CommandButton CmdGenerate 
         Caption         =   "&Generate"
         Default         =   -1  'True
         Height          =   375
         Left            =   120
         TabIndex        =   8
         Top             =   2280
         Width           =   1095
      End
      Begin VB.CheckBox chkPlot 
         Caption         =   "Residuals Against Time"
         Height          =   252
         Index           =   5
         Left            =   120
         TabIndex        =   7
         Top             =   1920
         Value           =   1  'Checked
         Width           =   3612
      End
      Begin VB.CheckBox chkPlot 
         Caption         =   "Residuals Against Flow"
         Height          =   252
         Index           =   4
         Left            =   120
         TabIndex        =   6
         Top             =   1680
         Value           =   1  'Checked
         Width           =   3612
      End
      Begin VB.CheckBox chkPlot 
         Caption         =   "Residuals Against Predicted Values"
         Height          =   252
         Index           =   3
         Left            =   120
         TabIndex        =   5
         Top             =   1440
         Value           =   1  'Checked
         Width           =   3612
      End
      Begin VB.CheckBox chkPlot 
         Caption         =   "Observed Concentrations Against Time"
         Height          =   252
         Index           =   2
         Left            =   120
         TabIndex        =   4
         Top             =   1200
         Value           =   1  'Checked
         Width           =   3735
      End
      Begin VB.CheckBox chkPlot 
         Caption         =   "Residuals Against Z-Scores"
         Height          =   252
         Index           =   1
         Left            =   120
         TabIndex        =   3
         Top             =   960
         Value           =   1  'Checked
         Width           =   3612
      End
      Begin VB.CheckBox chkPlot 
         Caption         =   "Boxplots of Residuals Against Month"
         Height          =   252
         Index           =   0
         Left            =   120
         TabIndex        =   2
         Top             =   720
         Value           =   1  'Checked
         Width           =   3612
      End
   End
End
Attribute VB_Name = "frmGenEstRes"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants

Private Sub cmdAll_Click()
  Dim i&
  For i = 0 To 5
    chkPlot(i).value = 1
  Next i
End Sub

Private Sub cmdCancel_Click()
  Unload frmGenEstRes
End Sub

Private Sub cmdCompare_Click()
  Dim cap$, outfile$
  cap = "GenScn Estimator Results"
  outfile = frmGenScnEstimator.lblFile(4)
  On Error GoTo 10
  Open outfile For Input As #1
  Close #1
  Call DispFile.OpenFile(outfile, cap, frmGenScnEstimator.Icon, False)
  GoTo 20
10 'continue
  MsgBox "This output file does not exist.", _
           vbExclamation, "GenScn Estimator Results"
20 'continue
End Sub

Private Sub cmdDaily_Click()
  Dim cap$, outfile$
  cap = "GenScn Estimator Results"
  outfile = frmGenScnEstimator.lblFile(5)
  On Error GoTo 10
  Open outfile For Input As #1
  Close #1
  Call DispFile.OpenFile(outfile, cap, frmGenScnEstimator.Icon, False)
  GoTo 20
10 'continue
  MsgBox "This output file does not exist.", _
           vbExclamation, "GenScn Estimator Results"
20 'continue
End Sub


Private Sub CmdGenerate_Click()
  
  Dim ngroup&, ni&(100), x#(3000), Title$, xtitle$, ytitle$, i&, j&
  Dim ndata&, nfun&, xp#(5000), ap#(5000, 10), range!(4), symbol$
  Dim XYD() As xyplotdata
  Dim g As Object
  Dim id6&(6), id3&(3), SDate&(6), EDate&(6)
  Dim vlab$(2), capt$, Cyrlab$, Calab$
  Dim Timbuf() As Double
  Dim icnt&, k&, n&
  
  If chkPlot(0).value = 1 Then
    'user wants boxplots
    Call F90_GBOXPN(i)
    If i > 0 Then
      'box plots available
      For j = 1 To i
        Call F90_GBOXP(j, ngroup, ni(), x(), Title)
        'ngroup = 3
        'ni(0) = 5
        'ni(1) = 16
        'ni(2) = 7
        'x(0) = 7#
        'x(1) = 9#
        'x(2) = 3#
        'x(3) = 1#
        'x(4) = 1#
        'x(5) = 25#
        'x(6) = 0#
        'x(7) = 1#
        'x(8) = 0#
        'x(9) = 5#
        'x(10) = 4#
        'x(11) = 3#
        'x(12) = 5#
        'x(13) = 5#
        'x(14) = 5#
        'x(15) = 5#
        'x(16) = 5#
        'x(17) = 5#
        'x(18) = 25#
        'x(19) = 15#
        'x(20) = 9#
        'x(21) = 10#
        'x(22) = 15#
        'x(23) = 20#
        'x(24) = 25#
        'x(25) = 2#
        'x(26) = 9#
        'x(27) = 12#
        'Title = "BOXPLOTS OF RESIDUALS AGAINST MONTH"
        ReDim XYD(ngroup)
        icnt = 0
        For k = 0 To ngroup
          XYD(k).NVal = ni(k)
          ReDim XYD(k).Var(0).Vals(XYD(k).NVal)
          XYD(k).Var(0).Trans = 1
          XYD(k).Var(0).Min = 1E+30
          XYD(k).Var(0).Max = -1E+30
          For n = 0 To XYD(k).NVal - 1
            XYD(k).Var(0).Vals(n) = x(icnt)
            icnt = icnt + 1
            If XYD(k).Var(0).Vals(n) < XYD(k).Var(0).Min Then
              XYD(k).Var(0).Min = XYD(k).Var(0).Vals(n)
            End If
            If XYD(k).Var(0).Vals(n) > XYD(k).Var(0).Max Then
              XYD(k).Var(0).Max = XYD(k).Var(0).Vals(n)
            End If
          Next n
        Next k
        Call GLInit(1, g, ngroup, 1)
        capt = "GenScn Estimator Boxplot"
        'Call GLLSpec(0, -1, -1, 3, -1)
        vlab(0) = ""
        vlab(1) = ""
        Call GLAxLab("", "", "", "")
        Call GLVarLab(vlab())
        Call GLTitl(Title, capt)
        Call GLIcon(frmGenEstRes.Icon)
        Call GLDoBox(g, 1, XYD(), 1)
      Next j
    Else
      'no box plots available
      MsgBox "Box plots are not available until the Estimator is run.", _
        vbExclamation, "GenScn Estimator Results"
    End If
  End If
  If chkPlot(1).value = 1 Or chkPlot(2).value = 1 Or chkPlot(3).value = 1 Or _
     chkPlot(4).value = 1 Or chkPlot(5).value = 1 Then
    'user wants plot
    Call F90_GPLOTN(i)
    If i > 0 Then
      'this type is available
      For j = 1 To i
        If chkPlot(j).value = 1 Then
          Call F90_GPLOTP(j, ndata, nfun, xp(), ap(), range(), symbol, xtitle, ytitle, Title)
          If j = 1 Or j = 3 Or j = 4 Then
            'xy plot of residuals
            ReDim XYD(0)
            XYD(0).NVal = ndata
            ReDim XYD(0).Var(0).Vals(XYD(0).NVal)
            ReDim XYD(0).Var(1).Vals(XYD(0).NVal)
            XYD(0).Var(0).Trans = 1
            XYD(0).Var(1).Trans = 1
            For k = 0 To 1
              XYD(0).Var(k).Min = 1E+30
              XYD(0).Var(k).Max = -1E+30
            Next k
            For n = 0 To XYD(0).NVal - 1
              XYD(0).Var(0).Vals(n) = ap(n, 0)
              XYD(0).Var(1).Vals(n) = xp(n)
              For k = 0 To 1
                If XYD(0).Var(k).Vals(n) < XYD(0).Var(k).Min Then
                  XYD(0).Var(k).Min = XYD(0).Var(k).Vals(n)
                End If
                If XYD(0).Var(k).Vals(n) > XYD(0).Var(k).Max Then
                  XYD(0).Var(k).Max = XYD(0).Var(k).Vals(n)
                End If
              Next k
            Next n
            For k = 0 To 1
              'make same pos and neg scale on Y-axis and X-axis
              If j <> 3 Or k <> 1 Then 'dont do for predicted values x-axis
                If XYD(0).Var(k).Max > Abs(XYD(0).Var(k).Min) And _
                   XYD(0).Var(k).Max > 0 Then 'adjust min
                  XYD(0).Var(k).Min = -1 * XYD(0).Var(k).Max
                ElseIf XYD(0).Var(k).Max < Abs(XYD(0).Var(k).Min) And _
                   XYD(0).Var(k).Max > 0 Then 'adjust max
                  XYD(0).Var(k).Max = -1 * XYD(0).Var(k).Min
                ElseIf XYD(0).Var(k).Max < 0 Then ' adjust max
                  XYD(0).Var(k).Max = -1 * XYD(0).Var(k).Min
                End If
              End If
            Next k
            Call GLInit(1, g, 1, 2)
            vlab(0) = xtitle
            vlab(1) = ytitle
            capt = "GenScn Estimator " & xtitle & " Plot"
            Call GLLSpec(0, -1, -1, 3, -1)
            Call GLTitl(Title, capt)
            Call GLAxLab(xtitle, ytitle, Cyrlab, Calab)
            Call GLVarLab(vlab())
            Call GLZLine(1, 1)
            Call GLDoXY(g, 1, XYD(), 1)
          ElseIf j = 2 Or j = 5 Then
            'do timeseries plot
            ReDim Timbuf(ndata, nfun)
            For k = 0 To nfun - 1 'put dates in buffer
              For n = 0 To ndata - 1
                Call F90_I3DATE(xp(n), id3(0))
                id6(0) = id3(0)
                id6(1) = id3(1)
                id6(2) = id3(2)
                id6(3) = 0
                id6(4) = 0
                id6(5) = 0
                Timbuf(n, k) = Date2J(id6())
              Next n
            Next k
            ReDim PLTser(nfun)
            For k = 0 To nfun - 1
              PLTser(k).NVal = ndata
              ReDim PLTser(k).Vals(PLTser(k).NVal)
              PLTser(k).ts = 1
              PLTser(k).Tu = 4
              PLTser(k).Min = 1E+30
              PLTser(k).Max = -1E+30
              For n = 0 To PLTser(0).NVal - 1
                PLTser(k).Vals(n) = ap(n, k)
                If PLTser(k).Vals(n) < PLTser(k).Min Then
                  PLTser(k).Min = PLTser(k).Vals(n)
                End If
                If PLTser(k).Vals(n) > PLTser(k).Max Then
                  PLTser(k).Max = PLTser(k).Vals(n)
                End If
              Next n
              If j = 5 Then
                'make same pos and neg scale on Y-axis
                If PLTser(k).Max > Abs(PLTser(k).Min) And _
                   PLTser(k).Max > 0 Then 'adjust min
                  PLTser(k).Min = -1 * PLTser(k).Max
                ElseIf PLTser(k).Max < Abs(PLTser(k).Min) And _
                       PLTser(k).Max > 0 Then 'adjust max
                  PLTser(k).Max = -1 * PLTser(k).Min
                ElseIf PLTser(k).Max < 0 Then ' adjust max
                  PLTser(k).Max = -1 * PLTser(k).Min
                End If
              End If
            Next k
            Call GLInit(1, g, nfun, 2 * nfun)
            vlab(0) = xtitle
            vlab(1) = ytitle
            capt = "GenScn Estimator " & xtitle & " Plot"
            Call GLVarLab(vlab())
            Call F90_I3DATE(xp(0), id3(0))
            SDate(0) = id3(0)
            SDate(1) = id3(1)
            SDate(2) = id3(2)
            SDate(3) = 0
            SDate(4) = 0
            SDate(5) = 0
            Call F90_I3DATE(xp(ndata - 1), id3(0))
            EDate(0) = id3(0)
            EDate(1) = id3(1)
            EDate(2) = id3(2)
            EDate(3) = 0
            EDate(4) = 0
            EDate(5) = 0
            Call GLDate(SDate(), EDate(), False)
            Call GLLSpec(0, 0, -1, 3, -1)
            Call GLTitl(Title, capt)
            Call GLLegend(vlab())
            Call GLAxLab(xtitle, ytitle, Cyrlab, Calab)
            Call GLZLine(1, 0)
            '---FIXME---Call GLDoTS(g, 1, PLTser(), Timbuf())
          End If
        End If
      Next j
    Else
      'this type is not available
      'MsgBox "Plots are not available until the Estimator is run.", _
        vbExclamation, "GenScn Estimator Results"
    End If
  End If
End Sub

Private Sub cmdMain_Click()
  Dim cap$, outfile$
  cap = "GenScn Estimator Results"
  outfile = frmGenScnEstimator.lblFile(3)
  On Error GoTo 10
  Open outfile For Input As #1
  Close #1
  Call DispFile.OpenFile(outfile, cap, frmGenScnEstimator.Icon, False)
  GoTo 20
10 'continue
  MsgBox "This output file does not exist.", _
           vbExclamation, "GenScn Estimator Results"
20 'continue
End Sub

Private Sub CmdNone_Click()
  Dim i&
  For i = 0 To 5
    chkPlot(i).value = 0
  Next i
End Sub

Private Sub Form_Load()
    If frmGenScnEstimator.lblFile(3) <> "<none>" Then
      cmdMain.Enabled = True
    End If
    If frmGenScnEstimator.lblFile(4) <> "<none>" Then
      cmdCompare.Enabled = True
    End If
    If frmGenScnEstimator.lblFile(5) <> "<none>" Then
      cmdDaily.Enabled = True
    End If
End Sub


