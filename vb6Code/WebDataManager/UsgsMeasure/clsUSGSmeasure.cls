VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsUSGSmeasure"
Attribute VB_GlobalNameSpace = True
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
Implements clsWebData

Private Const pLabel = "USGS Streamflow Measurements"
Private Const pClassName = "clsUSGSMeasure"
Private Const pHelpID = 32
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
  Set nod2 = nod1.newChild("measurement_site_no", "")
      nod2.AddAttribute "multiple", "true"
      nod2.AddAttribute "label", "Measurement Site Number"
        
'    Set nod2 = pProvides.createElement("begin_date")
'                                                 nod2.AddAttribute "type", "Date"
'                                                 nod2.AddAttribute "format", "yyyy-mm-dd"
'                                                 nod2.AddAttribute "optional", "true"
'                                                 nod2.AddAttribute "label", "Begin Date"
'    nod1.appendChild nod2
'    Set nod2 = pProvides.createElement("end_date")
'                                                 nod2.AddAttribute "type", "Date"
'                                                 nod2.AddAttribute "format", "yyyy-mm-dd"
'                                                 nod2.AddAttribute "optional", "true"
'                                                 nod2.AddAttribute "label", "End Date"
'    nod1.appendChild nod2
'    Set nod2 = pProvides.createElement("cache_dir")
'                                                 nod2.AddAttribute "format", "savepath"
'                                                 nod2.AddAttribute "label", "Save in Directory"
'    nod1.appendChild nod2
    
'    Set nod2 = pProvides.createElement("SHPfile")
'                                                 nod2.AddAttribute "format", "savefile"
'                                                 nod2.AddAttribute "optional", "true"
'                                                 nod2.AddAttribute "label", "Create point shape file"
'                                                 nod2.AddAttribute "filter", "Shape File|*.shp"
'    nod1.appendChild nod2

    Set nod2 = nod1.newChild("WDMfile", "")
        nod2.AddAttribute "format", "openfile"
        nod2.AddAttribute "optional", "true"
        nod2.AddAttribute "label", "Add to WDM file"
        nod2.AddAttribute "filter", "WDM File|*.wdm"
      
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
  clsWebData_Name = "clsUSGSMeasure"
End Property

Private Property Get clsWebData_Provides() As ChilkatXml
  Set clsWebData_Provides = pProvides
End Property

Private Function clsWebData_GetData(ByVal aQuery As ChilkatXml, ByRef aResult As ChilkatXml) As Boolean
  Dim lQueryChild As ChilkatXml
  Dim lCritChild As ChilkatXml
 'Dim state_abbrev As String
  Dim begin_date As String
  Dim end_date As String
  Dim cache_dir As String
  Dim save_filename As String
 'Dim SHPfilename As String
  Dim WDMfilename As String
  Dim myDownloadFiles As Collection 'of file names
  Dim url As String
  Dim iChild As Long, jChild As Long, iSite As Long
  Dim lstSites As FastCollection
  Dim vSite As Variant
  Dim lSite As ChilkatXml
  Dim vAttr As Variant
  Dim iAttr As Long
  Dim siteAttributes As String
  Dim FirstFile As Boolean
  Dim FilesNotCreated As String
  Dim nFilesNotCreated As Long
  Dim FileNumber As Long
  
  Dim nodStatus As ChilkatXml
  Dim findPos As Long
  Dim Msg As String
  Dim basins_dir As String
  Dim suffix As String
  Dim base_url As String
  Dim project_dir As String
  Dim date_format As String
  
  On Error GoTo ErrHand
  
  pManager.LogDbg "  clsUsgsMeasure GetData entry"
  
  basins_dir = pManager.CurrentStatusGetString("basins_dir", CurDir)
  cache_dir = pManager.CurrentStatusGetString("cache_dir") & pClassName & "\"
  project_dir = pManager.CurrentStatusGetString("project_dir")
'  SHPfilename = project_dir & pManager.CurrentStatusGetString("USGSmeasureSHPfile", "gage.shp")
  suffix = pManager.CurrentStatusGetString("USGSmeasureSaveSuffix", "_sm.txt")
  base_url = pManager.CurrentStatusGetString("USGSMeasureBaseURL", "http://nwis.waterdata.usgs.gov/usa/")
  date_format = pManager.CurrentStatusGetString("USGSMeasureDateFormat", "MM/DD/YYYY")
    
  clsWebData_GetData = True
  
  If Len(aQuery.ErrorLogText) > 0 Then
    pErrorDescription = "Parse Error:" & aQuery.ErrorLogText
    clsWebData_GetData = False
  Else
