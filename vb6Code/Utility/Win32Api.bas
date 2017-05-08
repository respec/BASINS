Attribute VB_Name = "modWin32Api"
Option Explicit
'Copyright 2003 by AQUA TERRA Consultants

' ##MODULE_NAME modWin32Api
' ##MODULE_DATE January 1, 2003
' ##MODULE_AUTHOR Mark Gray and Jack Kittle of AQUA TERRA CONSULTANTS
' ##MODULE_SUMMARY Declarations for miscellaneous Win32 API functions.
'

Private pWindowsSysDir As String
Private pWindowsDir As String
Private pLenWinSys As Long
Private pLenWin As Long

' <><><><><><>< Global Constants Section ><><><><><><><>

'Buffer size for strings
Public Const gintMAX_SIZE As Integer = 255

'  Help engine declarations: commands to pass WinHelp()
Global Const HELP_CONTEXT = &H1       ' Display topic identified by number in Data
Global Const HELP_QUIT = &H2          ' Terminate help
Global Const HELP_INDEX = &H3         ' Display index
Global Const HELP_HELPONHELP = &H4    ' Display help on using help
Global Const HELP_SETINDEX = &H5      ' Set an alternate Index for help file with more than one index
Global Const HELP_FINDER = &HB        ' Find help file
Global Const HELP_KEY = &H101         ' Display topic for keyword in Data
Global Const HELP_MULTIKEY = &H201    ' Lookup keyword in alternate table and display topic
Global Const LB_GETITEMHEIGHT = &H1A1 '
Global Const LB_GETTOPINDEX = &H1AE   '
'
Public Const WH_KEYBOARD = 2              '
Public Const KB_RELEASE_MASK = &H80000000 '
Public Const WM_CLOSE = &H10              '
Public Const LF_FACESIZE = 32             'Logical Font
Public Const FO_MOVE = &H1                '
Public Const FO_RENAME = &H4              '
Public Const FOF_SILENT = &H4             '
Public Const FOF_NOCONFIRMATION = &H10    '
Public Const FOF_FILESONLY = &H80         '
Public Const FOF_SIMPLEPROGRESS = &H100   '
Public Const FOF_NOCONFIRMMKDIR = &H200   '
Public Const SHARD_PATH = &H2&            '
Public Const GWL_STYLE = (-16)            '
Public Const GWL_EXSTYLE = (-20)          '
Public Const GWL_WNDPROC = (-4)           '
Public Const OFN_FILEMUSTEXIST = &H1000   '
Public Const OFN_HIDEREADONLY = &H4       '
Public Const WM_CONTEXTMENU = &H7B        '
Public Const WM_HELP = &H53               '
Public Const WM_NCDESTROY = &H82          '
Public Const WM_NOTIFY = &H4E             '
Public Const WM_TCARD = &H52              '
Public Const WS_CHILD = &H40000000        '
Public Const WS_VISIBLE = &H10000000      '
Public Const WS_EX_CONTEXTHELP = &H400    '
Public Const WS_EX_LAYERED = &H80000
Public Const STARTF_USESTDHANDLES = &H100 '
Public Const STD_INPUT_HANDLE = -10&      '
Public Const STD_OUTPUT_HANDLE = -11&     '
Public Const STD_ERROR_HANDLE = -12&      '
Public Const INFINITE = &HFFFFFFFF        'Infinite timeout
Public Const NORMAL_PRIORITY_CLASS = &H20 '
Public Const WAIT_TIMEOUT = &H102&        '
Public Const HELP_FORCEFILE = &H9&        '

'Hatch Styles
Public Const HS_HORIZONTAL = 0              '  -----
Public Const HS_VERTICAL = 1                '  |||||
Public Const HS_FDIAGONAL = 2               '  \\\\\
Public Const HS_BDIAGONAL = 3               '  /////
Public Const HS_CROSS = 4                   '  +++++
Public Const HS_DIAGCROSS = 5               '  xxxxx
Public Const HS_FDIAGONAL1 = 6
Public Const HS_BDIAGONAL1 = 7
Public Const HS_SOLID = 8
Public Const HS_DENSE1 = 9
Public Const HS_DENSE2 = 10
Public Const HS_DENSE3 = 11
Public Const HS_DENSE4 = 12
Public Const HS_DENSE5 = 13
Public Const HS_DENSE6 = 14
Public Const HS_DENSE7 = 15
Public Const HS_DENSE8 = 16
Public Const HS_NOSHADE = 17
Public Const HS_HALFTONE = 18
Public Const HS_SOLIDCLR = 19
Public Const HS_DITHEREDCLR = 20
Public Const HS_SOLIDTEXTCLR = 21
Public Const HS_DITHEREDTEXTCLR = 22
Public Const HS_SOLIDBKCLR = 23
Public Const HS_DITHEREDBKCLR = 24
Public Const HS_API_MAX = 25

'Pen Style structure and constants
Type LOGBRUSH
  lbStyle As Long
  lbColor As Long
  lbHatch As Long
End Type
Public Const PS_COSMETIC = &H0
Public Const PS_SOLID = 0
Public Const PS_GEOMETRIC = &H10000
Public Const PS_DASH = 1                    '  -------
Public Const PS_DOT = 2                     '  .......
Public Const PS_DASHDOT = 3                 '  _._._._
Public Const PS_DASHDOTDOT = 4              '  _.._.._
Public Const PS_ALTERNATE = 8
Public Const PS_JOIN_ROUND = &H0
Public Const PS_JOIN_BEVEL = &H1000
Public Const PS_JOIN_MITER = &H2000
Public Const PS_JOIN_MASK = &HF000
Public Const PS_ENDCAP_ROUND = &H0
Public Const PS_ENDCAP_SQUARE = &H100
Public Const PS_ENDCAP_FLAT = &H200
Public Const PS_ENDCAP_MASK = &HF00

'BitBlt raster operations
Public Const SRCCOPY = &HCC0020     ' (DWORD) dest = source
Public Const SRCPAINT = &HEE0086    ' (DWORD) dest = source OR dest
Public Const SRCAND = &H8800C6      ' (DWORD) dest = source AND dest
Public Const SRCINVERT = &H660046   ' (DWORD) dest = source XOR dest
Public Const SRCERASE = &H440328    ' (DWORD) dest = source AND (NOT dest )
Public Const NOTSRCCOPY = &H330008  ' (DWORD) dest = (NOT source)
Public Const NOTSRCERASE = &H1100A6 ' (DWORD) dest = (NOT src) AND (NOT dest)
Public Const MERGECOPY = &HC000CA   ' (DWORD) dest = (source AND pattern)
Public Const MERGEPAINT = &HBB0226  ' (DWORD) dest = (NOT source) OR dest
Public Const PATCOPY = &HF00021     ' (DWORD) dest = pattern
Public Const PATPAINT = &HFB0A09    ' (DWORD) dest = DPSnoo
Public Const PATINVERT = &H5A0049   ' (DWORD) dest = pattern XOR dest
Public Const DSTINVERT = &H550009   ' (DWORD) dest = (NOT dest)
Public Const BLACKNESS = &H42&      ' (DWORD) dest = BLACK
Public Const WHITENESS = &HFF0062   ' (DWORD) dest = WHITE

Public Enum SpecialFolderType
'Windows desktop virtual folder at the root of the name space
  CSIDL_DESKTOP = &H0
'File system directory that contains the
'user's program groups (which are also file system directories)
  CSIDL_PROGRAMS = &H2
'Control Panel - virtual folder containing icons for the control panel applications
  CSIDL_CONTROLS = &H3
'Printers folder - virtual folder containing installed printers.
  CSIDL_PRINTERS = &H4
'File system directory that serves as a common repository for documents (Documents folder)
  CSIDL_PERSONAL = &H5
'File system directory that contains the
'user's favorite Internet Explorer URLs
  CSIDL_FAVORITES = &H6
'File system directory that corresponds to the user's Startup program group
  CSIDL_STARTUP = &H7
'File system directory that contains the user's most recently used documents (Recent folder)
  CSIDL_RECENT = &H8
'File system directory that contains Send To menu items
  CSIDL_SENDTO = &H9
'Recycle bin file system directory containing file
'objects in the user's recycle bin. The location of
'this directory is not in the registry; it is marked
'with the hidden and system attributes to prevent the
'user from moving or deleting it.
  CSIDL_BITBUCKET = &HA
'File system directory containing Start menu items
  CSIDL_STARTMENU = &HB
'File system directory used to physically store
'file objects on the desktop (not to be confused
'with the desktop folder itself).
  CSIDL_DESKTOPDIRECTORY = &H10
'My Computer - virtual folder containing everything
'on the local computer: storage devices, printers,
'and Control Panel. The folder may also contain
'mapped network drives.
  CSIDL_DRIVES = &H11
'Network Neighborhood - virtual folder representing
'the top level of the network hierarchy
  CSIDL_NETWORK = &H12
'File system directory containing objects that
'appear in the network neighborhood
  CSIDL_NETHOOD = &H13
'Virtual folder containing fonts
  CSIDL_FONTS = &H14
'File system directory that serves as a
'common repository for document templates
'(ShellNew folder.)
  CSIDL_TEMPLATES = &H15
End Enum

'ShowWindow() Commands
Public Enum ShowWindowCommand
  SW_HIDE = 0
  SW_SHOWNORMAL = 1
  SW_NORMAL = 1
  SW_SHOWMINIMIZED = 2
  SW_SHOWMAXIMIZED = 3
  SW_MAXIMIZE = 3
  SW_SHOWNOACTIVATE = 4
  SW_SHOW = 5
  SW_MINIMIZE = 6
  SW_SHOWMINNOACTIVE = 7
  SW_SHOWNA = 8
  SW_RESTORE = 9
  SW_SHOWDEFAULT = 10
  SW_MAX = 10
End Enum

Public Type SHFILEOPSTRUCT
  hwnd        As Long
  wFunc       As Long
  pFrom       As String
  pTo         As String
  fFlags      As Integer
  fAborted    As Boolean
  hNameMaps   As Long
  sProgress   As String
