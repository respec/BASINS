'********************************************************************************************************
'File Name: frmScript.vb
'Description: MapWindow Script System
'********************************************************************************************************
'The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
'you may not use this file except in compliance with the License. You may obtain a copy of the License at 
'http://www.mozilla.org/MPL/ 
'Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
'ANY KIND, either express or implied. See the License for the specificlanguage governing rights and 
'limitations under the License. 
'
' The original code is
' Adapted for MapWindow by Chris Michaelis (cmichaelis@happysquirrel.com) on Jan 1 2006
' Originally created by Mark Gray of AquaTerra Consultants
'
'The Initial Developer of this version of the Original Code is Christopher Michaelis, done
'by reshifting and moving about the various utility functions from MapWindow's modPublic.vb
'(which no longer exists) and some useful utility functions from Aqua Terra Consulting.
'
'Contributor(s): (Open source contributors should list themselves and their modifications here). 
'Feb 8 2006 - Chris Michaelis - Added the "Online Script Directory" features.
'8/9/2006 - Paul Meems (pm) - Started Duth translation
'********************************************************************************************************

Imports System.CodeDom.Compiler
Imports System.Reflection
Imports System.Drawing
Imports System.Windows.forms
Imports System.IO
Imports System.Threading

Public Class frmScript
    Inherits System.Windows.Forms.Form

#Region "Declarations"
    'PM
    'Private pResources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmMain))
    Public Declare Function LockWindowUpdate Lib "user32" (ByVal hWnd As Integer) As Integer
