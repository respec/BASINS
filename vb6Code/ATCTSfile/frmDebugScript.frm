VERSION 5.00
Begin VB.Form frmDebugScript 
   Caption         =   "Debug Script"
   ClientHeight    =   4068
   ClientLeft      =   48
   ClientTop       =   276
   ClientWidth     =   9384
   LinkTopic       =   "Form1"
   ScaleHeight     =   4068
   ScaleWidth      =   9384
   StartUpPosition =   3  'Windows Default
   Begin VB.Frame fraButtons 
      BorderStyle     =   0  'None
      Caption         =   "Frame1"
      Height          =   1092
      Left            =   2040
      TabIndex        =   4
      Top             =   2880
      Width           =   5532
      Begin VB.CommandButton cmdStep 
         Caption         =   "&Step"
         Height          =   372
         Left            =   0
         TabIndex        =   5
         Top             =   720
         Width           =   1212
      End
      Begin VB.CommandButton cmdRun 
         Caption         =   "&Run"
         CausesValidation=   0   'False
         Height          =   372
         Left            =   2880
         TabIndex        =   7
         ToolTipText     =   "Read the rest of the file without stopping"
         Top             =   720
         Width           =   1212
      End
      Begin VB.CommandButton cmdQuit 
         Cancel          =   -1  'True
         Caption         =   "&Quit"
         Height          =   372
         Left            =   4320
         TabIndex        =   8
         ToolTipText     =   "Stop reading and discard any data that has been read"
         Top             =   720
         Width           =   1212
      End
      Begin VB.CommandButton cmdNext 
         Caption         =   "&Next Line"
         Default         =   -1  'True
         Height          =   372
         Left            =   1440
         TabIndex        =   6
         ToolTipText     =   "Step until the current line is finished"
         Top             =   720
         Width           =   1212
      End
      Begin VB.Label lblDate 
         Caption         =   "Date"
         Height          =   252
         Left            =   0
         TabIndex        =   10
         Top             =   0
         Width           =   5532
      End
      Begin VB.Label lblValue 
         Caption         =   "Value"
         Height          =   252
         Left            =   0
         TabIndex        =   9
         Top             =   360
         Width           =   5532
      End
   End
   Begin VB.ListBox lstCurrentScript 
      Height          =   1392
      Left            =   2040
      TabIndex        =   3
      Top             =   1440
      Width           =   7212
   End
   Begin VB.TextBox txtCurrentLine 
      Height          =   1332
      HideSelection   =   0   'False
      Left            =   2040
      MultiLine       =   -1  'True
      ScrollBars      =   2  'Vertical
      TabIndex        =   1
      Top             =   0
      Width           =   7212
   End
   Begin VB.Label lblCurProgramLine 
      Caption         =   "Current Script Line:"
      Height          =   252
      Left            =   120
      TabIndex        =   2
      Top             =   1440
      Width           =   1452
   End
   Begin VB.Label lblCurrentLine 
      Caption         =   "Current Input Line #"
      Height          =   252
      Left            =   120
      TabIndex        =   0
      Top             =   120
      Width           =   1812
   End
End
Attribute VB_Name = "frmDebugScript"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2002 by AQUA TERRA Consultants
Private ButtonPressed As String

Public Sub ShowScript(asc As clsATCscriptExpression)
  Dim str As String, delimPos As Long, LineNum As Long, lastdelim As Long
  ScriptAssigningLineNumbers = True
  str = asc.Printable
  ScriptAssigningLineNumbers = False
  Caption = "Debug Script " & asc.SubExpression(1).Printable
  txtCurrentLine.Text = ""
  lblCurrentLine.Caption = "Current Input Line "
  LineNum = 0
  lastdelim = 1
  lstCurrentScript.Clear
  delimPos = InStr(lastdelim, str, PrintEOL)
  While delimPos > 0
    lstCurrentScript.AddItem Mid(str, lastdelim, delimPos - lastdelim)
    lastdelim = delimPos + Len(PrintEOL)
    delimPos = InStr(lastdelim, str, PrintEOL)
  Wend
  'Me.Show 'FIXME trouble with vbModal GenScn vs. WDMUtil
End Sub

Public Sub NextLine()
  txtCurrentLine.Text = CurrentLine
  lblCurrentLine = "Current Input Line # " & CurrentLineNum
End Sub

Public Sub NewDate(index As Long, jdy As Double)
  lblDate = DumpDate(jdy, "Date(" & index & ")")
End Sub

Public Sub NewValue(index As Long, val As Single)
  lblValue = "Value(" & index & "): " & val
End Sub

Public Sub EvalExpression(asc As clsATCscriptExpression)
  Static PreviousLine As Long
  lstCurrentScript.ListIndex = asc.Line - 1
  'txtCurrentScript.Text = asc.Printable
  If ButtonPressed = "Next" And PreviousLine = asc.Line Then
    'Don't step through all the subexpressions on this line
  ElseIf ButtonPressed <> "Run" And ButtonPressed <> "Quit" Then
    ButtonPressed = ""
    While ButtonPressed = ""
      DoEvents
    Wend
    PreviousLine = asc.Line
  End If
End Sub

Private Sub cmdNext_Click()
  ButtonPressed = "Next"
End Sub

Private Sub cmdQuit_Click()
  ButtonPressed = "Quit"
  AbortScript = True
End Sub

Private Sub cmdRun_Click()
  ButtonPressed = "Run"
  DebuggingScript = False
  Me.Hide
End Sub

Private Sub cmdStep_Click()
  ButtonPressed = "Step"
End Sub

Private Sub cmdStep_KeyDown(KeyCode As Integer, Shift As Integer)
  Form_KeyDown KeyCode, Shift
End Sub

Private Sub Form_KeyDown(KeyCode As Integer, Shift As Integer)
  Select Case KeyCode
    Case vbKeyF8: ButtonPressed = "Step"
    Case vbKeyF5: ButtonPressed = "Run"
  End Select
End Sub

Private Sub Form_Resize()
  Dim newWidth As Single, newHeight As Single
  newWidth = Me.ScaleWidth - 2172
  If newWidth > 100 Then
    txtCurrentLine.Width = newWidth
    lstCurrentScript.Width = newWidth
    fraButtons.Top = Me.ScaleHeight - fraButtons.Height - 108
    newHeight = (fraButtons.Top - 276) / 2
    If newHeight > 100 Then
      txtCurrentLine.Height = newHeight
      lstCurrentScript.Height = newHeight
      lstCurrentScript.Top = newHeight + 78
      lblCurProgramLine.Top = lstCurrentScript.Top
    End If
  End If
End Sub