End Type

Public Type POINTAPI
  x As Long
  y As Long
End Type

Public Type HELPINFO
  cbSize As Long
  iContextType As Long
  iCtrlId As Long
  hItemHandle As Long
  dwContextId As Long
  MousePos As POINTAPI
End Type

Public Type NMHDR
  hwndFrom As Long
  idfrom As Long
  code As Long
End Type

Public Type OPENFILENAME
  lStructSize As Long
  hwndOwner As Long
  hInstance As Long
  lpstrFilter As String
  lpstrCustomFilter As String
  nMaxCustFilter As Long
  nFilterIndex As Long
  lpstrFile As String
  nMaxFile As Long
  lpstrFileTitle As String
  nMaxFileTitle As Long
  lpstrInitialDir As String
  lpstrTitle As String
  flags As Long
  nFileOffset As Integer
  nFileExtension As Integer
  lpstrDefExt As String
  lCustData As Long
  lpfnHook As Long
  lpTemplateName As String
End Type

Public Type RECT
  Left As Long
  Top As Long
  Right As Long
  Bottom As Long
End Type

Public Type PROCESS_INFORMATION
  hProcess As Long
  hThread As Long
  dwProcessId As Long
  dwThreadId As Long
End Type

Public Type STARTUPINFO
  cb As Long
  lpReserved As String
  lpDesktop As String
  lpTitle As String
  dwX As Long
  dwY As Long
  dwXSize As Long
  dwYSize As Long
  dwXCountChars As Long
  dwYCountChars As Long
  dwFillAttribute As Long
  dwFlags As Long
  wShowWindow As Integer
  cbReserved2 As Integer
  lpReserved2 As Long
  hStdInput As Long
  hStdOutput As Long
  hStdError As Long
End Type

Public Type SECURITY_ATTRIBUTES
  nLength As Long
  lpSecurityDescriptor As Long
  bInheritHandle As Long
End Type

Public Type LOGFONT
  lfHeight As Long
  lfWidth As Long
  lfEscapement As Long
  lfOrientation As Long
  lfWeight As Long
  lfItalic As Byte
  lfUnderline As Byte
  lfStrikeOut As Byte
  lfCharSet As Byte
  lfOutPrecision As Byte
  lfClipPrecision As Byte
  lfQuality As Byte
  lfPitchAndFamily As Byte
  lfFaceName(LF_FACESIZE) As Byte
End Type

Public Type MULTIKEYHELP
  mkSize As Integer
  mkKeylist As String * 1
  szKeyphrase As String * 253
End Type

Private Type OSVERSIONINFO 'for GetVersionEx API call
    dwOSVersionInfoSize As Long
    dwMajorVersion As Long
    dwMinorVersion As Long
    dwBuildNumber As Long
    dwPlatformId As Long
    szCSDVersion As String * 128
End Type

Private Type VERINFO                                            'Version FIXEDFILEINFO
    'There is data in the following two dwords, but it is for Windows internal
    '   use and we should ignore it
    Ignore(1 To 8) As Byte
    'Signature As Long
    'StrucVersion As Long
    FileVerPart2 As Integer
    FileVerPart1 As Integer
    FileVerPart4 As Integer
    FileVerPart3 As Integer
    ProductVerPart2 As Integer
    ProductVerPart1 As Integer
    ProductVerPart4 As Integer
    ProductVerPart3 As Integer
    FileFlagsMask As Long 'VersionFileFlags
    FileFlags As Long 'VersionFileFlags
    FileOS As Long 'VersionOperatingSystemTypes
    FileType As Long
    FileSubtype As Long 'VersionFileSubTypes
    'I've never seen any data in the following two dwords, so I'll ignore them
    Ignored(1 To 8) As Byte 'DateHighPart As Long, DateLowPart As Long
End Type

Private Type ITEMIDLIST
  mkid As Long
End Type

Declare Function WinHelp Lib "user32" Alias "WinHelpA" _
    (ByVal hwnd As Long, ByVal lpHelpFile As String, ByVal wCommand As Long, ByVal dwData As Any) As Long
  '##SUMMARY Starts Windows Help (WINHELP.EXE) and passes additional _
      data indicating the nature of the help requested by the application.
  '##PARAM hWndMain I Identifies the window requesting Help. The WinHelp function _
      uses this handle to keep track of which applications have requested Help. _
      If the uCommand parameter specifies HELP_CONTEXTMENU or HELP_WM_HELP, hWndMain _
      identifies the control requesting Help.
  '##PARAM lpszHelp I Address of a null-terminated string containing the path, if _
      necessary, and the name of the help file that WinHelp is to display.
  '##PARAM uCommand I Specifies the type of help requested.
  '##PARAM dwData I Specifies additional data. The value used depends on the value _
      of the uCommand parameter.
  '##RETURNS If the function succeeds, the return value is nonzero. _
    If the function fails, the return value is zero.

Declare Function ClientToScreen Lib "user32" (ByVal hwnd As Long, lpPoint As POINTAPI) As Long
Declare Function SetCursorPos Lib "user32" (ByVal x As Long, ByVal y As Long) As Long

Declare Function GetPixel Lib "gdi32" (ByVal hdc As Long, ByVal x As Long, ByVal y As Long) As Long
Declare Function SetPixel Lib "gdi32" (ByVal hdc As Long, ByVal x As Long, ByVal y As Long, ByVal crColor As Long) As Long
Declare Function Polyline Lib "gdi32" (ByVal hdc As Long, lpPoint As POINTAPI, ByVal nCount As Long) As Long
Declare Function LineTo Lib "gdi32" (ByVal hdc As Long, ByVal x As Long, ByVal y As Long) As Long
Declare Function MoveToEx Lib "gdi32" (ByVal hdc As Long, ByVal x As Long, ByVal y As Long, lpPoint As Long) As Long 'lpPoint As POINTAPI
Declare Function CreateHatchBrush Lib "gdi32" (ByVal nIndex As Long, ByVal crColor As Long) As Long
Declare Function CreatePatternBrush Lib "gdi32" (ByVal hBitmap As Long) As Long
Declare Function CreateRectRgn Lib "gdi32" (ByVal X1 As Long, ByVal Y1 As Long, ByVal X2 As Long, ByVal Y2 As Long) As Long
Declare Function SelectClipRgn Lib "gdi32" (ByVal hdc As Long, ByVal hRgn As Long) As Long
Declare Function SaveDC Lib "gdi32" (ByVal hdc As Long) As Long
Declare Function RestoreDC Lib "gdi32" (ByVal hdc As Long, ByVal nSavedDC As Long) As Long
Declare Function ExtCreatePen Lib "gdi32" (ByVal dwPenStyle As Long, ByVal dwWidth As Long, lplb As LOGBRUSH, ByVal dwStyleCount As Long, lpStyle As Long) As Long

Declare Function CreateCompatibleBitmap Lib "gdi32" (ByVal hdc As Long, ByVal nWidth As Long, ByVal nHeight As Long) As Long
Declare Function CreateCompatibleDC Lib "gdi32" (ByVal hdc As Long) As Long
Declare Function BitBlt Lib "gdi32" (ByVal hDestDC As Long, ByVal x As Long, ByVal y As Long, ByVal nWidth As Long, ByVal nHeight As Long, ByVal hSrcDC As Long, ByVal xSrc As Long, ByVal ySrc As Long, ByVal dwRop As Long) As Long
Declare Function StretchBlt Lib "gdi32" (ByVal hdc As Long, ByVal x As Long, ByVal y As Long, ByVal nWidth As Long, ByVal nHeight As Long, ByVal hSrcDC As Long, ByVal xSrc As Long, ByVal ySrc As Long, ByVal nSrcWidth As Long, ByVal nSrcHeight As Long, ByVal dwRop As Long) As Long
Declare Function PatBlt Lib "gdi32" (ByVal hdc As Long, ByVal x As Long, ByVal y As Long, ByVal nWidth As Long, ByVal nHeight As Long, ByVal dwRop As Long) As Long
'Declare Function MaskBlt Lib "gdi32" (ByVal hdcDest As Long, ByVal nXDest As Long, ByVal nYDest As Long, ByVal nWidth As Long, ByVal nHeight As Long, ByVal hdcSrc As Long, ByVal nXSrc As Long, ByVal nYSrc As Long, ByVal hbmMask As Long, ByVal xMask As Long, ByVal yMask As Long, ByVal dwRop As Long) As Long
Declare Function CreateBitmap Lib "gdi32" (ByVal nWidth As Long, ByVal nHeight As Long, ByVal nPlanes As Long, ByVal nBitCount As Long, lpBits As Any) As Long
Declare Function SetBkColor Lib "gdi32" (ByVal hdc As Long, ByVal crColor As Long) As Long

Declare Function GetLogicalDriveStrings Lib "kernel32" Alias "GetLogicalDriveStringsA" (ByVal nBufferLength As Long, ByVal lpBuffer As String) As Long
Declare Function GetDriveType Lib "kernel32" Alias "GetDriveTypeA" (ByVal nDrive As String) As Long
' drive type constants
Public Const DRIVE_CDROM = 5
Public Const DRIVE_FIXED = 3
Public Const DRIVE_RAMDISK = 6
Public Const DRIVE_REMOTE = 4
Public Const DRIVE_REMOVABLE = 2

Private Declare Function SetWindowPos Lib "user32" (ByVal hwnd As Long, _
   ByVal hWndInsertAfter As Long, _
   ByVal x As Long, _
   ByVal y As Long, _
   ByVal cx As Long, _
   ByVal cy As Long, _
   ByVal wFlags As Long) As Long

Private Declare Function RedrawWindow Lib "user32" (ByVal hwnd As Long, lprcUpdate As RECT, ByVal hrgnUpdate As Long, ByVal fuRedraw As Long) As Long
Private Const RDW_ERASE = &H4
Private Const RDW_INVALIDATE = &H1
Private Const RDW_ALLCHILDREN = &H80
Private Const RDW_FRAME = &H400

