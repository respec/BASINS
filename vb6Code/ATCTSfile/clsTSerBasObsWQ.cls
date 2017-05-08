VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsTSerBasObsWQ"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'##MODULE_REMARKS Copyright 2001-3 AQUA TERRA Consultants - Royalty-free use permitted under open source license
Implements ATCclsTserFile

Private pFileExt As String
Private pErrorDescription As String
Private pMonitor As Object
Private pMonitorSet As Boolean
Private pData As Collection
Private pDates As Collection

Private Type DBFTimSer
  Cons As String
  Scen As String
  Locn As String
  Count As Long
  Values() As Single
  flags() As Long
  jd() As Double
  jdflg() As Long
End Type

Private Type DBF
  Filename As String
  TSInfo() As DBFTimSer 'info for each data field
  index As FastCollection 'of keys "locn:cons"
  Count As Long
  DateField As Long 'field for date values
  TimeField As Long 'field for time values
  LocnField As Long 'field containing location info
  ConsField As Long 'field containing parm information
  ValuField As Long 'field containing data value
End Type
Private pDBF As DBF

Private Property Get ATCclsTserFile_AvailableAttributes() As Collection
  Set ATCclsTserFile_AvailableAttributes = New Collection
End Property

Private Sub ATCclsTserFile_clear()
  Set pData = Nothing
  Set pDates = Nothing
  ReDim pDBF.TSInfo(0)
  Set pDBF.index = New FastCollection
End Sub

Private Property Get ATCclsTserFile_Data(index As Long) As ATCData.ATCclsTserData
  Set ATCclsTserFile_Data = pData(index)
End Property

Private Property Get ATCclsTserFile_DataCount() As Long
  ATCclsTserFile_DataCount = pDBF.Count
End Property

Private Property Get ATCclsTserfile_DataCollection() As Collection
  Set ATCclsTserfile_DataCollection = pData
End Property

Private Property Get ATCclsTserFile_FileUnit() As Long

End Property

Private Property Let ATCclsTserFile_HelpFilename(ByVal newValue As String)
  App.HelpFile = newValue
End Property

Private Sub ATCclsTserFile_readData(dataObject As ATCclsTserData)

End Sub

Private Sub ATCclsTserFile_refresh()

End Sub

Private Function ATCclsTserFile_writeDataHeader(dataObject As ATCclsTserData, Optional oldHeader As ATTimSerDataHeader) As Boolean
  pErrorDescription = "Unable to Write a Data Header for Class BasObsWQ"
  ATCclsTserFile_writeDataHeader = False
End Function

Private Sub Class_Initialize()
  pDBF.Filename = "<unknown>"
  ReDim pDBF.TSInfo(0)
'  ReDim pDBF.LocnUnique(0)
'  ReDim pDBF.ConsUnique(0)
  pFileExt = "Obs WQ Files (*.dbf)|*.dbf|All Files (*.*)|*.*"
  pErrorDescription = ""
  pMonitorSet = False
  
  Set pData = Nothing
  Set pData = New Collection
  
  Set pDates = Nothing
  Set pDates = New Collection
End Sub

Private Property Get ATCclsTserFile_Label() As String
  ATCclsTserFile_Label = "BasObsWQ"
End Property

Private Property Set ATCclsTserFile_Monitor(ByVal Monitor As Object)
  Set pMonitor = Monitor
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
  ATCclsTserFile_FileName = pDBF.Filename
End Property

Private Property Let ATCclsTserFile_FileName(ByVal newValue$)
  If Len(Dir(newValue)) = 0 Then
    pErrorDescription = "FileName '" & newValue & "' not found"
  Else
    pDBF.Filename = newValue
    Call DBFRead
  End If
End Property

Private Property Get ATCclsTserFile_Description() As String
  ATCclsTserFile_Description = "Basins Observed Water Quality DBF Files"
End Property

