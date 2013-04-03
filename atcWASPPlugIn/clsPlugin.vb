'See atcWASPProject.vb in atcWASP project for comments

'Note: this is an update to the former WASP Setup plugin; due to the many additional libraries it references, it is being
'built to its own folder under the Plugins folder; the old atcWaspPlugin.dll version must be removed from BASINS plugins folder
'to avoid a conflict.

'Note: referenced WRDB libraries automatically include atcWDM and atcWDMVB libraries which will conflict with those same files elsewhere
'when the plugins are refreshed; a post-build event has been setup to delete these files.

Imports MapWinUtility
Imports atcWASP
Imports atcData.atcDataManager

Public Class PlugIn
    Inherits atcData.atcDataPlugin

    Private pWASPProject As atcWASPProject
    Private lfrmWASPInitialize As frmWASPInitialize
    Public pfrmWASPSetup As frmWASPSetup
    Public StartupAlreadyShown As Boolean = False


    Public ReadOnly Property MapWin() As MapWindow.Interfaces.IMapWin
        Get
            Return pMapWin
        End Get
    End Property

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "EPA WASP Model Builder"
        End Get
    End Property

    Public Overrides ReadOnly Property Author() As String
        Get
            Return "AQUA TERRA Consultants/Wilson Engineering"
        End Get
    End Property

    Public Overrides ReadOnly Property Description() As String
        Get
            Return "An interface for initializing an EPA WASP model with BASINS data."
        End Get
    End Property

    <CLSCompliant(False)> _
    Public Overrides Sub Initialize(ByVal aMapWin As MapWindow.Interfaces.IMapWin, ByVal aParentHandle As Integer)
        System.Windows.Forms.Application.EnableVisualStyles()
        MyBase.Initialize(aMapWin, aParentHandle)
        atcMwGisUtility.GisUtil.MappingObject = aMapWin
        AddMenuIfMissing(ModelsMenuName, "", ModelsMenuString, "mnuFile")
        AddMenuIfMissing(ModelsMenuName & "_WASP", ModelsMenuName, "WASP")
        pWASPProject = New atcWASPProject
    End Sub

    Public Overrides Sub Terminate()
        atcData.atcDataManager.RemoveMenuIfEmpty(ModelsMenuName & "_WASP")
        atcData.atcDataManager.RemoveMenuIfEmpty(ModelsMenuName)
    End Sub

    Public Overrides Sub ItemClicked(ByVal aItemName As String, ByRef aHandled As Boolean)
        If aItemName = ModelsMenuName & "_WASP" Then
            StartupAlreadyShown = False
            pfrmWASPSetup = New frmWASPSetup
            With pfrmWASPSetup
                .Show()
                .InitializeUI(Me, Nothing, 0)
            End With
            'WASPInitialize()
            aHandled = True
        End If
    End Sub

    Public Sub WASPInitialize()
        'want to retain filename if it exists
        Dim Filename As String = ""
        If WASPProject IsNot Nothing Then Filename = WASPProject.Filename
        pMapWin.View.CursorMode = MapWinGIS.tkCursorMode.cmSelection
        pWASPProject = New atcWASPProject
        pWASPProject.Filename = Filename
        lfrmWASPInitialize = New frmWASPInitialize
        With lfrmWASPInitialize
            .InitializeUI(Me)
            .Show()
        End With
    End Sub

    Public Overrides Sub LayerSelected(ByVal aHandle As Integer)
        MyBase.LayerSelected(aHandle)
        If lfrmWASPInitialize IsNot Nothing Then lfrmWASPInitialize.RefreshSelectionInfo()
    End Sub

    Public Overrides Sub ShapesSelected(ByVal aHandle As Integer, ByVal aSelectInfo As MapWindow.Interfaces.SelectInfo)
        MyBase.ShapesSelected(aHandle, aSelectInfo)
        If lfrmWASPInitialize IsNot Nothing Then lfrmWASPInitialize.RefreshSelectionInfo()
        If pfrmWASPSetup IsNot Nothing Then pfrmWASPSetup.RefreshSelectionInfo()
    End Sub

    'Public ReadOnly Property WASPProject() As atcWASPProject
    '    Get
    '        Return pWASPProject
    '    End Get
    'End Property

    Public Property WASPProject() As atcWASPProject
        Get
            Return pWASPProject
        End Get
        Set(ByVal value As atcWASPProject)
            pWASPProject = value
        End Set
    End Property
End Class
