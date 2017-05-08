Attribute VB_Name = "modBasObsWQ"
Option Explicit

Public Sub SortBasObsWQbyDate(DBFfilename As String)
  Dim readingDBF As New clsDBF
  readingDBF.OpenDBF DBFfilename
  SortBasObsWQbyDateObj readingDBF
  readingDBF.Clear
End Sub

Public Sub SortBasObsWQbyDateObj(readingDBF As clsDBF)
  Dim dateField As Long
  Dim timeField As Long
  Dim jd As Double
  Dim CurrentRecord As Long
  Dim CurrentRecordWriting As Long
  Dim NumRecords As Long
  Dim jDates() As Double
  Dim minDate As Double
  Dim minDateIndex As Long
  Dim inOrder As Boolean
    
  dateField = readingDBF.FieldNumber("DATE")
  timeField = readingDBF.FieldNumber("TIME")

  If dateField < 1 Then
    ' "No DATE field in " & readingDBF.Filename, vbOKOnly, "SortBasObsWQbyDate"
  ElseIf timeField < 1 Then
    ' "No TIME field in " & readingDBF.Filename, vbOKOnly, "SortBasObsWQbyDate"
  Else
    NumRecords = readingDBF.NumRecords
    minDate = 999999999
    inOrder = True
    ReDim jDates(NumRecords)
    For CurrentRecord = 1 To NumRecords
      readingDBF.CurrentRecord = CurrentRecord
      'calc julian date from date and time fields
      jd = parseWQObsDate(readingDBF.Value(dateField), readingDBF.Value(timeField))
      jDates(CurrentRecord) = jd
      If jd > 0 Then
        If jd < minDate Then
          If minDate < 999999999 Then inOrder = False
          minDate = jd
          minDateIndex = CurrentRecord
        End If
      End If
    Next
    
    If Not inOrder Then
      Dim sortedDBF As clsDBF
      Set sortedDBF = readingDBF.Cousin
      sortedDBF.NumRecords = readingDBF.NumRecords
      sortedDBF.InitData
      CurrentRecordWriting = 0
      While minDate < 999999999
        jd = minDate
        jDates(minDateIndex) = 0 'So we don't find this one as the next date again
        readingDBF.CurrentRecord = minDateIndex
        CurrentRecordWriting = CurrentRecordWriting + 1
        sortedDBF.CurrentRecord = CurrentRecordWriting
        sortedDBF.record = readingDBF.record
        
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
      sortedDBF.WriteDBF readingDBF.Filename
      sortedDBF.Clear
    End If
  End If
End Sub

'The two BASINS Observed Water Quality DBFs must have exactly the same format
'This can be assured by creating one as a .Cousin of the other
'The merged DBF will be written to destDBF.Filename
'Returns empty string on success, error message on failure
Public Function MergeBasObsWQ(destDBF As clsDBF, sourceDBF As clsDBF) As String
  Dim mergedDBF As clsDBF
  Dim dateField As Long
  Dim timeField As Long
  Dim CurrentRecordSource As Long
  Dim CurrentRecordDest As Long
  Dim CurrentRecordMerged As Long
  Dim CurrentSourceDate As Double
  Dim CurrentDestDate As Double
  Dim RecordContents() As Byte

  If sourceDBF.numFields <> destDBF.numFields Then GoTo NonMatchingFields
  For dateField = 1 To sourceDBF.numFields
    If sourceDBF.fieldName(dateField) <> destDBF.fieldName(dateField) Then GoTo NonMatchingFields
    If sourceDBF.FieldType(dateField) <> destDBF.FieldType(dateField) Then GoTo NonMatchingFields
    If sourceDBF.FieldLength(dateField) <> destDBF.FieldLength(dateField) Then GoTo NonMatchingFields
  Next
    
  dateField = destDBF.FieldNumber("DATE")
  timeField = destDBF.FieldNumber("TIME")
  
  CurrentRecordSource = 1
  sourceDBF.CurrentRecord = CurrentRecordSource
  destDBF.CurrentRecord = destDBF.NumRecords
  CurrentSourceDate = parseWQObsDate(sourceDBF.Value(dateField), sourceDBF.Value(timeField))
  CurrentDestDate = parseWQObsDate(destDBF.Value(dateField), destDBF.Value(timeField))

  If CurrentSourceDate >= CurrentDestDate Then
    'We can just append all records from source onto dest
    destDBF.NumRecords = destDBF.NumRecords + sourceDBF.NumRecords
    CurrentRecordMerged = destDBF.CurrentRecord
    For CurrentRecordSource = 1 To sourceDBF.NumRecords
      sourceDBF.CurrentRecord = CurrentRecordSource
      destDBF.CurrentRecord = CurrentRecordDest
      destDBF.record = sourceDBF.record
      CurrentRecordDest = CurrentRecordDest + 1
    Next
  Else
    CurrentRecordDest = 1
    destDBF.CurrentRecord = CurrentRecordDest
    CurrentDestDate = parseWQObsDate(destDBF.Value(dateField), destDBF.Value(timeField))
    Set mergedDBF = destDBF.Cousin
    mergedDBF.NumRecords = sourceDBF.NumRecords + destDBF.NumRecords
    mergedDBF.InitData
    
    mergedDBF.CurrentRecord = 1
    If CurrentDestDate > CurrentSourceDate Then
      mergedDBF.record = sourceDBF.record
      GoSub NextSourceRecord
    ElseIf CurrentDestDate < CurrentSourceDate Then
      mergedDBF.record = destDBF.record
      GoSub NextDestRecord
    Else 'CurrentDestDate = CurrentSourceDate
      RecordContents = sourceDBF.record
      mergedDBF.record = RecordContents
      GoSub NextSourceRecord
      If destDBF.MatchRecord(RecordContents) Then
        'Skip duplicate record from destDBF
        GoSub NextDestRecord
      End If
    End If
  