Private Sub DBFRead()
  Dim lData As ATCData.ATCclsTserData
  Dim lDates As ATCData.ATCclsTserDate
  Dim i&, l&, c&, j&, v!, s$, t$
  Dim jd As Double
  Dim readingDBF As clsATCTable
  Dim CurrentRecord As Long
  Dim NumRecords As Long
  Dim Pname$, pUnits$, pDesc$
  Dim vFieldVal As Variant
  Dim jDates() As Double
  Dim minDate As Double
  Dim minDateIndex As Long
  Dim inOrder As Boolean
  Dim recordsRead As Long
  
  'On Error GoTo dbferrhandler
  If pMonitorSet Then
     pMonitor.SendMonitorMessage "(OPEN BASINS OBS WQ DBF File)"
     pMonitor.SendMonitorMessage "(BUTTOFF CANCEL)"
     pMonitor.SendMonitorMessage "(BUTTOFF PAUSE)"
     pMonitor.SendMonitorMessage "(MSG1 " & pDBF.Filename & ")"
  End If
  
  Debug.Print "OpenDBF" & Now
  Set readingDBF = New clsATCTableDBF
  readingDBF.OpenFile pDBF.Filename
  
  pDBF.ConsField = -1
  pDBF.DateField = -1
  pDBF.TimeField = -1
  pDBF.LocnField = -1
  Set pDBF.index = New FastCollection
  
  For i = 1 To readingDBF.NumFields 'look for date and location fields
    s = UCase(readingDBF.FieldName(i))
    If s = "DATE" Then
      pDBF.DateField = i
    ElseIf s = "TIME" Then
      pDBF.TimeField = i
    ElseIf InStr(s, "ID") Then 'location
      If pDBF.LocnField = -1 Then 'only use first one
        'should be sure that field is in use here
        pDBF.LocnField = i
      End If
    ElseIf s = "PARM" Then
      pDBF.ConsField = i
    ElseIf s = "VALUE" Then
      pDBF.ValuField = i
    End If
  Next i
    
  If pDBF.DateField = -1 Or pDBF.ConsField = -1 Or pDBF.ValuField = -1 Or _
     pDBF.TimeField = -1 Or pDBF.LocnField = -1 Then
    pErrorDescription = ""
    If pDBF.DateField = -1 Then pErrorDescription = pErrorDescription & "DATE, "
    If pDBF.ConsField = -1 Then pErrorDescription = pErrorDescription & "PARM, "
    If pDBF.ValuField = -1 Then pErrorDescription = pErrorDescription & "VALUE, "
    If pDBF.TimeField = -1 Then pErrorDescription = pErrorDescription & "TIME, "
    If pDBF.LocnField = -1 Then pErrorDescription = pErrorDescription & "ID, "
    pErrorDescription = Left(pErrorDescription, Len(pErrorDescription) - 2)
    pErrorDescription = pErrorDescription & " Not Defined in File " & pDBF.Filename
  Else
    pDBF.Count = 0
    ReDim pDBF.TSInfo(0)
    
    NumRecords = readingDBF.NumRecords
    minDate = 999999999
    inOrder = True
    ReDim jDates(NumRecords)
    pMonitor.SendMonitorMessage "(MSG2 Reading dates)"
    Debug.Print "ReadDates" & Now
    For CurrentRecord = 1 To NumRecords
      readingDBF.CurrentRecord = CurrentRecord
      'calc julian date from date and time fields
      s = readingDBF.Value(pDBF.DateField)
      t = readingDBF.Value(pDBF.TimeField)
      If Not IsNumeric(t) Then t = "1200" 'assume noon for missing obstime
      jd = parseWQObsDate(s, t)
      jDates(CurrentRecord) = jd
      If jd > 0 Then
        If jd < minDate Then
          If minDate < 999999999 Then inOrder = False
          minDate = jd
          minDateIndex = CurrentRecord
        End If
      End If
      If CurrentRecord Mod 250 = 0 Then ' update progress message
        If pMonitorSet Then
          s = "(PROGRESS " & CStr(100 * CurrentRecord / NumRecords) & ")"
          pMonitor.SendMonitorMessage s
        End If
      End If
    Next
    
    Debug.Print "ProcessData" & Now
    CurrentRecord = minDateIndex
    While minDate < 999999999
      jd = minDate
      jDates(minDateIndex) = 0 'So we don't find this one as the next date again
      readingDBF.CurrentRecord = minDateIndex
      recordsRead = recordsRead + 1
      If recordsRead Mod 250 = 0 Then ' update progress message
        If pMonitorSet Then
          s = "(PROGRESS " & CStr(100 * recordsRead / NumRecords) & ")"
          pMonitor.SendMonitorMessage s
        End If
      End If
            
      i = pDBF.index.IndexFromKey(readingDBF.Value(pDBF.ConsField) & ":" & readingDBF.Value(pDBF.LocnField))
      
      If i < 0 Then 'need to add
        If pDBF.Count Mod 25 = 0 Then
          ReDim Preserve pDBF.TSInfo(pDBF.Count + 25)
        End If
        pDBF.Count = pDBF.Count + 1 'this could use cound of index
        With pDBF.TSInfo(pDBF.Count)
          .Cons = readingDBF.Value(pDBF.ConsField)
          .Locn = readingDBF.Value(pDBF.LocnField)
          .Scen = "OBSERVED"
          ReDim .Values(25)
          ReDim .flags(25)
          ReDim .jd(25)
          ReDim .jdflg(25)
          .jd(1) = jd
          .jdflg(1) = JDateIntrvl(jd)
          pDBF.index.Add i, .Cons & ":" & .Locn
          vFieldVal = readingDBF.Value(pDBF.ValuField)
          If IsNumeric(vFieldVal) Then
            v = vFieldVal
          Else
            If pMonitorSet Then
               pMonitor.SendMonitorMessage "(DBG Non-numeric value in record " & CurrentRecord & " = '" & vFieldVal & "')"
            End If
            v = -999
          End If
          .Values(1) = v
          .flags(1) = 0
          .Count = 1
        End With
      Else 'already have this one
        If jd < 1000 Then
          MsgBox "Bad Date: " & s & ":" & t
        Else
          With pDBF.TSInfo(i)
            j = .Count + 1
            If j > UBound(.Values) Then
              ReDim Preserve .Values(j + 25)
              ReDim Preserve .flags(j + 25)
              ReDim Preserve .jd(j + 25)
              ReDim Preserve .jdflg(j + 25)
            End If
            .jd(j) = jd
            .jdflg(j) = JDateIntrvl(jd)
            vFieldVal = readingDBF.Value(pDBF.ValuField)
            If IsNumeric(vFieldVal) Then
              v = vFieldVal
            Else
              If pMonitorSet Then
                pMonitor.SendMonitorMessage "(DBG Non-numeric value in record " & CurrentRecord & " = '" & vFieldVal & "')"
              End If
              v = -999
            End If
            .Values(j) = v
            .flags(j) = 0
            .Count = j
          End With
        End If
      End If
      
      minDate = 999999999
      If inOrder Then 'Don't have to search for minimum, just find next non-zero
        For CurrentRecord = CurrentRecord + 1 To NumRecords
          jd = jDates(CurrentRecord)
          If jd > 0 Then
            minDate = jd
            minDateIndex = CurrentRecord
            Exit For
          End If
        Next
      Else 'Search for minimum date
        For CurrentRecord = 1 To NumRecords
          jd = jDates(CurrentRecord)
          If jd > 0 Then
            If jd < minDate Then
              minDate = jd
              minDateIndex = CurrentRecord
            End If
          End If
        Next
      End If
    Wend
  End If
  
  If pDBF.Count > 0 Then
    Dim ParmCodesExist As Boolean
    Dim lParmCodeTable As clsATCTable
    
    Set lParmCodeTable = ParmCodeTable
    If lParmCodeTable Is Nothing Then
      ParmCodesExist = False
    Else
      ParmCodesExist = True
    End If

    Debug.Print "FillInAttributes" & Now
    ReDim Preserve pDBF.TSInfo(pDBF.Count)
    For i = 1 To pDBF.Count
      pDBF.TSInfo(i).Count = pDBF.TSInfo(i).Count
      ReDim Preserve pDBF.TSInfo(i).jd(pDBF.TSInfo(i).Count)
      ReDim Preserve pDBF.TSInfo(i).jdflg(pDBF.TSInfo(i).Count)
      Set lDates = Nothing
      Set lDates = New ATCData.ATCclsTserDate
      lDates.flags = pDBF.TSInfo(i).jdflg
      lDates.Values = pDBF.TSInfo(i).jd
      Set lDates.File = Me
      lDates.calcSummary
      pDates.Add lDates
      
      ReDim Preserve pDBF.TSInfo(i).Values(pDBF.TSInfo(i).Count)
      ReDim Preserve pDBF.TSInfo(i).flags(pDBF.TSInfo(i).Count)
      Set lData = Nothing
      Set lData = New ATCData.ATCclsTserData
      With lData.Header
        .ID = i
        .loc = pDBF.TSInfo(i).Locn
        If ParmCodesExist And IsNumeric(pDBF.TSInfo(i).Cons) Then
          'save parmcode as attribute
          lData.AttribSet "ParmCode", pDBF.TSInfo(i).Cons
          If Not (FindParmCode(CLng(pDBF.TSInfo(i).Cons), lParmCodeTable)) Then
            .con = pDBF.TSInfo(i).Cons
          Else
            .con = lParmCodeTable.Value(2)
            For j = 3 To lParmCodeTable.NumFields
              If Len(Trim(lParmCodeTable.Value(j))) > 0 Then
                lData.AttribSet lParmCodeTable.FieldName(j), lParmCodeTable.Value(j)
              End If
            Next j
          End If
        Else
          .con = pDBF.TSInfo(i).Cons
        End If
        .Sen = pDBF.TSInfo(i).Scen
        .desc = "BasObsWQ:" & .Sen & ":" & .loc & ":" & .con
      End With
      Set lData.Dates = lDates
      lData.Values = pDBF.TSInfo(i).Values
      lData.flags = pDBF.TSInfo(i).flags
      lData.calcSummary
      Set lData.File = Me
      pData.Add lData
    Next i
  End If
  Debug.Print "DoneDBFRead" & Now
