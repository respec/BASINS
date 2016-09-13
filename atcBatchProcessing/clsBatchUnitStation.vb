Imports atcData
Imports atcUtility
Imports System.Xml

Public Class clsBatchUnitStation
    Public StationID As String
    Public DataSource As atcTimeseriesRDB.atcTimeseriesRDB
    Public BFInputs As atcDataAttributes = New atcDataAttributes() 'Batch File Inputs
    Public Message As String = ""
    Public NeedToDownloadData As Boolean = True
    Public StationDrainageArea As Double = -99
    Public StationDataFilename As String = ""
    Public SiteInfoDir As String = ""

    Private pStreamFlowData As atcTimeseries
    Public Property StreamFlowData() As atcTimeseries
        Get
            If DataSource IsNot Nothing Then
                Dim lArg As New atcDataAttributes
                lArg.Add("Constituent", "streamflow")
                Dim lGroup As atcTimeseriesGroup = GetTS(lArg)
                If lGroup Is Nothing OrElse lGroup.Count = 0 Then
                    lArg.Clear()
                    lArg.Add("Constituent", "FLOW")
                    lGroup = GetTS(lArg)
                End If
                If lGroup IsNot Nothing AndAlso lGroup.Count > 0 Then
                    pStreamFlowData = lGroup(0)
                Else
                    pStreamFlowData = Nothing
                End If
                Return pStreamFlowData
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal value As atcTimeseries)
            pStreamFlowData = value
        End Set
    End Property

    Public Sub New()
    End Sub

    Public Sub New(ByVal aArgs As atcDataAttributes)
        If aArgs IsNot Nothing Then BFInputs = aArgs
    End Sub

    Public Function DoBaseFlowSeparation() As Boolean
        Dim lSuccess As Boolean = True

        If lSuccess Then
            Message = "SUCCESS"
        End If

        Return lSuccess
    End Function

    Private Property XML() As String
        Get
            If String.IsNullOrEmpty(StationID) Then Return ""
            Dim lXML As String = "<function name='GetNWISDailyDischarge'>" & vbCrLf
            lXML &= "<arguments>" & vbCrLf
            'lXML &= "<SaveWDM>" & WDMDownload & "</SaveWDM>"
            lXML &= "<SaveIn>" & SiteInfoDir & "</SaveIn>"
            lXML &= "<CacheFolder>C:\Basins\cache\</CacheFolder>"
            lXML &= "<stationid>" & StationID & "</stationid>" & vbCrLf
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

    Public Sub DownloadData()
        'Logger.Status("LABEL TITLE BASINS Data Download")
        Dim lXML As String = XML()
        If String.IsNullOrEmpty(lXML) Then
            Message = "Error: Station is not set, no download."
            Return
        End If
        Dim lQuery As New XmlDocument
        'Dim lNode As XmlNode
        'lQuery.LoadXml(XML())
        'lNode = lQuery.FirstChild.FirstChild
        'Dim lResult As String = D4EMNWISDataExtension.GetDailyDischarge(lNode)
        Dim lResult As String = BASINS.Execute(XML())

        If lResult Is Nothing Then
            Message = "QueryResult:Nothing"
        Else
            Message = "QueryResult:" & lResult
            Dim lSilentSuccess As Boolean = lResult.ToLower.Contains("<success />")
            If lSilentSuccess Then
                lResult = lResult.Replace("<success />", "").Trim
                Message = "QueryResultTrimmed:" & lResult
            End If
            If lResult.Length = 0 Then
                If lSilentSuccess Then Message = "Download Complete"
            ElseIf lResult.Contains("<success>") Then
                'open it in the main program
            Else
                Message = atcUtility.ReadableFromXML(lResult)
            End If
            Dim lBaseFilename As String = "NWIS_discharge_" & StationID & ".rdb"
            Dim lDataFilename As String = IO.Path.Combine(SiteInfoDir, "NWIS\" & lBaseFilename)
            If IO.File.Exists(lDataFilename) Then
                StationDataFilename = lDataFilename
            Else
                StationDataFilename = ""
            End If

            If Not String.IsNullOrEmpty(StationDataFilename) Then
                DataSource = New atcTimeseriesRDB.atcTimeseriesRDB()
                If DataSource.Open(StationDataFilename) Then
                    NeedToDownloadData = False
                Else
                    DataSource.Clear()
                    DataSource = Nothing
                    NeedToDownloadData = True
                    StationDataFilename = ""
                End If
            End If

        End If
    End Sub

    Public Sub ReadData()
        If DataSource Is Nothing Then
            DataSource = New atcTimeseriesRDB.atcTimeseriesRDB()
        Else
            DataSource.Clear()
        End If

        If DataSource.Open(StationDataFilename) Then
            NeedToDownloadData = False
        Else
            DataSource.Clear()
            DataSource = Nothing
            NeedToDownloadData = True
            StationDataFilename = ""
        End If
    End Sub
    Public Function GetTS(ByVal aFindThese As atcDataAttributes) As atcTimeseriesGroup
        'If DataSource Is Nothing OrElse aRDBFile <> StationDataFilename Then DataSource = New atcTimeseriesRDB.atcTimeseriesRDB()
        If aFindThese Is Nothing Then
            If DataSource IsNot Nothing Then
                Return DataSource.DataSets
            Else
                Return Nothing
            End If
        End If
        Dim lGroup As New atcTimeseriesGroup
        Dim lCons As String
        Dim lConsFind As String = aFindThese.GetValue("Constituent", "").ToString.ToLower()
        For Each lTS As atcTimeseries In DataSource.DataSets
            lCons = lTS.Attributes.GetValue("Constituent").ToString.ToLower()
            If lCons = lConsFind Then
                lGroup.Add(lTS)
            End If
        Next

        Return lGroup
    End Function

    Public Sub Clear()
        DataSource.Clear()
        DataSource = Nothing
        BFInputs.Clear()
    End Sub

End Class
