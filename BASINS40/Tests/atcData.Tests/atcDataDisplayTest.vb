Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports atcData



'''<summary>
'''This is a test class for atcDataDisplayTest and is intended
'''to contain all atcDataDisplayTest Unit Tests
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


    '''<summary>
    '''A test for atcDataDisplay Constructor
    '''</summary>
    <TestMethod()> _
    Public Sub atcDataDisplayConstructorTest()
        Dim target As atcDataDisplay = New atcDataDisplay()
        Assert.Inconclusive("TODO: Implement code to verify target")
    End Sub

    '''<summary>
    '''A test for Save
    '''</summary>
    <TestMethod()> _
    Public Sub SaveTest()
        Dim target As atcDataDisplay = New atcDataDisplay() ' TODO: Initialize to an appropriate value
        Dim aDataGroup As atcDataGroup = Nothing ' TODO: Initialize to an appropriate value
        Dim aFileName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aOption() As String = Nothing ' TODO: Initialize to an appropriate value
        target.Save(aDataGroup, aFileName, aOption)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for Show
    '''</summary>
    <TestMethod()> _
    Public Sub ShowTest()
        Dim target As atcDataDisplay = New atcDataDisplay() ' TODO: Initialize to an appropriate value
        Dim expected As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Object
        actual = target.Show
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for Show
    '''</summary>
    <TestMethod()> _
    Public Sub ShowTest1()
        Dim target As atcDataDisplay = New atcDataDisplay() ' TODO: Initialize to an appropriate value
        Dim aDataGroup As atcDataGroup = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Object
        actual = target.Show(aDataGroup)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub
End Class
