VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsWebDataManager"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit

Private pCurlExePath As String
Private pFormDownload As frmDownload
Private pResultString As String
Private pDataTypes As Collection
Private pCollFrmCriteria As Collection
Private pCurrentStatus As DOMDocument
Private pIPC As ATCoIPC
Attribute pIPC.VB_VarHelpID = -1
'Private pStatusFile As String

Public Property Get collFrmCriteria() As Collection
  Set collFrmCriteria = pCollFrmCriteria
End Property

Public Property Get CurrentStatus() As DOMDocument
  Set CurrentStatus = pCurrentStatus
End Property

Public Sub ShowCriteria(dat As clsWebData, Optional parentForm As Object = Nothing, Optional Index As Long = 0)
  Dim frmCrit As New frmCriteria
  collFrmCriteria.Add frmCrit
  Set frmCrit.Manager = Me
  frmCrit.HelpContextID = dat.HelpId
  Set frmCrit.DataType = dat
  frmCrit.SubIndex = Index
  frmCrit.Show
  
  If Not parentForm Is Nothing Then
    Set frmCrit.ParentCriteria = parentForm
    parentForm.Visible = False
    frmCrit.Top = parentForm.Top
    frmCrit.Left = parentForm.Left
  End If
End Sub

Public Sub ShowHTML(HTML As String)
  Dim Filename As String
  Filename = GetTmpFileName & ".html"
  SaveFileString Filename, HTML
  OpenFile Filename
End Sub

Public Sub CurrentStatusFromFile(Filename As String)
  Dim s As String
  Dim root As IXMLDOMElement
  Dim nod1 As IXMLDOMElement
  Dim lNoFile As Boolean
  Dim vFrm As Variant
  
  On Error Resume Next
  For Each vFrm In collFrmCriteria
    Unload vFrm
  Next
  Set pCollFrmCriteria = Nothing
  Set pCollFrmCriteria = New Collection
  
  On Error GoTo errHand
  
  If Len(Filename) = 0 Then
    lNoFile = True
  ElseIf Len(Dir(Filename)) = 0 Then
    lNoFile = True
  Else
    lNoFile = False
  End If
  If lNoFile Then 'no file, dummy Status structure
    Set pCurrentStatus.documentElement = pCurrentStatus.createElement("clsWebDataManager")
    Set root = pCurrentStatus.documentElement
    Set nod1 = pCurrentStatus.createElement("status_variables")
    root.appendChild nod1
  Else
    s = WholeFileString(Filename)
    pCurrentStatus.loadXML s
  End If
  
  'DomStatus pCurrentStatus, pStatusFile
  
  Exit Sub
  
errHand:
  MsgBox Err.Description, vbCritical, "Error in CurrentStatusFromFile"
End Sub

Public Function CurrentStatusGetString(ByVal StatusVariable As String) As String
  Dim lMatchNodeList As IXMLDOMNodeList
  Dim vOldNode As Variant
  Dim lOldNode As IXMLDOMElement
  Dim lNewNode As IXMLDOMElement
  Dim lBaseNode As IXMLDOMNodeList
  Dim retval As String
  
  On Error GoTo errHand
  
  Set lBaseNode = pCurrentStatus.getElementsByTagName("status_variables")
  StatusVariable = LCase(StatusVariable)
  For Each vOldNode In lBaseNode(0).childNodes
    Set lOldNode = vOldNode
    If LCase(lOldNode.nodeName) = StatusVariable Then
      If Len(lOldNode.Text) > 0 Then
        CurrentStatusGetString = lOldNode.Text
        Exit Function
      End If
    End If
  Next
    
  Exit Function
  
errHand:
  MsgBox "Error getting value of '" & StatusVariable & "' in status." & vbCr & Err.Description, vbOKOnly, "Web Data Manager"
End Function

