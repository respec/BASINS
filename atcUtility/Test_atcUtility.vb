Option Strict Off
Option Explicit On 
'Copyright 2005 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Imports NUnit.Framework
Imports atcUtility.modReflection
 
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
  Private dJ As Double = 53509.43264
  Private dX As Double = 0.00001

  <TestFixtureSetUp()> Public Sub init()
    d(0) = 2005
    d(1) = 5
    d(2) = 19
    d(3) = 10
    d(4) = 23
    d(5) = 0
    dS = Format(d(0), "0000") & "/" & Format(d(1), "00") & "/19 10:23"
  End Sub

  Public Sub TestVBdate2MJD()
    Assert.AreEqual(VBdate2MJD(dS), dJ, dX)
  End Sub

  Public Sub TestMJD2VBdate()
    Assert.AreEqual(Format(MJD2VBdate(dJ), "yyyy/MM/dd hh:mm"), dS)
  End Sub

  Public Sub TestATCformat()
    'ATCformat()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestDate2J()
    Assert.AreEqual(Date2J(d), dJ, dX)
  End Sub

  Public Sub TestHMS2J()
    Assert.AreEqual(HMS2J(d(3), d(4), d(5)), dJ Mod 1, dX)
  End Sub

  Public Sub TestJ2Date()
    'J2Date()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestJ2HMS()
    'J2HMS()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestINVMJD()
    'INVMJD()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestMJD()
    'MJD()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestJday()
    'Jday()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestJDateIntrvl()
    'JDateIntrvl()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestDateIntrvl()
    'DateIntrvl()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testdaymon()
    'daymon()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestaddUniqueDate()
    'addUniqueDate()
    Assert.Ignore("Test not yet written")
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

  Public Sub TestGetType()
    'GetType()
    Assert.Ignore("Test not yet written")
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

  Public Sub TestAbsolutePath()
    ' ##SUMMARY Converts an relative pathname to an absolute path given the starting directory.
    ' ##SUMMARY   Example: AbsolutePath("..\Data\DataFile.wdm", "C:\BASINS\models") = "C:\BASINS\Data\DataFile.wdm"
    ' ##PARAM StartPath I Relative file path and name.
    ' ##PARAM Filename I Absolute starting directory from which relative path is traced.
    ' ##RETURNS Absolute path and filename.
    Assert.AreEqual(AbsolutePath("..\Data\DataFile.wdm", "C:\BASINS\models"), "C:\BASINS\Data\DataFile.wdm")
  End Sub

  Public Sub TestRelativeFilename()
    ' ##SUMMARY Converts an absolute pathname to a relative path given the starting directory.
    ' ##SUMMARY If Filename is not on the same drive as StartPath, Filename is returned.
    ' ##SUMMARY   Example: RelativeFilename("c:\BASINS\Data\DataFile.wdm", "c:\BASINS") = "Data\DataFile.wdm"
    ' ##SUMMARY   Example: RelativeFilename("c:\BASINS\OtherData\DataFile.wdm", "c:\BASINS\Data") = "..\OtherData\DataFile.wdm"
    Assert.AreEqual(RelativeFilename("c:\BASINS\Data\DataFile.wdm", "c:\BASINS"), "Data\DataFile.wdm")
    Assert.AreEqual(RelativeFilename("c:\BASINS\OtherData\DataFile.wdm", "c:\BASINS\Data"), "..\OtherData\DataFile.wdm")
  End Sub

  Public Sub TestMkDirPath()
    ' ##SUMMARY Makes the specified directory and any above it that are not yet there.
    ' ##SUMMARY   Example: MkDirPath("C:\foo\bar") creates the "C:\foo" and "C:\foo\bar" directories if they do not already exist.
    ' ##PARAM newPath I Path to specified directory
    ' ##LOCAL UpPath - parent directory of newPath

    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestScalit()
    ' ##SUMMARY Determines an appropriate scale based on the _
    'minimum and maximum values and whether an arithmetic, probability, _
    'or logarithmic scale is requested. Minimum and maximum for probability _
    'plots must be standard deviates. For log scales, the minimum _
    'and maximum must not be transformed.
    Assert.Ignore("Test not yet written")
  End Sub


  Public Sub TestFirstStringPos()
    ' ##SUMMARY Searches Source for each item in SearchFor array.
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestTestFirstCharPos()
    ' ##SUMMARY Searches str for each character in chars.
    ' ##PARAM start I Position in str to start search
    ' ##PARAM str I String to be searched
    ' ##PARAM chars I String of characters to be individually searched for
    ' ##RETURNS  Position of first occurrence of chars character in Source. _
    'Returns len(str) + 1 if no characters from chars were found in Source.

    Assert.Ignore("Test not yet written")

  End Sub

  Public Sub TestStrNoNull()
    ' ##SUMMARY Replaces null string with blank character.
    ' ##SUMMARY   Example: StrNoNull("NotNull") = "NotNull"
    ' ##SUMMARY   Example: StrNoNull("") = " "
    ' ##PARAM s I String to be analyzed
    ' ##RETURNS  Returns a space character if string is empty. _
    'Returns incoming string otherwise.

    Assert.AreEqual(StrNoNull("NotNull"), "NotNull")
    Assert.AreEqual(StrNoNull(" N o t N u l l "), " N o t N u l l ")
    Assert.AreEqual(StrNoNull(""), " ")
  End Sub

  Public Sub TestReplaceStringNoCase()
    ' ##SUMMARY Replaces Find in Source with Replace (not case sensitive).
    ' ##SUMMARY Example: ReplaceStringNoCase("He came and he went", "He", "She") = "She came and She went"
    Assert.AreEqual(ReplaceStringNoCase("He came and he went", "He", "She"), "She came and She went")
  End Sub

  Public Sub TestReplaceString()
    ' ##SUMMARY Replaces Find in Source with Replace (case sensitive).
    ' ##SUMMARY   Example: ReplaceString("He left", "He", "She") = "She left"
    'any occurences of Find (case sensitive) are replaced with Replace.

    Assert.AreEqual(ReplaceString("He came and he went", "He", "She"), "She came and he went")
    Assert.AreEqual(ReplaceString("He came and He went", "He", "She"), "She came and She went")
  End Sub

  Public Sub TestReplaceStringToFile()
    ' ##SUMMARY Saves new string like Source to Filename with _
    'occurences of Find in Source replaced with Replace.
    ' ##SUMMARY   Example: ReplaceString("He left", "He", "She") = "She left"

    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestStrPrintable()
    Dim str As String
    Dim i As Short 'loop counter
    For i = 0 To 255
      str = Chr(i)
      Select Case i
        Case 0 : Assert.AreEqual(StrPrintable(str, "X"), "")
        Case 32 To 126 : Assert.AreEqual(StrPrintable(str, "X"), str)
        Case Else : Assert.AreEqual(StrPrintable(str, "X"), "X")
      End Select
    Next
  End Sub

  Public Sub TestStrSafeFilename()
    ' ##SUMMARY Converts, if necessary, non-printable characters in filename to printable alternative.
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestStrPad()
    ' ##SUMMARY Pads a string with specific character to achieve a specified length.

    Assert.AreEqual(StrPad("foo", 5, "X", False), "fooXX")
    Assert.AreEqual(StrPad("foo", 5, "X", True), "XXfoo")
    Assert.AreEqual(StrPad("foo", 5, "X"), "XXfoo")
    Assert.AreEqual(StrPad("foo", 3, "X"), "foo")
    Assert.AreEqual(StrPad("foo", 0, "X"), "foo")
    Assert.AreEqual(StrPad("foo", -1, "X"), "foo")

  End Sub

  Public Sub TestStrSplit()
    ' ##SUMMARY Divides string into 2 portions at position of 1st occurence of specified _
    'delimeter. Quote specifies a particular string that is exempt from the delimeter search.
    ' ##SUMMARY   Example: StrSplit("Julie, Todd, Jane, and Ray", ",", "") = "Julie", and "Todd, Jane, and Ray" is returned as Source.
    ' ##SUMMARY   Example: StrSplit("Julie, Todd, Jane, and Ray", "and", "") = "Julie, Todd, Jane," and "Ray" is returned as Source.
    Dim str As String
    str = "Julie, Todd, Jane, and Ray"
    Assert.AreEqual(StrSplit(str, ",", ""), "Julie")
    Assert.AreEqual(str, "Todd, Jane, and Ray")

    str = "Julie, Todd, Jane, and Ray"
    Assert.AreEqual(StrSplit(str, "and", ""), "Julie, Todd, Jane,")
    Assert.AreEqual(str, "Ray")

    str = "'Smith, Julie', 'Collins, Todd', Jane, and Ray"
    Assert.AreEqual(StrSplit(str, ",", "'"), "Smith, Julie")
    Assert.AreEqual(str, "'Collins, Todd', Jane, and Ray")

  End Sub

  Public Sub TestSwapBytes()
    ' ##SUMMARY Swaps between big and little endian 32-bit integers.
    ' ##SUMMARY   Example: SwapBytes(1) = 16777216
    Assert.AreEqual(SwapBytes(1), 16777216)
    Assert.AreEqual(SwapBytes(16777216), 1)
  End Sub

  Public Sub TestReadBigInt()
    ' ##SUMMARY Reads big-endian integer from file number and converts to _
    'Intel little-endian value.
    ' ##SUMMARY   Example: ReadBigInt(1) = 1398893856

    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub WriteBigInt(ByRef OutFile As Short, ByRef Value As Integer)
    ' ##SUMMARY Writes 32-bit integer as big endian to specified disk file.
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestLong2String()
    ' ##SUMMARY Parses long integer to FourByteType then prints out corresponding ascii codes.
    Assert.AreEqual(Long2String(98), "b" & Chr(0) & Chr(0) & Chr(0))
  End Sub

  Public Sub TestByte2String()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestWholeFileString()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestWholeFileBytes()
    Assert.Ignore("Test not yet written")
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

  Public Sub TestPatternMatch()
    ' ##SUMMARY Searches string for presence of pattern.
    ' ##SUMMARY Example: PatternMatch("He left", "He*") = True
    ' ##SUMMARY Example: PatternMatch("He left", "left*") = False

    Assert.IsTrue(PatternMatch("He left", "He*"))
    Assert.IsFalse(PatternMatch("He left", "left*"))
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

  Public Sub TestGetType()
    'GetType()
    Assert.Ignore("Test not yet written")
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

  Public Sub TestGetType()
    'GetType()
    Assert.Ignore("Test not yet written")
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

  Public Sub TestGetType()
    'GetType()
    Assert.Ignore("Test not yet written")
  End Sub

