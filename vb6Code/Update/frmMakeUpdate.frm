VERSION 5.00
Object = "{F9043C88-F6F2-101A-A3C9-08002B2F49FB}#1.2#0"; "COMDLG32.OCX"
Begin VB.Form frmMakeUpdate 
   Caption         =   "Make Update"
   ClientHeight    =   8145
   ClientLeft      =   60
   ClientTop       =   345
   ClientWidth     =   9720
   LinkTopic       =   "Form1"
   ScaleHeight     =   8145
   ScaleWidth      =   9720
   StartUpPosition =   3  'Windows Default
   Begin VB.CommandButton cmdWrite 
      Caption         =   "Write Selected"
      Height          =   315
      Left            =   120
      TabIndex        =   14
      Top             =   7800
      Width           =   1335
   End
   Begin VB.CheckBox chkPatch 
      Caption         =   "Patch"
      Height          =   255
      Left            =   120
      TabIndex        =   13
      ToolTipText     =   "Check if creating patch rather than full package, leaves Instructions"
      Top             =   1320
      Width           =   975
   End
   Begin VB.TextBox txtReleaseNote 
      Height          =   375
      Left            =   2520
      TabIndex        =   11
      Top             =   1200
      Width           =   7095
   End
   Begin VB.TextBox txtXMLOutDir 
      Height          =   375
      Left            =   2520
      TabIndex        =   3
      Top             =   840
      Width           =   7095
   End
   Begin VB.TextBox txtXMLinDir 
      Height          =   375
      Left            =   2520
      TabIndex        =   1
      Top             =   480
      Width           =   7095
   End
   Begin VB.TextBox txtComponents 
      Height          =   375
      Left            =   2520
      TabIndex        =   8
      Top             =   120
      Width           =   7095
   End
   Begin VB.CommandButton cmdOpenComponents 
      Caption         =   "Open"
      Height          =   255
      Left            =   120
      TabIndex        =   10
      Top             =   240
      Width           =   1095
   End
   Begin VB.CommandButton cmdOpenXMLin 
      Caption         =   "Open"
      Height          =   255
      Left            =   120
      TabIndex        =   7
      Top             =   600
      Width           =   1095
   End
   Begin VB.ListBox lstFiles 
      Height          =   6135
      Left            =   120
      Style           =   1  'Checkbox
      TabIndex        =   6
      Top             =   1680
      Width           =   2295
   End
   Begin VB.CommandButton cmdOpenXMLOut 
      Caption         =   "Open"
      Height          =   255
      Left            =   120
      TabIndex        =   5
      Top             =   960
      Width           =   1095
   End
   Begin VB.TextBox txtXML 
      Height          =   6375
      Left            =   2520
      MultiLine       =   -1  'True
      ScrollBars      =   3  'Both
      TabIndex        =   0
      Top             =   1680
      Width           =   7095
   End
   Begin MSComDlg.CommonDialog cdlg 
      Left            =   120
      Top             =   1200
      _ExtentX        =   847
      _ExtentY        =   847
      _Version        =   393216
      CancelError     =   -1  'True
   End
   Begin VB.Label lblReleaseNote 
      Alignment       =   1  'Right Justify
      Caption         =   "Release Note"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   255
      Left            =   1200
      TabIndex        =   12
      Top             =   1320
      Width           =   1215
   End
   Begin VB.Label lblComponents 
      Alignment       =   1  'Right Justify
      Caption         =   "Components"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   255
      Left            =   1200
      TabIndex        =   9
      Top             =   240
      Width           =   1215
   End
   Begin VB.Label lblXMLOutDir 
      Alignment       =   1  'Right Justify
      Caption         =   "XML Out Dir"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   255
      Left            =   1200
      TabIndex        =   4
      Top             =   960
      Width           =   1215
   End
   Begin VB.Label lblXMLinDir 
      Alignment       =   1  'Right Justify
      Caption         =   "XML In Dir"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   255
      Left            =   1200
      TabIndex        =   2
      Top             =   600
      Width           =   1215
   End
