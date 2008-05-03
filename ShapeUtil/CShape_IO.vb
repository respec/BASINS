Option Strict Off
Option Explicit On

Imports atcUtility
Imports System.IO

Friend Class CShape_IO
	'********************************************************************************
	'Class Module title :       CShape_IO
	'Author :  Kenneth R. McVay
	'Date   :  June 22, 1998
	'Purpose : To provide IO for shapefile and their associated index files
    '********************************************************************************
    'Converted to VB.Net September 29, 2006 by Mark Gray at AQUA TERRA Consultants
	
	'*****************************************************************************************
	'Enumeration READWRITEFLAG
	' Valid flags for the ShapefileOpen procedure
	Public Enum READWRITEFLAG
        ReadOnlyMode = 0
		Readwrite = 1
	End Enum
	
	'*****************************************************************************************
	'Append
	'used by the put procedures
	'if append is used for recordnumber then the record is added
	Private Const Append As Short = 0
	
	'*****************************************************************************************
	'const shpfilecode
	'the valid code for the filecode in the main headers
	Private Const shpFileCode As Short = 9994
	
	'*****************************************************************************************
	'enumeration FILESIZES
	' used for easy access to the header file sizes
	Private Enum FILESIZES
		MainHeaderSize = 100 'in bytes
		IndexRecSize = 8 'in bytes
		shpRecSize = 8
	End Enum
	
	'*****************************************************************************************
	'Enumeration FILETYPEENUM
	'easy access to all of the type identifiers available
	Public Enum FILETYPEENUM
		typeNullShape = 0
		typePoint = 1
		typePolyline = 3
		typePolygon = 5
		typeMultipoint = 8
		typePointZ = 11
		typePolyLineZ = 13
		typePolygonZ = 15
		typeMultiPointZ = 18
		typePointM = 21
		typePolyLineM = 23
		typePolygonM = 25
		typeMultiPointM = 28
		typeMultiPatch = 31
		typeWDM = 50
		typeUCI = 60
		typeTXT = 70
		typeRTF = 80
		typeRDB = 90
		typeUnknown = 99
	End Enum
	
	'*****************************************************************************************
	'Used for easy access to the main file header offsets
	Public Enum HEADEROFFSETS
		FileCode = 1
		u1 = 5
		u2 = 9
		u3 = 13
		u4 = 17
		u5 = 21
		FileLength = 25
		version = 29
		shptype = 33
		xMin = 37
		yMin = 45
		xMax = 53
		yMax = 61
		Zmin = 69
		Zmax = 77
		Mmin = 85
		Mmax = 93
	End Enum
	
	'*****************************************************************************************
	'Tempory shape files for updating records
	Private Const shptmp As String = "shptmp.tmp"
	Private Const indxtmp As String = "indxtmp.tmp"
	
	'********************************************************************************
	'The following Enum defines the errors that will be rasied in this Class Module
	Public Enum ShapeIOError
		NoSuchRecordERROR = vbObjectError + 512 + 2
		RecordsNotMatchERROR = vbObjectError + 512 + 3
		OffsetPastEOF = vbObjectError + 512 + 4
		InvalidShapeFile = vbObjectError + 512 + 5
		filenotopen = vbObjectError + 512 + 6
		WrongShapeType = vbObjectError + 512 + 7
		FileAlreadyOpen = vbObjectError + 512 + 8
		TypeNotSupported = vbObjectError + 512 + 9
		FileIsReadOnly = vbObjectError + 512 + 10
	End Enum
	
	'********************************************************************************
	' Variables initilized for the different shape record types and headers
	Private ShapeFileHeader As ShapeDefines.T_MainFileHeader
	Private IndexFileHeader As ShapeDefines.T_MainFileHeader
	Private RecordHeader As ShapeDefines.T_RecordHeader
	Private IndexRecordHeader As ShapeDefines.T_IndexRecordHeader
	Private XYPoint As ShapeDefines.T_shpXYPoint
	Private Polygon As ShapeDefines.T_shpPoly
	Private PointMZ As ShapeDefines.T_shpPointMZ
	'   The following all use either PointMZ or Polygon
	'    Private XYMultiPoint As ShapeDefines.T_shpXYMultiPoint
	'    Private MultiPointM As ShapeDefines.T_shpMultiPointM
	'    Private PolyLineM As ShapeDefines.T_shpPolyLineM
	'    Private PolygonM As ShapeDefines.T_shpPolygonM
	'    Private PointZ As ShapeDefines.T_shpPointZ
	'    Private MultiPointZ As ShapeDefines.T_shpMultiPointZ
	'    Private PolyLineZ As ShapeDefines.T_shpPolyLineZ
	'    Private PolygonZ As ShapeDefines.T_shpPolygonZ
	'    Private MultiPatch As ShapeDefines.T_shpMultiPatch
	
	'********************************************************************************
    Private sFnRdr As BinaryReader
    Private sFnWrt As BinaryWriter
    'Private sFnR As Integer '.shp file handle
    Private iFnR As Integer '.shx file handle
    Private dbFnR As Integer 'DataBase filename for read
    Private FnW As Integer ' FileHandle for writing a shapefile
    Private RDONLY As Boolean 'boolean value to identify if the shapefile should be readonly
    Private ShapeFilename As String 'string to hold the current shapefile name
    Private ShapeIsOpen As Boolean 'boolean value identifies if file is open
    Private thePath As String
    Private IndexfileName As String 'string to hold the current indexfilename  "nada" if not file exist
    Private IndexIsOpen As Boolean 'boolean value identifies if file is open

    Private RecordCount As Integer 'variable to hold the record count. calculated from index file

    Public Function SwapBytes(ByVal n As Integer) As Integer
        ' ##SUMMARY Swaps between big and little endian 32-bit integers.
        ' ##SUMMARY   Example: SwapBytes(1) = 16777216
        ' ##PARAM N I Any long integer
        ' ##RETURNS Modified input parameter N.
        Dim OrigBytes As Byte()
        Dim NewBytes As Byte()
        ' ##LOCAL OrigBytes - stores original bytes
        ' ##LOCAL NewBytes - stores new bytes

        OrigBytes = BitConverter.GetBytes(n)
        ReDim NewBytes(3)
        NewBytes(0) = OrigBytes(3)
        NewBytes(1) = OrigBytes(2)
        NewBytes(2) = OrigBytes(1)
        NewBytes(3) = OrigBytes(0)
        Return BitConverter.ToInt32(NewBytes, 0)
    End Function

    '********************************************************************************
    'Name ShapeFileOpen
    'Arguments :    FileName  The filename of the Shapefile to open
    '               ReadWriteFlag   To identify if readonly or read/write
    '********************************************************************************
    Public Function ShapeFileOpen(ByVal filename As String, ByVal RWFLAG As Integer) As Boolean
        If RWFLAG = READWRITEFLAG.ReadOnlyMode Then
            RDONLY = True
        Else
            RDONLY = False
        End If

        If isFileNameValid(filename) = False Then
            Throw New ApplicationException("The shapefile " & ShapeFilename & " does not have a valid filename")
        End If

        iFnR = FreeFile
        FileOpen(iFnR, IndexfileName, OpenMode.Binary) 'open the index file
        FileGet(iFnR, IndexFileHeader)
        With IndexFileHeader 'swap bytes of fields stored big endian
            .FileCode = SwapBytes(.FileCode)
            .FileLength = SwapBytes(.FileLength)
            .u1 = SwapBytes(.u1)
            .u2 = SwapBytes(.u2)
            .u3 = SwapBytes(.u3)
            .u4 = SwapBytes(.u4)
            .u5 = SwapBytes(.u5)
        End With
        If IndexFileHeader.FileCode = shpFileCode Then
            IndexIsOpen = True
            RecordCount = (IndexFileHeader.FileLength - 50) / 4 'set the global RecordCount
        Else
            FileClose((iFnR)) 'There was a problem with the index file
        End If

        sFnRdr = New BinaryReader(File.Open(ShapeFilename, FileMode.OpenOrCreate))
        With ShapeFileHeader 'swap bytes of fields stored big endian
            .FileCode = SwapBytes(sFnRdr.ReadInt32)
            If .FileCode <> shpFileCode Then 'Problem with the file
                Throw New ApplicationException("The shapefile " & ShapeFilename & " does not start with a valid code (" & .FileCode & ")")
            End If
            .u1 = SwapBytes(sFnRdr.ReadInt32)
            .u2 = SwapBytes(sFnRdr.ReadInt32)
            .u3 = SwapBytes(sFnRdr.ReadInt32)
            .u4 = SwapBytes(sFnRdr.ReadInt32)
            .u5 = SwapBytes(sFnRdr.ReadInt32)
            .FileLength = SwapBytes(sFnRdr.ReadInt32)
            .version = sFnRdr.ReadInt32
            .ShapeType = sFnRdr.ReadInt32
            .BndBoxXmin = sFnRdr.ReadDouble
            .BndBoxYmin = sFnRdr.ReadDouble
            .BndBoxXmax = sFnRdr.ReadDouble
            .BndBoxYmax = sFnRdr.ReadDouble
            .BndBoxZmin = sFnRdr.ReadDouble
            .BndBoxZmax = sFnRdr.ReadDouble
            .BndBoxMmin = sFnRdr.ReadDouble
            .BndBoxMmax = sFnRdr.ReadDouble
        End With
        '    Select Case ShapeFileHeader.ShapeType
        '        Case FILETYPEENUM.typePoint
        '        Case FILETYPEENUM.typeMultiPoint
        '        Case FILETYPEENUM.typePolyLine
        '        Case FILETYPEENUM.typePolygon
        '        Case FILETYPEENUM.typePointM:      Err.Raise TypeNotSupported, "CShape_IO::ShapefileOpen", "PointM not yet supported"
        '        Case FILETYPEENUM.typeMultiPointM: Err.Raise TypeNotSupported, "CShape_IO::ShapefileOpen", "MultiPointM not yet supported"
        '        Case FILETYPEENUM.typePolyLineM:   Err.Raise TypeNotSupported, "CShape_IO::ShapefileOpen", "PolylineM not yet supported"
        '        Case FILETYPEENUM.typePolygonM:    Err.Raise TypeNotSupported, "CShape_IO::ShapefileOpen", "PolygonM not yet supported"
        '        Case FILETYPEENUM.typePointZ:      Err.Raise TypeNotSupported, "CShape_IO::ShapefileOpen", "PointZ not yet supported"
        '        Case FILETYPEENUM.typeMultiPointZ: Err.Raise TypeNotSupported, "CShape_IO::ShapefileOpen", "MultiPointZ not yet supported"
        '        Case FILETYPEENUM.typePolyLineZ ':   Err.Raise TypeNotSupported, "CShape_IO::ShapefileOpen", "PolylineZ not yet supported"
        '        Case FILETYPEENUM.typePolygonZ:    Err.Raise TypeNotSupported, "CShape_IO::ShapefileOpen", "PolygonZ not yet supported"
        '        Case FILETYPEENUM.typeMultiPatch:  Err.Raise TypeNotSupported, "CShape_IO::ShapefileOpen", "MultiPatch not yet supported"
        '    End Select
        ShapeIsOpen = True
        ShapeFileOpen = True
    End Function

    '********************************************************************************
    'Property getShapeHeader
    'Returns the header info from a shape file
    '********************************************************************************
    Friend ReadOnly Property getShapeHeader() As ShapeDefines.T_MainFileHeader
        Get
            On Error GoTo ERR_ROUTINE

            If ShapeIsOpen Then
                Return ShapeFileHeader 'return info if file is open
            Else
                Err.Raise(ShapeIOError.filenotopen, "CShape_IO::getShapeHeader", "The ShapeFile is not Open")
            End If 'raise error if file is not open
            Exit Property

