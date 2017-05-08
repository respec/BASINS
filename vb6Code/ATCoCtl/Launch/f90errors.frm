VERSION 5.00
Object = "{BCB8F101-8780-11D1-90BC-00A024C11E04}#39.0#0"; "ATCoControls.ocx"
Begin VB.Form Form1 
   Caption         =   "Form1"
   ClientHeight    =   7140
   ClientLeft      =   45
   ClientTop       =   270
   ClientWidth     =   7815
   LinkTopic       =   "Form1"
   ScaleHeight     =   7140
   ScaleWidth      =   7815
   StartUpPosition =   3  'Windows Default
   Begin VB.ListBox List1 
      Height          =   6300
      ItemData        =   "Form1.frx":0000
      Left            =   1500
      List            =   "Form1.frx":0002
      TabIndex        =   3
      Top             =   480
      Width           =   6195
   End
   Begin VB.CommandButton Command1 
      Caption         =   "Test  All Errors"
      Height          =   252
      Index           =   1
      Left            =   120
      TabIndex        =   2
      Top             =   1620
      Width           =   1212
   End
   Begin VB.CommandButton Command1 
      Caption         =   "Test  1 Error"
      Height          =   252
      Index           =   0
      Left            =   120
      TabIndex        =   1
      Top             =   1200
      Width           =   1212
   End
   Begin AtCoControls.ATCoText ATCoText1 
      Height          =   312
      Left            =   120
      TabIndex        =   0
      Top             =   600
      Width           =   1092
      _ExtentX        =   1931
      _ExtentY        =   556
      HardMax         =   255
      HardMin         =   0
      SoftMax         =   255
      SoftMin         =   0
      MaxWidth        =   5
      DataType        =   1
      DefaultValue    =   "2"
      Value           =   "2"
      Enabled         =   -1  'True
   End
   Begin VB.Label lblThreadId 
      Caption         =   "Thread Id:"
      Height          =   252
      Left            =   2520
      TabIndex        =   6
      Top             =   120
      Width           =   2112
   End
   Begin VB.Label lblProcessId 
      Caption         =   "Process Id:"
      Height          =   252
      Left            =   4800
      TabIndex        =   5
      Top             =   120
      Width           =   2832
   End
   Begin VB.Label lblWindow 
      Caption         =   "Window Handle:"
      Height          =   252
      Left            =   120
      TabIndex        =   4
      Top             =   120
      Width           =   2172
   End
End
Attribute VB_Name = "Form1"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
Private Declare Function GetCurrentProcessId Lib "kernel32" () As Long
Private Declare Function GetCurrentThreadId Lib "kernel32" () As Long

Dim pipeHandle&

Private Sub Command1_Click(Index As Integer)

    Dim i&, j&, e&, s$, ef&, el&
    On Error GoTo ERRDESC
    
    List1.Clear
    
    If Index = 0 Then 'spec
      ef = CLng(ATCoText1.Value)
      el = ef
    Else 'all
      ef = 0
      el = 255
    End If
    
    For e = ef To el
    
      Dim written&, res&
      
      s = "(msg1 Processing " & e & ")"
      
      ReDim inbuf(Len(s)) As Byte
      inbuf() = s
      res = WriteFile(pipeHandle, inbuf(0), 2 * Len(s), written, 0)
    
      j = e
      s = ""
      Call EXCEPT(j, i)
      If Len(s) = 0 Then
        If i = 0 Then
          ' CStr(e) & ": No Error Test Implemented"
        ElseIf i = -2 Then
          s = CStr(e) & ": Error Caused DEATH in LF904.0e"
        ElseIf i = -1 Then
          s = CStr(e) & ": Error Not Fatal in LF90, Result: " & CStr(j)
        Else
          s = CStr(e) & ": Returns: " & CStr(i)
        End If
      End If
      If Len(s) > 0 Then
        List1.AddItem s
        s = "(msg2 " & s & ")"
        ReDim inbuf(Len(s)) As Byte
        inbuf() = s
        res = WriteFile(pipeHandle, inbuf(0), 2 * Len(s), written, 0)
      End If
    Next e
    
    Exit Sub
    
ERRDESC:
    s = CStr(e) & ": Returns: " & Err.Description
    Err.Clear
    Resume Next
End Sub

Private Sub Form_Load()

    lblThreadId.Caption = lblThreadId.Caption & "  " & GetCurrentThreadId()
    lblProcessId.Caption = lblProcessId.Caption & "  " & GetCurrentProcessId()
    lblWindow.Caption = lblWindow.Caption & "  " & hWnd

    pipeHandle = GetStdHandle(STD_OUTPUT_HANDLE)

End Sub
