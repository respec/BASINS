Attribute VB_Name = "modHtmlHelp"
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants
'
' HtmlHelp.Bas - Declarations for calling the HtmlHelp API
'
' NOTE: Some of the declarations in this file depend on
' on declarations in Win32Api.Bas.
'
' This first part of this file is a straight translation from
' the C/C++ header file, HtmlHelp.h. Some of the declarations
' are untested since the corresponding parts of HtmlHelp are
' not yet implemented.
'
' The second part of this file is a set of helper functions
' and declarations to make it easier to use the HtmlHelp API.
'

' Commands to pass to HtmlHelp()

Enum HH_COMMAND
    HH_DISPLAY_TOPIC = &H0
    HH_HELP_FINDER = &H0           ' WinHelp equivalent
    HH_DISPLAY_TOC = &H1           ' not currently implemented
    HH_DISPLAY_INDEX = &H2         ' not currently implemented
    HH_DISPLAY_SEARCH = &H3        ' not currently implemented
    HH_SET_WIN_TYPE = &H4
    HH_GET_WIN_TYPE = &H5
    HH_GET_WIN_HANDLE = &H6
    HH_GET_INFO_TYPES = &H7        ' not currently implemented
    HH_SET_INFO_TYPES = &H8        ' not currently implemented
    HH_SYNC = &H9
    HH_ADD_NAV_UI = &HA            ' not currently implemented
    HH_ADD_BUTTON = &HB            ' not currently implemented
    HH_GETBROWSER_APP = &HC        ' not currently implemented
    HH_KEYWORD_LOOKUP = &HD
    HH_DISPLAY_TEXT_POPUP = &HE    ' display string resource id or text in a popup window
    HH_HELP_CONTEXT = &HF          ' display mapped numeric value in dwData
    HH_TP_HELP_CONTEXTMENU = &H10  ' text popup help, same as WinHelp HELP_CONTEXTMENU
    HH_TP_HELP_WM_HELP = &H11      ' text popup help, same as WinHelp HELP_WM_HELP
    HH_CLOSE_ALL = &H12            ' close all windows opened directly or indirectly by the caller
    HH_ALINK_LOOKUP = &H13         ' ALink version of HH_KEYWORD_LOOKUP
End Enum

Public Const HHWIN_PROP_ONTOP = (2 ^ 1)             ' Top-most window (not currently implemented)
Public Const HHWIN_PROP_NOTITLEBAR = (2 ^ 2)        ' no title bar
Public Const HHWIN_PROP_NODEF_STYLES = (2 ^ 3)      ' no default window styles (only HH_WINTYPE.dwStyles)
Public Const HHWIN_PROP_NODEF_EXSTYLES = (2 ^ 4)    ' no default extended window styles (only HH_WINTYPE.dwExStyles)
Public Const HHWIN_PROP_TRI_PANE = (2 ^ 5)          ' use a tri-pane window
Public Const HHWIN_PROP_NOTB_TEXT = (2 ^ 6)         ' no text on toolbar buttons
Public Const HHWIN_PROP_POST_QUIT = (2 ^ 7)         ' post WM_QUIT message when window closes
Public Const HHWIN_PROP_AUTO_SYNC = (2 ^ 8)         ' automatically ssync contents and index
Public Const HHWIN_PROP_TRACKING = (2 ^ 9)          ' send tracking notification messages
Public Const HHWIN_PROP_TAB_SEARCH = (2 ^ 10)       ' include search tab in navigation pane
Public Const HHWIN_PROP_TAB_HISTORY = (2 ^ 11)      ' include history tab in navigation pane
Public Const HHWIN_PROP_TAB_FAVORITES = (2 ^ 12)    ' include favorites tab in navigation pane
Public Const HHWIN_PROP_CHANGE_TITLE = (2 ^ 13)     ' Put current HTML title in title bar
Public Const HHWIN_PROP_NAV_ONLY_WIN = (2 ^ 14)     ' Only display the navigation window
Public Const HHWIN_PROP_NO_TOOLBAR = (2 ^ 15)       ' Don't display a toolbar
Public Const HHWIN_PROP_MENU = (2 ^ 16)             ' Menu

