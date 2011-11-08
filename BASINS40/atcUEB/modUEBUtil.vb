Imports atcData
Imports atcUtility
Imports MapWinUtility
Imports MapWinUtility.Strings

Module modUEBUtil
    Friend pFrmUEB As frmUEB

    Public Sub OpenMasterFile(ByVal aFilename As String,
                              ByRef aWeatherFileName As String, ByRef aOutputFileName As String, _
                              ByVal aParameterFileName As String, ByRef aSiteFileName As String, _
                              ByRef aBCParameterFileName As String, ByRef aRadOpt As Integer)

        Dim lStr As String = WholeFileString(aFilename)

        aWeatherFileName = StrRetRem(lStr) 'should be weather input file
        aOutputFileName = StrRetRem(lStr) 'should be output file
        aParameterFileName = StrRetRem(lStr) 'should be parameter file
        aSiteFileName = StrRetRem(lStr) 'should be site file
        aBCParameterFileName = StrRetRem(lStr) 'should be B-C parameter file
        aRadOpt = StrRetRem(lStr) 'should be input radiation option
    End Sub
End Module
