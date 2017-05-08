VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsPCSPermit"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True

Option Explicit
Implements clsWebData

Private Const pLabel = "PCS Permit"
Private Const pClassName = "clsPCSPermit"
Private pBaseUrl As String
Private Const pCSVprefix = "Click on the underlined filename : <a href="
Private Const pNoRecordsString = "No CSV output file is available, because your query retrieved no records."
Private pProvides As DOMDocument
Private pManager As clsWebDataManager

Private Sub Class_Initialize()
  Dim root As IXMLDOMElement
  Dim nod1 As IXMLDOMElement
  Dim nod2 As IXMLDOMElement
  Dim nod3 As IXMLDOMElement
  
  Set pProvides = New DOMDocument
  Set pProvides.documentElement = pProvides.createElement(pClassName)
  Set root = pProvides.documentElement
  Set nod1 = pProvides.createElement("output")
    root.appendChild nod1
    Set nod2 = pProvides.createElement("file")
                                                 nod2.setAttribute "format", "csv"
    nod1.appendChild nod2
  
  Set nod1 = pProvides.createElement("criteria")
    root.appendChild nod1
    Set nod2 = pProvides.createElement("npdes")
                                                 nod2.setAttribute "multiple", "true"
                                                 nod2.setAttribute "label", "NPDES #"
    nod1.appendChild nod2
    
    Set nod2 = pProvides.createElement("save_dir")
                                                 nod2.setAttribute "format", "savepath" ' jk 5/27/03
                                                 nod2.setAttribute "label", "Save in Directory"
    nod1.appendChild nod2
      
  Set root = Nothing
  Set nod1 = Nothing
  Set nod2 = Nothing

End Sub

Private Function BuildQueryURL_PCS_CUR_EFFL_LIM(npdes As String) As String
  Dim URL As String
  URL = pBaseUrl & "database_type=PCS"
  URL = URL & "&fac_name=npdes#"
  URL = URL & "&fac_search=Equal to"
  URL = URL & "&Value=" & npdes
  
  URL = URL & "&zipcode="
  URL = URL & "&usgs_huc="
  URL = URL & "&city="
  URL = URL & "&county="
  URL = URL & "&state="
  URL = URL & "&sic_code="
  URL = URL & "&chemsearch=chemname"
  URL = URL & "&chemstatus=Equal to"
  URL = URL & "&chemvalue="
  URL = URL & "&where_selection=dummy"
  URL = URL & "&table_1=PCS_CUR_EFFL_LIM.NPDES"
  URL = URL & "&table_1=PCS_CUR_EFFL_LIM.CHANGE_OF_LIMIT_STAT"
  URL = URL & "&table_1=PCS_CUR_EFFL_LIM.CONTESTED_PARAM_IND"
  URL = URL & "&table_1=PCS_CUR_EFFL_LIM.DISCHARGE_NUM"
  URL = URL & "&table_1=PCS_CUR_EFFL_LIM.DOCKET_NUMBER"
  URL = URL & "&table_1=PCS_CUR_EFFL_LIM.LIMIT_TYPE"
  URL = URL & "&table_1=PCS_CUR_EFFL_LIM.MODIF_NUM"
  URL = URL & "&table_1=PCS_CUR_EFFL_LIM.MODIF_PERIOD_END_DATE"
  URL = URL & "&table_1=PCS_CUR_EFFL_LIM.MODIF_PERIOD_START_DATE"
  URL = URL & "&table_1=PCS_CUR_EFFL_LIM.MONITORING_LOC"
  URL = URL & "&table_1=PCS_CUR_EFFL_LIM.PARAM_CODE"
  URL = URL & "&table_1=PCS_CUR_EFFL_LIM.PIPE_SET_QUALIFIER"
  URL = URL & "&table_1=PCS_CUR_EFFL_LIM.REPORT_DESIG"
  URL = URL & "&table_1=PCS_CUR_EFFL_LIM.SEASON_NUM"
  URL = URL & "&table1_type= "
  URL = URL & StrRepeat(13, "&table1_type=Equal to")
  URL = URL & StrRepeat(14, "&table1_value=")
  URL = URL & StrRepeat(14, "&column_number=")
  URL = URL & StrRepeat(14, "&sort_selection=")
  URL = URL & StrRepeat(14, "&sort_order=Ascending")
  URL = URL & "&group_sequence=test"
  URL = URL & "&csv_output=Output to CSV File"
  BuildQueryURL_PCS_CUR_EFFL_LIM = ReplaceString(URL, " ", "%20")
