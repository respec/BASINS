'********************************************************************************************************
'Filename:      clsXMLProjectFile.vb
'Description:   Friend class that contains functions for reading and wrting project and config files.
'This class has been updated to manage project files and to provide a globally available instance of the 
'class that is used to hold all global project related variables.  In prior versions of MapWindow (3.x) 
'this the global variables were stored in a variety of disparate places including the main MapWindow form.  
'********************************************************************************************************
'The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
'you may not use this file except in compliance with the License. You may obtain a copy of the License at 
'http://www.mozilla.org/MPL/ 
'Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
'ANY KIND, either express or implied. See the License for the specificlanguage governing rights and 
'limitations under the License. 
'
'The Original Code is MapWindow Open Source. 
'
'The Initial Developer of this version of the Original Code is Daniel P. Ames using portions created by 
'Utah State University and the Idaho National Engineering and Environmental Lab that were released as 
'public domain in March 2004.  
'
'Contributor(s): (Open source contributors should list themselves and their modifications here). 
'Last Update:   1/12/2005, dpa
'3/23/2005 fixed Recent Projects menu, mgray
'6/9/2005 fixed grid loading to not rebuild the image every time - dpa
'7/21/2005 Added functionality to warn the user if a layer is missing when a project is being loaded, asking them if they'd like to locate said file. - Chris Michaelis 
'9/19/2005 Added functionality to overwrite the default "DefaultDir"
'4/29/2007 Tom Shanley (tws) added save/restore of shape-level formatting; and setting to control that
'********************************************************************************************************

Imports System.Xml
Imports System.Runtime.Serialization.Formatters.Binary

Friend Class XmlProjectFile
    'Private Variables
    Private p_Doc As New XmlDocument
    Private m_ErrorOccured As Boolean
    Private m_ErrorMsg As String = "The following errors occured:" + Chr(13) + Chr(13)
    Private m_panel As System.Windows.Forms.StatusBarPanel

    'Public Variables 
    Public ProjectFileName As String
    Public ConfigFileName As String
    Public ConfigLoaded As Boolean
    Public Modified As Boolean
    Public RecentProjects As New Collections.ArrayList
    Public Shared m_MainToolbarButtons As New Hashtable
    Public ProjectProjection As String 'PROJ4 string
    Public m_MapUnits As String 'Meters, Feet, etc
    Public ShowStatusBarCoords_Projected As Boolean = True 'Default to true
    Public ShowStatusBarCoords_Alternate As String = "(None)" 'Default to true
    Public StatusBarCoordsNumDecimals As Integer = 3
    Public StatusBarAlternateCoordsNumDecimals As Integer = 3
    Public StatusBarCoordsUseCommas As Boolean = True
    Public StatusBarAlternateCoordsUseCommas As Boolean = True
    Public NoPromptToSendErrors As Boolean = False
    Public SaveShapeSettings As Boolean = False ' default false >> no surprises for other plugin developers doing shape-level formatting

