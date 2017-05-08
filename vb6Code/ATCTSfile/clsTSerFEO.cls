VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsTSerFEO"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'Copyright 2002 by AQUA TERRA Consultants
Implements ATCclsTserFile

Private pFileBytes() As Byte
Private pFileInt() As Integer
Private pFileSng() As Single

Private pFilename As String
Private pFileExt As String
Private pErrorDescription As String
Private pMonitor As Object
Private pMonitorSet As Boolean
Private pData As Collection
Private pDates As ATCData.ATCclsTserDate
Private pDatesPopulated As Boolean
Private XTIOFF As Long

Private Type FeqFtf
    Bindex As Long
    Lindex As Long
    HA As Long
    NTab As Long
    Type As Long
    LSta As Long
    Shift As Long
    Off As Long
End Type

Private Type FeqLocDir
    Location As String
    Constit As Collection
    Label As String
    LocClass As String
    Branch As Long
    NodeID As String
    Station As Single
    Invert As Single
    UserID As String
    FtabIndex As Long
    FtabTyp As Long
    SeriesOffset As Long
    Easting As Double
    Northing As Double
End Type

Private Type FeqDataFile
    version As String ' FEQ version
    Scenario As String
    NameFeo As String ' output file name
    NameTsd As String ' timeseries file name
    RecLen As Long ' timeseries file record length
    NameFtf As String ' function table file name
    ItemPerRec As Long ' items per record
    LeftItemCnt As Long ' left over item count
    NumbFullRec As Long ' number of full records
    LocDir() As FeqLocDir
    LocCount As Long ' count of locations
    Term As String ' termination status
    ftf() As FeqFtf
End Type
Private f As FeqDataFile

Private Property Get ATCclsTserFile_AvailableAttributes() As Collection
  Set ATCclsTserFile_AvailableAttributes = New Collection
End Property

Private Sub ATCclsTserFile_clear()
  Set pData = Nothing
  Set pDates = Nothing
  ReDim f.LocDir(0)
  'ReDim f.JDFlg(0)
  'ReDim f.JDay(0)
  'ReDim f.flags(0)
End Sub

Private Property Get ATCclsTserFile_Data(index As Long) As ATCData.ATCclsTserData
  Set ATCclsTserFile_Data = pData(index)
End Property

Private Property Get ATCclsTserFile_DataCount() As Long
  ATCclsTserFile_DataCount = pData.Count ' f.ConCount * f.LocCount
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
  ATCclsTserFile_Label = "FEO"
End Property

Private Sub ATCclsTserFile_readData(dataObject As ATCclsTserData)
  Call FillFeqTimser(dataObject)
End Sub

Private Sub ATCclsTserFile_refresh()

End Sub

Private Function ATCclsTserFile_writeDataHeader(dataObject As ATCclsTserData, Optional oldHeader As ATTimSerDataHeader) As Boolean
  pErrorDescription = "Unable to Write a Data Header for Class FEO"
  ATCclsTserFile_writeDataHeader = False
End Function

Private Sub Class_Initialize()
  f.NameFeo = "<unknown>"
  pFileExt = "FEO Files (*.feo)|*.feo"
  pErrorDescription = ""
  pMonitorSet = False
  
  Set pData = Nothing
  Set pData = New Collection
  ReDim pFileBytes(0)
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
  ATCclsTserFile_FileName = f.NameFeo
End Property

Private Property Let ATCclsTserFile_FileName(ByVal newValue As String)
  If Len(Dir(newValue)) = 0 Then
    pErrorDescription = "FileName '" & newValue & "' not found"
  Else
    f.NameFeo = newValue
    Call FeoRead
  End If
End Property

Private Property Get ATCclsTserFile_DCnt() As Long ' dataset count
  If f.LocCount > 0 Then
    ATCclsTserFile_DCnt = pData.Count 'f.LocCount * f.ConCount
  Else
    ATCclsTserFile_DCnt = 0
  End If
End Property

Private Property Get ATCclsTserFile_Description() As String
  ATCclsTserFile_Description = "FEO Time Series Data Type"
End Property

