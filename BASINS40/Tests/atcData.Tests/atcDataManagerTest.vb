Imports System.Drawing
Imports System.Collections.Generic
Imports atcUtility
Imports System
Imports System.Collections
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports atcData

'''<summary>
'''This is a test class for atcDataManagerTest and is intended
'''to contain all atcDataManagerTest Unit Tests
'''</summary>
<TestClass()> _
Public Class atcDataManagerTest
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

    '''<summary>Test atcDataManager Constructor</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub atcDataManagerConstructorTest()
        Dim target As atcDataManager_Accessor = New atcDataManager_Accessor()
        Assert.Inconclusive("TODO: Implement code to verify target")
    End Sub

    '''<summary>Test Clear</summary>
    <TestMethod()> Public Sub ClearTest()
        atcDataManager.Clear()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test DataSets</summary>
    <TestMethod()> Public Sub DataSetsTest()
        Dim expected As atcTimeseriesGroup = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcTimeseriesGroup
        actual = atcDataManager.DataSets
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test DataSourceByName</summary>
    <TestMethod()> Public Sub DataSourceByNameTest()
        Dim aDataSourceName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcTimeseriesSource
        actual = atcDataManager.DataSourceByName(aDataSourceName)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test DataSourceBySpecification</summary>
    <TestMethod()> Public Sub DataSourceBySpecificationTest()
        Dim aSpecification As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcTimeseriesSource
        actual = atcDataManager.DataSourceBySpecification(aSpecification)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test DisplayAttributesSet</summary>
    <TestMethod()> Public Sub DisplayAttributesSetTest()
        Dim aNewValues As IEnumerable = Nothing ' TODO: Initialize to an appropriate value
        atcDataManager.DisplayAttributesSet(aNewValues)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test GetPlugins</summary>
    <TestMethod()> Public Sub GetPluginsTest()
        Dim aBaseType As Type = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As atcCollection = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcCollection
        actual = atcDataManager.GetPlugins(aBaseType)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test LoadPlugin</summary>
    <TestMethod()> Public Sub LoadPluginTest()
        Dim aPluginName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = atcDataManager.LoadPlugin(aPluginName)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test OpenDataSource</summary>
    <TestMethod()> Public Sub OpenDataSourceTest()
        Dim aNewSource As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim aSpecification As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aAttributes As atcDataAttributes = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = atcDataManager.OpenDataSource(aNewSource, aSpecification, aAttributes)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test OpenDataSource</summary>
    <TestMethod()> Public Sub OpenDataSourceTest1()
        Dim aSpecification As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = atcDataManager.OpenDataSource(aSpecification)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test RemoveDataSource</summary>
    <TestMethod()> Public Sub RemoveDataSourceTest()
        Dim aSpecification As String = String.Empty ' TODO: Initialize to an appropriate value
        atcDataManager.RemoveDataSource(aSpecification)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test RemoveDataSource</summary>
    <TestMethod()> Public Sub RemoveDataSourceTest1()
        Dim aIndex As Integer = 0 ' TODO: Initialize to an appropriate value
        atcDataManager.RemoveDataSource(aIndex)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test RemoveDataSource</summary>
    <TestMethod()> Public Sub RemoveDataSourceTest2()
        Dim aDataSource As atcDataSource = Nothing ' TODO: Initialize to an appropriate value
        atcDataManager.RemoveDataSource(aDataSource)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test RemoveDuplicates</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub RemoveDuplicatesTest()
        Dim aStringList As List(Of String) = Nothing ' TODO: Initialize to an appropriate value
        atcDataManager_Accessor.RemoveDuplicates(aStringList)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test RemoveMenuIfEmpty</summary>
    <TestMethod()> Public Sub RemoveMenuIfEmptyTest()
        Dim aMenuName As String = String.Empty ' TODO: Initialize to an appropriate value
        atcDataManager.RemoveMenuIfEmpty(aMenuName)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test RemovedDataSource</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub RemovedDataSourceTest()
        Dim aDataSource As atcDataSource = Nothing ' TODO: Initialize to an appropriate value
        atcDataManager_Accessor.RemovedDataSource(aDataSource)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test SelectionAttributesSet</summary>
    <TestMethod()> Public Sub SelectionAttributesSetTest()
        Dim aNewValues As IEnumerable = Nothing ' TODO: Initialize to an appropriate value
        atcDataManager.SelectionAttributesSet(aNewValues)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test ShowDisplay</summary>
    <TestMethod()> Public Sub ShowDisplayTest()
        Dim aDisplayName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aTimeseriesGroup As atcTimeseriesGroup = Nothing ' TODO: Initialize to an appropriate value
        atcDataManager.ShowDisplay(aDisplayName, aTimeseriesGroup)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test ToString</summary>
    <TestMethod()> Public Sub ToStringTest()
        Dim target As atcDataManager_Accessor = New atcDataManager_Accessor() ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = target.ToString
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test UserManage</summary>
    <TestMethod()> Public Sub UserManageTest()
        Dim aTitle As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aDefaultIndex As Integer = 0 ' TODO: Initialize to an appropriate value
        atcDataManager.UserManage(aTitle, aDefaultIndex)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test UserOpenDataFile</summary>
    <TestMethod()> Public Sub UserOpenDataFileTest()
        Dim aNeedToOpen As Boolean = False ' TODO: Initialize to an appropriate value
        Dim aNeedToSave As Boolean = False ' TODO: Initialize to an appropriate value
        Dim expected As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcTimeseriesSource
        actual = atcDataManager.UserOpenDataFile(aNeedToOpen, aNeedToSave)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test UserSaveData</summary>
    <TestMethod()> Public Sub UserSaveDataTest()
        Dim aSpecification As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = atcDataManager.UserSaveData(aSpecification)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test UserSelectData</summary>
    <TestMethod()> Public Sub UserSelectDataTest()
        Dim aTitle As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aSelected As atcDataGroup = Nothing ' TODO: Initialize to an appropriate value
        Dim aAvailable As atcDataGroup = Nothing ' TODO: Initialize to an appropriate value
        Dim aModal As Boolean = False ' TODO: Initialize to an appropriate value
        Dim aCancelReturnsOriginalSelected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim aIcon As Icon = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As atcDataGroup = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcDataGroup
        actual = atcDataManager.UserSelectData(aTitle, aSelected, aAvailable, aModal, aCancelReturnsOriginalSelected, aIcon)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test UserSelectData</summary>
    <TestMethod()> Public Sub UserSelectDataTest1()
        Dim aTitle As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aSelected As atcDataGroup = Nothing ' TODO: Initialize to an appropriate value
        Dim aAvailable As atcDataGroup = Nothing ' TODO: Initialize to an appropriate value
        Dim aModal As Boolean = False ' TODO: Initialize to an appropriate value
        Dim aCancelReturnsOriginalSelected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim expected As atcDataGroup = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcDataGroup
        actual = atcDataManager.UserSelectData(aTitle, aSelected, aAvailable, aModal, aCancelReturnsOriginalSelected)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test UserSelectDataSource</summary>
    <TestMethod()> Public Sub UserSelectDataSourceTest()
        Dim aCategories As ArrayList = Nothing ' TODO: Initialize to an appropriate value
        Dim aTitle As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aNeedToOpen As Boolean = False ' TODO: Initialize to an appropriate value
        Dim aNeedToSave As Boolean = False ' TODO: Initialize to an appropriate value
        Dim expected As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcTimeseriesSource
        actual = atcDataManager.UserSelectDataSource(aCategories, aTitle, aNeedToOpen, aNeedToSave)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test UserSelectDisplay</summary>
    <TestMethod()> Public Sub UserSelectDisplayTest()
        Dim aTitle As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aDataGroup As atcTimeseriesGroup = Nothing ' TODO: Initialize to an appropriate value
        atcDataManager.UserSelectDisplay(aTitle, aDataGroup)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test __ENCAddToList</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub __ENCAddToListTest()
        Dim value As Object = Nothing ' TODO: Initialize to an appropriate value
        atcDataManager_Accessor.__ENCAddToList(value)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test DataSources</summary>
    <TestMethod()> Public Sub DataSourcesTest()
        Dim actual As ArrayList
        actual = atcDataManager.DataSources
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test DisplayAttributes</summary>
    <TestMethod()> Public Sub DisplayAttributesTest()
        Dim actual As List(Of String)
        actual = atcDataManager.DisplayAttributes
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test SelectionAttributes</summary>
    <TestMethod()> Public Sub SelectionAttributesTest()
        Dim actual As List(Of String)
        actual = atcDataManager.SelectionAttributes
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test XML</summary>
    <TestMethod()> Public Sub XMLTest()
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        atcDataManager.XML = expected
        actual = atcDataManager.XML
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub
End Class
