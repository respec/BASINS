VERSION 5.00
Object = "{F9043C88-F6F2-101A-A3C9-08002B2F49FB}#1.2#0"; "COMDLG32.OCX"
Begin VB.Form frmMain 
   Caption         =   "Database Utility"
   ClientHeight    =   3345
   ClientLeft      =   60
   ClientTop       =   345
   ClientWidth     =   4320
   Icon            =   "main.frx":0000
   LinkTopic       =   "Form1"
   ScaleHeight     =   3345
   ScaleWidth      =   4320
   StartUpPosition =   3  'Windows Default
   Begin VB.CheckBox chkBASINSNHD 
      Caption         =   "BASINS NHD"
      Height          =   255
      Left            =   2280
      TabIndex        =   5
      Top             =   1680
      Width           =   1815
   End
   Begin VB.CommandButton cmdGeoDatabaseToShape 
      Caption         =   "GeoDatabase to Shape"
      Height          =   495
      Left            =   120
      TabIndex        =   4
      Top             =   1560
      Width           =   1935
   End
   Begin VB.CheckBox chkData 
      Caption         =   "Include data"
      Height          =   255
      Left            =   240
      TabIndex        =   3
      Top             =   720
      Value           =   1  'Checked
      Width           =   1815
   End
   Begin VB.CommandButton cmdXMLtoDatabase 
      Caption         =   "XML to Database"
      Height          =   495
      Left            =   2280
      TabIndex        =   1
      Top             =   120
      Width           =   1935
   End
   Begin MSComDlg.CommonDialog cdlg 
      Left            =   3840
      Top             =   600
      _ExtentX        =   847
      _ExtentY        =   847
      _Version        =   393216
      CancelError     =   -1  'True
   End
   Begin VB.CommandButton cmdDatabaseToXML 
      Caption         =   "Database to XML"
      Height          =   495
      Left            =   120
      TabIndex        =   0
      Top             =   120
      Width           =   1935
   End
   Begin VB.Label lblStatus 
      Alignment       =   2  'Center
      Height          =   495
      Left            =   0
      TabIndex        =   2
      Top             =   2760
      Width           =   4095
   End
End
Attribute VB_Name = "frmMain"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Private Sub chkBASINSNHD_Click()
  If chkBASINSNHD.Value = vbChecked Then
    g_NHD_DBF = True
  Else
    g_NHD_DBF = False
  End If
End Sub

Private Sub chkData_Click()
  If chkData.Value = vbChecked Then CopyData = True Else CopyData = False
End Sub

Private Sub cmdDatabaseToXML_Click()
  Dim DatabaseFilename As String
  Dim XMLFilename As String
  
  On Error GoTo NeverMind
  
  cdlg.DialogTitle = "Select database to read"
  cdlg.Filter = "Microsoft Database *.mdb|*.mdb|All Files|*.*"
  cdlg.FilterIndex = 1
  cdlg.ShowOpen
  DatabaseFilename = cdlg.filename
  
  cdlg.DialogTitle = "Save XML file as"
  cdlg.Filter = "XML files *.xml|*.xml|All Files|*.*"
  cdlg.FilterIndex = 1
  cdlg.DefaultExt = "xml"
  cdlg.filename = FilenameNoExt(DatabaseFilename) & ".xml"
  cdlg.ShowOpen
  XMLFilename = cdlg.filename
  
  lblStatus = "Converting " & DatabaseFilename & " to " & XMLFilename: lblStatus.Refresh
  
  On Error GoTo 0
  Me.MousePointer = vbHourglass
  DatabaseToXML DatabaseFilename, XMLFilename
  Me.MousePointer = vbDefault
  
  lblStatus = "Finished " & XMLFilename: lblStatus.Refresh
  Exit Sub
NeverMind:
  lblStatus = Err.Description: lblStatus.Refresh
End Sub

Private Sub cmdGeoDatabaseToShape_Click()
  Dim DatabaseFilename As String
  Dim ShapeFilename As String
  
  On Error GoTo NeverMind
  
  cdlg.DialogTitle = "Select database to read"
  cdlg.Filter = "Microsoft Database *.mdb|*.mdb|All Files|*.*"
  cdlg.FilterIndex = 1
  cdlg.ShowOpen
  DatabaseFilename = cdlg.filename
    
  lblStatus = "Opening " & DatabaseFilename & " to create shape files": lblStatus.Refresh
  
  On Error GoTo 0
  Me.MousePointer = vbHourglass
  GeoDatabaseToShape DatabaseFilename
  Me.MousePointer = vbDefault
  
  lblStatus = "Finished writing " & ShapeFilename: lblStatus.Refresh
  Exit Sub
NeverMind:
  lblStatus = Err.Description: lblStatus.Refresh
End Sub

Private Sub cmdXMLtoDatabase_Click()
  Dim DatabaseFilename As String
  Dim XMLFilename As String
  
  On Error GoTo NeverMind
  
  cdlg.DialogTitle = "Select XML file to read"
  cdlg.Filter = "XML files *.xml|*.xml|All Files|*.*"
  cdlg.FilterIndex = 1
  cdlg.ShowOpen
  XMLFilename = cdlg.filename
  
  cdlg.DialogTitle = "Save database as"
  cdlg.Filter = "Microsoft Database *.mdb|*.mdb|All Files|*.*"
  cdlg.FilterIndex = 1
  cdlg.DefaultExt = "mdb"
  cdlg.filename = FilenameNoExt(cdlg.filename) & ".mdb"
  cdlg.ShowOpen
  DatabaseFilename = cdlg.filename
  
  lblStatus = "Converting " & XMLFilename & " to " & DatabaseFilename: lblStatus.Refresh
  
  On Error GoTo 0
  Me.MousePointer = vbHourglass
  XMLtoDatabase XMLFilename, DatabaseFilename
  Me.MousePointer = vbDefault
  
  lblStatus = "Finished " & DatabaseFilename: lblStatus.Refresh
  Exit Sub
NeverMind:
  lblStatus = Err.Description: lblStatus.Refresh
End Sub

Private Sub Form_Load()
  If chkData.Value = vbChecked Then CopyData = True Else CopyData = False
End Sub

Private Sub Form_Resize()
  Dim w&, h&
  w = Me.ScaleWidth
  h = Me.ScaleHeight
  If w > 2000 Then lblStatus.Width = w - 225
  If h > lblStatus.Top + 300 Then lblStatus.Height = h - lblStatus.Top - 50
End Sub
