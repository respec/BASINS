VERSION 5.00
Begin VB.Form frmG 
   Appearance      =   0  'Flat
   AutoRedraw      =   -1  'True
   BackColor       =   &H80000005&
   Caption         =   "Graph"
   ClientHeight    =   5295
   ClientLeft      =   345
   ClientTop       =   1665
   ClientWidth     =   7935
   BeginProperty Font 
      Name            =   "Marlett"
      Size            =   9.75
      Charset         =   2
      Weight          =   700
      Underline       =   0   'False
      Italic          =   0   'False
      Strikethrough   =   0   'False
   EndProperty
   ForeColor       =   &H00FFFFFF&
   HelpContextID   =   601
   Icon            =   "frmGraph.frx":0000
   LinkTopic       =   "Form2"
   PaletteMode     =   1  'UseZOrder
   ScaleHeight     =   5295
   ScaleWidth      =   7935
   Tag             =   "-1"
   Visible         =   0   'False
   Begin MSComDlg.CommonDialog CommonDialog1 
      Left            =   3480
      Top             =   2400
      _ExtentX        =   688
      _ExtentY        =   688
      _Version        =   393216
      CancelError     =   -1  'True
      DialogTitle     =   "Generate Windows Metafile"
      Filter          =   "Windows Metafile Files (*.wmf)|*.wmf|All Files(*.*)|*.*"
      Flags           =   6
      FontSize        =   2.54052e-29
      InitDir         =   "curdir"
   End
   Begin VB.PictureBox scrGraph 
      Appearance      =   0  'Flat
      AutoRedraw      =   -1  'True
      BackColor       =   &H00FFFFFF&
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   9.75
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H00000000&
      Height          =   5292
      Left            =   0
      ScaleHeight     =   5265
      ScaleMode       =   0  'User
      ScaleWidth      =   7905
      TabIndex        =   0
      Top             =   0
      Width           =   7932
      Begin ATCoCtl.ATCoText txtPopup 
         Height          =   375
         Left            =   600
         TabIndex        =   1
         Top             =   2040
         Visible         =   0   'False
         Width           =   5055
         _ExtentX        =   8916
         _ExtentY        =   661
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
   Begin VB.Menu mnuFile 
      Caption         =   "&File"
      Begin VB.Menu mnuPrint 
         Caption         =   "&Print"
      End
      Begin VB.Menu mnuSaveImage 
         Caption         =   "&Save"
      End
      Begin VB.Menu mnuSaveSpec 
         Caption         =   "Save &Specification"
         Visible         =   0   'False
      End
      Begin VB.Menu mnuOpenSpec 
         Caption         =   "&Open Specification"
      End
      Begin VB.Menu mnuSep 
         Caption         =   "-"
      End
      Begin VB.Menu mnuClose 
         Caption         =   "&Close"
      End
   End
   Begin VB.Menu mnuEdit 
      Caption         =   "&Edit"
      Begin VB.Menu mnuEdGraph 
         Caption         =   "&Axes"
         Index           =   0
      End
      Begin VB.Menu mnuEdGraph 
         Caption         =   "&Titles"
         Index           =   1
      End
      Begin VB.Menu mnuEdGraph 
         Caption         =   "&Curves"
         Index           =   2
      End
      Begin VB.Menu mnuEdGraph 
         Caption         =   "&Lines"
         Index           =   3
      End
      Begin VB.Menu mnuEdGraph 
         Caption         =   "&General"
         Index           =   4
      End
      Begin VB.Menu mnuFont 
         Caption         =   "&Font"
      End
      Begin VB.Menu mnuViewSep 
         Caption         =   "-"
      End
      Begin VB.Menu mnuCopy 
         Caption         =   "Copy to Clip&board"
      End
   End
   Begin VB.Menu mnuView 
      Caption         =   "&View"
      Begin VB.Menu mnuTransparent 
         Caption         =   "&Transparent"
      End
      Begin VB.Menu mnuList 
         Caption         =   "&Listing"
      End
   End
   Begin VB.Menu mnuCoordinateS 
      Caption         =   "&Coordinates"
      Begin VB.Menu mnuCoordinateTooltips 
         Caption         =   "Tooltips"
      End
      Begin VB.Menu mnuCoordinateMenubar 
         Caption         =   "Menubar"
      End
      Begin VB.Menu mnuCoordinateLines 
         Caption         =   "Lines"
      End
      Begin VB.Menu mnuCoordinateSeparator 
         Caption         =   "-"
      End
      Begin VB.Menu mnuCoordinateSnap 
         Caption         =   "Snap to nearest"
      End
      Begin VB.Menu mnuCoordinateSnapX 
         Caption         =   "Snap to nearest X"
      End
      Begin VB.Menu mnuCoordinateSnapY 
         Caption         =   "Snap to nearest Y"
      End
      Begin VB.Menu mnuCoordinateCurve 
         Caption         =   "-"
         Index           =   0
         Visible         =   0   'False
      End
   End
End
Attribute VB_Name = "frmG"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2001 by AQUA TERRA Consultants

'Public atcgr As ATCoGraph
'Dim GraphHDC As Long 'for redraw graph
Dim result As Long
'Dim di As DOCINFO

'plotting data buffer
Const POSMAX As Long = 18
Dim bufpos(2, 2 * POSMAX) As Long '(1=start pos in yx 2=end pos in yx, curve number)
Dim wchvr(2, POSMAX) As Integer
Dim yx() As Double
Dim dataLabels() As String
Dim nDataLabels As Long
Dim DataLabelPosition As Long  '0=none, 1=horizontal, 2=vertical, 3=popup
Dim nearest As Long            'Y position in yx() of nearest point to mouse. -999=none, -10?=const line, -?=legend item -99=legend
Dim curveToTrack As Long
Const NearnessTolerance = 0.1  'inches beyond which nothing is nearest
Dim PopupEditing As Long              '0=none, 1-leftY, 2-rightY, 3-Aux, 4-X, 20-Title, <0 = legend
Private pModal As Boolean

Public Graph As Object
'plotting stuff
Public Title As String
Public Gridx As Long    'grid x:  1=yes
Public lGridy As Long
Public rGridy As Long
Public MrkSiz As Single
Private Type AxType
  AType As Long   'axis type 0TM,1AR,2LG,3-8PRB
  SType As Long   'sub axis type (time:0-undef,6-Yr,5-Mo,4-Dy,3-Hr)
  STypeInt As Long 'sub axis type Interval value
  minv As Single  'min axis value
  maxv As Single  'max axis value
  NTic As Long    'number of tics
  TLen As Single  'tic length
  label As String 'axis label
End Type
Dim Axis(1 To 4) As AxType '1-leftY,2-rightY,3-Aux,4-X
Private Type VarType
    minv As Single
    maxv As Single
    WchAx As Long
    Trans As Long
    label As String
End Type
Private Type CrvType
    CType As Long
    LType As Long
    LThck As Long
    SType As Long
    Color As Long
    LegLbl As String * 80
End Type
Private Type LineType
    WchAx As Long
    LType As Long
    LThck As Long
    SType As Long
    Color As Long
    LegLbl As String
    Value As String
End Type
Dim Var(2 * POSMAX) As VarType
Dim Crv(POSMAX) As CrvType
Dim Lines() As LineType

Dim Nvar As Long
Dim ncrv As Long
Dim nLine As Long

Dim dtype(POSMAX) As Long
Dim TSTEP(POSMAX) As Long
Dim TUnits(POSMAX) As Long
Dim fill(POSMAX) As Long
Dim sdatim(6) As Long
Dim edatim(6) As Long
Dim XLegLoc As Single, YLegLoc As Single
Dim XLegMin As Single, YLegMin As Single
Dim XLegMax As Single, YLegMax As Single

Dim XTxtLoc As Single, YTxtLoc As Single
Dim XTxtMin As Single, YTxtMin As Single
Dim XTxtMax As Single, YTxtMax As Single
Dim XtraText As String
Dim LogPixelsY As Long, LogPixelsX As Long
Dim HorzRes As Long, VertRes As Long
Dim AspectX As Long, AspectY As Long
Dim HorzSize As Long, VertSize As Long
Dim XLen As Single, YLen As Single, ALen As Single
Dim lastX1 As Single, lastY1 As Single, lastX2 As Single, lastY2 As Single 'line dragging position
Dim XMouseDown As Single, YMouseDown As Single
Dim draggingAxis As Long 'negative=mouse near but not dragging
'0=none,1=left,2=right,3=aux(not implemented),4=bottom, 5=top, 6=ConstX, 7=ConstY, 8=extra text, 9=legend, 10=rectangle
Dim showingCrosshairs As Boolean
Dim crosshairX As Single, crosshairY As Single

Private Function Wc2DcY(Graph As Object, ByVal y As Single) As Single
  Dim d As Single
  d = ((y - CSng(Graph.ScaleTop)) / CSng(Graph.ScaleHeight))
  Wc2DcY = d * Graph.Height * CSng(LogPixelsY) / 1440!
End Function

Private Function Wc2DcX(Graph As Object, ByVal x As Single) As Single
  Dim d As Single
  d = ((x - CSng(Graph.ScaleLeft)) / CSng(Graph.ScaleWidth))
  Wc2DcX = d * CSng(Graph.Width) * CSng(LogPixelsX) / 1440!
End Function

Private Function Wc2Val(Graph As Object, ByRef axisNum As Long, ByVal wc As Single, Optional AllowOutsideAxisRange As Boolean = True) As Single
  Dim AxisLen As Single
  Select Case axisNum
    Case 1, 2
        If ALen > 0 Then  'aux axis in use
          AxisLen = YLen - (ALen * YLen) - Abs(Graph.TextHeight("X"))
        Else  'no aux axis
          AxisLen = YLen
        End If
    Case 3: AxisLen = ALen * YLen
            wc = wc - (YLen - (ALen * YLen))
    Case 4: AxisLen = XLen
  End Select
  With Axis(axisNum)
    Wc2Val = .minv + (.maxv - .minv) * (wc / AxisLen)
    
    If Not AllowOutsideAxisRange Then
      If Wc2Val > .maxv Then
'        If ALen > 0 And axisNum = 1 Then 'Switch to aux axis
'          axisNum = 3
'          Wc2Val = Wc2Val(Graph, axisNum, wc - AxisLen - Abs(Graph.TextHeight("X")), AllowOutsideAxisRange)
'          Exit Function
'        Else
          Wc2Val = NONE
'        End If
      End If
    
      If Wc2Val < .minv Then Wc2Val = NONE
    End If
    
    If Wc2Val <> NONE Then
      Select Case .AType
        Case 0:                          'Time
        Case 1:                          'Arithmetic
        Case 2, -2: Wc2Val = 10 ^ Wc2Val 'Log
        '3 to 8 are probability
      End Select
    End If
  End With
End Function

Public Sub GraphPrint(ByVal str As String, Optional AngleDegrees As Long = 0)
  Dim FontToUse As Long, FontToSave As Long
  Dim hdc As Long, localX As Long, localY As Long
  hdc = Graph.hdc
  localX = Wc2DcX(Graph, Graph.CurrentX)
  localY = Wc2DcY(Graph, Graph.CurrentY)
  lf.lfEscapement = AngleDegrees * 10
  lf.lfOrientation = lf.lfEscapement
  FontToUse = CreateFontIndirect(lf)
  FontToSave = SelectObject(hdc, FontToUse)
  TextOut hdc, localX, localY, str, Len(str)
  lf.lfEscapement = 0
  lf.lfOrientation = 0
  SelectObject hdc, FontToSave
  DeleteObject FontToUse
End Sub


'value must be just a number along WchAx to align line with
Public Sub AddLine(WchAx As Long, LType As Long, LThck As Long, SType As Long, Color As Long, LegLbl As String, Value As String)
  NumLines = NumLines + 1
  With Lines(NumLines)
    If WchAx = 0 Then .WchAx = 4 Else .WchAx = WchAx
    .LType = LType - 1
    .LThck = LThck
    .SType = SType
    .Color = Color
    .LegLbl = LegLbl
    .Value = Value
  End With
End Sub

Public Sub GetLine(index As Long, ByRef WchAx As Long, ByRef LType As Long, ByRef LThck As Long, ByRef SType As Long, ByRef Color As Long, ByRef LegLbl As String, ByRef Value As String)
  'index As Long, WchAx As Long, LType As Long, LThck As Long, SType As Long, Color As Long, LegLbl As String, Value As String
  If index > 0 And index <= NumLines Then
    With Lines(index)
      WchAx = .WchAx
      LType = .LType + 1
      LThck = .LThck
      SType = .SType
      Color = .Color
      LegLbl = .LegLbl
      Value = .Value
    End With
  End If
End Sub

Public Sub DispError(SubID As String, e As Object)

  DbgMsg "From Sub " & SubID & ":" & e.Description & ", # " & e.Number
    
    'Static ecnt%
    'ecnt = ecnt + 1
    'If ecnt < 5 Then
      'MsgBox "From Sub " & SubID & ":" & vbCrLf & e.Description & vbCrLf & "Error number " & e.Number, 48
    'End If

End Sub

Public Sub GetAddText(XTPos As Single, YTPos As Single, AddedText As String)

    XTPos = XTxtLoc
    YTPos = YTxtLoc
    AddedText = XtraText

End Sub

Public Sub SetAddText(XTPos As Single, YTPos As Single, AddedText As String)

    XTxtLoc = XTPos
    YTxtLoc = YTPos
    XtraText = AddedText

End Sub

Public Sub SetBoxPlot(i As Long, median As Single, x() As Single)

    Dim lpoints() As Single, npts As Long, j As Long, hspread As Single
 
    npts = bufpos(2, i) - bufpos(1, i) + 1
    ReDim lpoints(npts)
    For j = bufpos(1, i) To bufpos(2, i)
      'fill local array of points
      lpoints(j - bufpos(1, i)) = yx(j)
    Next j
    Call F90_ASRTRP(npts, lpoints(0))
    'find median
    If npts / 2 = Int(npts / 2) Then
      'even number of points
      median = (lpoints(npts / 2) + lpoints((npts / 2) - 1)) / 2
    Else
      'odd number of points
      median = lpoints(Int(npts / 2))
    End If
    'figure dimensions of boxes
    If npts / 4 = Int(npts / 4) Then
      'npts is divisible by four (8,12,16...)
      x(1) = (lpoints(Int(npts / 4)) + lpoints(Int(npts / 4) - 1)) / 2
      x(2) = (lpoints(Int(3 * npts / 4)) + lpoints(Int(3 * npts / 4) - 1)) / 2
    ElseIf npts / 2 = Int(npts / 2) Then
      'npts not divisible by four but by two (6,10,14...)
      x(1) = lpoints(((npts + 2) / 4) - 1)
      x(2) = lpoints(((npts + 2) * 3 / 4) - 2)
    ElseIf (npts + 1) / 4 = Int((npts + 1) / 4) Then
      'npts is odd, avg two values (7,11,15...)
      x(1) = (lpoints(Int((npts + 1) / 4)) + lpoints(Int((npts + 1) / 4) - 1)) / 2
      x(2) = (lpoints(Int(3 * (npts + 1) / 4) - 1) + lpoints(Int(3 * (npts + 1) / 4) - 2)) / 2
    Else
      'npts is odd, one value needed (9,13,17...)
      x(1) = lpoints(Int((npts + 3) / 4) - 1)
      x(2) = lpoints(Int(2 * (npts + 3) / 4))
    End If
    'calculate ends of whiskers
    hspread = x(2) - x(1)
    x(0) = x(1)
    x(3) = x(2)
    j = 0
    While j <= npts And x(0) = x(1)
      If lpoints(j) >= x(1) - (1.5 * hspread) Then
        x(0) = lpoints(j)
      End If
      j = j + 1
    Wend
    j = npts - 1
    While j >= 0 And x(3) = x(2)
      If lpoints(j) <= x(2) + (1.5 * hspread) Then
        x(3) = lpoints(j)
      End If
      j = j - 1
    Wend
