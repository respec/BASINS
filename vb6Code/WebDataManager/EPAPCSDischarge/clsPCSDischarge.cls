VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsPCSDischarge"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True

Option Explicit
Implements clsWebData

Private Const pEnableImportingToDBF As Boolean = False

Private Const pLabel As String = "PCS Discharge"
Private Const pClassName As String = "clsPCSDischarge"
Private Const pHelpID = 30
Private pBaseURL As String
Private Const pCSVprefix As String = "Click on the underlined filename : <a href="
Private Const pNoRecordsString As String = "No CSV output file is available, because your query retrieved no records."
Private Const pNoRecordsMessage As String = "no records"
Private pProvides As ChilkatXml
Private pManager As clsWebDataManager

Private Sub Class_Initialize()
  Dim root As ChilkatXml
  Dim nod1 As ChilkatXml
  Dim nod2 As ChilkatXml
  
  Set pProvides = New ChilkatXml
  pProvides.Tag = pClassName
  Set nod1 = pProvides.newChild("output", "")
  Set nod2 = nod1.newChild("file", "")
      nod2.AddAttribute "format", "csv"
  
  Set nod1 = pProvides.newChild("criteria", "")
  Set nod2 = nod1.newChild("npdes", "")
      nod2.AddAttribute "multiple", "true"
      nod2.AddAttribute "label", "NPDES #"
      
  Set nod1 = Nothing
  Set nod2 = Nothing

End Sub

'Private Function BuildQueryURL_PCS_CUR_DMR_MEASUREMENT(npdes As String) As String
'  Dim URL As String
'  URL = pBaseURL & "database_type=PCS"
'  URL = URL & "&fac_name=npdes#"
'  URL = URL & "&fac_search=Equal to"
'  URL = URL & "&Value=" & npdes
'
'  URL = URL & "&zipcode="
'  URL = URL & "&usgs_huc="
'  URL = URL & "&city="
'  URL = URL & "&county="
'  URL = URL & "&state="
'  URL = URL & "&sic_code="
'  URL = URL & "&chemsearch=chemname"
'  URL = URL & "&chemstatus=Equal to"
'  URL = URL & "&chemvalue="
'  URL = URL & "&where_selection=dummy"
'  URL = URL & "&table_1=PCS_CUR_DMR_MEASUREMENT.NPDES"
'  URL = URL & "&table_1=PCS_CUR_DMR_MEASUREMENT.CONCENTRATION_UNIT_CODE"
'  URL = URL & "&table_1=PCS_CUR_DMR_MEASUREMENT.CONCENTR_AVG"
'  URL = URL & "&table_1=PCS_CUR_DMR_MEASUREMENT.CONCENTR_MAX"
'  URL = URL & "&table_1=PCS_CUR_DMR_MEASUREMENT.CONCENTR_MIN"
'  URL = URL & "&table_1=PCS_CUR_DMR_MEASUREMENT.DISCHARGE_NUM"
'  URL = URL & "&table_1=PCS_CUR_DMR_MEASUREMENT.LIMIT_TYPE"
'  URL = URL & "&table_1=PCS_CUR_DMR_MEASUREMENT.MEAS_VIOL_CODE"
'  URL = URL & "&table_1=PCS_CUR_DMR_MEASUREMENT.MODIF_NUM"
'  URL = URL & "&table_1=PCS_CUR_DMR_MEASUREMENT.MONITORING_LOC"
'  URL = URL & "&table_1=PCS_CUR_DMR_MEASUREMENT.MONITORING_PERIOD_END_DATE"
'  URL = URL & "&table_1=PCS_CUR_DMR_MEASUREMENT.NO_DISCHARGE_IND"
'  URL = URL & "&table_1=PCS_CUR_DMR_MEASUREMENT.PARAM_CODE"
'  URL = URL & "&table_1=PCS_CUR_DMR_MEASUREMENT.PIPE_SET_QUALIFIER"
'  URL = URL & "&table_1=PCS_CUR_DMR_MEASUREMENT.QNCR_MV_DET_CODE"
'  URL = URL & "&table_1=PCS_CUR_DMR_MEASUREMENT.QNCR_MV_DET_DATE"
'  URL = URL & "&table_1=PCS_CUR_DMR_MEASUREMENT.QNCR_MV_RESOL_CODE"
'  URL = URL & "&table_1=PCS_CUR_DMR_MEASUREMENT.QNCR_MV_RESOL_DATE"
'  URL = URL & "&table_1=PCS_CUR_DMR_MEASUREMENT.QTY_AVG"
'  URL = URL & "&table_1=PCS_CUR_DMR_MEASUREMENT.QTY_MAX"
'  URL = URL & "&table_1=PCS_CUR_DMR_MEASUREMENT.QUANTITY_UNIT_CODE"
'  URL = URL & "&table_1=PCS_CUR_DMR_MEASUREMENT.REPORT_DESIG"
'  URL = URL & "&table_1=PCS_CUR_DMR_MEASUREMENT.SEASON_NUM"
'  URL = URL & "&table1_type= "
'  URL = URL & StrRepeat(22, "&table1_type=Equal to")
'  URL = URL & StrRepeat(23, "&table1_value=")
'  URL = URL & StrRepeat(23, "&column_number=")
'  URL = URL & StrRepeat(23, "&sort_selection=")
'  URL = URL & StrRepeat(23, "&sort_order=Ascending")
'  URL = URL & "&group_sequence=test"
'  URL = URL & "&csv_output=Output to CSV File"
'  BuildQueryURL_PCS_CUR_DMR_MEASUREMENT = ReplaceString(URL, " ", "%20")
'End Function

