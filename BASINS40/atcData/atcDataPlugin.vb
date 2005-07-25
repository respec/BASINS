'atcDataPlugin is a base class for plugins that can
'read, write, manipulate, or display atcTimeseries
'This class implements MapWindow.Interfaces.IPlugin so it can be loaded by the MapWindow plugin code
'Name and Description need to be overridden/shadowed by inheritors. Others may be overridden if desired.
'Additionally, data-related methods are available for inheritors.

Public Class atcDataPlugin
  Implements MapWindow.Interfaces.IPlugin

  'The name that appears in the Plug-ins menu to identify this plug-in.
  'Must be overridden in inheriting class to return something unique or plugin will not load.
  Public Overridable ReadOnly Property Name() As String Implements MapWindow.Interfaces.IPlugin.Name
    Get
      Return ""
    End Get
  End Property

  'A company name, individual, or organization name.
  Public Overridable ReadOnly Property Author() As String Implements MapWindow.Interfaces.IPlugin.Author
    Get
      Return "AQUA TERRA Consultants"
    End Get
  End Property

  'Useful for organizing sets of plugins that should be grouped together in the UI
  'Suggested categories include "File" and "Computation" for atcDataSource
  Public Overridable ReadOnly Property Category() As String
    Get
      Return ""
    End Get
  End Property

  'Longer version of Name with room to expand acronyms
  'Appears in the plug-ins dialog box when a user selects this plug-in.  
  Public Overridable ReadOnly Property Description() As String Implements MapWindow.Interfaces.IPlugin.Description
    Get
      Return ""
    End Get
  End Property

  'This is the Build Date for the plug-in.  You can either return a string of a hard-coded date
  'such as "January 1, 2003" or you can use the .NET function below to dynamically obtain the build
  'date of the assembly.
  Public Overridable ReadOnly Property BuildDate() As String Implements MapWindow.Interfaces.IPlugin.BuildDate
    Get
      Return System.IO.File.GetLastWriteTime(Me.GetType().Assembly.Location).ToString
    End Get
  End Property

  'Can either return a hard-coded string such as "1.0.0.1" or use  
  'GetVersionInfo to dynamically return the version number from the assembly itself.
  Public Overridable ReadOnly Property Version() As String Implements MapWindow.Interfaces.IPlugin.Version
    Get
      Return System.Diagnostics.FileVersionInfo.GetVersionInfo(Me.GetType().Assembly.Location).FileVersion
    End Get
  End Property

  'Fired when the plugin is loaded (by the plug-in dialog, the plugins menu, or at program start).
  'A good time to add buttons to the tool bar or menu items to the menu.  
  'Save a reference to the IMapWin that is passed for later access to MapWindow.
  Public Overridable Sub Initialize(ByVal MapWin As MapWindow.Interfaces.IMapWin, ByVal ParentHandle As Integer) Implements MapWindow.Interfaces.IPlugin.Initialize
  End Sub

  'Fired when the plugin is unloaded (by the plugin dialog, the plugins menu, or on program exit).
  'Remove any buttons from the tool bar or menu items from the menu that this plugin added.
  'If you don't do this, then you will leave dangling menus and buttons that don't do anything.
  Public Overridable Sub Terminate() Implements MapWindow.Interfaces.IPlugin.Terminate
  End Sub

  'Fires when a menu item or toolbar button is clicked.  If this plugin added a button or menu
  'on the Initialize event, this is where those events are handled.
  Public Overridable Sub ItemClicked(ByVal ItemName As String, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.ItemClicked
  End Sub

  'Fires when the user removes a layer from MapWindow.  
  'Useful if this plug-in depends on a particular layer being present,
  'or if it keeps an internal list of layers. 
  Public Overridable Sub LayerRemoved(ByVal Handle As Integer) Implements MapWindow.Interfaces.IPlugin.LayerRemoved
  End Sub

  'Fires when the user adds a layer to MapWindow.  
  'Useful in the same cases as LayerRemoved
  Public Overridable Sub LayersAdded(ByVal Layers() As MapWindow.Interfaces.Layer) Implements MapWindow.Interfaces.IPlugin.LayersAdded
  End Sub

  'Fires when the user clears all of the layers from MapWindow.
  'Useful in the same cases as LayerRemoved
  Public Overridable Sub LayersCleared() Implements MapWindow.Interfaces.IPlugin.LayersCleared
  End Sub

  'Fires when a user selects a layer in the legend. 
  Public Overridable Sub LayerSelected(ByVal Handle As Integer) Implements MapWindow.Interfaces.IPlugin.LayerSelected
  End Sub

  'Fires when a user double-clicks a layer in the legend.
  Public Overridable Sub LegendDoubleClick(ByVal Handle As Integer, ByVal Location As MapWindow.Interfaces.ClickLocation, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.LegendDoubleClick
  End Sub

  'Fires when a user holds a mouse button down in the legend.
  Public Overridable Sub LegendMouseDown(ByVal Handle As Integer, ByVal Button As Integer, ByVal Location As MapWindow.Interfaces.ClickLocation, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.LegendMouseDown
  End Sub

  'Fires when a user releases a mouse button in the legend.
  Public Overridable Sub LegendMouseUp(ByVal Handle As Integer, ByVal Button As Integer, ByVal Location As MapWindow.Interfaces.ClickLocation, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.LegendMouseUp
  End Sub

  'Fires after a user drags (ie draws a box) with the mouse on the map, at completion of the drag.
  'Bounds specifies the box that was "drawn"
  Public Overridable Sub MapDragFinished(ByVal Bounds As System.Drawing.Rectangle, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.MapDragFinished
  End Sub

  'Fires any time there is a zoom or pan that changes the extents of the map.
  Public Overridable Sub MapExtentsChanged() Implements MapWindow.Interfaces.IPlugin.MapExtentsChanged
  End Sub

  'Fires when the user presses a mouse button on the map. Note that x and y are returned
  'as screen coordinates (in pixels), not map coordinates.  If map coordinates are needed,
  'use g_MapWin.View.PixelToProj() where g_MapWin is a saved reference to MapWin from Initialize
  Public Overridable Sub MapMouseDown(ByVal Button As Integer, ByVal Shift As Integer, _
                          ByVal ScreenX As Integer, ByVal ScreenY As Integer, _
                          ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.MapMouseDown
  End Sub

  'Fires when the user moves the mouse over the map. See MapMouseDown.
  Public Overridable Sub MapMouseMove(ByVal ScreenX As Integer, ByVal ScreenY As Integer, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.MapMouseMove
  End Sub

  'Fires when the user releases a mouse button over the map. See MapMouseDown.
  Public Overridable Sub MapMouseUp(ByVal Button As Integer, ByVal Shift As Integer, _
                        ByVal ScreenX As Integer, ByVal ScreenY As Integer, _
                        ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.MapMouseUp
  End Sub

  'Plugins can communicate with each other using Messages.  If a message is sent then this event fires.
  'Set Handled=True to stop the message from being sent to any more plugins.
  Public Overridable Sub Message(ByVal msg As String, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.Message
  End Sub

  'Fires when a project is opened in MapWindow.
  'ProjectFile is the file name including path of the project that was opened.
  'SettingsString contains the string of data that is connected to this plugin and was 
  'stored in the project in a ProjectSaving event.
  Public Overridable Sub ProjectLoading(ByVal ProjectFile As String, ByVal SettingsString As String) Implements MapWindow.Interfaces.IPlugin.ProjectLoading
  End Sub

  'Fires when a project is saved in MapWindow.
  'ProjectFile is the file name including path of the project that is being saved.
  'SettingsString can be set to any string of data that is connected to this plugin
  'which should be stored in the project file.
  Public Overridable Sub ProjectSaving(ByVal ProjectFile As String, ByRef SettingsString As String) Implements MapWindow.Interfaces.IPlugin.ProjectSaving
  End Sub

  'Fires when the user selects one or more shapes using the select tool in MapWindow.
  'Handle is the Layer handle for the shapefile on which shapes were selected. 
  'SelectInfo holds information about the shapes that were selected. 
  Public Overridable Sub ShapesSelected(ByVal Handle As Integer, ByVal SelectInfo As MapWindow.Interfaces.SelectInfo) Implements MapWindow.Interfaces.IPlugin.ShapesSelected
  End Sub

  'Leftover part of IPlugin interface no longer in use
  Public Overridable ReadOnly Property SerialNumber() As String Implements MapWindow.Interfaces.IPlugin.SerialNumber
    Get
      Return ""
    End Get
  End Property

End Class
