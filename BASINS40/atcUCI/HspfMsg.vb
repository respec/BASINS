Option Strict Off
Option Explicit On

Imports MapWinUtility

<System.Runtime.InteropServices.ProgId("HspfMsg_NET.HspfMsg")> Public Class HspfMsg
    'Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license

    Private pMsgFileName As String
    Private pBlockDefs As Collection 'of HspfBlockDef
    Private pErrorDescription As String
    Private pTSGroupDefs As Collection 'of HspfTSGroupDefs

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
        Dim myDb As New atcMDB(aFilename)
        pMsgFileName = aFilename

        'Logger.Dbg("BlockDefns")
        Dim lBlkTable As DataTable = myDb.GetTable("BlockDefns")
        Dim lBlock As HspfBlockDef
        Dim lBlockFieldID As Integer = lBlkTable.Columns.IndexOf("ID")
        Dim lBlockFieldName As Integer = lBlkTable.Columns.IndexOf("Name")
        Dim lSections As New Collection

        'Logger.Dbg("SectionDefns")
        Dim lSecTable As DataTable = myDb.GetTable("SectionDefns")
        Dim lSection As HspfSectionDef
        Dim lSectionFieldID As Integer = lSecTable.Columns.IndexOf("ID")
        Dim lSectionFieldName As Integer = lSecTable.Columns.IndexOf("Name")
        Dim lSectionFieldBlockID As Integer = lSecTable.Columns.IndexOf("BlockID")
        Dim lCriticalSection As String
        Dim lTables As New Collection
        Dim lBlkTables As New Collection

        Dim lTabTable As DataTable = myDb.GetTable("TableDefns")
        Dim lTableFieldSectionID As Integer = lTabTable.Columns.IndexOf("SectionID")
        Dim ltable As HspfTableDef
        Dim lParms As New Collection

        Dim lParmTable As DataTable = myDb.GetTable("ParmDefns")
        Dim lParmFieldTableID As Integer = lParmTable.Columns.IndexOf("TableID")
        Dim lParm As HSPFParmDef
        Dim lTyp As String

        Dim lTSGroupTable As DataTable = myDb.GetTable("TSGroupDefns")
        Dim lTSGroup As HspfTSGroupDef
        Dim lTSMembers As Collection
        Dim lTSMemberTable As DataTable = myDb.GetTable("TSMemberDefns")
        Dim lMemberFieldTSGroupID As Integer = lTSMemberTable.Columns.IndexOf("TSGroupID")
        Dim lTSMember As HspfTSMemberDef

        Dim lNumeric As Boolean

        Dim lBlkCount As Integer = lBlkTable.Rows.Count
        Dim lBlkNow As Integer

        pBlockDefs = Nothing
        pBlockDefs = New Collection

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
            lSections = New Collection
            lBlkTables = Nothing
            lBlkTables = New Collection
            lCriticalSection = "BlockID = " & CStr(lBlock.Id)
            For Each lSecRow As DataRow In lSecTable.Rows
                If lSecRow.Item(lSectionFieldBlockID) = lBlock.Id Then
                    lSection = New HspfSectionDef
                    lSection.Name = lSecRow.Item(lSectionFieldName)
                    lSection.Id = lSecRow.Item(lSectionFieldID)
                    'Logger.Dbg("Section Row " & lSection.Name)
                    lTables = Nothing
                    lTables = New Collection

                    'lTabTable = myDb.GetTable("TableDefns WHERE SectionID = " & CStr(lSection.Id))
                    For Each lTabRow As DataRow In lTabTable.Rows
                        If lTabRow.Item(lTableFieldSectionID) = lSection.Id Then
                            ltable = New HspfTableDef
                            ltable.Id = lTabRow.Item(0)
                            ltable.Parent = lSection
                            ltable.Name = lTabRow.Item(2)

                            'Logger.Dbg("Table Row " & ltable.Name)

                            ltable.SGRP = lTabRow.Item(3)
                            ltable.NumOccur = lTabRow.Item(4)
                            ltable.HeaderE = lTabRow.Item(5)
                            ltable.HeaderM = lTabRow.Item(6)
                            ltable.Define = FilterNull(lTabRow.Item(7), " ")
                            If lTabTable.Columns.Count < 9 Then
                                ltable.OccurGroup = 0
                            Else
                                ltable.OccurGroup = lTabRow.Item(8)
                            End If
                            lParms = Nothing
                            lParms = New Collection

                            'lParmTable = myDb.GetTable("ParmDefns WHERE TableID = " & CStr(ltable.Id))
                            For Each lParmRow As DataRow In lParmTable.Rows
                                If lParmRow.Item(lParmFieldTableID) = ltable.Id Then
                                    lParm = New HSPFParmDef
                                    lParm.Name = lParmRow.Item(2) 'Name
                                    'Logger.Dbg("Parm Row " & lParm.Name)
                                    lTyp = lParmRow.Item(3) 'Type
                                    Select Case lTyp
                                        Case "I" : lNumeric = True : lParm.Typ = 1 ' ATCoInt
                                        Case "R" : lNumeric = True : lParm.Typ = 2 ' ATCoSng
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
                                    'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
                                    If IsDBNull(lParmRow.Item(8)) Then
                                        lParm.DefaultValue = " "
                                    Else
                                        lParm.DefaultValue = lParmRow.Item(8) 'default
                                    End If
                                    If lParmTable.Columns.Count > 10 Then
                                        'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
                                        If IsDBNull(lParmRow.Item(12)) Then
                                            lParm.MetricDefault = " "
                                        Else
                                            lParm.MetricDefault = lParmRow.Item(12) 'default
                                        End If
                                    Else 'use english default
                                        'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
                                        If IsDBNull(lParmRow.Item(8)) Then
                                            lParm.MetricDefault = " "
                                        Else
                                            lParm.MetricDefault = lParmRow.Item(8)
                                        End If
                                    End If
                                    lParm.Other = lParmRow.Item(4) & ":" & lParmRow.Item(5)
                                    'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
                                    If IsDBNull(lParmRow.Item(9)) Then
                                        lParm.Define = " "
                                    Else
                                        lParm.Define = lParmRow.Item(9)
                                    End If
                                    lParms.Add(lParm, lParm.Name)
                                End If
                            Next
                            ltable.ParmDefs = lParms
                            updateParmsMultLines((lBlock.Name), ltable)
                            lTables.Add(ltable, ltable.Name)
                            lBlkTables.Add(ltable, ltable.Name)
                        End If
                    Next
                    lSection.TableDefs = lTables
                    lSections.Add(lSection, lSection.Name)
                End If
            Next
            lBlock.SectionDefs = lSections
            lBlock.TableDefs = lBlkTables
            pBlockDefs.Add(lBlock, lBlock.Name)
        Next

        'Logger.Dbg("TSGroupDefns")
        'now read TS group and member info
        pTSGroupDefs = Nothing
        pTSGroupDefs = New Collection
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
            lTSGroup.BlockId = lTSGroupRow.Item(lTSGroupFieldBlockID)
            lTSMembers = Nothing
            lTSMembers = New Collection
            'lTSMemberTable = myDb.GetTable("TSMemberDefns WHERE TSGroupID = " & CStr(lTSGroup.Id))

            For Each lTSMemberRow As DataRow In lTSMemberTable.Rows
                If lTSMemberRow.Item(lMemberFieldTSGroupID) = lTSGroup.Id Then
                    lTSMember = New HspfTSMemberDef
                    lTSMember.Id = lTSMemberRow.Item(lTSMemberFieldID)
                    lTSMember.Name = lTSMemberRow.Item(lTSMemberFieldName)
                    lTSMember.TSGroupId = lTSMemberRow.Item(lTSMemberFieldTSGroupID)
                    lTSMember.Parent = lTSGroup
                    lTSMember.SCLU = lTSMemberRow.Item(lTSMemberFieldSCLU)
                    lTSMember.SGRP = lTSMemberRow.Item(lTSMemberFieldSGRP)
                    lTSMember.mdim1 = FilterNull(lTSMemberRow.Item(lTSMemberFieldmdim1))
                    lTSMember.mdim2 = FilterNull(lTSMemberRow.Item(lTSMemberFieldmdim2))
                    lTSMember.maxsb1 = FilterNull(lTSMemberRow.Item(lTSMemberFieldmaxsb1))
                    lTSMember.maxsb2 = FilterNull(lTSMemberRow.Item(lTSMemberFieldmaxsb2))
                    lTSMember.mkind = FilterNull(lTSMemberRow.Item(lTSMemberFieldmkind))
                    lTSMember.sptrn = FilterNull(lTSMemberRow.Item(lTSMemberFieldsptrn))
                    lTSMember.msect = FilterNull(lTSMemberRow.Item(lTSMemberFieldmsect))
                    lTSMember.mio = FilterNull(lTSMemberRow.Item(lTSMemberFieldmio))
                    lTSMember.osvbas = FilterNull(lTSMemberRow.Item(lTSMemberFieldosvbas))
                    lTSMember.osvoff = FilterNull(lTSMemberRow.Item(lTSMemberFieldosvoff))
                    lTSMember.eunits = FilterNull(lTSMemberRow.Item(lTSMemberFieldeunits), " ")
                    lTSMember.ltval1 = FilterNull(lTSMemberRow.Item(lTSMemberFieldltval1))
                    lTSMember.ltval2 = FilterNull(lTSMemberRow.Item(lTSMemberFieldltval2))
                    lTSMember.ltval3 = FilterNull(lTSMemberRow.Item(lTSMemberFieldltval3))
                    lTSMember.ltval4 = FilterNull(lTSMemberRow.Item(lTSMemberFieldltval4))
                    lTSMember.defn = FilterNull(lTSMemberRow.Item(lTSMemberFielddefn), " ")
                    lTSMember.munits = FilterNull(lTSMemberRow.Item(lTSMemberFieldmunits), " ")
                    lTSMember.ltval5 = FilterNull(lTSMemberRow.Item(lTSMemberFieldltval5))
                    lTSMember.ltval6 = FilterNull(lTSMemberRow.Item(lTSMemberFieldltval6))
                    lTSMember.ltval7 = FilterNull(lTSMemberRow.Item(lTSMemberFieldltval7))
                    lTSMember.ltval8 = FilterNull(lTSMemberRow.Item(lTSMemberFieldltval8))
                    lTSMembers.Add(lTSMember, lTSMember.Name)
                End If
            Next
            lTSGroup.MemberDefs = lTSMembers
            pTSGroupDefs.Add(lTSGroup, CStr(lTSGroup.Id))
        Next
        Logger.Dbg("HSPFMsg:Open Finished")
    End Sub

    Public Property Name() As String
        Get
            Return pMsgFileName
        End Get
        Set(ByVal newValue As String)
            pMsgFileName = newValue
        End Set
    End Property

    Public ReadOnly Property BlockDefs() As Collection
        Get 'of HspfBlockDef
            Return pBlockDefs
        End Get
    End Property

    Public ReadOnly Property TSGroupDefs() As Collection
        Get 'of HspfTSGroupDef
            Return pTSGroupDefs
        End Get
    End Property

    Public ReadOnly Property ErrorDescription() As String
        Get
            ErrorDescription = pErrorDescription
            pErrorDescription = ""
        End Get
    End Property

    'Private Function FilterNullOld(ByRef v As Object, Optional ByRef NullReturn As Object = 0) As Object
    '    If IsDBNull(v.value) Then
    '        Return NullReturn
    '    Else
    '        Return v.value
    '    End If
    'End Function

    Private Function FilterNull(ByRef v As Object, Optional ByRef NullReturn As Object = 0) As Object
        If IsDBNull(v) Then
            Return NullReturn
        Else
            Return v
        End If
    End Function

    Private Sub updateParmsMultLines(ByRef blockname As String, ByRef ltable As HspfTableDef)
        Dim i, j As Integer
        Dim lParm As HSPFParmDef

        With ltable
            If blockname = "DURANL" And .Name = "LEVELS" Then
                For i = 1 To 6
                    lParm = New HSPFParmDef
                    lParm.Name = "LEVE" & CStr(15 + i) 'Name
                    lParm.Typ = 2 ' ATCoSng
                    lParm.StartCol = 76 + (i * 5)
                    lParm.Length = 5
                    lParm.Min = -999
                    lParm.Max = -999
                    lParm.DefaultValue = 0
                    lParm.Other = lParm.StartCol & ":" & lParm.Length
                    lParm.Define = "LEVEL(2thru21) contains the 20 possible user-specified levels for which the input time series will be analyzed."
                    .ParmDefs.Add(lParm, lParm.Name)
                Next i
            ElseIf blockname = "DURANL" And .Name = "LCONC" Then
                For i = 1 To 3 'three fields to tack on
                    lParm = New HSPFParmDef
                    lParm.Name = "LCONC" & CStr(7 + i) 'Name
                    lParm.Typ = 2 ' ATCoSng
                    lParm.StartCol = 71 + (i * 10)
                    lParm.Length = 10
                    lParm.Min = -999
                    lParm.Max = -999
                    lParm.DefaultValue = 0
                    lParm.Other = lParm.StartCol & ":" & lParm.Length
                    lParm.Define = ""
                    .ParmDefs.Add(lParm, lParm.Name)
                Next i
            ElseIf blockname = "PERLND" And .Name = "IRRIG-SCHED" Then
                For i = 2 To 10 'up to 10 rows possible
                    lParm = New HSPFParmDef
                    lParm.Name = "IRYR" & CStr((2 * (i - 1)) + 1) 'year
                    lParm.Typ = 2 ' ATCoSng
                    lParm.StartCol = (70 * (i - 1)) + 12
                    lParm.Length = 4
                    lParm.Min = 0
                    lParm.Max = -999
                    lParm.DefaultValue = 0
                    lParm.Other = lParm.StartCol & ":" & lParm.Length
                    lParm.Define = ""
                    .ParmDefs.Add(lParm, lParm.Name)
                    lParm = New HSPFParmDef
                    lParm.Name = "IRMO" & CStr((2 * (i - 1)) + 1) 'month
                    lParm.Typ = 2 ' ATCoSng
                    lParm.StartCol = (70 * (i - 1)) + 17
                    lParm.Length = 2
                    lParm.Min = 1
                    lParm.Max = 12
                    lParm.DefaultValue = 1
                    lParm.Other = lParm.StartCol & ":" & lParm.Length
                    lParm.Define = ""
                    .ParmDefs.Add(lParm, lParm.Name)
                    lParm = New HSPFParmDef
                    lParm.Name = "IRDY" & CStr((2 * (i - 1)) + 1) 'day
                    lParm.Typ = 2 ' ATCoSng
                    lParm.StartCol = (70 * (i - 1)) + 20
                    lParm.Length = 2
                    lParm.Min = 1
                    lParm.Max = 31
                    lParm.DefaultValue = 1
                    lParm.Other = lParm.StartCol & ":" & lParm.Length
                    lParm.Define = ""
                    .ParmDefs.Add(lParm, lParm.Name)
                    lParm = New HSPFParmDef
                    lParm.Name = "IRHR" & CStr((2 * (i - 1)) + 1) 'hour
                    lParm.Typ = 2 ' ATCoSng
                    lParm.StartCol = (70 * (i - 1)) + 23
                    lParm.Length = 2
                    lParm.Min = 0
                    lParm.Max = 24
                    lParm.DefaultValue = 0
                    lParm.Other = lParm.StartCol & ":" & lParm.Length
                    lParm.Define = ""
                    .ParmDefs.Add(lParm, lParm.Name)
                    lParm = New HSPFParmDef
                    lParm.Name = "IRMI" & CStr((2 * (i - 1)) + 1) 'min
                    lParm.Typ = 2 ' ATCoSng
                    lParm.StartCol = (70 * (i - 1)) + 26
                    lParm.Length = 2
                    lParm.Min = 0
                    lParm.Max = 60
                    lParm.DefaultValue = 0
                    lParm.Other = lParm.StartCol & ":" & lParm.Length
                    lParm.Define = ""
                    .ParmDefs.Add(lParm, lParm.Name)
                    lParm = New HSPFParmDef
                    lParm.Name = "IRDUR" & CStr((2 * (i - 1)) + 1) 'duration
                    lParm.Typ = 2 ' ATCoSng
                    lParm.StartCol = (70 * (i - 1)) + 28
                    lParm.Length = 5
                    lParm.Min = 0
                    lParm.Max = -999
                    lParm.DefaultValue = 0
                    lParm.Other = lParm.StartCol & ":" & lParm.Length
                    lParm.Define = ""
                    .ParmDefs.Add(lParm, lParm.Name)
                    lParm = New HSPFParmDef
                    lParm.Name = "IRRAT" & CStr((2 * (i - 1)) + 1) 'rate
                    lParm.Typ = 2 ' ATCoSng
                    lParm.StartCol = (70 * (i - 1)) + 33
                    lParm.Length = 10
                    lParm.Min = 0
                    lParm.Max = -999
                    lParm.DefaultValue = 0
                    lParm.Other = lParm.StartCol & ":" & lParm.Length
                    lParm.Define = ""
                    .ParmDefs.Add(lParm, lParm.Name)
                    lParm = New HSPFParmDef
                    lParm.Name = "IRYR" & CStr(2 * i) '2nd year
                    lParm.Typ = 2 ' ATCoSng
                    lParm.StartCol = (70 * (i - 1)) + 49
                    lParm.Length = 4
                    lParm.Min = 0
                    lParm.Max = -999
                    lParm.DefaultValue = 0
                    lParm.Other = lParm.StartCol & ":" & lParm.Length
                    lParm.Define = ""
                    .ParmDefs.Add(lParm, lParm.Name)
                    lParm = New HSPFParmDef
                    lParm.Name = "IRMO" & CStr(2 * i) 'month
                    lParm.Typ = 2 ' ATCoSng
                    lParm.StartCol = (70 * (i - 1)) + 54
                    lParm.Length = 2
                    lParm.Min = 1
                    lParm.Max = 12
                    lParm.DefaultValue = 1
                    lParm.Other = lParm.StartCol & ":" & lParm.Length
                    lParm.Define = ""
                    .ParmDefs.Add(lParm, lParm.Name)
                    lParm = New HSPFParmDef
                    lParm.Name = "IRDY" & CStr(2 * i) 'day
                    lParm.Typ = 2 ' ATCoSng
                    lParm.StartCol = (70 * (i - 1)) + 57
                    lParm.Length = 2
                    lParm.Min = 1
                    lParm.Max = 31
                    lParm.DefaultValue = 1
                    lParm.Other = lParm.StartCol & ":" & lParm.Length
                    lParm.Define = ""
                    .ParmDefs.Add(lParm, lParm.Name)
                    lParm = New HSPFParmDef
                    lParm.Name = "IRHR" & CStr(2 * i) 'hour
                    lParm.Typ = 2 ' ATCoSng
                    lParm.StartCol = (70 * (i - 1)) + 60
                    lParm.Length = 2
                    lParm.Min = 0
                    lParm.Max = 24
                    lParm.DefaultValue = 0
                    lParm.Other = lParm.StartCol & ":" & lParm.Length
                    lParm.Define = ""
                    .ParmDefs.Add(lParm, lParm.Name)
                    lParm = New HSPFParmDef
                    lParm.Name = "IRMI" & CStr(2 * i) 'min
                    lParm.Typ = 2 ' ATCoSng
                    lParm.StartCol = (70 * (i - 1)) + 63
                    lParm.Length = 2
                    lParm.Min = 0
                    lParm.Max = 60
                    lParm.DefaultValue = 0
                    lParm.Other = lParm.StartCol & ":" & lParm.Length
                    lParm.Define = ""
                    .ParmDefs.Add(lParm, lParm.Name)
                    lParm = New HSPFParmDef
                    lParm.Name = "IRDUR" & CStr(2 * i) 'duration
                    lParm.Typ = 2 ' ATCoSng
                    lParm.StartCol = (70 * (i - 1)) + 65
                    lParm.Length = 5
                    lParm.Min = 0
                    lParm.Max = -999
                    lParm.DefaultValue = 0
                    lParm.Other = lParm.StartCol & ":" & lParm.Length
                    lParm.Define = ""
                    .ParmDefs.Add(lParm, lParm.Name)
                    lParm = New HSPFParmDef
                    lParm.Name = "IRRAT" & CStr(2 * i) 'rate
                    lParm.Typ = 2 ' ATCoSng
                    lParm.StartCol = (70 * (i - 1)) + 70
                    lParm.Length = 10
                    lParm.Min = 0
                    lParm.Max = -999
                    lParm.DefaultValue = 0
                    lParm.Other = lParm.StartCol & ":" & lParm.Length
                    lParm.Define = ""
                    .ParmDefs.Add(lParm, lParm.Name)
                Next i
            ElseIf blockname = "RCHRES" And .Name = "HT-BED-DELH" Then
                For i = 2 To 14 '100 values needed
                    For j = 1 To 7 '
                        lParm = New HSPFParmDef
                        lParm.Name = "DELH" & CStr((7 * (i - 1)) + j)
                        lParm.Typ = 2 ' ATCoSng
                        lParm.StartCol = (70 * (i - 1)) + 11 + (10 * (j - 1))
                        lParm.Length = 10
                        lParm.Min = -999
                        lParm.Max = -999
                        lParm.DefaultValue = 0
                        lParm.Other = lParm.StartCol & ":" & lParm.Length
                        lParm.Define = ""
                        .ParmDefs.Add(lParm, lParm.Name)
                    Next j
                Next i
                For i = 1 To 2 'two more fields to tack on to make 100
                    lParm = New HSPFParmDef
                    lParm.Name = "DELH" & CStr(98 + i) 'Name
                    lParm.Typ = 2 ' ATCoSng
                    lParm.StartCol = 991 + (10 * (i - 1))
                    lParm.Length = 10
                    lParm.Min = -999
                    lParm.Max = -999
                    lParm.DefaultValue = 0
                    lParm.Other = lParm.StartCol & ":" & lParm.Length
                    lParm.Define = ""
                    .ParmDefs.Add(lParm, lParm.Name)
                Next i
            ElseIf blockname = "RCHRES" And .Name = "HT-BED-DELTT" Then
                For i = 2 To 14 '100 values needed
                    For j = 1 To 7 '
                        lParm = New HSPFParmDef
                        lParm.Name = "DELTT" & CStr((7 * (i - 1)) + j)
                        lParm.Typ = 2 ' ATCoSng
                        lParm.StartCol = (70 * (i - 1)) + 11 + (10 * (j - 1))
                        lParm.Length = 10
                        lParm.Min = -999
                        lParm.Max = -999
                        lParm.DefaultValue = 0
                        lParm.Other = lParm.StartCol & ":" & lParm.Length
                        lParm.Define = ""
                        .ParmDefs.Add(lParm, lParm.Name)
                    Next j
                Next i
                For i = 1 To 2 'two more fields to tack on to make 100
                    lParm = New HSPFParmDef
                    lParm.Name = "DELTT" & CStr(98 + i) 'Name
                    lParm.Typ = 2 ' ATCoSng
                    lParm.StartCol = 991 + (10 * (i - 1))
                    lParm.Length = 10
                    lParm.Min = -999
                    lParm.Max = -999
                    lParm.DefaultValue = 0
                    lParm.Other = lParm.StartCol & ":" & lParm.Length
                    lParm.Define = ""
                    .ParmDefs.Add(lParm, lParm.Name)
                Next i
            ElseIf blockname = "RCHRES" And .Name = "GQ-PHOTPM" Then
                For i = 1 To 7 'seven fields to tack on
                    lParm = New HSPFParmDef
                    lParm.Name = "PHOTPM" & CStr(7 + i) 'Name
                    lParm.Typ = 2 ' ATCoSng
                    lParm.StartCol = 71 + (i * 10)
                    lParm.Length = 10
                    lParm.Min = 0
                    lParm.Max = -999
                    lParm.DefaultValue = 0
                    lParm.Other = lParm.StartCol & ":" & lParm.Length
                    lParm.Define = ""
                    .ParmDefs.Add(lParm, lParm.Name)
                Next i
                For i = 1 To 6 'six more fields to tack on
                    lParm = New HSPFParmDef
                    lParm.Name = "PHOTPM" & CStr(14 + i) 'Name
                    lParm.Typ = 2 ' ATCoSng
                    lParm.StartCol = 141 + (i * 10)
                    lParm.Length = 10
                    If i < 5 Then
                        lParm.Min = 0
                        lParm.Max = -999
                        lParm.DefaultValue = 0
                    ElseIf i = 5 Then
                        lParm.Min = 0.0001
                        lParm.Max = 10
                        lParm.DefaultValue = 1
                    Else
                        lParm.Min = 1
                        lParm.Max = 2
                        lParm.DefaultValue = 1
                    End If
                    lParm.Other = lParm.StartCol & ":" & lParm.Length
                    lParm.Define = ""
                    .ParmDefs.Add(lParm, lParm.Name)
                Next i
            ElseIf blockname = "RCHRES" And .Name = "GQ-ALPHA" Then
                For i = 1 To 7 'seven fields to tack on
                    lParm = New HSPFParmDef
                    lParm.Name = "ALPH" & CStr(7 + i) 'Name
                    lParm.Typ = 2 ' ATCoSng
                    lParm.StartCol = 71 + (i * 10)
                    lParm.Length = 10
                    lParm.Min = 0.00001
                    lParm.Max = -999
                    lParm.DefaultValue = -999
                    lParm.Other = lParm.StartCol & ":" & lParm.Length
                    lParm.Define = ""
                    .ParmDefs.Add(lParm, lParm.Name)
                Next i
                For i = 1 To 4 'four more fields to tack on
                    lParm = New HSPFParmDef
                    lParm.Name = "ALPH" & CStr(14 + i) 'Name
                    lParm.Typ = 2 ' ATCoSng
                    lParm.StartCol = 141 + (i * 10)
                    lParm.Length = 10
                    lParm.Min = 0.00001
                    lParm.Max = -999
                    lParm.DefaultValue = -999
                    lParm.Other = lParm.StartCol & ":" & lParm.Length
                    lParm.Define = ""
                    .ParmDefs.Add(lParm, lParm.Name)
                Next i
            ElseIf blockname = "RCHRES" And .Name = "GQ-GAMMA" Then
                For i = 1 To 7 'seven fields to tack on
                    lParm = New HSPFParmDef
                    lParm.Name = "GAMM" & CStr(7 + i) 'Name
                    lParm.Typ = 2 ' ATCoSng
                    lParm.StartCol = 71 + (i * 10)
                    lParm.Length = 10
                    lParm.Min = 0
                    lParm.Max = -999
                    lParm.DefaultValue = 0
                    lParm.Other = lParm.StartCol & ":" & lParm.Length
                    lParm.Define = ""
                    .ParmDefs.Add(lParm, lParm.Name)
                Next i
                For i = 1 To 4 'four more fields to tack on
                    lParm = New HSPFParmDef
                    lParm.Name = "GAMM" & CStr(14 + i) 'Name
                    lParm.Typ = 2 ' ATCoSng
                    lParm.StartCol = 141 + (i * 10)
                    lParm.Length = 10
                    lParm.Min = 0
                    lParm.Max = -999
                    lParm.DefaultValue = 0
                    lParm.Other = lParm.StartCol & ":" & lParm.Length
                    lParm.Define = ""
                    .ParmDefs.Add(lParm, lParm.Name)
                Next i
            ElseIf blockname = "RCHRES" And .Name = "GQ-DELTA" Then
                For i = 1 To 7 'seven fields to tack on
                    lParm = New HSPFParmDef
                    lParm.Name = "DEL" & CStr(7 + i) 'Name
                    lParm.Typ = 2 ' ATCoSng
                    lParm.StartCol = 71 + (i * 10)
                    lParm.Length = 10
                    lParm.Min = 0
                    lParm.Max = -999
                    lParm.DefaultValue = 0
                    lParm.Other = lParm.StartCol & ":" & lParm.Length
                    lParm.Define = ""
                    .ParmDefs.Add(lParm, lParm.Name)
                Next i
                For i = 1 To 4 'four more fields to tack on
                    lParm = New HSPFParmDef
                    lParm.Name = "DEL" & CStr(14 + i) 'Name
                    lParm.Typ = 2 ' ATCoSng
                    lParm.StartCol = 141 + (i * 10)
                    lParm.Length = 10
                    lParm.Min = 0
                    lParm.Max = -999
                    lParm.DefaultValue = 0
                    lParm.Other = lParm.StartCol & ":" & lParm.Length
                    lParm.Define = ""
                    .ParmDefs.Add(lParm, lParm.Name)
                Next i
            ElseIf blockname = "RCHRES" And .Name = "GQ-CLDFACT" Then
                For i = 1 To 7 'seven fields to tack on
                    lParm = New HSPFParmDef
                    lParm.Name = "KCLD" & CStr(7 + i) 'Name
                    lParm.Typ = 2 ' ATCoSng
                    lParm.StartCol = 71 + (i * 10)
                    lParm.Length = 10
                    lParm.Min = 0
                    lParm.Max = 1
                    lParm.DefaultValue = 0
                    lParm.Other = lParm.StartCol & ":" & lParm.Length
                    lParm.Define = ""
                    .ParmDefs.Add(lParm, lParm.Name)
                Next i
                For i = 1 To 4 'four more fields to tack on
                    lParm = New HSPFParmDef
                    lParm.Name = "KCLD" & CStr(14 + i) 'Name
                    lParm.Typ = 2 ' ATCoSng
                    lParm.StartCol = 141 + (i * 10)
                    lParm.Length = 10
                    lParm.Min = 0
                    lParm.Max = 1
                    lParm.DefaultValue = 0
                    lParm.Other = lParm.StartCol & ":" & lParm.Length
                    lParm.Define = ""
                    .ParmDefs.Add(lParm, lParm.Name)
                Next i
            ElseIf blockname = "RCHRES" And .Name = "GQ-DAUGHTER" Then
                For i = 2 To 3 '3 rows needed
                    For j = 1 To 3 'three values per row
                        lParm = New HSPFParmDef
                        lParm.Name = "ZERO" & CStr(i) & CStr(j)
                        lParm.Typ = 2 ' ATCoSng
                        lParm.StartCol = (70 * (i - 1)) + 11 + (10 * (j - 1))
                        lParm.Length = 10
                        lParm.Min = 0
                        lParm.Max = -999
                        lParm.DefaultValue = 0
                        lParm.Other = lParm.StartCol & ":" & lParm.Length
                        lParm.Define = ""
                        .ParmDefs.Add(lParm, lParm.Name)
                    Next j
                Next i
            ElseIf blockname = "REPORT" And .Name = "REPORT-SRC" Then
                For i = 2 To 25 'up to 25 rows possible
                    lParm = New HSPFParmDef
                    lParm.Name = "SRCID" & CStr(i) 'Name
                    lParm.Typ = 0 ' ATCoTxt
                    lParm.StartCol = (70 * (i - 1)) + 11
                    lParm.Length = 20
                    lParm.DefaultValue = ""
                    lParm.Other = lParm.StartCol & ":" & lParm.Length
                    lParm.Define = ""
                    .ParmDefs.Add(lParm, lParm.Name)
                Next i
            ElseIf blockname = "REPORT" And .Name = "REPORT-CON" Then
                For i = 2 To 20 'up to 20 rows possible
                    lParm = New HSPFParmDef
                    lParm.Name = "CONID" & CStr(i) 'Name
                    lParm.Typ = 0 ' ATCoTxt
                    lParm.StartCol = ((i - 1) * 70) + 11
                    lParm.Length = 20
                    lParm.DefaultValue = ""
                    lParm.Other = lParm.StartCol & ":" & lParm.Length
                    lParm.Define = ""
                    .ParmDefs.Add(lParm, lParm.Name)
                    lParm = New HSPFParmDef
                    lParm.Name = "TRAN" & CStr(i) 'tran
                    lParm.Typ = 0 ' ATCoTxt
                    lParm.StartCol = (70 * (i - 1)) + 32
                    lParm.Length = 4
                    lParm.DefaultValue = "SUM"
                    lParm.Other = lParm.StartCol & ":" & lParm.Length
                    lParm.Define = ""
                    .ParmDefs.Add(lParm, lParm.Name)
                    lParm = New HSPFParmDef
                    lParm.Name = "SIGD" & CStr(i) 'sig digits
                    lParm.Typ = 1 ' ATCoInt
                    lParm.StartCol = (70 * (i - 1)) + 36
                    lParm.Length = 5
                    lParm.Min = 2
                    lParm.Max = 5
                    lParm.DefaultValue = 5
                    lParm.Other = lParm.StartCol & ":" & lParm.Length
                    lParm.Define = ""
                    .ParmDefs.Add(lParm, lParm.Name)
                    lParm = New HSPFParmDef
                    lParm.Name = "DECPLA" & CStr(i) 'dec places
                    lParm.Typ = 1 ' ATCoInt
                    lParm.StartCol = (70 * (i - 1)) + 41
                    lParm.Length = 5
                    lParm.Min = 0
                    lParm.Max = 3
                    lParm.DefaultValue = 2
                    lParm.Other = lParm.StartCol & ":" & lParm.Length
                    lParm.Define = ""
                    .ParmDefs.Add(lParm, lParm.Name)
                Next i
            End If
        End With
    End Sub
End Class