Imports atcUtility
Imports MapWinUtility
Imports System.Reflection

''' <summary>Manages a set of currently open DataSources. 
'''          Uses the set of plugins currently loaded to find ones that inherit atcTimeseriesSource
''' </summary>
Public Class atcDataManager
    Private Shared pMapWin As MapWindow.Interfaces.IMapWin
    Private Shared pDataSources As ArrayList 'of atcTimeseriesSource, the currently open data sources
    Private Shared pSelectionAttributes As ArrayList
    Private Shared pDisplayAttributes As ArrayList
    Private Shared pManagerForm As frmManager

    Private Shared pDefaultSelectionAttributes() As String = {"Scenario", "Location", "Constituent"}
    Private Shared pDefaultDisplayAttributes() As String = {"History 1", "Constituent", "Id", "Min", "Max", "Mean"}

    Private Shared pLikelyShapeLocationFieldNames() As String = { _
        "ACC_ID", "COOP_ID", "COVNAME", "CU", "ECOREG_ID", "EPA_REG_ID", _
        "FIPS", "GAGE_ID", "ID", "LOCATION", "MUID", "NAWQA", "NPD", "NPDES", _
        "NSI_STAT", "RECID", "RIVRCH", "SITE_CODE", "ST", "UA_CODE", "URBAN_ID"}

    ''' <summary>Event raised when a data source is opened</summary>
    Shared Event OpenedData(ByVal aDataSource As atcTimeseriesSource)

    ''' <summary>Event raised when a data source is closed</summary>
    Shared Event ClosedData(ByVal aDataSource As atcTimeseriesSource)

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
        If pDataSources IsNot Nothing Then
            For Each lDataSource As atcTimeseriesSource In pDataSources
                lDataSource.Clear()
            Next
            pDataSources.Clear()
        End If
        pDataSources = New ArrayList
        pSelectionAttributes = New ArrayList(pDefaultSelectionAttributes)
        pDisplayAttributes = New ArrayList(pDefaultDisplayAttributes)
    End Sub

    ''' <summary>Set of atcTimeseriesSource objects representing currently open DataSources</summary>
    Public Shared ReadOnly Property DataSources() As ArrayList
        Get
            Return pDataSources
        End Get
    End Property

    ''' <summary>Set of atcDataSets found in currently open DataSources</summary>
    Public Shared Function DataSets() As atcTimeseriesGroup
        Dim lAllData As New atcTimeseriesGroup
        For Each lDataSource As atcTimeseriesSource In DataSources
            lAllData.AddRange(lDataSource.DataSets)
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
    Public Shared Function LoadPlugin(ByVal aPluginName As String) As Boolean
        Try
            Dim lKey As String = pMapWin.Plugins.GetPluginKey(aPluginName)
            'If Not g_MapWin.Plugins.PluginIsLoaded(lKey) Then 
            Return pMapWin.Plugins.StartPlugin(lKey)
        Catch e As Exception
            Logger.Dbg("Exception loading " & aPluginName & ": " & e.Message)
            Return False
        End Try
    End Function

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
    Public Shared Function OpenDataSource(ByVal aNewSource As atcTimeseriesSource, _
                                          ByVal aSpecification As String, _
                                          ByVal aAttributes As atcDataAttributes) As Boolean
        Try
            RemoveDataSource(aSpecification)
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

    Public Shared Function OpenDataSource(ByVal aSpecification As String) As Boolean
        RemoveDataSource(aSpecification)
        Dim lMatchDataSource As atcTimeseriesSource = Nothing
        Dim lPossibleDataSources As New atcCollection
        Dim lFileExtension As String = IO.Path.GetExtension(aSpecification)
        For Each lDataSource As atcTimeseriesSource In GetPlugins(GetType(atcTimeseriesSource))
            If lDataSource.Filter.Contains(lFileExtension) Then
                lPossibleDataSources.Add(lDataSource.Name, lDataSource)
            End If
        Next
        If lPossibleDataSources.Count = 1 Then
            lMatchDataSource = lPossibleDataSources(0)
            lMatchDataSource = lMatchDataSource.NewOne()
            Return OpenDataSource(lMatchDataSource, aSpecification, Nothing)
        ElseIf lPossibleDataSources.Count = 0 Then
            Logger.Msg("No Data Source Available for '" & FileExt(aSpecification) & "'", "Open Data Source Problem")
        Else
            Logger.Msg("Data Source Ambiguous for " & aSpecification, "Open Data Source Problem")
            'TODO: choose the source
        End If
        Return False
    End Function

    Public Shared Sub RemoveDataSource(ByVal aIndex As Integer)
        Dim lDataSource As atcTimeseriesSource = DataSources(aIndex)
        DataSources.RemoveAt(aIndex)
        RemovedDataSource(lDataSource)
    End Sub

    Public Shared Sub RemoveDataSource(ByVal aSpecification As String)
        Dim lDataSource As atcTimeseriesSource = DataSourceBySpecification(aSpecification)
        If lDataSource IsNot Nothing Then
            DataSources.Remove(lDataSource)
            RemovedDataSource(lDataSource)
        End If
    End Sub

    Public Shared Sub RemoveDataSource(ByVal aDataSource As atcDataSource)
        Try
            DataSources.Remove(aDataSource)
            RemovedDataSource(aDataSource)
        Catch e As Exception
            Logger.Dbg("Could not remove data source: " & e.Message)
            Exit Sub
        End Try
    End Sub

    Private Shared Sub RemovedDataSource(ByVal aDataSource As atcDataSource)
        Try
            RaiseEvent ClosedData(aDataSource)
            aDataSource.Clear() 'TODO: dispose and/or close to get rid of everything
        Catch e As Exception
            Logger.Dbg("RaiseEvent ClosedData: " & e.Message)
        End Try
    End Sub

    ''' <summary>Creates and returns an instance of a data source by name</summary>
    ''' <param name="aDataSourceName">
    '''     <para>Name of data source to create and return</para>
    ''' </param>  
    Public Shared Function DataSourceByName(ByVal aDataSourceName As String) As atcTimeseriesSource
        aDataSourceName = aDataSourceName.Replace("Timeseries::", "")
        For Each lDataSource As atcTimeseriesSource In GetPlugins(GetType(atcTimeseriesSource))
            If lDataSource.Name.Replace("Timeseries::", "") = aDataSourceName Then
                Return lDataSource.NewOne
            End If
        Next
        Return Nothing
    End Function

    ''' <summary>
    ''' Get a data source that is already open by its specification
    ''' </summary>
    ''' <param name="aSpecification">The file name for file-based data sources</param>
    ''' <returns>the data source if it is already open, Nothing if it is not open</returns>
    ''' <remarks>
    ''' Searching for a match is not case sensitive but it can be fooled if two different specifications refer to the same file
    ''' Always use consistent full path of files
    ''' Use OpenDataSource to open one that is not yet open
    ''' </remarks>
    Public Shared Function DataSourceBySpecification(ByVal aSpecification As String) As atcTimeseriesSource
        If DataSources IsNot Nothing Then
            aSpecification = aSpecification.ToLower
            For Each lDataSource As atcTimeseriesSource In DataSources
                If lDataSource.Specification.ToLower.Equals(aSpecification) Then
                    Return lDataSource
                End If
            Next
        End If
        Return Nothing
    End Function

#Region "User interaction"

    Public Shared Sub UserSelectDisplay(ByVal aTitle As String, ByVal aDataGroup As atcTimeseriesGroup)
        Dim lSelectDisplay As New frmSelectDisplay
        If Not aTitle Is Nothing AndAlso aTitle.Length > 0 Then lSelectDisplay.Text = aTitle
        lSelectDisplay.AskUser(aDataGroup)
    End Sub

    Public Shared Sub ShowDisplay(ByVal aDisplayName As String, ByVal aTimeseriesGroup As atcTimeseriesGroup)
        If aDisplayName Is Nothing OrElse aDisplayName.Length = 0 Then
            UserSelectDisplay("", aTimeseriesGroup)
        Else
            Dim lNewDisplay As atcDataDisplay
            For Each lDisplay As atcDataDisplay In GetPlugins(GetType(atcDataDisplay))
                If lDisplay.Name = aDisplayName OrElse lDisplay.Name.EndsWith("::" & aDisplayName) Then
                    Dim lType As System.Type = lDisplay.GetType()
                    Dim lAssembly As System.Reflection.Assembly = System.Reflection.Assembly.GetAssembly(lType)
                    lNewDisplay = lAssembly.CreateInstance(lType.FullName)
                    lNewDisplay.Initialize(pMapWin, Nothing) 'TODO: do we need the aParentHandle here?
                    lNewDisplay.Show(aTimeseriesGroup)
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
                                         Optional ByVal aNeedToSave As Boolean = False) As atcTimeseriesSource
        Dim lForm As New frmDataSource
        Dim lSelectedDataSource As atcTimeseriesSource = Nothing
        lForm.Text = aTitle
        lForm.AskUser(lSelectedDataSource, aNeedToOpen, aNeedToSave, aCategories)
        Return lSelectedDataSource
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="aTitle">Optional title for dialog window, default is 'Select Data'</param>
    ''' <param name="aSelected">Optional pre-selected group of data, default is no data already selected</param>
    ''' <param name="aAvailable">Optional group of all data available for selection, default is all open data</param>
    ''' <param name="aModal">Optional modality specification for window, default is True</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function UserSelectData(Optional ByVal aTitle As String = "", _
                                          Optional ByVal aSelected As atcDataGroup = Nothing, _
                                          Optional ByVal aAvailable As atcDataGroup = Nothing, _
                                          Optional ByVal aModal As Boolean = True) As atcDataGroup
        Return UserSelectData(aTitle, aSelected, aAvailable, aModal, True)
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="aTitle">Title for dialog window</param>
    ''' <param name="aSelected">pre-selected group of data</param>
    ''' <param name="aAvailable">group of all data available for selection</param>
    ''' <param name="aModal">modality specification for window</param>
    ''' <param name="aCancelReturnsOriginalSelected">choice of whether Cancel returns an empty group or the aSelected that was passed in</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function UserSelectData(ByVal aTitle As String, _
                                          ByVal aSelected As atcDataGroup, _
                                          ByVal aAvailable As atcDataGroup, _
                                          ByVal aModal As Boolean, _
                                          ByVal aCancelReturnsOriginalSelected As Boolean) As atcDataGroup
        Dim lForm As New frmSelectData
        Dim lNonePreSelected As Boolean = (aSelected Is Nothing OrElse aSelected.Count = 0)
        If aTitle.Length > 0 Then lForm.Text = aTitle
        If aAvailable IsNot Nothing Then lForm.AvailableData = aAvailable

        'Try automatically selecting data based on what is selected on the map

        If aSelected Is Nothing Then
            aSelected = DatasetsAtMapSelectedLocations()
        ElseIf aSelected.Count = 0 Then
            aSelected.AddRange(DatasetsAtMapSelectedLocations)
        End If

        aSelected = lForm.AskUser(aSelected, aModal)
        If Not lForm.SelectedOk Then 'User did not click Ok
            If lNonePreSelected OrElse Not aCancelReturnsOriginalSelected Then
                aSelected.Clear() 'caller does not want original selection back or there was not an original selection
            End If
        End If
        Return aSelected
    End Function

    ''' <summary>Ask user to manage data sources</summary>
    ''' <param name="aTitle">
    '''     <para>Optional title for dialog window, default is 'Data Sources'</para>
    ''' </param> 
    Public Shared Sub UserManage(Optional ByVal aTitle As String = "", Optional ByVal aDefaultIndex As Integer = -1)
        If pManagerForm Is Nothing OrElse pManagerForm.IsDisposed Then
            pManagerForm = New frmManager
        End If
        pManagerForm.BringToFront()
        pManagerForm.Focus()
        If aTitle.Length > 0 Then pManagerForm.Text = aTitle
        pManagerForm.Edit(aDefaultIndex)
    End Sub

    Friend Shared Function UserOpenDataFile(Optional ByVal aNeedToOpen As Boolean = True, _
                                            Optional ByVal aNeedToSave As Boolean = False) As atcTimeseriesSource
        Dim lFilesOnly As New ArrayList(1)
        lFilesOnly.Add("File")
        Dim lNewSource As atcTimeseriesSource = atcDataManager.UserSelectDataSource(lFilesOnly, "Select a File Type", aNeedToOpen, aNeedToSave)
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
        Dim lSaveIn As atcTimeseriesSource = Nothing
        Dim lSaveGroup As atcTimeseriesGroup = atcDataManager.UserSelectData("Select Data to Save")
        If Not lSaveGroup Is Nothing AndAlso lSaveGroup.Count > 0 Then

            'If we already have specified data source open, skip asking user
            If aSpecification IsNot Nothing AndAlso aSpecification.Length > 0 Then
                lSaveIn = atcDataManager.DataSourceBySpecification(aSpecification)
            End If

            If lSaveIn Is Nothing Then
                lSaveIn = UserOpenDataFile(False, True)
            End If

            If lSaveIn IsNot Nothing AndAlso lSaveIn.Specification.Length > 0 Then
                For Each lDataSet As atcDataSet In lSaveGroup
                    lSaveIn.AddDataSet(lDataSet, atcData.atcTimeseriesSource.EnumExistAction.ExistAskUser)
                Next
                Return lSaveIn.Save(lSaveIn.Specification)
            End If
        End If
        Return False
    End Function

    'Shared Function UserChooseButton(ByVal aTitle As String, _
    '                                 ByVal aMessage As String, _
    '                                 ByVal aButtonLabels As IEnumerable, _
    '                        Optional ByVal aTimeoutSeconds As Integer = 0, _
    '                        Optional ByVal aTimeoutLabel As String = "Cancel") As String
    '    Dim frm As New frmButtons
    '    frm.Icon = pMapWin.ApplicationInfo.FormIcon
    '    Return frm.AskUser(aTitle, aMessage, aButtonLabels, aTimeoutSeconds, aTimeoutLabel)
    'End Function
