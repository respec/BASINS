VERSION 5.00
Begin VB.Form frmConflicts 
   Caption         =   "Some files in archive already exist"
   ClientHeight    =   4965
   ClientLeft      =   60
   ClientTop       =   345
   ClientWidth     =   7470
   Icon            =   "frmConflicts.frx":0000
   LinkTopic       =   "Form1"
   ScaleHeight     =   4965
   ScaleWidth      =   7470
   StartUpPosition =   3  'Windows Default
   Begin ATCoCtl.ATCoSelectList asl 
      Height          =   3735
      Left            =   120
      TabIndex        =   0
      Top             =   480
      Width           =   7815
      _ExtentX        =   13785
      _ExtentY        =   6588
      RightLabel      =   "Overwrite Existing Files:"
      LeftLabel       =   "Conflicting Files:"
   End
   Begin VB.Frame fraButtons 
      BorderStyle     =   0  'None
      Height          =   375
      Left            =   2160
      TabIndex        =   2
      Top             =   4440
      Width           =   3135
      Begin VB.CommandButton cmdOk 
         Caption         =   "Ok"
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
         Left            =   0
         TabIndex        =   4
         Top             =   0
         Width           =   1455
      End
      Begin VB.CommandButton cmdCancel 
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
         Height          =   375
         Left            =   1680
         TabIndex        =   3
         Top             =   0
         Width           =   1455
      End
   End
   Begin VB.Label Label1 
      Caption         =   "Select which existing files should be replaced with files from the archive"
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
      Left            =   240
      TabIndex        =   1
      Top             =   120
      Width           =   7575
   End
End
Attribute VB_Name = "frmConflicts"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Private pButtonPressed As Integer

Public Function Cancelled() As Boolean
  If pButtonPressed = -1 Then Cancelled = True
End Function

Public Function SelectFiles(AvailableFiles As FastCollection) As FastCollection
  Dim i As Long
  Dim retval As New FastCollection
  asl.ClearLeft
  asl.ClearRight
  For i = 1 To AvailableFiles.Count
    asl.LeftItemFastAdd AvailableFiles.ItemByIndex(i)
    asl.LeftItemData(i - 1) = AvailableFiles.Key(i)
    Debug.Print AvailableFiles.ItemByIndex(i) & " + " & AvailableFiles.Key(i)
  Next
  pButtonPressed = 0
  Me.Show
  While pButtonPressed = 0
    DoEvents
    Sleep 50
  Wend
  Me.Hide
  DoEvents
  If pButtonPressed = 1 Then
    For i = 0 To asl.RightCount - 1
      retval.Add asl.RightItem(i), asl.RightItemData(i)
      pLogger.Log asl.RightItem(i) & " + " & asl.RightItemData(i)
    Next
  End If
  Set SelectFiles = retval
End Function

Private Sub cmdCancel_Click()
  pButtonPressed = -1
End Sub

Private Sub cmdOk_Click()
  pButtonPressed = 1
End Sub

Private Sub Form_QueryUnload(Cancel As Integer, UnloadMode As Integer)
  If UnloadMode = vbFormControlMenu And pButtonPressed = 0 Then
    'Make clicking X in the corner of the window the same as clicking Cancel button
    pButtonPressed = -1
    Cancel = 1
  End If
End Sub

Private Sub Form_Resize()
  Dim w As Long, h As Long
  w = Me.ScaleWidth
  h = Me.ScaleHeight
  
  If w > 500 And h > 3500 Then
    fraButtons.Left = (w - fraButtons.Width) / 2
    fraButtons.Top = h - 525
    asl.Height = fraButtons.Top - asl.Top - 140
    asl.Width = w + 345
  End If
End Sub
