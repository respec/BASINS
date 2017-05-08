VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsUSGSWQ"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
Implements clsWebData

Private Const pLabel = "USGS Water Quality"
Private Const pClassName = "clsUSGSWQ"
Private Const pHelpID = 33
Private pProvides As ChilkatXml
Private pErrorDescription As String

Private pManager As clsWebDataManager

Private Sub Class_Initialize()
  Dim nod1 As ChilkatXml
  Dim nod2 As ChilkatXml
  
  Set pProvides = New ChilkatXml
  pProvides.Tag = pClassName
  Set nod1 = pProvides.newChild("output", "")
  Set nod2 = nod1.newChild("file", "")
      nod2.AddAttribute "format", "RDB"
  
  Set nod1 = pProvides.newChild("criteria", "")
  Set nod2 = nod1.newChild("qw_site_no", "")
      nod2.AddAttribute "multiple", "true"
      nod2.AddAttribute "label", "Water Quality Site Number"
    
'    Set nod2 = nod1.NewChild("begin_date", "")
'                                                 nod2.AddAttribute "type", "Date"
'                                                 nod2.AddAttribute "format", "yyyy-mm-dd"
'                                                 nod2.AddAttribute "optional", "true"
'                                                 nod2.AddAttribute "label", "Begin Date"
'    Set nod2 = nod1.NewChild("end_date", "")
'                                                 nod2.AddAttribute "type", "Date"
'                                                 nod2.AddAttribute "format", "yyyy-mm-dd"
'                                                 nod2.AddAttribute "optional", "true"
'                                                 nod2.AddAttribute "label", "End Date"
'    Set nod2 = nod1.NewChild("cache_dir")
'                                                 nod2.AddAttribute "format", "savepath"
'                                                 nod2.AddAttribute "label", "Save in Directory"
      
  Set nod1 = Nothing
  Set nod2 = Nothing
End Sub

Private Property Get clsWebData_HelpId() As Long
  clsWebData_HelpId = pHelpID
End Property

Private Property Get clsWebData_Label() As String
  clsWebData_Label = pLabel
End Property

Private Property Get clsWebData_Name() As String
  clsWebData_Name = pClassName
End Property

Private Property Get clsWebData_Provides() As ChilkatXml
  Set clsWebData_Provides = pProvides
End Property

