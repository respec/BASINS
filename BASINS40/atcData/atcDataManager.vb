Imports atcUtility
Imports System.Reflection

''' <summary>Manages a set of currently open DataSources. 
'''          Uses the set of plugins currently loaded to find ones that inherit atcDataSource
''' </summary>
Public Class atcDataManager
  Private pMapWin As MapWindow.Interfaces.IMapWin
  Private pBasins As Object
  Private pDataSources As ArrayList 'of atcDataSource, the currently open data sources
  Private pSelectionAttributes As ArrayList
  Private pDisplayAttributes As ArrayList

  Private Const pInMemorySpecification As String = "<in memory>"

  ''' <summary>Event raised when a data source is opened</summary>
  Event OpenedData(ByVal aDataSource As atcDataSource)

  ''' <summary>Create a new instance of atcDataManager</summary>
  Public Sub New(ByVal aMapWin As MapWindow.Interfaces.IMapWin, ByVal aBasins As Object)
    pMapWin = aMapWin
    pBasins = aBasins
    Me.Clear()
  End Sub

  ''' <summary>Sets data manager to its initial state.
  '''          Defaults Datasources, Selection Attributes and Display Attributes.
  ''' </summary>
  Public Sub Clear()
    pDataSources = New ArrayList
    Dim lMemory As New atcDataSource
    lMemory.DataManager = Me
    lMemory.Specification = pInMemorySpecification
    pDataSources.Add(lMemory)

    pSelectionAttributes = New ArrayList
    pSelectionAttributes.Add("Scenario")
    pSelectionAttributes.Add("Location")
    pSelectionAttributes.Add("Constituent")

    pDisplayAttributes = New ArrayList
    pDisplayAttributes.Add("History 1")
    pDisplayAttributes.Add("Min")
    pDisplayAttributes.Add("Max")
    pDisplayAttributes.Add("Mean")
  End Sub

  'The BASINS plugin
  'Public ReadOnly Property Basins() As MapWindow.Interfaces.IPlugin
  '  Get
  '    Return pBasins
  '  End Get
  'End Property

  ''' <summary>Set of atcDataSource objects representing currently open DataSources</summary>
  Public ReadOnly Property DataSources() As ArrayList
    Get
      Return pDataSources
    End Get
  End Property

  ''' <summary>Set of atcDataSets found in currently open DataSources</summary>
  Public Function DataSets() As atcDataGroup
    Dim lAllData As New atcDataGroup
    For Each lSource As atcDataSource In DataSources
      For Each lTs As atcDataSet In lSource.DataSets
        lAllData.Add(lTs)
      Next
    Next
    Return lAllData
  End Function

  ''' <summary>Names of attributes used for selection of data in UI</summary)
  Public ReadOnly Property SelectionAttributes() As ArrayList
    Get
      Return pSelectionAttributes
    End Get
  End Property

  ''' <summary>Names of attributes used for listing of data in UI</summary>
  Public ReadOnly Property DisplayAttributes() As ArrayList
    Get
      Return pDisplayAttributes
    End Get
  End Property

  ''' <summary>Currently loaded plugins that inherit the specified class; returns empty objects</summary>
  ''' <param name="aBaseType">
  '''     <para>Type of plugin to match and return</para>
  ''' </param>  
  Public Function GetPlugins(ByVal aBaseType As Type) As atcCollection
    Dim lMatchingPlugIns As New atcCollection
    Dim lLastPlugIn As Integer = pMapWin.Plugins.Count() - 1
    For iPlugin As Integer = 0 To lLastPlugIn
      Dim lCurPlugin As MapWindow.Interfaces.IPlugin = pMapWin.Plugins.Item(iPlugin)
      If Not lCurPlugin Is Nothing Then
        If CType(lCurPlugin, Object).GetType().IsSubclassOf(aBaseType) Then
          lMatchingPlugIns.Add(lCurPlugin.Name, lCurPlugin)
        End If
      End If
    Next
    Return lMatchingPlugIns
  End Function

  ''' <summary>Open BASINS data source</summary>
  ''' <param name="aNewSource">
  '''     <para>Object containing instance of new data source</para>
  ''' </param>  
  ''' <param name="aSpecification">
  '''     <para>File name, connection string, or other information needed to initialize aNewSource</para>
  ''' </param>
  Public Function OpenDataSource(ByVal aNewSource As atcDataSource, ByVal aSpecification As String, ByVal aAttributes As atcDataAttributes) As Boolean
    aNewSource.DataManager = Me
    If aNewSource.Open(aSpecification, aAttributes) Then
      pDataSources.Add(aNewSource)
      RaiseEvent OpenedData(aNewSource)
      pMapWin.Project.Modified = True
      Return True
    Else
      LogDbg("Could not open '" & aSpecification & "' with '" & aNewSource.Name & "'")
      Return False
    End If
  End Function

  ''' <summary>Creates and returns an instance of a data source by name</summary>
  ''' <param name="aDataSourceName">
  '''     <para>Name of data source to create and return</para>
  ''' </param>  
  Public Function DataSourceByName(ByVal aDataSourceName As String) As atcDataSource
    For Each lDs As atcDataSource In GetPlugins(GetType(atcDataSource))
      If lDs.Name = aDataSourceName Then
        Return lDs.NewOne
      End If
    Next
  End Function

  ''' <summary>Ask user to select a data source</summary>
  ''' <param name="aCategories">
  '''     <para>Filter to limit user choices</para>
  ''' </param>  
  Public Function UserSelectDataSource(Optional ByVal aCategories As ArrayList = Nothing) As atcDataSource
    Dim lForm As New frmDataSource
    Dim lSelectedDataSource As atcDataSource
    lForm.AskUser(Me, lSelectedDataSource, True, False, aCategories)
    Return lSelectedDataSource
  End Function

  ''' <summary>Ask user to select data</summary>
  ''' <param name="aTitle">
  '''     <para>Optional title for dialog window, default is 'Select Data'</para>
  ''' </param>  
  ''' <param name="aGroup">
  '''     <para>Optional group to select data from, default is all available data</para>
  ''' </param>  
  ''' <param name="aModal">
  '''     <para>Optional modality specification for window, default is true</para>
  ''' </param>  
  Public Function UserSelectData(Optional ByVal aTitle As String = "", Optional ByVal aGroup As atcDataGroup = Nothing, Optional ByVal aModal As Boolean = True) As atcDataGroup
    Dim lForm As New frmSelectData
    If aTitle.Length > 0 Then lForm.Text = aTitle
    Return lForm.AskUser(Me, aGroup, aModal)
  End Function

  ''' <summary>Ask user to manage data sources</summary>
  ''' <param name="aTitle">
  '''     <para>Optional title for dialog window, default is 'Data Sources'</para>
  ''' </param> 
  Public Sub UserManage(Optional ByVal aTitle As String = "")
    Dim lForm As New frmManager
    If aTitle.Length > 0 Then lForm.Text = aTitle
    lForm.Edit(Me)
  End Sub

  ''' <summary>State of data manager in XML format</summary>
  Public Property XML() As Chilkat.Xml
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
        If lSource.CanSave Then 'TODO: better test to pass only types that just need a Specification string to open
          If Not lSource.Specification.Equals(pInMemorySpecification) Then
            lChildXML = lSaveXML.NewChild("DataSource", lSource.Name)
            lChildXML.AddAttribute("Specification", lSource.Specification)
          End If
        End If
      Next
      Return lSaveXML
    End Get
    Set(ByVal newValue As Chilkat.Xml)
      Me.Clear()
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
                LogMsg("No data source type found for '" & lSpecification & "'", "Data type not specified")
              Else
                Dim lNewDataSource As atcDataSource = DataSourceByName(lchildXML.Content)
                If lNewDataSource Is Nothing Then
                  LogMsg("Unable to open data source of type '" & lDataSourceType & "'", "Data type not found")
                Else
                  OpenDataSource(lNewDataSource, lSpecification, Nothing)
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
End Class
