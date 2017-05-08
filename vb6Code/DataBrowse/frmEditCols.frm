VERSION 5.00
Begin VB.Form frmEditCols 
   Caption         =   "Timeseries Columns"
   ClientHeight    =   3105
   ClientLeft      =   1230
   ClientTop       =   5445
   ClientWidth     =   7365
   HelpContextID   =   65
   Icon            =   "frmEditCols.frx":0000
   LinkTopic       =   "Form1"
   PaletteMode     =   1  'UseZOrder
   ScaleHeight     =   3105
   ScaleWidth      =   7365
   Begin ATCoCtl.ATCoSelectList asl 
      Height          =   2295
      Left            =   120
      TabIndex        =   0
      Top             =   0
      Width           =   7095
      _ExtentX        =   12515
      _ExtentY        =   4048
      RightLabel      =   "Show These Columns:"
      LeftLabel       =   "Available Columns:"
   End
   Begin VB.CommandButton cmdClose 
      Caption         =   "Reset"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
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
         Size            =   8.25
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
         Size            =   8.25
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
End
Attribute VB_Name = "frmEditCols"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Dim whichColumns& ' 1=SetShapeBrowseCols 2=

Private Sub cmdClose_Click(Index As Integer)

  If Index = 1 Then 'cancel
    Unload Me
  Else
    If asl.RightCount > 0 Then
      Dim c&, newCols&()
      ReDim newCols(0 To asl.RightCount - 1)
      For c = 0 To asl.RightCount - 1
        newCols(c) = asl.RightItemData(c)
      Next c
      SetShapeBrowseCols newCols
      frmMain.RefreshView
      Unload Me
    Else
      MsgBox "You must specify at least one column to show for the list.", 48
    End If
  End If

End Sub

Private Sub Form_Resize()
  Dim margin%
  margin = 225
  If Width > 4000 Then
    asl.left = margin
    asl.Width = Width - margin * 2
    'lblShow.Left = Width - margin - lblShow.Width
    'If lblShow.Left - margin > cmdClose(2).Left + cmdClose(2).Width Then lblShow.Visible = True Else lblShow.Visible = False
  End If
  If Height > 3000 Then
    cmdClose(0).top = Height - 900
    cmdClose(1).top = cmdClose(0).top
    cmdClose(2).top = cmdClose(0).top
    'lblShow.Top = cmdClose(0).Top + 120
    asl.Height = cmdClose(0).top - margin
  End If
End Sub
