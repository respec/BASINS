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
        Dim aValuesToAdd() As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcCollection = New atcCollection(aValuesToAdd)
        Assert.Inconclusive("TODO: Implement code to verify target")
    End Sub

    '''<summary>Test Add</summary>
    <TestMethod()> Public Sub AddTest()
        Dim aValuesToAdd() As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcCollection = New atcCollection(aValuesToAdd) ' TODO: Initialize to an appropriate value
        Dim aKey As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim aValue As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        actual = target.Add(aKey, aValue)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test Add</summary>
    <TestMethod()> Public Sub AddTest1()
        Dim aValuesToAdd() As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcCollection = New atcCollection(aValuesToAdd) ' TODO: Initialize to an appropriate value
        Dim aAddThese As atcCollection = Nothing ' TODO: Initialize to an appropriate value
        target.Add(aAddThese)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test Add</summary>
    <TestMethod()> Public Sub AddTest2()
        Dim aValuesToAdd() As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcCollection = New atcCollection(aValuesToAdd) ' TODO: Initialize to an appropriate value
        Dim aValue As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        actual = target.Add(aValue)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test AddRange</summary>
    <TestMethod()> Public Sub AddRangeTest()
        Dim aValuesToAdd() As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcCollection = New atcCollection(aValuesToAdd) ' TODO: Initialize to an appropriate value
        Dim aC As ICollection = Nothing ' TODO: Initialize to an appropriate value
        target.AddRange(aC)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test AddRange</summary>
    <TestMethod()> Public Sub AddRangeTest1()
        Dim aValuesToAdd() As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcCollection = New atcCollection(aValuesToAdd) ' TODO: Initialize to an appropriate value
        Dim aKeys As ArrayList = Nothing ' TODO: Initialize to an appropriate value
        Dim aValues As IEnumerable = Nothing ' TODO: Initialize to an appropriate value
        target.AddRange(aKeys, aValues)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test AddRange</summary>
    <TestMethod()> Public Sub AddRangeTest2()
        Dim aValuesToAdd() As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcCollection = New atcCollection(aValuesToAdd) ' TODO: Initialize to an appropriate value
        Dim aKeys As IEnumerable = Nothing ' TODO: Initialize to an appropriate value
        Dim aValues As IEnumerable = Nothing ' TODO: Initialize to an appropriate value
        target.AddRange(aKeys, aValues)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test BinarySearchForKey</summary>
    <TestMethod()> Public Sub BinarySearchForKeyTest()
        Dim aValuesToAdd() As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcCollection = New atcCollection(aValuesToAdd) ' TODO: Initialize to an appropriate value
        Dim aKey As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim expected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        actual = target.BinarySearchForKey(aKey)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test BinarySearchForKey</summary>
    <TestMethod()> Public Sub BinarySearchForKeyTest1()
        Dim aValuesToAdd() As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcCollection = New atcCollection(aValuesToAdd) ' TODO: Initialize to an appropriate value
        Dim aKey As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        actual = target.BinarySearchForKey(aKey)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test ChangeTo</summary>
    <TestMethod()> Public Sub ChangeToTest()
        Dim aValuesToAdd() As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcCollection = New atcCollection(aValuesToAdd) ' TODO: Initialize to an appropriate value
        Dim aNewItems As atcCollection = Nothing ' TODO: Initialize to an appropriate value
        target.ChangeTo(aNewItems)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test Clear</summary>
    <TestMethod()> Public Sub ClearTest()
        Dim aValuesToAdd() As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcCollection = New atcCollection(aValuesToAdd) ' TODO: Initialize to an appropriate value
        target.Clear()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test Clone</summary>
    <TestMethod()> Public Sub CloneTest()
        Dim aValuesToAdd() As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcCollection = New atcCollection(aValuesToAdd) ' TODO: Initialize to an appropriate value
        Dim expected As atcCollection = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcCollection
        actual = target.Clone
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test CompareValues</summary>
    <TestMethod(), DeploymentItem("atcUtility.dll")> _
    Public Sub CompareValuesTest()
        Dim param0 As PrivateObject = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcCollection_Accessor = New atcCollection_Accessor(param0) ' TODO: Initialize to an appropriate value
        Dim x As KeyValuePair(Of Object, Object) = New KeyValuePair(Of Object, Object)() ' TODO: Initialize to an appropriate value
        Dim y As KeyValuePair(Of Object, Object) = New KeyValuePair(Of Object, Object)() ' TODO: Initialize to an appropriate value
        Dim expected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        actual = target.CompareValues(x, y)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test DictionaryEntries</summary>
    <TestMethod()> Public Sub DictionaryEntriesTest()
        Dim aValuesToAdd() As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcCollection = New atcCollection(aValuesToAdd) ' TODO: Initialize to an appropriate value
        Dim expected As IEnumerable = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As IEnumerable
        actual = target.DictionaryEntries
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test Dispose</summary>
    <TestMethod()> Public Sub DisposeTest()
        Dim aValuesToAdd() As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim target As IDisposable = New atcCollection(aValuesToAdd) ' TODO: Initialize to an appropriate value
        target.Dispose()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test Increment</summary>
    <TestMethod()> Public Sub IncrementTest()
        Dim aValuesToAdd() As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcCollection = New atcCollection(aValuesToAdd) ' TODO: Initialize to an appropriate value
        Dim aKey As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim actual As Double
        actual = target.Increment(aKey)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test Increment</summary>
    <TestMethod()> Public Sub IncrementTest1()
        Dim aValuesToAdd() As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcCollection = New atcCollection(aValuesToAdd) ' TODO: Initialize to an appropriate value
        Dim aKey As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim aValue As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim expected As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim actual As Double
        actual = target.Increment(aKey, aValue)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test IndexFromKey</summary>
    <TestMethod()> Public Sub IndexFromKeyTest()
        Dim aValuesToAdd() As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcCollection = New atcCollection(aValuesToAdd) ' TODO: Initialize to an appropriate value
        Dim akey As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        actual = target.IndexFromKey(akey)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test Insert</summary>
    <TestMethod()> Public Sub InsertTest()
        Dim aValuesToAdd() As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcCollection = New atcCollection(aValuesToAdd) ' TODO: Initialize to an appropriate value
        Dim aIndex As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aValue As Object = Nothing ' TODO: Initialize to an appropriate value
        target.Insert(aIndex, aValue)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
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
