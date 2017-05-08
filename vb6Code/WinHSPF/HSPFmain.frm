VERSION 5.00
Object = "{831FDD16-0C5C-11D2-A9FC-0000F8754DA1}#2.0#0"; "MSCOMCTL.OCX"
Object = "{F9043C88-F6F2-101A-A3C9-08002B2F49FB}#1.2#0"; "comdlg32.ocx"
Object = "{872F11D5-3322-11D4-9D23-00A0C9768F70}#1.10#0"; "ATCoCtl.ocx"
Begin VB.Form HSPFMain 
   Caption         =   "Hydrological Simulation Program - Fortran (HSPF)"
   ClientHeight    =   6720
   ClientLeft      =   132
   ClientTop       =   612
   ClientWidth     =   12120
   BeginProperty Font 
      Name            =   "Arial"
      Size            =   7.8
      Charset         =   0
      Weight          =   400
      Underline       =   0   'False
      Italic          =   0   'False
      Strikethrough   =   0   'False
   EndProperty
   HelpContextID   =   33
   Icon            =   "HSPFmain.frx":0000
   LinkTopic       =   "Form1"
   ScaleHeight     =   560
   ScaleMode       =   3  'Pixel
   ScaleWidth      =   1010
   StartUpPosition =   3  'Windows Default
   Begin VB.PictureBox picBuffer 
      AutoRedraw      =   -1  'True
      BackColor       =   &H00FFFFFF&
      BorderStyle     =   0  'None
      FillStyle       =   0  'Solid
      FontTransparent =   0   'False
      Height          =   612
      Left            =   10920
      ScaleHeight     =   51
      ScaleMode       =   3  'Pixel
      ScaleWidth      =   91
      TabIndex        =   12
      Top             =   3600
      Visible         =   0   'False
      Width           =   1092
   End
   Begin VB.PictureBox picDetails 
      BorderStyle     =   0  'None
      Height          =   1572
      Left            =   0
      ScaleHeight     =   131
      ScaleMode       =   3  'Pixel
      ScaleWidth      =   961
      TabIndex        =   10
      Top             =   5160
      Width           =   11532
      Begin ATCoCtl.ATCoGrid agdDetails 
         Height          =   1524
         Left            =   0
         TabIndex        =   11
         Top             =   0
         Visible         =   0   'False
         Width           =   11532
         _ExtentX        =   20341
         _ExtentY        =   2709
         SelectionToggle =   -1  'True
         AllowBigSelection=   -1  'True
         AllowEditHeader =   0   'False
         AllowLoad       =   0   'False
         AllowSorting    =   0   'False
         Rows            =   2
         Cols            =   4
         ColWidthMinimum =   300
         gridFontBold    =   0   'False
         gridFontItalic  =   0   'False
         gridFontName    =   "Courier New"
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
         BackColorBkg    =   -2147483638
         BackColorSel    =   -2147483635
         ForeColorSel    =   -2147483634
         BackColorFixed  =   -2147483633
         ForeColorFixed  =   -2147483630
         InsideLimitsBackground=   -2147483643
         OutsideHardLimitBackground=   8421631
         OutsideSoftLimitBackground=   8454143
         ComboCheckValidValues=   0   'False
      End
   End
   Begin VB.Frame fraVsash 
      BackColor       =   &H8000000C&
      BorderStyle     =   0  'None
      Caption         =   "Frame1"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   5016
      Left            =   1680
      MousePointer    =   9  'Size W E
      TabIndex        =   6
      Top             =   0
      Width           =   96
   End
   Begin VB.PictureBox picTab 
      AutoRedraw      =   -1  'True
      BorderStyle     =   0  'None
      FillStyle       =   0  'Solid
      Height          =   4212
      Left            =   360
      ScaleHeight     =   351
      ScaleMode       =   3  'Pixel
      ScaleWidth      =   91
      TabIndex        =   5
      Top             =   600
      Width           =   1092
      Begin VB.CommandButton cmdScrollLegend 
         Caption         =   "^"
         Height          =   192
         Index           =   0
         Left            =   0
         TabIndex        =   9
         Top             =   0
         Visible         =   0   'False
         Width           =   1092
      End
      Begin VB.CommandButton cmdScrollLegend 
         Caption         =   "v"
         Height          =   192
         Index           =   1
         Left            =   0
         TabIndex        =   8
         Top             =   4020
         Visible         =   0   'False
         Width           =   1092
      End
      Begin VB.PictureBox picLegend 
         AutoRedraw      =   -1  'True
         BorderStyle     =   0  'None
         FillStyle       =   0  'Solid
         Height          =   480
         Index           =   0
         Left            =   0
         ScaleHeight     =   40
         ScaleMode       =   3  'Pixel
         ScaleWidth      =   90
         TabIndex        =   7
         Top             =   0
         Width           =   1080
      End
   End
   Begin VB.PictureBox picTree 
      AutoRedraw      =   -1  'True
      BackColor       =   &H8000000E&
      FillStyle       =   0  'Solid
      Height          =   4572
      Left            =   1800
      ScaleHeight     =   377
      ScaleMode       =   3  'Pixel
      ScaleWidth      =   807
      TabIndex        =   3
      Top             =   480
      Width           =   9732
      Begin VB.PictureBox picReach 
         AutoRedraw      =   -1  'True
         FillStyle       =   0  'Solid
         FontTransparent =   0   'False
         Height          =   612
         Index           =   0
         Left            =   240
         ScaleHeight     =   47
         ScaleMode       =   3  'Pixel
         ScaleWidth      =   87
         TabIndex        =   4
         Top             =   1680
         Width           =   1092
      End
   End
   Begin VB.Frame fraHsash 
      BackColor       =   &H8000000C&
      BorderStyle     =   0  'None
      Caption         =   "Frame1"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   96
      Left            =   0
      MousePointer    =   7  'Size N S
      TabIndex        =   2
      Top             =   5040
      Width           =   11172
   End
   Begin MSComctlLib.TabStrip tabLeft 
      Height          =   4452
      Left            =   0
      TabIndex        =   1
      Top             =   480
      Width           =   1572
      _ExtentX        =   2773
      _ExtentY        =   7853
      MultiRow        =   -1  'True
      Placement       =   2
      _Version        =   393216
      BeginProperty Tabs {1EFB6598-857C-11D1-B16A-00C0F0283628} 
         NumTabs         =   1
         BeginProperty Tab1 {1EFB659A-857C-11D1-B16A-00C0F0283628} 
            ImageVarType    =   2
         EndProperty
      EndProperty
      BeginProperty Font {0BE35203-8F91-11CE-9DE3-00AA004BB851} 
         Name            =   "Arial"
         Size            =   7.8
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
   End
   Begin MSComctlLib.ImageList imgMain 
      Left            =   0
      Top             =   120
      _ExtentX        =   995
      _ExtentY        =   995
      BackColor       =   -2147483643
      ImageWidth      =   32
      ImageHeight     =   32
      MaskColor       =   12632256
      _Version        =   393216
      BeginProperty Images {2C247F25-8591-11D1-B16A-00C0F0283628} 
         NumListImages   =   14
         BeginProperty ListImage1 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "HSPFmain.frx":08CA
            Key             =   "Open"
            Object.Tag             =   "Open"
         EndProperty
         BeginProperty ListImage2 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "HSPFmain.frx":11A6
            Key             =   "Create"
            Object.Tag             =   "Create"
         EndProperty
         BeginProperty ListImage3 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "HSPFmain.frx":1A82
            Key             =   "Save"
            Object.Tag             =   "Save"
         EndProperty
         BeginProperty ListImage4 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "HSPFmain.frx":235E
            Key             =   "Reach"
            Object.Tag             =   "Reach"
         EndProperty
         BeginProperty ListImage5 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "HSPFmain.frx":2C3A
            Key             =   "LandUse"
            Object.Tag             =   "LandUse"
         EndProperty
         BeginProperty ListImage6 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "HSPFmain.frx":3514
            Key             =   "Time"
            Object.Tag             =   "Time"
         EndProperty
         BeginProperty ListImage7 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "HSPFmain.frx":3DF0
            Key             =   "Control"
            Object.Tag             =   "Control"
         EndProperty
         BeginProperty ListImage8 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "HSPFmain.frx":46CC
            Key             =   "Pollutant"
            Object.Tag             =   "Pollutant"
         EndProperty
         BeginProperty ListImage9 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "HSPFmain.frx":4FA8
            Key             =   "Point"
            Object.Tag             =   "Point"
         EndProperty
         BeginProperty ListImage10 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "HSPFmain.frx":5882
            Key             =   "Default"
            Object.Tag             =   "Default"
         EndProperty
         BeginProperty ListImage11 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "HSPFmain.frx":615E
            Key             =   "Edit"
            Object.Tag             =   "Edit"
         EndProperty
         BeginProperty ListImage12 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "HSPFmain.frx":6A3A
            Key             =   "Output"
            Object.Tag             =   "Output"
         EndProperty
         BeginProperty ListImage13 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "HSPFmain.frx":7316
            Key             =   "Run"
            Object.Tag             =   "Run"
         EndProperty
         BeginProperty ListImage14 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "HSPFmain.frx":7BF2
            Key             =   "View"
            Object.Tag             =   "View"
         EndProperty
      EndProperty
   End
   Begin MSComctlLib.Toolbar tbrMain 
      Align           =   1  'Align Top
      Height          =   636
      Left            =   0
      TabIndex        =   0
      Top             =   0
      Width           =   12120
      _ExtentX        =   21378
      _ExtentY        =   1122
      ButtonWidth     =   1037
      ButtonHeight    =   995
      Appearance      =   1
      ImageList       =   "imgMain"
      _Version        =   393216
      MouseIcon       =   "HSPFmain.frx":84CE
   End
   Begin MSComDlg.CommonDialog CDFile 
      Left            =   8520
      Top             =   480
      _ExtentX        =   699
      _ExtentY        =   699
      _Version        =   393216
      FontSize        =   4.09255e-38
   End
   Begin VB.Menu mnuProject 
      Caption         =   "&File"
      Index           =   0
      Begin VB.Menu mnuNew 
         Caption         =   "&Create"
      End
      Begin VB.Menu mnuOpen 
         Caption         =   "&Open"
      End
      Begin VB.Menu mnuUnd1 
         Caption         =   "-"
         Index           =   0
      End
      Begin VB.Menu mnuConvert 
         Caption         =   "Conver&t NPSM"
      End
      Begin VB.Menu mnuUnd2 
         Caption         =   "-"
         Index           =   0
      End
      Begin VB.Menu mnuSave 
         Caption         =   "&Save"
      End
      Begin VB.Menu mnuSaveAs 
         Caption         =   "Save &As"
      End
      Begin VB.Menu mnuSaveDiagram 
         Caption         =   "Save &Diagram"
      End
      Begin VB.Menu mnuSaveLegend 
         Caption         =   "Save &Legend"
      End
      Begin VB.Menu mnuSaveGrid 
         Caption         =   "Save &Grid"
      End
      Begin VB.Menu mnuRecent 
         Caption         =   "-"
         Index           =   0
         Visible         =   0   'False
      End
      Begin VB.Menu mnuUnd 
         Caption         =   "-"
         Index           =   0
      End
      Begin VB.Menu mnuExit 
         Caption         =   "E&xit"
      End
   End
   Begin VB.Menu mnuEdit 
      Caption         =   "&Edit"
      Begin VB.Menu mnuEditOperParent 
         Caption         =   "Operation"
         Index           =   0
      End
   End
   Begin VB.Menu mnuPrint 
      Caption         =   "&Print"
      Visible         =   0   'False
      Begin VB.Menu mnuPrintTree 
         Caption         =   "Tree"
      End
      Begin VB.Menu mnuPrintLegend 
         Caption         =   "Legend"
      End
      Begin VB.Menu mnuPrintDetails 
         Caption         =   "Details"
      End
   End
   Begin VB.Menu mnuFunctions 
      Caption         =   "F&unctions"
      Begin VB.Menu mnuFunction 
         Caption         =   "&Reach"
         Index           =   0
      End
      Begin VB.Menu mnuFunction 
         Caption         =   "&Time"
         Index           =   1
      End
      Begin VB.Menu mnuFunction 
         Caption         =   "&LandUse"
         Index           =   2
      End
      Begin VB.Menu mnuFunction 
         Caption         =   "&Control"
         Index           =   3
      End
      Begin VB.Menu mnuFunction 
         Caption         =   "&Pollutant"
         Index           =   4
      End
      Begin VB.Menu mnuFunction 
         Caption         =   "Poin&T"
         Index           =   5
      End
      Begin VB.Menu mnuFunction 
         Caption         =   "&Default"
         Index           =   6
         Visible         =   0   'False
      End
      Begin VB.Menu mnuFunction 
         Caption         =   "&Edit"
         Index           =   7
      End
      Begin VB.Menu mnuFunction 
         Caption         =   "&Output"
         Index           =   8
      End
      Begin VB.Menu mnuFunction 
         Caption         =   "Ru&N"
         Index           =   9
      End
      Begin VB.Menu mnuView 
         Caption         =   "&View"
      End
      Begin VB.Menu mnuSpacer 
         Caption         =   "-"
      End
      Begin VB.Menu mnuBMP 
         Caption         =   "&BMP"
      End
      Begin VB.Menu mnuStarter 
         Caption         =   "&Starter"
      End
      Begin VB.Menu mnuHSPFParm 
         Caption         =   "&HSPFParm"
      End
      Begin VB.Menu mnuAQUATOX 
         Caption         =   "&AQUATOX"
      End
      Begin VB.Menu mnuPEST 
         Caption         =   "PEST"
      End
   End
   Begin VB.Menu mnuHelpMain 
      Caption         =   "&Help"
      Index           =   0
      Begin VB.Menu mnuHelp 
         Caption         =   "&Contents"
         Index           =   0
      End
      Begin VB.Menu mnuHelp 
         Caption         =   "&Index"
         Index           =   1
         Visible         =   0   'False
      End
      Begin VB.Menu mnuHelp 
         Caption         =   "&Search"
         Index           =   2
         Visible         =   0   'False
      End
      Begin VB.Menu mnuHelp 
         Caption         =   "&Debug"
         Index           =   3
      End
      Begin VB.Menu mnuUndX 
         Caption         =   "-"
      End
      Begin VB.Menu mnuSupport 
         Caption         =   "&Web Support"
         Index           =   1
      End
      Begin VB.Menu mnuUndY 
         Caption         =   "-"
      End
      Begin VB.Menu mnuAbout 
         Caption         =   "&About"
         Index           =   0
      End
      Begin VB.Menu mnuFeedback 
         Caption         =   "Send &Feedback"
      End
   End
