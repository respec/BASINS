'Store attributes (and calculate some attributes if given an atcTimeseries)
'Attributes are stored as a collection of atcDefinedValue

Imports atcUtility

Public Class atcDataAttributes
  Inherits atcCollection

  Private pOwner As Object 'atcTimeseries
  Private Shared pAllAliases As atcCollection     'of String, so more than one AttributeName can map to the same attribute
  Private Shared pAllDefinitions As atcCollection 'of atcAttributeDefinition

  'Returns lowercase key for use in Me and pAllDefinitions
  'Warning: modifies argument aAttributeName if a preferred alias is found
  Private Shared Function AttributeNameToKey(ByRef aAttributeName As String) As String
    Dim key As String = aAttributeName.ToLower
    Dim lAlias As String = pAllAliases.ItemByKey(key)
    If Not lAlias Is Nothing Then 'We have a preferred alias for this name
      aAttributeName = lAlias
      Return lAlias.ToLower       'use the preferred alias instead
    Else
      Return key
    End If
  End Function

  Public Shared Sub AddDefinition(ByVal aDefinition As atcAttributeDefinition)
    Dim key As String = AttributeNameToKey(aDefinition.Name)
    If Not pAllDefinitions.Keys.Contains(key) Then
      pAllDefinitions.Add(key, aDefinition)
    End If
  End Sub

  'Retrieve the atcAttributeDefinition for aAttributeName
  Public Shared Function GetDefinition(ByVal aAttributeName As String) As atcAttributeDefinition
    Return pAllDefinitions.ItemByKey(AttributeNameToKey(aAttributeName))
  End Function

  Public Shared Function AllDefinitions() As atcCollection
    Return pAllDefinitions
  End Function

  Public Property Owner() As Object
    Get
      Return pOwner
    End Get
    Set(ByVal newValue As Object)
      pOwner = newValue
    End Set
  End Property

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


  Public Function GetFormattedValue(ByVal aAttributeName As String, Optional ByVal aDefault As Object = "") As String
    'TODO: use definition for formatting 
    Dim lValue As Object = GetValue(aAttributeName, aDefault)

    If TypeOf (lValue) Is Double Then
      If InStr(LCase(aAttributeName), "jday", CompareMethod.Text) Then
        Return DumpDate(lValue)
      Else
        Return Format(lValue, "#,##0.#####")
      End If
    Else
      Return CStr(lValue)
    End If
  End Function

  'Retrieve or calculate the value for aAttributeName
  'returns aDefault if attribute has not been set and cannot be calculated
  Public Function GetValue(ByVal aAttributeName As String, Optional ByVal aDefault As Object = "") As Object
    Dim key As String = aAttributeName.ToLower
    Dim tmpAttribute As atcDefinedValue
    Try
      tmpAttribute = GetDefinedValue(aAttributeName)
    Catch  'Could not find 

      'TODO: Try to calculate attribute?

      Return aDefault 'Not found and could not calculate
    End Try
    If tmpAttribute Is Nothing Then
      Return aDefault
    Else
      Return tmpAttribute.Value
    End If
  End Function

  'Set attribute with definition aAttrDefinition to value aValue
  Public Function SetValue(ByVal aAttrDefinition As atcAttributeDefinition, ByVal aValue As Object, Optional ByVal aArguments As atcDataAttributes = Nothing) As Integer
    Dim key As String = AttributeNameToKey(aAttrDefinition.Name)
    Dim tmpAttrDefVal As atcDefinedValue
    Dim index As Integer = MyBase.Keys.IndexOf(key)
    If index = -1 Then
      AddDefinition(aAttrDefinition)
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
  Public Shadows Function Add(ByVal aAttributeName As String, ByVal aAttributeValue As Object) As Integer
    Dim lTmpAttrDef As New atcAttributeDefinition
    lTmpAttrDef = pAllDefinitions.ItemByKey(aAttributeName.ToLower)
    If lTmpAttrDef Is Nothing Then
      lTmpAttrDef = New atcAttributeDefinition
      lTmpAttrDef.Name = aAttributeName
    End If
    SetValue(lTmpAttrDef, aAttributeValue)
  End Function

  Public Shadows Function Add(ByVal aDefinedValue As Object) As Integer
    If aDefinedValue Is Nothing Then
      Return -1
    Else
      Return SetValue(aDefinedValue.Definition, aDefinedValue.Value, aDefinedValue.Arguments)
    End If
  End Function

  Public Sub New() 'ByVal aTimeseries As atcTimeseries)
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

  Public Shadows Function Clone() As atcDataAttributes
    Dim newClone As New atcDataAttributes
    For Each lAdv As atcDefinedValue In Me
      newClone.SetValue(lAdv.Definition, lAdv.Value, lAdv.Arguments)
    Next
    Return newClone
  End Function

  'Public Function GetDefinedValue(ByVal aIndex As Integer) As atcDefinedValue
  '  Return ItemByIndex(aIndex)
  'End Function

  Public Function GetDefinedValue(ByVal aAttributeName As String) As atcDefinedValue
    Dim key As String = AttributeNameToKey(aAttributeName)
    Dim tmpAttribute As atcDefinedValue = ItemByKey(key)
    If tmpAttribute Is Nothing Then  'Did not find the named attribute
      If Not Owner Is Nothing Then   'Need an owner to calculate an attribute
        Try
          Dim lOwnerTS As atcTimeseries = Owner
          Dim tmpDef As atcAttributeDefinition = pAllDefinitions.ItemByKey(key)
          If Not tmpDef Is Nothing Then 'We have a definition for this attribute but no value
            If tmpDef.Calculated Then   'Maybe we can go ahead and calculate it now...
              Dim lOperation As atcDataSet = tmpDef.Calculator.AvailableOperations.ItemByKey(aAttributeName.ToLower)
              If Not lOperation Is Nothing Then
                If lOperation.Attributes.Count = 1 Then 'Calculation should have just result as an attribute
                  Dim lCalculation As atcDefinedValue = lOperation.Attributes.ItemByIndex(0)
                  If lCalculation.Arguments.Count = 1 Then 'Simple calculation has only one argument
                    Dim lArg As atcDefinedValue = lCalculation.Arguments.ItemByIndex(0)
                    If lArg.Definition.TypeString = "atcTimeseries" Then 'Only argument must be atcTimeseries
                      Dim tmpArgs As atcDataAttributes = lCalculation.Arguments.Clone
                      tmpArgs.SetValue(lArg.Definition, lOwnerTS)
                      tmpDef.Calculator.Open(tmpDef.Name, tmpArgs)
                      tmpAttribute = ItemByKey(key)
                    End If
                  End If
                End If
              End If
            End If
          End If
        Catch CalcExcep As Exception
        End Try
      End If
    End If
    Return tmpAttribute
  End Function

  Public Shadows Property ItemByIndex(ByVal index As Integer) As atcDefinedValue
    Get
      Return MyBase.Item(index)
    End Get
    Set(ByVal newValue As atcDefinedValue)
      MyBase.Item(index) = newValue
    End Set
  End Property
  Default Public Shadows Property Item(ByVal index As Integer) As atcDefinedValue
    Get
      Return MyBase.Item(index)
    End Get
    Set(ByVal newValue As atcDefinedValue)
      MyBase.Item(index) = newValue
    End Set
  End Property
End Class
