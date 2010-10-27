Option Strict Off
Option Explicit On

''' <summary>
''' General utility subroutines and functions shared by many projects
''' </summary>
''' <remarks>Mark Gray and Jack Kittle of AQUA TERRA CONSULTANTS 2003-2010
''' Copyright 2010 AQUA TERRA Consultants - Royalty-free use permitted under open source license
''' </remarks>
Public Module modString

    ''' <summary>
    ''' Assign expression to result and return result
    ''' </summary>
    ''' <param name="aResult">output value of expression</param>
    ''' <param name="aExpression">input value of expression</param>
    ''' <returns>output value of expression</returns>
    ''' <remarks>use this like this 
    '''    While Assign(variable,expression) > 0
    '''       'use variable as needed
    '''    End While
    ''' </remarks>
    Function Assign(ByRef aResult As Integer, ByVal aExpression As Integer) As Integer
        aResult = aExpression
        Return aResult
    End Function
    Function Assign(ByRef aResult As Double, ByVal aExpression As Double) As Double
        aResult = aExpression
        Return aResult
    End Function
    Function Assign(ByRef aResult As String, ByVal aExpression As String) As String
        aResult = aExpression
        Return aResult
    End Function

    Public Function CapitalizeFirstLetter(ByVal aStr As String) As String
        If aStr Is Nothing OrElse aStr.Length < 1 Then
            Return ""
        ElseIf aStr.Length = 1 Then
            Return aStr.ToUpper
        Else
            Return aStr.Substring(0, 1).ToUpper & aStr.Substring(1).ToLower
        End If
    End Function

    Function RightJustify(ByVal aValue As Object, ByVal aWidth As Integer) As String
        Dim lStr As String = CStr(aValue)
        If lStr.Length >= aWidth Then
            Return lStr
        Else
            Return lStr.PadLeft(aWidth)
        End If
    End Function

    ''' <summary>
    ''' Round a double-precision number to the specified number of significant digits.
    ''' </summary>
    ''' <param name="aValue">Double-precision number to be formatted</param>
    ''' <param name="aDigits">Number of significant digits</param>
    ''' <returns>aValue rounded to aDigits significant digits.</returns>
    ''' <remarks>Example: Signif(1.23456, 3) =  1.23</remarks>
    Function SignificantDigits(ByVal aValue As Double, ByVal aDigits As Integer) As Double
        Dim lCurPower As Integer 'order of magnitude of val
        Dim lNegative As Boolean 'true if incoming number is negative
        Dim lShiftPower As Double 'magnitude by which val is temporarily shifted

        If aValue < 0 Then 'Have to use a positive number with Log10 below
            lNegative = True
            aValue = -aValue
        End If

        Try
            If Not Double.IsInfinity(aValue) AndAlso Not Double.IsNaN(aValue) AndAlso aValue > 0 Then
                lCurPower = Fix(Math.Log10(aValue))
                If aValue >= 1 Then
                    lCurPower += 1
                End If
                lShiftPower = 10 ^ (aDigits - lCurPower)
                aValue = aValue * lShiftPower 'Shift val so number of digits before decimal = significant digits
                aValue = Fix(aValue + 0.5) 'Round up if needed
                aValue = aValue / lShiftPower 'Shift val back to where it belongs

                If lNegative Then
                    aValue = -aValue
                End If
            End If
        Catch e As Exception
            Debug.Print("atcUtility:modString:SignificantDigits:error " & e.Message)
        End Try

        Return aValue
    End Function

    Function DecimalAlign(ByVal aValue As Double, _
                 Optional ByVal aFieldWidth As Integer = 12, _
                 Optional ByVal aDecimalPlaces As Integer = 3, _
                 Optional ByVal aSignificantDigits As Integer = 5) As String
        Dim lString As String
        If Double.IsNaN(aValue) Then
            If aFieldWidth > 5 Then
                lString = Space(aFieldWidth / 2) & "NaN"
            Else
                lString = "NaN"
            End If
        Else
            Dim lFormat As String = "###,##0."
            If aDecimalPlaces > 0 Then
                lFormat &= StrDup(aDecimalPlaces, "#")
            End If

            If aSignificantDigits > 0 Then
                lString = Format(SignificantDigits(aValue, aSignificantDigits), lFormat)
            Else
                lString = Format(aValue, lFormat)
            End If

            Dim lDString As String = DoubleToString(aValue, aFieldWidth, lFormat, , , aSignificantDigits)
            If Not lString.Equals(lDString) Then
                MapWinUtility.Logger.Dbg("DecimalAlign = '" & lString & "' DoubleToString = '" & lDString & "' Width=" & aFieldWidth & ", Decimals = " & aDecimalPlaces & ", Sig = " & aSignificantDigits & ", value = " & Format(aValue, "#,###.###############"))
            End If

        End If
        Return DecimalAlign(lString, aFieldWidth, aDecimalPlaces)
    End Function

    Function DecimalAlign(ByVal aValue As String, _
                          ByVal aFieldWidth As Integer, _
                          ByVal aWidthAfterDecimal As Integer) As String

        Dim lDecimalPosition As Integer = aValue.IndexOf("."c)
        If lDecimalPosition < 0 Then
            Return aValue.PadRight(aValue.Length + aWidthAfterDecimal + 1).PadLeft(aFieldWidth)
        Else
            'Width already after decimal = (aValue.Length - lDecimalPosition - 1)
            Dim lAddRight As Integer = aWidthAfterDecimal - (aValue.Length - lDecimalPosition - 1)
            Select Case lAddRight
                Case Is > 0 : aValue &= Space(lAddRight)
                Case Is < 0
                    If Not aValue.Contains("e") Then
                        Select Case aValue.Length + lAddRight
                            Case Is <= 0 : aValue = ""
                            Case lDecimalPosition : aValue = aValue.Substring(0, lDecimalPosition)
                            Case Is > lDecimalPosition
                                aValue = aValue.Substring(0, aValue.Length + lAddRight)
                                While aValue.EndsWith("0")
                                    aValue = aValue.Substring(0, aValue.Length - 1)
                                End While
                            Case Else
                                aValue = aValue.Substring(0, aValue.Length + lAddRight)
                        End Select
                    End If
            End Select
            Return aValue.PadLeft(aFieldWidth)
        End If
    End Function

    ''' <summary>
    ''' Reformat an array of floating point numbers around location of decimal place.
    ''' </summary>
    ''' <param name="aNumericStrings">floating point numbers as strings</param>
    ''' <param name="PadLeft">False to disable adding spaces left of numbers</param>
    ''' <param name="PadRight">False to disable adding spaces right of numbers</param>
    ''' <param name="MinWidth">Minimum width of resulting strings</param>
    ''' <remarks>aNumericStrings is both an input and output argument</remarks>
    Public Sub DecimalAlign(ByRef aNumericStrings() As String, Optional ByRef PadLeft As Boolean = True, Optional ByRef PadRight As Boolean = True, Optional ByRef MinWidth As Integer = 0)
        Dim lMaxDecimalPos As Integer   'furthest decimal position from left for all numbers in s
        Dim lMaxAfterDecimal As Integer 'furthest decimal position from right for all numbers in s
        Dim lAfterDecimal() As Integer  'array of digits after decimal
        Dim lDecimalPos() As Integer    'array of decimal positions from left
        Dim lIndex As Integer 'loop counter
        Dim lMaxIndex As Integer = aNumericStrings.GetUpperBound(0)

        ReDim lDecimalPos(lMaxIndex)
        ReDim lAfterDecimal(lMaxIndex)
        For lIndex = 0 To lMaxIndex
            lDecimalPos(lIndex) = InStr(aNumericStrings(lIndex), ".")
            If lDecimalPos(lIndex) = 0 Then lDecimalPos(lIndex) = Len(aNumericStrings(lIndex)) + 1
            If lDecimalPos(lIndex) > lMaxDecimalPos Then lMaxDecimalPos = lDecimalPos(lIndex)
            If PadRight Then
                lAfterDecimal(lIndex) = Len(aNumericStrings(lIndex)) - lDecimalPos(lIndex)
                If lAfterDecimal(lIndex) > lMaxAfterDecimal Then lMaxAfterDecimal = lAfterDecimal(lIndex)
            End If
        Next
        For lIndex = 0 To lMaxIndex
            If PadLeft Then
                If lDecimalPos(lIndex) < lMaxDecimalPos Then
                    aNumericStrings(lIndex) = Space(lMaxDecimalPos - lDecimalPos(lIndex)) & aNumericStrings(lIndex)
                End If
            End If
            If PadRight Then
                If lAfterDecimal(lIndex) < lMaxAfterDecimal Then
                    aNumericStrings(lIndex) = aNumericStrings(lIndex) & Space(lMaxAfterDecimal - lAfterDecimal(lIndex))
                End If
            End If
            If MinWidth > 0 Then aNumericStrings(lIndex) = StrPad(aNumericStrings(lIndex), MinWidth)
        Next

    End Sub

    Function DoubleToString(ByVal aValue As Double, _
                   Optional ByVal aMaxWidth As Integer = 10, _
                   Optional ByVal aFormat As String = Nothing, _
                   Optional ByVal aExpFormat As String = Nothing, _
                   Optional ByVal aCantFit As String = "#", _
                   Optional ByVal aSignificantDigits As Integer = 5) As String
        Dim lValue As Double
        If aSignificantDigits > 0 Then
            lValue = SignificantDigits(aValue, aSignificantDigits)
        Else
            lValue = aValue
        End If

        If aFormat Is Nothing OrElse aFormat.Length = 0 Then
            If aMaxWidth < 3 Then
                aFormat = "#.#"
            Else
                aFormat = "#,##0." & StrDup(aMaxWidth - 2, "#")
            End If
        End If

        Dim lString As String = Format(lValue, aFormat)
        Dim lOldString As String = ""

        If lString.Length > aMaxWidth Then 'Too long, need to shorten
            'First try leaving out commas to shorten
            Dim lDecimalPos As Integer = lString.IndexOf(".")
            Select Case lDecimalPos
                Case -1 'string does not contain a decimal, can't shorten by formatting with fewer decimal places
                    GoTo TryExpFormat
                Case aMaxWidth, aMaxWidth - 1 'string contains decimal point at end of max width, format without decimal places
                    lString = Format(lValue, "0")
                Case Is < aMaxWidth 'string contains a decimal before max width, format with fewer decimal places
                    If lValue >= 0 Then
                        lString = Format(lValue, "0." & StrDup(aMaxWidth - lDecimalPos, "#"))
                    Else
                        lString = Format(lValue, "0." & StrDup(aMaxWidth - lDecimalPos - 1, "#"))
                    End If
            End Select
        End If

        Dim lStrToDbl As Double = GetNaN()
        Try 'If formatted string cannot be parsed, skip to exponential format
            If Not Double.TryParse(lString, lStrToDbl) Then
                GoTo TryExpFormat
            End If

            'If formatted string changes the value too much, skip to exponential format
            Dim lAbs As Double = Math.Abs(aValue)
            If lAbs > 1.0E-30 _
                AndAlso (lAbs < 0.01 OrElse lAbs > 99999) _
                AndAlso (Math.Abs((aValue - lStrToDbl) / aValue) > (1 / 10 ^ (aSignificantDigits - 2))) Then
                lOldString = lString
                GoTo TryExpFormat
            Else
                lStrToDbl = GetNaN()
            End If
        Catch
            lStrToDbl = GetNaN()
            GoTo TryExpFormat
        End Try

        If lString.Length > aMaxWidth Then 'String is too long and cannot simply be truncated at or after decimal point
