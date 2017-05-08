VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsTSerPltgen"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Attribute VB_Ext_KEY = "SavedWithClassBuilder6" ,"Yes"
Attribute VB_Ext_KEY = "Top_Level" ,"Yes"
Option Explicit
'Copyright 2002 by AQUA TERRA Consultants
Implements ATCclsTserFile

Private pFileExt As String
Private pErrorDescription As String
Private pMonitor As Object
Private pMonitorSet As Boolean
Private pData As Collection
Private pDates As ATCData.ATCclsTserDate

'following types from genscn:gentry
Private Type PltgenColInfo
    Scenario As String
    Location As String
    Constituent As String
    StartPos As Long
    val() As Single 'data values
End Type

Private Type PltgenD
    NamePltgen As String 'Pltgen file name
    ColCount As Long 'count of data columns (not including Date column)
    ColInfo() As PltgenColInfo 'info for each data column
    Dates() As Double 'julian dates
    flags() As Long '
    NVALS As Long 'number of values
End Type
Private r As PltgenD

Private Property Get ATCclsTserFile_AvailableAttributes() As Collection
  Set ATCclsTserFile_AvailableAttributes = New Collection
End Property

Private Sub ATCclsTserFile_clear()
  Set pData = Nothing
  Set pDates = Nothing
  ReDim r.ColInfo(0)
  ReDim r.Dates(0)
  ReDim r.flags(0)
End Sub

Private Property Get ATCclsTserFile_Data(index As Long) As ATCData.ATCclsTserData
  Set ATCclsTserFile_Data = pData(index)
End Property

Private Property Get ATCclsTserFile_DataCount() As Long
  ATCclsTserFile_DataCount = r.ColCount
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
  ATCclsTserFile_Label = "PLTGEN"
End Property

Private Sub ATCclsTserFile_readData(dataObject As ATCclsTserData)

End Sub

Private Sub ATCclsTserFile_refresh()

End Sub

Private Function ATCclsTserFile_writeDataHeader(dataObject As ATCclsTserData, Optional oldHeader As ATTimSerDataHeader) As Boolean
  pErrorDescription = "Unable to Write a Data Header for Class PLTGEN"
  ATCclsTserFile_writeDataHeader = False
End Function

Private Sub Class_Initialize()
  r.NamePltgen = "<unknown>"
  pFileExt = "PLTGEN Files (*.plt)|*.plt|All Files (*.*)|*.*"
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
  ATCclsTserFile_FileName = r.NamePltgen
End Property

Private Property Let ATCclsTserFile_FileName(ByVal newValue$)
  If Len(Dir(newValue)) = 0 Then
    pErrorDescription = "FileName '" & newValue & "' not found"
  Else
    r.NamePltgen = newValue
    Call PltgenRead(r)
  End If
End Property

Private Property Get ATCclsTserFile_Description() As String
  ATCclsTserFile_Description = "Pltgen Time Series Data Type"
End Property

Private Property Get ATCclsTserFile_DCnt() As Long
  ATCclsTserFile_DCnt = r.ColCount
End Property

