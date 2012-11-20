Module netCDFViewer
    Sub main()
        Dim lDebug As Boolean = False

        If IO.File.Exists("netcdf.dll") Then IO.File.Delete("netcdf.dll")
        'IO.File.Copy("netcdf4.2.1.1.dll", "netcdf.dll")
        IO.File.Copy("netcdf3.6.dll", "netcdf.dll")

        Dim lReport As New System.Text.StringBuilder

        Dim lNetCDF_Version As String = NetCDF.nc_inq_libvers
        Dim lPathName As String = "..\..\..\..\Data\"
        Dim lOutFolder As String = lPathName & lNetCDF_Version.Substring(0, 3) & ".dump\"
        If Not IO.Directory.Exists(lOutFolder) Then IO.Directory.CreateDirectory(lOutFolder)

        Dim lBaseNames() As String = {"aspect", "ccgridfile", "hcanfile", "lafile", "lat", "longitude", "slope", "SubType", "Ta1", "Watershed"}
        For Each lBaseName As String In lBaseNames
            If lDebug Then lReport.AppendLine(lNetCDF_Version)
            Dim lFileName As String = lPathName & lBaseName & ".nc"
            lReport.AppendLine("File '" & lFileName & "'")

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
                                    Dim lValuesDouble(lArraySize - 1) As Double
                                    Dim lValuesFloat(lArraySize - 1) As Single
                                    Dim lValuesInt(lArraySize - 1) As Int32
                                    Select Case lXtype
                                        Case NetCDF.nc_type.NC_DOUBLE
                                            lResult = NetCDF.nc_get_vara_double(lNCId, lVarId, lBase, lCount, lValuesDouble)
                                        Case NetCDF.nc_type.NC_FLOAT
                                            lResult = NetCDF.nc_get_vara_float(lNCId, lVarId, lBase, lCount, lValuesFloat)
                                        Case NetCDF.nc_type.NC_INT
                                            lResult = NetCDF.nc_get_vara_int(lNCId, lVarId, lBase, lCount, lValuesInt)
                                    End Select
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
                                        For lValueIndex As Integer = 0 To lArraySize - 1
                                            Dim lString As String = "    (" & Format(lValueIndex, lFormatValueIndex) & "-"
                                            For lDimIndex As Integer = 0 To lNDims - 1
                                                Dim lDimPos As Integer = lBase(lDimIndex) + lOutputPosition(lDimIndex)
                                                lString &= Format(lDimPos, lFormatDimIndex(lDimIndex)) & ":"
                                            Next
                                            lString = lString.Remove(lString.Length - 1) & ") "
                                            Select Case lXtype
                                                Case NetCDF.nc_type.NC_DOUBLE
                                                    lReport.AppendLine(lString & lValuesDouble(lValueIndex))
                                                Case NetCDF.nc_type.NC_FLOAT
                                                    Dim lValue As Double = lValuesFloat(lValueIndex) * 10800 'sec/3hr TODO: make generic!
                                                    lReport.AppendLine(lString & Format(lValue, "##0.000"))
                                            End Select
                                            Dim lOutIndex As Integer = lNDims - 1
                                            lOutputPosition(lOutIndex) += 1
                                            While lOutIndex > 0 AndAlso lOutputPosition(lOutIndex) = lCount(lOutIndex)
                                                lOutputPosition(lOutIndex - 1) += 1
                                                lOutputPosition(lOutIndex) = 0
                                                lOutIndex -= 1
                                            End While
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
