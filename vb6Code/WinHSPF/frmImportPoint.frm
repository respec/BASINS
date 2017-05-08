VERSION 5.00
Begin VB.Form frmImportPoint 
   BorderStyle     =   1  'Fixed Single
   Caption         =   "WinHSPF - Import Point Source"
   ClientHeight    =   2460
   ClientLeft      =   48
   ClientTop       =   336
   ClientWidth     =   4620
   HelpContextID   =   39
   Icon            =   "frmImportPoint.frx":0000
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   2460
   ScaleWidth      =   4620
   StartUpPosition =   2  'CenterScreen
   Begin VB.CommandButton cmdFile 
      Caption         =   "Select"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   252
      Index           =   0
      Left            =   120
      TabIndex        =   9
      Top             =   240
      Width           =   732
   End
   Begin VB.ComboBox cboFac 
      Height          =   315
      Left            =   1320
      TabIndex        =   8
      Text            =   " "
      Top             =   1440
      Width           =   3135
   End
   Begin VB.ComboBox cboReach 
      Height          =   315
      Left            =   1320
      Style           =   2  'Dropdown List
      TabIndex        =   7
      Top             =   1080
      Width           =   3135
   End
   Begin VB.TextBox txtScen 
      Height          =   285
      Left            =   1680
      TabIndex        =   3
      Top             =   720
      Width           =   855
   End
   Begin VB.CommandButton cmdPoint 
      Cancel          =   -1  'True
      Caption         =   "&Cancel"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Index           =   1
      Left            =   2400
      TabIndex        =   1
      Top             =   1920
      Width           =   1335
   End
   Begin VB.CommandButton cmdPoint 
      Caption         =   "&OK"
      Default         =   -1  'True
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Index           =   0
      Left            =   840
      TabIndex        =   0
      Top             =   1920
      Width           =   1335
   End
   Begin MSComDlg.CommonDialog CDFile 
      Left            =   4080
      Top             =   240
      _ExtentX        =   699
      _ExtentY        =   699
      _Version        =   393216
      FontSize        =   4.09255e-38
   End
   Begin VB.Label lblFile 
      Alignment       =   1  'Right Justify
      BorderStyle     =   1  'Fixed Single
      Caption         =   "<none>"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   255
      Index           =   0
      Left            =   1080
      TabIndex        =   10
      Top             =   240
      Width           =   3375
   End
   Begin VB.Label Label5 
      Caption         =   "Facility"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   255
      Left            =   120
      TabIndex        =   6
      Top             =   1440
      Width           =   975
   End
   Begin VB.Label Label3 
      Caption         =   "Reach"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   255
      Left            =   120
      TabIndex        =   5
      Top             =   1080
      Width           =   855
   End
   Begin VB.Label Label2 
      Alignment       =   1  'Right Justify
      Caption         =   "PT-"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   255
      Left            =   1200
      TabIndex        =   4
      Top             =   720
      Width           =   495
   End
   Begin VB.Label Label1 
      Caption         =   "Scenario"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   255
      Left            =   120
      TabIndex        =   2
      Top             =   720
      Width           =   1095
   End
End
Attribute VB_Name = "frmImportPoint"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2002 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Dim facilityname$, retcod&, nconstits$, consnames$(), ndates&
Dim jdates!(), rloads!()

Private Sub cmdFile_Click(Index As Integer)
  Dim i&, s$, f$, fun&, wid$
  Dim tmetseg, ret&
 
  If FileExists(BASINSPath & "\modelout", True, False) Then
    ChDriveDir BASINSPath & "\modelout"
  End If
  cdFile.flags = &H8806&
  cdFile.Filter = "MUTSIN Files in BASINS 2 Format(*.mut)"
  cdFile.Filename = "*.mut"
  cdFile.DialogTitle = "Select MUTSIN File"
  On Error GoTo 50
  cdFile.CancelError = True
  cdFile.Action = 1
  lblFile(Index).Caption = cdFile.Filename
  'read mustin file
  Call ReadMutsin(lblFile(Index).Caption, facilityname, retcod, nconstits, consnames(), _
                           ndates, jdates(), rloads())
  cboFac.Text = facilityname
50        'continue here on cancel
End Sub

Private Sub cmdPoint_Click(Index As Integer)
  Dim sen$, loc$, con$, stanam$, tstype$, imready As Boolean
  Dim dashpos&, i&, rtemp!(), j&, newwdmid$, newdsn&, longloc$
  
  imready = True
  If Index = 0 Then
    'ok, check to make sure everything is filled
    If retcod <> 0 Then
      MsgBox "An input file be specified.", vbOKOnly, "Import Point Source Problem"
        imready = False
    Else
      If Len(txtScen) = 0 Then
        MsgBox "A scenario name must be entered.", vbOKOnly, "Import Point Source Problem"
        imready = False
      End If
      If Len(cboReach.List(cboReach.ListIndex)) = 0 Then
        MsgBox "A reach must be selected.", vbOKOnly, "Import Point Source Problem"
        imready = False
      End If
      If imready Then
        sen = "PT-" & UCase(Trim(txtScen))
        longloc = Trim(cboReach.List(cboReach.ListIndex))
        dashpos = InStr(1, longloc, "-")
        loc = "RCH" & Trim(Mid(longloc, 7, dashpos - 7))
        For i = 1 To nconstits
          con = UCase(consnames(i))
          If Len(con) > 8 Then
            con = Trim(Mid(con, 1, 8))
          End If
          stanam = UCase(facilityname)
          tstype = Mid(con, 1, 4)
          ReDim rtemp(ndates)
          For j = 1 To ndates
            rtemp(j) = rloads(((j - 1) * nconstits) + i)
          Next j
          myUci.AddPointSourceDataSet sen, loc, con, stanam, _
             tstype, ndates, jdates, rtemp, newwdmid, newdsn
          frmPoint.UpdateListsForNewPointSource sen, stanam, loc, con, newwdmid, _
             newdsn, "RCHRES", CInt(Mid(loc, 4)), longloc
        Next i
      End If
    End If
  End If
  If imready Then
    Unload Me
  End If
End Sub

Private Sub Form_Load()
  Dim lOper As HspfOperation
  Dim lOpnBlk As HspfOpnBlk
  Dim vpol As Variant, vfac As Variant
  Dim i&, ctmp$
  
  cboReach.Clear
  Set lOpnBlk = myUci.OpnBlks("RCHRES")
  For i = 1 To lOpnBlk.Count
    Set lOper = lOpnBlk.Ids(i)
    cboReach.AddItem "RCHRES " & lOper.id & " - " & lOper.Description
  Next i
  cboReach.ListIndex = 0
  
  cboFac.Clear
  'For i = 1 To frmPoint.cmbFac.ListCount - 1
  '  cboFac.AddItem frmPoint.cmbFac.List(i)
  'Next i
  For i = 1 To frmPoint.agdMasterPoint.rows
    ctmp = frmPoint.agdMasterPoint.textmatrix(i, 3)
    If Not InList(ctmp, cboFac) Then
      cboFac.AddItem ctmp
    End If
  Next i
  
  retcod = -1
  
End Sub

