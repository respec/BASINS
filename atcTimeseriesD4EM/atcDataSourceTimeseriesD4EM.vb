Imports atcUtility
Imports atcData
Imports MapWinUtility

Public Class atcDataSourceTimeseriesD4EM
    Inherits atcDataSource
    '##MODULE_REMARKS Copyright 2008 AQUA TERRA Consultants - Royalty-free use permitted under open source license

    Private Shared pFilter As String = "D4EM Text Files (*.txt)|*.txt"
    Private Shared pVersion As String = "#D4EM Timeseries"

    Public Overrides ReadOnly Property Description() As String
        Get
            Return "Timeseries D4EM"
        End Get
    End Property

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Timeseries::D4EM"
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

    Public Overrides ReadOnly Property CanSave() As Boolean
        Get
            Return True
        End Get
    End Property

    Public Overrides Function Open(ByVal aFileName As String, Optional ByVal aAttributes As atcData.atcDataAttributes = Nothing) As Boolean
        If Not MyBase.Open(aFileName, aAttributes) Then
            Return False
        Else
            If IO.File.Exists(Specification) Then
                Logger.Status("Reading " & IO.Path.GetFileName(Specification), True)
                Dim lFileStream As New IO.FileStream(Specification, IO.FileMode.Open, IO.FileAccess.Read)
                Dim lReader As New IO.StreamReader(lFileStream)
                Dim lVersion As String = lReader.ReadLine
                If lVersion <> pVersion Then
                    Logger.Dbg("Unknown D4EM file version: " & lVersion & "<>" & pVersion)
                    lReader.Close()
                    Return False
                Else
                    Try
                        Do
                            Dim lData As New atcTimeseries(Me)
                            Dim lAttributeName As String
                            Dim lAttributeValue As String
                            Dim lAttributeType As String
                            Do
                                lAttributeName = lReader.ReadLine
                                If lAttributeName = "<done>" Then
                                    Exit Do
                                End If
                                lAttributeType = lReader.ReadLine
                                lAttributeValue = lReader.ReadLine
                                Select Case lAttributeType(0)
                                    Case "I" : lData.Attributes.SetValue(lAttributeName, CInt(lAttributeValue))
                                    Case "D" : lData.Attributes.SetValue(lAttributeName, CDbl(lAttributeValue))
                                    Case Else
                                        lData.Attributes.SetValue(lAttributeName, lAttributeValue)
                                End Select
                            Loop

                            Dim lNumDates As Integer = lData.Attributes.GetValue("NumValues")
                            Dim lDateStartDate As Date = Date.Parse(lData.Attributes.GetValue("Start Date"))
                            Dim lTimeUnits As Object = lData.Attributes.GetValue("tu", atcTimeUnit.TUDay)
                            Dim lTU As atcUtility.atcTimeUnit
                            If lTimeUnits.GetType.IsInstanceOfType("") Then
                                lTU = System.Enum.Parse(lTU.GetType, lTimeUnits)
                                lData.Attributes.SetValue("tu", lTU)
                            Else
                                lTU = lTimeUnits
                            End If
                            Dim lTimeStep = lData.Attributes.GetValue("ts", 1)

                            Dim lDateStartJulian As Double = lDateStartDate.ToOADate
                            Dim lDates(Math.Abs(lNumDates)) As Double
                            lDates(0) = lDateStartJulian
                            For lIndex As Integer = 1 To lNumDates
                                lDates(lIndex) = TimAddJ(lDateStartJulian, lTU, lTimeStep, lIndex)
                            Next
                            lData.Dates = New atcTimeseries(Me)
                            lData.Dates.Values = lDates

                            Dim lValues(lNumDates) As Double
                            For lIndex As Integer = 0 To lNumDates
                                lValues(lIndex) = CDbl(lReader.ReadLine.Substring(10).Trim)
                            Next
                            lData.Values = lValues
                            DataSets.Add(lData)
                            Logger.Progress(lFileStream.Position, lFileStream.Length)
                        Loop
                    Catch ex As IO.EndOfStreamException
                        Logger.Status("")
                        Logger.Dbg("Read " & DataSets.Count & " from " & Specification)
                    End Try
                    lReader.Close()
                End If
            End If
        End If
        Return True
    End Function

    Public Overrides Function AddDatasets(ByVal aDataGroup As atcDataGroup) As Boolean
        Logger.Status("Writing " & IO.Path.GetFileName(Specification), True)
        Dim lIndex As Integer = 0
        For Each lDataSet As atcData.atcDataSet In aDataGroup
            AddDataset(lDataSet)
            lIndex += 1
            Logger.Progress(lIndex, aDataGroup.Count)
        Next
        Logger.Status("")
        Logger.Dbg("Wrote " & aDataGroup.Count & " Datasets")
    End Function

    Public Overrides Function AddDataset(ByVal aDataSet As atcData.atcDataSet, _
                                Optional ByVal aExistAction As atcData.atcDataSource.EnumExistAction = atcData.atcDataSource.EnumExistAction.ExistReplace) _
                                         As Boolean
        Dim lFileStream As New IO.FileStream(Specification, IO.FileMode.Append)
        Dim lWriter As New IO.StreamWriter(lFileStream)
        If lFileStream.Position = 0 Then
            lWriter.Write(pVersion)
        End If

        For Each lAttribute As atcDefinedValue In aDataSet.Attributes
            If Not lAttribute.Definition.Calculated Then
                Dim lName As String = lAttribute.Definition.Name
                Select Case lName
                    Case "Key", "Data Source"
                    Case Else
                        Dim lType As String = ""
                        Select Case lAttribute.Definition.TypeString
                            Case "String" : lType = "Str"
                            Case "Integer" : lType = "Int"
                            Case "Double" : lType = "Dbl"
                            Case "atcTimeUnit" : lType = "Str"
                            Case Else
                                Logger.Dbg("AttributeTypeNotDefined:" & lAttribute.Definition.TypeString)
                        End Select
                        If lType.Length > 0 Then
                            lWriter.WriteLine(lName)
                            lWriter.WriteLine(lType)
                            lWriter.WriteLine(lAttribute.Value.ToString.TrimEnd)
                        End If
                End Select
            End If
        Next

        Dim lTimeseries As atcTimeseries = aDataSet

        lWriter.WriteLine("NumValues")
        lWriter.WriteLine("Int")
        lWriter.WriteLine(lTimeseries.numValues)

        lWriter.WriteLine("Start Date")
        lWriter.WriteLine("Str")
        Dim lDateStartDate As Date = Date.FromOADate(lTimeseries.Dates.Value(0))
        lWriter.WriteLine(lTimeseries.numValues)

        lWriter.Write("<done>")

        Dim lTimeUnits As Integer = lTimeseries.Attributes.GetValue("tu", 4)
        Dim lTimeStep As Integer = lTimeseries.Attributes.GetValue("ts", 1)
        Dim lDateEndComputed As Double = TimAddJ(lTimeseries.Dates.Value(0), lTimeUnits, lTimeStep, lTimeseries.numValues)
        If Math.Abs(lDateEndComputed - lTimeseries.Dates.Value(lTimeseries.numValues)) > 0.00001 Then
            Logger.Dbg("Greater than expected difference between actual (" & Date.FromOADate(lTimeseries.Dates.Value(lTimeseries.numValues)).ToShortDateString _
                     & ") and computed (" & Date.FromOADate(lDateEndComputed).ToShortDateString & ") end dates")
        End If
        Dim lValues() As Double = lTimeseries.Values
        For lIndex As Integer = 0 To lTimeseries.numValues
            lWriter.WriteLine(((lIndex + 1) & " ").PadLeft(10) & DecimalAlign(lValues(lIndex), 15, 6))
        Next
        'todo: write value attributes (if any)

        lWriter.Close()
    End Function

    Public Overrides Function Save(ByVal SaveFileName As String, _
                          Optional ByVal ExistAction As EnumExistAction = EnumExistAction.ExistReplace) As Boolean
        If SaveFileName.ToLower.Equals(Me.Specification.ToLower) Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Sub New()
        Filter = pFilter
    End Sub
End Class
