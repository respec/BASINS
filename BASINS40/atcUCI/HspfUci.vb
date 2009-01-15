'Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license
Option Strict Off
Option Explicit On

Imports System.Text
Imports System.Collections.ObjectModel
Imports System.Collections.Hashtable
Imports MapWinUtility
Imports atcUtility
Imports atcSegmentation
Imports atcData

Public Class HspfUci
    Declare Function GetCurrentProcessId Lib "kernel32" () As Integer

    Public Msg As HspfMsg = Nothing
    Public Name As String = ""
    Public Comment As String = ""
    Public Edited As Boolean = False

    Private pInitialized As Boolean = False
    Public Property Initialized() As Boolean
        Get
            If Not (pInitialized) Then
                pErrorDescription = "UCI File not Initialized"
            End If
            Return pInitialized
        End Get
        Set(ByVal Value As Boolean)
            pInitialized = Value
        End Set
    End Property

    Public FastFlag As Boolean = False
    Public AcidPhFlag As Boolean = False
    Public MetSegs As Collection(Of HspfMetSeg)

    Private pMsgWDMName As String
    Private pMsgUnit As Integer
    Private pWdmUnit(4) As Integer
    Private pWDMObj(4) As atcWDM.atcDataSourceWDM
    Private pWdmCount As Integer

    Private pGlobalBlk As HspfGlobalBlk
    Public Property GlobalBlock() As HspfGlobalBlk
        Get
            Return pGlobalBlk
        End Get
        Set(ByVal Value As HspfGlobalBlk)
            pGlobalBlk = Value
        End Set
    End Property

    Private pFilesBlk As HspfFilesBlk
    Public Property FilesBlock() As HspfFilesBlk
        Get
            Return pFilesBlk
        End Get
        Set(ByVal Value As HspfFilesBlk)
            pFilesBlk = Value
        End Set
    End Property

    Private pOpnSeqBlk As HspfOpnSeqBlk
    Public Property OpnSeqBlock() As HspfOpnSeqBlk
        Get
            Return pOpnSeqBlk
        End Get
        Set(ByVal Value As HspfOpnSeqBlk)
            pOpnSeqBlk = Value
        End Set
    End Property

    Private pOpnBlks As HspfOpnBlks
    Public ReadOnly Property OpnBlks() As KeyedCollection(Of String, HspfOpnBlk)
        Get
            Return pOpnBlks
        End Get
    End Property

    Private pConnections As Collection(Of HspfConnection)
    Public ReadOnly Property Connections() As Collection(Of HspfConnection)
        Get
            Return pConnections
        End Get
    End Property

    Private pMassLinks As Collection(Of HspfMassLink)
    Public ReadOnly Property MassLinks() As Collection(Of HspfMassLink)
        Get
            Return pMassLinks
        End Get
    End Property

    Private pPointSources As Collection(Of HspfPointSource)
    Public ReadOnly Property PointSources() As Collection(Of HspfPointSource)
        Get
            Return pPointSources
        End Get
    End Property

    Private pPollutants As Collection(Of HspfPollutant)
    Public ReadOnly Property Pollutants() As Collection(Of HspfPollutant)
        Get
            Return pPollutants
        End Get
    End Property

    Private pMonthData As HspfMonthData

    Private pErrorDescription As String = ""
    Public Property ErrorDescription() As String
        Get
            ErrorDescription = pErrorDescription
            pErrorDescription = ""
        End Get
        Set(ByVal Value As String)
            pErrorDescription = Value
        End Set
    End Property

    Private pSpecialActionBlk As HspfSpecialActionBlk
    Public ReadOnly Property SpecialActionBlk() As HspfSpecialActionBlk
        Get
            SpecialActionBlk = pSpecialActionBlk
        End Get
    End Property

    Private pCategoryBlk As HspfCategoryBlk
    Public Property CategoryBlock() As HspfCategoryBlk
        Get
            Return pCategoryBlk
        End Get
        Set(ByVal Value As HspfCategoryBlk)
            pCategoryBlk = Value
        End Set
    End Property

    Private pMaxAreaByLand2Stream As Double = 0.0
    Public Property MaxAreaByLand2Stream() As Double
        Get
            If pMaxAreaByLand2Stream = 0 Then
                CalcMaxAreaByLand2Stream()
            End If
            Return pMaxAreaByLand2Stream
        End Get
        Set(ByVal Value As Double)
            pMaxAreaByLand2Stream = Value
        End Set
    End Property

    Public StarterPath As String = ""

    Private pOrder As ArrayList 'for saving order of blocks

    Private pIcon As System.Drawing.Image
    Public Property Icon() As System.Drawing.Image
        Get
            Return pIcon
        End Get
        Set(ByVal Value As System.Drawing.Image)
            pIcon = Value
            'TODO: myMsgBox.icon = Value
        End Set
    End Property

    Private pIPC As Object 'ATCoCtl.ATCoIPC
    Private pIPCset As Boolean = False
    Private pNaN As Double = GetNaN()

    Public Sub SendHspfMessage(ByVal aMessage As String)
        If pIPCset Then
            pIPC.SendProcessMessage("HSPFUCI", aMessage)
        End If
    End Sub

    Public Sub SendMonitorMessage(ByVal aMessage As String)
        If pIPCset Then
            pIPC.SendMonitorMessage(aMessage)
        End If
    End Sub

    'Public Property IPC() As Object
    '    Get
    '        Return pIPC
    '    End Get
    '    Set(ByVal Value As Object)
    '        pIPC = Value
    '        If pIPC Is Nothing Then IPCset = False Else IPCset = True

    '        'With pHspfEngine.launch
    '        '.SendComputeMessage "SPIPH " & .ComputeReadFromParent & " " & .ComputeWriteToParent
    '        '.SendComputeMessage "W99OPN"
    '        'Call F90_W99OPN  'open error file for fortan problems
    '        'Call F90_WDBFIN  'initialize WDM record buffer
    '        '.SendComputeMessage "PUTOLV 10"
    '        'Call F90_PUTOLV(10)
    '        'Call F90_SCNDBG(-1)
    '        'End With
    '    End Set
    'End Property

    'Public WriteOnly Property HelpFile() As String
    '	Set(ByVal Value As String)
    '		'UPGRADE_ISSUE: App property App.HelpFile was not upgraded. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="076C26E5-B7A9-4E77-B69C-B4448DF39E58"'
    '		App.HelpFile = Value
    '	End Set
    'End Property

    Public WriteOnly Property StatusIn() As Integer
        Set(ByVal Value As Integer)
            'pStatusIn = newStatusIn
        End Set
    End Property

    Public WriteOnly Property StatusOut() As Integer
        Set(ByVal Value As Integer)
            'pStatusOut = newStatusOut
        End Set
    End Property

    Public ReadOnly Property MonthData() As HspfMonthData
        Get
            Return pMonthData
        End Get
    End Property

    Public WriteOnly Property MsgWDMName() As String
        Set(ByVal Value As String)
            pMsgWDMName = Value
        End Set
    End Property

    Public ReadOnly Property WDMCount() As Integer
        Get
            Return pWdmCount
        End Get
    End Property

    Public Property MessageUnit() As Integer
        Get
            Return pMsgUnit
        End Get
        Set(ByVal Value As Integer)
            If Value = 0 Then
                F90_MSGUNIT(pMsgUnit)
            Else 'could check to be sure?
                pMsgUnit = Value
            End If
        End Set
    End Property

    Public Overrides Function ToString() As String
        Dim lSB As New StringBuilder
        If Comment.Length > 0 Then
            lSB.AppendLine(Comment)
        End If
        lSB.AppendLine("RUN")

        For Each lBlock As String In pOrder
            Dim lStr As String = ""
            Logger.Dbg("Write " & lBlock)
            Select Case lBlock
                Case "GLOBAL"
                    lStr = pGlobalBlk.ToString
                Case "FILES"
                    lStr = pFilesBlk.ToString
                Case "CATEGORY"
                    If Not pCategoryBlk Is Nothing AndAlso pCategoryBlk.Categories.Count > 0 Then
                        lStr = pCategoryBlk.ToString
                    End If
                Case "OPN SEQUENCE"
                    lStr = pOpnSeqBlk.ToString
                Case "MONTH DATA"
                    If Not pMonthData Is Nothing Then
                        lStr = pMonthData.ToString
                    End If
                Case "FTABLES"
                    If pOpnBlks.Contains("RCHRES") Then
                        Dim lOpnBlk As HspfOpnBlk = OpnBlks.Item("RCHRES")
                        If lOpnBlk.Count > 0 Then
                            lStr = lOpnBlk.Ids.Item(0).FTable.ToString
                        End If
                    End If
                Case "PERLND", "IMPLND", "RCHRES", "COPY", "PLTGEN", "DISPLY", _
                     "DURANL", "GENER", "MUTSIN", "BMPRAC", "REPORT"
                    If pOpnBlks.Contains(lBlock) Then
                        Dim lOpnBlk As HspfOpnBlk = pOpnBlks.Item(lBlock)
                        If lOpnBlk.Count > 0 Then
                            lStr = lOpnBlk.ToString
                        End If
                    End If
                Case "CONNECTIONS"
                    If pConnections.Count > 0 Then
                        lStr = pConnections.Item(0).ToString
                    End If
                Case "MASSLINKS"
                    If pMassLinks.Count > 0 Then
                        lStr = pMassLinks.Item(0).ToString
                    End If
                Case "SPECIAL ACTIONS"
                    If Not pSpecialActionBlk Is Nothing Then
                        lStr = pSpecialActionBlk.ToString
                    End If
            End Select
            If lStr.Length > 0 Then
                lSB.AppendLine(lStr)
            End If
        Next
        lSB.AppendLine("END RUN")

        Return lSB.ToString
    End Function

    Public Sub Save()
        IO.File.WriteAllText(Name, Me.ToString)
        Edited = False
    End Sub

    Public Sub SaveAs(ByRef aOldName As String, ByRef aNewName As String, _
                      ByRef aBaseDsn As Integer, ByRef aRelAbs As Integer)
        If aOldName <> aNewName Then
            pFilesBlk.NewName(aOldName, aNewName)
            newOutputDsns(aOldName, aNewName, aBaseDsn, aRelAbs)
        End If
        Save()
    End Sub

    Public Sub New()
        pOpnSeqBlk = New HspfOpnSeqBlk
        pConnections = New Collection(Of HspfConnection)
        pGlobalBlk = New HspfGlobalBlk
        pOpnBlks = New HspfOpnBlks
        pFilesBlk = New HspfFilesBlk
        MetSegs = New Collection(Of HspfMetSeg)
        pPointSources = New Collection(Of HspfPointSource)
        pMassLinks = New Collection(Of HspfMassLink)
        pPollutants = New Collection(Of HspfPollutant)

        pOrder = DefaultBlockOrder()
    End Sub

    'Public Sub CreateUci(ByRef M As HspfMsg, ByRef newName As String, ByRef outputwdm As String, ByRef metwdms() As String, ByRef wdmids() As String, ByRef MetDataDetails As String, ByRef oneseg As Boolean, ByRef PollutantList As Collection)

    '    'Call F90_SPIPH(pStatusIn, pStatusOut)
    '    Call CreateUciFromBASINS(Me, M, newName, outputwdm, metwdms, wdmids, MetDataDetails, oneseg, PollutantList)
    'End Sub

    Public Sub FastReadUciForStarter(ByRef aMsg As HspfMsg, ByRef aNewName As String)
        Dim lFilesOK As Boolean
        Dim lFullFg As Integer
        Dim lEchoFile As String = ""

        FastFlag = True
        lFullFg = -1
        ReadUci(aMsg, aNewName, lFullFg, lFilesOK, lEchoFile)
        FastFlag = False
    End Sub

    Public Sub FastReadUci(ByRef aMsg As HspfMsg, ByRef aNewName As String)
        'called by scripthspf, processes wdm files
        Dim lFilesOK As Boolean
        Dim lFullFg As Integer
        Dim lEchoFile As String = ""

        FastFlag = True
        lFullFg = -3
        ReadUci(aMsg, aNewName, lFullFg, lFilesOK, lEchoFile)
        FastFlag = False
    End Sub

    ''' <summary>
    ''' Read UCI file into this class
    ''' </summary>
    ''' <param name="aMsg">HspfMsg file object</param>
    ''' <param name="aNewName">File to read</param>
    ''' <param name="aFullFg">-3 = , -1 = starter</param>
    ''' <param name="aFilesOK">gets set to True if files are ok, false if not</param>
    ''' <param name="aEchoFile"></param>
    ''' <remarks></remarks>
    Public Sub ReadUci(ByRef aMsg As HspfMsg, _
                       ByRef aNewName As String, _
                       ByRef aFullFg As Integer, _
                       ByRef aFilesOK As Boolean, _
                       ByRef aEchoFile As String)
        Msg = aMsg

        If Not IO.File.Exists(aNewName) Then
            pErrorDescription = "UciFileName '" & aNewName & "' not found"
        Else
            Name = aNewName
            Logger.Dbg("UCIRecordCount " & ReadUCIRecords(Name))

            If aFullFg <> -1 Then 'not doing starter, process wdm files
                aFilesOK = PreScanFilesBlock(aEchoFile)
                aEchoFile = aEchoFile.Trim
            Else
                aFilesOK = True
            End If

            If aFilesOK Then
                Dim lName As String = IO.Path.GetFileNameWithoutExtension(Name)
                Dim lFlag As Integer
                If aFullFg = -3 Then
                    lFlag = aFullFg
                Else
                    lFlag = -2 'flag as coming from hspf class for status title
                End If

                If Not FastFlag Then 'do normal activate of uci, including run interpreter
                    SendHspfMessage("CURDIR " & CurDir())
                    SendHspfMessage("ACTIVATE " & lName & " " & lFlag)
                    Dim lMsg As String = WaitForChildMessage()
                    If lMsg.StartsWith("CURDIR") Then
                        lMsg = WaitForChildMessage()
                    End If
                    If CDbl(Right(lMsg, 1)) <> 0 Or lMsg.StartsWith("HSPFUCI exited with code") Then
                        pErrorDescription = "Error interpreting UCI File '" & lName & "'." & vbCrLf & vbCrLf & _
                                            "See the file '" & aEchoFile.Trim & "' for more details." & vbCrLf & _
                                            "Message " & lMsg
                        SendMonitorMessage(pErrorDescription)
                    End If
                    FastFlag = True
                End If

                pInitialized = True

                SendMonitorMessage("(Show)") 'where was the hide?
                If Not FastFlag Then
                    SendMonitorMessage("(Msg1 Building Collections)")
                End If

                SaveBlockOrder(pOrder)

                Comment = GetCommentBeforeBlock("RUN")

                pGlobalBlk = New HspfGlobalBlk
                pGlobalBlk.Uci = Me
                pGlobalBlk.ReadUciFile()

                pFilesBlk = New HspfFilesBlk
                pFilesBlk.Uci = Me
                pFilesBlk.ReadUciFile()

                pCategoryBlk = New HspfCategoryBlk
                pCategoryBlk.Uci = Me
                pCategoryBlk.ReadUciFile()

                pMonthData = New HspfMonthData
                pMonthData.Uci = Me
                pMonthData.ReadUciFile()

                pOpnSeqBlk = New HspfOpnSeqBlk
                pOpnSeqBlk.Uci = Me
                pOpnSeqBlk.ReadUciFile()

                pOpnBlks.Clear()
                Dim lOperIndex As Integer = 1
                Dim lOpnName As String = HspfOperName(lOperIndex)
                Dim lOpnblk As HspfOpnBlk
                While lOpnName <> "UNKNOWN"
                    lOpnblk = New HspfOpnBlk
                    lOpnblk.Name = lOpnName
                    lOpnblk.Uci = Me
                    pOpnBlks.Add(lOpnblk)
                    lOperIndex += 1
                    lOpnName = HspfOperName(lOperIndex)
                End While
                For Each lOpn As HspfOperation In pOpnSeqBlk.Opns
                    lOpnblk = pOpnBlks.Item(lOpn.Name)
                    lOpnblk.Ids.Add(lOpn)
                    lOpn.OpnBlk = lOpnblk
                Next
                Logger.Dbg("GeneralBlocksRead")

                For Each lOpnblk In pOpnBlks 'perlnd, implnd, etc
                    If lOpnblk.Count > 0 Then
                        lOpnblk.setTableValues(Msg.BlockDefs(lOpnblk.Name))
                        Logger.Dbg(lOpnblk.Name & " BlockRead")
                    End If
                Next
                Logger.Dbg("OperationBlocksRead")

                pSpecialActionBlk = New HspfSpecialActionBlk
                pSpecialActionBlk.Uci = Me
                pSpecialActionBlk.ReadUciFile()
                Logger.Dbg("SpecialActionBlockRead")

                ProcessFTables()
                Logger.Dbg("FtableBlockRead")

                pConnections = Nothing
                pConnections = New Collection(Of HspfConnection)
                Dim lConnection As New HspfConnection 'dummy to get entry point
                lConnection.ReadTimSer(Me)
                lConnection = Nothing
                For Each lOpn As HspfOperation In pOpnSeqBlk.Opns
                    lOpn.SetTimSerConnections()
                Next
                Logger.Dbg("ConnectionBlocksRead")

                pMassLinks.Clear()
                Dim lMassLink As New HspfMassLink
                lMassLink.readMassLinks(Me)
                Logger.Dbg("MassLinkBlockRead")

                'look for met segments
                Source2MetSeg()
                Logger.Dbg("MetSegmentsCreated " & MetSegs.Count)

                'look for point loads
                Source2Point()
                Logger.Dbg("PointSources " & pPointSources.Count)

                SendMonitorMessage("(Hide)")
            End If
        End If
        Edited = False 'all the reads set edited
    End Sub

    Public Sub CalcMaxAreaByLand2Stream()
        Dim lMaxArea As Double = 0.0
        If pInitialized Then
            Dim lOperationTypes() As String = {"RCHRES", "BMPRAC"} 'operations with contrib landuse area
            For Each lOperationType As String In lOperationTypes
                For Each lOperation As HspfOperation In pOpnBlks.Item(lOperationType).Ids 'each operation
                    For Each lConnection As HspfConnection In lOperation.Sources
                        Dim lCurrArea As Double = 0.0
                        If lConnection.Source.VolName = "PERLND" Or _
                           lConnection.Source.VolName = "IMPLND" Then
                            lCurrArea = lConnection.MFact
                            For Each lSourceConnection As HspfConnection In lOperation.Sources
                                If lSourceConnection.Source.VolName = "PERLND" Or _
                                   lSourceConnection.Source.VolName = "IMPLND" Or _
                                   lSourceConnection.Source.VolName = "BMPRAC" Then
                                    If Not lSourceConnection.Source.Opn Is Nothing And Not lConnection.Source.Opn Is Nothing Then
                                        If lSourceConnection.Source.Opn.Description = lConnection.Source.Opn.Description Then 'more
                                            lCurrArea += lSourceConnection.MFact
                                        End If
                                    End If
                                End If
                            Next lSourceConnection
                        End If
                        If lCurrArea > lMaxArea Then
                            lMaxArea = lCurrArea
                        End If
                    Next lConnection
                Next
            Next lOperationType
        End If
        pMaxAreaByLand2Stream = lMaxArea
    End Sub

    Public Sub Source2MetSeg()
        Dim lOperationTypes() As String = {"PERLND", "IMPLND", "RCHRES"}
        For Each lOperationType As String In lOperationTypes
            For Each lOperation As HspfOperation In pOpnBlks.Item(lOperationType).Ids
                Dim lMetSeg As New HspfMetSeg 'init moved here
                lMetSeg.Uci = Me
                Dim lComment As String = ""
                Dim lSourceIndex As Integer = 0
                Do While lSourceIndex < lOperation.Sources.Count
                    Dim lConnection As HspfConnection = lOperation.Sources.Item(lSourceIndex)
                    If lConnection.Typ = 1 Then
                        If lMetSeg.Add(lConnection) Then
                            lOperation.Sources.RemoveAt(lSourceIndex)
                            If lComment.Length = 0 Then
                                lComment = lConnection.Comment
                            End If
                        Else
                            lSourceIndex += 1
                        End If
                    Else
                        lSourceIndex += 1
                    End If
                Loop

                'check to see if we already have this met segment
                Dim lNewSeg As Boolean = True
                If MetSegs.Count > 0 Then
                    For Each lMetSegExisting As HspfMetSeg In MetSegs
                        If lMetSegExisting.Compare(lMetSeg, lOperation.Name) Then
                            lNewSeg = False
                            If lOperation.Name = "RCHRES" Then
                                'may need to update met seg
                                lMetSegExisting.UpdateMetSeg(lMetSeg)
                            End If
                            lOperation.MetSeg = lMetSegExisting
                            Exit For
                        End If
                    Next lMetSegExisting
                End If

                If lNewSeg Then
                    lMetSeg.Id = MetSegs.Count + 1
                    'get met seg name from precip data set
                    If lMetSeg.MetSegRecs.Count > 0 AndAlso _
                       lMetSeg.MetSegRecs.Contains("PREC") AndAlso _
                       lMetSeg.MetSegRecs("PREC").Source.VolId > 0 Then
                        With lMetSeg.MetSegRecs("PREC").Source
                            If pWdmCount > 0 Then
                                lMetSeg.ExpandMetSegName(.VolName, .VolId)
                            Else
                                If lComment.Length > 13 Then
                                    lMetSeg.Name = lComment.Substring(12)
                                Else
                                    lMetSeg.Name = lComment
                                End If
                            End If
                        End With
                        MetSegs.Add(lMetSeg)
                        lOperation.MetSeg = lMetSeg
                    Else 'need in case there is no prec in the met seg
                        lMetSeg.Name = ""
                        MetSegs.Add(lMetSeg)
                        lOperation.MetSeg = lMetSeg
                    End If
                    lMetSeg = New HspfMetSeg
                    lMetSeg.Uci = Me
                End If
            Next
            Dim lStr As String = "MetSegsComplete for " & lOperationType & " Count " & MetSegs.Count
            For Each lMetSeg As HspfMetSeg In MetSegs
                lStr &= " '" & lMetSeg.Id & ":<" & lMetSeg.Name & ">'"
            Next
            Logger.Dbg(lStr)
        Next lOperationType

        'set any undefined mfacts to 0
        If MetSegs.Count > 0 Then
            For Each lMetSegExisting As HspfMetSeg In MetSegs
                For Each lMetSegRecord As HspfMetSegRecord In lMetSegExisting.MetSegRecs
                    If lMetSegRecord.MFactP = -999.0# Then
                        lMetSegRecord.MFactP = 0
                    End If
                    If lMetSegRecord.MFactR = -999.0# Then
                        lMetSegRecord.MFactR = 0
                    End If
                Next lMetSegRecord
            Next lMetSegExisting
        End If
    End Sub

    Public Sub Source2Point()
        Dim lLastId As Integer = 0
        Dim lOperationTypes() As String = {"RCHRES", "COPY"} 'operations with assoc pt srcs
        For Each lOperationType As String In lOperationTypes
            For Each lOpn As HspfOperation In pOpnBlks.Item(lOperationType).Ids
                Dim lSourceIndex As Integer = 0
                Do While lSourceIndex < lOpn.Sources.Count
                    Dim lConnection As HspfConnection = lOpn.Sources.Item(lSourceIndex)
                    If (lConnection.Target.VolName = lOperationType And _
                        lConnection.Target.Group <> "EXTNL") And _
                        (lConnection.Source.VolName.StartsWith("WDM")) Then
                        'if wdm data set to rchres add to collection,
                        'or if wdm data set to copy and copy goes to rchres
                        Dim lNewPoint As Boolean = False
                        Dim lRFact As Single
                        If lConnection.Target.VolName = "COPY" Then
                            lRFact = 0
                            For lIndex As Integer = 1 To lConnection.Target.Opn.Targets.Count
                                If lConnection.Target.Opn.Targets.Item(lIndex).Target.VolName = "RCHRES" Then
                                    lNewPoint = True
                                    'sum up the mfacts (really for septic modeling)
                                    lRFact += lConnection.Target.Opn.Targets.Item(lIndex).MFact
                                End If
                            Next lIndex
                        ElseIf lConnection.Target.VolName = "RCHRES" Then
                            lNewPoint = True
                        End If
                        If lNewPoint Then
                            If Trim(lConnection.Source.VolName) = "WDM" Then
                                lConnection.Source.VolName = "WDM1"
                            End If
                            Dim lPoint As New HspfPointSource
                            lPoint.MFact = lConnection.MFact
                            If lConnection.Target.VolName = "COPY" Then
                                'save rfact for septics
                                lPoint.RFact = lRFact
                            End If
                            lPoint.Source = lConnection.Source
                            lPoint.Tran = lConnection.Tran
                            lPoint.Sgapstrg = lConnection.Sgapstrg
                            lPoint.Ssystem = lConnection.Ssystem
                            lPoint.Target = lConnection.Target
                            'pbd -- store associated operation id for use when writing
                            lPoint.AssocOperationId = lOpn.Id
                            'get point source name from any data set
                            If lPoint.Source.VolName.StartsWith("WDM") Then
                                Dim lDsn As Integer = lPoint.Source.VolId
                                If lDsn > 0 Then
                                    Dim lWdmId As String = lPoint.Source.VolName
                                    If pWdmCount > 0 Then
                                        lPoint.Name = GetWDMAttr(lWdmId, lDsn, "DESC")
                                        lPoint.Con = GetWDMAttr(lWdmId, lDsn, "CON")
                                    End If
                                End If
                            Else
                                lPoint.Name = lPoint.Source.VolName & " " & lPoint.Source.VolId
                                lPoint.Con = ""
                            End If
                            lPoint.Comment = lConnection.Comment
                            For Each lPointExisting As HspfPointSource In pPointSources
                                If lPointExisting.Name = lPoint.Name Then
                                    lPoint.Id = lPointExisting.Id
                                    Exit For
                                End If
                            Next lPointExisting
                            If lPoint.Id = 0 Then
                                lLastId += 1
                                lPoint.Id = lLastId
                            End If
                            pPointSources.Add(lPoint)
                            lOpn.PointSources.Add(lPoint)
                            lOpn.Sources.RemoveAt(lSourceIndex)
                        Else
                            lSourceIndex += 1
                        End If
                    Else
                        lSourceIndex += 1
                    End If
                Loop
            Next
        Next lOperationType
    End Sub

    Public Sub Point2Source()
        Dim lOperationTypes() As String = {"RCHRES", "COPY"} 'operations with assoc pt srcs
        For Each lOperationType As String In lOperationTypes
            For Each lOpn As HspfOperation In pOpnBlks.Item(lOperationType).Ids
                For Each lPoint As HspfPointSource In lOpn.PointSources
                    Dim lConn As HspfConnection = New HspfConnection
                    lConn.Uci = Me
                    If lPoint.Source.VolName = "MUTSIN" Then
                        lConn.Typ = 2
                    Else
                        lConn.Typ = 1
                    End If
                    lConn.Source = lPoint.Source
                    lConn.Ssystem = lPoint.Ssystem
                    lConn.Sgapstrg = lPoint.Sgapstrg
                    lConn.MFact = lPoint.MFact
                    lConn.Tran = lPoint.Tran
                    lConn.Target = lPoint.Target
                    'Me.Connections.Add lConn
                    lOpn.Sources.Add(lConn)
                Next lPoint
                'now remove all point sources
                lOpn.PointSources.Clear()
            Next
        Next lOperationType

        'now remove all point sources
        pPointSources.Clear()

        'need to synch collection of connections with opn connections
        RemoveConnectionsFromCollection(1) 'remove all type ext src
        For Each lOpn As HspfOperation In Me.OpnSeqBlock.Opns
            For lSourceIndex As Integer = 1 To lOpn.Sources.Count
                Dim lConn As HspfConnection = lOpn.Sources.Item(lSourceIndex - 1)
                If lConn.Typ = 1 Then
                    Me.Connections.Add(lConn)
                End If
            Next lSourceIndex
        Next
    End Sub

    Public Sub MetSeg2Source()
        Dim lOperationTypes() As String = {"PERLND", "IMPLND", "RCHRES"} 'operations with assoc met segs
        For Each lOperationType As String In lOperationTypes
            For Each lOperation As HspfOperation In pOpnBlks.Item(lOperationType).Ids
                If Not lOperation.MetSeg Is Nothing Then
                    For Each lMetSegRecord As HspfMetSegRecord In lOperation.MetSeg.MetSegRecs
                        With lMetSegRecord
                            If (lOperation.Name = "RCHRES" And .MFactR > 0.0#) Or _
                                   (lOperation.Name = "PERLND" And .MFactP > 0.0#) Or _
                                   (lOperation.Name = "IMPLND" And .MFactP > 0.0#) Then
                                Dim lConnection As New HspfConnection
                                lConnection.Uci = Me
                                lConnection.Typ = 1
                                'set source components
                                lConnection.Source.Group = .Source.Group
                                lConnection.Source.Member = .Source.Member
                                lConnection.Source.MemSub1 = .Source.MemSub1
                                lConnection.Source.MemSub2 = .Source.MemSub2
                                lConnection.Source.VolId = .Source.VolId
                                lConnection.Source.VolIdL = .Source.VolIdL
                                lConnection.Source.VolName = .Source.VolName
                                lConnection.Ssystem = .Ssystem
                                lConnection.Sgapstrg = .Sgapstrg
                                lConnection.Target.Group = "EXTNL"
                                If lOperation.Name = "RCHRES" Then
                                    lConnection.MFact = .MFactR
                                    Select Case .Name
                                        Case "PREC" : lConnection.Target.Member = "PREC"
                                        Case "ATEM" : lConnection.Target.Member = "GATMP"
                                        Case "DEWP" : lConnection.Target.Member = "DEWTMP"
                                        Case "WIND" : lConnection.Target.Member = "WIND"
                                        Case "SOLR" : lConnection.Target.Member = "SOLRAD"
                                        Case "CLOU" : lConnection.Target.Member = "CLOUD"
                                        Case "PEVT" : lConnection.Target.Member = "POTEV"
                                    End Select
                                Else
                                    lConnection.MFact = .MFactP
                                    Select Case .Name
                                        Case "PREC" : lConnection.Target.Member = "PREC"
                                        Case "ATEM" : lConnection.Target.Member = "GATMP"
                                        Case "DEWP" : lConnection.Target.Member = "DTMPG"
                                        Case "WIND" : lConnection.Target.Member = "WINMOV"
                                        Case "SOLR" : lConnection.Target.Member = "SOLRAD"
                                        Case "CLOU" : lConnection.Target.Member = "CLOUD"
                                        Case "PEVT" : lConnection.Target.Member = "PETINP"
                                    End Select
                                    If .Name = "ATEM" Then
                                        'get right air temp member name
                                        If lOperation.MetSeg.AirType = 1 Then
                                            lConnection.Target.Member = "GATMP"
                                        ElseIf lOperation.MetSeg.AirType = 2 Then
                                            lConnection.Target.Member = "AIRTMP"
                                            lConnection.Target.Group = "ATEMP"
                                        End If
                                    End If
                                End If
                                lConnection.Tran = .Tran
                                lConnection.Target.VolName = lOperation.Name
                                lConnection.Target.VolId = lOperation.Id
                                'Me.Connections.Add lConn
                                lOperation.Sources.Add(lConnection)
                            End If
                        End With
                    Next lMetSegRecord
                End If
            Next
        Next lOperationType

        'now remove all metsegs
        MetSegs.Clear()

        'need to synch collection of connections with opn connections
        RemoveConnectionsFromCollection(1) 'remove all type ext src
        For Each lOpn As HspfOperation In Me.OpnSeqBlock.Opns
            For Each lConnection As HspfConnection In lOpn.Sources
                If lConnection.Typ = 1 Then
                    Me.Connections.Add(lConnection)
                End If
            Next lConnection
        Next
    End Sub

    Public Sub RunUci(ByRef aReturnCode As Integer)
        'Call F90_SCNDBG(10)
        Dim lPath As String = IO.Path.GetDirectoryName(Name)
        If lPath.Length > 0 Then
            ChDriveDir(lPath)
        End If

        Dim lReturnCode As Integer
        ReportMissingTimsers(lReturnCode)
        If lReturnCode = 0 Then 'user chose do anyway after timser warning
            Dim lOption As Integer = -1 'dont interp in actscn (itll be done in simscn)
            'Call F90_ACTSCN(i, pWdmUnit(1), pMsgUnit, r, s, Len(s))
            'Call F90_SIMSCN(retcod)

            If lPath.Length > 0 Then
                SendHspfMessage("CURDIR " & lPath)
            End If
            Dim lFileName As String = IO.Path.GetFileNameWithoutExtension(Name)
            SendHspfMessage("ACTIVATE " & lFileName & " " & lOption)
            Dim lMsg As String = WaitForChildMessage()
            SendHspfMessage("SIMULATE") 'calls F90_SIMSCN
            lMsg = WaitForChildMessage()
            lMsg = WaitForChildMessage() 'Activate Complete
            While CDbl(UCase(CStr(InStr(lMsg, "PROGRESS")))) > 0
                lMsg = WaitForChildMessage()
            End While
            'Stop 'What should we be doing here exactly? Can't do GetExitCodeProcess any more.
            'ret = GetExitCodeProcess(Me.Monitor.launch.hComputeProcess, i)
            'If i <> &H103 Then
            'need to restart hspfengine
            If IsNumeric(Right(lMsg, 2)) Then
                aReturnCode = CInt(Right(lMsg, 2))
            End If

            RestartHSPFEngine()
            'have to reset wdms, may have changed pointers during simulate
            ClearWDM()
            InitWDMArray()
            SetWDMFiles()

            If IsNumeric(Right(lMsg, 1)) Then
                aReturnCode = CInt(Right(lMsg, 1))
            End If
            'next line fixed 10/28/03 to handle new ipc return message
            If CDbl(Right(lMsg, 1)) <> 0 Or lMsg.StartsWith("HSPFUCI exited with code") Then
                pErrorDescription = "Fatal HSPF error while running UCI file '" & lFileName & "'." & vbCrLf & vbCrLf & "See the file '" & EchoFileName() & "' for more details."
                SendMonitorMessage(pErrorDescription)
            End If
        End If
    End Sub

    Public Sub DeleteOperation(ByRef aName As String, ByRef aId As Integer)
        'figure out where this operation is in operation sequence block and delete it
        Dim lOperationIndex As Integer = 1
        For Each lOpn As HspfOperation In pOpnSeqBlk.Opns
            If lOpn.Name = aName And _
               lOpn.Id = aId Then
                pOpnSeqBlk.Delete(lOperationIndex)
            End If
            lOperationIndex += 1
        Next

        'need to remove from all operation type blocks
        Dim lOpnBlk As HspfOpnBlk = pOpnBlks.Item(aName)
        If Not lOpnBlk.OperFromID(aId) Is Nothing Then
            lOpnBlk.Ids.Remove("K" & aId)
        End If

        'remove connections
        'need to remove connections between this and anything else
        Dim lConnectionIndex As Integer = 1
        Dim lTargetVolId As Integer = 0
        Dim lSourceCount As Integer = 0
        Dim lSourceVolId() As Integer = {}
        Dim lMassLink As Integer
        For Each lConnection As HspfConnection In Me.Connections
            If (lConnection.Source.VolName = aName And lConnection.Source.VolId = aId) Or (lConnection.Target.VolName = aName And lConnection.Target.VolId = aId) Then
                lMassLink = lConnection.MassLink
                If lConnection.Target.VolId = aId And lConnection.Target.VolName = aName And lConnection.Source.VolName = aName Then
                    'remember the source
                    lSourceCount += 1
                    ReDim Preserve lSourceVolId(lSourceCount)
                    lSourceVolId(lSourceCount) = lConnection.Source.VolId
                ElseIf lConnection.Source.VolId = aId And lConnection.Source.VolName = aName And lConnection.Target.VolName = aName Then
                    'remember the target
                    lTargetVolId = lConnection.Target.VolId
                End If
                Me.Connections.RemoveAt(lConnectionIndex)
            Else
                lConnectionIndex += 1
            End If
        Next lConnection

        If lSourceCount > 0 And lTargetVolId > 0 Then
            'need to join sources and targets of this deleted opn
            For lConnectionIndex = 1 To lSourceCount
                Dim lConnection As HspfConnection = New HspfConnection
                lConnection.Uci = Me
                lConnection.Typ = 3
                lConnection.Source.VolName = aName
                lConnection.Source.VolId = lSourceVolId(lConnectionIndex)
                lConnection.Source.Opn = pOpnBlks.Item(aName).OperFromID(lSourceVolId(lConnectionIndex))
                lConnection.MFact = 1.0#
                lConnection.Target.VolName = aName
                lConnection.Target.VolId = lTargetVolId
                lConnection.Target.Opn = pOpnBlks.Item(aName).OperFromID(lTargetVolId)
                If lMassLink > 0 Then
                    lConnection.MassLink = lMassLink
                Else
                    lConnection.MassLink = 3
                End If
                Me.Connections.Add(lConnection)
                lConnection.Source.Opn.Targets.Add(lConnection)
                lConnection.Target.Opn.Sources.Add(lConnection)
            Next lConnectionIndex
        End If

        'remove this oper from source and target collections for other operations
        For Each lOpn As HspfOperation In pOpnSeqBlk.Opns
            Dim lTargetIndex As Integer = 1
            Do While lTargetIndex <= lOpn.Targets.Count
                If lOpn.Targets.Item(lTargetIndex).Target.VolId = aId And _
                   lOpn.Targets.Item(lTargetIndex).Target.VolName = aName Then
                    lOpn.Targets.RemoveAt(lTargetIndex)
                Else
                    lTargetIndex += 1
                End If
            Loop
            lTargetIndex = 1
            Do While lTargetIndex <= lOpn.Sources.Count
                If lOpn.Sources.Item(lTargetIndex).Source.VolId = aId And _
                   lOpn.Sources.Item(lTargetIndex).Source.VolName = aName Then
                    lOpn.Sources.RemoveAt(lTargetIndex)
                Else
                    lTargetIndex += 1
                End If
            Loop
        Next
    End Sub

    'Public Sub OpenWDM(ByRef OpenOrCreate As Integer, ByRef fname As String, ByRef fun As Integer, ByRef wid As String)
    '    Dim lFile As atcData.atcDataSource
    '    Dim iret, Ind As Integer

    '    If OpenOrCreate = 2 Then 'need to create
    '        fun = F90_WDBOPN(OpenOrCreate, fname, Len(fname))
    '        iret = F90_WDFLCL(fun)
    '    End If

    '    fun = 0
    '    lFile = AddWDMFile(fname)
    '    If Not lFile Is Nothing Then
    '        pWdmCount = pWdmCount + 1
    '        Ind = WDMInd(wid)
    '        pWdmUnit(Ind) = lFile.FileUnit
    '        fun = pWdmUnit(Ind)
    '    End If
    'End Sub

    Public Sub ClearWDM()
        Dim lMsg As String = "before close in ClearWDM"
        Call F90_FILSTA(lMsg, lMsg.Length)
        For lWdmIndex As Integer = 0 To 4
            If pWdmUnit(lWdmIndex) <> 0 Then
                pWdmUnit(lWdmIndex) = 0
                pWDMObj(lWdmIndex) = Nothing
            End If
        Next lWdmIndex
        pTserFiles.Clear()

        lMsg = "after close in ClearWDM"
        Call F90_FILSTA(lMsg, lMsg.Length)
    End Sub

    Public Sub InitWDMArray()
        pWdmCount = 0
        For lWdmIndex As Integer = 0 To 4
            pWdmUnit(lWdmIndex) = 0
        Next lWdmIndex
        If pMsgUnit = 0 Then 'not yet open
            Call F90_WDIINI()
            Call F90_WDBFIN()
            'IPC.SendProcessMessage "HSPFUCI", "WDIINI"
            'IPC.SendProcessMessage "HSPFUCI", "WDBFIN"
            Dim lWdmIndex As Integer = 1
            pMsgUnit = F90_WDBOPN(lWdmIndex, pMsgWDMName, Len(pMsgWDMName))
            SendHspfMessage("WDBOPN " & pMsgWDMName & " " & lWdmIndex)
            Dim lMsg As String = WaitForChildMessage()
            'could be better
            pMsgUnit = CInt(Right(lMsg, 3))
        End If
    End Sub

    'Public Sub GetMetSegNames(ByRef fun As Integer, ByRef numMetSeg As Integer, ByRef arrayMetSegs() As String, ByRef lMetDetails() As String, ByRef lMetDescs() As String)
    '    Dim i, dsn, j As Integer
    '    Dim lLocation As String
    '    Dim tempsj, tempej As Double
    '    Dim sdat(6) As Integer
    '    Dim edat(6) As Integer
    '    Dim lts As Collection 'of atcotimser
    '    Dim lTser As atcData.atcTimeseries
    '    Dim ldate As atcData.atcTimeseries
    '    Dim sj, ej As Double
    '    Dim llocts As Collection 'of atcotimser

    '    numMetSeg = 0

    '    'look for matching WDM datasets
    '    Call findtimser("OBSERVED", "", "PREC", lts)
    '    'return the names of the data sets from this wdm file
    '    For Each lTser In lts
    '        With lTser
    '            lLocation = .Attributes.GetValue("Location")
    '            If Len(lLocation) > 0 And .File.FileUnit = fun Then
    '                'first get the common dates from prec and pevt at this location
    '                Call findtimser("OBSERVED", lLocation, "PREC", llocts)
    '                ldate = llocts.Item(1).Dates
    '                sj = ldate.Value(0) '.SJDay
    '                ej = ldate.Value(.numValues) '.EJDay
    '                Call findtimser("", lLocation, "PEVT", llocts)
    '                For j = 1 To llocts.Count
    '                    ldate = llocts.Item(j).Dates
    '                    tempsj = ldate.Value(0) '.SJDay
    '                    tempej = ldate.Value(.numValues) '.EJDay
    '                    If tempsj > sj Then sj = tempsj
    '                    If tempej < ej Then ej = tempej
    '                Next j
    '                'now save info about this met station
    '                numMetSeg = numMetSeg + 1
    '                ReDim Preserve arrayMetSegs(numMetSeg)
    '                arrayMetSegs(numMetSeg - 1) = lLocation
    '                ReDim Preserve lMetDetails(numMetSeg)
    '                dsn = .Attributes.GetValue("ID")
    '                Call J2Date(sj, sdat)
    '                Call J2Date(ej, edat)
    '                Call timcnv(edat)
    '                lMetDetails(numMetSeg - 1) = CStr(dsn) & "," & CStr(sdat(0)) & "," & CStr(sdat(1)) & "," & CStr(sdat(2)) & "," & CStr(sdat(3)) & "," & CStr(sdat(4)) & "," & CStr(sdat(5)) & "," & CStr(edat(0)) & "," & CStr(edat(1)) & "," & CStr(edat(2)) & "," & CStr(edat(3)) & "," & CStr(edat(4)) & "," & CStr(edat(5))
    '                ReDim Preserve lMetDescs(numMetSeg)
    '                lMetDescs(numMetSeg - 1) = .Attributes.GetValue("Description")
    '            End If
    '        End With
    '    Next
    'End Sub

    Private Function FindFreeDSN(ByVal aWdmId As Integer, ByVal aStartDSN As Integer) As Integer
        Dim lFreeDsn As Integer = aStartDSN + 1
        While Not GetDataSetFromDsn(aWdmId, lFreeDsn) Is Nothing
            lFreeDsn += 1
        End While
        Return lFreeDsn
    End Function

    Public Sub AddExpertDsns(ByRef aId As Integer, _
                             ByRef aLocn As String, _
                             ByRef aBaseDsn As Integer, _
                             ByRef aDsn() As Integer, _
                             ByRef aOstr() As String)
        'TODO: make aOstr and aDsn a keyed collection - maybe returned from this routine as a function
        aOstr(1) = "SIMQ    "
        aOstr(2) = "SURO    "
        aOstr(3) = "IFWO    "
        aOstr(4) = "AGWO    "
        aOstr(5) = "PETX    "
        aOstr(6) = "SAET    "
        aOstr(7) = "UZSX    "
        aOstr(8) = "LZSX    "

        Dim lWdmsFileUnit As Integer = 0
        Dim lWdmId As Integer = 0
        For lWdmIndex As Integer = 4 To 1 Step -1
            If pWdmUnit(lWdmIndex) > 0 Then 'use this as the output wdm
                lWdmsFileUnit = pWdmUnit(lWdmIndex)
                lWdmId = lWdmIndex
                Exit For
            End If
        Next lWdmIndex

        If lWdmsFileUnit > 0 Then 'okay to continue
            Dim lDsn As Integer = aBaseDsn
            Dim lScenario As String = IO.Path.GetFileNameWithoutExtension(Name)

            For lIndex As Integer = 1 To 8 'create each of the expert system dsns
                lDsn = FindFreeDSN(lWdmId, lDsn)
                Dim lGenTs As atcData.atcTimeseries = New atcData.atcTimeseries(Nothing)
                With lGenTs.Attributes
                    .SetValue("ID", lDsn)
                    .SetValue("Scenario", lScenario.ToUpper)
                    .SetValue("Constituent", aOstr(lIndex).ToUpper)
                    .SetValue("Location", aLocn.ToUpper)
                End With
                Dim lTsDate As atcData.atcTimeseries = New atcData.atcTimeseries(Nothing)
                'TODO: create dates
                'With myDateSummary
                '    .CIntvl = True
                '    .ts = 1
                '    .Tu = 4
                '    .Intvl = 1
                'End With
                'TsDate.Summary = myDateSummary
                lGenTs.Dates = lTsDate

                lGenTs.Attributes.SetValue("TSTYPE", lGenTs.Attributes.GetValue("Constituent"))
                Dim lAddedDsn As Boolean = pWDMObj(lWdmId).AddDataset(lGenTs)
                aDsn(lIndex) = lDsn
            Next lIndex
        Else 'no wdm files in this uci
            Logger.Msg("No WDM Files are available with this UCI, so no calibration locations may be added", MsgBoxStyle.OkOnly, "Add Problem")
        End If
    End Sub

    'Public Sub AddAQUATOXDsns(ByRef Id As Integer, ByRef clocn As String, ByRef basedsn As Integer, ByRef plank As Integer, ByRef gqualfg() As Integer, ByRef wdmid As Integer, ByRef Member() As String, ByRef Sub1() As Integer, ByRef Group() As String, ByRef adsn() As Integer, ByRef ostr() As String)

    '    AddAQUATOXDsnsExt(Id, clocn, basedsn, plank, gqualfg, wdmid, Member, Sub1, Group, adsn, ostr, 4)
    'End Sub

    'Public Sub AddAQUATOXDsnsExt(ByRef Id As Integer, ByRef clocn As String, ByRef basedsn As Integer, ByRef plank As Integer, ByRef gqualfg() As Integer, ByRef wdmid As Integer, ByRef Member() As String, ByRef Sub1() As Integer, ByRef Group() As String, ByRef adsn() As Integer, ByRef ostr() As String, ByRef outtu As Integer)
    '    Dim wdmsfl, ndsn, j, i As Integer
    '    Dim cscen As String
    '    Dim GenTs As atcData.atcTimeseries
    '    Dim addeddsn As Boolean
    '    Dim lts As Collection 'of atcotimser
    '    Dim TsDate As atcData.atcTimeseries
    '    Dim lOper As HspfOperation
    '    Dim ltable As HspfTable
    '    Dim wid As String
    '    Dim deleteddsn As Integer
    '    Dim vConn As Object
    '    Dim lConn As HspfConnection
    '    Dim ctmp As String
    '    Dim referenced As Boolean

    '    Member(1) = "VOL" : Sub1(1) = 1 : Group(1) = "HYDR" : ostr(1) = "VOL     " 'volume (ac.ft) AVER
    '    Member(2) = "IVOL" : Sub1(2) = 1 : Group(2) = "HYDR" : ostr(2) = "IVOL    " 'inflow (ac.ft) SUM
    '    Member(3) = "RO" : Sub1(3) = 1 : Group(3) = "HYDR" : ostr(3) = "RO      " 'discharge in cfs AVER
    '    Member(4) = "SAREA" : Sub1(4) = 1 : Group(4) = "HYDR" : ostr(4) = "SARA     " 'surface area in acres AVER
    '    Member(5) = "AVDEP" : Sub1(5) = 1 : Group(5) = "HYDR" : ostr(5) = "AVDP    " 'mean depth in feet AVER
    '    Member(6) = "PRSUPY" : Sub1(6) = 1 : Group(6) = "HYDR" : ostr(6) = "PSUP    " 'volume in from precip (ac.ft) SUM
    '    Member(7) = "VOLEV" : Sub1(7) = 1 : Group(7) = "HYDR" : ostr(7) = "VEVP    " 'volume out to evap (ac.ft) SUM
    '    Member(8) = "TW" : Sub1(8) = 1 : Group(8) = "HTRCH" : ostr(8) = "TW      " 'water temp in degrees AVER
    '    Member(9) = "NUIF1" : Sub1(9) = 1 : Group(9) = "NUTRX" : ostr(9) = "NO3     " 'inflow of no3 in lbs SUM
    '    Member(10) = "NUIF1" : Sub1(10) = 2 : Group(10) = "NUTRX" : ostr(10) = "NH3     " 'inflow of nh2 in lbs SUM
    '    Member(11) = "NUIF1" : Sub1(11) = 3 : Group(11) = "NUTRX" : ostr(11) = "NO2     " 'inflow of no2 in lbs SUM
    '    Member(12) = "NUIF1" : Sub1(12) = 4 : Group(12) = "NUTRX" : ostr(12) = "PO4     " 'inflow of po4 in lbs SUM
    '    Member(13) = "OXIF" : Sub1(13) = 1 : Group(13) = "OXRX" : ostr(13) = "DO      " 'inflow of do in lbs SUM
    '    Member(14) = "OXIF" : Sub1(14) = 2 : Group(14) = "OXRX" : ostr(14) = "BOD     " 'inflow of bod in lbs SUM
    '    Member(15) = "PKIF" : Sub1(15) = 5 : Group(15) = "PLANK" : ostr(15) = "ORC     " 'inflow of organic c in lbs SUM
    '    Member(16) = "PKIF" : Sub1(16) = 1 : Group(16) = "PLANK" : ostr(16) = "PHYT    " 'inflow of phyto in lbs SUM
    '    Member(17) = "ISED" : Sub1(17) = 1 : Group(17) = "SEDTRN" : ostr(17) = "ISD1    " 'inflow of sediment in tons SUM
    '    Member(18) = "ISED" : Sub1(18) = 2 : Group(18) = "SEDTRN" : ostr(18) = "ISD2    " 'inflow of sediment in tons SUM
    '    Member(19) = "ISED" : Sub1(19) = 3 : Group(19) = "SEDTRN" : ostr(19) = "ISD3    " 'inflow of sediment in tons SUM
    '    Member(20) = "SSED" : Sub1(20) = 1 : Group(20) = "SEDTRN" : ostr(20) = "SSD1    " 'sediment conc mg/l AVER
    '    Member(21) = "SSED" : Sub1(21) = 2 : Group(21) = "SEDTRN" : ostr(21) = "SSD2    " 'sediment conc mg/l AVER
    '    Member(22) = "SSED" : Sub1(22) = 3 : Group(22) = "SEDTRN" : ostr(22) = "SSD3    " 'sediment conc mg/l AVER
    '    Member(23) = "TIQAL" : Sub1(23) = 1 : Group(23) = "GQUAL" : ostr(23) = "TIQ1    " 'total inflow of qual SUM
    '    Member(24) = "TIQAL" : Sub1(24) = 2 : Group(24) = "GQUAL" : ostr(24) = "TIQ2    " 'total inflow of qual SUM
    '    Member(25) = "TIQAL" : Sub1(25) = 3 : Group(25) = "GQUAL" : ostr(25) = "TIQ3    " 'total inflow of qual SUM
    '    Member(26) = "NUIF2" : Sub1(26) = 4 : Group(26) = "NUTRX" : ostr(26) = "PPO4    " 'inflow of particulate po4 in lbs SUM
    '    Member(27) = "TPKIF" : Sub1(27) = 2 : Group(27) = "PLANK" : ostr(27) = "TORP    " 'inflow of total organic p in lbs SUM
    '    Member(28) = "TPKIF" : Sub1(28) = 5 : Group(28) = "PLANK" : ostr(28) = "TTP     " 'inflow of total p in lbs SUM


    '    If plank <> 1 Then
    '        ostr(15) = ""
    '        ostr(16) = ""
    '        ostr(27) = ""
    '        ostr(28) = ""
    '    End If

    '    If gqualfg(1) <> 1 Then 'if any organic chemicals
    '        ostr(23) = ""
    '    End If
    '    If gqualfg(2) <> 1 Then
    '        ostr(24) = ""
    '    End If
    '    If gqualfg(3) <> 1 Then
    '        ostr(25) = ""
    '    End If

    '    'check to see that all timsers have inputs
    '    lOper = pOpnBlks.Item("RCHRES").OperFromID(Id)
    '    If lOper.TableExists("NUT-FLAGS") Then
    '        ltable = lOper.Tables.Item("NUT-FLAGS")
    '        If ltable.Parms("NH3FG").Value = 0 Then
    '            ostr(10) = ""
    '        End If
    '        If ltable.Parms("NO2FG").Value = 0 Then
    '            ostr(11) = ""
    '        End If
    '        If ltable.Parms("PO4FG").Value = 0 Then
    '            ostr(12) = ""
    '        End If
    '    Else
    '        ostr(10) = ""
    '        ostr(11) = ""
    '        ostr(12) = ""
    '        ostr(26) = ""
    '    End If
    '    If lOper.TableExists("PLNK-FLAGS") Then
    '        ltable = lOper.Tables.Item("PLNK-FLAGS")
    '        If ltable.Parms("PHYFG").Value = 0 Then
    '            ostr(16) = ""
    '        End If
    '    Else
    '        ostr(16) = ""
    '    End If

    '    For i = 4 To 1 Step -1
    '        If pWdmUnit(i) > 0 Then
    '            'use this as the output wdm
    '            wdmsfl = pWdmUnit(i)
    '            wdmid = i
    '        End If
    '    Next i

    '    If wdmsfl > 0 Then
    '        'okay to continue
    '        ndsn = basedsn
    '        cscen = IO.Path.GetFileNameWithoutExtension(pName)

    '        For j = 1 To 28
    '            'create each of the 28 aquatox dsns

    '            If Len(ostr(j)) > 0 Then

    '                'if there is already a dsn with this scen/loc/cons,
    '                'and it is unused in this uci, delete it to avoid confusion
    '                deleteddsn = 0
    '                findtimser(UCase(Trim(cscen)), Trim(clocn), Trim(ostr(j)), lts)
    '                For Each GenTs In lts
    '                    GetWDMIDFromUnit(GenTs.File.FileUnit, wid)
    '                    If CShort(Right(wid, 1)) = wdmid Then
    '                        'this is on our output wdm
    '                        'make sure it is not referenced in this UCI already
    '                        referenced = False
    '                        For Each vConn In Me.Connections
    '                            lConn = vConn
    '                            ctmp = lConn.Target.VolName
    '                            If ctmp = "WDM" Then ctmp = "WDM1"
    '                            If ctmp = wid And lConn.Target.VolId = GenTs.Attributes.GetValue("ID") Then
    '                                'this dataset is referenced in the uci, don't delete
    '                                referenced = True
    '                            End If
    '                        Next vConn
    '                        If Not referenced Then
    '                            'delete it to avoid confusion
    '                            deleteddsn = GenTs.Attributes.GetValue("ID")
    '                            ClearWDMDataSet(wid, deleteddsn)
    '                            DeleteWDMDataSet(wid, deleteddsn)
    '                        End If
    '                    End If
    '                Next

    '                If deleteddsn > 0 Then
    '                    ndsn = deleteddsn
    '                Else
    '                    ndsn = FindFreeDSN(wdmid, ndsn)
    '                End If

    '                GenTs = New atcData.atcTimeseries(Nothing)
    '                With GenTs.Attributes
    '                    .SetValue("ID", ndsn)
    '                    .SetValue("Scenario", UCase(cscen))
    '                    .SetValue("Constituent", UCase(ostr(j)))
    '                    .SetValue("Location", UCase(clocn))
    '                    .SetValue("Description", "AQUATOX Linkage Timeseries for " & ostr(j))
    '                End With

    '                TsDate = New atcData.atcTimeseries(Nothing)
    '                'TODO: Create dates
    '                'With myDateSummary
    '                '    .CIntvl = True
    '                '    .ts = 1
    '                '    If j = 1 Then
    '                '        .Tu = 3 'output vol as hourly
    '                '    Else
    '                '        '.Tu = 4 'the rest as daily by default, or hourly if requested
    '                '        .Tu = outtu
    '                '    End If
    '                '    .Intvl = 1
    '                'End With
    '                'TsDate.Summary = myDateSummary
    '                GenTs.Dates = TsDate

    '                GenTs.Attributes.SetValue("TSTYPE", GenTs.Attributes.GetValue("Constituent"))
    '                addeddsn = pWDMObj(wdmid).AddDataSet(GenTs, 0)
    '                adsn(j) = ndsn
    '            End If
    '        Next j
    '    Else
    '        'no wdm files in this uci
    '        Call MsgBox("No WDM Files are available with this UCI, so no AQUATOX locations may be added", MsgBoxStyle.OkOnly, "Add Problem")
    '    End If
    'End Sub

    Public Sub AddExpertExtTargets(ByRef reachid As Integer, _
                                   ByRef copyid As Integer, _
                                   ByRef ContribArea As Single, _
                                   ByRef adsn() As Integer, _
                                   ByRef ostr() As String)
        Dim i As Integer
        Dim MFact As Single
        Dim Tran, gap As String

        MFact = 12.0# / ContribArea
        'mfact = Format(mfact, "0.#######")
        AddExtTarget("RCHRES", reachid, "ROFLOW", "ROVOL", 1, 1, MFact, "    ", "WDM", adsn(1), ostr(1), 1, "ENGL", "AGGR", "REPL")

        If copyid > 0 Then
            MFact = 1.0# / ContribArea
            'mfact = Format(mfact, "0.#######")
            For i = 2 To 8
                If i < 7 Then
                    Tran = "    "
                Else
                    Tran = "AVER"
                End If
                'If i < 5 Then
                '  gap = "    "
                'Else
                gap = "AGGR"
                'End If

                AddExtTarget("COPY", copyid, "OUTPUT", "MEAN", i - 1, 1, MFact, Tran, "WDM", adsn(i), ostr(i), 1, "ENGL", gap, "REPL")
            Next i
        End If

    End Sub

    Public Sub AddAQUATOXExtTargets(ByRef reachid As Integer, _
                                    ByRef wdmid As Integer, _
                                    ByRef Member() As String, _
                                    ByRef Sub1() As Integer, _
                                    ByRef Group() As String, _
                                    ByRef adsn() As Integer, _
                                    ByRef ostr() As String)
        AddAQUATOXExtTargetsExt(reachid, wdmid, Member, Sub1, Group, adsn, ostr, 4)
    End Sub

    Public Sub AddAQUATOXExtTargetsExt(ByRef reachid As Integer, _
                                       ByRef wdmid As Integer, _
                                       ByRef Member() As String, _
                                       ByRef Sub1() As Integer, _
                                       ByRef Group() As String, _
                                       ByRef adsn() As Integer, _
                                       ByRef ostr() As String, _
                                       ByRef outtu As Integer)
        Dim i, Sub2 As Integer
        Dim MFact As Single
        Dim Tran, gap As String

        For i = 1 To 28
            If Len(ostr(i)) > 0 Then
                If i = 1 Or i = 3 Or i = 4 Or i = 5 Or i = 8 Or i = 20 Or i = 21 Or i = 22 Then
                    Tran = "AVER"
                Else
                    If Me.OpnSeqBlock.Delt = 1440 And outtu = 4 Then
                        'daily run and daily output requested
                        Tran = ""
                    ElseIf Me.OpnSeqBlock.Delt = 60 And outtu = 3 Then
                        'hourly run and hourly output requested
                        Tran = ""
                    Else
                        Tran = "SUM"
                    End If
                End If
                gap = "AGGR"
                MFact = 1.0#
                Sub2 = 1
                If i = 26 Then Sub2 = 2
                AddExtTarget("RCHRES", reachid, Group(i), Member(i), Sub1(i), Sub2, MFact, Tran, "WDM" & CStr(wdmid), adsn(i), ostr(i), 1, "METR", gap, "REPL")
            End If
        Next i

    End Sub

    Public Sub AddExpertSchematic(ByRef aReachId As Integer, _
                                  ByRef aCopyId As Integer)
        'add schematic block records for expert system copy data sets
        Dim lConsName As New Hashtable
        lConsName.Add("P:SURO", "1")
        lConsName.Add("P:IFWO", "2")
        lConsName.Add("P:AGWO", "3")
        lConsName.Add("P:PET", "4")
        lConsName.Add("P:TAET", "5")
        lConsName.Add("P:UZS", "6")
        lConsName.Add("P:LZS", "7")
        lConsName.Add("I:SURO", "1")
        lConsName.Add("I:PET", "4")
        lConsName.Add("I:IMPEV", "5")

        'determine mass link numbers
        Dim lPerlndMassLinkNumber As Integer = 0
        Dim lImplndMassLinkNumber As Integer = 0
        For Each lConnection As HspfConnection In pConnections
            If lConnection.Source.VolName = "PERLND" And _
               lConnection.Target.VolName = "COPY" Then
                lPerlndMassLinkNumber = lConnection.MassLink
            ElseIf lConnection.Source.VolName = "IMPLND" And _
                   lConnection.Target.VolName = "COPY" Then
                lImplndMassLinkNumber = lConnection.MassLink
            End If
        Next lConnection
        If lPerlndMassLinkNumber = 0 Then 'need to add perlnd masslink
            lPerlndMassLinkNumber = 90
            Dim lFound As Boolean = True
            Do Until lFound = False
                lFound = False
                For Each lMassLink As HspfMassLink In pMassLinks
                    If lMassLink.MassLinkId = lPerlndMassLinkNumber Then
                        lPerlndMassLinkNumber += 1
                        lFound = True
                        Exit For
                    End If
                Next lMassLink
            Loop
            'now add perlnd masslink
            For Each lTimserType As String In lConsName.Keys
                If lTimserType.StartsWith("P") Then
                    Dim lMassLink As New HspfMassLink
                    lMassLink.Uci = Me
                    lMassLink.MassLinkId = lPerlndMassLinkNumber
                    lMassLink.Source.VolName = "PERLND"
                    lMassLink.Source.VolId = 0
                    lMassLink.Source.Group = "PWATER"
                    lMassLink.Source.Member = lTimserType.Substring(2)
                    lMassLink.MFact = 1.0#
                    lMassLink.Tran = ""
                    lMassLink.Target.VolName = "COPY"
                    lMassLink.Target.VolId = 0
                    lMassLink.Target.Group = "INPUT"
                    lMassLink.Target.Member = "MEAN"
                    lMassLink.Target.MemSub1 = lConsName.Item(lTimserType)
                    pMassLinks.Add(lMassLink)
                End If
            Next lTimserType
        End If

        If lImplndMassLinkNumber = 0 Then
            'need to add implnd masslink
            lImplndMassLinkNumber = 91
            Dim lFound As Boolean = True
            Do Until lFound = False
                lFound = False
                For Each lMassLink As HspfMassLink In pMassLinks
                    If lMassLink.MassLinkId = lImplndMassLinkNumber Then
                        lImplndMassLinkNumber += 1
                        lFound = True
                        Exit For
                    End If
                Next lMassLink
            Loop
            'now add implnd masslink
            Dim lCopyIndex As Integer = 1
            For Each lTimserType As String In lConsName.Keys
                If lTimserType.StartsWith("I") Then
                    Dim lMassLink As New HspfMassLink
                    lMassLink.Uci = Me
                    lMassLink.MassLinkId = lImplndMassLinkNumber
                    lMassLink.Source.VolName = "IMPLND"
                    lMassLink.Source.VolId = 0
                    lMassLink.Source.Group = "IWATER"
                    lMassLink.Source.Member = lTimserType.Substring(2)
                    lMassLink.MFact = 1.0#
                    lMassLink.Tran = ""
                    lMassLink.Target.VolName = "COPY"
                    lMassLink.Target.VolId = 0
                    lMassLink.Target.Group = "INPUT"
                    lMassLink.Target.Member = "MEAN"
                    lMassLink.Target.MemSub1 = lConsName.Item(lTimserType)
                    pMassLinks.Add(lMassLink)
                End If
            Next lTimserType
        End If

        'add schematic records
        Dim lOperation As HspfOperation = pOpnBlks.Item("RCHRES").OperFromID(aReachId)
        AddCopyToSchematic(lOperation, aCopyId, lPerlndMassLinkNumber, lImplndMassLinkNumber)
        Dim lOperations As Collection(Of HspfOperation) = FindUpstreamOpns(lOperation)
        Do While lOperations.Count > 0
            lOperation = lOperations.Item(0)
            lOperations.RemoveAt(0)
            AddCopyToSchematic(lOperation, aCopyId, lPerlndMassLinkNumber, lImplndMassLinkNumber)
            lOperations = FindUpstreamOpns(lOperation)
        Loop
    End Sub

    Public Sub AddExtTarget(ByRef sname As String, _
                            ByRef sid As Integer, _
                            ByRef sgroup As String, _
                            ByRef Smember As String, _
                            ByRef Smem1 As Integer, _
                            ByRef Smem2 As Integer, _
                            ByRef MFact As Single, _
                            ByRef Tran As String, _
                            ByRef tname As String, _
                            ByRef Tid As Integer, _
                            ByRef tmember As String, _
                            ByRef Tsub1 As Integer, _
                            ByRef aSystem As String, _
                            ByRef gap As String, _
                            ByRef amd As String)

        Dim lOperation As HspfOperation
        Dim lConnection As HspfConnection

        lOperation = pOpnBlks.Item(sname).OperFromID(sid)
        lConnection = New HspfConnection
        With (lConnection)
            .Uci = Me
            .Typ = 4
            .Source.VolName = lOperation.Name
            .Source.VolId = lOperation.Id
            .Source.Group = sgroup
            .Source.Member = Smember
            .Source.MemSub1 = Smem1
            .Source.MemSub2 = Smem2
            .Source.Opn = lOperation
            .MFact = MFact
            .Tran = Tran
            .Target.VolName = tname
            .Target.VolId = Tid
            .Target.Member = tmember
            .Target.MemSub1 = Tsub1
            .Ssystem = aSystem
            .Sgapstrg = gap
            .Amdstrg = amd
        End With
        pConnections.Add(lConnection)
        lOperation.Targets.Add(lConnection)
    End Sub

    Public Sub AddOutputWDMDataSet(ByRef aLocation As String, ByRef aConstituent As String, _
                                   ByRef aBaseDsn As Integer, ByRef aWdmId As Integer, _
                                   ByRef aDsn As Integer)
        Dim lWdmId As Integer = 0
        AddOutputWDMDataSetExt(aLocation, aConstituent, aBaseDsn, lWdmId, 4, "", aDsn)
        aWdmId = lWdmId
    End Sub

    Public Sub AddOutputWDMDataSetExt(ByRef aLocation As String, ByRef aConstituent As String, _
                                      ByRef aBaseDsn As Integer, ByRef aWdmId As Integer, _
                                      ByRef aTUnit As Integer, ByRef aDescription As String, _
                                      ByRef aDsn As Integer)
        If aWdmId = 0 Then
            For lWdmIndex As Integer = 1 To 4
                If Not pWDMObj(lWdmIndex) Is Nothing Then 'use this as the output wdm
                    aWdmId = lWdmIndex
                    Exit For
                End If
            Next lWdmIndex
        End If

        If aWdmId > 0 Then 'okay to continue
            Dim lScenario As String = IO.Path.GetFileNameWithoutExtension(Name)
            Dim lDsn As Integer = FindFreeDSN(aWdmId, aBaseDsn)
            Dim lGenericTs As New atcData.atcTimeseries(Nothing)
            With lGenericTs.Attributes
                .SetValue("ID", lDsn)
                .SetValue("Scenario", lScenario.ToUpper)
                .SetValue("Constituent", aConstituent.ToUpper)
                .SetValue("Location", aLocation.ToUpper)
                .SetValue("Description", aDescription)
                .SetValue("TU", aTUnit)
                .SetValue("TS", 1)
                .SetValue("TSTYPE", aConstituent.ToUpper)
            End With
            Dim lTsDate As atcData.atcTimeseries = New atcData.atcTimeseries(Nothing)
            lGenericTs.Dates = lTsDate

            Dim lAddedDsn As Boolean = pWDMObj(aWdmId).AddDataset(lGenericTs, 0)
            aDsn = lDsn
        End If
    End Sub

    Public Sub ClearWDMDataSet(ByRef aWdmId As String, ByRef aDsn As Integer)
        Dim lWdmsfl, lId As Integer

        If aWdmId.Length < 4 Then
            lId = 1
        Else
            lId = CShort(aWdmId.Substring(4, 1))
        End If
        lWdmsfl = pWdmUnit(lId)
        Dim NewGenTs As New atcData.atcTimeseries(Nothing)
        If lWdmsfl > 0 Then
            Dim GenTs As atcData.atcTimeseries = GetDataSetFromDsn(lId, aDsn)
            'save attributes
            NewGenTs.Attributes.ChangeTo(GenTs.Attributes)
            Dim TsDate As atcData.atcTimeseries = New atcData.atcTimeseries(Nothing)
            'TODO: copy dates
            'With myDateSummary
            '    .CIntvl = GenTs.Dates.Summary.CIntvl
            '    .ts = GenTs.Dates.Summary.ts
            '    .Tu = GenTs.Dates.Summary.Tu
            '    .Intvl = GenTs.Dates.Summary.Intvl
            '    .SJDay = GenTs.Dates.Summary.SJDay
            '    .EJDay = GenTs.Dates.Summary.EJDay
            'End With
            'TsDate.Summary = myDateSummary
            NewGenTs.Dates = TsDate

            'delete dsn
            pWDMObj(lId).DataSets.Remove(GenTs)
            'add dsn
            Dim lAddDsn As Boolean = pWDMObj(lId).AddDataset(NewGenTs, 0)
        End If
    End Sub

    Public Sub DeleteWDMDataSet(ByRef aWdmId As String, ByRef aDsn As Integer)
        Dim lId As Integer
        If aWdmId.Length < 4 Then
            lId = 1
        Else
            lId = CShort(aWdmId.Substring(4, 1))
        End If

        If pWdmUnit(lId) > 0 Then
            Dim GenTs As atcData.atcTimeseries = GetDataSetFromDsn(lId, aDsn)
            GenTs.Dates.EnsureValuesRead()
            pWDMObj(lId).DataSets.Remove(GenTs)
        End If
    End Sub

    Public Sub ClearAllOutputDsns()
        For Each lConnection As HspfConnection In pConnections
            If lConnection.Typ = 4 Then
                If lConnection.Target.VolName.Substring(1, 3) = "WDM" Then 'clear this dsn
                    ClearWDMDataSet(lConnection.Target.VolName, lConnection.Target.VolId)
                End If
            End If
        Next lConnection
    End Sub

    Public Function AddWDMFile(ByRef aName As String) As atcWDM.atcDataSourceWDM
        Dim lFileAttribute As Integer = GetAttr(aName)
        If (lFileAttribute And FileAttribute.ReadOnly) <> 0 Then
            Try
                SetAttr(aName, lFileAttribute - FileAttribute.ReadOnly)
            Catch e As Exception
                Logger.Msg("The WDM file " & aName & " is Read Only and cannot be opened in that state.", vbExclamation, "File Open Problem")
                Return Nothing
            End Try
        End If

        Dim lWDMFile As New atcWDM.atcDataSourceWDM
        Dim lFound As Boolean = False
        If atcDataManager.DataSources IsNot Nothing Then
            For Each lBASINSDataSource As atcTimeseriesSource In atcDataManager.DataSources
                If lBASINSDataSource.Specification.ToUpper = IO.Path.GetFullPath(aName).ToUpper Then
                    'found it in the BASINS data sources
                    lWDMFile = lBASINSDataSource
                    lFound = True
                    Exit For
                End If
            Next
        End If

        If Not lFound Then
            If Not lWDMFile.Open(aName) Then 'had a problem
                Logger.Msg("Could not open WDM file" & vbCr & aName, MsgBoxStyle.Exclamation, "AddWDMFile Failed")
                Return Nothing
            Else
                pTserFiles.AddRange(lWDMFile.DataSets)
            End If
        End If
        Return lWDMFile

    End Function

    Public Function PreScanFilesBlock(ByRef aEchoFile As String) As Boolean
        Dim lFilesOK As Boolean = True
        Try
            Dim lString As String = Nothing
            Dim lReturnKey As Integer = -1
            Dim lReturnCode As Integer
            Dim lRecordType As Integer
            pWdmCount = 0
            aEchoFile = ""
            Do
                GetNextRecordFromBlock("FILES", lReturnKey, lString, lRecordType, lReturnCode)
                If lReturnCode <> 10 AndAlso lRecordType = 0 Then
                    Dim lFileName As String = lString.Substring(16).Trim
                    Dim lFilePath As String
                    If lString.StartsWith("WDM") Then
                        Dim lFile As atcData.atcTimeseriesSource = AddWDMFile(lFileName)
                        If Not lFile Is Nothing Then
                            pWdmCount += 1
                            Dim lInd As Integer = WDMInd(Left(lString, 4))
                            'TODO: ? pWdmUnit(Ind) = lFile.FileUnit
                            pWDMObj(lInd) = lFile
                        End If
                    ElseIf lString.Length > 16 Then 'make sure the other files are ok
                        lFilePath = IO.Path.GetDirectoryName(lFileName)
                        If lFilePath.Length > 0 AndAlso Not IO.Directory.Exists(lFilePath) Then
                            Logger.Msg("Error in Files Block:  Folder " & lFilePath & " does not exist.", MsgBoxStyle.OkOnly, "Open UCI Problem")
                            lFilesOK = False
                        ElseIf UCase(Right(lFileName, 4)) = ".MUT" Then  'does this file exist
                            If Not IO.File.Exists(lFileName) Then
                                Logger.Msg("Error in Files Block:  Input File " & lFileName & " does not exist.", MsgBoxStyle.OkOnly, "Open UCI Problem")
                                lFilesOK = False
                            End If
                        End If
                        If lString.StartsWith("MESSU") Then 'save echo file name
                            aEchoFile = lFileName.Trim
                        End If
                    End If
                End If
            Loop While lReturnCode = 2
            's = "PreScanFilesBlock exit"
            'F90_FILSTA s, Len(s)
            System.Windows.Forms.Application.DoEvents()
        Catch ex As Exception
            'TODO: myMsgBox.Show("Cannot open '" & Mid(s, 17, Len(s) - 16) & "' in PreScanFilesBlock." & vbCrLf & vbCrLf & "Error: " & Err.Description, "HSPF Files Error", "+-&OK")
            lFilesOK = False
        End Try
        Return lFilesOK
    End Function

    Public Sub SetWDMFiles()
        Dim Ind, i, iret As Integer
        Dim tname, s, w, tpath As String
        Dim lFile As atcData.atcTimeseriesSource
        Dim lHFile As HspfFile
        Dim FilesOK As Boolean
        Dim ifound As Boolean
        Dim j As Integer
        Dim M As String
        'used after editing files block to open wdm files
        On Error GoTo x

        FilesOK = True

        M = "at start of SetWDMFiles"
        Call F90_FILSTA(M, Len(M))

        pWdmCount = 0
        For i = 1 To pFilesBlk.Count
            lHFile = pFilesBlk.Value(i)
            If Len(lHFile.Typ) > 2 Then
                If lHFile.Typ.StartsWith("WDM") Then
                    'see if this wdm is already in project
                    ifound = False
                    If ifound = False And pWdmCount < 4 Then 'add it to project
                        M = "just before AddWDMFile"
                        Call F90_FILSTA(M, Len(M))
                        lFile = AddWDMFile(lHFile.Name)
                        M = "at end of AddWDMFile"
                        Call F90_FILSTA(M, Len(M))
                        If Not lFile Is Nothing Then
                            s = lHFile.Typ
                            Ind = WDMInd(Left(s, 4))
                            'TODO: ? pWdmUnit(Ind) = lFile.FileUnit
                            pWDMObj(Ind) = lFile
                            pWdmCount += 1
                        Else
                            Logger.Msg("Error in SetWDMFiles")
                        End If
                    End If
                End If
            End If
        Next i
        Exit Sub
x:
        Logger.Msg("Error " & Err.Description & " in SetWDMFiles")
        FilesOK = False
    End Sub

    'TODO: use new code for WDM
    Public Function GetWDMAttr(ByRef aWdmId As String, ByRef idsn As Integer, ByRef attr As String) As String
        Dim s As String
        Dim lDsn As atcData.atcTimeseries

        lDsn = GetDataSetFromDsn(WDMInd(aWdmId), idsn)
        If Not (lDsn Is Nothing) And attr = "LOC" Then
            s = lDsn.Attributes.GetValue("Location")
        ElseIf Not (lDsn Is Nothing) And attr = "CON" Then
            s = lDsn.Attributes.GetValue("Constituent")
        ElseIf Not (lDsn Is Nothing) And attr = "DESC" Then
            s = lDsn.Attributes.GetValue("Description")
        Else
            s = ""
        End If
        Return s
    End Function

    'TODO: can we get the right dataset by ID from the DataSets collection? Can if it is keyed by ID.
    Public Function GetDataSetFromDsn(ByRef lWdmInd As Integer, ByRef lDsn As Integer) As atcData.atcTimeseries
        For Each lDataSet As atcData.atcTimeseries In pWDMObj(lWdmInd).DataSets
            If lDsn = lDataSet.Attributes.GetValue("ID") Then
                Return lDataSet
            End If
        Next
        'MsgBox "DSN " & lDsn & " does not exist.", vbOKOnly
        Return Nothing
    End Function

    Public Function GetWDMObj(ByVal Index As Integer) As atcData.atcTimeseriesSource
        Return pWDMObj(Index)
    End Function

    Public Function GetWDMIdFromName(ByVal Name As String) As String
        GetWDMIdFromName = "WDM"
        For i As Integer = 1 To 4
            If Not pWDMObj(i) Is Nothing Then
                If pWDMObj(i).Specification.ToLower = Name.ToLower Then
                    GetWDMIdFromName = "WDM" & i
                End If
            End If
        Next
    End Function

    Public Function FindTimser(ByRef aScenario As String, _
                               ByRef aLocation As String, _
                               ByRef aConstituent As String) As Collection
        Dim lFindTimser As New Collection
        For Each lTser As atcData.atcTimeseries In pTserFiles
            With lTser.Attributes
                If (aScenario = .GetValue("Scenario") _
                  Or aScenario.Trim.Length = 0) And (aLocation = .GetValue("Location") _
                  Or aLocation.Trim.Length = 0) And (aConstituent = .GetValue("Constituent") _
                  Or aConstituent.Trim.Length = 0) Then 'need this timser
                    lFindTimser.Add(lTser)
                End If
            End With
        Next
        Return lFindTimser
    End Function

    Public Sub EditActivityAll()
        editActivityAllInit(Me, (Me.icon))
    End Sub

    Public Function WeightedSourceArea(ByVal aOperation As HspfOperation, _
                                       ByVal aSourceType As String, _
                                       ByRef aSourceCollection As atcCollection) As Double
        If aSourceCollection Is Nothing Then
            aSourceCollection = New atcCollection
        End If
        Dim lArea As Double = LocalWeightedSource(aSourceType, aOperation, aSourceCollection)
        Logger.Dbg("Weight" & aOperation.Name & " " & aOperation.Id & " " & lArea)
        For Each lOperationUp As HspfOperation In FindUpstreamOpns(aOperation)
            lArea += WeightedSourceArea(lOperationUp, aSourceType, aSourceCollection)
        Next
        Return lArea
    End Function

    Private Function LocalWeightedSource(ByVal aSourceType As String, _
                                         ByVal aOperation As HspfOperation, _
                                         ByVal aSourceCollection As atcCollection) As Double
        Dim lAreaTotal As Double = 0.0
        For Each lConnection As HspfConnection In aOperation.Sources
            If lConnection.Source.VolName = "PERLND" Or _
               lConnection.Source.VolName = "IMPLND" Then
                Dim lArea As Double = lConnection.MFact
                For Each lMetSegRec As atcUCI.HspfMetSegRecord In lConnection.Source.Opn.MetSeg.MetSegRecs
                    If lMetSegRec.Name = aSourceType Then
                        With lMetSegRec
                            lArea *= .MFactP
                            lAreaTotal += lArea
                            Dim lKey As Integer = .Source.VolId
                            aSourceCollection.Increment(lKey, lArea)
                            Logger.Dbg("Key " & lKey & " " & lConnection.Target.VolName & lConnection.Target.VolId & " Area " & lArea)
                        End With
                    End If
                Next
            End If
        Next lConnection
        Return lAreaTotal
    End Function

    Public Function UpstreamArea(ByRef aOperation As HspfOperation) As Double
        Dim lTotalArea As Double = LocalUpstreamArea(aOperation)
        For Each lOperationUp As HspfOperation In FindUpstreamOpns(aOperation)
            lTotalArea += UpstreamArea(lOperationUp)
        Next
        Return lTotalArea
    End Function

    Private Function LocalUpstreamArea(ByRef aOperation As HspfOperation) As Double
        Dim lUpArea As Double = 0.0
        For Each lConnection As HspfConnection In aOperation.Sources
            If lConnection.Source.VolName = "PERLND" Or _
               lConnection.Source.VolName = "IMPLND" Then
                lUpArea += lConnection.MFact
            End If
        Next lConnection
        Return lUpArea
    End Function

    Private Function FindUpstreamOpns(ByRef aOperation As HspfOperation) As Collection(Of HspfOperation)
        Dim lOperations As New Collection(Of HspfOperation)
        For Each lConnection As HspfConnection In aOperation.Sources
            If lConnection.Source.VolName = "RCHRES" Or _
               lConnection.Source.VolName = "BMPRAC" Then
                'add the source operation to the collection
                lOperations.Add(lConnection.Source.Opn)
            End If
        Next lConnection
        Return lOperations
    End Function

    Private Sub AddCopyToSchematic(ByRef aOpn As HspfOperation, _
                                   ByRef aCopyId As Integer, _
                                   ByRef aPerlndMasslink As Integer, _
                                   ByRef aImplndMasslink As Integer)
        'adds the copy record to the schematic block for each local land segment
        'contributing to this operation
        For lSourceIndex As Integer = 1 To aOpn.Sources.Count
            Dim lSourceConnection As HspfConnection = aOpn.Sources.Item(lSourceIndex)
            If lSourceConnection.Source.VolName = "PERLND" Or _
               lSourceConnection.Source.VolName = "IMPLND" Then 'copy this record
                'does this oper to copy already exist?
                Dim lCopyOpn As HspfOperation = pOpnBlks.Item("COPY").OperFromID(aCopyId)
                Dim lCopyOpnMatchIndex As Integer = 0
                Dim jConn As HspfConnection
                For lCopyOpnSourceIndex As Integer = 1 To lCopyOpn.Sources.Count
                    jConn = lCopyOpn.Sources.Item(lCopyOpnSourceIndex)
                    If jConn.Source.VolName = lSourceConnection.Source.VolName And _
                       jConn.Source.VolId = lSourceConnection.Source.VolId Then
                        lCopyOpnMatchIndex = lCopyOpnSourceIndex
                    End If
                Next lCopyOpnSourceIndex
                If lCopyOpnMatchIndex > 0 Then
                    jConn = lCopyOpn.Sources.Item(lCopyOpnMatchIndex)
                    jConn.MFact = jConn.MFact + lSourceConnection.MFact
                Else 'does not already exist
                    Dim lConn As New HspfConnection
                    lConn.Source.VolName = lSourceConnection.Source.VolName
                    lConn.Source.VolId = lSourceConnection.Source.VolId
                    lConn.Typ = lSourceConnection.Typ
                    lConn.MFact = lSourceConnection.MFact
                    lConn.Target.VolName = "COPY"
                    lConn.Target.VolId = aCopyId
                    If lConn.Source.VolName = "PERLND" Then
                        lConn.MassLink = aPerlndMasslink
                    Else
                        lConn.MassLink = aImplndMasslink
                    End If
                    pConnections.Add(lConn)
                    lSourceConnection.Source.Opn.Targets.Add(lConn)
                    lCopyOpn = pOpnBlks.Item("COPY").OperFromID(aCopyId)
                    lCopyOpn.Sources.Add(lConn)
                End If
            End If
        Next lSourceIndex
    End Sub

    Public Function OperationExists(ByVal aName As String, ByVal aId As Integer) As Boolean
        Dim lExists As Boolean = False
        Dim lOpnBlk As HspfOpnBlk = pOpnBlks.Item(aName)
        If lOpnBlk.Count > 0 Then
            For Each lOperation As HspfOperation In lOpnBlk.Ids
                If lOperation.Id = aId Then 'in use
                    lExists = True
                    Exit For
                End If
            Next lOperation
        End If
        Return lExists
    End Function

    Public Function AddOperation(ByRef aName As String, _
                                 ByRef aId As Integer) As HspfOperation
        'add an operation/oper id (ie copy 100) to the uci object
        Dim lOpnBlk As HspfOpnBlk = pOpnBlks.Item(aName)
        While OperationExists(aName, aId) 'get next free Id
            aId += 1
        End While

        Dim lOpn As New HspfOperation
        lOpn.Name = aName
        lOpn.Id = aId
        lOpn.Uci = Me

        lOpnBlk.Ids.Add(lOpn)
        lOpn.OpnBlk = lOpnBlk
        Return lOpn
    End Function

    Public Sub AddOperationToOpnSeqBlock(ByVal aOperationName As String, ByVal aOperationId As Integer, ByVal aPosition As Integer)

        'add to opn seq block
        If aPosition < Me.OpnSeqBlock.Opns.Count Then
            Me.OpnSeqBlock.AddBefore(Me.OpnBlks(aOperationName).OperFromID(aOperationId), aPosition)
        Else
            Me.OpnSeqBlock.Add(Me.OpnBlks(aOperationName).OperFromID(aOperationId))
        End If
        Me.OpnBlks(aOperationName).OperFromID(aOperationId).Uci = Me

        If Me.OpnBlks(aOperationName).Count > 1 Then
            'already have some of this operation
            For Each lTable As HspfTable In Me.OpnBlks(aOperationName).Ids(1).Tables
                'add this opn id to this table
                Me.AddTable(aOperationName, aOperationId, lTable.Name)
            Next lTable
        Else
            Dim lOpnBlk As HspfOpnBlk = Me.OpnBlks(aOperationName)
            Me.OpnBlks(aOperationName).OperFromID(aOperationId).OpnBlk = lOpnBlk
        End If

        'add dummy ftable if rchres
        If aOperationName = "RCHRES" Then
            Dim lOpn As HspfOperation
            lOpn = Me.OpnBlks("RCHRES").OperFromID(aOperationId)
            lOpn.FTable = New HspfFtable
            lOpn.FTable.Operation = lOpn
            lOpn.FTable.Id = aOperationId
        End If
    End Sub

    Public Sub AddTable(ByRef aOperationName As String, _
                        ByRef aOperationId As Integer, _
                        ByRef aTableName As String)
        'create a new table, or add this operation id to the current table
        Dim lOpnBlk As HspfOpnBlk = pOpnBlks.Item(aOperationName)
        If lOpnBlk.Count > 0 Then 'this operation block exists, okay to add table
            lOpnBlk.AddTable(aOperationId, aTableName, Msg.BlockDefs.Item(aOperationName))
        End If
    End Sub

    Public Sub RemoveTable(ByRef aOperationName As String, _
                           ByRef aOperationId As Integer, _
                           ByRef aTableName As String)
        'remove this operation id from the current table
        'remove whole table if this is the only operation in the table
        Dim lOpnBlk As HspfOpnBlk = pOpnBlks.Item(aOperationName)
        If lOpnBlk.Count > 0 Then 'operation block exists, okay to remove table
            lOpnBlk.RemoveTable(aOperationId, aTableName)
        End If
    End Sub

    Private Sub NewOutputDsns(ByVal aOldScenario As String, _
                              ByVal aNewScenario As String, _
                              ByVal aBaseDsn As Integer, _
                              ByVal aRelAbs As Integer)
        'build new output dsns on SaveAs

        'look for output wdm
        Dim lWdmId As Integer = 0
        For lWdmIndex As Integer = 4 To 1 Step -1
            If Not pWDMObj(lWdmIndex) Is Nothing Then 'use this as the output wdm
                lWdmId = lWdmIndex
            End If
        Next lWdmIndex

        If lWdmId > 0 Then 'okay to continue, look for matching WDM datasets
            Dim lts As Collection = FindTimser(aOldScenario.ToUpper, "", "")
            'return the names of the data sets from this wdm file
            Dim lDsn As Integer = 0
            For lIndex As Integer = 1 To lts.Count
                Dim lTimser As atcData.atcTimeseries = lts.Item(lIndex)
                'find a free dsn
                If aRelAbs = 1 Then
                    lDsn = CInt(lTimser.Attributes.GetValue("id")) + aBaseDsn - 1
                ElseIf lDsn = 0 Then
                    lDsn = aBaseDsn - 1
                End If
                lDsn = FindFreeDSN(lWdmId, lDsn)

                Dim lGenTs As New atcData.atcTimeseries(Nothing)
                'set attribs to the old version
                With lGenTs.Attributes
                    .SetValue("ID", lDsn)
                    .SetValue("Scenario", aNewScenario)
                    .SetValue("Constituent", lTimser.Attributes.GetValue("Constituent"))
                    .SetValue("Location", lTimser.Attributes.GetValue("Location"))
                    .SetValue("Description", lTimser.Attributes.GetValue("Description"))
                End With
                Dim TsDate As New atcData.atcTimeseries(Nothing)
                'TODO: Create dates
                'With myDateSummary
                '    .CIntvl = lTimser.Dates.Summary.CIntvl
                '    .ts = lTimser.Dates.Summary.ts
                '    .Tu = lTimser.Dates.Summary.Tu
                '    .Intvl = lTimser.Dates.Summary.Intvl
                'End With
                'TsDate.Summary = myDateSummary
                lGenTs.Dates = TsDate

                'now add the timser
                With lTimser.Attributes
                    Dim lAddedDsn As Boolean = AddWDMDataSet(lWdmId, lDsn, aNewScenario, _
                                                             .GetValue("Location"), _
                                                             .GetValue("Constituent"), _
                                                             lTimser.Attributes.GetValue("tu"), _
                                                             lTimser.Attributes.GetValue("ts"), _
                                                             .GetValue("Description"))
                End With
                'update tstype attribute
                lGenTs = Me.GetDataSetFromDsn(lWdmId, lDsn)
                If Not lGenTs Is Nothing Then
                    Dim lTsType As String = lTimser.Attributes.GetValue("TSTYPE")
                    lGenTs.Attributes.SetValue("TSTYPE", lTsType)
                    Dim Update As Boolean = pWDMObj(lWdmId).AddDataset(lGenTs, atcData.atcTimeseriesSource.EnumExistAction.ExistReplace)
                End If

                'change the appropriate ext targets record
                Dim cwdm As String = "WDM" & CStr(lWdmId)
                For Each lConnection As HspfConnection In pConnections
                    If lConnection.Typ = 4 Then
                        If (Trim(lConnection.Target.VolName) = cwdm Or (Trim(lConnection.Target.VolName) = "WDM" And lWdmId = 1)) And lConnection.Target.VolId = lTimser.Attributes.GetValue("id") Then
                            'found the old dsn in the ext targets, change it
                            lConnection.Target.VolId = lDsn
                        End If
                    End If
                Next lConnection
            Next lIndex
            'Me.GetWDMObj(wdmid).Refresh    'Not necessary
        End If
    End Sub

    Public Function AddWDMDataSet(ByVal aWdmId As Integer, _
                                  ByVal aDsn As Integer, _
                                  ByVal aScenario As String, _
                                  ByVal aLocation As String, _
                                  ByVal aConstituent As String, _
                                  ByVal aTimeUnits As Integer, _
                                  ByVal aTimeStep As Integer, _
                         Optional ByVal aDesc As String = "") As Boolean
        Dim lGenTs As New atcData.atcTimeseries(Nothing)
        With lGenTs.Attributes
            .SetValue("ID", aDsn)
            .SetValue("Scenario", aScenario.ToUpper)
            .SetValue("Constituent", aConstituent.ToUpper)
            .SetValue("Location", aLocation.ToUpper)
            .SetValue("ts", aTimeStep)
            .SetValue("tu", aTimeUnits)
            If aDesc.Length > 0 Then
                .SetValue("Description", aDesc.ToUpper)
            End If
        End With

        Dim lTsDate As atcData.atcTimeseries = New atcData.atcTimeseries(Nothing)
        'TODO: make dates
        'With myDateSummary
        '    .CIntvl = True
        '    .ts = ts
        '    .Tu = Tu
        '    .Intvl = 1
        'End With
        'TsDate.Summary = myDateSummary
        lGenTs.Dates = lTsDate
        lGenTs.Attributes.SetValue("TSTYPE", lGenTs.Attributes.GetValue("Constituent"))
        Return pWDMObj(aWdmId).AddDataset(lGenTs, 0)
    End Function

    Public Sub AddPointSourceDataSet(ByVal aScenario As String, _
                                     ByVal aLocation As String, _
                                     ByVal aConstituent As String, _
                                     ByVal aDescription As String, _
                                     ByVal aTsType As String, _
                                     ByVal aNdates As Integer, _
                                     ByVal aJdates() As Double, _
                                     ByVal aLoad() As Double, _
                                     ByRef aWdmid As Integer, _
                                     ByRef aDsn As Integer)
        If aWdmid = 0 Then
            For lWdmIndex As Integer = 1 To 4
                If Not pWDMObj(lWdmIndex) Is Nothing Then 'use this as the output wdm
                    aWdmid = lWdmIndex
                    Exit For
                End If
            Next lWdmIndex
        End If

        If aWdmid > 0 Then 'okay to continue
            Dim lDsn As Integer = FindFreeDSN(aWdmid, 7000)
            Dim lGenericTs As New atcData.atcTimeseries(Nothing)
            With lGenericTs.Attributes
                .SetValue("ID", lDsn)
                .SetValue("Scenario", aScenario.ToUpper)
                .SetValue("Constituent", aConstituent.ToUpper)
                .SetValue("Location", aLocation.ToUpper)
                .SetValue("Description", aDescription)
                .SetValue("STANAM", aDescription)
                .SetValue("TU", 4)  'assume daily
                .SetValue("TS", 1)
                .SetValue("TSTYPE", aTsType)
            End With

            'set the dates
            Dim lTsDate As atcData.atcTimeseries = New atcData.atcTimeseries(Nothing)
            Dim lNvals As Double
            Dim lSJDate As Double = 0
            Dim lEJDate As Double = 0
            If aNdates = 0 Then 'get dates from global block
                lSJDate = Me.GlobalBlock.SDateJ
                lEJDate = Me.GlobalBlock.EdateJ
            Else   'dates were supplied as an argument
                lSJDate = aJdates(1)
                lEJDate = aJdates(aNdates)
            End If
            lNvals = lEJDate - lSJDate
            Dim lDates(lNvals) As Double
            For lDateIndex As Integer = 0 To lNvals
                lDates(lDateIndex) = lSJDate + lDateIndex
            Next
            lTsDate.Values = lDates
            lGenericTs.Dates = lTsDate

            'now fill in the values
            Dim lValues(lNvals) As Double

            Dim lMultiplier As Double
            Dim lCurDate As Double
            If aConstituent.ToUpper = "FLOW" Then 'keep load in cfs
                lMultiplier = 1.0
            Else 'change load from pounds per hour to pounds per day
                lMultiplier = 24
            End If

            If aNdates = 0 Or aNdates = 1 Then 'use this value for all
                For lValueIndex As Integer = 0 To lNvals
                    lValues(lValueIndex) = aLoad(1) * lMultiplier
                Next
            Else  'use values passed in
                lCurDate = aJdates(1)
                Dim lDayCounter As Integer = 0
                Dim lValueCounter As Integer = 1
                Do While lCurDate <= aJdates(aNdates) 'loop through each day
                    lValues(lDayCounter) = aLoad(lValueCounter) * lMultiplier
                    lDayCounter = lDayCounter + 1
                    lCurDate = lCurDate + 1
                    If lValueCounter < aNdates Then
                        If lCurDate = aJdates(lValueCounter + 1) Then 'increment value
                            lValueCounter += 1
                        End If
                    End If
                Loop
            End If

            lGenericTs.Values = lValues

            Dim lAddedDsn As Boolean = pWDMObj(aWdmid).AddDataset(lGenericTs, 0)
            aDsn = lDsn
        End If
    End Sub

    Public Sub AddPoint(ByVal aWdmId As String, _
                        ByVal aWdmDsn As Integer, _
                        ByVal aTarId As Integer, _
                        ByVal aSourceName As String, _
                        ByVal aTargetGroup As String, _
                        ByVal aTargetMember As String, _
                        ByVal aTargetSub1 As Integer, _
                        ByVal aTargetSub2 As Integer)
        Dim lPoint As New HspfPointSource
        With lPoint
            .MFact = 1
            .Source.VolId = aWdmDsn
            .Source.VolName = aWdmId
            Dim lTimeUnits As Integer
            Dim lDsn As atcData.atcTimeseries = Me.GetDataSetFromDsn(WDMInd(aWdmId), aWdmDsn)
            If Not lDsn Is Nothing Then
                .Con = lDsn.Attributes.GetValue("Constituent")
                .Source.Member = lDsn.Attributes.GetValue("TSTYPE")
                lTimeUnits = lDsn.Attributes.GetValue("tu", 4)
            Else
                lTimeUnits = 4
            End If
            If .Source.Member = "Flow" Or _
               .Source.Member = "FLOW" Or _
               .Source.Member = "flow" Then 'mfactor needs to convert cfs to ac-ft/interval
                .MFact = 0.0826
                .Tran = "SAME"
            Else 'not flow, so assume pounds per day
                Dim lRunTs As Integer = 3
                If Me.OpnSeqBlock.Delt = 1440 Then
                    lRunTs = 4
                End If
                If lTimeUnits > lRunTs Then 'daily pt src in hourly run, for example
                    .Tran = "DIV"
                ElseIf lTimeUnits = lRunTs Then  'hourly in hourly run, for example
                    .Tran = "SAME"
                ElseIf lTimeUnits < lRunTs Then  'hourly pt src in daily run, for example
                    .Tran = "SUM"
                End If
            End If
            .Sgapstrg = ""
            .Ssystem = "ENGL"
            Dim lOpn As HspfOperation = pOpnBlks.Item("RCHRES").OperFromID(aTarId)
            .Target.Opn = lOpn
            .Target.VolName = "RCHRES"
            .Target.VolId = aTarId
            .Target.Group = aTargetGroup
            .Target.Member = aTargetMember
            .Target.MemSub1 = aTargetSub1
            .Target.MemSub2 = aTargetSub2
            .Name = aSourceName

            For Each lPointSource As HspfPointSource In pPointSources
                If lPointSource.Name = .Name And _
                   lPointSource.Target.VolId = aTarId Then
                    'use same id as an existing one
                    .Id = lPointSource.Id
                    Exit For
                End If
            Next lPointSource

            If .Id = 0 Then
                Dim lLastId As Integer = 1
                For Each lPointSource As HspfPointSource In pPointSources
                    If lPointSource.Id >= lLastId Then
                        lLastId = lPointSource.Id + 1
                    End If
                Next lPointSource
                'this is the id for the new one
                .Id = lLastId
            End If

            pPointSources.Add(lPoint)
            lOpn.PointSources.Add(lPoint)
        End With
    End Sub

    Public Sub RemovePoint(ByVal aWdmId As String, _
                           ByVal aWdmDsn As Integer, _
                           ByVal aTarId As Integer)
        For Each lPoint As HspfPointSource In pPointSources
            If lPoint.Source.VolName = aWdmId And _
               lPoint.Source.VolId = aWdmDsn And _
               lPoint.Target.VolId = aTarId Then
                'remove this one
                pPointSources.Remove(lPoint)
                Exit For
            End If
        Next lPoint

        Dim lOpn As HspfOperation = pOpnBlks.Item("RCHRES").OperFromID(aTarId)
        For Each lPoint As HspfPointSource In lOpn.PointSources
            If lPoint.Source.VolName = aWdmId And _
               lPoint.Source.VolId = aWdmDsn And _
               lPoint.Target.VolId = aTarId Then
                'remove this one
                lOpn.PointSources.Remove(lPoint)
                Exit For
            End If
        Next lPoint
    End Sub

    Public Sub GetWDMUnits(ByRef aWdmCount As Integer, ByRef aWdmUnits() As Integer)
        aWdmCount = 0
        For lWdmIndex As Integer = 1 To 4
            If pWdmUnit(lWdmIndex) > 0 Then 'add
                aWdmCount += 1
                ReDim Preserve aWdmUnits(aWdmCount)
                aWdmUnits(aWdmCount) = pWdmUnit(lWdmIndex)
            End If
        Next lWdmIndex
    End Sub

    Public Sub GetWDMIDFromUnit(ByVal aWdmUnit As Integer, ByRef aWdmId As String)
        aWdmId = ""
        For lWdmIndex As Integer = 1 To 4
            If pWdmUnit(lWdmIndex) > 0 Then
                If pWdmUnit(lWdmIndex) = aWdmUnit Then
                    aWdmId = "WDM" & lWdmIndex.ToString
                    Exit For
                End If
            End If
        Next lWdmIndex
    End Sub

    Public Sub RemoveConnectionsFromCollection(ByVal aConnectionType As Integer)
        Dim lConnectionIndex As Integer = 0
        Do While lConnectionIndex < Me.Connections.Count
            'remove this type of connections from pconnections collection
            Dim lConn As HspfConnection = Me.Connections.Item(lConnectionIndex)
            If lConn.Typ = aConnectionType Then
                Me.Connections.RemoveAt(lConnectionIndex)
            Else
                lConnectionIndex += 1
            End If
        Loop
    End Sub

    Public Function Copy() As HspfUci
        Dim lUCI As HspfUci = New HspfUci
        lUCI.Name = Me.Name
        Return lUCI
    End Function

    Public Function WaitForChildMessage() As String
        If pIPCset Then
            Dim lString As String
            Do  'process messages from parent
                lString = pIPC.GetProcessMessage("HSPFUCI") 'pHspfEngine.ReadTokenFromPipe(IPC.ParentRead, pipeBuffer, False)
                If lString.Length > 3 Then
                    Select Case (LCase(Left(lString, 3)))
                        Case "dbg", "msg" ', "com", "act"
                            pIPC.SendMonitorMessage(lString)
                            lString = ""
                    End Select
                End If
            Loop While lString.Length = 0
            Return lString
        Else
            Return "No process available"
        End If
    End Function

    Public Function EchoFileName() As String
        For lFileIndex As Integer = 1 To pFilesBlk.Count
            If pFilesBlk.Value(lFileIndex).Typ = "MESSU" Then
                Return pFilesBlk.Value(lFileIndex).Name
            End If
        Next lFileIndex
        Return ""
    End Function

    Private Sub ReportMissingTimsers(ByRef aReturnCode As Integer)
        If Me.MetSegs.Count > 0 Then
            MetSeg2Source()
        End If
        Point2Source()

        Dim lMissingTimsers As Collection(Of HspfStatusType)
        Dim lMessageText As String = ""
        For Each lOpn As HspfOperation In pOpnSeqBlk.Opns
            'lOpn.InputTimeseriesStatus.Update
            lMissingTimsers = lOpn.InputTimeseriesStatus.GetInfo(HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired, HspfStatus.HspfStatusPresentMissingEnum.HspfStatusMissing)
            If lMissingTimsers.Count > 0 Then
                For i As Integer = 0 To lMissingTimsers.Count - 1
                    lMessageText &= vbCrLf & lOpn.Name & " " & lOpn.Id & " " & lMissingTimsers.Item(i).Name
                Next i
            End If
        Next

        Source2MetSeg()
        Source2Point()

        If lMessageText.Length > 0 Then 'some missing timsers
            If Logger.Msg("WinHSPF has detected missing input time series" & vbCrLf & "required for the selected simulation options:" & vbCrLf & lMessageText & vbCrLf & vbCrLf & "Do you want to try running HSPF anyway?", MsgBoxStyle.OkCancel, "WinHSPF Simulate Problem") = MsgBoxResult.Cancel Then
                aReturnCode = -1
            Else
                aReturnCode = 0
            End If
        End If
    End Sub

    Public Sub PollutantsBuild()
        modPollutantsBuild(Me, Msg)
    End Sub

    Public Sub PollutantsUnBuild()
        modPollutantsUnBuild(Me, Msg)
    End Sub

    Private Sub ProcessFTables()
        Dim lBuff As String = Nothing
        Dim lDone As Boolean = False
        Dim lOmCode As Integer = HspfOmCode("FTABLES")
        Dim lInit As Integer = 1
        Dim lReturnKey As Integer = -1
        Dim lReturnCode As Integer
        Dim lRecordType As Integer
        Do Until lDone
            If Me.FastFlag Then
                GetNextRecordFromBlock("FTABLES", lReturnKey, lBuff, lRecordType, lReturnCode)
            Else
                Call REM_XBLOCK(Me, lOmCode, lInit, lReturnKey, lBuff, lReturnCode)
            End If
            lInit = 0
            If lBuff Is Nothing Then
                lDone = True
            ElseIf lBuff.Substring(2, 6) = "FTABLE" Then 'this is a new one
                Dim lId As Integer = CShort(lBuff.Substring(11, 4))
                'find which operation this ftable is associated with
                Dim lOperation As HspfOperation = Nothing
                For Each lOperationToCheck As HspfOperation In Me.OpnBlks.Item("RCHRES").Ids
                    If lOperationToCheck.Tables.Item("HYDR-PARM2").ParmValue("FTBUCI") = lId Then
                        lOperation = lOperationToCheck
                        Exit For
                    End If
                Next
                If Not lOperation Is Nothing Then
                    If Me.FastFlag Then
                        lRecordType = -999
                        Do Until lRecordType = 0
                            GetNextRecordFromBlock("FTABLES", lReturnKey, lBuff, lRecordType, lReturnCode)
                        Loop
                    Else
                        Call REM_XBLOCK(Me, lOmCode, lInit, lReturnKey, lBuff, lReturnCode)
                    End If
                    With lOperation.FTable
                        Dim lString As String = lBuff.Substring(0, 5)
                        If lString.Trim.Length > 0 Then
                            .Nrows = CInt(lString)
                        Else
                            .Nrows = 0
                        End If
                        lString = lBuff.Substring(5, 5)
                        If lString.Trim.Length > 0 Then
                            .Ncols = CInt(lString)
                        Else
                            .Ncols = 0
                        End If
                        Dim lRow As Integer = 1
                        Do While lRow <= .Nrows
                            If Me.FastFlag Then
                                GetNextRecordFromBlock("FTABLES", lReturnKey, lBuff, lRecordType, lReturnCode)
                            Else
                                lRecordType = 0
                                Call REM_XBLOCK(Me, lOmCode, lInit, lReturnKey, lBuff, lReturnCode)
                            End If
                            If lRecordType = -1 Then 'this is a comment
                                If .Comment.Length = 0 Then
                                    .Comment = lBuff
                                Else
                                    .Comment &= vbCrLf & lBuff
                                End If
                            Else 'this is a regular record
                                .Depth(lRow) = CDbl(Left(lBuff, 10))
                                .DepthAsRead(lRow) = Left(lBuff, 10)
                                .Area(lRow) = CDbl(Mid(lBuff, 11, 10))
                                .AreaAsRead(lRow) = Mid(lBuff, 11, 10)
                                .Volume(lRow) = CDbl(Mid(lBuff, 21, 10))
                                .VolumeAsRead(lRow) = Mid(lBuff, 21, 10)
                                Dim lExit As Integer = .Ncols - 3
                                If lExit > 0 Then
                                    .Outflow1(lRow) = CDbl(Mid(lBuff, 31, 10))
                                    .Outflow1AsRead(lRow) = Mid(lBuff, 31, 10)
                                End If
                                If lExit > 1 Then
                                    .Outflow2(lRow) = CDbl(Mid(lBuff, 41, 10))
                                    .Outflow2AsRead(lRow) = Mid(lBuff, 41, 10)
                                End If
                                If lExit > 2 Then
                                    .Outflow3(lRow) = CDbl(Mid(lBuff, 51, 10))
                                    .Outflow3AsRead(lRow) = Mid(lBuff, 51, 10)
                                End If
                                If lExit > 3 Then
                                    .Outflow4(lRow) = CDbl(Mid(lBuff, 61, 10))
                                    .Outflow4AsRead(lRow) = Mid(lBuff, 61, 10)
                                End If
                                If lExit > 4 Then
                                    .Outflow5(lRow) = CDbl(Mid(lBuff, 71, 10))
                                    .Outflow5AsRead(lRow) = Mid(lBuff, 71, 10)
                                End If
                                lRow += 1
                            End If
                        Loop
                    End With
                End If
            ElseIf lBuff.Trim = "END FTABLES" Then
                lDone = True
            ElseIf lReturnKey = 0 Then
                lDone = True
            ElseIf lReturnCode = 10 Then
                lDone = True
            End If
        Loop
    End Sub

    Private Sub RestartHSPFEngine()
        If pIPCset Then
            Dim HSPFEngineExe As String = GetSetting("HSPFEngine", "files", "HSPFEngine.exe", "HSPFEngine.exe")

            HSPFEngineExe = atcUtility.FindFile("Please locate HSPFEngine.exe", HSPFEngineExe)
            SaveSetting("HSPFEngine", "files", "HSPFEngine.exe", HSPFEngineExe)

            pIPC.ExitProcess("HSPFUCI")
            pIPC.StartProcess("HSPFUCI", HSPFEngineExe & " " & GetCurrentProcessId)
            SendHspfMessage("W99OPN")
            SendHspfMessage("WDBFIN")
            SendHspfMessage("PUTOLV 10")
            SendHspfMessage("SPIPH " & CStr(pIPC.hPipeReadFromParent("HSPFUCI")) & " " & CStr(pIPC.hPipeWriteToParent("HSPFUCI")) & " ")
            SendHspfMessage("WDBOPN " & pMsgWDMName & " 1")
            WaitForChildMessage()
        Else
            Logger.Msg("No interprocess communication available")
        End If
    End Sub

    Public Function CatAsInt(ByRef aCategory As String) As Integer
        'turn a two character category tag into its integer equivalent
        If aCategory.Length > 0 Then
            If Not Me.CategoryBlock Is Nothing Then 'have category block
                For Each lCategory As HspfCategory In Me.CategoryBlock.Categories
                    If lCategory.Tag = aCategory Then
                        Return lCategory.Id
                    End If
                Next lCategory
            End If
        End If
        Return Nothing
    End Function

    Public Function IntAsCat(ByRef aMember As String, _
                             ByRef aSub1or2 As Integer, _
                             ByRef aSint As String) As String
        'given a timeseries member name and a subscript, see if there is a
        'category equivalent.  if so, turn the integer category tag into its
        'two character equivalent
        Dim lIntAsCat As String = aSint
        If Not Me.CategoryBlock Is Nothing Then
            If IsNumeric(aSint) Then
                Dim lSint As Integer = CShort(aSint)
                If Me.CategoryBlock.Categories.Count > 0 And Me.CategoryBlock.Categories.Count >= lSint Then
                    'have category block
                    'check to see if this one is valid to convert into a category tag
                    If aMember = "COTDGT" And aSub1or2 = 2 Or _
                       aMember = "CIVOL" And aSub1or2 = 1 Or _
                       aMember = "CVOL" And aSub1or2 = 1 Or _
                       aMember = "CRO" And aSub1or2 = 1 Or _
                       aMember = "CO" And aSub1or2 = 2 Or _
                       aMember = "CDFVOL" And aSub1or2 = 2 Or _
                       aMember = "CROVOL" And aSub1or2 = 1 Or _
                       aMember = "COVOL" And aSub1or2 = 2 Then
                        IntAsCat = Me.CategoryBlock.Value(lSint).Tag
                    End If
                End If
            End If
        End If
        Return lIntAsCat
    End Function

    Public Sub CreateUciFromBASINS(ByRef aWatershed As Watershed, _
                                   ByRef aDataSources As Collection(Of atcData.atcTimeseriesSource), _
                                   ByRef aStarterUciName As String, _
                                   Optional ByRef aPollutantListFileName As String = "", _
                                   Optional ByRef aMetBaseDsn As Integer = 11, _
                                   Optional ByVal aMetWdmId As String = "WDM2")

        'get starter uci ready for use defaulting parameters and mass links
        Dim lDefUci As New HspfUci
        lDefUci.FastReadUciForStarter(Me.Msg, aStarterUciName)

        modCreateUci.CreateUciFromBASINS(aWatershed, Me, aDataSources, _
                                         lDefUci, _
                                         aPollutantListFileName, aMetBaseDsn, aMetWdmId)
    End Sub

    Public Sub CreateUciFromBASINS(ByRef aWatershed As Watershed, _
                                   ByRef aDataSources As Collection(Of atcData.atcTimeseriesSource), _
                                   ByRef aStarterUci As HspfUci, _
                                   Optional ByRef aPollutantListFileName As String = "", _
                                   Optional ByRef aMetBaseDsn As Integer = 11, _
                                   Optional ByVal aMetWdmId As String = "WDM2")

        modCreateUci.CreateUciFromBASINS(aWatershed, Me, aDataSources, _
                                         aStarterUci, _
                                         aPollutantListFileName, aMetBaseDsn, aMetWdmId)
    End Sub

    Public Function AreaReport(ByVal aReachColumns As Boolean) As String
        Dim lTable As New atcUtility.atcTableDelimited
        With lTable
            .Delimiter = vbTab
            Dim lPerlndCnt As Integer = Me.OpnBlks("PERLND").Ids.Count
            Dim lImplndCnt As Integer = Me.OpnBlks("IMPLND").Ids.Count
            Dim lRchresCnt As Integer = Me.OpnBlks("RCHRES").Ids.Count
            Dim lBmpracCnt As Integer = Me.OpnBlks("BMPRAC").Ids.Count
            .NumFields = lPerlndCnt + lImplndCnt + 2
            .NumRecords = lRchresCnt + lBmpracCnt + 2
            Dim lFieldIndex As Integer = 1
            .FieldName(lFieldIndex) = "BorRID"
            For Each lOperation As atcUCI.HspfOperation In Me.OpnBlks("PERLND").Ids
                lFieldIndex += 1
                .FieldName(lFieldIndex) = "P:" & lOperation.Id
            Next
            For Each lOperation As atcUCI.HspfOperation In Me.OpnBlks("IMPLND").Ids
                lFieldIndex += 1
                .FieldName(lFieldIndex) = "I:" & lOperation.Id
            Next
            lFieldIndex += 1
            .FieldName(lFieldIndex) = "Total"

            .CurrentRecord = 1
            For Each lOperation As atcUCI.HspfOperation In Me.OpnBlks("BMPRAC").Ids
                .Value(1) = "B:" & lOperation.Id
                For Each lConnection As atcUCI.HspfConnection In lOperation.Sources
                    If lConnection.Source.VolName = "PERLND" OrElse _
                       lConnection.Source.VolName = "IMPLND" Then
                        lFieldIndex = 2
                        While lFieldIndex < .NumFields
                            If .FieldName(lFieldIndex).Substring(2) = lConnection.Source.VolId Then
                                If lFieldIndex = 2 AndAlso .Value(1) = "B:11" Then
                                    Debug.Print("HI")
                                End If
                                If .Value(lFieldIndex).Length = 0 Then
                                    .Value(lFieldIndex) = lConnection.MFact
                                Else
                                    .Value(lFieldIndex) += lConnection.MFact
                                End If
                                Exit While
                            End If
                            lFieldIndex += 1
                        End While
                    End If
                Next
                .CurrentRecord += 1
            Next
            For Each lOperation As atcUCI.HspfOperation In Me.OpnBlks("RCHRES").Ids
                .Value(1) = "R:" & lOperation.Id
                For Each lConnection As atcUCI.HspfConnection In lOperation.Sources
                    If lConnection.Source.VolName = "PERLND" OrElse _
                       lConnection.Source.VolName = "IMPLND" Then
                        lFieldIndex = 2
                        While lFieldIndex < .NumFields
                            If .FieldName(lFieldIndex).Substring(2) = lConnection.Source.VolId Then
                                If .FieldName(lFieldIndex) = "P:101" And .Value(1) = "R:1" Then
                                    Logger.Dbg(.FieldName(lFieldIndex) & " " & lConnection.MFact)
                                End If
                                If .Value(lFieldIndex).Length = 0 Then
                                    .Value(lFieldIndex) = lConnection.MFact
                                Else
                                    .Value(lFieldIndex) += lConnection.MFact
                                End If
                                Exit While
                            End If
                            lFieldIndex += 1
                        End While
                    End If
                Next
                .CurrentRecord += 1
            Next
            .Value(1) = "Total"

            Dim lFieldTotals(.NumFields) As Double
            .CurrentRecord = 1
            While .CurrentRecord < .NumRecords
                For lFieldIndex = 2 To .NumFields - 1
                    If .Value(lFieldIndex).Length > 0 Then
                        If .Value(.NumFields).Length = 0 Then
                            .Value(.NumFields) = CDbl(.Value(lFieldIndex))
                        Else
                            .Value(.NumFields) += CDbl(.Value(lFieldIndex))
                        End If
                        lFieldTotals(lFieldIndex) += .Value(lFieldIndex)
                        lFieldTotals(.NumFields) += .Value(lFieldIndex)
                    End If
                Next
                .CurrentRecord += 1
            End While
            For lFieldIndex = 2 To .NumFields
                .Value(lFieldIndex) = lFieldTotals(lFieldIndex)
            Next
        End With

        Dim lStr As String
        If aReachColumns Then
            Dim lGridSource As New atcControls.atcGridSourceTable
            lGridSource.Table = lTable
            Dim lGridSourceRowColSwapper As New atcControls.atcGridSourceRowColumnSwapper(lGridSource)
            lGridSourceRowColSwapper.SwapRowsColumns = True
            lStr = lGridSourceRowColSwapper.ToString()
        Else
            lStr = lTable.ToString
        End If
        Return lStr
    End Function
End Class
