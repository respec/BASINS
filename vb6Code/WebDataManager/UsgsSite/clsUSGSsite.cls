VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsUSGSsite"
Attribute VB_GlobalNameSpace = True
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
Implements clsWebData

Private Const pLabel = "USGS Site"
Private Const pClassName = "clsUSGSsite"
Private Const pDefaultColumns = "&column_name=site_no" _
              & "&column_name=station_nm" _
              & "&column_name=state_cd" _
              & "&column_name=county_cd" _
              & "&column_name=huc_cd" _
              & "&column_name=dec_lat_va" _
              & "&column_name=dec_long_va" _
              & "&column_name=alt_va" _
              & "&column_name=drain_area_va" _
              & "&column_name=contrib_drain_area_va" _
              & "&column_name=agency_cd"

Private pProvides As ChilkatXml

Private pManager As clsWebDataManager

Private Sub Class_Initialize()
  Dim nod1 As ChilkatXml
  Dim nod2 As ChilkatXml
  Dim nod3 As ChilkatXml
  
  Set pProvides = New ChilkatXml
  pProvides.tag = pClassName
  Set nod1 = pProvides.newChild("output", "")
    Set nod2 = nod1.newChild("XML", "")
        nod2.AddAttribute "type", "ChilkatXml"
'    Set nod2 = pProvides.createElement("file")
'                                                 nod1.appendChild nod2
'                                                 nod2.AddAttribute "format", "RDB"
'      Set nod3 = pProvides.createElement("group")
'                                                 nod2.appendChild nod3
'                                                 nod3.AddAttribute "name", "needforfile"
  Set nod1 = pProvides.newChild("criteria", "")
    Set nod2 = nod1.newChild("huc_cd", "")
        nod2.AddAttribute "group", "area"
        nod2.AddAttribute "multiple", "true"
        nod2.AddAttribute "label", "8-digit HUC"
    Set nod2 = nod1.newChild("obs_count_nu", "")
        nod2.AddAttribute "optional", "true"
        nod2.AddAttribute "label", "Minimum number of observations"
'    Set nod2 = pProvides.createElement("group")
'                                                 nod1.appendChild nod2
'                                                 nod2.AddAttribute "group", "area"
'                                                 nod2.AddAttribute "label", "Area"
'                                                 nod2.AddAttribute "multiple", "false"
'    Set nod2 = pProvides.createElement("state_abbrev")
'                                                 nod1.appendChild nod2
'                                                 nod2.AddAttribute "type", "State"
'                                                 nod2.AddAttribute "group", "area"
'                                                 nod2.AddAttribute "multiple", "true"
'                                                 nod2.AddAttribute "label", "State Abbreviation"
'    Set nod2 = pProvides.createElement("state_fips")
'                                                 nod1.appendChild nod2
'                                                 nod2.AddAttribute "type", "State"
'                                                 nod2.AddAttribute "group", "area"
'                                                 nod2.AddAttribute "multiple", "true"
'                                                 nod2.AddAttribute "label", "State FIPS Code"
'    Set nod2 = pProvides.createElement("huc_2")
'                                                 nod1.appendChild nod2
'                                                 nod2.AddAttribute "group", "area"
'                                                 nod2.AddAttribute "multiple", "true"
'                                                 nod2.AddAttribute "label", "2-digit HUC"
'    Set nod2 = pProvides.createElement("group")
'                                                 nod1.appendChild nod2
'                                                 nod2.AddAttribute "name", "needforfile"
'                                                 nod2.AddAttribute "label", "File Specifications"
  Set nod1 = pProvides.newChild("available", "")
    Set nod2 = nod1.newChild("site", "")
      Set nod3 = nod2.newChild("site_no", "")
          nod3.AddAttribute "name", "site number"
          nod3.AddAttribute "type", "string"
          nod3.AddAttribute "selected", "true"
      Set nod3 = nod2.newChild("qw_site_no", "")
          nod3.AddAttribute "name", "water quality site number"
          nod3.AddAttribute "type", "string"
          nod3.AddAttribute "selected", "true"
      Set nod3 = nod2.newChild("discharge_site_no", "")
          nod3.AddAttribute "name", "discharge site number"
          nod3.AddAttribute "type", "string"
          nod3.AddAttribute "selected", "true"
      Set nod3 = nod2.newChild("measurement_site_no", "")
          nod3.AddAttribute "name", "measurement site number"
          nod3.AddAttribute "type", "string"
          nod3.AddAttribute "selected", "true"