#Region "Save Config File"
    'This region includes functions that are part of saving the config file.

    Public Function SaveConfig() As Boolean
        'This function saves the config file. In prior versions, filename was 
        'a parameter.  Now it is a local variable. 
        'Also, version 3 used the DotNetBar which had an export function
        'that would export the current layout of the bars. This has been removed.
        Dim Root As XmlElement
        Dim Path As String
        Dim Reader As System.IO.StreamReader
        Dim Ver As String
        Dim tStr As String

        Try
            Ver = App.VersionString()
            Path = System.IO.Path.GetTempFileName()
            Reader = New System.IO.StreamReader(Path)
            tStr = Reader.ReadToEnd()
            p_Doc.LoadXml("<Mapwin type='configurationfile' version='" + Ver + "'>" + tStr + "</Mapwin>")
            Root = p_Doc.DocumentElement

            'Add the AppInfo
            AddAppInfo(p_Doc, Root)

            'Add the recent projects
            AddRecentProjects(p_Doc, Root)

            'Add the properties of the view to the project file
            AddViewElement(p_Doc, Root)

            'Add the list of the plugins to the project file
            AddPluginsElement(p_Doc, Root, True)

            'Add the application plugins - these are plugins that are 
            'required by a particular application - e.g. BASINS specific 
            'plugins.
            AddApplicationPluginsElement(p_Doc, Root, True)

            'close
            Reader.Close()
            System.IO.File.Delete(Path)

            MapWinUtility.Logger.Dbg("Saving Configuration: " + ConfigFileName)
            p_Doc.Save(ConfigFileName)

            Return True
        Catch ex As System.Exception
            ShowError(ex)
            Return False
        End Try
    End Function

    Private Sub AddAppInfo(ByRef m_Doc As Xml.XmlDocument, ByVal Parent As XmlElement)
        'This sub writes the customizable application info to the configuration file.
        'This info is now pulled from the global AppInfo object as of version 4.
        '1/16/2005
        Dim AppInfoXML As XmlElement = m_Doc.CreateElement("AppInfo")
        Dim SplashImage As XmlElement = m_Doc.CreateElement("SplashPicture")
        Dim WindowIcon As XmlElement = m_Doc.CreateElement("WindowIcon")
        Dim Name As XmlAttribute = m_Doc.CreateAttribute("Name")
        Dim Version As XmlAttribute = m_Doc.CreateAttribute("Version")
        Dim BuildDate As XmlAttribute = m_Doc.CreateAttribute("BuildDate")
        Dim Developer As XmlAttribute = m_Doc.CreateAttribute("Developer")
        Dim Comments As XmlAttribute = m_Doc.CreateAttribute("Comments")
        Dim HelpFilePath As XmlAttribute = m_Doc.CreateAttribute("HelpFilePath")
        Dim UseSplashScreen As XmlAttribute = m_Doc.CreateAttribute("UseSplashScreen")
        Dim SplashPicture As XmlAttribute = m_Doc.CreateAttribute("SplashPicture")
        Dim SplashTime As XmlAttribute = m_Doc.CreateAttribute("SplashTime")
        Dim DefaultDir As XmlAttribute = m_Doc.CreateAttribute("DefaultDir")
        Dim URL As XmlAttribute = m_Doc.CreateAttribute("URL")
        Dim ShowWelcomeScreen As XmlAttribute = m_Doc.CreateAttribute("ShowWelcomeScreen")
        Dim WelcomePlugin As XmlAttribute = m_Doc.CreateAttribute("WelcomePlugin")
        Dim NeverShowProjectionDialog As XmlAttribute = m_Doc.CreateAttribute("NeverShowProjectionDialog")
        Dim NoPromptToSendErrorsXml As XmlAttribute = m_Doc.CreateAttribute("NoPromptToSendErrors")
        Dim LogfilePathXml As XmlAttribute = m_Doc.CreateAttribute("LogfilePath")

        'Set the attributes
        LogfilePathXml.InnerText = AppInfo.LogfilePath
        Name.InnerText = AppInfo.Name
        Version.InnerText = AppInfo.Version
        BuildDate.InnerText = AppInfo.BuildDate
        Developer.InnerText = AppInfo.Developer
        Comments.InnerText = AppInfo.Comments
        HelpFilePath.InnerText = GetRelativePath(AppInfo.HelpFilePath, System.Reflection.Assembly.GetAssembly(Me.GetType).Location)
        SplashTime.InnerText = AppInfo.SplashTime.ToString()
        DefaultDir.InnerText = GetRelativePath(AppInfo.DefaultDir, System.Reflection.Assembly.GetAssembly(Me.GetType).Location)
        URL.InnerText = AppInfo.URL
        ShowWelcomeScreen.InnerText = AppInfo.ShowWelcomeScreen.ToString()
        WelcomePlugin.InnerText = AppInfo.WelcomePlugin
        NeverShowProjectionDialog.InnerText = AppInfo.NeverShowProjectionDialog.ToString
        NoPromptToSendErrorsXml.InnerText = NoPromptToSendErrors.ToString()

        'Add the attributes to the appInfo element
        With AppInfoXML.Attributes
            .Append(Name)
            .Append(Version)
            .Append(BuildDate)
            .Append(Developer)
            .Append(Comments)
            .Append(HelpFilePath)
            .Append(UseSplashScreen)
            .Append(SplashTime)
            .Append(DefaultDir)
            .Append(URL)
            .Append(ShowWelcomeScreen)
            .Append(WelcomePlugin)
            .Append(NeverShowProjectionDialog)
            .Append(NoPromptToSendErrorsXml)
            .Append(LogfilePathXml)
        End With

        SaveImage(m_Doc, AppInfo.SplashPicture, SplashImage)
        SaveImage(m_Doc, frmMain.Icon, WindowIcon)

        AppInfoXML.AppendChild(WindowIcon)
        AppInfoXML.AppendChild(SplashImage)

        Parent.AppendChild(AppInfoXML)
    End Sub

    Private Sub SaveImage(ByRef m_Doc As Xml.XmlDocument, ByVal img As Object, ByVal parent As XmlElement)
        Dim image As XmlElement = m_Doc.CreateElement("Image")
        Dim type As XmlAttribute = m_Doc.CreateAttribute("Type")

        Dim typ As String = ""

        'set the properies of the image
        image.InnerText = ConvertImageToString(img, typ)
        type.InnerText = typ

        'add the properties to the images
        image.Attributes.Append(type)

        parent.AppendChild(image)
    End Sub

    Private Sub AddRecentProjects(ByRef m_Doc As Xml.XmlDocument, ByVal Parent As XmlElement)
        'Adds information about the recent projects to the XML document
        'Changed in v.4. to pull recent projects from projinfo object.
        '1/16/2005
        Try
            Dim i As Integer
            Dim RecentFiles As XmlElement = m_Doc.CreateElement("RecentProjects")
            Dim FileXML As XmlElement

            If ProjInfo.RecentProjects.Count <> 0 Then
                For i = 0 To ProjInfo.RecentProjects.Count - 1
                    FileXML = m_Doc.CreateElement("Project")
                    FileXML.InnerText = Me.GetRelativePath(ProjInfo.RecentProjects(i).ToString, ConfigFileName)
                    RecentFiles.AppendChild(FileXML)
                Next
            End If

            Parent.AppendChild(RecentFiles)
        Catch ex As System.Exception
            ShowError(ex)
        End Try
    End Sub

    Private Sub AddViewElement(ByRef m_Doc As Xml.XmlDocument, ByVal Parent As XmlElement)
        'Adds information about the current view to the config file. 
        'At this point, frmMain must exist or this function will die.
        Dim View As XmlElement = m_Doc.CreateElement("View")
        Dim WindowWidth As XmlAttribute = m_Doc.CreateAttribute("WindowWidth")
        Dim WindowHeight As XmlAttribute = m_Doc.CreateAttribute("WindowHeight")
        Dim LocationX As XmlAttribute = m_Doc.CreateAttribute("LocationX")
        Dim LocationY As XmlAttribute = m_Doc.CreateAttribute("LocationY")
        Dim WindowState As XmlAttribute = m_Doc.CreateAttribute("WindowState")
        Dim ViewColor As XmlAttribute = m_Doc.CreateAttribute("ViewBackColor")
        Dim CanUndockPreviewMap As XmlAttribute = m_Doc.CreateAttribute("CanUndockPreviewMap")
        Dim CanUndockLegend As XmlAttribute = m_Doc.CreateAttribute("CanUndockLegend")
        Dim CanHidePreviewMap As XmlAttribute = m_Doc.CreateAttribute("CanHidePreviewMap")
        Dim CanHideLegend As XmlAttribute = m_Doc.CreateAttribute("CanHideLegend")
        Dim ShowCustomizeContextMenuStrip As XmlAttribute = m_Doc.CreateAttribute("ShowCustomizeContextMenuStrip")
        Dim CanPreviewMapDockLeft As XmlAttribute = m_Doc.CreateAttribute("CanPreviewMapDockLeft")
        Dim CanLegendDockLeft As XmlAttribute = m_Doc.CreateAttribute("CanLegendDockLeft")
        Dim CanPreviewMapDockRight As XmlAttribute = m_Doc.CreateAttribute("CanPreviewMapDockRight")
        Dim CanLegendDockRight As XmlAttribute = m_Doc.CreateAttribute("CanLegendDockRight")
        Dim LoadTIFFandIMGasgridAttr As Xml.XmlAttribute = m_Doc.CreateAttribute("LoadTIFFandIMGasgrid")
        Dim LoadESRIAsGridAttr As Xml.XmlAttribute = m_Doc.CreateAttribute("LoadESRIAsGrid")
        Dim MouseWheelBehavior As Xml.XmlAttribute = m_Doc.CreateAttribute("MouseWheelBehavior")

        'set the properties
        With frmMain
            If .WindowState = FormWindowState.Maximized Then
                WindowState.InnerText = CInt(Windows.Forms.FormWindowState.Maximized).ToString
                LocationX.InnerText = "692"
                LocationY.InnerText = "531"
                WindowWidth.InnerText = "163"
                WindowHeight.InnerText = "68"
            ElseIf .WindowState = FormWindowState.Normal Then
                WindowState.InnerText = CInt(FormWindowState.Normal).ToString
                LocationX.InnerText = .Location.X.ToString()
                LocationY.InnerText = .Location.Y.ToString()
                WindowWidth.InnerText = .Width.ToString()
                WindowHeight.InnerText = .Height.ToString()
            ElseIf .WindowState = FormWindowState.Minimized Then
                WindowState.InnerText = CInt(FormWindowState.Minimized).ToString
                LocationX.InnerText = "692"
                LocationY.InnerText = "531"
                WindowWidth.InnerText = "163"
                WindowHeight.InnerText = "68"
            End If

        End With

        'add attributes to the view
        View.Attributes.Append(WindowWidth)
        View.Attributes.Append(WindowHeight)
        View.Attributes.Append(LocationX)
        View.Attributes.Append(LocationY)
        View.Attributes.Append(WindowState)
        View.Attributes.Append(ViewColor)
        View.Attributes.Append(CanUndockPreviewMap)
        View.Attributes.Append(CanUndockLegend)
        View.Attributes.Append(CanHidePreviewMap)
        View.Attributes.Append(CanHideLegend)
        View.Attributes.Append(ShowCustomizeContextMenuStrip)
        View.Attributes.Append(CanPreviewMapDockLeft)
        View.Attributes.Append(CanLegendDockLeft)
        View.Attributes.Append(CanPreviewMapDockRight)
        View.Attributes.Append(CanLegendDockRight)
        View.Attributes.Append(LoadTIFFandIMGasgridAttr)
        View.Attributes.Append(LoadESRIAsGridAttr)
        View.Attributes.Append(MouseWheelBehavior)

        Parent.AppendChild(View)
    End Sub

    Private Sub AddPluginsElement(ByRef m_Doc As Xml.XmlDocument, ByVal Parent As XmlElement, ByVal LoadingConfig As Boolean)
        'Adds the plugins to the configuration file.
        Dim Plugins As XmlElement = m_Doc.CreateElement("Plugins")
        Dim Plugin As Interfaces.PluginInfo

        Dim ar As Collection = frmMain.m_PluginManager.LoadedPlugins
        'Note that collections start at 1 for some bizarre reason
        For i As Integer = 1 To ar.Count
            Plugin = CType(frmMain.m_PluginManager.PluginsList(MapWinUtility.PluginManagementTools.GenerateKey(ar(i).GetType())), Interfaces.PluginInfo)
            AddPluginElement(m_Doc, ar(i), Plugin.Key, Plugins, LoadingConfig)
        Next

        Parent.AppendChild(Plugins)
    End Sub

    Private Sub AddPluginElement(ByRef m_Doc As Xml.XmlDocument, ByVal Plugin As Object, ByVal PluginKey As String, ByVal Parent As XmlElement, ByVal LoadingConfig As Boolean)
        'Adds information for a single plugin to the configuration file.
        Dim NewPlugin As XmlElement = m_Doc.CreateElement("Plugin")
        Dim SettingsString As XmlAttribute = m_Doc.CreateAttribute("SettingsString")
        Dim KeyXML As XmlAttribute = m_Doc.CreateAttribute("Key")
        Dim SetString As String = ""

        'Plugin properties
        If LoadingConfig = False Then
            'Saving project
            If TypeOf Plugin Is MapWindow.Interfaces.IPlugin Or TypeOf Plugin Is MapWindow.PluginInterfaces.IProjectEvents Then
                Plugin.ProjectSaving(ProjectFileName, SetString)
            Else
                SetString = ""
            End If
        End If

        SettingsString.InnerText = SetString
        KeyXML.InnerText = PluginKey

        NewPlugin.Attributes.Append(SettingsString)
        NewPlugin.Attributes.Append(KeyXML)

        Parent.AppendChild(NewPlugin)
    End Sub

    Private Sub AddApplicationPluginsElement(ByRef m_Doc As Xml.XmlDocument, ByVal Parent As XmlElement, ByVal LoadingConfig As Boolean)
        'Adds information about application required plugins to the config file XML
        Dim Plugins As XmlElement = m_Doc.CreateElement("ApplicationPlugins")
        Dim Dir As XmlAttribute = m_Doc.CreateAttribute("PluginDir")
        Dim Plugin As Interfaces.IPlugin
        Dim Item As DictionaryEntry
        'Dim PluginInfo As PluginInfo - unused

        'save the application dir
        Dir.InnerText = Me.GetRelativePath(AppInfo.ApplicationPluginDir, ConfigFileName)

        'save all of the application plugins
        For Each Item In frmMain.m_PluginManager.m_ApplicationPlugins
            If Not Item.Value Is Nothing Then
                If TypeOf Item.Value Is Interfaces.IPlugin Then
                    Plugin = CType(Item.Value, Interfaces.IPlugin)
                    AddPluginElement(m_Doc, Plugin, Item.Key.ToString(), Plugins, LoadingConfig)
                Else
                    Plugin = CType(Item.Value, PluginInterfaces.IBasePlugin)
                    AddPluginElement(m_Doc, Plugin, Item.Key.ToString(), Plugins, LoadingConfig)
                End If
            End If
        Next

        Plugins.Attributes.Append(Dir)
        Parent.AppendChild(Plugins)
    End Sub

