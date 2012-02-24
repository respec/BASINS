Imports atcUtility
Imports atcData

Public Class WREGStation
    Public StationID As String = ""
    Public Latitude As Double
    Public Longitude As Double
    Public NumAnnualSeries As Integer
    Public ZeroNonZero As Integer = 2 'currently not used by WREG
    Public FreqZero As Integer = 0 'currently not used by WREG
    Public RegionalSkew As Double = -99.9 'default dummy value
    Public Cont1PR2 As Double = 1 'by default if any data is downloaded, then it is a continuous daily timeseries
    Public StatFlowCharNdayFreq As atcCollection 'FlowChar
    Public StatLP3G As atcCollection 'LP3G
    Public StatLP3K As atcCollection 'LP3K
    Public StatLP3s As atcCollection 'LP3s
    Public TsAnnual As atcTimeseriesGroup 'USGS########.txt 's content
    Public Prefix As String = "USGS"
    Public Property WREGAnnualTSFilename() As String
        Get
            Return Prefix & StationID & ".txt" 'USGS########.txt
        End Get
        Set(value As String)
        End Set
    End Property

    Public Sub Clear()
        If StatFlowCharNdayFreq IsNot Nothing Then
            StatFlowCharNdayFreq.Clear() : StatFlowCharNdayFreq = Nothing
        End If
        If TsAnnual IsNot Nothing Then
            For Each lTs As atcTimeseries In TsAnnual
                lTs.Clear()
            Next
            TsAnnual.Clear()
            TsAnnual = Nothing
        End If
        If StatLP3G IsNot Nothing Then
            StatLP3G.Clear() : StatLP3G = Nothing
        End If
        If StatLP3K IsNot Nothing Then
            StatLP3K.Clear() : StatLP3K = Nothing
        End If
        If StatLP3s IsNot Nothing Then
            StatLP3s.Clear() : StatLP3s = Nothing
        End If
    End Sub
End Class

