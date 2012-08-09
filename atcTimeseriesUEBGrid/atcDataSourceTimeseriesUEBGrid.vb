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
        '        If MyBase.Open(aFileName, aAttributes) Then
        Dim lFileContents As String = WholeFileString(aFileName)
        Dim lRec As String
        Dim lDate(4) As Integer
        Dim lChrSepDates() As String = {" ", ".", vbTab}
        Dim lChrSepVals() As String = {" ", vbTab}
        Dim lDateStrArray() As String
        Dim lValStrArray() As String
        Dim lVarName As String
        Dim lWatershedID As Integer
        Dim lJDate As Double
        Dim lVal As Double
        Dim lDatasets As New atcTimeseriesGroup
        Dim lData As atcTimeseries
        Dim lGroupBuilder As New atcTimeseriesGroupBuilder(Me)
        Dim lTSBuilder As atcTimeseriesBuilder

        Specification = aFileName
        Logger.Status("Opening '" & Specification & "'", True)
        Try
            While lFileContents.Length > 0
                lRec = StrSplit(lFileContents, vbCrLf, "")
                lDateStrArray = lRec.Split(lChrSepDates, StringSplitOptions.None)
                For i As Integer = 0 To 4
                    Integer.TryParse(lDateStrArray(i), lDate(i))
                Next
                If lDate(4) > 0 Then 'process fraction of hour
                    lDate(4) = lDate(4) / 100 * 60
                End If
                lRec = StrSplit(lFileContents, vbCrLf, "") 'SHOULD BE REMOVED
                lValStrArray = lRec.Split(lChrSepVals, StringSplitOptions.None)
                lVarName = lValStrArray(0)
                lRec = StrSplit(lFileContents, vbCrLf, "") 'SHOULD BE REMOVED
                lValStrArray = lRec.Split(lChrSepVals, StringSplitOptions.None)
                Integer.TryParse(lValStrArray(1), lWatershedID)
                Double.TryParse(lValStrArray(2), lVal)
                lJDate = Jday(lDate(0), lDate(1), lDate(2), lDate(3), lDate(4), 0)
                lTSBuilder = lGroupBuilder.Builder(lVarName & ":" & lWatershedID.ToString)
                With lTSBuilder.Attributes
                    'Set attributes of newly created builder
                    If Not .ContainsAttribute("Location") Then
                        .SetValue("Scenario", FilenameNoExt(FilenameNoPath(aFileName)))
                        .SetValue("Location", lWatershedID)
                        .SetValue("Constituent", lVarName)
                        .SetValue("Description", "UEB Aggregated Output")
                        .AddHistory("Read from " & Specification)
                    End If
                End With
                lTSBuilder.AddValue(lJDate, lVal)
            End While
            lGroupBuilder.CreateTimeseriesAddToGroup(DataSets)
            Open = True

        Catch endEx As Exception
            Open = False
        End Try
        Logger.Status("")
        '        End If
    End Function

    Public Sub New()
        Filter = pFilter
    End Sub
End Class
