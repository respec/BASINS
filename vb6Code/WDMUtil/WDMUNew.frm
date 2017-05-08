VERSION 5.00
Begin VB.Form frmWDMUNew 
   Caption         =   "WDMUtil New WDM File"
   ClientHeight    =   1668
   ClientLeft      =   48
   ClientTop       =   336
   ClientWidth     =   5256
   BeginProperty Font 
      Name            =   "MS Sans Serif"
      Size            =   7.8
      Charset         =   0
      Weight          =   700
      Underline       =   0   'False
      Italic          =   0   'False
      Strikethrough   =   0   'False
   EndProperty
   LinkTopic       =   "Form1"
   ScaleHeight     =   1668
   ScaleWidth      =   5256
   StartUpPosition =   3  'Windows Default
   Begin VB.CommandButton cmdNew 
      Caption         =   "Cancel"
      Height          =   372
      Index           =   1
      Left            =   2640
      TabIndex        =   4
      Top             =   1200
      Width           =   852
   End
   Begin VB.CommandButton cmdNew 
      Caption         =   "OK"
      Height          =   372
      Index           =   0
      Left            =   1080
      TabIndex        =   3
      Top             =   1200
      Width           =   852
   End
   Begin VB.CheckBox chkBasInf 
      Caption         =   "Build associated BASINS information file"
      Height          =   252
      Left            =   120
      TabIndex        =   2
      Top             =   720
      Value           =   1  'Checked
      Width           =   4932
   End
   Begin VB.TextBox txtWDM 
      Height          =   288
      Left            =   2760
      TabIndex        =   1
      Top             =   240
      Width           =   2412
   End
   Begin VB.Label lblWDM 
      Caption         =   "Enter name of new WDM file:"
      Height          =   252
      Left            =   120
      TabIndex        =   0
      Top             =   240
      Width           =   2652
   End
End
Attribute VB_Name = "frmWDMUNew"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Private Sub cmdNew_Click(Index As Integer)

  Dim fun&, i&, FName$

  frmWDMUNew.Tag = ""
  If Index = 0 Then 'try to build new wdm file
    FName = CurDir & "\" & txtWDM.Text
    If Len(FName) > 0 Then 'valid name entered
      'create WDM file
      i = 2
      fun = F90_WDBOPN(i, FName, Len(FName))
      If fun > 0 Then 'successful build
        'close it up (will be opened within status file later)
        i = F90_WDFLCL(fun)
        If chkBasInf.Value = 1 Then 'build BASINS info file
          NoBasInf = False
        Else 'don't build BASINS info file
          NoBasInf = True
        End If
        'pass new file name back to main
        frmWDMUNew.Tag = FName
      Else 'problem building WDM file
        MsgBox "Problem building WDM file: " & FName, vbExclamation, "WDMUtil New WDM File Problem"
      End If
    End If
  End If
  frmWDMUNew.Hide

End Sub
