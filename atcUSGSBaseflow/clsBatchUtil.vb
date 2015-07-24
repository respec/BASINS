Imports atcUtility
Imports MapWinUtility
Imports atcData
Imports System.Xml

Public Class clsBatchUtil

    Public Shared WDMDownload As String = ""
    Public Shared SiteInfoDir As String = ""
    Public StationList As atcCollection 'of Station IDs as strings

    Private Shared Property XML(ByVal aStationList As atcCollection) As String
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

    Public Shared Sub DownloadData(ByVal aStationList As atcCollection)
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

    Public Shared Function ReadTSFromRDB(ByVal aRDBFile As String, Optional ByVal aFindThese As atcDataAttributes = Nothing) As atcTimeseriesGroup
        Dim lRDBReader As New atcTimeseriesRDB.atcTimeseriesRDB()
        Dim lGroup As atcTimeseriesGroup = Nothing
        If lRDBReader.Open(aRDBFile) Then
            lGroup = New atcTimeseriesGroup()
            Dim lConsFind As String = aFindThese.GetValue("Constituent", "").ToString().ToLower()
            Dim lArrNames() As String
            If Not String.IsNullOrEmpty(lConsFind) Then
                lArrNames = lConsFind.Split(",")
                Dim lCons As String
                For Each lTS As atcTimeseries In lRDBReader.DataSets
                    lCons = lTS.Attributes.GetValue("Constituent").ToString
                    For Each lName As String In lArrNames
                        lName = lName.Trim()
                        If Not String.IsNullOrEmpty(lName) AndAlso String.Compare(lCons, lName, True) = 0 Then
                            lGroup.Add(lTS.Clone)
                            Exit For
                        End If
                    Next
                Next
            End If
        End If
        lRDBReader.Clear()
        lRDBReader = Nothing
        Return lGroup
    End Function
End Class
