Imports System.Collections.Generic
Imports atcUtility
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports atcData

'''<summary>
'''This is a test class for atcTimeseriesTest and is intended
'''to contain all atcTimeseriesTest Unit Tests (Done)
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

    Private Function CreateTestTimeseries(Optional ByVal aSetValue As Double = 0.0) As atcTimeseries
        Dim lStartDate As Double = Jday(2011, 1, 1, 0, 0, 0)
        Dim lEndDate As Double = Jday(2011, 1, 1, 6, 0, 0)
        Dim lDate() As Integer = Nothing
        Dim lTs As atcTimeseries = atcData.NewTimeseries(lStartDate, lEndDate, atcUtility.atcTimeUnit.TUHour, 1)
        Dim lYear As Integer = 0
        For lInd As Integer = 0 To lTs.numValues - 1
            If aSetValue <> 0.0 Then
                lTs.Value(lInd + 1) = aSetValue
            Else
                lTs.Value(lInd + 1) = 1.0
            End If
        Next
        Return lTs
    End Function

#Region "Additional test attributes"
    '
    'You can use the following additional attributes as you write your tests:
    '
    'Use ClassInitialize to run code before running the first test in the class
    <ClassInitialize()> _
    Public Shared Sub MyClassInitialize(ByVal testContext As TestContext)
        Dim lHelper As New atcTimeseriesStatistics.atcTimeseriesStatistics
        lHelper.Initialize()
    End Sub
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

    '''<summary>Test atcTimeseries Constructor</summary>
    <TestMethod()> Public Sub atcTimeseriesConstructorTest()
        Dim aTimeseriesSource As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseries = New atcTimeseries(aTimeseriesSource)
        Assert.IsInstanceOfType(target, GetType(atcTimeseries))
    End Sub

    '''<summary>Test Clear</summary>
    <TestMethod()> Public Sub ClearTest()
        Dim aTimeseriesSource As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseries = New atcTimeseries(aTimeseriesSource) ' TODO: Initialize to an appropriate value
        target.Clear()
        Dim lexpectedNumValues As Int64 = 0
        Assert.AreEqual(lexpectedNumValues, target.numValues)
    End Sub

    '''<summary>Test Clone</summary>
    <TestMethod()> Public Sub CloneTest()
        Dim aTimeseriesSource As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseries = New atcTimeseries(aTimeseriesSource) ' TODO: Initialize to an appropriate value
        Dim aDataSource As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As atcTimeseries = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcTimeseries = target.Clone(aDataSource)
        Assert.AreNotEqual(target.Serial, actual.Serial)
        Assert.AreEqual(target.numValues, actual.numValues)
    End Sub

    '''<summary>Test Clone</summary>
    <TestMethod()> Public Sub CloneTest1()
        Dim aTimeseriesSource As New atcTimeseriesSource
        Dim target As atcTimeseries = New atcTimeseries(aTimeseriesSource) ' TODO: Initialize to an appropriate value
        Dim actual As atcTimeseries = target.Clone
        Assert.AreEqual(target.numValues, actual.numValues)
        Assert.AreNotEqual(target.Serial, actual.Serial)
    End Sub

    '''<summary>Test EnsureValuesRead</summary>
    <TestMethod()> Public Sub EnsureValuesReadTest()
        Dim aTimeseriesSource As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseries = New atcTimeseries(aTimeseriesSource) ' TODO: Initialize to an appropriate value
        target.EnsureValuesRead()
        Assert.AreEqual(False, target.ValuesNeedToBeRead)
    End Sub

    '''<summary>Test FirstNumeric</summary>
    <TestMethod()> Public Sub FirstNumericTest()
        Dim aTimeseriesSource As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseries = New atcTimeseries(aTimeseriesSource) ' TODO: Initialize to an appropriate value
        Dim actual As Double = target.FirstNumeric
        Assert.AreEqual(0.0, actual)
        Dim lValues() As Double = {Double.NaN, Double.PositiveInfinity, Double.NegativeInfinity, 3.5, 2.5, 1.5}
        target.Values = lValues
        actual = target.FirstNumeric
        Assert.AreEqual(3.5, actual)
    End Sub

    '''<summary>Test IndexOfValue</summary>
    <TestMethod()> Public Sub IndexOfValueTest()
        Dim aTimeseriesSource As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseries = New atcTimeseries(aTimeseriesSource) ' TODO: Initialize to an appropriate value
        Dim lValues() As Double = {Double.NaN, Double.PositiveInfinity, Double.NegativeInfinity, 1.5, 2.5, 3.5}
        target.Values = lValues
        Dim aValue As Double = 2.5
        Dim aAssumeSorted As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Integer = target.IndexOfValue(aValue, aAssumeSorted)
        Assert.AreEqual(4, actual)
        aAssumeSorted = True
        actual = target.IndexOfValue(aValue, aAssumeSorted)
        Assert.AreEqual(4, actual)
    End Sub

    '''<summary>Test OriginalParent</summary>
    <TestMethod()> Public Sub OriginalParentTest()
        Dim aTimeseriesSource As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseries = New atcTimeseries(aTimeseriesSource) ' TODO: Initialize to an appropriate value
        Dim actual As atcTimeseries = target.OriginalParent
        Assert.AreSame(target, actual)
    End Sub

    '''<summary>Test SetInterval</summary>
    <TestMethod()> Public Sub SetIntervalTest()
        Dim aTimeseriesSource As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseries = New atcTimeseries(aTimeseriesSource) ' TODO: Initialize to an appropriate value
        target.Attributes.SetValue("Tu", atcTimeUnit.TUDay)
        target.Attributes.SetValue("ts", 2)
        target.SetInterval()
        Assert.AreEqual(2, target.Attributes.GetValue("interval"))
        target.Attributes.SetValue("tu", atcTimeUnit.TUMinute)
        target.Attributes.SetValue("ts", 144)
        target.SetInterval()
        Assert.AreEqual(0.1, target.Attributes.GetValue("interval"))
        target.Attributes.SetValue("tu", atcTimeUnit.TUSecond)
        target.Attributes.SetValue("ts", 144 * 6)
        target.SetInterval()
        Assert.AreEqual(0.01, target.Attributes.GetValue("interval"))

    End Sub

    '''<summary>Test SetInterval</summary>
    <TestMethod()> Public Sub SetIntervalTest1()
        Dim aTimeseriesSource As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseries = New atcTimeseries(aTimeseriesSource) ' TODO: Initialize to an appropriate value
        Dim aTimeUnit As modDate.atcTimeUnit = atcTimeUnit.TUDay
        Dim aTimeStep As Integer = 2 ' TODO: Initialize to an appropriate value
        target.SetInterval(aTimeUnit, aTimeStep)
        Assert.AreEqual(2, target.Attributes.GetValue("interval"))
        aTimeUnit = atcTimeUnit.TUMinute
        aTimeStep = 144
        target.SetInterval(aTimeUnit, aTimeStep)
        Assert.AreEqual(0.1, target.Attributes.GetValue("interval"))
        aTimeUnit = atcTimeUnit.TUSecond
        aTimeStep = 144 * 6
        target.SetInterval(aTimeUnit, aTimeStep)
        Assert.AreEqual(0.01, target.Attributes.GetValue("interval"))
    End Sub

    '''<summary>Test ValueAttributeDefinitions</summary>
    <TestMethod()> Public Sub ValueAttributeDefinitionsTest()
        Dim aTimeseriesSource As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseries = New atcTimeseries(aTimeseriesSource) ' TODO: Initialize to an appropriate value
        Dim actual As List(Of atcAttributeDefinition) = target.ValueAttributeDefinitions
        Assert.AreEqual(0, actual.Count)
    End Sub

    '''<summary>Test ValueAttributesExist</summary>
    <TestMethod()> Public Sub ValueAttributesExistTest()
        Dim aTimeseriesSource As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseries = New atcTimeseries(aTimeseriesSource) ' TODO: Initialize to an appropriate value
        Assert.AreEqual(False, target.ValueAttributesExist(1))
        Assert.AreEqual(False, target.ValueAttributesExist(-1))
        Assert.AreEqual(False, target.ValueAttributesExist(100))
    End Sub

    '''<summary>Test ValueAttributesGetValue</summary>
    <TestMethod()> Public Sub ValueAttributesGetValueTest()
        Dim aTimeseriesSource As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseries = New atcTimeseries(aTimeseriesSource) ' TODO: Initialize to an appropriate value
        Dim aIndex As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aAttributeName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aDefault As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Object = target.ValueAttributesGetValue(aIndex, aAttributeName, aDefault)
        Assert.AreEqual(expected, actual)
    End Sub

    '''<summary>Test op_Addition</summary>
    <TestMethod()> Public Sub op_AdditionTest()
        Dim aTimeseries1 As atcTimeseries = CreateTestTimeseries()
        Dim aTimeseries2 As atcTimeseries = CreateTestTimeseries(3)

        Dim actual As atcTimeseries = aTimeseries1 + aTimeseries2
        Assert.AreEqual(4.0, actual.Attributes.GetValue("Mean"))
    End Sub

    '''<summary>Test op_Addition</summary>
    <TestMethod()> Public Sub op_AdditionTest1()
        Dim aTimeseries As atcTimeseries = CreateTestTimeseries()
        Dim aValue As Double = -1
        Dim actual As atcTimeseries = aTimeseries + aValue
        Assert.AreEqual(0.0, actual.Attributes.GetValue("Mean"))
    End Sub

    '''<summary>Test op_Division</summary>
    <TestMethod()> Public Sub op_DivisionTest()
        Dim aTimeseries1 As atcTimeseries = CreateTestTimeseries(400)
        Dim aTimeseries2 As atcTimeseries = CreateTestTimeseries(2.0)
        Dim actual As atcTimeseries = aTimeseries1 / aTimeseries2
        Assert.AreEqual(200.0, actual.Attributes.GetValue("Mean"))
    End Sub

    '''<summary>Test op_Division</summary>
    <TestMethod()> Public Sub op_DivisionTest1()
        Dim aTimeseries As atcTimeseries = CreateTestTimeseries(400.0)
        Dim aValue As Double = 2
        Dim actual As atcTimeseries = aTimeseries / aValue
        Assert.AreEqual(200.0, actual.Attributes.GetValue("Mean"))
    End Sub

    '''<summary>Test op_Exponent</summary>
    <TestMethod()> Public Sub op_ExponentTest()
        Dim aTimeseries As atcTimeseries = CreateTestTimeseries(400.0)
        Dim aValue As Double = 0.5
        Dim actual As atcTimeseries = atcTimeseries.op_Exponent(aTimeseries, aValue)
        Assert.AreEqual(20.0, actual.Attributes.GetValue("Mean"))
    End Sub

    '''<summary>Test op_Exponent</summary>
    <TestMethod()> Public Sub op_ExponentTest1()
        Dim aTimeseries1 As atcTimeseries = CreateTestTimeseries(400.0)
        Dim aTimeseries2 As atcTimeseries = CreateTestTimeseries(0.5)
        Dim actual As atcTimeseries = atcTimeseries.op_Exponent(aTimeseries1, aTimeseries2)
        Assert.AreEqual(20.0, actual.Attributes.GetValue("Mean"))
    End Sub

    '''<summary>Test op_Multiply</summary>
    <TestMethod()> Public Sub op_MultiplyTest()
        Dim aTimeseries As atcTimeseries = CreateTestTimeseries(20.0)
        Dim aValue As Double = 2 ' TODO: Initialize to an appropriate value
        Dim actual As atcTimeseries = aTimeseries * aValue
        Assert.AreEqual(40.0, actual.Attributes.GetValue("Mean"))
    End Sub

    '''<summary>Test op_Multiply</summary>
    <TestMethod()> Public Sub op_MultiplyTest1()
        Dim aTimeseries1 As atcTimeseries = CreateTestTimeseries(20.0)
        Dim aTimeseries2 As atcTimeseries = CreateTestTimeseries(0.5)
        Dim actual As atcTimeseries = aTimeseries1 * aTimeseries2
        Assert.AreEqual(10.0, actual.Attributes.GetValue("Mean"))
    End Sub

    '''<summary>Test op_Subtraction</summary>
    <TestMethod()> Public Sub op_SubtractionTest()
        Dim aTimeseries1 As atcTimeseries = CreateTestTimeseries(1.5)
        Dim aTimeseries2 As atcTimeseries = CreateTestTimeseries(20.0)
        Dim actual As atcTimeseries = aTimeseries1 - aTimeseries2
        Assert.AreEqual(-18.5, actual.Attributes.GetValue("Mean"))
    End Sub

    '''<summary>Test op_Subtraction</summary>
    <TestMethod()> Public Sub op_SubtractionTest1()
        Dim aTimeseries As atcTimeseries = CreateTestTimeseries(1.5)
        Dim aValue As Double = 20.0
        Dim actual As atcTimeseries = aTimeseries - aValue
        Assert.AreEqual(-18.5, actual.Attributes.GetValue("Mean"))
    End Sub

    '''<summary>Test Dates</summary>
    <TestMethod()> Public Sub DatesTest()
        Dim aTimeseriesSource As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseries = New atcTimeseries(aTimeseriesSource) ' TODO: Initialize to an appropriate value
        Assert.AreEqual(Nothing, target.Dates)
        target = CreateTestTimeseries(5)
        Dim lCount As Int64 = 6
        Assert.AreEqual(lCount, target.Dates.numValues)
    End Sub

    '''<summary>Test Value</summary>
    <TestMethod()> Public Sub ValueTest()
        Dim target As atcTimeseries = CreateTestTimeseries(5)
        Dim aIndex As Integer = 4 ' TODO: Initialize to an appropriate value
        target.Value(aIndex) = 10.0
        Dim actual As Double = target.Value(aIndex)
        Assert.AreEqual(10.0, actual)
    End Sub

    '''<summary>Test ValueAttributes</summary>
    <TestMethod()> Public Sub ValueAttributesTest()
        Dim target As atcTimeseries = CreateTestTimeseries(5)
        Dim aIndex As Integer = 4
        target.ValueAttributes(aIndex) = New atcDataAttributes()
        Dim actual As atcDataAttributes = target.ValueAttributes(aIndex)
        Assert.AreEqual(0, actual.Count)
    End Sub

    '''<summary>Test Values</summary>
    <TestMethod()> Public Sub ValuesTest()
        Dim target As atcTimeseries = CreateTestTimeseries(5)
        Dim actual() As Double = target.Values()
        Dim lnew() As Double = {Double.NaN, 2.5, 2.5, 2.5, 2.5, 2.5, 2.5}
        target.Values = lnew
        actual = target.Values
        Assert.AreEqual(lnew(0), target.Value(0))
        Assert.AreEqual(2.5, target.Value(6))
    End Sub

    '''<summary>Test ValuesNeedToBeRead</summary>
    <TestMethod()> Public Sub ValuesNeedToBeReadTest()
        Dim target As atcTimeseries = CreateTestTimeseries(5)
        target.ValuesNeedToBeRead = False
        Assert.AreEqual(False, target.ValuesNeedToBeRead)
        Assert.AreEqual(False, target.Dates.ValuesNeedToBeRead)

        target.ValuesNeedToBeRead = True
        Assert.AreEqual(True, target.ValuesNeedToBeRead)
        Assert.AreEqual(True, target.Dates.ValuesNeedToBeRead)
    End Sub

    '''<summary>Test numValues</summary>
    <TestMethod()> Public Sub numValuesTest()
        Dim aTimeseriesSource As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseries = New atcTimeseries(aTimeseriesSource) ' TODO: Initialize to an appropriate value
        Dim lCount As Int64 = 0
        Assert.AreEqual(lCount, target.numValues)
        target = CreateTestTimeseries(5)
        lCount = 6
        Assert.AreEqual(lCount, target.numValues)
    End Sub
End Class
