VERSION 5.00
Begin VB.Form frmGenScnNewScen 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "GenScn Scenario New"
   ClientHeight    =   6360
   ClientLeft      =   996
   ClientTop       =   1356
   ClientWidth     =   7596
   BeginProperty Font 
      Name            =   "MS Sans Serif"
      Size            =   7.8
      Charset         =   0
      Weight          =   700
      Underline       =   0   'False
      Italic          =   0   'False
      Strikethrough   =   0   'False
   EndProperty
   HelpContextID   =   58
   Icon            =   "GenNew.frx":0000
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   PaletteMode     =   1  'UseZOrder
   ScaleHeight     =   6360
   ScaleWidth      =   7596
   Begin VB.Frame Frame2 
      Caption         =   "Output Level"
      Height          =   1212
      Left            =   5040
      TabIndex        =   59
      Top             =   1200
      Width           =   2292
      Begin VB.ComboBox cmbSpecial 
         Height          =   288
         Left            =   1080
         Style           =   2  'Dropdown List
         TabIndex        =   21
         Top             =   720
         WhatsThisHelpID =   28
         Width           =   972
      End
      Begin VB.ComboBox cmbOutput 
         Height          =   288
         Left            =   1080
         Style           =   2  'Dropdown List
         TabIndex        =   19
         Top             =   360
         WhatsThisHelpID =   27
         Width           =   972
      End
      Begin VB.Label lblSpecAct 
         Alignment       =   1  'Right Justify
         Caption         =   "Special &Actions:"
         Height          =   492
         Left            =   120
         TabIndex        =   20
         Top             =   600
         WhatsThisHelpID =   28
         Width           =   852
      End
      Begin VB.Label lblGeneral 
         Alignment       =   1  'Right Justify
         Caption         =   "General:"
         Height          =   252
         Left            =   120
         TabIndex        =   18
         Top             =   360
         WhatsThisHelpID =   27
         Width           =   852
      End
   End
   Begin VB.Frame Frame1 
      Caption         =   "Run Span"
      Height          =   1212
      Left            =   240
      TabIndex        =   55
      Top             =   1200
      Width           =   2772
      Begin VB.TextBox txtEYear 
         Height          =   288
         Left            =   720
         TabIndex        =   11
         Top             =   840
         WhatsThisHelpID =   26
         Width           =   612
      End
      Begin VB.TextBox txtSYear 
         Height          =   288
         Left            =   720
         TabIndex        =   7
         Top             =   480
         WhatsThisHelpID =   25
         Width           =   612
      End
      Begin VB.TextBox txtSMonth 
         Height          =   288
         Left            =   1440
         TabIndex        =   8
         Top             =   480
         Width           =   492
      End
      Begin VB.TextBox txtEMonth 
         Height          =   288
         Left            =   1440
         TabIndex        =   12
         Top             =   840
         Width           =   492
      End
      Begin VB.TextBox txtSDay 
         Height          =   288
         Left            =   2040
         TabIndex        =   9
         Top             =   480
         Width           =   492
      End
      Begin VB.TextBox txtEDay 
         Height          =   288
         Left            =   2040
         TabIndex        =   13
         Top             =   840
         Width           =   492
      End
      Begin VB.Label lblEnd 
         Alignment       =   1  'Right Justify
         Caption         =   "&End:"
         Height          =   252
         Left            =   120
         TabIndex        =   10
         Top             =   840
         WhatsThisHelpID =   26
         Width           =   492
      End
      Begin VB.Label lblStart 
         Alignment       =   1  'Right Justify
         Caption         =   "S&tart:"
         Height          =   252
         Left            =   120
         TabIndex        =   6
         Top             =   480
         WhatsThisHelpID =   25
         Width           =   492
      End
      Begin VB.Label lblYear 
         Alignment       =   2  'Center
         Caption         =   "Year"
         Height          =   252
         Left            =   720
         TabIndex        =   58
         Top             =   240
         Width           =   612
      End
      Begin VB.Label lblMonth 
         Alignment       =   2  'Center
         Caption         =   "Month"
         Height          =   252
         Left            =   1440
         TabIndex        =   57
         Top             =   240
         Width           =   492
      End
      Begin VB.Label lblDay 
         Alignment       =   2  'Center
         Caption         =   "Day"
         Height          =   252
         Left            =   2040
         TabIndex        =   56
         Top             =   240
         Width           =   492
      End
   End
   Begin VB.CommandButton cmdNew 
      Caption         =   "&Process Files"
      Height          =   372
      Index           =   3
      Left            =   2040
      TabIndex        =   30
      Top             =   5400
      Width           =   1572
   End
   Begin TabDlg.SSTab SSNewTab 
      Height          =   2532
      Left            =   120
      TabIndex        =   22
      Top             =   2640
      Width           =   7332
      _ExtentX        =   12933
      _ExtentY        =   4466
      _Version        =   393216
      Tabs            =   2
      TabsPerRow      =   2
      TabHeight       =   420
      TabCaption(0)   =   "Input Files"
      TabPicture(0)   =   "GenNew.frx":0442
      Tab(0).ControlEnabled=   -1  'True
      Tab(0).Control(0)=   "lblFile(5)"
      Tab(0).Control(0).Enabled=   0   'False
      Tab(0).Control(1)=   "lblFile(4)"
      Tab(0).Control(1).Enabled=   0   'False
      Tab(0).Control(2)=   "lblFile(3)"
      Tab(0).Control(2).Enabled=   0   'False
      Tab(0).Control(3)=   "lblFile(2)"
      Tab(0).Control(3).Enabled=   0   'False
      Tab(0).Control(4)=   "lblFile(1)"
      Tab(0).Control(4).Enabled=   0   'False
      Tab(0).Control(5)=   "lblFile(0)"
      Tab(0).Control(5).Enabled=   0   'False
      Tab(0).Control(6)=   "lblRating"
      Tab(0).Control(6).Enabled=   0   'False
      Tab(0).Control(7)=   "lblMetsta"
      Tab(0).Control(7).Enabled=   0   'False
      Tab(0).Control(8)=   "lblLanduse"
      Tab(0).Control(8).Enabled=   0   'False
      Tab(0).Control(9)=   "lblArea"
      Tab(0).Control(9).Enabled=   0   'False
      Tab(0).Control(10)=   "lblConn"
      Tab(0).Control(10).Enabled=   0   'False
      Tab(0).Control(11)=   "lblReach"
      Tab(0).Control(11).Enabled=   0   'False
      Tab(0).Control(12)=   "CDFile(0)"
      Tab(0).Control(12).Enabled=   0   'False
      Tab(0).Control(13)=   "CDFile(1)"
      Tab(0).Control(13).Enabled=   0   'False
      Tab(0).Control(14)=   "CDFile(2)"
      Tab(0).Control(14).Enabled=   0   'False
      Tab(0).Control(15)=   "CDFile(3)"
      Tab(0).Control(15).Enabled=   0   'False
      Tab(0).Control(16)=   "CDFile(4)"
      Tab(0).Control(16).Enabled=   0   'False
      Tab(0).Control(17)=   "CDFile(5)"
      Tab(0).Control(17).Enabled=   0   'False
      Tab(0).Control(18)=   "cmdFile(5)"
      Tab(0).Control(18).Enabled=   0   'False
      Tab(0).Control(19)=   "cmdFile(4)"
      Tab(0).Control(19).Enabled=   0   'False
      Tab(0).Control(20)=   "cmdFile(3)"
      Tab(0).Control(20).Enabled=   0   'False
      Tab(0).Control(21)=   "cmdFile(2)"
      Tab(0).Control(21).Enabled=   0   'False
      Tab(0).Control(22)=   "cmdFile(1)"
      Tab(0).Control(22).Enabled=   0   'False
      Tab(0).Control(23)=   "cmdFile(0)"
      Tab(0).Control(23).Enabled=   0   'False
      Tab(0).ControlCount=   24
      TabCaption(1)   =   "Summary"
      TabPicture(1)   =   "GenNew.frx":045E
      Tab(1).ControlEnabled=   0   'False
      Tab(1).Control(0)=   "lblSum(6)"
      Tab(1).Control(1)=   "lblSum(5)"
      Tab(1).Control(2)=   "lblSum(4)"
      Tab(1).Control(3)=   "lblSum(3)"
      Tab(1).Control(4)=   "lblSum(2)"
      Tab(1).Control(5)=   "lblSum(1)"
      Tab(1).Control(6)=   "lblSum(0)"
      Tab(1).ControlCount=   7
      Begin VB.CommandButton cmdFile 
         Caption         =   "Select"
         Height          =   252
         Index           =   0
         Left            =   240
         TabIndex        =   23
         Top             =   360
         Width           =   852
      End
      Begin VB.CommandButton cmdFile 
         Caption         =   "Select"
         Height          =   252
         Index           =   1
         Left            =   240
         TabIndex        =   24
         Top             =   720
         Width           =   852
      End
      Begin VB.CommandButton cmdFile 
         Caption         =   "Select"
         Height          =   252
         Index           =   2
         Left            =   240
         TabIndex        =   25
         Top             =   1080
         Width           =   852
      End
      Begin VB.CommandButton cmdFile 
         Caption         =   "Select"
         Height          =   252
         Index           =   3
         Left            =   240
         TabIndex        =   26
         Top             =   1440
         Width           =   852
      End
      Begin VB.CommandButton cmdFile 
         Caption         =   "Select"
         Height          =   252
         Index           =   4
         Left            =   240
         TabIndex        =   27
         Top             =   1800
         Width           =   852
      End
      Begin VB.CommandButton cmdFile 
         Caption         =   "Select"
         Height          =   252
         Index           =   5
         Left            =   240
         TabIndex        =   28
         Top             =   2160
         Width           =   852
      End
      Begin MSComDlg.CommonDialog CDFile 
         Index           =   5
         Left            =   0
         Top             =   2040
         _ExtentX        =   699
         _ExtentY        =   699
         _Version        =   393216
         FontSize        =   3.90888e-38
      End
      Begin MSComDlg.CommonDialog CDFile 
         Index           =   4
         Left            =   0
         Top             =   1680
         _ExtentX        =   699
         _ExtentY        =   699
         _Version        =   393216
         FontSize        =   3.90888e-38
      End
      Begin MSComDlg.CommonDialog CDFile 
         Index           =   3
         Left            =   0
         Top             =   1320
         _ExtentX        =   699
         _ExtentY        =   699
         _Version        =   393216
         FontSize        =   3.90888e-38
      End
      Begin MSComDlg.CommonDialog CDFile 
         Index           =   2
         Left            =   0
         Top             =   960
         _ExtentX        =   699
         _ExtentY        =   699
         _Version        =   393216
         FontSize        =   3.90888e-38
      End
      Begin MSComDlg.CommonDialog CDFile 
         Index           =   1
         Left            =   0
         Top             =   600
         _ExtentX        =   699
         _ExtentY        =   699
         _Version        =   393216
         FontSize        =   3.90888e-38
      End
      Begin MSComDlg.CommonDialog CDFile 
         Index           =   0
         Left            =   0
         Top             =   240
         _ExtentX        =   699
         _ExtentY        =   699
         _Version        =   393216
         FontSize        =   3.90888e-38
      End
      Begin VB.Label lblSum 
         Height          =   252
         Index           =   6
         Left            =   -74760
         TabIndex        =   54
         Top             =   1920
         Width           =   6852
      End
      Begin VB.Label lblSum 
         Height          =   252
         Index           =   5
         Left            =   -74760
         TabIndex        =   53
         Top             =   1680
         Width           =   6852
      End
      Begin VB.Label lblSum 
         Height          =   252
         Index           =   4
         Left            =   -74760
         TabIndex        =   52
         Top             =   1440
         Width           =   6852
      End
      Begin VB.Label lblSum 
         Height          =   252
         Index           =   3
         Left            =   -74760
         TabIndex        =   51
         Top             =   1200
         Width           =   6852
      End
      Begin VB.Label lblSum 
         Height          =   252
         Index           =   2
         Left            =   -74760
         TabIndex        =   50
         Top             =   960
         Width           =   6852
      End
      Begin VB.Label lblSum 
         Height          =   252
         Index           =   1
         Left            =   -74760
         TabIndex        =   49
         Top             =   720
         Width           =   6852
      End
      Begin VB.Label lblSum 
         Height          =   252
         Index           =   0
         Left            =   -74760
         TabIndex        =   48
         Top             =   480
         Width           =   6852
      End
      Begin VB.Label lblReach 
         Caption         =   "Reaches"
         Height          =   252
         Left            =   1200
         TabIndex        =   47
         Top             =   360
         Width           =   1932
      End
      Begin VB.Label lblConn 
         Caption         =   "Connections"
         Height          =   252
         Left            =   1200
         TabIndex        =   46
         Top             =   720
         Width           =   1932
      End
      Begin VB.Label lblArea 
         Caption         =   "Areas"
         Height          =   252
         Left            =   1200
         TabIndex        =   45
         Top             =   1080
         Width           =   1932
      End
      Begin VB.Label lblLanduse 
         Caption         =   "Land Uses (optional)"
         Height          =   252
         Left            =   1200
         TabIndex        =   44
         Top             =   1800
         Width           =   1932
      End
      Begin VB.Label lblMetsta 
         Caption         =   "Met Stations (optional)"
         Height          =   252
         Left            =   1200
         TabIndex        =   43
         Top             =   2160
         Width           =   1932
      End
      Begin VB.Label lblRating 
         Caption         =   "Rating Curves"
         Height          =   252
         Left            =   1200
         TabIndex        =   42
         Top             =   1440
         Width           =   1932
      End
      Begin VB.Label lblFile 
         Caption         =   "<none>"
         Height          =   252
         Index           =   0
         Left            =   3240
         TabIndex        =   41
         Top             =   360
         Width           =   3972
      End
      Begin VB.Label lblFile 
         Caption         =   "<none>"
         Height          =   252
         Index           =   1
         Left            =   3240
         TabIndex        =   40
         Top             =   720
         Width           =   3972
      End
      Begin VB.Label lblFile 
         Caption         =   "<none>"
         Height          =   252
         Index           =   2
         Left            =   3240
         TabIndex        =   39
         Top             =   1080
         Width           =   3972
      End
      Begin VB.Label lblFile 
         Caption         =   "<none>"
         Height          =   252
         Index           =   3
         Left            =   3240
         TabIndex        =   38
         Top             =   1440
         Width           =   3972
      End
      Begin VB.Label lblFile 
         Caption         =   "<none>"
         Height          =   252
         Index           =   4
         Left            =   3240
         TabIndex        =   37
         Top             =   1800
         Width           =   3972
      End
      Begin VB.Label lblFile 
         Caption         =   "<none>"
         Height          =   252
         Index           =   5
         Left            =   3240
         TabIndex        =   36
         Top             =   2160
         Width           =   3972
      End
   End
   Begin VB.CommandButton cmdSave 
      Caption         =   "&Save Specs"
      Height          =   372
      Left            =   1080
      TabIndex        =   33
      Top             =   5880
      Width           =   1572
   End
   Begin VB.CommandButton cmdGet 
      Caption         =   "&Get Specs"
      Height          =   372
      Left            =   3000
      TabIndex        =   34
      Top             =   5880
      Width           =   1572
   End
   Begin VB.CommandButton cmdClear 
      Caption         =   "C&lear Specs"
      Height          =   372
      Left            =   4920
      TabIndex        =   35
      Top             =   5880
      Width           =   1572
   End
   Begin VB.TextBox txtInfo 
      Height          =   285
      Left            =   1080
      TabIndex        =   5
      Top             =   720
      WhatsThisHelpID =   24
      Width           =   6255
   End
   Begin VB.ComboBox cmbUnits 
      Height          =   288
      Left            =   3840
      Style           =   2  'Dropdown List
      TabIndex        =   17
      Top             =   2040
      WhatsThisHelpID =   30
      Width           =   972
   End
   Begin VB.ComboBox cmbRunflag 
      Height          =   288
      Left            =   3840
      Style           =   2  'Dropdown List
      TabIndex        =   15
      Top             =   1440
      WhatsThisHelpID =   29
      Width           =   972
   End
   Begin VB.ComboBox cmbModel 
      Height          =   288
      Left            =   3480
      TabIndex        =   3
      Text            =   "cmbModel"
      Top             =   240
      Width           =   1212
   End
   Begin VB.CommandButton cmdNew 
      Cancel          =   -1  'True
      Caption         =   "&Close"
      Height          =   372
      Index           =   2
      Left            =   5880
      TabIndex        =   32
      Top             =   5400
      Width           =   1572
   End
   Begin VB.CommandButton cmdNew 
      Caption         =   "&Build UCI"
      Height          =   372
      Index           =   1
      Left            =   3960
      TabIndex        =   31
      Top             =   5400
      Width           =   1572
   End
   Begin VB.CommandButton cmdNew 
      Caption         =   "S&implify Network"
      Height          =   372
      HelpContextID   =   68
      Index           =   0
      Left            =   120
      TabIndex        =   29
      Top             =   5400
      Width           =   1572
   End
   Begin VB.TextBox txtName 
      Height          =   288
      Left            =   1080
      TabIndex        =   1
      Top             =   240
      Width           =   1212
   End
   Begin MSComDlg.CommonDialog CDSave 
      Left            =   1200
      Top             =   5760
      _ExtentX        =   699
      _ExtentY        =   699
      _Version        =   393216
      FontSize        =   3.90888e-38
   End
   Begin MSComDlg.CommonDialog CDGet 
      Left            =   3000
      Top             =   5760
      _ExtentX        =   699
      _ExtentY        =   699
      _Version        =   393216
      FontSize        =   3.90888e-38
   End
   Begin VB.Label lblInfo 
      Alignment       =   1  'Right Justify
      Caption         =   "&Run Info:"
      Height          =   252
      Left            =   120
      TabIndex        =   4
      Top             =   720
      WhatsThisHelpID =   24
      Width           =   852
   End
   Begin VB.Label lblRunflag 
      Alignment       =   1  'Right Justify
      Caption         =   "Run &Flag:"
      Height          =   492
      Left            =   3120
      TabIndex        =   14
      Top             =   1320
      WhatsThisHelpID =   29
      Width           =   612
   End
   Begin VB.Label lblUnits 
      Alignment       =   1  'Right Justify
      Caption         =   "&Units:"
      Height          =   252
      Left            =   3240
      TabIndex        =   16
      Top             =   2040
      WhatsThisHelpID =   30
      Width           =   492
   End
   Begin VB.Label lblModel 
      Caption         =   "&Model:"
      Height          =   252
      Left            =   2760
      TabIndex        =   2
      Top             =   240
      Width           =   612
   End
   Begin VB.Label lblName 
      Caption         =   "&Name:"
      Height          =   252
      Left            =   240
      TabIndex        =   0
      Top             =   240
      Width           =   612
   End
