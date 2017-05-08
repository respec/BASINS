VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "UnzipFile"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
Option Explicit
'
' UnZip Class
'
' Chris Eastwood July 1999
'
Public Enum ZMessageLevel
    All = 0
    Less = 1
    NoMessages = 2
End Enum
Public Enum ZExtractType
    Extract = 0
    ListContents = 1
End Enum
Public Enum ZPrivilege
    Ignore = 0
    ACL = 1
    Privileges = 2
End Enum

Private miExtractNewer     As Integer  ' 1 = Extract Only Newer, Else 0
Private miSpaceUnderScore  As Integer  ' 1 = Convert Space To Underscore, Else 0
Private miPromptOverwrite  As Integer  ' 1 = Prompt To Overwrite Required, Else 0
Private miQuiet            As ZMessageLevel  ' 2 = No Messages, 1 = Less, 0 = All
Private miWriteStdOut      As Integer  ' 1 = Write To Stdout, Else 0
Private miTestZip          As Integer  ' 1 = Test Zip File, Else 0
Private miExtractList      As ZExtractType  ' 0 = Extract, 1 = List Contents
Private miExtractOnlyNewer As Integer  ' 1 = Extract Only Newer, Else 0
Private miDisplayComment   As Integer  ' 1 = Display Zip File Comment, Else 0
Private miHonorDirectories As Integer  ' 1 = Honor Directories, Else 0
Private miOverWriteFiles   As Integer  ' 1 = Overwrite Files, Else 0
Private miConvertCR_CRLF   As Integer  ' 1 = Convert CR To CRLF, Else 0
Private miVerbose          As Integer  ' 1 = Zip Info Verbose
Private miCaseSensitivity  As Integer  ' 1 = Case Insensitivity, 0 = Case Sensitivity
Private miPrivilege        As ZPrivilege  ' 1 = ACL, 2 = Privileges, Else 0
Private msZipFileName      As String   ' The Zip File Name
Private msExtractDir       As String   ' Extraction Directory, Null If Current Directory

Public Property Get ExtractNewer() As Boolean
    ExtractNewer = miExtractNewer = 1
End Property

Public Property Let ExtractNewer(ByVal bExtractNewer As Boolean)
    miExtractNewer = IIf(bExtractNewer, 1, 0)
End Property

Public Property Get SpaceToUnderScore() As Boolean
    SpaceToUnderScore = miSpaceUnderScore = 1
End Property

Public Property Let SpaceToUnderScore(ByVal bConvert As Boolean)
    miSpaceUnderScore = IIf(bConvert, 1, 0)
End Property

Public Property Get PromptOverwrite() As Boolean
    PromptOverwrite = miPromptOverwrite = 1
End Property

Public Property Let PromptOverwrite(ByVal bPrompt As Boolean)
    miPromptOverwrite = IIf(bPrompt, 1, 0)
End Property

Public Property Get MessageLevel() As ZMessageLevel
    MessageLevel = miQuiet
End Property

Public Property Let MessageLevel(ByVal iLevel As ZMessageLevel)
    miQuiet = iLevel
End Property

Public Property Get WriteToStdOut() As Boolean
    WriteToStdOut = miWriteStdOut = 1
End Property

Public Property Let WriteToStdOut(ByVal bWrite As Boolean)
    miWriteStdOut = IIf(bWrite, 1, 0)
End Property

Public Property Get TestZip() As Boolean
    TestZip = miTestZip = 1
End Property

Public Property Let TestZip(ByVal bTest As Boolean)
    miTestZip = IIf(bTest, 1, 0)
End Property

Public Property Get ExtractList() As ZExtractType
    ExtractList = miExtractList
End Property

Public Property Let ExtractList(ByVal zExType As ZExtractType)
    miExtractList = zExType
End Property

Public Property Get ExtractOnlyNewer() As Boolean
    ExtractOnlyNewer = miExtractOnlyNewer = 1
End Property