#End Region

#Region "Save Project File"
    'this region includes functions that are part of saving the project file

    Public Function SaveProject() As Boolean
        'This function saves XML project files. As with the "SaveConfig" function,
        'this expects a current frmMain object from which to grab some info.
        Dim Root As XmlElement
        Dim Ver As String
        Dim ConfigPath As XmlAttribute

        If Len(ProjectFileName) = 0 Then
            Return False
            Exit Function
        End If

        Try
            Ver = App.VersionString()

            '**** add the following elements to "mwprj" ****
            p_Doc = New XmlDocument
            Dim prjName As String = frmMain.Text.Replace("'", "")
            p_Doc.LoadXml("<Mapwin name='" + prjName + "' type='projectfile' version='" + Ver + "'></Mapwin>")
            Root = p_Doc.DocumentElement

            'Add the configuration path
            ConfigPath = p_Doc.CreateAttribute("ConfigurationPath")
            ConfigPath.InnerText = GetRelativePath(ConfigFileName, ProjectFileName)
            Root.Attributes.Append(ConfigPath)

            'Add the projection
            Dim proj As Xml.XmlAttribute = p_Doc.CreateAttribute("ProjectProjection")
            proj.InnerText = ProjectProjection
            Root.Attributes.Append(proj)

            'Add the map units
            Dim mapunit As Xml.XmlAttribute = p_Doc.CreateAttribute("MapUnits")
            mapunit.InnerText = modMain.frmMain.Project.MapUnits
            Root.Attributes.Append(mapunit)

            'Add the status bar coord customizations
            Dim xStatusBarAlternateCoordsNumDecimals As Xml.XmlAttribute = p_Doc.CreateAttribute("StatusBarAlternateCoordsNumDecimals")
            xStatusBarAlternateCoordsNumDecimals.InnerText = StatusBarAlternateCoordsNumDecimals.ToString()
            Root.Attributes.Append(xStatusBarAlternateCoordsNumDecimals)
            Dim xStatusBarCoordsNumDecimals As Xml.XmlAttribute = p_Doc.CreateAttribute("StatusBarCoordsNumDecimals")
            xStatusBarCoordsNumDecimals.InnerText = StatusBarCoordsNumDecimals.ToString()
            Root.Attributes.Append(xStatusBarCoordsNumDecimals)
            Dim xStatusBarAlternateCoordsUseCommas As Xml.XmlAttribute = p_Doc.CreateAttribute("StatusBarAlternateCoordsUseCommas")
            xStatusBarAlternateCoordsUseCommas.InnerText = StatusBarAlternateCoordsUseCommas.ToString()
            Root.Attributes.Append(xStatusBarAlternateCoordsUseCommas)
            Dim xStatusBarCoordsUseCommas As Xml.XmlAttribute = p_Doc.CreateAttribute("StatusBarCoordsUseCommas")
            xStatusBarCoordsUseCommas.InnerText = StatusBarCoordsUseCommas.ToString()
            Root.Attributes.Append(xStatusBarCoordsUseCommas)

            'Add whether to display various coordinate systems in the status bar
            Dim coord_projected As Xml.XmlAttribute = p_Doc.CreateAttribute("ShowStatusBarCoords_Projected")
            coord_projected.InnerText = ShowStatusBarCoords_Projected.ToString()
            Root.Attributes.Append(coord_projected)
            Dim coord_alternate As Xml.XmlAttribute = p_Doc.CreateAttribute("ShowStatusBarCoords_Alternate")
            coord_alternate.InnerText = ShowStatusBarCoords_Alternate
            Root.Attributes.Append(coord_alternate)

            'Add the save shape settings behavior
            Dim saveshapesettinfgsbehavior As Xml.XmlAttribute = p_Doc.CreateAttribute("SaveShapeSettings")
            saveshapesettinfgsbehavior.InnerText = Me.SaveShapeSettings.ToString()
            Root.Attributes.Append(saveshapesettinfgsbehavior)

            'Add this project to the list of recent projects
            AddToRecentProjects(ProjectFileName)

            'Add the list of the plugins to the project file
            AddPluginsElement(p_Doc, Root, False)

            'Add the application plugins
            AddApplicationPluginsElement(p_Doc, Root, False)

            'Save the project file.
            MapWinUtility.Logger.Dbg("Saving Project: " + ProjectFileName)
            p_Doc.Save(ProjectFileName)
            frmMain.SetModified(False)
            Return True
        Catch ex As System.Exception
            ShowError(ex)
            Return False
        End Try
    End Function

