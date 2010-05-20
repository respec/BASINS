'********************************************************************************************************
'File Name: clsNet.vb
'Description: Network and Internet-oriented functions.
'********************************************************************************************************
'The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
'you may not use this file except in compliance with the License. You may obtain a copy of the License at 
'http://www.mozilla.org/MPL/ 
'Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
'ANY KIND, either express or implied. See the License for the specificlanguage governing rights and 
'limitations under the License. 
'
'The Original Code is MapWindow Open Source Utility Library. 
'
'The Initial Developer of this version of the Original Code is Christopher Michaelis.
'
'Contributor(s): (Open source contributors should list themselves and their modifications here). 
'
'*******************************************************************************************************

Public Class Net

    Private Shared m_WebProxy As System.Net.WebProxy = Nothing
    Private Shared m_WebProxyNumTries As Integer = 0

    ''' <summary>
    ''' Make a System.Net.WebRequest and return its response
    ''' </summary>
    ''' <param name="aUrl">URL to retrieve</param>
    ''' <param name="aHeaders">Optional headers for web request</param>
    ''' <param name="aTimeout">Milliseconds to wait for a response before deciding it has failed</param>
    ''' <returns>WebResponse that comes from the requested URL</returns>
    ''' <remarks>If a proxy server is needed, prompt for it then proceed</remarks>
    Public Shared Function GetWebResponse(ByVal aUrl As String, _
                                 Optional ByVal aHeaders As System.Net.WebHeaderCollection = Nothing, _
                                 Optional ByVal aTimeout As Integer = 20000) As System.Net.WebResponse
        Dim response As System.Net.WebResponse = Nothing
        Dim request As System.Net.WebRequest = System.Net.WebRequest.Create(aUrl)
        SetRequestProxy(request)
        Try 'Try to get a response from the web
            request.Timeout = aTimeout
            If aHeaders IsNot Nothing Then
                request.Headers = aHeaders
            End If
            response = request.GetResponse()
        Catch we As System.Net.WebException
            If WebExceptionSetProxy(we) Then
                Return GetWebResponse(aUrl, aHeaders, aTimeout)
            Else
                Throw we
            End If
        End Try
        Return response
    End Function

    ''' <summary>
    ''' Private routine for using locally cached proxy server information
    ''' </summary>
    ''' <param name="aRequest">Set the proxy server of this request</param>
    Private Shared Sub SetRequestProxy(ByVal aRequest As System.Net.WebRequest)
        If m_WebProxy Is Nothing Then ' have not yet set a web proxy
            ' First, try just using default credentials. This works for most users.
            aRequest.Proxy.Credentials = System.Net.CredentialCache.DefaultCredentials()
        Else ' Another try, using settings provided by the user
            aRequest.Proxy = m_WebProxy
        End If
    End Sub

    ''' <summary>
    ''' Private function for parsing proxy server information from a string
    ''' </summary>
    ''' <param name="aProxyString">Proxy information formatted as:
    ''' servername:port
    ''' or for servers requiring authentication:
    ''' servername:port:username:password
    ''' </param>
    ''' <returns>
    ''' True if proxy server information was parsed from string into m_WebProxy
    ''' False if proxy server information could not be parsed from aProxyString
    ''' </returns>
    ''' <remarks>port must be numeric, 80 and 8080 are popular ports</remarks>
    Private Shared Function ProxyFromString(ByVal aProxyString As String) As Boolean
        Dim lProxySettings() As String = aProxyString.Split(":")
        If lProxySettings.Length > 1 AndAlso IsNumeric(lProxySettings(1)) Then
            m_WebProxy = New System.Net.WebProxy(lProxySettings(0), CInt(lProxySettings(1)))
            If lProxySettings.Length > 3 Then
                m_WebProxy.Credentials = New System.Net.NetworkCredential(lProxySettings(2), lProxySettings(3))
            End If
            Return True
        End If
    End Function

    ''' <summary>
    ''' Private routine to handle a WebException that is a result of needing proxy information
    ''' </summary>
    ''' <param name="aWebException">WebException that may be the result of needing proxy information</param>
    ''' <returns>
    ''' True if proxy information was found. True implies that another attempt at the same action which produced the given exception may succeed.
    ''' False means one of the following:
    '''  a) the exception was not (407) Proxy Authentication Required
    '''  b) proxy information was not found
    '''  c) too many tries have been made to get proxy information
    ''' </returns>
    ''' <remarks>
    ''' The first time this is called, it checks the registry for saved proxy information.
    ''' If no information is found in the registry, or another proxy exception occurs,
    ''' the user is prompted for proxy information which is then saved in the registry.
    ''' </remarks>
    Private Shared Function WebExceptionSetProxy(ByVal aWebException As System.Net.WebException) As Boolean
        If m_WebProxyNumTries < 3 Then 'Limit number of times we ask user for proxy information
            If aWebException.Message.IndexOf("(407)") > -1 Then '(407) Proxy Authentication Required
                MapWinUtility.Logger.Dbg("Proxy Authentication Required")
                m_WebProxyNumTries += 1
                Dim lProxyString As String

                If m_WebProxyNumTries = 1 Then
                    'If we have saved proxy information in the registry, use it
                    lProxyString = GetSetting("MapWinUtility", "Net", "ProxyServer", "")
                    If lProxyString.Length > 0 AndAlso ProxyFromString(lProxyString) AndAlso m_WebProxy IsNot Nothing Then
                        Return True
                    End If
                End If

                lProxyString = InputBox("Please enter proxy information", _
                                        "Proxy Settings", _
                                        "servername:80:username:password")
                If ProxyFromString(lProxyString) Then
                    SaveSetting("MapWinUtility", "Net", "ProxyServer", lProxyString)
                    Return True
                End If
            End If
        End If
        Return False
    End Function

    ''' <summary>
    ''' Send an HTTP POST to a URL and return the results as a string
    ''' </summary>
    ''' <param name="fullUrl">URL to access</param>
    ''' <param name="postdata">data to sent in POST</param>
    ''' <param name="bAllowAutoRedirect">True to allow redirection from fullUrl</param>
    ''' <param name="iTimeout">fail after not getting a response for this many milliseconds</param>
    ''' <returns>response from web server as a string</returns>
    Public Shared Function ExecuteUrl(ByVal fullUrl As String, ByVal postdata As String, Optional ByVal bAllowAutoRedirect As Boolean = True, Optional ByVal iTimeout As Integer = System.Threading.Timeout.Infinite) As String
        Dim webRequest As System.Net.HttpWebRequest
        Dim webResponse As System.Net.HttpWebResponse = Nothing
        Try
            'Create an HttpWebRequest with the specified URL.
            webRequest = CType(System.Net.WebRequest.Create(fullUrl), System.Net.HttpWebRequest)
            SetRequestProxy(webRequest)
            webRequest.AllowAutoRedirect = bAllowAutoRedirect
            webRequest.Method = "POST"
            webRequest.ContentType = "application/x-www-form-urlencoded"
            webRequest.ContentLength = postdata.Length
            'webRequest.MaximumAutomaticRedirections = 50
            webRequest.Timeout = iTimeout

            Dim requestStream As System.IO.Stream = webRequest.GetRequestStream()
            Dim postBytes As Byte() = System.Text.Encoding.ASCII.GetBytes(postdata)
            requestStream.Write(postBytes, 0, postBytes.Length)
            requestStream.Close()

            'Send the request and wait for a response.
            Try
                webResponse = CType(webRequest.GetResponse(), System.Net.HttpWebResponse)
                Select Case (webResponse.StatusCode)
                    Case System.Net.HttpStatusCode.OK
                        'read the content from the response
                        Dim responseStream As System.IO.Stream = _
                            webResponse.GetResponseStream()
                        Dim responseEncoding As System.Text.Encoding = _
                            System.Text.Encoding.UTF8
                        ' Pipes the response stream to a higher level stream reader with the required encoding format.
                        Dim responseReader As New System.IO.StreamReader(responseStream, responseEncoding)
                        Dim responseContent As String = _
                            responseReader.ReadToEnd()
                        Return responseContent
                    Case System.Net.HttpStatusCode.Redirect, System.Net.HttpStatusCode.MovedPermanently
                        Throw New System.Exception(String.Format( _
                            "Unable to read response content. URL has moved. StatusCode={0}.", _
                            webResponse.StatusCode))
                    Case System.Net.HttpStatusCode.NotFound
                        Throw New System.Exception(String.Format( _
                            "Unable to read response content. URL not found. StatusCode={0}.", _
                            webResponse.StatusCode))
                    Case Else
                        Throw New System.Exception(String.Format( _
                            "Unable to read response content. StatusCode={0}.", _
                            webResponse.StatusCode))
                End Select
            Catch we As System.Net.WebException
                If WebExceptionSetProxy(we) Then 'Try again after setting web proxy
                    Return ExecuteUrl(fullUrl, postdata, bAllowAutoRedirect, iTimeout)
                Else
                    Throw we
                End If
            Finally
                If webResponse IsNot Nothing Then
                    webResponse.Close()
                End If
            End Try
        Catch e As System.Exception
            Throw New System.Exception("Unable to execute URL '" & fullUrl & "'", e)
        End Try
    End Function

    ''' <summary>
    ''' Download from a URL to a string
    ''' </summary>
    ''' <param name="URL">URL to request</param>
    ''' <returns>Response from URL as string</returns>
    Public Shared Function DownloadFile(ByVal URL As String) As String
        Dim lDownloadedString As New System.Text.StringBuilder
        Try
            Dim lWebResponse As System.Net.WebResponse = GetWebResponse(URL)
            If lWebResponse IsNot Nothing Then
                Dim lContentLength As String = lWebResponse.Headers.Item("Content-Length")
                If lContentLength <> "0" Then ' Nothing can be ok, but zero means there is no content
                    Dim input As IO.Stream = lWebResponse.GetResponseStream()
                    If input IsNot Nothing Then
                        Dim count As Long = 128 * 1024 ' 128k at a time
                        Dim buffer(count - 1) As Byte
                        Do
                            count = input.Read(buffer, 0, count)
                            If count = 0 Then Exit Do 'finished download
                            lDownloadedString.Append(System.Text.Encoding.UTF8.GetString(buffer, 0, count))
                        Loop
                    End If
                End If
            End If
        Catch ex As Exception
            lDownloadedString.Append(ex.Message)
        End Try
        Return lDownloadedString.ToString
    End Function

    ''' <summary>
    ''' Check for an internet connection by attempting to get a response from CheckAgainstURL
    ''' </summary>
    ''' <param name="CheckAgainstURL">URL to access</param>
    ''' <param name="TimeoutMilliseconds">Fail after not getting a response for this many milliseconds</param>
    ''' <returns>True if a response was received from CheckAgainstURL, False if there was an error or timeout</returns>
    Public Shared Function CheckInternetConnection(ByVal CheckAgainstURL As String, Optional ByVal TimeoutMilliseconds As Integer = 2000) As Boolean
        Dim objResp As System.Net.WebResponse = Nothing
        Try
            objResp = GetWebResponse(CheckAgainstURL, , TimeoutMilliseconds)
            objResp.Close()
            Return True
        Catch ex As System.Net.WebException
            Logger.Dbg("WebException: " & ex.ToString())
            Return False
        Catch ex As Exception
            Logger.Dbg("Exception: " & ex.ToString())
            Return False
        Finally
            If Not objResp Is Nothing Then objResp.Close()
        End Try
    End Function
End Class
