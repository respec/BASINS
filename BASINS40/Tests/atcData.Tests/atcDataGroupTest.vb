Imports System.Collections

Imports atcUtility

Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports atcData



'''<summary>
'''This is a test class for atcDataGroupTest and is intended
'''to contain all atcDataGroupTest Unit Tests
'''</summary>
<TestClass()> _
Public Class atcDataGroupTest


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


    '''<summary>
    '''A test for atcDataGroup Constructor
    '''</summary>
    <TestMethod()> _
    Public Sub atcDataGroupConstructorTest()
        Dim target As atcDataGroup = New atcDataGroup()
        Assert.Inconclusive("TODO: Implement code to verify target")
    End Sub

    '''<summary>
    '''A test for atcDataGroup Constructor
    '''</summary>
    <TestMethod()> _
    Public Sub atcDataGroupConstructorTest1()
        Dim aDataSet As atcDataSet = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcDataGroup = New atcDataGroup(aDataSet)
        Assert.Inconclusive("TODO: Implement code to verify target")
    End Sub

    '''<summary>
    '''A test for atcDataGroup Constructor
    '''</summary>
    <TestMethod()> _
    Public Sub atcDataGroupConstructorTest2()
        Dim aDataSets() As atcDataSet = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcDataGroup = New atcDataGroup(aDataSets)
        Assert.Inconclusive("TODO: Implement code to verify target")
    End Sub

    '''<summary>
    '''A test for Add
    '''</summary>
    <TestMethod()> _
    Public Sub AddTest()
        Dim target As atcDataGroup = New atcDataGroup() ' TODO: Initialize to an appropriate value
        Dim aKey As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim aDataSet As atcDataSet = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        actual = target.Add(aKey, aDataSet)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for Add
    '''</summary>
    <TestMethod()> _
    Public Sub AddTest1()
        Dim target As atcDataGroup = New atcDataGroup() ' TODO: Initialize to an appropriate value
        Dim aDataSet As atcDataSet = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        actual = target.Add(aDataSet)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for Add
    '''</summary>
    <TestMethod()> _
    Public Sub AddTest2()
        Dim target As atcDataGroup = New atcDataGroup() ' TODO: Initialize to an appropriate value
        Dim aAddThese As atcCollection = Nothing ' TODO: Initialize to an appropriate value
        target.Add(aAddThese)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for AddRange
    '''</summary>
    <TestMethod()> _
    Public Sub AddRangeTest()
        Dim target As atcDataGroup = New atcDataGroup() ' TODO: Initialize to an appropriate value
        Dim aAddThese As IEnumerable = Nothing ' TODO: Initialize to an appropriate value
        target.AddRange(aAddThese)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for AddRange
    '''</summary>
    <TestMethod()> _
    Public Sub AddRangeTest1()
        Dim target As atcDataGroup = New atcDataGroup() ' TODO: Initialize to an appropriate value
        Dim aKeys As IEnumerable = Nothing ' TODO: Initialize to an appropriate value
        Dim aValues As IEnumerable = Nothing ' TODO: Initialize to an appropriate value
        target.AddRange(aKeys, aValues)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for ChangeTo
    '''</summary>
    <TestMethod()> _
    Public Sub ChangeToTest()
        Dim target As atcDataGroup = New atcDataGroup() ' TODO: Initialize to an appropriate value
        Dim aNewGroup As atcDataGroup = Nothing ' TODO: Initialize to an appropriate value
        target.ChangeTo(aNewGroup)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for Clear
    '''</summary>
    <TestMethod()> _
    Public Sub ClearTest()
        Dim target As atcDataGroup = New atcDataGroup() ' TODO: Initialize to an appropriate value
        target.Clear()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for Clone
    '''</summary>
    <TestMethod()> _
    Public Sub CloneTest()
        Dim target As atcDataGroup = New atcDataGroup() ' TODO: Initialize to an appropriate value
        Dim expected As atcDataGroup = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcDataGroup
        actual = target.Clone
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for CommonAttributeValue
    '''</summary>
    <TestMethod()> _
    Public Sub CommonAttributeValueTest()
        Dim target As atcDataGroup = New atcDataGroup() ' TODO: Initialize to an appropriate value
        Dim aAttributeName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aMissingValue As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Object
        actual = target.CommonAttributeValue(aAttributeName, aMissingValue)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for Dispose
    '''</summary>
    <TestMethod()> _
    Public Sub DisposeTest()
        Dim target As atcDataGroup = New atcDataGroup() ' TODO: Initialize to an appropriate value
        target.Dispose()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for FindData
    '''</summary>
    <TestMethod()> _
    Public Sub FindDataTest()
        Dim target As atcDataGroup = New atcDataGroup() ' TODO: Initialize to an appropriate value
        Dim aAttributeName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aValues As atcCollection = Nothing ' TODO: Initialize to an appropriate value
        Dim aLimit As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim expected As atcDataGroup = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcDataGroup
        actual = target.FindData(aAttributeName, aValues, aLimit)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for FindData
    '''</summary>
    <TestMethod()> _
    Public Sub FindDataTest1()
        Dim target As atcDataGroup = New atcDataGroup() ' TODO: Initialize to an appropriate value
        Dim aAttributeName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aValue As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aLimit As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim expected As atcDataGroup = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcDataGroup
        actual = target.FindData(aAttributeName, aValue, aLimit)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for IndexOfSerial
    '''</summary>
    <TestMethod()> _
    Public Sub IndexOfSerialTest()
        Dim target As atcDataGroup = New atcDataGroup() ' TODO: Initialize to an appropriate value
        Dim aSerial As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim expected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        actual = target.IndexOfSerial(aSerial)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for Insert
    '''</summary>
    <TestMethod()> _
    Public Sub InsertTest()
        Dim target As atcDataGroup = New atcDataGroup() ' TODO: Initialize to an appropriate value
        Dim aIndex As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aDataSet As atcDataSet = Nothing ' TODO: Initialize to an appropriate value
        target.Insert(aIndex, aDataSet)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for RaiseAddedOne
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub RaiseAddedOneTest()
        Dim target As atcDataGroup_Accessor = New atcDataGroup_Accessor() ' TODO: Initialize to an appropriate value
        Dim aDataSet As atcDataSet = Nothing ' TODO: Initialize to an appropriate value
        target.RaiseAddedOne(aDataSet)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for RaiseRemovedOne
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub RaiseRemovedOneTest()
        Dim target As atcDataGroup_Accessor = New atcDataGroup_Accessor() ' TODO: Initialize to an appropriate value
        Dim aDataSet As atcDataSet = Nothing ' TODO: Initialize to an appropriate value
        target.RaiseRemovedOne(aDataSet)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for Remove
    '''</summary>
    <TestMethod()> _
    Public Sub RemoveTest()
        Dim target As atcDataGroup = New atcDataGroup() ' TODO: Initialize to an appropriate value
        Dim aRemoveThese As atcCollection = Nothing ' TODO: Initialize to an appropriate value
        target.Remove(aRemoveThese)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for Remove
    '''</summary>
    <TestMethod()> _
    Public Sub RemoveTest1()
        Dim target As atcDataGroup = New atcDataGroup() ' TODO: Initialize to an appropriate value
        Dim aDataSet As atcDataSet = Nothing ' TODO: Initialize to an appropriate value
        target.Remove(aDataSet)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for RemoveAt
    '''</summary>
    <TestMethod()> _
    Public Sub RemoveAtTest()
        Dim target As atcDataGroup = New atcDataGroup() ' TODO: Initialize to an appropriate value
        Dim aIndex As Integer = 0 ' TODO: Initialize to an appropriate value
        target.RemoveAt(aIndex)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for RemoveRange
    '''</summary>
    <TestMethod()> _
    Public Sub RemoveRangeTest()
        Dim target As atcDataGroup = New atcDataGroup() ' TODO: Initialize to an appropriate value
        Dim aIndex As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aNumber As Integer = 0 ' TODO: Initialize to an appropriate value
        target.RemoveRange(aIndex, aNumber)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for SortedAttributeValues
    '''</summary>
    <TestMethod()> _
    Public Sub SortedAttributeValuesTest()
        Dim target As atcDataGroup = New atcDataGroup() ' TODO: Initialize to an appropriate value
        Dim aAttributeDefinition As atcAttributeDefinition = Nothing ' TODO: Initialize to an appropriate value
        Dim aMissingValue As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As atcCollection = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcCollection
        actual = target.SortedAttributeValues(aAttributeDefinition, aMissingValue)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for SortedAttributeValues
    '''</summary>
    <TestMethod()> _
    Public Sub SortedAttributeValuesTest1()
        Dim target As atcDataGroup = New atcDataGroup() ' TODO: Initialize to an appropriate value
        Dim aAttributeName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aMissingValue As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As atcCollection = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcCollection
        actual = target.SortedAttributeValues(aAttributeName, aMissingValue)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for ToString
    '''</summary>
    <TestMethod()> _
    Public Sub ToStringTest()
        Dim target As atcDataGroup = New atcDataGroup() ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = target.ToString
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for __ENCAddToList
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub __ENCAddToListTest()
        Dim value As Object = Nothing ' TODO: Initialize to an appropriate value
        atcDataGroup_Accessor.__ENCAddToList(value)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for Item
    '''</summary>
    <TestMethod()> _
    Public Sub ItemTest()
        Dim target As atcDataGroup = New atcDataGroup() ' TODO: Initialize to an appropriate value
        Dim aIndex As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim expected As atcDataSet = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcDataSet
        target(aIndex) = expected
        actual = target(aIndex)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for ItemByIndex
    '''</summary>
    <TestMethod()> _
    Public Sub ItemByIndexTest()
        Dim target As atcDataGroup = New atcDataGroup() ' TODO: Initialize to an appropriate value
        Dim aIndex As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim expected As atcDataSet = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcDataSet
        target.ItemByIndex(aIndex) = expected
        actual = target.ItemByIndex(aIndex)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for ItemByKey
    '''</summary>
    <TestMethod()> _
    Public Sub ItemByKeyTest()
        Dim target As atcDataGroup = New atcDataGroup() ' TODO: Initialize to an appropriate value
        Dim aKey As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As atcDataSet = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcDataSet
        target.ItemByKey(aKey) = expected
        actual = target.ItemByKey(aKey)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for SelectedData
    '''</summary>
    <TestMethod()> _
    Public Sub SelectedDataTest()
        Dim target As atcDataGroup = New atcDataGroup() ' TODO: Initialize to an appropriate value
        Dim expected As atcDataGroup = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcDataGroup
        target.SelectedData = expected
        actual = target.SelectedData
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub
End Class
