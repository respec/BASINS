''' <summary>
''' Excerpted and translated routines from SWAT 
''' </summary>
''' <remarks>function and subroutine names are taken from SWAT, variable names updated to meet BASINS conventions</remarks>
Module modSWAT_MetUtil
    ''' <summary>
    ''' Generate a random numbers ranging from 0.0 to 1.0.
    ''' In the process of calculating the random number, the seed (x1) is set to a new value.
    ''' This function implements the prime-modulus generator xi = 16807 xi Mod(2**(31) - 1)
    ''' using code which ensures that no intermediate result uses more than 31 bits
    ''' </summary>
    ''' <param name="aX">random number generator seed (integer) where 0 less than x1 less than 2147483647</param>
    ''' <returns>random number between 0.0 and 1.0</returns>
    ''' <remarks>
    ''' the theory behind the code is summarized in Bratley, P., B.L. Fox and L.E. Schrage. 1983. A Guide to Simulation.
    ''' Springer-Verlag, New York. (pages 199-202)
    ''' </remarks>
    Function Aunif(ByRef aX As Integer) As Single
        Dim lX As Integer = aX / 127773
        aX = 16807 * (aX - lX * 127773) - lX * 2836
        If (aX < 0) Then aX += 2147483647
        Return aX * 0.0000000004656612875
    End Function

    ''' <summary>
    ''' Generate a random number from a triangular distribution
    ''' given X axis points at start, end, and peak Y value
    ''' </summary>
    ''' <param name="aT1">lower limit for distribution</param>
    ''' <param name="aT2">monthly mean for distribution</param>
    ''' <param name="aT3">upper limit for distribution</param>
    ''' <param name="aT4">random number seed </param>
    ''' <returns>daily value generated for distribution</returns>
    ''' <remarks></remarks>
    Function Atri(ByVal aT1 As Single, ByVal aT2 As Single, ByVal aT3 As Single, ByRef aT4 As Integer) As Single
        Dim lU3 As Single = aT2 - aT1
        Dim lRn As Single = Aunif(aT4)
        Dim lY As Single = 2.0 / (aT3 - aT1)
        Dim lB2 As Single = aT3 - aT2
        Dim lB1 As Single = lRn / lY
        Dim lX1 As Single = lY * lU3 / 2.0

        Dim lXx As Single
        Dim lYy As Single
        Dim lAtri As Single
        If (lRn <= lX1) Then
            lXx = 2.0 * lB1 * lU3
            If (lXx <= 0) Then
                lYy = 0
            Else
                lYy = Math.Sqrt(lXx)
            End If
            lAtri = lYy + aT1
        Else
            lXx = lB2 * lB2 - 2.0 * lB2 * (lB1 - 0.5 * lU3)
            If (lXx <= 0) Then
                lYy = 0
            Else
                lYy = Math.Sqrt(lXx)
            End If
            lAtri = aT3 - lYy
        End If

        Dim lAmn As Single = (aT3 + aT2 + aT1) / 3.0
        lAtri = lAtri * aT2 / lAmn

        If (lAtri >= 1.0) Then
            lAtri = 0.99
        ElseIf (lAtri <= 0.0) Then
            lAtri = 0.001
        End If
        Return lAtri
    End Function

    Sub Rdwgn()

    End Sub

    Public Class SwatWeatherStations
        Inherits System.Collections.ObjectModel.KeyedCollection(Of String, SwatWeatherStation)
        Protected Overrides Function GetKeyForItem(ByVal aStationLocation As SwatWeatherStation) As String
            Return aStationLocation.Key
        End Function

        Public Sub New(Optional ByVal aFileName As String = "AllStns.txt")
            Dim lStationTable As New atcUtility.atcTableDelimited
            With lStationTable
                .Delimiter = " """
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
                        AddStation(lStationTable)
                    End If
                    .MoveNext()
                End While
            End With
            MapWinUtility.Logger.Dbg("  Built PointLocations with Count " & Count)
        End Sub

        ''' <summary>Fill specific details about point from record in table</summary>
        ''' <param name="aTable">table containing locations</param>
        ''' <remarks>uses current record from table</remarks>
        Public Sub AddStation(ByVal aTable As atcUtility.atcTable)

        End Sub
    End Class

    Public Class SwatWeatherStation
        Public Name As String

        ''' <summary></summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Function Key() As String
            Return Name
        End Function
    End Class
End Module