FinishedMerging:
    
  End If
  
  mergedDBF.WriteDBF destDBF.Filename
  
  Exit Function
  
NextSourceRecord:
  If sourceDBF.CurrentRecord = sourceDBF.NumRecords Then
    CurrentSourceDate = 9E+18
    If CurrentSourceDate = CurrentDestDate Then GoTo FinishedMerging
  Else
    sourceDBF.CurrentRecord = sourceDBF.CurrentRecord + 1
    CurrentSourceDate = parseWQObsDate(sourceDBF.Value(dateField), sourceDBF.Value(timeField))
  End If
  Return

NextDestRecord:
  If destDBF.CurrentRecord = destDBF.NumRecords Then
    CurrentDestDate = 9E+18
    If CurrentDestDate = CurrentSourceDate Then GoTo FinishedMerging
  Else
    destDBF.CurrentRecord = destDBF.CurrentRecord + 1
    CurrentDestDate = parseWQObsDate(destDBF.Value(dateField), destDBF.Value(timeField))
  End If
  Return

NonMatchingFields:
  MergeBasObsWQ = "Fields for BASINS Observed Water Quality DBFs do not match, so they cannot be merged"
  Exit Function
End Function

'Expand date field of old BasObsWQ files from two-digit years (6-digit dates) to 4-digit years (8-digit dates)
Public Function ReformatBasObsWQ(ByRef aDBF As clsDBF) As clsDBF
  Dim oldFieldNumber() As Long
  Dim iDateField As Long
  Dim iNewField As Long
  Dim iRecord As Long
  Dim retval As clsDBF
  Set retval = NewBasObsWQdbf
  ReDim oldFieldNumber(retval.numFields)
  
  For iNewField = 1 To retval.numFields
    oldFieldNumber(iNewField) = aDBF.FieldNumber(retval.fieldName(iNewField))
    If UCase(retval.fieldName(iNewField)) = "DATE" Then iDateField = iNewField
    'Expand field length if old DBF had a wider field
    If aDBF.FieldLength(oldFieldNumber(iNewField)) > retval.FieldLength(iNewField) Then
      retval.FieldLength(iNewField) = aDBF.FieldLength(oldFieldNumber(iNewField))
    End If
  Next
  retval.NumRecords = aDBF.NumRecords
  retval.InitData
  
  For iRecord = 1 To aDBF.NumRecords
    retval.CurrentRecord = iRecord
    aDBF.CurrentRecord = iRecord
    For iNewField = 1 To retval.numFields
      retval.Value(iNewField) = aDBF.Value(oldFieldNumber(iNewField))
      If iNewField = iDateField And Len(retval.Value(iNewField)) = 6 Then
        retval.Value(iNewField) = "19" & retval.Value(iNewField)
      End If
    Next
  Next
  Set ReformatBasObsWQ = retval
End Function

Public Function NewBasObsWQdbf(Optional aNumRecords As Long = 0) As clsDBF
  Set NewBasObsWQdbf = New clsDBF
  With NewBasObsWQdbf
    .Year = CInt(Format(Now, "yyyy")) - 1900
    .Month = CByte(Format(Now, "mm"))
    .Day = CByte(Format(Now, "dd"))
    .numFields = 10
  
    .fieldName(1) = "BSTAT_ID"
    .FieldType(1) = "C"
    .FieldLength(1) = 28
    .FieldDecimalCount(1) = 0
  
    .fieldName(2) = "ID_SAMP"
    .FieldType(2) = "C"
    .FieldLength(2) = 10
    .FieldDecimalCount(2) = 0
  
    .fieldName(3) = "ID"
    .FieldType(3) = "C"
    .FieldLength(3) = 5
    .FieldDecimalCount(3) = 0
  
    .fieldName(4) = "TIME"
    .FieldType(4) = "C"
    .FieldLength(4) = 4
    .FieldDecimalCount(4) = 0
  
    .fieldName(5) = "DEPTH"
    .FieldType(5) = "N"
    .FieldLength(5) = 20
    .FieldDecimalCount(5) = 5
  
    .fieldName(6) = "PARM"
    .FieldType(6) = "C"
    .FieldLength(6) = 5
    .FieldDecimalCount(6) = 0
  
    .fieldName(7) = "REMARK"
    .FieldType(7) = "C"
    .FieldLength(7) = 1
    .FieldDecimalCount(7) = 0
  
    .fieldName(8) = "VALUE"
    .FieldType(8) = "N"
    .FieldLength(8) = 20
    .FieldDecimalCount(8) = 5
  
    .fieldName(9) = "BCU"
    .FieldType(9) = "C"
    .FieldLength(9) = 8
    .FieldDecimalCount(9) = 0
  
    .fieldName(10) = "DATE"
    .FieldType(10) = "C"
    .FieldLength(10) = 8
    .FieldDecimalCount(10) = 0
  
    .NumRecords = aNumRecords
    .InitData
  End With
End Function

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

