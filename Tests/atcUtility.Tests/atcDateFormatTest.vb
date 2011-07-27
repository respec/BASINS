Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports atcUtility

'''<summary>
'''This is a test class for atcDateFormatTest and is intended
'''to contain all atcDateFormatTest Unit Tests (Done)
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

    '''<summary>Test atcDateFormat Constructor</summary>
    <TestMethod()> Public Sub atcDateFormatConstructorTest()
        Dim target As atcDateFormat = New atcDateFormat()
        Assert.IsInstanceOfType(target, GetType(atcDateFormat))
    End Sub

    '''<summary>Test JDateToString</summary>
    <TestMethod()> Public Sub JDateToStringTest()
        Dim target As atcDateFormat = New atcDateFormat()
        Dim aJulianDate As Double = Jday(1999, 5, 5, 23, 0, 0)
        Dim expected As String = "1999/05/05 23:00"
        Dim actual As String = target.JDateToString(aJulianDate)
        Assert.AreEqual(expected, actual)
    End Sub

    '''<summary>Test MonthString</summary>
    <TestMethod(), DeploymentItem("atcUtility.dll")> _
    Public Sub MonthStringTest()
        Dim target As atcDateFormat_Accessor = New atcDateFormat_Accessor() ' TODO: Initialize to an appropriate value
        Dim aDate() As Integer = {1999, 5, 5, 23, 0, 0}
        Dim expected As String = "05"
        Dim actual As String = target.MonthString(aDate)
        Assert.AreEqual(expected, actual)
    End Sub

    '''<summary>Test YearString</summary>
    <TestMethod(), DeploymentItem("atcUtility.dll")> _
    Public Sub YearStringTest()
        Dim target As atcDateFormat_Accessor = New atcDateFormat_Accessor() ' TODO: Initialize to an appropriate value
        Dim aDate() As Integer = {1999, 5, 5, 23, 0, 0}
        Dim expected As String = "1999"
        Dim actual As String = target.YearString(aDate)
        Assert.AreEqual(expected, actual)
    End Sub

    '''<summary>Test DateOrder</summary>
    <TestMethod()> Public Sub DateOrderTest()
        Dim target As atcDateFormat = New atcDateFormat() ' TODO: Initialize to an appropriate value
        Dim expected As atcDateFormat.DateOrderEnum = atcDateFormat.DateOrderEnum.YearMonthDay

        target.DateOrder = expected
        Dim actual As atcDateFormat.DateOrderEnum = target.DateOrder
        Assert.AreEqual(expected, actual)
    End Sub

    '''<summary>Test DateSeparator</summary>
    <TestMethod()> Public Sub DateSeparatorTest()
        Dim target As atcDateFormat = New atcDateFormat() ' TODO: Initialize to an appropriate value
        Dim expected As String = "/"
        target.DateSeparator = expected
        Dim actual As String = target.DateSeparator
        Assert.AreEqual(expected, actual)
    End Sub

    '''<summary>Test IncludeDays</summary>
    <TestMethod()> Public Sub IncludeDaysTest()
        Dim target As atcDateFormat = New atcDateFormat() ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        target.IncludeDays = expected
        actual = target.IncludeDays
        Assert.AreEqual(expected, actual)
    End Sub

    '''<summary>Test IncludeHours</summary>
    <TestMethod()> Public Sub IncludeHoursTest()
        Dim target As atcDateFormat = New atcDateFormat() ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        target.IncludeHours = expected
        actual = target.IncludeHours
        Assert.AreEqual(expected, actual)
    End Sub

    '''<summary>Test IncludeMinutes</summary>
    <TestMethod()> Public Sub IncludeMinutesTest()
        Dim target As atcDateFormat = New atcDateFormat() ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        target.IncludeMinutes = expected
        actual = target.IncludeMinutes
        Assert.AreEqual(expected, actual)
    End Sub

    '''<summary>Test IncludeMonths</summary>
    <TestMethod()> Public Sub IncludeMonthsTest()
        Dim target As atcDateFormat = New atcDateFormat() ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        target.IncludeMonths = expected
        actual = target.IncludeMonths
        Assert.AreEqual(expected, actual)
    End Sub

    '''<summary>Test IncludeSeconds</summary>
    <TestMethod()> Public Sub IncludeSecondsTest()
        Dim target As atcDateFormat = New atcDateFormat() ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        target.IncludeSeconds = expected
        actual = target.IncludeSeconds
        Assert.AreEqual(expected, actual)
    End Sub

    '''<summary>Test IncludeYears</summary>
    <TestMethod()> Public Sub IncludeYearsTest()
        Dim target As atcDateFormat = New atcDateFormat() ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        target.IncludeYears = expected
        actual = target.IncludeYears
        Assert.AreEqual(expected, actual)
    End Sub

    '''<summary>Test Midnight24</summary>
    <TestMethod()> Public Sub Midnight24Test()
        Dim target As atcDateFormat = New atcDateFormat() ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        target.Midnight24 = expected
        actual = target.Midnight24
        Assert.AreEqual(expected, actual)
    End Sub

    '''<summary>Test MonthNames</summary>
    <TestMethod()> Public Sub MonthNamesTest()
        Dim target As atcDateFormat = New atcDateFormat() ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        target.MonthNames = expected
        actual = target.MonthNames
        Assert.AreEqual(expected, actual)
    End Sub

    '''<summary>Test TimeSeparator</summary>
    <TestMethod()> Public Sub TimeSeparatorTest()
        Dim target As atcDateFormat = New atcDateFormat() ' TODO: Initialize to an appropriate value
        Dim expected As String = ","

        target.TimeSeparator = expected
        Dim actual As String = target.TimeSeparator
        Assert.AreEqual(expected, actual)
    End Sub

    '''<summary>Test TwoDigitYears</summary>
    <TestMethod()> Public Sub TwoDigitYearsTest()
        Dim target As atcDateFormat = New atcDateFormat() ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        target.TwoDigitYears = expected
        actual = target.TwoDigitYears
        Assert.AreEqual(expected, actual)
    End Sub
End Class
