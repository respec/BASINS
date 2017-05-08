VERSION 5.00
Begin VB.Form frmGenScnEstimator 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "GenScn Estimator"
   ClientHeight    =   6744
   ClientLeft      =   1416
   ClientTop       =   1296
   ClientWidth     =   7572
   BeginProperty Font 
      Name            =   "MS Sans Serif"
      Size            =   7.8
      Charset         =   0
      Weight          =   700
      Underline       =   0   'False
      Italic          =   0   'False
      Strikethrough   =   0   'False
   EndProperty
   HelpContextID   =   113
   Icon            =   "genestim.frx":0000
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   PaletteMode     =   1  'UseZOrder
   ScaleHeight     =   6744
   ScaleWidth      =   7572
   Begin VB.Frame frmDate 
      Caption         =   "Calibration Period"
      Height          =   1332
      Left            =   120
      TabIndex        =   44
      Top             =   2640
      Width           =   3735
      Begin VB.TextBox txtCed 
         Height          =   288
         Left            =   2640
         TabIndex        =   14
         Top             =   840
         Width           =   732
      End
      Begin VB.TextBox txtCsd 
         Height          =   288
         Left            =   2640
         TabIndex        =   11
         Top             =   480
         Width           =   732
      End
      Begin VB.TextBox txtCem 
         Height          =   288
         Left            =   1800
         TabIndex        =   13
         Top             =   840
         Width           =   732
      End
      Begin VB.TextBox txtCsm 
         Height          =   288
         Left            =   1800
         TabIndex        =   10
         Top             =   480
         Width           =   732
      End
      Begin VB.TextBox txtCey 
         Height          =   288
         Left            =   960
         TabIndex        =   12
         Top             =   840
         Width           =   732
      End
      Begin VB.TextBox txtCsy 
         Height          =   288
         Left            =   960
         TabIndex        =   9
         Top             =   480
         Width           =   732
      End
      Begin VB.Label Label6 
         Alignment       =   1  'Right Justify
         Caption         =   "End:"
         Height          =   255
         Left            =   360
         TabIndex        =   49
         Top             =   840
         Width           =   495
      End
      Begin VB.Label Label5 
         Alignment       =   1  'Right Justify
         Caption         =   "Start:"
         Height          =   255
         Left            =   360
         TabIndex        =   48
         Top             =   480
         Width           =   495
      End
      Begin VB.Label Label4 
         Alignment       =   2  'Center
         Caption         =   "Day"
         Height          =   255
         Left            =   2640
         TabIndex        =   47
         Top             =   240
         Width           =   735
      End
      Begin VB.Label Label3 
         Alignment       =   2  'Center
         Caption         =   "Month"
         Height          =   255
         Left            =   1800
         TabIndex        =   46
         Top             =   240
         Width           =   735
      End
      Begin VB.Label Label2 
         Alignment       =   2  'Center
         Caption         =   "Year"
         Height          =   255
         Left            =   960
         TabIndex        =   45
         Top             =   240
         Width           =   735
      End
   End
   Begin VB.Frame frmRegress 
      Caption         =   "Regressors"
      Height          =   2412
      Left            =   5400
      TabIndex        =   43
      Top             =   1560
      Width           =   2052
      Begin VB.ListBox lstRegress 
         Height          =   1200
         Left            =   120
         MultiSelect     =   1  'Simple
         TabIndex        =   8
         Top             =   360
         Width           =   1812
      End
   End
   Begin VB.Frame frmParameters 
      Caption         =   "Parameter"
      Height          =   2412
      Left            =   3960
      TabIndex        =   42
      Top             =   1560
      Width           =   1332
      Begin VB.ListBox lstParams 
         Height          =   1200
         Left            =   120
         TabIndex        =   7
         Top             =   360
         Width           =   1092
      End
   End
   Begin VB.Frame frmOutput 
      Caption         =   "Output Files"
      Height          =   1332
      Left            =   120
      TabIndex        =   35
      Top             =   4080
      Width           =   7335
      Begin VB.CommandButton cmdFile 
         Caption         =   "Select"
         Height          =   252
         Index           =   5
         Left            =   120
         TabIndex        =   17
         Top             =   960
         Width           =   852
      End
      Begin VB.CommandButton cmdFile 
         Caption         =   "Select"
         Height          =   252
         Index           =   4
         Left            =   120
         TabIndex        =   16
         Top             =   600
         Width           =   852
      End
      Begin VB.CommandButton cmdFile 
         Caption         =   "Select"
         Height          =   252
         Index           =   3
         Left            =   120
         TabIndex        =   15
         Top             =   240
         Width           =   852
      End
      Begin MSComDlg.CommonDialog CDFile 
         Index           =   5
         Left            =   0
         Top             =   960
         _ExtentX        =   699
         _ExtentY        =   699
         _Version        =   393216
         FontSize        =   4.09255e-38
      End
      Begin MSComDlg.CommonDialog CDFile 
         Index           =   4
         Left            =   0
         Top             =   600
         _ExtentX        =   699
         _ExtentY        =   699
         _Version        =   393216
         FontSize        =   4.09255e-38
      End
      Begin MSComDlg.CommonDialog CDFile 
         Index           =   3
         Left            =   0
         Top             =   240
         _ExtentX        =   699
         _ExtentY        =   699
         _Version        =   393216
         FontSize        =   4.09255e-38
      End
      Begin VB.Label lblFile 
         Caption         =   "<none>"
         Height          =   252
         Index           =   5
         Left            =   3120
         TabIndex        =   41
         Top             =   960
         Width           =   3972
      End
      Begin VB.Label lblFile 
         Caption         =   "<none>"
         Height          =   252
         Index           =   4
         Left            =   3120
         TabIndex        =   40
         Top             =   600
         Width           =   3972
      End
      Begin VB.Label lblFile 
         Caption         =   "<none>"
         Height          =   252
         Index           =   3
         Left            =   3120
         TabIndex        =   39
         Top             =   240
         Width           =   3972
      End
      Begin VB.Label lblDaily 
         Caption         =   "Daily Loads"
         Height          =   252
         Left            =   1080
         TabIndex        =   38
         Top             =   960
         Width           =   1932
      End
      Begin VB.Label lblOutComp 
         Caption         =   "Comparison"
         Height          =   252
         Left            =   1080
         TabIndex        =   37
         Top             =   600
         Width           =   1932
      End
      Begin VB.Label lblOutfile 
         Caption         =   "Main"
         Height          =   252
         Left            =   1080
         TabIndex        =   36
         Top             =   240
         Width           =   1932
      End
   End
   Begin VB.Frame frmInput 
      Caption         =   "Input Files"
      Height          =   1332
      Left            =   120
      TabIndex        =   28
      Top             =   120
      Width           =   7335
      Begin VB.CommandButton cmdFile 
         Caption         =   "Select"
         Height          =   252
         Index           =   2
         Left            =   120
         TabIndex        =   2
         Top             =   960
         Width           =   852
      End
      Begin VB.CommandButton cmdFile 
         Caption         =   "Select"
         Height          =   252
         Index           =   1
         Left            =   120
         TabIndex        =   1
         Top             =   600
         Width           =   852
      End
      Begin VB.CommandButton cmdFile 
         Caption         =   "Select"
         Height          =   252
         Index           =   0
         Left            =   120
         TabIndex        =   0
         Top             =   240
         Width           =   852
      End
      Begin MSComDlg.CommonDialog CDFile 
         Index           =   2
         Left            =   0
         Top             =   960
         _ExtentX        =   699
         _ExtentY        =   699
         _Version        =   393216
         FontSize        =   4.09255e-38
      End
      Begin MSComDlg.CommonDialog CDFile 
         Index           =   1
         Left            =   0
         Top             =   600
         _ExtentX        =   699
         _ExtentY        =   699
         _Version        =   393216
         FontSize        =   4.09255e-38
      End
      Begin MSComDlg.CommonDialog CDFile 
         Index           =   0
         Left            =   0
         Top             =   240
         _ExtentX        =   699
         _ExtentY        =   699
         _Version        =   393216
         FontSize        =   4.09255e-38
      End
      Begin VB.Label lblFile 
         Caption         =   "<none>"
         Height          =   252
         Index           =   0
         Left            =   3120
         TabIndex        =   34
         Top             =   240
         Width           =   3972
      End
      Begin VB.Label lblFile 
         Caption         =   "<none>"
         Height          =   252
         Index           =   1
         Left            =   3120
         TabIndex        =   33
         Top             =   600
         Width           =   3972
      End
      Begin VB.Label lblFile 
         Caption         =   "<none>"
         Height          =   252
         Index           =   2
         Left            =   3120
         TabIndex        =   32
         Top             =   960
         Width           =   3972
      End
      Begin VB.Label lblParameters 
         Caption         =   "Parameters"
         Height          =   252
         Left            =   1080
         TabIndex        =   31
         Top             =   960
         Width           =   1932
      End
      Begin VB.Label lblWater 
         Caption         =   "Water Quality"
         Height          =   252
         Left            =   1080
         TabIndex        =   30
         Top             =   600
         Width           =   1932
      End
      Begin VB.Label lblFlow 
         Caption         =   "Flow (Q) "
         Height          =   252
         Left            =   1080
         TabIndex        =   29
         Top             =   240
         Width           =   1932
      End
   End
   Begin VB.Frame frmVariances 
      Caption         =   "Variances"
      Height          =   972
      Left            =   120
      TabIndex        =   27
      Top             =   1560
      Width           =   1695
      Begin VB.OptionButton optVar 
         Caption         =   "Approximate"
         Height          =   252
         Index           =   1
         Left            =   120
         TabIndex        =   3
         Top             =   240
         Value           =   -1  'True
         Width           =   1455
      End
      Begin VB.OptionButton optVar 
         Caption         =   "Exact"
         Height          =   252
         Index           =   0
         Left            =   120
         TabIndex        =   4
         Top             =   480
         Width           =   1332
      End
   End
   Begin VB.Frame frmYears 
      Caption         =   "Years for Estimation"
      Height          =   972
      Left            =   1920
      TabIndex        =   24
      Top             =   1560
      Width           =   1932
      Begin VB.TextBox txtEyear 
         Height          =   288
         Left            =   720
         TabIndex        =   6
         Top             =   600
         Width           =   852
      End
      Begin VB.TextBox txtSyear 
         Height          =   288
         Left            =   720
         TabIndex        =   5
         Top             =   240
         WhatsThisHelpID =   25
         Width           =   852
      End
      Begin VB.Label Label1 
         Alignment       =   1  'Right Justify
         Caption         =   "Last:"
         Height          =   252
         Left            =   120
         TabIndex        =   26
         Top             =   600
         Width           =   492
      End
      Begin VB.Label lblSyear 
         Alignment       =   1  'Right Justify
         Caption         =   "First:"
         Height          =   252
         Left            =   120
         TabIndex        =   25
         Top             =   240
         WhatsThisHelpID =   25
         Width           =   492
      End
   End
   Begin VB.CommandButton cmdNew 
      Caption         =   "Results"
      Height          =   372
      Index           =   0
      Left            =   4800
      TabIndex        =   20
      Top             =   5640
      Width           =   1332
   End
   Begin VB.CommandButton cmdClear 
      Caption         =   "Clear Specs"
      Height          =   372
      Left            =   4800
      TabIndex        =   23
      Top             =   6240
      Width           =   1332
   End
   Begin VB.CommandButton cmdGet 
      Caption         =   "Get Specs"
      Height          =   372
      Left            =   3120
      TabIndex        =   22
      Top             =   6240
      Width           =   1332
   End
   Begin VB.CommandButton cmdSave 
      Caption         =   "Save Specs"
      Height          =   372
      Left            =   1440
      TabIndex        =   21
      Top             =   6240
      Width           =   1332
   End
   Begin VB.CommandButton cmdNew 
      Cancel          =   -1  'True
      Caption         =   "Cancel"
      Height          =   372
      Index           =   2
      Left            =   3120
      TabIndex        =   19
      Top             =   5640
      Width           =   1332
   End
   Begin VB.CommandButton cmdNew 
      Caption         =   "&OK"
      Height          =   372
      Index           =   1
      Left            =   1440
      TabIndex        =   18
      Top             =   5640
      Width           =   1332
   End
   Begin MSComDlg.CommonDialog CDGet 
      Left            =   3000
      Top             =   6240
      _ExtentX        =   699
      _ExtentY        =   699
      _Version        =   393216
      FontSize        =   4.09255e-38
   End
   Begin MSComDlg.CommonDialog CDSave 
      Left            =   1320
      Top             =   6240
      _ExtentX        =   699
      _ExtentY        =   699
      _Version        =   393216
      FontSize        =   4.09255e-38
   End
