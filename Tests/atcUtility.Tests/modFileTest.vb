Imports System.IO
Imports System.Collections
Imports System.Text
Imports System.Collections.Specialized
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports atcUtility

'''<summary>
'''This is a test class for modFileTest and is intended
'''to contain all modFileTest Unit Tests
'''</summary>
<TestClass()> _
Public Class modFileTest
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

    '''<summary>Test AbsolutePath</summary>
    <TestMethod()> Public Sub AbsolutePathTest()
        Dim aFileName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aStartPath As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = modFile.AbsolutePath(aFileName, aStartPath)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test AddFilesInDir</summary>
    <TestMethod()> Public Sub AddFilesInDirTest()
        Dim aFilenames As NameValueCollection = Nothing ' TODO: Initialize to an appropriate value
        Dim aFilenamesExpected As NameValueCollection = Nothing ' TODO: Initialize to an appropriate value
        Dim aDirName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aSubdirs As Boolean = False ' TODO: Initialize to an appropriate value
        Dim aFileFilter As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aAttributes As Integer = 0 ' TODO: Initialize to an appropriate value
        modFile.AddFilesInDir(aFilenames, aDirName, aSubdirs, aFileFilter, aAttributes)
        Assert.AreEqual(aFilenamesExpected, aFilenames)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test AppendFileString</summary>
    <TestMethod()> Public Sub AppendFileStringTest()
        Dim filename As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim appendString As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = modFile.AppendFileString(filename, appendString)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test ChDriveDir</summary>
    <TestMethod()> Public Sub ChDriveDirTest()
        Dim aPath As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = modFile.ChDriveDir(aPath)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test ConvertLongPathToShort</summary>
    <TestMethod()> Public Sub ConvertLongPathToShortTest()
        Dim aLongPathName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = modFile.ConvertLongPathToShort(aLongPathName)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test FileExt</summary>
    <TestMethod()> Public Sub FileExtTest()
        Dim aStr As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = modFile.FileExt(aStr)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test FileToBase64</summary>
    <TestMethod()> Public Sub FileToBase64Test()
        Dim InputFilePath As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim OutputFilePath As String = String.Empty ' TODO: Initialize to an appropriate value
        modFile.FileToBase64(InputFilePath, OutputFilePath)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test FilenameNoPath</summary>
    <TestMethod()> Public Sub FilenameNoPathTest()
        Dim aStr As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = modFile.FilenameNoPath(aStr)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test FilenameSetExt</summary>
    <TestMethod()> Public Sub FilenameSetExtTest()
        Dim aStr As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aExt As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = modFile.FilenameSetExt(aStr, aExt)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test FindFile</summary>
    <TestMethod()> Public Sub FindFileTest()
        Dim aFileDialogTitle As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aDefaultFileName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aDefaultExt As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aFileFilter As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aUserVerifyFileName As Boolean = False ' TODO: Initialize to an appropriate value
        Dim aChangeIntoDir As Boolean = False ' TODO: Initialize to an appropriate value
        Dim aFilterIndex As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aFilterIndexExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = modFile.FindFile(aFileDialogTitle, aDefaultFileName, aDefaultExt, aFileFilter, aUserVerifyFileName, aChangeIntoDir, aFilterIndex)
        Assert.AreEqual(aFilterIndexExpected, aFilterIndex)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test FindFileFilter</summary>
    <TestMethod()> Public Sub FindFileFilterTest()
        Dim FileFilters As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim FileFilterIndex As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = modFile.FindFileFilter(FileFilters, FileFilterIndex)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test FindFileFilterIndex</summary>
    <TestMethod()> Public Sub FindFileFilterIndexTest()
        Dim aAllFilters As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aFindFilter As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = modFile.FindFileFilterIndex(aAllFilters, aFindFilter)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test FindRecursive</summary>
    <TestMethod(), DeploymentItem("atcUtility.dll")> _
    Public Sub FindRecursiveTest()
        Dim aFilename As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aStartDirs() As String = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = modFile_Accessor.FindRecursive(aFilename, aStartDirs)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test GetMaxValue</summary>
    <TestMethod()> Public Sub GetMaxValueTest()
        Dim expected As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim actual As Double
        actual = modFile.GetMaxValue
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test GetMaxValueInternal</summary>
    <TestMethod(), DeploymentItem("atcUtility.dll")> _
    Public Sub GetMaxValueInternalTest()
        Dim aMaxValue As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim expected As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim actual As Double
        actual = modFile_Accessor.GetMaxValueInternal(aMaxValue)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test GetMinValue</summary>
    <TestMethod()> Public Sub GetMinValueTest()
        Dim expected As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim actual As Double
        actual = modFile.GetMinValue
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test GetMinValueInternal</summary>
    <TestMethod(), DeploymentItem("atcUtility.dll")> _
    Public Sub GetMinValueInternalTest()
        Dim aMinValue As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim expected As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim actual As Double
        actual = modFile_Accessor.GetMinValueInternal(aMinValue)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test GetNaN</summary>
    <TestMethod()> Public Sub GetNaNTest()
        Dim expected As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim actual As Double
        actual = modFile.GetNaN
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test GetNaNInternal</summary>
    <TestMethod(), DeploymentItem("atcUtility.dll")> _
    Public Sub GetNaNInternalTest()
        Dim aNaN As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim expected As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim actual As Double
        actual = modFile_Accessor.GetNaNInternal(aNaN)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test GetNewFileName</summary>
    <TestMethod()> Public Sub GetNewFileNameTest()
        Dim aBaseName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aExtension As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = modFile.GetNewFileName(aBaseName, aExtension)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test GetShortPathName</summary>
    <TestMethod()> Public Sub GetShortPathNameTest()
        Dim lpszLongPath As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim lpszShortPath As StringBuilder = Nothing ' TODO: Initialize to an appropriate value
        Dim cchBuffer As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim expected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        actual = modFile.GetShortPathName(lpszLongPath, lpszShortPath, cchBuffer)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test GetTemporaryFileName</summary>
    <TestMethod()> Public Sub GetTemporaryFileNameTest()
        Dim aBaseName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aExtension As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = modFile.GetTemporaryFileName(aBaseName, aExtension)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test LinesInFile</summary>
    <TestMethod()> Public Sub LinesInFileTest()
        Dim aFileName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As IEnumerable = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As IEnumerable
        actual = modFile.LinesInFile(aFileName)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test LinesInFile</summary>
    <TestMethod()> Public Sub LinesInFileTest1()
        Dim aFileReader As BinaryReader = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As IEnumerable = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As IEnumerable
        actual = modFile.LinesInFile(aFileReader)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test LinesInFileReadLine</summary>
    <TestMethod()> Public Sub LinesInFileReadLineTest()
        Dim aFileReader As StreamReader = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As IEnumerable = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As IEnumerable
        actual = modFile.LinesInFileReadLine(aFileReader)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test NewTempDir</summary>
    <TestMethod()> Public Sub NewTempDirTest()
        Dim aBaseName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = modFile.NewTempDir(aBaseName)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test NextLine</summary>
    <TestMethod()> Public Sub NextLineTest()
        Dim aReader As BinaryReader = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = modFile.NextLine(aReader)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test OpenFile</summary>
    <TestMethod()> Public Sub OpenFileTest()
        Dim FileOrURL As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim Wait As Boolean = False ' TODO: Initialize to an appropriate value
        modFile.OpenFile(FileOrURL, Wait)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test ReadBigInt</summary>
    <TestMethod()> Public Sub ReadBigIntTest()
        Dim InFile As Short = 0 ' TODO: Initialize to an appropriate value
        Dim expected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        actual = modFile.ReadBigInt(InFile)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test RelativeFilename</summary>
    <TestMethod()> Public Sub RelativeFilenameTest()
        Dim filename As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim StartPath As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = modFile.RelativeFilename(filename, StartPath)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test ReplaceStringToFile</summary>
    <TestMethod()> Public Sub ReplaceStringToFileTest()
        Dim Source As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim Find As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim ReplaceWith As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim filename As String = String.Empty ' TODO: Initialize to an appropriate value
        modFile.ReplaceStringToFile(Source, Find, ReplaceWith, filename)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test ReportFilesInDir</summary>
    <TestMethod()> Public Sub ReportFilesInDirTest()
        Dim aDirName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aSubdirs As Boolean = False ' TODO: Initialize to an appropriate value
        Dim aFileFilter As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aAttributes As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = modFile.ReportFilesInDir(aDirName, aSubdirs, aFileFilter, aAttributes)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test SafeFilename</summary>
    <TestMethod()> Public Sub SafeFilenameTest()
        Dim aStr As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aReplaceWith As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = modFile.SafeFilename(aStr, aReplaceWith)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test SaveFileBytes</summary>
    <TestMethod()> Public Sub SaveFileBytesTest()
        Dim filename As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim FileContents() As Byte = Nothing ' TODO: Initialize to an appropriate value
        modFile.SaveFileBytes(filename, FileContents)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test SaveFileString</summary>
    <TestMethod()> Public Sub SaveFileStringTest()
        Dim filename As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim FileContents As String = String.Empty ' TODO: Initialize to an appropriate value
        modFile.SaveFileString(filename, FileContents)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test ShowHelp</summary>
    <TestMethod()> Public Sub ShowHelpTest()
        Dim aHelpTopic As String = String.Empty ' TODO: Initialize to an appropriate value
        modFile.ShowHelp(aHelpTopic)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test SwapBytes</summary>
    <TestMethod()> Public Sub SwapBytesTest()
        Dim n As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim expected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        actual = modFile.SwapBytes(n)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test TryCopyGroup</summary>
    <TestMethod()> Public Sub TryCopyGroupTest()
        Dim aBaseFilename As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aDestinationPath As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aExtensions() As String = Nothing ' TODO: Initialize to an appropriate value
        Dim aVerbose As Boolean = False ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = modFile.TryCopyGroup(aBaseFilename, aDestinationPath, aExtensions, aVerbose)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test TryCopyShapefile</summary>
    <TestMethod()> Public Sub TryCopyShapefileTest()
        Dim aShapeFilename As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aDestinationPath As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = modFile.TryCopyShapefile(aShapeFilename, aDestinationPath)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test TryDeleteGroup</summary>
    <TestMethod()> Public Sub TryDeleteGroupTest()
        Dim aBaseFilename As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aExtensions() As String = Nothing ' TODO: Initialize to an appropriate value
        Dim aVerbose As Boolean = False ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = modFile.TryDeleteGroup(aBaseFilename, aExtensions, aVerbose)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test TryDeleteShapefile</summary>
    <TestMethod()> Public Sub TryDeleteShapefileTest()
        Dim aShapeFilename As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = modFile.TryDeleteShapefile(aShapeFilename)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test TryMove</summary>
    <TestMethod()> Public Sub TryMoveTest()
        Dim aFromFilename As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aToPath As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = modFile.TryMove(aFromFilename, aToPath)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test TryMove</summary>
    <TestMethod()> Public Sub TryMoveTest1()
        Dim aFromFilename As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aToPath As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aVerbose As Boolean = False ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = modFile.TryMove(aFromFilename, aToPath, aVerbose)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test TryMoveGroup</summary>
    <TestMethod()> Public Sub TryMoveGroupTest()
        Dim aBaseFilename As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aDestinationPath As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aExtensions() As String = Nothing ' TODO: Initialize to an appropriate value
        Dim aVerbose As Boolean = False ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = modFile.TryMoveGroup(aBaseFilename, aDestinationPath, aExtensions, aVerbose)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test TryMoveShapefile</summary>
    <TestMethod()> Public Sub TryMoveShapefileTest()
        Dim aShapeFilename As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aDestinationPath As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = modFile.TryMoveShapefile(aShapeFilename, aDestinationPath)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test WriteBigInt</summary>
    <TestMethod()> Public Sub WriteBigIntTest()
        Dim OutFile As Short = 0 ' TODO: Initialize to an appropriate value
        Dim Value As Integer = 0 ' TODO: Initialize to an appropriate value
        modFile.WriteBigInt(OutFile, Value)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test _controlfp</summary>
    <TestMethod()> Public Sub _controlfpTest()
        Dim newControl As UInteger = 0 ' TODO: Initialize to an appropriate value
        Dim mask As UInteger = 0 ' TODO: Initialize to an appropriate value
        Dim expected As UInteger = 0 ' TODO: Initialize to an appropriate value
        Dim actual As UInteger
        actual = modFile._controlfp(newControl, mask)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test _fpreset</summary>
    <TestMethod()> Public Sub _fpresetTest()
        modFile._fpreset()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test _statusfp</summary>
    <TestMethod()> Public Sub _statusfpTest()
        Dim expected As UInteger = 0 ' TODO: Initialize to an appropriate value
        Dim actual As UInteger
        actual = modFile._statusfp
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub
End Class
