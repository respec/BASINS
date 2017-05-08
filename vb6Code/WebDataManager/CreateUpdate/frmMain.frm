VERSION 5.00
Object = "{F9043C88-F6F2-101A-A3C9-08002B2F49FB}#1.2#0"; "COMDLG32.OCX"
Begin VB.Form frmMain 
   Caption         =   "BASINS Update"
   ClientHeight    =   1275
   ClientLeft      =   60
   ClientTop       =   345
   ClientWidth     =   3495
   Icon            =   "frmMain.frx":0000
   LinkTopic       =   "Form1"
   ScaleHeight     =   1275
   ScaleWidth      =   3495
   StartUpPosition =   3  'Windows Default
   Begin VB.CommandButton cmdAdd 
      Caption         =   "Add/Update Component in Test"
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
      Left            =   120
      TabIndex        =   1
      Top             =   120
      Width           =   3255
   End
   Begin VB.CommandButton cmdRelease 
      Caption         =   "Copy Test to Release"
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
      Left            =   480
      TabIndex        =   0
      Top             =   720
      Width           =   2535
   End
   Begin MSComDlg.CommonDialog cdlg 
      Left            =   0
      Top             =   0
      _ExtentX        =   847
      _ExtentY        =   847
      _Version        =   393216
   End
End
Attribute VB_Name = "frmMain"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Private Sub cmdAdd_Click()
  Me.Hide
  frmUpdates.Show
End Sub

Private Sub cmdRelease_Click()
  Dim testComponentsDir As String
  Dim releaseComponentsDir As String
  
  If FileExists(ReleaseFile & ".bak") Then Kill ReleaseFile & ".bak"
  Name ReleaseFile As ReleaseFile & ".bak"
  FileCopy TestFile, ReleaseFile
  
  PathNameOnly (pXMLfilename) & "\components\" & FilenameNoPath(pFilenameToUpload)
  If FileExists(pXMLfilename & ".bak") Then Kill pXMLfilename & ".bak"
  Name pXMLfilename As pXMLfilename & ".bak"
  SaveFileString pXMLfilename, ReplaceString(AvailableUpdates.xml, "/test/", "/")
  
  testComponentsDir = PathNameOnly(TestFile) & "\components"
  releaseComponentsDir = PathNameOnly(ReleaseFile) & "\components"
  
  OpenFile PathNameOnly(ReleaseFile) & "\", cdlg
  OpenFile PathNameOnly(TestFile) & "\", cdlg
  
  Logger.LogMsg "Now manually copy the folder " _
    & vbCr & testComponentsDir & vbCr & " to " _
    & vbCr & releaseComponentsDir & "'", "Update"
  
End Sub

Private Sub Form_Load()
  Set Logger = New clsATCoLogger
  Logger.Log2Debug = True
End Sub
