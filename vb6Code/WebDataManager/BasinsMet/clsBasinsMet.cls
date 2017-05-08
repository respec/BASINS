VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsBasinsMet"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
Implements clsWebData

Private Const pLabel = "BASINS Meteorological WDM"
Private Const pShortLabel = "BASINS Met Data"
Private Const pClassName = "clsBasinsMet"
Private Const pHelpID = 25
Private pProvides As ChilkatXml
Private pManager As clsWebDataManager
Private pCriteria As ChilkatXml

Private pErrorDescription As String
'http://www.epa.gov/waterscience/ftp/basins/wdm_data/AK_wdm.exe

Private Sub Class_Initialize()
  Dim nod1 As ChilkatXml
  Dim nod2 As ChilkatXml
  
  Set pProvides = New ChilkatXml
  pProvides.Tag = pClassName
  Set nod1 = pProvides.newChild("output", "")
  Set nod2 = nod1.newChild("file", "")
      nod2.AddAttribute "format", "exe"
  
  Set pCriteria = pProvides.newChild("criteria", "")
  Set nod2 = pCriteria.newChild("state_abbrev", "")
      nod2.AddAttribute "label", "State 2-letter abbreviation"
      nod2.AddAttribute "multiple", "true"
          
  Set nod2 = Nothing
End Sub

Private Function clsWebData_ErrorDescription() As String
  clsWebData_ErrorDescription = pErrorDescription
  pErrorDescription = ""
End Function

Private Function clsWebData_GetData(ByVal aQuery As ChilkatXml, aResult As ChilkatXml) As Boolean
  Dim lQueryChild As ChilkatXml
  Dim lCritChild As ChilkatXml
  Dim url As String
  Dim urlEnd As String
  Dim state_cd As String
  Dim state_abbrev As String
  Dim save_dir As String
  Dim Filename As String
  Dim ichild As Long, jchild As Long
  Dim FirstFile As Boolean
  Dim project_dir As String
  Dim basins_dir As String
  Dim Projector As String
  Dim Msg As String
  Dim BuildEXE As String
  Dim AVExe As String
  
  Dim lstStates As FastCollection
  Dim vState As Variant
  Dim iState As Long 'Index of current state for download progress indicator
  Dim nodStatus As ChilkatXml
  Dim nDownloads As Long 'Total number of files to download this time for this data type
  Dim iDownload As Long  'index of current file being downloaded. Max will be nDownloads
  Dim StepName As String

  On Error GoTo ErrHand

  StepName = "basins_dir"
  basins_dir = pManager.CurrentStatusGetString("basins_dir", "C:\BASINS\")
  If UCase(Left(basins_dir, 5)) = UCase("c:\vb") Then 'exception for development environment
    basins_dir = "C:\BASINS\"
  End If
  save_dir = basins_dir & "data\met_data\"
  
  pManager.LogDbg "  " & pClassName & " GetData entry"
      
  StepName = "BASINSmetURL"
  url = pManager.CurrentStatusGetString("BASINSmetURL", "http://www.epa.gov/waterscience/ftp/basins/wdm_data/")
  urlEnd = pManager.CurrentStatusGetString("BASINSmetURLend", "_wdm.exe")
  If Right(url, 1) <> "/" Then url = url & "/"

  pManager.LogDbg "  " & pClassName & " GetData url '" & url & "'"
  pManager.LogDbg "  " & pClassName & " GetData dest '" & save_dir & "'"
  
  StepName = "aResult.Tag = pClassName"
  aResult.Tag = pClassName
  Set nodStatus = aResult.newChild("status", "")

  FirstFile = True
  StepName = "getElementsByTagName"
  Set lstStates = GetChildrenWithTag(aQuery.GetChildWithTag("criteria"), "state_abbrev")
  nDownloads = lstStates.Count
  For Each vState In lstStates
    iState = iState + 1
    state_abbrev = vState.Content
    If Len(Trim(state_abbrev)) = 0 Then
      pManager.LogDbg pClassName & " GetData: Empty state abbreviation skipped"
    Else
      Filename = state_abbrev & urlEnd
      If FileExists(save_dir & state_abbrev & ".wdm") Then
        pManager.LogDbg "Already have file '" & save_dir & state_abbrev & ".wdm'" _
                   & " - skipping download of " & "'" & url & Filename & "'"
      Else
        iDownload = iDownload + 1
        StepName = "download " & iDownload
        If Not pManager.download(url & Filename, _
                                 save_dir & Filename, _
                                 FirstFile, _
                                "Downloading " & pShortLabel & " " & Filename & " (" & iDownload & " of " & nDownloads & ")") Then
          nodStatus.AddAttribute "message", "User Cancelled"
          pManager.LogDbg "  " & pClassName & " User Cancelled"
                
          Exit Function '!!!!!!!!!!!!!!!!!!!
        End If
        
        'unpack to download directory
        StepName = "unpack " & save_dir & Filename
        pManager.StartProcess "Unzip" & Left(Filename, 6), _
                              save_dir & Filename & " /auto " & save_dir
        If FileExists(save_dir & state_abbrev & ".wdm") Then
          pManager.LogDbg "Successfully unpacked '" & save_dir & state_abbrev & ".wdm'"
          pManager.LogDbg "Deleting '" & save_dir & Filename & "'"
          'delete downloaded file
          If FileExists(save_dir & Filename) Then Kill save_dir & Filename
        Else
          pManager.LogDbg "Failed to unpack '" & save_dir & state_abbrev & ".wdm' from " _
                                      & "'" & save_dir & Filename & "'"
        End If
        StepName = "FirstFile = False"
        FirstFile = False
      End If
    End If
  Next
  
  pManager.LogDbg "  " & pClassName & " GetData exit"
  nodStatus.AddAttribute "complete", "true"
  clsWebData_GetData = True
  
  Exit Function
  
ErrHand:
  pManager.LogMsg StepName & vbCrLf & Err.Description, pClassName & " GetData"
  nodStatus.AddAttribute "complete", "false"
  clsWebData_GetData = False
End Function

Private Property Get clsWebData_HelpId() As Long
  clsWebData_HelpId = pHelpID
End Property

Private Property Get clsWebData_Label() As String
  clsWebData_Label = pLabel
End Property

Private Property Set clsWebData_Manager(myManager As clsWebDataManager)
  Set pManager = Nothing
  Set pManager = myManager
End Property

Private Property Get clsWebData_Manager() As clsWebDataManager
  Set clsWebData_Manager = pManager
End Property

Private Property Get clsWebData_Name() As String
  clsWebData_Name = pClassName
End Property

Private Property Get clsWebData_Provides() As ChilkatXml
  Set clsWebData_Provides = pProvides
End Property

Private Sub clsWebData_Specify(Optional parent As Object = Nothing, Optional ByVal Index As Long = 0)
  pManager.ShowCriteria Me, parent, Index
End Sub

Private Property Get clsWebData_Version() As String
  clsWebData_Version = App.Major & "." & App.Minor & "." & App.Revision
End Property

