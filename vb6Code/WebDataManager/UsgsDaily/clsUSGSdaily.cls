VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsUSGSdaily"
Attribute VB_GlobalNameSpace = True
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
Implements clsWebData

Private Const pLabel = "USGS Daily Streamflow"
Private Const pClassName = "clsUSGSdaily"
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
  Set nod2 = nod1.newChild("discharge_site_no", "")
      nod2.AddAttribute "multiple", "true"
      nod2.AddAttribute "label", "Discharge Site Number"
        
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
  clsWebData_Name = "clsUSGSdaily"
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
  Dim msg As String
  Dim basins_dir As String
  Dim suffix As String
  Dim base_url As String
  Dim project_dir As String
  
  On Error GoTo ErrHand
  
  pManager.LogDbg "  clsUsgsDaily GetData entry"
  
  'http://waterdata.usgs.gov/nwis/dv?cb_00060=on&format=rdb&begin_date=2005-07-13&end_date=2006-07-13&site_no=01591000&referred_module=sw
  basins_dir = pManager.CurrentStatusGetString("basins_dir", CurDir)
  cache_dir = pManager.CurrentStatusGetString("cache_dir") & pClassName & "\"
  project_dir = pManager.CurrentStatusGetString("project_dir")
'  SHPfilename = project_dir & pManager.CurrentStatusGetString("USGSdailySHPfile", "gage.shp")
  suffix = pManager.CurrentStatusGetString("USGSdailySaveSuffix", "_dv.txt")
  base_url = pManager.CurrentStatusGetString("USGSDailyBaseURL", "http://waterdata.usgs.gov/nwis/dv?cb_00060=on")
    
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
      project_dir = project_dir & "USGSflow\"
      pManager.LogDbg "Saving RDB files in " & project_dir
    End If
    MkDirPath cache_dir
    MkDirPath project_dir
    
    aResult.Tag = pClassName
    Set nodStatus = aResult.newChild("status", "")
    nodStatus.AddAttribute "complete", "false"
    
    FirstFile = True
    iSite = 0
    Set lstSites = GetChildrenWithTag(aQuery.GetChildWithTag("criteria"), "discharge_site_no")
    If lstSites.Count = 0 Then
      Set lstSites = GetChildrenWithTag(aQuery.GetChildWithTag("criteria"), "site_no")
    End If
    For Each vSite In lstSites 'For iSite = 1 To nSites
      iSite = iSite + 1
      Set lSite = vSite
      url = base_url
      url = url & "&format=rdb" & _
                  "&begin_date=" & begin_date & _
                  "&end_date=" & end_date & _
                  "&site_no=" & lSite.Content

      siteAttributes = "# " & url & vbCrLf
      For iAttr = 0 To lSite.NumAttributes - 1
        siteAttributes = siteAttributes & "# " & lSite.GetAttributeName(iAttr) _
                                & Space(48 - Len(lSite.GetAttributeName(iAttr))) _
                                               & lSite.GetAttributeValue(iAttr) & vbCrLf
      Next
      save_filename = cache_dir & lSite.Content & suffix
      
      If Not pManager.Download(url, save_filename, FirstFile, "Downloading " & pLabel & " (" & iSite & " of " & lstSites.Count & ")", siteAttributes) Then
        nodStatus.AddAttribute "message", "User Cancelled"

        Exit Function '!!!!!!!!!!!!!!!!!!!
      
      End If
      msg = WholeFileString(save_filename)
      
      findPos = InStr(msg, "<html")
      If findPos > 0 Then 'Got an error message or web page, not the data we expected
        msg = Mid(pManager.ResultString, findPos)
        Kill save_filename
        nFilesNotCreated = nFilesNotCreated + 1
        FilesNotCreated = FilesNotCreated & "   " & FilenameNoPath(save_filename)
        If InStr(msg, "No data") > 0 Then
          FilesNotCreated = FilesNotCreated & " (no data)"
        ElseIf InStr(msg, "No site") > 0 Then
          FilesNotCreated = FilesNotCreated & " (no site)"
        Else
          FilesNotCreated = FilesNotCreated & " (error)"

