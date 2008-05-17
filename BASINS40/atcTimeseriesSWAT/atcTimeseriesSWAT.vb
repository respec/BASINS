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
    '##MODULE_REMARKS Copyright 2005 AQUA TERRA Consultants - Royalty-free use permitted under open source license

    Private Shared pFilter As String = "SWAT Output Files (output.*)|output.*"
    Private pColDefs As Hashtable
    'Private pReadAll As Boolean = False

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
            Dim lTable As New atcTableFixedStreaming
            With lTable
                .NumHeaderRows = 9
                If .OpenFile(Specification) Then
                    Dim lTSBuilders As New atcData.atcTimeseriesGroupBuilder(Me)
                    Dim lTSBuilder As atcData.atcTimeseriesBuilder
                    Dim lField As Integer
                    Dim lFieldStart As Integer = 1
                    Dim lLocation As String
                    Dim lKey As String
                    Dim lDate As Double
                    Dim lYear As Integer
                    Dim lConstituentHeader As String = .Header(9)
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
                                .FieldStart(lField) = lFieldStart
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
                                .FieldStart(lField) = lFieldStart
                                lFieldStart += .FieldLength(lField)
                            Next
                        Case ".hru", ".hrux"
                            lLastField = 3 + (lConstituentHeader.Length - 35) / 10
                            .NumFields = lLastField
                            For lField = 1 To lLastField
                                Select Case lField
                                    Case 1 : .FieldLength(lField) = 9
                                    Case 2 : .FieldLength(lField) = 19
                                    Case 3 : .FieldLength(lField) = 5
                                    Case 71, 72 : .FieldLength(lField) = 11
                                    Case Else : .FieldLength(lField) = 10
                                End Select
                                .FieldName(lField) = Mid(lConstituentHeader, lFieldStart, .FieldLength(lField)).Trim
                                .FieldStart(lField) = lFieldStart
                                lFieldStart += .FieldLength(lField)
                            Next
                        Case Else
                            Throw New ApplicationException("Unknown file extension for " & Specification)
                    End Select

                    Logger.Status("Reading records for " & Format((lLastField - 3), "#,###") & " constituents from " & Specification, True)
                    .CurrentRecord = 1
                    Do
                        lLocation = .Value(1)

                        'MON column assumed to hold year
                        Try
                            If Integer.TryParse(.Value(3).Trim, lYear) Then
                                Logger.Status("Reading year " & lYear)
                                lDate = atcUtility.Jday(lYear, 12, 31, 24, 0, 0)
                                For lField = 4 To lLastField
                                    Dim lFieldName As String = .FieldName(lField)
                                    If lFieldsToProcess.Length = 0 OrElse lFieldsToProcess.Contains(lDelim & lFieldName & lDelim) Then
                                        lKey = lFieldName & ":" & lLocation
                                        lTSBuilder = lTSBuilders.Builder(lKey)
                                        If lTSBuilder.NumValues = 0 Then
                                            lTSBuilder.AddValue(atcUtility.Jday(lYear, 1, 1, 0, 0, 0), Double.NaN)
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
                        'Logger.Progress(.CurrentRecord, ?)
                        .CurrentRecord += 1
                    Loop
                    Logger.Dbg("Created Builders From " & .CurrentRecord & " Records")
                    Logger.Progress("Creating Timeseries", 0, 0)
                    lTSBuilders.CreateTimeseriesAddToGroup(Me.DataSets)
                    Logger.Dbg("Created " & Me.DataSets.Count & " DataSets")
                    For Each lDataSet As atcData.atcTimeseries In Me.DataSets
                        With lDataSet.Attributes
                            Dim lKeyParts() As String = .GetValue("Key").Split(":")
                            .SetValue("Scenario", "Simulated")
                            .SetValue("Units", SplitUnits(lKeyParts(0)))
                            .SetValue("Constituent", lKeyParts(0))
                            .SetValue("Location", lKeyParts(1))
                            'TODO: next 4 are hard coded for annual wdm datasets, make more generic
                            .SetValue("tu", 6) 'annual
                            .SetValue("ts", 1)
                            .SetValue("tsbyr", 1900)
                            .SetValue("tgroup", 6)
                        End With
                    Next
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