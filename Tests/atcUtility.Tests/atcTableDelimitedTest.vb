Imports System.IO
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports atcUtility

'''<summary>
'''This is a test class for atcTableDelimitedTest and is intended
'''to contain all atcTableDelimitedTest Unit Tests (Done, except ToStringPivotedTest)
'''</summary>
<TestClass()> _
Public Class atcTableDelimitedTest
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

    Private pFileName As String = "C:\dev\BASINS40\Tests\Data\Test_atcTableDelimited.csv"
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

    '''<summary>Test atcTableDelimited Constructor</summary>
    <TestMethod()> Public Sub atcTableDelimitedConstructorTest()
        Dim target As atcTableDelimited = New atcTableDelimited()
        Assert.IsInstanceOfType(target, GetType(atcTableDelimited))
    End Sub

    '''<summary>Test Clear</summary>
    <TestMethod()> Public Sub ClearTest()
        Dim target As atcTableDelimited = New atcTableDelimited() ' TODO: Initialize to an appropriate value
        target.Delimiter = ","
        target.NumHeaderRows = 2
        target.Header(1) = "header1"
        target.Header(2) = "header2"

        Assert.AreEqual(2, target.NumHeaderRows)
        target.Clear()
        Assert.AreEqual(0, target.NumHeaderRows) 'Problem: Need to reset the NumHeaderRows when Clear
    End Sub

    '''<summary>Test Cousin</summary>
    <TestMethod()> Public Sub CousinTest()
        Dim target As atcTableDelimited = New atcTableDelimited() ' TODO: Initialize to an appropriate value
        target.NumFields = 3
        Dim actual As IatcTable = target.Cousin
        Assert.AreEqual(3, actual.NumFields)
    End Sub

    '''<summary>Test CreationCode</summary>
    <TestMethod()> Public Sub CreationCodeTest()
        Dim target As atcTableDelimited = New atcTableDelimited() ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = target.CreationCode
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test FieldNumber</summary>
    <TestMethod()> Public Sub FieldNumberTest()
        Dim target As atcTableDelimited = New atcTableDelimited()
        target.NumFields = 3
        target.FieldName(1) = "Test1"
        target.FieldName(2) = "Test2"
        target.FieldName(3) = "Test3"
        Assert.AreEqual(2, target.FieldNumber("Test2"))
    End Sub

    '''<summary>Test OpenFile</summary>
    <TestMethod()> Public Sub OpenFileTest()
        Dim target As atcTableDelimited = New atcTableDelimited()
        target.Delimiter = ","
        target.NumHeaderRows = 1
        Dim actual As Boolean = target.OpenFile(pFileName)
        Assert.AreEqual(True, actual)
        Assert.AreEqual(1, target.NumHeaderRows)
        Assert.AreEqual(3, target.NumFields)
        target.Clear()
    End Sub

    '''<summary>Test OpenStream</summary>
    <TestMethod()> Public Sub OpenStreamTest()
        Dim target As atcTableDelimited = New atcTableDelimited()
        Dim lStream As New IO.StreamReader(pFileName)
        Dim actual As Boolean = target.OpenStream(lStream.BaseStream)
        Assert.AreEqual(True, actual)
        target.Clear()
    End Sub

    '''<summary>Test OpenString</summary>
    <TestMethod()> Public Sub OpenStringTest()
        Dim target As atcTableDelimited = New atcTableDelimited() 

        Dim lStream As New IO.StreamReader(pFileName)
        Dim lString As String = lStream.ReadToEnd()
        Dim actual As Boolean = target.OpenString(lString)
        Assert.AreEqual(True, actual)
        target.Clear()
    End Sub

    '''<summary>Test ToString</summary>
    <TestMethod()> Public Sub ToStringTest()
        Dim target As atcTableDelimited = New atcTableDelimited() ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = target.ToString
        Assert.AreEqual(expected, actual)
    End Sub

    '''<summary>Test ToStringPivoted</summary>
    <TestMethod()> Public Sub ToStringPivotedTest()
        Dim target As atcTableDelimited = New atcTableDelimited()
        target.Delimiter = ","
        target.NumFields = 3
        target.NumHeaderRows = 0 'Default, the first row is field name's row
        target.OpenFile(pFileName)
        target.CurrentRecord = 2
        Dim lNewTab As New atcTableDelimited()
        lNewTab.Delimiter = ","
        lNewTab.NumFields = 4
        lNewTab.NumHeaderRows = 0
        lNewTab.OpenString(target.ToStringPivoted)
        lNewTab.CurrentRecord = 2
        Assert.AreEqual(target.Value(2), lNewTab.Value(2))
    End Sub

    '''<summary>Test WriteFile</summary>
    <TestMethod()> Public Sub WriteFileTest()
        Dim target As atcTableDelimited = New atcTableDelimited() ' TODO: Initialize to an appropriate value
        Dim aFilename As String = IO.Path.ChangeExtension(pFileName, "txt")
        Dim expected As Boolean = True
        Dim actual As Boolean = target.WriteFile(aFilename)
        Assert.AreEqual(expected, actual)
    End Sub

    '''<summary>Test CurrentRecord</summary>
    <TestMethod()> Public Sub CurrentRecordTest()
        Dim target As atcTableDelimited = New atcTableDelimited()
        target.Delimiter = ","
        target.OpenFile(pFileName)
        target.CurrentRecord = 2
        Assert.AreEqual(2, target.CurrentRecord)
        target.Clear()
    End Sub

    '''<summary>Test Delimiter</summary>
    <TestMethod()> Public Sub DelimiterTest()
        Dim target As atcTableDelimited = New atcTableDelimited() ' TODO: Initialize to an appropriate value
        Dim expected As Char = Global.Microsoft.VisualBasic.ChrW(0) ' TODO: Initialize to an appropriate value
        Dim actual As Char
        target.Delimiter = expected
        actual = target.Delimiter
        Assert.AreEqual(expected, actual)
    End Sub

    '''<summary>Test FieldLength</summary>
    <TestMethod()> Public Sub FieldLengthTest()
        Dim target As atcTableDelimited = New atcTableDelimited()
        target.NumFields = 3
        target.Delimiter = ","
        Dim aFieldNumber As Integer = 2
        target.FieldLength(aFieldNumber) = 5
        Dim actual As Integer = target.FieldLength(aFieldNumber)
        Assert.AreEqual(5, actual)
    End Sub

    '''<summary>Test FieldName</summary>
    <TestMethod()> Public Sub FieldNameTest()
        Dim target As atcTableDelimited = New atcTableDelimited() ' TODO: Initialize to an appropriate value
        target.NumFields = 3
        target.Delimiter = ","
        Dim aFieldNumber As Integer = 2
        target.FieldName(aFieldNumber) = "Test2"
        Dim actual As String = target.FieldName(aFieldNumber)
        Assert.AreEqual("Test2", actual)
    End Sub

    '''<summary>Test FieldType</summary>
    <TestMethod()> Public Sub FieldTypeTest()
        Dim target As atcTableDelimited = New atcTableDelimited()
        target.NumFields = 3
        Dim aFieldNumber As Integer = 2
        Dim expected As String = "String"
        target.FieldType(aFieldNumber) = expected
        Dim actual As String = target.FieldType(aFieldNumber)
        Assert.AreEqual(expected, actual)
    End Sub

    '''<summary>Test NumFields</summary>
    <TestMethod()> Public Sub NumFieldsTest()
        Dim target As atcTableDelimited = New atcTableDelimited() 
        target.NumFields = 3
        Assert.AreEqual(3, target.NumFields)
    End Sub

    '''<summary>Test NumRecords</summary>
    <TestMethod()> Public Sub NumRecordsTest()
        Dim target As atcTableDelimited = New atcTableDelimited()
        target.Delimiter = ","
        target.NumHeaderRows = 0
        target.OpenFile(pFileName)

        Assert.AreEqual(3, target.NumRecords)
        target.Clear()
    End Sub

    '''<summary>Test TrimValues</summary>
    <TestMethod()> Public Sub TrimValuesTest()
        Dim target As atcTableDelimited = New atcTableDelimited() ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        target.TrimValues = expected
        actual = target.TrimValues
        Assert.AreEqual(expected, actual)
    End Sub

    '''<summary>Test Value</summary>
    <TestMethod()> Public Sub ValueTest()
        Dim target As atcTableDelimited = New atcTableDelimited()
        target.Delimiter = ","
        target.NumHeaderRows = 0
        target.OpenFile(pFileName)
        target.CurrentRecord = 2
        Assert.AreEqual("Value22", target.Value(2))
        target.Clear()
    End Sub
End Class
