VERSION 5.00
Begin VB.Form frmCheckBoxes 
   Caption         =   "Select Updates to Install"
   ClientHeight    =   3195
   ClientLeft      =   60
   ClientTop       =   345
   ClientWidth     =   6480
   Icon            =   "frmCheckBoxes.frx":0000
   LinkTopic       =   "Form1"
   ScaleHeight     =   3195
   ScaleWidth      =   6480
   StartUpPosition =   3  'Windows Default
   Begin VB.Frame fraButtons 
      BorderStyle     =   0  'None
      Height          =   615
      Left            =   360
      TabIndex        =   4
      Top             =   2520
      Width           =   5295
      Begin VB.CommandButton cmdAllNone 
         Caption         =   "Select &None"
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
         Index           =   1
         Left            =   1200
         TabIndex        =   8
         Top             =   120
         Width           =   1335
      End
      Begin VB.CommandButton cmdAllNone 
         Caption         =   "Select &All"
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
         Index           =   0
         Left            =   0
         TabIndex        =   7
         Top             =   120
         Width           =   1095
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
         Left            =   4080
         TabIndex        =   6
         Top             =   120
         Width           =   1095
      End
      Begin VB.CommandButton cmdOk 
         Caption         =   "Ok"
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
         Height          =   375
         Left            =   2760
         TabIndex        =   5
         Top             =   120
         Width           =   1095
      End
   End
   Begin VB.Frame fraChk 
      BorderStyle     =   0  'None
      Height          =   375
      Index           =   0
      Left            =   120
      TabIndex        =   1
      Top             =   720
      Width           =   6255
      Begin VB.CommandButton cmdDetails 
         Caption         =   "Details"
         Height          =   315
         Index           =   0
         Left            =   5400
         TabIndex        =   3
         Top             =   0
         Width           =   855
      End
      Begin VB.CheckBox chk 
         Height          =   255
         Index           =   0
         Left            =   0
         TabIndex        =   2
         Top             =   60
         Value           =   1  'Checked
         Width           =   5175
      End
   End
   Begin VB.Label lblTop 
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
      Left            =   240
      TabIndex        =   0
      Top             =   240
      Width           =   4335
   End
End
Attribute VB_Name = "frmCheckBoxes"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Private Const pBaseChkWidth As Long = 375
Private Const pBaseCmdWidth As Long = 375
Private pMaxWidth As Long
Private pSelectedKeys() As String

Private Sub chk_Click(Index As Integer)
  Dim RequiresPos As Integer
  Dim RequiresStr As String
  Dim RequiresStrPos As Integer
  Dim SetChk As Integer
  Dim ComponentName As String
  Dim Desc As String
  Dim SearchStr As String
  Dim SkipAfterSearch As Integer
  Dim OtherNameDirection As Integer

  ComponentName = chk(Index).Caption
  Desc = cmdDetails(Index).Tag

  If chk(Index).Value = vbChecked Then 'Check everything this one requires
    SearchStr = ComponentName & " requires "
    SkipAfterSearch = Len(SearchStr)
    OtherNameDirection = 1
  Else 'If we un-selected this, uncheck whatever requires it, too
    SearchStr = " requires " & ComponentName
    SkipAfterSearch = -1
    OtherNameDirection = -1
  End If
    
  RequiresPos = InStr(Desc, SearchStr)
  While RequiresPos > 0
    RequiresStrPos = RequiresPos + SkipAfterSearch
    While Asc(Mid(Desc, RequiresStrPos)) > 32
      RequiresStrPos = RequiresStrPos + OtherNameDirection
    Wend
    If RequiresStrPos < RequiresPos Then
      RequiresStr = Mid(Desc, RequiresStrPos + 1, RequiresPos + SkipAfterSearch - RequiresStrPos)
    Else
      RequiresStr = Mid(Desc, RequiresPos + SkipAfterSearch, RequiresStrPos - (RequiresPos + SkipAfterSearch))
    End If
    For SetChk = chk.LBound To chk.UBound
      If chk(SetChk).Caption = RequiresStr Then
        If chk(SetChk).Value <> chk(Index).Value Then
          chk(SetChk).Value = chk(Index).Value
        End If
      End If
    Next
    RequiresPos = InStr(RequiresPos + 1, Desc, SearchStr)
  Wend
    
End Sub

Private Sub cmdAllNone_Click(Index As Integer)
  Dim chkIndex As Long
  For chkIndex = chk.LBound To chk.UBound
    If Index = 0 Then
      chk(chkIndex).Value = vbChecked
    Else
      chk(chkIndex).Value = vbUnchecked
    End If
  Next