End
Attribute VB_Name = "frmGenScnNewScen"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants
Dim oldsyear$, oldsmonth$, oldsday$
Dim oldeyear$, oldemonth$, oldeday$
Dim isyear&, ismonth&, isday&
Dim ieyear&, iemonth&, ieday&

Sub NewScenInit()
  Dim i&
  cmbModel.clear
  cmbOutput.clear
  cmbSpecial.clear
  cmbRunflag.clear
  cmbUnits.clear
  cmbModel.AddItem "BLTM"
  cmbModel.AddItem "DAFLOW"
  cmbModel.AddItem "ESTREND"
  cmbModel.AddItem "FEQ"
  cmbModel.AddItem "HSPF"
  cmbModel.AddItem "QUAL2"
  cmbModel.ListIndex = 4
  cmbOutput.AddItem 0
  cmbOutput.AddItem 1
  cmbOutput.AddItem 2
  cmbOutput.AddItem 3
  cmbOutput.AddItem 4
  cmbOutput.AddItem 5
  cmbOutput.AddItem 6
  cmbOutput.AddItem 7
  cmbOutput.AddItem 8
  cmbOutput.AddItem 9
  cmbOutput.AddItem 10
  cmbOutput.ListIndex = 1
  cmbSpecial.AddItem 0
  cmbSpecial.AddItem 1
  cmbSpecial.AddItem 2
  cmbSpecial.AddItem 3
  cmbSpecial.AddItem 4
  cmbSpecial.AddItem 5
  cmbSpecial.AddItem 6
  cmbSpecial.AddItem 7
  cmbSpecial.AddItem 8
  cmbSpecial.AddItem 9
  cmbSpecial.AddItem 10
  cmbSpecial.ListIndex = 0
  cmbRunflag.AddItem "Interp"
  cmbRunflag.AddItem "Run"
  cmbRunflag.ListIndex = 1
  cmbUnits.AddItem "English"
  cmbUnits.AddItem "Metric"
  cmbUnits.ListIndex = 0
  isyear = 0
  ismonth = 0
  isday = 0
  ieyear = 0
  iemonth = 0
  ieday = 0
  txtName = " "
  txtInfo = " "
  txtSyear = " "
  txtSMonth = " "
  txtSday = " "
  txtEyear = " "
  txtEMonth = " "
  txtEday = " "
  For i = 0 To 5
    lblFile(i).Caption = "<none>"
  Next i
  cmdNew(1).Enabled = False
  cmdNew(3).Enabled = False
  Call ClearSummary