Private Declare Function SetLayeredWindowAttributes Lib "user32" _
  (ByVal hwnd As Long, _
   ByVal crKey As Long, _
   ByVal bAlpha As Byte, _
   ByVal dwFlags As Long) As Long

Private Declare Function FindExecutable Lib "shell32.dll" Alias _
  "FindExecutableA" _
  (ByVal lpFile As String, _
   ByVal lpDirectory As String, _
   ByVal lpResult As String) As Long

Public Declare Function TextOut Lib "gdi32" Alias "TextOutA" _
   (ByVal hdc As Long, ByVal x As Long, ByVal y As Long, _
   ByVal lpString As String, ByVal nCount As Long) As Long

'For LibraryExists
Private Declare Function LoadLibrary Lib "Kernel" (ByVal f$) As Integer
Private Declare Sub FreeLibrary Lib "Kernel" (ByVal h As Integer)
Private Declare Function SetErrorMode Lib "Kernel" (ByVal wMode As Integer) As Integer

Private Declare Function GetFileVersionInfoSize Lib "version.dll" Alias "GetFileVersionInfoSizeA" (ByVal sFile As String, lpLen As Long) As Long
Private Declare Function GetFileVersionInfo Lib "version.dll" Alias "GetFileVersionInfoA" (ByVal sFile As String, ByVal lpIgnored As Long, ByVal lpSize As Long, ByVal lpBuf As Long) As Long
Private Declare Function VerQueryValue Lib "version.dll" Alias "VerQueryValueA" (ByVal lpBuf As Long, ByVal szReceive As String, lpBufPtr As Long, lLen As Long) As Long

Private Declare Function GetWindowsDirectory Lib "kernel32" Alias "GetWindowsDirectoryA" (ByVal lpBuffer As String, ByVal nSize As Long) As Long
Private Declare Function GetSystemDirectory Lib "kernel32" Alias "GetSystemDirectoryA" (ByVal lpBuffer As String, ByVal nSize As Long) As Long
Private Declare Function GetVersionEx Lib "kernel32" Alias "GetVersionExA" (lpVersionInformation As OSVERSIONINFO) As Long


Declare Function GetLastError Lib "kernel32" () As Long
  '##SUMMARY The calling thread’s last-error code value. _
      The last-error code is maintained on a per-thread basis. _
      Multiple threads do not overwrite each other’s last-error code.
  '##RETURNS The calling thread’s last-error code value. _
      Functions set this value by calling the SetLastError function. _
      The Return Value section of each reference page notes the conditions _
      under which the function sets the last-error code.
Declare Function GetModuleHandle Lib "kernel32" Alias "GetModuleHandleA" _
    (ByVal lpModuleName As String) As Long
  '##SUMMARY Identifies the ID of the module that is actually executing.
  '##PARAM lpModuleName I Specifies the file name of the module to load.
  '##RETURNS The handle of an already loaded DLL.
Declare Function GetModuleFileName Lib "kernel32" Alias "GetModuleFileNameA" _
    (ByVal hModule As Long, ByVal lpFileName As String, ByVal nSize As Long) As Long
  '##SUMMARY Retrieves the full path and filename for the executable file containing _
      the specified module.
  '##PARAM hModule I Identifies the module whose executable filename is being requested. _
      If this parameter is NULL, GetModuleFileName returns the path for the file used to _
      create the calling process.
  '##PARAM lpFileName O Points to a buffer that is filled in with the path and filename _
      of the given module.
  '##PARAM nSize I Specifies the length, in characters, of the lpFilename buffer. If the _
      length of the path and filename exceeds this limit, the string is truncated.
  '##RETURNS If the function succeeds, the return value is the length, in characters, _
      of the string copied to the buffer. If the function fails, the return value is zero.
Declare Function FloodFill Lib "gdi32" _
    (ByVal hdc As Long, ByVal x As Long, ByVal y As Long, ByVal crColor As Long) As Long
  '##SUMMARY Fills an area of the display surface with the current brush. _
      The area is assumed to be bounded as specified by the crFill parameter.
  '##PARAM hDC I Identifies a device context.
  '##PARAM y I Specifies the logical x-coordinate of the point where filling is to begin.
  '##PARAM y I Specifies the logical y-coordinate of the point where filling is to begin.
  '##PARAM crColor I Specifies the color of the boundary or of the area to be filled.
  '##RETURNS If the function succeeds, the return value is nonzero. _
    If the function fails, the return value is zero.
Declare Function CreateFontIndirect Lib "gdi32" Alias "CreateFontIndirectA" _
    (lpLogFont As LOGFONT) As Long
  '##SUMMARY Creates a logical font that has the characteristics specified in the _
      specified structure. _
      The font can subsequently be selected as the current font for any device context.
  '##PARAM lpLogFont I Points to a LOGFONT structure that defines the characteristics of _
      the logical font.
  '##RETURNS If the function succeeds, the return value is a handle to a logical font. _
    If the function fails, the return value is NULL.
Declare Function Polygon Lib "gdi32" _
    (ByVal hdc As Long, lpPoint As POINTAPI, ByVal nCount As Long) As Long
  '##SUMMARY Draws a polygon consisting of two or more vertices connected by straight lines. _
      The polygon is outlined by using the current pen and filled by using the current brush _
      and polygon fill mode.
  '##PARAM hdc I Identifies the device context.
  '##PARAM lpPoint I Points to an array of POINT structures that specify the vertices of _
      the polygon.
  '##PARAM nCount I Specifies the number of vertices in the array. _
      This value must be greater than or equal to 2.
  '##RETURNS If the function succeeds, the return value is nonzero. _
    If the function fails, the return value is zero.
Declare Function CreateSolidBrush Lib "gdi32" _
    (ByVal crColor As Long) As Long
  '##SUMMARY Creates a logical brush that has the specified solid color.
  '##PARAM crColor I Specifies the color of the brush.
  '##RETURNS If the function succeeds, the return value identifies a logical brush. _
    If the function fails, the return value is NULL.
Declare Function SetPolyFillMode Lib "gdi32" _
    (ByVal hdc As Long, ByVal nPolyFillMode As Long) As Long
  '##SUMMARY Sets the polygon fill mode for functions that fill polygons.
  '##PARAM hdc I Identifies the device context.
  '##PARAM nPolyFillMode I Specifies the new fill mode. This parameter can be either _
      of the following values: _
    <UL> _
    <li>alternate - Selects alternate mode (fills the area between odd-numbered _
        and even-numbered polygon sides on each scan line) _
    <li>winding - Selects winding mode (fills any region with a nonzero winding value) _
    </UL>
  '##RETURNS The previous filling mode. _
      If an error occurs, the return value is zero.
Declare Function GetPolyFillMode Lib "gdi32" _
    (ByVal hdc As Long) As Long
  '##SUMMARY Retrieves the current polygon fill mode.
  '##PARAM hdc I Identifies the device context.
  '##RETURNS If the function succeeds, the return value specifies the polygon fill mode, _
      which can be either of the following values: _
    <UL> _
    <li>alternate - Selects alternate mode (fills the area between odd-numbered _
        and even-numbered polygon sides on each scan line) _
    <li>winding - Selects winding mode (fills any region with a nonzero winding value) _
    </UL> _
    If an error occurs, the return value is zero.
Declare Function DeleteObject Lib "gdi32" _
    (ByVal hObject As Long) As Long
  '##SUMMARY Deletes a logical pen, brush, font, bitmap, _
    region, or palette, freeing all system resources associated with the _
    object. After the object is deleted, the specified handle is no longer valid.
  '##PARAM hObject I Identifies a logical pen, brush, font, bitmap, region, or palette.
  '##RETURNS If the function succeeds, the return value is nonzero. _
    If the specified handle is not valid or is currently selected into a device _
    context, the return value is zero.
Declare Function SelectObject% Lib "gdi32" _
    (ByVal hdc As Long, ByVal hObject As Long)
  '##SUMMARY Selects an object into the specified device context. _
    The new object replaces the previous object of the same type.
  '##PARAM hdc I Identifies the device context.
  '##PARAM hObject I Identifies the object to be selected.
  '##RETURNS If the selected object is not a region and the function succeeds, the return _
    value is the handle of the object being replaced. If the selected object is a region _
    and the function succeeds, the return value is one of the following 3 values: _
    <OL> _
    <li>SIMPLEREGION - Region consists of a single rectangle. _
    <li>COMPLEXREGION - Region consists of more than one rectangle. _
    <li>NULLREGION - Region is empty. _
    </OL>
Declare Function GetDeviceCaps Lib "gdi32" _
    (ByVal hdc As Long, ByVal nIndex As Long) As Long
  '##SUMMARY Retrieves device-specific information about a specified device.
  '##PARAM hDC I Identifies the device context.
  '##PARAM nIndex I Specifies the item to return. _
      This parameter can be one of the following values:
  '##RETURNS Value of the desired item.
Declare Function CallWindowProc Lib "user32" Alias "CallWindowProcA" _
    (ByVal lpPrevWndFunc As Long, ByVal hwnd As Long, ByVal msg As Long, ByVal wParam As Long, lParam As Any) As Long
  '##SUMMARY Passes message information to the specified window procedure.
  '##PARAM lpPrevWndFunc I Pointer to the previous window procedure.
  '##PARAM hwnd M Identifies the window procedure to receive the message.
  '##PARAM msg I Specifies the message.
  '##PARAM wParam I Specifies additional message-specific information; _
      depends on the value of the Msg parameter.
  '##PARAM lParam I Specifies additional message-specific information; _
      depends on the value of the Msg parameter.
  '##RETURNS The result of the message processing; depends on the message sent.
Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" _
    (Dest As Any, Source As Any, ByVal nBytes As Long)
  '##SUMMARY Copies a block of memory from one location to another.
  '##PARAM Dest I Points to starting address of copied block’s destination.
  '##PARAM Source I Points to starting address of the block of memory to copy.
  '##PARAM nBytes I Specifies the size, in bytes, of block of memory to copy.
  '##RETURNS No return value.