End
Attribute VB_Name = "frmGenScnEstimator"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants
Dim oldsyear$, oldeyear$
Dim isyear&, ieyear&
Dim oldcsy$, oldcsm$, oldcsd$, oldcey$, oldcem$, oldced$
Dim csy&, csm&, csd&, cey&, cem&, ced&

Private Sub EstimInit()
  Dim i&
  isyear = 1900
  txtSyear.Text = isyear
  ieyear = 9999
  txtEyear.Text = ieyear
  For i = 0 To 5
    lblFile(i).Caption = "<none>"
  Next i
  optVar(0).value = False
  optVar(1).value = True
  txtCsy.Text = 1900
  txtCsm.Text = 1
  txtCsd.Text = 1
  txtCey.Text = 9999
  txtCem.Text = 1
  txtCed.Text = 1
  lstRegress.ListIndex = -1
  For i = 1 To lstRegress.ListCount
    lstRegress.Selected(i - 1) = False
  Next i
  lstParams.ListIndex = -1
  Call FillParam
End Sub

Sub FillParam()
  Dim i&
  Dim nvars&, ivars&(1000)
  lstParams.clear
  If lblFile(2).Caption <> "<none>" Then
    'try opening this file first
    On Error GoTo 10
    Open lblFile(2).Caption For Input As #2
    Close #2
    Call F90_GPCODE(nvars, ivars(0), lblFile(2).Caption, Len(lblFile(2).Caption))
    If nvars > 0 Then
      For i = 1 To nvars
        If Len(CStr(ivars(i - 1))) = 1 Then
          lstParams.AddItem "P0000" & ivars(i - 1)
        ElseIf Len(CStr(ivars(i - 1))) = 2 Then
          lstParams.AddItem "P000" & ivars(i - 1)
        ElseIf Len(CStr(ivars(i - 1))) = 3 Then
          lstParams.AddItem "P00" & ivars(i - 1)
        ElseIf Len(CStr(ivars(i - 1))) = 4 Then
          lstParams.AddItem "P0" & ivars(i - 1)
        Else
          lstParams.AddItem "P" & ivars(i - 1)
        End If
      Next i
    End If
    GoTo 20
