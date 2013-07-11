Imports atcUtility
Imports atcData
Imports MapWinUtility
Imports System.IO

Public Class atcTimeseriesGeoSFMOutput
    Inherits atcTimeseriesSource
    '##MODULE_REMARKS Copyright 2010 AQUA TERRA Consultants - Royalty-free use permitted under open source license

    Private Shared pFileFilter As String = "GeoSFM Output Files (*.*)|*.*"
    Private pErrorDescription As String
    Private pChrSep() As String = {" ", ",", vbCrLf}

    Public Overrides ReadOnly Property Description() As String
        Get
            Return "GeoSFM Model Output Data"
        End Get
    End Property

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Timeseries::GeoSFM Output"
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
        Dim lData As atcTimeseries
        Dim lTSAttributes() As String

        If aFileName Is Nothing OrElse aFileName.Length = 0 OrElse Not FileExists(aFileName) Then
            aFileName = FindFile("Select " & Name & " file to open", , , pFileFilter, True, , 1)
        End If

        If Not FileExists(aFileName) Then
            pErrorDescription = "File '" & aFileName & "' not found"
            Open = False
        Else
            MyBase.Open(aFileName)
            Dim inStream As New FileStream(aFileName, FileMode.Open, FileAccess.Read)
            Dim inBuffer As New BufferedStream(inStream)
            Dim inReader As New BinaryReader(inBuffer)
            Dim curLine As String = ""

            Dim MissingVal As Double = -999
            Dim MissingAcc As Double = -998

            Dim lBufSiz As Integer = 100
            Dim lTSInd As Integer
            Dim lJDate As Double
            Dim lCurVal As Double
            Dim lYr As Integer
            Dim lMon As Integer
            Dim lDay As Integer
            Dim lHour As Integer
            Dim lTStep As Integer = 1
            Dim lStr As String
            Dim lStrArray() As String
            Dim lCons As String
            Dim i As Integer

            While curLine.Length = 0
                curLine = NextLine(inReader) 'get subbasin IDs from header record
            End While

            Try
                'lStr = ReplaceRepeats(curLine, " ").Trim 'remove extra blanks
                lTSAttributes = curLine.Split(pChrSep, StringSplitOptions.RemoveEmptyEntries)

                If (lTSAttributes.Count > 1) Then 'subbasins found

                    Do
                        curLine = NextLine(inReader) 'now read data records
                        'lStr = ReplaceRepeats(curLine, " ").Trim 'remove extra blanks
                        lStrArray = curLine.Split(pChrSep, StringSplitOptions.RemoveEmptyEntries)
                        lYr = lStrArray(0).Substring(0, 4)
                        lDay = lStrArray(0).Substring(4, 3)
                        lJDate = Jday(lYr, 1, 1, 0, 0, 0) + lDay - 1
                        For i = 1 To UBound(lStrArray)
                            lData = DataSets.ItemByKey(lTSAttributes(i))
                            If lData Is Nothing Then 'haven't encountered this constituent yet
                                lData = New atcTimeseries(Me)
                                lData.Dates = New atcTimeseries(Me)
                                lData.numValues = lBufSiz
                                lData.Value(0) = GetNaN()
                                lData.Dates.Value(0) = 0
                                lData.Attributes.SetValue("Count", 0)
                                lData.Attributes.SetValue("Scenario", "SIMULATED")
                                lCons = FilenameNoExt(FilenameNoPath(aFileName)).ToUpper
                                If lCons.Contains("STREAMFLOW") Then
                                    'lCons = "FLOW"
                                    lData.Attributes.SetValue("Units", "cms")
                                ElseIf lCons.Contains("BASINRUNOFFYIELD") Then
                                    lCons = "BASINRUNOFF"
                                ElseIf lCons.Contains("INTERFLOW") Then
                                    lCons = "INTERFLOW"
                                ElseIf lCons.Contains("BASEFLOW") Then
                                    lCons = "BASEFLOW"
                                End If
                                lData.Attributes.SetValue("Constituent", lCons)
                                lData.Attributes.SetValue("Location", lTSAttributes(i))
                                lData.Attributes.SetValue("Description", "GeoSFM Output Data")
                                lData.Attributes.SetValue("tu", 4)
                                lData.Attributes.SetValue("point", False)
                                lData.Attributes.SetValue("TSFILL", MissingVal)
                                lData.Attributes.AddHistory("Read From " & Specification)
                                DataSets.Add(lTSAttributes(i), lData)
                            End If
                            lTSInd = lData.Attributes.GetValue("Count")
                            If lTSInd >= lData.numValues Then 'expand buffer
                                lData.numValues = lTSInd * 2
                            End If
                            lTSInd += 1
                            lCurVal = CDbl(lStrArray(i))
                            If Not Double.IsNaN(lCurVal) Then
                                lData.Dates.Value(lTSInd) = lJDate
                                lData.Value(lTSInd) = lCurVal
                                lData.Attributes.SetValue("Count", lTSInd)
                            End If
                        Next

                    Loop
                    Open = True
                Else
                    Logger.Dbg("PROBLEM:  Test of format for file " & aFileName & " failed." & vbCrLf & _
                               "Be sure this file is in the accepted GeoSFM Output Data format.")
                    Open = False
                End If
            Catch endEx As EndOfStreamException
                Dim lDataSets As New atcTimeseriesGroup
                Dim lDataFilled As atcTimeseries
                Dim lInd As Integer = 0
                For Each lData In DataSets
                    lInd += 1
                    lData.Attributes.SetValue("ID", lInd)
                    lData.numValues = lData.Attributes.GetValue("Count")
                    If lData.numValues > 0 Then
                        lData.Dates.Value(0) = lData.Dates.Value(1) - JulianHour 'set 0th date to start of 1st interval
                        'lTStep = Math.Round((lData.Dates.Value(2) - lData.Dates.Value(1)) * 24)
                        lDataFilled = FillValues(lData, 4, lTStep, MissingVal, MissingVal, MissingAcc)
                        If Not lDataFilled Is Nothing Then
                            lDataFilled.Attributes.ChangeTo(lData.Attributes)
                            lDataFilled.ValuesNeedToBeRead = False
                            lDataFilled.Dates.ValuesNeedToBeRead = False
                            lDataSets.Add(lDataFilled)
                        End If
                    End If
                Next
                DataSets.Clear() 'get rid of initial "unfilled" data sets
                For Each lData In lDataSets
                    DataSets.Add(lData)
                Next
                Open = True
            Catch ex As Exception
                Logger.Dbg("PROBLEM processing the following record: " & vbCrLf & _
                           "     " & curLine & vbCrLf & "     " & ex.Message)
                Open = False
            End Try
            inReader.Close()
            inBuffer.Close()
            inStream.Close()
        End If

    End Function

    Public Overrides Sub ReadData(ByVal aReadMe As atcData.atcDataSet)
        If Not Me.DataSets.Contains(aReadMe) Then
            System.Diagnostics.Debug.WriteLine("Cannot read data: not from this file")
        Else

            'Should not ever get here since we are now reading all data in Open

            'pReadAll = True
            'Open(Me.FileName)
            'pReadAll = False
        End If
    End Sub

    Public Sub New()
        Filter = pFileFilter
    End Sub
End Class
