Imports atcMwGisUtility
Imports atcData
Imports MapWinUtility

Public Class PlugIn
    Implements MapWindow.Interfaces.IPlugin

    Private pMapWin As MapWindow.Interfaces.IMapWin

    Private Const ParentMenuName As String = "BasinsModels"
    Private Const ParentMenuString As String = "Models"

    Public ReadOnly Property Name() As String Implements MapWindow.Interfaces.IPlugin.Name
        Get
            Return "USLE Sediment Estimator (SEDIMENT)"
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
            Return "An interface for generating soil erosion and sediment transport using the Universal Soil Loss Equation (USLE)"
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

        atcDataManager.AddMenuIfMissing(ParentMenuName, "", ParentMenuString, "mnuFile")
        Dim mnu As MapWindow.Interfaces.MenuItem
        mnu = pMapWin.Menus.AddMenu(ParentMenuName & "_SEDIMENT", ParentMenuName, Nothing, "SEDIMENT")
        mnu.Enabled = True
    End Sub

    Public Sub Terminate() Implements MapWindow.Interfaces.IPlugin.Terminate
        pMapWin.Menus.Remove(ParentMenuName & "_SEDIMENT")
        atcDataManager.RemoveMenuIfEmpty(ParentMenuName)
    End Sub

    Public Sub ItemClicked(ByVal ItemName As String, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.ItemClicked
        If ItemName = ParentMenuName & "_SEDIMENT" Then
            GisUtil.MappingObject = pMapWin
            Dim lSediment As New frmSediment
            lSediment.Show()
            Handled = True
        End If
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

    ''' <summary>
    ''' Debugging tool that displays value of currently selected grid or all polygon shape file field values as your mouse moves over them
    ''' This is useful for checking the computed grids and quickly seeing if the grids add up
    ''' </summary>
    ''' <remarks>Turn this off for final version</remarks>
    Public Sub MapMouseMove(ByVal ScreenX As Integer, ByVal ScreenY As Integer, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.MapMouseMove
        If SedimentForm Is Nothing Then Exit Sub
        With pMapWin
            Dim x, y As Double, c, r As Integer
            Dim value As Object = "No Data"
            .View.PixelToProj(ScreenX, ScreenY, x, y)
            Dim lyr As MapWindow.Interfaces.Layer = .Layers(.Layers.CurrentLayer)
            If lyr Is Nothing Then Exit Sub
            If lyr.LayerType = MapWindow.Interfaces.eLayerType.Grid Then 'see if is polygon shapefile
                Dim Grid As MapWinGIS.Grid = lyr.GetGridObject
                Grid.ProjToCell(x, y, c, r)
                If c >= 0 And c <= Grid.Header.NumberCols - 1 And r >= 0 And r <= Grid.Header.NumberRows - 1 Then
                    value = Math.Round(Grid.Value(c, r), 4)
                    If value = Grid.Header.NodataValue Then value = "No Data"
                End If
            ElseIf lyr.LayerType = MapWindow.Interfaces.eLayerType.PolygonShapefile Then
                Dim sf As MapWinGIS.Shapefile = lyr.GetObject
                sf.BeginPointInShapefile()
                Dim i As Integer = sf.PointInShapefile(x, y)
                If i <> -1 Then
                    value = ""
                    For j As Integer = 0 To sf.NumFields - 1
                        value &= IIf(j = 0, "", ", ") & String.Format(sf.Field(j).Name & "={0}", sf.CellValue(j, i))
                    Next
                End If
                sf.EndPointInShapefile()
                'ElseIf lyr.LayerType = MapWindow.Interfaces.eLayerType.Image Then
                '    Dim img As MapWinGIS.Image = lyr.GetObject
                '    With img
                '        c = (x - .XllCenter) / .dX
                '        r = (y - .YllCenter) / .dY
                '        If c >= 0 And c <= .Width - 1 And r >= 0 And r <= .Height - 1 Then
                '            value = Drawing.Color.FromArgb(.Value(r, c)).ToString

                '        End If
                '    End With
            Else
                .StatusBar(2).Text = ""
            End If
            Dim s As String = String.Format("{0} = {1}", lyr.Name, value)
            .StatusBar(2).Text = s
        End With
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