End Function


Private Function BuildQueryURL_PCS_CUR_EFFL_LIM_CONCENTR(npdes As String) As String
  Dim URL As String
  URL = pBaseUrl & "database_type=PCS"
  URL = URL & "&fac_name=npdes#"
  URL = URL & "&fac_search=Equal to"
  URL = URL & "&Value=" & npdes
  

  URL = URL & "&zipcode="
  URL = URL & "&usgs_huc="
  URL = URL & "&city="
  URL = URL & "&county="
  URL = URL & "&state="
  URL = URL & "&sic_code="
  URL = URL & "&chemsearch=chemname"
  URL = URL & "&chemstatus=Equal to"
  URL = URL & "&chemvalue="
  URL = URL & "&where_selection=dummy"
  URL = URL & "&table_1=PCS_CUR_EFFL_LIM_CONCENTR.NPDES"
  URL = URL & "&table_1=PCS_CUR_EFFL_LIM_CONCENTR.CONC_AVG_LIMIT"
  URL = URL & "&table_1=PCS_CUR_EFFL_LIM_CONCENTR.CONC_AVG_LIM_STAT_BASE"
  URL = URL & "&table_1=PCS_CUR_EFFL_LIM_CONCENTR.CONC_MAX_LIMIT"
  URL = URL & "&table_1=PCS_CUR_EFFL_LIM_CONCENTR.CONC_MAX_LIM_STAT_BASE"
  URL = URL & "&table_1=PCS_CUR_EFFL_LIM_CONCENTR.CONC_MIN_LIMIT"
  URL = URL & "&table_1=PCS_CUR_EFFL_LIM_CONCENTR.CONC_MIN_LIM_STAT_BASE"
  URL = URL & "&table_1=PCS_CUR_EFFL_LIM_CONCENTR.CONC_UNIT_CODE"
  URL = URL & "&table_1=PCS_CUR_EFFL_LIM_CONCENTR.DISCHARGE_NUM"
  URL = URL & "&table_1=PCS_CUR_EFFL_LIM_CONCENTR.LIMIT_TYPE"
  URL = URL & "&table_1=PCS_CUR_EFFL_LIM_CONCENTR.MODIF_NUM"
  URL = URL & "&table_1=PCS_CUR_EFFL_LIM_CONCENTR.MONITORING_LOC"
  URL = URL & "&table_1=PCS_CUR_EFFL_LIM_CONCENTR.PARAM_CODE"
  URL = URL & "&table_1=PCS_CUR_EFFL_LIM_CONCENTR.PIPE_SET_QUALIFIER"
  URL = URL & "&table_1=PCS_CUR_EFFL_LIM_CONCENTR.REPORT_DESIG"
  URL = URL & "&table_1=PCS_CUR_EFFL_LIM_CONCENTR.SEASON_NUM"
  URL = URL & "&table_1=PCS_CUR_EFFL_LIM_CONCENTR.STAT_LIM_AVG_OVERRIDE"
  URL = URL & "&table_1=PCS_CUR_EFFL_LIM_CONCENTR.STAT_LIM_MIN_OVERRIDE"
  URL = URL & "&table1_type= "
  URL = URL & StrRepeat(17, "&table1_type=Equal to")
  URL = URL & StrRepeat(18, "&table1_value=")
  URL = URL & StrRepeat(18, "&column_number=")
  URL = URL & StrRepeat(18, "&sort_selection=")
  URL = URL & StrRepeat(18, "&sort_order=Ascending")
  URL = URL & "&group_sequence=test"
  URL = URL & "&csv_output=Output to CSV File"
  BuildQueryURL_PCS_CUR_EFFL_LIM_CONCENTR = ReplaceString(URL, " ", "%20")
End Function