End Sub

Private Sub PutSpecs(Filename)
  Dim i&
  Open Filename For Output As #1
  Print #1, txtName
  Print #1, cmbModel.ListIndex
  Print #1, txtInfo
  Print #1, txtSyear
  Print #1, txtSMonth
  Print #1, txtSday
  Print #1, txtEyear
  Print #1, txtEMonth
  Print #1, txtEday
  Print #1, cmbOutput.ListIndex
  Print #1, cmbSpecial.ListIndex
  Print #1, cmbRunflag.ListIndex
  Print #1, cmbUnits.ListIndex
  For i = 0 To 5
    Print #1, lblFile(i).Caption
  Next i
  Close #1
End Sub

Public Sub ClearSummary()
  Dim i&
  For i = 0 To 6
    lblSum(i).Caption = ""
  Next i
End Sub

Private Sub FillSummary()
  Dim nr&, nc&, nlu&, nm&, ne&, nout&, i&, cnamstr$, nexts&
    Call F90_UCGNRC(nr)
    lblSum(0).Caption = nr & " Reaches"
    Call F90_UCGNCO(nc)
    lblSum(1).Caption = nc & " Connections"
    Call F90_UCGNLA(nlu)
    Call F90_UCGNME(nm, ne)
    If lblFile(5).Caption <> "<none>" Then
      lblSum(2).Caption = nlu & " Land Uses"
      lblSum(3).Caption = nm & " Met Segments"
      lblSum(6).Caption = "Files and data sets for use with the Expert System will be generated."
    Else
      lblSum(2).Caption = "No land surface response."
      lblSum(3).Caption = "Local inflows will be estimated using observed flow data."
    End If
    ReDim iout&(nr)
    Dim cname$
    nout = 0
    Call F90_UCGOUT(nr, iout(0))
    For i = 0 To nr - 1
      If iout(i) > 0 Then
        nout = nout + 1
        Call F90_UCGIRC(i + 1, cname)
        If nout = 1 Then
          cnamstr = cname
        Else
          cnamstr = cnamstr & ", " & cname
        End If
      End If
    Next i
    If nout = 1 Then
      lblSum(5).Caption = "Data will be output at the following site: " & cnamstr
    ElseIf nout > 1 Then
      lblSum(5).Caption = "Data will be output at the following sites: " & cnamstr
    ElseIf nout = 0 Then
      lblSum(5).Caption = "No output sites have been found."
    End If
    Call F90_UCGNEX(nexts)
    If nexts = 1 Then
      lblSum(4).Caption = "1 external source flow data set"
    ElseIf nexts > 1 Then
      lblSum(4).Caption = nexts & " external source flow data sets"
    End If
