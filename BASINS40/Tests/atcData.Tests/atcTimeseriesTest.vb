Imports System.Collections.Generic

Imports atcUtility

Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports atcData



'''<summary>
'''This is a test class for atcTimeseriesTest and is intended
'''to contain all atcTimeseriesTest Unit Tests
'''</summary>
<TestClass()> _
Public Class atcTimeseriesTest


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
    '''A test for atcTimeseries Constructor
    '''</summary>
    <TestMethod()> _
    Public Sub atcTimeseriesConstructorTest()
        Dim aTimeseriesSource As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseries = New atcTimeseries(aTimeseriesSource)
        Assert.Inconclusive("TODO: Implement code to verify target")
    End Sub

    '''<summary>
    '''A test for Clear
    '''</summary>
    <TestMethod()> _
    Public Sub ClearTest()
        Dim aTimeseriesSource As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseries = New atcTimeseries(aTimeseriesSource) ' TODO: Initialize to an appropriate value
        target.Clear()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for Clone
    '''</summary>
    <TestMethod()> _
    Public Sub CloneTest()
        Dim aTimeseriesSource As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseries = New atcTimeseries(aTimeseriesSource) ' TODO: Initialize to an appropriate value
        Dim aDataSource As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As atcDataSet = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcDataSet
        actual = target.Clone(aDataSource)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for Clone
    '''</summary>
    <TestMethod()> _
    Public Sub CloneTest1()
        Dim aTimeseriesSource As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseries = New atcTimeseries(aTimeseriesSource) ' TODO: Initialize to an appropriate value
        Dim expected As atcDataSet = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcDataSet
        actual = target.Clone
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for EnsureValuesRead
    '''</summary>
    <TestMethod()> _
    Public Sub EnsureValuesReadTest()
        Dim aTimeseriesSource As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseries = New atcTimeseries(aTimeseriesSource) ' TODO: Initialize to an appropriate value
        target.EnsureValuesRead()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for FirstNumeric
    '''</summary>
    <TestMethod()> _
    Public Sub FirstNumericTest()
        Dim aTimeseriesSource As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseries = New atcTimeseries(aTimeseriesSource) ' TODO: Initialize to an appropriate value
        Dim expected As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim actual As Double
        actual = target.FirstNumeric
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for IndexOfValue
    '''</summary>
    <TestMethod()> _
    Public Sub IndexOfValueTest()
        Dim aTimeseriesSource As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseries = New atcTimeseries(aTimeseriesSource) ' TODO: Initialize to an appropriate value
        Dim aValue As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim aAssumeSorted As Boolean = False ' TODO: Initialize to an appropriate value
        Dim expected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        actual = target.IndexOfValue(aValue, aAssumeSorted)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for OriginalParent
    '''</summary>
    <TestMethod()> _
    Public Sub OriginalParentTest()
        Dim aTimeseriesSource As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseries = New atcTimeseries(aTimeseriesSource) ' TODO: Initialize to an appropriate value
        Dim expected As atcTimeseries = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcTimeseries
        actual = target.OriginalParent
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for SetInterval
    '''</summary>
    <TestMethod()> _
    Public Sub SetIntervalTest()
        Dim aTimeseriesSource As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseries = New atcTimeseries(aTimeseriesSource) ' TODO: Initialize to an appropriate value
        target.SetInterval()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for SetInterval
    '''</summary>
    <TestMethod()> _
    Public Sub SetIntervalTest1()
        Dim aTimeseriesSource As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseries = New atcTimeseries(aTimeseriesSource) ' TODO: Initialize to an appropriate value
        Dim aTimeUnit As modDate.atcTimeUnit = New modDate.atcTimeUnit() ' TODO: Initialize to an appropriate value
        Dim aTimeStep As Integer = 0 ' TODO: Initialize to an appropriate value
        target.SetInterval(aTimeUnit, aTimeStep)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for ValueAttributeDefinitions
    '''</summary>
    <TestMethod()> _
    Public Sub ValueAttributeDefinitionsTest()
        Dim aTimeseriesSource As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseries = New atcTimeseries(aTimeseriesSource) ' TODO: Initialize to an appropriate value
        Dim expected As List(Of atcAttributeDefinition) = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As List(Of atcAttributeDefinition)
        actual = target.ValueAttributeDefinitions
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for ValueAttributesExist
    '''</summary>
    <TestMethod()> _
    Public Sub ValueAttributesExistTest()
        Dim aTimeseriesSource As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseries = New atcTimeseries(aTimeseriesSource) ' TODO: Initialize to an appropriate value
        Dim aIndex As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = target.ValueAttributesExist(aIndex)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for ValueAttributesGetValue
    '''</summary>
    <TestMethod()> _
    Public Sub ValueAttributesGetValueTest()
        Dim aTimeseriesSource As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseries = New atcTimeseries(aTimeseriesSource) ' TODO: Initialize to an appropriate value
        Dim aIndex As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aAttributeName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aDefault As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Object
        actual = target.ValueAttributesGetValue(aIndex, aAttributeName, aDefault)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for op_Addition
    '''</summary>
    <TestMethod()> _
    Public Sub op_AdditionTest()
        Dim aTimeseries1 As atcTimeseries = Nothing ' TODO: Initialize to an appropriate value
        Dim aTimeseries2 As atcTimeseries = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As atcTimeseries = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcTimeseries
        actual = (aTimeseries1 + aTimeseries2)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for op_Addition
    '''</summary>
    <TestMethod()> _
    Public Sub op_AdditionTest1()
        Dim aTimeseries As atcTimeseries = Nothing ' TODO: Initialize to an appropriate value
        Dim aValue As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim expected As atcTimeseries = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcTimeseries
        actual = (aTimeseries + aValue)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for op_Division
    '''</summary>
    <TestMethod()> _
    Public Sub op_DivisionTest()
        Dim aTimeseries1 As atcTimeseries = Nothing ' TODO: Initialize to an appropriate value
        Dim aTimeseries2 As atcTimeseries = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As atcTimeseries = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcTimeseries
        actual = (aTimeseries1 / aTimeseries2)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for op_Division
    '''</summary>
    <TestMethod()> _
    Public Sub op_DivisionTest1()
        Dim aTimeseries As atcTimeseries = Nothing ' TODO: Initialize to an appropriate value
        Dim aValue As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim expected As atcTimeseries = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcTimeseries
        actual = (aTimeseries / aValue)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for op_Exponent
    '''</summary>
    <TestMethod()> _
    Public Sub op_ExponentTest()
        Dim aTimeseries As atcTimeseries = Nothing ' TODO: Initialize to an appropriate value
        Dim aValue As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim expected As atcTimeseries = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcTimeseries
        actual = atcTimeseries.op_Exponent(aTimeseries, aValue)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for op_Exponent
    '''</summary>
    <TestMethod()> _
    Public Sub op_ExponentTest1()
        Dim aTimeseries1 As atcTimeseries = Nothing ' TODO: Initialize to an appropriate value
        Dim aTimeseries2 As atcTimeseries = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As atcTimeseries = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcTimeseries
        actual = atcTimeseries.op_Exponent(aTimeseries1, aTimeseries2)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for op_Multiply
    '''</summary>
    <TestMethod()> _
    Public Sub op_MultiplyTest()
        Dim aTimeseries As atcTimeseries = Nothing ' TODO: Initialize to an appropriate value
        Dim aValue As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim expected As atcTimeseries = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcTimeseries
        actual = (aTimeseries * aValue)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for op_Multiply
    '''</summary>
    <TestMethod()> _
    Public Sub op_MultiplyTest1()
        Dim aTimeseries1 As atcTimeseries = Nothing ' TODO: Initialize to an appropriate value
        Dim aTimeseries2 As atcTimeseries = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As atcTimeseries = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcTimeseries
        actual = (aTimeseries1 * aTimeseries2)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for op_Subtraction
    '''</summary>
    <TestMethod()> _
    Public Sub op_SubtractionTest()
        Dim aTimeseries1 As atcTimeseries = Nothing ' TODO: Initialize to an appropriate value
        Dim aTimeseries2 As atcTimeseries = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As atcTimeseries = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcTimeseries
        actual = (aTimeseries1 - aTimeseries2)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for op_Subtraction
    '''</summary>
    <TestMethod()> _
    Public Sub op_SubtractionTest1()
        Dim aTimeseries As atcTimeseries = Nothing ' TODO: Initialize to an appropriate value
        Dim aValue As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim expected As atcTimeseries = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcTimeseries
        actual = (aTimeseries - aValue)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for Dates
    '''</summary>
    <TestMethod()> _
    Public Sub DatesTest()
        Dim aTimeseriesSource As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseries = New atcTimeseries(aTimeseriesSource) ' TODO: Initialize to an appropriate value
        Dim expected As atcTimeseries = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcTimeseries
        target.Dates = expected
        actual = target.Dates
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for Value
    '''</summary>
    <TestMethod()> _
    Public Sub ValueTest()
        Dim aTimeseriesSource As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseries = New atcTimeseries(aTimeseriesSource) ' TODO: Initialize to an appropriate value
        Dim aIndex As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim expected As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim actual As Double
        target.Value(aIndex) = expected
        actual = target.Value(aIndex)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for ValueAttributes
    '''</summary>
    <TestMethod()> _
    Public Sub ValueAttributesTest()
        Dim aTimeseriesSource As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseries = New atcTimeseries(aTimeseriesSource) ' TODO: Initialize to an appropriate value
        Dim aIndex As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim expected As atcDataAttributes = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcDataAttributes
        target.ValueAttributes(aIndex) = expected
        actual = target.ValueAttributes(aIndex)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for Values
    '''</summary>
    <TestMethod()> _
    Public Sub ValuesTest()
        Dim aTimeseriesSource As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseries = New atcTimeseries(aTimeseriesSource) ' TODO: Initialize to an appropriate value
        Dim expected() As Double = Nothing ' TODO: Initialize to an appropriate value
        Dim actual() As Double
        target.Values = expected
        actual = target.Values
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for ValuesNeedToBeRead
    '''</summary>
    <TestMethod()> _
    Public Sub ValuesNeedToBeReadTest()
        Dim aTimeseriesSource As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseries = New atcTimeseries(aTimeseriesSource) ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        target.ValuesNeedToBeRead = expected
        actual = target.ValuesNeedToBeRead
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for numValues
    '''</summary>
    <TestMethod()> _
    Public Sub numValuesTest()
        Dim aTimeseriesSource As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseries = New atcTimeseries(aTimeseriesSource) ' TODO: Initialize to an appropriate value
        Dim expected As Long = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Long
        target.numValues = expected
        actual = target.numValues
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub
End Class
