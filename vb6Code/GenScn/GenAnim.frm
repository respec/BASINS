VERSION 5.00
Begin VB.Form frmGenScnAnimate 
   Caption         =   "GenScn Map Animation"
   ClientHeight    =   6636
   ClientLeft      =   456
   ClientTop       =   1236
   ClientWidth     =   9384
   BeginProperty Font 
      Name            =   "MS Sans Serif"
      Size            =   7.8
      Charset         =   0
      Weight          =   700
      Underline       =   0   'False
      Italic          =   0   'False
      Strikethrough   =   0   'False
   EndProperty
   HelpContextID   =   111
   Icon            =   "GenAnim.frx":0000
   LinkTopic       =   "Form1"
   PaletteMode     =   1  'UseZOrder
   ScaleHeight     =   6636
   ScaleWidth      =   9384
   Begin VB.Frame fraCmds 
      Height          =   3732
      Left            =   6480
      TabIndex        =   14
      Top             =   0
      Width           =   2775
      Begin VB.CommandButton cmdRamp 
         Caption         =   "Ramp"
         Height          =   252
         Left            =   1920
         TabIndex        =   31
         Top             =   2880
         Width           =   732
      End
      Begin VB.CheckBox chkEditIndiv 
         Caption         =   "&Individual Locations"
         Height          =   255
         Left            =   120
         TabIndex        =   28
         Top             =   2520
         Width           =   2532
      End
      Begin VB.ComboBox cboStyle 
         Height          =   288
         Left            =   1200
         Style           =   2  'Dropdown List
         TabIndex        =   33
         Top             =   3240
         Width           =   1452
      End
      Begin VB.CommandButton cmdAnimate 
         Height          =   300
         Index           =   4
         Left            =   1800
         Picture         =   "GenAnim.frx":0442
         Style           =   1  'Graphical
         TabIndex        =   11
         ToolTipText     =   "One Frame"
         Top             =   600
         Width           =   300
      End
      Begin VB.CommandButton cmdAnimate 
         Height          =   300
         Index           =   5
         Left            =   720
         Picture         =   "GenAnim.frx":074C
         Style           =   1  'Graphical
         TabIndex        =   5
         ToolTipText     =   "Back One Frame"
         Top             =   600
         Width           =   300
      End
      Begin VB.CommandButton cmdAnimate 
         Height          =   300
         Index           =   3
         Left            =   1080
         Picture         =   "GenAnim.frx":0A56
         Style           =   1  'Graphical
         TabIndex        =   7
         ToolTipText     =   "Stop"
         Top             =   600
         Width           =   300
      End
      Begin VB.CommandButton cmdAnimate 
         Height          =   300
         Index           =   2
         Left            =   1440
         Picture         =   "GenAnim.frx":1320
         Style           =   1  'Graphical
         TabIndex        =   9
         ToolTipText     =   "Pause"
         Top             =   600
         Width           =   300
      End
      Begin VB.CommandButton cmdAnimate 
         Height          =   300
         Index           =   1
         Left            =   360
         Picture         =   "GenAnim.frx":162A
         Style           =   1  'Graphical
         TabIndex        =   3
         ToolTipText     =   "Reverse Play"
         Top             =   600
         Width           =   300
      End
      Begin VB.CommandButton cmdAnimate 
         Height          =   300
         Index           =   0
         Left            =   2160
         Picture         =   "GenAnim.frx":1934
         Style           =   1  'Graphical
         TabIndex        =   15
         ToolTipText     =   "Play"
         Top             =   600
         Width           =   300
      End
      Begin VB.CheckBox chkPause 
         Caption         =   "&High"
         Height          =   255
         Index           =   1
         Left            =   1920
         TabIndex        =   17
         Top             =   960
         Width           =   732
      End
      Begin VB.CheckBox chkPause 
         Caption         =   "&Low"
         Height          =   255
         Index           =   0
         Left            =   1320
         TabIndex        =   16
         Top             =   960
         Width           =   732
      End
      Begin VB.CommandButton cmdSavePic 
         Caption         =   "Sa&ve Map"
         Height          =   252
         Left            =   1440
         TabIndex        =   21
         Top             =   1320
         Width           =   1212
      End
      Begin VB.CommandButton cmdPrint 
         Caption         =   "&Print Map"
         Height          =   252
         Left            =   1440
         TabIndex        =   22
         Top             =   1680
         Width           =   1212
      End
      Begin VB.CommandButton cmdSave 
         Caption         =   "&Save Specs"
         Height          =   252
         Left            =   120
         TabIndex        =   18
         Top             =   1320
         Width           =   1212
      End
      Begin VB.CommandButton cmdGet 
         Caption         =   "&Get Specs"
         Height          =   252
         Left            =   120
         TabIndex        =   19
         Top             =   1680
         Width           =   1212
      End
      Begin VB.CommandButton cmdClear 
         Caption         =   "Cl&ear Specs"
         Height          =   252
         Left            =   120
         TabIndex        =   20
         Top             =   2040
         Visible         =   0   'False
         Width           =   1212
      End
      Begin VB.CommandButton cmdProfile 
         Caption         =   "Pro&file Plot"
         Height          =   252
         Left            =   1440
         TabIndex        =   23
         Top             =   2040
         Width           =   1212
      End
      Begin VB.ComboBox cboDate 
         Height          =   315
         Left            =   360
         Style           =   2  'Dropdown List
         TabIndex        =   0
         Top             =   240
         Width           =   2080
      End
      Begin MSComDlg.CommonDialog CDGet 
         Left            =   120
         Top             =   2040
         _ExtentX        =   699
         _ExtentY        =   699
         _Version        =   393216
         FontSize        =   1.93691e-37
      End
      Begin MSComDlg.CommonDialog CDSave 
         Left            =   120
         Top             =   1560
         _ExtentX        =   699
         _ExtentY        =   699
         _Version        =   393216
         FontSize        =   1.93691e-37
      End
      Begin MSComctlLib.ImageList ImageList1 
         Left            =   2280
         Top             =   600
         _ExtentX        =   804
         _ExtentY        =   804
         BackColor       =   -2147483643
         ImageWidth      =   16
         ImageHeight     =   16
         MaskColor       =   8421376
         _Version        =   393216
         BeginProperty Images {2C247F25-8591-11D1-B16A-00C0F0283628} 
            NumListImages   =   4
            BeginProperty ListImage1 {2C247F27-8591-11D1-B16A-00C0F0283628} 
               Picture         =   "GenAnim.frx":1C3E
               Key             =   ""
            EndProperty
            BeginProperty ListImage2 {2C247F27-8591-11D1-B16A-00C0F0283628} 
               Picture         =   "GenAnim.frx":2190
               Key             =   ""
            EndProperty
            BeginProperty ListImage3 {2C247F27-8591-11D1-B16A-00C0F0283628} 
               Picture         =   "GenAnim.frx":26E2
               Key             =   ""
            EndProperty
            BeginProperty ListImage4 {2C247F27-8591-11D1-B16A-00C0F0283628} 
               Picture         =   "GenAnim.frx":2C34
               Key             =   ""
            EndProperty
         EndProperty
      End
      Begin ATCoCtl.ATCoText txtRanges 
         Height          =   252
         Left            =   1200
         TabIndex        =   30
         Top             =   2880
         Width           =   492
         _ExtentX        =   868
         _ExtentY        =   445
         InsideLimitsBackground=   16777215
         OutsideHardLimitBackground=   8421631
         OutsideSoftLimitBackground=   8454143
         HardMax         =   20
         HardMin         =   1
         SoftMax         =   -999
         SoftMin         =   -999
         MaxWidth        =   -999
         Alignment       =   1
         DataType        =   1
         DefaultValue    =   "3"
         Value           =   "3"
         Enabled         =   -1  'True
      End
      Begin VB.Label Label3 
         Caption         =   "Value Pause"
         Height          =   252
         Left            =   120
         TabIndex        =   35
         Top             =   980
         Width           =   1092
      End
      Begin VB.Label lblRanges 
         Caption         =   "Ranges:"
         Height          =   252
         Left            =   120
         TabIndex        =   29
         Top             =   2880
         Width           =   1092
      End
      Begin VB.Label lblStyle 
         Caption         =   "Draw Style:"
         Height          =   252
         Left            =   120
         TabIndex        =   32
         Top             =   3240
         Width           =   1092
      End
      Begin VB.Label lblAnimButton 
         Caption         =   "&<"
         Height          =   255
         Index           =   7
         Left            =   360
         TabIndex        =   2
         Top             =   600
         Width           =   135
      End
      Begin VB.Label lblAnimButton 
         Caption         =   "&>"
         Height          =   255
         Index           =   6
         Left            =   2160
         TabIndex        =   13
         Top             =   600
         Width           =   135
      End
      Begin VB.Label lblAnimButton 
         Caption         =   "&6"
         Height          =   255
         Index           =   5
         Left            =   2160
         TabIndex        =   12
         Top             =   600
         Width           =   135
      End
      Begin VB.Label lblAnimButton 
         Caption         =   "&5"
         Height          =   255
         Index           =   4
         Left            =   1800
         TabIndex        =   10
         Top             =   600
         Width           =   135
      End
      Begin VB.Label lblAnimButton 
         Caption         =   "&4"
         Height          =   255
         Index           =   3
         Left            =   1440
         TabIndex        =   8
         Top             =   600
         Width           =   135
      End
      Begin VB.Label lblAnimButton 
         Caption         =   "&3"
         Height          =   255
         Index           =   2
         Left            =   1080
         TabIndex        =   6
         Top             =   600
         Width           =   135
      End
      Begin VB.Label lblAnimButton 
         Caption         =   "&2"
         Height          =   255
         Index           =   1
         Left            =   720
         TabIndex        =   4
         Top             =   600
         Width           =   135
      End
      Begin VB.Label lblAnimButton 
         Caption         =   "&1"
         Height          =   255
         Index           =   0
         Left            =   360
         TabIndex        =   1
         Top             =   600
         Width           =   135
      End
   End
   Begin VB.PictureBox pctBuffer 
      AutoRedraw      =   -1  'True
      AutoSize        =   -1  'True
      Height          =   3372
      Left            =   0
      ScaleHeight     =   3324
      ScaleWidth      =   6144
      TabIndex        =   27
      Top             =   480
      Visible         =   0   'False
      Width           =   6192
   End
   Begin VB.Frame sash 
      Appearance      =   0  'Flat
      BorderStyle     =   0  'None
      Caption         =   "Frame1"
      ForeColor       =   &H80000008&
      Height          =   60
      Left            =   0
      MousePointer    =   7  'Size N S
      TabIndex        =   26
      Top             =   3960
      Width           =   6975
   End
   Begin ATCoCtl.ATCoGrid agdSegments 
      Height          =   1932
      Left            =   0
      TabIndex        =   34
      Top             =   4200
      Width           =   7812
      _ExtentX        =   13780
      _ExtentY        =   3408
      SelectionToggle =   0   'False
      AllowBigSelection=   0   'False
      AllowEditHeader =   0   'False
      AllowLoad       =   0   'False
      AllowSorting    =   -1  'True
      Rows            =   2
      Cols            =   5
      ColWidthMinimum =   300
      gridFontBold    =   0   'False
      gridFontItalic  =   0   'False
      gridFontName    =   "MS Sans Serif"
      gridFontSize    =   8
      gridFontUnderline=   0   'False
      gridFontWeight  =   400
      gridFontWidth   =   0
      Header          =   "Loc"
      FixedRows       =   1
      FixedCols       =   0
      ScrollBars      =   3
      SelectionMode   =   0
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
      ComboCheckValidValues=   -1  'True
   End
   Begin MSComDlg.CommonDialog ComPrt 
      Left            =   0
      Top             =   0
      _ExtentX        =   699
      _ExtentY        =   699
      _Version        =   393216
      FontSize        =   1.93691e-37
   End
   Begin MSComDlg.CommonDialog comMap 
      Left            =   0
      Top             =   0
      _ExtentX        =   699
      _ExtentY        =   699
      _Version        =   393216
      FontSize        =   1.93691e-37
   End
   Begin VB.PictureBox pctAnim 
      AutoRedraw      =   -1  'True
      AutoSize        =   -1  'True
      Height          =   3372
      Left            =   0
      ScaleHeight     =   3324
      ScaleWidth      =   6144
      TabIndex        =   25
      Top             =   480
      Visible         =   0   'False
      Width           =   6192
   End
   Begin ATML2k.ATCoMap Map2 
      Height          =   3912
      Left            =   0
      TabIndex        =   24
      Top             =   0
      Width           =   6312
      _ExtentX        =   11134
      _ExtentY        =   6900
      RefreshMapLayer =   0   'False
      ConfirmSelections=   0   'False
      Enabled         =   -1  'True
      LegendVisible   =   0   'False
      ToolbarVisible  =   -1  'True
   End
