VERSION 5.00
Begin VB.Form frmPointScenario 
   BorderStyle     =   1  'Fixed Single
   Caption         =   "WinHSPF - Create Point Source Scenario"
   ClientHeight    =   2220
   ClientLeft      =   36
   ClientTop       =   276
   ClientWidth     =   4560
   Icon            =   "frmPointScenario.frx":0000
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   2220
   ScaleWidth      =   4560
   StartUpPosition =   3  'Windows Default
   Begin ATCoCtl.ATCoText atxMult 
      Height          =   252
      Left            =   2640
      TabIndex        =   7
      Top             =   1200
      Width           =   1332
      _ExtentX        =   2350
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
      DefaultValue    =   1
      Value           =   "1"
      Enabled         =   -1  'True
   End
   Begin VB.TextBox txtNew 
      Height          =   288
      Left            =   2640
      TabIndex        =   6
      Text            =   "Text1"
      Top             =   720
      Width           =   1332
   End
   Begin VB.ComboBox cboBase 
      Height          =   288
      Left            =   2640
      Style           =   2  'Dropdown List
      TabIndex        =   5
      Top             =   240
      Width           =   1332
   End
   Begin VB.CommandButton cmdCancel 
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
      Height          =   372
      Left            =   2400
      TabIndex        =   4
      Top             =   1680
      Width           =   1212
   End
   Begin VB.CommandButton cmdOK 
      Caption         =   "&OK"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   372
      Left            =   960
      TabIndex        =   3
      Top             =   1680
      Width           =   1212
   End
   Begin VB.Label lblMult 
      Caption         =   "Multiplier:"
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
      Left            =   480
      TabIndex        =   2
      Top             =   1200
      Width           =   1692
   End
   Begin VB.Label lblNew 
      Caption         =   "New Scenario Name:"
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
      Left            =   480
      TabIndex        =   1
      Top             =   720
      Width           =   1932
   End
   Begin VB.Label lblBase 
      Caption         =   "Base on Scenario:"
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
      Left            =   480
      TabIndex        =   0
      Top             =   240
      Width           =   1932
   End
End
Attribute VB_Name = "frmPointScenario"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
Dim baseTs As Collection

Private Sub cmdCancel_Click()
  Unload Me
End Sub

Private Sub cmdOk_Click()
  Dim lts As Collection, i&, j&, ctemp$
  Dim sen$, loc$, con$, tstype$, stanam$
  Dim jdates!(), rload!(), newwdmid$, newdsn&
  Dim longloc$, Id&, rmult!
  Dim lOper As HspfOperation
  
  If Len(txtNew.Text) > 0 Then
    'new name has been entered
    Set baseTs = New Collection
    ctemp = "PT-" & cboBase.List(cboBase.ListIndex)
    Call myUci.FindTimSer(ctemp, "", "", lts)
    For i = 1 To lts.Count
      'create new timeseries
      sen = "PT-" & UCase(txtNew.Text)
      loc = lts(i).Header.loc
      con = lts(i).Header.con
      tstype = Mid(con, 1, 4)
      stanam = lts(i).Header.Desc
      longloc = loc
      If IsNumeric(Mid(loc, 4)) Then
        Id = CInt(Mid(loc, 4))
        Set loper = myUci.OpnBlks("RCHRES").operfromid(Id)
        If Not loper Is Nothing Then
          longloc = "RCHRES " & loper.Id & " - " & loper.Description
        End If
      End If
      rmult = atxMult.Value
      ReDim jdates(lts(i).dates.Summary.NVALS)
      ReDim rload(lts(i).dates.Summary.NVALS)
      For j = 1 To lts(i).dates.Summary.NVALS
        jdates(j) = lts(i).dates.Value(j)
        rload(j) = lts(i).Value(j) * rmult
      Next j
      myUci.AddPointSourceDataSet sen, loc, con, stanam, tstype, _
          0, jdates, rload, newwdmid, newdsn
      frmPoint.UpdateListsForNewPointSource sen, stanam, loc, con, newwdmid, _
        newdsn, "RCHRES", CInt(Mid(loc, 4)), longloc
    Next i
    Unload Me
  Else
    'no new scenario name entered
    MsgBox "A new scenario name must be entered.", vbOKOnly, "Create Scenario Problem"
  End If
End Sub

Private Sub Form_Load()
  Dim lts As Collection, i&, ctemp$
  
  cboBase.Clear
  Call myUci.FindTimSer("", "", "", lts)
  For i = 1 To lts.Count
    ctemp = lts(i).Header.sen
    If Len(ctemp) > 3 Then
      If Mid(ctemp, 1, 3) = "PT-" Then
        ctemp = Mid(ctemp, 4)
        If Not InList(ctemp, cboBase) Then
          cboBase.AddItem ctemp
        End If
      End If
    End If
  Next i
  If cboBase.listcount > 0 Then
    cboBase.ListIndex = 0
  End If
  txtNew.Text = ""
  
End Sub
