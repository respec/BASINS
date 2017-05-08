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

Private Const pLabel = "Storet Station"
Private Const pClassName = "clsStoretStatn"
Private Const pBaseHUCUrl = "http://oaspub.epa.gov/storpubl/Huc_Hub?"
Private Const pBaseStationURL = "http://oaspub.epa.gov/storpubl/station_selection?"
Private Const pBaseStationHubURL = "http://oaspub.epa.gov/storpubl/station_hub"

Private pManager As clsWebDataManager
Private pProvides As DOMDocument

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
    Set nod2 = pProvides.createElement("XML")
                                                 nod2.setAttribute "type", "DOMDocument"
    nod1.appendChild nod2
'    Set nod2 = pProvides.createElement("file")
'                                                 nod2.setAttribute "format", "csv"
'    nod1.appendChild nod2
  
  Set nod1 = pProvides.createElement("criteria")
    root.appendChild nod1
    Set nod2 = pProvides.createElement("huc_cd")
                                                 nod2.setAttribute "label", "HUC"
    nod1.appendChild nod2
    
'    Set nod2 = pProvides.createElement("save_dir")
'                                                 nod2.setAttribute "format", "openpath"
'                                                 nod2.setAttribute "label", "Save in Directory"
'    nod1.appendChild nod2
      
  Set nod1 = pProvides.createElement("available")
    root.appendChild nod1
    Set nod2 = pProvides.createElement("storetstation")
      nod1.appendChild nod2
      Set nod3 = pProvides.createElement("storetstation")
                                                 nod2.appendChild nod3
                                                 nod3.setAttribute "name", "Storet Station"
                                                 nod3.setAttribute "type", "string"
                                                 nod3.setAttribute "required", "true"
                                                 nod3.setAttribute "selected", "true"
  Set root = Nothing
  Set nod1 = Nothing
  Set nod2 = Nothing

End Sub

Private Function BuildSelectHUCURL(HUC As String) As String
  Dim url As String
  url = pBaseHUCUrl
  If Len(HUC) = 1 Then HUC = "0" & HUC
  Select Case Len(HUC)
    Case 2
      url = url & "D_REGION_LIST=" & HUC & "~"
      Select Case HUC
        Case "01": url = url & "512"   '01       - New England Region
        Case "02": url = url & "6734"  '02       - Mid Atlantic Region"
        Case "03": url = url & "25568" '03       - South Atlantic-Gulf Region"
        Case "04": url = url & "1041"  '04       - Great Lakes Region"
        Case "05": url = url & "2224"  '05       - Ohio Region"
        Case "06": url = url & "32"    '06       - Tennessee Region"
        Case "07": url = url & "2557"  '07       - Upper Mississippi Region"
        Case "08": url = url & "343"   '08       - Lower Mississippi Region"
        Case "09": url = url & "802"   '09       - Souris-Red-Rainy Region"
        Case "10": url = url & "4990"  '10       - Missouri Region"
        Case "11": url = url & "650"   '11       - Arkansas-White-Red Region"
        Case "12": url = url & "14"    '12       - Texas-Gulf Region"
        Case "13": url = url & "130"   '13       - Rio Grande Region"
        Case "14": url = url & "2370"  '14       - Upper Colorado Region"
        Case "15": url = url & "954"   '15       - Lower Colorado Region"
        Case "16": url = url & "3054"  '16       - Great Basin Region"
        Case "17": url = url & "2594"  '17       - Pacific Northwest Region"
        Case "18": url = url & "5"     '18       - California Region"
        Case "19": url = url & ""      '19       - Alaska Region"
        Case "20": url = url & ""      '20       - Hawaii Region"
        Case "21": url = url & "234"   '21       - Caribbean Region"
      End Select
      url = url & "&huc_id=region"
    Case 4
      url = url & "D_SubRegion_List=" & HUC & "~"
      url = url & "&huc_id=subregion"
    Case 6
      url = url & "D_AcctUnit_List=" & HUC & "~"
      url = url & "&huc_id=acctunit"
    Case 8
      url = url & "&D_CatUnit_List=" & HUC & "~"
      url = url & "&huc_id=catunit"
  End Select
  
  url = url & "&delim=~"

  BuildSelectHUCURL = url
End Function

