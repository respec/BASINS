Imports System.Drawing
Imports System.Collections.Generic
Imports atcUtility
Imports System
Imports System.Collections
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports atcData

'''<summary>
'''This is a test class for atcDataManagerTest and is intended
'''to contain all atcDataManagerTest Unit Tests (Partially done, some needs user interaction)
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
        Assert.AreNotEqual(atcDataManager.DataSources, Nothing)
        Assert.AreEqual(atcDataManager.DataSources.Count, 0)
    End Sub

    '''<summary>Test DataSets</summary>
    <TestMethod()> Public Sub DataSetsTest()
        atcDataManager.Clear()
        Assert.IsInstanceOfType(atcDataManager.DataSets, GetType(atcData.atcDataGroup))
        Assert.AreNotEqual(Nothing, atcDataManager.DataSets)
        Assert.AreEqual(0, atcDataManager.DataSets.Count)
    End Sub

    '''<summary>Test DataSourceByName</summary>
    <TestMethod()> Public Sub DataSourceByNameTest()
        Dim aDataSourceName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcTimeseriesSource = atcDataManager.DataSourceByName(aDataSourceName)
        Assert.AreEqual(expected, actual)
    End Sub

    '''<summary>Test DataSourceBySpecification</summary>
    <TestMethod()> Public Sub DataSourceBySpecificationTest()
        Dim aSpecification As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcTimeseriesSource = atcDataManager.DataSourceBySpecification(aSpecification)
        Assert.AreEqual(expected, actual)
    End Sub

    '''<summary>Test DisplayAttributesSet</summary>
    <TestMethod()> Public Sub DisplayAttributesSetTest()
        Assert.AreEqual(atcDataManager.DisplayAttributes.Count, 6) 'history 1, id, ..., min, max, mean
        Dim aNewValues As IEnumerable = {"Test 1", "Test 2"}
        atcDataManager.DisplayAttributesSet(aNewValues)
        Assert.AreEqual(atcDataManager.DisplayAttributes.Count, 2)
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
        atcDataManager.Clear()
        Dim aNewSource As New atcTimeseriesSource ' TODO: Initialize to an appropriate value
        Dim aSpecification As String = IO.Path.Combine(Environment.SystemDirectory(), "cmd.exe")
        Dim aAttributes As atcDataAttributes = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = True ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = atcDataManager.OpenDataSource(aNewSource, aSpecification, aAttributes)
        Assert.AreEqual(expected, actual)
    End Sub

    '''<summary>Test OpenDataSource</summary>
    <TestMethod()> Public Sub OpenDataSourceTest1()
        atcDataManager.Clear()
        Dim aSpecification As String = IO.Path.Combine(Environment.SystemDirectory(), "cmd.exe")
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = atcDataManager.OpenDataSource(aSpecification)
        Assert.AreEqual(expected, actual)
    End Sub

    '''<summary>Test RemoveDataSource</summary>
    <TestMethod()> Public Sub RemoveDataSourceTest()
        atcDataManager.Clear()
        Dim aSpecification As String = IO.Path.Combine(Environment.SystemDirectory(), "cmd.exe")
        Dim lDataSource As New atcTimeseriesSource
        lDataSource.Specification = aSpecification
        atcDataManager.DataSources.Add(lDataSource)
        Assert.AreEqual(atcDataManager.DataSources.Count, 1)
        atcDataManager.RemoveDataSource(aSpecification)
        Assert.AreEqual(atcDataManager.DataSources.Count, 0)
    End Sub

    '''<summary>Test RemoveDataSource</summary>
    <TestMethod()> Public Sub RemoveDataSourceTest1()
        atcDataManager.Clear()
        Dim aSpecification As String = IO.Path.Combine(Environment.SystemDirectory(), "cmd.exe")
        Dim lDataSource As New atcTimeseriesSource
        lDataSource.Specification = aSpecification
        atcDataManager.DataSources.Add(lDataSource)
        Assert.AreEqual(atcDataManager.DataSources.Count, 1)
        atcDataManager.RemoveDataSource(0)
        Assert.AreEqual(atcDataManager.DataSources.Count, 0)
    End Sub

    '''<summary>Test RemoveDataSource</summary>
    <TestMethod()> Public Sub RemoveDataSourceTest2()
        atcDataManager.Clear()
        Dim aSpecification As String = IO.Path.Combine(Environment.SystemDirectory(), "cmd.exe")
        Dim lDataSource As New atcTimeseriesSource
        lDataSource.Specification = aSpecification
        atcDataManager.DataSources.Add(lDataSource)
        Assert.AreEqual(atcDataManager.DataSources.Count, 1)
        atcDataManager.RemoveDataSource(lDataSource)
        Assert.AreEqual(atcDataManager.DataSources.Count, 0)
    End Sub

    '''<summary>Test RemoveDuplicates</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub RemoveDuplicatesTest()
        Dim aStringList As New List(Of String)
        aStringList.Add("Test1")
        aStringList.Add("Test2")
        aStringList.Add("Test3")
        aStringList.Add("Test4")
        aStringList.Add("Test4")
        aStringList.Add("Test2")
        aStringList.Add("Test5")

        atcDataManager_Accessor.RemoveDuplicates(aStringList)
        Dim lStringList As New List(Of String)
        lStringList.Add("Test1")
        lStringList.Add("Test2")
        lStringList.Add("Test3")
        lStringList.Add("Test4")
        lStringList.Add("Test5")

        Dim lSuccess As Boolean = True
        If aStringList.Count <> lStringList.Count Then
            lSuccess = False
        Else
            For i As Integer = 0 To aStringList.Count - 1
                If aStringList(i).CompareTo(lStringList(i)) <> 0 Then
                    lSuccess = False
                    Exit For
                End If
            Next
        End If
        If lSuccess Then
            Assert.AreEqual(True, True)
        Else
            Assert.AreEqual(True, False)
        End If
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
        atcDataManager.DisplayAttributesSet(Nothing)
        Dim lAttCount As Integer = atcDataManager.DisplayAttributes.Count
        Dim aNewValues As IEnumerable = {"Test 1", "Test 2"}
        atcDataManager.SelectionAttributesSet(aNewValues)
        Assert.AreEqual(2, atcDataManager.SelectionAttributes.Count)
    End Sub

    '''<summary>Test ShowDisplay</summary>
    <TestMethod()> Public Sub ShowDisplayTest()
        atcDataManager.Clear()
        Dim aDisplayName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aTimeseriesGroup As New atcTimeseriesGroup
        Try
            atcDataManager.ShowDisplay(aDisplayName, aTimeseriesGroup)
            Assert.AreEqual(True, True)
        Catch ex As Exception
            Assert.AreEqual(True, False)
        End Try
    End Sub

    '''<summary>Test ToString</summary>
    <TestMethod()> Public Sub ToStringTest()
        atcDataManager.Clear()
        Dim target As atcDataManager_Accessor = New atcDataManager_Accessor() ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        Try
            actual = target.ToString
            Assert.AreEqual(True, True)
        Catch ex As Exception
            Assert.AreEqual(False, True)
        End Try
    End Sub

    '''<summary>Test UserManage</summary>
    <TestMethod()> Public Sub UserManageTest()
        atcDataManager.Clear()
        Dim aTitle As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aDefaultIndex As Integer = 0 ' TODO: Initialize to an appropriate value
        Try
            atcDataManager.UserManage(aTitle, aDefaultIndex)
            Assert.AreEqual(True, True)
        Catch ex As Exception
            Assert.AreEqual(True, False)
        End Try
    End Sub

    '''<summary>Test UserOpenDataFile</summary>
    ''' Needs user interaction
    <TestMethod()> Public Sub UserOpenDataFileTest()
        atcDataManager.Clear()
        Dim aNeedToOpen As Boolean = False ' TODO: Initialize to an appropriate value
        Dim aNeedToSave As Boolean = False ' TODO: Initialize to an appropriate value
        Dim expected As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcTimeseriesSource
        Try
            actual = atcDataManager.UserOpenDataFile(aNeedToOpen, aNeedToSave)
            Assert.AreEqual(expected, actual)
        Catch ex As Exception
            Assert.AreEqual(True, False)
        End Try
    End Sub

    '''<summary>Test UserSaveData</summary>
    ''' Needs user interaction
    <TestMethod()> Public Sub UserSaveDataTest()
        atcDataManager.Clear()
        Dim aSpecification As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        Try
            actual = atcDataManager.UserSaveData(aSpecification)
            Assert.AreEqual(expected, actual)
        Catch ex As Exception
            Assert.AreEqual(True, False)
        End Try
    End Sub

    '''<summary>Test UserSelectData</summary>
    ''' Needs user interaction
    <TestMethod()> Public Sub UserSelectDataTest()
        atcDataManager.Clear()
        Dim aTitle As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aSelected As New atcTimeseriesGroup
        Dim aAvailable As New atcTimeseriesGroup
        Dim aModal As Boolean = True ' TODO: Initialize to an appropriate value
        Dim aCancelReturnsOriginalSelected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim aIcon As Icon = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As atcTimeseriesGroup = aSelected ' TODO: Initialize to an appropriate value
        Dim actual As atcTimeseriesGroup
        actual = atcDataManager.UserSelectData(aTitle, aSelected, aAvailable, aModal, aCancelReturnsOriginalSelected, aIcon)
        Assert.AreEqual(expected, actual)

    End Sub

    '''<summary>Test UserSelectData</summary>
    ''' Needs user interaction
    <TestMethod()> Public Sub UserSelectDataTest1()
        atcDataManager.Clear()
        Dim aTitle As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aSelected As New atcTimeseriesGroup
        Dim aAvailable As New atcTimeseriesGroup
        Dim aModal As Boolean = True ' TODO: Initialize to an appropriate value
        Dim aCancelReturnsOriginalSelected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim expected As atcTimeseriesGroup = aSelected ' TODO: Initialize to an appropriate value
        Dim actual As atcTimeseriesGroup
        actual = atcDataManager.UserSelectData(aTitle, aSelected, aAvailable, aModal, aCancelReturnsOriginalSelected)
        Assert.AreEqual(expected, actual)
    End Sub

    '''<summary>Test UserSelectDataSource</summary>
    ''' Needs user interaction
    <TestMethod()> Public Sub UserSelectDataSourceTest()
        atcDataManager.Clear()
        Dim aCategories As ArrayList = Nothing ' TODO: Initialize to an appropriate value
        Dim aTitle As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aNeedToOpen As Boolean = False ' TODO: Initialize to an appropriate value
        Dim aNeedToSave As Boolean = False ' TODO: Initialize to an appropriate value
        Dim expected As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcTimeseriesSource
        actual = atcDataManager.UserSelectDataSource(aCategories, aTitle, aNeedToOpen, aNeedToSave)
        Assert.AreEqual(expected, actual)
    End Sub

    '''<summary>Test UserSelectDisplay</summary>
    <TestMethod()> Public Sub UserSelectDisplayTest()
        Dim aTitle As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aDataGroup As New atcTimeseriesGroup
        Try
            atcDataManager.UserSelectDisplay(aTitle, aDataGroup)
            Assert.AreEqual(True, True)
        Catch ex As Exception
            Assert.AreEqual(True, False)
        End Try
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
        atcDataManager.Clear()
        Assert.IsInstanceOfType(atcDataManager.DataSources, GetType(ArrayList))
    End Sub

    '''<summary>Test DisplayAttributes</summary>
    <TestMethod()> Public Sub DisplayAttributesTest()
        atcDataManager.Clear()
        Assert.AreEqual(atcDataManager.DisplayAttributes.Count, 6)
    End Sub

    '''<summary>Test SelectionAttributes</summary>
    <TestMethod()> Public Sub SelectionAttributesTest()
        atcDataManager.Clear()
        Assert.AreEqual(atcDataManager.SelectionAttributes.Count, 3)
    End Sub

    '''<summary>Test XML</summary>
    <TestMethod()> Public Sub XMLTest()
        atcDataManager.Clear()
        'Test for Get
        Dim lXMLText As String = atcDataManager.XML
        If lXMLText.StartsWith("") AndAlso lXMLText.EndsWith("") Then
            Assert.AreEqual(True, True)
        Else
            Assert.AreEqual(True, False)
        End If
        'Test for Set
        'Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        'Dim actual As String
        'atcDataManager.XML = expected
        'actual = atcDataManager.XML
        'Assert.AreEqual(expected, actual)
        'Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub
End Class
