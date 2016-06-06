'Copyright 2001-7 AQUA TERRA Consultants - Royalty-free use permitted under open source license
Option Strict Off
Option Explicit On

Imports atcData
Imports atcData.atcDataManager
Imports atcData.atcDataSource.EnumExistAction
Imports atcUtility
Imports MapWinUtility
Imports System.Collections.Specialized

Public Class atcDataSourceWDM
    Inherits atcData.atcTimeseriesSource

    'Set at beginning of execution if a particular message file is desired
    Public Shared HSPFMsgFilename As String = Nothing

    Private Shared pFilter As String = "WDM Files (*.wdm)|*.wdm"
    Private Shared pShowViewMessage As Boolean = True
    Private Shared pAskAboutMissingTuTs As Boolean = True
    Private pDates As New Generic.List(Of atcTimeseries)
    Private pQuick As Boolean = False
    Private Shared pNan As Double = GetNaN()

    Private Const pEpsilon As Double = 1.0E-20 ' tolerance for detecting missing value, System.Double.Epsilon is too small
    Private Shared pMsg As atcMsgWDM
    Private pTu As atcTimeUnit = 0 'default time units, default 2-minutes
    Private pTs As Integer = 0 'default timestep, default 1 

#If GISProvider = "DotSpatial" Then
#Else
    Public Const ImportMenuName As String = "BasinsImportToWDM"
    Public Const ImportMenuString As String = "Import to WDM"

    <CLSCompliant(False)> _
    Public Overrides Sub Initialize(ByVal aMapWin As MapWindow.Interfaces.IMapWin, ByVal aParentHandle As Integer)
        g_MapWin = aMapWin
        MyBase.Initialize(aMapWin, aParentHandle)
        AddMenuIfMissing(ImportMenuName, FileMenuName, ImportMenuString, ManageDataMenuName)
    End Sub

    Public Overrides Sub ItemClicked(ByVal aItemName As String, ByRef aHandled As Boolean)
        If aItemName = ImportMenuName Then
            Dim lFormImport As New frmImport
            lFormImport.Show()
            aHandled = True
        End If
    End Sub
#End If

    'Can be set by user to avoid asking user each time
    Private pExistAskUserAction As EnumExistAction = ExistAskUser

    Public Overrides Sub Clear()
        MyBase.Clear()
        pQuick = False
        pDates.Clear()
        SaveSetting("WDM", "AddDataset", "ExistAskUserAction", "")
    End Sub

    Public Sub New()
        MyBase.New()
        Filter = pFilter
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
        pDates.Clear()

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
            Case 17 : Return True 'tcode
                'Case 27: Return True 'tsbyr  'jlk commmented to fix winhspf problem
            Case 33 : Return True 'tsstep
                'Case 45 : Return True 'staname  'prh - don't force STANAM to be read
            Case 288 : Return True 'idscen
            Case 289 : Return True 'idlocn
            Case 290 : Return True 'idcons
                'Case 10 : Return True 'description
            Case Else : Return False
        End Select
    End Function

    Public Overrides Function AddDatasets(ByVal aDataGroup As atcDataGroup) As Boolean
        Dim lNumSaved As Integer = 0
        Dim lDSN As Integer
        Dim lLowestDSN As Integer = Integer.MaxValue
        Dim lHighestDSN As Integer = 0
        Dim lLabel As String = FilenameNoPath(Specification) & " contains " & DataSets.Count & " datasets"
        Dim lHaveDSNconflict As Boolean = False

        If DataSets.Count > 0 Then
            For Each lDataSet As atcDataSet In DataSets
                lDSN = lDataSet.Attributes.GetValue("dsn", 0)
                If lDSN > 0 Then
                    If lDSN > lHighestDSN Then lHighestDSN = lDSN
                    If lDSN < lLowestDSN Then lLowestDSN = lDSN
                End If
            Next
            lLabel &= " numbered " & lLowestDSN & " to " & lHighestDSN

            For Each lDataSet As atcTimeseries In aDataGroup
                lDSN = lDataSet.Attributes.GetValue("dsn", 0)
                If DataSets.Keys.Contains(lDSN) Then
                    lHaveDSNconflict = True
                End If
            Next
        End If

        Dim lHighestNewDSN As Integer = 0
        If lHaveDSNconflict Then lHighestNewDSN = lHighestDSN

        Dim lCloneTsGroup As New atcTimeseriesGroup
        For Each lDataSet As atcTimeseries In aDataGroup
            lDSN = lDataSet.Attributes.GetValue("dsn", 1)
            If lDSN > lHighestNewDSN AndAlso lDSN < 9999 Then
                lHighestNewDSN = lDSN
            Else
                lHighestNewDSN += 1
            End If

            Dim lClone As atcTimeseries = lDataSet.Clone(Me)
            lClone.Attributes.SetValue("New DSN", lHighestNewDSN)
            lCloneTsGroup.Add(lClone)
        Next
        Dim lOverwriteSelected As Boolean = False
