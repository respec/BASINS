VERSION 5.00
Begin VB.Form frmGenScnImportWD 
   Caption         =   "GenScn Activate OBSERVED Watstore"
   ClientHeight    =   6144
   ClientLeft      =   948
   ClientTop       =   2196
   ClientWidth     =   6528
   BeginProperty Font 
      Name            =   "MS Sans Serif"
      Size            =   7.8
      Charset         =   0
      Weight          =   700
      Underline       =   0   'False
      Italic          =   0   'False
      Strikethrough   =   0   'False
   EndProperty
   HelpContextID   =   53
   Icon            =   "GenImpWD.frx":0000
   LinkTopic       =   "Form1"
   PaletteMode     =   1  'UseZOrder
   ScaleHeight     =   6144
   ScaleWidth      =   6528
   Begin VB.CommandButton cmdImport 
      Caption         =   "Begin"
      Height          =   375
      Index           =   3
      Left            =   360
      TabIndex        =   20
      Top             =   5640
      Width           =   972
   End
   Begin VB.Frame frmSpecs 
      Caption         =   "Data Set Specifications"
      Enabled         =   0   'False
      Height          =   4812
      Left            =   120
      TabIndex        =   25
      Top             =   720
      Width           =   6252
      Begin VB.TextBox txtAtt 
         Enabled         =   0   'False
         Height          =   288
         Index           =   12
         Left            =   1680
         TabIndex        =   9
         Text            =   "<none>"
         Top             =   3600
         Width           =   972
      End
      Begin VB.TextBox txtAtt 
         Enabled         =   0   'False
         Height          =   288
         Index           =   11
         Left            =   4920
         TabIndex        =   15
         Text            =   "<none>"
         Top             =   3240
         Width           =   972
      End
      Begin VB.TextBox txtAtt 
         Enabled         =   0   'False
         Height          =   288
         Index           =   10
         Left            =   4920
         TabIndex        =   12
         Text            =   "<none>"
         Top             =   2160
         Width           =   972
      End
      Begin VB.TextBox txtAtt 
         Enabled         =   0   'False
         Height          =   288
         Index           =   9
         Left            =   4920
         TabIndex        =   11
         Text            =   "<none>"
         Top             =   1800
         Width           =   972
      End
      Begin VB.TextBox txtAtt 
         Enabled         =   0   'False
         Height          =   288
         Index           =   8
         Left            =   4920
         TabIndex        =   10
         Text            =   "<none>"
         Top             =   1440
         Width           =   972
      End
      Begin VB.TextBox txtAtt 
         Enabled         =   0   'False
         Height          =   288
         Index           =   7
         Left            =   4920
         TabIndex        =   14
         Text            =   "<none>"
         Top             =   2880
         Width           =   972
      End
      Begin VB.TextBox txtAtt 
         Enabled         =   0   'False
         Height          =   288
         Index           =   6
         Left            =   4920
         TabIndex        =   13
         Text            =   "<none>"
         Top             =   2520
         Width           =   972
      End
      Begin VB.TextBox txtAtt 
         Enabled         =   0   'False
         Height          =   288
         Index           =   5
         Left            =   1680
         TabIndex        =   8
         Text            =   "<none>"
         Top             =   3240
         Width           =   972
      End
      Begin VB.TextBox txtAtt 
         Enabled         =   0   'False
         Height          =   288
         Index           =   4
         Left            =   1680
         TabIndex        =   7
         Text            =   "<none>"
         Top             =   2880
         Width           =   972
      End
      Begin VB.TextBox txtAtt 
         Enabled         =   0   'False
         Height          =   288
         Index           =   3
         Left            =   1680
         TabIndex        =   6
         Text            =   "<none>"
         Top             =   2520
         Width           =   972
      End
      Begin VB.TextBox txtAtt 
         Enabled         =   0   'False
         Height          =   288
         Index           =   2
         Left            =   1680
         TabIndex        =   5
         Text            =   "<none>"
         Top             =   2160
         Width           =   972
      End
      Begin VB.TextBox txtAtt 
         Enabled         =   0   'False
         Height          =   288
         Index           =   1
         Left            =   1680
         TabIndex        =   4
         Text            =   "<none>"
         Top             =   1800
         Width           =   972
      End
      Begin VB.TextBox txtAtt 
         Enabled         =   0   'False
         Height          =   288
         Index           =   0
         Left            =   1680
         TabIndex        =   3
         Text            =   "<none>"
         Top             =   1440
         Width           =   972
      End
      Begin VB.TextBox txtName 
         Enabled         =   0   'False
         Height          =   288
         Left            =   1080
         TabIndex        =   2
         Text            =   "<none>"
         Top             =   720
         Width           =   4812
      End
      Begin VB.TextBox txtID 
         Enabled         =   0   'False
         Height          =   288
         Left            =   1080
         TabIndex        =   1
         Text            =   "<none>"
         Top             =   360
         Width           =   1452
      End
      Begin VB.TextBox txtCons 
         Enabled         =   0   'False
         Height          =   288
         Left            =   2400
         TabIndex        =   17
         Text            =   "FLOW"
         Top             =   4320
         Width           =   1452
      End
      Begin VB.TextBox txtLoc 
         Enabled         =   0   'False
         Height          =   288
         Left            =   4080
         TabIndex        =   18
         Text            =   "<none>"
         Top             =   4320
         Width           =   1452
      End
      Begin VB.TextBox txtScen 
         Enabled         =   0   'False
         Height          =   288
         Left            =   720
         TabIndex        =   16
         Text            =   "OBSERVED"
         Top             =   4320
         Width           =   1452
      End
      Begin VB.Label lblAtts 
         Caption         =   "Attributes:"
         Enabled         =   0   'False
         Height          =   252
         Left            =   2880
         TabIndex        =   45
         Top             =   1200
         Width           =   1332
      End
      Begin VB.Label lblCodes 
         Caption         =   "Codes:"
         Enabled         =   0   'False
         Height          =   252
         Left            =   120
         TabIndex        =   44
         Top             =   1200
         Width           =   612
      End
      Begin VB.Label lblAtt 
         Caption         =   "Site"
         Enabled         =   0   'False
         Height          =   252
         Index           =   12
         Left            =   360
         TabIndex        =   43
         Top             =   3600
         Width           =   972
      End
      Begin VB.Label lblAtt 
         Caption         =   "Timeseries Type"
         Enabled         =   0   'False
         Height          =   252
         Index           =   11
         Left            =   3120
         TabIndex        =   42
         Top             =   3240
         Width           =   1572
      End
      Begin VB.Label lblAtt 
         Caption         =   "Base Discharge"
         Enabled         =   0   'False
         Height          =   252
         Index           =   10
         Left            =   3120
         TabIndex        =   41
         Top             =   2160
         Width           =   1452
      End
      Begin VB.Label lblAtt 
         Caption         =   "Datum Elevation"
         Enabled         =   0   'False
         Height          =   252
         Index           =   9
         Left            =   3120
         TabIndex        =   40
         Top             =   1800
         Width           =   1452
      End
      Begin VB.Label lblAtt 
         Caption         =   "Total Drainage Area"
         Enabled         =   0   'False
         Height          =   252
         Index           =   8
         Left            =   3120
         TabIndex        =   39
         Top             =   1440
         Width           =   1812
      End
      Begin VB.Label lblAtt 
         Caption         =   "Longitude"
         Enabled         =   0   'False
         Height          =   252
         Index           =   7
         Left            =   3120
         TabIndex        =   38
         Top             =   2880
         Width           =   972
      End
      Begin VB.Label lblAtt 
         Caption         =   "Latitude"
         Enabled         =   0   'False
         Height          =   252
         Index           =   6
         Left            =   3120
         TabIndex        =   37
         Top             =   2520
         Width           =   732
      End
      Begin VB.Label lblAtt 
         Caption         =   "Hydrologic Unit"
         Enabled         =   0   'False
         Height          =   252
         Index           =   5
         Left            =   360
         TabIndex        =   36
         Top             =   3240
         Width           =   1332
      End
      Begin VB.Label lblAtt 
         Caption         =   "County"
         Enabled         =   0   'False
         Height          =   252
         Index           =   4
         Left            =   360
         TabIndex        =   35
         Top             =   2880
         Width           =   612
      End
      Begin VB.Label lblAtt 
         Caption         =   "District"
         Enabled         =   0   'False
         Height          =   252
         Index           =   3
         Left            =   360
         TabIndex        =   34
         Top             =   2520
         Width           =   612
      End
      Begin VB.Label lblAtt 
         Caption         =   "State FIPS"
         Enabled         =   0   'False
         Height          =   252
         Index           =   2
         Left            =   360
         TabIndex        =   33
         Top             =   2160
         Width           =   972
      End
      Begin VB.Label lblAtt 
         Caption         =   "Statistics"
         Enabled         =   0   'False
         Height          =   252
         Index           =   1
         Left            =   360
         TabIndex        =   32
         Top             =   1800
         Width           =   852
      End
      Begin VB.Label lblAtt 
         Caption         =   "Parameter"
         Enabled         =   0   'False
         Height          =   252
         Index           =   0
         Left            =   360
         TabIndex        =   31
         Top             =   1440
         Width           =   972
      End
      Begin VB.Label lblCons 
         Alignment       =   2  'Center
         Caption         =   "Constituent"
         Enabled         =   0   'False
         Height          =   252
         Left            =   2400
         TabIndex        =   30
         Top             =   4080
         Width           =   1452
      End
      Begin VB.Label lblLoc 
         Alignment       =   2  'Center
         Caption         =   "Location"
         Enabled         =   0   'False
         Height          =   252
         Left            =   4080
         TabIndex        =   29
         Top             =   4080
         Width           =   1452
      End
      Begin VB.Label lblScen 
         Alignment       =   2  'Center
         Caption         =   "Scenario"
         Enabled         =   0   'False
         Height          =   252
         Left            =   720
         TabIndex        =   28
         Top             =   4080
         Width           =   1452
      End
      Begin VB.Label lblStanam 
         Caption         =   "Name:"
         Enabled         =   0   'False
         Height          =   252
         Left            =   360
         TabIndex        =   27
         Top             =   720
         Width           =   612
      End
      Begin VB.Label lblStaid 
         Caption         =   "Station ID:"
         Enabled         =   0   'False
         Height          =   252
         Left            =   120
         TabIndex        =   26
         Top             =   360
         Width           =   972
      End
   End
   Begin VB.CommandButton cmdImport 
      Cancel          =   -1  'True
      Caption         =   "Cancel"
      Height          =   375
      Index           =   2
      Left            =   1440
      TabIndex        =   21
      Top             =   5640
      Width           =   972
   End
   Begin VB.CommandButton cmdImport 
      Caption         =   "Save"
      Enabled         =   0   'False
      Height          =   375
      Index           =   1
      Left            =   4080
      TabIndex        =   22
      Top             =   5640
      Width           =   972
   End
   Begin VB.CommandButton cmdImport 
      Caption         =   "Skip"
      Enabled         =   0   'False
      Height          =   375
      Index           =   0
      Left            =   5160
      TabIndex        =   23
      Top             =   5640
      Width           =   972
   End
   Begin VB.CommandButton cmdFile 
      Caption         =   "Select"
      Default         =   -1  'True
      Height          =   252
      Index           =   0
      Left            =   360
      TabIndex        =   0
      Top             =   360
      Width           =   852
   End
   Begin MSComDlg.CommonDialog CDFile 
      Index           =   0
      Left            =   0
      Top             =   360
      _ExtentX        =   699
      _ExtentY        =   699
      _Version        =   393216
      FontSize        =   2.54052e-29
   End
   Begin VB.Label lblFile 
      Caption         =   "<none>"
      Height          =   252
      Index           =   0
      Left            =   1320
      TabIndex        =   24
      Top             =   360
      Width           =   4212
   End
   Begin VB.Label lblFiles 
      Caption         =   "Watstore Daily Values File:"
      Height          =   252
      Left            =   120
      TabIndex        =   19
      Top             =   120
      Width           =   2412
   End
