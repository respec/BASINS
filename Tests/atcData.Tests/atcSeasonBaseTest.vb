Imports System.Collections
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports atcData

'''<summary>
'''This is a test class for atcSeasonBaseTest and is intended
'''to contain all atcSeasonBaseTest Unit Tests
'''</summary>
<TestClass()> _
Public Class atcSeasonBaseTest
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

    '''<summary>Test atcSeasonBase Constructor</summary>
    <TestMethod()> Public Sub atcSeasonBaseConstructorTest()
        Dim target As atcSeasonBase = New atcSeasonBase()
        Assert.Inconclusive("TODO: Implement code to verify target")
    End Sub

    '''<summary>Test AllSeasonNames</summary>
    <TestMethod()> Public Sub AllSeasonNamesTest()
        Dim target As atcSeasonBase = New atcSeasonBase() ' TODO: Initialize to an appropriate value
        Dim expected() As String = Nothing ' TODO: Initialize to an appropriate value
        Dim actual() As String
        actual = target.AllSeasonNames
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test AllSeasons</summary>
    <TestMethod()> Public Sub AllSeasonsTest()
        Dim target As atcSeasonBase = New atcSeasonBase() ' TODO: Initialize to an appropriate value
        Dim expected() As Integer = Nothing ' TODO: Initialize to an appropriate value
        Dim actual() As Integer
        actual = target.AllSeasons
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test AllSeasonsInDates</summary>
    <TestMethod()> Public Sub AllSeasonsInDatesTest()
        Dim target As atcSeasonBase = New atcSeasonBase() ' TODO: Initialize to an appropriate value
        Dim aDates() As Double = Nothing ' TODO: Initialize to an appropriate value
        Dim expected() As Integer = Nothing ' TODO: Initialize to an appropriate value
        Dim actual() As Integer
        actual = target.AllSeasonsInDates(aDates)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test Clone</summary>
    <TestMethod()> Public Sub CloneTest()
        Dim target As atcSeasonBase = New atcSeasonBase() ' TODO: Initialize to an appropriate value
        Dim expected As atcSeasonBase = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcSeasonBase
        actual = target.Clone
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test SeasonIndex</summary>
    <TestMethod()> Public Sub SeasonIndexTest()
        Dim target As atcSeasonBase = New atcSeasonBase() ' TODO: Initialize to an appropriate value
        Dim aDate As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim expected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        actual = target.SeasonIndex(aDate)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test SeasonName</summary>
    <TestMethod()> Public Sub SeasonNameTest()
        Dim target As atcSeasonBase = New atcSeasonBase() ' TODO: Initialize to an appropriate value
        Dim aDate As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = target.SeasonName(aDate)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test SeasonName</summary>
    <TestMethod()> Public Sub SeasonNameTest1()
        Dim target As atcSeasonBase = New atcSeasonBase() ' TODO: Initialize to an appropriate value
        Dim aIndex As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = target.SeasonName(aIndex)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test SeasonYearFraction</summary>
    <TestMethod()> Public Sub SeasonYearFractionTest()
        Dim target As atcSeasonBase = New atcSeasonBase() ' TODO: Initialize to an appropriate value
        Dim aIndex As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim expected As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim actual As Double
        actual = target.SeasonYearFraction(aIndex)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test SeasonsSelectedString</summary>
    <TestMethod()> Public Sub SeasonsSelectedStringTest()
        Dim target As atcSeasonBase = New atcSeasonBase() ' TODO: Initialize to an appropriate value
        Dim aXML As Boolean = False ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = target.SeasonsSelectedString(aXML)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test SetSeasonalAttributes</summary>
    <TestMethod()> Public Sub SetSeasonalAttributesTest()
        Dim target As atcSeasonBase = New atcSeasonBase() ' TODO: Initialize to an appropriate value
        Dim aTS As atcTimeseries = Nothing ' TODO: Initialize to an appropriate value
        Dim aAttributes As atcDataAttributes = Nothing ' TODO: Initialize to an appropriate value
        Dim aCalculatedAttributes As atcDataAttributes = Nothing ' TODO: Initialize to an appropriate value
        target.SetSeasonalAttributes(aTS, aAttributes, aCalculatedAttributes)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test Split</summary>
    <TestMethod()> Public Sub SplitTest()
        Dim target As atcSeasonBase = New atcSeasonBase() ' TODO: Initialize to an appropriate value
        Dim aTS As atcTimeseries = Nothing ' TODO: Initialize to an appropriate value
        Dim aSource As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As atcTimeseriesGroup = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcTimeseriesGroup
        actual = target.Split(aTS, aSource)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test SplitBySelected</summary>
    <TestMethod()> Public Sub SplitBySelectedTest()
        Dim target As atcSeasonBase = New atcSeasonBase() ' TODO: Initialize to an appropriate value
        Dim aTS As atcTimeseries = Nothing ' TODO: Initialize to an appropriate value
        Dim aSource As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As atcTimeseriesGroup = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcTimeseriesGroup
        actual = target.SplitBySelected(aTS, aSource)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test ToString</summary>
    <TestMethod()> Public Sub ToStringTest()
        Dim target As atcSeasonBase = New atcSeasonBase() ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = target.ToString
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test Category</summary>
    <TestMethod()> Public Sub CategoryTest()
        Dim target As atcSeasonBase = New atcSeasonBase() ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = target.Category
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test Description</summary>
    <TestMethod()> Public Sub DescriptionTest()
        Dim target As atcSeasonBase = New atcSeasonBase() ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = target.Description
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test Name</summary>
    <TestMethod()> Public Sub NameTest()
        Dim target As atcSeasonBase = New atcSeasonBase() ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = target.Name
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test SeasonSelected</summary>
    <TestMethod()> Public Sub SeasonSelectedTest()
        Dim target As atcSeasonBase = New atcSeasonBase() ' TODO: Initialize to an appropriate value
        Dim aSeasonIndex As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        target.SeasonSelected(aSeasonIndex) = expected
        actual = target.SeasonSelected(aSeasonIndex)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test SeasonsSelected</summary>
    <TestMethod()> Public Sub SeasonsSelectedTest()
        Dim target As atcSeasonBase = New atcSeasonBase() ' TODO: Initialize to an appropriate value
        Dim expected As ArrayList = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ArrayList
        target.SeasonsSelected = expected
        actual = target.SeasonsSelected
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test SeasonsSelectedXML</summary>
    <TestMethod()> Public Sub SeasonsSelectedXMLTest()
        Dim target As atcSeasonBase = New atcSeasonBase() ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        target.SeasonsSelectedXML = expected
        actual = target.SeasonsSelectedXML
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub
End Class
