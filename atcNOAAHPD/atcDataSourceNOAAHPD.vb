Option Strict Off
Option Explicit On 

Imports atcData
Imports atcUtility
Imports MapWinUtility

Imports Microsoft.VisualBasic
Imports System.Collections
Imports System.IO

Public Class atcDataSourceNOAAHPD
    Inherits atcDataSource
    '##MODULE_REMARKS Copyright 2005 AQUA TERRA Consultants - Royalty-free use permitted under open source license

    Private Shared pFileFilter As String = "TD-3240 Files (*.ncd)|*.ncd|(*.*)|*.*"
    Private pErrorDescription As String
    Private pColDefs As Hashtable
    'Private pReadAll As Boolean = False

    Public Overrides ReadOnly Property Description() As String
        Get
            Return "NOAA Hourly Precip Data, Archive Format, TD-3240"
        End Get
    End Property

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Timeseries::NOAA HPD"
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
        Dim lData As atcTimeseries = Nothing
        Dim lDates As atcTimeseries = Nothing

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
            Dim maxRepeat As Integer = 24
            Dim lAccumStart As Integer = 0
            Dim lAddBlank As Integer = 0 'assume no blanks between fields
            Dim lRepeatsVary As Boolean = True 'assume repeats vary from record to record
            Dim lRepeatStart As Integer

            Dim MissingVal As Double = -999
            Dim MissingAcc As Double = -998

            Dim vals(100) As Double 'array of data values
            Dim dats(100) As Double 'array of julian dates
            Dim iVal As Integer = 0 'current index for populating vals and dats

            pColDefs = New Hashtable
            Dim ColRecType As ColDef = AddColEnd(1, 3, "RecType")
            Dim ColState As ColDef = AddColEnd(4, 5, "State")
            Dim ColLocation As ColDef = AddColEnd(4, 9, "Location")
            Dim ColElementType As ColDef = AddColEnd(12, 15, "ElementType")
            Dim ColUnits As ColDef = AddColEnd(16, 17, "Units")
            Dim ColYear As ColDef = AddColEnd(18, 21, "Year")
            Dim ColMonth As ColDef = AddColEnd(22, 23, "Month")
            Dim ColDay As ColDef = AddColEnd(26, 27, "Day")
            Dim ColRepeats As ColDef = AddColEnd(28, 30, "Repeats")
            lRepeatStart = 31

            'do 1st line read and column population to check format
            curLine = NextLine(inReader) 'read line to examine format
            If Mid(curLine, ColRecType.StartCol, 3) <> "HPD" OrElse _
               Mid(curLine, ColElementType.StartCol, 4) <> "HPCP" Then
                'not standard TD3240 format, try to adjust columns to work with format
                Dim lpos As Integer = InStr(curLine, "HPCP")
                If lpos > 0 Then
                    ColElementType.StartCol = lpos
                    If Mid(curLine, lpos + 4, 2) = "HI" OrElse Mid(curLine, lpos + 4, 2) = "HT" Then
                        ColUnits.StartCol = lpos + 4
                    ElseIf Mid(curLine, lpos + 5, 2) = "HI" OrElse Mid(curLine, lpos + 5, 2) = "HT" Then
                        'blanks between fields
                        lAddBlank = 1
                        ColUnits.StartCol = lpos + 5
                    Else 'no units found, not valid format
                        Logger.Dbg("PROBLEM:  No valid unit type found - Unit type found is '" & ColUnits.Value & "'")
                    End If
                    'now adjust start columns for ensuing fields
                    ColYear.StartCol = ColUnits.StartCol + 2 + lAddBlank
                    ColMonth.StartCol = ColYear.StartCol + 4 + lAddBlank
                    ColDay.StartCol = ColMonth.StartCol + 2 + lAddBlank
                    If Mid(curLine, ColDay.StartCol + 2 + lAddBlank, 4) = "0100" Then
                        'looks like 1st of month, assume all records have 24 values
                        lRepeatsVary = False
                        repeatsThisLine = 25 'include daily sum value ala varying record length format
                        lRepeatStart = ColDay.StartCol + 2 + lAddBlank
                    Else 'assume number of repeats follows
                        ColRepeats.StartCol = ColDay.StartCol + 2 + lAddBlank
                        ColRepeats.Value = Mid(curLine, ColRepeats.StartCol, ColRepeats.Length)
                        If Not IsNumeric(ColRepeats.Value) OrElse _
                           ColRepeats.Value < 0 OrElse ColRepeats.Value > 25 Then
                            'invalid number of repeats column
                            Logger.Dbg("PROBLEM:  Invalid number of repeats found - repeat value is '" & ColRepeats.Value & "'")
                        End If
                    End If
                    'see if station id is in 1st 6 chars
                    If IsNumeric(Mid(curLine, 1, 6)) Then 'assume it valid id
                        ColLocation.StartCol = 1
                        ColState.StartCol = 1 'assume state is 1st 2 characters
                    End If
                Else 'no HPCP element found, not valid format
                    Logger.Dbg("PROBLEM:  No valid Element type found - Element type found is '" & ColElementType.Value & "'")
                End If
            End If

            'These columns can repeat multiple times per line
            Dim ColHour(0) As ColDef
            Dim ColMinute(0) As ColDef
            Dim ColValue(0) As ColDef
            Dim ColFlag1(0) As ColDef
            Dim ColFlag2(0) As ColDef

            RedimensionRepeating(maxRepeat, lRepeatStart, lAddBlank, ColHour, ColMinute, ColValue, ColFlag1, ColFlag2)

            Try
                PopulateColumns(curLine)
                If lRepeatsVary Then repeatsThisLine = CInt(ColRepeats.Value)

                'If (ColRecType.Value = "HPD" AndAlso _
                If (ColElementType.Value = "HPCP" AndAlso _
                   (ColUnits.Value = "HI" OrElse ColUnits.Value = "HT") AndAlso _
                   IsNumeric(ColValue(0).Value) AndAlso _
                   IsNumeric(ColYear.Value) AndAlso _
                   IsNumeric(ColMonth.Value) AndAlso _
                   IsNumeric(ColDay.Value) AndAlso _
                   IsNumeric(ColHour(0).Value) AndAlso _
                   IsNumeric(ColMinute(0).Value) AndAlso _
                   ColYear.Value > 1700 AndAlso _
                   ColMonth.Value < 13 AndAlso _
                   ColDay.Value > 0 AndAlso _
                   ColDay.Value < 32 AndAlso _
                   ColHour(0).Value < 26 AndAlso _
                   ColMinute(0).Value <= 60) Then

                    lDates = New atcTimeseries(Me)
                    lData = New atcTimeseries(Me)
                    lData.Dates = lDates
                    lData.Attributes.SetValue("Scenario", "OBSERVED")
                    lData.Attributes.SetValue("Location", ColLocation.Value)
                    lData.Attributes.SetValue("Constituent", ColElementType.Value)
                    lData.Attributes.SetValue("Description", "Hourly Precip in Inches")
                    lData.Attributes.SetValue("tu", 3)
                    lData.Attributes.SetValue("ts", 1)
                    lData.Attributes.SetValue("point", False)
                    lData.Attributes.SetValue("TSFILL", MissingVal)
                    Me.AddDataSet(lData)
                    Dim InMissing As Boolean = False
                    Dim InAccum As Boolean = False
                    lData.Attributes.SetValue("MVal", MissingVal)
                    lData.Attributes.SetValue("MAcc", MissingAcc)
                    Do
                        If repeatsThisLine > maxRepeat Then
                            maxRepeat = repeatsThisLine
                            RedimensionRepeating(maxRepeat, lRepeatStart, lAddBlank, ColHour, ColMinute, ColValue, ColFlag1, ColFlag2)
                            PopulateColumns(curLine)
                        End If
                        For repeat = 0 To repeatsThisLine - 2
                            If ColFlag1(repeat).Value <> "I" Then
                                iVal = iVal + 1
                                If iVal > vals.GetUpperBound(0) Then
                                    ReDim Preserve vals(iVal * 2)
                                    ReDim Preserve dats(iVal * 2)
                                End If
                                If InMissing Then
                                    vals(iVal) = MissingVal
                                Else
                                    vals(iVal) = CDbl(ColValue(repeat).Value) / 100
                                End If
                                dats(iVal) = Jday(ColYear.Value, _
                                                   ColMonth.Value, _
                                                   ColDay.Value, _
                                                   ColHour(repeat).Value, _
                                                   ColMinute(repeat).Value, 0)
                                Select Case ColFlag1(repeat).Value
                                    Case " " 'Usually flag is blank

                                    Case "[", "{"
                                        vals(iVal) = MissingVal
                                        InMissing = True
                                    Case "]", "}"
                                        vals(iVal) = MissingVal
                                        InMissing = False
                                    Case "M", "D" 'Same as above Case "]", "}" ?
                                        vals(iVal) = MissingVal
                                        InMissing = False
                                    Case "a"
                                        InAccum = True
                                        lAccumStart = iVal
                                    Case "A"
                                        Select Case ColValue(repeat).Value
                                            Case "000000", "99999", " 99999"
                                                If ColFlag2(repeat).Value = "Q" Then
                                                    'accumulated period ends with flawed value, set all to missing
                                                    InAccum = False
                                                    For i As Integer = lAccumStart To iVal
                                                        vals(i) = MissingVal
                                                    Next
                                                Else
                                                    InAccum = True
                                                End If
                                            Case Else
                                                InAccum = False
                                        End Select
                                End Select
                                If InAccum Then vals(iVal) = MissingAcc
                                If ColFlag2(repeat).Value = "Q" Then 'log warning about Q code
                                    Logger.Dbg("WARNING: A Data Quality Flag of 'Q' was found for station " & ColLocation.Value & _
                                               " at " & ColYear.Value & "/" & ColMonth.Value & "/" & ColDay.Value & _
                                               " " & ColHour(repeat).Value & ":" & ColMinute(repeat).Value)
                                End If
                            End If
                        Next
                        curLine = NextLine(inReader)
                        PopulateColumns(curLine)
                        If lRepeatsVary Then repeatsThisLine = CInt(ColRepeats.Value)
                    Loop
                    Open = True
                Else
                    Logger.Dbg("PROBLEM:  Test of format for file " & aFileName & " failed." & vbCrLf & _
                               "Be sure this file follows the accepted NOAA TD-3240 format.")
                    Open = False
                End If
            Catch endEx As EndOfStreamException
                'round start and end dates to beginning/end of month
                Dim lDate(5) As Integer
                J2Date(dats(1), lDate)
                lDate(2) = 1 'first day of month
                lDate(3) = 0
                lDate(4) = 0
                dats(0) = Date2J(lDate)
                'see if last value fall in middle of the month
                J2Date(dats(iVal), lDate)
                If Not (lDate(2) = 1 AndAlso lDate(3) = 0) AndAlso _
                       (lDate(2) < daymon(lDate(0), lDate(1)) Or lDate(3) < 24) Then
                    'add value to finish the month
                    lDate(2) = daymon(lDate(0), lDate(1))
                    lDate(3) = 24
                    iVal += 1
                    ReDim Preserve vals(iVal)
                    ReDim Preserve dats(iVal)
                    vals(iVal) = 0
                    dats(iVal) = Date2J(lDate)
                Else 'resize arrays so NumValues get set to proper size
                    ReDim Preserve vals(iVal)
                    ReDim Preserve dats(iVal)
                End If
                lData.Values = vals
                lDates.Values = dats
                lData.Dates = lDates
                Dim lDataFilled As atcTimeseries = FillValues(lData, atcTimeUnit.TUHour, 1, 0.0, MissingVal, MissingAcc)
                lData.ValuesNeedToBeRead = False
                lDates.ValuesNeedToBeRead = False
                DataSets.Clear()
                DataSets.Add(lDataFilled)
                Open = True
            End Try
            inReader.Close()
            inBuffer.Close()
            inStream.Close()
        End If
    End Function

    Private Sub RedimensionRepeating(ByVal aMaximum As Integer, _
                                     ByVal aRptStart As Integer, _
                                     ByVal aAddBlank As Integer, _
                                     ByRef ColHour() As ColDef, _
                                     ByRef ColMinute() As ColDef, _
                                     ByRef ColValue() As ColDef, _
                                     ByRef ColFlag1() As ColDef, _
                                     ByRef ColFlag2() As ColDef)

        Dim oldMaximum As Integer = ColHour.GetUpperBound(0)
        Dim repeatOffset As Integer
        ReDim Preserve ColHour(aMaximum)
        ReDim Preserve ColMinute(aMaximum)
        ReDim Preserve ColValue(aMaximum)
        ReDim Preserve ColFlag1(aMaximum)
        ReDim Preserve ColFlag2(aMaximum)

        If oldMaximum = 0 Then oldMaximum = -1 'need to add first column

        For repeat As Integer = oldMaximum + 1 To aMaximum
            '12 characters in 4 fields repeat, with possible blanks between them
            repeatOffset = aRptStart + repeat * 12 + repeat * aAddBlank * 4
            'Hour and Minute are part of same data field, so no need to add possible blank between them
            ColHour(repeat) = AddColumn(repeatOffset, 2, "Hour." & repeat)
            ColMinute(repeat) = AddColumn(repeatOffset + 2, 2, "Minute." & repeat)
            ColValue(repeat) = AddColumn(repeatOffset + 4 + aAddBlank, 6, "Value." & repeat)
            ColFlag1(repeat) = AddColumn(repeatOffset + 10 + 2 * aAddBlank, 1, "Flag1." & repeat)
            ColFlag2(repeat) = AddColumn(repeatOffset + 11 + 3 * aAddBlank, 1, "Flag2." & repeat)
        Next

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