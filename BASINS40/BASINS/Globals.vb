Module Globals
  'Declare this as global so that it can be accessed throughout the plug-in project.
  'The variable is initialized in the plugin_Initialize event.
  Public g_MapWin As MapWindow.Interfaces.IMapWin
  Public g_MapWinWindowHandle As Integer
  Public g_AppName As String = "BASINS4"
  Public g_BasinsDrives As String = ""
End Module
