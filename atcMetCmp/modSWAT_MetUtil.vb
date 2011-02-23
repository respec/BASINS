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

    ''' <summary>
    ''' generates solar radiation 
    ''' </summary>
    ''' <param name="Precip"></param>
    ''' <param name="SolarRadiationAver"></param>
    ''' <remarks>based on SWAT 2005 routine SlrGen</remarks>
    Function ComputeSolarRadiation(ByVal Precip As Double, ByVal SolarRadiationAver As Double)
!!    ~ ~ ~ INCOMING VARIABLES ~ ~ ~
!!    name        |units         |definition
!!    ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ 
!!    hru_rmx(:)  |MJ/m^2        |maximum possible radiation for the day in HRU
!!    j           |none          |HRU number
!!    i_mo        |none          |month being simulated
!!    pr_w(3,:,:) |none          |proportion of wet days in a month
!!    solarav(:,:)|MJ/m^2/day    |average daily solar radiation for the month
!!    subp(:)     |mm H2O        |precipitation for the day in HRU
!!    wgncur(3,:) |none          |parameter which predicts impact of precip on
!!                               |daily solar radiation
!!    ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~

!!    ~ ~ ~ OUTGOING VARIABLES ~ ~ ~
!!    name        |units         |definition
!!    ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~
!!    hru_ra(:)   |MJ/m^2        |solar radiation for the day in HRU
!!    ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~

!!    ~ ~ ~ LOCAL DEFINITIONS ~ ~ ~
!!    name        |units         |definition
!!    ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~
!!    rav         |MJ/m^2        |modified monthly average solar radiation
!!    rx          |none          |variable to hold intermediate calculation
!!    ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~

!!    ~ ~ ~ ~ ~ ~ END SPECIFICATIONS ~ ~ ~ ~ ~ ~

        use(parm)

      integer, intent (in) :: j

real:   rx : , rav


      rav = 0.
      rav = solarav(i_mo,hru_sub(j)) /                                  &
     &                              (1. - 0.5 * pr_w(3,i_mo,hru_sub(j)))
      if (subp(j) > 0.0) rav = 0.5 * rav
            rx = 0.
            rx = hru_rmx(j) - rav
      hru_ra(j) = rav + wgncur(3,j) * rx / 4.
      if (hru_ra(j) <= 0.) hru_ra(j) = .05 * hru_rmx(j)

            Return
            End
End Module

Public Class SwatWeatherStations
    Inherits System.Collections.ObjectModel.KeyedCollection(Of String, SwatWeatherStation)
    Protected Overrides Function GetKeyForItem(ByVal aStation As SwatWeatherStation) As String
        Return aStation.Key
    End Function

    Public Sub New(Optional ByVal aFileName As String = "AllStns.txt")
        Dim lStationTable As New atcUtility.atcTableDelimited
        With lStationTable
            .Delimiter = " "
            If IO.File.Exists(aFileName) Then
                MapWinUtility.Logger.Dbg("UsingExternalFile " & aFileName & " in " & IO.Directory.GetCurrentDirectory)
                .OpenFile(aFileName)
            Else
                MapWinUtility.Logger.Dbg("UsingInternalFile " & aFileName)
                Dim lAssembly As System.Reflection.Assembly = System.Reflection.Assembly.GetExecutingAssembly
                Dim lReader As New IO.StreamReader(lAssembly.GetManifestResourceStream("atcMetCmp." + aFileName))
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
        Dim lFieldValues() As String = Split(aTable.CurrentRecordAsDelimitedString, ",")
        Dim lStation As New SwatWeatherStation
        With lStation
            .State = lFieldValues(0)
            .NameKey = lFieldValues(1)
            .Name = lFieldValues(2).Replace("_", " ")
            .Id = lFieldValues(3)
            .Lat = lFieldValues(4)
            .Lng = lFieldValues(5)
            For lMonthIndex As Integer = 1 To 12
                .AirTempMaxAv(lMonthIndex) = lFieldValues(7 + lMonthIndex)
                .AirTempMinAv(lMonthIndex) = lFieldValues(19 + lMonthIndex)
                .SolarAv(lMonthIndex) = lFieldValues(139 + lMonthIndex)
                .DewpointAv(lMonthIndex) = lFieldValues(151 + lMonthIndex)
                .WindAv(lMonthIndex) = lFieldValues(163 + lMonthIndex)
            Next
        End With
        Me.Add(lStation)
    End Sub
End Class

Public Class SwatWeatherStation
    Public State As String
    Public NameKey As String
    Public Name As String
    Public Id As Integer
    Public Lat As Double
    Public Lng As Double
    Public Elev As Double
    Public PrecDaysPerMonth As Double
    Public PrecAv(12) As Double
    Public AirTempMaxAv(12) As Double '9-20
    Public AirTempMinAv(12) As Double '21-32
    Public Prob_WetAfterDry(12) As Double  '
    Public Prob_WetAfterWet(12) As Double  '
    Public SolarAv(12) As Double '141-152
    Public DewpointAv(12) As Double '153-164
    Public WindAv(12) As Double '165-176

    ''' <summary></summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable Function Key() As String
        Return NameKey
    End Function
End Class