10  'continue
    MsgBox "The specified parameter file does not exist.", _
         vbExclamation, "GenScn Estimator"
20  'continue
  End If
End Sub

Private Sub GetSpecs(Filename)
  Dim ichr$, inum As Variant, i&, inumx As Variant
  On Error GoTo 10
  Open Filename For Input As #1
  Call EstimInit
  Line Input #1, ichr
  txtSyear = ichr
  Line Input #1, ichr
  txtEyear = ichr
  For i = 0 To 5
    Line Input #1, ichr
    lblFile(i).Caption = ichr
  Next i
  Line Input #1, inum
  If inum = 1 Then
    optVar(0) = True
    optVar(1) = False
  Else
    optVar(0) = False
    optVar(1) = True
  End If
  Line Input #1, ichr
  txtCsy.Text = ichr
  Line Input #1, ichr
  txtCsm.Text = ichr
  Line Input #1, ichr
  txtCsd.Text = ichr
  Line Input #1, ichr
  txtCey.Text = ichr
  Line Input #1, ichr
  txtCem.Text = ichr
  Line Input #1, ichr
  txtCed.Text = ichr
  lstRegress.ListIndex = -1
  For i = 1 To lstRegress.ListCount
    lstRegress.Selected(i - 1) = False
  Next i
  Line Input #1, inum
  If inum > 0 Then
    For i = 1 To inum
      Line Input #1, inumx
      lstRegress.Selected(inumx) = True
    Next i
  End If
  Line Input #1, inum
  Close #1
  Call FillParam
  lstParams.ListIndex = inum
  GoTo 20
