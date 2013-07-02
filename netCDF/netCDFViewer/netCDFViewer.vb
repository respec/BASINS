Imports atcTimeseriesNetCDF

Module netCDFViewer
    Const pMaxDataWrite As Integer = 20000

    Sub main()
        Dim lDebug As Boolean = False

        If IO.File.Exists("netcdf.dll") Then IO.File.Delete("netcdf.dll")
        IO.File.Copy("netcdf4.2.1.1.dll", "netcdf.dll")
        'IO.File.Copy("netcdf3.6.dll", "netcdf.dll")

        Dim lReport As New System.Text.StringBuilder

        'learn how to calc statistics
        Dim lStatistics As New atcTimeseriesStatistics.atcTimeseriesStatistics
        lStatistics.Initialize()

        Dim lNetCDF_Version As String = NetCDF.nc_inq_libvers
        Dim lPathName As String = "..\..\..\..\Data\"
        Dim lOutFolder As String = lPathName & lNetCDF_Version.Substring(0, 3) & ".dump\"
        If Not IO.Directory.Exists(lOutFolder) Then IO.Directory.CreateDirectory(lOutFolder)

        Dim lBaseNames() As String = {"merra.rfe.90m", "rfe90m200309", "SUBWatersheds_LangtangKhola", "merra.rfe.90m.200301", _
                                      "swit", "Ta1", "LangtangKholaWatershed", "srtm_54_07_LangtangFill_LambertWatershed", "srtm_54_07_LangtangFill_LambertUEBAspect", "srtm_54_07_LangtangFill_LambertUEBSlope"}
        '{ "swit", "merra.prod.assim.20061230", "merra.prod.rad.20061231", "aspect", "ccgridfile", "hcanfile", "lafile", "lat", "longitude", "slope", "SubType", "Watershed"}
        For Each lBaseName As String In lBaseNames
            If lDebug Then lReport.AppendLine(lNetCDF_Version)
            Dim lFileName As String = lPathName & lBaseName & ".nc"

            If lFileName.Contains("merra") Then
                lFileName = IO.Path.ChangeExtension(lFileName, ".dat")
            End If
            lReport.AppendLine("File '" & lFileName & "'")

            Dim lAttributes As New atcData.atcDataAttributes
            If lFileName.Contains("merra") OrElse lFileName.Contains("rfe90m") Then
                lAttributes.Add("AggregateGrid", lPathName & "SUBWatersheds_LangtangKhola.nc")
            End If

            Dim lNetCDFFile As New atcTimeseriesNetCDF.atcTimeseriesNetCDF
            lNetCDFFile.Debug = True
            If Not lNetCDFFile.Open(lFileName, lAttributes) Then
                If lFileName.Contains("swit") Then
                    'Point C
                    lAttributes.Add("YIndex", 246) 'col, y, latitude 28.18167
                    lAttributes.Add("XIndex", 100) 'row, x, longitude 85.59833
                    lNetCDFFile.Open(lFileName, lAttributes)
                    'Point E
                    lAttributes.Clear()
                    lAttributes.Add("YIndex", 170) 'col, y, latitude 28.24500
                    lAttributes.Add("XIndex", 270) 'row, x, longitude 85.74000
                    lNetCDFFile.Open(lFileName, lAttributes)
                    'Point B
                    lAttributes.Clear()
                    lAttributes.Add("YIndex", 230) 'col, y, latitude 28.19500
                    lAttributes.Add("XIndex", 100) 'row, x, longitude 85.59833
                    lNetCDFFile.Open(lFileName, lAttributes)
                End If
            End If
            For Each lTimeseries As atcData.atcTimeseries In lNetCDFFile.DataSets
                If lTimeseries.Attributes.GetValue("Location") <> "ID_128" Then 'skip the no data points
                    lTimeseries.Attributes.CalculateAll()
                    Dim lDataTree As New atcDataTree.atcDataTreePlugin
                    'Dim lDataTreeFileName As String = IO.Path.ChangeExtension(lFileName.Replace(".nc", "#.nc"), "list")
                    Dim lDataTreeFileName As String = IO.Path.GetDirectoryName(lFileName) & "\" & IO.Path.GetFileNameWithoutExtension(lTimeseries.Attributes.GetValue("Data Source")) & "_" & lTimeseries.Attributes.GetValue("Constituent") & "#.list"
                    Dim lXYString As String = "_Y" & (lTimeseries.Attributes.GetValue("Y Index")) & "_X" & (lTimeseries.Attributes.GetValue("X Index"))
                    If lXYString = "_Y_X" Then
                        lXYString = "_" & lTimeseries.Attributes.GetValue("Location")
                    End If
                    lDataTree.Save(New atcData.atcTimeseriesGroup(lTimeseries), lDataTreeFileName.Replace("#", lXYString), "Display 25")
                End If
            Next

            Dim lNCId As Int32
            Dim lResult As Int32 = NetCDF.nc_open(lFileName, _
                                                  NetCDF.cmode.NC_NOWRITE, _
                                                  lNCId)
            If lResult <> 0 Then
                lReport.AppendLine("Open Problem " & NetCDF.nc_strerror(lResult))
            Else
                If lDebug Then lReport.AppendLine("Open OK, ID: " & lNCId)
                Dim lNDims As Int32
                Dim lNVars As Int32
                Dim lNGatts As Int32
                Dim lUnlimdimid As Int32
                NetCDF.nc_inq(lNCId, lNDims, lNVars, lNGatts, lUnlimdimid)
                lReport.AppendLine("NDims " & lNDims & " NVars " & lNVars & " NGatts " & lNGatts & " Unlimdim id " & lUnlimdimid)

                Dim lLen As Int32
                For lDimId As Integer = 0 To lNDims - 1
                    Dim lName As New System.Text.StringBuilder(NetCDF.netCDF_limits.NC_MAX_NAME)
                    NetCDF.nc_inq_dim(lNCId, lDimId, lName, lLen)
                    lReport.AppendLine("  DimId " & lDimId & " Name '" & lName.ToString & "' Len " & lLen)
                Next

                For lVarId As Integer = NetCDF.NC_GLOBAL To lNVars - 1 'start with global at -1
                    If lVarId = -1 Then
                        lReport.AppendLine("Global")
                        lReport.Append(AttsToString(lNCId, lVarId))
                    Else
                        lResult = NetCDF.nc_inq_varndims(lNCId, lVarId, lNDims)
                        If lResult <> 0 Then
                            lReport.AppendLine(ErrorString(lResult, lNCId, lVarId))
                        Else
                            Dim lXtype As NetCDF.nc_type
                            Dim lDimIds(lNDims) As Int32
                            Dim lNAtts As Int32
                            Dim lName As New System.Text.StringBuilder(NetCDF.netCDF_limits.NC_MAX_NAME)
                            NetCDF.nc_inq_var(lNCId, lVarId, lName, lXtype, lNDims, lDimIds, lNAtts)
                            lReport.Append("VarId " & lVarId & " Name '" & lName.ToString & _
                                               "' Type " & lXtype & " NAtts " & lNAtts & _
                                               " NDims " & lNDims)
                            Dim lDimString As String = ""
                            Dim lDimLen As Integer
                            Dim lCount() As Integer
                            If lNDims > 0 Then
                                ReDim lCount(lNDims - 1)
                                lDimString = " Dims '"
                                For lDimIndex As Integer = 0 To lNDims - 1
                                    lResult = NetCDF.nc_inq_dimlen(lNCId, lDimIds(lDimIndex), lDimLen)
                                    lCount(lDimIndex) = lDimLen
                                    lDimString &= Str(lDimIds(lDimIndex)) & ":" & lDimLen & ", "
                                Next
                                lDimString = lDimString.Remove(lDimString.Length - 2)
                                lDimString &= "'"
                                lReport.AppendLine(lDimString)
                                lReport.Append(AttsToString(lNCId, lVarId))
                                If lDimLen > 0 Then
                                    Dim lBase(lNDims) As Integer

                                    Dim lArraySize As Integer = 1
                                    For lDimIndex As Integer = 0 To lNDims - 1
                                        lArraySize *= lCount(lDimIndex)
                                    Next
                                    Dim lArraySubset As Boolean = False
                                    Dim lArraySizeMax As Integer = 100000
                                    If lArraySize > lArraySizeMax Then
                                        lReport.Append("ArraySize " & lArraySize & " reset to " & lArraySizeMax)
                                        lArraySize = lArraySizeMax
                                        lArraySubset = True
                                    End If
                                    Dim lValuesDouble(lArraySize - 1) As Double
                                    Dim lValuesFloat(lArraySize - 1) As Single
                                    Dim lValuesInt(lArraySize - 1) As Int32
                                    Dim lValuesShort(lArraySize - 1) As Int16
                                    Dim lValuesByte(lArraySize - 1) As Byte
                                    If Not lArraySubset Then
                                        Select Case lXtype
                                            Case NetCDF.nc_type.NC_DOUBLE
                                                lResult = NetCDF.nc_get_vara_double(lNCId, lVarId, lBase, lCount, lValuesDouble)
                                            Case NetCDF.nc_type.NC_FLOAT
                                                lResult = NetCDF.nc_get_vara_float(lNCId, lVarId, lBase, lCount, lValuesFloat)
                                            Case NetCDF.nc_type.NC_SHORT
                                                lResult = NetCDF.nc_get_vara_short(lNCId, lVarId, lBase, lCount, lValuesShort)
                                            Case NetCDF.nc_type.NC_INT
                                                lResult = NetCDF.nc_get_vara_int(lNCId, lVarId, lBase, lCount, lValuesInt)
                                            Case Else
                                                lResult = "<unknown type " & lXtype & ">"
                                        End Select
                                    Else
                                        'TODO: do something other that first value if a timeseries with X and Y set is available
                                        For lDimIndex As Integer = 0 To lNDims - 1
                                            If lDimIndex < lNDims - 1 Then
                                                lCount(lDimIndex) = 1
                                            End If
                                        Next
                                        Select Case lXtype
                                            Case NetCDF.nc_type.NC_DOUBLE
                                                lResult = NetCDF.nc_get_vara_double(lNCId, lVarId, lBase, lCount, lValuesDouble)
                                            Case NetCDF.nc_type.NC_FLOAT
                                                lResult = NetCDF.nc_get_vara_float(lNCId, lVarId, lBase, lCount, lValuesFloat)
                                            Case NetCDF.nc_type.NC_SHORT
                                                lResult = NetCDF.nc_get_vara_short(lNCId, lVarId, lBase, lCount, lValuesShort)
                                            Case NetCDF.nc_type.NC_INT
                                                lResult = NetCDF.nc_get_vara_int(lNCId, lVarId, lBase, lCount, lValuesInt)
                                            Case NetCDF.nc_type.NC_BYTE
                                                lResult = NetCDF.nc_get_vara_uchar(lNCId, lVarId, lBase, lCount, lValuesByte)
                                            Case Else
                                                lResult = "<unknown type " & lXtype & ">"
                                        End Select
                                    End If
                                    If lResult <> 0 Then
                                        lReport.AppendLine(ErrorString(lResult, lNCId, lVarId))
                                    Else
                                        Dim lOutputPosition() = {0, 0, 0, 0, 0}

                                        Dim lFormatValueIndex As String = "0"
                                        If lArraySize > 1 Then
                                            lFormatValueIndex = StrDup(CInt(Math.Ceiling(Math.Log10(lArraySize - 1))), "0")
                                        End If
                                        Dim lFormatDimIndex(lNDims)
                                        For lDimIndex As Integer = 0 To lNDims - 1
                                            Dim lDimPosMax As Integer = lBase(lDimIndex) + lCount(lDimIndex) - 1
                                            If lDimPosMax > 0 Then
                                                lFormatDimIndex(lDimIndex) = StrDup(CInt(Math.Ceiling(Math.Log10(lDimPosMax))), "0")
                                            Else
                                                lFormatDimIndex(lDimIndex) = "0"
                                            End If
                                        Next

                                        Dim lMissingCount As Integer = 0
                                        Dim lZeroCount As Integer = 0
                                        Dim lGoodCount As Integer = 0
                                        For lValueIndex As Integer = 0 To lArraySize - 1
                                            Dim lString As String = "    (" & Format(lValueIndex, lFormatValueIndex) & ":"
                                            For lDimIndex As Integer = 0 To lNDims - 1
                                                Dim lDimPos As Integer = lBase(lDimIndex) + lOutputPosition(lDimIndex)
                                                lString &= Format(lDimPos, lFormatDimIndex(lDimIndex)) & ","
                                            Next
                                            lString = lString.Remove(lString.Length - 1) & ") "
                                            Select Case lXtype
                                                Case NetCDF.nc_type.NC_DOUBLE
                                                    lReport.AppendLine(lString & lValuesDouble(lValueIndex))
                                                Case NetCDF.nc_type.NC_FLOAT, NetCDF.nc_type.NC_INT, NetCDF.nc_type.NC_SHORT
                                                    Dim lValue As Double
                                                    If lXtype = NetCDF.nc_type.NC_INT Then
                                                        lValue = lValuesInt(lValueIndex)
                                                    ElseIf lXtype = NetCDF.nc_type.NC_SHORT Then
                                                        lValue = lValuesShort(lValueIndex)
                                                    Else
                                                        lValue = lValuesFloat(lValueIndex) ' * 10800 'sec/3hr TODO: make generic!
                                                    End If
                                                    If lValue = -9999 OrElse lValue < -2000000000.0 Then 'missing
                                                        If lZeroCount > 0 Then
                                                            lReport.AppendLine("    ... " & lZeroCount & " zero values skipped")
                                                            lZeroCount = 0
                                                        End If
                                                        lMissingCount += 1
                                                    Else
                                                        If lMissingCount > 0 Then
                                                            lReport.AppendLine("    ... " & lMissingCount & " missing values skipped")
                                                            lMissingCount = 0
                                                        End If
                                                        If lBaseName <> "swit" OrElse lValue <> 0 Then
                                                            If lZeroCount > 0 Then
                                                                lReport.AppendLine("    ... " & lZeroCount & " zero values skipped")
                                                                lZeroCount = 0
                                                            End If
                                                            lGoodCount += 1
                                                            If Math.Abs(lValue) > 0.000001 Then
                                                                lReport.AppendLine(lString & Format(lValue, "##0.000000"))
                                                            Else
                                                                lReport.AppendLine(lString & lValue)
                                                            End If
                                                        Else
                                                            lZeroCount += 1
                                                        End If
                                                    End If
                                                Case Else
                                                    lReport.AppendLine(lString & "    <unknown type " & lXtype & ">")
                                                    Exit For
                                            End Select
                                            Dim lOutIndex As Integer = lNDims - 1
                                            lOutputPosition(lOutIndex) += 1
                                            While lOutIndex > 0 AndAlso lOutputPosition(lOutIndex) = lCount(lOutIndex)
                                                lOutputPosition(lOutIndex - 1) += 1
                                                lOutputPosition(lOutIndex) = 0
                                                lOutIndex -= 1
                                            End While
                                            If lGoodCount >= pMaxDataWrite Then
                                                lReport.AppendLine("    ... " & lArraySize - lValueIndex & " values skipped")
                                                Exit For
                                            End If
                                        Next
                                    End If
                                End If
                            End If
                        End If
                    End If
                Next
                lResult = NetCDF.nc_close(lNCId)
            End If
            IO.File.WriteAllText(lOutFolder & lBaseName & ".log", lReport.ToString)
            lReport.Length = 0
        Next
    End Sub

    Function AttsToString(ByVal aNcid As Int32, ByVal aVarId As Int32) As String
        Dim lNAtts As Int32
        Dim lResult As Integer = NetCDF.nc_inq_varnatts(aNcid, aVarId, lNAtts)
        If lResult <> 0 Then
            Return ("nc_inq_varnatts Problem " & ErrorString(lResult, aNcid, aVarId))
        Else
            Dim lName As New System.Text.StringBuilder(NetCDF.netCDF_limits.NC_MAX_NAME)
            Dim lAttsSB As New System.Text.StringBuilder
            For lAttId As Integer = 0 To lNAtts - 1
                lResult = NetCDF.nc_inq_attname(aNcid, aVarId, lAttId, lName)
                If lResult <> 0 Then
                    lAttsSB.AppendLine("nc_inq_attname Problem " & ErrorString(lResult, aNcid, aVarId))
                Else
                    Dim lLen As Int32
                    Dim lNameString As String = lName.ToString
                    lResult = NetCDF.nc_inq_attlen(aNcid, aVarId, lNameString, lLen)
                    If lResult <> 0 Then
                        lAttsSB.AppendLine("nc_inq_attid Problem " & ErrorString(lResult, aNcid, aVarId, lName.ToString))
                    ElseIf lLen = 0 Then
                        lAttsSB.AppendLine("length Problem AttId " & lAttId & " Name '" & lName.ToString & "'")
                    Else
                        Dim lXtype As NetCDF.nc_type
                        lResult = NetCDF.nc_inq_atttype(aNcid, aVarId, lNameString, lXtype, lLen)
                        If lResult <> 0 Then
                            lAttsSB.AppendLine("nc_inq_atttype Problem " & NetCDF.nc_strerror(lResult) & lAttId & " Name '" & lNameString & "'")
                        Else
                            Dim lAttString As String = ""
                            Select Case lXtype
                                Case NetCDF.nc_type.NC_CHAR
                                    Dim lAttText As New System.Text.StringBuilder(lLen - 1)
                                    NetCDF.nc_get_att_text(aNcid, aVarId, lNameString, lAttText)
                                    lAttString = lAttText.ToString
                                    If lAttString.Length > lLen Then
                                        lAttString = lAttString.Remove(lLen)
                                    End If
                                Case NetCDF.nc_type.NC_BYTE
                                    Dim lAttbyte(lLen - 1) As Byte
                                    NetCDF.nc_get_att_uchar(aNcid, aVarId, lNameString, lAttbyte)
                                    For lIndex As Integer = 0 To lLen - 1
                                        lAttString &= Str(lAttbyte(lIndex))
                                    Next
                                Case NetCDF.nc_type.NC_SHORT
                                    Dim lAttint(lLen - 1) As Int16
                                    NetCDF.nc_get_att_short(aNcid, aVarId, lNameString, lAttint)
                                    For lindex As Integer = 0 To lLen - 1
                                        lAttString &= Str(lAttint(lindex)) & ", "
                                    Next
                                    lAttString = lAttString.Remove(lAttString.Length - 2)
                                Case NetCDF.nc_type.NC_INT
                                    Dim lAttint(lLen - 1) As Int32
                                    NetCDF.nc_get_att_int(aNcid, aVarId, lNameString, lAttint)
                                    For lindex As Integer = 0 To lLen - 1
                                        lAttString &= Str(lAttint(lindex)) & ", "
                                    Next
                                    lAttString = lAttString.Remove(lAttString.Length - 2)
                                Case NetCDF.nc_type.NC_FLOAT
                                    Dim lAttSingle(lLen - 1) As Single
                                    NetCDF.nc_get_att_float(aNcid, aVarId, lNameString, lAttSingle)
                                    For lindex As Integer = 0 To lLen - 1
                                        lAttString &= Str(lAttSingle(lindex)) & ", "
                                    Next
                                    lAttString = lAttString.Remove(lAttString.Length - 2)
                                Case NetCDF.nc_type.NC_DOUBLE
                                    Dim lAttDouble(lLen - 1) As Double
                                    NetCDF.nc_get_att_double(aNcid, aVarId, lNameString, lAttDouble)
                                    For lindex As Integer = 0 To lLen - 1
                                        lAttString &= Str(lAttDouble(lindex)) & ", "
                                    Next
                                    lAttString = lAttString.Remove(lAttString.Length - 2)

                                Case Else
                                    lAttString = "UnknownType " & lXtype
                            End Select
                            lAttsSB.AppendLine("  AttId " & lAttId & " Name '" & lNameString & "' Type " & lXtype & " Len " & lLen & " Value '" & lAttString & "'")
                        End If
                    End If
                End If
            Next
            Return lAttsSB.ToString
        End If
    End Function

    Private Function ErrorString(ByVal aResult As Int32, ByVal aNcid As Int32, ByVal aVarId As Int32) As String
        Return ("Error " & aResult & " " & NetCDF.nc_strerror(aResult) & " (" & aNcid & "," & aVarId & ")")
    End Function

    Private Function ErrorString(ByVal aResult As Int32, ByVal aNcid As Int32, ByVal aVarId As Int32, ByVal aName As String) As String
        Return ("Error " & aResult & " " & NetCDF.nc_strerror(aResult) & " (" & aNcid & "," & aVarId & ",'" & aName & "')")
    End Function
End Module
