Attribute VB_Name = "modUploadUpdate"
Option Explicit

Public Const TestFile = "\\Hspf\ftp\download\test\Upgrade.xml"
Public Const ReleaseFile = "\\Hspf\ftp\download\Upgrade.xml"

Public Logger As clsATCoLogger

Public Function MakeComponentXML(Filename As String, _
                                 Destination As String, _
                                 Optional Instructions As String = "", _
                                 Optional ReleaseNote As String = "", _
                                 Optional ComponentName As String = "") As String
  Dim xml As String
  Dim thisFileInfo As String
  Dim verString As String
  On Error GoTo SomeError
  
  verString = GetFileVerString(Filename)
  While Right(verString, 2) = ".0"
    verString = Left(verString, Len(verString) - 2)
  Wend
  
  If Len(ComponentName) = 0 Then ComponentName = FilenameOnly(Filename)
  'SetWinDirs
  xml = "<?xml version=""1.0"" standalone=""no""?>"
 'xml = xml & vbCrLf & "<!DOCTYPE ATCCompMl SYSTEM ""http://hspf.com/pub/download/ATCCompMl.dtd"">"
  xml = xml & vbCrLf & "<ATCCompMl>"
  xml = xml & vbCrLf & "  <Component"
  xml = xml & vbCrLf & "     File=""" & FilenameNoPath(Filename) & """"
  xml = xml & vbCrLf & "     Version=""" & verString & """"
  xml = xml & vbCrLf & "     Date=""" & Format(FileDateTime(Filename), "MM/DD/YYYY hh:mm:ss am/pm") & """"
  xml = xml & vbCrLf & "     Size=""" & Format(FileLen(Filename), "#,###") & """"
  xml = xml & vbCrLf & "     Name=""" & ComponentName & """"
  xml = xml & vbCrLf & "     Destination=""" & Destination & """"
  xml = xml & ">"
  xml = xml & vbCrLf & "     <Instructions>" & vbCrLf & Instructions & vbCrLf & "     </Instructions>"
  xml = xml & vbCrLf & "     <ReleaseNote>" & vbCrLf & ReleaseNote & vbCrLf & "     </ReleaseNote>"
  xml = xml & vbCrLf & "  </Component>"
  xml = xml & vbCrLf & "</ATCCompMl>"
  MakeComponentXML = xml
  Exit Function
SomeError:
  Logger.LogMsg Err.Description, "MakeComponentXML"
End Function