End Sub

Private Sub GetSpecs(Filename)
  Dim ichr$, inum As Variant, i&
  On Error GoTo 10
  Open Filename For Input As #1
  Call NewScenInit
  Line Input #1, ichr
  txtName = ichr
  Line Input #1, inum
  cmbModel.ListIndex = inum
  Line Input #1, ichr
  txtInfo = ichr
  Line Input #1, ichr
  txtSyear = ichr
  isyear = val(txtSyear)
  Line Input #1, ichr
  txtSMonth = ichr
  ismonth = val(txtSMonth)
  Line Input #1, ichr
  txtSday = ichr
  isday = val(txtSday)
  Line Input #1, ichr
  txtEyear = ichr
  ieyear = val(txtEyear)
  Line Input #1, ichr
  txtEMonth = ichr
  iemonth = val(txtEMonth)
  Line Input #1, ichr
  txtEday = ichr
  ieday = val(txtEday)
  Line Input #1, inum
  cmbOutput.ListIndex = inum
  Line Input #1, inum
  cmbSpecial.ListIndex = inum
  Line Input #1, inum
  cmbRunflag.ListIndex = inum
  Line Input #1, inum
  cmbUnits.ListIndex = inum
  For i = 0 To 5
    Line Input #1, ichr
    lblFile(i).Caption = ichr
  Next i
  Close #1
  If lblFile(0).Caption <> "<none>" And _
     lblFile(1).Caption <> "<none>" And _
     lblFile(2).Caption <> "<none>" And _
     lblFile(3).Caption <> "<none>" Then
    cmdNew(3).Enabled = True
  End If
  GoTo 20
