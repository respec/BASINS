Option Strict Off
Option Explicit On

Imports atcData
Imports atcUtility
Imports MapWinUtility
Imports Microsoft.VisualBasic
Imports System.Collections
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.String

Partial Public Class atcTimeseriesSWAT
    'Inherits atcTimeseriesSource
    ''##MODULE_REMARKS Copyright 2008 AQUA TERRA Consultants - Royalty-free use permitted under open source license
    ''##TZHAI: for reading and manipulating SWAT Met (PCP and TMP)
    'Now the SWAT met data access is via its ascii input format
    'the access (read and write) is part of the atcTimeseriesSWAT class
    'The ultimate assumption is that the order of timeseries read in is NOT to be changed
    'as this will impact both access and write procedures
    'The Open function can take in a string attribute as a list of station IDs for selective access
    'Usage:
    'Dim lSwatTS As New atcTimeseriesSWAT.atcTimeseriesSWAT
    'Dim lAtt As New atcDataAttributes
    '    lAtt.SetValue("GAGE", "74;88")
    '    lSwatTS.Open("M:\SWATCAT\pcp1.pcp", lAtt)
    '    lSwatTS.Save("", atcDataSource.EnumExistAction.ExistNoAction)
    'If the station list attribute is nothing, then all stations are read in

    Private pNumRecord As Integer = 0 ' in this class, the num record is the same as num values in a dataset

    Private Function OpenTableMet() As atcTable
        Dim lTable As atcTable
        Dim lConstituent As String = String.Empty
        Dim lTableStreaming As atcTableFixedStreaming = Nothing
        If IO.Path.GetFileNameWithoutExtension(Specification) = "tab" Then
            lTable = New atcTableDelimited
            pTableDelimited = True
        Else
            If IO.Path.GetExtension(Specification) = ".pcp" Then
                lConstituent = "PCP" ' mm
            ElseIf IO.Path.GetExtension(Specification) = ".tmp" Then
                lConstituent = "TMP"   ' degree C
            Else
                lConstituent = "SWATMET"
            End If
            lTableStreaming = New atcTableFixedStreaming
            lTable = lTableStreaming
            pTableDelimited = False
        End If

        With lTable

            .NumHeaderRows = 1
            pBaseDataField = 5

            If .OpenFile(Specification) Then
                'Set header text
                Attributes.SetValue("HeaderText", lTable.Header)
                Dim lConstituentHeader As String = lConstituent ' share common header, but later append the station id (i.e. column number)
                Dim lField As Integer
                Dim lLastField As Integer
                Dim lFieldStart As Integer = 1
                Select Case IO.Path.GetExtension(Specification).ToLower
                    Case ".pcp"
                        lLastField = CountMetStations(Specification)
                        .NumFields = lLastField
                        For lField = 1 To lLastField
                            Select Case lField
                                Case 1
                                    .FieldLength(lField) = 7
                                    .FieldName(lField) = "Date"
                                Case Else
                                    .FieldLength(lField) = 5
                                    .FieldName(lField) = lConstituentHeader & (lField - 1).ToString ' e.g. PCP1 PCP2 etc
                            End Select
                            If lTableStreaming IsNot Nothing Then lTableStreaming.FieldStart(lField) = lFieldStart
                            lFieldStart += .FieldLength(lField)
                        Next
                    Case ".tmp"
                        lLastField = CountMetStations(Specification) * 2 - 1
                        .NumFields = lLastField
                        For lField = 1 To lLastField
                            Select Case lField
                                Case 1
                                    .FieldLength(lField) = 7
                                    .FieldName(lField) = "Date"
                                Case Else
                                    .FieldLength(lField) = 5
                                    Dim ltmpVar As String = String.Empty
                                    Dim lstnNum As Integer
                                    If lField Mod 2 = 0 Then
                                        ltmpVar = "Max"
                                        lstnNum = lField / 2
                                    Else
                                        ltmpVar = "Min"
                                    End If
                                    .FieldName(lField) = lConstituentHeader & lstnNum.ToString & ltmpVar ' e.g. TMP1Max, TMP1Min etc
                            End Select
                            If lTableStreaming IsNot Nothing Then lTableStreaming.FieldStart(lField) = lFieldStart
                            lFieldStart += .FieldLength(lField)
                        Next
                    Case Else
                        Throw New ApplicationException("Unknown file extension for " & Specification)
                End Select
                'Should be two less than the formula below, it seems below counts the end of line character(s)??
                pRecordLength = lTableStreaming.FieldStart(.NumFields) + .FieldLength(.NumFields) + 1
                Return lTable
            Else
                lTable = Nothing
                Return Nothing
            End If
        End With
    End Function

    Private Function OpenMet(Optional ByVal aAttributes As atcData.atcDataAttributes = Nothing) As Boolean
        Dim lGAGEsToProcess() As String = Nothing
        Dim lDelim As String = ";"
        If aAttributes IsNot Nothing Then
            'Assuming client code passing in gages id in ascending order or have to sort it into ascending order
            If aAttributes.ContainsAttribute("GAGE") Then lGAGEsToProcess = aAttributes.GetValue("GAGE", "").ToString.Split(lDelim)
        End If

        Dim lKnowInterval As Boolean = False
        Dim lKnowYearBase As Boolean = False

        Dim lLocation As String = String.Empty
        Dim lConstituent As String = String.Empty
        Dim lSWATStnCtr As Integer
        Dim lFieldName As String = String.Empty

        Dim lLocations As New List(Of String)
        Dim lSubs As New List(Of String)
        Dim lFieldNums As New List(Of Integer)
        Dim lFieldNames As New List(Of String)

