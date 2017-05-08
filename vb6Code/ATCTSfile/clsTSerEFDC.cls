VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsTSerEFDC"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'Copyright 2002 by AQUA TERRA Consultants
Implements ATCclsTserFile

Private pFileExt As String
Private pFileName As String
Private pErrorDescription As String
Private pMonitor As Object
Private pMonitorSet As Boolean
Private pData As Collection
Private pDates As ATCData.ATCclsTserDate
Private pJulianOffset As Double
Private ColumnStart() As Long, ColumnWidth() As Long

Private Property Get ATCclsTserFile_AvailableAttributes() As Collection
  Set ATCclsTserFile_AvailableAttributes = New Collection
End Property

Private Sub ATCclsTserFile_clear()
  Set pData = Nothing
  Set pDates = Nothing
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

Private Property Let ATCclsTserFile_HelpFilename(ByVal NewValue As String)
  App.HelpFile = NewValue
End Property

Private Property Get ATCclsTserFile_Label() As String
  ATCclsTserFile_Label = "EFDC"
End Property

Private Sub ATCclsTserFile_readData(dataObject As ATCclsTserData)
  Dim curFilename As String
  Dim inFile As Integer
  Dim buf As String
  Dim NVALS As Long
  Dim loc As String
  Dim i As Long
  Dim Column As Long
  Dim valStart As Integer, valWidth As Integer
  Dim lValues() As Single
  Dim Location As String
  Dim valString As String
  
  On Error GoTo Errhand
  
  If pMonitorSet Then
    pMonitor.SendMonitorMessage "(OPEN Reading EFDC output file " & pFileName & ")"
    pMonitor.SendMonitorMessage "(BUTTOFF CANCEL)"
    pMonitor.SendMonitorMessage "(BUTTOFF PAUSE)"
  End If
  inFile = FreeFile(0)
  Open pFileName For Input As #inFile
  Column = CInt(dataObject.Attrib("OutputColumn", "1"))
  valWidth = ColumnWidth(Column)
  valStart = ColumnStart(Column)
  loc = dataObject.Header.loc
  NVALS = pDates.Summary.NVALS
  ReDim lValues(NVALS)
  i = 0
  If pMonitorSet Then pMonitor.SendMonitorMessage "(MSG1 Reading " & NVALS & " values for " & dataObject.Header.con & " at " & loc & ")"
  Do While Not EOF(inFile)
    Line Input #inFile, buf
    If Mid(buf, 6, 1) <> " " Then '    465.5000 TIME IN DAYS
      i = i + 1
      Line Input #inFile, buf 'Skip column headers
    Else
      Location = Trim(Left(buf, 5))
      If Location = loc Then
        valString = Trim(Mid(buf, valStart, valWidth))
        If IsNumeric(valString) Then
          lValues(i) = CSng(valString)
        Else
          lValues(i) = -999
          If pMonitorSet Then pMonitor.SendMonitorMessage "(MSG2 Non-numeric " & dataObject.Header.con & " at " & loc & ", time step " & i & ", setting value to -999)"
        End If
        If i = NVALS Then Exit Do 'Just found last value, skip reading to end of file
      End If
    End If
  Loop
  Close #inFile
  dataObject.Values = lValues
  If pMonitorSet Then pMonitor.SendMonitorMessage "(CLOSE)"

Errhand:
  pErrorDescription = "readData: buf = " & buf & " i = " & i & " description = " & err.Description
  If pMonitorSet Then pMonitor.SendMonitorMessage "(MSG2 " & pErrorDescription & ")"
  
End Sub

Private Sub ATCclsTserFile_refresh()

End Sub

Private Function ATCclsTserFile_writeDataHeader(dataObject As ATCclsTserData, Optional oldHeader As ATTimSerDataHeader) As Boolean
  pErrorDescription = "Unable to Write a Data Header for Class EFDC"
  ATCclsTserFile_writeDataHeader = False
End Function

Private Sub Class_Initialize()
  pFileExt = "EFDC Files (*.OUT)|*.OUT|All Files (*.*)|*.*"
  pErrorDescription = ""
  pMonitorSet = False
  Set pData = New Collection
  'pJulianOffset = MJD(2000, 1, 1)
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
  ATCclsTserFile_FileName = pFileName
End Property

