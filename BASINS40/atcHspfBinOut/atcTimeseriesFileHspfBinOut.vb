Option Strict Off
Option Explicit On

Imports atcData
Imports atcUtility
Imports MapWinUtility


Public Class atcTimeseriesFileHspfBinOut
    Inherits atcData.atcDataSource
    '##MODULE_REMARKS Copyright 2005 AQUA TERRA Consultants - Royalty-free use permitted under open source license

    Private pFileFilter As String = "HSPF Binary Output Files (*.hbn)|*.hbn"
    Private pName As String = "Timeseries::HSPF Binary Output"
    Private pErrorDescription As String
    'Private pMonitor As Object
    'Private pMonitorSet As Boolean = False
    Private pDates As ArrayList 'of atcTimeseries
    Private pNvals As Integer

    Private pBinFile As clsHspfBinary
    Private pHSPFNetwork As clsNetworkHspfOutput
    Private pFileExt As String

    'todo: where should this really be?
    Private Enum ATCTimeUnit
        TUSecond = 1
        TUMinute = 2
        TUHour = 3
        TUDay = 4
        TUMonth = 5
        TUYear = 6
        TUCentury = 7
    End Enum

    Public ReadOnly Property AvailableAttributes() As Collection
        Get
            Dim retval As Collection
            'Dim vAttribute As Variant
            'Dim lCurTSerAttr As ATCclsAttributeDefinition

            'needed to edit attributes? that can't be done for this type!
            'for now - just return nothing

            retval = New Collection
            'Set lCurTSerAttr = New ATCclsAttributeDefinition

            'If pHSPFOutput.DataCollection.Count > 0 Then
            '  For Each vAttribute In pHSPFOutput.DataCollection(1).Attribs
            '    lCurTSerAttr.Name = vAttribute.Name
            '    retval.Add lCurTSerAttr
            '  Next
            'End If
            Return retval
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

    Public ReadOnly Property ErrorDescription() As String
        Get
            ErrorDescription = pErrorDescription
            pErrorDescription = ""
        End Get
    End Property

    Public ReadOnly Property FileUnit() As Integer
        Get
            Return 0 'unknown here, known thru clsHspfBinary:clsFtnUnfFile
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

    Public WriteOnly Property Monitor() As Object
        Set(ByVal Value As Object)
            'pMonitor = Value
            'pMonitorSet = True
        End Set
    End Property

    Private Sub BuildTSers()
        Dim lTSer As atcTimeseries
        Dim lBaseAttributes As atcDataAttributes 'attributes related to dates common to all in ts in a header
        Dim lBaseTSer As atcTimeseries
        Dim lBinHeader As clsHspfBinHeader
        Dim lData As clsHspfBinData
        Dim lTu As Integer, lTs As Integer, lIntvl As Integer
        Dim lTDate(5) As Integer
        Dim lSJDate As Double, lEJDate As Double, lOutLev As Integer
        Dim i As Integer, j As Integer ', s As String

        'If pMonitorSet Then
        '    pMonitor.SendMonitorMessage("(OPEN HSPF Binary Output File)")
        '    pMonitor.SendMonitorMessage("(BUTTOFF CANCEL)")
        '    pMonitor.SendMonitorMessage("(BUTTOFF PAUSE)")
        '    pMonitor.SendMonitorMessage("(MSG1 " & Specification & ")")
        'End If

        pBinFile = New clsHspfBinary
        'pBinFile.Monitor = pMonitor
        pBinFile.Filename = Specification

        i = 0
        For Each lBinHeader In pBinFile.Headers '.Values
            With lBinHeader
                lData = .Data.ItemByIndex(0)
                lSJDate = Date2J(lData.DateArray)
                lOutLev = lData.OutLev
                If .Data.Count = 1 Then
                    j = 1 'force daily
                Else
                    j = 1
                    While lOutLev <> .Data.ItemByIndex(j).OutLev And i < .Data.Count 'looking for same outlev
                        j = j + 1
                    End While
                End If
                If j < .Data.Count Then
                    lData = .Data.ItemByIndex(j)
                    lEJDate = Date2J(lData.DateArray)
                Else 'only one value dont know what interval is, assume day
                    lEJDate = lSJDate + 1
                End If
                'lDates = New ATCclsTserDate
                lBaseTSer = New atcTimeseries(Me)
                lBaseAttributes = New atcDataAttributes
                With lBaseAttributes
                    .SetValue("CIntvl", True)
                    .SetValue("History 1", "Read from " & pBinFile.Filename)
                    Dim lFileDetails As System.IO.FileInfo = New System.IO.FileInfo(pBinFile.Filename)
                    .SetValue("Date Created", lFileDetails.CreationTime)
                    .SetValue("Date Modified", lFileDetails.LastWriteTime)
                    If lEJDate - lSJDate >= 1 Then 'daily or longer interval
                        lTs = 1
                        lTu = lBinHeader.Data.ItemByIndex(0).OutLev + 1
                        lIntvl = 1
                        'If lTu = ATCTimeUnit.TUDay Then
                        'Else 'undefined for monthly or annual
                        'End If
                    Else 'use minute
                        lTu = ATCTimeUnit.TUMinute
                        lTs = timdifJ(lSJDate, lEJDate, ATCTimeUnit.TUMinute, 1)
                        lIntvl = lTs / 1440
                    End If
                    .SetValue("Ts", lTs)
                    .SetValue("Tu", lTu)
                    .SetValue("Intvl", lIntvl)
                    lTDate = lBinHeader.Data.ItemByIndex(0).DateArray
                    lSJDate = TimAddJ(Date2J(lTDate), lTu, lTs, -lIntvl)
                    .SetValue("SJDay", lSJDate)
                    lTDate = lBinHeader.Data.ItemByIndex(lBinHeader.Data.Count - 1).DateArray
                    lEJDate = Date2J(lTDate)
                    .SetValue("EJDay", lEJDate)
                    pNvals = timdifJ(lSJDate, lEJDate, lTu, lTs)
                End With

                For j = 0 To .VarNames.Count - 1
                    lTSer = New atcTimeseries(Me)
                    For Each lAttributeName As DictionaryEntry In lBaseAttributes.ValuesSortedByName
                        With lBaseAttributes
                            lTSer.Attributes.SetValue(atcDataAttributes.GetDefinition(lAttributeName.Key), lAttributeName.Value)
                        End With
                    Next
                    With lTSer
                        .Attributes.SetValue("Operation", lBinHeader.id.OperationName)
                        .Attributes.SetValue("Section", lBinHeader.id.SectionName)
                        .Attributes.SetValue("IDSCEN", FilenameOnly(Specification))
                        .Attributes.SetValue("IDLOCN", Left(lBinHeader.id.OperationName, 1) & ":" & (lBinHeader.id.OperationNumber))
                        .Attributes.SetValue("IDCONS", lBinHeader.VarNames.ItemByIndex(j))
                        .Attributes.SetValue("ID", Me.DataSets.Count)
                        If lBinHeader.VarNames.ItemByIndex(j) = "LZS" Then 'TODO: need better check here
                            .Attributes.SetValue("Point", True)
                        End If
                        .ValuesNeedToBeRead = True
                        .Dates = New atcTimeseries(Me)
                        AddDataSet(lTSer)
                    End With
                Next j
            End With
            Logger.Progress(pBinFile.Headers.Count + i, pBinFile.Headers.Count * 2)
            'If pMonitorSet Then
            '    s = "(PROGRESS " & CStr(50 + ((100 * i) / (pBinFile.Headers.Count * 2))) & ")"
            '    pMonitor.SendMonitorMessage(s)
            'End If
            i = i + 1
        Next

        'If pMonitorSet Then
        '    pMonitor.SendMonitorMessage("(CLOSE)")
        '    pMonitor.SendMonitorMessage("(BUTTON CANCEL)")
        '    pMonitor.SendMonitorMessage("(BUTTON PAUSE)")
        'End If

    End Sub

    Public Overrides Sub readData(ByVal aDataSet As atcDataSet)
        Dim lts As atcTimeseries = aDataSet
        Dim lVind As Integer, lOutLev As Integer
        Dim lBinHeader As clsHspfBinHeader
        Dim lBd As clsHspfBinData
        Dim lKey As String
        Dim lCurJday As Double
        Dim lSJday As Integer
        Dim lEJday As Integer
        Dim i As Integer
        Dim j As Integer
        Dim v() As Double
        Dim f() As Integer
        Dim d() As Double
        ReDim v(0)
        ReDim f(0)
        ReDim d(0)

        lKey = lts.Attributes.GetValue("Operation") & ":" & _
               Mid(lts.Attributes.GetValue("IDLOCN"), 3) & ":" & _
               lts.Attributes.GetValue("Section")

        Try
            lBinHeader = pBinFile.Headers.ItemByKey(lKey)
            With lBinHeader
                lVind = .VarNames.IndexFromKey(lts.Attributes.GetValue("IDCONS"))
                If lVind >= 0 Then
                    ReDim v(pNvals)
                    ReDim f(pNvals)
                    ReDim d(pNvals)
                    lSJday = lts.Attributes.GetValue("SJDay")
                    lEJday = lts.Attributes.GetValue("EJDay")
                    lOutLev = .Data.ItemByIndex(0).OutLev
                    d(0) = lSJday
                    j = 1
                    For i = 0 To .Data.Count - 1
                        lBd = .Data.ItemByIndex(i)
                        lCurJday = Date2J(lBd.DateArray)
                        If lCurJday >= lSJday Then
                            If lCurJday > lEJday Then Exit For
                            If .Data.ItemByIndex(i).OutLev = lOutLev Then
                                v(j) = .Data.ItemByIndex(i).value(lVind)
                                d(j) = lCurJday
                                j = j + 1
                            End If
                        End If
                    Next i
                    If (lts.Attributes.GetValue("Point", False)) Then
                        v(0) = Double.NaN
                    Else
                        v(0) = v(1)
                    End If
                Else
                    pErrorDescription = "Could not retrieve HSPF Binary data values for variable: " & lts.Attributes.GetValue("IDCONS")
                End If
            End With
        Catch
            pErrorDescription = "Could not retrieve data values for HSPF Binary TSER" & "Key = " & lKey
            ReDim v(0)
            ReDim f(0)
        End Try
        't.flags = f
        lts.Dates.Values = d
        lts.Values = v
        't.calcSummary()
        ' next 2 might be automatic
        ReDim v(0)
        ReDim f(0)
        lts.ValuesNeedToBeRead = False
    End Sub

    Public Sub Refresh()
        pBinFile.ReadNewRecords()
    End Sub

    Public Function RemoveTimSer(ByVal t As atcTimeseries) As Boolean
        pErrorDescription = "Unable to Remove a Time Series for " & Description
        Return False
    End Function

    Public Function RewriteTimSer(ByVal t As atcTimeseries) As Boolean
        pErrorDescription = "Unable to Rewrite a Time Series for " & Description
        Return False
    End Function

    Public Function SaveAs(ByVal Filename As String) As Boolean
        pErrorDescription = "Unable to SaveAS for " & Description
        Return False
    End Function

    Public Sub ShowFilterEdit(ByVal icon As Object) 'should this be a ATCclsTserFile property or function?
        '  pHSPFOutput.Filter.ShowFilterEdit icon
    End Sub

    'Public Overrides ReadOnly Property FileFilter() As String
    '  Get
    '    Return pFileFilter
    '  End Get
    'End Property

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
        If aFileName Is Nothing OrElse aFileName.Length = 0 OrElse Not FileExists(aFileName) Then
            aFileName = FindFile("Select " & Name & " file to open", , , pFileFilter, True, , 1)
        End If
        If FileExists(aFileName) Then
            aFileName = AbsolutePath(aFileName, CurDir())
            Specification = aFileName
            BuildTSers()
            Return True
        End If
    End Function

End Class