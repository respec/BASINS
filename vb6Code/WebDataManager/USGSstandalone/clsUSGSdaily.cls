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
Private pProvides As DOMDocument
Private pErrorDescription As String

Private pManager As clsWebDataManager

Private Sub Class_Initialize()
  Dim root As IXMLDOMElement
  Dim nod1 As IXMLDOMElement
  Dim nod2 As IXMLDOMElement
  
  Set pProvides = New DOMDocument
  Set pProvides.documentElement = pProvides.createElement(pClassName)
  Set root = pProvides.documentElement
  Set nod1 = pProvides.createElement("output")
    root.appendChild nod1
    Set nod2 = pProvides.createElement("file")
                                                 nod2.setAttribute "format", "RDB"
    nod1.appendChild nod2
  
  Set nod1 = pProvides.createElement("criteria")
    root.appendChild nod1
    Set nod2 = pProvides.createElement("site_no")
                                                 nod2.setAttribute "multiple", "true"
                                                 nod2.setAttribute "label", "Site Number"
    nod1.appendChild nod2
    
'    Set nod2 = pProvides.createElement("begin_date")
'                                                 nod2.setAttribute "type", "Date"
'                                                 nod2.setAttribute "format", "yyyy-mm-dd"
'                                                 nod2.setAttribute "optional", "true"
'                                                 nod2.setAttribute "label", "Begin Date"
'    nod1.appendChild nod2
'    Set nod2 = pProvides.createElement("end_date")
'                                                 nod2.setAttribute "type", "Date"
'                                                 nod2.setAttribute "format", "yyyy-mm-dd"
'                                                 nod2.setAttribute "optional", "true"
'                                                 nod2.setAttribute "label", "End Date"
'    nod1.appendChild nod2
    Set nod2 = pProvides.createElement("save_dir")
                                                 nod2.setAttribute "format", "savepath"
                                                 nod2.setAttribute "label", "Save in Directory"
    nod1.appendChild nod2
    
    Set nod2 = pProvides.createElement("SHPfilename")
                                                 nod2.setAttribute "format", "savefile"
                                                 nod2.setAttribute "optional", "true"
                                                 nod2.setAttribute "label", "Create point shape file"
                                                 nod2.setAttribute "filter", "Shape File|*.shp"
    nod1.appendChild nod2

    Set nod2 = pProvides.createElement("WDMfilename")
                                                 nod2.setAttribute "format", "openfile"
                                                 nod2.setAttribute "optional", "true"
                                                 nod2.setAttribute "label", "Add to WDM file"
                                                 nod2.setAttribute "filter", "WDM File|*.wdm"
    nod1.appendChild nod2
      
      
  Set root = Nothing
  Set nod1 = Nothing
  Set nod2 = Nothing
End Sub

Private Property Get clsWebData_HelpId() As Long
  clsWebData_HelpId = 11
End Property

Private Property Get clsWebData_Label() As String
  clsWebData_Label = pLabel
End Property

Private Property Get clsWebData_Name() As String
  clsWebData_Name = "clsUSGSdaily"
End Property

Private Property Get clsWebData_Provides() As DOMDocument
  Set clsWebData_Provides = pProvides
End Property

'Private Property Get clsWebData_Criteria() As DOMDocument
'  Dim root As IXMLDOMElement
'  Dim nod As IXMLDOMElement
'  Dim Dxml As DOMDocument
'  Set Dxml = New DOMDocument
'  Set Dxml.documentElement = Dxml.createElement("dataset")
'  Set root = Dxml.documentElement
'  Set nod = Dxml.createElement("site_no")
'                                                 nod.setAttribute "multiple", "true"
'                                                 root.appendChild nod
'  Set nod = Dxml.createElement("begin_date")
'                                                 nod.setAttribute "format", "yyyy-mm-dd"
'                                                 nod.setAttribute "optional", "true"
'                                                 root.appendChild nod
'  Set nod = Dxml.createElement("end_date")
'                                                 nod.setAttribute "format", "yyyy-mm-dd"
'                                                 nod.setAttribute "optional", "true"
'                                                 root.appendChild nod
'  Set nod = Dxml.createElement("state_abbrev")
'                                                 root.appendChild nod
'  Set nod = Dxml.createElement("save_dir")
'                                                 nod.setAttribute "type", "path"
'                                                 nod.setAttribute "label", "Save in directory"
'                                                 root.appendChild nod
'  Set clsWebData_Criteria = Dxml
'  Set Dxml = Nothing
'End Property

Private Function clsWebData_GetData(ByVal Query As DOMDocument, ByRef Result As DOMDocument) As Boolean
'  Dim nSites As Long
'  Dim site_no() As String
  Dim state_abbrev As String
  Dim begin_date As String
  Dim end_date As String
  Dim save_dir As String
  Dim save_filename As String
  Dim SHPfilename As String
  Dim WDMfilename As String
  Dim myDownloadFiles As Collection 'of file names
  Dim URL As String
  Dim iChild As Long, jChild As Long, iSite As Long
  Dim lstSites As IXMLDOMNodeList
  Dim vSite As Variant
  Dim lSite As IXMLDOMNode
  Dim vAttr As Variant
  Dim siteAttributes As String
  Dim FirstFile As Boolean