ReOpenTable:
        Dim lTable As atcTable = OpenTableMet()
        If lTable Is Nothing Then
            Logger.Dbg("Unable to open " & Specification)
            Return False
        Else
            Dim lDefScenario As atcAttributeDefinition = atcDataAttributes.GetDefinition("Scenario", True)
            Dim lDefLocation As atcAttributeDefinition = atcDataAttributes.GetDefinition("Location", True)
            Dim lDefConstituent As atcAttributeDefinition = atcDataAttributes.GetDefinition("Constituent", True)
            Dim lDefUnits As atcAttributeDefinition = atcDataAttributes.GetDefinition("Units", True)
            Dim lDefID As atcAttributeDefinition = atcDataAttributes.GetDefinition("ID", True)
            Dim lDefLatitude As atcAttributeDefinition = atcDataAttributes.GetDefinition("Latitude", True)
            Dim lDefLongitude As atcAttributeDefinition = atcDataAttributes.GetDefinition("Longitude", True)
            Dim lDefElevation As atcAttributeDefinition = atcDataAttributes.GetDefinition("Elevation", True)

            Me.Attributes.AddHistory("Read from " & Specification)
            Dim lAttHistory1 As atcDefinedValue = Me.Attributes.GetDefinedValue("History 1")

            Try
                Me.Attributes.SetValue(lDefScenario, IO.Path.GetFileName(IO.Path.GetDirectoryName(IO.Path.GetDirectoryName(Specification))))
            Catch
                Me.Attributes.SetValue(lDefScenario, "Simulated")
            End Try
            Dim lAttScenario As atcDefinedValue = Me.Attributes.GetDefinedValue("Scenario")

            Dim lDatesReading As New Generic.List(Of Double)

            Dim lLatitudes() As String = Nothing
            Dim lLongitudes() As String = Nothing
            Dim lElevations() As String = Nothing
            With lTable
                Dim lNumGages As Integer
                Select Case pType
                    Case SWATDATATYPE.SWATDATATYPE_pcp
                        lNumGages = .NumFields - 1
                        .CurrentRecord = 1
                        lLatitudes = .CurrentRecordAsDelimitedString(vbTab).Split(vbTab)
                        .CurrentRecord = 2
                        lLongitudes = .CurrentRecordAsDelimitedString(vbTab).Split(vbTab)
                        lTable.CurrentRecord = 3
                        lElevations = .CurrentRecordAsDelimitedString(vbTab).Split(vbTab)
                    Case SWATDATATYPE.SWATDATATYPE_tmp
                        lNumGages = (.NumFields - 1) / 2
                        ReDim lLatitudes(lNumGages + 1)
                        ReDim lLongitudes(lNumGages + 1)
                        ReDim lElevations(lNumGages + 1)

                        .CurrentRecord = 1
                        For lDSN As Integer = 2 To .NumFields Step 2
                            lLatitudes(lDSN / 2) = .Value(lDSN) & .Value(lDSN + 1)
                        Next
                        lTable.CurrentRecord = 2
                        For lDSN As Integer = 2 To .NumFields Step 2
                            lLongitudes(lDSN / 2) = .Value(lDSN) & .Value(lDSN + 1)
                        Next

                        lTable.CurrentRecord = 3
                        For lDSN As Integer = 2 To .NumFields Step 2
                            lElevations(lDSN / 2) = .Value(lDSN) & .Value(lDSN + 1)
                        Next
                End Select

                'Dim lTSBuilders As New atcData.atcTimeseriesGroupBuilder(Me)
                'Dim lTSBuilder As atcData.atcTimeseriesBuilder
                'Dim lFirstLocation As String = ""
                Logger.Status("Reading " & Format((.NumFields - pBaseDataField + 1), "#,###") & " constituents from " & Specification, True)

                '**********************for debug**********************
                'Dim lsw As New System.IO.StreamWriter("G:\SWATCAT\met\tmp1_headerinfo.txt", False)
                'Dim lLine As String = String.Empty
                'Dim lLineLong As String = String.Empty
                'Dim lLineElev As String = String.Empty

                ''write out header, Lati, Long, Elev
                'lLine &= "Lati".PadRight(7, " ")
                'lLineLong &= "Long".PadRight(7, " ")
                'lLineElev &= "Elev".PadRight(7, " ")
                ''Me.DataSets.SortedAttributeValues("FieldIndex")
                'For I As Integer = 1 To 131
                '    lLine &= String.Format("{0:00.0000000}", lLatitudes(I)).PadLeft(10, " ")
                '    lLineLong &= String.Format("{0:#00.000000}", lLongitudes(I)).PadLeft(10, " ")
                '    lLineElev &= String.Format("{0:#########0}", lElevations(I)).PadLeft(10, " ")
                'Next
                'lsw.WriteLine(lLine)
                'lsw.WriteLine(lLineLong)
                'lsw.WriteLine(lLineElev)
                'lsw.Flush() : lsw.Close()
                '********************for debug end***********************

                .CurrentRecord = 4
                Dim ldateStr As String = .Value(1).ToString.Trim
                Dim lyr As Integer = Integer.Parse(ldateStr.Substring(0, 4))
                Dim lDOY As Integer = Integer.Parse(ldateStr.Substring(4, 3))
                Dim lJdate As Double = atcUtility.Date2J(lyr, 1, 1) + lDOY - 1
                Dim lDate(5) As Integer

                While Not .EOF
                    ldateStr = .Value(1).ToString.Trim
                    lyr = Integer.Parse(ldateStr.Substring(0, 4))
                    lDOY = Integer.Parse(ldateStr.Substring(4, 3))
                    lJdate = atcUtility.Date2J(lyr, 1, 1) + lDOY - 1
                    atcUtility.J2Date(lJdate, lDate)

                    lDatesReading.Add(atcUtility.Jday(lyr, lDate(1), lDate(2), 0, 0, 0))
                    .CurrentRecord += 1
                    pNumRecord += 1
                End While
                lDatesReading.Add(atcUtility.Jday(lyr, lDate(1), lDate(2), 24, 0, 0))
            End With

            'For SWAT Met data, the num value in a dataset is the same as the number of records in the file
            pNumValues = pNumRecord
            pDates = New atcTimeseries(Me)

            Dim lDatasetsSoFar As Integer = 0
            Dim lTotalDatasets As Integer = lTable.NumFields - 1
            Dim lTS As atcData.atcTimeseries

            pDates.Values = lDatesReading.ToArray

            Logger.Status("Reading " & Format(lTotalDatasets, "#,###") & " datasets from " & Specification, True)
            Dim lunit As String = String.Empty
            Dim lIDRegex As New Regex("\d+")
            Dim lMatchCollection As MatchCollection = Nothing
            'Assuming SWAT Met data are all daily
            pTimeUnit = atcTimeUnit.TUDay
            For lFieldIndex As Integer = 2 To lTable.NumFields
                Try
                    lFieldName = lTable.FieldName(lFieldIndex)
                    lMatchCollection = lIDRegex.Matches(lFieldName)
                    If lMatchCollection IsNot Nothing Then
                        lSWATStnCtr = lMatchCollection(0).Value 'e.g. 1 in PCP1 TMP1Max
                        lMatchCollection = Nothing
                    Else
                        lSWATStnCtr = 1
                    End If

                    If lGAGEsToProcess IsNot Nothing Then
                        If System.Array.IndexOf(lGAGEsToProcess, lSWATStnCtr.ToString) < 0 Then
                            Continue For
                        End If
                    End If
                    'Do adjustment to PCP and Temp column index
                    If lFieldName.Contains("TMP") Then
                        lunit = "Celsius"
                    ElseIf lFieldName.Contains("PCP") Then
                        lunit = "mm"
                    End If
                    lTS = New atcTimeseries(Me)
                    DataSets.Add(lDatasetsSoFar + 1, lTS)
                    lDatasetsSoFar += 1
                    With lTS.Attributes
                        .Add(lAttHistory1)
                        .Add(lAttScenario)
                        .SetValue("SWATSTNID", lSWATStnCtr.ToString) ' SWAT station counter
                        .SetValue("FieldIndex", lFieldIndex.ToString) ' the column index in the pcp file, 1-based
                        .SetValue(lDefUnits, lunit)
                        lTS.SetInterval(pTimeUnit, 1) 'set tu, ts, interval
                        .SetValue(lDefLatitude, lLatitudes(lSWATStnCtr))
                        .SetValue(lDefLongitude, lLongitudes(lSWATStnCtr))
                        .SetValue(lDefElevation, lElevations(lSWATStnCtr))
                        Select Case Me.pType
                            Case SWATDATATYPE.SWATDATATYPE_pcp
                                .SetValue(lDefConstituent, "PREC")
                            Case SWATDATATYPE.SWATDATATYPE_tmp
                                .SetValue(lDefConstituent, "ATEM")
                        End Select
                        .SetValue(lDefLocation, lFieldName) ' same as SWAT station counter
                        .SetValue(lDefID, lDatasetsSoFar.ToString) ' sequential dataset counter, 1-based
                    End With
                    lTS.ValuesNeedToBeRead = True
                    lTS.Dates = pDates
                Catch ex As Exception
                    Logger.Dbg("Stopping opening SWAT output: " & ex.Message)
                    Exit For
                End Try
