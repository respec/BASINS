VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "CShape_IO"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
Attribute VB_Ext_KEY = "SavedWithClassBuilder" ,"Yes"
Attribute VB_Ext_KEY = "Top_Level" ,"Yes"
Option Explicit
'********************************************************************************
'Class Module title :       CShape_IO
'Author :  Kenneth R. McVay
'Date   :  June 22, 1998
'Purpose : To provide IO for shapefile and their associated index files
'********************************************************************************

'********************************************************************************
'This dll is used for byte swapping the longs
'Private Declare Function Swap_32 Lib "ByteSwap.dll" (ByVal aLong As Long) As Long
'replaced with SwapBytes VB function in Utility.bas 12-1-98
'********************************************************************************

'*****************************************************************************************
'Enumeration READWRITEFLAG
' Valid flags for the ShapefileOpen procedure
Public Enum READWRITEFLAG
    ReadOnly = 0
    Readwrite = 1
End Enum

'*****************************************************************************************
'Append
'used by the put procedures
'if append is used for recordnumber then the record is added
Private Const Append = 0

'*****************************************************************************************
'const shpfilecode
'the valid code for the filecode in the main headers
Private Const shpFileCode = 9994

'*****************************************************************************************
'enumeration FILESIZES
' used for easy access to the header file sizes
Private Enum FILESIZES
    MainHeaderSize = 100       'in bytes
    IndexRecSize = 8           ' in bytes
    shpRecSize = 8
End Enum

'*****************************************************************************************
'Enumeration FILETYPEENUM
'easy access to all of the type identifiers available
Public Enum FILETYPEENUM
    typeNullShape = 0
    typePoint = 1
    typepolyline = 3
    typepolygon = 5
    typeMultiPoint = 8
    typePointZ = 11
    typePolyLineZ = 13
    typePolygonZ = 15
    typeMultiPointZ = 18
    typePointM = 21
    typePolyLineM = 23
    typePolygonM = 25
    typeMultiPointM = 28
    typeMultiPatch = 31
    typeDBF = 40
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
    zMin = 69
    zMax = 77
    mMin = 85
    mMax = 93
End Enum

'*****************************************************************************************
'Tempory shape files for updating records
Private Const shptmp = "shptmp.tmp"
Private Const indxtmp = "indxtmp.tmp"


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
    Private XYMultiPoint As ShapeDefines.T_shpXYMultiPoint
    Private PolyLine As ShapeDefines.T_shpPolyLine
    Private Polygon As ShapeDefines.T_shpPolygon
    Private PointM As ShapeDefines.T_shpPointM
    Private MultiPointM As ShapeDefines.T_shpMultiPointM
    Private PolyLineM As ShapeDefines.T_shpPolyLineM
    Private PolygonM As ShapeDefines.T_shpPolygonM
    Private PointZ As ShapeDefines.T_shpPointZ
    Private MultiPointZ As ShapeDefines.T_shpMultiPointZ
    Private PolyLineZ As ShapeDefines.T_shpPolyLineZ
    Private PolygonZ As ShapeDefines.T_shpPolygonZ
    Private MultiPatch As ShapeDefines.T_shpMultiPatch

'********************************************************************************

Private sFnR As Long         ' FileHandle for reading a shapefile
Private iFnR As Long         'Index filename for read
Private dbFnR As Long        'DataBase filename for read
Private FnW As Long         ' FileHandle for writing a shapefile
Private RDONLY As Boolean     'boolean value to identify if the shapefile should be readonly
Private ShapeFilename As String 'string to hold the current shapefile name
Private ShapeIsOpen As Boolean  'boolean value identifies if file is open
Private thePath As String
Private IndexfileName As String 'string to hold the current indexfilename  "nada" if not file exist
Private IndexIsOpen As Boolean  'boolean value identifies if file is open

Private RecordCount As Long     'variable to hold the record count. calculated from index file

'********************************************************************************
'Name ShapeFileOpen
'Arguments :    FileName  The filename of the Shapefile to open
'               ReadWriteFlag   To identify if readonly or read/write
'********************************************************************************
Public Function ShapeFileOpen(ByVal Filename As String, _
                              ByVal RWFLAG As Long) As Boolean
  On Error GoTo ERR_ROUTINE
                                 
    If RWFLAG = READWRITEFLAG.ReadOnly Then
        RDONLY = True
    Else
        RDONLY = False
    End If
    
    If isFileNameValid(Filename) = False Then   'Raise error if something is wrong with shapefile
        Err.Raise InvalidShapeFile, "CShape_IO::ShapefileOpen", _
                    "The specified shapefile is not valid or extension is invalid"
        Exit Function
    End If
    
    
        iFnR = FreeFile()
        Open IndexfileName For Binary As iFnR   'open the index file
        Get iFnR, , IndexFileHeader
        With IndexFileHeader
            .FileCode = SwapBytes(.FileCode)
            .FileLength = SwapBytes(.FileLength)      'read in the indexfile header
            .u1 = SwapBytes(.u1)
            .u2 = SwapBytes(.u2)
            .u3 = SwapBytes(.u3)
            .u4 = SwapBytes(.u4)
            .u5 = SwapBytes(.u5)
        End With
        If IndexFileHeader.FileCode = shpFileCode Then
            IndexIsOpen = True
            RecordCount = (IndexFileHeader.FileLength - 50) / 4         'set the globe RecordCount
        Else
            Close (iFnR)        'There was a problem with the index file
        End If
   
    
    sFnR = FreeFile()
    Open ShapeFilename For Binary As sFnR   'open the shapefile
    Get sFnR, , ShapeFileHeader
    With ShapeFileHeader
        .FileCode = SwapBytes(.FileCode)
        .FileLength = SwapBytes(.FileLength)  'read the header info
        .u1 = SwapBytes(.u1)
        .u2 = SwapBytes(.u2)
        .u3 = SwapBytes(.u3)
        .u4 = SwapBytes(.u4)
        .u5 = SwapBytes(.u5)
    End With
    Select Case ShapeFileHeader.ShapeType
        Case FILETYPEENUM.typePoint
        Case FILETYPEENUM.typeMultiPoint
        Case FILETYPEENUM.typepolyline
        Case FILETYPEENUM.typepolygon
        Case FILETYPEENUM.typePointM:      Err.Raise TypeNotSupported, "CShape_IO::ShapefileOpen", "PointM not yet supported"
        Case FILETYPEENUM.typeMultiPointM: Err.Raise TypeNotSupported, "CShape_IO::ShapefileOpen", "MultiPointM not yet supported"
        Case FILETYPEENUM.typePolyLineM:   Err.Raise TypeNotSupported, "CShape_IO::ShapefileOpen", "PolylineM not yet supported"
        Case FILETYPEENUM.typePolygonM:    Err.Raise TypeNotSupported, "CShape_IO::ShapefileOpen", "PolygonM not yet supported"
        Case FILETYPEENUM.typePointZ:      Err.Raise TypeNotSupported, "CShape_IO::ShapefileOpen", "PointZ not yet supported"
        Case FILETYPEENUM.typeMultiPointZ: Err.Raise TypeNotSupported, "CShape_IO::ShapefileOpen", "MultiPointZ not yet supported"
        Case FILETYPEENUM.typePolyLineZ:   Err.Raise TypeNotSupported, "CShape_IO::ShapefileOpen", "PolylineZ not yet supported"
        Case FILETYPEENUM.typePolygonZ:    Err.Raise TypeNotSupported, "CShape_IO::ShapefileOpen", "PolygonZ not yet supported"
        Case FILETYPEENUM.typeMultiPatch:  Err.Raise TypeNotSupported, "CShape_IO::ShapefileOpen", "MultiPatch not yet supported"
    End Select
    If ShapeFileHeader.FileCode <> shpFileCode Then    'Problem with the file
        Err.Raise InvalidShapeFile, "CShape_IO::ShapefileOpen", _
                    "The specified shapefile is not valid"
        Exit Function
    End If
    ShapeIsOpen = True
    ShapeFileOpen = True
    Exit Function
    
