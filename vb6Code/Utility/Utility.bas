Attribute VB_Name = "UTILITY"
Option Explicit
'##MODULE_REMARKS Copyright 2001-3 AQUA TERRA Consultants - Royalty-free use permitted under open source license

' ##MODULE_NAME UTILITY
' ##MODULE_DATE March 3, 2003
' ##MODULE_AUTHOR Mark Gray and Jack Kittle of AQUA TERRA CONSULTANTS
' ##MODULE_DESCRIPTION General utility subroutines and functions shared by many projects (don't _
 change)

Type LongType
' ##SUMMARY 1 long integer variable.
' ##UDT_MEMBER_DESCRIPTION l long integer value
  l As Long
End Type

Type FourByteType
' ##SUMMARY Set of 4 Bytes.
' ##UDT_MEMBER_DESCRIPTION b1 first byte of FourByteType
' ##UDT_MEMBER_DESCRIPTION b2 second byte of FourByteType
' ##UDT_MEMBER_DESCRIPTION b3 third byte of FourByteType
' ##UDT_MEMBER_DESCRIPTION b4 fourth byte of FourByteType
  b1 As Byte
  b2 As Byte
  b3 As Byte
  b4 As Byte
End Type

Type IntegerType
' ##SUMMARY 1 short integer variable.
' ##UDT_MEMBER_DESCRIPTION l short integer value
  i As Integer
End Type

Type TwoByteType
' ##SUMMARY Set of 2 Bytes.
' ##UDT_MEMBER_DESCRIPTION b1 first byte of TwoByteType
' ##UDT_MEMBER_DESCRIPTION b2 second byte of TwoByteType
  b1 As Byte
  b2 As Byte
End Type

Type SingleType
' ##SUMMARY 1 single-precision floating-point variable.
' ##UDT_MEMBER_DESCRIPTION single-precision floating value
  s As Single
End Type

Sub CopyI(ByRef n As Long, ByRef a() As Long, ByRef b() As Long)
Attribute CopyI.VB_Description = "Copies a long integer array."
' ##SUMMARY Copies a long integer array.
' ##PARAM n I Number of values to copy
' ##PARAM a I Integer array to copy from
' ##PARAM b M Integer array to copy to
  ' ##LOCAL Index in copy loop
  Dim i As Long
  For i = 0 To n - 1
    b(i) = a(i)
  Next i
End Sub

Function ChDriveDir(ByVal newPath As String) As Boolean
Attribute ChDriveDir.VB_Description = "Changes directory and, if necessary, drive. Returns True if successful."
' ##SUMMARY Changes directory and, if necessary, drive. Returns True if successful.
' ##PARAM newPath I New pathname.
' ##RETURNS True if directory change is successful.
  On Error GoTo Failed
  If FileExists(newPath, True, False) Then
    If Mid(newPath, 2, 1) = ":" Then ChDrive newPath
    ChDir newPath
    ChDriveDir = True
  End If
Failed:
End Function