10  Close #1
    Call EstimInit
20  'go here if everything read okay
End Sub

Private Sub PutSpecs(Filename)
  Dim i&, inum&, ichr$
  Open Filename For Output As #1
  Print #1, txtSyear
  Print #1, txtEyear
  For i = 0 To 5
    Print #1, lblFile(i).Caption
  Next i
  If optVar(0) = True Then
    inum = 1
  Else
    inum = 0
  End If
  Print #1, inum
  ichr = txtCsy.Text
  Print #1, ichr
  ichr = txtCsm.Text
  Print #1, ichr
  ichr = txtCsd.Text
  Print #1, ichr
  ichr = txtCey.Text
  Print #1, ichr
  ichr = txtCem.Text
  Print #1, ichr
  ichr = txtCed.Text
  Print #1, ichr
  inum = lstRegress.SelCount
  Print #1, inum
  If inum > 0 Then
    For i = 0 To lstRegress.ListCount - 1
      If lstRegress.Selected(i) = True Then
        Print #1, i
      End If
    Next i
  End If
  inum = lstParams.ListIndex
  Print #1, inum
  Close #1
End Sub


Private Sub cmdClear_Click()
    Call EstimInit
End Sub


Private Sub cmdFile_Click(index As Integer)
    CDFile(index).flags = &H8806&
    Select Case index
    Case 0
      CDFile(index).filter = "ASCII files (*.q)|*.q|All files (*.*)|*.*"
      CDFile(index).Filename = "test.q"
      CDFile(index).DialogTitle = "GenScn Estimator Flow File"
    Case 1
      CDFile(index).filter = "ASCII files (*.qw)|*.qw|All files (*.*)|*.*"
      CDFile(index).Filename = "test.qw"
      CDFile(index).DialogTitle = "GenScn Estimator Water Quality File"
    Case 2
      CDFile(index).filter = "ASCII files (*.cmnd)|*.cmnd|All files (*.*)|*.*"
      CDFile(index).Filename = "test.qw.cmnd"
      CDFile(index).DialogTitle = "GenScn Estimator Water Quality Parameter File"
    Case 3
      CDFile(index).filter = "ASCII files (*.out)|*.out|All files (*.*)|*.*"
      CDFile(index).Filename = "main.out"
      CDFile(index).DialogTitle = "GenScn Estimator Main Output File"
    Case 4
      CDFile(index).filter = "ASCII files (*.out)|*.out|All files (*.*)|*.*"
      CDFile(index).Filename = "comp.out"
      CDFile(index).DialogTitle = "GenScn Estimator Comparison Output File"
    Case 5
      CDFile(index).filter = "ASCII files (*.out)|*.out|All files (*.*)|*.*"
      CDFile(index).Filename = "daily.out"
      CDFile(index).DialogTitle = "GenScn Estimator Daily Loads Output File"
    End Select
    On Error GoTo 10
    CDFile(index).CancelError = True
    If index = 3 Or index = 4 Or index = 5 Then
      CDFile(index).Action = 2
    Else
      CDFile(index).Action = 1
    End If
    lblFile(index).Caption = CDFile(index).Filename
    If index = 2 Then
      Call FillParam
    End If
