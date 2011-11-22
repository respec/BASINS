Imports atcUtility
Imports atcData
Imports MapWinUtility
Imports System.IO

Public Class atcTimeseriesUEBOutput
    Inherits atcTimeseriesSource
    '##MODULE_REMARKS Copyright 2010 AQUA TERRA Consultants - Royalty-free use permitted under open source license

    Private Shared pFileFilter As String = "UEB Output Files (*.*)|*.*"
    Private pErrorDescription As String
    Private pChrSep() As String = {" ", vbTab, vbCrLf}

    Public Overrides ReadOnly Property Description() As String
        Get
            Return "UEB Model Output Data"
        End Get
    End Property

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Timeseries::UEB Output"
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
        Dim lTSAttributes() As String = {"", "", "", "", "Atmos TF", "HRI", "ATemp", "Precip Rate", "Wind Speed", "RelH", _
                                        "Solar Rad", "Longwave Rad", "Zenith Angle Cos", "Snow Energy", "Snow Water Eq", _
                                        "Snow Albedo", "Prec-Rain", "Prec-Snow", "Albedo", "Sens Heat Flux", "Laten Heat Flux", _
                                        "Sublimation", "Melt Outflow", "Melt Energy", "Snow Energy Flux", "Snow Mass Flux", _
                                        "Snow Ave Temp", "Snow Surf Temp", "Cum Precip", "Cum Sublimation", "Cum Melt Outflow", _
                                         "Net Radiation", "Mass Error"}

        If aFileName Is Nothing OrElse aFileName.Length = 0 OrElse Not FileExists(aFileName) Then
            aFileName = FindFile("Select " & Name & " file to open", , , pFileFilter, True, , 1)
        End If

        If Not FileExists(aFileName) Then
            pErrorDescription = "File '" & aFileName & "' not found"
        Else
            Me.Specification = aFileName
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
            Dim lStr As String
            Dim lStrArray() As String
            Dim i As Integer
            Dim lTSPos As Integer
            Dim lDPos As Integer

            'do 1st line read and column population to check format
            curLine = NextLine(inReader) 'read line to examine format

            Try
                lStr = ReplaceRepeats(curLine, " ") 'remove extra blanks
                lStrArray = lStr.Split(pChrSep, StringSplitOptions.None)

                If (IsInteger(lStrArray(0)) AndAlso _
                   IsInteger(lStrArray(1)) AndAlso _
                   IsInteger(lStrArray(2))) Then 'first 3 elements of first line appear to be yy-mm-dd
                    Dim lCentury As Integer
                    lYr = lStrArray(0)
                    If lYr > 50 Then 'assume run starts before 2000
                        lCentury = 1900
                    Else
                        lCentury = 2000
                    End If
                    lYr += lCentury
                    lMon = lStrArray(1)
                    lDay = lStrArray(2)
                    lHour = lStrArray(3)
                    lTSPos = 0
                    lDPos = 4

                    Do
                        lJDate = Jday(lYr, lMon, lDay, lHour, 0, 0)
                        For i = lDPos To UBound(lStrArray)
                            lData = DataSets.ItemByKey(lTSAttributes(lTSPos))
                            If lData Is Nothing Then 'haven't encountered this constituent yet
                                lData = New atcTimeseries(Me)
                                lData.Dates = New atcTimeseries(Me)
                                lData.numValues = lBufSiz
                                lData.Value(0) = GetNaN()
                                lData.Dates.Value(0) = 0
                                lData.Attributes.SetValue("Count", 0)
                                lData.Attributes.SetValue("Scenario", "SIMULATED")
                                lData.Attributes.SetValue("Location", FilenameNoExt(FilenameNoPath(aFileName)))
                                lData.Attributes.SetValue("Constituent", lTSAttributes(lTSPos))
                                lData.Attributes.SetValue("Description", "UEB Output Data")
                                lData.Attributes.SetValue("tu", 3)
                                lData.Attributes.SetValue("ts", 1)
                                lData.Attributes.SetValue("point", False)
                                lData.Attributes.SetValue("TSFILL", MissingVal)
                                DataSets.Add(lTSAttributes(lTSPos), lData)
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
                            lTSPos += 1
                        Next

                        curLine = NextLine(inReader)
                        lStrArray = lStr.Split(pChrSep, StringSplitOptions.None)
                        If (IsInteger(lStrArray(0)) AndAlso _
                           IsInteger(lStrArray(1)) AndAlso _
                           IsInteger(lStrArray(2))) Then 'first 3 elements of first line appear to be yy-mm-dd
                            lDPos = 4
                            lTSPos = 0
                            lYr += lStrArray(0) + lCentury
                            lMon = lStrArray(1)
                            lDay = lStrArray(2)
                            lHour = lStrArray(3)
                        Else 'continuation of outputs for same date
                            lDPos = 0
                        End If

                    Loop
                    Open = True
                Else
                    Logger.Dbg("PROBLEM:  Test of format for file " & aFileName & " failed." & vbCrLf & _
                               "Be sure this file is in the accepted UEB Output Data format.")
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
                        lDataFilled = FillValues(lData, 3, 1, MissingVal, MissingVal, MissingAcc)
                        If Not lDataFilled Is Nothing Then
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