Private Sub FeoRead()
  Dim jday() As Double  'julian day of values
  Dim jdflg() As Long
  Dim lData As ATCData.ATCclsTserData
  'Dim lDataHeader As ATCData.ATTimSerDataHeader
  Dim i%, j%, b%, s$, d#, l&, jdif#
  Dim LabelWidth As Long, NodeIdWidth As Long, UserIDWidth As Long, FieldStart As Long
  Dim datsum As ATTimSerDateSummary
  Dim path As String
  Dim progress As String
  Dim fileformat As Long
  
  LabelWidth = 8
  NodeIdWidth = 6
  UserIDWidth = 12 '8
  XTIOFF = 7
  fileformat = 0
  
  On Error GoTo error:
  
  If pMonitorSet Then
    pMonitor.SendMonitorMessage "(OPEN FEO File)"
    pMonitor.SendMonitorMessage "(BUTTOFF CANCEL)"
    pMonitor.SendMonitorMessage "(BUTTOFF PAUSE)"
    pMonitor.SendMonitorMessage "(MSG1 " & f.NameFeo & ")"
  End If
  
  progress = "i = FreeFile(0)"
  i = FreeFile(0)
  progress = "Open f.NameFeo For Input As #i"
  Open f.NameFeo For Input As #i
  progress = "path = PathNameOnly(f.NameFeo)"
  path = PathNameOnly(f.NameFeo)
  progress = "ChDriveDir " & path
  ChDriveDir path
  progress = "ReDim f.LocDir(0) "
  ReDim f.LocDir(0)
  
  Do While Not EOF(i) ' Loop until end of file.
    progress = "Line Input #i, s"
    Line Input #i, s
    Select Case Trim(s)
    
    Case "-VERSION":      Line Input #i, s
                          f.version = Trim(s)
    
    Case "-FILE_FORMAT":  Line Input #i, s
                          If IsNumeric(s) Then
                            fileformat = CLng(s)
                            If fileformat = 1 Or fileformat = 2 Then
                              LabelWidth = 16
                              NodeIdWidth = 7
                              UserIDWidth = 16
                              If fileformat = 1 Then
                                XTIOFF = 21
                              ElseIf fileformat = 2 Then
                                XTIOFF = 0
                              End If
                            ElseIf fileformat > 2 Then
                              MsgBox "This format (version " & s & ") is not recognized." & vbCr & "This program only knows about version 0 and 1.", vbCritical, "FEO Read Error"
                              Exit Sub
                            End If
                          End If
    
    Case "-FILES":        Line Input #i, s ': f.NameFeo = Trim(s)
                          f.Scenario = FilenameOnly(f.NameFeo)
                          Line Input #i, s: f.NameTsd = Trim(s)
                          Line Input #i, s: f.NameFtf = Trim(s)
    Case "-UNITS":        'nothing yet
    
    Case "-GATE_PUMP_STATUS":        'nothing yet
    
    Case "-FTF":          Line Input #i, s 'NUMBER_OF_FULL_RECORDS=
                          f.NumbFullRec = Right(s, 8)
                          Line Input #i, s 'ITEMS_PER_RECORD=
                          f.ItemPerRec = Right(s, 8)
                          Line Input #i, s 'LETOVER_ITEM_KNT=
                          f.LeftItemCnt = Right(s, 8)
                          If Len(Dir(f.NameFtf)) > 0 Then
                            If pMonitorSet Then pMonitor.SendMonitorMessage "(MSG2 Loading FTF File)"
                            progress = "Loading FTF: " & f.NameFtf
                            pFileBytes = WholeFileBytes(f.NameFtf)
                            If pMonitorSet Then pMonitor.SendMonitorMessage "(MSG2 Cleaning FTF File - " & UBound(pFileBytes) & " bytes)"
                            progress = "FtnUnFmtClean"
                            pFileBytes = FtnUnFmtClean(pFileBytes)
                            If pMonitorSet Then pMonitor.SendMonitorMessage "(MSG2 Scanning FTF File)"
                            progress = "FtfRead"
                            FtfRead
                          Else
                            ReDim pFileBytes(0)
                            pErrorDescription = "FTF file not found: " & f.NameFtf
                          End If
                          'Call F90_FILTAB(f.NameFtf, f.NumbFullRec, f.ItemPerRec, f.LeftItemCnt, Len(f.NameFtf))
                          '****^^^^^^^^^^***********
    Case "-TSDDIR":
      If pMonitorSet Then pMonitor.SendMonitorMessage "(MSG2 Reading Node Information in FEO TSDDIR)"
      Line Input #i, s 'First line of column labels
      Line Input #i, s 'Second line of labels
      Line Input #i, s 'Dashes separating labels from table
      b = 0
      j = 0
      While b >= 0
        Line Input #i, s
        If IsNumeric(Mid(s, LabelWidth + 9, 5)) Then
          b = Trim(Mid(s, LabelWidth + 9, 5))
        Else
          b = -1
        End If
        If b >= 0 Then
          With f.LocDir(j)
            .Branch = b
            FieldStart = 2
            
            .Label = Trim(Mid(s, FieldStart, LabelWidth))
            FieldStart = FieldStart + LabelWidth + 1
            
            .LocClass = Trim(Mid(s, FieldStart, 5))
            FieldStart = FieldStart + 12
            
            .NodeID = Trim(Mid(s, FieldStart, NodeIdWidth))
            FieldStart = FieldStart + NodeIdWidth + 1
            
            .Station = Trim(Mid(s, FieldStart, 15))
            FieldStart = FieldStart + 16
            
            .Invert = Trim(Mid(s, FieldStart, 10))
            FieldStart = FieldStart + 11
            
            .UserID = Trim(Mid(s, FieldStart, UserIDWidth))
            FieldStart = FieldStart + UserIDWidth + 1
            
            .FtabIndex = Trim(Mid(s, FieldStart, 8))
            FieldStart = FieldStart + 9
            
            'Call F90_TABTYP(.FtabIndex, .FtabTyp)
            .FtabTyp = FtabTyp(.FtabIndex)
            .SeriesOffset = Mid(s, FieldStart, 8)
            FieldStart = FieldStart + 9
            If Len(Trim(Mid(s, FieldStart, 12))) > 0 Then
              If IsNumeric(Trim(Mid(s, FieldStart, 12))) Then
                .Easting = CDbl(Trim(Mid(s, FieldStart, 12)))
              End If
            End If
            FieldStart = FieldStart + 13
            If Len(Trim(Mid(s, FieldStart, 12))) > 0 Then
              If IsNumeric(Trim(Mid(s, FieldStart, 12))) Then
                .Northing = CDbl(Trim(Mid(s, FieldStart, 12)))
              End If
            End If
            ' update later
            .Location = Trim(.NodeID)
            Set .Constit = New Collection
            .Constit.Add "FLOW"
            .Constit.Add "WSELEV"
            If .LocClass = "LPR" And fileformat <> 2 Then 'dont know how to handle for format2
              .Constit.Add "SUR_AREA"
              .Constit.Add "VOLUME"
            Else
              SetConstitFromTabType f.LocDir(j)
            End If
          End With
          j = j + 1
          ReDim Preserve f.LocDir(j)
        End If
      Wend
      f.LocCount = UBound(f.LocDir)
      
    Case "-TSD"
      If pMonitorSet Then pMonitor.SendMonitorMessage "(MSG2 Reading Date Information in FEO TSD)"
      Line Input #i, s 'LOCATION_KNT=
      s = Trim(Mid(s, 14, 8))
      If IsNumeric(s) Then
        If CLng(s) <> f.LocCount Then
          MsgBox "Warning: Location count in '" & f.NameFeo & "' = " & s & vbCr & _
                 "But number of locations read = " & f.LocCount, vbCritical, "FEO Read"
        End If
      End If
      Line Input #i, s 'Start time labels
      Line Input #i, s 'Start time values
      datsum.SJDay = CDbl(Mid(s, 23, 15))
      Line Input #i, s 'End time labels
      Line Input #i, s 'End time values
      datsum.EJDay = CDbl(Mid(s, 23, 15))
      datsum.NVALS = Mid(s, 40, 10)
      If pMonitorSet Then
        pMonitor.SendMonitorMessage "(MSG2 Start Day = " & DumpDate(datsum.SJDay) _
                                       & " End = " & DumpDate(datsum.EJDay) _
                                       & " NVALS = " & datsum.NVALS & ")"
      End If
    Case "-TERMINATION":  Line Input #i, f.Term
    Case Else
      If pMonitorSet Then pMonitor.SendMonitorMessage "(MSG2 Skipping line " & s & ")"
    End Select
  Loop
  Close #i
  If pMonitorSet Then pMonitor.SendMonitorMessage "(MSG2 Creating Dates)"
  f.RecLen = (f.LocCount + 1) * 8
  
  'Need to re-jigger if he has fixed FEQ to not save bogus zero element of array
  
  'datsum.NVALS = datsum.NVALS - 1
  ReDim jday(datsum.NVALS)
  ReDim jdflg(datsum.NVALS)
  
  Set pDates = Nothing
  Set pDates = New ATCData.ATCclsTserDate
  pDates.Summary = datsum
  
  'Read in all the dates - maybe we don't have to do this right away
  progress = "If Len(Dir(f.NameTsd)) = 0 Then -- " & f.NameTsd
  If Len(Dir(f.NameTsd)) = 0 Then
    pErrorDescription = "Could not find TSD file " & f.NameTsd
    If pMonitorSet Then pMonitor.SendMonitorMessage "(MSG2 " & pErrorDescription & ")"
    Exit Sub
  End If
  
  progress = "Open f.NameTsd For Random "
  Open f.NameTsd For Random As #i Len = f.RecLen
  For j = 1 To datsum.NVALS
    Get #i, j, d
    jday(j) = d
    jdflg(j) = JDateIntrvl(d)
  Next j
  Close #i

  pDates.flags = jdflg
  pDates.Values = jday
  Set pDates.File = Me
