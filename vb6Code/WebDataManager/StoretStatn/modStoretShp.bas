Attribute VB_Name = "modStoretShp"
Option Explicit

Public Sub main()
  Dim csv As New clsCSV
  csv.Delimiter = "~"
  csv.OpenCSV "C:\vbExperimental\WebDataManager\StoretStatn\allstations.narrow.txt"
  WriteStoretShape csv, "c:\basins\cache\storet"
End Sub

'Sub test(DBFname As String)
'  Dim lDbf As New clsDBF
'  With lDbf
'    .OpenDBF DBFname
'    SaveFileString "c:\test.bas", .CreationCode()
'    .Clear
'  End With
'End Sub

'Org ID
'Station ID
'Station Name
'Org Name
'Primary Type
'Secondary Type
'S/G/O Indicator
'Well Number
'Well Name
'Pipe Number
'NAICS Code
'Spring Type Improvement
'Permanence
'USGS Geologic Unit Code-Name
'Spring Other Name
'USGS Lithologic Unit Code-Name
'Location Point Type
'Point Sequence Number
'Point Name
'Latitude
'Longitude
'Horizontal Datum
'Geopositioning Method
'Map Scale
'Elevation
'Elevation Datum
'Elevation Method
'Country Name
'State
'County
'Hydrologic Unit Code
'RF1 Segment Code
'RF1 Segment Name
'RF1 Mileage
'On Reach Ind
'NRCS Watershed ID
'Primary Estuary
'Secondary Estuary
'Other Estuary Name
'Great Lake Name
'Ocean Name
'Natv American Land Name
'FRS Key Identifier
'Station Document/Graphic Name

Public Sub WriteStoretShape(aCSV As clsCSV, ByVal newBaseFilename As String)
  Dim CSVfield() As Long
  Dim CSValue As String
  Dim record As Long
  Dim field As Long
  Dim latField As Long
  Dim lonField As Long
  Dim errRecord As Long
  Dim XYPoint As T_shpXYPoint
  Dim newSHP As CShape_IO
  Dim newDBF As clsDBF
  
  latField = aCSV.FieldNumber("Latitude")
  lonField = aCSV.FieldNumber("Longitude")
  
  If latField = 0 Then
    MsgBox "Latitude not found, cannot write STORET shape file"
    Exit Sub
  End If
  
  If lonField = 0 Then
    MsgBox "Longitude not found, cannot write STORET shape file"
    Exit Sub
  End If
  
  Set newDBF = New clsDBF
  With newDBF
    .Year = CInt(Format(Now, "yyyy")) - 1900
    .Month = CInt(Format(Now, "mm"))
    .Day = CInt(Format(Now, "dd"))
    
    .NumFields = 4
    ReDim CSVfield(aCSV.NumFields)
    CSVfield(1) = 1
    CSVfield(2) = 2
    CSVfield(3) = latField
    CSVfield(4) = lonField
    
    
    .FieldName(1) = "Agency"
    .FieldName(2) = "Station"
    .FieldName(3) = aCSV.FieldName(CSVfield(3))
    .FieldName(4) = aCSV.FieldName(CSVfield(4))
    
    .FieldType(1) = "C"
    .FieldType(2) = "C"
    .FieldType(3) = "N"
    .FieldType(4) = "N"
    .FieldLength(1) = 8
    .FieldLength(2) = 15
    .FieldLength(3) = 10
    .FieldLength(4) = 10
    .FieldDecimalCount(1) = 0
    .FieldDecimalCount(2) = 0
    .FieldDecimalCount(3) = 7
    .FieldDecimalCount(4) = 7
    
'    .NumFields = aCSV.NumFields
'    ReDim CSVfield(.NumFields)
'    For field = 1 To aCSV.NumFields
'      CSVfield(field) = field 'Use to re-map field numbers later as in PCS
'      .FieldName(field) = aCSV.FieldName(field)
'      .FieldType(field) = "C"
'      Debug.Print aCSV.FieldLength(field)
'      .FieldLength(field) = aCSV.FieldLength(field)
'      .FieldDecimalCount(field) = 0
'    Next
  
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
    For field = 1 To newDBF.NumFields
      If CSVfield(field) > 0 Then
        CSValue = aCSV.Value(CSVfield(field))
        Select Case field
          Case 1
            newDBF.Value(field) = CSValue
          Case 2
            newDBF.Value(field) = CSValue
          Case latField
            newDBF.Value(field) = CSValue
            If IsNumeric(CSValue) Then
              XYPoint.thePoint.y = CSng(CSValue)
            Else
              GoTo BadLatLong
            End If
          Case lonField
            newDBF.Value(field) = CSValue
            If IsNumeric(CSValue) Then
              XYPoint.thePoint.x = CSng(CSValue)
            Else
              GoTo BadLatLong
            End If
          Case Else: newDBF.Value(field) = CSValue
        End Select
      End If
SkipValue:
    Next
    newSHP.putXYPoint 0, XYPoint
    If (XYPoint.thePoint.x = 0 Or XYPoint.thePoint.y = 0) And errRecord < record Then
      Debug.Print "Warning: Lat/Long not set for point " & record
    End If
    Debug.Print record / aCSV.NumRecords
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
  GoTo SkipValue
End Sub