Public Property Let ExtractOnlyNewer(ByVal bOnlyNewer As Boolean)
    miExtractOnlyNewer = IIf(bOnlyNewer, 1, 0)
End Property

Public Property Get DisplayComment() As Boolean
    DisplayComment = miDisplayComment = 1
End Property

Public Property Let DisplayComment(ByVal bDisplay As Boolean)
    miDisplayComment = IIf(bDisplay, 1, 0)
End Property

Public Property Get HonorDirectories() As Boolean
    HonorDirectories = miHonorDirectories = 1
End Property

Public Property Let HonorDirectories(ByVal bHonor As Boolean)
    miHonorDirectories = IIf(bHonor, 1, 0)
End Property

Public Property Get OverWriteFiles() As Boolean
    OverWriteFiles = miOverWriteFiles = 1
End Property

Public Property Let OverWriteFiles(ByVal bOverWrite As Boolean)
    miOverWriteFiles = IIf(bOverWrite, 1, 0)
End Property

Public Property Get ConvertCRtoCRLF() As Boolean
    ConvertCRtoCRLF = miConvertCR_CRLF = 1
End Property

Public Property Let ConvertCRtoCRLF(ByVal bConvert As Boolean)
    miConvertCR_CRLF = IIf(bConvert, 1, 0)
End Property

Public Property Get Verbose() As Boolean
    Verbose = miVerbose = 1
End Property

Public Property Let Verbose(ByVal bVerbose As Boolean)
    miVerbose = IIf(bVerbose, 1, 0)
End Property

Public Property Get CaseSensitive() As Boolean
    CaseSensitive = miCaseSensitivity = 1
End Property

Public Property Let CaseSensitive(ByVal bCaseSensitive As Boolean)
    miCaseSensitivity = IIf(bCaseSensitive, 1, 0)
End Property

Public Property Get Privilege() As ZPrivilege
    Privilege = miPrivilege
End Property

Public Property Let Privilege(ByVal zPriv As ZPrivilege)
    miPrivilege = zPriv
End Property

Public Property Get ZipFileName() As String
    ZipFileName = msZipFileName
End Property

Public Property Let ZipFileName(ByVal sZipFileName As String)
    msZipFileName = sZipFileName
End Property

Public Property Get ExtractDir() As String
    ExtractDir = msExtractDir
End Property

Public Property Let ExtractDir(ByVal sExtractDir As String)
    msExtractDir = sExtractDir
End Property

Public Function Unzip(Optional sZipFileName As String, _
    Optional sExtractDir As String) As Long
    
On Error GoTo vbErrorHandler

    Dim lRet As Long
    
    If Len(sZipFileName) > 0 Then
        msZipFileName = sZipFileName
    End If
    
    If Len(sExtractDir) > 0 Then
        msExtractDir = sExtractDir
    End If
    
    
    lRet = VBUnzip(msZipFileName, msExtractDir, miExtractNewer, _
        miSpaceUnderScore, miPromptOverwrite, CInt(miQuiet), _
        miWriteStdOut, miTestZip, CInt(miExtractList), _
        miExtractOnlyNewer, miDisplayComment, miHonorDirectories, _
        miOverWriteFiles, miConvertCR_CRLF, miVerbose, _
        miCaseSensitivity, CInt(miPrivilege))
    
    Unzip = lRet
    
    Exit Function

vbErrorHandler:
    Err.Raise Err.Number, "UnZipFile::Unzip", Err.Description

End Function

Private Sub Class_Initialize()
    miExtractNewer = 0
    miSpaceUnderScore = 0
    miPromptOverwrite = 0
    miQuiet = NoMessages
    miWriteStdOut = 0
    miTestZip = 0
    miExtractList = Extract
    miExtractOnlyNewer = 0
    miDisplayComment = 0
    miHonorDirectories = 1
    miOverWriteFiles = 1
    miConvertCR_CRLF = 0
    miVerbose = 0
    miCaseSensitivity = 1
    miPrivilege = Ignore
End Sub

Public Function GetLastMessage() As String
    GetLastMessage = msOutput
End Function