End
Attribute VB_Name = "HSPFMain"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2002 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Private Enum LegendType
  LegLand = 0
  LegMet = 1
  LegPoint = 2
End Enum

Private OpTyps As Variant
Private BaseCaption As String
Private HsashDragging As Boolean
Private VsashDragging As Boolean
Private HsashDragStart As Single
Private VsashDragStart As Single
Private TreeDragging As Boolean
Private TreeStartX As Single, TreeStartY As Single
Private TreeRectX As Single, TreeRectY As Single
Private LegendScrollPos As Long, LegendFullHeight As Long
Private HighlightColor As Long

Private lOpns As Collection 'Set of operations pictured in the main tree diagram
Private LandSelected() As Boolean
Private MetSelected As Long
Private PointSelected As Long
Private ReachSelected() As Boolean
Private BeforeDragReachSelected() As Boolean 'Allow contracting drag selection

Private CurrentLegend As LegendType
Private LegendOrder As Collection
Private ColorMap As Collection 'of OLE_COLOR

Public W_EXEWINHSPF As String
Public W_HSPFMSGMDB As String
Public W_POLLUTANTLIST As String
Public W_HSPFMSGWDM As String
Public W_STARTERPATH As String
Public W_HSPFEngineExe As String

Private TreeProblemMessageDisplayed As Boolean

Public relabs&, basedsn&, newname$  'used in save as
Public MessageUnit As Long 'needed?

Private Sub InitColorMap()
  Set ColorMap = Nothing
  Set ColorMap = New Collection
  ColorMap.Add TextOrNumericColor("blue"), "WaterWetlands"
  ColorMap.Add TextOrNumericColor("blue"), "Water/Wetland"
  ColorMap.Add TextOrNumericColor("red"), "Construction"
  ColorMap.Add TextOrNumericColor("azure"), "Institutional"
  ColorMap.Add TextOrNumericColor("green"), "Agricultural"
  ColorMap.Add TextOrNumericColor("forestgreen1"), "Forest/Open"
  ColorMap.Add TextOrNumericColor("forestgreen1"), "Forest"
  ColorMap.Add TextOrNumericColor("brickred"), "Commercial"
  ColorMap.Add TextOrNumericColor("gray"), "Urban"
  ColorMap.Add TextOrNumericColor("goldenrod"), "LowResidential"
  ColorMap.Add TextOrNumericColor("orange"), "MedResidential"
  ColorMap.Add TextOrNumericColor("orange"), "Residential"
  ColorMap.Add TextOrNumericColor("orangered"), "MultResidential"
  ColorMap.Add TextOrNumericColor("darkbrown"), "HIGH TILL CROPLAND"
  ColorMap.Add TextOrNumericColor("brown"), "LOW TILL CROPLAND"
  ColorMap.Add TextOrNumericColor("lightgreen"), "Pasture"
  ColorMap.Add TextOrNumericColor("lemonchiffon"), "Hay"
  ColorMap.Add TextOrNumericColor("pink"), "Animal/Feedlot"
End Sub

Private Sub PopulateLandGrid()
  Dim rch&, luse&, S$, lName$, lId&
  Dim lOper As HspfOperation, vConn As Variant, lConn As HspfConnection
  Dim lDesc As String
  Dim AddedThisReach As Boolean, ReachNames$, LastName$
  Dim Ptotal!, Itotal!, PgrandTotal!, IgrandTotal!
  Dim t#
  Dim AreaUnits$
  
  If myUci.Name = "" Then Exit Sub
  If myUci.GlobalBlock.emfg = 1 Then
    AreaUnits = " (Acres)"
  Else
    AreaUnits = " (Hectares)"
  End If
  t = 0
  With agdDetails
    .Visible = False
    .ClearData
    .Header = ""
    .rows = 0
    
    .cols = 5
    .ColTitle(0) = "Land Use"
    .ColTitle(1) = "Reaches"
    .ColTitle(2) = "Implnd" & AreaUnits: .ColType(2) = ATCoTxt
    .ColTitle(3) = "Perlnd" & AreaUnits: .ColType(3) = ATCoTxt
    .ColTitle(4) = "Total" & AreaUnits:  .ColType(4) = ATCoTxt
    For luse = 0 To picLegend.Count - 1
      Ptotal = 0
      Itotal = 0
      ReachNames = ""
      LastName = ""
      If LegendSelected(luse) Then
        lDesc = picLegend(luse).tag
        .rows = .rows + 1
        .TextMatrix(.rows, 0) = lDesc
        For rch = 0 To picReach.Count - 1
          AddedThisReach = False
          If ReachSelected(rch) Then
            Set lOper = lOpns(CLng(picReach(rch).tag))
            For Each vConn In lOper.Sources
              Set lConn = vConn
              If lConn.Source.volname = "PERLND" Then
                If Not lConn.Source.Opn Is Nothing Then
                  If lDesc = lConn.Source.Opn.Description Then
                    Ptotal = Ptotal + lConn.MFact
                    GoSub AddReachName
                  End If
                End If
              ElseIf lConn.Source.volname = "IMPLND" Then
                If Not lConn.Source.Opn Is Nothing Then
                  If lDesc = lConn.Source.Opn.Description Then
                    Itotal = Itotal + lConn.MFact
                    GoSub AddReachName
                  End If
                End If
              End If
            Next
          End If
        Next
        If Len(ReachNames) < 2 Then
          .TextMatrix(.rows, 1) = ""
        Else 'remove final ", "
          .TextMatrix(.rows, 1) = Left(ReachNames, Len(ReachNames) - 2)
        End If
        .TextMatrix(.rows, 2) = NumFmted(Itotal, 8, 1)
        .TextMatrix(.rows, 3) = NumFmted(Ptotal, 8, 1)
        .TextMatrix(.rows, 4) = NumFmted(Ptotal + Itotal, 8, 1)
        PgrandTotal = PgrandTotal + Ptotal
        IgrandTotal = IgrandTotal + Itotal
      End If
    Next
    .rows = .rows + 1
    .TextMatrix(.rows, 0) = "Total"
    .TextMatrix(.rows, 1) = ""
    .TextMatrix(.rows, 2) = NumFmted(IgrandTotal, 8, 1)
    .TextMatrix(.rows, 3) = NumFmted(PgrandTotal, 8, 1)
    .TextMatrix(.rows, 4) = NumFmted(PgrandTotal + IgrandTotal, 8, 1)
'    For i = 0 To picReach.Count - 1
'      If ReachSelected(i) Then
'        Set lOper = lOpns(CLng(picReach(i).tag))
'        For Each vConn In lOper.Sources
'          Set lConn = vConn
'          'Debug.Print lConn.source.volname
'          If lConn.source.volname = "PERLND" Or lConn.source.volname = "IMPLND" Then
'            lDesc = lConn.source.Opn.Description
'            If LegendSelected(lDesc) Then
'              .rows = .rows + 1
'              .TextMatrix(.rows, 0) = lConn.source.volname & " " & lConn.source.volid
'              .TextMatrix(.rows, 1) = lDesc
'              .TextMatrix(.rows, 2) = lOper.Name & " " & lOper.id
'              .TextMatrix(.rows, 3) = lConn.MFact
'              t = t + lConn.MFact
'            End If
'          End If
'        Next vConn
'      End If
'    Next i
    .ColsSizeByContents
    .ColsSizeToWidth
    .Visible = True
'    lblTotal(0) = "Total: " & t
'    lblTotal(1).Visible = False
'    lblTotal(2).Visible = False
'    OrigTotal = t
  End With
  Exit Sub
AddReachName:
  If Not AddedThisReach Then
    If lOper.Name <> LastName Then
      LastName = lOper.Name
      ReachNames = ReachNames & LastName & " "
    End If
    ReachNames = ReachNames & lOper.Id & ", "
    AddedThisReach = True
  End If
  Return
End Sub

Private Sub PopulateMetGrid()
  Dim r&, cdata&, cSour&, cMfacP&, ctran&, cMfacR&
  Dim lmetseg As HspfMetSeg
  
  If myUci.Name = "" Then Exit Sub
  If myUci.MetSegs.Count = 0 Then Exit Sub
  
  If IsNumeric(picLegend(MetSelected).tag) Then
    Set lmetseg = myUci.MetSegs(CInt(picLegend(MetSelected).tag))
  Else
    Set lmetseg = myUci.MetSegs(1)
  End If
  With agdDetails
    .Visible = False
    .ClearData
    .Header = ""
    .rows = 7
    cdata = 0
    cSour = 1
    cMfacP = 2
    cMfacR = 3
    ctran = 4
    .cols = 5
    .ColTitle(cdata) = "Data Type": .ColType(0) = ATCoTxt
    .ColTitle(cSour) = "Source":    .ColType(1) = ATCoTxt
    .ColTitle(cMfacP) = "P/I MFact":   .ColType(2) = ATCoSng
    .ColTitle(cMfacR) = "R MFact":   .ColType(3) = ATCoSng
    .ColTitle(ctran) = "Tran":      .ColType(4) = ATCoTxt
  End With
  
  For r = 1 To 7
    With lmetseg.MetSegRec(r)
      Select Case r
        Case 1: agdDetails.TextMatrix(r, cdata) = "Precip"
        Case 2: agdDetails.TextMatrix(r, cdata) = "Air Temp"
        Case 3: agdDetails.TextMatrix(r, cdata) = "Dew Point"
        Case 4: agdDetails.TextMatrix(r, cdata) = "Wind"
        Case 5: agdDetails.TextMatrix(r, cdata) = "Solar Rad"
        Case 6: agdDetails.TextMatrix(r, cdata) = "Cloud"
        Case 7: agdDetails.TextMatrix(r, cdata) = "Pot Evap"
      End Select
      If .Typ = 0 Then
        agdDetails.TextMatrix(r, cSour) = ""
        agdDetails.TextMatrix(r, cMfacP) = ""
        agdDetails.TextMatrix(r, cMfacR) = ""
        agdDetails.TextMatrix(r, ctran) = ""
      Else
        agdDetails.TextMatrix(r, cSour) = .Source.volname & " " & .Source.volid
        agdDetails.TextMatrix(r, cMfacP) = .MFactP
        agdDetails.TextMatrix(r, cMfacR) = .MFactR
        agdDetails.TextMatrix(r, ctran) = .Tran
      End If
    End With
  Next
  agdDetails.ColsSizeToWidth
  agdDetails.Visible = True
