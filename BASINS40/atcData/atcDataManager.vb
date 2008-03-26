Imports atcUtility
Imports MapWinUtility
Imports System.Reflection

''' <summary>Manages a set of currently open DataSources. 
'''          Uses the set of plugins currently loaded to find ones that inherit atcDataSource
''' </summary>
Public Class atcDataManager
    Private Shared pMapWin As MapWindow.Interfaces.IMapWin
    Private Shared pDataSources As ArrayList 'of atcDataSource, the currently open data sources
    Private Shared pSelectionAttributes As ArrayList
    Private Shared pDisplayAttributes As ArrayList

    Private Const pInMemorySpecification As String = "<in memory>"
    Private Shared pLikelyShapeLocationFieldNames() As String = { _
        "ACC_ID", "COOP_ID", "COVNAME", "CU", "ECOREG_ID", "EPA_REG_ID", _
        "FIPS", "GAGE_ID", "ID", "LOCATION", "MUID", "NAWQA", "NPD", "NPDES", _
        "NSI_STAT", "RECID", "RIVRCH", "SITE_CODE", "ST", "UA_CODE", "URBAN_ID"}

    ''' <summary>Event raised when a data source is opened</summary>
    Shared Event OpenedData(ByVal aDataSource As atcDataSource)

    ''' <summary>Create a new instance of atcDataManager</summary>
    Private Sub New()
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value>Pointer to the root interface for the MapWindow</value>
    ''' <remarks></remarks>
    <CLSCompliant(False)> _
    Public Shared WriteOnly Property MapWindow() As MapWindow.Interfaces.IMapWin
        Set(ByVal aMapWin As MapWindow.Interfaces.IMapWin)
            If pMapWin Is Nothing AndAlso Not aMapWin Is Nothing Then
                pMapWin = aMapWin
                Clear()
            End If
        End Set
    End Property

    ''' <summary>Sets data manager to its initial state.
    '''          Defaults Datasources, Selection Attributes and Display Attributes.
    ''' </summary>
    Public Shared Sub Clear()
        pDataSources = New ArrayList
        Dim lMemory As New atcDataSource
        lMemory.Specification = pInMemorySpecification
        pDataSources.Add(lMemory)

        pSelectionAttributes = New ArrayList
        pSelectionAttributes.Add("Scenario")
        pSelectionAttributes.Add("Location")
        pSelectionAttributes.Add("Constituent")

        pDisplayAttributes = New ArrayList
        pDisplayAttributes.Add("History 1")
        pDisplayAttributes.Add("Constituent")
        pDisplayAttributes.Add("Id")
        pDisplayAttributes.Add("Min")
        pDisplayAttributes.Add("Max")
        pDisplayAttributes.Add("Mean")
    End Sub

    ''' <summary>Set of atcDataSource objects representing currently open DataSources</summary>
    Public Shared ReadOnly Property DataSources() As ArrayList
        Get
            Return pDataSources
        End Get
    End Property

    ''' <summary>Set of atcDataSets found in currently open DataSources</summary>
    Public Shared Function DataSets() As atcDataGroup
        Dim lAllData As New atcDataGroup
        For Each lSource As atcDataSource In DataSources
            For Each lTs As atcDataSet In lSource.DataSets
                lAllData.Add(lTs)
            Next
        Next
        Return lAllData
    End Function

    ''' <summary>Names of attributes used for selection of data in UI</summary>
    Public Shared ReadOnly Property SelectionAttributes() As ArrayList
        Get
            Return pSelectionAttributes
        End Get
    End Property

    ''' <summary>Names of attributes used for listing of data in UI</summary>
    Public Shared ReadOnly Property DisplayAttributes() As ArrayList
        Get
            Return pDisplayAttributes
        End Get
    End Property

    ''' <summary>Currently loaded plugins that inherit the specified class; returns empty objects</summary>
    ''' <param name="aBaseType">
    '''     <para>Type of plugin to match and return</para>
    ''' </param>  
    Public Shared Function GetPlugins(ByVal aBaseType As Type) As atcCollection
        Dim lMatchingPlugIns As New atcCollection
        If Not pMapWin Is Nothing Then
            Dim lLastPlugIn As Integer = pMapWin.Plugins.Count() - 1
            For iPlugin As Integer = 0 To lLastPlugIn
                Dim lCurPlugin As MapWindow.Interfaces.IPlugin = pMapWin.Plugins.Item(iPlugin)
                If Not lCurPlugin Is Nothing Then
                    If CType(lCurPlugin, Object).GetType().IsSubclassOf(aBaseType) Then
                        lMatchingPlugIns.Add(lCurPlugin.Name, lCurPlugin)
                    End If
                End If
            Next
        End If
        Return lMatchingPlugIns
    End Function

    ''' <summary>
    ''' Activate the named plugin
    ''' </summary>
    ''' <param name="aPluginName">Name of plugin to activate</param>
    ''' <remarks></remarks>
    Public Shared Sub LoadPlugin(ByVal aPluginName As String)
        Try
            Dim lKey As String = pMapWin.Plugins.GetPluginKey(aPluginName)
            'If Not g_MapWin.Plugins.PluginIsLoaded(lKey) Then 
            pMapWin.Plugins.StartPlugin(lKey)
        Catch e As Exception
            Logger.Dbg("Exception loading " & aPluginName & ": " & e.Message)
        End Try
    End Sub

    ''' <summary>Open BASINS data source</summary>
    ''' <param name="aNewSource">
    '''     <para>Instance of data source that can open the specified data</para>
    ''' </param>  
    ''' <param name="aSpecification">
    '''     <para>File name, connection string, or other information needed to initialize aNewSource</para>
    ''' </param>
    ''' <param name="aAttributes">
    '''     <para>Attributes associated with specification, may be NOTHING</para>
    ''' </param>
    ''' <returns>Boolean - True if source opened, False otherwise</returns>
    Public Shared Function OpenDataSource(ByVal aNewSource As atcDataSource, _
                                   ByVal aSpecification As String, _
                                   ByVal aAttributes As atcDataAttributes) As Boolean
        Try
            If aNewSource.Open(aSpecification, aAttributes) Then
                Logger.Dbg("DataSetCount:" & aNewSource.DataSets.Count & ":Specification:" & aNewSource.Specification)
                pDataSources.Add(aNewSource)
                RaiseEvent OpenedData(aNewSource)
                If Not pMapWin Is Nothing Then
                    pMapWin.Project.Modified = True
                    If aNewSource.CanSave Then
                        AddMenuIfMissing(SaveDataMenuName, FileMenuName, SaveDataMenuString, "mnuSaveAs")
                        AddMenuIfMissing(SaveDataMenuName & "_" & aNewSource.Specification, SaveDataMenuName, aNewSource.Specification)
                    End If
                End If
                Return True
            Else
                If Logger.LastDbgText.Length > 0 Then
                    Logger.Dbg("OpenFailure:Specification:'" & aNewSource.Specification & "'" & vbCrLf & _
                               "Source Name:" & aNewSource.Name & vbCrLf & _
                               "Details:" & Logger.LastDbgText)
                End If
                Return False
            End If
        Catch ex As Exception
            Logger.Dbg("Exception:" & ex.Message & vbCrLf & _
                       "Traceback:" & ex.StackTrace & vbCrLf & _
                       "Specification:'" & aSpecification & "'" & vbCrLf & _
                       "SpecificationNew:" & aNewSource.Specification & vbCrLf & _
                       "Source Name:" & aNewSource.Name)
            Return False
        End Try
    End Function

    ''' <summary>Creates and returns an instance of a data source by name</summary>
    ''' <param name="aDataSourceName">
    '''     <para>Name of data source to create and return</para>
    ''' </param>  
    Public Shared Function DataSourceByName(ByVal aDataSourceName As String) As atcDataSource
        aDataSourceName = aDataSourceName.Replace("Timeseries::", "")
        For Each lDataSource As atcDataSource In GetPlugins(GetType(atcDataSource))
            If lDataSource.Name.Replace("Timeseries::", "") = aDataSourceName Then
                Return lDataSource.NewOne
            End If
        Next
        Return Nothing
    End Function

#Region "User interaction"

    Public Shared Sub UserSelectDisplay(ByVal aTitle As String, ByVal aDataGroup As atcDataGroup)
        Dim lSelectDisplay As New frmSelectDisplay
        If Not aTitle Is Nothing AndAlso aTitle.Length > 0 Then lSelectDisplay.Text = aTitle
        lSelectDisplay.AskUser(aDataGroup)
    End Sub

    Public Shared Sub ShowDisplay(ByVal aDisplayName As String, ByVal aDataGroup As atcDataGroup)
        If aDisplayName Is Nothing OrElse aDisplayName.Length = 0 Then
            UserSelectDisplay("", aDataGroup)
        Else
            Dim lNewDisplay As atcDataDisplay
            For Each lDisp As atcDataDisplay In GetPlugins(GetType(atcDataDisplay))
                If lDisp.Name = aDisplayName OrElse lDisp.Name.EndsWith("::" & aDisplayName) Then
                    Dim lType As System.Type = lDisp.GetType()
                    Dim lAssembly As System.Reflection.Assembly = System.Reflection.Assembly.GetAssembly(lType)
                    lNewDisplay = lAssembly.CreateInstance(lType.FullName)
                    lNewDisplay.Initialize(pMapWin, Nothing) 'TODO: do we need the aParentHandle here?
                    lNewDisplay.Show(aDataGroup)
                    Exit Sub
                End If
            Next
        End If
    End Sub

    ''' <summary>Ask user to select a data source</summary>
    ''' <param name="aCategories">
    '''     <para>Filter to limit user choices</para>
    ''' </param>  
    ''' <param name="aTitle">
    '''     <para>Title of window</para>
    ''' </param>  
    ''' <param name="aNeedToOpen">
    '''     <para>True to only include data sources that can open</para>
    ''' </param>  
    ''' <param name="aNeedToSave">
    '''     <para>True to only include data sources that can save</para>
    ''' </param>  
    Public Shared Function UserSelectDataSource(Optional ByVal aCategories As ArrayList = Nothing, _
                                         Optional ByVal aTitle As String = "Select a Data Source", _
                                         Optional ByVal aNeedToOpen As Boolean = True, _
                                         Optional ByVal aNeedToSave As Boolean = False) As atcDataSource
        Dim lForm As New frmDataSource
        Dim lSelectedDataSource As atcDataSource = Nothing
        lForm.Text = aTitle
        lForm.AskUser(lSelectedDataSource, aNeedToOpen, aNeedToSave, aCategories)
        Return lSelectedDataSource
    End Function

    ''' <summary>Ask user to select data</summary>
    ''' <param name="aTitle">
    '''     <para>Optional title for dialog window, default is 'Select Data'</para>
    ''' </param>  
    ''' <param name="aGroup">
    '''     <para>Optional pre-selected group of data, default is no data already selected</para>
    ''' </param>  
    ''' <param name="aModal">
    '''     <para>Optional modality specification for window, default is True</para>
    ''' </param>  
    Public Shared Function UserSelectData(Optional ByVal aTitle As String = "", Optional ByVal aGroup As atcDataGroup = Nothing, Optional ByVal aModal As Boolean = True) As atcDataGroup
        Dim lForm As New frmSelectData
        If aTitle.Length > 0 Then lForm.Text = aTitle
        Dim lAutoSelected As Boolean = False

        'Try automatically selecting data based on what is selected on the map
        If aGroup Is Nothing OrElse aGroup.Count = 0 Then
            If aGroup Is Nothing Then
                aGroup = DatasetsAtMapSelectedLocations()
            Else
                aGroup.AddRange(DatasetsAtMapSelectedLocations)
            End If
            If aGroup.Count > 0 Then lAutoSelected = True
        End If
        aGroup = lForm.AskUser(aGroup, aModal)
        If lAutoSelected AndAlso Not lForm.SelectedOk AndAlso Not aGroup Is Nothing Then
            aGroup.Clear() 'We got back our location-selected data but the user didn't click Ok
        End If
        Return aGroup
    End Function

    ''' <summary>Ask user to manage data sources</summary>
    ''' <param name="aTitle">
    '''     <para>Optional title for dialog window, default is 'Data Sources'</para>
    ''' </param> 
    Public Shared Sub UserManage(Optional ByVal aTitle As String = "")
        Dim lForm As New frmManager
        If aTitle.Length > 0 Then lForm.Text = aTitle
        lForm.Edit()
    End Sub

    Friend Shared Function UserOpenDataFile(Optional ByVal aNeedToOpen As Boolean = True, _
                                     Optional ByVal aNeedToSave As Boolean = False) As atcDataSource
        Dim lFilesOnly As New ArrayList(1)
        lFilesOnly.Add("File")
        Dim lNewSource As atcDataSource = atcDataManager.UserSelectDataSource(lFilesOnly, "Select a File Type", aNeedToOpen, aNeedToSave)
        If Not lNewSource Is Nothing Then 'user did not cancel
            If Not atcDataManager.OpenDataSource(lNewSource, lNewSource.Specification, Nothing) Then
                If Logger.LastDbgText.Length > 0 Then
                    Logger.Msg(Logger.LastDbgText, "Data Open Problem")
                End If
            End If
        End If
        Return lNewSource
    End Function

    Friend Shared Function UserSaveData(ByVal aSpecification As String) As Boolean
        Dim lSaveIn As atcDataSource = Nothing
        Dim lSaveGroup As atcDataGroup = atcDataManager.UserSelectData("Select Data to Save")
        If Not lSaveGroup Is Nothing AndAlso lSaveGroup.Count > 0 Then
            For Each lDataSource As atcDataSource In atcDataManager.DataSources
                If lDataSource.Specification = aSpecification Then
                    lSaveIn = lDataSource
                    Exit For
                End If
            Next

            If lSaveIn Is Nothing Then
                lSaveIn = UserOpenDataFile(False, True)
            End If

            If Not lSaveIn Is Nothing And lSaveIn.Specification.Length > 0 Then
                For Each lDataSet As atcDataSet In lSaveGroup
                    lSaveIn.AddDataSet(lDataSet, atcData.atcDataSource.EnumExistAction.ExistRenumber)
                Next
                Return lSaveIn.Save(lSaveIn.Specification)
            End If
        End If
        Return False
    End Function
