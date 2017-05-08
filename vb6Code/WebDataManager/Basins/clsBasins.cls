VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsBasins"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
Implements clsWebData

Private Const pLabel = "BASINS 303d, Census, DEM, DEMG, NED, NHD, GIRAS landuse, Legacy STORET"
Private Const pShortLabel = "BASINS Data"
Private Const pTypePrefix = "BASINS "
Private Const pClassName = "clsBasins"
Private Const pHelpID = 24
Private pProvides As ChilkatXml
Private pManager As clsWebDataManager
Private pCriteria As ChilkatXml

Private pErrorDescription As String

Private Sub SetProvidesCriteria()
  Dim nod1 As ChilkatXml
  Dim nod2 As ChilkatXml
  
  Set pProvides = New ChilkatXml
  pProvides.Tag = pClassName
  Set nod1 = pProvides.newChild("output", "")
  Set nod2 = nod1.newChild("file", "")
      nod2.AddAttribute "format", "exe"
  
  Set pCriteria = pProvides.newChild("criteria", "")
  Set nod2 = pCriteria.newChild("huc_cd", "")
      nod2.AddAttribute "label", "8-digit HUC"
      nod2.AddAttribute "multiple", "true"
    
  'Don't offer to download core data unless this is a BasinsInitialSetup
  If Not pManager Is Nothing Then
    If LCase(pManager.CurrentStatusGetString("download_type")) <> "basinsinitialsetup" Then GoTo SkipCore
  End If
    
  Set nod2 = pCriteria.newChild("want_core_basins", "")
      nod2.AddAttribute "format", "boolean"
      nod2.AddAttribute "label", "Core BASINS data"
      nod2.AddAttribute "optional", "true"
    
SkipCore:
    
  Set nod2 = pCriteria.newChild("want_303d_basins", "")
      nod2.AddAttribute "format", "boolean"
      nod2.AddAttribute "label", pTypePrefix & "303d"
      nod2.AddAttribute "optional", "true"
    
  Set nod2 = pCriteria.newChild("want_census_basins", "")
      nod2.AddAttribute "format", "boolean"
      nod2.AddAttribute "label", pTypePrefix & "Census"
      nod2.AddAttribute "optional", "true"
    
  Set nod2 = pCriteria.newChild("want_dem_basins", "")
      nod2.AddAttribute "format", "boolean"
      nod2.AddAttribute "label", pTypePrefix & "Digital Elevation Model (DEM)"
      nod2.AddAttribute "optional", "true"
    
  Set nod2 = pCriteria.newChild("want_demg_basins", "")
      nod2.AddAttribute "format", "boolean"
      nod2.AddAttribute "label", pTypePrefix & "DEM Grid (DEMG)"
      nod2.AddAttribute "optional", "true"
    
  Set nod2 = pCriteria.newChild("want_ned_basins", "")
      nod2.AddAttribute "format", "boolean"
      nod2.AddAttribute "label", pTypePrefix & "National Elevation Dataset (NED)"
      nod2.AddAttribute "optional", "true"

  Set nod2 = pCriteria.newChild("want_nhd_basins", "")
      nod2.AddAttribute "format", "boolean"
      nod2.AddAttribute "label", pTypePrefix & "National Hydrography Dataset (NHD)"
      nod2.AddAttribute "optional", "true"
    
  Set nod2 = pCriteria.newChild("want_giras_basins", "")
      nod2.AddAttribute "format", "boolean"
      nod2.AddAttribute "label", pTypePrefix & "GIRAS landuse"
      nod2.AddAttribute "optional", "true"
  
  Set nod2 = pCriteria.newChild("want_lstoret_basins", "")
      nod2.AddAttribute "format", "boolean"
      nod2.AddAttribute "label", pTypePrefix & "Legacy STORET"
      nod2.AddAttribute "optional", "true"
    
'  Set nod2 = pProvides.createElement("cache_dir")
'      nod2.AddAttribute "format", "savepath"
'      nod2.AddAttribute "label", "Save in Directory"

  Set nod1 = Nothing
  Set nod2 = Nothing
End Sub