Public Const HHWIN_PARAM_PROPERTIES = (2 ^ 1)       ' valid fsWinProperties
Public Const HHWIN_PARAM_STYLES = (2 ^ 2)           ' valid dwStyles
Public Const HHWIN_PARAM_EXSTYLES = (2 ^ 3)         ' valid dwExStyles
Public Const HHWIN_PARAM_RECT = (2 ^ 4)             ' valid rcWindowPos
Public Const HHWIN_PARAM_NAV_WIDTH = (2 ^ 5)        ' valid iNavWidth
Public Const HHWIN_PARAM_SHOWSTATE = (2 ^ 6)        ' valid nShowState
Public Const HHWIN_PARAM_INFOTYPES = (2 ^ 7)        ' valid painfoTypes
Public Const HHWIN_PARAM_TB_FLAGS = (2 ^ 8)         ' valid fsToolBarFlags
Public Const HHWIN_PARAM_EXPANSION = (2 ^ 9)        ' valid fNotExpanded
Public Const HHWIN_PARAM_TABPOS = (2 ^ 10)          ' valid tabpos
Public Const HHWIN_PARAM_TABORDER = (2 ^ 11)        ' valid taborder
Public Const HHWIN_PARAM_HISTORY_COUNT = (2 ^ 12)   ' valid cHistory
Public Const HHWIN_PARAM_CUR_TAB = (2 ^ 13)         ' valid curNavType

Public Const HHWIN_BUTTON_EXPAND = (2 ^ 1)
Public Const HHWIN_BUTTON_BACK = (2 ^ 2)
Public Const HHWIN_BUTTON_FORWARD = (2 ^ 3)
Public Const HHWIN_BUTTON_STOP = (2 ^ 4)
Public Const HHWIN_BUTTON_REFRESH = (2 ^ 5)
Public Const HHWIN_BUTTON_HOME = (2 ^ 6)
Public Const HHWIN_BUTTON_BROWSE_FWD = (2 ^ 7)
Public Const HHWIN_BUTTON_BROWSE_BCK = (2 ^ 8)
Public Const HHWIN_BUTTON_NOTES = (2 ^ 9)
Public Const HHWIN_BUTTON_CONTENTS = (2 ^ 10)       ' not implemented
Public Const HHWIN_BUTTON_SYNC = (2 ^ 11)
Public Const HHWIN_BUTTON_OPTIONS = (2 ^ 12)
Public Const HHWIN_BUTTON_PRINT = (2 ^ 13)          ' not implemented
Public Const HHWIN_BUTTON_INDEX = (2 ^ 14)          ' not implemented
Public Const HHWIN_BUTTON_SEARCH = (2 ^ 15)         ' not implemented
Public Const HHWIN_BUTTON_HISTORY = (2 ^ 16)        ' not implemented
Public Const HHWIN_BUTTON_FAVORITES = (2 ^ 17)      ' not implemented
Public Const HHWIN_BUTTON_JUMP1 = (2 ^ 18)
Public Const HHWIN_BUTTON_JUMP2 = (2 ^ 19)
Public Const HHWIN_BUTTON_ZOOM = (2 ^ 20)
Public Const HHWIN_BUTTON_TOC_NEXT = (2 ^ 21)
Public Const HHWIN_BUTTON_TOC_PREV = (2 ^ 22)

Public Const HHWIN_DEF_BUTTONS = (HHWIN_BUTTON_EXPAND Or HHWIN_BUTTON_BACK Or HHWIN_BUTTON_OPTIONS Or HHWIN_BUTTON_PRINT)

' Button IDs

Public Const IDTB_EXPAND = 200
Public Const IDTB_CONTRACT = 201
Public Const IDTB_STOP = 202
Public Const IDTB_REFRESH = 203
Public Const IDTB_BACK = 204
Public Const IDTB_HOME = 205
Public Const IDTB_SYNC = 206
Public Const IDTB_PRINT = 207
Public Const IDTB_OPTIONS = 208
Public Const IDTB_FORWARD = 209
Public Const IDTB_NOTES = 210           ' not implemented
Public Const IDTB_BROWSE_FWD = 211
Public Const IDTB_BROWSE_BACK = 212
Public Const IDTB_CONTENTS = 213        ' not implemented
Public Const IDTB_INDEX = 214           ' not implemented
Public Const IDTB_SEARCH = 215          ' not implemented
Public Const IDTB_HISTORY = 216         ' not implemented
Public Const IDTB_FAVORITES = 217       ' not implemented
Public Const IDTB_JUMP1 = 218
Public Const IDTB_JUMP2 = 219
Public Const IDTB_CUSTOMIZE = 221

' Notification codes

Public Const HHN_FIRST = -860
Public Const HHN_LAST = -879

Public Const HHN_NAVCOMPLETE = HHN_FIRST
Public Const HHN_TRACK = HHN_FIRST - 1

