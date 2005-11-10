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
  Public Overrides Function Open(ByVal aOperationName As String, _
                        Optional ByVal aArgs As atcDataAttributes = Nothing) As Boolean
    Dim lSeasonName As String
    Dim ltsGroup As atcDataGroup

    If Not aArgs Is Nothing Then
      ltsGroup = DatasetOrGroupToGroup(aArgs.GetValue("Timeseries"))
    End If
    If ltsGroup Is Nothing Then
      ltsGroup = DataManager.UserSelectData("Select data for " & aOperationName)
    End If
    If Not ltsGroup Is Nothing Then
      If aOperationName.IndexOf("::") > 0 Then
        Dim a As [Assembly] = Reflection.Assembly.GetExecutingAssembly
        Dim lassyTypes As Type() = a.GetTypes()
        lSeasonName = StrSplit(aOperationName, "::", "")
        For Each typ As Type In lassyTypes
          If SeasonClassNameToLabel(typ.Name).Equals(lSeasonName) Then
            Try
              pSeasons = typ.InvokeMember(Nothing, Reflection.BindingFlags.CreateInstance, Nothing, Nothing, New Object() {})
            Catch e As Exception
              LogDbg("Exception creating " & lSeasonName & ": " & e.Message)
            End Try
            Exit For
          End If
        Next
      Else
        'TODO: use form to ask for seasons
        pSeasons = New atcSeasonsMonth
      End If
      If pSeasons Is Nothing Then
        LogDbg("Could not create season of type '" & lSeasonName & "'")
      Else
        Select Case aOperationName.ToLower
          Case "split"
            For Each lts As atcTimeseries In ltsGroup
              MyBase.DataSets.AddRange(pSeasons.Split(lts, Me))
            Next
            If MyBase.DataSets.Count > 0 Then Return True
          Case "seasonalattributes"
            Dim lAttributes As atcDataAttributes = aArgs.GetValue("Attributes")
            Dim lCalculatedAttributes As atcDataAttributes = aArgs.GetValue("CalculatedAttributes")
            If lAttributes Is Nothing OrElse lAttributes.Count = 0 Then
              Dim lForm As New frmSpecifySeasonalAttributes
              lForm.AskUser(ltsGroup, AvailableOperations(False, True))
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

  Public Overrides Sub Initialize(ByVal MapWin As MapWindow.Interfaces.IMapWin, _
                                  ByVal ParentHandle As Integer)
  End Sub

  Public Overrides Function ToString() As String
    Return Name.Substring(23) 'Skip first part of Name which is "Timeseries::Seasonal - "
  End Function

  Public Overloads ReadOnly Property AvailableOperations(ByVal aIncludeSplits As Boolean, _
                                                         ByVal aIncludeSeasonalAttributes As Boolean) As atcDataAttributes
    Get
      Dim lOperations As atcDataAttributes
      If aIncludeSplits AndAlso aIncludeSeasonalAttributes AndAlso Not pAvailableOperations Is Nothing Then
        lOperations = pAvailableOperations
      Else
        lOperations = New atcDataAttributes
        Dim lArguments As atcDataAttributes
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

            If aIncludeSeasonalAttributes Then
              Dim lSeasonalAttributes As New atcAttributeDefinition
              With lSeasonalAttributes
                .Name = SeasonClassNameToLabel(typ.Name) & "::SeasonalAttributes"
                .Category = "Attributes"
                .Description = "Attribute values calculated per season"
                .Editable = False
                .TypeString = "atcDataAttributes"
                .Calculator = Me
              End With
              lArguments = New atcDataAttributes
              lArguments.SetValue(defTimeSeriesOne, Nothing)
              lArguments.SetValue("Attributes", Nothing)

              lOperations.SetValue(lSeasonalAttributes, Nothing, lArguments)
            End If

            If aIncludeSplits Then
              Dim lSeasonalSplit As New atcAttributeDefinition
              With lSeasonalSplit
                .Name = SeasonClassNameToLabel(typ.Name) & "::Split"
                .Category = "Split"
                .Description = "Split a timeseries by season"
                .Editable = False
                .TypeString = "atcDataGroup"
                .Calculator = Me
              End With

              lArguments = New atcDataAttributes
              lArguments.SetValue(defTimeSeriesOne, Nothing)

              lOperations.SetValue(lSeasonalSplit, Nothing, lArguments)
            End If
          End If
        Next

        If aIncludeSplits AndAlso aIncludeSeasonalAttributes Then
          pAvailableOperations = lOperations
        End If

      End If
      Return lOperations
    End Get
  End Property

  Private Function SeasonClassNameToLabel(ByVal aName As String) As String
    If aName.StartsWith("atc") Then
      Dim lCamelCase As String = aName.Substring(10)
      If lCamelCase.Equals("AMorPM") Then
        Return "AM or PM"
      Else
        Dim lReturn As String = lCamelCase.Substring(0, 1)
        For iCh As Integer = 1 To lCamelCase.Length - 1
          Dim lCurChar As String = lCamelCase.Substring(iCh, 1)
          If lCurChar.ToUpper.Equals(lCurChar) Then
            lReturn &= " "
          End If
          lReturn &= lCurChar
        Next
        Return lReturn
      End If
    Else
      Return ""
    End If
  End Function

  Public Overloads Overrides ReadOnly Property AvailableOperations() As atcDataAttributes
    Get
      Return AvailableOperations(True, True)
    End Get
  End Property
End Class