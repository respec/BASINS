Imports atcData
Imports atcUtility
Imports MapWinUtility
Imports System.Xml

''' <summary>
''' 
''' </summary>
''' <remarks>
'''Copyright 2005-2008 AQUA TERRA Consultants - Royalty-free use permitted under open source license
''' </remarks>
Public Class atcTimeseriesSUSTAIN
    Inherits atcData.atcTimeseriesSource

    Public WatershedNumber As Integer
    Public WatershedAcres As Double
    Public Delimiter As Char = vbTab

    Private pFilter As String = "SUSTAIN Timeseries Files (*.txt)|*.txt"
    Private pName As String = "Timeseries::SUSTAIN"
    Private Shared pNaN As Double = GetNaN()
    Private Const pUnknownUnits As String = "<unknown>"

    Public ReadOnly Property AvailableAttributes() As Collection
        Get
            'needed to edit attributes? that can't be done for this type!
            Return New Collection 'empty!
        End Get
    End Property

    Public Overrides ReadOnly Property Category() As String
        Get
            Return "File"
        End Get
    End Property

    Public Overrides ReadOnly Property Description() As String
        Get
            Return "SUSTAIN Timeseries"
        End Get
    End Property

    Public WriteOnly Property HelpFilename() As String
        Set(ByVal newValue As String)
            'TODO:how do we handle helpfiles?
            'App.HelpFile = newvalue
        End Set
    End Property

    Public ReadOnly Property Label() As String
        Get
            Return "SUSTAIN"
        End Get
    End Property

    Public Overrides Function Open(ByVal aSpecification As String, Optional ByVal aAttributes As atcData.atcDataAttributes = Nothing) As Boolean
        If MyBase.Open(aSpecification, aAttributes) Then
            Dim lInHeader As Boolean = True
            Dim lDelimiters(0) As Char
            Logger.Dbg("Opening atcTimeseriesSUSTAIN file: " & aSpecification)
            lDelimiters(0) = " "c
            Dim lGroupBuilder As New atcTimeseriesGroupBuilder(Me)
            For Each lLine As String In LinesInFile(aSpecification)
                If lInHeader Then
                    If lLine.ToLower.Contains("date/time") Then
                        lInHeader = False
                    Else
                        'TODO: parse constituents, descriptions, units
                    End If
                Else
                    Dim lDateDouble As Double
                    Dim lFieldValues() As String
                    If lLine.Contains(vbTab) Then
                        lFieldValues = lLine.Split(vbTab)
                    Else
                        lFieldValues = lLine.Split(lDelimiters, StringSplitOptions.RemoveEmptyEntries)
                    End If
                    Try
                        If lFieldValues.Count > 6 Then
                            lDateDouble = atcUtility.modDate.Date2J(lFieldValues(1), lFieldValues(2), lFieldValues(3), lFieldValues(4), lFieldValues(5), 0)
                        End If
                        While DataSets.Count < lFieldValues.Count - 6
                            DataSets.Add(New atcTimeseries(Me))
                        End While


                    Catch exParse As Exception
                        Logger.Dbg("Unable to find dates/values in line: '" & lLine & "' " & vbCrLf & exParse.Message)
                    End Try
                End If
            Next
            Return True
        End If
        Return False
    End Function

    Public Function RemoveTimSer(ByVal aTimeseries As atcTimeseries) As Boolean
        Throw New ApplicationException("Unable to Remove Time Series " & aTimeseries.ToString & vbCrLf & "From:" & Specification)
    End Function

    Public Function RewriteTimSer(ByVal aTimeseries As atcTimeseries) As Boolean
        Throw New ApplicationException("Unable to Rewrite Time Series " & aTimeseries.ToString & vbCrLf & "From:" & Specification)
    End Function

    Public Overrides Function Save(ByVal aSaveFileName As String, Optional ByVal aExistAction As atcData.atcDataSource.EnumExistAction = atcData.atcDataSource.EnumExistAction.ExistReplace) As Boolean
        Try
            Logger.Dbg("Save atcTimeseriesSUSTAIN in " & aSaveFileName)
            If IO.File.Exists(aSaveFileName) Then
                Dim lExtension As String = IO.Path.GetExtension(aSaveFileName)
                Dim lRenamedFilename As String = GetTemporaryFileName(aSaveFileName.Substring(0, aSaveFileName.Length - lExtension.Length), lExtension)
                Select Case aExistAction
                    Case EnumExistAction.ExistAppend
                        Logger.Dbg("Save: File already exists and aExistAction = ExistAppend, not implemented.")
                        Throw New ApplicationException("Append not implemented for atcTimeseriesSUSTAIN.Save")
                    Case EnumExistAction.ExistAskUser
                        Select Case Logger.MsgCustom("Attempting to save but file already exists: " & vbCrLf & aSaveFileName, "File already exists", _
                                                     "Overwrite", "Do not write", "Save as " & IO.Path.GetFileName(lRenamedFilename))
                            Case "Overwrite"
                                IO.File.Delete(aSaveFileName)
                            Case "Do not write"
                                Return False
                            Case Else
                                aSaveFileName = lRenamedFilename
                        End Select
                    Case EnumExistAction.ExistNoAction
                        Logger.Dbg("Save: File already exists and aExistAction = ExistNoAction, not saving " & aSaveFileName)
                    Case EnumExistAction.ExistReplace
                        Logger.Dbg("Save: File already exists, deleting old " & aSaveFileName)
                        IO.File.Delete(aSaveFileName)
                    Case EnumExistAction.ExistRenumber
                        Logger.Dbg("Save: File already exists and aExistAction = ExistRenumber, saving as " & lRenamedFilename)
                        aSaveFileName = lRenamedFilename
                End Select
            End If

            Dim lTimeseries As atcTimeseries = Me.DataSets(0)
            Dim lWriter As New System.IO.StreamWriter(aSaveFileName)
            lWriter.WriteLine("TT-----------------------------------------------------------------------------------------")
            lWriter.WriteLine("TT LSPC -- Formatted file for use by SUSTAIN")
            lWriter.WriteLine("TT Version 4.1 ")
            lWriter.WriteLine("TT")
            lWriter.WriteLine("TT Designed and maintained by:")
            lWriter.WriteLine("TT     AQUA TERRA Consultants")
            lWriter.WriteLine("TT     http://aquaterra.com/")
            lWriter.WriteLine("TT     Mountain View, CA")
            lWriter.WriteLine("TT     (650) 962-1864")
            lWriter.WriteLine("TT-----------------------------------------------------------------------------------------")
            lWriter.WriteLine("TT LSPC MODEL OUTPUT FILE")
            lWriter.WriteLine("TT Time interval: " & lTimeseries.Attributes.GetValue("Interval", JulianHour) / JulianMinute & " min      Output option: timestep")
            lWriter.WriteLine("TT Label")

            'lWriter.WriteLine("TT SURO     surface outflow volume (in-acre/timestep)")
            'lWriter.WriteLine("TT AGWI     groundwater recharge volume (in-acre/timestep)")
            'lWriter.WriteLine("TT SOSED    sediments load from land (tons/timestep)")
            'lWriter.WriteLine("TT SOQUAL   surface flux of QUAL from the PLS_TP (lb/timestep)")
            For Each lTimeseries In Me.DataSets
                lWriter.WriteLine("TT " & lTimeseries.Attributes.GetValue("Constituent").ToString.PadRight(8) & " " _
                                        & lTimeseries.Attributes.GetValue("Description") & " (" _
                                        & lTimeseries.Attributes.GetValue("Units") & ")")
            Next

            lWriter.WriteLine("TT")
            lWriter.WriteLine("TT WATERSHED_" & WatershedNumber & " Area:    " & Format(WatershedAcres, "0.000") & " (acres)")
            lWriter.WriteLine("TT Date/time					Values")
            lWriter.WriteLine("TT")

            Dim lLastTimeStep As Integer = Me.DataSets(0).Dates.numValues

            For lTimeStep As Integer = 1 To lLastTimeStep
                Dim lDateArray(5) As Integer
                modDate.J2Date(Me.DataSets(0).Dates.Value(lTimeStep), lDateArray)
                lWriter.Write(WatershedNumber & Delimiter & lDateArray(0) & Delimiter & lDateArray(1) & Delimiter & lDateArray(2) & Delimiter & lDateArray(3) & Delimiter & lDateArray(4))
                For Each lTimeseries In Me.DataSets
                    'TODO: if delimiter is a space, fill column to some width?
                    lWriter.Write(Delimiter & Format(lTimeseries.Value(lTimeStep), "0.00E+00"))
                Next
                lWriter.WriteLine()
            Next

            lWriter.Close()
            Return True
        Catch e As Exception
            Logger.Msg("Error writing '" & aSaveFileName & "': " & e.ToString, MsgBoxStyle.OkOnly, "Did not write file")
            Return False
        End Try
    End Function

    Public Overrides ReadOnly Property Name() As String
        Get
            Return pName
        End Get
    End Property

    Public Overrides ReadOnly Property CanOpen() As Boolean
        Get
            Return True
        End Get
    End Property

    Public Overrides ReadOnly Property CanSave() As Boolean
        Get
            Return True
        End Get
    End Property

    Public Sub New()
        Filter = pFilter
    End Sub

    Private Shared pShowViewMessage As Boolean = True

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
End Class