10      'continue here on cancel
End Sub

Private Sub cmdGet_Click()
    CDGet.flags = &H1806&
    CDGet.filter = "GenScn Estimator Files (*.ges)|*.ges"
    CDGet.Filename = "*.ges"
    CDGet.DialogTitle = "GenScn Estimator Get Specs File"
    On Error GoTo 10
    CDGet.CancelError = True
    CDGet.Action = 1
    Call GetSpecs(CDGet.Filename)
10 'continue here on cancel
End Sub

Private Sub cmdNew_Click(index As Integer)
  Dim ireg&(200), rbegin!, rend!
  Dim qnam$, qwnam$, cmnnam$, outnam$, compnam$, daynam$, icnt&, i&, defname$
  If index = 0 Then
    'do results menu
    frmGenEstRes.Show
  ElseIf index = 1 Then
    'run estimator
    Dim retcod&, mflag&, mescod&, eaflag&, ifyr&, ilyr&, nreg&, ipcode&
    If lblFile(0).Caption = "<none>" Or _
       lblFile(1).Caption = "<none>" Or _
       lblFile(2).Caption = "<none>" Or _
       lblFile(3).Caption = "<none>" Then
       'this file is not optional
       MsgBox "One or more required files have not been selected.", _
         vbExclamation, "GenScn Estimator"
    Else
      'try opening these files first
      On Error GoTo 10
      Open lblFile(0).Caption For Input As #2
      Close #2
      Open lblFile(1).Caption For Input As #2
      Close #2
      Open lblFile(2).Caption For Input As #2
      Close #2
      If lstParams.ListIndex < 0 Then
        MsgBox "One parameter must be selected from the list.", _
           vbExclamation, "GenScn Estimator"
      Else
        If lstRegress.SelCount < 1 Then
          MsgBox "At least one regressor must be selected from the list.", _
            vbExclamation, "GenScn Estimator"
        Else
          'okay to run
          qnam = lblFile(0).Caption
          qwnam = lblFile(1).Caption
          cmnnam = lblFile(2).Caption
          outnam = lblFile(3).Caption
          compnam = lblFile(4).Caption
          daynam = lblFile(5).Caption
          If compnam = "<none>" Then
            compnam = ""
          End If
          If daynam = "<none>" Then
            daynam = ""
          End If
          If optVar(0) = True Then
            eaflag = 0
          Else
            eaflag = 1
          End If
          ifyr = CInt(txtSyear.Text)
          ilyr = CInt(txtEyear.Text)
          mflag = 0
          'ipcode = CInt(Mid(lstParams.List(ListIndex + 1), 2, 5))
          ipcode = CInt(Mid(lstParams.List(lstParams.ListIndex + 1), 2, 5))
          'convert dates to real numbers
          Call F90_REALDATE(CInt(txtCsy.Text), CInt(txtCsm.Text), CInt(txtCsd.Text), rbegin!)
          Call F90_REALDATE(CInt(txtCey.Text), CInt(txtCem.Text), CInt(txtCed.Text), rend!)
          'set regressors
          nreg = lstRegress.SelCount
          icnt = 0
          For i = 1 To lstRegress.ListCount
            If lstRegress.Selected(i - 1) = True Then
              ireg(icnt) = i
              icnt = icnt + 1
            End If
          Next i
          MousePointer = vbHourglass
          Call F90_DPLOTI
          Call F90_ESTIM(mflag, eaflag, ifyr, ilyr, ipcode, _
                      rbegin, rend, nreg, ireg(0), retcod, mescod, _
                      outnam, compnam, daynam, qnam, qwnam, cmnnam, _
                      Len(outnam), Len(compnam), Len(daynam), Len(qnam), _
                      Len(qwnam), Len(cmnnam))
          MousePointer = vbDefault
          If mescod = 1 Then
            MsgBox "Insufficient data for model calibration.", _
              vbExclamation, "GenScn Estimator"
          ElseIf mescod = 2 Then
            MsgBox "Invalid year -- no flow data available.", _
              vbExclamation, "GenScn Estimator"
          ElseIf mescod = 3 Then
            MsgBox "DV Flow unavailable for point.", _
              vbExclamation, "GenScn Estimator"
          ElseIf mescod = 4 Then
            MsgBox "Flow missing or <= 0 in q file, 1.d-12 substituted for flow.", _
              vbExclamation, "GenScn Estimator"
          ElseIf mescod = 5 Then
            MsgBox "Instantaneous flow not available.", _
              vbExclamation, "GenScn Estimator"
          ElseIf mescod = 6 Then
            MsgBox "Zero or negative value found.", _
              vbExclamation, "GenScn Estimator"
          ElseIf mescod = 7 Then
            MsgBox "Unrecognized remark code found, recoded as missing value.", _
              vbExclamation, "GenScn Estimator"
          End If
          If retcod = 1 Then
            MsgBox "Error:  Estimation and calibration data sets differ.", _
              vbExclamation, "GenScn Estimator"
          ElseIf retcod = 2 Then
            MsgBox "Error:  Parameter code value not found in list.", _
              vbExclamation, "GenScn Estimator"
          ElseIf retcod = 3 Then
            MsgBox "Error:  Missing or negative flow value.", _
              vbExclamation, "GenScn Estimator"
          ElseIf retcod = 4 Then
            MsgBox "Error:  Year must be between 1901 and 2050.", _
              vbExclamation, "GenScn Estimator"
          ElseIf retcod = 0 Then
            'successful run of estimator
            MsgBox "The Estimator program has run successfully.", 0, _
              "GenScn Estimator"
          End If
        End If
      End If
      GoTo 20
