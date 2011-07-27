Imports System.Collections
Imports atcUtility
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports atcData

'''<summary>
'''This is a test class for atcDataAttributesTest and is intended
'''to contain all atcDataAttributesTest Unit Tests (Done)
'''</summary>
<TestClass()> _
Public Class atcDataAttributesTest
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
    <ClassInitialize()> _
    Public Shared Sub MyClassInitialize(ByVal testContext As TestContext)
        Dim target As atcDataAttributes = New atcDataAttributes()
    End Sub
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

    '''<summary>Test atcDataAttributes Constructor</summary>
    <TestMethod()> Public Sub atcDataAttributesConstructorTest()
        Dim target As atcDataAttributes = New atcDataAttributes()
        Assert.IsInstanceOfType(atcDataAttributes.AllDefinitions, GetType(atcCollection))
        Assert.AreNotEqual(atcDataAttributes.AllDefinitions.Count, 0)
    End Sub

    '''<summary>Test Add</summary>
    <TestMethod()> Public Sub AddTest()
        Dim target As atcDataAttributes = New atcDataAttributes() ' TODO: Initialize to an appropriate value
        Dim aAttributeName As String = "Test1"
        Dim aAttributeValue As Object = "Test1Value"

        Dim expected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        actual = target.Add(aAttributeName, aAttributeValue)
        Assert.AreEqual(expected, actual)

    End Sub

    '''<summary>Test Add</summary>
    <TestMethod()> Public Sub AddTest1()
        Dim target As atcDataAttributes = New atcDataAttributes() ' TODO: Initialize to an appropriate value
        Dim aDefinedValue As New atcDefinedValue
        Dim latcAttributeDef As New atcAttributeDefinition
        latcAttributeDef.Name = "Test1"
        aDefinedValue.Definition = latcAttributeDef
        aDefinedValue.Value = "Test1Value"
        Dim expected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        actual = target.Add(aDefinedValue)
        Assert.AreEqual(expected, actual)
    End Sub

    '''<summary>Test AddDefinition</summary>
    <TestMethod()> Public Sub AddDefinitionTest()

        Dim aDefinition As New atcAttributeDefinition
        aDefinition.Name = "Test1"

        Dim expected As atcAttributeDefinition = aDefinition
        Assert.AreEqual(atcDataAttributes.AddDefinition(aDefinition), expected)
    End Sub

    '''<summary>Test AddHistory</summary>
    <TestMethod()> Public Sub AddHistoryTest()
        Dim target As atcDataAttributes = New atcDataAttributes()
        Dim aNewEvent As String = IO.Path.Combine(Environment.SystemDirectory, "cmd.exe")
        target.AddHistory(aNewEvent)
        Assert.AreEqual(target(0).Value, aNewEvent)
    End Sub

    '''<summary>Test AllDefinitions</summary>
    <TestMethod()> Public Sub AllDefinitionsTest()
        Dim target As atcDataAttributes = New atcDataAttributes()
        Assert.IsInstanceOfType(atcDataAttributes.AllDefinitions, GetType(atcCollection))
    End Sub

    '''<summary>Test AttributeNameToKey</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub AttributeNameToKeyTest()
        Dim target As atcDataAttributes = New atcDataAttributes()
        Dim aAttributeName As String = "ID"
        Assert.AreEqual(atcDataAttributes_Accessor.AttributeNameToKey(aAttributeName).ToUpper, "ID")
    End Sub

    '''<summary>Test CalculateAll</summary>
    <TestMethod()> Public Sub CalculateAllTest()
        Dim target As atcDataAttributes = New atcDataAttributes() ' TODO: Initialize to an appropriate value
        Try
            target.CalculateAll()
            Assert.AreEqual(True, True)
        Catch ex As Exception
            Assert.AreEqual(True, False)
        End Try
    End Sub

    '''<summary>Test ChangeTo</summary>
    <TestMethod()> Public Sub ChangeToTest()
        Dim target As atcDataAttributes = New atcDataAttributes() ' TODO: Initialize to an appropriate value
        Dim aNewItems As New atcDataAttributes
        Dim lNewDef As New atcAttributeDefinition
        lNewDef.Name = "Test1"
        Dim lNewDefValue As New atcDefinedValue
        lNewDefValue.Definition = lNewDef
        aNewItems.Add(lNewDefValue)
        aNewItems(0).Value = "Test1Value"

        target.ChangeTo(aNewItems)
        Assert.AreEqual(target(0).Definition.Name, "Test1")
        Assert.AreEqual(target(0).Value, "Test1Value")
    End Sub

    '''<summary>Test Clone</summary>
    <TestMethod()> Public Sub CloneTest()
        Dim target As atcDataAttributes = New atcDataAttributes() ' TODO: Initialize to an appropriate value
        Dim lNewDef As New atcAttributeDefinition
        lNewDef.Name = "Test1"
        Dim lNewDefValue As New atcDefinedValue
        lNewDefValue.Definition = lNewDef
        target.Add(lNewDefValue)
        target(0).Value = "Test1Value"

        Dim lClone As atcDataAttributes = target.Clone
        Assert.AreEqual(target.Count, lClone.Count)
        Assert.AreEqual(target(0).Definition.Name, lClone(0).Definition.Name)
        Assert.AreEqual(target(0).Value, lClone(0).Value)
    End Sub

    '''<summary>Test ContainsAttribute</summary>
    <TestMethod()> Public Sub ContainsAttributeTest()
        Dim target As atcDataAttributes = New atcDataAttributes() ' TODO: Initialize to an appropriate value
        Dim lNewDef As New atcAttributeDefinition
        lNewDef.Name = "Test1"
        Dim lNewDefValue As New atcDefinedValue
        lNewDefValue.Definition = lNewDef
        target.Add(lNewDefValue)
        target(0).Value = "Test1Value"

        Assert.AreEqual(target.ContainsAttribute("Test1"), True)
        Assert.AreEqual(target.ContainsAttribute("Test2"), False)
    End Sub

    '''<summary>Test DiscardCalculated</summary>
    <TestMethod()> Public Sub DiscardCalculatedTest()
        Dim target As atcDataAttributes = New atcDataAttributes() ' TODO: Initialize to an appropriate value
        Dim lNewDef As New atcAttributeDefinition
        lNewDef.Name = "Test1"
        lNewDef.Calculator = New atcTimeseriesSource()
        Dim lNewDefValue As New atcDefinedValue
        lNewDefValue.Definition = lNewDef
        target.Add(lNewDefValue)
        target(0).Value = "Test1Value"

        Assert.AreEqual(target.ContainsAttribute("Test1"), True)
        target.DiscardCalculated()
        Assert.AreEqual(target.ContainsAttribute("Test1"), False)
    End Sub

    '''<summary>Test GetDefinedValue</summary>
    <TestMethod()> Public Sub GetDefinedValueTest()
        Dim target As atcDataAttributes = New atcDataAttributes() ' TODO: Initialize to an appropriate value
        Dim lNewDef As New atcAttributeDefinition
        lNewDef.Name = "Test1"
        Dim lNewDefValue As New atcDefinedValue
        lNewDefValue.Definition = lNewDef
        target.Add(lNewDefValue)
        target(0).Value = "Test1Value"

        Assert.AreEqual(target.GetDefinedValue("Test1"), lNewDefValue)

    End Sub

    '''<summary>Test GetDefinition</summary>
    <TestMethod()> Public Sub GetDefinitionTest()
        Dim target As atcDataAttributes = New atcDataAttributes()
        Dim lNewDef As New atcAttributeDefinition
        lNewDef.Name = "Test1"
        atcDataAttributes.AddDefinition(lNewDef)

        Assert.AreEqual(atcDataAttributes.GetDefinition("Test1"), lNewDef)
        Assert.AreEqual(atcDataAttributes.GetDefinition("Test2", False), Nothing)
        Assert.AreNotEqual(atcDataAttributes.GetDefinition("Test2", True), Nothing)
    End Sub

    '''<summary>Test GetFormattedValue</summary>
    <TestMethod()> Public Sub GetFormattedValueTest()
        Dim target As atcDataAttributes = New atcDataAttributes() ' TODO: Initialize to an appropriate value
        Dim lNewDef As New atcAttributeDefinition
        lNewDef.Name = "Test1"
        Dim lNewDefValue As New atcDefinedValue
        lNewDefValue.Definition = lNewDef
        target.Add(lNewDefValue)
        target(0).Value = "Test1Value"

        Assert.AreEqual(target.GetFormattedValue("Test1"), "Test1Value")
    End Sub

    '''<summary>Test GetValue</summary>
    <TestMethod()> Public Sub GetValueTest()
        Dim target As atcDataAttributes = New atcDataAttributes() ' TODO: Initialize to an appropriate value
        Dim aAttributeName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim lNewDef As New atcAttributeDefinition
        lNewDef.Name = "Test1"
        Dim lNewDefValue As New atcDefinedValue
        lNewDefValue.Definition = lNewDef
        target.Add(lNewDefValue)
        target(0).Value = "Test1Value"
        Assert.AreEqual(target.GetValue("Test1").ToString, "Test1Value")
        target(0).Value = New atcTimeseries(Nothing)
        Assert.IsInstanceOfType(target(0).Value, GetType(atcTimeseries))
    End Sub

    '''<summary>Test IsSimple</summary>
    <TestMethod()> Public Sub IsSimpleTest()
        Dim target As atcDataAttributes = New atcDataAttributes()
        Dim aAttributeName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim lNewDef As New atcAttributeDefinition
        lNewDef.Name = "Test1"
        Dim lNewDefValue As New atcDefinedValue
        lNewDefValue.Definition = lNewDef
        target.Add(lNewDefValue)
        target(0).Value = "Test1Value"
        Assert.AreEqual(atcDataAttributes.IsSimple(lNewDef, "Test1", lNewDefValue), True)
    End Sub

    '''<summary>Test PreferredName</summary>
    <TestMethod()> Public Sub PreferredNameTest()
        Dim target As atcDataAttributes = New atcDataAttributes()
        Dim aAttributeName As String = "ID"
        Dim aAttributeNameExpected As String = "ID"
        Assert.AreEqual(atcDataAttributes.PreferredName(aAttributeName), aAttributeNameExpected)
        aAttributeName = "tu"
        aAttributeNameExpected = "Time Unit"
        Assert.AreEqual(atcDataAttributes.PreferredName(aAttributeName), aAttributeNameExpected)
    End Sub

    '''<summary>Test RemoveByKey</summary>
    <TestMethod()> Public Sub RemoveByKeyTest()
        Dim target As atcDataAttributes = New atcDataAttributes() ' TODO: Initialize to an appropriate value
        Dim lNewDef As New atcAttributeDefinition
        lNewDef.Name = "Test1"
        Dim lNewDefValue As New atcDefinedValue
        lNewDefValue.Definition = lNewDef
        target.Add(lNewDefValue)
        target(0).Value = "Test1Value"

        Assert.AreEqual(target.ContainsAttribute("Test1"), True)
        target.RemoveByKey("Test1")
        Assert.AreEqual(target.ContainsAttribute("Test1"), False)
    End Sub

    '''<summary>Test SetValue</summary>
    <TestMethod()> Public Sub SetValueTest()
        Dim target As atcDataAttributes = New atcDataAttributes() ' TODO: Initialize to an appropriate value
        Dim lNewDef As New atcAttributeDefinition
        lNewDef.Name = "Test1"
        Dim lNewDefValue As New atcDefinedValue
        lNewDefValue.Definition = lNewDef
        target.Add(lNewDefValue)
        target(0).Value = "Test1Value"

        Assert.AreEqual(target(0).Value, "Test1Value")
        target.SetValue("Test1", "TestJustSetNew")
        Assert.AreEqual(target(0).Value, "TestJustSetNew")
    End Sub

    '''<summary>Test SetValue</summary>
    <TestMethod()> Public Sub SetValueTest1()
        Dim target As atcDataAttributes = New atcDataAttributes() ' TODO: Initialize to an appropriate value
        Dim lNewDef As New atcAttributeDefinition
        lNewDef.Name = "Test1"
        Dim lNewDefValue As New atcDefinedValue
        lNewDefValue.Definition = lNewDef
        target.Add(lNewDefValue)
        target(0).Value = "Test1Value"

        Assert.AreEqual(target(0).Value, "Test1Value")
        target.SetValue(lNewDef, "TestJustSetNew")
        Assert.AreEqual(target(0).Value, "TestJustSetNew")
    End Sub

    '''<summary>Test SetValueIfMissing</summary>
    <TestMethod()> Public Sub SetValueIfMissingTest()
        Dim target As atcDataAttributes = New atcDataAttributes() ' TODO: Initialize to an appropriate value
        Dim lNewDef As New atcAttributeDefinition
        lNewDef.Name = "Test1"
        Dim lNewDefValue As New atcDefinedValue
        lNewDefValue.Definition = lNewDef
        target.Add(lNewDefValue)
        target(0).Value = "Test1Value"

        Assert.AreEqual(target(0).Value, "Test1Value")
        target.SetValueIfMissing("Test1", "TestJustSetNewAgain")
        Assert.AreNotEqual(target(0).Value, "TestJustSetNewAgain")
        target.RemoveByKey("Test1")
        Assert.AreEqual(target.ContainsAttribute("Test1"), False)
        target.SetValueIfMissing("Test1", "TestJustSetNewAgain")
        Assert.AreEqual(target(0).Value, "TestJustSetNewAgain")
    End Sub

    '''<summary>Test ToString</summary>
    <TestMethod()> Public Sub ToStringTest()
        Dim target As atcDataAttributes = New atcDataAttributes() ' TODO: Initialize to an appropriate value
        Assert.AreEqual(target.ToString.Length, 0)
        Dim lNewDef As New atcAttributeDefinition
        lNewDef.Name = "Test1"
        Dim lNewDefValue As New atcDefinedValue
        lNewDefValue.Definition = lNewDef
        target.Add(lNewDefValue)
        target(0).Value = "Test1Value"
        Assert.AreNotEqual(target.ToString.Length, 0)
    End Sub

    '''<summary>Test ValuesSortedByName</summary>
    <TestMethod()> Public Sub ValuesSortedByNameTest()
        Dim target As atcDataAttributes = New atcDataAttributes() ' TODO: Initialize to an appropriate value
        Dim lNewDef2 As New atcAttributeDefinition
        lNewDef2.Name = "Test2"
        Dim lNewDefValue2 As New atcDefinedValue
        lNewDefValue2.Definition = lNewDef2
        target.Add(lNewDefValue2)
        target(0).Value = "Test2Value"

        Dim lNewDef As New atcAttributeDefinition
        lNewDef.Name = "Test1"
        Dim lNewDefValue As New atcDefinedValue
        lNewDefValue.Definition = lNewDef
        target.Add(lNewDefValue)
        target(1).Value = "Test1Value"

        Assert.AreNotEqual(target(0).Definition.Name, "Test1")
        Assert.AreEqual(target(1).Value, "Test1Value")

        Dim lSL As SortedList = target.ValuesSortedByName()
        For Each lKey As String In lSL.Keys
            Assert.AreEqual(lSL(lKey), "Test1Value")
            Exit For
        Next
    End Sub

    '''<summary>Test Item</summary>
    <TestMethod()> Public Sub ItemTest()
        Dim target As atcDataAttributes = New atcDataAttributes() ' TODO: Initialize to an appropriate value
        Assert.AreEqual(target.Item(0), Nothing)
        Dim lNewDef As New atcAttributeDefinition
        lNewDef.Name = "Test1"
        Dim lNewDefValue As New atcDefinedValue
        lNewDefValue.Definition = lNewDef
        target.Add(lNewDefValue)
        target(0).Value = "Test1Value"
        Assert.AreEqual(target.Item(0).Definition.Name, "Test1")
    End Sub

    '''<summary>Test ItemByIndex</summary>
    <TestMethod()> Public Sub ItemByIndexTest()
        Dim target As atcDataAttributes = New atcDataAttributes() ' TODO: Initialize to an appropriate value
        Dim lO As atcDefinedValue = Nothing
        Try
            lO = target.ItemByIndex(0)
        Catch ex As Exception
            'Yup, it is not checking for size first
            Assert.AreEqual(lO, Nothing)
        End Try

        Dim lNewDef As New atcAttributeDefinition
        lNewDef.Name = "Test1"
        Dim lNewDefValue As New atcDefinedValue
        lNewDefValue.Definition = lNewDef
        target.Add(lNewDefValue)
        target(0).Value = "Test1Value"
        Assert.AreEqual(target.ItemByIndex(0).Definition.Name, "Test1")
    End Sub

    '''<summary>Test Owner</summary>
    <TestMethod()> Public Sub OwnerTest()
        Dim target As atcDataAttributes = New atcDataAttributes() ' TODO: Initialize to an appropriate value
        Dim expected As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Object
        target.Owner = expected
        actual = target.Owner
        Assert.AreEqual(expected, actual)
    End Sub
End Class
