Imports atcMwGisUtility
Imports MapWinUtility
Imports atcData.atcDataManager

Public Class PlugIn
    Inherits atcData.atcDataPlugin

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Pollutant Loading Estimator (PLOAD)"
        End Get
    End Property

    Public Overrides ReadOnly Property Description() As String
        Get
            Return "An interface for generating pollutant loading estimates"
        End Get
    End Property

    <CLSCompliant(False)> _
    Public Overrides Sub Initialize(ByVal MapWin As MapWindow.Interfaces.IMapWin, ByVal ParentHandle As Integer)
        pMapWin = MapWin

        atcData.atcDataManager.AddMenuIfMissing(ModelsMenuName, "", ModelsMenuString, "mnuFile")
        atcData.atcDataManager.AddMenuIfMissing(ModelsMenuName & "_PLOAD", ModelsMenuName, "PLOAD")
    End Sub

    Public Overrides Sub Terminate()
        atcData.atcDataManager.RemoveMenuIfEmpty(ModelsMenuName & "_PLOAD")
        atcData.atcDataManager.RemoveMenuIfEmpty(ModelsMenuName)
    End Sub

    Public Overrides Sub ItemClicked(ByVal ItemName As String, ByRef Handled As Boolean)
        If ItemName = ModelsMenuName & "_PLOAD" Then
            GisUtil.MappingObject = pMapWin
            Dim lPollutantLoading As New frmPollutantLoading
            lPollutantLoading.InitializeUI()
            lPollutantLoading.Show()
            Handled = True
        End If
    End Sub

End Class
