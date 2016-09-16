Imports MapWinGIS
Imports MapWindow.Interfaces
Imports System.Collections.Specialized
Imports System.Windows.Forms.Application
Imports MapWinUtility
Imports MapWinUtility.Strings
Imports atcData
Imports atcUtility
Imports atcMwGisUtility

Public Module modExecute
    ''' <summary>Run D4EMDownload.exe to retrieve data described in aQuery.</summary>
    ''' <returns>
    ''' String describing error or success.
    ''' </returns>
    Public Function Execute(ByVal aQuery As String) As String
        Dim lResult As String = ""
        Try
            Dim lQueryFilename As String = GetTemporaryFileName("DataDownloadQuery", ".txt")
            Dim lResultsFilename As String = GetTemporaryFileName("DataDownloadResults", ".txt")
            Logger.Dbg("Writing Data Download Query to " & lQueryFilename & ", requesting results in " & lResultsFilename)
            SaveFileString(lQueryFilename, aQuery)

            Dim lD4EMDownloadExe As String = IO.Path.Combine(PathNameOnly(Reflection.Assembly.GetEntryAssembly.Location), "D4EMDownload") & g_PathChar & "D4EMDownload.exe"

            If Not FileExists(lD4EMDownloadExe) Then
                lD4EMDownloadExe = FindFile("Please Locate D4EMDownload.exe", "D4EMDownload.exe")
            End If
CheckDownloadExe:
            If IO.File.Exists(lD4EMDownloadExe) Then
                If lD4EMDownloadExe.ToLowerInvariant().EndsWith("d4emdownload.exe") Then
                    Dim lArgs As String = """" & lResultsFilename & """ """ & lQueryFilename & """" '& " /debug"
                    LaunchProgram(lD4EMDownloadExe, IO.Path.GetDirectoryName(lQueryFilename), lArgs)
                    If IO.File.Exists(lResultsFilename) Then
                        Return IO.File.ReadAllText(lResultsFilename).TrimEnd(vbLf).TrimEnd(vbCr)
                    Else
                        Return "<error>Download did not complete, result status Not found. Query was: " & aQuery & "</error>"
                    End If
                Else
                    lD4EMDownloadExe = FindFile("Please Locate D4EMDownload.exe", "D4EMDownload.exe", aUserVerifyFileName:=True)
                    GoTo CheckDownloadExe
                End If
            End If
            Return "<error>User Canceled</error>"

        Catch lCancelEx As ProgressCancelException
            lResult = "<error>User Canceled</error>" 'TODO: send back partial results???
            Logger.Canceled = False
        Catch lEx As Exception
            lResult = "<error>" & lEx.ToString & "</error>"
        End Try
        Return lResult
    End Function

End Module
