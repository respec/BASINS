Option Strict Off
Option Explicit On

Imports atcData
Imports atcUtility
Imports MapWinUtility
Imports System.Collections
Imports System.IO
Imports System.Text
Imports atcTimeseriesNetCDF.NetCDF
Imports atcTimeseriesNetCDF.atcNetCDF

''' <summary>
''' Reads NetCDF binary files containing time-series values
''' </summary>
''' <remarks>
''' </remarks>
Public Class atcTimeseriesNetCDF
    Inherits atcTimeseriesSource

    Private Shared pFilter As String = "NetCDF Files (*.nc)|*.nc|All Files (*.*)|*.*"
    Private pErrorDescription As String
    Private pJulianOffset As Double = New Date(1900, 1, 1).Subtract(New Date(1, 1, 1)).TotalDays
    Private pDebug As Boolean

    Public Overrides ReadOnly Property Description() As String
        Get
            Return "NetCDF File"
        End Get
    End Property

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Timeseries::NetCDF"
        End Get
    End Property

    Public Overrides ReadOnly Property Category() As String
        Get
            Return "File"
        End Get
    End Property

    Public Overrides ReadOnly Property CanOpen() As Boolean
        Get
            Return True
        End Get
    End Property

    Public Property Debug() As Boolean
        Get
            Return pDebug
        End Get
        Set(ByVal value As Boolean)
            pDebug = value
        End Set
    End Property


    Public Overrides Function Open(ByVal aFileName As String, _
                          Optional ByVal aAttributes As atcData.atcDataAttributes = Nothing) As Boolean
        If aFileName Is Nothing OrElse aFileName.Length = 0 OrElse Not FileExists(aFileName) Then
            'aFileName = FindFile("Select " & Name & " file to open", , , Filter, True, , 1)

            Dim cdlg As New Windows.Forms.OpenFileDialog
            With cdlg
                .Title = "Select " & Name & " file(s) to open"
                .Filter = pFilter
                .FilterIndex = 1
                .DefaultExt = ".nc"
                .Multiselect = True
                If .ShowDialog() <> Windows.Forms.DialogResult.OK Then
                    ' user clicked Cancel
                    Logger.Dbg("User Cancelled File Selection Dialog")
                    Logger.LastDbgText = "" 'forget about this - user was in control - no additional message box needed
                    Return False
                End If
                aFileName = String.Join(";", .FileNames)
            End With
        End If
        Me.Specification = aFileName
        For Each lFileName As String In aFileName.Split(";")
            If Not FileExists(lFileName) Then
                pErrorDescription = "File '" & lFileName & "' not found"
                Me.Specification = ""
            Else
                Dim lLogfileName As String = Logger.FileName
                Try
                    If pDebug Then
                        Logger.StartToFile(IO.Path.ChangeExtension(lFileName, ".log"), , False, True)
                    End If

                    Dim lNetCDFFile As New atcNetCDFFile(lFileName)
                    If lNetCDFFile.TimeDimension Is Nothing Then
                        pErrorDescription = "No timeseries found in " & lFileName
                        Logger.Dbg(pErrorDescription)
                        Return False
                    Else
                        'how many timeseries?
                        Dim lTimeseriesCount As Integer = 1
                        If lNetCDFFile.EastWestDimension Is Nothing AndAlso lNetCDFFile.NorthSouthDimension IsNot Nothing Then
                            lTimeseriesCount = lNetCDFFile.NorthSouthDimension.Length
                        ElseIf lNetCDFFile.EastWestDimension IsNot Nothing AndAlso lNetCDFFile.NorthSouthDimension Is Nothing Then
                            lTimeseriesCount = lNetCDFFile.NorthSouthDimension.Length
                        ElseIf lNetCDFFile.EastWestDimension IsNot Nothing AndAlso lNetCDFFile.NorthSouthDimension IsNot Nothing Then
                            lTimeseriesCount = lNetCDFFile.EastWestDimension.Length * lNetCDFFile.NorthSouthDimension.Length
                        End If
                        Logger.Dbg("TimeseriesCount:" & lTimeseriesCount)

                        'get the timeseries
                        Dim lTimeVariable As atcNetCDFVariable = lNetCDFFile.Variables(lNetCDFFile.TimeDimension.ID)
                        Dim lTimeUnits As String = lTimeVariable.Attributes.GetValue("Units").ToString.Replace("days since ", "")
                        Dim lDateBase As Date = lTimeUnits.Substring(0, 10)
                        Dim lDates As New atcTimeseries(Me)
                        Dim lNumValues As Integer = lTimeVariable.Dimensions(0).Length
                        ReDim lDates.Values(lNumValues)
                        lDates.Values(0) = lDateBase.ToOADate
                        For lTimeIndex As Integer = 1 To lNumValues
                            lDates.Values(lTimeIndex) = lDates.Values(0) + lTimeVariable.Values(lTimeIndex - 1)
                        Next
                        Dim lDataVariable As atcNetCDFVariable = lNetCDFFile.Variables(lNetCDFFile.Variables.Count - 1)
                        For lTimeseriesIndex As Integer = 0 To lTimeseriesCount - 1
                            Dim lTimeseries As New atcTimeseries(Me)
                            lTimeseries.Dates = lDates
                            ReDim lTimeseries.Values(lNumValues)
                            For lTimeIndex As Integer = 0 To lNumValues - 1
                                Dim lValueIndex As Integer = (lTimeseriesCount * lTimeIndex) + lTimeseriesIndex
                                lTimeseries.Values(lTimeIndex + 1) = lDataVariable.Values(lValueIndex)
                            Next
                            Me.DataSets.Add(lTimeseries)
                        Next
                        Logger.Dbg("TimeseriesBuilt")
                    End If
                Catch e As Exception
                    Logger.Dbg("Exception reading '" & aFileName & "': " & e.Message, e.StackTrace)
                    Return False
                Finally
                    If pDebug Then
                        Logger.StartToFile(lLogfileName, True)
                    End If
                End Try
            End If
        Next
        Return (pData.Count > 0)
    End Function

    Public Sub New()
        Filter = pFilter
    End Sub

End Class