'  pDates.calcSummary 'This could cause shifting of date when the data is constant interval - better to skip it
  pDatesPopulated = True
  
  If pMonitorSet Then pMonitor.SendMonitorMessage "(MSG2 Creating Datsets)"
  For j = 0 To f.LocCount - 1
    For i = 1 To f.LocDir(j).Constit.Count
      Set lData = Nothing
      Set lData = New ATCData.ATCclsTserData
      With lData.Header
        .id = j * f.LocDir(j).Constit.Count + i
        .loc = f.LocDir(j).Location
        .con = f.LocDir(j).Constit(i)
        .Sen = f.Scenario
        .desc = "FEO:" & .Sen & ":" & .loc & ":" & .con
      End With
      lData.AttribSet "BRANCH", CStr(f.LocDir(j).Branch)
      lData.AttribSet "NODE", f.LocDir(j).NodeID
      If j < f.LocCount - 1 Then 'this is a guess!
        lData.AttribSet "DSNODE", f.LocDir(j + 1).NodeID
      End If
      lData.AttribSet "DISTANCE", CStr(f.LocDir(j).Station)
      lData.AttribSet "INVERT", CStr(f.LocDir(j).Invert)
      lData.AttribSet "FtabType", CStr(f.LocDir(j).FtabTyp)
      lData.AttribSet "FtabIndex", CStr(f.LocDir(j).FtabIndex)
      lData.AttribSet "LocClass", f.LocDir(j).LocClass
      lData.AttribSet "SeriesOffset", CStr(f.LocDir(j).SeriesOffset)
      lData.AttribSet "UserID", f.LocDir(j).UserID
      Set lData.Dates = pDates
      Set lData.File = Me
      pData.Add lData
    Next i
  Next j
  
  GoTo e:

error:
  MsgFeedback "Error #" & err.Number & ": " & err.Description & vbCrLf _
       & "NameFeo = " & f.NameFeo & vbCrLf _
       & "CurDir = " & CurDir & vbCrLf _
       & "progress = " & progress & vbCrLf _
       & "s = " & s & vbCrLf _
       & "pErrorDescription = " & pErrorDescription

e:
  If pMonitorSet Then pMonitor.SendMonitorMessage "(CLOSE)"
  On Error GoTo 0

End Sub

Private Sub MsgFeedback(Msg As String)
  Dim afb As clsATCoFeedback
  Set afb = New clsATCoFeedback
  afb.AddText Msg
  afb.Show App, frmGenRDBInit.Icon
End Sub

Private Sub AddToColl(coll As Collection, ParamArray newValues())
  Dim val As Variant
  For Each val In newValues
    coll.Add val
  Next
End Sub

Private Sub SetConstitFromTabType(ByRef LocDir As FeqLocDir)
  With LocDir
    Select Case .FtabTyp
    Case 20: AddToColl .Constit, "A", "T", "DT", "K", "DK", "B", "DB"
    Case 23: AddToColl .Constit, "A", "T", "DT", "K", "DK", "B", "DB", "MA", "DMA", "MQ", "DMQ"
    Case 21: AddToColl .Constit, "A", "T", "DT", "J", "K", "DK", "B", "DB"
    Case 22: AddToColl .Constit, "A", "T", "DT", "J", "K", "DK", "B", "DB", "ALP", "DALP", "QC"
    Case 24: AddToColl .Constit, "A", "T", "DT", "J", "K", "DK", "B", "DB", "MA", "DMA", "MQ", "DMQ"
    Case 25: AddToColl .Constit, "A", "T", "DT", "J", "K", "DK", "B", "DB", "ALP", "DALP", "QC", "MA", "DMA", "MQ", "DMQ"
    End Select
  End With