End
Attribute VB_Name = "frmGenScnImportWD"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants
Dim rval!(7), ival&(6)
Dim inwat&
Dim oldstr$
Private Sub cmdFile_Click(index As Integer)
  Dim i&
    CDFile(index).flags = &H1806&
    CDFile(index).filter = "Watstore files (*.wat)|*.wat"
    CDFile(index).Filename = "*.wat"
    CDFile(index).DialogTitle = "GenScn Import Watstore Data"
    On Error GoTo 10
    CDFile(index).CancelError = True
    CDFile(index).Action = 1
    lblFile(index).Caption = CDFile(index).Filename
    cmdImport(0).Enabled = False
    cmdImport(1).Enabled = False
    frmSpecs.Enabled = False
    lblStaid.Enabled = False
    txtID.Enabled = False
    lblStanam.Enabled = False
    txtName.Enabled = False
    lblCodes.Enabled = False
    lblAtts.Enabled = False
    For i = 0 To 12
      lblAtt(i).Enabled = False
      txtAtt(i).Enabled = False
    Next i
    lblScen.Enabled = False
    lblCons.Enabled = False
    lblLoc.Enabled = False
    txtScen.Enabled = False
    txtCons.Enabled = False
    txtLoc.Enabled = False
    cmdImport(3).Enabled = True
    inwat = FreeFile(0)
