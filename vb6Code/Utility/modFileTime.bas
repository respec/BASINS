Attribute VB_Name = "modFileTime"
Option Explicit

Private Declare Function CreateFile Lib "kernel32" Alias "CreateFileA" (ByVal lpFileName As String, ByVal dwDesiredAccess As Long, ByVal dwShareMode As Long, lpSecurityAttributes As SECURITY_ATTRIBUTES, ByVal dwCreationDisposition As Long, ByVal dwFlagsAndAttributes As Long, ByVal hTemplateFile As Long) As Long
Private Declare Function CloseHandle Lib "kernel32" (ByVal hObject As Long) As Long

Private Declare Function GetFileTime Lib "kernel32" (ByVal hFile As Long, lpCreationTime As FILETIME, lpLastAccessTime As FILETIME, lpLastWriteTime As FILETIME) As Long
Private Declare Function SetFileTime Lib "kernel32" (ByVal hFile As Long, lpCreationTime As FILETIME, lpLastAccessTime As FILETIME, lpLastWriteTime As FILETIME) As Long
Private Declare Function FileTimeToSystemTime Lib "kernel32" (lpFileTime As FILETIME, lpSystemTime As SYSTEMTIME) As Long
Private Declare Function SystemTimeToFileTime Lib "kernel32" (lpSystemTime As SYSTEMTIME, lpFileTime As FILETIME) As Long

Private Declare Function LocalFileTimeToFileTime Lib "kernel32" (lpLocalFileTime As FILETIME, lpFileTime As FILETIME) As Long
Private Declare Function FileTimeToLocalFileTime Lib "kernel32" (lpFileTime As FILETIME, lpLocalFileTime As FILETIME) As Long
'Private Declare Function GetTimeZoneInformation Lib "kernel32" (lpTimeZoneInformation As TIME_ZONE_INFORMATION) As Long

Private Const GENERIC_READ = &H80000000
Private Const GENERIC_WRITE = &H40000000
Private Const FILE_SHARE_READ = &H1
Private Const FILE_SHARE_WRITE = &H2
Private Const OPEN_EXISTING = 3
Private Const INVALID_FILE_HANDLE = -1

Public Type FILETIME
  dwLowDateTime As Long
  dwHighDateTime As Long
End Type

Private Type SYSTEMTIME
  wYear As Integer
  wMonth As Integer
  wDayOfWeek As Integer
  wDay As Integer
  wHour As Integer
  wMinute As Integer
  wSecond As Integer
  wMilliseconds As Integer
End Type

Private Type SECURITY_ATTRIBUTES
  nLength As Long
  lpSecurityDescriptor As Long
  bInheritHandle As Long
End Type

'Private Type TIME_ZONE_INFORMATION
'  Bias As Long
'  StandardName(32) As Integer
'  StandardDate As SYSTEMTIME
'  StandardBias As Long
'  DaylightName(32) As Integer
'  DaylightDate As SYSTEMTIME
'  DaylightBias As Long
'End Type

Public Sub GetFileTimesGMT(filename As String, ByRef mtime As Date, atime As Date, ctime As Date)
  Dim mfTime As FILETIME
  Dim afTime As FILETIME
  Dim cfTime As FILETIME
  Dim sTime As SYSTEMTIME
  Dim hFile As Long
  Dim sa As SECURITY_ATTRIBUTES
  hFile = CreateFile(filename, GENERIC_READ, FILE_SHARE_READ, sa, OPEN_EXISTING, 0, 0)
  If hFile <> INVALID_FILE_HANDLE Then
    GetFileTime hFile, cfTime, afTime, mfTime
    CloseHandle hFile
    
    mtime = FileTimeToDate(mfTime)
    atime = FileTimeToDate(afTime)
    ctime = FileTimeToDate(cfTime)
  End If
End Sub

Public Sub SetFileTimesGMT(filename As String, ByRef mtime As Date, atime As Date, ctime As Date)
  Dim hFile As Long
  Dim sa As SECURITY_ATTRIBUTES
  hFile = CreateFile(filename, GENERIC_WRITE, FILE_SHARE_READ Or FILE_SHARE_WRITE, sa, OPEN_EXISTING, 0, 0)
  If hFile <> INVALID_FILE_HANDLE Then
    SetFileTime hFile, DateToFileTime(ctime), DateToFileTime(atime), DateToFileTime(mtime)
    CloseHandle hFile
  End If
End Sub

Private Function FileTimeToDate(fTime As FILETIME) As Date
  Dim sTime As SYSTEMTIME
  FileTimeToSystemTime fTime, sTime
  FileTimeToDate = DateSerial(sTime.wYear, sTime.wMonth, sTime.wDay) _
                 + TimeSerial(sTime.wHour - 1, sTime.wMinute, sTime.wSecond)
                                        ' - 1 is a mysterious fudge - Daylight savings?
End Function

Private Function DateToFileTime(d As Date) As FILETIME
  Dim retval As FILETIME
  Dim sTime As SYSTEMTIME
  With sTime
    .wYear = Year(d)
    .wMonth = Month(d)
    .wDay = Day(d)
    .wDayOfWeek = Weekday(d) - 1
    .wHour = Hour(d) + 1 ' + 1 is a mysterious fudge - Daylight savings?
    .wMinute = Minute(d)
    .wSecond = Second(d)
  End With
  SystemTimeToFileTime sTime, retval
  DateToFileTime = retval
End Function

Public Function UTCDateToDate(d As Date) As Date
  Dim filename As String
  filename = GetTmpFileName
  SetFileTimesGMT filename, d, d, d
  UTCDateToDate = FileDateTime(filename)
  Kill filename
'  Dim st As SYSTEMTIME
'  Dim ft As FILETIME
'
'  st.wDay = Day(d)
'  st.wHour = Hour(d)
'  st.wMilliseconds = 0
'  st.wMinute = Minute(d)
'  st.wMonth = Month(d)
'  st.wSecond = Second(d)
'  st.wYear = Year(d)
'  SystemTimeToFileTime st, ft
'  FileTimeToLocalFileTime ft, ft
'  FileTimeToSystemTime ft, st
'  UTCDateToDate = DateSerial(st.wYear, st.wMonth, st.wDay) + TimeSerial(st.wHour, st.wMinute, st.wSecond)
End Function

'Public Function DateToUTCDate(d As Date) As Date
'  Dim st As SYSTEMTIME
'  Dim ft As FILETIME
'
'  st.wDay = Day(d)
'  st.wHour = Hour(d)
'  st.wMilliseconds = 0
'  st.wMinute = Minute(d)
'  st.wMonth = Month(d)
'  st.wSecond = Second(d)
'  st.wYear = Year(d)
'  SystemTimeToFileTime st, ft
'  LocalFileTimeToFileTime ft, ft
'  FileTimeToSystemTime ft, st
'  DateToUTCDate = DateSerial(st.wYear, st.wMonth, st.wDay) + TimeSerial(st.wHour, st.wMinute, st.wSecond)
'End Function

