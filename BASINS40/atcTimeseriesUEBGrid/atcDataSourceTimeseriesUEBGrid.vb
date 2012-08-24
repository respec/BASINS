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
            Dim lFileContents As String = WholeFileString(Specification)
            Dim lRec As String
            Dim lDate(4) As Integer
            Dim lChrSepVals() As String = {" ", vbTab}
            Dim lStrArray() As String
            Dim lVarName As String
            Dim lWatershedID As Integer
            Dim lJDate As Double
            Dim lVal As Double
            Dim lDatasets As New atcTimeseriesGroup
            Dim lGroupBuilder As New atcTimeseriesGroupBuilder(Me)
            Dim lTSBuilder As atcTimeseriesBuilder

            Logger.Status("Opening '" & Specification & "'", True)
            Try
                Do 'may need to process header record
                    lRec = StrSplit(lFileContents, vbCrLf, "")
                    lRec = ReplaceRepeats(lRec, " ")
                    lStrArray = lRec.Split(lChrSepVals, StringSplitOptions.None)
                Loop Until IsNumeric(lStrArray(0))
                While lFileContents.Length > 0
                    For i As Integer = 0 To 2
                        Integer.TryParse(lStrArray(i), lDate(i))
                    Next
                    If lStrArray(3).Contains(".") Then 'process fraction of hour
                        Integer.TryParse(lStrArray(3).Substring(0, lStrArray(3).IndexOf(".")), lDate(3))
                        Integer.TryParse(lStrArray(3).Substring(lStrArray(3).IndexOf(".") + 1), lDate(4))
                        lDate(4) = lDate(4) / 100 * 60
                    Else
                        Integer.TryParse(lStrArray(3), lDate(3))
                        lDate(4) = 0
                    End If
                    lVarName = lStrArray(4)
                    Integer.TryParse(lStrArray(5), lWatershedID)
                    Double.TryParse(lStrArray(6), lVal)
                    lJDate = Jday(lDate(0), lDate(1), lDate(2), lDate(3), lDate(4), 0)
                    lTSBuilder = lGroupBuilder.Builder(lVarName & ":" & lWatershedID.ToString)
                    With lTSBuilder.Attributes
                        'Set attributes of newly created builder
                        If Not .ContainsAttribute("Location") Then
                            .SetValue("Scenario", FilenameNoExt(FilenameNoPath(Specification)))
                            .SetValue("Location", lWatershedID)
                            .SetValue("Constituent", lVarName)
                            .SetValue("Description", "UEB Aggregated Output")
                            .AddHistory("Read from " & Specification)
                        End If
                    End With
                    lTSBuilder.AddValue(lJDate, lVal)
                    lRec = StrSplit(lFileContents, vbCrLf, "")
                    lRec = ReplaceRepeats(lRec, " ")
                    lStrArray = lRec.Split(lChrSepVals, StringSplitOptions.None)
                End While
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
