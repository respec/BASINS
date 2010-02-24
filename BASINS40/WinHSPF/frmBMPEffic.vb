Imports MapWinUtility
Imports atcUCI
Imports atcUtility
Imports System.Collections.ObjectModel

Public Class frmBMPEffic

    Private pMsgTitle As String
    Private pOpnID As Integer
    Private pOpnDesc As String
    Private pLoading As Boolean
    Private pBmpDB As atcMDB

    Friend Class HSPFBmpParms
        Friend Name As String
        Friend DBName As String
        Friend TableName As String
        Friend TableNumber As Long
        Friend StartColumn As Long
        Friend Length As Long
        Friend Key As Long
    End Class
    Friend BMPParms As Collection(Of HSPFBmpParms)

    Public Sub Init(ByVal aOperationId As Integer, ByVal aOperationDesc As String)
        pOpnID = aOperationId
        pOpnDesc = aOperationDesc
        lblId.Text = "BMP Operation # " & pOpnID
        cmdUpdateUCI.Enabled = False
    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        Me.Icon = pIcon

        pLoading = True
        pMsgTitle = "Best Management Practices Efficiency Editor"
        Dim lBMPDesc As String = UCase(pOpnDesc)
        lblId.Text = "BMP Operation # " & pOpnID
        cmbBmpName.Items.Clear()
        cmbBmpName.Items.Add("<unknown>")

        Dim lBasinsBinLoc As String = PathNameOnly(System.Reflection.Assembly.GetEntryAssembly.Location)
        Dim lFilename As String = lBasinsBinLoc & "\bmp.mdb"
        If Not IO.File.Exists(lFilename) Then
            lFilename = GetSetting("HSPF", "BMP Database", "Path")
            If Not IO.File.Exists(lFilename) Then
                lFilename = atcUtility.FindFile("Please locate 'bmp.mdb' in a writable directory", "bmp.mdb")
                SaveSetting("HSPF", "BMP Database", "Path", lFilename)
            End If
        End If
        Dim lBmpDB As New atcMDB(lFilename)
        pBmpDB = lBmpDB

        Dim lPracticeTable As DataTable = pBmpDB.GetTable("Practice")
        Dim lPracticeNameField As Integer = lPracticeTable.Columns.IndexOf("Name")

        For Each lPracticeRow As DataRow In lPracticeTable.Rows
            cmbBmpName.Items.Add(lPracticeRow.Item(lPracticeNameField))
            If InStr(lBMPDesc, UCase(cmbBmpName.Items(cmbBmpName.Items.Count - 1))) Then
                cmbBmpName.SelectedIndex = cmbBmpName.Items.Count - 1
            End If
        Next

        Dim lParmTable As DataTable = pBmpDB.GetTable("HSPFParms")
        Dim lParmNameField As Integer = lParmTable.Columns.IndexOf("Name")
        Dim lDBNameField As Integer = lParmTable.Columns.IndexOf("DBName")
        Dim lHSPFTableNameField As Integer = lParmTable.Columns.IndexOf("HSPFTable")
        Dim lTableNumberField As Integer = lParmTable.Columns.IndexOf("TableNumber")
        Dim lStartColumnField As Integer = lParmTable.Columns.IndexOf("StartColumn")
        Dim lLengthField As Integer = lParmTable.Columns.IndexOf("Length")

        BMPParms = New Collection(Of HSPFBmpParms)
        For Each lParmRow As DataRow In lParmTable.Rows
            Dim lParm As New HSPFBmpParms
            lParm.Name = lParmRow.Item(lParmNameField)
            lParm.DBName = lParmRow.Item(lDBNameField)
            lParm.TableName = lParmRow.Item(lHSPFTableNameField)
            lParm.TableNumber = lParmRow.Item(lTableNumberField)
            lParm.StartColumn = lParmRow.Item(lStartColumnField)
            lParm.Length = lParmRow.Item(lLengthField)
            BMPParms.Add(lParm)
        Next

        If cmbBmpName.SelectedIndex < 0 Then
            cmbBmpName.SelectedIndex = 0
        End If

        With agdBmpEfc
            .Source = New atcControls.atcGridSource
            .Clear()
            .AllowHorizontalScrolling = False
            .AllowNewValidValues = True
            .Visible = True
        End With

        pLoading = False
        RefreshGrid()
    End Sub

    Private Sub cmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        Dim lAns As Microsoft.VisualBasic.MsgBoxResult

        If cmdUpdateUCI.Enabled = True Then
            lAns = Logger.Msg("You have changes to your UCI made which have not been saved. " & vbCrLf & _
                              "OK trashes them, Cancel allows you a chance to update your UCI.", _
                              vbOKCancel, pMsgTitle)
        Else 'no changes pending
            lAns = vbOK
        End If

        If lAns = vbOK Then
            Me.Dispose()
        End If
    End Sub

    Private Sub RefreshGrid()
        With agdBmpEfc.Source
            .Rows = 1
            .FixedColumns = 1
            .FixedRows = 1
            .Columns = 2
            .CellValue(0, 0) = "Constituent"
            .CellEditable(0, 0) = False
            .CellValue(0, 1) = "Fraction"
            .CellEditable(0, 1) = False
            If cmbBmpName.SelectedIndex <= 0 Then
                cmbBmpName.SelectedIndex = 0
            Else
                .Columns = 4
                .CellValue(0, 2) = "DB Range"
                .CellEditable(0, 2) = False
                .CellValue(0, 3) = "Reference"
                .CellEditable(0, 3) = False
                '    grdBMPEfc.ColWidth(3) = 0
            End If
            .Rows = BMPParms.Count
            For lIndex As Integer = 1 To BMPParms.Count
                .CellValue(lIndex, 0) = BMPParms(lIndex - 1).Name
                .CellColor(lIndex, 0) = Me.BackColor
            Next
        End With

        RefreshCurrentFromUCI()
        If cmbBmpName.SelectedIndex > 0 Then RefreshRangeFromDB()
        agdBmpEfc.SizeAllColumnsToContents()
        agdBmpEfc.Refresh()
    End Sub

    Private Sub RefreshCurrentFromUCI()
        Dim lOpnBlk As HspfOpnBlk = pUCI.OpnBlks("BMPRAC")
        Dim lOper As HspfOperation = lOpnBlk.OperFromID(pOpnID)

        For i As Integer = 1 To BMPParms.Count
            If Not lOper Is Nothing Then
                Dim lTable As HspfTable = lOper.Tables(BMPParms(i).TableName)
                If Not lTable Is Nothing Then
                    Dim lParmIndex As Integer
                    If lTable.Name = "GQ-FRAC" Or lTable.Name = "CONS-FRAC" Then
                        lParmIndex = Int(BMPParms(i).StartColumn / 10) - 1
                    Else
                        lParmIndex = Int(BMPParms(i).StartColumn / 10)
                    End If
                    Dim lParm As HspfParm = lTable.Parms(lParmIndex)
                    If Not lParm Is Nothing Then
                        agdBmpEfc.Source.CellValue(i, 1) = lParm.Value
                    Else
                        agdBmpEfc.Source.CellValue(i, 1) = 0.0#
                    End If
                Else
                    agdBmpEfc.Source.CellValue(i, 1) = 0.0#
                End If
            Else
                agdBmpEfc.Source.CellValue(i, 1) = 0.0#
            End If
            agdBmpEfc.Source.CellEditable(i, 1) = True
        Next
    End Sub

    Private Sub RefreshRangeFromDB()
        Dim lRangeTable As New DataTable
        lRangeTable = pBmpDB.GetTable("Ranges")
        Dim lConstituentIDField As Integer = lRangeTable.Columns.IndexOf("ConstituentID")
        Dim lPracticeIDField As Integer = lRangeTable.Columns.IndexOf("PracticeID")
        Dim lRangeField As Integer = lRangeTable.Columns.IndexOf("Range")
        Dim lReferenceIDField As Integer = lRangeTable.Columns.IndexOf("ReferenceID")

        For lBmpIndex As Integer = 1 To BMPParms.Count
            Dim lRange As String = "<not available>"
            Dim lRef As String = ""
            If Len(BMPParms(lBmpIndex - 1).DBName) > 0 Then
                For Each lRangeRow As DataRow In lRangeTable.Rows
                    If lRangeRow.Item(lConstituentIDField) = BMPParms(lBmpIndex - 1).DBName And _
                       lRangeRow.Item(lPracticeIDField) = cmbBmpName.SelectedIndex Then
                        lRange = lRangeRow.Item(lRangeField)
                        lRef = lRangeRow.Item(lReferenceIDField)
                        Exit For
                    End If
                Next
            End If
            agdBmpEfc.Source.CellValue(lBmpIndex, 2) = lRange
            agdBmpEfc.Source.CellValue(lBmpIndex, 3) = lRef
        Next
    End Sub

    Private Sub cmbBmpName_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbBmpName.SelectedIndexChanged
        If Not pLoading Then
            RefreshGrid()
        End If
    End Sub

    Private Sub cmdUpdateUCI_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpdateUCI.Click
        Dim lOpnBlk As HspfOpnBlk = pUCI.OpnBlks("BMPRAC")
        Dim lOper As HspfOperation = lOpnBlk.OperFromID(pOpnID)

        For lIndex As Integer = 1 To BMPParms.Count
            If Not lOper Is Nothing Then
                Dim lTable As HspfTable = lOper.Tables(BMPParms(lIndex - 1).TableName)
                If Not lTable Is Nothing Then
                    Dim lParmIndex As Integer = 0
                    If lTable.Name = "GQ-FRAC" Or lTable.Name = "CONS-FRAC" Then
                        lParmIndex = Int(BMPParms(lIndex - 1).StartColumn / 10) - 1
                    Else
                        lParmIndex = Int(BMPParms(lIndex - 1).StartColumn / 10)
                    End If
                    Dim lParm As HspfParm = lTable.Parms(lParmIndex)
                    If Not lParm Is Nothing Then
                        lParm.Value = agdBmpEfc.Source.CellValue(lIndex, 1)
                    End If
                End If
            End If
        Next lIndex

        cmdUpdateUCI.Enabled = False
    End Sub

    Private Sub agdBmpEfc_CellEdited(ByVal aGrid As atcControls.atcGrid, ByVal aRow As Integer, ByVal aColumn As Integer) Handles agdBmpEfc.CellEdited
        cmdUpdateUCI.Enabled = True

        Dim lMinValue As Integer = 0
        Dim lMaxValue As Integer = 1

        Dim lNewValue As String = aGrid.Source.CellValue(aRow, aColumn)
        Dim lNewValueNumeric As Double = -999
        If IsNumeric(lNewValue) Then lNewValueNumeric = CDbl(lNewValue)
        Dim lNewColor As Color = aGrid.Source.CellColor(aRow, aColumn)
        If (lNewValueNumeric >= lMinValue And lMinValue <> -999) AndAlso (lNewValueNumeric <= lMaxValue And lMaxValue <> -999) Then
            lNewColor = aGrid.CellBackColor
        Else
            lNewColor = Color.Pink
        End If
        If Not lNewColor.Equals(aGrid.Source.CellColor(aRow, aColumn)) Then
            aGrid.Source.CellColor(aRow, aColumn) = lNewColor
        End If
        
    End Sub

    Private Sub agdBmpEfc_MouseDownCell(ByVal aGrid As atcControls.atcGrid, ByVal aRow As Integer, ByVal aColumn As Integer) Handles agdBmpEfc.MouseDownCell

        Dim lReferenceTable As DataTable = pBmpDB.GetTable("Reference")
        Dim lIDField As Integer = lReferenceTable.Columns.IndexOf("ID")
        Dim lDetailField As Integer = lReferenceTable.Columns.IndexOf("Detail")

        Dim v As String = agdBmpEfc.Source.CellValue(aRow, 3)
        Dim S As String = "Reference: <not applicable>"
        If Len(v) > 0 Then
            For Each lReferenceRow As DataRow In lReferenceTable.Rows
                If lReferenceRow.Item(lIDField) = agdBmpEfc.Source.CellValue(aRow, 3) Then
                    S = "Reference: " & lReferenceRow.Item(lDetailField)
                    Exit For
                End If
            Next
        End If

        lblReference.Text = S
    End Sub
End Class