VERSION 5.00
Begin VB.Form frmAddOperation 
   BorderStyle     =   1  'Fixed Single
   Caption         =   "Add Operation"
   ClientHeight    =   4335
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   5310
   HelpContextID   =   31
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   4335
   ScaleWidth      =   5310
   StartUpPosition =   2  'CenterScreen
   Begin VB.ListBox lstDown 
      Height          =   1620
      Left            =   2760
      MultiSelect     =   1  'Simple
      TabIndex        =   9
      Top             =   1800
      Width           =   2175
   End
   Begin VB.ListBox lstUp 
      Height          =   1620
      Left            =   360
      MultiSelect     =   1  'Simple
      TabIndex        =   8
      Top             =   1800
      Width           =   2175
   End
   Begin VB.CommandButton cmdOk 
      Caption         =   "&OK"
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
      Height          =   495
      Left            =   1560
      TabIndex        =   7
      Top             =   3720
      Width           =   852
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
      Height          =   495
      Left            =   2760
      TabIndex        =   6
      Top             =   3720
      Width           =   852
   End
   Begin VB.TextBox Text1 
      Height          =   285
      Left            =   4320
      TabIndex        =   4
      Text            =   " "
      Top             =   360
      Width           =   615
   End
   Begin VB.TextBox txtDesc 
      Height          =   285
      Left            =   1800
      TabIndex        =   3
      Top             =   960
      Width           =   3135
   End
   Begin VB.ComboBox cboOpTyp 
      Height          =   315
      Left            =   1800
      TabIndex        =   0
      Text            =   "Combo1"
      Top             =   360
      Width           =   1335
   End
   Begin VB.Label lblDown 
      Caption         =   "Downstream Operations"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   255
      Left            =   2760
      TabIndex        =   11
      Top             =   1560
      Width           =   2175
   End
   Begin VB.Label lblUp 
      Caption         =   "Upstream Operations"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   255
      Left            =   360
      TabIndex        =   10
      Top             =   1560
      Width           =   2175
   End
   Begin VB.Label lblNumber 
      Alignment       =   1  'Right Justify
      Caption         =   "Number"
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
      Left            =   3480
      TabIndex        =   5
      Top             =   360
      Width           =   735
   End
   Begin VB.Label lblDesc 
      Alignment       =   1  'Right Justify
      Caption         =   "Description"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   255
      Left            =   480
      TabIndex        =   2
      Top             =   960
      Width           =   1215
   End
   Begin VB.Label lblOpTyp 
      Alignment       =   1  'Right Justify
      Caption         =   "Operation Type"
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
      Left            =   120
      TabIndex        =   1
      Top             =   360
      Width           =   1575
   End
End
Attribute VB_Name = "frmAddOperation"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2002 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Dim pCtl As ctlOpnSeqBlkEdit
Dim pUci As HspfUci
Dim pGrid As atcogrid

Public Sub init(u As HspfUci, ctl As ctlOpnSeqBlkEdit, g As atcogrid)
  Dim i&
  
  Set pCtl = ctl
  Set pUci = u
  Set pGrid = g
  
  Me.icon = ctl.frm.icon
  With cboOpTyp
    .Clear
    For i = 1 To u.OpnBlks.Count
      .AddItem u.OpnBlks(i).Name
    Next i
  End With
  
  lstUp.Clear
  lstDown.Clear
  For i = 1 To u.OpnSeqBlock.Opns.Count
    If i < pGrid.selstartrow Or (i = pGrid.selstartrow And i = pGrid.rows) Then
      lstUp.AddItem u.OpnSeqBlock.Opn(i).Name & " " & u.OpnSeqBlock.Opn(i).Id
    Else
      lstDown.AddItem u.OpnSeqBlock.Opn(i).Name & " " & u.OpnSeqBlock.Opn(i).Id
    End If
  Next i
  
End Sub

Private Sub cmdCancel_Click()
  Unload Me
End Sub

