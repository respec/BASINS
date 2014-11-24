Option Strict Off
Option Explicit On

Imports atcData
Imports MapWinUtility
Imports atcUtility
Imports System.Collections.ObjectModel

Friend Class HspfBinaryId
    Friend AsKey As String

    Friend Sub New(ByVal aKey As String)
        AsKey = aKey
    End Sub
    Friend Sub New(ByVal aOperationName As String, ByVal aOperationNumber As Integer, ByVal aSectionName As String)
        AsKey = aOperationName & ":" & aOperationNumber & ":" & aSectionName
    End Sub

    Friend Function OperationName() As String
        Return AsKey.Substring(0, AsKey.IndexOf(":"c))
    End Function

    Friend Function OperationNumber() As String
        Dim lStart As Integer = AsKey.IndexOf(":"c) + 1
        Dim lEnd As Integer = AsKey.IndexOf(":"c, lStart)
        Return AsKey.Substring(lStart, lEnd - lStart)
    End Function

    Friend Function SectionName() As String
        Return AsKey.Substring(AsKey.LastIndexOf(":"c) + 1)
    End Function
End Class

'Friend Class HspfBinaryData
'    Inherits KeyedCollection(Of String, HspfBinaryDatum)
'    Protected Overrides Function GetKeyForItem(ByVal aHspfBinaryDatum As HspfBinaryDatum) As String
'        Return aHspfBinaryDatum.AsKey
'    End Function
'End Class

'Friend Class HspfBinaryDatum
'    'Public OutLev As Byte = 0
'    Public JDate As Double = 0
'    Friend ValuesStartPosition As Long = 0
'    'Friend Function AsKey() As String
'    '    Return JDate
'    '    'Return OutLev & ":" & JDate
'    'End Function
'End Class