10    'continue
      MsgBox "One or more of the specified input file does not exist.", _
         vbExclamation, "GenScn Estimator"
20    'continue
    End If
  ElseIf index = 2 Then
    'close
    defname = p.StatusFilePath & "\default.ges"
    Call PutSpecs(defname)
    Unload frmGenScnEstimator
  End If
End Sub

Private Sub cmdSave_Click()
    CDSave.flags = &H8806&
    CDSave.filter = "GenScn Estimator Files (*.ges)|*.ges"
    CDSave.Filename = "*.ges"
    CDSave.DialogTitle = "GenScn Estimator Save Specs File"
    On Error GoTo 10
    CDSave.CancelError = True
    CDSave.Action = 2
    Call PutSpecs(CDSave.Filename)
10 'continue here on cancel
End Sub

Private Sub Form_Load()
  Dim i&, defname$
    Call F90_DPLOTI
    lstRegress.AddItem "CONSTANT"
    lstRegress.AddItem "LOG-FLOW"
    lstRegress.AddItem "LOG-FLOW SQUARED"
    lstRegress.AddItem "SQRT FLOW"
    lstRegress.AddItem "DEC_TIME"
    lstRegress.AddItem "DEC_TIME SQUARED"
    lstRegress.AddItem "SIN(2*PI*T)"
    lstRegress.AddItem "COS(2*PI*T)"
    lstRegress.AddItem "SIN(4*PI*T)"
    lstRegress.AddItem "COS(4*PI*T)"
    lstRegress.AddItem "SIN(6*PI*T)"
    lstRegress.AddItem "COS(6*PI*T)"
    lstRegress.AddItem "I-JAN"
    lstRegress.AddItem "I-FEB"
    lstRegress.AddItem "I-MAR"
    lstRegress.AddItem "I-APR"
    lstRegress.AddItem "I-MAY"
    lstRegress.AddItem "I-JUN"
    lstRegress.AddItem "I-JUL"
    lstRegress.AddItem "I-AUG"
    lstRegress.AddItem "I-SEP"
    lstRegress.AddItem "I-OCT"
    lstRegress.AddItem "I-NOV"
    lstRegress.AddItem "I-DEC"
    For i = 1 To 150
      lstRegress.AddItem "I-" & 1900 + i
    Next i
    defname = p.StatusFilePath & "\default.ges"
    Call GetSpecs(defname)
