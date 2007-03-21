''' <summary>
''' Collection that extends ArrayList with Keys
''' </summary>
''' <remarks>
''' Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license
''' </remarks>
Public Class atcCollection
    Inherits ArrayList

    Private pKeys As ArrayList = New ArrayList

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

    Public Shadows Function Add(ByVal Value As Object) As Integer
        Return Add(Value, Value)
    End Function
    Public Shadows Function Add(ByVal key As Object, ByVal value As Object) As Integer
        pKeys.Add(key)
        Return MyBase.Add(value)
    End Function

    Public Shadows Sub AddRange(ByVal c As System.Collections.ICollection)
        pKeys.AddRange(c)
        MyBase.AddRange(c)
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
        pKeys.Clear()
        MyBase.Clear()
    End Sub

    Public Shadows Function Clone() As atcCollection
        Dim newClone As New atcCollection
        For index As Integer = 0 To MyBase.Count - 1
            newClone.Add(pKeys.Item(index), MyBase.Item(index))
        Next
        Return newClone
    End Function

    Public Shadows Sub Insert(ByVal index As Integer, ByVal value As Object)
        Insert(index, value, value)
    End Sub
    Public Shadows Sub Insert(ByVal index As Integer, ByVal key As Object, ByVal value As Object)
        pKeys.Insert(index, key)
        MyBase.Insert(index, value)
    End Sub

    Public Shadows Sub InsertRange(ByVal index As Integer, ByVal collValues As ICollection)
        InsertRange(index, collValues, collValues)
    End Sub
    Public Shadows Sub InsertRange(ByVal index As Integer, ByVal collKeys As ICollection, ByVal collValues As ICollection)
        pKeys.InsertRange(index, collKeys)
        MyBase.InsertRange(index, collValues)
    End Sub

    Public Shadows Sub Remove(ByVal value As Object)
        Try
            Dim index As Integer = MyBase.IndexOf(value)
            If index >= 0 Then RemoveAt(index)
        Catch e As Exception
        End Try
    End Sub

    Public Overridable Sub RemoveByKey(ByVal key As Object)
        Try
            Dim index As Integer = pKeys.IndexOf(key)
            If index >= 0 Then RemoveAt(index)
        Catch e As Exception
        End Try
    End Sub

    Public Shadows Sub RemoveAt(ByVal index As Integer)
        pKeys.RemoveAt(index)
        MyBase.RemoveAt(index)
    End Sub

    Public Shadows Sub RemoveRange(ByVal index As Integer, ByVal count As Integer)
        pKeys.RemoveRange(index, count)
        MyBase.RemoveRange(index, count)
    End Sub

    Public Shadows Sub Reverse()
        pKeys.Reverse()
        MyBase.Reverse()
    End Sub

    Public Shadows Sub Reverse(ByVal index As Integer, ByVal count As Integer)
        pKeys.Reverse(index, count)
        MyBase.Reverse(index, count)
    End Sub

    Public Shadows Sub SetRange(ByVal index As Integer, ByVal values As ICollection)
        SetRange(index, values, values)
    End Sub
    Public Shadows Sub SetRange(ByVal index As Integer, ByVal keys As ICollection, ByVal values As ICollection)
        pKeys.SetRange(index, keys)
        MyBase.SetRange(index, keys)
    End Sub

    Public Shadows Sub Sort()
        Sort(New Comparer(New System.Globalization.CultureInfo("")))
    End Sub

    Public Shadows Sub Sort(ByVal comparer As System.Collections.IComparer)
        Sort(0, MyBase.Count, comparer)
    End Sub

    Public Shadows Sub Sort(ByVal index As Integer, ByVal count As Integer, ByVal comparer As System.Collections.IComparer)
        Dim lNewKeys As ArrayList = New ArrayList(pKeys)
        Dim lNewValues As New ArrayList
        Dim lOldIndex As Integer
        lNewKeys.Sort(index, count, comparer)
        For Each lNewKey As Object In lNewKeys
            lOldIndex = pKeys.IndexOf(lNewKey)
            lNewValues.Add(MyBase.Item(lOldIndex))
            pKeys.Item(lOldIndex) = Nothing
        Next
        MyBase.Clear()
        MyBase.AddRange(lNewValues)
    End Sub

    Public Shadows Sub TrimToSize()
        pKeys.TrimToSize()
        MyBase.TrimToSize()
    End Sub

    'Exactly the same as Item. Added for clarity as a parallel to ItemByKey.
    Public Overridable Property ItemByIndex(ByVal index As Integer) As Object
        Get
            Return MyBase.Item(index)
        End Get
        Set(ByVal newValue As Object)
            MyBase.Item(index) = newValue
        End Set
    End Property

    Public Property ItemByKey(ByVal key As Object) As Object
        Get
            Dim index As Integer = IndexFromKey(key)
            If index >= 0 Then
                Return MyBase.Item(index)
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal newValue As Object)
            Dim index As Integer = IndexFromKey(key)
            If index >= 0 Then
                MyBase.Item(index) = newValue
            Else
                Add(key, newValue)
            End If
        End Set
    End Property

    Public Function IndexFromKey(ByVal key As Object) As Integer
        Try
            Return pKeys.IndexOf(key)
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
        Dim lString As New Text.StringBuilder
        lString.AppendLine("Collection count = " & lCount)

        Dim lStop As Integer = lCount - 1
        If lStop >= aNumValues - 1 Then lStop = aNumValues - 2
        For lIndex As Integer = 0 To lStop
            Try
                lString.AppendLine(lIndex & " (" & Me.Keys.Item(lIndex) & ") " & Me.ItemByIndex(lIndex))
            Catch
                'Skip listing unprintable keys/values
            End Try
        Next
        If lCount - lStop > 1 Then
            lString.AppendLine("(skipped " & lCount - lStop - 2 & " values)")
            lString.AppendLine(lCount - 1 & " (" & Me.Keys.Item(lCount - 1) & ") " & Me.ItemByIndex(lCount - 1))
        End If
        Return lString.ToString
    End Function

    Public Overrides Function ToString() As String
        Return Me.ToString(10)
    End Function
End Class
