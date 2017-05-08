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
'Copyright 2000 by AQUA TERRA Consultants
Implements ATCclsTserFile

Private pFileExt As String
Private pFilePath As String
Private pErrorDescription As String
Private pMonitor As Object
Private pMonitorSet As Boolean
Private pData As Collection
Private pDates As ATCData.ATCclsTserDate
Private pJulianOffset As Double

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
  Dim i As Long
  Dim valStart As Integer, valWidth As Integer
  Dim lValues() As Single
  curFilename = dataObject.Attrib("Filename")
  If Len(curFilename) > 0 Then
    If Len(Dir(curFilename)) > 0 Then
      If pMonitorSet Then
        pMonitor.SendMonitorMessage "(OPEN Reading EFDC output file " & curFilename & ")"
        pMonitor.SendMonitorMessage "(BUTTOFF CANCEL)"
        pMonitor.SendMonitorMessage "(BUTTOFF PAUSE)"
      End If
      inFile = FreeFile(0)
      Open curFilename For Input As #inFile
      Line Input #inFile, buf  ' ignore line 1
      Line Input #inFile, buf  ' AT LOCATION  cell__XXX
      Line Input #inFile, buf  ' CELL I,J =
      Line Input #inFile, buf  ' TIME IN FIRST COLUMN HAS UNITS OF DAYS
      valWidth = 13
      valStart = 1 + CInt(dataObject.Attrib("OutCol")) * valWidth
      NVALS = pDates.Summary.NVALS
      ReDim lValues(NVALS)
      For i = 1 To NVALS
        Line Input #inFile, buf
        lValues(i) = Mid(buf, valStart, valWidth)
      Next
      Close #inFile
      dataObject.Values = lValues
      If pMonitorSet Then pMonitor.SendMonitorMessage "(CLOSE)"
    End If
  End If
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
  pJulianOffset = MJD(1990, 1, 1)
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
  ATCclsTserFile_FileName = pFilePath
End Property

Private Property Let ATCclsTserFile_FileName(ByVal NewValue$)
  If Len(Dir(NewValue)) = 0 Then
    pErrorDescription = "FileName '" & NewValue & "' not found"
  Else
    pFilePath = PathNameOnly(NewValue)
    If Len(pFilePath) < 1 Then
      pFilePath = CurDir
    Else
      ChDir pFilePath
    End If
    pFilePath = pFilePath & "\"
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
  Dim slashpos As Long
  Dim Scenario As String, Location As String
  Dim CellI As String, CellJ As String
  Dim curFilename As String
  Dim inFile As Integer
  Dim buf As String
  Dim nextId As Long
  Dim OutCol As Long
  Dim dateStr As String
  Dim datevals() As Double
  Dim nDateVals As Long
  Dim dimDateVals As Long
  nextId = 1
  
  Set pDates = Nothing
  Set pDates = New ATCclsTserDate
  
  If pMonitorSet Then
    pMonitor.SendMonitorMessage "(OPEN Scanning for EFDC output files in " & pFilePath & ")"
    pMonitor.SendMonitorMessage "(BUTTOFF CANCEL)"
    pMonitor.SendMonitorMessage "(BUTTOFF PAUSE)"
  End If
  slashpos = InStrRev(pFilePath, "\", Len(pFilePath) - 1)
  If slashpos = 0 Then
    Scenario = pFilePath
  Else
    Scenario = Mid(pFilePath, slashpos + 1)
  End If
  Scenario = Left(Scenario, Len(Scenario) - 1) 'Trim \ from end
  
  inFile = FreeFile(0)
  curFilename = Dir("*.OUT")
  While Len(curFilename) > 0
    pMonitor.SendMonitorMessage "(MSG1 Opening " & curFilename & ")"
    Open curFilename For Input As #inFile
    Line Input #inFile, buf  ' ignore line 1
    Line Input #inFile, buf  ' AT LOCATION  cell__XXX
    Location = Trim(Mid(buf, 21))
    While Left(Location, 1) = "0"
      Location = Mid(Location, 2)
    Wend
    Line Input #inFile, buf  ' CELL I,J =
    CellI = Trim(Mid(buf, 13, 5))
    CellJ = Trim(Mid(buf, 18, 5))
    OutCol = 1
    Select Case UCase(Left(FilenameOnly(curFilename), 5))
      Case "Q3DTS"
        GoSub NewData: lData.Header.con = "Q3DTS Vol (m^3/s)"
      Case "SELTS"
        GoSub NewData: lData.Header.con = "SELTS1 Surface Elevation (m)"
        GoSub NewData: lData.Header.con = "SELTS2 Depth (m)"
        GoSub NewData: lData.Header.con = "SELTS3 (m)"
      Case "UVTTS"
        GoSub NewData: lData.Header.con = "UVTTS1 U (m^3/s)"
        GoSub NewData: lData.Header.con = "UVTTS2 V (m^3/s)"
      Case "UVETS"
        GoSub NewData: lData.Header.con = "UVETS1 E (cm/s)"
        GoSub NewData: lData.Header.con = "UVETS2 N (cm/s)"
        GoSub NewData: lData.Header.con = "UVETS3 Bot Tau (cm/s)^2"
        GoSub NewData: lData.Header.con = "UVETS4"
        GoSub NewData: lData.Header.con = "UVETS5"
        GoSub NewData: lData.Header.con = "UVETS6"
        GoSub NewData: lData.Header.con = "UVETS7"
        GoSub NewData: lData.Header.con = "UVETS8"
    End Select
    If dimDateVals = 0 Then 'populate dates
      Line Input #inFile, buf  ' TIME IN FIRST COLUMN HAS UNITS OF DAYS
      dimDateVals = 100
      ReDim datevals(dimDateVals)
      While Not EOF(inFile)
        Line Input #inFile, buf
        If Len(buf) > 0 Then
          dateStr = Trim(Left(buf, 13))
          If IsNumeric(dateStr) Then
            nDateVals = nDateVals + 1
            If nDateVals > dimDateVals Then
              dimDateVals = dimDateVals * 2
              ReDim Preserve datevals(dimDateVals)
            End If
            datevals(nDateVals) = CDbl(dateStr) + pJulianOffset
          End If
        End If
      Wend
      ReDim Preserve datevals(nDateVals)
      pDates.Values = datevals
      pDates.calcSummary
    End If
    Close #inFile
    curFilename = Dir
  Wend
CloseMonitor:
  If pMonitorSet Then
    pMonitor.SendMonitorMessage "(CLOSE)"
    pMonitor.SendMonitorMessage "(BUTTON CANCEL)"
    pMonitor.SendMonitorMessage "(BUTTON PAUSE)"
  End If
  Unload frmGenPLTInit
  Exit Sub

NewData:
  Set lData = New ATCclsTserData
  Set lData.Dates = pDates
  Set lData.File = Me
  pData.Add lData
  lData.Header.Sen = Scenario
  lData.Header.loc = Location
  lData.Header.id = nextId
  lData.AttribSet "Filename", curFilename
  lData.AttribSet "CellI", CellI
  lData.AttribSet "CellJ", CellJ
  lData.AttribSet "OutCol", CStr(OutCol)
  OutCol = OutCol + 1
  nextId = nextId + 1
  Return

Errhand:
  MsgBox "Error reading EFDC file " & curFilename & vbCr & err.Description, vbOKOnly, "EFDC"
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

