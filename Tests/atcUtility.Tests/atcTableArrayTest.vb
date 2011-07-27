Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports atcUtility

'''<summary>
'''This is a test class for atcTableArrayTest and is intended
'''to contain all atcTableArrayTest Unit Tests (Done)
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
        Assert.IsInstanceOfType(target, GetType(atcTableArray))
    End Sub

    '''<summary>Test Clear</summary>
    <TestMethod()> Public Sub ClearTest()
        Dim target As atcTableArray = New atcTableArray()
        target.NumHeaderRows = 2
        target.Header(1) = "header1"
        target.Header(2) = "header2"
        target.NumRecords = 2
        Dim lrec1() As String = {"rec1"}
        Dim lrec2() As String = {"rec2"}
        target.pRecords(0) = lrec1
        target.pRecords(1) = lrec2

        Assert.AreEqual(2, target.pRecords.Count)
        Assert.AreEqual(2, target.NumHeaderRows)
        target.Clear()
        Assert.AreEqual(0, target.pRecords.Count)
        Assert.AreEqual(0, target.NumHeaderRows) 'Problem: Need to reset the NumHeaderRows when Clear

    End Sub

    '''<summary>Test ClearData</summary>
    <TestMethod()> Public Sub ClearDataTest()
        Dim target As atcTableArray = New atcTableArray()
        target.NumHeaderRows = 2
        target.Header(1) = "header1"
        target.Header(2) = "header2"
        target.NumRecords = 2
        Dim lrec1() As String = {"rec1"}
        Dim lrec2() As String = {"rec2"}
        target.pRecords(0) = lrec1
        target.pRecords(1) = lrec2
        Assert.AreEqual(2, target.pRecords.Count)
        target.ClearData()
        Assert.AreEqual(0, target.pRecords.Count)
    End Sub

    '''<summary>Test Cousin</summary>
    <TestMethod()> Public Sub CousinTest()
        Dim target As atcTableArray = New atcTableArray()
        target.NumFields = 3
        Dim actual As IatcTable = target.Cousin
        Assert.AreEqual(3, actual.NumFields)
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
        Dim target As atcTableArray = New atcTableArray()
        Assert.AreEqual("", target.CreationCode)
    End Sub

    '''<summary>Test OpenFile</summary>
    <TestMethod()> Public Sub OpenFileTest()
        Dim target As atcTableArray = New atcTableArray()
        Assert.AreEqual(False, target.OpenFile(IO.Path.Combine(Environment.SystemDirectory, "cmd.exe")))
    End Sub

    '''<summary>Test WriteFile</summary>
    <TestMethod()> Public Sub WriteFileTest()
        Dim target As atcTableArray = New atcTableArray()
        target.NumHeaderRows = 2
        target.Header(1) = "header1"
        target.Header(2) = "header2"
        target.NumRecords = 2
        Dim lrec1() As String = {"rec1"}
        Dim lrec2() As String = {"rec2"}
        target.pRecords(0) = lrec1
        target.pRecords(1) = lrec2
        target.NumFields = 1
        target.FieldName(1) = "Field1" 'Must have fieldname specified, or it will fail
        Dim aFilename As String = IO.Path.Combine(Environment.CurrentDirectory, "atcTableArrayWriteFile.txt")
        Dim expected As Boolean = True
        Dim actual As Boolean = target.WriteFile(aFilename)
        Assert.AreEqual(expected, actual)
    End Sub

    '''<summary>Test CurrentRecord</summary>
    <TestMethod()> Public Sub CurrentRecordTest()
        Dim target As atcTableArray = New atcTableArray()
        target.NumHeaderRows = 2
        target.Header(1) = "header1"
        target.Header(2) = "header2"
        target.NumRecords = 2
        Dim lrec1() As String = {"rec1"}
        Dim lrec2() As String = {"rec2"}
        target.pRecords(0) = lrec1
        target.pRecords(1) = lrec2
        target.NumFields = 1
        target.FieldName(1) = "Field1" 'Must have fieldname specified, or it will fail

        target.CurrentRecord = 1
        Dim actual As Integer = target.CurrentRecord
        Assert.AreEqual(1, actual)
        target.CurrentRecord = 2
        actual = target.CurrentRecord
        Assert.AreEqual(2, actual)
        target.CurrentRecord = 3
        Assert.AreEqual(True, target.EOF)
    End Sub

    '''<summary>Test Delimiter</summary>
    <TestMethod()> Public Sub DelimiterTest()
        Dim target As atcTableArray = New atcTableArray() ' TODO: Initialize to an appropriate value
        Dim expected As Char = Global.Microsoft.VisualBasic.ChrW(0) ' TODO: Initialize to an appropriate value

        target.Delimiter = expected
        Dim actual As Char = target.Delimiter
        Assert.AreEqual(expected, actual)
    End Sub

    '''<summary>Test FieldLength</summary>
    <TestMethod()> Public Sub FieldLengthTest()
        Dim target As atcTableArray = New atcTableArray()
        target.NumFields = 3
        target.FieldLength(1) = 1
        target.FieldLength(2) = 2
        target.FieldLength(3) = 3
        Assert.AreEqual(1, target.FieldLength(1))
        Assert.AreEqual(2, target.FieldLength(2))
        Assert.AreEqual(3, target.FieldLength(3))
    End Sub

    '''<summary>Test FieldName</summary>
    <TestMethod()> Public Sub FieldNameTest()
        Dim target As atcTableArray = New atcTableArray()
        target.NumFields = 3
        target.FieldName(1) = "1"
        target.FieldName(2) = "2"
        target.FieldName(3) = "3"
        Assert.AreEqual("1", target.FieldName(1))
        Assert.AreEqual("2", target.FieldName(2))
        Assert.AreEqual("3", target.FieldName(3))
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
        Dim target As atcTableArray = New atcTableArray()
        Assert.AreEqual(0, target.NumFields)
        target.NumFields = 3

        Assert.AreEqual(3, target.NumFields)
        Assert.AreEqual("", target.FieldName(target.NumFields))
    End Sub

    '''<summary>Test NumRecords</summary>
    <TestMethod()> Public Sub NumRecordsTest()
        Dim target As atcTableArray = New atcTableArray()
        Assert.AreEqual(0, target.NumRecords)
        target.NumRecords = 3
        Assert.AreEqual(3, target.NumRecords)
        target.NumRecords = 2
        Assert.AreEqual(2, target.NumRecords)
    End Sub

    '''<summary>Test Value</summary>
    <TestMethod()> Public Sub ValueTest()
        Dim target As atcTableArray = New atcTableArray()
        target.NumHeaderRows = 2
        target.Header(1) = "header1"
        target.Header(2) = "header2"
        target.NumRecords = 2
        Dim lrec1() As String = {"rec11", "rec12", "rec13"}
        Dim lrec2() As String = {"rec21", "rec22", "rec23"}
        target.pRecords(0) = lrec1
        target.pRecords(1) = lrec2
        target.NumFields = 3
        target.FieldName(1) = "Field1"
        target.FieldName(2) = "Field2"
        target.FieldName(3) = "Field3"
        target.CurrentRecord = 1
        Dim actual As String = target.Value(1)
        Assert.AreEqual("rec11", actual)
        target.CurrentRecord += 1
        actual = target.Value(3)
        Assert.AreEqual("rec23", actual)
    End Sub
End Class
