Imports System
Imports atcData
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports atcWDM

'''<summary>
'''This is a test class for atcDataSourceWDMTest and is intended
'''to contain all atcDataSourceWDMTest Unit Tests
'''</summary>
<TestClass()> _
Public Class atcDataSourceWDMTest
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
            testContextInstance = value
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

    '''<summary>Test atcDataSourceWDM Constructor</summary>
    <TestMethod()> Public Sub atcDataSourceWDMConstructorTest()
        Dim target As atcDataSourceWDM = New atcDataSourceWDM()
        Assert.Inconclusive("TODO: Implement code to verify target")
    End Sub

    '''<summary>Test AddDataSet</summary>
    <TestMethod()> Public Sub AddDataSetTest()
        Dim target As atcDataSourceWDM = New atcDataSourceWDM() ' TODO: Initialize to an appropriate value
        Dim aDataSet As atcDataSet = Nothing ' TODO: Initialize to an appropriate value
        Dim aExistAction As atcDataSource.EnumExistAction = New atcDataSource.EnumExistAction() ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = target.AddDataset(aDataSet, aExistAction)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test AddDatasets</summary>
    <TestMethod()> Public Sub AddDatasetsTest()
        Dim target As atcDataSourceWDM = New atcDataSourceWDM() ' TODO: Initialize to an appropriate value
        Dim aDataGroup As atcTimeseriesGroup = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = target.AddDatasets(aDataGroup)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test AttrStored</summary>
    <TestMethod(), DeploymentItem("atcWDM.dll")> _
    Public Sub AttrStoredTest()
        Dim target As atcDataSourceWDM_Accessor = New atcDataSourceWDM_Accessor() ' TODO: Initialize to an appropriate value
        Dim aSaind As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = target.AttrStored(aSaind)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test AttrVal2String</summary>
    <TestMethod(), DeploymentItem("atcWDM.dll")> _
    Public Sub AttrVal2StringTest()
        Dim target As atcDataSourceWDM_Accessor = New atcDataSourceWDM_Accessor() ' TODO: Initialize to an appropriate value
        Dim aSaInd As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aSaVal() As Integer = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = target.AttrVal2String(aSaInd, aSaVal)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test Clear</summary>
    <TestMethod()> Public Sub ClearTest()
        Dim target As atcDataSourceWDM = New atcDataSourceWDM() ' TODO: Initialize to an appropriate value
        target.Clear()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test DateToyyyyMMddHHmmss</summary>
    <TestMethod(), DeploymentItem("atcWDM.dll")> _
    Public Sub DateToyyyyMMddHHmmssTest()
        Dim target As atcDataSourceWDM_Accessor = New atcDataSourceWDM_Accessor() ' TODO: Initialize to an appropriate value
        Dim aDate As DateTime = New DateTime() ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = target.DateToyyyyMMddHHmmss(aDate)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test DsnBld</summary>
    <TestMethod(), DeploymentItem("atcWDM.dll")> _
    Public Sub DsnBldTest()
        Dim target As atcDataSourceWDM_Accessor = New atcDataSourceWDM_Accessor() ' TODO: Initialize to an appropriate value
        Dim aFileUnit As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aTs As atcTimeseries = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = target.DsnBld(aFileUnit, aTs)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test DsnReadGeneral</summary>
    <TestMethod(), DeploymentItem("atcWDM.dll")> _
    Public Sub DsnReadGeneralTest()
        Dim target As atcDataSourceWDM_Accessor = New atcDataSourceWDM_Accessor() ' TODO: Initialize to an appropriate value
        Dim aFileUnit As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aDataset As atcTimeseries = Nothing ' TODO: Initialize to an appropriate value
        target.DsnReadGeneral(aFileUnit, aDataset)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test DsnWriteAttribute</summary>
    <TestMethod(), DeploymentItem("atcWDM.dll")> _
    Public Sub DsnWriteAttributeTest()
        Dim target As atcDataSourceWDM_Accessor = New atcDataSourceWDM_Accessor() ' TODO: Initialize to an appropriate value
        Dim aFileUnit As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aMsgUnit As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aDsn As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aAttribute As atcDefinedValue = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = target.DsnWriteAttribute(aFileUnit, aMsgUnit, aDsn, aAttribute)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test DsnWriteAttributes</summary>
    <TestMethod(), DeploymentItem("atcWDM.dll")> _
    Public Sub DsnWriteAttributesTest()
        Dim target As atcDataSourceWDM_Accessor = New atcDataSourceWDM_Accessor() ' TODO: Initialize to an appropriate value
        Dim aFileUnit As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aTs As atcTimeseries = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = target.DsnWriteAttributes(aFileUnit, aTs)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test Finalize</summary>
    <TestMethod(), DeploymentItem("atcWDM.dll")> _
    Public Sub FinalizeTest()
        Dim target As atcDataSourceWDM_Accessor = New atcDataSourceWDM_Accessor() ' TODO: Initialize to an appropriate value
        target.Finalize()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test MemUsage</summary>
    <TestMethod(), DeploymentItem("atcWDM.dll")> _
    Public Sub MemUsageTest()
        Dim target As atcDataSourceWDM_Accessor = New atcDataSourceWDM_Accessor() ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = target.MemUsage
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test Open</summary>
    <TestMethod()> Public Sub OpenTest()
        Dim target As atcDataSourceWDM = New atcDataSourceWDM() ' TODO: Initialize to an appropriate value
        Dim aFileName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aAttributes As atcDataAttributes = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = target.Open(aFileName, aAttributes)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test ReadData</summary>
    <TestMethod()> Public Sub ReadDataTest()
        Dim target As atcDataSourceWDM = New atcDataSourceWDM() ' TODO: Initialize to an appropriate value
        Dim aReadMe As atcDataSet = Nothing ' TODO: Initialize to an appropriate value
        target.ReadData(aReadMe)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test Refresh</summary>
    <TestMethod(), DeploymentItem("atcWDM.dll")> _
    Public Sub RefreshTest()
        Dim target As atcDataSourceWDM_Accessor = New atcDataSourceWDM_Accessor() ' TODO: Initialize to an appropriate value
        Dim aWdmUnit As Integer = 0 ' TODO: Initialize to an appropriate value
        target.Refresh(aWdmUnit)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test RefreshDsn</summary>
    <TestMethod(), DeploymentItem("atcWDM.dll")> _
    Public Sub RefreshDsnTest()
        Dim target As atcDataSourceWDM_Accessor = New atcDataSourceWDM_Accessor() ' TODO: Initialize to an appropriate value
        Dim aFileUnit As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aDsn As Integer = 0 ' TODO: Initialize to an appropriate value
        target.RefreshDsn(aFileUnit, aDsn)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test RemoveDataset</summary>
    <TestMethod()> Public Sub RemoveDatasetTest()
        Dim target As atcDataSourceWDM = New atcDataSourceWDM() ' TODO: Initialize to an appropriate value
        Dim aDataSet As atcDataSet = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = target.RemoveDataset(aDataSet)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test Save</summary>
    <TestMethod()> Public Sub SaveTest()
        Dim target As atcDataSourceWDM = New atcDataSourceWDM() ' TODO: Initialize to an appropriate value
        Dim SaveFileName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim ExistAction As atcDataSource.EnumExistAction = New atcDataSource.EnumExistAction() ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = target.Save(SaveFileName, ExistAction)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test View</summary>
    <TestMethod()> Public Sub ViewTest()
        Dim target As atcDataSourceWDM = New atcDataSourceWDM() ' TODO: Initialize to an appropriate value
        target.View()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test WriteAttribute</summary>
    <TestMethod()> Public Sub WriteAttributeTest()
        Dim target As atcDataSourceWDM = New atcDataSourceWDM() ' TODO: Initialize to an appropriate value
        Dim aDataSet As atcDataSet = Nothing ' TODO: Initialize to an appropriate value
        Dim aAttribute As atcDefinedValue = Nothing ' TODO: Initialize to an appropriate value
        Dim aNewValue As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = target.WriteAttribute(aDataSet, aAttribute, aNewValue)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test WriteAttributes</summary>
    <TestMethod()> Public Sub WriteAttributesTest()
        Dim target As atcDataSourceWDM = New atcDataSourceWDM() ' TODO: Initialize to an appropriate value
        Dim aDataSet As atcDataSet = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = target.WriteAttributes(aDataSet)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test findNextDsn</summary>
    <TestMethod(), DeploymentItem("atcWDM.dll")> _
    Public Sub findNextDsnTest()
        Dim target As atcDataSourceWDM_Accessor = New atcDataSourceWDM_Accessor() ' TODO: Initialize to an appropriate value
        Dim aDsn As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim expected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        actual = target.findNextDsn(aDsn)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test CanOpen</summary>
    <TestMethod()> Public Sub CanOpenTest()
        Dim target As atcDataSourceWDM = New atcDataSourceWDM() ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = target.CanOpen
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test CanRemoveDataset</summary>
    <TestMethod()> Public Sub CanRemoveDatasetTest()
        Dim target As atcDataSourceWDM = New atcDataSourceWDM() ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = target.CanRemoveDataset
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test CanSave</summary>
    <TestMethod()> Public Sub CanSaveTest()
        Dim target As atcDataSourceWDM = New atcDataSourceWDM() ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = target.CanSave
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test Category</summary>
    <TestMethod()> Public Sub CategoryTest()
        Dim target As atcDataSourceWDM = New atcDataSourceWDM() ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = target.Category
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test Description</summary>
    <TestMethod()> Public Sub DescriptionTest()
        Dim target As atcDataSourceWDM = New atcDataSourceWDM() ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = target.Description
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test Name</summary>
    <TestMethod()> Public Sub NameTest()
        Dim target As atcDataSourceWDM = New atcDataSourceWDM() ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = target.Name
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub
End Class