Private Sub cmdOK_Click()
  Dim opname$, opid&, Desc$, i&, ctxt$, j&, tname$, tnum&

  'first check to make sure this name/number are okay
  opname = cboOpTyp
  If IsNumeric(Text1) Then
    opid = CInt(Text1)
  Else
    opid = 0
  End If
  Desc = txtDesc
  If Len(opname) > 0 And opid > 0 Then
    If pUci.OpnBlks(opname).OperFromID(opid) Is Nothing Then
      'okay oper to add
      pUci.AddOperation opname, opid
      pCtl.AddOperationToOpnSeqBlock opname, opid, pGrid.selstartrow
      pUci.OpnBlks(opname).OperFromID(opid).Description = Desc
      With pGrid
        If .selstartrow > 0 And .selstartrow < .rows Then
          .InsertRow .selstartrow
          .TextMatrix(.selstartrow, 0) = opname
          .TextMatrix(.selstartrow, 1) = opid
          pCtl.DoLimits
        Else
          .InsertRow .selstartrow
          .TextMatrix(.selstartrow + 1, 0) = opname
          .TextMatrix(.selstartrow + 1, 1) = opid
          pCtl.DoLimits
        End If
      End With
      If lstUp.SelCount > 0 Then
        'want some upstream operations
        For i = 0 To lstUp.ListCount - 1
          If lstUp.Selected(i) Then
            ctxt = lstUp.List(i)
            j = InStr(1, ctxt, " ")
            tname = Mid(ctxt, 1, j - 1)
            tnum = CInt(Mid(ctxt, j + 1))
            Call AddConnection(tname, tnum, opname, opid)
          End If
        Next i
      End If
      If lstDown.SelCount > 0 Then
        'want some downstream operations
        For i = 0 To lstDown.ListCount - 1
          If lstDown.Selected(i) Then
            ctxt = lstDown.List(i)
            j = InStr(1, ctxt, " ")
            tname = Mid(ctxt, 1, j - 1)
            tnum = CInt(Mid(ctxt, j + 1))
            Call AddConnection(opname, opid, tname, tnum)
          End If
        Next i
      End If
      Unload Me
    Else
      'this oper already exists!
      myMsgBox.Show "This operation already exists." & vbCrLf & vbCrLf & _
        "Try a different operation type or number.", _
        "Add Operation Problem", "-+&OK"
      'MsgBox "This operation already exists." & vbCrLf & vbCrLf & _
      '  "Try a different operation type or number.", vbOKOnly, _
      '  "Add Operation Problem"
    End If
  Else
    'some info not entered!
    myMsgBox.Show "A valid value has not been entered for the" & vbCrLf & _
        "operation type or number.", _
        "Add Operation Problem", "-+&OK"
    'MsgBox "A valid value has not been entered for the" & vbCrLf & _
    '    "operation type or number.", vbOKOnly, _
    '    "Add Operation Problem"
  End If
End Sub

Private Sub Form_Unload(Cancel As Integer)
  'pCtl.frm.Show
End Sub

Private Sub AddConnection(sname$, sid&, tname$, Tid&)
  Dim mlid&
  Dim lConnection As HspfConnection
  
  If Not pUci.MassLinks Is Nothing Then
    mlid = pUci.MassLinks(1).FindMassLinkID(sname, tname)
  Else
    mlid = 0
  End If
  If mlid < 1 Then
    'no masslink exists for this combination, display message
    myMsgBox.Show "No connections exist between operation types " & sname & " and " & tname & "." & vbCrLf & vbCrLf & _
        "This connection must be added manually by editing the Schematic and MassLink Blocks.", _
        "Add Operation Problem", "-+&OK"
    'MsgBox "No connections exist between operation types " & sname & " and " & tname & "." & vbCrLf & vbCrLf & _
    '    "This connection must be added manually by editing the Schematic and MassLink Blocks.", _
    '    , vbOKOnly, "Add Operation Problem"
  Else
    'masslink exists, go ahead and add connection
    Set lConnection = New HspfConnection
    Set lConnection.Uci = pUci
    lConnection.typ = 3
    lConnection.Source.VolName = sname
    lConnection.Source.VolId = sid
    Set lConnection.Source.Opn = pUci.OpnBlks(sname).OperFromID(sid)
    lConnection.MFact = 1
    lConnection.Target.VolName = tname
    lConnection.Target.VolId = Tid
    Set lConnection.Target.Opn = pUci.OpnBlks(tname).OperFromID(Tid)
    lConnection.MassLink = mlid
    pUci.Connections.Add lConnection
    pUci.OpnBlks(sname).OperFromID(sid).Targets.Add lConnection
    pUci.OpnBlks(tname).OperFromID(Tid).Sources.Add lConnection
  End If
End Sub
