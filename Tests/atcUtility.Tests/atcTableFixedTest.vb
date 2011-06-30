Imports System.IO

Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports atcUtility



'''<summary>
'''This is a test class for atcTableFixedTest and is intended
'''to contain all atcTableFixedTest Unit Tests
'''</summary>
<TestClass()> _
Public Class atcTableFixedTest


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
    '''A test for atcTableFixed Constructor
    '''</summary>
    <TestMethod()> _
    Public Sub atcTableFixedConstructorTest()
        Dim target As atcTableFixed = New atcTableFixed()
        Assert.Inconclusive("TODO: Implement code to verify target")
    End Sub

    '''<summary>
    '''A test for Clear
    '''</summary>
    <TestMethod()> _
    Public Sub ClearTest()
        Dim target As atcTableFixed = New atcTableFixed() ' TODO: Initialize to an appropriate value
        target.Clear()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for ClearData
    '''</summary>
    <TestMethod()> _
    Public Sub ClearDataTest()
        Dim target As atcTableFixed = New atcTableFixed() ' TODO: Initialize to an appropriate value
        target.ClearData()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for Cousin
    '''</summary>
    <TestMethod()> _
    Public Sub CousinTest()
        Dim target As atcTableFixed = New atcTableFixed() ' TODO: Initialize to an appropriate value
        Dim expected As IatcTable = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As IatcTable
        actual = target.Cousin
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for CreationCode
    '''</summary>
    <TestMethod()> _
    Public Sub CreationCodeTest()
        Dim target As atcTableFixed = New atcTableFixed() ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = target.CreationCode
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for FieldNumber
    '''</summary>
    <TestMethod()> _
    Public Sub FieldNumberTest()
        Dim target As atcTableFixed = New atcTableFixed() ' TODO: Initialize to an appropriate value
        Dim aFieldName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        actual = target.FieldNumber(aFieldName)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for OpenFile
    '''</summary>
    <TestMethod()> _
    Public Sub OpenFileTest()
        Dim target As atcTableFixed = New atcTableFixed() ' TODO: Initialize to an appropriate value
        Dim Filename As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = target.OpenFile(Filename)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for OpenStream
    '''</summary>
    <TestMethod()> _
    Public Sub OpenStreamTest()
        Dim target As atcTableFixed = New atcTableFixed() ' TODO: Initialize to an appropriate value
        Dim aStream As Stream = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = target.OpenStream(aStream)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for OpenString
    '''</summary>
    <TestMethod()> _
    Public Sub OpenStringTest()
        Dim target As atcTableFixed = New atcTableFixed() ' TODO: Initialize to an appropriate value
        Dim aString As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = target.OpenString(aString)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for WriteFile
    '''</summary>
    <TestMethod()> _
    Public Sub WriteFileTest()
        Dim target As atcTableFixed = New atcTableFixed() ' TODO: Initialize to an appropriate value
        Dim aFilename As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = target.WriteFile(aFilename)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for CurrentRecord
    '''</summary>
    <TestMethod()> _
    Public Sub CurrentRecordTest()
        Dim target As atcTableFixed = New atcTableFixed() ' TODO: Initialize to an appropriate value
        Dim expected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        target.CurrentRecord = expected
        actual = target.CurrentRecord
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for FieldLength
    '''</summary>
    <TestMethod()> _
    Public Sub FieldLengthTest()
        Dim target As atcTableFixed = New atcTableFixed() ' TODO: Initialize to an appropriate value
        Dim aFieldNumber As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim expected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        target.FieldLength(aFieldNumber) = expected
        actual = target.FieldLength(aFieldNumber)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for FieldName
    '''</summary>
    <TestMethod()> _
    Public Sub FieldNameTest()
        Dim target As atcTableFixed = New atcTableFixed() ' TODO: Initialize to an appropriate value
        Dim aFieldNumber As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        target.FieldName(aFieldNumber) = expected
        actual = target.FieldName(aFieldNumber)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for FieldStart
    '''</summary>
    <TestMethod()> _
    Public Sub FieldStartTest()
        Dim target As atcTableFixed = New atcTableFixed() ' TODO: Initialize to an appropriate value
        Dim aFieldNumber As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim expected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        target.FieldStart(aFieldNumber) = expected
        actual = target.FieldStart(aFieldNumber)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for FieldType
    '''</summary>
    <TestMethod()> _
    Public Sub FieldTypeTest()
        Dim target As atcTableFixed = New atcTableFixed() ' TODO: Initialize to an appropriate value
        Dim aFieldNumber As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        target.FieldType(aFieldNumber) = expected
        actual = target.FieldType(aFieldNumber)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for NumFields
    '''</summary>
    <TestMethod()> _
    Public Sub NumFieldsTest()
        Dim target As atcTableFixed = New atcTableFixed() ' TODO: Initialize to an appropriate value
        Dim expected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        target.NumFields = expected
        actual = target.NumFields
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for NumRecords
    '''</summary>
    <TestMethod()> _
    Public Sub NumRecordsTest()
        Dim target As atcTableFixed = New atcTableFixed() ' TODO: Initialize to an appropriate value
        Dim expected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        target.NumRecords = expected
        actual = target.NumRecords
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for Value
    '''</summary>
    <TestMethod()> _
    Public Sub ValueTest()
        Dim target As atcTableFixed = New atcTableFixed() ' TODO: Initialize to an appropriate value
        Dim aFieldNumber As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        target.Value(aFieldNumber) = expected
        actual = target.Value(aFieldNumber)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub
End Class