Friend Class HspfBinaryHeaders
    Inherits KeyedCollection(Of String, HspfBinaryHeader)

    Private Shared pVersion As Integer = &H48424E32 'HBN2 

    Protected Overrides Function GetKeyForItem(ByVal aHspfBinaryHeader As HspfBinaryHeader) As String
        Return aHspfBinaryHeader.Id.AsKey
    End Function

    Public Function Open(ByVal aFileName As String) As Boolean
        If FileExists(aFileName) Then
            'Logger.Status("Reading " & IO.Path.GetFileName(aFilename), True)
            Dim lFileStream As New IO.FileStream(aFileName, IO.FileMode.Open, IO.FileAccess.Read)
            Dim lReader As New IO.BinaryReader(lFileStream)
            Dim lVersion As Integer = lReader.ReadInt32
            If lVersion <> pVersion Then
                Logger.Dbg("BadMagicNumber for " & aFileName & " (" & Hex(lVersion) & "<>" & Hex(pVersion) & ")")
                lReader.Close()
                Return False
            Else
                Try
                    Me.Clear()

                    Dim lNumDates As Integer = lReader.ReadInt32
                    If lNumDates <= 0 Then
                        Logger.Dbg("Number of dates <= 0 for " & aFileName & " (" & lNumDates & ")")
                        lReader.Close()
                        Return False
                    End If
                    HspfBinaryHeader.AllDates.Clear()
                    HspfBinaryHeader.AllDates.Capacity = lNumDates
                    For lDatesIndex As Integer = 1 To lNumDates
                        Dim lReadDates As New atcTimeseries(Nothing)
                        Dim lTimeUnit As atcTimeUnit = lReader.ReadInt32
                        If lTimeUnit = atcTimeUnit.TUUnknown Then
                            lReadDates.numValues = lReader.ReadInt32
                            For lIndex As Integer = 0 To lReadDates.numValues
                                lReadDates.Value(lIndex) = lReader.ReadDouble
                            Next
                        Else
                            Dim lTimeStep As Integer = lReader.ReadInt32
                            Dim lStartDate As Double = lReader.ReadDouble
                            Dim lEndDate As Double = lReader.ReadDouble
                            lReadDates.Values = NewDates(lStartDate, lEndDate, lTimeUnit, lTimeStep)
                            lReadDates.SetInterval(lTimeUnit, lTimeStep)
                        End If
                        HspfBinaryHeader.AllDates.Add(lReadDates)
                    Next

                    Dim lNumHeaders As Integer = lReader.ReadInt32
                    If lNumHeaders <= 0 Then
                        Logger.Dbg("Number of headers <= 0 for " & aFileName & " (" & lNumHeaders & ")")
                        lReader.Close()
                        Return False
                    End If
                    For lHeaderIndex As Integer = 1 To lNumHeaders
                        Dim lHeader As New HspfBinaryHeader
                        lHeader.ReadFromHeader(lReader)
                        Me.Add(lHeader)
                        Logger.Progress(Me.Count / 2, lNumHeaders)
                    Next
                Catch ex As Exception
                    Logger.Dbg("Exception opening " & aFileName & vbCrLf & ex.ToString)
                    Me.Clear()
                    Return False
                Finally
                    lReader.Close()
                End Try
            End If
        End If
    End Function

    Public Sub SaveAs(ByVal aFileName As String)
        Logger.Dbg("Saving " & aFileName)
        Dim lWriter As IO.BinaryWriter = Nothing
        Try
            Dim lFileStream As New IO.FileStream(aFileName, IO.FileMode.Create)
            lWriter = New IO.BinaryWriter(lFileStream)
            lWriter.Write(pVersion)

            lWriter.Write(HspfBinaryHeader.AllDates.Count)
            For Each lDates As atcTimeseries In HspfBinaryHeader.AllDates
                Dim lTimeUnit As atcTimeUnit = atcTimeUnit.TUUnknown
                Dim lTimeStep As Integer = 1
                If lDates.numValues > 1 Then 'Compute Interval and beginning of first interval
                    Dim lInterval As Double = GetNaN()
                    CalcTimeUnitStep(lDates.Value(1), lDates.Value(2), lTimeUnit, lTimeStep)
                    If lTimeUnit <> atcTimeUnit.TUUnknown Then
                        lDates.Value(0) = TimAddJ(lDates.Value(1), lTimeUnit, lTimeStep, -1)
                        lDates.Attributes.SetValue("Time Step", lTimeStep)
                        lDates.Attributes.SetValue("Time Unit", lTimeUnit)
                        lInterval = CalcInterval(lTimeUnit, lTimeStep)
                        If Double.IsNaN(lInterval) Then
                            lDates.Attributes.RemoveByKey("Interval")
                        Else
                            lDates.Attributes.SetValue("Interval", lInterval)
                        End If
                    End If
                End If

                lWriter.Write(lTimeUnit)
                If lTimeUnit = atcTimeUnit.TUUnknown Then
                    lWriter.Write(lDates.numValues)
                    For lIndex As Integer = 0 To lDates.numValues
                        lWriter.Write(lDates.Value(lIndex))
                    Next
                Else
                    lWriter.Write(lTimeStep)
                    lWriter.Write(lDates.Value(0))
                    lWriter.Write(lDates.Value(lDates.numValues))
                End If
            Next

            lWriter.Write(Me.Count)
            For Each lHeader As HspfBinaryHeader In Me
                lHeader.Write(lWriter)
            Next
            Logger.Dbg("Saved " & aFileName)
        Catch ex As Exception
            Logger.Dbg("Exception Saving " & aFileName & vbCrLf & ex.ToString)
        Finally
            Try
                If lWriter IsNot Nothing Then lWriter.Close()
            Catch
            End Try
        End Try
    End Sub

End Class

