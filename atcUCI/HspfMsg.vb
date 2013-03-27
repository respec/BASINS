Option Strict Off
Option Explicit On

Imports System.Collections.ObjectModel
Imports MapWinUtility

'Copyright 2006-8 AQUA TERRA Consultants - Royalty-free use permitted under open source license
Public Class HspfMsg
    Private pErrorDescription As String

    Public Name As String

    Private pBlockDefs As HspfBlockDefs
    Public ReadOnly Property BlockDefs() As HspfBlockDefs
        Get
            Return pBlockDefs
        End Get
    End Property

    Private pTSGroupDefs As HspfTSGroupDefs
    Public ReadOnly Property TSGroupDefs() As HspfTSGroupDefs
        Get
            Return pTSGroupDefs
        End Get
    End Property

    Public ReadOnly Property ErrorDescription() As String
        Get
            ErrorDescription = pErrorDescription
            pErrorDescription = ""
        End Get
    End Property

    'Public WriteOnly Property Monitor() As Object
    '    Set(ByVal Value As Object)
    '        IPC = Value
    '        If IPC Is Nothing Then IPCset = False Else IPCset = True
    '    End Set
    'End Property

    Public Sub Open(ByVal aFilename As String)
        If IO.Path.GetExtension(aFilename) = ".wdm" Then
            OpenFromWDM(aFilename)
        Else
            OpenFromMDB(aFilename)
        End If
    End Sub

    Private Sub OpenFromMDB(ByVal aFilename As String)
        If aFilename.Length = 0 Then
            aFilename = "HSPFmsg.mdb"
        End If
        If Not IO.File.Exists(aFilename) Then
            aFilename = GetSetting("HSPF", "MessageMDB", "Path")
            If Not IO.File.Exists(aFilename) Then
                aFilename = atcUtility.FindFile("Please locate 'HSPFmsg.mdb' in a writable directory", "HSPFmsg.mdb")
                SaveSetting("HSPF", "MessageMDB", "Path", aFilename)
            End If
        End If

        Logger.Dbg("Opening " & aFilename)
        Dim lMsgMDb As New atcUtility.atcMDB(aFilename)
        Name = aFilename

        'Logger.Dbg("BlockDefns")
        Dim lBlkTable As DataTable = lMsgMDb.GetTable("BlockDefns")
        Dim lBlock As HspfBlockDef
        Dim lBlockFieldID As Integer = lBlkTable.Columns.IndexOf("ID")
        Dim lBlockFieldName As Integer = lBlkTable.Columns.IndexOf("Name")
        Dim lSections As New HspfSectionDefs

        'Logger.Dbg("SectionDefns")
        Dim lSecTable As DataTable = lMsgMDb.GetTable("SectionDefns")
        Dim lSection As HspfSectionDef
        Dim lSectionFieldID As Integer = lSecTable.Columns.IndexOf("ID")
        Dim lSectionFieldName As Integer = lSecTable.Columns.IndexOf("Name")
        Dim lSectionFieldBlockID As Integer = lSecTable.Columns.IndexOf("BlockID")
        Dim lCriticalSection As String
        Dim lTable As HspfTableDef
        Dim lBlkTableDefs As New HspfTableDefs

        Dim lTabTable As DataTable = lMsgMDb.GetTable("TableDefns")
        Dim lTableFieldSectionID As Integer = lTabTable.Columns.IndexOf("SectionID")
        Dim lTableDefs As HspfTableDefs
        Dim lParms As New HspfParmDefs

        Dim lParmTable As DataTable = lMsgMDb.GetTable("ParmDefns")
        Dim lParmFieldTableID As Integer = lParmTable.Columns.IndexOf("TableID")
        Dim lParm As HSPFParmDef
        Dim lTyp As String

        Dim lTSGroupTable As DataTable = lMsgMDb.GetTable("TSGroupDefns")
        Dim lTSGroup As HspfTSGroupDef
        Dim lTSMembers As Collection(Of HspfTSMemberDef)
        Dim lTSMemberTable As DataTable = lMsgMDb.GetTable("TSMemberDefns")
        Dim lMemberFieldTSGroupID As Integer = lTSMemberTable.Columns.IndexOf("TSGroupID")
        Dim lTSMember As HspfTSMemberDef

        Dim lNumeric As Boolean

        Dim lBlkCount As Integer = lBlkTable.Rows.Count
        Dim lBlkNow As Integer

        pBlockDefs = Nothing
        pBlockDefs = New HspfBlockDefs

        lBlkNow = 0
        For Each lBlockRow As DataRow In lBlkTable.Rows
            'progress bar (dumb)
            's = "(Progress " & lBlkNow * 100 / lBlkCount & ")"
            'IPC.SendMonitorMessage s
            lBlkNow = lBlkNow + 1
            'Logger.Dbg("Block Row " & lBlkNow)

            lBlock = New HspfBlockDef
            lBlock.Id = lBlockRow.Item(lBlockFieldID)
            lBlock.Name = lBlockRow.Item(lBlockFieldName)
            lSections = Nothing
            lSections = New HspfSectionDefs
            lBlkTableDefs = Nothing
            lBlkTableDefs = New HspfTableDefs
            lCriticalSection = "BlockID = " & CStr(lBlock.Id)
            For Each lSecRow As DataRow In lSecTable.Rows
                If lSecRow.Item(lSectionFieldBlockID) = lBlock.Id Then
                    lSection = New HspfSectionDef
                    lSection.Name = lSecRow.Item(lSectionFieldName)
                    lSection.Id = lSecRow.Item(lSectionFieldID)
                    'Logger.Dbg("Section Row " & lSection.Name)
                    lTableDefs = Nothing
                    lTableDefs = New HspfTableDefs

                    'lTabTable = myDb.GetTable("TableDefns WHERE SectionID = " & CStr(lSection.Id))
                    For Each lTabRow As DataRow In lTabTable.Rows
                        If lTabRow.Item(lTableFieldSectionID) = lSection.Id Then
                            lTable = New HspfTableDef
                            lTable.Id = lTabRow.Item(0)
                            lTable.Parent = lSection
                            lTable.Name = lTabRow.Item(2)

                            'Logger.Dbg("Table Row " & ltable.Name)

                            lTable.SGRP = lTabRow.Item(3)
                            lTable.NumOccur = lTabRow.Item(4)
                            lTable.HeaderE = lTabRow.Item(5)
                            lTable.HeaderM = lTabRow.Item(6)
                            lTable.Define = FilterNull(lTabRow.Item(7), " ")
                            If lTabTable.Columns.Count < 9 Then
                                lTable.OccurGroup = 0
                            Else
                                lTable.OccurGroup = lTabRow.Item(8)
                            End If
                            lParms = Nothing
                            lParms = New HspfParmDefs

                            'lParmTable = myDb.GetTable("ParmDefns WHERE TableID = " & CStr(ltable.Id))
                            For Each lParmRow As DataRow In lParmTable.Rows
                                If lParmRow.Item(lParmFieldTableID) = lTable.Id Then
                                    lParm = New HSPFParmDef
                                    lParm.Name = lParmRow.Item(2) 'Name
                                    'Logger.Dbg("Parm Row " & lParm.Name)
                                    lTyp = lParmRow.Item(3) 'Type
                                    Select Case lTyp
                                        Case "I" : lNumeric = True : lParm.Typ = 1 ' ATCoInt
                                        Case "R" : lNumeric = True : lParm.Typ = 2 ' ATCoDbl
                                        Case "C" : lNumeric = False : lParm.Typ = 0 ' ATCoTxt
                                        Case Else : lNumeric = False : lParm.Typ = -999
                                    End Select
                                    lParm.StartCol = lParmRow.Item(4)
                                    lParm.Length = lParmRow.Item(5)
                                    If lNumeric Then
                                        lParm.Min = lParmRow.Item(6)
                                        lParm.Max = lParmRow.Item(7)
                                        If lParmTable.Columns.Count > 10 Then
                                            lParm.MetricMin = lParmRow.Item(10)
                                            lParm.MetricMax = lParmRow.Item(11)
                                        Else
                                            lParm.MetricMin = lParmRow.Item(6)
                                            lParm.MetricMax = lParmRow.Item(7)
                                        End If
                                    End If
                                    lParm.DefaultValue = lParmRow.Item(8) & " " 'default
                                    If lParmTable.Columns.Count > 10 Then
                                        lParm.MetricDefault = lParmRow.Item(12) & " " 'default
                                    Else 'use english default
                                        lParm.MetricDefault = lParmRow.Item(8) & " "
                                    End If
                                    lParm.Other = lParmRow.Item(4) & ":" & lParmRow.Item(5)
                                    lParm.Define = lParmRow.Item(9) & " "
                                    lParms.Add(lParm)
                                End If
                            Next
                            lTable.ParmDefs = lParms
                            UpdateParmsMultLines((lBlock.Name), lTable)
                            lTableDefs.Add(lTable)
                            lBlkTableDefs.Add(lTable)
                        End If
                    Next
                    lSection.TableDefs = lTableDefs
                    lSections.Add(lSection)
                End If
            Next
            lBlock.SectionDefs = lSections
            lBlock.TableDefs = lBlkTableDefs
            pBlockDefs.Add(lBlock)
        Next

        'Logger.Dbg("TSGroupDefns")
        'now read TS group and member info
        pTSGroupDefs = Nothing
        pTSGroupDefs = New HspfTSGroupDefs
        Dim lTSGroupFieldID As Integer = lTSGroupTable.Columns.IndexOf("ID")
        Dim lTSGroupFieldName As Integer = lTSGroupTable.Columns.IndexOf("Name")
        Dim lTSGroupFieldBlockID As Integer = lTSGroupTable.Columns.IndexOf("BlockID")

        Dim lTSMemberFieldID As Integer = lTSMemberTable.Columns.IndexOf("ID")
        Dim lTSMemberFieldName As Integer = lTSMemberTable.Columns.IndexOf("Name")
        Dim lTSMemberFieldTSGroupID As Integer = lTSMemberTable.Columns.IndexOf("TSGroupID")
        Dim lTSMemberFieldSCLU As Integer = lTSMemberTable.Columns.IndexOf("SCLU")
        Dim lTSMemberFieldSGRP As Integer = lTSMemberTable.Columns.IndexOf("SGRP")
        Dim lTSMemberFieldmdim1 As Integer = lTSMemberTable.Columns.IndexOf("mdim1")
        Dim lTSMemberFieldmdim2 As Integer = lTSMemberTable.Columns.IndexOf("mdim2")
        Dim lTSMemberFieldmaxsb1 As Integer = lTSMemberTable.Columns.IndexOf("maxsb1")
        Dim lTSMemberFieldmaxsb2 As Integer = lTSMemberTable.Columns.IndexOf("maxsb2")
        Dim lTSMemberFieldmkind As Integer = lTSMemberTable.Columns.IndexOf("mkind")
        Dim lTSMemberFieldsptrn As Integer = lTSMemberTable.Columns.IndexOf("sptrn")
        Dim lTSMemberFieldmsect As Integer = lTSMemberTable.Columns.IndexOf("msect")
        Dim lTSMemberFieldmio As Integer = lTSMemberTable.Columns.IndexOf("mio")
        Dim lTSMemberFieldosvbas As Integer = lTSMemberTable.Columns.IndexOf("osvbas")
        Dim lTSMemberFieldosvoff As Integer = lTSMemberTable.Columns.IndexOf("osvoff")
        Dim lTSMemberFieldeunits As Integer = lTSMemberTable.Columns.IndexOf("eunits")
        Dim lTSMemberFieldltval1 As Integer = lTSMemberTable.Columns.IndexOf("ltval1")
        Dim lTSMemberFieldltval2 As Integer = lTSMemberTable.Columns.IndexOf("ltval2")
        Dim lTSMemberFieldltval3 As Integer = lTSMemberTable.Columns.IndexOf("ltval3")
        Dim lTSMemberFieldltval4 As Integer = lTSMemberTable.Columns.IndexOf("ltval4")
        Dim lTSMemberFielddefn As Integer = lTSMemberTable.Columns.IndexOf("defn")
        Dim lTSMemberFieldmunits As Integer = lTSMemberTable.Columns.IndexOf("munits")
        Dim lTSMemberFieldltval5 As Integer = lTSMemberTable.Columns.IndexOf("ltval5")
        Dim lTSMemberFieldltval6 As Integer = lTSMemberTable.Columns.IndexOf("ltval6")
        Dim lTSMemberFieldltval7 As Integer = lTSMemberTable.Columns.IndexOf("ltval7")
        Dim lTSMemberFieldltval8 As Integer = lTSMemberTable.Columns.IndexOf("ltval8")

        For Each lTSGroupRow As DataRow In lTSGroupTable.Rows
            lTSGroup = New HspfTSGroupDef
            lTSGroup.Id = lTSGroupRow.Item(lTSGroupFieldID)
            lTSGroup.Name = lTSGroupRow.Item(lTSGroupFieldName)
            'If IPCset Then
            '    s = "(MSG3 Reading about Timeseries Groups and Members for " & lTSGroup.Name & ")"
            '    'IPC.SendMonitorMessage s
            'End If
            lTSGroup.BlockID = lTSGroupRow.Item(lTSGroupFieldBlockID)
            lTSMembers = Nothing
            lTSMembers = New Collection(Of HspfTSMemberDef)
            'lTSMemberTable = myDb.GetTable("TSMemberDefns WHERE TSGroupID = " & CStr(lTSGroup.Id))

            For Each lTSMemberRow As DataRow In lTSMemberTable.Rows
                If lTSMemberRow.Item(lMemberFieldTSGroupID) = lTSGroup.Id Then
                    lTSMember = New HspfTSMemberDef
                    lTSMember.Id = lTSMemberRow.Item(lTSMemberFieldID)
                    lTSMember.Name = lTSMemberRow.Item(lTSMemberFieldName)
                    lTSMember.TSGroupID = lTSMemberRow.Item(lTSMemberFieldTSGroupID)
                    lTSMember.Parent = lTSGroup
                    lTSMember.SCLU = lTSMemberRow.Item(lTSMemberFieldSCLU)
                    lTSMember.SGRP = lTSMemberRow.Item(lTSMemberFieldSGRP)
                    lTSMember.MDim1 = FilterNull(lTSMemberRow.Item(lTSMemberFieldmdim1))
                    lTSMember.MDim2 = FilterNull(lTSMemberRow.Item(lTSMemberFieldmdim2))
                    lTSMember.Maxsb1 = FilterNull(lTSMemberRow.Item(lTSMemberFieldmaxsb1))
                    lTSMember.Maxsb2 = FilterNull(lTSMemberRow.Item(lTSMemberFieldmaxsb2))
                    lTSMember.MKind = FilterNull(lTSMemberRow.Item(lTSMemberFieldmkind))
                    lTSMember.Sptrn = FilterNull(lTSMemberRow.Item(lTSMemberFieldsptrn))
                    lTSMember.Msect = FilterNull(lTSMemberRow.Item(lTSMemberFieldmsect))
                    lTSMember.Mio = FilterNull(lTSMemberRow.Item(lTSMemberFieldmio))
                    lTSMember.OsvBas = FilterNull(lTSMemberRow.Item(lTSMemberFieldosvbas))
                    lTSMember.OsvOff = FilterNull(lTSMemberRow.Item(lTSMemberFieldosvoff))
                    lTSMember.EUnits = FilterNull(lTSMemberRow.Item(lTSMemberFieldeunits), " ")
                    lTSMember.Ltval1 = FilterNull(lTSMemberRow.Item(lTSMemberFieldltval1))
                    lTSMember.Ltval2 = FilterNull(lTSMemberRow.Item(lTSMemberFieldltval2))
                    lTSMember.Ltval3 = FilterNull(lTSMemberRow.Item(lTSMemberFieldltval3))
                    lTSMember.Ltval4 = FilterNull(lTSMemberRow.Item(lTSMemberFieldltval4))
                    lTSMember.Defn = FilterNull(lTSMemberRow.Item(lTSMemberFielddefn), " ")
                    lTSMember.MUnits = FilterNull(lTSMemberRow.Item(lTSMemberFieldmunits), " ")
                    lTSMember.Ltval5 = FilterNull(lTSMemberRow.Item(lTSMemberFieldltval5))
                    lTSMember.Ltval6 = FilterNull(lTSMemberRow.Item(lTSMemberFieldltval6))
                    lTSMember.Ltval7 = FilterNull(lTSMemberRow.Item(lTSMemberFieldltval7))
                    lTSMember.Ltval8 = FilterNull(lTSMemberRow.Item(lTSMemberFieldltval8))
                    lTSMembers.Add(lTSMember)
                End If
            Next
            lTSGroup.MemberDefs = lTSMembers
            pTSGroupDefs.Add(lTSGroup)
        Next
        Logger.Dbg("HSPFMsg:Open Finished")
    End Sub

    Private Sub OpenFromWDM(ByVal aFilename As String)
        If aFilename.Length = 0 Then
            aFilename = "hspfmsg.wdm"
        End If
        If Not IO.File.Exists(aFilename) Then
            aFilename = GetSetting("HSPF", "MessageWDM", "Path")
            If Not IO.File.Exists(aFilename) Then
                aFilename = atcUtility.FindFile("Please locate 'hspfmsg.wdm' in a writable directory", "hspfmsg.wdm")
                SaveSetting("HSPF", "MessageWDM", "Path", aFilename)
            End If
        End If

        Logger.Dbg("HSPFMsg:Open Msg From WDM")
        Name = aFilename

        Dim lFmsg As Integer = F90_INQNAM(aFilename, Len(aFilename))
        Dim lNeedtoClose As Boolean = False
        If lFmsg = 0 Then
            lFmsg = F90_WDBOPN(1, aFilename, Len(aFilename))
            Call F90_MSGUNIT(lFmsg)
            lNeedtoClose = True
        End If
        'Call F90_W99OPN() 'open error file
        'Call F90_WDBFIN() 'initialize WDM record buffer
        'Call F90_PUTOLV(10)

        Dim lBlockInitFg As Integer = 1
        Dim lBlockCont As Integer = 1
        Dim lBlockSCLU As Integer = 201
        Dim lBlockSGRP As Integer = 22
        Dim lBlockOpcnt As Integer = 0
        Dim lBlockRetid As Integer = 0
        Dim lBlockOlen As Integer = 0
        Dim lBlockObuff As String = ""
        Dim lBlockTabnam As String
        Dim lBlockOmcode As Integer
        Dim lBlkNow As Integer = 0

        pBlockDefs = Nothing
        pBlockDefs = New HspfBlockDefs
        Do While lBlockCont <> 0
            'get each block and its id number
            lBlockRetid = 0
            lBlockOlen = 80
            Call F90_WMSGTT(lFmsg, lBlockSCLU, lBlockSGRP, lBlockInitFg, lBlockOlen, lBlockCont, lBlockObuff)
            lBlockTabnam = Mid(lBlockObuff, 1, 12)
            lBlockOmcode = CInt(Mid(lBlockObuff, 18, 3))
            If lBlockOmcode = 100 Then
                lBlockOpcnt = lBlockOpcnt + 1
                lBlockOmcode = 120 + lBlockOpcnt
            End If
            lBlockInitFg = 0

            Dim lBlock As New HspfBlockDef
            lBlock.Id = lBlockOmcode
            lBlock.Name = Trim(lBlockTabnam)
            pBlockDefs.Add(lBlock)
        Loop

        Dim lSections As New HspfSectionDefs
        For Each lBlock As HspfBlockDef In pBlockDefs
            lSections = Nothing
            lSections = New HspfSectionDefs
            Dim lSectionName As String = ""
            Dim lSectionBlockID As Integer = 0
            Dim lSectionID As Integer = 0

            If lBlock.Id = 121 Or lBlock.Id = 122 Or lBlock.Id = 123 Then
                Dim lSectionInitFg As Integer = 1
                Dim lSectionCont As Integer = 1
                Dim lSectionSCLU As Integer = lBlock.Id
                Dim lSectionSGRP As Integer = 2
                Dim lSectionRetid As Integer = 0
                Dim lSectionOlen As Integer = 0
                Dim lSectionObuff As String = ""
                Do While lSectionCont <> 0
                    'get each section and its id number
                    lSectionRetid = 0
                    lSectionOlen = 80
                    Call F90_WMSGTT(lFmsg, lSectionSCLU, lSectionSGRP, lSectionInitFg, lSectionOlen, lSectionCont, lSectionObuff)
                    'add each block name to the block definition table
                    'If Trim(Mid(obuff, 1, 6)) <> "ACIDPH" Then
                    lSectionName = Trim(Mid(lSectionObuff, 1, 8))
                    lSectionBlockID = lSectionSCLU
                    lSectionID = ((lBlock.Id - 120) * 100) + CInt(Mid(lSectionObuff, 10, 3))
                    'End If
                    lSectionInitFg = 0

                    Dim lSection As New HspfSectionDef
                    Dim lTables As New HspfTableDefs
                    lSection.Id = lSectionID
                    lSection.Name = lSectionName
                    lSection.TableDefs = lTables
                    lSections.Add(lSection)
                Loop
            Else
                'add dummy sections for blocks without sections
                If lBlock.Id < 121 Or lBlock.Id > 123 Then
                    'add each block name to the block definition table
                    lSectionName = "<NONE>"
                    lSectionBlockID = lBlock.Id
                    lSectionID = lBlock.Id

                    Dim lSection As New HspfSectionDef
                    Dim lTables As New HspfTableDefs
                    lSection.Id = lSectionID
                    lSection.Name = lSectionName
                    lSection.TableDefs = lTables
                    lSections.Add(lSection)
                End If
            End If

            lBlock.SectionDefs = lSections
        Next

        Dim lnflds As Integer
        Dim lscol(30) As Integer
        Dim lflen(30) As Integer
        Dim lftyp As String = ""
        Dim lapos(30) As Integer
        Dim limin(30) As Integer
        Dim limax(30) As Integer
        Dim lidef(30) As Integer
        Dim lrmin(30) As Single
        Dim lrmax(30) As Single
        Dim lrdef(30) As Single
        Dim limetmin(30) As Integer
        Dim limetmax(30) As Integer
        Dim limetdef(30) As Integer
        Dim lrmetmin(30) As Single
        Dim lrmetmax(30) As Single
        Dim lrmetdef(30) As Single
        Dim lnmhdr As Integer
        Dim lhdrbuf(10) As String
        Dim lfdnam(30) As String
        Dim lTableRetid As Integer = 0
        Dim ltabno As Integer = 1
        Dim luunits As Integer = 1
        Dim lAssoc As String = ""
        Dim lgptr As Integer
        Dim lfptr(64) As Integer
        Dim lisect As Integer
        Dim lirept As Integer
        Dim lheader As String
        Dim lcontfg As Integer
        Dim lInit As Integer
        Dim lkflg As Integer
        Dim lkwd As String = ""
        Dim ltabret As Integer
        Dim ladjLen As Integer
        Dim lnmhdrM As Integer
        Dim lhdrbufM(10) As String
        Dim ltyp As String = ""
        Dim lTableRecCount As Integer = 0
        Dim lParmRecCount As Integer = 0
        For Each lBlock As HspfBlockDef In pBlockDefs
            Dim lBlockTableDefs As New HspfTableDefs
            Dim lTableDefs As New HspfTableDefs

            'loop through each block

            If lBlock.Id > 2 Then
                'cant do for global block, get details about all others
                If lBlock.Id < 100 Then
                    'uci block (files,ext targs, etc)
                    Call F90_XTINFO(lBlock.Id, ltabno, 2, 0, lnflds, lscol, lflen, lftyp, lapos, limetmin, limetmax, limetdef, lrmetmin, lrmetmax, lrmetdef, lnmhdr, lhdrbuf, lfdnam, lisect, lirept, lTableRetid)
                    Call F90_XTINFO(lBlock.Id, ltabno, luunits, 0, lnflds, lscol, lflen, lftyp, lapos, limin, limax, lidef, lrmin, lrmax, lrdef, lnmhdr, lhdrbuf, lfdnam, lisect, lirept, lTableRetid)
                    Call F90_WMSGTW(CInt(1), lAssoc)
                    Call F90_WMSGTH(lgptr, lfptr(0))
                    If lTableRetid = 0 Then
                        lTableRecCount += 1
                        'got some info about this block
                        Dim lTable As New HspfTableDef
                        '!Name = Trim(tabnam(i - 1))
                        lTable.Name = "<NONE>"
                        lTable.Id = lTableRecCount
                        lTable.NumOccur = lirept
                        lTable.SGRP = 1
                        lheader = addComment(RTrim(lhdrbuf(0)), 0)
                        For j As Integer = 2 To lnmhdr
                            lheader = lheader & vbCrLf & addComment(RTrim(lhdrbuf(j - 1)), 0)
                        Next j
                        lTable.HeaderE = lheader
                        lTable.HeaderM = lheader
                        lTable.OccurGroup = 0
                        'lTable.Define = FilterNull(lTabRow.Item(7), " ")
                        'If lTabTable.Columns.Count < 9 Then
                        '    lTable.OccurGroup = 0
                        'Else
                        '    lTable.OccurGroup = lTabRow.Item(8)
                        'End If
                        Dim lParms As New HspfParmDefs
                        For j As Integer = 1 To lnflds
                            'loop through parameter fields
                            If Len(Trim(lfdnam(j - 1))) > 0 Then
                                'dont add fields without field names
                                Dim lParm As New HSPFParmDef
                                lParmRecCount += 1
                                lParm.Name = Trim(lfdnam(j - 1))
                                'lParm.id = lParmRecCount
                                'lParm.TableId = lTableRecCount
                                lParm.StartCol = lscol(j - 1) - 3
                                lParm.Length = lflen(j - 1)
                                ltyp = Mid(lftyp, j, 1)
                                Select Case ltyp
                                    Case "I" : lParm.Typ = 1 ' ATCoInt
                                    Case "R" : lParm.Typ = 2 ' ATCoDbl
                                    Case "C" : lParm.Typ = 0 ' ATCoTxt
                                    Case Else : lParm.Typ = -999
                                End Select
                                If lParm.Typ = 1 Then
                                    lParm.Min = limin(lapos(j - 1) - 1)
                                    lParm.Max = limax(lapos(j - 1) - 1)
                                    lParm.DefaultValue = lidef(lapos(j - 1) - 1) & " "
                                    lParm.MetricMin = limetmin(lapos(j - 1) - 1)
                                    lParm.MetricMax = limetmax(lapos(j - 1) - 1)
                                    lParm.MetricDefault = limetdef(lapos(j - 1) - 1) & " "
                                ElseIf lParm.Typ = 2 Then
                                    lParm.Min = lrmin(lapos(j - 1) - 1)
                                    lParm.Max = lrmax(lapos(j - 1) - 1)
                                    lParm.DefaultValue = lrdef(lapos(j - 1) - 1) & " "
                                    lParm.MetricMin = lrmetmin(lapos(j - 1) - 1)
                                    lParm.MetricMax = lrmetmax(lapos(j - 1) - 1)
                                    lParm.MetricDefault = lrmetdef(lapos(j - 1) - 1) & " "
                                Else
                                    lParm.DefaultValue = " "
                                    lParm.MetricDefault = " "
                                End If
                                lParms.Add(lParm)
                            End If
                        Next j
                        lTable.ParmDefs = lParms
                        lTableDefs.Add(lTable)
                        lBlockTableDefs.Add(lTable)
                        For Each lSection As HspfSectionDef In lBlock.SectionDefs
                            If lSection.Id = lBlock.Id Then
                                lSection.TableDefs.Add(lTable)
                                lTable.Parent = lSection
                            End If
                        Next
                    End If
                Else
                    'this is an operation type table
                    lInit = 1
                    lTableRetid = 0
                    ltabno = 1
                    Do While lTableRetid > -1
                        'loop through each operation table (perlnd, mutsin, etc)
                        Call F90_GTNXKW(lInit, lBlock.Id, lkwd, lkflg, lcontfg, ltabret)
                        lInit = 0
                        'do for metric table, then english
                        Call F90_XTINFO(lBlock.Id, ltabno, 2, 0, lnflds, lscol, lflen, lftyp, lapos, limetmin, limetmax, limetdef, lrmetmin, lrmetmax, lrmetdef, lnmhdrM, lhdrbufM, lfdnam, lisect, lirept, lTableRetid)
                        Call F90_XTINFO(lBlock.Id, ltabno, luunits, 0, lnflds, lscol, lflen, lftyp, lapos, limin, limax, lidef, lrmin, lrmax, lrdef, lnmhdr, lhdrbuf, lfdnam, lisect, lirept, lTableRetid)
                        Call F90_WMSGTW(CInt(1), lAssoc)
                        If lTableRetid = 0 Then
                            'got a table
                            lTableRecCount += 1
                            Dim lTable As New HspfTableDef
                            If lflen(0) = 8 Then
                                'need to add 2 characters to starting pos
                                ladjLen = 2
                            Else
                                ladjLen = 0
                            End If
                            'If Mid(kwd, 1, 5) <> "ACID-" Then
                            'need to update some table names that were truncated
                            lkwd = AddChar2Keyword(lkwd)
                            lTable.Name = Trim(lkwd)
                            lTable.Id = lTableRecCount
                            Dim lSectionID As Integer = 0
                            If lisect = 0 Then
                                lSectionID = lBlock.Id
                                lTable.OccurGroup = 0
                            ElseIf lisect > 20 Then
                                'a group of repeating tables is denoted by this
                                'strip off the last digit of isect
                                lSectionID = (100 * (lBlock.Id - 120)) + Int(lisect / 10)
                                lTable.OccurGroup = lisect - (Int(lisect / 10) * 10)
                            Else
                                lSectionID = (100 * (lBlock.Id - 120)) + lisect
                                lTable.OccurGroup = 0
                            End If
                            lTable.NumOccur = lirept
                            lTable.SGRP = ltabno
                            lheader = addComment(RTrim(lhdrbuf(0)), ladjLen)
                            For j As Integer = 2 To lnmhdr
                                lheader = lheader & vbCrLf & addComment(RTrim(lhdrbuf(j - 1)), ladjLen)
                            Next j
                            lTable.HeaderE = lheader
                            lheader = addComment(RTrim(lhdrbufM(0)), ladjLen)
                            For j As Integer = 2 To lnmhdrM
                                lheader = lheader & vbCrLf & addComment(RTrim(lhdrbufM(j - 1)), ladjLen)
                            Next j
                            lTable.HeaderM = lheader

                            'add info about the fields of this table to the parameter definition table
                            Dim lParms As New HspfParmDefs
                            For j As Integer = 2 To lnflds
                                'loop through fields
                                If Len(Trim(lfdnam(j - 1))) > 0 Then
                                    'dont add fields without field names
                                    Dim lParm As New HSPFParmDef
                                    lParmRecCount += 1
                                    lParm.Name = Trim(lfdnam(j - 1))
                                    'lParm.id = lParmRecCount
                                    'lParm.tableid = lTableRecCount
                                    ltyp = Mid(lftyp, j, 1)
                                    Select Case ltyp
                                        Case "I" : lParm.Typ = 1 ' ATCoInt
                                        Case "R" : lParm.Typ = 2 ' ATCoDbl
                                        Case "C" : lParm.Typ = 0 ' ATCoTxt
                                        Case Else : lParm.Typ = -999
                                    End Select
                                    lParm.StartCol = lscol(j - 1) + ladjLen
                                    lParm.Length = lflen(j - 1)
                                    If lParm.Typ = 1 Then
                                        lParm.Min = limin(lapos(j - 1) - 1)
                                        lParm.Max = limax(lapos(j - 1) - 1)
                                        lParm.DefaultValue = lidef(lapos(j - 1) - 1) & " "
                                        lParm.MetricMin = limetmin(lapos(j - 1) - 1)
                                        lParm.MetricMax = limetmax(lapos(j - 1) - 1)
                                        lParm.MetricDefault = limetdef(lapos(j - 1) - 1) & " "
                                    ElseIf lParm.Typ = 2 Then
                                        lParm.Min = lrmin(lapos(j - 1) - 1)
                                        lParm.Max = lrmax(lapos(j - 1) - 1)
                                        lParm.DefaultValue = lrdef(lapos(j - 1) - 1) & " "
                                        lParm.MetricMin = lrmetmin(lapos(j - 1) - 1)
                                        lParm.MetricMax = lrmetmax(lapos(j - 1) - 1)
                                        lParm.MetricDefault = lrmetdef(lapos(j - 1) - 1) & " "
                                    ElseIf lParm.Typ = 0 Then
                                        'special case for some 4character category parms
                                        If lflen(j - 1) = 4 Then
                                            If (Left(Trim(lfdnam(j - 1)), 4) = "CTAG" Or Left(Trim(lfdnam(j - 1)), 5) = "CFVOL" Or Trim(lfdnam(j - 1)) = "CEVAP" Or Trim(lfdnam(j - 1)) = "CPREC" Or Trim(lfdnam(j - 1)) = "ICAT") Then
                                                lParm.Length = 2
                                                lParm.StartCol = lscol(j - 1) + ladjLen + 2
                                            End If
                                        End If
                                        lParm.DefaultValue = " "
                                        lParm.MetricDefault = " "
                                    End If
                                    lParms.Add(lParm)
                                End If
                            Next j
                            'End If
                            lTable.ParmDefs = lParms
                            UpdateParmsMultLines(lBlock.Name, lTable)
                            lTableDefs.Add(lTable)
                            lBlockTableDefs.Add(lTable)
                            For Each lSection As HspfSectionDef In lBlock.SectionDefs
                                If lSection.Id = lSectionID Then
                                    lSection.TableDefs.Add(lTable)
                                    lTable.Parent = lSection
                                End If
                            Next
                        End If
                        ltabno = ltabno + 1
                    Loop
                End If
            End If
            lBlock.TableDefs = lBlockTableDefs
        Next

        'get help information for each table
        Dim lHhdrbuf(10) As String
        Dim lHolen As Integer
        Dim lHinitfg As Integer
        Dim lHcont As Integer
        Dim lHobuff As String
        Dim lHTempbuff As String
        Dim lHSclu As Integer
        Dim lHgptr As Integer
        Dim lHisect As Integer
        Dim lHirept As Integer
        Dim lHRetid As Integer
        Dim lHFptr(64) As Integer
        For Each lBlock As HspfBlockDef In pBlockDefs
            For Each lSection As HspfSectionDef In lBlock.SectionDefs
                For Each lTable As HspfTableDef In lSection.TableDefs
                    Call F90_XTINFO(lBlock.Id, lTable.SGRP, 1, 0, lnflds, lscol, lflen, lftyp, lapos, limin, limax, lidef, lrmin, lrmax, lrdef, lnmhdr, lHhdrbuf, lfdnam, lHisect, lHirept, lHRetid)
                    lHinitfg = 1
                    lHolen = 80
                    lHcont = 1
                    lHobuff = ""
                    lHTempbuff = ""
                    lHSclu = -1
                    Call F90_WMSGTH(lHgptr, lHFptr(0))
                    Do While lHcont = 1
                        If lHgptr > 0 Then
                            lHolen = 80
                            Call F90_WMSGTT(lFmsg, lHSclu, lHgptr, lHinitfg, lHolen, lHcont, lHobuff)
                            If Len(lHTempbuff) = 0 Then
                                lHTempbuff = Trim(lHobuff)
                            Else
                                lHTempbuff = lHTempbuff & " " & Trim(lHobuff)
                            End If
                            lHSclu = 0
                        Else
                            lHcont = 0
                        End If
                    Loop
                    If lHTempbuff.Length > 0 Then
                        lTable.Define = Trim(lHTempbuff)
                    Else
                        lTable.Define = " "
                    End If
                    'now fill parameter help
                    Dim lHistart As Integer = 2
                    If lBlock.Id = 4 Then
                        'special case for ftables
                        lHistart = 1
                    End If
                    For lFieldIndex As Integer = lHistart To lnflds
                        If Len(Trim(lfdnam(lFieldIndex - 1))) > 0 Then
                            'dont add help for fields without field names
                            If lHFptr(lFieldIndex - 1) > 0 Then
                                lHinitfg = 1
                                lHolen = 80
                                lHcont = 1
                                lHobuff = ""
                                lHTempbuff = ""
                                lHSclu = -1
                                Do While lHcont = 1
                                    lHolen = 80
                                    Call F90_WMSGTT(lFmsg, lHSclu, lHFptr(lFieldIndex - 1), lHinitfg, lHolen, lHcont, lHobuff)
                                    If Len(lHTempbuff) = 0 Then
                                        lHTempbuff = Trim(lHobuff)
                                    Else
                                        lHTempbuff = lHTempbuff & " " & Trim(lHobuff)
                                    End If
                                    lHSclu = 0
                                Loop
                                lTable.ParmDefs(lFieldIndex - lHistart).Define = Trim(lHTempbuff) & " "
                            Else
                                lTable.ParmDefs(lFieldIndex - lHistart).Define = " "
                            End If
                        End If
                    Next lFieldIndex
                Next
            Next
        Next

        'fill table of timeseries groups/member names for each operation
        pTSGroupDefs = Nothing
        pTSGroupDefs = New HspfTSGroupDefs
        Dim lTSInitfg As Integer = 1
        Dim lTSCont As Integer = 1
        Dim lTSSgrp As Integer = 1
        Dim lTSIgroup As Integer = 0
        Dim lTSRetid As Integer = 0
        Dim lTSOlen As Integer = 0
        Dim lTSObuff As String = ""
        Dim lTSNgroup As Integer = 0
        Dim lTSMembers As New Collection(Of HspfTSMemberDef)
        For Each lBlock As HspfBlockDef In pBlockDefs
            If lBlock.Id > 100 Then
                lTSInitfg = 1
                lTSCont = 1
                lTSSgrp = 1
                lTSIgroup = 0
                Do While lTSCont <> 0
                    'get each group name
                    lTSRetid = 0
                    lTSOlen = 10
                    Call F90_WMSGTT(lFmsg, lBlock.Id + 20, lTSSgrp, lTSInitfg, lTSOlen, lTSCont, lTSObuff)
                    Dim lTSGroup As New HspfTSGroupDef
                    lTSGroup.Name = Trim(Mid(lTSObuff, 1, 6))
                    lTSGroup.BlockID = lBlock.Id
                    lTSIgroup = lTSIgroup + 1
                    lTSGroup.Id = ((lBlock.Id - 120) * 100) + lTSIgroup
                    'save base number for associated sgrp
                    lTSGroup.GroupBase = CShort(Mid(lTSObuff, 8, 3))
                    lTSInitfg = 0
                    lTSMembers = New Collection(Of HspfTSMemberDef)
                    lTSGroup.MemberDefs = lTSMembers
                    pTSGroupDefs.Add(lTSGroup)
                Loop
            End If
        Next

        lTSMembers = New Collection(Of HspfTSMemberDef)
        lTSIgroup = 0
        Dim lPrevGroup As Integer = 0
        Dim lPrevBlock As Integer = 0
        For Each lGroup As HspfTSGroupDef In pTSGroupDefs
            'lGroup.MemberDefs = lTSMembers
            'now populate member names
            If lGroup.BlockID <> lPrevBlock Then
                lTSIgroup = 0
            End If
            If lGroup.Id <> lPrevGroup Then
                lTSIgroup += 1
            End If
            lTSInitfg = 1
            lTSCont = 1
            lTSSgrp = 1 + lTSIgroup
            lTSNgroup = 1
            Do While lTSCont <> 0
                'get each member name
                lTSRetid = 0
                lTSOlen = 8
                Call F90_WMSGTT(lFmsg, lGroup.BlockID + 20, lTSSgrp, lTSInitfg, lTSOlen, lTSCont, lTSObuff)
                Dim lTSMember As New HspfTSMemberDef
                lTSMember.Name = Trim(Mid(lTSObuff, 1, 6))
                lTSMember.TSGroupID = ((lGroup.BlockID - 120) * 100) + lTSIgroup
                lTSMember.Id = lTSMembers.Count + 1
                lTSMember.SCLU = lGroup.BlockID + 20
                lTSMember.SGRP = lGroup.GroupBase + lTSNgroup
                lTSNgroup = lTSNgroup + 1
                lTSInitfg = 0
                lTSMembers.Add(lTSMember)
                lPrevGroup = lTSMember.TSGroupID
                lPrevBlock = lGroup.BlockID
            Loop
        Next

        ''now populate member details
        For Each lTSMember As HspfTSMemberDef In lTSMembers
            'get first line of details
            lTSInitfg = 1
            lTSOlen = 80
            lTSCont = 1
            lTSObuff = ""
            Dim lSclu As Integer = lTSMember.SCLU
            Dim lSgrp As Integer = lTSMember.SGRP
            Call F90_WMSGTT(lFmsg, lSclu, lSgrp, lTSInitfg, lTSOlen, lTSCont, lTSObuff)
            lTSMember.MDim1 = CShort(Mid(lTSObuff, 7, 3))
            lTSMember.MDim2 = CShort(Mid(lTSObuff, 10, 3))
            lTSMember.Maxsb1 = CShort(Mid(lTSObuff, 13, 6))
            lTSMember.Maxsb2 = CShort(Mid(lTSObuff, 19, 6))
            lTSMember.MKind = CShort(Mid(lTSObuff, 25, 2))
            Dim lSptrn As String = Mid(lTSObuff, 27, 2)
            If lSptrn.Trim.Length > 0 Then
                lTSMember.Sptrn = CShort(lSptrn)
            End If
            lTSMember.Msect = CShort(Mid(lTSObuff, 29, 2))
            Dim lMio As String = Mid(lTSObuff, 31, 2)
            If lMio.Trim.Length > 0 Then
                lTSMember.Mio = CShort(lMio)
            End If
            lTSMember.OsvBas = CShort(Mid(lTSObuff, 33, 5))
            lTSMember.OsvOff = CShort(Mid(lTSObuff, 38, 6))
            If Len(Trim(Mid(lTSObuff, 49, 8))) > 0 Then
                lTSMember.EUnits = Trim(Mid(lTSObuff, 49, 8))
            Else
                lTSMember.EUnits = " "
            End If
            lTSMember.Ltval1 = CSng(Mid(lTSObuff, 57, 4))
            lTSMember.Ltval2 = CSng(Mid(lTSObuff, 61, 8))
            lTSMember.Ltval3 = CSng(Mid(lTSObuff, 69, 4))
            lTSMember.Ltval4 = CSng(Mid(lTSObuff, 73, 8))
            'now get second line of details
            lTSInitfg = 0
            lTSOlen = 80
            Call F90_WMSGTT(lFmsg, lSclu, lSgrp, lTSInitfg, lTSOlen, lTSCont, lTSObuff)
            lTSMember.Defn = Trim(Mid(lTSObuff, 1, 43))
            If Len(Trim(Mid(lTSObuff, 49, 8))) > 0 Then
                lTSMember.MUnits = Trim(Mid(lTSObuff, 49, 8))
            Else
                lTSMember.MUnits = " "
            End If
            lTSMember.Ltval5 = CSng(Mid(lTSObuff, 57, 4))
            lTSMember.Ltval6 = CSng(Mid(lTSObuff, 61, 8))
            lTSMember.Ltval7 = CSng(Mid(lTSObuff, 69, 4))
            lTSMember.Ltval8 = CSng(Mid(lTSObuff, 73, 8))
            For Each lGroup As HspfTSGroupDef In pTSGroupDefs
                If lGroup.Id = lTSMember.TSGroupID Then
                    lGroup.MemberDefs.Add(lTSMember)
                    lTSMember.Parent = lGroup
                End If
            Next
        Next

        If lNeedtoClose AndAlso lFmsg > 0 Then
            Dim lRetcod As Integer = F90_WDFLCL(lFmsg)
        End If
        Logger.Dbg("HSPFMsg:Open Finished")
    End Sub

    Private Function FilterNull(ByRef aValue As Object, Optional ByRef aNullReturn As Object = 0) As Object
        If IsDBNull(aValue) Then
            Return aNullReturn
        ElseIf aValue.ToString.Length = 0 Then
            Return aNullReturn
        Else
            Return aValue
        End If
    End Function

    Private Sub UpdateParmsMultLines(ByRef aBlockName As String, ByRef aTable As HspfTableDef)
        With aTable
            If aBlockName = "DURANL" And .Name = "LEVELS" Then
                For i As Integer = 1 To 6
                    Dim lParmDef As New HSPFParmDef
                    lParmDef.Name = "LEVE" & CStr(15 + i) 'Name
                    lParmDef.Typ = 2 ' ATCoDbl
                    lParmDef.StartCol = 76 + (i * 5)
                    lParmDef.Length = 5
                    lParmDef.Min = -999
                    lParmDef.Max = -999
                    lParmDef.DefaultValue = 0
                    lParmDef.Other = lParmDef.StartCol & ":" & lParmDef.Length
                    lParmDef.Define = "LEVEL(2thru21) contains the 20 possible user-specified levels for which the input time series will be analyzed."
                    .ParmDefs.Add(lParmDef)
                Next i
            ElseIf aBlockName = "DURANL" And .Name = "LCONC" Then
                For i As Integer = 1 To 3 'three fields to tack on
                    Dim lParmDef As New HSPFParmDef
                    lParmDef.Name = "LCONC" & CStr(7 + i) 'Name
                    lParmDef.Typ = 2 ' ATCoDbl
                    lParmDef.StartCol = 71 + (i * 10)
                    lParmDef.Length = 10
                    lParmDef.Min = -999
                    lParmDef.Max = -999
                    lParmDef.DefaultValue = 0
                    lParmDef.Other = lParmDef.StartCol & ":" & lParmDef.Length
                    lParmDef.Define = ""
                    .ParmDefs.Add(lParmDef)
                Next i
            ElseIf aBlockName = "PERLND" And .Name = "IRRIG-SCHED" Then
                For i As Integer = 2 To 10 'up to 10 rows possible
                    Dim lParmDef As New HSPFParmDef
                    lParmDef.Name = "IRYR" & CStr((2 * (i - 1)) + 1) 'year
                    lParmDef.Typ = 2 ' ATCoDbl
                    lParmDef.StartCol = (70 * (i - 1)) + 12
                    lParmDef.Length = 4
                    lParmDef.Min = 0
                    lParmDef.Max = -999
                    lParmDef.DefaultValue = 0
                    lParmDef.Other = lParmDef.StartCol & ":" & lParmDef.Length
                    lParmDef.Define = ""
                    .ParmDefs.Add(lParmDef)
                    lParmDef = New HSPFParmDef
                    lParmDef.Name = "IRMO" & CStr((2 * (i - 1)) + 1) 'month
                    lParmDef.Typ = 2 ' ATCoDbl
                    lParmDef.StartCol = (70 * (i - 1)) + 17
                    lParmDef.Length = 2
                    lParmDef.Min = 1
                    lParmDef.Max = 12
                    lParmDef.DefaultValue = 1
                    lParmDef.Other = lParmDef.StartCol & ":" & lParmDef.Length
                    lParmDef.Define = ""
                    .ParmDefs.Add(lParmDef)
                    lParmDef = New HSPFParmDef
                    lParmDef.Name = "IRDY" & CStr((2 * (i - 1)) + 1) 'day
                    lParmDef.Typ = 2 ' ATCoDbl
                    lParmDef.StartCol = (70 * (i - 1)) + 20
                    lParmDef.Length = 2
                    lParmDef.Min = 1
                    lParmDef.Max = 31
                    lParmDef.DefaultValue = 1
                    lParmDef.Other = lParmDef.StartCol & ":" & lParmDef.Length
                    lParmDef.Define = ""
                    .ParmDefs.Add(lParmDef)
                    lParmDef = New HSPFParmDef
                    lParmDef.Name = "IRHR" & CStr((2 * (i - 1)) + 1) 'hour
                    lParmDef.Typ = 2 ' ATCoDbl
                    lParmDef.StartCol = (70 * (i - 1)) + 23
                    lParmDef.Length = 2
                    lParmDef.Min = 0
                    lParmDef.Max = 24
                    lParmDef.DefaultValue = 0
                    lParmDef.Other = lParmDef.StartCol & ":" & lParmDef.Length
                    lParmDef.Define = ""
                    .ParmDefs.Add(lParmDef)
                    lParmDef = New HSPFParmDef
                    lParmDef.Name = "IRMI" & CStr((2 * (i - 1)) + 1) 'min
                    lParmDef.Typ = 2 ' ATCoDbl
                    lParmDef.StartCol = (70 * (i - 1)) + 26
                    lParmDef.Length = 2
                    lParmDef.Min = 0
                    lParmDef.Max = 60
                    lParmDef.DefaultValue = 0
                    lParmDef.Other = lParmDef.StartCol & ":" & lParmDef.Length
                    lParmDef.Define = ""
                    .ParmDefs.Add(lParmDef)
                    lParmDef = New HSPFParmDef
                    lParmDef.Name = "IRDUR" & CStr((2 * (i - 1)) + 1) 'duration
                    lParmDef.Typ = 2 ' ATCoDbl
                    lParmDef.StartCol = (70 * (i - 1)) + 28
                    lParmDef.Length = 5
                    lParmDef.Min = 0
                    lParmDef.Max = -999
                    lParmDef.DefaultValue = 0
                    lParmDef.Other = lParmDef.StartCol & ":" & lParmDef.Length
                    lParmDef.Define = ""
                    .ParmDefs.Add(lParmDef)
                    lParmDef = New HSPFParmDef
                    lParmDef.Name = "IRRAT" & CStr((2 * (i - 1)) + 1) 'rate
                    lParmDef.Typ = 2 ' ATCoDbl
                    lParmDef.StartCol = (70 * (i - 1)) + 33
                    lParmDef.Length = 10
                    lParmDef.Min = 0
                    lParmDef.Max = -999
                    lParmDef.DefaultValue = 0
                    lParmDef.Other = lParmDef.StartCol & ":" & lParmDef.Length
                    lParmDef.Define = ""
                    .ParmDefs.Add(lParmDef)
                    lParmDef = New HSPFParmDef
                    lParmDef.Name = "IRYR" & CStr(2 * i) '2nd year
                    lParmDef.Typ = 2 ' ATCoDbl
                    lParmDef.StartCol = (70 * (i - 1)) + 49
                    lParmDef.Length = 4
                    lParmDef.Min = 0
                    lParmDef.Max = -999
                    lParmDef.DefaultValue = 0
                    lParmDef.Other = lParmDef.StartCol & ":" & lParmDef.Length
                    lParmDef.Define = ""
                    .ParmDefs.Add(lParmDef)
                    lParmDef = New HSPFParmDef
                    lParmDef.Name = "IRMO" & CStr(2 * i) 'month
                    lParmDef.Typ = 2 ' ATCoDbl
                    lParmDef.StartCol = (70 * (i - 1)) + 54
                    lParmDef.Length = 2
                    lParmDef.Min = 1
                    lParmDef.Max = 12
                    lParmDef.DefaultValue = 1
                    lParmDef.Other = lParmDef.StartCol & ":" & lParmDef.Length
                    lParmDef.Define = ""
                    .ParmDefs.Add(lParmDef)
                    lParmDef = New HSPFParmDef
                    lParmDef.Name = "IRDY" & CStr(2 * i) 'day
                    lParmDef.Typ = 2 ' ATCoDbl
                    lParmDef.StartCol = (70 * (i - 1)) + 57
                    lParmDef.Length = 2
                    lParmDef.Min = 1
                    lParmDef.Max = 31
                    lParmDef.DefaultValue = 1
                    lParmDef.Other = lParmDef.StartCol & ":" & lParmDef.Length
                    lParmDef.Define = ""
                    .ParmDefs.Add(lParmDef)
                    lParmDef = New HSPFParmDef
                    lParmDef.Name = "IRHR" & CStr(2 * i) 'hour
                    lParmDef.Typ = 2 ' ATCoDbl
                    lParmDef.StartCol = (70 * (i - 1)) + 60
                    lParmDef.Length = 2
                    lParmDef.Min = 0
                    lParmDef.Max = 24
                    lParmDef.DefaultValue = 0
                    lParmDef.Other = lParmDef.StartCol & ":" & lParmDef.Length
                    lParmDef.Define = ""
                    .ParmDefs.Add(lParmDef)
                    lParmDef = New HSPFParmDef
                    lParmDef.Name = "IRMI" & CStr(2 * i) 'min
                    lParmDef.Typ = 2 ' ATCoDbl
                    lParmDef.StartCol = (70 * (i - 1)) + 63
                    lParmDef.Length = 2
                    lParmDef.Min = 0
                    lParmDef.Max = 60
                    lParmDef.DefaultValue = 0
                    lParmDef.Other = lParmDef.StartCol & ":" & lParmDef.Length
                    lParmDef.Define = ""
                    .ParmDefs.Add(lParmDef)
                    lParmDef = New HSPFParmDef
                    lParmDef.Name = "IRDUR" & CStr(2 * i) 'duration
                    lParmDef.Typ = 2 ' ATCoDbl
                    lParmDef.StartCol = (70 * (i - 1)) + 65
                    lParmDef.Length = 5
                    lParmDef.Min = 0
                    lParmDef.Max = -999
                    lParmDef.DefaultValue = 0
                    lParmDef.Other = lParmDef.StartCol & ":" & lParmDef.Length
                    lParmDef.Define = ""
                    .ParmDefs.Add(lParmDef)
                    lParmDef = New HSPFParmDef
                    lParmDef.Name = "IRRAT" & CStr(2 * i) 'rate
                    lParmDef.Typ = 2 ' ATCoDbl
                    lParmDef.StartCol = (70 * (i - 1)) + 70
                    lParmDef.Length = 10
                    lParmDef.Min = 0
                    lParmDef.Max = -999
                    lParmDef.DefaultValue = 0
                    lParmDef.Other = lParmDef.StartCol & ":" & lParmDef.Length
                    lParmDef.Define = ""
                    .ParmDefs.Add(lParmDef)
                Next i
            ElseIf aBlockName = "RCHRES" And .Name = "HT-BED-DELH" Then
                For i As Integer = 2 To 14 '100 values needed
                    For j As Integer = 1 To 7 '
                        Dim lParmDef As New HSPFParmDef
                        lParmDef.Name = "DELH" & CStr((7 * (i - 1)) + j)
                        lParmDef.Typ = 2 ' ATCoDbl
                        lParmDef.StartCol = (70 * (i - 1)) + 11 + (10 * (j - 1))
                        lParmDef.Length = 10
                        lParmDef.Min = -999
                        lParmDef.Max = -999
                        lParmDef.DefaultValue = 0
                        lParmDef.Other = lParmDef.StartCol & ":" & lParmDef.Length
                        lParmDef.Define = ""
                        .ParmDefs.Add(lParmDef)
                    Next j
                Next i
                For i As Integer = 1 To 2 'two more fields to tack on to make 100
                    Dim lParmDef As New HSPFParmDef
                    lParmDef.Name = "DELH" & CStr(98 + i) 'Name
                    lParmDef.Typ = 2 ' ATCoDbl
                    lParmDef.StartCol = 991 + (10 * (i - 1))
                    lParmDef.Length = 10
                    lParmDef.Min = -999
                    lParmDef.Max = -999
                    lParmDef.DefaultValue = 0
                    lParmDef.Other = lParmDef.StartCol & ":" & lParmDef.Length
                    lParmDef.Define = ""
                    .ParmDefs.Add(lParmDef)
                Next i
            ElseIf aBlockName = "RCHRES" And .Name = "HT-BED-DELTT" Then
                For i As Integer = 2 To 14 '100 values needed
                    For j As Integer = 1 To 7 '
                        Dim lParmDef As New HSPFParmDef
                        lParmDef.Name = "DELTT" & CStr((7 * (i - 1)) + j)
                        lParmDef.Typ = 2 ' ATCoDbl
                        lParmDef.StartCol = (70 * (i - 1)) + 11 + (10 * (j - 1))
                        lParmDef.Length = 10
                        lParmDef.Min = -999
                        lParmDef.Max = -999
                        lParmDef.DefaultValue = 0
                        lParmDef.Other = lParmDef.StartCol & ":" & lParmDef.Length
                        lParmDef.Define = ""
                        .ParmDefs.Add(lParmDef)
                    Next j
                Next i
                For i As Integer = 1 To 2 'two more fields to tack on to make 100
                    Dim lParmDef As New HSPFParmDef
                    lParmDef.Name = "DELTT" & CStr(98 + i) 'Name
                    lParmDef.Typ = 2 ' ATCoDbl
                    lParmDef.StartCol = 991 + (10 * (i - 1))
                    lParmDef.Length = 10
                    lParmDef.Min = -999
                    lParmDef.Max = -999
                    lParmDef.DefaultValue = 0
                    lParmDef.Other = lParmDef.StartCol & ":" & lParmDef.Length
                    lParmDef.Define = ""
                    .ParmDefs.Add(lParmDef)
                Next i
            ElseIf aBlockName = "RCHRES" And .Name = "GQ-PHOTPM" Then
                For i As Integer = 1 To 7 'seven fields to tack on
                    Dim lParmDef As New HSPFParmDef
                    lParmDef.Name = "PHOTPM" & CStr(7 + i) 'Name
                    lParmDef.Typ = 2 ' ATCoDbl
                    lParmDef.StartCol = 71 + (i * 10)
                    lParmDef.Length = 10
                    lParmDef.Min = 0
                    lParmDef.Max = -999
                    lParmDef.DefaultValue = 0
                    lParmDef.Other = lParmDef.StartCol & ":" & lParmDef.Length
                    lParmDef.Define = ""
                    .ParmDefs.Add(lParmDef)
                Next i
                For i As Integer = 1 To 6 'six more fields to tack on
                    Dim lParmDef As New HSPFParmDef
                    lParmDef.Name = "PHOTPM" & CStr(14 + i) 'Name
                    lParmDef.Typ = 2 ' ATCoDbl
                    lParmDef.StartCol = 141 + (i * 10)
                    lParmDef.Length = 10
                    If i < 5 Then
                        lParmDef.Min = 0
                        lParmDef.Max = -999
                        lParmDef.DefaultValue = 0
                    ElseIf i = 5 Then
                        lParmDef.Min = 0.0001
                        lParmDef.Max = 10
                        lParmDef.DefaultValue = 1
                    Else
                        lParmDef.Min = 1
                        lParmDef.Max = 2
                        lParmDef.DefaultValue = 1
                    End If
                    lParmDef.Other = lParmDef.StartCol & ":" & lParmDef.Length
                    lParmDef.Define = ""
                    .ParmDefs.Add(lParmDef)
                Next i
            ElseIf aBlockName = "RCHRES" And .Name = "GQ-ALPHA" Then
                For i As Integer = 1 To 7 'seven fields to tack on
                    Dim lParmDef As New HSPFParmDef
                    lParmDef.Name = "ALPH" & CStr(7 + i) 'Name
                    lParmDef.Typ = 2 ' ATCoDbl
                    lParmDef.StartCol = 71 + (i * 10)
                    lParmDef.Length = 10
                    lParmDef.Min = 0.00001
                    lParmDef.Max = -999
                    lParmDef.DefaultValue = -999
                    lParmDef.Other = lParmDef.StartCol & ":" & lParmDef.Length
                    lParmDef.Define = ""
                    .ParmDefs.Add(lParmDef)
                Next i
                For i As Integer = 1 To 4 'four more fields to tack on
                    Dim lParmDef As New HSPFParmDef
                    lParmDef.Name = "ALPH" & CStr(14 + i) 'Name
                    lParmDef.Typ = 2 ' ATCoDbl
                    lParmDef.StartCol = 141 + (i * 10)
                    lParmDef.Length = 10
                    lParmDef.Min = 0.00001
                    lParmDef.Max = -999
                    lParmDef.DefaultValue = -999
                    lParmDef.Other = lParmDef.StartCol & ":" & lParmDef.Length
                    lParmDef.Define = ""
                    .ParmDefs.Add(lParmDef)
                Next i
            ElseIf aBlockName = "RCHRES" And .Name = "GQ-GAMMA" Then
                For i As Integer = 1 To 7 'seven fields to tack on
                    Dim lParmDef As New HSPFParmDef
                    lParmDef.Name = "GAMM" & CStr(7 + i) 'Name
                    lParmDef.Typ = 2 ' ATCoDbl
                    lParmDef.StartCol = 71 + (i * 10)
                    lParmDef.Length = 10
                    lParmDef.Min = 0
                    lParmDef.Max = -999
                    lParmDef.DefaultValue = 0
                    lParmDef.Other = lParmDef.StartCol & ":" & lParmDef.Length
                    lParmDef.Define = ""
                    .ParmDefs.Add(lParmDef)
                Next i
                For i As Integer = 1 To 4 'four more fields to tack on
                    Dim lParmDef As New HSPFParmDef
                    lParmDef.Name = "GAMM" & CStr(14 + i) 'Name
                    lParmDef.Typ = 2 ' ATCoDbl
                    lParmDef.StartCol = 141 + (i * 10)
                    lParmDef.Length = 10
                    lParmDef.Min = 0
                    lParmDef.Max = -999
                    lParmDef.DefaultValue = 0
                    lParmDef.Other = lParmDef.StartCol & ":" & lParmDef.Length
                    lParmDef.Define = ""
                    .ParmDefs.Add(lParmDef)
                Next i
            ElseIf aBlockName = "RCHRES" And .Name = "GQ-DELTA" Then
                For i As Integer = 1 To 7 'seven fields to tack on
                    Dim lParmDef As New HSPFParmDef
                    lParmDef.Name = "DEL" & CStr(7 + i) 'Name
                    lParmDef.Typ = 2 ' ATCoDbl
                    lParmDef.StartCol = 71 + (i * 10)
                    lParmDef.Length = 10
                    lParmDef.Min = 0
                    lParmDef.Max = -999
                    lParmDef.DefaultValue = 0
                    lParmDef.Other = lParmDef.StartCol & ":" & lParmDef.Length
                    lParmDef.Define = ""
                    .ParmDefs.Add(lParmDef)
                Next i
                For i As Integer = 1 To 4 'four more fields to tack on
                    Dim lParmDef As New HSPFParmDef
                    lParmDef.Name = "DEL" & CStr(14 + i) 'Name
                    lParmDef.Typ = 2 ' ATCoDbl
                    lParmDef.StartCol = 141 + (i * 10)
                    lParmDef.Length = 10
                    lParmDef.Min = 0
                    lParmDef.Max = -999
                    lParmDef.DefaultValue = 0
                    lParmDef.Other = lParmDef.StartCol & ":" & lParmDef.Length
                    lParmDef.Define = ""
                    .ParmDefs.Add(lParmDef)
                Next i
            ElseIf aBlockName = "RCHRES" And .Name = "GQ-CLDFACT" Then
                For i As Integer = 1 To 7 'seven fields to tack on
                    Dim lParmDef As New HSPFParmDef
                    lParmDef.Name = "KCLD" & CStr(7 + i) 'Name
                    lParmDef.Typ = 2 ' ATCoDbl
                    lParmDef.StartCol = 71 + (i * 10)
                    lParmDef.Length = 10
                    lParmDef.Min = 0
                    lParmDef.Max = 1
                    lParmDef.DefaultValue = 0
                    lParmDef.Other = lParmDef.StartCol & ":" & lParmDef.Length
                    lParmDef.Define = ""
                    .ParmDefs.Add(lParmDef)
                Next i
                For i As Integer = 1 To 4 'four more fields to tack on
                    Dim lParmDef As New HSPFParmDef
                    lParmDef.Name = "KCLD" & CStr(14 + i) 'Name
                    lParmDef.Typ = 2 ' ATCoDbl
                    lParmDef.StartCol = 141 + (i * 10)
                    lParmDef.Length = 10
                    lParmDef.Min = 0
                    lParmDef.Max = 1
                    lParmDef.DefaultValue = 0
                    lParmDef.Other = lParmDef.StartCol & ":" & lParmDef.Length
                    lParmDef.Define = ""
                    .ParmDefs.Add(lParmDef)
                Next i
            ElseIf aBlockName = "RCHRES" And .Name = "GQ-DAUGHTER" Then
                For i As Integer = 2 To 3 '3 rows needed
                    For j As Integer = 1 To 3 'three values per row
                        Dim lParmDef As New HSPFParmDef
                        lParmDef.Name = "ZERO" & CStr(i) & CStr(j)
                        lParmDef.Typ = 2 ' ATCoDbl
                        lParmDef.StartCol = (70 * (i - 1)) + 11 + (10 * (j - 1))
                        lParmDef.Length = 10
                        lParmDef.Min = 0
                        lParmDef.Max = -999
                        lParmDef.DefaultValue = 0
                        lParmDef.Other = lParmDef.StartCol & ":" & lParmDef.Length
                        lParmDef.Define = ""
                        .ParmDefs.Add(lParmDef)
                    Next j
                Next i
            ElseIf aBlockName = "REPORT" And .Name = "REPORT-SRC" Then
                For i As Integer = 2 To 25 'up to 25 rows possible
                    Dim lParmDef As New HSPFParmDef
                    lParmDef.Name = "SRCID" & CStr(i) 'Name
                    lParmDef.Typ = 0 ' ATCoTxt
                    lParmDef.StartCol = (70 * (i - 1)) + 11
                    lParmDef.Length = 20
                    lParmDef.DefaultValue = ""
                    lParmDef.Other = lParmDef.StartCol & ":" & lParmDef.Length
                    lParmDef.Define = ""
                    .ParmDefs.Add(lParmDef)
                Next i
            ElseIf aBlockName = "REPORT" And .Name = "REPORT-CON" Then
                For i As Integer = 2 To 20 'up to 20 rows possible
                    Dim lParmDef As New HSPFParmDef
                    lParmDef.Name = "CONID" & CStr(i) 'Name
                    lParmDef.Typ = 0 ' ATCoTxt
                    lParmDef.StartCol = ((i - 1) * 70) + 11
                    lParmDef.Length = 20
                    lParmDef.DefaultValue = ""
                    lParmDef.Other = lParmDef.StartCol & ":" & lParmDef.Length
                    lParmDef.Define = ""
                    .ParmDefs.Add(lParmDef)
                    lParmDef = New HSPFParmDef
                    lParmDef.Name = "TRAN" & CStr(i) 'tran
                    lParmDef.Typ = 0 ' ATCoTxt
                    lParmDef.StartCol = (70 * (i - 1)) + 32
                    lParmDef.Length = 4
                    lParmDef.DefaultValue = "SUM"
                    lParmDef.Other = lParmDef.StartCol & ":" & lParmDef.Length
                    lParmDef.Define = ""
                    .ParmDefs.Add(lParmDef)
                    lParmDef = New HSPFParmDef
                    lParmDef.Name = "SIGD" & CStr(i) 'sig digits
                    lParmDef.Typ = 1 ' ATCoInt
                    lParmDef.StartCol = (70 * (i - 1)) + 36
                    lParmDef.Length = 5
                    lParmDef.Min = 2
                    lParmDef.Max = 5
                    lParmDef.DefaultValue = 5
                    lParmDef.Other = lParmDef.StartCol & ":" & lParmDef.Length
                    lParmDef.Define = ""
                    .ParmDefs.Add(lParmDef)
                    lParmDef = New HSPFParmDef
                    lParmDef.Name = "DECPLA" & CStr(i) 'dec places
                    lParmDef.Typ = 1 ' ATCoInt
                    lParmDef.StartCol = (70 * (i - 1)) + 41
                    lParmDef.Length = 5
                    lParmDef.Min = 0
                    lParmDef.Max = 3
                    lParmDef.DefaultValue = 2
                    lParmDef.Other = lParmDef.StartCol & ":" & lParmDef.Length
                    lParmDef.Define = ""
                    .ParmDefs.Add(lParmDef)
                Next i
            End If
        End With
    End Sub

    Private Function addComment(ByRef s As String, ByRef adjLen As Integer) As String
        Dim i As Integer
        Dim t As String
        If adjLen > 0 Then 'adjust length for old aide 78 char problem - add 2 blanks at 1 and 6
            s = " " & Left(s, 4) & " " & Right(s, Len(s) - 4)
        End If
        If Not (InStr(s, "***")) Then 'not a comment
            i = InStr(s, "   ")
            If i > 0 Then 'replace first three blanks
                t = Left(s, i - 1) & "***" & Right(s, Len(s) - i - 2)
                s = t
            ElseIf Len(s) < 78 Then  'at end
                s = s & "***"
            Else
                s = Left(s, 77) & "***"
            End If
        End If
        addComment = s
    End Function

    Public Sub New()
    End Sub
    Public Sub New(ByVal aFileName As String)
        Me.Open(aFileName)
    End Sub

    Public Overrides Function ToString() As String
        'used to dump contents of object to a string for testing
        Dim s As String = ""
        For Each lBlock As HspfBlockDef In pBlockDefs
            s = s & lBlock.Id & " " & lBlock.Name & vbCrLf
            For Each lSection As HspfSectionDef In lBlock.SectionDefs
                s = s & "  " & lSection.Id & " " & lSection.Name & vbCrLf
                If lSection.TableDefs IsNot Nothing Then
                    For Each lTable As HspfTableDef In lSection.TableDefs
                        If lTable.Name = "MON-MELT-FAC" Then
                            Logger.Dbg(lBlock.Name)
                        End If
                        s = s & "    " & lTable.Id & " " & lTable.Name & " " & lTable.HeaderE & " " & lTable.HeaderM & " " & _
                            lTable.NumOccur & " " & lTable.OccurGroup & " " & lTable.Parent.Name & " " & lTable.SGRP & " " & lTable.Define & vbCrLf
                        For Each lParm As HSPFParmDef In lTable.ParmDefs
                            s = s & "      " & lParm.Name & " " & lParm.Length & " " & lParm.Min & " " & lParm.Max & " " & lParm.DefaultValue & _
                            " " & lParm.MetricMin & lParm.MetricMax & " " & lParm.MetricDefault & " " & lParm.Define & _
                            " " & lParm.Other & " " & lParm.SoftMax & " " & lParm.SoftMin & " " & lParm.StartCol & lParm.Typ & vbCrLf
                            If lParm.Parent IsNot Nothing Then
                                s = s & "      " & lParm.Parent.name
                            End If
                        Next
                    Next
                End If
            Next
        Next
        For Each lBlock As HspfBlockDef In pBlockDefs
            For Each lTable As HspfTableDef In lBlock.TableDefs
                s = s & "    " & lTable.Id & " " & lTable.Name & vbCrLf
            Next
        Next
        For Each lGroup As HspfTSGroupDef In pTSGroupDefs
            s = s & lGroup.Id & " " & lGroup.Name & " " & lGroup.BlockID & " " & vbCrLf
            For Each lMember As HspfTSMemberDef In lGroup.MemberDefs
                s = s & "  " & lMember.Id & " " & lMember.Name & " " & lMember.Defn & " " & lMember.EUnits & " " & lMember.MUnits & _
                    " " & lMember.Ltval1 & " " & lMember.Ltval2 & " " & lMember.Ltval3 & " " & lMember.Ltval4 & _
                    " " & lMember.Ltval5 & " " & lMember.Ltval6 & " " & lMember.Ltval7 & " " & lMember.Ltval8 & _
                    " " & lMember.Maxsb1 & " " & lMember.Maxsb2 & " " & lMember.MDim1 & " " & lMember.MDim2 & _
                    " " & lMember.Mio & " " & lMember.MKind & " " & lMember.Msect & " " & lMember.OsvBas & _
                    " " & lMember.OsvOff & " " & lMember.SCLU & " " & lMember.SGRP & " " & lMember.Sptrn & " " & lMember.TSGroupID & vbCrLf
                If lMember.Parent IsNot Nothing Then
                    s = s & "      " & lMember.Parent.Name
                End If
            Next
        Next
        Return s
    End Function

    Private Function AddChar2Keyword(ByRef aKeyword As String) As String
        Dim lKeyword As String = aKeyword

        Select Case lKeyword
            Case "MON-IFLW-CON" : lKeyword &= "C"
            Case "MON-GRND-CON" : lKeyword &= "C"
            Case "PEST-AD-FLAG" : lKeyword &= "S"
            Case "PHOS-AD-FLAG" : lKeyword &= "S"
            Case "TRAC-AD-FLAG" : lKeyword &= "S"
            Case "PLNK-AD-FLAG" : lKeyword &= "S"
            Case "HYDR-CATEGOR" : lKeyword &= "Y"
            Case Else
        End Select

        Return lKeyword
    End Function
End Class