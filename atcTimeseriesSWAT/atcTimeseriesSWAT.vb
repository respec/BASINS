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

    Private Shared pFilter As String = "SWAT Output Files (*.rch, *.sub, *.dat)|*.rch;*.sub;*.dat"
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
            Dim lTable As New atcTableFixed
            With lTable
                .NumHeaderRows = 9
                If .OpenFile(Specification) Then
                    Dim lTSBuilders As New atcData.atcTimeseriesGroupBuilder(Me)
                    Dim lTSBuilder As atcData.atcTimeseriesBuilder
                    Dim lField As Integer
                    Dim lCurrentRecord As Integer
                    Dim lLastRecord As Integer = .NumRecords
                    Dim lFieldStart As Integer = 1
                    Dim lLocation As String
                    Dim lKey As String
                    Dim lDate As Double
                    Dim lYear As Integer
                    Dim lMonth As Integer
                    Dim lDay As Integer = 1
                    Dim lDaily As Boolean = False
                    Dim lYearly As Boolean = False
                    Dim lConstituentHeader As String = .Header(9)
                    Dim lLastField As Integer = 3 + (lConstituentHeader.Length - 25) / 12
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

                    'Scan for first record with a year instead of a month in column 3
                    For lCurrentRecord = 1 To lLastRecord
                        .CurrentRecord = lCurrentRecord
                        lYear = Integer.Parse(.Value(3).Trim)
                        If lYear > 12 Then Exit For
                    Next

                    If lYear = 13 Then
                        lDaily = True
                        lYear = 1900
                    End If

                    Logger.Status("Reading " & Format(lLastRecord, "#,###") & " records * " & Format((lLastField - 3), "#,###") & " constituents from " & Specification, True)
                    For lCurrentRecord = 1 To lLastRecord
                        .CurrentRecord = lCurrentRecord
                        lLocation = .Value(1)

                        'MON column may hold month or day or year
                        Try
                            lMonth = Integer.Parse(.Value(3).Trim)
                            If lDaily Then
                                If lMonth < lDay Then
                                    lYear += 1
                                End If
                                lDay = lMonth
                                lMonth = 1
                            Else
                                If lMonth > 12 Then
                                    lYear = lMonth
                                    lYearly = True
                                    lMonth = 1
                                    Logger.Status("Reading year " & lYear)
                                ElseIf lYearly Then
                                    lYear += 1
                                    lYearly = False
                                End If
                                lDay = atcUtility.daymon(lYear, lMonth)
                            End If

                            lDate = atcUtility.Jday(lYear, lMonth, lDay, 24, 0, 0)
                            For lField = 4 To lLastField
                                lKey = .FieldName(lField) & ":" & lLocation
                                If lYearly Then lKey &= ":Y"
                                lTSBuilder = lTSBuilders.Builder(lKey)
                                lTSBuilder.AddValue(lDate, Double.Parse(.Value(lField).Trim))
                            Next
                        Catch ex As FormatException
                        End Try
                        Logger.Progress(lCurrentRecord, lLastRecord)
                    Next
                    'Logger.Dbg("Created Builders")
                    Logger.Progress("Creating Timeseries", 0, 0)
                    lTSBuilders.CreateTimeseriesAddToGroup(Me.DataSets)
                    For Each lDataSet As atcData.atcTimeseries In Me.DataSets
                        With lDataSet.Attributes
                            Dim lKeyParts() As String = .GetValue("Key").Split(":")
                            .SetValue("Scenario", "Simulated")
                            .SetValue("Units", SplitUnits(lKeyParts(0)))
                            .SetValue("Constituent", lKeyParts(0))
                            .SetValue("Location", lKeyParts(1))
                        End With
                    Next
                    Logger.Status("")
                    Return True
                Else
                    Logger.Dbg("Problem reading CliGen parameters into table in file " & aFileName & "." & vbCrLf & _
                               "Check format of specified CliGen file.")
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