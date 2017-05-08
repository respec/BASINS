VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsTSerSWATDbf"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'Copyright 2002 by AQUA TERRA Consultants

Implements ATCclsTserFile

Private pFileExt As String
Private pErrorDescription As String
Private pMonitor As Object
Private pMonitorSet As Boolean
Private pData As Collection
Private pDates As ATCData.ATCclsTserDate
Private pRecordsPerTimestep As Long
Private pDatedRecords As Long
Private pDbfFile As clsATCTable ' New clsDBF
Private pFilename As String
Private pPathName As String
Private pDbfFileName As String
Private pTableName As String

Private Property Get ATCclsTserFile_AvailableAttributes() As Collection
  Set ATCclsTserFile_AvailableAttributes = New Collection
End Property

Private Sub ATCclsTserFile_clear()
  Set pData = Nothing
  Set pDates = Nothing
  Set pDbfFile = Nothing
End Sub

Private Property Get ATCclsTserFile_Data(index As Long) As ATCData.ATCclsTserData
  Set ATCclsTserFile_Data = pData(index)
End Property

Private Property Get ATCclsTserFile_DataCount() As Long
  ATCclsTserFile_DataCount = pData.Count
End Property

Private Property Get ATCclsTserfile_DataCollection() As Collection
  Set ATCclsTserfile_DataCollection = pData
End Property

Private Property Get ATCclsTserFile_FileUnit() As Long

End Property

Private Property Let ATCclsTserFile_HelpFilename(ByVal newValue As String)
  App.HelpFile = newValue
End Property

Private Property Get ATCclsTserFile_Label() As String
  ATCclsTserFile_Label = "SWATDBF"
End Property

Private Sub ATCclsTserFile_readData(dataObject As ATCclsTserData)
  Dim recNum As Long, recOff As Long
  Dim datarray() As Single
  Dim fldNum As Long, id As Long
  Dim dateIndex As Long
  Dim sValue As String
  
  id = dataObject.Header.id
  fldNum = id Mod 1000
  recOff = 1 + ((id - fldNum) / 1000)
  
  If pMonitorSet Then pMonitor.dbg "Reading data from " & pFilename & " for " & id
  
  ReDim datarray(pDatedRecords / pRecordsPerTimestep)
    
  dateIndex = 1
  recNum = recOff
  While recNum <= pDatedRecords
    pDbfFile.CurrentRecord = recNum
    sValue = pDbfFile.Value(fldNum)
    If IsNumeric(sValue) Then
      datarray(dateIndex) = sValue
    Else
      datarray(dateIndex) = -999
    End If
    recNum = recNum + pRecordsPerTimestep
    dateIndex = dateIndex + 1
  Wend
  
  dataObject.Values = datarray
  dataObject.calcSummary
  
End Sub

Private Sub ATCclsTserFile_refresh()

End Sub

Private Function ATCclsTserFile_writeDataHeader(dataObject As ATCclsTserData, Optional oldHeader As ATTimSerDataHeader) As Boolean
  pErrorDescription = "Unable to Write a Data Header for Class SWATDbf"
  ATCclsTserFile_writeDataHeader = False
End Function

Private Sub Class_Initialize()
  pFilename = "<unknown>"
  pFileExt = "SWAT DBF Files (*.dbf)|*.dbf|All Files (*.*)|*.*"
  pErrorDescription = ""
  pMonitorSet = False
  Set pData = New Collection
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

Private Property Get ATCclsTserFile_FileName() As String
  ATCclsTserFile_FileName = pFilename
End Property

Private Property Let ATCclsTserFile_FileName(ByVal newValue$)
  If Len(Dir(newValue)) = 0 Then
    pErrorDescription = "FileName '" & newValue & "' not found"
  Else
    pFilename = newValue
    pPathName = PathNameOnly(newValue)
    pTableName = FilenameOnly(newValue)
    pDbfFileName = FilenameNoExt(pFilename) & ".dbf"
    If Len(pPathName) = 0 Then pPathName = CurDir
    DBFRead
  End If
End Property

Private Property Get ATCclsTserFile_DCnt() As Long
  ATCclsTserFile_DCnt = pData.Count
End Property

Private Property Get ATCclsTserFile_Description() As String
  ATCclsTserFile_Description = "SWAT DBF Time Series Data Type"
End Property