ERR_ROUTINE:
    Err.Raise Err.Number, Err.Source, Err.Description
    
End Function

'********************************************************************************
'Property getShapeHeader
'Returns the header info from a shape file
'********************************************************************************
Friend Property Get getShapeHeader() As ShapeDefines.T_MainFileHeader
    On Error GoTo ERR_ROUTINE
    
    If ShapeIsOpen Then
        getShapeHeader = ShapeFileHeader    'return info if file is open
    Else
        Err.Raise filenotopen, "CShape_IO::getShapeHeader", "The ShapeFile is not Open"
    End If          'raise error if file is not open
    Exit Property
    
ERR_ROUTINE:
    Err.Raise Err.Number, Err.Source, Err.Description
End Property

'********************************************************************************
'Property getRecordCount
'Returns the number of records
'********************************************************************************
Public Property Get getRecordCount() As Long
    If IndexIsOpen Then  'can only get count if index is available
        getRecordCount = RecordCount
    Else
        Err.Raise filenotopen, "CShape_IO::getRecordCount", "Index file not open or invalid"
    End If
End Property

'********************************************************************************
' sub getRecordHeader
' reads the individual record header given the offset to the header
'********************************************************************************
Private Sub getRecordHeader(offset As Long)
    Get sFnR, offset, RecordHeader
    With RecordHeader
        .ContentLength = SwapBytes(.ContentLength)
        .RecordNumber = SwapBytes(.RecordNumber)
    End With
End Sub

'********************************************************************************
'Function FindOffset
' given a valid record number, returns the offset to the record
'offset is gotten from the index file
'********************************************************************************
Private Function FindOffset(ByVal RecordNumber As Long) As Boolean
    Dim ByteOffset As Long
    
    If RecordNumber = 1 Then
        ByteOffset = FILESIZES.MainHeaderSize + 1
    Else
        ByteOffset = FILESIZES.MainHeaderSize + ((RecordNumber - 1) * 8) + 1
    End If
    If ByteOffset > IndexFileHeader.FileLength * 2 - (FILESIZES.IndexRecSize - 1) Then
        FindOffset = False      'offset is greater then the length of the main file
    Else
        Get iFnR, ByteOffset, IndexRecordHeader
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
Private Function chkForErrBeforeRW(ByVal RecordNumber As Long, ByVal aSource As String, ByVal ShapeType) As Boolean
    On Error GoTo 0
    
    If ShapeIsOpen = False Then
        Err.Raise filenotopen, "CShape_IO::getXYPoint", _
                                "You have to open the shapefile before you can read it"
    End If
    If ShapeFileHeader.ShapeType <> ShapeType Then
        Err.Raise WrongShapeType, aSource, _
                                "The shapefile you are trying to read from is the wrong type"
    End If
    If RecordNumber > RecordCount Or RecordNumber < 0 Then
        Err.Raise NoSuchRecordERROR, aSource, "You passed an invalid record number"
    End If
End Function

'********************************************************************************
'Property getXYPoint
'Given a valid record number the point record is returned
'********************************************************************************
Friend Property Get getXYPoint(ByVal RecordNumber As Long) As ShapeDefines.T_shpXYPoint
   On Error GoTo ERR_ROUTINE
    
    chkForErrBeforeRW RecordNumber, "CShape_IO::getXYPoint", FILETYPEENUM.typePoint
    If FindOffset(RecordNumber) Then
        getRecordHeader (IndexRecordHeader.offset * 2 + 1)
        If RecordHeader.RecordNumber = RecordNumber Then
            Get sFnR, , XYPoint
        Else
            Err.Raise RecordsNotMatchERROR, "CShape_IO::getXYPoint", "Index Record does not match main file Record"
        End If
    Else
       Err.Raise OffsetPastEOF, "CShape_IO::getXYPoint", "The offset read from index file is larger then the mainfile size"
    End If
    getXYPoint = XYPoint
    Exit Property
    
ERR_ROUTINE:
    Err.Raise Err.Number, Err.Source, Err.Description
End Property

'*******************************************************************************************
'function CreateTmpShp
'called by one of the put functions if we have a valid record number

Private Function CreateTmpShp(iTfn As Long, sTfn As Long, ByVal ShapeType As Long) As Boolean
    Dim ShpTmpHeader As ShapeDefines.T_MainFileHeader
    Dim IndxTmpHeader As ShapeDefines.T_MainFileHeader
    
    On Error GoTo 0
    
    sTfn = FreeFile
    Open thePath + shptmp For Binary As sTfn
    iTfn = FreeFile
    Open thePath + indxtmp For Binary As iTfn
    ShpTmpHeader = ShapeFileHeader
    IndxTmpHeader = IndexFileHeader
    Put sTfn, , ShpTmpHeader
    Put iTfn, , IndxTmpHeader
    CreateTmpShp = True
    
End Function

'******************************************************************************************
'Sub putXYPoint
'called by the user to either write a new polygon Record or update an existing one
'RecordNumber = 0 = Append will append a record to the end of the file
'any valid RecordNumber between 1 and RecordCount will update the record with theRecord
'Temps are created and filled out with the correct records then the originals are
'deleted and the temps are renamed

