Imports atcUtility
Imports atcData
Imports MapWinUtility
Imports MapWinUtility.Strings
Imports HDF5DotNet

Public Class atcDataSourceTimeseriesHSF5
    Inherits atcTimeseriesSource
    '##MODULE_REMARKS Copyright 2017 RESPEC - Royalty-free use permitted under open source license

    Private Shared pFilter As String = "HDF5 Files (*.h5)|*.h5"
    Private pColDefs As Hashtable

    Public Overrides ReadOnly Property Description() As String
        Get
            Return "Timeseries HDF5"
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

    Public Overrides Function Open(ByVal aFileName As String, Optional ByVal aAttributes As atcData.atcDataAttributes = Nothing) As Boolean
        If MyBase.Open(aFileName, aAttributes) Then
            H5.Open()
            Debug.Print("H5Version " & H5.Version.Major & ":" & H5.Version.Minor)

            Dim lFileId As H5FileId = H5F.open(aFileName, H5F.OpenMode.ACC_RDONLY)
            Debug.Print("ID " & lFileId.Id & " for " & aFileName)

            'input timeseries
            Dim lGrpName As String = "/TIMESERIES"
            Dim lGrpId As H5GroupId = H5G.open(lFileId, lGrpName)
            Dim lAtCnt As Integer = H5A.getNumberOfAttributes(lGrpId)
            Debug.Print("Attirbute Count for Input Timeseries" & lAtCnt & " for " & lGrpName)
            Dim lCnt As Integer = H5G.getNumObjects(lGrpId)
            Debug.Print("Potential Input Timeseries Count " & lCnt)

            For lTsID As Integer = 0 To lCnt - 1
                Dim lTsName As String = H5G.getObjectNameByIndex(lGrpId, lTsID)
                If lTsName.StartsWith("TS") Then
                    Me.AddDataSet(BuildTimeSeries(lGrpId, lTsName))
                End If
            Next
            Debug.Print("Actual Input Timeseries Count " & Me.DataSets.Count)

            'output timeseries
            lGrpName = "/RESULTS"
            lGrpId = H5G.open(lFileId, lGrpName)
            lAtCnt = H5A.getNumberOfAttributes(lGrpId)
            Debug.Print("Attirbute Count for Output Timeseries " & lAtCnt & " for " & lGrpName)
            lCnt = H5G.getNumObjects(lGrpId)
            Debug.Print("Operation Count " & lCnt)
            For lTsID As Integer = 0 To lCnt - 1
                Dim lOperationName As String = H5G.getObjectNameByIndex(lGrpId, lTsID)
                Debug.Print("  " & lOperationName)
            Next

        End If

    End Function

    Public Sub New()
        Filter = pFilter
    End Sub

    Private Function BuildTimeSeries(aGrpID As H5LocId, aTsNAme As String) As atcTimeseries
        Dim lTsGrpId As H5GroupId = H5G.open(aGrpID, aTsNAme)
        Dim lTimeSeries As New atcTimeseries(Me)

        Dim lTsAtCnt As Integer = H5A.getNumberOfAttributes(lTsGrpId)
        Debug.Print("Attribute Count " & lTsAtCnt & " for " & aTsNAme)

        For lAtIndex As Integer = 0 To lTsAtCnt - 1
            Dim lAt As H5AttributeId = H5A.openIndex(lTsGrpId, lAtIndex)
            Dim lAtInfo As H5AttributeInfo = H5A.getInfo(lAt)
            Dim lAtSize As Integer = lAtInfo.dataSize
            Dim lAtTypeId As H5DataTypeId = H5A.getType(lAt)
            Dim lAtType As H5T.H5Type = H5T.getClass(lAtTypeId)
            Dim lAtName As String = H5A.getName(lAt).ToUpper
            Debug.Print("  Name,Size,Type " & lAtNAme & ":" & lAtSize & ":" & lAtType)
            Select Case lAtType
                Case H5T.H5TClass.STRING
                    Dim lAtValue(lAtSize - 1) As Byte
                    H5A.read(Of Byte)(lAt, lAtTypeId, New H5Array(Of Byte)(lAtValue))
                    Dim lAtString As String = System.Text.Encoding.ASCII.GetString(lAtValue)
                    If lAtString <> vbNullChar Then
                        Debug.Print("    Length,Value " & lAtValue.Length & ":'" & lAtString.Substring(0, lAtSize) & "'")
                        lTimeSeries.Attributes.Add(lAtName, lAtString.Substring(0, lAtSize))
                    Else
                        Debug.Print("    Null Value - Skipped")
                    End If
                Case H5T.H5TClass.FLOAT
                    Dim lAtValue(0) As Double
                    H5A.read(Of Double)(lAt, lAtTypeId, New H5Array(Of Double)(lAtValue))
                    Debug.Print("    Length,Value " & lAtValue.Length & ":'" & lAtValue(0).ToString & "'")
                    lTimeSeries.Attributes.Add(lAtName, lAtValue(0))
                Case H5T.H5TClass.INTEGER
                    Dim lAtvalue(0) As Integer
                    H5A.read(Of Integer)(lAt, lAtTypeId, New H5Array(Of Integer)(lAtvalue))
                    Debug.Print("    Length,Value " & lAtvalue.Length & ":'" & lAtvalue(0).ToString & "'")
                    lTimeSeries.Attributes.Add(lAtName, lAtvalue(0))
                Case Else
                    Debug.Print("    Unknown Type")
                    lTimeSeries.Attributes.Add(lAtName, "?")
            End Select
        Next
        lTimeSeries.Attributes.Add("Name", aTsNAme)

        Dim lCnt As Integer = H5G.getNumObjects(lTsGrpId)

        Dim lDateGrpId As H5DataSetId = H5D.open(lTsGrpId, "index")
        Dim lDateStorageSize As Integer = H5D.getStorageSize(lDateGrpId)
        Dim lDateArraySize As Integer = lDateStorageSize / 4
        Dim lDates(lDateArraySize - 1) As Long
        Dim lDateTypeId As H5DataTypeId = H5D.getType(lDateGrpId)
        H5D.read(Of Int64)(lDateGrpId, lDateTypeId, New H5Array(Of Long)(lDates))
        Debug.Print(lDateStorageSize.ToString & ":" & lDateArraySize.ToString & ":" & lDates(0).ToString)

        Dim lValueGrpId As H5DataSetId = H5D.open(lTsGrpId, "values")
        Dim lValueSpace As H5DataSpaceId = H5D.getSpace(lValueGrpId)
        Debug.Print(lValueGrpId.Id.ToString & ":" & lValueSpace.Id.ToString)
        'Dim lAt As H5AttributeId = H5D.open(lDateGrpId, lAtIndex)

        Return lTimeSeries

    End Function
End Class
