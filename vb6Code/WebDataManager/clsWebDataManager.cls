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

Private Const cTmpFileTag As String = "{tmpfile}"

Private pBatch As Boolean
Private pCurlExePath As String
Private pFormDownload As frmDownload
Private pResultString As String
Private pDataTypes As FastCollection
Private pWizard As frmWizard
Private pCurrentStatus As ChilkatXml
Private pIPC As ATCoIPC
Attribute pIPC.VB_VarHelpID = -1
Private pMsg As ATCoMessage
Private pState As Long
Private pLogPath As String

Private Enum Enum_States
  state_initialize = 0
  state_select = 1
  state_criteria = 2
  state_download = 3
  state_update = 4
  state_finish = 1000
  state_cancel_select = 1001
  state_cancel_criteria = 1002
  state_cancel_download = 1003
  state_cancel_update = 1004
End Enum

Public Sub ShowDownload()
  
  If pFormDownload Is Nothing Then Set pFormDownload = New frmDownload
  
  If Not pFormDownload Is Nothing Then
    With pFormDownload
      .Show
      .WindowState = vbNormal
      If .Left < 0 Or .Left > Screen.Width - 100 Then .Left = 10
      If .Top < 0 Or .Top > Screen.Height - 100 Then .Top = 10
    End With
  End If
End Sub

Public Property Get LogPath() As String
  LogPath = pLogPath 'pLogger.GetFileName
End Property

Public Sub LogDbg(Message As String)
  Debug.Print Message
  pIPC.dbg Message
End Sub

Public Function LogMsg(aMessage As String, aTitle As String, ParamArray aButtonName()) As Long
  Dim lButtonName() As Variant
  Dim nButtons As Long
  
  nButtons = UBound(aButtonName) - LBound(aButtonName) + 1
  
  LogDbg "LogMsg:" & aTitle & ":" & aMessage & ":" & nButtons
  
  If Batch Then
    LogMsg = 1 'Always assume user clicked first button, never show messages during batch
    If nButtons > 0 Then 'Button name(s) specified
      LogDbg "LogMsg:BatchButton:" & aButtonName(LBound(aButtonName))
    End If
  Else
    If nButtons < 1 Then 'Create an Ok button if no buttons were specified
      ReDim lButtonName(0)
      lButtonName(0) = "&OK"
    Else
      lButtonName = aButtonName
    End If
    LogMsg = pMsg.ShowArray(aMessage, aTitle, lButtonName)
    If UBound(lButtonName) > 0 Then
      If (LogMsg > 0) Then
        LogDbg "LogMsg:UserSelectedButton:" & lButtonName(LogMsg - 1)
      Else
        LogDbg "LogMsg:UserEscaped"
      End If
    End If
  End If
End Function

'See Enum_States above for valid values
Public Property Get State() As Long
  State = pState
End Property
Public Property Let State(newState As Long)
  If newState >= 999 Then pIPC.ExitProcess 0
  pState = newState
End Property

Public Property Get Batch() As Boolean
  Batch = pBatch
End Property
Public Property Let Batch(newValue As Boolean)
  pBatch = newValue
End Property

Public Property Get CurrentStatus() As ChilkatXml
  Set CurrentStatus = pCurrentStatus
End Property

Public Sub SelectDataType()
  Dim lBasinsDirName As String
  Dim ichk As Long
  Dim iDataType As Long
  Dim lDataTypesToGet As FastCollection
  Dim lDataTypeCaption As String
  Dim nBoxesChecked As Long
  Dim FoundCheckbox() As Boolean
  
  On Error GoTo ErrRecover
  
  LogDbg "SelectDataType"
  
  lBasinsDirName = CurrentStatusGetString("project_dir")
  With Wizard
    .LoadList
    Select Case LCase(CurrentStatusGetString("download_type"))
      Case LCase("BasinsInitialSetup")
        LogDbg "Creating New BASINS Project in '" & lBasinsDirName & "'"
        MkDirPath lBasinsDirName
        ChDriveDir lBasinsDirName
        .chkDataType(0).Value = vbChecked 'Select BASINS Data
        .Step = .Step + 1 'Skip selecting other data type (we are just getting BASINS data)
        .chkCriteria(2).Value = vbChecked 'Select core data
        .Step = .Step + 1 'Skip selecting other BASINS data (we are just getting core data)
        .cmdFinish_Click
      Case "batch"
        Me.Batch = True
        LogDbg "Batch download to '" & lBasinsDirName & "'"
        MkDirPath lBasinsDirName
        ChDriveDir lBasinsDirName
        Set lDataTypesToGet = CurrentStatusGetList("data_type")
        ReDim FoundCheckbox(1 To lDataTypesToGet.Count)
        For iDataType = 1 To lDataTypesToGet.Count
          lDataTypeCaption = LCase(lDataTypesToGet.ItemByIndex(iDataType).content)
          FoundCheckbox(iDataType) = False
          For ichk = .chkDataType.LBound To .chkDataType.UBound
            If LCase(.chkDataType(ichk).Caption) = lDataTypeCaption Then
              .chkDataType(ichk).Value = vbChecked 'Select data to get
              nBoxesChecked = nBoxesChecked + 1
              FoundCheckbox(iDataType) = True
              Exit For
            End If
          Next
        Next
        LogDbg "Checkbox state step " & .Step
        For ichk = .chkDataType.LBound To .chkDataType.UBound
          If .chkDataType(ichk).Value = vbChecked Then
            LogDbg "  + '" & .chkDataType(ichk).Caption & "'"
          Else
            LogDbg "  - '" & .chkDataType(ichk).Caption & "'"
          End If
        Next
        
        If nBoxesChecked > 0 Then
          LogDbg "Batch downloading " & nBoxesChecked & " data types"
          .Step = .Step + 1 'Skip selecting data types
          For iDataType = 1 To lDataTypesToGet.Count
            lDataTypeCaption = LCase(lDataTypesToGet.ItemByIndex(iDataType).content)
            For ichk = .chkCriteria.LBound To .chkCriteria.UBound
              If LCase(.lblCriteria(ichk).Caption) = lDataTypeCaption Then
                .chkCriteria(ichk).Value = vbChecked 'Select data to get
                nBoxesChecked = nBoxesChecked + 1
                FoundCheckbox(iDataType) = True
                Exit For
              End If
            Next
            If Not FoundCheckbox(iDataType) Then
              LogMsg "Unable to find checkbox for '" & lDataTypesToGet.ItemByIndex(iDataType) & "'", "Batch Select Data Type"
            End If
          Next
          LogDbg "Checkbox state step " & .Step
          For ichk = .chkCriteria.LBound To .chkCriteria.UBound
            If .chkCriteria(ichk).Value = vbChecked Then
              LogDbg "  + '" & .lblCriteria(ichk).Caption & "'"
            Else
              LogDbg "  - '" & .lblCriteria(ichk).Caption & "'"
            End If
          Next
        
        End If
        If nBoxesChecked = lDataTypesToGet.Count Then
          .Step = .Step + 1 'Skip second selection/specification step
          .cmdFinish_Click
        Else
          Me.Batch = False
          LogDbg "Batch download turned interactive - found " & nBoxesChecked & " of " & lDataTypesToGet.Count & " requested checkboxes"
        End If
    End Select
  End With
  
  Exit Sub

