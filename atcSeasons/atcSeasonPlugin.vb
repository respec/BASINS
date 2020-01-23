Imports atcData
Imports atcUtility
Imports MapWinUtility
Imports MapWinUtility.Strings
Imports System.Reflection

Public Class atcSeasonPlugin
    Inherits atcTimeseriesSource

    Private pAvailableOperations As atcDataAttributes ' atcDataGroup
    Private pName As String = "Timeseries::Seasons"
    Private pSeasons As atcSeasonBase

    Public Overrides ReadOnly Property Name() As String
        Get
            If pSeasons Is Nothing Then
                Return pName
            Else
                Return pSeasons.Name
            End If
        End Get
    End Property

    Public Overrides ReadOnly Property Category() As String
        Get
#If Toolbox = "Hydro" Then
            Return "Subset and Filter Time Series"
#Else
            Return "Seasons"
#End If
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
        Dim lSeasonName As String = ""
        Dim lTimeSeriesGroup As atcTimeseriesGroup = TimeseriesGroupFromArguments(aArgs)
        MyBase.DataSets.Clear()
        If lTimeSeriesGroup Is Nothing Then
            lTimeSeriesGroup = atcDataManager.UserSelectData("Select data for " & aOperationName)
        End If
        If lTimeSeriesGroup IsNot Nothing AndAlso lTimeSeriesGroup.Count > 0 Then
            If aOperationName.IndexOf("::") > 0 Then
                lSeasonName = StrSplit(aOperationName, "::", "")
                pSeasons = atcSeasonBase.CreateSeasonObject("atcSeasons" & lSeasonName)
            End If
            Select Case aOperationName.ToLower
                Case "split filter"
                    If pSeasons Is Nothing Then
                        Dim lForm As New frmFilterData
                        Dim lTserProcessed As atcTimeseriesGroup = lForm.AskUser(lTimeSeriesGroup)
                        If lTserProcessed IsNot Nothing Then
                            DataSets.AddRange(lTserProcessed)
                            If DataSets.Count < 1 Then
                                Throw New ApplicationException("No data found in selected seasons")
                            Else
                                Me.Specification = "Split " & DataSets(0).Attributes.GetValue("SeasonDefinition").ToString
                            End If
                            Return True
                        Else
                            Return False
                        End If
                    Else
                        For Each lts As atcTimeseries In lTimeSeriesGroup
                            MyBase.DataSets.AddRange(pSeasons.Split(lts, Me))
                        Next
                        Return MyBase.DataSets.Count > 0
                    End If
                Case "split"
                    If pSeasons Is Nothing Then
                        Dim lForm As New frmSpecifySplit
                        If lForm.AskUser(lTimeSeriesGroup, AvailableOperations(True, False), MyBase.DataSets) Then
                            If DataSets.Count < 1 Then
                                Throw New ApplicationException("No data found in selected seasons")
                            Else
                                Me.Specification = "Split " & DataSets(0).Attributes.GetValue("SeasonDefinition").ToString
                            End If
                            Return True
                        Else
                            Return False
                        End If
                    Else
                        For Each lts As atcTimeseries In lTimeSeriesGroup
                            MyBase.DataSets.AddRange(pSeasons.Split(lts, Me))
                        Next
                        Return MyBase.DataSets.Count > 0
                    End If
                Case "attributes", "seasonalattributes"
                    Dim lAttributes As atcDataAttributes = Nothing
                    Dim lCalculatedAttributes As atcDataAttributes = Nothing
                    If aArgs IsNot Nothing AndAlso aArgs.Count > 0 Then
                        lAttributes = aArgs.GetValue("Attributes")
                        lCalculatedAttributes = aArgs.GetValue("CalculatedAttributes")
                    End If
                    If pSeasons Is Nothing OrElse lAttributes Is Nothing OrElse lAttributes.Count = 0 Then
                        Dim lForm As New frmSpecifySeasonalAttributes
                        If lForm.AskUser(lTimeSeriesGroup, AvailableOperations(False, True)) Then
                            'atcDataManager.ShowDisplay("Analysis::Data Tree", lTimeSeriesGroup)
                        End If
                    Else
                        For Each lts As atcTimeseries In lTimeSeriesGroup
                            pSeasons.SetSeasonalAttributes(lts, lAttributes, lCalculatedAttributes)
                        Next
                    End If
                    DataSets.Clear()
                    DataSets.AddRange(lTimeSeriesGroup)
                    Return True
                Case Else
                    Logger.Msg("Operation '" & aOperationName & "' not recognized", "Season Plugin Open")
            End Select
        End If
        Return False
    End Function

    Public Overrides Function ToString() As String
        Return Name.Replace("Timeseries::Seasonal - ", "")
    End Function

    Public Overloads ReadOnly Property AvailableOperations(ByVal aIncludeSplits As Boolean, _
                                                           ByVal aIncludeSeasonalAttributes As Boolean) As atcDataAttributes
        Get
            Dim lOperations As atcDataAttributes
            If aIncludeSplits AndAlso aIncludeSeasonalAttributes AndAlso pAvailableOperations IsNot Nothing Then
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

                For Each typ As Type In atcData.atcSeasonBase.AllSeasonTypes
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
                            .DefaultValue = typ
                            '.ValidList = New ArrayList
                            '.ValidList.AddRange(typ.AllSeasonNames)
                        End With

                        lArguments = New atcDataAttributes
                        lArguments.SetValue(defTimeSeriesOne, Nothing)

                        lOperations.SetValue(lSeasonalSplit, Nothing, lArguments)
                    End If
                Next

                If aIncludeSplits AndAlso aIncludeSeasonalAttributes Then
                    pAvailableOperations = lOperations
                End If

            End If
            Return lOperations
        End Get
    End Property

    Public Shared Function SeasonClassNameToLabel(ByVal aName As String) As String
        Return atcSeasonBase.SeasonClassNameToLabel(aName)
    End Function

    Public Overloads Overrides ReadOnly Property AvailableOperations() As atcDataAttributes
        Get
            'Return AvailableOperations(True, True)
            Dim lOperations As atcDataAttributes

            lOperations = New atcDataAttributes
            Dim lArguments As atcDataAttributes
            Dim defTimeSeriesOne As New atcAttributeDefinition
            With defTimeSeriesOne
                .Name = "Timeseries"
                .Description = "One time series"
                .Editable = True
                .TypeString = "atcTimeseries"
            End With