'    ReDim site_no(0)
    Set lQueryChild = aQuery.FirstChild
    While Not lQueryChild Is Nothing
      Select Case LCase(lQueryChild.Tag)
        Case "criteria"
          Set lCritChild = lQueryChild.FirstChild
          While Not lCritChild Is Nothing
            Select Case LCase(lCritChild.Tag)
              Case "begin_date":     begin_date = lCritChild.Content
              Case "end_date":         end_date = lCritChild.Content
'             Case "state_abbrev": state_abbrev = lCritChild.Content
              Case "cache_dir":       cache_dir = lCritChild.Content & pClassName & "\"
'             Case "shpfile":       SHPfilename = lCritChild.Content
              Case "wdmfile":       WDMfilename = lCritChild.Content
            End Select
            If lCritChild.NextSibling2 = 0 Then Set lCritChild = Nothing
          Wend
      End Select
      If lQueryChild.NextSibling2 = 0 Then Set lQueryChild = Nothing
    Wend
    
    If Len(WDMfilename) > 0 Then
      Set myDownloadFiles = New Collection
    Else 'Save downloaded RDB files in folder inside project_dir if we are not adding to WDM
      project_dir = project_dir & "USGSmeasure\"
      pManager.LogDbg "Saving RDB files in " & project_dir
    End If
    MkDirPath cache_dir
    MkDirPath project_dir
    
    aResult.Tag = pClassName
    Set nodStatus = aResult.newChild("status", "")
    nodStatus.AddAttribute "complete", "false"
    
    FirstFile = True
    iSite = 0
    Set lstSites = GetChildrenWithTag(aQuery.GetChildWithTag("criteria"), "measurement_site_no")
    If lstSites.Count = 0 Then
      Set lstSites = GetChildrenWithTag(aQuery.GetChildWithTag("criteria"), "site_no")
    End If
    For Each vSite In lstSites 'For iSite = 1 To nSites
      iSite = iSite + 1
      Set lSite = vSite
      url = base_url
     'If Len(state_abbrev) > 0 Then url = url & LCase(state_abbrev) & "/"
      url = url & "nwis/measurements?site_no=" & lSite.Content & _
               "&agency_cd=USGS" & _
               "&format=rdb"

      siteAttributes = "# " & url & vbCrLf
      For iAttr = 0 To lSite.NumAttributes - 1
        siteAttributes = siteAttributes & "# " & lSite.GetAttributeName(iAttr) _
                                & Space(48 - Len(lSite.GetAttributeName(iAttr))) _
                                               & lSite.GetAttributeValue(iAttr) & vbCrLf
      Next
     'save_filename = cache_dir & state_abbrev & lSite.Content & suffix
      save_filename = cache_dir & lSite.Content & suffix
      
      If Not pManager.Download(url, save_filename, FirstFile, "Downloading " & pLabel & " (" & iSite & " of " & lstSites.Count & ")", siteAttributes) Then
        nodStatus.AddAttribute "message", "User Cancelled"

        Exit Function '!!!!!!!!!!!!!!!!!!!
      
      End If
      Msg = WholeFileString(save_filename)
      findPos = InStr(Msg, "<HTML>")
      If findPos > 0 Then 'Got an error message or web page, not the data we expected
        Msg = Mid(pManager.ResultString, findPos)
        Kill save_filename
        nFilesNotCreated = nFilesNotCreated + 1
        FilesNotCreated = FilesNotCreated & "   " & FilenameNoPath(save_filename)
        If InStr(Msg, "No data were found") > 0 Then
          FilesNotCreated = FilesNotCreated & " (no data)"
        Else
          findPos = InStr(Msg, "3>")
          If findPos > 0 Then
            Msg = Mid(Msg, findPos + 2)
            findPos = InStr(Msg, "</")
            If findPos > 0 Then Msg = Left(Msg, findPos - 1)
          End If
          If Len(Msg) < 80 Then
            FilesNotCreated = FilesNotCreated & " (" & Msg & ")"
          Else
            Msg = Mid(pManager.ResultString, InStr(pManager.ResultString, "<HTML>"))
            Msg = ReplaceString(Msg, "<HEAD>", "<HEAD><base href=""" & base_url & "nwis")
            pManager.ShowHTML Msg
          End If
        End If
        FilesNotCreated = FilesNotCreated & vbCrLf
      ElseIf Len(WDMfilename) > 0 Then
        myDownloadFiles.Add save_filename
      Else
        pManager.LogDbg "Copying downloaded file to " & project_dir & FilenameNoPath(save_filename)
        FileCopy save_filename, project_dir & FilenameNoPath(save_filename)
      End If
      FirstFile = False
    Next
    If Len(WDMfilename) > 0 Then
      pManager.LogDbg "Saving downloaded data to " & WDMfilename
      On Error GoTo ErrSaveWDM
      ConvertUsgsSm2Wdm WDMfilename, myDownloadFiles
      'SaveWDMandSHP WDMfilename, SHPfilename, basins_dir, myDownloadFiles
      On Error GoTo ErrHand
    End If
