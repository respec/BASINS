Imports System.Drawing
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports atcData

'''<summary>
'''This is a test class for atcDataPluginTest and is intended
'''to contain all atcDataPluginTest Unit Tests
'''</summary>
<TestClass()> _
Public Class atcDataPluginTest
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

    '''<summary>Test atcDataPlugin Constructor</summary>
    <TestMethod()> Public Sub atcDataPluginConstructorTest()
        Dim target As atcDataPlugin = New atcDataPlugin()
        Assert.Inconclusive("TODO: Implement code to verify target")
    End Sub

    '''<summary>Test NewOne</summary>
    <TestMethod()> Public Sub NewOneTest()
        Dim target As atcDataPlugin = New atcDataPlugin() ' TODO: Initialize to an appropriate value
        Dim expected As atcDataPlugin = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcDataPlugin
        actual = target.NewOne
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test ToString</summary>
    <TestMethod()> Public Sub ToStringTest()
        Dim target As atcDataPlugin = New atcDataPlugin() ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = target.ToString
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test Author</summary>
    <TestMethod()> Public Sub AuthorTest()
        Dim target As atcDataPlugin = New atcDataPlugin() ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = target.Author
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test BuildDate</summary>
    <TestMethod()> Public Sub BuildDateTest()
        Dim target As atcDataPlugin = New atcDataPlugin() ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = target.BuildDate
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test Category</summary>
    <TestMethod()> Public Sub CategoryTest()
        Dim target As atcDataPlugin = New atcDataPlugin() ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = target.Category
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test Description</summary>
    <TestMethod()> Public Sub DescriptionTest()
        Dim target As atcDataPlugin = New atcDataPlugin() ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = target.Description
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test Icon</summary>
    <TestMethod()> Public Sub IconTest()
        Dim target As atcDataPlugin = New atcDataPlugin() ' TODO: Initialize to an appropriate value
        Dim actual As Icon
        actual = target.Icon
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test Name</summary>
    <TestMethod()> Public Sub NameTest()
        Dim target As atcDataPlugin = New atcDataPlugin() ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = target.Name
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test SerialNumber</summary>
    <TestMethod()> Public Sub SerialNumberTest()
        Dim target As atcDataPlugin = New atcDataPlugin() ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = target.SerialNumber
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test Version</summary>
    <TestMethod()> Public Sub VersionTest()
        Dim target As atcDataPlugin = New atcDataPlugin() ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = target.Version
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub
End Class
