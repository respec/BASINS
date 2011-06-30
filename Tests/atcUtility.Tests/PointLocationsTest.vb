Imports System.Collections.Generic
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports atcUtility

'''<summary>
'''This is a test class for PointLocationsTest and is intended
'''to contain all PointLocationsTest Unit Tests
'''</summary>
<TestClass()> _
Public Class PointLocationsTest
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

    Friend Overridable Function CreatePointLocations() As PointLocations
        'TODO: Instantiate an appropriate concrete class.
        Dim target As PointLocations = Nothing
        Return target
    End Function

    '''<summary>Test AddLocation</summary>
    <TestMethod()> Public Sub AddLocationTest()
        Dim target As PointLocations = CreatePointLocations() ' TODO: Initialize to an appropriate value
        Dim aTable As atcTable = Nothing ' TODO: Initialize to an appropriate value
        target.AddLocation(aTable)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test Closest</summary>
    <TestMethod()> Public Sub ClosestTest()
        Dim target As PointLocations = CreatePointLocations() ' TODO: Initialize to an appropriate value
        Dim aLatitude As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim aLongitude As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim aMaxCount As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim expected As SortedList(Of Double, PointLocation) = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As SortedList(Of Double, PointLocation)
        actual = target.Closest(aLatitude, aLongitude, aMaxCount)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    Friend Overridable Function CreatePointLocations_Accessor() As PointLocations_Accessor
        'TODO: Instantiate an appropriate concrete class.
        Dim target As PointLocations_Accessor = Nothing
        Return target
    End Function

    '''<summary>Test Delimeter</summary>
    <TestMethod(), DeploymentItem("atcUtility.dll")> _
    Public Sub DelimeterTest()
        Dim param0 As PrivateObject = Nothing ' TODO: Initialize to an appropriate value
        Dim target As PointLocations_Accessor = New PointLocations_Accessor(param0) ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = target.Delimeter
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test GetKeyForItem</summary>
    <TestMethod(), DeploymentItem("atcUtility.dll")> _
    Public Sub GetKeyForItemTest()
        Dim param0 As PrivateObject = Nothing ' TODO: Initialize to an appropriate value
        Dim target As PointLocations_Accessor = New PointLocations_Accessor(param0) ' TODO: Initialize to an appropriate value
        Dim aPointLocation As PointLocation = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = target.GetKeyForItem(aPointLocation)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test InternalFilename</summary>
    <TestMethod(), DeploymentItem("atcUtility.dll")> _
    Public Sub InternalFilenameTest()
        'Private Accessor for InternalFilename is not found. Please rebuild the containing project or run the Publicize.exe manually.
        Assert.Inconclusive("Private Accessor for InternalFilename is not found. Please rebuild the containing" & _
                " project or run the Publicize.exe manually.")
    End Sub
End Class
