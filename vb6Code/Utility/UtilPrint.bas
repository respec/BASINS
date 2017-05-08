Attribute VB_Name = "UtilPrint"
Option Explicit
'##MODULE_REMARKS Copyright 2001-3 AQUA TERRA Consultants - Royalty-free use permitted under open source license

' Global constants for Win32 API
Public Const CCHDEVICENAME = 32
Public Const CCHFORMNAME = 32
Public Const GMEM_FIXED = &H0
Public Const GMEM_MOVEABLE = &H2
Public Const GMEM_ZEROINIT = &H40
Public Const DM_DUPLEX = &H1000&
Public Const DM_ORIENTATION = &H1&
Public Const PD_ALLPAGES = &H0
Public Const PD_COLLATE = &H10
Public Const PD_DISABLEPRINTTOFILE = &H80000
Public Const PD_ENABLEPRINTHOOK = &H1000
Public Const PD_ENABLEPRINTTEMPLATE = &H4000
Public Const PD_ENABLEPRINTTEMPLATEHANDLE = &H10000
Public Const PD_ENABLESETUPHOOK = &H2000
Public Const PD_ENABLESETUPTEMPLATE = &H8000
Public Const PD_ENABLESETUPTEMPLATEHANDLE = &H20000
Public Const PD_HIDEPRINTTOFILE = &H100000
Public Const PD_NONETWORKBUTTON = &H200000
Public Const PD_NOPAGENUMS = &H8
Public Const PD_NOSELECTION = &H4
Public Const PD_NOWARNING = &H80
Public Const PD_PAGENUMS = &H2
Public Const PD_PRINTSETUP = &H40
Public Const PD_PRINTTOFILE = &H20
Public Const PD_RETURNDC = &H100
Public Const PD_RETURNDEFAULT = &H400
Public Const PD_RETURNIC = &H200
Public Const PD_SELECTION = &H1
Public Const PD_SHOWHELP = &H800
Public Const PD_USEDEVMODECOPIES = &H40000
Public Const PD_USEDEVMODECOPIESANDCOLLATE = &H40000

'Custom Global Constants
Public Const DLG_PRINT = 0
Public Const DLG_PRINTSETUP = 1

'type definitions:
Type PRINTDLG_TYPE
    lStructSize As Long
    hwndOwner As Long
    hDevMode As Long
    hDevNames As Long
    hdc As Long
    flags As Long
    nFromPage As Integer
    nToPage As Integer
    nMinPage As Integer
    nMaxPage As Integer
    nCopies As Integer
    hInstance As Long
    lCustData As Long
    lpfnPrintHook As Long
    lpfnSetupHook As Long
    lpPrintTemplateName As String
    lpSetupTemplateName As String
    hPrintTemplate As Long
    hSetupTemplate As Long
End Type

Type DEVNAMES_TYPE
    wDriverOffset As Integer
    wDeviceOffset As Integer
    wOutputOffset As Integer
    wDefault As Integer
    extra As String * 100
End Type

Type DEVMODE_TYPE
    dmDeviceName As String * CCHDEVICENAME
    dmSpecVersion As Integer
    dmDriverVersion As Integer
    dmSize As Integer
    dmDriverExtra As Integer
    dmFields As Long
    dmOrientation As Integer
    dmPaperSize As Integer
    dmPaperLength As Integer
    dmPaperWidth As Integer
    dmScale As Integer
    dmCopies As Integer
    dmDefaultSource As Integer
    dmPrintQuality As Integer
    dmColor As Integer
    dmDuplex As Integer
    dmYResolution As Integer
    dmTTOption As Integer
    dmCollate As Integer
    dmFormName As String * CCHFORMNAME
    dmUnusedPadding As Integer
    dmBitsPerPel As Integer
    dmPelsWidth As Long
    dmPelsHeight As Long
    dmDisplayFlags As Long
    dmDisplayFrequency As Long
End Type

Type DOCINFO
    cbSize As Long
    lpszDocName As String
    lpszOutput As String
    lpszDatatype As String
    fwType As Long
End Type

'API declarations:
Public Declare Function PrintDialog Lib "comdlg32.dll" _
    Alias "PrintDlgA" (pPrintdlg As PRINTDLG_TYPE) As Long

Public Declare Function GlobalLock Lib "Kernel32" _
         (ByVal hMem As Long) As Long

Public Declare Function GlobalUnlock Lib "Kernel32" _
         (ByVal hMem As Long) As Long

Public Declare Function GlobalAlloc Lib "Kernel32" _
         (ByVal wFlags As Long, ByVal dwBytes As Long) As Long

Public Declare Function GlobalFree Lib "Kernel32" _
         (ByVal hMem As Long) As Long
         
Declare Function EndPage Lib "gdi32" (ByVal hdc As Long) As Long
Declare Function EndDoc Lib "gdi32" (ByVal hdc As Long) As Long
Declare Function StartDoc Lib "gdi32" Alias "StartDocA" (ByVal hdc As Long, lpdi As DOCINFO) As Long
Declare Function StartPage Lib "gdi32" (ByVal hdc As Long) As Long
'not same as API !!
Declare Function CreateDC Lib "gdi32" Alias "CreateDCA" (ByVal lpDriverName As String, ByVal lpDeviceName As String, ByVal lpOutput As Long, lpInitData As Long) As Long
Declare Function DeleteDC Lib "gdi32" (ByVal hdc As Long) As Long