Friend Sub putXYPoint(ByVal RecordNumber As Long, theRecord As ShapeDefines.T_shpXYPoint)
    Dim iTfn As Long
    Dim sTfn As Long
    Dim i As Long
    Dim IOrecord As ShapeDefines.T_shpXYPoint
    Dim shpRecHdr As ShapeDefines.T_RecordHeader
    Dim indxRecHdr As ShapeDefines.T_IndexRecordHeader
    Dim shpFileSize As Long
    Dim indxfilesize As Long
    Dim recsize As Long
    
    On Error GoTo ERR_ROUTINE
    
    If RDONLY = True Then
        Err.Raise FileIsReadOnly, "CShape_IO::putXYPoint", "The file is opened as readonly"
        Exit Sub
    End If
    chkForErrBeforeRW RecordNumber, "CShape_IO::UpdateXYPoint", FILETYPEENUM.typePoint
    If RecordNumber <> Append Then
        CreateTmpShp iTfn, sTfn, FILETYPEENUM.typePoint
        shpFileSize = FILESIZES.MainHeaderSize
        indxfilesize = FILESIZES.MainHeaderSize
        For i = 1 To RecordNumber - 1
            IOrecord = getXYPoint(i)
            recsize = Len(IOrecord)
            shpRecHdr.RecordNumber = SwapBytes(RecordHeader.RecordNumber)
            shpRecHdr.ContentLength = SwapBytes(recsize / 2)
            indxRecHdr.ContentLength = SwapBytes(recsize / 2)
            indxRecHdr.offset = SwapBytes((shpFileSize) / 2)
            shpFileSize = shpFileSize + recsize + FILESIZES.shpRecSize
            indxfilesize = indxfilesize + FILESIZES.IndexRecSize
            Put sTfn, , shpRecHdr
            Put sTfn, , IOrecord
            Put iTfn, , indxRecHdr
        Next
    
        recsize = Len(theRecord)
        shpRecHdr.ContentLength = SwapBytes(recsize / 2)
        shpRecHdr.RecordNumber = SwapBytes(RecordNumber)
        indxRecHdr.ContentLength = SwapBytes(recsize / 2)
        indxRecHdr.offset = SwapBytes(shpFileSize / 2)
        shpFileSize = shpFileSize + recsize + FILESIZES.shpRecSize
        indxfilesize = indxfilesize + FILESIZES.IndexRecSize
        Put sTfn, , shpRecHdr
        Put sTfn, , theRecord
        Put iTfn, , indxRecHdr
    
       For i = RecordNumber + 1 To RecordCount
           IOrecord = getXYPoint(i)
           recsize = Len(IOrecord)
           shpRecHdr.RecordNumber = SwapBytes(RecordHeader.RecordNumber)
           shpRecHdr.ContentLength = SwapBytes(recsize / 2)
           indxRecHdr.ContentLength = SwapBytes(recsize / 2)
           indxRecHdr.offset = SwapBytes(shpFileSize / 2)
           shpFileSize = shpFileSize + recsize + FILESIZES.shpRecSize
           indxfilesize = indxfilesize + FILESIZES.IndexRecSize
           Put sTfn, , shpRecHdr
           Put sTfn, , IOrecord
            Put iTfn, , indxRecHdr
        Next
    
        Put sTfn, HEADEROFFSETS.FileLength, SwapBytes(shpFileSize / 2)
        Put sTfn, FileCode, SwapBytes(shpFileCode)
        Put iTfn, HEADEROFFSETS.FileLength, SwapBytes(indxfilesize / 2)
        Put iTfn, FileCode, SwapBytes(shpFileCode)
        
        With theRecord.thePoint
          If .x < ShapeFileHeader.BndBoxXmin Then
               Put sTfn, xMin, .x
               Put iTfn, xMin, .x
           End If
           If .x > ShapeFileHeader.BndBoxXmax Then
             Put sTfn, xMax, .x
              Put iTfn, xMax, .x
            End If
            If .y < ShapeFileHeader.BndBoxYmin Then
                Put sTfn, yMin, .y
               Put iTfn, yMin, .y
           End If
            If .y > ShapeFileHeader.BndBoxYmax Then
                Put sTfn, yMax, .y
                Put iTfn, yMax, .y
            End If
        End With
      Close (sTfn)
      Close (iTfn)
      Close (sFnR)
      ShapeIsOpen = False
      Close (iFnR)
      IndexIsOpen = False
      Kill (ShapeFilename)
      Kill (IndexfileName)
      Name (thePath + shptmp) As ShapeFilename
      Name (thePath + indxtmp) As IndexfileName
        ShapeFileOpen ShapeFilename, READWRITEFLAG.Readwrite
    Else
        recsize = Len(theRecord)
        shpFileSize = LOF(sFnR)
        indxfilesize = LOF(iFnR)
        shpRecHdr.ContentLength = SwapBytes(recsize / 2)
        shpRecHdr.RecordNumber = SwapBytes(RecordCount + 1)
        indxRecHdr.ContentLength = SwapBytes(recsize / 2)
        indxRecHdr.offset = SwapBytes(shpFileSize / 2)
        Put sFnR, shpFileSize + 1, shpRecHdr
        Put sFnR, , theRecord
        Put iFnR, indxfilesize + 1, indxRecHdr
        shpFileSize = shpFileSize + recsize + FILESIZES.shpRecSize
        indxfilesize = indxfilesize + FILESIZES.IndexRecSize
        RecordCount = RecordCount + 1
        ShapeFileHeader.FileLength = shpFileSize / 2
        IndexFileHeader.FileLength = indxfilesize / 2
        Put sFnR, HEADEROFFSETS.FileLength, SwapBytes(shpFileSize / 2)
        Put iFnR, HEADEROFFSETS.FileLength, SwapBytes(indxfilesize / 2)
        With theRecord.thePoint
          If .x < ShapeFileHeader.BndBoxXmin Then
               Put sFnR, xMin, .x
               Put iFnR, xMin, .x
               ShapeFileHeader.BndBoxXmin = .x
               IndexFileHeader.BndBoxXmin = .x
           End If
           If .x > ShapeFileHeader.BndBoxXmax Then
                Put sFnR, xMax, .x
                Put iFnR, xMax, .x
                ShapeFileHeader.BndBoxXmax = .x
                IndexFileHeader.BndBoxXmax = .x
            End If
            If .y < ShapeFileHeader.BndBoxYmin Then
                Put sFnR, yMin, .y
                Put iFnR, yMin, .y
                ShapeFileHeader.BndBoxYmin = .y
                IndexFileHeader.BndBoxYmin = .y
           End If
            If .y > ShapeFileHeader.BndBoxYmax Then
                Put sFnR, yMax, .y
                Put iFnR, yMax, .y
                ShapeFileHeader.BndBoxYmax = .y
                IndexFileHeader.BndBoxYmax = .y
            End If
        End With
    End If
    Exit Sub
ERR_ROUTINE:
    Err.Raise Err.Number, Err.Source, Err.Description
