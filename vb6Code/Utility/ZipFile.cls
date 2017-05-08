VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "ZipFile"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
Option Explicit
'
'
' Chris Eastwood July 1999 - adapted from code at the
' InfoZip homepage.
'
Public Enum ZTranslate
    CRLFtoLF = 1
    LFtoCRLF = 2
End Enum

Private mCollection As Collection     ' Collection of Files to Zip
Private miRecurseFolders As Integer   ' Recurse Folders ?
Private msZipFileName As String       ' Zip File Name
Private miEncrypt As Integer          ' Encryption ?
Private miSystem As Integer           ' System Files
Private msRootDirectory As String     ' Root Directory
Private miVerbose As Integer          ' Verbose Zip
Private miQuiet As Integer            ' Quiet Zip
Private miTranslateCRLF As ZTranslate ' Translate CRLF / LF Chars
Private miUpdateZip As Integer        ' Updating Existing Zip ?


Private Sub Class_Initialize()
'
' Initialise the collection
'
    Set mCollection = New Collection
'
' We have to add in a dummy file into the collection because
' the Zip routines fall over otherwise.
'
' I think this is a bug, but it's not documented anywhere
' on the InfoZip website.
'
' The Zip process *always* fails on the first file,
' regardless of whether it's a valid file or not!
'
'    mCollection.Add "querty", "querty"
    miEncrypt = 0
    miSystem = 0
    msRootDirectory = "\"
    miQuiet = 0
    miUpdateZip = 0
    
End Sub

Private Sub Class_Terminate()
'
' Terminate the collection
'
    Set mCollection = Nothing
End Sub

Public Property Get RecurseFolders() As Boolean
    RecurseFolders = miRecurseFolders = 1
End Property

Public Property Let RecurseFolders(ByVal bRecurse As Boolean)
    miRecurseFolders = IIf(bRecurse, 1, 0)
End Property

Public Property Get ZipFileName() As String
    ZipFileName = msZipFileName
End Property

Public Property Let ZipFileName(ByVal sZipFileName As String)
    msZipFileName = sZipFileName '& vbNullChar
End Property

Public Property Get Encrypted() As Boolean
    Encrypted = miEncrypt = 1
End Property

Public Property Let Encrypted(ByVal bEncrypt As Boolean)
    miEncrypt = IIf(bEncrypt, 1, 0)
End Property

Public Property Get IncludeSystemFiles() As Boolean
    IncludeSystemFiles = miSystem = 1
End Property

Public Property Let IncludeSystemFiles(ByVal bInclude As Boolean)
    miSystem = IIf(bInclude, 1, 0)
End Property

Public Property Get ZipFileCount() As Long
    If mCollection Is Nothing Then
        ZipFileCount = 0
    Else
        ZipFileCount = mCollection.Count - 1
    End If
End Property


Public Property Get RootDirectory() As String
  RootDirectory = msRootDirectory
End Property
Public Property Let RootDirectory(ByVal sRootDir As String)
  msRootDirectory = sRootDir ' & vbNullChar
End Property

Public Property Get UpdatingZip() As Boolean
  UpdatingZip = miUpdateZip = 1
End Property
Public Property Let UpdatingZip(ByVal bUpdating As Boolean)
  miUpdateZip = IIf(bUpdating, 1, 0)
End Property

Public Function AddFile(ByVal sFilename As String)
  Dim lCount As Long
  Dim sFile As String
  
  On Error Resume Next
  
  sFile = mCollection.Item(sFilename)
  
  If Len(sFile) = 0 Then
    Err.clear
    On Error GoTo 0
    mCollection.Add sFilename, sFilename
  Else
    On Error GoTo 0
    Err.Raise vbObjectError + 2001, "Zip::AddFile", "File is already in Zip List"
  End If
End Function

Public Function RemoveFile(ByVal sFilename As String)
  Dim lCount As Long
  Dim sFile As String
  
  On Error Resume Next
  
  sFile = mCollection.Item(sFilename)
  
  If Len(sFile) = 0 Then
    Err.Raise vbObjectError + 2002, "Zip::RemoveFile", "File is not in Zip List"
  Else
    mCollection.Remove sFilename
  End If
End Function

Public Function MakeZipFile() As Long
  Dim zFileArray As ZIPnames
  Dim sFilename As Variant
  Dim lFileCount As Long
  Dim iIgnorePath As Integer
  Dim iRecurse As Integer

'On Error GoTo vbErrorHandler


  
  lFileCount = 0
  
  For Each sFilename In mCollection
    zFileArray.s(lFileCount) = sFilename
    lFileCount = lFileCount + 1
  Next

      
  MakeZipFile = VBZip( _
      CInt(lFileCount), _
      msZipFileName, _
      zFileArray, _
      iIgnorePath, _
      miRecurseFolders, _
      miUpdateZip, _
      0, msRootDirectory)

  Exit Function

vbErrorHandler:
  MakeZipFile = -99
  Err.Raise Err.Number, "ZipFile::MakeZipFile", Err.Description

End Function

Public Function GetLastMessage() As String
    GetLastMessage = msOutput
End Function
