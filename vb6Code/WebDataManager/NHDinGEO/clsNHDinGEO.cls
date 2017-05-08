VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsNHDinGEO"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit

Implements clsWebData

Private Const pLabel = "National Hydrography Dataset (NHDinGEO)"
Private Const pClassName = "clsNHDinGEO"
Private Const pHelpID = 28
Private pProvides As ChilkatXml
Private pManager As clsWebDataManager
Private pCriteria As ChilkatXml

'Private huc_cd As String

Private Sub Class_Initialize()
  Dim nod1 As ChilkatXml
  Dim nod2 As ChilkatXml
  
  Set pProvides = New ChilkatXml
  pProvides.Tag = pClassName
  Set nod1 = pProvides.newChild("output", "")
  Set nod2 = nod1.newChild("file", "")
      nod2.AddAttribute "format", "mdb"
  
  Set pCriteria = pProvides.newChild("criteria", "")
  Set nod2 = pCriteria.newChild("huc_cd", "")
      nod2.AddAttribute "label", "8-digit HUC"
      nod2.AddAttribute "multiple", "true"
      
'  Set nod1 = pProvides.newChild("criteria", "")
'  Set nod2 = nod1.newChild("MDBfile", "")
'      nod2.AddAttribute "format", "openfile"
'      nod2.AddAttribute "label", "Downloaded NHDinGEO file"
'      nod2.AddAttribute "filter", "Geodatabase File|*.mdb|All Files|*.*"
  
  Set nod1 = Nothing
  Set nod2 = Nothing

End Sub

