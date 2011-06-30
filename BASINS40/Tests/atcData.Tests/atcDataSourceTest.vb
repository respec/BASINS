Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports atcData



'''<summary>
'''This is a test class for atcDataSourceTest and is intended
'''to contain all atcDataSourceTest Unit Tests
'''</summary>
<TestClass()> _
Public Class atcDataSourceTest


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
    '''A test for atcDataSource Constructor
    '''</summary>
    <TestMethod()> _
    Public Sub atcDataSourceConstructorTest()
        Dim target As atcDataSource = New atcDataSource()
        Assert.Inconclusive("TODO: Implement code to verify target")
    End Sub

    '''<summary>
    '''A test for AddDataSet
    '''</summary>
    <TestMethod()> _
    Public Sub AddDataSetTest()
        Dim target As atcDataSource = New atcDataSource() ' TODO: Initialize to an appropriate value
        Dim aDs As atcDataSet = Nothing ' TODO: Initialize to an appropriate value
        Dim aExistAction As atcDataSource.EnumExistAction = New atcDataSource.EnumExistAction() ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = target.AddDataSet(aDs, aExistAction)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for AddDataSets
    '''</summary>
    <TestMethod()> _
    Public Sub AddDataSetsTest()
        Dim target As atcDataSource = New atcDataSource() ' TODO: Initialize to an appropriate value
        Dim aDataGroup As atcDataGroup = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = target.AddDataSets(aDataGroup)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for Clear
    '''</summary>
    <TestMethod()> _
    Public Sub ClearTest()
        Dim target As atcDataSource = New atcDataSource() ' TODO: Initialize to an appropriate value
        target.Clear()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for Finalize
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub FinalizeTest()
        Dim target As atcDataSource_Accessor = New atcDataSource_Accessor() ' TODO: Initialize to an appropriate value
        target.Finalize()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for Open
    '''</summary>
    <TestMethod()> _
    Public Sub OpenTest()
        Dim target As atcDataSource = New atcDataSource() ' TODO: Initialize to an appropriate value
        Dim aSpecification As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aAttributes As atcDataAttributes = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = target.Open(aSpecification, aAttributes)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for ReadData
    '''</summary>
    <TestMethod()> _
    Public Sub ReadDataTest()
        Dim target As atcDataSource = New atcDataSource() ' TODO: Initialize to an appropriate value
        Dim aData As atcDataSet = Nothing ' TODO: Initialize to an appropriate value
        target.ReadData(aData)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for RemoveDataset
    '''</summary>
    <TestMethod()> _
    Public Sub RemoveDatasetTest()
        Dim target As atcDataSource = New atcDataSource() ' TODO: Initialize to an appropriate value
        Dim aDataSet As atcDataSet = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = target.RemoveDataset(aDataSet)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for Save
    '''</summary>
    <TestMethod()> _
    Public Sub SaveTest()
        Dim target As atcDataSource = New atcDataSource() ' TODO: Initialize to an appropriate value
        Dim aSaveFileName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aExistAction As atcDataSource.EnumExistAction = New atcDataSource.EnumExistAction() ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = target.Save(aSaveFileName, aExistAction)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for ToString
    '''</summary>
    <TestMethod()> _
    Public Sub ToStringTest()
        Dim target As atcDataSource = New atcDataSource() ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = target.ToString
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for View
    '''</summary>
    <TestMethod()> _
    Public Sub ViewTest()
        Dim target As atcDataSource = New atcDataSource() ' TODO: Initialize to an appropriate value
        target.View()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for Attributes
    '''</summary>
    <TestMethod()> _
    Public Sub AttributesTest()
        Dim target As atcDataSource = New atcDataSource() ' TODO: Initialize to an appropriate value
        Dim actual As atcDataAttributes
        actual = target.Attributes
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for AvailableOperations
    '''</summary>
    <TestMethod()> _
    Public Sub AvailableOperationsTest()
        Dim target As atcDataSource = New atcDataSource() ' TODO: Initialize to an appropriate value
        Dim actual As atcDataAttributes
        actual = target.AvailableOperations
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for CanOpen
    '''</summary>
    <TestMethod()> _
    Public Sub CanOpenTest()
        Dim target As atcDataSource = New atcDataSource() ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = target.CanOpen
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for CanRemoveDataset
    '''</summary>
    <TestMethod()> _
    Public Sub CanRemoveDatasetTest()
        Dim target As atcDataSource = New atcDataSource() ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = target.CanRemoveDataset
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for CanSave
    '''</summary>
    <TestMethod()> _
    Public Sub CanSaveTest()
        Dim target As atcDataSource = New atcDataSource() ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = target.CanSave
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for DataSets
    '''</summary>
    <TestMethod()> _
    Public Sub DataSetsTest()
        Dim target As atcDataSource = New atcDataSource() ' TODO: Initialize to an appropriate value
        Dim actual As atcDataGroup
        actual = target.DataSets
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for Filter
    '''</summary>
    <TestMethod()> _
    Public Sub FilterTest()
        Dim target As atcDataSource = New atcDataSource() ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        target.Filter = expected
        actual = target.Filter
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for Specification
    '''</summary>
    <TestMethod()> _
    Public Sub SpecificationTest()
        Dim target As atcDataSource = New atcDataSource() ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        target.Specification = expected
        actual = target.Specification
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub
End Class
