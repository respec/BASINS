Imports atcData
Imports atcUtility
Imports MapWinUtility

Imports System.Drawing

Public Class clsUSGSBaseflowPlugin
    Inherits atcData.atcDataDisplay

    Private pRequiredHelperPlugin As String = "Timeseries::Meteorologic Generation" 'atcMetCmp

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Analysis::USGS Base-Flow Separation"
        End Get
    End Property

    Public Overrides ReadOnly Property Icon() As System.Drawing.Icon
        Get
            Dim lResources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUSGSBaseflowBatch))
            Return CType(lResources.GetObject("$this.Icon"), System.Drawing.Icon)
        End Get
    End Property

    Public Overrides Function Show(ByVal aTimeseriesGroup As atcData.atcDataGroup) As Object
        Dim lTimeseriesGroup As atcTimeseriesGroup = aTimeseriesGroup
        Show = Nothing
        Dim lBatchTitle As String = "Base-flow Separation Batch"
        Dim lChoice As String = Logger.MsgCustomOwned("Please choose analysis approach below:",
                                                      "Base-Flow Separation Analysis",
                                                      Nothing,
                                                      New String() {"Interactive", "Batch File", "Batch Map"})
        If lChoice = "Batch File" Then
            Dim lfrmBatch As New frmBatch()
            ShowForm(lfrmBatch)
            Return lfrmBatch
        ElseIf lChoice = "Batch Map" Then
            'If lTimeseriesGroup Is Nothing Then lTimeseriesGroup = New atcTimeseriesGroup()
            'lTimeseriesGroup = atcDataManager.UserSelectData("Select Daily Streamflow for Analysis", lTimeseriesGroup)
            'If lTimeseriesGroup.Count > 1 Then
            '    atcUSGSUtility.atcUSGSScreen.GraphDataDuration(lTimeseriesGroup)
            'End If
            Dim lgisLayerFound As Boolean = False
            Dim lstnSelected As Integer = 0
#If GISProvider = "DotSpatial" Then
#Else
            Dim lMapLayer As MapWindow.Interfaces.Layer = Nothing
            For Each lMapLayer In pMapWin.Layers
                If lMapLayer.Name.ToLower.Contains("nwis daily discharge stations") Then
                    lgisLayerFound = True
                    lstnSelected = lMapLayer.SelectedShapes.NumSelected
                    'lHandled = True
                    Exit For
                End If
            Next
#End If
            Dim lContinueWithNoStns As Boolean = False
            Dim lContAction As String = ""
            If lgisLayerFound Then
                If lstnSelected = 0 Then
#If GISProvider = "DotSpatial" Then
#Else
                    lContAction = Logger.MsgCustomOwned("No stream gage is selected." & vbCrLf & "Layer: " & lMapLayer.Name & vbCrLf & "Continue?", lBatchTitle, Nothing, New String() {"Yes", "No"})
#End If
                    If lContAction = "No" Then
                        Return Nothing
                    Else
                        lContinueWithNoStns = True
                    End If
                ElseIf lstnSelected < 2 Then
#If GISProvider = "DotSpatial" Then
#Else
                    Logger.Msg("Batch process can handle more than 1 stream gages." & vbCrLf & vbCrLf & "Layer: " & lMapLayer.Name, lBatchTitle)
#End If
                End If
            Else
                lContAction = Logger.MsgCustomOwned("Could not find stream gage station layer: NWIS Daily Discharge Stations." & vbCrLf & "Continue?", lBatchTitle, Nothing, New String() {"Yes", "No"})
                If lContAction = "No" Then
                    Return Nothing
                Else
                    lContinueWithNoStns = True
                End If
            End If
            Dim lSelectedStationIDs As New atcCollection()
