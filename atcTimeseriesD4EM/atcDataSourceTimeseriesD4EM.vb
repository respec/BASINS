Imports atcUtility
Imports atcData
Imports MapWinUtility

Public Class atcDataSourceTimeseriesD4EM
    Inherits atcTimeseriesSource
    '##MODULE_REMARKS Copyright 2008 AQUA TERRA Consultants - Royalty-free use permitted under open source license

    Private Shared pFilter As String = "D4EM Text Files (*.txt)|*.txt"
    Private Shared pVersion As String = "#D4EM Met Data"
    Private Shared pColumns() As String = {"pcp", "tmin", "tmean", "tmax", "slr", "wnd", "hmd", "pet", "clo", "dew"}

    Public DataColumnWidth As Integer = 15
    Public DataDecimalWidth As Integer = 6
    Public DayColumnWidth As Integer = 10

    Public Overrides ReadOnly Property Description() As String
        Get
            Return "Timeseries D4EM"
        End Get
    End Property

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Timeseries::D4EM"
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
        If Not MyBase.Open(aFileName, aAttributes) Then
            Return False
        Else
            If IO.File.Exists(Specification) Then
                Logger.Status("Reading " & IO.Path.GetFileName(Specification), True)
                'Dim lFileStream As New IO.FileStream(Specification, IO.FileMode.Open, IO.FileAccess.Read)
                'Dim lReader As New IO.StreamReader(lFileStream)
                'Dim lVersion As String = lReader.ReadLine
                'If lVersion <> pVersion Then
                '    Logger.Dbg("Unknown D4EM file version: " & lVersion & "<>" & pVersion)
                '    lReader.Close()
                '    Return False
                'Else
                '    Try
                '        Do
                '            Dim lData As New atcTimeseries(Me)
                '            Dim lAttributeName As String
                '            Dim lAttributeValue As String
                '            Dim lAttributeType As String
                '            Do
                '                lAttributeName = lReader.ReadLine
                '                If lAttributeName = "<done>" Then
                '                    Exit Do
                '                End If
                '                lAttributeType = lReader.ReadLine
                '                lAttributeValue = lReader.ReadLine
                '                Select Case lAttributeType(0)
                '                    Case "I" : lData.Attributes.SetValue(lAttributeName, CInt(lAttributeValue))
                '                    Case "D" : lData.Attributes.SetValue(lAttributeName, CDbl(lAttributeValue))
                '                    Case Else
                '                        lData.Attributes.SetValue(lAttributeName, lAttributeValue)
                '                End Select
                '            Loop

                '            Dim lNumDates As Integer = lData.Attributes.GetValue("NumValues")
                '            Dim lDateStartDate As Date = Date.Parse(lData.Attributes.GetValue("Start Date"))
                '            Dim lTimeUnits As Object = lData.Attributes.GetValue("tu", atcTimeUnit.TUDay)
                '            Dim lTU As atcUtility.atcTimeUnit
                '            If lTimeUnits.GetType.IsInstanceOfType("") Then
                '                lTU = System.Enum.Parse(lTU.GetType, lTimeUnits)
                '                lData.Attributes.SetValue("tu", lTU)
                '            Else
                '                lTU = lTimeUnits
                '            End If
                '            Dim lTimeStep = lData.Attributes.GetValue("ts", 1)

                '            Dim lDateStartJulian As Double = lDateStartDate.ToOADate
                '            Dim lDates(Math.Abs(lNumDates)) As Double
                '            lDates(0) = lDateStartJulian
                '            For lIndex As Integer = 1 To lNumDates
                '                lDates(lIndex) = TimAddJ(lDateStartJulian, lTU, lTimeStep, lIndex)
                '            Next
                '            lData.Dates = New atcTimeseries(Me)
                '            lData.Dates.Values = lDates

                '            Dim lValues(lNumDates) As Double
                '            For lIndex As Integer = 0 To lNumDates
                '                lValues(lIndex) = CDbl(lReader.ReadLine.Substring(10).Trim)
                '            Next
                '            lData.Values = lValues
                '            DataSets.Add(lData)
                '            Logger.Progress(lFileStream.Position, lFileStream.Length)
                '        Loop
                '    Catch ex As IO.EndOfStreamException
                '        Logger.Status("")
                '        Logger.Dbg("Read " & DataSets.Count & " from " & Specification)
                '    End Try
                '    lReader.Close()
                'End If
            End If
        End If
        Return True
    End Function

    Private Sub WriteDatasetAttributes(ByVal aDataSet As atcData.atcDataSet, ByVal aFileName As String)
        Dim lFileStream As New IO.FileStream(aFileName, IO.FileMode.Append)
        Dim lWriter As New IO.StreamWriter(lFileStream)
        Dim lValue As String
        If lFileStream.Position = 0 Then
            lWriter.WriteLine(pVersion & " Attributes")
        End If

        If aDataSet Is Nothing Then
            lWriter.WriteLine("Data set not present")
        Else
            For Each lAttribute As atcDefinedValue In aDataSet.Attributes
                If Not lAttribute.Definition.Calculated Then
                    Dim lName As String = lAttribute.Definition.Name
                    Select Case lName
                        Case "Key", "Data Source"
                        Case Else
                            Dim lType As String = ""
                            Select Case lAttribute.Definition.TypeString
                                Case "String" : lType = "Str"
                                Case "Integer" : lType = "Int"
                                Case "Single" : lType = "Sgl"
                                Case "Double" : lType = "Dbl"
                                Case "atcTimeUnit" : lType = "Str"
                                Case Else
                                    Logger.Dbg("AttributeTypeNotDefined:" & lAttribute.Definition.TypeString)
                            End Select
                            If lType.Length > 0 Then
                                lValue = lAttribute.Value.ToString.TrimEnd
                                lWriter.WriteLine(lName)
                                lWriter.WriteLine(lType & "(" & lValue.Length & ")")
                                lWriter.WriteLine(lValue)
                            End If
                    End Select
                End If
            Next

            Dim lTimeseries As atcTimeseries = aDataSet

            lValue = lTimeseries.numValues
            lWriter.WriteLine("NumValues")
            lWriter.WriteLine("Int(" & lValue.Length & ")")
            lWriter.WriteLine(lValue)

            lValue = Format(Date.FromOADate(lTimeseries.Dates.Value(0)), "yyyy-MM-dd")
            lWriter.WriteLine("Start Date")
            lWriter.WriteLine("Str(" & lValue.Length & ")")
            lWriter.WriteLine(lValue)

            lWriter.Write("<done>")

            'Dim lTimeUnits As atcTimeUnit = lTimeseries.Attributes.GetValue("tu", atcTimeUnit.TUDay)
            'Dim lTimeStep As Integer = lTimeseries.Attributes.GetValue("ts", 1)
            'Dim lDateEndComputed As Double = TimAddJ(lTimeseries.Dates.Value(0), lTimeUnits, lTimeStep, lTimeseries.numValues)
            'If Math.Abs(lDateEndComputed - lTimeseries.Dates.Value(lTimeseries.numValues)) > 0.00001 Then
            '    Logger.Dbg("Greater than expected difference between actual (" & Date.FromOADate(lTimeseries.Dates.Value(lTimeseries.numValues)).ToShortDateString _
            '             & ") and computed (" & Date.FromOADate(lDateEndComputed).ToShortDateString & ") end dates")
            'End If
            'Dim lValues() As Double = lTimeseries.Values
            'For lIndex As Integer = 0 To lTimeseries.numValues
            '    lWriter.WriteLine(((lIndex + 1) & " ").PadLeft(DayColumnWidth) & DecimalAlign(lValues(lIndex), DataColumnWidth, DataDecimalWidth))
            'Next
            'todo: write value attributes (if any)
        End If

        lWriter.Close()
    End Sub

    Public Overrides Function Save(ByVal aSaveFileName As String, _
                          Optional ByVal aExistAction As EnumExistAction = EnumExistAction.ExistReplace) As Boolean
        Me.Specification = aSaveFileName
        Dim lGroup As New atcDataGroup
        Dim lTS As atcTimeseries = DataSets(0)
        Dim lStartDate As Double = lTS.Dates.Value(0)
        Dim lEndDate As Double = lTS.Dates.Value(lTS.numValues)
        Dim lNumDays As Integer = lTS.numValues
        Dim lColumnName As String
        For Each lColumnName In pColumns
            If DataSets.Keys.Contains(lColumnName) Then
                lTS = DataSets.ItemByKey(lColumnName)
            ElseIf lColumnName = "hmd" Then
                'Default relative humidity to 40%
                lTS = NewTimeseries(lStartDate, lEndDate, atcTimeUnit.TUDay, 1, Me, 40)
                lTS.Attributes.SetValueIfMissing("Units", "%")
            Else
                lTS = Nothing
            End If
            lGroup.Add(lColumnName, lTS)
            WriteDatasetAttributes(lTS, aSaveFileName & "." & lColumnName & "-attributes.txt")
        Next

        Dim lFileStream As New IO.FileStream(Specification, IO.FileMode.Create)
        Dim lWriter As New IO.StreamWriter(lFileStream)
        lWriter.WriteLine(pVersion & " Start Date " & Format(Date.FromOADate(lStartDate), "yyyy-MM-dd") & " Number of Days " & lNumDays)

        'Column Titles
        lWriter.Write("Day".PadLeft(DayColumnWidth))
        For Each lColumnName In pColumns
            lWriter.Write(lColumnName.PadLeft(DataColumnWidth - DataDecimalWidth + 1).PadRight(DataColumnWidth))
        Next
        lWriter.WriteLine()

        'Units row
        lWriter.Write("".PadLeft(DayColumnWidth))
        For Each lTS In lGroup
            If lTS Is Nothing Then
                lWriter.Write(Space(DataColumnWidth))
            Else
                lWriter.Write(lTS.Attributes.GetValue("Units", "").PadLeft(DataColumnWidth - DataDecimalWidth + 1).PadRight(DataColumnWidth))
            End If
        Next
        lWriter.WriteLine()

        'Day number and all values for that day
        For lDay As Integer = 1 To lNumDays
            lWriter.Write(CStr(lDay).PadLeft(DayColumnWidth))
            For Each lTS In lGroup
                If lTS Is Nothing Then
                    lWriter.Write(Space(DataColumnWidth))
                Else
                    lWriter.Write(DecimalAlign(lTS.Value(lDay), DataColumnWidth, DataDecimalWidth))
                End If
            Next
            lWriter.WriteLine()
        Next

        lWriter.Close()
    End Function

    Public Function SaveArcSWAT(ByVal aProjectFolder As String) As Boolean
        Dim aSaveFileName As String = aProjectFolder
        Me.Specification = aSaveFileName

        Dim dctTS As Dictionary(Of String, atcTimeseries) = New Dictionary(Of String, atcTimeseries)
        Dim lGroup As New atcDataGroup
        Dim lTS As atcTimeseries = DataSets(0)
        Dim lStartDate As Double = lTS.Dates.Value(0)
        Dim lEndDate As Double = lTS.Dates.Value(lTS.numValues)
        Dim lNumDays As Integer = lTS.numValues
        Dim lColumnName As String
        For Each lColumnName In pColumns
            If DataSets.Keys.Contains(lColumnName) Then
                lTS = DataSets.ItemByKey(lColumnName)
            ElseIf lColumnName = "hmd" Then
                'Default relative humidity to 40%
                lTS = NewTimeseries(lStartDate, lEndDate, atcTimeUnit.TUDay, 1, Me, 40)
                lTS.Attributes.SetValueIfMissing("Units", "%")
            Else
                lTS = Nothing
            End If
            lGroup.Add(lColumnName, lTS)
            'WriteDatasetAttributes(lTS, aSaveFileName & "." & lColumnName & "-attributes.txt")
        Next

        Dim sLoc As String = ""
        Dim sLat As String = ""
        Dim sLon As String = ""
        Dim sStationName As String = ""

        Dim lName As String

        For Each lAttribute As atcDefinedValue In lTS.Attributes
            If Not lAttribute.Definition.Calculated Then
                lName = lAttribute.Definition.Name
                'sStation = lAttribute.Value.ToString()
                'Console.WriteLine("Name:" + lName + "-  Value: " + sStation)
                If (lName = "Location") Then
                    sLoc = lAttribute.Value
                End If
                If (lName = "Longitude") Then
                    sLon = lAttribute.Value
                End If
                If (lName = "Latitude") Then
                    sLat = lAttribute.Value
                End If
                If (lName = "STANAM") Then
                    sStationName = lAttribute.Value
                End If
            End If
        Next



        Dim dctWriters As Dictionary(Of String, atcTableDBF)
        dctWriters = CreateDbfDataWriters(lNumDays)
        Dim dctStationWriters As Dictionary(Of String, atcTableDBF)
        dctStationWriters = CreateDbfStationWriters()

        Dim dbfWriter As atcTableDBF
        Dim dbfStationWriter As atcTableDBF
        Dim sd As String = DumpDate(lStartDate)
        Dim startDate As Date = Convert.ToDateTime(sd)
        'Dim dt As DateTime = New DateTime("")
        Dim sDate As String = startDate.ToShortDateString()


        'Day number and all values for that day
        For lDay As Integer = 1 To lNumDays
            sDate = startDate.AddDays(lDay - 1).ToString("yyyyMMdd")

            For Each lColumnName In pColumns
                lTS = lGroup.ItemByKey(lColumnName)

                If Not (lTS Is Nothing) Then

                    If (lColumnName = "tmin") Then

                        dbfWriter = dctWriters("temp")

                        If (lDay > 1) Then
                            dbfWriter.MoveNext()
                        End If

                        dbfWriter.Value(1) = sDate
                        dbfWriter.Value(3) = lTS.Value(lDay)

                        dbfStationWriter = dctStationWriters("temp")

                    ElseIf (lColumnName = "tmax") Then
                        dbfWriter = dctWriters("temp")
                        dbfWriter.Value(2) = lTS.Value(lDay)


                    ElseIf (lColumnName = "tmean") Then
                        'skip
                    Else
                        dbfWriter = dctWriters(lColumnName)
                        If (lDay > 1) Then
                            dbfWriter.MoveNext()
                        End If

                        dbfWriter.Value(1) = sDate
                        dbfWriter.Value(2) = lTS.Value(lDay)

                    End If
                End If

            Next
        Next

        Dim sFileData As String = ""
        Dim sFileStation As String = ""
        For Each lColumnName In dctWriters.Keys

            sFileData = "Met" + lColumnName + "D"
            sFileStation = "Met" + lColumnName + "S"

            dbfWriter = dctWriters(lColumnName)
            dbfWriter.WriteFile(aProjectFolder + "\ArcSWAT\" + sFileData + ".dbf")


            dbfStationWriter = dctStationWriters(lColumnName)
            dbfStationWriter.Value(1) = "1"
            dbfStationWriter.Value(2) = sFileData
            dbfStationWriter.Value(3) = sLat
            dbfStationWriter.Value(4) = sLon
            dbfStationWriter.Value(5) = "0"
            dbfStationWriter.WriteFile(aProjectFolder + "\ArcSWAT\" + sFileStation + ".dbf")


        Next


        'lWriter.WriteLine()


        'lWriter.Close()
    End Function

    Public Sub New()
        Filter = pFilter
    End Sub


    Public Function CreateTimeSeriesDbfFile(ByVal numRecords As Integer, ByVal columnName As String) As atcTableDBF


        Dim newDBF As New atcTableDBF()


        newDBF.NumFields = 2

        newDBF.FieldName(1) = "DATE"
        newDBF.FieldType(1) = "D"
        newDBF.FieldLength(1) = 10
        newDBF.FieldDecimalCount(1) = 1

        newDBF.FieldName(2) = columnName
        newDBF.FieldType(2) = "N"
        newDBF.FieldLength(2) = 5
        newDBF.FieldDecimalCount(2) = 1

        newDBF.NumRecords = numRecords
        newDBF.InitData()


        newDBF.MoveFirst()


        Return newDBF

    End Function

    Public Function CreateTempTimeSeriesDbfFile(ByVal numRecords As Integer) As atcTableDBF


        Dim newDBF As New atcTableDBF()

        newDBF.NumFields = 3

        newDBF.FieldName(1) = "DATE"
        newDBF.FieldType(1) = "D"
        newDBF.FieldLength(1) = 10
        newDBF.FieldDecimalCount(1) = 0

        newDBF.FieldName(2) = "MAX"
        newDBF.FieldType(2) = "N"
        newDBF.FieldLength(2) = 5
        newDBF.FieldDecimalCount(2) = 1

        newDBF.FieldName(3) = "MIN"
        newDBF.FieldType(3) = "N"
        newDBF.FieldLength(3) = 5
        newDBF.FieldDecimalCount(3) = 1

        newDBF.NumRecords = numRecords
        newDBF.InitData()

        newDBF.MoveFirst()

        Return newDBF

    End Function

    Private Function CreateDbfDataWriters(ByVal numRecords As Integer) As Dictionary(Of String, atcTableDBF)
        Dim lColumnName As String

        'Dim dbfWriters As Generic.List(Of atcTableDBF) = New Generic.List(Of atcTableDBF)
        Dim dctWriters As Dictionary(Of String, atcTableDBF) = New Dictionary(Of String, atcTableDBF)

        Dim newDBF As atcTableDBF
        For Each lColumnName In pColumns
            If Not ((lColumnName = "tmin") Or (lColumnName = "tmean") Or (lColumnName = "tmax")) Then
                newDBF = CreateTimeSeriesDbfFile(numRecords, lColumnName)
                'dbfWriters.Add(newDBF)
                dctWriters.Add(lColumnName, newDBF)
            End If
        Next

        newDBF = CreateTempTimeSeriesDbfFile(numRecords)
        dctWriters.Add("temp", newDBF)
        'dbfWriters.Add(newDBF)

        Return dctWriters


    End Function


    Private Function CreateDbfStationWriters() As Dictionary(Of String, atcTableDBF)
        Dim lColumnName As String

        'Dim dbfWriters As Generic.List(Of atcTableDBF) = New Generic.List(Of atcTableDBF)
        Dim dctWriters As Dictionary(Of String, atcTableDBF) = New Dictionary(Of String, atcTableDBF)

        Dim newDBF As atcTableDBF
        For Each lColumnName In pColumns
            If Not ((lColumnName = "tmin") Or (lColumnName = "tmean") Or (lColumnName = "tmax")) Then
                newDBF = CreateStationDbfFile()
                'dbfWriters.Add(newDBF)
                dctWriters.Add(lColumnName, newDBF)
            End If
        Next

        newDBF = CreateStationDbfFile()
        dctWriters.Add("temp", newDBF)
        'dbfWriters.Add(newDBF)

        Return dctWriters
    End Function
    Public Function CreateStationDbfFile() As atcTableDBF


        Dim newDBF As New atcTableDBF()

        newDBF.NumFields = 5
        newDBF.FieldName(1) = "ID"
        newDBF.FieldType(1) = "N"
        newDBF.FieldLength(1) = 4
        newDBF.FieldDecimalCount(1) = 0

        newDBF.FieldName(2) = "NAME"
        newDBF.FieldType(2) = "C"
        newDBF.FieldLength(2) = 8
        newDBF.FieldDecimalCount(2) = 0

        newDBF.FieldName(3) = "LAT"
        newDBF.FieldType(3) = "N"
        newDBF.FieldLength(3) = 9
        newDBF.FieldDecimalCount(3) = 4

        newDBF.FieldName(4) = "LONG"
        newDBF.FieldType(4) = "N"
        newDBF.FieldLength(4) = 9
        newDBF.FieldDecimalCount(4) = 4

        newDBF.FieldName(5) = "ELEVATION"
        newDBF.FieldType(5) = "N"
        newDBF.FieldLength(5) = 11
        newDBF.FieldDecimalCount(5) = 0
        newDBF.NumRecords = 1

        newDBF.InitData()

        newDBF.MoveFirst()


        Return newDBF

    End Function

    Private Sub BuildDbfWriters()

    End Sub
End Class