#End Region

#Region "Load Config"
    'This region includes functions that are part of loading config files.
    'It is assumed that the frmmain may or may not have been created yet.

    Public Function LoadConfig(ByVal Load_Plugins As Boolean) As Boolean
        'This function loads a config file and returns success or failure.
        'Updated in version 4 to use the configfilename stored in this class and to
        'not use the dotnetbar stuff.
        g_SyncPluginMenuDefer = True
        Try
            Dim Doc As New XmlDocument  'The xmldocument config file
            Dim Root As XmlElement      'An xml element
            ' Try
            'change the cursor to the wait cursor
            If Not frmMain Is Nothing Then
                'if frmmain exists then show a waitcursor
                frmMain.Cursor = System.Windows.Forms.Cursors.WaitCursor
                Windows.Forms.Application.DoEvents()
                'Unload all of the plugins
                frmMain.m_PluginManager.UnloadAll()
                frmMain.m_PluginManager.UnloadApplicationPlugins()
            End If

            ChDir(System.IO.Path.GetDirectoryName(ConfigFileName))

            '**** add the following elements to "mwcfg" ****
            Doc = New XmlDocument

            'Chris M 3/13/2006 - if the config doesn't exist, save -- this will safe a new
            'default.
            If Not System.IO.File.Exists(ConfigFileName) Then
                MapWinUtility.Logger.Dbg("Loading Configuration: Creating default configuration file")
                'Prepare the default application plugin location first:
                AppInfo.ApplicationPluginDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\ApplicationPlugins"
                frmMain.m_PluginManager.LoadApplicationPlugins(AppInfo.ApplicationPluginDir)
                SaveConfig()
            End If

            MapWinUtility.Logger.Dbg("Loading Configuration: " + ConfigFileName)
            Doc.Load(ConfigFileName)
            Root = Doc.DocumentElement

            'load the View
            LoadView(Root.Item("View"))

            'force the mapwindow to show
            'frmMain.Show()
            System.Windows.Forms.Application.DoEvents()

            'load Appinfo
            LoadAppInfo(Root.Item("AppInfo"))

            'load recent files
            LoadRecentProjects(Root.Item("RecentProjects"))

            'load the Plugins
            If Load_Plugins = True Then
                LoadPlugins(Root.Item("Plugins"), True)
            End If

            'load the application plugins
            LoadApplicationPlugins(Root.Item("ApplicationPlugins"), True)

            frmMain.Update()

            '            ConfigFileName = System.IO.Path.GetFullPath(fileName)
            ConfigLoaded = True

            'Catch e As System.Exception
            '    m_ErrorMsg += "Error in LoadConfig(), Message: " + e.Message & Chr(13)
            '    m_ErrorOccured = True
            'End Try

            'change the cursor back to the default
            frmMain.Cursor = System.Windows.Forms.Cursors.Default

            'Dim pluginEnumerator As IDictionaryEnumerator
            'pluginEnumerator = frmMain.m_PluginManager.PluginsList.GetEnumerator()
            'Dim handled As Boolean = False
            'Dim plugin As MapWindow.Interfaces.IPlugin
            'Dim pluginfo As PluginInfo
            'While pluginEnumerator.MoveNext
            '    pluginfo = CType(pluginEnumerator.Value, PluginInfo)
            '    plugin = pluginfo.CreatePluginObject()
            '    Try
            '        plugin.Message("SPLASH_SCREEN", handled)
            '    Catch ex As Exception
            '        TODO()
            '    End Try
            '    If handled Then
            '        Exit While
            '    End If
            'End While

        Catch ex As Exception
            m_ErrorOccured = True
            m_ErrorMsg = ex.ToString
        End Try

        If m_ErrorOccured Then
            MapWinUtility.Logger.Msg(m_ErrorMsg, MsgBoxStyle.Exclamation, "Configuration File Error Report")
            m_ErrorOccured = False
        End If
        g_SyncPluginMenuDefer = False
        frmMain.SynchPluginMenu()
    End Function


    Public Function LoadProject(ByVal Filename As String, Optional ByVal LayersOnly As Boolean = False, Optional ByVal LayersIntoGroup As String = "") As Boolean
        'This loads a project XML file.
        g_SyncPluginMenuDefer = True

        Dim Doc As New XmlDocument
        Dim Root As XmlElement

        If Not System.IO.File.Exists(Filename) Then
            Return False
            Exit Function
        End If

        MapWinUtility.Logger.Dbg("Loading Project: " + Filename)

        Try
            ChDir(System.IO.Path.GetDirectoryName(Filename))
            'Set default directory to folder containing current project
            AppInfo.DefaultDir = CurDir()

            'add the project to the most recent projects
            AddToRecentProjects(Filename)

            Doc.Load(Filename)
            Root = Doc.DocumentElement

            If Not LayersOnly Then
                'Load the config file if it exists
                Try
                    Dim NewConfigFile As String = App.Path & "\light.mwcfg" 'Root.Attributes("ConfigurationPath").InnerText
                    If System.IO.File.Exists(System.IO.Path.GetFullPath(NewConfigFile)) Then
                        'Open the configfile if it isn't already open
                        If ConfigFileName <> System.IO.Path.GetFullPath(NewConfigFile) Then
                            'save the old configFile beforeloading the new one
                            If Not ConfigFileName Is Nothing Then
                                SaveConfig()
                            End If
                            ConfigFileName = System.IO.Path.GetFullPath(NewConfigFile)
                            LoadConfig(True)
                        End If
                    End If
                Catch ex As Exception
                    'No ConfigurationPath or error loading it
                End Try

                'Load the projection if it exists
                Try
                    ProjectProjection = Root.Attributes("ProjectProjection").InnerText
                Catch ex As Exception
                    ProjectProjection = ""
                End Try

                'Load the map units if the setting exists
                Try
                    m_MapUnits = Root.Attributes("MapUnits").InnerText
                Catch ex As Exception
                    m_MapUnits = ""
                End Try

                'Load the status bar coord customizations
                Try
                    StatusBarAlternateCoordsNumDecimals = Integer.Parse(Root.Attributes("StatusBarAlternateCoordsNumDecimals").InnerText)
                    StatusBarCoordsNumDecimals = Integer.Parse(Root.Attributes("StatusBarCoordsNumDecimals").InnerText)
                    StatusBarAlternateCoordsUseCommas = Boolean.Parse(Root.Attributes("StatusBarAlternateCoordsUseCommas").InnerText)
                    StatusBarCoordsUseCommas = Boolean.Parse(Root.Attributes("StatusBarCoordsUseCommas").InnerText)
                Catch ex As Exception
                    StatusBarAlternateCoordsNumDecimals = 3
                    StatusBarCoordsNumDecimals = 3
                    StatusBarAlternateCoordsUseCommas = True
                    StatusBarCoordsUseCommas = True
                End Try

                'Load whether to display various coordinate systems in the status bar.
                'Default to true while doing this.
                Try
                    ShowStatusBarCoords_Projected = Boolean.Parse(Root.Attributes("ShowStatusBarCoords_Projected").InnerText)
                Catch ex As Exception
                    ShowStatusBarCoords_Projected = True
                End Try
                Try
                    ShowStatusBarCoords_Alternate = Root.Attributes("ShowStatusBarCoords_Alternate").InnerText
                Catch ex As Exception
                    ShowStatusBarCoords_Alternate = MapWindow.Interfaces.UnitOfMeasure.Kilometers.ToString()
                End Try
            End If

            ' load the SaveShapeSettings behavior
            Try
                Me.SaveShapeSettings = Boolean.Parse(Root.Attributes("SaveShapeSettings").InnerText)
            Catch ex As Exception
                Me.SaveShapeSettings = False
            End Try


            'make sure we are in the proper directory so relative paths work
            ChDir(System.IO.Path.GetDirectoryName(Filename))

            If Not LayersOnly Then
                'load the Plugins causes it to call project loading
                LoadPlugins(Root.Item("Plugins"), False)

                'load the application plugins causes it to call project loading
                LoadApplicationPlugins(Root.Item("ApplicationPlugins"), False)
            End If

            'BugZilla 315: Default directory should start set to project location
            AppInfo.DefaultDir = System.IO.Path.GetDirectoryName(Filename)

            frmMain.SetModified(False)
            Return True

        Catch e As System.Exception
            m_ErrorMsg += "Error in LoadProject(), Message: " + e.Message + Chr(13)
            m_ErrorOccured = True
        End Try

        If m_ErrorOccured Then
            mapwinutility.logger.msg(m_ErrorMsg, MsgBoxStyle.Exclamation, "Project File Error Report")
            m_ErrorOccured = False
        End If

        g_SyncPluginMenuDefer = False
        frmMain.SynchPluginMenu()
    End Function