'    If Len(SHPfilename) > 0 Then
'      pManager.LogDbg "Saving any new stations found to '" & SHPfilename & "'"
'    End If
    If nFilesNotCreated > 0 Then
      save_filename = project_dir & "USGSmeasureNoData.txt"
      While Len(Dir(save_filename)) > 0
        FileNumber = FileNumber + 1
        save_filename = project_dir & "USGSmeasureNoData(" & FileNumber & ").txt"
      Wend
      If nFilesNotCreated > 10 Then
        findPos = 1
        For FileNumber = 1 To 10
          findPos = InStr(findPos + 1, FilesNotCreated, vbCr)
        Next
        Msg = Left(FilesNotCreated, findPos) & " (and " & (nFilesNotCreated - 10) & " more)"
      Else
        Msg = FilesNotCreated
      End If
      If pManager.LogMsg("Did not find data for " & nFilesNotCreated & " of " & lstSites.Count & " stations: " & vbCr & vbCr _
               & Msg & vbCr _
               & "Save this list to " & save_filename & "?", _
               pLabel & " - Some data not found", "+&Yes", "-&No") = 1 Then
        SaveFileString save_filename, FilesNotCreated
      End If
    End If
    
    nodStatus.AddAttribute "complete", "true"
  End If
  pManager.LogDbg "  clsUsgsMeasure GetData exit"
  Exit Function

ErrHand:
  pManager.LogMsg "Error '" & Err.Description & "'", pLabel & " GetData"
  Exit Function
ErrSaveWDM:
  pManager.LogMsg "Error writing WDM file" & vbCr & Err.Description & vbCr & "Libraries may need to be installed for saving WDM files", _
                         pLabel & " GetData"
End Function

'Private Sub SaveWDMandSHP(WDMfilename As String, SHPfilename As String, basins_dir As String, myDownloadFiles As Collection)
'  Dim Msg As String
'  Dim tsfile As ATCclsTserFile
'  Set tsfile = ConvertUsgsSm2Wdm(WDMfilename, myDownloadFiles) ', Attribs)
'  If Len(SHPfilename) > 0 Then
'    pManager.LogDbg pLabel & " WriteShapeFile " & SHPfilename
'    WriteShapeFile tsfile, SHPfilename, "LNGDEG", "LATDEG", PopulatedAttributes(tsfile)
'    pManager.AppendProjectXML "<convert_shape>" & SHPfilename & "</convert_shape>" & vbCrLf & _
'                              "  <add_shape>" & SHPfilename & "</add_shape>"
'  End If
'End Sub

Private Function PopulatedAttributes(tsfile As ATCclsTserFile) As String()
  Dim AllAttributes As Collection
  Dim Attrib As String
  Dim retval() As String
  Dim nVal As Long
  Dim vAttrib As Variant
    
  Set AllAttributes = tsfile.AvailableAttributes
  ReDim retval(AllAttributes.Count)
  
  If tsfile.DataCount > 0 Then
    For Each vAttrib In AllAttributes
      Attrib = vAttrib.Name
      If Len(tsfile.Data(1).Attrib(Attrib)) > 0 Then
        nVal = nVal + 1
        retval(nVal) = Attrib
      End If
    Next
  End If
  ReDim Preserve retval(nVal)
  PopulatedAttributes = retval
End Function


Private Property Set clsWebData_Manager(myManager As clsWebDataManager)
  Set pManager = Nothing
  Set pManager = myManager
End Property

Private Property Get clsWebData_Manager() As clsWebDataManager
  Set clsWebData_Manager = pManager
End Property

Private Sub clsWebData_Specify(Optional parent As Object = Nothing, Optional ByVal Index As Long = 0)
'  Dim cache_dir As String
  pManager.CurrentStatusUpdateString "USGSSitemeasurement", "measurement", "set by " & pLabel
'  cache_dir = pManager.CurrentStatusGetString("cache_dir")
'  If Len(cache_dir) > 0 Then
'    If Len(pManager.CurrentStatusGetString("SHPfilename")) = 0 Then
'      pManager.CurrentStatusUpdateString "SHPfilename", cache_dir & "untitled.shp"
'    End If
'  End If
  pManager.ShowCriteria Me, parent, Index
End Sub

Private Property Get clsWebData_Version() As String
  clsWebData_Version = App.Major & "." & App.Minor & "." & App.Revision
