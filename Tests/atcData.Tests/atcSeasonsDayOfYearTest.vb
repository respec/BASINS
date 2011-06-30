Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports atcData

'''<summary>
'''This is a test class for atcSeasonsDayOfYearTest and is intended
'''to contain all atcSeasonsDayOfYearTest Unit Tests
'''</summary>
<TestClass()> _
Public Class atcSeasonsDayOfYearTest
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

    '''<summary>Test atcSeasonsDayOfYear Constructor</summary>
    <TestMethod()> Public Sub atcSeasonsDayOfYearConstructorTest()
        Dim target As atcSeasonsDayOfYear = New atcSeasonsDayOfYear()
        Assert.Inconclusive("TODO: Implement code to verify target")
    End Sub

    '''<summary>Test AllSeasons</summary>
    <TestMethod()> Public Sub AllSeasonsTest()
        Dim target As atcSeasonsDayOfYear = New atcSeasonsDayOfYear() ' TODO: Initialize to an appropriate value
        Dim expected() As Integer = Nothing ' TODO: Initialize to an appropriate value
        Dim actual() As Integer
        actual = target.AllSeasons
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test Clone</summary>
    <TestMethod()> Public Sub CloneTest()
        Dim target As atcSeasonsDayOfYear = New atcSeasonsDayOfYear() ' TODO: Initialize to an appropriate value
        Dim expected As atcSeasonBase = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcSeasonBase
        actual = target.Clone
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test SeasonIndex</summary>
    <TestMethod()> Public Sub SeasonIndexTest()
        Dim target As atcSeasonsDayOfYear = New atcSeasonsDayOfYear() ' TODO: Initialize to an appropriate value
        Dim aDate As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim expected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        actual = target.SeasonIndex(aDate)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test SeasonName</summary>
    <TestMethod()> Public Sub SeasonNameTest()
        Dim target As atcSeasonsDayOfYear = New atcSeasonsDayOfYear() ' TODO: Initialize to an appropriate value
        Dim aIndex As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = target.SeasonName(aIndex)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test SeasonSelected</summary>
    <TestMethod()> Public Sub SeasonSelectedTest()
        Dim target As atcSeasonsDayOfYear = New atcSeasonsDayOfYear() ' TODO: Initialize to an appropriate value
        Dim aSeasonIndex As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        target.SeasonSelected(aSeasonIndex) = expected
        actual = target.SeasonSelected(aSeasonIndex)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub
End Class