Private Function BuildSelectStationURL(ByVal StationForm As String) As String
  Dim srchpos As Long
  Dim url As String
  url = pBaseStationURL
  srchpos = InStr(StationForm, "<INPUT ")
  If srchpos = 0 Then
    url = ""
  Else
    StationForm = Mid(StationForm, srchpos)
    srchpos = InStr(StationForm, "<TABLE")
    If srchpos = 0 Then
      url = ""
    Else
      StationForm = Left(StationForm, srchpos - 1)
      StationForm = ReplaceString(StationForm, _
                                "<INPUT TYPE=""hidden"" NAME=""D_STATION_ID_LIST"" VALUE=""", _
                                "&D_STATION_ID_LIST=")
      StationForm = ReplaceString(StationForm, """>", "")
      StationForm = ReplaceString(StationForm, " ", "")
      StationForm = ReplaceString(StationForm, vbLf, "")
      url = url & Mid(StationForm, 2) 'Skip first &
      url = url & "&D_ORG_LIST=ALL"
      url = url & "&D_STYPE_LIST=ALL"
    End If
  End If
  BuildSelectStationURL = url
End Function


Private Function clsWebData_GetData(ByVal Query As MSXML.IXMLDOMDocument, result As MSXML.IXMLDOMDocument) As Boolean
  Dim Filename As String
  Dim huc_cd As String
  Dim save_dir As String
  Dim outputFormat As String
  Dim FirstFile As Boolean
  
  Dim root As IXMLDOMElement
  Dim nodStatus As IXMLDOMElement
  Dim nod2 As IXMLDOMElement
  Dim nod3 As IXMLDOMElement
  Dim nodList As IXMLDOMNodeList
  Dim webResponse As DOMDocument
  
  Dim URLstart As String
  Dim url As String
  Dim searchFor As String
  
  Dim DownloadNumber As Long
    
  clsWebData_GetData = True
  FirstFile = True
  
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
                Case "huc_cd":     huc_cd = .childNodes(0).nodeValue
                'Case "save_dir": save_dir = .childNodes(0).nodeValue
              End Select
            End With
          Next
      End Select
    End With
  Next

  Set result.documentElement = pProvides.createElement(pClassName)
  Set root = result.documentElement
  Set nodStatus = result.createElement("status")
  
  url = BuildSelectHUCURL(huc_cd)
  
  If Not pManager.Download(url, "", True, "Downloading " & pLabel) Then
    nodStatus.setAttribute "complete", "false"
    nodStatus.setAttribute "message", "User Cancelled"
    root.appendChild nodStatus

    Exit Function '!!!!!!!!!!!!!!!!!!!!!!!
    
  End If
  FirstFile = False

  url = BuildSelectStationURL(pManager.ResultString)

  If Len(url) = 0 Then
    nodStatus.setAttribute "complete", "false"
    nodStatus.setAttribute "message", "ResultString undefined"
  Else
    If Not pManager.Download(url, "", False, "Downloading " & pLabel) Then
      MsgBox "Error downloading " & pLabel & vbCr & url, pLabel
    Else
      nodStatus.setAttribute "complete", "true"
      If Len(save_dir) > 0 Then
        Set nod2 = result.createElement("file")
        nod2.setAttribute "name", save_dir & Filename
        root.appendChild nod2
      Else
        Call ParseStationsFrom(pManager.ResultString, result.childNodes(0))
      End If
    End If
  End If
  root.appendChild nodStatus
End Function

Private Sub ParseStationsFrom(ByVal result As String, BaseChildNode As IXMLDOMNode)
  Dim lNode As IXMLDOMElement
  Dim parsePos As Long
  Dim fieldnames() As String, nFields As Integer
  Dim curline As String
  Dim buf As String
  Dim field As Integer, NameField As Integer
  
  nFields = 0
  ReDim fieldnames(0)
  
  parsePos = InStr(result, "READONLY")
  If parsePos > 0 Then parsePos = InStr(parsePos, result, "VALUE=")
  If parsePos > 0 Then
    result = Mid(result, parsePos + 7)
    curline = StrSplit(result, """", "")
    While Len(curline) > 0
      buf = Trim(StrSplit(curline, " ", ""))
      If Len(buf) > 0 Then
        nFields = nFields + 1
        ReDim Preserve fieldnames(nFields)
        fieldnames(nFields) = buf
      End If
      'If UCase(buf) = FACILITY_KEY_FIELD Then NameField = nFields
    Wend
    parsePos = InStr(result, "<OPTION value=")
    If parsePos > 0 Then
      result = Mid(result, parsePos)
      While Len(result) > 0
        curline = StrSplit(result, vbLf, "")
        buf = StrSplit(curline, "=", "") 'Skip <OPTION value=
        buf = Trim(StrSplit(curline, ">", ""))
        Set lNode = pProvides.createElement("storetstation")
        lNode.Text = buf
        For field = 1 To nFields
          buf = Trim(StrSplit(curline, "~", ""))
          If Len(buf) > 0 Then lNode.setAttribute fieldnames(field), buf
        Next
        BaseChildNode.appendChild lNode
        Set lNode = Nothing
      Wend
    End If
  End If
End Sub

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

