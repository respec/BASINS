VERSION 5.00
Begin VB.Form frmMain 
   Caption         =   "Form1"
   ClientHeight    =   3195
   ClientLeft      =   60
   ClientTop       =   345
   ClientWidth     =   4680
   LinkTopic       =   "Form1"
   ScaleHeight     =   3195
   ScaleWidth      =   4680
   StartUpPosition =   3  'Windows Default
   Begin VB.CommandButton cmdOpenWDM 
      Caption         =   "Open WDM"
      Height          =   495
      Left            =   1680
      TabIndex        =   0
      Top             =   1800
      Width           =   1095
   End
End
Attribute VB_Name = "frmMain"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Private Sub cmdOpenWDM_Click()
  Dim iTser As Long
  Dim lTser As ATCclsTserData
  
  Dim wdmFile As ATCclsTserFile
  Dim gdbFile As ATCclsTserFile
  
  Set wdmFile = New clsTSerWDM
  Set gdbFile = New clsTSerGDB
  
  wdmFile.FileName = "d:\basins\data\ct.wdm"
  
  gdbFile.FileName = "whatever.mdb"
  
  MsgBox wdmFile.DataCount
  
  For iTser = 1 To wdmFile.DataCount
    Set lTser = wdmFile.Data(iTser)
    gdbFile.AddTimSer lTser
  Next
End Sub
