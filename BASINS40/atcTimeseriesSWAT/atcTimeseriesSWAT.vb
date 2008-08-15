Option Strict Off
Option Explicit On

Imports atcData
Imports atcUtility
Imports MapWinUtility
Imports Microsoft.VisualBasic
Imports System.Collections
Imports System.IO

Public Class atcTimeseriesSWAT
    Inherits atcDataSource
    '##MODULE_REMARKS Copyright 2008 AQUA TERRA Consultants - Royalty-free use permitted under open source license

    Private Shared pFilter As String = "SWAT Output Files (output.*)|output.*"
    Private pNaN As Double = GetNaN()

    Public Overrides ReadOnly Property Description() As String
        Get
            Return "SWAT Output Text"
        End Get
    End Property

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Timeseries::" & Description
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
            Dim lDelim As String = ";"
            Dim lFieldsToProcess As String = ""
            If aAttributes IsNot Nothing Then
                lFieldsToProcess = lDelim & aAttributes.GetValue("FieldName", "") & lDelim
            End If
            Dim lTableDelimited As Boolean = False
            Dim lTable As atcTable
            If IO.Path.GetFileNameWithoutExtension(aFileName) = "tab" Then
                lTable = New atcTableDelimited
                lTableDelimited = True
            Else
                lTable = New atcTableFixedStreaming
            End If
            With lTable
                Dim lBaseDataField As Integer
                Dim lSubIdField As Integer
                If lTableDelimited Then
                    .NumHeaderRows = 0
                    CType(lTable, atcTableDelimited).Delimiter = vbTab
                    lBaseDataField = 6
                    lSubIdField = 3
                Else
                    .NumHeaderRows = 9
                    lBaseDataField = 4
                    lSubIdField = 2
                End If
                If .OpenFile(Specification) Then
                    Dim lTSBuilders As New atcData.atcTimeseriesGroupBuilder(Me)
                    Dim lTSBuilder As atcData.atcTimeseriesBuilder
                    Dim lField As Integer
                    Dim lFieldStart As Integer = 1
                    Dim lLocation As String
                    Dim lKey As String
                    Dim lDate As Double
                    Dim lYear As Integer
                    Dim lSaveSubwatershedId As Boolean = False
                    Dim lConstituentHeader As String = ""
                    If Not lTableDelimited Then
                        lConstituentHeader = CType(lTable, atcTableFixedStreaming).Header(9)
                    End If
                    Dim lLastField As Integer
                    Select Case IO.Path.GetExtension(Specification).ToLower
                        Case ".rch"
                            lLastField = 3 + (lConstituentHeader.Length - 25) / 12
                            .NumFields = lLastField
                            For lField = 1 To lLastField
                                Select Case lField
                                    Case 1 : .FieldLength(lField) = 10
                                    Case 2 : .FieldLength(lField) = 9
                                    Case 3 : .FieldLength(lField) = 6
                                    Case Else : .FieldLength(lField) = 12
                                End Select
                                .FieldName(lField) = Mid(lConstituentHeader, lFieldStart, .FieldLength(lField)).Trim
                                CType(lTable, atcTableFixedStreaming).FieldStart(lField) = lFieldStart
                                lFieldStart += .FieldLength(lField)
                            Next
                        Case ".sub"
                            lLastField = 3 + (lConstituentHeader.Length - 26) / 10
                            .NumFields = lLastField
                            For lField = 1 To lLastField
                                Select Case lField
                                    Case 1 : .FieldLength(lField) = 10
                                    Case 2 : .FieldLength(lField) = 9
                                    Case 3 : .FieldLength(lField) = 5
                                    Case 23 : .FieldLength(lField) = 12
                                    Case Else : .FieldLength(lField) = 10
                                End Select
                                .FieldName(lField) = Mid(lConstituentHeader, lFieldStart, .FieldLength(lField)).Trim
                                CType(lTable, atcTableFixedStreaming).FieldStart(lField) = lFieldStart
                                lFieldStart += .FieldLength(lField)
                            Next
                        Case ".hru", ".hrux"
                            lSaveSubwatershedId = True
                            If Not lTableDelimited Then
                                lLastField = 3 + (lConstituentHeader.Length - 35) / 10
                                .NumFields = lLastField
                                For lField = 1 To lLastField
                                    Select Case lField
                                        Case 1 : .FieldLength(lField) = 9
                                        Case 2 : .FieldLength(lField) = 5
                                        Case 3 : .FieldLength(lField) = 5
                                        Case 71, 72 : .FieldLength(lField) = 11
                                        Case Else : .FieldLength(lField) = 10
                                    End Select
                                    .FieldName(lField) = Mid(lConstituentHeader, lFieldStart, .FieldLength(lField)).Trim
                                    CType(lTable, atcTableFixedStreaming).FieldStart(lField) = lFieldStart
                                    Select Case lField
                                        Case 1 : lFieldStart = 19 'skip to sub
                                        Case 2 : lFieldStart = 29 'skip to mon
                                        Case Else : lFieldStart += .FieldLength(lField)
                                    End Select
                                Next
                            End If
                        Case Else
                            Throw New ApplicationException("Unknown file extension for " & Specification)
                    End Select

                    Logger.Status("Reading records for " & Format((.NumFields - lBaseDataField + 1), "#,###") & " constituents from " & Specification, True)
                    .CurrentRecord = 1
                    Dim lYearReading As Integer = 0
                    Dim lYearBase As Integer = 0
                    Do
                        If lTableDelimited Then
                            lLocation = .Value(1).ToString.Replace("""", "").PadLeft(4) & .Value(2).ToString.PadLeft(5)
                        Else
                            lLocation = .Value(1)
                        End If

                        'MON column assumed to hold year
                        Try
                            If Integer.TryParse(.Value(lBaseDataField - 1).Trim, lYear) Then
                                If lYear <> lYearReading Then
                                    Logger.Status("Reading year " & lYear)
                                    lYearReading = lYear
                                    If lYearBase = 0 Then lYearBase = lYear
                                End If
                                lDate = atcUtility.Jday(lYear, 12, 31, 24, 0, 0)
                                For lField = lBaseDataField To .NumFields
                                    Dim lFieldName As String = .FieldName(lField).ToString.Replace("""", "")
                                    If lFieldsToProcess.Length = 0 OrElse lFieldsToProcess.Contains(lDelim & lFieldName.Replace("_", "/") & lDelim) Then
                                        lKey = lFieldName & ":" & lLocation
                                        lTSBuilder = lTSBuilders.Builder(lKey)
                                        If lTSBuilder.NumValues = 0 Then
                                            Dim lYearFill As Integer = lYearBase
                                            While lYear >= lYearFill
                                                lTSBuilder.AddValue(atcUtility.Jday(lYearFill, 1, 1, 0, 0, 0), GetNaN)
                                                lYearFill += 1
                                            End While
                                            If lSaveSubwatershedId Then
                                                lTSBuilder.Attributes.Add("SubId", .Value(lSubIdField))
                                                lTSBuilder.Attributes.Add("CropId", lLocation.Substring(0, 4))
                                                lTSBuilder.Attributes.Add("HruId", lLocation.Substring(4, 5))
                                            End If
                                        End If
                                        lTSBuilder.AddValue(lDate, Double.Parse(.Value(lField).Trim))
                                    End If
                                Next
                            Else 'got to end of run summary, value is number of years as a decimal
                                Exit Do
                            End If
                        Catch ex As FormatException
                            Logger.Dbg("FormatException " & .CurrentRecord & ":" & lField & ":" & .Value(lField))
                        End Try
                        .CurrentRecord += 1
                    Loop
                    Logger.Dbg("Created " & lTSBuilders.Count & " Builders From " & .CurrentRecord & " Records")

                    Dim lTimeseriesGroup As atcDataGroup = lTSBuilders.CreateTimeseriesGroup()
                    Logger.Status("Updating Timeseries")

                    For Each lDataSet As atcData.atcTimeseries In lTimeseriesGroup
                        lDataSet = FillValues(lDataSet, atcTimeUnit.TUYear, 1, pNaN, pNaN, pNaN)
                        With lDataSet.Attributes
                            Dim lKeyParts() As String = .GetValue("Key").Split(":")
                            .SetValue("Scenario", "Simulate") 'TODO: get a name for the scenario
                            .SetValue("Units", SplitUnits(lKeyParts(0)).Trim)
                            .SetValue("Constituent", lKeyParts(0).Trim)
                            .SetValue("Location", lKeyParts(1).Trim)
                        End With
                        Me.DataSets.Add(lDataSet)
                        Logger.Progress(Me.DataSets.Count, lTSBuilders.Count)
                    Next
                    Logger.Dbg("Created " & Me.DataSets.Count & " DataSets")
                    Logger.Status("")
                    Return True
                Else
                    Logger.Dbg("Problem reading " & Specification)
                    Return False
                End If
            End With
        End If
    End Function

    Private Function SplitUnits(ByRef aConstituent As String) As String
        Dim lUnitsStart As Integer = 0
        For Each lChar As Char In aConstituent.ToCharArray
            If Char.IsLower(lChar) Then Exit For
            lUnitsStart += 1
        Next
        SplitUnits = aConstituent.Substring(lUnitsStart)
        aConstituent = aConstituent.Substring(0, lUnitsStart)
    End Function

    Public Sub New()
        Filter = pFilter
    End Sub
End Class