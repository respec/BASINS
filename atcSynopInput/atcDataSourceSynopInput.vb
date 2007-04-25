Option Strict Off
Option Explicit On

Imports atcData
Imports atcUtility
Imports MapWinUtility
Imports Microsoft.VisualBasic
Imports System.Collections
Imports System.IO

Public Class atcDataSourceSynopInput
    Inherits atcDataSource
    '##MODULE_REMARKS Copyright 2007 AQUA TERRA Consultants - Royalty-free use permitted under open source license

    Private Shared pFileFilter As String = "Synop Input Files (*.inp)|*.inp"
    Private pErrorDescription As String

    Public Overrides ReadOnly Property Description() As String
        Get
            Return "Synop Input"
        End Get
    End Property

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Timeseries::SynopInput"
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

    Public Overrides ReadOnly Property CanSave() As Boolean
        Get
            Return True
        End Get
    End Property

    Public Overrides Function Open(ByVal aFileName As String, Optional ByVal aAttributes As atcData.atcDataAttributes = Nothing) As Boolean

        If aFileName Is Nothing OrElse aFileName.Length = 0 OrElse Not FileExists(aFileName) Then
            aFileName = FindFile("Select " & Name & " file to open", , , pFileFilter, True, , 1)
        End If

        If Not FileExists(aFileName) Then
            pErrorDescription = "File '" & aFileName & "' not found"
        Else
            Me.Specification = aFileName

            Try
                Dim lTable As New atcTableFixed
                Dim lTSIndex As Integer = 0
                Dim lNCons As Integer = 0
                Dim lConsName As String = "PRECIP"
                Dim lDate As Double = 0
                Dim lData As atcTimeseries
                Dim lDateArr(6) As Integer
                lDateArr(4) = 0 'No minutes in this file format
                lDateArr(5) = 0 'No seconds in this file format

                With lTable
                    .NumHeaderRows = 0

                    .NumFields = 6
                    .FieldStart(1) = 1
                    .FieldLength(1) = 6
                    .FieldName(1) = "Location"

                    .FieldStart(2) = 7
                    .FieldLength(2) = 10
                    .FieldName(2) = "Year"

                    .FieldStart(3) = 17
                    .FieldLength(3) = 10
                    .FieldName(3) = "Month"

                    .FieldStart(4) = 27
                    .FieldLength(4) = 10
                    .FieldName(4) = "Day"

                    .FieldStart(5) = 37
                    .FieldLength(5) = 10
                    .FieldName(5) = "Hour"

                    .FieldStart(6) = 47
                    .FieldLength(6) = 10
                    .FieldName(6) = "Value"

                    .OpenFile(aFileName)

                    lData = New atcTimeseries(Me)
                    lData.Dates = New atcTimeseries(Me)
                    lData.numValues = lTable.NumRecords
                    lData.Attributes.SetValue("Count", 0)
                    lData.Attributes.SetValue("Scenario", "OBSERVED")
                    lData.Attributes.SetValue("Location", lTable.Value(1))
                    lData.Attributes.SetValue("Constituent", lConsName)
                    lData.Attributes.SetValue("point", False)
                    DataSets.Add(lData)
                    'lData.Value(0) = Double.NaN
                    'lData.Dates.Value(0) = Double.NaN

                    While Not lTable.atEOF
                        lDateArr(0) = .Value(2)
                        lDateArr(1) = .Value(3)
                        lDateArr(2) = .Value(4)
                        lDateArr(3) = .Value(5)

                        lDate = Date2J(lDateArr)
                        If lDate <> 0 Then
                            lTSIndex += 1
                            lData.Value(lTSIndex) = .Value(6)
                            lData.Dates.Value(lTSIndex) = lDate
                        End If
                        lTable.MoveNext()
                    End While
                End With
                If lTSIndex <> lTable.NumRecords Then
                    Logger.Msg("Expected " & lTable.NumRecords & " records in '" & aFileName & "' but found " & lTSIndex, MsgBoxStyle.Exclamation, "Open Synop Input")
                    lData.numValues = lTSIndex
                End If
                Open = True
            Catch endEx As EndOfStreamException
                Open = False
            End Try
        End If
    End Function

    Public Overrides Function Save(ByVal aSaveFileName As String, _
                          Optional ByVal aExistAction As EnumExistAction = EnumExistAction.ExistReplace) As Boolean
        For Each lDataSet As atcTimeseries In Me.DataSets
            For lIndex As Integer = 1 To lDataSet.numValues
                Dim lDateArr(6) As Integer
                J2Date(lDataSet.Dates.Value(lIndex), lDateArr)
                timcnv(lDateArr)
                ' station?   year     month       day      hour hundredths of an inch?
                ' 11111      2003         1         9        14        14

                AppendFileString(aSaveFileName, " 00000" _
                                & StrPad(lDateArr(0), 10) _
                                & StrPad(lDateArr(1), 10) _
                                & StrPad(lDateArr(2), 10) _
                                & StrPad(lDateArr(3), 10) _
                                & StrPad(CInt(100 * lDataSet.Value(lIndex)), 10) & vbLf)
            Next
        Next
    End Function


End Class
