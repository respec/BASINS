VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsNHD"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit

Implements clsWebData

Private Const pLabel = "National Hydrography Dataset (NHD)"
Private Const pClassName = "clsNHD"
Private Const pHelpID = 27
Private pProvides As ChilkatXml
Private pManager As clsWebDataManager

Private Sub Class_Initialize()
  Dim nod1 As ChilkatXml
  Dim nod2 As ChilkatXml
  
  Set pProvides = New ChilkatXml
  pProvides.Tag = pClassName
  Set nod1 = pProvides.newChild("output", "")
  Set nod2 = nod1.newChild("file", "")
      nod2.AddAttribute "format", "tgz"
  
  Set nod1 = pProvides.newChild("criteria", "")
  Set nod2 = nod1.newChild("huc_cd", "")
      nod2.AddAttribute "label", "8-digit HUC"
      nod2.AddAttribute "multiple", "true"
    
  Set nod2 = nod1.newChild("medium_resolution_nhd", "")
      nod2.AddAttribute "format", "boolean"
      nod2.AddAttribute "label", "Medium resolution NHD"
      nod2.AddAttribute "optional", "true"
    
  Set nod2 = nod1.newChild("high_resolution_nhd", "")
      nod2.AddAttribute "format", "boolean"
      nod2.AddAttribute "label", "High resolution NHD"
      nod2.AddAttribute "optional", "true"
      nod2.AddAttribute "selected", "true"
    
' Set nod2 = nod1.newChild("save_dir", "")
'     nod2.AddAttribute "format", "savepath" 'changed from openpath jlk 5/27/03
'     nod2.AddAttribute "label", "Save in Directory"
      
  Set nod1 = Nothing
  Set nod2 = Nothing

End Sub

Private Function clsWebData_GetData(ByVal aQuery As ChilkatXml, aResult As ChilkatXml) As Boolean
  Dim lQueryChild As ChilkatXml
  Dim lCritChild As ChilkatXml
  Dim nodStatus As ChilkatXml
  Dim base_url As String
  Dim high_url As String
  Dim med_url As String
  Dim url As String
  Dim cache_dir As String
  Dim huc_cd As String
  Dim hucs As FastCollection
  Dim vHuc As Variant
  Dim save_filename As String
  Dim ichild As Long, jchild As Long
  Dim high_resolution_nhd As Boolean
  Dim medium_resolution_nhd As Boolean
  Dim lTar As clsTar
  Dim iFile As Long
  Dim project_dir As String
  
  pManager.LogDbg "  clsNHD GetData entry"
  
  aResult.Tag = pClassName
  Set nodStatus = aResult.newChild("status", "")
  clsWebData_GetData = True
  
  project_dir = pManager.CurrentStatusGetString("project_dir")
  cache_dir = pManager.CurrentStatusGetString("cache_dir") & pClassName & "\"
  
  Set hucs = New FastCollection
  
  Set lQueryChild = aQuery.FirstChild
  While Not lQueryChild Is Nothing
    Select Case LCase(lQueryChild.Tag)
      Case "criteria"
        Set lCritChild = lQueryChild.FirstChild
        While Not lCritChild Is Nothing
          Select Case LCase(lCritChild.Tag)
            Case "huc_cd":                hucs.Add lCritChild.Content
            Case "high_resolution_nhd":   If LCase(lCritChild.Content) = "true" Then high_resolution_nhd = True
            Case "medium_resolution_nhd": If LCase(lCritChild.Content) = "true" Then medium_resolution_nhd = True
          End Select
          If lCritChild.NextSibling2 = 0 Then Set lCritChild = Nothing
        Wend
    End Select
    If lQueryChild.NextSibling2 = 0 Then Set lQueryChild = Nothing
  Wend
          
  base_url = pManager.CurrentStatusGetString("NHDbaseURL", "ftp://nhdftp.usgs.gov/FOD_Data/")
  high_url = pManager.CurrentStatusGetString("NHDhighURL", "High/arc/")
  med_url = pManager.CurrentStatusGetString("NHDmedURL", "Medium/arc/")
  
  ichild = InStr(base_url, "high_resolution")
  If ichild > 0 Then base_url = Left(base_url, ichild - 1)
  
  ichild = InStr(base_url, "medium_resolution")
  If ichild > 0 Then base_url = Left(base_url, ichild - 1)
  
  If Right(base_url, 1) <> "/" Then base_url = base_url & "/"

  For Each vHuc In hucs
    huc_cd = vHuc
    save_filename = cache_dir & huc_cd & ".tgz"
    If high_resolution_nhd Then
      url = base_url & high_url & huc_cd & ".tgz"
      GoSub DownloadOne
    End If
    If medium_resolution_nhd Then
      'Need to name file differently in case we also have or will get high resolution
      save_filename = cache_dir & huc_cd & "medium.tgz"
      url = base_url & med_url & huc_cd & ".tgz"
      GoSub DownloadOne
    End If
    
    If FileExists(save_filename) Then 'ungzip and untar downloaded file
      Set lTar = New clsTar
      lTar.TarFilename = save_filename
      For iFile = 1 To lTar.ArchiveFilenames.Count
        If Right(lTar.ArchiveFilenames(iFile), 1) = "/" Then
          MkDirPath cache_dir & lTar.ArchiveFilenames(iFile)
        Else
          'if file already exists, delete it
          If FileExists(cache_dir & lTar.ArchiveFilenames(iFile)) Then
            Kill cache_dir & lTar.ArchiveFilenames(iFile)
          End If
          lTar.ExtractFile iFile, cache_dir, lTar.ArchiveFilenames(iFile)
        End If
      Next
      MkDirPath project_dir & "nhd\dd\"
      pManager.AppendProjectXML _
        "<convert_coverage output=" & """" & project_dir & "nhd\dd\" & huc_cd & ".shp" & """" & ">" & cache_dir & huc_cd & "\nhd\route.rch</convert_coverage>" & vbCrLf & _
        "  <convert_shape output=" & """" & project_dir & "nhd\" & huc_cd & ".shp" & """" & ">" & project_dir & "nhd\dd\" & huc_cd & ".shp</convert_shape>" & vbCrLf & _
        "  <add_shape>" & project_dir & "nhd\" & huc_cd & ".shp</add_shape>"
    End If
  Next
  
  nodStatus.AddAttribute "complete", "true"
  pManager.LogDbg "  clsNHD GetData exit"
  Exit Function
  
