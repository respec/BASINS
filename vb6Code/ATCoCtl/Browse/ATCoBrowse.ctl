VERSION 5.00
Begin VB.UserControl ATCoBrowse 
   ClientHeight    =   2940
   ClientLeft      =   0
   ClientTop       =   0
   ClientWidth     =   5415
   ScaleHeight     =   2940
   ScaleWidth      =   5415
   ToolboxBitmap   =   "ATCoBrowse.ctx":0000
   Begin VB.Frame sash 
      BorderStyle     =   0  'None
      Caption         =   "Frame1"
      Height          =   2532
      Left            =   2160
      MousePointer    =   9  'Size W E
      TabIndex        =   4
      Top             =   0
      Width           =   132
   End
   Begin VB.FileListBox FileBox 
      Height          =   1845
      Left            =   2280
      TabIndex        =   3
      Top             =   336
      Width           =   3132
   End
   Begin VB.DirListBox DirBox 
      Height          =   2016
      Left            =   0
      TabIndex        =   2
      Top             =   360
      Width           =   2172
   End
   Begin VB.DriveListBox DriveBox 
      Height          =   288
      Left            =   0
      TabIndex        =   1
      Top             =   0
      Width           =   2172
   End
   Begin VB.TextBox txtFilename 
      Height          =   288
      Left            =   0
      TabIndex        =   0
      Text            =   "filename"
      Top             =   2640
      Width           =   5412
   End
   Begin VB.Label lblFilename 
      Caption         =   "File Name"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   252
      Left            =   0
      TabIndex        =   6
      Top             =   2400
      Width           =   1452
   End
   Begin VB.Label lblPattern 
      Caption         =   "Files Matching"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   252
      Left            =   2300
      TabIndex        =   5
      Top             =   60
      Width           =   2892
   End
End
Attribute VB_Name = "ATCoBrowse"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'Copyright 2001 by AQUA TERRA Consultants
Private pSashRatio As Single
Private pSashDragging As Boolean
Private pDirectoryOnly As Boolean

Private Sub DriveBox_Change()
  If Len(DriveBox.Drive) > 0 Then DirBox.path = Left(DriveBox.Drive, 1) & ":\"
End Sub

Private Sub DirBox_Change()
  Debug.Print "DirBox_Change"
  If pDirectoryOnly Then
    txtFilename.Text = DirBox.path
  Else
    FileBox.path = DirBox.path
    Dim tmpFilename As String
    tmpFilename = DirBox.path
    If Right(tmpFilename, 1) <> "\" Then tmpFilename = tmpFilename & "\"
    txtFilename.Text = tmpFilename & FilenameNoPath(txtFilename.Text)
  End If
End Sub

Private Sub SetFilenameFromControls()
End Sub

Private Sub FileBox_Click()
  Dim tmpFilename As String
  tmpFilename = DirBox.path
  If Right(tmpFilename, 1) <> "\" Then tmpFilename = tmpFilename & "\"
  txtFilename.Text = tmpFilename & FileBox.Filename
End Sub

Private Sub sash_MouseDown(Button As Integer, Shift As Integer, X As Single, Y As Single)
  If Not pDirectoryOnly Then pSashDragging = True
End Sub

Private Sub sash_MouseMove(Button As Integer, Shift As Integer, X As Single, Y As Single)
  If pSashDragging Then
    SashRatio = X - (Width - sash.Width)
    ResizeNow
  End If
End Sub

Private Sub sash_MouseUp(Button As Integer, Shift As Integer, X As Single, Y As Single)
  pSashDragging = False
End Sub

Private Sub UserControl_Initialize()
  DriveBox.Drive = CurDir
  DirBox.path = CurDir
  Pattern = "*.*"
  pSashRatio = 0.5
End Sub

Private Sub UserControl_Resize()
  ResizeNow
End Sub

Private Sub ResizeNow()
  If Height > 1000 And Width > 2000 Then
    If pDirectoryOnly Then SashRatio = 1
    txtFilename.Top = Height - txtFilename.Height
    txtFilename.Width = Width
    lblFilename.Top = txtFilename.Top - 240
    
    FileBox.Height = Height - 750
    sash.Height = FileBox.Height + FileBox.Top
    DirBox.Height = FileBox.Height - 168
    
    DirBox.Width = (Width - sash.Width) * SashRatio
    DriveBox.Width = DirBox.Width
    FileBox.Width = Width - sash.Width - DirBox.Width
    sash.Left = DirBox.Width
    FileBox.Left = sash.Left + sash.Width
    lblPattern.Left = FileBox.Left + 20
  End If
End Sub

Public Property Get SashRatio() As Single
  SashRatio = pSashRatio
End Property

Public Property Let SashRatio(ByVal NewValue As Single)
  If NewValue < 0 Then
    pSashRatio = 0
  ElseIf NewValue > 1 Then
    If SashRatio <= 100 Then
      pSashRatio = NewValue / 100
    Else
      pSashRatio = 1
    End If
  Else
    pSashRatio = NewValue
  End If
  ResizeNow
End Property

Public Property Get Filename() As String
  Dim retval As String
  Debug.Print "Get Filename"
  retval = txtFilename.Text
  If InStr(retval, "\") = 0 Then retval = DirBox.path & "\" & retval
  If InStr(retval, ":") = 0 Then retval = Left(DriveBox.Drive, 1) & ":" & retval
  Filename = retval
End Property
Public Property Let Filename(ByVal NewValue As String)
  Dim newPath As String
  Debug.Print "Let Filename = " & NewValue
  On Error Resume Next
  newPath = PathNameOnly(NewValue)
  If Len(newPath) > 0 Then
    If Mid(newPath, 2, 1) = ":" Then DriveBox.Drive = Left(newPath, 1)
    DirBox.path = newPath
    FileBox.path = newPath
  End If
  'If Len(Dir(NewValue)) > 0 Then
  FileBox.Filename = NewValue
  'If Len(Dir(NewValue, vbNormal)) = 0 Then
  '  If Right(NewValue, 1) <> "\" Then
  '    NewValue = NewValue & "\"
  '  End If
  'End If
  txtFilename.Text = NewValue
End Property

' example: browse.Pattern = "*.exe; *.bat"
Public Property Get Pattern() As String
  Pattern = FileBox.Pattern
End Property
Public Property Let Pattern(ByVal NewValue As String)
  FileBox.Pattern = NewValue
  lblPattern.Caption = "Files Matching " & NewValue
End Property

Public Property Get DirectoryOnly() As Boolean
  DirectoryOnly = pDirectoryOnly
End Property
Public Property Let DirectoryOnly(ByVal NewValue As Boolean)
  pDirectoryOnly = NewValue
  If Not pDirectoryOnly Then pSashRatio = 0.5
  ResizeNow
End Property