End Sub

Private Sub PopulatePointGrid()
  Dim r&, cdata&, cSour&, cMfac&, ctran&
  Dim lPoint As HspfPoint, i&, lcon$, vpol As Variant, lpol$
  
  If myUci.Name = "" Then Exit Sub
  If myUci.PointSources.Count = 0 Then Exit Sub
  
  With agdDetails
    .Visible = False
    .ClearData
    .Header = ""
    .rows = 1
    .cols = 2
    .ColTitle(0) = "Point Source": .ColType(0) = ATCoTxt
    .ColTitle(1) = "Constituent": .ColType(1) = ATCoTxt
  End With
  
  If IsNumeric(picLegend(PointSelected).tag) Then
    Set lPoint = myUci.PointSources(CInt(picLegend(PointSelected).tag))
  Else
    Set lPoint = myUci.PointSources(1)
  End If
  
  i = 0
  For Each lPoint In myUci.PointSources
    If lPoint.Id = PointSelected + 1 Then
      i = i + 1
      With agdDetails
        If .rows < i Then .rows = .rows + 1
        .TextMatrix(.rows, 0) = lPoint.Name
        lcon = lPoint.con
        If Len(lcon) > 0 Then
          'look for this con in pollutant list
          For Each vpol In PollutantList
            lpol = vpol
            If Mid(lcon, 1, 5) = Mid(lpol, 1, 5) Then
              lcon = lpol
              Exit For
            End If
          Next vpol
        End If
        .TextMatrix(.rows, 1) = lcon
      End With
    End If
  Next
  agdDetails.ColsSizeByContents
  agdDetails.Visible = True
End Sub

Private Sub SetLegendScrollButtons()
  
  If LegendScrollPos > 0 Then
    cmdScrollLegend(0).Visible = True
  Else
    cmdScrollLegend(0).Visible = False
  End If

  If LegendFullHeight - LegendScrollPos > picTab.Height Then
    cmdScrollLegend(1).Visible = True
  Else
    cmdScrollLegend(1).Visible = False
  End If
  
End Sub

Private Sub ScrollLegend()
  Dim boxHeight As Long, txtHeight As Long, ypos As Long, Index As Long
  txtHeight = picTab.TextHeight("X")
  boxHeight = txtHeight * 2
  ypos = txtHeight
  If LegendFullHeight - picTab.Height < LegendScrollPos Then
    LegendScrollPos = LegendFullHeight - picTab.Height
  End If
  If LegendScrollPos < 0 Then LegendScrollPos = 0
  For Index = 0 To picLegend.Count - 1
    picLegend(Index).Top = ypos - LegendScrollPos
    ypos = ypos + picLegend(Index).Height
  Next
  SetLegendScrollButtons
End Sub

Public Sub UpdateDetails()
  Select Case CurrentLegend
    Case LegLand:  PopulateLandGrid
    Case LegMet:   PopulateMetGrid
    Case LegPoint: PopulatePointGrid
    Case Else
      agdDetails.Visible = False
  End Select
End Sub

Public Sub UpdateLegend()
  Dim item As Variant
  Dim Key As String
  Dim Index As Long, srch As Long, oprindex As Long
  Dim colr As Long, i&, S$
  Dim xpos As Long, ypos As Long, colonpos As Long
  Dim boxHeight As Long, boxWidth As Long, txtHeight As Long
  Dim lOper As HspfOperation, maxpic As Long, tname$, spos&, cpos&
  
  Set LegendOrder = Nothing
  
  picTab.Cls
  For Index = 0 To picLegend.Count - 1
    With picLegend(Index)
      .tag = ""
      .Cls
      .Visible = False
      .Width = picTab.Width
    End With
  Next
  
  boxWidth = picTab.Width * 0.4
  txtHeight = picTab.TextHeight("X")
  boxHeight = txtHeight * 2
  Index = 0
  ypos = 0 'boxHeight + txtHeight
  maxpic = 0
  picTab.CurrentY = ypos
  Select Case CurrentLegend
    Case LegLand
      Set LegendOrder = New Collection
      picTab.CurrentX = 0
      picTab.CurrentY = 0
      picTab.Font.size = 8
      If picTab.TextWidth("Perlnd   Implnd") > picTab.Width Then
        picTab.CurrentX = (boxWidth - picTab.TextWidth("Per")) / 2
        picTab.Print " Per";
        picTab.CurrentX = picTab.Width - ((boxWidth + picTab.TextWidth("Imp")) / 2)
        picTab.Print "Imp "
      Else
        picTab.CurrentX = (boxWidth - picTab.TextWidth("Perlnd")) / 2
        picTab.Print " Perlnd";
        picTab.CurrentX = picTab.Width - ((boxWidth + picTab.TextWidth("Implnd")) / 2)
        picTab.Print "Implnd "
      End If
      ypos = txtHeight
      picLegend(0).Top = ypos
      If myUci.Name <> "" Then
      For oprindex = 1 To myUci.OpnSeqBlock.Opns.Count
        Set lOper = myUci.OpnSeqBlock.Opn(oprindex)
        If lOper.Name = "PERLND" Or lOper.Name = "IMPLND" Then
          Key = lOper.Description
          colonpos = InStr(Key, ":")
          If colonpos > 0 Then Key = Mid(Key, colonpos + 1)
          For Index = 0 To picLegend.Count - 1
            If Key = picLegend(Index).tag Or picLegend(Index).tag = "" Then Exit For
          Next
          If Index >= picLegend.Count Then
            load picLegend(Index)
            picLegend(Index).tag = ""
            picLegend(Index).Width = picTab.Width
          End If
          With picLegend(Index)
            If Index > maxpic Then maxpic = Index
            .Height = boxHeight * 1.5 + txtHeight
            If .tag = "" Then
              .tag = Key
              .Top = ypos - LegendScrollPos
              ypos = ypos + .Height
              .Visible = True
            End If
            .CurrentX = (.Width - .TextWidth(Key)) / 2
            If .CurrentX < 2 Then .CurrentX = 2
            .CurrentY = boxHeight * 1.5
            picLegend(Index).Print Key
            
            On Error GoTo ColorError
            colr = ColorMap(Key)
            On Error GoTo 0
            .CurrentY = boxHeight / 4
            If lOper.Name = "PERLND" Then .CurrentX = 0 Else .CurrentX = .Width - boxWidth
            picLegend(Index).Line -Step(boxWidth, boxHeight), colr, BF
          End With
        End If
      Next
      End If
      LegendFullHeight = ypos
      ReDim Preserve LandSelected(maxpic)
      For Index = 0 To picLegend.Count - 1
        If picLegend(Index).tag <> "" Then LegendOrder.Add picLegend(Index).tag
      Next Index
    Case LegMet
      Dim lmetseg As HspfMetSeg
      For Each lmetseg In myUci.MetSegs
        If Index >= picLegend.Count Then load picLegend(Index)
        With picLegend(Index)
          .tag = Index + 1
          i = InStr(1, lmetseg.Name, ",")
          If i > 0 Then
            S = Mid(lmetseg.Name, 1, i - 1) & vbCr & Mid(lmetseg.Name, i + 1)
          Else
            S = lmetseg.Name
          End If
          Key = lmetseg.Id & ":" & S
          .CurrentX = (.Width - .TextWidth(Key)) / 2
          .CurrentY = (.Height - .TextHeight(Key)) / 2
          picLegend(Index).Print Key  'Precip location name might be nicer
          If Index > 0 Then .Top = picLegend(Index - 1).Top + picLegend(Index - 1).Height
          .Visible = True
        End With
        Index = Index + 1
      Next
      LegendFullHeight = picLegend(0).Height * (Index + 1)
    Case LegPoint
      Dim lPoint As HspfPoint
      For Each lPoint In myUci.PointSources
        Index = lPoint.Id - 1
        If Index >= picLegend.Count Then load picLegend(Index)
        With picLegend(Index)
          .tag = Index + 1
          'find a way to shorten name
          spos = InStr(1, lPoint.Name, " ")
          cpos = InStr(1, lPoint.Name, ",")
          If spos < 10 And spos > 2 Then
            tname = Left(lPoint.Name, spos - 1)
          ElseIf cpos < 10 And cpos > 2 Then
            tname = Left(lPoint.Name, cpos - 1)
          Else
            tname = Left(lPoint.Name, 10)
          End If
          Key = lPoint.Id & ":" & tname
          .CurrentX = (.Width - .TextWidth(Key)) / 2
          .CurrentY = (.Height - .TextHeight(Key)) / 2
          picLegend(Index).Print Key
          If Index > 0 Then .Top = picLegend(Index - 1).Top + picLegend(Index - 1).Height
          .Visible = True
        End With
        Index = Index + 1
      Next
      LegendFullHeight = picLegend(0).Height * (Index + 1)
    Case Else
  End Select
  SetLegendScrollButtons
  RefreshLegendSelections
  UpdateDetails
  Exit Sub
ColorError:
  colr = vbBlack
  Err.Clear
  Resume Next
End Sub

Private Sub cmdScrollLegend_Click(Index As Integer)
  If Index = 0 Then
    LegendScrollPos = LegendScrollPos - picTab.Height / 4
  Else
    LegendScrollPos = LegendScrollPos + picTab.Height / 4
  End If
  If LegendScrollPos < 0 Then LegendScrollPos = 0
  If LegendScrollPos > LegendFullHeight Then LegendScrollPos = LegendFullHeight
  ScrollLegend
End Sub

Private Sub AddToolButton(buttName$, TipText$)
  tbrMain.Buttons.Add tbrMain.Buttons.Count + 1, buttName, , , buttName
  tbrMain.Buttons(tbrMain.Buttons.Count).tooltiptext = TipText
End Sub
Private Sub InitToolbar()
  tbrMain.ImageList = imgMain
  AddToolButton "Create", "Create New Project"
  AddToolButton "Open", "Open Existing Project"
  AddToolButton "Save", "Save Current Project"
  tbrMain.Buttons.Add tbrMain.Buttons.Count + 1, "Dummy", , tbrSeparator
  AddToolButton "Reach", "Reach Properties Editor"
  AddToolButton "Time", "Simulation Time and Meteorological Data"
  AddToolButton "LandUse", "Land Use Editor"
  AddToolButton "Control", "Control Cards"
  AddToolButton "Pollutant", "Pollutant Selection"
  AddToolButton "Point", "Point Sources"
  'AddToolButton "Default", "Default Data Assignment"
  AddToolButton "Edit", "Input Data Editor"
  AddToolButton "Output", "Output Manager"
  AddToolButton "Run", "Run HSPF"
  AddToolButton "View", "View Output through GenScn"
End Sub

Private Sub Form_Load()
  Dim commandline$, clen&, M$
