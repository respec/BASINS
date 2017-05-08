VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsTSerSWMM"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'##MODULE_REMARKS Copyright 2001-3 AQUA TERRA Consultants - Royalty-free use permitted under open source license
Implements ATCclsTserFile

Private pErrorDescription As String
Private pMonitor As Object
Private pMonitorSet As Boolean
Private pData As Collection
Private pDates As ATCData.ATCclsTserDate
Private pFileName As String

Private Type SwmmTimser
  v() As Single 'flow and pollutants loads
End Type

Private Type SwmmPoll
  Pname As String 'pollutant names
  Punit As String 'pollutant units, eg mg/l, MPN/l, JTU, etc.
  Ndim As Long 'type of concentration units, 0-mg/l, 1=other
End Type

Private Type SwmmLocation
  Name As String 'location id (handles either NLOC (numeric) or KLOC (alphanumeric) SWMM convention)
  q As SwmmTimser 'flow array
  Poll() As SwmmTimser 'dim by Npoll
End Type

Private Type SwmmTime
  jday As Double 'Julian start date/time for each time step
  Delta As Single 'step size in seconds for next time step
End Type

Private Type SwmmData
  Title1 As String 'first line of title from 1st block
  Title2 As String 'second line of title from 1st block
  Idatez As Long 'starting date
  Tzero As Single 'starting time
  Title3 As String 'first line of title from prior block
  Title4 As String 'second line of title from prior block
  Source As String 'name of immediately prior block
  Locats As Long 'number of locations
  Npoll As Long 'number of pollutants
  Triba As Single 'tributary service area, acres
  Jce As Long 'flag for type of location numbers, 0-numeric, 1-alphanumeric
  p() As SwmmPoll 'array of pollutant info
  Qconv As Single 'conversion factor to get flow in cfs
  Tcount As Long 'count of time steps
  t() As SwmmTime 'array of time step info
  l() As SwmmLocation ' size Locats
End Type
Dim lsd As SwmmData

Private Property Let ATCclsTserFile_HelpFilename(ByVal newValue As String)
  App.HelpFile = newValue
End Property

Private Property Get ATCclsTserFile_TypeTimSer() As String
  ATCclsTserFile_TypeTimSer = "SWMM"
End Property

Private Sub Class_Initialize()
  pFileName = "<unknown>"
  pErrorDescription = ""
  pMonitorSet = False
  lsd.Npoll = 0
  lsd.Locats = 0
  
  Set pData = Nothing
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
  ATCclsTserFile_FileExtension = "SWMM Files (*.dnt;*.int)|*.dnt;*.int|All Files (*.*)|*.*"
End Property
Private Property Get ATCclsTserFile_ThreeCharExtension() As String
  ATCclsTserFile_ThreeCharExtension = "dnt"
End Property

Private Property Get ATCclsTserFile_FileUnit() As Long

End Property

Private Property Get ATCclsTserFile_FileName() As String
  ATCclsTserFile_FileName = pFileName
End Property
Private Property Let ATCclsTserFile_FileName(ByVal newValue$)
  If Len(Dir(newValue)) = 0 Then
    pErrorDescription = "FileName '" & newValue & "' not found"
  Else
    pFileName = newValue
    SwmmRead
  End If
End Property

Private Property Get ATCclsTserFile_DCnt() As Long
  ATCclsTserFile_DCnt = lsd.Npoll * lsd.Locats
End Property

Private Property Get ATCclsTserFile_Description() As String
  ATCclsTserFile_Description = "SWMM Time Series Data Type"
End Property

Private Property Get ATCclsTserFile_Label() As String
  ATCclsTserFile_Label = "SWMM"
End Property

Private Property Get ATCclsTserFile_AvailableAttributes() As Collection
  Set ATCclsTserFile_AvailableAttributes = New Collection
End Property

Private Property Get ATCclsTserfile_DataCollection() As Collection
  Set ATCclsTserfile_DataCollection = pData
End Property

Private Property Get ATCclsTserFile_DataCount() As Long
  ATCclsTserFile_DataCount = pData.Count
End Property