Type HHN_NOTIFY
    hdr As NMHDR
    pszUrl As String            ' Multi-byte, null-terminated string
End Type

'Type HH_POPUP
'    cbStruct As Long            ' size of this structure
'    hinst As Long               ' instance handle for string resource
'    idString As Long            ' string resource id, or text id if pszFile is specified in HtmlHelp call
'    pszText As String           ' used if idString is zero
'    pt As POINTAPI              ' top center of popup window
'    clrForeground As Long       ' use -1 for default
'    clrBackground As Long       ' use -1 for default
'    rcMargins As RECT           ' amount of space between edges of window and text, -1 for each member to ignore
'    pszFont As String           ' facename, point size, char set, BOLD ITALIC UNDERLINE
'End Type

Type HH_AKLINK
    cbStruct As Long            ' sizeof this structure
    fReserved As Long           ' must be FALSE (really!)
    pszKeywords As String       ' semi-colon separated keywords
    pszUrl As String            ' URL to jump to if no keywords found (may be NULL)
    pszMsgText As String        ' Message text to display in MessageBox if pszUrl is NULL and no keyword match
    pszMsgTitle As String       ' Message text to display in MessageBox if pszUrl is NULL and no keyword match
    pszWindow As String         ' Window to display URL in
    fIndexOnFail As Long        ' Displays index if keyword lookup fails.
End Type

Enum HHACT_
    HHACT_TAB_CONTENTS
    HHACT_TAB_INDEX
    HHACT_TAB_SEARCH
    HHACT_TAB_HISTORY
    HHACT_TAB_FAVORITES
    
    HHACT_EXPAND
    HHACT_CONTRACT
    HHACT_BACK
    HHACT_FORWARD
    HHACT_STOP
    HHACT_REFRESH
    HHACT_HOME
    HHACT_SYNC
    HHACT_OPTIONS
    HHACT_PRINT
    HHACT_HIGHLIGHT
    HHACT_CUSTOMIZE
    HHACT_JUMP1
    HHACT_JUMP2
    HHACT_ZOOM
    HHACT_TOC_NEXT
    HHACT_TOC_PREV
    HHACT_NOTES

    HHACT_LAST_ENUM
End Enum

Enum HHWIN_NAVTYPE_
    HHWIN_NAVTYPE_TOC
    HHWIN_NAVTYPE_INDEX
    HHWIN_NAVTYPE_SEARCH
    HHWIN_NAVTYPE_HISTORY       ' not implemented
    HHWIN_NAVTYPE_FAVORITES     ' not implemented
End Enum

Enum HHWIN_NAVTAB_
    HHWIN_NAVTAB_TOP
    HHWIN_NAVTAB_LEFT
    HHWIN_NAVTAB_BOTTOM
End Enum

Public Const HH_MAX_TABS = 19               ' maximum number of tabs

Enum HH_TAB_
    HH_TAB_CONTENTS
    HH_TAB_INDEX
    HH_TAB_SEARCH
    HH_TAB_HISTORY
    HH_TAB_FAVORITES
End Enum

' HH_DISPLAY_SEARCH Command Related Structures and Constants

Public Const HH_FTS_DEFAULT_PROXIMITY = -1

Type HH_FTS_QUERY
    cbStruct As Long            ' Sizeof structure in bytes.
    fUniCodeStrings As Long     ' TRUE if all strings are unicode.
    pszSearchQuery As String    ' String containing the search query.
    iProximity As Long          ' Word proximity.
    fStemmedSearch As Long      ' TRUE for StemmedSearch only.
    fTitleOnly As Long          ' TRUE for Title search only.
    fExecute As Long            ' TRUE to initiate the search.
    pszWindow As String         ' Window to display in
End Type

