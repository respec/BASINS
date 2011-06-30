Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports atcUtility



'''<summary>
'''This is a test class for atcCollection_clsDictionaryEnumeratorTest and is intended
'''to contain all atcCollection_clsDictionaryEnumeratorTest Unit Tests
'''</summary>
<TestClass()> _
Public Class atcCollection_clsDictionaryEnumeratorTest


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
    '''A test for clsDictionaryEnumerator Constructor
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcUtility.dll")> _
    Public Sub atcCollection_clsDictionaryEnumeratorConstructorTest()
        Dim aCollection As atcCollection = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcCollection_Accessor.clsDictionaryEnumerator = New atcCollection_Accessor.clsDictionaryEnumerator(aCollection)
        Assert.Inconclusive("TODO: Implement code to verify target")
    End Sub

    '''<summary>
    '''A test for Dispose
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcUtility.dll")> _
    Public Sub DisposeTest()
        'Class inheritance across assemblies is not preserved by private accessors. However, a static method AttachShadow() is provided in each private accessor class to transfer a private object from one private accessor class to another.
        Assert.Inconclusive("Class inheritance across assemblies is not preserved by private accessors. Howeve" & _
                "r, a static method AttachShadow() is provided in each private accessor class to " & _
                "transfer a private object from one private accessor class to another.")
    End Sub

    '''<summary>
    '''A test for GetEnumerator
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcUtility.dll")> _
    Public Sub GetEnumeratorTest()
        'Class inheritance across assemblies is not preserved by private accessors. However, a static method AttachShadow() is provided in each private accessor class to transfer a private object from one private accessor class to another.
        Assert.Inconclusive("Class inheritance across assemblies is not preserved by private accessors. Howeve" & _
                "r, a static method AttachShadow() is provided in each private accessor class to " & _
                "transfer a private object from one private accessor class to another.")
    End Sub

    '''<summary>
    '''A test for MoveNext
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcUtility.dll")> _
    Public Sub MoveNextTest()
        'Class inheritance across assemblies is not preserved by private accessors. However, a static method AttachShadow() is provided in each private accessor class to transfer a private object from one private accessor class to another.
        Assert.Inconclusive("Class inheritance across assemblies is not preserved by private accessors. Howeve" & _
                "r, a static method AttachShadow() is provided in each private accessor class to " & _
                "transfer a private object from one private accessor class to another.")
    End Sub

    '''<summary>
    '''A test for Reset
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcUtility.dll")> _
    Public Sub ResetTest()
        'Class inheritance across assemblies is not preserved by private accessors. However, a static method AttachShadow() is provided in each private accessor class to transfer a private object from one private accessor class to another.
        Assert.Inconclusive("Class inheritance across assemblies is not preserved by private accessors. Howeve" & _
                "r, a static method AttachShadow() is provided in each private accessor class to " & _
                "transfer a private object from one private accessor class to another.")
    End Sub

    '''<summary>
    '''A test for Current
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcUtility.dll")> _
    Public Sub CurrentTest()
        'Class inheritance across assemblies is not preserved by private accessors. However, a static method AttachShadow() is provided in each private accessor class to transfer a private object from one private accessor class to another.
        Assert.Inconclusive("Class inheritance across assemblies is not preserved by private accessors. Howeve" & _
                "r, a static method AttachShadow() is provided in each private accessor class to " & _
                "transfer a private object from one private accessor class to another.")
    End Sub
End Class
