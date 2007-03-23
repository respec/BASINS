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
    Private pQuick As Boolean = False
    Private pNan As Double
    Private pEpsilon As Double
    Private Shared pMsg As atcMsgWDM

    Private Sub Clear()
        pErrorDescription = ""
        Specification = "<unknown>"
        pQuick = False

        pDates = Nothing
        pDates = New ArrayList
    End Sub

    Public Sub New()
        MyBase.New()

        'kludge to force loading of system.double, removal may cause program to exit without message!
        pNan = System.Double.NaN
        pEpsilon = System.Double.Epsilon

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
            Else
                'Logger.Dbg("Refresh " & lDsn)
            End If
            If lDsn = -1 Then
                'Logger.Status("WDM Refresh Complete")
                'Logger.Progress(100, 0)
                'Logger.Progress("WDM Refresh Complete", 100, 0)
            Else 'try the next dsn
                lDsn += 1
                lProgPrev = lProg
                lProg = (100 * lDsn) / 32000
                'Logger.Progress(lProg, lProgPrev)
                'Logger.Progress("WDM Refresh", lProg, lProgPrev)
            End If
        End While
    End Sub

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

    Public Overrides Function AddDataset(ByVal aDataSet As atcData.atcDataSet, _
                                Optional ByVal aExistAction As atcData.atcDataSource.EnumExistAction = atcData.atcDataSource.EnumExistAction.ExistReplace) _
                                         As Boolean
        'Logger.Dbg("atcDataSourceWdm:AddDataset:entry:" & aExistAction)
        Dim lWdmHandle As New atcWdmHandle(0, Specification)
        Try
            Dim lTimser As atcTimeseries = aDataSet
            Dim lTs As Integer = lTimser.Attributes.GetValue("ts")
            Dim lTu As Integer = lTimser.Attributes.GetValue("tu")
            Dim lNvals As Integer = lTimser.numValues
            Dim lSJDay As Double = lTimser.Dates.Value(0)
            Dim lSDat(5) As Integer
            J2Date(lSJDay, lSDat)
            Dim lEJDay As Double = lTimser.Dates.Value(lTimser.numValues)
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

            'Logger.Dbg("atcDataSourceWdm:AddDataset:WdmUnit:Dsn:" & lWdmHandle.Unit & ":" & lDsn)

            If F90_WDCKDT(lWdmHandle.Unit, lDsn) > 0 Then 'dataset exists, what do we do?
                'Logger.Dbg("atcDataSourceWdm:AddDataset:DatasetAlreadyExists")
                If aExistAction = ExistReplace Then
                    'Logger.Dbg("atcDataSourceWdm:AddDataset:ExistReplace:")
                    Dim lExistTimser As atcTimeseries = DataSets.ItemByKey(lDsn)
                    Dim lRet As Integer
                    F90_WDDSDL(lWdmHandle.Unit, lDsn, lRet)
                    'Logger.Dbg("atcDataSourceWdm:AddDataset:RemovedOld:" & lWdmHandle.Unit & ":" & lDsn & ":" & lRet)
                    If Not lExistTimser Is Nothing Then
                        DataSets.Remove(lExistTimser)
                    End If
                ElseIf aExistAction = ExistAppend Then 'find dataset and try to append to it
                    'Logger.Dbg("atcDataSourceWdm:AddDataset:ExistAppend:" & lWdmHandle.Unit & ":" & lDsn)
                    Dim lExistTimser As atcTimeseries = DataSets.ItemByKey(lDsn)
                    If lTimser.numValues > 0 AndAlso _
                       lExistTimser.numValues > 0 AndAlso _
                       lTimser.Dates.Value(1) <= lExistTimser.Dates.Value(lExistTimser.numValues) Then
                        Throw New Exception("atcDataSourceWDM:AddDataset: Unable to append new TSer " & _
                                            lTimser.ToString & " to existing TSer " & _
                                            lExistTimser.ToString & vbCrLf & _
                                            "New TSer start date (" & lTimser.Dates.Value(1) & _
                                            ") preceeds exising TSer end date (" & _
                                            lExistTimser.Dates.Value(lExistTimser.numValues) & ")")
                    End If
                Else 'use next available number
                    lDsn = findNextDsn(lDsn)
                    'Logger.Dbg("atcDataSourceWdm:AddNew:" & lWdmHandle.Unit & ":" & lDsn)
                    aDataSet.Attributes.SetValue("Id", lDsn)
                End If
            Else
                'Logger.Dbg("atcDataSourceWdm:AddDataset:NewDataSet:WdmUnit:Dsn:" & lWdmHandle.Unit & ":" & lDsn)
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
                'Logger.Dbg("atcDataSourceWdm:AddDataset:WDTPUT:call:" & _
                '            lWdmHandle.Unit & ":" & lDsn & ":" & lTs & ":" & lNvals & ":" & _
                '            lSDat(0) & ":" & lSDat(1) & ":" & lSDat(2) & ":" & lRet)
                If lNvals > 0 Then
                    F90_WDTPUT(lWdmHandle.Unit, lDsn, lTs, lSDat(0), lNvals, CInt(1), CInt(0), lTu, lV(1), lRet)
                End If
                If lRet <> 0 Then
                    Logger.Dbg("atcDataSourceWdm:AddDataset:WDTPUT:call:" & _
                                lWdmHandle.Unit & ":" & lDsn & ":" & lTs & ":" & lNvals & ":" & _
                                lSDat(0) & ":" & lSDat(1) & ":" & lSDat(2) & ":" & lRet)
                    'Logger.Dbg("atcDataSourceWdm:AddDataset:WDTPUT:back:" & _
                    '            lWdmHandle.Unit & ":" & lDsn & ":" & lRet)
                End If
            End If

            lWdmHandle.Dispose()
            Return True
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
        Dim lDsn As Integer = aDataSet.Attributes.GetValue("id", 0)

        If Not aNewValue Is Nothing Then aAttribute.Value = aNewValue

        WriteAttribute = DsnWriteAttribute(lWdmHandle.Unit, lMsg.Unit, lDsn, aAttribute)

        lMsg.Dispose()
        lWdmHandle.Dispose()
        'Logger.Dbg("atcDataSourceWdm:WriteAttributes:end")
    End Function

    Private Function findNextDsn(ByVal aDsn As Integer) As Integer
        Dim lDatasets As atcDataGroup = DataSets
        While lDatasets.Keys.Contains(aDsn)
            aDsn += 1
        End While
        Return aDsn
    End Function

    Public Function RemoveDataset(ByVal aDataSet As atcData.atcDataSet) As Boolean
        Dim lTimser As atcTimeseries = aDataSet
        Dim lWdmHandle As New atcWdmHandle(0, Specification)
        Dim lRetcod As Integer

        Call F90_WDDSDL(lWdmHandle.Unit, (aDataSet.Attributes.GetValue("id")), lRetcod)
        If lRetcod = 0 Then
            RemoveDataset = True
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
            RemoveDataset = False
            pErrorDescription = "WDM:RemoveDataset:DSN:" & aDataSet.Attributes.GetValue("id") & ":Retcod:" & lRetcod
            Logger.Dbg(pErrorDescription)
        End If
        lWdmHandle.Dispose()
    End Function

    Public Overrides Function Save(ByVal SaveFileName As String, _
                          Optional ByVal ExistAction As EnumExistAction = EnumExistAction.ExistReplace) As Boolean
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

    ''' <summary>
    ''' Put a new dataset in the WDM file from aTs
    ''' </summary>
    ''' <param name="aFileUnit">WDM file handle</param>
    ''' <param name="aTs">data set to add to WDM</param>
    ''' <returns></returns>
    Private Function DsnBld(ByVal aFileUnit As Integer, _
                            ByVal aTs As atcTimeseries) As Boolean
        Dim lDsn, lNSasp, lNUp, lNDn, lNSa, lNDp, lPsa As Integer

        lDsn = aTs.Attributes.GetValue("id", 0)
        lNDn = aTs.Attributes.GetValue("NDN", 10)
        lNUp = aTs.Attributes.GetValue("NUP", 10)
        lNSa = aTs.Attributes.GetValue("NSA", 30)
        lNSasp = aTs.Attributes.GetValue("NSASP", 100)
        lNDp = aTs.Attributes.GetValue("NDP", 300)
        'create label on wdm file
        F90_WDLBAX(aFileUnit, lDsn, 1, lNDn, lNUp, lNSa, lNSasp, lNDp, lPsa)
        'write attributes
        DsnBld = DsnWriteAttributes(aFileUnit, aTs)
    End Function

    Private Function DsnWriteAttributes(ByVal aFileUnit As Integer, _
                                        ByVal aTs As atcTimeseries) As Boolean
        Dim lCSDat(6) As Integer
        Dim lDecade As Integer
        Dim lIVal As Integer
        Dim lStr As String
        Dim lTs As Integer
        Dim lTu As Integer

        Dim lMsg As atcWdmHandle = pMsg.MsgHandle
        Dim lMsgUnit As Integer = lMsg.Unit
        Dim lDsn As Integer = aTs.Attributes.GetValue("id", 0)

        'add needed attributes
        lStr = aTs.Attributes.GetValue("cons")
        If lStr.Length > 4 Then lStr = lStr.Substring(0, 4)
        aTs.Attributes.SetValueIfMissing("TSTYPE", lStr)
        aTs.Attributes.SetValueIfMissing("TGROUP", 6)
        aTs.Attributes.SetValueIfMissing("COMPFG", 1)
        aTs.Attributes.SetValueIfMissing("TSFORM", 1)
        aTs.Attributes.SetValueIfMissing("TSFILL", -999)

        lStr = aTs.Attributes.GetValue("tu")
        If lStr.Length = 0 Then
            CalcTimeUnitStep(aTs.Dates.Value(0), aTs.Dates.Value(1), lTu, lTs)
            aTs.Attributes.SetValue("tu", lTu)
            aTs.Attributes.SetValue("ts", lTs)
        Else
            lTu = lStr
            lTs = aTs.Attributes.GetValue("ts")
        End If
        lIVal = aTs.Attributes.GetValue("VBTIME", 1)
        If lTs > 1 Then lIVal = 2 'timestep > 1 vbtime must vary
        aTs.Attributes.SetValueIfMissing("VBTIME", lIVal)

        J2Date(aTs.Dates.Value(0), lCSDat)
        lDecade = lCSDat(0) Mod 10
        If lDecade > 0 Then 'subtract back to start of this decade
            lIVal = lCSDat(0) - lDecade
        Else 'back to start of previous decade
            lIVal = lCSDat(0) - 10
        End If
        aTs.Attributes.SetValue("TSBYR", lIVal)

        aTs.Attributes.SetValueIfMissing("scen", "")
        aTs.Attributes.SetValueIfMissing("cons", "")
        aTs.Attributes.SetValueIfMissing("locn", "")
        aTs.Attributes.SetValueIfMissing("desc", "")

        DsnWriteAttributes = True
        For lAttributeIndex As Integer = 0 To aTs.Attributes.Count - 1
            If Not DsnWriteAttribute(aFileUnit, lMsgUnit, lDsn, aTs.Attributes(lAttributeIndex)) Then
                DsnWriteAttributes = False
                Exit For
            End If
        Next

        If pErrorDescription.Length > 0 Then
            Logger.Dbg(pErrorDescription)
        End If

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

        Select Case (lName.ToLower)
            'Case "units"   'store Units ID as DCODE in WDM
            '    lName = "DCODE"
            '    lValue = CStr(GetUnitID(lValue))
            Case "time unit" : lName = "tcode"
            Case "time step" : lName = "tsstep"
            Case "time step" : lName = "tsstep"
            Case "scenario" : lName = "idscen"
            Case "location" : lName = "idlocn"
            Case "constituent" : lName = "idcons"
            Case "description" : lName = "descrp"
            Case "date created"
                lName = "datcre"
                lValue = DateToyyyyMMddHHmmss(lValue)
            Case "date modified"
                lName = "datmod"
                lValue = DateToyyyyMMddHHmmss(lValue)
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
            If lRetcod <> 0 Then
                If Math.Abs(lRetcod) = 104 Then 'cant update if data already present
                    Logger.Dbg("Skip:" & lName & ", data present")
                Else
                    If Len(pErrorDescription) = 0 Then
                        pErrorDescription = "Unable to Write Data Attribute to WDM"
                    End If
                    pErrorDescription &= vbCrLf & "  Attribute:" & lName & ", Value:" & lValue & ", Retcod:" & lRetcod
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

    Public Overrides Function Open(ByVal aFileName As String, _
                          Optional ByVal aAttributes As atcDataAttributes = Nothing) _
                                   As Boolean
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
            If lWdmHandle.Unit > 0 Then
                With aReadMe.Attributes
                    If Not CBool(.GetValue("HeaderComplete", False)) Then
                        DsnReadGeneral(lWdmHandle.Unit, lReadTS)
                    End If

                    If Not CBool(.GetValue("HeaderOnly", False)) Then
                        lReadTS.ValuesNeedToBeRead = False

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
                                If (lV(iVal) - lTsFill) < pEpsilon Then
                                    lVd(iVal) = pNan
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
                               ByRef aDataset As atcTimeseries)
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

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
End Class
