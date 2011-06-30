Imports System
Imports System.Collections.Generic
Imports System.Collections
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports atcUtility

'''<summary>
'''This is a test class for atcCollectionTest and is intended
'''to contain all atcCollectionTest Unit Tests
'''</summary>
<TestClass()> _
Public Class atcCollectionTest
    Private testContextInstance As TestContext

    '''<summary>
    '''Gets or sets the test context which provides
    '''information about and functionality for the current test run.
    '''</summary>
    Public Property TestContext() As TestContext
        Get
            Return testContextInstance
        End Get
        Set(ByVal value As TestContext)
            testContextInstance = Value
        End Set
    End Property

#Region "Additional test attributes"
    '
    'You can use the following additional attributes as you write your tests:
    '
    'Use ClassInitialize to run code before running the first test in the class
    '<ClassInitialize()>  _
    'Public Shared Sub MyClassInitialize(ByVal testContext As TestContext)
    'End Sub
    '
    'Use ClassCleanup to run code after all tests in a class have run
    '<ClassCleanup()>  _
    'Public Shared Sub MyClassCleanup()
    'End Sub
    '
    'Use TestInitialize to run code before running each test
    '<TestInitialize()>  _
    'Public Sub MyTestInitialize()
    'End Sub
    '
    'Use TestCleanup to run code after each test has run
    '<TestCleanup()>  _
    'Public Sub MyTestCleanup()
    'End Sub
    '