Debug.Print "Starting HSPFMain at " & Time
  
  InitColorMap
  
  Set myMsgBox = New ATCoMessage
  Set myMsgBox.Icon = Me.Icon
    
  Call SetFiles
  
  'group here (with hspf manual) ??
  BaseCaption = Caption
  
  'MsgBox "W_EXEWINHSPF = " & W_EXEWINHSPF
  'MsgBox "ChDrive " & Left(W_EXEWINHSPF, 2)
  'ChDrive Left(W_EXEWINHSPF, 2)
  'MsgBox "ChDir " & Mid(W_EXEWINHSPF, 3, Len(W_EXEWINHSPF) - Len("C:winhspf.exe"))
  ChDriveDir Left(W_EXEWINHSPF, Len(W_EXEWINHSPF) - Len("winhspf.exe"))
  'MsgBox "Launch Monitor " & W_EXESTATUS
  Set IPC = New ATCoIPC
  With IPC
    .SendMonitorMessage "(OPEN WinHSPF Status Monitor)"
    .SendMonitorMessage "(LogToFile " & LogFileName & ")"
    .SendMonitorMessage "(Msg1 Initializing WinHSPF)"
    
    F90_W99OPN
    F90_WDBFIN
    F90_PUTOLV 10
    F90_SPIPH .hPipeReadFromProcess(0), .hPipeWriteToProcess(0) ' .ComputeRead, .ComputeWrite '.ComputeReadFromParent, .ComputeWriteToParent
    MessageUnit = F90_WDBOPN(1, W_HSPFMSGWDM, Len(W_HSPFMSGWDM))
    .SendMonitorMessage "(BUTTOFF OUTPUT)"
    .SendMonitorMessage "(BUTTOFF PAUSE)"
    .SendMonitorMessage "(BUTTOFF CANCEL)"
    'MsgBox "myMsg.name = " & W_HSPFMSGMDB
    .SendMonitorMessage "(Msg1 Initializing HspfMsg)"
    Set myMsg = New HspfMsg
    Set myMsg.Monitor = IPC
    myMsg.Name = W_HSPFMSGMDB
    .SendMonitorMessage "(Msg1 Opening Default UCI 'starter.uci')"
  End With
  StartHSPFEngine
  OpenDefaultUCI W_STARTERPATH, "starter.uci"
  'IPC.SendMonitorMessage "(Successfully Read Default UCI)"
  
  Set myUci = New HspfUci
  myUci.HelpFile = App.HelpFile
  myUci.StarterPath = W_STARTERPATH
  Set myUci.Monitor = IPC
  
  RetrieveWindowSettings Me, gAppName
  RetrieveRecentFiles mnuRecent, gAppName
  
  picReach(0).BorderStyle = 0 'no border
  picTree.Visible = False
  picTree.PSet (0, 0), vbHighlight
  ReDim Preserve ReachSelected(1)
  HighlightColor = picTree.Point(0, 0)

  fraHsash.BackColor = vbButtonFace
  fraVsash.BackColor = vbButtonFace
  
  InitToolbar
    
  ReDim LandSelected(0)
  MetSelected = 0 '=Index of legend picture box, index and ID of met = this + 1
  'ReDim PointSelected(0)
  With tabLeft
    .Tabs.Clear 'need keyboard shortcuts, & shows up on tab (but works), different font?
    .Tabs.Add 1, , "Land Surface": .Tabs(1).tag = LegLand
    .Tabs.Add 2, , "Met Segs":     .Tabs(2).tag = LegMet
    .Tabs.Add 3, , "Point Sources":  .Tabs(3).tag = LegPoint
  End With
  OpTyps = Array("RCHRES", "BMPRAC") 'for standard tree
  Me.Height = 5970
  
  SetEditMenu
  ReadPollutantList
  
  'IPC.SendMonitorMessage "(CLOSE)"
  IPC.SendMonitorMessage "(BUTTON OUTPUT)"
  IPC.SendMonitorMessage "(BUTTON PAUSE)"
  IPC.SendMonitorMessage "(BUTTON CANCEL)"
  
  TreeProblemMessageDisplayed = False
  
  If FileExists(BASINSPath & "\modelout", True, False) Then
    ChDriveDir BASINSPath & "\modelout"
  End If

Debug.Print "Finishing HSPFMain at " & Time

End Sub

Private Sub Form_Resize()
  Dim w&, h&
  Static LegendWidth&
  h = Me.ScaleHeight
  w = Me.ScaleWidth
  'dont do if minimized
  If h > tbrMain.Top + tbrMain.Height * 2 And w > fraVsash.Left + fraVsash.Width Then
    On Error Resume Next
    picTree.Top = tbrMain.Height
    tabLeft.Top = picTree.Top
    tabLeft.Width = fraVsash.Left
    fraVsash.Top = picTree.Top
    picDetails.Top = h - picDetails.Height
    agdDetails.Height = picDetails.Height
    fraHsash.Top = picDetails.Top - fraHsash.Height
    picTree.Left = fraVsash.Left + fraVsash.Width
    If fraHsash.Top > picTree.Top Then picTree.Height = fraHsash.Top - picTree.Top
    fraVsash.Height = picTree.Height
    tabLeft.Height = picTree.Height
      picTab.Top = tabLeft.ClientTop + 5 'tabLeft.top + 8
      picTab.Left = tabLeft.ClientLeft + 5
      picTab.Height = tabLeft.ClientHeight - 10
      picTab.Width = tabLeft.ClientWidth - 10
        cmdScrollLegend(0).Width = picTab.Width
        cmdScrollLegend(1).Width = picTab.Width
    fraHsash.Width = w
    picTree.Width = w - fraVsash.Left - fraVsash.Width
    'lblScenario.Left = picTree.Left + fraVsash.Width
    picDetails.Width = w
    agdDetails.Width = w
    If tabLeft.Height > 34 Then
      'picTab.height = tabLeft.height - 20
      cmdScrollLegend(1).Top = picTab.Height - cmdScrollLegend(1).Height
      ScrollLegend
    End If
    On Error GoTo 0
    If Len(myUci.Name) > 0 Then
      If picTab.Width <> LegendWidth Then
        UpdateLegend
        LegendWidth = picTab.Width
      End If
      BuildTree
    End If
  End If
ExitSub:
End Sub

Private Sub Form_Unload(Cancel As Integer)
  Dim frm As Variant
  SaveWindowSettings Me, gAppName
  SaveRecentFiles mnuRecent, gAppName
  IPC.SendMonitorMessage "(EXIT)"
  IPC.SendProcessMessage "HSPFUCI", "EXIT"
  For Each frm In Forms
    If frm.Name <> Me.Name Then
      Unload frm
      Set frm = Nothing
    End If
  Next frm
End Sub

Private Function LegendSelected(ByVal tag As String) As Boolean
  Dim i&
  LegendSelected = False
  If IsNumeric(tag) Then
    'If picLegend(CInt(tag)).Point(0, 0) = HighlightColor Then LegendSelected = True
    Select Case CurrentLegend
    Case LegLand:
      If CInt(tag) <= UBound(LandSelected) Then
        LegendSelected = LandSelected(CInt(tag))
      End If
    Case LegMet:  If CInt(tag) = MetSelected Then LegendSelected = True
    End Select
  Else
    For i = 0 To picLegend.Count - 1
      If tag = picLegend(i).tag Then
        If picLegend(i).Point(0, 0) = HighlightColor Then LegendSelected = True
        Exit Function
      End If
    Next
  End If
End Function

Private Sub mnuAQUATOX_Click()
  doAction "AQUATOX", "Link to AQUATOX"
End Sub

Private Sub mnuBMP_Click()
  doAction "BMP", "Edit Best Management Practices"
End Sub

Private Sub mnuConvert_Click()
  doAction "Convert", "Convert NPSM to WinHSPF"
End Sub

Private Sub mnuEditOperParent_Click(Index As Integer)
  Dim tabname$, i&
  
  If Len(myUci.Name) < 1 Then
    DisableAll True
    myMsgBox.Show "No Project is active." & vbCrLf & vbCrLf & _
                  "Open or Create a Project before selecting this menu item.", _
                  "WinHSPF: Edit Problem", "+-&Close"
    DisableAll False
  Else
    tabname = mnuEditOperParent(Index).Caption
    If myUci.MetSegs.Count > 0 Then
      myUci.MetSeg2Source
    End If
    myUci.Point2Source

    If tabname = "GLOBAL" Then
      myUci.GlobalBlock.Edit
    ElseIf tabname = "OPN SEQUENCE" Then
      myUci.OpnSeqBlock.Edit
    ElseIf tabname = "FILES" Then
      myUci.filesblock.Edit
    ElseIf tabname = "CATEGORY" And Not myUci.categoryblock Is Nothing Then
      myUci.categoryblock.Edit
    ElseIf tabname = "FTABLES" Then
      myUci.OpnBlks("RCHRES").Ids(1).Ftable.Edit
    ElseIf tabname = "MONTH-DATA" And Not myUci.MonthData Is Nothing Then
      myUci.MonthData.Edit
    ElseIf tabname = "EXT SOURCES" Then
      myUci.Connections(1).EditExtSrc
    ElseIf tabname = "NETWORK" Then
      myUci.Connections(1).EditNetwork
    ElseIf tabname = "SCHEMATIC" Then
      myUci.Connections(1).EditSchematic
    ElseIf tabname = "EXT TARGETS" Then
      myUci.Connections(1).EditExtTar
    ElseIf tabname = "MASS-LINK" Then
      myUci.MassLinks(1).Edit
    ElseIf tabname = "SPEC-ACTIONS" And Not myUci.SpecialActionBlk Is Nothing Then
      myUci.SpecialActionBlk.Edit
    Else
      DisableAll True
      myMsgBox.Show "Table/Block " & tabname & " not found.", "Edit Problem", "+&Ok"
      DisableAll False
    End If
    myUci.Source2MetSeg
    myUci.Source2Point

    ClearTree
    BuildTree
    UpdateLegend
    UpdateDetails
  End If
End Sub

