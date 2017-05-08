VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsPCSFacility"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
Implements clsWebData

Private Const pLabel = "PCS Facility"
Private Const pClassName = "clsPCSFacility"
Private Const pHelpID = 130

Private pProvides As ChilkatXml
Private pManager As clsWebDataManager

'http://www.epa.gov/enviro/html/pcs/adhoc.html
'Effluent Limits--Allowable Discharges
'Effluent Measurements / Violations

'http://oaspub.epa.gov/enviro/ad_hoc_table_column_select.retrieval_list?
'database_type=PCS&
'selected_subjects=Effluent+Limits--Allowable+Discharges&
'selected_subjects=Effluent+Measurements/Violations&
'subject_selection=+&
'table_1=+

Private Sub Class_Initialize()
  Dim root As ChilkatXml
  Dim nod1 As ChilkatXml
  Dim nod2 As ChilkatXml
  Dim nod3 As ChilkatXml
  
  Set pProvides = New ChilkatXml
  pProvides.Tag = pClassName
  Set nod1 = pProvides.newChild("output", "")
  Set nod2 = nod1.newChild("XML", "")
      nod2.AddAttribute "type", "ChilkatXml"
' Set nod2 = nod1.newChild("file", "")
'     nod2.AddAttribute "format", "csv"
  
  Set nod1 = pProvides.newChild("criteria", "")
  Set nod2 = nod1.newChild("huc_cd", "")
      nod2.AddAttribute "label", "8-digit HUC"
      nod2.AddAttribute "multiple", "true"
      
  Set nod1 = pProvides.newChild("available", "")
  Set nod2 = nod1.newChild("npdes", "")
  Set nod3 = nod2.newChild("npdes", "")
      nod3.AddAttribute "name", "NPDES #"
      nod3.AddAttribute "type", "string"
      nod3.AddAttribute "selected", "true"
  
  Set nod1 = Nothing
  Set nod2 = Nothing
  Set nod3 = Nothing

End Sub

Private Function BuildQueryURL(HUC As String) As String
  Dim urlAfterHUC As String
    
  urlAfterHUC = pManager.CurrentStatusGetString("PCSFacilityAfterHUC", "none")
  If urlAfterHUC = "none" Then
    Dim tbl As String, s1 As String, s2 As String
    tbl = "&table_1=V_PCS_PERMIT_FACILITIES."
    s1 = "&table1_type=+&table1_value=&column_number=&sort_selection=&sort_order=Ascending"""
    s2 = "&table1_type=Equal+to&table1_value=&column_number=&sort_selection=&sort_order=Ascending"""
    urlAfterHUC = "&City=&county=&state=&sic_code=&chemsearch=chemname&chemstatus=Equal+to&chemvalue=&where_selection=dummy"
    urlAfterHUC = urlAfterHUC & tbl & "NPDES" & s1 _
                              & tbl & "LATITUDE" & s2 _
                              & tbl & "LONGITUDE" & s2 _
                              & tbl & "LOC_NAME" & s2 _
                              & tbl & "CITY_NAME" & s2 _
                              & tbl & "COUNTY_NAME" & s2 _
                              & tbl & "REGION" & s2 _
                              & tbl & "COUNTY_CODE" & s2 _
                              & tbl & "LOC_ZIP_CODE" & s2 _
                              & tbl & "LOC_PHONE_NUM" & s2 _
                              & tbl & "PERMIT_EXPIRED_DATE" & s2 _
                              & tbl & "PERMIT_ISSUED_DATE" & s2 _
                              & tbl & "CODE_OF_ACCURACY" & s2 _
                              & tbl & "FLOW_RATE" & s2 _
                              & tbl & "RECEIVING_WATERS" & s2 _
                              & tbl & "TYPE_OF_OWNERSHIP" & s2 _
                              & tbl & "MAJOR_DISCHARGE_INDICATOR" & s1 _
                              & tbl & "NAME_1" & s1 _
                              & tbl & "NAME_2" & s2 _
                              & "&group_sequence=test&csv_output=Output+to+CSV+File"
                     
                             '& tbl & "INACTIVE_DATE" & s2
  End If
  
  BuildQueryURL = pManager.CurrentStatusGetString("PCSFacilitybaseURL", "http://oaspub.epa.gov/enviro/ad_hoc_build_sql1.get_table?")
  BuildQueryURL = BuildQueryURL & pManager.CurrentStatusGetString("PCSFacilityBeforeHUC", "database_type=PCS&fac_name=fac_name&fac_search=Equal+to&Value=&zipcode=&usgs_huc=")
  BuildQueryURL = BuildQueryURL & HUC & urlAfterHUC
  
End Function

Private Function clsWebData_GetData(ByVal aQuery As ChilkatXml, aResult As ChilkatXml) As Boolean
  Dim FileName As String
  Dim lstHucs As FastCollection
  Dim huc_cd As String
  Dim huc_progress As String
  Dim project_dir As String
  Dim FirstFile As Boolean
  
  Dim nodStatus As ChilkatXml
  Dim webResponse As ChilkatXml
  
  Dim URLstart As String
  Dim url As String
  Dim SearchFor As String
  
  Dim DownloadNumber As Long
  Dim cache_filename As String
  Dim cache_dir As String
  Dim QueryCached As Boolean
  Dim CSVCached As Boolean
  
  pManager.Logdbg "  clsWebDataPCSFacility GetData entry"
  cache_dir = pManager.CurrentStatusGetString("cache_dir")
  
  clsWebData_GetData = True
  FirstFile = True

  aResult.Tag = pClassName
  Set nodStatus = aResult.newChild("status", "")
  
  Set lstHucs = GetChildrenWithTag(aQuery.GetChildWithTag("criteria"), "huc_cd")
  For DownloadNumber = 1 To lstHucs.count
    huc_cd = lstHucs.ItemByIndex(DownloadNumber).Content
    huc_progress = " HUC " & huc_cd & " (" & DownloadNumber & " of " & lstHucs.count & ")"
    url = ""
    
    'Can skip querying if we already have the CSV file
    cache_filename = pClassName & "\" & huc_cd & "-2.csv"
    CSVCached = FileExists(cache_dir & cache_filename)
    If CSVCached Then GoTo OpenCSV
    
