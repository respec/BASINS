VERSION 5.00
Begin VB.Form frmBasinsPest 
   Caption         =   "BASINS PEST"
   ClientHeight    =   2445
   ClientLeft      =   60
   ClientTop       =   345
   ClientWidth     =   4665
   BeginProperty Font 
      Name            =   "MS Sans Serif"
      Size            =   8.25
      Charset         =   0
      Weight          =   700
      Underline       =   0   'False
      Italic          =   0   'False
      Strikethrough   =   0   'False
   EndProperty
   Icon            =   "BasinsPest.frx":0000
   LinkTopic       =   "Form1"
   ScaleHeight     =   2445
   ScaleWidth      =   4665
   StartUpPosition =   3  'Windows Default
   Begin VB.CommandButton cmdClose 
      Caption         =   "Close"
      Height          =   375
      Left            =   3360
      TabIndex        =   13
      Top             =   2040
      Width           =   1215
   End
   Begin VB.CommandButton cmdUpdateUCI 
      Caption         =   "Update UCI"
      Height          =   375
      Left            =   1560
      TabIndex        =   12
      Top             =   2040
      Width           =   1215
   End
   Begin VB.CheckBox chkAUI 
      Caption         =   "Automatic User Intervention"
      Height          =   255
      Left            =   960
      TabIndex        =   11
      Top             =   1680
      Value           =   1  'Checked
      Width           =   3015
   End
   Begin VB.ComboBox cboSimDSNs 
      Height          =   315
      Left            =   2760
      Style           =   2  'Dropdown List
      TabIndex        =   10
      Top             =   1320
      Width           =   855
   End
   Begin VB.TextBox txtObsDSN 
      Height          =   285
      Left            =   2760
      TabIndex        =   8
      Top             =   960
      Width           =   615
   End
   Begin VB.CommandButton cmdRunPest 
      Caption         =   "Run PEST"
      Height          =   375
      Left            =   120
      TabIndex        =   6
      Top             =   2040
      Width           =   1215
   End
   Begin VB.CommandButton cmdPar2ParWrite 
      Caption         =   "Write PAR2PAR"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   255
      Left            =   5520
      TabIndex        =   5
      Top             =   4440
      Visible         =   0   'False
      Width           =   1575
   End
   Begin VB.CommandButton cmdPar2ParOpen 
      Caption         =   "Open PAR2PAR"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   255
      Left            =   5520
      TabIndex        =   4
      Top             =   120
      Visible         =   0   'False
      Width           =   1575
   End
   Begin VB.CommandButton cmdGroupWrite 
      Caption         =   "Write Group File"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   255
      Left            =   1920
      TabIndex        =   3
      Top             =   4440
      Visible         =   0   'False
      Width           =   1575
   End
   Begin VB.CommandButton cmdTSPWrite 
      Caption         =   "Write TSPROC File"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   255
      Left            =   3720
      TabIndex        =   2
      Top             =   4440
      Visible         =   0   'False
      Width           =   1575
   End
   Begin VB.CommandButton cmdParmWrite 
      Caption         =   "Write Parm File"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   255
      Left            =   120
      TabIndex        =   1
      Top             =   4440
      Visible         =   0   'False
      Width           =   1575
   End
   Begin VB.Label Label4 
      Caption         =   "Simulated DSN:"
      Height          =   255
      Left            =   1200
      TabIndex        =   9
      Top             =   1320
      Width           =   1575
   End
   Begin VB.Label Label3 
      Caption         =   "Observed DSN:"
      Height          =   255
      Left            =   1200
      TabIndex        =   7
      Top             =   960
      Width           =   1455
   End
   Begin VB.Label lblUCIName 
      Height          =   855
      Left            =   120
      TabIndex        =   0
      Top             =   120
      Width           =   4455
   End
End
Attribute VB_Name = "frmBasinsPest"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'Copyright 2003 AQUA TERRA Consultants - Royalty-free use permitted under open source license
Option Explicit
Private pPest As clsPest

Public Property Let Pest(newvalue As clsPest)
  Set pPest = newvalue
End Property

Private Sub cmdClose_Click()
  Unload Me
End Sub

Private Sub cmdRunPest_Click()
  Dim ErrStr As String
  If IsNumeric(txtObsDSN.Text) And IsNumeric(cboSimDSNs.List(cboSimDSNs.ListIndex)) Then
    If chkAUI.Value = 1 Then pPest.AUI = True
    ErrStr = pPest.InitBasinsPest(CLng(txtObsDSN.Text), CLng(cboSimDSNs.List(cboSimDSNs.ListIndex)))
    If Len(ErrStr) = 0 Then
      pPest.Run
    Else
      MsgBox ErrStr, vbExclamation, "BASINS PEST"
    End If
  Else
    MsgBox "Need to specify observed and simulated data sets before running Pest!", vbExclamation, "BASINS PEST"
  End If
End Sub


Private Sub cmdUpdateUCI_Click()
  pPest.UpdateUCIFromSup
  MsgBox "UCI file " & pPest.UCI.Name & vbCrLf & "has been updated with the final set of PEST parameters.", vbInformation, "BASINS PEST"
End Sub

Private Sub Form_Load()
  Dim i As Long, j As Long
  Dim lOper As HspfOperation
  Dim lconn As HspfConnection
  Dim ff As New ATCoFindFile
  Dim lParmFile As New clsPestParmFile
  Dim lGroupFile As New clsPestGroupFile

  ff.SetDialogProperties "Please locate starter PEST parameter file", "parm_start.dat"
  ff.SetRegistryInfo "BASINS PEST", "Defaults", "PestParmStart"
  lParmFile.Filename = ff.GetName
  pPest.InitParmFile = lParmFile
  ff.SetDialogProperties "Please locate starter PEST group file", "group_start.dat"
  ff.SetRegistryInfo "BASINS PEST", "Defaults", "PestGroupStart"
  lGroupFile.Filename = ff.GetName
  pPest.InitGroupFile = lGroupFile
  lblUCIName.Caption = "Run PEST for " & pPest.ModelName & vbCrLf & _
                       "Input file is: " & pPest.UCI.Name
  For i = 1 To pPest.UCI.OpnSeqBlock.Opns.Count
    Set lOper = pPest.UCI.OpnSeqBlock.opn(i)
    For j = 1 To lOper.Targets.Count
      Set lconn = lOper.Targets(j)
      If Left(lconn.Target.volname, 3) = "WDM" And Trim(lconn.Target.member) = "FLOW" Then
        'this is an output flow location
        cboSimDSNs.AddItem CStr(lconn.Target.VolId)
      End If
    Next j
  Next i
  If cboSimDSNs.ListCount > 0 Then
    cboSimDSNs.ListIndex = 0
  Else
    MsgBox "No simulated FLOW data sets were found on the project WDM file." & vbCrLf & _
           "PEST cannot be run without observed and simulated FLOW data sets." & vbCrLf & _
           "Update UCI and WDM file to create simulated FLOW data set.", vbExclamation, "BASINS PEST"
  End If

End Sub

