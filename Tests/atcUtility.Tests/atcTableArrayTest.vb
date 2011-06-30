Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports atcUtility

'''<summary>
'''This is a test class for atcTableArrayTest and is intended
'''to contain all atcTableArrayTest Unit Tests
'''</summary>
<TestClass()> _
Public Class atcTableArrayTest
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

    '''<summary>Test atcTableArray Constructor</summary>
    <TestMethod()> Public Sub atcTableArrayConstructorTest()
        Dim target As atcTableArray = New atcTableArray()
        Assert.Inconclusive("TODO: Implement code to verify target")
    End Sub

    '''<summary>Test Clear</summary>
    <TestMethod()> Public Sub ClearTest()
        Dim target As atcTableArray = New atcTableArray() ' TODO: Initialize to an appropriate value
        target.Clear()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test ClearData</summary>
    <TestMethod()> Public Sub ClearDataTest()
        Dim target As atcTableArray = New atcTableArray() ' TODO: Initialize to an appropriate value
        target.ClearData()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test Cousin</summary>
    <TestMethod()> Public Sub CousinTest()
        Dim target As atcTableArray = New atcTableArray() ' TODO: Initialize to an appropriate value
        Dim expected As IatcTable = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As IatcTable
        actual = target.Cousin
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test CreationCode</summary>
    <TestMethod()> Public Sub CreationCodeTest()
        Dim target As atcTableArray = New atcTableArray() ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = target.CreationCode
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test FieldNumber</summary>
    <TestMethod()> Public Sub FieldNumberTest()
        Dim target As atcTableArray = New atcTableArray() ' TODO: Initialize to an appropriate value
        Dim aFieldName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        actual = target.FieldNumber(aFieldName)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test OpenFile</summary>
    <TestMethod()> Public Sub OpenFileTest()
        Dim target As atcTableArray = New atcTableArray() ' TODO: Initialize to an appropriate value
        Dim Filename As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = target.OpenFile(Filename)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test WriteFile</summary>
    <TestMethod()> Public Sub WriteFileTest()
        Dim target As atcTableArray = New atcTableArray() ' TODO: Initialize to an appropriate value
        Dim aFilename As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = target.WriteFile(aFilename)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test CurrentRecord</summary>
    <TestMethod()> Public Sub CurrentRecordTest()
        Dim target As atcTableArray = New atcTableArray() ' TODO: Initialize to an appropriate value
        Dim expected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        target.CurrentRecord = expected
        actual = target.CurrentRecord
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test Delimiter</summary>
    <TestMethod()> Public Sub DelimiterTest()
        Dim target As atcTableArray = New atcTableArray() ' TODO: Initialize to an appropriate value
        Dim expected As Char = Global.Microsoft.VisualBasic.ChrW(0) ' TODO: Initialize to an appropriate value
        Dim actual As Char
        target.Delimiter = expected
        actual = target.Delimiter
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test FieldLength</summary>
    <TestMethod()> Public Sub FieldLengthTest()
        Dim target As atcTableArray = New atcTableArray() ' TODO: Initialize to an appropriate value
        Dim aFieldNumber As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim expected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        target.FieldLength(aFieldNumber) = expected
        actual = target.FieldLength(aFieldNumber)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test FieldName</summary>
    <TestMethod()> Public Sub FieldNameTest()
        Dim target As atcTableArray = New atcTableArray() ' TODO: Initialize to an appropriate value
        Dim aFieldNumber As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        target.FieldName(aFieldNumber) = expected
        actual = target.FieldName(aFieldNumber)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test FieldType</summary>
    <TestMethod()> Public Sub FieldTypeTest()
        Dim target As atcTableArray = New atcTableArray() ' TODO: Initialize to an appropriate value
        Dim aFieldNumber As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        target.FieldType(aFieldNumber) = expected
        actual = target.FieldType(aFieldNumber)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test NumFields</summary>
    <TestMethod()> Public Sub NumFieldsTest()
        Dim target As atcTableArray = New atcTableArray() ' TODO: Initialize to an appropriate value
        Dim expected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        target.NumFields = expected
        actual = target.NumFields
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test NumRecords</summary>
    <TestMethod()> Public Sub NumRecordsTest()
        Dim target As atcTableArray = New atcTableArray() ' TODO: Initialize to an appropriate value
        Dim expected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        target.NumRecords = expected
        actual = target.NumRecords
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test Value</summary>
    <TestMethod()> Public Sub ValueTest()
        Dim target As atcTableArray = New atcTableArray() ' TODO: Initialize to an appropriate value
        Dim aFieldNumber As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        target.Value(aFieldNumber) = expected
        actual = target.Value(aFieldNumber)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub
End Class