Declare Function CopyRect Lib "user32" _
    (lpDestRect As RECT, lpSourceRect As RECT) As Long
  '##SUMMARY Copies the coordinates of one rectangle to another.
  '##PARAM lpDestRect M Points to the RECT structure that will receive the _
      logical coordinates of the source rectangle.
  '##PARAM lpSourceRect I Points to the RECT structure whose coordinates are to be copied.
  '##RETURNS If the function succeeds, the return value is nonzero. _
    If the function fails, the return value is zero.
Declare Function DestroyWindow Lib "user32" _
    (ByVal hwnd As Long) As Long
  '##SUMMARY Destroys the specified window. _
      The function sends WM_DESTROY and WM_NCDESTROY messages to the window _
      to deactivate it and remove the keyboard focus from it. _
      The function also destroys the window’s menu, flushes the thread message _
      queue, destroys timers, removes clipboard ownership, and breaks the _
      clipboard viewer chain (if the window is at the top of the viewer chain).
  '##PARAM hWnd I Identifies the window to be destroyed.
  '##RETURNS If the function succeeds, the return value is nonzero. _
    If the function fails, the return value is zero.
Declare Function GetCursorPos Lib "user32" _
    (lpPoint As POINTAPI) As Long
  '##SUMMARY Retrieves the cursor’s position, in screen coordinates.
  '##PARAM lpPoint M Points to a POINT structure that receives the _
      screen coordinates of the cursor.
  '##RETURNS If the function succeeds, the return value is nonzero. _
    If the function fails, the return value is zero.
Declare Function GetDlgCtrlID Lib "user32" _
    (ByVal hwnd As Long) As Long
  '##SUMMARY Retrieves the ID of a control given its handle.
  '##PARAM hWnd I Identifies the window whose device context is to be retrieved.
  '##RETURNS If the function succeeds, the return value identifies the control ID. _
    If the function fails, the return value is NULL.
Declare Function GetFileTitle Lib "comdlg32.dll" Alias "GetFileTitleA" _
    (ByVal lpszFile As String, ByVal lpszTitle As String, ByVal cbBuf As Integer) As Integer
  '##SUMMARY The GetFileTitle function returns the name of the file _
    identified by the lpszFile parameter.
  '##PARAM lpszFile I Pointer to the name and location of a file.
  '##PARAM lpszTitle I Pointer to a buffer into which the function is _
    to copy the name of the file.
  '##PARAM cbBuf I Specifies the length, in characters, of the buffer _
    pointed to by the lpszTitle parameter.
  '##RETURNS If the function succeeds, the return value is zero. _
    If the filename is invalid, the return value is a negative number. _
    If the buffer pointed to by the lpszTitle parameter is too small, _
    the return value is a positive integer that specifies the required _
    buffer size, in bytes (ANSI version) or characters (Unicode version). _
    The required buffer size includes the terminating null character.
Declare Function GetOpenFileName Lib "comdlg32.dll" Alias "GetOpenFileNameA" _
    (pOpenfilename As OPENFILENAME) As Long
  '##SUMMARY Creates an Open common dialog box that lets the _
    user specify the drive, directory, and the name of a file or set of files to open.
  '##PARAM pOpenfilename I Pointer to an OPENFILENAME structure that contains _
    information used to initialize the dialog box. When GetOpenFileName returns, _
    this structure contains information about the user’s file selection.
  '##RETURNS If the user specifies a filename and clicks the OK button, _
    the return value is nonzero. The buffer pointed to by the lpstrFile _
    member of the OPENFILENAME structure contains the full path and _
    filename specified by the user. If the user cancels or closes the _
    Open dialog box or an error occurs, the return value is zero.
Declare Function GetWindowLong Lib "user32" Alias "GetWindowLongA" _
    (ByVal hwnd As Long, ByVal nIndex As Long) As Long
  '##SUMMARY Retrieves information about the specified window. _
    The function also retrieves the 32-bit (long) value at the specified offset into the _
    extra window memory of a window.
  '##PARAM hWnd I Identifies the window and, indirectly, the class to which the window belongs.
  '##PARAM nIndex I Specifies the zero-based offset to the value to be retrieved. Valid values are in the range zero through the number of bytes of extra window memory, minus four; for example, if you specified 12 or more bytes of extra memory, a value of 8 would be an index to the third 32-bit integer.
  '##RETURNS If the function succeeds, the return value is the requested 32-bit value. _
    If the function fails, the return value is zero.
Declare Function IsBadStringPtrp Lib "kernel32" Alias "IsBadStringPtrA" _
    (ByVal lpsz As Long, ByVal ucchMax As Long) As Long
  '##SUMMARY Verifies that the calling process has read access to a range of _
    memory pointed to by a string pointer.
  '##PARAM lpsz I Pointer to a null-terminated string, either Unicode or ASCII.
  '##PARAM ucchMax I Specifies the maximum size, in TCHARs, of the string. _
    The function checks for read access in all bytes up to the string's _
    terminating null character or up to the number of bytes specified by this _
    parameter, whichever is smaller. If this parameter is zero, the return value is zero.
  '##RETURNS If the calling process has read access to all characters up to the _
    string's terminating null character or up to the number of characters specified _
    by ucchMax, the return value is zero. If the calling process does not have read _
    access to all characters up to the string's terminating null character or up to _
    the number of characters specified by ucchMax, the return value is nonzero. _
    If the application is compiled as a debugging version, and the process does not _
    have read access to the entire memory range specified, the function causes an _
    assertion and breaks into the debugger. Leaving the debugger, the function _
    continues as usual, and returns a nonzero value This behavior is by design, _
    as a debugging aid.
Declare Function lstrlenp Lib "kernel32" Alias "lstrlenA" _
    (ByVal lpString As Long) As Long
  '##SUMMARY Retrieves the length in bytes (ANSI version) or characters (Unicode _
    version) of the specified string (not including the terminating null character).
  '##PARAM lpString I Points to a null-terminated string.
  '##RETURNS If the function succeeds, the return value specifies the length of _
    the string in bytes (ANSI version) or characters (Unicode version).
Declare Function MoveWindow Lib "user32" _
    (ByVal hwnd As Long, ByVal x As Long, ByVal y As Long, ByVal nWidth As Long, ByVal nHeight As Long, ByVal bRepaint As Long) As Long
  '##SUMMARY Changes the position and dimensions of the specified window. For a top-level window, the position and dimensions are relative to the upper-left corner of the screen. For a child window, they are relative to the upper-left corner of the parent window’s client area.
  '##PARAM hWnd I Identifies the window.
  '##PARAM x I Specifies the new position of the left side of the window.
  '##PARAM y I Specifies the new position of the top of the window.
  '##PARAM nWidth I Specifies the new width of the window.
  '##PARAM nHeight I Specifies the new height of the window.
  '##PARAM bRepaint M Specifies whether the window is to be repainted. _
    If this parameter is TRUE, the window receives a WM_PAINT message. _
    If the parameter is FALSE, no repainting of any kind occurs. _
    This applies to the client area, the nonclient area (including the _
    title bar and scroll bars), and any part of the parent window uncovered _
    as a result of moving a child window. If this parameter is FALSE, the _
    application must explicitly invalidate or redraw any parts of the window _
    and parent window that need redrawing.
  '##RETURNS If the function succeeds, the return value is nonzero. _
    If the function fails, the return value is zero.
Declare Function SetWindowLong Lib "user32" Alias "SetWindowLongA" _
    (ByVal hwnd As Long, ByVal nIndex As Long, ByVal dwNewLong As Long) As Long
  '##SUMMARY Changes an attribute of the specified window. _
    The function also sets a 32-bit (long) value at the specified offset _
    into the extra window memory of a window.
  '##PARAM hWnd I Identifies the window and, indirectly, the class to which _
    the window belongs.
  '##PARAM nIndex I Specifies the zero-based offset to the value to be set. _
    Valid values are in the range zero through the number of bytes of extra _
    window memory, minus 4; for example, if you specified 12 or more bytes _
    of extra memory, a value of 8 would be an index to the third 32-bit integer.
  '##PARAM dwNewLong I Specifies the replacement value.
  '##RETURNS If the function succeeds, the return value is the previous value of the specified 32-bit integer. _
    If the function fails, the return value is zero.
Declare Function CreatePipe Lib "kernel32" _
    (phReadPipe As Long, phWritePipe As Long, lpPipeAttributes As SECURITY_ATTRIBUTES, ByVal nSize As Long) As Long
  '##SUMMARY Creates an anonymous pipe, and returns handles to the read and write ends _
    of the pipe.
  '##PARAM hReadPipe O Pointer to a variable that receives the read handle for the pipe.
  '##PARAM hWritePipe O Pointer to a variable that receives the write handle for the pipe.
  '##PARAM lpPipeAttributes I Pointer to a structure that determines whether the _
    returned handle can be inherited by child processes. If lpPipeAttributes is NULL, _
    the handle cannot be inherited.
  '##PARAM nSize I Specifies the buffer size for the pipe, in bytes. The size is _
    only a suggestion; the system uses the value to calculate an appropriate buffering _
    mechanism. If this parameter is zero, the system uses the default buffer size.
  '##RETURNS If the function succeeds, the return value is nonzero. _
    If the function fails, the return value is zero.
Declare Function CloseHandle Lib "kernel32" _
    (ByVal hObject As Long) As Long
  '##SUMMARY Closes an open object handle.
  '##PARAM hObject I Identifies an open object handle.
  '##RETURNS If the function succeeds, the return value is nonzero. _
    If the function fails, the return value is zero.
