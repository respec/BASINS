Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports atcData

'''<summary>
'''This is a test class for atcDefinedValueTest and is intended
'''to contain all atcDefinedValueTest Unit Tests (Done)
'''</summary>
<TestClass()> _
Public Class atcDefinedValueTest
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

    '''<summary>Test atcDefinedValue Constructor</summary>
    <TestMethod()> Public Sub atcDefinedValueConstructorTest()
        Dim aDefinition As atcAttributeDefinition = Nothing ' TODO: Initialize to an appropriate value
        Dim aValue As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim aArguments As atcDataAttributes = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcDefinedValue = New atcDefinedValue(aDefinition, aValue, aArguments)
        Assert.IsInstanceOfType(target, GetType(atcDefinedValue))
    End Sub

    '''<summary>Test atcDefinedValue Constructor</summary>
    <TestMethod()> Public Sub atcDefinedValueConstructorTest1()
        Dim target As atcDefinedValue = New atcDefinedValue()
        Assert.IsInstanceOfType(target, GetType(atcDefinedValue))
    End Sub

    '''<summary>Test Clone</summary>
    <TestMethod()> Public Sub CloneTest()
        Dim target As atcDefinedValue = New atcDefinedValue() ' TODO: Initialize to an appropriate value
        Dim actual As atcDefinedValue
        actual = target.Clone
        Assert.AreEqual(target.Definition, actual.Definition)
        Assert.AreEqual(target.Value, actual.Value)
        Assert.AreEqual(target.Arguments, actual.Arguments)
    End Sub

    '''<summary>Test ToString</summary>
    <TestMethod()> Public Sub ToStringTest()
        Dim target As atcDefinedValue = New atcDefinedValue() ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = target.ToString
        Assert.AreNotEqual(expected, actual)
    End Sub
End Class