End
Attribute VB_Name = "frmGenScnAnimate"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants

Private AnimRch() As MapShapeLine

Private TsRch() As Long   'pointer to reach index for each timeseries
Private ants As Long      'count of timeseries being animated
Private nRanges As Long   'Number of colored ranges
Private nRangesAnInf As Long 'nRanges stored in AnInf array
Private lTs As Collection 'local copy of tser
Private PPinit&           'profile plot init flag

'animation parameters
Private Its&, tsinc&
Private widind(1 To 5) As String 'widths of animation lines/symbols
Private SashDragging As Boolean

Private Type AnimInfo
  valu As Single ' lower data bound
  colr As Long   ' color
  styl As Long   ' line width
End Type
Private AnInf() As AnimInfo

Private stopfg As Boolean
Private Animating As Boolean
Private AnimInfoChanged As Boolean
Private DefaultStyle As Long
Private CustomStyle As Long
Private LastStyleName As String

'Const AnimBitmapFilename = "anim.bmp"

Private Sub Animate() 'main draw animation routine

  Dim curTs&, c&, j&, k&, r&, b&, ob&
  Dim x1#, x2#, y1#, y2#, radius#, lValue#
  Dim v As Variant
  Dim o As Object
  Dim poly() As POINTAPI
  
  If Not Me.Visible Then Exit Sub
  
  If Not Animating Then
    Animating = True
    CmdAvail True
    
    If Not pctAnim.Visible Then DrawPicture
    
    Set o = pctAnim
    radius = o.ScaleWidth / 150
    Do 'While not stopfg 'changes with pause/stop click or done(+ or -)
      If AnimInfoChanged Then SetAnimInfo
      Set o.Picture = pctBuffer.Picture 'LoadPicture(AnimBitmapFilename)
      Its = cboDate.ListIndex + tsinc
      If Its >= 0 And Its < cboDate.ListCount Then
        cboDate.ListIndex = Its
        For curTs = 0 To ants - 1
          If Its < lTs(curTs + 1).dates.Summary.NVALS Then
            lValue = lTs(curTs + 1).value(Its + 1)
            c = 1
            Do
              If lValue <= AnInf(curTs, c).valu Then
                o.ForeColor = AnInf(curTs, c - 1).colr
                o.DrawWidth = AnInf(curTs, c - 1).styl
                Exit Do
              End If
              c = c + 1
            Loop While c < nRanges
            
            If c = nRanges Then
              o.ForeColor = AnInf(curTs, c - 1).colr
              o.DrawWidth = AnInf(curTs, c - 1).styl
              If chkPause(1).value = 1 Then stopfg = True
            End If
  
            If c = 1 And chkPause(0).value = 1 Then stopfg = True
  
            r = TsRch(curTs + 1) - 1
            For j = 0 To AnimRch(r).linecnt - 1
              v = AnimRch(r).RchMapLine(j).VertAr
              If AnimRch(r).RchMapLine(j).ShapeType = 5 Then 'fill polygon
                With AnimRch(r).RchMapLine(j)
                  ReDim poly(.VertCnt - 1)
                  For k = 0 To .VertCnt - 1
                    ' -2 is a kludge adjustment
                    poly(k).X = -2 + (((v(k, 0) - o.ScaleLeft) / o.ScaleWidth) * o.Width) / Screen.TwipsPerPixelX
                    poly(k).Y = -2 + (((v(k, 1) - o.ScaleTop) / o.ScaleHeight) * o.Height) / Screen.TwipsPerPixelY
                  Next k
                  b = CreateSolidBrush(o.ForeColor)
                  ob = SelectObject(o.hDC, b)
                  k = Polygon(o.hDC, poly(0), .VertCnt - 1)
                  k = SelectObject(o.hDC, ob)
                  k = DeleteObject(b)
                End With
              ElseIf AnimRch(r).RchMapLine(j).ShapeType = 1 Then
                o.Circle (v(0, 0), v(0, 1)), radius
              Else 'line
                x2 = v(0, 0)
                y2 = v(0, 1)
                For k = 1 To AnimRch(r).RchMapLine(j).VertCnt - 1
                  x1 = x2
                  y1 = y2
                  x2 = v(k, 0)
                  y2 = v(k, 1)
                  o.Line (x1, y1)-(x2, y2)
                Next k
              End If
            Next j
          End If
        Next curTs
        DoEvents
      Else ' done at either end
        stopfg = True
      End If
    Loop While Not stopfg  'changes with pause/stop click or done(+ or -)
    If Its < 0 Then
      Its = 0
    End If
    CmdAvail True
    Animating = False
  End If
