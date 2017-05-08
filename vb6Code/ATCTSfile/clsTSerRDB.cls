VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsTSerRDB"
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
Private pData As Collection
Private pDates As ATCData.ATCclsTserDate

Private pMonitor As Object
Private pMonitorSet As Boolean

'following types from genscn:gentry
Private Type RDBColInfo
    Header As String
    ColPos As Long 'starting column for this field
    FldLen As Long 'width of this field
    Field As Long 'field number of this field
    Scenario As String
    Location As String
    Constituent As String
    val() As Single 'data values
    flg() As Long
End Type

Private Type RDBD
    NameRDB As String 'rdb file name
    ColCount As Long 'count of data columns (not including Date column)
    ColInfo() As RDBColInfo 'info for each data column
    DatePos As Long 'starting column for date field
    DateLen As Long 'width of date field
    DateField As Long 'field number containing date
    JDate() As Double 'julian dates and values
    jdflg() As Long
    NVALS As Long 'number of values
End Type

Private r As RDBD

Private Property Get ATCclsTserFile_AvailableAttributes() As Collection
  Set ATCclsTserFile_AvailableAttributes = New Collection
End Property

Private Sub ATCclsTserFile_clear()
  Set pData = Nothing
  Set pDates = Nothing
  ReDim r.ColInfo(0)
  ReDim r.JDate(0)
  ReDim r.jdflg(0)
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

Private Property Let ATCclsTserFile_HelpFilename(ByVal newvalue As String)
  App.HelpFile = newvalue
End Property

Private Property Get ATCclsTserFile_Label() As String
  ATCclsTserFile_Label = "RDB"
End Property

Private Sub ATCclsTserFile_readData(dataObject As ATCData.ATCclsTserData)

End Sub

Private Sub ATCclsTserFile_refresh()

End Sub

Private Function ATCclsTserFile_writeDataHeader(dataObject As ATCclsTserData, Optional oldHeader As ATTimSerDataHeader) As Boolean
  pErrorDescription = "Unable to Write a Data Header for Class RDB"
  ATCclsTserFile_writeDataHeader = False
End Function

Private Sub Class_Initialize()
    
  r.NameRDB = "<unknown>"
  pFileExt = "RDB Files (*.rdb)|*.rdb|All Files (*.*)|*.*"
  pErrorDescription = ""
  pMonitorSet = False
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
  ATCclsTserFile_FileExtension = pFileExt
End Property

Private Property Get ATCclsTserFile_FileName() As String
  ATCclsTserFile_FileName = r.NameRDB
End Property

Private Property Let ATCclsTserFile_FileName(ByVal newvalue$)
  If Len(Dir(newvalue)) = 0 Then
    pErrorDescription = "FileName '" & newvalue & "' not found"
  Else
    r.NameRDB = newvalue
    Call RDBRead(r)
  End If
End Property

Private Property Get ATCclsTserFile_Description() As String
  ATCclsTserFile_Description = "RDB Time Series Data Type"
End Property

