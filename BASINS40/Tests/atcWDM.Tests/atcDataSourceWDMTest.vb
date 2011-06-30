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
    '''A test for atcDataSourceWDM Constructor
    '''</summary>
    <TestMethod()> _
    Public Sub atcDataSourceWDMConstructorTest()
        Dim target As atcDataSourceWDM = New atcDataSourceWDM()
        Assert.Inconclusive("TODO: Implement code to verify target")
    End Sub

    '''<summary>
    '''A test for AddDataSet
    '''</summary>
    <TestMethod()> _
    Public Sub AddDataSetTest()
        Dim target As atcDataSourceWDM = New atcDataSourceWDM() ' TODO: Initialize to an appropriate value
        Dim aDataSet As atcDataSet = Nothing ' TODO: Initialize to an appropriate value
        Dim aExistAction As atcDataSource.EnumExistAction = New atcDataSource.EnumExistAction() ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = target.AddDataset(aDataSet, aExistAction)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for AddDatasets
    '''</summary>
    <TestMethod()> _
    Public Sub AddDatasetsTest()
        Dim target As atcDataSourceWDM = New atcDataSourceWDM() ' TODO: Initialize to an appropriate value
        Dim aDataGroup As atcTimeseriesGroup = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = target.AddDatasets(aDataGroup)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for AttrStored
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcWDM.dll")> _
    Public Sub AttrStoredTest()
        Dim target As atcDataSourceWDM_Accessor = New atcDataSourceWDM_Accessor() ' TODO: Initialize to an appropriate value
        Dim aSaind As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = target.AttrStored(aSaind)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for AttrVal2String
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcWDM.dll")> _
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

    '''<summary>
    '''A test for Clear
    '''</summary>
    <TestMethod()> _
    Public Sub ClearTest()
        Dim target As atcDataSourceWDM = New atcDataSourceWDM() ' TODO: Initialize to an appropriate value
        target.Clear()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for DateToyyyyMMddHHmmss
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcWDM.dll")> _
    Public Sub DateToyyyyMMddHHmmssTest()
        Dim target As atcDataSourceWDM_Accessor = New atcDataSourceWDM_Accessor() ' TODO: Initialize to an appropriate value
        Dim aDate As DateTime = New DateTime() ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = target.DateToyyyyMMddHHmmss(aDate)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for DsnBld
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcWDM.dll")> _
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

    '''<summary>
    '''A test for DsnReadGeneral
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcWDM.dll")> _
    Public Sub DsnReadGeneralTest()
        Dim target As atcDataSourceWDM_Accessor = New atcDataSourceWDM_Accessor() ' TODO: Initialize to an appropriate value
        Dim aFileUnit As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aDataset As atcTimeseries = Nothing ' TODO: Initialize to an appropriate value
        target.DsnReadGeneral(aFileUnit, aDataset)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for DsnWriteAttribute
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcWDM.dll")> _
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

    '''<summary>
    '''A test for DsnWriteAttributes
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcWDM.dll")> _
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

    '''<summary>
    '''A test for Finalize
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcWDM.dll")> _
    Public Sub FinalizeTest()
        Dim target As atcDataSourceWDM_Accessor = New atcDataSourceWDM_Accessor() ' TODO: Initialize to an appropriate value
        target.Finalize()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for MemUsage
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcWDM.dll")> _
    Public Sub MemUsageTest()
        Dim target As atcDataSourceWDM_Accessor = New atcDataSourceWDM_Accessor() ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = target.MemUsage
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for Open
    '''</summary>
    <TestMethod()> _
    Public Sub OpenTest()
        Dim target As atcDataSourceWDM = New atcDataSourceWDM() ' TODO: Initialize to an appropriate value
        Dim aFileName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aAttributes As atcDataAttributes = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = target.Open(aFileName, aAttributes)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for ReadData
    '''</summary>
    <TestMethod()> _
    Public Sub ReadDataTest()
        Dim target As atcDataSourceWDM = New atcDataSourceWDM() ' TODO: Initialize to an appropriate value
        Dim aReadMe As atcDataSet = Nothing ' TODO: Initialize to an appropriate value
        target.ReadData(aReadMe)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for Refresh
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcWDM.dll")> _
    Public Sub RefreshTest()
        Dim target As atcDataSourceWDM_Accessor = New atcDataSourceWDM_Accessor() ' TODO: Initialize to an appropriate value
        Dim aWdmUnit As Integer = 0 ' TODO: Initialize to an appropriate value
        target.Refresh(aWdmUnit)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for RefreshDsn
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcWDM.dll")> _
    Public Sub RefreshDsnTest()
        Dim target As atcDataSourceWDM_Accessor = New atcDataSourceWDM_Accessor() ' TODO: Initialize to an appropriate value
        Dim aFileUnit As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aDsn As Integer = 0 ' TODO: Initialize to an appropriate value
        target.RefreshDsn(aFileUnit, aDsn)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for RemoveDataset
    '''</summary>
    <TestMethod()> _
    Public Sub RemoveDatasetTest()
        Dim target As atcDataSourceWDM = New atcDataSourceWDM() ' TODO: Initialize to an appropriate value
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
        Dim target As atcDataSourceWDM = New atcDataSourceWDM() ' TODO: Initialize to an appropriate value
        Dim SaveFileName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim ExistAction As atcDataSource.EnumExistAction = New atcDataSource.EnumExistAction() ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = target.Save(SaveFileName, ExistAction)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for View
    '''</summary>
    <TestMethod()> _
    Public Sub ViewTest()
        Dim target As atcDataSourceWDM = New atcDataSourceWDM() ' TODO: Initialize to an appropriate value
        target.View()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for WriteAttribute
    '''</summary>
    <TestMethod()> _
    Public Sub WriteAttributeTest()
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

    '''<summary>
    '''A test for WriteAttributes
    '''</summary>
    <TestMethod()> _
    Public Sub WriteAttributesTest()
        Dim target As atcDataSourceWDM = New atcDataSourceWDM() ' TODO: Initialize to an appropriate value
        Dim aDataSet As atcDataSet = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = target.WriteAttributes(aDataSet)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for findNextDsn
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcWDM.dll")> _
    Public Sub findNextDsnTest()
        Dim target As atcDataSourceWDM_Accessor = New atcDataSourceWDM_Accessor() ' TODO: Initialize to an appropriate value
        Dim aDsn As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim expected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        actual = target.findNextDsn(aDsn)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for CanOpen
    '''</summary>
    <TestMethod()> _
    Public Sub CanOpenTest()
        Dim target As atcDataSourceWDM = New atcDataSourceWDM() ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = target.CanOpen
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for CanRemoveDataset
    '''</summary>
    <TestMethod()> _
    Public Sub CanRemoveDatasetTest()
        Dim target As atcDataSourceWDM = New atcDataSourceWDM() ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = target.CanRemoveDataset
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for CanSave
    '''</summary>
    <TestMethod()> _
    Public Sub CanSaveTest()
        Dim target As atcDataSourceWDM = New atcDataSourceWDM() ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = target.CanSave
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for Category
    '''</summary>
    <TestMethod()> _
    Public Sub CategoryTest()
        Dim target As atcDataSourceWDM = New atcDataSourceWDM() ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = target.Category
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for Description
    '''</summary>
    <TestMethod()> _
    Public Sub DescriptionTest()
        Dim target As atcDataSourceWDM = New atcDataSourceWDM() ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = target.Description
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for Name
    '''</summary>
    <TestMethod()> _
    Public Sub NameTest()
        Dim target As atcDataSourceWDM = New atcDataSourceWDM() ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = target.Name
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub
End Class
