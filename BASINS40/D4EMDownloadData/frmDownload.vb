Imports System.Net

Public Class frmDownload

    Public Const CancelString As String = "<Cancel>"
    Private Const pGeographicProjection As String = "+proj=latlong +datum=NAD83"
    Private pMapWin As MapWindow.Interfaces.IMapWin
    Private pOk As Boolean = False

    Public Function AskUser(ByVal aMapWin As MapWindow.Interfaces.IMapWin) As String
        pMapWin = aMapWin
        cboAOI.SelectedIndex = GetSetting("DataDownload", "Defaults", "AOI", 0)
        'chkBASINS_Met.Checked = True
        Me.ShowDialog()
        If pOk Then
            Return Me.XML
        Else
            Return CancelString
        End If
    End Function

    Private Sub btnDownload_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDownload.Click
        SaveSetting("DataDownload", "Defaults", "AOI", cboAOI.SelectedIndex)
        pOk = True
        Me.Close()
    End Sub

    Public Function SelectedRegion() As D4EMDataManager.Region
        Try
            Dim lExtents As MapWinGIS.Extents = Nothing
            Select Case cboAOI.SelectedIndex
                Case 0 : lExtents = pMapWin.View.Extents
                Case 1 : lExtents = pMapWin.Layers(pMapWin.Layers.CurrentLayer).Extents
            End Select
            If Not lExtents Is Nothing Then
                Dim lAOI As New D4EMDataManager.Region(lExtents.yMax, lExtents.yMin, lExtents.xMin, lExtents.xMax, pMapWin.Project.ProjectProjection)
                Return lAOI.GetProjected(pGeographicProjection)
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        Return Nothing
    End Function

    Public Property XML() As String
        Get
            Dim lXML As String = ""
            Dim lDesiredProjection As String = ""
            Dim lSaveFolder As String = "" '"<arg name='CacheFolder'>" & "c:\temp\" & "</arg>" & vbCrLf
            Dim lRegion As String = Me.SelectedRegion.XML

            If Not pMapWin.Project Is Nothing Then
                If pMapWin.Project.ProjectProjection.Length > 0 Then
                    lDesiredProjection = "<arg name='DesiredProjection'>" & pMapWin.Project.ProjectProjection & "</arg>" & vbCrLf
                End If
                If pMapWin.Project.FileName.Length > 0 Then
                    lSaveFolder &= "<arg name='SaveAs'>" & IO.Path.GetDirectoryName(pMapWin.Project.FileName) & "</arg>" & vbCrLf
                End If
            End If

            For Each lControl As Windows.Forms.Control In Me.Controls
                If lControl.Name.StartsWith("grp") AndAlso lControl.HasChildren Then
                    Dim lCheckedChildren As String = ""
                    For Each lChild As Windows.Forms.CheckBox In lControl.Controls
                        If lChild.Checked Then
                            lCheckedChildren &= "<arg name='DataType'>" & lChild.Name.Substring(lControl.Name.Length + 1) & "</arg>" & vbCrLf
                        End If
                    Next
                    If lCheckedChildren.Length > 0 Then
                        lXML &= "<function name='Get" & lControl.Name.Substring(3) & "'>" & vbCrLf _
                             & "<arguments>" & vbCrLf _
                             & lCheckedChildren _
                             & lSaveFolder _
                             & lDesiredProjection _
                             & lRegion _
                             & "</arguments>" & vbCrLf _
                             & "</function>" & vbCrLf
                    End If
                End If
            Next
            Return lXML
        End Get
        Set(ByVal value As String)

        End Set
    End Property

    '''' <summary>
    '''' Synchronous HTTP download
    '''' </summary>
    'Private Function URLtoString(ByVal Url As String, Optional ByVal aCookieContainer As CookieContainer = Nothing) As String
    '    Dim BytesProcessed As Integer = 0
    '    Debug.Assert(Url.StartsWith("http://"))
    '    Dim DownloadStartTime As Date = DateTime.Now

    '    ' create content stream from memory or file
    '    Dim ContentStream As New Text.StringBuilder

    '    ' Create the request object.
    '    Dim request As HttpWebRequest = HttpWebRequest.Create(Url)
    '    request.CookieContainer = aCookieContainer        
    '    Dim response As HttpWebResponse = request.GetResponse()
    '    ' only if server responds 200 OK
    '    If (response.StatusCode = HttpStatusCode.OK) Then
    '        Dim readBuffer As Byte()
    '        ReDim readBuffer(2048)
    '        Dim responseStream As IO.Stream = response.GetResponseStream()
    '        Dim iByte As Integer
    '        While (True)
    '            Dim bytesRead As Integer = responseStream.Read(readBuffer, 0, readBuffer.Length)
    '            If (bytesRead <= 0) Then Exit While
    '            iByte = 0
    '            While iByte < bytesRead
    '                ContentStream.Append(Chr(readBuffer(iByte)))
    '                iByte += 1
    '            End While
    '            BytesProcessed += bytesRead
    '        End While
    '    End If
    '    Return ContentStream.ToString
    'End Function

    'Private Function DownloadGeocache() As String
    '    Dim cookieContainer As New System.Net.CookieContainer()
    '    ' create the web request
    '    Dim req As HttpWebRequest = HttpWebRequest.Create("http://www.geocaching.com/login/default.aspx")
    '    req.CookieContainer = cookieContainer
    '    req.Method = "POST"
    '    ' create post data
    '    Dim bytePostData As Byte() = System.Text.Encoding.GetEncoding(1252).GetBytes("myUsername=vatavian&myPassword=vageca&cookie=on&Button1=Login")
    '    ' create stream object
    '    Dim streamPostData As System.IO.Stream = req.GetRequestStream()
    '    ' write to the stream
    '    streamPostData.Write(bytePostData, 0, bytePostData.Length)
    '    ' close the stream
    '    streamPostData.Close()
    '    ' wait for the response
    '    Dim resp As HttpWebResponse = req.GetResponse()
    '    ' initialize header collection
    '    Dim coll As WebHeaderCollection = resp.Headers
    '    ' get site minder cookie data
    '    Dim sessionId As String = coll.Get("Set-Cookie")
    '    ' get current location
    '    Dim location As String = coll.Get("Location")
    '    ' close response stream
    '    resp.GetResponseStream().Close()
    '    ' create new cookie container object
    '    ' add cookie headers to cookie container
    '    'cookieContainer.SetCookies(New System.Uri("http://www.geocaching.com/"), sessionId)

    '    ' Print the properties of each cookie.
    '    Dim cook As Cookie
    '    For Each cook In resp.Cookies
    '        Console.WriteLine("Cookie:")
    '        Console.WriteLine("{0} = {1}", cook.Name, cook.Value)
    '        Console.WriteLine("Domain: {0}", cook.Domain)
    '        Console.WriteLine("Path: {0}", cook.Path)
    '        Console.WriteLine("Port: {0}", cook.Port)
    '        Console.WriteLine("Secure: {0}", cook.Secure)

    '        Console.WriteLine("When issued: {0}", cook.TimeStamp)
    '        Console.WriteLine("Expires: {0} (expired? {1})", cook.Expires, cook.Expired)
    '        Console.WriteLine("Don't save: {0}", cook.Discard)
    '        Console.WriteLine("Comment: {0}", cook.Comment)
    '        Console.WriteLine("Uri for comments: {0}", cook.CommentUri)
    '        Console.WriteLine("Version: RFC {0}", IIf(cook.Version = 1, "2109", "2965"))

    '        ' Show the string representation of the cookie.
    '        Console.WriteLine("String: {0}", cook.ToString())
    '    Next cook

    '    Return URLtoString("http://www.geocaching.com/seek/nearest.aspx?lat=33.775&lon=-84.295", cookieContainer)

    'End Function

End Class