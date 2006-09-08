Imports atcUtility

Public Class PlugIn
    Implements MapWindow.Interfaces.IPlugin

    Public pMapWin As MapWindow.Interfaces.IMapWin
    Private pParentMenu As MapWindow.Interfaces.MenuItem

    Private Const ParentMenuName As String = "BasinsAnalysis"
    Private Const ParentMenuString As String = "Analysis"

    Public ReadOnly Property Name() As String Implements MapWindow.Interfaces.IPlugin.Name
        Get
            Return "Analysis::Lookup Tables"
        End Get
    End Property

    Public ReadOnly Property Author() As String Implements MapWindow.Interfaces.IPlugin.Author
        Get
            Return "AQUA TERRA Consultants"
        End Get
    End Property

    Public ReadOnly Property SerialNumber() As String Implements MapWindow.Interfaces.IPlugin.SerialNumber
        Get
            Return "G14R/KCU1FOWVVI"
        End Get
    End Property

    Public ReadOnly Property Description() As String Implements MapWindow.Interfaces.IPlugin.Description
        Get
            Return "An interface for viewing the BASINS Lookup Tables."
        End Get
    End Property

    Public ReadOnly Property BuildDate() As String Implements MapWindow.Interfaces.IPlugin.BuildDate
        Get
            Return System.IO.File.GetLastWriteTime(Me.GetType().Assembly.Location)
        End Get
    End Property

    Public ReadOnly Property Version() As String Implements MapWindow.Interfaces.IPlugin.Version
        Get
            Return System.Diagnostics.FileVersionInfo.GetVersionInfo(Me.GetType().Assembly.Location).FileVersion
        End Get
    End Property

    Public Sub Initialize(ByVal MapWin As MapWindow.Interfaces.IMapWin, ByVal ParentHandle As Integer) Implements MapWindow.Interfaces.IPlugin.Initialize
        pMapWin = MapWin
        pParentMenu = pMapWin.Menus(ParentMenuName)
        If pParentMenu Is Nothing Then
            pParentMenu = pMapWin.Menus.AddMenu(ParentMenuName, "", Nothing, ParentMenuString, "mnuFile")
        End If

        pMapWin.Menus.AddMenu(ParentMenuName & "_LookupSeparator", ParentMenuName, Nothing, "-")
        pMapWin.Menus.AddMenu(ParentMenuName & "_Projection", ParentMenuName, Nothing, "Projection Parameters")
        pMapWin.Menus.AddMenu(ParentMenuName & "_Storet", ParentMenuName, Nothing, "STORET Agency Codes")
        pMapWin.Menus.AddMenu(ParentMenuName & "_Sic", ParentMenuName, Nothing, "Standard Industrial Classification Codes")
        pMapWin.Menus.AddMenu(ParentMenuName & "_WQ", ParentMenuName, Nothing, "Water Quality Criteria 304a")
        pMapWin.Menus.AddMenu(ParentMenuName & "_LookupSeparator2", ParentMenuName, Nothing, "-")

    End Sub

    Public Sub Terminate() Implements MapWindow.Interfaces.IPlugin.Terminate
        Try
            pMapWin.Menus.Remove(ParentMenuName & "_LookupSeparator")
            pMapWin.Menus.Remove(ParentMenuName & "_Projection")
            pMapWin.Menus.Remove(ParentMenuName & "_Storet")
            pMapWin.Menus.Remove(ParentMenuName & "_Sic")
            pMapWin.Menus.Remove(ParentMenuName & "_WQ")
            pMapWin.Menus.Remove(ParentMenuName & "_LookupSeparator2")

            If pParentMenu.NumSubItems = 0 Then
                pMapWin.Menus.Remove(ParentMenuName)
            End If
        Catch
            'ignore
        End Try
    End Sub

    Public Sub ItemClicked(ByVal ItemName As String, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.ItemClicked
        'This event fires when a menu item or toolbar button is clicked.  So if you added a button or menu
        'on the Initialize event, then this is where you would handle it.
        If ItemName.StartsWith(ParentMenuName & "_") Then
            Select Case ItemName.Substring(ParentMenuName.Length + 1)
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

    Public Sub LayerRemoved(ByVal Handle As Integer) Implements MapWindow.Interfaces.IPlugin.LayerRemoved
    End Sub

    Public Sub LayersAdded(ByVal Layers() As MapWindow.Interfaces.Layer) Implements MapWindow.Interfaces.IPlugin.LayersAdded
    End Sub

    Public Sub LayersCleared() Implements MapWindow.Interfaces.IPlugin.LayersCleared
    End Sub

    Public Sub LayerSelected(ByVal Handle As Integer) Implements MapWindow.Interfaces.IPlugin.LayerSelected
    End Sub

    Public Sub LegendDoubleClick(ByVal Handle As Integer, ByVal Location As MapWindow.Interfaces.ClickLocation, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.LegendDoubleClick
    End Sub

    Public Sub LegendMouseDown(ByVal Handle As Integer, ByVal Button As Integer, ByVal Location As MapWindow.Interfaces.ClickLocation, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.LegendMouseDown
    End Sub

    Public Sub LegendMouseUp(ByVal Handle As Integer, ByVal Button As Integer, ByVal Location As MapWindow.Interfaces.ClickLocation, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.LegendMouseUp
    End Sub

    Public Sub MapDragFinished(ByVal Bounds As System.Drawing.Rectangle, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.MapDragFinished
    End Sub

    Public Sub MapExtentsChanged() Implements MapWindow.Interfaces.IPlugin.MapExtentsChanged
    End Sub

    Public Sub MapMouseDown(ByVal Button As Integer, ByVal Shift As Integer, ByVal x As Integer, ByVal y As Integer, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.MapMouseDown
    End Sub

    Public Sub MapMouseMove(ByVal ScreenX As Integer, ByVal ScreenY As Integer, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.MapMouseMove
    End Sub

    Public Sub MapMouseUp(ByVal Button As Integer, ByVal Shift As Integer, ByVal x As Integer, ByVal y As Integer, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.MapMouseUp
    End Sub

    Public Sub Message(ByVal msg As String, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.Message
    End Sub

    Public Sub ProjectLoading(ByVal ProjectFile As String, ByVal SettingsString As String) Implements MapWindow.Interfaces.IPlugin.ProjectLoading
    End Sub

    Public Sub ProjectSaving(ByVal ProjectFile As String, ByRef SettingsString As String) Implements MapWindow.Interfaces.IPlugin.ProjectSaving
    End Sub

    Public Sub ShapesSelected(ByVal Handle As Integer, ByVal SelectInfo As MapWindow.Interfaces.SelectInfo) Implements MapWindow.Interfaces.IPlugin.ShapesSelected
    End Sub

End Class
