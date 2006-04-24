Option Strict Off
Option Explicit On 

Public Module modString
    '##MODULE_REMARKS Copyright 2001-5 AQUA TERRA Consultants - Royalty-free use permitted under open source license

    ' ##MODULE_NAME UTILITY
    ' ##MODULE_DATE March 3, 2003
    ' ##MODULE_AUTHOR Mark Gray and Jack Kittle of AQUA TERRA CONSULTANTS
    ' ##MODULE_DESCRIPTION General utility subroutines and functions shared by many projects (don't _
    'change)


    Public Function Log10(ByVal x As Double) As Double
        ' ##SUMMARY Calculates the log 10 of a given number.
        ' ##SUMMARY   Example: Log10(218.7761624) = 2.34
        ' ##PARAM X I Double-precision number
        ' ##RETURNS Log 10 of given number.
        'Do not try to calculate if (X <= 0)
        If x > 0 Then Log10 = System.Math.Log(x, 10) Else Log10 = 1
    End Function

    '    Function NumFmtRE(ByVal rtmp As Single, Optional ByRef maxWidth As Integer = 16) As String
    '        ' ##SUMMARY Converts single-precision number to string with exponential syntax if length of number exceeds specified length.
    '        ' ##SUMMARY If unspecified, length defaults to 16.
    '        ' ##SUMMARY   Example: NumFmtRE(123000000, 7) = "1.23e-8"
    '        ' ##PARAM rtmp I Single-precision number to be formatted
    '        ' ##PARAM maxWidth I Length of string to be returned including decimal point and exponential syntax
    '        ' ##RETURNS Input parameter rtmp formatted, if necessary, to scientific notation.
    '        ' ##LOCAL LogVal - double-precision log10 value of rtmp
    '        ' ##LOCAL retval - string used as antecedent to NumFmtRE
    '        ' ##LOCAL expFormat - string syntax of exponential format
    '        ' ##LOCAL DecimalPlaces - long number of decimal places
    '        Dim LogVal As Double
    '        Dim retval As String
    '        Dim expFormat As String
    '        Dim DecimalPlaces As Integer

    '        retval = CStr(rtmp)
    '        NumFmtRE = retval

    '        If rtmp <> 0 And maxWidth > 0 Then
    '            If Len(retval) > maxWidth Then
    '                'Determine appropriate log syntax
    '                LogVal = System.Math.Abs(Log10(System.Math.Abs(rtmp)))
    '                If LogVal >= 100 Then
    '                    expFormat = "e-###"
    '                ElseIf LogVal >= 10 Then
    '                    expFormat = "e-##"
    '                Else
    '                    expFormat = "e-#"
    '                End If
    '                'Set appropriate decimal position
    '                DecimalPlaces = maxWidth - Len(expFormat) - 2
    '                If DecimalPlaces < 1 Then DecimalPlaces = 1
    '                retval = String.Format("#." & New String("#", DecimalPlaces) & expFormat, rtmp)
    '            End If
    '        End If
    '        NumFmtRE = retval
    '    End Function

    '    Function NumFmted(ByVal rtmp As Single, ByVal wid As Integer, ByVal dpla As Integer) As String
    '        ' ##SUMMARY Converts single precision number to string with specified format
    '        ' ##SUMMARY   Example: NumFmted(1.23, 6, 3) = " 1.230"
    '        ' ##PARAM rtmp I Single-precision number to be formatted
    '        ' ##PARAM wid I Long width of formatted number
    '        ' ##PARAM dpla I Long number of decimal places in formatted number
    '        ' ##RETURNS Input parameter rtmp formatted to specified width _
    '        'with specified number of decimal places (left buffered).
    '        ' ##LOCAL fmt - string representation of generic format (i.e., "#0.000")
    '        ' ##LOCAL nspc - long number of leading/trailing blanks
    '        ' ##LOCAL stmp - string used to build formatted number
    '        Dim fmt As String
    '        Dim nspc As Integer
    '        Dim stmp As String

    '        On Error GoTo prob

    '        If wid - dpla - 2 > 0 Then
    '            fmt = New String("#", wid - dpla - 2) & "0." & New String("0", dpla) 'force 0.
    '        Else
    '            fmt = New String("#", wid - dpla - 1) & "." & New String("0", dpla) 'orig way
    '        End If
    '        stmp = String.Format(fmt, rtmp)
    '        'add leading blanks
    '        nspc = wid - dpla - InStr(1, stmp, ".")
    '        If nspc < 0 Then nspc = 0
    '        stmp = Space(nspc) & stmp
    '        'add trailing blanks
    '        nspc = wid - Len(stmp)
    '        If nspc < 0 Then nspc = 0
    '        NumFmted = stmp & Space(nspc)
    '        Exit Function
    'prob:
    '        System.Diagnostics.Debug.WriteLine("NumFmted Problem rtmp:" & rtmp & " wid:" & wid & " dpla:" & dpla)
    '        NumFmted = New String("#", wid)

    '    End Function

  Function RightJustify(ByVal aValue As Object, ByVal aWidth As Integer) As String
    Dim lStr As String = CStr(aValue)
    If lStr.Length >= aWidth Then
      Return lStr
    Else
      Return Space(aWidth - lStr.Length) & lStr
    End If
  End Function

  '    Function NumFmtI(ByRef itmp As Integer, ByRef wid As Integer) As String
  '        ' ##SUMMARY Converts an integer to string of specified length, padded with leading zeros.
  '        ' ##SUMMARY Specified length must equal or exceed length of integer.
  '        ' ##SUMMARY   Example: NumFmtI(1234, 6) = "001234"
  '        ' ##PARAM itmp I Long integer to be formatted
  '        ' ##PARAM wid I Width of formatted integer
  '        ' ##RETURNS Input parameter itmp formatted to specified width.
  '        ' ##LOCAL fmt - string representation of generic format (i.e., "#####0")
  '        ' ##LOCAL stmp - string used to build formatted number
  '        Dim fmt As String
  '        Dim stmp As String

  '        If wid > 1 Then
  '            fmt = New String("#", wid - 1)
  '        End If
  '        'assure at least one digit is output
  '        fmt = fmt & "0"
  '        stmp = String.Format(fmt, itmp)
  '        'add leading blanks
  '        NumFmtI = Space(wid - Len(stmp)) & stmp
  '    End Function

  '    Function Signif(ByRef fvalue As Double, ByRef Metric As Boolean) As Double
  '        '##SUMMARY Converts double-precision number to three significant digits
  '        '##SUMMARY   Example: Signif(1.23456, True) =  1.23000001907349
  '        '##PARAM fvalue - double-precision number to be formatted
  '        '##PARAM metric - Boolean whether metric or not
  '        '##LOCAL tmp - Double-precision number used as antecedent to Signif
  '        Dim tmp As Double

  '        If Metric And fvalue < 10 Then
  '            fvalue = Fix(fvalue * 100 + 0.5)
  '            Signif = fvalue / 100
  '        ElseIf Metric And fvalue < 100 Then
  '            fvalue = Fix(fvalue * 10 + 0.5)
  '            Signif = fvalue / 10
  '        ElseIf fvalue < 100.0# Then
  '            Signif = Fix(fvalue + 0.5)
  '        Else
  '            tmp = 10.0# ^ Fix(Log10(fvalue)) / 100.0#
  '            Signif = Fix(fvalue / tmp + 0.5) * tmp
  '        End If

  '    End Function

  Function SignificantDigits(ByVal Value As Double, ByVal digits As Integer) As Double
    ' ##SUMMARY Rounds double-precision number to specified number of significant digits.
    ' ##SUMMARY   Example: Signif(1.23456, 3) =  1.23
    ' ##PARAM val I Double-precision number to be formatted
    ' ##PARAM digits I Number of significant digits
    ' ##RETURNS Input parameter val rounded to specified number of significant digits.
    Dim CurPower As Integer 'order of magnitude of val
    Dim Negative As Boolean 'true if incoming number is negative
    Dim ShiftPower As Double 'magnitude by which val is temporarily shifted

    If Value < 0 Then 'Have to use a positive number with Log10 below
      Negative = True
      Value = -Value
    End If

    CurPower = Fix(Log10(Value))
    If Value >= 1 Then CurPower += 1
    ShiftPower = 10 ^ (digits - CurPower)
    Value = Value * ShiftPower 'Shift val so number of digits before decimal = significant digits
    Value = Fix(Value + 0.5) 'Round up if needed
    Value = Value / ShiftPower 'Shift val back to where it belongs

    If Negative Then Value = -Value

    SignificantDigits = Value
  End Function

  Function DoubleToString(ByVal aValue As Double, _
                 Optional ByVal aMaxWidth As Integer = 10, _
                 Optional ByVal aFormat As String = "#,##0.########", _
                 Optional ByVal aExpFormat As String = "#.#e#", _
                 Optional ByVal aCantFit As String = "#", _
                 Optional ByVal aSignificantDigits As Integer = 5) As String
    Dim lValue As Double
    If aSignificantDigits > 0 Then
      lValue = SignificantDigits(aValue, aSignificantDigits)
    Else
      lValue = aValue
    End If

    Dim lString As String = Format(lValue, aFormat)
    If lString.Length <= aMaxWidth Then
      Return lString
    Else
      Dim lDecimalPos As Integer = lString.IndexOf(".")
      Select Case lDecimalPos
        Case Is < 1 'string does not contain a decimal or it is the first character
        Case aMaxWidth, aMaxWidth - 1
          Return Left(lString, lDecimalPos) 'Truncate at decimal and remove trailing decimal
        Case Is < aMaxWidth
          Return Left(lString, aMaxWidth) 'Truncate at least one digit after decimal
      End Select
      'At this point we know string is too long and cannot simply be truncated at or after decimal point
      lString = Format(lValue, aExpFormat)
      If lString.Length <= aMaxWidth Then
        Return lString
      Else
        Return aCantFit 'Placeholder for number that does not fit even in exponential notation
      End If
    End If
  End Function

  '    Function Str_Read() As String
  '        ' ##SUMMARY Reads a string from a binary database of open file.
  '        ' ##SUMMARY Assumes open file # is '1'.
  '        ' ##RETURNS String from a binary file.
  '        ' ##LOCAL tmp - string used to read binary input one character at a time
  '        ' ##LOCAL stmp - string used as antecedent to Str_Read
  '        Dim tmp As String
  '        Dim stmp As String

  '        stmp = ""
  '        Do
  '            tmp = InputString(1, 1)
  '            If Asc(tmp) = 0 Then
  '                Exit Do
  '            Else
  '                stmp = stmp & tmp
  '            End If
  '        Loop
  '        Str_Read = stmp

  '    End Function

  Public Function IsInteger(ByRef istr As String) As Boolean
    ' ##SUMMARY Checks to see whether incoming string is an integer or not.
    ' ##SUMMARY Returns true if each character in string is in range [0-9].
    ' ##SUMMARY   Example: IsInteger(12345) = True
    ' ##SUMMARY   Example: IsInteger(123.45) = False
    ' ##PARAM istr I String to be checked for integer status
    ' ##RETURNS True if input parameter istr is an integer.
    ' ##LOCAL a - long number set to ascii code of each successive character in istr
    ' ##LOCAL Length - long length of istr
    ' ##LOCAL pos - long position of character in istr being checked
    Dim a As Integer
    Dim Length As Integer
    Dim pos As Integer

    IsInteger = False
    Length = Len(istr)
    For pos = 1 To Length
      a = Asc(Mid(istr, pos, 1))
      If a < 48 Then Exit Function
      If a > 57 Then Exit Function
    Next pos
    IsInteger = True
  End Function

  Public Function IsAlpha(ByRef istr As String) As Boolean
    ' ##SUMMARY Checks to see whether incoming string is entirely alphabetic.
    ' ##SUMMARY   Example: IsAlpha(abcde) = True
    ' ##SUMMARY   Example: IsAlpha(abc123) = False
    ' ##PARAM istr I String to be checked for alphabetic status  ' ##LOCAL a - long number set to ascii code of each successive character in istr
    ' ##RETURNS True if input parameter istr contains only [A-Z] or [a-z].
    Dim a As Integer
    Dim Length As Integer
    Dim pos As Integer
    ' ##LOCAL a - position of character in istr
    ' ##LOCAL Length - length of istr
    ' ##LOCAL pos - position of character in istr being checked

    IsAlpha = False
    Length = Len(istr)
    For pos = 1 To Length
      a = Asc(Mid(istr, pos, 1))
      If a < 65 Then Exit Function
      If a > 90 And a < 97 Then Exit Function
      If a > 122 Then Exit Function
    Next pos
    IsAlpha = True
  End Function

  Public Function IsAlphaNumeric(ByRef istr As String) As Boolean
    ' ##SUMMARY Checks to see whether incoming string is entirely alphanumeric.
    ' ##SUMMARY   Example: IsAlphaNumeric(abc123) = True
    ' ##SUMMARY   Example: IsAlphaNumeric(#$*&!) = False
    ' ##PARAM istr I String to be checked for alphanumeric status
    ' ##RETURNS True if input parameter istr contains only [A-Z], [a-z], or [0-9].
    Dim a As Integer
    Dim Length As Integer
    Dim pos As Integer
    ' ##LOCAL a - long number set to ascii code of each successive character in istr
    ' ##LOCAL Length - length of istr
    ' ##LOCAL pos - position of character in istr being checked

    IsAlphaNumeric = False
    Length = Len(istr)
    For pos = 1 To Length
      a = Asc(Mid(istr, pos, 1))
      If a < 48 Then Exit Function
      If a > 57 And a < 65 Then Exit Function
      If a > 90 And a < 97 Then Exit Function
      If a > 122 Then Exit Function
    Next pos
    IsAlphaNumeric = True
  End Function

  Public Function ByteIsPrintable(ByRef b As Byte) As Boolean
    ' ##SUMMARY Checks to see whether incoming byte is printable.
    ' ##SUMMARY   Example: ByteIsPrintable(44) = True
    ' ##SUMMARY   Example: IsAlphaNumeric(7) = False
    ' ##PARAM b I Byte to be checked for printable status
    ' ##RETURNS True if input parameter b is ASCII code 9, 10, 12, 13, 32 - 126.
    Select Case b
      Case 9, 10, 12, 13 : ByteIsPrintable = True
      Case Is < 32 : ByteIsPrintable = False
      Case Is < 127 : ByteIsPrintable = True
    End Select
  End Function

  'Public Sub NumChr(ByRef ilen As Integer, ByRef inam() As Integer, ByRef outstr As String)
  '    ' ##SUMMARY Returns String of characters associated with specified ascii _
  '    'character codes in inam().
  '    ' ##PARAM ilen I Upper bound of inam() dimension
  '    ' ##PARAM inam I Array of long ascii character codes
  '    ' ##PARAM outstr O String of converted ascii codes
  '    Dim i As Integer
  '    ' ##LOCAL i - long number used as index for member of inam()

  '    outstr = ""
  '    For i = 0 To ilen
  '        outstr = outstr & Chr(inam(i))
  '    Next i

  'End Sub

  'Function StrAdd(ByRef o As Object, ByRef old As String, ByRef Add As String, ByRef mwid As String) As String
  '    ' ##SUMMARY Concatenates given string plus addendum, up to maximum length _
  '    'allowed by object (padded right).
  '    ' ##PARAM o I Object containing text
  '    ' ##PARAM old I Existing string
  '    ' ##PARAM Add I New string to addend to existing string
  '    ' ##PARAM mwid I MAXimum length of addendum
  '    ' ##RETURNS Concatenated input parameters, old and Add, plus right padding _
  '    'if necessary to extend string to specified width.
  '    Dim nst As String
  '    ' ##LOCAL nst - new string built with Add plus trailing blanks

  '    nst = Add
  '    Do While o.TextWidth(nst) < o.TextWidth(mwid)
  '        nst = nst & " "
  '    Loop
  '    StrAdd = old & nst
  'End Function

  Public Sub Scalit(ByVal itype As Short, ByVal mMin As Single, ByVal mMax As Single, _
                    ByRef plmn As Single, ByRef plmx As Single)
    ' ##SUMMARY Determines an appropriate scale based on the _
    'minimum and maximum values and whether an arithmetic, probability, _
    'or logarithmic scale is requested. Minimum and maximum for probability _
    'plots must be standard deviates. For log scales, the minimum _
    'and maximum must not be transformed.
    ' ##PARAM itype I Integer indicating type of number scale (0-1 = arithmetic, 2 = probability, other = logarithmic)
    ' ##PARAM mMin I Single-precision minimum incoming data value
    ' ##PARAM mMax I Single-precision maximum incoming data value
    ' ##PARAM plmn O Single-precision return value for scale minimum
    ' ##PARAM plmx O Single-precision return value for scale maximum
    Dim a As Short
    Dim i As Short
    Dim inc As Short
    Dim x As Single
    Dim m As Single
    Dim tmax As Single
    ' ##LOCAL a - short integer holds log10 min/max values rounded down to nearest magnitude
    ' ##LOCAL i - short integer used as index for r()
    ' ##LOCAL inc - short integer increments i by 1 or -1
    ' ##LOCAL X - single-precision rounded data min/max values
    ' ##LOCAL M - single-precision estimator of min/max values for arithmetic scale
    ' ##LOCAL tmax - single-precision min/max values for distribution plots (+ = max, - = min)

    Static r(15) As Single
    If itype = 0 Or itype = 1 Then
      'arithmetic scale
      'get next lowest mult of 10

      ' ##LOCAL r - holds possible values for multiplier used in determining M from X
      If r(1) < 0.09 Then
        r(1) = 0.1
        r(2) = 0.15
        r(3) = 0.2
        r(4) = 0.4
        r(5) = 0.5
        r(6) = 0.6
        r(7) = 0.8
        r(8) = 1.0#
        r(9) = 1.5
        r(10) = 2.0#
        r(11) = 4.0#
        r(12) = 5.0#
        r(13) = 6.0#
        r(14) = 8.0#
        r(15) = 10.0#
      End If

      x = Rndlow(mMax)
      If x > 0.0# Then
        inc = 1
        i = 1
      Else
        inc = -1
        i = 15
      End If
      Do
        m = r(i) * x
        i = i + inc
      Loop While mMax > m And i <= 15 And i >= 1
      plmx = m

      If mMin < 0.5 * mMax And mMin >= 0.0# And itype = 1 Then
        plmn = 0.0#
      Else
        'get next lowest mult of 10
        x = Rndlow(mMin)
        If x >= 0.0# Then
          inc = -1
          i = 15
        Else
          inc = 1
          i = 1
        End If
        Do
          m = r(i) * x
          i = i + inc
        Loop While mMin < m And i >= 1 And i <= 15
        plmn = m
      End If

    ElseIf itype = 2 Then
      'logarithmic scale
      If mMin > 0.000000001 Then
        a = Fix(Log10(CDbl(mMin)))
      Else
        'too small or neg value, set to -9
        a = -9
      End If
      If mMin < 1.0# Then a = a - 1
      plmn = 10.0# ^ a

      If mMax > 0.000000001 Then
        a = Fix(Log10(CDbl(mMax)))
      Else
        'too small or neg value, set to -8
        a = -8
      End If
      If mMax > 1.0# Then a = a + 1
      plmx = 10.0# ^ a

      If plmn * 10000000.0# < plmx Then
        'limit range to 7 cycles
        plmn = plmx / 10000000.0#
      End If

    Else
      'probability plots - assumes data transformed to normal deviates
      tmax = System.Math.Abs(mMax)
      If System.Math.Abs(mMin) > tmax Then tmax = System.Math.Abs(mMin)
      tmax = CSng(Fix(tmax * 10.0#) + 1) / 10.0#
      If tmax > 4.0# Then tmax = 4.0#
      plmn = -tmax
      plmx = tmax
    End If

  End Sub

  Public Function Rndlow(ByRef px As Single) As Single
    ' ##SUMMARY Sets values less than 1.0E-19 to 0.0 for the _
    'plotting routines for bug in DISSPLA/PR1ME. Otherwise returns values _
    'rounded to lower magnitude.
    ' ##SUMMARY   Example: Rndlow(1.0E-20) = 0
    ' ##SUMMARY   Example: Rndlow(11000) = 10000
    ' ##PARAM px I Single-precision datum
    ' ##RETURNS Incoming px value, rounded to 0.0 if less than 1.0E-19.
    Dim a As Integer
    Dim x As Single
    Dim sign As Single
    ' ##LOCAL a - short integer holds absolute value of log10 rounded down to nearest magnitude
    ' ##LOCAL X - single-precision set to absolute value of px
    ' ##LOCAL sign - single-precision holds positive or negative sign for px

    sign = 1.0#
    If px < 0.0# Then sign = -1.0#
    x = System.Math.Abs(px)
    If x < 1.0E-19 Then
      Rndlow = 0.0#
    Else
      a = Int(Log10(CDbl(x)))
      Rndlow = sign * 10.0# ^ a
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

  'Use String.IndexOfAny instead of this
  'Public Function FirstCharPos(ByVal start As Integer, ByVal Source As String, ByVal chars As String) As Integer
  '  ' ##SUMMARY Searches str for each character in chars.
  '  ' ##PARAM start I Position in str to start search
  '  ' ##PARAM str I String to be searched
  '  ' ##PARAM chars I String of characters to be individually searched for
  '  ' ##RETURNS  Position of first occurrence of chars character in Source. _
  '  'Returns len(str) + 1 if no characters from chars were found in Source.
  '  Dim retval As Integer
  '  Dim curval As Integer
  '  Dim CharPos As Integer
  '  Dim LenChars As Integer
  '  ' ##LOCAL retval - long return value for FirstCharPos
  '  ' ##LOCAL curval - long position of currently first-occurring character
  '  ' ##LOCAL CharPos - long length of chars
  '  ' ##LOCAL LenChars - long length of subString

  '  retval = Len(Source) + 1
  '  LenChars = Len(chars)
  '  For CharPos = 1 To LenChars
  '    curval = InStr(start, Source, Mid(chars, CharPos, 1))
  '    If curval > 0 And curval < retval Then retval = curval
  '  Next CharPos
  '  FirstCharPos = retval
  'End Function

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

  'Function StrRetRem(ByRef S As String) As String
  '    ' ##SUMMARY Divides string into 2 portions at position of 1st occurence of comma or space.
  '    ' ##SUMMARY   Example: StrRetRem("This string") = "This", and s is reduced to "string"
  '    ' ##SUMMARY   Example: StrRetRem("This,string") = "This", and s is reduced to "string"
  '    ' ##PARAM s M String to be analyzed
  '    ' ##RETURNS  Returns leading portion of incoming string up to first occurence of delimeter. _
  '    'Returns input parameter without that portion. If no comma or space in string, _
  '    'returns whole string, and input parameter reduced to null string.
  '    Dim l As String
  '    Dim i As Integer
  '    Dim j As Integer
  '    ' ##LOCAL l - string to return
  '    ' ##LOCAL i - position of blank delimeter
  '    ' ##LOCAL j - position of comma delimeter

  '    S = LTrim(S) 'remove leading blanks

  '    i = InStr(S, "'")
  '    If i = 1 Then 'string beginning
  '        S = Mid(S, 2)
  '        i = InStr(S, "'") 'string end
  '    Else
  '        i = InStr(S, " ") 'blank delimeter
  '        j = InStr(S, ",") 'comma delimeter
  '        If j > 0 Then 'comma found
  '            If i = 0 Or j < i Then
  '                i = j
  '            End If
  '        End If
  '    End If

  '    If i > 0 Then 'found delimeter
  '        l = Left(S, i - 1) 'string to return
  '        S = LTrim(Mid(S, i + 1)) 'string remaining
  '        If InStr(Left(S, 1), ",") = 1 And i <> j Then S = Mid(S, 2)
  '    Else 'take it all
  '        l = S
  '        S = "" 'nothing left
  '    End If

  '    StrRetRem = l

  'End Function

  'Function StrTokens(ByVal Source As String, ByRef delim As String, ByRef quote As String) As String()
  '    ' ##SUMMARY Divides string into portions separated by specified delimeter.
  '    ' ##SUMMARY   Example: StrTokens("Very,Special,string") = Array size 2;
  '    ' ##SUMMARY                                               (0)="Very", (1)="Special", (2)="string" and
  '    ' ##SUMMARY                                               String is reduced to ""
  '    ' ##PARAM Source M String to be analyzed
  '    ' ##PARAM delim I delimeter to look for in string Source
  '    ' ##PARAM quote I Multi-character string exempted from search.
  '    ' ##RETURNS  Returns array of string portions separated by specified delimeter.
  '    Dim retval() As String
  '    Dim sizeRetval As Integer
  '    Dim nTokens As Integer
  '    ' ##LOCAL retval - string array to return
  '    ' ##LOCAL sizeRetval - dimension variable for sizing string array
  '    ' ##LOCAL nTokens - number of tokens found in string Source

  '    sizeRetval = 20
  '    ReDim retval(sizeRetval)
  '    While Len(Source) > 0
  '        If nTokens > sizeRetval Then
  '            sizeRetval = sizeRetval * 2
  '            ReDim Preserve retval(sizeRetval)
  '        End If
  '        retval(nTokens) = StrSplit(Source, delim, quote)
  '        nTokens = nTokens + 1
  '    End While
  '    ReDim Preserve retval(nTokens - 1)
  '    Return retval
  'End Function

  'Sub DumpStrings(ByRef arr() As String)
  '    ' ##SUMMARY Outputs array of strings to debug window.
  '    ' ##PARAM arr I array of strings to output
  '    Dim i As Integer
  '    ' ##LOCAL i - counter for looping through arrays
  '    For i = LBound(arr) To UBound(arr)
  '        System.Diagnostics.Debug.WriteLine(String.Format("00", i) & ": " & arr(i))
  '    Next
  'End Sub


  Public Function StrSplit(ByRef Source As String, ByRef delim As String, ByRef quote As String) As String
    ' ##SUMMARY Divides string into 2 portions at position of 1st occurence of specified _
    'delimeter. Quote specifies a particular string that is exempt from the delimeter search.
    ' ##SUMMARY   Example: StrSplit("Julie, Todd, Jane, and Ray", ",", "") = "Julie", and "Todd, Jane, and Ray" is returned as Source.
    ' ##SUMMARY   Example: StrSplit("Julie, Todd, Jane, and Ray", ",", "Julie, Todd") = "Julie, Todd", and "Jane, and Ray" is returned as Source.
    ' ##PARAM Source M String to be analyzed
    ' ##PARAM delim I Single-character string delimeter
    ' ##PARAM quote I Multi-character string exempted from search.
    ' ##RETURNS  Returns leading portion of incoming string up to first occurence of delimeter. _
    'Returns input parameter without that portion. If no delimiter in string, _
    'returns whole string, and input parameter reduced to null string.
    Dim retval As String
    Dim i As Integer
    Dim quoted As Boolean
    Dim trimlen As Integer
    Dim quotlen As Integer
    ' ##LOCAL retval - string to return as StrSplit
    ' ##LOCAL i - long character position of search through Source
    ' ##LOCAL quoted - Boolean whether quote was encountered in Source
    ' ##LOCAL trimlen - long length of delimeter, or quote if encountered first
    ' ##LOCAL quotlen - long length of quote

    Source = LTrim(Source) 'remove leading blanks
    quotlen = Len(quote)
    If quotlen > 0 Then
      i = InStr(Source, quote)
      If i = 1 Then 'string beginning
        trimlen = quotlen
        Source = Mid(Source, trimlen + 1)
        i = InStr(Source, quote) 'string end
        quoted = True
      Else
        i = InStr(Source, delim)
        trimlen = Len(delim)
      End If
    Else
      i = InStr(Source, delim)
      trimlen = Len(delim)
    End If

    If i > 0 Then 'found delimeter
      retval = Left(Source, i - 1) 'string to return
      If Right(retval, 1) = " " Then retval = RTrim(retval)
      Source = LTrim(Mid(Source, i + trimlen)) 'string remaining
      If quoted And Len(Source) > 0 Then
        If Left(Source, Len(delim)) = delim Then Source = LTrim(Mid(Source, Len(delim) + 1))
      End If
    Else 'take it all
      retval = Source
      Source = "" 'nothing left
    End If

    StrSplit = retval

  End Function

  'Public Function StrRepeat(ByRef repeat As Integer, ByRef Source As String) As String
  '    ' ##SUMMARY Repeats specified string specified number of times.
  '    ' ##SUMMARY   Example: StrRepeat(3, "I wish I were in Kansas. ")
  '    ' ##PARAM repeat I Number of times for Source to be repeated
  '    ' ##PARAM Source I String to be repeated then returned
  '    ' ##RETURNS Returns input parameter Source in succession specified number of times.
  '    Dim retval As String
  '    Dim i As Integer
  '    ' ##LOCAL retval - string to return as StrRepeat
  '    ' ##LOCAL i - long index for 'repeat' loop

  '    For i = 1 To repeat
  '        retval = retval & Source
  '    Next
  '    StrRepeat = retval
  'End Function

  Function StrFirstInt(ByRef Source As String) As Integer
    ' ##SUMMARY Divides alpha numeric sequence into leading numbers and trailing characters.
    ' ##SUMMARY   Example: StrFirstInt("123Go!) = "123", and changes Source to "Go!"
    ' ##PARAM Source M String to be analyzed
    ' ##RETURNS  Returns leading numbers in Source, and returns Source without those numbers.
    Dim retval As Integer = 0
    Dim pos As Integer = 1
    ' ##LOCAL retval - number found at beginning of Source
    ' ##LOCAL pos - long character position in search through Source

    If IsNumeric(Left(Source, 2)) Then pos = 3 'account for negative number - sign
    While IsNumeric(Mid(Source, pos, 1))
      pos += 1
    End While

    If pos >= 2 Then
      retval = CInt(Left(Source, pos - 1))
      Source = LTrim(Mid(Source, pos))
    End If

    Return retval
  End Function

  'Sub StrToDate(ByRef txt As String, ByRef datevar As Object)
  '    ' ##SUMMARY Converts yyyy/mm/dd date string to date variant.
  '    ' ##PARAM txt I Date string
  '    ' ##PARAM datevar O Date variant
  '    Dim ilen As Integer
  '    Dim ipos As Integer
  '    Dim dattmp As Object
  '    ' ##LOCAL ilen - long length of text string
  '    ' ##LOCAL ipos - long character position in parse through text
  '    ' ##LOCAL dattmp - intermediate date variant

  '    txt = Trim(txt)
  '    ilen = Len(txt)
  '    datevar = ""
  '    If ilen > 0 Then
  '        ipos = InStr(txt, "/")
  '        If ipos > 0 Then
  '            dattmp = Right(txt, ilen - ipos) & "/" & Left(txt, ipos - 1)
  '        End If
  '    End If
  '    datevar = DateSerial(Year(dattmp), Month(dattmp), VB.Day(dattmp))
  'End Sub

  'Sub GetDate(ByRef big As Double, ByRef dyval As Object)
  '    ' ##SUMMARY Converts double-precision date to date variant.
  '    ' ##PARAM big I Double-precision date (i.e., 19851101 = Nov 1, 1985)
  '    ' ##PARAM dyval O Date variant
  '    Dim yr As Integer
  '    Dim mo As Integer
  '    Dim dy As Integer
  '    Dim tmp As Integer
  '    ' ##LOCAL yr - long year
  '    ' ##LOCAL mo - long month
  '    ' ##LOCAL dy - long day
  '    ' ##LOCAL tmp - long temporary value of double-precision date as it is parsed

  '    yr = big / 10000
  '    tmp = big - (yr * 10000)
  '    mo = tmp / 100
  '    dy = tmp - (mo * 100)
  '    dyval = DateSerial(yr, mo, dy)
  'End Sub

  'Sub GetDateParts(ByRef big As Double, ByRef yr As Integer, ByRef mo As Integer, ByRef dy As Integer)
  '    ' ##SUMMARY Converts double-precision date to traditional year, month, and day parts.
  '    ' ##SUMMARY   Example: big = 19851101 returns yr = 1985, mo = 11, dy = 1
  '    ' ##PARAM big I Double-precision date
  '    ' ##PARAM yr O Long year
  '    ' ##PARAM mo O Long month
  '    ' ##PARAM dy O Long day
  '    Dim tmp As Double
  '    ' ##LOCAL tmp - double-precision temporary value of date as it is parsed

  '    yr = big / 10000
  '    tmp = big - (yr * 10000)
  '    mo = tmp / 100
  '    dy = tmp - (mo * 100)
  'End Sub

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

  Public Function ReplaceStringNoCase(ByRef Source As String, ByRef Find As String, ByRef ReplaceWith As String) As String
    ' ##SUMMARY Replaces Find in Source with Replace (not case sensitive).
    ' ##SUMMARY Example: ReplaceStringNoCase("He came and he went", "He", "She") = "She came and She went"
    ' ##PARAM Source I Full string to be searched
    ' ##PARAM Find I Substring to be searched for and replaced
    ' ##PARAM Replace I Substring to replace Find
    ' ##RETURNS Returns new string like Source except that _
    'any occurences of Find (not case sensitive) are replaced with Replace.
    Dim retval As String
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
        retval = retval & Mid(Source, lastFindEnd, findPos - lastFindEnd) & ReplaceWith
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
    Dim retval As String
    Dim findPos As Integer
    Dim lastFindEnd As Integer
    Dim findlen As Integer
    Dim replacelen As Integer
    ' ##LOCAL retval - string to be returned as ReplaceString
    ' ##LOCAL findpos - long position of Find in Source
    ' ##LOCAL lastFindEnd - long position of first character after last replaced string in Source
    ' ##LOCAL findlen - long length of Find
    ' ##LOCAL replacelen - long length of Replace

    findlen = Len(Find)
    If findlen > 0 Then
      replacelen = Len(ReplaceWith)
      findPos = InStr(Source, Find)
      lastFindEnd = 1
      While findPos > 0
        retval = retval & Mid(Source, lastFindEnd, findPos - lastFindEnd) & ReplaceWith
        lastFindEnd = findPos + findlen
        findPos = InStr(findPos + findlen, Source, Find)
      End While
      ReplaceString = retval & Mid(Source, lastFindEnd)
    Else
      ReplaceString = Source
    End If
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
        Case 0 : GoTo EndFound
        Case 32 To 126 : retval = retval & ch
        Case Else : retval = retval & ReplaceWith
      End Select
    Next
EndFound:
    StrPrintable = retval
  End Function

  Public Function StrPad(ByRef S As String, ByVal NewLength As Short, Optional ByRef PadWith As String = " ", Optional ByRef PadLeft As Boolean = True) As String
    ' ##SUMMARY Pads a string with specific character to achieve a specified length.
    ' ##PARAM S M String to be padded.
    ' ##PARAM NewLength I Length of padded string to be returned.
    ' ##PARAM PadWith I Character with which to pad the string.
    ' ##PARAM PadLeft I Pad left if true, pad right if false.
    ' ##RETURNS Input parameter S padded to left or right (default=left) with _
    'specific character (default=space) to specified length.
    Dim CharsToAdd As Short 'number of characters added to S

    CharsToAdd = NewLength - Len(S)
    If CharsToAdd <= 0 Then
      StrPad = S
    ElseIf PadLeft Then
      StrPad = New String(PadWith, CharsToAdd) & S
    Else
      StrPad = S & New String(PadWith, CharsToAdd)
    End If

  End Function

  'Public Sub DecimalAlign(ByRef S() As String, Optional ByRef PadLeft As Boolean = True, Optional ByRef PadRight As Boolean = True, Optional ByRef MinWidth As Short = 0)
  '    ' ##SUMMARY Formats array of floating point decimals around location of decimal place.
  '    ' ##PARAM S M String array containing values to be formatted.
  '    ' ##PARAM PadLeft I Number of spaces reserved to the left of the decimal place.
  '    ' ##PARAM PadRight I Number of spaces reserved to the right of the decimal place.
  '    ' ##PARAM MinWidth I Minimum number of spaces reserved for overall formatted number.
  '    Dim MaxDecimalPos As Short 'furthest decimal position from left for all numbers in s
  '    Dim MaxAfterDecimal As Short 'furthest decimal position from right for all numbers in s
  '    Dim AfterDecimal() As Short 'array of digits after decimal
  '    Dim DecimalPos() As Short 'array of decimal positions from left
  '    Dim iMin As Short 'lower bound of s
  '    Dim iMax As Short 'upper bound of s
  '    Dim i As Short 'loop counter

  '    iMin = LBound(S)
  '    iMax = UBound(S)
  '    ReDim DecimalPos(iMax)
  '    ReDim AfterDecimal(iMax)
  '    For i = iMin To iMax
  '        DecimalPos(i) = InStr(S(i), ".")
  '        If DecimalPos(i) = 0 Then DecimalPos(i) = Len(S(i)) + 1
  '        If DecimalPos(i) > MaxDecimalPos Then MaxDecimalPos = DecimalPos(i)
  '        If PadRight Then
  '            AfterDecimal(i) = Len(S(i)) - DecimalPos(i)
  '            If AfterDecimal(i) > MaxAfterDecimal Then MaxAfterDecimal = AfterDecimal(i)
  '        End If
  '    Next
  '    For i = iMin To iMax
  '        If PadLeft Then
  '            If DecimalPos(i) < MaxDecimalPos Then
  '                S(i) = Space(MaxDecimalPos - DecimalPos(i)) & S(i)
  '            End If
  '        End If
  '        If PadRight Then
  '            If AfterDecimal(i) < MaxAfterDecimal Then
  '                S(i) = S(i) & Space(MaxAfterDecimal - AfterDecimal(i))
  '            End If
  '        End If
  '        If MinWidth > 0 Then S(i) = StrPad(S(i), MinWidth)
  '    Next

  'End Sub

  'Public Sub DispError(ByRef SubID As String, ByRef e As Object)
  '    ' ##SUMMARY Displays error in message box for up to first 4 errors from same module.
  '    ' ##PARAM SubID I ID of subroutine
  '    ' ##PARAM E I Error object
  '    Static ecnt As Integer
  '    ' ##LOCAL ecnt - running count of errors from module

  '    ecnt = ecnt + 1
  '    If ecnt < 5 Then
  '        MsgBox("From Sub " & SubID & ":" & vbCrLf & e.Description & vbCrLf & "Error number " & e.Number & vbCrLf, 48)
  '    End If

  'End Sub

  'Sub ChkProb(ByRef id As String, ByRef txt As String, ByRef Min As String, ByRef Max As String, ByRef opt As Integer, ByRef rsp As Integer)
  '    ' ##SUMMARY Displays problem message box when entered value out of range.
  '    ' ##PARAM id I Name of variable
  '    ' ##PARAM txt I Entered value of variable
  '    ' ##PARAM Min I Minimum variable value
  '    ' ##PARAM Max I Maximum variable value
  '    ' ##PARAM opt I Flag allowing user confirmation to override limits (1 = OK)
  '    ' ##PARAM rsp O Response indicating whether overriding limits allowed (0 = no)
  '    Dim eStr As String
  '    Dim NL As String
  '    ' ##LOCAL eStr - error string
  '    ' ##LOCAL nl - carriage return line feed string

  '    NL = Chr(13) & Chr(10)
  '    eStr = "'" & txt & "' is not a valid value for" & NL
  '    eStr = eStr & "'" & id & "'" & NL & NL
  '    eStr = eStr & "Min:  " & Min
  '    eStr = eStr & "  Max:  " & Max & NL
  '    If opt = 1 Then 'confirm override of limits
  '        eStr = eStr & "Please confirm that you want to use it."
  '        rsp = MsgBox(eStr, 305, id & "Problem")
  '    Else
  '        MsgBox(eStr, MsgBoxStyle.Exclamation, id & "Problem")
  '        rsp = 0
  '    End If

  'End Sub

  'Sub ChkTxtI(ByRef id As String, ByRef Min As Integer, ByRef Max As Integer, ByRef txt As String, ByRef cVal As Integer, ByRef chdFlg As Integer)
  '    ' ##SUMMARY Checks entered integer value for valid range. _
  '    'Automatically allows user confirmation to override Min/Max limits.
  '    ' ##PARAM ID I Variable name
  '    ' ##PARAM Min I Minimum variable value
  '    ' ##PARAM Max I Maximum variable value
  '    ' ##PARAM txt I Entered value of variable
  '    ' ##PARAM cVal I Previous value of variable
  '    ' ##PARAM chdFlg O Flag for whether integer value changed (0 = no, 1 = yes)
  '    Dim opt As Integer
  '    ' ##LOCAL opt - long set to 1 allowing user confirmation to override limits

  '    opt = 1
  '    Call ChkTxtIOpt(id, Min, Max, opt, txt, cVal, chdFlg)

  'End Sub

  'Sub ChkTxtR(ByRef id As String, ByRef Min As Single, ByRef Max As Single, ByRef txt As String, ByRef cVal As Single, ByRef chdFlg As Integer)
  '    ' ##SUMMARY Checks entered real value for valid range. _
  '    'Automatically allows user confirmation to override Min/Max limits.
  '    ' ##PARAM ID I Variable name
  '    ' ##PARAM Min I Minimum variable value
  '    ' ##PARAM Max I Maximum variable value
  '    ' ##PARAM txt I Entered value of variable
  '    ' ##PARAM cVal I Previous value of variable
  '    ' ##PARAM chdFlg O Flag for whether real value changed (0 = no, 1 = yes)
  '    Dim opt As Integer
  '    ' ##LOCAL opt - long set to 1 allowing user confirmation to override limits

  '    opt = 1
  '    Call ChkTxtROpt(id, Min, Max, opt, txt, cVal, chdFlg)

  'End Sub

  'Public Sub ChkTxtIOpt(ByRef id As String, ByRef Min As Integer, ByRef Max As Integer, ByRef opt As Integer, ByRef txt As String, ByRef cVal As Integer, ByRef chdFlg As Integer)
  '    ' ##SUMMARY Checks entered integer value for valid range. _
  '    'If opt = 1, allows user confirmation to override value outside Min/Max limits.
  '    ' ##PARAM ID I Variable name
  '    ' ##PARAM Min I Minimum variable value
  '    ' ##PARAM Max I Maximum variable value
  '    ' ##PARAM opt I Flag allowing user confirmation to override limits (1 = yes)
  '    ' ##PARAM txt I Entered value of variable
  '    ' ##PARAM cVal I Previous value of variable
  '    ' ##PARAM chdFlg O Flag for whether integer value changed (0 = no, 1 = yes)
  '    Dim oldval As Integer
  '    Dim newval As Integer
  '    Dim probFlg As Integer
  '    Dim rsp As Integer
  '    ' ##LOCAL oldval - long previous value of variable
  '    ' ##LOCAL newval - long user-entered value rounded to nearest integer
  '    ' ##LOCAL probFlg - long problem flag (-1 = no change, 0 = no problem, 1 = invalid change)
  '    ' ##LOCAL rsp - long response indicating whether overriding limits allowed (0 = no, 1 = yes)

  '    probFlg = 1
  '    chdFlg = 0
  '    If IsNumeric(txt) Then
  '        '   no really bad problems
  '        oldval = cVal
  '        newval = CShort(txt)
  '        If (oldval = newval) Then
  '            '     no change
  '            probFlg = -1
  '        ElseIf (newval >= Min And newval <= Max) Then
  '            '     update listing
  '            cVal = newval
  '            probFlg = 0
  '            chdFlg = 1
  '        End If
  '    End If
  '    If probFlg = 1 Then
  '        '   not a valid change
  '        Call ChkProb(id, txt, CStr(Min), CStr(Max), opt, rsp)
  '    End If
  '    txt = CStr(cVal)

  'End Sub

  'Public Sub ChkTxtROpt(ByRef id As String, ByRef Min As Single, ByRef Max As Single, ByRef opt As Integer, ByRef txt As String, ByRef cVal As Single, ByRef chdFlg As Integer)
  '    ' ##SUMMARY Checks entered real value for valid range. _
  '    'If opt = 1, allows user confirmation to override value outside Min/Max limits.
  '    ' ##PARAM ID I Variable name
  '    ' ##PARAM Min I Minimum variable value
  '    ' ##PARAM Max I Maximum variable value
  '    ' ##PARAM opt I Flag allowing user confirmation to override limits (1 = yes)
  '    ' ##PARAM txt I Entered value of variable
  '    ' ##PARAM cVal I Previous value of variable
  '    ' ##PARAM chdFlg O Flag for whether real value changed (0 = no, 1 = yes)
  '    Dim oldval As Single
  '    Dim newval As Single
  '    Dim rsp As Integer
  '    ' ##LOCAL oldval - long previous value of variable
  '    ' ##LOCAL newval - long user-entered value rounded to nearest integer
  '    ' ##LOCAL rsp - long response indicating whether overriding limits allowed (0 = no, 1 = yes)

  '    If IsNumeric(txt) Then
  '        'no really bad problems
  '        oldval = cVal
  '        newval = CDbl(txt)
  '        If (oldval = newval) Then
  '            'no change
  '            chdFlg = 0
  '        ElseIf (newval >= Min And newval <= Max) Then
  '            'update listing
  '            cVal = newval
  '            chdFlg = 1
  '        Else
  '            'not a valid change
  '            Call ChkProb(id, txt, CStr(Min), CStr(Max), opt, rsp)
  '            If rsp = 1 Then
  '                'user wants to use value anyway
  '                cVal = newval
  '                chdFlg = -1
  '            Else
  '                'don't use value
  '                chdFlg = 0
  '            End If
  '        End If
  '    ElseIf lenStr(txt) > 0 Then
  '        MsgBox("Value entered is not a number.", 48)
  '        chdFlg = 0
  '    End If
  '    If cVal <> -999 Then
  '        '-999 indicates undefined value, leave field blank
  '        txt = CStr(cVal)
  '    End If
  'End Sub

  Public Function Long2String(ByRef Value As Integer) As String
    ' ##SUMMARY Parses long integer to FourByteType then prints out corresponding ascii codes.
    ' ##SUMMARY   Example: Long2String(98) = "b   "
    ' ##PARAM Value I Value to be converted
    ' ##RETURNS Input parameter Val in string form.
    Dim bVal As Byte()
    ' ##LOCAL bval - FourByteType equivalent value of lVal

    bVal = System.BitConverter.GetBytes(Value)
    Return Chr(bVal(0)) & Chr(bVal(1)) & Chr(bVal(2)) & Chr(bVal(3))
  End Function

  Public Function Long2Single(ByRef Value As Integer) As Single
    ' ##SUMMARY Sets long integer to LongType then converts to SingleType.
    ' ##SUMMARY   Example: Long2Single(999999999) =  4.723787E-03
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

  '    Public Sub SortIntegerArray(ByRef opt As Integer, ByRef cnt As Integer, ByRef iVal() As Integer, ByRef pos() As Integer)
  '        ' ##SUMMARY Sorts integers in array into ascending order.
  '        ' ##PARAM opt I Sort option (0 = sort in place, 1 = move values in array to sorted position)
  '        ' ##PARAM cnt I Count of integers to sort
  '        ' ##PARAM iVal I Array of integers to sort
  '        ' ##PARAM pos O Array containing sorted order of integers
  '        Dim i As Integer
  '        Dim j As Integer
  '        Dim jpt As Integer
  '        Dim jpt1 As Integer
  '        Dim itmp As Integer
  '        ' ##LOCAL i - long counter for outer loop
  '        ' ##LOCAL j - long counter for inner loop
  '        ' ##LOCAL jpt - long pointer to j index
  '        ' ##LOCAL jpt1 - long pointer to (j + 1) index
  '        ' ##LOCAL itmp - long temporary holder for values in iVal array

  '        'set default positions(assume in order)
  '        For i = 1 To cnt
  '            pos(i) = i
  '        Next i

  '        'make a pointer to values with bubble sort
  '        For i = cnt To 2 Step -1
  '            For j = 1 To i - 1
  '                jpt = pos(j)
  '                jpt1 = pos(j + 1)
  '                If (iVal(jpt) > iVal(jpt1)) Then
  '                    pos(j + 1) = jpt
  '                    pos(j) = jpt1
  '                End If
  '            Next j
  '        Next i

  '        If (opt = 1) Then
  '            'move integer values to their sorted positions
  '            For i = 1 To cnt
  '                If (pos(i) <> i) Then
  '                    'need to move ints, first save whats in target space
  '                    itmp = iVal(i)
  '                    'move sorted data to target position
  '                    iVal(i) = iVal(pos(i))
  '                    'move temp data to source position
  '                    iVal(pos(i)) = itmp
  '                    'find the pointer to the other value we are moving
  '                    j = i
  '50:                 'CONTINUE
  '                    j = j + 1
  '                    If (pos(j) <> i) Then GoTo 50
  '                    pos(j) = pos(i)
  '                    pos(i) = i
  '                End If
  '            Next i
  '        End If

  '    End Sub

  '    Public Sub SortRealArray(ByRef opt As Integer, ByRef cnt As Integer, ByRef rval() As Single, ByRef pos() As Integer)
  '        ' ##SUMMARY Sorts array of real numbers into ascending order.
  '        ' ##PARAM opt I Integer indicating sort option: 0 - sort in place, 1 - move values in _
  '        'array to sorted position.
  '        ' ##PARAM cnt I Count of real numbers to sort.
  '        ' ##PARAM rval M Array of real numbers to sort.
  '        ' ##PARAM pos O Integer array containing sorted order of real numbers.
  '        Dim i As Integer
  '        Dim j As Integer
  '        Dim jpt As Integer
  '        Dim jpt1 As Integer
  '        Dim rtmp As Single
  '        ' ##LOCAL i - long counter for outer loop
  '        ' ##LOCAL j - long counter for inner loop
  '        ' ##LOCAL jpt - long pointer to j index
  '        ' ##LOCAL jpt1 - long pointer to (j + 1) index
  '        ' ##LOCAL itmp - long temporary holder for values in rVal array

  '        'set default positions(assume in order)
  '        For i = 1 To cnt
  '            pos(i) = i
  '        Next i

  '        'make a pointer to values with bubble sort
  '        For i = cnt To 2 Step -1
  '            For j = 1 To i - 1
  '                jpt = pos(j)
  '                jpt1 = pos(j + 1)
  '                If (rval(jpt) > rval(jpt1)) Then
  '                    pos(j + 1) = jpt
  '                    pos(j) = jpt1
  '                End If
  '            Next j
  '        Next i

  '        If (opt = 1) Then
  '            'move real values to their sorted positions
  '            For i = 1 To cnt
  '                If (pos(i) <> i) Then
  '                    'need to move reals, first save whats in target space
  '                    rtmp = rval(i)
  '                    'move sorted data to target position
  '                    rval(i) = rval(pos(i))
  '                    'move temp data to source position
  '                    rval(pos(i)) = rtmp
  '                    'find the pointer to the other value we are moving
  '                    j = i
  '50:                 'CONTINUE
  '                    j = j + 1
  '                    If (pos(j) <> i) Then GoTo 50
  '                    pos(j) = pos(i)
  '                    pos(i) = i
  '                End If
  '            Next i
  '        End If

  '    End Sub

  Public Function PatternMatch(ByVal Source As String, ByVal Pattern As String) As Boolean
    ' ##SUMMARY Searches string for presence of pattern.
    ' ##SUMMARY Example: PatternMatch("He left", "He*") = True
    ' ##SUMMARY Example: PatternMatch("He left", "left*") = False
    ' ##PARAM Str I String to be searched
    ' ##PARAM Pattern I Substring to be searched for
    ' ##RETURNS Boolean indicating whether substring was found in contents of string.
    ' ##REMARKS Special characters: # as any digit, ? as any character, * as zero or more of _
    'any character. If pattern does not contain a leading * the pattern must match _
    'the beginning of str. If pattern does not contain a trailing * the pattern must _
    'match the end of str.
    Dim patCh As String
    Dim strCh As String
    Dim patPos As Integer
    Dim strPos As Integer
    Dim lenPat As Integer
    Dim lenStr As Integer
    Dim findPos As Integer
    ' ##LOCAL patCh - string character in Pattern
    ' ##LOCAL strCh - string character in Str
    ' ##LOCAL patPos - long position of character in Pattern
    ' ##LOCAL strPos - long position of character in Str
    ' ##LOCAL lenPat - long length of Pattern
    ' ##LOCAL lenStr - long length of Str
    ' ##LOCAL findPos - long position of patCh in Str relative to strPos

    lenPat = Len(Pattern)
    lenStr = Len(Source)
    strPos = 1
    For patPos = 1 To Len(Pattern)
      strCh = Mid(Source, strPos, 1)
      patCh = Mid(Pattern, patPos, 1)
      Select Case patCh
        Case "#"
          If Not IsNumeric(strCh) Then
            GoTo NoMatch
          Else
            strPos = strPos + 1
          End If
        Case "?"
          strPos = strPos + 1
        Case "*"
MatchStar:
          patPos = patPos + 1
          If patPos > lenPat Then 'Trailing * in pattern, match to end of string
            strPos = lenStr + 1
          Else
            patCh = Mid(Pattern, patPos, 1)
            Select Case patCh
              Case "?", "*" : GoTo MatchStar 'Skip redundant wild cards
              Case "#"
                While Not IsNumeric(strCh)
                  strPos = strPos + 1
                  If strPos > lenStr Then GoTo NoMatch
                  strCh = Mid(Source, strPos, 1)
                End While
                strPos = strPos + 1
              Case Else
                findPos = InStr(strPos, Source, patCh)
                If findPos = 0 Then
                  GoTo NoMatch
                Else
                  strPos = findPos + 1
                End If
            End Select
          End If
        Case Else
          If strCh <> patCh Then
            GoTo NoMatch
          Else
            strPos = strPos + 1
          End If
      End Select
    Next
    If strPos > Len(Source) Then PatternMatch = True
NoMatch:
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
    Dim i As Long
    Dim j As Long
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

End Module
