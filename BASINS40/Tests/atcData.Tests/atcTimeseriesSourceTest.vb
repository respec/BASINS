Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports atcData



'''<summary>
'''This is a test class for atcTimeseriesSourceTest and is intended
'''to contain all atcTimeseriesSourceTest Unit Tests
'''</summary>
<TestClass()> _
Public Class atcTimeseriesSourceTest


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
    '''A test for atcTimeseriesSource Constructor
    '''</summary>
    <TestMethod()> _
    Public Sub atcTimeseriesSourceConstructorTest()
        Dim target As atcTimeseriesSource = New atcTimeseriesSource()
        Assert.Inconclusive("TODO: Implement code to verify target")
    End Sub

    '''<summary>
    '''A test for AddDatasets
    '''</summary>
    <TestMethod()> _
    Public Sub AddDatasetsTest()
        Dim target As atcTimeseriesSource = New atcTimeseriesSource() ' TODO: Initialize to an appropriate value
        Dim aTimeseriesGroup As atcTimeseriesGroup = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = target.AddDatasets(aTimeseriesGroup)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for DataSets
    '''</summary>
    <TestMethod()> _
    Public Sub DataSetsTest()
        Dim target As atcTimeseriesSource = New atcTimeseriesSource() ' TODO: Initialize to an appropriate value
        Dim actual As atcTimeseriesGroup
        actual = target.DataSets
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub
End Class