#If BatchMode Then
#Else
        Dim lfrm As New frmSave
        lCloneTsGroup = lfrm.AskUser(lCloneTsGroup, lLabel, lHighestDSN)
        lOverwriteSelected = lfrm.OverwriteSelected
#End If
        If lCloneTsGroup IsNot Nothing AndAlso lCloneTsGroup.Count > 0 Then
            Dim lExistAction As atcDataSource.EnumExistAction = ExistAskUser
            If lOverwriteSelected Then lExistAction = ExistReplace
            For Each lDataset As atcTimeseries In lCloneTsGroup
                Dim lNewDSN As Integer = lDataset.Attributes.GetValue("New DSN", 0)
                If lNewDSN > 0 Then
                    lDataset.Attributes.SetValue("DSN", lNewDSN)
                    lDataset.Attributes.RemoveByKey("New DSN")
                End If
                If AddDataset(lDataset, lExistAction) Then lNumSaved += 1
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
                                Optional ByVal aExistAction As atcData.atcDataSource.EnumExistAction _
                                                             = atcData.atcDataSource.EnumExistAction.ExistReplace) _
                                         As Boolean
        'Logger.Dbg("atcDataSourceWdm:AddDataset:EntryWithAction:" & aExistAction & ":" & MemUsage())
        Using lWdmHandle As New atcWdmHandle(0, Specification)
            Try
                Dim lTimser As atcTimeseries = aDataSet
                Dim lTs As Integer = lTimser.Attributes.GetValue("ts", 0)
                Dim lTu As atcTimeUnit = lTimser.Attributes.GetValue("tu", atcTimeUnit.TUUnknown)
                Dim lDsn As Integer = lTimser.Attributes.GetValue("id", 1)
                Dim lTGroup As Integer = lTimser.Attributes.GetValue("tgroup", 6)
                'Dim lTSBYr As Integer = lTimser.Attributes.GetValue("tsbyr", 1900)
                Dim lTimserConst As atcTimeseries = Nothing

                If lTs = 0 OrElse lTu = atcTimeUnit.TUUnknown Then ' sparse dataset - fill in dummy values for write
                    lTimserConst = Me.FillSparseTS(lTimser, lTs, lTu, lDsn)
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

                'Logger.Dbg("atcDataSourceWdm:AddDataset:WdmUnit:Dsn:" & lWdmHandle.Unit & ":" & lDsn)

                Dim lDsnExists As Boolean = DataSets.Keys.Contains(lDsn) ' Integer = F90_WDCKDT(lWdmHandle.Unit, lDsn)
                If lDsnExists Then ' lDsnExists > 0 Then 'dataset exists, what do we do?
                    'Logger.Dbg("atcDataSourceWdm:AddDataset:DatasetAlreadyExists")
                    'Change asking user into what the user already chose for all
                    If aExistAction = ExistAskUser Then aExistAction = pExistAskUserAction
                    Dim lExistTimser As atcTimeseries = DataSets.ItemByKey(lDsn)
                    If lExistTimser.Serial = lTimser.Serial Then
                        Logger.Dbg("Adding a dataset that already exists in WDM: replacing old version in WDM, DSN=" & lDsn)
                        aExistAction = ExistReplace
                    End If
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
                            lTimser.Attributes.SetValue("Id", lDsn)
                    End Select
                Else
                    'Logger.Dbg("atcDataSourceWdm:AddDataset:NewDataSet:WdmUnit:Dsn:" & lWdmHandle.Unit & ":" & lDsn)
                End If

                Dim lWriteIt As Boolean = False
                If lDsnExists AndAlso aExistAction = ExistAppend Then 'just write appended data
                    lWriteIt = True
                ElseIf DsnBld(lWdmHandle.Unit, lTimser) Then
                    DataSets.Add(lDsn, lTimser)
                    lWriteIt = True
                End If

                If lWriteIt AndAlso lNvals > 0 Then
                    Dim lTSFillDbl As Double = lTimser.Attributes.GetValue("tsfill", -999)
                    Dim lTSFillSng As Single
                    If Double.IsNaN(lTSFillDbl) Then
                        lTSFillSng = -999
                    Else
                        lTSFillSng = CSng(lTSFillDbl)
                    End If
                    Dim lValue As Double
                    Dim lV(lNvals) As Single
                    Dim lRet As Integer
                    For i As Integer = 1 To lNvals
                        lValue = lTimser.Value(i)
                        If Double.IsNaN(lValue) Then
                            lV(i - 1) = lTSFillSng
                        Else
                            lV(i - 1) = CSng(lValue)
                        End If
                    Next
                    If Double.IsNaN(lTimser.Dates.Value(0)) Then
                        If lTimser.numValues > 1 Then
                            'TODO: should use timadd?
                            lTimser.Dates.Value(0) = lTimser.Dates.Value(1) - (lTimser.Dates.Value(2) - lTimser.Dates.Value(1))
                        End If
                    End If
                    J2DateRounddown(lTimser.Dates.Value(0), lTu, lSDat)

                    'Logger.Dbg("atcDataSourceWdm:AddDataset:WDTPUT:call:" & _
                    '            lWdmHandle.Unit & ":" & lDsn & ":" & lTs & ":" & lNvals & ":" & _
                    '            lSDat(0) & ":" & lSDat(1) & ":" & lSDat(2) & ":" & lRet)
                    If lNvals > 0 Then
                        F90_WDTPUT(lWdmHandle.Unit, lDsn, lTs, lSDat, lNvals, CInt(1), CInt(0), lTu, lV, lRet)
                    Else
                        Logger.Dbg("Expected number of values greater than zero, but was = " & lNvals)
                    End If
                    'Clearing means we can't use it for graphing, listing, etc. after
                    'If lTimserConst IsNot Nothing Then
                    '    lTimserConst.Clear() 'TODO: maybe just part?
                    'End If
                    If lRet <> 0 Then
                        Throw New ApplicationException("WDTPUT:call:" & _
                                    lWdmHandle.Unit & ":" & lDsn & ":" & lTs & ":" & lNvals & ":" & _
                                    lSDat(0) & ":" & lSDat(1) & ":" & lSDat(2) & ":" & lRet)
                        'Logger.Dbg("atcDataSourceWdm:AddDataset:WDTPUT:back:" & _
                        '            lWdmHandle.Unit & ":" & lDsn & ":" & lRet)
                    End If
                    lTimser.Attributes.SetValue("Data Source", Specification)
                    lTimser.Attributes.AddHistory("Added to " & Specification)
                    Return True
                Else
                    Return False
                End If

            Catch ex As Exception
                Logger.Dbg("atcDataSourceWdm:AddDataSet:" & ex.ToString)
                Return False
            End Try
        End Using
    End Function

    Private Function FillSparseTS(ByRef lTimser As atcTimeseries, ByRef lTs As Integer, ByRef lTu As atcTimeUnit, ByRef lDSN As Integer) As atcTimeseries
        Dim lTimserConst As atcTimeseries
        Dim lAggr As Integer = lTimser.Attributes.GetValue("aggregation", 0)