ErrRecover:
  LogMsg "Error '" & Err.Description & "'", "SelectDataType"
End Sub

Private Function Wizard() As frmWizard
  If pWizard Is Nothing Then
    Set pWizard = New frmWizard
    Set pWizard.Manager = Me
  End If
  Set Wizard = pWizard
End Function

Public Sub ShowCriteria(dat As clsWebData, Optional parentForm As Object = Nothing, Optional Index As Long = 0)
  Dim Step As String
  On Error GoTo ErrHand
  Step = "Log"
  LogDbg "ShowCriteria for " & dat.Label
  
  With Wizard
    Step = ".AddCriteriaFor": .AddCriteriaFor dat
    Step = ".Show":           .Show
  End With
  
  Exit Sub
ErrHand:
  LogMsg Step & vbCr & Err.Description, "clsWebDataManager.ShowCriteria"
End Sub

Public Sub ShowHTML(aHTML As String)
  If Batch Then
    LogDbg "Skipping display of HTML in batch mode"
  Else
    Dim Filename As String
    Filename = GetTmpFileName & ".html"
    While FileExists(Filename)
      Filename = GetTmpFileName & ".html"
    Wend
    SaveFileString Filename, aHTML
    OpenFile Filename
  End If
End Sub

Public Sub CurrentStatusFromFile(aFilename As String)
  Dim s As String
  Dim nthChild As Long
  Dim lSaveStatus As FastCollection
  Dim lNodesWithTag As FastCollection
  Dim lMatchNodeList As ChilkatXml
  Dim vNode As Variant
  Dim lOldNode As ChilkatXml
  Dim lNewNode As ChilkatXml
  Dim lBaseNode As ChilkatXml
  Dim vFrm As Variant
  
  Dim HUCs As FastCollection
  Dim FIPS As Long
  Dim vHUC As Variant
  Dim Abbrev As String
  
  LogDbg "CurrentStatusFromFile:Entry '" & aFilename & "'"
  On Error GoTo ErrHand
  
  If Not FileExists(aFilename) Then
    LogDbg "CurrentStatusFromFile:FileNotFound"
    If pCurrentStatus Is Nothing Then
'      Set pCurrentStatus.documentElement = pCurrentStatus.createElement("clsWebDataManager")
'      Set root = pCurrentStatus.documentElement
'      Set lNewNode = pCurrentStatus.createElement("status_variables")
'      root.appendChild lNewNode
    End If
  Else
    Set lSaveStatus = New FastCollection
    Set lBaseNode = pCurrentStatus.GetChildWithTag("status_variables")
    If Not lBaseNode Is Nothing Then
      Set lOldNode = lBaseNode.FirstChild
      While Not lOldNode Is Nothing
        LogDbg "CurrentStatusFromFile:Save " & lOldNode.Tag 'lOldNode.GetXml
        lSaveStatus.Add lOldNode, lOldNode.Tag 'loldnode.Content
        Set lOldNode = lOldNode.NextSibling 'NextSibling2 would change what was added to lSaveStatus
      Wend
    End If
    LogDbg "CurrentStatusFromFile:Saved " & lSaveStatus.Count & " status variables b4 loading from file"
    
    pCurrentStatus.Clear  'jlk 4/21/04 to avoid dup keys
    pCurrentStatus.LoadXmlFile aFilename
    
    LogDbg "Loaded Current Status From File " & aFilename
    
    Set lBaseNode = pCurrentStatus.GetChildWithTag("status_variables")
    If lBaseNode Is Nothing Then
      Set lBaseNode = pCurrentStatus.newChild("status_variables", "")
    End If
    'Translate loaded state_cd into state_abbrev
    Set lNodesWithTag = GetChildrenWithTag(lBaseNode, "state_cd")
    For Each vNode In lNodesWithTag
      Set lNewNode = vNode
      s = CStr(lNewNode.GetAttrValue("status"))
      If Len(s) > 0 Then s = ", " & s
      lNewNode.AddAttribute "status", "From state_cd " & lBaseNode.content & s
      lNewNode.Tag = "state_abbrev"
      lNewNode.content = FIPS2Abbrev(lBaseNode.content)
      nthChild = nthChild + 1
    Next
        
    For Each vNode In lSaveStatus
      Set lOldNode = vNode
      s = Me.CurrentStatusGetString(lOldNode.Tag)
      If Len(s) = 0 Then
        If Len(lOldNode.content) > 0 Then
          LogDbg "Restoring value for '" & lOldNode.Tag & "'"
          CurrentStatusUpdateString lOldNode.Tag, lOldNode.content
        Else
          LogDbg "Not restoring value for '" & lOldNode.Tag & "' because we don't have an old value"
        End If
      Else
        LogDbg "Not restoring value for '" & lOldNode.Tag & "' because we have a new value"
      End If
    Next
        
  End If
  
  RememberDefault "project_dir"
  RememberDefault "basins_dir"
  RememberDefault "cache_dir"
  
  'If we weren't given a HUC, try getting HUCs from available states and/or counties
  If Len(CurrentStatusGetString("huc_cd")) = 0 Then
    s = "state_abbrev": GoSub AddHUCsOverlappingFIPS
    s = "county_cd":    GoSub AddHUCsOverlappingFIPS
    s = "county_fips":  GoSub AddHUCsOverlappingFIPS
  End If
  
  'If we weren't given a state, try getting state(s) from HUC(s) if available
  If Len(CurrentStatusGetString("state_abbrev")) = 0 Then
    Dim FIPSs As FastCollection
    Dim vFIPS As Variant
    Set lNodesWithTag = GetChildrenWithTag(lBaseNode, "huc_cd")
    For Each vNode In lNodesWithTag
      Set lOldNode = vNode
      If IsNumeric(lOldNode.content) Then
        Set FIPSs = GetFIPS(CLng(lOldNode.content), True, False, True)
        For Each vFIPS In FIPSs
          Abbrev = FIPS2Abbrev(vFIPS)
          If lBaseNode.SearchForContent(Nothing, "state_abbrev", Abbrev) Is Nothing Then
            Set lNewNode = lBaseNode.newChild("state_abbrev", Abbrev)
            lNewNode.AddAttribute "status", "set from HUC " & lOldNode.content
            LogDbg "Adding state_abbrev " & lNewNode.content & " (FIPS=" & vFIPS & ") from HUC " & lOldNode.content
            lBaseNode.AddChildTree lNewNode
          Else
            LogDbg "Already added state_abbrev " & Abbrev
          End If
        Next
      End If
      nthChild = nthChild + 1
    Next
    
  End If
  
  Exit Sub
  
AddHUCsOverlappingFIPS:
  Set lNodesWithTag = GetChildrenWithTag(lBaseNode, s)
  For Each vNode In lNodesWithTag
    Set lOldNode = vNode
    If IsNumeric(lOldNode.content) Then
      FIPS = CLng(lOldNode.content)
    Else
      FIPS = CLng(Abbrev2FIPS(lOldNode.content))
    End If
    If FIPS > 0 Then
      Set HUCs = GetHUCS(FIPS, True)
      For Each vHUC In HUCs
        If lBaseNode.SearchForContent(Nothing, "huc_cd", vHUC) Is Nothing Then
          Set lNewNode = lBaseNode.newChild("huc_cd", vHUC)
          lNewNode.AddAttribute "status", "set from FIPS " & lOldNode.content
          LogDbg "Adding huc_cd " & vHUC & " from FIPS " & lOldNode.content
        Else
          LogDbg "Already added huc_cd " & vHUC
        End If
      Next
    End If
  Next
  Return
  
