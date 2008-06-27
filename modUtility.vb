Imports System.Data.OleDb

Module modUtility
    Friend Function HeaderString() As String
        'For debugging, put a dummy date in all output files so we can compare files and not find unimportant differences
        Return "6/13/2008 12:00:00 AM D4EM-SWAT Interface"
        'Return Date.Now.ToString & " D4EM-SWAT Interface"
    End Function

    Friend Function StringFname(ByVal aSubBasin As String, ByVal aType As String) As String
        If aSubBasin.Length > 5 Then
            Throw New ApplicationException("Subbasin " & aSubBasin & " Problem")
        End If
        If aType.StartsWith(".") Then aType = aType.Substring(1)
        Return aSubBasin.PadLeft(5, "0") & "0000." & aType
    End Function

    Friend Function StringFnameHRUs(ByVal aSub As String, ByVal aHRU As String) As String
        If aSub.Length > 5 OrElse aHRU.Length > 4 Then
            Throw New ApplicationException("Subbasin " & aSub & " Hru " & aHRU & " Problem")
        End If
        Return aSub.PadLeft(5, "0") & aHRU.PadLeft(4, "0")
    End Function

    ''' <summary>
    ''' Create fixed with column string for writing to text file
    ''' </summary>
    ''' <param name="aValue">Number to format</param>
    ''' <param name="aDec">Number of decimal digits</param>
    ''' <param name="aSpc">Spaces to add after number</param>
    ''' <param name="aLeft">???</param>
    ''' <returns>Formatted string</returns>
    ''' <remarks>TODO: simplify!</remarks>
    Friend Function MakeString(ByVal aValue As Object, ByVal aDec As Integer, ByVal aSpc As Integer, ByVal aLeft As Integer) As String
        Dim lBufthevalue As String
        aValue = FormatNumber(aValue, aDec, TriState.True, TriState.False, TriState.False)

        Dim lValue As Integer = 16 - Left(aValue.ToString, aLeft).Length
        If (lValue = 0) Then
            lBufthevalue = ""
        Else
            lBufthevalue = Space(lValue)
        End If
        Return lBufthevalue & aValue.ToString & Space(aSpc)
    End Function

    Friend Sub ReplaceNonAscii(ByVal aFilename As String)
        Dim lBytes As Byte() = IO.File.ReadAllBytes(aFilename)
        Dim lByteIndex As Integer = 0
        Dim lLastByte As Integer = lBytes.GetUpperBound(0)
        While lByteIndex < lBytes.Length
            If lBytes(lByteIndex) = 194 Then
                For lByteCopyTo As Integer = lByteIndex To lLastByte - 1
                    lBytes(lByteCopyTo) = lBytes(lByteCopyTo + 1)
                Next
                lLastByte -= 1
            Else
                lByteIndex += 1
            End If
        End While
        If lLastByte < lBytes.GetUpperBound(0) Then
            Array.Resize(lBytes, lLastByte + 1)
            IO.File.WriteAllBytes(aFilename, lBytes)
        End If
    End Sub

    Friend Function GroupOfStrings(ByVal aPattern As String, ByVal aCount As Integer, ByVal aDelimiter As String) As String
        GroupOfStrings = ""
        For i As Integer = 1 To aCount
            GroupOfStrings &= aPattern.Replace("#", i.ToString)
            If i < aCount Then GroupOfStrings &= aDelimiter
        Next
    End Function

    Friend Function ArrayToString(ByVal aArray() As Single, ByVal aDelimiter As String) As String
        ArrayToString = ""
        For i As Integer = 0 To aArray.GetUpperBound(0)
            ArrayToString &= aArray(i)
            If i < aArray.GetUpperBound(0) Then ArrayToString &= aDelimiter
        Next
    End Function

    Friend Function ArrayToString(ByVal aArray() As Double, ByVal aDelimiter As String) As String
        ArrayToString = ""
        For i As Integer = 0 To aArray.GetUpperBound(0)
            ArrayToString &= aArray(i)
            If i < aArray.GetUpperBound(0) Then ArrayToString &= aDelimiter
        Next
    End Function

    Friend Function ArrayToString(ByVal aArray() As String, ByVal aDelimiter As String) As String
        ArrayToString = ""
        For i As Integer = 0 To aArray.GetUpperBound(0)
            ArrayToString &= aArray(i)
            If i < aArray.GetUpperBound(0) Then ArrayToString &= aDelimiter
        Next
    End Function

    Friend Function QueryDB(ByVal aQuery As String, ByVal aConnection As OleDbConnection) As DataTable
        Dim lCommand As OleDbCommand = New OleDbCommand(aQuery, aConnection)
        lCommand.CommandTimeout = 30
        Dim lOleDbDataAdapter As New OleDbDataAdapter
        lOleDbDataAdapter.SelectCommand = lCommand
        Dim lDataSet As New DataSet
        Dim lTableName As String = "lTable"
        lOleDbDataAdapter.Fill(lDataSet, lTableName)
        Return lDataSet.Tables(lTableName)
    End Function

    Public Sub ExecuteNonQuery(ByVal aSQL As String, ByVal aConnection As OleDbConnection)
        'Dim lStartTime As Date = Date.Now
        Dim lCommand As New System.Data.OleDb.OleDbCommand(aSQL, aConnection)
        lCommand.CommandTimeout = 30
        lCommand.ExecuteNonQuery()
    End Sub

End Module
