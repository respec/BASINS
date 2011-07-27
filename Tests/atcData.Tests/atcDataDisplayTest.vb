Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports atcData

'''<summary>
'''This is a test class for atcDataDisplayTest and is intended
'''to contain all atcDataDisplayTest Unit Tests (Done)
'''</summary>
<TestClass()> _
Public Class atcDataDisplayTest
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

    '''<summary>Test atcDataDisplay Constructor</summary>
    <TestMethod()> Public Sub atcDataDisplayConstructorTest()
        Dim target As atcDataDisplay = New atcDataDisplay()
        Assert.IsInstanceOfType(target, GetType(atcDataDisplay))
    End Sub

    '''<summary>Test Save</summary>
    <TestMethod()> Public Sub SaveTest()
        Dim target As atcDataDisplay = New atcDataDisplay() ' TODO: Initialize to an appropriate value
        Dim aDataGroup As atcDataGroup = Nothing ' TODO: Initialize to an appropriate value
        Dim aFileName As String = IO.Path.Combine(Environment.CurrentDirectory, "zTest_atcDataDisplay_Save.txt")
        Dim aOption() As String = Nothing ' TODO: Initialize to an appropriate value
        Try
            target.Save(aDataGroup, aFileName, aOption)
            Assert.AreEqual(True, True)
        Catch ex As Exception
            Assert.AreEqual(True, False)
        End Try
    End Sub

    '''<summary>Test Show</summary>
    <TestMethod()> Public Sub ShowTest()
        Dim target As atcDataDisplay = New atcDataDisplay() ' TODO: Initialize to an appropriate value
        Dim expected As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Object
        actual = target.Show
        Assert.AreEqual(expected, actual)
    End Sub

    '''<summary>Test Show</summary>
    <TestMethod()> Public Sub ShowTest1()
        Dim target As atcDataDisplay = New atcDataDisplay() ' TODO: Initialize to an appropriate value
        Dim aDataGroup As atcDataGroup = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Object
        actual = target.Show(aDataGroup)
        Assert.AreEqual(expected, actual)
    End Sub
End Class
