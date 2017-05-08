VERSION 5.00
Begin VB.Form frmMsg 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "Dialog Caption"
   ClientHeight    =   2160
   ClientLeft      =   2760
   ClientTop       =   3756
   ClientWidth     =   4116
   Icon            =   "frmMsg.frx":0000
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   2160
   ScaleWidth      =   4116
   ShowInTaskbar   =   0   'False
   Begin VB.CheckBox chkShow 
      Caption         =   "Show this message next time"
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
      Left            =   600
      TabIndex        =   2
      Top             =   1320
      Value           =   1  'Checked
      Visible         =   0   'False
      Width           =   3135
   End
   Begin VB.CommandButton cmd 
      Caption         =   "Ok"
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
      Left            =   1440
      TabIndex        =   0
      Top             =   1680
      Width           =   1215
   End
   Begin VB.Label lblMessage 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   "Label1"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   915
      Left            =   120
      TabIndex        =   1
      Top             =   240
      Width           =   3945
   End
End
Attribute VB_Name = "frmMsg"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2002 by AQUA TERRA Consultants
'See documentation in ATCoMessage

Private ButtonPressed&
Private LostFocusButton As Integer
Private pRegApp$
Private pRegSection$
Private pRegKey$
Private ModalOrNot As Long
Private ShowChk As Boolean

Private Const MinButWidth = 1200
Private Const ButSpacing = 130

Public Event ButtonPress(Message$, title$, Button&, buttonLabel$)

'We assume that the registry key has already been checked by ATCoMessage
' and if its value = 0 we are not being called here
'If pRegApp <> "" then we show the checkbox,
' allowing the user to change the value to 0 (unchecked) or 1 (checked)
Public Sub ShowMessage(Message$, title$, ButtonOnLostFocus&, _
                        regApp$, regSection$, regKey$, ButtonName())
  Dim butNum&, numNames&, item, lCaption$
  
  pRegApp = regApp
  pRegSection = regSection
  pRegKey = regKey
  
  If pRegApp <> "" And pRegSection <> "" And pRegKey <> "" Then
    ShowChk = True
  Else
    ShowChk = False
  End If
  
  ButtonPressed = -1
  LostFocusButton = -1
  lblMessage.Caption = Message
  Me.Caption = title
  
  ModalOrNot = vbModeless
  On Error GoTo MakeModal
  Me.Hide
MadeModal:

  numNames = UBound(ButtonName) + 1
'  numNames = 0
'  For Each item In ButtonName
'    If LCase(item) = "vbmodal" Then
'      ModalOrNot = vbModal
'    ElseIf LCase(item) = "vbmodeless" Then
'      ModalOrNot = vbModeless
'    Else
'      numNames = numNames + 1
'    End If
'  Next
'  If ModalOrNot = vbModeless Then Me.Hide
  
  If numNames < cmd.Count Then
    While cmd.Count - 1 >= numNames And cmd.Count > 0
      Unload cmd(cmd.Count - 1)
    Wend
  Else
    While cmd.Count < numNames
      Load cmd(cmd.Count)
    Wend
  End If
  For butNum = 0 To cmd.Count - 1
    lCaption = ButtonName(butNum)
    Do 'set default and cancel from leading chars in button name
      If Left(lCaption, 1) = "-" Then
        cmd(butNum).Cancel = True
        lCaption = Right(lCaption, Len(lCaption) - 1)
      ElseIf Left(lCaption, 1) = "+" Then
        cmd(butNum).default = True
        lCaption = Right(lCaption, Len(lCaption) - 1)
      Else
        Exit Do
      End If
    Loop
    cmd(butNum).Caption = lCaption
  Next butNum
  ResizeForm
  chkShow.Visible = ShowChk
  Me.Show ModalOrNot
  Me.ZOrder
  If ModalOrNot = vbModeless Then Me.SetFocus
  LostFocusButton = ButtonOnLostFocus
  DoEvents
  Exit Sub
  
MakeModal:
  ModalOrNot = vbModal
  GoTo MadeModal
End Sub

