Imports atcUCI
Imports atcData
Imports atcControls
Imports atcUtility
Imports MapWinUtility
Imports System.Collections.ObjectModel

Public Class frmHspfParm

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        Me.Icon = pIcon
        Me.MinimumSize = Me.Size
        Me.MaximumSize = Me.Size

        Dim i As Integer
        Dim vOpn As Object
        Dim lOpn As HspfOperation
        Dim lOpType As HspfOpnBlk
        Dim Desc As String
        Dim lRow As Integer

        With agdStarter
            .Source = New atcControls.atcGridSource
            .Clear()
            .AllowHorizontalScrolling = False
            .AllowNewValidValues = True
            .Visible = True
            .Source.Rows = 0
            .Source.Columns = 2
            .Source.FixedRows = 1
            .Source.CellValue(0, 0) = "Starter Operation"
            .Source.CellValue(0, 1) = "Mapped from HSPFParm Operation"
        End With

        If radioParameter.Checked Then
            agdStarter.Visible = True
        Else
            agdStarter.Visible = False
        End If

        cmdApply.Enabled = False

        For i = 0 To pUCI.OpnBlks.Count - 1
            lOpType = pUCI.OpnBlks(i)
            With agdStarter.Source
                For Each vOpn In lOpType.Ids
                    lRow = .Rows
                    lOpn = vOpn
                    Desc = lOpn.Description
                    .CellValue(lRow, 0) = lOpn.Name & " " & lOpn.Id & " (" & Desc & ")"
                    .CellValue(lRow, 1) = "<none>"
                Next vOpn
            End With
        Next


        agdStarter.SizeAllColumnsToContents()
        agdStarter.Refresh()

        'temporarty fill of list.

    End Sub

    '    Private Sub agdStarter_CommitChange(ByVal ChangeFromRow As Long, ByVal ChangeToRow As Long, ByVal ChangeFromCol As Long, ByVal ChangeToCol As Long)
    '        Dim i As Long
    '        DoLimits()
    '        For i = 1 To agdStarter.rows
    '            If agdStarter.TextMatrix(i, 1) <> "<none>" Then
    '                cmdApply.Enabled = True
    '            End If
    '        Next i
    '    End Sub

    '    Private Sub agdStarter_RowColChange()
    '        DoLimits()
    '    End Sub

    '    Private Sub agdStarter_TextChange(ByVal ChangeFromRow As Long, ByVal ChangeToRow As Long, ByVal ChangeFromCol As Long, ByVal ChangeToCol As Long)
    '        DoLimits()
    '    End Sub

    '    Private Sub cmdApply_Click()
    '        Dim defOpn As HspfOperation
    '        Dim ctemp As String
    '        Dim OpId As Long
    '        Dim OpType As String
    '        Dim Desc As String
    '        Dim lParm As clsHSPFParm
    '        Dim i As Long
    '        Dim j As Long
    '        Dim ilen As Long
    '        Dim ipos As Long

    '        If OptAssign(0) Then
    '            'by parameter
    '            For i = 0 To lstParms.listcount - 1
    '                If lstParms.Selected(i) Then
    '                    lParm = HSPFParms(lstParms.ItemData(i))
    '                    For j = 0 To lstStarter.listcount - 1
    '                        If lstStarter.Selected(j) Then
    '                            'set this parm in starter
    '                            ctemp = lstStarter.List(j)
    '                            OpType = StrRetRem(ctemp)
    '                            OpId = CInt(StrRetRem(ctemp))
    '                            defOpn = defUci.OpnBlks(OpType).operfromid(OpId)
    '                            If defOpn.TableExists(lParm.Table) Then
    '                                defOpn.tables(lParm.Table).Parms(lParm.Parm) = lParm.Value
    '                            Else
    '                                myMsgBox.Show("Table " & lParm.Table & " does not exist in the Starter UCI.", "HSPFParm Linkage Problem", "OK")
    '                            End If
    '                        End If
    '                    Next j
    '                End If
    '            Next i
    '        Else
    '            'by land use
    '            With agdStarter
    '                For i = 1 To .rows
    '                    If .TextMatrix(i, 1) <> "<none>" Then
    '                        ctemp = .TextMatrix(i, 1)
    '                        OpType = StrRetRem(ctemp)
    '                        OpId = CInt(StrRetRem(ctemp))
    '                        ilen = Len(ctemp)
    '                        ipos = InStr(1, ctemp, "(")
    '                        Desc = Mid(ctemp, ipos + 1, ilen - ipos - 1)
    '                        For j = 1 To HSPFParms.Count
    '                            lParm = HSPFParms(j)
    '                            If lParm.OpType = OpType And lParm.Desc = Desc Then
    '                                'update this parameter
    '                                ctemp = .TextMatrix(i, 0)
    '                                OpType = StrRetRem(ctemp)
    '                                OpId = CInt(StrRetRem(ctemp))
    '                                defOpn = defUci.OpnBlks(OpType).operfromid(OpId)
    '                                If defOpn.TableExists(lParm.Table) Then
    '                                    defOpn.tables(lParm.Table).Parms(lParm.Parm) = lParm.Value
    '                                Else
    '                                    myMsgBox.Show("Table " & lParm.Table & " does not exist in the Starter UCI.", "HSPFParm Linkage Problem", "OK")
    '                                End If
    '                            End If
    '                        Next j
    '                    End If
    '                Next i
    '            End With
    '        End If
    '        'now save starter uci file
    '        ChDriveDir(HSPFMain.W_STARTERPATH)
    '        defUci.Save()
    '    End Sub

    '    Private Sub cmdClose_Click()
    '        Unload(Me)
    '    End Sub

    '    Private Sub cmdFile_Click()
    '        Dim ret&, ArrayIds$(), cnt&, i&

    '        If FileExists(BASINSPath & "\modelout", True, False) Then
    '            ChDriveDir(BASINSPath & "\modelout")
    '        End If
    '        CDFile.flags = &H8806&
    '        CDFile.Filter = "HSPFParm Report Files (*.*)"
    '        CDFile.Filename = "*.*"
    '        CDFile.DialogTitle = "Select HSPFParm Report File"
    '        On Error GoTo 50
    '        CDFile.CancelError = True
    '        CDFile.Action = 1
    '        'read file here
    '        ReadReportFile(CDFile.Filename, ret)
    '        If ret = 0 Then
    '            lblFile.Caption = CDFile.Filename
    '            'fill in grid or lists
    '            Me.MousePointer = vbHourglass
    '            RefreshParms()
    '            DefaultGrid()
    '            Me.MousePointer = vbNormal
    '        End If
    '50:     'continue here on cancel
    '    End Sub

    '    Private Sub cmdStart_Click()
    '        StartHSPFParm()
    '    End Sub

    '    Private Sub StartHSPFParm()
    '        Dim HSPFParmEXE$
    '        Dim ff As New ATCoFindFile
    '        '    Dim reg As New ATCoRegistry
    '        '    HSPFParmEXE = reg.RegGetString(HKEY_LOCAL_MACHINE, "SOFTWARE\AQUA TERRA Consultants\HSPFParm\ExePath", "") & "\hspfparm.exe"
    '        '    If Len(Dir(HSPFParmEXE)) = 0 Then HSPFParmEXE = PathNameOnly(App.EXEName) & "\hspfparm.exe"
    '        '    If Len(Dir(HSPFParmEXE)) = 0 Then HSPFParmEXE = CurDir & "\hspfparm.exe"
    '        '    If Len(Dir(HSPFParmEXE)) = 0 Then HSPFParmEXE = "c:\program files\basins\bin\hspfparm.exe"
    '        '    If Len(Dir(HSPFParmEXE)) = 0 Then HSPFParmEXE = "c:\basins\models\HSPF\bin\hspfparm.exe"
    '        '
    '        '    If Len(Dir(HSPFParmEXE)) = 0 Then
    '        '      'hspfparm not in registry
    '        '      On Error GoTo NeverMind
    '        '      CDFile.CancelError = True
    '        '      CDFile.DialogTitle = "Please locate HSPFParm.exe so HSPFParm can be started."
    '        '      CDFile.Filename = "HSPFParm.exe"
    '        '      CDFile.ShowOpen
    '        '      HSPFParmEXE = CDFile.Filename
    '        '    End If
    '        ff.SetDialogProperties("Please locate HSPFParm.exe so HSPFParm can be started", "HSPFParm.exe")
    '        ff.SetRegistryInfo("HSPFParm", "files", "ExePath")
    '        HSPFParmEXE = ff.GetName
    '        If Not FileExists(HSPFParmEXE) Then
    '            DisableAll(True)
    '            myMsgBox.Show("WinHSPF could not find HSPFParm.exe", "Start HSPFParm Problem", "+-&Close")
    '            DisableAll(False)
    '        Else
    '            'Me.Hide
    '            IPC.dbg("Starting HSPFParm... " & HSPFParmEXE)
    '            IPC.StartProcess("HSPFParm", HSPFParmEXE, 0, 864000)
    '            IPC.dbg("Finished Running HSPFParm")
    '            'Me.Show
    '        End If
    'NeverMind:
    '    End Sub


    

    '    Private Sub lstParms_Click()
    '        Dim lopname$
    '        Dim lOpType As HspfOpnBlk
    '        Dim vOpn As Object
    '        Dim lOpn As HspfOperation
    '        Dim tempOpname$
    '        Dim Desc As String
    '        Dim i As Long

    '        If lstParms.SelCount > 0 Then
    '            If lstParms.SelCount > 1 Then
    '                tempOpname = ""
    '                For i = 1 To lstParms.listcount
    '                    If lstParms.Selected(i - 1) Then
    '                        If tempOpname = "" Then
    '                            tempOpname = HSPFParms(lstParms.ItemData(i - 1)).OpType
    '                        ElseIf tempOpname <> HSPFParms(lstParms.ItemData(i - 1)).OpType Then
    '                            'unselect operations if different oper names
    '                            lstParms.Selected(i - 1) = False
    '                        End If
    '                    End If
    '                Next i
    '            End If

    '            lstStarter.Clear()
    '            For i = 1 To lstParms.listcount
    '                If lstParms.Selected(i - 1) Then
    '                    lopname = HSPFParms(lstParms.ItemData(i - 1)).OpType
    '                    If defUci.OpnBlks(lopname).Count > 0 Then
    '                        lOpType = defUci.OpnBlks(lopname)
    '                        For Each vOpn In lOpType.Ids
    '                            lOpn = vOpn
    '                            Desc = lOpn.Description
    '                            lstStarter.AddItem(lOpn.Name & " " & lOpn.Id & " - " & lOpn.Description)
    '                        Next vOpn
    '                        Exit For
    '                    End If
    '                End If
    '            Next i
    '        Else
    '            lstStarter.Clear()
    '        End If
    '    End Sub

    '    Private Sub lstStarter_Click()
    '        cmdApply.Enabled = True
    '    End Sub

    '    Private Sub OptAssign_Click(ByVal Index As Integer)
    '        Dim i As Long
    '        cmdApply.Enabled = False
    '        If OptAssign(0) Then
    '            fraParameter.Visible = True
    '            agdStarter.Visible = False
    '            If lstStarter.SelCount > 0 Then
    '                cmdApply.Enabled = True
    '            End If
    '        Else
    '            agdStarter.Visible = True
    '            fraParameter.Visible = False
    '            For i = 1 To agdStarter.rows
    '                If agdStarter.TextMatrix(i, 1) <> "<none>" Then
    '                    cmdApply.Enabled = True
    '                End If
    '            Next i
    '        End If
    '    End Sub

    '    Private Sub RefreshParms()
    '        Dim vParm As Object, lParm As clsHSPFParm
    '        Dim ctemp As String

    '        lstParms.Clear()
    '        For Each vParm In HSPFParms
    '            lParm = vParm
    '            ctemp = lParm.Parm & " = " & lParm.Value & " (" & lParm.OpType & " " & lParm.OpId
    '            If Len(lParm.Desc) > 0 Then
    '                ctemp = ctemp & " - " & lParm.Desc & ")"
    '            Else
    '                ctemp = ctemp & ")"
    '            End If
    '            If Not InList(ctemp, lstParms) Then
    '                lstParms.AddItem(ctemp)
    '                lstParms.ItemData(lstParms.listcount - 1) = lParm.Id
    '            End If
    '        Next vParm
    '    End Sub

    '    Private Sub DefaultGrid()
    '        Dim lOpType As HspfOpnBlk
    '        Dim vOpn As Object
    '        Dim lOpn As HspfOperation
    '        Dim vParm As Object, lParm As clsHSPFParm
    '        Dim ctemp As String, i&, row&

    '        row = 0
    '        For i = 1 To defUci.OpnBlks.Count
    '            lOpType = defUci.OpnBlks(i)
    '            For Each vOpn In lOpType.Ids
    '                lOpn = vOpn
    '                row = row + 1
    '                For Each vParm In HSPFParms
    '                    lParm = vParm
    '                    If lParm.OpType = lOpn.Name And UCase(lParm.Desc) = UCase(lOpn.Description) Then
    '                        'matching land use name
    '                        ctemp = vParm.OpType & " " & vParm.OpId & " (" & vParm.Desc & ")"
    '                        agdStarter.TextMatrix(row, 1) = ctemp
    '                    End If
    '                Next vParm
    '            Next vOpn
    '        Next i

    '    End Sub

    '    Private Function FindTableName(ByVal lOpType$, ByVal lparmname$) As String
    '        Dim i&, vTable As Object, lTable As HspfTableDef
    '        Dim vParm As Object

    '        FindTableName = ""
    '        For Each vTable In myMsg.BlockDefs(lOpType).TableDefs
    '            lTable = vTable
    '            For Each vParm In lTable.ParmDefs
    '                If vParm.Name = lparmname Then
    '                    FindTableName = lTable.Name
    '                    Exit For
    '                End If
    '            Next vParm
    '            If Len(FindTableName) > 0 Then Exit For
    '        Next vTable
    '    End Function

    '    Private Sub DoLimits()
    '        Dim vHSPFParm As Object, tHSPFParm As clsHSPFParm
    '        Dim ctemp$, listcount&, alist$(), ifound As Boolean
    '        Dim i As Long

    '        With agdStarter
    '            .ClearValues()
    '            If .col = 1 Then
    '                If Not HSPFParms Is Nothing Then
    '                    .ColEditable(1) = True
    '                    .AddValue("<none>")
    '                    listcount = 0
    '                    For Each vHSPFParm In HSPFParms
    '                        tHSPFParm = vHSPFParm
    '                        If tHSPFParm.OpType = Mid(.TextMatrix(.row, 0), 1, 6) Then
    '                            ctemp = tHSPFParm.OpType & " " & tHSPFParm.OpId & " (" & tHSPFParm.Desc & ")"
    '                            If listcount = 0 Then
    '                                listcount = listcount + 1
    '                                ReDim Preserve alist(listcount)
    '                                alist(listcount) = ctemp
    '                            Else
    '                                For i = 1 To listcount
    '                                    ifound = False
    '                                    If alist(i) = ctemp Then
    '                                        ifound = True
    '                                    End If
    '                                Next i
    '                                If ifound = False Then
    '                                    listcount = listcount + 1
    '                                    ReDim Preserve alist(listcount)
    '                                    alist(listcount) = ctemp
    '                                End If
    '                            End If
    '                        End If
    '                    Next vHSPFParm
    '                    For i = 1 To listcount
    '                        .AddValue(alist(i))
    '                    Next i
    '                End If
    '            End If
    '        End With
    '    End Sub

    Private Sub cmdFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdFile.Click
        Dim ArrayIds$(), cnt&, i&
        Dim lFileName As String
        Dim ret As Integer


        Try
            OpenFileDialog1.InitialDirectory = System.Reflection.Assembly.GetEntryAssembly.Location
            OpenFileDialog1.Filter = "HSPFParm Report Files | *.prn"
            OpenFileDialog1.FileName = "*.*"
            OpenFileDialog1.Title = "Select HSPFParm Report File"

            If OpenFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                lFileName = OpenFileDialog1.FileName
            Else
                Exit Try
            End If

            '.net conversion note: ReadReportFile function now included in this sub
            Dim lstr$, i&, ilen&, havedesc As Boolean, j&
            Dim lParm$, loptyp$, lOpId$, lDesc$, lValue$
            Dim dOpn As HspfOperation, lTable$
            Dim lparmname$(), lTabledef As HspfTableDef
            Dim tHSPFParm As clsHSPFParm
            Dim istart As Long
            Dim nparms As Long

            HspfParms = New Collection 'of type hspfparm

            ret = 0
            i = FreeFile(0)
            On Error GoTo ErrHandler
      Open Filename For Input As #i
            Do Until EOF(i)
        Line Input #i, lstr
                lstr = Trim(lstr)
                ilen = Len(lstr)
                If ilen > 8 Then
                    If Mid(lstr, 1, 9) = "Parameter" Then
                        'process these parameter records
            Line Input #i, lstr 'blank line
            Line Input #i, lstr 'header line
                        lstr = Trim(lstr)
                        ilen = Len(lstr)
                        If ilen > 50 Then
                            'have operation description
                            havedesc = True
                        Else
                            havedesc = False
                        End If
            Line Input #i, lstr 'parameter line
                        Do Until Len(Trim(lstr)) = 0
                            tHSPFParm = New clsHSPFParm
                            tHSPFParm.Parm = StrRetRem(lstr)
                            tHSPFParm.Value = StrRetRem(lstr)
                            tHSPFParm.OpType = StrRetRem(lstr)
                            tHSPFParm.OpId = StrRetRem(lstr)
                            tHSPFParm.Table = FindTableName(tHSPFParm.OpType, tHSPFParm.Parm)
                            If havedesc Then
                                tHSPFParm.Desc = Trim(Mid(lstr, 21))
                            Else
                                tHSPFParm.Desc = ""
                            End If
                            tHSPFParm.Id = HspfParms.Count + 1
                            HspfParms.Add(tHSPFParm)
              Line Input #i, lstr 'parameter line
                        Loop
                    End If
                End If
                If ilen > 5 Then
                    If Mid(lstr, 1, 5) = "Table" Then
                        'process these table records
                        lTable = Trim(Mid(lstr, 7))
            Line Input #i, lstr
                        lstr = Trim(lstr)
                        loptyp = Trim(Mid(lstr, 9))
            Line Input #i, lstr 'blank
            Line Input #i, lstr 'header
                        If Mid(lstr, 21, 4) = "Desc" Then
                            'have operation description
                            havedesc = True
                            istart = 40
                        Else
                            havedesc = False
                            istart = 20
                        End If
                        If Mid(lstr, 21, 5) = "Occur" Or Mid(lstr, 21, 6) = "QUALID" Or Mid(lstr, 21, 4) = "GQID" Then
                            'multiple occurance table, don't do for now
                        Else
                            'ok to continue
                            ilen = Len(Trim(lstr)) - istart

                            'nparms = Int(ilen / 10) + 1   'why assume 10 chars
                            lTabledef = myMsg.BlockDefs(loptyp).TableDefs(lTable)
                            nparms = lTabledef.ParmDefs.Count

                            ReDim lparmname(nparms)
                            lstr = Mid(lstr, istart + 1)
                            For j = 1 To nparms
                                'lparmname(j) = StrRetRem(lstr)
                                lparmname(j) = lTabledef.ParmDefs(j).Name
                            Next j
              Line Input #i, lstr 'data line
                            Do Until Len(Trim(lstr)) = 0
                                lOpId = Mid(lstr, 1, 5)
                                If havedesc Then
                                    lDesc = Mid(lstr, 21, 20)
                                Else
                                    lDesc = ""
                                End If
                                lstr = Mid(lstr, istart + 1)
                                For j = 1 To nparms
                                    tHSPFParm = New clsHSPFParm
                                    tHSPFParm.Parm = lparmname(j)
                                    tHSPFParm.Table = lTable
                                    'tHSPFParm.Value = StrRetRem(lstr)
                                    If lTabledef.ParmDefs(j).typ <> 0 And Len(lstr) >= lTabledef.ParmDefs(j).Length Then
                                        tHSPFParm.Value = Mid(lstr, 1, lTabledef.ParmDefs(j).Length)
                                    End If
                                    lstr = Mid(lstr, lTabledef.ParmDefs(j).Length + 1)
                                    tHSPFParm.OpType = loptyp
                                    tHSPFParm.OpId = lOpId
                                    tHSPFParm.Desc = Trim(lDesc)
                                    tHSPFParm.Id = HspfParms.Count + 1
                                    HspfParms.Add(tHSPFParm)
                                Next j
                Line Input #i, lstr 'data line
                            Loop
                        End If
                    End If
                End If
            Loop
      Close #i
            Exit Sub
            '.net conversion note: end of the essence of the formeer ReadReportfile sub from VB6 code/

            If ret = 0 Then
                lblFile.Text = lFileName
                'fill in grid or lists
                Me.Cursor = Cursors.WaitCursor
                RefreshParms()
                DefaultGrid()
                Me.Cursor = Cursors.Arrow
            End If

        Catch ex As Exception
            Logger.Message("There was a problem loading the file specified." & vbCrLf & "Check the syntax of the file and make sure it is not locked by another process.", "HSPFParm Report File load error", MessageBoxButtons.OK, MessageBoxIcon.Error, Windows.Forms.DialogResult.OK)
        End Try

        '        If FileExists(BASINSPath & "\modelout", True, False) Then
        '            ChDriveDir(BASINSPath & "\modelout")
        '        End If
        '        CDFile.flags = &H8806&
        '        CDFile.Filter = "HSPFParm Report Files (*.*)"
        '        CDFile.Filename = "*.*"
        '        CDFile.DialogTitle = "Select HSPFParm Report File"
        '        On Error GoTo 50
        '        CDFile.CancelError = True
        '        CDFile.Action = 1
        '        'read file here
        '        ReadReportFile(CDFile.Filename, ret)
        '        If ret = 0 Then
        '            lblFile.Caption = CDFile.Filename
        '            'fill in grid or lists
        '            Me.MousePointer = vbHourglass
        '            RefreshParms()
        '            DefaultGrid()
        '            Me.MousePointer = vbNormal
        '        End If
        '50:     'continue here on cancel
    End Sub
End Class