Declare Function WaitForSingleObject Lib "kernel32" _
    (ByVal hHandle As Long, ByVal dwMilliseconds As Long) As Long
  '##SUMMARY Returns when one of the following occurs: _
    <UL> _
    <li>The specified object is in the signaled state _
    <li>The time-out interval elapses _
    </UL>
  '##PARAM hHandle I Identifies the object. For a list of the object _
    types whose handles can be specified, see the following Remarks section.
  '##PARAM dwMilliseconds I Specifies the time-out interval, in milliseconds. _
    The function returns if the interval elapses, even if the object’s state _
    is nonsignaled. If dwMilliseconds is zero, the function tests the object’s _
    state and returns immediately. If dwMilliseconds is INFINITE, the function’s _
    time-out interval never elapses.
  '##RETURNS If the function succeeds, the return value indicates the event that _
    caused the function to return: _
    <UL> _
    <li>WAIT_ABANDONED - The specified object is a mutex object that was not released _
      by the thread that owned the mutex object before the owning thread terminated. _
      Ownership of the mutex object is granted to the calling thread, and the mutex _
      is set to nonsignaled. _
    <li>WAIT_OBJECT_0 - The state of the specified object is signaled. _
    <li>WAIT_TIMEOUT - The time-out interval elapsed, and the object’s state is nonsignaled. _
    </UL> _
    If the function fails, the return value is WAIT_FAILED.
Declare Function CreateProcessBynum Lib "kernel32" Alias "CreateProcessA" _
    (ByVal lpApplicationName As String, ByVal lpCommandLine As String, ByVal lpProcessAttributes As Long, _
      ByVal lpThreadAttributes As Long, ByVal bInheritHandles As Long, ByVal dwCreationFlags As Long, _
      lpEnvironment As Any, ByVal lpCurrentDirectory As String, lpStartupInfo As STARTUPINFO, _
      lpProcessInformation As PROCESS_INFORMATION) As Long
  '##SUMMARY Creates a new process and its primary thread. _
    The new process executes the specified executable file.
  '##PARAM lpApplicationName I Pointer to a null-terminated string that specifies the _
    module to execute.
  '##PARAM lpCommandLine I Pointer to a null-terminated string that specifies the _
    command line to execute.
  '##PARAM lpProcessAttributes I Pointer to a SECURITY_ATTRIBUTES structure that _
    determines whether the returned handle can be inherited by child processes.
  '##PARAM lpThreadAttributes I Pointer to a SECURITY_ATTRIBUTES structure that _
    determines whether the returned handle can be inherited by child processes.
  '##PARAM bInheritHandles I Indicates whether the new process inherits handles _
    from the calling process. If TRUE, each inheritable open handle in the calling _
    process is inherited by the new process. Inherited handles have the same value _
    and access privileges as the original handles.
  '##PARAM dwCreationFlags I Specifies additional flags that control the priority _
    class and the creation of the process.
  '##PARAM lpEnvironment I Points to an environment block for the new process. _
    If this parameter is NULL, the new process uses the environment of the calling process.
  '##PARAM lpCurrentDirectory I Points to a null-terminated string that specifies _
    the current drive and directory for the child process. The string must be a full _
    path and filename that includes a drive letter. If this parameter is NULL, the _
    new process is created with the same current drive and directory as the calling _
    process. This option is provided primarily for shells that need to start an _
    application and specify its initial drive and working directory.
  '##PARAM lpStartupInfo I Points to a STARTUPINFO structure that specifies how _
    the main window for the new process should appear.
  '##PARAM lpProcessInformation O Points to a PROCESS_INFORMATION structure that _
    receives identification information about the new process.
  '##RETURNS If the function succeeds, the return value is nonzero. _
    If the function fails, the return value is zero.
Declare Function TerminateProcess Lib "kernel32" _
    (ByVal hProcess As Long, ByVal uexitcode As Long) As Long
  '##SUMMARY Terminates the specified process and all of its threads.
  '##PARAM hProcess I Identifies the process to terminate.
  '##PARAM uexitcode I Specifies the exit code for the process and for _
    all threads terminated as a result of this call.
  '##RETURNS If the function succeeds, the return value is nonzero. _
    If the function fails, the return value is zero.
Declare Function WaitForInputIdle Lib "user32" _
    (ByVal hProcess As Long, ByVal dwMilliseconds As Long) As Long
  '##SUMMARY Waits until the given process is waiting for user input _
    with no input pending, or until the time-out interval has elapsed.
  '##PARAM hProcess I Handle of process to wait for.
  '##PARAM dwMilliseconds I time-out interval in milliseconds
  '##RETURNS Indication of whether the process is ready or whether it timed out: _
    0 = process ready, or _
    WAIT_TIMEOUT = The time-out interval elapsed, and the process is not ready.
Declare Function SetStdHandle Lib "kernel32" _
    (ByVal nStdHandle As Long, ByVal nHandle As Long) As Long
  '##SUMMARY Sets a handle for the standard input, standard output, or standard error device.
  '##PARAM nStdHandle I Specifies the standard device for which to set the handle.
  '##PARAM nHandle I Specifies the standard device handle.
  '##RETURNS If the function succeeds, the return value is nonzero. _
    If the function fails, the return value is zero.
Declare Function GetStdHandle Lib "kernel32" _
    (ByVal nStdHandle As Long) As Long
  '##SUMMARY Retrieves a handle for the standard input, standard output, _
    or standard error device.
  '##PARAM nStdHandle I Specifies the standard device for which to return the handle.
  '##RETURNS If the function succeeds, the return value is nonzero. _
    If the function fails, the return value is zero.
Declare Function PeekNamedPipe Lib "kernel32" _
    (ByVal hNamedPipe As Long, lpBuffer As Any, ByVal nBufferSize As Long, lpBytesRead As Long, lpTotalBytesAvail As Long, lpBytesLeftThisMessage As Long) As Long
  '##SUMMARY Copies data from a named or anonymous pipe into a buffer without _
    removing it from the pipe. It also returns information about data in the pipe.
  '##PARAM hNamedPipe I The handle to the pipe.
  '##PARAM lpBuffer O Points to the buffer that receives the data read from the pipe.
  '##PARAM nBufferSize I The size of the buffer.
  '##PARAM lpBytesRead O Points to the number of bytes read.
  '##RETURNS If the function succeeds, the return value is nonzero. _
    If the function fails, the return value is zero.
Declare Function ReadFile Lib "kernel32" _
    (ByVal hFile As Long, lpBuffer As Any, ByVal nNumberOfBytesToRead As Long, lpNumberOfBytesRead As Long, ByVal lpOverlapped As Long) As Long
  '##SUMMARY Reads data from a file, starting at the position indicated by the file pointer.
  '##PARAM hFile I Identifies the file to be read.
  '##PARAM lpBuffer O Points to the buffer that receives the data read from the file.
  '##PARAM nNumberOfBytesToRead I Specifies the number of bytes to be read from the file.
  '##PARAM lpNumberOfBytesRead O Points to the number of bytes read.
  '##PARAM lpOverlapped O Points to an OVERLAPPED structure.
  '##RETURNS If the function succeeds, the return value is nonzero. _
    If the return value is nonzero and the number of bytes read is zero, _
    the file pointer was beyond the current end of the file at the time of _
    the read operation. However, if the file was opened with FILE_FLAG_OVERLAPPED _
    and lpOverlapped is not NULL, the return value is FALSE and GetLastError _
    returns ERROR_HANDLE_EOF when the file pointer goes beyond the current end of file. _
    If the function fails, the return value is zero.
Declare Function WriteFile Lib "kernel32" _
    (ByVal hFile As Long, lpBuffer As Any, ByVal nNumberOfBytesToWrite As Long, lpNumberOfBytesWritten As Long, ByVal lpOverlapped As Long) As Long
  '##SUMMARY Writes data to a file and is designed for both synchronous and asynchronous _
    operation. The function starts writing data to the file at the position indicated by _
    the file pointer.
  '##PARAM hFile I Identifies the file to be written to.
  '##PARAM lpBuffer I Points to the buffer containing the data to be written to the file.
  '##PARAM nNumberOfBytesToWrite I Specifies the number of bytes to write to the file.
  '##PARAM lpNumberOfBytesWritten O Points to the number of bytes written by this function call.
  '##PARAM lpOverlapped O Points to an OVERLAPPED structure.
  '##RETURNS If the function succeeds, the return value is nonzero. _
    If the function fails, the return value is zero.
Declare Function GetExitCodeProcess Lib "kernel32" _
    (ByVal hProcess As Long, lpExitCode As Long) As Long
  '##SUMMARY Retrieves the termination status of the specified process.
  '##PARAM hProcess I Identifies the process.
  '##PARAM lpExitCode O Points to a 32-bit variable to receive the process termination status.
  '##RETURNS If the function succeeds, the return value is nonzero. _
    If the function fails, the return value is zero.
Declare Function GetCurrentProcessId Lib "kernel32" () As Long
  '##SUMMARY Returns the process identifier of the calling process.
  '##RETURNS The return value is the process identifier of the calling process.
Declare Function GetFocus Lib "user32" () As Long
  '##SUMMARY Retrieves the handle of the window that has the keyboard focus, _
    if the window is associated with the calling thread’s message queue.
  '##RETURNS If the function succeeds, the return value is the handle of the _
    window with the keyboard focus. If the calling thread’s message queue does _
    not have an associated window with the keyboard focus, the return value is NULL.
Public Declare Function SHAddToRecentDocs Lib "shell32.dll" _
    (ByVal dwFlags As Long, ByVal dwData As String) As Long
  '##SUMMARY Adds a document to the shell’s list of recently used documents or _
    clears all documents from the list. The user gains access to the list _
    through the Start menu of the Windows taskbar.
  '##PARAM dwFlags I Flag that indicates the meaning of the dwData parameter; _
    <OL> _
    <li>either SHARD_PATH = pv is the address of a path string, or _
    <li>SHARD_PIDL = pv is the address of an item identifier list. _
    </OL>
  '##PARAM dwData I Pointer to a buffer that contains the path and filename of _
    the document, or the address of an ITEMIDLIST structure that contains an _
    item identifier list uniquely identifying the document. _
    If this parameter is NULL, the function clears all documents from the list.
  '##RETURNS No return value.
