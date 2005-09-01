Option Strict Off
Option Explicit On 
'Copyright 2005 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Imports NUnit.Framework
Imports atcUtility.modReflection

Imports System.Drawing

<TestFixture()> Public Class Test_Builder
  Public Sub TestsAllPresent()
    Dim lTestBuildStatus As String

    lTestBuildStatus = BuildMissingTests("c:\test\")
    Assert.AreEqual("All tests present.", lTestBuildStatus, lTestBuildStatus)
  End Sub
End Class

<TestFixture()> Public Class Test_clsATCTable

  Public Sub TestMovePrevious()
    'MovePrevious()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestMoveNext()
    'MoveNext()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestMoveLast()
    'MoveLast()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestMoveFirst()
    'MoveFirst()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestClearData()
    'ClearData()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestClear()
    'Clear()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_FileName()
    'set_FileName()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_FileName()
    'get_FileName()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Value()
    'get_Value()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Value()
    'set_Value()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_NumRecords()
    'get_NumRecords()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_NumRecords()
    'set_NumRecords()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_NumFields()
    'get_NumFields()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_NumFields()
    'set_NumFields()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_FieldType()
    'get_FieldType()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_FieldType()
    'set_FieldType()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_FieldName()
    'set_FieldName()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_FieldName()
    'get_FieldName()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_FieldLength()
    'set_FieldLength()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_FieldLength()
    'get_FieldLength()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_CurrentRecord()
    'get_CurrentRecord()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_CurrentRecord()
    'set_CurrentRecord()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_atEOF()
    'get_atEOF()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_atEOF()
    'set_atEOF()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_atBOF()
    'set_atBOF()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_atBOF()
    'get_atBOF()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSummaryFile()
    'SummaryFile()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSummaryFields()
    'SummaryFields()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSummary()
    'Summary()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestFindNext()
    'FindNext()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestFindFirst()
    'FindFirst()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestFieldNumber()
    'FieldNumber()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestWriteFile()
    'WriteFile()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestOpenFile()
    'OpenFile()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestCreationCode()
    'CreationCode()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestCousin()
    'Cousin()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestCurrentRecordAsDelimitedString()
    'CurrentRecordAsDelimitedString()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetHashCode()
    'GetHashCode()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestEquals()
    'not applicable?
  End Sub

  Public Sub TestToString()
    'ToString()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetType()
    'GetType()
    Assert.Ignore("Test not yet written")
  End Sub

End Class

<TestFixture()> Public Class Test_clsATCTableDBF

  Public Sub TestMovePrevious()
    'MovePrevious()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestMoveNext()
    'MoveNext()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestMoveLast()
    'MoveLast()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestMoveFirst()
    'MoveFirst()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestClearData()
    'ClearData()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestClear()
    'Clear()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_FileName()
    'set_FileName()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_FileName()
    'get_FileName()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Value()
    'get_Value()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Value()
    'set_Value()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_NumRecords()
    'get_NumRecords()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_NumRecords()
    'set_NumRecords()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_NumFields()
    'get_NumFields()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_NumFields()
    'set_NumFields()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_FieldType()
    'get_FieldType()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_FieldType()
    'set_FieldType()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_FieldName()
    'set_FieldName()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_FieldName()
    'get_FieldName()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_FieldLength()
    'set_FieldLength()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_FieldLength()
    'get_FieldLength()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_CurrentRecord()
    'get_CurrentRecord()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_CurrentRecord()
    'set_CurrentRecord()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_atEOF()
    'get_atEOF()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_atEOF()
    'set_atEOF()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_atBOF()
    'set_atBOF()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_atBOF()
    'get_atBOF()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSummaryFile()
    'SummaryFile()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSummaryFields()
    'SummaryFields()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSummary()
    'Summary()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestFindNext()
    'FindNext()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestFindFirst()
    'FindFirst()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestFieldNumber()
    'FieldNumber()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestWriteFile()
    'WriteFile()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestOpenFile()
    'OpenFile()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestCreationCode()
    'CreationCode()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestCousin()
    'Cousin()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestCurrentRecordAsDelimitedString()
    'CurrentRecordAsDelimitedString()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetHashCode()
    'GetHashCode()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestEquals()
    'not applicable?
  End Sub

  Public Sub TestToString()
    'ToString()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_FieldDecimalCount()
    'get_FieldDecimalCount()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_FieldDecimalCount()
    'set_FieldDecimalCount()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Logger()
    'set_Logger()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestFindMatch()
    'FindMatch()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestInitData()
    'InitData()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetType()
    'GetType()
    Assert.Ignore("Test not yet written")
  End Sub

End Class

<TestFixture()> Public Class Test_clsFieldDescriptor

  Public Sub TestGetHashCode()
    'GetHashCode()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestEquals()
    'Equals()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestToString()
    'ToString()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestReadFromFile()
    'ReadFromFile()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestWriteToFile()
    'WriteToFile()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetType()
    'GetType()
    Assert.Ignore("Test not yet written")
  End Sub

End Class

<TestFixture()> Public Class Test_clsDownload
  'TODO - write tests AFTER class completed, talk to Mark b4 any effort
End Class

<TestFixture()> Public Class Test_clsHeader

  Public Sub TestGetHashCode()
    'GetHashCode()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestEquals()
    'Equals()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestToString()
    'ToString()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestReadFromFile()
    'ReadFromFile()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestWriteToFile()
    'WriteToFile()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetType()
    'GetType()
    Assert.Ignore("Test not yet written")
  End Sub

End Class

<TestFixture()> Public Class Test_IATCTable

  Public Sub TestMovePrevious()
    'MovePrevious()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestMoveNext()
    'MoveNext()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestMoveLast()
    'MoveLast()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestMoveFirst()
    'MoveFirst()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestClearData()
    'ClearData()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestClear()
    'Clear()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_FileName()
    'set_FileName()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_FileName()
    'get_FileName()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Value()
    'get_Value()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Value()
    'set_Value()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_NumRecords()
    'get_NumRecords()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_NumRecords()
    'set_NumRecords()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_NumFields()
    'get_NumFields()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_NumFields()
    'set_NumFields()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_FieldType()
    'get_FieldType()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_FieldType()
    'set_FieldType()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_FieldName()
    'set_FieldName()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_FieldName()
    'get_FieldName()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_FieldLength()
    'set_FieldLength()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_FieldLength()
    'get_FieldLength()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_CurrentRecord()
    'get_CurrentRecord()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_CurrentRecord()
    'set_CurrentRecord()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_atEOF()
    'get_atEOF()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_atEOF()
    'set_atEOF()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_atBOF()
    'set_atBOF()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_atBOF()
    'get_atBOF()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSummaryFile()
    'SummaryFile()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSummaryFields()
    'SummaryFields()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSummary()
    'Summary()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestFindNext()
    'FindNext()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestFindFirst()
    'FindFirst()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestFieldNumber()
    'FieldNumber()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestWriteFile()
    'WriteFile()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestOpenFile()
    'OpenFile()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestCreationCode()
    'CreationCode()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestCousin()
    'Cousin()
    Assert.Ignore("Test not yet written")
  End Sub

End Class

<TestFixture()> Public Class Test_modDate
  Private d(6) As Integer
  Private dS As String
  Private dJ As Double = 38491.4326388889
  Private dX As Double = 0.00001
  Private dVB As Date

  <TestFixtureSetUp()> Public Sub init()
    d(0) = 2005
    d(1) = 5
    d(2) = 19
    d(3) = 10
    d(4) = 23
    d(5) = 0
    dS = Format(d(0), "0000") & "/" & Format(d(1), "00") & "/" & _
         Format(d(2), "00") & " " & Format(d(3), "00") & ":" & _
         Format(d(4), "00")
    dVB = New Date(d(0), d(1), d(2), d(3), d(4), d(5))
  End Sub

  Public Sub TestJ2DateRoundup()
    'J2DateRoundup()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestCalcTimeUnitStep()
    'CalcTimeUnitStep()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestVBdate2MJD()
    Assert.AreEqual(dJ, VBdate2MJD(dVB), dX)
  End Sub

  Public Sub TestMJD2VBdate()
    Assert.AreEqual(dVB.Ticks, MJD2VBdate(dJ).Ticks)
  End Sub

  Public Sub TestATCformat()
    'ATCformat()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestDate2J()
    Assert.AreEqual(dJ, Date2J(d), dX)
  End Sub

  Public Sub TestHMS2J()
    Assert.AreEqual(dJ Mod 1, HMS2J(d(3), d(4), d(5)), dX)
  End Sub

  Public Sub TestJ2Date()
    Dim lD(6) As Integer

    J2Date(dJ, lD)
    Assert.AreEqual(d, lD)
  End Sub

  Public Sub TestJ2HMS()
    Dim lH, lM, lS As Integer
    Dim lF As Double

    J2HMS(dJ, lH, lM, lS, lF)
    Assert.AreEqual(d(3), lH, "Fail Hour")
    Assert.AreEqual(d(4), lM, "Fail Minute")
    Assert.AreEqual(d(5), lS, "Fail Second")
  End Sub

  Public Sub TestINVMJD()
    Dim lY, lM, lD As Integer

    INVMJD(dJ, lY, lM, lD)
    Assert.AreEqual(d(0), lY, "Fail Year")
    Assert.AreEqual(d(1), lM, "Fail Month")
    Assert.AreEqual(d(2), lD, "Fail Day")
  End Sub

  Public Sub TestMJD()
    Assert.AreEqual(Fix(dJ), CDbl(MJD(d(0), d(1), d(2))))
  End Sub

  Public Sub TestJday()
    Assert.AreEqual(dJ, Jday(d(0), d(1), d(2), d(3), d(4), d(5)), dX)
  End Sub

  Public Sub TestJDateIntrvl()
    Assert.AreEqual(5, JDateIntrvl(dJ))
  End Sub

  Public Sub TestDateIntrvl()
    Assert.AreEqual(5, DateIntrvl(d))
  End Sub

  Public Sub Testdaymon()
    Assert.AreEqual(31, daymon(d(0), d(1)))
  End Sub

  Public Sub TestaddUniqueDate()
    Dim lJ(6) As Double, lI(6) As Integer
    Assert.AreEqual(True, addUniqueDate(dJ, lJ, lI))
    Assert.AreEqual(False, addUniqueDate(dJ, lJ, lI))
    Assert.AreEqual(True, addUniqueDate(dJ + 1, lJ, lI))
    Assert.AreEqual(True, addUniqueDate(dJ + 2, lJ, lI))
  End Sub

  Public Sub TestDumpDate()
    'DumpDate()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestDTMCMN()
    'DTMCMN()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestDatCmn()
    'DatCmn()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testtimcnv()
    'timcnv()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestTimAddJ()
    'TimAddJ()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestTIMADD()
    'TIMADD()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testtimdif()
    'timdif()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TesttimdifJ()
    'timdifJ()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetHashCode()
    Assert.AreEqual(GetHashCode, GetHashCode)  'dummy test
  End Sub

  Public Sub TestEquals()
    'not applicable for module, dummy follows
    Assert.AreEqual(True, Equals(Me))
  End Sub

  Public Sub TestToString()
    'ToString()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetType()
    'not applicable
  End Sub

End Class

<TestFixture()> Public Class Test_modFile
  Private Structure TestData
    Dim strDrive As String
    Dim strPath As String
    Dim strName As String
    Dim strExt As String
    Dim strFile As String
    Dim strFull As String
  End Structure
  Private myTest(0) As TestData

  <TestFixtureSetUp()> Public Sub init()
    With myTest(0)
      .strDrive = "c:"
      .strPath = .strDrive & "\test\atcUtility"
      .strName = "foo"
      .strExt = "txt"
      .strFile = .strName & "." & .strExt
      .strFull = .strPath & "\" & .strFile
    End With
    'TODO - add more tests with ugly strings, etc
  End Sub

  <TestFixtureTearDown()> Public Sub dispose()
    'MsgBox("End stest of modFile")
    'TODO - save results here?
  End Sub

  Public Sub TestFindFileFilterIndex()
    'FindFileFilterIndex()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestChDriveDir()
    For Each lTest As TestData In myTest
      With lTest
        Assert.IsTrue(ChDriveDir(.strDrive & "\"))
        Assert.AreEqual(CurDir, .strDrive & "\")
        Assert.IsTrue(ChDriveDir(.strPath))
        Assert.AreEqual(CurDir, .strPath)
      End With
    Next
  End Sub

  Public Sub TestFilenameOnly()
    For Each lTest As TestData In myTest
      With lTest
        Assert.AreEqual(FilenameOnly(.strFull), .strName)
      End With
    Next
  End Sub

  Public Sub TestFilenameNoPath()
    For Each lTest As TestData In myTest
      With lTest
        Assert.AreEqual(FilenameNoPath(.strFull), .strFile)
      End With
    Next
  End Sub

  Public Sub TestFilenameNoExt()
    For Each lTest As TestData In myTest
      With lTest
        Assert.AreEqual(FilenameNoExt(.strFull), .strPath & "\" & .strName)
      End With
    Next
  End Sub

  Public Sub TestFileExt()
    For Each lTest As TestData In myTest
      With lTest
        Assert.AreEqual(FileExt(.strFull), .strExt)
      End With
    Next
  End Sub

  Public Sub TestPathNameOnly()
    For Each lTest As TestData In myTest
      With lTest
        Assert.AreEqual(PathNameOnly(.strFull), .strPath)
      End With
    Next
  End Sub

  Public Sub TestFilenameSetExt()
    For Each lTest As TestData In myTest
      With lTest
        Assert.AreEqual(FilenameSetExt(.strFull, "png"), .strPath & "\" & .strName & ".png")
        'TODO - improve next test.
        Assert.AreEqual(FilenameSetExt("C:\foo\bardtxt", "png"), "C:\foo\bardtxt.png")
      End With
    Next
  End Sub

  Public Sub TestFindFileFilter()
    Assert.AreEqual("WDM Files (*.wdm)|*.wdm", FindFileFilter("WDM Files (*.wdm)|*.wdm|All Files (*.*)|*.*", 1))
  End Sub

  Public Sub TestAbsolutePath()
    Assert.AreEqual(AbsolutePath("..\Data\DataFile.wdm", "C:\BASINS\models"), "C:\BASINS\Data\DataFile.wdm")
  End Sub

  Public Sub TestRelativeFilename()
    Assert.AreEqual(RelativeFilename("c:\BASINS\Data\DataFile.wdm", "c:\BASINS"), "Data\DataFile.wdm")
    Assert.AreEqual(RelativeFilename("c:\BASINS\OtherData\DataFile.wdm", "c:\BASINS\Data"), "..\OtherData\DataFile.wdm")
  End Sub

  Public Sub TestMkDirPath()
    Dim lPath As String = "c:\test\atcUtility\dummy\dummy"
    MkDirPath(lPath)
    Assert.AreEqual(True, FileExists(lPath, True))
    RmDir(lPath)
    RmDir(Left(lPath, Len(lPath) - 6))
  End Sub

  Public Sub TestReplaceStringToFile()
    Dim lf As String = "c:\test\atcUtility\data\stringToFile.txt"
    ReplaceStringToFile("He left", "He", "She", lf)
    Assert.AreEqual("She left", WholeFileString(lf))
    Kill(lf)
  End Sub

  Public Sub TestFirstMismatch()
    ' ##SUMMARY Compares 2 files and locates first sequential byte that is different between files.
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSaveFileString()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSaveFileBytes()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestAppendFileString()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestFileExists()

    Assert.Ignore("Test not yet written")
  End Sub

  'Open a file using the default method the system would have used if it was double-clicked
  Public Sub TestOpenFile()
    'Use a .NET process() to launch the file or URL
    'TODO: wait for newProcess to finish

    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestFileToBase64()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestAddFilesInDir()
    'AddFilesInDir()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestRemoveFilesInDir()
    'RemoveFilesInDir()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestFindFile()
    'FindFile()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetHashCode()
    Assert.AreEqual(GetHashCode, GetHashCode)  'dummy test
  End Sub

  Public Sub TestEquals()
    'not applicable for module, dummy follows
    Assert.AreEqual(True, Equals(Me))
  End Sub

  Public Sub TestToString()
    'ToString()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetType()
    'not applicable
  End Sub

End Class

<TestFixture()> Public Class Test_modLog
  'TODO - write tests AFTER module completed, talk to Mark b4 any effort

  Public Sub TestLogDbg()
    'LogDbg()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestLogMsg()
    'LogMsg()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestLogCmd()
    'LogCmd()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSendFeedback()
    'SendFeedback()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetHashCode()
    Assert.AreEqual(GetHashCode, GetHashCode)  'dummy test
  End Sub

  Public Sub TestEquals()
    'not applicable for module, dummy follows
    Assert.AreEqual(True, Equals(Me))
  End Sub

  Public Sub TestToString()
    'ToString()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetType()
    'not applicable
  End Sub

End Class

<TestFixture()> Public Class Test_modScript
  Public Sub TestRunScript()
    'RunScript()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestCompileScript()
    'CompileScript()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetHashCode()
    Assert.AreEqual(GetHashCode, GetHashCode)  'dummy test
  End Sub

  Public Sub TestEquals()
    'not applicable for module, dummy follows
    Assert.AreEqual(True, Equals(Me))
  End Sub

  Public Sub TestToString()
    'ToString()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetType()
    'not applicable
  End Sub

End Class

<TestFixture()> Public Class Test_modString
  <TestFixtureSetUp()> Public Sub init()
    'MsgBox("Start test of modString")
  End Sub

  <TestFixtureTearDown()> Public Sub dispose()
    'MsgBox("End stest of modString")
  End Sub

  Public Sub TestSignificantDigits()
    Assert.AreEqual(220, SignificantDigits(224, 2))
    Assert.AreEqual(230, SignificantDigits(225, 2))
    Assert.AreEqual(225, SignificantDigits(225.4, 3))
    Assert.AreEqual(226, SignificantDigits(225.5, 3))
    Assert.AreEqual(34.542, SignificantDigits(34.542493456, 5))
    Assert.AreEqual(3.4542, SignificantDigits(3.4542493456, 5))
    Assert.AreEqual(-3.4542, SignificantDigits(-3.4542493456, 5))
    Assert.AreEqual(0.34542, SignificantDigits(0.34542493456, 5))
    Assert.AreEqual(0.034542, SignificantDigits(0.034542493456, 5))
  End Sub

  Public Sub TestDoubleToString()
    Assert.AreEqual("220", DoubleToString(224, 4, , , , 2))
  End Sub

  Public Sub TestCountString()
    Assert.AreEqual(2, CountString("HiHixx", "Hi"))
  End Sub

  Public Sub TestLog10()
    Assert.AreEqual(1, Log10(10))
    Assert.AreEqual(1, Log10(0))
  End Sub
  Public Sub TestIsInteger()
    Assert.IsTrue(IsInteger("12345"))
    Assert.IsFalse(IsInteger("123.45"))
  End Sub

  Public Sub TestIsAlpha()
    Assert.IsTrue(IsAlpha("abcde"))
    Assert.IsFalse(IsAlpha("abc123"))
  End Sub

  Public Sub TestIsAlphaNumeric()
    Assert.IsTrue(IsAlphaNumeric("abc123"))
    Assert.IsFalse(IsAlphaNumeric("abc123!"))
  End Sub

  Public Sub TestByteIsPrintable()
    Dim b As Short 'Byte type overflows at end of For loop
    For b = 0 To 255
      Select Case b
        Case 9, 10, 12, 13 : Assert.IsTrue(ByteIsPrintable(b))
        Case Is < 32 : Assert.IsFalse(ByteIsPrintable(ByteIsPrintable(b)))
        Case Is < 127 : Assert.IsTrue(ByteIsPrintable(b))
        Case Else : Assert.IsFalse((ByteIsPrintable(ByteIsPrintable(b))))
      End Select
    Next
  End Sub

  Public Sub TestRndlow()
    Dim d1 As Double = 1.1
    Dim d2 As Double = 1

    Assert.AreEqual(0, Rndlow(1.0E-20))

    While d1 < 10000000
      Assert.AreEqual(d2, Rndlow(d1))
      d1 = d1 * 10
      d2 = d2 * 10
    End While
  End Sub

  Public Sub TestScalit()
    Dim lMin, lMax As Single

    Scalit(1, 2.3, 4.5, lMin, lMax)
    Assert.AreEqual(2, lMin, "Fail Min")
    Assert.AreEqual(5, lMax, "Fail Max")

    Scalit(1, -2.3, 45, lMin, lMax)
    Assert.AreEqual(-4, lMin, "Fail Min")
    Assert.AreEqual(50, lMax, "Fail Max")

    Scalit(2, 2.3, 4.5, lMin, lMax)
    Assert.AreEqual(1, lMin, "Fail Min")
    Assert.AreEqual(10, lMax, "Fail Max")

    Scalit(2, 2.3, 455, lMin, lMax)
    Assert.AreEqual(1, lMin, "Fail Min")
    Assert.AreEqual(1000, lMax, "Fail Max")
  End Sub

  Public Sub TestFirstStringPos()
    Dim i As Integer

    i = FirstStringPos(1, "newFoo", "hiThere", "newFoo", "endString")
    Assert.AreEqual(1, i)
  End Sub

  Public Sub TestFirstCharPos()
    'FirstCharPos()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestStrNoNull()
    Assert.AreEqual("NotNull", StrNoNull("NotNull"))
    Assert.AreEqual(" N o t N u l l ", StrNoNull(" N o t N u l l "))
    Assert.AreEqual(" ", StrNoNull(""))
  End Sub

  Public Sub TestStrPad()
    Assert.AreEqual("fooXX", StrPad("foo", 5, "X", False))
    Assert.AreEqual("XXfoo", StrPad("foo", 5, "X", True))
    Assert.AreEqual("XXfoo", StrPad("foo", 5, "X"))
    Assert.AreEqual("foo", StrPad("foo", 3, "X"))
    Assert.AreEqual("foo", StrPad("foo", 0, "X"))
    Assert.AreEqual("foo", StrPad("foo", -1, "X"))
  End Sub

  Public Sub TestStrSplit()
    Dim str As String

    str = "Julie, Todd, Jane, and Ray"
    Assert.AreEqual("Julie", StrSplit(str, ",", ""))
    Assert.AreEqual("Todd, Jane, and Ray", str)

    str = "Julie, Todd, Jane, and Ray"
    Assert.AreEqual("Julie, Todd, Jane,", StrSplit(str, "and", ""))
    Assert.AreEqual("Ray", str)

    str = "'Smith, Julie', 'Collins, Todd', Jane, and Ray"
    Assert.AreEqual("Smith, Julie", StrSplit(str, ",", "'"))
    Assert.AreEqual("'Collins, Todd', Jane, and Ray", str)

  End Sub

  Public Sub TestReplaceStringNoCase()
    Assert.AreEqual("She came and She went", ReplaceStringNoCase("He came and he went", "He", "She"))
  End Sub

  Public Sub TestReplaceString()
    Assert.AreEqual("She came and he went", ReplaceString("He came and he went", "He", "She"))
    Assert.AreEqual("She came and She went", ReplaceString("He came and He went", "He", "She"))
  End Sub

  Public Sub TestStrPrintable()
    Dim lS As String
    For i As Integer = 0 To 255
      lS = Chr(i)
      Select Case i
        Case 0 : Assert.AreEqual("", StrPrintable(lS, "X"))
        Case 32 To 126 : Assert.AreEqual(lS, StrPrintable(lS, "X"))
        Case Else : Assert.AreEqual("X", StrPrintable(lS, "X"))
      End Select
    Next
  End Sub

  Public Sub TestStrSafeFilename()
    Assert.AreEqual("filename", StrSafeFilename("file" & Chr(1) & "name", ""))
    Assert.AreEqual("file", StrSafeFilename("file" & Chr(0) & "name", ""))
  End Sub

  Public Sub TestSwapBytes()
    Assert.AreEqual(&H1000000, SwapBytes(1))
    Assert.AreEqual(1, SwapBytes(&H1000000))
  End Sub

  Public Sub TestReadBigInt()
    Dim lF As String = "c:\test\atcUtility\data\BigInt.txt"
    Dim lFun As Integer = 1
    Dim lByte As Byte()
    Dim lValue As Integer = 1

    lByte = System.BitConverter.GetBytes(lValue)
    SaveFileBytes(lF, lByte)
    FileOpen(lFun, lF, OpenMode.Binary)
    Assert.AreEqual(&H1000000, ReadBigInt(lFun))
    FileClose(lFun)
    Kill(lF)
  End Sub

  Public Sub TestWriteBigInt()
    Dim lF As String = "c:\test\atcUtility\data\BigInt.txt"
    Dim lFun As Integer = 1
    Dim lByte As Byte()
    Dim lValue As Integer = 1

    lByte = System.BitConverter.GetBytes(lValue)
    FileOpen(lFun, lF, OpenMode.Binary)
    WriteBigInt(lFun, lValue)
    FileClose(lFun)
    lByte = WholeFileBytes(lF)
    Assert.AreEqual(3, UBound(lByte))
    Assert.AreEqual(&H1000000, System.BitConverter.ToInt32(lByte, 0))
    Kill(lF)
  End Sub

  Public Sub TestByte2String()
    Dim lByte As Byte()
    Dim lC As Char = "t"

    lByte = System.BitConverter.GetBytes(lC)
    Assert.AreEqual("t", Byte2String(lByte, 0, 1))
  End Sub

  Public Sub TestLong2Single()
    Assert.AreEqual(0.004723787, Long2Single(999999999), 0.00001)
  End Sub

  Public Sub TestLong2String()
    Assert.AreEqual("b" & Chr(0) & Chr(0) & Chr(0), Long2String(98))
  End Sub

  Public Sub TestWholeFileString()
    'WholeFileString()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestWholeFileBytes()
    'WholeFileBytes()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestFirstMismatch()
    'FirstMismatch()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestPatternMatch()
    Assert.IsTrue(PatternMatch("He left", "He*"))
    Assert.IsFalse(PatternMatch("He left", "left*"))
  End Sub

  Public Sub TestFileToBase64()
    'FileToBase64()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestStrRetRem()
    'StrRetRem()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetHashCode()
    Assert.AreEqual(GetHashCode, GetHashCode)  'dummy test
  End Sub

  Public Sub TestEquals()
    'not applicable for module, dummy follows
    Assert.AreEqual(True, Equals(Me))
  End Sub

  Public Sub TestToString()
    'ToString()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetType()
    'not applicable
  End Sub

End Class

<TestFixture()> Public Class Test_modUnits

  Public Sub TestGetParameterUnits()
    'GetParameterUnits()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetConversionFactor()
    'GetConversionFactor()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetUnitDescription()
    'GetUnitDescription()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetUnitID()
    'GetUnitID()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetUnitName()
    'GetUnitName()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetUnitCategory()
    'GetUnitCategory()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetAllUnitsInCategory()
    'GetAllUnitsInCategory()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetAllUnitCategories()
    'GetAllUnitCategories()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetHashCode()
    Assert.AreEqual(GetHashCode, GetHashCode)  'dummy test
  End Sub

  Public Sub TestEquals()
    'not applicable for module, dummy follows
    Assert.AreEqual(True, Equals(Me))
  End Sub

  Public Sub TestToString()
    'ToString()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetType()
    'not applicable
  End Sub

End Class

<TestFixture()> Public Class Test_TableOpener

  Public Sub TestOpenAnyTable()
    'OpenAnyTable()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetHashCode()
    'GetHashCode()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestEquals()
    'Equals()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestToString()
    Assert.AreEqual(ToString().ToLower, "atcutility.test_tableopener")
  End Sub

  Public Sub TestGetType()
    'not applicable
  End Sub

End Class

<TestFixture()> Public Class Test_UtilColor

  <TestFixtureSetUp()> Public Sub init()
    InitMatchingColors("c:\test\atcutility\data\graphcolors.txt")
  End Sub

  Public Sub TestInitMatchingColors()
    Try
      InitMatchingColors("c:\test\atcutility\data\graphcolors.txt")
    Catch
      Assert.Fail(Err.Description)
    End Try
  End Sub

  Public Sub TestGetMatchingColor()
    Assert.AreEqual(Color.Blue.ToArgb, GetMatchingColor("OBSERVED::").ToArgb, "Fail to Match OBSERVED Blue")
    Assert.AreEqual(Color.Red.ToArgb, GetMatchingColor("SIMULATED::").ToArgb, "Fail to Match SIMULATED Red")
  End Sub

  Public Sub TestTextOrNumericColor()
    Dim lRed As Color = Color.Red
    Dim lGot As Color
    Assert.AreEqual(lRed.ToArgb, TextOrNumericColor("red").ToArgb, "Fail to Match Red")
    Assert.AreEqual(lRed.ToArgb, TextOrNumericColor(lRed.ToArgb).ToArgb, "Fail to Match Red as ARGB")
    Assert.AreEqual(Color.Green.ToArgb, TextOrNumericColor("green").ToArgb, "Fail to Match Green")
    Assert.AreEqual(Color.Blue.ToArgb, TextOrNumericColor("blue").ToArgb, "Fail to Match Blue")
  End Sub

  Public Sub TestcolorName()
    Assert.AreEqual("red", colorName(Color.Red).ToLower, "Fail to Match Red")
    Assert.AreEqual("cyan", colorName(Color.Cyan).ToLower, "Fail to Match Cyan")
  End Sub

  Public Sub TestGetHashCode()
    Assert.AreEqual(GetHashCode, GetHashCode)  'dummy test
  End Sub

  Public Sub TestEquals()
    'not applicable for module, dummy follows
    Assert.AreEqual(True, Equals(Me))
  End Sub

  Public Sub TestToString()
    'not applicable for module, dummy follows
    Assert.AreEqual("atcutility.test_utilcolor", ToString().ToLower)
  End Sub

  Public Sub TestGetType()
    'not applicable
  End Sub

End Class

<TestFixture()> Public Class Test_atcCollection

  Public Sub TestGetEnumerator()
    'GetEnumerator()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetHashCode()
    'GetHashCode()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestEquals()
    'Equals()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestToString()
    'ToString()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestAdd()
    'Add()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestClear()
    'Clear()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Count()
    'get_Count()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestItemByIndex()
    'ItemByIndex()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestIndexFromKey()
    'IndexFromKey()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestIndexFromValue()
    'IndexFromValue()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestKeyByIndex()
    'KeyByIndex()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestRemoveByIndex()
    'RemoveByIndex()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestRemoveByKey()
    'RemoveByKey()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestRemoveByValue()
    'RemoveByValue()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetType()
    'GetType()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_ItemByIndex()
    'set_ItemByIndex()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_ItemByIndex()
    'get_ItemByIndex()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestClone()
    'Clone()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_IsSynchronized()
    'get_IsSynchronized()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_SyncRoot()
    'get_SyncRoot()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestCopyTo()
    'CopyTo()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestRemoveAt()
    'RemoveAt()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestRemove()
    'Remove()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestInsert()
    'Insert()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestIndexOf()
    'IndexOf()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_IsFixedSize()
    'get_IsFixedSize()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_IsReadOnly()
    'get_IsReadOnly()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestContains()
    'Contains()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Item()
    'set_Item()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Item()
    'get_Item()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestTrimToSize()
    'TrimToSize()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestToArray()
    'ToArray()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSort()
    'Sort()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetRange()
    'GetRange()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSetRange()
    'SetRange()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestReverse()
    'Reverse()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestRemoveRange()
    'RemoveRange()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestLastIndexOf()
    'LastIndexOf()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestInsertRange()
    'InsertRange()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestBinarySearch()
    'BinarySearch()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestAddRange()
    'AddRange()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Capacity()
    'set_Capacity()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Capacity()
    'get_Capacity()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Keys()
    'get_Keys()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Keys()
    'set_Keys()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_ItemByKey()
    'get_ItemByKey()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_ItemByKey()
    'set_ItemByKey()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestChangeTo()
    'ChangeTo()
    Assert.Ignore("Test not yet written")
  End Sub

End Class