Private Function BuildQueryURL_PCS_DMR_MEASUREMENT(npdes As String) As String
  Dim URL As String
  URL = pBaseURL & "database_type=PCS"
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
  URL = URL & "&table_1=PCS_DMR_MEASUREMENT.NPDES"
  URL = URL & "&table_1=PCS_DMR_MEASUREMENT.CONCENTRATION_UNIT_CODE"
  URL = URL & "&table_1=PCS_DMR_MEASUREMENT.CONCENTR_AVG"
  URL = URL & "&table_1=PCS_DMR_MEASUREMENT.CONCENTR_MAX"
  URL = URL & "&table_1=PCS_DMR_MEASUREMENT.CONCENTR_MIN"
  URL = URL & "&table_1=PCS_DMR_MEASUREMENT.DISCHARGE_NUM"
  URL = URL & "&table_1=PCS_DMR_MEASUREMENT.LIMIT_TYPE"
  URL = URL & "&table_1=PCS_DMR_MEASUREMENT.MEAS_VIOL_CODE"
  URL = URL & "&table_1=PCS_DMR_MEASUREMENT.MODIF_NUM"
  URL = URL & "&table_1=PCS_DMR_MEASUREMENT.MONITORING_PERIOD_END_DATE"
  URL = URL & "&table_1=PCS_DMR_MEASUREMENT.NO_DISCHARGE_IND"
  URL = URL & "&table_1=PCS_DMR_MEASUREMENT.PARAM_CODE"
  URL = URL & "&table_1=PCS_DMR_MEASUREMENT.PIPE_SET_QUALIFIER"
  URL = URL & "&table_1=PCS_DMR_MEASUREMENT.QNCR_MV_DET_CODE"
  URL = URL & "&table_1=PCS_DMR_MEASUREMENT.QNCR_MV_DET_DATE"
  URL = URL & "&table_1=PCS_DMR_MEASUREMENT.QNCR_MV_RESOL_CODE"
  URL = URL & "&table_1=PCS_DMR_MEASUREMENT.QNCR_MV_RESOL_DATE"
  URL = URL & "&table_1=PCS_DMR_MEASUREMENT.QTY_AVG"
  URL = URL & "&table_1=PCS_DMR_MEASUREMENT.QTY_MAX"
  URL = URL & "&table_1=PCS_DMR_MEASUREMENT.QUANTITY_UNIT_CODE"
  URL = URL & "&table_1=PCS_DMR_MEASUREMENT.REPORT_DESIG"
  URL = URL & "&table_1=PCS_DMR_MEASUREMENT.SEASON_NUM"
  URL = URL & "&table1_type= "
  URL = URL & StrRepeat(21, "&table1_type=Equal to")
  URL = URL & StrRepeat(22, "&table1_value=")
  URL = URL & StrRepeat(22, "&column_number=")
  URL = URL & StrRepeat(22, "&sort_selection=")
  URL = URL & StrRepeat(22, "&sort_order=Ascending")
  URL = URL & "&group_sequence=test"
  URL = URL & "&csv_output=Output to CSV File"
  BuildQueryURL_PCS_DMR_MEASUREMENT = ReplaceString(URL, " ", "%20")
End Function

