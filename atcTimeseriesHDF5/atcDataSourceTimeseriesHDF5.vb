Imports atcUtility
Imports atcData
Imports MapWinUtility
Imports MapWinUtility.Strings
Imports System.DateTime
Imports HDF5DotNet
Imports System

Public Class atcDataSourceTimeseriesHSF5
    Inherits atcTimeseriesSource
    '##MODULE_REMARKS Copyright 2017 RESPEC - Royalty-free use permitted under open source license

    Private Shared pFilter As String = "HDF5 Files (*.h5)|*.h5"
    Private pColDefs As Hashtable

    Public ReadOnly Property AvailableAttributes() As Collection
        Get
            'needed to edit attributes? that can't be done for this type!
            Return New Collection 'empty!
        End Get
    End Property

    Public Overrides ReadOnly Property Description() As String
        Get
            Return "HDF5 Timeseries"
        End Get
    End Property

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Timeseries::HDF5"
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

    Public ReadOnly Property Label() As String
        Get
            Return "HDF5"
        End Get
    End Property

    ''' <summary>
    ''' timeseries class for HDF5 files produced by HSP2, read only for now
    ''' </summary>
    ''' <param name="aFileName">name of existing hdf5 file, if missing or blank - query user for name</param>
    ''' <param name="aAttributes">optional options</param>
    ''' <returns></returns>
    Public Overrides Function Open(ByVal aFileName As String, Optional ByVal aAttributes As atcData.atcDataAttributes = Nothing) As Boolean
        Dim lAttributes As atcData.atcDataAttributes = aAttributes
        If lAttributes Is Nothing Then 'no options provided, provide empty class to avoid problems
            lAttributes = New atcData.atcDataAttributes
        Else 'summarize options
            Debug.Print("OpenHDF5 File:'" & aFileName & "' Options:")
            For Each lAttribue As atcData.atcDefinedValue In lAttributes
                Debug.Print(lAttribue.Definition.Name & ":" & lAttribue.Value)
            Next
        End If

        If MyBase.Open(aFileName, lAttributes) Then
            H5.Open()
            Debug.Print("H5Version " & H5.Version.Major & ":" & H5.Version.Minor)

            Dim lFileId As H5FileId = H5F.open(Specification, H5F.OpenMode.ACC_RDONLY)
            Debug.Print("ID " & lFileId.Id & " for " & aFileName)
            Dim lGrpName As String
            Dim lGrpId As H5GroupId
            Dim lAtCnt As Integer

            If lAttributes.GetValue("ProcessInputTS", True) Then 'input timeseries
                lGrpName = "/TIMESERIES"
                lGrpId = H5G.open(lFileId, lGrpName)
                lAtCnt = H5A.getNumberOfAttributes(lGrpId)
                Debug.Print("Attirbute Count for Input Timeseries" & lAtCnt & " for " & lGrpName)
                Dim lCnt As Integer = H5G.getNumObjects(lGrpId)
                Debug.Print("Potential Input Timeseries Count " & lCnt)
                For lTsID As Integer = 0 To lCnt - 1
                    Dim lTsName As String = H5G.getObjectNameByIndex(lGrpId, lTsID)
                    If lTsName.StartsWith("TS") Then
                        'cant use dates from earlier thimeseries because each one is independent!
                        AddDataSet(BuildTimeSeries(lGrpId, lTsName, "OBSERVED"))
                    End If
                Next
                Debug.Print("Actual Input Timeseries Count " & DataSets.Count)
            Else
                Debug.Print("Input Timeseries Skipped " & DataSets.Count)
            End If

            If lAttributes.GetValue("ProcessOutputTS", True) Then 'output timeseries
                Dim lOutputConstituent As String = lAttributes.GetValue("OutputConstituent", "")
                Dim lOutputLocation As String = lAttributes.GetValue("OutputLocation", "")

                Dim lOutputSection As New atcAttributeDefinition
                lOutputSection.Name = "OutputSection"
                lOutputSection.DefaultValue = Nothing
                Dim lOutputColumn As New atcAttributeDefinition
                lOutputColumn.Name = "OutputColumn"
                lOutputColumn.DefaultValue = Nothing

                lGrpName = "/RESULTS"
                lGrpId = H5G.open(lFileId, lGrpName)
                lAtCnt = H5A.getNumberOfAttributes(lGrpId)
                Debug.Print("Attirbute Count for " & lGrpName & " is " & lAtCnt)
                Dim lOpCnt As Integer = H5G.getNumObjects(lGrpId)
                Debug.Print("Operation Count " & lOpCnt)
                For lOpInd As Integer = 0 To lOpCnt - 1
                    Dim lOpnName As String = H5G.getObjectNameByIndex(lGrpId, lOpInd)
                    Dim lOpnId As H5GroupId = H5G.open(lGrpId, lOpnName)
                    Dim lSectCnt As Integer = H5G.getNumObjects(lOpnId)
                    Debug.Print("Section Count for " & lOpnName & " is " & lSectCnt)
                    For lSecInd As Integer = 0 To lSectCnt - 1
                        Dim lSecName As String = H5G.getObjectNameByIndex(lOpnId, lSecInd)
                        Dim lSecId As H5GroupId = H5G.open(lOpnId, lSecName)
                        Dim lSecCnt As Integer = H5G.getNumObjects(lSecId)
                        Dim lConsId As H5DataSetId = H5D.open(lSecId, H5G.getObjectNameByIndex(lSecId, 2))
                        Dim lConsStorageSize As Integer = H5D.getStorageSize(lConsId)
                        Dim lConsSpaceId As H5DataSpaceId = H5D.getSpace(lConsId)
                        Dim lConsDims() As Long = H5S.getSimpleExtentDims(lConsSpaceId)
                        Dim lConsArraySize As Integer = lConsDims(0)
                        Dim lConsLength As Integer = lConsStorageSize / lConsArraySize
                        Debug.Print("  Section " & lSecName & "  Timseries Count " & lConsArraySize)
                        Dim lConsTypeId As H5DataTypeId = H5D.getType(lConsId)
                        Dim lCons(lConsStorageSize - 1) As Byte
                        H5D.read(Of Byte)(lConsId, lConsTypeId, New H5Array(Of Byte)(lCons))
                        Dim lConsNames(lConsArraySize - 1) As String
                        Dim lConsDateDatasetIndex As Integer = DataSets.Count
                        For lConInd As Integer = 0 To lConsArraySize - 1
                            lConsNames(lConInd) = System.Text.Encoding.ASCII.GetString(lCons, lConInd * lConsLength, lConsLength).Replace(vbNullChar, " ")
                            Dim lConsName As String = lConsNames(lConInd).Trim
                            'Debug.Print("    ConName " & lConsName)
                            If ((lOutputConstituent.Length = 0 Or lConsName = lOutputConstituent) AndAlso
                                (lOutputLocation.Length = 0 Or lOpnName = lOutputLocation)) Then
                                Dim lTimeSeries As New atcTimeseries(Me)
                                lTimeSeries.Attributes.Add("Constituent", lConsName)
                                lTimeSeries.Attributes.Add("Location", lOpnName)
                                lTimeSeries.Attributes.Add("Scenario", "Simulated")
                                If DataSets.Count <> lConsDateDatasetIndex Then 'use dates from first dataset in this group
                                    lTimeSeries.Dates = DataSets(lConsDateDatasetIndex).Dates 'reference, don't want copy/clone!
                                Else
                                    ReadDates(lTimeSeries, lSecId, "axis1")
                                End If
                                'too many datasets run out of memory, need to just build headers then read data as needed
                                With lTimeSeries
                                    .numValues = lTimeSeries.Dates.numValues
                                    .Attributes.Add(New atcDefinedValue(lOutputSection, lSecId))
                                    .Attributes.Add(New atcDefinedValue(lOutputColumn, lConInd))
                                    lTimeSeries.Dates.Attributes.Add("Shared", True)
                                    .ValuesNeedToBeRead = True
                                End With
                                AddDataSet(lTimeSeries)
                            End If
                        Next
                    Next
                Next
            Else
                Debug.Print("Output Timeseries Skipped " & DataSets.Count)
            End If
        End If
        Debug.Print("Dataset Count " & DataSets.Count)
        Return True
    End Function

    Public Sub New()
        Filter = pFilter
    End Sub

    Private Function BuildTimeSeries(aGrpID As H5LocId, aTsName As String, aScenario As String, Optional aDatesTS As atcTimeseries = Nothing) As atcTimeseries
        Dim lTsGrpId As H5GroupId = H5G.open(aGrpID, aTsName)

        Dim lTsAtCnt As Integer = H5A.getNumberOfAttributes(lTsGrpId)
        'Debug.Print("Attribute Count " & lTsAtCnt & " for " & aTsName)

        Dim lTimeSeries As New atcTimeseries(Me)

        For lAtIndex As Integer = 0 To lTsAtCnt - 1
            Dim lAt As H5AttributeId = H5A.openIndex(lTsGrpId, lAtIndex)
            Dim lAtInfo As H5AttributeInfo = H5A.getInfo(lAt)
            Dim lAtSize As Integer = lAtInfo.dataSize
            Dim lAtTypeId As H5DataTypeId = H5A.getType(lAt)
            Dim lAtType As H5T.H5Type = H5T.getClass(lAtTypeId)
            Dim lAtName As String = H5A.getName(lAt).ToUpper
            'Debug.Print("  Name,Size,Type " & lAtName & ":" & lAtSize & ":" & lAtType)
            Select Case lAtType
                Case H5T.H5TClass.STRING
                    Dim lAtValue(lAtSize - 1) As Byte
                    H5A.read(Of Byte)(lAt, lAtTypeId, New H5Array(Of Byte)(lAtValue))
                    Dim lAtString As String = System.Text.Encoding.ASCII.GetString(lAtValue)
                    If lAtString <> vbNullChar Then
                        Dim lValue As String = lAtString.Substring(0, lAtSize)
                        'Debug.Print("    Length,Value " & lAtValue.Length & ":'" & lValue & "'")
                        If lAtName = "SCENARIO" And lValue = "NA" Then
                            lValue = aScenario
                        End If
                        lTimeSeries.Attributes.Add(lAtName, lValue)
                    Else
                        Debug.Print("    Null Value - Skipped")
                    End If
                Case H5T.H5TClass.FLOAT
                    Dim lAtValue(0) As Double
                    H5A.read(Of Double)(lAt, lAtTypeId, New H5Array(Of Double)(lAtValue))
                    'Debug.Print("    Length,Value " & lAtValue.Length & ":'" & lAtValue(0).ToString & "'")
                    lTimeSeries.Attributes.Add(lAtName, lAtValue(0))
                Case H5T.H5TClass.INTEGER
                    Dim lAtvalue(0) As Integer
                    H5A.read(Of Integer)(lAt, lAtTypeId, New H5Array(Of Integer)(lAtvalue))
                    'Debug.Print("    Length,Value " & lAtvalue.Length & ":'" & lAtvalue(0).ToString & "'")
                    lTimeSeries.Attributes.Add(lAtName, lAtvalue(0))
                Case Else
                    'Debug.Print("    Unknown Type")
                    lTimeSeries.Attributes.Add(lAtName, "?")
            End Select
        Next
        lTimeSeries.Attributes.Add("Name", aTsName)

        If IsNothing(aDatesTS) Then
            ReadDates(lTimeSeries, lTsGrpId, "index")
        Else
            lTimeSeries.Dates = aDatesTS.Dates
        End If

        ReadDatesAndData(lTimeSeries, lTsGrpId, "index", "values")

        Return lTimeSeries
    End Function

    Private Sub ReadDates(aTimeseries As atcTimeseries, aTsGrpId As H5GroupId, aDateTableName As String)
        Dim lNumValues As Integer

        If IsNothing(aTimeseries.Dates) Then 'need dates
            Dim lDateGrpId As H5DataSetId = H5D.open(aTsGrpId, aDateTableName)
            Dim lDateStorageSize As Integer = H5D.getStorageSize(lDateGrpId)
            lNumValues = lDateStorageSize / 8
            Dim lDates(lNumValues - 1) As Long
            Dim lDateTypeId As H5DataTypeId = H5D.getType(lDateGrpId)
            H5D.read(Of Int64)(lDateGrpId, lDateTypeId, New H5Array(Of Long)(lDates))
            'Debug.Print(lDateStorageSize.ToString & ":" & lNumValues.ToString & ":" & lDates(0).ToString)
            Dim lDateBase As New System.DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local)
            Dim lDateNow As System.DateTime = lDateBase.AddMilliseconds(lDates(0) / (10 ^ 6))
            Dim lDatesJ(lNumValues) As Double
            For lIndex As Integer = 0 To lNumValues - 1
                lDateNow = lDateBase.AddMilliseconds(lDates(lIndex) / (10 ^ 6))
                With lDateNow
                    lDatesJ(lIndex) = Date2J(.Year, .Month, .Day, .Hour, .Minute, .Second)
                End With
                If lIndex < 3 Or lIndex > lNumValues - 2 Then
                    'Debug.Print(lDateNow.ToString & ":" & DumpDate(lDatesJ(lIndex)))
                End If
            Next
            lDatesJ(lNumValues) = (2 * (lDatesJ(lNumValues - 1))) - lDatesJ(lNumValues - 2)
            'Debug.Print(DumpDate(lDatesJ(lNumValues)))
            aTimeseries.Dates = New atcTimeseries(Me)
            aTimeseries.Dates.Values = lDatesJ
        Else
            Debug.Print("ReadDates, no dates, why?")
        End If
    End Sub

    Public Overrides Sub ReadData(ByVal aData As atcDataSet)
        If aData.Attributes.GetValue("OutputSection") Is Nothing Then
            'data has been read
        Else
            'Debug.Print("Need to read data for " & aData.ToString)
            Dim lTimeseries As atcTimeseries = aData
            Dim lNumValues As Integer = lTimeseries.Dates.numValues
            Dim lColumn As Integer = lTimeseries.Attributes.GetValue("OutputColumn")
            Dim lGroupId As H5GroupId = lTimeseries.Attributes.GetValue("OutputSection")
            Dim lValueGrpId As H5DataSetId = H5D.open(lGroupId, "block0_values")

            Dim lValuesArraySize As Integer = H5D.getStorageSize(lValueGrpId) / 4
            Dim lValueArraySize As Integer = lTimeseries.Dates.numValues
            Dim lValueColumns As Integer = lValuesArraySize / lValueArraySize
            Dim lValues(lValuesArraySize - 1) As Single
            Dim lValueTypeId As H5DataTypeId = H5D.getType(lValueGrpId)
            'read all data (may be multiple ts)
            H5D.read(Of Single)(lValueGrpId, lValueTypeId, New H5Array(Of Single)(lValues))
            'Debug.Print(lValuesArraySize.ToString & ":" & lValueArraySize.ToString & ":" & lValues(0).ToString & ":" & lValues(1).ToString)
            ReDim lTimeseries.Values(lNumValues)
            For lIndex As Integer = 1 To lNumValues
                Dim lArrayIndex As Integer = (lValueColumns * (lIndex - 1)) + lColumn
                lTimeseries.Values(lIndex) = lValues(lArrayIndex)
            Next lIndex

            lTimeseries.SetInterval()
            lTimeseries.Attributes.CalculateAll()
        End If
    End Sub

    Private Sub ReadDatesAndData(aTimeseries As atcTimeseries, aTsGrpId As H5GroupId, aDateTableName As String, aDataTableName As String, Optional aColumn As Integer = -1)
        Dim lNumValues As Integer
        Dim lColumn As Integer = aColumn
        If lColumn < 0 Then lColumn = 0

        If IsNothing(aTimeseries.Dates) Then 'need dates
            Dim lDateGrpId As H5DataSetId = H5D.open(aTsGrpId, aDateTableName)
            Dim lDateStorageSize As Integer = H5D.getStorageSize(lDateGrpId)
            lNumValues = lDateStorageSize / 8
            Dim lDates(lNumValues - 1) As Long
            Dim lDateTypeId As H5DataTypeId = H5D.getType(lDateGrpId)
            H5D.read(Of Int64)(lDateGrpId, lDateTypeId, New H5Array(Of Long)(lDates))
            'Debug.Print(lDateStorageSize.ToString & ":" & lNumValues.ToString & ":" & lDates(0).ToString)
            Dim lDateBase As New System.DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local)
            Dim lDateNow As System.DateTime = lDateBase.AddMilliseconds(lDates(0) / (10 ^ 6))
            Dim lDatesJ(lNumValues) As Double
            For lIndex As Integer = 0 To lNumValues - 1
                lDateNow = lDateBase.AddMilliseconds(lDates(lIndex) / (10 ^ 6))
                With lDateNow
                    lDatesJ(lIndex) = Date2J(.Year, .Month, .Day, .Hour, .Minute, .Second)
                End With
                If lIndex < 3 Or lIndex > lNumValues - 2 Then
                    'Debug.Print(lDateNow.ToString & ":" & DumpDate(lDatesJ(lIndex)))
                End If
            Next
            lDatesJ(lNumValues) = (2 * (lDatesJ(lNumValues - 1))) - lDatesJ(lNumValues - 2)
            'Debug.Print(DumpDate(lDatesJ(lNumValues)))
            aTimeseries.Dates = New atcTimeseries(Me)
            aTimeseries.Dates.Values = lDatesJ
        Else 'using dates from first column
            lNumValues = aTimeseries.Dates.numValues
        End If

        Dim lValueGrpId As H5DataSetId = H5D.open(aTsGrpId, aDataTableName)
        Dim lValuesArraySize As Integer = H5D.getStorageSize(lValueGrpId) / 4
        Dim lValueArraySize As Integer = aTimeseries.Dates.numValues
        Dim lValueColumns As Integer = lValuesArraySize / lValueArraySize
        Dim lValues(lValuesArraySize - 1) As Single
        Dim lValueTypeId As H5DataTypeId = H5D.getType(lValueGrpId)
        'read all data (may be multiple ts)
        H5D.read(Of Single)(lValueGrpId, lValueTypeId, New H5Array(Of Single)(lValues))
        'Debug.Print(lValuesArraySize.ToString & ":" & lValueArraySize.ToString & ":" & lValues(0).ToString & ":" & lValues(1).ToString)
        ReDim aTimeseries.Values(lNumValues)
        For lIndex As Integer = 1 To lNumValues
            Dim lArrayIndex As Integer = (lValueColumns * (lIndex - 1)) + lColumn
            aTimeseries.Values(lIndex) = lValues(lArrayIndex)
        Next lIndex

        aTimeseries.SetInterval()
        aTimeseries.Attributes.CalculateAll()

    End Sub
End Class
