Imports atcUtility
Imports atcData
Imports MapWinUtility

Public Class atcDataSourceTimeseriesSWMM5Output
    Inherits atcTimeseriesSource
    '##MODULE_REMARKS Copyright 2010 AQUA TERRA Consultants - Royalty-free use permitted under open source license

    Private Shared pFilter As String = "SWMM5 Output Files (*.out)|*.out"
    Private pColDefs As Hashtable

    Public Overrides ReadOnly Property Description() As String
        Get
            Return "Timeseries SWMM5 Output"
        End Get
    End Property

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Timeseries::SWMM5 Output"
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
            OpenSwmmOutFile(MyBase.Specification)
            Dim lData As atcTimeseries = Nothing
            'TODO: fill in parsing of output file here!
            DataSets.Add(lData)
        End If
    End Function

    Public Sub New()
        Filter = pFilter
    End Sub
End Class
