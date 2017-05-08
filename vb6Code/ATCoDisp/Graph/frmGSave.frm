VERSION 5.00
Begin VB.Form frmGSave 
   Caption         =   "Graph Save"
   ClientHeight    =   4605
   ClientLeft      =   60
   ClientTop       =   345
   ClientWidth     =   9360
   Icon            =   "frmGSave.frx":0000
   LinkTopic       =   "Form1"
   ScaleHeight     =   4605
   ScaleWidth      =   9360
   StartUpPosition =   3  'Windows Default
   Begin VB.Frame fraButtons 
      BorderStyle     =   0  'None
      Caption         =   "Frame1"
      Height          =   372
      Left            =   360
      TabIndex        =   18
      Top             =   4080
      Width           =   2292
      Begin VB.CommandButton cmdSave 
         Caption         =   "&Save"
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
         Height          =   375
         Left            =   0
         TabIndex        =   1
         Top             =   0
         Width           =   975
      End
      Begin VB.CommandButton cmdCancel 
         Cancel          =   -1  'True
         Caption         =   "&Cancel"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   375
         Left            =   1320
         TabIndex        =   2
         Top             =   0
         Width           =   975
      End
   End
   Begin ATCoCtl.ATCoBrowse browse 
      Height          =   4212
      Left            =   3240
      TabIndex        =   0
      Top             =   240
      Width           =   6012
      _ExtentX        =   10610
      _ExtentY        =   7435
   End
   Begin VB.Frame fraSpecs 
      BorderStyle     =   0  'None
      Caption         =   "Frame1"
      Height          =   3252
      Left            =   240
      TabIndex        =   14
      Top             =   480
      Visible         =   0   'False
      Width           =   2772
      Begin VB.CheckBox chkCurve 
         Caption         =   "Curve Properties"
         Height          =   255
         Left            =   240
         TabIndex        =   4
         ToolTipText     =   "styles, colors, labels"
         Top             =   120
         Value           =   1  'Checked
         Width           =   2292
      End
      Begin VB.CheckBox chkLegLoc 
         Caption         =   "Legend Location"
         Height          =   255
         Left            =   240
         TabIndex        =   6
         Top             =   600
         Value           =   1  'Checked
         Width           =   2292
      End
      Begin VB.CheckBox chkMainTitle 
         Caption         =   "Main Title, Window Caption"
         Height          =   255
         Left            =   240
         TabIndex        =   8
         Top             =   1080
         Value           =   1  'Checked
         Width           =   2292
      End
      Begin VB.Frame fraAxis 
         Caption         =   "Axis Properites"
         Height          =   1452
         Left            =   0
         TabIndex        =   15
         Top             =   1680
         Width           =   2532
         Begin VB.CheckBox chkTitles 
            Caption         =   "Titles"
            Height          =   255
            Left            =   240
            TabIndex        =   10
            Top             =   360
            Value           =   1  'Checked
            Width           =   2052
         End
         Begin VB.CheckBox chkTypes 
            Caption         =   "Types (arithmetic, log)"
            Enabled         =   0   'False
            Height          =   255
            Left            =   240
            TabIndex        =   11
            Top             =   600
            Width           =   2172
         End
         Begin VB.CheckBox chkScales 
            Caption         =   "Scales and Tics"
            Height          =   255
            Left            =   240
            TabIndex        =   12
            Top             =   840
            Value           =   1  'Checked
            Width           =   2172
         End
         Begin VB.CheckBox chkGrid 
            Caption         =   "Grid"
            Height          =   255
            Left            =   240
            TabIndex        =   13
            Top             =   1080
            Value           =   1  'Checked
            Width           =   2052
         End
      End
      Begin VB.CheckBox chkAddTxt 
         Caption         =   "Additional Text"
         Height          =   255
         Left            =   240
         TabIndex        =   7
         Top             =   840
         Value           =   1  'Checked
         Width           =   2292
      End
      Begin VB.CheckBox chkDataLabels 
         Caption         =   "Data Labels"
         Height          =   255
         Left            =   240
         TabIndex        =   9
         Top             =   1320
         Value           =   1  'Checked
         Width           =   2292
      End
      Begin VB.CheckBox chkLine 
         Caption         =   "Formula Lines"
         Height          =   255
         Left            =   240
         TabIndex        =   5
         Top             =   360
         Value           =   1  'Checked
         Width           =   2292
      End
   End
   Begin VB.Frame fraPicture 
      BorderStyle     =   0  'None
      Caption         =   "Frame1"
      Height          =   3132
      Left            =   240
      TabIndex        =   16
      Top             =   480
      Width           =   2772
      Begin VB.OptionButton optPictureType 
         Caption         =   "Windows Bitmap (*.bmp)"
         Height          =   492
         Index           =   0
         Left            =   0
         TabIndex        =   17
         ToolTipText     =   "Windows Bitmap"
         Top             =   0
         Value           =   -1  'True
         Width           =   2772
      End
   End
   Begin ComctlLib.TabStrip tabSaveType 
      Height          =   3732
      Left            =   120
      TabIndex        =   3
      Top             =   120
      Width           =   3012
      _ExtentX        =   5318
      _ExtentY        =   6588
      _Version        =   327682
      BeginProperty Tabs {0713E432-850A-101B-AFC0-4210102A8DA7} 
         NumTabs         =   2
         BeginProperty Tab1 {0713F341-850A-101B-AFC0-4210102A8DA7} 
            Caption         =   "Picture"
            Key             =   ""
            Object.Tag             =   ""
            ImageVarType    =   2
         EndProperty
         BeginProperty Tab2 {0713F341-850A-101B-AFC0-4210102A8DA7} 
            Caption         =   "Specification"
            Key             =   ""
            Object.Tag             =   ""
            ImageVarType    =   2
         EndProperty
      EndProperty
   End
