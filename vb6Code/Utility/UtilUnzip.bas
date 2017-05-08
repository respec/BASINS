Attribute VB_Name = "UtilUnzip"
Option Explicit
'-----------------------------------------------------
' Sample VB 5 code to drive unzip32.dll
' Contributed to the Info-Zip project by Mike Le Voi
'
' Contact me at: mlevoi@modemss.brisnet.org.au
'
' Visit my home page at: http://modemss.brisnet.org.au/~mlevoi
'
' Use this code at your own risk. Nothing implied or warranted
' to work on your machine :-)
'-----------------------------------------------------

Private Const CBCHARLEN As Long = 32800

Private pManager As clsWebDataManager

' argv
Private Type ZIPnames
    s(0 To 99) As String
End Type

' Callback large "string" (sic)
Private Type CBChar
    ch(32800) As Byte
End Type

' Callback small "string" (sic)
Private Type CBCh
    ch(256) As Byte
End Type

' DCL structure
Private Type DCLIST
    ExtractOnlyNewer As Long
    SpaceToUnderscore As Long
    PromptToOverwrite As Long
    fQuiet As Long
    ncflag As Long
    ntflag As Long
    nvflag As Long
    nUflag As Long
    nzflag As Long
    ndflag As Long
    noflag As Long
    naflag As Long
    nZIflag As Long
    C_flag As Long
    fPrivilege As Long
    Zip As String
    ExtractDir As String
End Type

' Userfunctions structure
Private Type USERFUNCTION
    DllPrnt As Long
    DLLSND As Long
    DLLREPLACE As Long
    DLLPASSWORD As Long
    DLLMESSAGE As Long
    DLLSERVICE As Long
    TotalSizeComp As Long
    TotalSize As Long
    CompFactor As Long
    NumMembers As Long
    cchComment As Integer
End Type

' Unzip32.dll version structure
Private Type UZPVER
    structlen As Long
    flag As Long
    beta As String * 10
    date As String * 20
    zlib As String * 10
    unzip(1 To 4) As Byte
    zipinfo(1 To 4) As Byte
    os2dll As Long
    windll(1 To 4) As Byte
End Type

' This assumes unzip32.dll is in
' your \windows\system directory!
Private Declare Function windll_unzip Lib "unzip32.dll" _
    (ByVal ifnc As Long, ByRef ifnv As ZIPnames, _
     ByVal xfnc As Long, ByRef xfnv As ZIPnames, _
     dcll As DCLIST, Userf As USERFUNCTION) As Long

Private Declare Sub UzpVersion2 Lib "unzip32.dll" _
    (uzpv As UZPVER)

' Private structures
'Dim MYDCL As DCLIST
'Dim MYUSER As USERFUNCTION
'Dim MYVER As UZPVER

Private vbzipnum As Long
Private vbzipmes As String
Private vbzipinf As String

' Puts a function pointer in a structure
Function FnPtr(ByVal lp As Long) As Long
    FnPtr = lp
End Function

' Callback for unzip32.dll
Sub ReceiveDllMessage(ByVal ucsize As Long, _
    ByVal csiz As Long, _
    ByVal cfactor As Integer, _
    ByVal mo As Integer, _
    ByVal dy As Integer, _
    ByVal yr As Integer, _
    ByVal hh As Integer, _
    ByVal mm As Integer, _
    ByVal c As Byte, ByRef fname As CBCh, _
    ByRef meth As CBCh, ByVal crc As Long, _
    ByVal fCrypt As Byte)

    Dim strout As String * 80

    On Error Resume Next  ' always put this in callback routines!
    strout = Space(80)
    If vbzipnum = 0 Then
        Mid$(strout, 1, 50) = "File"
        Mid$(strout, 53, 4) = "Size"
        Mid$(strout, 62, 4) = "Date"
        Mid$(strout, 71, 4) = "Time"
        If Not pManager Is Nothing Then pManager.LogDbg strout
        vbzipmes = strout + vbCrLf
        strout = Space(80)
    End If
    Mid$(strout, 1, 50) = Right(BytesToString(fname.ch, 255), 50)
    Mid$(strout, 51, 7) = Right("        " + str$(ucsize), 7)
    Mid$(strout, 60, 3) = Right("0" & str$(dy), 2) + "/"
    Mid$(strout, 63, 3) = Right("0" + Trim$(str$(mo)), 2) + "/"
    Mid$(strout, 66, 2) = Right("0" + Trim$(str$(yr)), 2)
    Mid$(strout, 70, 3) = Right(str$(hh), 2) + ":"
    Mid$(strout, 73, 2) = Right("0" + Trim$(str$(mm)), 2)
    ' Mid$(strout, 75, 2) = Right$(" " + Str$(cfactor), 2)
    ' Mid$(strout, 78, 8) = Right$("        " + Str$(csiz), 8)
    ' s0 = CBCharToString(meth, 255)
    If Not pManager Is Nothing Then pManager.LogDbg strout
    vbzipmes = vbzipmes + strout + vbCrLf
    vbzipnum = vbzipnum + 1
End Sub

' Callback for unzip32.dll
Function DllPrnt(ByRef fname As CBChar, ByVal x As Long) As Long
    On Error Resume Next  ' always put this in callback routines!
    vbzipinf = vbzipinf + BytesToString(fname.ch, x)
    DllPrnt = 0
End Function

' Callback for unzip32.dll
Function DllPass(ByRef s1 As Byte, x As Long, _
    ByRef s2 As Byte, _
    ByRef s3 As Byte) As Long

    On Error Resume Next  ' always put this in callback routines!
    DllPass = 1           ' DllPass not supported - always return 1

End Function

