Option Strict Off
Option Explicit On

Imports atcData
Imports atcUtility
Imports MapWinUtility
Imports Microsoft.VisualBasic
Imports System.Collections
Imports System.IO

Partial Public Class atcTimeseriesNCDC
    Inherits atcTimeseriesSource
    '##MODULE_REMARKS Copyright 2008 AQUA TERRA Consultants - Royalty-free use permitted under open source license

    Private Shared pFilter As String = "NCDC New Data Files (*.csv)|*.csv"
    Private Shared pNaN As Double = GetNaN()
    Private pBaseDataField As Integer
    Private pSubIdField As Integer
    Private pMONcontains As Integer = 0 'IPRINT from file.cio, 0=Monthly, 1=Daily, 2=Yearly
    Private pTimeUnit As atcTimeUnit = atcTimeUnit.TUMonth
    Private pYearBase As Integer = 1900
    Private pNumValues As Integer = 0
    Private pSaveSubwatershedId As Boolean = False
    Private pHeaderSize As Integer = 0
    Private pRecordLength As Integer = 0
    Private pDates As atcTimeseries = Nothing 'Can share dates since they will be the same for all ts in a file
    Private pType As String = ""
    Private Shared pDatasetID As Integer
    Private Shared pDefaultHeader As String = "awsId,wbanId,gmtDate,gmtTime,elemId,elemfld1,elemfld2,elemfld3,elemfld4,elemfld5,elemfld6,elemfld7,elemfld8,elemfld9,elemfld10,elemfld11,elemfld12,elemfld13,dataSrcFlag,rptType"

    Public Enum Cols
        awsId = 1
        wbanId = 2
        gmtDate = 3
        gmtTime = 4
        elemId = 5
        elemfld1 = 6 'value field for WND, TMP, and GF1
        elemfld2 = 7 'value field for AA1 and CIG
        elemfld3 = 8
        elemfld4 = 9
        elemfld5 = 10
        elemfld6 = 11
        elemfld7 = 12
        elemfld8 = 13
        elemfld9 = 14
        elemfld10 = 15
        elemfld11 = 16
        elemfld12 = 17
        elemfld13 = 18
        dataSrcFlag = 19
        rptType = 20
    End Enum

    Private Class RawData

        Public ColumnIndex As Integer
        Public ValidType As Boolean
        Private pRptType As String
        Public Property RptType() As String
            Get
                Return pRptType
            End Get
            Set(ByVal value As String)
                ValidType = True
                Select Case value
                    Case "FM-12"
                        PointType = False
                        TimeUnit = atcTimeUnit.TUHour
                    Case "FM-15"
                        PointType = False
                        TimeUnit = atcTimeUnit.TUHour
                    Case "SOD"
                        PointType = False
                        TimeUnit = atcTimeUnit.TUDay
                    Case "FM-16"
                        PointType = False
                        TimeUnit = atcTimeUnit.TUMinute
                        ValidType = False
                    Case Else
                        ValidType = False
                End Select
                If ValidType Then
                    pRptType = value
                Else
                    pRptType = "invalid"
                End If
            End Set
        End Property

        Public TimeUnit As atcTimeUnit
        Public PointType As Boolean
        Public DateBeg As Double

        Public Function DateEnd() As Double
            Return DateCalc(DateEndYMD, DateEndHMS)
        End Function

        Public DateEndYMD As String
        Public DateEndHMS As String

        Public ListTime As List(Of Double)
        Public ListValue As List(Of Double)

        Public Sub New()
            ListTime = New List(Of Double)
            ListValue = New List(Of Double)
        End Sub

        Public Sub ResetBeginDate()
            Select Case TimeUnit
                Case atcTimeUnit.TUHour
                    DateBeg -= JulianHour
                Case atcTimeUnit.TUDay
                    DateBeg -= JulianHour * 24
                Case atcTimeUnit.TUMinute
                    'not do anything as it is point type
                    DateBeg -= JulianMinute
            End Select
        End Sub

        Public Function DateCalc(ByVal agmtDate As String, ByVal agmtTime As String) As Double
            Dim lDate(5) As Integer

            lDate(0) = Integer.Parse(agmtDate.Substring(0, 4))
            lDate(1) = Integer.Parse(agmtDate.Substring(4, 2))
            lDate(2) = Integer.Parse(agmtDate.Substring(6, 2))

            lDate(3) = Integer.Parse(agmtTime.Substring(0, 2))
            lDate(4) = Integer.Parse(agmtTime.Substring(2, 2))
            lDate(5) = 0

            Return Date2J(lDate)
        End Function

        Public Sub Clear()
            ListTime.Clear() : ListTime = Nothing
            ListValue.Clear() : ListValue = Nothing
        End Sub
    End Class

    Public Overrides ReadOnly Property Description() As String
        Get
            Return "NCDC New Data Files"
        End Get
    End Property

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Timeseries::" & Description.Substring(0, 4)
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
            Return False 'can save met data when called by code, but does not yet support saving to user-selected file
        End Get
    End Property

    Public Overrides Function Open(ByVal aFileName As String, Optional ByVal aAttributes As atcData.atcDataAttributes = Nothing) As Boolean
        If MyBase.Open(aFileName, aAttributes) Then 'see if file exists

            Dim lRptTypeDataGroup As New atcCollection()

            Dim lStationIdPart1 As String = "unknown"
            Dim lStationIdPart2 As String = "unknown"
            Dim lConstituent As String = "unknown"
            Dim lConstituentBASINS As String = "unknown"

            Dim lRptType As String = ""
            Dim lDate(5) As Integer

            'scan through the file for timeseries
            Dim lTable As atcTable = OpenTable()
            With lTable
                lStationIdPart1 = .Value(Cols.awsId)
                lStationIdPart2 = .Value(Cols.wbanId)
                lConstituent = .Value(Cols.elemId)

                Dim lValueColumn As Integer = Cols.elemfld1

                Select Case lConstituent.ToUpper
                    Case "AA1" : lConstituentBASINS = "PREC" : lValueColumn = Cols.elemfld2
                    Case "TMP" : lConstituentBASINS = "ATEM"
                    Case "WND" : lConstituentBASINS = "WIND" : lValueColumn = Cols.elemfld4
                    Case "DEW" : lConstituentBASINS = "DEWP"
                    Case "GF1" : lConstituentBASINS = "SKYCOND"
                    Case Else : lConstituentBASINS = "UNK"
                End Select

                While Not .EOF
                    lRptType = .Value(Cols.rptType)
                    If Not lRptTypeDataGroup.Keys.Contains(lRptType) Then
                        If SkipRptType Is Nothing OrElse Not SkipRptType.Contains(lRptType) Then
                            Dim lNewData As New RawData()
                            lNewData.RptType = lRptType
                            If lNewData.ValidType Then
                                lNewData.DateBeg = lNewData.DateCalc(.Value(Cols.gmtDate), .Value(Cols.gmtTime))
                                lNewData.ColumnIndex = lValueColumn
                                lRptTypeDataGroup.Add(lRptType, lNewData)
                            End If
                        End If
                    Else
                        'Set end date
                        With CType(lRptTypeDataGroup.ItemByKey(lRptType), RawData)
                            .DateEndYMD = lTable.Value(Cols.gmtDate)
                            .DateEndHMS = lTable.Value(Cols.gmtTime)
                        End With
                    End If
                    .MoveNext()
                End While
            End With 'lTable

            lTable.Clear()
            lTable = Nothing

            Dim lTs As atcTimeseries = Nothing
            Dim lDates As atcTimeseries = Nothing
            Dim lNewDates() As Double
            For Each lRptType In lRptTypeDataGroup.Keys
                lTs = New atcTimeseries(Me)
                lDates = New atcTimeseries(Nothing)
                Dim lRawData As RawData = lRptTypeDataGroup.ItemByKey(lRptType)
                With lRawData
                    lTs.ValuesNeedToBeRead = True
                    .ResetBeginDate()
                    lNewDates = NewDates(lRawData.DateBeg, lRawData.DateEnd, .TimeUnit, 1)
                    lDates.Values = lNewDates
                    lTs.Dates = lDates
                    'lTs.numValues = lDates.numValues 'cannot set this as is will think already has values
                    lTs.SetInterval(.TimeUnit, 1)
                    lTs.Attributes.SetValue("Point", .PointType)
                End With

                With lTs.Attributes
                    .SetValue("STAID1", lStationIdPart1)
                    .SetValue("STAID2", lStationIdPart2)
                    .SetValue("Scenario", "OBSERVED")
                    .SetValue("RptType", lRptType)
                    .SetValue("Location", lStationIdPart1 & lStationIdPart2)
                    .SetValue("History 1", FilenameNoPath(Specification))
                    .SetValue("Constituent", lConstituentBASINS)
                    .SetValue("FieldIndex", lRawData.ColumnIndex) 'currently only reading the actual value
                    '.SetValue("ID", pDatasetID)
                End With
                DataSets.Add(lTs)
            Next 'lRptType
            Return True
        Else
            Return False
        End If
    End Function

    'For reading of all values without regard for report type (last field)
    'Public Function Open0(ByVal aFileName As String, Optional ByVal aAttributes As atcData.atcDataAttributes = Nothing) As Boolean
    '    If MyBase.Open(aFileName, aAttributes) Then 'see if file exists

    '        'scan through the file AND populate data, assuming each downloaded data file is quite small
    '        'e.g. one month at a time
    '        Dim lTs As atcTimeseries = Nothing

    '        Dim lRptTypeDataGroup As New atcCollection()
    '        Dim lRptTypeValuesGroup As New atcCollection()

    '        Dim lStationIdPart1 As String = "unknown"
    '        Dim lStationIdPart2 As String = "unknown"
    '        Dim lConstituent As String = "unknown"
    '        Dim lRptType As String = ""
    '        Dim lDate(5) As Integer

    '        Dim lTable As New atcTableDelimited()
    '        With lTable

    '            .Delimiter = ","
    '            .NumFieldNameRows = 0
    '            .OpenFile(Specification)
    '            .CurrentRecord = 1
    '            If lStationIdPart1 = "unknown" Then
    '                lStationIdPart1 = .Value(Cols.awsId)
    '                lStationIdPart2 = .Value(Cols.wbanId)
    '                lConstituent = .Value(Cols.elemId)
    '            End If

    '            While Not .EOF
    '                lRptType = .Value(Cols.rptType)
    '                If Not lRptTypeDataGroup.Keys.Contains(lRptType) Then
    '                    Dim lNewData As New RawData()
    '                    With lNewData
    '                        .RptType = lRptType
    '                        .DateBeg = .DateCalc(lTable.Value(Cols.gmtDate), lTable.Value(Cols.gmtTime))
    '                        Dim lValue As Double = Double.Parse(lTable.Value(Cols.elemfld2))
    '                        .ListValue.Add(lValue)
    '                        .ListTime.Add(.DateBeg)
    '                    End With
    '                    lRptTypeDataGroup.Add(lRptType, lNewData)
    '                Else
    '                    'Set end date
    '                    With CType(lRptTypeDataGroup.ItemByKey(lRptType), RawData)
    '                        .DateEnd = .DateCalc(lTable.Value(Cols.gmtDate), lTable.Value(Cols.gmtTime))
    '                        .ListTime.Add(.DateEnd)
    '                        Dim lValue As Double = Double.Parse(lTable.Value(Cols.elemfld2))
    '                        .ListValue.Add(lValue)
    '                    End With
    '                End If
    '                .MoveNext()
    '            End While
    '        End With 'lTable

    '        lTable.Clear()
    '        lTable = Nothing

    '        Dim lDates As atcTimeseries = Nothing
    '        For Each lRptType In lRptTypeDataGroup.Keys
    '            'If lRptType = "FM-16" Then Continue For 'discard the point measurement readings
    '            pDatasetID += 1
    '            lTs = New atcTimeseries(Me)
    '            lTs.Attributes.SetValue("STAID1", lStationIdPart1)
    '            lTs.Attributes.SetValue("STAID2", lStationIdPart2)
    '            lTs.Attributes.SetValue("Scenario", "OBSERVED")
    '            lTs.Attributes.SetValue("Location", lStationIdPart1 & lStationIdPart2)
    '            lTs.Attributes.SetValue("History 1", FilenameNoPath(Specification))
    '            lTs.Attributes.SetValue("Constituent", lConstituent)
    '            lTs.Attributes.SetValue("ID", pDatasetID)

    '            Dim lRawData As RawData = lRptTypeDataGroup.ItemByKey(lRptType)
    '            With lRawData
    '                .ResetBeginDate()

    '                lDates = New atcTimeseries(Nothing)
    '                Dim lNewDates() As Double = NewDates(lRawData.DateBeg, lRawData.DateEnd, .TimeUnit, 1)
    '                lDates.Values = lNewDates
    '                lTs.Dates = lDates
    '                lTs.SetInterval(.TimeUnit, 1)
    '                lTs.numValues = lDates.numValues

    '                Dim LastIndex As Integer = 0
    '                Dim lMissingValue As Boolean
    '                Dim J As Integer
    '                For I As Integer = 1 To lTs.Dates.numValues
    '                    lMissingValue = True
    '                    For J = LastIndex To .ListTime.Count - 1
    '                        If DumpDate(.ListTime(J)) = DumpDate(lTs.Dates.Value(I)) Then
    '                            lTs.Value(I) = .ListValue(J)
    '                            LastIndex = J
    '                            lMissingValue = False
    '                            Exit For
    '                        End If
    '                    Next
    '                    If lMissingValue Then
    '                        lTs.Value(I) = -999
    '                        If J >= .ListTime.Count - 1 Then LastIndex = 0
    '                    End If

    '                Next
    '            End With

    '            DataSets.Add(lTs)
    '        Next
    '    End If
    'End Function

    Public Overrides Function Save(ByVal SaveFileName As String, _
                       Optional ByVal ExistAction As EnumExistAction = EnumExistAction.ExistReplace) As Boolean
        'TODO: when called from client code, the pType is unknow, need a way to set it
        Select Case pType
        End Select
    End Function

    Private Sub AddTsToList(ByVal aReadTS As atcTimeseries, _
                            ByVal aReadThese As Generic.List(Of atcTimeseries), _
                            ByVal aReadRptTypes As Generic.List(Of Integer), _
                            ByVal aUniqueRptTypes As Generic.List(Of String), _
                            ByVal aReadField As Generic.List(Of Integer), _
                            ByVal aReadValues As Generic.List(Of RawData))

        Dim lField As Integer = aReadTS.Attributes.GetValue("FieldIndex", 0)
        If lField < 1 Then
            Logger.Dbg("Dataset does not have a field index:" & aReadTS.ToString & vbCrLf & _
                       "Source:'" & Specification & "'")
        Else
            aReadThese.Add(aReadTS)
            Dim lRptType As String = aReadTS.Attributes.GetValue("RptType", "<unk>")
            Dim lRptTypeIndex As Integer = aUniqueRptTypes.IndexOf(lRptType)
            If lRptTypeIndex < 0 Then
                lRptTypeIndex = aUniqueRptTypes.Count
                aUniqueRptTypes.Add(lRptType)
            End If
            aReadRptTypes.Add(lRptTypeIndex)
            aReadField.Add(lField)
            'Dim lVd(pNumValues) As Double 'array of double data values
            'For lValueIndex As Integer = 0 To pNumValues
            '    lVd(lValueIndex) = pNaN
            'Next

            Dim lVd As New RawData
            lVd.RptType = lRptType
            aReadValues.Add(lVd)
        End If
    End Sub

    Private Function OpenTable() As atcTable
        Dim lTable As New atcTableDelimited()
        With lTable
            .Delimiter = ","
            .NumFieldNameRows = 0
            If .OpenFile(Specification) Then
                .CurrentRecord = 1
                Attributes.SetValue("HeaderText", pDefaultHeader)
            Else
                lTable = Nothing
            End If
        End With

        Return lTable
    End Function

    Public Overrides Sub ReadData(ByVal aReadMe As atcDataSet)
        If Not DataSets.Contains(aReadMe) Then
            Logger.Dbg("Dataset not from this source:" & aReadMe.ToString & vbCrLf & _
                       "Source:'" & Specification & "'")
        Else
            Dim lReadTS As atcTimeseries
            Dim lReadThese As New Generic.List(Of atcTimeseries)
            Dim lUniqueRptTypes As New Generic.List(Of String)
            Dim lReadRptTypes As New Generic.List(Of Integer)
            Dim lReadField As New Generic.List(Of Integer)
            'Dim lReadValues As New Generic.List(Of Double()) 'array of double data values for each timeseries
            Dim lReadValues As New List(Of RawData)

            Dim lMissingValue As Double = -999

            If Me.DataSets.Count < 30 Then
                'Reading all datasets at once is much faster than one at a time
                For Each lReadTS In Me.DataSets
                    If lReadTS.Serial = aReadMe.Serial OrElse lReadTS.ValuesNeedToBeRead Then
                        AddTsToList(lReadTS, lReadThese, lReadRptTypes, lUniqueRptTypes, lReadField, lReadValues)
                    End If
                Next
            Else
                AddTsToList(aReadMe, lReadThese, lReadRptTypes, lUniqueRptTypes, lReadField, lReadValues)
            End If

            Dim lFinalReadingIndex As Integer = lReadThese.Count - 1
            If lFinalReadingIndex < 0 Then
                Logger.Dbg("No datasets to read")
            Else
                Dim lTable As atcTable = OpenTable()
                If lTable Is Nothing Then
                    Logger.Dbg("Unable to open " & Specification)
                Else
                    Dim lVd As RawData = Nothing
                    Dim lValue As Double
                    With lTable
                        Dim lFieldIndex As Integer
                        While Not .EOF
                            Try
                                ' lTable 
                                For lDSctr As Integer = 0 To lReadValues.Count - 1
                                    lFieldIndex = Integer.Parse(DataSets(lDSctr).Attributes.GetValue("FieldIndex"))
                                    lVd = lReadValues(lDSctr)
                                    If lVd.RptType = .Value(Cols.rptType) Then
                                        If Not Double.TryParse(.Value(lFieldIndex).Trim, lValue) Then
                                            lVd.ListValue.Add(pNaN)
                                        Else
                                            lVd.ListValue.Add(lValue)
                                        End If
                                        lVd.ListTime.Add(lVd.DateCalc(.Value(Cols.gmtDate), .Value(Cols.gmtTime)))
                                        lReadValues(lDSctr) = lVd
                                        Exit For
                                    End If
                                Next
                            Catch ex As FormatException
                                Logger.Dbg("FormatException " & .CurrentRecord & ":" & lFieldIndex & ":" & .Value(lFieldIndex))
                            Catch ex As Exception
                                Logger.Dbg("Stopping reading NCDC data at record " & .CurrentRecord & ": " & ex.Message)
                            End Try
                            .CurrentRecord += 1
                        End While
                        'Logger.Dbg("Read " & Format(lNumTS, "#,##0") & " timeseries From " & Format(.CurrentRecord, "#,##0") & " Records")
                    End With 'lTable

                    For lDSctr As Integer = 0 To DataSets.Count - 1
                        lReadTS = lReadThese(lDSctr)
                        lReadTS.numValues = lReadTS.Dates.numValues

                        lVd = lReadValues(lDSctr)

                        lReadTS.ValuesNeedToBeRead = False

                        If lVd.ListValue.Count - 1 = lReadTS.Dates.numValues Then
                            lReadTS.Values = lVd.ListValue.ToArray()
                        Else
                            Dim LastIndex As Integer = 0
                            Dim lFoundMatchingDate As Boolean
                            Dim J As Integer
                            Dim lDateTs(5) As Integer
                            Dim lDateList(5) As Integer
                            For I As Integer = 1 To lReadTS.Dates.numValues
                                lFoundMatchingDate = False

                                For J = LastIndex To lVd.ListTime.Count - 1
                                    If Math.Abs(lReadTS.Dates.Value(I) - lVd.ListTime(J)) < atcUtility.JulianSecond Then ' DumpDate(lReadTS.Dates.Value(I)) = DumpDate(lVd.ListTime(J)) Then
                                        lFoundMatchingDate = True
                                    Else
                                        J2Date(lReadTS.Dates.Value(I), lDateTs)
                                        J2Date(lVd.ListTime(J), lDateList)
                                        If lDateTs(0) = lDateList(0) AndAlso _
                                           lDateTs(1) = lDateList(1) AndAlso _
                                           lDateTs(2) = lDateList(2) AndAlso _
                                           lDateTs(3) = lDateList(3) Then
                                            If lVd.TimeUnit = atcTimeUnit.TUMinute Then
                                                If lDateTs(4) = lDateList(4) Then
                                                    lFoundMatchingDate = True
                                                End If
                                            Else
                                                lFoundMatchingDate = True
                                            End If
                                        End If
                                    End If

                                    If lFoundMatchingDate Then
                                        lReadTS.Value(I) = lVd.ListValue(J)
                                        LastIndex = J
                                        Exit For
                                    ElseIf lVd.ListTime(J) > lReadTS.Dates.Value(I) Then
                                        LastIndex = J - 1
                                        Exit For
                                    End If
                                Next 'date in the list of raw dates

                                If Not lFoundMatchingDate Then
                                    lReadTS.Value(I) = lMissingValue
                                    If J >= lVd.ListTime.Count - 1 Then LastIndex = 0
                                End If
                            Next 'date in the timeseries
                        End If

                        With lReadTS.Attributes
                            .SetValue("point", False)
                            .SetValue("TSFILL", pNaN)
                            .SetValue("MVal", pNaN)
                            .SetValue("MAcc", pNaN)
                        End With
                        Logger.Progress(lDSctr, lFinalReadingIndex)
                    Next
                    'pNumValues = lValueIndex
                    Logger.Progress("", 0, 0)
                End If
            End If

            'Clean up memory some
            lReadThese.Clear()
            lReadRptTypes.Clear()
            lReadField.Clear()
            For Each lRawdata In lReadValues
                lRawdata.Clear()
                lRawdata = Nothing
            Next
            lReadValues.Clear()
        End If
    End Sub

    Public SkipRptType As Generic.List(Of String) = Nothing

    Public Sub New()
        Filter = pFilter
    End Sub
End Class