Imports System.IO
Imports System.Net
Imports System.Text

Public Class Downloader
    Private Const BufSize = 1024
    Private Shared Initialized As Boolean = False

    Private Shared Sub InitDownloader()
        If Not Initialized Then
            'Register FTP handler so WebRequest.Create(url) will work if url strats with "ftp:"
            Dim Creator As FtpRequestCreator = New FtpRequestCreator
            WebRequest.RegisterPrefix("ftp:", Creator)

            Initialized = True
        End If
    End Sub

    Public Shared Sub DownloadURL(ByVal url As String, ByVal SaveAsFilename As String)
        Dim wc As New WebClient
        InitDownloader()
        wc.DownloadFile(url, SaveAsFilename)
        'Dim SaveStream As New FileStream(SaveAsFilename, FileMode.CreateNew)
        'Dim SaveWriter As New BinaryWriter(SaveStream)

        'Dim req As WebRequest = WebRequest.Create(url)
        'Dim res As WebResponse = req.GetResponse()

        'Dim nBytesInBuffer As Integer = 0
        'Dim nBytesSoFar As Long = 0
        'Dim Buffer(BufSize) As Byte
        'Dim stream As Stream = res.GetResponseStream()

        'Debug.WriteLine("res.ContentLength = " & res.ContentLength())
        'nBytesInBuffer = stream.Read(Buffer, 0, BufSize)
        'While nBytesInBuffer > 0
        '    Debug.Write(".")
        '    If nBytesSoFar Mod nBytesInBuffer * 80 = 0 Then Debug.WriteLine("")
        '    nBytesSoFar += nBytesInBuffer
        '    SaveWriter.Write(Buffer, 0, nBytesInBuffer)
        '    nBytesInBuffer = stream.Read(Buffer, 0, BufSize)
        'End While

        'SaveWriter.Close()
        'SaveStream.Close()
    End Sub
End Class