Public Sub CurrentStatusUpdateString(StatusVariable As String, Value As String, Optional Status As String = "unknown")
  Dim lMatchNodeList As IXMLDOMNodeList
  Dim vOldNode As Variant
  Dim lOldNode As IXMLDOMElement
  Dim lNewNode As IXMLDOMElement
  Dim lBaseNode As IXMLDOMNodeList
  
  On Error GoTo errHand
  
  Set lBaseNode = pCurrentStatus.getElementsByTagName("status_variables")
  
  Set lMatchNodeList = pCurrentStatus.getElementsByTagName(StatusVariable)
  For Each vOldNode In lMatchNodeList
    Set lOldNode = vOldNode
    lBaseNode(0).removeChild lOldNode
  Next
    
  Set lNewNode = pCurrentStatus.createElement(StatusVariable)
  lNewNode.Text = Value
  lNewNode.setAttribute "status", Status
  lBaseNode(0).appendChild lNewNode
  
  Exit Sub
  
errHand:
  MsgBox "Error updating string '" & Value & "' in status." & vbCr & Err.Description, vbOKOnly, "Web Data Manager"
End Sub
Public Sub CurrentStatusUpdateList(StatusVariable As String, ValueList As IXMLDOMNodeList, Optional Status As String = "unknown")
  Dim lMatchNodeList As IXMLDOMNodeList
  Dim vOldNode As Variant
  Dim lOldNode As IXMLDOMElement
  Dim vNewNode As IXMLDOMElement
  Dim lBaseNode As IXMLDOMNodeList
  
  On Error GoTo errHand
  
  Set lBaseNode = pCurrentStatus.getElementsByTagName("status_variables")
  
  'If Index = 0 Then
    Set lMatchNodeList = pCurrentStatus.getElementsByTagName(StatusVariable)
    For Each vOldNode In lMatchNodeList
      Set lOldNode = vOldNode
      lBaseNode(0).removeChild lOldNode
    Next
  'End If
    
  For Each vNewNode In ValueList
    vNewNode.setAttribute "status", Status
    lBaseNode(0).appendChild vNewNode
  Next
  
  Exit Sub
  
errHand:
  MsgBox "Error updating list in status." & vbCr & Err.Description, vbOKOnly, "Web Data Manager"
End Sub

Public Property Get DataTypes() As Collection
  Set DataTypes = pDataTypes
End Property
Public Function DataTypeFromLabel(myLabel As String) As clsWebData
  Dim vDataType As Variant
  
  For Each vDataType In pDataTypes
    If vDataType.Label = myLabel Then
      Set DataTypeFromLabel = vDataType
      Exit For
    End If
  Next
End Function

'Public Property Let StatusFile(newStatusFile As String)
'  pStatusFile = newStatusFile
'End Property
'Public Property Get StatusFile() As String
'  StatusFile = pStatusFile
'End Property

Public Property Let ResultString(newResultString As String)
  pResultString = newResultString
End Property
Public Property Get ResultString() As String
  ResultString = pResultString
End Property

Public Function Provides(Attr As String) As Collection
  Dim lProvides As Collection
  Dim vDataType As Variant
  Dim lDataType As clsWebData
  Dim lNodeList As IXMLDOMNodeList
  Dim lAvailable As IXMLDOMNodeList
  Dim lDom As DOMDocument
 
  Set lProvides = Nothing
  Set lProvides = New Collection
  
  For Each vDataType In pDataTypes
    Set lDataType = vDataType
    Set lAvailable = lDataType.Provides.getElementsByTagName("available")
    If lAvailable.Length > 0 Then
      Set lDom = New DOMDocument
      lDom.loadXML lAvailable(0).xml
      Set lNodeList = lDom.getElementsByTagName(Attr)
      If lNodeList.Length > 0 Then
        lProvides.Add lDataType.Label
      End If
    End If
  Next
  
  Set Provides = lProvides
End Function

