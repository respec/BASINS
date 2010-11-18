Imports atcUtility
Imports MapWinUtility

''' <summary>
'''     <para>
'''         Store attributes (and calculate some attributes if given an
'''         <see cref="atcData.atcTimeseries">atcTimeseries</see>)
'''     </para>
''' </summary>
''' <remarks>Attributes are stored as a collection of atcDefinedValue</remarks>
Public Class atcDataAttributes
    Inherits atcCollection

    Private pOwner As Object 'atcTimeseries
    Private Shared pAllAliases As atcCollection     'of String, so more than one AttributeName can map to the same attribute
    Private Shared pAllDefinitions As atcCollection 'of atcAttributeDefinition
    Private Shared pDateFormat As New atcDateFormat

    'Returns preferred alias of the given attribute name, or returns given attribute name unchanged if there is no preferred alias
    Public Shared Function PreferredName(ByRef aAttributeName As String) As String
        Dim lAlias As String = pAllAliases.ItemByKey(aAttributeName.ToLower)
        If lAlias IsNot Nothing Then 'We have a preferred alias for this name
            aAttributeName = lAlias
        ElseIf aAttributeName.Length = 6 _
            AndAlso (aAttributeName.Substring(0, 1).ToUpper = "H" OrElse aAttributeName.Substring(0, 1).ToUpper = "L") _
            AndAlso IsNumeric(aAttributeName.Substring(1)) Then
            If aAttributeName.Substring(0, 1).ToUpper = "L" Then                
                aAttributeName = CInt(aAttributeName.Substring(1, 2)) & "Low" & CInt(aAttributeName.Substring(3))
                If aAttributeName = "7Low10" Then aAttributeName = "7Q10"
            Else
                aAttributeName = CInt(aAttributeName.Substring(1, 2)) & "High" & CInt(aAttributeName.Substring(3))
            End If
        End If
        Return aAttributeName
    End Function

    ''' <summary>
    ''' Returns lowercase key for use in Me and pAllDefinitions
    ''' </summary>
    ''' <param name="aAttributeName"></param>
    ''' <returns>lowercase key</returns>
    ''' <remarks></remarks>
    Private Shared Function AttributeNameToKey(ByVal aAttributeName As String) As String
        Return PreferredName(aAttributeName).ToLower
    End Function

    Public Shared Function AddDefinition(ByVal aDefinition As atcAttributeDefinition) As atcAttributeDefinition
        Dim lKey As String = AttributeNameToKey(aDefinition.Name)
        Dim lAddDefinition As atcAttributeDefinition
        If Not pAllDefinitions.Keys.Contains(lKey) Then
            aDefinition.Name = PreferredName(aDefinition.Name)
            pAllDefinitions.Add(lKey, aDefinition)
            lAddDefinition = aDefinition
        ElseIf aDefinition.Calculated Then
            pAllDefinitions.ItemByKey(lKey) = aDefinition
            lAddDefinition = aDefinition
        Else
            lAddDefinition = pAllDefinitions.ItemByKey(lKey)
        End If
        Return lAddDefinition
    End Function

    ''' <summary>
    ''' Retrieve the atcAttributeDefinition for aAttributeName
    ''' </summary>
    ''' <param name="aAttributeName">Name of definition to return</param>
    ''' <param name="aCreate">If a definition by this name does not already exist, True creates a new definition, False returns Nothing</param>
    ''' <returns>AttributeDefinition</returns>
    ''' <remarks></remarks>
    Public Shared Function GetDefinition(ByVal aAttributeName As String, Optional ByVal aCreate As Boolean = False) As atcAttributeDefinition
        Dim lKey As String = AttributeNameToKey(aAttributeName)
        Dim lDef As atcAttributeDefinition = pAllDefinitions.ItemByKey(lKey)
        If lDef Is Nothing Then
            If lKey.StartsWith("%sum") Then
                lDef = pAllDefinitions.ItemByKey("%sum*")
            ElseIf lKey.StartsWith("%") Then
                lDef = pAllDefinitions.ItemByKey("%*")
            ElseIf lKey.Contains("low") Then
                If lKey.Contains("Variance of Estimate") Then
                    lDef = pAllDefinitions.ItemByKey("Variance of Estimate")
                ElseIf lKey.Contains("CI Lower") Then
                    lDef = pAllDefinitions.ItemByKey("CI Lower")
                ElseIf lKey.Contains("CI Upper") Then
                    lDef = pAllDefinitions.ItemByKey("CI Upper")
                ElseIf lKey.Contains("K Value") Then
                    lDef = pAllDefinitions.ItemByKey("K Value")
                Else
                    lDef = pAllDefinitions.ItemByKey("n-day low value")
                End If
            ElseIf lKey.Contains("high") Then
                lDef = pAllDefinitions.ItemByKey("n-day high value")
            End If
            If lDef IsNot Nothing Then            'Found a generic definition
                lDef = lDef.Clone(aAttributeName) 'Make a specific definition
            ElseIf aCreate Then
                lDef = New atcAttributeDefinition
                lDef.Name = aAttributeName
                pAllDefinitions.Add(lKey, lDef)
            End If
        End If
        Return lDef
    End Function

    'Returns collection of all known atcAttributeDefinition objects
    Public Shared Function AllDefinitions() As atcCollection
        Return pAllDefinitions
    End Function

    ''' <summary>
    ''' Append to the set of history items at the next available index, starting at 1
    ''' </summary>
    ''' <param name="aNewEvent">Description of what happened at this point in history</param>
    Public Sub AddHistory(ByVal aNewEvent As String)
        Dim lInsertAt As Integer = 1
        While ContainsAttribute("History " & lInsertAt)
            lInsertAt += 1
        End While
        SetValue("History " & lInsertAt, aNewEvent)
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
        Dim lSortedList As New SortedList(New CaseInsensitiveComparer)
        For Each lAdv As atcDefinedValue In Me
            lSortedList.Add(lAdv.Definition.Name, lAdv.Value)
        Next
        Return lSortedList
    End Function

    'True if aAttributeName has been set
    Public Function ContainsAttribute(ByVal aAttributeName As String) As Boolean
        Return Keys.Contains(AttributeNameToKey(aAttributeName))
    End Function

    Public Function GetFormattedValue(ByVal aAttributeName As String, Optional ByVal aDefault As Object = "") As String
        'TODO: use definition for formatting 
        Try
            Dim lValue As Object = aDefault
            Dim lTypeString As String = Nothing

            Try
                Dim tmpAttribute As atcDefinedValue
                tmpAttribute = GetDefinedValue(aAttributeName)
                If tmpAttribute IsNot Nothing Then
                    lValue = tmpAttribute.Value
                    lTypeString = tmpAttribute.Definition.TypeString
                Else
                    If aDefault Is Nothing Then 'search for default value in attribute definition
                        Dim lDef As atcAttributeDefinition = GetDefinition(aAttributeName)
                        If lDef IsNot Nothing Then
                            lValue = lDef.DefaultValue
                            lTypeString = lDef.TypeString
                        End If
                    End If
                End If
            Catch  'Could not find 
            End Try

            If lTypeString Is Nothing Then lTypeString = lValue.GetType.Name

            Try
                Select Case lTypeString
                    Case "Double"