ERR_ROUTINE:
            Err.Raise(Err.Number, Err.Source, Err.Description)
        End Get
    End Property

    '********************************************************************************
    'Property getRecordCount
    'Returns the number of records
    '********************************************************************************
    Public ReadOnly Property getRecordCount() As Integer
        Get
            If IndexIsOpen Then 'can only get count if index is available
                getRecordCount = RecordCount
            Else
                Err.Raise(ShapeIOError.filenotopen, "CShape_IO::getRecordCount", "Index file not open or invalid")
            End If
        End Get
    End Property

    '********************************************************************************
    'Property getXYPoint
    'Given a valid record number the point record is returned
    '********************************************************************************
    Friend ReadOnly Property getXYPoint(ByVal RecordNumber As Integer) As ShapeDefines.T_shpXYPoint
        Get
            On Error GoTo ERR_ROUTINE

            chkForErrBeforeRW(RecordNumber, "CShape_IO::getXYPoint", FILETYPEENUM.typePoint)
            If FindOffset(RecordNumber) Then
                getRecordHeader((IndexRecordHeader.offset * 2 + 1))
                If RecordHeader.RecordNumber = RecordNumber Then
                    With XYPoint
                        .ShapeType = sFnRdr.ReadInt32
                        With .thePoint
                            .x = sFnRdr.ReadDouble
                            .y = sFnRdr.ReadDouble
                        End With
                    End With
                Else
                    Err.Raise(ShapeIOError.RecordsNotMatchERROR, "CShape_IO::getXYPoint", "Index Record does not match main file Record")
                End If
            Else
                Err.Raise(ShapeIOError.OffsetPastEOF, "CShape_IO::getXYPoint", "The offset read from index file is larger than mainfile size")
            End If
            Return XYPoint
            Exit Property

ERR_ROUTINE:
            Err.Raise(Err.Number, Err.Source, Err.Description)
        End Get
    End Property

    '********************************************************************************
    'Property getPoly
    'given a valid record number the XYMultiPoint, Polygon or Polyline record is returned

    Friend ReadOnly Property getPoly(ByVal RecordNumber As Integer) As ShapeDefines.T_shpPoly
        Get
            On Error GoTo ERR_ROUTINE

            chkForErrBeforeRW(RecordNumber, "CShape_IO::getPolygon", FILETYPEENUM.typePolygon)

            If FindOffset(RecordNumber) Then
                getRecordHeader((IndexRecordHeader.offset * 2 + 1))
                With Polygon
                    .ShapeType = sFnRdr.ReadInt32
                    With .Box
                        .xMin = sFnRdr.ReadDouble
                        .yMin = sFnRdr.ReadDouble
                        .xMax = sFnRdr.ReadDouble
                        .yMax = sFnRdr.ReadDouble
                    End With

                    Select Case .ShapeType 'MultiPoint types have no parts
                        Case FILETYPEENUM.typeMultipoint, FILETYPEENUM.typeMultiPointZ, FILETYPEENUM.typeMultiPointM
                            .NumParts = 0
                        Case Else
                            .NumParts = sFnRdr.ReadInt32
                    End Select

                    .NumPoints = sFnRdr.ReadInt32

                    ReDim .Parts(.NumParts - 1)
                    If .NumParts > 0 Then
                        For lPartsIndex As Integer = 0 To .NumParts - 1
                            .Parts(lPartsIndex) = sFnRdr.ReadInt32
                        Next
                        If .ShapeType = FILETYPEENUM.typeMultiPatch Then
                            ReDim .PartTypes(.NumParts - 1)
                            For lPartsIndex As Integer = 0 To .NumParts - 1
                                .PartTypes(lPartsIndex) = sFnRdr.ReadInt32
                            Next
                        End If
                    End If

                    ReDim .thePoints(.NumPoints - 1)
                    For lPointIndex As Integer = 0 To .NumPoints - 1
                        With .thePoints(lPointIndex)
                            .x = sFnRdr.ReadDouble
                            .y = sFnRdr.ReadDouble
                        End With
                    Next

                    Select Case .ShapeType 'Read Z values for shapes with Z
                        Case FILETYPEENUM.typePolyLineZ, FILETYPEENUM.typePolygonZ, FILETYPEENUM.typeMultiPointZ, FILETYPEENUM.typeMultiPatch
                            .Zmin = sFnRdr.ReadDouble
                            .Zmax = sFnRdr.ReadDouble
                            ReDim .Zarray(.NumPoints - 1)
                            For lPointIndex As Integer = 0 To .NumPoints - 1
                                .Zarray(lPointIndex) = sFnRdr.ReadDouble
                            Next
                    End Select

                    Select Case .ShapeType 'Read M values for shapes with M
                        Case FILETYPEENUM.typePolyLineZ, FILETYPEENUM.typePolygonZ, FILETYPEENUM.typeMultiPointZ, FILETYPEENUM.typePolyLineM, FILETYPEENUM.typePolygonM, FILETYPEENUM.typeMultiPointM, FILETYPEENUM.typeMultiPatch
                            .Mmin = sFnRdr.ReadDouble
                            .Mmax = sFnRdr.ReadDouble
                            ReDim .Marray(.NumPoints - 1)
                            For lPointIndex As Integer = 0 To .NumPoints - 1
                                .Marray(lPointIndex) = sFnRdr.ReadDouble
                            Next
                    End Select
                End With
            Else
                Err.Raise(ShapeIOError.OffsetPastEOF, "CShape_IO::getPoly", "The offset read from index file is larger than the mainfile size")
            End If
            Return Polygon
            Exit Property

