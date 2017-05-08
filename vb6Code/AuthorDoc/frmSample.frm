VERSION 5.00
Object = "{F9043C88-F6F2-101A-A3C9-08002B2F49FB}#1.2#0"; "COMDLG32.OCX"
Object = "{3B7C8863-D78F-101B-B9B5-04021C009402}#1.2#0"; "RICHTX32.OCX"
Begin VB.Form frmSample 
   Caption         =   "Sample"
   ClientHeight    =   2700
   ClientLeft      =   45
   ClientTop       =   315
   ClientWidth     =   3495
   Icon            =   "frmSample.frx":0000
   LinkTopic       =   "Form1"
   ScaleHeight     =   180
   ScaleMode       =   3  'Pixel
   ScaleWidth      =   233
   StartUpPosition =   3  'Windows Default
   Begin MSComDlg.CommonDialog cdlg 
      Left            =   0
      Top             =   2280
      _ExtentX        =   688
      _ExtentY        =   688
      _Version        =   393216
   End
   Begin VB.PictureBox img 
      AutoRedraw      =   -1  'True
      AutoSize        =   -1  'True
      BackColor       =   &H00FFFFFF&
      BeginProperty Font 
         Name            =   "Courier New"
         Size            =   9
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   2532
      Left            =   0
      ScaleHeight     =   165
      ScaleMode       =   3  'Pixel
      ScaleWidth      =   205
      TabIndex        =   0
      Top             =   0
      Width           =   3132
   End
   Begin RichTextLib.RichTextBox txt 
      Height          =   2292
      Left            =   0
      TabIndex        =   1
      Top             =   0
      Width           =   3372
      _ExtentX        =   5953
      _ExtentY        =   4048
      _Version        =   393217
      ScrollBars      =   2
      TextRTF         =   $"frmSample.frx":0442
   End
End
Attribute VB_Name = "frmSample"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants

Private Declare Function GetModuleUsage Lib "Kernel" (ByVal hModule As Integer) As Integer
Private Const SW_HIDE = 0      'Hidden Window
Private Const SW_NORMAL = 1    'Normal Window
Private Const SW_MAXIMIZE = 3  'Maximized Window
Private Const SW_MINIMIZE = 6  'Minimized Window

Public Sub SetImage(Filename$)

  Dim tempFilename As String
  Dim cmdline As String
  
  Me.caption = Filename
  Select Case UCase(Right(Filename, 3))
    Case "BMP", "GIF"
      img.Picture = LoadPicture(Filename)
    Case Else
      tempFilename = GetTmpPath & FilenameOnly(Filename) & ".bmp"
      ' -D = delete original, -quiet = no output, -o = output filename
      cmdline = "-o """ & tempFilename & """ -out bmp """ & Filename & """"
      RunNconvert cmdline
      On Error GoTo ErrLoad
      img.Picture = LoadPicture(tempFilename)
      Kill tempFilename
  End Select
  img.Visible = True
  txt.Visible = False
  frmMain.Visible = True
  Me.Show
  Me.Left = frmMain.Left + frmMain.Width
  frmMain.SetFocus
  'Me.Width = img.Width
  'Me.Height = img.Height
  Exit Sub
  
ErrLoad:
  Debug.Print Err.Description
  Resume Next
End Sub

Public Sub SetText(fullpath$)
  Dim dotpos&, Filename$, ext$
  Filename = FilenameOnly(fullpath)
  Me.caption = Filename
  txt.Visible = True
  img.Visible = False
  dotpos = InStrRev(fullpath, ".")
  If dotpos > 0 Then ext = Mid(fullpath, dotpos) Else ext = ""
  frmMain.LoadTextboxFromFile PathNameOnly(fullpath), Filename, ext, txt
  Me.Show
  frmMain.SetFocus
End Sub

Private Sub Form_Resize()
  If ScaleWidth > 112 And Height > 375 Then
    img.Width = ScaleWidth
    img.Height = ScaleHeight
    txt.Width = Width - 108
    txt.Height = Height - 372
  End If
End Sub
