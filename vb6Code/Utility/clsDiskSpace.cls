VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsDiskSpace"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
Option Explicit

' Constant used to indicate if we're dealing with NT
Private Const VER_PLATFORM_WIN32_NT As Long = 2

' drive type constants
Private Const DRIVE_CDROM = 5
Private Const DRIVE_FIXED = 3
Private Const DRIVE_RAMDISK = 6
Private Const DRIVE_REMOTE = 4
Private Const DRIVE_REMOVABLE = 2

' Structure needed for GetVersionEx
Private Type OSVERSIONINFO
    dwOSVersionInfoSize As Long
    dwMajorVersion As Long
    dwMinorVersion As Long
    dwBuildNumber As Long
    dwPlatformId As Long
    szCSDVersion(1 To 128) As Byte
End Type

' Holds the working value of Available Space
Private m_lnAvail As Currency
' Holds the working value of Total Space
Private m_lnTotal As Currency
' Holds the working value of Total Free Space
Private m_lnFree As Currency
' Flag to indicate we need to adjust for the
' decimal point in the Currency data type
Private m_bAdjust As Boolean
' Drive type
Private m_lnDriveType As Long

' API declarations
Private Declare Function GetDriveType _
    Lib "kernel32" _
    Alias "GetDriveTypeA" _
    (ByVal nDrive As String) _
    As Long

Private Declare Function GetDiskFreeSpace _
    Lib "kernel32" _
    Alias "GetDiskFreeSpaceA" _
    (ByVal lpRootPathName As String, _
    lpSectorsPerCluster As Long, _
    lpBytesPerSector As Long, _
    lpNumberOfFreeClusters As Long, _
    lpTotalNumberOfClusters As Long) As Long

Private Declare Function GetDiskFreeSpaceEx _
    Lib "kernel32" _
    Alias "GetDiskFreeSpaceExA" _
    (ByVal lpDirectoryName As String, _
    lpFreeBytesAvailableToCaller As Currency, _
    lpTotalNumberOfBytes As Currency, _
    lpTotalNumberOfFreeBytes As Currency) As Long

Private Declare Function GetVersionEx _
    Lib "kernel32" _
    Alias "GetVersionExA" _
    (lpVersionInformation As OSVERSIONINFO) As Long

Private Function ConvertUnits(Space As Variant, Units As String)
  If m_bAdjust Then 'adjust currency kludge for fixed decimal place
    Space = Space * 10000
  End If
  
  Select Case LCase(Units)
    Case "kb": ConvertUnits = Space / (2 ^ 10)
    Case "mb": ConvertUnits = Space / (2 ^ 20)
    Case "gb": ConvertUnits = Space / (2 ^ 30)
    Case Else: ConvertUnits = Space
  End Select
End Function

Friend Property Get AvailableSpace(Optional Units As String = "mb") As Variant
  AvailableSpace = ConvertUnits(m_lnAvail, Units)
End Property

Friend Property Get TotalSpace(Optional Units As String = "mb") As Variant
  TotalSpace = ConvertUnits(m_lnTotal, Units)
End Property

Friend Property Get TotalFreeSpace(Optional Units As String = "mb") As Variant
  TotalFreeSpace = ConvertUnits(m_lnFree, Units)
End Property

Friend Property Get DriveType() As String
  Select Case m_lnDriveType
    Case DRIVE_CDROM: DriveType = "CDROM"
    Case DRIVE_FIXED: DriveType = "FIXED"
    Case DRIVE_RAMDISK: DriveType = "RAMDISK"
    Case DRIVE_REMOTE: DriveType = "REMOTE"
    Case DRIVE_REMOVABLE: DriveType = "REMOVABLE"
    Case Else: DriveType = "<unknown:" & m_lnDriveType & ">"
  End Select
End Property

Friend Function GetFreeSpace(ByVal sPath As String) As Boolean

  m_lnDriveType = GetDriveType(sPath)
  
  If m_lnDriveType = DRIVE_FIXED Then
    If CanUseEx Then   'can use the Ex version of the API
      GetFreeSpace = SpaceEx(sPath)
    Else 'use the old version
      GetFreeSpace = Space(sPath)
    End If
  End If
