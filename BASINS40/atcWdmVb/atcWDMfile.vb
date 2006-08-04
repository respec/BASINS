'Copyright 2006 by AQUA TERRA Consultants - Royalty-free use permitted under open source license
Option Strict Off
Option Explicit On

Imports atcData
Imports atcData.atcDataSource.EnumExistAction
Imports atcUtility
Imports MapWinUtility
Imports atcWdmVb.atcWdmFileHandle

Public Class atcWDMfile
    Inherits atcData.atcDataSource

    Shared pMsgWdm As New atcMsgWDMvb
    Private Const pFileFilter As String = "WDM Files (*.wdm)|*.wdm"

    Public Overrides ReadOnly Property Category() As String
        Get
            Return "File"
        End Get
    End Property

    Public Overrides ReadOnly Property Description() As String
        Get
            Return "WDM Time Series VB"
        End Get
    End Property

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Timeseries::WDM VB"
        End Get
    End Property

    Public Overrides ReadOnly Property CanOpen() As Boolean
        Get
            Return True
        End Get
    End Property

    Public Overrides ReadOnly Property CanSave() As Boolean
        Get
            Return False 'TODO: change when this can save
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
                    Logger.Dbg("atcWDMfile:Open:User Cancelled File Dialogue for Open WDM file")
                    Open = False
                End If
            End With
        End If

        Dim lWdm As atcWdmFileHandle = Nothing
        If FileExists(aFileName) Then
            lWdm = New atcWdmFileHandle(0, aFileName)
        ElseIf FilenameNoPath(aFileName).Length > 0 Then
            Logger.Dbg("atcWDMfile:Open:WDM file " & aFileName & " does not exist - it will be created")
            MkDirPath(PathNameOnly(aFileName))
            lWdm = New atcWdmFileHandle(2, aFileName)
        Else
            Logger.Dbg("atcWDMfile:Open:File does not exist and cannot create '" & aFileName & "'")
            Open = False
        End If

        If Not lWdm Is Nothing Then
            Specification = aFileName
            Open = True 'assume the best
            'do some basic checks
            Dim lVal As Int32 = lWdm.ReadInt32(Wdm_Fields.PPRBKR)
            If lVal <> -998 Then
                Logger.Dbg("atcWDMfile:Open:PrimaryBackwardRecordPointer for FileDefinitionRecord:" & lVal & " should be -998")
                Open = False
            End If
            Refresh(lWdm)
            lWdm.Dispose()
            Return True 'Successfully opened
        Else
            Return False
        End If
    End Function

    Private Sub Refresh(ByVal aWdm As atcWdmFileHandle)
        Dim lProg As Integer = 0
        Dim lProgPrev As Integer

        DataSets.Clear()
        'pDates = Nothing
        'pDates = New ArrayList

        Dim lDsn As Int32 = aWdm.ReadInt32(Wdm_Fields.DSFST_Timeseries)
        While lDsn > 0
            Dim lRec As Int32 = aWdm.FirstRecordNumberFromDsn(lDsn)
            Logger.Dbg("Dsn: " & lDsn & " Rec: " & lRec)
            DataSets.Add(lDsn, DataSetFromWdm(aWdm, lDsn))
            lDsn = aWdm.ReadInt32(lRec, 2)
            If lDsn = 0 Then
                Logger.Progress("WDM Refresh Complete", 100, 0)
            Else 'try the next dsn
                lProgPrev = lProg
                lProg = (100 * lDsn) / 32000
                Logger.Progress("WDM Refresh", lProg, lProgPrev)
            End If
        End While
    End Sub

    Private Function DataSetFromWdm(ByVal aWdm As atcWdmFileHandle, ByVal aDsn As Int32) As atcTimeseries
        Dim lRec As Int32 = aWdm.FirstRecordNumberFromDsn(aDsn)
        Dim lDsn As Int32 = aWdm.ReadInt32(lRec, 5)
        If lDsn <> aDsn Then
            Throw New Exception("DataSetFromWdm:ExpectedDsn: " & aDsn & " FoundDsn: " & lDsn)
        End If
        Dim lDataSet As New atcTimeseries(Me)
        lDataSet.Dates = New atcTimeseries(Me)

        'generic attributes
        lDataSet.Attributes.SetValue("id", aDsn)
        lDataSet.Attributes.AddHistory("read fromVB " & Specification)

        AttributesFromWdm(aWdm, lDataSet, lRec)

        TimeseriesDataFromWdm(aWdm, lDataSet, lRec)

        Return lDataSet
    End Function

    Private Sub AttributesFromWdm(ByVal aWdm As atcWdmFileHandle, ByVal aDataSet As atcDataSet, ByVal aRec As Int32)
        'attributes
        Dim lPsa As Int32 = aWdm.ReadInt32(aRec, 10)
        Dim lSacnt As Int32 = aWdm.ReadInt32(aRec, lPsa)
        Dim lPSastr As Int32 = aWdm.ReadInt32
        Dim lAttributeDefinition As atcAttributeDefinition
        Logger.Dbg("  Psa,Sacnt,Psastr:" & lPsa & ":" & lSacnt & ":" & lPSastr)
        Dim lSaind(lSacnt) As Int32
        Dim lPos(lSacnt) As Int32
        For lInd As Int32 = 1 To lSacnt 'get index and location for available attributes
            lSaind(lInd) = aWdm.ReadInt32
            lPos(lInd) = aWdm.ReadInt32
            Logger.Dbg("    Ind:Saind:Pos:" & lInd & ":" & lSaind(lInd) & ":" & lPos(lInd))
        Next
        For lind As Int32 = 1 To lSacnt 'get the values
            lAttributeDefinition = pMsgWdm.Attributes.ItemByIndex(lSaind(lind))
            aWdm.Seek(aRec, lPos(lind))
            With lAttributeDefinition
                Select Case .TypeString
                    Case "Integer"
                        aDataSet.Attributes.SetValue(lAttributeDefinition, aWdm.ReadInt32)
                    Case "Single"
                        aDataSet.Attributes.SetValue(lAttributeDefinition, aWdm.ReadSingle)
                    Case "String"
                        Dim lS As String = ""
                        For lI As Int32 = 0 To (.Max / 4) - 1
                            Dim lV As Int32 = aWdm.ReadInt32
                            lS &= Long2String(lV)
                        Next
                        lS = lS.TrimEnd
                        Select Case UCase(.Name)
                            Case "DATCRE", "DATMOD", "DATE CREATED", "DATE MODIFIED"
                                Dim lDate As Date
                                lDate = New Date(CInt(lS.Substring(0, 4)), _
                                                 CInt(lS.Substring(4, 2)), _
                                                 CInt(lS.Substring(6, 2)), _
                                                 CInt(lS.Substring(8, 2)), _
                                                 CInt(lS.Substring(10, 2)), _
                                                 CInt(lS.Substring(12, 2)))
                                aDataSet.Attributes.SetValue(lAttributeDefinition, lDate)
                            Case "DCODE"
                                'lData.Attributes.SetValue(UnitsAttributeDefinition(True), GetUnitName(CInt(S)))
                            Case Else
                                aDataSet.Attributes.SetValue(lAttributeDefinition, lS)
                        End Select
                    Case Else
                        Logger.Msg("Help Me With Attribute Type " & .TypeString)
                End Select
            End With
        Next
    End Sub

    Private Sub TimeseriesDataFromWdm(ByVal aWdm As atcWdmFileHandle, ByVal aDataSet As atcTimeseries, ByVal aRec As Int32)
        Dim lTsbyr As Int32 = aDataSet.Attributes.GetValue("TSBYR", 1900)
        Dim lTsbmo As Int32 = aDataSet.Attributes.GetValue("TSBMO", 1)
        Dim lTsbdy As Int32 = aDataSet.Attributes.GetValue("TSBDY", 1)
        Dim lTsbhr As Int32 = aDataSet.Attributes.GetValue("TSBHR", 0)
        Dim lTsFill As Double = aDataSet.Attributes.GetValue("TSFILL", -999)
        Dim lBaseDateJ As Double = MJD(lTsbyr, lTsbmo, lTsbdy) + (lTsbhr / 24.0)
        Dim lTgroup As atcTimeUnit = aDataSet.Attributes.GetValue("TGROUP", 6)
        Logger.Dbg("  Group:BaseDate:" & lTgroup & ":" & lBaseDateJ & ":" & _
                    MJD2VBdate(lBaseDateJ) & ":" & _
                    lTsbyr & ":" & lTsbmo & ":" & lTsbdy & ":" & lTsbhr)

        Dim lPointDataBlocks As Int32 = aWdm.ReadInt32(aRec, 11) 'PDAT
        Dim lPointDataValues As Int32 = aWdm.ReadInt32 'PDATV
        Dim lDataPointerInUseCount As Int32 = aWdm.ReadInt32(aRec, lPointDataBlocks) 'DPCNT
        Dim lDataPointerCount As Int32 = lPointDataValues - lPointDataBlocks - 2
        Dim lDataPointerGroups(lDataPointerCount) As UInt32
        Dim lFreePosition As UInt32 = aWdm.ReadUInt32  'FREPOS
        Dim lFreeRecord As UInt32 = lFreePosition >> 9
        Dim lFreeOffset As UInt32 = lFreePosition And &H1FF
        'Logger.Dbg("  Pdat:Pdatv:Dpcnt:Frepos:" & _
        '            lPointDataBlocks & ":" & lPointDataValues & ":" & lDataPointerCount & _
        '            ":" & lFreePosition & "(" & lFreeRecord & "," & lFreeOffset & ")")

        'Dim ls As String = "  DataPointers: "
        Dim lInUse As Int32 = 0
        For lInd As Int32 = 1 To lDataPointerCount
            lDataPointerGroups(lInd) = aWdm.ReadUInt32
            'ls &= lDataPointerGroups(lInd) & ", "
            If lDataPointerGroups(lInd) <> 0 Then lInUse += 1
        Next
        'Logger.Dbg(ls.TrimEnd(",", " "))
        'Logger.Dbg("  DataPointersInUse:" & lInUse & ":" & lDataPointerInUseCount)

        Dim lData As New ArrayList
        lData.Add(Double.NaN)
        Dim lDate As New ArrayList
        Dim lCurrentDateJ As Double

        For lInd As Int32 = 1 To lDataPointerCount
            Dim lDataPointerGroup As UInt32 = lDataPointerGroups(lInd)
            If lDataPointerGroup > 0 Then 'data in this group
                Dim lDataRecord As UInt32 = lDataPointerGroup >> 9
                Dim lDataOffset As UInt32 = lDataPointerGroup And &H1FF
                'Logger.Dbg("  Group:Record:Offset" & lind & ":" & lDataRecord & ":" & lDataOffset)
                aWdm.Seek(lDataRecord, lDataOffset)
                Dim lGroupDate As UInt32 = aWdm.ReadUInt32
                lDataOffset += 1
                Dim lGroupDateJ As Double = WdmTimserGroupDate2JDate(lGroupDate)
                Dim lGroupDateJCalc As Double = TimAddJ(lBaseDateJ, lTgroup, 1, lInd - 1)
                If lGroupDateJ <> lGroupDateJCalc Then
                    Logger.Dbg("  Problem with group dates:" & lGroupDateJ & ":" & lGroupDateJCalc)
                End If
                'Logger.Dbg("  Group:Date:" & DumpDate(lGroupDateJ) & ":" & lGroupDateJ)
                lCurrentDateJ = lGroupDateJ
                Dim lEndDateJ As Double = TimAddJ(lGroupDateJ, lTgroup, 1, 1)
                Dim lBlockCount As Int32 = 0
                While (lEndDateJ - lCurrentDateJ) > 0.00001 'about 1 second
                    Dim lBlockStartDateJ As Double = lCurrentDateJ
                    Dim lBlockControlWord As UInt32 = aWdm.ReadUInt32
                    lBlockCount += 1
                    lDataOffset += 1
                    Dim lBlockNumVals As UInt32 = lBlockControlWord >> 16
                    Dim lBlockTimeStep As UInt32 = (lBlockControlWord >> 10) And &H3F
                    Dim lBlockTimeUnits As UInt32 = (lBlockControlWord >> 7) And &H7
                    Dim lBlockCompression As UInt32 = (lBlockControlWord >> 5) And &H3
                    Dim lBlockQuality As UInt32 = lBlockControlWord And &H1F
                    'If lBlockCount < 5 Then
                    '    Logger.Dbg("  Block:Date:Nov:TS:TU,Cmp:Qual:" & DumpDate(lCurrentDateJ) & ":" & _
                    '               lBlockNumVals & ":" & lBlockTimeStep & ":" & lBlockTimeUnits & ":" & _
                    '               lBlockCompression & ":" & lBlockQuality)
                    'End If
                    If lBlockCompression = 0 Then
                        For lPos As Int32 = 1 To lBlockNumVals
                            lDate.Add(lCurrentDateJ)
                            lCurrentDateJ = TimAddJ(lBlockStartDateJ, lBlockTimeUnits, lBlockTimeStep, lPos)
                            Dim lDataCurrent As Double = CDbl(aWdm.ReadSingle())
                            If Math.Abs(lDataCurrent - lTsFill) < Double.Epsilon Then
                                lDataCurrent = Double.NaN
                            End If
                            lData.Add(lDataCurrent)
                        Next
                        lDataOffset += lBlockNumVals
                    Else
                        Dim lDataComp As Double = CDbl(aWdm.ReadSingle())
                        If Math.Abs(lDataComp - lTsFill) < Double.Epsilon Then
                            lDataComp = Double.NaN
                        End If
                        For lPos As Int32 = 1 To lBlockNumVals
                            lDate.Add(lCurrentDateJ)
                            lCurrentDateJ = TimAddJ(lBlockStartDateJ, lBlockTimeUnits, lBlockTimeStep, lPos)
                            lData.Add(lDataComp)
                        Next
                        lDataOffset += 1
                    End If
                    If lDataOffset >= 512 Then 'move to next record
                        lDataRecord = aWdm.ReadInt32(CInt(lDataRecord), 4)
                        lDataOffset = 5
                        aWdm.Seek(lDataRecord, lDataOffset)
                        'Logger.Dbg("  NewRecord:" & lDataRecord)
                        lBlockCount = 0
                    End If
                    'If lBlockCount < 5 Then
                    '    Logger.Dbg("  EndBlockLoop:" & lCurrentDateJ & ":" & lEndDateJ)
                    'End If
                End While
            End If
        Next
        lDate.Add(lCurrentDateJ)
        Logger.Dbg("Done Dsn:DataCount:" & aDataSet.Attributes.GetValue("ID") & ":" & lData.Count)
        Dim lDataD(lData.Count - 1) As Double
        lData.CopyTo(lDataD)
        aDataSet.Values = lDataD
        Dim lDateD(lData.Count - 1) As Double
        lDate.CopyTo(lDateD)
        aDataSet.Dates.Values = lDateD
    End Sub

    Private Function WdmTimserGroupDate2JDate(ByVal aGroupDate As UInt32) As Double
        Dim lYr As UInt32 = aGroupDate >> 14
        Dim lMo As UInt32 = (aGroupDate >> 10) And &HF
        Dim lDy As UInt32 = (aGroupDate >> 5) And &H1F
        Dim lHr As UInt32 = aGroupDate And &H1F
        Dim lDateJ As Double = MJD(lYr, lMo, lDy) + (lHr / CDbl(24.0))
        Return lDateJ
    End Function

    Public Overrides Function ToString() As String
        Dim lBuilder As New Text.StringBuilder
        lBuilder.Append("WDMFileName = " & Specification & vbCrLf)
        'lBuilder.Append("Datasets" & vbCr)
        'For lInd As Integer = 1 To pDsnRecordPointer.GetUpperBound(0)
        'lBuilder.Append(Int32ToStringIfNotZero("Dsn(" & lInd & ") at ", pDsnRecordPointer(lInd)))
        'Next
        lBuilder.Append("Check" & vbCrLf)
        lBuilder.Append(Check(True))
        'Dim lRecordInUse As Int32() = RecordUsageMap()
        'For lInd As Integer = 1 To lRecordInUse.GetUpperBound(0)
        '    lBuilder.Append(Int32ToStringIfNotZero("Rec(" & lInd & ") use ", lRecordInUse(lInd)) & vbCr)
        'Next
        Return lBuilder.ToString
    End Function

    Friend Function Check(ByVal aVerbose As Boolean) As String
        Dim lWdm As New atcWdmFileHandle(1, Specification)
        Check = lWdm.Check(aVerbose)
        lWdm.Dispose()
    End Function
End Class