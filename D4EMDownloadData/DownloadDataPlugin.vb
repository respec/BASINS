Imports MapWinUtility
#If GISProvider = "DotSpatial" Then
Imports DotSpatial.Controls
Imports DotSpatial.Extensions
#End If

Public Class DownloadDataPlugin
#If GISProvider = "DotSpatial" Then
    Private g_Menus As DotSpatial.Controls.LayoutMenuStrip
    Friend Shared g_MapWin As AppManager
    Public Shared DSProject As DotSpatial.Controls.SerializationManager = Nothing
#Else
    Implements MapWindow.Interfaces.IPlugin
    Private g_Menus As MapWindow.Interfaces.Menus
    Friend Shared g_MapWin As MapWindow.Interfaces.IMapWin
#End If
    Private g_MainForm As Integer

    Private Const pMenuLabel As String = "Download Data"
    Private Const pMenuName As String = "mnuDownloadDataD4EM"

#If GISProvider = "DotSpatial" Then
    Public Sub Initialize(ByVal MapWin As AppManager, ByVal ParentHandle As Integer)
        g_MapWin = MapWin
        g_Menus = Nothing
        g_MainForm = ParentHandle
        ' AddMenuIfMissing(pMenuName, "mnuFile", pMenuLabel, "mnuFileBreak2")
        Dim lSettingArgs As New atcData.atcDataAttributes()
        Dim lcacheDir As String = MapWin.SerializationManager.CurrentProjectDirectory
        If Not IO.Directory.Exists(lcacheDir) Then
            lcacheDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        End If
        With lSettingArgs
            .SetValue("MapWin", MapWin)
            .SetValue("CacheDir", lcacheDir)
        End With
        BASINS.Initialize(lSettingArgs)
    End Sub
    Public Sub Show(ByVal aItemName As String, ByRef aHandled As Boolean)
        Select Case aItemName
            Case pMenuName
                aHandled = True
                'atcMwGisUtility.GisUtil.MappingObject = g_MapWin
                'If BASINS.NationalProjectIsOpen() Then
                'BASINS.SpecifyAndCreateNewProject()
                'ElseIf g_MapWin.Layers.NumLayers < 1 AndAlso (g_MapWin.Project Is Nothing OrElse g_MapWin.Project.FileName Is Nothing) Then
                'BASINS.LoadNationalProject()
                'Else
                Dim lDownloadForm As New frmDownload
                Dim lQuery As String = lDownloadForm.AskUser(g_MapWin, g_MainForm)
                If lQuery.Length > 0 Then
                    If lQuery.Equals(frmDownload.CancelString) Then
                        'User cancelled download form
                    Else
                        'g_MapWin.View.MapCursor = MapWinGIS.tkCursor.crsrWait
                        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor 'Logger.Busy = True
                        System.Windows.Forms.Application.DoEvents() 'refresh main form to get rid of vestiges of download form
                        'Make sure all loaded plugins are available for DataManager
                        'Dim lPlugins As New ArrayList
                        'For lPluginIndex As Integer = 0 To g_MapWin.Plugins.Count
                        '    Try
                        '        If Not g_MapWin.Plugins.Item(lPluginIndex) Is Nothing Then
                        '            lPlugins.Add(g_MapWin.Plugins.Item(lPluginIndex))
                        '        End If
                        '    Catch ex As Exception
                        '    End Try
                        'Next
                        'Dim lDownloadManager As New D4EMDataManager.DataManager(lPlugins)
                        Logger.Status("LABEL TITLE BASINS Data Download")
                        Dim lResult As String = atcD4EMLauncher.Execute(lQuery)
                        If lResult Is Nothing Then
                            Logger.Dbg("QueryResult:Nothing")
                        Else
                            'Logger.Msg(lResult, "Result of Query from DataManager")
                            Logger.Dbg("QueryResult:" & lResult)
                            Dim lSilentSuccess As Boolean = lResult.ToLower.Contains("<success />")
                            If lSilentSuccess Then
                                lResult = lResult.Replace("<success />", "").Trim
                                Logger.Dbg("QueryResultTrimmed:" & lResult)
                            End If
                            If lResult.Length = 0 Then
                                If lSilentSuccess Then Logger.Msg("Download Complete", "Data Download")
                            ElseIf lResult.Contains("<success>") Then
                                BASINS.ProcessDownloadResults(lResult)
                            Else
                                If lResult.Contains("<error>Error downloading from https://www.mrlc.gov") Then
                                    'pbd better error message here when server down?
                                    ''"<error>Error downloading from https://www.mrlc.gov/geoserver/mrlc_download/wms?SERVICE=WMS&request=GetMap&layers=NLCD_2019_Land_Cover_L48&bbox=-84.3885248345288, 33.4322709170764, -75.0747797710852, 38.044461713196&width=25995&height=13002&srs=EPSG:4326&format=image/geotiff&version=1.1.1, file not created: C:\USGSHydroToolboxDS\HydrologicToolbox1.1\data\03020201\NLCD\NLCD_landcover_2019.tif</error>"
                                    Logger.Msg("NLCD WMS Server not responding, appears to be down", "Data Download")
                                Else
                                    Logger.Msg(atcUtility.ReadableFromXML(lResult), "Data Download")
                                End If
                            End If
                        End If

                        'g_MapWin.View.MapCursor = MapWinGIS.tkCursor.crsrMapDefault
                        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default 'Logger.Busy = False
                        Logger.Status("LABEL TITLE BASINS Status")
                    End If
                End If
                'End If
        End Select
    End Sub
