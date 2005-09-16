Imports atcUtility

Imports Microsoft.VisualBasic
Imports System.Collections
Imports System.IO
Imports System.Windows.Forms

Public Module ScriptSetBaseDir
  Public Function Main(ByVal aTestSet As String, _
                       ByVal aTestDir As String) As Boolean
    Dim lBaseDir as String

    If aTestDir.Length = 0 Then 'default directory
      aTestDir = "C:\Test"
    End If

    Dim lAllowExit as Boolean = False
    If CurDir.ToLower.StartsWith(aTestDir.ToLower) Then
      lAllowExit = True
      lBaseDir = aTestDir & "\" & aTestSet & "\"
    Else
      lAllowExit = False
      lBaseDir = aTestDir & "\" & aTestSet & "\current\"
    End If

    If FileExists(lBaseDir,True,False) Then
      ChDriveDir(lBaseDir)
    Else
      MsgBox ("Cannot File Directory " & lBaseDir)
    End If

    Return lAllowExit

  End Function
End Module
