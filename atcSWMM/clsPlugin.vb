Imports atcMwGisUtility
Imports atcData

Public Class PlugIn
    Inherits atcData.atcDataPlugin
    'TODO: get these from BASINS4 or plugInManager?
    Private Const pModelsMenuName As String = "BasinsModels"
    Private Const pModelsMenuString As String = "Models"

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "EPA SWMM 5.0 Setup"
        End Get
    End Property

    Public Overrides ReadOnly Property Author() As String
        Get
            Return "AQUA TERRA Consultants"
        End Get
    End Property

    Public Overrides ReadOnly Property Description() As String
        Get
            Return "An interface for initializing the EPA SWMM 5.0 model."
        End Get
    End Property

    <CLSCompliant(False)> _
    Public Overrides Sub Initialize(ByVal MapWin As MapWindow.Interfaces.IMapWin, ByVal ParentHandle As Integer)
        pMapWin = MapWin
        atcData.atcDataManager.AddMenuIfMissing(pModelsMenuName, "", pModelsMenuString, "mnuFile")
        atcData.atcDataManager.AddMenuIfMissing(pModelsMenuName & "_SWMM", pModelsMenuName, "SWMM")
    End Sub

    Public Overrides Sub Terminate()
        atcData.atcDataManager.RemoveMenuIfEmpty(pModelsMenuName & "_SWMM")
        atcData.atcDataManager.RemoveMenuIfEmpty(pModelsMenuName)
    End Sub

    Public Overrides Sub ItemClicked(ByVal aItemName As String, ByRef Handled As Boolean)
        If aItemName = pModelsMenuName & "_SWMM" Then
            Dim main As New frmSWMMSetup
            GisUtil.MappingObject = pMapWin
            main.InitializeUI()
            main.Show()
            Handled = True
        End If
    End Sub
End Class