#End Region

#Region "MapWindow location selection"
    ''' <summary>
    ''' Return the currently loaded datasets whose Location attribute matches a currently selected shape on the map
    ''' </summary>
    Private Shared Function DatasetsAtMapSelectedLocations() As atcDataGroup
        Dim lGroup As New atcDataGroup
        Dim lLocations As ArrayList = MapSelectedLocations()
        If lLocations.Count > 0 Then
            For Each lDataSet As atcDataSet In DataSets()
                If lLocations.Contains(lDataSet.Attributes.GetValue("Location", "")) Then
                    lGroup.Add(lDataSet)
                End If
            Next
        End If
        Return lGroup
    End Function

    ''' <summary>
    ''' Return the one-based location field index in the layer DBF
    ''' </summary>
    ''' <param name="aLayerDBF">Table to find a location field in. Contains one row per shape.</param>
    ''' <remarks>
    ''' Uses file layers.dbf to determine which field in the layer's dbf contains locations.
    ''' Base layer file name is in first column, location field index is in fourth column.
    ''' If layers.dbf is not found or layer is not found in it, scans aLayerDBF for a field 
    ''' named in pLikelyShapeLocationFieldNames.
    ''' </remarks>
    Private Shared Function MapLayerLocationField(ByVal aLayerDBF As atcTableDBF) As Integer
        Dim lLocationField As Integer = 0 'One-based field index. Zero indicates we have not found the index yet.

        Dim lLayerListFilename As String = FindFile("Please locate layers.dbf", "\BASINS\etc\DataDownload\layers.dbf") 'table containing location field for BASINS layers
        If FileExists(lLayerListFilename) Then
            Try
                Dim lLayersDBF As New atcTableDBF
                If lLayersDBF.OpenFile(lLayerListFilename) Then
                    If lLayersDBF.FindFirst(1, IO.Path.GetFileNameWithoutExtension(aLayerDBF.FileName)) Then
                        lLocationField = CInt(lLayersDBF.Value(4))
                    End If
                End If
                lLayersDBF.Clear()
            Catch
            End Try
        End If

        If lLocationField = 0 Then 'search for a likely location field name
            For Each lLocationFieldName As String In pLikelyShapeLocationFieldNames
                lLocationField = aLayerDBF.FieldNumber(lLocationFieldName)
                If lLocationField > 0 Then Exit For
            Next
        End If
        Return lLocationField
    End Function

    ''' <summary>
    ''' Returns list of locations (as string) of currently selected shapes on the map
    ''' Returns empty list if there is no dbf for the current layer or if location field cannot be determined
    ''' </summary>
    Private Shared Function MapSelectedLocations() As ArrayList
        Dim lLocations As New ArrayList
        If Not pMapWin Is Nothing _
           AndAlso Not pMapWin.View Is Nothing _
           AndAlso pMapWin.View.SelectedShapes.NumSelected > 0 _
           AndAlso pMapWin.Layers.IsValidHandle(pMapWin.Layers.CurrentLayer) Then
            Dim lCurrentLayerDBFname As String = FilenameSetExt(pMapWin.Layers(pMapWin.Layers.CurrentLayer).FileName.ToLower, "dbf")
            If IO.File.Exists(lCurrentLayerDBFname) Then
                Dim lDBF As New atcTableDBF 'Table associated with current layer on map
                If lDBF.OpenFile(lCurrentLayerDBFname) Then
                    Dim lLocationField As Integer = MapLayerLocationField(lDBF) 'One-based field index. Zero indicates we have not found the index yet.
                    If lLocationField > 0 Then
                        Dim lLocation As String
                        For lSelectionIndex As Integer = pMapWin.View.SelectedShapes.NumSelected - 1 To 0 Step -1
                            lDBF.CurrentRecord = pMapWin.View.SelectedShapes.Item(lSelectionIndex).ShapeIndex + 1 ' +1 because ShapeIndex is zero based
                            lLocation = lDBF.Value(lLocationField)
                            If Not lLocations.Contains(lLocation) Then
                                lLocations.Add(lDBF.Value(lLocationField))
                            End If
                        Next
                    End If
                End If
            End If
        End If
        Return lLocations
    End Function

    Friend Shared Sub SelectLocationsOnMap(ByVal aLocations As ArrayList, ByVal aUnselectOthers As Boolean)
        If Not pMapWin Is Nothing AndAlso Not pMapWin.Layers Is Nothing AndAlso pMapWin.Layers.IsValidHandle(pMapWin.Layers.CurrentLayer) Then
            Dim lCurrentLayerDBFname As String = FilenameSetExt(pMapWin.Layers(pMapWin.Layers.CurrentLayer).FileName.ToLower, "dbf")
            If IO.File.Exists(lCurrentLayerDBFname) Then
                Dim lDBF As New atcTableDBF 'Table associated with current layer on map
                If lDBF.OpenFile(lCurrentLayerDBFname) Then
                    Dim lLocationField As Integer = MapLayerLocationField(lDBF)
                    If lLocationField > 0 Then 'One-based field index. Zero indicates we have not found the field index.
                        If aUnselectOthers Then pMapWin.View.SelectedShapes.ClearSelectedShapes()
                        For Each lLocation As String In aLocations
                            If lDBF.FindFirst(lLocationField, lLocation) Then
                                Do
                                    pMapWin.View.SelectedShapes.AddByIndex(lDBF.CurrentRecord - 1, Drawing.Color.Yellow) ' -1 because AddByIndex is zero based
                                Loop While lDBF.FindNext(lLocationField, lLocation)
                            End If
                        Next
                        pMapWin.View.Redraw()
                    End If
                End If
            End If
        End If
    End Sub
#End Region

    ''' <summary>State of data manager in XML format</summary>
    ''' <value>Chilkat.Xml</value>
    ''' <requirements>
    ''' Chilkat Xml from
    ''' <a href="http://www.xml-parser.com/downloads.htm">http://www.xml-parser.com/downloads.htm</a>
    ''' </requirements>
    <CLSCompliant(False)> _
     Public Shared Property XML() As Chilkat.Xml
        Get
            Dim lSaveXML As New Chilkat.Xml
            Dim lChildXML As Chilkat.Xml
            lSaveXML.Tag = "DataManager"
            For Each lName As String In pSelectionAttributes
                lSaveXML.NewChild("SelectionAttribute", lName)
            Next
            For Each lName As String In pDisplayAttributes
                lSaveXML.NewChild("DisplayAttribute", lName)
            Next
            For Each lSource As atcDataSource In pDataSources
                'If lSource.CanSave Then 'TODO: better test to pass only types that just need a Specification string to open
                'If Not lSource.Specification.Equals(pInMemorySpecification) Then
                If lSource.Category = "File" Then
                    lChildXML = lSaveXML.NewChild("DataSource", lSource.Name)
                    lChildXML.AddAttribute("Specification", lSource.Specification)
                End If
                'End If
            Next
            Return lSaveXML
        End Get
        Set(ByVal newValue As Chilkat.Xml)
            Clear()
            If Not newValue Is Nothing Then
                Dim clearedSelectionAttributes As Boolean = False
                Dim clearedDisplayAttributes As Boolean = False
                Dim lchildXML As Chilkat.Xml
                lchildXML = newValue.FirstChild

                While Not lchildXML Is Nothing
                    Select Case lchildXML.Tag
                        Case "DataFile", "DataSource"
                            Dim lDataSourceType As String = lchildXML.Content
                            Dim lSpecification As String = lchildXML.GetAttrValue("Specification")
                            If lSpecification.Equals(pInMemorySpecification) Then
                                'Ignore, we do not save this but we used to
                            ElseIf lDataSourceType Is Nothing OrElse lDataSourceType.Length = 0 Then
                                Logger.Msg("No data source type found for '" & lSpecification & "'", "Data type not specified")
                            ElseIf lDataSourceType = "WDM" AndAlso Not FileExists(lSpecification) Then
                                Logger.Dbg("Skipping file that does not exist: '" & lSpecification & "'")
                            Else
                                Dim lNewDataSource As atcDataSource = DataSourceByName(lchildXML.Content)
                                If lNewDataSource Is Nothing Then
                                    Logger.Msg("Unable to open data source of type '" & lDataSourceType & "'", "Data type not found")
                                ElseIf lNewDataSource.Category = "File" Then
                                    OpenDataSource(lNewDataSource, lSpecification, Nothing)
                                Else
                                    Logger.Dbg("Skipping opening data source that is not a File: " & lSpecification)
                                End If
                            End If
                        Case "SelectionAttribute"
                            If Not clearedSelectionAttributes Then
                                clearedSelectionAttributes = True
                                pSelectionAttributes.Clear()
                            End If
                            pSelectionAttributes.Add(lchildXML.Content)
                        Case "DisplayAttribute"
                            If Not clearedDisplayAttributes Then
                                pDisplayAttributes.Clear()
                                clearedDisplayAttributes = True
                            End If
                            pDisplayAttributes.Add(lchildXML.Content)
                    End Select
                    If Not lchildXML.NextSibling2 Then lchildXML = Nothing
                End While
            End If
        End Set
    End Property

#Region "MapWindow menu handling"
    Public Const FileMenuName As String = "mnuFile"

    Public Const NewDataMenuName As String = "BasinsNewData"
    Public Const NewDataMenuString As String = "New Data"

    Public Const OpenDataMenuName As String = "BasinsOpenData"
    Public Const OpenDataMenuString As String = "Open Data"

    Public Const ManageDataMenuName As String = "BasinsManageData"
    Public Const ManageDataMenuString As String = "Manage Data"

    Public Const SaveDataMenuName As String = "BasinsSaveData"
    Public Const SaveDataMenuString As String = "Save Data In..."

    Public Const ComputeMenuName As String = "BasinsCompute"
    Public Const ComputeMenuString As String = "Compute"

    Public Const AnalysisMenuName As String = "BasinsAnalysis"
    Public Const AnalysisMenuString As String = "Analysis"

    Public Const LaunchMenuName As String = "BasinsLaunch"
    Public Const LaunchMenuString As String = "Launch"

    Public Const ModelsMenuName As String = "BasinsModels"
    Public Const ModelsMenuString As String = "Models"

    Public Shared Sub RemoveMenuIfEmpty(ByVal aMenuName As String)
        If Not pMapWin Is Nothing Then
            With pMapWin.Menus
                Dim lMenu As MapWindow.Interfaces.MenuItem = .Item(aMenuName)
                If Not lMenu Is Nothing Then 'This menu exists
                    If lMenu.NumSubItems = 0 Then pMapWin.Menus.Remove(aMenuName)
                End If
            End With
        End If
    End Sub

    <CLSCompliant(False)> _
    Public Shared Function AddMenuIfMissing(ByVal aMenuName As String, _
                                    ByVal aParent As String, _
                                    ByVal aMenuText As String, _
                           Optional ByVal aAfter As String = "", _
                           Optional ByVal aBefore As String = "", _
                           Optional ByVal aAlphabetical As Boolean = False) _
                           As MapWindow.Interfaces.MenuItem
        If pMapWin Is Nothing Then
            Return Nothing
        Else
            With pMapWin.Menus
                'Dim lMenu As MapWindow.Interfaces.MenuItem = .Item(aMenuName)
                'If Not lMenu Is Nothing Then 'This item already exists
                '    Return lMenu
                If aAlphabetical And aParent.Length > 0 Then
                    'Need parent to do alphabetical search for position
                    Dim lParentMenu As MapWindow.Interfaces.MenuItem = .Item(aParent)
                    Dim lSubmenuIndex As Integer = 0
                    Dim lExistingMenu As MapWindow.Interfaces.MenuItem

                    If aAfter.Length > 0 Then
                        'First make sure we are after a particular item
                        While lSubmenuIndex < lParentMenu.NumSubItems
                            lExistingMenu = lParentMenu.SubItem(lSubmenuIndex)
                            If Not lExistingMenu Is Nothing AndAlso _
                               Not lExistingMenu.Name Is Nothing AndAlso _
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
                        If Not lExistingMenu Is Nothing AndAlso _
                           Not lExistingMenu.Name Is Nothing Then
                            If (aBefore.Length > 0 AndAlso lExistingMenu.Text = aBefore) OrElse _
                               lExistingMenu.Text > aMenuText Then
                                'Add before existing menu with alphabetically later text
                                Return .AddMenu(aMenuName, aParent, aMenuText, lExistingMenu.Name)
                            End If
                        End If
                        lSubmenuIndex += 1
                    End While
                    'Add at default position, after last parent subitem
                    Return .AddMenu(aMenuName, aParent, Nothing, aMenuText)
                ElseIf aBefore.Length > 0 Then
                    Return .AddMenu(aMenuName, aParent, aMenuText, aBefore)
                Else
                    Return .AddMenu(aMenuName, aParent, Nothing, aMenuText, aAfter)
                End If
            End With
        End If
    End Function
#End Region

    Public Overrides Function ToString() As String
        Dim lString As String = "atcDataManger:"
        For Each lDataSource As atcDataSource In pDataSources
            lString &= lDataSource.ToString & vbCrLf
        Next lDataSource
        Return lString.TrimEnd(New Char() {vbCr, vbLf})
    End Function
End Class
