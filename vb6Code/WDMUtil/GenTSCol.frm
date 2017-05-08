VERSION 5.00
Begin VB.Form frmGenScnTSCol 
   Caption         =   "Timeseries Columns"
   ClientHeight    =   3108
   ClientLeft      =   1236
   ClientTop       =   5448
   ClientWidth     =   7356
   HelpContextID   =   41
   Icon            =   "GenTSCol.frx":0000
   LinkTopic       =   "Form1"
   PaletteMode     =   1  'UseZOrder
   ScaleHeight     =   3108
   ScaleWidth      =   7356
   Begin ATCoCtl.ATCoSelectList asl 
      Height          =   2295
      Left            =   120
      TabIndex        =   0
      Top             =   0
      Width           =   7095
      _ExtentX        =   12510
      _ExtentY        =   4043
      RightLabel      =   "Selected:"
      LeftLabel       =   "Available:"
   End
   Begin VB.CommandButton cmdClose 
      Caption         =   "Reset"
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
      Index           =   2
      Left            =   2040
      TabIndex        =   3
      Top             =   2400
      Width           =   852
   End
   Begin VB.CommandButton cmdClose 
      Cancel          =   -1  'True
      Caption         =   "Cancel"
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
      Index           =   1
      Left            =   1080
      TabIndex        =   2
      Top             =   2400
      Width           =   852
   End
   Begin VB.CommandButton cmdClose 
      Caption         =   "OK"
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
      Index           =   0
      Left            =   120
      TabIndex        =   1
      Top             =   2400
      Width           =   852
   End
   Begin VB.Label lblShow 
      Caption         =   "Columns displayed left to right"
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
      Left            =   4320
      TabIndex        =   4
      Top             =   2520
      Width           =   2895
   End
End
Attribute VB_Name = "frmGenScnTSCol"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Private Sub cmdClose_Click(Index As Integer)

    Dim i&, j&, nounld&
    Dim colstr$
    Dim clmX As ColumnHeader

    If Index = 0 Then 'ok
      If asl.RightCount > 0 Then
        frmGenScn.agdDSN.ClearData
        frmGenScn.agdDSN.cols = 1
        ntscol = 0
        For i = 0 To asl.RightCount - 1
          j = 0
          While j <= MXTSCOL
            If asl.RightItem(i) = tscolname(j) Then
              colstr = String(tscolwidth(j), "A")
              frmGenScn.agdDSN.ColTitle(ntscol) = tscolname(j)
              frmGenScn.agdDSN.ColSelectable(ntscol) = True
              tscolid(ntscol) = j
              ntscol = ntscol + 1
              j = MXTSCOL
            End If
            j = j + 1
          Wend
        Next i
        Call frmGenScn.RefreshTSList
      Else
        MsgBox "You must specify at least one column to show for the list of timeseries data.", 48
      End If
    End If
    If nounld = 0 Then 'ok to unload form
      Unload frmGenScnTSCol
    End If

End Sub

Private Sub Form_Load()
    Dim i&, j&
    Dim colnam$

    For i = 0 To MXTSCOL
      asl.LeftItem(i) = tscolname(i)
    Next i
    For i = 0 To ntscol - 2
      colnam = tscolname(tscolid(i))
      j = 0
      While j < asl.LeftCount
        If asl.LeftItem(j) = colnam Then
          asl.MoveRight j
          j = asl.LeftCount
        End If
        j = j + 1
      Wend
    Next i

End Sub

Private Sub Form_Resize()
  Dim margin%
  margin = 225
  If Width > 4000 Then
    asl.left = margin
    asl.Width = Width - margin * 2
    lblShow.left = Width - margin - lblShow.Width
    If lblShow.left - margin > cmdClose(2).left + cmdClose(2).Width Then lblShow.Visible = True Else lblShow.Visible = False
  End If
  If Height > 3000 Then
    cmdClose(0).Top = Height - 900
    cmdClose(1).Top = cmdClose(0).Top
    cmdClose(2).Top = cmdClose(0).Top
    lblShow.Top = cmdClose(0).Top + 120
    asl.Height = cmdClose(0).Top - margin
  End If
End Sub