End Sub
'********************************************************************************
'property getXYMultiPoint
'given a valid record number a points record is returned
'********************************************************************************
Friend Property Get getXYMultiPoint(ByVal RecordNumber As Long) As ShapeDefines.T_shpXYMultiPoint
    On Error GoTo ERR_ROUTINE
    
    chkForErrBeforeRW RecordNumber, "CShape_IO::getXYMultiPoint", FILETYPEENUM.typeMultiPoint
    
    If FindOffset(RecordNumber) Then
        getRecordHeader (IndexRecordHeader.offset * 2 + 1)
        With XYMultiPoint
            Get sFnR, , .ShapeType
            If .ShapeType = typeMultiPoint Then
              Get sFnR, , .Box
              Get sFnR, , .NumPoints
              ReDim .thePoints(0 To .NumPoints - 1)
              Get sFnR, , .thePoints
            End If
        End With
    Else
       Err.Raise OffsetPastEOF, "CShape_IO::getXYMultiPoint", "The offset read from index file is larger then the mainfile size"
    End If
    getXYMultiPoint = XYMultiPoint
    Exit Property
    
ERR_ROUTINE:
    Err.Raise Err.Number, Err.Source, Err.Description
End Property


'******************************************************************************************
'Sub putPXYMultiPoint
'called by the user to either write a new polygon Record or update an existing one
'RecordNumber = 0 = Append will append a record to the end of the file
'any valid RecordNumber between 1 and RecordCount will update the record with theRecord
'
'Temps are created and filled out with the correct records then the originals are
'deleted and the temps are renamed

Friend Sub putXYMultiPoint(ByVal RecordNumber As Long, theRecord As T_shpXYMultiPoint)
    Dim iTfn As Long
    Dim sTfn As Long
    Dim i As Long
    Dim IOrecord As ShapeDefines.T_shpXYMultiPoint
    Dim shpRecHdr As ShapeDefines.T_RecordHeader
    Dim indxRecHdr As ShapeDefines.T_IndexRecordHeader
    Dim shpFileSize As Long
    Dim indxfilesize As Long
    Dim recsize As Long
        
    On Error GoTo ERR_ROUTINE
    
    If RDONLY = True Then
        Err.Raise FileIsReadOnly, "CShape_IO::putXYMultiPoint", "The file is opened as readonly"
        Exit Sub
    End If
    chkForErrBeforeRW RecordNumber, "CShape_IO::UpdateXYMultiPoint", FILETYPEENUM.typeMultiPoint
    If RecordNumber <> Append Then
        CreateTmpShp iTfn, sTfn, FILETYPEENUM.typeMultiPoint
        shpFileSize = FILESIZES.MainHeaderSize
        indxfilesize = FILESIZES.MainHeaderSize
        For i = 1 To RecordNumber - 1
            IOrecord = getXYMultiPoint(i)
            recsize = Len(IOrecord.Box) + Len(IOrecord.NumPoints) + _
                Len(IOrecord.ShapeType) + (UBound(IOrecord.thePoints, 1) + 1) * 16
            shpRecHdr.RecordNumber = SwapBytes(RecordHeader.RecordNumber)
            shpRecHdr.ContentLength = SwapBytes(recsize / 2)
            indxRecHdr.ContentLength = SwapBytes(recsize / 2)
            indxRecHdr.offset = SwapBytes((shpFileSize) / 2)
            shpFileSize = shpFileSize + recsize + FILESIZES.shpRecSize
            indxfilesize = indxfilesize + FILESIZES.IndexRecSize
            Put sTfn, , shpRecHdr
            Put sTfn, , IOrecord.ShapeType
            Put sTfn, , IOrecord.Box
            Put sTfn, , IOrecord.NumPoints
            Put sTfn, , IOrecord.thePoints
            Put iTfn, , indxRecHdr
        Next
    
        recsize = Len(theRecord.Box) + Len(theRecord.NumPoints) + _
                Len(theRecord.ShapeType) + (UBound(theRecord.thePoints, 1) + 1) * 16
        shpRecHdr.ContentLength = SwapBytes(recsize / 2)
        shpRecHdr.RecordNumber = SwapBytes(RecordNumber)
        indxRecHdr.ContentLength = SwapBytes(recsize / 2)
        indxRecHdr.offset = SwapBytes(shpFileSize / 2)
        shpFileSize = shpFileSize + recsize + FILESIZES.shpRecSize
        indxfilesize = indxfilesize + FILESIZES.IndexRecSize
        Put sTfn, , shpRecHdr
        Put sTfn, , theRecord.ShapeType
        Put sTfn, , theRecord.Box
        Put sTfn, , theRecord.NumPoints
        Put sTfn, , theRecord.thePoints
        Put iTfn, , indxRecHdr
    
       For i = RecordNumber + 1 To RecordCount
           IOrecord = getXYMultiPoint(i)
           recsize = Len(IOrecord.Box) + Len(IOrecord.NumPoints) + _
                Len(IOrecord.ShapeType) + (UBound(IOrecord.thePoints, 1) + 1) * 16
           shpRecHdr.RecordNumber = SwapBytes(RecordHeader.RecordNumber)
           shpRecHdr.ContentLength = SwapBytes(recsize / 2)
           indxRecHdr.ContentLength = SwapBytes(recsize / 2)
           indxRecHdr.offset = SwapBytes(shpFileSize / 2)
           shpFileSize = shpFileSize + recsize + FILESIZES.shpRecSize
           indxfilesize = indxfilesize + FILESIZES.IndexRecSize
           Put sTfn, , shpRecHdr
           Put sTfn, , IOrecord.ShapeType
           Put sTfn, , IOrecord.Box
           Put sTfn, , IOrecord.NumPoints
           Put sTfn, , IOrecord.thePoints
           Put iTfn, , indxRecHdr
        Next
    
        Put sTfn, HEADEROFFSETS.FileLength, SwapBytes(shpFileSize / 2)
        Put sTfn, FileCode, SwapBytes(shpFileCode)
        Put iTfn, HEADEROFFSETS.FileLength, SwapBytes(indxfilesize / 2)
        Put iTfn, FileCode, SwapBytes(shpFileCode)
        UpdateBoundingBox theRecord.Box, sTfn, iTfn
        
        Close (sTfn)
        Close (iTfn)
        Close (sFnR)
        ShapeIsOpen = False
        Close (iFnR)
        IndexIsOpen = False
        Kill (ShapeFilename)
        Kill (IndexfileName)
        Name (thePath + shptmp) As ShapeFilename
        Name (thePath + indxtmp) As IndexfileName
        ShapeFileOpen ShapeFilename, READWRITEFLAG.Readwrite
    Else
        sTfn = sFnR
        iTfn = iFnR
        recsize = Len(theRecord.Box) + Len(theRecord.NumPoints) + _
                Len(theRecord.ShapeType) + (UBound(theRecord.thePoints, 1) + 1) * 16
        shpFileSize = LOF(sFnR)
        indxfilesize = LOF(iFnR)
        shpRecHdr.ContentLength = SwapBytes(recsize / 2)
        shpRecHdr.RecordNumber = SwapBytes(RecordCount + 1)
        indxRecHdr.ContentLength = SwapBytes(recsize / 2)
        indxRecHdr.offset = SwapBytes(shpFileSize / 2)
        Put sFnR, shpFileSize + 1, shpRecHdr
        Put sFnR, , theRecord.ShapeType
        Put sTfn, , theRecord.Box
        Put sTfn, , theRecord.NumPoints
        Put sTfn, , theRecord.thePoints
        Put iFnR, indxfilesize + 1, indxRecHdr
        shpFileSize = shpFileSize + recsize + FILESIZES.shpRecSize
        indxfilesize = indxfilesize + FILESIZES.IndexRecSize
        RecordCount = RecordCount + 1
        ShapeFileHeader.FileLength = shpFileSize / 2
        IndexFileHeader.FileLength = indxfilesize / 2
        Put sFnR, HEADEROFFSETS.FileLength, SwapBytes(shpFileSize / 2)
        Put iFnR, HEADEROFFSETS.FileLength, SwapBytes(indxfilesize / 2)
        UpdateBoundingBox theRecord.Box, sTfn, iTfn
    End If
    Exit Sub