End Sub

Private Sub cmdCancel_Click()
  Me.Hide
End Sub

Private Sub cmdDetails_Click(Index As Integer)
  LogMsg cmdDetails(Index).Tag, chk(Index).Caption & " " & cmdDetails(Index).Caption
End Sub

Private Sub cmdOk_Click()
  Dim nSelected As Long
  Dim i As Long
  
  nSelected = 0
  For i = 0 To fraChk.UBound
    If chk(i).Value = vbChecked Then nSelected = nSelected + 1
  Next
  
  ReDim pSelectedKeys(nSelected)
  
  nSelected = 0
  For i = 0 To fraChk.UBound
    If chk(i).Value = vbChecked Then
      nSelected = nSelected + 1
      pSelectedKeys(nSelected) = chk(i).Tag
    End If
  Next
  Me.Hide
End Sub

Private Sub Form_Resize()
  Dim w As Long
  Dim h As Long
  Dim i As Long
  w = ScaleWidth
  h = ScaleHeight
  If w > fraButtons.Width Then
    For i = 0 To fraChk.UBound
      fraChk(i).Width = w - 480
      cmdDetails(i).Left = fraChk(i).Width - cmdDetails(i).Width
      chk(i).Width = cmdDetails(i).Left
    Next
    fraButtons.Left = (w - fraButtons.Width) / 2
  End If
  If h > 1000 Then fraButtons.Top = h - fraButtons.Height
End Sub

Public Sub Add(chkCaption As String, _
               key As String, _
               details As String, _
               startChecked As Boolean, _
               Optional DetailsCaption As String = "Details")
  Dim fraWidth As Long
  Dim i As Long
  
  i = fraChk.UBound
  If Len(chk(i).Caption) > 0 Then
    i = i + 1
    Load fraChk(i)
    Load chk(i):               Set chk(i).Container = fraChk(i)
    Load cmdDetails(i): Set cmdDetails(i).Container = fraChk(i)
    fraChk(i).Top = fraChk(i - 1).Top + fraChk(i - 1).Height + 50
  End If
  fraChk(i).Visible = True
  chk(i).Visible = True
  cmdDetails(i).Visible = True
  
  If Len(details) = 0 Then
    cmdDetails(i).Width = 0
  Else
    cmdDetails(i).Caption = DetailsCaption
    cmdDetails(i).Width = Me.TextWidth(DetailsCaption) + pBaseCmdWidth
    cmdDetails(i).Tag = details
  End If
  
  chk(i).Tag = key
  chk(i).Caption = chkCaption
  'chk(i).ToolTipText = details
  fraWidth = Me.TextWidth(chkCaption) + cmdDetails(i).Width + pBaseChkWidth
  If fraWidth > pMaxWidth Then pMaxWidth = fraWidth

  If startChecked Then
    chk(i).Value = vbChecked
  Else
    chk(i).Value = vbUnchecked
  End If
  LogDbg "  frmCheckBoxes: " & chkCaption & " : " & details
End Sub

Public Function AskUserToSelect(titleBar As String, topLabel As String) As String()
  Me.Caption = titleBar
  lblTop.Caption = topLabel
  If Me.TextWidth(topLabel) > pMaxWidth Then pMaxWidth = Me.TextWidth(topLabel)
  SizeFromContents
  ReDim pSelectedKeys(0)
  Me.Show
  Do
    DoEvents
    Sleep 50
    DoEvents
  Loop While Me.Visible
  AskUserToSelect = pSelectedKeys
End Function

Private Sub SizeFromContents()
  Dim MinWidth As Long
  Dim newWidth As Long
  Dim newHeight As Long
  
  'Find the best available width for window. (Width - ScaleWidth) = width of border
  newWidth = pMaxWidth + 480 + Width - ScaleWidth
  MinWidth = fraButtons.Width + 300 + Width - ScaleWidth
  If newWidth < MinWidth Then newWidth = MinWidth
  If newWidth > Screen.Width * 0.9 Then
    Width = Screen.Width * 0.9
  Else
    Width = newWidth
  End If
  
  'Find best available height for window. (Height - ScaleHeight) is the height of the title bar and border
  With fraChk(fraChk.UBound)
    newHeight = .Top + .Height + fraButtons.Height + Height - ScaleHeight
  End With
  If newHeight > Screen.Height * 0.9 Then
    Height = Screen.Height * 0.9
  Else
    Height = newHeight
  End If

End Sub
