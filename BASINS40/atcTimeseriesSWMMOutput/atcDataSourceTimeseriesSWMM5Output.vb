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
            Try
                pSWMM5_OutputFile = New SWMM5_OutputFile(Specification)

                Dim lScenario As String = IO.Path.GetFileNameWithoutExtension(MyBase.Specification)

                'TODO: set attributes appropriately
                '      need to set the units for each parameter of each object

                Dim lSubParmNamesBasic() As String = {"Precipitation", "Snow Depth", "Losses", "Runoff", "GW Flow", "GW Elev."}
                Dim lLinkParmNamesBasic() As String = {"Flow", "Depth", "Velocity", "Froude#", "Capacity"}
                Dim lNodeParmNamesBasic() As String = {"Depth", "Head", "Volume", "Lateral Inflow", "Total Inflow", "Flooding"}
                Dim lSysParmNames() As String = {"Temperature", "Precipitation", "Snow Depth", "Losses", "Runoff", "DW Inflow", "GW Inflow", "I&I Inflow", "Direct Inflow", "Total Inflow", "Flooding", "Outflow", "Storage", "Evaporation"}

                Dim lSubParmUnitsUS() As String = {"in/hr", "in", "in/hr", "cfs", "cfs", "ft"} 'pollutants' units are from .inp
                Dim lLinkParmUnitsUS() As String = {"cfs", "ft", "fps", "unitless", "unitless"} 'pollutants' units are from .inp
                Dim lNodeParmUnitsUS() As String = {"ft", "ft", "ft3", "cfs", "cfs", "cfs"} 'pollutants' units are from .inp
                Dim lSysParmUnitsUS() As String = {"deg F", "in/hr", "in", "in/hr", "cfs", "cfs", "cfs", "cfs", "cfs", "cfs", "cfs", "cfs", "?cfs?", "in/day"}

                Dim lSubParmNames As New ArrayList(lSubParmNamesBasic)
                Dim lLinkParmNames As New ArrayList(lLinkParmNamesBasic)
                Dim lNodeParmNames As New ArrayList(lNodeParmNamesBasic)

                Dim lSubParmUnits As New ArrayList(lSubParmUnitsUS)
                Dim lLinkParmUnits As New ArrayList(lLinkParmUnitsUS)
                Dim lNodeParmUnits As New ArrayList(lNodeParmUnitsUS)
                Dim lSysParmUnits As New ArrayList(lSysParmUnitsUS)

                If pSWMM5_OutputFile.SWMM_Npolluts > 0 Then
                    For i As Integer = 1 To pSWMM5_OutputFile.SWMM_Npolluts
                        lSubParmNames.Add(pSWMM5_OutputFile.SWMM_PollutId(i - 1))
                        lLinkParmNames.Add(pSWMM5_OutputFile.SWMM_PollutId(i - 1))
                        lNodeParmNames.Add(pSWMM5_OutputFile.SWMM_PollutId(i - 1))

                        lSubParmUnits.Add("FromInput")
                        lLinkParmUnits.Add("FromInput")
                        lNodeParmUnits.Add("FromInput")
                    Next
                End If

                Dim lNumValues As Int32 = pSWMM5_OutputFile.TimeStarts.GetUpperBound(0)

                'Set up common date array
                Dim lDates As atcTimeseries = New atcTimeseries(Me)
                With lDates
                    .numValues = lNumValues
                    Dim lTimeIndex As Integer = 0
                    For Each lDateValue As Double In pSWMM5_OutputFile.TimeStarts
                        'SWMM Julian date conventions match ours!!
                        .Value(lTimeIndex) = lDateValue
                        lTimeIndex += 1
                    Next
                End With

                For lLocationIndex As Integer = 0 To pSWMM5_OutputFile.SWMM_Nsubcatch - 1
                    For lParmIndex As Integer = 0 To lSubParmNames.Count - 1
                        Dim lData As New atcTimeseries(Me)
                        lData.Attributes.SetValue("Location", pSWMM5_OutputFile.SWMM_SubcatchId(lLocationIndex))
                        lData.Attributes.SetValue("LocationIndex", lLocationIndex)
                        lData.Attributes.SetValue("Constituent", lSubParmNames(lParmIndex))
                        lData.Attributes.SetValue("ObjectType", SWMM5_OutputFile.SUBCATCH)
                        lData.Attributes.SetValue("ParmIndex", lParmIndex)
                        lData.Attributes.SetValue("Scenario", lScenario)
                        lData.Attributes.SetValue("Units", lSubParmUnits(lParmIndex))
                        lData.ValuesNeedToBeRead = True
                        lData.numValues = lNumValues
                        lData.Dates = lDates
                        lData.Attributes.AddHistory("Read from " & Specification)
                        lData.Attributes.SetValue("ID", DataSets.Count + 1)
                        DataSets.Add(lData)
                    Next
                Next

                For lLocationIndex As Integer = 0 To pSWMM5_OutputFile.SWMM_Nlinks - 1
                    For lParmIndex As Integer = 0 To lLinkParmNames.Count - 1
                        Dim lData As New atcTimeseries(Me)
                        lData.Attributes.SetValue("Location", pSWMM5_OutputFile.SWMM_LinkId(lLocationIndex))
                        lData.Attributes.SetValue("LocationIndex", lLocationIndex)
                        lData.Attributes.SetValue("Constituent", lLinkParmNames(lParmIndex))
                        lData.Attributes.SetValue("ObjectType", SWMM5_OutputFile.LINK)
                        lData.Attributes.SetValue("ParmIndex", lParmIndex)
                        lData.Attributes.SetValue("Scenario", lScenario)
                        lData.Attributes.SetValue("Units", lLinkParmUnits(lParmIndex))
                        lData.ValuesNeedToBeRead = True
                        lData.numValues = lNumValues
                        lData.Dates = lDates
                        lData.Attributes.AddHistory("Read from " & Specification)
                        lData.Attributes.SetValue("ID", DataSets.Count + 1)
                        DataSets.Add(lData)
                    Next
                Next

                For lLocationIndex As Integer = 0 To pSWMM5_OutputFile.SWMM_Nnodes - 1
                    For lParmIndex As Integer = 0 To lNodeParmNames.Count - 1
                        Dim lData As New atcTimeseries(Me)
                        lData.Attributes.SetValue("Location", pSWMM5_OutputFile.SWMM_NodeId(lLocationIndex))
                        lData.Attributes.SetValue("LocationIndex", lLocationIndex)
                        lData.Attributes.SetValue("Constituent", lNodeParmNames(lParmIndex))
                        lData.Attributes.SetValue("ObjectType", SWMM5_OutputFile.NODE)
                        lData.Attributes.SetValue("ParmIndex", lParmIndex)
                        lData.Attributes.SetValue("Scenario", lScenario)
                        lData.Attributes.SetValue("Units", lNodeParmUnits(lParmIndex))
                        lData.ValuesNeedToBeRead = True
                        lData.numValues = lNumValues
                        lData.Dates = lDates
                        lData.Attributes.AddHistory("Read from " & Specification)
                        lData.Attributes.SetValue("ID", DataSets.Count + 1)
                        DataSets.Add(lData)
                    Next
                Next

                For lParmIndex As Integer = 0 To lSysParmNames.Length - 1
                    Dim lData As New atcTimeseries(Me)
                    lData.Attributes.SetValue("Location", "WHOLESYS")
                    lData.Attributes.SetValue("LocationIndex", 0)
                    lData.Attributes.SetValue("Constituent", lSysParmNames(lParmIndex))
                    lData.Attributes.SetValue("ObjectType", SWMM5_OutputFile.SYS)
                    lData.Attributes.SetValue("ParmIndex", lParmIndex)
                    lData.Attributes.SetValue("Scenario", lScenario)
                    lData.Attributes.SetValue("Units", lSysParmUnits(lParmIndex))
                    lData.ValuesNeedToBeRead = True
                    lData.numValues = lNumValues
                    lData.Dates = lDates
                    lData.Attributes.AddHistory("Read from " & Specification)
                    lData.Attributes.SetValue("ID", DataSets.Count + 1)
                    DataSets.Add(lData)
                Next

                Return True
            Catch e As Exception
                Logger.Dbg("Could not open " & aFileName & ": " & e.ToString)
            End Try
        End If
        Return False
    End Function

    Public Overrides Sub ReadData(ByVal aData As atcData.atcDataSet)
        Dim lData As atcTimeseries = aData
        lData.ValuesNeedToBeRead = False

        Dim lObjectType As Integer = aData.Attributes.GetValue("ObjectType")
        Dim lLocationIndex As Integer = aData.Attributes.GetValue("LocationIndex")
        Dim lParmIndex As Integer = aData.Attributes.GetValue("ParmIndex")
        Dim lValue As Single

        pSWMM5_OutputFile.OpenSwmmOutFile(Specification)

        For lTimeStep As Integer = 1 To pSWMM5_OutputFile.TimeStarts.Length - 1
            pSWMM5_OutputFile.GetSwmmResult(lObjectType, lLocationIndex, lParmIndex, lTimeStep, lValue)
            lData.Values(lTimeStep) = lValue
        Next

        pSWMM5_OutputFile.CloseSwmmOutFile()
    End Sub

    Public Sub New()
        Filter = pFilter
    End Sub
End Class