End Sub

Public Sub SetConstLines(yconfg As Long, yconval As Single, xconfg As Long, xconval As Single)

  If yconfg = 1 Then AddLine 1, 1, 1, 0, vbBlack, "y=" & yconval, CStr(yconval)
  If xconfg = 1 Then AddLine 4, 1, 1, 0, vbBlack, "x=" & xconval, CStr(yconval)

End Sub

'Dim ReSize As Boolean
Public Sub SetFill(icrv As Long, ifill As Long)

    'set fill for each curve
    If icrv <= POSMAX Then
      fill(icrv) = ifill
    End If

End Sub

Public Sub GetGrid(getGridX As Long, getLgridY As Long, getRgridY As Long)
  getGridX = Gridx
  getLgridY = lGridy
  getRgridY = rGridy
End Sub

Public Sub SetGrid(ByVal newGridX As Long, ByVal newLgridY As Long, ByVal newRgridY As Long)
  'set whether grid is displayed for x and y axes
  Gridx = newGridX
  lGridy = newLgridY
  rGridy = newRgridY
End Sub

Public Sub GetTitles(titl As String, capt As String)

    'get plot title and form caption
    titl = Title
    capt = Caption

End Sub
Public Sub GetLegLoc(XLPos As Single, YLPos As Single)

    XLPos = XLegLoc
    YLPos = YLegLoc

End Sub

Public Sub SetLegLoc(XLPos As Single, YLPos As Single)

    XLegLoc = XLPos
    YLegLoc = YLPos

End Sub

Public Sub SetTitles(titl As String, capt As String)

    'set plot title and form caption
    Title = titl
    Caption = capt

End Sub

Public Sub SetIcon(ic As Object)

    'set icon for plot form
    icon = ic

End Sub

Public Sub GetVarInfo(vmin() As Single, vmax() As Single, which() As Long, tran() As Long, vlab() As String)
    'get min/max, which axis, any transformation,
    'and a label for each variable in the plot
    Dim i As Long

    For i = 0 To Nvar - 1
      vmin(i) = Var(i).minv
      vmax(i) = Var(i).maxv
      which(i) = Var(i).WchAx
      vlab(i) = Var(i).label
      tran(i) = Var(i).Trans
    Next i

End Sub
Public Sub SetVarInfo(vmin() As Single, vmax() As Single, which() As Long, tran() As Long, vlab() As String)
    'set min/max, which axis, any transformation,
    'and a label for each variable in the plot
    Dim i As Long

    For i = 0 To Nvar - 1
      Var(i).minv = vmin(LBound(vmin) + i)
      Var(i).maxv = vmax(LBound(vmax) + i)
      Var(i).WchAx = which(LBound(which) + i)
      Var(i).label = vlab(LBound(vlab) + i)
      Var(i).Trans = tran(LBound(tran) + i)
    Next i

End Sub

'Private Sub Form_Activate()
'
'  If frmG.Tag > -1 Then
'    Set curG = g(frmG.Tag)
'  End If
'
'End Sub

Private Sub Form_Load()
  nDataLabels = 0
  Title = ""
  Gridx = 0
  lGridy = 0
  rGridy = 0
  'ConstYfg = 0
  'ConstXfg = 0
  XLegLoc = 0
  YLegLoc = 1
  ncrv = 0
  nLine = 0
  Nvar = 0
  draggingAxis = 0
  
  On Error Resume Next 'Ignore errors if registry is corrupted
  mnuCoordinateLines.Checked = GetSetting(appName, "Coordinates", "Lines", False)
  mnuCoordinateMenubar.Checked = GetSetting(appName, "Coordinates", "Menubar", False)
  mnuCoordinateSnap.Checked = GetSetting(appName, "Coordinates", "Snap", False)
  mnuCoordinateSnapX.Checked = GetSetting(appName, "Coordinates", "SnapX", False)
  mnuCoordinateSnapY.Checked = GetSetting(appName, "Coordinates", "SnapY", False)
  mnuCoordinateTooltips.Checked = GetSetting(appName, "Coordinates", "Tooltips", False)
End Sub

Private Sub Form_Resize()

    If Visible And Height > 675 And Width > 100 Then
      'ok to resize (not minimized)
      scrGraph.Height = Me.ScaleHeight 'Height - 600
      scrGraph.Width = Me.ScaleWidth 'Width - 100
'      ReSize = True
      Call ReDrawGraph(0)
    End If

End Sub

Private Sub Form_Unload(Cancel As Integer)
  SaveSetting appName, "Coordinates", "Lines", mnuCoordinateLines.Checked
  SaveSetting appName, "Coordinates", "Menubar", mnuCoordinateMenubar.Checked
  SaveSetting appName, "Coordinates", "Snap", mnuCoordinateSnap.Checked
  SaveSetting appName, "Coordinates", "SnapX", mnuCoordinateSnapX.Checked
  SaveSetting appName, "Coordinates", "SnapY", mnuCoordinateSnapY.Checked
  SaveSetting appName, "Coordinates", "Tooltips", mnuCoordinateTooltips.Checked
End Sub

Private Sub mnuClose_Click()
  Unload Me
  'Me.Tag = -1
End Sub

Private Sub mnuCoordinateCurve_Click(index As Integer)
  Dim iMnu As Integer
  If Not mnuCoordinateCurve(index).Checked Then 'check this one and un-check all the others
    For iMnu = 1 To mnuCoordinateCurve.UBound
      If iMnu <> index Then mnuCoordinateCurve(iMnu).Checked = False
    Next
    mnuCoordinateCurve(index).Checked = True
    curveToTrack = index - 1
  End If
End Sub

Private Sub mnuCoordinateLines_Click()
  mnuCoordinateLines.Checked = Not mnuCoordinateLines.Checked
End Sub

Private Sub mnuCoordinateMenubar_Click()
  mnuCoordinateMenubar.Checked = Not mnuCoordinateMenubar.Checked
End Sub

Private Sub mnuCoordinateSnap_Click()
  mnuCoordinateSnap.Checked = Not mnuCoordinateSnap.Checked
  mnuCoordinateSnapX.Checked = False
  mnuCoordinateSnapY.Checked = False
End Sub

Private Sub mnuCoordinateSnapX_Click()
  mnuCoordinateSnapX.Checked = Not mnuCoordinateSnapX.Checked
  mnuCoordinateSnap.Checked = False
  mnuCoordinateSnapY.Checked = False
End Sub

Private Sub mnuCoordinateSnapY_Click()
  mnuCoordinateSnapY.Checked = Not mnuCoordinateSnapY.Checked
  mnuCoordinateSnap.Checked = False
  mnuCoordinateSnapX.Checked = False
End Sub

Private Sub mnuCoordinateTooltips_Click()
  mnuCoordinateTooltips.Checked = Not mnuCoordinateTooltips.Checked
End Sub

Private Sub mnuCopy_Click()
  Clipboard.Clear
  Clipboard.SetData scrGraph.Image
End Sub

Private Sub mnuEdGraph_Click(index As Integer)
  Set frmG = Me
  frmGEdit.sstGEdit.Tab = index
  frmGEdit.Show 1
End Sub

Private Sub mnuFont_Click()
  CopyFont scrGraph, CommonDialog1
  On Error GoTo ExitSub
  CommonDialog1.CancelError = True
  CommonDialog1.flags = cdlCFBoth + cdlCFScalableOnly + cdlCFWYSIWYG
  CommonDialog1.ShowFont
  CopyFont CommonDialog1, scrGraph
  CopyFont CommonDialog1, txtPopup
  ReDrawGraph 0
ExitSub:
  CommonDialog1.CancelError = False
End Sub

Private Sub CopyFont(src As Object, dst As Object)
  On Error Resume Next 'Some objects have only some of the font attributes
  dst.FontBold = src.FontBold
  dst.FontItalic = src.FontItalic
  dst.FontName = src.FontName
  dst.FontSize = src.FontSize
  dst.FontStrikethru = src.FontStrikethru
  dst.FontUnderline = src.FontUnderline
  dst.FontTransparent = src.FontTransparent
End Sub

Private Sub mnuList_Click()

    Dim i As Long, j As Long, NVal As Long, LType As Long, kx As Long, ky As Long, ipos As Long
    Dim rval() As Double

    MousePointer = vbHourglass
    Call frmL.init
    If Crv(0).CType < 5 Then 'constant interval timeseries
      LType = 1
    ElseIf Crv(0).CType = 5 Then 'non constant interval timeseries
      LType = 2
    Else 'xy
      LType = 0
    End If
    Call frmL.SetListType(LType)
    ipos = -1
    For j = 0 To ncrv - 1
      ky = wchvr(1, j)
      NVal = bufpos(2, ky) - bufpos(1, ky) + 1
      ReDim rval(NVal)
      If LType = 0 Or LType = 2 Then
        'xy listing or date tagged timeseries, include x data
        kx = wchvr(2, j)
        If bufpos(1, kx) > bufpos(1, ky) Then
          'set y data 1st
          If bufpos(1, ky) > ipos Then
            'haven't already added this data
            For i = 0 To NVal - 1
              rval(i) = yx(bufpos(1, ky) + i)
            Next i
            Call frmL.SetData(ky, bufpos(1, ky), NVal, rval, i)
            ipos = bufpos(1, ky)
          End If
          'now set x
          If bufpos(1, kx) > ipos Then
            'haven't already added this data
            For i = 0 To NVal - 1
              rval(i) = yx(bufpos(1, kx) + i)
            Next i
            Call frmL.SetData(kx, bufpos(1, kx), NVal, rval, i)
            ipos = bufpos(1, kx)
          End If
        Else
          'set x data 1st
          If bufpos(1, kx) > ipos Then
            'haven't already added this data
            For i = 0 To NVal - 1
              rval(i) = yx(bufpos(1, kx) + i)
            Next i
            Call frmL.SetData(kx, bufpos(1, kx), NVal, rval, i)
            ipos = bufpos(1, kx)
          End If
          'now set y
          If bufpos(1, ky) > ipos Then
            'haven't already added this data
            For i = 0 To NVal - 1
              rval(i) = yx(bufpos(1, ky) + i)
            Next i
            Call frmL.SetData(ky, bufpos(1, ky), NVal, rval, i)
            ipos = bufpos(1, ky)
          End If
        End If
      Else
        kx = ncrv
        'just set y data
        If bufpos(1, ky) > ipos Then
          'haven't already added this data
          For i = 0 To NVal - 1
            rval(i) = yx(bufpos(1, ky) + i)
          Next i
          Call frmL.SetData(ky, bufpos(1, ky), NVal, rval, i)
          ipos = bufpos(1, ky)
        End If
      End If
      Call frmL.SetVars(j, ky, kx)
    Next j
    Call frmL.SetNumVars(ncrv, Nvar)
    Call frmL.SetTime(TSTEP, TUnits, sdatim, edatim, dtype)
    For j = 0 To Nvar - 1
      Call frmL.SetFldPrms(j, 8, 4, 2)
      Call frmL.SetVarLabel(j, Var(j).label)
      Call frmL.SetFldRange(j, -1E+30, 1E+30, True)
    Next j
    If LType > 0 Then 'set default time summations
      Call frmL.SetSums(CLng(1), dtype, TUnits, 9, 0)
    End If
    Call frmL.SetTitles(Title, Me.Caption & " List")
    Call frmL.ShowIt(pModal)
    MousePointer = vbDefault

End Sub

'Private Sub mnuMeta_Click()
'
'    Dim ErrRet As Long, fname As String
'    On Error GoTo errhandler
'    ErrRet = 0
'    'get name from user
'    CommonDialog1.ShowSave
'    fname = CommonDialog1.Filename
'
'    ErrRet = 1
'    If FileLen(fname) > 0 Then
'      'get rid of existing file
'      If MsgBox("Replace existing file " & fname & "?", vbOKCancel, "Confirmation") = vbOK Then
'        Kill fname
'      Else
'        Exit Sub
'      End If
'    End If
'    ErrRet = 0
'BackFromErr3:
'
'    SavePicture scrGraph.Image, fname
'
'BackFromErr:
'    Exit Sub
'errhandler:
'    If ErrRet = 0 Then
'      Resume BackFromErr
'    Else
'      Resume BackFromErr3
'    End If
'    Resume BackFromErr
'
'End Sub

Private Sub mnuOpenSpec_Click()
  On Error GoTo ExitSub
  'CommonDialog1.Filename = SaveFilename
  CommonDialog1.Filter = "Graph files (*.grf)|*.grf|All Files (*.*)|*.*"
  'CommonDialog1.flags = &H3804& 'create & not read only
  CommonDialog1.ShowOpen
  If Len(CommonDialog1.FileName) > 0 Then
    On Error GoTo SaveError
    RetrieveSpecs CommonDialog1.FileName
  End If
ExitSub:
  Exit Sub
SaveError:
  MsgBox "Could not open " & CommonDialog1.FileName & vbCr & Err.Description, vbOKOnly, "Error"
End Sub

Private Sub mnuPrint_Click()
  'Printer.Orientation = vbPRORLandscape 'vbPRORPortrait
      
'  scrGraph.Width = Printer.Width 'GetDeviceCaps(Printer.hDC, 8) '* Screen.TwipsPerPixelX
'  scrGraph.Height = Printer.Height 'GetDeviceCaps(Printer.hDC, 10) '* Screen.TwipsPerPixelY
'  ReDrawGraph 0
'  Printer.Print "";
'  Printer.PaintPicture scrGraph.Image, 0, 0
'  Printer.EndDoc
  'If MsgBox("Directly printing a graph does not reliably make a good image." _
  '  & vbCrLf & "The reccommended procedure is to save a metafile," _
  '  & vbCrLf & "then import it into another program such as Word or Powerpoint for printing." _
  '  & vbCrLf & "Do you want to attempt to print directly to the printer?", vbYesNo, "Graph Print Warning") = vbYes Then
    Call ReDrawGraph(1)
  'End If
