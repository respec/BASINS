Imports atcUtility
Imports atcData
Imports MapWinUtility

Public Class atcDataSourceTimeseriesWRDB
    Inherits atcDataSource
    '##MODULE_REMARKS Copyright 2007 AQUA TERRA Consultants - Royalty-free use permitted under open source license

    Private Shared pFilter As String = "WRDB Archive Files (*.txt)|*.txt|All Files|*.*"
    Private pColDefs As Hashtable

    Public Overrides ReadOnly Property Description() As String
        Get
            Return "WRDB Archive"
        End Get
    End Property

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Timeseries::WRDB Archive"
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
            Dim lDateCol As Integer = -1
            Dim lLocnCol As Integer = -1
            Dim lConsCol As Integer = -1
            Dim lCcodeCol As Integer = -1
            Dim lRcodeCol As Integer = -1
            Dim lValueCol As Integer
            Dim lLocation As String = ""
            Dim lConstituentName, lCcode, lValue As String
            Dim lStr As String
            Dim lDate As Double
            Dim lTable As atcTableDelimited
            lTable = New atcTableDelimited
            lTable.Delimiter = vbTab
            Logger.Status("Opening '" & Specification & "'", True)
            lTable.OpenFile(Specification)
            Logger.Dbg("NumFields:" & lTable.NumFields)
            Logger.Dbg("NumRecords:" & lTable.NumRecords)

            Try
                For lColumn As Integer = 1 To lTable.NumFields
                    lStr = UCase(lTable.FieldName(lColumn))
                    Select Case lStr
                        Case "DATE_TIME" : lDateCol = lColumn : Logger.Dbg("DateColumn:" & lColumn)
                        Case "STATION_ID" : lLocnCol = lColumn : Logger.Dbg("IdColumn:" & lColumn)
                        Case "CCODE" : lCcodeCol = lColumn : Logger.Dbg("CcodeCol:" & lColumn)
                        Case "RCODE" : lRcodeCol = lColumn : Logger.Dbg("RcodeCol:" & lColumn)
                        Case "PCODE" : lConsCol = lColumn : Logger.Dbg("ConsColumn:" & lColumn)
                        Case "RESULT" : lValueCol = lColumn : Logger.Dbg("ValueCol:" & lColumn)
                    End Select
                Next

                If lDateCol > 0 AndAlso lValueCol > 0 AndAlso lLocnCol > 0 Then
                    Dim lGroupBuilder As New atcTimeseriesGroupBuilder(Me)
                    Dim lTSBuilder As atcTimeseriesBuilder
                    For lRecordNumber As Integer = 1 To lTable.NumRecords
                        lTable.CurrentRecord = lRecordNumber
                        lLocation = lTable.Value(lLocnCol)
                        lStr = lTable.Value(lValueCol)
                        If lStr.Length > 0 Then
                            lConstituentName = lTable.Value(lConsCol)
                            lCcode = lTable.Value(lCcodeCol)
                            lTSBuilder = lGroupBuilder.Builder(lLocation & ":" & lConstituentName & ":" & lCcode)
                            With lTSBuilder.Attributes
                                'Set attributes of newly created builder
                                If Not .ContainsAttribute("Location") Then
                                    .SetValue("Scenario", "OBSERVED")
                                    .SetValue("Location", lLocation)
                                    .SetValue("Constituent", lConstituentName)
                                    .SetValue("CCode", lCcode)
                                    .SetValue("Point", True)
                                    .AddHistory("Read from " & Specification)
                                End If
                            End With
                            lValue = lTable.Value(lValueCol)
                            lDate = Date.Parse(lTable.Value(lDateCol)).ToOADate
                            lTSBuilder.AddValue(lDate, lValue)
                            If lTable.Value(lRcodeCol).Length > 0 Then
                                lTSBuilder.AddValueAttribute("RCode", lTable.Value(lRcodeCol))
                            End If
                        End If
                        Logger.Progress(lRecordNumber, lTable.NumRecords)
                    Next lRecordNumber
                    lGroupBuilder.CreateTimeseriesAddToGroup(DataSets)
                    Open = True
                ElseIf lDateCol < 0 Then
                    Open = False
                    Logger.Msg("Unable to identify Date column in " & Specification, "WRDB Open")
                ElseIf lLocnCol < 0 Then
                    Open = False
                    Logger.Msg("Unable to identify ID column in " & Specification, "WRDB Open")
                End If
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
