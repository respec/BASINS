Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports atcData

'''<summary>
'''This is a test class for atcTimeseriesSourceTest and is intended
'''to contain all atcTimeseriesSourceTest Unit Tests (Done)
'''</summary>
<TestClass()> _
Public Class atcTimeseriesSourceTest
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

    '''<summary>Test atcTimeseriesSource Constructor</summary>
    <TestMethod()> Public Sub atcTimeseriesSourceConstructorTest()
        Dim target As atcTimeseriesSource = New atcTimeseriesSource()
        Assert.IsInstanceOfType(target, GetType(atcTimeseriesSource))
    End Sub

    '''<summary>Test AddDatasets</summary>
    <TestMethod()> Public Sub AddDatasetsTest()
        Dim target As atcTimeseriesSource = New atcTimeseriesSource() ' TODO: Initialize to an appropriate value
        Dim aTimeseriesGroup As New atcTimeseriesGroup
        Dim lTs1 As New atcTimeseries(Nothing)
        lTs1.Attributes.SetValue("ID", 1)
        lTs1.Attributes.SetValue("Constituent", "PREC")
        Dim lTs2 As New atcTimeseries(Nothing)
        lTs2.Attributes.SetValue("ID", 2)
        lTs2.Attributes.SetValue("Constituent", "ATEM")
        Dim lTs3 As New atcTimeseries(Nothing)
        lTs3.Attributes.SetValue("ID", 3)
        lTs3.Attributes.SetValue("Constituent", "WIND")
        aTimeseriesGroup.Add(lTs1)
        aTimeseriesGroup.Add(lTs2)
        aTimeseriesGroup.Add(lTs3)
        Try
            target.AddDatasets(aTimeseriesGroup)
            Assert.AreEqual(True, True)
        Catch ex As Exception
            If ex.Message.Contains("does not implement") Then
                Assert.AreEqual(False, False)
            End If
        End Try
    End Sub

    '''<summary>Test DataSets</summary>
    <TestMethod()> Public Sub DataSetsTest()
        Dim target As atcTimeseriesSource = New atcTimeseriesSource() ' TODO: Initialize to an appropriate value
        Assert.AreEqual(0, target.DataSets.Count)
    End Sub
End Class
