Attribute VB_Name = "UtilMap"
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants

'Public Enum ATCoRendType
'  ATCoNoRend = 0
'  ATCoValueRend = 1
'  ATCoClassBreakRend = 2
'End Enum

Public Type MapFieldInfo
  Name As String    'name of column in dbf file
  Caption As String 'same as Name by default, used for display
  Column As Integer 'If displayed in a grid, what column, (< 0 if not visible)
End Type

'Public Type MapBranchInfo
'  Key As String 'idlocn
'  DownID As String
'  Branch As String
'End Type

Public Type MapLayerInfo
  Name As String
  path As String
  baseFilename As String 'ESRI shape file this layer will be drawn from
  fileSize As Long       'length of shape file in bytes
  RendType As Long       'should be As ATCoRendType
  keyField As String    'name of unique ID field used for selection, not editable in grid
  LabelField As String  'name of field containing names to be used in tooltips
  TableIndex As Long
  Animate As Boolean
  BranchField As String
  DownIDField As String
  LengthField As String
  Selected() As Boolean
  nSelected As Long
  Fields() As MapFieldInfo
  nFields As Long
'  BranchInfo() As MapBranchInfo
'  nBranchInfo As Long
End Type

Private MapDB As Database

Public FileTypeName() As String
Public DataTypeName() As String

Public Sub InitFileTypeName()
  ReDim FileTypeName(99)
  FileTypeName(0) = "Null"
  FileTypeName(1) = "Point"
  FileTypeName(3) = "PolyLine"
  FileTypeName(5) = "Polygon"
  FileTypeName(8) = "MultiPoint"
  FileTypeName(11) = "PointZ"
  FileTypeName(13) = "PolyLineZ"
  FileTypeName(15) = "PolygonZ"
  FileTypeName(18) = "MultiPointZ"
  FileTypeName(21) = "PointM"
  FileTypeName(23) = "PolyLineM"
  FileTypeName(25) = "PolygonM"
  FileTypeName(28) = "MultiPointM"
  FileTypeName(31) = "MultiPatch"
  FileTypeName(40) = "DBF"
  FileTypeName(50) = "WDM"
  FileTypeName(60) = "UCI"
  FileTypeName(70) = "Text"
  FileTypeName(80) = "RichText"
  FileTypeName(90) = "RDB"
  FileTypeName(99) = "Unknown"
End Sub

Public Sub InitDataTypeName()
  ReDim DataTypeName(100) 'I don't know right now
  DataTypeName(dbBigInt) = "Big Integer"
  DataTypeName(dbBinary) = "Binary"
  DataTypeName(dbBoolean) = "Boolean"
  DataTypeName(dbByte) = "Byte"
  DataTypeName(dbChar) = "Char"
  DataTypeName(dbCurrency) = "Currency"
  DataTypeName(dbDate) = "Date / Time"
  DataTypeName(dbDecimal) = "Decimal"
  DataTypeName(dbDouble) = "Double"
  DataTypeName(dbFloat) = "Float"
  DataTypeName(dbGUID) = "Guid"
  DataTypeName(dbInteger) = "Integer"
  DataTypeName(dbLong) = "Long"
  DataTypeName(dbLongBinary) = "Long Binary"
  DataTypeName(dbMemo) = "Memo"
  DataTypeName(dbNumeric) = "Numeric"
  DataTypeName(dbSingle) = "Single"
  DataTypeName(dbText) = "Text"
  DataTypeName(dbTime) = "Time"
  DataTypeName(dbTimeStamp) = "Time Stamp"
  DataTypeName(dbVarBinary) = "VarBinary"
End Sub

'Call with full path and extension of .dbf file
'Closes previously opened DBF, if any, without saving updates
Public Function OpenDBF(Filename$, ReadOnly As Boolean) As Recordset
  Dim DBFfilename$, dbname$, MapRS As Recordset
  'be sure database assoc with map is closed
  On Error Resume Next
  Set OpenDBF = Nothing
  MapDB.Close
  On Error GoTo openError
  dbname = Left(Filename, Len(Filename) - (Len(Dir(Filename)) + 1))
  DBFfilename = FilenameOnly(Filename) & ".dbf"
  If Len(DBFfilename) > 12 Then
    If (MsgBox("The map database file " & Filename & " has too long a name to be opened." & vbCr & "The maximum length is 8 characters plus .dbf" & vbCr & "Try anyway?", vbYesNo) = vbNo) Then Exit Function
  End If
  Set MapDB = OpenDatabase(dbname, False, ReadOnly, "DBASE IV")
  Set MapRS = MapDB.OpenRecordset(DBFfilename, dbOpenDynaset)
  If Not ReadOnly And Not MapRS.Updatable Then
    MsgBox "Attempted to update " & DBFfilename & " but it was not updatable."
    MapRS.Close
  'Else
  '  OpenDBF = True
  End If
  Set OpenDBF = MapRS
  Exit Function
openError:
  MsgBox "Error opening " & Filename & vbCr & Err.Description
End Function

Public Function GetDatabase$(f As String)
  GetDatabase = Left(f, Len(f) - (Len(Dir(f)) + 1))
End Function

