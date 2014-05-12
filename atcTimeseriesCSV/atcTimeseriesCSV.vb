Option Strict Off
Option Explicit On

Imports atcData
Imports atcUtility
Imports MapWinUtility
Imports Microsoft.VisualBasic
Imports System.Collections
Imports System.IO

Partial Public Class atcTimeseriesCSV
    Inherits atcTimeseriesSource
    '##MODULE_REMARKS Copyright 2008 AQUA TERRA Consultants - Royalty-free use permitted under open source license

    Public Delimiter As String = ","
    Public DateOffset As Double = 0 'Add this Julian value to all dates read

    Private Shared pFilter As String = "Comma-Separated Value Files (*.csv)|*.csv"
    Private Shared pNaN As Double = GetNaN()

    Private pDates As atcTimeseries = Nothing 'Can share dates since they will be the same for all ts in a file

    Public Overrides ReadOnly Property Description() As String
        Get
            Return "Comma-Separated Value Files"
        End Get
    End Property

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Timeseries::CSV"
        End Get
    End Property

    Public Overrides ReadOnly Property Category() As String
        Get
            Return "File"
        End Get
    End Property

    Public Overrides ReadOnly Property CanOpen() As Boolean
        Get
            Return True 'yes, this class can open files
        End Get
    End Property

    Public Overrides ReadOnly Property CanSave() As Boolean
        Get
            Return False 'can save met data when called by code, but does not yet support saving to user-selected file
        End Get
    End Property

    Public Overrides Function Open(ByVal aFileName As String, Optional ByVal aAttributes As atcData.atcDataAttributes = Nothing) As Boolean
        If MyBase.Open(aFileName, aAttributes) Then 'see if file exists
            Dim lConstituent As String = IO.Path.GetFileNameWithoutExtension(Specification)
            Dim lFirstRecord As Boolean = True
            Dim lGroupBuilder As New atcTimeseriesGroupBuilder(Me)
            Dim lDate As Date
            Dim lSegments As Boolean = False
            For Each lLine As String In LinesInFile(Specification)
                If lFirstRecord Then
                    lFirstRecord = False
                    If lLine.Contains("Output Variable:") Then
                        Dim lConstituentStart As Integer = lLine.IndexOf(":") + 2
                        Dim lNumberStart As Integer = lLine.IndexOf("Number of Segments")
                        If lNumberStart > 0 Then
                            lSegments = True
                            lConstituent = lLine.Substring(lConstituentStart, lNumberStart - lConstituentStart).Trim
                        Else
                            lConstituent = lLine.Substring(lConstituentStart).Trim
                        End If
                    Else
                        GoTo ParseValues
                    End If
                Else
ParseValues:
                    Dim lDelimPos As Integer = lLine.IndexOf(Delimiter)
                    If lDelimPos < 0 Then Continue For
                    Dim lDateJ As Double                    
                    If Double.TryParse(lLine.Substring(0, lDelimPos).Trim, lDateJ) Then
                        If lDateJ < 2 AndAlso DateOffset < 1 Then
                            Dim lStartFilename As String = Specification & ".start"
                            If IO.File.Exists(lStartFilename) Then
                                Dim lSavedStartDate As String = IO.File.ReadAllText(lStartFilename)
                                If Date.TryParse(lSavedStartDate, lDate) Then
                                    DateOffset = lDate.ToOADate - lDateJ
                                End If
                            End If
                            If DateOffset < 1 Then ' Didn't find it, ask user
                                Dim lDefaultStartDate As String = ""
                                Try
                                    lDefaultStartDate = GetSetting(Me.Name, "Defaults", "StartDate", "")
                                Catch
                                End Try
                                Dim lEnteredStartDate As String = InputBox("Please enter the starting date of data in " & IO.Path.GetFileName(Specification), "Starting date not found", lDefaultStartDate)
                                If Date.TryParse(lEnteredStartDate, lDate) Then
                                    DateOffset = lDate.ToOADate - lDateJ
                                    Try
                                        IO.File.WriteAllText(lStartFilename, lEnteredStartDate)
                                    Catch
                                    End Try
                                    Try
                                        SaveSetting(Me.Name, "Defaults", "StartDate", lEnteredStartDate)
                                    Catch
                                    End Try
                                End If
                            End If
                        End If
                        lDateJ += DateOffset
                        Dim lDateInt(5) As Integer
                        J2Date(lDateJ, lDateInt)
                        lDate = New Date(lDateInt(0), lDateInt(1), lDateInt(2), lDateInt(3), lDateInt(4), lDateInt(5))

                        Dim lDataValueStrings() As String = lLine.Substring(lDelimPos + 1).Split(Delimiter)
                        Dim lNumValues As Integer = lDataValueStrings.Length
                        Dim lDataValues(lNumValues - 1) As Double

                        For lValIndex As Integer = 0 To lNumValues - 1
                            If Not Double.TryParse(lDataValueStrings(lValIndex), lDataValues(lValIndex)) Then
                                lDataValues(lValIndex) = pNaN
                            End If
                        Next

                        lGroupBuilder.AddValues(lDate, lDataValues)
                    End If
                    End If
            Next

            lGroupBuilder.CreateTimeseriesAddToGroup(Me.DataSets)

            Dim lIndex As Integer = 1
            For Each lTS As atcTimeseries In Me.DataSets
                With lTS.Attributes
                    .SetValue("Constituent", lConstituent)
                    If lSegments Then
                        .SetValue("Scenario", "Computed")
                        .SetValue("Location", .GetValue("ID"))
                    Else
                        .SetValue("Scenario", "Unknown")
                        .SetValue("Location", "Unknown")
                    End If
                End With
                lIndex += 1
            Next

            'Dim lTs As atcTimeseries = Nothing
            'Dim lDates As atcTimeseries = Nothing
            'Dim lNewDates() As Double

            'lTs = New atcTimeseries(Me)
            'lDates = New atcTimeseries(Nothing)
            'lTs.ValuesNeedToBeRead = True
            'lNewDates = NewDates(lRawData.DateBeg, lRawData.DateEnd, .TimeUnit, 1)
            'lDates.Values = lNewDates
            'lTs.Dates = lDates
            ''lTs.numValues = lDates.numValues 'cannot set this as is will think already has values
            'lTs.SetInterval(.TimeUnit, 1)
            'lTs.Attributes.SetValue("Point", .PointType)

            'With lTs.Attributes
            '    .SetValue("Scenario", "OBSERVED")
            '    .SetValue("History 1", FilenameNoPath(Specification))
            '    .SetValue("Constituent", lConstituent)
            '    .SetValue("FieldIndex", lRawData.ColumnIndex) 'currently only reading the actual value
            '    '.SetValue("ID", pDatasetID)
            'End With
            'DataSets.Add(lTs)

            Return True
        End If
        Return False
    End Function

    'Public Overrides Function Save(ByVal SaveFileName As String, _
    '                   Optional ByVal ExistAction As EnumExistAction = EnumExistAction.ExistReplace) As Boolean
    'End Function

    Private Function OpenTable() As atcTable
        Dim lTable As New atcTableDelimited()
        With lTable
            .Delimiter = Me.Delimiter
            .NumFieldNameRows = 0
            If .OpenFile(Specification) Then
                .CurrentRecord = 1
                'Attributes.SetValue("HeaderText", pDefaultHeader)
            Else
                lTable = Nothing
            End If
        End With

        Return lTable
    End Function

    Public Sub New()
        Filter = pFilter
    End Sub
End Class