'      Set nod3 = pProvides.createElement("station_nm")
'                                                 nod2.appendChild nod3
'                                                 nod3.AddAttribute "name", "station name"
'                                                 nod3.AddAttribute "type", "string"
'                                                 nod3.AddAttribute "optional", "true"
'                                                 nod3.AddAttribute "selected", "true"
'      Set nod3 = pProvides.createElement("agency_cd")
'                                                 nod2.appendChild nod3
'                                                 nod3.AddAttribute "name", "agency code"
'                                                 nod3.AddAttribute "type", "string"
'                                                 nod3.AddAttribute "optional", "true"
'                                                 nod3.AddAttribute "selected", "false"
'      Set nod3 = pProvides.createElement("dec_lat_va")
'                                                 nod2.appendChild nod3
'                                                 nod3.AddAttribute "name", "latitude"
'                                                 nod3.AddAttribute "type", "double"
'                                                 nod3.AddAttribute "optional", "true"
'                                                 nod3.AddAttribute "selected", "true"
'      Set nod3 = pProvides.createElement("dec_long_va")
'                                                 nod2.appendChild nod3
'                                                 nod3.AddAttribute "name", "longitude"
'                                                 nod3.AddAttribute "type", "double"
'                                                 nod3.AddAttribute "optional", "true"
'                                                 nod3.AddAttribute "selected", "true"
'      Set nod3 = pProvides.createElement("state_cd")
'                                                 nod2.appendChild nod3
'                                                 nod3.AddAttribute "name", "state fips"
'                                                 nod3.AddAttribute "type", "string"
'                                                 nod3.AddAttribute "optional", "true"
'                                                 nod3.AddAttribute "selected", "false"
'      Set nod3 = pProvides.createElement("county_cd")
'                                                 nod2.appendChild nod3
'                                                 nod3.AddAttribute "name", "county fips"
'                                                 nod3.AddAttribute "type", "string"
'                                                 nod3.AddAttribute "optional", "true"
'                                                 nod3.AddAttribute "selected", "false"
'      Set nod3 = pProvides.createElement("alt_va")
'                                                 nod2.appendChild nod3
'                                                 nod3.AddAttribute "name", "altitude"
'                                                 nod3.AddAttribute "type", "string"
'                                                 nod3.AddAttribute "selected", "true"
'                                                 nod3.AddAttribute "optional", "true"
'      Set nod3 = pProvides.createElement("huc_cd")
'                                                 nod2.appendChild nod3
'                                                 nod3.AddAttribute "name", "huc8 code"
'                                                 nod3.AddAttribute "type", "string"
'                                                 nod3.AddAttribute "optional", "true"
'                                                 nod3.AddAttribute "selected", "true"
'      Set nod3 = pProvides.createElement("drain_area_va")
'                                                 nod2.appendChild nod3
'                                                 nod3.AddAttribute "name", "drainage area"
'                                                 nod3.AddAttribute "type", "double"
'                                                 nod3.AddAttribute "optional", "true"
'                                                 nod3.AddAttribute "selected", "true"
'      Set nod3 = pProvides.createElement("")
'                                                 nod2.appendChild nod3
'                                                 nod3.AddAttribute "name", "contributing drainage area"
'                                                 nod3.AddAttribute "type", "double"
'                                                 nod3.AddAttribute "optional", "true"
'                                                 nod3.AddAttribute "selected", "false"
  'debug.print root.x

End Sub

Private Property Get clsWebData_HelpId() As Long
  clsWebData_HelpId = 29
End Property

Private Property Get clsWebData_Provides() As ChilkatXml
  Set clsWebData_Provides = pProvides
End Property