Type HH_WINTYPE
    cbStruct As Long            ' IN: size of this structure including all Information Types
    fUniCodeStrings As Long     ' IN/OUT: TRUE if all strings are in UNICODE
    pszType  As String          ' IN/OUT: Name of a type of window
    fsValidMembers As Long      ' IN: Bit flag of valid members (HHWIN_PARAM_)
    fsWinProperties As Long     ' IN/OUT: Properties/attributes of the window (HHWIN_)

    pszCaption As String        ' IN/OUT: Window title
    dwStyles  As Long           ' IN/OUT: Window styles
    dwExStyles As Long          ' IN/OUT: Extended Window styles
    rcWindowPos As RECT         ' IN: Starting position, OUT: current position
    nShowState As Long          ' IN: show state (e.g., SW_SHOW)

    hWndHelp As Long            ' OUT: window handle
    hwndCaller As Long          ' OUT: who called this window

    paInfoTypes As Long         ' IN: Pointer to an array of Information Types

    ' The following members are only valid if HHWIN_PROP_TRI_PANE is set

    hwndToolBar As Long         ' OUT: toolbar window in tri-pane window
    hwndNavigation As Long      ' OUT: navigation window in tri-pane window
    hwndHTML As Long            ' OUT: window displaying HTML in tri-pane window
    iNavWidth As Long           ' IN/OUT: width of navigation window
    rcHTML As RECT              ' OUT: HTML window coordinates

    pszToc As String            ' IN: Location of the table of contents file
    pszIndex As String          ' IN: Location of the index file
    pszFile As String           ' IN: Default location of the html file
    pszHome As String           ' IN/OUT: html file to display when Home button is clicked
    fsToolBarFlags As Long      ' IN: flags controling the appearance of the toolbar
    fNotExpanded As Long        ' IN: TRUE/FALSE to contract or expand, OUT: current state
    curNavType As Long          ' IN/OUT: UI to display in the navigational pane
    tabpos As HHWIN_NAVTAB_     ' IN/OUT: HHWIN_NAVTAB_TOP, HHWIN_NAVTAB_LEFT, or HHWIN_NAVTAB_BOTTOM
    idNotify As Long            ' IN: ID to use for WM_NOTIFY messages
    tabOrder(HH_MAX_TABS) As Byte ' IN/OUT: tab order: Contents, Index, Search, History, Favorites, Reserved 1-5, Custom tabs
    cHistory As Long            ' IN/OUT: number of history items to keep (default is 30)
    pszJump1 As String          ' Text for HHWIN_BUTTON_JUMP1
    pszJump2 As String          ' Text for HHWIN_BUTTON_JUMP2
    pszUrlJump1 As String       ' URL for HHWIN_BUTTON_JUMP1
    pszUrlJump2 As String       ' URL for HHWIN_BUTTON_JUMP2
    rcMinSize As RECT           ' Minimum size for window (ignored in version 1)
    cbInfoTypes As Long         ' size of paInfoTypes;
End Type

Type HHNTRACK
    hdr As NMHDR
    pszCurUrl As String         ' Multi-byte, null-terminated string
    idAction As HHACT_          ' HHACT_ value
    hhWinType As HH_WINTYPE     ' current window type
End Type

Declare Function HtmlHelp Lib "hhctrl.ocx" Alias "HtmlHelpA" (ByVal hwndCaller As Long, ByVal pszFile As String, ByVal uCommand As HH_COMMAND, dwData As Any) As Long
'
' END translation of HtmlHelp.h
'

' HH_WINTYPE_KLUGE required to interpret HH_WINTYPE pointers
' See GetWinTypeFromPointer below
Private Type HH_WINTYPE_KLUGE
    cbStruct As Long            ' IN: size of this structure including all Information Types
    fUniCodeStrings As Long     ' IN/OUT: TRUE if all strings are in UNICODE
    pszType  As Long            ' IN/OUT: Name of a type of window
    fsValidMembers As Long      ' IN: Bit flag of valid members (HHWIN_PARAM_)
    fsWinProperties As Long     ' IN/OUT: Properties/attributes of the window (HHWIN_)

    pszCaption As Long          ' IN/OUT: Window title
    dwStyles  As Long           ' IN/OUT: Window styles
    dwExStyles As Long          ' IN/OUT: Extended Window styles
    rcWindowPos As RECT         ' IN: Starting position, OUT: current position
    nShowState As Long          ' IN: show state (e.g., SW_SHOW)

    hWndHelp As Long            ' OUT: window handle
    hwndCaller As Long          ' OUT: who called this window

    paInfoTypes As Long         ' IN: Pointer to an array of Information Types

    ' The following members are only valid if HHWIN_PROP_TRI_PANE is set

    hwndToolBar As Long         ' OUT: toolbar window in tri-pane window
    hwndNavigation As Long      ' OUT: navigation window in tri-pane window
    hwndHTML As Long            ' OUT: window displaying HTML in tri-pane window
    iNavWidth As Long           ' IN/OUT: width of navigation window
    rcHTML As RECT              ' OUT: HTML window coordinates

    pszToc As Long              ' IN: Location of the table of contents file
    pszIndex As Long            ' IN: Location of the index file
    pszFile As Long             ' IN: Default location of the html file
    pszHome As Long             ' IN/OUT: html file to display when Home button is clicked
    fsToolBarFlags As Long      ' IN: flags controling the appearance of the toolbar
    fNotExpanded As Long        ' IN: TRUE/FALSE to contract or expand, OUT: current state
    curNavType As Long          ' IN/OUT: UI to display in the navigational pane
    tabpos As Long              ' IN/OUT: NAVTAB_TOP, NAVTAB_LEFT, or NAVTAB_BOTTOM
    idNotify As Long            ' IN: ID to use for WM_NOTIFY messages
    tabOrder(HH_MAX_TABS) As Byte ' IN/OUT: tab order: Contents, Index, Search, History, Favorites, Reserved 1-5, Custom tabs
    cHistory As Long            ' IN/OUT: number of history items to keep (default is 30)
    pszJump1 As Long            ' Text for HHWIN_BUTTON_JUMP1
    pszJump2 As Long            ' Text for HHWIN_BUTTON_JUMP2
    pszUrlJump1 As Long         ' URL for HHWIN_BUTTON_JUMP1
    pszUrlJump2 As Long         ' URL for HHWIN_BUTTON_JUMP2
    rcMinSize As RECT           ' Minimum size for window (ignored in version 1)
    cbInfoTypes As Long         ' size of paInfoTypes;
