Option Strict Off
Option Explicit On 

Imports atcData
Imports atcUtility
Imports MapWinUtility

Imports Microsoft.VisualBasic
Imports System.Collections
Imports System.IO

Public Class atcDataSourceNOAA
    Inherits atcTimeseriesSource
    '##MODULE_REMARKS Copyright 2005 AQUA TERRA Consultants - Royalty-free use permitted under open source license

    Private Shared pFileFilter As String = "NOAA Files (*.txt)|*.txt|All Files (*,*)|*.*"
    Private pErrorDescription As String
    Private pColDefs As Hashtable
    'Private pReadAll As Boolean = False

    Public Overrides ReadOnly Property Description() As String
        Get
            Return "NOAA Summary of the Day, Archive Format, TD-3200"
        End Get
    End Property

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Timeseries::NOAA"
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

    Private Function AddColEnd(ByVal aStart As Integer, ByVal aEnd As Integer, ByVal aName As String) As ColDef
        Return AddColumn(aStart, aEnd - aStart + 1, aName)
    End Function

    Private Function AddColumn(ByVal aStart As Integer, ByVal aWidth As Integer, ByVal aName As String) As ColDef
        AddColumn = New ColDef(aStart, aWidth, aName)
        pColDefs.Add(aName, AddColumn)
    End Function

    Public Overrides Function Open(ByVal aFileName As String, Optional ByVal aAttributes As atcData.atcDataAttributes = Nothing) As Boolean
        Dim lData As atcTimeseries
        Dim lDataObs As atcTimeseries = Nothing
        Dim lDataFilled As atcTimeseries
        Dim lTSKey As String

        If aFileName Is Nothing OrElse aFileName.Length = 0 OrElse Not FileExists(aFileName) Then
            aFileName = FindFile("Select " & Name & " file to open", , , pFileFilter, True, , 1)
        End If

        If Not FileExists(aFileName) Then
            pErrorDescription = "File '" & aFileName & "' not found"
        Else
            Me.Specification = aFileName
            Dim inStream As New FileStream(aFileName, FileMode.Open, FileAccess.Read)
            Dim inBuffer As New BufferedStream(inStream)
            Dim inReader As New BinaryReader(inBuffer)
            Dim curLine As String
            Dim repeat As Integer = 0
            Dim repeatsThisLine As Integer
            Dim maxRepeat As Integer = 31
            Dim MissingVal As Double = -999
            Dim MissingAcc As Double = -998

            Dim lBufSiz As Integer = 100
            Dim vals(100) As Double 'array of data values
            Dim dats(100) As Double 'array of julian dates
            Dim iVal As Integer = 0 'current index for populating vals and dats

            'read 1st 3 chars to see which SOD format (short or long)
            curLine = inReader.ReadChars(3)
            Dim lLongForm As Boolean = False

            pColDefs = New Hashtable
            Dim ColRecType As ColDef
            Dim ColLocation As ColDef
            Dim ColElementType As ColDef
            Dim ColUnits As ColDef
            Dim ColYear As ColDef
            Dim ColMonth As ColDef
            Dim ColRepeats As ColDef
            If curLine = "DLY" Then 'short SOD format
                ColRecType = AddColEnd(1, 3, "RecType")
                ColLocation = AddColEnd(4, 9, "Location")
                ColElementType = AddColEnd(12, 15, "ElementType")
                ColUnits = AddColEnd(16, 17, "Units")
                ColYear = AddColEnd(18, 21, "Year")
                ColMonth = AddColEnd(22, 23, "Month")
                ColRepeats = AddColEnd(28, 30, "Repeats")
            ElseIf curLine = "320" OrElse curLine = "321" Then 'long SOD format (3200 or 3210)
                lLongForm = True
                ColRecType = AddColEnd(1, 3, "RecType")
                ColLocation = AddColEnd(6, 11, "Location")
                ColElementType = AddColEnd(53, 56, "ElementType")
                ColUnits = AddColEnd(58, 59, "Units")
                ColYear = AddColEnd(61, 64, "Year")
                ColMonth = AddColEnd(65, 66, "Month")
                'no # values element in this format
                'ColRepeats = AddColEnd(28, 30, "Repeats")
            End If
            'reset to start of file
            inReader.BaseStream.Position = 0

            'These columns can repeat multiple times per line
            Dim ColDay(0) As ColDef
            Dim ColHour(0) As ColDef
            Dim ColValue(0) As ColDef
            Dim ColFlag1(0) As ColDef
            Dim ColFlag2(0) As ColDef

            RedimensionRepeating(lLongForm, maxRepeat, ColDay, ColHour, ColValue, ColFlag1, ColFlag2)

            Try
                curLine = NextLine(inReader)
                PopulateColumns(curLine)
                If ColRecType.Value = "DLY" Then 'read # repeats from record
                    repeatsThisLine = ColRepeats.Value
                Else 'determine # repeats from year/month
                    repeatsThisLine = DayMon(ColYear.Value, ColMonth.Value)
                End If

                If ((ColRecType.Value = "DLY" Or ColRecType.Value = "320" Or ColRecType.Value = "321") AndAlso _
                   IsNumeric(ColValue(0).Value) AndAlso _
                   IsNumeric(ColYear.Value) AndAlso _
                   IsNumeric(ColMonth.Value) AndAlso _
                   IsNumeric(ColDay(0).Value) AndAlso _
                   IsNumeric(ColHour(0).Value) AndAlso _
                   ColYear.Value > 1700 AndAlso _
                   ColMonth.Value < 13 AndAlso _
                   ColDay(0).Value > 0 AndAlso _
                   ColDay(0).Value < 32 AndAlso _
                   (ColHour(0).Value < 26 Or ColHour(0).Value = 99)) Then

                    Dim InMissing As Boolean = False
                    Dim InAccum As Boolean = False
                    Dim lJDate As Double
                    Dim lTSInd As Integer
                    Dim lInd As Integer
                    Do
                        If repeatsThisLine > maxRepeat Then
                            maxRepeat = repeatsThisLine
                            RedimensionRepeating(lLongForm, maxRepeat, ColDay, ColHour, ColValue, ColFlag1, ColFlag2)
                            PopulateColumns(curLine)
                        End If
                        lTSKey = ColLocation.Value & ":" & ColElementType.Value
                        lData = DataSets.ItemByKey(lTSKey)
                        If lData Is Nothing Then
                            lData = New atcTimeseries(Me)
                            lData.Dates = New atcTimeseries(Me)
                            lData.numValues = lBufSiz
                            lData.Value(0) = GetNaN()
                            lData.Dates.Value(0) = GetNaN()
                            lData.Attributes.SetValue("Count", 0)
                            lData.Attributes.SetValue("Scenario", "OBSERVED")
                            lData.Attributes.SetValue("Location", ColLocation.Value)
                            lData.Attributes.SetValue("Constituent", ColElementType.Value)
                            lData.Attributes.SetValue("Description", "Summary of the Day")
                            lData.Attributes.SetValue("tu", 4)
                            lData.Attributes.SetValue("ts", 1)
                            lData.Attributes.SetValue("point", False)
                            lData.Attributes.SetValue("TSFILL", MissingVal)
                            lData.Attributes.SetValue("MVal", MissingVal)
                            lData.Attributes.SetValue("MAcc", MissingAcc)
                            DataSets.Add(lTSKey, lData)
                            lDataObs = lData.Clone
                            lDataObs.Attributes.SetValue("Count", 0)
                            lDataObs.Attributes.SetValue("Constituent", ColElementType.Value & "-OBS")
                            lDataObs.Attributes.SetValue("Description", "SOD Observation Times")
                            DataSets.Add(lTSKey & "-OBS", lDataObs)
                        Else 'find parallel obs time timeseries
                            lDataObs = DataSets.ItemByKey(lTSKey & "-OBS")
                        End If
                        lTSInd = lData.Attributes.GetValue("Count")
                        If lTSInd + repeatsThisLine > lData.numValues Then 'expand buffer
                            lData.numValues += lBufSiz
                            lDataObs.numValues += lBufSiz
                        End If
                        For repeat = 0 To repeatsThisLine - 1
                            'Data Quality Flag2 of 2 or 3 indicates invalid value
                            'subsequent valid coming for value of 2
                            If ColFlag2(repeat).Value <> "2" AndAlso _
                               ColFlag2(repeat).Value <> "3" AndAlso _
                               (ColDay(repeat).Value < 28 OrElse _
                               ColDay(repeat).Value <= DayMon(ColYear.Value, ColMonth.Value)) Then
                                'valid value and not exceeding days in month
                                lTSInd += 1
                                If ColValue(repeat).Value.IndexOf("99999") = -1 Then 'make sure its not a missing value
                                    Select Case ColUnits.Value
                                        Case "HI" 'hundredths of inches
                                            lData.Value(lTSInd) = CDbl(ColValue(repeat).Value) / 100
                                        Case "TI" 'tenths of inches
                                            lData.Value(lTSInd) = CDbl(ColValue(repeat).Value) / 10
                                        Case Else
                                            lData.Value(lTSInd) = CDbl(ColValue(repeat).Value)
                                    End Select
                                End If
                                lJDate = Jday(ColYear.Value, _
                                              ColMonth.Value, _
                                              ColDay(repeat).Value, _
                                              24, 0, 0)
                                lData.Dates.Value(lTSInd) = lJDate
                                lDataObs.Dates.Value(lTSInd) = lJDate
                                If ColHour(repeat).Value = "99" Then 'missing obs time
                                    lDataObs.Value(lTSInd) = MissingVal
                                Else 'save valid obs time
                                    lDataObs.Value(lTSInd) = ColHour(repeat).Value
                                End If
                                Select Case ColFlag1(repeat).Value
                                    Case " " 'Usually flag is blank
                                        If InAccum AndAlso lData.Value(lTSInd) > 0 Then
                                            'first positive value after start of accumulated end the accum period
                                            InAccum = False
                                        End If
                                    Case "S"
                                        InAccum = True
                                    Case "A", "B"
                                        InAccum = False
                                        'look for preceeding missing values that should be accumulated
                                        lInd = lTSInd - 1
                                        While lInd > 0 AndAlso lData.Value(lInd) = MissingVal
                                            lData.Value(lInd) = MissingAcc
                                            lInd -= 1
                                        End While
                                    Case "M"
                                        lData.Value(lTSInd) = MissingVal
                                End Select
                                If InAccum Then lData.Value(lTSInd) = MissingAcc
                                lData.Attributes.SetValue("Count", lTSInd)
                                lDataObs.Attributes.SetValue("Count", lTSInd)
                            End If
                        Next
                        curLine = NextLine(inReader)
                        'Logger.Progress("Processing NOAA SOD File", inStream.Position, inStream.Length)
                        PopulateColumns(curLine)
                        If ColRecType.Value = "DLY" Then 'read # repeats from record
                            repeatsThisLine = ColRepeats.Value
                        Else 'determine # repeats from year/month
                            repeatsThisLine = DayMon(ColYear.Value, ColMonth.Value)
                        End If
                    Loop
                    Open = True
                Else
                    Logger.Dbg("PROBLEM:  Test of format for file " & aFileName & " failed." & vbCrLf & _
                               "Be sure this file follows the accepted NOAA TD-3200 format.")
                    Open = False
                End If
            Catch endEx As EndOfStreamException
                Dim lDataSets As New atcTimeseriesGroup
                Dim lInd As Integer = 0
                For Each lData In DataSets
                    lData.numValues = lData.Attributes.GetValue("Count")
                    If lData.numValues > 0 Then
                        lInd += 1
                        lData.Attributes.SetValue("ID", lInd)
                        lData.Dates.Value(0) = lData.Dates.Value(1) - 1 'set 0th date to start of 1st interval
                        lDataFilled = FillValues(lData, 4, 1, MissingVal, MissingVal, MissingAcc)
                        If Not lDataFilled Is Nothing Then
                            lDataFilled.ValuesNeedToBeRead = False
                            lDataFilled.Dates.ValuesNeedToBeRead = False
                            lDataSets.Add(lDataFilled)
                        End If
                    End If
                Next
                DataSets.Clear() 'get rid of initial "unfilled" data sets
                For Each lData In lDataSets
                    DataSets.Add(lData)
                Next
                Open = True
            End Try
            inReader.Close()
            inBuffer.Close()
            inStream.Close()
            End If
    End Function

    Private Sub RedimensionRepeating(ByVal aLongForm As Boolean, _
                                     ByVal aMaximum As Integer, _
                                     ByRef ColDay() As ColDef, _
                                     ByRef ColHour() As ColDef, _
                                     ByRef ColValue() As ColDef, _
                                     ByRef ColFlag1() As ColDef, _
                                     ByRef ColFlag2() As ColDef)

        Dim oldMaximum As Integer = ColHour.GetUpperBound(0)
        Dim repeatOffset As Integer
        ReDim Preserve ColDay(aMaximum)
        ReDim Preserve ColHour(aMaximum)
        ReDim Preserve ColValue(aMaximum)
        ReDim Preserve ColFlag1(aMaximum)
        ReDim Preserve ColFlag2(aMaximum)

        If oldMaximum = 0 Then oldMaximum = -1 'need to add first column

        If aLongForm Then
            For repeat As Integer = oldMaximum + 1 To aMaximum
                repeatOffset = repeat * 16 '16 characters from 68 to 83 repeat
                ColDay(repeat) = AddColumn(68 + repeatOffset, 2, "Day." & repeat)
                ColHour(repeat) = AddColumn(70 + repeatOffset, 2, "Hour." & repeat)
                ColValue(repeat) = AddColumn(73 + repeatOffset, 6, "Value." & repeat)
                ColFlag1(repeat) = AddColumn(80 + repeatOffset, 1, "Flag1." & repeat)
                ColFlag2(repeat) = AddColumn(82 + repeatOffset, 1, "Flag2." & repeat)
            Next
        Else
            For repeat As Integer = oldMaximum + 1 To aMaximum
                repeatOffset = repeat * 12 '12 characters from 31 to 42 repeat
                ColDay(repeat) = AddColumn(31 + repeatOffset, 2, "Day." & repeat)
                ColHour(repeat) = AddColumn(33 + repeatOffset, 2, "Hour." & repeat)
                ColValue(repeat) = AddColumn(35 + repeatOffset, 6, "Value." & repeat)
                ColFlag1(repeat) = AddColumn(41 + repeatOffset, 1, "Flag1." & repeat)
                ColFlag2(repeat) = AddColumn(42 + repeatOffset, 1, "Flag2." & repeat)
            Next
        End If

    End Sub

    'Set values in pColDefs from current row/line of file
    Private Function PopulateColumns(ByVal aRow As String) As Boolean
        If pColDefs Is Nothing Then Return False
        Dim rowLength As Integer = aRow.Length
        For Each de As DictionaryEntry In pColDefs
            Dim cd As ColDef = de.Value
            If cd.StartCol > rowLength Then
                cd.Value = ""
            ElseIf cd.StartCol + cd.Length > rowLength Then
                cd.Value = Mid(aRow, cd.StartCol)
            Else
                cd.Value = Mid(aRow, cd.StartCol, cd.Length)
            End If
        Next
    End Function

    Public Overrides Sub ReadData(ByVal aReadMe As atcData.atcDataSet)
        If Not Me.DataSets.Contains(aReadMe) Then
            System.Diagnostics.Debug.WriteLine("Cannot read data: not from this file")
        Else

            'Should not ever get here since we are now reading all data in Open

            'pReadAll = True
            'Open(Me.FileName)
            'pReadAll = False
        End If
    End Sub

    Private Class ColDef
        Public Name As String
        Public StartCol As Long
        Public Length As Integer
        Public Value As String

        Public Sub New(ByVal aStart As Integer, ByVal aWidth As Integer, ByVal aName As String)
            Me.StartCol = aStart
            Me.Length = aWidth
            Me.Name = aName
        End Sub

    End Class

End Class