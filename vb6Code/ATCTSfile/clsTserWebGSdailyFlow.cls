VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsTserWebGSdailyFlow"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants
Implements ATCclsTserFile

Private Const CatalogURL = "http://water.usgs.gov/nwis/discharge?sort_key=site_no&format=sitefile_output&sitefile_output_format=rdb&column_name=site_no&column_name=station_nm&column_name=dec_lat_va&column_name=dec_long_va&column_name=state_cd&column_name=county_cd&column_name=alt_va&column_name=huc_cd&column_name=drain_area_va"

Private pFileName As String
Private pFileExt As String
Private pErrorDescription As String
Private pData As Collection
Private pDates As Collection

Private pMonitor As Object
Private pMonitorSet As Boolean

Private Property Get ATCclsTserFile_AvailableAttributes() As Collection
  Set ATCclsTserFile_AvailableAttributes = New Collection
End Property

Private Sub ATCclsTserFile_clear()
  Set pData = Nothing
  Set pData = New Collection
  
  Set pDates = Nothing
  Set pDates = New Collection
End Sub

Private Property Get ATCclsTserFile_Data(index As Long) As ATCclsTserData
  Set ATCclsTserFile_Data = pData(index)
End Property

Private Property Get ATCclsTserFile_DataCount() As Long
  ATCclsTserFile_DataCount = pData.Count
End Property

Private Property Get ATCclsTserfile_DataCollection() As Collection
  Set ATCclsTserfile_DataCollection = pData
End Property

Private Property Get ATCclsTserFile_FileUnit() As Long
  ATCclsTserFile_FileUnit = 1
End Property

Private Property Let ATCclsTserFile_HelpFilename(ByVal NewValue As String)
  App.HelpFile = NewValue
End Property

Private Sub ATCclsTserFile_readData(dataObject As ATCclsTserData)
  'pData.add dataObject
End Sub

Private Sub ATCclsTserFile_refresh()
  'pErrorDescription = "Unable to refresh time series"
End Sub

Private Function ATCclsTserFile_writeDataHeader(dataObject As ATCclsTserData, Optional oldHeader As ATTimSerDataHeader) As Boolean
  'It seems harmless to simply do nothing, rather than give an error
  'pErrorDescription = "Unable to Write a Data Header for this type of time series"
  ATCclsTserFile_writeDataHeader = True
End Function

Private Sub Class_Initialize()
  pFileName = ""
  pFileExt = "All Files (*.*)|*.*"
  pErrorDescription = ""
  pMonitorSet = False
  ATCclsTserFile_clear
End Sub

Private Property Set ATCclsTserFile_Monitor(ByVal o As Object)
  Set pMonitor = o
  pMonitorSet = True
End Property

Private Property Get ATCclsTserFile_ErrorDescription() As String
  ATCclsTserFile_ErrorDescription = pErrorDescription
  pErrorDescription = ""
End Property

Private Property Get ATCclsTserFile_FileExtension() As String
  ATCclsTserFile_FileExtension = pFileExt
End Property

Private Property Get ATCclsTserFile_Label() As String
  ATCclsTserFile_Label = "Web GS Daily Flow"
End Property

Private Property Get ATCclsTserFile_FileName() As String
  ATCclsTserFile_FileName = pFileName
End Property