Public Declare Function SHFileOperation Lib "shell32.dll" Alias "SHFileOperationA" _
    (lpFileOp As SHFILEOPSTRUCT) As Long
  '##SUMMARY Performs a copy, move, rename, or delete operation on a file system object.
  '##PARAM lpFileOp I Pointer to an SHFILEOPSTRUCT structure that contains information _
    the function needs to carry out the operation.
  '##RETURNS Returns zero if successful or nonzero if an error occurs.
Public Declare Function SHGetPathFromIDList Lib "shell32.dll" Alias "SHGetPathFromIDListA" _
    (ByVal pidl As Long, ByVal pszPath As String) As Long
  '##SUMMARY Converts an item identifier list to a file system path.
  '##PARAM pidl I Pointer to an item identifier list that specifies a file _
    or directory location relative to the root of the name space (the desktop).
  '##PARAM pszPath O Pointer to a buffer that receives the file system path. _
    The size of this buffer is assumed to be MAX_PATH bytes.
  '##RETURNS Returns TRUE if successful or FALSE if an error occurs ¾ for example, _
    if the location specified by the pidl parameter is not part of the file system.

Private Declare Sub CoTaskMemFree Lib "ole32.dll" (ByVal pv As Long)

Public Declare Function SHGetSpecialFolderLocation Lib "shell32.dll" _
    (ByVal hwndOwner As Long, ByVal nFolder As Long, pidl As ITEMIDLIST) As Long
  '##SUMMARY Retrieves the location of a special folder.
  '##PARAM hwndOwner I Handle of the owner window that the client should specify _
    if it displays a dialog box or message box.
  '##PARAM nFolder I Value specifying the folder to retrieve the location of.
  '##PARAM ppidl O Address that receives a pointer to an item identifier list _
    specifying the folder’s location relative to the root of the name space (the desktop).
  '##RETURNS Returns NOERROR if successful or an OLE-defined error result otherwise.
Declare Function GetTempPath Lib "kernel32" Alias "GetTempPathA" _
    (ByVal nBufferLength As Long, ByVal lpBuffer As String) As Long
  '##SUMMARY Retrieves the path of the directory designated for temporary files. _
    This function supersedes the GetTempDrive function.
  '##PARAM nBufferLength I Specifies the size, in characters, of the string buffer _
    identified by lpBuffer.
  '##PARAM lpBuffer O Points to a string buffer that receives the null-terminated _
    string specifying the temporary file path.
  '##RETURNS If the function succeeds, the return value is the length, in characters, _
    of the string copied to lpBuffer, not including the terminating null character. _
    If the return value is greater than nBufferLength, the return value is the size of _
    the buffer required to hold the path. If the function fails, the return value is _
    zero. To get extended error information, call GetLastError.

Public Declare Function GetComputerName Lib "kernel32" Alias "GetComputerNameA" _
   (ByVal lpBuffer As String, _
             nSize As Long) As Long
Public Declare Function GetUserName& Lib "advapi32.dll" Alias "GetUserNameA" _
   (ByVal lpBuffer As String, _
             nSize As Long)


Public Declare Function CallNextHookEx Lib "user32" _
    (ByVal hHook As Long, ByVal nCode As Long, ByVal wParam As Long, ByVal lParam As Long) As Long
  '##SUMMARY Passes the hook information to the next hook procedure in the current hook chain. A hook procedure can call this function either before or after processing the hook information.
  '##PARAM hHook O Identifies the current hook. An application receives this handle as a result of a previous call to the SetWindowsHookEx function.
  '##PARAM nCode I Specifies the hook code passed to the current hook procedure. _
    The next hook procedure uses this code to determine how to process the hook information.
  '##PARAM wParam - Specifies the wParam value passed to the current hook procedure. _
    The meaning of this parameter depends on the type of hook associated with the _
    current hook chain.
  '##PARAM lParam - Specifies the lParam value passed to the current hook procedure. _
    The meaning of this parameter depends on the type of hook associated with the _
    current hook chain.
  '##RETURNS If the function succeeds, the return value is the value returned by the _
    next hook procedure in the chain. The current hook procedure must also return _
    this value. The meaning of the return value depends on the hook type.
Public Declare Function GetKeyNameText Lib "user32" Alias "GetKeyNameTextA" _
    (ByVal lParam As Long, ByVal keyName As String, ByVal size As Long) As Long
  '##SUMMARY Retrieves the string representing the name of a key.
  '##PARAM lParam I Scan code in bits 16-23.
  '##PARAM keyName O Pointer to a buffer which will hold the keyboard text name.
  '##PARAM size I use value of 256.
  '##RETURNS Length of string.
Public Declare Function UnhookWindowsHookEx Lib "user32" _
    (ByVal hHook As Long) As Long
  '##SUMMARY Removes a hook procedure installed in a hook chain by the _
    SetWindowsHookEx function.
  '##PARAM hHook I Identifies the hook to be removed. This parameter is a _
    hook handle obtained by a previous call to SetWindowsHookEx.
  '##RETURNS If the function succeeds, the return value is nonzero. _
    If the function fails, the return value is zero.
Public Declare Function SetWindowsHookEx Lib "user32" Alias "SetWindowsHookExA" _
    (ByVal idHook As Long, ByVal lpfn As Long, ByVal hmod As Long, ByVal dwThreadId As Long) As Long
  '##SUMMARY Installs an application-defined hook _
    procedure into a hook chain. You would install a hook procedure to monitor the _
    system for certain types of events. These events are associated either with a _
    specific thread or with all threads in the system.
  '##PARAM idHook I Specifies the type of hook procedure to be installed.
  '##PARAM lpfn I Points to the hook procedure. If the dwThreadId parameter is _
    zero or specifies the identifier of a thread created by a different process, _
    the lpfn parameter must point to a hook procedure in a dynamic-link library _
    (DLL). Otherwise, lpfn can point to a hook procedure in the code associated _
    with the current process.
  '##PARAM hmod I Identifies the DLL containing the hook procedure pointed to by _
    the lpfn parameter. The hMod parameter must be set to NULL if the dwThreadId _
    parameter specifies a thread created by the current process and if the hook _
    procedure is within the code associated with the current process.
  '##PARAM dwThreadId I Specifies the identifier of the thread with which the hook _
    procedure is to be associated. If this parameter is zero, the hook procedure is _
    associated with all existing threads.
  '##RETURNS If the function succeeds, the return value is the handle of the hook procedure. _
    If the function fails, the return value is NULL.
Public Declare Function GetProfileString Lib "kernel32" Alias "GetProfileStringA" _
    (ByVal lpAppName As String, ByVal lpKeyName As String, ByVal lpDefault As String, _
     ByVal lpReturnedString As String, ByVal nSize As Long) As Long
  '##SUMMARY Retrieves the string associated with a _
    key in the specified section of the Win.ini file.
  '##PARAM lpAppName I Pointer to a null-terminated string that specifies the name _
    of the section containing the key. If this parameter is NULL, the function _
    copies all section names in the file to the supplied buffer.
  '##PARAM lpKeyName I Pointer to a null-terminated string specifying the name _
    of the key whose associated string is to be retrieved. If this parameter is _
    NULL, the function copies all keys in the given section to the supplied buffer. _
    Each string is followed by a null character, and the final string is followed _
    by a second null character.
  '##PARAM lpDefault I Pointer to a null-terminated default string.
  '##PARAM lpReturnedString O Pointer to a buffer that receives the character string.
  '##PARAM nSize I Specifies the size, in TCHARs, of the buffer pointed to by the _
    lpReturnedString parameter.
  '##RETURNS The return value is the number of characters copied to the buffer, _
    not including the null-terminating character. _
    If neither lpAppName nor lpKeyName _
    is NULL and the supplied destination buffer is too small to hold the requested _
    string, the string is truncated and followed by a null character, and the return _
    value is equal to nSize minus one. _
    If either lpAppName or lpKeyName is NULL and the supplied destination buffer is _
    too small to hold all the strings, the last string is truncated and followed by _
    two null characters. In this case, the return value is equal to nSize minus two.
Public Declare Function WriteProfileString Lib "kernel32" Alias "WriteProfileStringA" _
    (ByVal lpszSection As String, ByVal lpszKeyName As String, ByVal lpszString As String) As Long
  '##SUMMARY Copies a string into the specified section of the Win.ini file.
  '##PARAM lpszSection I Pointer to a null-terminated string that specifies _
    the section to which the string is to be copied. If the section does not _
    exist, it is created. The name of the section is not case-sensitive; the _
    string can be any combination of uppercase and lowercase letters.
  '##PARAM lpszKeyName I Pointer to a null-terminated string containing the key _
    to be associated with the string. If the key does not exist in the specified _
    section, it is created. If this parameter is NULL, the entire section, _
    including all entries in the section, is deleted.
  '##PARAM lpszString I Pointer to a null-terminated string to be written to the file. _
    If this parameter is NULL, the key pointed to by the lpKeyName parameter is deleted.
  '##RETURNS If the function successfully copies the string to the Win.ini file, the _
    return value is nonzero. If the function fails, or if it flushes the cached version _
    of Win.ini, the return value is zero.
Declare Function GetWindowText Lib "user32" Alias "GetWindowTextA" _
    (ByVal hwnd As Long, ByVal lpString As String, ByVal aint As Long) As Long
  '##SUMMARY Copies the text of the specified window’s title bar (if it has one) into _
    a buffer. If the specified window is a control, the text of the control is copied.
  '##PARAM hWnd I Identifies the window or control containing the text.
  '##PARAM lpString O Points to the buffer that will receive the text.
  '##PARAM aint I Specifies the maximum number of characters to copy to the buffer, _
    including the NULL character. If the text exceeds this limit, it is truncated.
  '##RETURNS If the function succeeds, the return value is the length, in characters, _
    of the copied string, not including the terminating null character. _
    If the window has no title bar or text, if the title bar is empty, or if _
    the window or control handle is invalid, the return value is zero.
