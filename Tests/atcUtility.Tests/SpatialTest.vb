Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports atcUtility

'''<summary>
'''This is a test class for SpatialTest and is intended
'''to contain all SpatialTest Unit Tests
'''</summary>
<TestClass()> _
Public Class SpatialTest
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

    '''<summary>Test Spatial Constructor</summary>
    <TestMethod()> Public Sub SpatialConstructorTest()
        Dim target As Spatial = New Spatial()
        Assert.Inconclusive("TODO: Implement code to verify target")
    End Sub

    '''<summary>Test GreatCircleDistance</summary>
    <TestMethod()> Public Sub GreatCircleDistanceTest()
        Dim aLong1 As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim aLat1 As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim aLong2 As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim aLat2 As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim expected As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim actual As Double
        actual = Spatial.GreatCircleDistance(aLong1, aLat1, aLong2, aLat2)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub
End Class