Private Property Let ATCclsTserFile_FileName(ByVal NewValue$)
  If Len(Dir(NewValue)) = 0 Then
    pErrorDescription = "FileName '" & NewValue & "' not found"
  Else
    pFileName = NewValue
    If InStr(NewValue, "\") = 0 Then pFileName = CurDir & "\" & NewValue
    LocateFilesReadDates
  End If
End Property

Private Property Get ATCclsTserFile_Description() As String
  ATCclsTserFile_Description = "EFDC Time Series Data Type"
End Property

Private Property Get ATCclsTserFile_DCnt() As Long
  ATCclsTserFile_DCnt = pData.Count
End Property

Private Sub LocateFilesReadDates()
  Dim lData As ATCData.ATCclsTserData
  Dim spacepos As Long
  Dim Scenario As String, Location As String
  'Dim CellI As String, CellJ As String
  Dim LSU As String, lsd As String, LFU As String, LFD As String, LCU As String, LCD As String, BEL As String
  Dim curFilename As String
  Dim inFile As Integer
  Dim buf As String
  Dim ColumnHeader() As String, Col As Long, Cols As Long
  Dim bufLen As Long
  Dim nextId As Long
  Dim OutCol As Long
  Dim dateStr As String
  Dim datevals() As Double
  Dim nDateVals As Long
  Dim dimDateVals As Long
  
  On Error GoTo Errhand1
  
  nextId = 1
  Set pDates = Nothing
  Set pDates = New ATCclsTserDate
  dimDateVals = 100
  ReDim datevals(dimDateVals)
  
  Scenario = FilenameOnly(pFileName)
  
  If pMonitorSet Then
    pMonitor.SendMonitorMessage "(OPEN Scanning EFDC output in " & pFileName & ")"
    pMonitor.SendMonitorMessage "(BUTTOFF CANCEL)"
    pMonitor.SendMonitorMessage "(BUTTOFF PAUSE)"
  End If
  
  inFile = FreeFile(0)
  Open pFileName For Input As #inFile
  Line Input #inFile, buf '    465.5000 TIME IN DAYS
  While Not EOF(inFile)
    dateStr = Trim(Left(buf, 13))
    If pMonitorSet Then pMonitor.SendMonitorMessage "(MSG2 Read time " & dateStr & ")"
    If IsNumeric(dateStr) Then
      nDateVals = nDateVals + 1
      If nDateVals > dimDateVals Then
        dimDateVals = dimDateVals * 2
        ReDim Preserve datevals(dimDateVals)
      End If
      If pJulianOffset = 0 Then
        If IsNumeric(Mid(buf, 15, 4)) Then
          pJulianOffset = MJD(CLng(Mid(buf, 15, 4)), 1, 1)
        End If
      End If
      datevals(nDateVals) = CDbl(dateStr) + pJulianOffset
    Else
      If pMonitorSet Then pMonitor.SendMonitorMessage "(MSG2 Error - non-numeric date: " & Trim(buf) & ")"
    End If
                            
    Line Input #inFile, buf  '  L1D   LSU  LSD  LFU LFD  LCU  LCD      X         Y        BEL     DEP     AREA     WPER    BSRF    VEL        Q       SAL     TEM       SED      SND1      SND2     SEDB        SNDB1       SNDB2      TAUBED
                          'was  L1D    L    I    J      X         Y        BEL     DEP     AREA     WPER    BSRF    VEL        Q       SAL     TEM       SED      SND1      SND2     SEDB        SNDB1       SNDB2      TAUBED
    If Cols < 1 Then
      ReDim ColumnHeader(50)
      While Len(buf) > 0
        Cols = Cols + 1
        ColumnHeader(Cols) = StrRetRem(buf)
      Wend
      ColumnHeader(1) = "ID"
      ReDim Preserve ColumnHeader(Cols)
      ReDim ColumnStart(Cols)
      ReDim ColumnWidth(Cols)
      Line Input #inFile, buf '    1    2    0    0    0    0    0    453.15   4401.44    3.05    0.89     22.28   26.78   25.00   -0.56    -12.476    0.00   20.00      9.97     46.01    112.23   0.100E+06   0.145E+06   0.923E+05   0.610E+01
                           'was    1    2    2    2  20000.00  20000.00   -3.05    4.15    569.13  159.58  159.87    0.00      0.000   29.17   20.00      0.30      0.00      0.00   0.100E+06   0.100E+06   0.100E+06   0.962E-03
      bufLen = Len(buf)
      spacepos = 1
      For Col = 1 To Cols
        ColumnStart(Col) = spacepos
        While Mid(buf, spacepos, 1) = " "
          spacepos = spacepos + 1
        Wend
        Do While Mid(buf, spacepos, 1) <> " "
          spacepos = spacepos + 1
          If spacepos > bufLen Then Exit Do
        Loop
        ColumnWidth(Col) = spacepos - ColumnStart(Col)
      Next
    Else
      Line Input #inFile, buf  '    1    2    2    2  20000.00  20000.00   -3.05    4.15    569.13  159.58  159.87    0.00      0.000   29.17   20.00      0.30      0.00      0.00   0.100E+06   0.100E+06   0.100E+06   0.962E-03
    End If
    
    If pData.Count = 0 Then 'Add grid of datasets (columns x locations)
      If pMonitorSet Then pMonitor.SendMonitorMessage "(MSG1 Creating datasets for " & (Cols - 9) & " constituents)"
      While Mid(buf, 6, 1) = " " And Not EOF(inFile)
        Location = Trim(Left(buf, 5))
        LSU = Trim(Mid(buf, 6, 5))
        lsd = Trim(Mid(buf, 11, 5))
        LFU = Trim(Mid(buf, 16, 5))
        LFD = Trim(Mid(buf, 21, 5))
        LCU = Trim(Mid(buf, 26, 5))
        LCD = Trim(Mid(buf, 31, 5))
        'BEL = Trim(Mid(buf, 57, 7))
        'CellI = Trim(Mid(buf, 11, 5))
        'CellJ = Trim(Mid(buf, 16, 5))
        For Col = 10 To Cols 'skip 9 columns of non-varying data in this file (!)
          Set lData = New ATCclsTserData
          Set lData.Dates = pDates
          Set lData.File = Me
          pData.Add lData
          lData.Header.Sen = Scenario
          lData.Header.loc = Location
          lData.Header.con = ColumnHeader(Col)
          lData.Header.id = nextId
          lData.AttribSet "LSU", LSU
          lData.AttribSet "LSD", lsd
          lData.AttribSet "LFU", LFU
          lData.AttribSet "LFD", LFD
          lData.AttribSet "LCU", LCU
          lData.AttribSet "LCD", LCD
          'lData.AttribSet "BEL", BEL
          lData.AttribSet "OutputColumn", CStr(Col)
          nextId = nextId + 1
          Set lData = Nothing
        Next
        Line Input #inFile, buf  '    1    2    2    2  20000.00  20000.00   -3.05    4.15    569.13  159.58  159.87    0.00      0.000   29.17   20.00      0.30      0.00      0.00   0.100E+06   0.100E+06   0.100E+06   0.962E-03
      Wend
      If pMonitorSet Then pMonitor.SendMonitorMessage "(MSG1 Created " & (nextId - 1) & " datasets for " & (nextId - 1) / (Cols - 9) & " locations and " & (Cols - 9) & " constituents)"
    Else
      While Mid(buf, 6, 1) = " " And Not EOF(inFile)
        Line Input #inFile, buf  '    1    2    2    2  20000.00  20000.00   -3.05    4.15    569.13  159.58  159.87    0.00      0.000   29.17   20.00      0.30      0.00      0.00   0.100E+06   0.100E+06   0.100E+06   0.962E-03
      Wend
    End If
  Wend
CloseInfile:
  On Error GoTo Errhand2
  Close #inFile

  ReDim Preserve datevals(nDateVals)
  pDates.Values = datevals
  pDates.calcSummary


CloseMonitor:
  If pMonitorSet Then
    pMonitor.SendMonitorMessage "(CLOSE)"
    pMonitor.SendMonitorMessage "(BUTTON CANCEL)"
    pMonitor.SendMonitorMessage "(BUTTON PAUSE)"
  End If
  Unload frmGenPLTInit
  Exit Sub

Errhand1:
  MsgBox "Error reading EFDC file " & curFilename & vbCr & err.Description, vbOKOnly, "EFDC"
  err.Clear
  GoTo CloseInfile

Errhand2:
  MsgBox "Error reading EFDC file " & curFilename & vbCr & err.Description, vbOKOnly, "EFDC"
  err.Clear
  GoTo CloseMonitor
End Sub

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

Private Function ATCclsTserFile_SaveAs(Filename As String) As Boolean
  ATCclsTserFile_SaveAs = False
  pErrorDescription = "SaveAs not supported for " & ATCclsTserFile_Description
End Function