Private Sub mnuFeedback_Click()
  Dim feedback As clsATCoFeedback
  Set feedback = New clsATCoFeedback
  feedback.AddFile Left(App.path, InStr(4, App.path, "\")) & "unins000.dat"
  feedback.AddText StatusString(False)
  feedback.AddText WholeFileString(LogFileName)
  feedback.Show App, Me.Icon
End Sub

Private Sub mnuHSPFParm_Click()
  frmHspfParm.Show vbModal
End Sub

Private Sub mnuPEST_Click()
  doAction "PEST", "Link to PEST"
End Sub

Private Sub mnuPrintDetails_Click()
  If Len(myUci.Name) < 1 Then
    DisableAll True
    myMsgBox.Show "No Project is active." & vbCrLf & vbCrLf & _
                  "Open or Create a Project before selecting this menu item.", _
                  "WinHSPF: Edit Problem", "+-&Close"
    DisableAll False
  Else
    agdDetails.SaveGridInteractive
  End If
End Sub

Private Sub mnuPrintLegend_Click()
  DisableAll True
  myMsgBox.Show "Option 'Print Legend' is not yet implemented.", "Not Yet Implemented", "+-&Close"
  DisableAll False
End Sub

Private Sub mnuPrintTree_Click()
  If Len(myUci.Name) < 1 Then
    DisableAll True
    myMsgBox.Show "No Project is active." & vbCrLf & vbCrLf & _
                  "Open or Create a Project before selecting this menu item.", _
                  "WinHSPF: Edit Problem", "+-&Close"
    DisableAll False
  Else
    BuildTree True
  End If
End Sub

Private Sub mnuRecent_Click(Index As Integer)
  Dim newFilePath$
  newFilePath = mnuRecent(Index).tag
  If Index > 0 Then
    OpenUCI (newFilePath)
  End If
End Sub

Private Sub mnuSaveDiagram_Click()
  Dim r&
  
  For r = 0 To picReach.Count - 1
    picTree.PaintPicture picReach(r).Image, picReach(r).Left, picReach(r).Top
    picReach(r).Visible = False
  Next r
  IPC.SavePictureAs picTree, IPC.SavePictureDialog("untitled", "=Save Diagram As...")
  For r = 0 To picReach.Count - 1
    picReach(r).Visible = True
  Next r
    
End Sub

Private Sub mnuSaveGrid_Click()
  agdDetails.SaveGridInteractive
End Sub

Private Sub mnuSaveLegend_Click()
  Dim l&
  picTab.Cls
  For l = 0 To picLegend.Count - 1
    picTab.PaintPicture picLegend(l).Image, picLegend(l).Left, picLegend(l).Top
    picLegend(l).Visible = False
  Next l
  IPC.SavePictureAs picTab, IPC.SavePictureDialog("untitled", "=Save Diagram As...")
  For l = 0 To picReach.Count - 1
    picLegend(l).Visible = True
  Next l
End Sub

Private Sub mnuStarter_Click()
  If Len(myUci.Name) > 0 Then
    frmStarter.Show vbModal
  Else
    DisableAll True
    myMsgBox.Show "No Project is active." & vbCrLf & vbCrLf & _
                      "Open or Create a Project before using the Starter feature.", _
                      "WinHSPF: 'Starter' Problem", "+-&Close"
    DisableAll False
  End If
End Sub

Private Sub picLegend_Click(Index As Integer)
  Dim tmp As Integer, rch As Long
  Dim vConn As Variant, i&
  Dim lConn As HspfConnection
  Select Case CurrentLegend
    Case LegLand
      LandSelected(Index) = Not LandSelected(Index)
      DrawLegendSelectBox Index, LandSelected(Index)
    Case LegMet
      Dim lOper As HspfOperation
      If MetSelected <> Index Then
        DrawLegendSelectBox MetSelected, False
        MetSelected = Index
      End If
      DrawLegendSelectBox MetSelected, True
      For rch = 0 To picReach.Count - 1
        If IsNumeric(picReach(rch).tag) Then
          Set lOper = lOpns(CLng(picReach(rch).tag))
          If lOper.MetSeg Is Nothing Then
  '          For Each vConn In lOper.Sources
  '              Set lConn = vConn
  '              If lConn.source.volname = "PERLND" Or lConn.source.volname = "IMPLND" Then
  '                if lconn.Parent.m
            SetReachSelected rch, False
          Else
            If lOper.MetSeg.Id = MetSelected + 1 Then
              SetReachSelected rch, True
            Else
              SetReachSelected rch, False
            End If
          End If
        End If
      Next rch
    Case LegPoint
      If PointSelected <> Index Then
        DrawLegendSelectBox PointSelected, False
        PointSelected = Index
      End If
      DrawLegendSelectBox PointSelected, True
      For rch = 0 To picReach.Count - 1
        If IsNumeric(picReach(rch).tag) Then
          Set lOper = lOpns(CLng(picReach(rch).tag))
          If lOper.PointSources Is Nothing Or lOper.PointSources.Count = 0 Then
            SetReachSelected rch, False
          Else
            Dim lPoint As HspfPoint
            i = 0
            SetReachSelected rch, False
            For Each lPoint In lOper.PointSources
              i = i + 1
              If lOper.PointSources(i).Id = PointSelected + 1 Then
                SetReachSelected rch, True
              End If
            Next lPoint
          End If
        End If
      Next rch
  End Select
End Sub

Private Sub DrawLegendSelectBox(Index, Selected As Boolean)
  Dim colr&
  With picLegend(Index)
    If Selected Then colr = HighlightColor Else colr = picLegend(Index).BackColor
    .FillStyle = vbFSTransparent
    .DrawStyle = vbSolid
    .DrawWidth = 2
    picLegend(Index).Line (1, 1)-(.Width - 1, .Height - 1), colr, B
    UpdateDetails
  End With
End Sub

Private Sub RefreshLegendSelections()
  Dim Index As Integer
  For Index = 0 To UBound(LandSelected)
    DrawLegendSelectBox Index, LegendSelected(Index)
  Next Index
End Sub

Private Sub SetReachSelected(reachIndex&, sel As Boolean)
  Dim colr&, BorderWidth&
  BorderWidth = 3
  With picReach(reachIndex)
    If sel Then colr = HighlightColor Else colr = picTree.BackColor
    .FillStyle = vbFSSolid
    .DrawStyle = vbSolid
    .DrawWidth = 1
    picTree.Line (.Left - BorderWidth - 1, .Top - BorderWidth - 1)-Step(.Width + BorderWidth * 2, .Height + BorderWidth * 2), colr, BF
    ReachSelected(reachIndex) = sel
  End With
End Sub

Private Sub fraHsash_MouseDown(Button As Integer, Shift As Integer, x As Single, y As Single)
  HsashDragging = True
  HsashDragStart = y
  fraHsash.BackColor = vb3DShadow
End Sub

Private Sub fraHsash_MouseUp(Button As Integer, Shift As Integer, x As Single, y As Single)
  HsashDragging = False
  fraHsash.BackColor = vbButtonFace
End Sub

Private Sub fraHsash_MouseMove(Button As Integer, Shift As Integer, x As Single, y As Single)
  If HsashDragging Then
    Dim h&
    h = ScaleHeight - (fraHsash.Top + (y - HsashDragStart) / Screen.TwipsPerPixelY + fraHsash.Height)
    If h > 0 Then picDetails.Height = h
    Form_Resize
  End If
End Sub

Private Sub fraVsash_MouseDown(Button As Integer, Shift As Integer, x As Single, y As Single)
  VsashDragging = True
  VsashDragStart = x
  fraVsash.BackColor = vb3DShadow
End Sub

Private Sub fraVsash_MouseUp(Button As Integer, Shift As Integer, x As Single, y As Single)
  VsashDragging = False
  fraVsash.BackColor = vbButtonFace
  UpdateLegend
End Sub

Private Sub fraVsash_MouseMove(Button As Integer, Shift As Integer, x As Single, y As Single)
  If VsashDragging Then
    fraVsash.Left = fraVsash.Left + (x - VsashDragStart) / Screen.TwipsPerPixelX
    Form_Resize
  End If
End Sub

Private Sub mnuAbout_Click(Index As Integer)
  doAction "About"
End Sub

Private Sub mnuExit_Click()
  Dim iresp&
  
  iresp = 1
  If Not myUci Is Nothing Then
    If myUci.Edited Then
      DisableAll True
      iresp = myMsgBox.Show("Changes have been made since your last Save." & vbCrLf & vbCrLf & _
                                     "Are you sure you want to Exit?", _
                                     "Confirm Exit", "+&Exit", "-&Cancel")
      DisableAll False
    End If
  End If
  If iresp = 1 Then
    Unload Me
  End If
End Sub

Private Sub mnuFunction_Click(Index As Integer)
  Select Case Index
    Case 0: doAction "Reach", "Reach Properties Editor"
    Case 1: doAction "Time", "Simulation Time and Meteorological Data"
    Case 2: doAction "LandUse", "Land Use Editor"
    Case 3: doAction "Control", "Control Cards"
    Case 4: doAction "Pollutant", "Pollutant Selection"
    Case 5: doAction "Point", "Point Sources"
    Case 6: doAction "Default", "Default Data Assignment"
    Case 7: doAction "Edit", "Input Data Editor"
    Case 8: doAction "Output", "Output Manager"
    Case 9: doAction "Run", "Run HSPF"
    Case 10: doAction "View", "View Time Series Output"
  End Select
End Sub
Private Sub mnuHelp_Click(Index As Integer)
  Select Case Index
    Case 3: IPC.SendMonitorMessage "(SHOW)"
            IPC.SendMonitorMessage "(TEXT ON)"
    Case Else: OpenFile App.HelpFile, CDFile   'just do contents for now
  End Select
  'HtmlHelp Me.hwnd, App.HelpFile, HH_DISPLAY_TOC, 0
End Sub
Private Sub mnuNew_Click()
  doAction "Create"
End Sub
Private Sub mnuOpen_Click()
  doAction "Open"
End Sub
Private Sub mnuSave_Click()
  doAction "Save", "Save"
End Sub
Private Sub mnuSaveAs_Click()
  doAction "SaveAs", "SaveAs"
End Sub

Private Sub mnuSupport_Click(Index As Integer)
  If Index = 0 Then
    doAction "PhoneSupport"
  Else
    doAction "WebSupport"
  End If
End Sub

Private Sub mnuView_Click()
  doAction "View", "View Time Series Output"
End Sub

Private Sub picLegend_DblClick(Index As Integer)
  Dim lOper As HspfOperation, i&, iresp&
  Dim oprindex&, Key$, colonpos&
  Dim match As Collection 'of hspfoperations
  Dim ButtonNames$()

  Set match = New Collection
  For oprindex = 1 To myUci.OpnSeqBlock.Opns.Count
    Set lOper = myUci.OpnSeqBlock.Opn(oprindex)
    If lOper.Name = "PERLND" Or lOper.Name = "IMPLND" Then
      Key = lOper.Description
      colonpos = InStr(Key, ":")
      If colonpos > 0 Then Key = Mid(Key, colonpos + 1)
      If Key = picLegend(Index).tag Then
        match.Add lOper
      End If
    End If
  Next
  iresp = -1
  If match.Count > 1 Then
    DisableAll True
    ReDim ButtonNames(match.Count - 1)
    For i = 1 To match.Count
      ButtonNames(i - 1) = match(i).Name & " " & match(i).Id
    Next i
    If match.Count = 2 Then
      iresp = myMsgBox.Show("Which operation would you like to edit?", _
                            "WinHSPF Edit Operation Query", ButtonNames(0), ButtonNames(1))
    ElseIf match.Count = 3 Then
      iresp = myMsgBox.Show("Which operation would you like to edit?", _
                            "WinHSPF Edit Operation Query", ButtonNames(0), ButtonNames(1), ButtonNames(2))
    ElseIf match.Count = 4 Then
      iresp = myMsgBox.Show("Which operation would you like to edit?", _
                            "WinHSPF Edit Operation Query", ButtonNames(0), ButtonNames(1), ButtonNames(2), ButtonNames(3))
    ElseIf match.Count = 5 Then
       iresp = myMsgBox.Show("Which operation would you like to edit?", _
                            "WinHSPF Edit Operation Query", ButtonNames(0), ButtonNames(1), ButtonNames(2), ButtonNames(3), ButtonNames(4))
    Else
      myMsgBox.Show "Multiple operations match this description." & vbCrLf & _
                  "Use 'Edit Opn Sequence' to edit the desired operation.", _
                  "WinHSPF Edit Operation Problem", "OK"
    End If
    DisableAll False
  ElseIf match.Count = 1 Then
    iresp = 1
  ElseIf match.Count = 0 And tabLeft.SelectedItem = "Met Segs" Then
    frmTime.Show
  ElseIf match.Count = 0 And tabLeft.SelectedItem = "Point Sources" Then
    frmPoint.Show
  End If
  If iresp > 0 Then
    'do edit operation
    If myUci.MetSegs.Count > 0 Then
      myUci.MetSeg2Source
    End If
    myUci.Point2Source
    match(iresp).Edit
    myUci.Source2MetSeg
    myUci.Source2Point
  End If
End Sub

Private Sub picReach_DblClick(Index As Integer)
  Dim lOper As HspfOperation
  
  Set lOper = lOpns(CLng(picReach(Index).tag))
  
  If myUci.MetSegs.Count > 0 Then
    myUci.MetSeg2Source
  End If
  myUci.Point2Source
  
  lOper.Edit
  
  myUci.Source2MetSeg
  myUci.Source2Point
End Sub

Private Sub picReach_MouseUp(Index As Integer, Button As Integer, Shift As Integer, x As Single, y As Single)
  If TreeDragging Then
    picTree_MouseUp Button, Shift, x + picReach(Index).Left, y + picReach(Index).Top
  Else
    SetReachSelected (Index), Not ReachSelected(Index)
    UpdateDetails
  End If
End Sub


Private Sub picTree_Click()
  UpdateDetails
End Sub

Private Sub picTree_MouseDown(Button As Integer, Shift As Integer, x As Single, y As Single)
  Dim rch&
  TreeStartX = x
  TreeStartY = y
  TreeRectX = x
  TreeRectY = y
  TreeDragging = True
  
  If Shift = 0 Then 'deselect all reaches if user is not holding shift, ctrl, or alt
    For rch = 0 To UBound(ReachSelected)
      SetReachSelected rch, False
    Next
  End If
  
  ReDim BeforeDragReachSelected(UBound(ReachSelected))
  For rch = 0 To UBound(ReachSelected)
    BeforeDragReachSelected(rch) = ReachSelected(rch)
  Next

End Sub

Private Function RectOverlaps(minX As Single, maxX As Single, minY As Single, maxY As Single, _
                              Top As Single, Left As Single, Width As Single, Height As Single)
  If Top > minY And Top < maxY And Left > minX And Left < maxX Then
    RectOverlaps = True
  ElseIf Top + Height > minY And Top + Height < maxY And Left + Width > minX And Left + Width < maxX Then
    RectOverlaps = True
  ElseIf Top + Height > minY And Top + Height < maxY And Left > minX And Left < maxX Then
    RectOverlaps = True
  ElseIf Top > minY And Top < maxY And Left + Width > minX And Left + Width < maxX Then
    RectOverlaps = True
  Else
    RectOverlaps = False
  End If
End Function

Private Sub picTree_MouseMove(Button As Integer, Shift As Integer, x As Single, y As Single)
  Dim minX As Single, maxX As Single, minY As Single, maxY As Single
  Dim rch&
  If TreeDragging Then
    DrawTreeDragRect 'undraw last box
    TreeRectX = x
    TreeRectY = y
    DrawTreeDragRect 'Draw new box
    
    If x < TreeStartX Then
      minX = x: maxX = TreeStartX
    Else
      minX = TreeStartX: maxX = x
    End If
    
    If y < TreeStartY Then
      minY = y: maxY = TreeStartY
    Else
      minY = TreeStartY: maxY = y
    End If
    
    For rch = 0 To UBound(ReachSelected)
      ReachSelected(rch) = BeforeDragReachSelected(rch)
      If Not ReachSelected(rch) Then
        With picReach(rch)
          ReachSelected(rch) = RectOverlaps(minX, maxX, minY, maxY, .Top, .Left, .Width, .Height)
        End With
      End If
      SetReachSelected rch, ReachSelected(rch)
    Next
  End If
End Sub

Private Sub picTree_MouseUp(Button As Integer, Shift As Integer, x As Single, y As Single)
  If TreeDragging Then
    DrawTreeDragRect
    TreeDragging = False
    UpdateDetails
  End If
End Sub

Private Sub DrawTreeDragRect()
  Dim DM&, FS&
  DM = picTree.DrawMode
  FS = picTree.FillStyle
  picTree.DrawMode = vbNotXorPen
  picTree.FillStyle = vbFSTransparent
  If TreeStartX = TreeRectX Or TreeStartY = TreeRectY Then 'one-dimensional
    picTree.Line (TreeStartX, TreeStartY)-(TreeRectX, TreeRectY), vbButtonText
  Else
    picTree.Line (TreeStartX, TreeStartY)-(TreeRectX, TreeRectY), vbButtonText, B
  End If
  picTree.DrawMode = DM
  picTree.FillStyle = FS
End Sub

Private Sub tabLeft_Click()
  CurrentLegend = tabLeft.SelectedItem.tag
  UpdateLegend
  BuildTree
End Sub

Private Sub tbrMain_ButtonClick(ByVal Button As MSComctlLib.Button)
  doAction Button.Key, Button.tooltiptext
End Sub

Public Sub OpenUCI(Optional ByVal myFileName As String)
  Dim f$, S$, hin&, hout&, i&, filesok As Boolean, iresp&, echofile$
  Dim DispFile As ATCoDispFile
 
  DisableAll True
  i = 1
  If Len(myUci.Name) > 0 Then
    'already have an active uci, warn user
    i = myMsgBox.Show("Only one UCI can be active at a time." & vbCrLf & _
               "Continuing will deactivate the current UCI." & vbCrLf & _
               "Are you sure you want to continue?", "UCI Open Warning", "+-&OK", "&Cancel")
  End If
  If i = 1 Then
    IPC.dbg "Opening UCI '" & myFileName & "'"
    'okay to open a uci file
    tabLeft.Tabs(1).Selected = True
    IPC.dbg "about to call CloseUCI"
    Call CloseUCI 'close one if already opened
    IPC.dbg "back from CloseUCI"
    If defUci.Pollutants.Count > 0 Then  'need to remove all pollutants
      Do Until defUci.Pollutants.Count = 0
        defUci.Pollutants.Remove 1
      Loop
    End If
    CDFile.Filename = myFileName
    If Len(myFileName) = 0 Then
      CDFile.Filename = GetSetting(gAppName, "Open", "LastUCI", "")
      If Not FileExists(CDFile.Filename) Then
        CDFile.Filename = BASINSPath
        i = InStr(UCase(BASINSPath), "BASINS")
        If i > 0 Then
          CDFile.Filename = Left(CDFile.Filename, i + 5) & "\modelout\*.uci"
        Else
          CDFile.Filename = PathNameOnly(CDFile.Filename) & "\*.uci"
        End If
      End If
      ChDriveDir PathNameOnly(CDFile.Filename)
      CDFile.CancelError = True
      
      CDFile.flags = &H8806&
      CDFile.Filter = "User Control Input Files (*.uci)|*.uci"
      CDFile.Filename = "*.uci"
      CDFile.DialogTitle = "Open Project"
      On Error GoTo 10
      CDFile.CancelError = True
      CDFile.ShowOpen
      myFileName = CDFile.Filename
      SaveSetting gAppName, "Open", "LastUCI", myFileName
    End If
    IPC.dbg "about to ChDriveDir " & PathNameOnly(myFileName)
    ChDriveDir PathNameOnly(myFileName)
    IPC.dbg "about to InitColorMap"
    InitColorMap
    Me.MousePointer = vbHourglass
    'hin = myMonitor.Launch.ComputeRead
    'hout = myMonitor.Launch.ComputeWrite
    'myUci.StatusIn = hin
    'myUci.StatusOut = hout
    myUci.MessageUnit = MessageUnit
    'IPC.SendMonitorMessage "(OPEN WinHSPF)"
    f = LCase(CDFile.Filename)
    IPC.dbg "ReadUci " & f
    Set myUci.Icon = Me.Icon
    'myUci.FastFlag = True
    myUci.ReadUci myMsg, f, 1, filesok, echofile
    'myUci.FastFlag = False
    myUci.MsgWDMName = W_HSPFMSGWDM
    If Not filesok Then
      IPC.dbg "Files not OK, Closing UCI"
      CloseUCI
    Else
      'add tree diagram to main window
      S = myUci.ErrorDescription
      If Len(S) > 0 Then
        iresp = myMsgBox.Show(S, "HSPF Problem", "+-&Continue", "&View Errors")
        If iresp = 2 Then
          Set DispFile = New ATCoDispFile
          DispFile.FindString = "ERROR"
          DispFile.OpenFile echofile, "WinHSPF Error View", Me.Icon, False
          Set DispFile = Nothing
        End If
        'CloseUCI
      End If
      'Else
        TreeProblemMessageDisplayed = False
        ClearTree
        BuildTree
        AddRecentFile mnuRecent, f
        Caption = BaseCaption & ": " & FilenameOnly(myUci.Name)
        'lblScenario.Caption = "Project(Scenario): " & FileNameOnly(myUci.Name)
        tabLeft_Click
        UpdateLegend
      'End If
    End If
10      ' continue here on cancel
    Me.MousePointer = vbNormal
    Err.Clear
  End If
  DisableAll False
End Sub

Public Sub OpenDefaultUCI(filepath$, Filename$)
  Dim f$, S$, hin&, hout&, filesok As Boolean, echofile$

  'IPC.SendMonitorMessage "(in openDefaultUCI)" & W_HSPFMSGWDM
  'CDFile.Flags = &H8806&
  'CDFile.Filter = "User Control Input Files (*.uci)"
  'CDFile.filename = "*.uci"
  'CDFile.DialogTitle = "Open Default UCI"
  'On Error GoTo 10
  'CDFile.CancelError = True
  'CDFile.Action = 1
  'f = LCase(CDFile.filename)
  
  'ChDrive Left(W_STARTERPATH, 1)
  'ChDir (W_STARTERPATH)
  'f = "starter.uci"
  ChDriveDir filepath
  f = Filename
  Me.MousePointer = vbHourglass
  Set defUci = Nothing
  Set defUci = New HspfUci
  Set defUci.Monitor = IPC
  defUci.HelpFile = App.HelpFile
  defUci.MessageUnit = MessageUnit
  'myUci.InitWDMArray
  defUci.MsgWDMName = W_HSPFMSGWDM
  'defUci.InitWDMArray
  'hin = mymonitor.launch.ComputeRead
  'hout = mymonitor.launch.ComputeWrite
  'defUci.StatusIn = hin
  'defUci.StatusOut = hout
  defUci.FastReadUciForStarter myMsg, f
  'defUci.ReadUci myMsg, f, -1, filesok, echofile
  Set defUci.Icon = Me.Icon
  Me.MousePointer = vbNormal
  S = defUci.ErrorDescription
10    ' continue here on cancel
  Err.Clear
  'clear out wdm array for myuci
  defUci.InitWDMArray
End Sub

Private Sub CreateUCI()
  Dim hin&, hout&, i&
  
  i = 1
  If Len(myUci.Name) > 0 Then
    'already have an active uci, warn user
    DisableAll True
    i = myMsgBox.Show("Only one UCI can be active at a time." & vbCrLf & _
               "Continuing will deactivate the current UCI." & vbCrLf & _
               "Are you sure you want to continue?", "UCI Create Warning", "+-&OK", "&Cancel")
    DisableAll False
  End If
  If i = 1 Then
    'okay to create a uci file
    tabLeft.Tabs(1).Selected = True
    If Len(myUci.Name) > 0 Then Call CloseUCI
    'hin = myMonitor.Launch.ComputeRead
    'hout = myMonitor.Launch.ComputeWrite
    'myUci.StatusIn = hin
    'myUci.StatusOut = hout
    myUci.MessageUnit = MessageUnit
    frmCreate.Show vbModal
    TreeProblemMessageDisplayed = False
  End If
End Sub

Public Sub DoCreate(watershedfile$, outputWDM$, metwdms$(), _
                    wdmids$(), MetDataDetails$, oneseg As Boolean)

  Dim S$, hin&, hout&

  picTree.Visible = False
  InitColorMap
  Do Until picLegend.UBound = 0
    Unload picLegend(picLegend.UBound)
  Loop
  picLegend(0).Visible = False
  Me.MousePointer = vbHourglass
  'hin = myMonitor.Launch.ComputeRead
  'hout = myMonitor.Launch.ComputeWrite
  'myUci.StatusIn = hin
  'myUci.StatusOut = hout
  
  myUci.CreateUCI myMsg, watershedfile, outputWDM, metwdms, _
                  wdmids, MetDataDetails, oneseg, PollutantList
  
  Set myUci.Icon = Me.Icon
  'add tree diagram to main window
  ClearTree
  BuildTree
  Me.MousePointer = vbNormal
  S = myUci.ErrorDescription
  Caption = BaseCaption & ": " & FilenameOnly(myUci.Name)
  tabLeft_Click
  UpdateLegend
End Sub

Private Sub doAction(buttontext$, Optional tooltiptext$)
  Dim n$, i&, retcod&, iresp&, oldname$, S$
  Dim DispFile As ATCoDispFile

  Select Case buttontext 'First few actions do not require an active project
    Case "Create":     CreateUCI
    Case "Open":       OpenUCI
    Case "WebSupport": ShellExecute Me.hwnd, "Open", "http://www.epa.gov/ost/basins/", vbNullString, vbNullString, 1 'SW_SHOWNORMAL
    Case "PhoneSupport": myMsgBox.Show "Option 'Phone Support' is not yet implemented.", "Not Yet Implemented", "+-&Close"
    Case "About":        frmAbout.Show
                         'myMsgBox.Show "WinHSPF Version 2.0.2", "About WinHSPF", "+-&Close"
    Case Else            'Remaining actions require an active project
      If Len(myUci.Name) < 1 Then
        DisableAll True
        myMsgBox.Show "No Project is active." & vbCrLf & vbCrLf & _
                      "Open or Create a Project before using the '" & tooltiptext & "' feature.", _
                      "WinHSPF: '" & tooltiptext & "' Problem", "+-&Close"
        DisableAll False
      Else
        Select Case buttontext
          Case "Control"
            DisableAll True
            iresp = myMsgBox.Show("Choose a format for editing control cards.", _
                                  "Control Window Selection", "+&Tables", "&Descriptions")
            DisableAll False
            If iresp = 1 Then
              myUci.EditActivityAll
              'check for missing tables, add if needed
              CheckAndAddMissingTables "PERLND"
              CheckAndAddMissingTables "IMPLND"
              CheckAndAddMissingTables "RCHRES"
              CheckAndAddMassLinks
              Call SetMissingValuesToDefaults(myUci, defUci)
            ElseIf iresp = 2 Then
              frmControl.Show 1                        'old control card editor
            End If
          Case "Pollutant": myUci.PollutantsBuild
                            frmPollutant.Show vbModal
                            myUci.PollutantsUnBuild
                            Call CheckAndAddMissingTables("PERLND")
                            Call CheckAndAddMissingTables("IMPLND")
                            Call CheckAndAddMissingTables("RCHRES")
                            Call UpdateFlagDependencies("RCHRES")
                            Call SetMissingValuesToDefaults(myUci, defUci)
          Case "Edit":      frmEdit.Show vbModal       'input data editor
                            'Refresh all in case anything has been edited
                            ClearTree
                            BuildTree
                            UpdateLegend
                            UpdateDetails
          Case "LandUse":   frmLand.Show 1            'landuse editor
                            ClearTree
                            BuildTree
                            UpdateLegend
                            UpdateDetails
          Case "Point":     frmPoint.Show             'point sources
          Case "Reach":
            If myUci.OpnBlks("RCHRES").Count > 0 Then
              frmReach.Show vbModal     'reach properties editor
              ClearTree
              BuildTree
              UpdateLegend
              UpdateDetails
            Else
              myMsgBox.Show "The current project contains no reaches.", _
                      "Reach Editor Problem", "+-&OK"
            End If
          'Case "Default":   OpenDefaultUCI
          Case "Output":    frmOutput.Show 1
          Case "Run"                                  'run HSPF
            iresp = 1
            If myUci.Edited Then
              DisableAll True
              iresp = myMsgBox.Show("Changes have been made since your last Save." & vbCrLf & vbCrLf & _
                                     "Do you want to save these changes?", _
                                     "Confirm Save UCI", "+&Save/Run", "-&Cancel")
              If iresp = 1 Then myUci.Save
              DisableAll False
            End If
            If iresp = 1 Then
              DisableAll True
              myUci.ClearAllOutputDsns
              myUci.RunUci retcod   'now activate and run
              If retcod = -99 Then StartHSPFEngine
              S = myUci.ErrorDescription
              If Len(S) > 0 Then
                iresp = myMsgBox.Show(S, "HSPF Problem", "+-&OK", "&View Errors")
                If iresp = 2 Then
                  Set DispFile = New ATCoDispFile
                  DispFile.FindString = "ERROR"
                  DispFile.OpenFile myUci.EchoFileName, "WinHSPF Error View", Me.Icon, False
                  Set DispFile = Nothing
                End If
              End If
              DisableAll False
            End If
          Case "Save": myUci.Save                 'save uci file
          Case "SaveAs"                           'save uci file As
            oldname = FilenameOnly(myUci.Name)
            Me.MousePointer = vbHourglass
            newname = ""
            frmSaveAs.Show 1, HSPFMain
            If Len(newname) > 0 Then
              myUci.Name = newname
              If Len(myUci.Name) > 3 Then
                If UCase(Right(myUci.Name, 4)) <> ".UCI" Then  'force to have uci extension
                  myUci.Name = myUci.Name & ".uci"
                End If
              End If
              newname = FilenameOnly(myUci.Name)
              myUci.SaveAs oldname, newname, basedsn, relabs
              AddRecentFile mnuRecent, myUci.Name
              Caption = BaseCaption & ": " & FilenameOnly(myUci.Name)
            End If
            Me.MousePointer = vbNormal
          Case "Time":      frmTime.Show vbModal       'simulation time and met data
          Case "View":      StartGenScn                'call genscn
          Case "BMP":       frmBMP.Show vbModal        'bmp editor
            ClearTree
          Case "Convert":   frmConvert.Show vbModal    'convert npsm to winhspf uci
          Case "AQUATOX":   frmAQUATOX.Init i          'winhspf-aquatox link
                            If i = 0 Then
                              myMsgBox.Show "At least one AQUATOX output location must be specified " & vbCrLf & "in the Output Manager " & _
                                "before linking to AQUATOX.", "WinHSPF-AQUATOX Problem", "OK"
                            Else
                              frmAQUATOX.Show vbModal
                            End If
          Case "PEST":  Dim PEST As Object 'New clsPest
                        On Error GoTo NoPest
                        Set PEST = CreateObject("ATCoPest.clsPest")
                        PEST.Uci = myUci
                        PEST.IPC = IPC
                        PEST.modelname = "HSPF"
                        PEST.Edit
          Case Else
            myMsgBox.Show "Option '" & tooltiptext & "' is not yet implemented.", "Not Yet Implemented", "+-&Close"
        End Select
        BuildTree
      End If
  End Select
  Exit Sub
NoPest:
  MsgBox "Could not create PEST object ATCoPest.clsPest" & vbCr & "Perhaps ATCoPest.dll is missing?"
End Sub

Public Sub BuildTree(Optional Printing As Boolean = False)
  
  Dim i&, j&, k&, iCur&, iTrib&(), iNow&(), ypos&, iTotalTrib&(), iTotalTribOp$()
  Dim lOpn As HspfOperation, lSrcOpn As HspfOperation, lTarOpn As HspfOperation
  Dim lSrc As HspfConnection
  Dim istart&, ifinish&, workingwidth&, ifound As Boolean
  Dim maxlayer&, iLayer&
  Dim itemp&, itribcnt&, ltribcnt&, itotaltribcnt&
  Dim curline&, newstart&, newfinish&, Xbase&, Ybase&
  Dim loptyp
  Dim dy&, halfBoxHeight&, halfBoxWidth&
  Dim PicTop() As Long, PicLeft() As Long
  Dim drawsurface As Object
  Dim pic As PictureBox
  Dim lOpnBlk As HspfOpnBlk
  Dim icount&
  
  icount = 0
  If myUci.OpnBlks.Count > 0 Then
    For Each loptyp In OpTyps
      Set lOpnBlk = myUci.OpnBlks(loptyp)
      icount = icount + lOpnBlk.Count
    Next loptyp
  End If
  
  If icount > 0 Then
    'okay to do tree
    Set lOpns = Nothing
    Set lOpns = New Collection
    
    If myUci.Name = "" Then Exit Sub
    
    For j = 1 To myUci.OpnSeqBlock.Opns.Count 'or other bottom oper
      Set lOpn = myUci.OpnSeqBlock.Opns(j)
      For Each loptyp In OpTyps
        If lOpn.Name = loptyp Then
          lOpns.Add lOpn
          Exit For
        End If
      Next loptyp
    Next j
    Set lOpn = lOpns(lOpns.Count)
    
    'find max number of layers in tree
    maxlayer = 0
    For i = 1 To lOpns.Count - 1
      iLayer = DownLayers(1, i, lOpns)
      If iLayer > maxlayer Then
        maxlayer = iLayer
      End If
    Next i
    If maxlayer = 0 Then maxlayer = 1
    
    If Printing Then
      Printer.ScaleMode = vbPixels
      picBuffer.Width = Printer.ScaleWidth / 6
      picBuffer.Height = Printer.ScaleHeight / 20
      dy = Printer.ScaleHeight / (maxlayer + 1)
      Set drawsurface = Printer
      Set pic = picBuffer
    Else
      picTree.Cls
      picTree.Visible = True 'False
      dy = (picTree.Height - 20) / maxlayer
      Set drawsurface = picTree
      Set pic = picReach(0)
    End If
    ReDim PicTop(picReach.Count)
    ReDim PicLeft(picReach.Count)
  
    halfBoxHeight = pic.Height / 2
    halfBoxWidth = pic.Width / 2
    Ybase = drawsurface.ScaleHeight - dy / 2
    PicTop(0) = Ybase - halfBoxHeight
    PicLeft(0) = drawsurface.ScaleWidth / 2 - halfBoxWidth
    
    lOpn.setPicture pic, ColorMap, CurrentLegend, LegendOrder
    drawBorder pic, Not Printing
    If Printing Then
      drawsurface.PaintPicture pic.Image, PicLeft(0), PicTop(0)
    Else
      pic.Top = PicTop(0)
      pic.Left = PicLeft(0) '+ pic.width
      pic.tag = lOpns.Count
    End If
    'initialize before big tree loop
    istart = 0
    ifinish = 0
    curline = 0
    newstart = 0
    newfinish = 0
    
    itotaltribcnt = 0
10    'start of tree loop
    newstart = 0
    newfinish = 0
    itribcnt = 0
    
    'ypos = Ybase - istart * dy 'lineT(istart).Y1 - dy
    For j = istart To ifinish  'loop the top layer of branches already placed - count next layer
      ltribcnt = 0
      iCur = picReach(j).tag
      Set lTarOpn = lOpns(iCur)
      For i = iCur - 1 To 1 Step -1
        Set lSrcOpn = lOpns(i)
        For Each lSrc In lTarOpn.Sources
          If lSrcOpn.Name = lSrc.Source.volname And _
             lSrcOpn.Id = lSrc.Source.volid Then
            'found a trib to this branch
            ifound = False
'            For k = 1 To itribcnt 'check to see if we already have it
'              If iTrib(k) = i Then
'                ifound = True       fix 092602 to catch all mult exit situations
'              End If
'            Next k
            For k = 1 To itotaltribcnt 'check to see if we already have it
              If iTotalTrib(k) = lSrcOpn.Id And iTotalTribOp(k) = lSrcOpn.Name Then
                ifound = True
              End If
            Next k
            If ifound Then
              'we can't do tree diagram with multiple exits from a reach,
              'just skip this connection on tree diagram for now
              If TreeProblemMessageDisplayed = False Then
                MsgBox "A reach with multiple exits has been detected." & _
                       vbCrLf & "Only the first exit will be drawn in the tree diagram." _
                       , vbOKOnly, "WinHSPF Problem"
                TreeProblemMessageDisplayed = True
              End If
            Else
              ltribcnt = ltribcnt + 1
              itribcnt = itribcnt + 1
              itotaltribcnt = itotaltribcnt + 1
              ReDim Preserve iTrib(itribcnt)
              iTrib(itribcnt) = i
              ReDim Preserve iNow(itribcnt)
              iNow(itribcnt) = j
              ReDim Preserve iTotalTrib(itotaltribcnt)
              iTotalTrib(itotaltribcnt) = lSrcOpn.Id
              ReDim Preserve iTotalTribOp(itotaltribcnt)
              iTotalTribOp(itotaltribcnt) = lSrcOpn.Name
            End If
          End If
        Next lSrc
      Next i
      If ltribcnt = 0 Then 'space holder
        itribcnt = itribcnt + 1
        ReDim Preserve iTrib(itribcnt)
        ReDim Preserve iNow(itribcnt)
        iTrib(itribcnt) = 0
        iNow(itribcnt) = 0
      End If
    Next j
    
    workingwidth = drawsurface.ScaleWidth / itribcnt '* Screen.TwipsPerPixelX
    Xbase = workingwidth / 2
    For i = 1 To itribcnt  'loop to place each trib to this branch
      If iTrib(i) > 0 Then 'not a space holder
        curline = curline + 1
        If newstart = 0 Then newstart = curline   'the first branch on this row
        newfinish = curline
        j = iNow(i)
        If Not Printing Then Set pic = picReach(curline)
        PicTop(curline) = PicTop(j) - dy
        PicLeft(curline) = Xbase - halfBoxWidth
        lOpns(iTrib(i)).setPicture pic, ColorMap, CurrentLegend, LegendOrder
        drawBorder pic, Not Printing
        drawsurface.Line (Xbase, PicTop(curline) + halfBoxHeight)-(PicLeft(j) + halfBoxWidth, PicTop(j) + halfBoxHeight)
        With pic
          If Printing Then
            drawsurface.PaintPicture .Image, PicLeft(curline), PicTop(curline)
          Else
            .Top = PicTop(curline)
            .Left = PicLeft(curline) '+ .width
            .tag = iTrib(i)
            .Visible = True
          End If
        End With
      End If
      Xbase = Xbase + workingwidth
    Next i
    
    If newstart > 0 Then  'another row of branches to do
      istart = newstart
      ifinish = newfinish
    End If
    If newstart > 0 Then GoTo 10
    
    If Printing Then
      Printer.EndDoc
    Else
      ReDim Preserve ReachSelected(picReach.Count - 1)
      For i = 0 To picReach.Count - 1
        SetReachSelected i, ReachSelected(i)
      Next
      picTree.Visible = True
    End If
  End If
End Sub

Private Sub drawBorder(pic As PictureBox, threeD As Boolean)
  Dim colr&
  If threeD Then colr = vb3DHighlight Else colr = vbBlack
  pic.Line (0, 0)-(0, pic.Height), colr
  pic.Line (0, 0)-(pic.Width, 0), colr
  If threeD Then colr = vb3DShadow Else colr = vbBlack
  pic.Line (0, pic.Height - 1)-(pic.Width - 1, pic.Height - 1), colr
  pic.Line (pic.Width - 1, 0)-(pic.Width - 1, pic.Height - 1), colr
End Sub


Private Function DownLayers(iLayer&, iOpnInd&, lOpns As Collection) As Long
  Dim j&, lLayer&, mLayer&
  Dim lSrcOpn As HspfOperation
  Dim lTarOpn As HspfOperation
  Dim lTar As HspfConnection

  mLayer = iLayer
  Set lSrcOpn = lOpns(iOpnInd)
  With lSrcOpn
    For Each lTar In .targets
      For j = iOpnInd + 1 To lOpns.Count
        Set lTarOpn = lOpns(j)
        If lTarOpn.Name = lTar.Target.volname And _
           lTarOpn.Id = lTar.Target.volid Then
          lLayer = DownLayers(iLayer + 1, j, lOpns)
          If lLayer > mLayer Then
            mLayer = lLayer
          End If
        End If
      Next j
    Next lTar
  End With
  DownLayers = mLayer
  
End Function

Public Sub ClearTree()
  Dim lOpnBlk As HspfOpnBlk
  Dim loptyp As Variant
  Dim i&, icount&
  
  picTree.Cls
  For i = 1 To picReach.UBound 'unload any previously loaded branches
    'Unload lineT(i)
    Unload picReach(i)
  Next i
  
  icount = 0
  For Each loptyp In OpTyps
    Set lOpnBlk = myUci.OpnBlks(loptyp)
    icount = icount + lOpnBlk.Count
  Next loptyp
  
  For i = 1 To icount - 1
    'Load lineT(i)
    load picReach(i)
  Next i
  
End Sub

Private Sub SetFiles()
  Dim hdle&, i&, ExName$, ExPath$
  Dim S As String * 100
  Dim ff As ATCoFindFile
  
  Set ff = New ATCoFindFile
  
  hdle = GetModuleHandle(gAppName)
  i = GetModuleFileName(hdle, S, 100)
  ExName = Left(S, InStr(S, Chr(0)) - 1)
  ExPath = PathNameOnly(ExName) & "\"
  
  If InStr(UCase(ExName), "VB6.EXE") Then
    'Running VB
    W_EXEWINHSPF = MACHINE_EXEWINHSPF
'    W_HSPFMSGMDB = MACHINE_HSPFMSGMDB
'    W_POLLUTANTLIST = MACHINE_POLLUTANTLIST
'    W_HSPFMSGWDM = MACHINE_HSPFMSGWDM
'    W_STARTERPATH = MACHINE_STARTERPATH
  Else
    'running from exe
    W_EXEWINHSPF = ExName
'    W_HSPFMSGMDB = ExPath & "hspfmsg.mdb"
'    W_POLLUTANTLIST = ExPath & "poltnt_2.prn"
'    W_HSPFMSGWDM = ExPath & "hspfmsg.wdm"
'    W_STARTERPATH = ExPath & "starter"
  End If
  
  ff.SetDialogProperties "Please locate hspfmsg.mdb", ExPath & "hspfmsg.mdb"
  ff.SetRegistryInfo App.EXEName, "files", "hspfmsg.mdb"
  W_HSPFMSGMDB = ff.GetName
  
  ff.SetDialogProperties "Please locate hspfmsg.wdm", ExPath & "hspfmsg.wdm"
  ff.SetRegistryInfo App.EXEName, "files", "hspfmsg.wdm"
  W_HSPFMSGWDM = ff.GetName
  
  ff.SetDialogProperties "Please locate poltnt_2.prn", ExPath & "poltnt_2.prn"
  ff.SetRegistryInfo App.EXEName, "files", "poltnt_2.prn"
  W_POLLUTANTLIST = ff.GetName

  ff.SetDialogProperties "Please locate starter.uci", ExPath & "starter\starter.uci"
  ff.SetRegistryInfo App.EXEName, "files", "starter.uci"
  W_STARTERPATH = PathNameOnly(ff.GetName)
  
  ff.SetDialogProperties "Please locate WinHSPF.chm", AbsolutePath("..\doc\WinHSPF.chm", PathNameOnly(W_EXEWINHSPF))
  ff.SetRegistryInfo gAppName, "files", "WinHSPF.chm"
  App.HelpFile = ff.GetName
  
  ff.SetDialogProperties "Please locate HSPFEngine.exe", ExPath & "HSPFEngine.exe"
  ff.SetRegistryInfo "HSPFEngine", "files", "HSPFEngine.exe"
  W_HSPFEngineExe = ff.GetName
'  App.HelpFile = reg.RegGetString(HKEY_LOCAL_MACHINE, "SOFTWARE\AQUA TERRA Consultants\WinHSPF\DocPath", "") & "\WinHSPF.chm"
'  If Len(Dir(App.HelpFile)) = 0 Then App.HelpFile = AbsolutePath("..\doc\WinHSPF.chm", PathNameOnly(W_EXEWINHSPF))
'  If Len(Dir(App.HelpFile)) = 0 Then App.HelpFile = ExPath & "WinHSPF.chm"
'  If Len(Dir(App.HelpFile)) = 0 Then App.HelpFile = "WinHSPF.chm"

End Sub

Private Sub SetEditMenu()

  Dim vBlock As Variant, lBlock As HspfBlockDef
  Dim i&, j&, addflg As Boolean
  
  i = 0
  For Each vBlock In myMsg.BlockDefs
    Set lBlock = vBlock
    addflg = False
    If lBlock.SectionDefs(1).TableDefs.Count = 0 Then
      addflg = True
    ElseIf lBlock.SectionDefs(1).TableDefs.Count = 1 Then
      If lBlock.SectionDefs(1).TableDefs(1).Name = "<NONE>" Then
        addflg = True
      End If
    End If
    If addflg Then
      i = i + 1
      If i > 1 Then
        load mnuEditOperParent(i - 1)
      End If
      mnuEditOperParent(i - 1).Caption = lBlock.Name
    End If
  Next vBlock
End Sub

Public Sub CloseUCI()
    picTree.Visible = False
    agdDetails.Clear
    Do Until picLegend.UBound = 0
      Unload picLegend(picLegend.UBound)
    Loop
    Me.Caption = "Hydrological Simulation Program - Fortran (HSPF)"
    agdDetails.Visible = False
    picLegend(0).Visible = False
    myUci.ClearWDM
    Set myUci = Nothing
    Set myUci = New HspfUci
    Set myUci.Monitor = IPC
    myUci.HelpFile = App.HelpFile
    myUci.StarterPath = W_STARTERPATH
    IPC.dbg "about to call InitWDMArray " & W_HSPFMSGWDM
    myUci.MsgWDMName = W_HSPFMSGWDM
    myUci.MessageUnit = MessageUnit
    myUci.InitWDMArray
    IPC.dbg "back from InitWDMArray"
End Sub
    
Private Sub ReadPollutantList()
  Dim i&, pstring$, tindex$, j&
  
  Set PollutantList = Nothing
  Set PollutantList = New Collection
  
  On Error GoTo filenotfound
  i = FreeFile(0)
  Open W_POLLUTANTLIST For Input As i
  On Error Resume Next
  Line Input #i, pstring
  
  j = 0
  Do Until EOF(i)
    Line Input #i, pstring
    j = j + 1
    PollutantList.Add Trim(pstring), CStr(j)
  Loop
  Close #i
  
  Exit Sub
filenotfound:
  Call MsgBox("Unable to Open Pollutant List File (" & W_POLLUTANTLIST & ")", , "WinHSPF - Pollutant List Problem")
End Sub

Private Sub StartGenScn()
  Dim GenScnEXE$, uCommand$
  Dim mappath$, staname$
  Dim reg As New ATCoRegistry
  Dim TempUciName$
  Dim ff As ATCoFindFile
  
  GenScnEXE = reg.RegGetString(HKEY_LOCAL_MACHINE, "SOFTWARE\AQUA TERRA Consultants\GenScn\ExePath", "") & "\genscn.exe"
  
  If Not FileExists(GenScnEXE) Then GenScnEXE = BASINSPath & "\models\HSPF\bin\genscn.exe"
  If Not FileExists(GenScnEXE) Then GenScnEXE = PathNameOnly(App.EXEName) & "\genscn.exe"
  If Not FileExists(GenScnEXE) Then GenScnEXE = CurDir & "\genscn.exe"
  If Not FileExists(GenScnEXE) Then GenScnEXE = "c:\program files\basins\bin\genscn.exe"
  If Not FileExists(GenScnEXE) Then GenScnEXE = "c:\basins\models\HSPF\bin\genscn.exe"
  If Not FileExists(GenScnEXE) Then GenScnEXE = GetSetting("GenScn", "ExePath", "") & "\genscn.exe"
  If Not FileExists(GenScnEXE) Then GenScnEXE = GetSetting("GenScn", "ExePath", "WithFilename")
  If Not FileExists(GenScnEXE) Then
    'genscn not in registry
    Set ff = New ATCoFindFile
    ff.SetDialogProperties "Please locate GenScn.exe so GenScn can be started.", "GenScn.exe"
    ff.SetRegistryInfo "GenScn", "ExePath", "WithFilename"
    GenScnEXE = ff.GetName
  End If
  If Not FileExists(GenScnEXE) Then
    DisableAll True
    myMsgBox.Show "WinHSPF could not find GenScn.exe", "View Output Problem", "+-&Close"
    DisableAll False
  Else
    'see if .sta file exists
    staname = PathNameOnly(myUci.Name) & "\" & FilenameOnly(myUci.Name) & ".sta"
    If FileExists(staname) Then '.sta exists
      uCommand = " " & staname
    Else
      'no .sta file, so set up intermediate window entering GenScn
      'see if map file exists
      mappath = BASINSPath & "\modelout\" & FilenameOnly(myUci.Name) & "\"
      If FileExists(mappath & FilenameOnly(myUci.Name) & ".map") Then   'map exists
        mappath = mappath & FilenameOnly(myUci.Name) & ".map"
      Else 'see if map file is with uci file
        mappath = PathNameOnly(myUci.Name) & "\"
        If FileExists(mappath & FilenameOnly(myUci.Name) & ".map") Then 'map exists
          mappath = mappath & FilenameOnly(myUci.Name) & ".map"
        Else
          mappath = ""
        End If
      End If
      'command line includes type, basins loc, project, map file name
      If Len(BASINSPath) > 0 Then
        'uCommand = " /BASHSPF " & BASINSPath & " " & FilenameOnly(myUci.Name) & " " & mappath
        uCommand = " /BASHSPF '" & BASINSPath & "','" & myUci.Name & "','" & mappath & "'"
      Else
        uCommand = ""
      End If
    End If
    'TempUciName = myUci.Name
    'CloseUCI
    'Me.Hide
    IPC.dbg "Starting GenScn... " & GenScnEXE & " " & uCommand
    IPC.StartProcess "GenScn", GenScnEXE & " " & uCommand, 0, 864000 'Kill after 10 days
    IPC.dbg "Finished Running GenScn"
    'Me.Show
    'IPC.dbg "Opening UCI '" & TempUciName & "' after running GenScn"
    'OpenUCI TempUciName
    'Shell genscnexe & " " & uCommand
  End If
NeverMind:
End Sub

Public Function StatusString(Optional AboutFlag As Boolean = True) As String
  Dim vFile As Variant
  Dim curClsTserFile As ATCclsTserFile
  Dim S$, i&, j&
  Dim tfile As HspfFile
  
  If AboutFlag Then
    S = "WinHSPF - Version " & App.Major & "." & App.Minor
    If App.Revision >= 1000 Then
      If App.Revision > 1000 Then S = S & " build " & App.Revision - 1000
      S = S & vbCrLf ' " final" & vbCrLf
    Else
      S = S & " beta " & App.Revision & vbCrLf
      S = S & "FOR TESTING AND EVALUATION USE ONLY" & vbCrLf
    End If
    S = S & "-----------" & vbCrLf
    S = S & "Inquiries about this software should be directed to" & vbCrLf
    S = S & "the organization which supplied you this software." & vbCrLf
    S = S & "-----------" & vbCrLf
    i = 0
  Else
    i = 2
  End If
  
  S = S & Space(i) & "Current Directory: " & CurDir & vbCrLf & vbCrLf
  If Len(myUci.Name) = 0 Then
    S = S & Space(i) & "No Project Active" & vbCrLf
  Else
    S = S & Space(i) & "Project File: " & myUci.Name & vbCrLf
    If Len(W_HSPFMSGWDM) > 0 Then
      S = S & Space(i) & "HSPF Message File: " & W_HSPFMSGWDM & vbCrLf
    End If
    
    For j = 0 To 4
      If Not myUci.GetWDMObj(j) Is Nothing Then
        Set curClsTserFile = myUci.GetWDMObj(j)
        S = S & Space(i) & " " & curClsTserFile.Label & _
        " File: " & curClsTserFile.Filename & vbCrLf
      End If
    Next j
  End If
  
  StatusString = S
End Function

Public Sub DeactivateAcidph()
  'always happens unless winhpsf was started with 'acidph' on the command line
  Dim lBlockDef As HspfBlockDef
  Dim vTable As Variant
  Dim lGroupDef As HspfTSGroupDef
  Dim vGroupDef As Variant
  Dim i As Long
  
  Set lBlockDef = myMsg.BlockDefs("RCHRES")
  If Not lBlockDef Is Nothing Then
    If Not lBlockDef.SectionDefs("ACIDPH") Is Nothing Then
      lBlockDef.SectionDefs.Remove ("ACIDPH")
    End If
    For Each vTable In lBlockDef.TableDefs
      If Mid(vTable.Name, 1, 5) = "ACID-" Then
        lBlockDef.TableDefs.Remove (vTable.Name)
      End If
    Next vTable
  End If
  
  i = 0
  For Each vGroupDef In myMsg.TSGroupDefs
    i = i + 1
    If vGroupDef.Name = "ACIDPH" Then
      myMsg.TSGroupDefs.Remove i
    End If
  Next vGroupDef

End Sub