End Type

' HHN_NOTIFY_KLUGE required to interpret HHN_NOTIFY pointers
' See GetHHN_NOTIFYFromPointer below
Private Type HHN_NOTIFY_KLUGE
    hdr As NMHDR
    pszUrl As Long              ' pointer to multi-byte, null-terminated string
End Type

' HHNTRACK_KLUGE required to interpret HHNTRACK pointers
' See GetHHNTRACKFromPointer below
Private Type HHNTRACK_KLUGE
    hdr As NMHDR
    pszCurUrl As Long           ' pointer to multi-byte, null-terminated string
    idAction As HHACT_          ' HHACT_ value
    phhWinType As Long          ' pointer to HH_WINTYPE
End Type

' For use with HH_TP_HELP_CONTEXTMENU and HH_TP_HELP_WM_HELP
Public Type HH_IDPAIR
    dwControlId As Long
    dwTopicId As Long
End Type

' Helper functions
Public Sub CollectIdPairs(frm As Form, IdPairs() As HH_IDPAIR)

    ' Assume every control makes a pair for now
    ReDim IdPairs(frm.Controls.Count * 2)

    Dim i As Integer
    Dim ctl As Object
    Dim hwnd As Long
    On Error Resume Next
    For Each ctl In frm.Controls
        If (ctl.WhatsThisHelpID <> 0) Then
            ' The control has a valid context id
            If (ctl.hwnd <> 0) Then
                ' The control has a window too, so make an ID pair for it
                IdPairs(i).dwControlId = GetDlgCtrlID(ctl.hwnd)
                IdPairs(i).dwTopicId = ctl.WhatsThisHelpID
                i = i + 1
            End If
        End If
    Next ctl
    
    ' Terminate the list
    IdPairs(i).dwControlId = 0
    IdPairs(i).dwTopicId = 0
    
    ' Throw away unused pairs
    ReDim Preserve IdPairs(i)

End Sub

' GetStringFromPointer - copies an ANSI string value from a pointer
' into a VB string
Public Function GetStringFromPointer(lpstr As Long)

    Dim vbstr As String
    
    If (lpstr = 0) Then
        ' NULL pointer, interpret as empty strnig
        vbstr = vbNullString
    Else
        ' Get length of string
        Dim iLength As Long
        iLength = lstrlenp(lpstr)
        
        ' Allocate some space to copy it into
        vbstr = String(iLength, 0)
        
        ' Copy the string data
        ' Note that VB automatically converts from ANSI to Unicode
        Call CopyMemory(ByVal vbstr, ByVal lpstr, iLength)
    End If

    GetStringFromPointer = vbstr

End Function

