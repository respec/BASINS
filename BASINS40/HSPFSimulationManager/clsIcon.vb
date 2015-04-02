Imports atcUtility
Imports MapWinUtility
Imports System.Collections.ObjectModel

Public Enum EnumLegendType
    LegLand = 0
    LegMet = 1
    LegPoint = 2
End Enum

Public Class clsUciScenario
    Implements IComparable

    Public ScenarioName As String
    Public UciFileName As String
    Private pUciFile As atcUCI.HspfUci

    Public Sub New(aScenarioSpecification As String, aBasePath As String)
        Dim lSplit() As String = aScenarioSpecification.Split("|")
        If lSplit.Length = 2 Then
            ScenarioName = lSplit(0).Trim
            UciFileName = IO.Path.Combine(aBasePath, lSplit(1).Trim)
        ElseIf lSplit.Length = 1 Then
            UciFileName = IO.Path.Combine(aBasePath, aScenarioSpecification.Trim)
            ScenarioName = IO.Path.GetFileNameWithoutExtension(UciFileName)
        Else
            Throw New ApplicationException("Expected one pipe delimiter in scenario specification: " & aScenarioSpecification)
        End If
    End Sub

    Public Overrides Function ToString() As String
        Return ScenarioName & " (" & UciFileName & ")"
    End Function

    Public Function CompareTo(obj As Object) As Integer Implements IComparable.CompareTo
        Dim lResult As Integer = 1
        If obj IsNot Nothing Then
            lResult = UciFileName.ToLower.CompareTo(obj.UciFileName.ToLower)
            If lResult = 0 Then
                lResult = ScenarioName.ToLower.CompareTo(obj.ScenarioName.ToLower)
            End If
        End If
        Return lResult
    End Function

    Public Overrides Function Equals(obj As Object) As Boolean
        Try
            Return obj IsNot Nothing AndAlso Not IsDBNull(obj) AndAlso _
                UciFileName.ToLower.Equals(obj.UciFileName.ToLower) AndAlso _
                ScenarioName.ToLower.Equals(obj.ScenarioName.ToLower)
        Catch
            Return False
        End Try
    End Function

    Public Function UciFile() As atcUCI.HspfUci
        If pUciFile Is Nothing OrElse pUciFile.Name <> UciFileName Then
            pUciFile = OpenUCI(UciFileName)
        End If
        Return pUciFile
    End Function
End Class

Public Class clsIcon
    Inherits Windows.Forms.Control

    Public Selected As Boolean
    Public Scenarios As New List(Of clsUciScenario)
    Private pScenario As clsUciScenario
    Public WatershedImage As Image
    Public WatershedImageFilename As String
    Public WatershedName As String = ""

    Public DownstreamIcon As clsIcon
    Public UpstreamIcons As New Generic.List(Of clsIcon)
    Public DistanceFromOutlet As Integer = -1

    Sub New()
        SetStyle(ControlStyles.DoubleBuffer Or ControlStyles.UserPaint Or ControlStyles.AllPaintingInWmPaint, True)
        UpdateStyles()
        Me.AllowDrop = True
        BackColor = Drawing.SystemColors.ButtonFace
    End Sub

    Public Overrides Function ToString() As String
        If Scenario Is Nothing Then
            Return WatershedName
        Else
            Return WatershedName & ": " & Scenario.ToString
        End If
    End Function

    Public Function Key() As String
        Return WatershedName.ToLowerInvariant()
    End Function

    Public Function Center() As Point
        Return New Point(Me.Left + Me.Width / 2, Me.Top + Me.Height / 2)
    End Function

    Public Property UciFileName() As String
        Get
            Return pScenario.UciFileName
        End Get
        Set(value As String)
            Dim lValue As String = value.ToLower
            For Each lScenario As clsUciScenario In Scenarios
                If lScenario.UciFileName.ToLower.Equals(lValue) Then
                    Scenario = lScenario
                    Exit Property
                End If
            Next
            Dim lScenarioName As String = IO.Path.GetFileNameWithoutExtension(value) ' InputBox("Enter Scenario Name for this UCI file", frmHspfSimulationManager.g_AppNameLong, IO.Path.GetFileNameWithoutExtension(value))
            Scenario = New clsUciScenario(lScenarioName & "|" & value, "")
        End Set
    End Property

    Public Property Scenario() As clsUciScenario
        Set(value As clsUciScenario)
            pScenario = value
            ScenariosAdd(value)
            If FileExists(value.UciFileName) Then
                Me.BackColor = Drawing.SystemColors.ButtonFace
            Else
                Me.BackColor = Drawing.Color.Salmon
            End If
        End Set
        Get
            Return pScenario
        End Get
    End Property

    Public Function ScenariosContains(aScenario As clsUciScenario)
        For Each lExistingScenario In Scenarios
            If lExistingScenario.Equals(aScenario) Then
                Return True
            End If
        Next
        Return False
    End Function

    Public Sub ScenariosAdd(aScenario As clsUciScenario)
        If aScenario IsNot Nothing Then
            If Not ScenariosContains(aScenario) Then
                Scenarios.Add(aScenario)
                If Scenario Is Nothing Then
                    Scenario = aScenario
                End If
            End If
        End If
    End Sub

    Public Function UciFile() As atcUCI.HspfUci
        Return Scenario.UciFile
    End Function

End Class

Friend Class IconCollection
    Inherits KeyedCollection(Of String, clsIcon)
    Protected Overrides Function GetKeyForItem(ByVal item As clsIcon) As String
        Return item.Key
    End Function

    Friend Function FindOrAddIcon(ByVal aWatershedName As String) As clsIcon
        Dim lIcon As clsIcon
        If Contains(aWatershedName.ToLowerInvariant) Then
            lIcon = Item(aWatershedName.ToLowerInvariant)
        Else
            lIcon = New clsIcon
            lIcon.WatershedName = aWatershedName
            Add(lIcon)
        End If
        Return lIcon
    End Function
End Class

Public Class PanelDoubleBuffer
    Inherits Panel
    Sub New()
        SetStyle(ControlStyles.DoubleBuffer Or ControlStyles.UserPaint Or ControlStyles.AllPaintingInWmPaint, True)
        Me.BackgroundImageLayout = ImageLayout.None
        UpdateStyles()
    End Sub
End Class