End Sub

Private Sub txtCed_GotFocus()
   oldced = txtCed.Text
End Sub

Private Sub txtCed_LostFocus()
   Dim newced$, chgflg&
    If Len(txtCed.Text) > 0 Then
      newced = txtCed.Text
      Call ChkTxtI("Day", 1, 31, newced, ced, chgflg)
      If chgflg <> 1 Then
        txtCed.Text = oldced
      End If
    Else
      txtCed.Text = oldced
    End If
End Sub

Private Sub txtCem_GotFocus()
   oldcem = txtCem.Text
End Sub


Private Sub txtCem_LostFocus()
   Dim newcem$, chgflg&
    If Len(txtCem.Text) > 0 Then
      newcem = txtCem.Text
      Call ChkTxtI("Month", 1, 12, newcem, cem, chgflg)
      If chgflg <> 1 Then
        txtCem.Text = oldcem
      End If
    Else
      txtCem.Text = oldcem
    End If
End Sub


Private Sub txtCey_GotFocus()
   oldcey = txtCey.Text
End Sub


Private Sub txtCey_LostFocus()
   Dim newcey$, chgflg&
    If Len(txtCey.Text) > 0 Then
      newcey = txtCey.Text
      Call ChkTxtI("Year", 1900, 9999, newcey, cey, chgflg)
      If chgflg <> 1 Then
        txtCey.Text = oldcey
      End If
    Else
      txtCey.Text = oldcey
    End If
