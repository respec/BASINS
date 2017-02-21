Imports atcUtility
Imports MapWinUtility

''' <summary>
'''     <para>
'''         Store attributes (and calculate some attributes if given an <see cref="atcData.atcTimeseries">atcTimeseries</see>)
'''     </para>
''' </summary>
''' <remarks>Attributes are stored as a collection of atcDefinedValue</remarks>
Public Class atcDataAttributes
    Implements Generic.IEnumerable(Of atcDefinedValue)

    ''' <summary>
    ''' Optional storage of attributes that are shared with other data sets, Nothing if not in use.
    ''' </summary>
    Public SharedAttributes As atcDataAttributes = Nothing

    ''' <summary>
    ''' Attributes are stored here, keyed by name
    ''' </summary>
    Private pAttributes As New atcCollection

    Private pOwner As Object 'atcTimeseries

    ''' <summary>Attribute Name Aliases</summary>
    Private Shared pAllAliases As Generic.SortedList(Of String, String) = InitAliases()

    ''' <summary>All of the atcAttributeDefinitions in use</summary>
    Private Shared pAllDefinitions As atcCollection = InitDefinitions()

    ''' <summary>Format to use for dates in GetFormattedValue</summary>
    Private Shared pDateFormat As New atcDateFormat

    ''' <summary>Get the preferred alias of the given attribute name</summary>
    ''' <param name="aAttributeName">name to search for an alias of</param>
    ''' <returns>preferred alias</returns>
    ''' <remarks>returns given attribute name unchanged if there is no preferred alias</remarks>
    Public Shared Function PreferredName(ByRef aAttributeName As String) As String
        If String.IsNullOrEmpty(aAttributeName) Then
            aAttributeName = "Nothing"
        End If
        Dim lNameLower As String = aAttributeName.ToLower
        Dim lAlias As String = Nothing
        If pAllAliases.TryGetValue(lNameLower, lAlias) Then 'We have a preferred alias for this name
            aAttributeName = lAlias
        ElseIf aAttributeName.StartsWith("%") Then
            Dim lPercentString As String = aAttributeName.Substring(1)
            If IsNumeric(lPercentString) Then
                aAttributeName = "%" & Format(CDbl(lPercentString), "00.####")
            End If
        Else
            If aAttributeName.Length = 6 Then 'Check for high/low attribute names
                Dim lFirstChar As String = aAttributeName.Substring(0, 1).ToUpper
                If (lFirstChar = "H" OrElse lFirstChar = "L") _
                   AndAlso IsNumeric(aAttributeName.Substring(1)) Then
                    If lFirstChar = "L" Then
                        aAttributeName = CInt(aAttributeName.Substring(1, 2)) & "Low" & CInt(aAttributeName.Substring(3))
                        If aAttributeName = "7Low10" Then aAttributeName = "7Q10"
                    Else
                        aAttributeName = CInt(aAttributeName.Substring(1, 2)) & "High" & CInt(aAttributeName.Substring(3))
                    End If
                End If
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
        Dim lAddDefinition As atcAttributeDefinition = Nothing
        If aDefinition IsNot Nothing Then
            Dim lKey As String = AttributeNameToKey(aDefinition.Name)
            If Not pAllDefinitions.Keys.Contains(lKey) Then
                aDefinition.Name = PreferredName(aDefinition.Name)
                pAllDefinitions.Add(lKey, aDefinition)
                'Logger.Dbg("New attribute: " & aDefinition.Name)
                lAddDefinition = aDefinition
            ElseIf aDefinition.Calculated Then
                pAllDefinitions.ItemByKey(lKey) = aDefinition
                lAddDefinition = aDefinition
            Else
                lAddDefinition = pAllDefinitions.ItemByKey(lKey)
            End If
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
                ElseIf IsNumeric(lKey.Substring(lKey.IndexOf("low") + 3)) Then
                    lDef = pAllDefinitions.ItemByKey("n-day low value")
                Else
                    lDef = pAllDefinitions.ItemByKey("n-day low attribute")
                End If
            ElseIf lKey.Contains("high") Then
                If IsNumeric(lKey.Substring(lKey.IndexOf("high") + 4)) Then
                    lDef = pAllDefinitions.ItemByKey("n-day high value")
                Else
                    lDef = pAllDefinitions.ItemByKey("n-day high attribute")
                End If
            End If
            If lDef IsNot Nothing Then            'Found a generic definition
                lDef = lDef.Clone(aAttributeName) 'Make a specific definition
            ElseIf aCreate Then
                lDef = New atcAttributeDefinition
                lDef.Name = PreferredName(aAttributeName)
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

    ''' <summary>
    ''' The names (as keys) and values of all attributes that are set. (sorted by name)
    ''' </summary>
    Public Function ValuesSortedByName() As SortedList
        Dim lSortedList As New SortedList(New CaseInsensitiveComparer)
        For Each lAdv As atcDefinedValue In pAttributes
            lSortedList.Add(lAdv.Definition.Name, lAdv.Value)
        Next
        If SharedAttributes IsNot Nothing Then
            For Each lSharedAtt As atcDefinedValue In SharedAttributes
                If Not lSortedList.ContainsKey(lSharedAtt.Definition.Name) Then
                    lSortedList.Add(lSharedAtt.Definition.Name, lSharedAtt.Value)
                End If
            Next
        End If
        Return lSortedList
    End Function

    'True if aAttributeName has been set
    Public Function ContainsAttribute(ByVal aAttributeName As String) As Boolean
        Dim lKey As String = AttributeNameToKey(aAttributeName)
        Return pAttributes.Keys.Contains(lKey) OrElse _
            SharedAttributes IsNot Nothing AndAlso SharedAttributes.ContainsAttribute(lKey)
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
                                Try
                                    Return pDateFormat.JDateToString(lValue)
                                Catch
                                End Try
                            End If
                            Return lValue
                        Else
                            Return DoubleToString(lValue, 15)
                        End If
                    Case "Integer"
