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
    Public Value() As Double
    Public Sub New()
        ReDim DateArray(5)
    End Sub
End Class

Friend Class clsHspfBinHeader
    Public Id As New clsHspfBinId
    Public VarNames As atcCollection 'of String
    Public Data As atcCollection 'of HSPFBinaryData
End Class

''' <summary>
''' 
''' </summary>
''' <remarks>
'''Copyright 2005-2008 AQUA TERRA Consultants - Royalty-free use permitted under open source license
'''</remarks>
Friend Class clsHspfBinary
    Private pFileName As String
    Private pErrorDescription As String
    Private pFile As clsFtnUnfFile
    Private pFileRecordIndex As Integer

    Dim pHeaders As atcCollection 'of HSPFBinaryHeader

    Friend ReadOnly Property DateAsJulian(ByVal aHeaderIndex As Integer, ByVal aDataIndex As Integer) As Double
        Get
            Dim lData As clsHspfBinData = Header(aHeaderIndex).Data.ItemByIndex(aDataIndex)
            Return Date2J(lData.DateArray)
        End Get
    End Property

    Friend ReadOnly Property DateAsText(ByVal aHeaderIndex As Integer, ByVal aDataIndex As Integer) As String
        Get
            Dim lData As clsHspfBinData = Header(aHeaderIndex).Data.ItemByIndex(aDataIndex)
            With lData
                DateAsText = .DateArray(0) & "/" & .DateArray(1) & "/" & .DateArray(2) & " " & _
                             .DateArray(3) & ":" & .DateArray(4) & ":" & .DateArray(5)
            End With
        End Get
    End Property

    Friend ReadOnly Property Header(ByVal aHeaderIndex As Integer) As clsHspfBinHeader
        Get
            Return pHeaders.ItemByIndex(aHeaderIndex)
        End Get
    End Property

    Friend ReadOnly Property Headers() As atcCollection
        Get
            Return pHeaders
        End Get
    End Property

    Friend ReadOnly Property HeaderIdAsText(ByVal aHeaderIndex As Integer) As String
        Get
            Return HspfBinaryIdAsText(Header(aHeaderIndex).id)
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
        Dim lHspfBinId As New clsHspfBinId

        pFile.ReadRestOfRecordsInFile()
        While pFileRecordIndex < pFile.RecordCount
            Dim lCurrentRecord() As Byte = pFile.Rec(pFileRecordIndex)
            Select Case BitConverter.ToInt32(lCurrentRecord, 0)
                Case 0 'header record
                    Dim lHspfBinHeader As clsHspfBinHeader = New clsHspfBinHeader
                    With lHspfBinHeader
                        Dim lKey As String = MakeIdAndKey(lCurrentRecord, .id)
                        .VarNames = New atcCollection

                        'Byte position within current header record     
                        Dim lBytePosition As Integer = 24
                        While lBytePosition < UBound(lCurrentRecord)
                            Dim lVariableNameLength As Integer = BitConverter.ToInt32(lCurrentRecord, lBytePosition)
                            Dim lVariableName As String = Byte2String(lCurrentRecord, lBytePosition + 4, lVariableNameLength).Trim
                            'On Error Resume Next
                            If InStr(lVariableName, "(") Then
                                Logger.Dbg("***** Removing Parens From:" & lVariableName)
                                lVariableName = lVariableName.Replace("("c, " "c).Replace(")"c, " "c)
                            End If
                            .VarNames.Add(lVariableName, lVariableName)
                            'On Error GoTo 0
                            lBytePosition += lVariableNameLength + 4
                        End While

                        .Data = New atcCollection
                        pHeaders.Add(lKey, lHspfBinHeader)
                    End With
                    'Debug.Print("header:" & myKey)
                Case 1 'data record
                    Dim lKey As String = MakeIdAndKey(lCurrentRecord, lHspfBinId)
                    Try
                        Dim lHspfBinData As clsHspfBinData = New clsHspfBinData
                        With lHspfBinData
                            .UnitFlag = BitConverter.ToInt32(lCurrentRecord, 24)
                            .OutLev = BitConverter.ToInt32(lCurrentRecord, 28)
                            For j As Integer = 0 To 4
                                .DateArray(j) = BitConverter.ToInt32(lCurrentRecord, 32 + (j * 4))
                            Next j
                            If .DateArray(4) = 60 And .DateArray(3) = 24 Then
                                .DateArray(4) = 0 'otherwise jdate is first hour in next day
                            End If
                            Dim lVariableCount As Integer = pHeaders.ItemByKey(lKey).VarNames.Count
                            ReDim .value(lVariableCount - 1)
                            For j As Integer = 0 To lVariableCount - 1
                                .value(j) = BitConverter.ToSingle(lCurrentRecord, 52 + (j * 4))
                            Next j
                            Dim lDataKey As String = .OutLev & ":" & Date2J(.DateArray)
                            Try
                                pHeaders.ItemByKey(lKey).Data.Add(lDataKey, lHspfBinData) 'should this be a date string YYYY/MM/DD HH?
                            Catch e As Exception
                                Logger.Dbg("***** ReadNewRecords:Fail to add data to header " & e.ToString)
                            End Try
                        End With
                    Catch e As Exception
                        Dim lString As String = "***** Data Without Header for Key:" & lKey & " " & e.ToString
                        Logger.Dbg(lString)
                        pErrorDescription = lString
                    End Try
                Case Else
                    Dim lString As String = "***** Bad Record Type: " & BitConverter.ToInt32(lCurrentRecord, 0)
                    Logger.Dbg(lString)
                    pErrorDescription = lString
            End Select
            Logger.Progress(pFileRecordIndex, pFile.RecordCount * 2)
            pFileRecordIndex += 1
        End While
    End Sub

    Private Function MakeIdAndKey(ByVal aRecord() As Byte, ByVal aHspfBinId As clsHspfBinId) As String
        With aHspfBinId
            .OperationName = Trim(Byte2String(aRecord, 4, 8))
            .OperationNumber = BitConverter.ToInt32(aRecord, 12)
            .SectionName = Trim(Byte2String(aRecord, 16, 8))
        End With
        Return HspfBinaryIdAsText(aHspfBinId)
    End Function

    Private Function HspfBinaryIdAsText(ByVal aHspfBinId As clsHspfBinId) As String
        With aHspfBinId
            Return .OperationName & ":" & .OperationNumber & ":" & .SectionName
        End With
    End Function

    Friend ReadOnly Property ErrorDescription() As String
        Get
            ErrorDescription = pErrorDescription
            pErrorDescription = ""
        End Get
    End Property

    Public Function BinaryValue(ByVal aOperationKey As String, ByVal aVariableKey As String, ByVal aDateKey As String) As Single
        Dim lValue As Double = Double.NaN

        Try
            Dim lHeaderIndex As Integer = pHeaders.ItemByKey(aOperationKey)
            If pHeaders.ItemByIndex(lHeaderIndex).VarNames.KeyExists(aVariableKey) Then
                Dim lVarIndex As Integer = pHeaders.ItemByIndex(lHeaderIndex).VarNames.IndexFromKey(aVariableKey)
                Dim lDateIndex As Integer
                If pHeaders.ItemByIndex(lHeaderIndex).Data.KeyExists(aDateKey) Then
                    lDateIndex = pHeaders.ItemByIndex(lHeaderIndex).Data.IndexFromKey(aDateKey)
                    lValue = pHeaders.ItemByIndex(lHeaderIndex).Data(lDateIndex).value(lVarIndex)
                Else
                    pErrorDescription = "Missing Datekey " & aDateKey & _
                                        " in Date Key Collection for Header " & _
                                       HspfBinaryIdAsText(Header(lHeaderIndex).id)
                End If
            Else
                pErrorDescription = "Missing Varkey " & aVariableKey & _
                                   " in Variable Name Collection for Header " & _
                                   HspfBinaryIdAsText(Header(lHeaderIndex).id)
            End If
        Catch
            pErrorDescription = "Missing Opkey " & aOperationKey & " in Headers Collection"
        End Try
        Return lValue
    End Function
End Class