End
Attribute VB_Name = "frmGSave"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2001 by AQUA TERRA Consultants

Private SaveFilename As String
Private SaveExtension As String
Private pIPC As ATCoIPC ' AtCoLaunch
Private pScrGraph As PictureBox

Public Sub SetFilename(newName$)
  SaveFilename = newName
  browse.Filename = newName
  browse.Pattern = "*." & SaveExtension
End Sub

Private Sub cmdCancel_Click()
  Me.Hide
End Sub

Private Sub cmdSave_Click()
  On Error GoTo ExitSub
  SaveFilename = browse.Filename
  
  If Len(SaveFilename) > 0 Then
    SaveFilename = FilenameSetExt(SaveFilename, SaveExtension)
    If Len(Dir(SaveFilename)) > 0 Then
      If MsgBox(SaveFilename & vbCr & "Overwrite existing file?", vbYesNo, "Save Graph") = vbNo Then
        Exit Sub
      Else
        Kill SaveFilename
      End If
    End If
    On Error GoTo SaveError
    Select Case LCase(SaveExtension)
      Case "grf"
        Dim fu%
        fu = FreeFile(0)
        Open SaveFilename For Output As #fu
        SaveGraphSpec fu
        Close fu
      Case "bmp"
        SavePicture pScrGraph.Image, SaveFilename
'      Case "wmf"
'        SavePicture pScrGraph.Picture, SaveFilename
      Case Else
        pIPC.SavePictureAs pScrGraph, SaveFilename
    End Select
  End If
  
  SaveSetting App.EXEName, "Defaults", "GraphSaveType", SaveExtension

ExitSub:
  Me.Hide
  Exit Sub
SaveError:
  MsgBox "Could not save " & SaveFilename & vbCr & Err.Description, vbOKOnly, "Error"
End Sub

