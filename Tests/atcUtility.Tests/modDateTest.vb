Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports atcUtility



'''<summary>
'''This is a test class for modDateTest and is intended
'''to contain all modDateTest Unit Tests
'''</summary>
<TestClass()> _
Public Class modDateTest


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
    '''A test for AddUniqueDate
    '''</summary>
    <TestMethod()> _
    Public Sub AddUniqueDateTest()
        Dim aJdate As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim aJdateExisting() As Double = Nothing ' TODO: Initialize to an appropriate value
        Dim aJdateExistingExpected() As Double = Nothing ' TODO: Initialize to an appropriate value
        Dim aJDateExistingInterval() As Integer = Nothing ' TODO: Initialize to an appropriate value
        Dim aJDateExistingIntervalExpected() As Integer = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = modDate.AddUniqueDate(aJdate, aJdateExisting, aJDateExistingInterval)
        Assert.AreEqual(aJdateExistingExpected, aJdateExisting)
        Assert.AreEqual(aJDateExistingIntervalExpected, aJDateExistingInterval)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for CalcTimeUnitStep
    '''</summary>
    <TestMethod()> _
    Public Sub CalcTimeUnitStepTest()
        Dim aSJDate As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim aEJDate As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim aTimeUnit As modDate.atcTimeUnit = New modDate.atcTimeUnit() ' TODO: Initialize to an appropriate value
        Dim aTimeUnitExpected As modDate.atcTimeUnit = New modDate.atcTimeUnit() ' TODO: Initialize to an appropriate value
        Dim aTimeStep As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aTimeStepExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        modDate.CalcTimeUnitStep(aSJDate, aEJDate, aTimeUnit, aTimeStep)
        Assert.AreEqual(aTimeUnitExpected, aTimeUnit)
        Assert.AreEqual(aTimeStepExpected, aTimeStep)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for Date2J
    '''</summary>
    <TestMethod()> _
    Public Sub Date2JTest()
        Dim aYr As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aMo As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aDy As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aHr As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aMn As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aSc As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim expected As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim actual As Double
        actual = modDate.Date2J(aYr, aMo, aDy, aHr, aMn, aSc)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for Date2J
    '''</summary>
    <TestMethod()> _
    Public Sub Date2JTest1()
        Dim aDate() As Integer = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim actual As Double
        actual = modDate.Date2J(aDate)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for DateIntrvl
    '''</summary>
    <TestMethod()> _
    Public Sub DateIntrvlTest()
        Dim aDate() As Integer = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        actual = modDate.DateIntrvl(aDate)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for DayMon
    '''</summary>
    <TestMethod()> _
    Public Sub DayMonTest()
        Dim aYr As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aMo As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim expected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        actual = modDate.DayMon(aYr, aMo)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for DumpDate
    '''</summary>
    <TestMethod()> _
    Public Sub DumpDateTest()
        Dim aDateJ As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = modDate.DumpDate(aDateJ)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for FormatTime
    '''</summary>
    <TestMethod()> _
    Public Sub FormatTimeTest()
        Dim aSeconds As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = modDate.FormatTime(aSeconds)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for HMS2J
    '''</summary>
    <TestMethod()> _
    Public Sub HMS2JTest()
        Dim aHr As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aMi As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aSc As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim expected As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim actual As Double
        actual = modDate.HMS2J(aHr, aMi, aSc)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for INVMJD
    '''</summary>
    <TestMethod()> _
    Public Sub INVMJDTest()
        Dim aMJD As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aYr As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aYrExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aMn As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aMnExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aDy As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aDyExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        modDate.INVMJD(aMJD, aYr, aMn, aDy)
        Assert.AreEqual(aYrExpected, aYr)
        Assert.AreEqual(aMnExpected, aMn)
        Assert.AreEqual(aDyExpected, aDy)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for J2Date
    '''</summary>
    <TestMethod()> _
    Public Sub J2DateTest()
        Dim aJd As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim aDate() As Integer = Nothing ' TODO: Initialize to an appropriate value
        Dim aDateExpected() As Integer = Nothing ' TODO: Initialize to an appropriate value
        modDate.J2Date(aJd, aDate)
        Assert.AreEqual(aDateExpected, aDate)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for J2DateRounddown
    '''</summary>
    <TestMethod()> _
    Public Sub J2DateRounddownTest()
        Dim aJDate As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim aTU As modDate.atcTimeUnit = New modDate.atcTimeUnit() ' TODO: Initialize to an appropriate value
        Dim aDate() As Integer = Nothing ' TODO: Initialize to an appropriate value
        Dim aDateExpected() As Integer = Nothing ' TODO: Initialize to an appropriate value
        modDate.J2DateRounddown(aJDate, aTU, aDate)
        Assert.AreEqual(aDateExpected, aDate)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for J2DateRoundup
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcUtility.dll")> _
    Public Sub J2DateRoundupTest()
        Dim aJDate As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim aTU As modDate.atcTimeUnit = New modDate.atcTimeUnit() ' TODO: Initialize to an appropriate value
        Dim aDate() As Integer = Nothing ' TODO: Initialize to an appropriate value
        modDate_Accessor.J2DateRoundup(aJDate, aTU, aDate)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for J2HMS
    '''</summary>
    <TestMethod()> _
    Public Sub J2HMSTest()
        Dim aJd As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim aHr As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aHrExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aMi As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aMiExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aSc As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aScExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aFrac As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim aFracExpected As Double = 0.0! ' TODO: Initialize to an appropriate value
        modDate.J2HMS(aJd, aHr, aMi, aSc, aFrac)
        Assert.AreEqual(aHrExpected, aHr)
        Assert.AreEqual(aMiExpected, aMi)
        Assert.AreEqual(aScExpected, aSc)
        Assert.AreEqual(aFracExpected, aFrac)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for JDateIntrvl
    '''</summary>
    <TestMethod()> _
    Public Sub JDateIntrvlTest()
        Dim aJd As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim expected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        actual = modDate.JDateIntrvl(aJd)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for Jday
    '''</summary>
    <TestMethod()> _
    Public Sub JdayTest()
        Dim aYr As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aMo As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aDy As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aHr As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aMn As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aSc As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim expected As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim actual As Double
        actual = modDate.Jday(aYr, aMo, aDy, aHr, aMn, aSc)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for MJD
    '''</summary>
    <TestMethod()> _
    Public Sub MJDTest()
        Dim aYr As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aMn As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aDy As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim expected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        actual = modDate.MJD(aYr, aMn, aDy)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for StringToJdate
    '''</summary>
    <TestMethod()> _
    Public Sub StringToJdateTest()
        Dim aText As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aIntervalStart As Boolean = False ' TODO: Initialize to an appropriate value
        Dim expected As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim actual As Double
        actual = modDate.StringToJdate(aText, aIntervalStart)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for TIMADD
    '''</summary>
    <TestMethod()> _
    Public Sub TIMADDTest()
        Dim DATE1() As Integer = Nothing ' TODO: Initialize to an appropriate value
        Dim TCODE As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim TSTEP As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim NVALS As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim DATE2() As Integer = Nothing ' TODO: Initialize to an appropriate value
        Dim DATE2Expected() As Integer = Nothing ' TODO: Initialize to an appropriate value
        modDate.TIMADD(DATE1, TCODE, TSTEP, NVALS, DATE2)
        Assert.AreEqual(DATE2Expected, DATE2)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for TimAddJ
    '''</summary>
    <TestMethod()> _
    Public Sub TimAddJTest()
        Dim aStartDate As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim aTimeUnits As modDate.atcTimeUnit = New modDate.atcTimeUnit() ' TODO: Initialize to an appropriate value
        Dim aTimeStep As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aNumValues As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim expected As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim actual As Double
        actual = modDate.TimAddJ(aStartDate, aTimeUnits, aTimeStep, aNumValues)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for TimDif
    '''</summary>
    <TestMethod()> _
    Public Sub TimDifTest()
        Dim DATE1() As Integer = Nothing ' TODO: Initialize to an appropriate value
        Dim DATE2() As Integer = Nothing ' TODO: Initialize to an appropriate value
        Dim TCODE As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim TSTEP As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim NVALS As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim NVALSExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        modDate.TimDif(DATE1, DATE2, TCODE, TSTEP, NVALS)
        Assert.AreEqual(NVALSExpected, NVALS)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for TimeSpanAsString
    '''</summary>
    <TestMethod()> _
    Public Sub TimeSpanAsStringTest()
        Dim aSDateJ As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim aEdateJ As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim aPrefix As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = modDate.TimeSpanAsString(aSDateJ, aEdateJ, aPrefix)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for YearCount
    '''</summary>
    <TestMethod()> _
    Public Sub YearCountTest()
        Dim aSDateJ As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim aEdateJ As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim expected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        actual = modDate.YearCount(aSDateJ, aEdateJ)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for cmptim
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcUtility.dll")> _
    Public Sub cmptimTest()
        Dim tcode1 As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim tstep1 As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim tcode2 As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim tstep2 As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim tstepf As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim tstepfExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim tcdcmp As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim tcdcmpExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        modDate_Accessor.cmptim(tcode1, tstep1, tcode2, tstep2, tstepf, tcdcmp)
        Assert.AreEqual(tstepfExpected, tstepf)
        Assert.AreEqual(tcdcmpExpected, tcdcmp)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for cmptm2
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcUtility.dll")> _
    Public Sub cmptm2Test()
        Dim tc1 As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim ts1 As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim tc2 As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim ts2 As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim tstepf As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim tstepfExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim tcdcmp As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim tcdcmpExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        modDate_Accessor.cmptm2(tc1, ts1, tc2, ts2, tstepf, tcdcmp)
        Assert.AreEqual(tstepfExpected, tstepf)
        Assert.AreEqual(tcdcmpExpected, tcdcmp)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for pTimDif
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcUtility.dll")> _
    Public Sub pTimDifTest()
        Dim StartJDate As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim EndJDate As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim DATE1() As Integer = Nothing ' TODO: Initialize to an appropriate value
        Dim DATE2() As Integer = Nothing ' TODO: Initialize to an appropriate value
        Dim TCODE As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim TSTEP As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim expected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        actual = modDate_Accessor.pTimDif(StartJDate, EndJDate, DATE1, DATE2, TCODE, TSTEP)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for timcnv
    '''</summary>
    <TestMethod()> _
    Public Sub timcnvTest()
        Dim d() As Integer = Nothing ' TODO: Initialize to an appropriate value
        Dim dExpected() As Integer = Nothing ' TODO: Initialize to an appropriate value
        modDate.timcnv(d)
        Assert.AreEqual(dExpected, d)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for timdifJ
    '''</summary>
    <TestMethod()> _
    Public Sub timdifJTest()
        Dim StartJDate As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim EndJDate As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim TCODE As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim TSTEP As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim expected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        actual = modDate.timdifJ(StartJDate, EndJDate, TCODE, TSTEP)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub
End Class
