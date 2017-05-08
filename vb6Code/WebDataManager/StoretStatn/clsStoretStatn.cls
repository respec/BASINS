VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsStoretStatn"
Attribute VB_GlobalNameSpace = True
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
Implements clsWebData

'Downloads station data from STORET Data Warehouse http://www.epa.gov/STORET/dw_home.html

Private Const pLabel = "STORET Station"
Private Const pClassName = "clsStoretStatn"
Private Const pHelpID = 131
Private pManager As clsWebDataManager
Private pProvides As ChilkatXml

Private pBuildingShapeDBF As clsDBF
Private pExistingShapeDBF As clsDBF

Private Sub Class_Initialize()
  Dim nod1 As ChilkatXml
  Dim nod2 As ChilkatXml
  Dim nod3 As ChilkatXml
  
  Set pProvides = New ChilkatXml
  Set pProvides = New ChilkatXml
  pProvides.Tag = pClassName
  Set nod1 = pProvides.newChild("output", "")
  Set nod2 = nod1.newChild("XML", "")
      nod2.AddAttribute "type", "ChilkatXml"
' Set nod2 = nod1.newChild("file")
'     nod2.AddAttribute "format", "csv"
  
  Set nod1 = pProvides.newChild("criteria", "")
  Set nod2 = nod1.newChild("huc_cd", "")
      nod2.AddAttribute "label", "HUC"
        
  Set nod1 = pProvides.newChild("available", "")
  Set nod2 = nod1.newChild("station", "")
  Set nod3 = nod2.newChild("storetstation", "")
      nod3.AddAttribute "name", "Storet Station"
      nod3.AddAttribute "type", "string"
      nod3.AddAttribute "selected", "true"
  
  Set nod1 = Nothing
  Set nod2 = Nothing
  Set nod3 = Nothing

End Sub

Private Function BuildSelectHUCURL(HUC As String) As String
  Dim urlAfterHUC As String
  Dim iColumn As Long
  
  urlAfterHUC = pManager.CurrentStatusGetString("StoretStatnAfterHUC", "none")
  If urlAfterHUC = "none" Then
    urlAfterHUC = "&v_StationCount=&v_filters=~~HUC&v_mode=Online"  'We don't know the count, but we need to mention it to get a result
    For iColumn = 1 To 33
      urlAfterHUC = urlAfterHUC & "&v_Columns=" & iColumn
      Select Case iColumn 'Insert a few extra columns between the first 33
        Case 16: urlAfterHUC = urlAfterHUC & "&v_Columns=16.5"
        Case 24: urlAfterHUC = urlAfterHUC & "&v_Columns=24.1&v_Columns=24.2"
        Case 31: urlAfterHUC = urlAfterHUC & "&v_Columns=31.5"
      End Select
    Next
    '1 = Org ID
    '2 = Station ID
    '3 = Station Name
    '4 = Org Name
    
    '5 = Primary Type
    '6 = Secondary Type
    '7 = S/G/O Indicator
    '8 = Well Number
    '9 = Well Name
    '10 = Pipe Number
    '11 = NAICS Code
    '12 = Spring Info http://www.epa.gov/storpubl/modern/doc/DW_storet_help.html#651
  
    '13 = Location Point Type
    '14 = Point Sequence Number
    '15 = Point Name
    '16 = Latitude/Longitude http://www.epa.gov/storpubl/modern/doc/DW_storet_help.html#652
    '16.5
    '17 = Lat/Long Info      http://www.epa.gov/storpubl/modern/doc/DW_storet_help.html#653'
    '18 = Elevation (w/ Units)
    '19 = Additional Elevation Info
    
    '20 = Country Name
    '21 = State
    '22 = County
    
    '23 = Hydrologic Unit Code
    '24 = Hydrologic Unit Name
    '24.1
    '24.2
    '25 = RF1 Info
    '26 = NRCS Watershed ID
    '27 = Estuary Info
    '28 = Great Lake Name
    '29 = Ocean Name
  
    '30 = Natv American Land Name
    '31 = FRS Key Identifier
    '31.5
    '32 = Station Document/Graphic Name
    '33 = Station Document/Graphic URL
  End If
  
  BuildSelectHUCURL = pManager.CurrentStatusGetString("StoretStatnBaseHucURL", _
                                         "http://oaspub.epa.gov/storpubl/DW_STATION_HUB_custom?") _
                    & pManager.CurrentStatusGetString("StoretStatnBeforeHUC", "v_FromStmt=FROM FA_STATION FA" _
                                                                           & "&v_WhereStmt=Where FA.generated_huc = ")
  BuildSelectHUCURL = ReplaceString(BuildSelectHUCURL & HUC & urlAfterHUC, " ", "%20")