Private Property Let ATCclsTserFile_FileName(ByVal NewValue$)
  Dim DBF As clsDBF
  Dim CurrentRecord As Long
  Dim NewData As ATCclsTserData
  Dim percent As Long, LastPercent As Long
  Dim noDates As ATCclsTserDate
  Set noDates = New ATCclsTserDate
  
  pFileName = FilenameNoExt(NewValue)
  If Len(Dir(pFileName & ".dbf")) = 0 Then
    Set DBF = BuildSHPandDBF(pFileName)
    If pMonitorSet Then
      pMonitor.SendMonitorMessage "(OPEN Reading Web Catalog into memory)"
      pMonitor.SendMonitorMessage "(BUTTOFF CANCEL)"
      pMonitor.SendMonitorMessage "(BUTTOFF PAUSE)"
    End If
  Else
    If pMonitorSet Then
      pMonitor.SendMonitorMessage "(OPEN Reading Web Catalog)"
      pMonitor.SendMonitorMessage "(BUTTOFF CANCEL)"
      pMonitor.SendMonitorMessage "(BUTTOFF PAUSE)"
      pMonitor.SendMonitorMessage "(MSG1 Opening " & pFileName & ".dbf" & ")"
      pMonitor.SendMonitorMessage "(MSG2 This may take a minute or more)"
    End If
    
    Set DBF = New clsDBF
    DBF.OpenDBF pFileName & ".dbf"
  
  End If
  
  If pMonitorSet Then
    pMonitor.SendMonitorMessage "(MSG1 Reading " & DBF.NumRecords & " stations from " & pFileName & ".dbf)"
    pMonitor.SendMonitorMessage "(MSG2 0)"
    pMonitor.SendMonitorMessage "(MSG3 0%)"
    pMonitor.SendMonitorMessage "(MSG4 " & DBF.NumRecords & ")"
  End If
  
  For CurrentRecord = 1 To DBF.NumRecords
    If pMonitorSet Then
      percent = CurrentRecord * 100 / DBF.NumRecords
      If percent > LastPercent Then
        pMonitor.SendMonitorMessage "(MSG3 " & percent & "%)"
        pMonitor.SendMonitorMessage "(PROGRESS " & percent & ")"
        LastPercent = percent
      End If
    End If
    
    DBF.CurrentRecord = CurrentRecord
    Set NewData = New ATCclsTserData
    Set NewData.File = Me
    Set NewData.Dates = noDates
    With NewData.Header
      .Sen = "OBSERVED"
      .con = "FLOW"
      .loc = DBF.Value(1)
      .desc = DBF.Value(2)
      .id = CurrentRecord
    End With
    NewData.AttribSet "Latitude", DBF.Value(3)
    NewData.AttribSet "Longitude", DBF.Value(4)
    NewData.AttribSet "State", DBF.Value(5)
    NewData.AttribSet "County", DBF.Value(6)
    NewData.AttribSet "Altitude", DBF.Value(7)
    NewData.AttribSet "HUC", DBF.Value(8)
    NewData.AttribSet "Drainage Area", DBF.Value(9)
    pData.Add NewData
    Set NewData = Nothing
  Next
  DBF.Clear
  Set DBF = Nothing
  If pMonitorSet Then pMonitor.SendMonitorMessage "(CLOSE)"
End Property

Public Property Get ATCclsTserFile_Description() As String
  ATCclsTserFile_Description = "Web GS Daily Flow"
End Property

Private Function ATCclsTserFile_AddTimSer(t As ATCclsTserData, Optional ExistAction& = 0) As Boolean
  Dim i&, lExAct&, retcod&, s$, TsInd&
  Dim BtnName() As Variant, nBtns&, nextBtnName As String
  Dim AppendFg As Boolean, OvwrtFg As Boolean
  Dim myMsgBox As ATCoMessage, UsrExAct&
  
  Dim foundId As Boolean, sameId As ATCclsTserData, id As Long, newId As Long
  Dim foundSerial As Boolean, serial As Long
  Dim vData As Variant, vDate As Variant
  
  serial = t.serial: foundSerial = False
  id = t.Header.id:  foundId = False
  For Each vData In pData
    If vData.serial = serial Then foundSerial = True
    If vData.Header.id = id Then foundId = True: Set sameId = vData
  Next
  
  If foundSerial Then
    ATCclsTserFile_AddTimSer = False
    pErrorDescription = "AddTimSer: time series already added"
  ElseIf foundId Then
    lExAct = ExistAction 'use local copy of what to do if DSN exists
    AppendFg = False
    OvwrtFg = False
    newId = findNextId(id + 1) 'find next available Id
    If lExAct = TsIdNoAction Then  'just report the problem
      pErrorDescription = "WebGSDailyFlow:AddTimSer:Id " & id & " Exists:Next Available is " & newId
      Exit Function
    ElseIf lExAct > TsIdRenum Then 'ask user what to do
      nBtns = 0
      If lExAct And TsIdReplAsk Then nextBtnName = "&Overwrite": GoSub AddButtonName