cancel:
  If Not readingDBF Is Nothing Then
    readingDBF.Clear
    Set readingDBF = Nothing
  End If
  
  If pMonitorSet Then
     pMonitor.SendMonitorMessage "(CLOSE)"
     pMonitor.SendMonitorMessage "(BUTTON CANCEL)"
     pMonitor.SendMonitorMessage "(BUTTON PAUSE)"
  End If
  
  Exit Sub
dbferrhandler:
  pErrorDescription = err.Description
  If pMonitorSet Then pMonitor.SendMonitorMessage "(DBG " & pErrorDescription & ")"
  GoTo cancel
End Sub

Private Function parseWQObsDate(s As String, t As String) As Double
  'assume point values at specified time
  Dim d(5) As Long 'date array
  Dim l As Long 'Length of year (2 or 4 digit year)
  Dim i As Long 'Year offset (1900 for 2-digit year)

  If IsNumeric(s) Then
    If Len(s) = 8 Then ' 4 dig yr
      l = 4
      i = 0
    Else
      l = 2
      i = 1900
    End If
    d(0) = Left(s, l) + i
    d(1) = Mid(s, l + 1, 2)
    d(2) = Right(s, 2)
    If IsNumeric(t) Then
      d(3) = Left(t, 2)
      d(4) = Right(t, 2)
    End If
    parseWQObsDate = Date2J(d)
  Else
    parseWQObsDate = 0
  End If
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
