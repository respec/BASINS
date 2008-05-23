Imports MapWinUtility
Imports atcSWMM

Public Class PlugIn
    Inherits atcData.atcDataPlugin
    'TODO: get these from BASINS4 or plugInManager?
    Private Const pModelsMenuName As String = "BasinsModels"
    Private Const pModelsMenuString As String = "Models"

    Private pSWMMProject As SWMMProject

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
            Return "An interface for initializing the EPA SWMM 5.0 model with BASINS data."
        End Get
    End Property

    <CLSCompliant(False)> _
    Public Overrides Sub Initialize(ByVal aMapWin As MapWindow.Interfaces.IMapWin, ByVal aParentHandle As Integer)
        pMapWin = aMapWin
        atcData.atcDataManager.AddMenuIfMissing(pModelsMenuName, "", pModelsMenuString, "mnuFile")
        atcData.atcDataManager.AddMenuIfMissing(pModelsMenuName & "_SWMM", pModelsMenuName, "SWMM")
        pSWMMProject = New SWMMProject
    End Sub

    Public Overrides Sub Terminate()
        atcData.atcDataManager.RemoveMenuIfEmpty(pModelsMenuName & "_SWMM")
        atcData.atcDataManager.RemoveMenuIfEmpty(pModelsMenuName)
    End Sub

    Public Overrides Sub ItemClicked(ByVal aItemName As String, ByRef aHandled As Boolean)
        If aItemName = pModelsMenuName & "_SWMM" Then
            atcMwGisUtility.GisUtil.MappingObject = pMapWin
            Dim lfrmSWMMSetup As New frmSWMMSetup
            lfrmSWMMSetup.InitializeUI(Me)
            lfrmSWMMSetup.Show()
            aHandled = True
        End If
    End Sub

    Public ReadOnly Property SWMMProject() As SWMMProject
        Get
            Return pSWMMProject
        End Get
    End Property
End Class