ERR_ROUTINE:
    Err.Raise Err.Number, Err.Source, Err.Description
End Sub



'********************************************************************************
'property getPolyLine
'given a valid record number the polyline record is returned
'********************************************************************************
Friend Property Get getPolyLine(ByVal RecordNumber As Long) As ShapeDefines.T_shpPolyLine
    'On Error GoTo ERR_ROUTINE
    
    chkForErrBeforeRW RecordNumber, "CShape_IO::getPolyLine", FILETYPEENUM.typepolyline
    
    If FindOffset(RecordNumber) Then
        getRecordHeader (IndexRecordHeader.offset * 2 + 1)
        With PolyLine
            Get sFnR, , .ShapeType
            If .ShapeType = typepolyline Then
              Get sFnR, , .Box
              Get sFnR, , .NumParts
              Get sFnR, , .NumPoints
              ReDim .Parts(0 To .NumParts - 1)
              Get sFnR, , .Parts
              ReDim .thePoints(0 To .NumPoints - 1)
              Get sFnR, , .thePoints
            End If
        End With
    Else
       Err.Raise OffsetPastEOF, "CShape_IO::getPolyLine", "The offset read from index file is larger then the mainfile size"
    End If
    getPolyLine = PolyLine
    Exit Property
    
ERR_ROUTINE:
    Err.Raise Err.Number, Err.Source, Err.Description

End Property

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
Private Sub UpdateBoundingBox(Box As T_BoundingBox, shap&, indx&)
    With Box
        If .xMin < ShapeFileHeader.BndBoxXmin Then
            Put shap, xMin, .xMin
            Put indx, xMin, .xMin
            ShapeFileHeader.BndBoxXmin = .xMin
            IndexFileHeader.BndBoxXmin = .xMin
        End If
        If .xMax > ShapeFileHeader.BndBoxXmax Then
            Put shap, xMax, .xMax
            Put indx, xMax, .xMax
            ShapeFileHeader.BndBoxXmax = .xMax
            IndexFileHeader.BndBoxXmax = .xMax
        End If
        If .yMin < ShapeFileHeader.BndBoxYmin Then
            Put shap, yMin, .yMin
            Put indx, yMin, .yMin
            ShapeFileHeader.BndBoxYmin = .yMin
            IndexFileHeader.BndBoxYmin = .yMin
        End If
        If Box.yMax > ShapeFileHeader.BndBoxYmax Then
            Put shap, yMax, .yMax
            Put indx, yMax, .yMax
            ShapeFileHeader.BndBoxYmax = .yMax
            IndexFileHeader.BndBoxYmax = .yMax
        End If
    End With
End Sub

'******************************************************************************************
'Sub putPolyline
'called by the user to either write a new polygon Record or update an existing one
'RecordNumber = 0 = Append will append a record to the end of the file
'any valid RecordNumber between 1 and RecordCount will update the record with theRecord
'
'Temps are created and filled out with the correct records then the originals are
'deleted and the temps are renamed