End Sub


Private Sub DrwAxis()

    'put tics and labels on axes
On Error GoTo errorhandler9:
    Dim ymlen As Single, auxlen As Single, SizeL As Single, rmax1 As Single, stdev As Single
    Dim npts As Long, ndays As Long, tstype As Long, tsmin As Long, ldt(6) As Long

    If ALen > 0 Then
      auxlen = ALen * YLen
      ymlen = YLen - auxlen - Abs(Graph.TextHeight(""))
      SizeL = Abs(Graph.TextHeight(""))
      Call matchl(Graph, SizeL, Axis(3).minv, Axis(3).maxv, Axis(1).minv, Axis(1).maxv, Abs(Axis(1).AType), Axis(3).NTic, Axis(1).NTic, rmax1)
      Call AuxAxe(Graph, XLen, ymlen, auxlen, SizeL, Axis(3).label, Axis(3).NTic, Axis(3).minv, Axis(3).maxv, rmax1)
    Else
      ymlen = YLen
      rmax1 = 999
    End If
    'Left Y axis
    If Axis(1).AType = 1 Then 'arithmetic axis
      Call ArAxis(Graph, XLen, ymlen, Axis(1).label, Axis(1).minv, Axis(1).maxv, 1, rmax1, Axis(1).TLen, Axis(1).NTic, lGridy)
    ElseIf Abs(Axis(1).AType) = 2 Then 'logarithmic axis
      Call LgAxis(Graph, XLen, ymlen, Axis(1).label, Axis(1).minv, Axis(1).maxv, 1, rmax1, Axis(1).TLen, lGridy)
    End If
    'Right Y axis
    If Axis(2).AType = 1 Then 'arithmetic axis
      Call ArAxis(Graph, XLen, ymlen, Axis(2).label, Axis(2).minv, Axis(2).maxv, 2, rmax1, Axis(2).TLen, Axis(2).NTic, rGridy)
    ElseIf Abs(Axis(2).AType) = 2 Then 'logarithmic axis
      Call LgAxis(Graph, XLen, ymlen, Axis(2).label, Axis(2).minv, Axis(2).maxv, 2, rmax1, Axis(2).TLen, rGridy)
    ElseIf Axis(1).AType > 0 Then 'put same tics as on left y
      Call RhtBdr(Graph, Abs(Axis(1).AType), XLen, ymlen, Axis(1).TLen, Axis(1).NTic, Axis(1).minv, Axis(1).maxv)
    End If
    
    'X axis
    If Axis(4).AType = 0 Then 'time axis
      If Axis(4).SType = 0 Then 'unknown subtype
        Dim ConstInt As Boolean, i As Long
        'assume constant interval
        ConstInt = True
        For i = 0 To ncrv - 1
          If Crv(i).CType = 5 Then 'not constant interval
            ConstInt = False
          End If
        Next i
        Call TmAxisCalc(Graph, XLen, ymlen, auxlen, Axis(4).label, Axis(4).TLen, Gridx, ncrv, TSTEP, TUnits, sdatim, edatim, ConstInt, Axis(4).SType, Axis(4).STypeInt)
      End If
      
      'determine if any time series are point
      tstype = 1
      For i = 0 To ncrv - 1
        If dtype(i) = 2 Then tstype = 2
      Next i
      
      If Axis(4).SType = 5 Then  'NPTS must be number of months not n-month time steps
        Call timdif(sdatim, edatim, Axis(4).SType, 1, npts)
      Else
        Call timdif(sdatim, edatim, Axis(4).SType, Axis(4).STypeInt, npts)
      End If
      
      Call timdif(sdatim, edatim, 4, 1, ndays)
      'If sdatim(2) = 1 And sdatim(3) = 24 Then ndays = ndays + 1
      If edatim(3) > 0 And edatim(3) < 24 Then 'some hour to plot on last day
        ndays = ndays + 1
      End If

      Call CopyI(6, sdatim, ldt) 'needed because changes in next routine
      If npts > 0 Then Call TmAxis(Graph, XLen, ymlen, auxlen, ldt, Axis(4).SType, npts, ndays, tstype, Axis(4).STypeInt, Gridx)
      
    ElseIf Axis(4).AType = 1 Then 'arithmetic axis
      Call AxAxis(Graph, XLen, ymlen, Axis(4).label, Axis(4).minv, Axis(4).maxv, Axis(4).TLen, Axis(4).NTic, Gridx)
    ElseIf Abs(Axis(4).AType) = 2 Then 'logarithmic axis
      Call LxAxis(Graph, XLen, ymlen, Axis(4).label, Axis(4).minv, Axis(4).maxv, Axis(4).TLen, Gridx)
    ElseIf Axis(4).AType >= 3 Then 'probability axis
      stdev = Abs(Axis(4).minv)
      If Axis(4).maxv > stdev Then stdev = Axis(4).maxv
      Call PbAxis(Graph, XLen, ymlen, Axis(4).AType - 2, Axis(4).label, Axis(4).TLen, Axis(4).minv, Axis(4).maxv, stdev, Gridx)
    ElseIf Axis(4).AType = -1 Then 'boxplot axis
      Call AxAxis(Graph, XLen, ymlen, Axis(4).label, Axis(4).minv, Axis(4).maxv, Axis(4).TLen, Axis(4).NTic, -1)
      Graph.Line (0, 0)-(XLen, 0), Graph.ForeColor
    End If
Exit Sub
errorhandler9:   ' Error handler line label.
 Call DispError("DrwAxis", Err)
 Resume Next ' Resume procedure.

End Sub

Public Sub ReDrawGraph(outflag As Long)

    'this is the guts of drawing any graph
    Dim fp As Long, tp As Long
    Dim i As Long, retcod As Long
    Dim lstr As String
    Dim numcol As Long, clr As Long

On Error GoTo ErrorHandler3
    'set output object
    If outflag <= 0 Then
      'output to screen
      Set Graph = scrGraph
      'clear the graph
      Graph.Cls
    Else
      'output to printer
      'CommonDialog1.ShowPrinter
      'allow landscape
      fp = 1
      tp = 1
      Call ShowPrinterX(Me, fp, tp, 1, 1, PD_NOSELECTION + PD_NOPAGENUMS + PD_DISABLEPRINTTOFILE)
      If tp = -1 Then Exit Sub 'cancel selected for printer

      'see MS KB Q175535 (fix fails!)
      'create a printer device context
      'GraphHDC = CreateDC(Printer.DriverName, Printer.DeviceName, CLng(0), CLng(0))
      
      'di.cbSize = 20
      'result = StartDoc(GraphHDC, di)
      'result = StartPage(GraphHDC)
      'Graph.ForeColor = 0
      
      Set Graph = Printer
      numcol = GetDeviceCaps(Graph.hdc, 24)
      If numcol > 2 Or numcol = -1 Then
        'printer has color capability
        outflag = 2
      Else
        i = MsgBox("If printer has only black and white capability," & vbCrLf & "be sure line types are unique to identify each curve.", vbOKCancel + vbQuestion)
        If i = vbCancel Then 'don't do print
          Exit Sub
        End If
      End If
      Printer.FontName = scrGraph.FontName
      Printer.FontBold = scrGraph.FontBold
      Printer.FontItalic = scrGraph.FontItalic
      Printer.FontSize = scrGraph.FontSize
      Printer.FontUnderline = scrGraph.FontUnderline
'On Error GoTo CouldNotSetFont
'      Set Printer.Font = scrGraph.Font
'On Error GoTo ErrorHandler3
    End If
    'scale graph
    Call ReScaleGraph(retcod)
    If retcod <> 0 Then GoTo ErrorHandler3
    LogPixelsY = GetDeviceCaps(Graph.hdc, 90)
    LogPixelsX = GetDeviceCaps(Graph.hdc, 88)
    HorzRes = GetDeviceCaps(Graph.hdc, 8)
    VertRes = GetDeviceCaps(Graph.hdc, 10)
    AspectX = GetDeviceCaps(Graph.hdc, 40)
    AspectY = GetDeviceCaps(Graph.hdc, 42)
    HorzSize = GetDeviceCaps(Graph.hdc, 4)
    VertSize = GetDeviceCaps(Graph.hdc, 6)
    
    If outflag > 0 Then 'kludge printer
      LogPixelsY = LogPixelsY * 0.95
      LogPixelsX = LogPixelsX * 0.95
    End If
    SetLogFont scrGraph.Font
    'Debug.Print LogPixelsY, VertRes, AspectY, VertSize, graph.Height
    'Debug.Print LogPixelsX, HorzRes, AspectX, HorzSize, graph.Width

    'make axes log adjustments if needed
    If Axis(1).AType = 2 Then
      Axis(1).minv = Fix(1.01 * Log10(CDbl(Axis(1).minv)))
      Axis(1).maxv = Fix(1.01 * Log10(CDbl(Axis(1).maxv)))
      'indicate transformation already done
      Axis(1).AType = -2
    End If
    If Axis(2).AType = 2 Then
      Axis(2).minv = Fix(1.01 * Log10(CDbl(Axis(2).minv)))
      Axis(2).maxv = Fix(1.01 * Log10(CDbl(Axis(2).maxv)))
      'indicate transformation already done
      Axis(2).AType = -2
    End If
    If Axis(4).AType = 2 Then
      Axis(4).minv = Fix(1.01 * Log10(CDbl(Axis(4).minv)))
      Axis(4).maxv = Fix(1.01 * Log10(CDbl(Axis(4).maxv)))
      'indicate transformation already done
      Axis(4).AType = -2
    End If
        
    If Crv(0).CType = 10 Then
      'draw boxplot
      Call DrawBoxPlot
    Else
      'draw the curves
      Call DrwLines
    End If
    
    'title
    Call DrwTitle
    'axes numbering/tics
    'if moved up before drawing other things so grid doesn't get drawn over data, zero data overwrites axis lines
    Call DrwAxis
    If outflag = 0 Then
      'show it on screen
'      Show
'      Graph.SetFocus
'      ReSize = False
    ElseIf outflag > 0 Then
      'terminate printer output
      Graph.EndDoc
    End If
Exit Sub
ErrorHandler3:   ' Error handler line label.
 Call DispError("ReDrawGraph", Err)
Exit Sub

CouldNotSetFont:
  MsgBox "Could not set printer font", vbOKOnly, "ReDrawGraph"
  Err.Clear
  Resume Next
End Sub

'Call this before calling CreateFontIndirect lf
Private Sub SetLogFont(f As StdFont)
  Dim i As Long, lstr As String
  lf.lfOutPrecision = 0
  lf.lfCharSet = 1        'DEFAULT_CHARSET
  lf.lfClipPrecision = 16 'CLIP_LH_ANGLES
  lf.lfQuality = 2        'PROOF_QUALITY
  lf.lfPitchAndFamily = 4 'TRUETYPE_FONTTYPE
  lf.lfOrientation = 0
  lf.lfEscapement = 0

  lstr = f.Name & Chr$(0)
  For i = 0 To Len(lstr) - 1
    lf.lfFaceName(i) = AscB(Mid(lstr, i + 1, 1))
  Next i
  lf.lfHeight = f.size * Abs(LogPixelsY) / 72
  If f.Bold Then lf.lfWeight = 700 Else lf.lfWeight = 400
  If f.Italic Then lf.lfItalic = 1 Else lf.lfItalic = 0
  lf.lfCharSet = f.Charset
  If f.Underline Then lf.lfUnderline = 1 Else lf.lfUnderline = 0
End Sub

Private Sub DrwLines()

  'draw curves for a plot
  Dim i As Long, j As Long, k As Long
  Dim js As Double, je As Double, jt As Double, jratio As Double, tXinc As Double
  Dim ipos As Long, EDate As Long, lLen As Long
  Dim pos As Long, clr As Long, PtCnt As Long, kx As Long, ky As Long
  Dim xaxtyp As Long
  Dim meanfg As Boolean
  Dim ilgnd As Long
  Dim ypos As Long, xpos As Long
  Dim minypos As Long, maxypos As Long, minxpos As Long
  Dim rtmp As Single
  Dim lextnt As Single
  Dim xinc As Single, yinc As Single
  Dim xp As Single, yp As Single
  Dim xmin As Single, xmax As Single
  Dim ymin As Single, ymax As Single
  Dim yylen As Single, base As Single
  Dim xtmp As Single, ytmp As Single
  Dim slope As Single, txthgt As Single
  Dim lstr As String
  
  'Variables for clipping to the right part of the graph
  Dim hClipRegion As Long 'New device handle with clipping
  Dim hDCsave As Long 'Save the device handle without clipping
  'Corners of clip rectangle in Windows API coordinates
  Dim ClipX1 As Long, ClipY1 As Long
  Dim ClipX2 As Long, ClipY2 As Long