'      If lExAct And TsIdAppendAsk Then nextBtnName = "&Append": GoSub AddButtonName
      If lExAct And TsIdRenumAsk Then nextBtnName = "&Renumber": GoSub AddButtonName
      'always have Cancel as last button (and default)
      nextBtnName = "+-&Cancel": GoSub AddButtonName
      Set myMsgBox = New ATCoCtl.ATCoMessage
      UsrExAct = myMsgBox.Showarray("ID number " & id & " already exists." & vbCrLf & "Next available ID is " & newId & vbCrLf & "What should be done to it?", "Id Collision", BtnName)
      If InStr(BtnName(UsrExAct - 1), "Overwrite") > 0 Then
        lExAct = TsIdRepl 'set to overwrite
      ElseIf InStr(BtnName(UsrExAct - 1), "Append") > 0 Then
        lExAct = TsIdAppend 'set to append
      ElseIf InStr(BtnName(UsrExAct - 1), "Renumber") > 0 Then
        lExAct = TsIdRenum 'set to renumber
      ElseIf InStr(BtnName(UsrExAct - 1), "Cancel") > 0 Then
        pErrorDescription = "WebGSDailyFlow:AddTimSer:Id " & id & ".  User Cancelled on message box to resolve."
        Exit Function
      End If
    End If
    Select Case lExAct
      Case TsIdRepl 'overwrite the conflicting data set
        If Not ATCclsTserFile_RemoveTimSer(sameId) Then Exit Function
      Case TsIdAppend 'append to data set
        pErrorDescription = "WebGSDailyFlow:AddTimSer:Append not yet available."
        Exit Function
'        If t.Dates.Summary.SJDay >= lData.Dates.Summary.EJDay Then
'          'start of new data follows end of existing, ok to append
'        Else 'can't append, new and existing data spans overlap
'          If ExistAction > TsIdRenum Then 'report problem to user
'            MsgBox "Time span of new data and existing data overlap." & vbCrLf & _
'                   "Unable to append data to data-set number " & t.Header.Id & ".", vbExclamation, "Data Problem"
'          End If
'          pErrorDescription = "WebGSDailyFlow:AddTimSer:Id " & Id & " could not have new data appended to it."
'          Exit Function
'        End If
      Case TsIdRenum 'renumber data set
        t.Header.id = newId 'assign to next available data-set number
    End Select
  End If
    
  ATCclsTserFile_AddTimSer = True
  pData.Add t
  Set t.File = Me
  
  'May need to add dates too, if another timeseries has not already added them
  foundSerial = False
  serial = t.Dates.serial
  For Each vDate In pDates
    If vDate.serial = t.serial Then foundSerial = True: Exit For
  Next
  If Not foundSerial Then pDates.Add t.Dates
    
  Exit Function

AddButtonName:
  ReDim Preserve BtnName(nBtns)
  BtnName(nBtns) = nextBtnName
  nBtns = nBtns + 1
  Return
End Function

Private Function findNextId(IdStart As Long) As Long
  Dim vData As Variant
  Dim retval As Long
  
  retval = IdStart
  For Each vData In pData
    If retval = vData.Header.id Then retval = findNextId(retval + 1): Exit For
  Next vData
  findNextId = retval
End Function

Private Function ATCclsTserFile_RemoveTimSer(t As ATCclsTserData) As Boolean
  Dim i&, searchSerial As Long
  Dim retval As Boolean
  Dim removeDate As Boolean
  searchSerial = t.serial
  retval = False
  For i = 1 To pData.Count
    If pData(i).serial = searchSerial Then
      pData.Remove i
      retval = True
      Exit For
    End If
  Next
  
  removeDate = True
  searchSerial = t.Dates.serial
  For i = 1 To pData.Count
    If pData(i).Dates.serial = searchSerial Then removeDate = False: Exit For
  Next

  If removeDate Then
    For i = 1 To pDates.Count
      If pDates(i).serial = searchSerial Then pDates.Remove i: Exit For
    Next
  End If
  If retval = False Then
    pErrorDescription = "RemoveTimSer: did not find " & ATCclsTserFile_Description
  End If
  ATCclsTserFile_RemoveTimSer = retval
End Function

