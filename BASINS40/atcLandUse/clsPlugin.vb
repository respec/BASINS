Imports atcMwGisUtility
Imports atcData.atcDataManager

Public Class PlugIn
    Inherits atcData.atcDataPlugin

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Analysis::Reclassify Land Use"
        End Get
    End Property

    Public Overrides ReadOnly Property Description() As String
        Get
            Return "An interface for reclassifying land use."
        End Get
    End Property

    <CLSCompliant(False)> _
    Public Overrides Sub Initialize(ByVal MapWin As MapWindow.Interfaces.IMapWin, ByVal ParentHandle As Integer)
        pMapWin = MapWin

        atcData.atcDataManager.AddMenuIfMissing(AnalysisMenuName, "", AnalysisMenuString, "mnuFile")
        atcData.atcDataManager.AddMenuIfMissing(AnalysisMenuName & "_LandUse", AnalysisMenuName, "Reclassify Land Use")
    End Sub

    Public Overrides Sub ItemClicked(ByVal ItemName As String, ByRef Handled As Boolean)
        If ItemName = AnalysisMenuName & "_LandUse" Then
            GisUtil.MappingObject = pMapWin
            Dim main As New frmLandUse
            main.InitializeUI()
            main.Show()
            Handled = True
        End If
    End Sub
End Class
