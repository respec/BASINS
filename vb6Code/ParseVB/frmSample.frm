VERSION 5.00
Object = "{3B7C8863-D78F-101B-B9B5-04021C009402}#1.2#0"; "RICHTX32.OCX"
Begin VB.Form frmSample 
   Caption         =   "Form1"
   ClientHeight    =   2496
   ClientLeft      =   48
   ClientTop       =   312
   ClientWidth     =   3588
   LinkTopic       =   "Form1"
   ScaleHeight     =   2496
   ScaleWidth      =   3588
   StartUpPosition =   3  'Windows Default
   Begin RichTextLib.RichTextBox txt 
      Height          =   2292
      Left            =   0
      TabIndex        =   1
      Top             =   0
      Width           =   3372
      _ExtentX        =   5948
      _ExtentY        =   4043
      _Version        =   393217
      Enabled         =   -1  'True
      ScrollBars      =   2
      TextRTF         =   $"frmSample.frx":0000
   End
   Begin VB.PictureBox img 
      AutoSize        =   -1  'True
      Height          =   2052
      Left            =   0
      ScaleHeight     =   2004
      ScaleWidth      =   3084
      TabIndex        =   0
      Top             =   0
      Width           =   3132
   End
End
Attribute VB_Name = "frmSample"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Public Sub SetImage(filename$)
  Me.caption = filename
  img.Picture = LoadPicture(filename)
  img.Visible = True
  txt.Visible = False
  Me.Show
  frmMain.SetFocus
  'Me.Width = img.Width
  'Me.Height = img.Height
End Sub

Public Sub SetText(fullpath$)
  Dim dotpos&, filename$, ext$
  filename = FileNameOnly(fullpath)
  Me.caption = filename
  txt.Visible = True
  img.Visible = False
  dotpos = InStrRev(fullpath, ".")
  If dotpos > 0 Then ext = Mid(fullpath, dotpos) Else ext = ""
  frmMain.LoadTextboxFromFile PathNameOnly(fullpath), filename, ext, txt
  Me.Show
  frmMain.SetFocus
End Sub

Private Sub Form_Resize()
  If Width > 110 And Height > 375 Then
    img.Width = Width - 108
    txt.Width = Width - 108
    txt.Height = Height - 372
    img.Height = Height - 372
  End If
End Sub
