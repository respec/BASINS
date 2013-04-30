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
    Public ConstituentVariables As New List(Of atcNetCDFVariable)
    Public Dimensions As New List(Of atcNetCDFDimension)
    Public EastWestDimension As atcNetCDFDimension = Nothing
    Public NorthSouthDimension As atcNetCDFDimension = Nothing
    Public TimeDimension As atcNetCDFDimension = Nothing
    Public ConstituentDimensions As New List(Of atcNetCDFDimension)

    Private pNcID As Int32 = -1
    Public ReadOnly FileName As String

    Public Sub New(ByVal aFileName As String)
        Dim lNumAttributes As Int32
        Dim lNumVariables As Int32
        Dim lNumDimensions As Int32
        Dim lUnlimitedDimension As Int32
        Dim lDimensionName As New StringBuilder(netCDF_limits.NC_MAX_NAME)

        'Open the netcdf file and read it out.
        FileName = IO.Path.GetFullPath(aFileName)
        Logger.Dbg("openNetCDFFile:" & FileName)
        Try
            pNcID = NcID
            nc(nc_inq(pNcID, lNumDimensions, lNumVariables, lNumAttributes, lUnlimitedDimension))

            Logger.Dbg("Read " & lNumDimensions & " dims, " & lNumVariables & " vars, " & _
                lNumAttributes & " global atts, and an unlimited dimension of " & lUnlimitedDimension & ".")

            'Read dimension metadata.
            For lDimensionIndex = 0 To lNumDimensions - 1
                Dim lNewDimension As New atcNetCDFDimension(pNcID, lDimensionIndex)
                Logger.Dbg("dimid: " & lDimensionIndex & " name: " & lNewDimension.Name.ToString & " len: " & lNewDimension.Length)
                Dimensions.Add(lNewDimension)
                Select Case lNewDimension.Name.ToString.ToLower
                    Case "east-west", "xcoord", "longitude", "lon" : EastWestDimension = lNewDimension
                    Case "north-south", "ycoord", "latitude", "lat" : NorthSouthDimension = lNewDimension
                    Case "time" : TimeDimension = lNewDimension
                    Case Else : ConstituentDimensions.Add(lNewDimension)
                End Select
            Next

            'Read global attributes.
            For lAttributeIndex As Integer = 0 To lNumAttributes - 1
                ReadAttribute(pNcID, NC_GLOBAL, lAttributeIndex, Me.Attributes)
            Next

            'Read variables, including their attributes.
            For lVariableIndex = 0 To lNumVariables - 1
                Dim lNewVariable As New atcNetCDFVariable(Me, lVariableIndex, Dimensions)
                Variables.Add(lNewVariable)
                If lNewVariable.ID <> EastWestDimension.ID AndAlso _
                   lNewVariable.ID <> NorthSouthDimension.ID AndAlso _
                   (TimeDimension Is Nothing OrElse lNewVariable.ID <> TimeDimension.ID) Then
                    ConstituentVariables.Add(lNewVariable)
                End If
            Next
        Finally
            Logger.Dbg("Close netCDF file " & pNcID & " " & FileName)
            nc(nc_close(pNcID))
            pNcID = -1
            Logger.Dbg("ClosedNetCDFFile")
        End Try
    End Sub

    Friend ReadOnly Property NcID() As Int32
        Get
            If pNcID = -1 Then
                nc(nc_open(FileName, cmode.NC_NOWRITE, pNcID))
            End If
            Return pNcID
        End Get
    End Property
End Class
