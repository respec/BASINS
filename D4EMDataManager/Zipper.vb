Imports MapWinUtility
Imports ICSharpCode.SharpZipLib.Zip

''' <summary>
''' wrapper class around SharpZipLib for unziping files
''' </summary>
''' <remarks>requires ICSharpCode.SharpZipLib.dll</remarks>
Public Class Zipper
    ''' <summary>
    ''' unzip specified zipfile to specified folder
    ''' </summary>
    ''' <param name="aZipFilename"></param>
    ''' <param name="aDestinationFolder"></param>
    ''' <param name="aIncrementProgressAfter"></param>
    ''' <param name="aProgressSameLevel"></param>
    ''' <remarks>optional progress available using MapWinUtility.ProgressLevel</remarks>
    Public Shared Sub UnzipFile(ByVal aZipFilename As String, _
                                ByVal aDestinationFolder As String, _
                       Optional ByVal aIncrementProgressAfter As Boolean = False, _
                       Optional ByVal aProgressSameLevel As Boolean = False)
         Using lLevel As New MapWinUtility.ProgressLevel(aIncrementProgressAfter, aProgressSameLevel)
            Try
                Dim lStreamSize As Long = My.Computer.FileSystem.GetFileInfo(aZipFilename).Length
                'Need Integer values for Progress, if file is too big, we fit it into Integer by dividing by lProgressDivide
                Dim lProgressDivide As Long = 1
                While lStreamSize / lProgressDivide > Integer.MaxValue
                    lProgressDivide *= 1000
                End While
                Dim lStreamSizeInt As Integer = lStreamSize / lProgressDivide

                Logger.Status("UnzipFile " & aZipFilename & " to " & aDestinationFolder, True)
                Dim lFileStream As IO.FileStream = IO.File.OpenRead(aZipFilename)
                Dim lStream As New ZipInputStream(lFileStream)
                Dim lEntry As ZipEntry = lStream.GetNextEntry
                Dim lNumFiles As Integer = 0
                Dim lTotalBytes As Long = 0
                While (Not lEntry Is Nothing)
                    Dim lDestinationName As String = IO.Path.Combine(aDestinationFolder, lEntry.Name)
                    Logger.Status("Label Middle " & lEntry.Name, False)
                    If lEntry.IsDirectory Then
                        IO.Directory.CreateDirectory(lDestinationName)
                    Else
                        IO.Directory.CreateDirectory(IO.Path.GetDirectoryName(lDestinationName))
                        Dim lFile As New IO.FileStream(lDestinationName, IO.FileMode.Create)
                        Dim lData As Byte()
                        ReDim lData(2048)
                        Try
                            Dim lSize As Integer = lStream.Read(lData, 0, lData.Length)
                            While lSize > 0
                                lTotalBytes += lSize
                                lFile.Write(lData, 0, lSize)
                                lSize = lStream.Read(lData, 0, lData.Length)
                                Logger.Progress(lTotalBytes / lProgressDivide, lStreamSizeInt)
                            End While
                            lFile.Close()
                        Catch lCancelException As MapWinUtility.ProgressCancelException
                            lStream.Close()
                            lFile.Close()
                            lFile.Dispose()
                            IO.File.Delete(lDestinationName) 'TODO: delete all files already unzipped?
                            Logger.Status("Canceled after unzipping " & lNumFiles & " files, " & Format(lTotalBytes, "#,###") & " bytes of " & Format(lStreamSize, "#,###") & " from " & aZipFilename, True)
                            Throw
                        End Try
                        lNumFiles += 1
                    End If
                    lEntry = lStream.GetNextEntry
                End While
                lStream.Close()
                lStream.Dispose()
                lFileStream.Close()
                lFileStream.Dispose()
                Logger.Dbg("Unzipped " & lNumFiles & " files, " & Format(lTotalBytes, "#,###") & " bytes")
            Catch lException As Exception
                Logger.Dbg("Error unzipping:" & lException.Message)
                Throw New ApplicationException("Unable to unzip '" & aZipFilename _
                                                        & "' to '" & aDestinationFolder _
                                                           & "': " & lException.Message)
            End Try
        End Using
    End Sub
End Class
