Attribute VB_Name = "modFileType"
Option Explicit
'##MODULE_REMARKS Copyright 2004 AQUA TERRA Consultants - Royalty-free use permitted under open source license

'##MODULE_DESCRIPTION Subroutine for determining file type

Public Function FileType(Filename As String) As String
'##SUMMARY FileType - Returns a string describing the type of the specified file
'##PARAM FileName - name of file to test
  Dim inFile As Integer, FileLength&
  Dim buf() As Byte
  '##LOCAL InFile - long filenumber of text file
  '##LOCAL retval() - byte array containing return values
  
  If Not FileExists(Filename) Then
    FileType = "no file"
  Else
    FileType = "unknown"
    On Error GoTo ErrorReading
    inFile = FreeFile(0)
    Open Filename For Binary As inFile
    FileLength = LOF(inFile)
    ReDim buf(4)
    Get #inFile, , buf
    Close inFile
    If buf(0) = &H4D And buf(1) = &H5A Then
      FileType = "exe"
    'ElseIf buf(0) = &H50 And buf(1) = &H45 Then
    '  FileType = "exe"
    ElseIf buf(0) = &H4C And buf(1) = &H0 And buf(2) = &H0 And buf(3) = &H0 Then
      FileType = "lnk"
    
    'archive formats
    ElseIf buf(0) = &H50 And buf(1) = &H4B And buf(2) = &H3 And buf(3) = &H4 Then
      FileType = "zip"
    ElseIf buf(0) = &H42 And buf(1) = &H5A Then
      FileType = "bzip"
    ElseIf buf(0) = &H1F And buf(1) = &H8B Then
      FileType = "gzip"
    ElseIf buf(0) = &H75 And buf(1) = &H73 And buf(2) = &H74 And buf(3) = &H61 Then
      FileType = "tar"
      
    'image formats
    ElseIf buf(0) = &H42 And buf(1) = &H4D Then
      FileType = "bmp"
    ElseIf buf(0) = &H47 And buf(1) = &H49 And buf(2) = &H46 And buf(3) = &H38 Then
      FileType = "gif"
    ElseIf buf(0) = &HFF And buf(1) = &HD8 And buf(2) = &HFF And buf(3) = &HE0 Then
      FileType = "jpg"
    ElseIf buf(0) = &H89 And buf(1) = &H50 And buf(2) = &H4E And buf(3) = &H47 Then
      FileType = "png"
    ElseIf buf(0) = &H25 And buf(1) = &H21 Then
      FileType = "ps"
    ElseIf buf(0) = &H4D And buf(1) = &H4D And buf(2) = &H0 And buf(3) = &H2A Then
      FileType = "tif-be" 'big endian (Motorola)
    ElseIf buf(0) = &H49 And buf(1) = &H49 And buf(2) = &H2A And buf(3) = &H0 Then
      FileType = "tif"
    ElseIf ByteIsPrintable(buf(0)) And ByteIsPrintable(buf(1)) And ByteIsPrintable(buf(2)) And ByteIsPrintable(buf(3)) Then
      Dim i As Long, lastByte As Long
      buf = WholeFileBytes(Filename)
      lastByte = UBound(buf)
      For i = 4 To lastByte
        If Not ByteIsPrintable(buf(i)) Then
          If i = lastByte And buf(i) = 0 Then
            'Let a zero byte exist, but only at end of file
          Else
            FileType = "unknown " & Hex(buf(0)) & " " & Hex(buf(1)) & " " & Hex(buf(2)) & " " & Hex(buf(3)) & " (" & i & "=" & buf(i) & ")"
            Exit Function
          End If
        End If
      Next
      FileType = "txt"
    Else
      FileType = "unknown " & Hex(buf(0)) & " " & Hex(buf(1)) & " " & Hex(buf(2)) & " " & Hex(buf(3))
    End If
  End If
  Exit Function

ErrorReading:
  MsgBox "Error reading '" & Filename & "'" & vbCr & vbCr & Err.Description, vbOKOnly, "FileType"
End Function

