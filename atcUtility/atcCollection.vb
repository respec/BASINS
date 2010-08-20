''' <summary>
''' Collection that extends ArrayList with Keys
''' </summary>
''' <remarks>
''' Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license
''' </remarks>
Public Class atcCollection
    Inherits ArrayList
    Implements IDisposable

    Private pKeys As ArrayList = New ArrayList

    'Do we need to do this or will operator = do this by default?
    'Public Shared Operator =(ByVal a1 As atcCollection, _
    '                         ByVal a2 As atcCollection) As Boolean
    '    Return Object.Equals(a1, a2)
    'End Operator

    'Public Shared Operator <>(ByVal a1 As atcCollection, _
    '                          ByVal a2 As atcCollection) As Boolean
    '    Return Not Object.Equals(a1, a2)
    'End Operator

    ''' <summary>Returns first index of a key equal to or higher than aKey</summary>
    ''' <param name="aKey">Key to search for</param>
    ''' <returns>Returns aKeys.Count if aKeys is empty or contains only values less than aKey</returns>
    ''' <remarks>Only works for collections sorted by key</remarks>
    Public Function BinarySearchForKey(ByVal aKey As String) As Integer
        Dim lHigher As Integer = pKeys.Count
        Dim lLower As Integer = -1
        Dim lProbe As Integer
        While (lHigher - lLower > 1)
            lProbe = (lHigher + lLower) / 2
            If (pKeys.Item(lProbe) < aKey) Then
                lLower = lProbe
            Else
                lHigher = lProbe
            End If
        End While
        Return lHigher
    End Function

    ''' <summary>Returns first index of a key equal to or higher than aKey</summary>
    ''' <param name="aKey">Key to search for</param>
    ''' <returns>Returns aKeys.Count if aKeys is empty or contains only values less than aKey</returns>
    ''' <remarks>Only works for collections sorted by key</remarks>
    Public Function BinarySearchForKey(ByVal aKey As Double) As Integer
        Dim lHigher As Integer = pKeys.Count
        Dim lLower As Integer = -1
        Dim lProbe As Integer
        While (lHigher - lLower > 1)
            lProbe = (lHigher + lLower) / 2
            If (CDbl(pKeys.Item(lProbe)) < aKey) Then
                lLower = lProbe
            Else
                lHigher = lProbe
            End If
        End While
        Return lHigher
    End Function

    Public Sub New(ByVal ParamArray aValuesToAdd() As Object)
        MyBase.New()
        AddRange(aValuesToAdd)
    End Sub

    Public Property Keys() As ArrayList
        Get
            Return pKeys
        End Get
        Set(ByVal newValue As ArrayList)
            pKeys = newValue
        End Set
    End Property

    Public Shadows Function Add(ByVal aValue As Object) As Integer
        Return Add(aValue, aValue)
    End Function
    Public Shadows Function Add(ByVal aKey As Object, ByVal aValue As Object) As Integer
        Dim lKeyIndex As Integer = IndexFromKey(aKey)
        If lKeyIndex = -1 Then 'no key, add it
            pKeys.Add(aKey)
            Return MyBase.Add(aValue)
        ElseIf aValue = MyBase.Item(lKeyIndex) Then 'key exists and values match, return its index
            Return lKeyIndex
        Else 'conflict with values
            Throw New ApplicationException("Key " & aKey.ToString & " exists, new Value '" & aValue.ToString & "' <> old Value '" & MyBase.Item(lKeyIndex).ToString & "'")
        End If
    End Function

    Public Function Increment(ByVal aKey As Object) As Double
        Return Increment(aKey, 1)
    End Function

    Public Function Increment(ByVal aKey As Object, ByVal aValue As Double) As Double
        Dim lKeyIndex As Integer = IndexFromKey(aKey)
        If lKeyIndex = -1 Then
            Add(aKey, aValue)
            Return aValue
        Else
            Item(lKeyIndex) += aValue
            Return Item(lKeyIndex)
        End If
    End Function

    ''' <summary>Add items from an atcCollection along with their keys</summary>
    Public Shadows Sub Add(ByVal aAddThese As atcCollection)
        Dim lLastIndex As Integer = aAddThese.Count - 1
        For lIndex As Integer = 0 To lLastIndex
            Add(aAddThese.Keys(lIndex), aAddThese.ItemByIndex(lIndex))
        Next
    End Sub

    Public Shadows Sub AddRange(ByVal aKeys As System.Collections.IEnumerable, ByVal aValues As System.Collections.IEnumerable)
        pKeys.AddRange(aKeys)
        MyBase.AddRange(aValues)
    End Sub

    Public Shadows Sub AddRange(ByVal aC As System.Collections.ICollection)
        pKeys.AddRange(aC)
        MyBase.AddRange(aC)
    End Sub

    Public Shadows Property Capacity() As Integer
        Get
            Return MyBase.Capacity
        End Get
        Set(ByVal newValue As Integer)
            pKeys.Capacity = newValue
            MyBase.Capacity = newValue
        End Set
    End Property

    Public Shadows Sub Clear()
        If pKeys IsNot Nothing Then pKeys.Clear()
        MyBase.Clear()
    End Sub

    Public Overridable Sub Dispose() Implements IDisposable.Dispose
        If pKeys IsNot Nothing Then pKeys.Clear()
        MyBase.Clear()
    End Sub

    Public Shadows Function Clone() As atcCollection
        Dim newClone As New atcCollection
        For index As Integer = 0 To MyBase.Count - 1
            newClone.Add(pKeys.Item(index), MyBase.Item(index))
        Next
        Return newClone
    End Function

    Public Shadows Sub Insert(ByVal aIndex As Integer, ByVal aValue As Object)
        Insert(aIndex, aValue, aValue)
    End Sub
    Public Shadows Sub Insert(ByVal aIndex As Integer, ByVal aKey As Object, ByVal aValue As Object)
        pKeys.Insert(aIndex, aKey)
        MyBase.Insert(aIndex, aValue)
    End Sub

    Public Shadows Sub InsertRange(ByVal aIndex As Integer, ByVal aCollValues As ICollection)
        InsertRange(aIndex, aCollValues, aCollValues)
    End Sub
    Public Shadows Sub InsertRange(ByVal aIndex As Integer, ByVal aCollKeys As ICollection, ByVal aCollValues As ICollection)
        pKeys.InsertRange(aIndex, aCollKeys)
        MyBase.InsertRange(aIndex, aCollValues)
    End Sub

    Public Shadows Sub Remove(ByVal aValue As Object)
        Try
            Dim lIndex As Integer = MyBase.IndexOf(aValue)
            If lIndex >= 0 Then RemoveAt(lIndex)
        Catch e As Exception
        End Try
    End Sub

    Public Overridable Sub RemoveByKey(ByVal aKey As Object)
        Try
            Dim lIndex As Integer = pKeys.IndexOf(aKey)
            If lIndex >= 0 Then RemoveAt(lIndex)
        Catch e As Exception
        End Try
    End Sub

    Public Shadows Sub RemoveAt(ByVal aIndex As Integer)
        pKeys.RemoveAt(aIndex)
        MyBase.RemoveAt(aIndex)
    End Sub

    Public Shadows Sub RemoveRange(ByVal aIndex As Integer, ByVal aCount As Integer)
        pKeys.RemoveRange(aIndex, aCount)
        MyBase.RemoveRange(aIndex, aCount)
    End Sub

    Public Shadows Sub Reverse()
        pKeys.Reverse()
        MyBase.Reverse()
    End Sub

    Public Shadows Sub Reverse(ByVal aIndex As Integer, ByVal aCount As Integer)
        pKeys.Reverse(aIndex, aCount)
        MyBase.Reverse(aIndex, aCount)
    End Sub

    Public Shadows Sub SetRange(ByVal aIndex As Integer, ByVal aValues As ICollection)
        SetRange(aIndex, aValues, aValues)
    End Sub
    Public Shadows Sub SetRange(ByVal aIndex As Integer, ByVal aKeys As ICollection, ByVal aValues As ICollection)
        pKeys.SetRange(aIndex, aKeys)
        MyBase.SetRange(aIndex, aValues)
    End Sub

    Public Shadows Sub Sort()
        Sort(New Comparer(New System.Globalization.CultureInfo("")))
    End Sub

    Public Shadows Sub Sort(ByVal aComparer As System.Collections.IComparer)
        Sort(0, MyBase.Count, aComparer)
    End Sub

    Public Shadows Sub Sort(ByVal aIndex As Integer, ByVal aCount As Integer, ByVal aComparer As System.Collections.IComparer)
        Dim lNewKeys As ArrayList = New ArrayList(pKeys)
        Dim lNewValues As New ArrayList
        Dim lOldIndex As Integer
        lNewKeys.Sort(aIndex, aCount, aComparer)
        For Each lNewKey As Object In lNewKeys
            lOldIndex = pKeys.IndexOf(lNewKey)
            lNewValues.Add(MyBase.Item(lOldIndex))
            pKeys.Item(lOldIndex) = Nothing
        Next
        MyBase.Clear()
        MyBase.AddRange(lNewValues)
        pKeys = lNewKeys
    End Sub

    Public Sub SortByValue()
        Dim lsl As New Generic.List(Of Generic.KeyValuePair(Of Object, Object))
        For lIndex As Integer = 0 To Me.Count - 1
            lsl.Add(New Generic.KeyValuePair(Of Object, Object)(pKeys(lIndex), Item(lIndex)))
        Next
        lsl.Sort(AddressOf CompareValues)
        Me.Clear()
        For Each lPair As Generic.KeyValuePair(Of Object, Object) In lsl
            Me.Add(lPair.Key, lPair.Value)
        Next
    End Sub

    Private Function CompareValues(ByVal x As Generic.KeyValuePair(Of Object, Object), _
                                   ByVal y As Generic.KeyValuePair(Of Object, Object)) As Integer
        Return x.Value.CompareTo(y.Value)
    End Function

    Public Shadows Sub TrimToSize()
        pKeys.TrimToSize()
        MyBase.TrimToSize()
    End Sub

    'Exactly the same as Item. Added for clarity as a parallel to ItemByKey.
    Public Overridable Property ItemByIndex(ByVal aIndex As Integer) As Object
        Get
            Return MyBase.Item(aIndex)
        End Get
        Set(ByVal newValue As Object)
            MyBase.Item(aIndex) = newValue
        End Set
    End Property

    Public Property ItemByKey(ByVal aKey As Object) As Object
        Get
            Dim index As Integer = IndexFromKey(aKey)
            If index >= 0 Then
                Return MyBase.Item(index)
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal newValue As Object)
            Dim index As Integer = IndexFromKey(aKey)
            If index >= 0 Then
                MyBase.Item(index) = newValue
            Else
                Add(aKey, newValue)
            End If
        End Set
    End Property

    Public Function IndexFromKey(ByVal akey As Object) As Integer
        Try

            Return pKeys.IndexOf(akey)
        Catch e As Exception
            Return -1
        End Try
    End Function

    Public Sub ChangeTo(ByVal aNewItems As atcCollection)
        Clear()
        For index As Integer = 0 To aNewItems.Count - 1
            Add(aNewItems.Keys(index), aNewItems.ItemByIndex(index))
        Next
    End Sub

    Public Overloads Function ToString(ByVal aNumValues As Integer) As String
        Dim lCount As Integer = Me.Count
        Dim lString As New System.Text.StringBuilder
        lString.AppendLine("Collection count = " & lCount)

        Dim lStop As Integer = lCount - 1
        If lStop >= aNumValues - 1 Then lStop = aNumValues - 2
        For lIndex As Integer = 0 To lStop
            Try
                Dim lItem As Object = Me.ItemByIndex(lIndex)
                If TypeOf (lItem) Is ArrayList Then
                    lString.AppendLine(lIndex & " (" & Me.Keys.Item(lIndex).ToString & ") " & "<ArrayList>")
                Else
                    lString.AppendLine(lIndex & " (" & Me.Keys.Item(lIndex).ToString & ") " & lItem.ToString)
                End If
            Catch
                'Skip listing unprintable keys/values
            End Try
        Next
        If lCount - lStop > 2 Then
            lString.AppendLine("(skipped " & lCount - lStop - 2 & " values)")
            Dim lItem As Object = Me.ItemByIndex(lCount - 1)
            If TypeOf (lItem) Is ArrayList Then
                lString.AppendLine(lCount - 1 & " (" & Me.Keys.Item(lCount - 1).ToString & ") " & "<ArrayList>")
            Else
                lString.AppendLine(lCount - 1 & " (" & Me.Keys.Item(lCount - 1).ToString & ") " & Me.ItemByIndex(lCount - 1).ToString)
            End If
        End If
        Return lString.ToString
    End Function

    Public Overrides Function ToString() As String
        Return Me.ToString(10)
    End Function

    Public Function DictionaryEntries() As IEnumerable
        Return New clsDictionaryEnumerator(Me)
    End Function

    ''' <summary>
    ''' Enumerator returns key/value pairs as DictionaryEntry objects
    ''' </summary>
    Private Class clsDictionaryEnumerator
        Implements IEnumerable, IEnumerator, IDisposable

        Private pCollection As atcCollection
        Private pIndex As Integer

        Public Sub New(ByVal aCollection As atcCollection)
            pCollection = aCollection
            pIndex = -1
        End Sub

        ReadOnly Property Current() As Object Implements IEnumerator.Current
            Get
                Return New DictionaryEntry(pCollection.Keys(pIndex), pCollection.ItemByIndex(pIndex))
            End Get
        End Property

        Public Function MoveNext() As Boolean Implements IEnumerator.MoveNext
            If pIndex + 1 < pCollection.Count Then
                pIndex += 1
                Return True
            Else
                Return False
            End If
        End Function

        Public Sub Reset() Implements IEnumerator.Reset
            pIndex = -1
        End Sub

        Public Function GetEnumerator() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
            Return Me
        End Function

        Public Sub Dispose() Implements IDisposable.Dispose
        End Sub
    End Class
End Class

