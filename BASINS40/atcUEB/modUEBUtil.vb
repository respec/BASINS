Imports atcData
Imports atcUtility
Imports MapWinUtility
Imports MapWinUtility.Strings
Imports atcTimeseriesNetCDF

Module modUEBUtil

    Public Sub OpenMasterFile(ByVal aFilename As String, ByRef aRunLabel As String, _
                              ByRef aParameterFileName As String, ByRef aSiteFileName As String, _
                              ByRef aInputControlFileName As String, ByRef aOutputControlFileName As String, _
                              ByRef aWatershedFileName As String, ByRef aWatershedXVarName As String, _
                              ByRef aWatershedYVarName As String, ByRef aWatershedVariableName As String, _
                              ByRef aAggOutputControlFileName As String, ByRef aAggOutputFileName As String)


        Dim lPath As String = PathNameOnly(aFilename) & "\"
        Dim lFileStr As String = WholeFileString(aFilename)
        Dim lKey As String

        aRunLabel = StrSplit(lFileStr, vbCrLf, "") 'description of this run
        aParameterFileName = lPath & StrSplit(lFileStr, vbCrLf, "") 'should be parameter file
        aSiteFileName = lPath & StrSplit(lFileStr, vbCrLf, "") 'should be site file
        aInputControlFileName = lPath & StrSplit(lFileStr, vbCrLf, "") 'should be input variable file
        aOutputControlFileName = lPath & StrSplit(lFileStr, vbCrLf, "") 'should be output variable file
        Dim lRec As String = StrSplit(lFileStr, vbCrLf, "") 'should be watershed file and its variable specs
        aWatershedFileName = lPath & StrSplit(lRec, ";", "") 'should be watershed grid file
        While lRec.Length > 0
            lKey = StrSplit(lRec, ":", "")
            Select Case lKey.ToUpper
                Case "X" : aWatershedXVarName = StrSplit(lRec, ";", "")
                Case "Y" : aWatershedYVarName = StrSplit(lRec, ";", "")
                Case "D" : aWatershedVariableName = StrSplit(lRec, ";", "")
            End Select
        End While
        If aWatershedXVarName Is Nothing Or aWatershedYVarName Is Nothing Then 'look for X/Y variable names on netCDF file
            If FileExists(aWatershedFileName) Then
                Dim lNetCDFFile As New atcTimeseriesNetCDF.atcNetCDFFile(aWatershedFileName)
                For Each lDim As atcTimeseriesNetCDF.atcNetCDFDimension In lNetCDFFile.Dimensions
                    If lDim.Name.ToLower.StartsWith("x") Or lDim.Name.ToLower.StartsWith("lon") Then
                        aWatershedXVarName = lDim.Name
                    ElseIf lDim.Name.ToLower.StartsWith("y") Or lDim.Name.ToLower.StartsWith("lat") Then
                        aWatershedYVarName = lDim.Name
                    End If
                Next
            End If
        End If
        aAggOutputControlFileName = lPath & StrSplit(lFileStr, vbCrLf, "") 'should be aggregated output control file
        aAggOutputFileName = lPath & StrSplit(lFileStr, vbCrLf, "") 'should be aggregated output file

    End Sub

    Public Function WriteMasterFile(ByVal aFilename As String, ByRef aRunLabel As String, _
                              ByRef aParameterFileName As String, ByRef aSiteFileName As String, _
                              ByRef aInputControlFileName As String, ByRef aOutputControlFileName As String, _
                              ByRef aWatershedFileName As String, ByRef aWatershedXVarName As String, _
                              ByRef aWatershedYVarName As String, ByRef aWatershedVariableName As String, _
                              ByRef aAggOutputControlFileName As String, ByRef aAggOutputFileName As String) As Boolean

        Dim lStr As String
        Dim lFilename As String = aFilename
        Dim lPath As String = PathNameOnly(lFilename)
        If lPath.Length = 0 Then
            lPath = CurDir()
            lFilename = lPath & "\" & lFilename
        End If

        If aParameterFileName.Length > 0 AndAlso aSiteFileName.Length > 0 AndAlso _
           aInputControlFileName.Length > 0 AndAlso aOutputControlFileName.Length > 0 AndAlso _
           aWatershedFileName.Length > 0 AndAlso aWatershedVariableName.Length > 0 AndAlso _
           aAggOutputControlFileName.Length > 0 AndAlso aAggOutputFileName.Length > 0 Then
            Try
                lStr = aRunLabel & vbCrLf & RelativeFilename(aParameterFileName, lPath) & vbCrLf & _
                       RelativeFilename(aSiteFileName, lPath) & vbCrLf & _
                       RelativeFilename(aInputControlFileName, lPath) & vbCrLf & _
                       RelativeFilename(aOutputControlFileName, lPath) & vbCrLf & _
                       RelativeFilename(aWatershedFileName, lPath) & ";X:" & aWatershedXVarName & ";Y:" & aWatershedYVarName & ";D:" & aWatershedVariableName & vbCrLf & _
                       RelativeFilename(aAggOutputControlFileName, lPath) & vbCrLf & _
                       RelativeFilename(aAggOutputFileName, lPath)
                SaveFileString(lFilename, lStr)
                Return True
            Catch ex As Exception
                Return False
            End Try
        Else
            Return False
        End If
    End Function

    Public Function GetNetCDFVarNames(ByVal aNetCDFFileName As String, ByRef aXVarName As String, _
                                      ByRef aYVarName As String, ByRef aTimeVarName As String, ByRef aDataVarNames As Generic.List(Of String)) As Boolean
        Try
            'Dim lNetCDFFile As New atcTimeseriesNetCDF.atcTimeseriesNetCDF
            'lNetCDFFile.Debug = True
            'lNetCDFFile.Open(aNetCDFFileName)

            Dim lNCId As Int32
            Dim lResult As Int32 = NetCDF.nc_open(aNetCDFFileName, _
                                                  NetCDF.cmode.NC_NOWRITE, _
                                                  lNCId)
            If lResult <> 0 Then
                'lReport.AppendLine("Open Problem " & NetCDF.nc_strerror(lResult))
                Return False
            Else
                Dim lNDims As Int32
                Dim lNVars As Int32
                Dim lNGatts As Int32
                Dim lUnlimdimid As Int32
                NetCDF.nc_inq(lNCId, lNDims, lNVars, lNGatts, lUnlimdimid)

                Dim lLen As Int32
                Dim lDimNames As New ArrayList
                For lDimId As Integer = 0 To lNDims - 1
                    Dim lName As New System.Text.StringBuilder(NetCDF.netCDF_limits.NC_MAX_NAME)
                    NetCDF.nc_inq_dim(lNCId, lDimId, lName, lLen)
                    lDimNames.Add(lName.ToString.ToLower)
                Next

                aDataVarNames = New Generic.List(Of String)
                aXVarName = ""
                aYVarName = ""
                aTimeVarName = ""
                For lVarId As Integer = NetCDF.NC_GLOBAL To lNVars - 1
                    lResult = NetCDF.nc_inq_varndims(lNCId, lVarId, lNDims)
                    If lResult <> 0 Then
                        Return False
                    Else
                        Dim lXtype As NetCDF.nc_type
                        Dim lDimIds(lNDims) As Int32
                        Dim lNAtts As Int32
                        Dim lVarNDims As Int32
                        Dim lName As New System.Text.StringBuilder(NetCDF.netCDF_limits.NC_MAX_NAME)
                        NetCDF.nc_inq_var(lNCId, lVarId, lName, lXtype, lVarNDims, lDimIds, lNAtts)
                        If lDimNames.Contains(lName.ToString.ToLower) Then 'this is a dimension variable
                            Select Case lName.ToString.ToLower
                                Case "longitude", "x"
                                    aYVarName = lName.ToString
                                Case "latitude", "y"
                                    aXVarName = lName.ToString
                                Case "time", "t"
                                    aTimeVarName = lName.ToString
                            End Select
                        ElseIf lVarNDims = lNDims Then 'this is a data variable
                            aDataVarNames.Add(lName.ToString)
                        End If
                    End If
                Next
                lResult = NetCDF.nc_close(lNCId)
            End If
        Catch ex As Exception
            Return False
        End Try

    End Function

End Module