On Error GoTo errorhandler10
  txthgt = Abs(Graph.TextHeight("X"))
  Graph.DrawWidth = 1
  DrwLegend "resetcount", 0, 0, 0, 0
  For i = 0 To ncrv - 1
    'assign variables for this curve
    ky = wchvr(1, i)
    kx = wchvr(2, i)
    minxpos = bufpos(1, kx)
    minypos = bufpos(1, ky)
    maxypos = bufpos(2, ky)
    If Crv(i).CType < 6 Then 'time X-axis
      xaxtyp = 0
      js = Date2J(sdatim)
      je = Date2J(edatim)
      If je > js Then
        If Crv(i).CType < 5 Then 'constant interval
          Dim nYvals As Long    'Number of y-values available for this curve
          Dim nIntWide As Long  'Number of full intervals of space available in graph
          Dim tdatim(6) As Long 'time of end of last interval there is space for
          nYvals = maxypos - minypos + 1
          timdif sdatim, edatim, TUnits(i), TSTEP(i), nIntWide
          TIMADD sdatim, TUnits(i), TSTEP(i), nIntWide, tdatim
          jt = Date2J(tdatim)
          jratio = (jt - js) / (je - js)
          tXinc = XLen * jratio / nYvals
          'xinc is inches per interval
          xinc = tXinc
          Graph.CurrentX = xinc
        Else 'not constant interval data
          'xinc is inches per julian day
          xinc = XLen / (je - js)
          Graph.CurrentX = 0
        End If
      End If
      xtmp = 0
    Else 'numeric X-axis
      With Axis(Var(kx).WchAx)
        xmin = .minv
        xmax = .maxv
        xaxtyp = .AType
      End With
      xinc = XLen / (xmax - xmin)
      If Abs(xaxtyp) = 2 Then 'transform to log
        Graph.CurrentX = (Log10(CDbl(yx(minxpos))) - xmin) * xinc
      Else
        Graph.CurrentX = (yx(minxpos) - xmin) * xinc
      End If
    End If
    ymin = Axis(Var(ky).WchAx).minv
    ymax = Axis(Var(ky).WchAx).maxv
    If ymax <> ymin Then
      If Var(ky).WchAx = 3 Then  'aux axis line
        yylen = ALen * YLen
        base = YLen - yylen
      Else  'main plot line
        If ALen > 0 Then  'aux axis in use
          yylen = YLen - (ALen * YLen) - Abs(Graph.TextHeight("X"))
        Else  'no aux axis
          yylen = YLen
        End If
        base = 0
      End If
      
      'Set clipping region so graph lines do not escape graphing area
      ClipX1 = (0 - Graph.ScaleLeft) / Graph.ScaleWidth * Graph.Width / Screen.TwipsPerPixelX
      ClipY1 = ((yylen + base) - Graph.ScaleTop) / Graph.ScaleHeight * Graph.Height / Screen.TwipsPerPixelY
      ClipX2 = ClipX1 + XLen * Graph.Width / Graph.ScaleWidth / Screen.TwipsPerPixelX
      ClipY2 = (base - Graph.ScaleTop) * Graph.Height / Graph.ScaleHeight / Screen.TwipsPerPixelY
      hClipRegion = CreateRectRgn(ClipX1, ClipY1, ClipX2, ClipY2)
      hDCsave = SaveDC(Graph.hdc)
      SelectClipRgn Graph.hdc, hClipRegion
      
      yinc = yylen / (ymax - ymin)
      'where is first point?
      If Var(ky).Trans = 1 Then
        Graph.CurrentY = (yx(minypos) - ymin) * yinc + base
      Else
        Graph.CurrentY = (Log10(CDbl(yx(minypos))) - ymin) * yinc + base
      End If
      If xaxtyp = 0 And dtype(i) = 1 Then
        meanfg = True 'plot as mean data
      Else
        meanfg = False 'plot as point data
      End If
      'indicate curves w/color
      clr = Crv(i).Color
      'If Crv(i).LType = 0 Then
        'only set thickness for solid lines
        Graph.DrawWidth = Crv(i).LThck
      'End If
      If Crv(i).LType >= 0 Then Graph.DrawStyle = (Crv(i).LType Mod 5)
      If Crv(i).SType > 0 Then Crv(i).SType = ((Crv(i).SType - 1) Mod 5) + 1
      PtCnt = maxypos - minypos + 1
      pos = 0
      pos = pos + 1
      For ypos = minypos To maxypos
        xpos = minxpos + ypos - minypos
        If xaxtyp = 0 Then 'time X-axis
          If Crv(i).CType < 5 Then 'constant interval
            xp = (ypos - minypos + 1) * xinc
          Else 'not constant interval data
            xp = (yx(xpos) - js) * xinc
          End If
        ElseIf Abs(xaxtyp) = 2 Then 'transform to log
          xp = (Log10(CDbl(yx(xpos))) - xmin) * xinc
        Else 'numeric X-axis
          xp = (yx(xpos) - xmin) * xinc
        End If
        If Var(ky).Trans = 1 Then 'arithmetic Y
          yp = (yx(ypos) - ymin) * yinc + base
        Else 'logarithmic Y
          If yx(ypos) <> 0 Then
            yp = (Log10(CDbl(yx(ypos))) - ymin) * yinc + base
          Else
            yp = 0 'make sure first point comes from x axis
          End If
        End If
        If Crv(i).LType >= 0 Then 'plot lines
          If meanfg Then
            'plot mean over interval
            Graph.Line -(xtmp, yp), clr
            Graph.Line (xtmp, yp)-(xp, yp), clr
          Else
            Graph.Line -(xp, yp), clr
          End If
        End If
        If Crv(i).SType > 0 Then 'plot symbols
          If meanfg Then
            'plot mean over interval
            Call GrMark(Graph, xtmp, yp, Crv(i).SType, clr)
          End If
          Call GrMark(Graph, xp, yp, Crv(i).SType, clr)
        End If
        If DataLabelPosition > 0 And nDataLabels > 0 Then
          If ypos >= LBound(dataLabels) And ypos <= UBound(dataLabels) Then
            Dim SaveX As Single, SaveY As Single
            SaveX = Graph.CurrentX: SaveY = Graph.CurrentY
            Select Case DataLabelPosition
            Case 1: GraphPrint dataLabels(ypos)
            Case 2: Graph.CurrentX = Graph.CurrentX - txthgt / 2
                    Graph.CurrentY = Graph.CurrentY + txthgt / 2
                    GraphPrint dataLabels(ypos), 90
            End Select
            Graph.CurrentX = SaveX: Graph.CurrentY = SaveY
          End If
        End If
        'save current points
        xtmp = xp
        ytmp = yp
        pos = pos + 1
      Next ypos
      
      'un-set clipping region
      RestoreDC Graph.hdc, hDCsave
      DeleteObject hClipRegion

    End If
    If Var(ky).WchAx < 3 And Len(Crv(i).LegLbl) > 0 Then DrwLegend Crv(i).LegLbl, Crv(i).LType, Crv(i).SType, clr, yylen
    Graph.DrawWidth = 1
  Next i
  DrwMathLines
  If Len(XtraText) > 0 Then
    XTxtMin = XTxtLoc * XLen
    YTxtMin = YTxtLoc * YLen
    XTxtMax = XTxtMin
    YTxtMax = YTxtMin
    Graph.CurrentY = YTxtLoc * YLen
    lstr = XtraText
    Do
      Graph.CurrentX = XTxtLoc * XLen
      ipos = InStr(lstr, "&")
      If ipos > 0 Then
        GraphPrint Left(lstr, ipos - 1)
        lstr = Mid(lstr, ipos + 1)
      Else
        GraphPrint lstr
      End If
      If Graph.CurrentX > XTxtMax Then XTxtMax = Graph.CurrentX
      Graph.Print
    Loop While ipos > 0
    If Graph.CurrentY < YTxtMin Then YTxtMin = Graph.CurrentY
    If Graph.CurrentY > YTxtMax Then YTxtMax = Graph.CurrentY
  End If
  Graph.DrawStyle = 0
  Exit Sub
  
errorhandler10:   ' Error handler line label.
 Call DispError("DrwLines", Err)
 Resume Next ' Resume procedure.
 
End Sub

'Private Function ypos(yval As Double, axisNum As Long) As Single
'  Dim yylen As Single, base As Single, val As Single, WordHeight As Single, axMin As Single, axMax As Single, inc As Single
'  WordHeight = Abs(Graph.TextHeight("X"))
'  val = yval
'  If axisNum = 3 Then  'aux axis line
'    yylen = ALen * YLen
'    base = YLen - yylen
'  Else  'main plot line
'    If ALen > 0 Then  'aux axis in use
'      yylen = YLen - (ALen * YLen) - WordHeight
'    Else  'no aux axis
'      yylen = YLen
'    End If
'    base = 0
'  End If
'  axMin = Axis(axisNum).minv
'  axMax = Axis(axisNum).maxv
'  If axMax - axMin < 0.001 Then
'    axMin = 0
'    axMax = yylen
'    inc = yylen
'  Else
'    inc = yylen / (axMax - axMin)
'  End If
'
'  Select Case Axis(axisNum).AType '0TM,1AR,2LG,3-8PRB
'    Case 2, -2:    val = (Log10(CDbl(val)) - axMin) * inc 'logarithmic axis
'    Case Else:     val = (val - axMin) * inc
'  End Select
'
'  If axisNum = 3 Then val = val + base
'  ypos = val
'End Function

Private Sub DrwMathLines()
  Dim lnum As Long, val As Single
  Dim Value As String, tmp As String
  Dim axisNum As Long, axMax As Single, axMin As Single, inc As Single
  Dim yylen As Single, base As Single, WordHeight As Single
  WordHeight = Abs(Graph.TextHeight("X"))
  
  For lnum = 1 To nLine ' ncrv To ncrv + nLine - 1
    With Lines(lnum)
      Value = LCase(Trim(.Value))
      If .LType = 0 Then    'thickness only works for solid lines
        Graph.DrawWidth = .LThck
      Else
        Graph.DrawWidth = 1
      End If
      If .LType >= 0 Then Graph.DrawStyle = (.LType Mod 5)
      axisNum = .WchAx
      If axisNum = 3 Then  'aux axis line
        yylen = ALen * YLen
        base = YLen - yylen
      Else  'main plot line
        If ALen > 0 Then  'aux axis in use
          yylen = YLen - (ALen * YLen) - WordHeight
        Else  'no aux axis
          yylen = YLen
        End If
        base = 0
      End If
      axMin = Axis(axisNum).minv
      axMax = Axis(axisNum).maxv
      If axMax - axMin < 0.001 Then
        axMin = 0
        If axisNum = 4 Then
          axMax = XLen
          inc = XLen
        Else
          axMax = yylen
          inc = yylen
        End If
      Else
        inc = yylen / (axMax - axMin)
      End If

      If IsNumeric(Value) Then
        val = Value
        Select Case Axis(axisNum).AType '0TM,1AR,2LG,3-8PRB
          Case 2, -2:    val = (Log10(CDbl(val)) - axMin) * inc 'logarithmic axis
          Case Else: val = (val - axMin) * inc
        End Select
        
        If .LType >= 0 Then
          If axisNum = 4 Then
            If val >= 0 And val <= XLen Then Graph.Line (val, 0)-(val, base + yylen), .Color
          ElseIf axisNum = 1 Or axisNum = 2 Then
            If val >= base And val <= yylen + base Then Graph.Line (0, val)-(XLen, val), .Color
          ElseIf axisNum = 3 Then
            Graph.Line (0, val + base)-(XLen, val + base), .Color
          End If
        End If
      End If
      If axisNum = 3 Then yylen = YLen - (ALen * YLen) - WordHeight
      DrwLegend .LegLbl, .LType, .SType, .Color, yylen
    End With
NextLine:
  Next lnum
  Graph.DrawWidth = 1
  Graph.DrawStyle = 0
End Sub

'Private Sub DrwConstX(xmin As Single, xmax As Single, xinc As Single, yylen As Single, base As Single)
'  Dim xp As Single
'  If ConstXfg = 1 Then
'    If ConstX >= xmin And ConstX <= xmax Then
'      If Axis(4).AType = 1 Then 'arithmetic X
'        xp = (ConstX - xmin) * xinc
'      Else 'logarithmic X
'        xp = (Log10(CDbl(ConstX)) - xmin) * xinc
'      End If
'      Graph.Line (xp, 0)-(xp, base + yylen)
'    End If
'  End If
'End Sub

'Private Sub DrwConstY(ymin As Single, ymax As Single, yinc As Single)
'  Dim yp As Single
'  If ConstYfg = 1 Then 'draw constant y line
'    If ConstY >= ymin And ConstY <= ymax Then
'      If Axis(1).AType = 1 Then 'arithmetic Y
'        yp = (ConstY - ymin) * yinc '+ base
'      Else 'logarithmic Y
'        yp = (Log10(CDbl(ConstY)) - ymin) * yinc '+ base
'      End If
'      Graph.Line (0, yp)-(XLen, yp)
'    End If
'  End If
'End Sub

Private Sub DrwLegend(LegLbl As String, LType As Long, SType As Long, clr As Long, yylen)
  Static ilgnd As Long
  Dim xp As Single, yp As Single, lstr As String, tclr As Long
  If LegLbl = "resetcount" Then
    ilgnd = 0
    If XLegLoc = NONE Then XLegMin = 0.05 * XLen Else XLegMin = XLegLoc * XLen
    YLegMin = 999999999#
    XLegMax = 0
    YLegMax = -999999999#
  ElseIf ncrv > 1 And XLegLoc > -2# And lenStr(LegLbl) > 0 Then
    lstr = Trim(LegLbl)
    If YLegLoc <> NONE Then 'user value
      yp = YLegLoc * yylen - (ilgnd + 0.25) * -Graph.TextHeight(lstr) - 1.25 * Axis(3).TLen
    Else 'default
      yp = yylen - (ilgnd + 0.25) * -Graph.TextHeight(lstr) - 1.25 * Axis(3).TLen
    End If
    ilgnd = ilgnd + 1
    Graph.CurrentY = yp
    Graph.CurrentX = XLegMin
    xp = Graph.CurrentX
    If LType >= 0 Then
      Graph.Line -(xp + 0.05 * XLen, yp), clr
    End If
    If SType > 0 Then
      Call GrMark(Graph, xp, yp, SType, clr)
      xp = xp + 0.05 * XLen
      Call GrMark(Graph, xp, yp, SType, clr)
    End If
    Graph.CurrentX = Graph.CurrentX + Graph.TextWidth(" ")
    Graph.CurrentY = yp + 0.4 * Abs(Graph.TextHeight(lstr))
'        lextnt = Graph.CurrentX + Graph.TextWidth(lstr)
'        If lextnt > XLen Then
'          'legend label exceeds right y axis
'          llen = LenStr(lstr)
'          While lextnt > XLen
'            llen = llen - 1
'            lstr = Left(lstr, llen)
'            lextnt = Graph.CurrentX + Graph.TextWidth(lstr)
'          Wend
'        End If
    tclr = Graph.ForeColor
    Graph.ForeColor = clr
    GraphPrint lstr
    Graph.ForeColor = tclr
    If Graph.CurrentX > XLegMax Then XLegMax = Graph.CurrentX
    If Graph.CurrentY > YLegMax Then YLegMax = Graph.CurrentY ': Debug.Print "New YLegMax"
    Graph.Print
    If Graph.CurrentY < YLegMin Then YLegMin = Graph.CurrentY ': Debug.Print "New YLegMin"
    
    'If ilgnd >= ncrv Then Graph.Line (XLegMin, YLegMin)-(XLegMax, YLegMax), vbRed, B
  End If
End Sub

Private Sub ReScaleGraph(retcod As Long)

    'reset scale based on data being plotted
On Error GoTo errorhandler5
    'local version units are world coord
    Dim RmrgX As Single, TmrgY As Single, LmrgX As Single, BmrgY As Single, i As Long
    'Dim wid As Single, hei As Single
    'set scale mode to inches
    Graph.ScaleMode = 5
'    '720 is 1/2 inch in twips (old method)
    'set margin sizes
    If Abs(Axis(2).AType) > 0 Then  'using right axis
      RmrgX = 0.75
    Else
      RmrgX = 0.5
    End If
    LmrgX = 0.75
    TmrgY = 0.5
    BmrgY = 1#
    'wid = Graph.Width
    'hei = Graph.Height
    'tics are 1/2 of text height
    For i = 1 To 4
      Axis(i).TLen = Graph.TextHeight("") / 2
    Next i
    XLen = Graph.ScaleWidth - LmrgX - RmrgX
    YLen = Graph.ScaleHeight - BmrgY - TmrgY
    If XLen <= 0 Or YLen <= 0 Then GoTo errorhandler5
    Graph.Scale (-LmrgX, YLen + TmrgY)-(XLen + RmrgX, -BmrgY)
    If Graph.ScaleTop <> YLen + TmrgY Then 'VB6 bug workaround
      Graph.ScaleTop = YLen + TmrgY
      Graph.ScaleHeight = -BmrgY - Graph.ScaleTop
    End If
    retcod = 0
    
