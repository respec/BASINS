Imports atcUtility
Imports atcData
Imports MapWinUtility

Public Class atcDataSourceTimeseriesSWMM5Output
    Inherits atcTimeseriesSource
    '##MODULE_REMARKS Copyright 2010 AQUA TERRA Consultants - Royalty-free use permitted under open source license

    Private Shared pFilter As String = "SWMM5 Output Files (*.out)|*.out"
    Private pSWMM5_OutputFile As SWMM5_OutputFile

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
            pSWMM5_OutputFile = New SWMM5_OutputFile
            If pSWMM5_OutputFile.OpenSwmmOutFile(MyBase.Specification) = 0 Then
                Dim lLink As Integer = 2 'TODO: how to get this constants -> pSWMM5_OutputFile.LINK
                Dim lNumValues As Int16 = pSWMM5_OutputFile.TimeStarts.GetUpperBound(0)
                Dim lValue As Single

                'TODO: build timeseries for each possible output timeseries
                'TODO: set attributes appropriately
                'TODO: should these be filled yet or as needed?

                'this is a sample filled timeseries
                Dim lData As New atcTimeseries(Me)
                lData.numValues = lNumValues
                lData.Dates = New atcTimeseries(Me)
                With lData.Dates
                    .numValues = lNumValues
                    Dim lIndex As Integer = 0
                    For Each lDateValue As Double In pSWMM5_OutputFile.TimeStarts
                        'SWMM Julian date conventions match ours!!
                        .Value(lIndex) = lDateValue
                        pSWMM5_OutputFile.GetSwmmResult(lLink, 1, 1, lIndex, lValue)
                        lData.Values(lIndex) = lValue
                        lIndex += 1
                    Next
                End With
                'TODO: fill in parsing of output file here!
                DataSets.Add(lData)
                Return True
            Else
                Return False
            End If
        Else
            Return False
        End If
    End Function

    Public Sub New()
        Filter = pFilter
    End Sub
End Class
