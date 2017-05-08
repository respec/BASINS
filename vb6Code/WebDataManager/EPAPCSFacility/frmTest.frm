VERSION 5.00
Object = "{F9043C88-F6F2-101A-A3C9-08002B2F49FB}#1.2#0"; "COMDLG32.OCX"
Begin VB.Form Form1 
   Caption         =   "EPA PCS Facility CSV to Shape File"
   ClientHeight    =   735
   ClientLeft      =   60
   ClientTop       =   345
   ClientWidth     =   4965
   LinkTopic       =   "Form1"
   ScaleHeight     =   735
   ScaleWidth      =   4965
   StartUpPosition =   3  'Windows Default
   Begin VB.CommandButton cmdCSVtoSHP 
      Caption         =   "CSV to SHP"
      Height          =   495
      Left            =   120
      TabIndex        =   0
      Top             =   120
      Width           =   1455
   End
   Begin MSComDlg.CommonDialog cdlg 
      Left            =   1920
      Top             =   120
      _ExtentX        =   847
      _ExtentY        =   847
      _Version        =   393216
   End
End
Attribute VB_Name = "Form1"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Private Sub cmdCSVtoSHP_Click()
  With cdlg
    .CancelError = True
    .DialogTitle = "Select a downloaded PCS facility file"
    .Filter = "CSV files|*.csv|All files|*.*"
    
    On Error GoTo NeverMind
    .ShowOpen
    On Error GoTo 0
    
    Dim csvFile As clsCSV
    Set csvFile = New clsCSV
    csvFile.OpenCSV .filename
    
    'Field names in CSV are too long for DBF, so shorten them by removing
'    Dim dotpos As Long
'    Dim fld As Long
'    For fld = 1 To csvFile.NumFields
'      dotpos = InStrRev(csvFile.FieldName(fld), ".")
'      If dotpos > 0 Then
'        csvFile.FieldName(fld) = Mid(csvFile.FieldName(fld), dotpos + 1)
'      End If
'    Next
    
    .DefaultExt = "shp"
    .DialogTitle = "Save Shape As"
    .Filter = "SHP files|*.shp|All files|*.*"
    .filename = FilenameNoExt(.filename) & ".shp"
    
    On Error GoTo NeverMind
    .ShowSave
    On Error GoTo 0
    
    WritePCSShape csvFile, FilenameNoExt(.filename)
  
  End With
NeverMind:
End Sub
