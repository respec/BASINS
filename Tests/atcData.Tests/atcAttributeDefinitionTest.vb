Imports System.Collections
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports atcData

'''<summary>
'''This is a test class for atcAttributeDefinitionTest and is intended
'''to contain all atcAttributeDefinitionTest Unit Tests (Done)
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
        Assert.AreEqual(target.TypeString, "String")
        Assert.AreEqual(target.ID, 0)
        Assert.AreEqual(target.DefaultValue, Nothing)
        Assert.AreEqual(target.ValidList.Count, 0)
    End Sub

    '''<summary>Test Clear</summary>
    <TestMethod()> Public Sub ClearTest()
        Dim target As atcAttributeDefinition = New atcAttributeDefinition()
        Assert.AreEqual(target.TypeString, "String")
        Assert.AreEqual(target.ID, 0)
        Assert.AreEqual(target.DefaultValue, Nothing)
        Assert.AreEqual(target.ValidList.Count, 0)
    End Sub

    '''<summary>Test Clone</summary>
    <TestMethod()> Public Sub CloneTest()
        Dim target As atcAttributeDefinition = New atcAttributeDefinition()
        Dim lclone As atcAttributeDefinition = target.Clone()
        Assert.AreEqual(target.TypeString, lclone.TypeString)
        Assert.AreEqual(target.ID, lclone.ID)
        Assert.AreEqual(target.DefaultValue, lclone.DefaultValue)
        Assert.AreEqual(target.ValidList.Count, lclone.ValidList.Count)
    End Sub

    '''<summary>Test IsNumeric</summary>
    <TestMethod()> Public Sub IsNumericTest()
        Dim target As atcAttributeDefinition = New atcAttributeDefinition() ' TODO: Initialize to an appropriate value
        target.TypeString = "integer"
        Assert.AreEqual(target.IsNumeric, True)
        target.TypeString = "single"
        Assert.AreEqual(target.IsNumeric, True)
        target.TypeString = "double"
        Assert.AreEqual(target.IsNumeric, True)
        target.TypeString = "string"
        Assert.AreEqual(target.IsNumeric, False)
    End Sub

    '''<summary>Test ToString</summary>
    <TestMethod()> Public Sub ToStringTest()
        Dim target As atcAttributeDefinition = New atcAttributeDefinition() ' TODO: Initialize to an appropriate value
        Assert.AreNotEqual(target.ToString.Length, 0)
    End Sub

    '''<summary>Test Calculated</summary>
    <TestMethod()> Public Sub CalculatedTest()
        Dim target As atcAttributeDefinition = New atcAttributeDefinition() ' TODO: Initialize to an appropriate value
        Assert.AreEqual(target.Calculated, False)
        target.Calculator = New atcTimeseriesSource
        Assert.AreEqual(target.Calculated, True)
    End Sub

    '''<summary>Test Calculator</summary>
    <TestMethod()> Public Sub CalculatorTest()
        Dim target As atcAttributeDefinition = New atcAttributeDefinition() ' TODO: Initialize to an appropriate value
        Assert.AreEqual(target.Calculator, Nothing)
    End Sub

    '''<summary>Test Category</summary>
    <TestMethod()> Public Sub CategoryTest()
        Dim target As atcAttributeDefinition = New atcAttributeDefinition() ' TODO: Initialize to an appropriate value
        Assert.AreEqual(target.Category, "")
    End Sub

    '''<summary>Test CopiesInherit</summary>
    <TestMethod()> Public Sub CopiesInheritTest()
        Dim target As atcAttributeDefinition = New atcAttributeDefinition() ' TODO: Initialize to an appropriate value
        Assert.IsInstanceOfType(target.CopiesInherit, GetType(Boolean))
    End Sub

    '''<summary>Test DefaultValue</summary>
    <TestMethod()> Public Sub DefaultValueTest()
        Dim target As atcAttributeDefinition = New atcAttributeDefinition() ' TODO: Initialize to an appropriate value
        Assert.AreEqual(target.DefaultValue, Nothing)
        target.DefaultValue = New Object
        Assert.IsInstanceOfType(target.DefaultValue, GetType(Object))
        target.DefaultValue = New atcTimeseries(Nothing)
        Assert.IsInstanceOfType(target.DefaultValue, GetType(atcTimeseries))
    End Sub

    '''<summary>Test Description</summary>
    <TestMethod()> Public Sub DescriptionTest()
        Dim target As atcAttributeDefinition = New atcAttributeDefinition() ' TODO: Initialize to an appropriate value
        target.Description = "Test 1"
        Assert.AreEqual(target.Description, "Test 1")
    End Sub

    '''<summary>Test Editable</summary>
    <TestMethod()> Public Sub EditableTest()
        Dim target As atcAttributeDefinition = New atcAttributeDefinition()
        target.Editable = False
        Assert.AreEqual(target.Editable, False)
        target.Editable = True
        Assert.AreEqual(target.Editable, True)
    End Sub

    '''<summary>Test Help</summary>
    <TestMethod()> Public Sub HelpTest()
        Dim target As atcAttributeDefinition = New atcAttributeDefinition() ' TODO: Initialize to an appropriate value
        target.Help = "Test 1"
        Assert.AreEqual(target.Help, "Test 1")
    End Sub

    '''<summary>Test ID</summary>
    <TestMethod()> Public Sub IDTest()
        Dim target As atcAttributeDefinition = New atcAttributeDefinition() ' TODO: Initialize to an appropriate value
        target.ID = 1
        Assert.AreEqual(target.ID, 1)
    End Sub

    '''<summary>Test Max</summary>
    <TestMethod()> Public Sub MaxTest()
        Dim target As atcAttributeDefinition = New atcAttributeDefinition() ' TODO: Initialize to an appropriate value
        target.Max = Double.MaxValue
        Assert.AreEqual(target.Max, Double.MaxValue)
    End Sub

    '''<summary>Test Min</summary>
    <TestMethod()> Public Sub MinTest()
        Dim target As atcAttributeDefinition = New atcAttributeDefinition() ' TODO: Initialize to an appropriate value
        target.Min = Double.MinValue
        Assert.AreEqual(target.Min, Double.MinValue)
    End Sub

    '''<summary>Test Name</summary>
    <TestMethod()> Public Sub NameTest()
        Dim target As atcAttributeDefinition = New atcAttributeDefinition() ' TODO: Initialize to an appropriate value
        target.Name = "Test 1"
        Assert.AreEqual(target.Name, "Test 1")
    End Sub

    '''<summary>Test TypeString</summary>
    <TestMethod()> Public Sub TypeStringTest()
        Dim target As atcAttributeDefinition = New atcAttributeDefinition() ' TODO: Initialize to an appropriate value
        Assert.AreEqual(target.TypeString, "String")
        target.TypeString = "Test 1"
        Assert.AreEqual(target.TypeString, "Test 1")
    End Sub

    '''<summary>Test ValidList</summary>
    <TestMethod()> Public Sub ValidListTest()
        Dim target As atcAttributeDefinition = New atcAttributeDefinition()
        Assert.IsInstanceOfType(target.ValidList, GetType(ArrayList))
        target.ValidList = New ArrayList(10)
        Assert.AreEqual(target.ValidList.Count, 0)
    End Sub
End Class