Public Sub ShowPrinterX(frmOwner As Form, _
         lFromPage&, lToPage&, lMinPage&, lMaxPage&, _
         Optional PrintFlags As Long = PD_NOSELECTION + PD_NOPAGENUMS + PD_DISABLEPRINTTOFILE)

    Dim PrintDlg As PRINTDLG_TYPE
    Dim DEVMODE As DEVMODE_TYPE
    Dim DevName As DEVNAMES_TYPE
          
    Dim lpDevMode As Long, lpDevName As Long
    Dim bReturn As Integer
    Dim objPrinter As Printer, NewPrinterName As String
    Dim strSetting As String

    ' Use PrintDialog to get the handle to a memory
    ' block with a DevMode and DevName structures

    PrintDlg.lStructSize = Len(PrintDlg)
    PrintDlg.hwndOwner = frmOwner.hwnd

    PrintDlg.nFromPage = lFromPage
    PrintDlg.nToPage = lToPage
    PrintDlg.nMinPage = lMinPage
    PrintDlg.nMaxPage = lMaxPage
    PrintDlg.flags = PrintFlags

    'Set the current orientation and duplex setting
    DEVMODE.dmDeviceName = Printer.DeviceName
    DEVMODE.dmSize = Len(DEVMODE)
    DEVMODE.dmFields = DM_ORIENTATION Or DM_DUPLEX
    DEVMODE.dmOrientation = Printer.Orientation
    On Error Resume Next
    DEVMODE.dmDuplex = Printer.Duplex
    DEVMODE.dmPrintQuality = Printer.PrintQuality
    
    On Error GoTo 0

    'Allocate memory for the initialization hDevMode structure
    'and copy the settings gathered above into this memory
    PrintDlg.hDevMode = GlobalAlloc(GMEM_MOVEABLE Or _
                                    GMEM_ZEROINIT, Len(DEVMODE))
    lpDevMode = GlobalLock(PrintDlg.hDevMode)
    If lpDevMode > 0 Then
      CopyMemory ByVal lpDevMode, DEVMODE, Len(DEVMODE)
      bReturn = GlobalUnlock(lpDevMode)
    End If

    'Set the current driver, device, and port name strings
    With DevName
      .wDriverOffset = 8
      .wDeviceOffset = .wDriverOffset + 1 + Len(Printer.DriverName)
      .wOutputOffset = .wDeviceOffset + 1 + Len(Printer.Port)
      .wDefault = 0
    End With
    
    With Printer
      DevName.extra = .DriverName & Chr(0) & _
                      .DeviceName & Chr(0) & .Port & Chr(0)
    End With

    'Allocate memory for the initial hDevName structure
    'and copy the settings gathered above into this memory
    PrintDlg.hDevNames = GlobalAlloc(GMEM_MOVEABLE Or _
                         GMEM_ZEROINIT, Len(DevName))
    lpDevName = GlobalLock(PrintDlg.hDevNames)
    If lpDevName > 0 Then
      CopyMemory ByVal lpDevName, DevName, Len(DevName)
      bReturn = GlobalUnlock(lpDevName)
    End If

    'Call the print dialog up and let the user make changes
    If PrintDialog(PrintDlg) Then
      'First get the DevName structure.
      lpDevName = GlobalLock(PrintDlg.hDevNames)
      CopyMemory DevName, ByVal lpDevName, 45
      bReturn = GlobalUnlock(lpDevName)
      GlobalFree PrintDlg.hDevNames

      'Next get the DevMode structure and set the printer
      'properties appropriately
      lpDevMode = GlobalLock(PrintDlg.hDevMode)
      CopyMemory DEVMODE, ByVal lpDevMode, Len(DEVMODE)
      bReturn = GlobalUnlock(PrintDlg.hDevMode)
      GlobalFree PrintDlg.hDevMode
      
      NewPrinterName = UCase$(Left(DEVMODE.dmDeviceName, _
                       InStr(DEVMODE.dmDeviceName, Chr$(0)) - 1))
      If Printer.DeviceName <> NewPrinterName Then
        For Each objPrinter In Printers
           If UCase$(objPrinter.DeviceName) = NewPrinterName Then
             Set Printer = objPrinter
           End If
        Next
      End If
      
      On Error Resume Next

      'Set printer object properties according to selections made
      'by user
      With Printer
        .Copies = DEVMODE.dmCopies
        .Duplex = DEVMODE.dmDuplex
        .Orientation = DEVMODE.dmOrientation
        '.PaperSize = DEVMODE.dmPaperSize
        '.Width = DEVMODE.dmPaperWidth
        '.Height = DEVMODE.dmPaperLength
        '.PrintQuality = DEVMODE.dmPrintQuality
      End With
      On Error GoTo 0
      
      If CBool(PrintDlg.flags And PD_PAGENUMS) Then 'some pages
        lFromPage = PrintDlg.nFromPage
        lToPage = PrintDlg.nToPage
      Else ' all pages
        lFromPage = lMinPage
        lToPage = lMaxPage
      End If
    Else 'cancel
      lToPage = -1
    End If

    'Display the results in the immediate (debug) window
    'With Printer
    '  If .Orientation = 1 Then
    '    strSetting = "Portrait. "
    '  Else
    '    strSetting = "Landscape. "
    '  End If
    '  Debug.Print "Copies = " & .Copies, "Orientation = " & strSetting
    'End With
    
End Sub
