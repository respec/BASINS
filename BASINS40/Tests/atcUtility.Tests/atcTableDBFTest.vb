Imports System.Collections
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports atcUtility

'''<summary>
'''This is a test class for atcTableDBFTest and is intended
'''to contain all atcTableDBFTest Unit Tests (Done, with problem)
'''</summary>
<TestClass()> _
Public Class atcTableDBFTest
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
    Private pFileName As String = "C:\dev\BASINS40\Tests\Data\Test_atcTableDBF.dbf"
    Private pFileName1 As String = "C:\dev\BASINS40\Tests\Data\Test_atcTableDBF1.dbf"

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

    '''<summary>Test atcTableDBF Constructor</summary>
    <TestMethod()> Public Sub atcTableDBFConstructorTest()
        Dim target As atcTableDBF = New atcTableDBF()
        Assert.AreEqual(0, target.NumFields)
        Assert.AreEqual(0, target.NumHeaderRows)
        Assert.AreEqual(0, target.NumRecords)
    End Sub

    '''<summary>Test Clear</summary>
    <TestMethod()> Public Sub ClearTest()
        Dim target As atcTableDBF = New atcTableDBF() ' TODO: Initialize to an appropriate value
        target.Clear()
        Assert.AreEqual(0, target.NumFields)
        Assert.AreEqual(0, target.NumHeaderRows)
        Assert.AreEqual(0, target.NumRecords)
    End Sub

    '''<summary>Test ClearData</summary>
    <TestMethod()> Public Sub ClearDataTest()
        Dim target As atcTableDBF = New atcTableDBF() ' TODO: Initialize to an appropriate value
        target.ClearData()
        Assert.AreEqual(0, target.NumFields)
        Assert.AreEqual(0, target.NumHeaderRows)
        Assert.AreEqual(0, target.NumRecords)
    End Sub

    '''<summary>Test Cousin</summary>
    <TestMethod()> Public Sub CousinTest()
        Dim target As atcTableDBF = New atcTableDBF() ' TODO: Initialize to an appropriate value
        target.NumFields = 3
        Dim actual As IatcTable = target.Cousin
        Assert.AreEqual(3, actual.NumFields)
    End Sub

    '''<summary>Test CreationCode</summary>
    <TestMethod()> Public Sub CreationCodeTest()
        Dim target As atcTableDBF = New atcTableDBF() ' TODO: Initialize to an appropriate value
        Dim actual As String = target.CreationCode
        Assert.AreNotEqual("", actual)
    End Sub

    '''<summary>Test FieldNumber</summary>
    <TestMethod()> Public Sub FieldNumberTest()
        Dim target As atcTableDBF = New atcTableDBF() ' TODO: Initialize to an appropriate value
        target.NumFields = 3
        target.FieldName(1) = "Field1"
        target.FieldName(2) = "Field2"
        target.FieldName(3) = "Field3"
        Assert.AreEqual(2, target.FieldNumber("Field2"))
    End Sub

    '''<summary>Test FindFirst</summary>
    <TestMethod()> Public Sub FindFirstTest()
        Dim target As atcTableDBF = New atcTableDBF()
        target.OpenFile(pFileName)

        Dim aFieldNumber As Integer = 3
        Dim aFindValue As String = "Two"
        Dim aStartRecord As Integer = 1
        Dim aEndRecord As Integer = target.NumRecords
        Dim expected As Boolean = True
        Dim actual As Boolean = target.FindFirst(aFieldNumber, aFindValue, aStartRecord, aEndRecord)
        Assert.AreEqual(expected, actual)
        aFindValue = "Anything"
        actual = target.FindFirst(aFieldNumber, aFindValue, aStartRecord, aEndRecord)
        Assert.AreNotEqual(expected, actual)
        target.Clear()
    End Sub

    '''<summary>Test FindRecord</summary>
    <TestMethod()> Public Sub FindRecordTest()
        Dim target As atcTableDBF = New atcTableDBF()
        Dim FindValue() As Byte = Nothing
        Dim aStartRecord As Integer = 1
        Dim aEndRecord As Integer = target.NumRecords
        Dim expected As Boolean = False
        Dim actual As Boolean = target.FindRecord(FindValue, aStartRecord, aEndRecord)
        Assert.AreEqual(expected, actual)
    End Sub

    '''<summary>Test InitData</summary>
    <TestMethod()> Public Sub InitDataTest()
        Dim target As atcTableDBF = New atcTableDBF()
        target.OpenFile(pFileName)
        Assert.AreEqual(3, target.NumRecords)
        Assert.AreEqual(3, target.NumFields)
        target.InitData()
        Assert.AreEqual(3, target.NumRecords)
        Assert.AreEqual(3, target.NumFields)
        target.Clear()

        target = New atcTableDBF()
        target.NumFields = 3
        target.NumRecords = 3
        For I As Integer = 1 To target.NumFields
            target.FieldLength(I) = 9
        Next
        target.InitData()
        Assert.AreEqual(3, target.NumRecords)
        Assert.AreEqual(3, target.NumFields)
        target.Clear()

    End Sub

    '''<summary>Test MatchRecord</summary>
    <TestMethod()> Public Sub MatchRecordTest()
        Dim target As atcTableDBF = New atcTableDBF()
        target.OpenFile(pFileName)
        Dim FindValue() As Byte = Nothing
        ReDim FindValue(target.RawBytesPerRecord)
        Array.Copy(target.RawRecord(), FindValue, target.RawBytesPerRecord)
        Dim expected As Boolean = True
        Dim actual As Boolean = target.MatchRecord(FindValue)
        Assert.AreEqual(expected, actual)
        target.Clear()
    End Sub

    '''<summary>Test Merge</summary>
    <TestMethod()> Public Sub MergeTest()
        Dim target As atcTableDBF = New atcTableDBF()
        target.OpenFile(pFileName)

        Dim aAddFrom As New atcTableDBF()
        aAddFrom.OpenFile(pFileName1)

        Dim aKeyFieldNames() As String = Nothing
        Dim aDuplicateAction As Integer = 0
        Dim aAddedIndexes As ArrayList = Nothing
        target.Merge(aAddFrom, aKeyFieldNames, aDuplicateAction, aAddedIndexes)

        Assert.AreEqual(7, target.NumRecords)
        Assert.AreEqual(3, target.NumFields)

        target.Clear()
        aAddFrom.Clear()

        target.OpenFile(pFileName)
        aAddFrom.OpenFile(pFileName1)

        aDuplicateAction = 1
        target.Merge(aAddFrom, aKeyFieldNames, aDuplicateAction, aAddedIndexes)
        Assert.AreEqual(7, target.NumRecords)
        Assert.AreEqual(3, target.NumFields)

        target.Clear()
        aAddFrom.Clear()
    End Sub

    '''<summary>Test OpenFile</summary>
    <TestMethod()> Public Sub OpenFileTest()
        Dim target As atcTableDBF = New atcTableDBF()
        Dim actual As Boolean = target.OpenFile(pFileName)
        Assert.AreEqual(True, actual)
        target.Clear()
    End Sub

    '''<summary>Test SetDataAddresses</summary>
    <TestMethod(), DeploymentItem("atcUtility.dll")> _
    Public Sub SetDataAddressesTest()
        Dim target As atcTableDBF_Accessor = New atcTableDBF_Accessor() ' TODO: Initialize to an appropriate value
        target.SetDataAddresses()
        Assert.Inconclusive("Private method")
    End Sub

    '''<summary>Test SummaryDataBinary</summary>
    <TestMethod()> Public Sub SummaryDataBinaryTest()
        Dim target As atcTableDBF = New atcTableDBF()
        target.OpenFile(pFileName)
        Dim actual As String = target.SummaryDataBinary
        Assert.AreNotEqual("", actual)
        target.Clear()
    End Sub

    '''<summary>Test SummaryFields</summary>
    <TestMethod()> Public Sub SummaryFieldsTest()
        Dim target As atcTableDBF = New atcTableDBF()
        target.OpenFile(pFileName)
        Dim aFormat As String = String.Empty
        Dim actual As String = target.SummaryFields(aFormat)
        Assert.AreNotEqual("", actual)
        target.Clear()
    End Sub

    '''<summary>Test SummaryFile</summary>
    <TestMethod()> Public Sub SummaryFileTest()
        Dim target As atcTableDBF = New atcTableDBF()
        Dim aFormat As String = String.Empty
        Dim actual As String = target.SummaryFile(aFormat)
        Assert.AreNotEqual("", actual)
        target.Clear()
    End Sub

    '''<summary>Test TrimNull</summary>
    <TestMethod(), DeploymentItem("atcUtility.dll")> _
    Public Sub TrimNullTest()
        Dim target As atcTableDBF_Accessor = New atcTableDBF_Accessor() ' TODO: Initialize to an appropriate value
        Dim Value As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = target.TrimNull(Value)
        Assert.AreEqual(expected, actual)
    End Sub

    '''<summary>Test WriteFile</summary>
    <TestMethod()> Public Sub WriteFileTest()
        Dim target As atcTableDBF = New atcTableDBF()
        target.OpenFile(pFileName1)
        Dim aFilename As String = pFileName1.Replace("1", "WriteOut")
        Dim expected As Boolean = True
        Dim actual As Boolean = target.WriteFile(aFilename)
        Assert.AreEqual(expected, actual)
        target.Clear()
    End Sub

    '''<summary>Test findAllNew</summary>
    <TestMethod()> Public Sub findAllNewTest()
        Dim target As atcTableDBF = New atcTableDBF()
        target.OpenFile(pFileName)
        Dim aOtherTable As New atcTableDBF()
        aOtherTable.OpenFile(pFileName1)

        Dim aField As Integer = 3
        Dim actual As ArrayList = target.findAllNew(aOtherTable, aField)
        Assert.AreEqual(3, actual.Count) 'problem, found a 4th 'New' record, but actuall should be only 3

        aField = 0
        actual.Clear()
        actual = target.findAllNew(aOtherTable, aField)
        Assert.AreEqual(3, actual.Count) 'problem, found a 4th 'New' record, but actuall should be only 3

        target.Clear()
        aOtherTable.Clear()

    End Sub

    '''<summary>Test findBytes</summary>
    <TestMethod(), DeploymentItem("atcUtility.dll")> _
    Public Sub findBytesTest()
        Dim target As atcTableDBF_Accessor = New atcTableDBF_Accessor()
        Dim aFindThis() As Byte = {0, 1, 2, 3}
        Dim aFindFirstByte As Integer = 1
        Dim aFindNumBytes As Integer = 2
        Dim aSearchIn() As Byte = {0, 0, 1, 2, 4, 1, 2, 0, 0}
        Dim aSearchStart As Integer = 1
        Dim aSearchStride As Integer = 4
        Dim aSearchStop As Integer = 8
        Dim aFieldLength As Integer = 5
        Dim expected As Integer = 2
        Dim actual As Integer
        actual = target.findBytes(aFindThis, aFindFirstByte, aFindNumBytes, aSearchIn, aSearchStart, aSearchStride, aSearchStop, aFieldLength)
        Assert.AreEqual(expected, actual)
    End Sub

    '''<summary>Test CurrentRecord</summary>
    <TestMethod()> Public Sub CurrentRecordTest()
        Dim target As atcTableDBF = New atcTableDBF()
        target.OpenFile(pFileName)
        target.CurrentRecord = 2
        Assert.AreEqual("2", target.Value(1))
    End Sub

    '''<summary>Test Day</summary>
    <TestMethod()> Public Sub DayTest()
        Dim target As atcTableDBF = New atcTableDBF() ' TODO: Initialize to an appropriate value
        Dim expected As Byte = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Byte
        target.Day = expected
        actual = target.Day
        Assert.AreEqual(expected, actual)
    End Sub

    '''<summary>Test FieldDecimalCount</summary>
    <TestMethod()> Public Sub FieldDecimalCountTest()
        Dim target As atcTableDBF = New atcTableDBF()
        target.OpenFile(pFileName)

        Dim aFieldNumber As Integer = 2
        Dim expected As Byte = 2 ' TODO: Initialize to an appropriate value
        Dim actual As Byte = target.FieldDecimalCount(aFieldNumber)
        Assert.AreEqual(CByte(1), actual)
        target.FieldDecimalCount(aFieldNumber) = expected
        actual = target.FieldDecimalCount(aFieldNumber)
        Assert.AreEqual(CByte(2), actual)
    End Sub

    '''<summary>Test FieldLength</summary>
    <TestMethod()> Public Sub FieldLengthTest()
        Dim target As atcTableDBF = New atcTableDBF()
        target.NumFields = 5
        Dim aFieldNumber As Integer = 5
        Dim expected As Integer = 5
        Dim actual As Integer
        target.FieldLength(aFieldNumber) = expected
        actual = target.FieldLength(aFieldNumber)
        Assert.AreEqual(expected, actual)
    End Sub

    '''<summary>Test FieldName</summary>
    <TestMethod()> Public Sub FieldNameTest()
        Dim target As atcTableDBF = New atcTableDBF()
        target.NumFields = 5
        Dim aFieldNumber As Integer = 5
        Dim expected As String = "TestField5"
        Dim actual As String
        target.FieldName(aFieldNumber) = expected
        actual = target.FieldName(aFieldNumber)
        Assert.AreEqual(expected, actual)
    End Sub

    '''<summary>Test FieldType</summary>
    <TestMethod()> Public Sub FieldTypeTest()
        Dim target As atcTableDBF = New atcTableDBF()
        target.NumFields = 5
        Dim aFieldNumber As Integer = 5
        Dim expected As String = "String"
        Dim actual As String
        target.FieldType(aFieldNumber) = expected
        actual = target.FieldType(aFieldNumber)
        Assert.AreEqual(expected, actual)
    End Sub

    '''<summary>Test Month</summary>
    <TestMethod()> Public Sub MonthTest()
        Dim target As atcTableDBF = New atcTableDBF()
        Dim expected As Byte = 0
        Dim actual As Byte
        target.Month = expected
        actual = target.Month
        Assert.AreEqual(expected, actual)
    End Sub

    '''<summary>Test NumFields</summary>
    <TestMethod()> Public Sub NumFieldsTest()
        Dim target As atcTableDBF = New atcTableDBF()
        Dim expected As Integer = 5
        Dim actual As Integer
        target.NumFields = expected
        actual = target.NumFields
        Assert.AreEqual(expected, actual)
    End Sub

    '''<summary>Test NumRecords</summary>
    <TestMethod()> Public Sub NumRecordsTest()
        Dim target As atcTableDBF = New atcTableDBF()
        Dim expected As Integer = 5
        Dim actual As Integer
        target.NumRecords = expected
        actual = target.NumRecords
        Assert.AreEqual(expected, actual)
    End Sub

    '''<summary>Test RawBytesPerRecord</summary>
    <TestMethod()> Public Sub RawBytesPerRecordTest()
        Dim target As atcTableDBF = New atcTableDBF()
        Dim expected As Integer = 55
        Dim actual As Integer
        target.RawBytesPerRecord = expected
        actual = target.RawBytesPerRecord
        Assert.AreEqual(expected, actual)
    End Sub

    '''<summary>Test RawCurrentRecordStart</summary>
    <TestMethod()> Public Sub RawCurrentRecordStartTest()
        Dim target As atcTableDBF = New atcTableDBF()
        Dim expected As Integer = 1
        Dim actual As Integer
        target.RawCurrentRecordStart = expected
        actual = target.RawCurrentRecordStart
        Assert.AreEqual(expected, actual)
    End Sub

    '''<summary>Test RawData</summary>
    <TestMethod()> Public Sub RawDataTest()
        Dim target As atcTableDBF = New atcTableDBF()
        Dim expected() As Byte = {0, 1, 2, 3, 4, 5}
        Dim actual() As Byte
        target.RawData = expected
        actual = target.RawData
        Assert.AreEqual(expected, actual)
    End Sub

    '''<summary>Test RawRecord</summary>
    <TestMethod()> Public Sub RawRecordTest()
        Dim target As atcTableDBF = New atcTableDBF()
        target.RawRecord = New Byte() {1, 1, 1, 1, 1, 1}
        Dim expected() As Byte = {0, 1, 2, 3, 4, 5}
        target.RawRecord = expected 'problem: can't assign a new byte() like this as in the code
        Dim actual() As Byte = target.RawRecord
        Assert.AreEqual(expected.Length, actual.Length)
    End Sub

    '''<summary>Test RawValueStart</summary>
    <TestMethod()> Public Sub RawValueStartTest()
        Dim target As atcTableDBF = New atcTableDBF()
        target.NumFields = 5
        Dim aFieldNumber As Integer = 5
        Dim actual As Integer = target.RawValueStart(aFieldNumber)
        Assert.AreEqual(0, actual)
    End Sub

    '''<summary>Test Value</summary>
    <TestMethod()> Public Sub ValueTest()
        Dim target As atcTableDBF = New atcTableDBF()
        target.OpenFile(pFileName)
        Dim aFieldNumber As Integer = 3
        Dim expected As String = "JustChanged"
        target.CurrentRecord = 2
        target.Value(aFieldNumber) = expected
        Dim actual As String = target.Value(aFieldNumber)
        Assert.AreEqual(expected, actual)
        target.Clear()
    End Sub

    '''<summary>Test Year</summary>
    <TestMethod()> Public Sub YearTest()
        Dim target As atcTableDBF = New atcTableDBF()
        Dim expected As Integer = 1999

        target.Year = expected
        Dim actual As Integer = target.Year
        Assert.AreEqual(expected, actual)
    End Sub
End Class