Private Function clsWebData_GetData(ByVal aQuery As ChilkatXml, aResult As ChilkatXml) As Boolean
  Dim filename As String
  Dim dbfFilename As String
  Dim vnpdesNode As Variant
  Dim npdesNode As ChilkatXml
  Dim npdes As String
  Dim huc_cd As String
  Dim huc_index As Long
  Dim cache_dir As String
  Dim project_dir As String
  Dim outputFormat As String
  Dim FirstFile As Boolean
  Dim ErrMsg As String
  Dim DestinationDBFs As New FastCollection
  Dim FilesNotCreated As String
  Dim nFilesNotCreated As Long
  Dim iFile As Long
  Dim findPos As Long
  Dim Msg As String
  
  Dim nodStatus As ChilkatXml
  
  Dim lstSites As FastCollection
  
  Dim URL As String
  Dim lCsv As clsCSV
  Dim lDbf As clsDBF
  Dim downloadindex As Long
  
  pManager.LogDbg "  clsWebDataPCSdischarge GetData entry"

  cache_dir = pManager.CurrentStatusGetString("cache_dir") & pClassName & "\"
  project_dir = pManager.CurrentStatusGetString("project_dir")

  pBaseURL = pManager.CurrentStatusGetString("PCSDischargeBaseURL", _
             "http://oaspub.epa.gov/enviro/ad_hoc_build_sql1.get_table?")
  
  clsWebData_GetData = True
  
  aResult.Tag = pClassName
  Set nodStatus = aResult.newChild("status", "")
  nodStatus.AddAttribute "complete", "false"
  clsWebData_GetData = False
  
  FirstFile = True
  Set lstSites = GetChildrenWithTag(aQuery.GetChildWithTag("criteria"), "npdes")
  For Each vnpdesNode In lstSites
    Set npdesNode = vnpdesNode
    npdes = npdesNode.Content
    'Get PCS_CUR_DMR_MEASUREMENT table
    'downloadindex = downloadindex + 1
    'filename = cache_dir & npdes & ".PCS_CUR_DMR_MEASUREMENT.CSV"
    'ErrMsg = PermitDownload(BuildQueryURL_PCS_CUR_DMR_MEASUREMENT(npdes), filename, FirstFile)
    'GoSub MergeFilenameWithDBF
    
    'FirstFile = False

    'Get PCS_DMR_MEASUREMENT table
    downloadindex = downloadindex + 1
    filename = cache_dir & npdes & ".PCS_DMR_MEASUREMENT.CSV"
    ErrMsg = PermitDownload(BuildQueryURL_PCS_DMR_MEASUREMENT(npdes), filename, FirstFile, "(" & downloadindex & " of " & lstSites.Count & ")")
    GoSub MergeFilenameWithDBF
    
    FirstFile = False
  
  Next
  
  nodStatus.AddAttribute "complete", "true"
  
EndOfFunction:
  If nFilesNotCreated > 0 Then
    filename = project_dir & "pcs\PCS_DischargeNoData" & Format(Date, "yyyy-mm-dd") & Format(Time, "atHH-MM") & ".txt"
'    iFile = 1
'    While FileExists(filename)
'      iFile = iFile + 1
'      filename = project_dir & "pcs\" & "PCS_DischargeNoData(" & iFile & ").txt"
'    Wend
'    If nFilesNotCreated <= 10 Then
'      Msg = FilesNotCreated
'    Else
'      findPos = 1
'      For iFile = 1 To 10
'        findPos = InStr(findPos + 1, FilesNotCreated, vbCr)
'      Next
'      Msg = Left(FilesNotCreated, findPos) & " (and " & (nFilesNotCreated - 10) & " more)"
'    End If
    AppendFileString filename, FilesNotCreated
    pManager.LogDbg "  " & pLabel & " did not download " & nFilesNotCreated & " stations: " & vbCr & Msg & vbCr _
             & "because there were no matching records in the database." & vbCr _
             & "List saved to " & filename
  End If
  
  If DestinationDBFs.Count = 0 Then
    pManager.LogDbg "  " & pLabel & " No destination DBFs to write"
  Else
    For iFile = 1 To DestinationDBFs.Count
      Set lDbf = DestinationDBFs.ItemByIndex(iFile)
      If DestinationDBFs.Count > 1 Then
        pManager.LogDbg "  " & pLabel & " Writing " & lDbf.filename & " (" & iFile & " of " & DestinationDBFs.Count & ")"
      Else
        pManager.LogDbg "  " & pLabel & " Writing " & lDbf.filename
      End If
      lDbf.WriteDBF lDbf.filename
      Set lDbf = Nothing
    Next
  End If
  pManager.LogDbg "  " & pLabel & " GetData exit"

  Exit Function

