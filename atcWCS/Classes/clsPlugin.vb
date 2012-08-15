Imports atcMwGisUtility
Imports atcData
Imports MapWinUtility

'The "Inherits" approach is the preferred method of creating a plugin, however MW continues to warn about a missing plugin when this method is used
'Therefore, revert to "Implements" method

'Public Class atcWCSPlugin
'    Inherits atcData.atcDataPlugin

'    Private Const ParentMenuName As String = "BasinsAnalysis"
'    Private Const ParentMenuString As String = "Analysis"

'    Public Overrides ReadOnly Property Name() As String
'        Get
'            Return "Watershed Characterization System (WCS)"
'        End Get
'    End Property

'    Public Overrides ReadOnly Property Author() As String
'        Get
'            Return "AQUA TERRA Consultants/Clayton Engineering"
'        End Get
'    End Property

'    Public Overrides ReadOnly Property Description() As String
'        Get
'            Return "An interface for generating watershed reports and providing various utilities."
'        End Get
'    End Property

'    <CLSCompliant(False)> _
'    Public Overrides Sub Initialize(ByVal aMapWin As MapWindow.Interfaces.IMapWin, _
'                                    ByVal aParentHandle As Integer)
'        MyBase.Initialize(aMapWin, aParentHandle)
'        pMapWin = aMapWin
'        gMapWin = aMapWin
'        MapWindowForm = Control.FromHandle(New IntPtr(aParentHandle))

'        atcDataManager.AddMenuIfMissing(ParentMenuName, "", ParentMenuString, "mnuFile")
'        Dim mnu As MapWindow.Interfaces.MenuItem
'        mnu = pMapWin.Menus.AddMenu(ParentMenuName & "_WCS", ParentMenuName, Nothing, "Watershed Characterization System (WCS)")
'        mnu.Enabled = True
'    End Sub

'    Public Overrides Sub Terminate()
'        pMapWin.Menus.Remove(ParentMenuName & "_WCS")
'        atcDataManager.RemoveMenuIfEmpty(ParentMenuName)
'    End Sub

'    Public Overrides Sub ItemClicked(ByVal aItemName As String, ByRef aHandled As Boolean)
'        'If Not aItemName.Contains("WCS") Then Exit Sub
'        If aItemName = ParentMenuName & "_WCS" Then 'menu item: display or hide toolbar
'            If String.IsNullOrEmpty(GisUtil.ProjectFileName) Then
'                Logger.Message("You must load a BASINS project before starting WCS.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error, DialogResult.OK)
'                Exit Sub
'            End If
'            With pMapWin.Menus.Item(aItemName)
'                GisUtil.MappingObject = pMapWin
'                WCSForm = New frmWCS
'                WCSForm.Show()
'            End With
'        End If
'        aHandled = True
'    End Sub

'    Public Overrides Sub MapMouseMove(ByVal aScreenX As Integer, ByVal aScreenY As Integer, ByRef aHandled As Boolean)
'        MyBase.MapMouseMove(aScreenX, aScreenY, aHandled)
'        If WCSForm Is Nothing Then Exit Sub
'        With pMapWin
'            Dim x, y As Double, c, r As Integer
'            Dim value As Object = "No Data"
'            .View.PixelToProj(aScreenX, aScreenY, x, y)
'            Dim lyr As MapWindow.Interfaces.Layer = .Layers(.Layers.CurrentLayer)
'            If lyr Is Nothing Then Exit Sub
'            If lyr.LayerType = MapWindow.Interfaces.eLayerType.Grid Then 'see if is polygon shapefile
'                Dim Grid As MapWinGIS.Grid = lyr.GetGridObject
'                Grid.ProjToCell(x, y, c, r)
'                If c >= 0 And c <= Grid.Header.NumberCols - 1 And r >= 0 And r <= Grid.Header.NumberRows - 1 Then
'                    'note: it will return a double, even if is integer grid!, also, seem to be off by 1 digit from nodatavalue (e.g., -2147483648 vs -2147483647)
'                    If Grid.DataType = MapWinGIS.GridDataType.LongDataType Then
'                        value = CInt(Grid.Value(c, r))
'                    Else
'                        value = Math.Round(Grid.Value(c, r), 4)
'                    End If
'                    If value = Grid.Header.NodataValue Or value < Integer.MinValue + 2 Then value = "No Data"
'                End If
'            ElseIf lyr.LayerType = MapWindow.Interfaces.eLayerType.PolygonShapefile Then
'                Dim sf As MapWinGIS.Shapefile = lyr.GetObject
'                sf.BeginPointInShapefile()
'                Dim i As Integer = sf.PointInShapefile(x, y)
'                If i <> -1 Then
'                    value = ""
'                    For j As Integer = 0 To sf.NumFields - 1
'                        value &= IIf(j = 0, "", ", ") & String.Format(sf.Field(j).Name & "={0}", sf.CellValue(j, i))
'                    Next
'                End If
'                sf.EndPointInShapefile()
'            Else
'                .StatusBar(2).Text = ""
'            End If
'            Dim s As String = String.Format("{0}({1},{2}) = {3}", lyr.Name, r, c, value)
'            .StatusBar(2).Text = s
'        End With
'    End Sub

