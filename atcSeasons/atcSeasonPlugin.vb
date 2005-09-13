Imports atcdata
Imports atcUtility

Imports System.Reflection

Public Class atcSeasonPlugin
  Inherits atcDataSource

  Private pAvailableOperations As atcDataAttributes ' atcDataGroup
  Private pName As String = "Timeseries::Seasons"
  Private pSeasons As atcSeasonBase

  Public Overrides ReadOnly Property Name() As String
    Get
      If pSeasons Is Nothing Then
        Return "Timeseries::Seasonal"
      Else
        Return pSeasons.Name
      End If
    End Get
  End Property

  Public Overrides ReadOnly Property Category() As String
    Get
      Return "Seasons"
    End Get
  End Property

  Public Overrides ReadOnly Property Description() As String
    Get
      Return Name
    End Get
  End Property

  Public Overrides ReadOnly Property CanOpen() As Boolean
    Get
      Return True
    End Get
  End Property

  'The only element of aArgs is an atcDataGroup or atcTimeseries
  'The attribute(s) will be set to the result(s) of calculation(s)
  Public Overrides Function Open(ByVal aOperationName As String, Optional ByVal aArgs As atcDataAttributes = Nothing) As Boolean
    Dim lSeasonName As String = StrSplit(aOperationName, "::", "")
    Dim a As [Assembly] = Reflection.Assembly.GetExecutingAssembly
    Dim lassyTypes As Type() = a.GetTypes()
    For Each typ As Type In lassyTypes
      If typ.Name.Substring(10).ToLower.Equals(lSeasonName.ToLower) Then
        pSeasons = typ.InvokeMember(Nothing, Reflection.BindingFlags.CreateInstance, Nothing, Nothing, New Object() {})
        Exit For
      End If
    Next
    If pSeasons Is Nothing Then
      LogDbg("Could not create season of type '" & lSeasonName & "'")
    Else
      Dim ltsGroup As atcDataGroup
      If aArgs Is Nothing Then
        ltsGroup = DataManager.UserSelectData("Select data for " & aOperationName)
      Else
        Try
          ltsGroup = aArgs.GetValue("Timeseries")
        Catch
        End Try
        If ltsGroup Is Nothing Then
          Dim lts As atcTimeseries
          Try
            lts = aArgs.GetValue("Timeseries")
            If Not lts Is Nothing Then
              ltsGroup = New atcDataGroup(lts)
            End If
          Catch
          End Try
        End If
      End If
      If Not ltsGroup Is Nothing Then
        Select Case aOperationName.ToLower
          Case "split"
            For Each lts As atcTimeseries In ltsGroup
              MyBase.DataSets.AddRange(pSeasons.Split(lts, Me))
            Next
            If MyBase.DataSets.Count > 0 Then Return True
          Case "seasonalattributes"
            Dim lAttributes As atcDataAttributes = aArgs.GetValue("Attributes")
            Dim lCalculatedAttributes As atcDataAttributes = aArgs.GetValue("CalculatedAttributes")
            If lAttributes Is Nothing Then
              Dim lForm As New frmSpecifySeasonalAttributes
              lAttributes = lForm.AskUser(ltsGroup, AvailableOperations)
            End If
            If lAttributes Is Nothing Then
              LogDbg("No seasonal attributes found to calculate")
            Else
              For Each lts As atcTimeseries In ltsGroup
                pSeasons.SetSeasonalAttributes(lts, lAttributes, lCalculatedAttributes)
              Next
            End If
            Return True
        End Select
      End If
    End If
  End Function

  Public Overrides Sub Initialize(ByVal MapWin As MapWindow.Interfaces.IMapWin, ByVal ParentHandle As Integer)
  End Sub

  Public Overrides Function ToString() As String
    Return Name.Substring(23) 'Skip first part of Name which is "Timeseries::Seasonal - "
  End Function

  Public Overrides ReadOnly Property AvailableOperations() As atcDataAttributes
    Get
      If pAvailableOperations Is Nothing Then
        pAvailableOperations = New atcDataAttributes

        Dim defTimeSeriesOne As New atcAttributeDefinition
        With defTimeSeriesOne
          .Name = "Timeseries"
          .Description = "One time series"
          .Editable = True
          .TypeString = "atcTimeseries"
        End With

        Dim a As [Assembly] = Reflection.Assembly.GetExecutingAssembly
        Dim lassyTypes As Type() = a.GetTypes()
        For Each typ As Type In lassyTypes
          If typ.Name.StartsWith("atcSeasons") Then

            Dim lSeasonalAttributes As New atcAttributeDefinition
            With lSeasonalAttributes
              .Name = typ.Name & " Seasonal Attributes"
              .Category = "" 'Me.ToString
              .Description = "Attribute values calculated per season"
              .Editable = False
              .TypeString = "atcDataAttributes"
              .Calculator = Me
            End With
            Dim lArguments As atcDataAttributes = New atcDataAttributes
            lArguments.SetValue(defTimeSeriesOne, Nothing)
            lArguments.SetValue("Attributes", Nothing)

            pAvailableOperations.SetValue(lSeasonalAttributes, Nothing, lArguments)

            Dim lSeasonalSplit As New atcAttributeDefinition
            With lSeasonalAttributes
              .Name = typ.Name.Substring(10) & "::Split"
              .Category = "Split"
              .Description = "Split a timeseries by season"
              .Editable = False
              .TypeString = "atcDataGroup"
              .Calculator = Me
            End With

            lArguments = New atcDataAttributes
            lArguments.SetValue(defTimeSeriesOne, Nothing)

            pAvailableOperations.SetValue(lSeasonalSplit, Nothing, lArguments)

          End If
        Next

      End If
      Return pAvailableOperations
    End Get
  End Property
End Class