Function FilenameOnly(ByRef istr As String) As String
Attribute FilenameOnly.VB_Description = "Converts full path, filename, and extension to filename only."
' ##SUMMARY Converts full path, filename, and extension to filename only.
' ##SUMMARY   Example: FilenameOnly("C:\foo\bar.txt") = "bar"
' ##PARAM istr I Filename with path and extension.
' ##RETURNS Filename without directory path or extension.
  ' ##LOCAL slashpos - position of slash in filename
  ' ##LOCAL dotpos - position of "dot" in filename
  ' ##LOCAL retval - return value of FilenameOnly
  Dim slashpos As Long
  Dim dotpos As Long
  Dim retval As String
  
  slashpos = InStrRev(istr, "\")
  retval = Mid(istr, slashpos + 1) 'if istr contains no \ then slashpos = 0
  
  dotpos = InStrRev(retval, ".")
  If dotpos > 0 Then retval = Left(retval, dotpos - 1)
  
  FilenameOnly = retval

End Function

Function FilenameNoPath(ByRef istr As String) As String
Attribute FilenameNoPath.VB_Description = "Converts full path, filename, and extension to only filename with extension."
' ##SUMMARY Converts full path, filename, and extension to only filename with extension.
' ##SUMMARY   Example: FilenameNoPath ("C:\foo\bar.txt") = "bar.txt"
' ##PARAM istr I Filename with path and extension.
' ##RETURNS Filename and extension without directory path.
  ' ##LOCAL slashpos - position of slash in filename
  Dim slashpos As Long
  
  slashpos = InStrRev(istr, "\")
  If slashpos > 0 Then
    FilenameNoPath = Mid(istr, slashpos + 1)
  Else
    FilenameNoPath = istr
  End If
End Function

Function FilenameNoExt(ByRef istr As String) As String
Attribute FilenameNoExt.VB_Description = "Converts full path, filename, and extension to only path and filename without extension."
' ##SUMMARY Converts full path, filename, and extension to only path and filename without extension.
' ##SUMMARY   Example: FilenameNoExt ("C:\foo\bar.txt") = "C:\foo\bar"
' ##PARAM istr I Filename with path and extension.
' ##RETURNS Path and filename without extension.
  ' ##LOCAL dotpos - position of "dot" in filename
  Dim dotpos As Long
  
  FilenameNoExt = istr
  dotpos = InStrRev(istr, ".")
  If dotpos > 0 Then
    If dotpos > InStrRev(istr, "\") Then FilenameNoExt = Left(istr, dotpos - 1)
  End If
End Function

Function FileExt(ByRef istr As String) As String
Attribute FileExt.VB_Description = "Reduces full path, filename, and extension to only extension."
' ##SUMMARY Reduces full path, filename, and extension to only extension.
' ##SUMMARY   Example: FileExt ("C:\foo\bar.txt") = "txt"
' ##PARAM istr I Filename with path and extension.
' ##RETURNS Extension without path or filename.
  ' ##LOCAL dotpos - position of "dot" in filename.
  Dim dotpos As Long
  
  FileExt = ""
  dotpos = InStrRev(istr, ".")
  If dotpos > 0 Then
    If dotpos > InStrRev(istr, "\") Then FileExt = Mid(istr, dotpos + 1) 'directory name may contain a dot
  End If
End Function

Function PathNameOnly(ByRef istr As String) As String
Attribute PathNameOnly.VB_Description = "Reduces full path, filename, and extension to only path."
' ##SUMMARY Reduces full path, filename, and extension to only path.
' ##SUMMARY   Example: PathNameOnly ("C:\foo\bar.txt") = "C:\foo"
' ##PARAM istr I Filename with path and extension.
' ##RETURNS Directory path without filename or extension.
  ' ##LOCAL slashpos - position of slash in filename
  Dim slashpos As Long
  
  slashpos = InStrRev(istr, "\")
  If slashpos > 0 Then
    PathNameOnly = Mid(istr, 1, slashpos - 1)
  Else
    PathNameOnly = ""
  End If
End Function

Function FilenameSetExt(ByRef istr As String, ByRef newExt As String) As String
Attribute FilenameSetExt.VB_Description = "Converts extension of filename from existing to specified."
' ##SUMMARY Converts extension of filename from existing to specified.
' ##SUMMARY   Example: FilenameSetExt ("C:\foo\bar.txt", "png") = "C:\foo\bar.png"
' ##SUMMARY   Example: FilenameSetExt ("C:\foo\bardtxt", "png") = "C:\foo\bardtxt.png"
' ##PARAM istr I Filename with path and extension.
' ##PARAM newExt I Extension to be added or to replace current extension.
' ##RETURNS Filename with new extension.
  ' ##LOCAL slashpos - position of slash in filename.
  ' ##LOCAL dotpos - position of "dot" in filename.
  Dim slashpos As Long
  Dim dotpos As Long
  
  slashpos = InStrRev(istr, "\")
  dotpos = InStr(slashpos + 1, istr, ".")
  If dotpos > 0 Then
    FilenameSetExt = Left(istr, dotpos) & newExt
  Else
    FilenameSetExt = istr & "." & newExt
  End If
End Function

Function AbsolutePath(ByVal Filename As String, ByVal StartPath As String) As String
Attribute AbsolutePath.VB_Description = "Converts an relative pathname to an absolute path given the starting directory."
' ##SUMMARY Converts an relative pathname to an absolute path given the starting directory.
' ##SUMMARY   Example: AbsolutePath("..\Data\DataFile.wdm", "C:\BASINS\models") = "C:\BASINS\Data\DataFile.wdm"
' ##PARAM StartPath I Relative file path and name.
' ##PARAM Filename I Absolute starting directory from which relative path is traced.
' ##RETURNS Absolute path and filename.
  ' ##LOCAL slashposFilename - position of slash in filename.
  ' ##LOCAL slashposPath - position of slash in pathname.
  Dim slashposFilename As Long
  Dim slashposPath As Long
  
  If Right(StartPath, 1) = "\" Then StartPath = Left(StartPath, Len(StartPath) - 1)
  
  If UCase(Left(Filename, 2)) <> UCase(Left(StartPath, 2)) Then Filename = StartPath & "\" & Filename
  
  slashposFilename = InStr(Filename, "\..\")
  While slashposFilename > 0
    slashposPath = InStrRev(Filename, "\", slashposFilename - 1)
    If slashposPath = 0 Then
      slashposFilename = 0
    Else
      Filename = Left(Filename, slashposPath) & Mid(Filename, slashposFilename + 4)
      slashposFilename = InStr(Filename, "\..\")
    End If
  Wend
  AbsolutePath = Filename
End Function

Function RelativeFilename(ByVal Filename As String, ByVal StartPath As String) As String
Attribute RelativeFilename.VB_Description = "Converts an absolute pathname to a relative path given the starting directory."
' ##SUMMARY Converts an absolute pathname to a relative path given the starting directory.
' ##SUMMARY If Filename is not on the same drive as StartPath, Filename is returned.
' ##SUMMARY   Example: RelativeFilename("c:\BASINS\Data\DataFile.wdm", "c:\BASINS") = "Data\DataFile.wdm"
' ##SUMMARY   Example: RelativeFilename("c:\BASINS\OtherData\DataFile.wdm", "c:\BASINS\Data") = "..\OtherData\DataFile.wdm"
' ##PARAM Filename I Absolute file path and name
' ##PARAM StartPath I Absolute starting directory from which relative path is traced
' ##RETURNS Relative path and filename.
  ' ##LOCAL slashposFilename - position of slash in filename
  ' ##LOCAL slashposPath - position of slash in pathname
  ' ##LOCAL sameUntil - position in pathname of divergence between Filename and StartPath
  Dim slashposFilename As Long
  Dim slashposPath As Long
  Dim sameUntil As Long
  
  'Remove trailing slash if necessary
  If Right(StartPath, 1) = "\" Then StartPath = Left(StartPath, Len(StartPath) - 1)
  
  If Len(Filename) > 2 Then
    If Left(Filename, 3) = "..\" Then
      'Concatenate StartPath and Filename
      Filename = StartPath & "\" & Filename
    End If
  End If
  
  'Adjust path for Filename as necessary
  slashposFilename = InStr(Filename, "\..\")
  While slashposFilename > 0
    slashposPath = InStrRev(Filename, "\", slashposFilename - 1)
    If slashposPath = 0 Then
      slashposFilename = 0
    Else
      Filename = Left(Filename, slashposPath) & Mid(Filename, slashposFilename + 4)
      slashposFilename = InStr(Filename, "\..\")
    End If
  Wend
      
  If InStr(Filename, "\") = 0 Then
    'No path to check, so assume it is a file in StartPath
  ElseIf LCase(Left(Filename, 2)) <> LCase(Left(StartPath, 2)) Then
    'filename is already relative or is on different drive
  Else
    'Reconcile StartPath and Filename
    slashposPath = Len(StartPath)
    If Mid(Filename, slashposPath + 1, 1) = "\" Then 'Filename might include whole path
      If LCase(Left(Filename, slashposPath)) = LCase(StartPath) Then
        sameUntil = slashposPath + 1
        GoTo FoundSameUntil
      End If
    End If
    slashposFilename = 0
    slashposPath = 0
    'Search for point of divergence between StartPath and Filename
    While slashposFilename = slashposPath
      If LCase(Left(Filename, slashposPath)) = LCase(Left(StartPath, slashposPath)) Then
        sameUntil = slashposPath
        slashposFilename = InStr(slashposPath + 1, Filename, "\")
        slashposPath = InStr(slashposPath + 1, StartPath, "\")
        If slashposPath = 0 Then slashposPath = -1 'If neither has another \, must end loop
      Else
        slashposFilename = 0
        slashposPath = -1
      End If
    Wend
FoundSameUntil:
    'Set relative path from point of divergence between StartPath and Filename
    Filename = Mid(Filename, sameUntil + 1)
    If sameUntil < 1 Then sameUntil = 1
    slashposPath = InStr(sameUntil, StartPath, "\")
    While slashposPath > 0
      Filename = "..\" & Filename
      slashposPath = InStr(slashposPath + 1, StartPath, "\")
    Wend
  End If
  RelativeFilename = Filename
End Function

Public Sub MkDirPath(ByVal newPath As String)
Attribute MkDirPath.VB_Description = "Makes the specified directory and any above it that are not yet there."
' ##SUMMARY Makes the specified directory and any above it that are not yet there.
' ##SUMMARY   Example: MkDirPath("C:\foo\bar") creates the "C:\foo" and "C:\foo\bar" directories if they do not already exist.
' ##PARAM newPath I Path to specified directory
  ' ##LOCAL UpPath - parent directory of newPath
  Dim UpPath As String
  
  If Len(newPath) > 0 And Not FileExists(newPath, True) Then
    If Right(newPath, 1) = "\" Then newPath = Left(newPath, Len(newPath) - 1)
    UpPath = PathNameOnly(newPath)
    If Len(UpPath) > 0 Then MkDirPath UpPath
    MkDir newPath
  End If
End Sub

Function InList(ByRef s As String, ByRef lst As Object) As Boolean
Attribute InList.VB_Description = "Returns Boolean for presence of string as item in ListBox."
' ##SUMMARY Returns Boolean for presence of string as item in ListBox.
' ##SUMMARY   Example: InList("Georgia", StateList) = True (assuming object contains list of states)
' ##PARAM s I String to be searched for in list
' ##PARAM lst I Object containing list
' ##RETURNS True if ListBox lst contains item s.
 ' ##LOCAL i - index for members of lst object
 ' ##LOCAL f - Boolean used to set InList
  Dim i As Long
  Dim f As Boolean
    
    i = 0
    f = False
    Do While Not f
      If s = lst.List(i) Then
        f = True
      ElseIf i < lst.ListCount - 1 Then
        i = i + 1
      Else
        Exit Do
      End If
    Loop
    
    InList = f
    
End Function

Function lenStr(ByRef istr As String) As Long
Attribute lenStr.VB_Description = "Returns length of given string."
' ##SUMMARY Returns length of given string.
' ##SUMMARY   Example: LenStr("HowLong") = 7
' ##PARAM istr I String whose length is to be determined
' ##RETURNS Integer length of istr.
  lenStr = Len(RTrim(istr))
End Function

Function ListFindFirstSel(ByRef o As Object) As Long
Attribute ListFindFirstSel.VB_Description = "Locates first selected item in ListBox."
' ##SUMMARY Locates first selected item in ListBox.
' ##SUMMARY   Example: ListFindFirstSel(StateList) = "Alabama" (assuming object contains list of selected states)
' ##PARAM o I Object containing list
' ##RETURNS Index of first selected item in ListBox.
  ' ##LOCAL i - index for members of lst object
   Dim i As Long
   
   On Error GoTo x:

   For i = 1 To o.ListItems.Count
     If o.ListItems(i).Selected = True Then
       ListFindFirstSel = i
       Exit Function
     End If
   Next i
x:
   On Error GoTo y:
     'Set to first row of ATCoGrid as default if that type of object
     ListFindFirstSel = o.SelStartRow
     Exit Function
y:
   ListFindFirstSel = 0
End Function

Function Log10(ByVal x As Double) As Double
Attribute Log10.VB_Description = "Calculates the log 10 of a given number."
' ##SUMMARY Calculates the log 10 of a given number.
' ##SUMMARY   Example: Log10(218.7761624) = 2.34
' ##PARAM X I Double-precision number
' ##RETURNS Log 10 of given number.
  'Do not try to calculate if (X <= 0)
  If x > 0 Then Log10 = Log(x) / Log(10#) Else Log10 = 1
End Function

Function NumFmtRE(ByVal rtmp As Single, Optional ByRef maxWidth As Long = 16) As String
Attribute NumFmtRE.VB_Description = "Converts single-precision number to string with exponential syntax if length of number exceeds specified length."
' ##SUMMARY Converts single-precision number to string with exponential syntax if length of number exceeds specified length.
' ##SUMMARY If unspecified, length defaults to 16.
' ##SUMMARY   Example: NumFmtRE(123000000, 7) = "1.23e-8"
' ##PARAM rtmp I Single-precision number to be formatted
' ##PARAM maxWidth I Length of string to be returned including decimal point and exponential syntax
' ##RETURNS Input parameter rtmp formatted, if necessary, to scientific notation.
  ' ##LOCAL LogVal - double-precision log10 value of rtmp
  ' ##LOCAL retval - string used as antecedent to NumFmtRE
  ' ##LOCAL expFormat - string syntax of exponential format
  ' ##LOCAL DecimalPlaces - long number of decimal places
  Dim LogVal As Double
  Dim retval As String
  Dim expFormat As String
  Dim DecimalPlaces As Long
  
  retval = CStr(rtmp)
  NumFmtRE = retval

  If rtmp <> 0 And maxWidth > 0 Then
    If Len(retval) > maxWidth Then
      If Len(retval) - maxWidth = 1 And Left(retval, 2) = "0." Then
        'special case, can just eliminate leading zero
        retval = Mid(retval, 2)
      ElseIf Len(retval) - maxWidth = 1 And Left(retval, 3) = "-0." Then
        'special case, can just eliminate leading zero
        retval = "-" & Mid(retval, 3)
      Else
        'Determine appropriate log syntax
        LogVal = Abs(Log10(Abs(rtmp)))
        If LogVal >= 100 Then
          expFormat = "e-###"
        ElseIf LogVal >= 10 Then
          expFormat = "e-##"
        Else
          expFormat = "e-#"
        End If
        'Set appropriate decimal position
        DecimalPlaces = maxWidth - Len(expFormat) - 2
        'If DecimalPlaces < 1 Then DecimalPlaces = 1  'pbd changed to accomodate 1.e-5
        If (DecimalPlaces < 0) Or (DecimalPlaces = 0 And rtmp > 1#) Then
          DecimalPlaces = 1
        End If
        
        retval = Format(rtmp, "#." & String(DecimalPlaces, "#") & expFormat)
      End If
    End If
  End If
  NumFmtRE = retval
End Function

Function NumFmted(ByVal rtmp As Single, ByVal wid As Long, ByVal dpla As Long) As String
Attribute NumFmted.VB_Description = "Converts single precision number to string with specified format"
' ##SUMMARY Converts single precision number to string with specified format
' ##SUMMARY   Example: NumFmted(1.23, 6, 3) = " 1.230"
' ##PARAM rtmp I Single-precision number to be formatted
' ##PARAM wid I Long width of formatted number
' ##PARAM dpla I Long number of decimal places in formatted number
' ##RETURNS Input parameter rtmp formatted to specified width _
    with specified number of decimal places (left buffered).
  ' ##LOCAL fmt - string representation of generic format (i.e., "#0.000")
  ' ##LOCAL nspc - long number of leading/trailing blanks
  ' ##LOCAL stmp - string used to build formatted number
  Dim fmt As String
  Dim nspc As Long
  Dim stmp As String

    On Error GoTo prob

    If wid - dpla - 2 > 0 Then
      fmt = String$(wid - dpla - 2, "#") & "0." & String$(dpla, "0") 'force 0.
    Else
      fmt = String$(wid - dpla - 1, "#") & "." & String$(dpla, "0") 'orig way
    End If
    stmp = Format$(rtmp, fmt)
    'add leading blanks
    nspc = wid - dpla - InStr(1, stmp, ".")
    If nspc < 0 Then nspc = 0
    stmp = Space$(nspc) & stmp
    'add trailing blanks
    nspc = wid - Len(stmp)
    If nspc < 0 Then nspc = 0
    NumFmted = stmp & Space$(nspc)
    Exit Function
prob:
    Debug.Print "NumFmted Problem", rtmp, wid, dpla
    NumFmted = String(wid, "#")

End Function

Function NumFmtI(ByRef itmp As Long, ByRef wid As Long) As String
Attribute NumFmtI.VB_Description = "Converts an integer to string of specified length, padded with leading zeros."
' ##SUMMARY Converts an integer to string of specified length, padded with leading zeros.
' ##SUMMARY Specified length must equal or exceed length of integer.
' ##SUMMARY   Example: NumFmtI(1234, 6) = "001234"
' ##PARAM itmp I Long integer to be formatted
' ##PARAM wid I Width of formatted integer
' ##RETURNS Input parameter itmp formatted to specified width.
  ' ##LOCAL fmt - string representation of generic format (i.e., "#####0")
  ' ##LOCAL stmp - string used to build formatted number
  Dim fmt As String
  Dim stmp As String

    If wid > 1 Then
      fmt = String$(wid - 1, "#")
    End If
    'assure at least one digit is output
    fmt = fmt & "0"
    stmp = Format$(itmp, fmt)
    'add leading blanks
    NumFmtI = Space$(wid - Len(stmp)) & stmp

End Function

Function Signif(ByRef fvalue As Double, ByRef Metric As Boolean) As Double
Attribute Signif.VB_Description = "Converts double-precision number to three significant digits."
'##SUMMARY Converts double-precision number to three significant digits
'##SUMMARY   Example: Signif(1.23456, True) =  1.23000001907349
'##PARAM fvalue - double-precision number to be formatted
'##PARAM metric - Boolean whether metric or not
  '##LOCAL tmp - Double-precision number used as antecedent to Signif
  Dim tmp As Double

    If Metric And fvalue < 10 Then
      fvalue = Fix(fvalue * 100 + 0.5)
      Signif = fvalue / 100
    ElseIf Metric And fvalue < 100 Then
      fvalue = Fix(fvalue * 10 + 0.5)
      Signif = fvalue / 10
    ElseIf fvalue < 100# Then
      Signif = Fix(fvalue + 0.5)
    Else
      tmp = 10# ^ Fix(Log10(fvalue)) / 100#
      Signif = Fix(fvalue / tmp + 0.5) * tmp
    End If

End Function
Function SignificantDigits(ByVal Val As Double, ByVal digits As Long) As Double
Attribute SignificantDigits.VB_Description = "Rounds double-precision number to specified number of significant digits."
' ##SUMMARY Rounds double-precision number to specified number of significant digits.
' ##SUMMARY   Example: Signif(1.23456, 3) =  1.23
' ##PARAM val I Double-precision number to be formatted
' ##PARAM digits I Number of significant digits
' ##RETURNS Input parameter val rounded to specified number of significant digits.
  Dim CurPower As Long 'order of magnitude of val
  Dim Negative As Boolean 'true if incoming number is negative
  Dim ShiftPower As Double 'magnitude by which val is temporarily shifted
  
  If Val < 0 Then 'Have to use a positive number with Log10 below
    Negative = True
    Val = -Val
  End If
  
  CurPower = Fix(Log10(Val))
  If CurPower >= 0 Then CurPower = CurPower + 1
  ShiftPower = 10 ^ (digits - CurPower)
  Val = Val * ShiftPower 'Shift val so number of digits before decimal = significant digits
  Val = Fix(Val + 0.5)   'Round up if needed
  Val = Val / ShiftPower 'Shift val back to where it belongs

  If Negative Then Val = -Val
  
  SignificantDigits = Val
End Function

Function Str_Read() As String
Attribute Str_Read.VB_Description = "Reads a string from a binary database of open file."
' ##SUMMARY Reads a string from a binary database of open file.
' ##SUMMARY Assumes open file # is '1'.
' ##RETURNS String from a binary file.
  ' ##LOCAL tmp - string used to read binary input one character at a time
  ' ##LOCAL stmp - string used as antecedent to Str_Read
  Dim tmp As String
  Dim stmp As String

    stmp$ = ""
    Do
      tmp$ = Input$(1, #1)
      If Asc(tmp$) = 0 Then
        Exit Do
      Else
        stmp$ = stmp$ & tmp$
      End If
    Loop
    Str_Read$ = stmp$

End Function

Public Function IsInteger(ByRef istr As String) As Boolean
Attribute IsInteger.VB_Description = "Checks to see whether incoming string is an integer or not."
' ##SUMMARY Checks to see whether incoming string is an integer or not.
' ##SUMMARY Returns true if each character in string is in range [0-9].
' ##SUMMARY   Example: IsInteger(12345) = True
' ##SUMMARY   Example: IsInteger(123.45) = False
' ##PARAM istr I String to be checked for integer status
' ##RETURNS True if input parameter istr is an integer.
  ' ##LOCAL a - long number set to ascii code of each successive character in istr
  ' ##LOCAL Length - long length of istr
  ' ##LOCAL pos - long position of character in istr being checked
  Dim a As Long
  Dim Length As Long
  Dim pos As Long

  IsInteger = False
  Length = Len(istr)
  For pos = 1 To Length
    a = Asc(Mid(istr, pos, 1))
    If a < 48 Then Exit Function
    If a > 57 Then Exit Function
  Next pos
  IsInteger = True
End Function

Public Function IsAlpha(ByRef istr As String) As Boolean
Attribute IsAlpha.VB_Description = "Checks to see whether incoming string is entirely alphabetic."
' ##SUMMARY Checks to see whether incoming string is entirely alphabetic.
' ##SUMMARY   Example: IsAlpha(abcde) = True
' ##SUMMARY   Example: IsAlpha(abc123) = False
' ##PARAM istr I String to be checked for alphabetic status  ' ##LOCAL a - long number set to ascii code of each successive character in istr
' ##RETURNS True if input parameter istr contains only [A-Z] or [a-z].
  Dim a As Long
  Dim Length As Long
  Dim pos As Long
  ' ##LOCAL a - position of character in istr
  ' ##LOCAL Length - length of istr
  ' ##LOCAL pos - position of character in istr being checked
  
  IsAlpha = False
  Length = Len(istr)
  For pos = 1 To Length
    a = Asc(Mid(istr, pos, 1))
    If a < 65 Then Exit Function
    If a > 90 And a < 97 Then Exit Function
    If a > 122 Then Exit Function
  Next pos
  IsAlpha = True
End Function

Public Function IsAlphaNumeric(ByRef istr As String) As Boolean
Attribute IsAlphaNumeric.VB_Description = "Checks to see whether incoming string is entirely alphanumeric."
' ##SUMMARY Checks to see whether incoming string is entirely alphanumeric.
' ##SUMMARY   Example: IsAlphaNumeric(abc123) = True
' ##SUMMARY   Example: IsAlphaNumeric(#$*&!) = False
' ##PARAM istr I String to be checked for alphanumeric status
' ##RETURNS True if input parameter istr contains only [A-Z], [a-z], or [0-9].
  Dim a As Long
  Dim Length As Long
  Dim pos As Long
  ' ##LOCAL a - long number set to ascii code of each successive character in istr
  ' ##LOCAL Length - length of istr
  ' ##LOCAL pos - position of character in istr being checked
  
  IsAlphaNumeric = False
  Length = Len(istr)
  For pos = 1 To Length
    a = Asc(Mid(istr, pos, 1))
    If a < 48 Then Exit Function
    If a > 57 And a < 65 Then Exit Function
    If a > 90 And a < 97 Then Exit Function
    If a > 122 Then Exit Function
  Next pos
  IsAlphaNumeric = True
End Function

Public Function ByteIsPrintable(ByRef b As Byte) As Boolean
Attribute ByteIsPrintable.VB_Description = "Checks to see whether incoming byte is printable."
' ##SUMMARY Checks to see whether incoming byte is printable.
' ##SUMMARY   Example: ByteIsPrintable(44) = True
' ##SUMMARY   Example: IsAlphaNumeric(7) = False
' ##PARAM b I Byte to be checked for printable status
' ##RETURNS True if input parameter b is ASCII code 9, 10, 12, 13, 32 - 126.
  Select Case b
    Case 9, 10, 12, 13: ByteIsPrintable = True
    Case Is < 32: ByteIsPrintable = False
    Case Is < 127: ByteIsPrintable = True
  End Select
End Function

Public Sub NumChr(ByRef ilen As Long, ByRef inam() As Long, ByRef outstr As String)
Attribute NumChr.VB_Description = "Returns String of characters associated with specified ascii character codes in inam()."
' ##SUMMARY Returns String of characters associated with specified ascii _
 character codes in inam().
' ##PARAM ilen I Upper bound of inam() dimension
' ##PARAM inam I Array of long ascii character codes
' ##PARAM outstr O String of converted ascii codes
  Dim i As Long
  ' ##LOCAL i - long number used as index for member of inam()
    
  outstr = ""
  For i = 0 To ilen
    outstr = outstr & Chr(inam(i))
  Next i

End Sub

Function StrAdd(ByRef o As Object, ByRef old As String, ByRef Add As String, _
                ByRef mwid As String) As String
Attribute StrAdd.VB_Description = "Concatenates given string plus addendum, up to maximum length allowed by object (padded right)."
' ##SUMMARY Concatenates given string plus addendum, up to maximum length _
   allowed by object (padded right).
' ##PARAM o I Object containing text
' ##PARAM old I Existing string
' ##PARAM Add I New string to addend to existing string
' ##PARAM mwid I MAXimum length of addendum
' ##RETURNS Concatenated input parameters, old and Add, plus right padding _
   if necessary to extend string to specified width.
  Dim nst As String
  ' ##LOCAL nst - new string built with Add plus trailing blanks

   nst = Add
   Do While o.TextWidth(nst) < o.TextWidth(mwid)
     nst = nst & " "
   Loop
   StrAdd = old & nst
End Function

Public Sub Scalit(ByRef itype As Integer, ByRef mMin As Single, ByRef mMax As Single, _
                  ByRef plmn As Single, ByRef plmx As Single)
Attribute Scalit.VB_Description = "Determines an appropriate scale based on the minimum and maximum values and whether an arithmetic, probability, or logarithmic scale is requested. Minimum and maximum for probability plots must be standard deviates. For log scales, the minimum and maximum must not be transformed."
' ##SUMMARY Determines an appropriate scale based on the _
 minimum and maximum values and whether an arithmetic, probability, _
 or logarithmic scale is requested. Minimum and maximum for probability _
 plots must be standard deviates. For log scales, the minimum _
 and maximum must not be transformed.
' ##PARAM itype I Integer indicating type of number scale (0-1 = arithmetic, 2 = probability, other = logarithmic)
' ##PARAM mMin I Single-precision minimum incoming data value
' ##PARAM mMax I Single-precision maximum incoming data value
' ##PARAM plmn O Single-precision return value for scale minimum
' ##PARAM plmx O Single-precision return value for scale maximum
  Dim a As Integer
  Dim i As Integer
  Dim inc As Integer
  Dim x As Single
  Dim m As Single
  Dim tmax As Single
  ' ##LOCAL a - short integer holds log10 min/max values rounded down to nearest magnitude
  ' ##LOCAL i - short integer used as index for r()
  ' ##LOCAL inc - short integer increments i by 1 or -1
  ' ##LOCAL X - single-precision rounded data min/max values
  ' ##LOCAL M - single-precision estimator of min/max values for arithmetic scale
  ' ##LOCAL tmax - single-precision min/max values for distribution plots (+ = max, - = min)
  
  
  If itype = 0 Or itype = 1 Then
    'arithmetic scale
    'get next lowest mult of 10
  
    Static r(1 To 15) As Single
    ' ##LOCAL r - holds possible values for multiplier used in determining M from X
    If r(1) < 0.09 Then
      r(1) = 0.1
      r(2) = 0.15
      r(3) = 0.2
      r(4) = 0.4
      r(5) = 0.5
      r(6) = 0.6
      r(7) = 0.8
      r(8) = 1#
      r(9) = 1.5
      r(10) = 2#
      r(11) = 4#
      r(12) = 5#
      r(13) = 6#
      r(14) = 8#
      r(15) = 10#
    End If
    
      x = Rndlow(mMax)
      If x > 0# Then
        inc = 1
        i = 1
      Else
        inc = -1
        i = 15
      End If
      Do
        m = r(i) * x
        i = i + inc
      Loop While mMax > m And i <= 15 And i >= 1
      plmx = m

      If mMin < 0.5 * mMax And mMin >= 0# And itype = 1 Then
        plmn = 0#
      Else
        'get next lowest mult of 10
        x = Rndlow(mMin)
        If x >= 0# Then
          inc = -1
          i = 15
        Else
          inc = 1
          i = 1
        End If
        Do
          m = r(i) * x
          i = i + inc
        Loop While mMin < m And i >= 1 And i <= 15
        plmn = m
      End If

    ElseIf itype = 2 Then
      'logarithmic scale
      If mMin > 0.000000001 Then
        a = Fix(Log10(CDbl(mMin)))
      Else
        'too small or neg value, set to -9
        a = -9
      End If
      If mMin < 1# Then a = a - 1
      plmn = 10# ^ a

      If mMax > 0.000000001 Then
        a = Fix(Log10(CDbl(mMax)))
      Else
        'too small or neg value, set to -8
        a = -8
      End If
      If mMax > 1# Then a = a + 1
      plmx = 10# ^ a

      If plmn * 10000000# < plmx Then
        'limit range to 7 cycles
        plmn = plmx / 10000000#
      End If

    Else
      'probability plots - assumes data transformed to normal deviates
      tmax = Abs(mMax)
      If Abs(mMin) > tmax Then tmax = Abs(mMin)
      tmax = CSng(Fix(tmax * 10#) + 1) / 10#
      If tmax > 4# Then tmax = 4#
      plmn = -tmax
      plmx = tmax
    End If

End Sub

Public Function Rndlow(ByRef px As Single) As Single
Attribute Rndlow.VB_Description = "Sets values less than 1.0E-19 to 0.0 for the plotting routines for bug in DISSPLA/PR1ME. Otherwise returns values rounded to lower magnitude."
' ##SUMMARY Sets values less than 1.0E-19 to 0.0 for the _
 plotting routines for bug in DISSPLA/PR1ME. Otherwise returns values _
 rounded to lower magnitude.
' ##SUMMARY   Example: Rndlow(1.0E-20) = 0
' ##SUMMARY   Example: Rndlow(11000) = 10000
' ##PARAM px I Single-precision datum
' ##RETURNS Incoming px value, rounded to 0.0 if less than 1.0E-19.
  Dim a As Long
  Dim x As Single
  Dim sign As Single
  ' ##LOCAL a - short integer holds absolute value of log10 rounded down to nearest magnitude
  ' ##LOCAL X - single-precision set to absolute value of px
  ' ##LOCAL sign - single-precision holds positive or negative sign for px

    sign = 1#
    If px < 0# Then sign = -1#
    x = Abs(px)
    If x < 1E-19 Then
      Rndlow = 0#
    Else
      a = Int(Log10(CDbl(x)))
      Rndlow = sign * 10# ^ a
    End If

End Function

Public Function FirstStringPos(ByRef start As Long, ByRef Source As String, ParamArray SearchFor()) As Long
Attribute FirstStringPos.VB_Description = "Searches Source for each item in SearchFor array."
' ##SUMMARY Searches Source for each item in SearchFor array.
' ##PARAM start I Position in Source to start search
' ##PARAM Source I String to be searched
' ##PARAM SearchFor I Array of strings to be individually searched for
' ##RETURNS  Position of first occurrence of SearchFor item in Source. _
   Returns 0 if none were found.
  Dim vSearchFor As Variant
  Dim foundPos As Long
  Dim findPos As Long
  ' ##LOCAL vSearchFor - member of ParamArray; substring to be searched for in Source
  ' ##LOCAL foundPos - position of substring in Source
  ' ##LOCAL findPos - position of first occurence of any member of ParamArray in Source
  
  For Each vSearchFor In SearchFor
    findPos = InStr(start, Source, vSearchFor)
    If findPos > 0 Then
      If foundPos = 0 Or foundPos > findPos Then foundPos = findPos
    End If
  Next
  FirstStringPos = foundPos
End Function


Public Function FirstCharPos(ByRef start As Long, ByRef str As String, ByRef chars As String) As Long
Attribute FirstCharPos.VB_Description = "Searches str for each character in chars."
' ##SUMMARY Searches str for each character in chars.
' ##PARAM start I Position in str to start search
' ##PARAM str I String to be searched
' ##PARAM chars I String of characters to be individually searched for
' ##RETURNS  Position of first occurrence of chars character in Source. _
   Returns len(str) + 1 if no characters from chars were found in Source.
  Dim retval As Long
  Dim curval As Long
  Dim CharPos As Long
  Dim LenChars As Long
  ' ##LOCAL retval - long return value for FirstCharPos
  ' ##LOCAL curval - long position of currently first-occurring character
  ' ##LOCAL CharPos - long length of chars
  ' ##LOCAL LenChars - long length of subString
  
  retval = Len(str) + 1
  LenChars = Len(chars)
  For CharPos = 1 To LenChars
    curval = InStr(start, str, Mid(chars, CharPos, 1))
    If curval > 0 And curval < retval Then retval = curval
  Next CharPos
  FirstCharPos = retval
End Function

Function StrNoNull(ByRef s As String) As String
Attribute StrNoNull.VB_Description = "Replaces null string with blank character."
' ##SUMMARY Replaces null string with blank character.
' ##SUMMARY   Example: StrNoNull("NotNull") = "NotNull"
' ##SUMMARY   Example: StrNoNull("") = " "
' ##PARAM s I String to be analyzed
' ##RETURNS  Returns a blank character if string is null. _
 Returns incoming string otherwise.
    If Len(s) = 0 Then
      StrNoNull = " "
    Else
      StrNoNull = s
    End If
End Function

Function StrRetRem(ByRef s As String) As String
Attribute StrRetRem.VB_Description = "Divides string into 2 portions at position of 1st occurence of comma or space."
' ##SUMMARY Divides string into 2 portions at position of 1st occurence of comma or space.
' ##SUMMARY   Example: StrRetRem("This string") = "This", and s is reduced to "string"
' ##SUMMARY   Example: StrRetRem("This,string") = "This", and s is reduced to "string"
' ##PARAM s M String to be analyzed
' ##RETURNS  Returns leading portion of incoming string up to first occurence of delimeter. _
   Returns input parameter without that portion. If no comma or space in string, _
   returns whole string, and input parameter reduced to null string.
  Dim l As String
  Dim i As Long
  Dim j As Long
  ' ##LOCAL l - string to return
  ' ##LOCAL i - position of blank delimeter
  ' ##LOCAL j - position of comma delimeter
    
    s = LTrim(s) 'remove leading blanks
    
    i = InStr(s, "'")
    If i = 1 Then 'string beginning
      s = Mid(s, 2)
      i = InStr(s, "'") 'string end
    Else
      i = InStr(s, " ") 'blank delimeter
      j = InStr(s, ",") 'comma delimeter
      If j > 0 Then 'comma found
        If i = 0 Or j < i Then
          i = j
        End If
      End If
    End If
       
    If i > 0 Then 'found delimeter
      l = Left(s, i - 1) 'string to return
      s = LTrim(Mid(s, i + 1)) 'string remaining
      If InStr(Left(s, 1), ",") = 1 And i <> j Then s = Mid(s, 2)
    Else 'take it all
      l = s
      s = "" 'nothing left
    End If
    
    StrRetRem = l
    
End Function

Function StrTokens(ByVal Source As String, ByRef delim As String, ByRef quote As String) As String()
Attribute StrTokens.VB_Description = "Divides string into portions separated by specified delimeter."
' ##SUMMARY Divides string into portions separated by specified delimeter.
' ##SUMMARY   Example: StrTokens("Very,Special,string") = Array
' ##SUMMARY           (0)="Very", (1)="Special", (2)="string"
' ##PARAM Source M String to be analyzed
' ##PARAM delim I delimeter to look for in string Source
' ##PARAM quote I Multi-character string exempted from search.
' ##RETURNS  Returns array of string portions separated by specified delimeter.
  Dim retval() As String
  Dim sizeRetval As Long
  Dim nTokens As Long
  ' ##LOCAL retval - string array to return
  ' ##LOCAL sizeRetval - dimension variable for sizing string array
  ' ##LOCAL nTokens - number of tokens found in string Source
  
  sizeRetval = 20
  ReDim retval(sizeRetval)
  While Len(Source) > 0
    If nTokens > sizeRetval Then
      sizeRetval = sizeRetval * 2
      ReDim Preserve retval(sizeRetval)
    End If
    retval(nTokens) = StrSplit(Source, delim, quote)
    nTokens = nTokens + 1
  Wend
  ReDim Preserve retval(nTokens - 1)
  StrTokens = retval
End Function

Sub DumpStrings(ByRef arr() As String)
Attribute DumpStrings.VB_Description = "Outputs array of strings to debug window."
' ##SUMMARY Outputs array of strings to debug window.
' ##PARAM arr I array of strings to output
  Dim i As Long
  ' ##LOCAL i - counter for looping through arrays
  For i = LBound(arr) To UBound(arr)
    Debug.Print Format(i, "00") & ": " & arr(i)
  Next
End Sub


Function StrSplit(ByRef Source As String, ByRef delim As String, ByRef quote As String) As String
Attribute StrSplit.VB_Description = "Divides string into 2 portions at position of 1st occurence of specified delimeter. Quote specifies a particular string that is exempt from the delimeter search."
' ##SUMMARY Divides string into 2 portions at position of 1st occurence of specified _
   delimeter. Quote specifies a particular string that is exempt from the delimeter search.
' ##SUMMARY   Example: StrSplit("Julie, Todd, Jane, and Ray", ",", "") = "Julie", and "Todd, Jane, and Ray" is returned as Source.
' ##SUMMARY   Example: StrSplit("Julie, Todd, Jane, and Ray", ",", "Julie, Todd") = "Julie, Todd", and "Jane, and Ray" is returned as Source.
' ##PARAM Source M String to be analyzed
' ##PARAM delim I Single-character string delimeter
' ##PARAM quote I Multi-character string exempted from search.
' ##RETURNS  Returns leading portion of incoming string up to first occurence of delimeter. _
   Returns input parameter without that portion. If no delimiter in string, _
   returns whole string, and input parameter reduced to null string.
  Dim retval As String
  Dim i As Long
  Dim quoted As Boolean
  Dim trimlen As Long
  Dim quotlen As Long
  ' ##LOCAL retval - string to return as StrSplit
  ' ##LOCAL i - long character position of search through Source
  ' ##LOCAL quoted - Boolean whether quote was encountered in Source
  ' ##LOCAL trimlen - long length of delimeter, or quote if encountered first
  ' ##LOCAL quotlen - long length of quote
    
    Source = LTrim(Source) 'remove leading blanks
    quotlen = Len(quote)
    If quotlen > 0 Then
      i = InStr(Source, quote)
      If i = 1 Then 'string beginning
        trimlen = quotlen
        Source = Mid(Source, trimlen + 1)
        i = InStr(Source, quote) 'string end
        quoted = True
      Else
        i = InStr(Source, delim)
        trimlen = Len(delim)
      End If
    Else
      i = InStr(Source, delim)
      trimlen = Len(delim)
    End If
       
    If i > 0 Then 'found delimeter
      retval = Left(Source, i - 1) 'string to return
      Source = LTrim(Mid(Source, i + trimlen)) 'string remaining
      If quoted And Len(Source) > 0 Then
        If Left(Source, Len(delim)) = delim Then Source = Mid(Source, Len(delim) + 1)
      End If
    Else 'take it all
      retval = Source
      Source = "" 'nothing left
    End If
    
    StrSplit = retval
    
End Function

Public Function StrRepeat(ByRef repeat As Long, ByRef Source As String) As String
Attribute StrRepeat.VB_Description = "Repeats specified string specified number of times."
' ##SUMMARY Repeats specified string specified number of times.
' ##SUMMARY   Example: StrRepeat(3, "I wish I were in Kansas. ")
' ##PARAM repeat I Number of times for Source to be repeated
' ##PARAM Source I String to be repeated then returned
' ##RETURNS Returns input parameter Source in succession specified number of times.
  Dim retval As String
  Dim i As Long
  ' ##LOCAL retval - string to return as StrRepeat
  ' ##LOCAL i - long index for 'repeat' loop
  
  For i = 1 To repeat
    retval = retval & Source
  Next
  StrRepeat = retval
End Function

Function StrFirstInt(ByRef Source As String) As Long
Attribute StrFirstInt.VB_Description = "Divides alpha numeric sequence into leading numbers and trailing characters."
' ##SUMMARY Divides alpha numeric sequence into leading numbers and trailing characters.
' ##SUMMARY   Example: StrFirstInt("123Go!) = "123", and returns "Go!" as Source
' ##PARAM Source M String to be analyzed
' ##RETURNS  Returns leading numbers in Source, and returns Source without those numbers.
  Dim retval As Long
  Dim pos As Long
  ' ##LOCAL retval - number found at beginning of Source
  ' ##LOCAL pos - long character position in search through Source
  
  pos = 1
  If IsNumeric(Left(Source, 2)) Then pos = 3 'account for negative number - sign
  While IsNumeric(Mid(Source, pos, 1))
    pos = pos + 1
  Wend
  If pos < 2 Then
    retval = 0
  Else
    retval = CLng(Left(Source, pos - 1))
    Source = LTrim(Mid(Source, pos))
  End If
  StrFirstInt = retval
End Function

Sub StrToDate(ByRef txt As String, ByRef datevar As Variant)
Attribute StrToDate.VB_Description = "Converts yyyy/mm/dd date string to date variant."
' ##SUMMARY Converts yyyy/mm/dd date string to date variant.
' ##PARAM txt I Date string
' ##PARAM datevar O Date variant
  Dim ilen As Long
  Dim ipos As Long
  Dim dattmp As Variant
  ' ##LOCAL ilen - long length of text string
  ' ##LOCAL ipos - long character position in parse through text
  ' ##LOCAL dattmp - intermediate date variant
  
    txt = Trim$(txt)
    ilen = Len(txt)
    datevar = ""
    If ilen > 0 Then
      ipos = InStr(txt, "/")
      If ipos > 0 Then
        dattmp = Right(txt, ilen - ipos) & "/" & Left(txt, ipos - 1)
      End If
    End If
    datevar = DateSerial(Year(dattmp), Month(dattmp), Day(dattmp))
End Sub

Sub GetDate(ByRef big As Double, ByRef dyval As Variant)
Attribute GetDate.VB_Description = "Converts double-precision date to date variant."
' ##SUMMARY Converts double-precision date to date variant.
' ##PARAM big I Double-precision date (i.e., 19851101 = Nov 1, 1985)
' ##PARAM dyval O Date variant
  Dim yr As Long
  Dim mo As Long
  Dim dy As Long
  Dim tmp As Long
  ' ##LOCAL yr - long year
  ' ##LOCAL mo - long month
  ' ##LOCAL dy - long day
  ' ##LOCAL tmp - long temporary value of double-precision date as it is parsed

  yr = big / 10000
  tmp = big - (yr * 10000)
  mo = tmp / 100
  dy = tmp - (mo * 100)
  dyval = DateSerial(yr, mo, dy)
End Sub

Sub GetDateParts(ByRef big As Double, ByRef yr As Long, ByRef mo As Long, ByRef dy As Long)
Attribute GetDateParts.VB_Description = "Converts double-precision date to traditional year, month, and day parts."
' ##SUMMARY Converts double-precision date to traditional year, month, and day parts.
' ##SUMMARY   Example: big = 19851101 returns yr = 1985, mo = 11, dy = 1
' ##PARAM big I Double-precision date
' ##PARAM yr O Long year
' ##PARAM mo O Long month
' ##PARAM dy O Long day
  Dim tmp As Double
  ' ##LOCAL tmp - double-precision temporary value of date as it is parsed
  
  yr = big / 10000
  tmp = big - (yr * 10000)
  mo = tmp / 100
  dy = tmp - (mo * 100)
End Sub

Public Function CountString(ByRef Source As String, ByRef Find As String) As Long
Attribute CountString.VB_Description = "Searches for occurences of Find in Source."
' ##SUMMARY Searches for occurences of Find in Source.
' ##SUMMARY   Example: CountString("The lead man was lead-footed", "lead") = 2
' ##PARAM Source I Full string to be searched
' ##PARAM Find I Substring to be searched for
' ##RETURNS  Returns number of occurences of Find in Source.
  Dim retval As Long
  Dim findPos As Long
  Dim findlen As Long
  ' ##LOCAL retval - string to be returned as CountString
  ' ##LOCAL findpos - long position of Find in Source
  ' ##LOCAL findlen - long length of Find
  
  findlen = Len(Find)
  If findlen > 0 Then
    findPos = InStr(Source, Find)
    While findPos > 0
      retval = retval + 1
      findPos = InStr(findPos + findlen, Source, Find)
    Wend
  End If
  CountString = retval
End Function

Public Function ReplaceStringNoCase(ByRef Source As String, ByRef Find As String, ByRef Replace As String) As String
Attribute ReplaceStringNoCase.VB_Description = "Replaces Find in Source with Replace (not case sensitive)."
' ##SUMMARY Replaces Find in Source with Replace (not case sensitive).
' ##SUMMARY Example: ReplaceStringNoCase("He came and he went", "He", "She") = "She came and She went"
' ##PARAM Source I Full string to be searched
' ##PARAM Find I Substring to be searched for and replaced
' ##PARAM Replace I Substring to replace Find
' ##RETURNS Returns new string like Source except that _
   any occurences of Find (not case sensitive) are replaced with Replace.
  Dim retval As String
  Dim findPos As Long
  Dim lastFindEnd As Long
  Dim findlen As Long
  Dim replacelen As Long
  Dim lSource As String
  Dim lFind As String
  ' ##LOCAL retval - string to be returned as ReplaceString
  ' ##LOCAL findpos - long position of Find in Source
  ' ##LOCAL lastFindEnd - long position of first character after last replaced string in Source
  ' ##LOCAL findlen - long length of Find
  ' ##LOCAL replacelen - long length of Replace
  ' ##LOCAL lSource - local version of input parameter Source
  ' ##LOCAL lFind - local version of input parameter Find
  
  findlen = Len(Find)
  If findlen > 0 Then
    replacelen = Len(Replace)
    lSource = LCase(Source)
    lFind = LCase(Find)
    findPos = InStr(lSource, lFind)
    lastFindEnd = 1
    While findPos > 0
      retval = retval & Mid(Source, lastFindEnd, findPos - lastFindEnd) & Replace
      lastFindEnd = findPos + findlen
      findPos = InStr(findPos + findlen, lSource, lFind)
    Wend
    ReplaceStringNoCase = retval & Mid(Source, lastFindEnd)
  Else
    ReplaceStringNoCase = Source
  End If
End Function

Public Function ReplaceString(ByRef Source As String, ByRef Find As String, ByRef Replace As String) As String
Attribute ReplaceString.VB_Description = "Replaces Find in Source with Replace (case sensitive)."
' ##SUMMARY Replaces Find in Source with Replace (case sensitive).
' ##SUMMARY   Example: ReplaceString("He left", "He", "She") = "She left"
' ##PARAM Source I Full string to be searched
' ##PARAM Find I Substring to be searched for and replaced
' ##PARAM Replace I Substring to replace Find
' ##RETURNS Returns new string like Source except that _
   any occurences of Find (case sensitive) are replaced with Replace.
  Dim retval As String
  Dim findPos As Long
  Dim lastFindEnd As Long
  Dim findlen As Long
  Dim replacelen As Long
  ' ##LOCAL retval - string to be returned as ReplaceString
  ' ##LOCAL findpos - long position of Find in Source
  ' ##LOCAL lastFindEnd - long position of first character after last replaced string in Source
  ' ##LOCAL findlen - long length of Find
  ' ##LOCAL replacelen - long length of Replace
  
  findlen = Len(Find)
  If findlen > 0 Then
    replacelen = Len(Replace)
    findPos = InStr(Source, Find)
    lastFindEnd = 1
    While findPos > 0
      retval = retval & Mid(Source, lastFindEnd, findPos - lastFindEnd) & Replace
      lastFindEnd = findPos + findlen
      findPos = InStr(findPos + findlen, Source, Find)
    Wend
    ReplaceString = retval & Mid(Source, lastFindEnd)
  Else
    ReplaceString = Source
  End If
End Function

Public Sub ReplaceStringToFile(ByRef Source As String, ByRef Find As String, _
                               ByRef Replace As String, ByRef Filename As String)
Attribute ReplaceStringToFile.VB_Description = "Saves new string like Source to Filename with occurences of Find in Source replaced with Replace."
' ##SUMMARY Saves new string like Source to Filename with _
 occurences of Find in Source replaced with Replace.
' ##SUMMARY   Example: ReplaceString("He left", "He", "She") = "She left"
' ##PARAM Source I Full string to be searched
' ##PARAM Find I Substring to be searched for and replaced
' ##PARAM Replace I Substring to replace Find
' ##PARAM Filename I Name of output text file
  Dim findPos As Long
  Dim findlen As Long
  Dim lastFindEnd As Long
  Dim replacelen As Long
  Dim OutFile As Integer
  ' ##LOCAL findpos - long position of Find in Source
  ' ##LOCAL findlen - long length of Find
  ' ##LOCAL lastFindEnd - long position of first character after last replaced string in Source
  ' ##LOCAL replacelen - long length of Replace
  ' ##LOCAL OutFile - integer filenumber of output text file
  
  findlen = Len(Find)
  If findlen > 0 Then
    replacelen = Len(Replace)
    findPos = InStr(Source, Find)
    If findPos > 0 Then
      lastFindEnd = 1
      On Error GoTo ErrorWriting
      OutFile = FreeFile(0)
      Open Filename For Output As OutFile
      While findPos > 0
        Print #OutFile, Mid(Source, lastFindEnd, findPos - lastFindEnd) & Replace;
        lastFindEnd = findPos + findlen
        findPos = InStr(findPos + findlen, Source, Find)
      Wend
      Print #OutFile, Mid(Source, lastFindEnd);
      Close OutFile
    Else
      SaveFileString Filename, Source
    End If
  Else
    SaveFileString Filename, Source
  End If
  Exit Sub

ErrorWriting:
  MsgBox "Error writing '" & Filename & "'" & vbCr & vbCr & Err.Description, vbOKOnly, "ReplaceStringToFile"
End Sub

Public Sub StrTrim(ByRef istr As String)
Attribute StrTrim.VB_Description = "Removes all blanks from a string."
' ##SUMMARY Removes all blanks from a string.
' ##SUMMARY   Example: StrTrim "No Blanks" changes istr to "NoBlanks"
' ##PARAM istr I String to be searched
  Dim lstr As String
  Dim bpos As Long
  ' ##LOCAL lstr - local string
  ' ##LOCAL bpos - long position of blank

    lstr = ""
    bpos = InStr(istr, " ")
    While bpos > 0
      lstr = lstr & Mid(istr, 1, bpos)
      istr = LTrim(Mid(istr, bpos))
      bpos = InStr(istr, " ")
    Wend
    istr = lstr & istr

End Sub

Public Function StrPrintable(ByRef s As String, Optional ByRef ReplaceWith As String = "") As String
Attribute StrPrintable.VB_Description = "Converts, if necessary, non-printable characters in string to printable alternative."
' ##SUMMARY Converts, if necessary, non-printable characters in string to printable _
          alternative.
' ##PARAM S I String to be converted, if necessary.
' ##PARAM ReplaceWith I Character to replace non-printable characters in S (default="").
' ##RETURNS Input parameter S with non-printable characters replaced with specific _
          printable character.
  Dim retval As String  'return string
  Dim i As Integer      'loop counter
  Dim strLen As Integer 'length of string
  Dim ch As String      'individual character in string
  
  strLen = Len(s)
  For i = 1 To strLen
    ch = Mid(s, i, 1)
    Select Case Asc(ch)
      Case 0: GoTo EndFound
      Case 32 To 126: retval = retval & ch
      Case Else: retval = retval & ReplaceWith
    End Select
  Next
EndFound:
  StrPrintable = retval
End Function

Public Function StrSafeFilename(ByRef s As String, Optional ByRef ReplaceWith As String = "_") As String
Attribute StrSafeFilename.VB_Description = "Converts, if necessary, non-printable characters in filename to printable alternative."
' ##SUMMARY Converts, if necessary, non-printable characters in filename to printable _
          alternative.
' ##PARAM S I Filename to be converted, if necessary.
' ##PARAM ReplaceWith I Character to replace non-printable characters in S (default="").
' ##RETURNS Input parameter S with non-printable characters replaced with specific _
          printable character (default="").
  Dim retval As String  'return string
  Dim i As Integer      'loop counter
  Dim strLen As Integer 'length of string
  Dim ch As String      'individual character in filename
  
  strLen = Len(s)
  For i = 1 To strLen
    ch = Mid(s, i, 1)
    Select Case Asc(ch)
      Case 0: GoTo EndFound
      Case Is < 32, 34, 42, 47, 58, 60, 62, 63, 92, 124, Is > 126: retval = retval & ReplaceWith
      Case Else: retval = retval & ch
    End Select
  Next
EndFound:
  StrSafeFilename = retval
End Function

Public Function StrPad(ByRef s As String, _
                       ByVal NewLength As Integer, _
                       Optional ByRef PadWith As String = " ", _
                       Optional ByRef PadLeft As Boolean = True) As String
Attribute StrPad.VB_Description = "Pads a string with specific character to achieve a specified length."
' ##SUMMARY Pads a string with specific character to achieve a specified length.
' ##PARAM S M String to be padded.
' ##PARAM NewLength I Length of padded string to be returned.
' ##PARAM PadWith I Character with which to pad the string.
' ##PARAM PadLeft I Pad left if true, pad right if false.
' ##RETURNS Input parameter S padded to left or right (default=left) with _
   specific character (default=space) to specified length.
  Dim CharsToAdd As Integer 'number of characters added to S
  
  CharsToAdd = NewLength - Len(s)
  If CharsToAdd <= 0 Then
    StrPad = s
  ElseIf PadLeft Then
    StrPad = String(CharsToAdd, PadWith) & s
  Else
    StrPad = s & String(CharsToAdd, PadWith)
  End If
    
End Function

Public Sub DecimalAlign(ByRef s() As String, Optional ByRef PadLeft As Boolean = True, _
                                             Optional ByRef PadRight As Boolean = True, _
                                             Optional ByRef MinWidth As Integer = 0)
Attribute DecimalAlign.VB_Description = "Formats array of floating point decimals around location of decimal place."
' ##SUMMARY Formats array of floating point decimals around location of decimal place.
' ##PARAM S M String array containing values to be formatted.
' ##PARAM PadLeft I Number of spaces reserved to the left of the decimal place.
' ##PARAM PadRight I Number of spaces reserved to the right of the decimal place.
' ##PARAM MinWidth I Minimum number of spaces reserved for overall formatted number.
  Dim MaxDecimalPos As Integer   'furthest decimal position from left for all numbers in s
  Dim MaxAfterDecimal As Integer 'furthest decimal position from right for all numbers in s
  Dim AfterDecimal() As Integer  'array of digits after decimal
  Dim DecimalPos() As Integer    'array of decimal positions from left
  Dim iMin As Integer            'lower bound of s
  Dim iMax As Integer            'upper bound of s
  Dim i As Integer               'loop counter
  
  iMin = LBound(s)
  iMax = UBound(s)
  ReDim DecimalPos(iMin To iMax)
  ReDim AfterDecimal(iMin To iMax)
  For i = iMin To iMax
    DecimalPos(i) = InStr(s(i), ".")
    If DecimalPos(i) = 0 Then DecimalPos(i) = Len(s(i)) + 1
    If DecimalPos(i) > MaxDecimalPos Then MaxDecimalPos = DecimalPos(i)
    If PadRight Then
      AfterDecimal(i) = Len(s(i)) - DecimalPos(i)
      If AfterDecimal(i) > MaxAfterDecimal Then MaxAfterDecimal = AfterDecimal(i)
    End If
  Next
  For i = iMin To iMax
    If PadLeft Then
      If DecimalPos(i) < MaxDecimalPos Then
        s(i) = Space(MaxDecimalPos - DecimalPos(i)) & s(i)
      End If
    End If
    If PadRight Then
      If AfterDecimal(i) < MaxAfterDecimal Then
        s(i) = s(i) & Space(MaxAfterDecimal - AfterDecimal(i))
      End If
    End If
    If MinWidth > 0 Then s(i) = StrPad(s(i), MinWidth)
  Next
  
End Sub

Function SwapBytes(ByRef n As Long) As Long
Attribute SwapBytes.VB_Description = "Swaps between big and little endian 32-bit integers."
' ##SUMMARY Swaps between big and little endian 32-bit integers.
' ##SUMMARY   Example: SwapBytes(1) = 16777216
' ##PARAM N I Any long integer
' ##RETURNS Modified input parameter N.
  Dim TempLong As LongType
  Dim OrigBytes As FourByteType
  Dim NewBytes As FourByteType
  ' ##LOCAL TempLong - temporarily stores original and converted values
  ' ##LOCAL OrigBytes - stores original bytes
  ' ##LOCAL NewBytes - stores new bytes
  
  TempLong.l = n
  LSet OrigBytes = TempLong
  NewBytes.b1 = OrigBytes.b4
  NewBytes.b2 = OrigBytes.b3
  NewBytes.b3 = OrigBytes.b2
  NewBytes.b4 = OrigBytes.b1
  LSet TempLong = NewBytes
  SwapBytes = TempLong.l
End Function

Public Function ReadBigInt(ByRef inFile As Integer) As Long
Attribute ReadBigInt.VB_Description = "Reads big-endian integer from file number and converts to Intel little-endian value."
' ##SUMMARY Reads big-endian integer from file number and converts to _
   Intel little-endian value.
' ##SUMMARY   Example: ReadBigInt(1) = 1398893856
' ##PARAM InFile I Open file number
' ##RETURNS Input parameter InFile converted to Intel little-endian value.
  Dim n As Long
  ' ##LOCAL n - variable into which data is read

  Get #inFile, , n
  ReadBigInt = SwapBytes(n)
End Function

Public Sub WriteBigInt(ByRef OutFile As Integer, ByRef Val As Long)
Attribute WriteBigInt.VB_Description = "Writes 32-bit integer as big endian to specified disk file."
' ##SUMMARY Writes 32-bit integer as big endian to specified disk file.
' ##PARAM OutFile I File number
' ##PARAM Val I 32-bit integer
  Dim TempLong As LongType
  Dim NewBytes As FourByteType
  ' ##LOCAL TempLong - stores original integer value
  ' ##LOCAL NewBytes - stores new bytes
  
  TempLong.l = Val
  LSet NewBytes = TempLong
  Put OutFile, , NewBytes.b4
  Put OutFile, , NewBytes.b3
  Put OutFile, , NewBytes.b2
  Put OutFile, , NewBytes.b1
  'Dim b1 As Byte, b2 As Byte, b3 As Byte, b4 As Byte
  'b1 = (val And &HFF000000) / &H1000000
  'b2 = (val And &HFF0000) / &H10000
  'b3 = (val And &HFF00) / &H100
  'b4 = val And &HFF
  'Put outfile, , b1
  'Put outfile, , b2
  'Put outfile, , b3
  'Put outfile, , b4
End Sub

Public Sub DispError(ByRef SubID As String, ByRef e As Object)
Attribute DispError.VB_Description = "Displays error in message box for up to first 4 errors from same module."
' ##SUMMARY Displays error in message box for up to first 4 errors from same module.
' ##PARAM SubID I ID of subroutine
' ##PARAM E I Error object
  Static ecnt As Long
  ' ##LOCAL ecnt - running count of errors from module
  
    ecnt = ecnt + 1
    If ecnt < 5 Then
      MsgBox "From Sub " & SubID & ":" & vbCrLf & e.Description & vbCrLf & "Error number " & e.Number & vbCrLf, 48
    End If

End Sub

Sub ChkProb(ByRef id As String, ByRef txt As String, ByRef Min As String, _
            ByRef Max As String, ByRef opt As Long, ByRef rsp As Long)
Attribute ChkProb.VB_Description = "Displays problem message box when entered value out of range."
' ##SUMMARY Displays problem message box when entered value out of range.
' ##PARAM id I Name of variable
' ##PARAM txt I Entered value of variable
' ##PARAM Min I Minimum variable value
' ##PARAM Max I Maximum variable value
' ##PARAM opt I Flag allowing user confirmation to override limits (1 = OK)
' ##PARAM rsp O Response indicating whether overriding limits allowed (0 = no)
  Dim eStr As String
  Dim NL As String
  ' ##LOCAL eStr - error string
  ' ##LOCAL nl - carriage return line feed string

    NL = Chr(13) + Chr(10)
    eStr = "'" & txt & "' is not a valid value for" & NL
    eStr = eStr & "'" & id & "'" & NL & NL
    eStr = eStr & "Min:  " & Min
    eStr = eStr & "  Max:  " & Max & NL
    If opt = 1 Then 'confirm override of limits
      eStr = eStr & "Please confirm that you want to use it."
      rsp = MsgBox(eStr, 305, id & "Problem")
    Else
      MsgBox eStr, vbExclamation, id & "Problem"
      rsp = 0
    End If

End Sub

Sub ChkTxtI(ByRef id As String, ByRef Min As Long, ByRef Max As Long, _
            ByRef txt As String, ByRef cVal As Long, chdFlg As Long)
Attribute ChkTxtI.VB_Description = "Checks entered integer value for valid range. Automatically allows user confirmation to override Min/Max limits."
' ##SUMMARY Checks entered integer value for valid range. _
 Automatically allows user confirmation to override Min/Max limits.
' ##PARAM ID I Variable name
' ##PARAM Min I Minimum variable value
' ##PARAM Max I Maximum variable value
' ##PARAM txt I Entered value of variable
' ##PARAM cVal I Previous value of variable
' ##PARAM chdFlg O Flag for whether integer value changed (0 = no, 1 = yes)
  Dim opt As Long
  ' ##LOCAL opt - long set to 1 allowing user confirmation to override limits

    opt = 1
    Call ChkTxtIOpt(id, Min, Max, opt, txt, cVal, chdFlg)

End Sub

Sub ChkTxtR(ByRef id As String, ByRef Min As Single, ByRef Max As Single, _
            ByRef txt As String, ByRef cVal As Single, ByRef chdFlg As Long)
Attribute ChkTxtR.VB_Description = "Checks entered real value for valid range. Automatically allows user confirmation to override Min/Max limits."
' ##SUMMARY Checks entered real value for valid range. _
 Automatically allows user confirmation to override Min/Max limits.
' ##PARAM ID I Variable name
' ##PARAM Min I Minimum variable value
' ##PARAM Max I Maximum variable value
' ##PARAM txt I Entered value of variable
' ##PARAM cVal I Previous value of variable
' ##PARAM chdFlg O Flag for whether real value changed (0 = no, 1 = yes)
  Dim opt As Long
  ' ##LOCAL opt - long set to 1 allowing user confirmation to override limits

    opt = 1
    Call ChkTxtROpt(id, Min, Max, opt, txt, cVal, chdFlg)

End Sub

Public Sub ChkTxtIOpt(ByRef id As String, ByRef Min As Long, ByRef Max As Long, ByRef opt As Long, _
                      ByRef txt As String, ByRef cVal As Long, ByRef chdFlg As Long)
Attribute ChkTxtIOpt.VB_Description = "Checks entered integer value for valid range. If opt = 1, allows user confirmation to override value outside Min/Max limits."
' ##SUMMARY Checks entered integer value for valid range. _
 If opt = 1, allows user confirmation to override value outside Min/Max limits.
' ##PARAM ID I Variable name
' ##PARAM Min I Minimum variable value
' ##PARAM Max I Maximum variable value
' ##PARAM opt I Flag allowing user confirmation to override limits (1 = yes)
' ##PARAM txt I Entered value of variable
' ##PARAM cVal I Previous value of variable
' ##PARAM chdFlg O Flag for whether integer value changed (0 = no, 1 = yes)
  Dim oldval As Long
  Dim newval As Long
  Dim probFlg As Long
  Dim rsp As Long
  ' ##LOCAL oldval - long previous value of variable
  ' ##LOCAL newval - long user-entered value rounded to nearest integer
  ' ##LOCAL probFlg - long problem flag (-1 = no change, 0 = no problem, 1 = invalid change)
  ' ##LOCAL rsp - long response indicating whether overriding limits allowed (0 = no, 1 = yes)

    probFlg = 1
    chdFlg = 0
  If IsNumeric(txt) Then
'   no really bad problems
    oldval = cVal
    newval = CInt(txt)
    If (oldval = newval) Then
'     no change
      probFlg = -1
    ElseIf (newval >= Min And newval <= Max) Then
'     update listing
      cVal = newval
      probFlg = 0
      chdFlg = 1
    End If
  End If
  If probFlg = 1 Then
'   not a valid change
    Call ChkProb(id, txt, CStr(Min), CStr(Max), opt, rsp)
  End If
    txt = CStr(cVal)

End Sub

Public Sub ChkTxtROpt(ByRef id As String, ByRef Min As Single, ByRef Max As Single, ByRef opt As Long, _
                      ByRef txt As String, ByRef cVal As Single, ByRef chdFlg As Long)
Attribute ChkTxtROpt.VB_Description = "Checks entered real value for valid range. If opt = 1, allows user confirmation to override value outside Min/Max limits."
' ##SUMMARY Checks entered real value for valid range. _
 If opt = 1, allows user confirmation to override value outside Min/Max limits.
' ##PARAM ID I Variable name
' ##PARAM Min I Minimum variable value
' ##PARAM Max I Maximum variable value
' ##PARAM opt I Flag allowing user confirmation to override limits (1 = yes)
' ##PARAM txt I Entered value of variable
' ##PARAM cVal I Previous value of variable
' ##PARAM chdFlg O Flag for whether real value changed (0 = no, 1 = yes)
  Dim oldval As Single
  Dim newval As Single
  Dim rsp As Long
  ' ##LOCAL oldval - long previous value of variable
  ' ##LOCAL newval - long user-entered value rounded to nearest integer
  ' ##LOCAL rsp - long response indicating whether overriding limits allowed (0 = no, 1 = yes)

    If IsNumeric(txt) Then
      'no really bad problems
      oldval = cVal
      newval = CDbl(txt)
      If (oldval = newval) Then
        'no change
        chdFlg = 0
      ElseIf (newval >= Min And newval <= Max) Then
        'update listing
       cVal = newval
        chdFlg = 1
      Else
        'not a valid change
        Call ChkProb(id, txt, CStr(Min), CStr(Max), opt, rsp)
        If rsp = 1 Then
          'user wants to use value anyway
          cVal = newval
          chdFlg = -1
        Else
          'don't use value
          chdFlg = 0
        End If
      End If
    ElseIf lenStr(txt) > 0 Then
      MsgBox "Value entered is not a number.", 48
      chdFlg = 0
    End If
    If cVal <> -999 Then
      '-999 indicates undefined value, leave field blank
      txt = CStr(cVal)
    End If

End Sub

Public Function Long2String(ByRef Val As Long) As String
Attribute Long2String.VB_Description = "Parses long integer to FourByteType then prints out corresponding ascii codes."
' ##SUMMARY Parses long integer to FourByteType then prints out corresponding ascii codes.
' ##SUMMARY   Example: Long2String(98) = "b   "
' ##PARAM Val I Value to be converted
' ##RETURNS Input parameter Val in string form.
  Dim lVal As LongType
  Dim bVal As FourByteType
  Dim s As String
  ' ##LOCAL lval - LongType equivalent value of Val
  ' ##LOCAL bval - FourByteType equivalent value of lVal
  ' ##LOCAL s - string antecedent to Long2String
  
  lVal.l = Val
  LSet bVal = lVal
  s = Chr(bVal.b1) & Chr(bVal.b2) & Chr(bVal.b3) & Chr(bVal.b4)
  Long2String = s
  
End Function

Public Function Long2Single(ByRef Val As Long) As Single
Attribute Long2Single.VB_Description = "Sets long integer to LongType then converts to SingleType."
' ##SUMMARY Sets long integer to LongType then converts to SingleType.
' ##SUMMARY   Example: Long2Single(999999999) =  4.723787E-03
' ##PARAM Val I Value to be converted
' ##RETURNS Input parameter Val in single precision form.
  Dim lVal As LongType
  Dim rval As SingleType
  ' ##LOCAL lVal - LongType equivalent value of Val
  ' ##LOCAL rVal - SingleType equivalent value of lVal
  
  lVal.l = Val
  LSet rval = lVal
  Long2Single = rval.s
  
End Function

Public Function Byte2Integer(ByRef Byt() As Byte, ByRef Ind As Long) As Integer
Attribute Byte2Integer.VB_Description = "Converts sequence of two members in Byte array to TwoByteType then converts to short integer."
' ##SUMMARY Converts sequence of two members in Byte array _
 to TwoByteType then converts to short integer.
' ##SUMMARY   Example: Byte2Integer(Byt, 1) = 257 (= 1 + 256) where Byt(1) = 1, Byt(2) = 1
' ##PARAM Byt() I Array containing byte values to be converted
' ##PARAM Ind I Index for first of two sequential elements in Byt() to be analyzed
' ##RETURNS Sequence of 2 values from input array Byt converted to a short integer.
  Dim bVal As TwoByteType
  Dim iVal As IntegerType
  ' ##LOCAL bVal - holds TwoByteType values for members of Byt() array
  ' ##LOCAL lVal - ShortType equivalent value of bVal
  
  bVal.b1 = Byt(Ind)
  bVal.b2 = Byt(Ind + 1)
  
  LSet iVal = bVal
  Byte2Integer = iVal.i
End Function

Public Function Byte2Long(ByRef Byt() As Byte, ByRef Ind As Long) As Long
Attribute Byte2Long.VB_Description = "Converts sequence of four members in Byte array to LongType then converts to long integer."
' ##SUMMARY Converts sequence of four members in Byte array _
 to LongType then converts to long integer.
' ##SUMMARY   Example: Byte2Long(Byt, 1) = 16843009 (= 1 + 256 + 256^2 + 256^3) _
 where Byt(1) = 1, Byt(2) = 1, Byt(3) = 1, Byt(4) = 1
' ##PARAM Byt() I Array containing byte values to be converted
' ##PARAM Ind I Index for first of four sequential elements in Byt() to be analyzed
' ##RETURNS Sequence of 4 values from input array Byt converted to a long integer.
  Dim bVal As FourByteType
  Dim lVal As LongType
  ' ##LOCAL bVal - holds FourByteType values for members of Byt() array
  ' ##LOCAL lVal - LongType equivalent value of bVal
  
  bVal.b1 = Byt(Ind)
  bVal.b2 = Byt(Ind + 1)
  bVal.b3 = Byt(Ind + 2)
  bVal.b4 = Byt(Ind + 3)
  
  LSet lVal = bVal
  Byte2Long = lVal.l
End Function

Public Function Byte2Single(ByRef Byt() As Byte, ByRef Ind As Long) As Single
Attribute Byte2Single.VB_Description = "Converts sequence of four members in Byte array to SingleType then converts to single-precision number."
' ##SUMMARY Converts sequence of four members in Byte array _
 to SingleType then converts to single-precision number.
' ##SUMMARY   Example: Byte2Single(Byt, 1) = 1.037238E-08 _
 where Byt(1) = 50, Byt(2) = 50, Byt(50) = 1, Byt(50) = 1
' ##PARAM Byt() I Array containing byte values to be converted
' ##PARAM Ind I Index for first of four sequential elements in Byt() to be analyzed
' ##RETURNS Sequence of 4 values from input array Byt converted to a single precision decimal.
  Dim bVal As FourByteType
  Dim rval As SingleType
  ' ##LOCAL bVal - holds FourByteType values for members of Byt() array
  ' ##LOCAL lVal - LongType equivalent value of bVal
  
  bVal.b1 = Byt(Ind)
  bVal.b2 = Byt(Ind + 1)
  bVal.b3 = Byt(Ind + 2)
  bVal.b4 = Byt(Ind + 3)
  
  LSet rval = bVal
  On Error Resume Next
  Byte2Single = rval.s
  
End Function

Public Function Byte2String(ByRef Byt() As Byte, ByRef Ind As Long, ByRef Length As Long) As String
Attribute Byte2String.VB_Description = "Converts sequence of members in Byte array to string of corresponding ascii characters."
' ##SUMMARY Converts sequence of members in Byte array to string of _
 corresponding ascii characters.
' ##SUMMARY   Example: Byte2String(Byt, 1, 3) = "See" _
 where Byt(1) = 83, Byt(2) = 101, Byt(3) = 101
' ##PARAM Byt() I Array containing byte values to be converted
' ##PARAM Ind I Index of first element in Byt() to be analyzed
' ##PARAM Length I Number of members in Byt array to be sequentially analyzed
' ##RETURNS Sequence of 4 values from input array Byt converted to a string.
  Dim s As String
  Dim i As Long
  Dim c As Long
  ' ##LOCAL s - string antecedent to Byte2String
  ' ##LOCAL i - long counter as index to Byt array
  ' ##LOCAL c - string set to each incremental character from Byt array
  
  s = ""
  For i = 0 To Length - 1
    c = Byt(Ind + i)
    If c = 0 Then c = 32 'blank
    s = s & Chr(c)
  Next i
  Byte2String = s
End Function

Public Function WholeFileString(ByRef Filename As String) As String
Attribute WholeFileString.VB_Description = "Converts specified text file to a string."
' ##SUMMARY Converts specified text file to a string.
' ##PARAM FileName I Name of text file
' ##RETURNS Returns contents of specified text file as string.
  Dim inFile As Integer
  Dim FileLength As Long
  ' ##LOCAL InFile - long filenumber of text file
  ' ##LOCAL FileLength - long length of text file contents
  
StartOver:
  On Error GoTo ErrorReading
  
  inFile = FreeFile(0)
  Open Filename For Binary As inFile
  FileLength = LOF(inFile)
  WholeFileString = Space(FileLength)
  Get #inFile, , WholeFileString
  Close inFile
  Exit Function

ErrorReading:
  If Err.Number = 62 Then 'Input past end of file
    'We must be trying to read a binary file into a string
    'This should no longer happen now that we are opening as Binary
    WholeFileString = "binary file could not be read as string"
    Resume Next
  Else
    If MsgBox("Error reading '" & Filename & "'" & vbCr & vbCr _
            & Err.Description, vbRetryCancel, "WholeFileString") = vbRetry Then
       Resume StartOver
    Else
      WholeFileString = "Error #" & Err.Number & ": " & Err.Description
    End If
  End If
End Function


Public Function WholeFileBytes(ByRef Filename As String) As Byte()
Attribute WholeFileBytes.VB_Description = "Converts specified text file to Byte array"
' ##SUMMARY Converts specified text file to Byte array
' ##PARAM FileName I Name of text file
' ##RETURNS Returns contents of specified text file in Byte array.
  Dim inFile As Integer, FileLength&
  Dim retval() As Byte
  ' ##LOCAL InFile - long filenumber of text file
  ' ##LOCAL retval() - byte array containing return values
  
  On Error GoTo ErrorReading
  
  inFile = FreeFile(0)
  Open Filename For Binary As inFile
  FileLength = LOF(inFile)
  ReDim retval(FileLength)
  Get #inFile, , retval
  WholeFileBytes = retval
  Close inFile
  Exit Function

ErrorReading:
  MsgBox "Error reading '" & Filename & "'" & vbCr & vbCr & Err.Description, vbOKOnly, "WholeFileBytes"
End Function

Public Function FirstMismatch(ByRef filename1 As String, ByRef filename2 As String) As Long
Attribute FirstMismatch.VB_Description = "Compares 2 files and locates first sequential byte that is different between files."
' ##SUMMARY Compares 2 files and locates first sequential byte that is different between files.
' ##PARAM filename1 I Name of first file
' ##PARAM filename2 I Name of second file
' ##RETURNS Returns byte position of first non-matching byte between two files: _
   zero if they match, -1 if there was an error.
  Dim InFile1 As Integer, FileLength1 As Long
  Dim InFile2 As Integer, FileLength2 As Long
  Dim minLength As Long
  Dim longBytes As Long
  Dim testL1 As Long, testL2 As Long
  Dim testB1 As Byte, testB2 As Byte
  Dim i As Long
  ' ##LOCAL InFile1 - file handle of first file
  ' ##LOCAL InFile2 - file handle of second file
  ' ##LOCAL FileLength1 - length of first file in bytes
  ' ##LOCAL FileLength2 - length of first file in bytes
  ' ##LOCAL i - byte index in files
  
  On Error GoTo ErrorReading
  
  If Not FileExists(filename1) Or Not FileExists(filename2) Then
    FirstMismatch = -1
  Else
    InFile1 = FreeFile(0)
    Open filename1 For Binary As InFile1
    FileLength1 = LOF(InFile1)
    
    InFile2 = FreeFile(0)
    Open filename2 For Binary As InFile2
    FileLength2 = LOF(InFile2)
    
    If FileLength1 < FileLength2 Then
      minLength = FileLength1
    Else
      minLength = FileLength2
    End If
    
    longBytes = minLength - minLength Mod 4
    
    For i = 1 To longBytes Step 4
      Get #InFile1, , testL1
      Get #InFile2, , testL2
      If testL1 <> testL2 Then Exit For
    Next
    
    Do While i <= minLength
      Get #InFile1, i, testB1
      Get #InFile2, i, testB2
      If testB1 <> testB2 Then Exit Do
      i = i + 1
    Loop
    
    If i <= minLength Then 'Found a mismatch before the shorter file ended
      FirstMismatch = i
    ElseIf FileLength1 <> FileLength2 Then 'Longer file matched shorter one while it lasted
      FirstMismatch = i
    Else
      FirstMismatch = 0
    End If
    
    Close InFile1
    Close InFile2
  End If
  Exit Function

ErrorReading:
  MsgBox "Error reading '" & filename1 & "'" & vbCr & "or '" & filename2 & "'" & vbCr & Err.Description, vbOKOnly, "WholeFileBytes"
  On Error Resume Next
  Close InFile1
  Close InFile2
End Function

Public Sub SaveFileString(ByRef Filename As String, ByRef FileContents As String)
Attribute SaveFileString.VB_Description = "Saves incoming string to a text file."
' ##SUMMARY Saves incoming string to a text file.
' ##PARAM FileName I Name of output text file
' ##PARAM FileContents I Incoming string to be saved to file
  Dim OutFile As Integer
  ' ##LOCAL OutFile - integer filenumber of output text file
  
  On Error GoTo ErrorWriting
  
  MkDirPath PathNameOnly(Filename)
  
  OutFile = FreeFile(0)
  Open Filename For Output As OutFile
  Print #OutFile, FileContents;
  Close OutFile
  Exit Sub

ErrorWriting:
  MsgBox "Error writing '" & Filename & "'" & vbCr & vbCr & Err.Description, vbOKOnly, "SaveFileString"
End Sub

Public Sub SaveFileBytes(ByRef Filename As String, ByRef FileContents() As Byte)
Attribute SaveFileBytes.VB_Description = "Saves incoming Byte array to a text file."
' ##SUMMARY Saves incoming Byte array to a text file.
' ##PARAM FileName I Name of output text file
' ##PARAM FileContents I Incoming Byte array to be saved to file
  Dim OutFile As Integer
  ' ##LOCAL OutFile - integer filenumber of output text file
  
  On Error GoTo ErrorWriting
  
  MkDirPath PathNameOnly(Filename)
  
  OutFile = FreeFile(0)
  Open Filename For Binary As OutFile
  Put #OutFile, , FileContents
  Close OutFile
  Exit Sub

ErrorWriting:
  MsgBox "Error writing '" & Filename & "'" & vbCr & vbCr & Err.Description, vbOKOnly, "SaveFileBytes"
End Sub

Public Sub AppendFileString(ByRef Filename As String, ByRef appendString As String)
Attribute AppendFileString.VB_Description = "Appends incoming string to existing text file."
' ##SUMMARY Appends incoming string to existing text file.
' ##PARAM FileName I Name of existing text file
' ##PARAM appendString I Incoming string to be appended
  Dim OutFile As Integer
  ' ##LOCAL OutFile - integer filenumber of existing text file
  
  On Error GoTo ErrorWriting
  
  MkDirPath PathNameOnly(Filename)
  
  OutFile = FreeFile(0)
  Open Filename For Append As OutFile
  Print #OutFile, appendString;
  Close OutFile
  Exit Sub

ErrorWriting:
  MsgBox "Error writing '" & Filename & "'" & vbCr & vbCr & Err.Description, vbOKOnly, "AppendFileString"
End Sub

Public Sub SortIntegerArray(ByRef opt As Long, ByRef cnt As Long, ByRef iVal() As Long, ByRef pos() As Long)
Attribute SortIntegerArray.VB_Description = "Sorts integers in array into ascending order."
' ##SUMMARY Sorts integers in array into ascending order.
' ##PARAM opt I Sort option (0 = sort in place, 1 = move values in array to sorted position)
' ##PARAM cnt I Count of integers to sort
' ##PARAM iVal I Array of integers to sort
' ##PARAM pos O Array containing sorted order of integers
  Dim i As Long
  Dim j As Long
  Dim jpt As Long
  Dim jpt1 As Long
  Dim itmp As Long
  ' ##LOCAL i - long counter for outer loop
  ' ##LOCAL j - long counter for inner loop
  ' ##LOCAL jpt - long pointer to j index
  ' ##LOCAL jpt1 - long pointer to (j + 1) index
  ' ##LOCAL itmp - long temporary holder for values in iVal array

  'set default positions(assume in order)
  For i = 1 To cnt
    pos(i) = i
  Next i

  'make a pointer to values with bubble sort
  For i = cnt To 2 Step -1
    For j = 1 To i - 1
      jpt = pos(j)
      jpt1 = pos(j + 1)
      If (iVal(jpt) > iVal(jpt1)) Then
        pos(j + 1) = jpt
        pos(j) = jpt1
      End If
    Next j
  Next i

  If (opt = 1) Then
    'move integer values to their sorted positions
    For i = 1 To cnt
      If (pos(i) <> i) Then
        'need to move ints, first save whats in target space
        itmp = iVal(i)
        'move sorted data to target position
        iVal(i) = iVal(pos(i))
        'move temp data to source position
        iVal(pos(i)) = itmp
        'find the pointer to the other value we are moving
        j = i
50      'CONTINUE
        j = j + 1
        If (pos(j) <> i) Then GoTo 50
        pos(j) = pos(i)
        pos(i) = i
      End If
    Next i
  End If

End Sub

Public Sub SortRealArray(ByRef opt As Long, ByRef cnt As Long, _
                         ByRef rval() As Single, ByRef pos() As Long)
Attribute SortRealArray.VB_Description = "Sorts array of real numbers into ascending order."
' ##SUMMARY Sorts array of real numbers into ascending order.
' ##PARAM opt I Integer indicating sort option: 0 - sort in place, 1 - move values in _
          array to sorted position.
' ##PARAM cnt I Count of real numbers to sort.
' ##PARAM rval M Array of real numbers to sort.
' ##PARAM pos O Integer array containing sorted order of real numbers.
  Dim i As Long
  Dim j As Long
  Dim jpt As Long
  Dim jpt1 As Long
  Dim rtmp As Single
  ' ##LOCAL i - long counter for outer loop
  ' ##LOCAL j - long counter for inner loop
  ' ##LOCAL jpt - long pointer to j index
  ' ##LOCAL jpt1 - long pointer to (j + 1) index
  ' ##LOCAL itmp - long temporary holder for values in rVal array

  'set default positions(assume in order)
  For i = 1 To cnt
    pos(i) = i
  Next i

  'make a pointer to values with bubble sort
  For i = cnt To 2 Step -1
    For j = 1 To i - 1
      jpt = pos(j)
      jpt1 = pos(j + 1)
      If (rval(jpt) > rval(jpt1)) Then
        pos(j + 1) = jpt
        pos(j) = jpt1
      End If
    Next j
  Next i

  If (opt = 1) Then
    'move real values to their sorted positions
    For i = 1 To cnt
      If (pos(i) <> i) Then
        'need to move reals, first save whats in target space
        rtmp = rval(i)
        'move sorted data to target position
        rval(i) = rval(pos(i))
        'move temp data to source position
        rval(pos(i)) = rtmp
        'find the pointer to the other value we are moving
        j = i
50      'CONTINUE
        j = j + 1
        If (pos(j) <> i) Then GoTo 50
        pos(j) = pos(i)
        pos(i) = i
      End If
    Next i
  End If

End Sub

Public Function PatternMatch(ByVal str As String, ByVal Pattern As String) As Boolean
Attribute PatternMatch.VB_Description = "Searches string for presence of substring."
' ##SUMMARY Searches string for presence of substring.
' ##SUMMARY Example: PatternMatch("He left", "He") = True
' ##SUMMARY Example: PatternMatch("He left", "She") = False
' ##PARAM Str I String to be searched
' ##PARAM Pattern I Substring to be searched for
' ##RETURNS Boolean indicating whether substring was found in contents of string.
' ##REMARKS Special characters: # as any digit, ? as any character, * as zero or more of _
          any character. If pattern does not contain a leading * the pattern must match _
          the beginning of str. If pattern does not contain a trailing * the pattern must _
          match the end of str.
  Dim patCh As String
  Dim strCh As String
  Dim patPos As Long
  Dim strPos As Long
  Dim lenPat As Long
  Dim lenStr As Long
  Dim findPos As Long
  ' ##LOCAL patCh - string character in Pattern
  ' ##LOCAL strCh - string character in Str
  ' ##LOCAL patPos - long position of character in Pattern
  ' ##LOCAL strPos - long position of character in Str
  ' ##LOCAL lenPat - long length of Pattern
  ' ##LOCAL lenStr - long length of Str
  ' ##LOCAL findPos - long position of patCh in Str relative to strPos
  
  lenPat = Len(Pattern)
  lenStr = Len(str)
  strPos = 1
  For patPos = 1 To Len(Pattern)
    strCh = Mid(str, strPos, 1)
    patCh = Mid(Pattern, patPos, 1)
    Select Case patCh
      Case "#"
        If Not IsNumeric(strCh) Then
          GoTo NoMatch
        Else
          strPos = strPos + 1
        End If
      Case "?"
        strPos = strPos + 1
      Case "*"
MatchStar:
        patPos = patPos + 1
        If patPos > lenPat Then 'Trailing * in pattern, match to end of string
          strPos = lenStr + 1
        Else
          patCh = Mid(Pattern, patPos, 1)
          Select Case patCh
            Case "?", "*": GoTo MatchStar 'Skip redundant wild cards
            Case "#"
              While Not IsNumeric(strCh)
                strPos = strPos + 1
                If strPos > lenStr Then GoTo NoMatch
                strCh = Mid(str, strPos, 1)
              Wend
              strPos = strPos + 1
            Case Else
              findPos = InStr(strPos, str, patCh)
              If findPos = 0 Then
                GoTo NoMatch
              Else
                strPos = findPos + 1
              End If
          End Select
        End If
      Case Else
        If strCh <> patCh Then
          GoTo NoMatch
        Else
          strPos = strPos + 1
        End If
    End Select
  Next
  If strPos > Len(str) Then PatternMatch = True
NoMatch:
End Function

Public Function FileExists(ByVal PathName As String, _
                        Optional AcceptDirectories As Boolean = False, _
                        Optional AcceptFiles As Boolean = True) As Boolean
Attribute FileExists.VB_Description = "Checks to see if specified file exists."
' ##SUMMARY Checks to see if specified file exists.
' ##PARAM PathName I Full path and filename.
' ##RETURNS True if file exists.

  On Error GoTo NoSuchFile
  
  If GetAttr(PathName) And vbDirectory Then
    FileExists = AcceptDirectories
  Else
    FileExists = AcceptFiles
  End If
NoSuchFile:
End Function

