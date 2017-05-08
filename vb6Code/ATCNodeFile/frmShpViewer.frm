VERSION 5.00
Object = "{F9043C88-F6F2-101A-A3C9-08002B2F49FB}#1.2#0"; "COMDLG32.OCX"
Begin VB.Form frmShpViewer 
   Caption         =   "Shape File Viewer"
   ClientHeight    =   7992
   ClientLeft      =   48
   ClientTop       =   276
   ClientWidth     =   9024
   LinkTopic       =   "Form1"
   ScaleHeight     =   7992
   ScaleWidth      =   9024
   StartUpPosition =   3  'Windows Default
   Begin VB.TextBox txtNshapes 
      Height          =   288
      Left            =   7560
      TabIndex        =   22
      Text            =   "txtNshapes"
      Top             =   3720
      Width           =   1212
   End
   Begin VB.TextBox txtWholeFile 
      BeginProperty Font 
         Name            =   "Courier New"
         Size            =   10.2
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   2772
      Index           =   2
      Left            =   120
      MultiLine       =   -1  'True
      ScrollBars      =   3  'Both
      TabIndex        =   21
      Text            =   "frmShpViewer.frx":0000
      Top             =   5160
      Width           =   8772
   End
   Begin VB.CommandButton cmdOpen 
      Caption         =   "&Open"
      Height          =   372
      Index           =   2
      Left            =   7800
      TabIndex        =   20
      Top             =   4680
      Width           =   1092
   End
   Begin VB.CommandButton cmdOpen 
      Caption         =   "&Open"
      Height          =   372
      Index           =   1
      Left            =   7800
      TabIndex        =   19
      Top             =   3000
      Width           =   1092
   End
   Begin MSComDlg.CommonDialog cdlg 
      Left            =   240
      Top             =   4680
      _ExtentX        =   677
      _ExtentY        =   677
      _Version        =   393216
   End
   Begin VB.TextBox txtActualLength 
      Height          =   288
      Left            =   6120
      TabIndex        =   18
      Text            =   "txtActualLength"
      Top             =   3360
      Width           =   1212
   End
   Begin VB.TextBox txtYmax 
      Height          =   288
      Left            =   1080
      TabIndex        =   16
      Text            =   "txtYmax"
      Top             =   4080
      Width           =   1212
   End
   Begin VB.TextBox txtYmin 
      Height          =   288
      Left            =   1080
      TabIndex        =   14
      Text            =   "txtYmin"
      Top             =   3720
      Width           =   1212
   End
   Begin VB.TextBox txtXmax 
      Height          =   288
      Left            =   1080
      TabIndex        =   12
      Text            =   "txtXmax"
      Top             =   3360
      Width           =   1212
   End
   Begin VB.TextBox txtXmin 
      Height          =   288
      Left            =   1080
      TabIndex        =   10
      Text            =   "txtXmin"
      Top             =   3000
      Width           =   1212
   End
   Begin VB.TextBox txtShapeType 
      Height          =   288
      Left            =   4800
      TabIndex        =   7
      Text            =   "txtShapeType"
      Top             =   4080
      Width           =   1212
   End
   Begin VB.TextBox txtFileVersion 
      Height          =   288
      Left            =   4800
      TabIndex        =   5
      Text            =   "txtFileVersion"
      Top             =   3720
      Width           =   1212
   End
   Begin VB.TextBox txtFileLength 
      Height          =   288
      Left            =   4800
      TabIndex        =   3
      Text            =   "txtFileLength"
      Top             =   3360
      Width           =   1212
   End
   Begin VB.TextBox txtFileCode 
      Height          =   288
      Left            =   4800
      TabIndex        =   1
      Text            =   "txtFileCode"
      Top             =   3000
      Width           =   1212
   End
   Begin VB.TextBox txtWholeFile 
      BeginProperty Font 
         Name            =   "Courier New"
         Size            =   10.2
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   2772
      Index           =   1
      Left            =   120
      MultiLine       =   -1  'True
      ScrollBars      =   3  'Both
      TabIndex        =   0
      Text            =   "frmShpViewer.frx":000D
      Top             =   120
      Width           =   8772
   End
   Begin VB.Label Label10 
      Alignment       =   1  'Right Justify
      Caption         =   "Number of shapes"
      Height          =   252
      Left            =   6120
      TabIndex        =   23
      Top             =   3756
      Width           =   1332
   End
   Begin VB.Label Label9 
      Caption         =   "Ymax"
      Height          =   252
      Left            =   240
      TabIndex        =   17
      Top             =   4116
      Width           =   732
   End
   Begin VB.Label Label8 
      Caption         =   "Ymin"
      Height          =   252
      Left            =   240
      TabIndex        =   15
      Top             =   3756
      Width           =   732
   End
   Begin VB.Label Label7 
      Caption         =   "Xmax"
      Height          =   252
      Left            =   240
      TabIndex        =   13
      Top             =   3396
      Width           =   732
   End
   Begin VB.Label Label6 
      Caption         =   "Xmin"
      Height          =   252
      Left            =   240
      TabIndex        =   11
      Top             =   3036
      Width           =   732
   End
   Begin VB.Label Label5 
      Caption         =   "0=null, 1=point, 3=arc, 5=polygon, 8=multipoint"
      Height          =   252
      Left            =   2520
      TabIndex        =   9
      Top             =   4440
      Width           =   3492
   End
   Begin VB.Label Label4 
      Caption         =   "Shape Type"
      Height          =   252
      Left            =   2520
      TabIndex        =   8
      Top             =   4116
      Width           =   2172
   End
   Begin VB.Label Label3 
      Caption         =   "File Version (expect 1000)"
      Height          =   252
      Left            =   2520
      TabIndex        =   6
      Top             =   3756
      Width           =   2172
   End
   Begin VB.Label Label2 
      Caption         =   "File Length"
      Height          =   252
      Left            =   2520
      TabIndex        =   4
      Top             =   3396
      Width           =   2172
   End
   Begin VB.Label Label1 
      Caption         =   "File Code (expect 9994)"
      Height          =   252
      Left            =   2520
      TabIndex        =   2
      Top             =   3036
      Width           =   2172
   End