TryExpFormat:
            Dim lExpPos As Integer
            Dim lDecimalCount As Integer = 0
            If aExpFormat Is Nothing OrElse aExpFormat.Length = 0 Then
                Select Case aMaxWidth
                    Case Is < 3
                        lString = aCantFit
                        lDecimalCount = -1
                        GoTo TryOldString
                    Case 3, 4
                        lDecimalCount = 0
                    Case Else
                        lDecimalCount = aMaxWidth - 4
                End Select
                aExpFormat = "0." & StrDup(lDecimalCount, "#") & "e0"
            Else
                Dim lDecimalPos As Integer = aExpFormat.IndexOf(".")
                If lDecimalPos >= 0 Then
                    lExpPos = aExpFormat.IndexOf("e", lDecimalPos + 1)
                    lDecimalCount = lExpPos - lDecimalPos - 1
                End If
            End If

ReFormatExp:
            lString = Format(lValue, aExpFormat).TrimEnd("e")
            If lString.Length > aMaxWidth Then
                If lDecimalCount > 0 Then 'Try again with fewer decimal places
                    lDecimalCount -= 1
                    aExpFormat = "0." & StrDup(lDecimalCount, "#") & "e0"
                    GoTo ReFormatExp
                Else 'Ran out of decimal places to remove, just can't fit
                    lString = aCantFit
                End If
            End If
