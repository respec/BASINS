VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsTar"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
Option Explicit

Private pFilename As String
Private pFileHandle As Integer
Private pArchiveFilenames As FastCollection
Private pArchiveFileHeaderRecords As FastCollection
Private pArchiveFileHeaders() As HeaderType
Private pArchiveFileCount As Long
Private pErrorDescription As String

Private pGzipPath As String
Private pGzipOptions As String
Private pRealFilename As String
Private pModifiedFile As Boolean

Private Const RECORDSIZE = 512
Private Const NAMSIZ = 100
Private Const TUNMLEN = 32
Private Const TGNMLEN = 32
Private Const SPARSE_IN_HDR = 4
Private Const EPOCH = #1/1/1970#

' The checksum field is filled with this while the checksum is computed.
Private Const CHKBLANKS = "        "            ' 8 blanks, no null

' The magic field is filled with this if uname and gname are valid.
Private Const TMAGIC = "ustar  "            ' 7 chars and a null

' The magic field is filled with this if this is a GNU format dump entry
Private Const GNUMAGIC = "GNUtar "          ' 7 chars and a null

' The linkflag defines the type of file
Private Const LF_OLDNORMAL = vbNullString ' Normal disk file, Unix compatible
Private Const LF_NORMAL = "0"          ' Normal disk file
Private Const LF_LINK = "1"            ' Link to previously dumped file
Private Const LF_SYMLINK = "2"         ' Symbolic link
Private Const LF_CHR = "3"             ' Character special file
Private Const LF_BLK = "4"             ' Block special file
Private Const LF_DIR = "5"             ' Directory
Private Const LF_FIFO = "6"            ' FIFO special file
Private Const LF_CONTIG = "7"          ' Contiguous file

' Further link types may be defined later.

' Bits used in the mode field - values in octal
Private Const TSUID = 4000           ' Set UID on execution
Private Const TSGID = 2000           ' Set GID on execution
Private Const TSVTX = 1000           ' Save text (sticky bit)

' File permissions
Private Const TUREAD = 400           ' read by owner
Private Const TUWRITE = 200          ' write by owner
Private Const TUEXEC = 100           ' execute/search by owner
Private Const TGREAD = 40            ' read by group
Private Const TGWRITE = 20           ' write by group
Private Const TGEXEC = 10            ' execute/search by group
Private Const TOREAD = 4             ' read by other
Private Const TOWRITE = 2            ' write by other
Private Const TOEXEC = 1             ' execute/search by other

Private Type RecordType
  b(1 To RECORDSIZE) As Byte
End Type

Dim ZeroBytes As RecordType

Private Type StringRecordType
  s As String * RECORDSIZE
End Type

Private Type sparse
  offset As String * 12
  numbytes As String * 12
End Type

Private Type HeaderType
  Name As String * NAMSIZ     '  1-100
  mode As String * 8          '101-108
  uid As String * 8           '109-116
  gid As String * 8           '116-124
  size As String * 12         '125-136
  mtime As String * 12        '137-148
  chksum As String * 8        '149-156
  linkflag As String * 1      '157
  linkname As String * NAMSIZ '158-257
  magic As String * 8         '258-265
  UName As String * TUNMLEN   '266-297
  gname As String * TGNMLEN   '298-329
  devmajor As String * 8      '330-337
  devminor As String * 8      '338-345
  'Additional fields used by GNU tar but not used by us
  atime As String * 12        '346-357
  ctime As String * 12        '358-370
  pad(142) As Byte            '371-512
'  offset As String * 12
'  longnames As String * 4
'  pad As String * 1
'  sp(SPARSE_IN_HDR) As sparse
'  isextended As String * 1
'  realsize As String * 12      ' true size of the sparse file
'  ending_blanks As String * 12 ' number of nulls at the end of the file
End Type

Public Property Get TarFilename() As String
  TarFilename = pRealFilename