10  Close #1
    Call NewScenInit
20  'go here if everything read okay
End Sub



Private Sub cmbModel_LostFocus()
  If cmbModel.ListIndex <> 4 Then
    MsgBox cmbModel.text & " is not yet supported." & Chr(13) & Chr(10) & _
           "The Model will be reset to HSPF.", vbExclamation, "GenScn Scenario New"
    cmbModel.SetFocus
    cmbModel.ListIndex = 4
  End If
End Sub

Private Sub cmdClear_Click()
    Call NewScenInit
End Sub


Private Sub cmdFile_Click(Index As Integer)
  CDFile(Index).flags = &H8806&
  If Index = 3 Then
    CDFile(Index).filter = "ASCII files (*.inp)|*.inp|All files (*.*)|*.*"
  Else
    CDFile(Index).filter = "ASCII files (*.inp)|*.inp|RDB files (*.rdb)|*.rdb|All files (*.*)|*.*"
  End If
  If Index = 0 Then
    CDFile(Index).Filename = "reach.inp"
    CDFile(Index).DialogTitle = "GenScn Scenario New Reach File"
  ElseIf Index = 1 Then
    CDFile(Index).Filename = "rconn.inp"
    CDFile(Index).DialogTitle = "GenScn Scenario New Connection File"
  ElseIf Index = 2 Then
    CDFile(Index).Filename = "area.inp"
    CDFile(Index).DialogTitle = "GenScn Scenario New Area File"
  ElseIf Index = 3 Then
    CDFile(Index).Filename = "rating.inp"
    CDFile(Index).DialogTitle = "GenScn Scenario New Rating Curve File"
  ElseIf Index = 4 Then
    CDFile(Index).Filename = "landuse.inp"
    CDFile(Index).DialogTitle = "GenScn Scenario New Land Use File"
  ElseIf Index = 5 Then
    CDFile(Index).Filename = "metsta.inp"
    CDFile(Index).DialogTitle = "GenScn Scenario New Met Station File"
  End If
  On Error GoTo 10
  CDFile(Index).CancelError = True
  CDFile(Index).Action = 1
  lblFile(Index).Caption = CDFile(Index).Filename