DownloadOne:
  If pManager.Download(url, save_filename, True, "Downloading " & pLabel) Then
    Dim msg As String
    If Not FileExists(save_filename) Then
      pManager.LogMsg "Failed to download " & save_filename & vbCrLf & "(NHD data not available for all HUCs)", pLabel & " download"
    ElseIf FileLen(save_filename) < 1000 Then
      Dim webResponse As String
      webResponse = WholeFileString(save_filename)
      If InStr(webResponse, "not available") Then 'Data is not available for this HUC
        msg = webResponse & vbCrLf & "NHD data is not available for all HUCs"
        GoTo NotFound
      ElseIf InStr(webResponse, "404 Not Found") Then
        msg = "Requested File Not Found" & vbCrLf & "NHD data is not available for all HUCs"
NotFound:
        Select Case pManager.LogMsg(msg, "NHD Not Found", "Abort", "Retry", "Skip", "Show Message")
          Case 1
            If FileExists(save_filename) Then Kill save_filename
            Exit Function
          Case 2
            If FileExists(save_filename) Then Kill save_filename
            GoTo DownloadOne
          Case 3
            If FileExists(save_filename) Then Kill save_filename
            Return
          Case 4
            If FileExists(save_filename & ".html") Then Kill save_filename & ".html"
            Name save_filename As save_filename & ".html"
            pManager.ShowHTML WholeFileString(save_filename & ".html")
            Sleep 1000
            Kill save_filename & ".html"
            GoTo NotFound
        End Select
      ElseIf InStr(webResponse, "302 Found") Then
        Dim hrefPos As Integer
        hrefPos = InStr(LCase(webResponse), "a href=")
        If hrefPos > 0 Then
          url = Mid(webResponse, hrefPos + 8)
          hrefPos = InStr(url, """")
          If hrefPos > 0 Then
            pManager.LogDbg "Following web site redirection"
            url = Left(url, hrefPos - 1)
            Kill save_filename
            GoTo DownloadOne
          End If
        End If
        msg = "Unable to parse web site redirection"
        GoTo NotFound
      Else
        msg = "Suspiciously small file downloaded"
        GoTo NotFound
      End If
    End If
    Return
  End If
  'Don't return if user cancelled Download

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
