Attribute VB_Name = "modDriverBatchUSGSDv2Wdm"
Option Explicit

Sub main()
  Dim myDownloadPath As String
  Dim myWDMFileName As String
  Dim myDownloadFileName As String
  Dim myDownloadFiles As Collection 'of file names
  Dim s As String
    
  s = Command
  If Len(s) = 0 Then s = "e:\basins\data\030701\download\UsgsDV, e:\basins\data\030701\download.wdm"
  myDownloadPath = StrRetRem(s)
  myWDMFileName = s
  
  Set myDownloadFiles = New Collection
  
  If Left(myDownloadPath, 1) <> Left(CurDir, 1) Then ChDrive Left(myDownloadPath, 1)
  ChDir myDownloadPath
  myDownloadFileName = Dir("*.txt")
  While Len(myDownloadFileName) > 0
    myDownloadFiles.Add myDownloadFileName
    myDownloadFileName = Dir
  Wend
  
  ConvertUsgsDv2Wdm myDownloadPath, myWDMFileName, myDownloadFiles
  
End Sub

