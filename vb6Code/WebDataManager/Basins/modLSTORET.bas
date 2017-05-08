Attribute VB_Name = "modLSTORET"
Option Explicit

Public Sub ConvertLSTORETtoShapeDBF(staFilename As String, existingDBFname As String)
  Dim iRecord As Long
  Dim newDBF As clsDBF
  Dim staTSV As clsCSV
  Dim existingDBF As clsDBF
  
  Set staTSV = New clsCSV
  staTSV.Delimiter = vbTab
  staTSV.quote = ""
  staTSV.OpenCSV staFilename
  
  If FileExists(existingDBFname) Then
    Set existingDBF = New clsDBF
    existingDBF.OpenDBF existingDBFname
    Set newDBF = existingDBF.Cousin
  Else
    Set newDBF = NewWqobsStationDBF
  End If
  With newDBF
    .NumRecords = staTSV.NumRecords
    .InitData
    
    For iRecord = 1 To .NumRecords
      .CurrentRecord = iRecord
      staTSV.CurrentRecord = iRecord
      .Value(2) = staTSV.Value(1)  'Agency -> AGENCY
      .Value(4) = staTSV.Value(2)  'Station -> STATION
      .Value(5) = staTSV.Value(14) 'Station Depth -> ST_DEPTH
      .Value(6) = Left(staTSV.Value(4), 2) 'State FIPS -> STATE
      .Value(7) = staTSV.Value(5)  'Latitude -> LAT
      .Value(8) = staTSV.Value(6)  'Longitude -> LONG
      .Value(9) = staTSV.Value(13) 'Station Type -> TYPE
      .Value(10) = staTSV.Value(3) 'Station Name -> LOCATION
      .Value(11) = staTSV.Value(7) 'huc -> CU
      .Value(12) = staTSV.Value(8) 'Rchmile Segment -> SEG
      .Value(13) = staTSV.Value(9) 'Miles Up Reach -> MILE
      Select Case staTSV.Value(10) 'Rchonoff -> ONOFF
        Case "0": .Value(14) = "OFF"
        Case "1": .Value(14) = "ON"
      End Select
      .Value(15) = staTSV.Value(7)  'huc -> BCU
      .Value(16) = .Value(4) & "+" & .Value(2)
    Next
    .WriteDBF FilenameNoExt(staFilename) & ".dbf"
  End With
End Sub

Public Function ConvertLSTORETtoDataDBF(resFilename As String, Optional CousinDBF As String = "") As clsDBF
  Dim iRecord As Long
  Dim newDBF As clsDBF
'  Dim resTSV As clsCSV
  Dim inFile As Integer
  Dim inField As Long
  Dim inValue(11) As String
  Dim fieldBSTAT_ID As Long
  Dim fieldTIME As Long
  Dim fieldDEPTH As Long
  Dim fieldPARAM As Long
  Dim fieldR As Long
  Dim fieldVALUE As Long
  Dim fieldBCU As Long
  Dim fieldDATE As Long
  Dim curLine As String
  
'  Set resTSV = New clsCSV
'  resTSV.Delimiter = vbTab
'  resTSV.quote = ""
'  resTSV.OpenCSV resFilename

  If Len(CousinDBF) > 0 Then
    If FileExists(CousinDBF) Then
      Dim iField As Long
      Dim oldDBF As clsDBF
      Set oldDBF = New clsDBF
      oldDBF.OpenDBF CousinDBF
      Set newDBF = oldDBF.Cousin
      
      fieldDATE = oldDBF.FieldNumber("DATE")
      If oldDBF.FieldLength(fieldDATE) < 8 Then 'Expand date field if old file used 2-digit years
        newDBF.FieldLength(fieldDATE) = 8
        newDBF.NumRecords = oldDBF.NumRecords
        newDBF.InitData
        For iRecord = 1 To newDBF.NumRecords
          oldDBF.CurrentRecord = iRecord
          newDBF.CurrentRecord = iRecord
          For iField = 1 To newDBF.numFields
            If iField = fieldDATE Then
              newDBF.Value(iField) = "19" & oldDBF.Value(iField)
            Else
              newDBF.Value(iField) = oldDBF.Value(iField)
            End If
          Next
        Next
        newDBF.WriteDBF CousinDBF
        newDBF.NumRecords = 0
      End If

      oldDBF.Clear
      Set oldDBF = Nothing
    End If
  End If
  If newDBF Is Nothing Then
    Set newDBF = NewBasObsWQdbf
  End If
  
  With newDBF
    .NumRecords = 1000 'resTSV.NumRecords
    .InitData
    
    fieldBSTAT_ID = .FieldNumber("BSTAT_ID")
    fieldTIME = .FieldNumber("TIME")
    fieldDEPTH = .FieldNumber("DEPTH")
    fieldPARAM = .FieldNumber("PARM")
    fieldR = .FieldNumber("REMARK")
    If fieldR = 0 Then fieldR = .FieldNumber("R")
    fieldVALUE = .FieldNumber("VALUE")
    fieldBCU = .FieldNumber("BCU")
    fieldDATE = .FieldNumber("DATE")
    
    If fieldBSTAT_ID = 0 Then
      ' "No BSTAT_ID"
    ElseIf fieldTIME = 0 Then
      ' "No TIME"
    ElseIf fieldDEPTH = 0 Then
      ' "No DEPTH"
    ElseIf fieldPARAM = 0 Then
      ' "No PARAM"
    ElseIf fieldR = 0 Then
      ' "No R"
    ElseIf fieldVALUE = 0 Then
      ' "No VALUE"
    ElseIf fieldBCU = 0 Then
      ' "No BCU"
    ElseIf fieldDATE = 0 Then
      ' "No DATE"
    Else
      inFile = FreeFile
      Open resFilename For Input As inFile
      Line Input #inFile, curLine 'Read header line
      iRecord = 1
      While Not EOF(inFile)
        Line Input #inFile, curLine
        For inField = 1 To 11
          inValue(inField) = StrSplit(curLine, vbTab, "")
        Next
        If iRecord > .NumRecords Then
          .NumRecords = .NumRecords * 1.5
        End If
        .CurrentRecord = iRecord
        .Value(fieldBSTAT_ID) = inValue(2) & "+" & inValue(1) 'Station + Agency -> BSTAT_ID
        .Value(fieldTIME) = inValue(8) 'Start Time -> TIME
        .Value(fieldDEPTH) = inValue(11) 'Sample Depth -> DEPTH
        .Value(fieldPARAM) = inValue(6)  'Param -> PARAM
        .Value(fieldR) = inValue(4)      'R -> R
        .Value(fieldVALUE) = inValue(3)  'Result Value -> VALUE
        .Value(fieldBCU) = inValue(5)  'HUC -> BCU
        .Value(fieldDATE) = ReplaceString(inValue(7), "-", "") 'Start Date -> DATE
        iRecord = iRecord + 1
      Wend
      Close inFile
      .NumRecords = .CurrentRecord
      Set ConvertLSTORETtoDataDBF = newDBF
    End If
  End With