#Region "Load Recent Projects"

    Private Sub LoadRecentProjects(ByVal RecentFiles As XmlElement)
        Try
            If (RecentFiles Is Nothing) Then Exit Sub

            Dim iChild As Integer
            Dim iRecentProject As Integer
            Dim path As String
            Dim pathLower As String
            Dim file As Xml.XmlNode
            Dim numChildNodes As Integer = RecentFiles.ChildNodes.Count

            'clear all previous files
            ProjInfo.RecentProjects.Clear()

            For iChild = 0 To numChildNodes - 1
                file = RecentFiles.ChildNodes(iChild)

                'get the full path of the file
                path = System.IO.Path.GetFullPath(file.InnerText)

                'Make sure we don't already have this project in the list.
                'Find a duplicate even if it has different capitalization.
                pathLower = path.ToLower
                iRecentProject = 0
                While iRecentProject < ProjInfo.RecentProjects.Count() AndAlso _
                    ProjInfo.RecentProjects.Item(iRecentProject).ToString.ToLower <> pathLower
                    iRecentProject += 1
                End While
                'iRecentProject = Count means we did not find a duplicate
                'Also, don't add recent projects that no longer exist
                If iRecentProject = ProjInfo.RecentProjects.Count() AndAlso _
                   System.IO.File.Exists(path) Then
                    ProjInfo.RecentProjects.Add(path)
                End If
            Next

            frmMain.BuildRecentProjectsMenu()

        Catch ex As System.Exception
            m_ErrorMsg += "Error: Loading the LoadRecentProjects" + Chr(13)
            m_ErrorOccured = True
            Exit Sub
        End Try
    End Sub

