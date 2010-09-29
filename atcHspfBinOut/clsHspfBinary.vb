Option Strict Off
Option Explicit On

Imports MapWinUtility
Imports atcUtility
Imports System.Collections.ObjectModel

Friend Class HspfBinaryId
    Friend OperationName As String = ""
    Friend OperationNumber As Integer = 0
    Friend SectionName As String = ""
    Friend ReadOnly Property AsKey() As String
        Get
            Return OperationName & ":" & OperationNumber & ":" & SectionName
        End Get
    End Property
End Class

Friend Class HspfBinaryData
    Inherits KeyedCollection(Of String, HspfBinaryDatum)
    Protected Overrides Function GetKeyForItem(ByVal aHspfBinaryDatum As HspfBinaryDatum) As String
        Return aHspfBinaryDatum.AsKey
    End Function
End Class

Friend Class HspfBinaryDatum
    Public UnitFlag As Integer = 0
    Public OutLev As Integer = 0
    Public DateArray(5) As Integer
    Public Value() As Double
    Friend ReadOnly Property AsKey() As String
        Get
            Return OutLev & ":" & Date2J(DateArray)
        End Get
    End Property
End Class

Friend Class HspfBinaryHeaders
    Inherits KeyedCollection(Of String, HspfBinaryHeader)
    Protected Overrides Function GetKeyForItem(ByVal aHspfBinaryHeader As HspfBinaryHeader) As String
        Return aHspfBinaryHeader.Id.AsKey
    End Function
End Class

Friend Class HspfBinaryHeader
    Public Id As New HspfBinaryId
    Public VarNames As atcCollection 'of String
    Public Data As HspfBinaryData
End Class

