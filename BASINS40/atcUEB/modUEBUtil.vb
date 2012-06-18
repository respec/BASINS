Imports atcData
Imports atcUtility
Imports MapWinUtility
Imports MapWinUtility.Strings

Module modUEBUtil
    Dim lChrSep() As String = {" ", vbTab, vbCrLf}


    Public Sub OpenMasterFile(ByVal aFilename As String, ByRef aRunLabel As String, _
                              ByRef aParameterFileName As String, ByRef aSiteFileName As String, _
                              ByRef aInputVarsFileName As String, ByRef aOutputVarsFileName As String, _
                              ByRef aWatershedFileName As String, ByRef aWatershedVariableName As String, _
                              ByRef aAggOutputControlFileName As String, ByRef aAggOutputFileName As String)


        Dim lPath As String = PathNameOnly(aFilename) & "\"
        Dim lFileStr As String = WholeFileString(aFilename)

        aRunLabel = StrSplit(lFileStr, vbCrLf, "") 'description of this run
        aParameterFileName = lPath & StrSplit(lFileStr, vbCrLf, "") 'should be parameter file
        aSiteFileName = lPath & StrSplit(lFileStr, vbCrLf, "") 'should be site file
        aInputVarsFileName = lPath & StrSplit(lFileStr, vbCrLf, "") 'should be input variable file
        aOutputVarsFileName = lPath & StrSplit(lFileStr, vbCrLf, "") 'should be output variable file
        aWatershedFileName = lPath & StrSplit(lFileStr, vbCrLf, "") 'should be watershed grid file
        aWatershedVariableName = StrSplit(lFileStr, vbCrLf, "") 'should be watershed variable name
        aAggOutputControlFileName = lPath & StrSplit(lFileStr, vbCrLf, "") 'should be aggregated output control file
        aAggOutputFileName = lPath & StrSplit(lFileStr, vbCrLf, "") 'should be aggregated output file

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

    Public Function WriteMasterFile(ByVal aFilename As String, ByRef aRunLabel As String, _
                              ByRef aParameterFileName As String, ByRef aSiteFileName As String, _
                              ByRef aInputVarsFileName As String, ByRef aOutputVarsFileName As String, _
                              ByRef aWatershedFileName As String, ByRef aAggOutputControlFileName As String, _
                              ByRef aAggOutputFileName As String) As Boolean

        Dim lStr As String

        If aParameterFileName.Length > 0 AndAlso aSiteFileName.Length > 0 AndAlso _
           aInputVarsFileName.Length > 0 AndAlso aOutputVarsFileName.Length > 0 AndAlso _
           aWatershedFileName.Length > 0 AndAlso aAggOutputControlFileName.Length > 0 AndAlso _
           aAggOutputFileName.Length > 0 Then
            Try
                lStr = aRunLabel & vbCrLf & aParameterFileName & vbCrLf & aSiteFileName & vbCrLf & _
                       aInputVarsFileName & vbCrLf & aOutputVarsFileName & vbCrLf & aWatershedFileName & vbCrLf & _
                       aAggOutputControlFileName & vbCrLf & aAggOutputFileName
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