End
Attribute VB_Name = "frmMakeUpdate"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Private Sub cmdWrite_Click()
  Dim i As Integer
  For i = 0 To lstFiles.ListCount - 1
    If lstFiles.Selected(i) Then RewriteXML (lstFiles.List(i))
  Next
End Sub

Private Sub Form_Load()
  txtComponents.Text = GetSetting("Update", "Dirs", "Components", "")
  txtXMLinDir.Text = GetSetting("Update", "Dirs", "XMLin", "")
  txtXMLOutDir.Text = GetSetting("Update", "Dirs", "XMLout", "")
End Sub

Private Sub Form_Resize()
  Dim w As Long
  Dim h As Long
  w = Me.ScaleWidth
  h = Me.ScaleHeight
  If w > txtXML.Left + 250 And h > txtXML.Top + 250 Then
    txtXML.Width = w - txtXML.Left - 120
    txtComponents.Width = txtXML.Width
    txtXMLinDir.Width = txtXML.Width
    txtXMLOutDir.Width = txtXML.Width
    txtReleaseNote.Width = txtXML.Width
  
    txtXML.Height = h - txtXML.Top - 120
    cmdWrite.Top = h - cmdWrite.Height - 120
    lstFiles.Height = cmdWrite.Top - lstFiles.Top - 120
  End If
End Sub

Private Sub Form_Unload(Cancel As Integer)
  SaveSetting "Update", "Dirs", "Components", txtComponents.Text
  SaveSetting "Update", "Dirs", "XMLin", txtXMLinDir.Text
  SaveSetting "Update", "Dirs", "XMLout", txtXMLOutDir.Text
End Sub

Private Sub cmdOpenComponents_Click()
  On Error GoTo errhand
  cdlg.filename = txtComponents.Text & "\untitled.dll"
  cdlg.DialogTitle = "Open an XML file from the input folder"
  cdlg.Filter = "Executables and libraries|*.dll,*.ocx,*.exe|*.*|*.*"
  cdlg.FilterIndex = 1
  cdlg.ShowOpen
  
  txtComponents.Text = PathNameOnly(cdlg.filename)
    
errhand:
End Sub

Private Sub cmdOpenXMLin_Click()
  On Error GoTo errhand
  cdlg.filename = txtXMLinDir.Text & "\untitled.xml"
  cdlg.DialogTitle = "Open an XML file from the input folder"
  cdlg.Filter = "*.xml|*.xml|*.*|*.*"
  cdlg.FilterIndex = 1
  cdlg.ShowOpen
  
  txtXMLinDir.Text = PathNameOnly(cdlg.filename)
  
errhand:
End Sub

Private Sub cmdOpenXMLOut_Click()
  On Error GoTo errhand
  cdlg.filename = txtXMLOutDir.Text & "\untitled.xml"
  cdlg.DialogTitle = "Save XML file as..."
  cdlg.Filter = "*.xml|*.xml|*.*|*.*"
  cdlg.FilterIndex = 1
  cdlg.ShowSave

  txtXMLOutDir.Text = PathNameOnly(cdlg.filename)

errhand:
End Sub

Private Sub UpdateXMLifAttributeChanged(aXML As ChilkatXml, aAttribName As String, aNewValue As String)
  If aXML.GetAttrValue(aAttribName) <> aNewValue Then
    logmsg aAttribName & ": " & aXML.GetAttrValue(aAttribName) & " -> " & aNewValue
    aXML.UpdateAttribute aAttribName, aNewValue
  End If
End Sub

