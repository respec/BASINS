Imports System.Data.OleDb

Module modUtility
    Friend Function StringFnameSubBasins(ByVal aSub As String) As String
        If aSub.Length > 5 Then
            Throw New ApplicationException("Subbasin " & aSub & " Problem")
        End If
        Return aSub.PadLeft(5, "0") & "0000"
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
        Return lBufthevalue + aValue.ToString + Space(aSpc)
    End Function
End Module