End Sub

Private Function FtabTyp(Ind As Long) As Long
  If UBound(pFileBytes) > 0 Then FtabTyp = Byte2Long(pFileBytes, (Ind + 2) * 4)
End Function

Private Sub FillFeqTimser(dataObject As ATCclsTserData)
  'Dim index#
  Dim i%, pos&, lind&, tcol&, r!, lp&, X!, e!, Apos&
  Dim addr&, ya!, c&
  'Dim a!, t!, dt!, j!, k!, dk!, b!, db!, alp!, dalp!, qc!, ma!, dma!, mq!, dmq!
  Dim a!(), t!(), dt!(), j!(), k!(), dk!(), b!(), db!(), alp!(), dalp!(), qc!(), ma!(), dma!(), mq!(), dmq!()
  Dim spos&
  Dim Ftab() As Single, LA As Long, HA As Long, FtabPos As Long, Xoff As Long
  'Dim flg&()
  Dim LocName As String, ConName As String

  'If pMonitorSet Then pMonitor.SendMonitorMessage "(OPEN Filling FEO Data)(BUTTOFF CANCEL)(BUTTOFF PAUSE)"
  
  If UBound(pFileBytes) = 0 Then
    pErrorDescription = "FTF file not found: " & f.NameFtf
    If pMonitorSet Then pMonitor.SendMonitorMessage "(MSG1 " & pErrorDescription & ")(CLOSE)"
    Exit Sub
  End If

  LocName = dataObject.Header.loc
  ConName = dataObject.Header.con
  'ReDim v(f.TimCount)
  If pMonitorSet Then pMonitor.SendMonitorMessage "(MSG1 Filling FEO Data for Node " & LocName & ", Constituent " & ConName & ")"
  
  'index = dataObject.Header.id                'FIXME -- Is this where the index should come from?
  lind = 0
  While f.LocDir(lind).Location <> LocName
    lind = lind + 1
    If lind > UBound(f.LocDir) Then
      MsgBox "Location '" & LocName & "' not found in FEO.", vbCritical, "FEQ Fill Timser"
      If pMonitorSet Then pMonitor.SendMonitorMessage "(CLOSE)"
      Exit Sub
    End If
  Wend
  c = 0
  While f.LocDir(lind).Constit(c + 1) <> ConName
    c = c + 1
    If c >= f.LocDir(lind).Constit.Count Then
      MsgBox "Constituent '" & ConName & "' not found in FEO.", vbCritical, "FEQ Fill Timser"
      Exit Sub
    End If
  Wend
  If c < 2 Then
    tcol = f.LocDir(lind).SeriesOffset + c
    X = 0
  Else
    tcol = f.LocDir(lind).SeriesOffset + 1
    X = c - 1
  End If
  e = f.LocDir(lind).Invert
  addr = f.LocDir(lind).FtabIndex
  
  Apos = pDates.Summary.NVALS
  If addr = 0 Then
    pErrorDescription = "No FtabIndex for node " & f.LocDir(lind).NodeID
  ReDim a(Apos)
    dataObject.Values = a
    dataObject.calcSummary
    Exit Sub
  End If
  'ReDim flg(Apos)
  ReDim a(Apos)
  If X > 0 Then
    ReDim t(Apos)
    ReDim dt(Apos)
    ReDim k(Apos)
    ReDim dk(Apos)
    ReDim b(Apos)
    ReDim db(Apos)
    'Some of these may get larger in the select below, but by default we want all of them dimensioned
    ReDim ma(0): ReDim dma(0): ReDim mq(0): ReDim dmq(0): ReDim j(0): ReDim alp(0): ReDim dalp(0): ReDim qc(0)
    Select Case f.LocDir(lind).FtabTyp
      Case 20:  Xoff = 5:  'all redims are included in defaults
      Case 21:  Xoff = 6:  ReDim j(Apos)
      Case 23:  Xoff = 7:                 ReDim ma(Apos):  ReDim dma(Apos):  ReDim mq(Apos): ReDim dmq(Apos)
      Case 24:  Xoff = 8:  ReDim j(Apos): ReDim ma(Apos):  ReDim dma(Apos):  ReDim mq(Apos): ReDim dmq(Apos)
      Case 22:  Xoff = 8:  ReDim j(Apos): ReDim alp(Apos): ReDim dalp(Apos): ReDim qc(Apos)
      Case 25:  Xoff = 10: ReDim j(Apos): ReDim alp(Apos): ReDim dalp(Apos): ReDim qc(Apos): ReDim ma(Apos): ReDim dma(Apos): ReDim mq(Apos): ReDim dmq(Apos)
    End Select
  End If
  
  LA = addr + XTIOFF
  HA = Byte2Long(pFileBytes, addr * 4)
  
  'Populate Ftab from a section of pFileBytes read from *.ftf
  ReDim Ftab(HA - LA)
  For FtabPos = LA To HA
    Ftab(FtabPos - LA) = Byte2Single(pFileBytes, FtabPos * 4)
  Next
  
  i = FreeFile(0)
  Open f.NameTsd For Binary Access Read As #i 'Len = f.RecLen
  spos = 0 'fix this!!!!
  Apos = 0
  For pos = spos To spos + pDates.Summary.NVALS - 1
    Apos = Apos + 1
    lp = (pos * f.RecLen) + (tcol * 4) + 5
    Get #i, lp, r
    If X > 0 Then ' need to use ftable
      ya = r - e
      Select Case f.LocDir(lind).FtabTyp
        Case 20: F90_XLKT20 addr, ya, a(Apos), t(Apos), dt(Apos), k(Apos), dk(Apos), b(Apos), db(Apos)
        Case 23: F90_XLKT23 addr, ya, a(Apos), t(Apos), dt(Apos), k(Apos), dk(Apos), b(Apos), db(Apos), ma(Apos), dma(Apos), mq(Apos), dmq(Apos)
        Case 21: F90_XLKT21 addr, ya, a(Apos), t(Apos), dt(Apos), j(Apos), k(Apos), dk(Apos), b(Apos), db(Apos)
        Case 22: F90_XLKT22 addr, ya, a(Apos), t(Apos), dt(Apos), j(Apos), k(Apos), dk(Apos), b(Apos), db(Apos), alp(Apos), dalp(Apos), qc(Apos)
        Case 24: F90_XLKT24 addr, ya, a(Apos), t(Apos), dt(Apos), j(Apos), k(Apos), dk(Apos), b(Apos), db(Apos), ma(Apos), dma(Apos), mq(Apos), dmq(Apos)
        Case 25: F90_XLKT25 addr, ya, a(Apos), t(Apos), dt(Apos), j(Apos), k(Apos), dk(Apos), b(Apos), db(Apos), alp(Apos), dalp(Apos), qc(Apos), ma(Apos), dma(Apos), mq(Apos), dmq(Apos)
      End Select
    Else
      a(Apos) = r
    End If
    'flg(Apos) = 0 'Start at index 0
  Next pos
  Close #i
  
  If X > 0 Then
    FindSetTS LocName, "A", a
    FindSetTS LocName, "T", t
    FindSetTS LocName, "DT", dt
    FindSetTS LocName, "J", j
    FindSetTS LocName, "K", k
    FindSetTS LocName, "DK", dk
    FindSetTS LocName, "B", b
    FindSetTS LocName, "DB", db
    FindSetTS LocName, "ALP", alp
    FindSetTS LocName, "DALP", dalp
    FindSetTS LocName, "QC", qc
    FindSetTS LocName, "MA", ma
    FindSetTS LocName, "DMA", dma
    FindSetTS LocName, "MQ", mq
    FindSetTS LocName, "DMQ", dmq
  Else
    dataObject.Values = a
    'dataObject.flags = flg
    dataObject.calcSummary
  End If
  'If pMonitorSet Then pMonitor.SendMonitorMessage "(CLOSE)"