Exit Sub
errorhandler5:   ' Error handler line label.
 Call DispError("ReScaleGraph", Err)
retcod = 1

End Sub

Private Sub BlankTitle()
  Dim tpos As Long, tp2 As Long, lwid As Single, lhgt As Single, lstr As String
  lhgt = Graph.TextHeight(Title)
  scrGraph.Line (0, 2.5 * lhgt)-(scrGraph.ScaleWidth, -1), vbWhite, BF

End Sub

Private Sub DrwTitle()
  'put title on the plot
  Dim tpos As Long, tp2 As Long, lwid As Single, lhgt As Single, lstr As String
  'where the title is printed
  lhgt = Graph.TextHeight(Title)
  Graph.CurrentY = 2.5 * lhgt
  tpos = InStr(Title, "&")
  If tpos > 0 Then
    lstr = Left(Title, tpos - 1)
    lwid = Graph.TextWidth(lstr)
    Graph.CurrentX = (XLen - lwid) / 2#
    GraphPrint lstr
    tp2 = InStr(tpos + 1, Title, "&")
    If tp2 > 0 Then
      lstr = Mid(Title, tpos + 1, tp2 - tpos - 1)
      lwid = Graph.TextWidth(lstr)
      Graph.CurrentX = (XLen - lwid) / 2#
      Graph.CurrentY = 3.3 * lhgt
      GraphPrint lstr
      lstr = Mid(Title, tp2 + 1)
      lwid = Graph.TextWidth(lstr)
      Graph.CurrentX = (XLen - lwid) / 2#
      Graph.CurrentY = 4.1 * lhgt
      GraphPrint lstr
    Else
      lstr = Mid(Title, tpos + 1)
      lwid = Graph.TextWidth(lstr)
      Graph.CurrentX = (XLen - lwid) / 2#
      Graph.CurrentY = 3.3 * lhgt
      GraphPrint lstr
    End If
  Else
    lwid = Graph.TextWidth(Title)
    Graph.CurrentX = (XLen - lwid) / 2#
    GraphPrint Title
  End If
End Sub


Public Sub SetData(inum As Long, ipos As Long, nv As Long, arra() As Double, retcod As Long)

    'put data to be plotted in plot buffer
    Dim i As Long, k As Long, i1 As Long, i2 As Long

    If inum <= 2 * POSMAX Then 'room for more data
      bufpos(1, inum) = ipos
      bufpos(2, inum) = ipos + nv - 1
      ReDim Preserve yx(bufpos(2, inum))
      'put values in buffer
      For i = 0 To nv - 1
        k = ipos + i
        yx(k) = arra(LBound(arra) + i)
      Next i
      retcod = 0
    Else
      retcod = 1
    End If

End Sub

Public Function GetNdataLabels() As Long
  GetNdataLabels = nDataLabels
End Function

Public Function GetDataLabelPosition() As Long
  GetDataLabelPosition = DataLabelPosition
End Function

Public Sub SetDataLabelPosition(newPosition As Long)
  DataLabelPosition = newPosition
End Sub

'Used for labeling profile plots
Public Sub SetDataLabels(scrpos As Long, ipos As Long, nv As Long, arra() As String, retcod As Long)
  Dim i As Long, maxI As Long, minA As Long
  minA = LBound(arra)
  maxI = ipos + nv - 1
  If UBound(arra) - minA + 1 >= nv Then
    If nDataLabels < 1 Then
      ReDim dataLabels(ipos To maxI)
    Else
      Dim newmin As Long, newmax As Long
      newmin = LBound(dataLabels)
      newmax = UBound(dataLabels)
      If newmin > ipos Or newmax < maxI Then
        If newmin > ipos Then newmin = ipos
        If newmax < maxI Then newmax = maxI
        ReDim Preserve dataLabels(newmin To newmax)
      End If
    End If
    nDataLabels = UBound(dataLabels) - LBound(dataLabels) + 1
    For i = ipos To maxI
      dataLabels(i) = arra(minA + i - ipos)
    Next i
    If DataLabelPosition = 0 Then DataLabelPosition = 1
    retcod = 0
  Else
    retcod = 1
  End If
End Sub

Public Sub SetAxesInfo(xtype As Long, xstype As Long, Xint As Long, ytype As Long, yrtype As Long, auxlen As Single, xlab As String, ylab As String, yrlab As String, alab As String)

    'set axes types and labels
    Axis(1).AType = ytype
    Axis(2).AType = yrtype
    ALen = auxlen
    If ALen > 0 Then  'aux axis in use
      Axis(3).AType = 1
    End If
    Axis(4).AType = xtype
    Axis(4).SType = xstype
    Axis(4).STypeInt = Xint
    Axis(1).label = ylab
    Axis(2).label = yrlab
    Axis(3).label = alab
    Axis(4).label = xlab

    If xtype > 2 Or ytype > 2 Then mnuView.Enabled = False

End Sub
Public Sub GetAxesInfo(xtype As Long, xstype As Long, Xint As Long, ytype As Long, yrtype As Long, auxlen As Single, xlab As String, ylab As String, yrlab As String, alab As String)

    'get axes types and labels
    ytype = Axis(1).AType
    yrtype = Axis(2).AType
    auxlen = ALen
    xtype = Axis(4).AType
    xstype = Axis(4).SType
    Xint = Axis(4).STypeInt
    ylab = Axis(1).label
    yrlab = Axis(2).label
    alab = Axis(3).label
    xlab = Axis(4).label

End Sub

Public Sub GetCurveInfo(crvtyp() As Long, LinTyp() As Long, linthk() As Long, symtyp() As Long, iColor() As Long, ilbl() As String)

    'get curve information
    Dim i As Long

    For i = 0 To ncrv - 1
      crvtyp(i) = Crv(i).CType
      LinTyp(i) = Crv(i).LType + 1
      linthk(i) = Crv(i).LThck
      symtyp(i) = Crv(i).SType
      iColor(i) = Crv(i).Color
      ilbl(i) = Crv(i).LegLbl
    Next i

End Sub

Public Sub SetCurveInfo(crvtyp() As Long, LinTyp() As Long, linthk() As Long, symtyp() As Long, iColor() As Long, ilbl() As String)

    'set curve information
    Dim i As Long

    mnuCoordinateCurve(0).Visible = True
    For i = 0 To ncrv - 1 '+ nLine
      If ncrv > 1 And crvtyp(i) > 5 Then mnuView.Enabled = False
      Crv(i).CType = crvtyp(i)
      Crv(i).LType = LinTyp(i) - 1
      Crv(i).LThck = linthk(i)
      Crv(i).SType = symtyp(i)
      Crv(i).Color = iColor(i)
      If Crv(i).Color > 0 And Crv(i).Color < 16 Then Crv(i).Color = QBColor(Crv(i).Color)
      Crv(i).LegLbl = ilbl(i)
      If mnuCoordinateCurve.UBound <= i Then Load mnuCoordinateCurve(i + 1)
      mnuCoordinateCurve(i + 1).Caption = "&" & i + 1 & " " & Crv(i).LegLbl
    Next i
    mnuCoordinateCurve(1).Checked = True
End Sub

Public Sub GetNumVars(icrv As Long, ivar As Long)

    'get the number of curves and variables
    icrv = ncrv
    ivar = Nvar

End Sub

Public Sub SetNumVars(icrv As Long, ivar As Long)

    'set the number of curves and variables
    If icrv <= POSMAX And ivar <= 2 * POSMAX Then
      ncrv = icrv
      Nvar = ivar
    Else
      MsgBox "SetNumVars: Too many curves (" & icrv & ") or variables (" & ivar & ")." & vbCrLf & "Max for curves is " & POSMAX & "." & vbCrLf & "Max for variables is " & 2 * POSMAX & ".", 48, "HassGraph Problem"
    End If

End Sub

Public Property Get NumVars() As Long
  NumVars = Nvar
End Property

Public Property Let NumVars(newvalue As Long)
  If newvalue <= 2 * POSMAX Then Nvar = newvalue
End Property

Public Property Get NumCurves() As Long
  NumCurves = ncrv
End Property

Public Property Let NumCurves(newvalue As Long)
  If newvalue <= POSMAX Then ncrv = newvalue
End Property

Public Property Get NumLines() As Long
  NumLines = nLine
End Property

Public Property Let NumLines(newvalue As Long)
  nLine = newvalue
  ReDim Preserve Lines(nLine)
End Property

Public Sub GetScale(plmn() As Single, plmx() As Single, NTICS() As Long)
    'get min, max, and number of tics for axes
    Dim i As Long

    For i = 1 To 4
      plmn(i) = Axis(i).minv
      plmx(i) = Axis(i).maxv
      NTICS(i) = Axis(i).NTic
    Next i

End Sub
Public Sub SetScale(plmn() As Single, plmx() As Single, NTICS() As Long)
    'set min, max, and number of tics for axes
    Dim i As Long

    For i = 1 To 4
      Axis(i).minv = plmn(LBound(plmn) + i - 1)
      Axis(i).maxv = plmx(LBound(plmx) + i - 1)
      Axis(i).NTic = NTICS(LBound(NTICS) + i - 1)
    Next i

End Sub

Public Sub ShowIt(Optional modal As Boolean = False)
  pModal = modal
  'generate current graph
  Set curG = Me
'    ReSize = True
  scrGraph.FontBold = False
  If modal Then
    curG.Show 1
  Else
    curG.Show
  End If
End Sub

'Public Sub init(atcgraf As ATCoGraph)
'  MsgBox "frmG:init"
'End Sub

Public Sub GetVars(curve As Long, yvar As Long, xvar As Long)

    'store which variables to use for each curve
    If curve <= POSMAX Then 'room for more curves
      yvar = wchvr(1, curve)
      xvar = wchvr(2, curve)
    End If

End Sub
Public Sub SetVars(curve As Long, yvar As Long, xvar As Long)

    'store which variables to use for each curve
    If curve <= POSMAX Then 'room for more curves
      wchvr(1, curve) = yvar
      wchvr(2, curve) = xvar
    End If

End Sub

Public Sub GetTime(gtstep() As Long, gtunit() As Long, gsdate() As Long, gedate() As Long, gdtype() As Long)

    Dim i As Long
    
    For i = 0 To ncrv - 1 Step 1
      gtstep(i) = TSTEP(i)
      gtunit(i) = TUnits(i)
      gdtype(i) = dtype(i)
    Next i
    For i = 0 To 5 Step 1
      gsdate(i) = sdatim(i)
      gedate(i) = edatim(i)
    Next i

End Sub

Public Sub SetTime(gtstep() As Long, gtunit() As Long, gsdate() As Long, gedate() As Long, gdtype() As Long)

    Dim i As Long
    
    For i = 0 To ncrv - 1 Step 1
      TSTEP(i) = gtstep(LBound(gtstep) + i)
      TUnits(i) = gtunit(LBound(gtunit) + i)
      dtype(i) = gdtype(LBound(gdtype) + i)
    Next i
    For i = 0 To 5 Step 1
      sdatim(i) = gsdate(i)
      edatim(i) = gedate(i)
    Next i

End Sub

Public Sub SetCurDrvDir(V As String, d As String)
    Dim e As Long
    
    On Error GoTo eh
    
    If Len(V) > 0 Then
      e = 1
      ChDrive V
    End If
    If Len(d) > 0 Then
      e = 2
      ChDir d
    End If
    Exit Sub
    
eh:
    On Error GoTo 0
    If e = 1 Then
      MsgBox "SetCurDrvDir:bad drive: " & V, 48, "HassGraph Problem"
    ElseIf e = 2 Then
      MsgBox "SetCurDrvDir:bad directory: " & d, 48, "HassGraph Problem"
    End If
End Sub

Private Sub mnuSaveImage_Click()
  Set frmGSave.IPC = pIPC
  Set frmGSave.scrGraph = scrGraph
  frmGSave.Show
'  Static LastSaveFilename As String
'  Dim SaveFile As String
'  If Not (pIPC Is Nothing) Then
'    SaveFile = pIPC.SavePictureDialog(LastSaveFilename, "+Windows Metafile (*.wmf)|*.wmf")
'    If LCase(Right(SaveFile, 3)) = "wmf" Then
'      SavePicture scrGraph.Image, SaveFile
'    Else
'      pIPC.SavePictureAs scrGraph, SaveFile
'    End If
'  End If
End Sub

Private Sub mnuSaveSpec_Click()
  Set frmGSave.IPC = pIPC
  frmGSave.Show
End Sub

Private Sub mnuTransparent_Click()
  mnuTransparent.Checked = Not mnuTransparent.Checked
  If mnuTransparent.Checked Then
    SetColorTransparent Me.hwnd, vbWhite
  Else
    UnsetTransparent Me.hwnd
  End If
End Sub

Private Sub ScrGraph_Click()
  Dim PopupWasVisible As Boolean
  DbgMsg "ScrGraph_Click"
  PopupWasVisible = txtPopup.Visible
  If PopupWasVisible Then txtPopup_CommitChange
  If lastX1 <> lastX2 Or lastY1 <> lastY2 Or showingCrosshairs Then
    XorLines
  End If
  If draggingAxis < 0 Then
    'Stop
    draggingAxis = -draggingAxis
  End If
  If scrGraph.MousePointer = vbIbeam Then
    If Not PopupWasVisible Then EditTextOnGraph
  ElseIf draggingAxis > 0 Then
    Dim X1 As Single, X2 As Single, Y1 As Single, Y2 As Single, tmp As Single
    X1 = SnapToTic(Wc2Val(scrGraph, 4, lastX1), 4)
    X2 = SnapToTic(Wc2Val(scrGraph, 4, lastX2), 4)
    If X1 > X2 Then
      tmp = X1
      X1 = X2
      X2 = tmp
    End If
    Y1 = SnapToTic(Wc2Val(scrGraph, 1, lastY1), 1) 'What should we do with multiple Y axes?
    Y2 = SnapToTic(Wc2Val(scrGraph, 1, lastY2), 1)
    If Y1 > Y2 Then
      tmp = Y1
      Y1 = Y2
      Y2 = tmp
    End If
    
    If draggingAxis = 1 Then
      If X1 < Axis(4).maxv And X1 <> Axis(4).minv Then Axis(4).minv = X1 Else Call mnuEdGraph_Click(0): Exit Sub
    ElseIf draggingAxis = 2 Then
      If X2 > Axis(4).minv And X1 <> Axis(4).maxv Then Axis(4).maxv = X2 Else Call mnuEdGraph_Click(0): Exit Sub
    ElseIf draggingAxis = 4 Then
      If Y1 < Axis(1).maxv And Y1 <> Axis(1).minv Then Axis(1).minv = Y1 Else Call mnuEdGraph_Click(0): Exit Sub
    ElseIf draggingAxis = 5 Then
      If Y2 > Axis(1).minv And Y2 <> Axis(1).maxv Then Axis(1).maxv = Y2 Else Call mnuEdGraph_Click(0): Exit Sub
    ElseIf draggingAxis = 8 Then
      If Sqr((XTxtLoc - lastX1 / XLen) ^ 2 + (YTxtLoc - lastY1 / YLen) ^ 2) > NearnessTolerance Then
        XTxtLoc = lastX1 / XLen
        YTxtLoc = lastY1 / YLen
      Else
        mnuEdGraph_Click (4) 'EditTextOnGraph
      End If
    ElseIf draggingAxis = 9 Then
      If Sqr((XLegLoc - lastX1 / XLen) ^ 2 + (YLegLoc - lastY1 / YLen) ^ 2) > NearnessTolerance Then
        XLegLoc = lastX1 / XLen
        YLegLoc = lastY1 / YLen
      Else
        mnuEdGraph_Click (2) 'EditTextOnGraph
      End If
    ElseIf draggingAxis > 100 And draggingAxis - 100 <= nLine Then ' constant line
      Dim newval As Single
      With Lines(draggingAxis - 100)
        If .WchAx = 4 Then
          newval = Wc2Val(scrGraph, 4, lastX1)
        Else
          newval = Wc2Val(scrGraph, .WchAx, lastY1)
        End If
        If Abs(.Value - newval) < NearnessTolerance Then
          mnuEdGraph_Click (3)
        Else
          .Value = newval
        End If
      End With
    ElseIf draggingAxis = 10 Then