'End Class

Public Class PlugIn
    Implements MapWindow.Interfaces.IPlugin

    Private pMapWin As MapWindow.Interfaces.IMapWin

    Private Const ParentMenuName As String = "BasinsAnalysis"
    Private Const ParentMenuString As String = "Analysis"

    Public ReadOnly Property Name() As String Implements MapWindow.Interfaces.IPlugin.Name
        Get
            Return "Watershed Characterization System (WCS)"
        End Get
    End Property

    Public ReadOnly Property Author() As String Implements MapWindow.Interfaces.IPlugin.Author
        Get
            Return "AQUA TERRA Consultants/Clayton Engineering"
        End Get
    End Property

    Public ReadOnly Property SerialNumber() As String Implements MapWindow.Interfaces.IPlugin.SerialNumber
        Get
            Return "G14R/KCU1FOWVVI"
        End Get
    End Property

    Public ReadOnly Property Description() As String Implements MapWindow.Interfaces.IPlugin.Description
        Get
            Return "An interface for generating watershed reports and providing various utilities."
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

    <CLSCompliant(False)> _
    Public Sub Initialize(ByVal MapWin As MapWindow.Interfaces.IMapWin, ByVal ParentHandle As Integer) Implements MapWindow.Interfaces.IPlugin.Initialize
        pMapWin = MapWin
        gMapWin = MapWin
        atcDataManager.MapWindow = MapWin
        MapWindowForm = Control.FromHandle(New IntPtr(ParentHandle))

        atcDataManager.AddMenuIfMissing(ParentMenuName, "", ParentMenuString, "mnuFile")
        Dim mnu As MapWindow.Interfaces.MenuItem = atcDataManager.AddMenuIfMissing(ParentMenuName & "_WCS", ParentMenuName, "Watershed Characterization System (WCS)")
        mnu.Enabled = True
    End Sub

    Public Sub Terminate() Implements MapWindow.Interfaces.IPlugin.Terminate
        pMapWin.Menus.Remove(ParentMenuName & "_WCS")
        atcDataManager.RemoveMenuIfEmpty(ParentMenuName)
    End Sub

    Public Sub ItemClicked(ByVal ItemName As String, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.ItemClicked
        If Not ItemName.Contains("WCS") Then Exit Sub
        If ItemName = ParentMenuName & "_WCS" Then
            If String.IsNullOrEmpty(GisUtil.ProjectFileName) Then
                Logger.Message("You must load a BASINS project before starting WCS.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error, DialogResult.OK)
                Exit Sub
            End If
            With pMapWin.Menus.Item(ItemName)
                GisUtil.MappingObject = pMapWin
                WCSForm = New frmWCS
                WCSForm.Show()
            End With
        End If
        Handled = True
    End Sub

    Public Sub LayerRemoved(ByVal Handle As Integer) Implements MapWindow.Interfaces.IPlugin.LayerRemoved
    End Sub

    <CLSCompliant(False)> _
    Public Sub LayersAdded(ByVal Layers() As MapWindow.Interfaces.Layer) Implements MapWindow.Interfaces.IPlugin.LayersAdded
    End Sub

    Public Sub LayersCleared() Implements MapWindow.Interfaces.IPlugin.LayersCleared
    End Sub

    Public Sub LayerSelected(ByVal Handle As Integer) Implements MapWindow.Interfaces.IPlugin.LayerSelected
    End Sub

    <CLSCompliant(False)> _
    Public Sub LegendDoubleClick(ByVal Handle As Integer, ByVal Location As MapWindow.Interfaces.ClickLocation, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.LegendDoubleClick
    End Sub

    <CLSCompliant(False)> _
    Public Sub LegendMouseDown(ByVal Handle As Integer, ByVal Button As Integer, ByVal Location As MapWindow.Interfaces.ClickLocation, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.LegendMouseDown
    End Sub

    <CLSCompliant(False)> _
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

    <CLSCompliant(False)> _
    Public Sub ShapesSelected(ByVal Handle As Integer, ByVal SelectInfo As MapWindow.Interfaces.SelectInfo) Implements MapWindow.Interfaces.IPlugin.ShapesSelected
    End Sub

End Class