End Sub

Private Sub FindSetTS(loc As String, con As String, setValues() As Single)
  Dim SetTS As ATCclsTserData
  If UBound(setValues) > 0 Then
    Set SetTS = FindTS(loc, con)
    If SetTS Is Nothing Then
      Debug.Print "(DBG Did not find " & loc & " " & con & ")"
      If pMonitorSet Then pMonitor.SendMonitorMessage "(DBG Did not find " & loc & " " & con & ")"
    Else
      Dim i& ', Msg$
      Dim lValues() As Single
      ReDim lValues(LBound(setValues) To UBound(setValues))
      'MsgBox "About to set values(" & LBound(setValues) & " - " & UBound(setValues) & ") for " & loc & " " & con, vbOKOnly, "FindSetTS"
      For i = LBound(setValues) To UBound(setValues)
        lValues(i) = setValues(i)
      Next
      SetTS.Values = lValues
      'MsgBox "Successfully set values(" & UBound(setValues) & " for " & loc & " " & con, vbOKOnly, "FindSetTS"
      SetTS.calcSummary
      'MsgBox "Successfully calculated summary for " & loc & " " & con, vbOKOnly, "FindSetTS"
    End If
  End If
End Sub

Private Function FindTS(loc As String, con As String) As ATCclsTserData
  Dim vTS As Variant
  Set FindTS = Nothing
  For Each vTS In pData
    If vTS.Header.loc = loc Then
      If vTS.Header.con = con Then
        Set FindTS = vTS
        Exit For
      End If
    End If
  Next