ERR_ROUTINE:
            Err.Raise(Err.Number, Err.Source, Err.Description)

        End Get
    End Property


    '********************************************************************************
    'Function getShapeName
    'Returns the name of the specified shape type, or of the file's shape type if not specified
    '********************************************************************************
    Public Function getShapeName(Optional ByRef aShapeType As Integer = -1) As String
        If aShapeType = -1 Then aShapeType = ShapeFileHeader.ShapeType
        Select Case aShapeType
            Case FILETYPEENUM.typeNullShape : getShapeName = "Null"
            Case FILETYPEENUM.typePoint : getShapeName = "Point"
            Case FILETYPEENUM.typePointM : getShapeName = "PointM"
            Case FILETYPEENUM.typePointZ : getShapeName = "PointZ"
            Case FILETYPEENUM.typePolyline : getShapeName = "Polyline"
            Case FILETYPEENUM.typePolygon : getShapeName = "Polygon"
            Case FILETYPEENUM.typeMultipoint : getShapeName = "Multipoint"
            Case FILETYPEENUM.typePolyLineZ : getShapeName = "PolyLineZ"
            Case FILETYPEENUM.typePolygonZ : getShapeName = "PolygonZ"
            Case FILETYPEENUM.typeMultiPointZ : getShapeName = "MultiPointZ"
            Case FILETYPEENUM.typePolyLineM : getShapeName = "PolyLineM"
            Case FILETYPEENUM.typePolygonM : getShapeName = "PolygonM"
            Case FILETYPEENUM.typeMultiPointM : getShapeName = "MultiPointM"
            Case FILETYPEENUM.typeMultiPatch : getShapeName = "MultiPatch"
            Case Else : getShapeName = "Unknown(" & aShapeType & ")"
        End Select
    End Function

    '********************************************************************************
    ' sub getRecordHeader
    ' reads the individual record header given the offset to the header
    '********************************************************************************
    Private Sub getRecordHeader(ByRef offset As Integer)
        sFnRdr.BaseStream.Seek(offset, SeekOrigin.Begin)
        With RecordHeader
            .ContentLength = SwapBytes(sFnRdr.ReadInt32)
            .RecordNumber = SwapBytes(sFnRdr.ReadInt32)
        End With
    End Sub

    '********************************************************************************
    'Function FindOffset
    ' given a valid record number, returns the offset to the record
    'offset is gotten from the index file
    '********************************************************************************
    Private Function FindOffset(ByVal RecordNumber As Integer) As Boolean
        Dim ByteOffset As Integer

        ByteOffset = FILESIZES.MainHeaderSize + ((RecordNumber - 1) * 8) + 1

        If ByteOffset > IndexFileHeader.FileLength * 2 - (FILESIZES.IndexRecSize - 1) Then
            FindOffset = False 'offset is greater than length of the main file
        Else
            FileGet(iFnR, IndexRecordHeader, ByteOffset)
            With IndexRecordHeader
                .ContentLength = SwapBytes(.ContentLength)
                .offset = SwapBytes(.offset)
            End With
            FindOffset = True
        End If
    End Function

    '******************************************************************************************
    'function chkForErrBeforeRW
    'called by all of the put and get procedures
    'it checks if the file is open, for the correct shape type, and if the record number is valid
    '
    'returns true on success
    'otherwise it raises and error
    Private Function chkForErrBeforeRW(ByVal RecordNumber As Integer, ByVal aSource As String, ByVal ShapeType As Object) As Boolean
        On Error GoTo 0

        If ShapeIsOpen = False Then
            Err.Raise(ShapeIOError.filenotopen, "CShape_IO::chkForErrBeforeRW", "You have to open the shapefile before you can read it")
        End If
        If ShapeFileHeader.ShapeType <> ShapeType Then
            If ShapeType = FILETYPEENUM.typePolygon Then
                Select Case ShapeFileHeader.ShapeType
                    Case FILETYPEENUM.typePolyline, FILETYPEENUM.typePolygon, FILETYPEENUM.typeMultipoint, FILETYPEENUM.typePolyLineZ, FILETYPEENUM.typePolygonZ, FILETYPEENUM.typeMultiPointZ, FILETYPEENUM.typePolyLineM, FILETYPEENUM.typePolygonM, FILETYPEENUM.typeMultiPointM, FILETYPEENUM.typeMultiPatch
                        'Not really a mismatch, ignore
                    Case Else
                        Err.Raise(ShapeIOError.WrongShapeType, aSource, "Shapefile header indicates type " & ShapeFileHeader.ShapeType & " but shape " & RecordNumber & " is type " & ShapeType)
                End Select
            Else
                Err.Raise(ShapeIOError.WrongShapeType, aSource, "Shapefile header indicates type " & ShapeFileHeader.ShapeType & " but shape " & RecordNumber & " is type " & ShapeType)
            End If
        End If
        If RecordNumber > RecordCount Or RecordNumber < 0 Then
            Err.Raise(ShapeIOError.NoSuchRecordERROR, aSource, "You passed an invalid record number")
        End If
    End Function

    '*******************************************************************************************
    'function CreateTmpShp
    'called by one of the put functions if we have a valid record number

    Private Function CreateTmpShp(ByRef iTfn As Integer, ByRef sTfnWrt As BinaryWriter, ByVal ShapeType As Integer) As Boolean
        'Dim ShpTmpHeader As ShapeDefines.T_MainFileHeader
        'Dim IndxTmpHeader As ShapeDefines.T_MainFileHeader

        On Error GoTo 0

        sTfnWrt = New BinaryWriter(File.Open(thePath & shptmp, FileMode.OpenOrCreate))
        iTfn = FreeFile()
        FileOpen(iTfn, thePath & indxtmp, OpenMode.Binary)
        'ShpTmpHeader = ShapeFileHeader
        'IndxTmpHeader = IndexFileHeader
        'Put sTfn, , ShpTmpHeader
        'Put iTfn, , IndxTmpHeader
        FilePut(sTfn, ShapeFileHeader)
        FilePut(iTfn, IndexFileHeader)
        CreateTmpShp = True

    End Function

    '******************************************************************************************
    'Sub putXYPoint
    'called by the user to either write a new polygon Record or update an existing one
    'RecordNumber = 0 = Append will append a record to the end of the file
    'any valid RecordNumber between 1 and RecordCount will update the record with theRecord
    'Temps are created and filled out with the correct records then the originals are
    'deleted and the temps are renamed

    Friend Sub putXYPoint(ByVal RecordNumber As Integer, ByRef theRecord As ShapeDefines.T_shpXYPoint)
        Dim iTfn As Integer
        Dim sTfnWrt As BinaryWriter
        Dim i As Integer
        Dim IOrecord As ShapeDefines.T_shpXYPoint
        Dim shpRecHdr As ShapeDefines.T_RecordHeader
        Dim indxRecHdr As ShapeDefines.T_IndexRecordHeader
        Dim shpFileSize As Integer
        Dim indxfilesize As Integer
        Dim recsize As Integer

        On Error GoTo ERR_ROUTINE

        If RDONLY = True Then
            Err.Raise(ShapeIOError.FileIsReadOnly, "CShape_IO::putXYPoint", "The file is opened as readonly")
            Exit Sub
        End If
        chkForErrBeforeRW(RecordNumber, "CShape_IO::UpdateXYPoint", FILETYPEENUM.typePoint)
        If RecordNumber <> Append Then
            CreateTmpShp(iTfn, sTfnWrt, FILETYPEENUM.typePoint)
            shpFileSize = FILESIZES.MainHeaderSize
            indxfilesize = FILESIZES.MainHeaderSize
            For i = 1 To RecordNumber - 1
                IOrecord = getXYPoint(i)
                Select Case IOrecord.ShapeType
                    Case FILETYPEENUM.typePoint : recsize = 20
                    Case FILETYPEENUM.typePointZ : recsize = 36
                    Case FILETYPEENUM.typePointM : recsize = 28
                    Case Else : Err.Raise(ShapeIOError.TypeNotSupported, "PutXYPoint", "Shape type " & IOrecord.ShapeType & " not supported as point")
                End Select
                shpRecHdr.RecordNumber = SwapBytes(RecordHeader.RecordNumber)
                shpRecHdr.ContentLength = SwapBytes(recsize / 2)
                indxRecHdr.ContentLength = SwapBytes(recsize / 2)
                indxRecHdr.offset = SwapBytes((shpFileSize) / 2)
                shpFileSize = shpFileSize + recsize + FILESIZES.shpRecSize
                indxfilesize = indxfilesize + FILESIZES.IndexRecSize
                FilePut(sTfn, shpRecHdr)
                FilePut(sTfn, IOrecord)
                FilePut(iTfn, indxRecHdr)
            Next

            recsize = Len(theRecord)
            shpRecHdr.ContentLength = SwapBytes(recsize / 2)
            shpRecHdr.RecordNumber = SwapBytes(RecordNumber)
            indxRecHdr.ContentLength = SwapBytes(recsize / 2)
            indxRecHdr.offset = SwapBytes(shpFileSize / 2)
            shpFileSize = shpFileSize + recsize + FILESIZES.shpRecSize
            indxfilesize = indxfilesize + FILESIZES.IndexRecSize
            FilePut(sTfn, shpRecHdr)
            FilePut(sTfn, theRecord)
            FilePut(iTfn, indxRecHdr)

            For i = RecordNumber + 1 To RecordCount
                IOrecord = getXYPoint(i)
                recsize = Len(IOrecord)
                shpRecHdr.RecordNumber = SwapBytes(RecordHeader.RecordNumber)
                shpRecHdr.ContentLength = SwapBytes(recsize / 2)
                indxRecHdr.ContentLength = SwapBytes(recsize / 2)
                indxRecHdr.offset = SwapBytes(shpFileSize / 2)
                shpFileSize = shpFileSize + recsize + FILESIZES.shpRecSize
                indxfilesize = indxfilesize + FILESIZES.IndexRecSize
                FilePut(sTfn, shpRecHdr)
                FilePut(sTfn, IOrecord)
                FilePut(iTfn, indxRecHdr)
            Next

            FilePut(sTfn, SwapBytes(shpFileSize / 2), CInt(HEADEROFFSETS.FileLength))
            FilePut(sTfn, SwapBytes(shpFileCode), CInt(HEADEROFFSETS.FileCode))
            FilePut(iTfn, SwapBytes(indxfilesize / 2), CInt(HEADEROFFSETS.FileLength))
            FilePut(iTfn, SwapBytes(shpFileCode), CInt(HEADEROFFSETS.FileCode))

            With theRecord.thePoint
                If .x < ShapeFileHeader.BndBoxXmin Then
                    FilePut(sTfn, .x, CInt(HEADEROFFSETS.xMin))
                    FilePut(iTfn, .x, CInt(HEADEROFFSETS.xMin))
                End If
                If .x > ShapeFileHeader.BndBoxXmax Then
                    FilePut(sTfn, .x, CInt(HEADEROFFSETS.xMax))
                    FilePut(iTfn, .x, CInt(HEADEROFFSETS.xMax))
                End If
                If .y < ShapeFileHeader.BndBoxYmin Then
                    FilePut(sTfn, .y, CInt(HEADEROFFSETS.yMin))
                    FilePut(iTfn, .y, CInt(HEADEROFFSETS.yMin))
                End If
                If .y > ShapeFileHeader.BndBoxYmax Then
                    FilePut(sTfn, .y, CInt(HEADEROFFSETS.yMax))
                    FilePut(iTfn, .y, CInt(HEADEROFFSETS.yMax))
                End If
            End With
            FileClose((sTfn))
            FileClose((iTfn))
            sFnRdr.Close()
            ShapeIsOpen = False
            FileClose((iFnR))
            IndexIsOpen = False
            Kill((ShapeFilename))
            Kill((IndexfileName))
            Rename((thePath & shptmp), ShapeFilename)
            Rename((thePath & indxtmp), IndexfileName)
            ShapeFileOpen(ShapeFilename, READWRITEFLAG.Readwrite)
        Else
            recsize = Len(theRecord)
            shpFileSize = sFnRdr.BaseStream.Length
            indxfilesize = LOF(iFnR)
            shpRecHdr.ContentLength = SwapBytes(recsize / 2)
            shpRecHdr.RecordNumber = SwapBytes(RecordCount + 1)
            indxRecHdr.ContentLength = SwapBytes(recsize / 2)
            indxRecHdr.offset = SwapBytes(shpFileSize / 2)
            sFnWrt.BaseStream.Seek(shpFileSize + 1, SeekOrigin.Begin)
            sFnWrt.Write(shpRecHdr.RecordNumber)
            sFnWrt.Write(shpRecHdr.ContentLength)
            With theRecord
                sFnWrt.Write(.ShapeType)
                With .thePoint
                    sFnWrt.Write(.x)
                    sFnWrt.Write(.y)
                End With
            End With
            FilePut(iFnR, indxRecHdr, indxfilesize + 1)
            shpFileSize = shpFileSize + recsize + FILESIZES.shpRecSize
            indxfilesize = indxfilesize + FILESIZES.IndexRecSize
            RecordCount = RecordCount + 1
            ShapeFileHeader.FileLength = shpFileSize / 2
            IndexFileHeader.FileLength = indxfilesize / 2
            sFnWrt.BaseStream.Seek(CInt(HEADEROFFSETS.FileLength), SeekOrigin.Begin)
            sFnWrt.Write(SwapBytes(shpFileSize / 2))
            FilePut(iFnR, SwapBytes(indxfilesize / 2), CInt(HEADEROFFSETS.FileLength))
            With theRecord.thePoint
                If .x < ShapeFileHeader.BndBoxXmin Then
                    sFnWrt.BaseStream.Seek(CInt(HEADEROFFSETS.xMin), SeekOrigin.Begin)
                    sFnWrt.Write(.x)
                    FilePut(iFnR, .x, CInt(HEADEROFFSETS.xMin))
                    ShapeFileHeader.BndBoxXmin = .x
                    IndexFileHeader.BndBoxXmin = .x
                End If
                If .x > ShapeFileHeader.BndBoxXmax Then
                    sFnWrt.BaseStream.Seek(CInt(HEADEROFFSETS.xMax), SeekOrigin.Begin)
                    sFnWrt.Write(.x)
                    FilePut(iFnR, .x, CInt(HEADEROFFSETS.xMax))
                    ShapeFileHeader.BndBoxXmax = .x
                    IndexFileHeader.BndBoxXmax = .x
                End If
                If .y < ShapeFileHeader.BndBoxYmin Then
                    sFnWrt.BaseStream.Seek(CInt(HEADEROFFSETS.yMin), SeekOrigin.Begin)
                    sFnWrt.Write(.y)
                    FilePut(iFnR, .y, CInt(HEADEROFFSETS.yMin))
                    ShapeFileHeader.BndBoxYmin = .y
                    IndexFileHeader.BndBoxYmin = .y
                End If
                If .y > ShapeFileHeader.BndBoxYmax Then
                    sFnWrt.BaseStream.Seek(CInt(HEADEROFFSETS.yMax), SeekOrigin.Begin)
                    sFnWrt.Write(.y)
                    FilePut(iFnR, .y, CInt(HEADEROFFSETS.yMax))
                    ShapeFileHeader.BndBoxYmax = .y
                    IndexFileHeader.BndBoxYmax = .y
                End If
            End With
        End If
        Exit Sub
