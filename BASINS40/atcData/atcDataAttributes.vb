Imports atcUtility
Imports MapWinUtility

''' <summary>
'''     <para>
'''         Store attributes (and calculate some attributes if given an
'''         <see cref="atcData~atcData.atcTimeseries">atcTimeseries</see>)
'''     </para>
''' </summary>
''' <remarks>Attributes are stored as a collection of atcDefinedValue</remarks>
Public Class atcDataAttributes
  Inherits atcCollection

  Private pOwner As Object 'atcTimeseries
  Private Shared pAllAliases As atcCollection     'of String, so more than one AttributeName can map to the same attribute
  Private Shared pAllDefinitions As atcCollection 'of atcAttributeDefinition
  Private Shared pDateFormat As New atcDateFormat

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

  'Returns collection of all known atcAttributeDefinition objects
  Public Shared Function AllDefinitions() As atcCollection
    Return pAllDefinitions
  End Function

  Public Sub AddHistory(ByVal aNewEvent As String, Optional ByVal aInsertAt As Integer = 1)
    Dim prevHistory As String = GetValue("History " & aInsertAt, Nothing)
    If Not prevHistory Is Nothing Then
      AddHistory(prevHistory, aInsertAt + 1)
    End If
    SetValue("History " & aInsertAt, aNewEvent)
  End Sub

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
    Return Keys.Contains(AttributeNameToKey(aAttributeName))
  End Function

  Public Function GetFormattedValue(ByVal aAttributeName As String, Optional ByVal aDefault As Object = "") As String
    'TODO: use definition for formatting 
    Try
      Dim lValue As Object = GetValue(aAttributeName, aDefault)
      Try
        If TypeOf (lValue) Is Double Then
          If InStr(LCase(aAttributeName), "jday", CompareMethod.Text) Then
            Return pDateFormat.JDateToString(lValue)
          Else
            Return DoubleToString(lValue, 15)
          End If
        ElseIf TypeOf (lValue) Is Integer Then
          Return Format(lValue, "#,###;-#,###;0")
        ElseIf TypeOf (lValue) Is atcTimeseries Then
          Return lValue.ToString
        ElseIf TypeOf (lValue) Is atcDataGroup Then
          Return lValue.ToString
        ElseIf InStr(aAttributeName.ToLower, "history") > 0 Then
          If InStr(lValue.ToString.ToLower, "read from") Then 'make value shorter by removing path and "read "
            Dim lString() As String = lValue.ToString.Split(" ")
            lString(0) = "from " & FilenameNoPath(lString(2))
            Return lString(0)
          Else
            Return lValue.ToString
          End If
        Else
          Return lValue.ToString
        End If
      Catch
        Return "<" & lValue.GetType.Name & ">"
      End Try
    Catch
      Return "<nothing>"
    End Try
  End Function

  'Retrieve or calculate the value for aAttributeName
  'returns aDefault if attribute has not been set and cannot be calculated
  Public Function GetValue(ByVal aAttributeName As String, Optional ByVal aDefault As Object = Nothing) As Object
    Dim key As String = AttributeNameToKey(aAttributeName)
    Dim tmpAttribute As atcDefinedValue
    Try
      tmpAttribute = GetDefinedValue(aAttributeName)
    Catch  'Could not find 

      'TODO: Try to calculate attribute?

      Return aDefault 'Not found and could not calculate
    End Try

    If tmpAttribute Is Nothing Then
      If aDefault Is Nothing Then
        Dim lDef As atcAttributeDefinition = GetDefinition(aAttributeName)
        If lDef Is Nothing Then
          Return aDefault
        Else
          Return lDef.DefaultValue
        End If
      Else
        Return aDefault
      End If
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
      tmpAttrDefVal = New atcDefinedValue
      tmpAttrDefVal.Definition = aAttrDefinition
      tmpAttrDefVal.Value = aValue
      If aArguments Is Nothing Then
        AddDefinition(aAttrDefinition) 'Add definition for attributes without arguments
      Else
        tmpAttrDefVal.Arguments = aArguments
        If aArguments.GetValue("SeasonDefinition") Is Nothing Then
          AddDefinition(aAttrDefinition) 'Add definition for attributes without season
        End If
      End If
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
    lTmpAttrDef = pAllDefinitions.ItemByKey(AttributeNameToKey(aAttributeName))
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
      Dim lUnitsDef As New atcAttributeDefinition
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

        .Add("7low10", "7Q10")
      End With
    End If
  End Sub

  Public Shadows Function Clone() As atcDataAttributes
    Dim newClone As New atcDataAttributes
    For Each lAdv As atcDefinedValue In Me
      newClone.SetValue(lAdv.Definition, lAdv.Value, lAdv.Arguments)
    Next
    Return newClone
  End Function

  'Calculate all the known attributes that can be calculated with no additional arguments
  Public Sub CalculateAll()
    Dim lCalculateThese As New ArrayList
    For Each lDef As atcAttributeDefinition In pAllDefinitions 'For each kind of attribute we know about
      If lDef.Calculated Then                                  'This attribute can be calculated
        Dim key As String = AttributeNameToKey(lDef.Name)
        If ItemByKey(key) Is Nothing Then                      'We do not yet have a value for this attribute
          lCalculateThese.Add(key)
        End If
      End If
    Next

    'Had to separate calculation from enumerating pAllDefinitions since definitions may get added
    For Each lAttributeKey As String In lCalculateThese
      GetDefinedValue(lAttributeKey)                               'GetDefinedValue will try to calculate
    Next
  End Sub

  Public Sub DiscardCalculated()
    'discard any calculated attributes
    'Step in reverse so we can remove by index without high indexes changing before they are removed
    For iAttribute As Integer = Count - 1 To 0 Step -1
      If ItemByIndex(iAttribute).Definition.Calculated Then
        RemoveAt(iAttribute)
      End If
    Next
  End Sub

  Public Function GetDefinedValue(ByVal aAttributeName As String) As atcDefinedValue
    Dim lKey As String = AttributeNameToKey(aAttributeName)
    Dim lAttribute As atcDefinedValue = ItemByKey(lKey)

    If lAttribute Is Nothing Then  'Did not find the named attribute
      If Not Owner Is Nothing Then   'Need an owner to calculate an attribute
        Try
          Dim lOwnerTS As atcTimeseries = Owner
          Dim lDef As atcAttributeDefinition = pAllDefinitions.ItemByKey(lKey)
          Dim lOperation As atcDefinedValue
          If lDef.Calculated AndAlso IsSimple(lDef, lKey, lOperation) Then
            Dim lArg As atcDefinedValue = lOperation.Arguments.ItemByIndex(0)
            Dim lArgs As atcDataAttributes = lOperation.Arguments.Clone
            lArgs.SetValue(lArg.Definition, New atcDataGroup(lOwnerTS))
            lDef.Calculator.Open(lDef.Name, lArgs)
            lAttribute = ItemByKey(lKey)
          End If
        Catch CalcExcep As Exception
          Logger.Dbg("Exception calculating " & aAttributeName & ": " & CalcExcep.Message)
        End Try
      End If
    End If
    Return lAttribute
  End Function

  'True if the attribute defined by aDef is of a simple type (Double, Integer, Boolean, String)
  'and is not calculated or can be calculated from just one atcTimeseries
  'Optional aKey is the attribute key, passing it is allowed for performance
  'Optional aOperation will be set to the operation definition that calculates the attribute
  Public Shared Function IsSimple(ByVal aDef As atcAttributeDefinition, _
                  Optional ByVal aKey As String = Nothing, _
                  Optional ByRef aOperation As atcDefinedValue = Nothing) As Boolean
    Select Case aDef.TypeString.ToLower
      Case "double", "integer", "boolean", "string"
        If aDef.Calculated Then   'Maybe we can go ahead and calculate it now...
          If aKey Is Nothing Then aKey = AttributeNameToKey(aDef.Name)
          aOperation = aDef.Calculator.AvailableOperations.GetDefinedValue(aKey)
          If Not aOperation Is Nothing AndAlso Not aOperation.Arguments Is Nothing Then
            If aOperation.Arguments.Count = 1 Then 'Simple calculation has only one argument
              Dim lArg As atcDefinedValue = aOperation.Arguments.ItemByIndex(0)
              If lArg.Definition.TypeString = "atcTimeseries" Then 'Only argument must be atcTimeseries
                Return True
              End If
            End If
          End If
        Else
          Return True
        End If
    End Select
    Return False
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

  Public Overrides Function ToString() As String
    Dim lAttributes As SortedList = ValuesSortedByName()
    Dim lS As String = ""
    For i As Integer = 0 To lAttributes.Count - 1
      Dim lAttributeName As String = lAttributes.GetKey(i)
      lS &= lAttributeName & vbTab & GetFormattedValue(lAttributeName) & vbCrLf
    Next
    Return lS
  End Function

  Public Shadows Sub RemoveByKey(ByVal key As Object)
    MyBase.RemoveByKey(AttributeNameToKey(key))
  End Sub
End Class