Public Sub GetWinTypeFromPointer(hhwt As HH_WINTYPE, phhwt As Long)

    If (phhwt = 0) Then
        ' NULL pointer
        Exit Sub
    End If

    ' Copy into the kluge structure
    Dim hhwtk As HH_WINTYPE_KLUGE
    Call CopyMemory(hhwtk, ByVal phhwt, Len(hhwtk))

    ' Now all the members except strings are taken care of
    ' Copy each member to the caller's structure
    hhwt.cbStruct = hhwtk.cbStruct
    hhwt.fUniCodeStrings = hhwtk.fUniCodeStrings
    hhwt.pszType = GetStringFromPointer(hhwtk.pszType)
    hhwt.fsValidMembers = hhwtk.fsValidMembers
    hhwt.fsWinProperties = hhwtk.fsWinProperties
    If (hhwtk.hWndHelp = 0) Then
        ' Window hasn't been created
        ' Copy values from structure
        hhwt.dwStyles = hhwtk.dwStyles
        hhwt.dwExStyles = hhwtk.dwExStyles
        hhwt.pszCaption = GetStringFromPointer(hhwtk.pszCaption)
    Else
        ' Window exists
        ' Copy values from the window itself
        hhwt.dwStyles = GetWindowLong(hhwtk.hWndHelp, GWL_STYLE)
        hhwt.dwExStyles = GetWindowLong(hhwtk.hWndHelp, GWL_EXSTYLE)
        Dim pszCaption As String
        pszCaption = String(255, 0)
        Dim iLength As Long
        iLength = GetWindowText(hhwtk.hWndHelp, pszCaption, 255)
        hhwt.pszCaption = Left(pszCaption, iLength)
    End If
    Call CopyRect(hhwt.rcWindowPos, hhwtk.rcWindowPos)
    hhwt.nShowState = hhwtk.nShowState
    hhwt.hWndHelp = hhwtk.hWndHelp
    hhwt.hwndCaller = hhwtk.hwndCaller
'   NOTE: paInfoTypes is not yet implemented
'   hhwt.paInfoTypes
    hhwt.hwndToolBar = hhwtk.hwndToolBar
    hhwt.hwndNavigation = hhwtk.hwndNavigation
    hhwt.hwndHTML = hhwtk.hwndHTML
    hhwt.iNavWidth = hhwtk.iNavWidth
    Call CopyRect(hhwt.rcHTML, hhwtk.rcHTML)
    hhwt.pszToc = GetStringFromPointer(hhwtk.pszToc)
    hhwt.pszIndex = GetStringFromPointer(hhwtk.pszIndex)
    hhwt.pszFile = GetStringFromPointer(hhwtk.pszFile)
    hhwt.pszHome = GetStringFromPointer(hhwtk.pszHome)
    hhwt.fsToolBarFlags = hhwtk.fsToolBarFlags
    hhwt.fNotExpanded = hhwtk.fNotExpanded
    hhwt.curNavType = hhwtk.curNavType
    hhwt.tabpos = hhwtk.tabpos
    hhwt.idNotify = hhwtk.idNotify
    Call CopyMemory(hhwt.tabOrder(0), hhwtk.tabOrder(0), HH_MAX_TABS)
    hhwt.cHistory = hhwtk.cHistory
    hhwt.pszJump1 = GetStringFromPointer(hhwtk.pszJump1)
    hhwt.pszJump2 = GetStringFromPointer(hhwtk.pszJump2)
    hhwt.pszUrlJump1 = GetStringFromPointer(hhwtk.pszUrlJump1)
    hhwt.pszUrlJump2 = GetStringFromPointer(hhwtk.pszUrlJump2)
    Call CopyRect(hhwt.rcMinSize, hhwtk.rcMinSize)
    hhwt.cbInfoTypes = hhwtk.cbInfoTypes

End Sub

Public Sub GetHHN_NOTIFYFromPointer(hhnn As HHN_NOTIFY, phhnn As Long)

    If (phhnn = 0) Then
        ' NULL pointer
        Exit Sub
    End If

    Dim hhnnk As HHN_NOTIFY_KLUGE
    Call CopyMemory(hhnnk, ByVal phhnn, Len(hhnnk))
    LSet hhnn.hdr = hhnnk.hdr
    hhnn.pszUrl = GetStringFromPointer(hhnnk.pszUrl)

End Sub

Public Sub GetHHNTRACKFromPointer(hhnt As HHNTRACK, phhnt As Long)

    If (phhnt = 0) Then
        ' NULL pointer
        Exit Sub
    End If

    Dim hhntk As HHNTRACK_KLUGE
    Call CopyMemory(hhntk, ByVal phhnt, Len(hhntk))
    LSet hhnt.hdr = hhntk.hdr
    hhnt.idAction = hhntk.idAction
    hhnt.pszCurUrl = GetStringFromPointer(hhntk.pszCurUrl)
    Call GetWinTypeFromPointer(hhnt.hhWinType, hhntk.phhWinType)

End Sub