ErrHand:
  LogMsg Err.Description, "Error in CurrentStatusFromFile"
End Sub

'If we don't already have a value for StatusVariable in CurrentStatus,
'try to assign a default value from the registry
Private Sub RememberDefault(ByVal aStatusVariable As String)
  Dim Value As String
  Value = Me.CurrentStatusGetString(aStatusVariable)
  If Len(Value) = 0 Then
    Select Case aStatusVariable
      Case "basins_dir":              Value = gAppPath & "\"
      Case "cache_dir", "save_dir":   Value = gCacheDir
      Case Else: Value = GetSetting(cAppName, "defaults", aStatusVariable, "")
    End Select
    If Len(Value) > 0 Then
      CurrentStatusUpdateString aStatusVariable, Value, "default"
    End If
  End If
End Sub

Public Function CurrentStatusGetString(ByVal aStatusVariable As String, _
                                    Optional Default As String = "") As String
  Dim lNode As ChilkatXml
  Dim lStatusVariable As String
  Dim retval As String
  
  retval = Default
'  If Len(Default) > 0 Then
'    Debug.Print "     <Variable Name=""" & lStatusVariable & """>" & Default & "</Variable>"
'  End If
  
  On Error GoTo ErrHand
    
  lStatusVariable = LCase(aStatusVariable)
  If lStatusVariable = "state_cd" Then lStatusVariable = "state_abbrev"

  Set lNode = pCurrentStatus.GetChildWithTag("status_variables")
  If Not lNode Is Nothing Then 'We expect to have a status_variables node
    If lNode.FirstChild2 = 0 Then Set lNode = Nothing
    While Not lNode Is Nothing
      If LCase(lNode.Tag) = lStatusVariable Then
        If Len(lNode.content) > 0 Then
          retval = lNode.content
          GoTo FoundValue
        End If
      End If
      If lNode.NextSibling2 = 0 Then Set lNode = Nothing
    Wend
  End If
FoundValue:
  
  'Add trailing backslash if needed
  If Right(lStatusVariable, 4) = "_dir" Then
    If Len(retval) > 0 Then
      If Not IsNumeric(retval) Then
        If Right(retval, 1) <> "\" Then retval = retval & "\"
      End If
    End If
  End If
  
  If aStatusVariable = "state_cd" Then
    retval = Abbrev2FIPS(retval)
  End If
  
  CurrentStatusGetString = retval
  
  Exit Function
  
ErrHand:
  LogMsg "Error getting value of '" & lStatusVariable & "' in status." _
                 & vbCr & Err.Description, "Web Data Manager:CurrentStatusGetString"
End Function

Public Sub CurrentStatusUpdateString(ByVal aStatusVariable As String, _
                                     ByVal aValue As String, _
                                  Optional aStatus As String = "")
  Dim lOldNode As ChilkatXml
  Dim lNewNode As ChilkatXml
  Dim lBaseNode As ChilkatXml
  Dim lStatusVariable As String
  Dim Value As String
  Dim nthChild As Long
  
  On Error GoTo ErrHand
  
  Set lBaseNode = pCurrentStatus.GetChildWithTag("status_variables")
  Value = aValue
  lStatusVariable = LCase(aStatusVariable)
  If Right(lStatusVariable, 4) = "_dir" Then
    If Len(Value) > 0 Then
      If Right(Value, 1) <> "\" Then Value = Value & "\"
    End If
  End If

  LogDbg "CurrentStatusUpdateString('" & lStatusVariable & "', '" & Value & "', " & aStatus & "')"
  
  If lStatusVariable = "state_cd" Then
    lStatusVariable = "state_abbrev"
    Value = FIPS2Abbrev(aValue)
    LogDbg "Saving state_cd as state_abbrev('" & Value & "')"
  End If
  
  RemoveChildrenWithTag lBaseNode, lStatusVariable
    
  Set lNewNode = lBaseNode.newChild(lStatusVariable, Value)
  If Len(aStatus) > 0 Then lNewNode.AddAttribute "status", aStatus
  
  If LCase(Right(lStatusVariable, 4)) = "_dir" Then
    SaveSetting cAppName, "defaults", LCase(lStatusVariable), Value
  End If
  
  Exit Sub
  
ErrHand:
  LogMsg "Error updating string '" & Value & "' in status." & vbCr & Err.Description, "Web Data Manager:CurrentStatusUpdateString"
End Sub

Public Function CurrentStatusGetList(ByVal aStatusVariable As String) As FastCollection
  Dim lNode As ChilkatXml
  Set lNode = pCurrentStatus.GetChildWithTag("status_variables")
  Set CurrentStatusGetList = GetChildrenWithTag(lNode, aStatusVariable)
End Function

Public Sub CurrentStatusUpdateList(aStatusVariable As String, aNewValues As FastCollection, Optional aStatus As String = "")
  Dim vOldNode As Variant
  Dim lNode As ChilkatXml
  Dim vNewValue As Variant
  Dim lBaseNode As ChilkatXml
  Dim lStatusVariable As String
  
  On Error GoTo ErrHand
  
  Set lBaseNode = pCurrentStatus.GetChildWithTag("status_variables")
  lStatusVariable = LCase(aStatusVariable)
    
  RemoveChildrenWithTag lBaseNode, lStatusVariable
    
  For Each vNewValue In aNewValues
    If VarType(vNewValue) = vbObject Then 'passed in a ChilkatXML
      lBaseNode.AddChildTree vNewValue
    Else                                  'passed in a simple type to be made into a ChilkatXML
      Set lNode = lBaseNode.newChild(lStatusVariable, CStr(vNewValue))
      If Len(aStatus) > 0 Then lNode.AddAttribute "status", aStatus
    End If
  Next
  
  Exit Sub
  
ErrHand:
  LogMsg "Error updating list in status." & vbCr & Err.Description, "Web Data Manager:CurrentStatusUpdateList"
End Sub

Public Property Get DataTypes() As FastCollection
  Set DataTypes = pDataTypes
End Property
Public Function DataTypeFromLabel(myLabel As String) As clsWebData
  Dim vDataType As Variant
  
  Set DataTypeFromLabel = pDataTypes.ItemByIndex(1) 'Default to first type (BASINS data)
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

'Collection of labels of classes providing the specified attribute
Public Function Provides(Attr As String) As FastCollection
  Dim lProvides As FastCollection
  Dim vDataType As Variant
  Dim lDataType As clsWebData
  Dim lAvailable As FastCollection
  Dim vAvailable As Variant
  Dim nodAvailable As ChilkatXml
 
  Set lProvides = Nothing
  Set lProvides = New FastCollection
  
  For Each vDataType In pDataTypes
    Set lDataType = vDataType
    Set lAvailable = GetChildrenWithTag(lDataType.Provides, "available")
    For Each vAvailable In lAvailable
      Set nodAvailable = vAvailable.SearchForTag(Nothing, Attr)
      If Not nodAvailable Is Nothing Then lProvides.Add lDataType.Label
    Next
    Set lAvailable = Nothing
  Next
  
  Set Provides = lProvides
