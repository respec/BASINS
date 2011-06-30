Imports System

Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports atcWDM



'''<summary>
'''This is a test class for atcWdmHandleTest and is intended
'''to contain all atcWdmHandleTest Unit Tests
'''</summary>
<TestClass()> _
Public Class atcWdmHandleTest


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
    '''A test for atcWdmHandle Constructor
    '''</summary>
    <TestMethod()> _
    Public Sub atcWdmHandleConstructorTest()
        Dim aRWCFlg As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aFileName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim target As atcWdmHandle = New atcWdmHandle(aRWCFlg, aFileName)
        Assert.Inconclusive("TODO: Implement code to verify target")
    End Sub

    '''<summary>
    '''A test for Dispose
    '''</summary>
    <TestMethod()> _
    Public Sub DisposeTest()
        Dim aRWCFlg As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aFileName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim target As IDisposable = New atcWdmHandle(aRWCFlg, aFileName) ' TODO: Initialize to an appropriate value
        target.Dispose()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for Finalize
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcWDM.dll")> _
    Public Sub FinalizeTest()
        Dim param0 As PrivateObject = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcWdmHandle_Accessor = New atcWdmHandle_Accessor(param0) ' TODO: Initialize to an appropriate value
        target.Finalize()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for Unit
    '''</summary>
    <TestMethod()> _
    Public Sub UnitTest()
        Dim aRWCFlg As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aFileName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim target As atcWdmHandle = New atcWdmHandle(aRWCFlg, aFileName) ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        actual = target.Unit
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub
End Class
