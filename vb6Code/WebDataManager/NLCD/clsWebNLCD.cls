VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsNLCD"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
Implements clsWebData

Private pBaseUrl As String
Private Const pLabel = "National Land Cover Database (NLCD)"
Private Const pClassName = "clsNLCD"
Private Const pHelpID = 29
Private pProvides As ChilkatXml
Private pManager As clsWebDataManager
Private pAllURL As FastCollection

Private Sub Class_Initialize()
  Dim nod1 As ChilkatXml
  Dim nod2 As ChilkatXml
  
  Set pProvides = New ChilkatXml
  pProvides.Tag = pClassName
  Set nod1 = pProvides.newChild("output", "")
  Set nod2 = nod1.newChild("file", "")
      nod2.AddAttribute "format", "gz"
  
  Set nod1 = pProvides.newChild("criteria", "")
  Set nod2 = pProvides.newChild("state_nlcd", "")
      nod2.AddAttribute "label", "State Name"
      nod2.AddAttribute "multiple", "true"
    
  Set nod2 = pProvides.newChild("tif_nlcd", "")
      nod2.AddAttribute "format", "boolean"
      nod2.AddAttribute "label", "TIFF NLCD"
      nod2.AddAttribute "optional", "true"
      nod2.AddAttribute "selected", "true"
      
  Set nod2 = pProvides.newChild("bin_nlcd", "")
      nod2.AddAttribute "format", "boolean"
      nod2.AddAttribute "label", "Binary NLCD"
      nod2.AddAttribute "optional", "true"

  Set nod1 = Nothing
  Set nod2 = Nothing
  
  Set pAllURL = New FastCollection
    
End Sub

