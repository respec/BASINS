Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports atcWDM

'''<summary>
'''This is a test class for Vb2F90Test and is intended
'''to contain all Vb2F90Test Unit Tests
'''</summary>
<TestClass()> _
Public Class Vb2F90Test
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

    '''<summary>Test F90_GETATT</summary>
    <TestMethod()> Public Sub F90_GETATTTest()
        Dim aWdmUnit As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aWdmUnitExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aDsn As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aDsnExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aInit As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aInitExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aSaInd As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aSaIndExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aSaVal() As Integer = Nothing ' TODO: Initialize to an appropriate value
        Vb2F90.F90_GETATT(aWdmUnit, aDsn, aInit, aSaInd, aSaVal)
        Assert.AreEqual(aWdmUnitExpected, aWdmUnit)
        Assert.AreEqual(aDsnExpected, aDsn)
        Assert.AreEqual(aInitExpected, aInit)
        Assert.AreEqual(aSaIndExpected, aSaInd)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test F90_INQNAM</summary>
    <TestMethod()> Public Sub F90_INQNAMTest()
        Dim aName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aNameExpected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aNameLen As Short = 0 ' TODO: Initialize to an appropriate value
        Dim expected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        actual = Vb2F90.F90_INQNAM(aName, aNameLen)
        Assert.AreEqual(aNameExpected, aName)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test F90_MSG</summary>
    <TestMethod()> Public Sub F90_MSGTest()
        Dim aMsg As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aMsgExpected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aMsgLen As Short = 0 ' TODO: Initialize to an appropriate value
        Vb2F90.F90_MSG(aMsg, aMsgLen)
        Assert.AreEqual(aMsgExpected, aMsg)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test F90_WDBOPNR</summary>
    <TestMethod()> Public Sub F90_WDBOPNRTest()
        Dim aRwflg As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aRwflgExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aNameExpected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aUnit As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aUnitExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aRetcod As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aRetcodExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aNameLen As Short = 0 ' TODO: Initialize to an appropriate value
        Vb2F90.F90_WDBOPNR(aRwflg, aName, aUnit, aRetcod, aNameLen)
        Assert.AreEqual(aRwflgExpected, aRwflg)
        Assert.AreEqual(aNameExpected, aName)
        Assert.AreEqual(aUnitExpected, aUnit)
        Assert.AreEqual(aRetcodExpected, aRetcod)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test F90_WDBSAC</summary>
    <TestMethod()> Public Sub F90_WDBSACTest()
        Dim aWdmUnit As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aWdmUnitExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aDsn As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aDsnExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aMsgUnit As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aMsgUnitExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aSaind As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aSaindExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aSalen As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aSalenExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aRetcod As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aRetcodExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aVal As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aValExpected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aValLen As Short = 0 ' TODO: Initialize to an appropriate value
        Vb2F90.F90_WDBSAC(aWdmUnit, aDsn, aMsgUnit, aSaind, aSalen, aRetcod, aVal, aValLen)
        Assert.AreEqual(aWdmUnitExpected, aWdmUnit)
        Assert.AreEqual(aDsnExpected, aDsn)
        Assert.AreEqual(aMsgUnitExpected, aMsgUnit)
        Assert.AreEqual(aSaindExpected, aSaind)
        Assert.AreEqual(aSalenExpected, aSalen)
        Assert.AreEqual(aRetcodExpected, aRetcod)
        Assert.AreEqual(aValExpected, aVal)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test F90_WDBSAI</summary>
    <TestMethod()> Public Sub F90_WDBSAITest()
        Dim aWdmUnit As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aWdmUnitExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aDsn As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aDsnExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aMsgUnit As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aMsgUnitExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aSaind As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aSaindExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aSalen As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aSalenExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aVal As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aValExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aRetcod As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aRetcodExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Vb2F90.F90_WDBSAI(aWdmUnit, aDsn, aMsgUnit, aSaind, aSalen, aVal, aRetcod)
        Assert.AreEqual(aWdmUnitExpected, aWdmUnit)
        Assert.AreEqual(aDsnExpected, aDsn)
        Assert.AreEqual(aMsgUnitExpected, aMsgUnit)
        Assert.AreEqual(aSaindExpected, aSaind)
        Assert.AreEqual(aSalenExpected, aSalen)
        Assert.AreEqual(aValExpected, aVal)
        Assert.AreEqual(aRetcodExpected, aRetcod)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test F90_WDBSAR</summary>
    <TestMethod()> Public Sub F90_WDBSARTest()
        Dim aWdmUnit As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aWdmUnitExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aDsn As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aDsnExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aMsgUnit As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aMsgUnitExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aSaind As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aSaindExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aSalen As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aSalenExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aVal As Single = 0.0! ' TODO: Initialize to an appropriate value
        Dim aValExpected As Single = 0.0! ' TODO: Initialize to an appropriate value
        Dim aRetcod As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aRetcodExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Vb2F90.F90_WDBSAR(aWdmUnit, aDsn, aMsgUnit, aSaind, aSalen, aVal, aRetcod)
        Assert.AreEqual(aWdmUnitExpected, aWdmUnit)
        Assert.AreEqual(aDsnExpected, aDsn)
        Assert.AreEqual(aMsgUnitExpected, aMsgUnit)
        Assert.AreEqual(aSaindExpected, aSaind)
        Assert.AreEqual(aSalenExpected, aSalen)
        Assert.AreEqual(aValExpected, aVal)
        Assert.AreEqual(aRetcodExpected, aRetcod)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test F90_WDBSGC</summary>
    <TestMethod()> Public Sub F90_WDBSGCTest()
        Dim aWdmUnit As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aWdmUnitExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aDsn As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aDsnExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aSaInd As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aSaIndExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aSaLen As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aSaLenExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aSaVal As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aSaValExpected As String = String.Empty ' TODO: Initialize to an appropriate value
        Vb2F90.F90_WDBSGC(aWdmUnit, aDsn, aSaInd, aSaLen, aSaVal)
        Assert.AreEqual(aWdmUnitExpected, aWdmUnit)
        Assert.AreEqual(aDsnExpected, aDsn)
        Assert.AreEqual(aSaIndExpected, aSaInd)
        Assert.AreEqual(aSaLenExpected, aSaLen)
        Assert.AreEqual(aSaValExpected, aSaVal)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test F90_WDBSGC_XX</summary>
    <TestMethod()> Public Sub F90_WDBSGC_XXTest()
        Dim aWdmUnit As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aWdmUnitExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aDsn As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aDsnExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aSaInd As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aSaIndExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aSaLen As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aSaLenExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aISaVal() As Integer = Nothing ' TODO: Initialize to an appropriate value
        Vb2F90.F90_WDBSGC_XX(aWdmUnit, aDsn, aSaInd, aSaLen, aISaVal)
        Assert.AreEqual(aWdmUnitExpected, aWdmUnit)
        Assert.AreEqual(aDsnExpected, aDsn)
        Assert.AreEqual(aSaIndExpected, aSaInd)
        Assert.AreEqual(aSaLenExpected, aSaLen)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test F90_WDBSGI</summary>
    <TestMethod()> Public Sub F90_WDBSGITest()
        Dim aWdmUnit As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aWdmUnitExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aDsn As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aDsnExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aSaInd As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aSaIndExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aSaLen As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aSaLenExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aSaVal As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aSaValExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aRetcod As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aRetcodExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Vb2F90.F90_WDBSGI(aWdmUnit, aDsn, aSaInd, aSaLen, aSaVal, aRetcod)
        Assert.AreEqual(aWdmUnitExpected, aWdmUnit)
        Assert.AreEqual(aDsnExpected, aDsn)
        Assert.AreEqual(aSaIndExpected, aSaInd)
        Assert.AreEqual(aSaLenExpected, aSaLen)
        Assert.AreEqual(aSaValExpected, aSaVal)
        Assert.AreEqual(aRetcodExpected, aRetcod)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test F90_WDBSGR</summary>
    <TestMethod()> Public Sub F90_WDBSGRTest()
        Dim aWdmUnit As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aWdmUnitExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aDsn As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aDsnExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aSaInd As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aSaIndExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aSaLen As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aSaLenExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aSaVal As Single = 0.0! ' TODO: Initialize to an appropriate value
        Dim aSaValExpected As Single = 0.0! ' TODO: Initialize to an appropriate value
        Dim aRetcod As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aRetcodExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Vb2F90.F90_WDBSGR(aWdmUnit, aDsn, aSaInd, aSaLen, aSaVal, aRetcod)
        Assert.AreEqual(aWdmUnitExpected, aWdmUnit)
        Assert.AreEqual(aDsnExpected, aDsn)
        Assert.AreEqual(aSaIndExpected, aSaInd)
        Assert.AreEqual(aSaLenExpected, aSaLen)
        Assert.AreEqual(aSaValExpected, aSaVal)
        Assert.AreEqual(aRetcodExpected, aRetcod)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test F90_WDCKDT</summary>
    <TestMethod()> Public Sub F90_WDCKDTTest()
        Dim aWdmUnit As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aWdmUnitExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aDsn As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aDsnExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim expected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        actual = Vb2F90.F90_WDCKDT(aWdmUnit, aDsn)
        Assert.AreEqual(aWdmUnitExpected, aWdmUnit)
        Assert.AreEqual(aDsnExpected, aDsn)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test F90_WDDSDL</summary>
    <TestMethod()> Public Sub F90_WDDSDLTest()
        Dim aWdmUnit As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aWdmUnitExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aDsn As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aDsnExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aRetcod As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aRetcodExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Vb2F90.F90_WDDSDL(aWdmUnit, aDsn, aRetcod)
        Assert.AreEqual(aWdmUnitExpected, aWdmUnit)
        Assert.AreEqual(aDsnExpected, aDsn)
        Assert.AreEqual(aRetcodExpected, aRetcod)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test F90_WDDSNX</summary>
    <TestMethod()> Public Sub F90_WDDSNXTest()
        Dim aWdmUnit As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aWdmUnitExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aDsn As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aDsnExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Vb2F90.F90_WDDSNX(aWdmUnit, aDsn)
        Assert.AreEqual(aWdmUnitExpected, aWdmUnit)
        Assert.AreEqual(aDsnExpected, aDsn)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test F90_WDDSRN</summary>
    <TestMethod()> Public Sub F90_WDDSRNTest()
        Dim aWdmUnit As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aWdmUnitExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aDsnOld As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aDsnOldExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aDsnNew As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aDsnNewExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aRetcod As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aRetcodExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Vb2F90.F90_WDDSRN(aWdmUnit, aDsnOld, aDsnNew, aRetcod)
        Assert.AreEqual(aWdmUnitExpected, aWdmUnit)
        Assert.AreEqual(aDsnOldExpected, aDsnOld)
        Assert.AreEqual(aDsnNewExpected, aDsnNew)
        Assert.AreEqual(aRetcodExpected, aRetcod)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test F90_WDFLCL</summary>
    <TestMethod()> Public Sub F90_WDFLCLTest()
        Dim aWdmUnit As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aWdmUnitExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim expected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        actual = Vb2F90.F90_WDFLCL(aWdmUnit)
        Assert.AreEqual(aWdmUnitExpected, aWdmUnit)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test F90_WDLBAD</summary>
    <TestMethod()> Public Sub F90_WDLBADTest()
        Dim aWdmUnit As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aWdmUnitExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aDsn As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aDsnExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aDsType As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aDsTypeExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aPsa As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aPsaExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Vb2F90.F90_WDLBAD(aWdmUnit, aDsn, aDsType, aPsa)
        Assert.AreEqual(aWdmUnitExpected, aWdmUnit)
        Assert.AreEqual(aDsnExpected, aDsn)
        Assert.AreEqual(aDsTypeExpected, aDsType)
        Assert.AreEqual(aPsaExpected, aPsa)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test F90_WDLBAX</summary>
    <TestMethod()> Public Sub F90_WDLBAXTest()
        Dim aWdmUnit As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aWdmUnitExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aDsn As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aDsnExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aDstype As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aDstypeExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aNDn As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aNDnExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aNUp As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aNUpExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aNSa As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aNSaExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aNSasp As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aNSaspExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aNDp As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aNDpExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aPsa As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aPsaExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Vb2F90.F90_WDLBAX(aWdmUnit, aDsn, aDstype, aNDn, aNUp, aNSa, aNSasp, aNDp, aPsa)
        Assert.AreEqual(aWdmUnitExpected, aWdmUnit)
        Assert.AreEqual(aDsnExpected, aDsn)
        Assert.AreEqual(aDstypeExpected, aDstype)
        Assert.AreEqual(aNDnExpected, aNDn)
        Assert.AreEqual(aNUpExpected, aNUp)
        Assert.AreEqual(aNSaExpected, aNSa)
        Assert.AreEqual(aNSaspExpected, aNSasp)
        Assert.AreEqual(aNDpExpected, aNDp)
        Assert.AreEqual(aPsaExpected, aPsa)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test F90_WDSAGY</summary>
    <TestMethod()> Public Sub F90_WDSAGYTest()
        Dim aWdmUnit As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aWdmUnitExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aSaind As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aSaindExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aLen As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aLenExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aType As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aTypeExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aMin As Single = 0.0! ' TODO: Initialize to an appropriate value
        Dim aMinExpected As Single = 0.0! ' TODO: Initialize to an appropriate value
        Dim aMax As Single = 0.0! ' TODO: Initialize to an appropriate value
        Dim aMaxExpected As Single = 0.0! ' TODO: Initialize to an appropriate value
        Dim aDef As Single = 0.0! ' TODO: Initialize to an appropriate value
        Dim aDefExpected As Single = 0.0! ' TODO: Initialize to an appropriate value
        Dim aHLen As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aHLenExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aHRec As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aHRecExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aHPos As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aHPosExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aVLen As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aVLenExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aNameExpected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aDesc As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aDescExpected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aValid As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aValidExpected As String = String.Empty ' TODO: Initialize to an appropriate value
        Vb2F90.F90_WDSAGY(aWdmUnit, aSaind, aLen, aType, aMin, aMax, aDef, aHLen, aHRec, aHPos, aVLen, aName, aDesc, aValid)
        Assert.AreEqual(aWdmUnitExpected, aWdmUnit)
        Assert.AreEqual(aSaindExpected, aSaind)
        Assert.AreEqual(aLenExpected, aLen)
        Assert.AreEqual(aTypeExpected, aType)
        Assert.AreEqual(aMinExpected, aMin)
        Assert.AreEqual(aMaxExpected, aMax)
        Assert.AreEqual(aDefExpected, aDef)
        Assert.AreEqual(aHLenExpected, aHLen)
        Assert.AreEqual(aHRecExpected, aHRec)
        Assert.AreEqual(aHPosExpected, aHPos)
        Assert.AreEqual(aVLenExpected, aVLen)
        Assert.AreEqual(aNameExpected, aName)
        Assert.AreEqual(aDescExpected, aDesc)
        Assert.AreEqual(aValidExpected, aValid)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test F90_WDSAGY_XX</summary>
    <TestMethod()> Public Sub F90_WDSAGY_XXTest()
        Dim aWdmUnit As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aWdmUnitExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aSaind As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aSaindExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aLen As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aLenExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aType As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aTypeExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aMin As Single = 0.0! ' TODO: Initialize to an appropriate value
        Dim aMinExpected As Single = 0.0! ' TODO: Initialize to an appropriate value
        Dim aMax As Single = 0.0! ' TODO: Initialize to an appropriate value
        Dim aMaxExpected As Single = 0.0! ' TODO: Initialize to an appropriate value
        Dim aDef As Single = 0.0! ' TODO: Initialize to an appropriate value
        Dim aDefExpected As Single = 0.0! ' TODO: Initialize to an appropriate value
        Dim aHLen As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aHLenExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aHRec As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aHRecExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aHPos As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aHPosExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aVLen As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aVLenExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aIName() As Integer = Nothing ' TODO: Initialize to an appropriate value
        Dim aIDesc() As Integer = Nothing ' TODO: Initialize to an appropriate value
        Dim aIValid() As Integer = Nothing ' TODO: Initialize to an appropriate value
        Vb2F90.F90_WDSAGY_XX(aWdmUnit, aSaind, aLen, aType, aMin, aMax, aDef, aHLen, aHRec, aHPos, aVLen, aIName, aIDesc, aIValid)
        Assert.AreEqual(aWdmUnitExpected, aWdmUnit)
        Assert.AreEqual(aSaindExpected, aSaind)
        Assert.AreEqual(aLenExpected, aLen)
        Assert.AreEqual(aTypeExpected, aType)
        Assert.AreEqual(aMinExpected, aMin)
        Assert.AreEqual(aMaxExpected, aMax)
        Assert.AreEqual(aDefExpected, aDef)
        Assert.AreEqual(aHLenExpected, aHLen)
        Assert.AreEqual(aHRecExpected, aHRec)
        Assert.AreEqual(aHPosExpected, aHPos)
        Assert.AreEqual(aVLenExpected, aVLen)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test F90_WDTGET</summary>
    <TestMethod()> Public Sub F90_WDTGETTest()
        Dim aWdmUnit As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aWdmUnitExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aDsn As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aDsnExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aDelt As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aDeltExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aDates() As Integer = Nothing ' TODO: Initialize to an appropriate value
        Dim aNval As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aNvalExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aDtran As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aDtranExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aQualfg As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aQualfgExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aTunits As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aTunitsExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aRVal() As Single = Nothing ' TODO: Initialize to an appropriate value
        Dim aRetcod As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aRetcodExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Vb2F90.F90_WDTGET(aWdmUnit, aDsn, aDelt, aDates, aNval, aDtran, aQualfg, aTunits, aRVal, aRetcod)
        Assert.AreEqual(aWdmUnitExpected, aWdmUnit)
        Assert.AreEqual(aDsnExpected, aDsn)
        Assert.AreEqual(aDeltExpected, aDelt)
        Assert.AreEqual(aNvalExpected, aNval)
        Assert.AreEqual(aDtranExpected, aDtran)
        Assert.AreEqual(aQualfgExpected, aQualfg)
        Assert.AreEqual(aTunitsExpected, aTunits)
        Assert.AreEqual(aRetcodExpected, aRetcod)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test F90_WDTPUT</summary>
    <TestMethod()> Public Sub F90_WDTPUTTest()
        Dim aWdmUnit As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aWdmUnitExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aDsn As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aDsnExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aDelt As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aDeltExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aDates() As Integer = Nothing ' TODO: Initialize to an appropriate value
        Dim aNval As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aNvalExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aDtran As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aDtranExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aQualfg As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aQualfgExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aTunits As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aTunitsExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aRVal() As Single = Nothing ' TODO: Initialize to an appropriate value
        Dim aRetcod As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aRetcodExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Vb2F90.F90_WDTPUT(aWdmUnit, aDsn, aDelt, aDates, aNval, aDtran, aQualfg, aTunits, aRVal, aRetcod)
        Assert.AreEqual(aWdmUnitExpected, aWdmUnit)
        Assert.AreEqual(aDsnExpected, aDsn)
        Assert.AreEqual(aDeltExpected, aDelt)
        Assert.AreEqual(aNvalExpected, aNval)
        Assert.AreEqual(aDtranExpected, aDtran)
        Assert.AreEqual(aQualfgExpected, aQualfg)
        Assert.AreEqual(aTunitsExpected, aTunits)
        Assert.AreEqual(aRetcodExpected, aRetcod)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test F90_WTFNDT</summary>
    <TestMethod()> Public Sub F90_WTFNDTTest()
        Dim aWdmUnit As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aWdmUnitExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aDsn As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aDsnExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aGpflg As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aGpflgExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aTdsfrc As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aTdsfrcExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aSDate() As Integer = Nothing ' TODO: Initialize to an appropriate value
        Dim aEDate() As Integer = Nothing ' TODO: Initialize to an appropriate value
        Dim aRetcod As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aRetcodExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Vb2F90.F90_WTFNDT(aWdmUnit, aDsn, aGpflg, aTdsfrc, aSDate, aEDate, aRetcod)
        Assert.AreEqual(aWdmUnitExpected, aWdmUnit)
        Assert.AreEqual(aDsnExpected, aDsn)
        Assert.AreEqual(aGpflgExpected, aGpflg)
        Assert.AreEqual(aTdsfrcExpected, aTdsfrc)
        Assert.AreEqual(aRetcodExpected, aRetcod)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test NumChr</summary>
    <TestMethod(), DeploymentItem("atcWDM.dll")> _
    Public Sub NumChrTest()
        Dim aLen As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aLenExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aIntStr() As Integer = Nothing ' TODO: Initialize to an appropriate value
        Dim aIntStrExpected() As Integer = Nothing ' TODO: Initialize to an appropriate value
        Dim aStr As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aStrExpected As String = String.Empty ' TODO: Initialize to an appropriate value
        Vb2F90_Accessor.NumChr(aLen, aIntStr, aStr)
        Assert.AreEqual(aLenExpected, aLen)
        Assert.AreEqual(aIntStrExpected, aIntStr)
        Assert.AreEqual(aStrExpected, aStr)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub
End Class
