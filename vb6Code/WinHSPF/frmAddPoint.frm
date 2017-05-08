VERSION 5.00
Begin VB.Form frmAddPoint 
   BorderStyle     =   1  'Fixed Single
   Caption         =   "WinHSPF - Create Point Source"
   ClientHeight    =   2532
   ClientLeft      =   48
   ClientTop       =   336
   ClientWidth     =   4680
   HelpContextID   =   39
   Icon            =   "frmAddPoint.frx":0000
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   2532
   ScaleWidth      =   4680
   StartUpPosition =   2  'CenterScreen
   Begin ATCoCtl.ATCoText atxValue 
      Height          =   255
      Left            =   1320
      TabIndex        =   12
      Top             =   1560
      Width           =   1575
      _ExtentX        =   2773
      _ExtentY        =   445
      InsideLimitsBackground=   16777215
      OutsideHardLimitBackground=   8421631
      OutsideSoftLimitBackground=   8454143
      HardMax         =   -999
      HardMin         =   -999
      SoftMax         =   -999
      SoftMin         =   -999
      MaxWidth        =   -999
      Alignment       =   1
      DataType        =   2
      DefaultValue    =   "0.0"
      Value           =   "0.0"
      Enabled         =   -1  'True
   End
   Begin VB.ComboBox cboFac 
      Height          =   315
      Left            =   1320
      TabIndex        =   10
      Text            =   " "
      Top             =   1200
      Width           =   3135
   End
   Begin VB.ComboBox cboPollutant 
      Height          =   315
      Left            =   1320
      TabIndex        =   9
      Text            =   " "
      Top             =   840
      Width           =   3135
   End
   Begin VB.ComboBox cboReach 
      Height          =   315
      Left            =   1320
      Style           =   2  'Dropdown List
      TabIndex        =   8
      Top             =   480
      Width           =   3135
   End
   Begin VB.TextBox txtScen 
      Height          =   285
      Left            =   1680
      TabIndex        =   3
      Top             =   120
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
      Left            =   2520
      TabIndex        =   1
      Top             =   2040
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
      Top             =   2040
      Width           =   1335
   End
   Begin VB.Label Label6 
      Caption         =   "Daily Load"
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
      TabIndex        =   11
      Top             =   1560
      Width           =   1215
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
      TabIndex        =   7
      Top             =   1200
      Width           =   975
   End
   Begin VB.Label Label4 
      Caption         =   "Pollutant"
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
      Top             =   840
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
      Top             =   480
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
      Top             =   120
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
      Top             =   120
      Width           =   1095
   End
End
Attribute VB_Name = "frmAddPoint"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2002 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Private Sub cmdPoint_Click(Index As Integer)
  Dim sen$, loc$, con$, stanam$, tstype$, load!, imready As Boolean
  Dim dashpos&, jdates!(1), rload!(1), newwdmid$, newdsn&, longloc$
  
  imready = True
  If Index = 0 Then
    'ok, check to make sure everything is filled
    If Len(txtScen) = 0 Then
      MsgBox "A scenario name must be entered.", vbOKOnly, "Create Point Source Problem"
      imready = False
    End If
    If Len(cboReach.List(cboReach.ListIndex)) = 0 Then
      MsgBox "A reach must be selected.", vbOKOnly, "Create Point Source Problem"
      imready = False
    End If
    If cboPollutant.ListIndex = -1 And Len(cboPollutant.Text) = 0 Then
      MsgBox "A pollutant must be entered.", vbOKOnly, "Create Point Source Problem"
      imready = False
    End If
    If imready Then
      sen = "PT-" & UCase(Trim(txtScen))
      longloc = Trim(cboReach.List(cboReach.ListIndex))
      dashpos = InStr(1, longloc, "-")
      loc = "RCH" & Trim(Mid(longloc, 7, dashpos - 7))
      If cboPollutant.ListIndex > -1 Then
        con = Mid(Trim(cboPollutant.List(cboPollutant.ListIndex)), 1, 8)
      Else
        con = Mid(Trim(cboPollutant.Text), 1, 8)
      End If
      If cboFac.ListIndex > -1 Then
        stanam = Trim(cboFac.List(cboFac.ListIndex))
      Else
        stanam = cboFac.Text
      End If
      con = UCase(con)
      stanam = UCase(stanam)
      tstype = Mid(con, 1, 4)
      If con = "FLOW" Then
        rload(1) = atxValue.Value
      Else
        rload(1) = atxValue.Value / 24#  'next call converts to daily
      End If
      myUci.AddPointSourceDataSet sen, loc, con, stanam, tstype, _
          0, jdates, rload, newwdmid, newdsn
      frmPoint.UpdateListsForNewPointSource sen, stanam, loc, con, newwdmid, _
        newdsn, "RCHRES", CInt(Mid(loc, 4)), longloc
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
  
  cboPollutant.Clear
  'For Each vpol In PollutantList
  '  cboPollutant.AddItem vpol
  'Next vpol
  For i = 1 To frmPoint.agdMasterPoint.rows
    ctmp = frmPoint.agdMasterPoint.textmatrix(i, 4)
    If Not InList(ctmp, cboPollutant) Then
      cboPollutant.AddItem ctmp
    End If
  Next i
  
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
  
End Sub

