Imports System.Collections.Generic

''' <summary>Base class for a collection of locations</summary>
''' <remarks></remarks>
Public MustInherit Class PointLocations
    Inherits System.Collections.ObjectModel.KeyedCollection(Of String, PointLocation)
    Protected Overrides Function GetKeyForItem(ByVal aPointLocation As PointLocation) As String
        Return aPointLocation.Key
    End Function

    ''' <summary>Opens specified table and fills locations</summary>
    ''' <param name="aFileName">Name of containing table</param>
    ''' <remarks>Existing external file in current directory used before internal default version of file</remarks>
    Public Sub New(ByVal aFileName As String)
        Dim lPointTable As New atcUtility.atcTableDelimited
        With lPointTable
            .Delimiter = Delimeter
            If IO.File.Exists(aFileName) Then
                MapWinUtility.Logger.Dbg("UsingExternalFile " & aFileName & " in " & IO.Directory.GetCurrentDirectory)
                .OpenFile(aFileName)
            Else
                MapWinUtility.Logger.Dbg("UsingInternalFile " & aFileName)
                Dim lAssembly As System.Reflection.Assembly = System.Reflection.Assembly.GetExecutingAssembly
                Dim lReader As New IO.StreamReader(lAssembly.GetManifestResourceStream("D4EMLite." + aFileName))
                .OpenString(lReader.ReadToEnd())
            End If
            MapWinUtility.Logger.Dbg("  Opened with FieldCount " & .NumFields & " RecordCount " & .NumRecords)
            While Not .EOF
                If .Value(1).ToString.Length > 0 Then
                    AddLocation(lPointTable)
                End If
                .MoveNext()
            End While
        End With
        MapWinUtility.Logger.Dbg("  Built PointLocations with Count " & Count)
    End Sub

    ''' <summary>Fill specific details about point from record in table</summary>
    ''' <param name="aTable">table containing locations</param>
    ''' <remarks>uses current record from table</remarks>
    Public MustOverride Sub AddLocation(ByVal aTable As atcUtility.atcTable)

    ''' <summary>Name of embedded file containing default locations</summary>
    ''' <returns>filename without path</returns>
    ''' <remarks></remarks>
    Protected MustOverride Function InternalFilename() As String

    ''' <summary>Delimter within records in table</summary>
    ''' <returns>record field delimeter</returns>
    ''' <remarks>may be different than original delimeter in table</remarks>
    Protected Overridable Function Delimeter() As String
        Return vbTab
    End Function

    ''' <summary>Determines closest locations to specfied point</summary>
    ''' <param name="aLatitude">Latitude in decimal degrees</param>
    ''' <param name="aLongitude">Longitude in decimal degrees</param>
    ''' <param name="aMaxCount">Maximum number of locations to return</param>
    ''' <returns>Closest locations</returns>
    ''' <remarks>Defauts to five closest locations</remarks>
    Public Function Closest(ByVal aLatitude As Double, ByVal aLongitude As Double, _
                            Optional ByVal aMaxCount As Integer = 5) As SortedList(Of Double, PointLocation)
        Dim lClosestPoints As New SortedList(Of Double, PointLocation)
        For Each lPointLocation As PointLocation In Me
            Dim lCalcDistance As Double = Spatial.GreatCircleDistance(aLongitude, aLatitude, lPointLocation.Longitude, lPointLocation.Latitude)
            Dim lTempDistance As Double = lCalcDistance
            While lClosestPoints.ContainsKey(lTempDistance)
                lTempDistance *= 1.00000001
            End While
            lClosestPoints.Add(lTempDistance, lPointLocation)
        Next
        Dim lClosestPointsToReturn As New SortedList(Of Double, PointLocation)
        Dim lCount As Integer = 0
        For Each lItem As KeyValuePair(Of Double, PointLocation) In lClosestPoints
            lClosestPointsToReturn.Add(lItem.Key, lItem.Value)
            lCount += 1
            If lCount >= aMaxCount Then Exit For
        Next
        Return lClosestPointsToReturn
    End Function
End Class

''' <summary>Base class for a location</summary>
''' <remarks>Contains minimum details</remarks>
Public MustInherit Class PointLocation
    ''' <summary>Table record associated with this point</summary>
    ''' <remarks></remarks>
    Public Record As String
    ''' <summary>Identifier for record</summary>
    ''' <remarks></remarks>
    Public Id As String
    ''' <summary>Latitude in decimal degrees</summary>
    ''' <remarks></remarks>
    Public Latitude As Double
    ''' <summary>Longitude in decimal degrees</summary>
    ''' <remarks></remarks>
    Public Longitude As Double

    ''' <summary></summary>
    ''' <remarks></remarks>
    Public Sub New()
    End Sub

    ''' <summary></summary>
    ''' <param name="aPointTable"></param>
    ''' <param name="aIdField"></param>
    ''' <param name="aLatitudeField"></param>
    ''' <param name="aLongitudeField"></param>
    ''' <param name="aDelimeter"></param>
    ''' <remarks></remarks>
    Public Sub New(ByVal aPointTable As atcUtility.atcTable, _
          Optional ByVal aIdField As Integer = -1, _
          Optional ByVal aLatitudeField As Integer = -1, _
          Optional ByVal aLongitudeField As Integer = -1, _
          Optional ByVal aDelimeter As String = vbTab)
        With aPointTable
            Record = .CurrentRecordAsDelimitedString(aDelimeter)

            Dim lIdField As Integer = aIdField
            If lIdField < 0 Then lIdField = .FieldNumber("id")
            Id = .Value(lIdField)

            Dim lLatitudeField As Integer = aLatitudeField
            If lLatitudeField <= 0 Then lLatitudeField = .FieldNumber("latitude")
            Latitude = Double.Parse(.Value(lLatitudeField).Replace("""", ""))

            Dim lLongitudeField As Integer = aLongitudeField
            If lLongitudeField <= 0 Then lLongitudeField = .FieldNumber("longitude")
            Longitude = Double.Parse(.Value(lLongitudeField).Replace("""", ""))
        End With
    End Sub

    ''' <summary></summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable Function Key() As String
        Return Id
    End Function

    ''' <summary></summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function Description() As String

    ''' <summary></summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function ToString() As String
        Return Description()
    End Function
End Class

''' <summary>Wrapper for distance calculations</summary>
''' <remarks></remarks>
Public Class Spatial
    Private Const DegreesToRadians As Double = 0.01745329252
 
    Public Shared Function GreatCircleDistance(ByVal aLong1 As Double, ByVal aLat1 As Double, ByVal aLong2 As Double, ByVal aLat2 As Double) As Double
        Dim lLat1 As Double = DegreesToRadians * aLat1
        Dim lLat2 As Double = DegreesToRadians * aLat2
        Dim lLong1 As Double = DegreesToRadians * aLong1
        Dim lLong2 As Double = DegreesToRadians * aLong2

        Dim lDistance As Double = 2 * Math.Asin(Math.Sqrt((Math.Sin((lLat1 - lLat2) / 2)) ^ 2 + _
                 Math.Cos(lLat1) * Math.Cos(lLat2) * (Math.Sin((lLong1 - lLong2) / 2)) ^ 2))
        lDistance *= 6366.71 'radians to km
        lDistance *= 1000  'km to m
        Return lDistance
    End Function
End Class