End Property
Public Property Let TarFilename(newValue As String)
  Dim TempIndex As Long
  If pRealFilename <> newValue Then
    pRealFilename = newValue
    Set pArchiveFilenames = Nothing
    Set pArchiveFilenames = New FastCollection
    Set pArchiveFileHeaderRecords = Nothing
    Set pArchiveFileHeaderRecords = New FastCollection
    ReDim pArchiveFileHeaders(10)
    pArchiveFileCount = 0
    If LCase(Right(newValue, 2)) = "gz" Then
      Do
        TempIndex = TempIndex + 1
        pFilename = GetTmpPath & "TarTemp" & TempIndex & ".tar"
        If FileExists(pFilename) Then 'Remove temporary files > 2 days old or empty
          If FileLen(pFilename) = 0 Or Now - FileDateTime(pFilename) > 2 Then
            Kill pFilename
            If FileExists(pFilename & ".gz") Then Kill pFilename & ".gz"
          End If
        End If
      Loop While FileExists(pFilename) Or FileExists(pFilename & ".gz")
      If FileExists(pRealFilename) Then
        If Not HasCorrectGzipMagic(pRealFilename) Then
          Debug.Print "Bad gzip magic, not decompressing: " & pRealFilename
        Else
          Debug.Print "Uncompressing to temporary file: " & pFilename
          FileCopy pRealFilename, pFilename & ".gz"
          Shell GzipPath & " -q -d """ & pFilename & ".gz""" '-q = quiet, -d = decompress
          While FileExists(pFilename & ".gz")
            DoEvents
            Sleep 50
          Wend
        End If
      End If
    Else
      pFilename = pRealFilename
    End If
  End If
End Property

Private Function HasCorrectGzipMagic(ByRef filename As String) As Boolean
' ##SUMMARY Returns True if named file has correct gzip magic header
' ##PARAM FileName I Name of text file
' ##RETURNS Returns True if named file has correct gzip magic header
  Dim InFile As Integer, FileLength&
  Dim magic(0 To 1) As Byte
  ' ##LOCAL InFile - long filenumber of text file
  ' ##LOCAL retval() - byte array containing return values
  
  On Error GoTo ErrorReading
  
  If FileExists(pRealFilename) Then
    InFile = FreeFile(0)
    Open filename For Binary As InFile
    Get #InFile, , magic
    Close InFile
  
    If magic(0) = &H1F And magic(1) = &H8B Then HasCorrectGzipMagic = True
  End If
  
  Exit Function

ErrorReading:
  Debug.Print "HasCorrectGzipMagic: Error reading '" & filename & "': " & Err.Description
End Function

' "-6" is the default, "-9" will compress more and be slower, "-1" will compress less and be faster
Public Property Get GzipOptions() As String
  GzipOptions = pGzipOptions
End Property
Public Property Let GzipOptions(newValue As String)
  pGzipOptions = newValue
End Property

Public Property Get GzipPath() As String
  Dim ff As ATCoFindFile
  If Len(pGzipPath) = 0 Then
    Set ff = New ATCoFindFile
    ff.SetDialogProperties "Please locate gzip.exe", CurDir & "\gzip.exe"
    ff.SetRegistryInfo "gzip", "files", "gzip.exe"
    pGzipPath = """" & ff.GetName & """"
  End If
  GzipPath = pGzipPath
End Property
Public Property Let GzipPath(newValue As String)
  pGzipPath = newValue
  If FileExists(pGzipPath) Then
    If LCase(GetSetting("gzip", "files", "gzip.exe")) <> LCase(pGzipPath) Then
      SaveSetting "gzip", "files", "gzip.exe", pGzipPath
    End If
  End If
End Property

Public Function ErrorDescription() As String
  ErrorDescription = pErrorDescription
  pErrorDescription = ""
End Function

Private Function OctalStringToLong(octal As String) As Long
  Dim i&, retval&, octDigit As String, lenOctal As Long
  lenOctal = Len(octal)
  retval = 0
  For i = 1 To lenOctal
    octDigit = Mid(octal, i, 1)
    If IsNumeric(octDigit) Then retval = retval * 8 + CLng(octDigit)
  Next i
  OctalStringToLong = retval
End Function

Private Function TrimNulls(s As String) As String
  Dim zeroPos&
  zeroPos = InStr(s, Chr(0))
  If zeroPos = 0 Then
    TrimNulls = s
  Else
    TrimNulls = Left(s, zeroPos - 1)
  End If
End Function