' Callback for unzip32.dll
Function DllRep(ByRef fname As CBChar) As Long
    On Error Resume Next  ' always put this in callback routines!
    Select Case MsgBox("Overwrite " & BytesToString(fname.ch, 255) & "?", vbYesNoCancel, "Unzip - File already exists")
      Case vbNo:     DllRep = 100 ' 100=do not overwrite - keep asking user
      Case vbCancel: DllRep = 104 ' 104=overwrite none
      Case Else:     DllRep = 102 ' 102=overwrite 103=overwrite all
    End Select
End Function

'return zero-terminated string up to maxLen characters long in byt
Private Function BytesToString(byt() As Byte, Optional maxLen As Long = CBCHARLEN) As String
  Dim i As Long
  While i <= maxLen
    If byt(i) = 0 Then
      Debug.Print BytesToString
      Exit Function
    Else
      BytesToString = BytesToString & Chr(byt(i))
      i = i + 1
    End If
  Wend
End Function

' ASCIIZ to String
Private Function szTrim(szString As String) As String
    Dim pos As Integer, ln As Integer

    pos = InStr(szString, Chr$(0))
    ln = Len(szString)
    Select Case pos
        Case Is > 1
            szTrim = Trim(Left(szString, pos - 1))
        Case 1
            szTrim = ""
        Case Else
            szTrim = Trim(szString)
    End Select
End Function

Private Sub UnzipExample()
  Dim zipfile$, unzipdir$
  
  ' Init global message variables
  vbzipinf = ""
  vbzipnum = 0
        
  ' Change the next 2 lines as required!
  zipfile = "c:\test\test.zip"
  unzipdir = "C:\test\ziptest"
  
  ' Let's go for it
  Call VBUnzip(zipfile, unzipdir, False, False, True, Nothing)
  
  ' Tell the user what happened
  If Len(vbzipmes) > 0 Then Debug.Print vbzipmes
  If Len(vbzipinf) > 0 Then
    Debug.Print "vbzipinf is:"
    Debug.Print vbzipinf
  End If
  If vbzipnum > 0 Then Debug.Print "Number of files: " & vbzipnum
End Sub

' Main subroutine
Function VBUnzip(zipFilename As String, _
            extractDirectory As String, _
            PromptToOverwrite As Boolean, _
            ListOnly As Boolean, _
            CreateDirs As Boolean, _
            manager As clsWebDataManager) As String
  Dim retcode As Long
  Dim MYDCL As DCLIST
  Dim MYUSER As USERFUNCTION
  
  Dim IncludeFiles As ZIPnames
  Dim ExcludeFiles As ZIPnames
  Dim numIncludeFiles As Long
  Dim numExcludeFiles As Long

  If Not IsNull(manager) Then Set pManager = manager

'    ' Select filenames if required
'    '   IncludeFiles.s(0) = "sfx16.dat"
'    '   IncludeFiles.s(1) = "sfx32.dat"
'    '   IncludeFiles.s(2) = "windll.h"
'    '   numIncludeFiles = 3
'    ' or just select all files
    IncludeFiles.s(0) = vbNullString
    numIncludeFiles = 0
'
'    ' Select filenames to exclude from processing
'    ' Note UNIX convention!
'    '   ExcludeFiles.s(0) = "VBSYX/VBSYX.MID"
'    '   ExcludeFiles.s(1) = "VBSYX/VBSYX.SYX"
'    '   numExcludeFiles = 2
'    ' or just select all files
    ExcludeFiles.s(0) = vbNullString
    numExcludeFiles = 0
  
  ' Set options
  If PromptToOverwrite Then
    MYDCL.PromptToOverwrite = 1   ' 1=prompt to overwrite required
    MYDCL.noflag = 1              ' 1=overwrite files
  End If
  MYDCL.ExtractOnlyNewer = 0      ' 1=extract only newer
  MYDCL.SpaceToUnderscore = 0     ' 1=convert space to underscore
  MYDCL.fQuiet = 0                ' 2=no messages 1=less 0=all
  MYDCL.ncflag = 0                ' 1=write to stdout
  MYDCL.ntflag = 0                ' 1=test zip
  If ListOnly Then
    MYDCL.nvflag = 1              ' 1=list contents
  Else
    MYDCL.nvflag = 0              ' 0=extract
  End If
  MYDCL.nUflag = 0                ' 1=extract only newer
  MYDCL.nzflag = 0                ' 1=display zip file comment
  If CreateDirs Then
    MYDCL.ndflag = 1              ' 1=honour directories
  Else
    MYDCL.ndflag = 0
  End If
  MYDCL.naflag = 0                ' 1=convert CR to CRLF
  MYDCL.nZIflag = 0               ' 1=Zip Info Verbose
  MYDCL.C_flag = 0                ' 1=Case insensitivity, 0=Case Sensitivity
  MYDCL.fPrivilege = 0            ' 1=ACL 2=priv
  MYDCL.Zip = zipFilename         ' ZIP name
  MYDCL.ExtractDir = extractDirectory ' Extraction directory, NULL for current directory
  ' Set Callback addresses
  ' Do not change
  MYUSER.DllPrnt = FnPtr(AddressOf DllPrnt)
  MYUSER.DLLSND = 0& ' not supported
  MYUSER.DLLREPLACE = FnPtr(AddressOf DllRep)
  MYUSER.DLLPASSWORD = FnPtr(AddressOf DllPass)
  MYUSER.DLLMESSAGE = FnPtr(AddressOf ReceiveDllMessage)
  MYUSER.DLLSERVICE = 0& ' not coded yet :)
      
  ' Go for it!
  retcode = windll_unzip(numIncludeFiles, IncludeFiles, _
                         numExcludeFiles, ExcludeFiles, _
                         MYDCL, MYUSER)
  If retcode <> 0 Then
    VBUnzip = "Unzip return code " & retcode
  End If
    
End Function