#End Region

    Private Sub LoadAppInfo(ByVal AppInfoXML As XmlElement)
        'Reads the custom application info from the config file.
        'this can be called before frmMain is loaded to determine
        'whether or not to show a splash screen and for how long.
        'Modified 1/16/2005
        Dim Type As String

        Try
            AppInfo.Name = AppInfoXML.Attributes("Name").InnerText
            AppInfo.Version = AppInfoXML.Attributes("Version").InnerText
            AppInfo.BuildDate = AppInfoXML.Attributes("BuildDate").InnerText
            AppInfo.Developer = AppInfoXML.Attributes("Developer").InnerText
            AppInfo.Comments = AppInfoXML.Attributes("Comments").InnerText
            AppInfo.SplashTime = CInt(AppInfoXML.Attributes("SplashTime").InnerText)

            Try
                AppInfo.LogfilePath = AppInfoXML.Attributes("LogfilePath").InnerText
                'Enable logging:
                MapWinUtility.Logger.StartToFile(AppInfo.LogfilePath, False, True, False)
            Catch
                AppInfo.LogfilePath = ""
            End Try

            Try
                NoPromptToSendErrors = CBool(AppInfoXML.Attributes("NoPromptToSendErrors").InnerText)
            Catch
                NoPromptToSendErrors = False
            End Try

            Dim NeverShowProjectionDialog As Boolean = False
            Try
                NeverShowProjectionDialog = CBool(AppInfoXML.Attributes("NeverShowProjectionDialog").InnerText)
            Catch ex As Exception
            End Try
            AppInfo.NeverShowProjectionDialog = NeverShowProjectionDialog

            If AppInfoXML.Attributes("WelcomePlugin") Is Nothing Then
                AppInfo.WelcomePlugin = Nothing
            Else
                AppInfo.WelcomePlugin = AppInfoXML.Attributes("WelcomePlugin").InnerText
            End If


            Try
                Dim strPath As String = System.IO.Path.GetFullPath(AppInfoXML.Attributes("DefaultDir").InnerText)
                If strPath <> Nothing Then
                    AppInfo.DefaultDir = strPath
                End If
            Catch
                'Should do some kind of logging here if the dir is invalid
            End Try

            If (AppInfoXML.HasAttribute("URL")) Then
                AppInfo.URL = AppInfoXML.Attributes("URL").InnerText
            End If

            If (AppInfoXML.HasAttribute("ShowWelcomeScreen")) Then
                AppInfo.ShowWelcomeScreen = Boolean.Parse(AppInfoXML.Attributes("ShowWelcomeScreen").InnerText)
            End If

            If (AppInfoXML.Attributes("HelpFilePath").InnerText <> "") Then
                AppInfo.HelpFilePath = System.IO.Path.GetFullPath(AppInfoXML.Attributes("HelpFilePath").InnerText)
            Else
                AppInfo.HelpFilePath = ""
            End If

            If (AppInfo.SplashTime < 0) Then
                AppInfo.SplashTime = 0
            End If

            'load the window title
            'Version Numbers: frmMain.Text = AppInfo.Name + " " + App.VersionString ' for now, will be rewritten later
            frmMain.Text = AppInfo.Name + " " ' for now, will be rewritten later

            'load the help munu text
            frmMain.m_Menu.AddMenu("mnuAboutMapWindow", "mnuHelp", Nothing, "&About " & AppInfo.Name)

            'load the Splash image
            With AppInfoXML.Item("SplashPicture").Item("Image")
                Type = .Attributes("Type").InnerText
                AppInfo.SplashPicture = CType(ConvertStringToImage(.InnerText, Type), Image)
            End With

            'load the Application icon
            With AppInfoXML.Item("WindowIcon").Item("Image")
                Type = .Attributes("Type").InnerText
                AppInfo.FormIcon = CType(ConvertStringToImage(.InnerText, Type), Icon)
                frmMain.Icon = CType(ConvertStringToImage(.InnerText, Type), Icon)
            End With

        Catch e As System.Exception
            m_ErrorMsg += "Error: Loading the appinfoxml" + Chr(13)
            m_ErrorOccured = True
            Exit Sub
        End Try
    End Sub