Private Function clsWebData_GetData(ByVal aQuery As ChilkatXml, ByRef aResult As ChilkatXml) As Boolean
  Dim lQueryChild As ChilkatXml
  Dim lCritChild As ChilkatXml
  Dim state_abbrev As String
  Dim begin_date As String
  Dim end_date As String
  Dim cache_dir As String
  Dim save_filename As String
 'Dim SHPfilename As String
 'Dim WDMfilename As String
 'Dim myDownloadFiles As Collection 'of file names
  Dim url As String
  Dim iSite As Long
  Dim lstSites As FastCollection
  Dim vSite As Variant
  Dim lSite As ChilkatXml
  Dim vAttr As Variant
  Dim iAttr As Long
  Dim siteAttributes As String
  Dim FirstFile As Boolean
  Dim StationsNoData As String
  Dim nStationsNoData As Long
  Dim FileNumber As Long
  Dim nodStatus As ChilkatXml
  Dim findPos As Long
  Dim Msg As String
  Dim suffix As String
  Dim base_url As String
  Dim project_dir As String
  Dim date_format As String
  Dim huc_cd As String
  Dim DBFfilename As String
  Dim DBFsAffected As New FastCollection
  Dim RDBfile As clsCSV
  Dim RDBrec As Long
  Dim dbfHUC As clsDBF
  Dim dbfHUCnew As clsDBF
  Dim fieldNum As Long
  Dim DBFrec As Long
  Dim numExistingDBFRecs As Long
  Dim fieldsToMatch(4)  As Long
  Dim fieldValues(4) As Variant
  Dim operator(4) As String
  
  Dim StatAgcyField As Long 'field for station number + agency code
  Dim dateField As Long 'field for date values
  Dim DateValue As Date
  Dim timeField As Long 'field for time values
  'Dim IdField As Long
  Dim ParmField As Long 'field containing parameter number
  Dim RemarkField As Long 'field containing remark code
  Dim ValuField As Long 'field containing data value
  Dim HucField As Long 'field containing 8-digit HUC
  
  On Error GoTo ErrHand
  
  pManager.LogDbg "  clsWebDataUsgsWQ GetData entry"

  operator(1) = "=" 'BSTAT_ID
  operator(2) = "=" 'TIME
  operator(3) = "=" 'PARM
  operator(4) = "=" 'DATE
  
  clsWebData_GetData = True
  base_url = pManager.CurrentStatusGetString("USGSWQBaseURL", "http://nwis.waterdata.usgs.gov/usa/")
  cache_dir = pManager.CurrentStatusGetString("cache_dir") & pClassName & "\"
  project_dir = pManager.CurrentStatusGetString("project_dir")
  suffix = pManager.CurrentStatusGetString("USGSWQsaveSuffix", "_wq.txt")
  date_format = pManager.CurrentStatusGetString("USGSWQdateFormat", "MM/DD/YYYY")
  
  MkDirPath cache_dir
  MkDirPath project_dir
  
  If Len(aQuery.ErrorLogText) > 0 Then
    pErrorDescription = "Parse Error:" & aQuery.ErrorLogText
    clsWebData_GetData = False
  Else
    Set lQueryChild = aQuery.FirstChild
    While Not lQueryChild Is Nothing
      Select Case LCase(lQueryChild.Tag)
        Case "criteria"
          Set lCritChild = lQueryChild.FirstChild
          While Not lCritChild Is Nothing
            Select Case LCase(lCritChild.Tag)
              Case "begin_date":     begin_date = lCritChild.Content
              Case "end_date":         end_date = lCritChild.Content
             'Case "state_abbrev": state_abbrev = lCritChild.Content
              Case "cache_dir":       cache_dir = lCritChild.Content & pClassName & "\"
             'Case "shpfilename":   SHPfilename = lCritChild.Content
             'Case "wdmfilename":   WDMfilename = lCritChild.Content
            End Select
            If lCritChild.NextSibling2 = 0 Then Set lCritChild = Nothing
          Wend
      End Select
      If lQueryChild.NextSibling2 = 0 Then Set lQueryChild = Nothing
    Wend
    
    'If Len(WDMfilename) > 0 Then Set myDownloadFiles = New Collection
    
    aResult.Tag = pClassName
    Set nodStatus = aResult.newChild("status", "")
    nodStatus.AddAttribute "complete", "false"
    
    FirstFile = True
    iSite = 0
    Set lstSites = GetChildrenWithTag(aQuery.GetChildWithTag("criteria"), "qw_site_no")
    If lstSites.Count = 0 Then
      Set lstSites = GetChildrenWithTag(aQuery.GetChildWithTag("criteria"), "site_no")
    End If
    For Each vSite In lstSites 'For iSite = 1 To nSites
      iSite = iSite + 1
      Set lSite = vSite
      'http://waterdata.usgs.gov/nwis/qwdata/?site_no=323614081442701
      'http://waterdata.usgs.gov/nwis/qwdata?site_no=323614081442701&agency_cd=USGS&begin_date=&end_date=&inventory_output=0&rdb_inventory_output=file&format=rdb&date_format=YYYY-MM-DD&rdb_compression=file&qw_sample_wide=0&submitted_form=brief_list
      url = base_url
      'url = GetSetting("WebDataManager", "USGSWQ", "BaseURL", "http://waterdata.usgs.gov/")
      If Len(state_abbrev) > 0 Then url = url & LCase(state_abbrev) & "/"
      url = url & "nwis/qwdata?site_no=" & lSite.Content & _
               "&agency_cd=USGS" & _
               "&begin_date=" & begin_date & _
               "&end_date=" & end_date & _
               "&format=rdb" & _
               "&date_format=" & date_format & _
               "&submitted_form=brief_list"
      huc_cd = lSite.GetAttrValue("huc_cd")
      siteAttributes = "# " & url & vbCrLf
      For iAttr = 0 To lSite.NumAttributes - 1
        siteAttributes = siteAttributes & "# " & lSite.GetAttributeName(iAttr) _
                                & Space(48 - Len(lSite.GetAttributeName(iAttr))) _
                                               & lSite.GetAttributeValue(iAttr) & vbCrLf
      Next
      save_filename = cache_dir & state_abbrev & lSite.Content & suffix
      
      If Not pManager.Download(url, save_filename, FirstFile, "Downloading " & pLabel & " (" & iSite & " of " & lstSites.Count & ")", siteAttributes) Then
        nodStatus.AddAttribute "message", "User Cancelled"

        Exit Function '!!!!!!!!!!!!!!!!!!!
      
      End If
      Msg = WholeFileString(save_filename)
      findPos = InStr(Msg, "<HTML>")
      If findPos > 0 Then
        Msg = Mid(pManager.ResultString, findPos)
        Kill save_filename
        nStationsNoData = nStationsNoData + 1
        StationsNoData = StationsNoData & "   " & FilenameOnly(save_filename)
        If InStr(Msg, "No data were found") > 0 Then
          StationsNoData = StationsNoData & " (no data)"
        Else
          findPos = InStr(Msg, "3>")
          If findPos > 0 Then
            Msg = Mid(Msg, findPos + 2)
            findPos = InStr(Msg, "</")
            If findPos > 0 Then Msg = Left(Msg, findPos - 1)
          End If
          If Len(Msg) < 80 Then
            StationsNoData = StationsNoData & " (" & Msg & ")"
          Else
            Msg = Mid(pManager.ResultString, InStr(pManager.ResultString, "<HTML>"))
            Msg = ReplaceString(Msg, "<HEAD>", "<HEAD><base href=""" & base_url & "nwis")
            pManager.ShowHTML Msg
          End If
        End If
        StationsNoData = StationsNoData & vbCrLf
      ElseIf Len(huc_cd) > 0 Then
        DBFfilename = project_dir & "wqobs\" & huc_cd & ".dbf"
        'If this file isn't already open, then open or create it
        If dbfHUC Is Nothing Then
          GoTo dbfHUCopen
        Else
          'If a different file is still open, close it first
          If DBFfilename <> dbfHUC.filename Then
