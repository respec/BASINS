'Store attributes (and calculate some attributes if given an atcTimeseries)
'Attributes are stored as a collection of atcDefinedValue

Imports atcUtility

Public Class atcDataAttributes
  Inherits atcCollection

  'Private pOwner As atcTimeseries
  Private Shared pAllAliases As atcCollection     'of String, so more than one AttributeName can map to the same attribute
  Private Shared pAllDefinitions As atcCollection 'of atcAttributeDefinition

  'Returns the names (as keys) and values of all attributes that are set. (sorted by name)
  Public Function ValuesSortedByName() As SortedList
    Dim lAll As New SortedList(New CaseInsensitiveComparer)
    For Each lAdv As atcDefinedValue In Me
      lAll.Add(lAdv.Definition.Name, lAdv.Value)
    Next
    Return lAll
  End Function

  'True if aAttributeName has been set
  Public Function ContainsAttribute(ByVal aAttributeName As String) As Boolean
    Return Keys.Contains(aAttributeName.ToLower)
  End Function

  ''True if aAttributeName can be calculated
  'Public Function canCalculate(ByVal aAttributeName As String) As Boolean
  '  If pOwner Is Nothing Then Return False 'Can't calculate any without a atcTimeseries
  '  'TODO: insert code for evaluating whether a value for attributeName can be calculated
  '  Return False
  'End Function

  'Retrieve the atcAttributeDefinition for aAttributeName
  'returns aDefault if attribute has not been set and cannot calculate
  Public Function GetDefinition(ByVal aAttributeName As String) As atcAttributeDefinition
    Dim tmpAttribute As atcDefinedValue
    Try
      tmpAttribute = GetDefinedValue(aAttributeName)
      If Not tmpAttribute Is Nothing Then
        Return tmpAttribute.Definition
      End If
    Catch
      'Not found
    End Try
    Return Nothing 'Not found or no definition found
  End Function

  'Retrieve or calculate the value for aAttributeName
  'returns aDefault if attribute has not been set and cannot be calculated
  Public Function GetValue(ByVal aAttributeName As String, Optional ByVal aDefault As Object = "") As Object
    Dim key As String = aAttributeName.ToLower
    Dim tmpAttribute As atcDefinedValue
    Try
      tmpAttribute = GetDefinedValue(aAttributeName)
      Return tmpAttribute.Value
    Catch  'Could not find 

      'TODO: Try to calculate attribute?

      Return aDefault 'Not found and could not calculate
    End Try
  End Function

  'Set attribute with definition aAttrDefinition to value aValue
  Public Function SetValue(ByVal aAttrDefinition As atcAttributeDefinition, ByVal aValue As Object, Optional ByVal aArguments As atcDataAttributes = Nothing) As Integer
    Dim key As String = aAttrDefinition.Name.ToLower
    Dim lAlias As String = pAllAliases.ItemByKey(key)

    Dim tmpAttrDefVal As atcDefinedValue

    If Not lAlias Is Nothing Then 'We have a preferred alias for this name
      key = lAlias.ToLower        'use the preferred alias instead
      aAttrDefinition.Name = lAlias
    End If

    Dim index As Integer = MyBase.Keys.IndexOf(key)
    If index = -1 Then
      Try
        pAllDefinitions.Add(key, aAttrDefinition)
      Catch
        'already present, no problem
      End Try
      tmpAttrDefVal = New atcDefinedValue
      tmpAttrDefVal.Definition = aAttrDefinition
      tmpAttrDefVal.Value = aValue
      If Not aArguments Is Nothing Then tmpAttrDefVal.Arguments = aArguments
      index = MyBase.Add(key, tmpAttrDefVal)
    Else  'Update existing attribute value
      tmpAttrDefVal = ItemByIndex(index)
      tmpAttrDefVal.Value = aValue
      If Not aArguments Is Nothing Then tmpAttrDefVal.Arguments = aArguments
    End If
    Return index
  End Function

  Public Sub SetValue(ByVal aAttributeName As String, ByVal aAttributeValue As Object)
    Add(aAttributeName, aAttributeValue)
  End Sub

  'Set attribute with name aAttributeName to value aValue
  Public Overloads Overrides Function Add(ByVal aAttributeName As Object, ByVal aAttributeValue As Object) As Integer
    Dim lTmpAttrDef As New atcAttributeDefinition
    lTmpAttrDef = pAllDefinitions.ItemByKey(aAttributeName.ToLower)
    If lTmpAttrDef Is Nothing Then
      lTmpAttrDef = New atcAttributeDefinition
      lTmpAttrDef.Name = aAttributeName
    End If
    SetValue(lTmpAttrDef, aAttributeValue)
  End Function

  Public Overloads Overrides Function Add(ByVal aDefinedValue As Object) As Integer
    If aDefinedValue Is Nothing Then
      Return -1
    Else
      Return SetValue(aDefinedValue.Definition, aDefinedValue.Value, aDefinedValue.Arguments)
    End If
  End Function

  Public Sub New() 'ByVal aTimeseries As atcTimeseries)
    'pOwner = aTimeseries
    MyBase.Clear()

    If pAllDefinitions Is Nothing Then
      pAllDefinitions = New atcCollection
      Dim lUnitsDef = New atcAttributeDefinition
      lUnitsDef.Name = "Units"
      lUnitsDef.TypeString = "String"
      lUnitsDef.Editable = True
      'lUnitsDef.ValidList = GetAllUnitsInCategory("all")
      pAllDefinitions.Add("units", lUnitsDef)
    End If

    If pAllAliases Is Nothing Then
      pAllAliases = New atcCollection 'of alias and internal name
      With pAllAliases
        .Add("sen", "Scenario")
        .Add("scen", "Scenario")
        .Add("idscen", "Scenario")

        .Add("loc", "Location")
        .Add("locn", "Location")
        .Add("idlocn", "Location")

        .Add("con", "Constituent")
        .Add("cons", "Constituent")
        .Add("idcons", "Constituent")

        .Add("desc", "Description")
        .Add("stanam", "Description")
        .Add("station name", "Description")

        .Add("long filename", "FileName")
        .Add("path", "FileName")

        .Add("ts", "Time Step")
        .Add("tu", "Time Unit")

        .Add("id", "ID")

        .Add("datcre", "Date Created")
        .Add("datmod", "Date Modified")
      End With
    End If

    '  Case "MAX" : Attrib = Max
    '  Case "MIN" : Attrib = Min
    '  Case "MEAN" : Attrib = Mean
    '  Case "GEOMETRIC MEAN" : Attrib = GeometricMean
    '  Case "SUM" : Attrib = Sum
    '  Case "STDDEVIATION" : Attrib = StdDeviation
    '  Case "VARIANCE" : Attrib = Variance
  End Sub

  'Public Function GetDefinedValue(ByVal aIndex As Integer) As atcDefinedValue
  '  Return ItemByIndex(aIndex)
  'End Function

  Public Function GetDefinedValue(ByVal aAttributeName As String) As atcDefinedValue
    Dim key As String = aAttributeName.ToLower
    Dim tmpAttribute As atcDefinedValue = ItemByKey(key)
    If Not tmpAttribute Is Nothing Then
      Return tmpAttribute
    Else
      key = pAllAliases.ItemByKey(key)
      If key Is Nothing Then
        Return Nothing
      Else
        key = key.ToString.ToLower
      End If

      If key.Equals(aAttributeName.ToLower) Then
        Return Nothing 'Not found and could not calculate
      Else
        Return GetDefinedValue(key)
      End If
    End If
  End Function

End Class
