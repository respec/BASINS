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
Private pArchiveFilenames As Collection

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
  uname As String * TUNMLEN   '266-297
  gname As String * TGNMLEN   '298-329
  devmajor As String * 8      '330-337
  devminor As String * 8      '338-345
  pad(166) As Byte            '346-512
  'Additional fields used by GNU tar but not used by us
'  atime As String * 12
'  ctime As String * 12
'  offset As String * 12
'  longnames As String * 4
'  pad As String * 1
'  sp(SPARSE_IN_HDR) As sparse
'  isextended As String * 1
'  realsize As String * 12      ' true size of the sparse file
'  ending_blanks As String * 12 ' number of nulls at the end of the file
End Type

Public Property Get TarFilename() As String
  TarFilename = pFilename
End Property
Public Property Let TarFilename(newValue As String)
  pFilename = newValue
  Set pArchiveFilenames = Nothing
  Set pArchiveFilenames = New Collection
End Property

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

Private Function LongToOctalString(val As Long, digits As Long) As String
  Dim retval As String
  Dim remainder As Long
  Dim digit As Long, digitVal As Long, power As Long
  retval = ""
  remainder = val
  For digit = digits - 1 To 0 Step -1
    power = 8 ^ digit
    digitVal = Int(remainder / power)
    retval = retval & CStr(digitVal)
    remainder = remainder - digitVal * power
  Next
  LongToOctalString = retval
End Function

Private Function ComputeChecksum(header As HeaderType) As String
  Dim byt As RecordType
  Dim sum As Long
  Dim index As Long
  sum = 0
  LSet byt = header
  For index = 1 To 512
    sum = sum + byt.b(index)
  Next
  ComputeChecksum = sum
End Function

