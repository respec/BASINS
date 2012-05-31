Imports atcData
Imports atcUtility
Imports MapWinUtility.Strings

Public Class clsUEBWeather

    Public FileName As String
    Public SJDate As Double
    Public TimeStep As Integer
    Public NSkip As Integer
    Public InitialEnergy As Double
    Public InitialH2OEquiv As Double
    Public InitialSnowAge As Double
    Public ATempTS As atcTimeseries
    Public PrecipTS As atcTimeseries
    Public WindTS As atcTimeseries
    Public RelHTS As atcTimeseries
    Public ATempRngTS As atcTimeseries
    Public ShortRadTS As atcTimeseries
    Public NetRadTS As atcTimeseries

    Public Sub ReadWeatherFile()

        Dim lSR As New IO.StreamReader(FileName)
        Dim lStr As String
        Dim lCnt As Integer = 9
        Dim lDate(5) As Integer
        Dim lCurDate As Double
        Dim lStrArray(7) As String
        Dim lChrSep() As String = {" ", vbTab, vbCrLf}

        lStr = lSR.ReadLine
        lStrArray = lStr.Split(lChrSep, lCnt, StringSplitOptions.None)
        lDate(1) = CInt(lStrArray(0))
        lDate(2) = CInt(lStrArray(1))
        lDate(0) = CInt(lStrArray(2))
        If lDate(0) < 100 Then
            lDate(0) += 1900
        End If
        lDate(3) = CInt(lStrArray(3))
        SJDate = Date2J(lDate)
        TimeStep = CInt(lStrArray(4))
        InitialEnergy = CDbl(lStrArray(5))
        InitialH2OEquiv = CDbl(lStrArray(6))
        InitialSnowAge = CDbl(lStrArray(7))

        lCurDate = SJDate
        lStr = lSR.ReadLine
        lStrArray = lStr.Split(lChrSep, lCnt, StringSplitOptions.None)
        NSkip = lStrArray(0)
        If NSkip > 0 Then
            For i As Integer = 1 To NSkip
                lStr = lSR.ReadLine
            Next
            lCurDate += TimeStep * NSkip / 24
        End If

        'TODO: read weather data into TSers

    End Sub

End Class