10  'continue here on cancel
End Sub


Private Sub cmdImport_Click(index As Integer)
    Dim i&, s$, iwdm&, id$, Name$, rcod&, csit$, ctyp$
    If index = 0 Then
      'skip this data set
      Call F90_WATHED(p.HSPFMsg.Unit, inwat, ival, rval, csit, ctyp, id, Name)
      txtID.text = id
      txtName.text = Name
      txtLoc.text = Left(Name, 8)
      txtAtt(12).text = Left(csit, 2)
      txtAtt(11).text = Left(ctyp, 4)
      txtAtt(0).text = ival(4)
      txtAtt(1).text = ival(5)
      txtAtt(2).text = ival(0)
      txtAtt(3).text = ival(1)
      txtAtt(4).text = ival(2)
      txtAtt(5).text = ival(3)
      txtAtt(6).text = rval(0)
      txtAtt(7).text = rval(1)
      txtAtt(8).text = rval(2)
      txtAtt(9).text = rval(4)
      txtAtt(10).text = rval(6)
      If Len(id) = 0 Then
        MsgBox "No more data sets are available in this Watstore file.", , "GenScn Import Watstore Data"
        Call F90_WATCLO(inwat)
        cmdImport(0).Enabled = False
        cmdImport(1).Enabled = False
        cmdImport(3).Enabled = False
        frmSpecs.Enabled = False
        lblStaid.Enabled = False
        txtID.Enabled = False
        lblCodes.Enabled = False
        lblAtts.Enabled = False
        For i = 0 To 12
          lblAtt(i).Enabled = False
          txtAtt(i).Enabled = False
        Next i
        lblStanam.Enabled = False
        txtName.Enabled = False
        lblScen.Enabled = False
        lblCons.Enabled = False
        lblLoc.Enabled = False
        txtScen.Enabled = False
        txtCons.Enabled = False
        txtLoc.Enabled = False
      End If
    ElseIf index = 1 Then
      'next data set, import data
      Call F90_WATINP(p.HSPFMsg.Unit, inwat, p.WDMFiles(1).fileUnit, rcod, ival(0), rval(0), txtScen.text, txtLoc.text, txtCons.text, _
                      txtID.text, txtName.text, txtAtt(12).text, txtAtt(11).text, _
                      Len(txtScen.text), Len(txtLoc.text), Len(txtCons.text), Len(txtID.text), Len(txtName.text), Len(txtAtt(12).text), Len(txtAtt(11).text))
      If rcod = 0 Then
        Debug.Print "frmGenScnImportWD:what is the dsn?" & ival(0)
        p.WDMFiles(1).Refresh 'Dsn (ival(0)) 'what is the dsn????
        'fill in next set of specs
        Call F90_WATHED(p.HSPFMsg.Unit, inwat, ival, rval, csit, ctyp, id, Name)
        txtID.text = id
        txtName.text = Name
        txtLoc.text = Left(Name, 8)
        txtAtt(12).text = Left(csit, 2)
        txtAtt(11).text = Left(ctyp, 4)
        txtAtt(0).text = ival(4)
        txtAtt(1).text = ival(5)
        txtAtt(2).text = ival(0)
        txtAtt(3).text = ival(1)
        txtAtt(4).text = ival(2)
        txtAtt(5).text = ival(3)
        txtAtt(6).text = rval(0)
        txtAtt(7).text = rval(1)
        txtAtt(8).text = rval(2)
        txtAtt(9).text = rval(4)
        txtAtt(10).text = rval(6)
        If Len(id) = 0 Then
          MsgBox "No more data sets are available in this Watstore file.", , "GenScn Import Watstore Data"
          Call F90_WATCLO(inwat)
          cmdImport(0).Enabled = False
          cmdImport(1).Enabled = False
          cmdImport(3).Enabled = False
          frmSpecs.Enabled = False
          lblStaid.Enabled = False
          txtID.Enabled = False
          lblStanam.Enabled = False
          txtName.Enabled = False
          lblCodes.Enabled = False
          lblAtts.Enabled = False
          For i = 0 To 12
            lblAtt(i).Enabled = False
            txtAtt(i).Enabled = False
          Next i
          lblScen.Enabled = False
          lblCons.Enabled = False
          lblLoc.Enabled = False
          txtScen.Enabled = False
          txtCons.Enabled = False
          txtLoc.Enabled = False
        End If
      Else
        'data set with these attributes already exists
        MsgBox "A timeseries with these attributes already exists.", vbExclamation, "GenScn Import Watstore Data"
      End If
    ElseIf index = 2 Then
      'cancel
      Unload frmGenScnImportWD
      frmGenScn.SetFocus
      Call F90_WATCLO(inwat)
      'Call WDMDir.TSDRRE(p.WDMFiles(1).fileUnit, 0&, 0&)
      'p.WDMFiles(1).Refresh
      'p.StatusFilePath = CurDir
      Call RefreshSLC
      'Call frmGenScn.RefreshMain
    ElseIf index = 3 Then
      'begin
      If lblFile(0).Caption = "<none>" Then
        'no file specified
        MsgBox "A Watstore file must be specified.", _
          vbExclamation, "GenScn Import Watstore Data"
      Else
        'watstore file specified
        'need to initialize
        Call F90_WATINI(lblFile(0).Caption, inwat, Len(lblFile(0).Caption))
        'fill in first set of specs
        Call F90_WATHED(p.HSPFMsg.Unit, inwat, ival, rval, csit, ctyp, id, Name)
        If Len(id) > 0 Then
          txtID.text = id
          txtName.text = Name
          txtLoc.text = Left(Name, 8)
          txtAtt(12).text = Left(csit, 2)
          txtAtt(11).text = Left(ctyp, 4)
          txtAtt(0).text = ival(4)
          txtAtt(1).text = ival(5)
          txtAtt(2).text = ival(0)
          txtAtt(3).text = ival(1)
          txtAtt(4).text = ival(2)
          txtAtt(5).text = ival(3)
          txtAtt(6).text = rval(0)
          txtAtt(7).text = rval(1)
          txtAtt(8).text = rval(2)
          txtAtt(9).text = rval(4)
          txtAtt(10).text = rval(6)
          cmdImport(0).Enabled = True
          cmdImport(1).Enabled = True
          frmSpecs.Enabled = True
          cmdImport(3).Enabled = False
          lblStaid.Enabled = True
          txtID.Enabled = True
          lblStanam.Enabled = True
          txtName.Enabled = True
          lblCodes.Enabled = True
          lblAtts.Enabled = True
          For i = 0 To 12
            lblAtt(i).Enabled = True
            txtAtt(i).Enabled = True
          Next i
          lblScen.Enabled = True
          lblCons.Enabled = True
          lblLoc.Enabled = True
          txtScen.Enabled = True
          txtCons.Enabled = True
          txtLoc.Enabled = True
        Else
          MsgBox "Unable to find any data sets in this Watstore file.", vbExclamation, "GenScn Import Watstore Data"
        End If
      End If
    End If