''' <summary>
''' 
''' </summary>
''' <remarks>
'''Copyright 2005-2008 AQUA TERRA Consultants - Royalty-free use permitted under open source license
'''</remarks>
Friend Class HspfBinary
    Private pFileName As String
    Private pErrorDescription As String
    Private pFile As clsFtnUnfFile
    Private pFileRecordIndex As Integer
    Private pHeaders As HspfBinaryHeaders

    'Friend ReadOnly Property DateAsJulian(ByVal aHeaderIndex As Integer, ByVal aDataIndex As Integer) As Double
    '    Get
    '        Dim lData As HspfBinaryData = Header(aHeaderIndex).Data.ItemByIndex(aDataIndex)
    '        Return Date2J(lData.DateArray)
    '    End Get
    'End Property

    'Friend ReadOnly Property DateAsText(ByVal aHeaderIndex As Integer, ByVal aDataIndex As Integer) As String
    '    Get
    '        Dim lData As HspfBinaryData = Header(aHeaderIndex).Data.ItemByIndex(aDataIndex)
    '        With lData
    '            DateAsText = .DateArray(0) & "/" & .DateArray(1) & "/" & .DateArray(2) & " " & _
    '                         .DateArray(3) & ":" & .DateArray(4) & ":" & .DateArray(5)
    '        End With
    '    End Get
    'End Property

    'Friend ReadOnly Property Header(ByVal aHeaderIndex As Integer) As HspfBinaryHeader
    '    Get
    '        If aHeaderIndex >= 0 AndAlso aHeaderIndex < pHeaders.Count Then
    '            Return pHeaders.Item(aHeaderIndex)
    '        Else
    '            Return Nothing
    '        End If
    '    End Get
    'End Property

    Friend ReadOnly Property Headers() As HspfBinaryHeaders
        Get
            Return pHeaders
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
            If pErrorDescription.Length = 0 Then
                pHeaders = Nothing
                pHeaders = New HspfBinaryHeaders
                pFileRecordIndex = 0
                ReadNewRecords()
            End If
        End Set
    End Property

    Friend Sub ReadNewRecords()
        Logger.Dbg("ReadNewRecords")
        pFile.ReadRestOfRecordsInFile()
        While pFileRecordIndex < pFile.RecordCount
            Dim lCurrentRecord() As Byte = pFile.Record(pFileRecordIndex)
            Select Case BitConverter.ToInt32(lCurrentRecord, 0)
                Case 0 'header record
                    Dim lHspfBinHeader As HspfBinaryHeader = New HspfBinaryHeader
                    With lHspfBinHeader
                        .Id = MakeId(lCurrentRecord)
                        .VarNames = New atcCollection

                        'Byte position within current header record     
                        Dim lBytePosition As Integer = 24
                        While lBytePosition < UBound(lCurrentRecord)
                            Dim lVariableNameLength As Integer = BitConverter.ToInt32(lCurrentRecord, lBytePosition)
                            Dim lVariableName As String = Byte2String(lCurrentRecord, lBytePosition + 4, lVariableNameLength).Trim
                            If lVariableName.IndexOf("(") > -1 Then
                                Logger.Dbg("***** Removing Parens From:" & lVariableName)
                                lVariableName = lVariableName.Replace("("c, " "c).Replace(")"c, " "c)
                            End If
                            .VarNames.Add(lVariableName, lVariableName)
                            lBytePosition += lVariableNameLength + 4
                        End While

                        .Data = New HspfBinaryData
                        pHeaders.Add(lHspfBinHeader)
                        'Logger.Dbg("Header:" & lHspfBinHeader.Id.AsKey)
                    End With
                Case 1 'data record
                    Dim lHspfBinId As HspfBinaryId = MakeId(lCurrentRecord)
                    Try
                        Dim lHspfBinaryHeader As HspfBinaryHeader = pHeaders.Item(lHspfBinId.AsKey)
                        Dim lHspfBinData As HspfBinaryDatum = New HspfBinaryDatum
                        With lHspfBinData
                            .UnitFlag = BitConverter.ToInt32(lCurrentRecord, 24)
                            .OutLev = BitConverter.ToInt32(lCurrentRecord, 28)
                            For lDateIndex As Integer = 0 To 4
                                .DateArray(lDateIndex) = BitConverter.ToInt32(lCurrentRecord, 32 + (lDateIndex * 4))
                            Next lDateIndex
                            If .DateArray(4) = 60 And .DateArray(3) = 24 Then
                                .DateArray(4) = 0 'otherwise jdate is first hour in next day
                            End If
                            Dim lVariableCount As Integer = lHspfBinaryHeader.VarNames.Count
                            ReDim .Value(lVariableCount - 1)
                            For lVariableIndex As Integer = 0 To lVariableCount - 1
                                Try
                                    Dim lValue As Double = BitConverter.ToSingle(lCurrentRecord, 52 + (lVariableIndex * 4))
                                    If lValue < -1.0E+29 Then lValue = GetNaN()
                                    .Value(lVariableIndex) = lValue
                                Catch e As Exception
                                    Logger.Dbg("***** Problem Converting Data for Key:" & lHspfBinId.AsKey & " " & e.ToString & vbCrLf & " at record " & pFileRecordIndex & " of " & pFile.RecordCount)
                                    .Value(lVariableIndex) = GetNaN()
                                End Try
                            Next lVariableIndex
                            Dim lDataKey As String = .OutLev & ":" & Date2J(.DateArray)
                            Try
                                lHspfBinaryHeader.Data.Add(lHspfBinData) 'should this be a date string YYYY/MM/DD HH?
                            Catch e As Exception
                                Logger.Dbg("***** ReadNewRecords:Fail to add data to header " & e.ToString & vbCrLf & " at record " & pFileRecordIndex & " of " & pFile.RecordCount)
                            End Try
                        End With
                    Catch e As Exception
                        Dim lString As String = "***** Data Without Header for Key:" & lHspfBinId.AsKey & " " & e.ToString & vbCrLf & " at record " & pFileRecordIndex & " of " & pFile.RecordCount
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
        Logger.Dbg("DoneRecord " & pFileRecordIndex)
    End Sub

    Private Function MakeId(ByVal aRecord() As Byte) As HspfBinaryId
        Dim lHspfBinId As New HspfBinaryId
        With lHspfBinId
            .OperationName = Trim(Byte2String(aRecord, 4, 8))
            .OperationNumber = BitConverter.ToInt32(aRecord, 12)
            .SectionName = Trim(Byte2String(aRecord, 16, 8))
        End With
        Return lHspfBinId
    End Function

    Friend ReadOnly Property ErrorDescription() As String
        Get
            ErrorDescription = pErrorDescription
            pErrorDescription = ""
        End Get
    End Property

    'Friend Function BinaryValue(ByVal aOperationKey As String, ByVal aVariableKey As String, ByVal aDateKey As String) As Single
    '    Dim lValue As Double = GetNaN

    '    Try
    '        Dim lHeaderKey As String = pHeaders.Item(aOperationKey).Id.AsKey
    '        If pHeaders.Item(lHeaderKey).VarNames.IndexFromKey(aVariableKey) > -1 Then
    '            Dim lVarIndex As Integer = pHeaders.Item(lHeaderKey).VarNames.IndexFromKey(aVariableKey)
    '            Try
    '                lValue = pHeaders.Item(lHeaderKey).Data(aDateKey).Value(lVarIndex)
    '            Catch
    '                pErrorDescription = "Missing Datekey " & aDateKey & _
    '                                    " in Date Key Collection for Header " & _
    '                                   pHeaders(lHeaderKey).Id.AsKey
    '            End Try
    '        Else
    '            pErrorDescription = "Missing Varkey " & aVariableKey & _
    '                               " in Variable Name Collection for Header " & _
    '                                pHeaders(lHeaderKey).Id.AsKey
    '        End If
    '    Catch
    '        pErrorDescription = "Missing Opkey " & aOperationKey & " in Headers Collection"
    '    End Try
    '    Return lValue
    'End Function
End Class