Private Function BuildQueryURL_PCS_CUR_EFFL_LIM_QTY(npdes As String) As String
  Dim URL As String
  URL = pBaseUrl & "database_type=PCS"
  URL = URL & "&fac_name=npdes#"
  URL = URL & "&fac_search=Equal to"
  URL = URL & "&Value=" & npdes

  URL = URL & "&zipcode="
  URL = URL & "&usgs_huc="
  URL = URL & "&city="
  URL = URL & "&county="
  URL = URL & "&state="
  URL = URL & "&sic_code="
  URL = URL & "&chemsearch=chemname"
  URL = URL & "&chemstatus=Equal to"
  URL = URL & "&chemvalue="
  URL = URL & "&where_selection=dummy"
  URL = URL & "&table_1=PCS_CUR_EFFL_LIM_QTY.NPDES"
  URL = URL & "&table_1=PCS_CUR_EFFL_LIM_QTY.DISCHARGE_NUM"
  URL = URL & "&table_1=PCS_CUR_EFFL_LIM_QTY.LIMIT_TYPE"
  URL = URL & "&table_1=PCS_CUR_EFFL_LIM_QTY.MODIF_NUM"
  URL = URL & "&table_1=PCS_CUR_EFFL_LIM_QTY.MONITORING_LOC"
  URL = URL & "&table_1=PCS_CUR_EFFL_LIM_QTY.PARAM_CODE"
  URL = URL & "&table_1=PCS_CUR_EFFL_LIM_QTY.PIPE_SET_QUALIFIER"
  URL = URL & "&table_1=PCS_CUR_EFFL_LIM_QTY.QTY_AVG_LIMIT"
  URL = URL & "&table_1=PCS_CUR_EFFL_LIM_QTY.QTY_AVG_LIM_STAT_BASE"
  URL = URL & "&table_1=PCS_CUR_EFFL_LIM_QTY.QTY_MAX_LIMIT"
  URL = URL & "&table_1=PCS_CUR_EFFL_LIM_QTY.QTY_MAX_LIM_STAT_BASE"
  URL = URL & "&table_1=PCS_CUR_EFFL_LIM_QTY.QTY_UNIT_CODE"
  URL = URL & "&table_1=PCS_CUR_EFFL_LIM_QTY.REPORT_DESIG"
  URL = URL & "&table_1=PCS_CUR_EFFL_LIM_QTY.SEASON_NUM"
  URL = URL & "&table1_type= "
  URL = URL & StrRepeat(13, "&table1_type=Equal to")
  URL = URL & StrRepeat(14, "&table1_value=")
  URL = URL & StrRepeat(14, "&column_number=")
  URL = URL & StrRepeat(14, "&sort_selection=")
  URL = URL & StrRepeat(14, "&sort_order=Ascending")
  URL = URL & "&group_sequence=test"
  URL = URL & "&csv_output=Output to CSV File"
  BuildQueryURL_PCS_CUR_EFFL_LIM_QTY = ReplaceString(URL, " ", "%20")
End Function