Private Function ATCclsTserFile_RewriteTimSer(t As ATCclsTserData) As Boolean
  ATCclsTserFile_RewriteTimSer = False
  pErrorDescription = "RewriteTimSer not supported for " & ATCclsTserFile_Description
End Function

Private Function ATCclsTserFile_SaveAs(Filename As String) As Boolean
  ATCclsTserFile_SaveAs = False
  pErrorDescription = "Save As not supported for " & ATCclsTserFile_Description
End Function

'if baseFilename = "c:/path/and/filename"
'Writes c:/path/and/filename.dbf
'   and c:/path/and/filename.shp
'   and c:/path/and/filename.shx
Private Function BuildSHPandDBF(baseFilename As String) As clsDBF
  'http://water.usgs.gov/nwis/discharge?drain_area_va_min=1000&drain_area_va_conjunction=and&drain_area_va_max=1100&sort_key=site_no&group_key=NONE&format=sitefile_output&sitefile_output_format=rdb&column_name=site_no&column_name=station_nm&column_name=dec_lat_va&column_name=dec_long_va&column_name=state_cd&column_name=county_cd&column_name=alt_va&column_name=huc_cd&column_name=drain_area_va&list_of_search_criteria=drain_area_va
  'http://water.usgs.gov/nwis/discharge?sort_key=site_no&format=sitefile_output&sitefile_output_format=rdb&column_name=site_no&column_name=station_nm&column_name=dec_lat_va&column_name=dec_long_va&column_name=state_cd&column_name=county_cd&column_name=alt_va&column_name=huc_cd&column_name=drain_area_va
  Dim AllStations As String
  Dim LenAllStations As Long
  Dim RowStart As Long, RowEnd As Long
  Dim ThisRow As String
  Dim newDBF As clsDBF
  Dim nRows As Long
  Dim shpfile%, shxfile%
  Dim RecordNumber As Long
  Dim FieldNumber As Long
  Dim FileLength As Long   'Length of Shape file
  Dim ShapeType As Long    '1 = point
  Dim X#, Y#, lowX#, lowY#, uppX#, uppY# 'Lat/Long for shape file
  Dim percent As Long, LastPercent As Long
  Dim NumBadCoords As Long 'Number of stations with non-numeric Lat or Long
  
  If pMonitorSet Then
    pMonitor.SendMonitorMessage "(OPEN Building Web Catalog)"
    pMonitor.SendMonitorMessage "(BUTTOFF CANCEL)"
    pMonitor.SendMonitorMessage "(BUTTOFF PAUSE)"
    pMonitor.SendMonitorMessage "(MSG1 Opening " & CatalogURL & ")"
    pMonitor.SendMonitorMessage "(MSG2 for catalog " & baseFilename & ")"
  End If
  
  AllStations = frmTransfer.TP.OpenURL(CatalogURL)
  LenAllStations = Len(AllStations)
  
  If pMonitorSet Then
    pMonitor.SendMonitorMessage "(MSG1 Transferred " & LenAllStations & " bytes)"
    pMonitor.SendMonitorMessage "(MSG2 Counting Stations)"
  End If
  
  'First count all the rows
  RowEnd = InStr(AllStations, "8s" & vbCr) + 2
  While RowEnd < LenAllStations
    RowStart = RowEnd + 1
    RowEnd = InStr(RowStart, AllStations, vbCr)
    If RowEnd = 0 Then RowEnd = LenAllStations + 1
    If RowEnd > RowStart + 2 Then
      nRows = nRows + 1
    End If
  Wend

  If pMonitorSet Then
    pMonitor.SendMonitorMessage "(MSG1 Reading " & nRows & " stations into Shape SHP and DBF)"
    pMonitor.SendMonitorMessage "(MSG2 0)"
    pMonitor.SendMonitorMessage "(MSG3 0%)"
    pMonitor.SendMonitorMessage "(MSG4 " & nRows & ")"
  End If
  
  Set newDBF = New clsDBF
  With newDBF
    .NumRecords = nRows
    .NumFields = 9
    .FieldName(1) = "site_no":       .FieldType(1) = "C": .FieldLength(1) = 15
    .FieldName(2) = "station_nm":    .FieldType(2) = "C": .FieldLength(2) = 50
    .FieldName(3) = "dec_lat_va":    .FieldType(3) = "N": .FieldLength(3) = 8: .FieldDecimalCount(3) = 0
    .FieldName(4) = "dec_long_va":   .FieldType(4) = "N": .FieldLength(4) = 8: .FieldDecimalCount(4) = 0
    .FieldName(5) = "state_cd":      .FieldType(5) = "C": .FieldLength(5) = 2
    .FieldName(6) = "county_cd":     .FieldType(6) = "C": .FieldLength(6) = 3
    .FieldName(7) = "alt_va":        .FieldType(7) = "C": .FieldLength(7) = 8
    .FieldName(8) = "huc_cd":        .FieldType(8) = "C": .FieldLength(8) = 16
    .FieldName(9) = "drain_area_va": .FieldType(9) = "C": .FieldLength(9) = 8
    .InitData
  End With
  
  CreateNewShapeFile baseFilename, 1
  'Point Shape Main file (.shp)
  shpfile = FreeFile(0)
  Open baseFilename & ".shp" For Binary Access Read Write As shpfile
  Call ReadShapeHeader(shpfile, FileLength, ShapeType, lowX, lowY, uppX, uppY)
  Seek #shpfile, 101

  'Point Shape Index file (.shx)
  shxfile = FreeFile(0)
  Open baseFilename & ".shx" For Binary Access Read Write As shxfile
  Seek #shxfile, 101
  
  RowEnd = InStr(AllStations, "8s" & vbCr) + 2
  RecordNumber = 0
  While RowEnd < LenAllStations
    RecordNumber = RecordNumber + 1
    
    If pMonitorSet Then
      percent = RecordNumber * 100 / nRows
      If percent > LastPercent Then
        pMonitor.SendMonitorMessage "(MSG3 " & percent & "%)"
        pMonitor.SendMonitorMessage "(PROGRESS " & percent & ")"
        LastPercent = percent
      End If
    End If
    
    RowStart = RowEnd + 1
    RowEnd = InStr(RowStart, AllStations, vbCr)
    If RowEnd = 0 Then RowEnd = LenAllStations + 1
    If RowEnd > RowStart + 2 Then
      ThisRow = Mid(AllStations, RowStart, RowEnd - RowStart)
      If asc(ThisRow) < 32 Then ThisRow = Mid(ThisRow, 2)
      For FieldNumber = 1 To newDBF.NumFields
        newDBF.Value(FieldNumber) = StrSplit(ThisRow, vbTab, "")
      Next
      If IsNumeric(newDBF.Value(4)) And IsNumeric(newDBF.Value(4)) Then
        X = newDBF.Value(4)
        Y = newDBF.Value(3)
      Else
        X = 119 - (NumBadCoords * 0.5)
        Y = 25
        If pMonitorSet Then pMonitor.SendMonitorMessage "(MSG1 Warning: Non-numeric Lat/Long for station " & newDBF.Value(1) & " " & newDBF.Value(2) & " - placed south of US at Lat " & Y & ", Long " & X & ")"
        NumBadCoords = NumBadCoords + 1
      End If
      If X > uppX Then uppX = X Else If X < lowX Then lowX = X
      If Y > uppY Then uppY = Y Else If Y < lowY Then lowY = Y
      Call WriteShapePointAll(shpfile, RecordNumber, X, Y)
      Call WriteShapePointIndex(shxfile, RecordNumber)
    End If
  Wend
  If pMonitorSet Then pMonitor.SendMonitorMessage "(MSG1 Writing " & baseFilename & ".dbf)"

  newDBF.WriteDBF baseFilename & ".dbf"
  newDBF.Clear
  Set newDBF = Nothing
  
  FileLength = 50 + RecordNumber * 14
  Call WriteShapeHeader(shpfile, FileLength, ShapeType, lowX, lowY, uppX, uppY)
  Close shpfile

  FileLength = 50 + RecordNumber * 4
  Call WriteShapeHeader(shxfile, FileLength, ShapeType, lowX, lowY, uppX, uppY)
  Close shxfile
  If pMonitorSet Then pMonitor.SendMonitorMessage "(CLOSE)"
End Function
