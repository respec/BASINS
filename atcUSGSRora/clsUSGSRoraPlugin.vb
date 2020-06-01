Imports atcData
Imports atcUtility
Imports MapWinUtility

Imports System.Drawing

Public Class clsUSGSRoraPlugin
    Inherits atcData.atcDataDisplay

    Private pRequiredHelperPlugin As String = "Timeseries::Meteorologic Generation" 'atcMetCmp

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Analysis::USGS RORA"
        End Get
    End Property

    Public Overrides ReadOnly Property Icon() As System.Drawing.Icon
        Get
            Dim lResources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUSGSRora))
            Return CType(lResources.GetObject("$this.Icon"), System.Drawing.Icon)
        End Get
    End Property

    Public Overrides Function Show(ByVal aTimeseriesGroup As atcData.atcDataGroup) As Object
        Show = Nothing

        Dim lTimeseriesGroup As atcTimeseriesGroup = aTimeseriesGroup
        If lTimeseriesGroup Is Nothing Then lTimeseriesGroup = New atcTimeseriesGroup
        If lTimeseriesGroup.Count = 0 Then 'ask user to specify some Data
            lTimeseriesGroup = atcDataManager.UserSelectData("Select Daily Streamflow for Analysis", lTimeseriesGroup)
        End If
        If lTimeseriesGroup.Count = 1 Then
            Dim lForm As New frmUSGSRora
            ShowForm(lTimeseriesGroup, lForm)
            Return lForm
        ElseIf lTimeseriesGroup.Count > 1 Then
            Logger.Msg("RORA analyzes one daily streamflow dataset at a time.", "USGS RORA")
        Else
            Logger.Msg("Need to select a daily streamflow dataset", "USGS RORA")
        End If
    End Function

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
            Dim lForm As New frmUSGSRora

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