Private Function BuildQueryURL_PCS_EFFL_LIM(npdes As String) As String
  Dim URL As String
  URL = pBaseUrl & "database_type=PCS"
  URL = URL & "&fac_name=npdes#"
  URL = URL & "&fac_search=Equal to"
  URL = URL & "&Value=" & npdes
  URL = URL & "&zipcode="
  URL = URL & "&usgs_huc="
  URL = URL & "&city="
  URL = URL & "&county="
  URL = URL & "&state="
  URL = URL & "&sic_code="
  URL = URL & "&chemsearch=chemname"
  URL = URL & "&chemstatus=Equal to"
  URL = URL & "&chemvalue="
  URL = URL & "&where_selection=dummy"
  URL = URL & "&table_1=PCS_EFFL_LIM.NPDES"
  URL = URL & "&table_1=PCS_EFFL_LIM.CHANGE_OF_LIMIT_STAT"
  URL = URL & "&table_1=PCS_EFFL_LIM.CONTESTED_PARAM_IND"
  URL = URL & "&table_1=PCS_EFFL_LIM.DISCHARGE_NUM"
  URL = URL & "&table_1=PCS_EFFL_LIM.DOCKET_NUMBER"
  URL = URL & "&table_1=PCS_EFFL_LIM.LIMIT_TYPE"
  URL = URL & "&table_1=PCS_EFFL_LIM.MODIF_NUM"
  URL = URL & "&table_1=PCS_EFFL_LIM.MODIF_PERIOD_END_DATE"
  URL = URL & "&table_1=PCS_EFFL_LIM.MODIF_PERIOD_START_DATE"
  URL = URL & "&table_1=PCS_EFFL_LIM.MONITORING_LOC"
  URL = URL & "&table_1=PCS_EFFL_LIM.PARAM_CODE"
  URL = URL & "&table_1=PCS_EFFL_LIM.PIPE_SET_QUALIFIER"
  URL = URL & "&table_1=PCS_EFFL_LIM.REPORT_DESIG"
  URL = URL & "&table_1=PCS_EFFL_LIM.SEASON_NUM"
  URL = URL & "&table1_type= "
  URL = URL & StrRepeat(13, "&table1_type=Equal to")
  URL = URL & StrRepeat(14, "&table1_value=")
  URL = URL & StrRepeat(14, "&column_number=")
  URL = URL & StrRepeat(14, "&sort_selection=")
  URL = URL & StrRepeat(14, "&sort_order=Ascending")
  URL = URL & "&group_sequence=test"
  URL = URL & "&csv_output=Output to CSV File"
  BuildQueryURL_PCS_EFFL_LIM = ReplaceString(URL, " ", "%20")
End Function

Private Function BuildQueryURL_PCS_EFFL_LIM_CONCENTR(npdes As String) As String
  Dim URL As String
  URL = pBaseUrl & "database_type=PCS"
  URL = URL & "&fac_name=npdes#"
  URL = URL & "&fac_search=Equal to"
  URL = URL & "&Value=" & npdes
  URL = URL & "&zipcode="
  URL = URL & "&usgs_huc="
  URL = URL & "&city="
  URL = URL & "&county="
  URL = URL & "&state="
  URL = URL & "&sic_code="
  URL = URL & "&chemsearch=chemname"
  URL = URL & "&chemstatus=Equal to"
  URL = URL & "&chemvalue="
  URL = URL & "&where_selection=dummy"
  URL = URL & "&table_1=PCS_EFFL_LIM_CONCENTR.NPDES"
  URL = URL & "&table_1=PCS_EFFL_LIM_CONCENTR.CONC_AVG_LIMIT"
  URL = URL & "&table_1=PCS_EFFL_LIM_CONCENTR.CONC_AVG_LIM_STAT_BASE"
  URL = URL & "&table_1=PCS_EFFL_LIM_CONCENTR.CONC_MAX_LIMIT"
  URL = URL & "&table_1=PCS_EFFL_LIM_CONCENTR.CONC_MAX_LIM_STAT_BASE"
  URL = URL & "&table_1=PCS_EFFL_LIM_CONCENTR.CONC_MIN_LIMIT"
  URL = URL & "&table_1=PCS_EFFL_LIM_CONCENTR.CONC_MIN_LIM_STAT_BASE"
  URL = URL & "&table_1=PCS_EFFL_LIM_CONCENTR.CONC_UNIT_CODE"
  URL = URL & "&table_1=PCS_EFFL_LIM_CONCENTR.DISCHARGE_NUM"
  URL = URL & "&table_1=PCS_EFFL_LIM_CONCENTR.LIMIT_TYPE"
  URL = URL & "&table_1=PCS_EFFL_LIM_CONCENTR.MODIF_NUM"
  URL = URL & "&table_1=PCS_EFFL_LIM_CONCENTR.MONITORING_LOC"
  URL = URL & "&table_1=PCS_EFFL_LIM_CONCENTR.PARAM_CODE"
  URL = URL & "&table_1=PCS_EFFL_LIM_CONCENTR.PIPE_SET_QUALIFIER"
  URL = URL & "&table_1=PCS_EFFL_LIM_CONCENTR.REPORT_DESIG"
  URL = URL & "&table_1=PCS_EFFL_LIM_CONCENTR.SEASON_NUM"
  URL = URL & "&table_1=PCS_EFFL_LIM_CONCENTR.STAT_LIM_AVG_OVERRIDE"
  URL = URL & "&table_1=PCS_EFFL_LIM_CONCENTR.STAT_LIM_MIN_OVERRIDE"
  URL = URL & "&table1_type= "
  URL = URL & StrRepeat(17, "&table1_type=Equal to")
  URL = URL & StrRepeat(18, "&table1_value=")
  URL = URL & StrRepeat(18, "&column_number=")
  URL = URL & StrRepeat(18, "&sort_selection=")
  URL = URL & StrRepeat(18, "&sort_order=Ascending")
  URL = URL & "&group_sequence=test"
  URL = URL & "&csv_output=Output to CSV File"
  BuildQueryURL_PCS_EFFL_LIM_CONCENTR = ReplaceString(URL, " ", "%20")