NextRecord:
                Logger.Progress(lDatasetsSoFar, lTotalDatasets)
            Next

            Logger.Dbg("Created " & Format(DataSets.Count, "#,##0") & " timeseries") ' from first " & Format(.CurrentRecord, "#,##0") & " records")
            Logger.Progress("", 0, 0)
            Return True
        End If
    End Function

    Private Sub ReadDataMet(ByVal aReadMe As atcDataSet)
        If Not DataSets.Contains(aReadMe) Then
            Logger.Dbg("Dataset not from this source:" & aReadMe.ToString & vbCrLf & _
                       "Source:'" & Specification & "'")
        Else

            Dim lReadTS As atcTimeseries
            Dim lReadThese As New Generic.List(Of atcTimeseries)
            Dim lUniqueLocations As New Generic.List(Of String)
            Dim lReadLocation As New Generic.List(Of Integer)
            Dim lReadField As New Generic.List(Of Integer)
            Dim lReadValues As New Generic.List(Of Double()) 'array of double data values for each timeseries


            If Me.DataSets.Count < 3000 Then
                'Reading all datasets at once is much faster than one at a time
                For Each lReadTS In Me.DataSets
                    If lReadTS.Serial = aReadMe.Serial OrElse lReadTS.ValuesNeedToBeRead Then
                        AddTsToList(lReadTS, lReadThese, lReadLocation, lUniqueLocations, lReadField, lReadValues)
                    End If
                Next
            Else
                AddTsToList(aReadMe, lReadThese, lReadLocation, lUniqueLocations, lReadField, lReadValues)
            End If
            Dim lFinalReadingIndex As Integer = lReadThese.Count - 1

            If lFinalReadingIndex < 0 Then
                Logger.Dbg("No datasets to read")
            Else
                Dim lTable As atcTable = OpenTableMet()
                If lTable Is Nothing Then
                    Logger.Dbg("Unable to open " & Specification)
                Else
                    Dim lVd() As Double
                    Dim lValueIndex As Integer = 0

                    With lTable
                        Dim lFieldIndex As Integer
                        .CurrentRecord = 4
                        While Not .EOF
                            lValueIndex += 1

                            Try
                                ' lTable 
                                For lDSctr As Integer = 0 To DataSets.Count - 1
                                    lFieldIndex = Integer.Parse(DataSets(lDSctr).Attributes.GetFormattedValue("FieldIndex"))
                                    lVd = lReadValues(lDSctr)
                                    If Not Double.TryParse(.Value(lFieldIndex).Trim, lVd(lValueIndex)) Then
                                        lVd(lValueIndex) = pNaN
                                    End If
                                    lReadValues(lDSctr) = lVd
                                Next
                            Catch ex As FormatException
                                Logger.Dbg("FormatException " & .CurrentRecord & ":" & lFieldIndex & ":" & .Value(lFieldIndex))
                            Catch ex As Exception
                                Logger.Dbg("Stopping reading SWAT output at record " & .CurrentRecord & ": " & ex.Message)
                            End Try
                            .CurrentRecord += 1
                        End While

                        'Logger.Dbg("Read " & Format(lNumTS, "#,##0") & " timeseries From " & Format(.CurrentRecord, "#,##0") & " Records")
                    End With

                    For lDSctr As Integer = 0 To DataSets.Count - 1
                        lReadTS = lReadThese(lDSctr)
                        lVd = lReadValues(lDSctr)
                        lValueIndex = lVd.Length - 1
                        If lValueIndex < pNumValues Then
                            ReDim Preserve lVd(lValueIndex)
                        End If

                        lReadTS.ValuesNeedToBeRead = False

                        lReadTS.Values = lVd
                        With lReadTS.Attributes
                            .SetValue("point", False)
                            .SetValue("TSFILL", pNaN)
                            .SetValue("MVal", pNaN)
                            .SetValue("MAcc", pNaN)
                        End With
                        Logger.Progress(lDSctr, lFinalReadingIndex)
                    Next
                    pNumValues = lValueIndex
                    Logger.Progress("", 0, 0)
                End If
            End If
            lReadThese.Clear()
            lReadLocation.Clear()
            lReadField.Clear()
            lReadValues.Clear()
        End If
    End Sub

    Private Sub WriteMetPcp(ByVal aFileName As String, ByVal aExistAction As EnumExistAction, Optional ByVal aStnList() As String = Nothing)
        If aExistAction = EnumExistAction.ExistNoAction Then
            aFileName = IO.Path.Combine(IO.Path.GetDirectoryName(Specification), IO.Path.GetFileNameWithoutExtension(Specification) & "_new.pcp")
        End If

        'Write it out
        Dim lsw As System.IO.StreamWriter = New System.IO.StreamWriter(aFileName, False)
        Dim ldarr(5) As Integer
        Dim ldoy As Integer
        Dim lLine As String = String.Empty
        Dim lLineLong As String = String.Empty
        Dim lLineElev As String = String.Empty

        'write out header, title
        lsw.Write(Attributes.GetValue("HeaderText", "Precipitation Input File" & vbCrLf))
        'write out header, Lati, Long, Elev
        lLine &= "Lati".PadRight(7, " ")
        lLineLong &= "Long".PadRight(7, " ")
        lLineElev &= "Elev".PadRight(7, " ")
        'Me.DataSets.SortedAttributeValues("FieldIndex")
        For I As Integer = 0 To Me.DataSets.Count - 1
            lLine &= String.Format("{0:00.00}", Me.DataSets(I).Attributes.GetValue("Latitude", 0)).PadLeft(5, " ")
            lLineLong &= String.Format("{0:#00.0}", Me.DataSets(I).Attributes.GetValue("Longitude", 0)).PadLeft(5, " ")
            lLineElev &= String.Format("{0:####0}", Me.DataSets(I).Attributes.GetValue("Elevation", 0)).PadLeft(5, " ")
        Next
        lsw.WriteLine(lLine)
        lsw.WriteLine(lLineLong)
        lsw.WriteLine(lLineElev)
        lLine = String.Empty
        lLineLong = String.Empty
        lLineElev = String.Empty

        Dim ldate As Date
        For d As Integer = 1 To Me.DataSets(0).numValues
            'print date
            J2Date(Me.DataSets(0).Dates.Value(d - 1), ldarr)
            ldate = New Date(ldarr(0), ldarr(1), ldarr(2))
            ldoy = ldate.DayOfYear()
            lLine &= String.Format("{0:0000}{1:000}", ldarr(0), ldoy)
            'lsw.Write("{0:0000}{1:000}", ldarr(0), ldoy) ' e.g. 1960009, the ninth day of 1960

            'print value
            If aStnList Is Nothing Then
                For i As Integer = 0 To Me.DataSets.Count - 1
                    lLine &= String.Format("{0:000.0}", Me.DataSets(i).Value(d))
                    'lsw.Write("{0:000.0}", Me.DataSets(i).Value(d))
                Next
            Else
                'For j As Integer = 1 To lTargetStnIDs.Length - 1
                '    If lTargetStnIDs(j) Is Nothing OrElse lTargetStnIDs(j) = "" Then
                '        Continue For
                '    End If
                '    For i As Integer = 0 To Me.DataSets.Count - 1
                '        If Integer.Parse(Me.DataSets(i).Attributes.GetFormattedValue("FieldIndex").ToString) = j Then
                '            lsw.Write("{0:000.0}", Me.DataSets(i).Value(d))
                '            Exit For
                '        End If
                '    Next
                'Next
            End If
            lsw.WriteLine(lLine)
            'lsw.Write(vbNewLine)
            lLine = String.Empty
        Next
        lsw.Close()
    End Sub

    Private Sub WriteMetTmp(ByVal aFileName As String, ByVal aExistAction As EnumExistAction, Optional ByVal aStnList() As String = Nothing)
        'Write it out
        If aExistAction = EnumExistAction.ExistNoAction Then
            aFileName = IO.Path.Combine(IO.Path.GetDirectoryName(Specification), IO.Path.GetFileNameWithoutExtension(Specification) & "_new.tmp")
        End If
        Dim lsw As System.IO.StreamWriter = New System.IO.StreamWriter(aFileName, False)
        Dim ldarr(5) As Integer
        Dim ldoy As Integer
        Dim lLine As String = String.Empty
        Dim lLineLong As String = String.Empty
        Dim lLineElev As String = String.Empty

        'write out header
        lsw.Write(Attributes.GetValue("HeaderText", "Temperature Input File" & vbCrLf))

        'write out header, Lati, Long, Elev
        lLine &= "Lati".PadRight(7, " ")
        lLineLong &= "Long".PadRight(7, " ")
        lLineElev &= "Elev".PadRight(7, " ")
        'Me.DataSets.SortedAttributeValues("FieldIndex")
        Dim lPrevStn As String = String.Empty
        Dim lThisStn As String = String.Empty
        For I As Integer = 0 To Me.DataSets.Count - 1
            lThisStn = Me.DataSets(I).Attributes.GetValue("SWATSTNID").ToString
            If lPrevStn <> lThisStn Then
                lLine &= String.Format("{0:00.0000000}", Me.DataSets(I).Attributes.GetValue("Latitude", 0)).PadLeft(10, " ")
                lLineLong &= String.Format("{0:#00.000000}", Me.DataSets(I).Attributes.GetValue("Longitude", 0)).PadLeft(10, " ")
                lLineElev &= String.Format("{0:#########0}", Me.DataSets(I).Attributes.GetValue("Elevation", 0)).PadLeft(10, " ")
                lPrevStn = lThisStn
            End If
        Next
        lsw.WriteLine(lLine)
        lsw.WriteLine(lLineLong)
        lsw.WriteLine(lLineElev)
        lLine = String.Empty
        lLineLong = String.Empty
        lLineElev = String.Empty

        Dim lval As Double = 0.0
        Dim ldate As Date
        Dim lvalFormatted As String = String.Empty
        For lValueIndex As Integer = 1 To Me.DataSets(0).numValues
            'print date
            J2Date(Me.DataSets(0).Dates.Value(lValueIndex - 1), ldarr)
            ldate = New Date(ldarr(0), ldarr(1), ldarr(2))
            ldoy = ldate.DayOfYear()
            lLine &= String.Format("{0:0000}{1:000}", ldarr(0), ldoy)
            'lsw.Write("{0:0000}{1:000}", ldarr(0), ldoy) ' e.g. 1960009, the ninth day of 1960

            If aStnList Is Nothing Then
                For i As Integer = 0 To Me.DataSets.Count - 1
                    lval = Me.DataSets(i).Value(lValueIndex)
                    lvalFormatted = String.Empty
                    If lval >= 0 Then
                        lvalFormatted = String.Format("{0:000.0}", lval)
                        'lsw.Write("{0:000.0}", lval)
                    Else
                        lvalFormatted = String.Format("{0:00.0}", lval)
                        'lsw.Write("{0:00.0}", lval)
                    End If
                    lLine &= lvalFormatted
                Next
            Else
                'For j As Integer = 1 To lStnID.Length - 1
                '    If lTargetStnIDs(j) Is Nothing OrElse lTargetStnIDs(j) = "" Then
                '        Continue For
                '    End If
                '    For i As Integer = 0 To Me.DataSets.Count - 1
                '        If Integer.Parse(Me.DataSets(i).Attributes.GetFormattedValue("FieldIndex").ToString) = j Then
                '            lval = Me.DataSets(i).Value(d + 1)
                '            If lval >= 0 Then
                '                lsw.Write("{0:000.0}", lval)
                '            Else
                '                lsw.Write("{0:00.0}", lval)
                '            End If
                '            Exit For
                '        End If
                '    Next
                'Next
            End If
            lsw.WriteLine(lLine)
            'lsw.Write(vbNewLine)
            lLine = String.Empty
        Next
        lsw.Close()
    End Sub

    Private Function CountMetStations(ByVal aSpecification As String) As Integer
        Dim lreader As System.IO.StreamReader = New System.IO.StreamReader(aSpecification)
        lreader.ReadLine()
        lreader.ReadLine()
        lreader.ReadLine()
        Dim line As String = lreader.ReadLine()
        Dim reg As New Regex("\s+")
        Dim out_string As String = reg.Replace(line, " ")
        lreader.Close()
        CountMetStations = out_string.Split(" ").GetLength(0)
    End Function

End Class
