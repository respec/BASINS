Imports atcData
Imports atcUtility
Imports MapWinUtility
Imports MapWinUtility.Strings

Module modUEBUtil

    Public Sub OpenMasterFile(ByVal aFilename As String, ByRef aRunLabel As String, _
                              ByRef aParameterFileName As String, ByRef aSiteFileName As String, _
                              ByRef aInputControlFileName As String, ByRef aOutputControlFileName As String, _
                              ByRef aWatershedFileName As String, ByRef aWatershedVariableName As String, _
                              ByRef aAggOutputControlFileName As String, ByRef aAggOutputFileName As String)


        Dim lPath As String = PathNameOnly(aFilename) & "\"
        Dim lFileStr As String = WholeFileString(aFilename)

        aRunLabel = StrSplit(lFileStr, vbCrLf, "") 'description of this run
        aParameterFileName = lPath & StrSplit(lFileStr, vbCrLf, "") 'should be parameter file
        aSiteFileName = lPath & StrSplit(lFileStr, vbCrLf, "") 'should be site file
        aInputControlFileName = lPath & StrSplit(lFileStr, vbCrLf, "") 'should be input variable file
        aOutputControlFileName = lPath & StrSplit(lFileStr, vbCrLf, "") 'should be output variable file
        aWatershedFileName = lPath & StrSplit(lFileStr, vbCrLf, "") 'should be watershed grid file
        aWatershedVariableName = StrSplit(lFileStr, vbCrLf, "") 'should be watershed variable name
        aAggOutputControlFileName = lPath & StrSplit(lFileStr, vbCrLf, "") 'should be aggregated output control file
        aAggOutputFileName = lPath & StrSplit(lFileStr, vbCrLf, "") 'should be aggregated output file

    End Sub

    Public Function WriteMasterFile(ByVal aFilename As String, ByRef aRunLabel As String, _
                              ByRef aParameterFileName As String, ByRef aSiteFileName As String, _
                              ByRef aInputControlFileName As String, ByRef aOutputControlFileName As String, _
                              ByRef aWatershedFileName As String, ByRef aWatershedVariableName As String, _
                              ByRef aAggOutputControlFileName As String, ByRef aAggOutputFileName As String) As Boolean

        Dim lStr As String
        Dim lPath As String = PathNameOnly(aFilename)

        If aParameterFileName.Length > 0 AndAlso aSiteFileName.Length > 0 AndAlso _
           aInputControlFileName.Length > 0 AndAlso aOutputControlFileName.Length > 0 AndAlso _
           aWatershedFileName.Length > 0 AndAlso aWatershedVariableName.Length > 0 AndAlso _
           aAggOutputControlFileName.Length > 0 AndAlso aAggOutputFileName.Length > 0 Then
            Try
                lStr = aRunLabel & vbCrLf & RelativeFilename(aParameterFileName, lPath) & vbCrLf & _
                       RelativeFilename(aSiteFileName, lPath) & vbCrLf & _
                       RelativeFilename(aInputControlFileName, lPath) & vbCrLf & _
                       RelativeFilename(aOutputControlFileName, lPath) & vbCrLf & _
                       RelativeFilename(aWatershedFileName, lPath) & vbCrLf & aWatershedVariableName & vbCrLf & _
                       RelativeFilename(aAggOutputControlFileName, lPath) & vbCrLf & _
                       RelativeFilename(aAggOutputFileName, lPath)
                SaveFileString(aFilename, lStr)
                Return True
            Catch ex As Exception
                Return False
            End Try
        Else
            Return False
        End If
    End Function

End Module