'This function cannot retrieve the text of an edit control in another application.
Declare Function GetWindow Lib "user32" _
    (ByVal hwnd As Long, ByVal wCmd As Long) As Long
  '##SUMMARY The GetWindow function retrieves the handle of a window that has _
    the specified relationship (Z order or owner) to the specified window.
  '##PARAM hWnd I Identifies a window. The window handle retrieved is relative _
    to this window, based on the value of the uCmd parameter.
  '##PARAM uCmd I Specifies the relationship between the specified window and _
    the window whose handle is to be retrieved.
  '##RETURNS If the function succeeds, the return value is a window handle. _
    If no window exists with the specified relationship to the specified window, _
    the return value is NULL.
Declare Function FindWindow Lib "user32" Alias "FindWindowA" _
    (ByVal lpClassName As Any, ByVal lpWindowName As Any) As Long
  '##SUMMARY Retrieves the handle to the top-level _
    window whose class name and window name match the specified strings. _
    This function does not search child windows.
  '##PARAM lpClassName I Points to a null-terminated string that specifies _
    the class name or is an atom that identifies the class-name string. _
    If this parameter is an atom, it must be a global atom created by a _
    previous call to the GlobalAddAtom function. The atom, a 16-bit value, must _
    be placed in the low-order word of lpClassName; the high-order word must be zero.
  '##PARAM lpWindowName I Points to a null-terminated string that specifies the _
    window name (the window’s title). If this parameter is NULL, all window names match.
  '##RETURNS If the function succeeds, the return value is the handle to the window _
    that has the specified class name and window name. _
    If the function fails, the return value is NULL.
Declare Function ShowWindow Lib "user32" _
    (ByVal hwnd As Long, ByVal nCmdShow As Long) As Long
  '##SUMMARY Sets the specified window’s show state.
  '##PARAM hWnd I Identifies the window.
  '##PARAM nCmdShow I Specifies how the window is to be shown. This parameter _
    is ignored the first time an application calls ShowWindow, if the program _
    that launched the application provides a STARTUPINFO structure. Otherwise, _
    the first time ShowWindow is called, the value should be the value obtained _
    by the WinMain function in its nCmdShow parameter.
  '##RETURNS If the window was previously visible, the return value is nonzero. _
    If the window was previously hidden, the return value is zero.
Declare Function SendMessage Lib "user32" Alias "SendMessageA" _
    (ByVal hwnd As Long, ByVal wMsg As Long, ByVal wParam As Long, lParam As Any) As Long
  '##SUMMARY Sends the specified message to a window or windows. _
    The function calls the window procedure for the specified window and does not return _
    until the window procedure has processed the message. The PostMessage function, in _
    contrast, posts a message to a thread’s message queue and returns immediately.
  '##PARAM hWnd M Identifies the window whose window procedure will receive the message. _
    If this parameter is HWND_BROADCAST, the message is sent to all top-level windows in _
    the system, including disabled or invisible unowned windows, overlapped windows, and _
    pop-up windows; but the message is not sent to child windows.
  '##PARAM wMsg I Specifies the message to be sent.
  '##PARAM wParam I Specifies additional message-specific information.
  '##PARAM lParam I Specifies additional message-specific information.
  '##RETURNS The return value specifies the result of the message processing _
    and depends on the message sent.
Declare Function ShellExecute Lib "shell32.dll" Alias "ShellExecuteA" _
    (ByVal hwnd As Long, ByVal lpOperation As String, ByVal lpFile As String, _
     ByVal lpParameters As String, ByVal lpDirectory As String, ByVal nShowCmd As Long) As Long
  '##SUMMARY Opens or prints a specified file. The file _
    can be an executable file or a document file.
  '##PARAM hwnd M Specifies a parent window. This window receives any message _
    boxes that an application produces. For example, an application may report _
    an error by producing a message box.
  '##PARAM lpOperation I Pointer to a null-terminated string that specifies the _
    operation to perform: "open", "print", or "explore".
  '##PARAM lpFile I Pointer to a null-terminated string that specifies the file _
    to open or print or the folder to open or explore. The function can open an _
    executable file or a document file. The function can print a document file.
  '##PARAM lpParameters I If lpFile specifies an executable file, lpParameters _
    is a pointer to a null-terminated string that specifies parameters to be _
    passed to the application. If lpFile specifies a document file, lpParameters _
    should be NULL.
  '##PARAM lpDirectory I Pointer to a null-terminated string that specifies the _
    default directory.
  '##PARAM nShowCmd I If lpFile specifies an executable file, nShowCmd specifies _
    how the application is to be shown when it is opened.
  '##RETURNS If the function succeeds, the return value is the instance handle of _
    the application that was run, or the handle of a dynamic data exchange (DDE) _
    server application. If the function fails, the return value is an error value _
    that is less than or equal to 32.
Declare Sub Sleep Lib "kernel32" (ByVal dwMilliseconds As Long)
  '##SUMMARY The Sleep function suspends the execution of the current thread for a specified interval.
  '##PARAM dwMilliseconds I Specifies the time, in milliseconds, for which to suspend execution. A value of zero causes the thread to relinquish the remainder of its time slice to any other thread of equal priority that is ready to run. If there are no other threads of equal priority ready to run, the function returns immediately, and the thread continues execution. A value of INFINITE causes an infinite delay.
  '##RETURNS This function does not return a value.

Private Declare Function GetTempFileNameA Lib "kernel32" (ByVal _
   lpszPath As String, ByVal lpPrefixString As String, ByVal wUnique _
   As Long, ByVal lpTempFileName As String) As Long

Public Function GetTmpFileName() As String
  Dim FileName As String
  Dim path As String

  path = GetTmpPath
  FileName = Space(Len(path) + 256)
  Call GetTempFileNameA(path, App.exeName, &H0, FileName)
  GetTmpFileName = Left(FileName, InStr(FileName, Chr$(0)) - 1)
End Function

Public Function GetTmpPath()
Attribute GetTmpPath.VB_Description = "The Sleep function suspends the execution of the current thread for a specified interval."
  '##SUMMARY Makes call to API Function GetTempPath.
  '##RETURNS  The path of the directory designated for temporary files.
  Dim strFolder As String 'directory pathname
  Dim lngResult As Long   'return code from call to GetTempPath
  Dim MAX_PATH As Long    'maximum length of directory pathname
  
  MAX_PATH = 260
  strFolder = String(MAX_PATH, 0)
  lngResult = GetTempPath(MAX_PATH, strFolder)
  If lngResult <> 0 Then
      GetTmpPath = Left(strFolder, InStr(strFolder, Chr(0)) - 1)
  Else
      GetTmpPath = ""
  End If
End Function

Sub SetFullSel(pcList As Control)
Attribute SetFullSel.VB_Description = "."
  '##SUMMARY .
  '##PARAM pcList M
    Dim iStyle As Long  '
    Dim IResult As Long '
    
    Const LVM_FIRST = &H1000
    Const LVM_SETEXTENDEDLISTVIEWSTYLE = LVM_FIRST + 54
    Const LVM_GETEXTENDEDLISTVIEWSTYLE = LVM_FIRST + 55
    Const LVS_FULLROWSELECT = &H20
    
    iStyle = SendMessage(pcList.hwnd, LVM_GETEXTENDEDLISTVIEWSTYLE, 0, 0)
    iStyle = iStyle Or LVS_FULLROWSELECT
    IResult = SendMessage(pcList.hwnd, LVM_SETEXTENDEDLISTVIEWSTYLE, 0, iStyle)
    
End Sub

Public Function GetSpecialFolder(lFolder As SpecialFolderType) As String
  Dim lp As ITEMIDLIST
  Dim tmpstr As String, hwnd As Long
  SHGetSpecialFolderLocation hwnd, lFolder, lp
  tmpstr = Space$(255)
  SHGetPathFromIDList lp.mkid, tmpstr
  If InStr(tmpstr, Chr$(0)) > 0 Then
      tmpstr = Left$(tmpstr, InStr(tmpstr, Chr$(0)) - 1)
  End If
  CoTaskMemFree lp.mkid
  GetSpecialFolder = tmpstr
End Function

Public Function FindAssociatedApplication(dataFile As String) As String
  Dim created As Boolean
  Dim appName As String
  appName = Space(255)
  
  If Not FileExists(dataFile) Then
    SaveFileString dataFile, ""
    created = True
  End If
  
  If FindExecutable(FilenameNoPath(dataFile), PathNameOnly(dataFile), appName) > 32 Then
    FindAssociatedApplication = Left(appName, InStr(appName, Chr$(0)) - 1)
  End If
  
  If created Then Kill dataFile
End Function

Public Function GetLogicalDriveStringsAsCollection() As Collection
  Dim lBuffer As String
  Dim lBufferLen As Long
  Dim lCol As New Collection
  
  lBufferLen = 512
  lBuffer = Space(lBufferLen)

  GetLogicalDriveStrings lBufferLen, lBuffer
  
  lBuffer = Trim(lBuffer)
  While Len(lBuffer) > 2
    lCol.Add StrSplit(lBuffer, Chr(0), "'")
  Wend
  
  Set GetLogicalDriveStringsAsCollection = lCol
  
End Function

Public Sub SetColorTransparent(hwnd As Long, colr As Long)
  Dim Style As Long
  Dim WinPlatform As String
  WinPlatform = GetWinPlatform
  If InStr(WinPlatform, "2000") > 0 Or InStr(WinPlatform, "XP") Then
    Style = GetWindowLong(hwnd, GWL_EXSTYLE) Or WS_EX_LAYERED
    SetWindowLong hwnd, GWL_EXSTYLE, Style
    SetLayeredWindowAttributes hwnd, colr, 0, 1 ' 1 = LWA_COLORKEY
  End If
End Sub