Friend Class HspfBinaryHeader
    Public Shared AllDates As New Generic.List(Of atcTimeseries)
    Private Shared pUniqueVariableNames As New Generic.List(Of String)
    'Public Shared pDuplicateVariableNames As Integer = 0
    'Public Shared pTotalVariableLength As Long = 0
    'Public Shared pDuplicateVariableLength As Long = 0

    Public Id As HspfBinaryId
    Public VarNames As New Generic.List(Of String)

    'Public Data As New Generic.List(Of HspfBinaryDatum)
    Public ValuesStartPosition As New Generic.List(Of Long)
    Public Dates As atcTimeseries = Nothing

    ''' <summary>
    ''' Output Level: 2=BIVL, 3=day, 4=month, 5=year, 6=never
    ''' </summary>
    Public OutLev As Byte = 6 'Default to 6=never, will be replaced by shortest interval in file.

    Public Sub ReadFromHbn(ByVal lCurrentRecord() As Byte, ByVal aLength As Integer, ByVal aId As HspfBinaryId)
        Id = aId
        'Byte position within current header record     
        Dim lBytePosition As Integer = 20
        While lBytePosition < aLength
            Dim lVariableNameLength As Integer = BitConverter.ToInt32(lCurrentRecord, lBytePosition)
            Dim lVariableName As String = System.Text.ASCIIEncoding.ASCII.GetString(lCurrentRecord, _
                lBytePosition + 4, lVariableNameLength).Replace("("c, " "c).Replace(")"c, " "c).Trim
            AddVariable(lVariableName)
            lBytePosition += lVariableNameLength + 4
        End While
        'Logger.Dbg("Header:" & lHspfBinHeader.Id.AsKey)
    End Sub

    Public Sub ReadFromHeader(ByVal aReader As IO.BinaryReader)
        Id = New HspfBinaryId(aReader.ReadString)
        Dim lNumVars As Integer = aReader.ReadInt32
        VarNames.Capacity = lNumVars
        For lVarIndex As Integer = 1 To lNumVars
            AddVariable(aReader.ReadString)
        Next
        Dates = AllDates(aReader.ReadInt32)
        Dim lNumData As Integer = Dates.numValues ' aReader.ReadInt32
        ValuesStartPosition.Capacity = lNumData
        For lDataIndex As Integer = 1 To lNumData
            ValuesStartPosition.Add(aReader.ReadInt64)
        Next
    End Sub

    Private Sub AddVariable(ByVal aVariableName As String)
        Dim lVariableIndex As Integer = pUniqueVariableNames.IndexOf(aVariableName)
        If lVariableIndex < 0 Then
            lVariableIndex = pUniqueVariableNames.Count
            pUniqueVariableNames.Add(aVariableName)
            '    pTotalVariableLength += aVariableName.Length
            'Else
            '    pDuplicateVariableNames += 1
            '    pDuplicateVariableLength += aVariableName.Length
        End If
        VarNames.Add(pUniqueVariableNames(lVariableIndex))
    End Sub

    Public Sub Write(ByVal aWriter As IO.BinaryWriter)
        aWriter.Write(Id.AsKey)
        aWriter.Write(VarNames.Count)
        For Each lVarName As String In VarNames
            aWriter.Write(lVarName)
        Next
        aWriter.Write(AllDates.IndexOf(Dates))
        'aWriter.Write(ValuesStartPosition.Count)
        For Each lValuesStartPosition As Long In ValuesStartPosition
            aWriter.Write(lValuesStartPosition)
        Next
    End Sub
End Class

''' <summary>
''' 
''' </summary>
''' <remarks>
'''Copyright 2005-2008 AQUA TERRA Consultants - Royalty-free use permitted under open source license
'''</remarks>
Friend Class HspfBinary

    Public UnitSystem As atcUnitSystem = atcUnitSystem.atcUnknown

    Private pFileRecordIndex As Integer
    Private pHeaders As HspfBinaryHeaders
    Private Shared pNaN As Double = GetNaN()

    Private pFileName As String = ""
    Private pBr As IO.BinaryReader ' pFileNum As Integer = 0
    Private pBytesInFile As Long = 0
    Private pProgressMax As Integer = 0
    Private pDivideForProgress As Long = 1
    Private pSeek As Long
    'Private pRecords As New Generic.List(Of UnformattedRecord)

    'Private Class UnformattedRecord
    '    'Friend StartPosition As Integer = 0
    '    Friend Length As Integer = 0
    'End Class

    'Private Function RecordLength(ByVal aIndex As Integer) As Integer
    '    If aIndex >= 0 AndAlso aIndex < pRecords.Count Then
    '        Return pRecords(aIndex).Length
    '    Else
    '        Return Nothing
    '    End If
    'End Function

    'Private ReadOnly Property Record(ByVal aIndex As Integer) As Byte()
    '    Get
    '        If aIndex >= 0 AndAlso aIndex < pRecords.Count Then
    '            With pRecords(aIndex)
    '                If .Length > 0 Then 'fill in the data
    '                    Dim lNeedToClose As Boolean = Open()
    '                    Seek(pFileNum, .StartPosition)
    '                    Dim lRecord(.Length - 1) As Byte
    '                    FileGet(pFileNum, lRecord)
    '                    If lNeedToClose Then Close()
    '                    Return lRecord
    '                Else 'whats the problem?
    '                    Throw New ApplicationException("***** clsFtnUnfFile:Record#" & aIndex & ":Len=0:Start=" & .StartPosition & ":Lof=" & pBytesInFile & ":File=" & pFileName)
    '                End If
    '            End With
    '        End If
    '        Return Nothing
    '    End Get
    'End Property

    'Private Function ReadRecord(ByVal aFirst As Boolean) As UnformattedRecord
    '    Dim lUnfRec As New UnformattedRecord
    '    With lUnfRec
    '        .StartPosition = Seek(pFileNum)
    '        .Length = FtnUnfSeqRecLen(pFileNum, aFirst)
    '        'Debug.Print .StartPos, .Len
    '        'If .Length > 0 Then 'fill in the data
    '        '    ReDim .Record(.Length - 1)
    '        '    FileGet(pFileNum, .Record)
    '        'Else 'whats the problem?
    '        '    Logger.Dbg("***** clsFtnUnfFile:ReadRestOfRecordsInFile:Len=0:Start=" & .StartPosition & ":Lof=" & pBytesInFile & ":File=" & pFileName)
    '        'End If
    '        Seek(pFileNum, Seek(pFileNum) + .Length) 'Skip contents of record
    '    End With
    '    Return lUnfRec
    'End Function

    'Private Sub ReadRestOfRecordsInFile(Optional ByVal aFirst As Boolean = False)
    '    Dim lNeedToClose As Boolean = Open()
    '    Do While Seek(pFileNum) < pBytesInFile - 2
    '        Dim lUnfRec As UnformattedRecord = ReadRecord(aFirst)
    '        pRecords.Add(lUnfRec)
    '    Loop
    '    If lNeedToClose Then Close()
    'End Sub

    Private Function FtnUnfSeqRecLen(ByVal aFileUnit As IO.BinaryReader) As Integer ', ByRef aFirst As Boolean) As Integer
        Static mLengthLast As Integer

        Dim c As Integer
        Dim h As Integer
        Dim lByte As Byte

        If pFileRecordIndex = 0 Then
            mLengthLast = 0
        Else
            c = 64
            pBr.ReadByte()
            While mLengthLast >= c
                c *= 256
                pBr.ReadByte()
            End While
        End If
        lByte = pBr.ReadByte()
        Dim lBytes As Integer = lByte And 3
        Dim lRecordLength As Integer = Fix(CSng(lByte) / 4)
        c = 64
        h = lBytes + 1
        Do While lBytes > 0
            lByte = pBr.ReadByte()
            lBytes -= 1
            lRecordLength += lByte * c
            c *= 256
        Loop
        mLengthLast = lRecordLength + h
        Return lRecordLength
    End Function

    ''' <summary>
    ''' Only for private use or in atcTimeseriesFileHspfBinOut.ReadData
    ''' Open file if it is not already open. Seek to the position where we were when Close was called
    ''' </summary>
    ''' <returns>true if file needed to be opened, false if file was already open</returns>
    ''' <remarks>use in conjunction with Close()</remarks>
    Friend Function Open(ByVal aSeekToLastPosition As Boolean) As Boolean
        If pBr Is Nothing Then
            Dim lFS As IO.FileStream = IO.File.Open(pFileName, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.Read)
            pBr = New IO.BinaryReader(lFS) ' pFileNum = FreeFile()
            'FileOpen(pFileNum, pFileName, OpenMode.Binary, OpenAccess.Read, OpenShare.Shared)
            pBytesInFile = lFS.Length ' LOF(pFileNum)
            If pBytesInFile < Integer.MaxValue Then
                pDivideForProgress = 1
                pProgressMax = pBytesInFile
            Else 'Can't display actual progress in bytes using Integer, so use percent
                pDivideForProgress = pBytesInFile / 100
                pProgressMax = 100
            End If
            pDivideForProgress *= 2 'Progress of reading through file is only first half of progress of opening
            If aSeekToLastPosition AndAlso pSeek > 0 Then pBr.BaseStream.Seek(pSeek, IO.SeekOrigin.Begin)
            Return True
        Else
            Return False
        End If
    End Function

    Friend Sub Close(ByVal aSavePosition As Boolean)
        If pBr IsNot Nothing Then
            If aSavePosition Then pSeek = pBr.BaseStream.Position
            pBr.Close()
            pBr = Nothing
        End If
    End Sub

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
            Return pFileName
        End Get
        Set(ByVal aFileName As String)
            If Not FileExists(aFileName) Then
                Throw New ApplicationException("File '" & aFileName & "' not found")
            Else
                pFileName = aFileName
                Close(False)
                pSeek = 0
                Open(True)
                If pBytesInFile = 0 Then
                    Throw New ApplicationException("File '" & aFileName & "' is empty")
                Else
                    Dim lByte As Byte = pBr.ReadByte ' FileGet(pFileNum, lByte)
                    If lByte <> &HFD Then
                        Throw New ApplicationException("File: '" & aFileName & "' is not a Fortran Unformatted Sequential File" & vbCrLf & "(does not begin with hex FD)")
                    End If
                End If
            End If
            If pHeaders IsNot Nothing Then pHeaders.Clear()
            pHeaders = New HspfBinaryHeaders
            'Dim lOldHeaderFileName As String = IO.Path.ChangeExtension(aFileName, ".hbnheader")
            'If IO.File.Exists(lOldHeaderFileName) Then TryDelete(lOldHeaderFileName)
            Dim lHeaderFileName As String = IO.Path.ChangeExtension(aFileName, ".hbnhead")
            If IO.File.Exists(lHeaderFileName) Then
                If IO.File.GetLastWriteTime(lHeaderFileName) < IO.File.GetLastWriteTime(aFileName) Then
                    TryDelete(lHeaderFileName)
                Else
                    If Not pHeaders.Open(lHeaderFileName) Then
                        TryDelete(lHeaderFileName)
                    End If
                End If
            End If
            pFileRecordIndex = 0
            'If we did not manage to populate headers from header file above, have to read the main file
            If pHeaders.Count = 0 Then
                ReadNewRecords()
                pHeaders.SaveAs(lHeaderFileName)
            End If
            Close(True)
        End Set
    End Property

    'Friend Function ReadBytes(ByVal aStartPosition As Long, ByVal aNumBytes As Integer) As Byte()
    '    Dim lNeedToClose As Boolean = Open()
    '    Try
    '        pBr.BaseStream.Position = aStartPosition
    '        Return pBr.ReadBytes(aNumBytes)
    '    Finally
    '        If lNeedToClose Then Close()
    '    End Try
    'End Function

    Friend Function ReadValue(ByVal aStartPosition As Long, ByVal aIndex As Integer) As Double
        pBr.BaseStream.Position = aStartPosition + aIndex * 4
        Dim lValue As Double = BitConverter.ToSingle(pBr.ReadBytes(4), 0)
        If lValue < -1.0E+29 Then
            lValue = pNaN
        End If
        Return lValue
    End Function

    Friend Sub ReadNewRecords()
        Logger.Dbg("ReadNewRecords")
        'Dim lLog As New Text.StringBuilder
        Dim lNeedToClose As Boolean = Open(True)
        Try
            Dim DateArray(5) As Integer
            Dim lLength As Integer
            Dim lSeek As Long = pBr.BaseStream.Position
            While lSeek < pBytesInFile - 2
                lLength = FtnUnfSeqRecLen(pBr)
                If lLength <= 0 Then 'whats the problem?
                    Throw New ApplicationException("***** clsFtnUnfFile:ReadRestOfRecordsInFile:Len=0:Start=" & lSeek & ":Lof=" & pBytesInFile & ":File=" & pFileName)
                End If
                Dim lRecordFlag As Integer = pBr.ReadInt32
                lLength -= 4
                Select Case lRecordFlag
                    Case 0 'header record
                        Dim lHspfBinaryHeader As HspfBinaryHeader = New HspfBinaryHeader
                        Dim lCurrentRecord() As Byte = pBr.ReadBytes(lLength)
                        lHspfBinaryHeader.ReadFromHbn(lCurrentRecord, lLength, MakeId(lCurrentRecord))
                        pHeaders.Add(lHspfBinaryHeader)

                    Case 1 'data record
                        Dim lValuesStartPosition As Long = pBr.BaseStream.Position + 48
                        Dim lCurrentRecord() As Byte = pBr.ReadBytes(lLength)
                        Dim lHspfBinKey As String = MakeId(lCurrentRecord).AsKey
                        Try
                            Dim lHspfBinaryHeader As HspfBinaryHeader = pHeaders.Item(lHspfBinKey)
                            Dim lOutLev As Integer = BitConverter.ToInt32(lCurrentRecord, 24)
                            If lOutLev <= lHspfBinaryHeader.OutLev Then
                                If lOutLev < lHspfBinaryHeader.OutLev Then
                                    'Discard data headers from longer interval, only keep shortest interval
                                    lHspfBinaryHeader.ValuesStartPosition.Clear()
                                    lHspfBinaryHeader.Dates = Nothing
                                    lHspfBinaryHeader.OutLev = lOutLev
                                End If
                                If UnitSystem = atcUnitSystem.atcUnknown Then
                                    UnitSystem = BitConverter.ToInt32(lCurrentRecord, 20)
                                ElseIf UnitSystem <> BitConverter.ToInt32(lCurrentRecord, 20) Then
                                    Throw New ApplicationException("Different unit systems not supported in same file: " & UnitSystem & " vs. " & BitConverter.ToInt32(lCurrentRecord, 20))
                                End If
                                lHspfBinaryHeader.ValuesStartPosition.Add(lValuesStartPosition)
                                Dim lNewDateIndex As Integer = lHspfBinaryHeader.ValuesStartPosition.Count
                                For lDateIndex As Integer = 0 To 4
                                    DateArray(lDateIndex) = BitConverter.ToInt32(lCurrentRecord, 28 + (lDateIndex * 4))
                                Next lDateIndex
                                If DateArray(4) = 60 AndAlso DateArray(3) = 24 Then
                                    DateArray(4) = 0 'otherwise jdate is first hour in next day
                                End If

                                Dim lNewDate As Double = Date2J(DateArray)
                                Dim lMatch1 As Boolean = False

                                If lHspfBinaryHeader.Dates IsNot Nothing Then
                                    If lNewDateIndex <= lHspfBinaryHeader.Dates.numValues Then
                                        'Test whether this new date matches next date 
                                        If Math.Abs(lNewDate - lHspfBinaryHeader.Dates.Value(lNewDateIndex)) < JulianSecond / 2 Then
                                            lMatch1 = True
                                        Else
                                            'Mismatch, need to find or create new Dates
                                        End If
                                    ElseIf lNewDateIndex = lHspfBinaryHeader.Dates.numValues + 1 Then
                                        lMatch1 = True
                                        With lHspfBinaryHeader.Dates
                                            .numValues = lNewDateIndex
                                            .Value(lNewDateIndex) = lNewDate

                                            If lNewDateIndex = 2 Then
                                                Dim lInterval As Double = GetNaN()
                                                Dim lTimeStep As Integer = 1
                                                Dim lTimeUnit As atcTimeUnit = atcTimeUnit.TUUnknown
                                                CalcTimeUnitStep(.Value(1), .Value(2), lTimeUnit, lTimeStep)
                                                .Value(0) = TimAddJ(.Value(1), lTimeUnit, lTimeStep, -1)
                                                .SetInterval(lTimeUnit, lTimeStep)
                                            End If
                                        End With
                                    End If
                                End If

                                If Not lMatch1 Then
                                    Dim lMatch2 As Boolean = False
                                    For Each lDates As atcTimeseries In HspfBinaryHeader.AllDates
                                        lMatch2 = False
                                        If lHspfBinaryHeader.Dates Is Nothing Then
                                            lMatch2 = (lNewDateIndex = 1)
                                        ElseIf lDates.Serial <> lHspfBinaryHeader.Dates.Serial Then
                                            'Check whether the dates we already had before match with lDates
                                            lMatch2 = True
                                            For lOldIndex As Integer = 1 To lNewDateIndex - 1
                                                If Math.Abs(lDates.Value(lOldIndex) - lHspfBinaryHeader.Dates.Value(lOldIndex)) > JulianSecond / 2 Then
                                                    lMatch2 = False
                                                    Exit For
                                                End If
                                            Next
                                        End If
                                        If lMatch2 Then 'Make sure newest value matches, or add it
                                            If lDates.numValues >= lNewDateIndex Then
                                                If Math.Abs(lDates.Value(lNewDateIndex) - lNewDate) > JulianSecond / 2 Then
                                                    lMatch2 = False
                                                End If
                                            ElseIf lDates.numValues = lNewDateIndex - 1 Then
                                                lDates.numValues = lNewDateIndex
                                                lDates.Values(lNewDateIndex) = lNewDate
                                            Else
                                                lMatch2 = False
                                            End If
                                        End If
                                        If lMatch2 Then
                                            lHspfBinaryHeader.Dates = lDates
                                            Exit For
                                        End If
                                    Next
                                    If Not lMatch2 OrElse lHspfBinaryHeader.Dates Is Nothing Then
                                        lHspfBinaryHeader.Dates = New atcTimeseries(Nothing)
                                        With lHspfBinaryHeader.Dates
                                            .Attributes.SetValue("Shared", True)
                                            .numValues = 1
                                            .Value(0) = pNaN
                                            .Value(1) = lNewDate
                                            '.Attributes.SetValue("NumValuesSet", 0)
                                        End With
                                        HspfBinaryHeader.AllDates.Add(lHspfBinaryHeader.Dates)
                                    End If
                                End If

                                'We used to read all the values here, now we wait and read them in atcTimeseriesFileHspfBinOut.ReadData
                                'Dim lVariableCount As Integer = lHspfBinaryHeader.VarNames.Count
                                'Dim lValues(lVariableCount - 1) As Double
                                'For lVariableIndex As Integer = 0 To lVariableCount - 1
                                '    Try
                                '        Dim lValue As Double = BitConverter.ToSingle(lCurrentRecord, 48 + (lVariableIndex * 4))
                                '        If lValue < -1.0E+29 Then
                                '            lValues(lVariableIndex) = pNaN
                                '        Else
                                '            lValues(lVariableIndex) = lValue
                                '        End If
                                '    Catch e As Exception
                                '        Logger.Dbg("***** Problem Converting Data for Key:" & lHspfBinId.AsKey & " " & e.ToString & vbCrLf & " at record " & pFileRecordIndex)
                                '        lValues(lVariableIndex) = pNaN
                                '    End Try
                                'Next lVariableIndex
                                'Try
                                '    lHspfBinaryHeader.Data.Add(lHspfBinData)
                                'Catch e As Exception
                                '    Logger.Dbg("***** ReadNewRecords:Could not add data to header " & e.ToString & vbCrLf & " at record " & pFileRecordIndex)
                                '    Throw e
                                'End Try
                            End If
                        Catch eKey As System.Collections.Generic.KeyNotFoundException
                            Dim lString As String = "***** Data Without Header for Key:" & lHspfBinKey & " " & eKey.ToString & vbCrLf & " at record " & pFileRecordIndex
                            Logger.Dbg(lString)
                            Throw New ApplicationException(lString)
                        End Try

                    Case Else
                        Dim lString As String = "***** Bad Record Type: " & lRecordFlag & " at record " & pFileRecordIndex & " byte " & lSeek
                        Logger.Dbg(lString)
                        Throw New ApplicationException(lString)
                End Select
                lSeek = pBr.BaseStream.Position
                Logger.Progress(lSeek / pDivideForProgress, pProgressMax)
                pFileRecordIndex += 1
            End While
            For Each lHspfBinaryHeader As HspfBinaryHeader In pHeaders
                lHspfBinaryHeader.ValuesStartPosition.TrimExcess()
            Next
        Catch ex As Exception
            Logger.Progress(0, 0)
            If ex.Message = "User Canceled" Then
                Logger.Msg("Canceled reading " & Filename, MsgBoxStyle.Exclamation, "Did not finish reading HSPF Binary File")
            Else
                Logger.Msg(Filename & vbCrLf & ex.ToString, MsgBoxStyle.Exclamation, "Did not finish reading HSPF Binary File")
            End If
            Logger.Dbg(MemUsage)
            Throw ex
        Finally
            If lNeedToClose Then Close(True)
        End Try
        Logger.Dbg("Read " & Format(pFileRecordIndex, "###,###") & " records (" & Format(pBytesInFile, "###,###") & " bytes)")
        'IO.File.WriteAllText(pFileName & ".logNew.txt", lLog.ToString)
    End Sub

    'Private Function MakeId(ByVal aRecord() As Byte) As HspfBinaryId
    '    Dim lHspfBinId As New HspfBinaryId
    '    With lHspfBinId
    '        .OperationName = (Chr(aRecord(0)) & Chr(aRecord(1)) & Chr(aRecord(2)) & Chr(aRecord(3)) _
    '                        & Chr(aRecord(4)) & Chr(aRecord(5)) & Chr(aRecord(6)) & Chr(aRecord(7))).Trim
    '        .OperationNumber = BitConverter.ToInt32(aRecord, 8)
    '        .SectionName = (Chr(aRecord(12)) & Chr(aRecord(13)) & Chr(aRecord(14)) & Chr(aRecord(15)) _
    '                      & Chr(aRecord(16)) & Chr(aRecord(17)) & Chr(aRecord(18)) & Chr(aRecord(19))).Trim
    '    End With
    '    Return lHspfBinId
    'End Function

    Private Function MakeId(ByVal aRecord() As Byte) As HspfBinaryId
        Return New HspfBinaryId( _
            aOperationName:=Text.ASCIIEncoding.ASCII.GetString(aRecord, 0, 8).Trim, _
            aOperationNumber:=BitConverter.ToInt32(aRecord, 8), _
            aSectionName:=Text.ASCIIEncoding.ASCII.GetString(aRecord, 12, 8).Trim)
    End Function

    'Friend Function BinaryValue(ByVal aOperationKey As String, ByVal aVariableKey As String, ByVal aDateKey As String) As Single
    '    Dim lValue As Double = pNaN

    '    Try
    '        Dim lHeaderKey As String = pHeaders.Item(aOperationKey).Id.AsKey
    '        If pHeaders.Item(lHeaderKey).VarNames.IndexFromKey(aVariableKey) > -1 Then
    '            Dim lVarIndex As Integer = pHeaders.Item(lHeaderKey).VarNames.IndexFromKey(aVariableKey)
    '            Try
    '                lValue = pHeaders.Item(lHeaderKey).Data(aDateKey).Value(lVarIndex)
    '            Catch
    '                Throw New ApplicationException("Missing Datekey " & aDateKey & _
    '                                    " in Date Key Collection for Header " & _
    '                                   pHeaders(lHeaderKey).Id.AsKey
    '            End Try
    '        Else
    '            Throw New ApplicationException("Missing Varkey " & aVariableKey & _
    '                               " in Variable Name Collection for Header " & _
    '                                pHeaders(lHeaderKey).Id.AsKey
    '        End If
    '    Catch
    '        Throw New ApplicationException("Missing Opkey " & aOperationKey & " in Headers Collection"
    '    End Try
    '    Return lValue
    'End Function
End Class
