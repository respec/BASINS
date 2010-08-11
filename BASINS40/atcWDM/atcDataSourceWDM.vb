'Copyright 2001-7 AQUA TERRA Consultants - Royalty-free use permitted under open source license
Option Strict Off
Option Explicit On

Imports atcData
Imports atcData.atcDataSource.EnumExistAction
Imports atcUtility
Imports MapWinUtility
Imports System.Collections.Specialized

Public Class atcDataSourceWDM
    Inherits atcData.atcTimeseriesSource

    Private Shared pFilter As String = "WDM Files (*.wdm)|*.wdm"
    Private pDates As Generic.List(Of atcTimeseries)
    Private pQuick As Boolean = False
    Private pNan As Double
    'Private pEpsilon As Double
    Private Shared pMsg As atcMsgWDM
    Private pTu As atcTimeUnit = 0 'default time units, default 2-minutes
    Private pTs As Integer = 0 'default timestep, default 1 
    Private pAskAboutMissingTuTs As Boolean = True

    'Can be set by user to avoid asking user each time
    Private pExistAskUserAction As EnumExistAction = ExistAskUser

    Public Overrides Sub Clear()
        MyBase.Clear()
        pQuick = False
        If pDates Is Nothing Then
            pDates = New Generic.List(Of atcTimeseries)
        Else
            pDates.Clear()
        End If
        SaveSetting("WDM", "AddDataset", "ExistAskUserAction", "")
    End Sub

    Public Sub New()
        MyBase.New()
        Filter = pFilter

        'kludge to force loading of system.double, removal may cause program to exit without message!
        pNan = GetNaN()
        'pEpsilon = System.Double.Epsilon

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
        pDates = New Generic.List(Of atcTimeseries)

        lDsn = 1
        While lDsn > 0
            F90_WDDSNX(aWdmUnit, lDsn) 'finds next dsn in use
            If lDsn > 0 Then
                If F90_WDCKDT(aWdmUnit, lDsn) = 1 Then
                    RefreshDsn(aWdmUnit, lDsn)
                End If
            Else
                'Logger.Dbg("Refresh " & lDsn)
            End If
            If lDsn > -1 Then 'try the next dsn
                lDsn += 1
                lProgPrev = lProg
                lProg = (100 * lDsn) / 32000
                'Logger.Progress("WDM Refresh", lProg, lProgPrev)
            End If
        End While
        'Logger.Progress("", 0, 0)
    End Sub

    'True if attribute is set by custom code and does not need to be processed along with all attributes
    Private Function AttrStored(ByVal aSaind As Integer) As Boolean 'somewhere else
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

    Public Overrides Function AddDatasets(ByVal aDataGroup As atcTimeseriesGroup) As Boolean
        Dim lfrm As New frmSave
        Dim lNumSaved As Integer = 0
        Dim lDSN As Integer
        Dim lLowestDSN As Integer = Integer.MaxValue
        Dim lHighestDSN As Integer = 0
        Dim lHighestNewDSN As Integer = 0
        Dim lLabel As String = FilenameNoPath(Specification) & " contains " & DataSets.Count & " datasets"

        If DataSets.Count > 0 Then
            For Each lDataSet As atcDataSet In DataSets
                lDSN = lDataSet.Attributes.GetValue("dsn", 0)
                If lDSN > 0 Then
                    If lDSN > lHighestDSN Then lHighestDSN = lDSN
                    If lDSN < lLowestDSN Then lLowestDSN = lDSN
                End If
            Next
            lLabel &= " numbered " & lLowestDSN & " to " & lHighestDSN
        End If

        lHighestNewDSN = lHighestDSN
        For Each lDataSet As atcDataSet In aDataGroup
            lDSN = lDataSet.Attributes.GetValue("dsn", 0)
            If lDSN > lHighestNewDSN AndAlso lDSN < 9999 Then
                lHighestNewDSN = lDSN
            Else
                lHighestNewDSN += 1
            End If
            lDataSet.Attributes.SetValue("New DSN", lHighestNewDSN)
        Next

        aDataGroup = lfrm.AskUser(aDataGroup, lLabel, lHighestDSN)
        If Not aDataGroup Is Nothing AndAlso aDataGroup.Count > 0 Then
            For Each lDataset As atcTimeseries In aDataGroup
                Dim lNewDSN As Integer = lDataset.Attributes.GetValue("New DSN", 0)
                If lNewDSN > 0 Then
                    lDataset.Attributes.SetValue("DSN", lNewDSN)
                    lDataset.Attributes.RemoveByKey("New DSN")
                End If
                If AddDataset(lDataset, ExistAskUser) Then lNumSaved += 1
            Next
            Dim lMsg As String = "Saved " & lNumSaved & " of " & aDataGroup.Count & " dataset"
            If aDataGroup.Count <> 1 Then lMsg &= "s"
            lMsg &= " in " & vbCrLf & Specification & vbCrLf & "(file now contains " & DataSets.Count & " dataset"
            If DataSets.Count <> 1 Then lMsg &= "s"
            Logger.Msg(lMsg & ")", "Saved Data")
            Return (lNumSaved = aDataGroup.Count)
        End If
        Return False
    End Function

    ''' <summary>
    ''' write timeseries data in dataset to wdm file
    ''' </summary>
    ''' <param name="aDataSet">dataset to write</param>
    ''' <param name="aExistAction">action to take if dataset with same id(dsn) already exists on wdm file</param>
    ''' <returns>true if successful, otherwise false</returns>
    ''' <remarks></remarks>
    Public Overrides Function AddDataset(ByVal aDataSet As atcData.atcDataSet, _
                                Optional ByVal aExistAction As atcData.atcDataSource.EnumExistAction = atcData.atcDataSource.EnumExistAction.ExistReplace) _
                                         As Boolean
        Logger.Dbg("atcDataSourceWdm:AddDataset:EntryWithAction:" & aExistAction & ":" & MemUsage())
        Dim lWdmHandle As New atcWdmHandle(0, Specification)
        Try
            Dim lTimser As atcTimeseries = aDataSet
            Dim lTimserConst As atcTimeseries = Nothing
            Dim lTs As Integer = lTimser.Attributes.GetValue("ts", 0)
            Dim lTu As atcTimeUnit = lTimser.Attributes.GetValue("tu", atcTimeUnit.TUUnknown)
            Dim lAggr As Integer = lTimser.Attributes.GetValue("aggregation", 0)
            Dim lDsn As Integer = aDataSet.Attributes.GetValue("id", 1)
            Dim lTGroup As Integer = aDataSet.Attributes.GetValue("tgroup", 6)
            Dim lTSBYr As Integer = aDataSet.Attributes.GetValue("tsbyr", 1900)

            If lTs = 0 OrElse lTu = atcTimeUnit.TUUnknown Then ' sparse dataset - fill in dummy values for write
                If pAskAboutMissingTuTs Then
                    Dim lFrmDefaultTimeInterval As New frmDefaultTimeInterval
                    pAskAboutMissingTuTs = lFrmDefaultTimeInterval.AskUser(lTimser.ToString & " #" & lDsn, pTu, pTs, lAggr)
                End If
                lTs = pTs
                lTu = pTu
                If pTs > 0 AndAlso pTu <> atcTimeUnit.TUUnknown Then
                    Dim lJulianInterval As Double
                    Select Case pTu
                        Case 2 : lJulianInterval = JulianMinute * lTs 'minute
                        Case 3 : lJulianInterval = JulianHour * lTs 'hour
                        Case 4 : lJulianInterval = lTs
                    End Select
                    lTimserConst = New atcTimeseries(Me)
                    With lTimserConst
                        .Attributes.ChangeTo(lTimser.Attributes)
                        .Dates = New atcTimeseries(Me)
                        Dim lNumValues As Integer
                        If lJulianInterval > 0 Then
                            .Dates.Value(0) = CInt(lTimser.Dates.Value(1)) - 1 'whole day portion
                            lNumValues = (lTimser.Dates.Value(lTimser.numValues) - .Dates.Value(0)) / lJulianInterval
                        Else
                            .Dates.Value(0) = TimAddJ(lTimser.Dates.Value(1), lTu, lTs, -1) 'for monthly data
                            lNumValues = lTimser.numValues
                        End If
                        .numValues = lNumValues
                        .Attributes.SetValue("ts", lTs)
                        .Attributes.SetValue("tu", lTu)
                        Dim lIndex As Integer = 1
                        For lIndexConst As Integer = 1 To lNumValues
                            Dim lDate As Double
                            If lJulianInterval > 0 Then
                                lDate = .Dates.Values(0) + (lJulianInterval * lIndexConst)
                            Else
                                lDate = TimAddJ(.Dates.Value(0), lTu, lTs, lIndexConst)
                            End If
                            Dim lAggregationCount As Integer = .ValueAttributes(lIndexConst).GetValue("AggregationCount", 0)
                            .Dates.Values(lIndexConst) = lDate
                            If lIndex <= lTimser.numValues AndAlso lDate >= (lTimser.Dates.Values(lIndex) - JulianSecond) Then
                                'todo: this uses only the last value, have a transformation 
                                lAggregationCount += 1
                                .ValueAttributes(lIndexConst).Add("AggregationCount", lAggregationCount)
                                Dim lValue As Double = lTimser.Values(lIndex)
                                If lAggregationCount = 1 Then 'save this first value
                                    .Values(lIndexConst) = lValue
                                Else
                                    Select Case lAggr
                                        Case 0 'aver
                                            .Value(lIndexConst) = ((.Value(lIndexConst) * (lAggregationCount - 1)) + lValue) / lAggregationCount
                                        Case 1 'sum
                                            .Value(lIndexConst) += lValue
                                        Case 2 'min
                                            If lValue < .Value(lIndexConst) Then
                                                .Value(lIndexConst) = lValue
                                            End If
                                        Case 3 'max
                                            If lValue > .Value(lIndexConst) Then
                                                .Value(lIndexConst) = lValue
                                            End If
                                        Case 4 'first - no change needed
                                        Case 5 'last
                                            .Value(lIndexConst) = lValue
                                    End Select
                                End If
                                lIndexConst -= 1 ' more for this value?
                                lIndex += 1 'increment the source
                            ElseIf lIndex > lTimser.numValues Then
                                Logger.Dbg("OutOfValuesAt:" & lTimser.numValues)
                            ElseIf lAggregationCount = 0 Then
                                .Values(lIndexConst) = pNan
                            End If
                        Next
                    End With
                    'Logger.Dbg(MemUsage)
                    lTimser = lTimserConst
                    'Logger.Dbg(MemUsage)
                Else
                    Throw New ApplicationException("No Time Units or Time Step on source dataset")
                End If
            End If

            Dim lNvals As Integer = lTimser.numValues
            Dim lSJDay As Double = lTimser.Attributes.GetValue("SJDay", 0)
            Dim lSDat(5) As Integer
            J2Date(lSJDay, lSDat)
            Dim lEJDay As Double = lTimser.Attributes.GetValue("EJDay", 0)
            Dim lEDat(5) As Integer
            J2Date(lEJDay, lEDat)
            Dim lNValsExpected As Integer
            TimDif(lSDat, lEDat, lTu, lTs, lNValsExpected)
            If lNvals < lNValsExpected Then
                'TODO:  make writing data smarter to deal with big gaps of missing, etc
                Throw New ApplicationException("NVals:" & lNvals & ":" & lNValsExpected & vbCrLf & _
                                               "in " & Specification & vbCrLf & _
                                               "dsn " & lTimser.Attributes.GetValue("ID", 0))
            End If

            'aDataSet.Attributes.CalculateAll() 'should we calculdate attributes on request, not here on creation

            'Logger.Dbg("atcDataSourceWdm:AddDataset:WdmUnit:Dsn:" & lWdmHandle.Unit & ":" & lDsn)

            Dim lDsnExists As Integer = F90_WDCKDT(lWdmHandle.Unit, lDsn)
            If lDsnExists > 0 Then 'dataset exists, what do we do?
                'Logger.Dbg("atcDataSourceWdm:AddDataset:DatasetAlreadyExists")
                Dim lExistTimser As atcTimeseries = DataSets.ItemByKey(lDsn)
                'Change asking user into what the user already chose for all
                If aExistAction = ExistAskUser Then aExistAction = pExistAskUserAction
                Select Case aExistAction
                    Case ExistNoAction
                        'Don't add, take no action
                        Logger.Dbg("DSN conflict:ExistNoAction:not added")
                        Return True
                    Case ExistAskUser
                        Select Case Logger.MsgCustomCheckbox("Existing dataset '" & lExistTimser.ToString & "'" & vbCrLf _
                                                           & "has same data set number as" & vbCrLf _
                                                           & "new dataset '" & lTimser.ToString & "'", _
                                                             "Dataset Number Conflict", _
                                                             "Use this answer for all datasets", "WDM", "AddDataset", "ExistAskUserAction", _
                                                             "Replace Existing", "Renumber New", "Discard New") ', "Append new to Existing")
                            Case "Replace Existing" : GoTo CaseExistReplace
                            Case "Renumber New" : GoTo CaseExistRenumber
                            Case "Discard New"
                                Logger.Dbg("Asked user to resolve DSN conflict, chose not to replace or renumber, not added")
                                Return False
                        End Select
                    Case ExistReplace
CaseExistReplace:
                        'Logger.Dbg("atcDataSourceWdm:AddDataset:ExistReplace:")
                        Dim lRet As Integer
                        F90_WDDSDL(lWdmHandle.Unit, lDsn, lRet)
                        'Logger.Dbg("atcDataSourceWdm:AddDataset:RemovedOld:" & lWdmHandle.Unit & ":" & lDsn & ":" & lRet)
                        If lExistTimser IsNot Nothing Then
                            DataSets.Remove(lExistTimser)
                        End If
                    Case ExistAppend  'find dataset and try to append to it
                        'Logger.Dbg("atcDataSourceWdm:AddDataset:ExistAppend:" & lWdmHandle.Unit & ":" & lDsn)
                        If lTimser.numValues > 0 AndAlso _
                           lExistTimser.numValues > 0 AndAlso _
                           lTimser.Dates.Value(1) <= lExistTimser.Dates.Value(lExistTimser.numValues) Then
                            Throw New ApplicationException("atcDataSourceWDM:AddDataset: Unable to append new TSer " & _
                                                lTimser.ToString & " to existing TSer " & _
                                                lExistTimser.ToString & vbCrLf & _
                                                "New TSer start date (" & lTimser.Dates.Value(1) & _
                                                ") preceeds exising TSer end date (" & _
                                                lExistTimser.Dates.Value(lExistTimser.numValues) & ")")
                        End If
                    Case ExistRenumber 'use next available number
CaseExistRenumber:
                        lDsn = findNextDsn(lDsn)
                        'Logger.Dbg("atcDataSourceWdm:AddNew:" & lWdmHandle.Unit & ":" & lDsn)
                        aDataSet.Attributes.SetValue("Id", lDsn)
                End Select
            Else
                'Logger.Dbg("atcDataSourceWdm:AddDataset:NewDataSet:WdmUnit:Dsn:" & lWdmHandle.Unit & ":" & lDsn)
            End If

            Dim lWriteIt As Boolean = False
            If lDsnExists > 0 And aExistAction = ExistAppend Then 'just write appended data
                lWriteIt = True
            ElseIf DsnBld(lWdmHandle.Unit, lTimser) Then
                DataSets.Add(lDsn, lTimser)
                lWriteIt = True
            End If

            If lWriteIt AndAlso lNvals > 0 Then
                Dim lTSFill As Double = lTimser.Attributes.GetValue("tsfill", -999)
                If Double.IsNaN(lTSFill) Then lTSFill = -999
                Dim lValue As Double
                Dim lV(lNvals) As Single
                Dim lRet As Integer
                For i As Integer = 1 To lNvals
                    lValue = lTimser.Value(i)
                    If Double.IsNaN(lValue) Then lValue = lTSFill
                    lV(i - 1) = lValue
                Next

                'J2DateRoundup(lTimser.Dates.Value(0), lTu, lSDat)
                J2DateRounddown(lTimser.Dates.Value(0), lTu, lSDat)

                'Logger.Dbg("atcDataSourceWdm:AddDataset:WDTPUT:call:" & _
                '            lWdmHandle.Unit & ":" & lDsn & ":" & lTs & ":" & lNvals & ":" & _
                '            lSDat(0) & ":" & lSDat(1) & ":" & lSDat(2) & ":" & lRet)
                If lNvals > 0 Then
                    'F90_WDTPUT(lWdmHandle.Unit, lDsn, lTs, lSDat(0), lNvals, CInt(1), CInt(0), lTu, lV(1), lRet)
                    F90_WDTPUT(lWdmHandle.Unit, lDsn, lTs, lSDat, lNvals, CInt(1), CInt(0), lTu, lV, lRet)
                End If
                If Not lTimserConst Is Nothing Then
                    lTimserConst.Clear() 'TODO: maybe just part?
                End If
                If lRet <> 0 Then
                    Throw New ApplicationException("WDTPUT:call:" & _
                                lWdmHandle.Unit & ":" & lDsn & ":" & lTs & ":" & lNvals & ":" & _
                                lSDat(0) & ":" & lSDat(1) & ":" & lSDat(2) & ":" & lRet)
                    'Logger.Dbg("atcDataSourceWdm:AddDataset:WDTPUT:back:" & _
                    '            lWdmHandle.Unit & ":" & lDsn & ":" & lRet)
                End If
                lWdmHandle.Dispose()
                Return True
            Else
                lWdmHandle.Dispose()
                Return False
            End If

        Catch ex As Exception
            Logger.Dbg("atcDataSourceWdm:AddDataSet:" & ex.ToString)
            lWdmHandle.Dispose()
            Return False
        End Try
    End Function

    Public Function WriteAttributes(ByVal aDataSet As atcData.atcDataSet) As Boolean
        'Logger.Dbg("atcDataSourceWdm:WriteAttributes:entry:" & aDataSet.ToString)
        Dim lWdmHandle As New atcWdmHandle(0, Specification)
        WriteAttributes = DsnWriteAttributes(lWdmHandle.Unit, aDataSet)
        lWdmHandle.Dispose()
        'Logger.Dbg("atcDataSourceWdm:WriteAttributes:end")
    End Function

    ''' <summary>
    ''' Write one attribute of a data set in memory to the dataset on the wdm file
    ''' </summary>
    ''' <param name="aDataSet">Data set which the attribute applies to</param>
    ''' <param name="aAttribute">Attribute to write</param>
    ''' <param name="aNewValue">New value for attribute (Optional)</param>
    ''' <returns>True on success, False on failure</returns>
    ''' <remarks>Use WriteAttributes to write all attributes</remarks>
    Public Function WriteAttribute(ByVal aDataSet As atcData.atcDataSet, _
                                   ByVal aAttribute As atcDefinedValue, _
                                   Optional ByVal aNewValue As Object = Nothing) As Boolean
        'Logger.Dbg("atcDataSourceWdm:WriteAttributes:entry:" & aDataSet.ToString)
        Dim lWdmHandle As New atcWdmHandle(0, Specification)
        Dim lMsg As atcWdmHandle = pMsg.MsgHandle
        Dim lDsn As Integer = aDataSet.Attributes.GetValue("id", 1)

        If Not aNewValue Is Nothing Then
            aAttribute.Value = aNewValue
        End If

        WriteAttribute = DsnWriteAttribute(lWdmHandle.Unit, lMsg.Unit, lDsn, aAttribute)

        lMsg.Dispose()
        lWdmHandle.Dispose()
        'Logger.Dbg("atcDataSourceWdm:WriteAttributes:end")
    End Function

    Private Function findNextDsn(ByVal aDsn As Integer) As Integer
        Dim lDatasets As atcTimeseriesGroup = DataSets
        While lDatasets.Keys.Contains(aDsn)
            aDsn += 1
        End While
        Return aDsn
    End Function

    Public Overrides Function RemoveDataset(ByVal aDataSet As atcData.atcDataSet) As Boolean
        Dim lRemoveDataset As Boolean = False

        Dim lRetcod As Integer
        Dim lWdmHandle As New atcWdmHandle(0, Specification)
        Call F90_WDDSDL(lWdmHandle.Unit, (aDataSet.Attributes.GetValue("id", 1)), lRetcod)
        lWdmHandle.Dispose()

        If lRetcod = 0 Then
            lRemoveDataset = True
            DataSets.Remove(aDataSet)

            Dim lRemoveDate As Boolean = True
            Dim lTimser As atcTimeseries = aDataSet
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
            lRemoveDataset = False
            Logger.Dbg("WDM:RemoveDataset:DSN:" & aDataSet.Attributes.GetValue("id", 1) & ":Retcod:" & lRetcod)
        End If
        Return lRemoveDataset
    End Function

    Public Overrides Function Save(ByVal SaveFileName As String, _
                          Optional ByVal ExistAction As EnumExistAction = EnumExistAction.ExistReplace) As Boolean
        If SaveFileName.ToLower.Equals(Me.Specification.ToLower) Then
            Return True
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' Put a new dataset in the WDM file from aTs
    ''' </summary>
    ''' <param name="aFileUnit">WDM file handle</param>
    ''' <param name="aTs">data set to add to WDM</param>
    ''' <returns></returns>
    Private Function DsnBld(ByVal aFileUnit As Integer, _
                            ByVal aTs As atcTimeseries) As Boolean

        If aTs.Attributes.GetValue("tu", atcTimeUnit.TUDay) = atcTimeUnit.TUYear Then
            'Provide more attribute space and less data pointer space for yearly timeseries
            With aTs.Attributes
                .SetValueIfMissing("TGROUP", 7)
                .SetValueIfMissing("TSBYR", 1700)
                .SetValueIfMissing("NSA", 100)
                .SetValue("NSASP", 130)
                .SetValue("NDP", 120)
            End With
        End If

        Dim lDsn As Integer = aTs.Attributes.GetValue("id", 1)
        Dim lNDn As Integer = aTs.Attributes.GetValue("NDN", 10)
        Dim lNUp As Integer = aTs.Attributes.GetValue("NUP", 10)
        Dim lNSa As Integer = aTs.Attributes.GetValue("NSA", 30)
        Dim lNSasp As Integer = aTs.Attributes.GetValue("NSASP", 100)
        Dim lNDp As Integer = aTs.Attributes.GetValue("NDP", 300)
        Dim lPsa As Integer

        'create label on wdm file
        F90_WDLBAX(aFileUnit, lDsn, 1, lNDn, lNUp, lNSa, lNSasp, lNDp, lPsa)
        'write attributes
        Return DsnWriteAttributes(aFileUnit, aTs)
    End Function

    Private Function DsnWriteAttributes(ByVal aFileUnit As Integer, _
                                        ByVal aTs As atcTimeseries) As Boolean
        Dim lDsn As Integer = aTs.Attributes.GetValue("id", 1)
        'add needed attributes
        Dim lStr As String = aTs.Attributes.GetValue("cons", "")
        If lStr.Length > 4 Then
            lStr = lStr.Substring(0, 4)
        End If
        aTs.Attributes.SetValueIfMissing("TSTYPE", lStr)
        aTs.Attributes.SetValueIfMissing("TGROUP", 6)
        aTs.Attributes.SetValueIfMissing("COMPFG", 1)
        aTs.Attributes.SetValueIfMissing("TSFORM", 1)
        aTs.Attributes.SetValueIfMissing("TSFILL", -999)
        Dim lTs As Integer
        Dim lTu As atcTimeUnit
        If Not aTs.Attributes.ContainsAttribute("tu") Then
            CalcTimeUnitStep(aTs.Dates.Value(0), aTs.Dates.Value(1), lTu, lTs)
            aTs.Attributes.SetValue("tu", lTu)
            aTs.Attributes.SetValue("ts", lTs)
        Else
            lTu = aTs.Attributes.GetValue("tu")
            lTs = aTs.Attributes.GetValue("ts")
        End If
        Dim lIVal As Integer = aTs.Attributes.GetValue("VBTIME", 1)
        If lTs > 1 Then
            lIVal = 2 'timestep > 1 vbtime must vary
        End If
        aTs.Attributes.SetValueIfMissing("VBTIME", lIVal)

        If lTu < atcTimeUnit.TUYear Then
            Dim lCSDat(6) As Integer
            If aTs.Dates Is Nothing OrElse aTs.Dates.numValues = 0 Then 'no dates in timeseries
                lCSDat(0) = 1950
            Else
                J2Date(aTs.Dates.Value(0), lCSDat)
            End If

            Dim lDecade As Integer = lCSDat(0) Mod 10
            If lDecade > 0 Then 'subtract back to start of this decade
                lIVal = lCSDat(0) - lDecade
            Else 'back to start of previous decade
                lIVal = lCSDat(0) - 10
            End If
            aTs.Attributes.SetValue("TSBYR", lIVal)
        Else
            aTs.Attributes.SetValueIfMissing("TSBYR", 1700)
        End If

        aTs.Attributes.SetValueIfMissing("scen", "")
        aTs.Attributes.SetValueIfMissing("cons", "")
        aTs.Attributes.SetValueIfMissing("locn", "")
        aTs.Attributes.SetValueIfMissing("desc", "")

        DsnWriteAttributes = True
        Dim lMsg As atcWdmHandle = pMsg.MsgHandle
        Dim lMsgUnit As Integer = lMsg.Unit
        For lAttributeIndex As Integer = 0 To aTs.Attributes.Count - 1
            If Not DsnWriteAttribute(aFileUnit, lMsgUnit, lDsn, aTs.Attributes(lAttributeIndex)) Then
                DsnWriteAttributes = False
                Exit For
            End If
        Next
        lMsg.Dispose()
    End Function

    Private Function DateToyyyyMMddHHmmss(ByVal aDate As Date) As String
        Return aDate.ToString("yyyyMMddHHmmss")
    End Function

    ''' <summary>
    ''' Write an attribute to the WDM file
    ''' </summary>
    ''' <returns>True if attribute was written OR if attribute is not in message file</returns>
    ''' <remarks>Only attributes in WDM message file can be written to WDM files</remarks>
    Private Function DsnWriteAttribute(ByVal aFileUnit As Integer, _
                                       ByVal aMsgUnit As Integer, _
                                       ByVal aDsn As Integer, _
                                       ByVal aAttribute As atcDefinedValue) As Boolean
        Dim lRetcod As Integer
        Dim lName As String = aAttribute.Definition.Name
        Dim lValue As Object = aAttribute.Value

        DsnWriteAttribute = True

        'TODO: integrate aliases lists here and in atcDataAttributes and atcMsgWDM
        Select Case (lName.ToLower)
            'Case "units"   'store Units ID as DCODE in WDM
            '    lName = "DCODE"
            '    lValue = CStr(GetUnitID(lValue))
            Case "latitude" : lName = "latdeg"
            Case "longitude" : lName = "lngdeg"
            Case "skew" : lName = "skewcf"
            Case "standard deviation" : lName = "stddev"
            Case "mean" : lName = "meanvl"
            Case "min" : lName = "minval"
            Case "max" : lName = "maxval"
            Case "count positive" : lName = "nonzro"
            Case "count zero" : lName = "numzro"

            Case "time unit" : lName = "tcode"
            Case "time step" : lName = "tsstep"
            Case "scenario" : lName = "idscen"
            Case "location" : lName = "idlocn"
            Case "constituent" : lName = "idcons"
            Case "description" : lName = "descrp"
            Case "date created", "datcre"
                lName = "datcre"
                lValue = DateToyyyyMMddHHmmss(lValue)
            Case "date modified", "datmod"
                lName = "datmod"
                lValue = DateToyyyyMMddHHmmss(lValue)
            Case "tsfill" 'Avoid writing NaN values in WDM file to make old tools happy
                If Double.IsNaN(lValue) Then lValue = -999

            Case "1low2" : lName = "L01002"
            Case "1low10" : lName = "L01010"
            Case "1low20" : lName = "L01020"

            Case "3low2" : lName = "L03002"
            Case "3low10" : lName = "L03010"
            Case "3low20" : lName = "L03020"

            Case "4low3" : lName = "L04003"

            Case "7low2" : lName = "L07002"
            Case "7low5" : lName = "L07005"
            Case "7low10", "7q10" : lName = "L07010"
            Case "7low20" : lName = "L07020"
            Case "7low50" : lName = "L07050"
            Case "7low100" : lName = "L07100"

            Case "14low2" : lName = "L14002"
            Case "14low10" : lName = "L14010"
            Case "14low20" : lName = "L14020"

            Case "30low2" : lName = "L30002"
            Case "30low10" : lName = "L30010"
            Case "30low20" : lName = "L30020"

            Case "90low2" : lName = "L90002"
            Case "90low10" : lName = "L90010"
            Case "90low20" : lName = "L90020"

            Case "1high2" : lName = "H01002"
            Case "1high5" : lName = "H01005"
            Case "1high10" : lName = "H01010"
            Case "1high20" : lName = "H01020"
            Case "1high25" : lName = "H01025"
            Case "1high50" : lName = "H01050"
            Case "1high100" : lName = "H01100"

            Case "3high2" : lName = "H03002"
            Case "3high5" : lName = "H03005"
            Case "3high10" : lName = "H03010"
            Case "3high20" : lName = "H03020"
            Case "3high25" : lName = "H03025"
            Case "3high50" : lName = "H03050"
            Case "3high100" : lName = "H03100"

            Case "7high2" : lName = "H07002"
            Case "7high5" : lName = "H07005"
            Case "7high10" : lName = "H07010"
            Case "7high20" : lName = "H07020"
            Case "7high25" : lName = "H07025"
            Case "7high50" : lName = "H07050"
            Case "7high100" : lName = "H07100"

            Case "15high2" : lName = "H15002"
            Case "15high5" : lName = "H15005"
            Case "15high10" : lName = "H15010"
            Case "15high20" : lName = "H15020"
            Case "15high25" : lName = "H15025"
            Case "15high50" : lName = "H15050"
            Case "15high100" : lName = "H15100"

            Case "30high2" : lName = "H30002"
            Case "30high5" : lName = "H30005"
            Case "30high10" : lName = "H30010"
            Case "30high20" : lName = "H30020"
            Case "30high25" : lName = "H30025"
            Case "30high50" : lName = "H30050"
            Case "30high100" : lName = "H30100"
        End Select

        Dim lMsgDefinition As atcAttributeDefinition = pMsg.Attributes.ItemByKey(lName.ToLower)

        If lMsgDefinition Is Nothing Then
            'Logger.Dbg("NoAttributeDefinition for " & lName)
        Else
            Select Case lMsgDefinition.TypeString
                Case "Integer"
                    If IsNumeric(lValue) Then
                        F90_WDBSAI(aFileUnit, aDsn, aMsgUnit, lMsgDefinition.ID, 1, CInt(lValue), lRetcod)
                    End If
                Case "Single"
                    If IsNumeric(lValue) Then
                        F90_WDBSAR(aFileUnit, aDsn, aMsgUnit, lMsgDefinition.ID, 1, CSng(lValue), lRetcod)
                    End If
                Case Else 'character
                    Dim lStr As String = lValue
                    If lStr.Length > lMsgDefinition.Max Then
                        lStr = lStr.Substring(0, lMsgDefinition.Max)
                        'Logger.Dbg("Attribute '" & lMsgDefinition.Name & "' truncated from '" & lValue & "' to '" & lStr & "'")
                    ElseIf lStr.Length < lMsgDefinition.Max Then
                        lStr = lStr.PadRight(lMsgDefinition.Max)
                    End If
                    F90_WDBSAC(aFileUnit, aDsn, aMsgUnit, lMsgDefinition.ID, lMsgDefinition.Max, lRetcod, lStr, lStr.Length)
            End Select
            'Logger.Dbg("Data Attribute to WDM: Attribute:" & lName & ", Value:" & lValue & ", Retcod:" & lRetcod)
            If lRetcod <> 0 Then
                If Math.Abs(lRetcod) = 104 Then 'cant update if data already present
                    Logger.Dbg("Skip:" & lName & ", data present")
                Else
                    Logger.Dbg("Unable to Write Data Attribute to WDM" & vbCrLf & _
                               "Attribute:" & lName & ", Value:" & lValue & ", Retcod:" & lRetcod)
                    DsnWriteAttribute = False
                End If
            End If
        End If
    End Function

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

    Public Overrides ReadOnly Property CanRemoveDataset() As Boolean
        Get
            Return True
        End Get
    End Property

    Public Overrides Function Open(ByVal aFileName As String, _
                          Optional ByVal aAttributes As atcDataAttributes = Nothing) _
                                   As Boolean

        If MyBase.Open(aFileName, aAttributes) Then
            Dim lWdmHandle As atcWdmHandle
            If FileExists(Specification) Then
                lWdmHandle = New atcWdmHandle(0, Specification)
            ElseIf IO.Path.GetFileName(Specification).Length > 0 Then
                Logger.Dbg("atcDataSourceWDM:Open:WDM file " & Specification & " does not exist - it will be created")
                MkDirPath(PathNameOnly(Specification))
                lWdmHandle = New atcWdmHandle(2, Specification)
            Else
                Logger.Dbg("atcDataSourceWDM:Open:Problem opening WDM file '" & Specification & "'")
                Return False
            End If

            If Not lWdmHandle Is Nothing AndAlso lWdmHandle.Unit > 0 Then
                pQuick = True
                Refresh(lWdmHandle.Unit)
                pQuick = False
                lWdmHandle.Dispose()
                Return True 'Successfully opened
            Else
                Logger.Dbg("atcDataSourceWDM:Open:Problem opening WDM file '" & Specification & "'")
                Return False
            End If
        Else
            Return False
        End If
    End Function

    Public Overrides Sub ReadData(ByVal aReadMe As atcDataSet)
        Dim lV() As Single 'array of data values
        Dim lVd() As Double 'array of double data values
        Dim lJd() As Double 'array of julian dates
        Dim lRetcod As Integer
        Dim lSdat(6) As Integer 'starting date
        Dim lSdatSeasonOffset(6) As Integer 'starting date with seasonal offset
        Dim lEdat(6) As Integer 'ending (or current) date
        Dim lTsFill As Double

        If Not DataSets.Contains(aReadMe) Then
            Logger.Dbg("WDM cannot read dataset with details:" & aReadMe.ToString & vbCrLf & _
                       "Specification:'" & Specification & "'")
        Else
            Dim lReadTS As atcTimeseries = aReadMe
            lReadTS.ValuesNeedToBeRead = False
            'Logger.dbg("WDM read data " & aReadMe.Attributes.GetValue("Location"))
            Dim lWdmHandle As New atcWdmHandle(0, Specification)
            If lWdmHandle.Unit > 0 Then
                With aReadMe.Attributes
                    If Not CBool(.GetValue("HeaderComplete", False)) Then
                        DsnReadGeneral(lWdmHandle.Unit, lReadTS)
                    End If

                    If Not CBool(.GetValue("HeaderOnly", False)) Then
                        lReadTS.ValuesNeedToBeRead = False

                        Dim lSJDay As Double = .GetValue("SJDay", 0)
                        J2Date(lSJDay, lSdat) 'saved starting date as array for wdtget
                        If .GetValue("TU") = atcUtility.atcTimeUnit.TUYear Then
                            Dim lStartMonth As Integer = .GetValue("SEASBG", 1)
                            Dim lStartDay As Integer = .GetValue("SEADBG", 1)
                            If lStartMonth > 1 OrElse lStartDay > 1 Then
                                lSJDay = TimAddJ(lSJDay, atcTimeUnit.TUMonth, 1, lStartMonth - 1) + lStartDay - 1
                            End If
                        End If
                        J2Date(lSJDay, lSdatSeasonOffset)
                        Dim nVals As Integer = lReadTS.numValues
                        If nVals = 0 Then 'constant inverval???
                            Dim lEJDay As Double = .GetValue("EJDay", 0)
                            J2Date(lEJDay, lEdat)
                            TimDif(lSdat, lEdat, .GetValue("tu", 0), .GetValue("ts", 0), nVals)
                            lReadTS.numValues = nVals
                        End If
                        If nVals > 0 Then
                            ReDim lV(nVals)
                            Dim lDsn As Integer = CInt(.GetValue("id", 1))
                            Dim lTimeStep As Integer = CInt(.GetValue("ts", 0))
                            Dim lTimeUnits As Integer = CInt(.GetValue("tu", 0))
                            Dim lTran As Integer = 0 'transformation = aver,same
                            Dim lQual As Integer = 31 'allowed quality code
                            'F90_WDTGET(lWdmHandle.Unit, lDsn, lTimeStep, lSdat(0), nVals, _
                            '           lTran, lQual, lTimeUnits, lV(1), lRetcod)
                            F90_WDTGET(lWdmHandle.Unit, lDsn, lTimeStep, lSdat, nVals, _
                                       lTran, lQual, lTimeUnits, lV, lRetcod)
                            If lRetcod <> 0 Then
                                Logger.Dbg("FailedToReadDataFor " & lDsn & " with code " & lRetcod)
                            End If

                            ReDim lJd(nVals)
                            lJd(0) = lSJDay
                            ReDim lVd(nVals)
                            lVd(0) = pNan

                            lTsFill = .GetValue("TSFill", -999)
                            If lTsFill >= 0 Then 'WDM convention - fill value, not undefined,
                                lTsFill = pNan
                            End If

                            Dim lInterval As Double = .GetValue("interval", 0)
                            Dim lConstInterval As Boolean = (Math.Abs(lInterval) > 0.00001)
                            For iVal As Integer = 1 To nVals
                                If Math.Abs((lV(iVal - 1) - lTsFill)) < 1.0E-20 Then 'pEpsilon Then
                                    lVd(iVal) = pNan
                                Else
                                    lVd(iVal) = lV(iVal - 1) 'TODO: test speed of this vs. using ReadDataset.Value(iVal) = v(iVal)
                                End If
                                If lConstInterval Then
                                    lJd(iVal) = lSJDay + iVal * lInterval
                                Else
                                    TIMADD(lSdatSeasonOffset, lTimeUnits, lTimeStep, iVal, lEdat)
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

    Private Sub RefreshDsn(ByVal aFileUnit As Integer, ByVal aDsn As Integer)
        Dim lDates As New atcTimeseries(Nothing)
        pDates.Add(lDates)

        Dim lData As New atcTimeseries(Me)
        lData.Dates = lDates
        lData.Attributes.SetValue("id", aDsn)
        lData.Attributes.AddHistory("Read from " & Specification)

        lData.ValuesNeedToBeRead = True
        DataSets.Add(aDsn, lData)
        If aFileUnit > 0 AndAlso lData IsNot Nothing Then
            Try
                DsnReadGeneral(aFileUnit, lData)
            Catch lEx As Exception
                Logger.Dbg("Problem")
            End Try
        Else
            Logger.Dbg("Problem")
        End If

        Dim lInit As Integer = 1
        Do
            Dim lSaind As Integer
            Dim lSaval(256) As Integer
            F90_GETATT(aFileUnit, aDsn, lInit, lSaind, lSaval)
            If lSaind > 0 Then 'process attribute
                If Not (AttrStored(lSaind)) Then
                    Try
                        Dim lAttributeDefinition As atcAttributeDefinition = pMsg.Attributes.ItemByIndex(lSaind)
                        Dim lName As String = lAttributeDefinition.Name
                        Dim lS As String = AttrVal2String(lSaind, lSaval)
                        Select Case UCase(lName)
                            Case "DATCRE", "DATMOD", "DATE CREATED", "DATE MODIFIED"
                                Dim lDate As Date
                                If IsNumeric(lS.Substring(0, 4)) Then
                                    Try 'Dates should be formatted YYYYMMDDhhmmss
                                        lDate = New Date(CInt(lS.Substring(0, 4)), _
                                                         CInt(lS.Substring(4, 2)), _
                                                         CInt(lS.Substring(6, 2)), _
                                                         CInt(lS.Substring(8, 2)), _
                                                         CInt(lS.Substring(10, 2)), _
                                                         CInt(lS.Substring(12, 2)))
                                    Catch ex As Exception
                                        GoTo ParseDate
                                    End Try
                                Else 'parse dates written as M/D/YYYY h:mm:ss (truncated to 14 characters)