Public Sub UnsetTransparent(hwnd As Long)
  Dim Style As Long
  Dim dummyRect As RECT
  Dim WinPlatform As String
  WinPlatform = GetWinPlatform
  If InStr(WinPlatform, "2000") > 0 Or InStr(WinPlatform, "XP") Then
    Style = GetWindowLong(hwnd, GWL_EXSTYLE) And Not WS_EX_LAYERED
    SetWindowLong hwnd, GWL_EXSTYLE, Style
    RedrawWindow hwnd, dummyRect, 0, RDW_ERASE Or RDW_INVALIDATE Or RDW_FRAME Or RDW_ALLCHILDREN
  End If
End Sub

Public Function GetExeFullPath() As String
  Dim hModule As Long
  Dim i As Long
  Dim s As String * 250
  
  hModule = GetModuleHandle(App.exeName & ".exe")
  i = GetModuleFileName(hModule, s, 250)
  GetExeFullPath = Left(s, InStr(s, Chr(0)) - 1)
End Function

'-----------------------------------------------------------
' SUB: AddDirSep
' Add a trailing directory path separator (back slash) to the
' end of a pathname unless one already exists
'
' IN/OUT: [strPathName] - path to add separator to
'-----------------------------------------------------------
'
Public Sub AddDirSep(strPathName As String)
  strPathName = RTrim$(strPathName)
  If Right$(strPathName, 1) <> "/" Then
    If Right$(strPathName, 1) <> "\" Then strPathName = strPathName & "\"
  End If
End Sub

Public Function APIComputerName() As String
  Dim UName As String
  Dim UNameLen As Long
  
  UName = Space(255)
  UNameLen = 255
  
  If GetComputerName(UName, UNameLen) <> 0 Then 'We got the name
    APIComputerName = Left(UName, UNameLen)
  Else
    APIComputerName = "Unknown"
  End If
End Function

Public Function APIUserName() As String
  Dim UName As String
  Dim UNameLen As Long
  
  UName = Space(255)
  UNameLen = 255
  
  If GetUserName(UName, UNameLen) <> 0 Then 'We got the user's name
    APIUserName = Left(UName, UNameLen - 1) 'Get all except the last character
  Else
    APIUserName = "Unknown"
  End If

End Function

Public Function GetFileVerString(ByVal sFile As String) As String
  Const sEXE As String = "\"
  Dim lVerSize As Long
  Dim lTemp As Long
  Dim lRet As Long
  Dim bInfo() As Byte
  Dim lpBuffer As Long
  Dim sVer As VERINFO

  '
  'Get the size of the file version info, allocate a buffer for it, and get the
  'version info.  Next, we query the Fixed file info portion, where the internal
  'file version used by the Windows VerInstallFile API is kept.  We then copy
  'the fixed file info into a VERINFO structure.
  '
  lVerSize = GetFileVersionInfoSize(sFile, lTemp)
  ReDim bInfo(lVerSize)
  If lVerSize > 0 Then
    lRet = GetFileVersionInfo(sFile, lTemp, lVerSize, VarPtr(bInfo(0)))
    If lRet <> 0 Then
      lRet = VerQueryValue(VarPtr(bInfo(0)), sEXE, lpBuffer, lVerSize)
      If lRet <> 0 Then
        CopyMemory sVer, ByVal lpBuffer, lVerSize
        GetFileVerString = sVer.FileVerPart1 & "." _
                         & sVer.FileVerPart2 & "." _
                         & sVer.FileVerPart3 & "." _
                         & sVer.FileVerPart4
      End If
    End If
  End If
End Function

Public Function LibraryExists(ByVal LibraryName As String, _
                       ByRef ErrorReturned As Integer, _
                       ByRef ErrorExplanation As String) As Boolean
  Dim OriginalErrorMode As Integer
  Const SEM_NOOPENFILEERRORBOX = &H8000
  Const SEM_FAILCRITICALERRORS = &H1

  OriginalErrorMode = SetErrorMode(SEM_NOOPENFILEERRORBOX Or SEM_FAILCRITICALERRORS)
  ErrorReturned = LoadLibrary(LibraryName)
  SetErrorMode OriginalErrorMode
  If ErrorReturned > 32 Then 'Not an error code, but a hInstance
    LibraryExists = True
    FreeLibrary (ErrorReturned)
  Else
    LibraryExists = False
    Select Case ErrorReturned
      Case 0:  ErrorExplanation = "System is out of memory, executable file is corrupt, or relocations are invalid."
      Case 2:  ErrorExplanation = "File not found."
      Case 3:  ErrorExplanation = "Path not found."
      Case 5:  ErrorExplanation = "Sharing or network protection error."
      Case 6:  ErrorExplanation = "Library required separate data segments for each task."
      Case 8:  ErrorExplanation = "Insufficient memory."
      Case 10: ErrorExplanation = "Incorrect Windows version."
      Case 11: ErrorExplanation = "It was either not a Windows application or there was an error in the file."
      Case 12: ErrorExplanation = "It was designed for a different operating system."
      Case 13: ErrorExplanation = "It was designed for MS-DOS 4.0."
      Case 14: ErrorExplanation = "File type unknown."
      Case 15: ErrorExplanation = "The file was designed for an earlier version of Windows."
      Case 16: ErrorExplanation = "An attempt was made to load a second instance of an executable file containing multiple data segments not marked read-only."
      Case 19: ErrorExplanation = "Attempt was made to load a compressed file. It must be decompressed before it can be loaded."
      Case 20: ErrorExplanation = "DLL file is invalid. This file or one called by it is corrupt."
      Case 21: ErrorExplanation = "The file requires Microsoft Windows 32-bit extensions."
      Case Else: ErrorExplanation = "The reason it wouldn't load is unclear. Error code: " & ErrorReturned
    End Select
  End If
End Function

Public Function StringFromBuffer(Buffer As String) As String
  Dim nPos As Long

  nPos = InStr(Buffer, vbNullChar)
  If nPos > 0 Then
      StringFromBuffer = Left$(Buffer, nPos - 1)
  Else
      StringFromBuffer = Buffer
  End If
End Function

Public Function GetWinPlatform() As String
  Dim osvi As OSVERSIONINFO
  Dim verString As String

  osvi.dwOSVersionInfoSize = Len(osvi)
  If GetVersionEx(osvi) = 0 Then
    GetWinPlatform = "GetVersionEx failed"
    Exit Function
  End If
  'dwPlatformId And 1 = True if win95-like
  'dwPlatformId And 2 = True if NT-like
  verString = "PlatformId=" & osvi.dwPlatformId
  Select Case osvi.dwPlatformId
    Case 1: verString = "Win95ish"
    Case 2:
      Select Case osvi.dwMajorVersion
        Case Is < 5: verString = "NT"
        Case 5:
          If osvi.dwMinorVersion = 0 Then
            verString = "2000"
          ElseIf osvi.dwMinorVersion = 1 Then
            verString = "XP"
          End If
      End Select
  End Select
  GetWinPlatform = verString & " " _
                 & "(" & osvi.dwMajorVersion _
                 & "." & osvi.dwMinorVersion _
                 & "." & osvi.dwBuildNumber & ") " _
                 & StringFromBuffer(osvi.szCSDVersion)
End Function

Public Function GetWindowsDir() As String
  Dim strBuf As String

  strBuf = Space$(gintMAX_SIZE)
  '
  'Get the windows directory and then trim the buffer to the exact length
  'returned and add a dir sep (backslash) if the API didn't return one
  '
  If GetWindowsDirectory(strBuf, gintMAX_SIZE) Then
    GetWindowsDir = StringFromBuffer(strBuf)
    AddDirSep GetWindowsDir
  End If
End Function

Public Function GetWindowsSysDir() As String
    Dim strBuf As String

    strBuf = Space$(gintMAX_SIZE)
    '
    'Get the system directory and then trim the buffer to the exact length
    'returned and add a dir sep (backslash) if the API didn't return one
    '
    If GetSystemDirectory(strBuf, gintMAX_SIZE) Then
        GetWindowsSysDir = StringFromBuffer(strBuf)
        AddDirSep GetWindowsSysDir
    End If
End Function

Public Sub BringFormToTop(frm As Form)
  frm.WindowState = vbNormal
  frm.Visible = True
  
' HWND_TOPMOST = -1
' HWND_NOTOPMOST = -2
' SWP_NOSIZE = 1
' SWP_NOMOVE = 2
' FLAGS = SWP_NOSIZE Or SWP_NOMOVE

  SetWindowPos frm.hwnd, -1, 0, 0, 0, 0, 3 '3=SWP_NOSIZE Or SWP_NOMOVE
  SetWindowPos frm.hwnd, -2, 0, 0, 0, 0, 3
End Sub

Private Sub SetWinDirs()
  If pLenWinSys = 0 Then
    pWindowsSysDir = GetWindowsSysDir
    pWindowsDir = GetWindowsDir
    'remove trailing \
    If Right(pWindowsSysDir, 1) = "\" Then pWindowsSysDir = Left(pWindowsSysDir, Len(pWindowsSysDir) - 1)
    If Right(pWindowsDir, 1) = "\" Then pWindowsDir = Left(pWindowsDir, Len(pWindowsDir) - 1)
    pLenWinSys = Len(pWindowsSysDir)
    pLenWin = Len(pWindowsDir)
  End If
End Sub

Public Function ExpandWinSysNames(abbreviatedFilename As String, Optional appPath As String = "") As String
  Dim retval As String
  SetWinDirs
  retval = ReplaceString(abbreviatedFilename, "{sys}", pWindowsSysDir)
  retval = ReplaceString(retval, "{win}", pWindowsDir)
  If Len(appPath) > 0 Then
    If Right(appPath, 1) = "\" Then
      retval = ReplaceString(retval, "{app}", Left(appPath, Len(appPath) - 1))
    Else
      retval = ReplaceString(retval, "{app}", appPath)
    End If
  End If
  ExpandWinSysNames = retval
End Function