Public Sub AppendFile(Filename As String, fileContents As String, fileDate As Date)
  Dim curRecord As Long, maxRecord As Long, ThisFileHeaderRecord As Long
  Dim curHeader As HeaderType, HeaderBytes As RecordType
  Dim curName As String
  Dim curBytes As Long, curBlocks As Long, bytesWritten As Long
  Dim SaveFilename As String, writeBlock As StringRecordType
  
  'curBytes = InStrRev(Filename, "\")
  'If curBytes > 0 Then
  '  SaveFilename = Mid(Filename, curBytes + 1)
  'Else
    SaveFilename = ReplaceString(Filename, "\", "/")
  'End If
  Debug.Print "Append: " & Filename & " (" & SaveFilename & ") " & Left(fileContents, 20) & "..."
  pFileHandle = FreeFile
  curRecord = 1
  
  Open pFilename For Random As pFileHandle Len = RECORDSIZE
  maxRecord = LOF(pFileHandle) / RECORDSIZE
  curName = "foo"
  While curRecord < maxRecord And curName <> ""
    Get pFileHandle, curRecord, curHeader
    curName = TrimNulls(curHeader.Name)
    curBlocks = 0
    If curName <> "" Then
      If curName = SaveFilename Then
        MsgBox SaveFilename & " already exists in this tar file", vbOKOnly, "Could not add file"
        GoTo Nevermind
      End If
      curBytes = OctalStringToLong(curHeader.size)
      curBlocks = curBytes / RECORDSIZE
      If curBlocks * RECORDSIZE < curBytes Then curBlocks = curBlocks + 1
      'Debug.Print curBlocks, curBytes, curHeader.name
    End If
    curRecord = curRecord + curBlocks + 1 'Move to header after this file
  Wend
  ThisFileHeaderRecord = curRecord
  curRecord = curRecord + 1
  curBytes = Len(fileContents)
  bytesWritten = 0
  While bytesWritten < curBytes
    writeBlock.s = Mid(fileContents, bytesWritten + 1, RECORDSIZE)
    bytesWritten = bytesWritten + RECORDSIZE
    If bytesWritten > curBytes Then
      Mid(writeBlock.s, RECORDSIZE - (bytesWritten - curBytes) + 1, bytesWritten - curBytes) = String(bytesWritten - curBytes, Chr(0))
    End If
    Put pFileHandle, curRecord, writeBlock
    curRecord = curRecord + 1
  Wend
  curBlocks = curBytes / RECORDSIZE
  If curBlocks * RECORDSIZE < curBytes Then curBlocks = curBlocks + 1
  LSet curHeader = ZeroBytes
  With curHeader
    .Name = SaveFilename & String(NAMSIZ - Len(SaveFilename), Chr(0))
    .mode = "0100755"
    .uid = "0000000"
    .gid = "0000000"
    .size = LongToOctalString(curBytes, 11)
    .mtime = LongToOctalString(DateDiff("s", EPOCH, fileDate), 11)
    .chksum = "        "
    .linkflag = "0"
    .magic = "       "
    .chksum = LongToOctalString(ComputeChecksum(curHeader), 6)
  End With
  Put pFileHandle, ThisFileHeaderRecord, curHeader
Nevermind:
  Close pFileHandle
End Sub

Public Property Get ArchiveFilenames() As Collection
  Dim retval As Collection
  Dim curRecord As Long, maxRecord As Long
  Dim curHeader As HeaderType, curName As String
  Dim curBytes As Long, curBlocks As Long
  
  If pArchiveFilenames.Count > 0 Then
    Set retval = pArchiveFilenames
  Else
    pFileHandle = FreeFile
    curRecord = 1
    Set retval = New Collection
    
    Open pFilename For Random As pFileHandle Len = RECORDSIZE
    maxRecord = LOF(pFileHandle) / RECORDSIZE
    While curRecord < maxRecord
      Get pFileHandle, curRecord, curHeader
      curName = TrimNulls(curHeader.Name)
      curBlocks = 0
      If curName <> "" Then
        retval.Add curName
        curBytes = OctalStringToLong(curHeader.size)
        curBlocks = curBytes / RECORDSIZE
        If curBlocks * RECORDSIZE < curBytes Then curBlocks = curBlocks + 1
      End If
      curRecord = curRecord + curBlocks + 1 'Move to header after this file
    Wend
    Close pFileHandle
    Set pArchiveFilenames = retval
  End If
  Set ArchiveFilenames = retval
End Property

Public Function ArchiveFilename(index As Long) As String
  Dim retval As Collection
  Dim curRecord As Long, maxRecord As Long
  Dim curHeader As HeaderType, curName As String
  Dim curBytes As Long, curBlocks As Long
  Dim curHeaderNum As Long
  
  curHeaderNum = 0
  pFileHandle = FreeFile
  curRecord = 1
  Set retval = New Collection
  
  Open pFilename For Random As pFileHandle Len = RECORDSIZE
  maxRecord = LOF(pFileHandle) / RECORDSIZE
  
  While curRecord < maxRecord And curHeaderNum < index
    Get pFileHandle, curRecord, curHeader
    curName = TrimNulls(curHeader.Name)
    curBlocks = 0
    If curName <> "" Then
      curHeaderNum = curHeaderNum + 1
      curBytes = OctalStringToLong(curHeader.size)
      curBlocks = curBytes / RECORDSIZE
      If curBlocks * RECORDSIZE < curBytes Then curBlocks = curBlocks + 1
    End If
    curRecord = curRecord + curBlocks + 1 'Move to header after this file
  Wend
  Close pFileHandle
  
  If curHeaderNum = index Then
    ArchiveFilename = curName
  Else
    ArchiveFilename = ""
  End If
End Function

Public Function fileContentsByName(Filename As String) As String
  Dim fn As Collection, item As Variant, index As Long, ShortFilename As String
  index = 1
  ShortFilename = FilenameNoPath(Filename)
  Set fn = ArchiveFilenames
  For Each item In fn
    If item = Filename Or item = ShortFilename Then
      fileContentsByName = fileContents(index)
      Exit Function
    Else
      index = index + 1
    End If
  Next
End Function

Public Function fileContents(index As Long) As String
  Dim retval As String
  Dim curRecord As Long, maxRecord As Long
  Dim curHeader As HeaderType, curName As String, curContents As StringRecordType
  Dim curBytes As Long, curBlocks As Long
  Dim curHeaderNum As Long
  
  curHeaderNum = 1
  pFileHandle = FreeFile
  curRecord = 1
  retval = ""
  
  Open pFilename For Random As pFileHandle Len = RECORDSIZE
  maxRecord = LOF(pFileHandle) / RECORDSIZE
  
  While curRecord < maxRecord And curHeaderNum <= index
    Get pFileHandle, curRecord, curHeader
    curName = TrimNulls(curHeader.Name)
    curBlocks = 0
    If curName <> "" Then
      'Debug.Print curHeader.chksum
      curHeaderNum = curHeaderNum + 1
      curBytes = OctalStringToLong(curHeader.size)
      curBlocks = curBytes / RECORDSIZE
      If curBlocks * RECORDSIZE < curBytes Then curBlocks = curBlocks + 1
    End If
    curRecord = curRecord + curBlocks + 1 'Move to header after this file
  Wend
  If curHeaderNum = index + 1 Then
    If curRecord - 1 <= maxRecord Then maxRecord = curRecord - 1
    curRecord = curRecord - curBlocks
    While curRecord <= maxRecord
      Get pFileHandle, curRecord, curContents
      retval = retval & curContents.s
      curRecord = curRecord + 1
    Wend
    retval = Left(retval, curBytes)
  End If
  Close pFileHandle
  fileContents = retval
End Function

Private Sub Class_Initialize()
  Dim i As Long
  For i = 1 To 512
    ZeroBytes.b(i) = 0
  Next
End Sub
