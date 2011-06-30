Imports atcData
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports atcWDM

'''<summary>
'''This is a test class for WDMGlobalTest and is intended
'''to contain all WDMGlobalTest Unit Tests
'''</summary>
<TestClass()> _
Public Class WDMGlobalTest
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

    '''<summary>Test UnitsAttributeDefinition</summary>
    <TestMethod()> Public Sub UnitsAttributeDefinitionTest()
        Dim Editable As Boolean = False ' TODO: Initialize to an appropriate value
        Dim EditableExpected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim expected As atcAttributeDefinition = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcAttributeDefinition
        actual = WDMGlobal.UnitsAttributeDefinition(Editable)
        Assert.AreEqual(EditableExpected, Editable)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub
End Class
