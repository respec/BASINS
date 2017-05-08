Attribute VB_Name = "modPCSshape"
Option Explicit

'Const pBaseDir = "c:\BasinsDownload Tutorial\"
'Const pBaseDirCore = pBaseDir & "BasinsCore\PCS3\"
'Const pBaseDirPCS = pBaseDir & "PCS Discharge\"
'Const pBaseDirPCSPermit = pBaseDir & "PCS Permit\"

Private Const pCSVprefix = "PCS_PERMIT_FACILITY."

'Sub test(DBFname As String)
'  Dim lDbf As New clsDBF
'  With lDbf
'    .OpenDBF DBFname
'    SaveFileString "c:\test.bas", .CreationCode()
'    .Clear
'  End With
'End Sub

Public Sub WritePCSShape(aCSV As clsCSV, ByVal newBaseFilename As String)
  Dim CSVfield(55) As Long
  Dim CSValue As String
  Dim record As Long
  Dim field As Long
  Dim errRecord As Long
  Dim XYPoint As T_shpXYPoint
  Dim newSHP As CShape_IO
  Dim newDBF As clsDBF
  Set newDBF = New clsDBF
  With newDBF
    .Year = CInt(Format(Now, "yyyy")) - 1900
    .Month = CInt(Format(Now, "mm"))
    .Day = CInt(Format(Now, "dd"))
    .numFields = 55
  
    .FieldName(1) = "NPDES":     CSVfield(1) = aCSV.FieldNumber("NPDES")
    .FieldType(1) = "C"
    .FieldLength(1) = 9
    .FieldDecimalCount(1) = 0
  
    .FieldName(2) = "FAC_NAME":  CSVfield(2) = aCSV.FieldNumber("NAME_1")
    .FieldType(2) = "C"
    .FieldLength(2) = 30
    .FieldDecimalCount(2) = 0
  
    .FieldName(3) = "OWNERSHIP": CSVfield(3) = aCSV.FieldNumber("TYPE_OF_OWNERSHIP")
    .FieldType(3) = "C"
    .FieldLength(3) = 9
    .FieldDecimalCount(3) = 0
  
    .FieldName(4) = "NEW_STATUS"
    .FieldType(4) = "C"
    .FieldLength(4) = 9
    .FieldDecimalCount(4) = 0
  
    .FieldName(5) = "STREAM_CLS"
    .FieldType(5) = "C"
    .FieldLength(5) = 9
    .FieldDecimalCount(5) = 0
  
    .FieldName(6) = "NEW_DATE"
    .FieldType(6) = "C"
    .FieldLength(6) = 9
    .FieldDecimalCount(6) = 0
  
    .FieldName(7) = "PRETREAT"
    .FieldType(7) = "C"
    .FieldLength(7) = 9
    .FieldDecimalCount(7) = 0
  
    .FieldName(8) = "FAC_UPDATE"
    .FieldType(8) = "C"
    .FieldLength(8) = 9
    .FieldDecimalCount(8) = 0
  
    .FieldName(9) = "REC_POTW"
    .FieldType(9) = "C"
    .FieldLength(9) = 9
    .FieldDecimalCount(9) = 0
  
    .FieldName(10) = "PCS_LAT": CSVfield(10) = aCSV.FieldNumber("LATITUDE")
    .FieldType(10) = "C"
    .FieldLength(10) = 16
    .FieldDecimalCount(10) = 0
  
    .FieldName(11) = "PCS_LONG": CSVfield(11) = aCSV.FieldNumber("LONGITUDE")
    .FieldType(11) = "C"
    .FieldLength(11) = 15
    .FieldDecimalCount(11) = 0
  
    .FieldName(12) = "LATLONG_AC": CSVfield(12) = aCSV.FieldNumber(pCSVprefix & "CODE_OF_ACCURACY")
    .FieldType(12) = "C"
    .FieldLength(12) = 9
    .FieldDecimalCount(12) = 0
  
    .FieldName(13) = "EF_LAT": CSVfield(13) = aCSV.FieldNumber("LATITUDE")
    .FieldType(13) = "C"
    .FieldLength(13) = 14
    .FieldDecimalCount(13) = 0
  
    .FieldName(14) = "EF_LONG": CSVfield(14) = aCSV.FieldNumber("LONGITUDE")
    .FieldType(14) = "C"
    .FieldLength(14) = 13
    .FieldDecimalCount(14) = 0
  
    .FieldName(15) = "EF_ACC_M"
    .FieldType(15) = "C"
    .FieldLength(15) = 9
    .FieldDecimalCount(15) = 0
  
    .FieldName(16) = "BLAT": CSVfield(16) = aCSV.FieldNumber("LATITUDE")
    .FieldType(16) = "C"
    .FieldLength(16) = 17
    .FieldDecimalCount(16) = 0
  
    .FieldName(17) = "BLONG": CSVfield(17) = aCSV.FieldNumber("LONGITUDE")
    .FieldType(17) = "C"
    .FieldLength(17) = 18
    .FieldDecimalCount(17) = 0
  
    .FieldName(18) = "BFIPS": CSVfield(18) = aCSV.FieldNumber(pCSVprefix & "COUNTY_CODE")
    .FieldType(18) = "C"
    .FieldLength(18) = 9
    .FieldDecimalCount(18) = 0
  
    .FieldName(19) = "STATE_ID"
    .FieldType(19) = "C"
    .FieldLength(19) = 8
    .FieldDecimalCount(19) = 0
  
    .FieldName(20) = "PERM_TYPE"
    .FieldType(20) = "C"
    .FieldLength(20) = 11
    .FieldDecimalCount(20) = 0
  
    .FieldName(21) = "ACTIVE"
    .FieldType(21) = "C"
    .FieldLength(21) = 8
    .FieldDecimalCount(21) = 0
  
    .FieldName(22) = "MAJOR_ID": CSVfield(22) = aCSV.FieldNumber(pCSVprefix & "MAJOR_DISCHARGE_INDICATOR")
    .FieldType(22) = "C"
    .FieldLength(22) = 5
    .FieldDecimalCount(22) = 0
  
    .FieldName(23) = "SIC2"
    .FieldType(23) = "C"
    .FieldLength(23) = 4
    .FieldDecimalCount(23) = 0
  
    .FieldName(24) = "SIC2D"
    .FieldType(24) = "C"
    .FieldLength(24) = 30
    .FieldDecimalCount(24) = 0
  
    .FieldName(25) = "IND_CLASS": CSVfield(25) = aCSV.FieldNumber(pCSVprefix & "INDUSTRY_CLASS")
    .FieldType(25) = "C"
    .FieldLength(25) = 9
    .FieldDecimalCount(25) = 0
  
    .FieldName(26) = "EPA_REG": CSVfield(26) = aCSV.FieldNumber(pCSVprefix & "REGION")
    .FieldType(26) = "C"
    .FieldLength(26) = 2
    .FieldDecimalCount(26) = 0
  
    .FieldName(27) = "STATE": CSVfield(27) = aCSV.FieldNumber(pCSVprefix & "NPDES")
    .FieldType(27) = "C"
    .FieldLength(27) = 2
    .FieldDecimalCount(27) = 0
  
    .FieldName(28) = "CITY": CSVfield(28) = aCSV.FieldNumber(pCSVprefix & "CITY_NAME")
    .FieldType(28) = "C"
    .FieldLength(28) = 20
    .FieldDecimalCount(28) = 0
  
    .FieldName(29) = "COUNTY": CSVfield(29) = aCSV.FieldNumber(pCSVprefix & "COUNTY_NAME")
    .FieldType(29) = "C"
    .FieldLength(29) = 20
    .FieldDecimalCount(29) = 0
  
    .FieldName(30) = "LOC_NAME"
    .FieldType(30) = "C"
    .FieldLength(30) = 30
    .FieldDecimalCount(30) = 0
  
    .FieldName(31) = "ADDRESS1"
    .FieldType(31) = "C"
    .FieldLength(31) = 30
    .FieldDecimalCount(31) = 0
  
    .FieldName(32) = "ADDRESS2"
    .FieldType(32) = "C"
    .FieldLength(32) = 30
    .FieldDecimalCount(32) = 0
  
    .FieldName(33) = "FAC_CITY"
    .FieldType(33) = "C"
    .FieldLength(33) = 23
    .FieldDecimalCount(33) = 0
  
    .FieldName(34) = "FAC_STATE"
    .FieldType(34) = "C"
    .FieldLength(34) = 2
    .FieldDecimalCount(34) = 0
  
    .FieldName(35) = "ZIP_CODE": CSVfield(35) = aCSV.FieldNumber(pCSVprefix & "LOC_ZIP_CODE")
    .FieldType(35) = "C"
    .FieldLength(35) = 9
    .FieldDecimalCount(35) = 0
  
    .FieldName(36) = "TELE": CSVfield(36) = aCSV.FieldNumber(pCSVprefix & "LOC_PHONE_NUM")
    .FieldType(36) = "C"
    .FieldLength(36) = 10
    .FieldDecimalCount(36) = 0
  
    .FieldName(37) = "RIV_BASIN"
    .FieldType(37) = "C"
    .FieldLength(37) = 20
    .FieldDecimalCount(37) = 0
  
    .FieldName(38) = "CU": CSVfield(38) = aCSV.FieldNumber(pCSVprefix & "USGS_HYDRO_BASIN_CODE")
    .FieldType(38) = "C"
    .FieldLength(38) = 8
    .FieldDecimalCount(38) = 0
  
    .FieldName(39) = "STREAM_SEG"
    .FieldType(39) = "C"
    .FieldLength(39) = 4
    .FieldDecimalCount(39) = 0
  
    .FieldName(40) = "REC_WATER": CSVfield(40) = aCSV.FieldNumber(pCSVprefix & "RECEIVING_WATERS")
    .FieldType(40) = "C"
    .FieldLength(40) = 35
    .FieldDecimalCount(40) = 0
  
    .FieldName(41) = "STREAM_MIL"
    .FieldType(41) = "C"
    .FieldLength(41) = 5
    .FieldDecimalCount(41) = 0
  
    .FieldName(42) = "PERM_AGENC"
    .FieldType(42) = "C"
    .FieldLength(42) = 5
    .FieldDecimalCount(42) = 0
  
    .FieldName(43) = "LIMIT_ID"
    .FieldType(43) = "C"
    .FieldLength(43) = 1
    .FieldDecimalCount(43) = 0
  
    .FieldName(44) = "BSOURCE"
    .FieldType(44) = "C"
    .FieldLength(44) = 3
    .FieldDecimalCount(44) = 0
  
    .FieldName(45) = "FLOW_RATE": CSVfield(45) = aCSV.FieldNumber(pCSVprefix & "FLOW_RATE")
    .FieldType(45) = "N"
    .FieldLength(45) = 20
    .FieldDecimalCount(45) = 5
  
    .FieldName(46) = "BSEG"
    .FieldType(46) = "C"
    .FieldLength(46) = 4
    .FieldDecimalCount(46) = 0
  
    .FieldName(47) = "PERM_ISSUE": CSVfield(47) = aCSV.FieldNumber(pCSVprefix & "ORIGINL_PERMIT_ISSUE_DATE")
    .FieldType(47) = "C"
    .FieldLength(47) = 6
    .FieldDecimalCount(47) = 0
  
    .FieldName(48) = "INACTIVE": CSVfield(48) = aCSV.FieldNumber(pCSVprefix & "INACTIVE_DATE")
    .FieldType(48) = "C"
    .FieldLength(48) = 6
    .FieldDecimalCount(48) = 0
  
    .FieldName(49) = "PERMIT_DAT": CSVfield(49) = aCSV.FieldNumber(pCSVprefix & "PERMIT_ISSUED_DATE")
    .FieldType(49) = "C"
    .FieldLength(49) = 6
    .FieldDecimalCount(49) = 0
  
    .FieldName(50) = "PERMIT_EXP": CSVfield(50) = aCSV.FieldNumber(pCSVprefix & "PERMIT_EXPIRED_DATE")
    .FieldType(50) = "C"
    .FieldLength(50) = 6
    .FieldDecimalCount(50) = 0
  
    .FieldName(51) = "PERMIT_EFF"
    .FieldType(51) = "C"
    .FieldLength(51) = 6
    .FieldDecimalCount(51) = 0
  
    .FieldName(52) = "PERMIT_MOD"
    .FieldType(52) = "C"
    .FieldLength(52) = 6
    .FieldDecimalCount(52) = 0
  
    .FieldName(53) = "BCU": CSVfield(53) = aCSV.FieldNumber(pCSVprefix & "USGS_HYDRO_BASIN_CODE")
    .FieldType(53) = "C"
    .FieldLength(53) = 12
    .FieldDecimalCount(53) = 0
  
    .FieldName(54) = "MAJOR_STAT"
    .FieldType(54) = "C"
    .FieldLength(54) = 16
    .FieldDecimalCount(54) = 0
  
    .FieldName(55) = "APP_TYPE"
    .FieldType(55) = "C"
    .FieldLength(55) = 14
    .FieldDecimalCount(55) = 0
  
    .NumRecords = aCSV.NumRecords
    .InitData
  End With
  
  Set newSHP = New CShape_IO
  newSHP.CreateNewShape newBaseFilename & ".shp", FILETYPEENUM.typePoint
  
  For record = 1 To aCSV.NumRecords
    newDBF.CurrentRecord = record
    aCSV.CurrentRecord = record
    XYPoint.thePoint.x = 0
    XYPoint.thePoint.y = 0
    XYPoint.ShapeType = 1
    For field = 1 To 55
      If CSVfield(field) > 0 Then
        CSValue = aCSV.Value(CSVfield(field))
        Select Case field
          Case 3: Select Case CSValue
                    Case "PRI": newDBF.Value(field) = "PRIVATE"
                    'TODO: find list of valid values for both tables
                  End Select
          Case 10, 11, 13, 14, 16, 17 'Lat/Long from DMS to decimal
            Dim degrees As Single
            Dim minsec As Single
            Dim lenValue As Integer
            Dim degStr As String
            Dim minStr As String
            Dim secStr As String
            
            lenValue = Len(CSValue)
            On Error GoTo BadLatLong
            degStr = Mid(CSValue, 1, lenValue - 5)
            minStr = Mid(CSValue, lenValue - 4, 2)
            secStr = Mid(CSValue, lenValue - 2, 3)
            
            degrees = CSng(degStr)
            minsec = CSng(secStr) 'Three-digit tenths of an arc second
            minsec = minsec / 600 'Convert tenths of seconds to minutes
            minsec = minsec + CSng(minStr) 'Add minutes
            minsec = minsec / 60  'Convert to degrees
            If degrees > 0 Then
              degrees = degrees + minsec
            Else
              degrees = degrees - minsec
            End If