ERR_ROUTINE:
        Err.Raise(Err.Number, Err.Source, Err.Description)
    End Sub
    '********************************************************************************
    'property getXYMultiPoint
    'given a valid record number a points record is returned
    '********************************************************************************
    'Friend Property Get getXYMultiPoint(ByVal RecordNumber As Long) As ShapeDefines.T_shpXYMultiPoint
    '    On Error GoTo ERR_ROUTINE
    '
    '    chkForErrBeforeRW RecordNumber, "CShape_IO::getXYMultiPoint", FILETYPEENUM.typeMultiPoint
    '
    '    If FindOffset(RecordNumber) Then
    '        getRecordHeader (IndexRecordHeader.offset * 2 + 1)
    '        With XYMultiPoint
    '            Get sFnR, , .ShapeType
    '            If .ShapeType = typeMultiPoint Then
    '              Get sFnR, , .Box
    '              Get sFnR, , .NumPoints
    '              ReDim .thePoints(0 To .NumPoints - 1)
    '              Get sFnR, , .thePoints
    '            End If
    '        End With
    '    Else
    '       Err.Raise OffsetPastEOF, "CShape_IO::getXYMultiPoint", "The offset read from index file is larger than mainfile size"
    '    End If
    '    getXYMultiPoint = XYMultiPoint
    '    Exit Property
    '
    'ERR_ROUTINE:
    '    Err.Raise Err.Number, Err.Source, Err.Description
    'End Property


    ''******************************************************************************************
    ''Sub putPXYMultiPoint
    ''called by the user to either write a new polygon Record or update an existing one
    ''RecordNumber = 0 = Append will append a record to the end of the file
    ''any valid RecordNumber between 1 and RecordCount will update the record with theRecord
    ''
    ''Temps are created and filled out with the correct records then the originals are
    ''deleted and the temps are renamed
    '
    'Friend Sub putXYMultiPoint(ByVal RecordNumber As Long, theRecord As T_shpXYMultiPoint)
    '    Dim iTfn As Long
    '    Dim sTfn As Long
    '    Dim i As Long
    '    Dim IOrecord As ShapeDefines.T_shpXYMultiPoint
    '    Dim shpRecHdr As ShapeDefines.T_RecordHeader
    '    Dim indxRecHdr As ShapeDefines.T_IndexRecordHeader
    '    Dim shpFileSize As Long
    '    Dim indxfilesize As Long
    '    Dim recsize As Long
    '
    '    On Error GoTo ERR_ROUTINE
    '
    '    If RDONLY = True Then
    '        Err.Raise FileIsReadOnly, "CShape_IO::putXYMultiPoint", "The file is opened as readonly"
    '        Exit Sub
    '    End If
    '    chkForErrBeforeRW RecordNumber, "CShape_IO::UpdateXYMultiPoint", FILETYPEENUM.typeMultiPoint
    '    If RecordNumber <> Append Then
    '        CreateTmpShp iTfn, sTfn, FILETYPEENUM.typeMultiPoint
    '        shpFileSize = FILESIZES.MainHeaderSize
    '        indxfilesize = FILESIZES.MainHeaderSize
    '        For i = 1 To RecordNumber - 1
    '            IOrecord = getXYMultiPoint(i)
    '            recsize = Len(IOrecord.Box) + Len(IOrecord.NumPoints) + _
    ''                Len(IOrecord.ShapeType) + (UBound(IOrecord.thePoints, 1) + 1) * 16
    '            shpRecHdr.RecordNumber = SwapBytes(RecordHeader.RecordNumber)
    '            shpRecHdr.ContentLength = SwapBytes(recsize / 2)
    '            indxRecHdr.ContentLength = SwapBytes(recsize / 2)
    '            indxRecHdr.offset = SwapBytes((shpFileSize) / 2)
    '            shpFileSize = shpFileSize + recsize + FILESIZES.shpRecSize
    '            indxfilesize = indxfilesize + FILESIZES.IndexRecSize
    '            Put sTfn, , shpRecHdr
    '            Put sTfn, , IOrecord.ShapeType
    '            Put sTfn, , IOrecord.Box
    '            Put sTfn, , IOrecord.NumPoints
    '            Put sTfn, , IOrecord.thePoints
    '            Put iTfn, , indxRecHdr
    '        Next
    '
    '        recsize = Len(theRecord.Box) + Len(theRecord.NumPoints) + _
    ''                Len(theRecord.ShapeType) + (UBound(theRecord.thePoints, 1) + 1) * 16
    '        shpRecHdr.ContentLength = SwapBytes(recsize / 2)
    '        shpRecHdr.RecordNumber = SwapBytes(RecordNumber)
    '        indxRecHdr.ContentLength = SwapBytes(recsize / 2)
    '        indxRecHdr.offset = SwapBytes(shpFileSize / 2)
    '        shpFileSize = shpFileSize + recsize + FILESIZES.shpRecSize
    '        indxfilesize = indxfilesize + FILESIZES.IndexRecSize
    '        Put sTfn, , shpRecHdr
    '        Put sTfn, , theRecord.ShapeType
    '        Put sTfn, , theRecord.Box
    '        Put sTfn, , theRecord.NumPoints
    '        Put sTfn, , theRecord.thePoints
    '        Put iTfn, , indxRecHdr
    '
    '       For i = RecordNumber + 1 To RecordCount
    '           IOrecord = getXYMultiPoint(i)
    '           recsize = Len(IOrecord.Box) + Len(IOrecord.NumPoints) + _
    ''                Len(IOrecord.ShapeType) + (UBound(IOrecord.thePoints, 1) + 1) * 16
    '           shpRecHdr.RecordNumber = SwapBytes(RecordHeader.RecordNumber)
    '           shpRecHdr.ContentLength = SwapBytes(recsize / 2)
    '           indxRecHdr.ContentLength = SwapBytes(recsize / 2)
    '           indxRecHdr.offset = SwapBytes(shpFileSize / 2)
    '           shpFileSize = shpFileSize + recsize + FILESIZES.shpRecSize
    '           indxfilesize = indxfilesize + FILESIZES.IndexRecSize
    '           Put sTfn, , shpRecHdr
    '           Put sTfn, , IOrecord.ShapeType
    '           Put sTfn, , IOrecord.Box
    '           Put sTfn, , IOrecord.NumPoints
    '           Put sTfn, , IOrecord.thePoints
    '           Put iTfn, , indxRecHdr
    '        Next
    '
    '        Put sTfn, HEADEROFFSETS.FileLength, SwapBytes(shpFileSize / 2)
    '        Put sTfn, FileCode, SwapBytes(shpFileCode)
    '        Put iTfn, HEADEROFFSETS.FileLength, SwapBytes(indxfilesize / 2)
    '        Put iTfn, FileCode, SwapBytes(shpFileCode)
    '        UpdateBoundingBox theRecord.Box, sTfn, iTfn
    '
    '        Close (sTfn)
    '        Close (iTfn)
    '        Close (sFnR)
    '        ShapeIsOpen = False
    '        Close (iFnR)
    '        IndexIsOpen = False
    '        Kill (ShapeFilename)
    '        Kill (IndexfileName)
    '        Name (thePath + shptmp) As ShapeFilename
    '        Name (thePath + indxtmp) As IndexfileName
    '        ShapeFileOpen ShapeFilename, READWRITEFLAG.Readwrite
    '    Else
    '        sTfn = sFnR
    '        iTfn = iFnR
    '        recsize = Len(theRecord.Box) + Len(theRecord.NumPoints) + _
    ''                Len(theRecord.ShapeType) + (UBound(theRecord.thePoints, 1) + 1) * 16
    '        shpFileSize = LOF(sFnR)
    '        indxfilesize = LOF(iFnR)
    '        shpRecHdr.ContentLength = SwapBytes(recsize / 2)
    '        shpRecHdr.RecordNumber = SwapBytes(RecordCount + 1)
    '        indxRecHdr.ContentLength = SwapBytes(recsize / 2)
    '        indxRecHdr.offset = SwapBytes(shpFileSize / 2)
    '        Put sFnR, shpFileSize + 1, shpRecHdr
    '        Put sFnR, , theRecord.ShapeType
    '        Put sTfn, , theRecord.Box
    '        Put sTfn, , theRecord.NumPoints
    '        Put sTfn, , theRecord.thePoints
    '        Put iFnR, indxfilesize + 1, indxRecHdr
    '        shpFileSize = shpFileSize + recsize + FILESIZES.shpRecSize
    '        indxfilesize = indxfilesize + FILESIZES.IndexRecSize
    '        RecordCount = RecordCount + 1
    '        ShapeFileHeader.FileLength = shpFileSize / 2
    '        IndexFileHeader.FileLength = indxfilesize / 2
    '        Put sFnR, HEADEROFFSETS.FileLength, SwapBytes(shpFileSize / 2)
    '        Put iFnR, HEADEROFFSETS.FileLength, SwapBytes(indxfilesize / 2)
    '        UpdateBoundingBox theRecord.Box, sTfn, iTfn
    '    End If
    '    Exit Sub
    'ERR_ROUTINE:
    '    Err.Raise Err.Number, Err.Source, Err.Description
    'End Sub

    '******************************************************************************************
    'Sub UpdateBoundingBox
    'called by the put subroutines to make sure the bounding box of a shape being written to
    'the shape file falls inside the bounding box in the header.
    '
    'Box is the bounding box of the new shape
    'shap is a file handle to the shape file being written
    'indx is a file handle to the index file being written
    '
    'If Box extends the current bounding box, ShapeFileHeader and IndexFileHeader are
    'updated and new values are written to the headers of the files shap and indx
    Private Sub UpdateBoundingBox(ByRef Box As T_BoundingBox, ByRef shap As Integer, ByRef indx As Integer)
        With Box
            If .xMin < ShapeFileHeader.BndBoxXmin Then
                FilePut(shap, .xMin, CInt(HEADEROFFSETS.xMin))
                FilePut(indx, .xMin, CInt(HEADEROFFSETS.xMin))
                ShapeFileHeader.BndBoxXmin = .xMin
                IndexFileHeader.BndBoxXmin = .xMin
            End If
            If .xMax > ShapeFileHeader.BndBoxXmax Then
                FilePut(shap, .xMax, CInt(HEADEROFFSETS.xMax))
                FilePut(indx, .xMax, CInt(HEADEROFFSETS.xMax))
                ShapeFileHeader.BndBoxXmax = .xMax
                IndexFileHeader.BndBoxXmax = .xMax
            End If
            If .yMin < ShapeFileHeader.BndBoxYmin Then
                FilePut(shap, .yMin, CInt(HEADEROFFSETS.yMin))
                FilePut(indx, .yMin, CInt(HEADEROFFSETS.yMin))
                ShapeFileHeader.BndBoxYmin = .yMin
                IndexFileHeader.BndBoxYmin = .yMin
            End If
            If Box.yMax > ShapeFileHeader.BndBoxYmax Then
                FilePut(shap, .yMax, CInt(HEADEROFFSETS.yMax))
                FilePut(indx, .yMax, CInt(HEADEROFFSETS.yMax))
                ShapeFileHeader.BndBoxYmax = .yMax
                IndexFileHeader.BndBoxYmax = .yMax
            End If
        End With
    End Sub

    '******************************************************************************************
    'Sub putPoly
    'called by the user to write a new polygon, polyline, or XYMultiPoint Record or update an existing one
    'RecordNumber = 0 = Append will append a record to the end of the file
    'any valid RecordNumber between 1 and RecordCount will update the record with theRecord
    '
    'Temps are created and filled out with the correct records then originals are
    'deleted and the temps are renamed

    Friend Sub putPoly(ByVal RecordNumber As Integer, ByRef theRecord As T_shpPoly)
        Dim iTfn As Integer
        Dim sTfnWrt As BinaryWriter
        Dim i As Integer
        Dim IOrecord As ShapeDefines.T_shpPoly
        Dim shpRecHdr As ShapeDefines.T_RecordHeader
        Dim indxRecHdr As ShapeDefines.T_IndexRecordHeader
        Dim shpFileSize As Integer
        Dim indxfilesize As Integer
        Dim recsize As Integer

        On Error GoTo ERR_ROUTINE

        If RDONLY = True Then
            Err.Raise(ShapeIOError.FileIsReadOnly, "CShape_IO::putPoly", "The file is opened as readonly")
            Exit Sub
        End If

        chkForErrBeforeRW(RecordNumber, "CShape_IO::putPoly", theRecord.ShapeType)
        If RecordNumber <> Append Then
            CreateTmpShp(iTfn, sTfnWrt, ShapeFileHeader.ShapeType)
            shpFileSize = FILESIZES.MainHeaderSize
            indxfilesize = FILESIZES.MainHeaderSize
            For i = 1 To RecordNumber - 1
                IOrecord = getPoly(i)
                recsize = GetRecSize(IOrecord)
                shpRecHdr.RecordNumber = SwapBytes(RecordHeader.RecordNumber)
                shpRecHdr.ContentLength = SwapBytes(recsize / 2)
                indxRecHdr.ContentLength = SwapBytes(recsize / 2)
                indxRecHdr.offset = SwapBytes((shpFileSize) / 2)
                shpFileSize = shpFileSize + recsize + FILESIZES.shpRecSize
                indxfilesize = indxfilesize + FILESIZES.IndexRecSize
                FilePut(sTfn, shpRecHdr)
                putPolyShape(sTfn, IOrecord)
                FilePut(iTfn, indxRecHdr)
            Next

            recsize = GetRecSize(theRecord)
            shpRecHdr.ContentLength = SwapBytes(recsize / 2)
            shpRecHdr.RecordNumber = SwapBytes(RecordNumber)
            indxRecHdr.ContentLength = SwapBytes(recsize / 2)
            indxRecHdr.offset = SwapBytes(shpFileSize / 2)
            shpFileSize = shpFileSize + recsize + FILESIZES.shpRecSize
            indxfilesize = indxfilesize + FILESIZES.IndexRecSize
            FilePut(sTfn, shpRecHdr)
            putPolyShape(sTfn, theRecord)
            FilePut(iTfn, indxRecHdr)

            For i = RecordNumber + 1 To RecordCount
                IOrecord = getPoly(i)
                recsize = GetRecSize(IOrecord)
                shpRecHdr.RecordNumber = SwapBytes(RecordHeader.RecordNumber)
                shpRecHdr.ContentLength = SwapBytes(recsize / 2)
                indxRecHdr.ContentLength = SwapBytes(recsize / 2)
                indxRecHdr.offset = SwapBytes(shpFileSize / 2)
                shpFileSize = shpFileSize + recsize + FILESIZES.shpRecSize
                indxfilesize = indxfilesize + FILESIZES.IndexRecSize
                FilePut(sTfn, shpRecHdr)
                putPolyShape(sTfn, IOrecord)
                FilePut(iTfn, indxRecHdr)
            Next

            FilePut(sTfn, SwapBytes(shpFileSize / 2), CInt(HEADEROFFSETS.FileLength))
            FilePut(sTfn, SwapBytes(shpFileCode), CInt(HEADEROFFSETS.FileCode))
            FilePut(iTfn, SwapBytes(indxfilesize / 2), CInt(HEADEROFFSETS.FileLength))
            FilePut(iTfn, SwapBytes(shpFileCode), CInt(HEADEROFFSETS.FileCode))
            UpdateBoundingBox(theRecord.Box, sTfn, iTfn)

            FileClose((sTfn))
            FileClose((iTfn))

            ShapeIsOpen = False
            sFnRdr.Close()
            IndexIsOpen = False
            Kill((ShapeFilename))
            Kill((IndexfileName))
            Rename((thePath & shptmp), ShapeFilename)
            Rename((thePath & indxtmp), IndexfileName)
            ShapeFileOpen(ShapeFilename, READWRITEFLAG.Readwrite)
        Else
            sTfnWrt = New BinaryWriter(sFnRdr.BaseStream)
            iTfn = iFnR
            recsize = GetRecSize(theRecord)
            shpFileSize = sFnRdr.BaseStream.Length
            indxfilesize = LOF(iFnR)
            shpRecHdr.ContentLength = SwapBytes(recsize / 2)
            shpRecHdr.RecordNumber = SwapBytes(RecordCount + 1)
            indxRecHdr.ContentLength = SwapBytes(recsize / 2)
            indxRecHdr.offset = SwapBytes(shpFileSize / 2)

            FilePut(sTfn, shpRecHdr, shpFileSize + 1)
            putPolyShape(sTfn, theRecord)
            FilePut(iTfn, indxRecHdr, indxfilesize + 1)

            shpFileSize = shpFileSize + recsize + FILESIZES.shpRecSize
            indxfilesize = indxfilesize + FILESIZES.IndexRecSize
            RecordCount = RecordCount + 1
            ShapeFileHeader.FileLength = shpFileSize / 2
            IndexFileHeader.FileLength = indxfilesize / 2
            FilePut(sTfn, SwapBytes(ShapeFileHeader.FileLength), CInt(HEADEROFFSETS.FileLength))
            FilePut(iTfn, SwapBytes(IndexFileHeader.FileLength), CInt(HEADEROFFSETS.FileLength))
            UpdateBoundingBox(theRecord.Box, sTfn, iTfn)
        End If
        Exit Sub