#Else
#Region "Plug-in Information"

    Public ReadOnly Property Name() As String Implements MapWindow.Interfaces.IPlugin.Name
        Get
            Return Description
        End Get
    End Property

    Public ReadOnly Property Author() As String Implements MapWindow.Interfaces.IPlugin.Author
        Get
            Return "AQUA TERRA Consultants"
        End Get
    End Property

    Public ReadOnly Property BuildDate() As String Implements MapWindow.Interfaces.IPlugin.BuildDate
        Get
            Return IO.File.GetLastWriteTime(Me.GetType().Assembly.Location)
        End Get
    End Property

    Public ReadOnly Property Version() As String Implements MapWindow.Interfaces.IPlugin.Version
        Get
            Return Diagnostics.FileVersionInfo.GetVersionInfo(Me.GetType().Assembly.Location).FileVersion
        End Get
    End Property

    Public ReadOnly Property Description() As String Implements MapWindow.Interfaces.IPlugin.Description
        Get
            Return "D4EM Data Download::Main"
        End Get
    End Property
#End Region

#Region "Unused Plug-in Interface Elements"

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

    Public ReadOnly Property SerialNumber() As String Implements MapWindow.Interfaces.IPlugin.SerialNumber
        Get
            Return ""
        End Get
    End Property

    Public Sub ShapesSelected(ByVal Handle As Integer, ByVal SelectInfo As MapWindow.Interfaces.SelectInfo) Implements MapWindow.Interfaces.IPlugin.ShapesSelected
    End Sub
#End Region

#Region "Copied from atcDataManager"
    Private Function AddMenuIfMissing(ByVal aMenuName As String,
                                  ByVal aParent As String,
                                  ByVal aMenuText As String,
                         Optional ByVal aAfter As String = "",
                         Optional ByVal aBefore As String = "",
                         Optional ByVal aAlphabetical As Boolean = False) _
                                     As MapWindow.Interfaces.MenuItem
        Dim lMenu As MapWindow.Interfaces.MenuItem = g_Menus.Item(aMenuName)
        If Not lMenu Is Nothing Then 'This item already exists
            Return lMenu
        ElseIf aAlphabetical And aParent.Length > 0 Then
            'Need parent to do alphabetical search for position
            Dim lParentMenu As MapWindow.Interfaces.MenuItem = g_Menus.Item(aParent)
            Dim lSubmenuIndex As Integer = 0
            Dim lExistingMenu As MapWindow.Interfaces.MenuItem

            If aAfter.Length > 0 Then
                'First make sure we are after a particular item
                While lSubmenuIndex < lParentMenu.NumSubItems
                    lExistingMenu = lParentMenu.SubItem(lSubmenuIndex)
                    If Not lExistingMenu Is Nothing AndAlso
                       Not lExistingMenu.Name Is Nothing AndAlso
                           lExistingMenu.Name.Equals(aAfter) Then
                        Exit While
                    End If
                    lExistingMenu = Nothing
                    lSubmenuIndex += 1
                End While
                If lSubmenuIndex >= lParentMenu.NumSubItems Then
                    'Did not find menu aAfter, so start at first subitem
                    lSubmenuIndex = 0
                End If
            End If

            'Find alphabetical position for new menu item
            While lSubmenuIndex < lParentMenu.NumSubItems
                lExistingMenu = lParentMenu.SubItem(lSubmenuIndex)
                If Not lExistingMenu Is Nothing AndAlso
                   Not lExistingMenu.Name Is Nothing Then
                    If (aBefore.Length > 0 AndAlso lExistingMenu.Text = aBefore) OrElse
                       lExistingMenu.Text > aMenuText Then
                        'Add before existing menu with alphabetically later text
                        Return g_Menus.AddMenu(aMenuName, aParent, aMenuText, lExistingMenu.Name)
                    End If
                End If
                lSubmenuIndex += 1
            End While
            'Add at default position, after last parent subitem
            Return g_Menus.AddMenu(aMenuName, aParent, Nothing, aMenuText)
        ElseIf aBefore.Length > 0 Then
            Return g_Menus.AddMenu(aMenuName, aParent, aMenuText, aBefore)
        Else
            Return g_Menus.AddMenu(aMenuName, aParent, Nothing, aMenuText, aAfter)
        End If
    End Function