End Function

Public Sub ShowTree(aTree As Variant, TreeCaption As String)
'  If VarType(aTree) = 9 Then
'    Set frmXMLTree.Tree = aTree
'  Else
'    Set frmXMLTree.Tree = New ChilkatXml
'    frmXMLTree.Tree.LoadXml aTree
'  End If
'  On Error Resume Next
'  frmXMLTree.Caption = TreeCaption & " XML Tree"
'  frmXMLTree.BuildTree
'  frmXMLTree.Show vbModal
End Sub

Public Property Let DownloadStatus(newStatus As String)
  If Not pWizard Is Nothing Then pWizard.DownloadStatus = newStatus
  LogDbg "DownloadStatus = " & newStatus
  DoEvents
End Property

'Returns True if download was successful
Public Function Download(ByVal URL As String, _
                         ByVal Filename As String, _
                         RestartTimer As Boolean, _
                         Optional newCaption As String = "Downloading", _
                         Optional prependToOutput As String = "", _
                         Optional cacheFilename As String = "") As Boolean
  Dim URLfirstpart As String
  Dim URLdatapart As String
  Dim logFilename As String
  Dim URLfileName As String
  Dim TemporaryDownload As Boolean
  Dim msg As String
  Dim CmdLine As String
  Dim cacheFullPath As String
  Dim dispStatus As String
  Dim lastStatusUpdate As Single
  Dim curlExitCode As Long
  Dim curlExitCodePos As Long
  Dim useActiveMode As Boolean
  Dim stepname As String
  
  Static RecursionLevel As Long
  Static TryingFindData As Boolean
  
  If pState >= 999 Then Exit Function
  
  On Error GoTo ErrHandler
  
  LogDbg "  Download '" & URL & "' --> '" & Filename & "'"
  
  If pFormDownload Is Nothing Then
    stepname = "New frmDownload"
    LogDbg "  Set pFormDownload = New frmDownload"
    Set pFormDownload = New frmDownload
  End If
  
  If RecursionLevel = 0 Then TryingFindData = False
  
  RecursionLevel = RecursionLevel + 1
  
  pState = state_download
  pResultString = ""
  stepname = "checking for existing file"
  If Len(Filename) > 0 Then
    If FileExists(Filename) Then 'Assume already existing file is the one we were about to download
      If FileLen(Filename) > 0 Then
        LogDbg "  Download: File '" & Filename & "' found so downloading was skipped"
        pResultString = "Existing file found"
        stepname = "checking existing file length"
        If FileLen(Filename) < 10000 Then
          On Error Resume Next
          stepname = "loading existing file into ResultString"
          pResultString = WholeFileString(Filename)
        End If
        
        GoTo WrapUp
      
      End If
    End If
  End If
  
  stepname = "checking for cached file"
  If Len(cacheFilename) > 0 Then
    cacheFullPath = gCacheDir & cacheFilename
    If Not FileExists(cacheFullPath) Then
      MkDirPath PathNameOnly(cacheFullPath)
    Else
      If FileLen(cacheFullPath) > 0 Then
        Download = True
        If Len(Filename) > 0 Then
          FileCopy cacheFullPath, Filename
          LogDbg "  Download: Cached file '" & cacheFullPath & "' copied to '" & Filename & "'"
        Else
          pResultString = WholeFileString(cacheFullPath)
          LogDbg "  Download: Cached file '" & cacheFullPath & "' found and used"
        End If
        
        GoTo WrapUp
      
      End If
    End If
  End If
  
  If pWizard Is Nothing Then
    stepname = "ShowDownload"
    LogDbg "  ShowDownload"
    ShowDownload
  Else
    DownloadStatus = newCaption
  End If
  stepname = "pFormDownload.Clear"
  pFormDownload.Clear RestartTimer
  
  stepname = "pFormDownload.Caption"
  pFormDownload.Caption = newCaption
  
  'pFormDownload.prependToOutput = prependToOutput
  stepname = "pFormDownload.txtFrom.Text"
  pFormDownload.txtFrom.Text = URL
  
  stepname = "pFormDownload.txtTo.Text"
  pFormDownload.txtTo.Text = Filename
  
  stepname = "URLfileName = GetTmpFileName"
  URLfileName = GetTmpFileName
  stepname = "URLfileName = '" & URLfileName & "'"
  
  stepname = "logFilename = GetTmpFileName"
  logFilename = GetTmpFileName
  stepname = "logFilename = '" & logFilename & "'"
  
  If Len(Filename) = 0 Then
    Filename = GetTmpFileName
    pFormDownload.txtTo.Text = "Temporary file: " & Filename
    TemporaryDownload = True
  End If

