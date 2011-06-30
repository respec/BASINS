Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports atcData

'''<summary>
'''This is a test class for atcDataSetTest and is intended
'''to contain all atcDataSetTest Unit Tests
'''</summary>
<TestClass()> _
Public Class atcDataSetTest
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

    '''<summary>Test atcDataSet Constructor</summary>
    <TestMethod()> Public Sub atcDataSetConstructorTest()
        Dim target As atcDataSet = New atcDataSet()
        Assert.Inconclusive("TODO: Implement code to verify target")
    End Sub

    '''<summary>Test Clear</summary>
    <TestMethod()> Public Sub ClearTest()
        Dim target As atcDataSet = New atcDataSet() ' TODO: Initialize to an appropriate value
        target.Clear()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test Clone</summary>
    <TestMethod()> Public Sub CloneTest()
        Dim target As atcDataSet = New atcDataSet() ' TODO: Initialize to an appropriate value
        Dim expected As atcDataSet = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcDataSet
        actual = target.Clone
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test SetStringFormat</summary>
    <TestMethod()> Public Sub SetStringFormatTest()
        Dim aAttributeNames() As String = Nothing ' TODO: Initialize to an appropriate value
        Dim aFormat As String = String.Empty ' TODO: Initialize to an appropriate value
        atcDataSet.SetStringFormat(aAttributeNames, aFormat)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test ToString</summary>
    <TestMethod()> Public Sub ToStringTest()
        Dim target As atcDataSet = New atcDataSet() ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = target.ToString
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test op_Equality</summary>
    <TestMethod()> Public Sub op_EqualityTest()
        Dim aArg1 As atcDataSet = Nothing ' TODO: Initialize to an appropriate value
        Dim aArg2 As atcDataSet = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = (aArg1 = aArg2)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test op_Inequality</summary>
    <TestMethod()> Public Sub op_InequalityTest()
        Dim aArg1 As atcDataSet = Nothing ' TODO: Initialize to an appropriate value
        Dim aArg2 As atcDataSet = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = (aArg1 <> aArg2)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test Attributes</summary>
    <TestMethod()> Public Sub AttributesTest()
        Dim target As atcDataSet = New atcDataSet() ' TODO: Initialize to an appropriate value
        Dim actual As atcDataAttributes
        actual = target.Attributes
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test Serial</summary>
    <TestMethod()> Public Sub SerialTest()
        Dim target As atcDataSet = New atcDataSet() ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        actual = target.Serial
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub
End Class