'RunQuery:
    If Not pManager.Download(BuildQueryURL(huc_cd), "", True, _
                             "Downloading " & pLabel & vbCr & huc_progress) Then
      nodStatus.AddAttribute "complete", "false"
      nodStatus.AddAttribute "message", "User Cancelled"
  
      Exit Function '!!!!!!!!!!!!!!!!!!!!!!!
      
    End If
    
    FirstFile = False
  
    If pManager.ResultString = "<undefined/>" Then
      nodStatus.AddAttribute "complete", "false"
      nodStatus.AddAttribute "message", "ResultString undefined"
    Else
      Dim URLedge As Integer
      url = pManager.ResultString
      SearchFor = "Total number of records returned from your query: 0<"
      If InStr(url, SearchFor) > 0 Then
        pManager.Logdbg pLabel & ": No records returned for " & huc_cd
        nodStatus.AddAttribute "message", "ResultString undefined"
      Else
        SearchFor = "Click on the underlined filename : <a href="
        URLedge = InStr(url, SearchFor)
        If URLedge = 0 Then
          If pManager.LogMsg("Could not find start of CSV URL. Show result?", pLabel, "+&Yes", "=&No") = 1 Then
            pManager.ShowHTML pManager.ResultString
          End If
        Else
          url = Trim(Mid(url, URLedge + Len(SearchFor)))
          URLedge = InStr(url, ">")
          If URLedge = 0 Then
            If pManager.LogMsg("Could not find end of CSV URL. Show result?", pLabel, "+&Yes", "=&No") = 1 Then
              pManager.ShowHTML pManager.ResultString
            End If
          Else
            url = Left(url, URLedge - 1)
            If Not pManager.Download(url, cache_dir & cache_filename, False, _
                                     "Downloading facility details" & vbCr & huc_progress) Then
              nodStatus.AddAttribute "complete", "false"
              nodStatus.AddAttribute "message", "User Cancelled"
            Else
OpenCSV:
              Dim shpBaseNames(0) As String
              Dim csv As clsCSV
              Set csv = New clsCSV
              csv.OpenCSV cache_dir & cache_filename
              project_dir = pManager.CurrentStatusGetString("project_dir")
              shpBaseNames(0) = cache_dir & pClassName & "\" & huc_cd & "_pcs3"
              pManager.Logdbg "Creating temporary shape file " & shpBaseNames(0)
              WritePCSShape csv, shpBaseNames(0)
              pManager.AppendProjectXML "<convert_shape output=""" & project_dir & "pcs3.shp" & """>" & shpBaseNames(0) & ".shp" & "</convert_shape>"
              nodStatus.AddAttribute "complete", "true"
              
              Call ParsePCSFacilitiesFrom(WholeFileString(cache_dir & cache_filename), aResult, huc_cd)
            End If
          End If
        End If
      End If
    End If
  Next
  pManager.Logdbg "  clsWebDataPCSFacility GetData exit"

End Function

Private Sub ParsePCSFacilitiesFrom(CSVstr As String, BaseChildNode As ChilkatXml, huc_cd As String)
  Dim lNode As ChilkatXml
  Dim fieldnames() As String, nFields As Integer
  Dim curLine As String
  Dim buf As String
  Dim field As Integer, NPDESfield As Integer
  Dim FieldNameNPDES As String
  
  FieldNameNPDES = pManager.CurrentStatusGetString("PCSFieldNameNPDES", "NPDES")
  
  nFields = 0
  ReDim fieldnames(0)
  
  curLine = StrSplit(CSVstr, vbLf, """")
  While Len(curLine) > 0
    buf = StrSplit(curLine, ",", """")
    If Len(buf) > 0 Then
      nFields = nFields + 1
      ReDim Preserve fieldnames(nFields)
      If InStr(buf, ".") > 0 Then
        fieldnames(nFields) = Mid(buf, InStr(buf, ".") + 1)
      Else
        fieldnames(nFields) = buf
      End If
    End If
    If UCase(buf) = FieldNameNPDES Then NPDESfield = nFields
  Wend
  
  'If we didn't find a name field, use the first one
  If NPDESfield = 0 And nFields > 0 Then NPDESfield = 1
  
  While Len(CSVstr) > 0
    curLine = StrSplit(CSVstr, vbLf, """")
    Set lNode = BaseChildNode.newChild("npdes", "")
    lNode.AddAttribute "huc_cd", huc_cd
    For field = 1 To nFields
      buf = Trim(StrSplit(curLine, ",", """"))
      If Len(buf) > 0 Then
        If field = NPDESfield Then
          lNode.Content = buf
        Else
          lNode.AddAttribute fieldnames(field), buf
        End If
      End If
    Next
    Set lNode = Nothing
  Wend
End Sub

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


