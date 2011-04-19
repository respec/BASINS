Imports atcData.atcDataManager

''' <summary>
'''     <para>Base class for plugins that can read, write, manipulate, or display atcData</para>
''' </summary>
''' <remarks>
'''     <para>This class implements MapWindow.Interfaces.IPlugin so it can be loaded by 
'''     the MapWindow plugin code</para>
'''     <para>Name must be overridden with a unique name for a child class to be loaded as a plugin. 
'''     Others may be overridden if desired.</para>
''' </remarks>
Public Class atcDataPlugin

#If GISProvider = "DotSpatial" Then

#Else
    Implements MapWindow.Interfaces.IPlugin
#End If

    Private Shared pNextSerial As Integer = 0 'Next serial number to be assigned
    Private pSerial As Integer 'Serial number of this object, assigned in order of creation at runtime

    ''' <summary>create a new atcDataPlugin</summary>
    Public Sub New()
        pSerial = System.Threading.Interlocked.Increment(pNextSerial) 'Safely increment pNextSerial
    End Sub

    ''' <summary></summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable ReadOnly Property Icon() As System.Drawing.Icon
        Get
            Return Nothing
        End Get
    End Property

    ''' <summary>Calls the version of New with no arguments.</summary>
    ''' <remarks>Inheriting classes that have no New w/o arguments must override.</remarks>
    Public Overridable Function NewOne() As atcDataPlugin
        Return Me.GetType.InvokeMember(Nothing, Reflection.BindingFlags.CreateInstance, Nothing, Nothing, New Object() {})
    End Function

    Public Overrides Function ToString() As String
        Return Name & ":" & Version & ":" & SerialNumber
    End Function

#If GISProvider = "DotSpatial" Then
    ''' <summary></summary>
    ''' <remarks></remarks>
    Public Overridable ReadOnly Property Name() As String  
        Get
            Return ""
        End Get
    End Property

    ''' <summary></summary>
    ''' <remarks> </remarks>
    Public Overridable ReadOnly Property SerialNumber() As String  
        Get
            Return pSerial.ToString
        End Get
    End Property

#Else

    <CLSCompliant(False)> _
    Protected pMapWin As MapWindow.Interfaces.IMapWin
    Protected pMapWinWindowHandle As Integer
    Protected pMenusAdded As New ArrayList

    ''' <summary>
    ''' String that appears in the MapWindow Plug-ins menu to identify this plug-in.
    ''' </summary>
    ''' <requirements>
    ''' Must be overridden in inheriting class to return something unique or plugin will not load.
    ''' </requirements>
    Public Overridable ReadOnly Property Name() As String Implements MapWindow.Interfaces.IPlugin.Name
        Get
            Return ""
        End Get
    End Property

    ''' <summary>A company name, individual, or organization name.</summary>
    Public Overridable ReadOnly Property Author() As String Implements MapWindow.Interfaces.IPlugin.Author
        Get
            Return "AQUA TERRA Consultants"
        End Get
    End Property

    ''' <summary>Groups related of plugins together in the UI</summary>
    ''' <remarks>Suggested categories include "File", "Computation", and "Download"</remarks>
    Public Overridable ReadOnly Property Category() As String
        Get
            Return ""
        End Get
    End Property

    ''' <summary>Longer version of <see cref="Name">Name</see> with room to expand acronyms</summary>
    ''' <remarks>Appears in the plug-ins dialog box when a user selects this plug-in.</remarks>
    Public Overridable ReadOnly Property Description() As String Implements MapWindow.Interfaces.IPlugin.Description
        Get
            Return ""
        End Get
    End Property

    ''' <summary>Date plug-in was built.</summary>
    ''' <remarks>
    ''' Either return a string of a hard-coded date such as "January 1, 2003" or
    ''' dynamically obtain the build date of the assembly.
    ''' </remarks>
    Public Overridable ReadOnly Property BuildDate() As String Implements MapWindow.Interfaces.IPlugin.BuildDate
        Get
            Return System.IO.File.GetLastWriteTime(Me.GetType().Assembly.Location).ToString()
        End Get
    End Property

    ''' <remarks>
    ''' Can either return a hard-coded string such as "1.0.0.1" or use<br />
    ''' GetVersionInfo to dynamically return the version number from the assembly itself.
    ''' </remarks>
    ''' <summary>Version number of the plug-in</summary>
    Public Overridable ReadOnly Property Version() As String Implements MapWindow.Interfaces.IPlugin.Version
        Get
            Return System.Diagnostics.FileVersionInfo.GetVersionInfo(Me.GetType().Assembly.Location).FileVersion
        End Get
    End Property

    ''' <summary>Fired when the plugin is loaded 
    ''' (by the plug-in dialog, the plugins menu, or at program start).
    ''' </summary>
    ''' <remarks>
    ''' A good time to add buttons to the tool bar or menu items to the menu.<br />
    ''' Save a reference to the IMapWin that is passed for later access to MapWindow.
    ''' </remarks>
    <CLSCompliant(False)> _
    Public Overridable Sub Initialize(ByVal aMapWin As MapWindow.Interfaces.IMapWin, _
                                      ByVal aParentHandle As Integer) _
                              Implements MapWindow.Interfaces.IPlugin.Initialize
        pMapWin = aMapWin
        pMapWinWindowHandle = aParentHandle
        atcDataManager.MapWindow = aMapWin

        AddMenuIfMissing(NewDataMenuName, FileMenuName, NewDataMenuString, "mnuNew")
        AddMenuIfMissing(OpenDataMenuName, FileMenuName, OpenDataMenuString, "mnuOpen")
        AddMenuIfMissing(ManageDataMenuName, FileMenuName, ManageDataMenuString, OpenDataMenuName)
        AddMenuIfMissing(SaveDataMenuName, FileMenuName, SaveDataMenuString, "mnuSaveAs")

        Dim lSubMenus As New Generic.List(Of String)
        Dim lRest As String = Name.Clone
        While lRest.Contains("::")
            lSubMenus.Add(MapWinUtility.Strings.StrSplit(lRest, "::", ""))
        End While
        If lSubMenus.Count > 0 Then
            Select Case lSubMenus(0)
                Case "Analysis"
                    'Add Analysis menu if it does not yet exist
                    Dim lParentMenuName As String = ""
                    Dim lMenuName As String = atcDataManager.AnalysisMenuName
                    atcDataManager.AddMenuIfMissing(lMenuName, "", atcDataManager.AnalysisMenuString, atcDataManager.FileMenuName)
                    lParentMenuName = lMenuName

                    'Add sub-menus if neeeded
                    For lLevel As Integer = 1 To lSubMenus.Count - 1
                        If lSubMenus(lLevel).Length > 0 Then
                            lMenuName &= "_" & lSubMenus(lLevel)
                            atcDataManager.AddMenuWithIcon(lMenuName, lParentMenuName, lSubMenus(lLevel), Me.Icon)
                            lParentMenuName = lMenuName
                        End If
                    Next

                    'Add menu item for this analysis
                    pMenusAdded.Add(atcDataManager.AddMenuWithIcon(atcDataManager.AnalysisMenuName & "_" & Name, lParentMenuName, lRest, Me.Icon, , , True))

                Case "Timeseries"
                    Try
                        Dim lDataSource As atcDataSource = Me
                        If lDataSource.Category <> "File" Then
                            Dim lCategoryMenuName As String = atcDataManager.ComputeMenuName & "_" & lDataSource.Category
                            Dim lOperations As atcDataAttributes = lDataSource.AvailableOperations
                            If Not lOperations Is Nothing AndAlso lOperations.Count > 0 Then
                                For Each lOperation As atcDefinedValue In lOperations
                                    Select Case lOperation.Definition.TypeString
                                        Case "atcTimeseries", "atcDataGroup", "atcTimeseriesGroup"
                                            atcDataManager.AddMenuIfMissing(atcDataManager.ComputeMenuName, "", atcDataManager.ComputeMenuString, atcDataManager.FileMenuName)
                                            pMenusAdded.Add(atcDataManager.AddMenuIfMissing(lCategoryMenuName, atcDataManager.ComputeMenuName, lDataSource.Category, , , True))
                                            'Operations might have categories to further divide them
                                            If lOperation.Definition.Category.Length > 0 Then
                                                Dim lSubCategoryName As String = lCategoryMenuName & "_" & lOperation.Definition.Category
                                                atcDataManager.AddMenuIfMissing(lSubCategoryName, lCategoryMenuName, lOperation.Definition.Category, , , True)
                                                atcDataManager.AddMenuIfMissing(lSubCategoryName & "_" & lOperation.Definition.Name & "_" & Name, lSubCategoryName, lOperation.Definition.Name, , , True)
                                            Else
                                                atcDataManager.AddMenuIfMissing(lCategoryMenuName & "_" & lOperation.Definition.Name & "_" & Name, lCategoryMenuName, lOperation.Definition.Name, , , True)
                                            End If
                                    End Select
                                Next
                            Else
                                atcDataManager.AddMenuIfMissing(atcDataManager.ComputeMenuName, "", atcDataManager.ComputeMenuString, atcDataManager.FileMenuName)
                                pMenusAdded.Add(atcDataManager.AddMenuIfMissing(lCategoryMenuName & "_" & Name, lCategoryMenuName, lDataSource.Description, , , True))
                            End If
                        End If
                    Catch
                        'Could not add to menu, probably wasn't an atcDataSource
                    End Try
            End Select
        End If
    End Sub

    ''' <summary>Fired when the plugin is unloaded
    ''' (by the plugin dialog, the plugins menu, or on program exit).
    ''' </summary>
    ''' <remarks>
    ''' Remove any buttons from the tool bar or menu items from the menu that this plugin
    ''' added.<br />
    ''' If you don't do this, then you will leave dangling menus and buttons that don't do
    ''' anything.
    ''' </remarks>
    Public Overridable Sub Terminate() Implements MapWindow.Interfaces.IPlugin.Terminate
        If Not pMapWin Is Nothing Then
            'pMapWin.Plugins.BroadcastMessage("atcDataPlugin unloading " & Name)

            For Each lMenu As MapWindow.Interfaces.MenuItem In pMenusAdded
                If Not lMenu Is Nothing AndAlso Not pMapWin.Menus.Item(lMenu.Name) Is Nothing Then
                    pMapWin.Menus.Remove(lMenu.Name)
                End If
            Next
            pMenusAdded.Clear()

            If Name.StartsWith("Analysis::") Then
                atcDataManager.RemoveMenuIfEmpty(atcDataManager.AnalysisMenuName)
            ElseIf Name.StartsWith("Timeseries::") Then
                atcDataManager.RemoveMenuIfEmpty(atcDataManager.ComputeMenuName)
            End If
        End If
    End Sub

    ''' <summary>Fires when a menu item or toolbar button is clicked.
    ''' If this plugin added a button or menu on the Initialize event, 
    ''' this is where those events are handled.
    ''' </summary>
    Public Overridable Sub ItemClicked(ByVal aItemName As String, ByRef aHandled As Boolean) _
                                          Implements MapWindow.Interfaces.IPlugin.ItemClicked
        'Default handling for all atcDataPlugin derivatives
        Select Case aItemName
            Case atcDataManager.NewDataMenuName
                atcDataManager.UserOpenDataFile(False, True)
                aHandled = True
            Case atcDataManager.OpenDataMenuName
                atcDataManager.UserOpenDataFile()
                aHandled = True
            Case atcDataManager.ManageDataMenuName
                atcDataManager.UserManage()
                aHandled = True
            Case Else
                Dim lName As String = Name
                If aItemName.StartsWith(atcDataManager.SaveDataMenuName & "_") Then
                    aHandled = True
                    atcDataManager.UserSaveData(aItemName.Substring(atcDataManager.SaveDataMenuName.Length + 1))
                ElseIf aItemName.Equals(atcDataManager.AnalysisMenuName & "_" & lName) Then
                    aHandled = True
                    Dim lNewObject As atcDataTool = Me.NewOne
                    lNewObject.Initialize(pMapWin, pMapWinWindowHandle)
                    lNewObject.Show()
                ElseIf aItemName.StartsWith(atcDataManager.ComputeMenuName & "_") AndAlso aItemName.EndsWith(lName) Then
                    aHandled = True
                    Try
                        Dim ds As atcDataSource = Me
                        Dim lItemName As String = aItemName '.Replace(" ", "")
                        lItemName = lItemName.Substring(atcDataManager.ComputeMenuName.Length + 1, lItemName.Length - atcDataManager.ComputeMenuName.Length - lName.Length - 2)
                        If lItemName.StartsWith(ds.Category & "_") Then
                            Dim lNewSource As atcDataSource = Nothing
                            lItemName = lItemName.Substring(ds.Category.Length + 1)
                            Dim lOperation As atcDefinedValue = ds.AvailableOperations.ItemByKey(lItemName.ToLower)
                            Dim lUnderscorePos As Integer = lItemName.IndexOf("_"c)
                            While lUnderscorePos >= 0 AndAlso lOperation Is Nothing
                                lOperation = ds.AvailableOperations.ItemByKey(lItemName.Substring(lUnderscorePos + 1).ToLower)
                                lUnderscorePos = lItemName.IndexOf("_"c, lUnderscorePos + 1)
                            End While

                            If Not lOperation Is Nothing Then
                                lNewSource = ds.NewOne
                                lNewSource.Specification = lOperation.Definition.Name
                            ElseIf lItemName.Equals(ds.Description) Then
                                lNewSource = ds.NewOne
                            End If
                            If Not lNewSource Is Nothing Then
                                If atcDataManager.OpenDataSource(lNewSource, lNewSource.Specification, Nothing) Then
                                    If lNewSource.DataSets.Count > 0 Then
                                        Dim lTitle As String = lNewSource.ToString
                                        atcDataManager.UserSelectDisplay(lTitle, lNewSource.DataSets)
                                    End If
                                End If
                            End If
                        End If
                    Catch
                    End Try
                End If
        End Select
    End Sub

    ''' <summary>Fires when the user removes a layer from MapWindow.<br />
    ''' Useful if this plug-in depends on a particular layer being present, 
    ''' or if it keeps an internal list of layers.
    ''' </summary>
    Public Overridable Sub LayerRemoved(ByVal aHandle As Integer) _
                              Implements MapWindow.Interfaces.IPlugin.LayerRemoved
    End Sub

    ''' <summary>Fires when the user adds a layer to MapWindow. 
    ''' Useful in the same cases as <see cref="LayerRemoved">LayerRemoved</see>
    ''' </summary>
    <CLSCompliant(False)> _
    Public Overridable Sub LayersAdded(ByVal aLayers() As MapWindow.Interfaces.Layer) _
                              Implements MapWindow.Interfaces.IPlugin.LayersAdded
    End Sub

    ''' <summary>Fires when the user clears all of the layers from MapWindow.</summary>
    ''' <remarks>Useful in the same cases as <see 
    ''' cref="LayerRemoved">LayerRemoved</see></remarks>
    Public Overridable Sub LayersCleared() _
                              Implements MapWindow.Interfaces.IPlugin.LayersCleared
    End Sub

    ''' <summary>Fires when a user selects a layer in the legend.</summary>
    Public Overridable Sub LayerSelected(ByVal aHandle As Integer) _
                              Implements MapWindow.Interfaces.IPlugin.LayerSelected
    End Sub

    ''' <summary>Fires when a user double-clicks a layer in the legend.</summary>
    <CLSCompliant(False)> _
    Public Overridable Sub LegendDoubleClick(ByVal aHandle As Integer, _
                                             ByVal aLocation As MapWindow.Interfaces.ClickLocation, _
                                             ByRef aHandled As Boolean) _
                              Implements MapWindow.Interfaces.IPlugin.LegendDoubleClick
    End Sub

    ''' <summary>Fires when a user holds a mouse button down in the legend.</summary>
    <CLSCompliant(False)> _
    Public Overridable Sub LegendMouseDown(ByVal aHandle As Integer, _
                                           ByVal aButton As Integer, _
                                           ByVal aLocation As MapWindow.Interfaces.ClickLocation, ByRef Handled As Boolean) _
                              Implements MapWindow.Interfaces.IPlugin.LegendMouseDown
    End Sub

    ''' <summary>Fires when a user releases a mouse button in the legend.</summary>
    <CLSCompliant(False)> _
    Public Overridable Sub LegendMouseUp(ByVal aHandle As Integer, _
                                         ByVal aButton As Integer, _
                                         ByVal aLocation As MapWindow.Interfaces.ClickLocation, ByRef Handled As Boolean) _
                              Implements MapWindow.Interfaces.IPlugin.LegendMouseUp
    End Sub

    ''' <summary>Fires after a user draws a box with the mouse on the map</summary>
    ''' <remarks>Bounds specifies the box that was "drawn"</remarks>
    Public Overridable Sub MapDragFinished(ByVal aBounds As System.Drawing.Rectangle, _
                                           ByRef Handled As Boolean) _
                              Implements MapWindow.Interfaces.IPlugin.MapDragFinished
    End Sub

    ''' <summary>Fires any time there is a zoom or pan that changes the map extents.</summary>
    Public Overridable Sub MapExtentsChanged() _
                              Implements MapWindow.Interfaces.IPlugin.MapExtentsChanged
    End Sub

    ''' <summary>Fires when the user presses a mouse button on the map.
    ''' The x and y are screen coordinates in pixels.
    ''' </summary>
    ''' <remarks>
    ''' If map coordinates are needed, use g_MapWin.View.PixelToProj() where g_MapWin is
    ''' a saved reference to MapWin from Initialize
    ''' </remarks>
    Public Overridable Sub MapMouseDown(ByVal aButton As Integer, _
                                ByVal aShift As Integer, _
                                ByVal aScreenX As Integer, _
                                ByVal aScreenY As Integer, _
                                ByRef Handled As Boolean) _
                      Implements MapWindow.Interfaces.IPlugin.MapMouseDown
    End Sub

    ''' <summary>Fires when the user moves the mouse over the map.
    ''' See <see cref="MapMouseDown">MapMouseDown</see>.
    ''' </summary>
    Public Overridable Sub MapMouseMove(ByVal aScreenX As Integer, _
                                        ByVal aScreenY As Integer, _
                                        ByRef aHandled As Boolean) _
                      Implements MapWindow.Interfaces.IPlugin.MapMouseMove
    End Sub

    ''' <summary>Fires when the user releases a mouse button over the map.
    ''' See <see cref="MapMouseDown">MapMouseDown</see>.
    ''' </summary>
    Public Overridable Sub MapMouseUp(ByVal aButton As Integer, _
                                      ByVal aShift As Integer, _
                                      ByVal aScreenX As Integer, _
                                      ByVal aScreenY As Integer, _
                                      ByRef aHandled As Boolean) _
                      Implements MapWindow.Interfaces.IPlugin.MapMouseUp
    End Sub

    ''' <summary>Plugins can communicate with each other using Messages.
    ''' If a message is sent then this event fires.
    ''' </summary>
    ''' <remarks>Set Handled=True to stop the message from being sent to any more 
    ''' plugins.</remarks>
    Public Overridable Sub Message(ByVal aMsg As String, _
                                   ByRef aHandled As Boolean) _
                      Implements MapWindow.Interfaces.IPlugin.Message
    End Sub

    ''' <summary>Fires when a project is opened in MapWindow.</summary>
    ''' <param name="aProjectFile">file name including path of the project that was 
    ''' opened.</param>
    ''' <param name="aSettingsString">
    ''' contains the string of data that is connected to this plugin and was stored in
    ''' the project in a ProjectSaving event.
    ''' </param>
    Public Overridable Sub ProjectLoading(ByVal aProjectFile As String, _
                                ByVal aSettingsString As String) _
                           Implements MapWindow.Interfaces.IPlugin.ProjectLoading
    End Sub

    ''' <summary>Fires when a project is saved in MapWindow.</summary>
    ''' <param name="aProjectFile">file name including path of the project that is being 
    ''' saved.</param>
    ''' <param name="aSettingsString">
    ''' be set to any string of data that is connected to this plugin<br />
    ''' which should be stored in the project file.
    ''' </param>
    Public Overridable Sub ProjectSaving(ByVal aProjectFile As String, _
                                         ByRef aSettingsString As String) _
                           Implements MapWindow.Interfaces.IPlugin.ProjectSaving
    End Sub

    ''' <summary>Fires when the user selects one or more shapes
    ''' using the select tool in MapWindow.</summary>
    ''' <param name="aHandle">Layer handle for the shapefile on which shapes were 
    ''' selected.</param>
    ''' <param name="aSelectInfo">holds information about the shapes that were 
    ''' selected.</param>
    <CLSCompliant(False)> _
    Public Overridable Sub ShapesSelected(ByVal aHandle As Integer, _
                                          ByVal aSelectInfo As MapWindow.Interfaces.SelectInfo) _
                        Implements MapWindow.Interfaces.IPlugin.ShapesSelected
    End Sub

    ''' <summary>Deprecated - do NOT use</summary>
    ''' <remarks>Leftover part of IPlugin interface no longer in use</remarks>
    Public Overridable ReadOnly Property SerialNumber() As String Implements MapWindow.Interfaces.IPlugin.SerialNumber
        Get
            Return pSerial.ToString
        End Get
    End Property
#End If

End Class
