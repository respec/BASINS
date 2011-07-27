Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports atcData

'''<summary>
'''This is a test class for atcDataSourceTest and is intended
'''to contain all atcDataSourceTest Unit Tests (Done)
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

    '''<summary>Test atcDataSource Constructor</summary>
    <TestMethod()> Public Sub atcDataSourceConstructorTest()
        Dim target As atcDataSource = New atcDataSource()
        Assert.IsInstanceOfType(target.DataSets, GetType(atcDataGroup))
        Assert.AreEqual(0, target.DataSets.Count)
    End Sub

    '''<summary>Test AddDataSet</summary>
    <TestMethod()> Public Sub AddDataSetTest()
        Dim target As atcDataSource = New atcDataSource()
        Dim aDs As New atcDataSet
        Dim aExistAction As atcDataSource.EnumExistAction = New atcDataSource.EnumExistAction()
        Dim expected As Boolean = True
        Dim actual As Boolean = target.AddDataSet(aDs, aExistAction)
        Assert.AreEqual(expected, actual)
        Assert.AreEqual(1, target.DataSets.Count)
        Assert.AreEqual(aDs.Serial, target.DataSets.Item(0).Serial)
    End Sub

    '''<summary>Test AddDataSets</summary>
    <TestMethod()> Public Sub AddDataSetsTest()
        Dim target As atcDataSource = New atcDataSource()
        Dim aDataGroup As New atcDataGroup
        Dim lDS As New atcDataSet
        Dim lAddThese As IEnumerable = {lDS, lDS.Clone}
        aDataGroup.AddRange(lAddThese)
        Dim lMsg As String = ""
        Dim actual As Boolean
        Try
            actual = target.AddDataSets(aDataGroup)
        Catch ex As Exception
            lMsg = ex.Message
        End Try
        If lMsg.Contains("does not implement") Then
            Assert.AreEqual(True, True)
        Else
            Assert.AreEqual(True, False)
        End If
    End Sub

    '''<summary>Test Clear</summary>
    <TestMethod()> Public Sub ClearTest()
        Dim target As atcDataSource = New atcDataSource()
        target.Clear()
        Assert.AreEqual(0, target.Attributes.Count)
        Assert.AreEqual(0, target.DataSets.Count)
        Assert.AreEqual("", target.Specification)

    End Sub

    '''<summary>Test Finalize</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub FinalizeTest()
        Dim target As atcDataSource_Accessor = New atcDataSource_Accessor() ' TODO: Initialize to an appropriate value
        target.Finalize()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test Open</summary>
    <TestMethod()> Public Sub OpenTest()
        Dim target As atcDataSource = New atcDataSource() ' TODO: Initialize to an appropriate value
        Dim aSpecification As String = IO.Path.Combine(Environment.SystemDirectory, "cmd.exe")
        Dim aAttributes As atcDataAttributes = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = True ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = target.Open(aSpecification, aAttributes)
        Assert.AreEqual(expected, actual)

    End Sub

    '''<summary>Test ReadData</summary>
    <TestMethod()> Public Sub ReadDataTest()
        Dim target As atcDataSource = New atcDataSource() ' TODO: Initialize to an appropriate value
        Dim aData As atcDataSet = Nothing ' TODO: Initialize to an appropriate value
        Dim lMsg As String = ""
        Try
            target.ReadData(aData)
        Catch ex As Exception
            lMsg = ex.Message
        End Try
        If lMsg.Contains("does not implement") Then
            Assert.AreEqual(True, True)
        Else
            Assert.AreEqual(True, False)
        End If
    End Sub

    '''<summary>Test RemoveDataset</summary>
    <TestMethod()> Public Sub RemoveDatasetTest()
        Dim target As atcDataSource = New atcDataSource() ' TODO: Initialize to an appropriate value
        Dim aData As atcDataSet = Nothing ' TODO: Initialize to an appropriate value
        Dim lMsg As String = ""
        Try
            target.RemoveDataset(aData)
        Catch ex As Exception
            lMsg = ex.Message
        End Try
        If lMsg.Contains("does not implement") Then
            Assert.AreEqual(True, True)
        Else
            Assert.AreEqual(True, False)
        End If
    End Sub

    '''<summary>Test Save</summary>
    <TestMethod()> Public Sub SaveTest()
        Dim target As atcDataSource = New atcDataSource() ' TODO: Initialize to an appropriate value
        Dim aSaveFileName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aExistAction As atcDataSource.EnumExistAction = New atcDataSource.EnumExistAction() ' TODO: Initialize to an appropriate value
        Dim lMsg As String = ""
        Try
            target.Save(aSaveFileName, aExistAction)
        Catch ex As Exception
            lMsg = ex.Message
        End Try
        If lMsg.Contains("does not implement") Then
            Assert.AreEqual(True, True)
        Else
            Assert.AreEqual(True, False)
        End If
    End Sub

    '''<summary>Test ToString</summary>
    <TestMethod()> Public Sub ToStringTest()
        Dim target As atcDataSource = New atcDataSource()
        Dim expected As String = target.Name
        If expected.Length = 0 Then
            expected = "atcDataSource:TypeNameUnknown"
        End If
        expected &= " '" & target.Specification & "' " & target.DataSets.Count & " datasets"
        Dim actual As String
        actual = target.ToString
        Assert.AreEqual(expected, actual)
    End Sub

    '''<summary>Test View</summary>
    <TestMethod()> Public Sub ViewTest()
        Dim target As atcDataSource = New atcDataSource()
        target.Specification = IO.Path.Combine(Environment.CurrentDirectory(), "ztest.txt")

        Dim lSuccess As Boolean = True
        Try
            target.View()
        Catch ex As Exception
            lSuccess = False
        End Try
        Assert.AreEqual(lSuccess, True)
    End Sub

    '''<summary>Test Attributes</summary>
    <TestMethod()> Public Sub AttributesTest()
        Dim target As atcDataSource = New atcDataSource() ' TODO: Initialize to an appropriate value
        Dim actual As atcDataAttributes
        actual = target.Attributes
        If actual Is Nothing Then
            Assert.AreEqual(True, False)
        Else
            Assert.AreEqual(True, True)
        End If
    End Sub

    '''<summary>Test AvailableOperations</summary>
    <TestMethod()> Public Sub AvailableOperationsTest()
        Dim target As atcDataSource = New atcDataSource() ' TODO: Initialize to an appropriate value
        Dim actual As atcDataAttributes
        actual = target.AvailableOperations
        Assert.IsInstanceOfType(actual, GetType(atcDataAttributes))
    End Sub

    '''<summary>Test CanOpen</summary>
    <TestMethod()> Public Sub CanOpenTest()
        Dim target As atcDataSource = New atcDataSource() ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = target.CanOpen
        Dim expected As Boolean = False
        Assert.AreEqual(expected, actual)
    End Sub

    '''<summary>Test CanRemoveDataset</summary>
    <TestMethod()> Public Sub CanRemoveDatasetTest()
        Dim target As atcDataSource = New atcDataSource() ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = target.CanRemoveDataset
        Dim expected As Boolean = False
        Assert.AreEqual(expected, actual)
    End Sub

    '''<summary>Test CanSave</summary>
    <TestMethod()> Public Sub CanSaveTest()
        Dim target As atcDataSource = New atcDataSource() ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = target.CanSave
        Dim expected As Boolean = False
        Assert.AreEqual(expected, actual)
    End Sub

    '''<summary>Test DataSets</summary>
    <TestMethod()> Public Sub DataSetsTest()
        Dim target As atcDataSource = New atcDataSource() ' TODO: Initialize to an appropriate value
        Assert.IsInstanceOfType(target.DataSets, GetType(atcDataGroup))
    End Sub

    '''<summary>Test Filter</summary>
    <TestMethod()> Public Sub FilterTest()
        Dim target As atcDataSource = New atcDataSource() ' TODO: Initialize to an appropriate value
        target.Filter = String.Empty
        Assert.AreEqual("", target.Filter)
        target.Filter = "This is a test"
        Assert.AreEqual(target.Filter, "This is a test")
    End Sub

    '''<summary>Test Specification</summary>
    <TestMethod()> Public Sub SpecificationTest()
        Dim target As atcDataSource = New atcDataSource() ' TODO: Initialize to an appropriate value
        target.Specification = String.Empty
        Assert.AreEqual("", target.Specification)
        target.Specification = "This is a test"
        Assert.AreEqual(target.Specification, "This is a test")
    End Sub
End Class
