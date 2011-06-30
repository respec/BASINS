Imports System.Collections
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports atcData

'''<summary>
'''This is a test class for atcAttributeDefinitionTest and is intended
'''to contain all atcAttributeDefinitionTest Unit Tests
'''</summary>
<TestClass()> _
Public Class atcAttributeDefinitionTest
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
            testContextInstance = value
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

    '''<summary>Test atcAttributeDefinition Constructor</summary>
    <TestMethod()> Public Sub atcAttributeDefinitionConstructorTest()
        Dim target As atcAttributeDefinition = New atcAttributeDefinition()
        Assert.Inconclusive("TODO: Implement code to verify target")
    End Sub

    '''<summary>Test Clear</summary>
    <TestMethod()> Public Sub ClearTest()
        Dim target As atcAttributeDefinition = New atcAttributeDefinition() ' TODO: Initialize to an appropriate value
        target.Clear()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test Clone</summary>
    <TestMethod()> Public Sub CloneTest()
        Dim target As atcAttributeDefinition = New atcAttributeDefinition() ' TODO: Initialize to an appropriate value
        Dim aNewName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aNewDescription As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As atcAttributeDefinition = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcAttributeDefinition
        actual = target.Clone(aNewName, aNewDescription)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test IsNumeric</summary>
    <TestMethod()> Public Sub IsNumericTest()
        Dim target As atcAttributeDefinition = New atcAttributeDefinition() ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = target.IsNumeric
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test ToString</summary>
    <TestMethod()> Public Sub ToStringTest()
        Dim target As atcAttributeDefinition = New atcAttributeDefinition() ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = target.ToString
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test Calculated</summary>
    <TestMethod()> Public Sub CalculatedTest()
        Dim target As atcAttributeDefinition = New atcAttributeDefinition() ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = target.Calculated
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test Calculator</summary>
    <TestMethod()> Public Sub CalculatorTest()
        Dim target As atcAttributeDefinition = New atcAttributeDefinition() ' TODO: Initialize to an appropriate value
        Dim expected As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcTimeseriesSource
        target.Calculator = expected
        actual = target.Calculator
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test Category</summary>
    <TestMethod()> Public Sub CategoryTest()
        Dim target As atcAttributeDefinition = New atcAttributeDefinition() ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        target.Category = expected
        actual = target.Category
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test CopiesInherit</summary>
    <TestMethod()> Public Sub CopiesInheritTest()
        Dim target As atcAttributeDefinition = New atcAttributeDefinition() ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        target.CopiesInherit = expected
        actual = target.CopiesInherit
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test DefaultValue</summary>
    <TestMethod()> Public Sub DefaultValueTest()
        Dim target As atcAttributeDefinition = New atcAttributeDefinition() ' TODO: Initialize to an appropriate value
        Dim expected As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Object
        target.DefaultValue = expected
        actual = target.DefaultValue
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test Description</summary>
    <TestMethod()> Public Sub DescriptionTest()
        Dim target As atcAttributeDefinition = New atcAttributeDefinition() ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        target.Description = expected
        actual = target.Description
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test Editable</summary>
    <TestMethod()> Public Sub EditableTest()
        Dim target As atcAttributeDefinition = New atcAttributeDefinition() ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        target.Editable = expected
        actual = target.Editable
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test Help</summary>
    <TestMethod()> Public Sub HelpTest()
        Dim target As atcAttributeDefinition = New atcAttributeDefinition() ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        target.Help = expected
        actual = target.Help
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test ID</summary>
    <TestMethod()> Public Sub IDTest()
        Dim target As atcAttributeDefinition = New atcAttributeDefinition() ' TODO: Initialize to an appropriate value
        Dim expected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        target.ID = expected
        actual = target.ID
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test Max</summary>
    <TestMethod()> Public Sub MaxTest()
        Dim target As atcAttributeDefinition = New atcAttributeDefinition() ' TODO: Initialize to an appropriate value
        Dim expected As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim actual As Double
        target.Max = expected
        actual = target.Max
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test Min</summary>
    <TestMethod()> Public Sub MinTest()
        Dim target As atcAttributeDefinition = New atcAttributeDefinition() ' TODO: Initialize to an appropriate value
        Dim expected As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim actual As Double
        target.Min = expected
        actual = target.Min
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test Name</summary>
    <TestMethod()> Public Sub NameTest()
        Dim target As atcAttributeDefinition = New atcAttributeDefinition() ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        target.Name = expected
        actual = target.Name
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test TypeString</summary>
    <TestMethod()> Public Sub TypeStringTest()
        Dim target As atcAttributeDefinition = New atcAttributeDefinition() ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        target.TypeString = expected
        actual = target.TypeString
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test ValidList</summary>
    <TestMethod()> Public Sub ValidListTest()
        Dim target As atcAttributeDefinition = New atcAttributeDefinition() ' TODO: Initialize to an appropriate value
        Dim expected As ArrayList = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ArrayList
        target.ValidList = expected
        actual = target.ValidList
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub
End Class