'            GoSub dbfHUCclose
dbfHUCopen:
            FileNumber = DBFsAffected.IndexFromKey(DBFfilename)
            If FileNumber > 0 Then
              pManager.LogDbg "Using open " & DBFfilename
              Set dbfHUC = DBFsAffected.ItemByIndex(FileNumber)
            ElseIf FileExists(DBFfilename) Then
              pManager.LogDbg "Opening existing " & DBFfilename
              Set dbfHUC = New clsDBF
              dbfHUC.OpenDBF DBFfilename
              If dbfHUC.FieldLength(dbfHUC.FieldNumber("DATE")) < 8 Then
                pManager.LogDbg "Re-writing with 4-digit years: " & DBFfilename
                Set dbfHUCnew = ReformatBasObsWQ(dbfHUC)
                dbfHUC.Clear
                Set dbfHUC = Nothing
                Set dbfHUC = dbfHUCnew
                dbfHUC.WriteDBF DBFfilename
              End If
              DBFsAffected.Add dbfHUC, dbfHUC.filename
            Else
              pManager.LogDbg "Creating new " & DBFfilename
              Set dbfHUC = NewBasObsWQdbf
              dbfHUC.WriteDBF DBFfilename 'Writing sets dbfHUC.Filename so it can be saved later
              DBFsAffected.Add dbfHUC, dbfHUC.filename
            End If
            numExistingDBFRecs = dbfHUC.NumRecords
          
            For fieldNum = 1 To dbfHUC.numFields 'look for date and location fields
              Select Case UCase(Trim(dbfHUC.fieldName(fieldNum)))
                Case "BSTAT_ID": StatAgcyField = fieldNum
                Case "DATE":  dateField = fieldNum
                Case "TIME":  timeField = fieldNum
                'Case "ID":    IdField = fieldNum
                Case "PARM":  ParmField = fieldNum
                Case "VALUE": ValuField = fieldNum
                Case "REMARK", "R": RemarkField = fieldNum
                Case "BCU":   HucField = fieldNum
              End Select
            Next
            fieldsToMatch(1) = StatAgcyField  'BSTAT_ID
            fieldsToMatch(2) = timeField  'TIME
            fieldsToMatch(3) = ParmField  'PARM
            fieldsToMatch(4) = dateField  'DATE
          End If
        End If
        
        Set RDBfile = New clsCSV
        RDBfile.Delimiter = vbTab
        RDBfile.OpenCSV save_filename
                
        For RDBrec = 2 To RDBfile.NumRecords 'Skip header in first record
          pManager.DownloadStatus = "Merging record " & RDBrec & " of " & RDBfile.NumRecords & " from " & lSite.Content & " into " & dbfHUC.filename
          RDBfile.CurrentRecord = RDBrec
          fieldValues(1) = RDBfile.Value(2) & "+" & RDBfile.Value(1) 'site_no + agency_cd -> BSTAT_ID
          fieldValues(2) = ReplaceString(RDBfile.Value(4), ":", "") 'sample_tm -> TIME
          fieldValues(3) = RDBfile.Value(5)   'parameter_cd -> PARM
          On Error GoTo ErrDateFormat
          DateValue = RDBfile.Value(3)
          fieldValues(4) = Format(DateValue, "YYYYMMDD") 'sample_dt -> DATE
