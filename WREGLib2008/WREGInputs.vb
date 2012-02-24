Imports atcUtility
Imports MapWinUtility
Imports atcData
Imports System.Xml
Imports System.Text.RegularExpressions

Public Class WREGInputs
    Public Stations As atcCollection
    Public IsOkSiteInfo As Boolean
    Public IsOkFlowChar As Boolean
    Public IsOkLP3G As Boolean
    Public IsOkLP3K As Boolean
    Public IsOkLP3s As Boolean
    Public IsOkTsAnnual As Boolean
    Public InputPath As String
    Public SiteInfoDir As String
    Public WDMDownload As String
    Public NDayFreqs As atcCollection
    Public SiteInfoFields() As String = {"Station ID", "Lat", "Long", "No. Annual Series", "Zero-1;NonZero-2", "FreqZero", "Regional Skew", "Cont-1;PR-2"}
    Public Delimiter As String = vbTab

    Private Property XML(ByVal aStationList As atcCollection) As String
        Get
            Dim lXML As String = "<function name='GetNWISDailyDischarge'>" & vbCrLf
            lXML &= "<arguments>" & vbCrLf
            lXML &= "<SaveWDM>" & WDMDownload & "</SaveWDM>"
            lXML &= "<SaveIn>" & SiteInfoDir & "</SaveIn>"
            lXML &= "<CacheFolder>C:\Basins\cache\</CacheFolder>"
            For Each lStation As String In aStationList
                lXML &= "<stationid>" & lStation & "</stationid>" & vbCrLf
            Next
            lXML &= "<clip>False</clip>"
            lXML &= "<merge>False</merge>"
            lXML &= "<joinattributes>true</joinattributes>"
            lXML &= "</arguments>"
            lXML &= "</function>"
            Return lXML
        End Get
        Set(ByVal value As String)
        End Set
    End Property

    Public Sub New()
        Stations = New atcCollection
        IsOkSiteInfo = False
        IsOkFlowChar = False
        IsOkLP3G = False
        IsOkLP3K = False
        IsOkLP3s = False
        IsOkTsAnnual = False
        InputPath = "" 'TODO: Need to allow setting this at run time
    End Sub

    Public Sub DownloadData(ByVal aStationList As atcCollection)
        Logger.Status("LABEL TITLE BASINS Data Download")
        Dim lQuery As New XmlDocument
        Dim lNode As XmlNode
        lQuery.LoadXml(XML(aStationList))
        lNode = lQuery.FirstChild.FirstChild
        Dim lResult As String = D4EMNWISDataExtension.GetDailyDischarge(lNode)
        If lResult Is Nothing Then
            Logger.Dbg("QueryResult:Nothing")
        Else
            Logger.Dbg("QueryResult:" & lResult)
            Dim lSilentSuccess As Boolean = lResult.ToLower.Contains("<success />")
            If lSilentSuccess Then
                lResult = lResult.Replace("<success />", "").Trim
                Logger.Dbg("QueryResultTrimmed:" & lResult)
            End If
            If lResult.Length = 0 Then
                If lSilentSuccess Then Logger.Msg("Download Complete", "Data Download")
            ElseIf lResult.Contains("<success>") Then
                'open it in the main program
            Else
                Logger.Msg(atcUtility.ReadableFromXML(lResult), "Data Download")
            End If
        End If
    End Sub

    Public Function WriteInputs() As Boolean
        If Stations Is Nothing OrElse Stations.Count = 0 Then
            Return False
        End If

        If NDayFreqs Is Nothing Then
            NDayFreqs = New atcCollection
            Dim lStation As WREGStation = Stations(0)
            For Each lKey As String In lStation.StatFlowCharNdayFreq.Keys
                NDayFreqs.Add(lKey)
            Next
        End If

        If Not WriteSiteInfo() OrElse _
           Not WriteFlowChar() OrElse _
           Not WriteLP3G() OrElse _
           Not WriteLP3K() OrElse _
           Not WriteLP3s() OrElse _
           Not WriteTsAnnual() Then
            Return False
        End If
        Return True
    End Function

    Public Function WriteSiteInfo() As Boolean
        Dim lSiteInfoFilename As String = IO.Path.Combine(InputPath, "SiteInfo.txt")
        Try
            TryDelete(lSiteInfoFilename)
            Dim lSW As IO.StreamWriter = New IO.StreamWriter(lSiteInfoFilename, False)
            'Write header
            lSW.WriteLine(String.Join(Delimiter, SiteInfoFields))

            'Write content
            Dim lContent As New System.Text.StringBuilder
            For Each lStation As WREGStation In Stations
                With lStation
                    lContent.Append(.StationID & Delimiter)
                    lContent.Append(.Latitude & Delimiter)
                    lContent.Append(.Longitude & Delimiter)
                    lContent.Append(.NumAnnualSeries & Delimiter)
                    lContent.Append(.ZeroNonZero & Delimiter)
                    lContent.Append(.FreqZero & Delimiter)
                    lContent.Append(.RegionalSkew & Delimiter)
                    lContent.AppendLine(.Cont1PR2)
                End With
            Next

            lSW.Write(lContent.ToString)
            lSW.Flush()
            lSW.Close()
            lSW = Nothing
            'lContent.Capacity = 0
            lContent = Nothing
            IsOkSiteInfo = True
        Catch ex As Exception
            Logger.Msg("WriteSiteInfo failed: " & ex.InnerException.Message, "WriteSiteInfo failed")
            IsOkSiteInfo = False
        End Try

        Return IsOkSiteInfo
    End Function

    Public Function WriteFlowChar() As Boolean
        Dim lFlowCharFilename As String = IO.Path.Combine(InputPath, "FlowChar.txt")
        Try
            TryDelete(lFlowCharFilename)
            Dim lSW As IO.StreamWriter = New IO.StreamWriter(lFlowCharFilename, False)

            'Write header
            Dim lHeaders As New List(Of String)
            lHeaders.Add("Station ID")
            For Each lNdayFreq As String In NDayFreqs
                lHeaders.Add(FCGKSHeader("Q", lNdayFreq))
            Next
            Dim lStrings() As String = lHeaders.ToArray
            lSW.WriteLine(String.Join(Delimiter, lStrings))

            'Write content
            Dim lContent As New System.Text.StringBuilder
            For Each lStation As WREGStation In Stations
                With lStation
                    lContent.Append(.StationID & Delimiter)
                    For I As Integer = 0 To .StatFlowCharNdayFreq.Count - 1
                        If I < .StatFlowCharNdayFreq.Count - 1 Then
                            lContent.Append(.StatFlowCharNdayFreq(I) & Delimiter)
                        Else
                            lContent.AppendLine(.StatFlowCharNdayFreq(I))
                        End If
                    Next
                End With
            Next

            lSW.Write(lContent.ToString)
            lSW.Flush()
            lSW.Close()
            lSW = Nothing
            'lContent.Capacity = 0
            lContent = Nothing

            IsOkFlowChar = True
        Catch ex As Exception
            Logger.Msg("WriteFlowChar failed: " & ex.InnerException.Message, "WriteFlowChar failed")
            IsOkFlowChar = True
        End Try

        Return IsOkFlowChar
    End Function

    Public Function WriteLP3G() As Boolean
        Dim lLP3GFilename As String = IO.Path.Combine(InputPath, "LP3G.txt")
        Try

            TryDelete(lLP3GFilename)
            Dim lSW As IO.StreamWriter = New IO.StreamWriter(lLP3GFilename, False)

            'Write header
            Dim lHeaders As New List(Of String)
            lHeaders.Add("Station ID")
            For Each lNdayFreq As String In NDayFreqs
                lHeaders.Add(FCGKSHeader("Skew", lNdayFreq))
            Next
            lSW.WriteLine(String.Join(Delimiter, lHeaders.ToArray))

            'Write content
            Dim lContent As New System.Text.StringBuilder
            For Each lStation As WREGStation In Stations
                With lStation
                    lContent.Append(.StationID & Delimiter)
                    For I As Integer = 0 To .StatLP3G.Count - 1
                        If I < .StatLP3G.Count - 1 Then
                            lContent.Append(.StatLP3G(I) & Delimiter)
                        Else
                            lContent.AppendLine(.StatLP3G(I))
                        End If
                    Next
                End With
            Next

            lSW.Write(lContent.ToString)
            lSW.Flush()
            lSW.Close()
            lSW = Nothing
            'lContent.Capacity = 0
            lContent = Nothing

            IsOkLP3G = True
        Catch ex As Exception
            Logger.Msg("WriteLP3G failed: " & ex.InnerException.Message, "WriteLP3G failed")
            IsOkLP3G = False
        End Try

        Return IsOkLP3G
    End Function

    Public Function WriteLP3K() As Boolean
        Dim lLP3KFilename As String = IO.Path.Combine(InputPath, "LP3K.txt")
        Try

            TryDelete(lLP3KFilename)
            Dim lSW As IO.StreamWriter = New IO.StreamWriter(lLP3KFilename, False)

            'Write header
            Dim lHeaders As New List(Of String)
            lHeaders.Add("Station ID")
            For Each lNdayFreq As String In NDayFreqs
                lHeaders.Add(FCGKSHeader("K-", lNdayFreq))
            Next
            lSW.WriteLine(String.Join(Delimiter, lHeaders.ToArray))

            'Write content
            Dim lContent As New System.Text.StringBuilder
            For Each lStation As WREGStation In Stations
                With lStation
                    lContent.Append(.StationID & Delimiter)
                    For I As Integer = 0 To .StatLP3K.Count - 1
                        If I < .StatLP3K.Count - 1 Then
                            lContent.Append(.StatLP3K(I) & Delimiter)
                        Else
                            lContent.AppendLine(.StatLP3K(I))
                        End If
                    Next
                End With
            Next

            lSW.Write(lContent.ToString)
            lSW.Flush()
            lSW.Close()
            lSW = Nothing
            'lContent.Capacity = 0
            lContent = Nothing

            IsOkLP3K = True
        Catch ex As Exception
            Logger.Msg("WriteLP3K failed: " & ex.InnerException.Message, "WriteLP3K failed")
            IsOkLP3K = False
        End Try

        Return IsOkLP3K
    End Function

    Public Function WriteLP3s() As Boolean
        Dim lLP3sFilename As String = IO.Path.Combine(InputPath, "LP3s.txt")
        Try
            TryDelete(lLP3sFilename)
            Dim lSW As IO.StreamWriter = New IO.StreamWriter(lLP3sFilename, False)

            'Write header
            Dim lHeaders As New List(Of String)
            lHeaders.Add("Station ID")
            For Each lNdayFreq As String In NDayFreqs
                lHeaders.Add(FCGKSHeader("s-", lNdayFreq))
            Next
            lSW.WriteLine(String.Join(Delimiter, lHeaders.ToArray))

            'Write content
            Dim lContent As New System.Text.StringBuilder
            For Each lStation As WREGStation In Stations
                With lStation
                    lContent.Append(.StationID & Delimiter)
                    For I As Integer = 0 To .StatLP3s.Count - 1
                        If I < .StatLP3s.Count - 1 Then
                            lContent.Append(.StatLP3s(I) & Delimiter)
                        Else
                            lContent.AppendLine(.StatLP3s(I))
                        End If
                    Next
                End With
            Next

            lSW.Write(lContent.ToString)
            lSW.Flush()
            lSW.Close()
            lSW = Nothing
            'lContent.Capacity = 0
            lContent = Nothing
            IsOkLP3s = True
        Catch ex As Exception
            Logger.Msg("WriteLP3s failed: " & ex.InnerException.Message, "WriteLP3s failed")
            IsOkLP3s = False
        End Try

        Return IsOkLP3s
    End Function

    Public Function WriteTsAnnual() As Boolean
        Dim lUSGSFilename As String = ""
        Dim lUSGSPrefix As String = ""
        Try
            For Each lStation As WREGStation In Stations
                lUSGSPrefix = "USGS" & lStation.StationID

                For Each lStat As String In lStation.TsAnnual.Keys
                    lUSGSFilename = IO.Path.Combine(InputPath, lUSGSPrefix & "." & lStat & ".txt")
                    TryDelete(lUSGSFilename)
                    Dim lSW As IO.StreamWriter = New IO.StreamWriter(lUSGSFilename, False)
                    Dim lDate(5) As Integer
                    Dim lTsAnnual As atcTimeseries = lStation.TsAnnual.ItemByKey(lStat)
                    For I As Integer = 1 To lTsAnnual.numValues
                        J2Date(lTsAnnual.Dates.Value(I), lDate)
                        lSW.WriteLine(lUSGSPrefix & Delimiter & lDate(0) & Delimiter & lTsAnnual.Value(I))
                    Next
                    lSW.Flush()
                    lSW.Close()
                    lSW = Nothing
                Next
            Next
            IsOkTsAnnual = True
        Catch ex As Exception
            Logger.Msg("WriteTsAnnual failed: " & ex.InnerException.Message, "WriteTsAnnual failed")
            IsOkTsAnnual = False
        End Try

        Return IsOkTsAnnual
    End Function

    Private Function FCGKSHeader(ByVal aPrefix As String, ByVal aNdayFreq As String) As String
        Dim lFreq As Integer = Integer.Parse(Regex.Split(aNdayFreq, "\D+")(1))
        lFreq = 1.0 / lFreq * 100.0
        Return aPrefix & lFreq & "%"
    End Function

    Public Sub Clear()
        If Stations IsNot Nothing Then
            For Each lStation As WREGStation In Stations
                lStation.Clear()
            Next
            Stations.Clear()
            Stations = Nothing
        End If
    End Sub
End Class