Public Sub SaveGraphSpec(fu%)
  Dim x!, y!, titl$, capt$, DLpos&, i&
    
  Dim Xtyp&, Ytyp&, YRtyp&, auxlen!, xlab$, ylab$, yrlab$, auxlab$
  Dim XStyp&, Xint& 'dont save these specs, force a look at data
  
  Call frmG.GetAxesInfo(Xtyp, XStyp, Xint, Ytyp, YRtyp, auxlen, xlab, ylab, yrlab, auxlab)
  
  If chkTitles.Value = 1 Then
    If Len(xlab) > 0 And Xtyp > 0 Then Print #fu, "Xlab '" & xlab & "'"
    If Len(ylab) > 0 Then Print #fu, "Ylab '" & ylab & "'"
    If Len(yrlab) > 0 And YRtyp > 0 Then Print #fu, "YRlab '" & yrlab & "'"
    If Len(auxlab) > 0 And auxlen > 0 Then Print #fu, "Alab '" & auxlab & "'"
  End If

  If chkTypes.Value = 1 Then
    Print #fu, "Xtyp " & Xtyp
    If Ytyp > 0 Then Print #fu, "Ytyp " & Ytyp
    If YRtyp > 0 Then Print #fu, "YRtyp " & YRtyp
  End If

  If chkScales.Value = 1 Then
    Dim pmin(1 To 4) As Single  'min axis value
    Dim pmax(1 To 4) As Single  'max axis value
    Dim NTic(1 To 4) As Long    'number of tics
    Call frmG.GetScale(pmin, pmax, NTic)
    If Ytyp > 0 Then Print #fu, "Scale 1 " & pmin(1) & " " & pmax(1) & " " & NTic(1)
    If YRtyp > 0 Then Print #fu, "Scale 2 " & pmin(2) & " " & pmax(2) & " " & NTic(2)
    If auxlen > 0 Then
      Print #fu, "Alen " & auxlen
      Print #fu, "Scale 3 " & pmin(3) & " " & pmax(3) & " " & NTic(3)
    End If
    If Xtyp > 0 Then Print #fu, "Scale 4 " & pmin(4) & " " & pmax(4) & " " & NTic(4)
  End If
  
  If chkGrid.Value = 1 Then
    Dim Xgrd&, Ygrd&, YRgrd&
    Call frmG.GetGrid(Xgrd, Ygrd, YRgrd)
    Print #fu, "Xgrd " & Xgrd
    If Ytyp > 0 Then Print #fu, "Ygrd " & Ygrd
    If YRtyp > 0 Then Print #fu, "YRgrd " & YRgrd
  End If

  If chkCurve.Value = 1 Then
    Dim ctyp() As Long
    Dim ltyp() As Long
    Dim lthk() As Long
    Dim styp() As Long
    Dim clr() As Long
    Dim legend() As String
    Dim lncrv&
    
    Dim ltstep() As Long
    Dim ltunit() As Long
    Dim ldtype() As Long
    Dim lsdate(5) As Long
    Dim ledate(5) As Long
    
    Dim vmin() As Single  'min data value/variable
    Dim vmax() As Single  'max data value/variable
    Dim which() As Long   'which axis code
    Dim tran() As Long    'transformation, 1-AR, 2-LG
    Dim vlab() As String  'label
    Dim lnvar As Long
    
    lncrv = frmG.NumCurves

    ReDim ctyp(lncrv - 1)
    ReDim ltyp(lncrv - 1)
    ReDim lthk(lncrv - 1)
    ReDim styp(lncrv - 1)
    ReDim clr(lncrv - 1)
    ReDim legend(lncrv - 1)
    Call frmG.GetCurveInfo(ctyp, ltyp, lthk, styp, clr, legend)

    'get time info
    ReDim ltstep(lncrv - 1)
    ReDim ltunit(lncrv - 1)
    ReDim ldtype(lncrv - 1)
    Call frmG.GetTime(ltstep, ltunit, lsdate, ledate, ldtype) 'Just using date type (point or mean)

    For i = 0 To lncrv - 1
      Print #fu, "Curve " & i & " " & ctyp(i) & " " & ltyp(i) & " " & lthk(i) & " " & styp(i) & " " & colorName(clr(i)) & " " & ldtype(i) & " '" & Trim(legend(i)) & "'"
    Next i
  
    lnvar = frmG.NumVars
    ReDim vmin(lnvar - 1)
    ReDim vmax(lnvar - 1)
    ReDim which(lnvar - 1)
    ReDim tran(lnvar - 1)
    ReDim vlab(lnvar - 1)
    Call frmG.GetVarInfo(vmin, vmax, which, tran, vlab)
    For i = 0 To lnvar - 1
      Print #fu, "Var " & i & " " & which(i) & " " & vlab(i)
    Next
  
  
  End If

  If chkLine.Value = 1 Then
    Dim lnNonDataLines&, WchAx&, LType&, LThck&, SType&, Color&, LegLbl$, Formula$
    lnNonDataLines = frmG.NumLines
    For i = 1 To lnNonDataLines
      frmG.GetLine i, WchAx, LType, LThck, SType, Color, LegLbl, Formula
      Print #fu, "Line " & i & " " & WchAx & " " & LType & " " & LThck & " " & SType & " " & colorName(Color) & " '" & Trim(LegLbl) & "'" & " '" & Trim(Formula) & "'"
    Next i
  End If

  If chkLegLoc.Value = 1 Then
    Call frmG.GetLegLoc(x, y)
    Print #fu, "LocLegend " & x & " " & y
  End If
  
  If chkAddTxt.Value = 1 Then
    Call frmG.GetAddText(x, y, titl)
    Print #fu, "AddText '" & titl & "' " & x & " " & y
  End If

  If chkMainTitle.Value = 1 Then
    Call frmG.GetTitles(titl, capt)
    Print #fu, "Title '" & titl & "' '" & capt & "'"
  End If

  If chkDataLabels.Value = 1 Then
    Print #fu, "DataLabels " & frmG.GetDataLabelPosition
  End If