Private Function LongToOctalString(Val As Long, digits As Long) As String
  Dim retval As String
  Dim remainder As Long
  Dim digit As Long, digitVal As Long, power As Long
  retval = ""
  remainder = Val
  For digit = digits - 1 To 0 Step -1
    power = 8 ^ digit
    digitVal = Int(remainder / power)
    retval = retval & CStr(digitVal)
    remainder = remainder - digitVal * power
  Next
  LongToOctalString = retval
End Function

Private Function ComputeChecksum(header As HeaderType) As String
  Dim Byt As RecordType
  Dim sum As Long
  Dim Index As Long
  sum = 0
  LSet Byt = header
  For Index = 1 To 512
    sum = sum + Byt.b(Index)
  Next
  ComputeChecksum = sum
End Function

Private Function OpenArchive() As Boolean
  On Error GoTo FailedToOpen
  pFileHandle = FreeFile
  Open pFilename For Random As pFileHandle Len = RECORDSIZE
  OpenArchive = True
FailedToOpen:
End Function

'Returns 0 if named file is in archive
'Returns record number for starting to write file if it is not already in archive
Private Function FindRecordForFile(filename As String) As Long
  Dim maxRecord As Long
  Dim curHeader As HeaderType
  Dim curRecord As Long
  Dim nFileBytes As Long
  Dim curBlocks As Long
  Dim curName As String
  curName = "foo"
  curRecord = 1
  maxRecord = LOF(pFileHandle) / RECORDSIZE
  While curRecord < maxRecord And curName <> ""
    Get pFileHandle, curRecord, curHeader
    curName = TrimNulls(curHeader.Name)
    curBlocks = 0
    If curName <> "" Then
      If curName = filename Then
        FindRecordForFile = 0
        Exit Function
      End If
      nFileBytes = OctalStringToLong(curHeader.size)
      curBlocks = nFileBytes / RECORDSIZE
      If curBlocks * RECORDSIZE < nFileBytes Then curBlocks = curBlocks + 1
      'Debug.Print curBlocks, nFileBytes, curHeader.name
    End If
    curRecord = curRecord + curBlocks + 1 'Move to header after this file
  Wend
  FindRecordForFile = curRecord
End Function

Private Sub WriteHeader(filename As String, _
                        nFileBytes As Long, _
                        HeaderRecordNumber As Long, _
                        mtimeGMT As Date, _
               Optional atimeGMT As Date = 0, _
               Optional ctimeGMT As Date = 0)
  Dim curHeader As HeaderType
  
  'Debug.Print "WriteHeader for " & Filename
  
  If atimeGMT = 0 Then atimeGMT = mtimeGMT
  If ctimeGMT = 0 Then ctimeGMT = mtimeGMT
  
  LSet curHeader = ZeroBytes
  With curHeader
    .Name = filename & String(NAMSIZ - Len(filename), Chr(0))
    .mode = "0100755"
    .uid = "0000000"
    .gid = "0000000"
    .size = LongToOctalString(nFileBytes, 11)
    .mtime = LongToOctalString(DateDiff("s", EPOCH, mtimeGMT), 11)
    .atime = LongToOctalString(DateDiff("s", EPOCH, atimeGMT), 11)
    .ctime = LongToOctalString(DateDiff("s", EPOCH, ctimeGMT), 11)
    .chksum = "        "
    .linkflag = "0"
    .magic = "       "
    .chksum = LongToOctalString(ComputeChecksum(curHeader), 6)
  End With
  Put pFileHandle, HeaderRecordNumber, curHeader
  RememberFileInArchive HeaderRecordNumber, curHeader
  pModifiedFile = True
End Sub

