'Store attributes (and calculate some attributes if given an atcTimeseries)
Public Class atcDataAttributes
  Private pOwner As atcTimeseries
  Private pAttributes As Hashtable         'of atcDefinedValue
  Private Shared pAliases As Hashtable     'of String, so more than one AttributeName can map to the same attribute
  Private Shared pDefinitions As Hashtable 'of atcAttributeDefinition

  'Returns the names (as keys) and values of all attributes that are set. (sorted by name)
  Public Function GetAll() As SortedList
    Dim lAll As New SortedList(New CaseInsensitiveComparer)
    Dim lAdv As atcDefinedValue
    For Each lKey As String In pAttributes.Keys
      lAdv = pAttributes.Item(lKey)
      lAll.Add(lAdv.Definition.Name, lAdv.Value)
    Next
    Return lAll
  End Function

  'True if aAttributeName has been set
  Public Function ContainsAttribute(ByVal aAttributeName As String) As Boolean
    Return pAttributes.ContainsKey(aAttributeName.ToLower)
  End Function

  'True if aAttributeName can be calculated
  Public Function canCalculate(ByVal aAttributeName As String) As Boolean
    If pOwner Is Nothing Then Return False 'Can't calculate any without a atcTimeseries
    'TODO: insert code for evaluating whether a value for attributeName can be calculated
    Return False
  End Function

  'Retrieve the atcAttributeDefinition for aAttributeName
  'returns aDefault if attribute has not been set and cannot calculate
  Public Function GetDefinition(ByVal aAttributeName As String) As atcAttributeDefinition
    Dim tmpAttribute As atcDefinedValue
    Try
      tmpAttribute = GetDefinedValue(aAttributeName)
      Return tmpAttribute.Definition
    Catch
      Return Nothing 'Not found
    End Try
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
  Public Sub SetValue(ByVal aAttrDefinition As atcAttributeDefinition, ByVal aValue As Object)
    Dim key As String = aAttrDefinition.Name.ToLower
    Dim lAlias As String = pAliases.Item(key)

    Dim tmpAttrDefVal As atcDefinedValue

    If Not lAlias Is Nothing Then 'We have a preferred alias for this name
      key = lAlias.ToLower        'use the preferred alias instead
      aAttrDefinition.Name = lAlias
    End If

    tmpAttrDefVal = pAttributes.Item(key)
    If tmpAttrDefVal Is Nothing Then
      Try
        pDefinitions.Add(key, aAttrDefinition)
      Catch
        'already present, no problem
      End Try
      tmpAttrDefVal = New atcDefinedValue
      tmpAttrDefVal.Definition = aAttrDefinition
      tmpAttrDefVal.Value = aValue
      pAttributes.Add(key, tmpAttrDefVal)
    Else  'Update existing attribute value
      tmpAttrDefVal.Value = aValue
    End If
  End Sub

  'Set attribute with name aAttributeName to value aValue
  Public Sub SetValue(ByVal aAttributeName As String, ByVal aValue As Object)
    Dim lTmpAttrDef As New atcAttributeDefinition
    lTmpAttrDef = pDefinitions.Item(aAttributeName.ToLower)
    If lTmpAttrDef Is Nothing Then
      lTmpAttrDef = New atcAttributeDefinition
      lTmpAttrDef.Name = aAttributeName
    End If
    SetValue(lTmpAttrDef, aValue)
  End Sub

  Public Sub New(ByVal aTimeseries As atcTimeseries)
    pOwner = aTimeseries
    pAttributes = New Hashtable

    If pDefinitions Is Nothing Then
      pDefinitions = New Hashtable
      Dim lUnitsDef = New atcAttributeDefinition
      lUnitsDef.Name = "Units"
      lUnitsDef.TypeString = "String"
      lUnitsDef.Editable = True
      'lUnitsDef.ValidList = GetAllUnitsInCategory("all")
      pDefinitions.Add("units", lUnitsDef)
    End If

    If pAliases Is Nothing Then
      pAliases = New Hashtable 'of alias and internal name
      With pAliases
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

  Public Function GetDefinedValue(ByVal aAttributeName As String, Optional ByVal aDefault As Object = "") As atcDefinedValue
    Dim key As String = aAttributeName.ToLower
    Dim tmpAttribute As atcDefinedValue
    tmpAttribute = pAttributes.Item(key)
    If Not tmpAttribute Is Nothing Then
      Return tmpAttribute
    Else
      key = pAliases.Item(key).ToString.ToLower
      If key Is Nothing OrElse key.Equals(aAttributeName.ToLower) Then
        Return aDefault 'Not found and could not calculate
      Else
        Return GetDefinedValue(key, aDefault)
      End If
    End If
  End Function
End Class
