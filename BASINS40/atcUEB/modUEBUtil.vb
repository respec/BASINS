Imports atcData
Imports atcUtility
Imports MapWinUtility
Imports MapWinUtility.Strings

Module modUEBUtil
    Dim lChrSep() As String = {" ", vbTab, vbCrLf}


    Public Sub OpenMasterFile(ByVal aFilename As String,
                              ByRef aWeatherFileName As String, ByRef aOutputFileName As String, _
                              ByRef aParameterFileName As String, ByRef aSiteFileName As String, _
                              ByRef aBCParameterFileName As String, ByRef aRadOpt As Integer)

        Dim lStr As String = WholeFileString(aFilename)

        aWeatherFileName = StrRetRem(lStr) 'should be weather input file
        aOutputFileName = StrRetRem(lStr) 'should be output file
        aParameterFileName = StrRetRem(lStr) 'should be parameter file
        aSiteFileName = StrRetRem(lStr) 'should be site file
        aBCParameterFileName = StrRetRem(lStr) 'should be B-C parameter file
        aRadOpt = StrRetRem(lStr) 'should be input radiation option
    End Sub

    Public Sub ReadWeatherFile(ByVal aFilename As String, _
                               ByRef aSDate() As Integer, ByRef aTStep As Integer, _
                               ByRef aInitialEnergy As Double, ByRef aInitialH2OEquiv As Double, ByRef aInitialSnowAge As Double)

        Dim lSR As New IO.StreamReader(aFilename)
        Dim lStr As String
        Dim lCnt As Integer = 9
        Dim lStrArray(7) As String

        lStr = lSR.ReadLine
        lStrArray = lStr.Split(lChrSep, lCnt, StringSplitOptions.None)
        aSDate(1) = CInt(lStrArray(0))
        aSDate(2) = CInt(lStrArray(1))
        aSDate(0) = CInt(lStrArray(2))
        If aSDate(0) < 100 Then
            aSDate(0) += 1900
        End If
        aSDate(3) = CInt(lStrArray(3))
        aTStep = CInt(lStrArray(4))
        aInitialEnergy = CDbl(lStrArray(5))
        aInitialH2OEquiv = CDbl(lStrArray(6))
        aInitialSnowAge = CDbl(lStrArray(7))
        lStr = lSR.ReadLine
        'TODO: how to handle skipping of met records
        'Dim lNumSkip As Integer = StrRetRem(lStr)
        'If lNumSkip > 0 Then

        'End If
    End Sub

    Public Sub ReadDataFile(ByVal aFilename As String, ByRef aDataArray() As Double)

        Dim lStr As String = WholeFileString(aFilename)
        lStr = ReplaceRepeats(lStr, " ") 'remove extra blanks
        Dim lStrArray() As String = lStr.Split(lChrSep, StringSplitOptions.None)
        Dim j As Integer = 0
        For i As Integer = 0 To UBound(lStrArray)
            If IsNumeric(lStrArray(i)) Then
                aDataArray(j) = CDbl(lStrArray(i))
                j += 1
            End If
        Next
    End Sub

End Module
