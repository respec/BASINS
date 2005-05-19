'==========================================================================
'  File:        ftpwebrequest.vb
'   Summary:    This file implements the ftp:// protocol using the Pluggable protocol feature 
'               of System.Net namespace

'Classes:   FtpWebRequest
'==========================================================================
Imports System
Imports System.IO
Imports System.Text
Imports System.Net
Imports System.Net.Sockets
Imports System.Diagnostics
Imports Microsoft.VisualBasic.Constants
Imports Microsoft.VisualBasic.Strings

'GET -> DOWNLOAD
'PUT -> UPLOAD
'LIST -> LIST
'CD -> ChangeDir
'PWD -> GetCurrentDirectory

Module ftpModule
  Public Enum FtpCommandType
    FtpControlCommand = 1
    FtpDataReceiveCommand = 2
    FtpDataSendCommand = 3
    FtpCommandNotSupported = 4
  End Enum

  Public Class ResponseDescription

    Private m_dwStatus As Integer
    Private m_szStatusDescription As String

    Public Property status() As Integer
      Get
        Return m_dwStatus
      End Get
      Set(ByVal Value As Integer)
        m_dwStatus = Value
      End Set
    End Property

    Public Property StatusDescription() As String
      Get
        Return m_szStatusDescription
      End Get
      Set(ByVal Value As String)
        m_szStatusDescription = Value
      End Set
    End Property

    Public ReadOnly Property PositivePreliminary() As Boolean
      Get
        Return (m_dwStatus \ 100 = 1)
      End Get
    End Property

    Public ReadOnly Property PositiveCompletion() As Boolean
      Get
        Return (m_dwStatus \ 100 = 2)
      End Get
    End Property

    Public ReadOnly Property PositiveIntermediate() As Boolean
      Get
        Return (m_dwStatus \ 100 = 3)
      End Get
    End Property

    Public ReadOnly Property TransientNegativeCompletion() As Boolean
      Get
        Return (m_dwStatus \ 100 = 4)
      End Get
    End Property

    Public ReadOnly Property PermanentNegativeCompletion() As Boolean
      Get
        Return (m_dwStatus \ 100 = 5)
      End Get
    End Property
  End Class

  Public Class FtpRequestCreator
    Implements IWebRequestCreate
    Public Sub New()
      'Nothing Special here
    End Sub
    Public Overridable Function Create(ByVal Url As Uri) As WebRequest Implements IWebRequestCreate.Create
      Return New FtpWebRequest(Url)
    End Function
  End Class

  Private Class FtpStream
    Inherits Stream 'inherit from stream class

    Private m_Stream As Stream
    Private m_fCanRead As Boolean
    Private m_fCanWrite As Boolean
    Private m_fCanSeek As Boolean

    Private m_fClosedByUser As Boolean

    'Constructur
    Sub New()
      m_Stream = Nothing
    End Sub

    Sub New(ByVal stream_l As Stream, ByVal canread As Boolean, ByVal canwrite As Boolean, ByVal canseek As Boolean)
      m_Stream = stream_l
      m_fCanRead = canread
      m_fCanWrite = canwrite
      m_fCanSeek = canseek
      m_fClosedByUser = False
    End Sub

    Public Overrides ReadOnly Property CanRead() As Boolean
      Get
        Return m_fCanRead
      End Get
    End Property

    Public Overrides ReadOnly Property CanWrite() As Boolean
      Get
        Return m_fCanWrite
      End Get
    End Property
    Public Overrides ReadOnly Property CanSeek() As Boolean
      Get
        Return m_fCanSeek
      End Get
    End Property
    Public Overrides ReadOnly Property Length() As Long
      Get
        Throw New NotSupportedException("This stream cannot be seeked")
      End Get
    End Property

    Public Overrides Property Position() As Long
      Get
        Throw New NotSupportedException("This stream cannot be seeked")
      End Get
      Set(ByVal Value As Long)
        Throw New NotSupportedException("This stream cannot be seeked")
      End Set
    End Property

    Public Overrides Function seek(ByVal offset As Long, ByVal origin As SeekOrigin) As Long
      Throw New NotSupportedException("This stream cannot be seeked")
    End Function

    Public Overrides Sub Flush()

    End Sub
    Public Overrides Sub SetLength(ByVal value As Long)
      Throw New NotSupportedException("This stream cannot be seeked")
    End Sub

    Public Overrides Sub close()
      m_fClosedByUser = True
    End Sub

    Public Overrides Sub Write(ByVal buffer() As Byte, ByVal offset As Integer, ByVal length As Integer)
      If (m_fClosedByUser) Then
        Throw New IOException("Cannot operate on a closed stream")
      End If
      InternalWrite(buffer, offset, length)
    End Sub

    Private Sub InternalWrite(ByVal buffer() As Byte, ByVal offset As Integer, ByVal length As Integer)
      m_Stream.Write(buffer, offset, length)
    End Sub

    Public Overrides Function Read(ByVal buffer() As Byte, ByVal offset As Integer, ByVal length As Integer) As Integer
      If (m_fClosedByUser) Then
        Throw New IOException("Cannot Operator on a closed stream")
      End If
      Return InternalRead(buffer, offset, length)
    End Function

    Private Function InternalRead(ByVal buffer() As Byte, ByVal offset As Integer, ByVal length As Integer) As Integer
      Return m_Stream.Read(buffer, offset, length)
    End Function

    Private Property InternalPosition() As Long
      Get
        Return m_Stream.Position
      End Get
      Set(ByVal Value As Long)
        m_Stream.Position = Value
      End Set
    End Property

    Private ReadOnly Property InternalLength() As Long
      Get
        Return m_Stream.Length
      End Get
    End Property
    Private Sub InternalClose()
      m_Stream.Close()
    End Sub
    Private Function GetStream() As Stream
      Return m_Stream
    End Function
  End Class


  Public Class FtpWebResponse
    Inherits WebResponse
    ' Properties overriding webresponse
    Private _StatusCode As Integer
    Private _StatusDescription As String
    Private _ContentType As String
    Private _Log As String
    Private _ResponseStream As Stream

    Friend Sub New()
      _StatusCode = -1
      _StatusDescription = Nothing
      _ResponseStream = Nothing
      _Log = Nothing
    End Sub

    Friend Sub New(ByVal StatusCode As Integer, ByVal StatusDescription As String, ByVal Log As String)
      _StatusCode = StatusCode
      _StatusDescription = StatusDescription
      _Log = Log
      _ResponseStream = Stream.Null
    End Sub


    Public Overrides Property ContentType() As String
      Get
        Return _ContentType
      End Get
      Set(ByVal Value As String)
        Throw New NotSupportedException("This property cannot be set")
      End Set
    End Property

    Public Overrides Function GetResponseStream() As Stream
      If _ResponseStream Is Nothing Then
        Throw New ApplicationException("No response stream for Me kind of method")
      End If

      Return _ResponseStream
    End Function

    Public ReadOnly Property Status() As Integer
      Get
        Return _StatusCode
      End Get
    End Property

    Public ReadOnly Property StatusDescription() As String
      Get
        Return _StatusDescription
      End Get
    End Property

    Public ReadOnly Property TransactionLog() As String
      Get
        Return _Log
      End Get
    End Property

    Friend Sub SetDownloadStream(ByVal datastream As Stream)
      _ResponseStream = datastream
      _ResponseStream.Position = 0
    End Sub

  End Class
  '
  ' Class: FtpWebRequest
  '
  ' This is the entry point for all Ftp:// requests.
  '
  Public Class FtpWebRequest
    Inherits WebRequest
    Private Const SOCKET_ERROR = -1

    Public m_szCmdParameter As String
    Private m_DataSocket As Socket
    Private m_ControlSocket As Socket

    Private m_RequestUri As Uri
    Private m_ProxyUri As Uri


    Private m_RequestStream As Stream

    Private m_Credentials As ICredentials
    Private m_Proxy As IWebProxy
    Private m_ServicePoint As UriBuilder
    Private m_bPassiveMode As Boolean

    Private m_dwdwContentLength As Long
    Private m_szContentType As String

    Private m_sbControlSocketLog As StringBuilder

    Private m_Exception As Exception

    Private m_szMethod As String
    Private m_szServerMethod As String
    Private m_CommandType As FtpCommandType

    ' various timeouts
    Private m_CommandSendTimeout As Integer
    Private m_DataReceiveTimeout As Integer
    Private m_ConnectTimeout As Integer
    Private m_DataSendTimeout As Integer

    Public Property Passive() As Boolean
      Get
        Return m_bPassiveMode
      End Get
      Set(ByVal Value As Boolean)
        m_bPassiveMode = Value
      End Set
    End Property

    Public Overrides Property Method() As String
      Get
        Return m_szMethod
      End Get
      Set(ByVal Value As String)
        If (Value = Nothing) Then
          Throw New ArgumentNullException("Method")
        End If
        m_szServerMethod = GetServerCommand(Value)
        m_CommandType = FindCommandType(m_szServerMethod)

        If (m_CommandType = FtpCommandType.FtpCommandNotSupported) Then
          Throw New NotSupportedException(Value & " is not supported")
        End If
        m_szMethod = Value
      End Set
    End Property

    Public Overrides Property Credentials() As ICredentials
      Get
        Return m_Credentials
      End Get
      Set(ByVal Value As ICredentials)
        m_Credentials = Value
      End Set
    End Property

    Public Overrides Property ConnectionGroupName() As String
      Get
        Throw New NotSupportedException
      End Get
      Set(ByVal Value As String)
        Throw New NotSupportedException
      End Set
    End Property

    Public Overrides Property ContentLength() As Long
      Get
        Return m_dwdwContentLength
      End Get
      Set(ByVal Value As Long)
        m_dwdwContentLength = Value
      End Set
    End Property

    Public Overrides Property ContentType() As String
      Get
        Return m_szContentType
      End Get
      Set(ByVal Value As String)
        m_szContentType = Value
      End Set
    End Property


    Public Overrides Property Proxy() As IWebProxy
      Get
        Return m_Proxy
      End Get
      Set(ByVal Value As IWebProxy)
        m_Proxy = Value
      End Set
    End Property

    Public Property DataSendTimeout() As Integer
      Get
        Return m_DataSendTimeout
      End Get
      Set(ByVal Value As Integer)
        m_DataSendTimeout = Value
      End Set
    End Property

    Public Property DataReceiveTimeout() As Integer
      Get
        Return m_DataReceiveTimeout
      End Get
      Set(ByVal Value As Integer)
        m_DataReceiveTimeout = Value
      End Set
    End Property

    Public Property ConnectTimeout() As Integer
      Get
        Return m_ConnectTimeout
      End Get
      Set(ByVal Value As Integer)
        m_ConnectTimeout = Value
      End Set
    End Property

    Public Property CommandTimeout() As Integer
      Get
        Return m_CommandSendTimeout
      End Get
      Set(ByVal Value As Integer)
        m_CommandSendTimeout = Value
      End Set
    End Property

    Public Overrides Function GetRequestStream() As Stream
      If (m_CommandType <> FtpCommandType.FtpDataSendCommand) Then
        Throw New InvalidOperationException("cant upload data with this method type")
      End If

      If (m_RequestStream Is Nothing) Then
        m_RequestStream = New FtpStream(New MemoryStream, False, True, False)
      Else
        Throw New InvalidOperationException("request stream already retrieved")
      End If
      Return m_RequestStream
    End Function


    'Constructor
    Sub New(ByVal Url As Uri)
      If (Url.Scheme <> "ftp") Then
        Throw New NotSupportedException("This protocol is not supported")
      End If
      Trace.WriteLine("FtpWebRequest::ctor(" & Url.AbsoluteUri & ")")
      m_RequestUri = Url
      Method = "get"
      m_bPassiveMode = True
      m_szCmdParameter = Url.AbsolutePath
      m_sbControlSocketLog = New StringBuilder
      m_ServicePoint = New UriBuilder(Url)
      m_CommandSendTimeout = 60000
      m_DataReceiveTimeout = 60000
      m_ConnectTimeout = 60000
      m_DataSendTimeout = 60000
    End Sub


    Public Overrides Function GetResponse() As WebResponse
      '
      ' 1) Login to the FTP server
      '
      Dim user As String = "anonymous"
      Dim pass As String = "Basins@epamail.epa.gov"
      '
      ' going through proxy ?
      '
      If (Not m_Proxy Is Nothing) Then
        m_ProxyUri = GetProxyUri()
        If (Not m_ProxyUri Is Nothing) Then
          If (Not m_Proxy.Credentials Is Nothing) Then
            Dim cred As NetworkCredential = m_Proxy.Credentials.GetCredential(m_ProxyUri, Nothing)
            user = cred.UserName
            pass = cred.Password

            If ((user = Nothing) Or (user = "")) Then
              user = "anonymous"
            End If

            If ((pass = Nothing) Or (pass = "")) Then
              pass = "User@"
            End If
          ElseIf (Not m_Credentials Is Nothing) Then
            Dim cred As NetworkCredential = m_Credentials.GetCredential(m_RequestUri, Nothing)
            If (Not cred Is Nothing) Then
              user = cred.UserName
              pass = cred.Password
            End If
            If ((user = Nothing) Or (user = "")) Then
              user = "anonymous"
            End If

            If ((pass = Nothing) Or (pass = "")) Then
              pass = "User@"
            End If
          End If
          user = user & "@" & m_RequestUri.Host.ToString()
        End If

        Dim u As Uri = m_Proxy.GetProxy(m_RequestUri)
        m_ServicePoint.Host = u.Host
        m_ServicePoint.Port = u.Port
      Else
        '
        ' we come here if no proxy was specified in WebRequest,
        ' or proxy was not needed for this uri
        '
        m_ServicePoint.Host = m_RequestUri.Host
        m_ServicePoint.Port = m_RequestUri.Port

        If (Not m_Credentials Is Nothing) Then

          Dim cred As NetworkCredential = m_Credentials.GetCredential(m_RequestUri, Nothing)
          user = cred.UserName
          pass = cred.Password
          If ((user = Nothing) Or (user = "")) Then
            user = "anonymous"
          End If

          If ((pass = Nothing) Or (pass = "")) Then
            pass = "User@"
          End If
        End If

      End If

      If (Not DoLogin(user, pass)) Then
        Throw New ApplicationException("Login Failed" & vbNewLine & "Server Log:" & vbNewLine & m_sbControlSocketLog.ToString())
      End If
      Return GetFtpResponse()
    End Function

    Private Function GetFtpResponse() As WebResponse

      Dim ftpresponse As FtpWebResponse = Nothing

      If m_CommandType = FtpCommandType.FtpDataReceiveCommand Then
        If m_bPassiveMode Then
          OpenPassiveDataConnection()
        Else
          OpenDataConnection()
        End If
      End If

      '
      ' negotiate data connection
      '
      Dim sztype As String = "I"
      If m_szContentType = "ascii" Then
        sztype = "A"
      End If

      SendCommand("TYPE", sztype)

      Dim resp_desc As ResponseDescription = ReceiveCommandResponse()

      If Not resp_desc.PositiveCompletion Then
        Throw New ApplicationException("Data negotiation failed:" & vbNewLine & m_sbControlSocketLog.ToString())
      End If

      If m_szServerMethod = "PWD" Then
        m_szCmdParameter = Nothing
      End If

      SendCommand(m_szServerMethod, m_szCmdParameter)

      'ftpresponse = ReceiveResponse();
      resp_desc = ReceiveCommandResponse()

      If m_CommandType = FtpCommandType.FtpDataSendCommand Then
        'if(resp_desc.Status/100 == 1) // Positive preliminary reply
        If resp_desc.PositivePreliminary Then
          If Not m_RequestStream Is Nothing Then
            Dim DataConnection As Socket
            If m_bPassiveMode Then
              DataConnection = m_DataSocket
            Else
              DataConnection = m_DataSocket.Accept()
            End If
            If DataConnection Is Nothing Then
              Throw New ProtocolViolationException("Accept failed ")
            End If

            SendData(DataConnection)
            DataConnection.Close()

            'ftpresponse = ReceiveResponse();
            Dim resp As ResponseDescription = ReceiveCommandResponse()

            ftpresponse = New FtpWebResponse(resp.Status, resp.StatusDescription, m_sbControlSocketLog.ToString())
          Else
            ' Data to be send is not specified
            Throw New ApplicationException("Data to be uploaded not specified")
          End If
        Else
          'Console.WriteLine(resp_desc.StatusDescription);	
          m_Exception = New ApplicationException(ComposeExceptionMessage(resp_desc, m_sbControlSocketLog.ToString()))
        End If
        CloseDataConnection()
      ElseIf m_CommandType = FtpCommandType.FtpDataReceiveCommand Then
        'if(resp_desc.Status/100 == 1) // Positive preliminary reply
        If resp_desc.PositivePreliminary Then
          Dim DataConnection As Socket
          If m_bPassiveMode Then
            DataConnection = m_DataSocket
          Else
            DataConnection = m_DataSocket.Accept()
          End If
          If DataConnection Is Nothing Then
            Throw New ProtocolViolationException("DataConnection failed ")
          End If
          Dim datastream As Stream = ReceiveData(DataConnection)
          'ftpresponse = ReceiveResponse();
          Dim resp As ResponseDescription = ReceiveCommandResponse()
          ftpresponse = New FtpWebResponse(resp.Status, resp.StatusDescription, m_sbControlSocketLog.ToString())
          ftpresponse.SetDownloadStream(datastream)
        Else
          m_Exception = New ApplicationException(ComposeExceptionMessage(resp_desc, m_sbControlSocketLog.ToString()))
        End If

        CloseDataConnection()
      Else
        '
        ' this is a FtpControlCommand
        '
        ftpresponse = New FtpWebResponse(resp_desc.status, resp_desc.StatusDescription, m_sbControlSocketLog.ToString())
      End If

      If Not m_Exception Is Nothing Then
        Debug.Assert(ftpresponse Is Nothing)
        Throw m_Exception
      End If

      Return ftpresponse
    End Function


    Private Function DoLogin(ByVal UserID As String, ByVal Password As String) As Boolean
      Dim resp As ResponseDescription

      OpenControlConnection(m_ServicePoint.Uri)

      SendCommand("USER", UserID)

      resp = ReceiveCommandResponse()

      If resp.status = 331 Then
        SendCommand("PASS", Password)
      Else
        Return False
      End If
      resp = ReceiveCommandResponse()

      If resp.status = 230 Then
        Return True
      End If

      Return False
    End Function

    Private Sub OpenDataConnection()
      If Not m_DataSocket Is Nothing Then
        Throw New ApplicationException("Data socket is already open.")
      End If
      m_DataSocket = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
      m_DataSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, m_ConnectTimeout)
      m_DataSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, m_DataReceiveTimeout)

      Dim localHostEnTry As IPHostEntry = Dns.GetHostByName(Dns.GetHostName())

      Dim epListener As IPEndPoint = New IPEndPoint(localHostEnTry.AddressList(0), 0)

      m_DataSocket.Bind(epListener)
      m_DataSocket.Listen(5)

      Dim localEP As IPEndPoint = CType(m_DataSocket.LocalEndPoint, IPEndPoint)
      Dim localEPAddress As UInt32 = Convert.ToUInt32(localEP.Address.Address)
      Dim szLocal As String = FormatAddress(Convert.ToInt64(localEPAddress), localEP.Port)
      SendCommand("PORT", szLocal)

      Dim resp_desc As ResponseDescription = ReceiveCommandResponse()
      'Console.WriteLine(resp_desc.StatusDescription & szLocal);
      If Not resp_desc.PositiveCompletion Then
        ' throw some exception here
        Throw New ApplicationException("Couldnt open data connection" & vbLf & ComposeExceptionMessage(resp_desc, m_sbControlSocketLog.ToString()))
      End If
    End Sub

    Private Sub OpenPassiveDataConnection()
      Trace.WriteLine("Opening Passive Data Connection")

      If Not m_DataSocket Is Nothing Then
        ' Through some exception here because passed socket is already in use
        Throw New ProtocolViolationException
      End If
      Dim IPAddressStr As String = Nothing
      Dim Port As Integer = 0
      SendCommand("PASV", "")

      'FtpWebResponse response = ReceiveResponse();
      Dim resp_desc As ResponseDescription = ReceiveCommandResponse()

      If Not resp_desc.PositiveCompletion Then
        ' throw some exception here
        Throw New ApplicationException("Couldnt open passive data connection" & vbLf & ComposeExceptionMessage(resp_desc, m_sbControlSocketLog.ToString()))
      End If

      'Console.WriteLine(resp_desc.StatusDescription);
      'Find the IPAddress and port address from response
      IPAddressStr = getIPAddress(resp_desc.StatusDescription)
      Port = getPort(resp_desc.StatusDescription)

      Trace.WriteLine("Creating passive data socket")
      m_DataSocket = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
      If m_DataSocket Is Nothing Then
        Throw New ProtocolViolationException("Error in creating Data Socket")
      End If

      m_DataSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, m_ConnectTimeout)
      m_DataSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, m_DataReceiveTimeout)

      'IPHostEntry serverHostEntry = Dns.GetHostByName(m_RequestUri.Host);		
      'IPEndPoint	serverEndPoint = new IPEndPoint(serverHostEntry.AddressList[0], Port);        		
      'IPEndPoint	serverEndPoint = new IPEndPoint(IPAddress.Parse(IPAddressStr), Port); 		

      Dim serverEndPoint As IPEndPoint = New IPEndPoint(IPAddress.Parse(IPAddressStr), Port)
      Trace.WriteLine("Passive Socket connecting to: " & IPAddressStr & " Port: " & Port)

      Try
        m_DataSocket.Connect(serverEndPoint)
      Catch e As Exception
        m_DataSocket.Close()
        m_DataSocket = Nothing
        'throw new ProtocolViolationException("Passive connection failure");   
        Throw
      End Try

      Return
    End Sub


    Private Function GetProxyUri() As Uri
      '
      ' going through a proxy ?
      '
      Dim u As Uri = Nothing
      If (Not m_Proxy Is Nothing) And (Not m_Proxy.IsBypassed(m_RequestUri)) Then
        u = m_Proxy.GetProxy(m_RequestUri)
      End If
      Return u
    End Function


    Private Sub OpenControlConnection(ByVal uriToConnect As Uri)
      Dim Host As String = uriToConnect.Host
      Dim Port As Integer = uriToConnect.Port

      If (Not m_ControlSocket Is Nothing) Then ' socketalready in use
        Throw New ProtocolViolationException("Control connection already in use")
      End If

      m_ControlSocket = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
      Dim clientIPEndPoint As EndPoint = New IPEndPoint(IPAddress.Any, 0)
      Dim clientEndPoint As EndPoint = clientIPEndPoint

      m_ControlSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, m_ConnectTimeout)
      m_ControlSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, m_DataReceiveTimeout)

      Try
        m_ControlSocket.Bind(clientEndPoint)
      Catch e As Exception
        m_ControlSocket.Close()
        m_ControlSocket = Nothing
        Throw New ApplicationException(" Error in opening control connection", e)
      End Try

      clientEndPoint = m_ControlSocket.LocalEndPoint
      Try
        Dim serverHostEnTry As IPHostEntry = Dns.GetHostByName(Host)
        Dim serverEndPoint As IPEndPoint = New IPEndPoint(serverHostEnTry.AddressList(0), Port)
        Try
          m_ControlSocket.Connect(serverEndPoint)
        Catch e As Exception
          ' throw some more suitable exception here 
          m_ControlSocket.Close()
          m_ControlSocket = Nothing
          Throw e
        Catch e As Exception
          'm_ControlSocket.Close();
          'm_ControlSocket = null;
          Throw e
        Finally
        End Try
      Finally
      End Try


      ' Get the initial response after connection		

      Dim responseStream As MemoryStream = New MemoryStream
      While (True)
        Dim BufferSize As Integer = 256
        Dim recvbuffer() As Byte = New Byte(BufferSize + 1) {}
        Dim bytesread As Integer = m_ControlSocket.Receive(recvbuffer, BufferSize, SocketFlags.None)
        responseStream.Write(recvbuffer, 0, bytesread)
        If (IsCompleteResponse(responseStream)) Then
          Exit While
        End If
      End While
      Return
    End Sub

    Public Sub CloseDataConnection()
      If Not m_DataSocket Is Nothing Then
        m_DataSocket.Close()
        m_DataSocket = Nothing
      End If
    End Sub

    Private Sub CloseControlConnection()
      m_ControlSocket.Close()
      m_ControlSocket = Nothing
    End Sub

    Private Function ReceiveData(ByVal Accept As Socket) As Stream
      If (m_DataSocket Is Nothing) Then ' something went wrong, this should never happen
        Throw New ArgumentNullException
      End If
      Dim responseStream As MemoryStream = New MemoryStream
      Dim BufferSize As Integer = 256
      Dim recvbuffer() As Byte = New Byte(BufferSize + 1) {}
      While (True)
        Dim bytesread As Integer = 0
        recvbuffer(bytesread) = CType(0, Byte)
        bytesread = Accept.Receive(recvbuffer, BufferSize, 0)
        If (bytesread <= 0) Then
          Exit While
        End If
        responseStream.Write(recvbuffer, 0, bytesread)
      End While
      'Console.WriteLine("Bytes Received " & responseStream.Length);
      Return responseStream
    End Function


    Private Sub SendCommand(ByVal RequestedMethod As String, ByVal Parametertopass As String)
      Dim Command As String = RequestedMethod
      If (Not Parametertopass Is Nothing And Parametertopass <> " ") Then
        Command &= " " & Parametertopass
      End If

      Command = Command & vbCrLf

      m_sbControlSocketLog.Append(Command)
      Trace.WriteLine(Command)
      Dim sendbuffer() As Byte = Encoding.ASCII.GetBytes(Command.ToCharArray())
      If (m_ControlSocket Is Nothing) Then
        Throw New ProtocolViolationException
      End If
      Dim cbReturn As Integer = m_ControlSocket.Send(sendbuffer, Command.Length, 0)
      If (cbReturn < 0) Then
        Throw New ApplicationException("Error writing to control socket")
      End If
      Return
    End Sub


    Private Function ReceiveCommandResponse() As ResponseDescription
      Dim resp_desc As ResponseDescription = New ResponseDescription
      Dim StatusCode As Integer = 0
      Dim StatusDescription As String = Nothing
      Dim bCompleteResponse As Boolean = False
      If (m_ControlSocket Is Nothing) Then ' something went wrong protocol violation exception
        Throw New ApplicationException("Control COnnection not opened")
      End If
      Dim responseStream As MemoryStream = New MemoryStream
      While (True)
        Dim BufferSize As Integer = 256
        Dim recvbuffer() As Byte = New Byte(BufferSize + 1) {}
        Dim bytesread As Integer = 0
        bytesread = m_ControlSocket.Receive(recvbuffer, BufferSize, 0)
        If (bytesread <= 0) Then
          Exit While
        End If
        responseStream.Write(recvbuffer, 0, bytesread)
        Dim szResponse As String = Encoding.ASCII.GetString(recvbuffer, 0, bytesread)
        Trace.WriteLine(szResponse)
        m_sbControlSocketLog.Append(szResponse)
        bCompleteResponse = IsCompleteResponse(responseStream)
        If (bCompleteResponse) Then
          Trace.WriteLine("___ RESPONSE IS COMPLETE")
          Exit While
        End If
      End While
      If (bCompleteResponse) Then
        Try
          responseStream.Position = 0
          Dim bStatusCode() As Byte = New Byte(3) {}
          responseStream.Read(bStatusCode, 0, 3)
          Dim statuscodestr As String = Encoding.ASCII.GetString(bStatusCode, 0, 3)
          StatusCode = Convert.ToInt16(statuscodestr)
        Catch
          StatusCode = -1
        End Try
        '
        ' Copy the response in Status Description
        '
        Dim responsesize As Integer = CType(responseStream.Length, Integer)
        responseStream.Position = 0
        Dim bStatusDescription() As Byte = New Byte(responsesize) {}
        responseStream.Read(bStatusDescription, 0, responsesize)
        StatusDescription = Encoding.ASCII.GetString(bStatusDescription, 4, responsesize - 4)
      Else
        ' something went wrong here
        Throw New ProtocolViolationException
      End If
      resp_desc.status = StatusCode
      resp_desc.StatusDescription = StatusDescription
      Return resp_desc
    End Function

    Private Function IsCompleteResponse(ByVal responseStream As Stream) As Boolean
      Dim bIsComplete As Boolean = False
      Dim responselength As Integer = CType(responseStream.Length, Integer)

      responseStream.Position = 0
      If responselength >= 5 Then
        Dim StatusCode As Integer = -1
        Dim ByteArray() As Byte = New Byte(responselength) {}
        Dim statuscodestr As String
        responseStream.Read(ByteArray, 0, responselength)
        statuscodestr = Encoding.ASCII.GetString(ByteArray, 0, responselength)
        If responselength = 5 And (ByteArray(responselength - 1) = Asc(vbLf)) Then
          ' Last character LF seems to be very special case, need to verify					
          bIsComplete = True

        ElseIf (ByteArray(responselength - 1) = Asc(vbLf)) And (ByteArray(responselength - 2) = Asc(vbCr)) Then
          bIsComplete = True
        End If
        If responselength = 5 And ByteArray(responselength - 1) = Asc(vbNewLine) Then
          ' Last character LF seems to be very special case, need to verify
          bIsComplete = True
        End If
        If bIsComplete Then
          Try
            StatusCode = Convert.ToInt16(statuscodestr.Substring(0, 3))
          Catch
            StatusCode = -1
          End Try
          If statuscodestr.Chars(3) = "-"c Then
            ' multiline response verify whether response is complete, reponse must be ended with CRLF					
            'find out the beginning of last line					
            Dim lastlinestart As Integer = 0
            For lastlinestart = responselength - 2 To 0 Step lastlinestart - 1
              If ByteArray(lastlinestart) = Asc(vbLf) And ByteArray(lastlinestart - 1) = Asc(vbCr) Then
                Exit For
              End If
            Next
            If lastlinestart = 0 Then
              bIsComplete = False ' Multilines not recieved						
            ElseIf statuscodestr.Chars(lastlinestart + 4) <> " "c Then
              bIsComplete = False
            Else
              Dim endcode As Integer = -1
              Try
                endcode = Convert.ToInt16(statuscodestr.Substring(lastlinestart + 1, 3))
              Catch
                endcode = -1
              End Try
              If endcode <> StatusCode Then
                bIsComplete = False ' error invalid response					
              End If
            End If

          ElseIf statuscodestr.Chars(3) <> " "c Then
            StatusCode = -1
          End If
        End If
      Else
        bIsComplete = False
      End If
      Return bIsComplete
    End Function


    ' Following function will return the status code if response is OK else return -1

    Public Shared Function GetServerCommand(ByVal command As String) As String
      If command Is Nothing Then
        Throw New ArgumentNullException("command")
      End If

      Dim cmd As String = command.ToLower()
      Dim ret As String = Nothing

      If cmd = "dir" Then
        ret = "LIST"
      ElseIf cmd = "get" Then
        ret = "RETR"
      ElseIf cmd = "cd" Then
        ret = "CWD"
      ElseIf cmd = "pwd" Then
        ret = "PWD"

      ElseIf cmd = "put" Then
        ret = "STOR"
      End If
      If ret Is Nothing Then
        Throw New NotSupportedException(command)
      End If
      Return ret
    End Function

    Public Shared Function FindCommandType(ByVal command As String) As FtpCommandType
      If (command.Equals("USER") Or command.Equals("PASS") Or command.Equals("CWD") _
          Or command.Equals("PWD") Or command.Equals("CDUP") Or command.Equals("QUIT")) Then
        Return FtpCommandType.FtpControlCommand
      ElseIf (command.Equals("RETR") Or command.Equals("LIST")) Then
        Return FtpCommandType.FtpDataReceiveCommand
      ElseIf (command.Equals("STOR") Or command.Equals("STOU")) Then
        Return FtpCommandType.FtpDataSendCommand
      Else
        Return FtpCommandType.FtpCommandNotSupported
      End If
    End Function

    Private Function SendData(ByVal Accept As Socket) As Integer
      If (Accept Is Nothing) Then
        Throw New ArgumentNullException
      End If
      Convert.ChangeType(m_RequestStream, GetType(FtpStream)).InternalPosition = 0
      Dim Length As Integer = Convert.ChangeType(m_RequestStream, GetType(FtpStream)).InternalLength
      Dim sendbuffer() As Byte = New Byte(Length) {}
      Convert.ChangeType(m_RequestStream, GetType(FtpStream)).InternalRead(sendbuffer, 0, Length)
      Dim cbReturn As Integer = Accept.Send(sendbuffer, Length, 0)
      '
      ' close the request stream
      '
      Convert.ChangeType(m_RequestStream, GetType(FtpStream)).InternalClose()
      Return cbReturn
    End Function

    Private Function FormatAddress(ByVal Address As Long, ByVal Port As Integer) As String

      Dim sb As StringBuilder = New StringBuilder(32)
      Dim Quotient As Long = Address
      Dim Remainder As Integer
      While (Quotient <> 0)
        Remainder = Quotient Mod 256
        Quotient = Quotient \ 256
        sb.Append(Remainder)
        sb.Append(","c)
      End While
      sb.Append(Port \ 256)
      sb.Append(","c)
      sb.Append(Port Mod 256)
      Return sb.ToString()
    End Function


    Private Function getIPAddress(ByVal str As String) As String
      Dim IPstr As StringBuilder = New StringBuilder(32)
      Dim Substr As String = Nothing
      Dim pos1 As Integer = str.IndexOf("(") + 1
      Dim pos2 As Integer = str.IndexOf(",")
      Dim i As Integer
      For i = 0 To 3 - 1 Step i + 1
        Substr = str.Substring(pos1, pos2 - pos1) & "."
        IPstr.Append(Substr)
        pos1 = pos2 + 1
        pos2 = str.IndexOf(",", pos1)
      Next
      Substr = str.Substring(pos1, pos2 - pos1)
      IPstr.Append(Substr)
      Return IPstr.ToString()
    End Function

    Private Function getPort(ByVal str As String) As Integer
      Dim Port As Integer = 0
      Dim pos1 As Integer = str.IndexOf("(")
      Dim pos2 As Integer = str.IndexOf(",")
      Dim i As Integer
      For i = 0 To 3 - 1 Step i + 1 'Skip the IP Address
        pos1 = pos2 + 1
        pos2 = str.IndexOf(",", pos1)
      Next
      pos1 = pos2 + 1
      pos2 = str.IndexOf(",", pos1)
      Dim PortSubstr1 As String = str.Substring(pos1, pos2 - pos1)
      pos1 = pos2 + 1
      pos2 = str.IndexOf(")", pos1)
      Dim PortSubstr2 As String = str.Substring(pos1, pos2 - pos1)
      'evaluate port number
      Port = Convert.ToInt32(PortSubstr1) * 256
      Port += Convert.ToInt32(PortSubstr2)
      Return Port
    End Function

    Private Function ComposeExceptionMessage(ByVal resp_desc As ResponseDescription, ByVal log As String) As String
      Dim sb As StringBuilder = New StringBuilder
      sb.Append("FTP Protocol Error....." & vbLf)
      sb.Append("Status: " & resp_desc.status.ToString & vbLf)
      sb.Append("Description: " & resp_desc.StatusDescription & vbLf)
      sb.Append(vbLf)
      sb.Append("--------------------------------" & vbLf)
      sb.Append(log)
      sb.Append(vbLf)
      Return sb.ToString()
    End Function
  End Class
End Module