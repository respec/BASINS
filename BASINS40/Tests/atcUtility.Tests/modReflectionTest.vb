Imports System.Collections

Imports System.Drawing

Imports System.IO

Imports System.Reflection

Imports System

Imports System.Windows.Forms

Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports atcUtility



'''<summary>
'''This is a test class for modReflectionTest and is intended
'''to contain all modReflectionTest Unit Tests
'''</summary>
<TestClass()> _
Public Class modReflectionTest


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
    '''A test for BuildMissingTests
    '''</summary>
    <TestMethod()> _
    Public Sub BuildMissingTestsTest()
        Dim aSavePath As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = modReflection.BuildMissingTests(aSavePath)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for CopyControlState
    '''</summary>
    <TestMethod()> _
    Public Sub CopyControlStateTest()
        Dim aOriginalControl As Control = Nothing ' TODO: Initialize to an appropriate value
        Dim aCopyTo As Control = Nothing ' TODO: Initialize to an appropriate value
        modReflection.CopyControlState(aOriginalControl, aCopyTo)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for FieldNames
    '''</summary>
    <TestMethod()> _
    Public Sub FieldNamesTest()
        Dim aType As Type = Nothing ' TODO: Initialize to an appropriate value
        Dim aDelimiter As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = modReflection.FieldNames(aType, aDelimiter)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for FieldValues
    '''</summary>
    <TestMethod()> _
    Public Sub FieldValuesTest()
        Dim aObject As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim aDelimiter As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = modReflection.FieldValues(aObject, aDelimiter)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for GetEmbeddedFileAsBinaryReader
    '''</summary>
    <TestMethod()> _
    Public Sub GetEmbeddedFileAsBinaryReaderTest()
        Dim aFileName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aAssembly As [Assembly] = Nothing ' TODO: Initialize to an appropriate value
        Dim aPrefixName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As BinaryReader = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As BinaryReader
        actual = modReflection.GetEmbeddedFileAsBinaryReader(aFileName, aAssembly, aPrefixName)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for GetEmbeddedFileAsBitmap
    '''</summary>
    <TestMethod()> _
    Public Sub GetEmbeddedFileAsBitmapTest()
        Dim fileName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aAssembly As [Assembly] = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As Bitmap = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Bitmap
        actual = modReflection.GetEmbeddedFileAsBitmap(fileName, aAssembly)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for GetEmbeddedFileAsString
    '''</summary>
    <TestMethod()> _
    Public Sub GetEmbeddedFileAsStringTest()
        Dim fileName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aFullResourceName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = modReflection.GetEmbeddedFileAsString(fileName, aFullResourceName)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for GetEmbeddedFileAsString
    '''</summary>
    <TestMethod()> _
    Public Sub GetEmbeddedFileAsStringTest1()
        Dim fileName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aAssembly As [Assembly] = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = modReflection.GetEmbeddedFileAsString(fileName, aAssembly)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for GetSomething
    '''</summary>
    <TestMethod()> _
    Public Sub GetSomethingTest()
        Dim aObject As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim aObjectExpected As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim aFieldName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Object
        actual = modReflection.GetSomething(aObject, aFieldName)
        Assert.AreEqual(aObjectExpected, aObject)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for MemUsage
    '''</summary>
    <TestMethod()> _
    Public Sub MemUsageTest()
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = modReflection.MemUsage
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for MethodAvailable
    '''</summary>
    <TestMethod()> _
    Public Sub MethodAvailableTest()
        Dim aT As Type = Nothing ' TODO: Initialize to an appropriate value
        Dim aTExpected As Type = Nothing ' TODO: Initialize to an appropriate value
        Dim aMethodName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = modReflection.MethodAvailable(aT, aMethodName)
        Assert.AreEqual(aTExpected, aT)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for MethodDetails
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcUtility.dll")> _
    Public Sub MethodDetailsTest()
        Dim aT As Type = Nothing ' TODO: Initialize to an appropriate value
        Dim aTExpected As Type = Nothing ' TODO: Initialize to an appropriate value
        Dim aMethodType As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aFlag As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = modReflection_Accessor.MethodDetails(aT, aMethodType, aFlag)
        Assert.AreEqual(aTExpected, aT)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for NeedToTest
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcUtility.dll")> _
    Public Sub NeedToTestTest()
        Dim aMethodName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = modReflection_Accessor.NeedToTest(aMethodName)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for NumberObjects
    '''</summary>
    <TestMethod()> _
    Public Sub NumberObjectsTest()
        Dim aObjects As ArrayList = Nothing ' TODO: Initialize to an appropriate value
        Dim aFieldToNumber As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aPrefix As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aStartIndex As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim expected As ArrayList = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ArrayList
        actual = modReflection.NumberObjects(aObjects, aFieldToNumber, aPrefix, aStartIndex)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for ReflectAssemblyAsString
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcUtility.dll")> _
    Public Sub ReflectAssemblyAsStringTest()
        Dim mA As [Assembly] = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = modReflection_Accessor.ReflectAssemblyAsString(mA)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for SetSomething
    '''</summary>
    <TestMethod()> _
    Public Sub SetSomethingTest()
        Dim aObject As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim aObjectExpected As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim aFieldName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aValue As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = modReflection.SetSomething(aObject, aFieldName, aValue)
        Assert.AreEqual(aObjectExpected, aObject)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for SetSomething
    '''</summary>
    <TestMethod()> _
    Public Sub SetSomethingTest1()
        Dim aObject As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim aObjectExpected As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim aFieldName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aValue As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim aLogProblems As Boolean = False ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = modReflection.SetSomething(aObject, aFieldName, aValue, aLogProblems)
        Assert.AreEqual(aObjectExpected, aObject)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub
End Class
