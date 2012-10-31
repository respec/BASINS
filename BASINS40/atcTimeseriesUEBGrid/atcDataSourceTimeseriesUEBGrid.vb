Imports atcUtility
Imports atcData
Imports MapWinUtility
Imports MapWinUtility.Strings

Public Class atcDataSourceTimeseriesUEBGrid
    Inherits atcTimeseriesSource
    '##MODULE_REMARKS Copyright 2007 AQUA TERRA Consultants - Royalty-free use permitted under open source license

    Private Shared pFilter As String = "UEBGrid Output Files (*.dat)|*.dat|(*.txt)|*.txt|All Files|*.*"
    Private pColDefs As Hashtable

    Public Overrides ReadOnly Property Description() As String
        Get
            Return "UEBGrid Output"
        End Get
    End Property

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Timeseries::UEBGrid Output"
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
            Logger.Status("Opening '" & Specification & "'", True)
            Dim lRec As String
            Dim lChrSepVals() As String = {" ", vbTab}
            Dim lStrArray() As String
            Dim lVarName As String
            Dim lWatershedID As Integer
            Dim lJDate As Double
            Dim lVal As Double
            Dim lDatasets As New atcTimeseriesGroup
            Dim lGroupBuilder As New atcTimeseriesGroupBuilder(Me)
            Dim lTSBuilder As atcTimeseriesBuilder
            Dim lYear As Integer
            Dim lMonth As Integer
            Dim lDay As Integer
            Dim lHour As Integer
            Dim lMinute As Integer
            Dim lScenario As String = FilenameNoExt(FilenameNoPath(Specification))

            Try
                For Each lRec In LinesInFile(aFileName)
                    lRec = ReplaceRepeats(lRec, " ")
                    lStrArray = lRec.Split(lChrSepVals, StringSplitOptions.None)
                    If Integer.TryParse(lStrArray(0), lYear) AndAlso _
                       Integer.TryParse(lStrArray(1), lMonth) AndAlso _
                       Integer.TryParse(lStrArray(2), lDay) Then
                        If lStrArray(3).Contains(".") Then 'process fraction of hour
                            Integer.TryParse(lStrArray(3).Substring(0, lStrArray(3).IndexOf(".")), lHour)
                            Integer.TryParse(lStrArray(3).Substring(lStrArray(3).IndexOf(".") + 1), lMinute)
                            lMinute = 60 * lMinute / 100
                        Else
                            Integer.TryParse(lStrArray(3), lHour)
                            lMinute = 0
                        End If
                        lVarName = lStrArray(4)
                        Integer.TryParse(lStrArray(5), lWatershedID)
                        Double.TryParse(lStrArray(6), lVal)
                        lJDate = Jday(lYear, lMonth, lDay, lHour, lMinute, 0)
                        lTSBuilder = lGroupBuilder.Builder(lVarName & ":" & lWatershedID.ToString)
                        If lTSBuilder.NumValues = 0 Then
                            With lTSBuilder.Attributes
                                'Set attributes of newly created builder
                                If Not .ContainsAttribute("Location") Then
                                    .SetValue("Scenario", lScenario)
                                    .SetValue("Location", lWatershedID)
                                    .SetValue("Constituent", lVarName)
                                    .SetValue("Stanam", "UEBGrid:" & lVarName)
                                    .SetValue("Description", "UEBGrid Aggregated Output")
                                    .SetValue("Units", "meters")
                                    .AddHistory("Read from " & Specification)
                                End If
                            End With
                        End If
                        lTSBuilder.AddValue(lJDate, lVal)
                    End If
                Next
                lGroupBuilder.CreateTimeseriesAddToGroup(DataSets)
                Open = True

            Catch endEx As Exception
                Open = False
            End Try
            Logger.Status("")
        End If
    End Function

    Public Sub New()
        Filter = pFilter
    End Sub
End Class