End Function

Private Sub XLKT20(ByVal addr As Long, ByVal ya As Single, _
        ByRef a!, ByRef t!, ByRef dt!, ByRef k!, ByRef dk!, ByRef b!, ByRef db!)

End Sub

Private Sub FtfRead()
  Dim i&, l&, j&, u&, kl&, kb&, s$, p&
  'Dim ftfDumpFilename As String
  ReDim f.ftf(0)
  
  'ftfDumpFilename = GetTmpFileName
  'Open ftfDumpFilename For Output As 99
  
  l = UBound(pFileBytes)
  For i = 0 To l - 3 Step 4
    j = Byte2Long(pFileBytes, i)
    If j >= 20 And j <= 26 Then
      u = UBound(f.ftf) + 1
      ReDim Preserve f.ftf(u)
      With f.ftf(u)
        .Bindex = i - 8
        .Lindex = (.Bindex - 1) / 4
        .Type = j
        If .Type = 20 Then
          .Off = 5
        ElseIf .Type = 21 Then
          .Off = 6
        ElseIf .Type = 22 Then
          .Off = 8
        Else
          .Off = 3 'add others
        End If
        .HA = Byte2Long(pFileBytes, .Bindex)
        .NTab = Byte2Long(pFileBytes, .Bindex + 4)
        .LSta = Byte2Long(pFileBytes, .Bindex + 12)
        's = NumFmtI(.Type, 2) & " " & NumFmtI(.Bindex, 12) & " " & NumFmtI(.Lindex, 12) & " " & _
        '    NumFmtI(.HA, 12) & " " & NumFmtI((.LSta), 12) & " " & NumFmtI(.NTab, 12)
        'Print #99, s
        'If .HA < 100000# Then
        '  For kl = .Lindex + 9 To .HA Step .Off
        '    kb = kl * 4
        '    s = "   " & NumFmtI(kb, 12) & " " & NumFmtI(kl, 12) & " "
        '    For p = 1 To .Off
        '      s = s & NumFmted(Byte2Single(pFileBytes, kb), 12, 3) & " "
        '      kb = kb + 4
        '    Next p
        '    Print #99, s
        '  Next kl
        '  Print #99, " "
        'End If
      End With
    End If
  Next i
  'Close #99
  'Kill ftfDumpFilename
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


