Option Strict Off
Option Explicit On

Imports atcData
Imports atcUtility
Imports MapWinUtility
Imports System.Collections
Imports System.IO
Imports System.Text
Imports atcTimeseriesNetCDF.NetCDF
Imports atcTimeseriesNetCDF.atcNetCDF

''' <summary>
''' Reads NetCDF binary files containing time-series values
''' </summary>
''' <remarks>
''' </remarks>
Public Class atcTimeseriesNetCDF
    Inherits atcTimeseriesSource

    Private Shared pFilter As String = "NetCDF Files (*.nc)|*.nc|All Files (*.*)|*.*"
    Private pErrorDescription As String
    Private pJulianOffset As Double = New Date(1900, 1, 1).Subtract(New Date(1, 1, 1)).TotalDays
    Private pDebug As Boolean

    Public Overrides ReadOnly Property Description() As String
        Get
            Return "NetCDF File"
        End Get
    End Property

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Timeseries::NetCDF"
        End Get
    End Property

    Public Overrides ReadOnly Property Category() As String
        Get
            Return "File"
        End Get
    End Property

    Public Overrides ReadOnly Property CanOpen() As Boolean
        Get
            Return True
        End Get
    End Property

    Public Property Debug() As Boolean
        Get
            Return pDebug
        End Get
        Set(ByVal value As Boolean)
            pDebug = value
        End Set
    End Property

    Public Overrides Function Open(ByVal aFileName As String, _
                          Optional ByVal aAttributes As atcData.atcDataAttributes = Nothing) As Boolean
        If aFileName Is Nothing OrElse aFileName.Length = 0 OrElse Not FileExists(aFileName) Then
            With New Windows.Forms.OpenFileDialog
                .Title = "Select " & Name & " file(s) to open"
                .Filter = pFilter
                .FilterIndex = 1
                .DefaultExt = ".nc"
                .Multiselect = True
                If .ShowDialog() <> Windows.Forms.DialogResult.OK Then
                    ' user clicked Cancel
                    Logger.Dbg("User Cancelled File Selection Dialog")
                    Logger.LastDbgText = "" 'forget about this - user was in control - no additional message box needed
                    Return False
                End If
                aFileName = String.Join(";", .FileNames)
            End With
        End If
        Me.Specification = IO.Path.GetFullPath(aFileName)

        Dim lLogfileName As String = Logger.FileName
        If pDebug Then
            Logger.StartToFile(IO.Path.ChangeExtension(aFileName, ".log"), , False, True)
        End If

        Dim lFileNames As String = aFileName
        If aFileName.ToLower.EndsWith(".dat") Then
            lFileNames = ""
            For Each lFileName As String In LinesInFile(aFileName)
                lFileNames &= lFileName & ";"
            Next
            'lFileNames.Remove(lFileNames.LastIndexOf(";"), 1)
            lFileNames = lFileNames.Substring(0, lFileNames.LastIndexOf(";"))
            Logger.Status("Reading netCDF files listed in " & aFileName)
        End If
        Logger.Dbg("ProcessFile(s) " & lFileNames)

        Dim lXIndexToget As Integer = -1
        Dim lYIndexToget As Integer = -1
        Dim lGetOnlyOneTimseries As Boolean = False

        Dim lAggregateGridFile As atcNetCDFFile = Nothing
        Dim lScenario As String = "netCDF"

        If aAttributes IsNot Nothing AndAlso aAttributes.Count > 0 Then
            lXIndexToget = aAttributes.GetValue("XIndex", lXIndexToget)
            lYIndexToget = aAttributes.GetValue("YIndex", lYIndexToget)
            If lXIndexToget >= 0 AndAlso lYIndexToget >= 0 Then
                lGetOnlyOneTimseries = True
                Me.Attributes.Add("XIndex", lXIndexToget)
                Me.Attributes.Add("YIndex", lYIndexToget)
                lScenario = "netCDF: X:" & lXIndexToget & " Y:" & lYIndexToget
            End If
            Dim lAggregateGridFileName As String = aAttributes.GetValue("AggregateGrid", "")
            If lAggregateGridFileName.Length > 0 Then
                lAggregateGridFile = New atcNetCDFFile(lAggregateGridFileName)
                Me.Attributes.Add("AggregateGrid", lAggregateGridFileName)
                lScenario = "netCDF: Aggregate:" & FilenameNoPath(lAggregateGridFileName)
            End If
        Else 'temporarily prompt for aggregate file
            With New Windows.Forms.OpenFileDialog
                .Title = "Select Aggregation Grid file for accessing timeseries in " & Specification
                .Filter = pFilter
                .FilterIndex = 1
                .DefaultExt = ".nc"
                .Multiselect = False
                If .ShowDialog() <> Windows.Forms.DialogResult.OK Then
                    ' user clicked Cancel
                    Logger.Dbg("User Cancelled File Selection Dialog")
                    Logger.LastDbgText = "" 'forget about this - user was in control - no additional message box needed
                    Return False
                End If
                Dim lAggregateGridFileName As String = .FileName
                If lAggregateGridFileName.Length > 0 Then
                    lAggregateGridFile = New atcNetCDFFile(lAggregateGridFileName)
                    Me.Attributes.Add("AggregateGrid", lAggregateGridFileName)
                    lScenario = "netCDF: Aggregate:" & FilenameNoPath(lAggregateGridFileName)
                End If
            End With

        End If

        Try
            Dim lFileindex As Integer = 0
            For Each lFileName As String In lFileNames.Split(";")
                If lFileNames.Split(";").Count > 0 Then
                    lFileindex += 1
                    Logger.Progress(lFileindex, lFileNames.Split(";").Count)
                End If
                Dim lDatesUpdated As Boolean = False
                If Not FileExists(lFileName) Then
                    lFileName = PathNameOnly(aFileName) & "\" & lFileName
                    If Not FileExists(lFileName) Then
                        pErrorDescription = "File '" & lFileName & "' not found"
                        Me.Specification = ""
                    End If
                End If

                If Me.Specification.Length > 0 Then
                    'TODO: check to see if this file is open for another timeseries group, need the collection of open timeseries files
                    Dim lNetCDFFile As New atcNetCDFFile(lFileName)
                    If lNetCDFFile.TimeDimension Is Nothing Then
                        pErrorDescription = "No timeseries found in " & lFileName
                        Logger.Dbg(pErrorDescription)
                        Return False
                    Else 'how many timeseries?
                        Dim lTimeseriesCount As Integer = 1
                        Dim lUniqueLocations As New atcCollection
                        If lAggregateGridFile IsNot Nothing Then
                            'aggregating, how many unique values in aggregate file
                            lUniqueLocations = FindUniqueValues(lAggregateGridFile)
                            lTimeseriesCount = lUniqueLocations.Count
                            Logger.Dbg("UniqueLocations: " & lTimeseriesCount)
                        Else
                            If lNetCDFFile.EastWestDimension Is Nothing AndAlso lNetCDFFile.NorthSouthDimension IsNot Nothing Then
                                lTimeseriesCount = lNetCDFFile.NorthSouthDimension.Length
                            ElseIf lNetCDFFile.EastWestDimension IsNot Nothing AndAlso lNetCDFFile.NorthSouthDimension Is Nothing Then
                                lTimeseriesCount = lNetCDFFile.NorthSouthDimension.Length
                            ElseIf lNetCDFFile.EastWestDimension IsNot Nothing AndAlso lNetCDFFile.NorthSouthDimension IsNot Nothing Then
                                lTimeseriesCount = lNetCDFFile.EastWestDimension.Length * lNetCDFFile.NorthSouthDimension.Length
                            End If
                            Logger.Dbg("TimeseriesCount:" & lTimeseriesCount)
                        End If

                        If lTimeseriesCount > 1000 Then

                            'TODO allow user to specify grid overlay here

                            If Not lGetOnlyOneTimseries Then
                                'too many timeseries, ask user which one
                                Dim lFrmIndex As New frmIndex
                                With lFrmIndex
                                    .txtXIndex.HardMax = lNetCDFFile.EastWestDimension.Length
                                    .txtYIndex.HardMax = lNetCDFFile.NorthSouthDimension.Length
                                    If .ShowDialog() <> Windows.Forms.DialogResult.Cancel Then
                                        lXIndexToget = .txtXIndex.Text
                                        lYIndexToget = .txtYIndex.Text
                                        lGetOnlyOneTimseries = True
                                        Me.Attributes.Add("XIndex", lXIndexToget)
                                        Me.Attributes.Add("YIndex", lYIndexToget)
                                        Logger.Dbg("User Specified X " & lXIndexToget & " Y " & lYIndexToget)
                                    Else
                                        Logger.Dbg("Too Many Timeseries, user did not specify")
                                        Return False
                                    End If
                                End With
                            End If
                        End If

                        'get the dates for the timeseries
                        Dim lTimeVariable As atcNetCDFVariable = lNetCDFFile.Variables(lNetCDFFile.TimeDimension.ID)
                        Dim lUnits As String = lTimeVariable.Attributes.GetValue("Units").ToString
                        Dim lTimeUnit As atcTimeUnit = atcTimeUnit.TUUnknown
                        Dim lTimeStepMultiplier As Double = 1.0
                        If lUnits.StartsWith("hour") Then
                            lTimeUnit = atcTimeUnit.TUHour
                            lTimeStepMultiplier = lTimeStepMultiplier / 24.0
                        ElseIf lUnits.StartsWith("day") Then
                            lTimeUnit = atcTimeUnit.TUDay
                        Else
                            Throw New Exception("unknown time units '" & lUnits & "'")
                        End If
                        Dim lDateBase As Date = lUnits.Remove(0, lUnits.IndexOf("since") + 6).Substring(0, 10)

                        Dim lDates As New atcTimeseries(Me)

                        'TODO: check to see that dates and values are put in the correct spot in the array
                        Dim lNumValues As Integer = lTimeVariable.Dimensions(0).Length
                        ReDim lDates.Values(lNumValues)
                        'lDates.Values(0) = lDateBase.ToOADate
                        For lTimeIndex As Integer = 0 To lNumValues - 1
                            lDates.Values(lTimeIndex) = lDateBase.ToOADate + (lTimeStepMultiplier * lTimeVariable.Values(lTimeIndex))
                            'If lTimeIndex = 1 Then 'deal with base date a long time before first value
                            '    Dim lTimeInterval As Double = lTimeVariable.Values(lTimeIndex) - lTimeVariable.Values(lTimeIndex - 1)
                            '    lDates.Values(0) = lDates.Values(1) - lTimeInterval
                            'End If
                        Next
                        lDates.Values(lNumValues) = lDates.Values(lNumValues - 1) + (lDates.Values(lNumValues - 1) - lDates.Values(lNumValues - 2))

                        'get the data
                        Dim lYIndex As Integer = 1
                        Dim lYValue As String = ""
                        Dim lXIndex As Integer = 1
                        Dim lXValue As String = ""
                        For lTimeseriesIndex As Integer = 0 To lTimeseriesCount - 1
                            Dim lLocation As String = ""
                            If lUniqueLocations.Count > 0 Then
                                lLocation = "ID_" & lUniqueLocations.Keys(lTimeseriesIndex)
                            ElseIf lNetCDFFile.EastWestDimension Is Nothing AndAlso lNetCDFFile.NorthSouthDimension IsNot Nothing Then
                                lYValue = lNetCDFFile.Variables(lNetCDFFile.NorthSouthDimension.ID).Values(lYIndex - 1)
                                lLocation &= "Y:" & lYValue
                            ElseIf lNetCDFFile.EastWestDimension IsNot Nothing AndAlso lNetCDFFile.NorthSouthDimension Is Nothing Then
                                lXValue = lNetCDFFile.Variables(lNetCDFFile.EastWestDimension.ID).Values(lXIndex - 1)
                                lLocation &= "X:" & lXValue
                            ElseIf lNetCDFFile.EastWestDimension IsNot Nothing AndAlso lNetCDFFile.NorthSouthDimension IsNot Nothing Then
                                lXValue = lNetCDFFile.Variables(lNetCDFFile.EastWestDimension.ID).Values(lXIndex - 1)
                                lLocation &= "X:" & lXValue
                                lYValue = lNetCDFFile.Variables(lNetCDFFile.NorthSouthDimension.ID).Values(lYIndex - 1)
                                lLocation &= " Y:" & lYValue
                            End If

                            If Not lGetOnlyOneTimseries OrElse _
                                  (lYIndex = lYIndexToget AndAlso lXIndex = lXIndexToget) OrElse _
                                  lUniqueLocations.Count > 0 AndAlso lLocation.Length > 0 Then
                                For lConstituentVariableIndex As Integer = 0 To lNetCDFFile.ConstituentVariables.Count - 1
                                    Dim lDataVariable As atcNetCDFVariable = lNetCDFFile.ConstituentVariables(lConstituentVariableIndex) 'lNetCDFFile.Variables(lNetCDFFile.Variables.Count - 1)
                                    Dim lDataVariableName As String = lDataVariable.Name
                                    Dim lMatchingTimeseriesGroup As atcDataGroup = Me.DataSets.FindData("Location", lLocation).FindData("Constituent", lDataVariableName)
                                    'Logger.Dbg("Count of Timeseries Matching " & lDataVariableName & ":" & lLocation & " " & lMatchingTimeseriesGroup.Count)
                                    Dim lTimeseries As atcTimeseries
                                    If lMatchingTimeseriesGroup.Count > 0 Then
                                        lTimeseries = lMatchingTimeseriesGroup.Item(0)
                                        'Logger.Dbg("Add data to " & lTimeseries.ToString)
                                        Dim lStartDateIndex As Integer
                                        If Not lDatesUpdated Then
                                            lStartDateIndex = lTimeseries.Dates.numValues
                                            ReDim Preserve lTimeseries.Dates.Values(lStartDateIndex + lDates.numValues)
                                            For lDateIndex As Integer = 1 To lDates.numValues
                                                lTimeseries.Dates.Values(lDateIndex + lStartDateIndex) = lDates.Values(lDateIndex)
                                            Next
                                            'Logger.Dbg("  Date Count From " & lStartDateIndex & " to " & lTimeseries.Dates.numValues)
                                            lDatesUpdated = True
                                        Else
                                            lStartDateIndex = lTimeseries.Dates.numValues - lDates.numValues
                                        End If
                                        Dim lNetCDFValuesCollection As atcCollection = lTimeseries.Attributes.GetValue("NetCDFValues")
                                        lNetCDFValuesCollection.Add(lStartDateIndex & ":" & lDates.numValues, lDataVariable)
                                    Else
                                        lTimeseries = New atcTimeseries(Me)
                                        For Each lDataAttribute As atcDefinedValue In lDataVariable.Attributes
                                            lTimeseries.Attributes.SetValue(lDataAttribute.Definition.Name, lDataAttribute.Value)
                                        Next
                                        Dim lNetCDFValuesCollection As New atcCollection
                                        lNetCDFValuesCollection.Add(0 & ":" & lDates.numValues, lDataVariable)
                                        lTimeseries.Attributes.SetValue("NetCDFValues", lNetCDFValuesCollection)
                                        If lUniqueLocations.Count > 0 AndAlso lLocation.Length > 0 Then
                                            Dim lCellLocations As ArrayList = lUniqueLocations(lTimeseriesIndex)
                                            lTimeseries.Attributes.SetValue("Location Indexes", lCellLocations)
                                            lTimeseries.Attributes.SetValue("CellCount", lCellLocations.Count)
                                        End If

                                        lTimeseries.SetInterval(lTimeUnit, 1)
                                        'lTimeseries.Attributes.SetValue("Constituent", IO.Path.GetFileNameWithoutExtension(lFileName) & "_" & lDataVariableName)
                                        lTimeseries.Attributes.SetValue("Constituent", lDataVariableName)
                                        lTimeseries.Attributes.SetValue("ID", lTimeseriesIndex + 1)
                                        If lUniqueLocations.Count = 0 Then
                                            If lNetCDFFile.NorthSouthDimension IsNot Nothing Then
                                                lTimeseries.Attributes.SetValue("Y", lYValue)
                                                lTimeseries.Attributes.SetValue("Y index", lYIndex)
                                            End If
                                            If lNetCDFFile.EastWestDimension IsNot Nothing Then
                                                lTimeseries.Attributes.SetValue("X", lXValue)
                                                lTimeseries.Attributes.SetValue("X index", lXIndex)
                                            End If
                                        End If
                                        lTimeseries.Attributes.SetValue("Location", lLocation)
                                        lTimeseries.Attributes.SetValue("Scenario", lScenario)
                                        lTimeseries.ValuesNeedToBeRead = True
                                        lTimeseries.Dates = lDates

                                        If lLocation.Length > 0 Then Me.DataSets.Add(lTimeseries)
                                    End If
                                Next
                            End If

                            If lNetCDFFile.EastWestDimension Is Nothing AndAlso lNetCDFFile.NorthSouthDimension IsNot Nothing Then
                                lYIndex += 1
                            ElseIf lNetCDFFile.EastWestDimension IsNot Nothing AndAlso lNetCDFFile.NorthSouthDimension Is Nothing Then
                                lXIndex += 1
                            ElseIf lNetCDFFile.EastWestDimension IsNot Nothing AndAlso lNetCDFFile.NorthSouthDimension IsNot Nothing Then
                                If lNetCDFFile.EastWestDimension.ID = 0 OrElse (lNetCDFFile.TimeDimension.ID = 0 AndAlso lNetCDFFile.EastWestDimension.ID = 1) Then
                                    lXIndex += 1
                                    If lXIndex > lNetCDFFile.EastWestDimension.Length Then
                                        lXIndex = 1
                                        lYIndex += 1
                                    End If
                                Else
                                    lYIndex += 1
                                    If lYIndex > lNetCDFFile.NorthSouthDimension.Length Then
                                        lYIndex = 1
                                        lXIndex += 1
                                    End If
                                End If
                            End If
                        Next
                        Logger.Dbg("TimeseriesBuilt:Count " & Me.DataSets.Count & " Length " & lNumValues)
                    End If
                End If
            Next
            For Each lTimeseries As atcTimeseries In Me.DataSets
                lTimeseries.Attributes.SetValue("End Date", lTimeseries.Dates.Value(lTimeseries.Dates.numValues))
                lTimeseries.Attributes.SetValue("Start Date", lTimeseries.Dates.Value(0))
            Next

        Catch e As Exception
            Logger.Dbg("Exception reading '" & aFileName & "': " & e.Message, e.StackTrace)
            Return False
        Finally
            If pDebug Then
                Logger.StartToFile(lLogfileName, True)
            End If
        End Try

        Return (pData.Count > 0)
    End Function

    Public Sub New()
        Filter = pFilter
    End Sub

    Public Overrides Sub ReadData(ByVal aData As atcData.atcDataSet)
        Dim lTimeseries As atcTimeseries = aData
        Dim lNumValues As Integer = lTimeseries.Dates.numValues
        ReDim lTimeseries.Values(lNumValues)

        Dim lDataVariables As atcCollection = lTimeseries.Attributes.GetValue("NetCDFValues")
        Dim lAggregateLocation As ArrayList = lTimeseries.Attributes.GetValue("Location Indexes")
        For lDataVariableIndex As Integer = 0 To lDataVariables.Count - 1
            Dim lDataVariable As atcNetCDFVariable = lDataVariables(lDataVariableIndex)
            Dim lKey As String = lDataVariables.Keys(lDataVariableIndex)
            Dim lKeyParts As Array = lKey.Split(":")
            Dim lDataVariableStartPosition As Integer = lKeyParts(0)
            Dim lDataVariableNumValues As Integer = lKeyParts(1)
            If lAggregateLocation Is Nothing Then
                Dim lXIndex As Integer = lTimeseries.Attributes.GetValue("X index")
                Dim lYIndex As Integer = lTimeseries.Attributes.GetValue("Y index")
                Dim lValues = lDataVariable.ReadArray(lDataVariableNumValues, lXIndex - 1, lYIndex - 1)
                For lTimeIndex As Integer = 0 To lDataVariableNumValues - 1
                    lTimeseries.Values(lTimeIndex + lDataVariableStartPosition + 1) = lValues(lTimeIndex)
                Next
            Else
                Logger.Dbg("Aggregating " & lAggregateLocation.Count & " points at " & lTimeseries.Attributes.GetValue("Location"))
                Logger.Status("Aggregating " & lTimeseries.Attributes.GetValue("constituent") & " points at " & lTimeseries.Attributes.GetValue("Location"))
                Dim lIndex As Integer = 1
                For Each lXYPair As String In lAggregateLocation
                    If lIndex = 1 OrElse lIndex Mod 1000 = 0 Then
                        Logger.Dbg("  Process " & lIndex & " of " & lAggregateLocation.Count & " at " & lXYPair)
                        Logger.Progress(lIndex, lAggregateLocation.Count)
                    End If
                    If lAggregateLocation.Count = 26 Then 'debug at ID_10
                        Logger.Dbg("  Process " & lXYPair)
                    End If
                    Dim lXIndex As Integer = StrRetRem(lXYPair)
                    Dim lYIndex As Integer = lXYPair
                    Dim lValues = lDataVariable.ReadArray(lDataVariableNumValues, lXIndex, lYIndex)
                    If lAggregateLocation.Count = 26 Then 'debug at ID_10
                        Logger.Dbg("     Value " & lValues(0))
                    End If
                    For lTimeIndex As Integer = 0 To lDataVariableNumValues - 1
                        lTimeseries.Values(lTimeIndex + lDataVariableStartPosition + 1) += lValues(lTimeIndex)
                    Next
                    lIndex += 1
                Next
            End If
        Next
        Logger.Progress("", 0, 0)
        If lAggregateLocation IsNot Nothing Then
            For lTimeIndex As Integer = 0 To lNumValues - 1
                lTimeseries.Values(lTimeIndex + 1) /= lAggregateLocation.Count
            Next
        End If
    End Sub

    Private Function FindUniqueValues(ByVal aAggregateGridFile As atcNetCDFFile) As atcCollection
        Dim lUniqueValues As New atcCollection
        Dim lDataVariable As atcNetCDFVariable = aAggregateGridFile.ConstituentVariables(0)
        For lX As Integer = 0 To aAggregateGridFile.EastWestDimension.Length - 1
            For lY As Integer = 0 To aAggregateGridFile.NorthSouthDimension.Length - 1
                Dim lDataValues = lDataVariable.ReadArray(1, lX, lY)
                If lDataValues(0) >= 0 AndAlso lDataValues(0) <> 128 Then
                    If Not lUniqueValues.Keys.Contains(lDataValues(0)) Then
                        lUniqueValues.Add(lDataValues(0), New ArrayList)
                    End If
                    Dim lValueCollection As ArrayList = lUniqueValues.ItemByKey(lDataValues(0))
                    'todo: is the top/bottome change y index a special case for merra data?
                    'Dim lValueToAdd As String = lX & "," & lY 
                    Dim lValueToAdd As String = lX & "," & aAggregateGridFile.NorthSouthDimension.Length - lY - 1
                    lValueCollection.Add(lValueToAdd)
                End If
            Next
        Next
        Return lUniqueValues
    End Function
End Class