End Sub


Private Sub Form_Load()
  inwat = 0
End Sub

Private Sub txtAtt_GotFocus(index As Integer)
    oldstr = txtAtt(index).text
End Sub

Private Sub txtAtt_LostFocus(index As Integer)
    Dim newstr$, chgflg&, itemp&, rtemp!
    If index >= 0 And index < 5 Then
      'integer
      newstr = txtAtt(index).text
      Call ChkTxtI("Integer Value", -99999, 10000000, newstr, itemp, chgflg)
      If chgflg <> 1 Then
        txtAtt(index).text = oldstr
      Else
        If index = 0 Then
          ival(4) = itemp
        ElseIf index = 1 Then
          ival(5) = itemp
        ElseIf index = 2 Then
          ival(0) = itemp
        ElseIf index = 3 Then
          ival(1) = itemp
        ElseIf index = 4 Then
          ival(2) = itemp
        End If
      End If
    ElseIf index >= 6 And index <= 10 Then
      'real
      newstr = txtAtt(index).text
      Call ChkTxtR("Real Value", -99999#, 10000000, newstr, rtemp, chgflg)
      If chgflg <> 1 Then
        txtAtt(index).text = oldstr
      Else
        If index = 6 Then
          rval(0) = rtemp
        ElseIf index = 7 Then
          rval(1) = rtemp
        ElseIf index = 8 Then
          rval(2) = rtemp
        ElseIf index = 9 Then
          rval(4) = rtemp
        ElseIf index = 10 Then
          rval(6) = rtemp
        End If
      End If
    End If
End Sub