TryOldString:
            'If we had a candidate string before trying exponent, use it if it was at least as good
            If lOldString.Length > 0 Then
                Dim lStrToDblExp As Double
                If Double.TryParse(lString, lStrToDblExp) Then
                    If Math.Abs(aValue - lStrToDbl) <= Math.Abs(aValue - lStrToDblExp) Then
                        lString = lOldString
                    End If
                Else
                    lString = lOldString
                End If
            End If
        End If
        Return lString
    End Function

    ''' <summary>
    ''' Checks to see whether string argument is an integer.
    ''' </summary>
    ''' <param name="aStr">String to be checked for integer status</param>
    ''' <returns>True if each character in string is in range [0-9]</returns>
    ''' <remarks>
    ''' Example: IsInteger(12345) = True
    ''' Example: IsInteger(123.45) = False
    ''' </remarks>
    Public Function IsInteger(ByRef aStr As String) As Boolean
        Dim lResult As Integer
        Return Integer.TryParse(aStr, lResult)
    End Function

    ''' <summary>
    ''' Checks to see whether incoming string is entirely alphabetic.
    ''' </summary>
    ''' <param name="aStr">String to be checked for alphabetic status</param>
    ''' <returns>True if input parameter istr contains only [A-Z] or [a-z]</returns>
    ''' <remarks>
    ''' Example: IsAlpha(abcde) = True
    ''' Example: IsAlpha(abc123) = False
    ''' </remarks>
    Public Function IsAlpha(ByRef aStr As String) As Boolean
        For Each lChar As Char In aStr.ToCharArray
            If Not Char.IsLetter(lChar) Then Return False
        Next
        Return True
    End Function

    ''' <summary>
    ''' Checks to see whether incoming string is entirely alphanumeric.
    ''' </summary>
    ''' <param name="aStr">String to be checked for alphabetic status</param>
    ''' <returns>True if input parameter istr contains only [A-Z] or [a-z] or [0-9]</returns>
    ''' <remarks>
    ''' Example: IsAlpha(abcde) = True
    ''' Example: IsAlpha(abc123) = True
    ''' Example: IsAlpha(!#?$) = False
    ''' </remarks>
    Public Function IsAlphaNumeric(ByRef aStr As String) As Boolean
        For Each lChar As Char In aStr.ToCharArray
            If Not Char.IsLetterOrDigit(lChar) Then Return False
        Next
        Return True
    End Function

    ''' <summary>
    ''' Checks to see whether incoming byte is printable.
    ''' </summary>
    ''' <param name="aByte">Byte to be checked for printable status</param>
    ''' <returns>True if argument is ASCII code 9, 10, 12, 13, 32 - 126</returns>
    ''' <remarks>
    ''' Example: ByteIsPrintable(44) = True
    ''' Example: ByteIsPrintable(7) = False
    ''' </remarks>
    Public Function ByteIsPrintable(ByRef aByte As Byte) As Boolean
        Select Case aByte
            Case 9, 10, 12, 13 : Return True
            Case Is < 32 : Return False
            Case Is < 127 : Return True
            Case Else : Return False
        End Select
    End Function

    ''' <summary>
    ''' Sets values less than 1.0E-19 to 0.0 for the plotting routines for bug in DISSPLA/PR1ME. 
    ''' Otherwise returns values rounded to lower magnitude.
    ''' </summary>
    ''' <param name="aX">Single-precision value</param>
    ''' <returns>Incoming value, rounded to 0.0 if less than 1.0E-19.</returns>
    ''' <remarks>Example: Rndlow(1.0E-20) = 0, Rndlow(11000) = 10000</remarks>
    Public Function Rndlow(ByRef aX As Double) As Double
        Dim lSign As Double = 1.0#
        If aX < 0.0# Then
            lSign = -1.0#
        End If

        Dim lX As Double = System.Math.Abs(aX)
        Dim lRndlow As Double
        If lX < 1.0E-19 Then
            lRndlow = 0.0#
        Else
            Dim lA As Integer 'integer holds absolute value of log10 rounded down to nearest magnitude
            lA = Int(Math.Log10(CDbl(lX)))
            lRndlow = lSign * 10.0# ^ lA
        End If
        Return lRndlow
    End Function

    ''' <summary>
    ''' Like String.Substring but gracefully return an empty or shorter string when it would have thrown an exception
    ''' </summary>
    ''' <param name="aSourceString">String to get substring of</param>
    ''' <param name="aStartIndex">Zero-based start position of substring</param>
    ''' <param name="aLength">Length of substring</param>
    ''' <remarks>Why could they not have implemented String.Substring more like this?</remarks>
    Public Function SafeSubstring(ByVal aSourceString As String, ByVal aStartIndex As Integer, ByVal aLength As Integer) As String
        If aSourceString Is Nothing OrElse aSourceString.Length <= aStartIndex Then
            Return ""
        ElseIf aSourceString.Length > aStartIndex + aLength Then
            Return aSourceString.Substring(aStartIndex, aLength)
        Else
            Return aSourceString.Substring(aStartIndex)
        End If
    End Function

    ''' <summary>
    ''' Like String.Substring but gracefully return an empty or shorter string when it would have thrown an exception
    ''' </summary>
    ''' <param name="aSourceString">String to get substring of</param>
    ''' <param name="aStartIndex">Zero-based start position of substring</param>
    ''' <remarks>Why could they not have implemented String.Substring more like this?</remarks>
    Public Function SafeSubstring(ByVal aSourceString As String, ByVal aStartIndex As Integer) As String
        If aSourceString Is Nothing OrElse aSourceString.Length <= aStartIndex Then
            Return ""
        Else
            Return aSourceString.Substring(aStartIndex)
        End If
    End Function

    Public Function FirstStringPos(ByVal start As Integer, ByVal Source As String, ByVal ParamArray SearchFor() As Object) As Integer
        ' ##SUMMARY Searches Source for each item in SearchFor array.
        ' ##PARAM start I Position in Source to start search
        ' ##PARAM Source I String to be searched
        ' ##PARAM SearchFor I Array of strings to be individually searched for
        ' ##RETURNS  Position of first occurrence of SearchFor item in Source. _
        'Returns 0 if none were found.
        Dim vSearchFor As Object
        Dim foundPos As Integer
        Dim findPos As Integer
        ' ##LOCAL vSearchFor - member of ParamArray; substring to be searched for in Source
        ' ##LOCAL foundPos - position of substring in Source
        ' ##LOCAL findPos - position of first occurence of any member of ParamArray in Source

        For Each vSearchFor In SearchFor
            findPos = InStr(start, Source, vSearchFor)
            If findPos > 0 Then
                If foundPos = 0 Or foundPos > findPos Then foundPos = findPos
            End If
        Next vSearchFor
        FirstStringPos = foundPos
    End Function

    Public Function StrNoNull(ByVal S As String) As String
        ' ##SUMMARY Replaces null string with blank character.
        ' ##SUMMARY   Example: StrNoNull("NotNull") = "NotNull"
        ' ##SUMMARY   Example: StrNoNull("") = " "
        ' ##PARAM s I String to be analyzed
        ' ##RETURNS  Returns a blank character if string is null. _
        'Returns incoming string otherwise.
        If Len(S) = 0 Then
            StrNoNull = " "
        Else
            StrNoNull = S
        End If
    End Function

    ''' <summary>
    ''' Find a block of text between two known strings
    ''' </summary>
    ''' <param name="aSource">Text to search through</param>
    ''' <param name="aStartsWith">String that indicates block to find is about to start</param>
    ''' <param name="aEndsWith">String that indicates block to find has ended</param>
    ''' <param name="aStartIndex">Optional offset within aSource to start searching</param>
    ''' <returns>Block of text that was found between aStartsWith and aEndsWith</returns>
    ''' <remarks>Returned string does not include aStartsWith and aEndsWith. 
    ''' Empty string is returned if aStartsWith or aEndsWith is not found.</remarks>
    Public Function StrFindBlock(ByVal aSource As String, ByVal aStartsWith As String, ByVal aEndsWith As String, Optional ByVal aStartIndex As Integer = 0) As String
        Dim lStartPosition As Integer = aSource.IndexOf(aStartsWith, aStartIndex)
        If lStartPosition < 0 Then
            Return ""
        Else
            lStartPosition += aStartsWith.Length
            Dim lEndPosition As Integer = aSource.IndexOf(aEndsWith, lStartPosition)
            If lEndPosition < 0 Then
                Return ""
            Else
                Return aSource.Substring(lStartPosition, lEndPosition - lStartPosition)
            End If
        End If
    End Function

    ''' <summary>
    ''' Replace a block of text between two known strings
    ''' </summary>
    ''' <param name="aSource">Text to search through</param>
    ''' <param name="aStartsWith">String that indicates beginning of block to find</param>
    ''' <param name="aEndsWith">String that indicates end of block to find</param>
    ''' <param name="aReplaceWith">String to replace block that is found</param>
    ''' <param name="aStartIndex">Optional offset within aSource to start searching</param>
    ''' <returns>aSource where block of text that was found between aStartsWith and aEndsWith is replaced by aReplaceWith</returns>
    ''' <remarks>Returned string includes aStartsWith and aEndsWith. 
    ''' aSource is returned unchanged if aStartsWith or aEndsWith is not found.</remarks>
    Public Function StrReplaceBlock(ByVal aSource As String, _
                                    ByVal aStartsWith As String, _
                                    ByVal aEndsWith As String, _
                                    ByVal aReplaceWith As String, _
                           Optional ByVal aStartIndex As Integer = 0) As String
        Dim lStartPosition As Integer = aSource.IndexOf(aStartsWith, aStartIndex)
        If lStartPosition < 0 Then
            Return aSource
        Else
            lStartPosition += aStartsWith.Length
            Dim lEndPosition As Integer = aSource.IndexOf(aEndsWith, lStartPosition)
            If lEndPosition < 0 Then
                Return aSource
            Else
                Return aSource.Substring(0, lStartPosition) & aReplaceWith & aSource.Substring(lEndPosition)
            End If
        End If
    End Function

    Function StrFirstInt(ByRef Source As String) As Integer
        ' ##SUMMARY Divides alpha numeric sequence into leading numbers and trailing characters.
        ' ##SUMMARY   Example: StrFirstInt("123Go!) = "123", and changes Source to "Go!"
        ' ##PARAM Source M String to be analyzed
        ' ##RETURNS  Returns leading numbers in Source, and returns Source without those numbers.
        Dim retval As Integer = 0
        Dim pos As Integer = 1
        ' ##LOCAL retval - number found at beginning of Source
        ' ##LOCAL pos - long character position in search through Source

        If Source.StartsWith("-") AndAlso IsNumeric(Left(Source, 2)) Then pos = 3 'account for negative number - sign
        While IsNumeric(Mid(Source, pos, 1))
            pos += 1
        End While

        If pos >= 2 Then
            retval = CInt(Left(Source, pos - 1))
            Source = LTrim(Mid(Source, pos))
        End If

        Return retval
    End Function

    Public Function CountString(ByRef Source As String, ByRef Find As String) As Integer
        ' ##SUMMARY Searches for occurences of Find in Source.
        ' ##SUMMARY   Example: CountString("The lead man was lead-footed", "lead") = 2
        ' ##PARAM Source I Full string to be searched
        ' ##PARAM Find I Substring to be searched for
        ' ##RETURNS  Returns number of occurences of Find in Source.
        Dim retval As Integer
        Dim findPos As Integer
        Dim findlen As Integer
        ' ##LOCAL retval - string to be returned as CountString
        ' ##LOCAL findpos - long position of Find in Source
        ' ##LOCAL findlen - long length of Find

        findlen = Len(Find)
        If findlen > 0 Then
            findPos = InStr(Source, Find)
            While findPos > 0
                retval = retval + 1
                findPos = InStr(findPos + findlen, Source, Find)
            End While
        End If
        CountString = retval
    End Function

    Public Function ReadableFromXML(ByVal aXML As String) As String
        Dim lHTML As Boolean = aXML.ToUpper.IndexOf("<HTML") > -1
        Dim lXML As String = aXML.Replace("<BR>", vbLf).Replace("&nbsp;", " ")
        Dim lSB As New System.Text.StringBuilder
        Dim lLastWasLF As Boolean = False
        Dim lIndex As Integer = 0
        While lIndex < lXML.Length
            Dim lChar As Char = lXML.Chars(lIndex)
            Select Case lChar
                Case "<"
                    Dim lClose As Integer = lXML.IndexOf(">", lIndex + 1)
                    If lXML.Chars(lIndex + 1) = "/" Then
                        If Not lLastWasLF Then lSB.Append(vbLf)
                        lLastWasLF = True
                    ElseIf lHTML Then
                    Else
                        lSB.Append(lXML.Substring(lIndex + 1, lClose - lIndex - 1) & ": ")
                        lLastWasLF = False
                    End If
                    lIndex = lClose
                Case Else : lSB.Append(lChar)
            End Select
            lIndex += 1
        End While
        Return lSB.ToString
    End Function

    Public Function ReplaceStringNoCase(ByRef Source As String, ByRef Find As String, ByRef ReplaceWith As String) As String
        ' ##SUMMARY Replaces Find in Source with Replace (not case sensitive).
        ' ##SUMMARY Example: ReplaceStringNoCase("He came and he went", "He", "She") = "She came and She went"
        ' ##PARAM Source I Full string to be searched
        ' ##PARAM Find I Substring to be searched for and replaced
        ' ##PARAM Replace I Substring to replace Find
        ' ##RETURNS Returns new string like Source except that _
        'any occurences of Find (not case sensitive) are replaced with Replace.
        Dim retval As String = ""
        Dim findPos As Integer
        Dim lastFindEnd As Integer
        Dim findlen As Integer
        Dim replacelen As Integer
        Dim lSource As String
        Dim lFind As String
        ' ##LOCAL retval - string to be returned as ReplaceString
        ' ##LOCAL findpos - long position of Find in Source
        ' ##LOCAL lastFindEnd - long position of first character after last replaced string in Source
        ' ##LOCAL findlen - long length of Find
        ' ##LOCAL replacelen - long length of Replace
        ' ##LOCAL lSource - local version of input parameter Source
        ' ##LOCAL lFind - local version of input parameter Find

        findlen = Len(Find)
        If findlen > 0 Then
            replacelen = Len(ReplaceWith)
            lSource = LCase(Source)
            lFind = LCase(Find)
            findPos = InStr(lSource, lFind)
            lastFindEnd = 1
            While findPos > 0
                retval &= Mid(Source, lastFindEnd, findPos - lastFindEnd) & ReplaceWith
                lastFindEnd = findPos + findlen
                findPos = InStr(findPos + findlen, lSource, lFind)
            End While
            ReplaceStringNoCase = retval & Mid(Source, lastFindEnd)
        Else
            ReplaceStringNoCase = Source
        End If
    End Function

    Public Function ReplaceString(ByRef Source As String, ByRef Find As String, ByRef ReplaceWith As String) As String
        ' ##SUMMARY Replaces Find in Source with Replace (case sensitive).
        ' ##SUMMARY   Example: ReplaceString("He left", "He", "She") = "She left"
        ' ##PARAM Source I Full string to be searched
        ' ##PARAM Find I Substring to be searched for and replaced
        ' ##PARAM Replace I Substring to replace Find
        ' ##RETURNS Returns new string like Source except that _
        'any occurences of Find (case sensitive) are replaced with Replace.

        Return Source.Replace(Find, ReplaceWith)

        'Dim retval As String = ""
        'Dim findPos As Integer
        'Dim lastFindEnd As Integer
        'Dim findlen As Integer
        'Dim replacelen As Integer
        '' ##LOCAL retval - string to be returned as ReplaceString
        '' ##LOCAL findpos - long position of Find in Source
        '' ##LOCAL lastFindEnd - long position of first character after last replaced string in Source
        '' ##LOCAL findlen - long length of Find
        '' ##LOCAL replacelen - long length of Replace

        'findlen = Len(Find)
        'If findlen > 0 Then
        '    replacelen = Len(ReplaceWith)
        '    findPos = InStr(Source, Find)
        '    lastFindEnd = 1
        '    While findPos > 0
        '        retval &= Mid(Source, lastFindEnd, findPos - lastFindEnd) & ReplaceWith
        '        lastFindEnd = findPos + findlen
        '        findPos = InStr(findPos + findlen, Source, Find)
        '    End While
        '    ReplaceString = retval & Mid(Source, lastFindEnd)
        'Else
        '    ReplaceString = Source
        'End If
    End Function

    'Public Sub StrTrim(ByRef istr As String)
    '    ' ##SUMMARY Removes all blanks from a string.
    '    ' ##SUMMARY   Example: StrTrim "No Blanks" changes istr to "NoBlanks"
    '    ' ##PARAM istr I String to be searched
    '    Dim lstr As String
    '    Dim bpos As Integer
    '    ' ##LOCAL lstr - local string
    '    ' ##LOCAL bpos - long position of blank

    '    lstr = ""
    '    bpos = InStr(istr, " ")
    '    While bpos > 0
    '        lstr = lstr & Mid(istr, 1, bpos)
    '        istr = LTrim(Mid(istr, bpos))
    '        bpos = InStr(istr, " ")
    '    End While
    '    istr = lstr & istr

    'End Sub

    Public Function StrPrintable(ByRef S As String, Optional ByRef ReplaceWith As String = "") As String
        ' ##SUMMARY Converts, if necessary, non-printable characters in string to printable _
        'alternative.
        ' ##PARAM S I String to be converted, if necessary.
        ' ##PARAM ReplaceWith I Character to replace non-printable characters in S (default="").
        ' ##RETURNS Input parameter S with non-printable characters replaced with specific _
        'printable character.
        Dim retval As String = "" 'return string
        Dim i As Short            'loop counter
        Dim strLen As Short       'length of string
        Dim ch As String          'individual character in string

        strLen = Len(S)
        For i = 1 To strLen
            ch = Mid(S, i, 1)
            Select Case Asc(ch)
                Case 0 : Exit For
                Case 32 To 126 : retval = retval & ch
                Case Else : retval = retval & ReplaceWith
            End Select
        Next
        Return retval
    End Function

    Public Function Center(ByVal aStr As String, ByVal aWidth As String) As String
        Dim lPadLength As Integer = aWidth - aStr.Length
        If lPadLength <= 0 Then
            Return aStr
        Else
            Dim lPadLeftLength As Integer = aStr.Length + (lPadLength / 2)
            Dim lStr As String = (aStr.PadLeft(lPadLeftLength)).PadRight(aWidth)
            Return lStr
        End If
    End Function

    Public Function StrPad(ByVal S As String, ByVal NewLength As Integer, Optional ByVal PadWith As String = " ", Optional ByVal PadLeft As Boolean = True) As String
        ' ##SUMMARY Pads a string with specific character to achieve a specified length.
        ' ##PARAM S M String to be padded.
        ' ##PARAM NewLength I Length of padded string to be returned.
        ' ##PARAM PadWith I Character with which to pad the string.
        ' ##PARAM PadLeft I Pad left if true, pad right if false.
        ' ##RETURNS Input parameter S padded to left or right (default=left) with specific character (default=space) to specified length.

        Dim NumCharsToAdd As Integer = NewLength - S.Length
        If NumCharsToAdd <= 0 Then
            Return S
        ElseIf PadLeft Then
            Return New String(PadWith, NumCharsToAdd) & S
        Else
            Return S & New String(PadWith, NumCharsToAdd)
        End If
    End Function

    Public Function Long2String(ByRef Value As Integer) As String
        ' ##SUMMARY Returns ASCII text version of four bytes in an integer
        ' ##SUMMARY Example: Long2String(98) = "b   "
        ' ##PARAM Value I Value to be converted
        ' ##RETURNS Input parameter Val in string form.
        Dim bVal As Byte()
        bVal = System.BitConverter.GetBytes(Value)
        Long2String = ""
        For lByte As Integer = 0 To 3
            If bVal(lByte) > 0 Then Long2String &= Chr(bVal(lByte))
        Next
    End Function

    Public Function Long2Single(ByRef Value As Integer) As Single
        ' ##SUMMARY Converts bytes of integer into SingleType.
        ' ##SUMMARY Example: Long2Single(999999999) =  4.723787E-03
        ' ##PARAM Value I Value to be converted
        ' ##RETURNS Input parameter Val in single precision form.
        Return System.BitConverter.ToSingle(System.BitConverter.GetBytes(Value), 0)
    End Function

    'Public Function Byte2Integer(ByRef Byt() As Byte, ByRef Ind As Integer) As Short
    '    Return System.BitConverter.ToInt16(Byt, Ind)
    'End Function

    'Public Function Byte2Long(ByRef Byt() As Byte, ByRef Ind As Integer) As Integer
    '    Return System.BitConverter.ToInt32(Byt, Ind)
    'End Function

    'Public Function Byte2Single(ByRef Byt() As Byte, ByRef Ind As Integer) As Single
    '    Return System.BitConverter.ToSingle(Byt, Ind)
    'End Function

    Public Function Byte2String(ByRef Byt() As Byte, ByRef StartAt As Integer, ByRef Length As Integer) As String
        ' ##SUMMARY Converts sequence of members in Byte array to string of _
        'corresponding ascii characters.
        ' ##SUMMARY   Example: Byte2String(Byt, 1, 3) = "See" _
        'where Byt(1) = 83, Byt(2) = 101, Byt(3) = 101
        ' ##PARAM Byt() I Array containing byte values to be converted
        ' ##PARAM StartAt I Index of first element in Byt() to be analyzed
        ' ##PARAM Length I Number of members in Byt array to be sequentially analyzed
        ' ##RETURNS String of Length populated from Byt
        Dim S As String
        Dim iByt As Integer
        Dim c As Integer
        ' ##LOCAL s - string antecedent to Byte2String
        ' ##LOCAL i - long counter as index to Byt array
        ' ##LOCAL c - string set to each incremental character from Byt array

        S = ""
        For iByt = 0 To Length - 1
            c = Byt(StartAt + iByt)
            If c = 0 Then c = 32 'space
            S = S & Chr(c)
        Next
        Return S
    End Function

    Function StrRetRem(ByRef S As String) As String
        ' ##SUMMARY Divides string into 2 portions at position of 1st occurence of comma or space.
        ' ##SUMMARY   Example: StrRetRem("This string") = "This", and s is reduced to "string"
        ' ##SUMMARY   Example: StrRetRem("This,string") = "This", and s is reduced to "string"
        ' ##PARAM s M String to be analyzed
        ' ##RETURNS  Returns leading portion of incoming string up to first occurence of delimeter. 
        '            Returns input parameter without that portion. If no comma or space in string, 
        '            returns whole string, and input parameter reduced to null string.
        Dim l As String
        Dim i As Integer
        Dim j As Integer
        ' ##LOCAL l - string to return
        ' ##LOCAL i - position of blank delimeter
        ' ##LOCAL j - position of comma delimeter

        S = LTrim(S) 'remove leading blanks

        i = InStr(S, "'")
        If i = 1 Then 'string beginning
            S = Mid(S, 2)
            i = InStr(S, "'") 'string end
        Else
            i = InStr(S, " ") 'blank delimeter
            j = InStr(S, ",") 'comma delimeter
            If j > 0 Then 'comma found
                If i = 0 Or j < i Then
                    i = j
                End If
            End If
        End If

        If i > 0 Then 'found delimeter
            l = Left(S, i - 1) 'string to return
            S = LTrim(Mid(S, i + 1)) 'string remaining
            If InStr(Left(S, 1), ",") = 1 And i <> j Then S = Mid(S, 2)
        Else 'take it all
            l = S
            S = "" 'nothing left
        End If

        StrRetRem = l

    End Function

    Public Function StringQuotedAsNeeded(ByVal aInputString As String, Optional ByVal aDelimiter As String = ",", Optional ByVal aQuote As String = """") As String
        If aInputString.Contains(aDelimiter) Then
            Return aQuote & aInputString & aQuote
        Else
            Return aInputString
        End If
    End Function
End Module