TryAgain:
  CmdLine = pCurlExePath
  If useActiveMode Then
    CmdLine = CmdLine & " -P -"
  ElseIf InStr(URL, "nhdftp") > 0 Then 'special case for NHD FTP
    CmdLine = CmdLine & " --disable-epsv"
  End If
  CmdLine = CmdLine & " -o""" & Filename & """ --trace-ascii " & logFilename
  
  URLdatapart = URL
  stepname = "StrSplit URLdatapart"
  URLfirstpart = StrSplit(URLdatapart, "?", "")
  If Len(URLdatapart) > 0 Then
    stepname = "SaveFileString, Len(URLdatapart) = " & Len(URLdatapart)
    SaveFileString URLfileName, URLdatapart
    CmdLine = CmdLine & " -d@" & URLfileName & " " & URLfirstpart
  Else
    CmdLine = CmdLine & " " & URL
  End If
  
  'make sure destination directory exists
  If Not FileExists(PathNameOnly(Filename), True, False) Then
    LogDbg "Creating directory " & PathNameOnly(Filename)
    MkDirPath PathNameOnly(Filename)
  End If
  stepname = "Starting curl"
  LogDbg "Starting Process: " & CmdLine
  pIPC.StartProcess "curl", CmdLine

  Do
    DoEvents
    stepname = "ShowWin"
    ShowWin pCurlExePath, SW_HIDE, 0
    'Msg = WholeFileString(logfileName)
    'If Msg <> pFormDownload.txtStatus.Text Then
    '  pFormDownload.txtStatus.Text = Right(Msg, 1000)
    '  pFormDownload.txtStatus.SelStart = Len(pFormDownload.txtStatus.Text)
    'End If
    stepname = "GetProcessMessage"
    pResultString = pIPC.GetProcessMessage("curl", 0.1)
    If InStr(pResultString, "expired") = 0 And InStr(pResultString, "% Total") = 0 Then
      stepname = "Trim Mid pResultString"
      msg = Trim(Mid(pResultString, 15, 6))
      If Len(msg) > 0 Then
        If IsNumeric(msg) Or IsNumeric(Left(msg, Len(msg) - 1)) Then
          dispStatus = "For current file, " & msg & " bytes transferred"
          msg = Trim(Mid(pResultString, 10, 5))
          If IsNumeric(msg) Then
            If CLng(msg) > 0 And CLng(msg) < 100 Then
              dispStatus = dispStatus & " (" & msg & "%)"
            End If
          End If
        Else
          dispStatus = pResultString
        End If
      Else
        dispStatus = pResultString
      End If
      stepname = "pFormDownload.txtStatus.Text = dispStatus"
      pFormDownload.txtStatus.Text = dispStatus
      If Timer - lastStatusUpdate > 1 Then
        DownloadStatus = newCaption & vbCr & dispStatus
        lastStatusUpdate = Timer
      End If
    End If
    If pState > 1000 Then
      pFormDownload.Caption = "Cancelled"
    ElseIf pFormDownload.Caption = "Cancelled" Then
      pState = pState + 1000
    End If
    curlExitCodePos = InStr(pResultString, "exited with code")
  Loop While curlExitCodePos = 0 And pState < 999
  
  stepname = "curlExitCode"
  If curlExitCodePos > 0 Then
    curlExitCode = Trim(Mid(pResultString, curlExitCodePos + 17, 2))
  Else
    curlExitCode = pIPC.ExitProcess("curl")
  End If
  
  LogDbg "  Download " & pFormDownload.Caption _
                    & " " & pFormDownload.txtStatus.Text _
                    & " (cURL exit code = " & curlExitCode & ")"
  
  Select Case curlExitCode
    Case 0:  LogDbg "  cURL indicates successful download"
    Case 1:  LogDbg "  Unsupported protocol. This build of curl has no support for this protocol."
    Case 2:  LogDbg "  Failed to initialize."
    Case 3:  LogDbg "  URL malformat. The syntax was not correct."
    Case 4:  LogDbg "  URL user malformatted. The user-part of the URL syntax was not correct."
    Case 5:  LogDbg "  couldn 't resolve proxy. The given proxy host could not be resolved."
    Case 6:  LogDbg "  couldn 't resolve host. The given remote host was not resolved."
    Case 7:
      If Not useActiveMode And LCase(Left(URL, 3)) = "ftp" Then
        LogDbg "  Failed to connect to host using passive FTP. Trying again using active mode."
        useActiveMode = True
        GoTo TryAgain
      Else
        LogDbg "  Failed to connect to host."
      End If
    Case 8:  LogDbg "  FTP weird server reply. The server sent data curl couldn't parse."
    Case 9:  LogDbg "  FTP access denied. The server denied login."
    Case 10: LogDbg "  FTP user/password incorrect. Either one or both were not accepted by the server."
    Case 11: LogDbg "  FTP weird PASS reply. Curl couldn't parse the reply sent to the PASS request."
    Case 12: LogDbg "  FTP weird USER reply. Curl couldn't parse the reply sent to the USER request."
    Case 13: LogDbg "  FTP weird PASV reply, Curl couldn't parse the reply sent to the PASV request."
    Case 14: LogDbg "  FTP weird 227 format. Curl couldn't parse the 227-line the server sent."
    Case 15: LogDbg "  FTP can 't get host. Couldn't resolve the host IP we got in the 227-line."
    Case 16: LogDbg "  FTP can 't reconnect. Couldn't connect to the host we got in the 227-line."
    Case 17: LogDbg "  FTP couldn 't set binary. Couldn't change transfer method to binary."
    Case 18: LogDbg "  Partial file. Only a part of the file was transfered."
    Case 19: LogDbg "  FTP couldn 't download/access the given file, the RETR (or similar) command  failed."
    Case 20: LogDbg "  FTP write error. The transfer was reported bad by the server."
    Case 21: LogDbg "  FTP quote error. A quote command returned error from the server."
    Case 22: LogDbg "  HTTP  page not retrieved. The requested url was not found or returned another error" _
               & " with the HTTP error code being 400 or above." ' This  return code only appears if -f/--fail is used.
    Case 23: LogDbg "  Write error. Curl couldn't write data to a local filesystem or similar."
    Case 24: LogDbg "  Malformed user. User name badly specified."
    Case 25: LogDbg "  FTP couldn 't STOR file. The server denied the STOR operation, used for FTP uploading."
    Case 26: LogDbg "  Read error. Various reading problems."
    Case 27: LogDbg "  Out of memory. A memory allocation request failed."
    Case 28: LogDbg "  Operation timeout. The specified time-out period was reached according to the  conditions."
    Case 29: LogDbg "  FTP couldn 't set ASCII. The server returned an unknown reply."
    Case 30: LogDbg "  FTP PORT failed. The PORT command failed. Not all FTP servers support the PORT command."
    Case 31: LogDbg "  FTP couldn 't use REST. The REST command failed. This command is  used  for  resumed FTP transfers."
    Case 32: LogDbg "  FTP  couldn't use SIZE. The SIZE command failed. The command is an extension to the original FTP spec RFC 959."
    Case 33: LogDbg "  HTTP range error."
    Case 34: LogDbg "  HTTP post error. Internal post-request generation error."
    Case 35: LogDbg "  SSL connect error. The SSL handshaking failed."
    Case 36: LogDbg "  FTP bad download resume. Couldn't continue an earlier aborted download."
    Case 37: LogDbg "  file couldn't read file. Failed to open the file. Permissions?"
    Case 38: LogDbg "  LDAP cannot bind. LDAP bind operation failed."
    Case 39: LogDbg "  LDAP search failed."
    Case 40: LogDbg "  Library not found. The LDAP library was not found."
    Case 41: LogDbg "  Function not found. A required LDAP function was not found."
    Case 42: LogDbg "  Aborted by callback. An application told curl to abort the operation."
    Case 43: LogDbg "  Internal error. A function was called with a bad parameter."
    Case 44: LogDbg "  Internal error. A function was called in a bad order."
    Case 45: LogDbg "  Interface error. A specified outgoing interface could not be used."
    Case 46: LogDbg "  Bad password entered. An error was signaled when the password was entered."
    Case 47: LogDbg "  Too many redirects. When following redirects, curl hit the maximum amount."
    Case 48: LogDbg "  Unknown TELNET option specified."
    Case 49: LogDbg "  Malformed telnet option."
    Case 51: LogDbg "  The remote peer's SSL certificate wasn't ok"
    Case 52: LogDbg "  The server didn't reply anything, which here is considered an error."
    Case 53: LogDbg "  SSL crypto engine not found"
    Case 54: LogDbg "  Cannot set SSL crypto engine as default"
    Case 55: LogDbg "  Failed sending network data"
    Case 56: LogDbg "  Failure in receiving network data"
    Case 57: LogDbg "  Share is in use (internal error)"
    Case 58: LogDbg "  Problem with the local certificate"
    Case 59: LogDbg "  couldn 't use specified SSL cipher"
    Case 60: LogDbg "  Problem with the CA cert (path? permission?)"
    Case 61: LogDbg "  Unrecognized transfer encoding"
    Case 62: LogDbg "  Invalid LDAP URL"
    Case 63: LogDbg "  Maximum file size exceeded"
    Case Else: LogDbg "  Unknown exit code " & curlExitCode
  End Select
  
  If pState >= 999 Then
    pFormDownload.txtStatus.Text = "Cancelled"
    pResultString = "<undefined/>"
    KillFile Filename
  Else
    If BadDownloadedFile(Filename) Then
      If InStr(URL, "finddata") = 0 And Not TryingFindData Then
        KillFile Filename
        TryingFindData = True
        LogDbg "Failed to download, trying finddata"
        Download = Download("http://hspf.com/cgi-bin/finddata.pl?url=" & URL, Filename, RestartTimer, newCaption, prependToOutput, cacheFilename)
      Else
        pResultString = pResultString & vbCrLf & "Could not download"
      End If
    Else
      If TemporaryDownload Then
        pResultString = WholeFileString(Filename)
        If Len(cacheFullPath) > 0 Then FileCopy Filename, cacheFullPath
        KillFile Filename
        LogDbg "  Download saved result and removed '" & Filename & "'"
        Filename = ""
      Else
        pResultString = "Downloaded " & FileLen(Filename) & " bytes"
        
        If Len(prependToOutput) > 0 Then
          pFormDownload.txtStatus.Text = "Prepending some data to file"
          pResultString = prependToOutput & WholeFileString(Filename)
          SaveFileString Filename, pResultString
          LogDbg "  Download prepended string to '" & Filename & "'"
          pFormDownload.txtStatus.Text = ""
        End If
        
        If Len(cacheFullPath) > 0 Then
          stepname = "Copy to cache"
          LogDbg "  Copying to cache '" & cacheFullPath & "'"
          FileCopy Filename, cacheFullPath
        End If
      
        If FileLen(Filename) < 10000 Then
          On Error Resume Next
          pResultString = WholeFileString(Filename)
        End If
      End If
    End If
  End If
  pFormDownload.txtStatus.Refresh
  On Error Resume Next
WrapUp:
  stepname = "WrapUp"
  LogDbg "  Wrapping up Download"
  If Len(logFilename) > 0 Then stepname = "Kill log file": KillFile logFilename
  If Len(URLfileName) > 0 Then stepname = "Kill URL file": KillFile URLfileName
  stepname = "Checking for 302 found"
  If InStr(pResultString, ">302 Found<") > 0 And RecursionLevel < 5 Then 'Redirected to another URL
    Dim newURL As String
    Dim hrefPos As Integer
    LogDbg "  Found 302 redirect"

    hrefPos = InStr(LCase(pResultString), "a href=")
    If hrefPos > 0 Then
      newURL = Mid(pResultString, hrefPos + 8)
      hrefPos = InStr(newURL, """")
      If hrefPos > 0 Then
        newURL = Left(newURL, hrefPos - 1)
        DownloadStatus = "Following web site redirection"
        LogDbg "  Deleting redirect " & Filename
        KillFile Filename
        If Len(cacheFullPath) > 0 Then KillFile cacheFullPath
        Download = Download(newURL, Filename, False, newCaption, prependToOutput, cacheFilename)
      Else
        LogDbg "  302 redirect failed to find closing quote"
      End If
    Else
      LogDbg "  302 redirect failed to find a href="
    End If
  ElseIf pResultString = "<undefined/>" Or Len(pResultString) = 0 Then
    LogDbg "  Download Incomplete " & pFormDownload.txtStatus.Text
    pFormDownload.txtStatus.Text = "Download Incomplete"
  Else
    Download = True
    LogDbg "  Download Complete " & pFormDownload.txtStatus.Text
    If Not pFormDownload Is Nothing Then pFormDownload.txtStatus.Text = ""
  End If
  RecursionLevel = RecursionLevel - 1