FormatDouble:           Dim lAttName As String = aAttributeName.ToLower
                        If lAttName.Contains("jday") OrElse lAttName.Contains("date") Then
                            If IsNumeric(lValue) Then
                                Return pDateFormat.JDateToString(lValue)
                            Else
                                Return lValue
                            End If
                        Else
                            Return DoubleToString(lValue, 15)
                        End If
                    Case "Integer"
FormatInteger:          Return Format(CInt(lValue), "#,###;-#,###;0")
                    Case "atcTimeseries"
                        Return lValue.ToString
                    Case "atcDataGroup", "atcTimeseriesGroup"
                        Return lValue.ToString
                    Case "atcTimeUnit"
FormatTimeUnit:         Dim lTU As atcTimeUnit = lValue
                        Return lTU.ToString.Substring(2)
                    Case Else
                        If aAttributeName.ToLower.Contains("history") AndAlso lValue.ToString.ToLower.StartsWith("read from") Then 'make value shorter by removing path and "read "
                            Return "from " & FilenameNoPath(lValue.ToString.Substring(10))
                        ElseIf TypeOf (lValue) Is DateTime Then
                            Dim lDate As DateTime = lValue
                            Return pDateFormat.JDateToString(lDate.ToOADate)
                        ElseIf TypeOf (lValue) Is Double Then
                            GoTo FormatDouble
                        ElseIf TypeOf (lValue) Is Integer Then
                            GoTo FormatInteger
                        ElseIf TypeOf (lValue) Is atcTimeUnit Then
                            GoTo FormatTimeUnit
                        Else
                            Return lValue.ToString
                        End If
                End Select
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
        Dim lDefinedValue As atcDefinedValue
        Try
            lDefinedValue = GetDefinedValue(aAttributeName)
        Catch  'Could not find 

            'TODO: Try to calculate attribute?

            Return aDefault 'Not found and could not calculate
        End Try

        If lDefinedValue Is Nothing Then
            If aDefault Is Nothing Then
                Dim lDefinition As atcAttributeDefinition = GetDefinition(aAttributeName)
                If lDefinition Is Nothing Then
                    Return aDefault
                Else
                    Return lDefinition.DefaultValue
                End If
            Else
                Return aDefault
            End If
        Else
            Return lDefinedValue.Value
        End If
    End Function

    'Set attribute with definition aAttrDefinition to value aValue
    Public Function SetValue(ByVal aAttrDefinition As atcAttributeDefinition, ByVal aValue As Object, Optional ByVal aArguments As atcDataAttributes = Nothing) As Integer
        Dim lKey As String = AttributeNameToKey(aAttrDefinition.Name)
        Dim lDefinedValue As atcDefinedValue
        Dim lIndex As Integer = MyBase.Keys.IndexOf(lKey)
        If lIndex = -1 Then
            lDefinedValue = New atcDefinedValue
            lDefinedValue.Value = aValue
            If aArguments Is Nothing Then 'Add definition for attributes without arguments
                lDefinedValue.Definition = AddDefinition(aAttrDefinition)
            Else
                lDefinedValue.Arguments = aArguments
                If aArguments.GetValue("SeasonDefinition") Is Nothing Then
                    lDefinedValue.Definition = AddDefinition(aAttrDefinition) 'Add definition for attributes without season
                Else
                    lDefinedValue.Definition = aAttrDefinition
                End If
            End If
            lIndex = MyBase.Add(lKey, lDefinedValue)
        Else  'Update existing attribute value
            lDefinedValue = ItemByIndex(lIndex)
            lDefinedValue.Value = aValue
            If Not aArguments Is Nothing Then lDefinedValue.Arguments = aArguments
        End If
        Return lIndex
    End Function

    Public Sub SetValue(ByVal aAttributeName As String, ByVal aAttributeValue As Object)
        'todo: force a read of data here with EnsureValuesRead?
        Add(aAttributeName, aAttributeValue)
    End Sub

    Public Sub SetValueIfMissing(ByVal aAttributeName As String, ByVal aAttributeValue As Object)
        If ItemByKey(AttributeNameToKey(aAttributeName)) Is Nothing Then
            'Did not find the named attribute, add with supplied value
            Add(aAttributeName, aAttributeValue)
        End If
    End Sub

    'Set attribute with name aAttributeName to value aValue
    Public Shadows Function Add(ByVal aAttributeName As String, ByVal aAttributeValue As Object) As Integer
        Dim lTmpAttrDef As atcAttributeDefinition = GetDefinition(aAttributeName)
        If lTmpAttrDef Is Nothing Then
            lTmpAttrDef = New atcAttributeDefinition
            lTmpAttrDef.Name = aAttributeName
        End If
        Return SetValue(lTmpAttrDef, aAttributeValue)
    End Function

    Public Shadows Function Add(ByVal aDefinedValue As atcDefinedValue) As Integer
        If aDefinedValue Is Nothing Then
            Return -1
        Else
            Dim lKey As String = AttributeNameToKey(aDefinedValue.Definition.Name)
            MyBase.RemoveByKey(lKey)
            Return MyBase.Add(lKey, aDefinedValue)
        End If
    End Function

    Public Sub New() 'ByVal aTimeseries As atcTimeseries)
        MyBase.Clear()

        If pAllDefinitions Is Nothing Then
            pAllDefinitions = New atcCollection
            Dim lDef As New atcAttributeDefinition
            lDef.Name = "Units"
            lDef.TypeString = "String"
            lDef.CopiesInherit = True
            lDef.Editable = True
            'lUnitsDef.ValidList = GetAllUnitsInCategory("all")
            pAllDefinitions.Add(lDef.Name.ToLower, lDef)

            lDef = New atcAttributeDefinition
            lDef.Name = "ID"
            lDef.TypeString = "Integer"
            lDef.CopiesInherit = True
            lDef.Editable = False
            pAllDefinitions.Add(lDef.Name.ToLower, lDef)

            lDef = New atcAttributeDefinition
            lDef.Name = "Time Unit"
            lDef.TypeString = "atcTimeUnit"
            lDef.CopiesInherit = True
            lDef.Editable = False
            pAllDefinitions.Add(lDef.Name.ToLower, lDef)

            lDef = New atcAttributeDefinition
            lDef.Name = "Data Source"
            lDef.TypeString = "String"
            lDef.CopiesInherit = False
            lDef.Editable = False
            pAllDefinitions.Add(lDef.Name.ToLower, lDef)
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
                .Add("descrp", "Description")

                '.Add("stanam", "Station Name")  'Add this when WDM code can handle it - mhg

                .Add("long filename", "FileName")
                .Add("path", "FileName")

                .Add("ts", "Time Step")
                .Add("tu", "Time Unit")
                .Add("time steps", "Time Step")
                .Add("time units", "Time Unit")

                .Add("dsn", "ID")

                .Add("datcre", "Date Created")
                .Add("datmod", "Date Modified")

                .Add("sjday", "Start Date")
                .Add("ejday", "End Date")

                .Add("latdeg", "Latitude")
                .Add("lngdeg", "Longitude")
                .Add("elev", "Elevation")

                .Add("skewcf", "Skew")
                .Add("stddev", "Standard Deviation")
                .Add("meanvl", "Mean")
                .Add("minval", "Min")
                .Add("minimum", "Min")
                .Add("maxval", "Max")
                .Add("maximum", "Max")
                .Add("nonzro", "Count Positive")
                .Add("numzro", "Count Zero")

                .Add("7low10", "7Q10")
                .Add("datasource", "Data Source")
            End With
        End If
    End Sub

    Public Shadows Sub ChangeTo(ByVal aNewItems As atcDataAttributes)
        Clear()
        For Each lAdv As atcDefinedValue In aNewItems
            SetValue(lAdv.Definition, lAdv.Value, lAdv.Arguments)
        Next
    End Sub

    Public Shadows Function Clone() As atcDataAttributes
        Dim newClone As New atcDataAttributes
        For Each lAdv As atcDefinedValue In Me
            If lAdv.Definition.CopiesInherit Then
                newClone.SetValue(lAdv.Definition, lAdv.Value, lAdv.Arguments)
            End If
        Next
        Return newClone
    End Function

    'Calculate all the known attributes that can be calculated with no additional arguments
    Public Sub CalculateAll()
        Dim lCalculateThese As New ArrayList
        For Each lDef As atcAttributeDefinition In pAllDefinitions 'For each kind of attribute we know about
            If lDef.Calculated Then                                  'This attribute can be calculated
                Dim key As String = AttributeNameToKey((lDef.Name))
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
        Dim lAttribute As atcDefinedValue = Nothing
        Try
            Dim lKey As String = AttributeNameToKey(aAttributeName)
            lAttribute = ItemByKey(lKey)

            If lAttribute Is Nothing Then  'Did not find the named attribute
                If Not Owner Is Nothing Then   'Need an owner to calculate an attribute
                    Try
                        Dim lDef As atcAttributeDefinition = GetDefinition(aAttributeName)
                        If Not lDef Is Nothing Then
                            Dim lOperation As atcDefinedValue = Nothing
                            If lDef.Calculated Then
                                If lDef.Calculator.Name.Contains("n-day") Then
                                    lOperation = lDef.Calculator.AvailableOperations.ItemByKey(lDef.Name)
                                    Dim lArgs As New atcDataAttributes
                                    lArgs.SetValue("Timeseries", New atcTimeseriesGroup(CType(Owner, atcTimeseries)))
                                    lDef.Calculator.Open(lKey, lArgs)
                                    lAttribute = ItemByKey(lKey)
                                ElseIf IsSimple(lDef, lKey, lOperation) Then
                                    Dim lArg As atcDefinedValue = lOperation.Arguments.ItemByIndex(0)
                                    Dim lArgs As atcDataAttributes = lOperation.Arguments.Clone
                                    lArgs.SetValue(lArg.Definition, New atcTimeseriesGroup(CType(Owner, atcTimeseries)))
                                    lDef.Calculator.Open(lKey, lArgs)
                                    lAttribute = ItemByKey(lKey)
                                End If
                            End If
                        End If
                    Catch NullExcep As NullReferenceException
                        'Ignore these
                    Catch CalcExcep As Exception
                        Logger.Dbg("Exception calculating " & aAttributeName & ": " & CalcExcep.Message)
                    End Try
                End If
            End If
        Catch e As Exception
            Logger.Dbg("GetDefinedValue(" & aAttributeName & ") Exception: " & e.Message)
        End Try
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
            Case "single", "double", "integer", "boolean", "string", "atctimeunit", "atccollection"
                If aDef.Calculated Then   'Maybe we can go ahead and calculate it now...
                    If aKey Is Nothing Then aKey = AttributeNameToKey((aDef.Name))
                    aOperation = aDef.Calculator.AvailableOperations.ItemByKey(aKey)

                    If aOperation Is Nothing AndAlso aKey.StartsWith("%") Then
                        aOperation = aDef.Calculator.AvailableOperations.ItemByKey("%*")
                    End If

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
            If MyBase.Keys.Count > index Then
                Return MyBase.Item(index)
            Else
                Logger.Dbg("Index " & index & " outOfRange (0," & MyBase.Keys.Count - 1 & ")")
                Return Nothing
            End If
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