Private Sub RewriteXML(aFilename As String)
  Dim XMLobj As New ChilkatXml
  Dim oldComponent As ChilkatXml
  Dim iChild As Integer
  Dim curChild As ChilkatXml
  
  Dim barefilename As String
  Dim componentFilename As String
  Dim verString As String
  
  barefilename = FilenameOnly(aFilename)
  componentFilename = txtComponents.Text & "\" & aFilename
  logmsg aFilename & "-------------------------"
  If FileExists(txtXMLinDir.Text & "\" & barefilename & ".xml") Then
    XMLobj.LoadXmlFile (txtXMLinDir.Text & "\" & barefilename & ".xml")
    Set oldComponent = XMLobj.FindChild("Component")
    If oldComponent Is Nothing Then
      logmsg "Could not find Component in " & txtXMLinDir.Text & "\" & barefilename & ".xml"
    Else
      verString = GetFileVerString(componentFilename)
      While Right(verString, 2) = ".0"
        verString = Left(verString, Len(verString) - 2)
      Wend
      UpdateXMLifAttributeChanged oldComponent, "Version", verString
      UpdateXMLifAttributeChanged oldComponent, "Date", Format(FileDateTime(componentFilename), "MM/DD/YYYY hh:mm:ss am/pm")
      UpdateXMLifAttributeChanged oldComponent, "Size", Format(FileLen(componentFilename), "#,###")
      
      If Len(txtReleaseNote.Text) > 0 Then
        Set curChild = oldComponent.GetChildWithTag("ReleaseNote")
        If curChild Is Nothing Then
          logmsg "Adding release note"
          oldComponent.newChild "ReleaseNote", txtReleaseNote.Text
        ElseIf curChild.Content <> txtReleaseNote.Text Then
          logmsg "ReleaseNote: " & curChild.Content & " -> " & txtReleaseNote.Text
          curChild.Content = txtReleaseNote.Text
        End If
      End If
      
      For iChild = (oldComponent.NumChildren - 1) To 0 Step -1
        Set curChild = oldComponent.GetChild(iChild)
        Select Case LCase(curChild.Tag)
          Case "instructions"
            If chkPatch.Value = vbUnchecked Then 'Remove instructions if not making patch
              oldComponent.ExtractChildByIndex iChild
              logmsg "Removed Instructions"
            End If
        End Select
      Next
    
    End If
  Else
    logmsg "Creating new XML"
    XMLobj.LoadXml MakeComponentXML(componentFilename, "", , txtReleaseNote.Text)
  End If
  If Len(txtXMLOutDir.Text) > 0 Then
    SaveFileString txtXMLOutDir.Text & "\" & barefilename & ".xml", FormatXML(XMLobj)
    logmsg "Saved " '& txtXMLOutDir.Text & "\" & barefilename & ".xml"
  End If
End Sub

Private Function FormatXML(aXML As ChilkatXml, Optional aIndent As String = "", Optional aIndentIncrement As String = "    ") As String
  Dim curXML As String
  Dim i As Integer
  Dim nextIndent As String
  
  nextIndent = aIndent + aIndentIncrement
  
  With aXML
    curXML = aIndent & "<" & .Tag
    
    For i = 1 To .NumAttributes
      Select Case LCase(.Tag)
        Case "variable" 'Don't put attributes on separate lines
          curXML = curXML & " " & .GetAttributeName(i - 1) & "=""" & .GetAttributeValue(i - 1) & """"
        Case Else
          curXML = curXML & vbCrLf & nextIndent & .GetAttributeName(i - 1) & "=""" & .GetAttributeValue(i - 1) & """"
      End Select
    Next

    curXML = curXML & ">"
    
    If Len(.Content) = 0 Then
      For i = 1 To .NumChildren
        curXML = curXML & vbCrLf & FormatXML(.GetChild(i - 1), nextIndent, aIndentIncrement)
      Next
      curXML = curXML & vbCrLf & aIndent & "</" & .Tag & ">" & vbCrLf
    ElseIf Len(.Content) < 70 Then
      curXML = curXML & .Content & "</" & .Tag & ">" & vbCrLf
    Else
      curXML = curXML & vbCrLf & nextIndent & .Content
      curXML = curXML & vbCrLf & aIndent & "</" & .Tag & ">" & vbCrLf
    End If
  End With
  FormatXML = curXML
'  curXML = aXML.GetXml
'  curXML = ReplaceString(curXML, ">", ">" & vbCrLf)
'Debug.Print curXML
'  indent curXML, "File="
'  indent curXML, "Version="
'  indent curXML, "Date="
'  indent curXML, "Size="
'  indent curXML, "Name="
'  indent curXML, "Destination="
'  indent curXML, "Destination="
  
'  curXML = ReplaceString(curXML, "File=", vbCrLf & "     " & "File=")
'  curXML = ReplaceString(curXML, "Version=", vbCrLf & "     " & "Version=")
'  curXML = ReplaceString(curXML, "Date=", vbCrLf & "     " & "Date=")
'  curXML = ReplaceString(curXML, "Size=", vbCrLf & "     " & "Size=")
'  curXML = ReplaceString(curXML, "Name=", vbCrLf & "     " & "Name=")
'  curXML = ReplaceString(curXML, "Destination=", vbCrLf & "     " & "Destination=")

End Function

Private Sub indent(ByRef aString As String, aSearchFor As String)
  aString = ReplaceString(aString, aSearchFor, vbCrLf & "     " & aSearchFor)
End Sub

Private Sub cmdOpenFile_Click()
  On Error GoTo errhand
  cdlg.ShowOpen
  txtXML.Text = MakeComponentXML(cdlg.filename, "")
  Clipboard.SetText txtXML.Text
    
errhand:
End Sub

Public Function MakeComponentXML(filename As String, _
                                 Destination As String, _
                                 Optional Instructions As String = "", _
                                 Optional ReleaseNote As String = "", _
                                 Optional ComponentName As String = "") As String
  Dim xml As String
  Dim thisFileInfo As String
  Dim verString As String
  On Error GoTo SomeError
  
  verString = GetFileVerString(filename)
  While Right(verString, 2) = ".0"
    verString = Left(verString, Len(verString) - 2)
  Wend
  
  If Len(ComponentName) = 0 Then ComponentName = FilenameOnly(filename)
  'SetWinDirs
  xml = "<?xml version=""1.0"" standalone=""no""?>"
 'xml = xml & vbCrLf & "<!DOCTYPE ATCCompMl SYSTEM ""http://hspf.com/pub/download/ATCCompMl.dtd"">"
  xml = xml & vbCrLf & "<ATCCompMl>"
  xml = xml & vbCrLf & "  <Component"
  xml = xml & vbCrLf & "     File=""" & FilenameNoPath(filename) & """"
  xml = xml & vbCrLf & "     Version=""" & verString & """"
  xml = xml & vbCrLf & "     Date=""" & Format(FileDateTime(filename), "MM/DD/YYYY hh:mm:ss am/pm") & """"
  xml = xml & vbCrLf & "     Size=""" & Format(FileLen(filename), "#,###") & """"
  xml = xml & vbCrLf & "     Name=""" & ComponentName & """"
  xml = xml & vbCrLf & "     Destination=""" & Destination & """"
  xml = xml & ">"
  xml = xml & vbCrLf & "     <Instructions>" & vbCrLf & Instructions & vbCrLf & "     </Instructions>"
  xml = xml & vbCrLf & "     <ReleaseNote>" & vbCrLf & ReleaseNote & vbCrLf & "     </ReleaseNote>"
  xml = xml & vbCrLf & "  </Component>"
  xml = xml & vbCrLf & "</ATCCompMl>"
  MakeComponentXML = xml
  Exit Function
SomeError:
  MsgBox Err.Description, vbCritical, "MakeComponentXML"
End Function

Private Sub logmsg(msg As String)
  txtXML = txtXML & msg & vbCrLf
End Sub

Private Sub txtComponents_Change()
  Dim curFilename As String
  lstFiles.Clear
  If FileExists(txtComponents.Text, True, False) Then
    curFilename = Dir(txtComponents.Text & "\*.*")
    While Len(curFilename) > 0
      lstFiles.AddItem FilenameNoPath(curFilename)
      curFilename = Dir
    Wend
  End If
End Sub

Private Sub txtXMLinDir_Change()
  If FileExists(txtXMLinDir.Text, True, False) Then
    Dim i As Integer
    For i = 0 To lstFiles.ListCount - 1
      lstFiles.Selected(i) = FileExists(txtXMLinDir.Text & "\" & filenameNoExt(lstFiles.List(i)) & ".xml")
    Next
  End If
End Sub
