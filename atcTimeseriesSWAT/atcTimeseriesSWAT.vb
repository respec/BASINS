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
            Return "SWAT Output Text Files"
        End Get
    End Property

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Timeseries::SWATOutputText"
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
                If .OpenFile(MyBase.Specification) Then
                    Dim lTSBuilders As New atcData.atcTimeseriesGroupBuilder(Me)
                    Dim lTSBuilder As atcData.atcTimeseriesBuilder
                    Dim lField As Integer
                    Dim lCurrentRecord As Integer
                    Dim lFieldStart As Integer = 1
                    Dim lLocation As String
                    Dim lConstituent As String
                    Dim lDate As Double
                    Dim lYear As Integer
                    Dim lMonth As Integer
                    Dim lIncrementYear As Boolean = False
                    Dim lConstituentHeader As String = .Header(9)
                    Dim lNumConstituents As Integer = (lConstituentHeader.Length - 25) / 12
                    .NumFields = lNumConstituents + 3
                    For lField = 1 To .NumFields
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
                    For lCurrentRecord = 1 To .NumRecords
                        .CurrentRecord = lCurrentRecord
                        lYear = CInt(.Value(3).Trim)
                        If lYear > 12 Then Exit For
                    Next

                    For lCurrentRecord = 1 To .NumRecords
                        .CurrentRecord = lCurrentRecord
                        lLocation = .Value(1)
                        lMonth = CInt(.Value(3).Trim)
                        If lMonth > 12 Then
                            lYear = lMonth
                            lIncrementYear = True 'Next time it is a regular month it will be next year
                        ElseIf lIncrementYear Then
                            lYear += 1
                        End If
                        lDate = atcUtility.MJD(lYear, lMonth, 1)
                        For lField = 5 To .NumFields
                            lConstituent = .FieldName(lField)
                            lTSBuilder = lTSBuilders.Builder(lConstituent & "::" & lLocation)
                            'lTSBuilder.Attributes.SetValue("Constituent", lConstituent)
                            'lTSBuilder.Attributes.SetValue("Location", lLocation)
                            lTSBuilder.AddValue(lDate, CDbl(.Value(lField)))
                        Next
                    Next
                    lTSBuilders.CreateTimeseriesAddToGroup(Me.DataSets)
                    For Each lDataSet As atcData.atcDataSet In Me.DataSets
                        With lDataSet.Attributes
                            Dim lKeys() As String = .GetValue("Key").Split("::")
                            .SetValue("Constituent", lKeys(0))
                            .SetValue("Location", lKeys(1))
                        End With
                    Next
                    Return True
                Else
                    Logger.Dbg("Problem reading CliGen parameters into table in file " & aFileName & "." & vbCrLf & _
                               "Check format of specified CliGen file.")
                    Return False
                End If
            End With
        End If
    End Function

    Public Sub New()
        Filter = pFilter
    End Sub
End Class