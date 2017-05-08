VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsStoretVisit"
Attribute VB_GlobalNameSpace = True
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
Implements clsWebData

'Downloads data from STORET Data Warehouse http://www.epa.gov/STORET/dw_home.html

Private Const pLabel = "Modernized STORET"
Private Const pClassName = "clsStoretVisit"
Private Const pHelpID = 31
Private Const pNotFound = "try again"
Private pManager As clsWebDataManager
Private pProvides As ChilkatXml

Private Sub Class_Initialize()
  Dim nod1 As ChilkatXml
  Dim nod2 As ChilkatXml
  
  Set pProvides = New ChilkatXml
  pProvides.Tag = pClassName
  Set nod1 = pProvides.newChild("output", "")
  Set nod2 = nod1.newChild("file", "")
      nod2.AddAttribute "format", "csv"
  
  Set nod1 = pProvides.newChild("criteria", "")
  Set nod2 = nod1.newChild("storetstation", "")
      nod2.AddAttribute "multiple", "true"
      nod2.AddAttribute "label", "Storet Station"

  Set nod1 = Nothing
  Set nod2 = Nothing

End Sub

Private Function GetOrganizationKey(aOrgID As String) As String
  Static sOrgKeys As FastCollection 'Cache the organization keys for further use during this session
  
  If sOrgKeys Is Nothing Then
    Dim findPos As Long
    Dim findEnd As Long
    Dim allValues As String
    Dim curKey As String
    Dim curID As String
    
    'Unfortunately cannot depend on saving a cache file of this, all these keys changed during testing
    If Not pManager.Download("http://oaspub.epa.gov/storpubl/DW_resultcriteria_station", _
                             "", False, "Retrieving list of organizations") Then ', "", pClassName & "\DW_resultcriteria_station.html") Then
      Exit Function
    Else
      allValues = UCase(pManager.ResultString)
      findPos = InStr(allValues, "<SELECT NAME=""D_ORG_LIST""")
      If findPos = 0 Then
        pManager.LogDbg "GetOrganizationKey: Did not find D_ORG_LIST"
      Else
        allValues = Mid(allValues, findPos)
        findPos = InStr(allValues, "</SELECT>")
        If findPos > 0 Then
          allValues = Left(allValues, findPos)
          Set sOrgKeys = New FastCollection
          findPos = InStr(allValues, " VALUE=")
          While findPos > 0
            findEnd = InStr(findPos + 1, allValues, ">")
            If findEnd = 0 Then
              findPos = 0
            Else
              curKey = ReplaceString(Mid(allValues, findPos + 7, findEnd - findPos - 7), """", "")
              If curKey = "0" Then
                findPos = InStr(findPos + 1, allValues, " VALUE=")
              Else
                findPos = InStr(findEnd + 1, allValues, "~")
                If findPos > 0 Then
                  curID = Trim(Mid(allValues, findEnd + 1, findPos - findEnd - 1))
                  sOrgKeys.Add curKey, curID
                  'Debug.Print " Found Organization " & curID & " with key " & curKey
                  pManager.LogDbg "Found Organization " & curID & " with key " & curKey
                  findPos = InStr(findPos + 1, allValues, " VALUE=")
                End If
              End If
            End If
          Wend
        End If
      End If
    End If
  End If
  If Not sOrgKeys Is Nothing Then GetOrganizationKey = sOrgKeys.ItemByKey(UCase(aOrgID))
End Function

Private Function GetStationKey(aOrgID As String, aStationID As String) As String
  'http://oaspub.epa.gov/storpubl/DW_STATION_POPUP?v_StationString=10439&v_org=53&v_searchtype=ID&v_extref=STANDARD
  '<option value =75822>21FLGW  ~10439          ~N/A~N/A~SRB-LS-1007

  'Commented out cache filename since we probably can't depend on caching station key
  '(org IDs can change, so these probably can too)
  If Not pManager.Download(pManager.CurrentStatusGetString("StoretVisitBaseStationKeyURL", _
                          "http://oaspub.epa.gov/storpubl/DW_STATION_POPUP?v_StationString=") _
                          & aStationID & "&v_org=" & GetOrganizationKey(aOrgID) & "&v_searchtype=ID&v_extref=STANDARD", _
    "", False, "Retrieving Station Key for " & aOrgID & "-" & aStationID) Then ', "", pClassName & "\StationKey-" & aOrgID & "-" & aStationID & ".html") Then
    pManager.LogDbg "Could not open page to find station key for " & aOrgID & " " & aStationID
    Exit Function
  Else
    Dim findPos As Long
    Dim findEnd As Long
    Dim allValues As String
    allValues = UCase(pManager.ResultString)
    findPos = InStr(allValues, UCase(aStationID))
    If findPos = 0 Then
      pManager.LogDbg "GetStationKey: Did not find Station ID '" & aStationID & "' in " & pManager.ResultString
      Exit Function
    Else
      findEnd = InStrRev(allValues, ">", findPos)
      findPos = InStrRev(allValues, "=", findPos)
      If findPos > 0 And findEnd > 0 Then
        GetStationKey = Trim(Mid(allValues, findPos + 1, findEnd - findPos - 1))
      Else
        pManager.LogDbg "GetStationKey: Could not parse station key for '" & aStationID & "' in " & pManager.ResultString
      End If
    End If
  End If

End Function

Private Function BuildURL(aStationID As String, aOrgID As String)
  Dim url As String
  Dim StationKey As String
  
  'Debug.Print "OrgID: " & aOrgID
  'Debug.Print "StationID: " & aStationID
    
  StationKey = GetStationKey(aOrgID, aStationID)
  
  'Debug.Print "StationKey: " & StationKey

  If Len(StationKey) > 0 Then
    url = pManager.CurrentStatusGetString("StoretVisitBaseDataURL", _
         "http://oaspub.epa.gov/storpubl/DW_result_hub_custom?v_FromStmt=FROM FA_REGULAR_RESULT FA")
    url = url & pManager.CurrentStatusGetString("StoretVisitBeforeStationKeyURL", _
                "&v_mode=Online" _
              & "&v_WhereStmt=WHERE 1=1 AND fk_station IN (")
    url = url & StationKey
    url = url & pManager.CurrentStatusGetString("StoretVisitAfterStationKeyURL", _
                ")&v_ResultCount=" _
              & "&v_filters=~~Organization~Stations" _
              & "&v_rule_optimization=FALSE" _
              & "&v_report_type=REGULAR")
    url = url & pManager.CurrentStatusGetString("StoretVisitColumnsURL", _
                "&v_Columns=2" _
              & "&v_Columns=3" _
              & "&v_Columns=5" _
              & "&v_Columns=7" _
              & "&v_Columns=12" _
              & "&v_Columns=13" _
              & "&v_Columns=15" _
              & "&v_Columns=17" _
              & "&v_Columns=30" _
              & "&v_Columns=31" _
              & "&v_Columns=40" _
              & "&v_Columns=44" _
              & "&v_Columns=45" _
              & "&v_Columns=46" _
              & "&v_Columns=47" _
              & "&v_Columns=49" _
              & "&v_Columns=58")
    BuildURL = ReplaceString(url, " ", "%20")
  End If
End Function

Private Function ParseURLforData(aResult As String) As String
  Dim findPos As Long
  Dim findEnd As Long
  Dim url As String
  
  findPos = InStr(aResult, "Download your file now")
  If findPos = 0 Then
    pManager.LogDbg pClassName & " ParseURLforData could not find 'Download your file now'"
  Else
    findEnd = InStrRev(aResult, """", findPos)
    If findEnd > 0 Then
      findPos = InStrRev(aResult, """", findEnd - 1)
      If findPos > 0 Then ParseURLforData = Mid(aResult, findPos + 1, findEnd - findPos - 1)
    End If
  End If
End Function

Private Function clsWebData_GetData(ByVal aQuery As ChilkatXml, aResult As ChilkatXml) As Boolean
  Dim lStation As ChilkatXml
  Dim lStationID As String
  Dim lOrgID As String
  Dim storetstation As Variant
  Dim project_dir As String
  Dim FirstFile As Boolean
  Dim ErrMsg As String
  Dim Filename As String
  Dim FilesNotCreated As String
  Dim nFilesNotCreated As Long
  Dim FileNumber As Long
  Dim findPos As Long
  Dim msg As String
  Dim buf As String
  
  Dim root As ChilkatXml
  Dim nodStatus As ChilkatXml
  
  Dim lstSites As New FastCollection
  
  Dim url As String
  Dim endinstructions As String
  
  Dim DownloadNumber As Long
    
  pManager.LogDbg "  clsWebDataStoretVisit GetData entry"
  project_dir = pManager.CurrentStatusGetString("project_dir")

  clsWebData_GetData = True
  
  Dim iChild As Long, jChild As Long, iSite As Long
  
  aResult.Tag = pClassName
  Set nodStatus = aResult.newChild("status", "")
  
  If Len(aQuery.ErrorLogText) > 0 Then
    pManager.LogDbg pClassName & " Query Parse Error:" & aQuery.ErrorLogText
  Else
    Set lstSites = GetChildrenWithTag(aQuery.GetChildWithTag("criteria"), "storetstation")
    FirstFile = True
    For Each storetstation In lstSites
      DownloadNumber = DownloadNumber + 1
      Set lStation = storetstation
      lStationID = lStation.GetAttrValue("Station_ID")
      lOrgID = lStation.GetAttrValue("Org_ID")
      Filename = "STORET-" & lOrgID & "-" & lStationID
      msg = "Downloading " & pLabel & " " & lOrgID & "-" & lStationID & " (" & DownloadNumber & " of " & lstSites.Count & ")"
      If FileExists(project_dir & Filename) Then
        pManager.LogDbg "Already have file " & project_dir & Filename
      Else
        url = BuildURL(lStationID, lOrgID)
        If Not pManager.Download(url, "", False, msg) Then
          FilesNotCreated = FilesNotCreated & "   " & Filename & vbCrLf
        Else
          url = ParseURLforData(pManager.ResultString)
          If Len(url) = 0 Then
            FilesNotCreated = FilesNotCreated & "   " & Filename & vbCrLf
          ElseIf Not pManager.Download(url, project_dir & Filename, False, msg) Then
            FilesNotCreated = FilesNotCreated & "   " & Filename & vbCrLf
          ElseIf FileLen(project_dir & Filename) = 266 Then 'Empty file, just headers
            pManager.LogDbg "Removing empty STORET download: " & Filename
            Kill project_dir & Filename
            FilesNotCreated = FilesNotCreated & "   " & Filename & vbCrLf
          Else
            'TODO: Merge with existing data and/or convert to BASINS observed WQ format
            endinstructions = pManager.CurrentStatusGetString("endinstructions")
            If InStr(endinstructions, pLabel & " downloaded data is saved in tilde-delimited files") = 0 Then
              endinstructions = endinstructions & pLabel & " downloaded data is saved in tilde-delimited files" & vbCrLf & "in " & project_dir & ":" & vbCrLf & vbCrLf
            End If
            endinstructions = endinstructions & Filename & vbCrLf & vbCrLf
            pManager.CurrentStatusUpdateString "endinstructions", endinstructions
          End If
        End If
      End If
    Next
  End If
  
EndOfFunction:
  nFilesNotCreated = CountString(FilesNotCreated, vbCr)
  If nFilesNotCreated > 0 Then
    Filename = project_dir & "StoretNoData.txt"
    While Len(Dir(Filename)) > 0
      FileNumber = FileNumber + 1
      Filename = project_dir & "StoretNoData(" & FileNumber & ").txt"
    Wend
    If nFilesNotCreated > 10 Then
      findPos = 1
      For FileNumber = 1 To 10
        findPos = InStr(findPos + 1, FilesNotCreated, vbCr)
      Next
      msg = Left(FilesNotCreated, findPos) & " (and " & (nFilesNotCreated - 10) & " more)"
    Else
      msg = FilesNotCreated
    End If
    If pManager.logMsg("Did not create " & nFilesNotCreated & " files: " & vbCr & vbCr _
             & msg & vbCr _
             & "because there were no matching records in the database." & vbCr _
             & "Save this list to " & Filename & "?", _
             "Some data not in database", "Yes", "No") = 1 Then
      SaveFileString Filename, FilesNotCreated
    End If
  End If

  pManager.LogDbg "  clsWebDataStoretVisit GetData exit"

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

