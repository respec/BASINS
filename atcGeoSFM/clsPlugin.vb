Imports MapWinUtility
Imports atcWASP
Imports atcData.atcDataManager

Public Class PlugIn
    Inherits atcData.atcDataPlugin

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "GeoSFM"
        End Get
    End Property

    Public Overrides ReadOnly Property Author() As String
        Get
            Return "AQUA TERRA Consultants"
        End Get
    End Property

    Public Overrides ReadOnly Property Description() As String
        Get
            Return "An interface for running the Geospatial Stream Flow Model GeoSFM."
        End Get
    End Property

    <CLSCompliant(False)> _
    Public Overrides Sub Initialize(ByVal aMapWin As MapWindow.Interfaces.IMapWin, ByVal aParentHandle As Integer)
        MyBase.Initialize(aMapWin, aParentHandle)
        atcMwGisUtility.GisUtil.MappingObject = aMapWin
        AddMenuIfMissing(ModelsMenuName, "", ModelsMenuString, "mnuFile")
        AddMenuIfMissing(ModelsMenuName & "_GeoSFM", ModelsMenuName, "GeoSFM")
    End Sub

    Public Overrides Sub Terminate()
        atcData.atcDataManager.RemoveMenuIfEmpty(ModelsMenuName & "_GeoSFM")
        atcData.atcDataManager.RemoveMenuIfEmpty(ModelsMenuName)
    End Sub

    Public Overrides Sub ItemClicked(ByVal aItemName As String, ByRef aHandled As Boolean)
        If aItemName = ModelsMenuName & "_GeoSFM" Then
            Dim lfrmGeoSFM As New frmGeoSFM
            With lfrmGeoSFM
                .InitializeUI(Me)
                Logger.Dbg("GeoSFM User Interface Initialized")
                .Show()
                Logger.Dbg("GeoSFM User Interface Shown")
                aHandled = True
            End With
        End If
    End Sub

End Class