Private Sub PltgenRead(r As PltgenD)
  Dim lData As ATCData.ATCclsTserData

  Dim i&, j&, llen&, ifl&, ofl&, ip&, icol&, datecol&, sp&, d#, skip&
  Dim dd#
  Dim ldt&(5), SLCflg&, errflg&, tabcnt&, ivar&, spos&
  Dim istr$, lstr$, dstr$, lochdr$(), hdrcnt&, chkstr$
  Dim lsen$(), LLoc$(), lcon$(), itemp&
  Dim allsen As Boolean, allloc As Boolean, dogrid As Boolean
  Dim allcon As Boolean, ProcessField As Boolean

  On Error GoTo PltgenErrhand
  
  If pMonitorSet Then
    pMonitor.SendMonitorMessage "(OPEN Pltgen File)"
    pMonitor.SendMonitorMessage "(BUTTOFF CANCEL)"
    pMonitor.SendMonitorMessage "(BUTTOFF PAUSE)"
    pMonitor.SendMonitorMessage "(MSG1 " & r.NamePltgen & ")"
  End If
   
  errflg = 1
  ifl = FreeFile(0)
  Open r.NamePltgen For Input As #ifl
  errflg = 2
   
  Line Input #ifl, istr  'ignore line 1
  Line Input #ifl, istr  'ignore line 2
  Line Input #ifl, istr
  istr = Mid(istr, Len(istr) - 2)
  ivar = CInt(istr)  'find number of time series in this file
   
  dogrid = False
  ReDim Preserve lcon(ivar)  'find constituent names
  ReDim Preserve lsen(ivar)
  ReDim Preserve LLoc(ivar)
  For i = 1 To 8
    Line Input #ifl, istr
  Next i
  For i = 1 To ivar  'read each const name
    Line Input #ifl, istr
    lcon(i - 1) = Trim(Mid(istr, 6, 8))
    lsen(i - 1) = Trim(Mid(r.NamePltgen, Len(r.NamePltgen) - 2)) 'set default scenario name
    LLoc(i - 1) = Trim(FilenameOnly(r.NamePltgen)) 'set default location name
    itemp = Len(istr)
    If itemp > 80 Then
      'read scenario name if available
      lsen(i - 1) = Trim(Mid(istr, 81, 8))
      If itemp > 90 Then
        'read location name if available
        LLoc(i - 1) = Trim(Mid(istr, 91, 8))
      Else
        'no location name available
        dogrid = True
      End If
    Else
      'no scenario name available
      dogrid = True
    End If
  Next i
  
  Line Input #ifl, istr
  chkstr = Mid(istr, 6, 9)
  While chkstr <> "Date/time"
    Line Input #ifl, istr
    chkstr = Mid(istr, 6, 9)
  Wend
  'For i = 1 To 2 'read blank lines
    Line Input #ifl, istr
  'Next i
  
  errflg = 4
  'new form for user specified s,l,c
  If dogrid Then
    Close #ifl 'close the file so that we can update attributes
    frmGenPLTInit.lblColHdr.Caption = "Timeseries in PLTGEN File " & r.NamePltgen
    With frmGenPLTInit.acgPltgen
      .ColTitle(0) = "Process"
      .ColTitle(1) = "Scenario"
      .ColTitle(2) = "Location"
      .ColTitle(3) = "Constituent"
      .ColType(0) = ATCoTxt
      .ColType(1) = ATCoTxt
      .ColType(2) = ATCoTxt
      .ColType(3) = ATCoTxt
      .ColEditable(0) = True
      .ColEditable(1) = True
      .ColEditable(2) = True
      .ColEditable(3) = True
      .ComboCheckValidValues = True
      .rows = 0
      For j = 1 To ivar
        .rows = .rows + 1
        .TextMatrix(j, 0) = "True"
        .TextMatrix(j, 1) = lsen(.rows - 1)
        .TextMatrix(j, 2) = LLoc(.rows - 1)
        .TextMatrix(j, 3) = lcon(.rows - 1)
      Next j
      If .rows < 20 Then
        frmGenPLTInit.Height = .DesiredHeight + 2100
      End If
      .ColsSizeByContents
    End With
    Call frmGenPLTInit.SetPltgenName(r.NamePltgen)
    frmGenPLTInit.Show 1
    
    If frmGenPLTInit.Tag = -1 Then GoTo canceldata
    If frmGenPLTInit.Tag = 1 Then 'okay
      'put entered values in data structure
      r.ColCount = 0
      For i = 1 To ivar
        If frmGenPLTInit.acgPltgen.TextMatrix(i, 0) = "True" Then
          'this one is to be processed
          r.ColCount = r.ColCount + 1
          ReDim Preserve r.ColInfo(r.ColCount)
          r.ColInfo(r.ColCount - 1).Scenario = frmGenPLTInit.acgPltgen.TextMatrix(i, 1)
          r.ColInfo(r.ColCount - 1).Location = frmGenPLTInit.acgPltgen.TextMatrix(i, 2)
          r.ColInfo(r.ColCount - 1).Constituent = frmGenPLTInit.acgPltgen.TextMatrix(i, 3)
          r.ColInfo(r.ColCount - 1).StartPos = i
        End If
      Next i
      'open the file again and return to end of headers
      Open r.NamePltgen For Input As #ifl
      Line Input #ifl, istr
      chkstr = Mid(istr, 6, 9)
      While chkstr <> "Date/time"
        Line Input #ifl, istr
        chkstr = Mid(istr, 6, 9)
      Wend
      'For i = 1 To 2 'read blank lines
        Line Input #ifl, istr
      'Next i
    End If
    Unload frmGenPLTInit
  Else
    r.ColCount = ivar
    ReDim Preserve r.ColInfo(r.ColCount)
    For i = 1 To ivar
      r.ColInfo(i - 1).Scenario = lsen(i - 1)
      r.ColInfo(i - 1).Location = LLoc(i - 1)
      r.ColInfo(i - 1).Constituent = lcon(i - 1)
      r.ColInfo(i - 1).StartPos = i
    Next i
  End If
  
  r.NVALS = 1 'Start at index one; we subtract one after the loop
  ReDim r.Dates(2000)
  ReDim r.flags(2000)
  For j = 0 To r.ColCount - 1
    ReDim r.ColInfo(j).val(2000)
  Next j
  'r.JDate.s.CIntvl = True
  While Not EOF(ifl)
    Line Input #ifl, istr
    'process dates
    ldt(0) = CInt(Mid(istr, 7, 4))
    ldt(1) = CInt(Mid(istr, 12, 2))
    ldt(2) = CInt(Mid(istr, 15, 2))
    ldt(3) = CInt(Mid(istr, 18, 2))
    ldt(4) = CInt(Mid(istr, 21, 2))
    ldt(5) = 0
    d = Date2J(ldt)
    
    If d > 0 Then 'valid date
      If r.NVALS >= UBound(r.Dates) Then  'make space larger
        ReDim Preserve r.Dates(r.NVALS + 2000)
        ReDim Preserve r.flags(r.NVALS + 2000)
        For j = 0 To r.ColCount - 1
          ReDim Preserve r.ColInfo(j).val(r.NVALS + 2000)
        Next j
      End If
      r.Dates(r.NVALS) = d
      r.flags(r.NVALS) = DateIntrvl(ldt)
      'process data
      For i = 0 To r.ColCount - 1
        spos = 25 + (14 * (r.ColInfo(i).StartPos - 1))
        lstr = Mid(istr, spos, 12)
        If IsNumeric(lstr) Then
          d = lstr
        Else
          d = -999 'undef
        End If
        r.ColInfo(i).val(r.NVALS) = d
      Next i
      r.NVALS = r.NVALS + 1
    Else
      pErrorDescription = "Unable to determine Date field in Pltgen file " & r.NamePltgen
    End If
  Wend
  Close #ifl
  
  r.NVALS = r.NVALS - 1
  ReDim Preserve r.Dates(r.NVALS)
  ReDim Preserve r.flags(r.NVALS)
  
  Set pDates = New ATCData.ATCclsTserDate
  pDates.flags = r.flags
  pDates.Values = r.Dates
  Set pDates.File = Me
  pDates.calcSummary
  
  For j = 0 To r.ColCount - 1
    ReDim Preserve r.ColInfo(j).val(r.NVALS)
    Set lData = New ATCData.ATCclsTserData
    With lData.Header
      .id = j + 1
      .loc = r.ColInfo(j).Location
      .con = r.ColInfo(j).Constituent
      .Sen = r.ColInfo(j).Scenario
      .desc = "Pltgen:" & .Sen & ":" & .loc & ":" & .con
    End With
    Set lData.Dates = pDates
    lData.Values = r.ColInfo(j).val
    lData.calcSummary
    Set lData.File = Me
    pData.Add lData
  Next j
  
  If pMonitorSet Then
    pMonitor.SendMonitorMessage "(CLOSE)"
    pMonitor.SendMonitorMessage "(BUTTON CANCEL)"
    pMonitor.SendMonitorMessage "(BUTTON PAUSE)"
  End If
  Exit Sub
  
