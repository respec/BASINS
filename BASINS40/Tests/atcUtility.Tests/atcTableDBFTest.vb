Imports System.Collections

Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports atcUtility



'''<summary>
'''This is a test class for atcTableDBFTest and is intended
'''to contain all atcTableDBFTest Unit Tests
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
    '''A test for atcTableDBF Constructor
    '''</summary>
    <TestMethod()> _
    Public Sub atcTableDBFConstructorTest()
        Dim target As atcTableDBF = New atcTableDBF()
        Assert.Inconclusive("TODO: Implement code to verify target")
    End Sub

    '''<summary>
    '''A test for Clear
    '''</summary>
    <TestMethod()> _
    Public Sub ClearTest()
        Dim target As atcTableDBF = New atcTableDBF() ' TODO: Initialize to an appropriate value
        target.Clear()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for ClearData
    '''</summary>
    <TestMethod()> _
    Public Sub ClearDataTest()
        Dim target As atcTableDBF = New atcTableDBF() ' TODO: Initialize to an appropriate value
        target.ClearData()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for Cousin
    '''</summary>
    <TestMethod()> _
    Public Sub CousinTest()
        Dim target As atcTableDBF = New atcTableDBF() ' TODO: Initialize to an appropriate value
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
        Dim target As atcTableDBF = New atcTableDBF() ' TODO: Initialize to an appropriate value
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
        Dim target As atcTableDBF = New atcTableDBF() ' TODO: Initialize to an appropriate value
        Dim aFieldName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        actual = target.FieldNumber(aFieldName)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for FindFirst
    '''</summary>
    <TestMethod()> _
    Public Sub FindFirstTest()
        Dim target As atcTableDBF = New atcTableDBF() ' TODO: Initialize to an appropriate value
        Dim aFieldNumber As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aFindValue As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aStartRecord As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aEndRecord As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = target.FindFirst(aFieldNumber, aFindValue, aStartRecord, aEndRecord)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for FindRecord
    '''</summary>
    <TestMethod()> _
    Public Sub FindRecordTest()
        Dim target As atcTableDBF = New atcTableDBF() ' TODO: Initialize to an appropriate value
        Dim FindValue() As Byte = Nothing ' TODO: Initialize to an appropriate value
        Dim aStartRecord As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aEndRecord As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = target.FindRecord(FindValue, aStartRecord, aEndRecord)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for InitData
    '''</summary>
    <TestMethod()> _
    Public Sub InitDataTest()
        Dim target As atcTableDBF = New atcTableDBF() ' TODO: Initialize to an appropriate value
        target.InitData()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for MatchRecord
    '''</summary>
    <TestMethod()> _
    Public Sub MatchRecordTest()
        Dim target As atcTableDBF = New atcTableDBF() ' TODO: Initialize to an appropriate value
        Dim FindValue() As Byte = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = target.MatchRecord(FindValue)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for Merge
    '''</summary>
    <TestMethod()> _
    Public Sub MergeTest()
        Dim target As atcTableDBF = New atcTableDBF() ' TODO: Initialize to an appropriate value
        Dim aAddFrom As atcTableDBF = Nothing ' TODO: Initialize to an appropriate value
        Dim aKeyFieldNames() As String = Nothing ' TODO: Initialize to an appropriate value
        Dim aDuplicateAction As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aAddedIndexes As ArrayList = Nothing ' TODO: Initialize to an appropriate value
        target.Merge(aAddFrom, aKeyFieldNames, aDuplicateAction, aAddedIndexes)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for OpenFile
    '''</summary>
    <TestMethod()> _
    Public Sub OpenFileTest()
        Dim target As atcTableDBF = New atcTableDBF() ' TODO: Initialize to an appropriate value
        Dim aFilename As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = target.OpenFile(aFilename)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for SetDataAddresses
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcUtility.dll")> _
    Public Sub SetDataAddressesTest()
        Dim target As atcTableDBF_Accessor = New atcTableDBF_Accessor() ' TODO: Initialize to an appropriate value
        target.SetDataAddresses()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for SummaryDataBinary
    '''</summary>
    <TestMethod()> _
    Public Sub SummaryDataBinaryTest()
        Dim target As atcTableDBF = New atcTableDBF() ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = target.SummaryDataBinary
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for SummaryFields
    '''</summary>
    <TestMethod()> _
    Public Sub SummaryFieldsTest()
        Dim target As atcTableDBF = New atcTableDBF() ' TODO: Initialize to an appropriate value
        Dim aFormat As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = target.SummaryFields(aFormat)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for SummaryFile
    '''</summary>
    <TestMethod()> _
    Public Sub SummaryFileTest()
        Dim target As atcTableDBF = New atcTableDBF() ' TODO: Initialize to an appropriate value
        Dim aFormat As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = target.SummaryFile(aFormat)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for TrimNull
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcUtility.dll")> _
    Public Sub TrimNullTest()
        Dim target As atcTableDBF_Accessor = New atcTableDBF_Accessor() ' TODO: Initialize to an appropriate value
        Dim Value As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = target.TrimNull(Value)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for WriteFile
    '''</summary>
    <TestMethod()> _
    Public Sub WriteFileTest()
        Dim target As atcTableDBF = New atcTableDBF() ' TODO: Initialize to an appropriate value
        Dim aFilename As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = target.WriteFile(aFilename)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for findAllNew
    '''</summary>
    <TestMethod()> _
    Public Sub findAllNewTest()
        Dim target As atcTableDBF = New atcTableDBF() ' TODO: Initialize to an appropriate value
        Dim aOtherTable As atcTableDBF = Nothing ' TODO: Initialize to an appropriate value
        Dim aField As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim expected As ArrayList = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ArrayList
        actual = target.findAllNew(aOtherTable, aField)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for findBytes
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcUtility.dll")> _
    Public Sub findBytesTest()
        Dim target As atcTableDBF_Accessor = New atcTableDBF_Accessor() ' TODO: Initialize to an appropriate value
        Dim aFindThis() As Byte = Nothing ' TODO: Initialize to an appropriate value
        Dim aFindFirstByte As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aFindNumBytes As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aSearchIn() As Byte = Nothing ' TODO: Initialize to an appropriate value
        Dim aSearchStart As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aSearchStride As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aSearchStop As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aFieldLength As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim expected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        actual = target.findBytes(aFindThis, aFindFirstByte, aFindNumBytes, aSearchIn, aSearchStart, aSearchStride, aSearchStop, aFieldLength)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for CurrentRecord
    '''</summary>
    <TestMethod()> _
    Public Sub CurrentRecordTest()
        Dim target As atcTableDBF = New atcTableDBF() ' TODO: Initialize to an appropriate value
        Dim expected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        target.CurrentRecord = expected
        actual = target.CurrentRecord
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for Day
    '''</summary>
    <TestMethod()> _
    Public Sub DayTest()
        Dim target As atcTableDBF = New atcTableDBF() ' TODO: Initialize to an appropriate value
        Dim expected As Byte = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Byte
        target.Day = expected
        actual = target.Day
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for FieldDecimalCount
    '''</summary>
    <TestMethod()> _
    Public Sub FieldDecimalCountTest()
        Dim target As atcTableDBF = New atcTableDBF() ' TODO: Initialize to an appropriate value
        Dim aFieldNumber As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim expected As Byte = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Byte
        target.FieldDecimalCount(aFieldNumber) = expected
        actual = target.FieldDecimalCount(aFieldNumber)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for FieldLength
    '''</summary>
    <TestMethod()> _
    Public Sub FieldLengthTest()
        Dim target As atcTableDBF = New atcTableDBF() ' TODO: Initialize to an appropriate value
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
        Dim target As atcTableDBF = New atcTableDBF() ' TODO: Initialize to an appropriate value
        Dim aFieldNumber As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        target.FieldName(aFieldNumber) = expected
        actual = target.FieldName(aFieldNumber)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for FieldType
    '''</summary>
    <TestMethod()> _
    Public Sub FieldTypeTest()
        Dim target As atcTableDBF = New atcTableDBF() ' TODO: Initialize to an appropriate value
        Dim aFieldNumber As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        target.FieldType(aFieldNumber) = expected
        actual = target.FieldType(aFieldNumber)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for Month
    '''</summary>
    <TestMethod()> _
    Public Sub MonthTest()
        Dim target As atcTableDBF = New atcTableDBF() ' TODO: Initialize to an appropriate value
        Dim expected As Byte = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Byte
        target.Month = expected
        actual = target.Month
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for NumFields
    '''</summary>
    <TestMethod()> _
    Public Sub NumFieldsTest()
        Dim target As atcTableDBF = New atcTableDBF() ' TODO: Initialize to an appropriate value
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
        Dim target As atcTableDBF = New atcTableDBF() ' TODO: Initialize to an appropriate value
        Dim expected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        target.NumRecords = expected
        actual = target.NumRecords
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for RawBytesPerRecord
    '''</summary>
    <TestMethod()> _
    Public Sub RawBytesPerRecordTest()
        Dim target As atcTableDBF = New atcTableDBF() ' TODO: Initialize to an appropriate value
        Dim expected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        target.RawBytesPerRecord = expected
        actual = target.RawBytesPerRecord
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for RawCurrentRecordStart
    '''</summary>
    <TestMethod()> _
    Public Sub RawCurrentRecordStartTest()
        Dim target As atcTableDBF = New atcTableDBF() ' TODO: Initialize to an appropriate value
        Dim expected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        target.RawCurrentRecordStart = expected
        actual = target.RawCurrentRecordStart
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for RawData
    '''</summary>
    <TestMethod()> _
    Public Sub RawDataTest()
        Dim target As atcTableDBF = New atcTableDBF() ' TODO: Initialize to an appropriate value
        Dim expected() As Byte = Nothing ' TODO: Initialize to an appropriate value
        Dim actual() As Byte
        target.RawData = expected
        actual = target.RawData
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for RawRecord
    '''</summary>
    <TestMethod()> _
    Public Sub RawRecordTest()
        Dim target As atcTableDBF = New atcTableDBF() ' TODO: Initialize to an appropriate value
        Dim expected() As Byte = Nothing ' TODO: Initialize to an appropriate value
        Dim actual() As Byte
        target.RawRecord = expected
        actual = target.RawRecord
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for RawValueStart
    '''</summary>
    <TestMethod()> _
    Public Sub RawValueStartTest()
        Dim target As atcTableDBF = New atcTableDBF() ' TODO: Initialize to an appropriate value
        Dim aFieldNumber As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        actual = target.RawValueStart(aFieldNumber)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for Value
    '''</summary>
    <TestMethod()> _
    Public Sub ValueTest()
        Dim target As atcTableDBF = New atcTableDBF() ' TODO: Initialize to an appropriate value
        Dim aFieldNumber As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        target.Value(aFieldNumber) = expected
        actual = target.Value(aFieldNumber)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for Year
    '''</summary>
    <TestMethod()> _
    Public Sub YearTest()
        Dim target As atcTableDBF = New atcTableDBF() ' TODO: Initialize to an appropriate value
        Dim expected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        target.Year = expected
        actual = target.Year
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub
End Class
