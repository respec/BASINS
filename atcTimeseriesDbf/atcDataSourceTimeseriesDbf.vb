Imports atcUtility
Imports atcData
Imports MapWinUtility
Imports MapWinUtility.Strings

Public Class atcDataSourceTimeseriesDbf
    Inherits atcTimeseriesSource
    '##MODULE_REMARKS Copyright 2007 AQUA TERRA Consultants - Royalty-free use permitted under open source license

    Private Shared pFilter As String = "DBF Files (*.dbf)|*.dbf"
    Private pColDefs As Hashtable

    Public Overrides ReadOnly Property Description() As String
        Get
            Return "Timeseries DBF"
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

        If MyBase.Open(aFileName, aAttributes) Then
            Dim lDateCol As Integer = -1
            Dim lTimeCol As Integer = -1
            Dim lLocnCol As Integer = -1
            Dim lValueCol As Integer
            Dim lConstituents As New atcCollection
            Dim lTSKey As String
            Dim lTSIndex As Integer
            Dim lLocation As String = ""
            Dim lConstituentName, lConstituentUnits, lValue As String
            Dim lStr As String

            Dim lDate As Double
            Dim lDateFormat As New atcDateFormat
            lDateFormat.IncludeHours = True
            lDateFormat.IncludeMinutes = True

            Dim lDBF As IatcTable
            lDBF = New atcTableDBF
            lDBF.OpenFile(Specification)
            Logger.Dbg("NumFields:" & lDBF.NumFields)
            Logger.Dbg("NumRecords:" & lDBF.NumRecords)

            Try
                For lColumn As Integer = 1 To lDBF.NumFields
                    lStr = UCase(lDBF.FieldName(lColumn))
                    Select Case lStr
                        Case "DATE"
                            lDateCol = lColumn
                            Logger.Dbg("DateColumn:" & lColumn)
                        Case "TIME"
                            lTimeCol = lColumn
                            Logger.Dbg("TimeColumn:" & lColumn)
                        Case "ID", "STREAM", "FAC_NAME" 'location
                            If lLocnCol = -1 Then 'only use first one
                                'should be sure that field is in use here
                                lLocnCol = lColumn
                                Logger.Dbg("IdColumn:" & lColumn)
                            End If
                        Case "SAMPLE", "PARM"
                            'SKIP
                        Case Else
                            lConstituents.Add(lColumn, lStr)
                            Logger.Dbg("ConstituentColumn:" & lColumn & " Name:" & lStr)
                    End Select
                Next

                If lDateCol > 0 AndAlso lTimeCol > 0 AndAlso lLocnCol > 0 Then
                    For lRecordNumber As Integer = 1 To lDBF.NumRecords
                        lDBF.CurrentRecord = lRecordNumber
                        lLocation = lDBF.Value(lLocnCol)
                        For lConstituentIndex As Integer = 0 To lConstituents.Count - 1
                            lValueCol = lConstituents.Keys(lConstituentIndex)
                            lStr = lDBF.Value(lValueCol)
                            If lStr.Length > 0 Then
                                lConstituentUnits = lConstituents.Item(lConstituentIndex)
                                lConstituentUnits = lConstituentUnits.Replace(")", "")
                                lConstituentName = StrSplit(lConstituentUnits, "_(", "'")
                                lTSKey = lLocation & ":" & lConstituentName
                                lData = DataSets.ItemByKey(lTSKey)
                                If lData Is Nothing Then 'create new timseries dataset
                                    lData = New atcTimeseries(Me)
                                    lData.Dates = New atcTimeseries(Me)
                                    lData.numValues = lDBF.NumRecords - lDBF.CurrentRecord + 1
                                    lData.Value(0) = GetNaN()
                                    lData.Dates.Value(0) = GetNaN()
                                    lData.Attributes.SetValue("ID", DataSets.Count + 1)
                                    lData.Attributes.SetValue("Count", 0)
                                    lData.Attributes.SetValue("Scenario", "OBSERVED")
                                    lData.Attributes.SetValue("Location", lLocation)
                                    lData.Attributes.SetValue("Constituent", lConstituentName)
                                    lData.Attributes.SetValue("Units", lConstituentUnits)
                                    lData.Attributes.SetValue("Point", True)
                                    lData.Attributes.AddHistory("Read from " & Specification)
                                    DataSets.Add(lTSKey, lData)
                                End If
                                lTSIndex = lData.Attributes.GetValue("Count") + 1
                                lValue = lDBF.Value(lValueCol)
                                lDate = parseDate(lDBF.Value(lDateCol), lDBF.Value(lTimeCol))
                                If lValue.IndexOf("<") >= 0 Then
                                    'Logger.Dbg("RemoveLessThan " & lTSKey & " " & lDateFormat.JDateToString(lDate) & " '" & lValue & "'")
                                    lValue = lValue.Replace("<", "")
                                    lData.ValueAttributes(lTSIndex).SetValueIfMissing("Conditional", "<")
                                End If
                                If IsNumeric(lValue) Then
                                    lData.Value(lTSIndex) = lValue
                                    lData.Dates.Value(lTSIndex) = lDate
                                    lData.Attributes.SetValue("Count", lTSIndex)
                                Else
                                    'TODO: handle other numeric values somehow!
                                    Logger.Dbg("NonNumericValue " & lTSKey & " " & lDateFormat.JDateToString(lDate) & " '" & lValue & "'")
                                    lData.Value(lTSIndex) = GetNaN()
                                    lData.Dates.Value(lTSIndex) = lDate
                                    lData.Attributes.SetValue("Count", lTSIndex)
                                End If
                            End If
                        Next
                    Next lRecordNumber
                    For Each lData In DataSets
                        lData.numValues = lData.Attributes.GetValue("Count")
                    Next
                    Open = True
                ElseIf lDateCol < 0 Then
                    Open = False
                    Logger.Msg("Unable to identify Date column in DBF file " & Specification, "DBF Open")
                ElseIf lTimeCol < 0 Then
                    Open = False
                    Logger.Msg("Unable to identify Time column in DBF file " & Specification, "DBF Open")
                ElseIf lLocnCol < 0 Then
                    Open = False
                    Logger.Msg("Unable to identify ID column in DBF file " & Specification, "DBF Open")
                End If
            Catch endEx As Exception
                Open = False
            End Try
        End If
    End Function

    Private Function parseDate(ByVal aDate As String, ByVal aTime As String) As Double
        Dim lDate As Date
        Dim lHour As Integer, lMinute As Integer
        Dim lTime As String

        lTime = aTime
        If lTime.Length >= 8 Then
            'todo: handle more gracefully something that is not a time
            lTime = "24:00"
        ElseIf lTime.Length = 0 Then
            lTime = "0:0"
        ElseIf lTime.IndexOf(":") = -1 Then
            lTime = lTime.PadLeft(4, "0")
            lTime = lTime.Insert(2, ":")
        End If
        lHour = StrSplit(lTime, ":", "'")
        lMinute = lTime
        lDate = Date.Parse(aDate)
        lDate = lDate.AddHours(lHour)
        lDate = lDate.AddMinutes(lMinute)
        Return lDate.ToOADate
    End Function

    Public Sub New()
        Filter = pFilter
    End Sub
End Class