#End Region

#Region "MapWindow location selection"
    ''' <summary>
    ''' Return the currently loaded datasets whose Location attribute matches a currently selected shape on the map
    ''' </summary>
    Private Shared Function DatasetsAtMapSelectedLocations() As atcTimeseriesGroup
        Dim lGroup As New atcTimeseriesGroup
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

        Dim lLayerListFilename As String = FindFile("Please locate layers.dbf", g_PathChar & "BASINS\etc\DataDownload\layers.dbf") 'table containing location field for BASINS layers
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
     Public Shared Property XML() As String
        Get
            Dim lSaveXML As New Text.StringBuilder("<DataManager>")
            For Each lName As String In pSelectionAttributes
                lSaveXML.Append("<SelectionAttribute>" & lName & "</SelectionAttribute>")
            Next
            For Each lName As String In pDisplayAttributes
                lSaveXML.Append("<DisplayAttribute>" & lName & "</DisplayAttribute>")
            Next
            For Each lSource As atcTimeseriesSource In pDataSources
                If lSource.Category = "File" Then
                    lSaveXML.Append("<DataSource Specification='" & lSource.Specification & "'>" & lSource.Name & "</DataSource>")
                End If
            Next
            lSaveXML.Append("</DataManager>")
            Return lSaveXML.ToString
        End Get
        Set(ByVal newXML As String)
            Dim newValue As New Xml.XmlDocument
            newValue.LoadXml(newXML)
            Clear()
            Dim clearedSelectionAttributes As Boolean = False
            Dim clearedDisplayAttributes As Boolean = False
            For Each lchildXML As Xml.XmlNode In newValue.FirstChild.ChildNodes
                Select Case lchildXML.Name
                    Case "DataFile", "DataSource"
                        Dim lDataSourceType As String = lchildXML.InnerText
                        Dim lSpecification As String = lchildXML.Attributes.GetNamedItem("Specification").InnerText
                        If lDataSourceType Is Nothing OrElse lDataSourceType.Length = 0 Then
                            Logger.Msg("No data source type found for '" & lSpecification & "'", "Data type not specified")
                        Else
                            Dim lNewDataSource As atcTimeseriesSource = DataSourceByName(lDataSourceType)
                            If lNewDataSource Is Nothing Then
                                Logger.Msg("Unable to open data source of type '" & lDataSourceType & "'", "Data type not found")
                            ElseIf lNewDataSource.Category = "File" Then
                                If Not FileExists(lSpecification) Then
                                    Logger.Dbg("Skipping data file that does not exist: '" & lSpecification & "'")
                                Else
                                    OpenDataSource(lNewDataSource, lSpecification, Nothing)
                                End If
                            Else
                                Logger.Dbg("Not yet able to open data source that is not a File: " & lSpecification)
                            End If
                        End If
                    Case "SelectionAttribute"
                        If Not clearedSelectionAttributes Then
                            clearedSelectionAttributes = True
                            pSelectionAttributes.Clear()
                        End If
                        pSelectionAttributes.Add(lchildXML.InnerText)
                    Case "DisplayAttribute"
                        If Not clearedDisplayAttributes Then
                            pDisplayAttributes.Clear()
                            clearedDisplayAttributes = True
                        End If
                        pDisplayAttributes.Add(lchildXML.InnerText)
                End Select
            Next
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
        Return AddMenuWithIcon(aMenuName, aParent, aMenuText, Nothing, aAfter, aBefore, aAlphabetical)
    End Function

    <CLSCompliant(False)> _
    Public Shared Function AddMenuWithIcon(ByVal aMenuName As String, _
                                           ByVal aParent As String, _
                                           ByVal aMenuText As String, _
                                           ByVal aIcon As System.Drawing.Icon, _
                                  Optional ByVal aAfter As String = "", _
                                  Optional ByVal aBefore As String = "", _
                                  Optional ByVal aAlphabetical As Boolean = False) _
                                              As MapWindow.Interfaces.MenuItem
        If pMapWin Is Nothing Then
            Return Nothing
        Else
            Dim lImage As System.Drawing.Bitmap = Nothing
            If aIcon IsNot Nothing Then
                lImage = aIcon.ToBitmap
            ElseIf pMapWin IsNot Nothing AndAlso pMapWin.ApplicationInfo.FormIcon IsNot Nothing Then
                lImage = pMapWin.ApplicationInfo.FormIcon.ToBitmap
            End If

            If lImage IsNot Nothing Then
                'make meunu image more transparent using alpha channel
                For lY As Integer = 0 To lImage.Height - 1
                    For lX As Integer = 0 To lImage.Width - 1
                        Dim lClr As System.Drawing.Color = lImage.GetPixel(lX, lY)
                        lImage.SetPixel(lX, lY, _
                          System.Drawing.Color.FromArgb(96, lClr.R, lClr.G, lClr.B))
                    Next
                Next
            End If

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
                                Return .AddMenu(aMenuName, aParent, lImage, aMenuText, lExistingMenu.Name)
                            End If
                        End If
                        lSubmenuIndex += 1
                    End While
                    'Add at default position, after last parent subitem
                    Return .AddMenu(aMenuName, aParent, lImage, aMenuText)
                ElseIf aBefore.Length > 0 Then
                    Return .AddMenu(aMenuName, aParent, lImage, aMenuText, aBefore)
                Else
                    Return .AddMenu(aMenuName, aParent, lImage, aMenuText, aAfter)
                End If
            End With
        End If
    End Function
#End Region

    Public Overrides Function ToString() As String
        Dim lString As String = "atcDataManger:"
        For Each lDataSource As atcTimeseriesSource In pDataSources
            lString &= lDataSource.ToString & vbCrLf
        Next lDataSource
        Return lString.TrimEnd(New Char() {vbCr, vbLf})
    End Function
End Class
