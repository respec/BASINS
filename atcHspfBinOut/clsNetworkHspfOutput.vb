Option Strict Off
Option Explicit On

Imports MapWinUtility
Imports MapWinUtility.Strings
Imports atcUtility

Friend Class clsNetworkHspfOutput
    '##MODULE_REMARKS Copyright 2005 AQUA TERRA Consultants - Royalty-free use permitted under open source license
    Private Class clsConnectionType
        Public UpID As String
        Public DnID As String
        Public Area As Single
    End Class

    Dim pTSerFile As atcTimeseriesFileHspfBinOut
    Dim pUCIFileName As String
    Dim pErrorDescription As String
    Dim pConns As ArrayList   'of ConnectionType

    Friend WriteOnly Property TSerFile() As atcTimeseriesFileHspfBinOut
        Set(ByVal Value As atcTimeseriesFileHspfBinOut)
            pTSerFile = Value
        End Set
    End Property

    Friend WriteOnly Property UCIFile() As String
        Set(ByVal newValue As String)
            Dim s As String, d As String

            If Len(Dir(newValue)) = 0 Then
                pErrorDescription = "UCI File Name '" & newValue & "' not found"
            Else
                'Screen.MousePointer = vbHourglass
                pUCIFileName = newValue
                d = PathNameOnly(pUCIFileName)
                ChDriveDir(d)
                s = WholeFileString(FilenameNoPath(pUCIFileName))
                GetUciConnections(s)
                'Screen.MousePointer = vbDefault
            End If
        End Set
    End Property

    Private Sub GetUciConnections(ByVal s As String)

        pConns = Nothing
        pConns = New ArrayList
        GetNetworkConnections(s)
        GetExtSrcConnections(s)
    End Sub

    Private Sub GetNetworkConnections(ByVal s As String)
        Dim Sch As String, SchRec As String
        Dim SchPos As Integer, SchLen As Integer, RchPos As Integer

        SchPos = 0
        While SchPos = 0
            SchPos = InStr(s, "SCHEMATIC")
            If SchPos > 0 Then 'found something, make sure its the real start
                SchRec = StrSplit(Mid(s, SchPos), vbCrLf, "")
                If Trim(SchRec) = "SCHEMATIC" Then 'it's the real start of the SCHEMATIC block
                    SchPos = SchPos + Len(SchRec) + 2 'skip SCHEMATIC record and cr/lf
                Else 'not the real start of the SCHEMATIC block
                    SchPos = 0
                End If
            Else
                SchPos = -1
            End If
        End While
        If SchPos > 11 Then
            SchLen = InStr(SchPos, s, "END SCHEMATIC") - SchPos
            Sch = Mid(s, SchPos, SchLen)
            While Len(Sch) > 0 'process schematic records
                SchRec = StrSplit(Sch, vbCrLf, "")
                If Len(SchRec) > 40 And InStr(SchRec, "***") = 0 Then 'looks like a valid record
                    RchPos = InStr(40, SchRec, "RCHRES")
                    If RchPos > 40 Then 'something connecting to a reach
                        Dim lConn As New clsConnectionType
                        lConn.UpID = Left(SchRec, 10)
                        lConn.DnID = Mid(SchRec, 44, 10)
                        If Len(Trim(Mid(SchRec, 29, 10))) > 0 Then
                            lConn.Area = CSng(Mid(SchRec, 29, 10))
                        Else
                            lConn.Area = 1.0#
                        End If
                        pConns.Add(lConn)
                    End If
                End If
            End While
        Else
            pErrorDescription = "Unable to find SCHEMATIC Block in UCI file: " & pUCIFileName
        End If

    End Sub

    Private Sub GetExtSrcConnections(ByVal s As String)
        Dim ES As String
        Dim ESRec As String
        Dim ESPos As Integer
        Dim ESLen As Integer
        Dim InPos As Integer

        ESPos = 0
        While ESPos = 0
            ESPos = InStr(s, "EXT SOURCES")
            If ESPos > 0 Then 'found something, make sure its the real start
                ESRec = StrSplit(Mid(s, ESPos), vbCrLf, "")
                If Trim(ESRec) = "EXT SOURCES" Then 'it's the real start of the EXT SOURCES block
                    ESPos = ESPos + Len(ESRec) + 2 'skip EXT SOURCES record and cr/lf
                Else 'not the real start of the EXT SOURCES block
                    ESPos = 0
                End If
            Else
                ESPos = -1
            End If
        End While
        If ESPos > 13 Then
            ESLen = InStr(ESPos, s, "END EXT SOURCES") - ESPos
            ES = Mid(s, ESPos, ESLen)
            While Len(ES) > 0 'process EXT SOURCES records
                ESRec = StrSplit(ES, vbCrLf, "")
                If Len(ESRec) > 40 And InStr(ESRec, "***") = 0 Then 'looks like a valid record
                    InPos = InStr(50, ESRec, "INFLOW")
                    If InPos > 50 Then 'something connecting to a reach
                        Dim lConn As New clsConnectionType
                        lConn.UpID = Left(ESRec, 10)
                        lConn.DnID = Mid(ESRec, 44, 10)
                        If Len(Trim(Mid(ESRec, 29, 10))) > 0 Then
                            lConn.Area = CSng(Mid(ESRec, 29, 10))
                        Else
                            lConn.Area = 1.0#
                        End If
                        pConns.Add(lConn)
                    End If
                End If
            End While
        Else
            pErrorDescription = "Unable to find EXT SOURCES Block in UCI file: " & pUCIFileName
        End If

    End Sub

    Friend ReadOnly Property ErrorDescription() As String
        Get
            ErrorDescription = pErrorDescription
            pErrorDescription = ""
        End Get
    End Property
End Class