Private Function clsWebData_GetData(ByVal aQuery As ChilkatXml, ByRef aResult As ChilkatXml) As Boolean
  Dim url As String
 'Dim state_abbrev As String
  Dim state_fips As String
  Dim huc_2 As String
  Dim huc_cd As String
  Dim obs_count_nu As String
  Dim available As String
  Dim outputFormat As String
  Dim FirstFile As Boolean
  
  Dim nodStatus As ChilkatXml
  Dim QueryChild As ChilkatXml
  Dim QueryGrandchild As ChilkatXml
  Dim nodList As FastCollection
  Dim webResponse As ChilkatXml
  
  Dim urlStart As Variant
  Dim urlStarts As FastCollection
  Dim DownloadNumber As Long
  Dim data_type As String
  Dim data_type_requested As String
  
  Dim findPos As Long
  Dim base_existingShapeName As String
  Dim base_newShapeName As String
  Dim cache_filename As String
  Dim cache_dir As String
  Dim project_dir As String
  Dim base_huc_url As String
  
  Dim existingDBF As clsDBF
  Dim newDBF As clsDBF
  
  base_huc_url = pManager.CurrentStatusGetString("USGSSiteBaseHUCURL", _
                        "http://nwis.waterdata.usgs.gov/usa/nwis/inventory")
  cache_dir = pManager.CurrentStatusGetString("cache_dir") & pClassName & "\"
  project_dir = pManager.CurrentStatusGetString("project_dir")
  
  pManager.LogDbg "  clsUsgsSite GetData entry"
  clsWebData_GetData = True
  FirstFile = True
  
  Dim iChild As Long, jChild As Long, iSite As Long
  ReDim site_no(0)
  outputFormat = "XML"
  
  Set aResult = New ChilkatXml
  aResult.tag = pClassName
  Set nodStatus = aResult.newChild("status", "")
  
  data_type_requested = pManager.CurrentStatusGetString("USGSSiteqw")
  If Len(data_type_requested) > 0 Then
    base_existingShapeName = project_dir & pManager.CurrentStatusGetString("USGSSiteLayer" & data_type_requested, "wqobs")
    data_type = "&data_type=" & data_type_requested
    GoSub GetSitesOfDataType
    pManager.CurrentStatusUpdateString "USGSSite" & data_type_requested, "", "completed by " & pLabel
  End If
  data_type_requested = pManager.CurrentStatusGetString("USGSSitedischarge")
  If Len(data_type_requested) > 0 Then
    base_existingShapeName = project_dir & pManager.CurrentStatusGetString("USGSSiteLayer" & data_type_requested, "gage")
    data_type = "&data_type=" & data_type_requested
    GoSub GetSitesOfDataType
    pManager.CurrentStatusUpdateString "USGSSite" & data_type_requested, "", "completed by " & pLabel
  End If
  data_type_requested = pManager.CurrentStatusGetString("USGSSitemeasurement")
  If Len(data_type_requested) > 0 Then
    base_huc_url = "http://nwis.waterdata.usgs.gov/nwis/measurements"
    base_existingShapeName = project_dir & pManager.CurrentStatusGetString("USGSSiteLayer" & data_type_requested, "gage")
    data_type = "&data_type=" & data_type_requested
    GoSub GetSitesOfDataType
    pManager.CurrentStatusUpdateString "USGSSite" & data_type_requested, "", "completed by " & pLabel
  End If
  
  'Type of data at sites not specified
  If Len(data_type) = 0 Then GoSub GetSitesOfDataType
  pManager.LogDbg "  clsUsgsSite GetData exit"
  Exit Function

GetSitesOfDataType:
  
  base_newShapeName = cache_dir & "new" & data_type_requested
  If FileExists(base_newShapeName & ".dbf") Then Kill base_newShapeName & ".dbf"
  If FileExists(base_newShapeName & ".shp") Then Kill base_newShapeName & ".shp"
  If FileExists(base_newShapeName & ".shx") Then Kill base_newShapeName & ".shx"
  
  If FileExists(base_existingShapeName & ".dbf") Then
    pManager.LogDbg "Opening " & base_existingShapeName & ".dbf"
    Set existingDBF = New clsDBF
    existingDBF.OpenDBF base_existingShapeName & ".dbf"
    If existingDBF.NumFields < 2 Then
      GoTo CreateNewGageDBFfromScratch
    Else
      Set newDBF = existingDBF.Cousin
      newDBF.InitData
    End If
  Else
CreateNewGageDBFfromScratch:
    Set existingDBF = Nothing
    Set newDBF = NewGageDBF
  End If
  
  available = ""
  Set urlStarts = New FastCollection
  Set QueryChild = aQuery.FirstChild
  While Not QueryChild Is Nothing
    Select Case LCase(QueryChild.tag)
      Case "criteria"
        Set QueryGrandchild = QueryChild.FirstChild
        While Not QueryGrandchild Is Nothing
          Select Case LCase(QueryGrandchild.tag)
