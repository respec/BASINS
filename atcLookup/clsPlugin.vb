Imports atcUtility
Imports atcData.atcDataManager

Public Class PlugIn
    Inherits atcData.atcDataPlugin

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Analysis::Lookup Tables"
        End Get
    End Property

    Public Overrides ReadOnly Property Description() As String
        Get
            Return "An interface for viewing the BASINS Lookup Tables."
        End Get
    End Property

    Public Overrides Sub Initialize(ByVal MapWin As MapWindow.Interfaces.IMapWin, ByVal ParentHandle As Integer)
        pMapWin = MapWin

        atcData.atcDataManager.AddMenuIfMissing(AnalysisMenuName, "", AnalysisMenuString, "mnuFile")

        pMapWin.Menus.AddMenu(AnalysisMenuName & "_LookupSeparator", AnalysisMenuName, Nothing, "-")
        atcData.atcDataManager.AddMenuIfMissing(AnalysisMenuName & "_Projection", AnalysisMenuName, "Projection Parameters")
        atcData.atcDataManager.AddMenuIfMissing(AnalysisMenuName & "_Storet", AnalysisMenuName, "STORET Agency Codes")
        atcData.atcDataManager.AddMenuIfMissing(AnalysisMenuName & "_Sic", AnalysisMenuName, "Standard Industrial Classification Codes")
        atcData.atcDataManager.AddMenuIfMissing(AnalysisMenuName & "_WQ", AnalysisMenuName, "Water Quality Criteria 304a")
        pMapWin.Menus.AddMenu(AnalysisMenuName & "_LookupSeparator2", AnalysisMenuName, Nothing, "-")
    End Sub

    Public Overrides Sub ItemClicked(ByVal ItemName As String, ByRef Handled As Boolean)
        If ItemName.StartsWith(AnalysisMenuName & "_") Then
            Select Case ItemName.Substring(AnalysisMenuName.Length + 1)
                Case "Projection"
                    Dim main As New frmProjection
                    main.InitializeUI(pMapWin.Project.FileName)
                    main.Show()
                    Handled = True
                Case "Storet"
                    'Dim main As New frmStoret
                    'main.initializeUI(pMapWin.Project.FileName)
                    'main.Show()
                    OpenFile("http://oaspub.epa.gov/stormoda/DW_stationcriteria_STN")
                    Handled = True
                Case "Sic"
                    'Dim main As New frmSic
                    'main.initializeUI(pMapWin.Project.FileName)
                    'main.Show()
                    'main.ReadDatabase()
                    OpenFile("http://www.epa.gov/enviro/html/sic_lkup2.html")
                    Handled = True
                Case "WQ"
                    'Dim main As New frmWQ
                    'main.initializeUI(pMapWin.Project.FileName)
                    'main.Show()
                    OpenFile("http://www.epa.gov/waterscience/criteria/wqcriteria.html")
                    Handled = True
            End Select
        End If
    End Sub
End Class