#If GISProvider = "DotSpatial" Then
#Else
            Dim lShp As New MapWinGIS.Shapefile()
            If lgisLayerFound AndAlso lShp.Open(lMapLayer.FileName, Nothing) Then
                Dim lFieldIndex As Integer = -99
                Dim lFieldIndex_nm As Integer = -99
                Dim lRecordIndex As Integer = 0
                For I As Integer = 0 To lShp.NumFields - 1
                    If lShp.Field(I).Name.ToLower() = "site_no" Then
                        lFieldIndex = I
                        'Exit For
                    End If
                    If lShp.Field(I).Name.ToLower() = "station_nm" Then
                        lFieldIndex_nm = I
                    End If
                    If lFieldIndex >= 0 AndAlso lFieldIndex_nm >= 0 Then
                        Exit For
                    End If
                Next
                If lFieldIndex < 0 Then
                    Logger.Msg("NWIS daily flow layer lack station ID field. No batch.", lBatchTitle)
                    Return Nothing
                End If
                Dim lStationId As String = ""
                Dim lStationNm As String = ""
                For I As Integer = 0 To lMapLayer.SelectedShapes.NumSelected - 1
                    lRecordIndex = lMapLayer.SelectedShapes(I).ShapeIndex()
                    lStationId = lShp.CellValue(lFieldIndex, lRecordIndex).ToString()
                    lStationNm = lShp.CellValue(lFieldIndex_nm, lRecordIndex).ToString()
                    If Not lSelectedStationIDs.Keys.Contains(lStationId) Then
                        lSelectedStationIDs.Add(lStationId, lStationNm)
                    End If
                Next
            End If
#End If
            If lSelectedStationIDs.Count > 0 OrElse lContinueWithNoStns Then
                If lSelectedStationIDs.Count > 0 Then
                    Logger.Msg("The batch is selecting the following stations for the batch run." & vbCrLf &
                           lSelectedStationIDs.ToString(),
                           lBatchTitle)
                End If
                Dim lfrmBatchMap As New frmBatchMap()
                lfrmBatchMap.Initiate(lSelectedStationIDs)
                lfrmBatchMap.ShowDialog()
                Return Nothing 'for now
            End If
        End If

        If lTimeseriesGroup Is Nothing Then lTimeseriesGroup = New atcTimeseriesGroup
        If lTimeseriesGroup.Count = 0 Then 'ask user to specify some Data
            lTimeseriesGroup = atcDataManager.UserSelectData("Select Daily Streamflow for Analysis", lTimeseriesGroup)
        End If
        If lTimeseriesGroup.Count = 1 Then
            Dim lForm As New frmUSGSBaseflow()
            ShowForm(lTimeseriesGroup, lForm)
            Return lForm
        ElseIf lTimeseriesGroup.Count > 1 Then
            Logger.Msg("Interactive mode analyzes one daily streamflow dataset at a time.", "USGS Base-Flow Separation")
        Else
            Logger.Msg("Need to select a daily streamflow dataset", "USGS Base-Flow Separation")
        End If
    End Function

    Private Sub ShowForm(ByVal aForm As frmBatch)
        aForm.Initialize()
    End Sub

    Private Sub ShowForm(ByVal aTimeseriesGroup As atcData.atcDataGroup, ByVal aForm As Object)
        LoadPlugin(pRequiredHelperPlugin)
        Dim lBasicAttributes As New Generic.List(Of String)
        With lBasicAttributes
            .Add("ID")
            .Add("Min")
            .Add("Max")
            .Add("Mean")
            .Add("Standard Deviation")
            .Add("Count")
            .Add("Count Missing")
            .Add("STAID")
            .Add("STANAM")
            .Add("Constituent")
            .Add("Location")
        End With

        aForm.Initialize(aTimeseriesGroup, lBasicAttributes, True)
    End Sub

    Public Overrides Sub Save(ByVal aTimeseriesGroup As atcData.atcDataGroup,
                              ByVal aFileName As String,
                              ByVal ParamArray aOption() As String)

        If Not aTimeseriesGroup Is Nothing AndAlso aTimeseriesGroup.Count > 0 Then
            LoadPlugin(pRequiredHelperPlugin)
            Dim lForm As New frmUSGSBaseflowBatch

            lForm.Initialize(aTimeseriesGroup)
            atcUtility.SaveFileString(aFileName, lForm.ToString)
            'lForm.Dispose()
        End If
    End Sub

#If GISProvider = "DotSpatial" Then
#Else
    Public Overrides Sub Initialize(ByVal aMapWin As MapWindow.Interfaces.IMapWin, ByVal aParentHandle As Integer)
        MyBase.Initialize(aMapWin, aParentHandle)
    End Sub
#End If

    Private Sub LoadPlugin(ByVal aPluginName As String)
        Try
#If GISProvider = "DotSpatial" Then
#Else
            Dim lKey As String = pMapWin.Plugins.GetPluginKey(aPluginName)
            'If Not g_MapWin.Plugins.PluginIsLoaded(lKey) Then 
            pMapWin.Plugins.StartPlugin(lKey)
#End If
        Catch e As Exception
            Logger.Dbg("Exception loading " & aPluginName & ": " & e.Message)
        End Try
    End Sub
End Class
