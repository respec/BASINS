Option Strict Off
Option Explicit On 

Imports atcData
Imports atcUtility
Imports MapWinUtility

Imports Microsoft.VisualBasic
Imports System.Collections
Imports System.IO

Public Class atcDataSourceNOAAISH
    Inherits atcTimeseriesSource
    '##MODULE_REMARKS Copyright 2005 AQUA TERRA Consultants - Royalty-free use permitted under open source license

    Private Shared pFileFilter As String = "ISH Data Files (*.*)|*.*"
    Private pErrorDescription As String
    Private pColDefs As Hashtable
    'Private pReadAll As Boolean = False

    Public Overrides ReadOnly Property Description() As String
        Get
            Return "Integrated Surface Hourly Data"
        End Get
    End Property

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Timeseries::NOAA ISH"
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
            Dim curLine As String = ""
            Dim lNumVals As Integer
            Dim lPos As Integer

            Dim MissingVal As Double = -999
            Dim MissingAcc As Double = -998

            Dim vals(100) As Double 'array of data values
            Dim dats(100) As Double 'array of julian dates
            Dim iVal As Integer = 0 'current index for populating vals and dats
            Dim lBufSiz As Integer = 100
            Dim lTSInd As Integer
            Dim lJDate As Double
            Dim lCurVal As Double
            Dim lMissStr As String
            Dim lOrigHour As String
            Dim lChr As String

            pColDefs = New Hashtable
            Dim ColRecLength As ColDef = AddColEnd(1, 4, "RecLength")
            Dim ColISHStation As ColDef = AddColEnd(4, 8, "ISHStation")
            Dim ColWBANStation As ColDef = AddColEnd(4, 8, "WBANStation")
            Dim ColYear As ColDef = AddColEnd(9, 12, "Year")
            Dim ColMonth As ColDef = AddColEnd(13, 14, "Month")
            Dim ColDay As ColDef = AddColEnd(15, 16, "Day")
            Dim ColHour As ColDef = AddColEnd(36, 37, "Hour")
            Dim ColMinute As ColDef = AddColEnd(39, 40, "Minute")
            Dim ColDataSource As ColDef = AddColEnd(59, 59, "DataSource")

            '3 mandatory (Wind, ATemp, DPTemp), up to 4 PREC, and up to 6 SKYC-LAY and SKYC-SUM
            Dim ColValue(30) As ColDef
            Dim ColFlag1(30) As ColDef

            'do 1st line read and column population to check format
            curLine = NextLine(inReader) 'read line to examine format
            ''check position 29 
            'lChr = Mid(curLine, 29, 1)
            'If lChr = "+" OrElse lChr = "-" Then 'updated format (April 4, 2005)
            '    Dim ColReportType As ColDef = AddColEnd(29, 33, "ReportType")
            '    lPos = 66
            'Else 'older format (Feb 25, 2002)
            '    Dim ColReportType As ColDef = AddColEnd(29, 33, "ReportType")
            '    lpos = 39
            'End If
            'lPos = 30
            'set mandatory data fields/flags
            lNumVals = 0
            lPos = InStr(curLine, "WIND", CompareMethod.Text)
            If lPos > 0 Then
                'lPos += 8 'skip from direction to speed column
                ColValue(lNumVals) = AddColumn(lPos, 8, "WIND")
                ColFlag1(lNumVals) = AddColumn(lPos + 8, 8, "WINDFlag")
                lNumVals += 1
            End If
            lPos = InStr(curLine, "ATEMP", CompareMethod.Text)
            If lPos > 0 Then
                ColValue(lNumVals) = AddColumn(lPos, 8, "ATEMP")
                ColFlag1(lNumVals) = AddColumn(lPos + 8, 8, "ATEMPFlag")
                lNumVals += 1
            End If
            lPos = InStr(curLine, "DEWP", CompareMethod.Text)
            If lPos > 0 Then
                ColValue(lNumVals) = AddColumn(lPos, 8, "DPTEMP")
                ColFlag1(lNumVals) = AddColumn(lPos + 8, 8, "TEMPFlag")
                lNumVals += 1
            End If
            lPos = InStr(curLine, "PRECIP", CompareMethod.Text)
            If lPos > 0 Then
                lPos += 3 'skip from interval length to amount field
                ColValue(lNumVals) = AddColumn(lPos, 5, "HPCP1")
                ColFlag1(lNumVals) = AddColumn(lPos + 8, 1, "HPCP1Flag")
                lNumVals += 1
            End If
            lPos = InStr(curLine, "CLOUD", CompareMethod.Text)
            If lPos > 0 Then
                ColValue(lNumVals) = AddColumn(lPos, 8, "CLOU")
                ColFlag1(lNumVals) = AddColumn(lPos + 8, 8, "CLOUFlag")
                lNumVals += 1
            End If
            curLine = NextLine(inReader) 'skip 2nd header record

            Try
                curLine = NextLine(inReader)
                'SetDataColumns(curLine, lNumVals, ColValue, ColFlag1) 'find any additional data
                PopulateColumns(curLine)

                If (IsNumeric(ColYear.Value) AndAlso _
                   IsNumeric(ColMonth.Value) AndAlso _
                   IsNumeric(ColDay.Value) AndAlso _
                   IsNumeric(ColHour.Value) AndAlso _
                   IsNumeric(ColMinute.Value) AndAlso _
                   ColYear.Value > 1700 AndAlso _
                   ColMonth.Value < 13 AndAlso _
                   ColDay.Value > 0 AndAlso _
                   ColDay.Value < 32 AndAlso _
                   ColHour.Value < 24 AndAlso _
                   ColMinute.Value <= 60) Then

                    Do
                        'If ColDataSource.Value <> "9" AndAlso ColMinute.Value = "00" Then
                        '    'only use values recorded on the hour, not in between
                        lOrigHour = ColHour.Value
                        'If ColMinute.Value > 0 Then 'round minute to nearest hour
                        '    ColHour.Value += Math.Round(ColMinute.Value / 60 + Double.Epsilon)
                        'End If
                        If ColMinute.Value > 0 Then 'round to next hour
                            ColHour.Value += 1
                        End If
                        lJDate = Jday(ColYear.Value, ColMonth.Value, ColDay.Value, ColHour.Value, 0, 0)
                        'lJDate -= 10 / 24
                        For i As Integer = 0 To lNumVals - 1
                            lData = DataSets.ItemByKey(ColValue(i).Name)
                            If lData Is Nothing Then 'haven't encountered this constituent yet
                                lData = New atcTimeseries(Me)
                                lData.Dates = New atcTimeseries(Me)
                                lData.numValues = lBufSiz
                                lData.Value(0) = GetNaN()
                                lData.Dates.Value(0) = 0
                                lData.Attributes.SetValue("Count", 0)
                                lData.Attributes.SetValue("Scenario", "OBSERVED")
                                lData.Attributes.SetValue("Location", ColISHStation.Value)
                                lData.Attributes.SetValue("STAID", ColWBANStation.Value)
                                lData.Attributes.SetValue("Constituent", ColValue(i).Name)
                                lData.Attributes.SetValue("Description", "Integrated Surface Hourly Data")
                                lData.Attributes.SetValue("tu", 3)
                                lData.Attributes.SetValue("ts", 1)
                                lData.Attributes.SetValue("point", False)
                                lData.Attributes.SetValue("TSFILL", MissingVal)
                                DataSets.Add(ColValue(i).Name, lData)
                            End If
                            lTSInd = lData.Attributes.GetValue("Count")
                            If lTSInd >= lData.numValues Then 'expand buffer
                                lData.numValues = lTSInd * 2
                            End If
                            If ColValue(i).Length > 4 Then
                                lMissStr = "999.9".Substring(0, 5)
                            Else
                                lMissStr = "9999".Substring(0, ColValue(i).Length)
                            End If
                            If (Not ColValue(i).Name = "HPCP1-TM" OrElse
                               ColValue(i).Value = "01") AndAlso
                               IsNumeric(ColValue(i).Value) Then
                                If lJDate > lData.Dates.Value(lTSInd) Then
                                    lTSInd += 1
                                    lCurVal = CDbl(ColValue(i).Value)
                                Else 'duplicate records for same date, see if there's an existing value
                                    If Not ColValue(i).Value = lMissStr AndAlso
                                       Not ColFlag1(i).Value.Contains("7") AndAlso
                                       Not ColFlag1(i).Value.Contains("3") Then 'current value is valid
                                        lCurVal = lData.Value(lTSInd)
                                        If ColValue(i).Name = "HPCP1" AndAlso
                                           ColValue(i).Value > lCurVal Then 'use more recent larger precip value
                                            lCurVal = CDbl(ColValue(i).Value)
                                        ElseIf lData.Value(lTSInd) = MissingVal Then
                                            'existing value is missing, replace with current valid one
                                            lCurVal = CDbl(ColValue(i).Value)
                                        Else 'keep existing value
                                            lCurVal = GetNaN()
                                        End If
                                    Else 'missing or unwanted value
                                        lCurVal = GetNaN()
                                    End If
                                End If
                                If Not Double.IsNaN(lCurVal) Then
                                    lData.Dates.Value(lTSInd) = lJDate
                                    If lCurVal = CDbl(lMissStr) OrElse
                                       ColFlag1(i).Value = "3" OrElse
                                       ColFlag1(i).Value = "7" Then 'Or ColFlag1(i).Value = "9" Then
                                        lData.Value(lTSInd) = MissingVal
                                    Else
                                        Select Case ColValue(i).Name.Substring(0, 4)
                                            Case "WIND" 'convert m/s to mph
                                                lData.Value(lTSInd) = lCurVal' * 2.23694 'no scale factor / 4.47
                                            Case "ATEM", "DPTE" 'convert Deg C to Deg F
                                                lData.Value(lTSInd) = lCurVal' * 9 / 5 + 32
                                            Case "HPCP"
                                                If ColValue(i).Name.EndsWith("TM") Then
                                                    'keep time units as is
                                                    lData.Value(lTSInd) = lCurVal
                                                Else
                                                    'convert mm to inches
                                                    lData.Value(lTSInd) = lCurVal / 25.4
                                                End If
                                                'Case "CLOU"
                                                '    Select Case CInt(lCurVal)
                                                '        Case 0, 1
                                                '            lData.Value(lTSInd) = 0
                                                '        Case 2
                                                '            lData.Value(lTSInd) = 2.5
                                                '        Case 3, 4, 5
                                                '            lData.Value(lTSInd) = lCurVal + 1
                                                '        Case 6
                                                '            lData.Value(lTSInd) = 7.5
                                                '        Case 7, 8
                                                '            lData.Value(lTSInd) = lCurVal + 2
                                                '        Case Else
                                                '            lData.Value(lTSInd) = MissingVal
                                                '    End Select
                                                '    If lData.Value(lTSInd) > 10 Then
                                                '        lData.Value(lTSInd) = 10
                                                '    End If
                                            Case Else
                                                lData.Value(lTSInd) = lCurVal
                                        End Select
                                        If (ColFlag1(i).Value = "2" OrElse ColFlag1(i).Value = "6") AndAlso
                                           (ColValue(i).Name = "WIND" OrElse ColValue(i).Name = "ATEMP" OrElse
                                            ColValue(i).Name = "DPTEMP" OrElse ColValue(i).Name = "HPCP1") Then
                                            Logger.Dbg("SUSPECT (flag=" & ColFlag1(i).Value & ") value (" & ColValue(i).Value & ") for " & ColValue(i).Name &
                                                       " found at " & ColYear.Value & "/" &
                                                       ColMonth.Value & "/" & ColDay.Value & " " &
                                                       ColHour.Value & ":" & ColMinute.Value)
                                        End If
                                    End If
                                    lData.Attributes.SetValue("Count", lTSInd)
                                End If
                            Else 'Precip time value > 1, ignore it 
                                'move ahead one in loop to skip ensuing precip value
                                'Logger.Dbg("Skipping Precip value with Time > 1 for " & ColValue(i).Name & " on " & _
                                'ColYear.Value & "/" & ColMonth.Value & "/" & ColDay.Value & " " & _
                                'lOrigHour & ":" & ColMinute.Value)
                                i += 1
                            End If
                        Next
                        'End If

                        curLine = NextLine(inReader)
                        'SetDataColumns(curLine, lNumVals, ColValue, ColFlag1) 'find any additional data
                        PopulateColumns(curLine)
                    Loop
                    Open = True
                Else
                    Logger.Dbg("PROBLEM:  Test of format for file " & aFileName & " failed." & vbCrLf & _
                               "Be sure this file follows the accepted Integrated Surface Hourly Data format.")
                    Open = False
                End If
            Catch endEx As EndOfStreamException
                Dim lDataSets As New atcTimeseriesGroup
                Dim lDataFilled As atcTimeseries
                Dim lInd As Integer = 0
                For Each lData In DataSets
                    lInd += 1
                    lData.Attributes.SetValue("ID", lInd)
                    lData.numValues = lData.Attributes.GetValue("Count")
                    If lData.numValues > 0 Then
                        lData.Dates.Value(0) = lData.Dates.Value(1) - JulianHour 'set 0th date to start of 1st interval
                        If lData.Attributes.GetValue("Constituent").ToString.Substring(0, 4) = "HPCP" Then
                            lDataFilled = FillValues(lData, 3, 1, 0.0, MissingVal, MissingAcc)
                        Else
                            lDataFilled = FillValues(lData, 3, 1, MissingVal, MissingVal, MissingAcc)
                        End If
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
            Catch ex As Exception
                Logger.Dbg("PROBLEM processing the following record: " & vbCrLf & _
                           "     " & curLine & vbCrLf & "     " & ex.Message)
            End Try
            inReader.Close()
            inBuffer.Close()
            inStream.Close()
        End If
    End Function

    Private Sub SetDataColumns(ByVal aCurRec As String, _
                               ByRef aNumVals As Integer, _
                               ByRef ColValue() As ColDef, _
                               ByRef ColFlag1() As ColDef)

        Dim i As Integer

        aNumVals = 4 '3
        'remove all possible pre-existing additional data elements
        For i = 1 To 6
            If i < 5 Then
                pColDefs.Remove("HPCP" & i & "-TM")
                pColDefs.Remove("HPCP" & i & "-TMFlag")
                pColDefs.Remove("HPCP" & i)
                pColDefs.Remove("HPCP" & i & "Flag")
            End If
            'pColDefs.Remove("SKY-LAY" & i)
            'pColDefs.Remove("SKY-LAY" & i & "Flag")
            pColDefs.Remove("SKY-SST" & i)
            pColDefs.Remove("SKY-SST" & i & "Flag")
            pColDefs.Remove("SKY-SUM" & i)
            pColDefs.Remove("SKY-SUM" & i & "Flag")
        Next
        pColDefs.Remove("SKYCOND")
        pColDefs.Remove("SKYCOND" & "Flag")
        If aCurRec.Length > 81 AndAlso aCurRec.Substring(78).Contains("ADD") Then
            'now look for additional data elements
            Dim lPos As Integer
            Dim lRemPos As Integer
            If aCurRec.Contains("REM") Then 'find start position of remarks section
                lRemPos = InStr(81, aCurRec, "REM", CompareMethod.Text)
            Else 'no remarks, set position to extremely large
                lRemPos = Integer.MaxValue
            End If
            For i = 1 To 6 'look for up to 4 precip and 6 sky cover readings
                If i < 5 Then
                    lPos = InStr(81, aCurRec, "AA" & i, CompareMethod.Text)
                    If lPos > 0 AndAlso lPos < lRemPos Then
                        ColValue(aNumVals) = AddColumn(lPos + 3, 2, "HPCP" & i & "-TM")
                        ColFlag1(aNumVals) = AddColumn(lPos + 10, 1, "HPCP" & i & "-TMFlag")
                        aNumVals += 1
                        ColValue(aNumVals) = AddColumn(lPos + 5, 4, "HPCP" & i)
                        ColFlag1(aNumVals) = AddColumn(lPos + 10, 1, "HPCP" & i & "Flag")
                        aNumVals += 1
                    End If
                End If
                'lPos = InStr(81, aCurRec, "GA" & i, CompareMethod.Text)
                'If lPos > 0 AndAlso lPos < lRemPos Then
                '    ColValue(aNumVals) = AddColumn(lPos + 3, 2, "SKY-LAY" & i)
                '    ColFlag1(aNumVals) = AddColumn(lPos + 5, 1, "SKY-LAY" & i & "Flag")
                '    aNumVals += 1
                'End If
                lPos = InStr(81, aCurRec, "GD" & i, CompareMethod.Text)
                If lPos > 0 AndAlso lPos < lRemPos Then
                    ColValue(aNumVals) = AddColumn(lPos + 3, 1, "SKY-SST" & i)
                    ColFlag1(aNumVals) = AddColumn(lPos + 6, 1, "SKY-SST" & i & "Flag")
                    aNumVals += 1
                    ColValue(aNumVals) = AddColumn(lPos + 4, 2, "SKY-SUM" & i)
                    ColFlag1(aNumVals) = AddColumn(lPos + 6, 1, "SKY-SUM" & i & "Flag")
                    aNumVals += 1
                End If
            Next
            lPos = InStr(81, aCurRec, "GF1", CompareMethod.Text)
            If lPos > 0 AndAlso lPos < lRemPos Then
                ColValue(aNumVals) = AddColumn(lPos + 3, 2, "SKYCOND")
                ColFlag1(aNumVals) = AddColumn(lPos + 7, 1, "SKYCOND" & "Flag")
                aNumVals += 1
            End If
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