' rectangle
      If X1 < X2 Then
        If X1 <> Axis(4).minv Then Axis(4).minv = X1
        If X2 <> Axis(4).maxv Then Axis(4).maxv = X2
      End If
      If Y1 < Y2 Then
        If Y1 <> Axis(1).minv Then Axis(1).minv = Y1
        If Y2 <> Axis(1).maxv Then Axis(1).maxv = Y2
      End If
      If X1 = X2 And Y1 = Y2 Then Call mnuEdGraph_Click(2): Exit Sub
          
    End If
    ReDrawGraph -1
  End If
  scrGraph.MousePointer = vbDefault
End Sub

Private Sub scrGraph_DblClick()
  Call mnuEdGraph_Click(0)
End Sub

Private Sub scrGraph_MouseDown(Button As Integer, Shift As Integer, x As Single, y As Single)
  XMouseDown = x
  YMouseDown = y
  Call scrGraph_MouseMove(Button, Shift, x, y)
End Sub
    
Private Sub scrGraph_MouseUp(Button As Integer, Shift As Integer, x As Single, y As Single)
  'Debug.Print "Up"
  'Call scrGraph_MouseMove(Button, Shift, X, Y)
End Sub

Private Sub scrGraph_MouseMove(Button As Integer, Shift As Integer, x As Single, y As Single)
  Static lastX As Single, lastY As Single
  If x = lastX And y = lastY Then Exit Sub
  
  Dim dcx As Single, dcy As Single, x0 As Single, y0 As Single, xMx As Single, yMx As Single
  Dim CoordinateText As String
  Dim TipText As String
  
  lastX = x
  lastY = y
  
  dcx = Wc2DcX(scrGraph, x)
  dcy = Wc2DcY(scrGraph, y)
  x0 = Wc2DcX(scrGraph, 0)
  y0 = Wc2DcY(scrGraph, 0)
  xMx = Wc2DcX(scrGraph, XLen)
  yMx = Wc2DcY(scrGraph, YLen)
  TipText = ""
    
  If Button Mod 2 = 1 Then
    'TipText = "(" & dcx & ", " & dcy & ")"
    'Debug.Print "drag at (" & x & ", " & y & ") of (" & XLen & ", " & YLen & ")"
    If scrGraph.MousePointer = vbIbeam Then
      Select Case draggingAxis 'Allow dragging of legend text to override editing if mouse is dragged
        Case -8, -9, 8, 9: If ((XMouseDown - x) * scrGraph.ScaleWidth) ^ 2 + ((YMouseDown - y) * scrGraph.ScaleHeight) ^ 2 > NearnessTolerance Then scrGraph.MousePointer = vbArrow
      End Select
    End If
    If draggingAxis < 0 Then draggingAxis = -draggingAxis
    If scrGraph.MousePointer <> vbDefault Then
      XorLines
      If scrGraph.MousePointer = vbSizeWE Then
        lastX1 = x
        lastY1 = 0
        lastX2 = x
        lastY2 = YLen
      ElseIf scrGraph.MousePointer = vbSizeNS Then
        lastX1 = 0
        lastY1 = y
        lastX2 = XLen
        lastY2 = y
      ElseIf scrGraph.MousePointer = vbCrosshair Then
        If Axis(4).AType = 0 Then
          lastX1 = 0
          lastX2 = XLen
        Else
          lastX2 = x
        End If
        lastY2 = y
      ElseIf scrGraph.MousePointer = vbArrow Then
        If draggingAxis = 9 Then
          lastX1 = x
          lastY1 = y
          lastX2 = x + (XLegMax - XLegMin)
          lastY2 = y - (YLegMax - YLegMin)
        ElseIf draggingAxis = 8 Then
          lastX1 = x
          lastY1 = y
          lastX2 = x + (XTxtMax - XTxtMin)
          lastY2 = y - (YTxtMax - YTxtMin)
        End If
      End If
      XorLines
    End If
  Else
    If showingCrosshairs Then XorLines
    lastX1 = 0:  lastX2 = 0:  lastY1 = 0:  lastY2 = 0 ' Stop drawing XOR line
    draggingAxis = 0
    'Debug.Print "move at (" & x & ", " & Y & ") of (" & XLen & ", " & YLen & ") pixels=(" & dcx & ", " & dcy & ") of (" & xMx & ", " & yMx & ")"
    
    Dim clickMargin As Long
    clickMargin = 3
    
    If dcy > yMx And dcy < y0 And Abs(dcx - x0) < clickMargin Then 'near left Y axis
      If Axis(4).AType = 1 Then 'Can drag if X is arithmetic
        draggingAxis = -1
        scrGraph.MousePointer = vbSizeWE
      End If
      'CoordinateText = LabelCoordinates(x, y)
      
    ElseIf dcy > yMx And dcy < y0 And Abs(xMx - dcx) < clickMargin Then 'near right Y axis
      If Axis(4).AType = 1 Then
        draggingAxis = -2
        scrGraph.MousePointer = vbSizeWE ': TipText = "Right Y-axis"
      End If
      'CoordinateText = LabelCoordinates(x, y)

    ElseIf dcx > x0 And dcx < xMx And Abs(dcy - y0) < clickMargin And Axis(1).AType = 1 Then
      draggingAxis = -4 'near bottom X axis
      scrGraph.MousePointer = vbSizeNS ': TipText = "Bottom X-axis"
      'CoordinateText = LabelCoordinates(x, y)
    
    ElseIf dcx > x0 And dcx < xMx And Abs(yMx - dcy) < clickMargin And Axis(1).AType = 1 And ALen = 0 Then
      draggingAxis = -5 'near top X axis
      scrGraph.MousePointer = vbSizeNS ': TipText = "Top X-axis"
      'CoordinateText = LabelCoordinates(x, y)
    
    Else 'not near any axis
      lastX1 = x:  lastY1 = y
      lastX2 = x:  lastY2 = y
      
      If y > YLegMin And y < YLegMax And x > XLegMin And x < XLegMax Then 'in legend
        nearest = NearestYx(x, y)
        draggingAxis = -9
        If nearest > -99 Then
          scrGraph.MousePointer = vbIbeam ': TipText = "Legend"
        Else
          scrGraph.MousePointer = vbArrow 'Moving legend
        End If
      ElseIf y > YTxtMin And y < YTxtMax And x > XTxtMin And x < XTxtMax Then 'in extra text
        scrGraph.MousePointer = vbIbeam
        draggingAxis = -8
      ElseIf dcx > x0 And dcx < xMx And dcy > y0 Then 'below graph
        scrGraph.MousePointer = vbIbeam ': TipText = "X label"
        
      ElseIf dcy > yMx And dcy < y0 And dcx < x0 Then 'left of graph
        scrGraph.MousePointer = vbIbeam ': TipText = "Y label"
        
      ElseIf dcy > yMx And dcy < y0 And dcx > xMx And Axis(2).AType > 0 Then 'right of graph
        scrGraph.MousePointer = vbIbeam ': TipText = "Y label"
        
      ElseIf dcy > yMx And dcy < y0 And dcx > x0 And dcx < xMx Then
        Dim foundX As Double, foundY As Double
        Dim nearestscrX As Single, nearestscrY As Single
        draggingAxis = -10                            'inside graph
        nearest = NearestYx(x, y, foundX, foundY, nearestscrX, nearestscrY, _
                            mnuCoordinateSnap.Checked Or mnuCoordinateSnapX.Checked Or mnuCoordinateSnapY.Checked, _
                            mnuCoordinateSnapX.Checked, _
                            mnuCoordinateSnapY.Checked)
        If nearest = NONE Then
          nearestscrX = x
          nearestscrY = y
          CoordinateText = LabelCoordinates(x, y, True)
        Else
          CoordinateText = LabelCoordinates(foundX, foundY, False)
        End If
        If mnuCoordinateLines.Checked Then
          crosshairX = nearestscrX
          crosshairY = nearestscrY
          showingCrosshairs = True
          XorLines
          showingCrosshairs = True
        End If
        If nearest = NONE Then
          scrGraph.MousePointer = vbCrosshair
        Else
          If nDataLabels > 0 And DataLabelPosition = 3 And nearest >= 0 Then
            If nearest >= LBound(dataLabels) And nearest <= UBound(dataLabels) Then
              TipText = dataLabels(nearest)
            End If
            scrGraph.MousePointer = vbCrosshair
          ElseIf nearest < -100 And nearest + 100 + nLine >= 0 Then 'near constant line
            draggingAxis = nearest
            If Lines(-(nearest + 100)).WchAx = 4 Then
              scrGraph.MousePointer = vbSizeWE ': TipText = "Constant X line"
            Else
              scrGraph.MousePointer = vbSizeNS ': TipText = "Constant Y line"
            End If
          ElseIf nearest < 0 Then 'near legend
            scrGraph.MousePointer = vbIbeam
          Else
            'CoordinateText = LabelCoordinates(foundX, foundY, False) & " Data"
            'Debug.Print TipText
            If Axis(1).AType = 1 Or Axis(4).AType = 1 Then scrGraph.MousePointer = vbCrosshair Else scrGraph.MousePointer = vbDefault
          End If
        End If
      Else
        scrGraph.MousePointer = vbDefault
      End If
    End If
  End If
  If Len(TipText) > 0 Then
    scrGraph.ToolTipText = TipText
  ElseIf mnuCoordinateTooltips.Checked And scrGraph.ToolTipText <> CoordinateText Then
    scrGraph.ToolTipText = CoordinateText
  ElseIf Len(scrGraph.ToolTipText) > 0 Then
    scrGraph.ToolTipText = ""
  End If
  
  If mnuCoordinateMenubar.Checked Then
    mnuCoordinateS.Caption = CoordinateText
  End If
  If Len(mnuCoordinateS.Caption) = 0 Then
    mnuCoordinateS.Caption = "&Coordinates"
  End If
End Sub

Private Sub XorLines()
  Dim scrDrawMode As Long
  scrDrawMode = scrGraph.DrawMode
  scrGraph.DrawMode = vbNotXorPen
  If showingCrosshairs Then
    scrGraph.Line (crosshairX, 0)-(crosshairX, YLen), vbBlack
    scrGraph.Line (0, crosshairY)-(XLen, crosshairY), vbBlack
    showingCrosshairs = False
  End If
  If lastX1 = lastX2 Or lastY1 = lastY2 Then
    scrGraph.Line (lastX1, lastY1)-(lastX2, lastY2), vbBlack
  Else
    scrGraph.Line (lastX1, lastY1)-(lastX2, lastY2), vbBlack, B
  End If
  scrGraph.DrawMode = scrDrawMode
End Sub

Private Function LabelCoordinates(ByVal x As Double, ByVal y As Double, translate As Boolean) As String
  Dim lx As String
  Dim LY As String
  Dim ly2 As String
  Dim la As String
  
  If Not translate Then
    lx = LabelTextFromValue(x, Var(wchvr(2, curveToTrack)).WchAx, translate)
    If dtype(curveToTrack) = 1 Then 'label as mean data
      lx = "Period ending " & lx
    End If
    LY = LabelTextFromValue(y, Var(wchvr(1, curveToTrack)).WchAx, translate)
  Else
    lx = LabelTextFromValue(x, 4, translate)
    LY = LabelTextFromValue(y, 1, translate)
    Select Case Axis(2).AType
      Case 1, 2, -2: ly2 = LabelTextFromValue(y, 2, translate)
    End Select
    If ALen > 0 Then
      la = LabelTextFromValue(y, 3, translate)
      If Len(la) > 0 Then LY = "": ly2 = ""
    End If
  End If
  LabelCoordinates = ""
  If Len(lx) > 0 Then LabelCoordinates = LabelCoordinates & lx
  If Len(lx) > 0 And Len(LY) > 0 Then LabelCoordinates = LabelCoordinates & ", "
  If Len(LY) > 0 Then LabelCoordinates = LabelCoordinates & LY
  If Len(ly2) > 0 Then
    If Len(LY) > 0 Then
      LabelCoordinates = LabelCoordinates & ", right = "
    ElseIf Len(lx) > 0 Then
      LabelCoordinates = LabelCoordinates & ", "
    End If
    LabelCoordinates = LabelCoordinates & ly2
  End If
  If Len(la) > 0 Then
    If Len(LabelCoordinates) > 0 Then LabelCoordinates = LabelCoordinates & ", "
    LabelCoordinates = LabelCoordinates & la
  End If
  LabelCoordinates = "(" & LabelCoordinates & ")"

End Function

