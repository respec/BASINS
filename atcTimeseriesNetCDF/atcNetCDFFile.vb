Imports atcData
Imports atcUtility
Imports MapWinUtility
Imports System.Collections
Imports System.IO
Imports System.Text
Imports atcTimeseriesNetCDF.NetCDF
Imports atcTimeseriesNetCDF.atcNetCDF

Public Class atcNetCDFFile
    Public Attributes As New atcData.atcDataAttributes
    Public Variables As New List(Of atcNetCDFVariable)
    Public Dimensions As New List(Of atcNetCDFDimension)
    Public EastWestDimension As atcNetCDFDimension = Nothing
    Public NorthSouthDimension As atcNetCDFDimension = Nothing
    Public TimeDimension As atcNetCDFDimension = Nothing
    Public ConstituentDimensions As New List(Of atcNetCDFDimension)

    Public Sub New(ByVal aFileName As String)
        Dim lNcId As Int32
        Dim lNumAttributes As Int32
        Dim lNumVariables As Int32
        Dim lNumDimensions As Int32
        Dim lUnlimitedDimension As Int32
        Dim lDimensionName As New StringBuilder(netCDF_limits.NC_MAX_NAME)

        'Open the netcdf file and read it out.
        Logger.Dbg("openNetCDFFile:" & IO.Path.GetFullPath(aFileName))
        nc(nc_open(aFileName, cmode.NC_NOWRITE, lNcId))
        Try
            nc(nc_inq(lNcId, lNumDimensions, lNumVariables, lNumAttributes, lUnlimitedDimension))

            Logger.Dbg("Read " & lNumDimensions & " dims, " & lNumVariables & " vars, " & _
                lNumAttributes & " global atts, and an unlimited dimension of " & lUnlimitedDimension & ".")

            'Read dimension metadata.
            For lDimensionIndex = 0 To lNumDimensions - 1
                Dim lNewDimension As New atcNetCDFDimension(lNcId, lDimensionIndex)
                Logger.Dbg("dimid: " & lDimensionIndex & " name: " & lNewDimension.Name.ToString & " len: " & lNewDimension.Length)
                Dimensions.Add(lNewDimension)
                Select Case lNewDimension.Name.ToString.ToLower
                    Case "east-west", "xcoord" : EastWestDimension = lNewDimension
                    Case "north-south", "ycoord" : NorthSouthDimension = lNewDimension
                    Case "time" : TimeDimension = lNewDimension
                    Case Else : ConstituentDimensions.Add(lNewDimension)
                End Select
            Next

            'Read global attributes.
            For lAttributeIndex As Integer = 0 To lNumAttributes - 1
                ReadAttribute(lNcId, NC_GLOBAL, lAttributeIndex, Me.Attributes)
            Next

            'Read variables, including their attributes.
            For lVariableIndex = 0 To lNumVariables - 1
                Variables.Add(New atcNetCDFVariable(lNcId, lVariableIndex, Dimensions))
            Next
        Finally
            'Close the netCDF file.
            nc(nc_close(lNcId))
            Logger.Dbg("ClosedNetCDFFile")
        End Try
    End Sub

End Class
