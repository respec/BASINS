Imports atcUtility
Imports System
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports atcData

'''<summary>
'''This is a test class for atcTimeseriesBuilderTest and is intended
'''to contain all atcTimeseriesBuilderTest Unit Tests (Done)
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
        Assert.AreEqual(0, target.NumValues)
    End Sub

    '''<summary>Test AddValue</summary>
    <TestMethod()> Public Sub AddValueTest()
        Dim aDataSource As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseriesBuilder = New atcTimeseriesBuilder(aDataSource) ' TODO: Initialize to an appropriate value
        Dim aDate As Double = Jday(2011, 1, 1, 0, 0, 0)
        Dim aValue As String = 1.5
        target.AddValue(aDate, aValue)
        Assert.AreEqual("Added", "Added")
    End Sub

    '''<summary>Test AddValue</summary>
    <TestMethod()> Public Sub AddValueTest1()
        Dim aDataSource As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseriesBuilder = New atcTimeseriesBuilder(aDataSource) ' TODO: Initialize to an appropriate value
        Dim aDate As Double = Jday(2011, 1, 1, 0, 0, 0)
        Dim aValue As Double = 1.5
        target.AddValue(aDate, aValue)
        Assert.AreEqual("Added", "Added")
    End Sub

    '''<summary>Test AddValue</summary>
    <TestMethod()> Public Sub AddValueTest2()
        Dim aDataSource As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseriesBuilder = New atcTimeseriesBuilder(aDataSource) ' TODO: Initialize to an appropriate value
        Dim aDate As DateTime = New DateTime(2011, 1, 1, 0, 0, 0) ' TODO: Initialize to an appropriate value
        Dim aValue As Double = 1.5
        target.AddValue(aDate, aValue)
        Assert.AreEqual("Added", "Added")
    End Sub

    '''<summary>Test AddValueAttribute</summary>
    <TestMethod()> Public Sub AddValueAttributeTest()
        Dim aDataSource As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseriesBuilder = New atcTimeseriesBuilder(aDataSource) ' TODO: Initialize to an appropriate value
        Dim aDate As DateTime = New DateTime(2011, 1, 1, 0, 0, 0) ' TODO: Initialize to an appropriate value
        Dim aValue As Double = 1.5
        target.AddValue(aDate, aValue)

        Dim aAttributeName As String = "Test1"
        Dim aAttributeValue As Object = "Test1Attribute)"
        target.AddValueAttribute(aAttributeName, aAttributeValue)
        Assert.AreEqual(Nothing, target.Attributes.GetValue("Test1"))
    End Sub

    '''<summary>Test AddValueAttributes</summary>
    <TestMethod()> Public Sub AddValueAttributesTest()
        Dim aDataSource As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseriesBuilder = New atcTimeseriesBuilder(aDataSource) ' TODO: Initialize to an appropriate value
        Dim aDate As DateTime = New DateTime(2011, 1, 1, 0, 0, 0) ' TODO: Initialize to an appropriate value
        Dim aValue As Double = 1.5
        target.AddValue(aDate, aValue)

        Dim aAttributes As New atcDataAttributes
        aAttributes.Add("Test1", "Test1Attribute")

        target.AddValueAttributes(aAttributes)
        Assert.AreEqual(Nothing, target.Attributes.GetValue("Test1"))
    End Sub

    '''<summary>Test CreateTimeseries</summary>
    <TestMethod()> Public Sub CreateTimeseriesTest()
        Dim aDataSource As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseriesBuilder = New atcTimeseriesBuilder(aDataSource) ' TODO: Initialize to an appropriate value
        Dim expected As Int64 = -1
        Dim actual As atcTimeseries = target.CreateTimeseries
        Assert.AreEqual(expected, actual.numValues)
        Dim lDate As New Date(2011, 1, 1, 0, 0, 0)
        Dim lValue As Double = 1.0
        target.AddValue(lDate, lValue)
        lDate = New Date(2011, 1, 2, 0, 0, 0)
        lValue = 2.0
        target.AddValue(lDate, lValue)
        lDate = New Date(2011, 1, 3, 0, 0, 0)
        lValue = 3.0
        target.AddValue(lDate, lValue)
        expected = 3
        actual = target.CreateTimeseries
        Assert.AreEqual(expected, actual.numValues)
    End Sub

    '''<summary>Test NumValues</summary>
    <TestMethod()> Public Sub NumValuesTest()
        Dim aDataSource As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseriesBuilder = New atcTimeseriesBuilder(aDataSource) ' TODO: Initialize to an appropriate value
        Assert.AreEqual(0, target.NumValues)
        Dim lDate As New Date(2011, 1, 1, 0, 0, 0)
        Dim lValue As Double = 1.0
        target.AddValue(lDate, lValue)
        lDate = New Date(2011, 1, 2, 0, 0, 0)
        lValue = 2.0
        target.AddValue(lDate, lValue)
        lDate = New Date(2011, 1, 3, 0, 0, 0)
        lValue = 3.0
        target.AddValue(lDate, lValue)
        Dim actual As Integer = target.NumValues
        Assert.AreEqual(4, actual) 'including the initial NaN
    End Sub

    '''<summary>Test Restart</summary>
    <TestMethod()> Public Sub RestartTest()
        Dim aDataSource As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseriesBuilder = New atcTimeseriesBuilder(aDataSource) ' TODO: Initialize to an appropriate value
        Dim lDate As New Date(2011, 1, 1, 0, 0, 0)
        Dim lValue As Double = 1.0
        target.AddValue(lDate, lValue)
        lDate = New Date(2011, 1, 2, 0, 0, 0)
        lValue = 2.0
        target.AddValue(lDate, lValue)
        lDate = New Date(2011, 1, 3, 0, 0, 0)
        lValue = 3.0
        target.AddValue(lDate, lValue)
        Assert.AreEqual(4, target.NumValues) 'including the initial NaN
        target.Restart()
        Assert.AreEqual(0, target.NumValues)
    End Sub

    '''<summary>Test Attributes</summary>
    <TestMethod()> Public Sub AttributesTest()
        Dim aDataSource As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseriesBuilder = New atcTimeseriesBuilder(aDataSource) ' TODO: Initialize to an appropriate value
        Dim actual As atcDataAttributes = target.Attributes
        Assert.AreEqual(0, actual.Count)

        Dim aDate As DateTime = New DateTime(2011, 1, 1, 0, 0, 0) ' TODO: Initialize to an appropriate value
        Dim aValue As Double = 1.5
        target.AddValue(aDate, aValue)

        Dim aAttributeName As String = "Test1"
        Dim aAttributeValue As Object = "Test1Attribute)"
        target.AddValueAttribute(aAttributeName, aAttributeValue)
        actual = target.Attributes
        Assert.AreEqual(0, actual.Count)
    End Sub

    '''<summary>Test LogDateFormat</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub LogDateFormatTest()
        Dim param0 As PrivateObject = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseriesBuilder_Accessor = New atcTimeseriesBuilder_Accessor(param0)
        Assert.Inconclusive("This method is a private readonly property")
        'Assert.AreEqual(Nothing, target.LogDateFormat)
        'Assert.AreEqual(True, target.LogDateFormat.IncludeHours)
        'Assert.AreEqual(True, target.LogDateFormat.IncludeMinutes)
    End Sub

    '''<summary>Test LogNonNumeric</summary>
    <TestMethod()> Public Sub LogNonNumericTest()
        Dim aDataSource As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseriesBuilder = New atcTimeseriesBuilder(aDataSource) ' TODO: Initialize to an appropriate value
        target.LogNonNumeric = False
        Assert.AreEqual(False, target.LogNonNumeric)
        target.LogNonNumeric = True
        Assert.AreEqual(True, target.LogNonNumeric)
    End Sub
End Class