'if translate = True, then we need to translate from screen coordinates before labeling
Private Function LabelTextFromValue(ByVal val As Double, ByVal axisNum As Long, ByVal translate As Boolean) As String
  Dim translatedVal As Double
  translatedVal = val
  Select Case Axis(axisNum).AType
    Case 0 'Time
      If translate Then
        Dim sday As Double
        Dim eday As Double
        sday = Date2J(sdatim())
        eday = Date2J(edatim())
        translatedVal = sday + (val / XLen) * (eday - sday)
      End If
      If translatedVal <> NONE Then
        LabelTextFromValue = JulianDateToString(translatedVal)
      End If
    Case 1, 2, -2
      If translate Then
        translatedVal = Wc2Val(scrGraph, axisNum, val, False)
      End If
      If translatedVal <> NONE Then
        Dim ndpl As Long, valinc As Single
        If Axis(axisNum).NTic > 0 Then
          valinc = (Axis(axisNum).maxv - Axis(axisNum).minv) / Axis(axisNum).NTic
          Call DecReq(Axis(axisNum).minv, valinc, Axis(axisNum).NTic, ndpl)
        End If
        If ndpl > 0 Then 'decimals needed
          Dim lbl As String
          Call F90_DECCHX((translatedVal), 12, 4, ndpl + 1, lbl)
          LabelTextFromValue = Trim(lbl) 'trim any blanks
          Call DecTrim(LabelTextFromValue) 'trim trailing "0"s
        Else
          LabelTextFromValue = Format(translatedVal, "#,###,###")
        End If

        'LabelTextFromValue = Format(translatedVal, "#,###,###.##")
        If Right(LabelTextFromValue, 1) = "." Then LabelTextFromValue = Left(LabelTextFromValue, Len(LabelTextFromValue) - 1)
        If Len(LabelTextFromValue) = 0 Then LabelTextFromValue = "0"
      End If
    '3 to 8 are proobability, not yet able to display those coordinates
  End Select
End Function

Private Function JulianDateToString(Jday As Double) As String
  Dim d(5) As Long
  J2Date Jday, d
  timcnv d 'convert to midnight 24:00
  JulianDateToString = d(0) & "/" & ATCformat(d(1), "00") & "/" & ATCformat(d(2), "00") & " " & ATCformat(d(3), "00") & ":" & ATCformat(d(4), "00")
End Function

Private Sub EditTextOnGraph()

  Dim FontToUse As Long, FontToSave As Long
  SetLogFont scrGraph.Font
'  FontToUse = CreateFontIndirect(lf)
'  FontToSave = SelectObject(Graph.hdc, FontToUse)
  txtPopup.Visible = False
  txtPopup.Font = scrGraph.Font
  txtPopup.Value = ""
  Dim tpos As Long, tp2 As Long, lwid As Single, lhgt As Single, lstr As String
  Dim dcx As Single, dcy As Single, x0 As Single, y0 As Single, xMx As Single, yMx As Single
  dcx = Wc2DcX(scrGraph, lastX1)
  dcy = Wc2DcY(scrGraph, lastY1)
  x0 = Wc2DcX(scrGraph, 0)
  y0 = Wc2DcY(scrGraph, 0)
  xMx = Wc2DcX(scrGraph, XLen)
  yMx = Wc2DcY(scrGraph, YLen)
  
  If dcx > x0 And dcx < xMx And dcy > y0 Then 'below graph
    lhgt = scrGraph.TextHeight(Title)
    If lastY1 < 2.5 * lhgt Then ' we are near the title
      Dim amper1 As Long, amper2 As Long
      amper1 = InStr(Title, "&")
      If amper1 = 0 Then 'one-line title
        txtPopup.Value = Title
        txtPopup.Top = 2.5 * lhgt
        PopupEditing = 50
      Else 'multi-line title
        amper2 = InStr(amper1 + 1, Title, "&")
        If amper2 > 0 And lastY1 < 4.5 * lhgt Then 'Third line
          txtPopup.Value = Mid(Title, amper2 + 1)
          txtPopup.Top = 4.5 * lhgt
          PopupEditing = 30
        Else
          If amper2 = 0 Then amper2 = Len(Title) + 1
          If lastY1 < 3.5 * lhgt Then 'Second line
            txtPopup.Value = Mid(Title, amper1 + 1, amper2 - amper1 - 1)
            txtPopup.Top = 3.5 * lhgt
            PopupEditing = 20
          Else 'First line
            txtPopup.Value = Left(Title, amper1 - 1)
            txtPopup.Top = 2.5 * lhgt
            PopupEditing = 10
          End If
        End If
      End If
    Else ' we are near the x-axis label
      If Axis(4).AType > 0 Then 'not time axis, so label is editable
        txtPopup.Value = Axis(4).label
        txtPopup.Top = 1.25 * lhgt
        PopupEditing = 4
      End If
    End If
    If PopupEditing > 0 Then
      lwid = scrGraph.TextWidth(txtPopup.Value) * 1.5
      txtPopup.Left = (XLen - lwid) / 2
    Else
      Call mnuEdGraph_Click(1)
    End If
    
  ElseIf dcy > yMx And dcy < y0 And dcx < x0 Then 'left of graph
    If dcy > yMx + ALen / (ALen + YLen) * (y0 - yMx) Then 'near left Y label
      PopupEditing = 1
      txtPopup.Value = Axis(1).label
      txtPopup.Top = YLen / 2
    ElseIf Axis(3).AType > 0 Then  'near aux axis label
      PopupEditing = 3
      txtPopup.Value = Axis(3).label
      txtPopup.Top = YLen + ALen / 2
    End If
    If PopupEditing > 0 Then
      txtPopup.Left = scrGraph.ScaleLeft
    Else
      Call mnuEdGraph_Click(1)
    End If
  ElseIf dcy > yMx And dcy < y0 And dcx > xMx Then 'right of graph
    If Axis(2).AType > 0 Then 'there is a right y axis
      PopupEditing = 2
      txtPopup.Value = Axis(2).label
      txtPopup.Top = YLen / 2
      txtPopup.Left = XLen
    End If
  ElseIf lastY1 > YLegMin And lastY1 < YLegMax And lastX1 > XLegMin And lastX1 < XLegMax Then 'inside legend
    If XLegLoc > -2 And nearest < 0 Then
      Dim curveNum As Long
      curveNum = -nearest - 1
      If curveNum >= 0 And curveNum < ncrv Then
        PopupEditing = nearest
        txtPopup.Value = Trim(Crv(curveNum).LegLbl)
        txtPopup.Top = YLegMax + curveNum * scrGraph.TextHeight(Crv(curveNum).LegLbl)
        txtPopup.Left = XLegMin + 0.05 * XLen
      Else
        PopupEditing = 0
      End If
    End If
  ElseIf lastY1 > YTxtMin And lastY1 < YTxtMax And lastX1 > XTxtMin And lastX1 < XTxtMax Then 'in extra text
    PopupEditing = -98
    txtPopup.Value = Trim(XtraText)
    txtPopup.Top = YTxtMax
    txtPopup.Left = XTxtMin
  'ElseIf dcy > yMx And dcy < y0 And dcx > x0 And dcx < xMx Then 'inside graph
  Else
    Call mnuEdGraph_Click(1)
  End If
  If txtPopup.Value <> "" Then
    txtPopup.Width = scrGraph.TextWidth(txtPopup.Value) * 1.5
    txtPopup.Visible = True
    txtPopup.SetFocus 'value = txtPopup.value
  End If
'  SelectObject Graph.hdc, FontToSave
'  DeleteObject FontToUse
End Sub

Public Sub DrawBoxPlot()

    Dim i As Long, j As Long, npts As Long, xaxtyp As Long, rpt() As Long, clr As Long, k As Long, nrpt As Long, tclr As Long
    Dim xinc As Single, yinc As Single, xp As Single, yp As Single, xmin As Single, xmax As Single, _
        ymin As Single, ymax As Single, median As Single, x(3) As Single, xpoint(3) As Single

    mnuList.Enabled = False
    xmin = Axis(4).minv
    xmax = Axis(4).maxv
    xaxtyp = Axis(4).AType
    xinc = XLen / (xmax - xmin)
    If Abs(xaxtyp) = 2 Then 'transform to log
      Graph.CurrentX = (Log10(CDbl(yx(bufpos(1, 0)))) - xmin) * xinc
    Else
      Graph.CurrentX = (yx(bufpos(1, 0)) - xmin) * xinc
    End If
    ymin = Axis(1).minv
    ymax = Axis(1).maxv
    yinc = YLen / (ymax - ymin)
    For i = 0 To ncrv - 1
      If ncrv = 1 Then
        yp = 0.5 * yinc
      Else
        yp = (ncrv - i) / ncrv * yinc
      End If
      'indicate curves w/color
      clr = Crv(i).Color
      npts = bufpos(2, i) - bufpos(1, i) + 1
      nrpt = 0
      If npts <= 5 Then 'just plot points
        For j = bufpos(1, i) To bufpos(2, i)
          xp = (yx(j) - xmin) * xinc
          For k = bufpos(1, i) To j - 1
            'look to see if this point already plotted
            If Abs(yx(j) - yx(k)) < 0.00001 Then
              'same value, need repeat indicator
              If nrpt < j Then
                '1st match for this value
                ReDim Preserve rpt(j)
                rpt(j) = 2
                nrpt = j
              Else 'additional match for this value
                rpt(j) = rpt(j) + 1
              End If
              rpt(k) = 0 'clear old repeat indicator
            End If
          Next k
          Call GrMark(Graph, xp, yp, 5, clr)
        Next j
        If nrpt > 0 Then
          For j = bufpos(1, i) To UBound(rpt)
            If rpt(j) > 0 Then 'display repeated point indicator
              Graph.CurrentX = (yx(j) - xmin) * xinc - 0.75 * Graph.TextWidth("0")
              Graph.CurrentY = yp - 0.25 * Abs(Graph.TextHeight("x"))
              tclr = Graph.ForeColor
              Graph.ForeColor = clr
              GraphPrint rpt(j)
              Graph.ForeColor = tclr
            End If
          Next j
        End If
      Else 'do full box plot
        'calculate median and box ranges here
        Call SetBoxPlot(i, median, xpoint())
        For j = 0 To 3
          x(j) = (xpoint(j) - xmin) * xinc
        Next j
        'plot median value
        xp = (median - xmin) * xinc
        Call GrMark(Graph, xp, yp, 3, clr)
        'draw preceeding/ensuing lines
        Graph.Line (x(0), yp)-(x(1), yp), clr
        Graph.Line (x(2), yp)-(x(3), yp), clr
        'draw actual box
        Graph.Line (x(1), yp - 0.25 * yinc / ncrv)-(x(1), yp + 0.25 * yinc / ncrv), clr
        Graph.Line -(x(2), yp + 0.25 * yinc / ncrv), clr
        Graph.Line -(x(2), yp - 0.25 * yinc / ncrv), clr
        Graph.Line -(x(1), yp - 0.25 * yinc / ncrv), clr
        'put markers at ends of whiskers
        Call GrMark(Graph, x(0), yp, 2, clr)
        Call GrMark(Graph, x(3), yp, 2, clr)
        'draw outliers
        nrpt = 0
        For j = bufpos(1, i) To bufpos(2, i)
          If yx(j) > (1.5 * (xpoint(2) - xpoint(1))) + xpoint(2) Or yx(j) < xpoint(1) - (1.5 * (xpoint(2) - xpoint(1))) Then
            'this is an outlier
            xp = (yx(j) - xmin) * xinc
            For k = bufpos(1, i) To j - 1
              'look to see if this point already plotted
              If Abs(yx(j) - yx(k)) < 0.00001 Then
                'same value, need repeat indicator
                If nrpt < j Then
                  '1st match for this value
                  ReDim Preserve rpt(j)
                  rpt(j) = 2
                  nrpt = j
                Else 'additional match for this value
                  rpt(j) = rpt(j) + 1
                End If
                rpt(k) = 0 'clear old repeat indicator
              End If
            Next k
            If (yx(j) > xpoint(2) + (1.5 * (xpoint(2) - xpoint(1))) And yx(j) <= xpoint(2) + (3 * (xpoint(2) - xpoint(1)))) _
            Or (yx(j) < xpoint(1) - (1.5 * (xpoint(2) - xpoint(1))) And yx(j) >= xpoint(1) - (3 * (xpoint(2) - xpoint(1)))) Then
              'outlier within 3 hinges
              Call GrMark(Graph, xp, yp, 4, clr)
            Else
              'outlier outside 3 hinges
              Call GrMark(Graph, xp, yp, 5, clr)
            End If
          End If
        Next j
        If nrpt > 0 Then
          For j = bufpos(1, i) To UBound(rpt)
            If rpt(j) > 0 Then 'display repeated point indicator
              Graph.CurrentX = (yx(j) - xmin) * xinc - 0.75 * Graph.TextWidth("0")
              Graph.CurrentY = yp - 0.25 * Abs(Graph.TextHeight("x"))
              tclr = Graph.ForeColor
              Graph.ForeColor = clr
              GraphPrint rpt(j)
              Graph.ForeColor = tclr
            End If
          Next j
        End If
      End If
    Next i

End Sub

Function SnapToTic(val As Single, axisNum As Long) As Single
  Dim retval As Single, ticval As Single, ticno As Long
  retval = val
  With Axis(axisNum)
    If .AType = 1 Then
      retval = .minv
      For ticno = 1 To .NTic
        ticval = .minv + (.maxv - .minv) * ticno / .NTic
        If Abs(val - ticval) < Abs(val - retval) Then retval = ticval
      Next ticno
    End If
  End With
  SnapToTic = retval
End Function

Private Function NearestYx(ByVal scrx As Single, _
                           ByVal scry As Single, _
                  Optional ByRef foundX As Double = 0, _
                  Optional ByRef foundY As Double = 0, _
                  Optional ByRef nearestscrX As Single = 0, _
                  Optional ByRef nearestscrY As Single = 0, _
                  Optional ByVal findEvenIfFar As Boolean = False, _
                  Optional ByVal snapX As Boolean = False, _
                  Optional ByVal snapY As Boolean = False) As Long

  Dim curX As Double, curY As Double
  Dim i As Long
  Dim ipos As Long, EDate As Long, lLen As Long
  Dim pos As Long, clr As Long, tclr As Long
  Dim kx As Long, ky As Long
  Dim xaxtyp As Long, ilgnd As Long
  Dim ypos As Long, xpos As Long
  Dim minypos As Long, maxypos As Long
  Dim minxpos As Long, maxxpos As Long
  Dim rtmp As Single, lextnt As Single
  Dim xinc As Single, yinc As Single
  Dim xp As Single, yp As Single
  Dim xmin As Single, xmax As Single, ymin As Single, ymax As Single
  Dim yylen As Single, base As Single
  Dim xtmp As Single, ytmp As Single, slope As Single
  Dim lstr As String, txt As String
  Dim nearestSoFar As Long, xdiff As Single, ydiff As Single, dist As Single, nearestDistance As Single
Dim js As Double
  Dim je As Double

  nearestDistance = 1000000
  NearestYx = NONE