#Region "Load View Functions"

    Private Function LoadView(ByVal view As XmlElement) As Boolean
        Try
            frmMain.WindowState = CType(view.Attributes("WindowState").InnerText, Windows.Forms.FormWindowState)

            If frmMain.WindowState = FormWindowState.Normal Then
                frmMain.Width = CInt(view.Attributes("WindowWidth").InnerText)
                frmMain.Height = CInt(view.Attributes("WindowHeight").InnerText)

                Dim drawPoint As New System.Drawing.Point(CInt(view.Attributes("LocationX").InnerText), CInt(view.Attributes("LocationY").InnerText))
                frmMain.Location = drawPoint
            End If

            If frmMain.WindowState = FormWindowState.Minimized Then
                frmMain.WindowState = FormWindowState.Normal
            End If

        Catch e As System.Exception
            m_ErrorMsg += "Error in LoadView(), Message: " + e.Message + Chr(13)
            m_ErrorOccured = True
            LoadView = False
            Exit Function
        End Try

        LoadView = True
    End Function

#End Region

#Region "Load Plugins"

    Private Sub LoadApplicationPlugins(ByVal plugins As XmlElement, ByVal loadingConfig As Boolean)
        Dim count As Integer
        Dim i As Integer

        'exit if this element does not exists
        If (plugins Is Nothing) Then Exit Sub

        If (loadingConfig) Then
            'get the application plugin dir
            If (plugins.Attributes("PluginDir").InnerText <> "") Then
                AppInfo.ApplicationPluginDir = System.IO.Path.GetFullPath(plugins.Attributes("PluginDir").InnerText)
            End If

            frmMain.m_PluginManager.LoadApplicationPlugins(AppInfo.ApplicationPluginDir)
        End If

        count = plugins.ChildNodes.Count
        For i = 0 To count - 1
            Try
                LoadPlugin(plugins.ChildNodes(i), loadingConfig, True)
            Catch e As System.Exception
                m_ErrorMsg += "Error in LoadApplicationPlugins(), Message: " + e.Message + Chr(13)
                m_ErrorOccured = True
            End Try
        Next
    End Sub

    Private Sub LoadPlugins(ByVal plugins As XmlElement, ByVal loadingConfig As Boolean)
        Dim count As Integer
        Dim i As Integer

        count = plugins.ChildNodes.Count
        For i = 0 To count - 1
            '       Try
            LoadPlugin(plugins.ChildNodes(i), loadingConfig, False)
            'Catch e As System.Exception
            '    m_ErrorMsg += "Error in LoadPlugins(), Message: " + e.Message + Chr(13)
            '    m_ErrorOccured = True
            'End Try
        Next

    End Sub

    Private Sub LoadPlugin(ByVal plugin As XmlNode, ByVal loadingConfig As Boolean, ByVal loadingApplictionPlugins As Boolean)
        Dim settingsString As String = plugin.Attributes("SettingsString").InnerText
        Dim key As String = plugin.Attributes("Key").InnerText

        'load the plugin if needed
        If Not frmMain.m_PluginManager.PluginIsLoaded(key) Then
            frmMain.m_PluginManager.StartPlugin(key)
        End If

        'send the loading event
        frmMain.m_PluginManager.ProjectLoading(key, ProjectFileName, settingsString)
    End Sub

#End Region

#End Region

#Region "Utilities"

    Shared Sub SaveMainToolbarButtons()

        'Try
        'store the mapwindow default button items
        Dim item As Collections.DictionaryEntry
        Dim enumerator As Collections.IEnumerator = frmMain.m_Toolbar.m_Buttons.GetEnumerator
        While (enumerator.MoveNext)
            item = CType(enumerator.Current, Collections.DictionaryEntry)
            If (Not m_MainToolbarButtons.ContainsKey(item.Key)) Then
                m_MainToolbarButtons.Add(item.Key, item.Value)
            End If
        End While

        'store the mapwindow default bars items
        enumerator = frmMain.m_Toolbar.tbars.GetEnumerator
        While (enumerator.MoveNext)
            item = CType(enumerator.Current, Collections.DictionaryEntry)
            If (Not m_MainToolbarButtons.ContainsKey(item.Key)) Then
                m_MainToolbarButtons.Add(item.Key, item.Value)
            End If
        End While

        'store the mapwindow default menus items
        enumerator = frmMain.m_Menu.m_MenuTable.GetEnumerator
        While (enumerator.MoveNext)
            item = CType(enumerator.Current, Collections.DictionaryEntry)
            If (Not m_MainToolbarButtons.ContainsKey(item.Key)) Then
                m_MainToolbarButtons.Add(item.Key, item.Value)
            End If
        End While
        'Catch ex As Exception
        '  ShowError(ex)
        'End Try
    End Sub

    Private Function ConvertImageToString(ByVal img As Object, ByRef type As String) As String
        Dim s As String = ""
        Dim path As String = System.IO.Path.GetTempFileName()

        If Not img Is Nothing Then
            Try
                'find the type of image it is
                If TypeOf img Is Icon Then
                    type = "Icon"
                    Dim image As Icon = CType(img, Icon)

                    'write the image to a temp file
                    Dim outStream As IO.Stream = IO.File.OpenWrite(path)
                    image.Save(outStream)
                    outStream.Close()
                    'ElseIf TypeOf img Is stdole.IPictureDisp Then
                    '    type = "IPictureDisp"
                    '    Dim cvter As New MapWinUtility.ImageUtils
                    '    Dim image As Image = New Bitmap(cvter.IPictureDispToImage(img))

                    '    'save bitmap
                    '    image.Save(path)
                ElseIf TypeOf img Is Bitmap Then
                    type = "Bitmap"
                    Dim image As Image = CType(img, Bitmap)

                    'save bitmap
                    image.Save(path)
                Else
                    type = "Unknown"
                    Return ""
                End If

                'initialize the reader to read binary
                Dim inStream As IO.Stream = IO.File.OpenRead(path)
                Dim reader As New System.IO.BinaryReader(inStream)

                'read in each byte and convert it to a char
                Dim numbytes As Long = reader.BaseStream.Length
                s = System.Convert.ToBase64String(reader.ReadBytes(CInt(numbytes)))

                reader.Close()

                'delete the temp file
                System.IO.File.Delete(path)

                Return s
            Catch e As System.Exception
                m_ErrorMsg += "Error in ConvertImageToString(), Message: " + e.Message + Chr(13)
                m_ErrorOccured = True
                Return s
            End Try
        End If

        If (System.IO.File.Exists(path)) Then
            System.IO.File.Delete(path)
        End If

        Return s
    End Function

    Private Function ConvertStringToImage(ByVal image As String, ByVal type As String) As Object
        Dim icon As Icon
        Dim bmp As Bitmap
        Dim mybyte() As Byte
        Dim path As String
        Dim outStream As IO.Stream

        If Len(image) > 0 Then
            Try
                path = System.IO.Path.GetTempFileName()
                g_KillList.Add(path)

                outStream = IO.File.OpenWrite(path)

                mybyte = System.Convert.FromBase64String(image)
                'write the image to a temp file
                ' cdm - modernize: size = UBound(mybyte)
                ' cdm - modernize: For i = 0 To size
                outStream.Write(mybyte, 0, mybyte.Length)
                ' cdm - modernize: Next
                outStream.Close()

                'open the image
                Select Case type
                    Case "Icon"
                        icon = New Icon(path)
                        Return icon
                    Case "Bitmap"
                        bmp = New Bitmap(path)
                        Return bmp
                    Case "IPictureDisp"
                        bmp = New Bitmap(path)
                        Dim cvter As New MapWinUtility.ImageUtils
                        Return cvter.ImageToIPictureDisp(bmp)
                End Select

            Catch ex As System.Exception
                MapWinUtility.Logger.Msg(ex.Message, "Error converting string to image in project file")
            End Try
        End If

        Return Nothing
    End Function

    Public Function GetRelativePath(ByVal Filename As String, ByVal ProjectFile As String) As String
        GetRelativePath = ""
        Dim a() As String, b() As String
        Dim i As Integer, j As Integer, k As Integer, Offset As Integer

        If Len(Filename) = 0 Or Len(ProjectFile) = 0 Then
            Return ""
        End If

        Try
            'If the drive is different then use the full path
            If System.IO.Path.GetPathRoot(Filename).ToLower() <> System.IO.Path.GetPathRoot(ProjectFile).ToLower() Then
                GetRelativePath = Filename
                Exit Function
            End If
            '
            'load a()
            ReDim a(0)
            a(0) = Filename
            i = 0
            Do
                i = i + 1
                ReDim Preserve a(i)
                Try
                    a(i) = System.IO.Directory.GetParent(a(i - 1)).FullName.ToLower()
                Catch
                End Try
            Loop Until a(i) = ""
            '
            'load b()
            ReDim b(0)
            b(0) = ProjectFile
            i = 0
            Do
                i = i + 1
                ReDim Preserve b(i)
                Try
                    b(i) = System.IO.Directory.GetParent(b(i - 1)).FullName.ToLower()
                Catch
                End Try
            Loop Until b(i) = ""
            '
            'look for match
            For i = 0 To UBound(a)
                For j = 0 To UBound(b)
                    If a(i) = b(j) Then
                        'found match
                        GoTo [CONTINUE]
                    End If
                Next j
            Next i
