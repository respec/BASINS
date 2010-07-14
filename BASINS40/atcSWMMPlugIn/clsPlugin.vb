Imports MapWinUtility
Imports atcSWMM
Imports atcData.atcDataManager

Public Class PlugIn
    Inherits atcData.atcDataPlugin

    Private pSWMMProject As atcSWMMProject

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
        MyBase.Initialize(aMapWin, aParentHandle)
        atcMwGisUtility.GisUtil.MappingObject = aMapWin
        AddMenuIfMissing(ModelsMenuName, "", ModelsMenuString, "mnuFile")
        AddMenuIfMissing(ModelsMenuName & "_SWMM", ModelsMenuName, "SWMM")
        pSWMMProject = New atcSWMMProject
        pSWMMProject.IsMetric = False
    End Sub

    Public Overrides Sub Terminate()
        atcData.atcDataManager.RemoveMenuIfEmpty(ModelsMenuName & "_SWMM")
        atcData.atcDataManager.RemoveMenuIfEmpty(ModelsMenuName)
    End Sub

    Public Overrides Sub ItemClicked(ByVal aItemName As String, ByRef aHandled As Boolean)
        If aItemName = ModelsMenuName & "_SWMM" Then
            pSWMMProject = New atcSWMMProject
            pSWMMProject.IsMetric = False
            Dim lfrmSWMMSetup As New frmSWMMSetup
            With lfrmSWMMSetup
                .InitializeUI(Me)
                Logger.Dbg("SWMMSetup Initialized")
                .Show()
                Logger.Dbg("SWMMSetup Shown")
                .InitializeMetStationList()
                .EnableControls(True)
                aHandled = True
            End With
        End If
    End Sub

    Public ReadOnly Property SWMMProject() As atcSWMMProject
        Get
            Return pSWMMProject
        End Get
    End Property
End Class
