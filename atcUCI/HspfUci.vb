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
    Private pHspfProcess As New Process

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

    Public AcidPhFlag As Boolean = False
    Public MetSegs As Collection(Of HspfMetSeg)

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
    Public Function OpnBlks() As KeyedCollection(Of String, HspfOpnBlk)
        Return pOpnBlks
    End Function

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

    Public Sub SendHspfMessage(ByVal aMessage As String)
        'If pIPCset Then
        '    pIPC.SendProcessMessage("HSPFUCI", aMessage)
        'End If
    End Sub

    Public Sub SendMonitorMessage(ByVal aMessage As String)
        'If pIPCset Then
        '    pIPC.SendMonitorMessage(aMessage)
        'End If
    End Sub

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

    Public ReadOnly Property WDMCount() As Integer
        Get
            Return pWdmCount
        End Get
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
            pFilesBlk.newName(aOldName, aNewName)
            NewOutputDsns(aOldName, aNewName, aBaseDsn, aRelAbs)
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
        pTserFiles = New atcData.atcTimeseriesGroup   'not fully implemented, pWDMObj(4) used instead
    End Sub

    Public Sub FastReadUciForStarter(ByRef aMsg As HspfMsg, ByRef aNewName As String)
        Dim lFilesOK As Boolean
        Dim lFullFg As Integer
        Dim lEchoFile As String = ""

        lFullFg = -1
        ReadUci(aMsg, aNewName, lFullFg, lFilesOK, lEchoFile)
    End Sub

    Public Sub ReadUciWithWDMs(ByRef aMsg As HspfMsg, ByRef aNewName As String)
        'called by scripthspf, processes wdm files
        Dim lFilesOK As Boolean
        Dim lFullFg As Integer
        Dim lEchoFile As String = ""

        lFullFg = -3
        ReadUci(aMsg, aNewName, lFullFg, lFilesOK, lEchoFile)
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

                pInitialized = True

                SendMonitorMessage("(Show)") 'where was the hide?

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
                lMassLink.ReadMassLinks(Me)
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
                            If lComment.Length = 0 And Not lConnection.Comment Is Nothing Then
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
                            For lIndex As Integer = 0 To lConnection.Target.Opn.Targets.Count - 1
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

        Dim lReturnCode As Integer = 0
        ReportMissingTimsers(lReturnCode)
        If lReturnCode = 0 Then 'user chose do anyway after timser warning

            Dim lProcessId As Integer = Process.GetCurrentProcess.Id
            pHspfProcess = New Process
            With pHspfProcess.StartInfo
                Dim HSPFEngineExe As String = GetSetting("HSPFEngineNet", "files", "HSPFEngineNet.exe", "HSPFEngineNet.exe")
                HSPFEngineExe = atcUtility.FindFile("Please locate HSPFEngineNet.exe", HSPFEngineExe)
                SaveSetting("HSPFEngine", "files", "HSPFEngineNet.exe", HSPFEngineExe)
                'note: the file HSPFEngineNet.exe is built over in D:\dev\HSPF\
                .FileName = HSPFEngineExe
                .Arguments = lProcessId '& " wait"
                .CreateNoWindow = True
                .UseShellExecute = False
                .RedirectStandardInput = True
                .RedirectStandardOutput = True
                AddHandler pHspfProcess.OutputDataReceived, AddressOf HspfMessageHandler
                .RedirectStandardError = True
                AddHandler pHspfProcess.ErrorDataReceived, AddressOf HspfMessageHandler
            End With
            Logger.Dbg("AboutToStart HSPF")
            pHspfProcess.Start()
            Logger.Dbg("Listen for Output or Error")
            pHspfProcess.BeginOutputReadLine()
            pHspfProcess.BeginErrorReadLine()

            System.Threading.Thread.Sleep(1000)
            pHspfProcess.StandardInput.WriteLine("MONITOR")

            Logger.Dbg("W99OPN")
            'System.Threading.Thread.Sleep(1000)
            pHspfProcess.StandardInput.WriteLine("W99OPN")

            Dim lPath As String = IO.Path.GetDirectoryName(Name)
            If lPath.Length > 0 Then
                ChDriveDir(lPath)
            End If
            Logger.Dbg("Curdir " & CurDir())
            If lPath.Length > 0 Then
                pHspfProcess.StandardInput.WriteLine("CURDIR " & lPath)
            End If
            Logger.Dbg("CurdirAfterPath " & CurDir())

            Dim lFileName As String = IO.Path.GetFileNameWithoutExtension(Name)
            Dim lOption As Integer = -1 'dont interp in actscn (itll be done in simscn)
            pHspfProcess.StandardInput.WriteLine("ACTIVATE " & lFileName & " " & lOption)

            pHspfProcess.WaitForExit()

            'have to reset wdms, may have changed pointers during simulate
            ClearWDM()
            SetWDMFiles()

        End If
    End Sub

    Private Sub HspfMessageHandler(ByVal aSendingProcess As Object, _
                                   ByVal aOutLine As DataReceivedEventArgs)
        If Not String.IsNullOrEmpty(aOutLine.Data) Then
            Dim lMsg As String = aOutLine.Data.ToString
            If lMsg.StartsWith("Activate complete") Then
                System.Threading.Thread.Sleep(2000)
                Logger.Dbg("SimulateStart")
                pHspfProcess.StandardInput.WriteLine("SIMULATE") 'calls F90_SIMSCN
            ElseIf lMsg.StartsWith("Simulate complete 0") Then
                System.Threading.Thread.Sleep(2000)
                Logger.Dbg("SimulateDone, TryToExit")
                pHspfProcess.StandardInput.WriteLine("EXIT")
            ElseIf lMsg.ToLower = "cancel" Then
                Application.DoEvents()
                System.Threading.Thread.Sleep(1000)
                If pHspfProcess.HasExited Then
                    Logger.Dbg("HSPF already exited")
                Else
                    pHspfProcess.StandardInput.WriteLine("MSG1 Canceled")
                    Application.DoEvents()
                    System.Threading.Thread.Sleep(2000)
                    pHspfProcess.Kill()
                End If
            ElseIf (Right(lMsg, 1) <> "0" AndAlso InStr(lMsg, "SPIPH") = 0) Or lMsg.StartsWith("HSPFUCI exited with code") Then
                pErrorDescription = "Fatal HSPF error while running UCI file '" & Name.Trim & "'." & vbCrLf & vbCrLf & "See the file '" & EchoFileName.Trim & "' for more details."
                Logger.Msg(pErrorDescription, MsgBoxStyle.Critical, "Problem Running HSPF")
                pHspfProcess.StandardInput.WriteLine("EXIT")
            ElseIf lMsg IsNot Nothing Then
                Logger.Dbg("Ignore " & lMsg)
            End If
        End If
    End Sub

    Public Sub DeleteOperation(ByRef aName As String, ByRef aId As Integer)
        'figure out where this operation is in operation sequence block and delete it
        Dim lDeleteOperationAtIndex As New Collection
        For lOperationIndex As Integer = 0 To pOpnSeqBlk.Opns.Count - 1
            Dim lHspfOperation As HspfOperation = pOpnSeqBlk.Opns.Item(lOperationIndex)
            If lHspfOperation.Name = aName AndAlso lHspfOperation.Id = aId Then
                'save the position of this operation for deleting
                lDeleteOperationAtIndex.Add(lOperationIndex)
            End If
        Next
        For lOperIndex As Integer = 1 To lDeleteOperationAtIndex.Count
            pOpnSeqBlk.Delete(lDeleteOperationAtIndex(lOperIndex))
        Next

        'need to remove from all operation type blocks
        Dim lOpnBlk As HspfOpnBlk = pOpnBlks.Item(aName)
        If Not lOpnBlk.OperFromID(aId) Is Nothing Then
            lOpnBlk.Ids.Remove("K" & aId)
        End If

        'remove connections
        'need to remove connections between this and anything else
        Dim lSourceCount As Integer = 0
        Dim lSourceVolId() As Integer = {}
        Dim lTargetVolId As Integer = 0
        Dim lRemoveUciConnectionAtIndex As New Collection
        Dim lMassLink As Integer = 0
        For lHspfConnectionIndex As Integer = 0 To Me.Connections.Count - 1
            Dim lHspfConnection As HspfConnection = Me.Connections.Item(lHspfConnectionIndex)

            If (lHspfConnection.Source.VolName = aName And lHspfConnection.Source.VolId = aId) Or (lHspfConnection.Target.VolName = aName And lHspfConnection.Target.VolId = aId) Then
                lMassLink = lHspfConnection.MassLink
                If lHspfConnection.Target.VolId = aId And lHspfConnection.Target.VolName = aName And lHspfConnection.Source.VolName = aName Then
                    'remember the source
                    lSourceCount += 1
                    ReDim Preserve lSourceVolId(lSourceCount)
                    lSourceVolId(lSourceCount) = lHspfConnection.Source.VolId
                ElseIf lHspfConnection.Source.VolId = aId And lHspfConnection.Source.VolName = aName And lHspfConnection.Target.VolName = aName Then
                    'remember the target
                    lTargetVolId = lHspfConnection.Target.VolId
                End If
                lRemoveUciConnectionAtIndex.Add(lHspfConnectionIndex)
            End If
        Next

        Dim lOffsetAfterDeleteIndex As Integer = 0
        For lOperIndex As Integer = 1 To lRemoveUciConnectionAtIndex.Count
            Me.Connections.RemoveAt(lRemoveUciConnectionAtIndex.Item(lOperIndex) - lOffsetAfterDeleteIndex)
            lOffsetAfterDeleteIndex += 1
        Next

        If lSourceCount > 0 And lTargetVolId > 0 Then
            'need to join sources and targets of this deleted opn
            For lSourceConnectionIndex As Integer = 1 To lSourceCount
                Dim lConnection As HspfConnection = New HspfConnection
                lConnection.Uci = Me
                lConnection.Typ = 3
                lConnection.Source.VolName = aName
                lConnection.Source.VolId = lSourceVolId(lSourceConnectionIndex)
                lConnection.Source.Opn = pOpnBlks.Item(aName).OperFromID(lSourceVolId(lSourceConnectionIndex))
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
            Next
        End If

        'remove this oper from source and target collections for other operations
        For lHspfOperationIndex As Integer = 0 To pOpnSeqBlk.Opns.Count - 1
            Dim lHspfOperation As HspfOperation = pOpnSeqBlk.Opns.Item(lHspfOperationIndex)

            Dim lDeleteTargetAtIndex As New Collection
            For lTargetIndex As Integer = 0 To lHspfOperation.Targets.Count - 1
                If lHspfOperation.Targets.Item(lTargetIndex).Target.VolId = aId AndAlso lHspfOperation.Targets.Item(lTargetIndex).Target.VolName = aName Then
                    lDeleteTargetAtIndex.Add(lTargetIndex)
                End If
            Next

            lOffsetAfterDeleteIndex = 0
            For lOperIndex As Integer = 1 To lDeleteTargetAtIndex.Count
                Me.Connections.RemoveAt(lDeleteTargetAtIndex.Item(lOperIndex) - lOffsetAfterDeleteIndex)
                lOffsetAfterDeleteIndex += 1
            Next

            Dim lDeleteSourceAtIndex As New Collection
            For lSourceIndex As Integer = 0 To lHspfOperation.Sources.Count - 1
                If lHspfOperation.Sources.Item(lSourceIndex).Source.VolId = aId AndAlso lHspfOperation.Sources.Item(lSourceIndex).Source.VolName = aName Then
                    lDeleteSourceAtIndex.Add(lSourceIndex)
                End If
            Next

            lOffsetAfterDeleteIndex = 0
            For lOperIndex As Integer = 1 To lDeleteSourceAtIndex.Count
                Me.Connections.RemoveAt(lDeleteSourceAtIndex.Item(lOperIndex) - lOffsetAfterDeleteIndex)
                lOffsetAfterDeleteIndex += 1
            Next
        Next
    End Sub

    Public Sub ClearWDM()
        For lWdmIndex As Integer = 0 To 4
            If Not pWDMObj(lWdmIndex) Is Nothing Then
                pWDMObj(lWdmIndex) = Nothing
            End If
        Next lWdmIndex
        pTserFiles.Clear()
        pWdmCount = 0
    End Sub

    Public Sub GetMetSegNames(ByRef aMetSegNames As Collection, ByRef aMetSegBaseDsns As Collection, ByRef aMetSegWDMIds As Collection, ByRef aMetSegDescs As Collection)

        'look for matching WDM datasets
        Dim lts As Collection = FindTimser("", "", "PREC")
        Dim lLoc As String
        Dim lSen As String
        'return the names of the data sets from this wdm file
        For Each lTser As atcData.atcTimeseries In lts
            lLoc = lTser.Attributes.GetValue("Location")
            lSen = lTser.Attributes.GetValue("Scenario")
            If lSen = "COMPUTED" Then
                'see if there is also observed at this location, skip this if there is
                Dim lLocts As Collection = FindTimser("OBSERVED", lLoc, "PREC")
                If lLocts.Count > 0 Then
                    lSen = "SKIP"
                End If
            End If
            If lSen = "OBSERVED" Or lSen = "COMPUTED" Then
                If Len(lLoc) > 0 Then
                    'this is one we want, save info about this met station
                    aMetSegNames.Add(lLoc)
                    aMetSegBaseDsns.Add(lTser.Attributes.GetValue("ID"))
                    aMetSegWDMIds.Add(GetWDMIdFromName(lTser.Attributes.GetValue("Data Source")))
                    aMetSegDescs.Add(lTser.Attributes.GetValue("STANAM"))
                End If
            End If
        Next
    End Sub

    Private Function FindFreeDSN(ByVal aWdmId As Integer, ByVal aStartDSN As Integer) As Integer
        Dim lFreeDsn As Integer = aStartDSN + 1
        While Not GetDataSetFromDsn(aWdmId, lFreeDsn) Is Nothing
            lFreeDsn += 1
        End While
        Return lFreeDsn
    End Function

    Public Sub AddExpertSystem(ByRef aId As Integer, _
                               ByRef aLocn As String, _
                               ByVal aWdm As atcWDM.atcDataSourceWDM, _
                               ByVal aWdmID As Integer, _
                               ByRef aBaseDsn As Integer, _
                               ByRef aDsns() As Integer, _
                               ByRef aOstr() As String, _
                               Optional ByRef aUpstreamArea As Double = 0.0)
        'TODO: think this through with PaulDuda!!!!!
        If pWdmCount = 0 Then
            pWDMObj(aWdmID) = aWdm
            AddExpertSystem(aId, aLocn, aWdmID, aBaseDsn, aDsns, aOstr, aUpstreamArea)
        End If
    End Sub

    Public Sub AddExpertSystem(ByRef aId As Integer, _
                               ByRef aLocn As String, _
                               ByVal aWdmId As Integer, _
                               ByRef aBaseDsn As Integer, _
                               ByRef aDsns() As Integer, _
                               ByRef aOstr() As String, _
                               Optional ByRef aUpstreamArea As Double = 0.0)
        'add data sets
        AddExpertDsns(aId, aLocn, aWdmId, aBaseDsn, aDsns, aOstr)
        'add to copy block
        Dim lCopyId As Integer = 1
        AddOperation("COPY", lCopyId)
        AddTable("COPY", lCopyId, "TIMESERIES")
        Dim lTable As HspfTable = OpnBlks("COPY").OperFromID(lCopyId).Tables("TIMESERIES")
        lTable.Parms("NMN").Value = 8
        'add to opn seq block
        OpnSeqBlock.Add(OpnBlks("COPY").OperFromID(lCopyId))
        'add to ext targets block
        Dim lContribArea As Double = aUpstreamArea
        If aUpstreamArea < 0.001 Then
            lContribArea = UpstreamArea(OpnBlks.Item("RCHRES").OperFromID(aId))
        End If
        AddExpertExtTargets(aId, lCopyId, aWdmId, lContribArea, aDsns, aOstr)
        'add mass-link and schematic copy records
        AddExpertSchematic(aId, lCopyId)
    End Sub

    Public Sub AddExpertDsns(ByVal aId As Integer, _
                             ByVal aLocn As String, _
                             ByVal aWdmId As Integer, _
                             ByVal aBaseDsn As Integer, _
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
        aOstr(9) = "SUPY    "

        If aWdmId > 0 Then 'okay to continue
            Dim lDsn As Integer = aBaseDsn
            Dim lScenario As String = IO.Path.GetFileNameWithoutExtension(Name)

            For lIndex As Integer = 1 To 9 'create each of the expert system dsns if missing
                Dim lMatchTimser As Collection = FindTimser(lScenario, aLocn, aOstr(lIndex).ToUpper)
                If lMatchTimser.Count > 0 Then
                    lDsn = CType(lMatchTimser(0), atcTimeseries).Attributes.GetValue("ID", 0).Value
                Else
                    lDsn = FindFreeDSN(aWdmId, lDsn)
                    Dim lGenTs As atcData.atcTimeseries = New atcData.atcTimeseries(Nothing)
                    With lGenTs.Attributes
                        .SetValue("ID", lDsn)
                        .SetValue("Scenario", lScenario.ToUpper)
                        .SetValue("Constituent", aOstr(lIndex).ToUpper)
                        .SetValue("Location", aLocn.ToUpper)
                        .SetValue("TU", 4)
                        .SetValue("TS", 1)
                        .SetValue("TSTYPE", aOstr(lIndex).ToUpper)
                        .SetValue("Data Source", pWDMObj(aWdmId).Specification)
                    End With
                    Dim lTsDate As atcData.atcTimeseries = New atcData.atcTimeseries(Nothing)
                    lGenTs.Dates = lTsDate

                    Dim lAddedDsn As Boolean = pWDMObj(aWdmId).AddDataset(lGenTs)
                End If
                aDsn(lIndex) = lDsn
            Next lIndex
        Else 'no wdm files in this uci
            Logger.Msg("No WDM Files are available with this UCI, so no calibration locations may be added", MsgBoxStyle.OkOnly, "Add Problem")
        End If

    End Sub

    Public Sub AddAQUATOXDsns(ByRef aId As Integer, _
                              ByRef aLocn As String, _
                              ByRef aBaseDsn As Integer, _
                              ByRef aPlankFg As Integer, _
                              ByRef aGqualFg() As Integer, _
                              ByRef aWdmId As Integer, _
                              ByRef aMember() As String, _
                              ByRef aSub1() As Integer, _
                              ByRef aGroup() As String, _
                              ByRef aDsn() As Integer, _
                              ByRef aOstr() As String)
        AddAQUATOXDsnsExt(aId, aLocn, aBaseDsn, aPlankFg, aGqualFg, aWdmId, aMember, aSub1, aGroup, aDsn, aOstr, 4)
    End Sub

    Public Sub AddAQUATOXDsnsExt(ByRef aId As Integer, _
                                 ByRef aLocn As String, _
                                 ByRef aBaseDsn As Integer, _
                                 ByRef aPlankFg As Integer, _
                                 ByRef aGqualFg() As Integer, _
                                 ByRef aWdmId As Integer, _
                                 ByRef aMember() As String, _
                                 ByRef aSub1() As Integer, _
                                 ByRef aGroup() As String, _
                                 ByRef aDsn() As Integer, _
                                 ByRef aOstr() As String, _
                                 ByRef aOutTu As Integer)

        aMember(1) = "VOL" : aSub1(1) = 1 : aGroup(1) = "HYDR" : aOstr(1) = "VOL     " 'volume (ac.ft) AVER
        aMember(2) = "IVOL" : aSub1(2) = 1 : aGroup(2) = "HYDR" : aOstr(2) = "IVOL    " 'inflow (ac.ft) SUM
        aMember(3) = "RO" : aSub1(3) = 1 : aGroup(3) = "HYDR" : aOstr(3) = "RO      " 'discharge in cfs AVER
        aMember(4) = "SAREA" : aSub1(4) = 1 : aGroup(4) = "HYDR" : aOstr(4) = "SARA     " 'surface area in acres AVER
        aMember(5) = "AVDEP" : aSub1(5) = 1 : aGroup(5) = "HYDR" : aOstr(5) = "AVDP    " 'mean depth in feet AVER
        aMember(6) = "PRSUPY" : aSub1(6) = 1 : aGroup(6) = "HYDR" : aOstr(6) = "PSUP    " 'volume in from precip (ac.ft) SUM
        aMember(7) = "VOLEV" : aSub1(7) = 1 : aGroup(7) = "HYDR" : aOstr(7) = "VEVP    " 'volume out to evap (ac.ft) SUM
        aMember(8) = "TW" : aSub1(8) = 1 : aGroup(8) = "HTRCH" : aOstr(8) = "TW      " 'water temp in degrees AVER
        aMember(9) = "NUIF1" : aSub1(9) = 1 : aGroup(9) = "NUTRX" : aOstr(9) = "NO3     " 'inflow of no3 in lbs SUM
        aMember(10) = "NUIF1" : aSub1(10) = 2 : aGroup(10) = "NUTRX" : aOstr(10) = "NH3     " 'inflow of nh2 in lbs SUM
        aMember(11) = "NUIF1" : aSub1(11) = 3 : aGroup(11) = "NUTRX" : aOstr(11) = "NO2     " 'inflow of no2 in lbs SUM
        aMember(12) = "NUIF1" : aSub1(12) = 4 : aGroup(12) = "NUTRX" : aOstr(12) = "PO4     " 'inflow of po4 in lbs SUM
        aMember(13) = "OXIF" : aSub1(13) = 1 : aGroup(13) = "OXRX" : aOstr(13) = "DO      " 'inflow of do in lbs SUM
        aMember(14) = "OXIF" : aSub1(14) = 2 : aGroup(14) = "OXRX" : aOstr(14) = "BOD     " 'inflow of bod in lbs SUM
        aMember(15) = "PKIF" : aSub1(15) = 5 : aGroup(15) = "PLANK" : aOstr(15) = "ORC     " 'inflow of organic c in lbs SUM
        aMember(16) = "PKIF" : aSub1(16) = 1 : aGroup(16) = "PLANK" : aOstr(16) = "PHYT    " 'inflow of phyto in lbs SUM
        aMember(17) = "ISED" : aSub1(17) = 1 : aGroup(17) = "SEDTRN" : aOstr(17) = "ISD1    " 'inflow of sediment in tons SUM
        aMember(18) = "ISED" : aSub1(18) = 2 : aGroup(18) = "SEDTRN" : aOstr(18) = "ISD2    " 'inflow of sediment in tons SUM
        aMember(19) = "ISED" : aSub1(19) = 3 : aGroup(19) = "SEDTRN" : aOstr(19) = "ISD3    " 'inflow of sediment in tons SUM
        aMember(20) = "SSED" : aSub1(20) = 1 : aGroup(20) = "SEDTRN" : aOstr(20) = "SSD1    " 'sediment conc mg/l AVER
        aMember(21) = "SSED" : aSub1(21) = 2 : aGroup(21) = "SEDTRN" : aOstr(21) = "SSD2    " 'sediment conc mg/l AVER
        aMember(22) = "SSED" : aSub1(22) = 3 : aGroup(22) = "SEDTRN" : aOstr(22) = "SSD3    " 'sediment conc mg/l AVER
        aMember(23) = "TIQAL" : aSub1(23) = 1 : aGroup(23) = "GQUAL" : aOstr(23) = "TIQ1    " 'total inflow of qual SUM
        aMember(24) = "TIQAL" : aSub1(24) = 2 : aGroup(24) = "GQUAL" : aOstr(24) = "TIQ2    " 'total inflow of qual SUM
        aMember(25) = "TIQAL" : aSub1(25) = 3 : aGroup(25) = "GQUAL" : aOstr(25) = "TIQ3    " 'total inflow of qual SUM
        aMember(26) = "NUIF2" : aSub1(26) = 4 : aGroup(26) = "NUTRX" : aOstr(26) = "PPO4    " 'inflow of particulate po4 in lbs SUM
        aMember(27) = "TPKIF" : aSub1(27) = 2 : aGroup(27) = "PLANK" : aOstr(27) = "TORP    " 'inflow of total organic p in lbs SUM
        aMember(28) = "TPKIF" : aSub1(28) = 5 : aGroup(28) = "PLANK" : aOstr(28) = "TTP     " 'inflow of total p in lbs SUM

        If aPlankFg <> 1 Then
            aOstr(15) = ""
            aOstr(16) = ""
            aOstr(27) = ""
            aOstr(28) = ""
        End If

        If aGqualFg(1) <> 1 Then 'if any organic chemicals
            aOstr(23) = ""
        End If
        If aGqualFg(2) <> 1 Then
            aOstr(24) = ""
        End If
        If aGqualFg(3) <> 1 Then
            aOstr(25) = ""
        End If

        'check to see that all timsers have inputs
        Dim lOper As HspfOperation = pOpnBlks.Item("RCHRES").OperFromID(aId)
        Dim lTable As HspfTable
        If lOper.TableExists("NUT-FLAGS") Then
            lTable = lOper.Tables.Item("NUT-FLAGS")
            If lTable.Parms("NH3FG").Value = 0 Then
                aOstr(10) = ""
            End If
            If lTable.Parms("NO2FG").Value = 0 Then
                aOstr(11) = ""
            End If
            If lTable.Parms("PO4FG").Value = 0 Then
                aOstr(12) = ""
            End If
        Else
            aOstr(10) = ""
            aOstr(11) = ""
            aOstr(12) = ""
            aOstr(26) = ""
        End If
        If lOper.TableExists("PLNK-FLAGS") Then
            lTable = lOper.Tables.Item("PLNK-FLAGS")
            If lTable.Parms("PHYFG").Value = 0 Then
                aOstr(16) = ""
            End If
        Else
            aOstr(16) = ""
        End If

        aWdmId = 0
        For lWdmIndex As Integer = 4 To 1 Step -1
            If Not pWDMObj(lWdmIndex) Is Nothing Then 'use this as the output wdm
                aWdmId = lWdmIndex
                Exit For
            End If
        Next lWdmIndex

        If aWdmId > 0 Then
            'okay to continue
            Dim lDsn As Integer = aBaseDsn
            Dim lScenario As String = IO.Path.GetFileNameWithoutExtension(Name)

            For lIndex As Integer = 1 To 28
                'create each of the 28 aquatox dsns

                Dim lReferenced As Boolean
                Dim lGenTs As atcData.atcTimeseries
                If aOstr(lIndex).Length > 0 Then
                    'if there is already a dsn with this scen/loc/cons,
                    'and it is unused in this uci, delete it to avoid confusion
                    Dim lDeletedDsn As Integer = 0
                    Dim lts As Collection = FindTimser(UCase(Trim(lScenario)), Trim(aLocn), Trim(aOstr(lIndex)))
                    For Each lGenTs In lts
                        Dim lWid As String = GetWDMIdFromName(lGenTs.Attributes.GetValue("Data Source"))
                        If CShort(Right(lWid, 1)) = aWdmId Then
                            'this is on our output wdm
                            'make sure it is not referenced in this UCI already
                            lReferenced = False
                            Dim lctmp As String
                            For Each lConn As HspfConnection In Me.Connections
                                lctmp = lConn.Target.VolName
                                If lctmp = "WDM" Then lctmp = "WDM1"
                                If lctmp = lWid And lConn.Target.VolId = lGenTs.Attributes.GetValue("ID") Then
                                    'this dataset is referenced in the uci, don't delete
                                    lReferenced = True
                                End If
                            Next lConn
                            If Not lReferenced Then
                                'delete it to avoid confusion
                                lDeletedDsn = lGenTs.Attributes.GetValue("ID")
                                ClearWDMDataSet(lWid, lDeletedDsn)
                                DeleteWDMDataSet(lWid, lDeletedDsn)
                            End If
                        End If
                    Next

                    If lDeletedDsn > 0 Then
                        lDsn = lDeletedDsn
                    Else
                        lDsn = FindFreeDSN(aWdmId, lDsn)
                    End If

                    lGenTs = New atcData.atcTimeseries(Nothing)
                    With lGenTs.Attributes
                        .SetValue("ID", lDsn)
                        .SetValue("Scenario", lScenario.ToUpper)
                        .SetValue("Constituent", aOstr(lIndex).ToUpper)
                        .SetValue("Location", aLocn.ToUpper)
                        .SetValue("Description", "AQUATOX Linkage Timeseries for " & aOstr(lIndex))
                        .SetValue("TSTYPE", aOstr(lIndex).ToUpper)
                        .SetValue("TU", aOutTu)
                        .SetValue("TS", 1)
                        .SetValue("Data Source", pWDMObj(aWdmId).Specification)
                    End With

                    Dim lTsDate As atcData.atcTimeseries = New atcData.atcTimeseries(Nothing)
                    lGenTs.Dates = lTsDate

                    Dim lAddedDsn As Boolean = pWDMObj(aWdmId).AddDataset(lGenTs)
                    aDsn(lIndex) = lDsn
                End If
            Next
        Else
            'no wdm files in this uci
            Logger.Msg("No WDM Files are available with this UCI, so no AQUATOX locations may be added", MsgBoxStyle.OkOnly, "Add Problem")
        End If
    End Sub

    Public Sub AddExpertExtTargets(ByRef reachid As Integer, _
                                   ByRef copyid As Integer, _
                                   ByVal aWdmId As Integer, _
                                   ByRef ContribArea As Single, _
                                   ByRef adsn() As Integer, _
                                   ByRef ostr() As String)
        Dim i As Integer
        Dim MFact As Single
        Dim Tran, gap As String

        MFact = 12.0# / ContribArea
        'mfact = Format(mfact, "0.#######")
        AddExtTarget("RCHRES", reachid, "ROFLOW", "ROVOL", 1, 1, MFact, "    ", "WDM" & aWdmId, adsn(1), ostr(1), 1, "ENGL", "AGGR", "REPL")

        If copyid > 0 Then
            MFact = 1.0# / ContribArea
            'mfact = Format(mfact, "0.#######")
            For i = 2 To 9
                If i < 7 Or i = 9 Then
                    Tran = "    "
                Else
                    Tran = "AVER"
                End If
                'If i < 5 Then
                '  gap = "    "
                'Else
                gap = "AGGR"
                'End If

                AddExtTarget("COPY", copyid, "OUTPUT", "MEAN", i - 1, 1, MFact, Tran, "WDM" & aWdmId, adsn(i), ostr(i), 1, "ENGL", gap, "REPL")
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
        'TODO: figure out if to use a term from SNOW
        lConsName.Add("P:SUPY", "8")
        lConsName.Add("I:SURO", "1")
        lConsName.Add("I:PET", "4")
        lConsName.Add("I:IMPEV", "5")
        'TODO: figure out if to use a term from SNOW
        lConsName.Add("I:SUPY", "8")

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
        Dim lOperations As Collection(Of HspfOperation) = FindUpstreamOpns(lOperation, True)
        Do While lOperations.Count > 0
            lOperation = lOperations.Item(0)
            lOperations.RemoveAt(0)
            AddCopyToSchematic(lOperation, aCopyId, lPerlndMassLinkNumber, lImplndMassLinkNumber)
            'TODO: this overwrote loperations!
            'lOperations = FindUpstreamOpns(lOperation)
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
                .SetValue("Data Source", pWDMObj(aWdmId).Specification)
            End With
            Dim lTsDate As atcData.atcTimeseries = New atcData.atcTimeseries(Nothing)
            lGenericTs.Dates = lTsDate

            Dim lAddedDsn As Boolean = pWDMObj(aWdmId).AddDataset(lGenericTs, 0)
            aDsn = lDsn
        End If
    End Sub

    Public Sub ClearWDMDataSet(ByRef aWdmId As String, ByRef aDsn As Integer)

        Dim lId As Integer
        If aWdmId.Length < 4 Then
            lId = 1
        Else
            lId = CShort(aWdmId.Substring(3, 1))
        End If
        Dim NewGenTs As New atcData.atcTimeseries(Nothing)
        If Not pWDMObj(lId) Is Nothing Then
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
            lId = CShort(aWdmId.Substring(3, 1))
        End If

        If Not pWDMObj(lId) Is Nothing Then
            Dim GenTs As atcData.atcTimeseries = GetDataSetFromDsn(lId, aDsn)
            GenTs.Dates.EnsureValuesRead()
            pWDMObj(lId).DataSets.Remove(GenTs)
        End If
    End Sub

    Public Sub ClearAllOutputDsns()
        For Each lConnection As HspfConnection In pConnections
            If lConnection.Typ = 4 Then
                If lConnection.Target.VolName.Substring(0, 3) = "WDM" Then 'clear this dsn
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

        Dim lWDMFile As atcWDM.atcDataSourceWDM = Nothing
        lWDMFile = atcDataManager.DataSourceBySpecification(IO.Path.GetFullPath(aName))
        If lWDMFile Is Nothing Then
            lWDMFile = New atcWDM.atcDataSourceWDM
            If Not lWDMFile.Open(aName) Then 'had a problem
                Logger.Msg("Could not open WDM file" & vbCr & aName, MsgBoxStyle.Exclamation, "AddWDMFile Failed")
                lWDMFile = Nothing
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
        'used after editing files block to open wdm files
        On Error GoTo x

        FilesOK = True

        pWdmCount = 0
        For i = 1 To pFilesBlk.Count
            lHFile = pFilesBlk.Value(i)
            If Len(lHFile.Typ) > 2 Then
                If lHFile.Typ.StartsWith("WDM") Then
                    'see if this wdm is already in project
                    ifound = False
                    If ifound = False And pWdmCount < 4 Then 'add it to project
                        lFile = AddWDMFile(lHFile.Name.Trim)
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
        If Not pWDMObj(lWdmInd) Is Nothing Then
            For Each lDataSet As atcData.atcTimeseries In pWDMObj(lWdmInd).DataSets
                If lDsn = lDataSet.Attributes.GetValue("ID") Then
                    Return lDataSet
                End If
            Next
        End If
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

        For lWdmIndex As Integer = 0 To 4
            If Not pWDMObj(lWdmIndex) Is Nothing Then
                For Each lTser As atcData.atcTimeseries In pWDMObj(lWdmIndex).DataSets   'TODO: upgrade to use pTserFiles everywhere
                    With lTser.Attributes
                        If (aScenario = .GetValue("Scenario") _
                          Or aScenario.Trim.Length = 0) And (aLocation = .GetValue("Location") _
                          Or aLocation.Trim.Length = 0) And (aConstituent = .GetValue("Constituent") _
                          Or aConstituent.Trim.Length = 0) Then 'need this timser
                            lFindTimser.Add(lTser)
                        End If
                    End With
                Next
            End If
        Next
        Return lFindTimser
    End Function

    Public Function WeightedSourceArea(ByVal aOperation As HspfOperation, _
                                       ByVal aSourceType As String, _
                                       ByRef aSourceCollection As atcCollection, _
                                       ByRef aOriginalArea As Double) As Double
        If aSourceCollection Is Nothing Then
            aSourceCollection = New atcCollection
        End If
        Dim lAreaWeighted As Double = LocalWeightedSource(aSourceType, aOperation, aSourceCollection, aOriginalArea)
        Logger.Dbg("Weight" & aOperation.Name & " " & aOperation.Id & " " & lAreaWeighted & " OriginalArea " & aOriginalArea)
        For Each lOperationUp As HspfOperation In FindUpstreamOpns(aOperation)
            lAreaWeighted += WeightedSourceArea(lOperationUp, aSourceType, aSourceCollection, aOriginalArea)
        Next
        Return lAreaWeighted
    End Function

    Private Function LocalWeightedSource(ByVal aSourceType As String, _
                                         ByVal aOperation As HspfOperation, _
                                         ByVal aSourceCollection As atcCollection, _
                                         ByRef aOriginalAreaTotal As Double) As Double
        Dim lAreaWeightedTotal As Double = 0.0
        For Each lConnection As HspfConnection In aOperation.Sources
            If lConnection.Source.VolName = "PERLND" Or _
               lConnection.Source.VolName = "IMPLND" Then
                Dim lAreaOriginal As Double = lConnection.MFact
                For Each lMetSegRec As atcUCI.HspfMetSegRecord In lConnection.Source.Opn.MetSeg.MetSegRecs
                    If lMetSegRec.Name = aSourceType Then
                        With lMetSegRec
                            aOriginalAreaTotal += lAreaOriginal
                            Dim lAreaWeighted As Double = lAreaOriginal * .MFactP
                            lAreaWeightedTotal += lAreaWeighted
                            Dim lKey As Integer = .Source.VolId
                            aSourceCollection.Increment(lKey, lAreaWeighted)
                            Logger.Dbg("Key " & lKey & " " & lConnection.Target.VolName & lConnection.Target.VolId & _
                                       " AreaWeighted " & lAreaWeighted & _
                                       " MFact " & .MFactP & _
                                       " AreaWeightedTotal " & lAreaWeightedTotal & _
                                       " OriginalArea " & lAreaOriginal & _
                                       " OriginalAreaTotal " & aOriginalAreaTotal)
                        End With
                    End If
                Next
            End If
        Next lConnection
        Return lAreaWeightedTotal
    End Function

    Public Function UpstreamArea(ByRef aOperation As HspfOperation) As Double
        Dim lTotalArea As Double = LocalUpstreamArea(aOperation)
        For Each lOperationUp As HspfOperation In FindUpstreamOpns(aOperation)
            lTotalArea += UpstreamArea(lOperationUp)
        Next
        Return lTotalArea
    End Function

    Public Function LocalUpstreamArea(ByRef aOperation As HspfOperation) As Double
        Dim lUpArea As Double = 0.0
        For Each lConnection As HspfConnection In aOperation.Sources
            If lConnection.Source.VolName = "PERLND" Or _
               lConnection.Source.VolName = "IMPLND" Then
                lUpArea += lConnection.MFact
            End If
        Next lConnection
        Return lUpArea
    End Function

    Private Function FindUpstreamOpns(ByRef aOperation As HspfOperation, _
                                      Optional ByVal aAllByRecursion As Boolean = False) As Collection(Of HspfOperation)
        Dim lOperations As New Collection(Of HspfOperation)
        For Each lConnection As HspfConnection In aOperation.Sources
            If lConnection.Source.VolName = "RCHRES" Or _
               lConnection.Source.VolName = "BMPRAC" Then
                'add the source operation to the collection
                lOperations.Add(lConnection.Source.Opn)
                If aAllByRecursion Then
                    For Each lOperation As HspfOperation In FindUpstreamOpns(lConnection.Source.Opn, True)
                        lOperations.Add(lOperation)
                    Next
                End If
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
        For lSourceIndex As Integer = 0 To aOpn.Sources.Count - 1
            Dim lSourceConnection As HspfConnection = aOpn.Sources.Item(lSourceIndex)
            If lSourceConnection.Source.VolName = "PERLND" Or _
               lSourceConnection.Source.VolName = "IMPLND" Then 'copy this record
                'does this oper to copy already exist?
                Dim lCopyOpn As HspfOperation = pOpnBlks.Item("COPY").OperFromID(aCopyId)
                Dim lCopyOpnMatchIndex As Integer = 0
                Dim jConn As HspfConnection
                For lCopyOpnSourceIndex As Integer = 0 To lCopyOpn.Sources.Count - 1
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
                    lConn.Uci = Me
                    lConn.Source.VolName = lSourceConnection.Source.VolName
                    lConn.Source.VolId = lSourceConnection.Source.VolId
                    lConn.Source.Opn = lSourceConnection.Source.Opn
                    lConn.Typ = lSourceConnection.Typ
                    lConn.MFact = lSourceConnection.MFact
                    lConn.Target.VolName = "COPY"
                    lConn.Target.VolId = aCopyId
                    lConn.Target.Opn = lCopyOpn
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
                .SetValue("Data Source", pWDMObj(aWdmid).Specification)
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

    'Public Sub GetWDMUnits(ByRef aWdmCount As Integer, ByRef aWdmUnits() As Integer)
    '    aWdmCount = 0
    '    For lWdmIndex As Integer = 1 To 4
    '        If Not pWDMObj(lWdmIndex) Is Nothing Then 'add
    '            aWdmCount += 1
    '            ReDim Preserve aWdmUnits(aWdmCount)
    '            aWdmUnits(aWdmCount) = pWdmUnit(lWdmIndex)
    '        End If
    '    Next lWdmIndex
    'End Sub

    'Public Sub GetWDMIDFromUnit(ByVal aWdmUnit As Integer, ByRef aWdmId As String)
    '    aWdmId = ""
    '    For lWdmIndex As Integer = 1 To 4
    '        If Not pWDMObj(lWdmIndex) Is Nothing Then
    '            If pWdmUnit(lWdmIndex) = aWdmUnit Then
    '                aWdmId = "WDM" & lWdmIndex.ToString
    '                Exit For
    '            End If
    '        End If
    '    Next lWdmIndex
    'End Sub

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
        'If pIPCset Then
        '    Dim lString As String = ""
        '    Do  'process messages from parent
        '        lString = pIPC.GetProcessMessage("HSPFUCI") 'pHspfEngine.ReadTokenFromPipe(IPC.ParentRead, pipeBuffer, False)
        '        If lString.Length > 3 Then
        '            Select Case (LCase(Left(lString, 3)))
        '                Case "dbg", "msg" ', "com", "act"
        '                    pIPC.SendMonitorMessage(lString)
        '                    lString = ""
        '            End Select
        '        End If
        '    Loop While lString.Length = 0
        '    Return lString
        'Else
        Return "No process available"
        'End If
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
            GetNextRecordFromBlock("FTABLES", lReturnKey, lBuff, lRecordType, lReturnCode)
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
                    lRecordType = -999
                    Do Until lRecordType = 0
                        GetNextRecordFromBlock("FTABLES", lReturnKey, lBuff, lRecordType, lReturnCode)
                    Loop
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
                            GetNextRecordFromBlock("FTABLES", lReturnKey, lBuff, lRecordType, lReturnCode)

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
                                   ByVal aWQConstituents() As String, _
                                   Optional ByRef aPollutantListFileName As String = "", _
                                   Optional ByRef aMetBaseDsn As Integer = 11, _
                                   Optional ByVal aMetWdmId As String = "WDM2")

        'get starter uci ready for use defaulting parameters and mass links
        Dim lDefUci As New HspfUci
        lDefUci.FastReadUciForStarter(Me.Msg, aStarterUciName)

        modCreateUci.CreateUciFromBASINS(aWatershed, Me, aDataSources, _
                                         lDefUci, _
                                         aPollutantListFileName, aMetBaseDsn, aMetWdmId)

        'add specified pollutants
        If aWQConstituents.Length > 0 Then
            If lDefUci.Pollutants.Count = 0 Then
                ReadPollutants(lDefUci)
            End If
            For lDefIndex As Integer = 0 To lDefUci.Pollutants.Count - 1
                For Each lCons As String In aWQConstituents
                    If lDefUci.Pollutants(lDefIndex).Name = lCons Then
                        Dim lPoll As HspfPollutant = lDefUci.Pollutants(lDefIndex)
                        Me.Pollutants.Add(lPoll)
                    End If
                Next
            Next
            PollutantsUnBuild()
        End If

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
        Dim lTable As atcTableDelimited = AreaTable()
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

    Public Function AreaTable() As atcTableDelimited
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
                            If .FieldName(lFieldIndex).Substring(2) = lConnection.Source.VolId And _
                               ((.FieldName(lFieldIndex).StartsWith("P") And lConnection.Source.VolName = "PERLND") Or _
                                (.FieldName(lFieldIndex).StartsWith("I") And lConnection.Source.VolName = "IMPLND")) Then
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
        Return lTable
    End Function

    Public Sub SetDefault(ByVal aDefaultUci As HspfUci)
        Dim lOpTypNames() As String = {"PERLND", "IMPLND", "RCHRES"}
        For Each lOpTypName As String In lOpTypNames
            If Me.OpnBlks(lOpTypName).Count > 0 Then
                Dim lOpTyp As HspfOpnBlk = Me.OpnBlks(lOpTypName)
                'Logger.Dbg lOpTyp.Name
                For Each lOperation As HspfOperation In lOpTyp.Ids
                    'Logger.Dbg lOpn.Description
                    Dim lOperationDefault As HspfOperation = MatchOperWithDefault(lOperation.Name, lOperation.Description, aDefaultUci)
                    If Not lOperationDefault Is Nothing Then
                        Logger.Dbg("Match " & lOperation.Id & ":" & lOperationDefault.Id & " " & lOperation.Description & ":" & lOperationDefault.Description)
                        For Each lTable As HspfTable In lOperation.Tables
                            If DefaultThisTable(lOpTyp.Name, lTable.Name) Then
                                If lOperationDefault.TableExists(lTable.Name) Then
                                    Dim lTableDefault As HspfTable = lOperationDefault.Tables(lTable.Name)
                                    'Logger.Dbg lTab.Name
                                    For Each lParm As HspfParm In lTable.Parms
                                        If DefaultThisParameter(lOpTyp.Name, lTable.Name, lParm.Name) Then
                                            If lParm.Value <> lParm.Name Then
                                                lParm.Value = lTableDefault.Parms(lParm.Name).Value
                                            End If
                                        End If
                                    Next lParm
                                End If
                            End If
                        Next lTable
                    End If
                Next lOperation
            End If
        Next lOpTypName
    End Sub

    Private Function DefaultThisTable(ByVal aOperationName As String, ByVal aTableName As String) As Boolean
        Dim lDefaultThisTable As Boolean
        If aOperationName = "PERLND" Or aOperationName = "IMPLND" Then
            If aTableName = "ACTIVITY" Or _
               aTableName = "PRINT-INFO" Or _
               aTableName = "GEN-INFO" Or _
               aTableName = "PWAT-PARM5" Then
                lDefaultThisTable = False
            ElseIf aTableName.StartsWith("QUAL") Then
                lDefaultThisTable = False
            Else
                lDefaultThisTable = True
            End If
        ElseIf aOperationName = "RCHRES" Then
            If aTableName = "ACTIVITY" Or _
               aTableName = "PRINT-INFO" Or _
               aTableName = "GEN-INFO" Or _
               aTableName = "HYDR-PARM1" Then
                lDefaultThisTable = False
            ElseIf aTableName.StartsWith("GQ-") Then
                lDefaultThisTable = False
            Else
                lDefaultThisTable = True
            End If
        Else
            lDefaultThisTable = False
        End If
        Return lDefaultThisTable
    End Function

    Private Function DefaultThisParameter(ByVal aOperationName As String, _
                                          ByVal aTableName As String, _
                                          ByVal aParmName As String) As Boolean
        Dim lDefaultThisParameter As Boolean = True
        If aOperationName = "PERLND" Then
            If aTableName = "PWAT-PARM2" Then
                If aParmName = "SLSUR" Or aParmName = "LSUR" Then
                    lDefaultThisParameter = False
                End If
            ElseIf aTableName = "NQUALS" Then
                If aParmName = "NQUAL" Then
                    lDefaultThisParameter = False
                End If
            End If
        ElseIf aOperationName = "IMPLND" Then
            If aTableName = "IWAT-PARM2" Then
                If aParmName = "SLSUR" Or aParmName = "LSUR" Then
                    lDefaultThisParameter = False
                End If
            ElseIf aTableName = "NQUALS" Then
                If aParmName = "NQUAL" Then
                    lDefaultThisParameter = False
                End If
            End If
        ElseIf aOperationName = "RCHRES" Then
            If aTableName = "HYDR-PARM2" Then
                If aParmName = "LEN" Or _
                   aParmName = "DELTH" Or _
                   aParmName = "FTBUCI" Then
                    lDefaultThisParameter = False
                End If
            ElseIf aTableName = "HYDR-INIT" Then
                If aParmName = "VOL" Then
                    lDefaultThisParameter = False
                End If
            ElseIf aTableName = "GQ-GENDATA" Then
                If aParmName = "NGQUAL" Then
                    lDefaultThisParameter = False
                End If
            End If
        End If
        Return lDefaultThisParameter
    End Function

    Private Function MatchOperWithDefault(ByVal aOpTypName As String, _
                                          ByVal aDescriptionDefault As String, _
                                          ByVal aUciDefault As HspfUci) _
                                          As HspfOperation
        Dim lOperationMatch As HspfOperation = Nothing

        For Each lOperation As HspfOperation In aUciDefault.OpnBlks(aOpTypName).Ids
            If lOperation.Description = aDescriptionDefault Then
                lOperationMatch = lOperation
                Exit For
            End If
        Next lOperation

        If lOperationMatch Is Nothing Then
            'a complete match not found, look for partial
            For Each lOperation As HspfOperation In aUciDefault.OpnBlks(aOpTypName).Ids
                If lOperation.Description.StartsWith(aDescriptionDefault) Then
                    lOperationMatch = lOperation
                    Exit For
                ElseIf aDescriptionDefault.StartsWith(lOperation.Description) Then
                    lOperationMatch = lOperation
                    Exit For
                ElseIf aDescriptionDefault.Length > 3 Then
                    If lOperation.Description.StartsWith(aDescriptionDefault.Substring(0, 4)) Then
                        lOperationMatch = lOperation
                        Exit For
                    End If
                End If
            Next lOperation
        End If

        If lOperationMatch Is Nothing Then
            'not found, use first one if avaluable
            If aUciDefault.OpnBlks(aOpTypName).Count > 0 Then
                lOperationMatch = aUciDefault.OpnBlks(aOpTypName).Ids(0)
            End If
        End If
        Return lOperationMatch
    End Function

    Public Sub ReadPollutants(ByVal aDefUCI As HspfUci)

        Dim lPollutantFileName As String = PathNameOnly(aDefUCI.Name) & "\pollutants.txt"
        If Not FileExists(lPollutantFileName) Then
            lPollutantFileName = FindFile("Please locate pollutants.txt", "pollutants.txt")
        End If

        Dim lRecords As New Collection
        If FileExists(lPollutantFileName) Then
            For Each lRecord As String In LinesInFile(lPollutantFileName)
                lRecords.Add(lRecord)
            Next
        End If

        Dim lCurrentIndex As Integer = 1
        Dim lCurrentRecord As String = ""
        Do While lCurrentIndex < lRecords.Count
            lCurrentRecord = lRecords(lCurrentIndex)
            If lCurrentRecord.StartsWith("CONSTIT") Then

                'found start of a constituent
                Dim lPoll As New HspfPollutant
                Dim lTemp As String = StrRetRem(lCurrentRecord)
                'lCcons = lcurrentrecord
                lPoll.Name = lCurrentRecord

                Dim lPtype As Integer = 0
                Dim lItype As Integer = 0
                Dim lRtype As Integer = 0
                Dim lFoundConstituentEnd As Boolean = False

                Do While Not lFoundConstituentEnd
                    lCurrentIndex += 1
                    lCurrentRecord = lRecords(lCurrentIndex)
                    If lCurrentRecord.StartsWith("END CONSTIT") Then
                        'this is the end of the constituent
                        lFoundConstituentEnd = True
                        lPoll.Id = aDefUCI.Pollutants.Count + 1
                        lPoll.Index = Me.Pollutants.Count + 1
                        If lPtype = 1 And lRtype = 1 Then
                            lPoll.ModelType = "PIG"
                        ElseIf lPtype = 1 Then
                            lPoll.ModelType = "PIOnly"
                        ElseIf lRtype = 1 Then
                            lPoll.ModelType = "GOnly"
                        Else
                            lPoll.ModelType = "Data"
                        End If
                        'see if we already have this constituent in the uci or defuci
                        Dim lFoundThisConstituentAlready As Boolean = False
                        For Each lTempPoll As HspfPollutant In Me.Pollutants
                            If lTempPoll.Name = lPoll.Name Then
                                lFoundThisConstituentAlready = True
                            End If
                        Next
                        For Each lTempPoll As HspfPollutant In aDefUCI.Pollutants
                            If lTempPoll.Name = lPoll.Name Then
                                lFoundThisConstituentAlready = True
                            End If
                        Next
                        If Not lFoundThisConstituentAlready Then
                            'add this constituent to the defuci
                            aDefUCI.Pollutants.Add(lPoll)
                        End If
                        lPoll = Nothing
                    ElseIf lCurrentRecord.StartsWith("PERLND") Or lCurrentRecord.StartsWith("IMPLND") Or lCurrentRecord.StartsWith("RCHRES") Then
                        'found start of an operation
                        Dim lOpnBlk As New HspfOpnBlk
                        Dim lOpTyp As String = Trim(Mid(lCurrentRecord, 1, 6))
                        lOpnBlk.Name = lOpTyp
                        lOpnBlk.Uci = aDefUCI
                        For Each lOper As HspfOperation In Me.OpnBlks(lOpTyp).Ids
                            lOpnBlk.Ids.Add(lOper)
                            Dim lTempOper As New HspfOperation
                            lTempOper.Name = lOper.Name
                            lTempOper.Id = lOper.Id
                            lTempOper.Description = lOper.Description
                            lTempOper.DefOpnId = DefaultOpnId(lTempOper, aDefUCI)
                            lTempOper.OpnBlk = lOpnBlk
                            lPoll.Operations.Add(lOpTyp & lTempOper.Id, lTempOper)
                        Next
                        Dim lEndofOperation As Boolean = False
                        Do While Not lEndofOperation
                            lCurrentIndex += 1
                            lCurrentRecord = lRecords(lCurrentIndex)
                            If lCurrentRecord.StartsWith("END " & lOpTyp) Then
                                'found end of operation
                                lEndofOperation = True
                            ElseIf lCurrentRecord.Trim.Length > 0 Then
                                'found start of table
                                Dim lTableName As String = RTrim(Mid(lCurrentRecord, 3))
                                Dim lEndofTable As Boolean = False
                                Do While Not lEndofTable
                                    lCurrentIndex += 1
                                    lCurrentRecord = lRecords(lCurrentIndex)
                                    If lCurrentRecord.Trim.Length > 0 Then
                                        If lCurrentRecord.StartsWith("  END " & lTableName) Then
                                            'found end of table
                                            lEndofTable = True
                                        Else
                                            If InStr(1, lCurrentRecord, "***") Then
                                                'comment, ignore
                                            Else
                                                'found line of table
                                                Dim lOpf As Integer = CInt(Mid(lCurrentRecord, 1, 5))
                                                Dim lOpl As Integer
                                                If Trim(Mid(lCurrentRecord, 6, 5)).Length = 0 Then
                                                    lOpl = lOpf
                                                Else
                                                    lOpl = CInt(Mid(lCurrentRecord, 6, 5))
                                                End If
                                                For Each lOper As Generic.KeyValuePair(Of String, HspfOperation) In lPoll.Operations
                                                    If lOper.Value.Name = lOpTyp Then
                                                        lOper.Value.DefOpnId = DefaultOpnId(lOper.Value, aDefUCI)
                                                        If lOpf = lOper.Value.DefOpnId Or (lOpf <= lOper.Value.DefOpnId And lOper.Value.DefOpnId <= lOpl) Then
                                                            Dim lTable As New HspfTable
                                                            lTable.Def = Me.Msg.BlockDefs(lOpTyp).TableDefs(lTableName)
                                                            lTable.Opn = lOper.Value
                                                            lTable.InitTable(lCurrentRecord)
                                                            If lTable.Name = "GQ-QALDATA" Then
                                                                lRtype = 1
                                                            ElseIf lTable.Name = "QUAL-PROPS" Then
                                                                lPtype = 1
                                                                lItype = 1
                                                            End If
                                                            lTable.OccurCount = 1
                                                            lTable.OccurNum = 1
                                                            lTable.OccurIndex = 0
                                                            If Not lOper.Value.TableExists(lTable.Name) Then
                                                                lOper.Value.Tables.Add(lTable)
                                                                If Not lPoll.TableExists(lTable.Name) Then
                                                                    lPoll.Tables.Add(lTable.Name, lTable)
                                                                End If
                                                            Else
                                                                'handle multiple occurs of this table
                                                                Dim ltempTable As HspfTable = lOper.Value.Tables(lTable.Name)
                                                                Dim lNOccurance As Integer = ltempTable.OccurCount + 1
                                                                Dim lTempName As String = ""
                                                                ltempTable.OccurCount = lNOccurance
                                                                For lTableIndex As Integer = 2 To lNOccurance - 1
                                                                    lTempName = lTable.Name & ":" & CStr(lTableIndex)
                                                                    ltempTable = lOper.Value.Tables(lTempName)
                                                                    ltempTable.OccurCount = lNOccurance
                                                                Next
                                                                lTable.OccurCount = lNOccurance
                                                                lTable.OccurNum = lNOccurance
                                                                lTempName = lTable.Name & ":" & CStr(lNOccurance)
                                                                lOper.Value.Tables.Add(lTable)
                                                                If Not lPoll.TableExists(lTempName) Then
                                                                    lPoll.Tables.Add(lTempName, lTable)
                                                                End If
                                                            End If
                                                        End If
                                                    End If
                                                Next
                                            End If
                                        End If
                                    End If
                                Loop
                            End If
                        Loop

                    ElseIf lCurrentRecord.StartsWith("MASS-LINKS") Then
                        Dim lFoundEndofMassLinks As Boolean = False
                        Do While Not lFoundEndofMassLinks
                            lCurrentIndex += 1
                            lCurrentRecord = lRecords(lCurrentIndex)
                            If lCurrentRecord.StartsWith("END MASS-LINKS") Then
                                'found end of masslinks
                                lFoundEndofMassLinks = True
                            ElseIf lCurrentRecord.Trim.Length > 0 Then
                                'found a masslink
                                Dim lML As New HspfMassLink
                                lML.Uci = aDefUCI
                                lML.Source.VolName = Trim(Mid(lCurrentRecord, 1, 6))
                                lML.Source.Group = Trim(Mid(lCurrentRecord, 12, 6))
                                lML.Source.Member = Trim(Mid(lCurrentRecord, 19, 6))
                                Dim lIstr As String = Trim(Mid(lCurrentRecord, 26, 1))
                                If Len(lIstr) = 0 Then
                                    lML.Source.MemSub1 = 0
                                Else
                                    lML.Source.MemSub1 = CInt(lIstr)
                                End If
                                lIstr = Trim(Mid(lCurrentRecord, 28, 1))
                                If Len(lIstr) = 0 Then
                                    lML.Source.MemSub2 = 0
                                Else
                                    lML.Source.MemSub2 = CInt(lIstr)
                                End If
                                lIstr = Trim(Mid(lCurrentRecord, 30, 10))
                                If Len(lIstr) = 0 Then
                                    lML.MFact = 1
                                Else
                                    lML.MFact = lIstr
                                End If
                                lML.Target.VolName = Trim(Mid(lCurrentRecord, 44, 6))
                                lML.Target.Group = Trim(Mid(lCurrentRecord, 59, 6))
                                lML.Target.Member = Trim(Mid(lCurrentRecord, 66, 6))
                                lIstr = Trim(Mid(lCurrentRecord, 73, 1))
                                If Len(lIstr) = 0 Then
                                    lML.Target.MemSub1 = 0
                                Else
                                    lML.Target.MemSub1 = CInt(lIstr)
                                End If
                                lIstr = Trim(Mid(lCurrentRecord, 75, 1))
                                If Len(lIstr) = 0 Then
                                    lML.Target.MemSub2 = 0
                                Else
                                    lML.Target.MemSub2 = CInt(lIstr)
                                End If
                                lML.MassLinkId = Me.MassLinks(1).FindMassLinkID(lML.Source.VolName, lML.Target.VolName)
                                lPoll.MassLinks.Add(lML)
                            End If
                        Loop
                    End If
                Loop
            End If

            lCurrentIndex += 1
        Loop

    End Sub

    Public Function DefaultOpnId(ByVal aOpn As HspfOperation, ByVal aDefUCI As HspfUci) As Long

        If aOpn.DefOpnId <> 0 Then
            DefaultOpnId = aOpn.DefOpnId
        Else
            Dim lDOpn As HspfOperation = MatchOperWithDefault(aOpn.Name, aOpn.Description, aDefUCI)
            If lDOpn Is Nothing Then
                DefaultOpnId = 0
            Else
                DefaultOpnId = lDOpn.Id
            End If
        End If

    End Function
End Class
