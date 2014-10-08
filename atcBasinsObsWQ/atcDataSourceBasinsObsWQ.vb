Option Strict Off
Option Explicit On

Imports atcData
Imports atcUtility
Imports MapWinUtility

Imports System.IO

Public Class atcDataSourceBasinsObsWQ
    Inherits atcTimeseriesSource
    '##MODULE_REMARKS Copyright 2005 AQUA TERRA Consultants - Royalty-free use permitted under open source license

    Private Shared pFilter As String = "Basins Observed WQ Files (*.dbf)|*.dbf"
    Private pColDefs As Hashtable
    'Private pReadAll As Boolean = False

    Public Overrides ReadOnly Property Description() As String
        Get
            Return "Basins Observed Water Quality DBF"
        End Get
    End Property

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Timeseries::BasinsObsWQ"
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
            Return True 'Open new or existing file, then use .AddDatasets to save new data to file
        End Get
    End Property

    Public Overrides Function Open(ByVal aFileName As String, Optional ByVal aAttributes As atcData.atcDataAttributes = Nothing) As Boolean
        If MyBase.Open(aFileName, aAttributes) Then
            If Not IO.File.Exists(Specification) Then
                Logger.Dbg("Opening new file " & Specification)
                Return True
            End If
            Dim lData As atcTimeseries
            Try
                Dim lDBF As New atcTableDBF
                Dim lDateCol As Integer = -1
                Dim lTimeCol As Integer = -1
                Dim lLocnCol As Integer = -1
                Dim lConsCol As Integer = -1
                Dim lValCol As Integer = -1
                Dim lTSKey As String
                Dim lTSIndex As Integer
                Dim lLocation As String = ""
                Dim lNaN As Double = GetNaN()
                lDBF.OpenFile(Specification)
                For lFieldIndex As Integer = 1 To lDBF.NumFields
                    Select Case lDBF.FieldName(lFieldIndex).ToUpper
                        Case "DATE" : lDateCol = lFieldIndex
                        Case "TIME" : lTimeCol = lFieldIndex
                        Case "PARM" : lConsCol = lFieldIndex
                        Case "VALUE" : lValCol = lFieldIndex
                        Case Else
                            If lLocnCol = -1 Then 'only use first one
                                If lDBF.FieldName(lFieldIndex).ToUpper.Contains("ID") Then 'location
                                    lLocnCol = lFieldIndex
                                End If
                            End If
                    End Select
                Next
                If lDateCol > 0 AndAlso lTimeCol > 0 AndAlso lLocnCol > 0 AndAlso _
                   lConsCol > 0 AndAlso lValCol > 0 Then
                    For lRecordNumber As Integer = 1 To lDBF.NumRecords
                        lDBF.CurrentRecord = lRecordNumber
                        lLocation = lDBF.Value(lLocnCol)
                        Dim lConstituentString As String = lDBF.Value(lConsCol)
                        lTSKey = lLocation & ":" & lConstituentString
                        lData = DataSets.ItemByKey(lTSKey)
                        If lData Is Nothing Then
                            lData = New atcTimeseries(Me)
                            lData.Dates = New atcTimeseries(Me)
                            lData.numValues = lDBF.NumRecords - lDBF.CurrentRecord + 1
                            lData.Value(0) = GetNaN()
                            lData.Dates.Value(0) = GetNaN()
                            lData.Attributes.SetValue("Count", 0)
                            lData.Attributes.SetValue("Scenario", "OBSERVED")
                            lData.Attributes.SetValue("Location", lLocation)
                            lData.Attributes.SetValue("Constituent", lConstituentString)
                            lData.Attributes.SetValue("Point", True)
                            lData.Attributes.AddHistory("Read from " & Specification)
                            DataSets.Add(lTSKey, lData)
                            Logger.Dbg("AddDataset:" & DataSets.Count & ":" & lTSKey)
                        End If
                        lTSIndex = lData.Attributes.GetValue("Count") + 1
                        Dim lDataValue As String = lDBF.Value(lValCol)
                        If IsNumeric(lDataValue) Then
                            lData.Value(lTSIndex) = lDataValue
                        Else
                            lData.Value(lTSIndex) = lNaN
                        End If
                        lData.Dates.Value(lTSIndex) = parseWQObsDate(lDBF.Value(lDateCol), lDBF.Value(lTimeCol))
                        lData.Attributes.SetValue("Count", lTSIndex)
                    Next lRecordNumber
                    For Each lData In DataSets
                        lData.numValues = lData.Attributes.GetValue("Count")
                    Next
                    Open = True
                ElseIf lDateCol < 0 Then
                    Open = False
                    Logger.Msg("Unable to identify Date column in BASINS Observed Water Quality file " & aFileName, "BASINS Obs WQ Open")
                ElseIf lTimeCol < 0 Then
                    Open = False
                    Logger.Msg("Unable to identify Time column in BASINS Observed Water Quality file " & aFileName, "BASINS Obs WQ Open")
                ElseIf lLocnCol < 0 Then
                    Open = False
                    Logger.Msg("Unable to identify ID column in BASINS Observed Water Quality file " & aFileName, "BASINS Obs WQ Open")
                ElseIf lConsCol < 0 Then
                    Open = False
                    Logger.Msg("Unable to identify Parm column in BASINS Observed Water Quality file " & aFileName, "BASINS Obs WQ Open")
                ElseIf lValCol < 0 Then
                    Open = False
                    Logger.Msg("Unable to identify Value column in BASINS Observed Water Quality file " & aFileName, "BASINS Obs WQ Open")
                End If
            Catch endEx As EndOfStreamException
                Open = False
            End Try
        End If
    End Function

    Private Function parseWQObsDate(ByVal aDate As String, ByVal aTime As String) As Double
        Dim lDate As String = aDate

        'Check for DBF with date formatted for human readability instead of as DBF date
        If lDate.Contains("/") Then
            Dim lParseDate As Date = Date.Parse(lDate)
            lDate = lParseDate.ToString("yyyyMMdd")
        End If

        Dim lTime As String = aTime.Replace(":", "")
        'assume point values at specified time
        Dim d(5) As Integer 'date array

        If Not IsNumeric(lTime) Then lTime = "1200" 'assume noon for missing obstime
        If IsNumeric(lDate) Then
            If Len(lDate) = 8 Then ' 4 dig yr
                d(0) = lDate.Substring(0, 4)
                d(1) = lDate.Substring(4, 2)
                d(2) = lDate.Substring(6, 2)
            Else
                d(0) = lDate.Substring(0, 2) + 1900
                d(1) = lDate.Substring(2, 2)
                d(2) = lDate.Substring(4, 2)
            End If

            If lTime.Length < 4 Then lTime = lTime.PadLeft(4, "0")
            d(3) = lTime.Substring(0, 2)
            d(4) = lTime.Substring(2, 2)
            Return Date2J(d)
        Else
            Return 0
        End If
    End Function

    Public Sub New()
        Filter = pFilter
    End Sub

    Public Overrides Function AddDatasets(ByVal aDataGroup As atcDataGroup) As Boolean
        For Each lTS As atcTimeseries In aDataGroup
            MyBase.AddDataSet(lTS, EnumExistAction.ExistReplace)
        Next
        SaveDataSets(aDataGroup, EnumExistAction.ExistAppend)
    End Function

    Public Overrides Function AddDataSet(ByVal aDs As atcData.atcDataSet, Optional ByVal aExistAction As atcData.atcDataSource.EnumExistAction = atcData.atcDataSource.EnumExistAction.ExistReplace) As Boolean
        If MyBase.AddDataSet(aDs, aExistAction) Then
            Return SaveDataSets(New atcDataGroup(aDs), aExistAction)
        End If
    End Function

    Public Overrides Function Save(ByVal aSaveFileName As String, Optional ByVal aExistAction As atcData.atcDataSource.EnumExistAction = atcData.atcDataSource.EnumExistAction.ExistReplace) As Boolean
        Specification = aSaveFileName
        Return SaveDataSets(Me.DataSets, aExistAction)
    End Function

    Private Function SaveDataSets(ByVal aDataSets As atcDataGroup, _
                                  ByVal aExistAction As atcData.atcDataSource.EnumExistAction) As Boolean
        Try
            Logger.Status("Writing " & Format(aDataSets.Count, "#,##0") & " datasets to " & IO.Path.GetFileName(Specification), True)
            If IO.File.Exists(Specification) Then
                Dim lExtension As String = IO.Path.GetExtension(Specification)
                Dim lRenamedFilename As String = GetTemporaryFileName(Specification.Substring(0, Specification.Length - lExtension.Length), lExtension)
                Select Case aExistAction
                    Case EnumExistAction.ExistAppend

                    Case EnumExistAction.ExistAskUser
                        Select Case Logger.MsgCustom("Attempting to save but file already exists: " & vbCrLf & Specification, "File already exists", _
                                                     "Append", "Overwrite", "Do not write", "Save as " & IO.Path.GetFileName(lRenamedFilename))
                            Case "Append"

                            Case "Overwrite"
                                IO.File.Delete(Specification)
                            Case "Do not write"
                                Return False
                            Case Else
                                Specification = lRenamedFilename
                        End Select
                    Case EnumExistAction.ExistNoAction
                        Logger.Dbg("Save: File already exists and aExistAction = ExistNoAction, not saving " & Specification)
                        Return False
                    Case EnumExistAction.ExistReplace
                        Logger.Dbg("Save: File already exists, deleting old " & Specification)
                        IO.File.Delete(Specification)
                    Case EnumExistAction.ExistRenumber
                        Logger.Dbg("Save: File already exists and aExistAction = ExistRenumber, saving as " & lRenamedFilename)
                        Specification = lRenamedFilename
                End Select
            End If

            Dim lNumRecords As Integer = 0
            Dim lMaxFieldLength() As Integer = {0, 16, 8, 4, 10, 10}
            'Dim lAllDataSets As New atcDataGroup()
            'lAllDataSets.AddRange(Me.DataSets)
            'For Each lNewTS As atcTimeseries In aDataSets
            '    If Not lAllDataSets.Contains(lNewTS) Then
            '        lAllDataSets.Add(lNewTS)
            '    End If
            'Next
            For Each lTimeseries As atcTimeseries In Me.DataSets
                lNumRecords += lTimeseries.numValues
                lMaxFieldLength(1) = Math.Max(lMaxFieldLength(1), lTimeseries.Attributes.GetFormattedValue("Location").Length)
                lMaxFieldLength(4) = Math.Max(lMaxFieldLength(4), lTimeseries.Attributes.GetFormattedValue("Constituent").Length)
            Next
            For Each lTimeseries As atcTimeseries In aDataSets
                If Not Me.DataSets.Contains(lTimeseries) Then
                    lNumRecords += lTimeseries.numValues
                    lMaxFieldLength(1) = Math.Max(lMaxFieldLength(1), lTimeseries.Attributes.GetFormattedValue("Location").Length)
                    lMaxFieldLength(4) = Math.Max(lMaxFieldLength(4), lTimeseries.Attributes.GetFormattedValue("Constituent").Length)
                End If
            Next
            Dim lStartRecord As Integer = 1
            Dim lSaveDBF As New atcTableDBF
            With lSaveDBF
                Dim lFirstRecord As Boolean = True
                If IO.File.Exists(Specification) Then
                    lSaveDBF.OpenFile(Specification)
                    lStartRecord = lSaveDBF.NumRecords + 1
                    lNumRecords += lSaveDBF.NumRecords
                    .NumRecords = lNumRecords
                    lFirstRecord = False
                Else
                    lSaveDBF.NumFields = 5
                    For lFieldIndex As Integer = 1 To lSaveDBF.NumFields
                        lSaveDBF.FieldLength(lFieldIndex) = lMaxFieldLength(lFieldIndex)
                    Next
                    .FieldType(1) = "C" : .FieldName(1) = "ID"
                    .FieldType(2) = "D" : .FieldName(2) = "DATE"
                    .FieldType(3) = "C" : .FieldName(3) = "TIME"
                    .FieldType(4) = "C" : .FieldName(4) = "PARM"
                    .FieldType(5) = "N" : .FieldName(5) = "VALUE" : .FieldDecimalCount(5) = 2
                    .NumRecords = lNumRecords
                    .InitData()
                End If
                .CurrentRecord = lStartRecord
                Dim lDateArray(6) As Integer
                Dim lProgress As Integer = 0
                For Each lTimeseries As atcTimeseries In aDataSets
                    WriteDataset(lTimeseries, lSaveDBF, lFirstRecord)
                    lProgress += 1
                    Logger.Progress(lProgress, aDataSets.Count)
                Next
                .WriteFile(Specification)
                .Clear()
                Return True
            End With
        Catch e As Exception
            Logger.Msg("Error saving '" & Specification & "': " & e.ToString, MsgBoxStyle.OkOnly, "Did not write file")
            Return False
        End Try
    End Function

    Private Sub WriteDataset(ByVal aTimeseries As atcData.atcTimeseries, ByVal aDBF As atcTableDBF, ByRef aFirstRecord As Boolean)
        With aDBF
            Dim lDateArray(6) As Integer
            Dim lLocation As String = aTimeseries.Attributes.GetFormattedValue("Location")
            Dim lConstituent As String = aTimeseries.Attributes.GetFormattedValue("Constituent")
            For lTimeIndex As Integer = 1 To aTimeseries.numValues
                If aFirstRecord Then
                    aFirstRecord = False
                Else
                    .CurrentRecord += 1
                End If
                J2Date(aTimeseries.Dates.Value(lTimeIndex), lDateArray)
                .Value(1) = lLocation
                .Value(2) = Format(lDateArray(0), "0000") & Format(lDateArray(1), "00") & Format(lDateArray(2), "00")
                .Value(3) = Format(lDateArray(3), "00") & Format(lDateArray(4), "00")
                .Value(4) = lConstituent
                .Value(5) = DoubleToString(aTimeseries.Value(lTimeIndex), .FieldLength(5))
            Next
        End With
    End Sub

End Class