MergeFilenameWithDBF: '(local subroutine)
  pManager.LogDbg pLabel & ":GetData:" & filename & ":" & ErrMsg
  Select Case ErrMsg
    Case "cancel"
                  GoTo EndOfFunction
    Case "success"
                If pEnableImportingToDBF Then
                  huc_cd = npdesNode.GetAttrValue("huc_cd")
                  huc_index = DestinationDBFs.IndexFromKey(huc_cd)
                  If huc_index > 0 Then
                    Set lDbf = DestinationDBFs.ItemByIndex(huc_index)
                  Else
                    dbfFilename = project_dir & "pcs\" & huc_cd & ".dbf"
                    If FileExists(dbfFilename) Then
                      Set lDbf = New clsDBF
                      lDbf.OpenDBF dbfFilename
                    Else
                      pManager.LogDbg "PCS data not found in this project and HUC, Creating " & dbfFilename
                      MkDirPath PathNameOnly(dbfFilename)
                      Set lDbf = NewPCSdbf
                      lDbf.WriteDBF dbfFilename 'Write empty DBF now to store Filename
                    End If
                    DestinationDBFs.Add lDbf, huc_cd
                  End If
                  pManager.DownloadStatus = pLabel & " Importing '" & filename & "' into '" & lDbf.filename & "'" & " (PCS site " & downloadindex & " of " & lstSites.Count & ")"
                  
                  Set lCsv = New clsCSV
                  lCsv.OpenCSV filename
                  ImportDMR2DBF lCsv, lDbf ', pManager.Logger
                  Set lCsv = Nothing
                Else
                  dbfFilename = project_dir & "pcs\" & FilenameNoPath(filename)
                  If FileExists(dbfFilename) Then
                    pManager.LogDbg pLabel & ":Overwriting:" & dbfFilename
                    Kill dbfFilename
                  Else
                    pManager.LogDbg pLabel & ":Copying downloaded data to:" & dbfFilename
                    MkDirPath PathNameOnly(dbfFilename)
                  End If
                  FileCopy filename, dbfFilename
                End If
    Case pNoRecordsMessage
                  FilesNotCreated = FilesNotCreated & "   " & FilenameNoPath(filename) & vbCrLf
                  nFilesNotCreated = nFilesNotCreated + 1
  End Select
  DoEvents
  Return

End Function

Private Function PermitDownload(ByVal URL As String, filename As String, FirstFile As Boolean, progress As String) As String
  Dim retval As String
  Dim URLedge As Integer
  
  retval = "success"
  If FileExists(filename) Then
    retval = WholeFileString(filename)
    If Left(retval, 5) <> "Error" Then
      retval = "success"
      pManager.LogDbg "Already have file, skipping download of " & filename
    ElseIf InStr(retval, pNoRecordsMessage) > 0 Then
      pManager.LogDbg "Previously found no records " & Mid(retval, 19) _
                 & ", skipping download of " & filename
      retval = pNoRecordsMessage
    Else
      pManager.LogDbg "Previous " & retval & vbCr & "Trying again to download " & filename
      retval = "success"
      GoTo TryDownload
    End If
  Else
TryDownload:
    'Get page telling us CSV file name
    If Not pManager.Download(URL, "", FirstFile, "Waiting for " & pLabel & " " & progress & " " & filename) Then
      retval = "cancel"
    ElseIf pManager.ResultString <> "<undefined/>" Then
      URL = pManager.ResultString
      
      'Find URL of CSV file
      URLedge = InStr(URL, pCSVprefix)
      If URLedge = 0 Then
        If InStr(URL, pNoRecordsString) > 0 Then
          retval = pNoRecordsMessage
        Else
          retval = "could not find CSV URL in the output:" & vbCr & vbCr & URL
        End If
      Else
        URL = Trim(Mid(URL, URLedge + Len(pCSVprefix)))
        URLedge = InStr(URL, ">")
        If URLedge = 0 Then
          retval = "could not find CSV URL in the output:" & vbCr & vbCr & URL
        Else
          URL = Left(URL, URLedge - 1)
          If Not pManager.Download(URL, filename, False, "Downloading " & pLabel & " " & progress & " " & filename) Then
            retval = "could not download data" & vbCr & vbCr & URL
          End If
        End If
      End If
    End If
  End If
  If retval <> "success" Then 'leave blank file so we don't look for it again
    SaveFileString filename, "Error: " & retval & vbCr & Now
  End If
  PermitDownload = retval
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

Private Sub clsWebData_Specify(Optional parent As Object = Nothing, Optional ByVal index As Long = 0)
  pManager.ShowCriteria Me, parent, index
End Sub

Private Property Get clsWebData_Version() As String
  clsWebData_Version = App.Major & "." & App.Minor & "." & App.Revision
End Property