#End Region

    Private pDefaultScript_VB As String = _
            "Imports MapWindow.Interfaces" & vbCrLf _
          & "Imports MapWinGIS" & vbCrLf _
          & "Imports System.Windows.Forms" & vbCrLf _
          & "Imports Microsoft.VisualBasic" & vbCrLf _
          & "Imports System" & vbCrLf _
          & vbCrLf _
          & "'Each script should (but doesn't have to) have a unique name. Change MyExample here to something meaningful. ScriptMain should remain as ""Main"" however." & vbCrLf _
          & "Public Module MyExample" & vbCrLf _
          & "  Public Sub ScriptMain(ByRef m_MapWin As IMapWin)" & vbCrLf _
          & "    mapwinutility.logger.msg(""This is a simple script to display the number of loaded layers in the map."")" & vbCrLf _
          & "    mapwinutility.logger.msg(""Number of Layers: "" & m_MapWin.Layers.NumLayers)" & vbCrLf _
          & "  End Sub" & vbCrLf _
          & "End Module"

    Private pDefaultPlugin_VB As String = _
          "Imports MapWindow.Interfaces" & vbCrLf _
          & "Imports MapWinGIS" & vbCrLf _
          & "Imports System.Windows.Forms" & vbCrLf _
          & "Imports Microsoft.VisualBasic" & vbCrLf _
          & "Imports System" & vbCrLf & _
          vbCrLf & _
        "' You must change the name of this class to something unique!" & vbCrLf & _
        "Public Class MyPlugin" + vbCrLf & _
        "    Implements MapWindow.Interfaces.IPlugin" + vbCrLf & _
        "" + vbCrLf & _
        "    Public g_MapWin As MapWindow.Interfaces.IMapWin" + vbCrLf & _
        "" + vbCrLf & _
        "    Public ReadOnly Property Name() As String Implements MapWindow.Interfaces.IPlugin.Name" + vbCrLf & _
        "        'This is one of the more important plug-in properties because if it is not set to something then" + vbCrLf & _
        "        'your plug-in will not load at all. This is the name that appears in the Plug-ins menu to identify" + vbCrLf & _
        "        'this plug-in." + vbCrLf & _
        "        Get" + vbCrLf & _
        "            Return ""My New Plugin""" + vbCrLf & _
        "        End Get" + vbCrLf & _
        "    End Property" + vbCrLf & _
        "" + vbCrLf & _
        "    Public ReadOnly Property Author() As String Implements MapWindow.Interfaces.IPlugin.Author" + vbCrLf & _
        "        'This is the author of the plug-in.  Licensees recieve a Serial Number that corresponds with" + vbCrLf & _
        "        'this Author name.  It can be a company name, individual, or organization name." + vbCrLf & _
        "        Get" + vbCrLf & _
        "            Return ""My Name""" + vbCrLf & _
        "        End Get" + vbCrLf & _
        "    End Property" + vbCrLf & _
        "" + vbCrLf & _
        "    Public ReadOnly Property SerialNumber() As String Implements MapWindow.Interfaces.IPlugin.SerialNumber" + vbCrLf & _
        "        'This is the plug-in serial number. " + vbCrLf & _
        "        'This is no longer needed, but the property must remain for backward compatibility." + vbCrLf & _
        "        Get" + vbCrLf & _
        "            Return """"" + vbCrLf & _
        "        End Get" + vbCrLf & _
        "    End Property" + vbCrLf & _
        "" + vbCrLf & _
        "    Public ReadOnly Property Description() As String Implements MapWindow.Interfaces.IPlugin.Description" + vbCrLf & _
        "        'This is a description of the plug-in.  It appears in the plug-ins dialog box when a user selects" + vbCrLf & _
        "        'your plug-in.  " + vbCrLf & _
        "        Get" + vbCrLf & _
        "            Return ""This is an example plug-in.""" + vbCrLf & _
        "        End Get" + vbCrLf & _
        "    End Property" + vbCrLf & _
        "" + vbCrLf & _
        "    Public ReadOnly Property BuildDate() As String Implements MapWindow.Interfaces.IPlugin.BuildDate" + vbCrLf & _
        "        'This is the Build Date for the plug-in.  You can either return a string of a hard-coded date" + vbCrLf & _
        "        'such as ""January 1, 2003"" or you can use the .NET function below to dynamically obtain the build" + vbCrLf & _
        "        'date of the assembly." + vbCrLf & _
        "        Get" + vbCrLf & _
        "            Return System.IO.File.GetLastWriteTime(Me.GetType().Assembly.Location)" + vbCrLf & _
        "        End Get" + vbCrLf & _
        "    End Property" + vbCrLf & _
        "" + vbCrLf & _
        "    Public ReadOnly Property Version() As String Implements MapWindow.Interfaces.IPlugin.Version" + vbCrLf & _
        "        'This is the version number of the plug-in.  You can either return a hard-coded string" + vbCrLf & _
        "        'such as ""1.0.0.1"" or you can use the .NET function shown below to dynamically return " + vbCrLf & _
        "        'the version number from the assembly itself." + vbCrLf & _
        "        Get" + vbCrLf & _
        "            Return System.Diagnostics.FileVersionInfo.GetVersionInfo(Me.GetType().Assembly.Location).FileVersion" + vbCrLf & _
        "        End Get" + vbCrLf & _
        "    End Property" + vbCrLf & _
        "" + vbCrLf & _
        "    Public Sub Initialize(ByVal MapWin As MapWindow.Interfaces.IMapWin, ByVal ParentHandle As Integer) Implements MapWindow.Interfaces.IPlugin.Initialize" + vbCrLf & _
        "        'This event is fired when the user loads your plug-in either through the plug-in dialog " + vbCrLf & _
        "        'box, or by checkmarking it in the plug-ins menu.  This is where you would add buttons to the" + vbCrLf & _
        "        'tool bar or menu items to the menu.  " + vbCrLf & _
        "        '" + vbCrLf & _
        "        'It is also standard to set a global reference to the IMapWin that is passed through here so that" + vbCrLf & _
        "        'you can access it elsewhere in your project to act on MapWindow." + vbCrLf & _
        "        g_MapWin = MapWin" + vbCrLf & _
        "    End Sub" + vbCrLf & _
        "" + vbCrLf & _
        "    Public Sub Terminate() Implements MapWindow.Interfaces.IPlugin.Terminate" + vbCrLf & _
        "        'This event is fired when the user unloads your plug-in either through the plug-in dialog " + vbCrLf & _
        "        'box, or by un-checkmarking it in the plug-ins menu.  This is where you would remove any" + vbCrLf & _
        "        'buttons from the tool bar tool bar or menu items from the menu that you may have added." + vbCrLf & _
        "        'If you don't do this, then you will leave dangling menus and buttons that don't do anything." + vbCrLf & _
        "    End Sub" + vbCrLf & _
        "" + vbCrLf & _
        "    Public Sub ItemClicked(ByVal ItemName As String, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.ItemClicked" + vbCrLf & _
        "        'This event fires when a menu item or toolbar button is clicked.  So if you added a button or menu" + vbCrLf & _
        "        'on the Initialize event, then this is where you would handle it." + vbCrLf & _
        "    End Sub" + vbCrLf & _
        "" + vbCrLf & _
        "    Public Sub LayerRemoved(ByVal Handle As Integer) Implements MapWindow.Interfaces.IPlugin.LayerRemoved" + vbCrLf & _
        "        'This event fires when the user removes a layer from MapWindow.  This is useful to know if your" + vbCrLf & _
        "        'plug-in depends on a particular layer being present. " + vbCrLf & _
        "    End Sub" + vbCrLf & _
        "" + vbCrLf & _
        "    Public Sub LayersAdded(ByVal Layers() As MapWindow.Interfaces.Layer) Implements MapWindow.Interfaces.IPlugin.LayersAdded" + vbCrLf & _
        "        'This event fires when the user adds a layer to MapWindow.  This is useful to know if your" + vbCrLf & _
        "        'plug-in depends on a particular layer being present. Also, if you keep an internal list of " + vbCrLf & _
        "        'available layers, for example you may be keeping a list of all ""point"" shapefiles, then you" + vbCrLf & _
        "        'would use this event to know when layers have been added or removed." + vbCrLf & _
        "    End Sub" + vbCrLf & _
        "" + vbCrLf & _
        "    Public Sub LayersCleared() Implements MapWindow.Interfaces.IPlugin.LayersCleared" + vbCrLf & _
        "        'This event fires when the user clears all of the layers from MapWindow.  As with LayersAdded " + vbCrLf & _
        "        'and LayersRemoved, this is useful to know if your plug-in depends on a particular layer being " + vbCrLf & _
        "        'present or if you are maintaining your own list of layers." + vbCrLf & _
        "    End Sub" + vbCrLf & _
        "" + vbCrLf & _
        "    Public Sub LayerSelected(ByVal Handle As Integer) Implements MapWindow.Interfaces.IPlugin.LayerSelected" + vbCrLf & _
        "        'This event fires when a user selects a layer in the legend. " + vbCrLf & _
        "    End Sub" + vbCrLf & _
        "" + vbCrLf & _
        "    Public Sub LegendDoubleClick(ByVal Handle As Integer, ByVal Location As MapWindow.Interfaces.ClickLocation, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.LegendDoubleClick" + vbCrLf & _
        "        'This event fires when a user double-clicks a layer in the legend." + vbCrLf & _
        "    End Sub" + vbCrLf & _
        "" + vbCrLf & _
        "    Public Sub LegendMouseDown(ByVal Handle As Integer, ByVal Button As Integer, ByVal Location As MapWindow.Interfaces.ClickLocation, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.LegendMouseDown" + vbCrLf & _
        "        'This event fires when a user holds a mouse button down in the legend." + vbCrLf & _
        "    End Sub" + vbCrLf & _
        "" + vbCrLf & _
        "    Public Sub LegendMouseUp(ByVal Handle As Integer, ByVal Button As Integer, ByVal Location As MapWindow.Interfaces.ClickLocation, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.LegendMouseUp" + vbCrLf & _
        "        'This event fires when a user releases a mouse button in the legend." + vbCrLf & _
        "    End Sub" + vbCrLf & _
        "" + vbCrLf & _
        "    Public Sub MapDragFinished(ByVal Bounds As System.Drawing.Rectangle, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.MapDragFinished" + vbCrLf & _
        "        'If a user drags (ie draws a box) with the mouse on the map, this event fires at completion of the drag" + vbCrLf & _
        "        'and returns a system.drawing.rectangle that has the bounds of the box that was ""drawn""" + vbCrLf & _
        "    End Sub" + vbCrLf & _
        "" + vbCrLf & _
        "    Public Sub MapExtentsChanged() Implements MapWindow.Interfaces.IPlugin.MapExtentsChanged" + vbCrLf & _
        "        'This event fires any time there is a zoom or pan that changes the extents of the map." + vbCrLf & _
        "    End Sub" + vbCrLf & _
        "" + vbCrLf & _
        "    Public Sub MapMouseDown(ByVal Button As Integer, ByVal Shift As Integer, ByVal x As Integer, ByVal y As Integer, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.MapMouseDown" + vbCrLf & _
        "        'This event fires when the user holds a mouse button down on the map. Note that x and y are returned" + vbCrLf & _
        "        'as screen coordinates (in pixels), not map coordinates.  So if you really need the map coordinates" + vbCrLf & _
        "        'then you need to use g_MapWin.View.PixelToProj()" + vbCrLf & _
        "    End Sub" + vbCrLf & _
        "" + vbCrLf & _
        "    Public Sub MapMouseMove(ByVal ScreenX As Integer, ByVal ScreenY As Integer, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.MapMouseMove" + vbCrLf & _
        "        'This event fires when the user moves the mouse over the map. Note that x and y are returned" + vbCrLf & _
        "        'as screen coordinates (in pixels), not map coordinates.  So if you really need the map coordinates" + vbCrLf & _
        "        'then you need to use g_MapWin.View.PixelToProj()" + vbCrLf & _
        "    End Sub" + vbCrLf & _
        "" + vbCrLf & _
        "    Public Sub MapMouseUp(ByVal Button As Integer, ByVal Shift As Integer, ByVal x As Integer, ByVal y As Integer, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.MapMouseUp" + vbCrLf & _
        "        'This event fires when the user releases a mouse button down on the map. Note that x and y are returned" + vbCrLf & _
        "        'as screen coordinates (in pixels), not map coordinates.  So if you really need the map coordinates" + vbCrLf & _
        "        'then you need to use g_MapWin.View.PixelToProj()" + vbCrLf & _
        "    End Sub" + vbCrLf & _
        "" + vbCrLf & _
        "    Public Sub Message(ByVal msg As String, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.Message" + vbCrLf & _
        "        'Plug-ins can communicate with eachother using Messages.  If a message is sent then this event fires." + vbCrLf & _
        "        'If you know the message is ""for you"" then you can set Handled=True and then it will not be sent to any" + vbCrLf & _
        "        'other plug-ins." + vbCrLf & _
        "    End Sub" + vbCrLf & _
        "" + vbCrLf & _
        "    Public Sub ProjectLoading(ByVal ProjectFile As String, ByVal SettingsString As String) Implements MapWindow.Interfaces.IPlugin.ProjectLoading" + vbCrLf & _
        "        'When the user opens a project in MapWindow, this event fires.  The ProjectFile is the file name of the" + vbCrLf & _
        "        'project that the user opened (including its path in case that is important for this this plug-in to know)." + vbCrLf & _
        "        'The SettingsString variable contains any string of data that is connected to this plug-in but is stored " + vbCrLf & _
        "        'on a project level. For example, a plug-in that shows streamflow data might allow the user to set a " + vbCrLf & _
        "        'separate database for each project (i.e. one database for the upper Missouri River Basin, a different " + vbCrLf & _
        "        'one for the Lower Colorado Basin.) In this case, the plug-in would store the database name in the " + vbCrLf & _
        "        'SettingsString of the project. " + vbCrLf & _
        "    End Sub" + vbCrLf & _
        "" + vbCrLf & _
        "    Public Sub ProjectSaving(ByVal ProjectFile As String, ByRef SettingsString As String) Implements MapWindow.Interfaces.IPlugin.ProjectSaving" + vbCrLf & _
        "        'When the user saves a project in MapWindow, this event fires.  The ProjectFile is the file name of the" + vbCrLf & _
        "        'project that the user is saving (including its path in case that is important for this this plug-in to know)." + vbCrLf & _
        "        'The SettingsString variable contains any string of data that is connected to this plug-in but is stored " + vbCrLf & _
        "        'on a project level. For example, a plug-in that shows streamflow data might allow the user to set a " + vbCrLf & _
        "        'separate database for each project (i.e. one database for the upper Missouri River Basin, a different " + vbCrLf & _
        "        'one for the Lower Colorado Basin.) In this case, the plug-in would store the database name in the " + vbCrLf & _
        "        'SettingsString of the project. " + vbCrLf & _
        "    End Sub" + vbCrLf & _
        "" + vbCrLf & _
        "    Public Sub ShapesSelected(ByVal Handle As Integer, ByVal SelectInfo As MapWindow.Interfaces.SelectInfo) Implements MapWindow.Interfaces.IPlugin.ShapesSelected" + vbCrLf & _
        "        'This event fires when the user selects one or more shapes using the select tool in MapWindow. Handle is the " + vbCrLf & _
        "        'Layer handle for the shapefile on which shapes were selected. SelectInfo holds information abou the " + vbCrLf & _
        "        'shapes that were selected. " + vbCrLf & _
        "    End Sub" + vbCrLf & _
        "" + vbCrLf & _
        "End Class"


    Private pFilter As String = "VB.net (*.vb)|*.vb|C#.net (*.cs)|*.cs|All files|*.*"
    Private pFilterVB As String = "VB.net (*.vb)|*.vb|All files|*.*"
    Private pFilterCS As String = "C#.net (*.cs)|*.cs|All files|*.*"

    Public pFileName As String = ""

    Private DataChanged As Boolean = False
    Private IgnoreDataChange As Boolean = False

    Private pDefaultScript_CS As String = _
        "using MapWindow.Interfaces;" + vbCrLf & _
        "using MapWinGIS;" + vbCrLf & _
        "using System.Windows.Forms;" + vbCrLf & _
        "using Microsoft.VisualBasic;" + vbCrLf & _
        "using System;" + vbCrLf & _
        "" + vbCrLf & _
        "// Each script should (but doesn't have to) have a unique name. Change MyExample here to something meaningful. ScriptMain should remain as ""Main"" however." + vbCrLf & _
        "class MyExample" + vbCrLf & _
        "{" + vbCrLf & _
        "  public static void ScriptMain(IMapWin m_MapWin)" + vbCrLf & _
        "  {    " + vbCrLf & _
        "    MessageBox.Show(""This is a simple script to display the number of loaded layers in the map."");" + vbCrLf & _
        "    MessageBox.Show(""Number of Layers: "" + m_MapWin.Layers.NumLayers);" + vbCrLf & _
        "  }" + vbCrLf & _
        "}" + vbCrLf
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents tbbNew As System.Windows.Forms.ToolStripButton
    Friend WithEvents tbbOpen As System.Windows.Forms.ToolStripButton
    Friend WithEvents tbbSave As System.Windows.Forms.ToolStripButton
    Friend WithEvents tbbsep As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tbbRun As System.Windows.Forms.ToolStripButton
    Friend WithEvents tbbCompile As System.Windows.Forms.ToolStripButton
    Friend WithEvents tbbHelp As System.Windows.Forms.ToolStripButton
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuNew As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuOpen As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuSave As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuClose As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem6 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuRun As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuCompile As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem2 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuViewRun As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuSubmit As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuHelp As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents BottomToolStripPanel As System.Windows.Forms.ToolStripPanel
    Friend WithEvents TopToolStripPanel As System.Windows.Forms.ToolStripPanel
    Friend WithEvents RightToolStripPanel As System.Windows.Forms.ToolStripPanel
    Friend WithEvents LeftToolStripPanel As System.Windows.Forms.ToolStripPanel
    Friend WithEvents ContentPanel As System.Windows.Forms.ToolStripContentPanel
    Friend WithEvents ToolStripContainer1 As System.Windows.Forms.ToolStripContainer
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents rdScript As System.Windows.Forms.RadioButton
    Friend WithEvents rdPlugin As System.Windows.Forms.RadioButton
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents rdVBNet As System.Windows.Forms.RadioButton
    Friend WithEvents rdCS As System.Windows.Forms.RadioButton
    Friend WithEvents mnuSep As System.Windows.Forms.ToolStripSeparator
    <CLSCompliant(False)> _
    Public WithEvents txtScript As IntellisenseEditor.IntellisenseEditor

    Private pDefaultPlugin_CS As String = _
        "using MapWindow.Interfaces;" + vbCrLf & _
        "using MapWinGIS;" + vbCrLf & _
        "using System.Windows.Forms;" + vbCrLf & _
        "using System;" + vbCrLf & _
        "" + vbCrLf & _
        "namespace MyNamespace" + vbCrLf & _
        "{" + vbCrLf & _
        "// Every plug-in must have a class that implements IPlugin. The class that implements this interface *MUST* be unique, or MapWindow will think it's an updated/different version of an existing plugin." + vbCrLf & _
        "	public class MyExample : MapWindow.Interfaces.IPlugin" + vbCrLf & _
        "	{" + vbCrLf & _
        "		// Change this to match the name of your class. This is the constructor." + vbCrLf & _
        "		public MyExample()" + vbCrLf & _
        "		{" + vbCrLf & _
        "		}" + vbCrLf & _
        "" + vbCrLf & _
        "	        //This event is fired when the user loads your plug-in either through the plug-in dialog " + vbCrLf & _
        "	        //box, or by checkmarking it in the plug-ins menu.  This is where you would add buttons to the" + vbCrLf & _
        "	        //tool bar or menu items to the menu.  " + vbCrLf & _
        "        	//" + vbCrLf & _
        "	        //It is also standard to set a global reference to the IMapWin that is passed through here so that" + vbCrLf & _
        "        	//you can access it elsewhere in your project to act on MapWindow." + vbCrLf & _
        "		public void Initialize(MapWindow.Interfaces.IMapWin m_MapWin, int ParentHandle)" + vbCrLf & _
        "		{" + vbCrLf & _
        "			MessageBox.Show(""This is a simple example script to display the number of loaded layers in the map."");" + vbCrLf & _
        "			MessageBox.Show(""Number of Layers: "" + m_MapWin.Layers.NumLayers);" + vbCrLf & _
        "		}" + vbCrLf & _
        "" + vbCrLf & _
        "        	//This is a description of the plug-in.  It appears in the plug-ins dialog box when a user selects" + vbCrLf & _
        "	        //your plug-in." + vbCrLf & _
        "		public string Description" + vbCrLf & _
        "		{" + vbCrLf & _
        "			get" + vbCrLf & _
        "			{" + vbCrLf & _
        "				return ""Example Script"";" + vbCrLf & _
        "			}" + vbCrLf & _
        "		}" + vbCrLf & _
        "" + vbCrLf & _
        "	        //This is the author of the plug-in. It can be a company name, individual, or organization name." + vbCrLf & _
        "		public string Author" + vbCrLf & _
        "		{" + vbCrLf & _
        "			get" + vbCrLf & _
        "			{" + vbCrLf & _
        "				return ""Script Author Name"";" + vbCrLf & _
        "			}" + vbCrLf & _
        "		}" + vbCrLf & _
        "" + vbCrLf & _
        "       		//This is one of the more important plug-in properties because if it is not set to something then" + vbCrLf & _
        "      	  	//your plug-in will not load at all. This is the name that appears in the Plug-ins menu to identify" + vbCrLf & _
        "      		//this plug-in." + vbCrLf & _
        "		public string Name" + vbCrLf & _
        "		{" + vbCrLf & _
        "			get" + vbCrLf & _
        "			{" + vbCrLf & _
        "				return ""Example Script"";" + vbCrLf & _
        "			}" + vbCrLf & _
        "		}" + vbCrLf & _
        "" + vbCrLf & _
        "	        //This is the version number of the plug-in.  You can either return a hard-coded string" + vbCrLf & _
        "        	//such as ""1.0.0.1"" or you can use the .NET function shown below to dynamically return " + vbCrLf & _
        "	        //the version number from the assembly itself." + vbCrLf & _
        "		public string Version" + vbCrLf & _
        "		{" + vbCrLf & _
        "			get" + vbCrLf & _
        "			{" + vbCrLf & _
        "				return ""1.0.0000"";" + vbCrLf & _
        "			}" + vbCrLf & _
        "		}" + vbCrLf & _
        "" + vbCrLf & _
        "	        //This is the Build Date for the plug-in.  You can either return a string of a hard-coded date" + vbCrLf & _
        "	        //such as ""January 1, 2003"" or you can use the .NET function below to dynamically obtain the build" + vbCrLf & _
        "	        //date of the assembly." + vbCrLf & _
        "		public string BuildDate" + vbCrLf & _
        "		{" + vbCrLf & _
        "			get" + vbCrLf & _
        "			{" + vbCrLf & _
        "				return System.DateTime.Now.ToString();" + vbCrLf & _
        "			}" + vbCrLf & _
        "		}" + vbCrLf & _
        "" + vbCrLf & _
        "	        //When the user opens a project in MapWindow, this event fires.  The ProjectFile is the file name of the" + vbCrLf & _
        "	        //project that the user opened (including its path in case that is important for this this plug-in to know)." + vbCrLf & _
        "	        //The SettingsString variable contains any string of data that is connected to this plug-in but is stored " + vbCrLf & _
        "	        //on a project level. For example, a plug-in that shows streamflow data might allow the user to set a " + vbCrLf & _
        "	        //separate database for each project (i.e. one database for the upper Missouri River Basin, a different " + vbCrLf & _
        "	        //one for the Lower Colorado Basin.) In this case, the plug-in would store the database name in the " + vbCrLf & _
        "	        //SettingsString of the project. " + vbCrLf & _
        "		public void ProjectLoading(string ProjectFile, string SettingsString)" + vbCrLf & _
        "		{" + vbCrLf & _
        "		}" + vbCrLf & _
        "" + vbCrLf & _
        "	        //Plug-ins can communicate with eachother using Messages.  If a message is sent then this event fires." + vbCrLf & _
        "        	//If you know the message is ""for you"" then you can set Handled=True and then it will not be sent to any" + vbCrLf & _
        "	        //other plug-ins." + vbCrLf & _
        "		public void Message(string msg, ref bool Handled)" + vbCrLf & _
        "		{" + vbCrLf & _
        "		}" + vbCrLf & _
        "" + vbCrLf & _
        "	        //This event fires any time there is a zoom or pan that changes the extents of the map." + vbCrLf & _
        "		public void MapExtentsChanged()" + vbCrLf & _
        "		{" + vbCrLf & _
        "		}" + vbCrLf & _
        "" + vbCrLf & _
        "	        //This event is fired when the user unloads your plug-in either through the plug-in dialog " + vbCrLf & _
        "	        //box, or by un-checkmarking it in the plug-ins menu.  This is where you would remove any" + vbCrLf & _
        "	        //buttons from the tool bar tool bar or menu items from the menu that you may have added." + vbCrLf & _
        "	        //If you don't do this, then you will leave dangling menus and buttons that don't do anything." + vbCrLf & _
        "		public void Terminate()" + vbCrLf & _
        "		{" + vbCrLf & _
        "		}" + vbCrLf & _
        "" + vbCrLf & _
        "	        //This event fires when the user holds a mouse button down on the map. Note that x and y are returned" + vbCrLf & _
        "        	//as screen coordinates (in pixels), not map coordinates.  So if you really need the map coordinates" + vbCrLf & _
        "	        //then you need to use g_MapWin.View.PixelToProj()" + vbCrLf & _
        "		public void MapMouseDown(int Button, int Shift, int x, int y, ref bool Handled)" + vbCrLf & _
        "		{" + vbCrLf & _
        "		}" + vbCrLf & _
        "" + vbCrLf & _
        "	        //When the user saves a project in MapWindow, this event fires.  The ProjectFile is the file name of the" + vbCrLf & _
        "	        //project that the user is saving (including its path in case that is important for this this plug-in to know)." + vbCrLf & _
        "	        //The SettingsString variable contains any string of data that is connected to this plug-in but is stored " + vbCrLf & _
        "	        //on a project level. For example, a plug-in that shows streamflow data might allow the user to set a " + vbCrLf & _
        "	        //separate database for each project (i.e. one database for the upper Missouri River Basin, a different " + vbCrLf & _
        "	        //one for the Lower Colorado Basin.) In this case, the plug-in would store the database name in the " + vbCrLf & _
        "	        //SettingsString of the project." + vbCrLf & _
        "		public void ProjectSaving(string ProjectFile, ref string SettingsString)" + vbCrLf & _
        "		{" + vbCrLf & _
        "		}" + vbCrLf & _
        "" + vbCrLf & _
        "	        //This event fires when a menu item or toolbar button is clicked.  So if you added a button or menu" + vbCrLf & _
        "	        //on the Initialize event, then this is where you would handle it." + vbCrLf & _
        "		public void ItemClicked(string ItemName, ref bool Handled)" + vbCrLf & _
        "		{" + vbCrLf & _
        "		}" + vbCrLf & _
        "" + vbCrLf & _
        "	        //This event fires when the user adds a layer to MapWindow.  This is useful to know if your" + vbCrLf & _
        "	        //plug-in depends on a particular layer being present. Also, if you keep an internal list of " + vbCrLf & _
        "        	//available layers, for example you may be keeping a list of all ""point"" shapefiles, then you" + vbCrLf & _
        "	        //would use this event to know when layers have been added or removed." + vbCrLf & _
        "		public void LayersAdded(MapWindow.Interfaces.Layer[] Layers)" + vbCrLf & _
        "		{" + vbCrLf & _
        "		}" + vbCrLf & _
        "" + vbCrLf & _
        "	        //This event fires when a user selects a layer in the legend. " + vbCrLf & _
        "		public void LayerSelected(int Handle)" + vbCrLf & _
        "		{" + vbCrLf & _
        "		}" + vbCrLf & _
        "" + vbCrLf & _
        "	        //If a user drags (ie draws a box) with the mouse on the map, this event fires at completion of the drag" + vbCrLf & _
        "	        //and returns a system.drawing.rectangle that has the bounds of the box that was ""drawn""" + vbCrLf & _
        "		public void MapDragFinished(System.Drawing.Rectangle Bounds, ref bool Handled)" + vbCrLf & _
        "		{" + vbCrLf & _
        "		}" + vbCrLf & _
        "" + vbCrLf & _
        "        	//This event fires when the user releases a mouse button down on the map. Note that x and y are returned" + vbCrLf & _
        "	        //as screen coordinates (in pixels), not map coordinates.  So if you really need the map coordinates" + vbCrLf & _
        "	        //then you need to use g_MapWin.View.PixelToProj()" + vbCrLf & _
        "		public void MapMouseUp(int Button, int Shift, int x, int y, ref bool Handled)" + vbCrLf & _
        "		{" + vbCrLf & _
        "		}" + vbCrLf & _
        "" + vbCrLf & _
        "	        //This event fires when a user double-clicks a layer in the legend." + vbCrLf & _
        "		public void LegendDoubleClick(int Handle, MapWindow.Interfaces.ClickLocation Location, ref bool Handled)" + vbCrLf & _
        "		{" + vbCrLf & _
        "		}" + vbCrLf & _
        "" + vbCrLf & _
        "	        //This event fires when a user holds a mouse button down in the legend." + vbCrLf & _
        "		public void LegendMouseDown(int Handle, int Button, MapWindow.Interfaces.ClickLocation Location, ref bool Handled)" + vbCrLf & _
        "		{" + vbCrLf & _
        "		}" + vbCrLf & _
        "" + vbCrLf & _
        "	        //This event fires when a user releases a mouse button in the legend." + vbCrLf & _
        "		public void LegendMouseUp(int Handle, int Button, MapWindow.Interfaces.ClickLocation Location, ref bool Handled)" + vbCrLf & _
        "		{" + vbCrLf & _
        "		}" + vbCrLf & _
        "" + vbCrLf & _
        "        	//This is the plug-in serial number. " + vbCrLf & _
        "	        //This is no longer needed, but the property must remain for backward compatibility." + vbCrLf & _
        "		public string SerialNumber" + vbCrLf & _
        "		{" + vbCrLf & _
        "			get" + vbCrLf & _
        "			{" + vbCrLf & _
        "				// This is unnecessary but the implementation must be here for backward compatibility." + vbCrLf & _
        "				return null;" + vbCrLf & _
        "			}" + vbCrLf & _
        "		}" + vbCrLf & _
        "" + vbCrLf & _
        "	        //This event fires when the user removes a layer from MapWindow.  This is useful to know if your" + vbCrLf & _
        "	        //plug-in depends on a particular layer being present. " + vbCrLf & _
        "		public void LayerRemoved(int Handle)" + vbCrLf & _
        "		{" + vbCrLf & _
        "		}" + vbCrLf & _
        "" + vbCrLf & _
        "	        //This event fires when the user moves the mouse over the map. Note that x and y are returned" + vbCrLf & _
        "        	//as screen coordinates (in pixels), not map coordinates.  So if you really need the map coordinates" + vbCrLf & _
        "	        //then you need to use g_MapWin.View.PixelToProj()" + vbCrLf & _
        "		public void MapMouseMove(int ScreenX, int ScreenY, ref bool Handled)" + vbCrLf & _
        "		{" + vbCrLf & _
        "		}" + vbCrLf & _
        "" + vbCrLf & _
        "	        //This event fires when the user clears all of the layers from MapWindow.  As with LayersAdded " + vbCrLf & _
        "	        //and LayersRemoved, this is useful to know if your plug-in depends on a particular layer being " + vbCrLf & _
        "	        //present or if you are maintaining your own list of layers." + vbCrLf & _
        "		public void LayersCleared()" + vbCrLf & _
        "		{" + vbCrLf & _
        "		}" + vbCrLf & _
        "" + vbCrLf & _
        "	        //This event fires when the user selects one or more shapes using the select tool in MapWindow. Handle is the " + vbCrLf & _
        "	        //Layer handle for the shapefile on which shapes were selected. SelectInfo holds information abou the " + vbCrLf & _
        "	        //shapes that were selected. " + vbCrLf & _
        "		public void ShapesSelected(int Handle, MapWindow.Interfaces.SelectInfo SelectInfo)" + vbCrLf & _
        "		{" + vbCrLf & _
        "		}" + vbCrLf & _
        "	}" + vbCrLf & _
        "}"

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()
    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents tools As System.Windows.Forms.ToolStrip
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmScript))
        Me.ToolStripContainer1 = New System.Windows.Forms.ToolStripContainer
        Me.txtScript = New IntellisenseEditor.IntellisenseEditor
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.rdScript = New System.Windows.Forms.RadioButton
        Me.rdPlugin = New System.Windows.Forms.RadioButton
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.rdVBNet = New System.Windows.Forms.RadioButton
        Me.rdCS = New System.Windows.Forms.RadioButton
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuNew = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuOpen = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuSave = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuSep = New System.Windows.Forms.ToolStripSeparator
        Me.mnuClose = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem6 = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuRun = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuCompile = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuViewRun = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuSubmit = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuHelp = New System.Windows.Forms.ToolStripMenuItem
        Me.tools = New System.Windows.Forms.ToolStrip
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.tbbNew = New System.Windows.Forms.ToolStripButton
        Me.tbbOpen = New System.Windows.Forms.ToolStripButton
        Me.tbbSave = New System.Windows.Forms.ToolStripButton
        Me.tbbsep = New System.Windows.Forms.ToolStripSeparator
        Me.tbbRun = New System.Windows.Forms.ToolStripButton
        Me.tbbCompile = New System.Windows.Forms.ToolStripButton
        Me.tbbHelp = New System.Windows.Forms.ToolStripButton
        Me.BottomToolStripPanel = New System.Windows.Forms.ToolStripPanel
        Me.TopToolStripPanel = New System.Windows.Forms.ToolStripPanel
        Me.RightToolStripPanel = New System.Windows.Forms.ToolStripPanel
        Me.LeftToolStripPanel = New System.Windows.Forms.ToolStripPanel
        Me.ContentPanel = New System.Windows.Forms.ToolStripContentPanel
        Me.ToolStripContainer1.ContentPanel.SuspendLayout()
        Me.ToolStripContainer1.TopToolStripPanel.SuspendLayout()
        Me.ToolStripContainer1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.MenuStrip1.SuspendLayout()
        Me.tools.SuspendLayout()
        Me.SuspendLayout()
        '
        'ToolStripContainer1
        '
        Me.ToolStripContainer1.AllowDrop = True
        '
        'ToolStripContainer1.ContentPanel
        '
        Me.ToolStripContainer1.ContentPanel.Controls.Add(Me.txtScript)
        Me.ToolStripContainer1.ContentPanel.Controls.Add(Me.GroupBox2)
        Me.ToolStripContainer1.ContentPanel.Controls.Add(Me.GroupBox1)
        resources.ApplyResources(Me.ToolStripContainer1.ContentPanel, "ToolStripContainer1.ContentPanel")
        resources.ApplyResources(Me.ToolStripContainer1, "ToolStripContainer1")
        Me.ToolStripContainer1.Name = "ToolStripContainer1"
        '
        'ToolStripContainer1.TopToolStripPanel
        '
        Me.ToolStripContainer1.TopToolStripPanel.Controls.Add(Me.MenuStrip1)
        Me.ToolStripContainer1.TopToolStripPanel.Controls.Add(Me.tools)
        '
        'txtScript
        '
        resources.ApplyResources(Me.txtScript, "txtScript")
        Me.txtScript.Name = "txtScript"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.rdScript)
        Me.GroupBox2.Controls.Add(Me.rdPlugin)
        resources.ApplyResources(Me.GroupBox2, "GroupBox2")
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.TabStop = False
        '
        'rdScript
        '
        Me.rdScript.Checked = True
        resources.ApplyResources(Me.rdScript, "rdScript")
        Me.rdScript.Name = "rdScript"
        Me.rdScript.TabStop = True
        '
        'rdPlugin
        '
        resources.ApplyResources(Me.rdPlugin, "rdPlugin")
        Me.rdPlugin.Name = "rdPlugin"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.rdVBNet)
        Me.GroupBox1.Controls.Add(Me.rdCS)
        resources.ApplyResources(Me.GroupBox1, "GroupBox1")
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.TabStop = False
        '
        'rdVBNet
        '
        Me.rdVBNet.Checked = True
        resources.ApplyResources(Me.rdVBNet, "rdVBNet")
        Me.rdVBNet.Name = "rdVBNet"
        Me.rdVBNet.TabStop = True
        '
        'rdCS
        '
        resources.ApplyResources(Me.rdCS, "rdCS")
        Me.rdCS.Name = "rdCS"
        '
        'MenuStrip1
        '
        resources.ApplyResources(Me.MenuStrip1, "MenuStrip1")
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem1, Me.ToolStripMenuItem6, Me.ToolStripMenuItem2, Me.mnuHelp})
        Me.MenuStrip1.Name = "MenuStrip1"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuNew, Me.mnuOpen, Me.mnuSave, Me.mnuSep, Me.mnuClose})
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        resources.ApplyResources(Me.ToolStripMenuItem1, "ToolStripMenuItem1")
        '
        'mnuNew
        '
        Me.mnuNew.Name = "mnuNew"
        resources.ApplyResources(Me.mnuNew, "mnuNew")
        '
        'mnuOpen
        '
        Me.mnuOpen.Name = "mnuOpen"
        resources.ApplyResources(Me.mnuOpen, "mnuOpen")
        '
        'mnuSave
        '
        Me.mnuSave.Name = "mnuSave"
        resources.ApplyResources(Me.mnuSave, "mnuSave")
        '
        'mnuSep
        '
        Me.mnuSep.Name = "mnuSep"
        resources.ApplyResources(Me.mnuSep, "mnuSep")
        '
        'mnuClose
        '
        Me.mnuClose.Name = "mnuClose"
        resources.ApplyResources(Me.mnuClose, "mnuClose")
        '
        'ToolStripMenuItem6
        '
        Me.ToolStripMenuItem6.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuRun, Me.mnuCompile})
        Me.ToolStripMenuItem6.Name = "ToolStripMenuItem6"
        resources.ApplyResources(Me.ToolStripMenuItem6, "ToolStripMenuItem6")
        '
        'mnuRun
        '
        Me.mnuRun.Name = "mnuRun"
        resources.ApplyResources(Me.mnuRun, "mnuRun")
        '
        'mnuCompile
        '
        Me.mnuCompile.Name = "mnuCompile"
        resources.ApplyResources(Me.mnuCompile, "mnuCompile")
        '
        'ToolStripMenuItem2
        '
        Me.ToolStripMenuItem2.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuViewRun, Me.mnuSubmit})
        Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        resources.ApplyResources(Me.ToolStripMenuItem2, "ToolStripMenuItem2")
        '
        'mnuViewRun
        '
        Me.mnuViewRun.Name = "mnuViewRun"
        resources.ApplyResources(Me.mnuViewRun, "mnuViewRun")
        '
        'mnuSubmit
        '
        Me.mnuSubmit.Name = "mnuSubmit"
        resources.ApplyResources(Me.mnuSubmit, "mnuSubmit")
        '
        'mnuHelp
        '
        Me.mnuHelp.Name = "mnuHelp"
        resources.ApplyResources(Me.mnuHelp, "mnuHelp")
        '
        'tools
        '
        resources.ApplyResources(Me.tools, "tools")
        Me.tools.ImageList = Me.ImageList1
        Me.tools.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tbbNew, Me.tbbOpen, Me.tbbSave, Me.tbbsep, Me.tbbRun, Me.tbbCompile, Me.tbbHelp})
        Me.tools.Name = "tools"
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "")
        Me.ImageList1.Images.SetKeyName(1, "")
        Me.ImageList1.Images.SetKeyName(2, "")
        Me.ImageList1.Images.SetKeyName(3, "")
        Me.ImageList1.Images.SetKeyName(4, "")
        Me.ImageList1.Images.SetKeyName(5, "")
        '
        'tbbNew
        '
        resources.ApplyResources(Me.tbbNew, "tbbNew")
        Me.tbbNew.Name = "tbbNew"
        '
        'tbbOpen
        '
        resources.ApplyResources(Me.tbbOpen, "tbbOpen")
        Me.tbbOpen.Name = "tbbOpen"
        '
        'tbbSave
        '
        resources.ApplyResources(Me.tbbSave, "tbbSave")
        Me.tbbSave.Name = "tbbSave"
        '
        'tbbsep
        '
        Me.tbbsep.Name = "tbbsep"
        resources.ApplyResources(Me.tbbsep, "tbbsep")
        '
        'tbbRun
        '
        resources.ApplyResources(Me.tbbRun, "tbbRun")
        Me.tbbRun.Name = "tbbRun"
        '
        'tbbCompile
        '
        resources.ApplyResources(Me.tbbCompile, "tbbCompile")
        Me.tbbCompile.Name = "tbbCompile"
        '
        'tbbHelp
        '
        resources.ApplyResources(Me.tbbHelp, "tbbHelp")
        Me.tbbHelp.Name = "tbbHelp"
        '
        'BottomToolStripPanel
        '
        resources.ApplyResources(Me.BottomToolStripPanel, "BottomToolStripPanel")
        Me.BottomToolStripPanel.Name = "BottomToolStripPanel"
        Me.BottomToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal
        Me.BottomToolStripPanel.RowMargin = New System.Windows.Forms.Padding(3, 0, 0, 0)
        '
        'TopToolStripPanel
        '
        resources.ApplyResources(Me.TopToolStripPanel, "TopToolStripPanel")
        Me.TopToolStripPanel.Name = "TopToolStripPanel"
        Me.TopToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal
        Me.TopToolStripPanel.RowMargin = New System.Windows.Forms.Padding(3, 0, 0, 0)
        '
        'RightToolStripPanel
        '
        resources.ApplyResources(Me.RightToolStripPanel, "RightToolStripPanel")
        Me.RightToolStripPanel.Name = "RightToolStripPanel"
        Me.RightToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal
        Me.RightToolStripPanel.RowMargin = New System.Windows.Forms.Padding(3, 0, 0, 0)
        '
        'LeftToolStripPanel
        '
        resources.ApplyResources(Me.LeftToolStripPanel, "LeftToolStripPanel")
        Me.LeftToolStripPanel.Name = "LeftToolStripPanel"
        Me.LeftToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal
        Me.LeftToolStripPanel.RowMargin = New System.Windows.Forms.Padding(3, 0, 0, 0)
        '
        'ContentPanel
        '
        resources.ApplyResources(Me.ContentPanel, "ContentPanel")
        '
        'frmScript
        '
        Me.AllowDrop = True
        resources.ApplyResources(Me, "$this")
        Me.Controls.Add(Me.ToolStripContainer1)
        Me.Name = "frmScript"
        Me.ToolStripContainer1.ContentPanel.ResumeLayout(False)
        Me.ToolStripContainer1.TopToolStripPanel.ResumeLayout(False)
        Me.ToolStripContainer1.TopToolStripPanel.PerformLayout()
        Me.ToolStripContainer1.ResumeLayout(False)
        Me.ToolStripContainer1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.tools.ResumeLayout(False)
        Me.tools.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub tools_ButtonClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ToolStripItemClickedEventArgs) Handles tools.ItemClicked
        PerformAction(CStr(e.ClickedItem.Name))
    End Sub

    Private Function Language() As String
        If rdVBNet.Checked Then
            Return "vb"
        ElseIf rdCS.Checked Then
            Return "cs"
        Else
            Return ""
        End If
    End Function

    Private Sub PerformAction(ByVal action As String)
        action = action.ToLower().Replace("&", "")
        Select Case action
            Case "tbbnew", "new"
                If rdVBNet.Checked Then
                    txtScript.Text = pDefaultScript_VB
                Else
                    txtScript.Text = pDefaultScript_CS
                End If

            Case "tbbopen", "open"
                Dim cdOpen As New Windows.Forms.OpenFileDialog
                Try
                    cdOpen.InitialDirectory = GetSetting("MapWindow", "Defaults", "ScriptPath", cdOpen.InitialDirectory)
                Catch
                End Try
                cdOpen.Filter = pFilter
                cdOpen.FileName = pFileName
                If cdOpen.ShowDialog() = Windows.Forms.DialogResult.OK Then
                    pFileName = cdOpen.FileName
                    Try
                        SaveSetting("MapWindow", "Defaults", "ScriptPath", System.IO.Path.GetDirectoryName(pFileName))
                    Catch
                    End Try
                    If (cdOpen.FileName.ToLower.EndsWith("cs")) Then
                        rdCS.Checked = True
                    Else
                        rdVBNet.Checked = True
                    End If
                    IgnoreDataChange = True
                    txtScript.Text = MapWinUtility.Strings.WholeFileString(pFileName)
                    IgnoreDataChange = False
                End If

            Case "tbbsave", "save"
                Dim cdSave As New Windows.Forms.SaveFileDialog
                If rdVBNet.Checked Then
                    cdSave.Filter = pFilterVB
                Else
                    cdSave.Filter = pFilterCS
                End If
                Try
                    cdSave.InitialDirectory = GetSetting("MapWindow", "Defaults", "ScriptPath", cdSave.InitialDirectory)
                Catch
                End Try
                cdSave.FileName = pFileName
                If cdSave.ShowDialog() = Windows.Forms.DialogResult.OK Then
                    pFileName = cdSave.FileName
                    Try
                        SaveSetting("MapWindow", "Defaults", "ScriptPath", System.IO.Path.GetDirectoryName(pFileName))
                    Catch
                    End Try
                    MapWinUtility.Strings.SaveFileString(pFileName, txtScript.Text)
                End If

                IgnoreDataChange = True
                IgnoreDataChange = False

            Case "tbbrun", "run script", "run"
                Dim errors As String = ""
                Dim args(0) As Object
                args(0) = frmMain

                MapWinUtility.Scripting.Run(Language, Nothing, txtScript.Text, errors, rdPlugin.Checked, CObj(frmMain), args)

                If Not errors Is Nothing AndAlso errors.Trim().Length > 0 Then
                    'should the logger be used here?
                    mapwinutility.logger.msg(errors, MsgBoxStyle.Exclamation, "Script Error")
                End If

            Case "tbbcompile", "plugin", "plug-in", "compile plug-in", "compile plugin", "compile"
                Dim cdSave As New Windows.Forms.SaveFileDialog
                Dim errors As String = ""
                Dim outPath As String = ""
                Dim assy As System.Reflection.Assembly
                cdSave.Filter = "DLL files (*.dll)|*.dll"
                cdSave.InitialDirectory = frmMain.Plugins.PluginFolder
                cdSave.OverwritePrompt = True
                Dim MustRename As Boolean = False

                If cdSave.ShowDialog() = Windows.Forms.DialogResult.OK Then
                    outPath = cdSave.FileName
                    If System.IO.File.Exists(outPath) Then
                        'Get the key, so we can turn it off and unload it:
                        Dim info As New PluginInfo()
                        info.Init(outPath, GetType(PluginInterfaces.IBasePlugin).GUID)
                        If Not info.Key = "" Then
                            frmMain.Plugins.StopPlugin(info.Key)
                            Dim plugin As Interfaces.IPlugin
                            For Each plugin In frmMain.m_PluginManager.LoadedPlugins
                                If plugin.Name = info.Name Then
                                    plugin.Terminate()
                                    plugin = Nothing
                                End If
                            Next
                            'Do not scan here -- or it will immediately reload the plug-in.
                            'frmMain.m_PluginManager.ScanPluginFolder()
                            'frmMain.SynchPluginMenu()
                        End If
                        info = Nothing 'no Dispose on this object; mark it as nothing

                        Try
                            System.IO.File.Delete(outPath)
                        Catch ex As Exception
                            MustRename = True
                        End Try

                        If MustRename Then
                            'Cannot delete the old file; the assembly is still referenced
                            'by .NET for some reason. Since it's not loaded in MW,
                            'just change the name and move on.
                            For z As Integer = 1 To 500
                                If Not System.IO.File.Exists(System.IO.Path.GetFileNameWithoutExtension(outPath) + "-" + z.ToString() + ".dll") Then
                                    outPath = System.IO.Path.GetFileNameWithoutExtension(outPath) + "-" + z.ToString() + ".dll"
                                    mapwinutility.logger.msg("Notice -- The old file could not be deleted." + vbCrLf + "The newest version of your plug-in will be called: " + vbCrLf + vbCrLf + outPath + vbCrLf + vbCrLf + _
                                            "You may need to close MapWindow and delete the old version for the new plug-in to load properly.", MsgBoxStyle.Information, "Renamed Plugin")
                                    Exit For
                                End If
                            Next z
                        End If
                    End If
                    assy = MapWinUtility.Scripting.Compile(Language, txtScript.Text, _
                                  errors, _
                                  outPath)
                    If errors.Length = 0 Then
                        frmMain.Plugins.AddFromFile(outPath)
                    Else
                        mapwinutility.logger.msg(errors, MsgBoxStyle.Exclamation, "Script error")
                    End If
                End If

            Case "tbbclose", "close"
                Me.Close()
        End Select
    End Sub

    Private Sub frmScript_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Me.DragDrop, ToolStripContainer1.DragDrop
        Try
            If e.Data.GetDataPresent(DataFormats.FileDrop) Then
                Dim a As Array = e.Data.GetData(DataFormats.FileDrop)
                If a.Length > 0 Then
                    If System.IO.File.Exists(a(0)) Then
                        IgnoreDataChange = True
                        txtScript.Text = MapWinUtility.Strings.WholeFileString(a(0))
                        IgnoreDataChange = False
                        e.Data.SetData(Nothing)
                        Exit Sub
                    End If
                End If
            End If
        Catch
        End Try
    End Sub

    Private Sub frmScript_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Me.DragEnter, ToolStripContainer1.DragEnter
        e.Effect = DragDropEffects.Copy
    End Sub

    Private Sub frmScript_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        txtScript.Shutdown()
    End Sub

    Private Sub frmScript_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        txtScript.AllowDrop = True
        If pFileName = "" Then
            'Load the default. Don't overwrite something already in the box
            '(a loaded saved script)

            IgnoreDataChange = True
            If System.IO.File.Exists(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) & "\mw_LastScript.dat") Then
                txtScript.Text = MapWinUtility.Strings.WholeFileString(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) & "\mw_LastScript.dat")
            Else
                txtScript.Text = pDefaultScript_CS
            End If

            DataChanged = False
            If GetSetting("MapWindow", "LastScript", "CS", "True") = "False" Then
                rdVBNet.Checked = True
                txtScript.SetVB()
            Else
                rdCS.Checked = True
                txtScript.SetCS()
            End If
            IgnoreDataChange = False
        End If
        txtScript.Init()

        'Height adjust -- internationalization seems to randomly reset the txtscript size
        txtScript.Width = Me.Width - 25
        txtScript.Height = Me.Height - 144
    End Sub

    Private Sub rdLangOrOutput_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdVBNet.CheckedChanged, rdCS.CheckedChanged, rdPlugin.CheckedChanged, rdScript.CheckedChanged
        If IgnoreDataChange Then Exit Sub

        If DataChanged Then
            'PM
            Dim b As MsgBoxResult = MapWinUtility.Logger.Msg("Do you wish to save your current script first?", MsgBoxStyle.YesNo, "Save first?")
            'Dim b As MsgBoxResult = mapwinutility.logger.msg(pResources.GetString("msgSaveCurrentScript.Text"), MsgBoxStyle.YesNo)
            If b = MsgBoxResult.Yes Then
                Dim cdSave As New Windows.Forms.SaveFileDialog
                cdSave.Filter = pFilter
                cdSave.FileName = pFileName
                If cdSave.ShowDialog() = Windows.Forms.DialogResult.OK Then
                    pFileName = cdSave.FileName
                    MapWinUtility.Strings.SaveFileString(pFileName, txtScript.Text)
                End If
            End If
        End If

        Me.Cursor = Cursors.WaitCursor

        IgnoreDataChange = True

        'Change the enabled menu buttons appropriately
        '(only allow run on scripts, only allow compile on plugins)
        mnuCompile.Enabled = Not rdScript.Checked
        mnuRun.Enabled = rdScript.Checked
        For i As Integer = 0 To tools.Items.Count - 1
            If CStr(tools.Items(i).Tag) = "Compile" Then
                tools.Items(i).Enabled = Not rdScript.Checked
            End If
            If CStr(tools.Items(i).Tag) = "Run" Then
                tools.Items(i).Enabled = rdScript.Checked
            End If
        Next

        If rdScript.Checked Then
            If rdVBNet.Checked Then
                txtScript.Text = pDefaultScript_VB
                txtScript.SetVB()
            Else
                txtScript.Text = pDefaultScript_CS
                txtScript.SetCS()
            End If
        Else
            If rdVBNet.Checked Then
                txtScript.Text = pDefaultPlugin_VB
                txtScript.SetVB()
            Else
                txtScript.Text = pDefaultPlugin_CS
                txtScript.SetCS()
            End If
        End If

        'If not visible, loading - don't save the state yet
        If (Me.Visible) Then SaveSetting("MapWindow", "LastScript", "CS", rdCS.Checked.ToString())

        Me.Cursor = Cursors.Default

        IgnoreDataChange = False
        DataChanged = False
    End Sub

    Private Sub txtScript_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = Keys.F5 Then
            PerformAction("Run")
        End If
    End Sub

    Private Sub txtScript_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If IgnoreDataChange Then Exit Sub

        DataChanged = True
    End Sub

    Public Sub RunSavedScript()
        If pFileName = "" Then Exit Sub 'nothing to do
        If Not System.IO.File.Exists(pFileName) Then
            'PM
            MapWinUtility.Logger.Msg("Warning: Ignoring the script name provided on the command line because it doesn't exist." & vbCrLf & vbCrLf & pFileName, MsgBoxStyle.Exclamation, "Nonexistant Script Passed")
            'Dim sMsg = String.Format(pResources.GetString("msgIgnoreScript.Text"), pFileName)
            'mapwinutility.logger.msg(sMsg, MsgBoxStyle.Exclamation, "Nonexistant Script Passed")
            Exit Sub
        End If

        Me.Show()
        Me.WindowState = FormWindowState.Minimized

        IgnoreDataChange = True
        txtScript.Text = MapWinUtility.Strings.WholeFileString(pFileName)
        IgnoreDataChange = False

        'Make it pretty
        Dim errors As String = ""
        Dim args(0) As Object
        args(0) = frmMain

        MapWinUtility.Scripting.Run(System.IO.Path.GetExtension(pFileName), Nothing, txtScript.Text, errors, rdPlugin.Checked, CObj(frmMain), args)

        If Not errors Is Nothing And Not errors.Trim() = "" Then
            mapwinutility.logger.msg(errors, MsgBoxStyle.Exclamation, "Script Error")
        End If

        Me.Close()
    End Sub

    Private Sub ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuHelp.Click, mnuSubmit.Click, mnuViewRun.Click, mnuRun.Click, mnuCompile.Click, mnuNew.Click, mnuSave.Click, mnuOpen.Click, mnuClose.Click, mnuCompile.Click
        PerformAction((CType(sender, System.Windows.Forms.ToolStripMenuItem).Text.Replace("&", "")))
    End Sub

    Private Sub frmScript_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        If Not txtScript.Text = "" Then
            SaveSetting("MapWindow", "LastScript", "CS", rdCS.Checked.ToString())
            If System.IO.File.Exists(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) & "\mw_LastScript.dat") Then
                Kill(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) & "\mw_LastScript.dat")
            End If

            MapWinUtility.Strings.SaveFileString(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) & "\mw_LastScript.dat", txtScript.Text)
        End If
    End Sub

    Private Sub frmScript_LocationChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LocationChanged

    End Sub
End Class