FormatInteger:
                        Select Case aAttributeName.ToUpper
                            Case "ID", "TSBYR", "YEAR" 'Do not format as number with comma separating thousands
                                Return CStr(lValue)
                        End Select
                        Return Format(CInt(lValue), "#,###;-#,###;0")
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
        Dim lDefinedValue As atcDefinedValue = Nothing
        Try
            lDefinedValue = GetDefinedValue(aAttributeName)
        Catch  'Could not find 
            Return aDefault
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
        Dim lIndex As Integer = pAttributes.Keys.IndexOf(lKey)
        If lIndex = -1 Then 'Only check pAttributes when setting, do not set any SharedAttributes here.
            lDefinedValue = New atcDefinedValue
            lDefinedValue.Value = aValue
            If aArguments Is Nothing Then 'Add definition for attributes without arguments
                lDefinedValue.Definition = AddDefinition(aAttrDefinition)
            Else
                lDefinedValue.Arguments = aArguments
                If Not aArguments.ContainsAttribute("SeasonDefinition") Then
                    lDefinedValue.Definition = AddDefinition(aAttrDefinition) 'Add definition for attributes without season
                Else
                    lDefinedValue.Definition = aAttrDefinition
                End If
            End If

            Dim lKeyIndex As Integer = pAllDefinitions.Keys.IndexOf(lKey)
            If lKeyIndex >= 0 Then 'Re-use existing key to save memory
                lKey = pAllDefinitions.Keys(lKeyIndex)
            End If
            lIndex = pAttributes.Add(lKey, lDefinedValue)

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
        If Not ContainsAttribute(aAttributeName) Then
            Add(aAttributeName, aAttributeValue)
        End If
    End Sub

    ''' <summary>
    ''' Set attribute with name aAttributeName to value aValue
    ''' </summary>
    ''' <param name="aAttributeName">Name of attribute to set</param>
    ''' <param name="aAttributeValue">Value of attribute</param>
    ''' <returns>Index of attribute</returns>
    ''' <remarks>Creates a new atcAttributeDefinition if needed</remarks>
    Public Shadows Function Add(ByVal aAttributeName As String, ByVal aAttributeValue As Object) As Integer
        Return SetValue(GetDefinition(aAttributeName, True), aAttributeValue)
    End Function

    Public Shadows Function Add(ByVal aDefinedValue As atcDefinedValue) As Integer
        If aDefinedValue Is Nothing Then
            Return -1
        Else
            Dim lKey As String = AttributeNameToKey(aDefinedValue.Definition.Name)
            pAttributes.RemoveByKey(lKey)
            Return pAttributes.Add(lKey, aDefinedValue)
        End If
    End Function

    Private Shared Function InitDefinitions() As atcCollection
        Dim lDefinitions As New atcCollection
        Dim lDef As New atcAttributeDefinition
        lDef.Name = "Units"
        lDef.TypeString = "String"
        lDef.CopiesInherit = True
        lDef.Editable = True
        'lUnitsDef.ValidList = GetAllUnitsInCategory("all")
        lDefinitions.Add(lDef.Name.ToLower, lDef)

        lDef = New atcAttributeDefinition
        lDef.Name = "ID"
        lDef.TypeString = "Integer"
        lDef.CopiesInherit = True
        lDef.Editable = False
        lDefinitions.Add(lDef.Name.ToLower, lDef)

        lDef = New atcAttributeDefinition
        lDef.Name = "Time Unit"
        lDef.TypeString = "atcTimeUnit"
        lDef.CopiesInherit = True
        lDef.Editable = False
        lDefinitions.Add(lDef.Name.ToLower, lDef)

        lDef = New atcAttributeDefinition
        lDef.Name = "Time Step"
        lDef.TypeString = "Integer"
        lDef.CopiesInherit = True
        lDef.Editable = False
        lDefinitions.Add(lDef.Name.ToLower, lDef)

        lDef = New atcAttributeDefinition
        lDef.Name = "Data Source"
        lDef.TypeString = "String"
        lDef.CopiesInherit = False
        lDef.Editable = False
        lDefinitions.Add(lDef.Name.ToLower, lDef)

        Return lDefinitions
    End Function

    Private Shared Function InitAliases() As Generic.SortedList(Of String, String)
        Dim lAliases As New Generic.SortedList(Of String, String) 'of alias and internal name
        With lAliases
            .Add("sen", "Scenario")
            .Add("scen", "Scenario")
            .Add("idscen", "Scenario")

            .Add("loc", "Location")
            .Add("locn", "Location")
            .Add("idlocn", "Location")
            .Add("site_no", "STAID")

            .Add("con", "Constituent")
            .Add("cons", "Constituent")
            .Add("idcons", "Constituent")

            .Add("desc", "Description")
            .Add("descrp", "Description")

            '.Add("stanam", "Station Name")  'Add this when WDM code can handle it - mhg
            .Add("station_nm", "StaNam")

            .Add("long filename", "FileName")
            .Add("path", "FileName")

            .Add("ts", "Time Step")
            .Add("tsstep", "Time Step")
            .Add("timestep", "Time Step")
            .Add("timesteps", "Time Step")
            .Add("time steps", "Time Step")

            .Add("tu", "Time Unit")
            .Add("tcode", "Time Unit")
            .Add("timeunit", "Time Unit")
            .Add("timeunits", "Time Unit")
            .Add("time units", "Time Unit")

            .Add("dsn", "ID")

            .Add("datcre", "Date Created")
            .Add("datmod", "Date Modified")

            .Add("sjday", "Start Date")
            .Add("ejday", "End Date")

            .Add("latdeg", "Latitude")
            .Add("lngdeg", "Longitude")
            .Add("dec_lat_va", "Latitude")
            .Add("dec_long_va", "Longitude")
            .Add("elev", "Elevation")
            .Add("alt_va", "Elevation")

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
            .Add("darea", "Drainage Area")
            .Add("drain_area_va", "Drainage Area")
            .Add("agency_cd", "AGENCY")
            .Add("state_cd", "STFIPS")
            .Add("county_cd", "CNTYFIPS")
            .Add("huc_cd", "HUCODE")

            'Also add lowercase version of all preferred names as keys
            Dim lAllPreferredNames As New Generic.SortedList(Of String, String)
            For Each lPreferredName As String In lAliases.Values
                Dim lLowerName As String = lPreferredName.ToLower
                If Not lAllPreferredNames.ContainsKey(lLowerName) Then
                    lAllPreferredNames.Add(lLowerName, lPreferredName)
                End If
            Next
            For Each lPreferredKey As String In lAllPreferredNames.Keys
                If Not .ContainsKey(lPreferredKey) Then
                    .Add(lPreferredKey, lAllPreferredNames.Item(lPreferredKey))
                End If
            Next
        End With
        Return lAliases
    End Function

    Public Sub New() 'ByVal aTimeseries As atcTimeseries)
        pAttributes.Clear()
    End Sub

    ''' <summary>
    ''' Discard existing attributes and add the ones in aNewItems instead
    ''' </summary>
    ''' <param name="aNewItems"></param>
    Public Shadows Sub ChangeTo(ByVal aNewItems As atcDataAttributes)
        Clear()
        For Each lAdv As atcDefinedValue In aNewItems
            SetValue(lAdv.Definition, lAdv.Value, lAdv.Arguments)
        Next
        SharedAttributes = Nothing
    End Sub

    ''' <summary>
    ''' Create a copy containing all attributes even if Definition.CopiesInherit = False
    ''' </summary>
    Public Shadows Function Clone() As atcDataAttributes
        Dim newClone As New atcDataAttributes
        For Each lAdv As atcDefinedValue In Me
            newClone.SetValue(lAdv.Definition, lAdv.Value, lAdv.Arguments)
        Next
        Return newClone
    End Function

    ''' <summary>
    ''' Create a copy containing the attributes whose Definition.CopiesInherit = True
    ''' </summary>
    Public Function Copy() As atcDataAttributes
        Dim newClone As New atcDataAttributes
        For Each lAdv As atcDefinedValue In Me
            If lAdv.Definition.CopiesInherit Then
                newClone.SetValue(lAdv.Definition, lAdv.Value, lAdv.Arguments)
            End If
        Next
        Return newClone
    End Function

    ''' <summary>
    ''' Calculate all the known attributes that can be calculated with no additional arguments
    ''' </summary>
    Public Sub CalculateAll()
        Dim lCalculateThese As New Generic.List(Of String)
        For Each lDef As atcAttributeDefinition In pAllDefinitions 'For each kind of attribute we know about
            If lDef.Calculated Then                                  'This attribute can be calculated
                If Not ContainsAttribute(lDef.Name) Then               'We do not yet have a value for this attribute
                    lCalculateThese.Add(lDef.Name)
                End If
            End If
        Next

        'Had to separate calculation from enumerating pAllDefinitions since definitions may get added
        For Each lAttributeKey As String In lCalculateThese
            GetDefinedValue(lAttributeKey)                               'GetDefinedValue will try to calculate
        Next
    End Sub

    ''' <summary>
    ''' Discard any calculated attributes. Useful after changing values.
    ''' </summary>
    ''' <remarks>Does not affect SharedAttributes.</remarks>
    Public Sub DiscardCalculated()
        'Step in reverse so we can remove by index without high indexes changing before they are removed
        For iAttribute As Integer = pAttributes.Count - 1 To 0 Step -1
            If ItemByIndex(iAttribute).Definition.Calculated Then
                pAttributes.RemoveAt(iAttribute)
            End If
        Next
    End Sub

    Public Function GetDefinedValue(ByVal aAttributeName As String) As atcDefinedValue
        Dim lAttribute As atcDefinedValue = Nothing
        Try
            Dim lKey As String = aAttributeName.ToLower
            lAttribute = pAttributes.ItemByKey(lKey)
            If lAttribute Is Nothing Then
                lKey = AttributeNameToKey(aAttributeName)
                lAttribute = pAttributes.ItemByKey(lKey)
            End If

            If lAttribute Is Nothing Then  'Did not find the named attribute
                If SharedAttributes IsNot Nothing Then
                    lAttribute = SharedAttributes.GetDefinedValue(lKey)
                End If
                If lAttribute Is Nothing Then  'Did not find the attribute as shared either
                    If Not Owner Is Nothing Then   'Need an owner to calculate an attribute
                        Try
                            Dim lDef As atcAttributeDefinition = GetDefinition(aAttributeName)
                            If Not lDef Is Nothing Then
                                Dim lOperation As atcDefinedValue = Nothing
                                If lDef.Calculated Then
                                    If lDef.Calculator.Name.Contains("n-day") Then
                                        lOperation = lDef.Calculator.AvailableOperations.GetDefinedValue(lDef.Name)
                                        Dim lArgs As New atcDataAttributes
                                        lArgs.SetValue("Timeseries", New atcTimeseriesGroup(CType(Owner, atcTimeseries)))
                                        lDef.Calculator.Open(lKey, lArgs)
                                        lAttribute = pAttributes.ItemByKey(lKey)
                                    ElseIf IsSimple(lDef, lKey, lOperation) Then
                                        Dim lArg As atcDefinedValue = lOperation.Arguments.ItemByIndex(0)
                                        Dim lArgs As atcDataAttributes = lOperation.Arguments.Clone
                                        lArgs.SetValue(lArg.Definition, New atcTimeseriesGroup(CType(Owner, atcTimeseries)))
                                        lDef.Calculator.Open(lKey, lArgs)
                                        lAttribute = pAttributes.ItemByKey(lKey)
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
            End If
        Catch e As Exception
            Logger.Dbg("GetDefinedValue(" & aAttributeName & ") Exception: " & e.Message)
        End Try
        Return lAttribute
    End Function

    ''' <summary>
    ''' True if the attribute defined by aDef is of a simple type (Single, Double, Integer, Boolean, String, atcTimeUnit, atcCollection)
    ''' and is not calculated or can be calculated from just one atcTimeseries
    ''' </summary>
    ''' <param name="aDef">Definition to test for simplicity</param>
    ''' <param name="aKey">Optional aKey is the attribute key, passing it is allowed for performance</param>
    ''' <param name="aOperation">Optional aOperation will be set to the operation definition that calculates the attribute</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function IsSimple(ByVal aDef As atcAttributeDefinition, _
                           Optional ByVal aKey As String = Nothing, _
                           Optional ByRef aOperation As atcDefinedValue = Nothing) As Boolean
        Select Case aDef.TypeString.ToLower
            Case "single", "double", "integer", "boolean", "string", "atctimeunit", "atccollection"
                If aDef.Calculated Then
                    If aKey Is Nothing Then aKey = AttributeNameToKey((aDef.Name))
                    aOperation = aDef.Calculator.AvailableOperations.GetDefinedValue(aKey)

                    If aOperation Is Nothing AndAlso aKey.StartsWith("%") Then
                        aOperation = aDef.Calculator.AvailableOperations.GetDefinedValue("%*")
                    End If

                    If aOperation IsNot Nothing AndAlso aOperation.Arguments IsNot Nothing Then
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

    Public Function ItemByIndex(ByVal aIndex As Integer) As atcDefinedValue
        If aIndex >= 0 Then
            If aIndex < pAttributes.Count Then
                Return pAttributes.ItemByIndex(aIndex)
            ElseIf SharedAttributes IsNot Nothing Then
                Return SharedAttributes.ItemByIndex(aIndex - pAttributes.Count)
            End If
        End If
        Throw New InvalidOperationException(aIndex & " out of range 0.." & Count() - 1)
    End Function

    Default Public Shadows Property Item(ByVal aIndex As Integer) As atcDefinedValue
        Get
            Return ItemByIndex(aIndex)
        End Get
        Set(ByVal newValue As atcDefinedValue)
            If aIndex < 0 Then
                Throw New InvalidOperationException(aIndex & " out of range 0.." & Count() - 1)
            ElseIf aIndex < pAttributes.Count Then
                pAttributes.Item(aIndex) = newValue
            ElseIf SharedAttributes IsNot Nothing Then
                SharedAttributes.Item(aIndex - pAttributes.Count) = newValue
            Else
                Throw New InvalidOperationException(aIndex & " out of range 0.." & Count() - 1)
            End If
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

    Public Sub Remove(ByVal aAttribute As atcDefinedValue)
        pAttributes.Remove(aAttribute)
    End Sub

    Public Shadows Sub RemoveByKey(ByVal aAttributeName As Object)
        pAttributes.RemoveByKey(AttributeNameToKey(aAttributeName))
    End Sub

    Public Sub Clear()
        SharedAttributes = Nothing
        pAttributes.Clear()
    End Sub

    Public Function Count() As Integer
        If SharedAttributes Is Nothing Then
            Return pAttributes.Count
        Else
            Return pAttributes.Count + SharedAttributes.Count
        End If
    End Function

    Public Function GetEnumerator() As System.Collections.Generic.IEnumerator(Of atcDefinedValue) Implements System.Collections.Generic.IEnumerable(Of atcDefinedValue).GetEnumerator
        Return New AttributesEnumerator(Me)
    End Function
    Private Function GetEnumerator1() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
        Return GetEnumerator()
    End Function

    Private Class AttributesEnumerator
        Implements Generic.IEnumerator(Of atcDefinedValue)

        Dim pAttributes As atcDataAttributes
        Dim pCurrentIndex As Integer = -1
        Dim pLastIndex As Integer

        Public Sub New(ByVal aAttributes As atcDataAttributes)
            pAttributes = aAttributes
            pLastIndex = pAttributes.Count - 1
        End Sub

        Public ReadOnly Property Current As atcDefinedValue Implements System.Collections.Generic.IEnumerator(Of atcDefinedValue).Current
            Get
                Return pAttributes.ItemByIndex(pCurrentIndex)
            End Get
        End Property
        Private ReadOnly Property Current1 As Object Implements System.Collections.IEnumerator.Current
            Get
                Return Me.Current            ' Just return the generic property value
            End Get
        End Property

        Public Function MoveNext() As Boolean Implements System.Collections.IEnumerator.MoveNext
            If pCurrentIndex < pLastIndex Then
                pCurrentIndex += 1
                Return True
            Else
                Return False
            End If
        End Function

        Public Sub Reset() Implements System.Collections.IEnumerator.Reset
            pCurrentIndex = -1
        End Sub

        Private Sub Dispose() Implements IDisposable.Dispose
            'We do not need to dispose anything, but need to implement Dispose
        End Sub
    End Class
End Class