10      'continue here on cancel
  If lblFile(0).Caption <> "<none>" And _
     lblFile(1).Caption <> "<none>" And _
     lblFile(2).Caption <> "<none>" And _
     lblFile(3).Caption <> "<none>" Then
    cmdNew(3).Enabled = True
  End If
  cmdNew(1).Enabled = False
  Call ClearSummary
End Sub

Private Sub cmdGet_Click()
    CDGet.flags = &H1806&
    CDGet.filter = "GenScn New Scenario Files (*.gns)|*.gns"
    CDGet.Filename = "*.gns"
    CDGet.DialogTitle = "GenScn New Scenario Get Specs File"
    On Error GoTo 10
    CDGet.CancelError = True
    CDGet.Action = 1
    Call GetSpecs(CDGet.Filename)
10 'continue here on cancel
End Sub

Private Sub cmdNew_Click(Index As Integer)
  Dim newName$, sdatim&(5), edatim&(5), expfg&
  Dim outlev&, resmfg&, runfg&, spout&, Unit&, retcod&
  Dim invalid&, i&
  Dim rninfo$, rchnam$, connam$, arenam$, ftbnam$, lunam$, metnam$, defname$
  If Index = 0 Then
    'simplify network
    Unload frmGenScnSimplify
    frmGenScnSimplify.Show
  ElseIf Index = 3 Then
    'preprocess files for ucicre
    MousePointer = vbHourglass
    newName = txtName.text
    If newName <> "" Then
      newName = UCase(newName)
      invalid = 0
      For i = 0 To frmGenScn.lstSLC(0).ListCount - 1
        If frmGenScn.lstSLC(0).List(i) = newName Then
          'invalid, name already used
          invalid = 1
        End If
      Next i
      If invalid = 1 Then
        MsgBox "This scenario name is already in use.", _
        vbExclamation, "GenScn Scenario New"
      Else
        If cmbModel.ListIndex = 4 Then
          'hspf chosen, okay
          If lblFile(0).Caption = "<none>" Or _
             lblFile(1).Caption = "<none>" Or _
             lblFile(2).Caption = "<none>" Or _
             lblFile(3).Caption = "<none>" Then
            'this file is not optional
            MsgBox "One or more required files have not been selected.", _
              vbExclamation, "GenScn Scenario New"
          Else
            If isyear = 0 Or ismonth = 0 Or isday = 0 Or _
               ieyear = 0 Or iemonth = 0 Or ieday = 0 Then
              'problem with dates
              MsgBox "One or more date specifications have not been entered.", _
              vbExclamation, "GenScn Scenario New"
            Else
              'okay to run
              outlev = cmbOutput.ListIndex
              spout = cmbSpecial.ListIndex
              runfg = cmbRunflag.ListIndex
              Unit = cmbUnits.ListIndex + 1
              resmfg = 0
              sdatim(0) = isyear
              sdatim(1) = ismonth
              sdatim(2) = isday
              sdatim(3) = 0
              sdatim(4) = 0
              edatim(0) = ieyear
              edatim(1) = iemonth
              edatim(2) = ieday
              edatim(3) = 0
              edatim(4) = 0
              expfg = 0
              If Len(txtInfo.text) = 0 Then
                rninfo = "<none>"
              Else
                rninfo = txtInfo.text
              End If
              rchnam = lblFile(0).Caption
              connam = lblFile(1).Caption
              arenam = lblFile(2).Caption
              ftbnam = lblFile(3).Caption
              lunam = lblFile(4).Caption
              metnam = lblFile(5).Caption
              ChDriveDir p.StatusFilePath
              Call F90_UMAKPR(p.WDMFiles(1).fileUnit, outlev, resmfg, _
                              runfg, spout, Unit, sdatim(0), edatim(0), _
                              expfg, retcod, rchnam, connam, arenam, _
                              ftbnam, lunam, metnam, newName, rninfo, _
                              Len(rchnam), Len(connam), Len(arenam), _
                              Len(ftbnam), Len(lunam), Len(metnam), _
                              Len(newName), Len(rninfo))
              If retcod = 3 Then
                'triple junction problem
                MsgBox "This network contains one or more triple junctions.", _
                vbExclamation, "GenScn Scenario New"
              ElseIf retcod = 2 Then
                'multiple outlets problem
                MsgBox "This network contains multiple outlets.", _
                vbExclamation, "GenScn Scenario New"
              ElseIf retcod = 1 Then
                'misordered reaches problem
                MsgBox "Reaches are not numbered sequentially.", _
                vbExclamation, "GenScn Scenario New"
              ElseIf retcod = -1 Then
                'file read error
                MsgBox "Error reading files.", _
                vbExclamation, "GenScn Scenario New"
              ElseIf retcod = -2 Then
                'file open error
                MsgBox "Error opening files.", _
                vbExclamation, "GenScn Scenario New"
              ElseIf retcod = 0 Then
                'everything is okay, may build
                cmdNew(1).Enabled = True
                Call FillSummary
                SSNewTab.Tab = 1
              End If
            End If
          End If
        Else
          'this model not yet supported
          MsgBox "The specified model is not yet supported.", _
            vbExclamation, "GenScn Scenario New"
        End If
      End If
    Else
      'no scenario name entered
      MsgBox "No scenario name has been entered.", _
        vbExclamation, "GenScn Scenario New"
    End If
    MousePointer = vbDefault
  ElseIf Index = 2 Then
    'close
    defname = p.StatusFilePath & "\default.gns"
    Call PutSpecs(defname)
    Unload frmGenScnNewScen
  ElseIf Index = 1 Then
    'build uci file
    MousePointer = vbHourglass
    newName = txtName.text
    If newName <> "" Then
      newName = UCase(newName)
      invalid = 0
      For i = 0 To frmGenScn.lstSLC(0).ListCount - 1
        If frmGenScn.lstSLC(0).List(i) = newName Then
          'invalid, name already used
          invalid = 1
        End If
      Next i
      If invalid = 1 Then
        MsgBox "This scenario name is already in use.", _
        vbExclamation, "GenScn Scenario New"
      Else
        'okay name
        expfg = 0
        If (Len(lblFile(4).Caption) > 0 And _
          Len(lblFile(5).Caption) > 0) Then
          expfg = 1
        End If
        ChDriveDir p.StatusFilePath
        Call F90_UMAKDO(p.HSPFMsg.Unit, _
                        expfg, newName, _
                        Len(newName))
        'successful creation
        MsgBox "Successful creation of new scenario.", 0, _
          "GenScn Scenario New"
        cmdNew(1).Enabled = False
        'reset scenario list and number of data sets
        p.ScenName.Add newName, newName
        frmGenScn.lstSLC(0).AddItem newName
        Dim Scen$, newcnt&
        'scen = "Scenarios:" & Chr(13) & Chr(10)
        Scen = frmGenScn.lstSLC(0).SelCount & " of " & frmGenScn.lstSLC(0).ListCount
        frmGenScn.lblSLC(0).Caption = Scen
        'count number of new data sets added
        'Call F90_TSESPC(newname, " ", " ", Len(RTrim(newname)), 1, 1)
        'dsn = 1
        'newcnt = 0
        'Do While dsn > 0
        '  Call WDMDir.TSDSM(dsn)
        '  If dsn > 0 Then
        '    newcnt = newcnt + 1
        '    dsn = dsn + 1
        '  End If
        'Loop
        'CntDsn = CntDsn + newcnt
        
        'may need to refresh wdm data class
        
        Call frmGenScn.UpdateLblDsn
      End If
    Else
      'no scenario name entered
      MsgBox "No scenario name has been entered.", _
        vbExclamation, "GenScn Scenario New"
    End If
    MousePointer = vbDefault
  End If
