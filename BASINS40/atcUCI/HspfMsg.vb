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

        'Logger.Dbg("Opening " & aFilename)
        Dim lMsgMDb As New atcMDB(aFilename)
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

    Public Sub New()
    End Sub
    Public Sub New(ByVal aFileName As String)
        Me.Open(aFileName)
    End Sub
End Class