[CONTINUE]:
            ' j is num steps to get from BasePath to common path
            ' so I need this many of "..\"
            For k = 1 To j - 1
                GetRelativePath = GetRelativePath & "..\"
            Next k

            'everything past a(i) needs to be appended now.
            If a(i).EndsWith("\") Then
                Offset = 0
            Else
                Offset = 1
            End If
            GetRelativePath = GetRelativePath & Filename.Substring(Len(a(i)) + Offset)
        Catch e As System.Exception
            Return ""
        End Try
    End Function

    Public Sub DeleteShapeFile(ByVal fileName As String)
        'Function for deleting a shapefile with its three pieces.
        Dim f1, f2, f3 As String

        f1 = System.IO.Path.ChangeExtension(fileName, ".shp")
        f2 = System.IO.Path.ChangeExtension(fileName, ".shx")
        f3 = System.IO.Path.ChangeExtension(fileName, ".dbf")

        If System.IO.File.Exists(f1) Then System.IO.File.Delete(f1)
        If System.IO.File.Exists(f2) Then System.IO.File.Delete(f2)
        If System.IO.File.Exists(f3) Then System.IO.File.Delete(f3)
    End Sub

    Private Sub AddToRecentProjects(ByVal ProjectName As String)
        Try
            'Remove any recent project names that match this one
            Dim NewNameLower As String = ProjectName.ToLower
            Dim iRecent As Integer = ProjInfo.RecentProjects.Count - 1
            While iRecent >= 0
                If CStr(ProjInfo.RecentProjects.Item(iRecent)).ToLower = NewNameLower Then
                    ProjInfo.RecentProjects.RemoveAt(iRecent)
                End If
                iRecent -= 1
            End While

            'Add this name to the start of the list
            ProjInfo.RecentProjects.Insert(0, ProjectName)

            'Make sure the list doesn't get longer than 10 items
            If (ProjInfo.RecentProjects.Count > 10) Then
                ProjInfo.RecentProjects.RemoveAt(ProjInfo.RecentProjects.Count - 1)
            End If
            frmMain.BuildRecentProjectsMenu()
        Catch ex As System.Exception
            ShowError(ex)
        End Try
    End Sub

#End Region

    Public Function GetSplashInfo() As Boolean
        'This is a new function in version 4.  It loads just enough of the project and config
        'file info to determine what to do about the splash screen.
        Dim Doc As XmlDocument
        Dim Root As XmlElement
        Dim TempPath As String

        If ProjectFileName.Length > 0 Then
            If System.IO.File.Exists(ProjectFileName) Then
                Doc = New XmlDocument
                Doc.Load(ProjectFileName)
                Root = Doc.DocumentElement
                ConfigFileName = ""  'Root.Attributes("ConfigurationPath").InnerText
            Else
                ProjectFileName = ""
            End If
        End If
        If ConfigFileName = "" Then
            ConfigFileName = App.Path & "\light.mwcfg"
        End If
        'convert from relative path to full path
        TempPath = System.IO.Path.GetFullPath(System.IO.Path.GetDirectoryName(ProjectFileName) & "\" & ConfigFileName)
        If System.IO.File.Exists(TempPath) Then
            'it exists so load just the needed appinfo stuff
            ConfigFileName = TempPath
            Doc = New XmlDocument
            Doc.Load(ConfigFileName)
            Root = Doc.DocumentElement
            LoadAppInfo(Root.Item("AppInfo"))
        Else
            Return False    'this is the worst case scenario in which we can't find the default.mwcfg file
        End If
        If AppInfo.SplashPicture Is Nothing Then
            Dim img As New Drawing.Bitmap(Me.GetType, "splash screen.bmp")
            AppInfo.SplashPicture = img
        End If
    End Function
End Class
