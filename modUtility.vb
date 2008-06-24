Imports System.Data.OleDb

Module modUtility
    Friend Function DateNowString() As String
        'For debugging, put a dummy date in all output files so we can compare files and not find unimportant differences
        Return "6/13/2008 12:00:00 AM"
        'Return Date.Now.ToString
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
End Module
