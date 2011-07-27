Imports atcUtility
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports atcData

'''<summary>
'''This is a test class for atcTimeseriesGroupTest and is intended
'''to contain all atcTimeseriesGroupTest Unit Tests (Done)
'''</summary>
<TestClass()> _
Public Class atcTimeseriesGroupTest
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

    '''<summary>Test atcTimeseriesGroup Constructor</summary>
    <TestMethod()> Public Sub atcTimeseriesGroupConstructorTest()
        Dim aTimeseries As New atcTimeseries(Nothing)
        Dim target As atcTimeseriesGroup = New atcTimeseriesGroup(aTimeseries)
        Assert.AreEqual(1, target.Count)
    End Sub

    '''<summary>Test atcTimeseriesGroup Constructor</summary>
    <TestMethod()> Public Sub atcTimeseriesGroupConstructorTest1()
        Dim aDataGroup As New atcDataGroup
        aDataGroup.Add(New atcTimeseries(Nothing))
        aDataGroup.Add(New atcTimeseries(Nothing))
        aDataGroup.Add(New atcTimeseries(Nothing))
        Dim target As atcTimeseriesGroup = New atcTimeseriesGroup(aDataGroup)
        Assert.AreEqual(3, aDataGroup.Count)
    End Sub

    '''<summary>Test atcTimeseriesGroup Constructor</summary>
    <TestMethod()> Public Sub atcTimeseriesGroupConstructorTest2()
        Dim aTimeseries() As atcTimeseries = {New atcTimeseries(Nothing), New atcTimeseries(Nothing)}
        Dim target As atcTimeseriesGroup = New atcTimeseriesGroup(aTimeseries)
        Assert.AreEqual(2, target.Count)
    End Sub

    '''<summary>Test atcTimeseriesGroup Constructor</summary>
    <TestMethod()> Public Sub atcTimeseriesGroupConstructorTest3()
        Dim target As atcTimeseriesGroup = New atcTimeseriesGroup()
        Assert.AreEqual(0, target.Count)
    End Sub

    '''<summary>Test Clone</summary>
    <TestMethod()> Public Sub CloneTest()
        Dim target As atcTimeseriesGroup = New atcTimeseriesGroup() ' TODO: Initialize to an appropriate value
        Dim actual As atcTimeseriesGroup = target.Clone
        Assert.AreEqual(target.Count, actual.Count)
        Dim aTimeseries() As atcTimeseries = {New atcTimeseries(Nothing), New atcTimeseries(Nothing)}
        target = New atcTimeseriesGroup(aTimeseries)
        actual = target.Clone
        Assert.AreEqual(target.Count, actual.Count)
    End Sub

    '''<summary>Test FindData</summary>
    <TestMethod()> Public Sub FindDataTest()
        Dim lTs1 As New atcTimeseries(Nothing)
        lTs1.Attributes.SetValue("ID", 1)
        lTs1.Attributes.SetValue("Constituent", "PREC")
        Dim lTs2 As New atcTimeseries(Nothing)
        lTs2.Attributes.SetValue("ID", 2)
        lTs2.Attributes.SetValue("Constituent", "ATEM")
        Dim lTs3 As New atcTimeseries(Nothing)
        lTs3.Attributes.SetValue("ID", 3)
        lTs3.Attributes.SetValue("Constituent", "WIND")

        Dim target As atcTimeseriesGroup = New atcTimeseriesGroup() ' TODO: Initialize to an appropriate value
        target.Add(lTs1)
        target.Add(lTs2)
        target.Add(lTs3)
        Dim aAttributeName As String = "Constituent"
        Dim aValue As String = "ATEM"
        Dim aLimit As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As atcTimeseriesGroup = target.FindData(aAttributeName, aValue, aLimit)
        Assert.AreEqual(2, actual(0).Attributes.GetValue("ID"))
    End Sub

    '''<summary>Test FindData</summary>
    <TestMethod()> Public Sub FindDataTest1()
        Dim lTs1 As New atcTimeseries(Nothing)
        lTs1.Attributes.SetValue("ID", 1)
        lTs1.Attributes.SetValue("Constituent", "PREC")
        Dim lTs2 As New atcTimeseries(Nothing)
        lTs2.Attributes.SetValue("ID", 2)
        lTs2.Attributes.SetValue("Constituent", "ATEM")
        Dim lTs3 As New atcTimeseries(Nothing)
        lTs3.Attributes.SetValue("ID", 3)
        lTs3.Attributes.SetValue("Constituent", "WIND")

        Dim target As atcTimeseriesGroup = New atcTimeseriesGroup() ' TODO: Initialize to an appropriate value
        target.Add(lTs1)
        target.Add(lTs2)
        target.Add(lTs3)
        Dim aAttributeName As String = "Constituent"
        Dim lAtts() As IEnumerable = {"ATEM", "WIND"}
        Dim aValue As New atcCollection(lAtts)
        Dim aLimit As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As atcTimeseriesGroup = target.FindData(aAttributeName, aValue, aLimit)
        Assert.AreEqual(2, actual(0).Attributes.GetValue("ID"))
        Assert.AreEqual(3, actual(1).Attributes.GetValue("ID"))
    End Sub

    '''<summary>Test __ENCAddToList</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub __ENCAddToListTest()
        Dim value As Object = Nothing ' TODO: Initialize to an appropriate value
        atcTimeseriesGroup_Accessor.__ENCAddToList(value)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test Item</summary>
    <TestMethod()> Public Sub ItemTest()
        Dim lTs1 As New atcTimeseries(Nothing)
        lTs1.Attributes.SetValue("ID", 1)
        lTs1.Attributes.SetValue("Constituent", "PREC")
        Dim lTs2 As New atcTimeseries(Nothing)
        lTs2.Attributes.SetValue("ID", 2)
        lTs2.Attributes.SetValue("Constituent", "ATEM")
        Dim lTs3 As New atcTimeseries(Nothing)
        lTs3.Attributes.SetValue("ID", 3)
        lTs3.Attributes.SetValue("Constituent", "WIND")

        Dim target As atcTimeseriesGroup = New atcTimeseriesGroup() ' TODO: Initialize to an appropriate value
        target.Add(lTs1)
        target.Add(lTs2)
        target.Add(lTs3)

        Assert.AreEqual("WIND", target.Item(2).Attributes.GetValue("Constituent"))

    End Sub

    '''<summary>Test ItemByIndex</summary>
    <TestMethod()> Public Sub ItemByIndexTest()
        Dim lTs1 As New atcTimeseries(Nothing)
        lTs1.Attributes.SetValue("ID", 1)
        lTs1.Attributes.SetValue("Constituent", "PREC")
        Dim lTs2 As New atcTimeseries(Nothing)
        lTs2.Attributes.SetValue("ID", 2)
        lTs2.Attributes.SetValue("Constituent", "ATEM")
        Dim lTs3 As New atcTimeseries(Nothing)
        lTs3.Attributes.SetValue("ID", 3)
        lTs3.Attributes.SetValue("Constituent", "WIND")

        Dim target As atcTimeseriesGroup = New atcTimeseriesGroup() ' TODO: Initialize to an appropriate value
        target.Add(lTs1)
        target.Add(lTs2)
        target.Add(lTs3)

        Assert.AreEqual("WIND", target.ItemByIndex(2).Attributes.GetValue("Constituent"))
    End Sub

    '''<summary>Test ItemByKey</summary>
    <TestMethod()> Public Sub ItemByKeyTest()
        Dim lTs1 As New atcTimeseries(Nothing)
        lTs1.Attributes.SetValue("ID", 1)
        lTs1.Attributes.SetValue("Constituent", "PREC")
        Dim lTs2 As New atcTimeseries(Nothing)
        lTs2.Attributes.SetValue("ID", 2)
        lTs2.Attributes.SetValue("Constituent", "ATEM")
        Dim lTs3 As New atcTimeseries(Nothing)
        lTs3.Attributes.SetValue("ID", 3)
        lTs3.Attributes.SetValue("Constituent", "WIND")

        Dim target As atcTimeseriesGroup = New atcTimeseriesGroup() ' TODO: Initialize to an appropriate value
        target.Add("1", lTs1)
        target.Add("2", lTs2)
        target.Add("3", lTs3)

        Assert.AreEqual("ATEM", target.ItemByKey("2").Attributes.GetValue("Constituent"))
    End Sub
End Class