On Error GoTo errorhandler11
  
  'assign variables for this curve
  ky = wchvr(1, curveToTrack)
  kx = wchvr(2, curveToTrack)
  minxpos = bufpos(1, kx)
  maxxpos = bufpos(2, kx)
  minypos = bufpos(1, ky)
  maxypos = bufpos(2, ky)
  If Crv(curveToTrack).CType < 6 Then 'time X-axis
    js = Date2J(sdatim)
    je = Date2J(edatim)
    xaxtyp = 0
    If Crv(curveToTrack).CType < 5 Then 'constant interval
      Dim nYvals As Long    'Number of y-values available for this curve
      Dim nIntWide As Long  'Number of full intervals of space available in graph
      Dim tdatim(6) As Long 'time of end of last interval there is space for
      Dim jt As Double      'double version of tdatim
      Dim jratio As Double  'ratio of space for intervals vs. total width
      nYvals = maxypos - minypos + 1
      timdif sdatim, edatim, TUnits(i), TSTEP(i), nIntWide
      TIMADD sdatim, TUnits(i), TSTEP(i), nIntWide, tdatim
      jt = Date2J(tdatim)
      jratio = (jt - js) / (je - js)
      'xinc = inches per interval
      xinc = XLen * jratio / nYvals
    Else 'not constant interval data
      'xinc = inches per julian day
      xinc = XLen / (je - js)
    End If
    xtmp = 0
  Else 'numeric X-axis
    xmin = Axis(Var(kx).WchAx).minv
    xmax = Axis(Var(kx).WchAx).maxv
    xaxtyp = Axis(Var(kx).WchAx).AType
    xinc = XLen / (xmax - xmin)
  End If
  ymin = Axis(Var(ky).WchAx).minv
  ymax = Axis(Var(ky).WchAx).maxv
  If ymax <> ymin Then
    If Var(ky).WchAx = 3 Then  'aux axis line
      yylen = ALen * YLen
      base = YLen - yylen
    Else  'main plot line
      If ALen > 0 Then  'aux axis in use
        yylen = YLen - (ALen * YLen) - Abs(Graph.TextHeight("X"))
      Else  'no aux axis
        yylen = YLen
      End If
      base = 0
    End If
    yinc = yylen / (ymax - ymin)
    For ypos = minypos To maxypos
      xpos = minxpos + ypos - minypos
      If xaxtyp = 0 Then 'time X-axis
        If Crv(curveToTrack).CType < 5 Then 'constant interval
          xp = (ypos - minypos + 1) * xinc
          curX = xmin + xp / XLen
        Else 'not constant interval data
          curX = yx(xpos)
          xp = (curX - js) * xinc
        End If
      ElseIf Abs(xaxtyp) = 2 Then 'transform to log
        curX = yx(xpos)
        xp = (Log10(curX) - xmin) * xinc
      Else 'numeric X-axis
        curX = yx(xpos)
        xp = (curX - xmin) * xinc
      End If
      curY = yx(ypos)
      If Var(ky).Trans = 1 Then 'arithmetic Y
        yp = (curY - ymin) * yinc + base
      Else 'logarithmic Y
        yp = (Log10(CDbl(curY)) - ymin) * yinc + base
      End If
      If xp < 0 Or xp > XLen Or yp > yylen + base Or yp < base Then
        'value outside plot
      Else 'good point
        'compare this point's coordinates (xp, yp) with sought coordinates
        xdiff = scrx - xp
        ydiff = scry - yp
        If snapX Then
          dist = Abs(xdiff)
        ElseIf snapY Then
          dist = Abs(ydiff)
        Else
          dist = xdiff * xdiff + ydiff * ydiff
        End If
        If dist < nearestDistance Then
          nearestDistance = dist
          nearestSoFar = ypos
          nearestscrX = xp
          nearestscrY = yp
          foundX = curX
          foundY = curY
'          txt = ypos
        End If
      End If
    Next ypos
  End If
  If Var(ky).WchAx < 3 And (XLegLoc > -2# Or XLegLoc = NONE) And lenStr(Crv(i).LegLbl) > 0 _
    And scrx < XLegMax And scrx > XLegMin And scry < YLegMax And scry > YLegMin Then
    'legend
    lstr = Trim(Crv(i).LegLbl)
    If YLegLoc <> NONE Then 'user value
      yp = YLegLoc * yylen - (ilgnd + 0.25) * -Graph.TextHeight(lstr) - 1.25 * Axis(3).TLen
    Else 'default
      yp = yylen - (ilgnd + 0.25) * -Graph.TextHeight(lstr) - 1.25 * Axis(3).TLen
    End If
    ilgnd = ilgnd + 1

    'xdiff = scrx - xp
    ydiff = scry - yp
    dist = ydiff * ydiff
    If dist < nearestDistance Then
      nearestDistance = dist
      nearestSoFar = -1 - i
      nearestscrX = scrx
      nearestscrY = yp
    End If
  End If
  
  'Check to see how close we are to lines
  For i = 1 To nLine
    Dim axNum As Long, val As Single, axMin As Single, axMax As Single, inc As Single, diff As Single, thisX As Single, thisY As Single
    
    If IsNumeric(Lines(i).Value) Then
      val = Lines(i).Value
      axNum = Lines(i).WchAx
      axMin = Axis(axNum).minv
      axMax = Axis(axNum).maxv
      
      If axNum = 4 Then
        inc = xinc: thisY = scry
      Else
        inc = yinc: thisX = scrx
      End If
      
      If axMax - axMin < 0.001 Then axMin = 0: axMax = inc

      If val >= axMin And val <= axMax Then
        If Axis(axNum).AType = 1 Then 'arithmetic
          xp = (val - axMin) * inc
        Else
          xp = (Log10(CDbl(val)) - axMin) * inc
        End If
        If axNum = 4 Then thisX = xp Else thisY = xp
        dist = (scrx - thisX) ^ 2 + (scry - thisY) ^ 2
        If dist < nearestDistance Then
          nearestDistance = dist
          nearestSoFar = -100 - i
          foundX = curX
          foundY = curY
          'Debug.Print "Constant " & axNum & "=" & val
        End If
      End If
    End If
  Next i
'  If ConstYfg = 1 Then 'there is a constant y line
'    If ConstY >= ymin And ConstY <= ymax Then
'      If Axis(1).AType = 1 Then 'arithmetic Y
'        yp = (ConstY - ymin) * yinc '+ base
'      Else 'logarithmic Y
'        yp = (Log10(CDbl(ConstY)) - ymin) * yinc '+ base
'      End If
'
'        ydiff = scry - yp
'        dist = ydiff * ydiff
'        If dist < nearestDistance Then
'          nearestDistance = dist
'          nearestSoFar = -107
'          nearestscrX = scrx
'          nearestscrY = yp
'          txt = "Constant Y=" & ConstY
'        End If
'    End If
'  End If

'  If ConstXfg = 1 Then 'there is a constant x line
'    If ConstX >= xmin And ConstX <= xmax Then
'      If Axis(4).AType = 1 Then 'arithmetic X
'        xp = (ConstX - xmin) * xinc
'      Else 'logarithmic X
'        xp = (Log10(CDbl(ConstX)) - xmin) * xinc
'      End If

'        xdiff = scrx - xp
'        dist = xdiff * xdiff
'        If dist < nearestDistance Then
'          nearestDistance = dist
'          nearestSoFar = -106
'          nearestscrX = xp
'          nearestscrY = scry
'          txt = "Constant X=" & ConstX
'        End If
'    End If
'  End If
          
  If nearestSoFar < 0 And nearestSoFar > -100 Then
    dist = 0
    If XLegLoc <> NONE Then 'user value
      xp = XLegLoc * XLen
    Else 'default
      xp = 0.05 * XLen
    End If
    If nearestscrX < xp + 0.05 * XLen Then nearestSoFar = -99 ' left of text in legend - drag legend
  Else
    dist = ((scrx - nearestscrX) * scrGraph.ScaleWidth) ^ 2 + ((scry - nearestscrY) * scrGraph.ScaleHeight) ^ 2
  End If
  'If Sqr(nearestDistance) < NearnessTolerance * scrGraph.ScaleWidth Then
  If findEvenIfFar Then
    NearestYx = nearestSoFar
  ElseIf Sqr(dist) < NearnessTolerance Then
    NearestYx = nearestSoFar
    'Debug.Print "Nearest (" & nearestscrX & ", " & nearestscrY & ") " & nearestSoFar & " dist=" & nearestDistance
  End If
Exit Function
errorhandler11:   ' Error handler line label.
  Call DispError("nearestYx", Err)
  Resume Next ' Resume procedure.

End Function

Private Sub txtPopup_CommitChange()
  Dim amper1 As Long, amper2 As Long
  txtPopup.Visible = False
  If PopupEditing > 0 Then
    If PopupEditing <= 4 Then
      Axis(PopupEditing).label = txtPopup.Value
      ReDrawGraph 0
    ElseIf PopupEditing = 10 Then 'First line of title
      amper1 = InStr(Title, "&")
      If amper1 < 1 Then
        Title = txtPopup.Value
      Else
        Title = txtPopup.Value & Mid(Title, amper1)
      End If
    ElseIf PopupEditing = 20 Then 'Second line of title
      amper1 = InStr(Title, "&")
      If amper1 < 1 Then
        Title = txtPopup.Value
      Else
        amper2 = InStr(amper1 + 1, Title, "&")
        If amper2 < 1 Then
          Title = Left(Title, amper1) & txtPopup.Value
        Else
          Title = Left(Title, amper1) & txtPopup.Value & Mid(Title, amper2)
        End If
      End If
    ElseIf PopupEditing = 30 Then 'Third line of title
      amper1 = InStr(Title, "&")
      If amper1 < 1 Then
        Title = txtPopup.Value
      Else
        amper2 = InStr(amper1 + 1, Title, "&")
        If amper2 < 1 Then
          Title = Left(Title, amper1) & txtPopup.Value
        Else
          Title = Left(Title, amper2) & txtPopup.Value
        End If
      End If
    ElseIf PopupEditing = 50 Then 'Whole one-line title
      Title = txtPopup.Value
    End If
    If PopupEditing >= 10 Then
      BlankTitle
      DrwTitle
    End If
  ElseIf PopupEditing = -98 Then
    XtraText = txtPopup.Value
    ReDrawGraph -1
  ElseIf PopupEditing < 0 And -PopupEditing <= ncrv Then
    Crv(-PopupEditing - 1).LegLbl = txtPopup.Value
    ReDrawGraph -1
  End If
  PopupEditing = 0
End Sub

Public Sub RetrieveSpecs(FileName As String)
  Dim AplusYlen As Single
  Dim lAlen As Single
  Dim i As Long
  Dim a As Long
  Dim fu As Integer
  Dim s As String ', rect As Rectangle
  Dim istr As String
  Dim rtyp As String
  Dim Utyp As String
  Dim lAlreadyAddedLine As Boolean
  
  DbgMsg "RetrieveSpecs:" & FileName
  
  On Error GoTo ReadErr
  fu = FreeFile(0)
  DbgMsg "RetrieveSpecs:OpenFile:" & FileName
  Open FileName For Input As #fu
  i = 0
  
  Do While Not EOF(fu)
    Line Input #fu, istr
    rtyp = UCase(StrRetRem(istr))
    DbgMsg "RetrieveSpecs:ReadRecord:" & rtyp & ":" & istr
    
    Select Case rtyp
      Case "":
      Case "ALEN":    ALen = CSng(istr)
      Case "YLAB":    Axis(1).label = StrRetRem(istr)
      Case "YRLAB":   Axis(2).label = StrRetRem(istr)
      Case "ALAB":    Axis(3).label = StrRetRem(istr)
      Case "XLAB":    Axis(4).label = StrRetRem(istr)
      Case "YTYP":    Axis(1).AType = CLng(StrRetRem(istr))
      Case "YRTYP":   Axis(2).AType = CLng(StrRetRem(istr))
      Case "XTYP":    Axis(4).AType = CLng(StrRetRem(istr))
      Case "XGRD":    Gridx = CLng(StrRetRem(istr))
      Case "YGRD":    lGridy = CLng(StrRetRem(istr))
      Case "YRGRD":   rGridy = CLng(StrRetRem(istr))
      Case "SCALE":   i = CLng(StrRetRem(istr))
                      Axis(i).minv = CSng(StrRetRem(istr))
                      Axis(i).maxv = CSng(StrRetRem(istr))
                      Axis(i).NTic = CLng(StrRetRem(istr))
      Case "CURVE"
                      i = CLng(StrRetRem(istr))
                      With Crv(i)
                        .CType = CLng(StrRetRem(istr))
                        .LType = CLng(StrRetRem(istr)) - 1
                        .LThck = CLng(StrRetRem(istr))
                        .SType = CLng(StrRetRem(istr))
                      
                        .Color = -1
                        s = StrRetRem(istr)
                        If IsNumeric(s) Then
                          Dim qb As Long
                          qb = s
                          If qb > 0 And qb < 16 Then .Color = QBColor(s)
                        End If
                        If .Color = -1 Then .Color = TextOrNumericColor(s)
                        
                        s = StrRetRem(istr)
                        If IsNumeric(s) Then
                          dtype(i) = CLng(s)
                        Else
                          istr = s & " " & istr
                        End If
                        
                        .LegLbl = StrRetRem(istr)
                      End With
      Case "VAR"
                      i = CLng(StrRetRem(istr))
                      Var(i).WchAx = CLng(StrRetRem(istr))
                      'Var(i).Trans = CLng(StrRetRem(istr))
                      Var(i).label = StrRetRem(istr)
      Case "LINE"
                      If NumLines > 0 And Not lAlreadyAddedLine Then NumLines = 0
                      DbgMsg "Adding line " & StrRetRem(istr)
                      AddLine CLng(StrRetRem(istr)), CLng(StrRetRem(istr)), CLng(StrRetRem(istr)), CLng(StrRetRem(istr)), TextOrNumericColor(StrRetRem(istr)), StrSplit(istr, " ", "'"), ReplaceString(istr, "'", "")
                      lAlreadyAddedLine = True
      Case "LOCLEGEND"
                      XLegLoc = CSng(StrRetRem(istr))
                      YLegLoc = CSng(StrRetRem(istr))
      Case "ADDTEXT"
                      XtraText = StrRetRem(istr)
                      XTxtLoc = CSng(StrRetRem(istr))
                      YTxtLoc = CSng(StrRetRem(istr))
      Case "TITLE"
                      Title = StrRetRem(istr)
                      Caption = StrRetRem(istr)
      Case "DATALABELS"
                      DataLabelPosition = CLng(StrRetRem(istr))
      Case Else
                      DbgMsg "RetrieveSpecs:Unknown directive:" & rtyp & ":" & istr
    End Select
  Loop
  DbgMsg "RetrieveSpecs:EndofMapFile"

  Close #fu
  ReDrawGraph 0
  Exit Sub 'completed ok
  
ReadErr:
    MsgBox "A problem occurred reading the graph file " & FileName & vbCrLf & Err.Description, 48, "Graph Problem"
    DbgMsg "RetrieveSpecs:Error:" & Err.Number & ":" & Err.Description & ":" & istr

End Sub

Private Sub DbgMsg(msg As String)
  If Not (pIPC Is Nothing) Then pIPC.dbg "ATCoGraph:frmG:" & msg
End Sub
