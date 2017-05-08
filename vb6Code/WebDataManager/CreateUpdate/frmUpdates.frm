VERSION 5.00
Object = "{F9043C88-F6F2-101A-A3C9-08002B2F49FB}#1.2#0"; "COMDLG32.OCX"
Begin VB.Form frmUpdates 
   Caption         =   "Upload Update"
   ClientHeight    =   4260
   ClientLeft      =   60
   ClientTop       =   345
   ClientWidth     =   11535
   Icon            =   "FRMUPD~1.frx":0000
   LinkTopic       =   "Form1"
   ScaleHeight     =   4260
   ScaleWidth      =   11535
   StartUpPosition =   3  'Windows Default
   Begin VB.CommandButton cmdCommit 
      Caption         =   "Save"
      Default         =   -1  'True
      Enabled         =   0   'False
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
      TabIndex        =   4
      Top             =   3720
      Width           =   1215
   End
   Begin VB.TextBox txtOldXML 
      BackColor       =   &H8000000F&
      Height          =   3015
      Left            =   120
      Locked          =   -1  'True
      MultiLine       =   -1  'True
      ScrollBars      =   3  'Both
      TabIndex        =   1
      Top             =   4680
      Width           =   11295
   End
   Begin VB.TextBox txtNewXML 
      Height          =   3015
      Left            =   120
      MultiLine       =   -1  'True
      ScrollBars      =   3  'Both
      TabIndex        =   0
      Top             =   480
      Width           =   11295
   End
   Begin MSComDlg.CommonDialog cdlg 
      Left            =   120
      Top             =   120
      _ExtentX        =   847
      _ExtentY        =   847
      _Version        =   393216
   End
   Begin VB.Label Label2 
      Caption         =   "New XML:"
      Height          =   255
      Left            =   120
      TabIndex        =   3
      Top             =   120
      Width           =   2175
   End
   Begin VB.Label Label1 
      Caption         =   "Replacing the following XML:"
      Height          =   255
      Left            =   120
      TabIndex        =   2
      Top             =   4320
      Width           =   2415
   End
End
Attribute VB_Name = "frmUpdates"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Private AvailableUpdates As DOMDocument

Private pXMLfilename As String
Private pFilenameToUpload As String

Private Sub Log(Msg As String)
  Logger.Log Msg
End Sub

Public Sub UploadUpdate(xmlFilename As String, filenameToUpload As String)
  Dim nNewComponents As Long
  Dim existingXml As String
  Dim xml As String
  Dim ComponentIndex As Long
  Dim ComponentName As String
  'Dim ReleaseNote As String
  Dim Destination As String
  Dim availNode As IXMLDOMNode
  Dim lNode As IXMLDOMNode
  Dim lLocalDoc As New DOMDocument
  Dim errmsg As String
    
  On Error GoTo ErrHand
  pXMLfilename = xmlFilename
  pFilenameToUpload = filenameToUpload
  
  Set AvailableUpdates = New DOMDocument

  Logger.Log "UploadUpdate '" & xmlFilename & "', '" & filenameToUpload & "'"
  existingXml = WholeFileString(xmlFilename)
    
  If InStr(existingXml, "<ATCCompMl>") = 0 Then
    Log "  Could not find <ATCCompMl> in '" & xmlFilename & "'"
    MsgBox "Could not find <ATCCompMl> in '" & xmlFilename & "'", vbCritical, "UploadUpdate"
'    If Logger.LogMsg("Could not find updates at " & url & vbCr _
'            & "View error message?", "WebDataManager:Check for Updates", _
'            "Yes", "No") = 1 Then
'      Me.ShowHTML pResultString
'    End If
  Else
    'txtOldXML.Tag = existingXml
    AvailableUpdates.loadXML existingXml
    If AvailableUpdates.parseError <> 0 Then
      errmsg = "Error parsing updates from " & xmlFilename & vbCr _
              & AvailableUpdates.parseError.reason & vbCr _
              & AvailableUpdates.parseError.srcText
      Logger.LogMsg errmsg, "UploadUpdate"
    Else
      ComponentName = FilenameOnly(filenameToUpload)
      lLocalDoc.loadXML MakeComponentXML(filenameToUpload, "{app}\etc\Extensions\DataDownload", , , ComponentName)
      Set lNode = FindNode(lLocalDoc, "Component", "Name", ComponentName)
      'Search for an existing version of the file we are planning to upload
      Set availNode = FindNode(AvailableUpdates, "Component", "File", FilenameNoPath(filenameToUpload))
      If availNode Is Nothing Then
        Me.Height = 4605
      Else
        Me.Height = 8250
        txtOldXML.Text = availNode.xml
        availNode.Attributes.getNamedItem("Version").Text = lNode.Attributes.getNamedItem("Version").Text
        availNode.Attributes.getNamedItem("Date").Text = lNode.Attributes.getNamedItem("Date").Text
        availNode.Attributes.getNamedItem("Size").Text = lNode.Attributes.getNamedItem("Size").Text
        Set lNode = Nothing
        Set lNode = availNode
        AvailableUpdates.childNodes(2).removeChild availNode ' .replaceChild lNode, availNode
      End If
      txtNewXML.Text = lNode.xml
      'txtNewXML.Tag = AvailableUpdates.xml
    End If
  End If
   cmdCommit.Enabled = True
  Log "UploadUpdate Exit"
  Exit Sub
