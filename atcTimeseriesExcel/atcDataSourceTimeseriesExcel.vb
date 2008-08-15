Imports atcData
Imports atccontrols
Imports atcUtility
Imports MapWinUtility

Public Class atcDataSourceTimeseriesExcel
    Inherits atcDataSource
    '##MODULE_REMARKS Copyright 2007 AQUA TERRA Consultants - Royalty-free use permitted under open source license

    Private Shared pFilter As String = "XLS Files (*.xls)|*.xls"
    Private pDatesColumns As Boolean = True

    Public Overrides ReadOnly Property Description() As String
        Get
            Return "Timeseries EXCEL"
        End Get
    End Property

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Timeseries::EXCEL"
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

    Public Overrides Function Open(ByVal aFileName As String, Optional ByVal aAttributes As atcData.atcDataAttributes = Nothing) As Boolean
        If MyBase.Open(aFileName, aAttributes) Then
            'requires 'Office 2003 Update: Redistributable Primary Interop Assemblies'
            'how do we reference these?
            'Dim xlApp As Microsoft.Office.Interop.Excel.Application
            'Dim xlBook As Microsoft.Office.Interop.Excel.Workbook
            'Dim xlSheet As Microsoft.Office.Interop.Excel.Worksheet

            Dim lConnection As System.Data.OleDb.OleDbConnection
            lConnection = New System.Data.OleDb.OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0;" & _
                                                                "data source=" & Specification & ";" & _
                                                                "Extended Properties=Excel 8.0;")
            lConnection.Open()

            ' Select the data from a sheet of the workbook.
            Dim lConstituentName As String = "NH4" ' TODO: need to enumerate available sheets and select
            Dim lQuery As String = "select * from [" & lConstituentName & "$]"
            Dim lDataAdapter As System.Data.OleDb.OleDbDataAdapter
            lDataAdapter = New System.Data.OleDb.OleDbDataAdapter(lQuery, lConnection)

            Dim lSysDataSet As New System.Data.DataSet()
            lDataAdapter.Fill(lSysDataSet)

            lConnection.Close()

            Dim lData, lDates As atcTimeseries
            Dim lLocation As String = ""
            Dim lConstituentUnits As String = ""
            Dim lTSKey As String

            With lSysDataSet.Tables(0)
                lDates = New atcTimeseries(Me)
                lDates.Value(0) = GetNaN
                lDates.numValues = .Columns.Count
                Dim lCount As Integer = 0
                For lCol As Integer = 1 To .Columns.Count - 1
                    Dim lValue As Object = .Rows(1).Item(lCol)
                    If Not TypeOf (lValue) Is DBNull Then 'save the date
                        lCount += 1
                        lDates.Value(lCount) = lValue
                    End If
                Next
                lDates.numValues = lCount

                For lRow As Integer = 2 To .Rows.Count - 1
                    Dim lValue As Object = .Rows(lRow).Item(0)
                    If TypeOf (lValue) Is DBNull Then 'done dates
                        Exit For
                    Else
                        lLocation = lValue
                        lTSKey = lLocation & ":" & lConstituentName
                        lData = New atcTimeseries(Me)
                        lData.Dates = lDates
                        lData.numValues = lDates.numValues
                        lData.Value(0) = GetNaN()
                        lData.Attributes.SetValue("ID", DataSets.Count + 1)
                        lData.Attributes.SetValue("Scenario", "OBSERVED")
                        lData.Attributes.SetValue("Location", lLocation)
                        lData.Attributes.SetValue("Constituent", lConstituentName)
                        'lData.Attributes.SetValue("Units", lConstituentUnits)
                        lData.Attributes.SetValue("Point", True)
                        For lCol As Integer = 1 To .Columns.Count - 1
                            Dim lDateValue As Object = .Rows(lRow).Item(lCol)
                            If TypeOf (lDateValue) Is DBNull Then 'save the date
                                lData.Value(lCol) = GetNaN()
                            Else
                                lData.Value(lCol) = lDateValue
                            End If
                        Next
                        lData.Attributes.AddHistory("Read from " & Specification)
                        DataSets.Add(lTSKey, lData)
                    End If
                Next
            End With
            Open = True
        End If
    End Function

    Public Sub New()
        Filter = pFilter
    End Sub
End Class
