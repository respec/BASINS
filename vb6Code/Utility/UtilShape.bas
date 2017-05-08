Attribute VB_Name = "UtilShape"
Option Explicit
'##MODULE_REMARKS Copyright 2001-3 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Global NextAttribSerial As Long
Global NextBranchSerial As Long
Global NextNodeSerial As Long

'Reads main header at beginning of shp or shx file
Public Sub ReadShapeHeader(inFile%, ByRef FileLength&, ByRef ShapeType&, ByRef lowX#, ByRef lowY#, ByRef uppX#, ByRef uppY#)
  Dim FileCode&, version&
  Seek #inFile, 1
  FileCode = ReadBigInt(inFile)
  If FileCode <> 9994 Then If MsgBox("Warning: File Code of " & FileCode & " found. Expected 9994.", vbOKCancel, "ReadShapeHeader") = vbCancel Then Exit Sub
  Seek #inFile, 25
  FileLength = ReadBigInt(inFile)
  Get #inFile, , version
  If version <> 1000 Then If MsgBox("Warning: Version of " & version & " found. Expected 1000.", vbOKCancel, "ReadShapeHeader") = vbCancel Then Exit Sub
  Get #inFile, , ShapeType '0=null, 1=point, 3=arc, 5=polygon, 8=multipoint
  ReadBoundingBox inFile, lowX, lowY, uppX, uppY
  'Dim s$
  's = "Code = " & FileCode & " (9994 expected), Version = " & version & " (1000 expected)" & vbCr
  's = s & "Length = " & FileLength & vbCr
  'If ShapeType = 1 Then s = s & "Points (Length implies " & (FileLength - 50) / 14 & " points in shp or " & (FileLength - 50) / 4 & " points in shx)" & vbCr Else s = s & "Shape Type = " & ShapeType
  's = s & ", Bounding box: (" & lowX & ", " & lowY & "), (" & uppX & ", " & uppY & ")" & vbCr
  'MsgBox s, vbOKOnly, "ReadShapeHeader"
  
End Sub

'Creates empty new BaseFilename.shp and BaseFilename.shx
'ShapeType: 0=null, 1=point, 3=arc, 5=polygon, 8=multipoint
'BaseFileName.dbf must be created separately
Public Sub CreateNewShapeFile(baseFilename$, ShapeType&)
  Dim f$, shpfile%, ShxFile%
  Dim lowX#, lowY#, lowZ#, lowM#
  Dim uppX#, uppY#, uppZ#, uppM#
  
  lowX = 1E+30: uppX = -1E+30
  lowY = 1E+30: uppY = -1E+30
  lowZ = 1E+30: uppZ = -1E+30
  lowM = 1E+30: uppM = -1E+30
  
  On Error GoTo AbortSub
  
  f = baseFilename & ".shp"
  shpfile = FreeFile(0)
  Open f For Binary Access Write As shpfile
  Call WriteShapeHeader(shpfile, 50, ShapeType, lowX, lowY, uppX, uppY, lowZ, lowM, uppZ, uppM)
  Close shpfile
  
  f = baseFilename & ".shx"
  ShxFile = FreeFile(0)
  Open f For Binary Access Write As ShxFile
  Call WriteShapeHeader(ShxFile, 50, ShapeType, lowX, lowY, uppX, uppY, lowZ, lowM, uppZ, uppM)
  Close ShxFile
  Exit Sub
AbortSub:
  MsgBox "Error Creating New Shape File " & f & vbCr & Err.Description
End Sub

Public Sub WriteShapeHeader(outfile%, FileLength&, ShapeType&, lowX#, lowY#, uppX#, uppY#, lowZ#, lowM#, uppZ#, uppM#)
  Dim i&, bigzero&, version&
  Seek #outfile, 1
  bigzero = SwapBytes(0&)
  lowZ = 0
  lowM = 0
  uppZ = 0
  uppM = 0
  WriteBigInt outfile, 9994
  For i = 1 To 5
    Put outfile, , bigzero
  Next i
  WriteBigInt outfile, FileLength
  version = 1000
  Put #outfile, , version
  Put #outfile, , ShapeType '0=null, 1=point, 3=arc, 5=polygon, 8=multipoint
  Put #outfile, , lowX      'bounding box
  Put #outfile, , lowY
  Put #outfile, , uppX
  Put #outfile, , uppY
  
  Put #outfile, , lowZ      'Not used, but must be in header for spacing
  Put #outfile, , uppZ      'Note different order that min/max X and Y
  Put #outfile, , lowM
  Put #outfile, , uppM
  'Dim s$
  's = "Code = 9994 expected, Version = " & version & " (1000 expected)" & vbCr
  's = s & "Length = " & FileLength & vbCr
  'If ShapeType = 1 Then s = s & "Points (Length implies " & (FileLength - 50) / 14 & " points in shp or " & (FileLength - 50) / 4 & " points in shx)" & vbCr Else s = s & "Shape Type = " & ShapeType
  's = s & ", Bounding box: (" & lowX & ", " & lowY & "), (" & uppX & ", " & uppY & ")" & vbCr
  'MsgBox s, vbOKOnly, "WriteShapeHeader"

End Sub

'Reads record header before each shape in shp file.
'Record header is three 4-byte integers:
'Big-endian RecordNumber (sequentially numbered in file)
'Big-endian ContentLength (measured in 16-bit words)
'Little-endian ShapeType 0=null, 1=point, 3=arc, 5=polygon, 8=multipoint
'pre: file pointer for infile must be positioned at first byte of record header
'post: file pointer for infile will be positioned at beginning of data for this record
Public Sub ReadShapeRecordHeader(inFile%, ByRef RecordNumber&, ByRef ContentLength&, ByRef ShapeType&)
  RecordNumber = ReadBigInt(inFile)
  ContentLength = ReadBigInt(inFile)
  Get #inFile, , ShapeType
End Sub

'pre: file pointer for outfile must be positioned at first byte of record header
'post: file pointer for outfile will be positioned at beginning of data for this record
Public Sub WriteShapeRecordHeader(outfile%, RecordNumber&, ContentLength&, ShapeType&)
  WriteBigInt outfile, RecordNumber
  WriteBigInt outfile, ContentLength
  Put #outfile, , ShapeType
End Sub

'Reads data for a point record in a shp file
'pre: file pointer for infile must be positioned just after the ShapeType for this record
'post: file pointer for infile will be positioned at beginning of record header for next record or at EOF if last record
Public Sub ReadShapePoint(inFile%, ByRef x#, ByRef y#)
  Get #inFile, , x
  Get #inFile, , y
End Sub

'pre: file pointer for outfile must be positioned just after the ShapeType for this record
'post: file pointer for outfile will be positioned at beginning of record header for next record or at EOF if last record
Public Sub WriteShapePoint(outfile%, x#, y#)
  Put #outfile, , x
  Put #outfile, , y
End Sub

'Does equivalent of WriteShapeRecordHeader plus WriteShapePoint for a point record
'pre: file pointer for outfile must be positioned at first byte of record header
'post: file pointer for outfile will be positioned at beginning of record header for next record or at EOF if last record
Public Sub WriteShapePointAll(outfile%, RecordNumber&, x#, y#)
  Dim ContentLength&, ShapeType&
  ContentLength = 10 '16-bit words: 2 for ShapeType, 4 for X, 4 for Y
  ShapeType = 1
  'Seek #outfile, 101 + 28 * (RecordNumber - 1)
  WriteBigInt outfile, RecordNumber
  WriteBigInt outfile, ContentLength
  Put #outfile, , ShapeType
  Put #outfile, , x
  Put #outfile, , y
End Sub

'Writes record in shx index file (two 4-byte big-endian integers) :
'Offset in shp file of this shape (measured in 16-bit words)
'ContentLength of this record in shp file
'pre: file pointer for outfile must be positioned at first byte of this record or EOF
'post: file pointer for outfile will be positioned at beginning of next record or at EOF if last record
Public Sub WriteShapeIndex(outfile%, ContentLength&, offset&)
  WriteBigInt outfile, offset
  WriteBigInt outfile, ContentLength
End Sub

'Writes record in shx index file
'Since points are all the same size, we can calculate ContentLength and Offset from the record number
Public Sub WriteShapePointIndex(outfile%, RecordNumber&)
  Dim offset&, ContentLength&
  offset = 50 + 14 * (RecordNumber - 1)
  ContentLength = 10 '16-bit words: 2 for ShapeType, 4 for X, 4 for Y
  WriteShapeIndex outfile, ContentLength, offset
End Sub

'Reads an arc record from a shp file
'bounding box: lowX, lowY, uppX, uppY
'NumParts: number of disjoint lines in this record
'NumPoints: total number of x,y points in this record
'Parts: array of zero-based indexes into point array (first point in each part)
'x, y: arrays of coordinates of points
'pre: infile pointer must be just after ShapeType for this record
'post: infile pointer will be at start of next record header or at EOF if last record
Public Sub ReadShapeArc(inFile%, ByRef lowX#, ByRef lowY#, ByRef uppX#, ByRef uppY#, ByRef NumParts&, ByRef NumPoints&, ByRef parts&(), ByRef x#(), ByRef y#())
  ReadBoundingBox inFile, lowX, lowY, uppX, uppY
  Get #inFile, , NumParts
  Get #inFile, , NumPoints
  ReadIntArray inFile, NumPoints, parts
  ReadPoints inFile, NumPoints, x, y
End Sub

Public Sub WriteShapeSingleLine(ByVal outfile%, ByVal outIndex%, ByVal RecordNumber&, ByRef x#(), ByRef y#())
  Dim lowX#, lowY#, uppX#, uppY#, ContentLength&, NumPoints&, parts&()
  Dim pt&
  ReDim parts(0)
  parts(0) = 0
  pt = LBound(x)
  NumPoints = UBound(x) - pt + 1
  lowX = x(pt): uppX = lowX
  lowY = y(pt): uppY = lowY
  While pt < UBound(x)
    pt = pt + 1
    If x(pt) > uppX Then uppX = x(pt) Else If x(pt) < lowX Then lowX = x(pt)
    If y(pt) > uppY Then uppY = y(pt) Else If y(pt) < lowY Then lowY = y(pt)
  Wend
  '16-bit words: 2 for ShapeType, 16 for bounding box, 2 each for numparts, NumPoints, one point, 4 for each X, 4 for each Y
  ContentLength = 2 + 16 + 2 + 2 + 2 + 8 * NumPoints
  WriteShapeIndex outIndex, ContentLength, Seek(outfile) / 2
  WriteShapeRecordHeader outfile, RecordNumber, ContentLength, 3
  WriteShapeArc outfile, lowX, lowY, uppX, uppY, 1, NumPoints, parts, x, y
End Sub

Public Sub WriteShapeArc(outfile%, ByRef lowX#, ByRef lowY#, ByRef uppX#, ByRef uppY#, ByRef NumParts&, ByRef NumPoints&, ByRef parts&(), ByRef x#(), ByRef y#())
  WriteBoundingBox outfile, lowX, lowY, uppX, uppY
  Put #outfile, , NumParts
  Put #outfile, , NumPoints
  WriteIntArray outfile, NumParts, parts
  WritePoints outfile, NumPoints, x, y
End Sub

Private Sub ReadBoundingBox(inFile, ByRef lowX#, ByRef lowY#, ByRef uppX#, ByRef uppY#)
  Get #inFile, , lowX
  Get #inFile, , lowY
  Get #inFile, , uppX
  Get #inFile, , uppY
End Sub

Private Sub WriteBoundingBox(outfile, ByRef lowX#, ByRef lowY#, ByRef uppX#, ByRef uppY#)
  Put #outfile, , lowX
  Put #outfile, , lowY
  Put #outfile, , uppX
  Put #outfile, , uppY
End Sub

Private Sub ReadIntArray(inFile, NumVals&, a&())
  Dim i&
  ReDim a(0 To NumVals - 1)
  For i = 0 To NumVals - 1
    Get #inFile, , a(i)
  Next i
End Sub

Private Sub WriteIntArray(outfile, NumVals&, a&())
  Dim i&
  For i = LBound(a) To LBound(a) + NumVals - 1
    Put #outfile, , a(i)
  Next i
End Sub

Private Sub ReadPoints(inFile, NumPoints&, ByRef x#(), ByRef y#())
  Dim i&
  ReDim x(0 To NumPoints - 1)
  ReDim y(0 To NumPoints - 1)
  For i = 0 To NumPoints - 1
    Get #inFile, , x(i)
    Get #inFile, , y(i)
  Next i
End Sub

Private Sub WritePoints(outfile, NumPoints&, ByRef x#(), ByRef y#())
  Dim i&
  For i = LBound(x) To LBound(x) + NumPoints - 1
    Put #outfile, , x(i)
    Put #outfile, , y(i)
  Next i
End Sub

