VERSION 5.00
Begin VB.Form frmTextBox 
   ClientHeight    =   4008
   ClientLeft      =   60
   ClientTop       =   348
   ClientWidth     =   7728
   LinkTopic       =   "Form1"
   ScaleHeight     =   4008
   ScaleWidth      =   7728
   Begin MSComDlg.CommonDialog cdlg 
      Left            =   3840
      Top             =   0
      _ExtentX        =   677
      _ExtentY        =   677
      _Version        =   393216
   End
   Begin VB.CommandButton cmdClear 
      Caption         =   "Clear"
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
      Left            =   2280
      TabIndex        =   3
      Top             =   3480
      Width           =   975
   End
   Begin VB.CommandButton cmdLoad 
      Caption         =   "Load..."
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
      Left            =   120
      TabIndex        =   2
      Top             =   3480
      Width           =   975
   End
   Begin VB.CommandButton cmdSave 
      Caption         =   "Save..."
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
      Left            =   1200
      TabIndex        =   1
      Top             =   3480
      Width           =   975
   End
   Begin RichTextLib.RichTextBox txtBox 
      Height          =   3372
      Left            =   0
      TabIndex        =   0
      Top             =   0
      Width           =   6132
      _ExtentX        =   10816
      _ExtentY        =   5948
      _Version        =   393217
      Enabled         =   -1  'True
      ScrollBars      =   2
      TextRTF         =   $"frmTextBox.frx":0000
      BeginProperty Font {0BE35203-8F91-11CE-9DE3-00AA004BB851} 
         Name            =   "Courier New"
         Size            =   10.2
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
   End
End
Attribute VB_Name = "frmTextBox"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants

Public Sub clear()
  text = ""
End Sub

Private Sub cmdClear_Click()
  clear
End Sub

Private Sub cmdLoad_Click()
  cdlg.ShowOpen
  If cdlg.Filename <> "" Then text = WholeFileString(cdlg.Filename)
End Sub

Private Sub cmdSave_Click()
  cdlg.ShowSave
  If cdlg.Filename <> "" Then SaveFileString cdlg.Filename, text
End Sub

Private Sub Form_Activate()
 ' dlgOpenFile.InitDir = g_InputPath
End Sub

Private Sub Form_QueryUnload(Cancel As Integer, UnloadMode As Integer)
  'If user tried to close this form, just hide it rather than unloading it
  If UnloadMode = vbFormControlMenu Then
    Me.Hide
    Cancel = True
  End If
End Sub

Private Sub Form_Resize()
  If Width > 120 Then txtBox.Width = Width - 120
  If Height > 700 Then
    txtBox.Height = Height - cmdLoad.Height * 2 - cmdLoad.Left * 2
    cmdLoad.Top = txtBox.Top + txtBox.Height + cmdLoad.Left
    cmdSave.Top = cmdLoad.Top
    cmdClear.Top = cmdLoad.Top
  End If
End Sub

Public Property Get text() As String
  text = txtBox.text
End Property

Public Property Let text(newValue As String)
  txtBox.text = newValue
'  Dim rtf$, rtf2$
'  Dim nextch&, maxch&
'  Dim openParen&, closeParen&, parenlevel&, spacepos&
'  If newvalue <> txtBox.text Then
'    rtf = RTF_START
'    nextch = 1
'    maxch = Len(newvalue)
'    While nextch <= maxch
'      openParen = InStr(nextch, newvalue, "(")
'      If openParen >= nextch Then
'        spacepos = InStr(openParen, newvalue, " ")
'        If spacepos > openParen Then
'          While Mid(newvalue, openParen + 1, 1) = "("
'            openParen = openParen + 1
'          Wend
'          rtf = rtf & Mid(newvalue, nextch, openParen - nextch + 1) _
'            & RTF_BOLD & Mid(newvalue, openParen + 1, spacepos - openParen) & RTF_PLAIN
'          nextch = spacepos + 1
'        Else
'          rtf = rtf & Mid(newvalue, nextch)
'          nextch = maxch + 1
'        End If
'      Else
'        rtf = rtf & Mid(newvalue, nextch)
'        nextch = maxch + 1
'      End If
'    Wend
'    rtf = rtf & RTF_END
'    rtf2 = ""
'    txtBox.textRTF = ""
'    nextch = 1
'    maxch = Len(rtf)
'    While nextch <= maxch
'      closeParen = InStr(nextch, rtf, ")")
'      If closeParen >= nextch Then
'        rtf2 = rtf2 & Mid(rtf, nextch, closeParen - nextch + 1) & "\par "
'        nextch = closeParen + 1
'      Else
'        rtf2 = rtf2 & Mid(rtf, nextch)
'        nextch = maxch + 1
'      End If
'    Wend
'    txtBox.textRTF = rtf2
'  End If
End Property

Public Property Get textRTF() As String
  textRTF = txtBox.textRTF
End Property

Public Property Let textRTF(newValue As String)
  txtBox.textRTF = newValue
End Property

Private Sub txtBox_MouseDown(Button As Integer, Shift As Integer, x As Single, y As Single)
  If Button = vbRightButton Then
    txtBox.SelBold = True
'    Debug.Print txtBox.textRTF
  End If
End Sub