End Sub


Private Sub txtCsd_GotFocus()
   oldcsd = txtCsd.Text
End Sub


Private Sub txtCsd_LostFocus()
   Dim newcsd$, chgflg&
    If Len(txtCsd.Text) > 0 Then
      newcsd = txtCsd.Text
      Call ChkTxtI("Day", 1, 31, newcsd, csd, chgflg)
      If chgflg <> 1 Then
        txtCsd.Text = oldcsd
      End If
    Else
      txtCsd.Text = oldcsd
    End If
End Sub


Private Sub txtCsm_GotFocus()
   oldcsm = txtCsm.Text
End Sub


Private Sub txtCsm_LostFocus()
   Dim newcsm$, chgflg&
    If Len(txtCsm.Text) > 0 Then
      newcsm = txtCsm.Text
      Call ChkTxtI("Month", 1, 12, newcsm, csm, chgflg)
      If chgflg <> 1 Then
        txtCsm.Text = oldcsm
      End If
    Else
      txtCsm.Text = oldcsm
    End If
End Sub


Private Sub txtcsy_GotFocus()
   oldcsy = txtCsy.Text
End Sub


Private Sub txtcsy_LostFocus()
   Dim newcsy$, chgflg&
    If Len(txtCsy.Text) > 0 Then
      newcsy = txtCsy.Text
      Call ChkTxtI("Year", 1900, 9999, newcsy, csy, chgflg)
      If chgflg <> 1 Then
        txtCsy.Text = oldcsy
      End If
    Else
      txtCsy.Text = oldcsy
    End If
End Sub


Private Sub txtEyear_GotFocus()
   oldeyear = txtEyear.Text
End Sub

Private Sub txtEyear_LostFocus()
   Dim neweyear$, chgflg&
    If Len(txtEyear.Text) > 0 Then
      neweyear = txtEyear.Text
      Call ChkTxtI("Year", 1900, 9999, neweyear, ieyear, chgflg)
      If chgflg <> 1 Then
        txtEyear.Text = oldeyear
      End If
    Else
      txtEyear.Text = oldeyear
    End If
End Sub


Private Sub txtSyear_GotFocus()
   oldsyear = txtSyear.Text
End Sub

Private Sub txtSyear_LostFocus()
   Dim newsyear$, chgflg&
    If Len(txtSyear.Text) > 0 Then
      newsyear = txtSyear.Text
      Call ChkTxtI("Year", 1900, 9999, newsyear, isyear, chgflg)
      If chgflg <> 1 Then
        txtSyear.Text = oldsyear
      End If
    Else
      txtSyear.Text = oldsyear
    End If
End Sub


