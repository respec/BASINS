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
            Return False 'no saving yet, but could implement if needed 
        End Get
    End Property

    Public Overrides Function Open(ByVal aFileName As String, Optional ByVal aAttributes As atcData.atcDataAttributes = Nothing) As Boolean

        If MyBase.Open(aFileName, aAttributes) Then
            Dim lData As atcTimeseries
            Try
                Dim lDBF As IatcTable
                Dim lDateCol As Integer = -1
                Dim lTimeCol As Integer = -1
                Dim lLocnCol As Integer = -1
                Dim lConsCol As Integer = -1
                Dim lValCol As Integer = -1
                Dim lTSKey As String
                Dim lTSIndex As Integer
                Dim lLocation As String = ""
                Dim s As String
                Dim lNaN As Double = GetNaN()
                lDBF = New atcTableDBF
                lDBF.OpenFile(Specification)
                For i As Integer = 1 To lDBF.NumFields
                    s = UCase(lDBF.FieldName(i))
                    If s = "DATE" Then
                        lDateCol = i
                    ElseIf s = "TIME" Then
                        lTimeCol = i
                    ElseIf InStr(s, "ID") Then 'location
                        If lLocnCol = -1 Then 'only use first one
                            'should be sure that field is in use here
                            lLocnCol = i
                        End If
                    ElseIf s = "PARM" Then
                        lConsCol = i
                    ElseIf s = "VALUE" Then
                        lValCol = i
                    End If
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
        'assume point values at specified time
        Dim d(5) As Integer 'date array
        Dim l As Integer 'Length of year (2 or 4 digit year)
        Dim i As Integer 'Year offset (1900 for 2-digit year)

        If Not IsNumeric(aTime) Then aTime = "1200" 'assume noon for missing obstime
        If IsNumeric(aDate) Then
            If Len(aDate) = 8 Then ' 4 dig yr
                l = 4
                i = 0
            Else
                l = 2
                i = 1900
            End If
            d(0) = Left(aDate, l) + i
            d(1) = Mid(aDate, l + 1, 2)
            d(2) = Right(aDate, 2)
            If IsNumeric(aTime) Then
                If aTime.Length < 4 Then aTime = aTime.PadLeft(4, "0")
                d(3) = Left(aTime, 2)
                d(4) = Right(aTime, 2)
            End If
            Return Date2J(d)
        Else
            Return 0
        End If
    End Function

    Public Sub New()
        Filter = pFilter
    End Sub
End Class