Friend Sub putPolyLine(ByVal RecordNumber As Long, theRecord As T_shpPolyLine)
    Dim iTfn As Long
    Dim sTfn As Long
    Dim i As Long
    Dim IOrecord As ShapeDefines.T_shpPolyLine
    Dim shpRecHdr As ShapeDefines.T_RecordHeader
    Dim indxRecHdr As ShapeDefines.T_IndexRecordHeader
    Dim shpFileSize As Long
    Dim indxfilesize As Long
    Dim recsize As Long
        
    On Error GoTo ERR_ROUTINE
    
    If RDONLY = True Then
        Err.Raise FileIsReadOnly, "CShape_IO::putPolyline", "The file is opened as readonly"
        Exit Sub
    End If
    chkForErrBeforeRW RecordNumber, "CShape_IO::putPolyline", FILETYPEENUM.typepolyline
    If RecordNumber <> Append Then
        CreateTmpShp iTfn, sTfn, FILETYPEENUM.typepolyline
        shpFileSize = FILESIZES.MainHeaderSize
        indxfilesize = FILESIZES.MainHeaderSize
        For i = 1 To RecordNumber - 1
            IOrecord = getPolyLine(i)
            recsize = Len(IOrecord.Box) + Len(IOrecord.NumParts) + Len(IOrecord.NumPoints) + _
                (UBound(IOrecord.Parts, 1) + 1) * 4 + Len(IOrecord.ShapeType) + (UBound(IOrecord.thePoints, 1) + 1) * 16
            shpRecHdr.RecordNumber = SwapBytes(RecordHeader.RecordNumber)
            shpRecHdr.ContentLength = SwapBytes(recsize / 2)
            indxRecHdr.ContentLength = SwapBytes(recsize / 2)
            indxRecHdr.offset = SwapBytes((shpFileSize) / 2)
            shpFileSize = shpFileSize + recsize + FILESIZES.shpRecSize
            indxfilesize = indxfilesize + FILESIZES.IndexRecSize
            Put sTfn, , shpRecHdr
            Put sTfn, , IOrecord.ShapeType
            Put sTfn, , IOrecord.Box
            Put sTfn, , IOrecord.NumParts
            Put sTfn, , IOrecord.NumPoints
            Put sTfn, , IOrecord.Parts
            Put sTfn, , IOrecord.thePoints
            Put iTfn, , indxRecHdr
        Next
    
        recsize = Len(theRecord.Box) + Len(theRecord.NumParts) + Len(theRecord.NumPoints) + _
                (UBound(theRecord.Parts, 1) + 1) * 4 + Len(theRecord.ShapeType) + (UBound(theRecord.thePoints, 1) + 1) * 16
        shpRecHdr.ContentLength = SwapBytes(recsize / 2)
        shpRecHdr.RecordNumber = SwapBytes(RecordNumber)
        indxRecHdr.ContentLength = SwapBytes(recsize / 2)
        indxRecHdr.offset = SwapBytes(shpFileSize / 2)
        shpFileSize = shpFileSize + recsize + FILESIZES.shpRecSize
        indxfilesize = indxfilesize + FILESIZES.IndexRecSize
        Put sTfn, , shpRecHdr
        Put sTfn, , theRecord.ShapeType
        Put sTfn, , theRecord.Box
        Put sTfn, , theRecord.NumParts
        Put sTfn, , theRecord.NumPoints
        Put sTfn, , theRecord.Parts
        Put sTfn, , theRecord.thePoints
        Put iTfn, , indxRecHdr
    
       For i = RecordNumber + 1 To RecordCount
           IOrecord = getPolyLine(i)
           recsize = Len(IOrecord.Box) + Len(IOrecord.NumParts) + Len(IOrecord.NumPoints) + _
                (UBound(IOrecord.Parts, 1) + 1) * 4 + Len(IOrecord.ShapeType) + (UBound(IOrecord.thePoints, 1) + 1) * 16
           shpRecHdr.RecordNumber = SwapBytes(RecordHeader.RecordNumber)
           shpRecHdr.ContentLength = SwapBytes(recsize / 2)
           indxRecHdr.ContentLength = SwapBytes(recsize / 2)
           indxRecHdr.offset = SwapBytes(shpFileSize / 2)
           shpFileSize = shpFileSize + recsize + FILESIZES.shpRecSize
           indxfilesize = indxfilesize + FILESIZES.IndexRecSize
           Put sTfn, , shpRecHdr
           Put sTfn, , IOrecord.ShapeType
           Put sTfn, , IOrecord.Box
           Put sTfn, , IOrecord.NumParts
           Put sTfn, , IOrecord.NumPoints
           Put sTfn, , IOrecord.Parts
           Put sTfn, , IOrecord.thePoints
           Put iTfn, , indxRecHdr
        Next
    
        Put sTfn, HEADEROFFSETS.FileLength, SwapBytes(shpFileSize / 2)
        Put sTfn, FileCode, SwapBytes(shpFileCode)
        Put iTfn, HEADEROFFSETS.FileLength, SwapBytes(indxfilesize / 2)
        Put iTfn, FileCode, SwapBytes(shpFileCode)
        UpdateBoundingBox theRecord.Box, sTfn, iTfn
        
        Close (sTfn)
        Close (iTfn)
        Close (sFnR)
        ShapeIsOpen = False
        Close (iFnR)
        IndexIsOpen = False
        Kill (ShapeFilename)
        Kill (IndexfileName)
        Name (thePath + shptmp) As ShapeFilename
        Name (thePath + indxtmp) As IndexfileName
        ShapeFileOpen ShapeFilename, READWRITEFLAG.Readwrite
    Else
        sTfn = sFnR
        iTfn = iFnR
        recsize = Len(theRecord.Box) + Len(theRecord.NumParts) + Len(theRecord.NumPoints) + _
                (UBound(theRecord.Parts, 1) + 1) * 4 + Len(theRecord.ShapeType) + (UBound(theRecord.thePoints, 1) + 1) * 16
        shpFileSize = LOF(sFnR)
        indxfilesize = LOF(iFnR)
        shpRecHdr.ContentLength = SwapBytes(recsize / 2)
        shpRecHdr.RecordNumber = SwapBytes(RecordCount + 1)
        indxRecHdr.ContentLength = SwapBytes(recsize / 2)
        indxRecHdr.offset = SwapBytes(shpFileSize / 2)
        
        Put sTfn, shpFileSize + 1, shpRecHdr
        Put sTfn, , theRecord.ShapeType
        Put sTfn, , theRecord.Box
        Put sTfn, , theRecord.NumParts
        Put sTfn, , theRecord.NumPoints
        Put sTfn, , theRecord.Parts
        Put sTfn, , theRecord.thePoints
        Put iTfn, indxfilesize + 1, indxRecHdr
        
        shpFileSize = shpFileSize + recsize + FILESIZES.shpRecSize
        indxfilesize = indxfilesize + FILESIZES.IndexRecSize
        RecordCount = RecordCount + 1
        ShapeFileHeader.FileLength = shpFileSize / 2
        IndexFileHeader.FileLength = indxfilesize / 2
        Put sTfn, HEADEROFFSETS.FileLength, SwapBytes(shpFileSize / 2)
        Put iTfn, HEADEROFFSETS.FileLength, SwapBytes(indxfilesize / 2)
        UpdateBoundingBox theRecord.Box, sTfn, iTfn
    End If
    Exit Sub
ERR_ROUTINE:
    Err.Raise Err.Number, Err.Source, Err.Description
End Sub

'********************************************************************************
'Property getPolygon
'given a valid record number the Polygon record is returned

Friend Property Get getPolygon(ByVal RecordNumber As Long) As ShapeDefines.T_shpPolygon
    On Error GoTo ERR_ROUTINE
    
    chkForErrBeforeRW RecordNumber, "CShape_IO::getPolygon", FILETYPEENUM.typepolygon
    
    If FindOffset(RecordNumber) Then
        getRecordHeader (IndexRecordHeader.offset * 2 + 1)
        With Polygon
            Get sFnR, , .ShapeType
            Get sFnR, , .Box
            Get sFnR, , .NumParts
            Get sFnR, , .NumPoints
            ReDim .Parts(0 To .NumParts - 1)
            Get sFnR, , .Parts
            ReDim .thePoints(0 To .NumPoints - 1)
            Get sFnR, , .thePoints
        End With
    Else
       Err.Raise OffsetPastEOF, "CShape_IO::getPolygon", "The offset read from index file is larger then the mainfile size"
    End If
    getPolygon = Polygon
    Exit Property
    
ERR_ROUTINE:
    Err.Raise Err.Number, Err.Source, Err.Description

End Property

'******************************************************************************************
'Sub putPolygon
'called by the user to either write a new polygon Record or update an existing one
'RecordNumber = 0 = Append will append a record to the end of the file
'any valid RecordNumber between 1 and RecordCount will update the record with theRecord
'
'Temps are created and filled out with the correct records then the originals are
'deleted and the temps are renamed