'Public Sub ShowTree(TreeDoc As Variant, TreeCaption As String)
'  If VarType(TreeDoc) = 9 Then
'    Set frmXMLTree.TreeDoc = TreeDoc
'  Else
'    Set frmXMLTree.TreeDoc = New DOMDocument
'    frmXMLTree.TreeDoc.loadXML TreeDoc
'  End If
'  On Error Resume Next
'  frmXMLTree.caption = TreeCaption & " XML Tree"
'  frmXMLTree.BuildTree
'  frmXMLTree.Show vbModal
'End Sub

'Returns True if download was successful
Public Function Download(ByVal URL As String, _
                         ByVal Filename As String, _
                         RestartTimer As Boolean, _
                         Optional newCaption As String = "Downloading", _
                         Optional prependToOutput As String = "", _
                         Optional OpenClose As String = "open") As Boolean
  Dim URLfirstpart As String
  Dim URLdatapart As String
  Dim logfileName As String
  Dim URLfileName As String
  Dim TemporaryDownload As Boolean
  Dim Msg As String
  Dim cmdline As String
  
  If pFormDownload Is Nothing Then Set pFormDownload = New frmDownload
  
  If InStr(OpenClose, "open") > 0 Then pFormDownload.Show
  pFormDownload.Clear RestartTimer
  pFormDownload.caption = newCaption
  'pFormDownload.prependToOutput = prependToOutput
  pFormDownload.txtFrom = URL
  pFormDownload.txtTo = Filename
  
  URLfileName = GetTmpFileName
  logfileName = GetTmpFileName
  If Len(Filename) = 0 Then
    Filename = GetTmpFileName
    pFormDownload.txtTo = "Temporary file: " & Filename
    TemporaryDownload = True
  End If
  
  cmdline = pCurlExePath & " -o" & Filename & " --trace-ascii " & logfileName
  
  URLdatapart = URL
  URLfirstpart = StrSplit(URLdatapart, "?", "")
  If Len(URLdatapart) > 0 Then
    SaveFileString URLfileName, URLdatapart
    cmdline = cmdline & " -d@" & URLfileName & " " & URLfirstpart
  Else
    cmdline = cmdline & " " & URL
  End If
  
  pIPC.StartProcess "curl", cmdline '& " > " & logfileName

  Do
    DoEvents
    ShowWin pCurlExePath, SW_MINIMIZE, 0
    'Msg = WholeFileString(logfileName)
    'If Msg <> pFormDownload.txtStatus Then
    '  pFormDownload.txtStatus = Right(Msg, 1000)
    '  pFormDownload.txtStatus.SelStart = Len(pFormDownload.txtStatus)
    'End If
    pResultString = pIPC.GetProcessMessage("curl", 1)
    If InStr(pResultString, "expired") = 0 And InStr(pResultString, "% Total") = 0 Then
      Msg = Trim(Mid(pResultString, 15, 6))
      If IsNumeric(Msg) Or IsNumeric(Left(Msg, Len(Msg) - 1)) Then
        pFormDownload.txtStatus = "For current file, " & Msg & " bytes transferred"
        Msg = Trim(Mid(pResultString, 10, 5))
        If IsNumeric(Msg) And CLng(Msg) > 0 And CLng(Msg) < 100 Then
          pFormDownload.txtStatus = pFormDownload.txtStatus & " (" & Msg & "%)"
        End If
      Else
        pFormDownload.txtStatus = pResultString
      End If
    End If
    'Debug.Print pResultString
  Loop While InStr(pResultString, "exited") = 0 And pFormDownload.caption <> "Cancelled"
      
  If pFormDownload.caption = "Cancelled" Then
    pFormDownload.txtStatus = "Cancelled"
    pResultString = "<undefined/>"
    pIPC.ExitProcess "curl"
    Kill Filename
  ElseIf TemporaryDownload Then
    pFormDownload.txtStatus = "Removing temporary file " & Filename
    pResultString = WholeFileString(Filename)
    Kill Filename
  ElseIf Len(prependToOutput) > 0 Then
    pFormDownload.txtStatus = "Prepending some data to file"
    SaveFileString Filename, prependToOutput & WholeFileString(Filename)
  End If
  pFormDownload.txtStatus.Refresh
  On Error Resume Next
  Kill logfileName
  Kill URLfileName
  If pResultString <> "<undefined/>" And Len(pResultString) > 0 Then
    Download = True
    pFormDownload.txtStatus = "Download Complete"
  End If
  If InStr(OpenClose, "close") > 0 Then
    pFormDownload.Hide
  Else
    pFormDownload.Deactivate
  End If
  Filename = PathNameOnly(Filename)
  If Len(Filename) > 0 Then
    If Right(Filename, 1) <> "\" Then Filename = Filename & "\"
    SaveSetting "WebDataManager", "defaults", "save_dir", Filename
  End If