ErrHand:
  errmsg = "Error uploading updates " & vbCr & Err.Description
  Logger.LogMsg errmsg, "UploadUpdate"
  Log "UploadUpdate Error Exit"
End Sub

Private Sub cmdCommit_Click()
  Dim lNode As IXMLDOMNode
  Dim lLocalDoc As New DOMDocument
  Dim xml As String
  xml = "<?xml version=""1.0"" standalone=""no""?>"
 'xml = xml & vbCrLf & "<!DOCTYPE ATCCompMl SYSTEM ""http://hspf.com/pub/download/ATCCompMl.dtd"">"
  xml = xml & vbCrLf & "<ATCCompMl>"
  xml = xml & vbCrLf & txtNewXML.Text
  xml = xml & vbCrLf & "</ATCCompMl>"

  lLocalDoc.loadXML xml
  Set lNode = lLocalDoc.childNodes(2).childNodes(0)
  AvailableUpdates.childNodes(2).appendChild lNode
  If MsgBox(AvailableUpdates.xml, vbOKCancel, "Proposed new XML") = vbOK Then
    FileCopy pFilenameToUpload, PathNameOnly(pXMLfilename) & "\components\" & FilenameNoPath(pFilenameToUpload)
    If FileExists(pXMLfilename & ".bak") Then Kill pXMLfilename & ".bak"
    Name pXMLfilename As pXMLfilename & ".bak"
    SaveFileString pXMLfilename, AvailableUpdates.xml
  End If
End Sub

Private Sub Form_Load()
  Me.Height = 4605
  On Error GoTo ErrHand
  
  Me.MousePointer = vbHourglass

  With cdlg
    .CancelError = True
    .DialogTitle = "Select file to upload to test server"
    .Filter = "All files (*.*)|*.*"
    .FilterIndex = 1
    .ShowOpen
    
    UploadUpdate TestFile, .Filename
    
  End With
  
  Me.MousePointer = vbDefault
  Exit Sub
  
ErrHand:
  Logger.LogMsg "Error: " & Err.Description & " (" & Err.Number & ")", "Update"
  Me.MousePointer = vbDefault
End Sub

Private Sub Form_Resize()
  If ScaleWidth > 1000 And ScaleHeight > 1000 Then
    txtNewXML.Width = ScaleWidth - 255
    txtOldXML.Width = txtNewXML.Width
  End If
End Sub

Private Function FindNode(lDoc As DOMDocument, _
                          lNodeName As String, _
                          lAttrName As String, _
                          ByVal lAttrValue As String) As IXMLDOMNode
  Dim i As Long
  
  lAttrValue = LCase(lAttrValue)
  
  With lDoc.getElementsByTagName(lNodeName)
    For i = 0 To .Length - 1
      With .Item(i).Attributes.getNamedItem(lAttrName)
        If LCase(.Text) = lAttrValue Then
          Exit For
        End If
      End With
    Next i
    Set FindNode = .Item(i)
  End With
End Function

'Private Sub txtNewXML_MouseDown(Button As Integer, Shift As Integer, x As Single, y As Single)
'  If Button = vbRightButton Then MsgBox txtNewXML.Tag, vbOKOnly, "Entire new XML"
'End Sub
'
'Private Sub txtOldXML_MouseDown(Button As Integer, Shift As Integer, x As Single, y As Single)
'  If Button = vbRightButton Then MsgBox txtOldXML.Tag, vbOKOnly, "Entire old XML"
'End Sub
