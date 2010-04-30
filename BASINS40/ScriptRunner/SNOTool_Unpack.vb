Imports ICSharpCode.SharpZipLib
Imports System.Collections.Specialized
Imports System.IO
Imports MapWindow.Interfaces
Imports MapWinUtility

Imports atcUtility
Imports atcData

Module ScriptSNOTool
    Private Const pInputPath = "C:\GisData\Illinois Snow\Cook_04-05\"
    Private Const pOutputPath = "C:\GisData\Illinois Snow\Cook_04-05\"
    Private Const pFileFilter = "2007118173839-*.tar"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.Dbg("SNOTool: Start:CurDir:" & CurDir())
        Logger.Dbg("SNOTool: Get all files in data directory " & pInputPath)

        Dim lUnzipper As GZip.GZipInputStream
        Dim lTar As Tar.TarArchive
        Dim ios As Stream
        Dim lFileStream As FileStream
        Dim lData(2048) As Byte
        Dim lSize As Integer

        Dim lFName As String
        Dim lOutPath As String
        Dim lZipFiles As NameValueCollection = Nothing
        Dim lFiles As NameValueCollection = Nothing
        AddFilesInDir(lFiles, pInputPath, True, pFileFilter)
        Logger.Dbg("SNOTool: Found " & lFiles.Count & " data files")

        For Each lFile As String In lFiles
            ios = File.OpenRead(lFile)
            lTar = Tar.TarArchive.CreateInputTarArchive(ios)
            lFName = FilenameNoPath(lFile)
            'use start of file name(s) up to either "-" or "."
            If lFile.Contains("-") Then
                lOutPath = pOutputPath & lFName.Substring(0, lFName.IndexOf("-") - 1)
            Else
                lOutPath = pOutputPath & lFName.Substring(0, lFName.IndexOf(".") - 1)
            End If
            lTar.ExtractContents(lOutPath)
            lTar.CloseArchive()
            ios.Close()
            AddFilesInDir(lZipFiles, lOutPath, False, "*.gz")
            For Each lZipFile As String In lZipFiles
                lUnzipper = New GZip.GZipInputStream(File.OpenRead(lZipFile))
                lFileStream = File.Create(lOutPath & "\temp.dat")
                While (True)
                    lSize = lUnzipper.Read(lData, 0, 1024)
                    If lSize > 0 Then
                        lFileStream.Write(lData, 0, lSize)
                    Else
                        Exit While
                    End If
                End While
                lUnzipper.Close()
                lFileStream.Close()
                FileCopy(lOutPath & "\temp.dat", lZipFile.Substring(0, lZipFile.Length - 3))
                Kill(lZipFile)
            Next
            Kill(lOutPath & "\temp.dat")
            lZipFiles.Clear()
        Next

        Logger.Dbg("SNOTool: Done")
    End Sub
End Module