#End Region

    Public Sub Initialize(ByVal MapWin As MapWindow.Interfaces.IMapWin, ByVal ParentHandle As Integer) Implements MapWindow.Interfaces.IPlugin.Initialize
        g_MapWin = MapWin
        g_Menus = MapWin.Menus
        g_MainForm = ParentHandle
        AddMenuIfMissing(pMenuName, "mnuFile", pMenuLabel, "mnuFileBreak2")
    End Sub

    Public Sub Terminate() Implements MapWindow.Interfaces.IPlugin.Terminate
        g_Menus.Remove(pMenuName)
    End Sub

    Public Sub ItemClicked(ByVal aItemName As String, ByRef aHandled As Boolean) Implements MapWindow.Interfaces.IPlugin.ItemClicked
        Select Case aItemName
            Case pMenuName
                aHandled = True
                atcMwGisUtility.GisUtil.MappingObject = g_MapWin
                If BASINS.NationalProjectIsOpen() Then
                    BASINS.SpecifyAndCreateNewProject()
                ElseIf g_MapWin.Layers.NumLayers < 1 AndAlso (g_MapWin.Project Is Nothing OrElse g_MapWin.Project.FileName Is Nothing) Then
                    BASINS.LoadNationalProject()
                Else
                    Dim lDownloadForm As New frmDownload
                    Dim lQuery As String = lDownloadForm.AskUser(g_MapWin, g_MainForm)
                    'Logger.Msg(lQuery, "Query from frmDownload")
                    If lQuery.Length > 0 Then
                        If lQuery.Equals(frmDownload.CancelString) Then
                            'User cancelled download form
                        Else
                            g_MapWin.View.MapCursor = MapWinGIS.tkCursor.crsrWait
                            Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor 'Logger.Busy = True
                            Windows.Forms.Application.DoEvents() 'refresh main form to get rid of vestiges of download form
                            'Make sure all loaded plugins are available for DataManager
                            'Dim lPlugins As New ArrayList
                            'For lPluginIndex As Integer = 0 To g_MapWin.Plugins.Count
                            '    Try
                            '        If Not g_MapWin.Plugins.Item(lPluginIndex) Is Nothing Then
                            '            lPlugins.Add(g_MapWin.Plugins.Item(lPluginIndex))
                            '        End If
                            '    Catch ex As Exception
                            '    End Try
                            'Next
                            'Dim lDownloadManager As New D4EMDataManager.DataManager(lPlugins)
                            Logger.Status("LABEL TITLE BASINS Data Download")
                            Dim lResult As String = atcD4EMLauncher.Execute(lQuery)
                            If lResult Is Nothing Then
                                Logger.Dbg("QueryResult:Nothing")
                            Else
                                'Logger.Msg(lResult, "Result of Query from DataManager")
                                Logger.Dbg("QueryResult:" & lResult)
                                Dim lSilentSuccess As Boolean = lResult.ToLower.Contains("<success />")
                                If lSilentSuccess Then
                                    lResult = lResult.Replace("<success />", "").Trim
                                    Logger.Dbg("QueryResultTrimmed:" & lResult)
                                End If
                                If lResult.Length = 0 Then
                                    If lSilentSuccess Then Logger.Msg("Download Complete", "Data Download")
                                ElseIf lResult.Contains("<success>") Then
                                    BASINS.ProcessDownloadResults(lResult)
                                Else
                                    Logger.Msg(atcUtility.ReadableFromXML(lResult), "Data Download")
                                End If
                            End If

                            g_MapWin.View.MapCursor = MapWinGIS.tkCursor.crsrMapDefault
                            Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default 'Logger.Busy = False
                            Logger.Status("LABEL TITLE BASINS Status")
                        End If
                    End If
                End If
        End Select
    End Sub
#End If
End Class
