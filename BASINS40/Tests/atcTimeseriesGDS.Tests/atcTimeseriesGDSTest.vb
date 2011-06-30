Imports atcData
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports atcTimeseriesGDS

'''<summary>
'''This is a test class for atcTimeseriesGDSTest and is intended
'''to contain all atcTimeseriesGDSTest Unit Tests
'''</summary>
<TestClass()> _
Public Class atcTimeseriesGDSTest
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

    '''<summary>Test atcTimeseriesGDS Constructor</summary>
    <TestMethod()> Public Sub atcTimeseriesGDSConstructorTest()
        Dim target As atcTimeseriesGDS = New atcTimeseriesGDS()
        Assert.Inconclusive("TODO: Implement code to verify target")
    End Sub

    '''<summary>Test Open</summary>
    <TestMethod()> Public Sub OpenTest()
        Dim target As atcTimeseriesGDS = New atcTimeseriesGDS() ' TODO: Initialize to an appropriate value
        Dim aFileName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aAttributes As atcDataAttributes = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = target.Open(aFileName, aAttributes)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test CanOpen</summary>
    <TestMethod()> Public Sub CanOpenTest()
        Dim target As atcTimeseriesGDS = New atcTimeseriesGDS() ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = target.CanOpen
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test Category</summary>
    <TestMethod()> Public Sub CategoryTest()
        Dim target As atcTimeseriesGDS = New atcTimeseriesGDS() ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = target.Category
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test Description</summary>
    <TestMethod()> Public Sub DescriptionTest()
        Dim target As atcTimeseriesGDS = New atcTimeseriesGDS() ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = target.Description
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test Name</summary>
    <TestMethod()> Public Sub NameTest()
        Dim target As atcTimeseriesGDS = New atcTimeseriesGDS() ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = target.Name
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub
End Class