End Sub

Private Sub cmdSave_Click()
    CDSave.flags = &H8806&
    CDSave.filter = "GenScn New Scenario Files (*.gns)|*.gns"
    CDSave.Filename = "*.gns"
    CDSave.DialogTitle = "GenScn New Scenario Save Specs File"
    On Error GoTo 10
    CDSave.CancelError = True
    CDSave.Action = 2
    Call PutSpecs(CDSave.Filename)
10 'continue here on cancel
End Sub

Private Sub Form_Load()
  Dim defname$
  cmdNew(1).Enabled = False
  cmdNew(3).Enabled = False
  defname = p.StatusFilePath & "\default.gns"
  Call GetSpecs(defname)
  Call ClearSummary
  SSNewTab.Tab = 0
End Sub

Private Sub txtEDay_GotFocus()
  oldeday = txtEday.text
End Sub


Private Sub txtEDay_LostFocus()
    Dim edaynew$, chgflg&
    If Len(txtEday.text) > 0 Then
      edaynew = txtEday.text
      Call ChkTxtI("Day", 1, 31, edaynew, ieday, chgflg)
      If chgflg <> 1 Then
        txtEday.text = oldeday
      End If
    Else
      txtEday.text = oldeday
    End If
End Sub


Private Sub txtEMonth_GotFocus()
    oldemonth = txtEMonth.text