End Function

' This function is used to determine whether or not we are running
' a version of Windows which supports the GetDiskFreeSpaceEx API.
' If we are, then we should call that in preference to the older
' GetDiskFreeSpace.
Private Function CanUseEx() As Boolean
    Dim nErr As Long
    Dim osinfo As OSVERSIONINFO

' Assume the worst, starting out
    CanUseEx = False

' Initialize the structure needed for GetVersionEx
    osinfo.dwOSVersionInfoSize = LenB(osinfo)

' Get the version information - this returns a BOOL
    nErr = GetVersionEx(osinfo)
    If nErr > 0 Then
    ' The function returned TRUE
        If osinfo.dwPlatformId = VER_PLATFORM_WIN32_NT Then
            If osinfo.dwMajorVersion > 3 Then
            ' NT 4 or higher can use the EX version
                CanUseEx = True
            End If
        Else
            If (osinfo.dwBuildNumber And &HFFFF&) >= 1111 Then
            ' Windows 95 OSR2 or higher can use the EX version
                CanUseEx = True
            End If
        End If
    Else
    ' If we had an error calling the API, find out what it is
        nErr = Err.LastDllError
    End If
            
End Function

Private Function SpaceEx(ByVal sPath As String) As Variant
    Dim nErr As Long

' UNC paths must end with a slash (indicating a directory)
    If (Left(sPath, 2) = "\\") Or (Left(sPath, 2) = "//") Then
        If Right(sPath, 1) <> "\" Then sPath = sPath & "\"
    End If

' Call the API to get the disk space - returns a BOOL. Note that
' we are feeding the return values directly into our local member
' variables - these will be "adjusted" to account for the four
' fixed decimal places inherent in the Currency data type in the
' Property Get routines
    nErr = GetDiskFreeSpaceEx(sPath, m_lnAvail, m_lnTotal, m_lnFree)
    If nErr > 0 Then
    ' Set the flag telling the Property Get routine to adjust the
    ' values to account for the decimal point
        m_bAdjust = True
    Else
    ' We got an error calling the API - find out which one
        nErr = Err.LastDllError
    End If

End Function

Private Function Space(ByVal sPath As String) As Variant
    Dim nSectors As Long
    Dim nBytes As Long
    Dim nClusters As Long
    Dim nFreeClusters As Long
    Dim lnBytesPerCluster As Currency
    Dim nErr As Long

' UNC paths must end with a slash (indicating a directory)
    If (Left(sPath, 2) = "\\") Or (Left(sPath, 2) = "//") Then
        If Right(sPath, 1) <> "\" Then sPath = sPath & "\"
    End If

' Call the API to get the disk space - returns a BOOL
    nErr = GetDiskFreeSpace(sPath, nSectors, nBytes, nClusters, nFreeClusters)
    If nErr > 0 Then
    ' We have to calculate the space values by multiplying
    ' the number of bytes per sector by the number of
    ' sectors per cluster, then multiplying that by the
    ' number of free and total clusters. We are saving
    ' the results in local Currency variables, so we should
    ' be relatively safe from overflow (but not completely
    ' due to the fixed decimal nature of the Currency
    ' data type. We also need to set a flag indicating that
    ' we put the *actual* values in here, so that we don't
    ' attempt to adjust the values in the Property Get
    ' routine (the way we need to for the Ex API call)
        m_bAdjust = False
        lnBytesPerCluster = nSectors & nBytes
        m_lnTotal = lnBytesPerCluster * nClusters
        m_lnFree = lnBytesPerCluster * nFreeClusters
    ' This version of the API doesn't give us any information
    ' concerning the number of bytes available for the current
    ' user - so we set it to zero to indicate that the data
    ' is not available
        m_lnAvail = 0
    Else
    ' We got an error calling the API - find out which one
        nErr = Err.LastDllError
    End If

End Function
