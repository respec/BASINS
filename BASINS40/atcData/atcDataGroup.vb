Imports atcUtility
Imports MapWinUtility

''' <summary>Group of atcDataSet objects and associated selection information</summary>
''' <remarks>
'''     <para>Sharable between different views of the same data.</para>
'''     <para>
'''         <br />
'''         Events are defined to allow different
'''         <see cref="atcData.atcDataDisplay">displays</see> to remain
'''         synchronized
'''     </para>
''' </remarks>
Public Class atcDataGroup
    Inherits atcCollection

    Private Shared pNaN As Double = GetNaN()

    Private pSelectedData As atcDataGroup 'tracks currently selected group within this group

    ''' <summary>One or more <see cref="atcData.atcDataSet">atcDataSet</see> were just added</summary>
    Public Event Added(ByVal aAdded As atcCollection)

    ''' <summary>One or more <see cref="atcData.atcDataSet">atcDataSet</see> were just removed</summary>
    Public Event Removed(ByVal aRemoved As atcCollection)

    Private Sub RaiseAddedOne(ByVal aDataSet As atcDataSet)
        Dim lDataSets As New atcCollection
        lDataSets.Add(aDataSet)
        RaiseEvent Added(lDataSets)
    End Sub

    Private Sub RaiseRemovedOne(ByVal aDataSet As atcDataSet)
        Dim lDataSets As New atcCollection
        lDataSets.Add(aDataSet)
        RaiseEvent Removed(lDataSets)
    End Sub

    ''' <summary>atcDataSet by index</summary>
    Default Public Shadows Property Item(ByVal aIndex As Integer) As atcDataSet
        Get
            Return MyBase.Item(aIndex)
        End Get
        Set(ByVal newValue As atcDataSet)
            MyBase.Item(aIndex) = newValue
        End Set
    End Property

    ''' <summary>atcDataSet by index</summary>
    Public Shadows Property ItemByIndex(ByVal aIndex As Integer) As atcDataSet
        Get
            Return MyBase.Item(aIndex)
        End Get
        Set(ByVal newValue As atcDataSet)
            MyBase.Item(aIndex) = newValue
        End Set
    End Property

    ''' <summary>atcDataSet by key</summary>
    Public Shadows Property ItemByKey(ByVal aKey As Object) As atcDataSet
        Get
            Return MyBase.ItemByKey(aKey)
        End Get
        Set(ByVal newValue As atcDataSet)
            MyBase.ItemByKey(aKey) = newValue
        End Set
    End Property

    ''' <summary>Create a new empty data group</summary>
    Public Sub New()
        MyBase.New()
    End Sub

    ''' <summary>Create a new data group and add a dataset
    ''' to the group with the default key of its serial number</summary>
    Public Sub New(ByVal aDataSet As atcDataSet)
        MyBase.New()
        Add(aDataSet.Serial, aDataSet)
    End Sub

    ''' <summary>Add an <see cref="atcData.atcDataSet">atcDataSet</see> 
    ''' to the group with the default key of its serial number </summary>
    Public Shadows Function Add(ByVal aDataSet As atcDataSet) As Integer
        Add(aDataSet.Serial, aDataSet)
    End Function

    ''' <summary>Add a dataset to the group with the key specified</summary>
    Public Shadows Function Add(ByVal aKey As Object, _
                                ByVal aDataSet As atcDataSet) As Integer
        MyBase.Add(aKey, aDataSet)
        RaiseAddedOne(aDataSet)
    End Function

    ''' <summary>Add items from an atcCollection or atcDataGroup to the group.</summary>
    Public Shadows Sub Add(ByVal aAddThese As atcCollection)
        Dim lLastIndex As Integer = aAddThese.Count - 1
        For lIndex As Integer = 0 To lLastIndex
            MyBase.Add(aAddThese.Keys(lIndex), aAddThese.ItemByIndex(lIndex))
        Next
        RaiseEvent Added(aAddThese)
    End Sub

    ''' <summary>Remove all datasets and selections from this data group.</summary>
    Public Shadows Sub Clear()
        If Not pSelectedData Is Nothing Then pSelectedData.Clear()
        If Count > 0 Then
            Dim lRemoved As atcDataGroup = Me.Clone
            MyBase.Clear()
            RaiseEvent Removed(lRemoved)
        End If
    End Sub

    Public Overrides Sub Dispose()
        If Not pSelectedData Is Nothing Then pSelectedData.Clear()
        MyBase.Dispose()
    End Sub

    ''' <summary>Create a copy of this data group</summary>
    Public Shadows Function Clone() As atcDataGroup
        Dim newClone As New atcDataGroup
        For index As Integer = 0 To MyBase.Count - 1
            newClone.Add(MyBase.Keys(index), MyBase.Item(index))
        Next
        Return newClone
    End Function

    ''' <summary>Change this group to match the new group
    ''' and raise the appropriate events.
    ''' </summary>
    Public Shadows Sub ChangeTo(ByVal aNewGroup As atcDataGroup)
        If aNewGroup Is Nothing Then
            Clear()
        Else
            Dim RemoveList As New atcCollection
            For Each oldTS As atcDataSet In Me
                If Not aNewGroup.Contains(oldTS) Then
                    RemoveList.Add(oldTS)
                End If
            Next

            Dim AddList As New atcCollection
            For Each savedTS As atcDataSet In aNewGroup
                If Not Contains(savedTS) Then
                    AddList.Add(savedTS)
                End If
            Next

            MyBase.ChangeTo(aNewGroup)
            If RemoveList.Count > 0 Then RaiseEvent Removed(RemoveList)
            If AddList.Count > 0 Then RaiseEvent Added(AddList)
        End If
    End Sub

    ''' <summary>Determines index of dataset specified by aSerial</summary>
    Public Function IndexOfSerial(ByVal aSerial As Integer) As Integer
        For iTS As Integer = 0 To Count - 1
            If Item(iTS).Serial = aSerial Then Return iTS
        Next
        Return -1
    End Function

    ''' <summary>Insert a new dataset at the specified index</summary>
    Public Shadows Sub Insert(ByVal aIndex As Integer, _
                              ByVal aDataSet As atcDataSet)
        MyBase.Insert(aIndex, aDataSet)
        RaiseAddedOne(aDataSet)
    End Sub

    ''' <summary>Remove dataset specified by aIndex from this group.</summary>
    ''' <remarks>
    '''     Cannot just Remove(ItemByIndex(index))
    '''     because this overriding RemoveAt is called by MyBase.Remove--infinite loop
    ''' </remarks>
    Public Shadows Sub RemoveAt(ByVal aIndex As Integer)
        Dim lDataSet As atcDataSet = ItemByIndex(aIndex)
        MyBase.RemoveAt(aIndex)
        RaiseRemovedOne(lDataSet)
    End Sub

    ''' <summary>Remove an <see cref="atcData.atcDataSet">atcDataSet</see> from this 
    ''' group.</summary>
    Public Shadows Sub Remove(ByVal aDataSet As atcDataSet)
        MyBase.Remove(aDataSet)
        RaiseRemovedOne(aDataSet)
    End Sub

    ''' <summary>Remove a set of datasets from this group.</summary>
    Public Shadows Sub Remove(ByVal aRemoveThese As atcCollection)
        If Not aRemoveThese Is Nothing AndAlso aRemoveThese.Count > 0 Then
            For Each ts As atcDataSet In aRemoveThese
                MyBase.Remove(ts)
            Next
            RaiseEvent Removed(aRemoveThese)
        End If
    End Sub

    ''' <summary>Remove a span of one or more DataSets from the group by index.</summary>
    Public Shadows Sub RemoveRange(ByVal aIndex As Integer, ByVal aNumber As Integer)
        Dim lRemoveThese As New atcCollection
        For index As Integer = aIndex To aIndex + aNumber - 1
            lRemoveThese.Add(Item(index))
        Next
        Remove(lRemoveThese)
    End Sub

    ''' <summary>
    ''' Return a subset of this atcDataGroup containing only the atcDataSets where the named attribute has the given value
    ''' </summary>
    ''' <param name="aAttributeName">Name of Attribute to check</param>
    ''' <param name="aValue">Value that given attribute must have to include dataset in group returned</param>
    ''' <param name="aLimit">Optional limit of how many data sets to return, default of 0 means there is no limit</param>
    ''' <remarks>search for value is not case sensitive</remarks>
    Public Function FindData(ByVal aAttributeName As String, ByVal aValue As String, Optional ByVal aLimit As Integer = 0) As atcDataGroup
        Dim lMatch As New atcDataGroup
        Dim lDataset As atcDataSet
        Try
            Dim lValue As String = aValue.ToLower
            For lIndex As Integer = 0 To Me.Count - 1
                lDataset = Me.ItemByIndex(lIndex)
                If CStr(lDataset.Attributes.GetValue(aAttributeName)).ToLower = lValue Then
                    lMatch.Add(Me.Keys(lIndex), lDataset)
                    If lMatch.Count = aLimit Then
                        Exit For 'Reached requested limit, stop looking for more
                    End If
                End If
            Next
        Catch
        End Try
        Return lMatch
    End Function

    ''' <summary>
    ''' Return a subset of this atcDataGroup containing only the atcDataSets where the named attribute has one of the given values
    ''' </summary>
    ''' <param name="aAttributeName">Name of Attribute to check</param>
    ''' <param name="aValues">Acceptable values for the given attribute to include dataset in group returned</param>
    ''' <param name="aLimit">Optional limit of how many data sets to return, default of 0 means there is no limit</param>
    ''' <remarks>search for value is not case sensitive</remarks>
    Public Function FindData(ByVal aAttributeName As String, ByVal aValues As atcCollection, Optional ByVal aLimit As Integer = 0) As atcDataGroup
        Dim lMatch As New atcDataGroup
        Dim lDataset As atcDataSet
        Try
            Dim lLowerValues As New atcCollection
            For Each lValue As String In aValues
                lLowerValues.Add(lValue.ToLower)
            Next

            For lIndex As Integer = 0 To Me.Count - 1
                lDataset = Me.ItemByIndex(lIndex)
                If lLowerValues.Contains(CStr(lDataset.Attributes.GetValue(aAttributeName)).ToLower) Then
                    lMatch.Add(Me.Keys(lIndex), lDataset)
                    If lMatch.Count = aLimit Then
                        Exit For 'Reached requested limit, stop looking for more
                    End If
                End If
            Next
        Catch
        End Try
        Return lMatch
    End Function

    Public Property SelectedData() As atcDataGroup
        Get
            If pSelectedData Is Nothing Then 'Initialize now if not already done
                pSelectedData = New atcDataGroup
            End If
            Return pSelectedData
        End Get
        Set(ByVal newValue As atcDataGroup)
            pSelectedData = newValue
        End Set
    End Property

    ''' <summary>
    ''' Return a sorted collection of unique values that data sets in this group have for the given attribute
    ''' </summary>
    ''' <param name="aAttributeName">Name of attribute to find values of</param>
    ''' <param name="aMissingValue">Optional string to use if a dataset does not have a value for the given attribute</param>
    Public Function SortedAttributeValues(ByVal aAttributeName As String, Optional ByVal aMissingValue As Object = Nothing) As atcCollection
        Return SortedAttributeValues(atcDataAttributes.GetDefinition(aAttributeName), aMissingValue)
    End Function

    ''' <summary>
    ''' Return a sorted collection of unique values that data sets in this group have for the given attribute
    ''' </summary>
    ''' <param name="aAttributeDefinition">Attribute to find values of</param>
    ''' <param name="aMissingValue">Optional string to use if a dataset does not have a value for the given attribute</param>
    Public Function SortedAttributeValues(ByVal aAttributeDefinition As atcAttributeDefinition, Optional ByVal aMissingValue As Object = Nothing) As atcCollection
        Dim lSortedValues As New atcCollection
        If Not aAttributeDefinition Is Nothing Then
            Dim lAttributeName As String = aAttributeDefinition.Name
            Dim lAttributeNumeric As Boolean = aAttributeDefinition.IsNumeric
            Dim lTsIndex As Integer = 0
            Dim lItemIndex As Integer = 0
            Logger.Status("Finding Values for " & lAttributeName)
            Dim lValue As String
            For Each ts As atcDataSet In Me
                Try
                    If lAttributeNumeric Then
                        Dim lKey As Double = ts.Attributes.GetValue(lAttributeName, pNaN)
                        If Not aMissingValue Is Nothing OrElse Not Double.IsNaN(lKey) Then
                            lItemIndex = lSortedValues.BinarySearchForKey(lKey)
                            If lItemIndex = lSortedValues.Count OrElse lKey <> lSortedValues.Keys.Item(lItemIndex) Then
                                lValue = ts.Attributes.GetFormattedValue(lAttributeName, aMissingValue)
                                If Not lValue Is Nothing Then
                                    lSortedValues.Insert(lItemIndex, lKey, lValue)
                                End If
                            End If
                        End If
                    Else
                        Dim lKey As String = ts.Attributes.GetValue(lAttributeName, aMissingValue)
                        If Not lKey Is Nothing Then
                            lItemIndex = lSortedValues.BinarySearchForKey(lKey)
                            If lItemIndex = lSortedValues.Count OrElse Not lKey.Equals(lSortedValues.Keys.Item(lItemIndex)) Then
                                lValue = ts.Attributes.GetFormattedValue(lAttributeName, aMissingValue)
                                If Not lValue Is Nothing Then
                                    lSortedValues.Insert(lItemIndex, lKey, lValue)
                                End If
                            End If
                        End If
                    End If
                Catch ex As Exception
                    Logger.Dbg("Can't display value of " & lAttributeName & ": " & ex.Message)
                End Try
                lTsIndex += 1
                Logger.Progress(lTsIndex, Count)
            Next
            Logger.Status("")
        End If
        Return lSortedValues
    End Function

    Public Function CommonAttributeValue(ByVal aAttributeName As String, Optional ByVal aMissingValue As Object = Nothing) As Object
        Dim lSetValue As Boolean = False
        CommonAttributeValue = Nothing
        For Each ts As atcDataSet In Me
            Dim lNewValue As Object = ts.Attributes.GetValue(aAttributeName, Nothing)
            If Not lSetValue Then
                CommonAttributeValue = lNewValue
                lSetValue = True
            Else
                If CommonAttributeValue Is Nothing OrElse lNewValue Is Nothing Then
                    Return aMissingValue
                ElseIf CommonAttributeValue <> lNewValue Then
                    Return aMissingValue
                End If
            End If
        Next
    End Function

    ''' <summary>Contents of this class expressed as a string.</summary>
    Public Shadows Function ToString() As String
        ToString = Count & " Data:"
        For Each lts As atcDataSet In Me
            ToString &= vbCrLf & "  " & lts.ToString
        Next
    End Function
End Class