'curFileFullPath = full path and file name to read from disk
'Filename        = path and file name to use for this file in archive
'Returns True if successful, Returns false and sets ErrorDescription if unsuccessful
Public Function AppendFileFromDisk(curFileFullPath As String, filename As String) As Boolean
  Dim mtimeGMT As Date, atimeGMT As Date, ctimeGMT As Date
  Dim curRecord As Long, ThisFileHeaderRecord As Long
  Dim curName As String
  Dim nFileBytes As Long, bytesWritten As Long
  Dim SaveFilename As String, writeBlock As RecordType
  Dim InFile As Integer
  Dim clearbyte As Long
  
  On Error GoTo ErrHand
  
  GetFileTimesGMT curFileFullPath, mtimeGMT, atimeGMT, ctimeGMT
  
  SaveFilename = ReplaceString(filename, "\", "/")
  OpenArchive
  
  'Check to make sure a file by this name is not already in the archive
  curRecord = FindRecordForFile(SaveFilename)
  If curRecord = 0 Then
    pErrorDescription = "AppendFileFromDisk: " & filename & " already exists in this tar file, could not append"
    AppendFileFromDisk = False
  Else
    InFile = FreeFile
    Open curFileFullPath For Random As InFile Len = RECORDSIZE
    nFileBytes = LOF(InFile)
    ThisFileHeaderRecord = curRecord
    WriteHeader SaveFilename, nFileBytes, curRecord, mtimeGMT, atimeGMT, ctimeGMT
    curRecord = curRecord + 1
    bytesWritten = 0
    While bytesWritten < nFileBytes
      Get InFile, , writeBlock
      bytesWritten = bytesWritten + RECORDSIZE
      If bytesWritten > nFileBytes Then
        For clearbyte = RECORDSIZE - (bytesWritten - nFileBytes) + 1 To RECORDSIZE
          writeBlock.b(clearbyte) = 0
        Next
      End If
      Put pFileHandle, , writeBlock
      curRecord = curRecord + 1
    Wend
    Close InFile
    AppendFileFromDisk = True
  End If
  Close pFileHandle
  Exit Function
ErrHand:
  pErrorDescription = "AppendFileFromDisk: " & Err.Description
  AppendFileFromDisk = False
End Function

'Filename        = path and file name to use for this file in archive
'Returns True if successful, Returns false and sets ErrorDescription if unsuccessful
Public Function AppendFileFromString(filename As String, _
                                 FileContents As String, _
                                     mtimeGMT As Date, _
                            Optional atimeGMT As Date = 0, _
                            Optional ctimeGMT As Date = 0) As Boolean
  Dim curRecord As Long, ThisFileHeaderRecord As Long
  Dim curName As String
  Dim nFileBytes As Long, bytesWritten As Long
  Dim SaveFilename As String, writeBlock As StringRecordType
  
  SaveFilename = ReplaceString(filename, "\", "/")
  OpenArchive
  
  'Check to make sure a file by this name is not already in the archive
  curRecord = FindRecordForFile(SaveFilename)
  If curRecord = 0 Then
    pErrorDescription = "AppendFileFromString: " & filename & " already exists in this tar file, could not append"
    AppendFileFromString = False
  Else
    nFileBytes = Len(FileContents)
    
    WriteHeader SaveFilename, nFileBytes, curRecord, mtimeGMT, atimeGMT, ctimeGMT

    curRecord = curRecord + 1
    bytesWritten = 0
    While bytesWritten < nFileBytes
      writeBlock.s = Mid(FileContents, bytesWritten + 1, RECORDSIZE)
      bytesWritten = bytesWritten + RECORDSIZE
      If bytesWritten > nFileBytes Then
        Mid(writeBlock.s, RECORDSIZE - (bytesWritten - nFileBytes) + 1, bytesWritten - nFileBytes) = String(bytesWritten - nFileBytes, Chr(0))
      End If
      Put pFileHandle, curRecord, writeBlock
      curRecord = curRecord + 1
    Wend
    AppendFileFromString = True
  End If
  Close pFileHandle
  Exit Function
ErrHand:
  pErrorDescription = "AppendFileFromString: " & Err.Description
  AppendFileFromString = False
End Function

Public Property Get ArchiveFilenames() As FastCollection
  ScanArchive
  Set ArchiveFilenames = pArchiveFilenames
End Property

Public Property Get FileDate(Index As Long) As Date
  ScanArchive
  If Index > 0 And Index <= pArchiveFileCount Then
    FileDate = UTCDateToDate(DateAdd("s", OctalStringToLong(pArchiveFileHeaders(Index).mtime), EPOCH))
  End If
End Property

Public Property Get FileSize(Index As Long) As Long
  ScanArchive
  If Index > 0 And Index <= pArchiveFileCount Then
    FileSize = OctalStringToLong(pArchiveFileHeaders(Index).size)
  End If
End Property

Private Sub ScanArchive()
  Dim curName As String
  Dim curBlocks As Long
  Dim curRecord As Long
  Dim maxRecord As Long
  Dim curHeader As HeaderType
  Dim nFileBytes As Long
  
  If pArchiveFilenames.Count = 0 Then
    If OpenArchive Then
      curRecord = 1
      maxRecord = LOF(pFileHandle) / RECORDSIZE
      While curRecord < maxRecord
        Get pFileHandle, curRecord, curHeader
        curName = TrimNulls(curHeader.Name)
        curBlocks = 0
        If curName <> "" Then
          RememberFileInArchive curRecord, curHeader
          
          nFileBytes = OctalStringToLong(curHeader.size)
          curBlocks = nFileBytes / RECORDSIZE
          If curBlocks * RECORDSIZE < nFileBytes Then curBlocks = curBlocks + 1
        End If
        curRecord = curRecord + curBlocks + 1 'Move to header after this file
      Wend
      Close pFileHandle
    End If
  End If
End Sub

Private Sub RememberFileInArchive(HeaderRecordNumber As Long, curHeader As HeaderType)
  Dim curName As String
  curName = TrimNulls(curHeader.Name)
  pArchiveFilenames.Add curName, LCase(curName)
  pArchiveFileHeaderRecords.Add HeaderRecordNumber
  pArchiveFileCount = pArchiveFileCount + 1
  If pArchiveFileCount > UBound(pArchiveFileHeaders) Then ReDim Preserve pArchiveFileHeaders(HeaderRecordNumber * 2)
  pArchiveFileHeaders(pArchiveFileCount) = curHeader
End Sub
          
'Public Function ArchiveFilename(index As Long) As String
'  Dim retval As Collection
'  Dim curRecord As Long, maxRecord As Long
'  Dim curHeader As HeaderType, curName As String
'  Dim nFileBytes As Long, curBlocks As Long
'  Dim curHeaderNum As Long
'
'  curHeaderNum = 0
'  curRecord = 1
'  Set retval = New Collection
'
'  OpenArchive
'  maxRecord = LOF(pFileHandle) / RECORDSIZE
'
'  While curRecord < maxRecord And curHeaderNum < index
'    Get pFileHandle, curRecord, curHeader
'    curName = TrimNulls(curHeader.Name)
'    curBlocks = 0
'    If curName <> "" Then
'      curHeaderNum = curHeaderNum + 1
'      nFileBytes = OctalStringToLong(curHeader.size)
'      curBlocks = nFileBytes / RECORDSIZE
'      If curBlocks * RECORDSIZE < nFileBytes Then curBlocks = curBlocks + 1
'    End If
'    curRecord = curRecord + curBlocks + 1 'Move to header after this file
'  Wend
'  Close pFileHandle
'
'  If curHeaderNum = index Then
'    ArchiveFilename = curName
'  Else
'    ArchiveFilename = ""
'  End If
'End Function


'AllNone values "all" or "none" skip questions about overwriting existing files
'If blank, the user will be asked about whether to overwrite existing files
'If the user gets asked about overwriting this file, AllNone will be changed to
'the user's answer: "overwrite" or "all" or "skip" or "none"
Public Sub ExtractFile(Index As Long, _
                       Optional ByVal DestinationDir As String = "", _
                       Optional ByVal DestinationFilename As String = "", _
                       Optional ByRef AllNone As String = "")
  Dim curRecord As Long
  Dim maxRecord As Long
  Dim curHeader As HeaderType
  Dim curName As String
  Dim curContents As RecordType
  Dim nFileBytes As Long
  Dim curBlocks As Long
  Dim curHeaderNum As Long
  Dim OutFile As Integer
  Dim nBytesLastRecord As Long
  Dim curByte As Long
  Dim fullOutFilename As String
  Dim tmpFilename As String
  Dim OverwriteAll As Boolean
  
  AllNone = LCase(AllNone)
  Select Case AllNone
    Case "all", "none", "": 'Leave lasting values or blank alone
    Case Else: AllNone = "" 'reset one-shot answers overwrite and skip
  End Select
  
  ScanArchive
  If pArchiveFileHeaderRecords.Count < Index Or Index < 1 Then
    MsgBox "File number " & Index & " not found between 1 and " & pArchiveFileHeaderRecords.Count, vbOKOnly, "FileContents"
  ElseIf OpenArchive Then
    
    If Len(DestinationDir) > 0 Then
      If Right(DestinationDir, 1) <> "\" Then DestinationDir = DestinationDir & "\"
    End If
    
    curRecord = pArchiveFileHeaderRecords(Index)
    maxRecord = LOF(pFileHandle) / RECORDSIZE
    
    Get pFileHandle, curRecord, curHeader
    If Len(DestinationFilename) = 0 Then DestinationFilename = TrimNulls(curHeader.Name)
    nFileBytes = OctalStringToLong(curHeader.size)
    curBlocks = nFileBytes / RECORDSIZE
    nBytesLastRecord = nFileBytes Mod RECORDSIZE
    If curBlocks * RECORDSIZE < nFileBytes Then curBlocks = curBlocks + 1
    If curRecord + curBlocks < maxRecord Then maxRecord = curRecord + curBlocks
    OutFile = FreeFile
    fullOutFilename = ReplaceString(DestinationDir & DestinationFilename, "/", "\")
    'If the file we are trying to write already exists, extract to a temporary file
    If FileExists(fullOutFilename) Then
      If FileLen(fullOutFilename) = 0 Then
        Kill fullOutFilename
        tmpFilename = fullOutFilename
      Else
        tmpFilename = GetTmpFileName
      End If
    Else
      tmpFilename = fullOutFilename
    End If
    Open tmpFilename For Binary As OutFile
    For curRecord = curRecord + 1 To maxRecord
      Get pFileHandle, curRecord, curContents
      If curRecord = maxRecord And nBytesLastRecord > 0 Then
        For curByte = 1 To nBytesLastRecord
          Put OutFile, , curContents.b(curByte)
        Next
      Else
        Put OutFile, , curContents
      End If
    Next
    Close pFileHandle
    Close OutFile
    
    SetFileTimesGMT tmpFilename, DateAdd("s", OctalStringToLong(curHeader.mtime), EPOCH), _
                                 DateAdd("s", OctalStringToLong(curHeader.atime), EPOCH), _
                                 DateAdd("s", OctalStringToLong(curHeader.ctime), EPOCH)
    
    'If we extracted to a temporary file, compare existing and temporary versions of file
    If (tmpFilename <> fullOutFilename) Then
      Select Case FirstMismatch(tmpFilename, fullOutFilename)
        Case -1: 'Error comparing files, should not happen since we know both exist
        Case 0:  'files match, so no need to ask, just remove newly unpacked file
          Kill tmpFilename
        Case Else:
          Select Case AllNone
            Case "all": GoTo OverwriteFile
            Case "none": GoTo SkipFile
            Case Else
              Dim lMsgBox As ATCoMessage
              Set lMsgBox = New ATCoMessage
              Select Case lMsgBox.Show("Trying to write file:" & vbCr _
                                      & fullOutFilename & vbCr _
                                      & "but file already exists and is different from file being extracted." & vbCr _
                                      & "Replace existing file?", _
                                      "Reading archive '" & pRealFilename & "'", _
                                      "Replace", "Replace All", "Skip File", "Abort")
                Case 1: AllNone = "overwrite"
OverwriteFile:          Kill fullOutFilename
                        Name tmpFilename As fullOutFilename
                Case 2: AllNone = "all"
                        Kill fullOutFilename
                        Name tmpFilename As fullOutFilename
                Case 3: AllNone = "skip"
SkipFile:               Kill tmpFilename
                Case 4: AllNone = "none"
                        Kill tmpFilename
              End Select
          End Select
          
'          If MsgBox("Trying to write file:" & vbCr _
'                  & fullOutFilename & vbCr _
'                  & "but file already exists and is different from file being extracted." & vbCr _
'                  & "Overwrite existing file?", vbYesNo, "Extracting from archive '" & pRealFilename & "'") = vbYes Then
'            Kill fullOutFilename
'            Name tmpFilename As fullOutFilename
'          Else
'            Kill tmpFilename
'          End If
      End Select
    End If
  End If
End Sub

Public Function FileAsString(Index As Long) As String
  Dim tmpFile As String
  tmpFile = GetTmpFileName
  ExtractFile Index, "", tmpFile
  FileAsString = WholeFileString(tmpFile)
  Kill tmpFile
End Function

Public Function FileIndexByName(filename As String) As Long
  Dim fn As FastCollection, item As Variant, Index As Long, ShortFilename As String
  Set fn = ArchiveFilenames
  Index = fn.IndexFromKey(LCase(filename))
  If Index > 0 Then
    FileIndexByName = Index
  Else
    ShortFilename = FilenameNoPath(filename)
    Index = fn.IndexFromKey(LCase(ShortFilename))
    If Index > 0 Then
      FileIndexByName = Index
    Else
      FileIndexByName = 0 'Not found yet
      Index = 1
      For Each item In fn
        If item = filename Or item = ShortFilename Then
          FileIndexByName = Index 'Found
          Exit For
        Else
          Index = Index + 1
        End If
      Next
    End If
  End If
End Function

'Public Function FileContents(index As Long) As String
'  Dim retval As String
'  Dim curRecord As Long, maxRecord As Long
'  Dim curHeader As HeaderType, curName As String, curContents As StringRecordType
'  Dim nFileBytes As Long, curBlocks As Long
'  Dim curHeaderNum As Long
'
'  curHeaderNum = 1
'  pFileHandle = FreeFile
'  curRecord = 1
'  retval = ""
'
'  Open pFilename For Random As pFileHandle Len = RECORDSIZE
'  maxRecord = LOF(pFileHandle) / RECORDSIZE
'
'  While curRecord < maxRecord And curHeaderNum <= index
'    Get pFileHandle, curRecord, curHeader
'    curName = TrimNulls(curHeader.Name)
'    curBlocks = 0
'    If curName <> "" Then
'      'Debug.Print curHeader.chksum
'      curHeaderNum = curHeaderNum + 1
'      nFileBytes = OctalStringToLong(curHeader.size)
'      curBlocks = nFileBytes / RECORDSIZE
'      If curBlocks * RECORDSIZE < nFileBytes Then curBlocks = curBlocks + 1
'    End If
'    curRecord = curRecord + curBlocks + 1 'Move to header after this file
'  Wend
'  If curHeaderNum = index + 1 Then
'    If curRecord - 1 <= maxRecord Then maxRecord = curRecord - 1
'    curRecord = curRecord - curBlocks
'    While curRecord <= maxRecord
'      Get pFileHandle, curRecord, curContents
'      retval = retval & curContents.s
'      curRecord = curRecord + 1
'    Wend
'    retval = Left(retval, nFileBytes)
'  End If
'  Close pFileHandle
'  FileContents = retval
'End Function

Private Sub Class_Initialize()
  Dim i As Long
  For i = 1 To 512
    ZeroBytes.b(i) = 0
  Next
  GzipOptions = "-6"
End Sub

'Save changes to disk, in case we have been appending to temporary tar file and want it gzipped now
Public Sub Flush()
  Dim FileToBeCompressed As String
  Dim CommandLine As String
  If pFilename <> pRealFilename And pModifiedFile And FileExists(pFilename) Then
    FileToBeCompressed = FilenameNoExt(pRealFilename)
    Debug.Print "FileCopy " & pFilename & " -> " & FileToBeCompressed
    FileCopy pFilename, FileToBeCompressed
    CommandLine = GzipPath & " -q " & GzipOptions & " """ & FileToBeCompressed & """"
    Debug.Print CommandLine
    Shell CommandLine
    While FileExists(FileToBeCompressed)
      DoEvents
      Sleep 50
    Wend
    pModifiedFile = False
  End If
End Sub

Private Sub Class_Terminate()
  Debug.Print "Terminating Archive " & pRealFilename & " at " & Now
  If pFilename <> pRealFilename Then
    Flush
    If FileExists(pFilename) Then Kill pFilename
  End If
End Sub
