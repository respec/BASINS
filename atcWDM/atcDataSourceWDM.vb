'Copyright 2001-5 AQUA TERRA Consultants - Royalty-free use permitted under open source license
Option Strict Off
Option Explicit On

Imports atcData
Imports atcData.atcDataSource.EnumExistAction
Imports atcUtility
Imports MapWinUtility
Imports System.Collections.Specialized

Public Class atcDataSourceWDM
    Inherits atcData.atcDataSource

    Private Shared pFileFilter As String = "WDM Files (*.wdm)|*.wdm"
    Private pErrorDescription As String
    Private pMonitor As Object
    Private pMonitorSet As Boolean
    Private pDates As ArrayList 'of atcTimeseries
    Private pQuick As Boolean
    Private Shared pMsg As atcMsgWDM

    'Private Structure BasinsInfo
    '  Dim desc As String
    '  Dim Nam As String
    '  Dim Elev As Single
    '  <VBFixedArray(3)> Dim sdat() As Integer
    '  <VBFixedArray(3)> Dim edat() As Integer
    '  Dim EvapCoef As Single
    '  <VBFixedArray(7)> Dim dsn() As Integer

    '  'UPGRADE_TODO: "Initialize" must be called to initialize instances of this structure. Click for more: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1026"'
    '  Public Sub Initialize()
    '    ReDim sdat(3)
    '    ReDim edat(3)
    '    ReDim dsn(7)
    '  End Sub
    'End Structure
    'Dim BasInf() As BasinsInfo
    'Dim BasInfAvail As Boolean

    'Private ReadOnly Property AvailableAttributes() As Collection
    '  Get
    '    Dim retval As Collection
    '    Dim vWDMAttr As Object
    '    Dim CurWDMAttr As clsAttributeWDM
    '    Dim CurTSerAttr As ATCData.ATCclsAttributeDefinition
    '    retval = New Collection
    '    For Each vWDMAttr In gMsg.Attributes
    '      CurTSerAttr = New ATCData.ATCclsAttributeDefinition
    '      CurWDMAttr = vWDMAttr
    '      With CurTSerAttr
    '        .DataType = CurWDMAttr.DataType
    '        .Default = CurWDMAttr.Default_Renamed
    '        .Description = CurWDMAttr.Description
    '        .Max = CurWDMAttr.Max
    '        .Min = CurWDMAttr.Min
    '        .Name = CurWDMAttr.Name
    '        .ValidValues = CurWDMAttr.ValidValues
    '      End With
    '      retval.Add(CurTSerAttr)
    '      CurTSerAttr = Nothing
    '    Next vWDMAttr
    '    AvailableAttributes = retval
    '  End Get
    'End Property

    'Public ReadOnly Property FileUnit() As Integer
    '  Get
    '    Return pFileUnit
    '  End Get
    'End Property

    'Private WriteOnly Property HelpFilename() As String Implements ATCData.ATCclsTserFile.HelpFilename
    '  Set(ByVal Value As String)
    '    'UPGRADE_ISSUE: App property App.HelpFile was not upgraded.
    '    App.HelpFile = Value
    '  End Set
    'End Property

    'Private WriteOnly Property Monitor() As Object Implements ATCData.ATCclsTserFile.Monitor
    '  Set(ByVal Value As Object)
    '    pMonitor = Value
    '    pMonitorSet = True
    '  End Set
    'End Property

    'Private ReadOnly Property ErrorDescription() As String Implements ATCData.ATCclsTserFile.ErrorDescription
    '  Get
    '    Return pErrorDescription
    '    pErrorDescription = ""
    '  End Get
    'End Property

    'Private ReadOnly Property MsgUnit() As Integer
    '  Get
    '    If pMsg Is Nothing Then
    '      Dim hspfMsgFileName As String = FindFile("Please locate HSPF message file", "hspfmsg.wdm")
    '      If Not FileExists(hspfMsgFileName) Then
    '        Logger.Msg("Could not find hspfmsg.wdm - " & hspfMsgFileName, "ATCdataWdm")
    '      Else
    '        pMsg = New atcMsgWDM
    '      End If
    '    End If
    '    Return pMsg.MsgUnit
    '  End Get
    '  'Set(ByVal Value As Integer)
    '  '  SetMsgUnit(Value)
    '  'End Set
    'End Property

    Private Sub Clear()
        pErrorDescription = ""
        pMonitorSet = False
        Specification = "<unknown>"
        pQuick = False

        pDates = Nothing
        pDates = New ArrayList
    End Sub

    '  oldID must be given if dataObject has a changed id
    Private Function writeDataHeader(ByRef aFileUnit As Integer, ByRef dataObject As atcTimeseries, Optional ByRef oldID As Integer = -1) As Boolean
        Dim salen, dsn, saind, lRetcod, i As Integer
        Dim c, S, l, d As String
        Dim lAttribs As atcDataAttributes = dataObject.Attributes

        Dim lMsgHandle As atcWdmHandle = pMsg.MsgHandle
        Dim lMsgUnit As Integer = lMsgHandle.Unit

        lRetcod = 0
        dsn = lAttribs.GetValue("id")
        Try
            If oldID <> -1 And oldID <> dsn Then 'try to change dsn
                Call F90_WDDSRN(aFileUnit, oldID, dsn, lRetcod)
            End If
        Catch
        End Try

        If lRetcod = 0 Then
            saind = 288
            salen = 8
            S = lAttribs.GetValue("scen")
            i = 1
            Call F90_WDBSAC(aFileUnit, dsn, lMsgUnit, saind, salen, lRetcod, S, Len(S))
            If lRetcod = 0 Then
                saind = 289
                c = lAttribs.GetValue("cons")
                i = 2
                Call F90_WDBSAC(aFileUnit, dsn, lMsgUnit, saind, salen, lRetcod, c, Len(c))
                If lRetcod = 0 Then
                    saind = 290
                    l = lAttribs.GetValue("locn")
                    i = 3
                    Call F90_WDBSAC(aFileUnit, dsn, lMsgUnit, saind, salen, lRetcod, l, Len(l))
                    If lRetcod = 0 Then
                        saind = 45
                        salen = 48
                        d = lAttribs.GetValue("desc")
                        If Len(d) > salen Then
                            Logger.Msg("Description: '" & d & vbCr & "truncated to: " & Left(d, salen), "WDM Write Data Header", MsgBoxStyle.Exclamation)
                        End If
                        i = 4
                        Call F90_WDBSAC(aFileUnit, dsn, lMsgUnit, saind, salen, lRetcod, d, Len(d))
                    End If
                End If
            End If
        End If

        If lRetcod = 0 Then
            writeDataHeader = DsnWriteAttributes(aFileUnit, dataObject)
        Else
            If Math.Abs(lRetcod) = 73 Then
                pErrorDescription = "Unable to renumber Dataset " & oldID & " to " & dsn
            Else
                pErrorDescription = "Unable to Write a Data Header for Class WDM, Retcod:" & lRetcod & " from " & i
            End If
            writeDataHeader = False
        End If

        lMsgHandle.Dispose()
    End Function

    Public Sub New()
        MyBase.New()
        If pMsg Is Nothing Then
            pMsg = New atcMsgWDM
            F90_MSG("WRITE", 5) 'turn on very detailed debugging of fortran to error.fil
        End If
        Clear()
    End Sub

    Private Sub Refresh(ByVal aWdmUnit As Integer)
        Dim lMsg As String
        Dim dsn As Integer
        Dim lDsn As Integer

        DataSets.Clear()

        pDates = Nothing
        pDates = New ArrayList

        If pMonitorSet Then
            pMonitor.SendMonitorMessage("(OPEN WDM File)")
            pMonitor.SendMonitorMessage("(BUTTOFF CANCEL)")
            pMonitor.SendMonitorMessage("(BUTTOFF PAUSE)")
            pMonitor.SendMonitorMessage("(MSG1 " & Specification & ")")
        End If

        dsn = 1
        While dsn > 0
            lDsn = dsn
            F90_WDDSNX(aWdmUnit, dsn)
            If dsn > 0 Then
                If F90_WDCKDT(aWdmUnit, dsn) = 1 Then
                    RefreshDsn(aWdmUnit, dsn)
                End If
            End If
            If pMonitorSet Then
                If dsn - lDsn > 100 Then
                    lMsg = "(PROGRESS " & CStr((100 * dsn) / 32000) & ")"
                    pMonitor.SendMonitorMessage(lMsg)
                ElseIf dsn = -1 Then
                    lMsg = "(PROGRESS 100)"
                    pMonitor.SendMonitorMessage(lMsg)
                End If
            End If
            dsn = dsn + 1
        End While

        If pMonitorSet Then
            pMonitor.SendMonitorMessage("(CLOSE)")
            pMonitor.SendMonitorMessage("(BUTTON CANCEL)")
            pMonitor.SendMonitorMessage("(BUTTON PAUSE)")
        End If

    End Sub

    Private Function GetDataSetFromDsn(ByRef lDsn As Integer) As atcTimeseries
        For Each curDataset As atcTimeseries In DataSets
            If curDataset.Attributes.GetValue("id") = lDsn Then
                Return curDataset
            End If
        Next
        Logger.Msg("DSN " & lDsn & " does not exist.", "DataFileWDM.GetDataSetFromDsn")
        Return Nothing
    End Function

    Private Function AttrStored(ByRef saind As Integer) As Boolean 'somewhere else
        Select Case saind
            Case 17 : AttrStored = True 'tcode
                'Case 27: AttrStored = True 'tsbyr  'jlk commmented to fix winhspf problem
            Case 33 : AttrStored = True 'tsstep
            Case 45 : AttrStored = True 'staname
            Case 288 : AttrStored = True 'idscen
            Case 289 : AttrStored = True 'idlocn
            Case 290 : AttrStored = True 'idcons
            Case Else : AttrStored = False
        End Select
    End Function

    Public Overrides Function AddDataset(ByVal aDataSet As atcData.atcDataSet, _
                                Optional ByVal aExistAction As atcData.atcDataSource.EnumExistAction = atcData.atcDataSource.EnumExistAction.ExistReplace) _
                                         As Boolean
        Logger.Dbg("atcDataSourceWdm:AddDataset:entry:" & aExistAction)
        Try
            Dim lTimser As atcTimeseries = aDataSet
            Dim lTs As Integer = lTimser.Attributes.GetValue("ts")
            Dim lTu As Integer = lTimser.Attributes.GetValue("tu")
            Dim lNvals As Integer = lTimser.numValues
            Dim lSJDay As Double = lTimser.Attributes.GetValue("SJDay", 0)
            Dim lSDat(5) As Integer
            J2Date(lSJDay, lSDat)
            Dim lEJDay As Double = lTimser.Attributes.GetValue("EJDay", 0)
            Dim lEDat(5) As Integer
            J2Date(lEJDay, lEDat)
            Dim lNValsExpected As Integer
            timdif(lSDat, lEDat, lTu, lTs, lNValsExpected)
            If lNvals <> lNValsExpected Then
                'TODO:  make writing data smarter to deal with big gaps of missing, etc
                Throw New Exception("NVals:" & lNvals & ":" & lNValsExpected)
            End If

            'aDataSet.Attributes.CalculateAll() 'should we calculdate attributes on request, not here on creation

            Dim lWdmHandle As New atcWdmHandle(0, Specification)
            Dim lDsn As Integer = aDataSet.Attributes.GetValue("id", 1)

            Logger.Dbg("atcDataSourceWdm:AddDataset:WdmUnit:Dsn:" & lWdmHandle.Unit & ":" & lDsn)

            If F90_WDCKDT(lWdmHandle.Unit, lDsn) > 0 Then 'dataset exists, what do we do?
                If aExistAction = ExistReplace Then
                    Dim lRet As Integer
                    F90_WDDSDL(lWdmHandle.Unit, lDsn, lRet)
                    Logger.Dbg("atcDataSourceWdm:RemovedOld:" & lWdmHandle.Unit & ":" & lDsn & ":" & lRet)
                ElseIf aExistAction = ExistAppend Then 'find dataset and try to append to it
                    Logger.Dbg("atcDataSourceWdm:ExistAppend:" & lWdmHandle.Unit & ":" & lDsn)
                    Dim lExistTimser As atcTimeseries = DataSets.ItemByKey(lDsn)
                    If lTimser.Dates.Value(1) <= lExistTimser.Dates.Value(lExistTimser.numValues) Then
                        Throw New Exception("atcDataSourceWDM:AddDataset: Unable to append new TSer " & _
                        lTimser.ToString & " to existing TSer " & lExistTimser.ToString & vbCrLf & _
                        "New TSer start date (" & lTimser.Dates.Value(1) & _
                        ") preceeds exising TSer end date (" & lExistTimser.Dates.Value(lExistTimser.numValues) & ")")
                    End If
                    Dim lExistEndDateJ As Double = lExistTimser.Dates.Value(lExistTimser.numValues)
                    Logger.Dbg("atcDataSourceWdm:OldFinalDate:" & DumpDate(lExistEndDateJ))
                    Dim lNewStartDateJ As Double = lTimser.Dates.Value(0)
                    Logger.Dbg("atcDataSourceWdm:NewFirstDate:" & DumpDate(lNewStartDateJ))
                Else
                    lDsn = findNextDsn(lDsn)
                    Logger.Dbg("atcDataSourceWdm:AddNew:" & lWdmHandle.Unit & ":" & lDsn)
                    aDataSet.Attributes.SetValue("Id", lDsn)
                End If
            Else
                Logger.Dbg("atcDataSourceWdm:AddDataset:NewDataSet:WdmUnit:Dsn:" & lWdmHandle.Unit & ":" & lDsn)
            End If

            Dim lWriteIt As Boolean = False
            If aExistAction = ExistAppend Then
                'just write appended data
                lWriteIt = True
            ElseIf DsnBld(lWdmHandle.Unit, lTimser) Then
                DataSets.Add(lDsn, lTimser)
                lWriteIt = True
            End If

            If lWriteIt Then
                Dim lTSFill As Double = lTimser.Attributes.GetValue("tsfill", -999)
                Dim lValue As Double
                Dim lV(lNvals) As Single
                Dim lRet As Integer
                For i As Integer = 1 To lNvals
                    lValue = lTimser.Value(i)
                    If Double.IsNaN(lValue) Then lValue = lTSFill
                    lV(i) = lValue
                Next

                J2DateRoundup(lTimser.Dates.Value(0), lTu, lSDat)
                Logger.Dbg("atcDataSourceWdm:AddDataset:WDTPUT:call:" & _
                            lWdmHandle.Unit & ":" & lDsn & ":" & lTs & ":" & lNvals & ":" & _
                            lSDat(0) & ":" & lSDat(1) & ":" & lSDat(2) & ":" & lRet)
                If lNvals > 0 Then
                    Call F90_WDTPUT(lWdmHandle.Unit, lDsn, lTs, lSDat(0), lNvals, CInt(1), CInt(0), lTu, lV(1), lRet)
                End If
                Logger.Dbg("atcDataSourceWdm:AddDataset:WDTPUT:back:" & _
                            lWdmHandle.Unit & ":" & lDsn & ":" & lRet)
            End If

            lWdmHandle.Dispose()
            Return True
        Catch ex As Exception
            Logger.Dbg("atcDataSourceWdm:AddDataSet:" & ex.ToString)
            Return False
        End Try

    End Function

    '    dsn = t.Header.id
    '    i = 1
    '    For Each vData In pData
    '      lData = vData
    '      If dsn = lData.Header.id Then
    '        If lData.serial = t.serial Then 'exists already
    '          AddTimSer = RewriteTimSer(t)
    '          Exit Function
    '        End If
    '        dsn = findNextDsn(dsn + 1) 'find next dsn
    '        If lExAct = ATCData.ATCTsIdExistAction.TsIdNoAction Then 'just report the problem
    '          pErrorDescription = "WDM:AddTimSer:Id(DSN) " & t.Header.id & " Exists:Next Available is " & dsn
    '          Exit Function
    '        ElseIf lExAct > ATCData.ATCTsIdExistAction.TsIdRenum Then  'ask user what to do
    '          nBtns = 0
    '          If lExAct And ATCData.ATCTsIdExistAction.TsIdReplAsk Then 'overwrite is an option
    '            ReDim Preserve BtnName(nBtns)
    '            'UPGRADE_WARNING: Couldn't resolve default property of object BtnName(nBtns)
    '            BtnName(nBtns) = "&Overwrite"
    '            nBtns = nBtns + 1
    '          End If
    '          If lExAct And ATCData.ATCTsIdExistAction.TsIdAppendAsk Then 'append is an option
    '            ReDim Preserve BtnName(nBtns)
    '            'UPGRADE_WARNING: Couldn't resolve default property of object BtnName(nBtns)
    '            BtnName(nBtns) = "&Append"
    '            nBtns = nBtns + 1
    '          End If
    '          If lExAct And ATCData.ATCTsIdExistAction.TsIdRenumAsk Then 'renumber is an option
    '            ReDim Preserve BtnName(nBtns)
    '            'UPGRADE_WARNING: Couldn't resolve default property of object BtnName(nBtns)
    '            BtnName(nBtns) = "&Renumber"
    '            nBtns = nBtns + 1
    '          End If
    '          'always have Cancel as last button (and default)
    '          ReDim Preserve BtnName(nBtns)
    '          'UPGRADE_WARNING: Couldn't resolve default property of object BtnName(nBtns)
    '          BtnName(nBtns) = "+-&Cancel"
    '          myMsgBox = New ATCoCtl.ATCoMessage
    '          UsrExAct = myMsgBox.Showarray("WDM data-set number " & t.Header.id & " already exists." & vbCrLf & "Next available data-set number is " & dsn & vbCrLf & "What should be done to it?", "WDM Data-set Number Problem", BtnName)
    '          'UPGRADE_WARNING: Couldn't resolve default property of object BtnName()
    '          If InStr(BtnName(UsrExAct - 1), "Overwrite") > 0 Then
    '            lExAct = ATCData.ATCTsIdExistAction.TsIdRepl 'set to overwrite
    '            'UPGRADE_WARNING: Couldn't resolve default property of object BtnName()
    '          ElseIf InStr(BtnName(UsrExAct - 1), "Append") > 0 Then
    '            lExAct = ATCData.ATCTsIdExistAction.TsIdAppend 'set to append
    '            'UPGRADE_WARNING: Couldn't resolve default property of object BtnName()
    '          ElseIf InStr(BtnName(UsrExAct - 1), "Renumber") > 0 Then
    '            lExAct = ATCData.ATCTsIdExistAction.TsIdRenum 'set to renumber
    '            'UPGRADE_WARNING: Couldn't resolve default property of object BtnName()
    '          ElseIf InStr(BtnName(UsrExAct - 1), "Cancel") > 0 Then
    '            pErrorDescription = "WDM:AddTimSer:Id(DSN) " & t.Header.id & ".  User Cancelled on message box to resolve."
    '            Exit Function
    '          End If
    '        End If
    '        If lExAct = ATCData.ATCTsIdExistAction.TsIdRepl Then 'overwrite the data set
    '          Call F90_WDDSDL(pFileUnit, t.Header.id, retcod)
    '          If retcod = 0 Then 'deleted ok
    '            'Set values in replaced time series
    '            'lData.Dtran = t.Dtran
    '            'lData.flags = VB6.CopyArray(t.flags)
    '            'lData.Max = t.Max
    '            'lData.Min = t.Min
    '            'lData.Values = VB6.CopyArray(t.Values)
    '            'lData.Dates = t.Dates
    '            'lData.Header = t.Header
    '            'lData.Attribs = t.Attribs

    '            'remove replaced timeseries from collections
    '            pDates.Remove((i))
    '            pData.Remove((i))
    '            OvwrtFg = True
    '          Else 'problem deleting
    '            If ExistAction > ATCData.ATCTsIdExistAction.TsIdRenum Then 'report problem to user
    '              MsgBox("Could not overwrite data-set number " & t.Header.id & " on WDM file " & pFileName & ".", MsgBoxStyle.Exclamation, "WDM Data Set Problem")
    '            End If
    '            pErrorDescription = "WDM:AddTimSer:Id(DSN) " & t.Header.id & " could not be deleted during data-set Overwrite."
    '            Exit Function
    '          End If
    '        ElseIf lExAct = ATCData.ATCTsIdExistAction.TsIdAppend Then  'append to data set
    '          If t.Dates.Summary.SJDay >= lData.Dates.Summary.EJDay Then
    '            'start of new data follows end of existing, ok to append
    '            AppendFg = True
    '            TsInd = i
    '          Else 'can't append, new and existing data spans overlap
    '            If ExistAction > ATCData.ATCTsIdExistAction.TsIdRenum Then 'report problem to user
    '              MsgBox("Time span of new data and existing data overlap." & vbCrLf & "Unable to append data to data-set number " & t.Header.id & " on WDM file " & pFileName & ".", MsgBoxStyle.Exclamation, "WDM Data Set Problem")
    '            End If
    '            pErrorDescription = "WDM:AddTimSer:Id(DSN) " & t.Header.id & " could not have new data appended to it."
    '            Exit Function
    '          End If
    '        ElseIf lExAct = ATCData.ATCTsIdExistAction.TsIdRenum Then  'renumber data set
    '          t.Header.id = dsn 'assign to next available data-set number
    '        End If
    '      End If
    '      i = i + 1
    '    Next vData

    '    If Not AppendFg Then
    '      pData.Add(t) 'add to internal collection
    '      pDates.Add(t.Dates)
    '      t.File = Me
    '    End If

    '    If pFileUnit > 0 Then 'save on wdm
    '      'i = F90_WDCKDT(pFileUnit, dsn)
    '      'If i = 0 Then 'dateset does not exist
    '      If Not AppendFg Then
    '        'create and add attributes
    '        bldOk = DsnBld(t)
    '      Else
    '        bldOk = True
    '      End If
    '      If bldOk Then
    '        'add data
    '        AddTimSer = RewriteTimSer(t)
    '        If AddTimSer And AppendFg Then
    '          'update end date
    '          pDates.Remove((TsInd))
    '          pData.Remove((TsInd))
    '          RefreshDsn((t.Header.id))
    '        End If
    '      Else
    '        AddTimSer = bldOk
    '      End If

    '      If ExistAction > ATCData.ATCTsIdExistAction.TsIdRenum Then 'report status
    '        If AddTimSer Then 'write succeeded
    '          If OvwrtFg Then
    '            S = "Successfully overwrote existing data-set number " & t.Header.id & " on WDM file " & pFileName & "."
    '          ElseIf AppendFg Then
    '            S = "Successfully appended data to existing data-set number " & t.Header.id & " on WDM file" & pFileName & "."
    '          Else 'new data set
    '            S = "New data-set number " & t.Header.id & " successfully stored on WDM file" & pFileName & "."
    '          End If
    '          MsgBox(S, MsgBoxStyle.Information + MsgBoxStyle.OKOnly, "WDM Data Set Add")
    '        Else 'problem
    '          If AppendFg Then
    '            S = "Problem appending data to data-set number " & t.Header.id & " on WDM file " & pFileName & "." & vbCr & ErrorDescription
    '          Else
    '            S = "Problem adding data-set number " & t.Header.id & " to WDM file " & pFileName & "." & vbCr & ErrorDescription
    '          End If
    '          MsgBox(S, MsgBoxStyle.Information + MsgBoxStyle.OKOnly, "WDM Data Set Problem")
    '        End If
    '      End If
    '    End If
    '    If lWdmOpen <> 1 Then i = F90_WDMCLO(pFileUnit)
    '    Exit Function
    'ErrHandler:
    '    MsgBox("Error adding timser" & vbCr & Err.Description, MsgBoxStyle.Critical, Label)

    Private Function findNextDsn(ByRef aDsn As Integer) As Integer
        Dim lDataset As atcDataSet
        Dim lRetval As Integer

        lRetval = aDsn
        For Each lDataset In DataSets
            If lDataset.GetType.FullName = "atcData.atcTimeseries" Then
                If lRetval = lDataset.Attributes.GetValue("Id", lRetval) Then
                    lRetval = findNextDsn(aDsn + 1)
                    Exit For
                End If
            End If
        Next lDataset
        findNextDsn = lRetval
    End Function

    Private Function RemoveTimSer(ByVal aFileUnit As Integer, ByRef t As atcTimeseries) As Boolean
        Dim retc As Integer
        Dim i, searchSerial As Integer
        Dim removeDate As Boolean

        Call F90_WDDSDL(aFileUnit, (t.Attributes.GetValue("id")), retc)
        If retc = 0 Then
            RemoveTimSer = True
            searchSerial = t.Serial
            For i = 1 To DataSets.Count()
                If DataSets.Item(i).Serial = searchSerial Then DataSets.RemoveAt(i) : Exit For
            Next

            removeDate = True
            searchSerial = t.Dates.Serial
            For i = 1 To DataSets.Count()
                Dim ts As atcTimeseries = DataSets.Item(i)
                If ts.Dates.Serial = searchSerial Then removeDate = False : Exit For
            Next

            If removeDate Then
                For i = 1 To pDates.Count()
                    If pDates.Item(i).serial = searchSerial Then pDates.Remove(i) : Exit For
                Next
            End If
        Else
            RemoveTimSer = False
            pErrorDescription = "WDM:RemoveTimSer:DSN" & t.Attributes.GetValue("id") & ":Retcod:" & retc
        End If
    End Function

    Public Overrides Function Save(ByVal SaveFileName As String, _
                          Optional ByRef ExistAction As EnumExistAction = EnumExistAction.ExistReplace) As Boolean
        'Dim i, lFileUnit As Integer
        'Dim lWdmOpen As Integer

        'TODO: check FileName and SaveFileName, use value of ExistAction

        'If Len(FileName) > 0 Then
        '  lWdmOpen = F90_WDMOPN(pFileUnit, FileName, Len(FileName))
        'Else
        '  lWdmOpen = -1
        'End If

        'i = 2
        'lFileUnit = F90_WDBOPN(i, SaveFileName, Len(SaveFileName))
        'If lFileUnit > 0 Then 'create worked
        '  If pFileUnit > 0 Then 'copy existing data here

        '    'close current
        '    i = F90_WDFLCL(pFileUnit)
        '  End If
        '  'update
        '  FileName = AbsolutePath(SaveFileName, CurDir())
        '  pFileUnit = lFileUnit
        '  refresh(lFileUnit)
        '  If lWdmOpen <> 1 Then i = F90_WDMCLO(pFileUnit)
        'End If
    End Function

    Private Function DsnBld(ByVal aFileUnit As Integer, ByRef t As atcTimeseries) As Boolean
        Dim dsn, nsasp, nup, ndn, nsa, ndp, i As Integer
        Dim salen, psa, iVal, saind, retcod As Integer
        Dim rVal As Single
        Dim ostr As String
        Dim CSDat(6) As Integer
        Dim lS As String
        Dim lTs As Integer
        Dim lTu As Integer

        Dim lMsgHandle As atcWdmHandle = pMsg.MsgHandle
        Dim lMsgUnit As Integer = lMsgHandle.Unit

        dsn = t.Attributes.GetValue("id", 0)
        'create label
        ndn = t.Attributes.GetValue("NDN", 10)
        nup = t.Attributes.GetValue("NUP", 10)
        nsa = t.Attributes.GetValue("NSA", 30)
        nsasp = t.Attributes.GetValue("NSASP", 100)
        ndp = t.Attributes.GetValue("NDP", 300)
        Call F90_WDLBAX(aFileUnit, dsn, 1, ndn, nup, nsa, nsasp, ndp, psa)

        'add needed attributes
        saind = 1 'tstype
        salen = 4
        ostr = t.Attributes.GetValue("TSTYPE", Left(t.Attributes.GetValue("cons"), 4))
        Call F90_WDBSAC(aFileUnit, dsn, lMsgUnit, saind, salen, retcod, ostr, Len(ostr))
        salen = 1
        saind = 34 'tgroup
        iVal = t.Attributes.GetValue("TGROUP", 6)
        Call F90_WDBSAI(aFileUnit, dsn, lMsgUnit, saind, salen, iVal, retcod)
        saind = 83 'compfg
        iVal = t.Attributes.GetValue("COMPFG", 1)
        Call F90_WDBSAI(aFileUnit, dsn, lMsgUnit, saind, salen, iVal, retcod)
        saind = 84 'tsform
        iVal = t.Attributes.GetValue("TSFORM", 1)
        Call F90_WDBSAI(aFileUnit, dsn, lMsgUnit, saind, salen, iVal, retcod)
        saind = 34 'tsfill
        rVal = t.Attributes.GetValue("TSFILL", -999)
        Call F90_WDBSAR(aFileUnit, dsn, lMsgUnit, saind, salen, iVal, retcod)
        saind = 17 'tcode
        lS = t.Attributes.GetValue("tu")
        If lS.Length = 0 Then
            CalcTimeUnitStep(t.Dates.Value(0), t.Dates.Value(1), lTu, lTs)
            t.Attributes.SetValue("tu", lTu)
            t.Attributes.SetValue("ts", lTs)
        Else
            lTu = lS
            saind = 33 'tsstep
            lTs = t.Attributes.GetValue("ts")
        End If
        saind = 17 'tcode
        Call F90_WDBSAI(aFileUnit, dsn, lMsgUnit, saind, salen, lTu, retcod)
        saind = 33 'tsstep
        Call F90_WDBSAI(aFileUnit, dsn, lMsgUnit, saind, salen, lTs, retcod)
        saind = 85 'vbtime
        iVal = t.Attributes.GetValue("VBTIME", 1)
        If lTs > 1 Then iVal = 2 'timestep > 1 vbtime must vary
        Call F90_WDBSAI(aFileUnit, dsn, lMsgUnit, saind, salen, iVal, retcod)
        Call J2Date(t.Dates.Value(0), CSDat)
        i = CSDat(0) Mod 10
        If i > 0 Then 'subtract back to start of this decade
            iVal = CSDat(0) - i
        Else 'back to start of previous decade
            iVal = CSDat(0) - 10
        End If
        saind = 27 'tsbyr
        Call F90_WDBSAI(aFileUnit, dsn, lMsgUnit, saind, salen, iVal, retcod)
        salen = 8
        saind = 288 'scenario
        ostr = UCase(Left(t.Attributes.GetValue("scen"), salen))
        Call F90_WDBSAC(aFileUnit, dsn, lMsgUnit, saind, salen, retcod, ostr, Len(ostr))
        saind = 289 'constituent
        ostr = UCase(Left(t.Attributes.GetValue("cons"), salen))
        Call F90_WDBSAC(aFileUnit, dsn, lMsgUnit, saind, salen, retcod, ostr, Len(ostr))
        saind = 290 'location
        ostr = UCase(Left(t.Attributes.GetValue("locn"), salen))
        Call F90_WDBSAC(aFileUnit, dsn, lMsgUnit, saind, salen, retcod, ostr, Len(ostr))
        salen = 48
        saind = 45 'description
        ostr = Left(t.Attributes.GetValue("desc"), salen)
        Call F90_WDBSAC(aFileUnit, dsn, lMsgUnit, saind, salen, retcod, ostr, Len(ostr))

        'others (from attrib)
        DsnBld = DsnWriteAttributes(aFileUnit, t)

        lMsgHandle.Dispose()
    End Function

    Private Function DsnWriteAttributes(ByRef aFileUnit As Integer, ByRef t As atcTimeseries) As Boolean
        Dim lName As String
        Dim lValue As Object
        Dim lDefinition As atcAttributeDefinition
        Dim lMsgDefinition As atcAttributeDefinition
        Dim lRetcod As Integer
        Dim lMsg As atcWdmHandle = pMsg.MsgHandle
        Dim lMsgUnit As Integer = lMsg.Unit
        Dim lDsn As Integer = t.Attributes.GetValue("id", 0)

        DsnWriteAttributes = True
        For i As Integer = 0 To t.Attributes.Count - 1
            lDefinition = t.Attributes(i).Definition
            lName = lDefinition.Name
            lValue = t.Attributes(i).Value
            If lName.ToLower = "units" Then  'store Units ID as DCODE in WDM
                lName = "DCODE"
                lValue = CStr(GetUnitID(lValue))
            End If
            lMsgDefinition = pMsg.Attributes.ItemByKey(lName.ToLower)
            If Not lMsgDefinition Is Nothing Then
                Select Case lMsgDefinition.TypeString
                    Case "Integer"
                        If IsNumeric(lValue) Then
                            F90_WDBSAI(aFileUnit, lDsn, lMsgUnit, lMsgDefinition.ID, 1, CInt(lValue), lRetcod)
                        End If
                    Case "Single"
                        If IsNumeric(lValue) Then
                            F90_WDBSAR(aFileUnit, lDsn, lMsgUnit, lMsgDefinition.ID, 1, CSng(lValue), lRetcod)
                        End If
                    Case Else 'character
                        F90_WDBSAC(aFileUnit, lDsn, lMsgUnit, lMsgDefinition.ID, lMsgDefinition.Max, lRetcod, lValue, Len(lValue))
                End Select
                If lRetcod <> 0 Then
                    If Math.Abs(lRetcod) = 104 Then 'cant update if data already present
                        Logger.Dbg("Skip:" & lName & ", data present")
                    Else
                        If Len(pErrorDescription) = 0 Then
                            pErrorDescription = "Unable to Write Data Attributes for Class WDM"
                        End If
                        pErrorDescription &= vbCrLf & "  Attribute:" & lName & ", Value:" & lValue & ", Retcod:" & lRetcod
                        DsnWriteAttributes = False
                    End If
                End If
            End If
        Next

        If Len(pErrorDescription) > 0 Then
            Logger.Dbg(pErrorDescription)
        End If

        lMsg.Dispose()
    End Function

    '  Public Function ReadBasInf() As Boolean
    '    Dim bFun As Integer
    '    Dim bNam As String
    '    Dim ip, i, j, NLoc As Integer
    '    Dim istr As String

    '    ReadBasInf = False

    '    bNam = PathNameOnly(pFileName) & "\" & FilenameOnly(pFileName) & ".inf"
    '    If FileExists(bNam) Then
    '      'read available BASINS .inf file
    '      bFun = FreeFile()
    '      FileOpen(bFun, bNam, OpenMode.Input)
    '      'reads location records from BASINS .inf file
    '      On Error GoTo ErrHandler
    '      Do While InStr(istr, "number of stations") <= 0 And Not EOF(bFun)
    '        istr = LineInput(bFun)
    '      Loop
    '      NLoc = CInt(Mid(istr, 1, 5))
    '      If NLoc > 0 Then 'valid number of locations read
    '        ReDim BasInf(NLoc - 1)
    '        istr = LineInput(bFun)
    '        istr = LineInput(bFun)
    '        For i = 0 To NLoc - 1
    '          istr = LineInput(bFun)
    '          ip = InStr(2, istr, New String(Chr(34), 1))
    '          If ip > 0 Then 'read description from between quotes
    '            BasInf(i).desc = Mid(istr, 2, ip - 2)
    '          End If
    '          BasInf(i).Nam = Trim(Mid(istr, 30, 10))
    '          BasInf(i).Elev = CSng(Mid(istr, 42, 10))
    '          BasInf(i).sdat(0) = CInt(Mid(istr, 54, 4))
    '          ip = 59
    '          For j = 1 To 3
    '            BasInf(i).sdat(j) = CInt(Mid(istr, ip, 2))
    '            ip = ip + 3
    '          Next j
    '          BasInf(i).edat(0) = CInt(Mid(istr, 69, 4))
    '          ip = 74
    '          For j = 1 To 3
    '            BasInf(i).edat(j) = CInt(Mid(istr, ip, 2))
    '            ip = ip + 3
    '          Next j
    '          BasInf(i).EvapCoef = CSng(Mid(istr, 86, 8))
    '          istr = LTrim(Mid(istr, 94))
    '          ip = 1
    '          j = 0
    '          Do While j <= 7 And ip > 0
    '            ip = InStr(istr, " ")
    '            If ip > 0 Then 'process next data-set number
    '              BasInf(i).dsn(j) = CInt(Mid(istr, 1, ip - 1))
    '              istr = LTrim(Mid(istr, ip))
    '            Else 'must be at end of string
    '              BasInf(i).dsn(j) = CInt(Mid(istr, 1))
    '            End If
    '            j = j + 1
    '          Loop
    '        Next i
    '      End If
    '      'read dataset variable names and descriptions
    '      ReDim DsnDescs(0)
    '      nDD = 0
    '      Do While Not EOF(bFun)
    '        ReDim Preserve DsnDescs(nDD)
    '        DsnDescs(nDD) = LineInput(bFun)
    '        nDD = nDD + 1
    '      Loop
    '      ReadBasInf = True
    '      FileClose(bFun)
    '    End If
    '    Exit Function

    'ErrHandler:
    '    MsgBox("Problem reading BASINS information file (.inf) associated with this WDM file." & vbCrLf & "Modifications made to this WDM file will not be reflected in the BASINS NPSM interface.", MsgBoxStyle.Information, "WDMUtil File Open")

    '  End Function

    'Private Sub HeaderFromBasinsInf(ByRef lData As atcTimeseries)
    '  Dim j, ist, i As Integer
    '  Dim lstr As String

    '  On Error GoTo 0
    '  With lData.Header
    '    .Sen = "OBSERVED"
    '    .con = lData.Attrib("TSTYPE")
    '    i = .id - 10
    '    ist = ((i - (i Mod 20)) / 20)
    '    If ist >= LBound(BasInf) And ist <= UBound(BasInf) Then
    '      lstr = BasInf(ist).Nam
    '      If Len(lstr) > 8 Then 'compress name to fit in 8 characters
    '        'try to weed out 00s from station ID
    '        j = InStr(lstr, "00")
    '        Do While j > 0 And Len(lstr) > 8
    '          If j > 1 Then
    '            lstr = Left(lstr, j - 1) & Mid(lstr, j + 2)
    '          Else
    '            lstr = Mid(lstr, j + 2)
    '          End If
    '        Loop
    '        If Len(lstr) > 8 Then 'couldn't weed out 00s to compress name
    '          lstr = Left(lstr, 8)
    '        End If
    '      End If
    '      .loc = lstr
    '      .desc = BasInf(ist).desc
    '    End If
    '  End With

    '  'save revisions to header back to dsn
    '  writeDataHeader(lData)

    'End Sub

    Public Overrides ReadOnly Property Category() As String
        Get
            Return "File"
        End Get
    End Property

    Public Overrides ReadOnly Property Description() As String
        Get
            Return "WDM Time Series"
        End Get
    End Property

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Timeseries::WDM"
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

    Public Overrides Function Open(ByVal aFileName As String, Optional ByVal aAttributes As atcDataAttributes = Nothing) As Boolean
        If aFileName Is Nothing OrElse aFileName.Length = 0 Then 'OrElse Not FileExists(aFileName) Then
            'aFileName = FindFile("Select WDM file to open", , , pFileFilter, True, , 1)
            Dim cdlg As New Windows.Forms.OpenFileDialog
            With cdlg
                .Title = "Select WDM file to open"
                .FileName = aFileName
                .Filter = pFileFilter
                .CheckFileExists = False
                If .ShowDialog() = Windows.Forms.DialogResult.OK Then
                    aFileName = AbsolutePath(.FileName, CurDir)
                Else 'cancel
                    Logger.Dbg("atcDataSourceWDM:Open:User Cancelled File Dialogue for Open WDM file")
                    Return False
                End If
            End With
        End If
        Dim lwdmhandle As atcWdmHandle
        If FileExists(aFileName) Then
            lwdmhandle = New atcWdmHandle(0, aFileName)
            'BasInfAvail = ReadBasInf()
        ElseIf FilenameNoPath(aFileName).Length > 0 Then
            Logger.Dbg("atcDataSourceWDM:Open:WDM file " & aFileName & " does not exist - it will be created")
            MkDirPath(PathNameOnly(aFileName))
            lwdmhandle = New atcWdmHandle(2, aFileName)
        Else
            Logger.Dbg("atcDataSourceWDM:Open:Problem opening WDM file '" & aFileName & "'")
            Return False
        End If
        If Not lwdmhandle Is Nothing AndAlso lwdmhandle.Unit > 0 Then
            Specification = aFileName
            pQuick = True
            Refresh(lwdmhandle.Unit)
            pQuick = False
            lwdmhandle.Dispose()
            Return True 'Successfully opened
        Else
            Logger.Dbg("atcDataSourceWDM:Open:Problem opening WDM file '" & aFileName & "'")
            Return False
        End If
    End Function

    Public Overrides Sub ReadData(ByVal aReadMe As atcDataSet)
        Dim v() As Single 'array of data values
        Dim dv() As Double 'array of data values
        Dim dd() As Double 'array of julian dates
        Dim nVals As Integer
        Dim iVal As Integer
        Dim lRetcod As Integer
        Dim lSdat(6) As Integer 'starting date
        Dim lEdat(6) As Integer 'ending (or current) date
        Dim lTsFill As Double

        If Not DataSets.Contains(aReadMe) Then
            Logger.Dbg("WDM cannot read dataset, not from this file" & vbCrLf & _
                       "Details:" & aReadMe.ToString)
        Else
            Dim lReadTS As atcTimeseries = aReadMe
            'Logger.dbg("WDM read data " & aReadMe.Attributes.GetValue("Location"))
            Dim lWdmHandle As New atcWdmHandle(0, Specification)
            'BasInfAvail = ReadBasInf()
            If lWdmHandle.Unit > 0 Then
                With aReadMe.Attributes
                    If Not CBool(.GetValue("HeaderComplete", False)) Then
                        DsnReadGeneral(lWdmHandle.Unit, aReadMe)
                    End If

                    If Not CBool(.GetValue("HeaderOnly", False)) Then
                        lReadTS.ValuesNeedToBeRead = False
                        lReadTS.Dates.ValuesNeedToBeRead = False

                        Dim lSJDay As Double = .GetValue("SJDay", 0)
                        J2Date(lSJDay, lSdat)
                        nVals = lReadTS.numValues
                        If nVals = 0 Then 'constant inverval???
                            Dim lEJDay As Double = .GetValue("EJDay", 0)
                            J2Date(lEJDay, lEdat)
                            timdif(lSdat, lEdat, .GetValue("tu", 0), .GetValue("ts", 0), nVals)
                            lReadTS.numValues = nVals
                        End If
                        If nVals > 0 Then
                            ReDim v(nVals)
                            Dim lDsn As Integer = CInt(.GetValue("id", 0))
                            Dim lTimeStep As Integer = CInt(.GetValue("ts", 0))
                            Dim lTimeUnits As Integer = CInt(.GetValue("tu", 0))
                            Dim lTran As Integer = 0 'transformation = aver,same
                            Dim lQual As Integer = 31 'allowed quality code
                            F90_WDTGET(lWdmHandle.Unit, lDsn, lTimeStep, lSdat(0), nVals, _
                                       lTran, lQual, lTimeUnits, v(1), lRetcod)

                            ReDim dd(nVals)
                            dd(0) = lSJDay
                            ReDim dv(nVals)
                            dv(0) = Double.NaN

                            lTsFill = .GetValue("TSFill", -999)
                            If lTsFill >= 0 Then 'WDM convention - fill value, not undefined,
                                lTsFill = Double.NaN
                            End If

                            Dim lInterval As Double = .GetValue("interval", 0)
                            Dim lConstInterval As Boolean = (Math.Abs(lInterval) > 0.00001)
                            For iVal = 1 To nVals
                                If (v(iVal) - lTsFill) < Double.Epsilon Then
                                    dv(iVal) = Double.NaN
                                Else
                                    dv(iVal) = v(iVal) 'TODO: test speed of this vs. using ReadDataset.Value(iVal) = v(iVal)
                                End If
                                If lConstInterval Then
                                    dd(iVal) = lSJDay + iVal * lInterval
                                Else
                                    TIMADD(lSdat, lTimeUnits, lTimeStep, iVal, lEdat)
                                    dd(iVal) = Date2J(lEdat)
                                End If
                            Next
                        Else
                            ReDim v(0)
                            ReDim dv(0)
                            ReDim dd(0)
                        End If
                        lReadTS.Values = dv
                        lReadTS.Dates.Values = dd
                    End If
                End With
                lWdmHandle.Dispose()
            End If
        End If
    End Sub

    Private Sub RefreshDsn(ByVal aFileUnit As Integer, ByRef aDsn As Integer)
        Dim lData As atcTimeseries
        Dim lDates As atcTimeseries
        Dim lDate As Date

        Dim lAttributeDefinition As atcAttributeDefinition

        Dim lS As String
        Dim lName As String
        Dim lInit As Integer
        Dim lSaind As Integer
        Dim lSaval(256) As Integer

        'Dim i As Integer

        lDates = New atcTimeseries(Nothing)
        pDates.Add(lDates)

        lData = New atcTimeseries(Me)
        lData.Dates = lDates
        lData.Attributes.SetValue("id", aDsn)
        lData.Attributes.SetValue("History 1", "Read from " & Specification)

        lData.ValuesNeedToBeRead = True
        lData.Dates.ValuesNeedToBeRead = True
        DataSets.Add(aDsn, lData)

        Call DsnReadGeneral(aFileUnit, lData)

        lInit = 1
        Do
            F90_GETATT(aFileUnit, aDsn, lInit, lSaind, lSaval(0))
            If lSaind > 0 Then 'process attribute
                If Not (AttrStored(lSaind)) Then
                    Try
                        lAttributeDefinition = pMsg.Attributes.ItemByIndex(lSaind)
                        lName = lAttributeDefinition.Name
                        lS = AttrVal2String(lSaind, lSaval)
                        Select Case UCase(lName)
                            Case "DATCRE", "DATMOD", "DATE CREATED", "DATE MODIFIED"
                                lDate = New Date(CInt(lS.Substring(0, 4)), _
                                                 CInt(lS.Substring(4, 2)), _
                                                 CInt(lS.Substring(6, 2)), _
                                                 CInt(lS.Substring(8, 2)), _
                                                 CInt(lS.Substring(10, 2)), _
                                                 CInt(lS.Substring(12, 2)))
                                lData.Attributes.SetValue(pMsg.Attributes.Item(lSaind), lDate)
                            Case "DCODE"
                                'lData.Attributes.SetValue(UnitsAttributeDefinition(True), GetUnitName(CInt(S)))
                            Case Else
                                lData.Attributes.SetValue(pMsg.Attributes.Item(lSaind), lS)
                        End Select
                    Catch
                    End Try
                End If
            Else 'all done
                Exit Do
            End If
        Loop

        'If Not lData.Attributes.isSet("Units") Then
        'lData.Attributes.SetValue(UnitsAttributeDefinition(True), "Unknown")
        'End If

        If pQuick Then
            lData.Attributes.SetValue("HeaderComplete", False) 'false, must do later when used
        End If

        'If lData.Attributes.GetValue("locn") = "<unk>" And BasInfAvail Then 'no location, try to check BASINS inf file
        'Call HeaderFromBasinsInf(lData)
        'End If

    End Sub

    'Before calling, make sure id property of aDataset has been set to dsn -- aDataset.Attributes.SetValue("id", dsn)
    Private Sub DsnReadGeneral(ByVal aFileUnit As Integer, ByRef aDataset As atcTimeseries)
        Dim saind, salen, retcod As Integer
        Dim ltu, lts, j As Integer, lNvals As Integer
        Dim lstr As String = ""
        Dim sdt(6) As Integer
        Dim edt(6) As Integer
        Dim dsfrc As Integer
        Dim dsn As Integer = aDataset.Attributes.GetValue("id", 0)

        salen = 1

        saind = 33 'time step
        Call F90_WDBSGI(aFileUnit, dsn, saind, salen, lts, retcod)
        If (retcod <> 0) Then ' set time step to default of 1
            lts = 1
        End If
        aDataset.Attributes.SetValue("ts", lts)

        saind = 17 'time units
        Call F90_WDBSGI(aFileUnit, dsn, saind, salen, ltu, retcod)
        If (retcod <> 0) Then 'set to default of daily time units
            ltu = 4
        End If
        aDataset.Attributes.SetValue("tu", ltu)

        'TODO: set constant interval/interval length attribute(s) in aDataset.Dates
        'lDateSum.CIntvl = True
        Select Case ltu
            Case 4 'day
                aDataset.Attributes.SetValue("interval", lts)
            Case 3 'hour
                aDataset.Attributes.SetValue("interval", lts / CDbl(24))
            Case 2 'minute
                aDataset.Attributes.SetValue("interval", lts / CDbl(1440))
        End Select

        If Not pQuick Then 'get start and end dates for each data set
            j = 1
            Call F90_WTFNDT(aFileUnit, dsn, j, dsfrc, sdt(0), edt(0), retcod)
            If sdt(0) > 0 Then
                timdif(sdt, edt, ltu, lts, lNvals)
                aDataset.Dates.Attributes.SetValue("SJDay", Date2J(sdt))
                aDataset.Dates.Attributes.SetValue("EJDay", Date2J(edt))
                aDataset.Attributes.SetValue("SJDay", Date2J(sdt))
                aDataset.Attributes.SetValue("EJDay", Date2J(edt))
                aDataset.numValues = lNvals
                aDataset.Dates.numValues = lNvals
            End If
        End If

        'get data-set scenario name
        saind = 288
        salen = 8
        Call F90_WDBSGC(aFileUnit, dsn, saind, salen, lstr)
        lstr = Trim(lstr)
        If Len(lstr) = 0 Then
            lstr = "<unk>"
        End If
        aDataset.Attributes.SetValue("scen", lstr)
        'get data-set location name
        saind = 290
        salen = 8
        Call F90_WDBSGC(aFileUnit, dsn, saind, salen, lstr)
        lstr = Trim(lstr)
        If Len(lstr) = 0 Then
            lstr = "<unk>"
        End If
        aDataset.Attributes.SetValue("locn", Trim(lstr))
        'get data-set constituent name
        saind = 289
        salen = 8
        Call F90_WDBSGC(aFileUnit, dsn, saind, salen, lstr)
        If Len(Trim(lstr)) = 0 Then 'try tstype for constituent
            saind = 1
            salen = 4
            Call F90_WDBSGC(aFileUnit, dsn, saind, salen, lstr)
        End If
        aDataset.Attributes.SetValue("cons", Trim(lstr))
        'station name
        Call F90_WDBSGC(aFileUnit, dsn, CInt(45), CInt(48), lstr)
        aDataset.Attributes.SetValue("desc", Trim(lstr))
        aDataset.Attributes.SetValue("HeaderComplete", True)
    End Sub

    Private Function AttrVal2String(ByRef saind As Integer, ByRef saval() As Integer) As String
        Dim lS As String
        Dim lI As Integer
        Dim lAttr As atcAttributeDefinition

        lAttr = pMsg.Attributes(saind)

        With lAttr
            lS = ""
            Select Case .TypeString
                Case "String"
                    For lI = 0 To (lAttr.Max / 4) - 1
                        lS &= Long2String(saval(lI))
                    Next
                Case "Single" : lS &= CStr(System.BitConverter.ToSingle(System.BitConverter.GetBytes((saval(lI))), 0))
                Case Else : lS &= CStr(saval(lI))
            End Select
        End With

        Return Trim(lS)
    End Function

    Protected Overrides Sub Finalize()
        'If pFileUnit > 0 Then
        '  Dim i As Integer = F90_WDMCLO(pFileUnit)
        'End If
        MyBase.Finalize()
    End Sub

End Class
