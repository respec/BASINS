Imports MapWinUtility
Imports atcWASP
Imports atcData.atcDataManager

Public Class PlugIn
    Inherits atcData.atcDataPlugin

    Private pWASPProject As atcWASPProject

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "EPA WASP 7.3 Setup"
        End Get
    End Property

    Public Overrides ReadOnly Property Author() As String
        Get
            Return "AQUA TERRA Consultants"
        End Get
    End Property

    Public Overrides ReadOnly Property Description() As String
        Get
            Return "An interface for initializing the EPA WASP 7.3 model with BASINS data."
        End Get
    End Property

    <CLSCompliant(False)> _
    Public Overrides Sub Initialize(ByVal aMapWin As MapWindow.Interfaces.IMapWin, ByVal aParentHandle As Integer)
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
            pWASPProject = New atcWASPProject
            Dim lfrmWASPInitialize As New frmWASPInitialize
            With lfrmWASPInitialize
                .InitializeUI(Me)
                .Show()
                aHandled = True
            End With
        End If
    End Sub

    Public ReadOnly Property WASPProject() As atcWASPProject
        Get
            Return pWASPProject
        End Get
    End Property
End Class
