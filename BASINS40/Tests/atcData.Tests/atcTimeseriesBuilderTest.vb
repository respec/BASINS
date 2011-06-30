Imports atcUtility
Imports System
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports atcData

'''<summary>
'''This is a test class for atcTimeseriesBuilderTest and is intended
'''to contain all atcTimeseriesBuilderTest Unit Tests
'''</summary>
<TestClass()> _
Public Class atcTimeseriesBuilderTest
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

    '''<summary>Test atcTimeseriesBuilder Constructor</summary>
    <TestMethod()> Public Sub atcTimeseriesBuilderConstructorTest()
        Dim aDataSource As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseriesBuilder = New atcTimeseriesBuilder(aDataSource)
        Assert.Inconclusive("TODO: Implement code to verify target")
    End Sub

    '''<summary>Test AddValue</summary>
    <TestMethod()> Public Sub AddValueTest()
        Dim aDataSource As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseriesBuilder = New atcTimeseriesBuilder(aDataSource) ' TODO: Initialize to an appropriate value
        Dim aDate As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim aValue As String = String.Empty ' TODO: Initialize to an appropriate value
        target.AddValue(aDate, aValue)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test AddValue</summary>
    <TestMethod()> Public Sub AddValueTest1()
        Dim aDataSource As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseriesBuilder = New atcTimeseriesBuilder(aDataSource) ' TODO: Initialize to an appropriate value
        Dim aDate As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim aValue As Double = 0.0! ' TODO: Initialize to an appropriate value
        target.AddValue(aDate, aValue)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test AddValue</summary>
    <TestMethod()> Public Sub AddValueTest2()
        Dim aDataSource As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseriesBuilder = New atcTimeseriesBuilder(aDataSource) ' TODO: Initialize to an appropriate value
        Dim aDate As DateTime = New DateTime() ' TODO: Initialize to an appropriate value
        Dim aValue As Double = 0.0! ' TODO: Initialize to an appropriate value
        target.AddValue(aDate, aValue)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test AddValueAttribute</summary>
    <TestMethod()> Public Sub AddValueAttributeTest()
        Dim aDataSource As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseriesBuilder = New atcTimeseriesBuilder(aDataSource) ' TODO: Initialize to an appropriate value
        Dim aAttributeName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aAttributeValue As Object = Nothing ' TODO: Initialize to an appropriate value
        target.AddValueAttribute(aAttributeName, aAttributeValue)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test AddValueAttributes</summary>
    <TestMethod()> Public Sub AddValueAttributesTest()
        Dim aDataSource As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseriesBuilder = New atcTimeseriesBuilder(aDataSource) ' TODO: Initialize to an appropriate value
        Dim aAttributes As atcDataAttributes = Nothing ' TODO: Initialize to an appropriate value
        target.AddValueAttributes(aAttributes)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test CreateTimeseries</summary>
    <TestMethod()> Public Sub CreateTimeseriesTest()
        Dim aDataSource As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseriesBuilder = New atcTimeseriesBuilder(aDataSource) ' TODO: Initialize to an appropriate value
        Dim expected As atcTimeseries = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcTimeseries
        actual = target.CreateTimeseries
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test NumValues</summary>
    <TestMethod()> Public Sub NumValuesTest()
        Dim aDataSource As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseriesBuilder = New atcTimeseriesBuilder(aDataSource) ' TODO: Initialize to an appropriate value
        Dim expected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        actual = target.NumValues
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test Restart</summary>
    <TestMethod()> Public Sub RestartTest()
        Dim aDataSource As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseriesBuilder = New atcTimeseriesBuilder(aDataSource) ' TODO: Initialize to an appropriate value
        target.Restart()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test Attributes</summary>
    <TestMethod()> Public Sub AttributesTest()
        Dim aDataSource As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseriesBuilder = New atcTimeseriesBuilder(aDataSource) ' TODO: Initialize to an appropriate value
        Dim actual As atcDataAttributes
        actual = target.Attributes
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test LogDateFormat</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub LogDateFormatTest()
        Dim param0 As PrivateObject = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseriesBuilder_Accessor = New atcTimeseriesBuilder_Accessor(param0) ' TODO: Initialize to an appropriate value
        Dim actual As atcDateFormat
        actual = target.LogDateFormat
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test LogNonNumeric</summary>
    <TestMethod()> Public Sub LogNonNumericTest()
        Dim aDataSource As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseriesBuilder = New atcTimeseriesBuilder(aDataSource) ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        target.LogNonNumeric = expected
        actual = target.LogNonNumeric
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub
End Class