'          findPos = InStr(msg, "3>")
'          If findPos > 0 Then
'            msg = Mid(msg, findPos + 2)
'            findPos = InStr(msg, "</")
'            If findPos > 0 Then msg = Left(msg, findPos - 1)
'          End If
'          If Len(msg) < 80 Then
'            FilesNotCreated = FilesNotCreated & " (" & msg & ")"
'          Else
'            msg = Mid(pManager.ResultString, InStr(pManager.ResultString, "<HTML>"))
'            msg = ReplaceString(msg, "<HEAD>", "<HEAD><base href=""" & base_url & "nwis")
'            pManager.ShowHTML msg
'          End If
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
      ConvertUsgsDv2Wdm WDMfilename, myDownloadFiles
      'SaveWDMandSHP WDMfilename, SHPfilename, basins_dir, myDownloadFiles
      On Error GoTo ErrHand
    End If
'    If Len(SHPfilename) > 0 Then
'      pManager.LogDbg "Saving any new stations found to '" & SHPfilename & "'"
'    End If
    If nFilesNotCreated > 0 Then
      save_filename = project_dir & "USGSflowNoData.txt"
      While Len(Dir(save_filename)) > 0
        FileNumber = FileNumber + 1
        save_filename = project_dir & "USGSflowNoData(" & FileNumber & ").txt"
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
      If pManager.LogMsg("Did not find data for " & nFilesNotCreated & " of " & lstSites.Count & " stations: " & vbCr & vbCr _
               & msg & vbCr _
               & "Save this list to " & save_filename & "?", _
               pLabel & " - Some data not found", "+&Yes", "-&No") = 1 Then
        SaveFileString save_filename, FilesNotCreated
      End If
    End If
    
    nodStatus.AddAttribute "complete", "true"
  End If
  pManager.LogDbg "  clsUsgsDaily GetData exit"
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
'  Set tsfile = ConvertUsgsDv2Wdm(WDMfilename, myDownloadFiles) ', Attribs)
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
  pManager.CurrentStatusUpdateString "USGSSitedischarge", "discharge", "set by " & pLabel
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

Private Function ConvertUsgsDv2Wdm(myWDMFileName As String, _
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
    
  Set myWDM = New clsTSerWDM
  Set myWDMExt = myWDM
  Set ConvertUsgsDv2Wdm = myWDM
  
  Set myRegistry = New ATCoRegistry

  Dim AllAttributes As Collection

  myRegistry.AppName = "GenScn"
'  New version of clsTSerWDM no longer requires us to set MsgUnit from here
'  p = GetSetting("HASS_ENT", "files", "HSPFMsgWDM", "c:\vbexpermental\genscn\bin\hspfmsg.wdm")
'  myWDMExt.MsgUnit = F90_WDBOPN(CLng(0), p, Len(p))

  If FileExists(myWDMFileName) Then
    myWDM.FileName = myWDMFileName
  Else
    myWDM.SaveAs myWDMFileName
    If LCase(myWDM.FileName) <> LCase(myWDMFileName) Then
      pManager.LogDbg pLabel & " could not create WDM file " & myWDMFileName
      
      Exit Function '!!!!!!!!!!!!!!!!!!!!
    
    End If
  End If
  
  pManager.LogDbg pLabel & " ConvertUsgsDv2Wdm " & myWDMFileName

  'use a registry key for the WDMUtil bin folder (scripts just below)
  p = myRegistry.RegGetString(HKEY_LOCAL_MACHINE, "SOFTWARE\AQUA TERRA Consultants\WDMUtil\ExePath", "")
  If Len(p) = 0 Then p = "c:\vbexperimental\wdmutil"
  myScriptLocation = p & "\scripts\USGSdvBASINSdownload.ws"
  
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
    myMem.FileName = myDownloadFileName & "|" & myScriptLocation
    If myMem.DataCount > 0 Then
      If UBound(myMem.Data(1).Values) > 1 Then
        myWDM.AddTimSer myMem.Data(1), TsIdRenum
        With myWDM.Data(myWDM.DataCount)
          'Accessing .Max forces reading all data in dataset
          pManager.LogDbg myDownloadFileName & " added to WDM. DataCount=" & myWDM.DataCount & ", ID=" & .Header.id & ", Max=" & .Max
        End With
      End If
    End If
  Next
  Set myMem = Nothing
End Function