#If Toolbox = "Hydro" Then
            Dim lSplitFilter As New atcAttributeDefinition
            With lSplitFilter
                .Name = "Split Filter"
                .Description = "Split and filter a timeseries"
                .Editable = False
                .TypeString = "atcDataGroup"
                .Calculator = Me
            End With
            lArguments = New atcDataAttributes
            lArguments.SetValue(defTimeSeriesOne, Nothing)
            lOperations.SetValue(lSplitFilter, Nothing, lArguments)
#Else
            Dim lSeasonalAttributes As New atcAttributeDefinition
            With lSeasonalAttributes
                .Name = "Attributes"
                .Description = "Attribute values calculated per season"
                .Editable = False
                .TypeString = "atcDataAttributes"
                .Calculator = Me
            End With
            lArguments = New atcDataAttributes
            lArguments.SetValue(defTimeSeriesOne, Nothing)
            lArguments.SetValue("Attributes", Nothing)
            lOperations.SetValue(lSeasonalAttributes, Nothing, lArguments)

            Dim lSeasonalSplit As New atcAttributeDefinition
            With lSeasonalSplit
                .Name = "Split"
                .Description = "Split a timeseries by season"
                .Editable = False
                .TypeString = "atcDataGroup"
                .Calculator = Me
            End With
            lArguments = New atcDataAttributes
            lArguments.SetValue(defTimeSeriesOne, Nothing)
            lOperations.SetValue(lSeasonalSplit, Nothing, lArguments)
#End If

            Return lOperations
        End Get
    End Property

    Public Overrides Sub ItemClicked(ByVal aItemName As String, ByRef aHandled As Boolean)
        Dim lAnalysisMenuName As String = atcDataManager.AnalysisMenuName
        If aItemName = lAnalysisMenuName & "_" & Category Then
            Dim lOperationName As String = "split filter"
            If Open(lOperationName) Then
                Dim lOperation As atcDefinedValue = AvailableOperations.GetDefinedValue(lOperationName)
                atcDataManager.DataSources.Remove(Me)
                If DataSets.Count > 0 Then
                    atcDataManager.DataSources.Add(Me)
                    'Dim lTitle As String = Me.ToString
                    'atcDataManager.UserSelectDisplay(lTitle, DataSets)
                    Logger.Dbg("DataSetCount:" & DataSets.Count & ":Specification:" & Me.Specification)
                    'RaiseEvent atcData.atcDataManager.OpenedData(Me)
                End If
            End If
        End If
    End Sub

End Class