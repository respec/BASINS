'Manages a set of currently open DataSources
'Uses the set of plugins currently loaded to find ones that inherit atcDataSource

Imports atcUtility

Imports System.Reflection

Public Class atcDataManager
  Private pMapWin As MapWindow.Interfaces.IMapWin
  Private pBasins As Object
  Private pDataSources As ArrayList 'of atcDataSource, the currently open data sources
  Private pSelectionAttributes As ArrayList
  Private pDisplayAttributes As ArrayList

  Event OpenedData(ByVal aDataSource As atcDataSource)

  Public Sub New(ByVal aMapWin As MapWindow.Interfaces.IMapWin, ByVal aBasins As Object)
    pMapWin = aMapWin
    pBasins = aBasins
    Me.Clear()
  End Sub

  Public Sub Clear()
    pDataSources = New ArrayList
    Dim lMemory As New atcDataSource
    lMemory.Specification = "<in memory>"
    pDataSources.Add(lMemory)

    pSelectionAttributes = New ArrayList
    pSelectionAttributes.Add("Scenario")
    pSelectionAttributes.Add("Location")
    pSelectionAttributes.Add("Constituent")

    pDisplayAttributes = pSelectionAttributes.Clone()
  End Sub

  'The set of atcDataSource objects representing currently open DataSources
  Public ReadOnly Property DataSources() As ArrayList
    Get
      Return pDataSources
    End Get
  End Property

  'Names of attributes used for selection of Data in UI
  Public ReadOnly Property SelectionAttributes() As ArrayList
    Get
      Return pSelectionAttributes
    End Get
  End Property

  'Names of attributes used for listing of Data in UI
  Public ReadOnly Property DisplayAttributes() As ArrayList
    Get
      Return pDisplayAttributes
    End Get
  End Property

  'The currently loaded plugins that inherit the specified class; returns empty objects
  Public Function GetPlugins(ByVal aBaseType As Type) As ICollection
    Dim retval As New ArrayList
    Dim lastPlugIn As Integer = pMapWin.Plugins.Count() - 1
    For iPlugin As Integer = 0 To lastPlugIn
      Dim curPlugin As MapWindow.Interfaces.IPlugin = pMapWin.Plugins.Item(iPlugin)
      If Not curPlugin Is Nothing Then
        If CType(curPlugin, Object).GetType().IsSubclassOf(aBaseType) Then
          retval.Add(curPlugin)
        End If
      End If
    Next
    Return retval
  End Function

  Public Function UserAddDataSource() As atcDataSource

  End Function

  'aSpecification = file name, connection string, or other information needed to initialize aNewSource
  Public Function AddDataSource(ByVal aNewSource As atcDataSource, ByVal aSpecification As String, ByVal aAttributes As atcDataAttributes) As Boolean
    If aNewSource.Open(aSpecification, aAttributes) Then
      pDataSources.Add(aNewSource)
      RaiseEvent OpenedData(aNewSource)
      Return True
    Else
      'TODO: LogError("Could not open '" & aSpecification & "' with '" & aNewSource.Name & "'")
      Return False
    End If
  End Function

  'Open a data source and return the new atcDataSource object
  'aFileFilter: selected filter in Open dialog - used to determine which class can open file
  'if aFileFilter is omitted, Open tries searching for a class that supports the extension of aFileName
  'Public Function OpenData(ByVal aFileNameOrConnectString As String) As atcDataSource ', Optional ByVal aFileFilter As String = "") As atcDataSource
  '  Dim newSource As atcDataSource

  '  pBasins.Busy = True
  '  If aFileNameOrConnectString.Length = 0 Then
  '    Dim allFileFilters As String = FileFilters()
  '    If aFileFilter.Length = 0 Then
  '      aFileFilter = GetSetting("atcData", "DataSource", "LastFileFilter")
  '    End If
  '    Dim FileFilterIndex As Integer = 1
  '    If aFileFilter.Length > 0 Then
  '      FileFilterIndex = FindFileFilterIndex(allFileFilters, aFileFilter)
  '    End If
  '    aFileNameOrConnectString = FindFile("Select data file to open", , , allFileFilters, True, , FileFilterIndex)
  '    aFileFilter = FindFileFilter(allFileFilters, FileFilterIndex)
  '    SaveSetting("atcData", "DataSource", "LastFileFilter", aFileFilter)
  '  End If

  '  If aFileNameOrConnectString.Length > 0 Then
  '    If aFileFilter.Length = 0 Then aFileFilter = System.IO.Path.GetExtension(aFileNameOrConnectString)
  '    aFileFilter = aFileFilter.ToLower

  '    Dim DataSourcePlugins As ICollection = GetPlugins(GetType(atcDataSource))
  '    For Each atf As atcDataSource In DataSourcePlugins
  '      'Might need a better test than this, or try more than one if multiple types open 
  '      'files with the same extension or if filter is not set for a atcDataSource type
  '      If atf.FileFilter.ToLower.IndexOf(aFileFilter) >= 0 Then
  '        Dim typ As System.Type = atf.GetType()
  '        Dim asm As System.Reflection.Assembly = System.Reflection.Assembly.GetAssembly(typ)
  '        newSource = asm.CreateInstance(typ.FullName)
  '      End If
  '    Next
  '  End If

  '  If newSource Is Nothing Then
  '    'TODO: LogError("Could not find a loaded plugin that can open '" & aFileNameOrConnectString & "'")
  '  Else
  '    If newSource.Open(aFileNameOrConnectString) Then
  '      pDataSources.Add(newSource)
  '      RaiseEvent OpenedData(newSource)
  '      OpenData = newSource
  '    Else
  '      'TODO: LogError("Could not open '" & aFileNameOrConnectString & "' with '" & newSource.Name & "'")
  '    End If
  '  End If
  '  pBasins.Busy = False
  'End Function

  'Returns FileFilters of all loaded atcDataSource types, formatted for common dialog
  'Public Function FileFilters() As String
  '  Dim retval As String = ""
  '  Dim DataSourcePlugins As ICollection = GetPlugins(GetType(atcDataSource))
  '  For Each atf As atcDataSource In DataSourcePlugins
  '    If retval.Length > 0 And atf.FileFilter.Length > 0 Then retval += "|" 'separate with |
  '    retval += atf.FileFilter
  '  Next
  '  Return retval
  'End Function

  Public Function UserCompute(Optional ByVal aGroup As atcDataGroup = Nothing) As atcDataSet
    Dim frmCompute As New atcData.frmComputeData
    Return frmCompute.AskUser(Me, aGroup)
  End Function

  Public Function UserSelectData(Optional ByVal aTitle As String = "", Optional ByVal aGroup As atcDataGroup = Nothing, Optional ByVal aModal As Boolean = True) As atcDataGroup
    Dim frmSelect As New atcData.frmSelectData
    If aTitle.Length > 0 Then frmSelect.Text = aTitle
    Return frmSelect.AskUser(Me, aGroup, aModal)
  End Function

  Public Sub UserManage(Optional ByVal aTitle As String = "")
    Dim frmManage As New frmManager
    If aTitle.Length > 0 Then frmManage.Text = aTitle
    frmManage.Edit(Me)
  End Sub

  Public Property XML() As Chilkat.Xml
    Get
      Dim saveXML As New Chilkat.Xml
      Dim lchildXML As Chilkat.Xml
      saveXML.Tag = "DataManager"
      For Each lName As String In pSelectionAttributes
        saveXML.NewChild("SelectionAttribute", lName)
      Next
      For Each lName As String In pDisplayAttributes
        saveXML.NewChild("DisplayAttribute", lName)
      Next
      For Each lSource As atcDataSource In pDataSources
        lchildXML = saveXML.NewChild("DataSource", "")
        lchildXML.AddAttribute("Specification", lSource.Specification)
        'lchildXML.AddAttribute("Filter", lSource.FileFilter)
      Next
      Return saveXML
    End Get
    Set(ByVal newValue As Chilkat.Xml)
      Dim clearedSelectionAttributes As Boolean = False
      Dim clearedDisplayAttributes As Boolean = False
      Dim lchildXML As Chilkat.Xml
      Me.Clear()
      lchildXML = newValue.FirstChild

      While Not lchildXML Is Nothing
        Select Case lchildXML.Tag
          Case "DataFile", "DataSource"
            OpenFile(lchildXML.GetAttrValue("FileName"), lchildXML.GetAttrValue("Filter"))
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

    End Set
  End Property
End Class
