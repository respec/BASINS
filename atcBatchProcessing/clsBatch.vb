Imports atcData
Imports atcTimeseriesBaseflow
Imports atcUtility

Public Interface IBatchProcessing
    Sub DoBatch()
    Sub SpecifyGlobal(ByVal aSpec As String)
    Sub SpecifyGroup(ByVal aSpec As String, ByVal aOpnCount As Integer)
    Sub MergeSpecs()
    Sub PopulateScenarios()
    Sub Clear()
End Interface

''' <summary>
''' The overall organizing program
''' </summary>
''' <remarks></remarks>
Public Class clsBatch
    Implements IBatchProcessing

    Public Enum ANALYSIS
        ITA
        DFLOW
    End Enum

    Private Specs As clsBatchBFSpec
    Public GlobalSettings As atcData.atcDataAttributes
    Public ListBatchUnitsData As atcCollection 'of clsBatchUnitBF, station id/location as key
    Public Delimiter As String = vbTab
    Public DownloadDataDirectory As String = ""
    Public MessageTimeseries As String = ""
    Public MessageParameters As String = ""
    Public Message As String

    Public Event StatusUpdate(ByVal aMsg As String)
    Public gProgressBar As Windows.Forms.ProgressBar = Nothing
    Public gTextStatus As Windows.Forms.TextBox = Nothing

    Public DownloadStations As atcCollection

    Public SpecFilename As String

    Public MethodsLastDone As ArrayList
    Public OutputFilenameRoot As String
    Public OutputDir As String

    Public Sub New(ByVal aSpecFilename As String)
        SpecFilename = aSpecFilename
    End Sub

    Public Sub New(ByVal aSpec As clsBatchBFSpec)
        Specs = aSpec
    End Sub

    Public Sub New()

    End Sub

    Public Sub ProcessBatch()
        If Specs Is Nothing Then Return

        For Each lBFOpnID As Integer In Specs.ListBatchBaseflowOpns.Keys
            Dim lBFOpn As atcCollection = Specs.ListBatchBaseflowOpns.ItemByKey(lBFOpnID)
            For Each lStation As clsBatchUnitStation In lBFOpn

            Next
        Next

    End Sub

    Public Overridable Sub DoBatch() Implements IBatchProcessing.DoBatch

    End Sub

    ''' <summary>
    ''' Only call this at time BF is done
    ''' </summary>
    ''' <param name="aStation"></param>
    ''' <param name="aPreserve">if work off the original Ts or off a clone</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Function GetStationStreamFlowData(ByVal aStation As clsBatchUnitStation, _
                                             ByVal aListBatchUnitsData As atcCollection, _
                                             Optional ByVal aPreserve As Boolean = False) As atcTimeseries
        Dim lDataFilename As String = ""
        If aListBatchUnitsData.Keys.Contains(aStation.StationID) Then
            lDataFilename = aListBatchUnitsData.ItemByKey(aStation.StationID)
        Else
            Dim lDataDir As String = GlobalSettings.GetValue(BFBatchInputNames.DataDir, "")
            Dim lDataFilenameInCache As String = IO.Path.Combine(lDataDir, "NWIS\NWIS_discharge_" & aStation.StationID & ".rdb")
            Dim lUseCached As Boolean = GlobalSettings.GetValue(BFBatchInputNames.UseCache, False)
            If lUseCached AndAlso IO.File.Exists(lDataFilenameInCache) Then
                aStation.SiteInfoDir = BFBatchInputNames.DataDir
                aStation.StationDataFilename = lDataFilenameInCache
                aStation.ReadData()
                lDataFilename = aStation.StationDataFilename
            Else
                'first download data, then read it
                lDataDir = aStation.StationDataFilename
                If lDataDir = "" Then
                    lDataDir = GlobalSettings.GetValue(BFBatchInputNames.DataDir, "")
                    If lDataDir = "" Then
                        Dim lOutDirDiag As New System.Windows.Forms.FolderBrowserDialog()
                        lOutDirDiag.Description = "Select Directory to Save All Downloaded Streamflow Data"
                        If lOutDirDiag.ShowDialog = Windows.Forms.DialogResult.OK Then
                            lDataDir = lOutDirDiag.SelectedPath
                            GlobalSettings.SetValue(BFBatchInputNames.DataDir, lDataDir)
                        End If
                        If String.IsNullOrEmpty(lDataDir) Then
                            Return Nothing
                        End If
                    ElseIf Not IO.Directory.Exists(lDataDir) Then
                        Try
                            IO.Directory.CreateDirectory(lDataDir)
                        Catch ex As Exception
                            Return Nothing
                        End Try
                    End If
                End If

                aStation.SiteInfoDir = lDataDir
                aStation.DownloadData()
                lDataFilename = aStation.StationDataFilename
            End If
        End If

        Dim lDataReady As Boolean = False
        If aStation.DataSource Is Nothing Then
            aStation.DataSource = New atcTimeseriesRDB.atcTimeseriesRDB()
            If aStation.DataSource.Open(lDataFilename) Then
                lDataReady = True
            End If
        End If
        If Not lDataReady Then
            If aStation.DataSource.DataSets.Count > 0 Then
                lDataReady = True
            Else
                'Already opened in the project
                For Each lDataSource As atcDataSource In atcDataManager.DataSources
                    Dim lSpec As String = lDataSource.Specification
                    If String.Compare(lDataFilename.Trim(), lSpec.Trim(), True) = 0 Then
                        lDataReady = True
                        aStation.DataSource.DataSets.AddRange(lDataSource.DataSets)
                        Exit For
                    End If
                Next
            End If
        End If
        If lDataReady Then
            Dim lGroup As atcTimeseriesGroup = aStation.DataSource.DataSets.FindData("Constituent", "Streamflow")
            If lGroup IsNot Nothing AndAlso lGroup.Count > 0 Then
                If aPreserve Then
                    Return lGroup(0)
                Else
                    Dim lTsFlow As atcTimeseries = lGroup(0).Clone()
                    'aStation.DataSource.Clear()
                    'aStation.DataSource = Nothing
                    Return lTsFlow
                End If
            End If
        End If

        Return Nothing
    End Function

    Friend Sub UpdateStatus(ByVal aMsg As String, Optional ByVal aAppend As Boolean = False, Optional ByVal aResetProgress As Boolean = False)
        If gProgressBar IsNot Nothing Then
            If aResetProgress Then
                gProgressBar.Minimum = 0
                gProgressBar.Maximum = 0
                gProgressBar.PerformStep()
                gProgressBar.Refresh()
            Else
                gProgressBar.PerformStep()
            End If
        End If
        If gTextStatus IsNot Nothing Then
            If aAppend Then
                gTextStatus.Text &= aMsg & vbCrLf
            Else
                gTextStatus.Text = aMsg
            End If
        End If
    End Sub

    Public Overridable Sub Clear() Implements IBatchProcessing.Clear
        If ListBatchUnitsData IsNot Nothing Then ListBatchUnitsData.Clear()
    End Sub

    Public Overridable Sub MergeSpecs() Implements IBatchProcessing.MergeSpecs

    End Sub

    Public Overridable Sub PopulateScenarios() Implements IBatchProcessing.PopulateScenarios

    End Sub

    Public Overridable Sub SpecifyGlobal(ByVal aSpec As String) Implements IBatchProcessing.SpecifyGlobal

    End Sub

    Public Overridable Sub SpecifyGroup(ByVal aSpec As String, ByVal aOpnCount As Integer) Implements IBatchProcessing.SpecifyGroup

    End Sub
End Class
