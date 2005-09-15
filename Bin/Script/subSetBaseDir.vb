Imports atcUtility

Imports Microsoft.VisualBasic
Imports System.Collections
Imports System.IO
Imports System.Windows.Forms

Public Module ScriptSetBaseDir
  Public Function Main(ByVal aTestSet As String, _
                       ByVal aTestDir As String) As Boolean
    If aTestDir.Length = 0 Then aTestDir = "C:\Test"
    Dim lAllowExit as Boolean = False
    If CurDir.ToLower.StartsWith(aTestDir.ToLower) Then
      lAllowExit = True
      ChDriveDir(aTestDir & "\" & aTestSet & "\")
    Else
      ChDriveDir(aTestDir & "\" & aTestSet & "\current")
    End If
    Return lAllowExit
  End Function
End Module