Private Property Get ATCclsTserFile_Data(index As Long) As ATCData.ATCclsTserData
  If index <= pData.Count And index > 0 Then
    Set ATCclsTserFile_Data = pData(index)
  Else
    Set ATCclsTserFile_Data = Nothing
  End If
End Property

Private Sub ATCclsTserFile_readData(dataObject As ATCData.ATCclsTserData)

End Sub

Private Sub ATCclsTserFile_clear()
  Set pData = Nothing
  Set pDates = Nothing
End Sub

Private Sub ATCclsTserFile_refresh()

End Sub


Private Function ATCclsTserFile_writeDataHeader(dataObject As ATCclsTserData, Optional oldHeader As ATTimSerDataHeader) As Boolean
  pErrorDescription = "Unable to Write a Data Header for " & ATCclsTserFile_Description
  ATCclsTserFile_writeDataHeader = False
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

Private Function ATCclsTserFile_SaveAs(Filename As String) As Boolean
  ATCclsTserFile_SaveAs = False
  pErrorDescription = "SaveAs not supported for " & ATCclsTserFile_Description
End Function

Sub SwmmRead()
  'read SWMM binary interface file
  Dim f%, l&, s$, i%, j%, lts&, d#
  Dim NLoc&, Julday&, Timday!
  Dim jday() As Double  'julian day of values
  Dim jdflg() As Long
  Dim lData As ATCData.ATCclsTserData

  'open and check validity of unformatted sequential file
  f = FtnUnfSeqInitRd(pFileName)
  If f <= 0 Then 'file not opened, not unformatted sequential
    pErrorDescription = "File '" & pFileName & "' is not a SWMM Interface File"
  Else 'file opened ok, read it
    l = FtnUnfSeqRecLen(f)
    lsd.Title1 = Input(l, #f)
    l = FtnUnfSeqRecLen(f)
    lsd.Title2 = Input(l, #f)
    l = FtnUnfSeqRecLen(f)
    Get #f, , lsd.Idatez
    Get #f, , lsd.Tzero
    l = FtnUnfSeqRecLen(f)
    lsd.Title3 = Input(l, #f)
    l = FtnUnfSeqRecLen(f)
    lsd.Title4 = Input(l, #f)
    l = FtnUnfSeqRecLen(f)
    lsd.Source = Input(20, #f)
    Get #f, , lsd.Locats
    ReDim lsd.l(lsd.Locats)
    Get #f, , lsd.Npoll
    ReDim lsd.p(lsd.Npoll)
    For i = 0 To lsd.Locats - 1
      ReDim lsd.l(i).Poll(lsd.Npoll)
    Next i
    Get #f, , lsd.Triba
    Get #f, , lsd.Jce
    l = FtnUnfSeqRecLen(f)
    For i = 0 To lsd.Locats - 1
      If lsd.Jce = 0 Then 'read numeric location name
        Get #f, , NLoc
        lsd.l(i).Name = Trim(CStr(NLoc))
      ElseIf lsd.Jce = 1 Then 'read alphanumeric location name
        lsd.l(i).Name = Trim(Input(10, #f))
      End If
    Next i
    If lsd.Npoll > 0 Then
      l = FtnUnfSeqRecLen(f)
      For i = 0 To lsd.Npoll - 1
        lsd.p(i).Pname = Trim(Input(8, #f))
      Next i
      l = FtnUnfSeqRecLen(f)
      For i = 0 To lsd.Npoll - 1
        lsd.p(i).Punit = Trim(Input(8, #f))
      Next i
      l = FtnUnfSeqRecLen(f)
      For i = 0 To lsd.Npoll - 1
        Get #f, , lsd.p(i).Ndim
      Next i
    End If
    l = FtnUnfSeqRecLen(f)
    Get #f, , lsd.Qconv

    lsd.Tcount = 0
    lts = FtnUnfSeqRecLen(f)
    Call SwmmSizeArrays(2000, jday, jdflg) 'initial size
    Do
      If lsd.Tcount > UBound(jday) Then 'add more space
        Call SwmmSizeArrays(lsd.Tcount + 2000, jday, jdflg)
      End If
      Get #f, , Julday
      Get #f, , Timday
      d = SWMMDate2Jul(Julday, Timday)
      lsd.t(lsd.Tcount).jday = d
      jday(lsd.Tcount) = d
      jdflg(lsd.Tcount) = JDateIntrvl(d)
      Get #f, , lsd.t(lsd.Tcount).Delta
      For i = 0 To lsd.Locats - 1
        Get #f, , lsd.l(i).q.v(lsd.Tcount)
        If lsd.Npoll > 0 Then
          For j = 0 To lsd.Npoll - 1
            Get #f, , lsd.l(i).Poll(j).v(lsd.Tcount)
          Next j
        End If
      Next i
      lsd.Tcount = lsd.Tcount + 1
      l = FtnUnfSeqRecLen(f)
      If l < lts Then
        Exit Do
      End If
    Loop
  End If

  Call SwmmSizeArrays(lsd.Tcount - 1, jday, jdflg) 'release unused space
  Close #f
   
  Set pDates = Nothing
  Set pDates = New ATCData.ATCclsTserDate
  pDates.flags = jdflg
  pDates.Values = jday
  Set pDates.File = Me
  pDates.calcSummary
   
  For j = 0 To lsd.Locats - 1
    For i = 0 To lsd.Npoll - 1
      Set lData = Nothing
      Set lData = New ATCData.ATCclsTserData
      With lData.Header
        .id = j * lsd.Npoll + i
        .loc = lsd.l(j).Name
        .con = lsd.p(i).Pname
        .Sen = FilenameOnly(pFileName)
        If Len(.Sen) > 8 Then .Sen = Left(.Sen, 8)
        .desc = "SWMM:" & .Sen & ":" & .loc & ":" & .con
      End With
      With lData
        .AttribSet "Units", lsd.p(i).Punit
        .Values = lsd.l(j).Poll(i).v
        Set .Dates = pDates
        Set .File = Me
      End With
      pData.Add lData
    Next i
  Next j
End Sub
Private Function SwmmSizeArrays(s&, jday#(), jdflg&())
  Dim i&, j&
  
  ReDim Preserve lsd.t(s)
  ReDim Preserve jday(s)
  ReDim Preserve jdflg(s)
  For i = 0 To lsd.Locats - 1
    ReDim Preserve lsd.l(i).q.v(s)
    For j = 0 To lsd.Npoll - 1
      ReDim Preserve lsd.l(i).Poll(j).v(s)
    Next j
  Next i
End Function
Private Function SWMMDate2Jul(Julday&, Timday!) As Double

  'convert SWMM convention date values
  'to Julian date/time value
  'Julday - 5 digit number, 2 digit year, 3 digit julian day
  'Timday - time of day in seconds

  Dim ldt&(5), jd&

  jd = Julday Mod 1000 'julian day
  'get month and day from year and julian day
  'MsgBox "Need to implement get month and day from year and julian day"
  'Call F90_JDMODY(ldt(0), jd, ldt(1), ldt(2))
  Call J2Date((jd), ldt)
  ldt(0) = Int(Julday / 1000) 'year
  If ldt(0) < 30 Then 'assume > year 2000
    ldt(0) = ldt(0) + 2000
  Else 'assume 1900 date
    ldt(0) = ldt(0) + 1900
  End If
  SWMMDate2Jul = MJD(ldt(0), ldt(1), ldt(2)) + Timday / 86400

End Function

Private Sub SWMMJul2Date(JDatim#, Julday&, Timday!)

  'convert Julian date/time value to
  'SWMM convention date values
  'JDatim - Julian date/time value
  'Julday - 5 digit number, 2 digit year, 3 digit julian day
  'Timday - time of day in seconds

  Dim ldt&(5), jd&, i&

  jd = Fix(JDatim)
  Call INVMJD(jd, ldt(0), ldt(1), ldt(2))
  Julday = (ldt(0) Mod 100) * 1000 'put 2 digit year in first 2 of 5 digits
  'calculate julian day
  For i = 1 To ldt(1) - 1 'sum days of months preceeding current month
    Julday = Julday + daymon(ldt(0), i)
  Next i
  Julday = Julday + ldt(2) 'include days of current month
  Timday = (JDatim - jd) * 86400#

End Sub