Friend Sub putPolygon(ByVal RecordNumber As Long, theRecord As T_shpPolygon)
    Dim iTfn As Long
    Dim sTfn As Long
    Dim i As Long
    Dim IOrecord As ShapeDefines.T_shpPolygon
    Dim shpRecHdr As ShapeDefines.T_RecordHeader
    Dim indxRecHdr As ShapeDefines.T_IndexRecordHeader
    Dim shpFileSize As Long
    Dim indxfilesize As Long
    Dim recsize As Long
    Dim tmp As Long
    On Error GoTo ERR_ROUTINE
    
    If RDONLY = True Then
        Err.Raise FileIsReadOnly, "CShape_IO::putPolygon", "The file is opened as readonly"
        Exit Sub
    End If
    chkForErrBeforeRW RecordNumber, "CShape_IO::putPolygon", FILETYPEENUM.typepolygon
    If RecordNumber <> Append Then
        CreateTmpShp iTfn, sTfn, FILETYPEENUM.typepolygon
        shpFileSize = FILESIZES.MainHeaderSize
        indxfilesize = FILESIZES.MainHeaderSize
        For i = 1 To RecordNumber - 1
            IOrecord = getPolygon(i)
            recsize = Len(IOrecord.Box) + Len(IOrecord.NumParts) + Len(IOrecord.NumPoints) + _
                (UBound(IOrecord.Parts, 1) + 1) * 4 + Len(IOrecord.ShapeType) + (UBound(IOrecord.thePoints, 1) + 1) * 16
            shpRecHdr.RecordNumber = SwapBytes(RecordHeader.RecordNumber)
            shpRecHdr.ContentLength = SwapBytes(recsize / 2)
            indxRecHdr.ContentLength = SwapBytes(recsize / 2)
            indxRecHdr.offset = SwapBytes((shpFileSize) / 2)
            shpFileSize = shpFileSize + recsize + FILESIZES.shpRecSize
            indxfilesize = indxfilesize + FILESIZES.IndexRecSize
            Put sTfn, , shpRecHdr
            Put sTfn, , IOrecord.ShapeType
            Put sTfn, , IOrecord.Box
            Put sTfn, , IOrecord.NumParts
            Put sTfn, , IOrecord.NumPoints
            Put sTfn, , IOrecord.Parts
            Put sTfn, , IOrecord.thePoints
            Put iTfn, , indxRecHdr
        Next
    
        recsize = Len(theRecord.Box) + Len(theRecord.NumParts) + Len(theRecord.NumPoints) + _
                (UBound(theRecord.Parts, 1) + 1) * 4 + Len(theRecord.ShapeType) + (UBound(theRecord.thePoints, 1) + 1) * 16
        shpRecHdr.ContentLength = SwapBytes(recsize / 2)
        shpRecHdr.RecordNumber = SwapBytes(RecordNumber)
        indxRecHdr.ContentLength = SwapBytes(recsize / 2)
        indxRecHdr.offset = SwapBytes(shpFileSize / 2)
        shpFileSize = shpFileSize + recsize + FILESIZES.shpRecSize
        indxfilesize = indxfilesize + FILESIZES.IndexRecSize
        Put sTfn, , shpRecHdr
        Put sTfn, , theRecord.ShapeType
        Put sTfn, , theRecord.Box
        Put sTfn, , theRecord.NumParts
        Put sTfn, , theRecord.NumPoints
        Put sTfn, , theRecord.Parts
        Put sTfn, , theRecord.thePoints
        Put iTfn, , indxRecHdr
    
       For i = RecordNumber + 1 To RecordCount
           IOrecord = getPolygon(i)
           recsize = Len(IOrecord.Box) + Len(IOrecord.NumParts) + Len(IOrecord.NumPoints) + _
                (UBound(IOrecord.Parts, 1) + 1) * 4 + Len(IOrecord.ShapeType) + (UBound(IOrecord.thePoints, 1) + 1) * 16
           shpRecHdr.RecordNumber = SwapBytes(RecordHeader.RecordNumber)
           shpRecHdr.ContentLength = SwapBytes(recsize / 2)
           indxRecHdr.ContentLength = SwapBytes(recsize / 2)
           indxRecHdr.offset = SwapBytes(shpFileSize / 2)
           shpFileSize = shpFileSize + recsize + FILESIZES.shpRecSize
           indxfilesize = indxfilesize + FILESIZES.IndexRecSize
           Put sTfn, , shpRecHdr
           Put sTfn, , IOrecord.ShapeType
           Put sTfn, , IOrecord.Box
           Put sTfn, , IOrecord.NumParts
           Put sTfn, , IOrecord.NumPoints
           Put sTfn, , IOrecord.Parts
           Put sTfn, , IOrecord.thePoints
           Put iTfn, , indxRecHdr
        Next
    
        Put sTfn, HEADEROFFSETS.FileLength, SwapBytes(shpFileSize / 2)
        Put sTfn, FileCode, SwapBytes(shpFileCode)
        Put iTfn, HEADEROFFSETS.FileLength, SwapBytes(indxfilesize / 2)
        Put iTfn, FileCode, SwapBytes(shpFileCode)
        UpdateBoundingBox theRecord.Box, sTfn, iTfn
        
        Close (sTfn)
        Close (iTfn)
        Close (sFnR)
        ShapeIsOpen = False
        Close (iFnR)
        IndexIsOpen = False
        Kill (ShapeFilename)
        Kill (IndexfileName)
        Name (thePath + shptmp) As ShapeFilename
        Name (thePath + indxtmp) As IndexfileName
        ShapeFileOpen ShapeFilename, READWRITEFLAG.Readwrite
    Else
        sTfn = sFnR
        iTfn = iFnR
        recsize = Len(theRecord.Box) + Len(theRecord.NumParts) + Len(theRecord.NumPoints) + _
                (UBound(theRecord.Parts, 1) + 1) * 4 + Len(theRecord.ShapeType) + (UBound(theRecord.thePoints, 1) + 1) * 16
        shpFileSize = LOF(sFnR)
        indxfilesize = LOF(iFnR)
        shpRecHdr.ContentLength = SwapBytes(recsize / 2)
        shpRecHdr.RecordNumber = SwapBytes(RecordCount + 1)
        indxRecHdr.ContentLength = SwapBytes(recsize / 2)
        indxRecHdr.offset = SwapBytes(shpFileSize / 2)
        Put sTfn, shpFileSize + 1, shpRecHdr
        Put sTfn, , theRecord.ShapeType
        Put sTfn, , theRecord.Box
        Put sTfn, , theRecord.NumParts
        Put sTfn, , theRecord.NumPoints
        Put sTfn, , theRecord.Parts
        Put sTfn, , theRecord.thePoints
        Put iTfn, indxfilesize + 1, indxRecHdr
        shpFileSize = shpFileSize + recsize + FILESIZES.shpRecSize
        indxfilesize = indxfilesize + FILESIZES.IndexRecSize
        RecordCount = RecordCount + 1
        ShapeFileHeader.FileLength = shpFileSize / 2
        IndexFileHeader.FileLength = indxfilesize / 2
        Put sFnR, HEADEROFFSETS.FileLength, SwapBytes(shpFileSize / 2)
        Put iFnR, HEADEROFFSETS.FileLength, SwapBytes(indxfilesize / 2)
        UpdateBoundingBox theRecord.Box, sTfn, iTfn
    End If
    Exit Sub
