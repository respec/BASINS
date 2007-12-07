Public Class PlugIn
    Implements MapWindow.Interfaces.IPlugin

    Private pMapWin As MapWindow.Interfaces.IMapWin
    Private pDraw As MapWindow.Interfaces.Draw
    Private pMain As frmManDelin
    Private pInitialized As Boolean

    'TODO: get these 3 from BASINS4 or plugInManager?
    Private Const DelineateMenuName As String = "btdmWatershedDelin"
    Private Const DelineateMenuString As String = "Watershed Delineation"

    Public ReadOnly Property Name() As String Implements MapWindow.Interfaces.IPlugin.Name
        'This is one of the more important plug-in properties because if it is not set to something then
        'your plug-in will not load at all. This is the name that appears in the Plug-ins menu to identify
        'this plug-in.
        Get
            Return "Manual Delineation"
        End Get
    End Property

    Public ReadOnly Property Author() As String Implements MapWindow.Interfaces.IPlugin.Author
        'This is the author of the plug-in.  It can be a company name, individual, or organization name.
        'You can use the license generator to generate a plug-in license, or use the name:
        '"Open Source Author"
        Get
            Return "AQUA TERRA Consultants"
        End Get
    End Property

    Public ReadOnly Property SerialNumber() As String Implements MapWindow.Interfaces.IPlugin.SerialNumber
        'This is the plug-in serial number and it corresponds with a particular author name (see the Author property).  
        'You can generate a serial number for your author name using the license generator, or use the serial number:
        'P/3I39l+3m8dSpr   - this corrsponds to the Author "Open Source Author"
        Get
            Return "G14R/KCU1FOWVVI"
        End Get
    End Property

    Public ReadOnly Property Description() As String Implements MapWindow.Interfaces.IPlugin.Description
        'This is a description of the plug-in.  It appears in the plug-ins dialog box when a user selects
        'your plug-in.  
        Get
            Return "This plug-in provides an interface for manually delineating watersheds."
        End Get
    End Property

    Public ReadOnly Property BuildDate() As String Implements MapWindow.Interfaces.IPlugin.BuildDate
        'This is the Build Date for the plug-in.  You can either return a string of a hard-coded date
        'such as "January 1, 2003" or you can use the .NET function below to dynamically obtain the build
        'date of the assembly.
        Get
            Return System.IO.File.GetLastWriteTime(Me.GetType().Assembly.Location)
        End Get
    End Property

    Public ReadOnly Property Version() As String Implements MapWindow.Interfaces.IPlugin.Version
        'This is the version number of the plug-in.  You can either return a hard-coded string
        'such as "1.0.0.1" or you can use the .NET function shown below to dynamically return 
        'the version number from the assembly itself.
        Get
            Return System.Diagnostics.FileVersionInfo.GetVersionInfo(Me.GetType().Assembly.Location).FileVersion
        End Get
    End Property

    <CLSCompliant(False)> _
    Public Sub Initialize(ByVal MapWin As MapWindow.Interfaces.IMapWin, ByVal ParentHandle As Integer) Implements MapWindow.Interfaces.IPlugin.Initialize
        Dim mnu As MapWindow.Interfaces.MenuItem

        pMapWin = MapWin
        pMapWin.Menus.AddMenu(DelineateMenuName, "", Nothing, DelineateMenuString, "mnuFile")
        mnu = pMapWin.Menus.AddMenu(DelineateMenuName & "_ManDelin", DelineateMenuName, Nothing, "&Manual")
        pInitialized = False

    End Sub

    Public Sub Terminate() Implements MapWindow.Interfaces.IPlugin.Terminate
    End Sub

    Public Sub ItemClicked(ByVal ItemName As String, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.ItemClicked
        'This event fires when a menu item or toolbar button is clicked.  So if you added a button or menu
        'on the Initialize event, then this is where you would handle it.

        If ItemName = DelineateMenuName & "_ManDelin" Then
            Dim lCreateNew As Boolean = True
            If Not pMain Is Nothing Then
                If pMain.Visible = True Then
                    pMain.BringToFront()
                    lCreateNew = False
                End If
            End If
            If lCreateNew Then
                Dim main As New frmManDelin
                main.Initialize(pMapWin)
                main.Show()
                Handled = True
                pMain = main
                pInitialized = True
            End If
        End If
    End Sub

    Public Sub TestClip()
        Dim lineShapefile As New MapWinGIS.Shapefile
        Dim lineShape As New MapWinGIS.Shape
        Dim polyShapefile As New MapWinGIS.Shapefile
        Dim polyShape As New MapWinGIS.Shape
        Dim success As Boolean

        success = lineShapefile.Open("c:\temp\temp1.shp")
        lineShape = lineShapefile.Shape(0)

        success = polyShapefile.Open("c:\temp\temp2.shp")
        polyShape = polyShapefile.Shape(0)

        success = MapWinGeoProc.SpatialOperations.ClipPolygonWithLine(polyShape, lineShape, "c:\temp\temp.shp")

    End Sub

    Public Sub LayerRemoved(ByVal Handle As Integer) Implements MapWindow.Interfaces.IPlugin.LayerRemoved
        'This event fires when the user removes a layer from MapWindow.  This is useful to know if your
        'plug-in depends on a particular layer being present. 
    End Sub

    <CLSCompliant(False)> _
    Public Sub LayersAdded(ByVal Layers() As MapWindow.Interfaces.Layer) Implements MapWindow.Interfaces.IPlugin.LayersAdded
        'This event fires when the user adds a layer to MapWindow.  This is useful to know if your
        'plug-in depends on a particular layer being present. Also, if you keep an internal list of 
        'available layers, for example you may be keeping a list of all "point" shapefiles, then you
        'would use this event to know when layers have been added or removed.
    End Sub

    Public Sub LayersCleared() Implements MapWindow.Interfaces.IPlugin.LayersCleared
        'This event fires when the user clears all of the layers from MapWindow.  As with LayersAdded 
        'and LayersRemoved, this is useful to know if your plug-in depends on a particular layer being 
        'present or if you are maintaining your own list of layers.
    End Sub

    Public Sub LayerSelected(ByVal Handle As Integer) Implements MapWindow.Interfaces.IPlugin.LayerSelected
        'This event fires when a user selects a layer in the legend. 
    End Sub

    <CLSCompliant(False)> _
    Public Sub LegendDoubleClick(ByVal Handle As Integer, ByVal Location As MapWindow.Interfaces.ClickLocation, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.LegendDoubleClick
        'This event fires when a user double-clicks a layer in the legend.
    End Sub

    <CLSCompliant(False)> _
    Public Sub LegendMouseDown(ByVal Handle As Integer, ByVal Button As Integer, ByVal Location As MapWindow.Interfaces.ClickLocation, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.LegendMouseDown
        'This event fires when a user holds a mouse button down in the legend.
    End Sub

    <CLSCompliant(False)> _
    Public Sub LegendMouseUp(ByVal Handle As Integer, ByVal Button As Integer, ByVal Location As MapWindow.Interfaces.ClickLocation, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.LegendMouseUp
        'This event fires when a user releases a mouse button in the legend.
    End Sub

    Public Sub MapDragFinished(ByVal Bounds As System.Drawing.Rectangle, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.MapDragFinished
        'If a user drags (ie draws a box) with the mouse on the map, this event fires at completion of the drag
        'and returns a system.drawing.rectangle that has the bounds of the box that was "drawn"
    End Sub

    Public Sub MapExtentsChanged() Implements MapWindow.Interfaces.IPlugin.MapExtentsChanged
        'This event fires any time there is a zoom or pan that changes the extents of the map.
    End Sub

    Public Sub MapMouseDown(ByVal Button As Integer, ByVal Shift As Integer, ByVal x As Integer, ByVal y As Integer, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.MapMouseDown
        'This event fires when the user holds a mouse button down on the map. Note that x and y are returned
        'as screen coordinates (in pixels), not map coordinates.  So if you really need the map coordinates
        'then you need to use g_MapWin.View.PixelToProj()
    End Sub

    Public Sub MapMouseMove(ByVal ScreenX As Integer, ByVal ScreenY As Integer, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.MapMouseMove
        'This event fires when the user moves the mouse over the map. Note that x and y are returned
        'as screen coordinates (in pixels), not map coordinates.  So if you really need the map coordinates
        'then you need to use g_MapWin.View.PixelToProj()
        Dim px As Double
        Dim py As Double
        If pInitialized Then
            pMapWin.View.PixelToProj(ScreenX, ScreenY, px, py)
            pMain.MouseDrawingMove(px, py)
        End If
    End Sub

    Public Sub MapMouseUp(ByVal Button As Integer, ByVal Shift As Integer, ByVal x As Integer, ByVal y As Integer, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.MapMouseUp
        'This event fires when the user releases a mouse button down on the map. Note that x and y are returned
        'as screen coordinates (in pixels), not map coordinates.  So if you really need the map coordinates
        'then you need to use g_MapWin.View.PixelToProj()
        Dim px As Double
        Dim py As Double
        If pInitialized Then
            pMapWin.View.PixelToProj(x, y, px, py)
            pMain.MouseButtonClickUp(px, py, Button)
        End If
    End Sub

    Public Sub Message(ByVal msg As String, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.Message
        'Plug-ins can communicate with eachother using Messages.  If a message is sent then this event fires.
        'If you know the message is "for you" then you can set Handled=True and then it will not be sent to any
        'other plug-ins.
    End Sub

    Public Sub ProjectLoading(ByVal ProjectFile As String, ByVal SettingsString As String) Implements MapWindow.Interfaces.IPlugin.ProjectLoading
        'When the user opens a project in MapWindow, this event fires.  The ProjectFile is the file name of the
        'project that the user opened (including its path in case that is important for this this plug-in to know).
        'The SettingsString variable contains any string of data that is connected to this plug-in but is stored 
        'on a project level. For example, a plug-in that shows streamflow data might allow the user to set a 
        'separate database for each project (i.e. one database for the upper Missouri River Basin, a different 
        'one for the Lower Colorado Basin.) In this case, the plug-in would store the database name in the 
        'SettingsString of the project. 
    End Sub

    Public Sub ProjectSaving(ByVal ProjectFile As String, ByRef SettingsString As String) Implements MapWindow.Interfaces.IPlugin.ProjectSaving
        'When the user saves a project in MapWindow, this event fires.  The ProjectFile is the file name of the
        'project that the user is saving (including its path in case that is important for this this plug-in to know).
        'The SettingsString variable contains any string of data that is connected to this plug-in but is stored 
        'on a project level. For example, a plug-in that shows streamflow data might allow the user to set a 
        'separate database for each project (i.e. one database for the upper Missouri River Basin, a different 
        'one for the Lower Colorado Basin.) In this case, the plug-in would store the database name in the 
        'SettingsString of the project. 
    End Sub

    <CLSCompliant(False)> _
    Public Sub ShapesSelected(ByVal Handle As Integer, ByVal SelectInfo As MapWindow.Interfaces.SelectInfo) Implements MapWindow.Interfaces.IPlugin.ShapesSelected
        'This event fires when the user selects one or more shapes using the select tool in MapWindow. Handle is the 
        'Layer handle for the shapefile on which shapes were selected. SelectInfo holds information abou the 
        'shapes that were selected. 
    End Sub

End Class