'              Case "huc_2":         urlStarts.Add "http://waterdata.usgs.gov/nwis/inventory?huc2_cd=" & QueryGrandchild.Content & data_type
'              Case "state_fips":    urlStarts.Add "http://waterdata.usgs.gov/nwis/inventory?state_cd=" & QueryGrandchild.Content & data_type
'              Case "state_abbrev":  urlStarts.Add "http://waterdata.usgs.gov/" & LCase(QueryGrandchild.Content) & "/nwis/inventory?" & Mid(data_type, 2)
            Case "huc_cd":        urlStarts.Add base_huc_url & "?" & Mid(data_type, 2) & "&huc_cd=" & LCase(QueryGrandchild.Content)
            Case "obs_count_nu":  obs_count_nu = "&obs_count_nu=" & QueryGrandchild.Content
          End Select
          If QueryGrandchild.NextSibling2 = 0 Then Set QueryGrandchild = Nothing
        Wend
      Case "available"
        Set QueryGrandchild = QueryChild.FirstChild
        While Not QueryGrandchild Is Nothing
          available = available & "&column_name=" & QueryGrandchild.Content
          If QueryGrandchild.NextSibling2 = 0 Then Set QueryGrandchild = Nothing
        Wend
    End Select
    If QueryChild.NextSibling2 = 0 Then Set QueryChild = Nothing
  Wend

  If Len(available) = 0 Then available = pDefaultColumns
  DownloadNumber = 0
  For Each urlStart In urlStarts
    DownloadNumber = DownloadNumber + 1