SkipSetDate:
          On Error GoTo ErrHand
          DBFrec = dbfHUC.FindMatch(fieldsToMatch, operator, fieldValues, , , numExistingDBFRecs)
          If DBFrec > 0 Then
            pManager.LogDbg "Found record at " & DBFrec & " matching " & fieldValues(1) & " " & fieldValues(2) & " " & fieldValues(3) & " " & fieldValues(4)
          Else
            'pManager.LogDbg "Adding record " & fieldValues(1) & " " & fieldValues(2) & " " & fieldValues(3) & " " & fieldValues(4)
            With dbfHUC
              .CurrentRecord = .NumRecords + 1
              For fieldNum = 1 To dbfHUC.numFields
                .Value(fieldNum) = ""
              Next
              .Value(StatAgcyField) = fieldValues(1)
              .Value(dateField) = fieldValues(4)
              .Value(timeField) = fieldValues(2)
              .Value(ParmField) = fieldValues(3)
              .Value(ValuField) = RDBfile.Value(6)   'result_va -> VALUE
              .Value(RemarkField) = RDBfile.Value(7) 'remark_cd -> REMARK
              .Value(HucField) = huc_cd          'huc_cd -> BCU
              'If Not IsNumeric(.Value(ValuField)) Then Stop
            End With
          End If
        Next
        RDBfile.Clear
        Set RDBfile = Nothing
      Else
        pManager.LogDbg "No HUC found, so saving " & pLabel & " data to " & project_dir & FilenameNoPath(save_filename)
        FileCopy save_filename, project_dir & FilenameNoPath(save_filename)
      End If
      FirstFile = False
    Next
'    GoSub dbfHUCclose

    For FileNumber = 1 To DBFsAffected.Count
      Set dbfHUC = DBFsAffected.ItemByIndex(FileNumber)
      pManager.DownloadStatus = "Saving " & dbfHUC.filename
      dbfHUC.WriteDBF dbfHUC.filename
      pManager.DownloadStatus = "Sorting " & dbfHUC.filename & " by date"
      SortBasObsWQbyDateObj dbfHUC
      pManager.DownloadStatus = "Finished with " & dbfHUC.filename
      Set dbfHUC = Nothing
    Next

    If nStationsNoData > 0 Then
      save_filename = project_dir & "USGSwqNoData.txt"
      FileNumber = 0
      While Len(Dir(save_filename)) > 0
        FileNumber = FileNumber + 1
        save_filename = project_dir & "USGSwqNoData(" & FileNumber & ").txt"
      Wend
      If nStationsNoData > 10 Then
        findPos = 1
        For FileNumber = 1 To 10
          findPos = InStr(findPos + 1, StationsNoData, vbCr)
        Next
        Msg = Left(StationsNoData, findPos) & " (and " & (nStationsNoData - 10) & " more)"
      Else
        Msg = StationsNoData
      End If
      If pManager.LogMsg("Did not find data for " & nStationsNoData & " stations: " & vbCr & vbCr _
               & Msg & vbCr _
               & "Save this list to " & save_filename & "?", _
               "Some data not found", "+&Yes", "-&No") = 1 Then
        SaveFileString save_filename, StationsNoData
      End If
    End If
    
    nodStatus.AddAttribute "complete", "true"
  End If

  pManager.LogDbg "  clsWebDataUsgsWQ GetData exit"
  
  Exit Function
ErrHand:
  pManager.LogMsg "Error '" & Err.Description & "'", pLabel & " GetData"
  Exit Function

ErrDateFormat:
  fieldValues(4) = ReplaceString(RDBfile.Value(3), "/", "")
  If Len(fieldValues(4)) = 8 Then 'Probably we have MMDDYYYY
    fieldValues(4) = Right(fieldValues(4), 4) & Left(fieldValues(4), 4) 'Move YYYY to the beginning
    If Right(fieldValues(4), 2) = "00" Then fieldValues(4) = Left(fieldValues(4), 7) & "1" 'Change bogus day "00" to "01"
  End If
  pManager.LogDbg "Error formatting date: " & RDBfile.Value(3) & " written as " & fieldValues(4)
  Resume SkipSetDate

'dbfHUCclose:
'  If Not dbfHUC Is Nothing Then
'    'If dbfHUC.NumRecords > numExistingDBFRecs Then
'    '  dbfHUC.WriteDBF dbfHUC.Filename
'    '  If DBFsAffected.IndexFromKey(dbfHUC.Filename) = 0 Then DBFsAffected.Add dbfHUC, dbfHUC.Filename
'    'End If
'    Set dbfHUC = Nothing
'  End If
'  Return

End Function

Private Property Set clsWebData_Manager(myManager As clsWebDataManager)
  Set pManager = Nothing
  Set pManager = myManager
End Property

Private Property Get clsWebData_Manager() As clsWebDataManager
  Set clsWebData_Manager = pManager
End Property

Private Sub clsWebData_Specify(Optional parent As Object = Nothing, Optional ByVal Index As Long = 0)
  pManager.CurrentStatusUpdateString "USGSSiteqw", "qw", "set by " & pLabel
  pManager.ShowCriteria Me, parent, Index
End Sub

Private Property Get clsWebData_Version() As String
  clsWebData_Version = App.Major & "." & App.Minor & "." & App.Revision
End Property
