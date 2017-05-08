VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "ATCoFindFile"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
Private pAppName As String
Private pRegistrySection As String
Private pRegistryKey As String
Private pRegistry As ATCoRegistry
Private Const pLocalMachinePrefix As String = "SOFTWARE\Aqua Terra Consultants\"
Private pFileDialogTitle As String
Private pDefaultFileName As String
Private pDefaultExt As String
Private pFileFilter As String

Public Sub SetRegistryInfo(newAppName As String, _
                           newSection As String, _
                           Optional newKey As String = "")
                           
  pAppName = newAppName
  pRegistrySection = newSection
  pRegistryKey = newKey
End Sub

Public Sub SetDialogProperties(nFileDialogTitle As String, _
                               Optional nDefaultFileName As String = " ", _
                               Optional nDefaultExt As String = "", _
                               Optional nFileFilter As String = "")
                                                                
  pFileDialogTitle = nFileDialogTitle
  pDefaultFileName = nDefaultFileName
  pDefaultExt = nDefaultExt
  pFileFilter = nFileFilter
  If (Len(pDefaultExt)) = 0 Then 'try to get from default name
    pDefaultExt = FileExt(pDefaultFileName)
  End If
  If (Len(pFileFilter)) = 0 Then 'try to get from default ext
    If Len(pDefaultExt) > 0 Then
      pFileFilter = pDefaultExt & " Files (*." & pDefaultExt & ")|*." & pDefaultExt & "|"
    End If
    pFileFilter = pFileFilter & "All files (*.*)|*.*"
  End If
  
End Sub

Public Function GetName(Optional lUserVerifyFileName As Boolean = False, _
                        Optional lChDir2FileDir As Boolean = False) As String
                        
  Dim lDir As String
  Dim baseFileName As String 'file name (with no path) of file we are looking for
  Dim lFileName As String
  Dim lRegistryFileName As String
  Dim lFileNameFoundInRegistry As Boolean
  Dim LookingForDir As Boolean

  Dim hdle&
  Dim S As String * 80
  Dim EXEName As String
  
  lDir = CurDir
  lFileName = Trim(pDefaultFileName)
  
  On Error Resume Next
  
  If Right(lFileName, 1) = "\" Then
    LookingForDir = True
    If Len(lFileName) = 1 Then lFileName = ""
  End If
  
  If Not FileExists(lFileName, True) Then 'don't already know where it is, first look in registry
    If Len(pRegistrySection) > 0 Then
      lRegistryFileName = GetSetting(pAppName, pRegistrySection, pRegistryKey, "")
      If Not FileExists(lRegistryFileName, True) Then
        lRegistryFileName = pRegistry.RegGetString(HKEY_LOCAL_MACHINE, pLocalMachinePrefix & pAppName & "\" & pRegistrySection, pRegistryKey)
      End If
    End If
    If Len(lRegistryFileName) > 0 Then
      If FileExists(lRegistryFileName, True) Then 'got from registry
        lFileName = lRegistryFileName
        lFileNameFoundInRegistry = True
        If lChDir2FileDir Then
          ChDriveDir PathNameOnly(lRegistryFileName)
        End If
      Else 'bad name in registry, message to user needed?
      End If
    End If
  End If
  If Not FileExists(lFileName, True) Then 'try some default locations if filename was specified, but not path
    If LookingForDir Then
      baseFileName = FilenameNoPath(Left(lFileName, Len(lFileName) - 1))
    Else
      baseFileName = FilenameNoPath(lFileName)
    End If
    If Len(baseFileName) > 0 Then
      lFileName = pDefaultFileName
      If Not FileExists(lFileName, True) Then lFileName = "c:\" & baseFileName
      If Not FileExists(lFileName, True) Then lFileName = "c:\winnt\" & baseFileName
      If Not FileExists(lFileName, True) Then lFileName = "c:\winnt\system\" & baseFileName
      If Not FileExists(lFileName, True) Then lFileName = "c:\winnt\system32\" & baseFileName
      If Not FileExists(lFileName, True) Then lFileName = "c:\windows\" & baseFileName
      If Not FileExists(lFileName, True) Then lFileName = "c:\windows\system\" & baseFileName
      If Not FileExists(lFileName, True) Then lFileName = "c:\windows\system32\" & baseFileName
      If Not FileExists(lFileName, True) Then lFileName = "c:\BASINS\models\HSPF\bin\" & baseFileName
      If Not FileExists(lFileName, True) Then lFileName = "c:\BASINS\models\HSPF\WDMUtil\" & baseFileName
      If Not FileExists(lFileName, True) Then lFileName = "d:\BASINS\models\HSPF\bin\" & baseFileName
      If Not FileExists(lFileName, True) Then lFileName = "d:\BASINS\models\HSPF\WDMUtil\" & baseFileName
      If Not FileExists(lFileName, True) Then 'Look in directory containing current .exe
        hdle = GetModuleHandle(Null)
        GetModuleFileName hdle, S, 80
        If hdle <> Null Then
          EXEName = UCase(Left(S, InStr(S, Chr(0)) - 1))
          lFileName = PathNameOnly(EXEName) & "\" & baseFileName
        End If
      End If
      If FileExists(lFileName, True) Then
        pDefaultFileName = lFileName
      Else
        lFileName = ""
      End If
    End If
  End If
  
  If Not FileExists(lFileName, True) Or lUserVerifyFileName Then 'ask the user
    On Error GoTo CancelledDialog
    With frmDummy.cdlg
      .DialogTitle = pFileDialogTitle
      lFileName = AbsolutePath(lFileName, CurDir)
      .Filename = pDefaultFileName
      .filter = pFileFilter
      .FilterIndex = 1
      .CancelError = True
      .DefaultExt = pDefaultExt
      .ShowOpen
      lFileName = AbsolutePath(.Filename, CurDir)
      If lFileName <> lRegistryFileName Then 'try to force registry update
        lFileNameFoundInRegistry = False
      End If
    End With
    If LookingForDir Then pDefaultFileName = PathNameOnly(pDefaultFileName)
    If Not lFileNameFoundInRegistry Then 'try to add the key to the registry
      If Len(pRegistrySection) > 0 Then
        SaveSetting pAppName, pRegistrySection, pRegistryKey, lFileName
      End If
    End If
  End If
  
  
NoFile:
  If Not lChDir2FileDir Or Len(lFileName) = 0 Then ChDriveDir lDir
  
  GetName = lFileName
  Exit Function
  
CancelledDialog:     'Return empty string if user clicked Cancel
  pDefaultFileName = ""
  Resume NoFile
End Function

Private Sub Class_Initialize()
  pAppName = ""
  pRegistryKey = ""
  pRegistrySection = ""
  Set pRegistry = New ATCoRegistry
  pFileDialogTitle = "Application Programmer Supplied"
  pDefaultFileName = " " 'if this is null, dir returns name of vbp
  pDefaultExt = ""
  pFileFilter = ""
End Sub