Private Function clsWebData_ErrorDescription() As String
  clsWebData_ErrorDescription = pErrorDescription
  pErrorDescription = ""
End Function

Private Function clsWebData_GetData(ByVal aQuery As ChilkatXml, aResult As ChilkatXml) As Boolean
  Dim lQueryChild As ChilkatXml
  Dim lCritChild As ChilkatXml
  Dim base_url As String
  Dim url As String
  Dim huc As String
  Dim cache_dir As String 'where we save the downloaded file
  Dim dest_dir As String 'where we put the unpacked data
  Dim downloaded_file As String 'Full path of latest file downloaded
  Dim FirstFile As Boolean
  
  Dim want_core As Boolean
  Dim want_303d As Boolean
  Dim want_census As Boolean
  Dim want_dem As Boolean
  Dim want_demg As Boolean
  Dim want_ned As Boolean
  Dim want_nhd As Boolean
  Dim want_giras As Boolean
  Dim want_lstoret As Boolean
  
  Dim core_suffix As String
  Dim dem_suffix As String
  Dim a303d_suffix As String
  Dim census_suffix As String
  Dim demg_suffix As String
  Dim ned_suffix As String
  Dim nhd_suffix As String
  Dim giras_suffix As String
  Dim lstoret_suffix As String
  Dim cur_suffix As String 'assigned to one of the above suffixes at a time
  
  Dim project_dir As String
  Dim project_apr As String
  Dim basins_dir As String
  Dim Msg As String
  'Dim BuildEXE As String
  Dim AVExe As String
  
  Dim lstHUCs As FastCollection
  Dim vHUC As Variant
  Dim iHUC As Long 'Index of current HUC for download progress indicator
  Dim nodStatus As ChilkatXml
  Dim nDownloads As Long 'Total number of files to download this time for this data type
  Dim iDownload As Long  'index of current file being downloaded. Max will be nDownloads
  Dim StepName As String
  Dim FileContents As String
  Dim user_response As Long
  Dim launched_by As String

  On Error GoTo ErrHand

  pManager.LogDbg "  clsBasins GetData entry"
  
  basins_dir = pManager.CurrentStatusGetString("basins_dir", CurDir)
  'If UCase(Left(basins_dir, 5)) = UCase("c:\vb") Then basins_dir = "C:\BASINS\" 'exception for running in VB
  project_dir = pManager.CurrentStatusGetString("project_dir")
  cache_dir = pManager.CurrentStatusGetString("cache_dir") & pClassName & "\"
  launched_by = pManager.CurrentStatusGetString("launched_by")

  
  StepName = "aResult.Tag = pClassName"
  aResult.Tag = pClassName
  Set nodStatus = aResult.newChild("status", "")
  nodStatus.AddAttribute "complete", "false"
  clsWebData_GetData = False
  
  StepName = "Parsing Query"
  If Len(aQuery.ErrorLogText) > 0 Then
    pErrorDescription = "Parse Error:" & aQuery.ErrorLogText
  Else
    Set lQueryChild = aQuery.FirstChild
    While Not lQueryChild Is Nothing
      Select Case LCase(lQueryChild.Tag)
        Case "criteria"
          Set lCritChild = lQueryChild.FirstChild
          While Not lCritChild Is Nothing
            Select Case LCase(lCritChild.Tag)
              'Case "cache_dir":
              '  cache_dir = lCritChild.Content & pClassName & "\"
              Case "want_core_basins":
                If LCase(lCritChild.Content) = "true" Then
                  want_core = True: nDownloads = nDownloads + 1
                  MkDirPath project_dir & "landuse"
                  MkDirPath project_dir & "wqobs"
                  MkDirPath project_dir & "pcs"
                End If
              Case "want_303d_basins":
                If LCase(lCritChild.Content) = "true" Then
                  want_303d = True:  nDownloads = nDownloads + 1
                  MkDirPath project_dir & "303d"
                End If
              Case "want_census_basins":
                If LCase(lCritChild.Content) = "true" Then
                  want_census = True:  nDownloads = nDownloads + 1
                  MkDirPath project_dir & "census"
                End If
              Case "want_dem_basins":
                If LCase(lCritChild.Content) = "true" Then
                  want_dem = True:  nDownloads = nDownloads + 1
                  MkDirPath project_dir & "dem"
                End If
              Case "want_demg_basins":
                If LCase(lCritChild.Content) = "true" Then
                  want_demg = True: nDownloads = nDownloads + 1
                  If launched_by <> "ArcView" Then
                    'will be getting gtif grid
                    MkDirPath project_dir & "demg"
                  End If
                End If
              Case "want_ned_basins":
                If LCase(lCritChild.Content) = "true" Then
                  want_ned = True: nDownloads = nDownloads + 1
                  If launched_by <> "ArcView" Then
                    'will be getting gtif grid
                    MkDirPath project_dir & "ned"
                  End If
                End If
              Case "want_nhd_basins":
                If LCase(lCritChild.Content) = "true" Then
                  want_nhd = True: nDownloads = nDownloads + 1
                End If
              Case "want_giras_basins":
                If LCase(lCritChild.Content) = "true" Then
                  want_giras = True:  nDownloads = nDownloads + 1
                  MkDirPath project_dir & "landuse"
                End If
              Case "want_lstoret_basins":
                If LCase(lCritChild.Content) = "true" Then
                  want_lstoret = True:  nDownloads = nDownloads + 1
                End If
            End Select
            If lCritChild.NextSibling2 = 0 Then Set lCritChild = Nothing
          Wend
      End Select
      If lQueryChild.NextSibling2 = 0 Then Set lQueryChild = Nothing
    Wend
  
    StepName = "BASINSbaseURL"
    base_url = pManager.CurrentStatusGetString("BASINSbaseURL", "http://www.epa.gov/waterscience/ftp/basins/gis_data/huc/")
    If Right(base_url, 1) <> "/" Then base_url = base_url & "/"
  
    core_suffix = pManager.CurrentStatusGetString("BASINScoreSuffix", "_core31.exe")
    a303d_suffix = pManager.CurrentStatusGetString("BASINS303dSuffix", "_303d.exe")
    census_suffix = pManager.CurrentStatusGetString("BASINScensusSuffix", "_census.exe")
    dem_suffix = pManager.CurrentStatusGetString("BASINSdemSuffix", "_dem.exe")
    demg_suffix = pManager.CurrentStatusGetString("BASINSdemgSuffix", "_DEMG.exe")
    ned_suffix = pManager.CurrentStatusGetString("BASINSnedSuffix", "_NED.exe")
    If launched_by <> "ArcView" Then
      pManager.LogDbg "  clsBasins GetData launched_by '" & launched_by & "'"
      demg_suffix = "_DEMGgtif.exe"
      ned_suffix = "_NEDgtif.exe"
    End If
    nhd_suffix = pManager.CurrentStatusGetString("BASINSnhdSuffix", "_nhd.exe")
    giras_suffix = pManager.CurrentStatusGetString("BASINSgirasSuffix", "_giras.exe")
    lstoret_suffix = pManager.CurrentStatusGetString("BASINSlstoretSuffix", "_lstoret.exe")
  
    pManager.LogDbg "  clsBasins GetData base_url '" & base_url & "'"
    pManager.LogDbg "  clsBasins GetData cache_dir '" & cache_dir & "'"
      
    FirstFile = True
    StepName = "getElementsByTagName"
    Set lstHUCs = GetChildrenWithTag(aQuery.GetChildWithTag("criteria"), "huc_cd")
    nDownloads = nDownloads * lstHUCs.Count
    For Each vHUC In lstHUCs 'For iHUC = 1 To nSites
      iHUC = iHUC + 1
      huc = vHUC.Content
      If want_core Then
        cur_suffix = core_suffix: GoSub download
        
        StepName = cache_dir & huc & "\pcs\" & huc & ".dbf"
        If FileExists(project_dir & "pcs\" & huc & ".dbf") Then
          pManager.LogDbg "Leaving existing core pcs " & huc & ".dbf"
          If FileExists(StepName) Then Kill StepName
        ElseIf FileExists(StepName) Then
          Name StepName As project_dir & "pcs\" & huc & ".dbf"
        Else
          pManager.LogDbg "Tried to copy core pcs but could not find: '" & StepName & "'"
        End If
        
        StepName = cache_dir & huc & "\wqobs\" & huc & ".dbf"
        If FileExists(project_dir & "wqobs\" & huc & ".dbf") Then
          pManager.LogDbg "Leaving existing core wqobs " & huc & ".dbf"
          If FileExists(StepName) Then Kill StepName
        ElseIf FileExists(StepName) Then
          Name StepName As project_dir & "wqobs\" & huc & ".dbf"
        Else
          pManager.LogDbg "Tried to copy core wqobs but could not find: '" & StepName & "'"
        End If
        
        StepName = "MergeNonShapeDBFs"
        MergeNonShapeDBFs cache_dir & huc, project_dir
        pManager.AppendProjectXML "<convert_dir output=""" & project_dir & """>" & cache_dir & huc & "\</convert_dir>"
      End If
      
      If want_303d Then
        cur_suffix = a303d_suffix:  GoSub download
        pManager.AppendProjectXML "<convert_dir output=""" & project_dir & "303d\"">" & cache_dir & "303d\" & "</convert_dir>" & vbCrLf & _
                                "  <add_allshapes>" & project_dir & "303d\</add_allshapes>"
      End If
      If want_census Then
        cur_suffix = census_suffix:  GoSub download
        pManager.AppendProjectXML "<convert_dir output=""" & project_dir & "census\"">" & cache_dir & "census\" & "</convert_dir>" & vbCrLf & _
                                "  <add_allshapes>" & project_dir & "census\</add_allshapes>"
      End If
      If want_dem Then
        cur_suffix = dem_suffix:  GoSub download
        pManager.AppendProjectXML "<convert_shape output=""" & project_dir & "dem\" & huc & ".shp"">" & cache_dir & "dem\" & huc & ".shp" & "</convert_shape>" & vbCrLf & _
                                "  <add_shape>" & project_dir & "dem\" & huc & ".shp" & "</add_shape>"
      End If
      If want_demg Then
        cur_suffix = demg_suffix: GoSub download
        If launched_by <> "ArcView" Then
          pManager.AppendProjectXML "<convert_grid output=""" & project_dir & "demg\" & huc & "demg.tif"">" & cache_dir & "demg\" & huc & "demg.tif" & "</convert_grid>" & vbCrLf & _
                                    "  <add_grid>" & project_dir & "demg\" & huc & "demg.tif" & "</add_grid>"
        End If
      End If
      If want_ned Then
        cur_suffix = ned_suffix:   GoSub download
        If launched_by <> "ArcView" Then
          pManager.AppendProjectXML "<convert_grid output=""" & project_dir & "ned\" & huc & "ned.tif"">" & cache_dir & "ned\" & huc & "ned.tif" & "</convert_grid>" & vbCrLf & _
                                    "  <add_grid>" & project_dir & "ned\" & huc & "ned.tif" & "</add_grid>"
        End If
      End If
      If want_nhd Then
        cur_suffix = nhd_suffix:   GoSub download
        pManager.AppendProjectXML "<convert_shape output=""" & project_dir & "nhd\" & huc & ".shp"">" & project_dir & "nhd\dd\" & huc & ".shp" & "</convert_shape>" & vbCrLf & _
                                "  <add_shape>" & project_dir & "nhd\" & huc & ".shp" & "</add_shape>"
      End If
      If want_giras Then
        cur_suffix = giras_suffix: GoSub download
        pManager.AppendProjectXML "<convert_dir output=""" & project_dir & "landuse\"">" & cache_dir & huc & "\landuse\" & "</convert_dir>" & vbCrLf & _
                                  "  <add_allshapes>" & project_dir & "landuse\</add_allshapes>"
  
  '      if PCS data is left out of core, need to do this after download
  '      If pManager.State >= 999 Then Exit Function
  '      MergeDBF project_dir & "pcs3_prm.dbf", cache_dir & "pcs3_prm.dbf", "**ALL**"
  '      If pManager.State >= 999 Then Exit Function
  '      MergeDBF project_dir & "pcs\" & huc & ".dbf", cache_dir & "pcs\" & huc & ".dbf", 1, 2, 3
        
        'If we are not projecting the whole dir, request projection of the pcs3
  '      If Not want_core Then
  '        pManager.AppendProjectXML "<convert_shape output=""" & project_dir & "pcs3.shp" & """>" & cache_dir & huc & "_pcs3.shp" & "</convert_shape>"
  '      End If
      End If
      If want_lstoret Then
        Dim newDBF As clsDBF
        cur_suffix = lstoret_suffix: GoSub download
        
        'Both data and shape create a file named <huc>.dbf in the cache.
        'They should not conflict, since the data version is created, then merged (moved or deleted)
        'before the shape one is created and slated to be projected/merged later
        downloaded_file = dest_dir & huc & ".res"
        If FileExists(downloaded_file) Then
          StepName = "Converting downloaded Legacy STORET results to wqobs data from " & huc
          pManager.DownloadStatus = StepName
          Set newDBF = ConvertLSTORETtoDataDBF(downloaded_file, project_dir & "wqobs\" & huc & ".dbf")
          If newDBF Is Nothing Then
            pManager.LogDbg "Failed to create BasObsWQ DBF from " & downloaded_file
          Else
            StepName = "Kill raw LSTORET Data " & downloaded_file
            If FileExists(downloaded_file) Then Kill downloaded_file
            StepName = "MergeDBF into " & project_dir & "wqobs\" & huc & ".dbf"
            MergeDBF project_dir & "wqobs\" & huc & ".dbf", "", newDBF, "**ALL**"
          End If
        Else
          pManager.LogDbg "Skipping unavailable " & downloaded_file
        End If
        
        downloaded_file = dest_dir & huc & ".sta"
        If FileExists(downloaded_file) Then
          pManager.DownloadStatus = "Converting downloaded Legacy STORET stations to wqobs shapes from " & huc
          ConvertLSTORETtoShapeDBF downloaded_file, project_dir & "wqobs.dbf"
          Kill downloaded_file
          pManager.AppendProjectXML "<convert_shape output=""" & project_dir & "wqobs.shp"">" & dest_dir & huc & ".shp" & "</convert_shape>"
        Else
          pManager.LogDbg "Skipping unavailable " & downloaded_file
        End If
        
      End If
    Next
  
    If want_core Then
      If pManager.CurrentStatusGetString("download_type") = "BasinsInitialSetup" Then
        If pManager.State >= 999 Then Exit Function
        CopyFromIfNeeded "sic.dbf", basins_dir & "data\national\", project_dir
        CopyFromIfNeeded "storetag.dbf", basins_dir & "data\national\", project_dir
        CopyFromIfNeeded "wqcriter.dbf", basins_dir & "data\national\", project_dir
        CopyFromIfNeeded "wqobs_prm.dbf", basins_dir & "data\national\", project_dir
        'build new apr
  '      If pManager.CurrentStatusGetString("launched_by") = "ArcView" Then
  '        project_apr = pManager.CurrentStatusGetString("project_apr")
  '        StepName = "build apr " & project_apr
  '        pManager.LogDbg StepName
  '        BuildEXE = basins_dir & "etc\BasinsArchive\BasinsArchive.exe"
  '        If FileExists(BuildEXE) Then
  '          pManager.StartProcess "BASINSArchive", BuildEXE & " /build, " & project_dir & ", " & project_apr
  '        End If
  '      End If
      End If
    End If
  
    Msg = ""
    If (want_demg Or want_ned) And launched_by = "ArcView" Then
      If want_demg Then Msg = "Downloaded DEM grid data to " & project_dir & "grids\dem\dd\" & vbCrLf
      If want_ned Then Msg = Msg & "Downloaded NED data to " & project_dir & "grids\ned\dd\" & vbCrLf _
                                 & "NED elevation is in centimeters." & vbCrLf _
                                 & "If using automatic delineation, set Z units to centimeters." & vbCrLf
      Msg = pShortLabel & " Instructions: " & vbCrLf & Msg & vbCrLf _
          & "Use of grid data in BASINS requires Spatial Analyst." & vbCrLf _
          & "If your existing data is projected, you will need to use " & vbCrLf _
          & "the Grid Projector extension to project this new grid data." & vbCrLf _
          & "Use the Add Theme (Grid) extension to add this grid to the view." & vbCrLf _
           & vbCrLf _
          & pManager.CurrentStatusGetString("endinstructions")
      pManager.CurrentStatusUpdateString "endinstructions", Msg
    End If
    If pManager.State < 999 Then
      nodStatus.AddAttribute "complete", "true"
      clsWebData_GetData = True
    End If
  End If
  
  pManager.LogDbg "  clsBasins GetData exit"
  
  Exit Function

download:
  If pManager.State >= 999 Then Exit Function
  iDownload = iDownload + 1
  StepName = "download " & iDownload
  downloaded_file = cache_dir & huc & cur_suffix
  url = base_url & huc & "/" & huc & cur_suffix
  
download2:
  If Not pManager.download(url, downloaded_file, FirstFile, _
                          "Downloading " & pShortLabel & " " & huc & cur_suffix _
                        & " (" & iDownload & " of " & nDownloads & ")") Then
UserCancelled:
    nodStatus.AddAttribute "message", "User Cancelled"
    pManager.LogDbg "  clsBasins User Cancelled"
    
    Exit Function '!!!!!!!!!!!!!!!!!!!
  
  End If
  'If InStr(pManager.ResultString, "No file created at ") > 0 Then
  'End If
  
  'unpack self-extracting archive
  StepName = "unpack " & cache_dir & huc & cur_suffix
  If cur_suffix = demg_suffix And launched_by = "ArcView" Then
    dest_dir = project_dir & "grids\dem\dd\" & huc
  ElseIf cur_suffix = demg_suffix And launched_by <> "ArcView" Then
    dest_dir = cache_dir & "demg\"
  ElseIf cur_suffix = ned_suffix And launched_by = "ArcView" Then
    dest_dir = project_dir & "grids\ned\dd\" & huc
  ElseIf cur_suffix = ned_suffix And launched_by <> "ArcView" Then
    dest_dir = cache_dir & "ned\"
  ElseIf cur_suffix = nhd_suffix Then
    dest_dir = project_dir & "nhd\dd\"
  ElseIf cur_suffix = a303d_suffix Then
    dest_dir = cache_dir & "303d\"
  ElseIf cur_suffix = census_suffix Then
    dest_dir = cache_dir & "census\"
  ElseIf cur_suffix = dem_suffix Then
    dest_dir = cache_dir & "dem\"
  ElseIf cur_suffix = lstoret_suffix Then
    dest_dir = cache_dir & huc & "\lstoret\"
  Else
    dest_dir = cache_dir
  End If
  
  MkDirPath dest_dir
  
  Select Case FileType(downloaded_file)
    Case "exe"
      pManager.StartProcess "Unzip" & Left(cur_suffix, Len(cur_suffix) - 4), _
                            downloaded_file & " /auto " & dest_dir
    Case "txt"
      FileContents = WholeFileString(downloaded_file)
      If Left(FileContents, 16) = "Data Unavailable" Then
        pManager.LogDbg "No data available for " & downloaded_file
      Else
        If FileExists(downloaded_file) Then Kill downloaded_file
DownloadError:
        user_response = pManager.LogMsg("Error downloading file:" & vbCr _
                                       & "'" & downloaded_file & "' (" & Len(FileContents) & " bytes)", _
                                         "clsBasins GetData", _
                                         "Abort", "Retry", "Ignore", "View Error")
        Select Case user_response
          Case 1: pManager.State = 1003: GoTo UserCancelled
          Case 2: GoTo download
          Case 3: 'Ignore
          Case 4: pManager.ShowHTML FileContents: GoTo DownloadError
        End Select
      End If
    Case Else
      Select Case pManager.LogMsg("Downloaded file:" & vbCr _
                                       & "'" & downloaded_file & "' (" & FileLen(downloaded_file) & " bytes)" & vbCr _
                                       & "should be a self-extracting archive, but it does not seem to be.", _
                                         "clsBasins GetData", _
                                         "Abort", "Retry", "Ignore")
        Case 1: If FileExists(downloaded_file) Then Kill downloaded_file
                pManager.State = 1003
                GoTo UserCancelled
        Case 2: If FileExists(downloaded_file) Then Kill downloaded_file
                GoTo download
        Case 3: If FileExists(downloaded_file) Then Kill downloaded_file
      End Select
  End Select
  StepName = "FirstFile = False"
  FirstFile = False
  Return
  
ErrHand:
  pManager.LogMsg StepName & vbCrLf & Err.Description, "clsBasins GetData"
End Function

Private Sub CopyFromIfNeeded(Filename As String, fromdir As String, todir As String)
  If Not FileExists(todir & Filename) Then
    If FileExists(fromdir & Filename) Then
      FileCopy fromdir & Filename, todir & Filename
    Else
      pManager.LogDbg "Could not copy " & Filename & " from " & fromdir & " to " & todir
    End If
  End If
End Sub

Private Sub StrAppend(ByRef Orig As String, Addition As String)
  If InStr(Orig, Addition) = 0 Then Orig = Orig & Addition
End Sub

'Find all DBFs that are not part of shape files in from_dir
'and merge them with ones by the same name in to_dir
'
'If a DBF by the same name is not already in to_dir, just move the new one into place
'otherwise append any records onto the file in to_dir if those records
'are not already there, then delete the file from from_dir
Private Sub MergeNonShapeDBFs(ByVal from_dir As String, ByVal to_dir As String)
  Dim DBFfilename As String
  Dim vFilename As Variant
  Dim allDBFnames As New FastCollection
  
  If Right(from_dir, 1) <> "\" Then from_dir = from_dir & "\"
  If Right(to_dir, 1) <> "\" Then to_dir = to_dir & "\"
  
  DBFfilename = Dir(from_dir & "*.dbf")
  While Len(DBFfilename) > 0
    If Not FileExists(from_dir & FilenameOnly(DBFfilename) & ".shp") Then
      allDBFnames.Add DBFfilename
    End If
    DBFfilename = Dir
  Wend
  
  For Each vFilename In allDBFnames
    MergeDBF to_dir & vFilename, from_dir & vFilename, Nothing, "**ALL**"
    DoEvents
    If pManager.State >= 999 Then Exit Sub
  Next
End Sub

Private Function MergeDBF(Destination As String, Source As String, sourceDBF As clsDBF, ParamArray keyFieldNames()) As clsDBF
  Dim iKey As Integer
  Dim sKeyFieldNames() As String
  Dim destDBF As clsDBF
  Dim hadRecords As Long
  Dim filesNotIdentical As Boolean
  Dim errMsg As String
  Dim starttime As Single
  starttime = Timer

  pManager.DownloadStatus = "Merging " & Mid(PathNameOnly(Source), 11) & " into " & Mid(Destination, 16) 'C:\BASINS\data\ = 15 chars
  If Not FileExists(Destination) Then
SaveSourceAsDest:
    If Len(Source) > 0 Then
      pManager.LogDbg "  clsBasins:MergeDBF: Moving Source to Destination"
      Name Source As Destination
    ElseIf Not sourceDBF Is Nothing Then
      pManager.LogDbg "  clsBasins:MergeDBF: Writing new '" & Destination & "'"
      sourceDBF.WriteDBF Destination
    Else
      pManager.LogDbg "MergeDBF: No Source and no Destination: nothing to do"
    End If
  Else
    Select Case FilenameOnly(Destination)
      Case "pcscode", "sic", "storetag", "wqcriter", "wqobs_prm" 'These files are the same for every HUC, no merging needed
      Case Else
        If Not FileExists(Source) Then
          filesNotIdentical = True
        ElseIf FileLen(Destination) <> FileLen(Source) Then
          filesNotIdentical = True
        ElseIf FirstMismatch(Destination, Source) > 0 Then
          filesNotIdentical = True
        End If
        If filesNotIdentical Then 'We have downloaded a non-identical file, so merging is needed
          Set destDBF = New clsDBF
          'Set destDBF.Logger = pManager.Logger
          destDBF.OpenDBF Destination
          
          If destDBF.NumRecords < 1 Then
            pManager.LogDbg "  clsBasins:MergeDBF No records in Destination"
            Set destDBF = Nothing
            Kill Destination
            GoTo SaveSourceAsDest
          Else
            If sourceDBF Is Nothing Then
              Set sourceDBF = New clsDBF
              'Set sourceDBF.Logger = pManager.Logger
              sourceDBF.OpenDBF Source
            End If
            If UBound(keyFieldNames) < LBound(keyFieldNames) Then
              ReDim sKeyFieldNames(0)
            Else
              ReDim sKeyFieldNames(LBound(keyFieldNames) To UBound(keyFieldNames))
            End If
            For iKey = LBound(keyFieldNames) To UBound(keyFieldNames)
              sKeyFieldNames(iKey) = keyFieldNames(iKey)
            Next
            hadRecords = destDBF.NumRecords
            If FilenameOnly(PathNameOnly(Destination)) = "wqobs" Then
              errMsg = MergeBasObsWQ(destDBF, sourceDBF)
            Else
              destDBF.Merge sourceDBF, sKeyFieldNames, 1
            End If
            If Len(errMsg) > 0 Then
              pManager.LogMsg errMsg, "Error merging '" & sourceDBF & "' into '" & destDBF
            End If
            If destDBF.NumRecords > hadRecords Then
              destDBF.WriteDBF Destination
            End If
            pManager.LogDbg "  clsBasins:MergeDBF Added " & destDBF.NumRecords - hadRecords & " records of " _
                                                       & sourceDBF.NumRecords _
                                                       & " to " & hadRecords _
                                                       & " yielding " & destDBF.NumRecords _
                                                       & " in " & Format(Timer - starttime, "#.### sec")
          End If
          Set MergeDBF = destDBF
          Set destDBF = Nothing
          Set sourceDBF = Nothing
        End If
    End Select
    On Error Resume Next
    If FileExists(Source) Then Kill Source
  End If
End Function

Private Sub TryCopy(fromFilename As String, toFilename As String, Optional move As Boolean = False)
  Dim Msg As String
  On Error GoTo ErrHand
  If Not FileExists(fromFilename) Then
    pManager.LogDbg pShortLabel & " Could not copy '" & fromFilename & "' to '" & toFilename & "' because the source file did not exist"
  Else
Restart:
    If FileExists(toFilename) Then
      Msg = "removing '" & toFilename & "'"
      Kill toFilename
    End If
    Msg = "copying '" & fromFilename & "' to '" & toFilename & "'"
    FileCopy fromFilename, toFilename
    If move Then
      Msg = "removing '" & fromFilename & "'"
      Kill fromFilename
    End If
    Exit Sub
ErrHand:
    If pManager.LogMsg("Error " & Msg & vbCrLf & Err.Description _
      & vbCrLf & "ArcView may still have this file open", pShortLabel, "Ignore", "Retry") = 2 Then
      Resume Restart
    End If
  End If
End Sub

Private Property Get clsWebData_HelpId() As Long
  clsWebData_HelpId = pHelpID
End Property

Private Property Get clsWebData_Label() As String
  Static gaveLongLabel As Long
  If gaveLongLabel < 2 Then 'Only give the long label the first time
    clsWebData_Label = pLabel
    gaveLongLabel = gaveLongLabel + 1
  Else
    clsWebData_Label = pShortLabel
  End If
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
  SetProvidesCriteria
  Set clsWebData_Provides = pProvides
End Property

Private Sub clsWebData_Specify(Optional parent As Object = Nothing, Optional ByVal Index As Long = 0)
  pManager.ShowCriteria Me, parent, Index
End Sub

Private Property Get clsWebData_Version() As String
  clsWebData_Version = App.Major & "." & App.Minor & "." & App.Revision
End Property
