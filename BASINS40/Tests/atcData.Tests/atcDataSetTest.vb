Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports atcData

'''<summary>
'''This is a test class for atcDataSetTest and is intended
'''to contain all atcDataSetTest Unit Tests (Done)
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
        Assert.IsInstanceOfType(target, GetType(atcDataSet))
        Dim lSer As Integer = target.Serial
        Dim target2 As atcDataSet = New atcDataSet()
        Assert.AreEqual(1, target2.Serial - target.Serial)
    End Sub

    '''<summary>Test Clear</summary>
    <TestMethod()> Public Sub ClearTest()
        Dim target As atcDataSet = New atcDataSet()
        Assert.IsInstanceOfType(target, GetType(atcDataSet))
        Assert.AreEqual(0, target.Attributes.Count)
    End Sub

    '''<summary>Test Clone</summary>
    <TestMethod()> Public Sub CloneTest()
        Dim target As atcDataSet = New atcDataSet() ' TODO: Initialize to an appropriate value
        Dim expected As atcDataSet = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcDataSet
        actual = target.Clone
        Assert.AreEqual(1, actual.Serial - target.Serial)
        Assert.AreEqual(actual.Attributes.Count, target.Attributes.Count)
    End Sub

    '''<summary>Test SetStringFormat</summary>
    <TestMethod()> Public Sub SetStringFormatTest()
        Dim aAttributeNames() As String = {"Test1", "Test2", "Test3"}
        Dim aFormat As String = String.Empty ' TODO: Initialize to an appropriate value
        atcDataSet.SetStringFormat(aAttributeNames, aFormat)
    End Sub

    '''<summary>Test ToString</summary>
    <TestMethod()> Public Sub ToStringTest()
        Dim aAttributeNames() As String = {"Att 1", "Att 2", "Att 3"}
        Dim aFormat As String = String.Empty ' TODO: Initialize to an appropriate value
        atcDataSet.SetStringFormat(aAttributeNames, aFormat)

        Dim lDS As atcDataSet = New atcDataSet
        lDS.Clear()
        lDS.Attributes.SetValue("Att 1", "Test1")
        lDS.Attributes.SetValue("Att 2", "Test2")
        lDS.Attributes.SetValue("Att 3", "Test3")
        Assert.AreEqual("Test1 Test2 Test3", lDS.ToString.Trim)
    End Sub

    '''<summary>Test op_Equality</summary>
    <TestMethod()> Public Sub op_EqualityTest()
        Dim aArg1 As New atcDataSet
        Dim aArg2 As New atcDataSet

        Assert.AreEqual(False, (aArg1 = aArg2))
        Assert.AreEqual(True, (aArg1 = aArg1))

    End Sub

    '''<summary>Test op_Inequality</summary>
    <TestMethod()> Public Sub op_InequalityTest()
        Dim aArg1 As New atcDataSet
        Dim aArg2 As New atcDataSet

        Assert.AreEqual(True, (aArg1 <> aArg2))
        Assert.AreEqual(False, (aArg1 <> aArg1))
    End Sub

    '''<summary>Test Attributes</summary>
    <TestMethod()> Public Sub AttributesTest()
        Dim target As atcDataSet = New atcDataSet()
        target.Clear()
        Assert.AreEqual(0, target.Attributes.Count)
        target.Attributes.SetValue("Att 1", "Test1")
        target.Attributes.SetValue("Att 2", "Test2")
        target.Attributes.SetValue("Att 3", "Test3")

        Assert.AreEqual(3, target.Attributes.Count)
    End Sub

    '''<summary>Test Serial</summary>
    <TestMethod()> Public Sub SerialTest()
        Dim target As atcDataSet = New atcDataSet()
        Dim lSer1 As Integer = target.Serial
        Assert.AreNotEqual(0, lSer1)
        Dim ltarget As atcDataSet = New atcDataSet()
        Assert.AreEqual(1, ltarget.Serial - lSer1)

    End Sub
End Class