'  If InStr(OpenClose, "close") > 0 Then
'    pFormDownload.Hide
'  Else
'    pFormDownload.Deactivate
'  End If

  Exit Function
  
ErrHandler:
  pResultString = ""
  
  'don't bother user with spurious error messages
  If InStr(Err.Description, "Out of memory") > 0 Then
    LogDbg "  Download Out of memory error suppressed (" & Err.Number & ") At step: " & stepname
    Resume Next
  Else
    LogDbg "  Download Error: " & Err.Description & " (" & Err.Number & ") At step: " & stepname
 
    Select Case LogMsg(Err.Description & vbCrLf & "At step " & stepname, "Download Error", "-+&Abort", "&Retry", "&Ignore")
      Case 1              'Abort
        pResultString = "<undefined/>"
        If Not pFormDownload Is Nothing Then pFormDownload.txtStatus.Text = "Download Aborted"
        Resume WrapUp
      Case 2: Resume      'Retry
      Case 3: Resume Next 'Ignore
    End Select
  End If
End Function

Private Function BadDownloadedFile(aFilename As String) As Boolean
  BadDownloadedFile = False 'Assume file is ok unless it fails a test later
  If Not FileExists(aFilename) Then
    LogDbg "BadDownloadedFile, does not exist: " & aFilename
    BadDownloadedFile = True
  ElseIf FileLen(aFilename) < 100000 Then 'Check small files for common error messages
    Dim content As String
    content = WholeFileString(aFilename)
    If InStr(content, "was not found") > 0 Then 'found in 404 message or EPA's custom msg
      LogDbg "BadDownloadedFile, not found on server: " & aFilename
      BadDownloadedFile = True
    ElseIf InStr(content, "File Not Found") > 0 Then 'found in 404 message or EPA's custom msg
      LogDbg "BadDownloadedFile, not found on server: " & aFilename
      BadDownloadedFile = True
    End If
  End If
End Function

Private Sub KillFile(Filename As String, Optional SecondsToWait As Single = 5)
  Dim startTime As Single
  startTime = Timer
  If FileExists(Filename) Then
    On Error GoTo ErrHand
    LogDbg "Removing temporary file " & Filename
WhileLoop:
    While FileExists(Filename)
      Kill Filename
    Wend
    LogDbg "Removed temporary file " & Filename
  Else
    LogDbg "Temporary file already not there " & Filename
  End If
  Exit Sub

ErrHand:
  If Timer - startTime > SecondsToWait Or Timer < startTime Then
    DownloadStatus = "Timed out after " & SecondsToWait & " seconds waiting to remove temporary file " & Filename & ":" & Err.Description
    Exit Sub
  End If
  DoEvents
  Sleep 100
  Resume WhileLoop
End Sub

Public Property Get Version(Optional Sp = 0) As String
  Dim vDataType As Variant
  
  Version = App.Major & "." & App.Minor & "." & App.Revision
  
  If Not pDataTypes Is Nothing Then
    For Each vDataType In pDataTypes
      Version = Version & vbCrLf & Space(Sp) & vDataType.Label & " " & vDataType.Version
    Next
  End If
End Property

Private Sub Class_Initialize()
  Initialize_All
End Sub