canceldata:
  If pMonitorSet Then
    pMonitor.SendMonitorMessage "(CLOSE)"
    pMonitor.SendMonitorMessage "(BUTTON CANCEL)"
    pMonitor.SendMonitorMessage "(BUTTON PAUSE)"
  End If
  Unload frmGenPLTInit
  Exit Sub

PltgenErrhand:
  If errflg = 1 Then
    pErrorDescription = "Problem opening specified Pltgen File " & r.NamePltgen
    Exit Sub
  ElseIf errflg = 2 Then
    pErrorDescription = "Problem processing Scenario/Location/Constituent info on Pltgen File " & r.NamePltgen
  ElseIf errflg = 3 Then
    pErrorDescription = "Problem finding Date field on Pltgen File " & r.NamePltgen
  ElseIf errflg = 4 Then
    pErrorDescription = "Problem processing field information on Pltgen File " & r.NamePltgen
  ElseIf errflg = 5 Then
    pErrorDescription = "Problem processing Date values on Pltgen File " & r.NamePltgen
  ElseIf errflg = 6 Then
    pErrorDescription = "Problem updating file " & r.NamePltgen & " with Scenario, Location, and Constituent information."
    errflg = 7
  End If
  Resume Next 'not always?
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

Private Function ATCclsTserFile_SaveAs(filename As String) As Boolean
  ATCclsTserFile_SaveAs = False
  pErrorDescription = "SaveAs not supported for " & ATCclsTserFile_Description
End Function
