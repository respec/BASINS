Option Strict Off
Option Explicit On

Imports atcData
Imports atcUtility
Imports atcUCI
Imports MapWinUtility
Imports System.Xml

''' <summary>
''' 
''' </summary>
''' <remarks>
'''Copyright 2005-2008 AQUA TERRA Consultants - Royalty-free use permitted under open source license
''' </remarks>
Public Class atcTimeseriesFileHspfBinOut
    Inherits atcData.atcTimeseriesSource

    Private pFilter As String = "HSPF Binary Output Files (*.hbn)|*.hbn"
    Private pName As String = "Timeseries::HSPF Binary Output"
    Private Shared pNaN As Double = GetNaN()
    Private Const pUnknownUnits As String = "<unknown>"

    Private pBinFile As HspfBinary

    Private pUnitsEnglish As New Generic.Dictionary(Of String, String)
    Private pUnitsTable As atcTable
    Private pUnitsTableModified As Boolean = False
    Private Shared pUnitsTableTemplate As atcTable
    Private pCountUnitsFound As Integer
    Private pCountUnitsMissing As Integer
    Private pCountUnitsHardCode As Integer
    Private pUnitsMissing As atcCollection
    Private Shared pHspfMsgUnits As Generic.Dictionary(Of String, String)
    Public DebugLevel As Integer = 1

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
            Return "HSPF Binary Output"
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
            Return "HSPFBinary"
        End Get
    End Property

    Private Sub BuildTSers()
        pCountUnitsFound = 0
        pCountUnitsMissing = 0
        pCountUnitsHardCode = 0
        pUnitsMissing = New atcCollection

        pBinFile = New HspfBinary
        pBinFile.Filename = Specification

        Try
            Dim lNumBinHeaders As Integer = pBinFile.Headers.Count
            If DebugLevel > 0 Then
                Logger.Dbg(MemUsage)
                Logger.Dbg("Parse " & lNumBinHeaders & " Headers")
            End If
            Dim lFileAttributes As New atcDataAttributes
            With lFileAttributes
                .SetValue("CIntvl", True)
                .SetValue("History 1", "Read from " & pBinFile.Filename)
                Dim lFileDetails As System.IO.FileInfo = New System.IO.FileInfo(pBinFile.Filename)
                .SetValue("Date Created", lFileDetails.CreationTime)
                .SetValue("Date Modified", lFileDetails.LastWriteTime)
                .SetValue("IDSCEN", IO.Path.GetFileNameWithoutExtension(Specification))
            End With
            Dim lHeaderIndex As Integer = 0
            'Dim lSJDate As Double, lEJDate As Double, lOutLev As Integer

            'Dim lCountVariables As New atcCollection
            'Dim lNumVariables As Integer = 0
            'Dim lCountData As Integer = 0
            'For Each lBinHeader As HspfBinaryHeader In pBinFile.Headers
            '    lNumVariables += lBinHeader.VarNames.Count
            '    For Each lConstituent As String In lBinHeader.VarNames
            '        lCountVariables.Increment(lConstituent)
            '    Next
            '    lCountData += lBinHeader.Data.Count
            'Next
            'Logger.Dbg(lCountData & " total Header.Data")
            'Logger.Dbg(lNumVariables & " total Variables")
            'For Each lVariable As String In lCountVariables.Keys
            '    Logger.Dbg(lCountVariables.ItemByKey(lVariable) & "  " & lVariable)
            'Next

            For Each lBinHeader As HspfBinaryHeader In pBinFile.Headers
                With lBinHeader
                    Dim lFirstBinDatum As HspfBinaryDatum = .Data.Item(0)
                    Dim lDates As New atcTimeseries(Me)
                    lDates.Attributes.SetValue("Shared", True)

                    'lSJDate = lData.JDate
                    'lOutLev = lData.OutLev
                    'Dim lDataIndex As Integer
                    'If .Data.Count = 1 Then
                    '    lDataIndex = 1 'force daily
                    'Else
                    '    lDataIndex = 1
                    '    While lOutLev <> .Data.Item(lDataIndex).OutLev And lHeaderIndex < .Data.Count 'looking for same outlev
                    '        lDataIndex += 1
                    '    End While
                    'End If
                    'If lDataIndex < .Data.Count Then
                    '    lData = .Data.Item(lDataIndex)
                    '    lEJDate = lData.JDate
                    'Else 'only one value dont know what interval is, assume day
                    '    lEJDate = lSJDate + 1
                    'End If

                    'attributes related to dates common to all in ts in a header 
                    'Dim lTu As Integer, lTs As Integer, lIntvl As Integer
                    Dim lBaseAttributes As atcDataAttributes = New atcDataAttributes
                    With lBaseAttributes
                        'If lEJDate - lSJDate >= 1 Then 'daily or longer interval
                        '    lTs = 1
                        '    lTu = lFirstBinDatum.OutLev + 1
                        '    lIntvl = 1
                        '    'If lTu = ATCTimeUnit.TUDay Then
                        '    'Else 'undefined for monthly or annual
                        '    'End If
                        'Else 'use minute
                        '    lTu = atcTimeUnit.TUMinute
                        '    lTs = timdifJ(lSJDate, lEJDate, atcTimeUnit.TUMinute, 1)
                        '    lIntvl = lTs / 1440
                        'End If
                        'If lTs < 1 Then
                        '    Logger.Dbg("TimestepProblem:" & lSJDate & ":" & lEJDate & ":" & lTu & ":" & lTs & ":" & _
                        '               lBinHeader.Id.OperationName & ":" & _
                        '               lBinHeader.Id.OperationNumber & ":" & _
                        '               lBinHeader.Id.SectionName)
                        '    lTs = 1
                        'End If
                        '.SetValue("Ts", lTs)
                        '.SetValue("Tu", lTu)
                        '.SetValue("Intvl", lIntvl)
                        'lSJDate = TimAddJ(lBinHeader.Data.Item(0).JDate, lTu, lTs, -lIntvl)
                        .ChangeTo(lFileAttributes)
                        .SetValue("SJDay", lFirstBinDatum.JDate) 'Is end of first interval correct for SJDay?
                        .SetValue("EJDay", lBinHeader.Data.Item(lBinHeader.Data.Count - 1).JDate)
                        .SetValue("Operation", lBinHeader.Id.OperationName)
                        .SetValue("Section", lBinHeader.Id.SectionName)
                        .SetValue("IDLOCN", Left(lBinHeader.Id.OperationName, 1) & ":" & (lBinHeader.Id.OperationNumber))
                    End With
                    For Each lConstituent As String In lBinHeader.VarNames
                        Dim lTSer As atcTimeseries = New atcTimeseries(Me)
                        lTSer.Attributes.SetValue("SharedAttributes", lBaseAttributes)
                        With lTSer
                            .Attributes.SetValue("IDCONS", lConstituent)
                            .Attributes.SetValue("UNITS", GetUnits(lConstituent, pBinFile.UnitSystem))
                            .Attributes.SetValue("ID", Me.DataSets.Count)
                            If lConstituent = "LZS" Then 'TODO: need better check here
                                .Attributes.SetValue("Point", True)
                            End If
                            .ValuesNeedToBeRead = True
                            '.SetInterval(lTu, lTs)
                            .Dates = lDates
                            AddDataSet(lTSer)
                        End With
                    Next
                End With
                lHeaderIndex += 1
                'Logger.Dbg("Loop " & i)
                Logger.Progress(lNumBinHeaders / 2 + lHeaderIndex / 2, lNumBinHeaders)
            Next
        Catch ex As ApplicationException
            Logger.Dbg(ex.Message)
        End Try
        If DebugLevel > 0 Then
            Logger.Dbg("Created " & DataSets.Count & " Datasets")
        End If

        If pUnitsTableModified Then
            With pUnitsTable
                Try
                    Logger.Dbg("SaveLocalUnitsTable:" & .FileName)
                    .WriteFile(.FileName)
                Catch exWriteUnits As Exception
                    Logger.Dbg("Could not save units table: " & exWriteUnits.Message)
                End Try
            End With
        End If
        Logger.Dbg("Units Assigned: " & pCountUnitsFound & ", Found in local dbf: " & pCountUnitsHardCode)
        Logger.Dbg("Units Missing: Unique: " & pUnitsMissing.Count & ", Total: " & pCountUnitsMissing)
        If DebugLevel > 1 Then
            For lIndex As Integer = 0 To pUnitsMissing.Count - 1
                Logger.Dbg("Missing " & pUnitsMissing.Keys(lIndex) & " " & pUnitsMissing.Item(lIndex))
            Next
        End If
    End Sub

    Private Function GetUnits(ByVal aConstituent As String, Optional ByVal aUnitSystem As atcUnitSystem = atcUnitSystem.atcEnglish) As String
        Dim lUnits As String = ""
        Dim lUnknownFlag As Boolean = False

        Dim lUnitsKey As String = aConstituent.ToLower
        If pUnitsEnglish.ContainsKey(lUnitsKey) Then
            Return pUnitsEnglish.Item(lUnitsKey)
        End If

        If pUnitsTable.FindFirst(1, aConstituent) Then 'cons in field 1
            lUnits = pUnitsTable.Value(2) 'units in field 2
            If lUnits = pUnknownUnits Then
                lUnknownFlag = True
                lUnits = "" 'forces another look at generic tables
            Else 'found a good one
                pCountUnitsHardCode += 1
            End If
        End If

        If lUnits.Length = 0 Then 'try generic unit table stored with dll - file may be customized for a specific installation
            If pUnitsTableTemplate Is Nothing Then 'open the generic table
                pUnitsTableTemplate = New atcTableDBF
                Dim lUnitsTemplateFileName As String = IO.Path.ChangeExtension(Reflection.Assembly.GetExecutingAssembly.Location, "units.dbf")
                If FileExists(lUnitsTemplateFileName) Then
                    pUnitsTableTemplate.OpenFile(lUnitsTemplateFileName)
                    Logger.Dbg("Using Units Template File:" & lUnitsTemplateFileName)
                Else
                    Logger.Dbg("Units Template File Not Found:" & lUnitsTemplateFileName)
                End If
            End If
            If pUnitsTableTemplate.FindFirst(1, aConstituent) Then 'cons in field 1
                lUnits = pUnitsTableTemplate.Value(2) 'units in field 2
                pCountUnitsFound += 1
            Else  'try HspfMsg
                If pHspfMsgUnits Is Nothing Then
                    Try
                        pHspfMsgUnits = New Generic.Dictionary(Of String, String)
                        Dim pHspfMsg As New atcUCI.HspfMsg
                        pHspfMsg.Open("hspfmsg.wdm")
                        For lTsGroupIndex As Integer = 0 To pHspfMsg.TSGroupDefs.Count - 1
                            Dim lTsGroup As atcUCI.HspfTSGroupDef = pHspfMsg.TSGroupDefs(lTsGroupIndex)
                            For lTsMemberIndex As Integer = 0 To lTsGroup.MemberDefs.Count - 1
                                Dim lTsMember As atcUCI.HspfTSMemberDef = lTsGroup.MemberDefs(lTsMemberIndex)
                                'todo: check english/metric flag, english assumed for now
                                If pHspfMsgUnits.ContainsKey(lTsMember.Name.ToLower) Then
                                    If pHspfMsgUnits.Item(lTsMember.Name.ToLower) <> lTsMember.EUnits Then
                                        If DebugLevel > 1 Then
                                            Logger.Dbg("UnitsConflict " & pHspfMsgUnits.Item(lTsMember.Name.ToLower).ToString & ":" & lTsMember.EUnits)
                                        End If
                                    End If
                                Else
                                    pHspfMsgUnits.Add(lTsMember.Name.ToLower, lTsMember.EUnits)
                                End If
                            Next
                        Next
                    Catch exHspfMsg As Exception
                        Logger.Dbg("Exception getting units from hspfmsg: " & exHspfMsg.ToString)
                    End Try
                    If DebugLevel > 1 Then
                        Logger.Dbg("MessageFileUnitCount " & pHspfMsgUnits.Count)
                    End If
                End If

                If pHspfMsgUnits.ContainsKey(lUnitsKey) Then
                    lUnits = pHspfMsgUnits.Item(lUnitsKey).ToString
                    If lUnits.Length > 0 Then pCountUnitsFound += 1
                End If

                If lUnits.Length = 0 Then
                    pCountUnitsMissing += 1
                    'Logger.Dbg("Missing " & aConstituent)
                    pUnitsMissing.Increment(aConstituent, 1)
                    lUnits = pUnknownUnits
                End If
            End If
            If Not lUnknownFlag Then 'save a local copy
                With pUnitsTable
                    .NumRecords += 1
                    .MoveLast()
                    .Value(1) = aConstituent
                    .Value(2) = lUnits
                    pUnitsTableModified = True
                End With
            End If
        End If
        pUnitsEnglish.Add(lUnitsKey, lUnits)
        Return lUnits
    End Function

    Public Overrides Sub ReadData(ByVal aDataSet As atcDataSet)

        Dim lTimeseries As atcTimeseries = aDataSet
        lTimeseries.ValuesNeedToBeRead = False
        Dim lKey As String = KeyFromAttributes(lTimeseries.Attributes)
        Dim lNeedToClose As Boolean = pBinFile.Open(False)
        Dim lNeedDates As Boolean = lTimeseries.Dates.numValues < 1
        Dim lValues As New Generic.List(Of Double)
        Dim lJDates As Generic.List(Of Double) = Nothing
        If lNeedDates Then lJDates = New Generic.List(Of Double)
        Dim lTimeUnit As atcTimeUnit = atcTimeUnit.TUUnknown
        Dim lTimeStep As Integer = 1
        Try
            Dim lBinHeader As HspfBinaryHeader = pBinFile.Headers(lKey)
            With lBinHeader
                Dim lVariableIndex As Integer = .VarNames.IndexOf(lTimeseries.Attributes.GetValue("IDCONS"))
                If lVariableIndex >= 0 Then
                    Dim lSJday As Double = lTimeseries.Attributes.GetValue("SJDay")
                    Dim lEJday As Double = lTimeseries.Attributes.GetValue("EJDay")
                    Dim lShared As atcDataAttributes = Nothing
                    If lNeedDates Then lJDates.Add(lSJday)
                    lValues.Add(pNaN)
                    For Each lHspfBinaryDatum As HspfBinaryDatum In .Data
                        Dim lCurJday As Double = lHspfBinaryDatum.JDate
                        If lCurJday >= lSJday Then
                            If lCurJday > lEJday Then
                                Exit For
                            End If
                            lValues.Add(pBinFile.ReadValue(lHspfBinaryDatum.ValuesStartPosition, lVariableIndex))
                            If lNeedDates Then
                                lJDates.Add(lCurJday)
                                If lJDates.Count = 3 Then 'Compute beginning of first interval
                                    CalcTimeUnitStep(lJDates(1), lJDates(2), lTimeUnit, lTimeStep)
                                    lJDates(0) = TimAddJ(lJDates(1), lTimeUnit, lTimeStep, -1)
                                    lTimeseries.SetInterval(lTimeUnit, lTimeStep)
                                    If lTimeUnit <> atcTimeUnit.TUUnknown Then
                                        lShared = lTimeseries.Attributes.GetValue("SharedAttributes")
                                        If lShared IsNot Nothing Then
                                            lShared.SetValue("Time Step", lTimeStep)
                                            lShared.SetValue("Time Unit", lTimeUnit)
                                            If lTimeseries.Attributes.ContainsAttribute("Interval") Then
                                                lShared.SetValue("Interval", lTimeseries.Attributes.GetValue("Interval"))
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    Next
                Else
                    Logger.Dbg("Could not retrieve HSPF Binary data values for variable: " & lTimeseries.Attributes.GetValue("IDCONS"))
                End If
            End With
        Catch ex As Exception
            Logger.Dbg("Could not retrieve data values for HSPF Binary TSER" & "Key = " & lKey & vbCrLf & _
                       "Message:" & ex.ToString)
        Finally
            If lNeedToClose Then pBinFile.Close(False)
        End Try
        With lTimeseries
            If lNeedDates Then
                Dim lNumValues As Integer = lJDates.Count - 1
                Dim lMatchingDates As atcTimeseries = Nothing
                'Search for existing Dates that exactly match these so we can share that one and save memory
                For Each lOtherTimeseries As atcTimeseries In DataSets
                    If Not lOtherTimeseries.ValuesNeedToBeRead Then
                        Dim lOtherDates As atcTimeseries = lOtherTimeseries.Dates
                        If lOtherDates.Serial <> .Dates.Serial AndAlso lOtherDates.numValues = lNumValues Then
                            lMatchingDates = lOtherDates
                            For lDateIndex As Integer = lNumValues To 1 Step -1
                                If lOtherDates.Value(lDateIndex) <> lJDates(lDateIndex) Then
                                    lMatchingDates = Nothing
                                    Exit For
                                End If
                            Next
                            If lMatchingDates IsNot Nothing Then Exit For
                        End If
                    End If
                Next
                If lMatchingDates Is Nothing Then
                    'Found a new set of dates, use them
                    .Dates.Values = lJDates.ToArray
                Else 'Already have these Dates, re-use them for all timeseries sharing this header
                    Dim lDisposingDates As atcTimeseries = .Dates
                    For Each lOtherTimeseries As atcTimeseries In DataSets
                        If KeyFromAttributes(lOtherTimeseries.Attributes) = lKey Then
                            lOtherTimeseries.Dates = lMatchingDates
                            If lTimeUnit <> atcTimeUnit.TUUnknown Then
                                Dim lShared As atcDataAttributes = lOtherTimeseries.Attributes.GetValue("SharedAttributes")
                                If lShared Is Nothing Then
                                    lOtherTimeseries.SetInterval(lTimeUnit, lTimeStep)
                                Else
                                    lShared.SetValue("Time Step", lTimeStep)
                                    lShared.SetValue("Time Unit", lTimeUnit)
                                    If lTimeseries.Attributes.ContainsAttribute("Interval") Then
                                        lShared.SetValue("Interval", lTimeseries.Attributes.GetValue("Interval"))
                                    End If
                                End If
                            End If
                        End If
                    Next
                    lDisposingDates.Clear()
                End If
            End If

            'Setting .Values below will clear calculated attributes, but we want to preserve them.
            Dim lPreserveCalculated As New Generic.List(Of atcDefinedValue)
            For Each lAttribute As atcDefinedValue In .Attributes
                If lAttribute.Definition.Calculated Then
                    lPreserveCalculated.Add(lAttribute)
                End If
            Next

            .Values = lValues.ToArray

            For Each lAttribute As atcDefinedValue In lPreserveCalculated
                .Attributes.Add(lAttribute)
            Next

            If (.Attributes.GetValue("Point", False)) Then
                .Values(0) = pNaN
            Else
                .Values(0) = lValues(1)
            End If
            .ValuesNeedToBeRead = False
        End With
        'atcDataManager.AddDiscardableTimeseries(lTimeseries)
    End Sub

    Private Function KeyFromAttributes(ByVal aAttributes As atcDataAttributes) As String
        Return aAttributes.GetValue("Operation", "unk") & ":" _
             & aAttributes.GetValue("IDLOCN", "u:unk").ToString.Substring(2) & ":" _
             & aAttributes.GetValue("Section", "unk")
    End Function

    Public Sub Refresh()
        pBinFile.ReadNewRecords()
    End Sub

    Public Function RemoveTimSer(ByVal aTimeseries As atcTimeseries) As Boolean
        Throw New ApplicationException("Unable to Remove Time Series " & aTimeseries.ToString & vbCrLf & "From:" & Specification)
    End Function

    Public Function RewriteTimSer(ByVal aTimeseries As atcTimeseries) As Boolean
        Throw New ApplicationException("Unable to Rewrite Time Series " & aTimeseries.ToString & vbCrLf & "From:" & Specification)
    End Function

    Public Function SaveAs(ByVal aFilename As String) As Boolean
        Throw New ApplicationException("Unable to SaveAs " & aFilename & vbCrLf & "From:" & Specification)
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
            Return False 'TODO: change this when we can
        End Get
    End Property

    Public Overrides Function Open(ByVal aFileName As String, Optional ByVal aAttributes As atcDataAttributes = Nothing) As Boolean
        If DebugLevel > 0 Then Logger.Dbg("Opening " & aFileName)
        If MyBase.Open(aFileName, aAttributes) Then
            pUnitsTable = New atcTableDBF
            Dim lFileName As String = IO.Path.ChangeExtension(Me.Specification, "units.dbf")
            If FileExists(lFileName) Then
                pUnitsTable.OpenFile(lFileName)
                If DebugLevel > 0 Then Logger.Dbg("UsingUnitsFile " & lFileName & " WithRecordCount " & pUnitsTable.NumRecords)
            Else 'create from template
                With pUnitsTable
                    .NumFields = 2
                    .FieldName(1) = "CONS"
                    .FieldType(1) = "C"
                    .FieldLength(1) = 32
                    .FieldName(2) = "UNITS"
                    .FieldType(2) = "C"
                    .FieldLength(2) = 16
                    .FileName = lFileName
                End With
                If DebugLevel > 0 Then Logger.Dbg("Start With Empty Units (" & lFileName & " not found)")
            End If
            BuildTSers()
            Return True
        End If
        Return False
    End Function

    Public Sub New()
        Filter = pFilter
    End Sub

    Private Shared pShowViewMessage As Boolean = True
    Public Overrides Sub View()
        If pShowViewMessage Then
            Select Case Logger.MsgCustom(Specification & vbCrLf & "No text viewer available for this file", "View", _
                                         "Ok", "Show File Folder", "Stop showing this message")
                Case "Show File Folder"
                    OpenFile(IO.Path.GetDirectoryName(Specification))
                Case "Stop showing this message"
                    pShowViewMessage = False
            End Select
        End If
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
End Class