ParseDate:                          Logger.Dbg(lName & " text date '" & lS & "' - unknown whether AM or PM")
                                    Dim lMonth As Integer = StrFirstInt(lS)
                                    lS = lS.Substring(1)
                                    Dim lDay As Integer = StrFirstInt(lS)
                                    lS = lS.Substring(1)
                                    Dim lYear As Integer = StrFirstInt(lS)
                                    lDate = New Date(lYear, lMonth, lDay, 12, 0, 0)
                                    Logger.Dbg(lName & "parsed as '" & lDate.ToString & "' rounded to noon")
                                End If
                                lData.Attributes.SetValue(pMsg.Attributes.Item(lSaind), lDate)
                                'Case "DCODE"
                                'lData.Attributes.SetValue(UnitsAttributeDefinition(True), GetUnitName(CInt(lS)))
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

        'If lData.Attributes.GetValue("locn") = "<unk>" And BasInfAvail Then 'no location, try to check BASINS inf file
        'Call HeaderFromBasinsInf(lData)
        'End If

    End Sub

    'Before calling, make sure id property of aDataset has been set to dsn -- aDataset.Attributes.SetValue("id", dsn)
    Private Sub DsnReadGeneral(ByVal aFileUnit As Integer, _
                               ByVal aDataset As atcTimeseries)
        Dim lSaInd, lSaLen, lRetcod As Integer
        Dim lTu As atcTimeUnit
        Dim lTs As Integer
        Dim lNvals As Integer
        Dim lStr As String = ""
        Dim lSdat(6) As Integer
        Dim lEdat(6) As Integer
        Dim lDsn As Integer = aDataset.Attributes.GetValue("id", 1)

        lSaLen = 1
        lSaInd = 33 'time step
        F90_WDBSGI(aFileUnit, lDsn, lSaInd, lSaLen, lTs, lRetcod)
        If (lRetcod <> 0) Then ' set time step to default of 1
            lTs = 1
        End If
        aDataset.Attributes.SetValue("ts", lTs)

        lSaInd = 17 'time unit
        Dim lTuInt As Integer
        F90_WDBSGI(aFileUnit, lDsn, lSaInd, lSaLen, lTuInt, lRetcod)
        If (lRetcod <> 0) Then 'set to default of daily time units
            lTu = atcTimeUnit.TUDay
        Else
            lTu = CType(lTuInt, atcTimeUnit)
        End If

        aDataset.Attributes.SetValue("tu", lTu)
        'TODO: set constant interval/interval length attribute(s) in aDataset.Dates
        'lDateSum.CIntvl = True
        Select Case lTu
            Case atcTimeUnit.TUDay
                aDataset.Attributes.SetValue("interval", lTs)
            Case atcTimeUnit.TUHour
                aDataset.Attributes.SetValue("interval", lTs / CDbl(24))
            Case atcTimeUnit.TUMinute
                aDataset.Attributes.SetValue("interval", lTs / CDbl(1440))
        End Select

        If Not pQuick Then 'get start and end dates for each data set
            Dim lGpFlg As Integer = 1
            Dim lDsfrc As Integer
            F90_WTFNDT(aFileUnit, lDsn, lGpFlg, lDsfrc, lSdat, lEdat, lRetcod)
            If lSdat(0) > 0 Then
                Dim lDates As atcTimeseries = aDataset.Dates
                Dim lSJDay As Double = Date2J(lSdat)
                Dim lEJDay As Double = Date2J(lEdat)
                aDataset.Attributes.SetValue("SJDay", lSJDay)
                aDataset.Attributes.SetValue("EJDay", lEJDay)
                lDates.Attributes.SetValue("SJDay", lSJDay)
                lDates.Attributes.SetValue("EJDay", lEJDay)
                TimDif(lSdat, lEdat, lTu, lTs, lNvals)
                aDataset.numValues = lNvals
                lDates.numValues = lNvals
            End If
            aDataset.Attributes.SetValue("HeaderComplete", True)
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
    End Sub

    Private Function AttrVal2String(ByVal aSaInd As Integer, ByVal aSaVal() As Integer) As String
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

    Public Overrides Sub View()
        Logger.Msg("No Viewer available for WDM files", "View Problem")
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Private Function MemUsage() As String
        System.GC.WaitForPendingFinalizers()
        Return "MemoryUsage(MB):" & Process.GetCurrentProcess.PrivateMemorySize64 / (2 ^ 20) & _
                    " Local(MB):" & System.GC.GetTotalMemory(True) / (2 ^ 20)
    End Function

End Class
