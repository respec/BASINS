''' <summary>
''' Check a URL containing an XML description of what update is available.
''' If an update is available, download it and ask the user whether to install it.
''' </summary>
''' <remarks>
''' Instead of having all versions of the application check the same URL for updates,
''' we assume that the URL to check will change with each update. This means that the
''' client does not need to decide whether the update is new, it can just assume that
''' any update it finds at its URL is the right one to install.
''' 
''' The web server can either keep a separate XML file available for every released version
''' or it can have a script that interprets the version information in the URL and decides
''' which update, if any, is needed for that version.
''' 
''' The updates can either be packaged as a one-size-fits-all update that will bring any
''' previous version up to the current one or as a set of progressively smaller updates 
''' for more recent versions. When installed, each update should be sure to change the URL
''' which will be used to check for new updates.
''' 
''' Example URL to check is in UpdateURL.txt
''' Example XML returned is in Update.xml
''' Example Perl CGI script for web server to interpret the URL and return the XML is in Update.pl
''' 
''' Copyright 2006 AQUA TERRA Consultants
''' Royalty-free use available under open source license
''' </remarks>
Module modMain

    Private pQuiet As Boolean = False
    Private pForm As frmUpdateCheck
    Private pSaveUpdateAs As String = ""

    Private pCancel As Boolean = False

    ''' <summary>
    ''' Check a URL containing an XML description of what update is available.
    ''' If an update is available, download it and ask the user whether to install it.
    ''' </summary>
    ''' <param name="args">
    ''' Command line can contain:
    ''' A URL to check for updates
    ''' A process ID of the main program - Updates will not be installed until that process exits.
    ''' The word quiet - If present, no window will be shown unless an update is found.
    ''' A directory to save the update package in
    ''' 
    ''' All command line arguments are optional. If not specified,
    ''' URL is read from the file UpdateURL.txt in the folder containing UpdateCheck.exe.
    ''' No process ID means no waiting for another program to exit.
    ''' No "quiet" means that a window is displayed as soon as the program starts checking for an update.
    ''' Directory to save update in defaults to the folder containing UpdateCheck.exe.
    ''' </param>
    ''' <remarks>
    ''' Directory to save the update package in should be enclosed in "quotes" in case there are any spaces in the path
    ''' </remarks>
    Sub main(ByVal args() As String)
        Dim lExecutingDirectory As String = IO.Path.GetDirectoryName(Reflection.Assembly.GetExecutingAssembly.Location)
        Dim lastArg As Integer = args.Length - 1
        Dim lErr As String = ""
        Dim lParentProcess As Process = Nothing
        Dim lSaveUpdateDir As String = ""
        Dim lUpdateCheckURL As String = ""
        Dim lUpdateString As String = ""

        For Each curArg As String In args
            If curArg.Contains("://") Then
                lUpdateCheckURL = curArg
            ElseIf curArg.ToLower.Equals("quiet") Then
                pQuiet = True
            ElseIf IsNumeric(curArg) Then
                Dim lParentProcessID As Integer = CInt(curArg)
                If lParentProcessID > 0 Then
                    Try
                        lParentProcess = Process.GetProcessById(lParentProcessID)
                    Catch exParent As Exception
                        'TODO: log message that parent process ID could not be found?
                    End Try
                End If
            ElseIf lSaveUpdateDir.Length > 0 Then 'Too many args are left after above rules, should only be one for SaveUpdateDir
                If lErr.Length = 0 Then lErr = lSaveUpdateDir
                lErr &= vbCrLf & curArg
            Else
                lSaveUpdateDir = curArg.Replace("""", "") 'remove quotes (quotes keep filename together on command line if it contains spaces)
            End If
        Next

        If lSaveUpdateDir.Length = 0 Then 'default to saving update in the directory we are running from
            lSaveUpdateDir = lExecutingDirectory
        End If

        If lUpdateCheckURL.Length = 0 Then
            Try 'Try finding UpdateURL.txt in the directory we are running from
                lUpdateCheckURL = IO.File.ReadAllText(IO.Path.Combine(lExecutingDirectory, "UpdateURL.txt"))
                lUpdateCheckURL = lUpdateCheckURL.Replace(vbCr, "")
                lUpdateCheckURL = lUpdateCheckURL.Replace(vbLf, "")
                lUpdateCheckURL = lUpdateCheckURL.Trim
            Catch ex As Exception
                'If we couldn't find it, lUpdateCheckURL is still blank
            End Try
        End If

        If lErr.Length > 0 Then
            If Not pQuiet Then MsgBox("Too many arguments left, only one can be update folder:" & vbCrLf & lErr, MsgBoxStyle.Critical, "Check for Updates")
        ElseIf lUpdateCheckURL.Length = 0 Then
            If Not pQuiet Then MsgBox("URL to check not specified", MsgBoxStyle.Critical, "Check for Updates")
        ElseIf lSaveUpdateDir.Length = 0 Then
            If Not pQuiet Then MsgBox("Location to save update not specified", MsgBoxStyle.Critical, "Check for Updates")
        Else 'Passed all argument checks
            'MsgBox("x")
            If Not pQuiet Then 'Show form
                pForm = New frmUpdateCheck
                With pForm
                    .Label1.Text = "Checking for an update" & vbCrLf & lUpdateCheckURL
                    .Progress.Style = ProgressBarStyle.Marquee
                    .Bounds = .RestoreBounds
                    .Show()
                    .Visible = True
                    .Activate()
                    '.BringToFront()
                    '.Refresh()
                End With
                Application.DoEvents()
            End If
            Try
                Dim lDownload As New Net.WebClient
                lDownload.Proxy.Credentials = System.Net.CredentialCache.DefaultCredentials
                lUpdateString = lDownload.DownloadString(New System.Uri(lUpdateCheckURL)).Trim
                lDownload = Nothing
                If lUpdateString.Length = 0 OrElse lUpdateString.Contains("No update") Then
                    If Not pQuiet Then
                        pForm.Label1.Text = lUpdateString
                        ShowExit()
                    End If
                Else
                    Dim lUpdateTitle As String = "Update ready to install"
                    Dim lUpdateQuestion As String = "Install now?"
                    Dim lParentTitle As String = "Cannot install update while main application is running"
                    Dim lParentMessage As String = "Close main application then press OK to install update."
                    Dim lDescription As String = "Update has been downloaded"
                    Dim lSize As Long = 0

                    Dim xD As New Xml.XmlDocument
                    xD.LoadXml(lUpdateString)
                    Dim x As Xml.XmlNode = xD.SelectSingleNode("//update")
                    If x IsNot Nothing Then x = x.FirstChild
                    While x IsNot Nothing
                        Select Case x.LocalName.ToLower
                            Case "updatetitle" : lUpdateTitle = x.InnerXml
                            Case "updatequestion" : lUpdateQuestion = x.InnerXml
                            Case "parenttitle" : lParentTitle = x.InnerXml
                            Case "parentmessage" : lParentMessage = x.InnerXml
                                'Case "adminrequired"
                                'TODO: find a robust way to determine if admin privleges are present
                                '    If Not My.User.IsInRole("BUILTIN\Administrators") Then
                                '        MsgBox("Log in as Administrator, then check for updates again to continue", MsgBoxStyle.OkOnly, "Administrator required")
                                '        Exit Sub
                                '    End If
                            Case "description" : lDescription = x.InnerXml
                            Case "size" : lSize = CInt(x.InnerXml)
                            Case "get"
                                IO.Directory.CreateDirectory(lSaveUpdateDir)
                                pSaveUpdateAs = IO.Path.Combine(lSaveUpdateDir, IO.Path.GetFileName(x.InnerXml))
                                DownloadFileWithProgress(x.InnerXml, pSaveUpdateAs, lSize)
                            Case "run" 'Run the specified command if given, or default to downloaded file
                                Dim lRunCommand As String = x.InnerXml
                                If lRunCommand.Length < 1 Then lRunCommand = pSaveUpdateAs
                                If lRunCommand.StartsWith("http") OrElse IO.File.Exists(lRunCommand) Then
                                    If pForm IsNot Nothing Then pForm.Close() : pForm = Nothing
                                    If MsgBox(lDescription & vbCrLf & vbCrLf & lUpdateQuestion, MsgBoxStyle.YesNo, lUpdateTitle) = MsgBoxResult.Yes Then
                                        If lParentProcess IsNot Nothing AndAlso lParentTitle <> "None" AndAlso lParentMessage <> "None" Then 'make sure parent exits before launching update
                                            While Not lParentProcess.HasExited
                                                If MsgBox(lParentMessage, MsgBoxStyle.OkCancel, lParentTitle) = MsgBoxResult.Cancel Then
                                                    Exit Sub
                                                End If
                                                Application.DoEvents()
                                            End While
                                        End If
                                        OpenFileOrURL(lRunCommand)
                                    End If
                                End If
                        End Select
                        x = x.NextSibling
                    End While
                End If
            Catch xe As Xml.XmlException
                'Didn't get valid XML describing an update, so assume there is no update available
            Catch we As Net.WebException
                If Not pQuiet Then
                    MsgBox("Error message:" & vbCr & we.Message, MsgBoxStyle.OkOnly, "Web Exception")
                End If
            Catch ex As Exception
                If Not pQuiet Then
                    MsgBox("Error message:" & vbCr & ex.Message, MsgBoxStyle.OkOnly, "Update Exception")
                End If
            End Try
        End If
    End Sub

    ''' <summary>
    ''' Get file from aURL and save it as aFilename
    ''' </summary>
    ''' <param name="aURL">URL of file to download</param>
    ''' <param name="aFilename">Name of file to save</param>
    ''' <param name="aTotalBytes">Size of file to download, if known</param>
    ''' <returns>True if file was downloaded successfully, False if not</returns>
    ''' <remarks>
    ''' If aFilename already exists and its size matches aTotalBytes (and aTotalBytes > 0), downloading is skipped.
    ''' If aFilename already exists and its size does not match aTotalBytes, the existing file is deleted.
    ''' </remarks>
    Private Function DownloadFileWithProgress(ByVal aURL As String, ByVal aFilename As String, Optional ByVal aTotalBytes As Long = 0) As Boolean
        If IO.File.Exists(aFilename) Then
            If aTotalBytes > 0 AndAlso FileLen(aFilename) = aTotalBytes Then
                Return True 'Already have right-size file by that name (TODO: check hash?)
            Else 'Have different file by that name, get rid of it and download new one
                IO.File.Delete(aFilename)
            End If
        End If

        Dim lSaveFile As IO.FileStream = Nothing
        Try
            Dim lWeb As Net.WebRequest
            Dim lBytesRead As Integer
            Dim lTotalBytesRead As Long
            Dim lTotalSizeString As String = ""
            Dim lKnowTotalSize As Boolean = False
            Dim lBuffer As Byte()
            Dim lBufferSize As Integer = 4096
            ReDim lBuffer(lBufferSize)

            lSaveFile = New IO.FileStream(aFilename, IO.FileMode.Create, IO.FileAccess.Write)

            lWeb = Net.WebRequest.Create(aURL)
            lWeb.Proxy.Credentials = System.Net.CredentialCache.DefaultCredentials
            Dim myWebResponse As Net.WebResponse = lWeb.GetResponse
            If myWebResponse.ContentLength > aTotalBytes Then
                aTotalBytes = myWebResponse.ContentLength
            End If
            If aTotalBytes > 0 Then
                lTotalSizeString = " of " & FormatFileSize(aTotalBytes)
            End If

            Dim sChunks As IO.Stream = myWebResponse.GetResponseStream

            If Not pForm Is Nothing Then
                With pForm
                    If aTotalBytes > 0 AndAlso aTotalBytes < Integer.MaxValue Then
                        .Progress.Maximum = aTotalBytes
                        .Progress.Style = ProgressBarStyle.Blocks
                        lKnowTotalSize = True
                    Else
                        .Progress.Style = ProgressBarStyle.Marquee
                        lKnowTotalSize = False
                    End If
                    .Label1.Text = "Downloading Update" & vbCrLf & aURL
                End With
            End If

            Do
                lBytesRead = sChunks.Read(lBuffer, 0, lBufferSize)
                lSaveFile.Write(lBuffer, 0, lBytesRead)
                lTotalBytesRead += lBytesRead
                If Not pForm Is Nothing Then
                    With pForm
                        If lKnowTotalSize Then
                            .Progress.Value = lTotalBytesRead
                        End If
                        If aTotalBytes > lTotalBytesRead Then
                            .Label2.Text = FormatFileSize(lTotalBytesRead) & lTotalSizeString & Format(lTotalBytesRead / aTotalBytes, " (0.0%)")
                        Else
                            .Label2.Text = FormatFileSize(lTotalBytesRead)
                        End If
                        .Refresh()
                    End With
                End If
                Application.DoEvents()
            Loop While Not pCancel And lBytesRead > 0
            sChunks.Close()
            lSaveFile.Close()
            Return True
        Catch ex As Exception
            If Not (lSaveFile Is Nothing) Then
                lSaveFile.Close()
                lSaveFile = Nothing
            End If
            If Not pQuiet Then
                MsgBox("Error message:" & vbCrLf & ex.Message & vbCrLf & vbCrLf _
                     & "Save" & vbCrLf & aURL & vbCrLf _
                     & "in file" & vbCrLf & aFilename & vbCrLf _
                     & "then check for updates again to continue", MsgBoxStyle.Critical, "Could not get update")
            End If
            End '!!!!!!!!!!!!!!!!!!!
        End Try
    End Function

    Private Function FormatFileSize(ByVal Size As Long) As String
        Try
            Dim KB As Integer = 1024
            Dim MB As Integer = KB * KB
            ' Return size of file in kilobytes.
            If Size < KB Then
                Return (Size.ToString("D") & " bytes")
            Else
                Select Case Size / KB
                    Case Is < 1000
                        Return CLng(Size / KB) & " K"
                    Case Is < 1000000
                        Return CLng((Size * 10 / MB)) / 10 & " M"
                    Case Else
                        Return CLng((Size * 100 / MB / KB)) / 100 & " G"
                End Select
            End If
        Catch ex As Exception
            Return Size.ToString
        End Try
    End Function

    'Open a file using the default method the system would have used if it was double-clicked
    Private Sub OpenFileOrURL(ByVal FileOrURL As String, Optional ByVal Wait As Boolean = False)
        Dim newProcess As New Process
        Try
            If FileOrURL <> "" Then
                newProcess.StartInfo.FileName = FileOrURL
                newProcess.Start()
                If Wait Then
                    newProcess.WaitForExit()
                End If
            End If
        Catch ex As System.Exception
            If Not pQuiet Then MsgBox("Could not open " & vbCrLf & FileOrURL & vbCrLf & ex.Message, MsgBoxStyle.Critical, "Could not open")
        End Try
    End Sub

    Private Sub ShowExit()
        If Not pForm Is Nothing Then
            With pForm
                .Progress.Visible = False
                .btnCancel.Visible = False
                .btnExit.Visible = True
                .Refresh()
                While Not pForm Is Nothing AndAlso .Visible
                    Threading.Thread.Sleep(10)
                    Application.DoEvents()
                End While
            End With
        End If
        End '!!!!!!!
    End Sub

    Public Sub Cancel()
        pCancel = True
        Threading.Thread.Sleep(10)
        Application.DoEvents()

        Dim lStartCancel As Date = Now

        'Remove partly-downloaded update if there is one
        'While loop is needed because the file is often not immediately free for deletion
        While IO.File.Exists(pSaveUpdateAs)
            Try
                IO.File.Delete(pSaveUpdateAs)
            Catch ex As Exception
                'If we have been trying for more than a second to delete, give up
                If Now.Subtract(lStartCancel).Seconds > 1 Then Exit While
                Threading.Thread.Sleep(10)
                Application.DoEvents()
            End Try
        End While

        If Not pForm Is Nothing Then pForm.Label1.Text = "Cancelled"
        ShowExit()
    End Sub

End Module