#If BatchMode Then
#Else
        If pAskAboutMissingTuTs Then
            Dim lFrmDefaultTimeInterval As New frmDefaultTimeInterval
            pAskAboutMissingTuTs = lFrmDefaultTimeInterval.AskUser(lTimser.ToString & " #" & lDSN, pTu, pTs, lAggr)
        End If
#End If
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
                Dim lZeroDate As Double
                Dim lNumValues As Integer
                If lJulianInterval > 0 Then
                    lZeroDate = CInt(lTimser.Dates.Value(1)) - 1 'whole day portion
                    lNumValues = (lTimser.Dates.Value(lTimser.numValues) - lZeroDate) / lJulianInterval
                Else
                    lZeroDate = TimAddJ(lTimser.Dates.Value(1), lTu, lTs, -1) 'for monthly data
                    lNumValues = lTimser.numValues
                End If
                .numValues = lNumValues
                .Dates.Value(0) = lZeroDate
                .SetInterval(lTu, lTs)
                Dim lIndex As Integer = 1
                For lIndexConst As Integer = 1 To lNumValues
                    Dim lDate As Double
                    If lJulianInterval > 0 Then
                        lDate = .Dates.Value(0) + (lJulianInterval * lIndexConst)
                    Else
                        lDate = TimAddJ(.Dates.Value(0), lTu, lTs, lIndexConst)
                    End If
                    Dim lAggregationCount As Integer = .ValueAttributes(lIndexConst).GetValue("AggregationCount", 0)
                    .Dates.Value(lIndexConst) = lDate
                    If lIndex <= lTimser.numValues AndAlso lDate >= (lTimser.Dates.Value(lIndex) - JulianSecond) Then
                        'todo: this uses only the last value, have a transformation 
                        lAggregationCount += 1
                        .ValueAttributes(lIndexConst).Add("AggregationCount", lAggregationCount)
                        Dim lValue As Double = lTimser.Value(lIndex)
                        If lAggregationCount = 1 Then 'save this first value
                            .Value(lIndexConst) = lValue
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
                        .Value(lIndexConst) = pNan
                    End If
                Next
            End With
            'Logger.Dbg(MemUsage)
            lTimser = lTimserConst
            'Logger.Dbg(MemUsage)
            Return lTimserConst
        Else
            Throw New ApplicationException("No Time Units or Time Step on source dataset")
        End If
    End Function

    Public Function WriteAttributes(ByVal aDataSet As atcData.atcDataSet) As Boolean
        'Logger.Dbg("atcDataSourceWdm:WriteAttributes:entry:" & aDataSet.ToString)
        Using lWdmHandle As New atcWdmHandle(0, Specification)
            WriteAttributes = DsnWriteAttributes(lWdmHandle.Unit, aDataSet)
        End Using
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
        'Logger.Dbg("atcDataSourceWdm:WriteAttributes:entry:" & aDataSet.ToString & ":" & aAttribute.Definition.Name & "=" & aAttribute.Value)
        Using lWdmHandle As New atcWdmHandle(0, Specification), lMsg As atcWdmHandle = pMsg.MsgHandle
            Dim lDsn As Integer = aDataSet.Attributes.GetValue("id", 1)

            If Not aNewValue Is Nothing Then
                aAttribute.Value = aNewValue
            End If

            WriteAttribute = DsnWriteAttribute(lWdmHandle.Unit, lMsg.Unit, lDsn, aAttribute)

        End Using
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
        Using lWdmHandle As New atcWdmHandle(0, Specification)
            Call F90_WDDSDL(lWdmHandle.Unit, (aDataSet.Attributes.GetValue("id", 1)), lRetcod)
        End Using

        If lRetcod = 0 Then
            lRemoveDataset = True
            DataSets.Remove(aDataSet)
            Dim lTimser As atcTimeseries = aDataSet
            If Not lTimser.ValuesNeedToBeRead Then
                If pDates.Contains(lTimser.Dates) AndAlso Not lTimser.Dates.Attributes.ContainsAttribute("Shared") Then
                    pDates.Remove(lTimser.Dates)
                End If
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
        With aTs.Attributes
            Dim lDsn As Integer = .GetValue("id", 1)
            'add needed attributes
            Dim lStr As String = .GetValue("cons", "")
            If lStr.Length > 4 Then
                lStr = lStr.Substring(0, 4)
            End If
            .SetValueIfMissing("TSTYPE", lStr)
            .SetValueIfMissing("TGROUP", 6)
            .SetValueIfMissing("COMPFG", 1)
            If .GetValue("Point", False) Then
                .SetValueIfMissing("TSFORM", 3) 'TSFORM=3 instantaneous @ time (end of timestep)
            Else
                .SetValueIfMissing("TSFORM", 1) 'TSFORM=1 mean over the timestep (default)
            End If
            .SetValueIfMissing("TSFILL", -999)
            Dim lTs As Integer = 1
            Dim lTu As atcTimeUnit = atcTimeUnit.TUUnknown
            If .ContainsAttribute("tu") Then
                lTu = .GetValue("tu")
                lTs = .GetValue("ts")
            ElseIf aTs.numValues > 0 Then
                CalcTimeUnitStep(aTs.Dates.Value(0), aTs.Dates.Value(1), lTu, lTs)
                aTs.SetInterval(lTu, lTs)
            End If
            Dim lIVal As Integer = .GetValue("VBTIME", 1)
            If lTs > 1 Then
                lIVal = 2 'timestep > 1 vbtime must vary
            End If
            .SetValueIfMissing("VBTIME", lIVal)

            If Not .ContainsAttribute("TSBYR") Then
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
                    .SetValue("TSBYR", lIVal)
                Else
                    .SetValueIfMissing("TSBYR", 1700)
                End If
            End If

            .SetValueIfMissing("scen", "")
            .SetValueIfMissing("cons", "")
            .SetValueIfMissing("locn", "")
            .SetValueIfMissing("desc", "")

            DsnWriteAttributes = True
            Using lMsg As atcWdmHandle = pMsg.MsgHandle
                Dim lMsgUnit As Integer = lMsg.Unit
                For Each lAttribute As atcDefinedValue In aTs.Attributes
                    'Debug.WriteLine(aTs.Attributes.ItemByIndex(lAttributeIndex).ToString)
                    If Not DsnWriteAttribute(aFileUnit, lMsgUnit, lDsn, lAttribute) Then
                        DsnWriteAttributes = False
                        Logger.Dbg("Failed to write Attribute " & lAttribute.ToString)
                        Exit For
                    End If
                Next
            End Using
        End With
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
            Case "elevation" : lName = "elev"
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
                    'Logger.Dbg("Skip:" & lName & ", data present")
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
                          Optional ByVal aAttributes As atcDataAttributes = Nothing) As Boolean

        If MyBase.Open(aFileName, aAttributes) Then
            Dim lRWCFlg As Integer = 0
            If Not FileExists(Specification) Then
                If IO.Path.GetFileName(Specification).Length > 0 Then
                    Logger.Dbg("atcDataSourceWDM:Open:WDM file " & Specification & " does not exist - it will be created")
                    MkDirPath(PathNameOnly(Specification))
                    lRWCFlg = 2
                Else
                    Logger.Dbg("atcDataSourceWDM:Open:Problem opening WDM file '" & Specification & "'")
                    Return False
                End If
            End If

            Using lWdmHandle As New atcWdmHandle(lRWCFlg, Specification)
                If lWdmHandle.Unit > 0 Then
                    pQuick = True
                    Refresh(lWdmHandle.Unit)
                    pQuick = False
                    Return True 'Successfully opened
                Else
                    Logger.Dbg("atcDataSourceWDM:Open:Problem opening WDM file '" & Specification & "'")
                End If
            End Using
        End If
        Return False
    End Function

    Public Overrides Sub ReadData(ByVal aReadMe As atcDataSet)
        If Not DataSets.Contains(aReadMe) Then
            Logger.Dbg("WDM cannot read dataset with details:" & aReadMe.ToString & vbCrLf & _
                       "Specification:'" & Specification & "'")
        Else
            Dim lVd(0) As Double 'array of double data values
            Dim lRetcod As Integer
            Dim lSdat(6) As Integer 'starting date
            Dim lSdatSeasonOffset(6) As Integer 'starting date with seasonal offset
            Dim lReadTS As atcTimeseries = aReadMe

            lReadTS.ValuesNeedToBeRead = False
            'Logger.dbg("WDM read data " & aReadMe.Attributes.GetValue("Location"))
            Using lWdmHandle As New atcWdmHandle(0, Specification)
                If lWdmHandle.Unit > 0 Then
                    With aReadMe.Attributes
                        If Not CBool(.GetValue("HeaderComplete", False)) Then
                            DsnReadGeneral(lWdmHandle.Unit, lReadTS)
                        End If

                        If Not CBool(.GetValue("HeaderOnly", False)) Then
                            Dim lTu As atcTimeUnit = .GetValue("Time Unit", atcTimeUnit.TUUnknown)
                            Dim lTs As Integer = .GetValue("Time Step", 1)
                            Dim lSJDay As Double = .GetValue("SJDay", 0)
                            J2Date(lSJDay, lSdat) 'saved starting date as array for wdtget
                            If lTu = atcUtility.atcTimeUnit.TUYear Then
                                Dim lStartMonth As Integer = .GetValue("SEASBG", 1)
                                Dim lStartDay As Integer = .GetValue("SEADBG", 1)
                                If lStartMonth > 1 OrElse lStartDay > 1 Then
                                    lSJDay = TimAddJ(lSJDay, atcTimeUnit.TUMonth, 1, lStartMonth - 1) + lStartDay - 1
                                End If
                            End If
                            J2Date(lSJDay, lSdatSeasonOffset)

                            Dim nVals As Integer = lReadTS.numValues
                            If nVals = 0 Then
                                Dim lEndDat(6) As Integer
                                J2Date(.GetValue("EJDay", 0), lEndDat)
                                TimDif(lSdat, lEndDat, .GetValue("tu", 0), lTs, nVals)
                                'lReadTS.numValues = nVals
                            End If
                            If nVals > 0 Then
                                Dim lDsn As Integer = CInt(.GetValue("id", 1))
                                Dim lTimeStep As Integer = CInt(.GetValue("ts", 0))
                                Dim lTimeUnits As Integer = CInt(.GetValue("tu", 0))
                                Dim lTran As Integer = 0 'transformation = aver,same
                                Dim lQual As Integer = 31 'allowed quality code
                                Dim lV(nVals) As Single 'data values come from WDM as Single
                                F90_WDTGET(lWdmHandle.Unit, lDsn, lTimeStep, lSdat, nVals, _
                                           lTran, lQual, lTimeUnits, lV, lRetcod)
                                If lRetcod <> 0 Then
                                    Throw New ApplicationException("ReadData Failed For " & lDsn & " with code " & lRetcod)
                                End If
                                Dim lDates As atcTimeseries = lReadTS.Dates
                                Dim lNeedDates As Boolean = lDates.ValuesNeedToBeRead OrElse (lDates.numValues <> nVals) OrElse lDates.Value(1) < 1
                                Dim lJd() As Double 'array of julian dates
                                If lNeedDates Then
                                    ReDim lJd(nVals)
                                    lJd(0) = lSJDay
                                Else
                                    lJd = Nothing
                                End If

                                ReDim lVd(nVals)
                                lVd(0) = pNan

                                Dim lTsFill As Double = .GetValue("TSFill", -999)
                                If lTsFill >= 0 Then 'WDM convention - fill value, not undefined,
                                    lTsFill = pNan
                                End If

                                Dim lInterval As Double = .GetValue("interval", 0)
                                Dim lConstInterval As Boolean = (Math.Abs(lInterval) > 0.00001)
                                Dim lCurDat(6) As Integer 'current date
                                Dim lDataCurrent As Double

                                For iVal As Integer = 1 To nVals
                                    lDataCurrent = CDbl(lV(iVal - 1))
                                    'If lDataCurrent < -90 Then Stop
                                    'If value is a very large negative number (from HSPF unable to compute) or is the fill value, treat it as missing
                                    If lDataCurrent < -1.0E+20 OrElse Math.Abs((lDataCurrent - lTsFill)) < pEpsilon Then
                                        lVd(iVal) = pNan
                                    Else
                                        lVd(iVal) = lDataCurrent 'TODO: test speed of this vs. using ReadDataset.Value(iVal) = v(iVal)
                                    End If
                                    If lNeedDates Then
                                        If lConstInterval Then
                                            lJd(iVal) = lSJDay + iVal * lInterval
                                        Else
                                            TIMADD(lSdatSeasonOffset, lTimeUnits, lTimeStep, iVal, lCurDat)
                                            lJd(iVal) = Date2J(lCurDat)
                                        End If
                                    End If
                                Next

                                If lNeedDates Then
                                    Dim lMatchingDates As atcTimeseries = Nothing
                                    'Search for existing Dates that exactly match these so we can share that one and save memory
                                    For Each lOtherDates As atcTimeseries In pDates
                                        If lOtherDates.Serial <> lReadTS.Dates.Serial AndAlso _
                                           lOtherDates.numValues = nVals AndAlso _
                                           lOtherDates.Value(1) = lJd(1) AndAlso _
                                           lOtherDates.Value(nVals) = lJd(nVals) Then
                                            If lConstInterval Then
                                                If Math.Abs(lInterval - lOtherDates.Attributes.GetValue("Interval", 0)) < pEpsilon Then
                                                    lMatchingDates = lOtherDates
                                                End If
                                            Else
                                                lMatchingDates = lOtherDates
                                                For lDateIndex As Integer = nVals To 0 Step -1
                                                    If lOtherDates.Value(lDateIndex) <> lJd(lDateIndex) Then
                                                        lMatchingDates = Nothing
                                                        Exit For
                                                    End If
                                                Next
                                            End If
                                            If lMatchingDates IsNot Nothing Then Exit For
                                        End If
                                    Next
                                    If lMatchingDates Is Nothing Then
                                        lReadTS.Dates.Values = lJd
                                        lReadTS.Dates.ValuesNeedToBeRead = False
                                        pDates.Add(lReadTS.Dates)
                                    Else
                                        lReadTS.Dates.Clear()
                                        lReadTS.Dates = lMatchingDates
                                        lMatchingDates.Attributes.SetValue("Shared", True)
                                    End If
                                End If
                            End If

                            lReadTS.Values = lVd
                        End If
                    End With
                    'atcDataManager.AddDiscardableTimeseries(lReadTS)
                End If
            End Using
        End If
    End Sub

    Private Sub RefreshDsn(ByVal aFileUnit As Integer, ByVal aDsn As Integer)
        Dim lData As New atcTimeseries(Me)
        Dim lDates As New atcTimeseries(Nothing) 'Using Nothing instead of Me here because we only read dates while doing ReadData for values
        lDates.ValuesNeedToBeRead = True
        lData.Dates = lDates
        lData.Attributes.SetValue("id", aDsn)
        lData.Attributes.AddHistory("Read from " & Specification)

        lData.ValuesNeedToBeRead = True
        DataSets.Add(aDsn, lData)
        If aFileUnit > 0 AndAlso lData IsNot Nothing Then
            Try
                DsnReadGeneral(aFileUnit, lData)
            Catch lEx As Exception
                Logger.Dbg("RefreshDsn: " & lEx.ToString)
            End Try
        Else
            Throw New ApplicationException("RefreshDsn: aFileUnit=" & aFileUnit & ", lData=" & (lData IsNot Nothing))
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
                                lData.Attributes.SetValue(lAttributeDefinition, lDate)
                                'Case "DCODE"
                                'lData.Attributes.SetValue(UnitsAttributeDefinition(True), GetUnitName(CInt(lS)))
                            Case "TSFORM"
                                lData.Attributes.SetValue("Point", (lS = "3"))
                                lData.Attributes.SetValue(lAttributeDefinition, lS)
                            Case Else
                                lData.Attributes.SetValue(lAttributeDefinition, lS)
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
        Dim lStr As String = ""
        Dim lSdat(6) As Integer
        Dim lEdat(6) As Integer
        Dim lDsn As Integer = aDataset.Attributes.GetValue("id", 1)

        lSaLen = 1
        lSaInd = 33 'time step (TSSTEP)
        F90_WDBSGI(aFileUnit, lDsn, lSaInd, lSaLen, lTs, lRetcod)
        If (lRetcod <> 0) Then ' set time step to default of 1
            lTs = 1
        End If
 
        lSaInd = 17 'time unit (TCODE)
        Dim lTuInt As Integer
        F90_WDBSGI(aFileUnit, lDsn, lSaInd, lSaLen, lTuInt, lRetcod)
        If (lRetcod <> 0) Then 'set to default of daily time units
            lTu = atcTimeUnit.TUDay
        Else
            lTu = CType(lTuInt, atcTimeUnit)
        End If

        aDataset.SetInterval(lTu, lTs)

        If Not pQuick Then 'get start and end dates for each data set
            Dim lGpFlg As Integer = 1
            Dim lDsfrc As Integer
            F90_WTFNDT(aFileUnit, lDsn, lGpFlg, lDsfrc, lSdat, lEdat, lRetcod)
            If lSdat(0) > 0 Then
                aDataset.Attributes.SetValue("SJDay", Date2J(lSdat))
                aDataset.Attributes.SetValue("EJDay", Date2J(lEdat))
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
        If pShowViewMessage Then
            Select Case Logger.MsgCustom(Specification & vbCrLf & "No text viewer available for WDM files", "View", _
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

    'Private Function MemUsage() As String
    '    System.GC.WaitForPendingFinalizers()
    '    Return "MemoryUsage(MB):" & Process.GetCurrentProcess.PrivateMemorySize64 / (2 ^ 20) & _
    '                " Local(MB):" & System.GC.GetTotalMemory(True) / (2 ^ 20)
    'End Function

End Class
