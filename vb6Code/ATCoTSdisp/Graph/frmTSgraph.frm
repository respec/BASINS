VERSION 5.00
Begin VB.Form frmTSgraph 
   Caption         =   "Timeseries Graph"
   ClientHeight    =   4080
   ClientLeft      =   48
   ClientTop       =   276
   ClientWidth     =   5280
   Icon            =   "frmTSgraph.frx":0000
   LinkTopic       =   "Form1"
   ScaleHeight     =   4080
   ScaleWidth      =   5280
   StartUpPosition =   3  'Windows Default
   Begin VB.PictureBox picGraph 
      AutoRedraw      =   -1  'True
      BackColor       =   &H00FFFFFF&
      Height          =   4092
      Left            =   0
      ScaleHeight     =   4044
      ScaleWidth      =   5244
      TabIndex        =   0
      Top             =   0
      Width           =   5292
      Begin VB.Timer timDraw 
         Enabled         =   0   'False
         Interval        =   1000
         Left            =   240
         Top             =   120
      End
      Begin ATCoCtl.ATCoText txtPopup 
         Height          =   372
         Left            =   120
         TabIndex        =   1
         Top             =   3360
         Visible         =   0   'False
         Width           =   5052
         _ExtentX        =   8911
         _ExtentY        =   656
         InsideLimitsBackground=   16777215
         OutsideHardLimitBackground=   8421631
         OutsideSoftLimitBackground=   8454143
         HardMax         =   -999
         HardMin         =   -999
         SoftMax         =   -999
         SoftMin         =   -999
         MaxWidth        =   5
         Alignment       =   1
         DataType        =   0
         DefaultValue    =   ""
         Value           =   ""
         Enabled         =   -1  'True
      End
   End
End
Attribute VB_Name = "frmTSgraph"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants

Dim pData As Collection 'of GraphElement

Public Sub DrawGraph()
  Dim tsv As Variant
  Dim ts As ATCclsTserData, ds As ATCclsTserDate
  Dim ConstInt As Boolean
  Dim tim As Double, timMin As Double, timMax As Double
  Dim val As Single, valMin As Single, valMax As Single
  Dim index As Long, minIndex As Long, maxIndex As Long
  Dim timStart As Single, timNow As Single, RefreshInterval As Single
  Dim LeftMargin As Single, RightMargin As Single
  Dim TopMargin As Single, BottomMargin As Single
  
  If pData Is Nothing Then Exit Sub
  
  RefreshInterval = 0.5
  
  With picGraph
    .Cls
    .ScaleMode = vbInches
    LeftMargin = 0.5 / .ScaleWidth
    RightMargin = 0.5 / .ScaleWidth
    TopMargin = 0.5 / .ScaleHeight
    BottomMargin = 0.5 / .ScaleHeight
    For Each tsv In pData
      Set ts = tsv
      Set ds = ts.Dates
      valMin = ts.Min
      valMax = ts.Max
      timMin = ds.Summary.SJDay
      timMax = ds.Summary.EJDay
      minIndex = 1
      maxIndex = ds.Summary.NVALS
      picGraph.Scale (timMin - (timMax - timMin) * LeftMargin, valMax + (valMax - valMin) * TopMargin)- _
                     (timMax + (timMax - timMin) * RightMargin, valMin - (valMax - valMin) * BottomMargin)
      .CurrentX = ds.Value(minIndex)
      .CurrentY = ts.Value(minIndex)
      '.FillStyle = vbSolid
      .DrawStyle = vbSolid
      .ForeColor = RGB(Rnd * 250, Rnd * 250, Rnd * 250)
      ConstInt = ds.Summary.CIntvl
      timStart = Timer
      For index = minIndex + 1 To maxIndex
        If ConstInt Then
          picGraph.Line -(.CurrentX, ts.Value(index))
          picGraph.Line -(ds.Value(index), ts.Value(index))
        Else
          picGraph.Line -(ds.Value(index), ts.Value(index))
        End If
        timNow = Timer
        If timNow - timStart > RefreshInterval Then .Refresh: timStart = timNow
      Next index
    Next tsv
  End With
End Sub

Public Property Set Data(newData As Collection)
  Set pData = Nothing
  Me.Show
  Set pData = newData
  DrawGraph
End Property

Private Sub Form_Resize()
  If Width > 100 And Height > 400 Then
    picGraph.Width = Width - 84
    picGraph.Height = Height - 312
    DrawGraph
  End If
End Sub

Private Sub timDraw_Timer()
  Debug.Print "timDraw_Timer"
  picGraph.Refresh
End Sub
