Imports System.Drawing

Imports System.Windows.Forms

Imports System

Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports atcControls



'''<summary>
'''This is a test class for atcTextTest and is intended
'''to contain all atcTextTest Unit Tests
'''</summary>
<TestClass()> _
Public Class atcTextTest


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
    '''A test for atcText Constructor
    '''</summary>
    <TestMethod()> _
    Public Sub atcTextConstructorTest()
        Dim target As atcText = New atcText()
        Assert.Inconclusive("TODO: Implement code to verify target")
    End Sub

    '''<summary>
    '''A test for ATCoTypeString
    '''</summary>
    <TestMethod()> _
    Public Sub ATCoTypeStringTest()
        Dim target As atcText = New atcText() ' TODO: Initialize to an appropriate value
        Dim DataType As atcText.ATCoDataType = New atcText.ATCoDataType() ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = target.ATCoTypeString(DataType)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for AboveLimit
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcControls.dll")> _
    Public Sub AboveLimitTest()
        Dim target As atcText_Accessor = New atcText_Accessor() ' TODO: Initialize to an appropriate value
        Dim testVal As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim LimitVal As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = target.AboveLimit(testVal, LimitVal)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for BelowLimit
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcControls.dll")> _
    Public Sub BelowLimitTest()
        Dim target As atcText_Accessor = New atcText_Accessor() ' TODO: Initialize to an appropriate value
        Dim testVal As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim LimitVal As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = target.BelowLimit(testVal, LimitVal)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for Dispose
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcControls.dll")> _
    Public Sub DisposeTest()
        Dim target As atcText_Accessor = New atcText_Accessor() ' TODO: Initialize to an appropriate value
        Dim disposing As Boolean = False ' TODO: Initialize to an appropriate value
        target.Dispose(disposing)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for FormatValue
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcControls.dll")> _
    Public Sub FormatValueTest()
        Dim target As atcText_Accessor = New atcText_Accessor() ' TODO: Initialize to an appropriate value
        Dim Val As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = target.FormatValue(Val)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for InitializeComponent
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcControls.dll")> _
    Public Sub InitializeComponentTest()
        Dim target As atcText_Accessor = New atcText_Accessor() ' TODO: Initialize to an appropriate value
        target.InitializeComponent()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for SetColorFromDialog
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcControls.dll")> _
    Public Sub SetColorFromDialogTest()
        Dim target As atcText_Accessor = New atcText_Accessor() ' TODO: Initialize to an appropriate value
        target.SetColorFromDialog()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for SetToolTip
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcControls.dll")> _
    Public Sub SetToolTipTest()
        Dim target As atcText_Accessor = New atcText_Accessor() ' TODO: Initialize to an appropriate value
        target.SetToolTip()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for ShowRange
    '''</summary>
    <TestMethod()> _
    Public Sub ShowRangeTest()
        Dim target As atcText = New atcText() ' TODO: Initialize to an appropriate value
        target.ShowRange()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for UserControl_EnterFocus
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcControls.dll")> _
    Public Sub UserControl_EnterFocusTest()
        Dim target As atcText_Accessor = New atcText_Accessor() ' TODO: Initialize to an appropriate value
        target.UserControl_EnterFocus()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for UserControl_InitProperties
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcControls.dll")> _
    Public Sub UserControl_InitPropertiesTest()
        Dim target As atcText_Accessor = New atcText_Accessor() ' TODO: Initialize to an appropriate value
        target.UserControl_InitProperties()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for UserControl_Initialize
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcControls.dll")> _
    Public Sub UserControl_InitializeTest()
        Dim target As atcText_Accessor = New atcText_Accessor() ' TODO: Initialize to an appropriate value
        target.UserControl_Initialize()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for UserControl_Resize
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcControls.dll")> _
    Public Sub UserControl_ResizeTest()
        Dim target As atcText_Accessor = New atcText_Accessor() ' TODO: Initialize to an appropriate value
        target.UserControl_Resize()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for Valid
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcControls.dll")> _
    Public Sub ValidTest()
        Dim target As atcText_Accessor = New atcText_Accessor() ' TODO: Initialize to an appropriate value
        Dim aValue As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = target.Valid(aValue)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for __ENCAddToList
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcControls.dll")> _
    Public Sub __ENCAddToListTest()
        Dim value As Object = Nothing ' TODO: Initialize to an appropriate value
        atcText_Accessor.__ENCAddToList(value)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for txtBox_GotFocus
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcControls.dll")> _
    Public Sub txtBox_GotFocusTest()
        Dim target As atcText_Accessor = New atcText_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.txtBox_GotFocus(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for txtBox_KeyDown
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcControls.dll")> _
    Public Sub txtBox_KeyDownTest()
        Dim target As atcText_Accessor = New atcText_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As KeyEventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.txtBox_KeyDown(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for txtBox_LostFocus
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcControls.dll")> _
    Public Sub txtBox_LostFocusTest()
        Dim target As atcText_Accessor = New atcText_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.txtBox_LostFocus(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for txtBox_MouseDown
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcControls.dll")> _
    Public Sub txtBox_MouseDownTest()
        Dim target As atcText_Accessor = New atcText_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As MouseEventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.txtBox_MouseDown(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for txtBox_TextChanged
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcControls.dll")> _
    Public Sub txtBox_TextChangedTest()
        Dim target As atcText_Accessor = New atcText_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.txtBox_TextChanged(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for Alignment
    '''</summary>
    <TestMethod()> _
    Public Sub AlignmentTest()
        Dim target As atcText = New atcText() ' TODO: Initialize to an appropriate value
        Dim expected As HorizontalAlignment = New HorizontalAlignment() ' TODO: Initialize to an appropriate value
        Dim actual As HorizontalAlignment
        target.Alignment = expected
        actual = target.Alignment
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for DataType
    '''</summary>
    <TestMethod()> _
    Public Sub DataTypeTest()
        Dim target As atcText = New atcText() ' TODO: Initialize to an appropriate value
        Dim expected As atcText.ATCoDataType = New atcText.ATCoDataType() ' TODO: Initialize to an appropriate value
        Dim actual As atcText.ATCoDataType
        target.DataType = expected
        actual = target.DataType
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for DefaultValue
    '''</summary>
    <TestMethod()> _
    Public Sub DefaultValueTest()
        Dim target As atcText = New atcText() ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        target.DefaultValue = expected
        actual = target.DefaultValue
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for Enabled
    '''</summary>
    <TestMethod()> _
    Public Sub EnabledTest()
        Dim target As atcText = New atcText() ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        target.Enabled = expected
        actual = target.Enabled
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for ForeColor
    '''</summary>
    <TestMethod()> _
    Public Sub ForeColorTest()
        Dim target As atcText = New atcText() ' TODO: Initialize to an appropriate value
        Dim expected As Color = New Color() ' TODO: Initialize to an appropriate value
        Dim actual As Color
        target.ForeColor = expected
        actual = target.ForeColor
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for HardMax
    '''</summary>
    <TestMethod()> _
    Public Sub HardMaxTest()
        Dim target As atcText = New atcText() ' TODO: Initialize to an appropriate value
        Dim expected As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim actual As Double
        target.HardMax = expected
        actual = target.HardMax
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for HardMin
    '''</summary>
    <TestMethod()> _
    Public Sub HardMinTest()
        Dim target As atcText = New atcText() ' TODO: Initialize to an appropriate value
        Dim expected As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim actual As Double
        target.HardMin = expected
        actual = target.HardMin
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for InsideLimitsBackground
    '''</summary>
    <TestMethod()> _
    Public Sub InsideLimitsBackgroundTest()
        Dim target As atcText = New atcText() ' TODO: Initialize to an appropriate value
        Dim expected As Color = New Color() ' TODO: Initialize to an appropriate value
        Dim actual As Color
        target.InsideLimitsBackground = expected
        actual = target.InsideLimitsBackground
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for MaxWidth
    '''</summary>
    <TestMethod()> _
    Public Sub MaxWidthTest()
        Dim target As atcText = New atcText() ' TODO: Initialize to an appropriate value
        Dim expected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        target.MaxWidth = expected
        actual = target.MaxWidth
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for NumericFormat
    '''</summary>
    <TestMethod()> _
    Public Sub NumericFormatTest()
        Dim target As atcText = New atcText() ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        target.NumericFormat = expected
        actual = target.NumericFormat
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for OutsideHardLimitBackground
    '''</summary>
    <TestMethod()> _
    Public Sub OutsideHardLimitBackgroundTest()
        Dim target As atcText = New atcText() ' TODO: Initialize to an appropriate value
        Dim expected As Color = New Color() ' TODO: Initialize to an appropriate value
        Dim actual As Color
        target.OutsideHardLimitBackground = expected
        actual = target.OutsideHardLimitBackground
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for OutsideSoftLimitBackground
    '''</summary>
    <TestMethod()> _
    Public Sub OutsideSoftLimitBackgroundTest()
        Dim target As atcText = New atcText() ' TODO: Initialize to an appropriate value
        Dim expected As Color = New Color() ' TODO: Initialize to an appropriate value
        Dim actual As Color
        target.OutsideSoftLimitBackground = expected
        actual = target.OutsideSoftLimitBackground
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for SelLength
    '''</summary>
    <TestMethod()> _
    Public Sub SelLengthTest()
        Dim target As atcText = New atcText() ' TODO: Initialize to an appropriate value
        Dim expected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        target.SelLength = expected
        actual = target.SelLength
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for SelStart
    '''</summary>
    <TestMethod()> _
    Public Sub SelStartTest()
        Dim target As atcText = New atcText() ' TODO: Initialize to an appropriate value
        Dim expected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        target.SelStart = expected
        actual = target.SelStart
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for SoftMax
    '''</summary>
    <TestMethod()> _
    Public Sub SoftMaxTest()
        Dim target As atcText = New atcText() ' TODO: Initialize to an appropriate value
        Dim expected As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim actual As Double
        target.SoftMax = expected
        actual = target.SoftMax
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for SoftMin
    '''</summary>
    <TestMethod()> _
    Public Sub SoftMinTest()
        Dim target As atcText = New atcText() ' TODO: Initialize to an appropriate value
        Dim expected As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim actual As Double
        target.SoftMin = expected
        actual = target.SoftMin
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for Text
    '''</summary>
    <TestMethod()> _
    Public Sub TextTest()
        Dim target As atcText = New atcText() ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        target.Text = expected
        actual = target.Text
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for ToolTip1
    '''</summary>
    <TestMethod()> _
    Public Sub ToolTip1Test()
        Dim target As atcText = New atcText() ' TODO: Initialize to an appropriate value
        Dim expected As ToolTip = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ToolTip
        target.ToolTip1 = expected
        actual = target.ToolTip1
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for ValueDouble
    '''</summary>
    <TestMethod()> _
    Public Sub ValueDoubleTest()
        Dim target As atcText = New atcText() ' TODO: Initialize to an appropriate value
        Dim expected As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim actual As Double
        target.ValueDouble = expected
        actual = target.ValueDouble
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for ValueInteger
    '''</summary>
    <TestMethod()> _
    Public Sub ValueIntegerTest()
        Dim target As atcText = New atcText() ' TODO: Initialize to an appropriate value
        Dim expected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        target.ValueInteger = expected
        actual = target.ValueInteger
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for txtBox
    '''</summary>
    <TestMethod()> _
    Public Sub txtBoxTest()
        Dim target As atcText = New atcText() ' TODO: Initialize to an appropriate value
        Dim expected As TextBox = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As TextBox
        target.txtBox = expected
        actual = target.txtBox
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub
End Class