'            If degrees > 0 Then
'              Debug.Print CSValue & " -> " & degStr & " " & minStr & """ " & secStr & "'  = " & degrees & vbTab;
'            Else
'              Debug.Print CSValue & " -> " & degStr & " " & minStr & """ " & secStr & "'  = " & degrees
'            End If
            newDBF.Value(field) = degrees
            If field = 10 Then
              XYPoint.thePoint.y = degrees
            ElseIf field = 11 Then
              XYPoint.thePoint.x = degrees
            End If
            
          Case 12: 'TODO: LATLONG_AC <- CODE_OF_ACCURACY
          Case 18: newDBF.Value(field) = CSValue 'TODO: add state FIPS
          Case 27: newDBF.Value(field) = Left(Trim(CSValue), 2)
          Case 47 To 52: 'TODO: convert dates
          
          Case Else: newDBF.Value(field) = CSValue
        End Select
      End If
SkipValue:
    Next
    newSHP.putXYPoint 0, XYPoint
    If (XYPoint.thePoint.x = 0 Or XYPoint.thePoint.y = 0) And errRecord < record Then
      Debug.Print "Warning: Lat/Long not set for point " & record
    End If
  Next
  newSHP.FileShutDown
  newDBF.WriteDBF newBaseFilename & ".dbf"
  Exit Sub
BadLatLong:
  If errRecord < record Then
    If Len(Trim(CSValue)) = 0 Then
      Debug.Print "Value missing for lat/long in record " & record
    Else
      Debug.Print "Could not parse lat/long value in record " & record & ": " & CSValue
    End If
    errRecord = record
  End If
  Resume SkipValue
End Sub
