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
    lMemory.DataManager = Me
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

  'aSpecification = file name, connection string, or other information needed to initialize aNewSource
  Public Function OpenDataSource(ByVal aNewSource As atcDataSource, ByVal aSpecification As String, ByVal aAttributes As atcDataAttributes) As Boolean
    aNewSource.DataManager = Me
    If aNewSource.Open(aSpecification, aAttributes) Then
      pDataSources.Add(aNewSource)
      RaiseEvent OpenedData(aNewSource)
      Return True
    Else
      'TODO: LogError("Could not open '" & aSpecification & "' with '" & aNewSource.Name & "'")
      Return False
    End If
  End Function

  Public Function UserOpenDataSource(Optional ByVal aCategories As ArrayList = Nothing, Optional ByVal aGroup As atcDataGroup = Nothing) As atcDataSource
    Dim frmDS As New frmDataSource
    Dim lSpecification As String = ""
    Dim lSelectedDataSource As atcDataSource
    frmDS.AskUser(Me, lSelectedDataSource, lSpecification, True, False, aCategories)
    If Not lSelectedDataSource Is Nothing Then
      If OpenDataSource(lSelectedDataSource, lSpecification, Nothing) Then
        If Not aGroup Is Nothing Then
          For Each lDataSet As atcDataSet In lSelectedDataSource.DataSets
            aGroup.Add(lDataSet)
          Next
        End If
      End If
    End If
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
