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
    Private pDates As ArrayList 'of atcTimeseries
    Private pQuick As Boolean
    Private Shared pMsg As atcMsgWDM

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

    Private Sub Clear()
        pErrorDescription = ""
        Specification = "<unknown>"
        pQuick = False

        pDates = Nothing
        pDates = New ArrayList
    End Sub

    '  oldID must be given if dataObject has a changed id
    Private Function writeDataHeader(ByRef aFileUnit As Integer, ByRef dataObject As atcTimeseries, Optional ByRef oldID As Integer = -1) As Boolean
        Dim salen, saind As Integer
        Dim c, S, l, d As String
        Dim dsn, lRetcod, i As Integer
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
                        d = lAttribs.GetValue("stanam")
                        If Len(d) > salen Then
                            Logger.Msg("Station name: '" & d & vbCr & "truncated to: " & Left(d, salen), "WDM Write Data Header", MsgBoxStyle.Exclamation)
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
        Dim lDsn As Integer
        Dim lProg As Integer = 0
        Dim lProgPrev As Integer

        DataSets.Clear()

        pDates = Nothing
        pDates = New ArrayList

        lDsn = 1
        While lDsn > 0
            F90_WDDSNX(aWdmUnit, lDsn) 'finds next dsn in use
            If lDsn > 0 Then
                If F90_WDCKDT(aWdmUnit, lDsn) = 1 Then
                    RefreshDsn(aWdmUnit, lDsn)
                End If
            End If
            If lDsn = -1 Then
                Logger.Progress("WDM Refresh Complete", 100, 0)
            Else 'try the next dsn
                lDsn += 1
                lProgPrev = lProg
                lProg = (100 * lDsn) / 32000
                Logger.Progress("WDM Refresh", lProg, lProgPrev)
            End If
        End While
    End Sub

    Private Function GetDataSetFromDsn(ByRef aDsn As Integer) As atcTimeseries
        For Each curDataset As atcTimeseries In DataSets
            If curDataset.Attributes.GetValue("id") = aDsn Then
                Return curDataset
            End If
        Next
        Logger.Msg("DSN " & aDsn & " does not exist.", "DataFileWDM.GetDataSetFromDsn")
        Return Nothing
    End Function

    Private Function AttrStored(ByRef aSaind As Integer) As Boolean 'somewhere else
        Select Case aSaind
            Case 17 : AttrStored = True 'tcode
                'Case 27: AttrStored = True 'tsbyr  'jlk commmented to fix winhspf problem
            Case 33 : AttrStored = True 'tsstep
                'Case 45 : AttrStored = True 'staname  'prh - don't force STANAM to be read
            Case 288 : AttrStored = True 'idscen
            Case 289 : AttrStored = True 'idlocn
            Case 290 : AttrStored = True 'idcons
                'Case 10 : AttrStored = True 'description
            Case Else : AttrStored = False
        End Select
    End Function

    Public Overrides Function AddDataset(ByVal aDataSet As atcData.atcDataSet, _
                                Optional ByVal aExistAction As atcData.atcDataSource.EnumExistAction = atcData.atcDataSource.EnumExistAction.ExistReplace) _
                                         As Boolean
        Logger.Dbg("atcDataSourceWdm:AddDataset:entry:" & aExistAction)
        Dim lWdmHandle As New atcWdmHandle(0, Specification)
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
            TimDif(lSDat, lEDat, lTu, lTs, lNValsExpected)
            If lNvals <> lNValsExpected Then
                'TODO:  make writing data smarter to deal with big gaps of missing, etc
                Throw New Exception("NVals:" & lNvals & ":" & lNValsExpected)
            End If

            'aDataSet.Attributes.CalculateAll() 'should we calculdate attributes on request, not here on creation

            Dim lDsn As Integer = aDataSet.Attributes.GetValue("id", 1)

            Logger.Dbg("atcDataSourceWdm:AddDataset:WdmUnit:Dsn:" & lWdmHandle.Unit & ":" & lDsn)

            If F90_WDCKDT(lWdmHandle.Unit, lDsn) > 0 Then 'dataset exists, what do we do?
                Logger.Dbg("atcDataSourceWdm:AddDataset:DatasetAlreadyExists")
                If aExistAction = ExistReplace Then
                    Dim lRet As Integer
                    Logger.Dbg("atcDataSourceWdm:AddDataset:ExistReplace:")
                    F90_WDDSDL(lWdmHandle.Unit, lDsn, lRet)
                    Logger.Dbg("atcDataSourceWdm:AddDataset:RemovedOld:" & lWdmHandle.Unit & ":" & lDsn & ":" & lRet)
                    Dim lReplaceThis As atcTimeseries = DataSets.ItemByKey(lDsn)
                    If Not lReplaceThis Is Nothing Then
                        DataSets.Remove(lReplaceThis)
                    End If
                ElseIf aExistAction = ExistAppend Then 'find dataset and try to append to it
                    Logger.Dbg("atcDataSourceWdm:AddDataset:ExistAppend:" & lWdmHandle.Unit & ":" & lDsn)
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
                Else 'use next available number
                    lDsn = findNextDsn(lDsn)
                    Logger.Dbg("atcDataSourceWdm:AddNew:" & lWdmHandle.Unit & ":" & lDsn)
                    aDataSet.Attributes.SetValue("Id", lDsn)
                End If
            Else
                Logger.Dbg("atcDataSourceWdm:AddDataset:NewDataSet:WdmUnit:Dsn:" & lWdmHandle.Unit & ":" & lDsn)
            End If

            Dim lWriteIt As Boolean = False
            If aExistAction = ExistAppend Then 'just write appended data
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
                    F90_WDTPUT(lWdmHandle.Unit, lDsn, lTs, lSDat(0), lNvals, CInt(1), CInt(0), lTu, lV(1), lRet)
                End If
                Logger.Dbg("atcDataSourceWdm:AddDataset:WDTPUT:back:" & _
                            lWdmHandle.Unit & ":" & lDsn & ":" & lRet)
            End If

            lWdmHandle.Dispose()
            Return True
        Catch ex As Exception
            Logger.Dbg("atcDataSourceWdm:AddDataSet:" & ex.ToString)
            lWdmHandle.Dispose()
            Return False
        End Try
    End Function

    Private Function findNextDsn(ByRef aDsn As Integer) As Integer
        Dim lNextDsn As Integer = aDsn
        For Each lDataset As atcDataSet In DataSets
            If lDataset.GetType.FullName = "atcData.atcTimeseries" Then
                If lNextDsn = lDataset.Attributes.GetValue("Id", lNextDsn) Then
                    lNextDsn = findNextDsn(aDsn + 1)
                    Exit For
                End If
            End If
        Next lDataset
        Return lNextDsn
    End Function

    Public Function RemoveDataset(ByVal aDataSet As atcData.atcDataSet) As Boolean
        Dim lTimser As atcTimeseries = aDataSet
        Dim lWdmHandle As New atcWdmHandle(0, Specification)
        Dim lRetcod As Integer

        Call F90_WDDSDL(lWdmHandle.Unit, (aDataSet.Attributes.GetValue("id")), lRetcod)
        If lRetcod = 0 Then
            RemoveTimSer = True
            DataSets.Remove(aDataSet)

            Dim lRemoveDate As Boolean = True
            Dim lSearchSerial As Integer = lTimser.Dates.Serial
            For Each lTs As atcTimeseries In DataSets
                If lTs.Dates.Serial = lSearchSerial Then
                    lRemoveDate = False
                    Exit For
                End If
            Next
            If lRemoveDate Then
                pDates.Remove(lTimser.Dates)
            End If
        Else
            RemoveTimSer = False
            pErrorDescription = "WDM:RemoveTimSer:DSN:" & aDataSet.Attributes.GetValue("id") & ":Retcod:" & lRetcod
            Logger.Dbg(pErrorDescription)
        End If
        lWdmHandle.Dispose()
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

    Private Function DsnBld(ByVal aFileUnit As Integer, ByRef aTs As atcTimeseries) As Boolean
        Dim lDsn, lNSasp, lNUp, lNDn, lNSa, lNDp As Integer
        Dim lDecade As Integer
        Dim lSaLen, lPsa, lIVal, lSaInd, lRetcod As Integer
        Dim lRVal As Single
        Dim lCSDat(6) As Integer
        Dim lStr As String
        Dim lTs As Integer
        Dim lTu As Integer

        Dim lMsgHandle As atcWdmHandle = pMsg.MsgHandle
        Dim lMsgUnit As Integer = lMsgHandle.Unit

        lDsn = aTs.Attributes.GetValue("id", 0)
        'create label
        lNDn = aTs.Attributes.GetValue("NDN", 10)
        lNUp = aTs.Attributes.GetValue("NUP", 10)
        lNSa = aTs.Attributes.GetValue("NSA", 30)
        lNSasp = aTs.Attributes.GetValue("NSASP", 100)
        lNDp = aTs.Attributes.GetValue("NDP", 300)
        F90_WDLBAX(aFileUnit, lDsn, 1, lNDn, lNUp, lNSa, lNSasp, lNDp, lPsa)

        'add needed attributes
        lSaInd = 1 'tstype
        lSaLen = 4
        lStr = aTs.Attributes.GetValue("TSTYPE", Left(aTs.Attributes.GetValue("cons"), 4))
        F90_WDBSAC(aFileUnit, lDsn, lMsgUnit, lSaInd, lSaLen, lRetcod, lStr, lStr.Length)
        lSaLen = 1
        lSaInd = 34 'tgroup
        lIVal = aTs.Attributes.GetValue("TGROUP", 6)
        F90_WDBSAI(aFileUnit, lDsn, lMsgUnit, lSaInd, lSaLen, lIVal, lRetcod)
        lSaInd = 83 'compfg
        lIVal = aTs.Attributes.GetValue("COMPFG", 1)
        F90_WDBSAI(aFileUnit, lDsn, lMsgUnit, lSaInd, lSaLen, lIVal, lRetcod)
        lSaInd = 84 'tsform
        lIVal = aTs.Attributes.GetValue("TSFORM", 1)
        F90_WDBSAI(aFileUnit, lDsn, lMsgUnit, lSaInd, lSaLen, lIVal, lRetcod)
        lSaInd = 34 'tsfill
        lRVal = aTs.Attributes.GetValue("TSFILL", -999)
        F90_WDBSAR(aFileUnit, lDsn, lMsgUnit, lSaInd, lSaLen, lRVal, lRetcod)
        lSaInd = 17 'tcode
        lStr = aTs.Attributes.GetValue("tu")
        If lStr.Length = 0 Then
            CalcTimeUnitStep(aTs.Dates.Value(0), aTs.Dates.Value(1), lTu, lTs)
            aTs.Attributes.SetValue("tu", lTu)
            aTs.Attributes.SetValue("ts", lTs)
        Else
            lTu = lStr
            lSaInd = 33 'tsstep
            lTs = aTs.Attributes.GetValue("ts")
        End If
        lSaInd = 17 'tcode
        F90_WDBSAI(aFileUnit, lDsn, lMsgUnit, lSaInd, lSaLen, lTu, lRetcod)
        lSaInd = 33 'tsstep
        F90_WDBSAI(aFileUnit, lDsn, lMsgUnit, lSaInd, lSaLen, lTs, lRetcod)
        lSaInd = 85 'vbtime
        lIVal = aTs.Attributes.GetValue("VBTIME", 1)
        If lTs > 1 Then lIVal = 2 'timestep > 1 vbtime must vary
        F90_WDBSAI(aFileUnit, lDsn, lMsgUnit, lSaInd, lSaLen, lIVal, lRetcod)
        J2Date(aTs.Dates.Value(0), lCSDat)
        lDecade = lCSDat(0) Mod 10
        If lDecade > 0 Then 'subtract back to start of this decade
            lIVal = lCSDat(0) - lDecade
        Else 'back to start of previous decade
            lIVal = lCSDat(0) - 10
        End If
        lSaInd = 27 'tsbyr
        F90_WDBSAI(aFileUnit, lDsn, lMsgUnit, lSaInd, lSaLen, lIVal, lRetcod)
        lSaLen = 8
        lSaInd = 288 'scenario
        lStr = UCase(Left(aTs.Attributes.GetValue("scen"), lSaLen))
        F90_WDBSAC(aFileUnit, lDsn, lMsgUnit, lSaInd, lSaLen, lRetcod, lStr, lStr.Length)
        lSaInd = 289 'constituent
        lStr = UCase(Left(aTs.Attributes.GetValue("cons"), lSaLen))
        F90_WDBSAC(aFileUnit, lDsn, lMsgUnit, lSaInd, lSaLen, lRetcod, lStr, lStr.Length)
        lSaInd = 290 'location
        lStr = UCase(Left(aTs.Attributes.GetValue("locn"), lSaLen))
        F90_WDBSAC(aFileUnit, lDsn, lMsgUnit, lSaInd, lSaLen, lRetcod, lStr, lStr.Length)
        lSaLen = 48
        lSaInd = 10 'description
        lStr = Left(aTs.Attributes.GetValue("desc"), lSaLen)
        F90_WDBSAC(aFileUnit, lDsn, lMsgUnit, lSaInd, lSaLen, lRetcod, lStr, lStr.Length)

        'others (from attrib)
        DsnBld = DsnWriteAttributes(aFileUnit, aTs)

        lMsgHandle.Dispose()
    End Function

    Private Function DsnWriteAttributes(ByRef aFileUnit As Integer, ByRef aTs As atcTimeseries) As Boolean
        Dim lName As String
        Dim lValue As Object
        Dim lDefinition As atcAttributeDefinition
        Dim lMsgDefinition As atcAttributeDefinition
        Dim lRetcod As Integer
        Dim lMsg As atcWdmHandle = pMsg.MsgHandle
        Dim lMsgUnit As Integer = lMsg.Unit
        Dim lDsn As Integer = aTs.Attributes.GetValue("id", 0)

        DsnWriteAttributes = True
        For lAttributeIndex As Integer = 0 To aTs.Attributes.Count - 1
            lDefinition = aTs.Attributes(lAttributeIndex).Definition
            lName = lDefinition.Name
            lValue = aTs.Attributes(lAttributeIndex).Value
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
                        If Len(lValue) > lMsgDefinition.Max Then
                            Logger.Dbg("Attribute '" & lMsgDefinition.Name & "' truncated from '" & lValue & "' to '" & Left(lValue, lMsgDefinition.Max) & "'")
                        End If
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
        If aFileName Is Nothing OrElse aFileName.Length = 0 Then
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
        Dim lV() As Single 'array of data values
        Dim lVd() As Double 'array of double data values
        Dim lJd() As Double 'array of julian dates
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
                        Dim nVals As Integer = lReadTS.numValues
                        If nVals = 0 Then 'constant inverval???
                            Dim lEJDay As Double = .GetValue("EJDay", 0)
                            J2Date(lEJDay, lEdat)
                            TimDif(lSdat, lEdat, .GetValue("tu", 0), .GetValue("ts", 0), nVals)
                            lReadTS.numValues = nVals
                        End If
                        If nVals > 0 Then
                            ReDim lV(nVals)
                            Dim lDsn As Integer = CInt(.GetValue("id", 0))
                            Dim lTimeStep As Integer = CInt(.GetValue("ts", 0))
                            Dim lTimeUnits As Integer = CInt(.GetValue("tu", 0))
                            Dim lTran As Integer = 0 'transformation = aver,same
                            Dim lQual As Integer = 31 'allowed quality code
                            F90_WDTGET(lWdmHandle.Unit, lDsn, lTimeStep, lSdat(0), nVals, _
                                       lTran, lQual, lTimeUnits, lV(1), lRetcod)

                            ReDim lJd(nVals)
                            lJd(0) = lSJDay
                            ReDim lVd(nVals)
                            lVd(0) = Double.NaN

                            lTsFill = .GetValue("TSFill", -999)
                            If lTsFill >= 0 Then 'WDM convention - fill value, not undefined,
                                lTsFill = Double.NaN
                            End If

                            Dim lInterval As Double = .GetValue("interval", 0)
                            Dim lConstInterval As Boolean = (Math.Abs(lInterval) > 0.00001)
                            For iVal As Integer = 1 To nVals
                                If (lV(iVal) - lTsFill) < Double.Epsilon Then
                                    lVd(iVal) = Double.NaN
                                Else
                                    lVd(iVal) = lV(iVal) 'TODO: test speed of this vs. using ReadDataset.Value(iVal) = v(iVal)
                                End If
                                If lConstInterval Then
                                    lJd(iVal) = lSJDay + iVal * lInterval
                                Else
                                    TIMADD(lSdat, lTimeUnits, lTimeStep, iVal, lEdat)
                                    lJd(iVal) = Date2J(lEdat)
                                End If
                            Next
                        Else
                            ReDim lV(0)
                            ReDim lVd(0)
                            ReDim lJd(0)
                        End If
                        lReadTS.Values = lVd
                        lReadTS.Dates.Values = lJd
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

        lDates = New atcTimeseries(Nothing)
        pDates.Add(lDates)

        lData = New atcTimeseries(Me)
        lData.Dates = lDates
        lData.Attributes.SetValue("id", aDsn)
        lData.Attributes.AddHistory("Read from " & Specification)

        lData.ValuesNeedToBeRead = True
        lData.Dates.ValuesNeedToBeRead = True
        DataSets.Add(aDsn, lData)

        DsnReadGeneral(aFileUnit, lData)

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
                    Catch ex As Exception
                        Logger.Dbg("RefreshDsn:" & aDsn & " Attr:" & lSaind & " Error:" & ex.ToString)
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
        Dim lSaInd, lSaLen, lRetcod As Integer
        Dim lTu, lTs As Integer
        Dim lNvals As Integer
        Dim lStr As String = ""
        Dim lSdat(6) As Integer
        Dim lEdat(6) As Integer
        Dim lDsn As Integer = aDataset.Attributes.GetValue("id", 0)

        lSaLen = 1
        lSaInd = 33 'time step
        F90_WDBSGI(aFileUnit, lDsn, lSaInd, lSaLen, lTs, lRetcod)
        If (lRetcod <> 0) Then ' set time step to default of 1
            lTs = 1
        End If
        aDataset.Attributes.SetValue("ts", lTs)

        lSaInd = 17 'time units
        F90_WDBSGI(aFileUnit, lDsn, lSaInd, lSaLen, lTu, lRetcod)
        If (lRetcod <> 0) Then 'set to default of daily time units
            lTu = 4
        End If
        aDataset.Attributes.SetValue("tu", lTu)

        'TODO: set constant interval/interval length attribute(s) in aDataset.Dates
        'lDateSum.CIntvl = True
        Select Case lTu
            Case 4 'day
                aDataset.Attributes.SetValue("interval", lTs)
            Case 3 'hour
                aDataset.Attributes.SetValue("interval", lTs / CDbl(24))
            Case 2 'minute
                aDataset.Attributes.SetValue("interval", lTs / CDbl(1440))
        End Select

        If Not pQuick Then 'get start and end dates for each data set
            Dim lGpFlg As Integer = 1
            Dim lDsfrc As Integer
            F90_WTFNDT(aFileUnit, lDsn, lGpFlg, lDsfrc, lSdat(0), lEdat(0), lRetcod)
            If lSdat(0) > 0 Then
                TimDif(lSdat, lEdat, lTu, lTs, lNvals)
                aDataset.Dates.Attributes.SetValue("SJDay", Date2J(lSdat))
                aDataset.Dates.Attributes.SetValue("EJDay", Date2J(lEdat))
                aDataset.Attributes.SetValue("SJDay", Date2J(lSdat))
                aDataset.Attributes.SetValue("EJDay", Date2J(lEdat))
                aDataset.numValues = lNvals
                aDataset.Dates.numValues = lNvals
            End If
        End If

        'get data-set scenario name
        lSaInd = 288
        lSaLen = 8
        F90_WDBSGC(aFileUnit, lDsn, lSaInd, lSaLen, lStr)
        If lStr.Length = 0 Then
            lStr = "<unk>"
        End If
        aDataset.Attributes.SetValue("scen", lStr)
        'get data-set location name
        lSaInd = 290
        lSaLen = 8
        F90_WDBSGC(aFileUnit, lDsn, lSaInd, lSaLen, lStr)
        If lStr.Length = 0 Then
            lStr = "<unk>"
        End If
        aDataset.Attributes.SetValue("locn", lStr)
        'get data-set constituent name
        lSaInd = 289
        lSaLen = 8
        F90_WDBSGC(aFileUnit, lDsn, lSaInd, lSaLen, lStr)
        If lStr.Length = 0 Then 'try tstype for constituent
            lSaInd = 1
            lSaLen = 4
            F90_WDBSGC(aFileUnit, lDsn, lSaInd, lSaLen, lStr)
        End If
        aDataset.Attributes.SetValue("cons", lStr)
        ''station name
        'F90_WDBSGC(aFileUnit, lDsn, CInt(45), CInt(48), lStr)
        'aDataset.Attributes.SetValue("stanam", lStr)
        ''description
        'F90_WDBSGC(aFileUnit, lDsn, CInt(10), CInt(80), lStr)
        'aDataset.Attributes.SetValue("descrp", lStr)
        'aDataset.Attributes.SetValue("HeaderComplete", True)
    End Sub

    Private Function AttrVal2String(ByRef aSaInd As Integer, ByRef aSaVal() As Integer) As String
        Dim lS As String
        Dim lI As Integer
        Dim lAttr As atcAttributeDefinition

        lAttr = pMsg.Attributes(aSaInd)
        With lAttr
            lS = ""
            Select Case .TypeString
                Case "String"
                    For lI = 0 To (lAttr.Max / 4) - 1
                        lS &= Long2String(aSaVal(lI))
                    Next
                Case "Single"
                    lS &= CStr(System.BitConverter.ToSingle(System.BitConverter.GetBytes((aSaVal(lI))), 0))
                Case Else
                    lS &= CStr(aSaVal(lI))
            End Select
        End With

        Return lS.Trim
    End Function

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
End Class