End Function

Private Function clsWebData_GetData(ByVal aQuery As ChilkatXml, aResult As ChilkatXml) As Boolean
  Dim lQueryChild As ChilkatXml
  Dim lCritChild As ChilkatXml
  Dim FileName As String
  Dim huc_cd As String
  Dim hucs As New FastCollection
  Dim vHuc As Variant
  Dim project_dir As String
  Dim destinationShapeFilename As String
  Dim FirstFile As Boolean
  Dim nodStatus As ChilkatXml
  Dim nod2 As ChilkatXml
  Dim nodList As FastCollection
  Dim webResponse As ChilkatXml
  Dim findPos As Long
  Dim URLstart As Long
  Dim url As String
  Dim DownloadNumber As Long
  Dim AllStationData As String
  Dim cache_dir As String
  Dim cache_filename As String
    
  pManager.LogDbg "  clsWebDataStoretStatn GetData entry"

  clsWebData_GetData = True
  FirstFile = True
  
  Dim iChild As Long, jChild As Long, iSite As Long
  
  FileName = ""
  project_dir = pManager.CurrentStatusGetString("project_dir")
  cache_dir = pManager.CurrentStatusGetString("cache_dir")
  Set lQueryChild = aQuery.FirstChild
  While Not lQueryChild Is Nothing
    Select Case LCase(lQueryChild.Tag)
      Case "criteria"
        Set lCritChild = lQueryChild.FirstChild
        While Not lCritChild Is Nothing
          Select Case LCase(lCritChild.Tag)
            Case "huc_cd":     hucs.Add lCritChild.Content
          End Select
          If lCritChild.NextSibling2 = 0 Then Set lCritChild = Nothing
        Wend
    End Select
    If lQueryChild.NextSibling2 = 0 Then Set lQueryChild = Nothing
  Wend

  aResult.Tag = pClassName
  Set nodStatus = aResult.newChild("status", "")
  
  For Each vHuc In hucs
    huc_cd = vHuc
    url = BuildSelectHUCURL(huc_cd)
    cache_filename = pClassName & "\" & huc_cd & ".html"
    If Not pManager.Download(url, "", True, "Downloading " & pLabel & " for " & huc_cd, "", cache_filename) Then
      nodStatus.AddAttribute "complete", "false"
      nodStatus.AddAttribute "message", "User Cancelled"
      If FileExists(cache_dir & cache_filename) Then Kill cache_dir & cache_filename
  
      Exit Function '!!!!!!!!!!!!!!!!!!!!!!!
      
    End If
    FirstFile = False
    findPos = InStr(pManager.ResultString, "Download your file now")
    If findPos > 0 Then
      URLstart = InStrRev(pManager.ResultString, "=""", findPos)
      url = Mid(pManager.ResultString, URLstart + 2)
      findPos = InStr(url, """")
      url = Left(url, findPos - 1)
      cache_filename = pClassName & "\" & huc_cd & ".txt"
      If Not pManager.Download(url, "", False, "Downloading " & pLabel, "", cache_filename) Then
        nodStatus.AddAttribute "complete", "false"
        nodStatus.AddAttribute "message", "Could not download " & pLabel & vbCr & url
        If FileExists(cache_dir & cache_filename) Then Kill cache_dir & cache_filename
      Else
        nodStatus.AddAttribute "complete", "true"
'        If Len(save_dir) > 0 Then
'          Set nod2 = aResult.newChild("file", "")
'          nod2.AddAttribute "name", save_dir & Filename
'        Else
        If pManager.ResultString = "binary file could not be read as string" Then
          AllStationData = AllStationData & WholeFileString(cache_dir & cache_filename)
        Else
          AllStationData = AllStationData & pManager.ResultString
        End If
'        End If
      End If
    ElseIf InStr(pManager.ResultString, "no Stations found") > 0 Then
      nodStatus.AddAttribute "complete", "false"
      nodStatus.AddAttribute "message", "No stations found in HUC " & huc_cd
      pManager.LogDbg "  No stations found in HUC " & huc_cd
    Else
      pManager.LogDbg "  Unknown response for HUC " & huc_cd & ":" & pManager.ResultString
      If FileExists(cache_dir & cache_filename) Then Kill cache_dir & cache_filename
    End If
  Next
  
  If Len(AllStationData) > 0 Then ParseStationsFrom AllStationData, aResult
  
  If pBuildingShapeDBF Is Nothing Then
    pManager.LogDbg "No " & pLabel & " found for the given area"
  Else
    If pBuildingShapeDBF.NumRecords > 0 Then
      pManager.LogDbg "Writing " & pBuildingShapeDBF.NumRecords & " stations to " & pBuildingShapeDBF.FileName
      pBuildingShapeDBF.WriteDBF pBuildingShapeDBF.FileName
      pManager.LogDbg "Creating shp and shx from dbf fields Latitude and Longitude"
      WriteShapePointsFromDBF pBuildingShapeDBF, _
                              pBuildingShapeDBF.FieldNumber("Latitude"), _
                              pBuildingShapeDBF.FieldNumber("Longitude")
      destinationShapeFilename = project_dir & "STORET stations.shp"
      pManager.LogDbg "Requesting merging and/or projecting into " & destinationShapeFilename
      pManager.AppendProjectXML "<convert_shape output=""" & destinationShapeFilename & """>" _
                                & FilenameSetExt(pBuildingShapeDBF.FileName, "shp") & "</convert_shape>" & vbCrLf & _
                                "  <add_shape>" & destinationShapeFilename & "</add_shape>"
    End If
    
    pBuildingShapeDBF.Clear
    Set pBuildingShapeDBF = Nothing
    
  End If
  
  pManager.LogDbg "  clsWebDataStoretStatn GetData exit"

End Function

Private Function DBFFieldNameFromOriginalName(OriginalName As String, iName As Long) As String
  Dim NewName As String
  NewName = ReplaceString(OriginalName, " ", "_")
  NewName = ReplaceString(NewName, "/", "")
  If Len(NewName) > 10 Then
    Select Case OriginalName
      Case "Station Name":                   NewName = "Name"
      Case "Primary Type":                   NewName = "Prim_Type"
      Case "Secondary Type":                 NewName = "Sec_Type"
      Case "S/G/O Indicator":                NewName = "Surf_Groun"
      Case "Well Number":                    NewName = "Well_Num"
      Case "Pipe Number":                    NewName = "Pipe_Num"
      Case "Spring Type Improvement":        NewName = "Spr_Imprvt"
      Case "USGS Geologic Unit Code-Name":   NewName = "Geo_Name"
      Case "Spring Other Name":              NewName = "Spr_Name"
      Case "USGS Lithologic Unit Code-Name": NewName = "Litho_Name"
      Case "Location Point Type":            NewName = "Point_Type"
      Case "Point Sequence Number":          NewName = "Point_Num"
      Case "Horizontal Datum":               NewName = "Horz_Datum"
      Case "Geopositioning Method":          NewName = "Geopos_met"
      Case "Elevation Datum":                NewName = "Elev_Datum"
      Case "Elevation Method":               NewName = "Elev_methd"
      Case "Country Name":                   NewName = "Cntry_Name"
      Case "Hydrologic Unit Code":           NewName = "HUC"
      Case "Hydrologic Unit Name":           NewName = "HUC_Name"
      Case "RF1 Segment Code":               NewName = "RF1_Code"
      Case "RF1 Segment Name":               NewName = "RF1_Name"
      Case "RF1 Mileage":                    NewName = "RF1_Mileag"
      Case "On Reach Ind":                   NewName = "On_Reach"
      Case "NRCS Watershed ID":              NewName = "NRCS_ID"
      Case "Primary Estuary":                NewName = "Prim_Estuy"
      Case "Secondary Estuary":              NewName = "Sec_Estury"
      Case "Other Estuary Name":             NewName = "Oth_Estury"
      Case "Great Lake Name":                NewName = "Grt_Lak_Nm"
      Case "Natv American Land Name":        NewName = "Natv_Name"
      Case "FRS Key Identifier":             NewName = "FRS_Key_ID"
      Case "Station Document/Graphic Name":  NewName = "Graphic_Nm"
      Case "Station Document/Graphic URL":   NewName = "Graphc_URL"
      Case Else: NewName = Left(NewName, 8) & iName
    End Select
  End If
  DBFFieldNameFromOriginalName = NewName
End Function

'result is the string returned from the modern STORET site as a list of stations
'BaseChildNode is the parent that will have <storetstation> children added to it from result
Private Sub ParseStationsFrom(ByVal StationData As String, BaseChildNode As ChilkatXml)
  Dim lNode As ChilkatXml
  Dim fieldnums() As Long
  Dim curLine As String
  Dim buf As String
  Dim iField As Long
  Dim iRecord As Long
  Dim iNextDBFrecord As Long
  Dim baseShapeName As String
  Dim csv As clsCSV
  Dim OrigFirstFieldName As String
'  Dim SkipDuplicates As Boolean
'  Dim SkipThis As Boolean
'  Dim MatchFields(2) As Long
'  Dim MatchValues(2) As Variant
'  Dim MatchOperators(2) As String

  Set csv = New clsCSV
  csv.Delimiter = "~"
  csv.OpenCSV StationData
  csv.CalculateFieldProperties 'Find field widths/types
  
  OrigFirstFieldName = csv.fieldName(1)
  
  For iField = 1 To csv.numFields
    csv.fieldName(iField) = DBFFieldNameFromOriginalName(csv.fieldName(iField), iField)
  Next
    
  For iRecord = 1 To csv.NumRecords
    csv.CurrentRecord = iRecord
    If pBuildingShapeDBF Is Nothing Then
      baseShapeName = pManager.CurrentStatusGetString("project_dir") & "STORETtemp"
'      If FileExists(baseShapeName & ".dbf") Then
'        pManager.LogDbg "Opening " & baseShapeName & ".dbf"
'        Set pExistingShapeDBF = New clsDBF
'        pExistingShapeDBF.OpenDBF baseShapeName & ".dbf"
'        If pExistingShapeDBF.numFields < 2 Or pExistingShapeDBF.NumRecords = 0 Then
'          GoTo CreateNewDBFfromScratch
'        Else
'          Set pBuildingShapeDBF = pExistingShapeDBF
'          iNextDBFrecord = pBuildingShapeDBF.NumRecords + 1
'          SkipDuplicates = True
'          MatchFields(1) = 1
'          MatchFields(2) = 2
'          MatchOperators(1) = "="
'          MatchOperators(2) = "="
'        End If
'      Else
'CreateNewDBFfromScratch:
        iNextDBFrecord = 1
        Set pExistingShapeDBF = Nothing
        Set pBuildingShapeDBF = NewStationDBF(csv)
        pBuildingShapeDBF.WriteDBF baseShapeName & ".dbf"
'      End If
      
      ReDim fieldnums(csv.numFields)
      For iField = 1 To csv.numFields
        fieldnums(iField) = pBuildingShapeDBF.FieldNumber(csv.fieldName(iField))
        If fieldnums(iField) > 0 Then
          pManager.LogDbg "Adding values in field " & iField & " (" & csv.fieldName(iField) & ") to DBF"
        Else
          pManager.LogDbg "Skipping values in field " & iField & " (" & csv.fieldName(iField) & ")"
        End If
      Next
    End If
    
    
    Select Case csv.Value(1)
      Case OrigFirstFieldName:  'skip duplicate headers (between data for multiple HUCs)
      Case "":                  'skip blank rows in csv
      Case Else
        Set lNode = BaseChildNode.newChild("storetstation", "")
        
'        If SkipDuplicates Then
'          MatchValues(1) = Trim(csv.Value(1))
'          MatchValues(2) = Trim(csv.Value(2))
'          If pBuildingShapeDBF.FindMatch(MatchFields, MatchOperators, MatchValues) Then
'            SkipThis = True
'          Else
'            SkipThis = False
'          End If
'        End If
'
'        If Not SkipThis Then
          pBuildingShapeDBF.CurrentRecord = iNextDBFrecord
          iNextDBFrecord = iNextDBFrecord + 1
'        End If
        
        For iField = 1 To csv.numFields
          buf = Trim(csv.Value(iField))
          If Len(buf) > 0 Then
            lNode.AddAttribute csv.fieldName(iField), buf
'            If Not SkipThis Then
              If fieldnums(iField) > 0 Then
                pBuildingShapeDBF.Value(fieldnums(iField)) = buf
              End If
'            End If
          End If
        Next
        lNode.Content = lNode.GetAttrValue("Org_ID") & "-" & lNode.GetAttrValue("Station_ID")
        Set lNode = Nothing
    End Select
  Next
  Set csv = Nothing
End Sub

Private Function NewStationDBF(csv As clsCSV) As clsDBF
  Dim iField As Long
  Dim newDBF As clsDBF
  Set newDBF = New clsDBF
  With newDBF
    .numFields = csv.numFields
    '.NumRecords = csv.NumRecords
    For iField = 1 To .numFields
      .fieldName(iField) = csv.fieldName(iField)
      .FieldLength(iField) = csv.FieldLength(iField)
      .FieldType(iField) = csv.FieldType(iField)
      .FieldDecimalCount(iField) = csv.FieldDecimalCount(iField)
    Next
    .InitData
  End With
  Set NewStationDBF = newDBF
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