Private Sub Initialize_All()
  Dim lWebData As clsWebData
  Dim lDataTypes As Variant
  Dim windir As String
  Dim Filename As String
  Dim Filenames() As String
  Dim i As Long
  Dim clsname As String
  Dim TriedAgain As Boolean
  Dim ff As New ATCoFindFile
    
  On Error GoTo ErrGeneral
    
  Set pMsg = New ATCoMessage

  'Look for C:\BASINS
  ff.SetDialogProperties "Please locate DataDownload.exe (expected in \BASINS\etc\DataDownload)", CurDir & "\DataDownload.exe"
  ff.SetRegistryInfo cAppName, "files", "app"
  gAppPath = ff.GetName
  If FileExists(gAppPath, True) Then
    If InStr(gAppPath, "\etc\") > 0 Then
      gAppPath = Left(gAppPath, InStr(gAppPath, "\etc\") - 1)
      SaveSetting cAppName, "files", "app", gAppPath
    End If
  End If
  
  gCacheDir = gAppPath & "\cache\"
  MkDirPath gCacheDir
  
  Set pIPC = New ATCoIPC
  pIPC.SendMonitorMessage "Caption BASINS Web Data Download"
  pLogPath = gCacheDir & Format(Date, "log\\yyyy-mm-dd") & Format(Time, "atHH-MM") & "_download.txt"
  pIPC.SendMonitorMessage "LogToFile " & pLogPath

'  Set pLogger = New clsATCoLogger
'  With pLogger
'    .SetFileName gCacheDir & Format(Date, "log\\yyyy-mm-dd") & Format(Time, "atHH-MM") & "_download.txt", False
'    .DateTime = True
'    .Log "Initialize_All Version " & Me.Version
'    Set .Icon = frmAbout.Icon
'  End With
      
  ff.SetRegistryInfo "curl", "paths", "EXEpath"
  ff.SetDialogProperties "Please locate curl.exe", gAppPath & "\etc\DataDownload\curl.exe"
  pCurlExePath = ff.GetName   'path = "c:\program files\Aqua_Terra\curl.exe"
    
  If Not pDataTypes Is Nothing Then pDataTypes.Clear
  Set pDataTypes = Nothing
  Set pDataTypes = New FastCollection
  
  Set pCurrentStatus = Nothing
  Set pCurrentStatus = New ChilkatXml
  pCurrentStatus.Tag = "clsWebDataManager"
  pCurrentStatus.NewChild2 "status_variables", ""
  
  CurrentStatusFromFile ""
  CurrentStatusUpdateString "basins_dir", gAppPath & "\", "default"
  CurrentStatusUpdateString "save_dir", gCacheDir, "default"
  CurrentStatusUpdateString "cache_dir", gCacheDir, "default"
  
  ff.SetRegistryInfo cAppName, "files", "WebDataManager.chm"
  ff.SetDialogProperties "Please locate WebDataManager.chm", gAppPath & "\Docs\WebDataManager.chm"
  App.HelpFile = ff.GetName

  Filenames = GetWebDataFileNames
  LogDbg "Initialize_All:Filenames " & UBound(Filenames)
  For i = 1 To UBound(Filenames)
    Filename = Filenames(i)
    clsname = Mid(Filename, 11)
    LogDbg "Initialize_All:Class " & clsname
        
    ReadComponentVariables gAppPath & "\etc\DataDownload\" & Filename & ".xml"
    
    On Error GoTo ErrCreate
    Set lWebData = CreateObject(Filename & ".cls" & clsname)
    On Error GoTo ErrGeneral
    Set lWebData.Manager = Me
    pDataTypes.Add lWebData, clsname
NextFile:
    Set lWebData = Nothing
  Next
  
  'AutomaticCheckForUpdates
  
  Exit Sub
  
ErrCreate:
  LogMsg "Error creating " & Filename & ".cls" & clsname & vbCr & "Could not open " & Filename, _
                 "ATCWebDataManager:Initialize_All"
  Resume NextFile
  
ErrGeneral:
  LogMsg "Error " & Err.Description & vbCr & " while initializing Web Data Manager", _
                 "ATCWebDataManager:Initialize_All"
End Sub

Private Function GetWebDataFileNames() As String()
  Dim verString As String
  Dim Filename As String
  Dim Filenames() As String
  Dim nFilenames As Long
  Dim i As Long
  Dim j As Long
  
  LogDbg "GetWebDataFileNames"
  ReDim Filenames(1000)
  Filename = Dir(gAppPath & "\etc\DataDownload\ATCWebData*.dll")
  
  While Len(Filename) > 0
    Filename = gAppPath & "\etc\DataDownload\" & Filename
    
    verString = GetFileVerString(Filename)
    While Right(verString, 2) = ".0"
      verString = Left(verString, Len(verString) - 2)
    Wend
    
    LogDbg "GetWebDataFileNames File=""" & FilenameNoPath(Filename) _
      & """ Version=""" & verString & """ Date=""" _
      & Format(FileDateTime(Filename), "MM/DD/YYYY hh:mm:ss am/pm") _
      & """ Size=""" & Format(FileLen(Filename), "#,###") & """"
    
    Filename = FilenameOnly(Filename)
    i = 1
    While i <= nFilenames
      If Filenames(i) > Filename Then
        For j = nFilenames To i Step -1
          Filenames(j + 1) = Filenames(j)
        Next
        Filenames(i) = Filename
        i = nFilenames + 2
      Else
        i = i + 1
      End If
    Wend
    If i = nFilenames + 1 Then Filenames(i) = Filename
    nFilenames = nFilenames + 1
    Filename = Dir
  Wend
  ReDim Preserve Filenames(nFilenames)
  LogDbg "GetWebDataFileNames:Exit"
  GetWebDataFileNames = Filenames
End Function

'Get status variables from XML associated with a component
Public Sub ReadComponentVariables(Filename As String)
  Dim lLocalDoc As New ChilkatXml
  Dim lNode As ChilkatXml
  Dim child As ChilkatXml
  Dim VariableName As String
  Dim lNodeChild As ChilkatXml
  
  On Error GoTo ErrHand
  
  If FileExists(Filename) Then
    LogDbg "  ReadComponentVariables:File '" & Filename & "'"
    lLocalDoc.LoadXml WholeFileString(Filename)
    If Len(lLocalDoc.ErrorLogText) <> 0 Then
      LogMsg "Parse error reading """ & Filename & """" & vbCr & lLocalDoc.ErrorLogText, "WebDataManager:ReadComponentVariables"
    Else
      Set lNode = lLocalDoc.SearchForAttribute(Nothing, "Component", "Name", FilenameOnly(Filename))
      If Not lNode Is Nothing Then
        Set child = lNode.FirstChild
        While Not child Is Nothing
          If LCase(child.Tag) = "variable" Then
            VariableName = child.GetAttrValue("Name")
            Me.CurrentStatusUpdateString VariableName, child.content
          End If
NextVariable:
          If child.NextSibling2 = 0 Then Set child = Nothing
        Wend
      End If
    End If
  Else
    LogDbg "  ReadComponentVariables:File '" & Filename & "' not found"
  End If
  
  Exit Sub
  
ErrHand:
  Resume NextVariable
End Sub

'CheckForUpdates returns True if updates were fetched and need to be installed
Public Function CheckForUpdates(URL As String, appPath As String, Optional postUpdateCommand As String = "") As Boolean
'Code moved to UpdateCheck.exe, have to leave this function here for binary compatibility
End Function

Private Sub AddRequires(aNode As ChilkatXml, Requires As FastCollection)
  Dim ComponentName As String
  Dim RequiredName As String
  Dim RequiredVersion As String
  Dim requiresChildren As FastCollection
  Dim childNode As ChilkatXml
  Dim vChild As Variant
  
  ComponentName = aNode.GetAttrValue("Name")
  On Error GoTo ErrHand
  
  Set requiresChildren = GetChildrenWithTag(aNode, "requires")
  For Each vChild In requiresChildren
    Set childNode = vChild
    RequiredName = childNode.GetAttrValue("Name")
    RequiredVersion = childNode.GetAttrValue("Version")
    Requires.Add ComponentName & " requires " & RequiredName & " " & RequiredVersion
NextChild:
  Next
  
  Exit Sub

ErrHand:
  LogDbg "AddRequires Error: " & Err.Description & " adding " & ComponentName
  Resume NextChild
End Sub

Public Function StartProcess(Name As String, CmdLine As String, Optional Wait4Completion As Boolean = True) As String
  Dim lWait As Single

  If Wait4Completion Then
    lWait = 300  'ask for permission to continue after 5 minutes (300 secs)
  Else
    lWait = 0
  End If
  LogDbg "WebDataManager StartProcess of '" & CmdLine & "' as '" & Name & "' with wait of " & lWait
  pIPC.StartProcess Name, CmdLine, lWait
  LogDbg "WebDataManager StartProcess Complete"
End Function

Public Sub AppendProjectXML(newXML As String)
  If InStr(gProjectXML, newXML) = 0 Then
    gProjectXML = gProjectXML & "  " & newXML & vbCrLf
  End If
End Sub

Public Sub ProjectDownloadedData()
  Dim basins_dir As String
  Dim project_xml As String
  Dim ProjectorXMLfilename As String
  Dim ProjectorAPRfilename As String
  Dim LaunchedByArcView As Boolean
  
  basins_dir = gAppPath & "\"
  project_xml = gProjectXML
  
  ProjectorXMLfilename = basins_dir & "Data\ATCProjector.xml"
  ProjectorAPRfilename = basins_dir & "etc\ATCProjector.apr"
  
  If Len(project_xml) = 0 Then
    LogDbg "No projection requested"
  Else
    Dim pXML As New ChilkatXml
    project_xml = "<projector>" & vbCrLf _
            & "  <project_dir>" & CurrentStatusGetString("project_dir") & "</project_dir>" & vbCrLf _
            & project_xml _
            & "</projector>"
    If Me.CurrentStatusGetString("launched_by") = "ArcView" Then
      LogDbg "Saving projector XML for ArcView"
      SaveFileString ProjectorXMLfilename, project_xml
    ElseIf Me.CurrentStatusGetString("launched_by") = "BASINS System Application" Then
      LogDbg "Saving projector XML for BASINS System Application"
      SaveFileString ProjectorXMLfilename, project_xml
    ElseIf pXML.LoadXml(project_xml) = 0 Then
      LogDbg "Unable to parse projector XML"
      SaveFileString ProjectorXMLfilename, project_xml
    Else
      ProcessProjectorXML pXML
      LogDbg "Writing '" & ProjectorXMLfilename & "' projection specification file"
      SaveFileString ProjectorXMLfilename, pXML.GetXml
    End If
  End If
End Sub

Private Sub ProcessProjectorXML(ByRef aXML As ChilkatXml)
  Dim theInputDirName As String
  Dim theOutputDirName As String
  Dim theOutputFileName As String
  Dim InputFileList As FastCollection
  Dim vFilename As Object
  Dim iFile As Long
  Dim curFilename As String
  Dim nextNode As ChilkatXml
  Dim curNode As ChilkatXml

  Debug.Print aXML.GetXml
  Set curNode = aXML.FirstChild
  
  While Not curNode Is Nothing
    LogDbg ("ProcessProjectorXML: " & curNode.GetXml)
    Set nextNode = curNode.NextSibling
    Select Case LCase(curNode.Tag)
      Case "add_shape"
      Case "add_allshape"
      Case "projector"
          ProcessProjectorXML curNode
      Case "project_dir"
      Case "convert_shape"
          theOutputFileName = curNode.GetAttrValue("output")
          curFilename = curNode.content
          ShapeUtilMerge curFilename, theOutputFileName, CurrentStatusGetString("project_dir") & "prj.proj"
          curNode.RemoveFromTree 'Did this, take it out of the instructions now
      Case "convert_coverage"
      Case "convert_grid"
      Case "convert_dir" 'loop through a directory, projecting all files in it
          theInputDirName = curNode.content
          theOutputDirName = curNode.GetAttrValue("output")
          'Remove trailing slash from theInputDirName and theOutputDirName
          If Right(theInputDirName, 1) = "\" Then theInputDirName = Left(theInputDirName, Len(theInputDirName) - 1)
          If Right(theOutputDirName, 1) = "\" Then theOutputDirName = Left(theOutputDirName, Len(theOutputDirName) - 1)

          Set InputFileList = Nothing
          Set InputFileList = New FastCollection
          AddFilesInDir InputFileList, theInputDirName, False, "*.shp"

          For iFile = 1 To InputFileList.Count
            curFilename = InputFileList.ItemByIndex(iFile)
            If (FileExt(curFilename) = "shp") Then
              'this is a shapefile
              theOutputFileName = theOutputDirName & "\" & FilenameNoPath(curFilename)
              'change projection and merge
              If (FileExists(theOutputFileName) And (InStr(1, theOutputFileName, "\landuse\") > 0)) Then
                'if the output file exists and it is a landuse shape, dont bother
              Else
                ShapeUtilMerge curFilename, theOutputFileName, CurrentStatusGetString("project_dir") & "prj.proj"
              End If
            End If
          Next
          curNode.RemoveFromTree 'Did this, take it out of the instructions now

      Case Else
          LogMsg "Unknown directive:" & vbCr & curNode.GetXml, "ProcessProjectorXML"
    End Select
    Set curNode = nextNode
  Wend
End Sub

Private Sub ShapeUtilMerge(ByVal aCurFilename As String, ByVal aOutputFilename As String, ByVal aProjectionFilename As String)
  Dim exe As String
  exe = "\basins\etc\datadownload\ShapeUtil.exe"
'  exe = FindFile("Please locate ShapeUtil.exe", "\basins\etc\datadownload\ShapeUtil.exe")
  If Len(exe) > 0 Then
'    Dim layersDBF As String
'    layersDBF = GetSetting("ShapeMerge", "files", "layers.dbf")
'    If Not FileExists(layersDBF) Then
'        LogDbg ("Did not find layers.dbf in registry " & layersDBF)
'        layersDBF = PathNameOnly(exe) & "\layers.dbf"
'        If FileExists(layersDBF) Then
'            LogDbg ("Saving layers.dbf location for ShapeUtil: " & layersDBF)
'            SaveSetting("ShapeMerge", "files", "layers.dbf", layersDBF)
'        Else
'            LogDbg ("Did not find layers.dbf in same path as ShapeUtil " & layersDBF)
'        End If
'    Else
'        LogDbg ("Found " & layersDBF)
'    End If
    LogDbg "Merging " & aCurFilename & vbCrLf _
         & "into " & aOutputFilename
    pIPC.StartProcess "ShapeMerge", exe & " """ & aOutputFilename & """ """ & aCurFilename & """ """ & aProjectionFilename & """", 120, 240
  Else
    LogDbg "Failed to find ShapeUtil.exe for merging " & aCurFilename & " into " & aOutputFilename
  End If
End Sub

