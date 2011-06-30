Imports System.Drawing
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports atcUtility

'''<summary>
'''This is a test class for UtilColorTest and is intended
'''to contain all UtilColorTest Unit Tests
'''</summary>
<TestClass()> _
Public Class UtilColorTest
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

    '''<summary>Test GetMatchingColor</summary>
    <TestMethod()> Public Sub GetMatchingColorTest()
        Dim aSpecification As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As Color = New Color() ' TODO: Initialize to an appropriate value
        Dim actual As Color
        actual = UtilColor.GetMatchingColor(aSpecification)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test InitMatchingColors</summary>
    <TestMethod()> Public Sub InitMatchingColorsTest()
        Dim aFilename As String = String.Empty ' TODO: Initialize to an appropriate value
        UtilColor.InitMatchingColors(aFilename)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test TextOrNumericColor</summary>
    <TestMethod()> Public Sub TextOrNumericColorTest()
        Dim aColorName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As Color = New Color() ' TODO: Initialize to an appropriate value
        Dim actual As Color
        actual = UtilColor.TextOrNumericColor(aColorName)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test colorDB</summary>
    <TestMethod(), DeploymentItem("atcUtility.dll")> _
    Public Sub colorDBTest()
        Dim expected As atcTable = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcTable
        actual = UtilColor_Accessor.colorDB
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test colorName</summary>
    <TestMethod()> Public Sub colorNameTest()
        Dim aColor As Color = New Color() ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = UtilColor.colorName(aColor)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub
End Class