ERR_ROUTINE:
        Err.Raise(Err.Number, Err.Source, Err.Description)
    End Sub

    Private Sub putPolyShape(ByRef sf As Integer, ByRef aPoly As ShapeDefines.T_shpPoly)
        '    typeMultiPoint = 8
        '    typePointZ = 11
        '    typePolyLineZ = 13
        '    typePolygonZ = 15
        '    typeMultiPointZ = 18
        '    typePointM = 21
        '    typePolyLineM = 23
        '    typePolygonM = 25
        '    typeMultiPointM = 28
        '    typeMultiPatch = 31

        With aPoly
            FilePut(sf, .ShapeType)
            FilePut(sf, .Box)
            Select Case .ShapeType 'MultiPoint types have no parts
                Case FILETYPEENUM.typeMultipoint, FILETYPEENUM.typeMultiPointZ, FILETYPEENUM.typeMultiPointM
                Case Else
                    FilePut(sf, .NumParts)
            End Select
            FilePut(sf, .NumPoints)
            If .NumParts > 0 Then FilePut(sf, .Parts)
            If .ShapeType = FILETYPEENUM.typeMultiPatch Then FilePut(sf, .PartTypes)
            FilePut(sf, .thePoints)

            Select Case .ShapeType 'Read Z values for shapes with Z
                Case FILETYPEENUM.typePolyLineZ, FILETYPEENUM.typePolygonZ, FILETYPEENUM.typeMultiPointZ, FILETYPEENUM.typeMultiPatch
                    FilePut(sf, .Zmin)
                    FilePut(sf, .Zmax)
                    FilePut(sf, .Zarray)
            End Select

            Select Case .ShapeType 'Read M values for shapes with M
                Case FILETYPEENUM.typePolyLineZ, FILETYPEENUM.typePolygonZ, FILETYPEENUM.typeMultiPointZ, FILETYPEENUM.typePolyLineM, FILETYPEENUM.typePolygonM, FILETYPEENUM.typeMultiPointM, FILETYPEENUM.typeMultiPatch
                    FilePut(sf, .Mmin)
                    FilePut(sf, .Mmax)
                    FilePut(sf, .Marray)
            End Select
        End With
    End Sub

    Private Function GetRecSize(ByRef aShapeRec As T_shpPoly) As Integer
        With aShapeRec
            'All remaining types have a box and points
            GetRecSize = Len(aShapeRec.ShapeType) + Len(aShapeRec.Box) + Len(aShapeRec.NumPoints) + aShapeRec.NumPoints * 16

            Select Case .ShapeType 'MultiPoint types have no parts
                Case FILETYPEENUM.typeMultipoint, FILETYPEENUM.typeMultiPointZ, FILETYPEENUM.typeMultiPointM
                Case Else 'Len(.NumParts) = 4 bytes, .NumParts + 1 = space for each part and .NumParts
                    GetRecSize = GetRecSize + Len(.NumParts) * (.NumParts + 1)
            End Select

            Select Case .ShapeType 'Count Z min/max/values for shapes with Z
                Case FILETYPEENUM.typePolyLineZ, FILETYPEENUM.typePolygonZ, FILETYPEENUM.typeMultiPointZ, FILETYPEENUM.typePolyLineM, FILETYPEENUM.typePolygonM, FILETYPEENUM.typeMultiPointM, FILETYPEENUM.typeMultiPatch
                    '.NumPoints + 1 = space for {min, max} and all points, 16 = 8 bytes/Double * 2
                    GetRecSize = GetRecSize + (aShapeRec.NumPoints + 1) * 16
            End Select

            Select Case .ShapeType 'Count M min/max/values for shapes with M
                Case FILETYPEENUM.typePolyLineM, FILETYPEENUM.typePolygonM, FILETYPEENUM.typeMultiPointM, FILETYPEENUM.typeMultiPatch
                    '.NumPoints + 1 = space for {min, max} and all points, 16 = 8 bytes/Double * 2
                    GetRecSize = GetRecSize + (aShapeRec.NumPoints + 1) * 16
            End Select

            If .ShapeType = FILETYPEENUM.typeMultiPatch Then 'Add space needed by patch type
                GetRecSize = GetRecSize + aShapeRec.NumParts * 4
            End If
        End With
    End Function


    '********************************************************************************
    'function isFileNameValid
    ' Checks for a valid shapefile name setting the global shapefilename
    ' checks for the index file and sets the global indexfilename if the index is present
    '********************************************************************************
    Function isFileNameValid(ByRef filename As String) As Boolean
        Dim place As Integer
        Dim dot As Integer
        Dim i As Integer
        Dim dotfound As Boolean
        Dim ext As Integer
        Dim extension As String
        Dim PathAndName As String
        Dim NameNoPath As String

        dotfound = False
        For i = 1 To Len(filename)
            If Mid(filename, i, 1) = "\" Then place = i
        Next
        For i = (Len(filename) - place) To Len(filename)
            If Mid(filename, i, 1) = "." Then
                dot = i
                dotfound = True
            End If
        Next
        If dotfound Then
            ext = Len(filename) - dot
            extension = Right(filename, 3)
            PathAndName = Left(filename, Len(filename) - ext - 1)
            NameNoPath = Right(PathAndName, Len(PathAndName) - place)
            If extension = "shp" Or extension = "SHP" Then
                ShapeFilename = filename
                IndexfileName = PathAndName & ".shx"
                If Not IO.File.Exists(IndexfileName) Then
                    CreateAnIndex()
                End If
                isFileNameValid = True
                thePath = Left(filename, place)
            Else
                isFileNameValid = False
                thePath = "nada"
            End If
        Else
            thePath = "nada"
            isFileNameValid = False
        End If

    End Function

    '****************************************************************************************
    'Function FixExtensions
    'Used to parse the file name and set the Path Variable, Shapefilename Var,
    'IndexFileName Var, and PathAndName Var

    Private Function FixExtensions(ByVal filename As String) As Boolean
        Dim place As Integer
        Dim dot As Integer
        Dim i As Integer
        Dim dotfound As Boolean
        Dim ext As Integer
        Dim PathAndName As String

        dotfound = False
        For i = 1 To Len(filename)
            If Mid(filename, i, 1) = "\" Then place = i
        Next
        For i = (Len(filename) - place) To Len(filename)
            If Mid(filename, i, 1) = "." Then
                dot = i
                dotfound = True
            End If
        Next
        If dotfound Then
            ext = Len(filename) - dot
            PathAndName = Left(filename, Len(filename) - ext - 1)
            ShapeFilename = PathAndName & ".shp"
            IndexfileName = PathAndName & ".shx"
        Else
            ShapeFilename = filename & ".shp"
            IndexfileName = filename & ".shx"
        End If
        FixExtensions = True

    End Function

    '********************************************************************************
    'Function CreateNewShape
    'When called it will create a new .shp and .shx files and write the header
    'Returns true on success
    Public Function CreateNewShape(ByVal filename As String, ByRef ShapeType As Integer) As Boolean
        Dim result As Boolean
        On Error GoTo ERR_ROUTINE

        If ShapeIsOpen = True Then
            Err.Raise(ShapeIOError.FileAlreadyOpen, "CShape_IO::CreateNewShape", "A Shapefile is already open for this instance")
        End If

        result = FixExtensions(filename)
        If IO.File.Exists(ShapeFilename) Then Kill(ShapeFilename)
        sFnWrt = New BinaryWriter(File.Open(ShapeFilename, FileMode.OpenOrCreate))
        ShapeIsOpen = True
        iFnR = FreeFile
        If IO.File.Exists(IndexfileName) Then Kill(IndexfileName)
        FileOpen(iFnR, IndexfileName, OpenMode.Binary)
        IndexIsOpen = True
        With ShapeFileHeader
            .FileCode = shpFileCode
            .FileLength = FILESIZES.MainHeaderSize / 2
            .version = 1000
            .ShapeType = ShapeType
            .BndBoxXmin = 1.0E+300
            .BndBoxYmin = 1.0E+300
            .BndBoxZmin = 1.0E+300
            .BndBoxMmin = 1.0E+300
            .BndBoxXmax = -1.0E+300
            .BndBoxYmax = -1.0E+300
            .BndBoxZmax = -1.0E+300
            .BndBoxMmax = -1.0E+300
            sFnWrt.Write(SwapBytes(.FileCode))
            sFnWrt.Write(SwapBytes(.u1))
            sFnWrt.Write(SwapBytes(.u2))
            sFnWrt.Write(SwapBytes(.u3))
            sFnWrt.Write(SwapBytes(.u4))
            sFnWrt.Write(SwapBytes(.u5))
            sFnWrt.Write(SwapBytes(.FileLength))
            sFnWrt.Write(.version)
            sFnWrt.Write(.BndBoxXmin)
            sFnWrt.Write(.BndBoxYmin)
            sFnWrt.Write(.BndBoxZmin)
            sFnWrt.Write(.BndBoxMmin)
            sFnWrt.Write(.BndBoxXmax)
            sFnWrt.Write(.BndBoxYmax)
            sFnWrt.Write(.BndBoxZmax)
            sFnWrt.Write(.BndBoxMmax)
        End With
        With IndexFileHeader
            .FileCode = SwapBytes(shpFileCode)
            .FileLength = SwapBytes(FILESIZES.MainHeaderSize / 2)
            .version = 1000
            .ShapeType = ShapeType
        End With

        FilePut(iFnR, IndexFileHeader)
        ShapeFileHeader.FileLength = FILESIZES.MainHeaderSize / 2
        IndexFileHeader.FileLength = FILESIZES.MainHeaderSize / 2
        CreateNewShape = True
        RecordCount = 0
        Exit Function

