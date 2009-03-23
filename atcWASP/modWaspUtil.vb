Module modWaspUtil
    Public Function IntegerToAlphabet(ByVal aNumber As Integer) As String
        'given 1 returns 'A'
        'given 26 returns 'Z'
        'given 27 returns 'AA'
        'given 28 returns 'AB'
        Dim lResult As String = ""

        If (26 > aNumber) Then
            lResult = Chr(aNumber + 65)
        Else
            Dim lColumn As Integer
            Do
                lColumn = aNumber Mod 26
                aNumber = (aNumber \ 26) - 1
                lResult = Chr(lColumn + 65) + lResult
            Loop Until (aNumber < 0)
        End If

        Return lResult
    End Function
End Module
