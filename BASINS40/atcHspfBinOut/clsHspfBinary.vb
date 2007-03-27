Option Strict Off
Option Explicit On

Imports MapWinUtility
Imports atcUtility

Friend Class clsHspfBinId
    Public OperationName As String = ""
    Public OperationNumber As Integer = 0
    Public SectionName As String = ""
End Class

Friend Class clsHspfBinData
    Public UnitFlag As Integer = 0
    Public OutLev As Integer = 0
    Public DateArray() As Integer
    Public value() As Single
    Public Sub New()
        ReDim DateArray(5)
    End Sub
End Class

Friend Class clsHspfBinHeader
    Public id As New clsHspfBinId
    Public VarNames As atcCollection 'of String
    Public Data As atcCollection 'of HSPFBinaryData
End Class

Friend Class clsHspfBinary
    '##MODULE_REMARKS Copyright 2005 AQUA TERRA Consultants - Royalty-free use permitted under open source license
    Private pFileName As String
    Private pErrorDescription As String
    Private pFile As clsFtnUnfFile
    Private pFileRecordIndex As Integer

    Dim pHeaders As atcCollection 'of HSPFBinaryHeader
    Private pMonitor As Object
    Private pMonitorSet As Boolean

    Friend ReadOnly Property DateAsJulian(ByVal hindex As Integer, ByVal dindex As Integer) As Double
        Get
            Dim lData As clsHspfBinData
            lData = Header(hindex).Data.ItemByIndex(dindex)
            Return Date2J(lData.DateArray)
        End Get
    End Property

    Friend ReadOnly Property DateAsText(ByVal hindex As Integer, ByVal dindex As Integer) As String
        Get
            Dim lData As clsHspfBinData
            lData = Header(hindex).Data.ItemByIndex(dindex)
            With lData
                DateAsText = .DateArray(0) & "/" & .DateArray(1) & "/" & .DateArray(2) & " " & _
                             .DateArray(3) & ":" & .DateArray(4) & ":" & .DateArray(5)
            End With
        End Get
    End Property

    Friend ReadOnly Property Header(ByVal index As Integer) As clsHspfBinHeader
        Get
            Return pHeaders.ItemByIndex(index)
        End Get
    End Property

    Friend ReadOnly Property Headers() As atcCollection
        Get
            Return pHeaders
        End Get
    End Property

    Friend WriteOnly Property Monitor() As Object
        Set(ByVal o As Object)
            pMonitor = o
            pMonitorSet = True
        End Set
    End Property

    Friend ReadOnly Property HeaderIdAsText(ByVal index As Integer) As String
        Get
            Return HspfBinaryIdAsText(Header(index).id)
        End Get
    End Property

    Friend Property Filename() As String
        Get
            Return pFile.Filename
        End Get
        Set(ByVal newFileName As String)
            pFile = New clsFtnUnfFile
            pFile.Filename = newFileName
            pErrorDescription = pFile.ErrorDescription
            If Len(pErrorDescription) = 0 Then
                pHeaders = Nothing
                pHeaders = New atcCollection
                pFileRecordIndex = 0
                ReadNewRecords()
            End If
        End Set
    End Property

    Friend Sub ReadNewRecords()
        Dim myId As New clsHspfBinId
        Dim myHeader As clsHspfBinHeader
        Dim myData As clsHspfBinData
        Dim myKey As String, myDataKey As String
        Dim j As Integer, varCnt As Integer
        Dim s As String
        Dim currec() As Byte
        Dim recbyte As Integer 'Byte position within current header record
        Dim thisVarName As String
        Dim varLen As Integer
        Dim i As Integer
        'Dim percent As Double, lastprog As Double

        pFile.ReadRestOfRecordsInFile()
        While pFileRecordIndex < pFile.RecordCount
            currec = pFile.Rec(pFileRecordIndex)
            Select Case BitConverter.ToInt32(currec, 0)
                Case 0 'header record
                    myHeader = New clsHspfBinHeader
                    With myHeader
                        myKey = MakeIdAndKey(currec, .id)
                        .VarNames = New atcCollection

                        recbyte = 24
                        While recbyte < UBound(currec)
                            varLen = BitConverter.ToInt32(currec, recbyte)
                            thisVarName = Byte2String(currec, recbyte + 4, varLen)
                            'On Error Resume Next
                            If InStr(thisVarName, "(") Then
                                Logger.Dbg("***** Bad VarName:" & thisVarName)
                                thisVarName = thisVarName.Replace("(", "")
                                thisVarName = thisVarName.Replace(")", "")
                            End If
                            .VarNames.Add(thisVarName, thisVarName)
                            'On Error GoTo 0
                            recbyte = recbyte + varLen + 4
                        End While

                        .Data = New atcCollection
                    End With
                    pHeaders.Add(myKey, myHeader)
                    'Debug.Print("header:" & myKey)
                Case 1 'data record
                    myKey = MakeIdAndKey(currec, myId)
                    'If pHeaders.ContainsKey(myKey) Then
                    Try
                        myData = New clsHspfBinData
                        With myData
                            .UnitFlag = BitConverter.ToInt32(currec, 24)
                            .OutLev = BitConverter.ToInt32(currec, 28)
                            For j = 0 To 4
                                .DateArray(j) = BitConverter.ToInt32(currec, 32 + (j * 4))
                            Next j
                            If .DateArray(4) = 60 And .DateArray(3) = 24 Then
                                .DateArray(4) = 0 'otherwise jdate is first hour in next day
                            End If
                            varCnt = pHeaders.ItemByKey(myKey).VarNames.Count
                            ReDim .value(varCnt - 1)
                            For j = 0 To varCnt - 1
                                .value(j) = BitConverter.ToSingle(currec, 52 + (j * 4))
                            Next j
                            myDataKey = .OutLev & ":" & Date2J(.DateArray)
                            Try
                                pHeaders.ItemByKey(myKey).Data.Add(myDataKey, myData) 'should this be a date string YYYY/MM/DD HH?
                            Catch e As Exception
                                Logger.Dbg("***** ReadNewRecords:Fail to add data to header " & e.ToString)
                            End Try
                        End With
                    Catch e As Exception
                        'Else
                        s = "***** Data Without Header for Key:" & myKey & " " & e.ToString
                        Logger.Dbg(s)
                        pErrorDescription = s
                        'End If
                    End Try
                Case Else
                    s = "***** Bad Record Type: " & BitConverter.ToInt32(currec, 0)
                    Logger.Dbg(s)
                    pErrorDescription = s
            End Select
            Logger.Progress(i, pFile.RecordCount * 2)
            'If pMonitorSet Then
            '    percent = (100 * i) / (pFile.RecordCount * 2)
            '    If percent > lastprog + 1 Then  ' update progress message
            '        s = "(PROGRESS " & CStr(percent) & ")"
            '        pMonitor.SendMonitorMessage(s)
            '        lastprog = percent
            '    End If
            'End If
            pFileRecordIndex = pFileRecordIndex + 1
        End While
    End Sub

    Private Function MakeIdAndKey(ByVal lRec() As Byte, ByVal lid As clsHspfBinId) As String
        With lid
            .OperationName = Trim(Byte2String(lRec, 4, 8))
            .OperationNumber = BitConverter.ToInt32(lRec, 12)
            .SectionName = Trim(Byte2String(lRec, 16, 8))
        End With
        MakeIdAndKey = HspfBinaryIdAsText(lid)

    End Function

    Private Function HspfBinaryIdAsText(ByVal lid As clsHspfBinId) As String
        With lid
            HspfBinaryIdAsText = .OperationName & ":" & .OperationNumber & ":" & .SectionName
        End With
    End Function

    Friend ReadOnly Property ErrorDescription() As String
        Get
            ErrorDescription = pErrorDescription
            pErrorDescription = ""
        End Get
    End Property

    Public Function BinaryValue(ByVal opKey As String, ByVal varKey As String, ByVal dateKey As String) As Single

        Dim HeaderIndex As Integer, VarIndex As Integer, DateIndex As Integer
        Dim val As Single

        val = -999.0#

        Try
            'If pHeaders.ContainsKey(opKey) Then
            HeaderIndex = pHeaders.ItemByKey(opKey)
            If pHeaders.ItemByIndex(HeaderIndex).VarNames.KeyExists(varKey) Then
                VarIndex = pHeaders.ItemByIndex(HeaderIndex).VarNames.IndexFromKey(varKey)
                If pHeaders.ItemByIndex(HeaderIndex).Data.KeyExists(dateKey) Then
                    DateIndex = pHeaders.ItemByIndex(HeaderIndex).Data.IndexFromKey(dateKey)
                    val = pHeaders.ItemByIndex(HeaderIndex).Data(DateIndex).value(VarIndex)
                Else
                    pErrorDescription = "Missing Datekey " & dateKey & _
                                        " in Date Key Collection for Header " & _
                                       HspfBinaryIdAsText(Header(HeaderIndex).id)
                End If
            Else
                pErrorDescription = "Missing Varkey " & varKey & _
                                   " in Variable Name Collection for Header " & _
                                   HspfBinaryIdAsText(Header(HeaderIndex).id)
            End If
        Catch
            'Else
            pErrorDescription = "Missing Opkey " & opKey & " in Headers Collection"
            'End If
        End Try
        Return val
    End Function
End Class
