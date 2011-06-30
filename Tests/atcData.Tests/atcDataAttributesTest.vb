Imports System.Collections

Imports atcUtility

Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports atcData



'''<summary>
'''This is a test class for atcDataAttributesTest and is intended
'''to contain all atcDataAttributesTest Unit Tests
'''</summary>
<TestClass()> _
Public Class atcDataAttributesTest


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
    '''A test for atcDataAttributes Constructor
    '''</summary>
    <TestMethod()> _
    Public Sub atcDataAttributesConstructorTest()
        Dim target As atcDataAttributes = New atcDataAttributes()
        Assert.Inconclusive("TODO: Implement code to verify target")
    End Sub

    '''<summary>
    '''A test for Add
    '''</summary>
    <TestMethod()> _
    Public Sub AddTest()
        Dim target As atcDataAttributes = New atcDataAttributes() ' TODO: Initialize to an appropriate value
        Dim aAttributeName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aAttributeValue As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        actual = target.Add(aAttributeName, aAttributeValue)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for Add
    '''</summary>
    <TestMethod()> _
    Public Sub AddTest1()
        Dim target As atcDataAttributes = New atcDataAttributes() ' TODO: Initialize to an appropriate value
        Dim aDefinedValue As atcDefinedValue = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        actual = target.Add(aDefinedValue)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for AddDefinition
    '''</summary>
    <TestMethod()> _
    Public Sub AddDefinitionTest()
        Dim aDefinition As atcAttributeDefinition = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As atcAttributeDefinition = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcAttributeDefinition
        actual = atcDataAttributes.AddDefinition(aDefinition)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for AddHistory
    '''</summary>
    <TestMethod()> _
    Public Sub AddHistoryTest()
        Dim target As atcDataAttributes = New atcDataAttributes() ' TODO: Initialize to an appropriate value
        Dim aNewEvent As String = String.Empty ' TODO: Initialize to an appropriate value
        target.AddHistory(aNewEvent)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for AllDefinitions
    '''</summary>
    <TestMethod()> _
    Public Sub AllDefinitionsTest()
        Dim expected As atcCollection = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcCollection
        actual = atcDataAttributes.AllDefinitions
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for AttributeNameToKey
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub AttributeNameToKeyTest()
        Dim aAttributeName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = atcDataAttributes_Accessor.AttributeNameToKey(aAttributeName)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for CalculateAll
    '''</summary>
    <TestMethod()> _
    Public Sub CalculateAllTest()
        Dim target As atcDataAttributes = New atcDataAttributes() ' TODO: Initialize to an appropriate value
        target.CalculateAll()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for ChangeTo
    '''</summary>
    <TestMethod()> _
    Public Sub ChangeToTest()
        Dim target As atcDataAttributes = New atcDataAttributes() ' TODO: Initialize to an appropriate value
        Dim aNewItems As atcDataAttributes = Nothing ' TODO: Initialize to an appropriate value
        target.ChangeTo(aNewItems)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for Clone
    '''</summary>
    <TestMethod()> _
    Public Sub CloneTest()
        Dim target As atcDataAttributes = New atcDataAttributes() ' TODO: Initialize to an appropriate value
        Dim expected As atcDataAttributes = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcDataAttributes
        actual = target.Clone
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for ContainsAttribute
    '''</summary>
    <TestMethod()> _
    Public Sub ContainsAttributeTest()
        Dim target As atcDataAttributes = New atcDataAttributes() ' TODO: Initialize to an appropriate value
        Dim aAttributeName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = target.ContainsAttribute(aAttributeName)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for DiscardCalculated
    '''</summary>
    <TestMethod()> _
    Public Sub DiscardCalculatedTest()
        Dim target As atcDataAttributes = New atcDataAttributes() ' TODO: Initialize to an appropriate value
        target.DiscardCalculated()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for GetDefinedValue
    '''</summary>
    <TestMethod()> _
    Public Sub GetDefinedValueTest()
        Dim target As atcDataAttributes = New atcDataAttributes() ' TODO: Initialize to an appropriate value
        Dim aAttributeName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As atcDefinedValue = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcDefinedValue
        actual = target.GetDefinedValue(aAttributeName)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for GetDefinition
    '''</summary>
    <TestMethod()> _
    Public Sub GetDefinitionTest()
        Dim aAttributeName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aCreate As Boolean = False ' TODO: Initialize to an appropriate value
        Dim expected As atcAttributeDefinition = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcAttributeDefinition
        actual = atcDataAttributes.GetDefinition(aAttributeName, aCreate)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for GetFormattedValue
    '''</summary>
    <TestMethod()> _
    Public Sub GetFormattedValueTest()
        Dim target As atcDataAttributes = New atcDataAttributes() ' TODO: Initialize to an appropriate value
        Dim aAttributeName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aDefault As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = target.GetFormattedValue(aAttributeName, aDefault)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for GetValue
    '''</summary>
    <TestMethod()> _
    Public Sub GetValueTest()
        Dim target As atcDataAttributes = New atcDataAttributes() ' TODO: Initialize to an appropriate value
        Dim aAttributeName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aDefault As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Object
        actual = target.GetValue(aAttributeName, aDefault)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for IsSimple
    '''</summary>
    <TestMethod()> _
    Public Sub IsSimpleTest()
        Dim aDef As atcAttributeDefinition = Nothing ' TODO: Initialize to an appropriate value
        Dim aKey As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aOperation As atcDefinedValue = Nothing ' TODO: Initialize to an appropriate value
        Dim aOperationExpected As atcDefinedValue = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = atcDataAttributes.IsSimple(aDef, aKey, aOperation)
        Assert.AreEqual(aOperationExpected, aOperation)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for PreferredName
    '''</summary>
    <TestMethod()> _
    Public Sub PreferredNameTest()
        Dim aAttributeName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aAttributeNameExpected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = atcDataAttributes.PreferredName(aAttributeName)
        Assert.AreEqual(aAttributeNameExpected, aAttributeName)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for RemoveByKey
    '''</summary>
    <TestMethod()> _
    Public Sub RemoveByKeyTest()
        Dim target As atcDataAttributes = New atcDataAttributes() ' TODO: Initialize to an appropriate value
        Dim aAttributeName As Object = Nothing ' TODO: Initialize to an appropriate value
        target.RemoveByKey(aAttributeName)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for SetValue
    '''</summary>
    <TestMethod()> _
    Public Sub SetValueTest()
        Dim target As atcDataAttributes = New atcDataAttributes() ' TODO: Initialize to an appropriate value
        Dim aAttributeName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aAttributeValue As Object = Nothing ' TODO: Initialize to an appropriate value
        target.SetValue(aAttributeName, aAttributeValue)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for SetValue
    '''</summary>
    <TestMethod()> _
    Public Sub SetValueTest1()
        Dim target As atcDataAttributes = New atcDataAttributes() ' TODO: Initialize to an appropriate value
        Dim aAttrDefinition As atcAttributeDefinition = Nothing ' TODO: Initialize to an appropriate value
        Dim aValue As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim aArguments As atcDataAttributes = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        actual = target.SetValue(aAttrDefinition, aValue, aArguments)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for SetValueIfMissing
    '''</summary>
    <TestMethod()> _
    Public Sub SetValueIfMissingTest()
        Dim target As atcDataAttributes = New atcDataAttributes() ' TODO: Initialize to an appropriate value
        Dim aAttributeName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aAttributeValue As Object = Nothing ' TODO: Initialize to an appropriate value
        target.SetValueIfMissing(aAttributeName, aAttributeValue)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for ToString
    '''</summary>
    <TestMethod()> _
    Public Sub ToStringTest()
        Dim target As atcDataAttributes = New atcDataAttributes() ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = target.ToString
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for ValuesSortedByName
    '''</summary>
    <TestMethod()> _
    Public Sub ValuesSortedByNameTest()
        Dim target As atcDataAttributes = New atcDataAttributes() ' TODO: Initialize to an appropriate value
        Dim expected As SortedList = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As SortedList
        actual = target.ValuesSortedByName
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for Item
    '''</summary>
    <TestMethod()> _
    Public Sub ItemTest()
        Dim target As atcDataAttributes = New atcDataAttributes() ' TODO: Initialize to an appropriate value
        Dim index As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim expected As atcDefinedValue = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcDefinedValue
        target(index) = expected
        actual = target(index)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for ItemByIndex
    '''</summary>
    <TestMethod()> _
    Public Sub ItemByIndexTest()
        Dim target As atcDataAttributes = New atcDataAttributes() ' TODO: Initialize to an appropriate value
        Dim index As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim expected As atcDefinedValue = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcDefinedValue
        target.ItemByIndex(index) = expected
        actual = target.ItemByIndex(index)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for Owner
    '''</summary>
    <TestMethod()> _
    Public Sub OwnerTest()
        Dim target As atcDataAttributes = New atcDataAttributes() ' TODO: Initialize to an appropriate value
        Dim expected As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Object
        target.Owner = expected
        actual = target.Owner
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub
End Class