#End Region

    '''<summary>Test atcCollection Constructor</summary>
    <TestMethod()> Public Sub atcCollectionConstructorTest()
        Dim lValuesToAdd() As String = {"A", "B", "C"}
        Dim lCollection As atcCollection = New atcCollection(lValuesToAdd)
        Assert.IsInstanceOfType(lCollection, GetType(atcCollection))
        Assert.AreEqual(3, lCollection.Count)
    End Sub

    '''<summary>Test Add</summary>
    <TestMethod()> Public Sub AddTest()
        Dim lCollection As atcCollection = New atcCollection
        Dim lActual As Integer = lCollection.Add("Key4", 4)
        Assert.AreEqual(0, lActual)
        lActual = lCollection.Add("Key9", 9)
        Assert.AreEqual(1, lActual)
        lActual = lCollection.Add("Key4", 4)
        Assert.AreEqual(0, lActual)
    End Sub

    '''<summary>Test Add</summary>
    <TestMethod()> Public Sub AddTest1()
        Dim lValuesToAdd() As String = {"A", "B", "C"}
        Dim lCollection As atcCollection = New atcCollection(lValuesToAdd)
        Dim lAddThese As atcCollection = New atcCollection({"D", "E"})
        lCollection.Add(lAddThese)
        Assert.AreEqual(5, lCollection.Count)
    End Sub

    '''<summary>Test Add</summary>
    <TestMethod()> Public Sub AddTest2()
        Dim lCollection As atcCollection = New atcCollection({"A", "B", "C"})
        Dim lActual As Integer = lCollection.Add("D")
        Assert.AreEqual(3, lActual)
      End Sub

    '''<summary>Test AddRange</summary>
    <TestMethod()> Public Sub AddRangeTest()
        Dim lCollection As atcCollection = New atcCollection({"A", "B", "C"})
        Dim aC As ICollection = ({"D", "E"})
        lCollection.AddRange(aC)
        Assert.AreEqual(5, lCollection.Count)
    End Sub

    '''<summary>Test AddRange</summary>
    <TestMethod()> Public Sub AddRangeTest1()
        Dim lCollection As atcCollection = New atcCollection({"A", "B", "C"})
        Dim lKeys As New ArrayList From {"KeyD", "KeyE"}
        Dim lValues As New ArrayList From {"D", "E"}
        lCollection.AddRange(lKeys, lValues)
        Assert.AreEqual(5, lCollection.Count)
        Assert.AreEqual("D", lCollection.ItemByKey("KeyD"))
    End Sub

    '''<summary>Test AddRange</summary>
    <TestMethod()> Public Sub AddRangeTest2()
        Dim lCollection As atcCollection = New atcCollection({"A", "B", "C"})
        Dim lKeys() As String = {"KeyD", "KeyE"}
        Dim lValues() As String = {"D", "E"}
        lCollection.AddRange(lKeys, lValues)
        Assert.AreEqual(5, lCollection.Count)
        Assert.AreEqual("D", lCollection.ItemByKey("KeyD"))
    End Sub

    '''<summary>Test BinarySearchForKey</summary>
    <TestMethod()> Public Sub BinarySearchForKeyTest()
        Dim lCollection As atcCollection = New atcCollection({"A", "B", "C", "D", "E"})
        Assert.AreEqual(3, lCollection.BinarySearchForKey("D"))
        Assert.AreEqual(5, lCollection.BinarySearchForKey("X"))
      End Sub

    '''<summary>Test BinarySearchForKey</summary>
    <TestMethod()> Public Sub BinarySearchForKeyTest1()
        Dim lKeys As New ArrayList From {1, 2, 3, 4, 5}
        Dim lValues As New ArrayList From {"A", "B", "C", "D", "E"}
        Dim lCollection As New atcCollection
        lCollection.AddRange(lKeys, lValues)
        Assert.AreEqual(3, lCollection.BinarySearchForKey(3.5))
        Assert.AreEqual(5, lCollection.BinarySearchForKey(6.5))
        Assert.AreEqual(0, lCollection.BinarySearchForKey(-1))
    End Sub

    '''<summary>Test ChangeTo</summary>
    <TestMethod()> Public Sub ChangeToTest()
        Dim lKeys As New ArrayList From {1, 2, 3, 4, 5}
        Dim lValues As New ArrayList From {"A", "B", "C", "D", "E"}
        Dim lCollection As New atcCollection
        lCollection.AddRange(lKeys, lValues)
        lCollection.ChangeTo(New atcCollection({"A1", "B2"}))
        Assert.AreEqual(2, lCollection.Count)
    End Sub

    '''<summary>Test Clear</summary>
    <TestMethod()> Public Sub ClearTest()
        Dim lCollection As atcCollection = New atcCollection({"A", "B", "C", "D", "E"})
        lCollection.Clear()
        Assert.AreEqual(0, lCollection.Count)
    End Sub

    '''<summary>Test Clone</summary>
    <TestMethod()> Public Sub CloneTest()
        Dim lCollection As atcCollection = New atcCollection({"A", "B", "C", "D", "E"})
        Dim lClonedCollection As atcCollection = lCollection.Clone
        Assert.AreEqual(lCollection.Count, lClonedCollection.Count)
        'TODO: should this test work?
        'Assert.AreEqual(lCollection, lClonedCollection)
      End Sub

    '''<summary>Test CompareValues</summary>
    <TestMethod(), DeploymentItem("atcUtility.dll")> _
    Public Sub CompareValuesTest()
        Dim lCollectionAccessor As atcCollection_Accessor = New atcCollection_Accessor(New atcCollection)
        Dim lX As KeyValuePair(Of Object, Object) = New KeyValuePair(Of Object, Object)(3, "C")
        Dim lY As KeyValuePair(Of Object, Object) = New KeyValuePair(Of Object, Object)(4, "D")
        Assert.AreEqual(-1, lCollectionAccessor.CompareValues(lX, lY))
        Dim lZ As KeyValuePair(Of Object, Object) = New KeyValuePair(Of Object, Object)(3, "C")
        Assert.AreEqual(0, lCollectionAccessor.CompareValues(lX, lZ))
    End Sub

    '''<summary>Test DictionaryEntries</summary>
    <TestMethod()> Public Sub DictionaryEntriesTest()
        Dim lCollection As atcCollection = New atcCollection({"A", "B", "C", "D", "E"})
        Assert.IsInstanceOfType(lCollection.DictionaryEntries, GetType(atcCollection_Accessor.clsDictionaryEnumerator))
    End Sub

    '''<summary>Test Dispose</summary>
    <TestMethod()> Public Sub DisposeTest()
        Dim lCollection As atcCollection = New atcCollection({"A", "B", "C", "D", "E"})
        Assert.AreEqual(5, lCollection.Keys.Count)
        Assert.AreEqual(5, lCollection.Count)
        lCollection.Dispose()
        Assert.AreEqual(0, lCollection.Keys.Count)
        Assert.AreEqual(0, lCollection.Count)
    End Sub

    '''<summary>Test Increment</summary>
    <TestMethod()> Public Sub IncrementTest()
        Dim lValues As New ArrayList From {1, 2, 3, 4, 5}
        Dim lKeys As New ArrayList From {"A", "B", "C", "D", "E"}
        Dim lCollection As New atcCollection
        lCollection.AddRange(lKeys, lValues)
        Assert.AreEqual(CDbl(4), lCollection.Increment("C"))
      End Sub

    '''<summary>Test Increment</summary>
    <TestMethod()> Public Sub IncrementTest1()
        Dim lValues As New ArrayList From {1, 2, 3, 4, 5}
        Dim lKeys As New ArrayList From {"A", "B", "C", "D", "E"}
        Dim lCollection As New atcCollection
        lCollection.AddRange(lKeys, lValues)
        Assert.AreEqual(CDbl(6), lCollection.Increment("C", 3))
        Assert.AreEqual(CDbl(4), lCollection.Increment("B", 2))
        Assert.AreEqual(CDbl(4), lCollection.Increment("C", -2))
    End Sub

    '''<summary>Test IndexFromKey</summary>
    <TestMethod()> Public Sub IndexFromKeyTest()
        Dim lValues As New ArrayList From {1, 2, 3, 4, 5}
        Dim lKeys As New ArrayList From {"A", "B", "C", "D", "E"}
        Dim lCollection As New atcCollection
        lCollection.AddRange(lKeys, lValues)
        Assert.AreEqual(2, lCollection.IndexFromKey("C"))
      End Sub

    '''<summary>Test Insert</summary>
    <TestMethod()> Public Sub InsertTest()
        Dim lValues As New ArrayList From {1, 2, 3, 4, 5}
        Dim lKeys As New ArrayList From {"A", "B", "C", "D", "E"}
        Dim lCollection As New atcCollection
        lCollection.AddRange(lKeys, lValues)
        lCollection.Insert(2, 2.5)
        Assert.AreEqual(CDbl(1), CDbl(lCollection.ItemByIndex(0)))
        Assert.AreEqual(CDbl(2.5), CDbl(lCollection.ItemByIndex(2)))
        Assert.AreEqual(CDbl(2), CDbl(lCollection.ItemByIndex(1)))
        Assert.AreEqual(CDbl(3), CDbl(lCollection.ItemByIndex(3)))
    End Sub

    '''<summary>Test Insert</summary>
    <TestMethod()> Public Sub InsertTest1()
        Dim aValuesToAdd() As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcCollection = New atcCollection(aValuesToAdd) ' TODO: Initialize to an appropriate value
        Dim aIndex As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aKey As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim aValue As Object = Nothing ' TODO: Initialize to an appropriate value
        target.Insert(aIndex, aKey, aValue)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test InsertRange</summary>
    <TestMethod()> Public Sub InsertRangeTest()
        Dim aValuesToAdd() As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcCollection = New atcCollection(aValuesToAdd) ' TODO: Initialize to an appropriate value
        Dim aIndex As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aCollValues As ICollection = Nothing ' TODO: Initialize to an appropriate value
        target.InsertRange(aIndex, aCollValues)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test InsertRange</summary>
    <TestMethod()> Public Sub InsertRangeTest1()
        Dim aValuesToAdd() As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcCollection = New atcCollection(aValuesToAdd) ' TODO: Initialize to an appropriate value
        Dim aIndex As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aCollKeys As ICollection = Nothing ' TODO: Initialize to an appropriate value
        Dim aCollValues As ICollection = Nothing ' TODO: Initialize to an appropriate value
        target.InsertRange(aIndex, aCollKeys, aCollValues)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test Remove</summary>
    <TestMethod()> Public Sub RemoveTest()
        Dim aValuesToAdd() As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcCollection = New atcCollection(aValuesToAdd) ' TODO: Initialize to an appropriate value
        Dim aValue As Object = Nothing ' TODO: Initialize to an appropriate value
        target.Remove(aValue)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test RemoveAt</summary>
    <TestMethod()> Public Sub RemoveAtTest()
        Dim aValuesToAdd() As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcCollection = New atcCollection(aValuesToAdd) ' TODO: Initialize to an appropriate value
        Dim aIndex As Integer = 0 ' TODO: Initialize to an appropriate value
        target.RemoveAt(aIndex)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test RemoveByKey</summary>
    <TestMethod()> Public Sub RemoveByKeyTest()
        Dim aValuesToAdd() As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcCollection = New atcCollection(aValuesToAdd) ' TODO: Initialize to an appropriate value
        Dim aKey As Object = Nothing ' TODO: Initialize to an appropriate value
        target.RemoveByKey(aKey)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test RemoveRange</summary>
    <TestMethod()> Public Sub RemoveRangeTest()
        Dim aValuesToAdd() As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcCollection = New atcCollection(aValuesToAdd) ' TODO: Initialize to an appropriate value
        Dim aIndex As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aCount As Integer = 0 ' TODO: Initialize to an appropriate value
        target.RemoveRange(aIndex, aCount)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test Reverse</summary>
    <TestMethod()> Public Sub ReverseTest()
        Dim aValuesToAdd() As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcCollection = New atcCollection(aValuesToAdd) ' TODO: Initialize to an appropriate value
        Dim aIndex As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aCount As Integer = 0 ' TODO: Initialize to an appropriate value
        target.Reverse(aIndex, aCount)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test Reverse</summary>
    <TestMethod()> Public Sub ReverseTest1()
        Dim aValuesToAdd() As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcCollection = New atcCollection(aValuesToAdd) ' TODO: Initialize to an appropriate value
        target.Reverse()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test SetRange</summary>
    <TestMethod()> Public Sub SetRangeTest()
        Dim aValuesToAdd() As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcCollection = New atcCollection(aValuesToAdd) ' TODO: Initialize to an appropriate value
        Dim aIndex As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aKeys As ICollection = Nothing ' TODO: Initialize to an appropriate value
        Dim aValues As ICollection = Nothing ' TODO: Initialize to an appropriate value
        target.SetRange(aIndex, aKeys, aValues)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test SetRange</summary>
    <TestMethod()> Public Sub SetRangeTest1()
        Dim aValuesToAdd() As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcCollection = New atcCollection(aValuesToAdd) ' TODO: Initialize to an appropriate value
        Dim aIndex As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aValues As ICollection = Nothing ' TODO: Initialize to an appropriate value
        target.SetRange(aIndex, aValues)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test Sort</summary>
    <TestMethod()> Public Sub SortTest()
        Dim aValuesToAdd() As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcCollection = New atcCollection(aValuesToAdd) ' TODO: Initialize to an appropriate value
        target.Sort()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test Sort</summary>
    <TestMethod()> Public Sub SortTest1()
        Dim aValuesToAdd() As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcCollection = New atcCollection(aValuesToAdd) ' TODO: Initialize to an appropriate value
        Dim aComparer As IComparer = Nothing ' TODO: Initialize to an appropriate value
        target.Sort(aComparer)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test Sort</summary>
    <TestMethod()> Public Sub SortTest2()
        Dim aValuesToAdd() As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcCollection = New atcCollection(aValuesToAdd) ' TODO: Initialize to an appropriate value
        Dim aIndex As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aCount As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aComparer As IComparer = Nothing ' TODO: Initialize to an appropriate value
        target.Sort(aIndex, aCount, aComparer)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test SortByValue</summary>
    <TestMethod()> Public Sub SortByValueTest()
        Dim aValuesToAdd() As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcCollection = New atcCollection(aValuesToAdd) ' TODO: Initialize to an appropriate value
        target.SortByValue()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test ToString</summary>
    <TestMethod()> Public Sub ToStringTest()
        Dim aValuesToAdd() As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcCollection = New atcCollection(aValuesToAdd) ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = target.ToString
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test ToString</summary>
    <TestMethod()> Public Sub ToStringTest1()
        Dim aValuesToAdd() As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcCollection = New atcCollection(aValuesToAdd) ' TODO: Initialize to an appropriate value
        Dim aNumValues As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = target.ToString(aNumValues)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test TrimToSize</summary>
    <TestMethod()> Public Sub TrimToSizeTest()
        Dim aValuesToAdd() As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcCollection = New atcCollection(aValuesToAdd) ' TODO: Initialize to an appropriate value
        target.TrimToSize()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test Capacity</summary>
    <TestMethod()> Public Sub CapacityTest()
        Dim aValuesToAdd() As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcCollection = New atcCollection(aValuesToAdd) ' TODO: Initialize to an appropriate value
        Dim expected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        target.Capacity = expected
        actual = target.Capacity
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test ItemByIndex</summary>
    <TestMethod()> Public Sub ItemByIndexTest()
        Dim aValuesToAdd() As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcCollection = New atcCollection(aValuesToAdd) ' TODO: Initialize to an appropriate value
        Dim aIndex As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim expected As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Object
        target.ItemByIndex(aIndex) = expected
        actual = target.ItemByIndex(aIndex)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test ItemByKey</summary>
    <TestMethod()> Public Sub ItemByKeyTest()
        Dim aValuesToAdd() As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcCollection = New atcCollection(aValuesToAdd) ' TODO: Initialize to an appropriate value
        Dim aKey As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Object
        target.ItemByKey(aKey) = expected
        actual = target.ItemByKey(aKey)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test Keys</summary>
    <TestMethod()> Public Sub KeysTest()
        Dim aValuesToAdd() As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcCollection = New atcCollection(aValuesToAdd) ' TODO: Initialize to an appropriate value
        Dim expected As ArrayList = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ArrayList
        target.Keys = expected
        actual = target.Keys
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub
End Class