End Function

Private Function BuildQueryURL_PCS_EFFL_LIM_QTY(npdes As String) As String
  Dim URL As String
  URL = pBaseUrl & "database_type=PCS"
  URL = URL & "&fac_name=npdes#"
  URL = URL & "&fac_search=Equal to"
  URL = URL & "&Value=" & npdes
  URL = URL & "&zipcode="
  URL = URL & "&usgs_huc="
  URL = URL & "&city="
  URL = URL & "&county="
  URL = URL & "&state="
  URL = URL & "&sic_code="
  URL = URL & "&chemsearch=chemname"
  URL = URL & "&chemstatus=Equal to"
  URL = URL & "&chemvalue="
  URL = URL & "&where_selection=dummy"
  URL = URL & "&table_1=PCS_EFFL_LIM_QTY.NPDES"
  URL = URL & "&table_1=PCS_EFFL_LIM_QTY.DISCHARGE_NUM"
  URL = URL & "&table_1=PCS_EFFL_LIM_QTY.LIMIT_TYPE"
  URL = URL & "&table_1=PCS_EFFL_LIM_QTY.MODIF_NUM"
  URL = URL & "&table_1=PCS_EFFL_LIM_QTY.MONITORING_LOC"
  URL = URL & "&table_1=PCS_EFFL_LIM_QTY.PARAM_CODE"
  URL = URL & "&table_1=PCS_EFFL_LIM_QTY.PIPE_SET_QUALIFIER"
  URL = URL & "&table_1=PCS_EFFL_LIM_QTY.QTY_AVG_LIMIT"
  URL = URL & "&table_1=PCS_EFFL_LIM_QTY.QTY_AVG_LIM_STAT_BASE"
  URL = URL & "&table_1=PCS_EFFL_LIM_QTY.QTY_MAX_LIMIT"
  URL = URL & "&table_1=PCS_EFFL_LIM_QTY.QTY_MAX_LIM_STAT_BASE"
  URL = URL & "&table_1=PCS_EFFL_LIM_QTY.QTY_UNIT_CODE"
  URL = URL & "&table_1=PCS_EFFL_LIM_QTY.REPORT_DESIG"
  URL = URL & "&table_1=PCS_EFFL_LIM_QTY.SEASON_NUM"
  URL = URL & "&table1_type= "
  URL = URL & StrRepeat(13, "&table1_type=Equal to")
  URL = URL & StrRepeat(14, "&table1_value=")
  URL = URL & StrRepeat(14, "&column_number=")
  URL = URL & StrRepeat(14, "&sort_selection=")
  URL = URL & StrRepeat(14, "&sort_order=Ascending")
  URL = URL & "&group_sequence=test"
  URL = URL & "&csv_output=Output to CSV File"
  BuildQueryURL_PCS_EFFL_LIM_QTY = ReplaceString(URL, " ", "%20")
End Function

