Imports atcMwGisUtility
Imports atcData.atcDataManager

Public Class PlugIn
    Inherits atcData.atcDataPlugin

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Model Setup for HSPF"
        End Get
    End Property

    Public Overrides ReadOnly Property Description() As String
        Get
            Return "An interface for setting up the HSPF watershed model."
        End Get
    End Property

    <CLSCompliant(False)> _
    Public Overrides Sub Initialize(ByVal MapWin As MapWindow.Interfaces.IMapWin, ByVal ParentHandle As Integer)
        pMapWin = MapWin
        atcData.atcDataManager.AddMenuIfMissing(ModelsMenuName, "", ModelsMenuString, "mnuFile")
        atcData.atcDataManager.AddMenuIfMissing(ModelsMenuName & "_HSPF", ModelsMenuName, "HSPF")
        'atcData.atcDataManager.AddMenuIfMissing(ModelsMenuName & "_AQUATOX", ModelsMenuName, "AQUATOX")
    End Sub

    Public Overrides Sub Terminate()
        atcData.atcDataManager.RemoveMenuIfEmpty(ModelsMenuName & "_HSPF")
        'atcData.atcDataManager.RemoveMenuIfEmpty(ModelsMenuName & "_AQUATOX")
        atcData.atcDataManager.RemoveMenuIfEmpty(ModelsMenuName)
    End Sub

    Public Overrides Sub ItemClicked(ByVal ItemName As String, ByRef Handled As Boolean)
        If ItemName = ModelsMenuName & "_HSPF" Then
            Dim lFormModelSetup As New frmModelSetup
            GisUtil.MappingObject = pMapWin
            lFormModelSetup.SetModelName("HSPF")
            lFormModelSetup.InitializeUI()
            lFormModelSetup.Show()
            lFormModelSetup.InitializeMetStationList()
            Handled = True
            'ElseIf ItemName = ModelsMenuName & "_AQUATOX" Then
            '    Dim lFormModelSetup As New frmModelSetup
            '    GisUtil.MappingObject = pMapWin
            '    lFormModelSetup.SetModelName("AQUATOX")
            '    lFormModelSetup.InitializeUI()
            '    lFormModelSetup.Show()
            '    Handled = True
        End If
    End Sub
End Class