Private Sub RDBRead(r As RDBD)
    Dim i&, j&, llen&, ifl&, ofl&, ip&, icol&, datecol&, sp&, d#, skip&
    Dim ldt&(5), SLCflg&, errflg&, tabcnt&
    Dim istr$, lstr$, dstr$, lochdr$(), hdrcnt&
    Dim lsen$(), LLoc$(), lcon$()
    Dim allsen As Boolean, allloc As Boolean
    Dim allcon As Boolean, ProcessField As Boolean
    Dim lData As ATCData.ATCclsTserData

    On Error GoTo rdberrhand 'off to find error locations
    
    If pMonitorSet Then
      pMonitor.SendMonitorMessage "(OPEN RDB File)"
      pMonitor.SendMonitorMessage "(BUTTOFF CANCEL)"
      pMonitor.SendMonitorMessage "(BUTTOFF PAUSE)"
      pMonitor.SendMonitorMessage "(MSG1 " & r.NameRDB & ")"
    End If

    SLCflg = 0
    errflg = 1
    ifl = FreeFile(0)
    Open r.NameRDB For Input As #ifl
    errflg = 2
    Line Input #ifl, istr
    While InStr(istr, "#") > 0 Or Len(istr) = 0
      If InStr(Left(istr, 12), "Scenario") > 0 Then
        ip = InStr(istr, "*")
        If ip > 0 Then 'same scenario for all columns
          allsen = True
          ReDim lsen(1)
          istr = Mid(istr, ip + 1)
          lsen(1) = LTrim(istr)
        Else 'scenarios vary with columns
          allsen = False
          ip = InStr(istr, "Scenario")
          istr = Mid(istr, ip)
          ip = InStr(istr, " ")
          istr = Mid(istr, ip)
          While Len(istr) > 0
            istr = LTrim(istr)
            ip = InStr(istr, " ")
            icol = CLng(Left(istr, ip - 1))
            ReDim Preserve lsen(icol)
            istr = LTrim(Mid(istr, ip))
            ip = InStr(istr, " ")
            If ip = 0 Then ip = Len(istr) + 1
            lsen(icol) = Left(istr, ip - 1)
            istr = Mid(istr, ip)
          Wend
        End If
        SLCflg = SLCflg + 1
      ElseIf InStr(Left(istr, 12), "Location") > 0 Then
        ip = InStr(istr, "*")
        If ip > 0 Then 'same location for all columns
          allloc = True
          ReDim LLoc(1)
          istr = Mid(istr, ip + 1)
          LLoc(1) = LTrim(istr)
        Else 'locations vary with columns
          allloc = False
          ip = InStr(istr, "Location")
          istr = Mid(istr, ip)
          ip = InStr(istr, " ")
          istr = Mid(istr, ip)
          While Len(istr) > 0
            istr = LTrim(istr)
            ip = InStr(istr, " ")
            icol = CLng(Left(istr, ip - 1))
            ReDim Preserve LLoc(icol)
            istr = LTrim(Mid(istr, ip))
            ip = InStr(istr, " ")
            If ip = 0 Then ip = Len(istr) + 1
            LLoc(icol) = Left(istr, ip - 1)
            istr = Mid(istr, ip)
          Wend
        End If
        SLCflg = SLCflg + 1
      ElseIf InStr(Left(istr, 15), "Constituent") > 0 Then
        ip = InStr(istr, "*")
        If ip > 0 Then 'same location for all columns
          allcon = True
          ReDim lcon(1)
          istr = Mid(istr, ip + 1)
          lcon(1) = LTrim(istr)
        Else 'locations vary with columns
          allcon = False
          ip = InStr(istr, "Constituent")
          istr = Mid(istr, ip)
          ip = InStr(istr, " ")
          istr = Mid(istr, ip)
          While Len(istr) > 0
            istr = LTrim(istr)
            ip = InStr(istr, " ")
            icol = CLng(Left(istr, ip - 1))
            ReDim Preserve lcon(icol)
            istr = LTrim(Mid(istr, ip))
            ip = InStr(istr, " ")
            If ip = 0 Then ip = Len(istr) + 1
            lcon(icol) = Left(istr, ip - 1)
            istr = Mid(istr, ip)
          Wend
        End If
        SLCflg = SLCflg + 1
      End If
      Line Input #ifl, istr
    Wend
    errflg = 3
    datecol = 0 'save column containing date
    tabcnt = 0 'count of tabs found
    i = 0
    While Len(istr) > 0
      i = i + 1
      istr = LTrim(istr)
      ip = InStr(istr, Chr(9))
      If ip = 0 Then 'use remaining portion of string
        ip = Len(istr) + 1
      Else
        tabcnt = tabcnt + 1
      End If
      ReDim Preserve lochdr(i)
      lochdr(i) = Left(istr, ip - 1)
      If datecol = 0 Then 'date column not yet defined
        If InStr(LCase(lochdr(i)), "date") > 0 Or _
           InStr(LCase(lochdr(i)), "time") > 0 Then
            datecol = i
            r.DateField = tabcnt
        End If
      End If
      istr = Mid(istr, ip + 1)
    Wend
    errflg = 4
    If datecol > 0 Then
      Line Input #ifl, istr
      'determine column formats
      r.ColCount = 0
      i = 0
      sp = 1 'keep track of start position for each column
      While Len(istr) > 0
        i = i + 1
        istr = LTrim(istr)
        ip = InStr(istr, Chr(9))
        If ip = 0 Then 'use remaining portion of string
          ip = Len(istr) + 1
        End If
        If InStr(Left(istr, ip - 1), "n") > 0 Then
          'numeric field
          ProcessField = True
          If SLCflg < 3 Then 'user needs to define SLC
            frmGenRDBInit.lblColHdr.Caption = "For data in column with header '" & lochdr(i) & "', enter:"
            frmGenRDBInit.Show 1
            If frmGenRDBInit.Tag = -1 Then GoTo canceldata
            If frmGenRDBInit.Tag = 1 Then
              'put entered values in data structure
              ReDim Preserve lsen(i)
              ReDim Preserve LLoc(i)
              ReDim Preserve lcon(i)
              lsen(i) = frmGenRDBInit.txtSLC(0).Text
              LLoc(i) = frmGenRDBInit.txtSLC(1).Text
              lcon(i) = frmGenRDBInit.txtSLC(2).Text
            Else 'skip field or cancel
              ProcessField = False
            End If
          End If
          If ProcessField Then
            ReDim Preserve r.ColInfo(r.ColCount)
            r.ColInfo(r.ColCount).Header = lochdr(i)
            r.ColInfo(r.ColCount).ColPos = sp
            r.ColInfo(r.ColCount).FldLen = Left(istr, ip - 2)
            r.ColInfo(r.ColCount).Field = i
            If allsen Then
              r.ColInfo(r.ColCount).Scenario = lsen(1)
            Else
              r.ColInfo(r.ColCount).Scenario = lsen(i)
            End If
            If allloc Then
              r.ColInfo(r.ColCount).Location = LLoc(1)
            Else
              r.ColInfo(r.ColCount).Location = LLoc(i)
            End If
            If allcon Then
              r.ColInfo(r.ColCount).Constituent = lcon(1)
            Else
              r.ColInfo(r.ColCount).Constituent = lcon(i)
            End If
            r.ColCount = r.ColCount + 1
          End If
        End If
        'determine width of field
        lstr = ""
        While IsNumeric(Mid(istr, 1, 1))
          lstr = lstr & Mid(istr, 1, 1)
          istr = Mid(istr, 2)
          ip = ip - 1
        Wend
        If i = datecol Then 'save width of date field
          r.DatePos = sp
          r.DateLen = CLng(lstr)
        End If
        'update start position for next field
        sp = sp + CLng(lstr) + 1
        istr = Mid(istr, ip + 1)
      Wend
      errflg = 5
      If SLCflg < 3 Then 'write user supplied S,L,C to file
        Close #ifl
        Open r.NameRDB For Input As #ifl
        ofl = FreeFile(0)
        Open "rdb.xxx" For Output As #ofl
        i = 1
        lstr = ""
        While i <= UBound(lsen)
          If Len(lsen(i)) > 0 Then
            If Len(lstr) = 0 Then
              lstr = lsen(i)
            End If
            If lsen(i) <> lstr Then
              allsen = False
              i = UBound(lsen)
            End If
          End If
          i = i + 1
        Wend
        i = 1
        lstr = ""
        While i <= UBound(LLoc)
          If Len(LLoc(i)) > 0 Then
            If Len(lstr) = 0 Then
              lstr = LLoc(i)
            End If
            If LLoc(i) <> lstr Then
              allloc = False
              i = UBound(LLoc)
            End If
          End If
          i = i + 1
        Wend
        i = 1
        lstr = ""
        While i <= UBound(lcon)
          If Len(lcon(i)) > 0 Then
            If Len(lstr) = 0 Then
              lstr = lcon(i)
            End If
            If lcon(i) <> lstr Then
              allcon = False
              i = UBound(lcon)
            End If
          End If
          i = i + 1
        Wend
        Line Input #ifl, istr
        While InStr(istr, "#") > 0 Or Len(istr) = 0
          Print #ofl, istr
          Line Input #ifl, istr
        Wend
        'insert S,L,C comment lines
        i = 0
        While Len(lsen(i)) = 0
          i = i + 1
        Wend
        If allsen Then
          lstr = "# Scenario: * " & lsen(i)
        Else
          lstr = "# Scenario: " & CStr(i) & " " & lsen(i)
          While i < UBound(lsen)
            i = i + 1
            If Len(lsen(i)) > 0 Then
              lstr = lstr & "  " & CStr(i) & " " & lsen(i)
            End If
          Wend
        End If
        Print #ofl, lstr
        i = 0
        While Len(LLoc(i)) = 0
          i = i + 1
        Wend
        If allloc Then
          lstr = "# Location: * " & LLoc(i)
        Else
          lstr = "# Location: " & CStr(i) & " " & LLoc(i)
          While i < UBound(LLoc)
            i = i + 1
            If Len(LLoc(i)) > 0 Then
              lstr = lstr & "  " & CStr(i) & " " & LLoc(i)
            End If
          Wend
        End If
        Print #ofl, lstr
        i = 0
        While Len(lcon(i)) = 0
          i = i + 1
        Wend
        If allcon Then
          lstr = "# Constituent: * " & lcon(i)
        Else
          lstr = "# Constituent: " & CStr(i) & " " & lcon(i)
          While i < UBound(lcon)
            i = i + 1
            If Len(lcon(i)) > 0 Then
              lstr = lstr & "  " & CStr(i) & " " & lcon(i)
            End If
          Wend
        End If
        Print #ofl, lstr
        'echo last line read from old file
        Print #ofl, istr
        'process 2nd header line
        Line Input #ifl, istr
        Print #ofl, istr
      End If
      r.NVALS = 1
      ReDim r.JDate(2000)
      ReDim r.jdflg(2000)
      For j = 0 To r.ColCount - 1
        ReDim r.ColInfo(j).val(2000)
        ReDim r.ColInfo(j).flg(2000)
      Next j
      While Not EOF(ifl)
        Line Input #ifl, istr
        If SLCflg < 3 Then 'echo line to new file
          Print #ofl, istr
        End If
        'process dates
        dstr = istr
        For i = 1 To r.DateField 'forward to date field
          lstr = StrSplit(dstr, Chr(9), "")
        Next i
        'default the date
        ldt(0) = 0
        ldt(1) = 12
        ldt(2) = 31
        ldt(3) = 24
        ldt(4) = 0
        ldt(5) = 0
        'parse the date
        i = 0
        ip = 1
        llen = 4
        While llen <= Len(lstr)
          ldt(i) = CLng(Left(lstr, llen))
          lstr = Mid(lstr, llen + 2)
          ip = ip + llen + 1
          llen = 2 'only year has 4 characters
          i = i + 1
        Wend
        d = Date2J(ldt)
        If d > 0 Then 'valid date
          If r.NVALS >= UBound(r.JDate) Then 'make space larger
            ReDim Preserve r.JDate(r.NVALS + 2000)
            ReDim Preserve r.jdflg(r.NVALS + 2000)
            For j = 0 To r.ColCount - 1
              ReDim Preserve r.ColInfo(j).val(r.NVALS + 2000)
              ReDim Preserve r.ColInfo(j).flg(r.NVALS + 2000)
            Next j
          End If
          r.JDate(r.NVALS) = d
          r.jdflg(r.NVALS) = DateIntrvl(ldt)
        
          'process data
          skip = r.ColInfo(0).Field
          For j = 0 To r.ColCount - 1
            If j > 0 Then
              skip = r.ColInfo(j).Field - r.ColInfo(j - 1).Field
            End If
            For i = 1 To skip
              'lstr = Mid(istr, r.ColInfo(j).ColPos, r.ColInfo(j).FldLen)
              lstr = StrSplit(istr, Chr(9), "")
            Next i
            If IsNumeric(lstr) Then
              d = lstr
            Else
              d = -999 'undef
            End If
            r.ColInfo(j).val(r.NVALS) = d
          Next j
          r.NVALS = r.NVALS + 1
        End If
      Wend
      r.NVALS = r.NVALS - 1
      ReDim Preserve r.JDate(r.NVALS)
      ReDim Preserve r.jdflg(r.NVALS)
      For j = 0 To r.ColCount - 1
        ReDim Preserve r.ColInfo(j).val(r.NVALS)
        ReDim Preserve r.ColInfo(j).flg(r.NVALS)
      Next j
    Else
      pErrorDescription = "Unable to determine Date field in RDB file " & r.NameRDB
    End If
    Close #ifl
    If SLCflg < 3 Then
      Close #ofl
      errflg = 6
      If FileLen("rdb.xxx") > FileLen(r.NameRDB) Then
        'successfl write of new file w/S,L,C
        Kill r.NameRDB
        Name "rdb.xxx" As r.NameRDB
        'Kill "rdb.xxx" this causes error ?? pbd
      Else 'problem with new file
        err.Raise (0)
      End If
    End If
    
    Set pDates = Nothing
    Set pDates = New ATCData.ATCclsTserDate
    
    pDates.flags = r.jdflg
    pDates.Values = r.JDate
    Set pDates.File = Me
    pDates.calcSummary
        
    For j = 0 To r.ColCount - 1
      Set lData = Nothing
      Set lData = New ATCData.ATCclsTserData
      With lData.Header
        .id = 1 + j
        .loc = r.ColInfo(j).Location
        .con = r.ColInfo(j).Constituent
        .Sen = r.ColInfo(j).Scenario
        .desc = "RDB:" & .Sen & ":" & .loc & ":" & .con
      End With
      'lData.Header = lDataHeader
      Set lData.Dates = pDates
      lData.flags = r.ColInfo(j).flg
      lData.Values = r.ColInfo(j).val
      Set lData.File = Me
      lData.calcSummary
      pData.Add lData
    Next j
    
