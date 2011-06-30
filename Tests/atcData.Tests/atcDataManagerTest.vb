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


    '''<summary>
    '''A test for atcDataManager Constructor
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub atcDataManagerConstructorTest()
        Dim target As atcDataManager_Accessor = New atcDataManager_Accessor()
        Assert.Inconclusive("TODO: Implement code to verify target")
    End Sub

    '''<summary>
    '''A test for Clear
    '''</summary>
    <TestMethod()> _
    Public Sub ClearTest()
        atcDataManager.Clear()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for DataSets
    '''</summary>
    <TestMethod()> _
    Public Sub DataSetsTest()
        Dim expected As atcTimeseriesGroup = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcTimeseriesGroup
        actual = atcDataManager.DataSets
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for DataSourceByName
    '''</summary>
    <TestMethod()> _
    Public Sub DataSourceByNameTest()
        Dim aDataSourceName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcTimeseriesSource
        actual = atcDataManager.DataSourceByName(aDataSourceName)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for DataSourceBySpecification
    '''</summary>
    <TestMethod()> _
    Public Sub DataSourceBySpecificationTest()
        Dim aSpecification As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcTimeseriesSource
        actual = atcDataManager.DataSourceBySpecification(aSpecification)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for DisplayAttributesSet
    '''</summary>
    <TestMethod()> _
    Public Sub DisplayAttributesSetTest()
        Dim aNewValues As IEnumerable = Nothing ' TODO: Initialize to an appropriate value
        atcDataManager.DisplayAttributesSet(aNewValues)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for GetPlugins
    '''</summary>
    <TestMethod()> _
    Public Sub GetPluginsTest()
        Dim aBaseType As Type = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As atcCollection = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcCollection
        actual = atcDataManager.GetPlugins(aBaseType)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for LoadPlugin
    '''</summary>
    <TestMethod()> _
    Public Sub LoadPluginTest()
        Dim aPluginName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = atcDataManager.LoadPlugin(aPluginName)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for OpenDataSource
    '''</summary>
    <TestMethod()> _
    Public Sub OpenDataSourceTest()
        Dim aNewSource As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim aSpecification As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aAttributes As atcDataAttributes = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = atcDataManager.OpenDataSource(aNewSource, aSpecification, aAttributes)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for OpenDataSource
    '''</summary>
    <TestMethod()> _
    Public Sub OpenDataSourceTest1()
        Dim aSpecification As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = atcDataManager.OpenDataSource(aSpecification)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for RemoveDataSource
    '''</summary>
    <TestMethod()> _
    Public Sub RemoveDataSourceTest()
        Dim aSpecification As String = String.Empty ' TODO: Initialize to an appropriate value
        atcDataManager.RemoveDataSource(aSpecification)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for RemoveDataSource
    '''</summary>
    <TestMethod()> _
    Public Sub RemoveDataSourceTest1()
        Dim aIndex As Integer = 0 ' TODO: Initialize to an appropriate value
        atcDataManager.RemoveDataSource(aIndex)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for RemoveDataSource
    '''</summary>
    <TestMethod()> _
    Public Sub RemoveDataSourceTest2()
        Dim aDataSource As atcDataSource = Nothing ' TODO: Initialize to an appropriate value
        atcDataManager.RemoveDataSource(aDataSource)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for RemoveDuplicates
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub RemoveDuplicatesTest()
        Dim aStringList As List(Of String) = Nothing ' TODO: Initialize to an appropriate value
        atcDataManager_Accessor.RemoveDuplicates(aStringList)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for RemoveMenuIfEmpty
    '''</summary>
    <TestMethod()> _
    Public Sub RemoveMenuIfEmptyTest()
        Dim aMenuName As String = String.Empty ' TODO: Initialize to an appropriate value
        atcDataManager.RemoveMenuIfEmpty(aMenuName)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for RemovedDataSource
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub RemovedDataSourceTest()
        Dim aDataSource As atcDataSource = Nothing ' TODO: Initialize to an appropriate value
        atcDataManager_Accessor.RemovedDataSource(aDataSource)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for SelectionAttributesSet
    '''</summary>
    <TestMethod()> _
    Public Sub SelectionAttributesSetTest()
        Dim aNewValues As IEnumerable = Nothing ' TODO: Initialize to an appropriate value
        atcDataManager.SelectionAttributesSet(aNewValues)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for ShowDisplay
    '''</summary>
    <TestMethod()> _
    Public Sub ShowDisplayTest()
        Dim aDisplayName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aTimeseriesGroup As atcTimeseriesGroup = Nothing ' TODO: Initialize to an appropriate value
        atcDataManager.ShowDisplay(aDisplayName, aTimeseriesGroup)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for ToString
    '''</summary>
    <TestMethod()> _
    Public Sub ToStringTest()
        Dim target As atcDataManager_Accessor = New atcDataManager_Accessor() ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = target.ToString
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for UserManage
    '''</summary>
    <TestMethod()> _
    Public Sub UserManageTest()
        Dim aTitle As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aDefaultIndex As Integer = 0 ' TODO: Initialize to an appropriate value
        atcDataManager.UserManage(aTitle, aDefaultIndex)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for UserOpenDataFile
    '''</summary>
    <TestMethod()> _
    Public Sub UserOpenDataFileTest()
        Dim aNeedToOpen As Boolean = False ' TODO: Initialize to an appropriate value
        Dim aNeedToSave As Boolean = False ' TODO: Initialize to an appropriate value
        Dim expected As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcTimeseriesSource
        actual = atcDataManager.UserOpenDataFile(aNeedToOpen, aNeedToSave)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for UserSaveData
    '''</summary>
    <TestMethod()> _
    Public Sub UserSaveDataTest()
        Dim aSpecification As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = atcDataManager.UserSaveData(aSpecification)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for UserSelectData
    '''</summary>
    <TestMethod()> _
    Public Sub UserSelectDataTest()
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

    '''<summary>
    '''A test for UserSelectData
    '''</summary>
    <TestMethod()> _
    Public Sub UserSelectDataTest1()
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

    '''<summary>
    '''A test for UserSelectDataSource
    '''</summary>
    <TestMethod()> _
    Public Sub UserSelectDataSourceTest()
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

    '''<summary>
    '''A test for UserSelectDisplay
    '''</summary>
    <TestMethod()> _
    Public Sub UserSelectDisplayTest()
        Dim aTitle As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aDataGroup As atcTimeseriesGroup = Nothing ' TODO: Initialize to an appropriate value
        atcDataManager.UserSelectDisplay(aTitle, aDataGroup)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for __ENCAddToList
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub __ENCAddToListTest()
        Dim value As Object = Nothing ' TODO: Initialize to an appropriate value
        atcDataManager_Accessor.__ENCAddToList(value)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for DataSources
    '''</summary>
    <TestMethod()> _
    Public Sub DataSourcesTest()
        Dim actual As ArrayList
        actual = atcDataManager.DataSources
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for DisplayAttributes
    '''</summary>
    <TestMethod()> _
    Public Sub DisplayAttributesTest()
        Dim actual As List(Of String)
        actual = atcDataManager.DisplayAttributes
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for SelectionAttributes
    '''</summary>
    <TestMethod()> _
    Public Sub SelectionAttributesTest()
        Dim actual As List(Of String)
        actual = atcDataManager.SelectionAttributes
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for XML
    '''</summary>
    <TestMethod()> _
    Public Sub XMLTest()
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        atcDataManager.XML = expected
        actual = atcDataManager.XML
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub
End Class
