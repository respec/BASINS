Imports atcUCI
Imports atcData
Imports atcControls
Imports atcUtility
Imports MapWinUtility
Imports System.Collections.ObjectModel
Imports System.IO

Public Class frmHspfParm

    Public pHspfParms As Collection(Of HSPFParameter)

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

        optParameter.Checked = True
        fraParameter.Visible = True
        agdStarter.Visible = False

        cmdApply.Enabled = False

        For i = 0 To pDefUCI.OpnBlks.Count - 1
            lOpType = pDefUCI.OpnBlks(i)
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

        'make column 1 editable
        For lRowIndex As Integer = 1 To agdStarter.Source.Rows - 1
            agdStarter.Source.CellEditable(lRowIndex, 1) = True
        Next
        agdStarter.SizeAllColumnsToContents()
        agdStarter.Refresh()

    End Sub

    Private Sub optParameter_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles optParameter.CheckedChanged
        RadioButtonChecked()
    End Sub

    Private Sub optLandUse_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles optLandUse.CheckedChanged
        RadioButtonChecked()
    End Sub

    Private Sub RadioButtonChecked()
        cmdApply.Enabled = False
        If optParameter.Checked Then
            fraParameter.Visible = True
            agdStarter.Visible = False
            If lstStarter.SelectedItems.Count > 0 Then
                cmdApply.Enabled = True
            End If
        Else
            agdStarter.Visible = True
            fraParameter.Visible = False
            For lRowIndex As Integer = 1 To agdStarter.Source.Rows - 1
                If agdStarter.Source.CellValue(lRowIndex, 1) <> "<none>" Then
                    cmdApply.Enabled = True
                End If
            Next lRowIndex
        End If
    End Sub

    Private Sub cmdStart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdStart.Click
        Dim lHSPFParmExe As String = FindFile("Please locate HSPFParm.exe so HSPFParm can be started", "HSPFParm.exe")
        If Not IO.File.Exists(lHSPFParmExe) Then
            Logger.Msg("Cannot find HSPFParm.exe", MsgBoxStyle.Critical, "WinHSPF-HSFPParm Problem")
        Else
            Logger.Dbg("Starting HSPFParm... " & lHSPFParmExe)
            Process.Start(lHSPFParmExe)
            Logger.Dbg("Finished Running HSPFParm")
        End If
    End Sub

    Private Sub cmdFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdFile.Click
        OpenFileDialog1.InitialDirectory = PathNameOnly(pUCIFullFileName)
        OpenFileDialog1.Filter = "HSPFParm Report Files (*.*)|*.*"
        OpenFileDialog1.FileName = "*.*"
        OpenFileDialog1.Title = "Select HSPFParm Report File"

        If OpenFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Dim lReturn As Integer = ReadReportFile(OpenFileDialog1.FileName)
            If lReturn = 0 Then
                lblFile.Text = OpenFileDialog1.FileName
                'fill in grid or lists
                Me.Cursor = Cursors.WaitCursor
                RefreshParms()
                DefaultGrid()
                Me.Cursor = Cursors.Default
            End If
        Else
            Exit Sub
        End If
    End Sub

    Private Sub cmdApply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdApply.Click
        If optParameter.Checked Then
            'by parameter
            For lIndex As Integer = 1 To lstParms.SelectedItems.Count
                Dim lParm As HSPFParameter = pHspfParms(lstParms.SelectedIndices(lIndex - 1))
                For Each lSelectedItem As String In lstStarter.SelectedItems
                    'set this parm in starter
                    Dim lOpType As String = StrRetRem(lSelectedItem)
                    Dim lOpId As Integer = CInt(StrRetRem(lSelectedItem))
                    Dim lDefOpn As HspfOperation = pDefUCI.OpnBlks(lOpType).OperFromID(lOpId)
                    If lDefOpn.TableExists(lParm.Table) Then
                        lDefOpn.Tables(lParm.Table).Parms(lParm.Parm).Value = lParm.Value
                    Else
                        Logger.Msg("Table " & lParm.Table & " does not exist in the Starter UCI.", "HSPFParm Linkage Problem")
                    End If
                Next
            Next
        Else
            'by land use
            With agdStarter.Source
                For lRow As Integer = 1 To .Rows - 1
                    If .CellValue(lRow, 1) <> "<none>" Then
                        Dim lTemp As String = .CellValue(lRow, 1)
                        Dim lOpType As String = StrRetRem(lTemp)
                        Dim lOpId As Integer = CInt(StrRetRem(lTemp))
                        Dim lPos As Integer = InStr(1, lTemp, "(")
                        Dim lDesc As String = Mid(lTemp, lPos + 1, lTemp.Length - lPos - 1)
                        For Each lParm As HSPFParameter In pHspfParms
                            If lParm.OpType = lOpType And lParm.Desc = lDesc Then
                                'update this parameter
                                lTemp = .CellValue(lRow, 0)
                                lOpType = StrRetRem(lTemp)
                                lOpId = CInt(StrRetRem(lTemp))
                                Dim lDefOpn As HspfOperation = pDefUCI.OpnBlks(lOpType).OperFromID(lOpId)
                                If lDefOpn.TableExists(lParm.Table) Then
                                    lDefOpn.Tables(lParm.Table).Parms(lParm.Parm).Value = lParm.Value
                                Else
                                    Logger.Msg("Table " & lParm.Table & " does not exist in the Starter UCI.", "HSPFParm Linkage Problem")
                                End If
                            End If
                        Next
                    End If
                Next lRow
            End With
        End If
        'now save starter uci file
        pDefUCI.Save()
    End Sub

    Private Sub cmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        Me.Dispose()
    End Sub

    Private Sub DoLimits(ByVal aSelectedColumn As Integer, ByVal aSelectedRow As Integer)
        With agdStarter
            Dim lValidValues As New Collection
            .AllowNewValidValues = True
            If aSelectedColumn = 1 Then
                If Not pHspfParms Is Nothing Then
                    lValidValues.Add("<none>")
                    For Each lHSPFParm As HSPFParameter In pHspfParms
                        If lHSPFParm.OpType = Mid(.Source.CellValue(aSelectedRow, 0), 1, 6) Then
                            Dim lTemp As String = lHSPFParm.OpType & " " & lHSPFParm.OpId & " (" & lHSPFParm.Desc & ")"
                            Dim lInList As Boolean = False
                            For Each lItem As String In lValidValues
                                If lItem = lTemp Then
                                    lInList = True
                                End If
                            Next
                            If Not lInList Then
                                lValidValues.Add(lTemp)
                            End If
                        End If
                    Next
                End If
            End If
            .ValidValues = lValidValues
            .AllowNewValidValues = False
            .Refresh()
        End With

    End Sub

    Private Function ReadReportFile(ByVal aFilename As String)
        Dim lHaveDesc As Boolean
        Dim lHSPFParm As HSPFParameter
        pHspfParms = New Collection(Of HSPFParameter)
        Dim lReturnCode As Integer = 0

        Try
            Dim lCurrentRecord As String
            Dim lStreamReader As New StreamReader(aFilename)
            Do Until lStreamReader.EndOfStream
                lCurrentRecord = lStreamReader.ReadLine.Trim
                If lCurrentRecord Is Nothing Then
                    Exit Do
                ElseIf lCurrentRecord.Trim.StartsWith("Parameter") Then
                    'process these parameter records
                    lCurrentRecord = lStreamReader.ReadLine.Trim 'blank line
                    lCurrentRecord = lStreamReader.ReadLine.Trim 'header line
                    If lCurrentRecord.Length > 50 Then
                        'have operation description
                        lHaveDesc = True
                    Else
                        lHaveDesc = False
                    End If
                    lCurrentRecord = lStreamReader.ReadLine.Trim 'parameter line
                    Do Until lCurrentRecord.Trim.Length = 0
                        lHSPFParm = New HSPFParameter
                        lHSPFParm.Parm = StrRetRem(lCurrentRecord)
                        lHSPFParm.Value = StrRetRem(lCurrentRecord)
                        lHSPFParm.OpType = StrRetRem(lCurrentRecord)
                        lHSPFParm.OpId = StrRetRem(lCurrentRecord)
                        lHSPFParm.Table = FindTableName(lHSPFParm.OpType, lHSPFParm.Parm)
                        If lHaveDesc Then
                            lHSPFParm.Desc = Trim(Mid(lCurrentRecord, 21))
                        Else
                            lHSPFParm.Desc = ""
                        End If
                        lHSPFParm.Id = pHspfParms.Count + 1
                        pHspfParms.Add(lHSPFParm)
                        lCurrentRecord = lStreamReader.ReadLine.Trim 'parameter line
                    Loop
                ElseIf lCurrentRecord.Trim.StartsWith("Table") Then
                    'process these table records
                    Dim lTable As String = Trim(Mid(lCurrentRecord, 7))
                    lCurrentRecord = lStreamReader.ReadLine.Trim
                    Dim lOpTyp As String = Trim(Mid(lCurrentRecord, 9))
                    lCurrentRecord = lStreamReader.ReadLine.Trim 'blank
                    lCurrentRecord = lStreamReader.ReadLine.Trim 'header
                    Dim lStart As Integer
                    If Mid(lCurrentRecord, 21, 4) = "Desc" Then
                        'have operation description
                        lHaveDesc = True
                        lStart = 40
                    Else
                        lHaveDesc = False
                        lStart = 20
                    End If
                    If Mid(lCurrentRecord, 21, 5) = "Occur" Or Mid(lCurrentRecord, 21, 6) = "QUALID" Or Mid(lCurrentRecord, 21, 4) = "GQID" Then
                        'multiple occurance table, don't do for now
                    Else
                        'ok to continue
                        Dim lTabledef As HspfTableDef = pMsg.BlockDefs(lOpTyp).TableDefs(lTable)
                        Dim lNumParms As Long = lTabledef.ParmDefs.Count
                        Dim lParmName(lNumParms) As String
                        lCurrentRecord = Mid(lCurrentRecord, lStart + 1)
                        For j As Integer = 0 To lNumParms - 1
                            lParmName(j) = lTabledef.ParmDefs(j).Name
                        Next j
                        lCurrentRecord = lStreamReader.ReadLine.Trim 'data line
                        Do Until lCurrentRecord.Trim.Length = 0
                            Dim lOpId As String = Mid(lCurrentRecord, 1, 5)
                            Dim lDesc As String = ""
                            If lHaveDesc Then
                                lDesc = Mid(lCurrentRecord, 21, 20)
                            Else
                                lDesc = ""
                            End If
                            lCurrentRecord = Mid(lCurrentRecord, lStart + 1)
                            For j As Integer = 0 To lNumParms - 1
                                lHSPFParm = New HSPFParameter
                                lHSPFParm.Parm = lParmName(j)
                                lHSPFParm.Table = lTable
                                If lTabledef.ParmDefs(j).Typ <> 0 And lCurrentRecord.Length >= lTabledef.ParmDefs(j).Length Then
                                    If IsNumeric(Mid(lCurrentRecord, 1, lTabledef.ParmDefs(j).Length)) Then
                                        lHSPFParm.Value = Mid(lCurrentRecord, 1, lTabledef.ParmDefs(j).Length)
                                    End If
                                End If
                                lCurrentRecord = Mid(lCurrentRecord, lTabledef.ParmDefs(j).Length + 1)
                                lHSPFParm.OpType = lOpTyp
                                lHSPFParm.OpId = lOpId
                                lHSPFParm.Desc = Trim(lDesc)
                                lHSPFParm.Id = pHspfParms.Count + 1
                                pHspfParms.Add(lHSPFParm)
                            Next j
                            lCurrentRecord = lStreamReader.ReadLine.Trim 'data line
                        Loop
                    End If
                End If
            Loop
        Catch e As ApplicationException
            Logger.Msg("Problem reading file " & aFilename & vbCrLf & e.Message, "HSPFParm Report File Problem")
            lReturnCode = 3
        End Try
        Return lReturnCode

    End Function

    Private Sub RefreshParms()
        lstParms.Items.Clear()
        For Each lParm As HSPFParameter In pHspfParms
            Dim lTemp As String = lParm.Parm & " = " & lParm.Value & " (" & lParm.OpType & " " & lParm.OpId
            If lParm.Desc.Length > 0 Then
                lTemp = lTemp & " - " & lParm.Desc & ")"
            Else
                lTemp = lTemp & ")"
            End If
            lstParms.Items.Add(lTemp)
        Next
    End Sub

    Private Sub DefaultGrid()
        Dim lRow As Integer = 0
        For Each lOpType As HspfOpnBlk In pDefUCI.OpnBlks
            For Each lOpn As HspfOperation In lOpType.Ids
                lRow += 1
                For Each lParm As HSPFParameter In pHspfParms
                    If lParm.OpType = lOpn.Name And lParm.Desc.ToUpper = lOpn.Description.ToUpper Then
                        'matching land use name
                        agdStarter.Source.CellValue(lRow, 1) = lParm.OpType & " " & lParm.OpId & " (" & lParm.Desc & ")"
                    End If
                Next
            Next lOpn
        Next lOpType
    End Sub

    Private Function FindTableName(ByVal aOpType As String, ByVal aParmName As String) As String
        FindTableName = ""
        For Each lTable As HspfTableDef In pMsg.BlockDefs(aOpType).TableDefs
            For Each lParm As HSPFParmDef In lTable.ParmDefs
                If lParm.Name = aParmName Then
                    FindTableName = lTable.Name
                    Exit For
                End If
            Next
            If Len(FindTableName) > 0 Then Exit For
        Next
    End Function

    Private Sub lstParms_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstParms.SelectedIndexChanged
        lstStarter.Items.Clear()
        For lIndex As Integer = 1 To lstParms.SelectedItems.Count
            Dim lOpName As String = pHspfParms(lstParms.SelectedIndices(lIndex - 1)).OpType
            If pDefUCI.OpnBlks(lOpName).Count > 0 Then
                Dim lOpType As HspfOpnBlk = pDefUCI.OpnBlks(lOpName)
                For Each lOpn As HspfOperation In lOpType.Ids
                    lstStarter.Items.Add(lOpn.Name & " " & lOpn.Id & " - " & lOpn.Description)
                Next
                Exit For
            End If
        Next lIndex
    End Sub

    Private Sub lstStarter_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstStarter.SelectedIndexChanged
        cmdApply.Enabled = True
    End Sub

    Private Sub agdStarter_CellEdited(ByVal aGrid As atcControls.atcGrid, ByVal aRow As Integer, ByVal aColumn As Integer) Handles agdStarter.CellEdited
        For lRow As Integer = 1 To agdStarter.Source.Rows - 1
            If agdStarter.Source.CellValue(lRow, 1) <> "<none>" Then
                cmdApply.Enabled = True
            End If
        Next
    End Sub

    Private Sub agdStarter_MouseDownCell(ByVal aGrid As atcControls.atcGrid, ByVal aRow As Integer, ByVal aColumn As Integer) Handles agdStarter.MouseDownCell
        DoLimits(aColumn, aRow)
    End Sub

    Private Sub frmHspfParm_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyValue = Windows.Forms.Keys.F1 Then
            ShowHelp(pWinHSPFManualName)
            ShowHelp("User's Guide\Detailed Functions\HSPFParm.html")
        End If
    End Sub
End Class

Public Class HSPFParameter
    Public Desc As String
    Public Parm As String
    Public Table As String
    Public Value As Single
    Public OpType As String
    Public OpId As Long
    Public Id As Long
End Class