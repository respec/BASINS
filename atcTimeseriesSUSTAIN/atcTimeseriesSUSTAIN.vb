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
    Public Delimiter As String = vbTab
    Public HeaderLineStart As String = "PLOT"
    Public BodyLineStart As String = "WatershedNumber"
    Public IncludeTimeWhenSaving As Boolean = True 'Change to False to save daily values in SWMM format (for SUSTAINOPT Climate.txt), also affects writing of headers

    Private pFilter As String = "SUSTAIN Timeseries Files (*.txt)|*.txt|PLTGEN Files (*.p*)|*.p*|All Files|*.*"
    Private pName As String = "Timeseries::SUSTAIN/PLTGEN"
    Private Shared pNaN As Double = GetNaN()
    Private Const pUnknownUnits As String = "<unknown>"

    Public ReadOnly Property AvailableAttributes() As Collection
        Get
            Return New Collection
        End Get
    End Property

    Public Overrides ReadOnly Property Category() As String
        Get
            Return "File"
        End Get
    End Property

    Public Overrides ReadOnly Property Description() As String
        Get
            Return "SUSTAIN or PLTGEN Timeseries"
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
            Return "SUSTAIN/PLTGEN"
        End Get
    End Property

    Public Overrides Function Open(ByVal aSpecification As String, Optional ByVal aAttributes As atcData.atcDataAttributes = Nothing) As Boolean
        If MyBase.Open(aSpecification, aAttributes) Then
            Try
                Dim lInHeader As Boolean = True
                Dim lInCurveLabels As Boolean = False
                Dim lLabelStart As Integer = 0
                Dim lLabelEnd As Integer = 0
                Dim lDescStart As Integer = 0
                Dim lTRANStart As Integer = 0
                'Dim lTRANCODStart As Integer = 0
                Dim lArea As Double = 0
                Dim lSpaceSeparator(0) As Char
                lSpaceSeparator(0) = " "c
                Logger.Dbg("Opening atcTimeseriesSUSTAIN file: " & Specification)
                Dim lGroupBuilder As New atcTimeseriesGroupBuilder(Me)
                Dim lSkipBlank As Integer = 0
                For Each lLine As String In LinesInFile(Specification)
                    If lInHeader Then
                        If lLine.ToLower.Contains("date/time") Then
                            lInHeader = False
                        ElseIf lLine.Contains("Area:") Then
                            lInCurveLabels = False
                            Dim lAreaStart As Integer = lLine.IndexOf("Area:") + 5
                            Dim lAreaEnd As Integer = lLine.IndexOf("(", lAreaStart)
                            If lAreaEnd > lAreaStart Then
                                Double.TryParse(lLine.Substring(lAreaStart, lAreaEnd - lAreaStart).Trim, lArea)
                            End If
                        ElseIf lInCurveLabels Then
                            Dim lConstituent As String = SafeSubstring(lLine, lLabelStart, lLabelEnd - lLabelStart + 1).Trim
                            If lConstituent.Length = 0 AndAlso lSkipBlank > 0 Then
                                lSkipBlank -= 1
                            ElseIf lConstituent.Length = 0 OrElse lConstituent.StartsWith("Time series (") Then
                                lInCurveLabels = False
                            Else
                                Dim lDescription As String = lConstituent
                                Dim lUnits As String = ""
                                Dim lTran As String = ""
                                Dim lTranCode As Integer = atcTran.TranNative
                                If lDescStart > 0 Then
                                    lDescription = SafeSubstring(lLine, lDescStart).Trim()
                                    Dim lOpenParen As Integer = lDescription.IndexOf("("c)
                                    If lOpenParen > 0 Then
                                        lUnits = modString.SafeSubstring(lDescription, lOpenParen + 1, lDescription.Length - lOpenParen - 2)
                                        lDescription = modString.SafeSubstring(lDescription, 0, lDescription.Length - lUnits.Length - 2).TrimEnd
                                    End If
                                End If
                                If lTRANStart > 0 Then
                                    Dim lTranAndCode() As String = lLine.Substring(lTRANStart).Split(lSpaceSeparator, StringSplitOptions.RemoveEmptyEntries)
                                    lTran = lTranAndCode(0)
                                    If lTranAndCode.Length > 1 Then
                                        Integer.TryParse(lTranAndCode(1), lTranCode)
                                    End If
                                End If
                                'Dim lKey As String = lConstituent & "," & lDescription & "," & lUnits & "," & lTranCode
                                With lGroupBuilder.Builder(lGroupBuilder.Count + 1)
                                    .Attributes.SetValue("Constituent", lConstituent)
                                    .Attributes.SetValue("Description", lDescription)
                                    If Not String.IsNullOrEmpty(lUnits) Then .Attributes.SetValue("Units", lUnits)
                                    If Not String.IsNullOrEmpty(lTran) Then .Attributes.SetValue("Transformation", lTran)
                                    If lTranCode <> atcTran.TranNative Then .Attributes.SetValue("TransformationCode", lTranCode)
                                End With
                            End If
                        ElseIf lLine.Contains("This output file was created at") Then
                            lInCurveLabels = True
                            lLabelStart = lLine.IndexOf("This output file was created at")
                            lLabelEnd = lLabelStart + 15
                            lDescStart = lLabelEnd + 1
                            lSkipBlank = 1
                        ElseIf lLine.EndsWith("Label") OrElse lLine.EndsWith("TRANCOD") Then
                            lInCurveLabels = True
                            lLabelStart = lLine.IndexOf("Label")
                            lLabelEnd = lLine.IndexOf("LINTYP")
                            If lLabelEnd < 0 Then
                                lLabelEnd = lLabelStart + 8
                                lDescStart = lLabelEnd + 1
                            Else
                                lTRANStart = lLine.IndexOf("TRAN ")
                                'lTRANCODStart = lLine.IndexOf("TRANCOD")
                            End If
                        End If
                    Else
                        Dim lDate As Date
                        Dim lDateDouble As Double
                        Dim lFieldValues As New Generic.List(Of String)
                        Try
                            If lLine.Contains(vbTab) Then
                                Dim lCurrentFieldValue As String = ""
                                For Each lCh In lLine
                                    Select Case lCh
                                        Case " "
                                            If lCurrentFieldValue.Length > 0 Then
                                                lFieldValues.Add(lCurrentFieldValue)
                                                lCurrentFieldValue = ""
                                            End If
                                        Case vbTab
                                            lFieldValues.Add(lCurrentFieldValue)
                                            lCurrentFieldValue = ""
                                        Case Else
                                            lCurrentFieldValue &= lCh
                                    End Select
                                Next
                                If lCurrentFieldValue.Length > 0 Then lFieldValues.Add(lCurrentFieldValue)
                            ElseIf lLine.Length > 22 + (lGroupBuilder.Count - 1) * 13 Then
                                lFieldValues.Add(lLine.Substring(0, 6).Trim) 'Prefix, not used
                                lFieldValues.Add(lLine.Substring(6, 4)) 'Year
                                lFieldValues.Add(lLine.Substring(11, 2)) 'Month
                                lFieldValues.Add(lLine.Substring(14, 2)) 'Day
                                lFieldValues.Add(lLine.Substring(17, 2)) 'Hour
                                lFieldValues.Add(lLine.Substring(20, 2)) 'Minute
                                For lDatasetIndex As Integer = 0 To lGroupBuilder.Count - 1
                                    lFieldValues.Add(SafeSubstring(lLine, 22 + 14 * lDatasetIndex, 14))
                                Next
                            End If
                            If lFieldValues.Count > 6 Then
                                lDateDouble = atcUtility.modDate.Date2J(lFieldValues(1), lFieldValues(2), lFieldValues(3), lFieldValues(4), lFieldValues(5), 0)
                                lDate = Date.FromOADate(lDateDouble) ' New Date(lFieldValues(1), lFieldValues(2), lFieldValues(3), lFieldValues(4), lFieldValues(5), 0)
                                Dim lDataValues(lGroupBuilder.Count - 1) As Double
                                For lDatasetIndex As Integer = 0 To lGroupBuilder.Count - 1
                                    Select Case lFieldValues(lDatasetIndex + 6)
                                        Case "-1.0000000E+30"
                                            lDataValues(lDatasetIndex) = pNaN
                                        Case Else
                                            If Not Double.TryParse(lFieldValues(lDatasetIndex + 6), lDataValues(lDatasetIndex)) Then
                                                Logger.Dbg("Unable to parse value in dataset " & lDatasetIndex + 1 & "' value = '" & lFieldValues(lDatasetIndex + 6) & "'" & vbCrLf & " line: '" & lLine & "'")
                                                lDataValues(lDatasetIndex) = pNaN
                                            End If
                                    End Select
                                Next
                                lGroupBuilder.AddValues(lDate, lDataValues)
                            End If
                        Catch exParse As Exception
                            Logger.Dbg("Unable to find dates/values in line: '" & lLine & "' " & vbCrLf & exParse.Message)
                        End Try
                    End If
                Next
                Logger.Dbg("Read " & lGroupBuilder.Count & " Timeseries")
                Logger.Status("")
                lGroupBuilder.CreateTimeseriesAddToGroup(Me.DataSets)

                'Shift all dates one time step, set all timeseries to refer to same Dates
                Dim lDates As atcTimeseries = Me.DataSets(0).Dates
                Dim lLastInterval As Double = lDates.Value(lDates.numValues) - lDates.Value(lDates.numValues - 1)

                For lIndex As Integer = 0 To lDates.numValues - 1
                    lDates.Value(lIndex) = lDates.Value(lIndex + 1)
                Next
                lDates.Value(lDates.numValues) = lDates.Value(lDates.numValues - 1) + lLastInterval
                lDates.Attributes.DiscardCalculated()
                lDates.Attributes.SetValue("Shared", True)
                For Each lTs As atcTimeseries In Me.DataSets
                    If lTs.Dates.Serial <> lDates.Serial Then
                        lTs.Dates.Clear()
                        lTs.Dates = lDates
                    End If
                Next

                Return True
            Catch ex As Exception
                Logger.Dbg("Exception opening atcTimeseriesSUSTAIN " & Specification & vbCrLf & ex.ToString)
                Throw ex
            End Try
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
            If IncludeTimeWhenSaving Then
                lWriter.WriteLine(HeaderLineStart & "-----------------------------------------------------------------------------------------")
                lWriter.WriteLine(HeaderLineStart & " HSPF output formatted for use by SUSTAIN")
                lWriter.WriteLine(HeaderLineStart & "")
                lWriter.WriteLine(HeaderLineStart & "     AQUA TERRA Consultants")
                lWriter.WriteLine(HeaderLineStart & "     http://aquaterra.com/")
                lWriter.WriteLine(HeaderLineStart & "     Mountain View, CA")
                lWriter.WriteLine(HeaderLineStart & "     (650) 962-1864")
                lWriter.WriteLine(HeaderLineStart & "-----------------------------------------------------------------------------------------")
                lWriter.WriteLine(HeaderLineStart & " MODEL OUTPUT FILE")
                lWriter.WriteLine(HeaderLineStart & " Time interval: " & lTimeseries.Attributes.GetValue("Interval", JulianHour) / JulianMinute & " min      Output option: timestep")
                lWriter.WriteLine(HeaderLineStart & " Label")
            End If

            Dim lLastTimeStep As Integer = lTimeseries.Dates.numValues
            Dim lInterval As Double = lTimeseries.Attributes.GetValue("Interval", JulianHour)
            Dim lDatasetsToWrite As New atcTimeseriesGroup(lTimeseries)
            For Each lTimeseries In Me.DataSets

                If lTimeseries.Attributes.GetValue("Interval", JulianHour) <> lInterval Then
                    Logger.Msg("Different interval data cannot be written to same file, skipping " & lTimeseries.ToString & " - " & DoubleToString(lTimeseries.Attributes.GetValue("Interval", JulianHour) * 24) & " hours <> " & DoubleToString(lInterval * 24))
                ElseIf lTimeseries.Dates.numValues < lLastTimeStep Then
                    Logger.Msg("Different number of values cannot be written to same file, skipping " & lTimeseries.ToString & " which contains " & lTimeseries.Dates.numValues & " values instead of " & lLastTimeStep)
                Else
                    lDatasetsToWrite.Add(lTimeseries)
                End If
            Next

            If IncludeTimeWhenSaving Then
                For Each lTimeseries In lDatasetsToWrite
                    Dim lUnits As String = lTimeseries.Attributes.GetValue("Units", "<unknown>")
                    If lUnits = "<unknown>" Then
                        lUnits = ""
                    Else
                        lUnits = " (" & lUnits & ")"
                    End If
                    lWriter.WriteLine(HeaderLineStart & " " _
                                    & lTimeseries.Attributes.GetValue("Constituent").ToString.PadRight(8) & " " _
                                    & lTimeseries.Attributes.GetValue("Description") & lUnits)
                Next

                lWriter.WriteLine(HeaderLineStart)
                If WatershedAcres > 0 Then
                    lWriter.WriteLine(HeaderLineStart & " WATERSHED_" & WatershedNumber & " Area:    " & Format(WatershedAcres, "0.000") & " (acres)")
                End If
                lWriter.WriteLine(HeaderLineStart & " Date/time					Values")
                lWriter.WriteLine(HeaderLineStart)
            End If

            Dim lSeasonal As Boolean = False
            If lDatasetsToWrite(0).Attributes.ContainsAttribute("seasbg") Then lSeasonal = True
            Dim lSeasons As New atcSeasonsYearSubset(lDatasetsToWrite(0).Attributes.GetValue("seasbg"),
                                                     lDatasetsToWrite(0).Attributes.GetValue("seadbg"),
                                                     lDatasetsToWrite(0).Attributes.GetValue("seasnd"),
                                                     lDatasetsToWrite(0).Attributes.GetValue("seadnd"))

            Dim lBodyLineStart As String = BodyLineStart.Replace("WatershedNumber", WatershedNumber)
            For lTimeStep As Integer = 1 To lLastTimeStep
                Dim lDateArray(5) As Integer
                modDate.J2Date(lDatasetsToWrite(0).Dates.Value(lTimeStep) - lInterval, lDateArray)
                If lSeasonal Then 'make sure date is in seasonal range
                    Dim lIndex As Integer = lSeasons.SeasonIndex(lDatasetsToWrite(0).Dates.Value(lTimeStep) - lInterval)
                    If lIndex = 0 Then 'outside of seasonal range, don't write it
                        GoTo FinishedLine
                    End If
                End If
                lWriter.Write(lBodyLineStart)
                If Delimiter = " " Then
                    lWriter.Write(lDateArray(0).ToString.PadLeft(6) &
                                  lDateArray(1).ToString.PadLeft(3) &
                                  lDateArray(2).ToString.PadLeft(3))
                    If IncludeTimeWhenSaving Then
                        lWriter.Write(lDateArray(3).ToString.PadLeft(3) &
                                      lDateArray(4).ToString.PadLeft(3))
                    End If

                    For Each lTimeseries In lDatasetsToWrite
                        lWriter.Write(Format(lTimeseries.Value(lTimeStep), "0.00000E+00").PadLeft(14))
                    Next
                Else
                    lWriter.Write(Delimiter & lDateArray(0) & _
                                  Delimiter & lDateArray(1) & _
                                  Delimiter & lDateArray(2))
                    If IncludeTimeWhenSaving Then
                        lWriter.Write(Delimiter & lDateArray(3) & _
                                      Delimiter & lDateArray(4))
                    End If
                    For Each lTimeseries In lDatasetsToWrite
                        lWriter.Write(Delimiter & Format(lTimeseries.Value(lTimeStep), "0.00E+00"))
                    Next
                End If
                lWriter.WriteLine()
FinishedLine:
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