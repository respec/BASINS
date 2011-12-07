Imports atcData
Imports atcUtility
Imports MapWinUtility
Imports MapWinUtility.Strings

Module modUEBUtil
    Dim lChrSep() As String = {" ", vbTab, vbCrLf}


    Public Sub OpenMasterFile(ByVal aFilename As String, _
                              ByRef aWeatherFileName As String, ByRef aOutputFileName As String, _
                              ByRef aParameterFileName As String, ByRef aSiteFileName As String, _
                              ByRef aBCParameterFileName As String, ByRef aRadOpt As Integer)
        Dim lPath As String = PathNameOnly(aFilename) & "\"
        Dim lStr As String = WholeFileString(aFilename)

        aWeatherFileName = lPath & StrRetRem(lStr) 'should be weather input file
        aOutputFileName = lPath & StrRetRem(lStr) 'should be output file
        aParameterFileName = lPath & StrRetRem(lStr) 'should be parameter file
        aSiteFileName = lPath & StrRetRem(lStr) 'should be site file
        aBCParameterFileName = lPath & StrRetRem(lStr) 'should be B-C parameter file
        aRadOpt = StrRetRem(lStr) 'should be input radiation option
    End Sub

    Public Sub ReadDataFile(ByVal aFilename As String, ByRef aDataArray() As Double)

        Dim lStr As String = WholeFileString(aFilename)
        lStr = ReplaceRepeats(lStr, " ") 'remove extra blanks
        Dim lStrArray() As String = lStr.Split(lChrSep, StringSplitOptions.None)
        ReDim aDataArray(lStrArray.Length - 1)
        Dim j As Integer = 0
        For i As Integer = 0 To UBound(lStrArray)
            If IsNumeric(lStrArray(i)) Then
                aDataArray(j) = CDbl(lStrArray(i))
                j += 1
            End If
        Next
    End Sub

    Public Function WriteMasterFile(ByVal aFilename As String, _
                                    ByRef aWeatherFileName As String, ByRef aOutputFileName As String, _
                                    ByRef aParameterFileName As String, ByRef aSiteFileName As String, _
                                    ByRef aBCParameterFileName As String, ByRef aRadOpt As Integer) As Boolean

        Dim lStr As String

        If aWeatherFileName.Length > 0 AndAlso aOutputFileName.Length > 0 AndAlso aParameterFileName.Length > 0 AndAlso _
           aSiteFileName.Length > 0 AndAlso aBCParameterFileName.Length > 0 Then
            Try
                lStr = aWeatherFileName & " " & aOutputFileName & " " & aParameterFileName & " " & aSiteFileName & " " & aBCParameterFileName & " " & aRadOpt
                SaveFileString(aFilename, lStr)
                Return True
            Catch ex As Exception
                Return False
            End Try
        Else
            Return False
        End If
    End Function

    Public Function WriteBCParmsFile(ByVal aFileName As String, ByVal aBCParmsArray() As Double) As Boolean

        Dim lStr As String = ""

        If aFileName.Length > 0 Then
            Try
                lStr = aBCParmsArray(0) & " " & aBCParmsArray(1) & "   A, C" & vbCrLf
                For i As Integer = 2 To UBound(aBCParmsArray)
                    lStr &= aBCParmsArray(i) & "   "
                    If ((i + 2) Mod 3) = 0 Then 'make a new line
                        lStr &= vbCrLf
                    End If
                Next
                SaveFileString(aFileName, lStr)
                Return True
            Catch ex As Exception
                Return False
            End Try
        Else
            Return False
        End If
    End Function
End Module