End Function

Public Property Get Version(Optional Sp = 0) As String
  Dim vDataType As Variant
  
  Version = App.Major & "." & App.Minor & "." & App.Revision
  
  For Each vDataType In pDataTypes
    Version = Version & vbCrLf & Space(Sp) & vDataType.Label & " " & vDataType.Version
  Next
  
End Property

Private Sub Class_Initialize()
  Dim lWebData As clsWebData
  Dim lDataTypes As Variant
  Dim iDataType As Integer
'  Dim windir As String
  Dim Filename As String
  Dim clsname As String
  Dim TriedAgain As Boolean
  Dim ff As New ATCoFindFile
  
  Set pIPC = New ATCoIPC
  pIPC.ExitProcess "Status" 'Don't need status monitor for this application
  
  ff.SetRegistryInfo "curl", "paths", "EXEpath"
  ff.SetDialogProperties "Please locate curl.exe", "curl.exe"
  pCurlExePath = ff.GetName   'path = "c:\program files\Aqua_Terra\curl.exe"
  
  On Error GoTo ErrGeneral
  
  Set pDataTypes = Nothing
  Set pDataTypes = New Collection
  Set pCollFrmCriteria = New Collection
  Set pCurrentStatus = New DOMDocument
  
  CurrentStatusFromFile ""
  
  Filename = GetSetting("WebDataManager", "defaults", "save_dir", "")
  If Len(Filename) > 0 Then
    CurrentStatusUpdateString "save_dir", Filename, "default from last session"
  End If
  
  Filename = GetSetting("WebDataManager", "defaults", "project_dir", "")
  If Len(Filename) > 0 Then
    CurrentStatusUpdateString "project_dir", Filename, "default from last session"
  End If
  
  App.HelpFile = GetSetting("WebDataManager", "defaults", "HelpFile", App.path & "\DataDownload.chm")
       
'  windir = Environ("windir")
'  If windir = "" Then
'    windir = "c:\winnt"
'    If Len(Dir(windir, vbDirectory)) < 1 Then windir = "c:\windows"
'  End If
'  Filename = Dir(windir & "\system32\ATCWebData*.dll")
'  GoSub FindSomeDataTypes
'  Filename = Dir(windir & "\system\ATCWebData*.dll")
'  GoSub FindSomeDataTypes
  
  clsname = "USGSsite"
  GoSub FindSomeDataTypes
  clsname = "USGSdaily"
  GoSub FindSomeDataTypes
  
  Exit Sub
  
FindSomeDataTypes:
'  While Len(Filename) > 0
'    Filename = FilenameOnly(Filename)
'    clsname = Mid(Filename, 11)
    On Error GoTo ErrCreate
    Set lWebData = CreateObject("USGSdownload.cls" & clsname) '(Filename & ".cls" & clsname)
    On Error GoTo ErrGeneral
    Set lWebData.Manager = Me
    pDataTypes.Add lWebData, clsname
NextFile:
    Set lWebData = Nothing
'    Filename = Dir
'  Wend
  Return
  
ErrCreate:
  MsgBox "Error creating cls" & clsname, vbOKOnly, Err.Description
  Resume NextFile
  
ErrGeneral:
  MsgBox Err.Description, vbOKOnly, "Error initializing Web Data Manager" & vbCr & Err.Description
End Sub

'Private Sub Class_Terminate()
'  Set pIPC = Nothing
'End Sub