Private Function clsWebData_GetData(ByVal aQuery As ChilkatXml, aResult As ChilkatXml) As Boolean
  Dim lQueryChild As ChilkatXml
  Dim lCritChild As ChilkatXml
  Dim nodStatus As ChilkatXml
  Dim project_dir As String
  Dim GeoDatabaseFilename As String
  Dim slashpos As Long
  Dim base_url As String
  Dim high_prefix As String
  Dim med_prefix As String
  Dim high_suffix As String
  Dim med_suffix As String
  Dim url As String
  Dim huc8 As String
  Dim huc4 As String
  Dim lstHUC8s As FastCollection
  Dim lstHUC4s As New FastCollection
  Dim vHUC As Variant
  Dim iHUC As Long 'Index of current HUC for download progress indicator
  Dim FirstFile As Boolean
  Dim zipMsg As String
  
  pManager.LogDbg "  clsNHD GetData entry"
  FirstFile = True
  
  aResult.Tag = pClassName
  Set nodStatus = aResult.newChild("status", "")
  
  base_url = pManager.CurrentStatusGetString("NHDinGEObaseURL", "ftp://nhdftp.usgs.gov/SubRegions/")
  If Right(base_url, 1) <> "/" Then base_url = base_url & "/"
  
  high_prefix = pManager.CurrentStatusGetString("NHDinGEOhighPrefix", "High/NHDH")
  med_prefix = pManager.CurrentStatusGetString("NHDinGEOmedPrefix", "Medium/NHDM")
  high_suffix = pManager.CurrentStatusGetString("NHDinGEOhighSuffix", ".zip")
  med_suffix = pManager.CurrentStatusGetString("NHDinGEOmedSuffix", ".zip")
  
  project_dir = pManager.CurrentStatusGetString("project_dir")
  
  MkDirPath project_dir & "nhd\dd\"
  Set lstHUC8s = GetChildrenWithTag(aQuery.GetChildWithTag("criteria"), "huc_cd")
  For Each vHUC In lstHUC8s
    iHUC = iHUC + 1
    huc8 = vHUC.Content
    huc4 = Left(huc8, 4)
    If lstHUC4s.IndexFromKey(huc4) < 1 Then 'Haven't just gotten this 4-digit HUC for another 8-digit
      lstHUC4s.Add huc4, huc4
      url = base_url & high_prefix & huc4 & high_suffix
      
      GeoDatabaseFilename = high_prefix & huc4 & high_suffix
      slashpos = InStrRev(GeoDatabaseFilename, "/")
      If slashpos > 0 Then
        GeoDatabaseFilename = Mid(GeoDatabaseFilename, slashpos + 1)
      End If
      GeoDatabaseFilename = project_dir & "nhd\dd\" & GeoDatabaseFilename
      
      If Not pManager.download(url, GeoDatabaseFilename, FirstFile, _
                              "Downloading NHD High Resolution for " & huc4, , _
                              pClassName & "\" & FilenameNoPath(GeoDatabaseFilename)) Then
        GoTo UserCancelled
      End If
      If LCase(FileExt(GeoDatabaseFilename)) = "zip" Then
        pManager.DownloadStatus = "Unzipping NHD High Resolution for " & huc4
        zipMsg = VBUnzip(GeoDatabaseFilename, PathNameOnly(GeoDatabaseFilename), False, False, False, pManager)
      End If
      If Len(zipMsg) > 0 Then 'Could not unzip
        pManager.LogDbg "Unable to unzip high resolution NHD GeoDatabase, so attempting medium"
        GoTo TryMedium
      Else
        GeoDatabaseFilename = FilenameSetExt(GeoDatabaseFilename, "mdb")
        pManager.DownloadStatus = "Converting NHD High Resolution to shape"
        If Not GeoDatabaseToShape(GeoDatabaseFilename, project_dir & "nhd\") Then
          pManager.LogDbg "Unable to open high resolution NHD GeoDatabase, so attempting medium"
TryMedium:
          url = base_url & med_prefix & huc4 & med_suffix
          GeoDatabaseFilename = project_dir & "nhd\dd\med" & huc4 & high_suffix
          If Not pManager.download(url, GeoDatabaseFilename, FirstFile, _
                                  "Downloading NHD Medium Resolution for " & huc4, , _
                                  pClassName & "\" & FilenameNoPath(GeoDatabaseFilename)) Then
            GoTo UserCancelled
          End If
          
          If LCase(FileExt(GeoDatabaseFilename)) = "zip" Then
            pManager.DownloadStatus = "Unzipping NHD Medium Resolution for " & huc4
            zipMsg = VBUnzip(GeoDatabaseFilename, PathNameOnly(GeoDatabaseFilename), False, False, False, pManager)
          End If
          If Len(zipMsg) > 0 Then 'Could not unzip
            pManager.LogDbg "Unable to unzip medium resolution NHD GeoDatabase"
          Else
            GeoDatabaseFilename = FilenameSetExt(GeoDatabaseFilename, "mdb")
            pManager.DownloadStatus = "Converting NHD Medium Resolution to shape"
            If Not GeoDatabaseToShape(GeoDatabaseFilename, project_dir & "nhd\") Then
              pManager.LogDbg "Unable to open medium resolution NHD GeoDatabase"
            End If
          End If
        End If
      End If
    End If
  Next
  
  clsWebData_GetData = True
  nodStatus.AddAttribute "complete", "true"
  pManager.LogDbg "  clsNHDinGEO GetData exit"
  
  Exit Function

UserCancelled:
  nodStatus.AddAttribute "message", "User Cancelled"
  pManager.LogDbg "  clsBasins User Cancelled"
  Exit Function

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

Private Function GeoDatabaseToShape(aDatabaseFilename As String, aSaveDir As String) As Boolean
  Dim gdb As New clsGeoDatabase
  Dim LayersToSave As FastCollection
  Dim iLayer As Long
  Dim LayerName As String
  Dim ShapeFilename As String
  
  'On Error GoTo ErrHandler
  
  If Not gdb.OpenGeoDatabase(aDatabaseFilename) Then
    pManager.LogDbg "GeoDatabaseToShape: Could not open '" & aDatabaseFilename & "'" & vbCrLf _
          & gdb.LastError
  Else
    Set LayersToSave = frmSelectLayers.SelectLayers(gdb)
    Unload frmSelectLayers
    For iLayer = 1 To LayersToSave.Count
      LayerName = LayersToSave.ItemByIndex(iLayer)
      pManager.DownloadStatus = "Converting table " & LayerName
      ShapeFilename = aSaveDir & "dd\" & FilenameOnly(aDatabaseFilename) & "-" & LayerName & ".shp"
      
      If LCase(LayerName) = "nhdflowline" Then
          gdb.SaveShape LayerName, ShapeFilename, "DSRCHID,ComID,NHDFlow,FromComID,ToComID", pManager
          pManager.DownloadStatus = "Reformatting NHD flowline"
          ReformatNHDflowlineDBF gdb.LastDBFsaved
          'If Len(huc_cd) = 8 Then
          '  LayerName = huc_cd
          '  ShapeFilename = aSaveDir & "dd\" & LayerName & ".shp"
          'End If
      Else
          gdb.SaveShape LayerName, ShapeFilename, , pManager
      End If
      
      pManager.AppendProjectXML _
        "  <convert_shape output=" & """" & aSaveDir & FilenameNoPath(ShapeFilename) & """>" & ShapeFilename & "</convert_shape>" & vbCrLf & _
        "  <add_shape>" & aSaveDir & FilenameNoPath(ShapeFilename) & "</add_shape>"
      
NextTable:
    Next
    GeoDatabaseToShape = True 'Success
  End If
      
  Exit Function

NoRecords:
  Resume NextTable

ErrHandler:
  If Err.Number = 32755 Then Resume NextTable
  pManager.LogMsg Err.Description, "GeoDatabaseToShape"
  Exit Function

End Function

Private Sub ReformatNHDflowlineDBF(oldDBF As clsDBF)
  'Dim oldDBF As clsDBF
  Dim newDBF As clsDBF
  Dim oldField As Long
  Dim newField As Long
  Dim DateField As Long
  Dim MetersField As Long
  Dim ComIDField As Long
  Dim ReachCodeField As Long
  Dim NewRchIDField As Long
  Dim iRecord As Long
  Dim OldBaseFilename As String
  Dim NewBaseFilename As String

  Set newDBF = NewNHDtable
  'Set oldDBF = New clsDBF
  'oldDBF.OpenDBF aShapeDBFfilename
  newDBF.NumRecords = oldDBF.NumRecords
  newDBF.InitData
  
  ReDim DBFmatchingField(oldDBF.numFields)
  For oldField = 1 To oldDBF.numFields
    Select Case LCase(oldDBF.fieldName(oldField))
      Case "comid":      newField = newDBF.FieldNumber("COM_ID"):  ComIDField = newField
      Case "fdate":      newField = newDBF.FieldNumber("RCH_DATE"): DateField = newField
      Case "resolution": newField = newDBF.FieldNumber("LEVEL")
      Case "gnis_id":    newField = newDBF.FieldNumber("GNIS_ID")
      Case "gnis_name":  newField = newDBF.FieldNumber("NAME")
      Case "lengthkm":   newField = newDBF.FieldNumber("METERS"): MetersField = newField
      Case "reachcode":  newField = newDBF.FieldNumber("RCH_CODE"): ReachCodeField = newField
      'Maybe there a a field by the same name, if not, zero
      Case Else: newField = newDBF.FieldNumber(oldDBF.fieldName(oldField))
    End Select
    DBFmatchingField(oldField) = newField
  Next
  NewRchIDField = newDBF.FieldNumber("RCHID")
  
  For iRecord = 1 To oldDBF.NumRecords
    oldDBF.CurrentRecord = iRecord
    newDBF.CurrentRecord = iRecord
    For oldField = 1 To oldDBF.numFields
      newField = DBFmatchingField(oldField)
      If newField > 0 Then
        Select Case newField
          Case MetersField
            If IsNumeric(oldDBF.Value(oldField)) Then
              newDBF.Value(newField) = oldDBF.Value(oldField) * 1000
            Else
              newDBF.Value(newField) = ""
            End If
        Case DateField
          newDBF.Value(newField) = ReformatDate(oldDBF.Value(oldField))
        Case ComIDField
          newDBF.Value(newField) = oldDBF.Value(oldField)
          If NewRchIDField > 0 Then 'Copy same ID to redundant field
            newDBF.Value(NewRchIDField) = oldDBF.Value(oldField)
          End If
        Case ReachCodeField
          newDBF.Value(newField) = oldDBF.Value(oldField)
          'If Len(huc_cd) < 8 Then huc_cd = Left(oldDBF.Value(oldField), 8)
        Case Else
          newDBF.Value(newField) = oldDBF.Value(oldField)
        End Select
      End If
    Next
  Next
'  If Len(Trim(huc_cd)) = 8 Then
'    OldBaseFilename = FilenameNoExt(oldDBF.FileName) & "."
'    NewBaseFilename = PathNameOnly(oldDBF.FileName) & "\" & huc_cd & "."
'    Kill oldDBF.FileName
'    If FileExists(NewBaseFilename & "shp") Then Kill NewBaseFilename & "shp"
'    If FileExists(NewBaseFilename & "shx") Then Kill NewBaseFilename & "shx"
'    newDBF.WriteDBF NewBaseFilename & "dbf"
'    Name OldBaseFilename & "shp" As NewBaseFilename & "shp"
'    Name OldBaseFilename & "shx" As NewBaseFilename & "shx"
'  Else
    newDBF.WriteDBF oldDBF.FileName
'    huc_cd = FilenameOnly(oldDBF.FileName)
'  End If
End Sub

'Re-format month/day/year to yyyymmdd
Private Function ReformatDate(mmddyyyy As String) As String
  Dim firstslash As Long
  Dim secondslash As Long
  ReformatDate = mmddyyyy 'default to original value if we can't parse
  firstslash = InStr(mmddyyyy, "/")
  If firstslash > 0 Then
    secondslash = InStr(firstslash + 1, mmddyyyy, "/")
    If secondslash > 0 Then
      ReformatDate = Mid(mmddyyyy, secondslash + 1) & _
        Format(Left(mmddyyyy, firstslash - 1), "00") & _
        Format(Mid(mmddyyyy, firstslash + 1, secondslash - firstslash - 1), "00")
    End If
  End If
End Function

Private Function NewNHDtable() As clsDBF
  Set NewNHDtable = New clsDBF
  With NewNHDtable
    .Year = CInt(Format(Now, "yyyy")) - 1900
    .Month = CByte(Format(Now, "mm"))
    .Day = CByte(Format(Now, "dd"))
    .numFields = 11
  
'    .FieldName(1) = "RCH_"
'    .FieldType(1) = "N"
'    .FieldLength(1) = 11
'    .FieldDecimalCount(1) = 0
'
'    .FieldName(2) = "RCH_ID"
'    .FieldType(2) = "N"
'    .FieldLength(2) = 11
'    .FieldDecimalCount(2) = 0
  
    .fieldName(1) = "COM_ID"
    .FieldType(1) = "N"
    .FieldLength(1) = 11
    .FieldDecimalCount(1) = 0
  
    .fieldName(2) = "RCH_CODE"
    .FieldType(2) = "C"
    .FieldLength(2) = 14
    .FieldDecimalCount(2) = 0
  
    .fieldName(3) = "RCH_DATE"
    .FieldType(3) = "C"
    .FieldLength(3) = 8
    .FieldDecimalCount(3) = 0
  
    .fieldName(4) = "LEVEL"
    .FieldType(4) = "N"
    .FieldLength(4) = 5
    .FieldDecimalCount(4) = 0
  
    .fieldName(5) = "METERS"
    .FieldType(5) = "N"
    .FieldLength(5) = 12
    .FieldDecimalCount(5) = 0
  
    .fieldName(6) = "GNIS_ID"
    .FieldType(6) = "C"
    .FieldLength(6) = 8
    .FieldDecimalCount(6) = 0
  
    .fieldName(7) = "NAME"
    .FieldType(7) = "C"
    .FieldLength(7) = 99
    .FieldDecimalCount(7) = 0
  
    .fieldName(8) = "RCHID"
    .FieldType(8) = "C"
    .FieldLength(8) = 10
    .FieldDecimalCount(8) = 0
  
    .fieldName(9) = "DSRCHID"
    .FieldType(9) = "C"
    .FieldLength(9) = 10
    .FieldDecimalCount(9) = 0
  
    .fieldName(10) = "FType"
    .FieldType(10) = "C"
    .FieldLength(10) = 10
    .FieldDecimalCount(10) = 0
  
    .fieldName(11) = "FCode"
    .FieldType(11) = "C"
    .FieldLength(11) = 10
    .FieldDecimalCount(11) = 0
  End With
End Function