RetryDownload:
    url = urlStart & obs_count_nu & _
             "&sort_key=site_no" & _
             "&group_key=NONE" & _
             "&format=sitefile_output" & _
             "&sitefile_output_format=" & LCase(outputFormat) & available & _
             "&list_of_search_criteria=data_type"
    
    'Try to use or create cached download of these stations if we are just using defaults
    'Also update the gage layer with newly found stations if needed
    cache_filename = ""
    If available = pDefaultColumns Then
      findPos = InStr(urlStart, "&huc_cd=")
      If findPos > 0 Then
        cache_filename = pClassName & "\" & Mid(urlStart, findPos + 8, findPos + 16) & data_type_requested & obs_count_nu & "." & LCase(outputFormat)
      End If
    End If

    If Not pManager.Download(url, "", FirstFile, "Downloading " & data_type_requested & " " & pLabel & " (" & DownloadNumber & " of " & urlStarts.count & ")", "", cache_filename) Then
      nodStatus.AddAttribute "complete", "false"
      nodStatus.AddAttribute "message", "User Cancelled"
      
      Exit Function '!!!!!!!!!!!!!!!!!!!!!!!
    
    End If

    FirstFile = False
    If pManager.ResultString = "<undefined/>" Then
      nodStatus.AddAttribute "complete", "false"
      nodStatus.AddAttribute "message", "ResultString undefined"
    Else
      nodStatus.AddAttribute "complete", "true"
      Set webResponse = New ChilkatXml
      webResponse.LoadXml ReplaceString(pManager.ResultString, "&", "and")
      If webResponse.tag = "usgs_nwis" Then
        Call ParseSiteNumFrom(webResponse, aResult, data_type_requested, existingDBF, newDBF)
      Else 'download didnt work
        If Len(pManager.ResultString) < 1100 Then pManager.LogDbg "----- Unexpected result: " & vbCrLf & pManager.ResultString & vbCrLf & "----- End of unexpected result"
        Select Case pManager.LogMsg("Download did not find expected data.", pLabel, "Abort", "Retry", "Skip", "Skip All " & pLabel, "View Retrieved Page")
          Case 1: pManager.State = 1003: Exit Function 'state_cancel_download
          Case 2: GoTo RetryDownload
          Case 3: 'Skip
          Case 4: Exit Function
          Case 5: pManager.ShowHTML pManager.ResultString
        End Select
      End If
    End If
  Next
  
  If Not newDBF Is Nothing Then
    If newDBF.NumRecords > 0 Then
      pManager.LogDbg "Writing " & newDBF.NumRecords & " new stations for " & base_existingShapeName & " to " & base_newShapeName
      newDBF.WriteDBF base_newShapeName & ".dbf"
      WriteShapePointsFromDBF newDBF, gage_field.LATDD, gage_field.LONGDD
      pManager.AppendProjectXML "<convert_shape output=""" & base_existingShapeName & ".shp" & """>" & base_newShapeName & ".shp" & "</convert_shape>"
    End If
    
    newDBF.Clear
    Set newDBF = Nothing
  End If
  
  Return

ErrHand:
  pManager.LogMsg "Error " & Err.Description, _
                         pLabel & " GetData"
End Function

Private Sub ParseSiteNumFrom(WebResponseNode As ChilkatXml, _
                                     aResult As ChilkatXml, _
                         data_type_requested As String, _
                         existingDBF As clsDBF, newDBF As clsDBF)
  Dim WebNode As ChilkatXml
  Dim WebChildNode As ChilkatXml
  Dim lNode As ChilkatXml
  Dim vChild As Variant
  Dim tag As String
  
  If Len(data_type_requested) > 0 Then
    tag = data_type_requested & "_site_no"
  Else
    tag = "site_no"
  End If
  
  Set WebNode = WebResponseNode.FirstChild
  While Not WebNode Is Nothing
    Set lNode = aResult.newChild(tag, "")
    Set WebChildNode = WebNode.FirstChild
    While Not WebChildNode Is Nothing
      If LCase(WebChildNode.tag) = "site_no" Then
        lNode.Content = WebChildNode.Content
      Else
        lNode.AddAttribute WebChildNode.tag, WebChildNode.Content
      End If
      If WebChildNode.NextSibling2 = 0 Then Set WebChildNode = Nothing
    Wend
    
    If Not newDBF Is Nothing Then
      If existingDBF Is Nothing Then
        AddNodeToDBF lNode, newDBF
      ElseIf existingDBF.FindFirst(4, lNode.Content) Then
        'Found existing wqobs station
      ElseIf existingDBF.FindFirst(5, "USGS" & lNode.Content) Then
        'Found existing gage station
      Else
        AddNodeToDBF lNode, newDBF
      End If
    End If
    
    Set lNode = Nothing
    If WebNode.NextSibling2 = 0 Then Set WebNode = Nothing
  Wend
End Sub

Private Sub AddNodeToDBF(aNode As ChilkatXml, aDBF As clsDBF)
  Dim fieldNum As Long
  aDBF.CurrentRecord = aDBF.NumRecords + 1
  For fieldNum = 1 To aDBF.NumFields
    aDBF.Value(fieldNum) = ""
  Next
  If aDBF.FieldName(gage_field.STCO) = "STCO" Then
     aDBF.Value(gage_field.AGCY) = "USGS" & aNode.Content
     aDBF.Value(gage_field.STCO) = aNode.GetAttrValue("state_cd") & aNode.GetAttrValue("county_cd")
     aDBF.Value(gage_field.LATDD) = aNode.GetAttrValue("dec_lat_va")
     aDBF.Value(gage_field.LONGDD) = aNode.GetAttrValue("dec_long_va")
     aDBF.Value(gage_field.Name) = aNode.GetAttrValue("station_nm")
    'aDBF.Value(gage_field.AREA) = aNode.getAttrValue("drain_area_va") Should we try to set area to drain_area_va or contrib_drain_area_va?
  Else
     aDBF.Value(2) = aNode.GetAttrValue("agency_cd")
     aDBF.Value(4) = aNode.Content
     aDBF.Value(6) = aNode.GetAttrValue("state_cd")
     aDBF.Value(7) = aNode.GetAttrValue("dec_lat_va")
     aDBF.Value(8) = aNode.GetAttrValue("dec_long_va")
     aDBF.Value(10) = aNode.GetAttrValue("station_nm")
     aDBF.Value(11) = aNode.GetAttrValue("huc_cd")
     aDBF.Value(15) = aNode.GetAttrValue("huc_cd")
     aDBF.Value(16) = aDBF.Value(4) & "+" & aDBF.Value(2)
  End If
End Sub

Private Property Get clsWebData_Label() As String
  clsWebData_Label = pLabel
End Property

Private Property Get clsWebData_Name() As String
  clsWebData_Name = "clsUSGSsite"
End Property

Private Property Set clsWebData_Manager(myManager As clsWebDataManager)
  Set pManager = Nothing
  Set pManager = myManager
End Property

Private Property Get clsWebData_Manager() As clsWebDataManager
  Set clsWebData_Manager = pManager
End Property

Private Sub clsWebData_Specify(Optional parent As Object = Nothing, Optional ByVal Index As Long = 0)
  pManager.ShowCriteria Me, parent, Index
End Sub

Private Property Get clsWebData_Version() As String
  clsWebData_Version = App.Major & "." & App.Minor & "." & App.Revision
End Property