End Property
'&sort_key=site_no
'&format=sitefile_output
'&sitefile_output_format=rdb
'&column_name=agency_cd
'&column_name=site_no
'&column_name=station_nm
'&column_name=lat_va
'&column_name=long_va
'&column_name=dec_lat_va
'&column_name=dec_long_va
'&column_name=coord_meth_cd
'&column_name=coord_acy_cd
'&column_name=coord_datum_cd
'&column_name=district_cd
'&column_name=state_cd
'&column_name=county_cd
'&column_name=country_cd
'&column_name=land_net_ds
'&column_name=map_nm
'&column_name=map_scale_fc
'&column_name=alt_va
'&column_name=alt_meth_cd
'&column_name=alt_acy_va
'&column_name=alt_datum_cd
'&column_name=huc_cd
'&column_name=basin_cd
'&column_name=topo_cd
'&column_name=station_type_cd
'&column_name=agency_use_cd
'&column_name=data_types_cd
'&column_name=instruments_cd
'&column_name=construction_dt
'&column_name=inventory_dt
'&column_name=drain_area_va
'&column_name=contrib_drain_area_va
'&column_name=tz_cd
'&column_name=local_time_fg
'&column_name=reliability_cd
'&column_name=gw_file_cd
'&column_name=gw_type_cd
'&column_name=site_use_1_cd
'&column_name=site_use_2_cd
'&column_name=site_use_3_cd
'&column_name=water_use_1_cd
'&column_name=water_use_2_cd
'&column_name=water_use_3_cd
'&column_name=nat_water_use_cd
'&column_name=aqfr_cd
'&column_name=aqfr_type_cd
'&column_name=well_depth_va
'&column_name=hole_depth_va
'&column_name=depth_src_cd
'&column_name=project_no
'&list_of_search_criteria=state_cd

Private Function ConvertUsgsSm2Wdm(myWDMFileName As String, _
                                myDownloadFiles As Collection) As ATCclsTserFile
  Dim myWDM As ATCclsTserFile
  Dim myWDMExt As clsTSerWDM
  Dim myMem As ATCclsTserFile
  Dim myRegistry As ATCoRegistry
  Dim myScriptLocation As String
  Dim myDownloadFileName As String
  Dim vDownloadFileName As Variant
  Dim p As String
  Dim s As String
  Dim iFile As Integer
  Dim i As Long
    
  Set myWDM = New clsTSerWDM
  Set myWDMExt = myWDM
  Set ConvertUsgsSm2Wdm = myWDM
  
  Set myRegistry = New ATCoRegistry

  Dim AllAttributes As Collection

  myRegistry.AppName = "GenScn"
'  New version of clsTSerWDM no longer requires us to set MsgUnit from here
'  p = GetSetting("HASS_ENT", "files", "HSPFMsgWDM", "c:\vbexpermental\genscn\bin\hspfmsg.wdm")
'  myWDMExt.MsgUnit = F90_WDBOPN(CLng(0), p, Len(p))

  If FileExists(myWDMFileName) Then
    myWDM.Filename = myWDMFileName
  Else
    myWDM.SaveAs myWDMFileName
    If LCase(myWDM.Filename) <> LCase(myWDMFileName) Then
      pManager.LogDbg pLabel & " could not create WDM file " & myWDMFileName
      
      Exit Function '!!!!!!!!!!!!!!!!!!!!
    
    End If
  End If
  
  pManager.LogDbg pLabel & " ConvertUsgsSm2Wdm " & myWDMFileName

  'use a registry key for the WDMUtil bin folder (scripts just below)
  p = myRegistry.RegGetString(HKEY_LOCAL_MACHINE, "SOFTWARE\AQUA TERRA Consultants\WDMUtil\ExePath", "")
  If Len(p) = 0 Then p = "c:\vbexperimental\wdmutil"
  myScriptLocation = p & "\scripts\USGSsmBASINSdownload.ws"
  
  Set AllAttributes = myWDM.AvailableAttributes
  
  For iFile = 1 To myDownloadFiles.Count
    myDownloadFileName = myDownloadFiles.Item(iFile)
    'Set myMsg.Icon = frmShapeAttributes.Icon
    pManager.DownloadStatus = "Reading downloaded file: " & myDownloadFileName _
                   & " (" & iFile & " of " & myDownloadFiles.Count & ") " & vbCr _
                   & "into " & myWDMFileName _
                   & ". This may take some time"
    If pManager.State >= 999 Then Exit Function
    Set myMem = Nothing
    Set myMem = New clsTSerMemory
    myMem.Filename = myDownloadFileName & "|" & myScriptLocation
    If myMem.DataCount > 0 Then
      For i = 1 To myMem.DataCount
        myWDM.AddTimSer myMem.Data(i), TsIdRenum
        With myWDM.Data(i)
          'Accessing .Max forces reading all data in dataset
          pManager.LogDbg myDownloadFileName & " added to WDM. DataCount=" & myWDM.DataCount & ", ID=" & .Header.Id & ", Max=" & .Max
        End With
      Next i
    End If
  Next
  Set myMem = Nothing
End Function