'Public Function WaitButtonPressNumber() As Long
'  Dim fgwin&
'  If ModalOrNot = vbModeless Then Me.SetFocus
'  While ButtonPressed < 0
'    DoEvents
'    If LostFocusButton > -1 Then
'      fgwin = GetFocus()
'      If fgwin = 0 Then cmd_Click LostFocusButton
'    End If
'  Wend
'  WaitButtonPressNumber = ButtonPressed + 1
'End Function
'
'Public Function WaitButtonPressLabel() As String
'  WaitButtonPressLabel = cmd(WaitButtonPressNumber - 1).Caption
'End Function

Private Sub chkShow_Click()
End Sub

Private Sub cmd_Click(Index As Integer)
  ButtonPressed = Index
  If ShowChk Then
    If chkShow.Value = 0 Then
      SaveSetting pRegApp, pRegSection, pRegKey, Index + 1
    End If
  End If
  RaiseEvent ButtonPress(lblMessage.Caption, Me.Caption, (Index + 1), cmd(Index).Caption)
  Unload Me
End Sub

Private Sub cmd_GotFocus(Index As Integer)
  Debug.Print "cmd (" & Index & ") got focus"
End Sub

Private Sub cmd_LostFocus(Index As Integer)
  Debug.Print "cmd (" & Index & ") lost focus"
End Sub

Private Sub Form_Deactivate()
  Debug.Print "Form_Deactivate"
  'If LostFocusButton > -1 Then cmd_Click LostFocusButton
End Sub

Private Sub Form_GotFocus()
  Debug.Print "Form_GotFocus"
End Sub

Private Sub Form_KeyDown(KeyCode As Integer, Shift As Integer)
  Debug.Print "Form_KeyDown " & KeyCode
End Sub

Private Sub Form_KeyPress(KeyAscii As Integer)
  Debug.Print "Form_KeyPress " & KeyAscii
End Sub

Private Sub Form_Load()
  ButtonPressed = -1
  LostFocusButton = -1
  Me.Caption = ""
  lblMessage.Caption = ""
End Sub

Private Sub ResizeForm()
  Dim butNum&, butWidth&, nextButLeft&, butTop&
  Dim lblWidth&, lblheight&, captWidth&
  
  captWidth = Me.TextWidth(Me.Caption)
  
  lblWidth = lblMessage.Width + 2 * ButSpacing
  lblheight = lblMessage.Height
  If ShowChk Then
    chkShow.Top = lblheight + 528
    lblheight = lblheight + chkShow.Height * 2
    If (chkShow.Width + chkShow.Left) > captWidth Then captWidth = (chkShow.Width + chkShow.Left)
  End If
  captWidth = captWidth + 10 * ButSpacing
  butTop = lblheight + 528
  Height = lblheight + 1344
  nextButLeft = ButSpacing
  For butNum = 0 To cmd.Count - 1
    butWidth = TextWidth(cmd(butNum).Caption) + 220
    If butWidth < MinButWidth Then butWidth = MinButWidth
    cmd(butNum).Width = butWidth
    cmd(butNum).Left = nextButLeft
    cmd(butNum).Top = butTop
    cmd(butNum).Visible = True
    nextButLeft = nextButLeft + butWidth + ButSpacing
  Next butNum
  nextButLeft = nextButLeft + ButSpacing
  If nextButLeft > lblWidth And nextButLeft > captWidth Then
    Width = nextButLeft
  Else
    If captWidth > lblWidth Then lblWidth = captWidth
    Width = lblWidth
    nextButLeft = (Width - nextButLeft) / 2 + ButSpacing
    For butNum = 0 To cmd.Count - 1
      cmd(butNum).Left = nextButLeft
      nextButLeft = nextButLeft + cmd(butNum).Width + ButSpacing
    Next butNum
  End If
End Sub

Private Sub Form_LostFocus()
  Debug.Print "Form_LostFocus"
End Sub

Private Sub Form_QueryUnload(Cancel As Integer, UnloadMode As Integer)
  If ButtonPressed < 0 Then
    If LostFocusButton >= 0 Then ButtonPressed = LostFocusButton Else ButtonPressed = 0
    RaiseEvent ButtonPress("", "", ButtonPressed + 1, "")
  End If
  DoEvents
End Sub