End Class

<TestFixture()> Public Class Test_modString
  <TestFixtureSetUp()> Public Sub init()
    'MsgBox("Start test of modString")
  End Sub

  <TestFixtureTearDown()> Public Sub dispose()
    'MsgBox("End stest of modString")
  End Sub

  Public Sub TestLog10()
    ' ##SUMMARY Calculates the log 10 of a given number.
    ' ##SUMMARY   Example: Log10(218.7761624) = 2.34
    Assert.AreEqual(Log10(10), 1)
    Assert.AreEqual(Log10(0), 1)
  End Sub
  Public Sub TestIsInteger()
    ' ##SUMMARY Checks to see whether incoming string is an integer or not.
    ' ##SUMMARY Returns true if each character in string is in range [0-9].
    ' ##SUMMARY   Example: IsInteger(12345) = True
    ' ##SUMMARY   Example: IsInteger(123.45) = False
    Assert.IsTrue(IsInteger("12345"))
    Assert.IsFalse(IsInteger("123.45"))
  End Sub

  Public Sub TestIsAlpha()
    ' ##SUMMARY Checks to see whether incoming string is entirely alphabetic.
    ' ##SUMMARY   Example: IsAlpha(abcde) = True
    ' ##SUMMARY   Example: IsAlpha(abc123) = False
    Assert.IsTrue(IsAlpha("abcde"))
    Assert.IsFalse(IsAlpha("abc123"))
  End Sub

  Public Sub TestIsAlphaNumeric()
    ' ##SUMMARY Checks to see whether incoming string is entirely alphanumeric.
    ' ##SUMMARY   Example: IsAlphaNumeric(abc123) = True
    ' ##SUMMARY   Example: IsAlphaNumeric(#$*&!) = False
    Assert.IsTrue(IsAlphaNumeric("abc123"))
    Assert.IsFalse(IsAlphaNumeric("abc123!"))
  End Sub

  Public Sub TestByteIsPrintable()
    ' ##SUMMARY Checks to see whether incoming byte is printable.
    ' ##SUMMARY   Example: ByteIsPrintable(44) = True
    ' ##SUMMARY   Example: ByteIsPrintable(7) = False
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
    ' ##SUMMARY Sets values less than 1.0E-19 to 0.0 for the _
    'plotting routines for bug in DISSPLA/PR1ME. Otherwise returns values _
    'rounded to lower magnitude.
    ' ##SUMMARY   Example: Rndlow(1.0E-20) = 0
    ' ##SUMMARY   Example: Rndlow(11000) = 10000
    Dim d1 As Double = 1.1
    Dim d2 As Double = 1

    Assert.AreEqual(Rndlow(1.0E-20), 0)

    While d1 < 10000000
      Assert.AreEqual(Rndlow(d1), d2)
      d1 = d1 * 10
      d2 = d2 * 10
    End While
  End Sub

  Public Sub TestScalit()
    'Scalit()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestFirstStringPos()
    'FirstStringPos()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestFirstCharPos()
    'FirstCharPos()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestStrNoNull()
    'StrNoNull()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestStrSplit()
    'StrSplit()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestReplaceStringNoCase()
    'ReplaceStringNoCase()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestReplaceString()
    'ReplaceString()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestStrPrintable()
    'StrPrintable()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestStrSafeFilename()
    'StrSafeFilename()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestStrPad()
    'StrPad()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSwapBytes()
    'SwapBytes()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestReadBigInt()
    'ReadBigInt()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestWriteBigInt()
    'WriteBigInt()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestLong2String()
    'Long2String()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestLong2Single()
    'Long2Single()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestByte2String()
    'Byte2String()
    Assert.Ignore("Test not yet written")
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
    'PatternMatch()
    Assert.Ignore("Test not yet written")
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

  Public Sub TestGetType()
    'GetType()
    Assert.Ignore("Test not yet written")
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

  Public Sub TestGetType()
    'GetType()
    Assert.Ignore("Test not yet written")
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
    Assert.AreEqual(GetMatchingColor("OBSERVED::"), &HFF0000, "Fail to Match Blue")
  End Sub

  Public Sub TestTextOrNumericColor()
    Assert.AreEqual(TextOrNumericColor("red"), &HFF, "Fail to Match Red")
    Assert.AreEqual(TextOrNumericColor("255"), &HFF, "Fail to Match Red as 255")
    Assert.AreEqual(TextOrNumericColor("blue"), &HFF0000, "Fail to Match Blue")
    Assert.AreEqual(TextOrNumericColor("green"), &HFF00, "Fail to Match Green")
  End Sub

  Public Sub TestcolorName()
    Assert.AreEqual(colorName(&HFF).ToLower, "red", "Fail to Match Red")
    Assert.AreEqual(colorName(&HFFFF00).ToLower, "cyan", "Fail to Match Cyan")
  End Sub

  Public Sub TestGetHashCode()
    Assert.AreEqual(GetHashCode, GetHashCode)  'dummy test
  End Sub

  Public Sub TestEquals()
    'not applicable for module, dummy follows
    Assert.AreEqual(Equals(Me), True)
  End Sub

  Public Sub TestToString()
    'not applicable for module, dummy follows
    Assert.AreEqual(ToString().ToLower, "atcutility.test_utilcolor")
  End Sub

  Public Sub TestGetType()
    'not applicable
  End Sub

End Class
