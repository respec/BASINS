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
    Public Shared Function ExecuteUrl(ByVal fullUrl As String, ByVal postdata As String, Optional ByVal bAllowAutoRedirect As Boolean = True, Optional ByVal iTimeout As Integer = System.Threading.Timeout.Infinite) As String
        Dim webRequest As System.Net.HttpWebRequest
        Dim webResponse As System.Net.HttpWebResponse = Nothing
        Try
            'Create an HttpWebRequest with the specified URL.
            webRequest = CType(System.Net.WebRequest.Create(fullUrl), System.Net.HttpWebRequest)
            webRequest.Proxy.Credentials = System.Net.CredentialCache.DefaultCredentials
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
                            "Unable to read response content.  URL has moved. StatusCode={0}.", _
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
                'If (we.Status = Net.WebExceptionStatus.Timeout) Then
                '    Return False
                'End If
                Throw New System.Exception( _
                    "Unable to execute URL.", _
                    we)
            Finally
                If (Not IsNothing(webResponse)) Then
                    webResponse.Close()
                End If
            End Try
        Catch e As System.Exception
            Throw New System.Exception( _
                "Unable to execute URL.", _
                e)
        End Try
    End Function

    Public Shared Function DownloadFile(ByVal URL As String) As String
        Dim myStringWebResource As String = Nothing
        Dim myWebClient As New System.Net.WebClient
        myWebClient.Proxy.Credentials = System.Net.CredentialCache.DefaultCredentials
        Try
            Dim tempfile As String = System.IO.Path.GetTempFileName()
            myWebClient.DownloadFile(URL, tempfile)
            Dim retval As String = MapWinUtility.Strings.WholeFileString(tempfile)
            Kill(tempfile)
            Return retval
        Catch ex As Exception
            Return ""
        End Try
    End Function

    Public Shared Function CheckInternetConnection(ByVal CheckAgainstURL As String, Optional ByVal TimeoutMilliseconds As Integer = 2000) As Boolean
        Dim objWebReq As System.Net.WebRequest
        Dim myUrl As New System.Uri(CheckAgainstURL)
        objWebReq = System.Net.WebRequest.Create(myUrl)
        'Proxy authentication required
        objWebReq.Proxy.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials 'System.Net.CredentialCache.DefaultCredentials
        Logger.Dbg("GetProxy: " & objWebReq.Proxy.GetProxy(myUrl).ToString())

        objWebReq.Timeout = TimeoutMilliseconds
        Dim objResp As System.Net.WebResponse = Nothing
        Try
            objResp = objWebReq.GetResponse
            objResp.Close()
            objWebReq = Nothing
            Return True
        Catch ex As System.Net.WebException
            Logger.Dbg("WebException: " & ex.ToString())
            Return False
        Catch ex As Exception
            Logger.Dbg("Exception: " & ex.ToString())
            Return False
        Finally
            If Not objResp Is Nothing Then objResp.Close()
            objWebReq = Nothing
        End Try
    End Function
End Class
