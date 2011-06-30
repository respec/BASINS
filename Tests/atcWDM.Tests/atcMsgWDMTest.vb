Imports atcUtility

Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports atcWDM



'''<summary>
'''This is a test class for atcMsgWDMTest and is intended
'''to contain all atcMsgWDMTest Unit Tests
'''</summary>
<TestClass()> _
Public Class atcMsgWDMTest


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
    '''A test for atcMsgWDM Constructor
    '''</summary>
    <TestMethod()> _
    Public Sub atcMsgWDMConstructorTest()
        Dim target As atcMsgWDM = New atcMsgWDM()
        Assert.Inconclusive("TODO: Implement code to verify target")
    End Sub

    '''<summary>
    '''A test for MsgHandle
    '''</summary>
    <TestMethod()> _
    Public Sub MsgHandleTest()
        Dim target As atcMsgWDM = New atcMsgWDM() ' TODO: Initialize to an appropriate value
        Dim expected As atcWdmHandle = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcWdmHandle
        actual = target.MsgHandle
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for Attributes
    '''</summary>
    <TestMethod()> _
    Public Sub AttributesTest()
        Dim target As atcMsgWDM = New atcMsgWDM() ' TODO: Initialize to an appropriate value
        Dim actual As atcCollection
        actual = target.Attributes
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub
End Class