End Function

Private Function NewWqobsStationDBF() As clsDBF
  Set NewWqobsStationDBF = New clsDBF
  With NewWqobsStationDBF
    .Year = CInt(Format(Now, "yyyy")) - 1900
    .Month = CByte(Format(Now, "mm"))
    .Day = CByte(Format(Now, "dd"))
    .numFields = 16
  
    .fieldName(1) = "ID"
    .FieldType(1) = "C"
    .FieldLength(1) = 5
    .FieldDecimalCount(1) = 0
  
    .fieldName(2) = "AGENCY"
    .FieldType(2) = "C"
    .FieldLength(2) = 8
    .FieldDecimalCount(2) = 0
  
    .fieldName(3) = "AGENCY_COD"
    .FieldType(3) = "N"
    .FieldLength(3) = 16
    .FieldDecimalCount(3) = 0
  
    .fieldName(4) = "STATION"
    .FieldType(4) = "C"
    .FieldLength(4) = 15
    .FieldDecimalCount(4) = 0
  
    .fieldName(5) = "ST_DEPTH"
    .FieldType(5) = "N"
    .FieldLength(5) = 12
    .FieldDecimalCount(5) = 0
  
    .fieldName(6) = "STATE"
    .FieldType(6) = "C"
    .FieldLength(6) = 2
    .FieldDecimalCount(6) = 0
  
    .fieldName(7) = "LAT"
    .FieldType(7) = "N"
    .FieldLength(7) = 12
    .FieldDecimalCount(7) = 5
  
    .fieldName(8) = "LONG"
    .FieldType(8) = "N"
    .FieldLength(8) = 12
    .FieldDecimalCount(8) = 5
  
    .fieldName(9) = "TYPE"
    .FieldType(9) = "C"
    .FieldLength(9) = 60
    .FieldDecimalCount(9) = 0
  
    .fieldName(10) = "LOCATION"
    .FieldType(10) = "C"
    .FieldLength(10) = 48
    .FieldDecimalCount(10) = 0
  
    .fieldName(11) = "CU"
    .FieldType(11) = "C"
    .FieldLength(11) = 8
    .FieldDecimalCount(11) = 0
  
    .fieldName(12) = "SEG"
    .FieldType(12) = "C"
    .FieldLength(12) = 3
    .FieldDecimalCount(12) = 0
  
    .fieldName(13) = "MILE"
    .FieldType(13) = "N"
    .FieldLength(13) = 12
    .FieldDecimalCount(13) = 0
  
    .fieldName(14) = "ONOFF"
    .FieldType(14) = "C"
    .FieldLength(14) = 3
    .FieldDecimalCount(14) = 0
   
    .fieldName(15) = "BCU"
    .FieldType(15) = "C"
    .FieldLength(15) = 8
    .FieldDecimalCount(15) = 0
   
    .fieldName(16) = "BSTAT_ID"
    .FieldType(16) = "C"
    .FieldLength(16) = 28
    .FieldDecimalCount(16) = 0
  End With
End Function