Private Function clsWebData_GetData(ByVal Query As MSXML.IXMLDOMDocument, Result As MSXML.IXMLDOMDocument) As Boolean
  Dim Filename As String
  Dim npdes As Variant
  Dim save_dir As String
  Dim outputFormat As String
  Dim FirstFile As Boolean
  Dim ErrMsg As String
  Dim FilesNotCreated As String
  Dim nFilesNotCreated As Long
  Dim FileNumber As Long
  Dim findPos As Long
  Dim msg As String
    
  Dim root As IXMLDOMElement
  Dim nodStatus As IXMLDOMElement
  Dim nod2 As IXMLDOMElement
  Dim nod3 As IXMLDOMElement
  Dim nodList As IXMLDOMNodeList
  
  Dim lstSites As IXMLDOMNodeList
  
  Dim URL As String
      
  pManager.Logger.Log "  clsWebDataPCSPermit GetData entry"
  clsWebData_GetData = True
  
  Dim iChild As Long, jChild As Long, iSite As Long
  ReDim site_no(0)
  Filename = ""
  outputFormat = "XML"
  For iChild = 0 To Query.childNodes(0).childNodes.Length - 1
    With Query.childNodes(0).childNodes.Item(iChild)
      Select Case LCase(.nodeName)
        Case "criteria"
          For jChild = 0 To .childNodes.Length - 1
            With .childNodes.Item(jChild)
              Select Case LCase(.nodeName)
                'Case "npdes": npdes = .childNodes(0).nodeValue
                Case "save_dir": save_dir = .childNodes(0).nodeValue
              End Select
            End With
          Next
      End Select
    End With
  Next

  Set Result.documentElement = pProvides.createElement(pClassName)
  Set root = Result.documentElement
  Set nodStatus = Result.createElement("status")
  nodStatus.setAttribute "complete", "false"
  root.appendChild nodStatus
  
  pBaseUrl = pManager.CurrentStatusGetString("PCSPermitBaseURL", _
              "http://oaspub.epa.gov/enviro/ad_hoc_build_sql.get_table?")
  
  FirstFile = True
  Set lstSites = Query.getElementsByTagName("npdes")
  For Each npdes In lstSites
  
    'Get PCS_CUR_EFFL_LIM table
    Filename = save_dir & npdes.Text & ".PCS_CUR_EFFL_LIM.CSV"
    ErrMsg = PermitDownload(BuildQueryURL_PCS_CUR_EFFL_LIM(npdes.Text), Filename, FirstFile)
    GoSub AddFilenameNode
    
    FirstFile = False

    'Get PCS_CUR_EFFL_LIM_CONCENTR table
    Filename = save_dir & npdes.Text & ".PCS_CUR_EFFL_LIM_CONCENTR.CSV"
    ErrMsg = PermitDownload(BuildQueryURL_PCS_CUR_EFFL_LIM_CONCENTR(npdes.Text), Filename, FirstFile)
    GoSub AddFilenameNode
  
    'Get PCS_CUR_EFFL_LIM_QTY table
    Filename = save_dir & npdes.Text & ".PCS_CUR_EFFL_LIM_QTY.CSV"
    ErrMsg = PermitDownload(BuildQueryURL_PCS_CUR_EFFL_LIM_QTY(npdes.Text), Filename, FirstFile)
    GoSub AddFilenameNode
  
    'Get PCS_EFFL_LIM table
    Filename = save_dir & npdes.Text & ".PCS_EFFL_LIM.CSV"
    ErrMsg = PermitDownload(BuildQueryURL_PCS_EFFL_LIM(npdes.Text), Filename, FirstFile)
    GoSub AddFilenameNode
    
    'Get PCS_EFFL_LIM_CONCENTR table
    Filename = save_dir & npdes.Text & ".PCS_EFFL_LIM_CONCENTR.CSV"
    ErrMsg = PermitDownload(BuildQueryURL_PCS_EFFL_LIM_CONCENTR(npdes.Text), Filename, FirstFile)
    GoSub AddFilenameNode
    
    'Get PCS_EFFL_LIM_QTY table
    Filename = save_dir & npdes.Text & ".PCS_EFFL_LIM_QTY.CSV"
    ErrMsg = PermitDownload(BuildQueryURL_PCS_EFFL_LIM_QTY(npdes.Text), Filename, FirstFile)
    GoSub AddFilenameNode
    
    
    nodStatus.setAttribute "complete", "true"
  Next
  
