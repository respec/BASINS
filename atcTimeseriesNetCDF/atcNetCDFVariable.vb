Imports atcTimeseriesNetCDF.NetCDF
Imports atcTimeseriesNetCDF.atcNetCDF
Imports MapWinUtility
Imports System.Text

Public Class atcNetCDFVariable

    Public ID As Int32
    Public Name As String

    Public Attributes As New atcData.atcDataAttributes
    Public NetCDFType As nc_type
    Public NumberOfDimensions As Int32

    Public Dimensions As New List(Of atcNetCDFDimension)

    'TODO: replace with atcNetCDFDimension class
    'Public DimensionIDs() As Integer
    'Public DimensionLengths() As Integer
    'Public DimensionNames() As String

    Public Values As Array
    Private Shared pValueDumpMax As Integer = 10

    Public Sub New(ByVal ncid As Int32, ByVal aVariableID As Int32, ByVal aFileDimensions As List(Of atcNetCDFDimension))
        ID = aVariableID
        nc(NetCDF.nc_inq_varndims(ncid, aVariableID, NumberOfDimensions))

        'ReDim DimensionIDs(NumberOfDimensions)
        'ReDim DimensionLengths(NumberOfDimensions)
        'ReDim DimensionNames(NumberOfDimensions)

        Dim lNumAttributes As Int32
        Dim varname As New StringBuilder(NetCDF.netCDF_limits.NC_MAX_NAME)

        Dim DimensionIDs(NumberOfDimensions - 1) As Int32
        nc(NetCDF.nc_inq_var(ncid, aVariableID, varname, NetCDFType, NumberOfDimensions, DimensionIDs, lNumAttributes))
        Name = varname.ToString

        Dim lString As String = "varid: " & Str(aVariableID) & " name: " & Name & " type: " & Str(NetCDFType) & " ndims: " & Str(NumberOfDimensions)

        For lDimIndex As Integer = 0 To NumberOfDimensions - 1
            Dim lDimension As atcNetCDFDimension = aFileDimensions(DimensionIDs(lDimIndex))
            Dimensions.Add(lDimension)
            lString &= " dim(" & DimensionIDs(lDimIndex) & ") '" & lDimension.Name & "' len=" & lDimension.Length
        Next

        Logger.Dbg(lString)

        'Read attributes, if any.
        For lAttributeIndex As Integer = 0 To lNumAttributes - 1
            ReadAttribute(ncid, aVariableID, lAttributeIndex, Attributes)
        Next

        Values = ReadArray(ncid, aVariableID, NetCDFType, TotalDimensionLength)
    End Sub

    Public Overrides Function ToString() As String
        Return Name
    End Function

    Public Function AsGrid(ByVal aFilename As String, Optional ByVal InRam As Boolean = True) As MapWinGIS.Grid
        Dim lGrid As New MapWinGIS.Grid
        Dim lHeader As New MapWinGIS.GridHeader
        Dim lInitialValue As Object = -9999

        Dim lNorthSouthDimension As atcNetCDFDimension = DimensionNamed("north-south")
        Dim lEastWestDimension As atcNetCDFDimension = DimensionNamed("east-west")

        If lNorthSouthDimension Is Nothing Then
            Throw New ApplicationException("Cannot create grid: no north-south dimension")
        ElseIf lEastWestDimension Is Nothing Then
            Throw New ApplicationException("Cannot create grid: no east-west dimension")
        Else
            lHeader.NumberRows = lNorthSouthDimension.Length
            lHeader.NumberCols = lEastWestDimension.Length

            If lGrid.CreateNew(aFilename, lHeader, MapWinGisGridDataType, lInitialValue, InRam) Then
                'lGrid.Value(lColumn, lRow) = 
                Return lGrid
            Else
                Throw New ApplicationException("Unable to create new grid file '" & aFilename & "'")
            End If
        End If
    End Function

    Private Function DimensionNamed(ByVal aName As String) As atcNetCDFDimension
        For Each lDimension As atcNetCDFDimension In Dimensions
            If lDimension.Name.Equals(aName) Then Return lDimension
        Next
        Return Nothing
    End Function

    Private Function MapWinGisGridDataType() As MapWinGIS.GridDataType
        Select Case NetCDFType
            Case nc_type.NC_BYTE : Return MapWinGIS.GridDataType.ByteDataType
            Case nc_type.NC_CHAR : Return MapWinGIS.GridDataType.ByteDataType
            Case nc_type.NC_DOUBLE : Return MapWinGIS.GridDataType.DoubleDataType
            Case nc_type.NC_FLOAT : Return MapWinGIS.GridDataType.FloatDataType
            Case nc_type.NC_INT : Return MapWinGIS.GridDataType.LongDataType
            Case nc_type.NC_SHORT : Return MapWinGIS.GridDataType.ShortDataType
            Case Else : Return MapWinGIS.GridDataType.UnknownDataType
        End Select
    End Function

    Public Shared Function ReadArray(ByVal ncid As Int32, ByVal aVariableID As Int32, ByVal xtype As NetCDF.nc_type, ByVal aDimensionLength As Integer) As Array
        Dim lReturnValues As Array = Nothing
        Select Case xtype
            Case NetCDF.nc_type.NC_BYTE
                Dim lValues(aDimensionLength - 1) As Byte
                nc(NetCDF.nc_get_var_uchar(ncid, aVariableID, lValues))
                lReturnValues = lValues

            Case NetCDF.nc_type.NC_CHAR
                Dim lValue As New StringBuilder(aDimensionLength - 1)
                nc(NetCDF.nc_get_var_text(ncid, aVariableID, lValue))
                lReturnValues = lValue.ToString.ToCharArray

            Case NetCDF.nc_type.NC_DOUBLE
                Dim lValues(aDimensionLength - 1) As Double
                nc(NetCDF.nc_get_var_double(ncid, aVariableID, lValues))
                lReturnValues = lValues

            Case NetCDF.nc_type.NC_INT
                Dim lValues(aDimensionLength - 1) As Integer
                nc(NetCDF.nc_get_var_int(ncid, aVariableID, lValues))
                lReturnValues = lValues

            Case NetCDF.nc_type.NC_FLOAT
                Dim lValues(aDimensionLength - 1) As Single
                nc(NetCDF.nc_get_var_float(ncid, aVariableID, lValues))
                lReturnValues = lValues

            Case NetCDF.nc_type.NC_SHORT
                Dim lValues(aDimensionLength - 1) As Short
                nc(NetCDF.nc_get_var_short(ncid, aVariableID, lValues))
                lReturnValues = lValues
        End Select
        If lReturnValues IsNot Nothing Then
            Dim lValueDumpCount As Integer = Math.Min(pValueDumpMax, aDimensionLength - 1)
            For lValueIndex = 0 To lValueDumpCount
                Logger.Dbg("value (" & lValueIndex & ") = " & Str(lReturnValues(lValueIndex)))
            Next
        End If
        Return lReturnValues
    End Function

    Private Function TotalDimensionLength() As Integer
        Dim lTotal As Integer = 1
        For Each lDimension As atcNetCDFDimension In Dimensions
            If lDimension.Length > 0 Then lTotal *= lDimension.Length
        Next
        Return lTotal
    End Function

End Class