'  Dim tsfile As ATCclsTserFile
  
  Dim root As IXMLDOMElement
  Dim nodStatus As IXMLDOMElement
  
  clsWebData_GetData = True
  
  If Query.parseError <> 0 Then
    pErrorDescription = "Parse Error:" & Query.parseError
    clsWebData_GetData = False
  Else
'    ReDim site_no(0)
    For iChild = 0 To Query.childNodes(0).childNodes.Length - 1
      With Query.childNodes(0).childNodes.Item(iChild)
        Select Case LCase(.nodeName)
          Case "criteria"
            For jChild = 0 To .childNodes.Length - 1
              With .childNodes.Item(jChild)
                Select Case LCase(.nodeName)
'                  Case "site_no":         nSites = nSites + 1
'                                          ReDim Preserve site_no(nSites)
'                                          site_no(nSites) = .childNodes(0).nodeValue
                  Case "begin_date":      begin_date = .childNodes(0).nodeValue
                  Case "end_date":        end_date = .childNodes(0).nodeValue
                  Case "state_abbrev":    state_abbrev = .childNodes(0).nodeValue
                  Case "save_dir":        save_dir = .childNodes(0).nodeValue
                  Case "shpfilename":     SHPfilename = .childNodes(0).nodeValue
                  Case "wdmfilename":     WDMfilename = .childNodes(0).nodeValue
                End Select
              End With
            Next
        End Select
      End With
    Next
    
    If Len(save_dir) > 0 Then
      If Right(save_dir, 1) <> "\" Then save_dir = save_dir & "\"
    End If
    If Len(WDMfilename) > 0 Then Set myDownloadFiles = New Collection
    
    
    Set Result.documentElement = pProvides.createElement(pClassName)
    Set root = Result.documentElement
    Set nodStatus = Result.createElement("status")
    nodStatus.setAttribute "complete", "false"
    
    FirstFile = True
    iSite = 0
    Set lstSites = Query.getElementsByTagName("site_no")
    For Each vSite In lstSites 'For iSite = 1 To nSites
      iSite = iSite + 1
      Set lSite = vSite
      URL = "http://waterdata.usgs.gov/"
      If Len(state_abbrev) > 0 Then URL = URL & LCase(state_abbrev) & "/"
      URL = URL & "nwis/discharge?site_no=" & vSite.Text & _
               "&agency_cd=USGS" & _
               "&begin_date=" & begin_date & _
               "&end_date=" & end_date & _
               "&format=rdb" & _
               "&date_format=YYYY-MM-DD" & _
               "&submitted_form=brief_list"
      siteAttributes = "# " & URL & vbCrLf
      For Each vAttr In vSite.Attributes
        siteAttributes = siteAttributes & "# " & vAttr.nodeName & Space(48 - Len(vAttr.nodeName)) & vAttr.Text & vbCrLf
      Next
      save_filename = save_dir & state_abbrev & vSite.Text & ".txt"
      
      If Not pManager.Download(URL, save_filename, FirstFile, "Downloading " & pLabel & " (" & iSite & " of " & lstSites.Length & ")", siteAttributes) Then
        nodStatus.setAttribute "message", "User Cancelled"
        root.appendChild nodStatus

        Exit Function '!!!!!!!!!!!!!!!!!!!
      
      End If
      If InStr(pManager.ResultString, "<HTML>") > 0 Then Stop
      
      FirstFile = False
      If Len(WDMfilename) > 0 Then myDownloadFiles.Add save_filename
    Next
'    If Len(WDMfilename) > 0 Then
'      Set tsfile = ConvertUsgsDv2Wdm(WDMfilename, myDownloadFiles)
'      If Len(SHPfilename) > 0 Then
'        With frmShapeAttributes
'          .SetDataFile tsfile
'          .Show vbModal
'          If frmShapeAttributes.Ok Then
'            WriteShapeFile tsfile, SHPfilename, .cboX.Text, .cboY.Text, .SelectedAttributes
'          End If
'        End With
'        Unload frmShapeAttributes
'      End If
'    End If
    nodStatus.setAttribute "complete", "true"
  End If
  root.appendChild nodStatus
End Function

Private Property Set clsWebData_Manager(myManager As clsWebDataManager)
  Set pManager = Nothing
  Set pManager = myManager
End Property

Private Property Get clsWebData_Manager() As clsWebDataManager
  Set clsWebData_Manager = pManager
End Property

Private Sub clsWebData_Specify(Optional parent As Object = Nothing, Optional ByVal Index As Long = 0)
  pManager.ShowCriteria Me, parent, Index
  pManager.CurrentStatusUpdateString "USGSSiteDataRequested", "discharge", "set by " & pLabel
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

