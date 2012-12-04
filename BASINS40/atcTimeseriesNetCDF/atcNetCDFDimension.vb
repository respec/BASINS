Imports System.Text
Imports atcTimeseriesNetCDF.NetCDF
Imports atcTimeseriesNetCDF.atcNetCDF

Public Class atcNetCDFDimension
    Public ID As Integer
    Public Name As String
    Public Length As Integer

    Public Sub New(ByVal aID As Integer, ByVal aName As String, ByVal aLength As Integer)
        ID = aID
        Name = aName
        Length = aLength
    End Sub

    Public Sub New(ByVal ncid As Integer, ByVal aDimensionID As Integer)
        ID = aDimensionID
        Dim lDimensionName As New StringBuilder(netCDF_limits.NC_MAX_NAME)
        nc(nc_inq_dim(ncid, aDimensionID, lDimensionName, Length))
        Name = lDimensionName.ToString
    End Sub

End Class
