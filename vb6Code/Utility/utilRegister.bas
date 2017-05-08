Attribute VB_Name = "utilRegister"
Option Explicit

Private Declare Function LoadLibrary Lib "kernel32" Alias "LoadLibraryA" _
(ByVal lpLibFileName As String) As Long

Private Declare Function GetProcAddress Lib "kernel32" (ByVal hModule As Long, _
  ByVal lpProcName As String) As Long

Private Declare Function CreateThread Lib "kernel32" (lpThreadAttributes As Any, _
 ByVal dwStackSize As Long, ByVal lpStartAddress As Long, ByVal lParameter As Long, _
 ByVal dwCreationFlags As Long, lpThreadID As Long) As Long
 
Private Declare Function WaitForSingleObject Lib "kernel32" (ByVal hHandle As Long, _
  ByVal dwMilliseconds As Long) As Long
 
Private Declare Function GetExitCodeThread Lib "kernel32" (ByVal hThread As Long, _
  lpExitCode As Long) As Long

Private Declare Sub ExitThread Lib "kernel32" (ByVal dwExitCode As Long)

Private Declare Function FreeLibrary Lib "kernel32" (ByVal hLibModule As Long) As Long

Private Declare Function CloseHandle Lib "kernel32" (ByVal hObject As Long) As Long

'Public Sub KillLibrary(ByVal inFileSpec As String)
'  Dim lLib As Long                 ' Store handle of the control library
'  lLib = LoadLibrary(inFileSpec)
'  While FreeLibrary(lLib) <> 0
'    Debug.Print "Free"
'  Wend
'  Kill inFileSpec
'End Sub

Public Function Register(ByVal LibraryFilename As String) As String
  Register = RegUnReg(LibraryFilename, True)
End Function

Public Function UnRegister(ByVal LibraryFilename As String) As String
  UnRegister = RegUnReg(LibraryFilename, False)
End Function

Private Function RegUnReg(ByVal inFileSpec As String, Register As Boolean) As String
  Dim lLib As Long                 ' Store handle of the control library
  Dim lpDLLEntryPoint As Long      ' Store the address of function called
  Dim lpThreadID As Long           ' Pointer that receives the thread identifier
  Dim lpExitCode As Long           ' Exit code of GetExitCodeThread
  Dim mThread
  Dim mresult
  Dim Msg As String
  
  On Error Resume Next
    
  ' Load the control DLL, i. e. map the specified DLL file into the
  ' address space of the calling process
  lLib = LoadLibrary(inFileSpec)
  If lLib = 0 Then
    ' e.g. file not exists or not a valid DLL file
    Msg = "Failure loading library " & inFileSpec
  Else
    ' Find and store the DLL entry point, i.e. obtain the address of the
    ' “DllRegisterServer” or "DllUnregisterServer" function (to register
    ' or deregister the server’s components in the registry).
    If Register Then
      lpDLLEntryPoint = GetProcAddress(lLib, "DllRegisterServer")
    Else
      lpDLLEntryPoint = GetProcAddress(lLib, "DllUnregisterServer")
    End If
  
    If lpDLLEntryPoint = vbNull Or lpDLLEntryPoint = 0 Then
      mThread = 0
    Else
      ' Create a thread to execute within the virtual address space of the calling process
      mThread = CreateThread(ByVal 0, 0, ByVal lpDLLEntryPoint, ByVal 0, 0, lpThreadID)
    End If
  
    If mThread = 0 Then
      Msg = "Process failed in obtaining entry point or creating thread."
    Else
      ' Use WaitForSingleObject to check the return state (i) when the specified object
      ' is in the signaled state or (ii) when the time-out interval elapses.  This
      ' function can be used to test Process and Thread.
      mresult = WaitForSingleObject(mThread, 10000)
      If mresult = 0 Then
        ' We don't call the dangerous TerminateThread(); after the last handle
        ' to an object is closed, the object is removed from the system.
        CloseHandle mThread
        If Register Then
          Msg = "Register Successful"
        Else
          Msg = "Unregister Successful"
        End If
      Else
        Msg = "Process failed in signaled state or time-out."
        ' Terminate the thread to free up resources that are used by the thread
        ' NB Calling ExitThread for an application's primary thread will cause
        ' the application to terminate
        lpExitCode = GetExitCodeThread(mThread, lpExitCode)
        ExitThread lpExitCode
      End If
    End If
    
    ' Decrements the reference count of loaded DLL module before leaving
    FreeLibrary lLib
  End If
  
  RegUnReg = Msg
End Function