Private Sub DBFRead()
  Dim tempdate&(6), NewDate&(6)
  Dim tsIndex As Long
  Dim HasAverage As Boolean
  Dim LocName As String, ConsSuffix As String
  Dim fldNum&, u&, dif#, l&, v#
  Dim id&
  Dim Scen$, odate$, ndate$
  Dim lData As ATCData.ATCclsTserData
  Dim DateSum As ATCData.ATTimSerDateSummary
  Dim DateField As Long        'field for date values
  Dim LocnField As Long        'field containing location info
  Dim LocnFieldName As String  'field containing location info
  Dim LandUseField As Long
  Dim SoilField As Long
  Dim nConsFields As Long
  Dim ConsName() As String
  Dim ConsField() As Long
  Dim lCurrentRecord As Long
  
  If pMonitorSet Then
     pMonitor.SendMonitorMessage "(OPEN SWAT DBF File)"
     pMonitor.SendMonitorMessage "(MSG1 " & pFilename & ")"
  End If
    
  Set pDbfFile = New clsATCTableDBF
  
  If InStr(UCase(pPathName), "\SCEN") > 0 Then
    Scen = Mid(pPathName, InStrRev(pPathName, "\") + 1)
  Else
    Scen = "BASE"
  End If
  If UCase(Scen) = "OBSERVED" Then Scen = "OBSERVED"
  Scen = Scen & ":" & pTableName
  While IsNumeric(Right(Scen, 1))
    Scen = Mid(Scen, 1, Len(Scen) - 1)
    If Scen = "" Then Scen = "Untitled"
  Wend
  
  pDbfFile.OpenFile pDbfFileName
    
  Set pDates = New ATCData.ATCclsTserDate
  Set pDates.File = Me
    
  DateField = -1
  LocnField = -1
  LandUseField = -1
  SoilField = -1
  
  nConsFields = 0
  ReDim ConsName(pDbfFile.NumFields)
  ReDim ConsField(pDbfFile.NumFields)
  For fldNum = 1 To pDbfFile.NumFields  'look for date and location fields
    Select Case UCase(pDbfFile.FieldName(fldNum))
      Case "DATE":            DateField = fldNum
      Case "HUC", "SUBBASIN": LocnField = fldNum 'location - should not be hard coded
                              LocnFieldName = pDbfFile.FieldName(fldNum) 'pRS.Fields(fldNum).Name
      Case "LANDUSE":         LandUseField = fldNum
      Case "SOIL":            SoilField = fldNum
      'Case "HRU":            'sub-subbasin, we ignore it for now
      Case Else
                              nConsFields = nConsFields + 1
                              ConsName(nConsFields) = pDbfFile.FieldName(fldNum) 'pRS.Fields(fldNum).Name
                              ConsField(nConsFields) = fldNum
    End Select
  Next

  If DateField = -1 Then
    If MsgBox("No Date Field Defined in File " & pFilename & ", Default Date to 1900?", vbYesNo, "SWAT DBF Read") = vbNo Then
      pErrorDescription = "No Date Field Defined in File " & pFilename
    Else
      DateField = -2 'special for dummy
      ndate = "1900"
      With DateSum
        .SJDay = parseTAMUDate(ndate, True)
        .EJDay = .SJDay
        .CIntvl = True
        .NVALS = 1
        .ts = 1
        .Tu = TUYear
        .Intvl = 0
      End With
    End If
  Else
    ndate = pDbfFile.Value(DateField)
    DateSum.SJDay = parseTAMUDate(ndate, True)
  End If
  odate = ndate
  If DateField <> -1 Then
    HasAverage = False
    If DateField > -1 Then
      pDbfFile.CurrentRecord = pDbfFile.NumRecords
      If Not IsNumeric(pDbfFile.Value(DateField)) Then HasAverage = True  'Averages at end of table
    End If
    
    lCurrentRecord = 1 'cant used pDbfFile.CurrentRecord because it can't be set out of range
    pDbfFile.CurrentRecord = lCurrentRecord
        
    Do Until lCurrentRecord = pDbfFile.NumRecords Or odate <> ndate 'scan data for first time step
      LocName = pDbfFile.Value(LocnField)
      If LandUseField >= 0 Then ConsSuffix = ":" & pDbfFile.Value(LandUseField) Else ConsSuffix = ""
      If SoilField >= 0 Then ConsSuffix = ConsSuffix & ":" & pDbfFile.Value(SoilField)
      For fldNum = 1 To nConsFields
        UniqueDBFtimser Scen, LocName, ConsName(fldNum) & ConsSuffix, ConsField(fldNum), pDbfFile.CurrentRecord
      Next
      lCurrentRecord = lCurrentRecord + 1
      pDbfFile.CurrentRecord = lCurrentRecord
      If DateField > -1 Then ndate = pDbfFile.Value(DateField)
    Loop
    
    pRecordsPerTimestep = pDbfFile.CurrentRecord - 1
    pDatedRecords = pDbfFile.NumRecords
    If HasAverage Then
      pDatedRecords = pDatedRecords - pRecordsPerTimestep
    End If
    
    With DateSum
      .CIntvl = True
      If IsNumeric(ndate) Then
        .Intvl = parseTAMUDate(ndate, True) - .SJDay
      ElseIf Len(odate) = 4 Then 'single year
        .Intvl = 365
      ElseIf Len(odate) = 6 Then 'single month
        .Intvl = 31
      Else
        .Intvl = 1
      End If
      .NVALS = pDatedRecords / pRecordsPerTimestep
      .ts = 1
      Select Case .Intvl
        Case Is < JulianMinute - JulianSecond: .Tu = TUSecond
        Case Is < JulianHour - JulianMinute:   .Tu = TUMinute
        Case Is < JulianHour * 2:              .Tu = TUHour
        Case Is < 1 + JulianHour:              .Tu = TUDay
        Case Is < 32:                          .Tu = TUMonth
        Case Is < 367:                         .Tu = TUYear
        Case Else:                             .Tu = TUCentury
      End Select
      
      If .Intvl < 28 Then '
        .EJDay = .SJDay + .Intvl * .NVALS
      Else 'special case for long intervals
        Call J2Date(.SJDay, tempdate)
        Call TIMADD(tempdate, .Tu, .ts, .NVALS, NewDate)
        .EJDay = Date2J(NewDate)
      End If
      
    End With
    pDates.Summary = DateSum
  End If
    
  If pMonitorSet Then pMonitor.SendMonitorMessage "(CLOSE SWAT DBF File)"
End Sub

Private Function parseTAMUDate(s$, StartOfInterval As Boolean) As Double
  'assume mean values at end of interval
  Dim j#, yr&, mn&, dy&
  If IsNumeric(s) Then
    If Len(s) = 8 Then 'day
      mn = Left(s, 2) 'Mid(s, 3, 2)
      dy = Mid(s, 3, 2)
      yr = Mid(s, 5, 4)
      If StartOfInterval Then
        j = jday(yr, mn, dy, 0, 0, 0)
      Else
        j = jday(yr, mn, dy, 24, 0, 0)
      End If
    ElseIf Len(s) = 6 Then 'month
      yr = Mid(s, 3, 4)
      mn = Left(s, 2)
      If StartOfInterval Then
        j = jday(yr, mn, 1, 0, 0, 0)
      Else
        j = jday(yr, mn, daymon(yr, mn), 24, 0, 0)
      End If
    Else 'year
      yr = Left(s, 4)
      If StartOfInterval Then
        j = jday(yr, 1, 1, 0, 0, 0)
      Else
        j = jday(yr, 12, 31, 24, 0, 0)
      End If
    End If
  Else
    j = 0
  End If
  
  parseTAMUDate = j
  
End Function

Private Function UniqueDBFtimser(Scen As String, LocName As String, _
                                 ConsName As String, ConsField As Long, _
                                 RecordNumber As Long) As Long
  Dim ds As ATCData.ATCclsTserData
  Dim lUnits As String
  Dim lFileType As String, vFileType As Variant, aFileTypes As Variant, lFileTypePos As Long
  Dim lParmName As String, lParmDelim As Long
  
  Set ds = New ATCclsTserData
  Set ds.Dates = pDates
  Set ds.File = Me
  With ds.Header
    .con = ConsName
    .loc = LocName
    .Sen = Scen
    .id = (RecordNumber - 1) * 1000 + ConsField
    .desc = "SWATDbf:" & Scen & ":" & LocName & ":" & ConsName
  End With
  lParmDelim = InStr(ConsName, ":")
  If lParmDelim > 0 Then
    lParmName = Left(ConsName, lParmDelim - 1)
  Else
    lParmName = ConsName
  End If
  aFileTypes = Array("rch", "bsb", "sbs")
  For Each vFileType In aFileTypes
    lFileTypePos = InStr(Scen, vFileType)
    If lFileTypePos > 0 Then
      lFileType = vFileType
      Exit For
    End If
  Next
  lUnits = GetParameterUnits(lParmName, "SwatDBFParameter", lFileType)
  ds.AttribSet "Units", lUnits, UnitsAttributeDefinition 'FIXME Should this be editable if it was already set?
  pData.Add ds
  UniqueDBFtimser = pData.Count
End Function

Private Function ATCclsTserFile_AddTimSer(t As ATCclsTserData, Optional ExistAction& = 0) As Boolean
  ATCclsTserFile_AddTimSer = False
  pErrorDescription = "AddTimSer not supported for " & ATCclsTserFile_Description
End Function

Private Function ATCclsTserFile_RemoveTimSer(t As ATCclsTserData) As Boolean
  ATCclsTserFile_RemoveTimSer = False
  pErrorDescription = "RemoveTimSer not supported for " & ATCclsTserFile_Description
End Function

Private Function ATCclsTserFile_RewriteTimSer(t As ATCclsTserData) As Boolean
  ATCclsTserFile_RewriteTimSer = False
  pErrorDescription = "RewriteTimSer not supported for " & ATCclsTserFile_Description
End Function

Private Function ATCclsTserFile_SaveAs(filename As String) As Boolean
  ATCclsTserFile_SaveAs = False
  pErrorDescription = "SaveAs not supported for " & ATCclsTserFile_Description
End Function