EndOfFunction:
  If nFilesNotCreated > 0 Then
    Filename = save_dir & "PCS_PermitNoData.txt"
    While Len(Dir(Filename)) > 0
      FileNumber = FileNumber + 1
      Filename = save_dir & "PCS_PermitNoData(" & FileNumber & ").txt"
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
'    If MsgBox("Did not create " & nFilesNotCreated & " files: " & vbCr & vbCr _
'             & msg & vbCr _
'             & "because there were no matching records in the database." & vbCr _
'             & "Save this list to " & Filename & "?", _
'             vbYesNo, _
'             "Some data not in database") = vbYes Then
    SaveFileString Filename, FilesNotCreated
'    End If
    pManager.Logger.Log pLabel & " did not download " & nFilesNotCreated & " files: " & vbCr & msg & vbCr _
             & "because there were no matching records in the database." & vbCr _
             & "List saved to " & Filename
  End If
  pManager.Logger.Log "  clsWebDataPCSFacility GetData exit"
  Exit Function

AddFilenameNode: '(local subroutine)
  Select Case ErrMsg
    Case "cancel"
                  GoTo EndOfFunction
    Case "success"
                  Set nod2 = Result.createElement("file")
                  nod2.setAttribute "name", Filename
                  root.appendChild nod2
    Case "no records"
                  FilesNotCreated = FilesNotCreated & "   " & FilenameNoPath(Filename) & vbCrLf
                  nFilesNotCreated = nFilesNotCreated + 1
    Case Else
                  Debug.Print "GetData: " & ErrMsg
  End Select
  Return

End Function

Private Function PermitDownload(ByVal URL As String, Filename As String, FirstFile As Boolean) As String
  Dim retval As String
  Dim URLedge As Integer
  
  retval = "success"
  
  'Get page telling us CSV file name
  If Not pManager.Download(URL, "", FirstFile, "Waiting for generation of " & pLabel & " " & Filename) Then
    retval = "cancel"
  ElseIf pManager.ResultString <> "<undefined/>" Then
    URL = pManager.ResultString
    
    'Find URL of CSV file
    URLedge = InStr(URL, pCSVprefix)
    If URLedge = 0 Then
      If InStr(URL, pNoRecordsString) > 0 Then
        retval = "no records"
      Else
        retval = "Could not find CSV URL in the output:" & vbCr & vbCr & URL
      End If
    Else
      URL = Trim(Mid(URL, URLedge + Len(pCSVprefix)))
      URLedge = InStr(URL, ">")
      If URLedge = 0 Then
        retval = "Could not find CSV URL in the output:" & vbCr & vbCr & URL
      Else
        URL = Left(URL, URLedge - 1)
        If Not pManager.Download(URL, Filename, False, "Downloading " & pLabel & " " & Filename) Then
          retval = "Error downloading PCS Facility data " & vbCr & vbCr & URL
        End If
      End If
    End If
  End If
  PermitDownload = retval
End Function

Private Property Get clsWebData_HelpId() As Long
  clsWebData_HelpId = 13
End Property

Private Property Get clsWebData_Label() As String
  clsWebData_Label = pLabel
End Property

Private Property Set clsWebData_Manager(myManager As ATCWebDataManager.clsWebDataManager)
  Set pManager = Nothing
  Set pManager = myManager
End Property

Private Property Get clsWebData_Manager() As ATCWebDataManager.clsWebDataManager
  Set clsWebData_Manager = pManager
End Property

Private Property Get clsWebData_Name() As String
  clsWebData_Name = pClassName
End Property

Private Property Get clsWebData_Provides() As MSXML.IXMLDOMDocument
  Set clsWebData_Provides = pProvides
End Property

Private Sub clsWebData_Specify(Optional parent As Object = Nothing, Optional ByVal Index As Long = 0)
  'If Len(pManager.CurrentStatusGetString("state_nlcd")) = 0 Then AddAllStates
  pManager.ShowCriteria Me, parent, Index
End Sub

Private Property Get clsWebData_Version() As String
  clsWebData_Version = App.Major & "." & App.Minor & "." & App.Revision
End Property