canceldata:
    If pMonitorSet Then
      pMonitor.SendMonitorMessage "(CLOSE)"
      pMonitor.SendMonitorMessage "(BUTTON CANCEL)"
      pMonitor.SendMonitorMessage "(BUTTON PAUSE)"
    End If
    
    Exit Sub

rdberrhand:
    If errflg = 1 Then
      pErrorDescription = "Problem opening specified RDB File " & r.NameRDB
      Exit Sub
    ElseIf errflg = 2 Then
      pErrorDescription = "Problem processing Scenario/Location/Constituent info on RDB File " & r.NameRDB
    ElseIf errflg = 3 Then
      pErrorDescription = "Problem finding Date field on RDB File " & r.NameRDB
    ElseIf errflg = 4 Then
      pErrorDescription = "Problem processing field information on RDB File " & r.NameRDB
    ElseIf errflg = 5 Then
      pErrorDescription = "Problem processing Date values on RDB File " & r.NameRDB
    ElseIf errflg = 6 Then
      pErrorDescription = "Problem updating file " & r.NameRDB & " with Scenario, Location, and Constituent information."
      errflg = 7
    End If
    Resume Next 'not always?

End Sub


Private Function ATCclsTserFile_AddTimSer(t As ATCclsTserData, Optional ExistAction& = 0) As Boolean
  If pDates Is Nothing Then
    Set pDates = t.Dates
  Else
    If t.Dates.serial <> pDates.serial Then
      ATCclsTserFile_AddTimSer = False
      pErrorDescription = "All timeseries in an RDB file must have the same dates."
      Exit Function '------!!!!!!
    End If
  End If
  
  pData.Add t
  
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
  Dim outf As Integer, outLine As String, vTS As Variant
  Dim index As Long, dataIndex As Long
  Dim ldt&(5)
  On Error GoTo ErrWrite
  outf = FreeFile(0)
  Open Filename For Output As #outf
  Print #outf, "#RDB"
  For dataIndex = 1 To pData.Count
    'dataIndex + 1 = column that this dataset is in - column 1 is the date
    Print #outf, "# Scenario: " & dataIndex + 1 & pData(dataIndex).Scen
  Next
  
  For index = 1 To pDates.Summary.NVALS
    J2Date pDates.Value(index), ldt
    outLine = ldt(0) & "/" & ldt(1) & "/" & ldt(2) & " " & ldt(3) & ":" & ldt(4) & vbTab
    For dataIndex = 1 To pData.Count
      outLine = outLine & pData(index).Value(dataIndex) & vbTab
    Next
    Print #outf, outLine
  Next
  Close outf
  
ErrWrite:
  ATCclsTserFile_SaveAs = False
  pErrorDescription = "Save RDB As '" & Filename & "' failed." & vbCr & err.Description
  
End Function