ERR_ROUTINE:
        Err.Raise(Err.Number, Err.Source, Err.Description)
    End Function

    '*******************************************************************************************
    'function FileShutDown
    'Must be called by the user to close the open shp ans shx files.
    'Returns true on success

    Public Function FileShutDown() As Boolean
        On Error GoTo 0
        If ShapeIsOpen Then
            sFnRdr.Close()
            ShapeIsOpen = False
        End If
        If IndexIsOpen Then
            FileClose((iFnR))
            IndexIsOpen = False
        End If
        FileShutDown = True
    End Function
	
	'******************************************************************************************
	'Sub CreateAnIndex
	'calling sequence Shapefileopen -> isFileNameValid -> CreateAnIndex
	'After the shape is open and if there is no .shx file then one will be created
	'by parsing the shape file
	
	Private Sub CreateAnIndex()
		Dim fn1 As Integer
		Dim fn2 As Integer
		Dim hdrShape As T_MainFileHeader
		Dim hdrIdx As T_MainFileHeader
		Dim hdrShpRec As T_RecordHeader
		Dim hdrIdxRec As T_IndexRecordHeader
		Dim idxFileSize As Integer
		Dim shpFileSize As Integer
		Dim offset As Integer
		Dim nextoffset As Integer
		Dim tmp As Integer
		Dim tmp2 As Integer
		
		On Error GoTo 0
		
		fn1 = FreeFile
		FileOpen(fn1, ShapeFilename, OpenMode.Binary)
		fn2 = FreeFile
		FileOpen(fn2, IndexfileName, OpenMode.Binary)
		
		idxFileSize = FILESIZES.MainHeaderSize
		shpFileSize = FILESIZES.MainHeaderSize
        FileGet(fn1, hdrShape)
        FilePut(fn2, hdrShape)
		nextoffset = shpFileSize + 1
		Do While Not EOF(fn1)
            FileGet(fn1, hdrShpRec, nextoffset)
			If Not EOF(fn1) Then
				nextoffset = nextoffset + SwapBytes(hdrShpRec.ContentLength) * 2 + FILESIZES.shpRecSize
				hdrIdxRec.ContentLength = hdrShpRec.ContentLength
				hdrIdxRec.offset = SwapBytes(shpFileSize / 2)
                FilePut(fn2, hdrIdxRec)
				idxFileSize = idxFileSize + FILESIZES.IndexRecSize
				shpFileSize = shpFileSize + SwapBytes(hdrShpRec.ContentLength) * 2 + FILESIZES.shpRecSize
			End If
		Loop 
        FilePut(fn2, SwapBytes(LOF(fn2) / 2), CInt(HEADEROFFSETS.FileLength))
		FileClose((fn1))
		FileClose((fn2))
	End Sub
	
    Public Sub New()
        MyBase.New()
        ShapeIsOpen = False
        IndexIsOpen = False
    End Sub
End Class