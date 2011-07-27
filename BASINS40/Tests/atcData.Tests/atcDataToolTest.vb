Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports atcData

'''<summary>
'''This is a test class for atcDataToolTest and is intended
'''to contain all atcDataToolTest Unit Tests (Done, needs more consideration for abstract class)
'''</summary>
<TestClass()> _
Public Class atcDataToolTest
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

    Friend Overridable Function CreateatcDataTool() As atcDataTool
        'TODO: Instantiate an appropriate concrete class.
        Dim target As atcDataTool = Nothing
        Return target
    End Function

    '''<summary>Test Show</summary>
    <TestMethod()> Public Sub ShowTest()
        Dim target As atcDataTool = CreateatcDataTool() ' TODO: Initialize to an appropriate value
        Assert.AreEqual(target, Nothing)
    End Sub
End Class
