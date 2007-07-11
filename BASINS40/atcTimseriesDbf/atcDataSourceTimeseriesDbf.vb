Imports atcUtility
Imports atcData
Imports MapWinUtility

Public Class atcDataSourceTimeseriesDbf
    Inherits atcDataSource
    '##MODULE_REMARKS Copyright 2007 AQUA TERRA Consultants - Royalty-free use permitted under open source license

    Private Shared pFileFilter As String = "DBF Files (*.dbf)|*.dbf"
    Private pErrorDescription As String
    Private pColDefs As Hashtable

    Public Overrides ReadOnly Property Description() As String
        Get
            Return "Timseries DBF"
        End Get
    End Property

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Timeseries::DBF"
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

        If aFileName Is Nothing OrElse aFileName.Length = 0 OrElse Not FileExists(aFileName) Then
            aFileName = FindFile("Select " & Name & " file to open", , , pFileFilter, True, , 1)
        End If

        If Not FileExists(aFileName) Then
            pErrorDescription = "File '" & aFileName & "' not found"
        Else
            Me.Specification = aFileName

            Try
                Logger.Dbg("Process:" & aFileName)
                Dim lDateCol As Integer = -1
                Dim lTimeCol As Integer = -1
                Dim lLocnCol As Integer = -1
                Dim lCons As New atcCollection
                Dim lTSKey As String
                Dim lTSIndex As Integer
                Dim lLocation As String = ""
                Dim lConstituentCode As Integer = -1
                Dim lStr As String
                Dim lDBF As IatcTable
                lDBF = New atcTableDBF
                lDBF.OpenFile(aFileName)
                Logger.Dbg("NumFields:" & lDBF.NumFields)
                Logger.Dbg("NumRecords:" & lDBF.NumRecords)

                For lColumn As Integer = 1 To lDBF.NumFields
                    lStr = UCase(lDBF.FieldName(lColumn))
                    If lStr = "DATE" Then
                        lDateCol = lColumn
                        Logger.Dbg("DateColumn:" & lColumn)
                    ElseIf lStr = "TIME" Then
                        lTimeCol = lColumn
                        Logger.Dbg("TimeColumn:" & lColumn)
                    ElseIf InStr(lStr, "ID") Then 'location
                        If lLocnCol = -1 Then 'only use first one
                            'should be sure that field is in use here
                            lLocnCol = lColumn
                            Logger.Dbg("IdColumn:" & lColumn)
                        End If
                    ElseIf lStr <> "SAMPLE" Then
                        lCons.Add(lColumn, lStr)
                        Logger.Dbg("ConstituentColumn:" & lColumn & " Name:" & lStr)
                    End If
                Next

                If lDateCol > 0 AndAlso lTimeCol > 0 AndAlso lLocnCol > 0 Then
                    While Not lDBF.atEOF
                        lLocation = lDBF.Value(lLocnCol)
                        'lConstituentCode = lDBF.Value(lConsCol)
                        lTSKey = lLocation & ":" & lConstituentCode
                        lData = DataSets.ItemByKey(lTSKey)
                        If lData Is Nothing Then
                            lData = New atcTimeseries(Me)
                            lData.Dates = New atcTimeseries(Me)
                            lData.numValues = lDBF.NumRecords - lDBF.CurrentRecord + 1
                            lData.Value(0) = Double.NaN
                            lData.Dates.Value(0) = Double.NaN
                            lData.Attributes.SetValue("Count", 0)
                            lData.Attributes.SetValue("Scenario", "OBSERVED")
                            lData.Attributes.SetValue("Location", lLocation)
                            lData.Attributes.SetValue("Constituent", lConstituentCode)
                            lData.Attributes.SetValue("Point", True)
                            DataSets.Add(lTSKey, lData)
                        End If
                        lTSIndex = lData.Attributes.GetValue("Count") + 1
                        'lData.Value(lTSIndex) = lDBF.Value(lValCol)
                        lData.Dates.Value(lTSIndex) = parseDate(lDBF.Value(lDateCol), lDBF.Value(lTimeCol))
                        lData.Attributes.SetValue("Count", lTSIndex)
                        lDBF.MoveNext()
                    End While
                    For Each lData In DataSets
                        lData.numValues = lData.Attributes.GetValue("Count")
                    Next
                    Open = True
                ElseIf lDateCol < 0 Then
                    Open = False
                    Logger.Msg("Unable to identify Date column in DBF file " & aFileName, "DBF Open")
                ElseIf lTimeCol < 0 Then
                    Open = False
                    Logger.Msg("Unable to identify Time column in DBF file " & aFileName, "DBF Open")
                ElseIf lLocnCol < 0 Then
                    Open = False
                    Logger.Msg("Unable to identify ID column in DBF file " & aFileName, "DBF Open")
                End If
            Catch endEx As Exception
                Open = False
            End Try
        End If
    End Function
    Private Function parseDate(ByVal aDate As String, ByVal aTime As String) As Double
        'assume point values at specified time
        Dim d(5) As Integer 'date array
        Dim l As Integer 'Length of year (2 or 4 digit year)
        Dim i As Integer 'Year offset (1900 for 2-digit year)

        If Not IsNumeric(aTime) Then aTime = "1200" 'assume noon for missing obstime
        If IsNumeric(aDate) Then
            If Len(aDate) = 8 Then ' 4 dig yr
                l = 4
                i = 0
            Else
                l = 2
                i = 1900
            End If
            d(0) = Left(aDate, l) + i
            d(1) = Mid(aDate, l + 1, 2)
            d(2) = Right(aDate, 2)
            If IsNumeric(aTime) Then
                d(3) = Left(aTime, 2)
                d(4) = Right(aTime, 2)
            End If
            Return Date2J(d)
        Else
            Return 0
        End If
    End Function
End Class