End Sub

Private Sub Form_Load()
  SaveExtension = GetSetting(App.EXEName, "Defaults", "GraphSaveType", "bmp")
  If SaveExtension = "" Then SaveExtension = "bmp"
  browse.Filename = "Untitled." & SaveExtension
  browse.Pattern = "*." & SaveExtension
  If SaveExtension = "grf" Then
    tabSaveType.SelectedItem = tabSaveType.Tabs(2)
  Else
    tabSaveType.SelectedItem = tabSaveType.Tabs(1)
  End If
  PopulatePictureType
End Sub

Private Sub Form_Resize()
  Dim w&, h&
  w = Me.ScaleWidth
  h = Me.ScaleHeight
  If w > 3400 And h > 1500 Then
    browse.Width = w - 3360
    browse.Height = h - 408
    fraButtons.Top = h - 528
    tabSaveType.Height = h - 876
    If tabSaveType.Height > 480 Then fraPicture.Height = tabSaveType.Height - 480
    fraSpecs.Height = fraPicture.Height
  End If
End Sub

Private Sub optPictureType_Click(index As Integer)
  tabSaveType_Click
End Sub

Private Sub tabSaveType_Click()
  Dim butt As Long
  
  Debug.Print "tabSaveType_Click"
  
  If tabSaveType.SelectedItem.index = 2 Then
    fraSpecs.Visible = True
    fraPicture.Visible = False
    SaveExtension = "grf"
  Else
    fraPicture.Visible = True
    fraSpecs.Visible = False
    For butt = 0 To optPictureType.Count - 1
      If optPictureType(butt).Value And Len(optPictureType(butt).Tag) > 0 Then
        SaveExtension = LCase(optPictureType(butt).Tag)
      End If
    Next
    PopulatePictureType
  End If
  If SaveExtension = "clb" Then
    browse.Visible = False
  Else
    browse.Visible = True
    browse.Filename = FilenameSetExt(browse.Filename, SaveExtension)
    browse.Pattern = "*." & SaveExtension
  End If
End Sub

Public Property Get IPC() As ATCoIPC 'AtCoLaunch
  Set IPC = pIPC
End Property

Public Property Set IPC(ByVal newIPC As ATCoIPC) ' AtCoLaunch)
  Set pIPC = newIPC
  PopulatePictureType
End Property

Public Property Set scrGraph(ByVal newScrGraph As PictureBox)
  Set pScrGraph = newScrGraph
End Property

Private Sub PopulatePictureType()
  Dim AvailList As String, AvailLabel As String, AvailPattern As String, delim As Long
  Dim optIndex As Long
  
  Debug.Print "PopulatePictureType"
  
  If pIPC Is Nothing Then    'We will just offer BMP
    AvailList = "Windows Bitmap (*.bmp)|*.bmp"
  Else
    'AvailList = "Windows Metafile (*.wmf)|*.wmf|" & pIPC.SavePictureAvailableTypes
    AvailList = pIPC.SavePictureAvailableTypes
  End If
  
  optIndex = -1
  delim = InStr(AvailList, "|")
  While delim > 0
    AvailLabel = Left(AvailList, delim - 1)
    AvailList = Mid(AvailList, delim + 1)
    delim = InStr(AvailList, "|")
    If delim = 0 Then delim = Len(AvailList) + 1
    AvailPattern = Left(AvailList, delim - 1)
    AvailList = Mid(AvailList, delim + 1)
    optIndex = optIndex + 1
    If optIndex > optPictureType.Count - 1 Then
      Load optPictureType(optIndex)
    End If
    With optPictureType(optIndex)
      .Caption = AvailLabel
      .Tag = LCase(Right(AvailPattern, 3))
      .Visible = True
      If .Tag = SaveExtension Then .Value = True
      If optIndex > 0 Then .Top = optPictureType(optIndex - 1).Top + 360
    End With
    delim = InStr(AvailList, "|")
  Wend
  optIndex = optIndex + 1
  While optPictureType.Count > optIndex
    Unload optPictureType(optPictureType.Count - 1)
  Wend
End Sub