End
Attribute VB_Name = "frmShpViewer"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Private pFile1Bytes() As Byte
Private pFile2Bytes() As Byte
Private hexchar(256) As String  '2-character hexadecimal to print for each byte value
Private pFilename(2) As String

Private Sub cmdOpen_MouseUp(Index As Integer, Button As Integer, Shift As Integer, X As Single, Y As Single)
  If Not Shift And 1 Then
    cdlg.ShowOpen
    pFilename(Index) = cdlg.Filename
    If Index = 1 Then
      pFile1Bytes = WholeFileBytes(pFilename(Index))
      PopulateHex txtWholeFile(1), pFile1Bytes
    Else
      pFile2Bytes = WholeFileBytes(pFilename(Index))
      PopulateHex txtWholeFile(2), pFile2Bytes
    End If
  End If
  PopulateAll pFilename(Index)
End Sub

Private Sub Form_Load()
  Dim ch$(16), i&
  For i = 0 To 9
    ch(i) = Chr(48 + i)        'Debug.Print i, ch(i)
  Next i
  For i = 97 To 102 '65 to 70 for upper case
    ch(i - 87) = Chr(i)        'Debug.Print i - 87, ch(i - 87)
  Next i
  For i = 0 To 255
    hexchar(i) = ch((i And 240) / 16) & ch(i And 15)        'Debug.Print i, hexchar(i)
  Next i
End Sub


Private Sub PopulateAll(Filename As String)
  Dim shxFilename As String
  Dim FileLength&, ShapeType&
  Dim FileLengthL&
  Dim lowX#, lowY#, uppX#, uppY#
  Dim version As Long
  Dim inFile As Integer
  
  inFile = FreeFile
  
  Open Filename For Binary As inFile
  
  ReadShapeHeader inFile, FileLength, ShapeType, lowX, lowY, uppX, uppY
  txtXmin.Text = lowX
  txtXmax.Text = uppX
  txtYmin.Text = lowY
  txtYmax.Text = uppY
  txtFileLength.Text = FileLength
  txtActualLength.Text = LOF(inFile) / 2
  txtShapeType.Text = ShapeType
  
  Seek #inFile, 1
  txtFileCode.Text = ReadBigInt(inFile)
  Seek #inFile, 29
  Get #inFile, , version
  txtFileVersion.Text = version
  Close #inFile

  shxFilename = Left(Filename, Len(Filename) - 1) & "x"
  If Len(Dir(shxFilename)) > 0 Then
    txtNshapes.Text = (FileLen(shxFilename) - 100) / 8
  Else
    txtNshapes.Text = "no shx"
  End If

End Sub

Private Sub PopulateHex(txtbox As TextBox, pFileBytes() As Byte)
  Dim hexText As String
  Dim b As Long, bytesCount As Long, wordsCount As Long
  txtbox.Text = ""
  
  For b = 1 To 49 Step 2
    hexText = hexText & Format(b, "00") & Format(b + 1, "00") & " "
  Next
  hexText = hexText & vbCrLf
  For b = 1 To 49 Step 2
    hexText = hexText & "---- "
  Next
  hexText = hexText & vbCrLf
  For b = LBound(pFileBytes) To UBound(pFileBytes)
    hexText = hexText & hexchar(pFileBytes(b))
    bytesCount = bytesCount + 1
    If bytesCount > 1 Then
      hexText = hexText & " "
      bytesCount = 0
      wordsCount = wordsCount + 1
      If wordsCount > 24 Then
        hexText = hexText & vbCrLf
        wordsCount = 0
      End If
    End If
  Next
  txtbox.Text = Left(hexText, 20000)
End Sub

Private Sub Form_Resize()
  If Me.ScaleWidth > 252 Then
    txtWholeFile(1).Width = Me.ScaleWidth - 252
    txtWholeFile(2).Width = txtWholeFile(1).Width
  End If
End Sub
