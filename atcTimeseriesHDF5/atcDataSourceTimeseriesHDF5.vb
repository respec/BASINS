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
            Debug.Print("Attribute Count " & lAtCnt & " for " & lGrpName)
            Dim lCnt As Integer = H5G.getNumObjects(lGrpId)
            Debug.Print("Potential Timeseries Count " & lCnt)

            Dim lTsNames As New Collection
            For lTsID As Integer = 0 To lCnt - 1
                Dim lTsName As String = H5G.getObjectNameByIndex(lGrpId, lTsID)
                If lTsName.StartsWith("TS") Then
                    lTsNames.Add(lTsName)
                    Dim lTsGrpId As H5GroupId = H5G.open(lGrpId, lTsName)
                    Dim lTsAtCnt As Integer = H5A.getNumberOfAttributes(lTsGrpId)
                    Debug.Print("Attribute Count " & lTsAtCnt & " for " & lTsName)
                    lCnt = H5G.getNumObjects(lTsGrpId)
                    Dim lDateGrpId As H5DataSetId = H5D.open(lTsGrpId, "index")
                    Debug.Print(lDateGrpId.Id)

                    Dim lData As New atcTimeseries(Me)
                    lData.Attributes.Add("Name", lTsName)
                    Me.DataSets.Add(lData)
                End If
            Next
            Debug.Print("Actual Input Timeseries Count " & lTsNames.Count)

            'output timeseries
            lGrpName = "/RESULTS"
        End If

    End Function

    Public Sub New()
        Filter = pFilter
    End Sub
End Class
