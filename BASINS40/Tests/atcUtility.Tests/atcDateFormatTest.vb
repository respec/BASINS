Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports atcUtility



'''<summary>
'''This is a test class for atcDateFormatTest and is intended
'''to contain all atcDateFormatTest Unit Tests
'''</summary>
<TestClass()> _
Public Class atcDateFormatTest


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
    '''A test for atcDateFormat Constructor
    '''</summary>
    <TestMethod()> _
    Public Sub atcDateFormatConstructorTest()
        Dim target As atcDateFormat = New atcDateFormat()
        Assert.Inconclusive("TODO: Implement code to verify target")
    End Sub

    '''<summary>
    '''A test for JDateToString
    '''</summary>
    <TestMethod()> _
    Public Sub JDateToStringTest()
        Dim target As atcDateFormat = New atcDateFormat() ' TODO: Initialize to an appropriate value
        Dim aJulianDate As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = target.JDateToString(aJulianDate)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for MonthString
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcUtility.dll")> _
    Public Sub MonthStringTest()
        Dim target As atcDateFormat_Accessor = New atcDateFormat_Accessor() ' TODO: Initialize to an appropriate value
        Dim aDate() As Integer = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = target.MonthString(aDate)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for YearString
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcUtility.dll")> _
    Public Sub YearStringTest()
        Dim target As atcDateFormat_Accessor = New atcDateFormat_Accessor() ' TODO: Initialize to an appropriate value
        Dim aDate() As Integer = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = target.YearString(aDate)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for DateOrder
    '''</summary>
    <TestMethod()> _
    Public Sub DateOrderTest()
        Dim target As atcDateFormat = New atcDateFormat() ' TODO: Initialize to an appropriate value
        Dim expected As atcDateFormat.DateOrderEnum = New atcDateFormat.DateOrderEnum() ' TODO: Initialize to an appropriate value
        Dim actual As atcDateFormat.DateOrderEnum
        target.DateOrder = expected
        actual = target.DateOrder
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for DateSeparator
    '''</summary>
    <TestMethod()> _
    Public Sub DateSeparatorTest()
        Dim target As atcDateFormat = New atcDateFormat() ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        target.DateSeparator = expected
        actual = target.DateSeparator
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for IncludeDays
    '''</summary>
    <TestMethod()> _
    Public Sub IncludeDaysTest()
        Dim target As atcDateFormat = New atcDateFormat() ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        target.IncludeDays = expected
        actual = target.IncludeDays
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for IncludeHours
    '''</summary>
    <TestMethod()> _
    Public Sub IncludeHoursTest()
        Dim target As atcDateFormat = New atcDateFormat() ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        target.IncludeHours = expected
        actual = target.IncludeHours
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for IncludeMinutes
    '''</summary>
    <TestMethod()> _
    Public Sub IncludeMinutesTest()
        Dim target As atcDateFormat = New atcDateFormat() ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        target.IncludeMinutes = expected
        actual = target.IncludeMinutes
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for IncludeMonths
    '''</summary>
    <TestMethod()> _
    Public Sub IncludeMonthsTest()
        Dim target As atcDateFormat = New atcDateFormat() ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        target.IncludeMonths = expected
        actual = target.IncludeMonths
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for IncludeSeconds
    '''</summary>
    <TestMethod()> _
    Public Sub IncludeSecondsTest()
        Dim target As atcDateFormat = New atcDateFormat() ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        target.IncludeSeconds = expected
        actual = target.IncludeSeconds
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for IncludeYears
    '''</summary>
    <TestMethod()> _
    Public Sub IncludeYearsTest()
        Dim target As atcDateFormat = New atcDateFormat() ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        target.IncludeYears = expected
        actual = target.IncludeYears
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for Midnight24
    '''</summary>
    <TestMethod()> _
    Public Sub Midnight24Test()
        Dim target As atcDateFormat = New atcDateFormat() ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        target.Midnight24 = expected
        actual = target.Midnight24
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for MonthNames
    '''</summary>
    <TestMethod()> _
    Public Sub MonthNamesTest()
        Dim target As atcDateFormat = New atcDateFormat() ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        target.MonthNames = expected
        actual = target.MonthNames
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for TimeSeparator
    '''</summary>
    <TestMethod()> _
    Public Sub TimeSeparatorTest()
        Dim target As atcDateFormat = New atcDateFormat() ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        target.TimeSeparator = expected
        actual = target.TimeSeparator
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for TwoDigitYears
    '''</summary>
    <TestMethod()> _
    Public Sub TwoDigitYearsTest()
        Dim target As atcDateFormat = New atcDateFormat() ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        target.TwoDigitYears = expected
        actual = target.TwoDigitYears
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub
End Class