End Sub

Private Sub txtEMonth_LostFocus()
    Dim emonthnew$, chgflg&
    If Len(txtEMonth.text) > 0 Then
      emonthnew = txtEMonth.text
      Call ChkTxtI("Month", 1, 12, emonthnew, iemonth, chgflg)
      If chgflg <> 1 Then
        txtEMonth.text = oldemonth
      End If
    Else
      txtEMonth.text = oldemonth
    End If
End Sub


Private Sub txtEyear_GotFocus()
    oldeyear = txtEyear.text
End Sub


Private Sub txtEyear_LostFocus()
    Dim eyearnew$, chgflg&
    If Len(txtEyear.text) > 0 Then
      eyearnew = txtEyear.text
      Call ChkTxtI("Year", 1900, 9999, eyearnew, ieyear, chgflg)
      If chgflg <> 1 Then
        txtEyear.text = oldeyear
      End If
    Else
      txtEyear.text = oldeyear
    End If
End Sub


Private Sub txtSDay_GotFocus()
  oldsday = txtSday.text
End Sub


Private Sub txtSDay_LostFocus()
    Dim sdaynew$, chgflg&
    If Len(txtSday.text) > 0 Then
      sdaynew = txtSday.text
      Call ChkTxtI("Day", 1, 31, sdaynew, isday, chgflg)
      If chgflg <> 1 Then
        txtSday.text = oldsday
      End If
    Else
      txtSday.text = oldsday
    End If
End Sub


Private Sub txtSMonth_GotFocus()
  oldsmonth = txtSMonth.text
End Sub


Private Sub txtSMonth_LostFocus()
    Dim smonthnew$, chgflg&
    If Len(txtSMonth.text) > 0 Then
      smonthnew = txtSMonth.text
      Call ChkTxtI("Month", 1, 12, smonthnew, ismonth, chgflg)
      If chgflg <> 1 Then
        txtSMonth.text = oldsmonth
      End If
    Else
      txtSMonth.text = oldsmonth
    End If
End Sub


Private Sub txtSyear_GotFocus()
  oldsyear = txtSyear.text
End Sub


Private Sub txtSyear_LostFocus()
    Dim syearnew$, chgflg&
    If Len(txtSyear.text) > 0 Then
      syearnew = txtSyear.text
      Call ChkTxtI("Year", 1900, 9999, syearnew, isyear, chgflg)
      If chgflg <> 1 Then
        txtSyear.text = oldsyear
      End If
    Else
      txtSyear.text = oldsyear
    End If
End Sub
