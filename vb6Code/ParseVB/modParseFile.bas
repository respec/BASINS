Attribute VB_Name = "modParseFile"
Option Explicit

Public GetFromTar As Boolean
Public TarFile As clsTar
Private TarFilenames As Collection

Public Function GetFileString(filename As String) As String
  If GetFromTar Then
    GetFileString = TarFile.fileContentsByName(filename)
  Else
    If FileExists(filename) Then
      GetFileString = WholeFileString(filename)
    Else
      MsgBox "File '" & filename & "' not found.", vbOKOnly, "Can't Open File"
    End If
  End If
End Function

'Returns next substring of source up to end of line
'Removes returned substring and CR/LF from Source
'Based on Utility:StrSplit
Function VBnextLine(Source As String) As String
  Dim retval$, i&, quoted As Boolean, trimlen As Long
  
  i = InStr(Source, vbCrLf)
     
  If i > 0 Then 'found delimeter
    retval = Left(Source, i - 1) 'string to return
    Source = Mid(Source, i + 2)  'string remaining
  Else 'take it all
    retval = Source
    Source = "" 'nothing left
  End If
  
  VBnextLine = retval
  
End Function