ERR_ROUTINE:
    Err.Raise Err.Number, Err.Source, Err.Description
End Sub

'********************************************************************************
'function isFileNameValid
' Checks for a valid shapefile name setting the global shapefilename
' checks for the index file and sets the global indexfilename if the index is present
'********************************************************************************
Function isFileNameValid(Filename As String) As Boolean
    Dim count As Long
    Dim place As Long
    Dim dot As Long
    Dim i As Long
    Dim newFN As String
    Dim dotfound As Boolean
    Dim ext As Long
    Dim extension As String
    Dim PathAndName As String
    Dim NameNoPath As String
    
    dotfound = False
    For i = 1 To Len(Filename)
        If Mid(Filename, i, 1) = "\" Then place = i
    Next
    For i = (Len(Filename) - place) To Len(Filename)
        If Mid(Filename, i, 1) = "." Then
            dot = i
            dotfound = True
        End If
    Next
    If dotfound Then
        ext = Len(Filename) - dot
        extension = Right(Filename, 3)
        PathAndName = Left(Filename, Len(Filename) - ext - 1)
        NameNoPath = Right(PathAndName, Len(PathAndName) - place)
        If extension = "shp" Or extension = "SHP" Then
            ShapeFilename = Filename
            If Dir(PathAndName + ".shx") = (NameNoPath + ".shx") Then
                IndexfileName = PathAndName + ".shx"
            Else
                IndexfileName = PathAndName + ".shx"
                Call CreateAnIndex
            End If
            isFileNameValid = True
            thePath = Left(Filename, place)
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

Private Function FixExtensions(ByVal Filename As String) As Boolean
    Dim count As Long
    Dim place As Long
    Dim dot As Long
    Dim i As Long
    Dim newFN As String
    Dim dotfound As Boolean
    Dim ext As Long
    Dim PathAndName As String
    
    dotfound = False
    For i = 1 To Len(Filename)
        If Mid(Filename, i, 1) = "\" Then place = i
    Next
    For i = (Len(Filename) - place) To Len(Filename)
        If Mid(Filename, i, 1) = "." Then
            dot = i
            dotfound = True
        End If
    Next
    If dotfound Then
        ext = Len(Filename) - dot
        PathAndName = Left(Filename, Len(Filename) - ext - 1)
        ShapeFilename = PathAndName + ".shp"
        IndexfileName = PathAndName + ".shx"
    Else
        ShapeFilename = Filename + ".shp"
        IndexfileName = Filename + ".shx"
    End If
   FixExtensions = True

End Function

'********************************************************************************
'Function CreateNewShape
'When called it will create a new .shp and .shx files and write the header
'Returns true on success
Public Function CreateNewShape(ByVal Filename As String, ShapeType As Long) As Boolean
    Dim result As Boolean
    On Error GoTo ERR_ROUTINE
    
    If ShapeIsOpen = True Then
        Err.Raise FileAlreadyOpen, "CShape_IO::CreateNewShape", _
                                   "A Shapefile is already open for this instance"
    End If
    
    result = FixExtensions(Filename)
    sFnR = FreeFile
    Open ShapeFilename For Binary As sFnR
    ShapeIsOpen = True
    iFnR = FreeFile
    Open IndexfileName For Binary As iFnR
    IndexIsOpen = True
    With ShapeFileHeader
        .FileCode = SwapBytes(shpFileCode)
        .version = 1000
        .ShapeType = ShapeType
        .BndBoxXmin = 1E+300
        .BndBoxYmin = 1E+300
        .BndBoxZmin = 1E+300
        .BndBoxMmin = 1E+300
        .BndBoxXmax = -1E+300
        .BndBoxYmax = -1E+300
        .BndBoxZmax = -1E+300
        .BndBoxMmax = -1E+300
    End With
    With IndexFileHeader
        .FileCode = SwapBytes(shpFileCode)
        .version = 1000
        .ShapeType = ShapeType
    End With
    Put sFnR, , ShapeFileHeader
    Put iFnR, , IndexFileHeader
    CreateNewShape = True
    RecordCount = 0
    Exit Function
    
ERR_ROUTINE:
  Err.Raise Err.Number, Err.Source, Err.Description
End Function

'*******************************************************************************************
'function FileShutDown
'Must be called by the user to close the open shp ans shx files.
'Returns true on success

Public Function FileShutDown() As Boolean
    On Error GoTo 0
    If ShapeIsOpen Then
        Close (sFnR)
        ShapeIsOpen = False
    End If
    If IndexIsOpen Then
        Close (iFnR)
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
    Dim fn1 As Long
    Dim fn2 As Long
    Dim hdrShape As T_MainFileHeader
    Dim hdrIdx As T_MainFileHeader
    Dim hdrShpRec As T_RecordHeader
    Dim hdrIdxRec As T_IndexRecordHeader
    Dim idxFileSize As Long
    Dim shpFileSize As Long
    Dim offset As Long
    Dim nextoffset As Long
    Dim tmp As Long
    Dim tmp2 As Long
    
    On Error GoTo 0
    
    fn1 = FreeFile
    Open ShapeFilename For Binary As fn1
    fn2 = FreeFile
    Open IndexfileName For Binary As fn2
    
    idxFileSize = FILESIZES.MainHeaderSize
    shpFileSize = FILESIZES.MainHeaderSize
    Get fn1, , hdrShape
    Put fn2, , hdrShape
    nextoffset = shpFileSize + 1
    Do While Not EOF(fn1)
        Get fn1, nextoffset, hdrShpRec
        If Not EOF(fn1) Then
            nextoffset = nextoffset + SwapBytes(hdrShpRec.ContentLength) * 2 + FILESIZES.shpRecSize
            hdrIdxRec.ContentLength = hdrShpRec.ContentLength
            hdrIdxRec.offset = SwapBytes(shpFileSize / 2)
            Put fn2, , hdrIdxRec
            idxFileSize = idxFileSize + FILESIZES.IndexRecSize
            shpFileSize = shpFileSize + SwapBytes(hdrShpRec.ContentLength) * 2 + FILESIZES.shpRecSize
        End If
    Loop
    Put fn2, HEADEROFFSETS.FileLength, SwapBytes(LOF(fn2) / 2)
    Close (fn1)
    Close (fn2)
End Sub

'********************************************************************************
' Class_Initialize
' sets the FileIsOpen variables to False on Startup
Private Sub Class_Initialize()
    ShapeIsOpen = False
    IndexIsOpen = False
End Sub