End Sub

Private Sub CmdAvail(TF As Boolean)
  Dim j&
  'For j = 1 To 8
  '  agdSegments.ColEditable(j) = TF
  'Next j
  cmdPrint.Enabled = TF
  cmdProfile.Enabled = TF
  cmdSavePic.Enabled = TF
  cmdSave.Enabled = TF
  cmdGet.Enabled = TF
  cmdClear.Enabled = TF
  'cmdAnim(0).Enabled = TF 'Cancel button
  'If Not (TF) Then cmdAnim(1).Enabled = TF
End Sub


Private Sub DrawPicture() 'make map a bit map, set picture box to map scale

  Dim x1#, x2#, y1#, y2#
  Dim e As Rectangle, p As Point
  Dim Filename As String
  Filename = GetTmpPath & "animate.bmp"
  Map2.map.ExportMap moExportBMP, Filename, 1 'moExportClipboardBMP, True, 1
  pctBuffer.Picture = LoadPicture(Filename)   'Clipboard.GetData
  Kill Filename
  Map2.Enabled = False
  Map2.Visible = False
  pctAnim.Top = Map2.Top + Map2.map.Container.Top
  pctAnim.Left = Map2.map.Left + Map2.Left
  'pctAnim.Width = Map2.Map.Width
  'pctAnim.Height = Map2.Map.Height
  pctAnim.Picture = pctBuffer.Picture 'LoadPicture(AnimBitmapFilename) 'AutoSize property sets width and height
  
  Set e = Map2.map.Extent
  Set p = e.Center
  x1 = p.X
  x2 = e.Width
  y1 = p.Y
  y2 = e.Height
  pctAnim.ScaleLeft = x1 - (x2 / 2#)
  pctAnim.ScaleWidth = x2
  pctAnim.ScaleTop = y1 + (y2 / 2#)
  pctAnim.ScaleHeight = -y2
  pctAnim.Visible = True

End Sub

Private Sub AnimInfoToGrid()
  Dim Location As Long, ColorRange As Long, r&, c&, w&
  Dim Warned As Boolean, FoundMinValue As Boolean
  Dim MinColor As Long, MaxColor As Long
  Dim MinValue As Single, MaxValue As Single
  
  With agdSegments
    Location = 0
    For r = 1 To .Rows
      Location = r - 1
      .row = r
      ColorRange = 0
      If nRanges <> nRangesAnInf Then
        If nRangesAnInf > 1 Then MinValue = AnInf(0, 1).valu: FoundMinValue = True
        MinColor = AnInf(0, 0).colr
        MaxValue = AnInf(ants - 1, nRangesAnInf - 1).valu
        MaxColor = AnInf(ants - 1, nRangesAnInf - 1).colr
        For c = 0 To .cols - 1
          .col = c
          Select Case Left(.ColTitle(c), 1)
            Case "<": .CellBackColor = MinColor
                      .text = ""
            Case "V": .CellBackColor = -1
                      ColorRange = ColorRange + 1
                      If nRanges < 3 Then
                        .text = MinValue
                      Else
                        .text = Fix(MinValue + (MaxValue - MinValue) * ((ColorRange - 1) / (nRanges - 2)))
                      End If
                      If IsNumeric(.text) Then
                        If CSng(.text) > MaxValue Then MaxValue = CSng(.text)
                        If Not FoundMinValue Then MinValue = MaxValue: FoundMinValue = True
                      End If
                      
            Case "S": .CellBackColor = -1
                      If ColorRange < nRangesAnInf Then
                        .text = widind(AnInf(Location, ColorRange).styl)
                      Else
                        .text = LastStyleName
                      End If
                      
            Case ">": .CellBackColor = RGBinterpolate(MinColor, MaxColor, (ColorRange / (nRanges - 1)))
                      .text = ""
            Case Else: .CellBackColor = -1
          End Select
        Next
      Else
        For c = 0 To .cols - 1
          .col = c
          Select Case Left(.ColTitle(c), 1)
            Case "<": .CellBackColor = AnInf(Location, ColorRange).colr
                      .text = ""
            Case "V": .CellBackColor = -1
                      ColorRange = ColorRange + 1
                      .text = AnInf(Location, ColorRange).valu
            Case ">": .CellBackColor = AnInf(Location, ColorRange).colr
                      .text = ""
            Case "S": .CellBackColor = -1
                      .text = widind(AnInf(Location, ColorRange).styl)
            Case Else: .CellBackColor = -1
          End Select
        Next c
      End If
    Next r
  End With
End Sub

Public Sub SetAnimInfo(Optional MakeRamp As Long = 0)
  
  Dim Location As Long, ColorRange As Long, r&, c&, w&
  Dim Warned As Boolean, FoundMinValue As Boolean
  Dim MinColor As Long, MaxColor As Long
  Dim MinValue As Single, MaxValue As Single

  ReDim AnInf(ants - 1, nRanges - 1)
  nRangesAnInf = nRanges
  
  With agdSegments
    For Location = 0 To ants - 1
      If chkEditIndiv.value = vbChecked Then
        r = Location + 1
      Else
        r = 1 'always use first row
      End If
      .row = r
      
      If MakeRamp = Location + 1 Then
        ColorRange = 0
        FoundMinValue = False
        MaxValue = -999
        MinValue = -9999
        For c = 0 To .cols - 1
          .col = c
          Select Case Left(.ColTitle(c), 1)
            Case "<": MinColor = .CellBackColor
            Case "V":
                      If IsNumeric(.text) Then
                        If CSng(.text) > MaxValue Then MaxValue = CSng(.text)
                        If Not FoundMinValue Then MinValue = MaxValue: FoundMinValue = True
                      End If
                      
            Case ">": MaxColor = .CellBackColor
          End Select
        Next
      End If
      
      ColorRange = 0
      For c = 0 To .cols - 1
        .col = c
        Select Case Left(.ColTitle(c), 1)
          Case "<": AnInf(Location, ColorRange).colr = .CellBackColor
                    AnInf(Location, ColorRange).styl = DefaultStyle
          Case "V": ColorRange = ColorRange + 1
                    If MakeRamp = Location + 1 Then
                      AnInf(Location, ColorRange).valu = MinValue + (MaxValue - MinValue) * ((ColorRange - 1) / (nRanges - 2))
                      .text = AnInf(Location, ColorRange).valu
                    Else
                      If IsNumeric(.text) Then
                        AnInf(Location, ColorRange).valu = CSng(.text)
                      Else
                        AnInf(Location, ColorRange).valu = 0
                        .text = "0"
                        'If Not Warned Then MsgBox "Value " & ColorRange & " is not numeric.", vbOKOnly, "Animation Problem"
                        'Warned = True
                      End If
                    End If
                    AnInf(Location, ColorRange).styl = DefaultStyle
          Case ">": AnInf(Location, ColorRange).colr = .CellBackColor
                    If MakeRamp = Location + 1 And ColorRange < (nRanges - 1) Then
                      AnInf(Location, ColorRange).colr = RGBinterpolate(MinColor, MaxColor, (ColorRange / (nRanges - 1)))
                      .TextMatrix(r, c) = AnInf(Location, ColorRange).colr
                      '.CellBackColor = AnInf(Location, ColorRange).colr
                    End If
          Case "S":
                    For w = LBound(widind) To UBound(widind)
                      If .text = widind(w) Then AnInf(Location, ColorRange).styl = w
                    Next
        End Select
      Next
    Next Location
  End With
  AnimInfoChanged = False
End Sub

Private Sub PutSpecs(Filename)
  Dim fu%
  Dim recnum&, fieldNum&, cellColor&
  
  SetAnimInfo
  On Error GoTo Errs
  fu = FreeFile(0)
  Open Filename For Output As #fu
  Print #fu, "Animation Settings Version #2"
  Print #fu, ants, nRanges
  For recnum = 0 To ants - 1
    For fieldNum = 0 To nRanges - 1
      Print #fu, AnInf(recnum, fieldNum).valu
      Print #fu, AnInf(recnum, fieldNum).colr
      Print #fu, AnInf(recnum, fieldNum).styl
    Next
  Next
  Close #fu
  Exit Sub

Errs:
  MsgBox "Error writing animation specifications" & vbCr & err.Description, vbOKOnly, "Animate"
  err.clear
  On Error Resume Next
  Close #fu

'  'only put specs for indiv segments
'  saveedit = chkEditIndiv
'  If chkEditIndiv = vbUnchecked Then
'    chkEditIndiv = vbChecked
'  End If
'
'  fu = FreeFile(0)
'  Open Filename For Output As #fu
'  Print #fu, ants
'  For i = 1 To ants
'    'Set itmx = lvwSegments.ListItems(i)
'    For j = 1 To 8
'      Print #fu, agdSegments.TextMatrix(i, j) 'itmx.SubItems(j)
'    Next j
'  Next i
'  Close #fu
'
'  chkEditIndiv = saveedit


End Sub

Private Sub GetSpecs(Filename)
  Dim fu%, rval!, buf$
  Dim totalInFile As Long, nRangesInFile As Long
  Dim recnum&, fieldNum&, cellColor&
  Dim FileFormat As Long
  Dim FileHeader As String
  
  On Error GoTo Errs
  fu = FreeFile(0)
  Open Filename For Input As #fu
  Line Input #fu, FileHeader
  If IsNumeric(FileHeader) Then
    FileFormat = 1
    totalInFile = CLng(totalInFile)
    nRangesInFile = 3
  Else
    recnum = InStr(FileHeader, "#")
    If recnum > 0 Then buf = Mid(FileHeader, recnum + 1)
    If IsNumeric(buf) Then FileFormat = CLng(buf)
    Input #fu, totalInFile, nRangesInFile
  End If
  If FileFormat > 0 Then
    ReDim AnInf(totalInFile - 1, nRangesInFile - 1)
    txtRanges.value = nRangesInFile
    SetupGrid
    nRangesAnInf = nRangesInFile
    'If chkEditIndiv = vbUnchecked Then totalInFile = 1
    If totalInFile > ants Then totalInFile = ants
    For recnum = 0 To totalInFile - 1
      Select Case FileFormat
        Case 1: 'Line Input #fu, cname: AnInf(recnum, 1). = cname
        Case 2:
          For fieldNum = 0 To nRangesInFile - 1
            Input #fu, AnInf(recnum, fieldNum).valu
            Input #fu, AnInf(recnum, fieldNum).colr
            Input #fu, AnInf(recnum, fieldNum).styl
          Next
      End Select
    Next
    While recnum < ants 'fill in with last values if we now have more ants
      For fieldNum = 0 To nRangesInFile - 1
        AnInf(recnum, fieldNum).valu = AnInf(nRangesInFile - 1, fieldNum).valu
        AnInf(recnum, fieldNum).colr = AnInf(nRangesInFile - 1, fieldNum).colr
        AnInf(recnum, fieldNum).styl = AnInf(nRangesInFile - 1, fieldNum).styl
      Next
      recnum = recnum + 1
    Wend
  End If
  Close #fu
  AnimInfoToGrid
  SetAnimInfo
  Exit Sub
Errs:
  MsgBox "Error reading animation specifications" & vbCr & err.Description, vbOKOnly, "Animate"
  err.clear
  On Error Resume Next
  Close #fu
End Sub
'          .row = recnum
'          Line Input #fu, cname
'          .TextMatrix(recnum, 1) = cname
'          Line Input #fu, cname
'          .TextMatrix(recnum, 2) = cname
'          For fieldNum = 3 To 8
'            Line Input #fu, cname
'            .TextMatrix(recnum, fieldNum) = cname
'          Next fieldNum
'        Next recnum
'        If ants > totalInFile Then
'          For recnum = totalInFile + 1 To ants
'            For fieldNum = 1 To 8
'              .TextMatrix(recnum, fieldNum) = .TextMatrix(1, fieldNum)
'            Next fieldNum
'          Next recnum
'        End If
'      End With
'    Else
'      'need just the first line
'      'agdSegments.TextMatrix(1, 0) = "<all>"
'      With agdSegments
'        recnum = 1
'        .row = recnum
'        Line Input #fu, cname
'        .TextMatrix(recnum, 1) = cname
'        Line Input #fu, cname
'        .TextMatrix(recnum, 2) = cname
'        For fieldNum = 3 To 8
'          Line Input #fu, cname
'          .TextMatrix(recnum, fieldNum) = cname
'        Next fieldNum
'      End With

Private Sub AnimInit()
  Dim i&, oldTs&, testTser&, testAnimRch&
  Dim AnimShapeFiles$
  
  AnimShapeFiles = ""
  For i = 0 To Map2.LayerCount - 1
    If Map2.LayerAnimate(i) Then
      If Len(AnimShapeFiles) > 0 Then AnimShapeFiles = AnimShapeFiles & ","
      AnimShapeFiles = AnimShapeFiles & Map2.LayerPath(i) & Map2.LayerFilename(i) & ".shp"
    End If
  Next i
  ReadShapeLine AnimShapeFiles, AnimRch, Map2
  'fill in timeseries data values
  agdSegments.Rows = 1
  agdSegments.ClearData
  'Call GenScnEntry.FillReachDetails(TSer)
  
'  ConstInt = True
'  Set lTser = FillTimSerExt(TSer, ConstInt)
  
  Call GenScnEntry.FillTimSer(Tser)
  ReDim TsRch(Tser.Count)
  ants = 1
  
  Set lTs = Nothing
  Set lTs = New Collection
  'fill in list items
  For testTser = 1 To Tser.Count
    TsRch(testTser) = -1
    testAnimRch = 0
    Do
      If Tser(testTser).Header.loc = AnimRch(testAnimRch).Name Then
        TsRch(ants) = testAnimRch + 1
        For oldTs = 1 To ants - 1
          If TsRch(oldTs) = TsRch(ants) Then 'already have this one
            TsRch(ants) = -1
          End If
        Next
        Exit Do
      End If
      testAnimRch = testAnimRch + 1
    Loop While testAnimRch <= UBound(AnimRch)
    
    If TsRch(ants) >= 0 Then 'new match added
      lTs.Add Tser(testTser)
      
'      With agdSegments
'        .row = ants
'        .Col = 0
'        .TextMatrix(ants, 0) = TSer(testTser).Header.loc
'        .TextMatrix(ants, 1) = NumFmted(signif(0.25 * TSer(testTser).Max, False), 8, 1)
'        .TextMatrix(ants, 2) = NumFmted(signif(0.75 * TSer(testTser).Max, False), 8, 1)
'        .TextMatrix(ants, 3) = "Green"
'        .TextMatrix(ants, 4) = "Yellow"
'        .TextMatrix(ants, 5) = "Red"
'        .TextMatrix(ants, 6) = "Thin"
'        .TextMatrix(ants, 7) = "Thin"
'        .TextMatrix(ants, 8) = "Thin"
'      End With
      ants = ants + 1
    End If
  Next testTser
  ants = ants - 1
  AnimInfoChanged = True
End Sub

Private Sub agdSegments_CommitChange(ChangeFromRow As Long, ChangeToRow As Long, ChangeFromCol As Long, ChangeToCol As Long)
  Debug.Print "agdSegments_CommitChange: ", ChangeFromRow, ChangeToRow
  AnimInfoChanged = True
  If chkEditIndiv.value = vbChecked Then
    If Left(agdSegments.ColTitle(ChangeFromCol), 1) = "V" Then
      SetupGrid
    End If
  End If
End Sub

Private Sub agdSegments_RowColChange()
  Dim i&
  With agdSegments
    .ClearValues
    If Left(.ColTitle(.col), 5) = "Style" Then
      For i = LBound(widind) To UBound(widind)
        .addValue widind(i)
      Next i
    End If
  End With
End Sub

Private Sub cboDate_Click()
  If Not Animating Then
    stopfg = True
    Animate
  End If
End Sub

Private Sub cboStyle_Click()
  If cboStyle.ListIndex > 0 Then LastStyleName = cboStyle.text
  If cboStyle.ListIndex <> DefaultStyle Then
    If DefaultStyle = CustomStyle Or cboStyle.ListIndex = CustomStyle Then
      SetAnimInfo
      SetupGrid
      AnimInfoToGrid
    Else
      DefaultStyle = cboStyle.ListIndex
    End If
    SetAnimInfo
  End If
End Sub

Private Sub chkEditIndiv_Click()
  Static InClick As Boolean
  Dim r&, c&, ctemp() As String, colrTemp() As Long
  If Not InClick Then
    InClick = True
    Me.MousePointer = vbHourglass
    If chkEditIndiv.value = vbChecked Then
      chkEditIndiv.value = vbUnchecked
      SetAnimInfo
      chkEditIndiv.value = vbChecked
      agdSegments.Rows = ants
      SetupGrid
      AnimInfoToGrid
      With agdSegments
        For r = 1 To ants      'Set location names
          .TextMatrix(r, 0) = Tser(r).Header.loc
        Next r
        If Me.WindowState = vbNormal And .Height < 900 Then .Height = 1900: Form_Resize
      End With
    Else
      agdSegments.Rows = 1
      If Me.WindowState = vbNormal And agdSegments.Height > 900 Then
        agdSegments.Height = 850
        Form_Resize
      End If
      SetAnimInfo
      SetupGrid
      AnimInfoToGrid
      With agdSegments
      '  .Rows = 1
      '  If Me.WindowState = vbNormal And .Height > 900 Then .Height = 850: Form_Resize
      End With
    End If
    Me.MousePointer = vbDefault
    AnimInfoChanged = True
    InClick = False
  End If
End Sub

Private Sub cmdAnimate_Click(Index As Integer)
  Dim i%

  If ants = 0 Then
    MsgBox "No Selected Timeseries Have Available Line Coverages"
  Else
    If Index = 0 Then 'forward animation
      tsinc = 1
      stopfg = False
      Animate
    ElseIf Index = 1 Then 'rewind
      tsinc = -1
      stopfg = False
      Animate
    ElseIf Index = 2 Then 'pause animation
      tsinc = 0
      stopfg = True
    ElseIf Index = 3 Then 'stop animation
      stopfg = True
      Its = -1
      tsinc = 0
      cboDate.ListIndex = 0
      pctAnim.Visible = False
      Map2.Enabled = True
      Map2.Visible = True
    ElseIf Index = 4 Then 'single step forward
      tsinc = 1
      stopfg = True
      Animate
      tsinc = 0
    ElseIf Index = 5 Then 'single step backward
      tsinc = -1
      stopfg = True
      Animate
      tsinc = 0
    End If
  End If
End Sub

Private Sub cmdClear_Click()
  AnimInit
  If chkEditIndiv.value = vbChecked Then agdSegments.Rows = 1
End Sub

Private Sub cmdGet_Click()
  CDGet.flags = &H1806&
  CDGet.filter = "GenScn Animation Files (*.gan)|*.gan"
  CDGet.Filename = "*.gan"
  CDGet.DialogTitle = "GenScn Animation Get Specs File"
  On Error GoTo 10
  CDGet.CancelError = True
  CDGet.Action = 1
  Call GetSpecs(CDGet.Filename)
  Call SetAnimInfo
10 'continue here on cancel
End Sub

Private Sub cmdPrint_Click()
  If pctAnim.Visible Then
    ComPrt.ShowPrinter
  
    Printer.PaintPicture pctAnim.Image, 0, 0
  
    Printer.EndDoc
  Else
    Map2.MapAction 110
  End If
End Sub

Private Sub cmdProfile_Click()
  Dim tstr$(0), senstr$(0), s$, constr$(0)
  
  s = cboDate.List(cboDate.ListIndex)
  If IsDate(s) Then
    tstr(0) = Year(s) & "/" & Format$(Month(s), "00") & "/" & Format$(Day(s), "00")
    tstr(0) = tstr(0) & " " & Format$(Hour(s), "00") & ":" & Format$(Minute(s), "00") & ":" & Format(Second(s), "00")
  
    senstr(0) = lTs(1).Header.sen
    constr(0) = lTs(1).Header.con
    Call GenScnEntry.DoProfPlot(lTs, 1, tstr(), 1, senstr(), 1, constr(), 0, 0, 0, 1, PPinit, 1, "", "", "Location")
  Else
    MsgBox "Bad Date for Profile Plot."
  End If
End Sub

Private Sub cmdRamp_Click()
  SetAnimInfo agdSegments.row
End Sub

Private Sub cmdSave_Click()
  CDSave.flags = &H8806&
  CDSave.filter = "GenScn Animation Files (*.gan)|*.gan"
  CDSave.Filename = "*.gan"
  CDSave.DialogTitle = "GenScn Animation Save Specs File"
  On Error GoTo 10
  CDSave.CancelError = True
  CDSave.Action = 2
  Call PutSpecs(CDSave.Filename)
10 'continue here on cancel
End Sub

Private Sub cmdSaveMap_Click(Index As Integer)
  Map2.MapAction 80 + (10 * Index)
End Sub

Private Sub cmdSavePic_Click()
'  Dim ErrRet&
  Dim fname$
  If Not pctAnim.Visible Then
    Map2.MapSave
  Else
    fname = IPC.SavePictureDialog("")
    If fname <> "" Then IPC.SavePictureAs pctAnim, fname
'    On Error GoTo errhandler
'    ErrRet = 0
'    'get name from user
'    comMap.DialogTitle = "Generate Windows Metafile"
'    comMap.filter = "Windows Metafile Files (*.wmf)|*.wmf|All Files|*.*"
'    comMap.FilterIndex = 0
'    comMap.CancelError = True
'    comMap.ShowSave
'    fname = comMap.Filename
'
'    ErrRet = 1
'    If FileLen(fname) > 0 Then
'      'get rid of existing file
'      Kill fname
'    End If
'    ErrRet = 0
'BackFromErr3:
'    SavePicture pctAnim.Image, fname
  End If
'BackFromErr:
'  Exit Sub
'errhandler:
'  If ErrRet = 0 Then
'    Resume BackFromErr
'  Else
'    Resume BackFromErr3
'  End If
'  Resume BackFromErr

End Sub

Private Sub SetupGrid()
  Dim c As Long, r As Long, range As Long, onerow As Boolean, v As String
  With agdSegments
    c = -1
    DefaultStyle = cboStyle.ListIndex
    'set column headers
    If chkEditIndiv.value = vbChecked Then onerow = False Else onerow = True
    If onerow Then
      .ColEditable(0) = True
      .Rows = 1
    Else
      c = c + 1:   .ColTitle(c) = "Location":          .ColType(c) = ATCoCtl.ATCoTxt
      .ColEditable(0) = False
      .Rows = ants
    End If
    
    'If onerow Then
    '  v = .TextMatrix(1, c + 2)
    '  If Not IsNumeric(v) Then v = .TextMatrix(1, c + 3)
    '  If Not IsNumeric(v) Then v = "Val 1"
    'Else
      v = "Val 1"
    'End If
    c = c + 1:     .ColTitle(c) = "<= " & v:           .ColType(c) = ATCoCtl.ATCoClr
    
    If DefaultStyle = CustomStyle Then
      c = c + 1:   .ColTitle(c) = "Style <= " & v:     .ColType(c) = ATCoCtl.ATCoTxt
      For r = 1 To .Rows
        If .TextMatrix(r, c) = "" Then .TextMatrix(r, c) = LastStyleName
      Next
    End If
    For range = 1 To nRanges - 1
      c = c + 1:   .ColTitle(c) = "Value " & range:    .ColType(c) = ATCoCtl.ATCoSng
      'If onerow Then
      '  v = .TextMatrix(1, c)
      '  If Not IsNumeric(v) Then v = "Val " & range
      'Else
        v = "Val " & range
      'End If
      c = c + 1:   .ColTitle(c) = "> " & v:           .ColType(c) = ATCoCtl.ATCoClr
      If DefaultStyle = CustomStyle Then
        c = c + 1: .ColTitle(c) = "Style > " & v: .ColType(c) = ATCoCtl.ATCoTxt
        For r = 1 To .Rows
          If .TextMatrix(r, c) = "" Then .TextMatrix(r, c) = LastStyleName
        Next
      End If
    Next
    .cols = c + 1
    For c = 1 To .cols - 1
      .ColEditable(c) = True
    Next
    .ColsSizeByContents
  End With

End Sub

Private Sub Form_Load()
  Dim j&, c&, ldt&(5)
  Dim lsen$, lcon$
  Dim clmX As ColumnHeader
  Dim l As ListImage, i As ImageList, b As Button, k$, t As Variant
  
  Set Map2.IPC = IPC
  agdSegments.Visible = True

  Map2.Visible = True
  Map2.ButtonVisible("Identify") = False
  Map2.ButtonVisible("Add") = False
  Map2.ButtonVisible("Move") = False

  widind(1) = "Thin"
  widind(2) = "Medium Thin"
  widind(3) = "Medium"
  widind(4) = "Medium Heavy"
  widind(5) = "Heavy"

  cboStyle.AddItem "Custom"
  For j = LBound(widind) To UBound(widind)
    cboStyle.AddItem widind(j)
  Next
  CustomStyle = 0 '(cboStyle.ListCount - 1) * 2
  DefaultStyle = 1
  cboStyle.ListIndex = 1

  CopyMap frmGenScn.Map1

  'dont show location points on map by default
  For j = 0 To Map2.LayerCount - 1
    If Map2.map.Layers(j).symbol.SymbolType = moPointSymbol Then Map2.map.Layers(j).Visible = False
  Next j

  'set scneario and constituent for header
  lsen = Tser(1).Header.sen
  lcon = Tser(1).Header.con
  'lblLoc.Caption = "Locations Available for Scenario " & lsen & " and Constituent " & lcon
  
  If ants > 0 Then
    Dim curTs As Long, MinMin As Single, MaxMax As Single
    MinMin = lTs(1).Min
    If MinMin < 0 Then MinMin = 0
    MaxMax = lTs(1).Max
    For curTs = 2 To ants
      If lTs(curTs).Min < MinMin Then
        If lTs(curTs).Min >= 0 Then MinMin = lTs(curTs).Min
      End If
      If lTs(curTs).Max > MaxMax Then MaxMax = lTs(curTs).Max
    Next
    SetupGrid
    With agdSegments
      .Header = "Scenario " & lsen & " and Constituent " & lcon
      '.Height = 850
      .row = 1
      .col = .cols - 1
      .CellBackColor = vbBlack
      .col = 0
      .CellBackColor = vbWhite
      .TextMatrix(1, 1) = MinMin + (MaxMax - MinMin) / (agdSegments.cols / 2)
      .TextMatrix(1, .cols - 2) = MaxMax - (MaxMax - MinMin) / (agdSegments.cols / 2)
    End With
    cmdRamp_Click
    If lTs(1).dates.Summary.CIntvl Then
      For j = 1 To lTs(1).dates.Summary.NVALS
        Call J2Date(lTs(1).dates.value(j) - lTs(1).dates.Summary.Intvl, ldt)
        Select Case lTs(1).dates.Summary.Tu
          Case TUSecond:  cboDate.AddItem ldt(0) & "/" & ldt(1) & "/" & ldt(2) & " " & ldt(3) & ":" & Format(ldt(4), "00") & ":" & Format(ldt(5), "00")
          Case TUMinute:  cboDate.AddItem ldt(0) & "/" & ldt(1) & "/" & ldt(2) & " " & ldt(3) & ":" & Format(ldt(4), "00")
          Case TUHour:    cboDate.AddItem ldt(0) & "/" & ldt(1) & "/" & ldt(2) & " " & ldt(3) & ":" & "00"
          Case TUDay:     cboDate.AddItem ldt(0) & "/" & ldt(1) & "/" & ldt(2)
          Case TUMonth:   cboDate.AddItem ldt(0) & "/" & ldt(1)
          Case TUYear:    cboDate.AddItem ldt(0)
          Case TUCentury: cboDate.AddItem ldt(0)
        End Select
      Next
    Else
      For j = 1 To lTs(1).dates.Summary.NVALS
        Call J2Date(lTs(1).dates.value(j), ldt)
        'If lts(1).Type = "WDM" Then
        '  Call F90_TIMADD(CSDat(0), lts(0).Tu, lts(0).ts, j, ldt(0))
        'ElseIf lts(0).Type = "FEQ" Then
        '  Call J2Date(p.FeoData(lts(0).FilIndex).JDay(j), ldt())
        'Else 'should use approp jdate instead!!!!
        '  Call F90_TIMADD(CSDat(0), lts(0).Tu, lts(0).ts, j, ldt(0))
        'End If
        cboDate.AddItem ldt(0) & "/" & ldt(1) & "/" & ldt(2) & " " & ldt(3) & ":" & Format(ldt(4), "00") & ":" & Format(ldt(5), "00")
      Next j
    End If
    cboDate.ListIndex = 0
    PPinit = 1
    frmGenScnAnimate.Show
    cmdAnimate(0).Enabled = True
  Else 'no animation timeseries
    MsgBox "Timeseries for animation do not match known reaches", vbExclamation, "GenScn Animate Problem"
  End If

End Sub

Private Sub CopyMap(fromMap As ATCoMap)
  Dim AnimShapeFiles As String
    
  'get map info
GetDataFromMainMap:
  With fromMap
    .SaveMapFile .MapFilePath & "AnimateTemp.map"
    Map2.SetMapData .MapFilePath, "AnimateTemp.map", AnimShapeFiles 'triggers changedlayers which inits
    Kill .MapFilePath & "AnimateTemp.map"
  End With
  
  nRanges = txtRanges.value
  If AnimShapeFiles = "" Then      'animation info not specified
    If MsgBox("No layers have been selected for animation." & vbCr & "Do you want to enable animation for some map layer(s)?", vbYesNo, "GenScn Animate Problem") = vbYes Then
      fromMap.MapAction 300
      GoTo GetDataFromMainMap
    Else
      agdSegments.Visible = False
    End If
  End If
  MousePointer = vbDefault
  Map2.UnsavedChanges = False
End Sub

Private Sub Form_Resize()
  Static Resizing As Boolean
  
  If Not Resizing Then
    Resizing = True
    If WindowState <> 1 Then
      Dim MapVisible As Boolean
      Dim HeightBelowMap&
      MapVisible = Map2.Visible
      Map2.Visible = False
      If Me.Width < 5000 Then
        Me.Width = 5000
      End If
      If Me.Height < 6500 Then
        Me.Height = 6500
      End If
      fraCmds.Left = Me.Width - fraCmds.Width - 150
      Map2.Width = fraCmds.Left - 150
      pctAnim.Width = Map2.Width
      agdSegments.Width = Width - 200
      'chkEditIndiv.Left = agdSegments.Left + agdSegments.Width - chkEditIndiv.Width
      sash.Width = agdSegments.Width
      HeightBelowMap = agdSegments.Height + sash.Height + 400
      If Height > Map2.Top + HeightBelowMap Then
        Map2.Height = Height - (Map2.Top + HeightBelowMap) 'agdSegments.Top - 300 'lblLoc.Top - 300
        sash.Top = Map2.Top + Map2.Height
        agdSegments.Top = sash.Top + sash.Height 'Me.Height - agdSegments.Height - 500
        'chkEditIndiv.Top = agdSegments.Top
      End If
      pctAnim.Height = Map2.Height
      If Not MapVisible Then
        'pctAnim.Visible = False
        Map2.Visible = True
        'DoEvents
        DrawPicture
        Map2.Visible = False
        'pctAnim.Visible = True
        Animate
      Else
        Map2.Visible = MapVisible
      End If
    End If
    Resizing = False
  End If
End Sub

Private Sub Form_Unload(Cancel As Integer)

  stopfg = True
  If Map2.UnsavedChanges Then
    Select Case MsgBox("Save changes to map?", vbYesNoCancel)
      Case vbYes:    Map2.MapSave
      Case vbCancel: Cancel = 1
    End Select
  End If
  If Cancel = 0 Then
    Its = 0
    'Call PutSpecs(p.StatusFilePath & "\default.gan")
  End If
End Sub

Private Sub Map2_ChangedLayers()
  AnimInit
End Sub

Private Sub sash_MouseDown(Button As Integer, Shift As Integer, X As Single, Y As Single)
  SashDragging = True
End Sub

Private Sub sash_MouseUp(Button As Integer, Shift As Integer, X As Single, Y As Single)
  SashDragging = False
End Sub

Private Sub sash_MouseMove(Button As Integer, Shift As Integer, X As Single, Y As Single)
  Dim newHeight&
  If SashDragging And (sash.Top + Y) > 0 And (sash.Top + Y < Height) Then
    sash.Top = sash.Top + Y
    newHeight = Height - (sash.Top + sash.Height + 400)
    If newHeight > 0 Then agdSegments.Height = newHeight
    If Me.WindowState = vbNormal Then Form_Resize
  End If
End Sub

Private Sub txtRanges_Change()
  SetAnimInfo
  nRanges = txtRanges.value
  SetupGrid
  AnimInfoToGrid
  SetAnimInfo
'  Dim MaxValue() As String
'  Dim MaxColor() As String
'  Dim row&, Col&
'  With agdSegments
'    ReDim MaxValue(.Rows)
'    ReDim MaxColor(.Rows)
'    For row = 1 To .Rows
'      .row = row
'      For Col = 1 To .cols - 1
'        Select Case Left(.ColTitle(Col), 1)
'          Case ">": .Col = Col: MaxColor(row) = .CellBackColor
'          Case "V": .Col = Col: MaxValue(row) = .text
'        End Select
'      Next
'    Next
'    nRanges = txtRanges.value
'    SetupGrid
'    For row = 1 To .Rows
'      .row = row
'      Col = .cols - 1
'      Do
'        Select Case Left(.ColTitle(Col), 1)
'          Case ">": .TextMatrix(row, Col) = MaxColor(row)
'          Case "V": .TextMatrix(row, Col) = MaxValue(row): Exit Do
'        End Select
'        Col = Col - 1
'      Loop While Col > 0
'      SetAnimInfo row
'    Next
'    SetupGrid
'    SetAnimInfo
'  End With
End Sub

Private Function RGBinterpolate(clr1 As Long, clr2 As Long, percent) As Long
  Dim Redness1&, Redness2&
  Dim Greenness1&, Greenness2&
  Dim Blueness1&, Blueness2&
  
  Redness1 = clr1 And &HFF
  Greenness1 = (clr1 And 65280) / &H100 '65280 = FF00 = green, VB thinks FF00 = -256
  Blueness1 = (clr1 And &HFF0000) / &H10000
  
  Redness2 = clr2 And &HFF
  Greenness2 = (clr2 And 65280) / &H100 '65280 = FF00 = green, VB thinks FF00 = -256
  Blueness2 = (clr2 And &HFF0000) / &H10000
  RGBinterpolate = RGB(Redness1 + (Redness2 - Redness1) * percent, Greenness1 + (Greenness2 - Greenness1) * percent, Blueness1 + (Blueness2 - Blueness1) * percent)
End Function