Private Function clsWebData_GetData(ByVal aQuery As ChilkatXml, aResult As ChilkatXml) As Boolean
  Dim lQueryChild As ChilkatXml
  Dim lCritChild As ChilkatXml
  Dim url As String
  Dim Filenames As New Collection
  Dim vFilename As Variant
  Dim tFilename As String
  Dim cache_dir As String
  Dim tif_nlcd As Boolean
  Dim bin_nlcd As Boolean
  Dim i As Long
  Dim GzipPath As String
  Dim basins_dir As String
  Dim project_dir As String
  Dim national_nlcd As String
  
  Dim nodStatus As ChilkatXml
  Dim nod2 As ChilkatXml
  Dim ff As ATCoFindFile
  Dim iChild As Long, jChild As Long, iSite As Long
  Dim startWait As Single
  Dim lastUpdate As Single
  Dim launched_by As String
  Dim inputname As String
  Dim statename As String
  Dim clipname As String
  Dim projectedname As String
  ReDim site_no(0)
      
  On Error GoTo ErrHand
  pManager.LogDbg pClassName & " GetData entry"
  launched_by = pManager.CurrentStatusGetString("launched_by")
  
  clsWebData_GetData = True
  
  
  basins_dir = pManager.CurrentStatusGetString("basins_dir")
  national_nlcd = basins_dir & "data\national\nlcd\" 'Save unpacked version in national data folder
  cache_dir = pManager.CurrentStatusGetString("cache_dir") & pClassName & "\"
  MkDirPath national_nlcd
  MkDirPath cache_dir
  
  aResult.Tag = pClassName
  Set nodStatus = aResult.newChild("status", "")
  
  Set lQueryChild = aQuery.FirstChild
  While Not lQueryChild Is Nothing
    Select Case LCase(lQueryChild.Tag)
      Case "criteria"
        Set lCritChild = lQueryChild.FirstChild
        While Not lCritChild Is Nothing
          Select Case LCase(lCritChild.Tag)
            Case "state_nlcd":
              If pAllURL.KeyExists(lCritChild.Content) Then
                Filenames.Add pAllURL(lCritChild.Content)
              Else
                Err.Raise 1, "GetData", "State '" & lCritChild.Content & "' was not found"
              End If
            Case "tif_nlcd": If LCase(lCritChild.Content) = "true" Then tif_nlcd = True
            Case "bin_nlcd": If LCase(lCritChild.Content) = "true" Then bin_nlcd = True
            Case "cache_dir":   cache_dir = lCritChild.Content & pClassName & "\"
          End Select
          If lCritChild.NextSibling2 = 0 Then Set lCritChild = Nothing
        Wend
    End Select
    If lQueryChild.NextSibling2 = 0 Then Set lQueryChild = Nothing
  Wend
  
  SetBaseURL
  
  For Each vFilename In Filenames
    For i = 1 To 2
      tFilename = ""
      If tif_nlcd And i = 1 Then
        tFilename = vFilename
      ElseIf bin_nlcd And i = 2 Then
        tFilename = ReplaceString(CStr(vFilename), ".tif.", ".bin.")
      End If
      If Len(tFilename) > 0 Then
        If FileExists(national_nlcd & Left(tFilename, Len(tFilename) - 3)) Then 'compare without .gz
          pManager.LogDbg pClassName & " File exists, not downloading " & national_nlcd & Left(tFilename, Len(tFilename) - 3)
        Else
          url = pBaseUrl & tFilename
          If Not pManager.Download(url, cache_dir & tFilename, True, "Downloading " & pLabel) Then
            nodStatus.AddAttribute "complete", "false"
            nodStatus.AddAttribute "message", "User Cancelled"
            Exit Function '!!!!!!!!!!!!!!!!!!!!!!!
          End If
          If pManager.ResultString = "<undefined/>" Then
            nodStatus.AddAttribute "complete", "false"
            nodStatus.AddAttribute "message", "ResultString undefined"
          Else
            nodStatus.AddAttribute "complete", "true"
            If Len(vFilename) = 0 Then
              pManager.LogDbg pClassName & " GetData skipping blank filename"
            Else 'results to file
              Set nod2 = aResult.newChild("file", "")
              nod2.AddAttribute "name", cache_dir & tFilename
                  
              startWait = Timer
              lastUpdate = startWait
              pManager.DownloadStatus = "Unpacking NLCD to " & national_nlcd & Left(tFilename, Len(tFilename) - 3) & "..."
              'unzip to designated place
              If ff Is Nothing Then
                Set ff = New ATCoFindFile
                ff.SetDialogProperties "Please locate gzip.exe", CurDir & "\gzip.exe"
                ff.SetRegistryInfo "gzip", "files", "gzip.exe"
                GzipPath = """" & ff.GetName & """"
              End If
              pManager.LogDbg "Copying '" & cache_dir & tFilename & "' to '" & national_nlcd & tFilename & "'"
              FileCopy cache_dir & tFilename, national_nlcd & tFilename
              pManager.LogDbg "Shell '" & GzipPath & " -q -d -f """ & national_nlcd & tFilename & "'"
              Shell GzipPath & " -q -d -f """ & national_nlcd & tFilename & "" '-q = quiet, -d = decompress, -f = force overwrite
              While FileExists(national_nlcd & tFilename)
                DoEvents
                Sleep 100
                If Timer - lastUpdate > 1 Then
                  lastUpdate = Timer
                  pManager.DownloadStatus = "Unpacking NLCD to " & national_nlcd & Left(tFilename, Len(tFilename) - 3) & vbCr & "(Elapsed time " & Int(lastUpdate - startWait) & " sec)"
                End If
              Wend
            End If
          End If
        End If
      End If
    Next i
  Next
  If launched_by = "ArcView" Then
    If tif_nlcd Then
      pManager.LogDbg pClassName & " Launched by ArcView, write end instructions"
      pManager.CurrentStatusUpdateString "endinstructions", pLabel & " Instructions: " & vbCrLf _
            & "TIFF data has been downloaded to '" & national_nlcd & "'." & vbCrLf _
            & vbCrLf _
            & "Use the 'Landuse (Image)' option of the Add Theme (Grid) extension " & vbCrLf _
            & "to convert this image to a grid and add the resulting grid to the view " & vbCrLf _
            & "(requires Spatial Analyst)." & vbCrLf & vbCrLf _
            & "This image data is already projected to Albers Equal Area, Ref Lat 23 degrees." & vbCrLf _
            & "If you need this grid in another projection, use the Grid Projector" & vbCrLf _
            & "extension to project the resulting grid." & vbCrLf _
            & vbCrLf _
            & pManager.CurrentStatusGetString("endinstructions")
    End If
  Else
    pManager.LogDbg pClassName & " Not launched by ArcView, write projector xml"
    
    'clear out rogue endinstruction messages
    i = InStr(1, pManager.CurrentStatusGetString("endinstructions"), "Use the 'Landuse (Image)' option of the Add Theme (Grid)")
    If i > 0 Then
      pManager.CurrentStatusUpdateString "endinstructions", ""
      pManager.LogDbg pClassName & " Cleared rogue endinstruction message"
    End If
    
    For Each vFilename In Filenames
      For i = 1 To 2
        tFilename = ""
        If tif_nlcd And i = 1 Then
          tFilename = vFilename
          pManager.LogDbg pClassName & " tfilename = " & tFilename
          'tfilename will have the .gz extension
          statename = FilenameOnly(tFilename)
          Do While InStr(statename, ".")
            statename = FilenameOnly(statename)
          Loop
          pManager.LogDbg pClassName & " statename = " & statename
          MkDirPath national_nlcd & statename & "\"
          pManager.LogDbg pClassName & " made dir " & national_nlcd & statename & "\"
          'outputname = national_nlcd & statename & "\" & Left(tFilename, Len(tFilename) - 3)
          inputname = national_nlcd & Left(tFilename, Len(tFilename) - 3)
          project_dir = pManager.CurrentStatusGetString("project_dir")
          clipname = project_dir & "nlcd\" & statename & "_clip.tif"
          projectedname = project_dir & "nlcd\" & statename & "_clip_projected.tif"
          'pManager.AppendProjectXML "<convert_grid output=""" & outputname & """>" & inputname & "</convert_grid>" & vbCrLf & _
          '                          "  <add_grid>" & outputname & "</add_grid>"
          pManager.AppendProjectXML "<clip_grid output=""" & clipname & """>" & inputname & "</clip_grid>" & vbCrLf & _
                                    "  <convert_grid output=""" & projectedname & """>" & clipname & "</convert_grid>" & vbCrLf & _
                                    "  <add_grid>" & projectedname & "</add_grid>"
        End If
      Next i
    Next
  End If
  
  pManager.LogDbg pClassName & " GetData exit"
  
  Exit Function

ErrHand:
  If pManager.LogMsg("This data could not be downloaded automatically." & vbCr _
          & "(" & Err.Description & ")" & vbCr _
          & "Visit " & pBaseUrl & " now to find data?", _
          "clsNLCD GetData", "+&Yes", "-&No") = 1 Then
    If Not pManager.Batch Then OpenFile pBaseUrl
  End If
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
  Dim query As ChilkatXml
  Dim Result As ChilkatXml
  
  AddAllStates
  
  If pManager.Batch Then
    Set query = frmSpecifyNLCD.GetDefault(pManager)
  Else
    Set query = frmSpecifyNLCD.AskUser(pManager)
  End If
  If pManager.State < 999 Then
    Set Result = New ChilkatXml
    clsWebData_GetData query, Result
  End If
End Sub

Private Property Get clsWebData_Version() As String
  clsWebData_Version = App.Major & "." & App.Minor & "." & App.Revision
End Property

Private Sub AddAllStates()
  Dim FileName As String
  Dim state_nlcd As String
  Dim ResultString As String
  Dim HREFpos As Integer
  Dim NLCDpos As Integer
  Dim endHref As Integer
  Dim allNLCD As ChilkatXml
  Dim pValues As FastCollection
  Dim strAllNLCD As String
  
  SetBaseURL
  If pManager.Download(pBaseUrl, "", True, "Finding Available Data", , pClassName & "\states.xml") Then
    ResultString = pManager.ResultString
    If Len(ResultString) > 0 Then
      HREFpos = InStr(ResultString, "A HREF=")
      strAllNLCD = "<allNLCD>"
      While HREFpos > 0
        endHref = InStr(HREFpos + 1, ResultString, """>")
        If endHref > 0 Then
          NLCDpos = InStr(HREFpos + 1, ResultString, ".nlcd.tif")
          If NLCDpos > 0 And NLCDpos < endHref Then
            FileName = Mid(ResultString, HREFpos + 8, endHref - HREFpos - 8)
            state_nlcd = Mid(ResultString, HREFpos + 8, NLCDpos - HREFpos - 8)
            pManager.LogDbg pClassName & " " & state_nlcd & " " & FileName
            pAllURL.Add FileName, state_nlcd
            strAllNLCD = strAllNLCD & "<state_nlcd>" & state_nlcd & "</state_nlcd>"
          End If
        End If
        HREFpos = InStr(HREFpos + 1, ResultString, "A HREF=")
      Wend
      strAllNLCD = strAllNLCD & "</allNLCD>"
      Set allNLCD = New ChilkatXml
      allNLCD.LoadXml strAllNLCD
      Set pValues = GetChildrenWithTag(allNLCD, "state_nlcd")
      pManager.CurrentStatusUpdateList "state_nlcd", pValues, "set by " & pClassName
    End If
  
  ElseIf pManager.Batch Then
    pManager.LogDbg "Could not find list of states for " & pLabel
  
  ElseIf pManager.LogMsg("This data could not be found automatically." & vbCr _
          & "Visit " & pBaseUrl & " now to find data?", pLabel, "+&Yes", "-&No") = 1 Then
    OpenFile pBaseUrl
  End If

End Sub

Private Sub SetBaseURL()
  If Len(pBaseUrl) = 0 Then
    pBaseUrl = pManager.CurrentStatusGetString("NLCDbaseURL", _
                                               "http://edcftp.cr.usgs.gov/pub/data/landcover/states/")
  End If
End Sub

