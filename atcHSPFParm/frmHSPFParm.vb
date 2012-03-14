Imports atcUtility
Imports atcMwGisUtility
Imports MapWinUtility
Imports System.Drawing
Imports System.Data

Public Class frmHSPFParm
    Inherits System.Windows.Forms.Form

    <CLSCompliant(False)> Public Database As atcUCI.atcMDB
#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents cmdAdd As System.Windows.Forms.Button
    Friend WithEvents cmdMap As System.Windows.Forms.Button
    Friend WithEvents cmdHelp As System.Windows.Forms.Button
    Friend WithEvents cmdWrite As System.Windows.Forms.Button
    Friend WithEvents gbxWatershed As System.Windows.Forms.GroupBox
    Friend WithEvents gbxScenario As System.Windows.Forms.GroupBox
    Friend WithEvents gbxSegment As System.Windows.Forms.GroupBox
    Friend WithEvents gbxTable As System.Windows.Forms.GroupBox
    Friend WithEvents gbxValues As System.Windows.Forms.GroupBox
    Friend WithEvents cmdDetails As System.Windows.Forms.Button
    Friend WithEvents agdWatershed As atcControls.atcGrid
    Friend WithEvents agdScenario As atcControls.atcGrid
    Friend WithEvents agdSegment As atcControls.atcGrid
    Friend WithEvents agdTable As atcControls.atcGrid
    Friend WithEvents agdValues As atcControls.atcGrid
    Friend WithEvents cmdAbout As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmHSPFParm))
        Me.cmdMap = New System.Windows.Forms.Button
        Me.cmdAdd = New System.Windows.Forms.Button
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.cmdAbout = New System.Windows.Forms.Button
        Me.cmdWrite = New System.Windows.Forms.Button
        Me.gbxWatershed = New System.Windows.Forms.GroupBox
        Me.agdWatershed = New atcControls.atcGrid
        Me.cmdDetails = New System.Windows.Forms.Button
        Me.gbxScenario = New System.Windows.Forms.GroupBox
        Me.agdScenario = New atcControls.atcGrid
        Me.gbxSegment = New System.Windows.Forms.GroupBox
        Me.agdSegment = New atcControls.atcGrid
        Me.gbxTable = New System.Windows.Forms.GroupBox
        Me.agdTable = New atcControls.atcGrid
        Me.gbxValues = New System.Windows.Forms.GroupBox
        Me.agdValues = New atcControls.atcGrid
        Me.gbxWatershed.SuspendLayout()
        Me.gbxScenario.SuspendLayout()
        Me.gbxSegment.SuspendLayout()
        Me.gbxTable.SuspendLayout()
        Me.gbxValues.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdMap
        '
        Me.cmdMap.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdMap.Location = New System.Drawing.Point(12, 12)
        Me.cmdMap.Name = "cmdMap"
        Me.cmdMap.Size = New System.Drawing.Size(84, 28)
        Me.cmdMap.TabIndex = 2
        Me.cmdMap.Text = "View Map"
        '
        'cmdAdd
        '
        Me.cmdAdd.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAdd.Location = New System.Drawing.Point(102, 12)
        Me.cmdAdd.Name = "cmdAdd"
        Me.cmdAdd.Size = New System.Drawing.Size(84, 28)
        Me.cmdAdd.TabIndex = 5
        Me.cmdAdd.Text = "Add Project"
        '
        'cmdHelp
        '
        Me.cmdHelp.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.Location = New System.Drawing.Point(400, 545)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.Size = New System.Drawing.Size(65, 28)
        Me.cmdHelp.TabIndex = 6
        Me.cmdHelp.Text = "Help"
        '
        'cmdAbout
        '
        Me.cmdAbout.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdAbout.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAbout.Location = New System.Drawing.Point(473, 545)
        Me.cmdAbout.Name = "cmdAbout"
        Me.cmdAbout.Size = New System.Drawing.Size(72, 28)
        Me.cmdAbout.TabIndex = 7
        Me.cmdAbout.Text = "About"
        '
        'cmdWrite
        '
        Me.cmdWrite.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdWrite.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdWrite.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdWrite.Location = New System.Drawing.Point(12, 544)
        Me.cmdWrite.Name = "cmdWrite"
        Me.cmdWrite.Size = New System.Drawing.Size(84, 28)
        Me.cmdWrite.TabIndex = 8
        Me.cmdWrite.Text = "Write to File"
        '
        'gbxWatershed
        '
        Me.gbxWatershed.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbxWatershed.Controls.Add(Me.agdWatershed)
        Me.gbxWatershed.Controls.Add(Me.cmdDetails)
        Me.gbxWatershed.Location = New System.Drawing.Point(12, 46)
        Me.gbxWatershed.Name = "gbxWatershed"
        Me.gbxWatershed.Size = New System.Drawing.Size(536, 93)
        Me.gbxWatershed.TabIndex = 9
        Me.gbxWatershed.TabStop = False
        Me.gbxWatershed.Text = "Watershed"
        '
        'agdWatershed
        '
        Me.agdWatershed.AllowHorizontalScrolling = True
        Me.agdWatershed.AllowNewValidValues = False
        Me.agdWatershed.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.agdWatershed.CellBackColor = System.Drawing.SystemColors.Window
        Me.agdWatershed.Fixed3D = False
        Me.agdWatershed.LineColor = System.Drawing.SystemColors.Control
        Me.agdWatershed.LineWidth = 1.0!
        Me.agdWatershed.Location = New System.Drawing.Point(16, 19)
        Me.agdWatershed.Name = "agdWatershed"
        Me.agdWatershed.Size = New System.Drawing.Size(437, 57)
        Me.agdWatershed.Source = Nothing
        Me.agdWatershed.TabIndex = 10
        '
        'cmdDetails
        '
        Me.cmdDetails.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdDetails.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdDetails.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDetails.Location = New System.Drawing.Point(461, 19)
        Me.cmdDetails.Name = "cmdDetails"
        Me.cmdDetails.Size = New System.Drawing.Size(59, 22)
        Me.cmdDetails.TabIndex = 9
        Me.cmdDetails.Text = "Details"
        '
        'gbxScenario
        '
        Me.gbxScenario.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbxScenario.Controls.Add(Me.agdScenario)
        Me.gbxScenario.Location = New System.Drawing.Point(12, 145)
        Me.gbxScenario.Name = "gbxScenario"
        Me.gbxScenario.Size = New System.Drawing.Size(536, 93)
        Me.gbxScenario.TabIndex = 10
        Me.gbxScenario.TabStop = False
        Me.gbxScenario.Text = "Scenario"
        '
        'agdScenario
        '
        Me.agdScenario.AllowHorizontalScrolling = True
        Me.agdScenario.AllowNewValidValues = False
        Me.agdScenario.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.agdScenario.CellBackColor = System.Drawing.SystemColors.Window
        Me.agdScenario.Fixed3D = False
        Me.agdScenario.LineColor = System.Drawing.SystemColors.Control
        Me.agdScenario.LineWidth = 1.0!
        Me.agdScenario.Location = New System.Drawing.Point(16, 19)
        Me.agdScenario.Name = "agdScenario"
        Me.agdScenario.Size = New System.Drawing.Size(504, 57)
        Me.agdScenario.Source = Nothing
        Me.agdScenario.TabIndex = 11
        '
        'gbxSegment
        '
        Me.gbxSegment.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbxSegment.Controls.Add(Me.agdSegment)
        Me.gbxSegment.Location = New System.Drawing.Point(12, 244)
        Me.gbxSegment.Name = "gbxSegment"
        Me.gbxSegment.Size = New System.Drawing.Size(536, 93)
        Me.gbxSegment.TabIndex = 11
        Me.gbxSegment.TabStop = False
        Me.gbxSegment.Text = "Segment"
        '
        'agdSegment
        '
        Me.agdSegment.AllowHorizontalScrolling = True
        Me.agdSegment.AllowNewValidValues = False
        Me.agdSegment.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.agdSegment.CellBackColor = System.Drawing.SystemColors.Window
        Me.agdSegment.Fixed3D = False
        Me.agdSegment.LineColor = System.Drawing.SystemColors.Control
        Me.agdSegment.LineWidth = 1.0!
        Me.agdSegment.Location = New System.Drawing.Point(16, 18)
        Me.agdSegment.Name = "agdSegment"
        Me.agdSegment.Size = New System.Drawing.Size(504, 57)
        Me.agdSegment.Source = Nothing
        Me.agdSegment.TabIndex = 12
        '
        'gbxTable
        '
        Me.gbxTable.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbxTable.Controls.Add(Me.agdTable)
        Me.gbxTable.Location = New System.Drawing.Point(12, 343)
        Me.gbxTable.Name = "gbxTable"
        Me.gbxTable.Size = New System.Drawing.Size(536, 93)
        Me.gbxTable.TabIndex = 12
        Me.gbxTable.TabStop = False
        Me.gbxTable.Text = "Table/Parameter"
        '
        'agdTable
        '
        Me.agdTable.AllowHorizontalScrolling = True
        Me.agdTable.AllowNewValidValues = False
        Me.agdTable.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.agdTable.CellBackColor = System.Drawing.SystemColors.Window
        Me.agdTable.Fixed3D = False
        Me.agdTable.LineColor = System.Drawing.SystemColors.Control
        Me.agdTable.LineWidth = 1.0!
        Me.agdTable.Location = New System.Drawing.Point(16, 18)
        Me.agdTable.Name = "agdTable"
        Me.agdTable.Size = New System.Drawing.Size(504, 57)
        Me.agdTable.Source = Nothing
        Me.agdTable.TabIndex = 13
        '
        'gbxValues
        '
        Me.gbxValues.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbxValues.Controls.Add(Me.agdValues)
        Me.gbxValues.Location = New System.Drawing.Point(12, 442)
        Me.gbxValues.Name = "gbxValues"
        Me.gbxValues.Size = New System.Drawing.Size(536, 93)
        Me.gbxValues.TabIndex = 13
        Me.gbxValues.TabStop = False
        Me.gbxValues.Text = "Values"
        '
        'agdValues
        '
        Me.agdValues.AllowHorizontalScrolling = True
        Me.agdValues.AllowNewValidValues = False
        Me.agdValues.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.agdValues.CellBackColor = System.Drawing.SystemColors.Window
        Me.agdValues.Fixed3D = False
        Me.agdValues.LineColor = System.Drawing.SystemColors.Control
        Me.agdValues.LineWidth = 1.0!
        Me.agdValues.Location = New System.Drawing.Point(16, 18)
        Me.agdValues.Name = "agdValues"
        Me.agdValues.Size = New System.Drawing.Size(504, 57)
        Me.agdValues.Source = Nothing
        Me.agdValues.TabIndex = 14
        '
        'frmHSPFParm
        '
        Me.AcceptButton = Me.cmdMap
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.CancelButton = Me.cmdAdd
        Me.ClientSize = New System.Drawing.Size(560, 584)
        Me.Controls.Add(Me.gbxValues)
        Me.Controls.Add(Me.gbxTable)
        Me.Controls.Add(Me.gbxSegment)
        Me.Controls.Add(Me.gbxScenario)
        Me.Controls.Add(Me.gbxWatershed)
        Me.Controls.Add(Me.cmdWrite)
        Me.Controls.Add(Me.cmdAbout)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdAdd)
        Me.Controls.Add(Me.cmdMap)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Name = "frmHSPFParm"
        Me.Text = "BASINS HSPFParm - Parameter Database for HSPF"
        Me.gbxWatershed.ResumeLayout(False)
        Me.gbxScenario.ResumeLayout(False)
        Me.gbxSegment.ResumeLayout(False)
        Me.gbxTable.ResumeLayout(False)
        Me.gbxValues.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub cmdAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        Me.Close()
    End Sub

    Private Sub cmdAbout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAbout.Click
        Logger.Msg("BASINS HSPFParm" & vbCrLf & vbCrLf & "Version 2.0", MsgBoxStyle.OkOnly, "BASINS HSPFParm")
    End Sub

    Private Sub cmdHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdHelp.Click
        ShowHelp("BASINS Details\Watershed and Instream Model Setup\HSPF.html")
    End Sub

    Private Sub cmdMap_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdMap.Click

    End Sub

    Private Sub cmdWrite_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdWrite.Click

    End Sub

    Public Sub InitializeUI(ByVal aPath As String, ByVal aDBName As String)
        With agdWatershed
            .Source = New atcControls.atcGridSource
            .AllowHorizontalScrolling = False
        End With
        With agdWatershed.Source
            'populate from point dbf
            Dim lPointDbfName As String = aPath & "\projdef.dbf"
            If FileExists(lPointDbfName) Then
                Dim lPointTable As New atcTableDBF
                lPointTable.OpenFile(lPointDbfName)
                Dim lNumCols As Integer = lPointTable.NumFields
                'If lNumCols > 6 Then lNumCols = 6
                .Columns = lNumCols
                .ColorCells = True
                .FixedRows = 1
                .FixedColumns = 0
                lPointTable.MoveFirst()
                For lColIndex As Integer = 0 To lNumCols - 1
                    .CellColor(0, lColIndex) = SystemColors.ControlDark
                    .CellValue(0, lColIndex) = lPointTable.FieldName(lColIndex + 1)
                    If lPointTable.FieldName(lColIndex + 1) = "WATERSNAME" Then
                        .CellValue(0, lColIndex) = "Project Name"
                    End If
                    If lPointTable.FieldName(lColIndex + 1) = "LOCATION" Then
                        .CellValue(0, lColIndex) = "Location"
                    End If
                    If lPointTable.FieldName(lColIndex + 1) = "DRAINAGEAR" Then
                        .CellValue(0, lColIndex) = "Drainage Area"
                    End If
                    If lPointTable.FieldName(lColIndex + 1) = "COMMENTS" Then
                        .CellValue(0, lColIndex) = "Comments"
                    End If
                    If lPointTable.FieldName(lColIndex + 1) = "PHYS" Then
                        .CellValue(0, lColIndex) = "Physiographic Setting"
                    End If
                Next
                .Rows = 1
                Dim lNumRows As Integer = lPointTable.NumRecords
                For lRowIndex As Integer = 1 To lNumRows
                    .Rows += 1
                    For lColIndex As Integer = 0 To lNumCols - 1
                        .CellValue(lRowIndex, lColIndex) = lPointTable.Value(lColIndex + 1)
                    Next
                    lPointTable.MoveNext()
                Next
            End If
        End With
        agdWatershed.SizeAllColumnsToContents()
        agdWatershed.Refresh()

        With agdScenario
            .Source = New atcControls.atcGridSource
            .AllowHorizontalScrolling = False
        End With
        With agdScenario.Source
            .Columns = 4
            .ColorCells = True
            .FixedRows = 1
            .FixedColumns = 0
            .CellValue(0, 0) = "Name"
            .CellValue(0, 1) = "Project Name"
            .CellValue(0, 2) = "ID"
            .CellValue(0, 3) = "WatershedID"
            .Rows = 1
        End With
        agdScenario.SizeAllColumnsToContents()
        agdScenario.Refresh()

        With agdSegment
            .Source = New atcControls.atcGridSource
            .AllowHorizontalScrolling = False
        End With
        With agdSegment.Source
            .Columns = 4
            .ColorCells = True
            .FixedRows = 1
            .FixedColumns = 0
            .CellValue(0, 0) = "Name"
            .CellValue(0, 1) = "Description"
            .CellValue(0, 2) = "Scenario"
            .CellValue(0, 3) = "Project"
            .Rows = 1
        End With
        agdSegment.SizeAllColumnsToContents()
        agdSegment.Refresh()

        'open the database here
        Database = New atcUCI.atcMDB(aDBName)
    End Sub

    Private Sub frmModelSetup_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyValue = Windows.Forms.Keys.F1 Then
            ShowHelp("BASINS Details\Watershed and Instream Model Setup\HSPFParm.html")
        End If
    End Sub

    Private Sub frmHSPFParm_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        Dim lUsableSpace As Single = cmdWrite.Location.Y - gbxWatershed.Top
        gbxWatershed.Height = (lUsableSpace / 5) - 7
        gbxScenario.Height = (lUsableSpace / 5) - 7
        gbxSegment.Height = (lUsableSpace / 5) - 7
        gbxTable.Height = (lUsableSpace / 5) - 7
        gbxValues.Height = (lUsableSpace / 5) - 7
        gbxScenario.Top = gbxWatershed.Top + (lUsableSpace / 5)
        gbxSegment.Top = gbxWatershed.Top + (2 * lUsableSpace / 5)
        gbxTable.Top = gbxWatershed.Top + (3 * lUsableSpace / 5)
        gbxValues.Top = gbxWatershed.Top + (4 * lUsableSpace / 5)
        agdWatershed.Height = gbxWatershed.Height - 36
        agdScenario.Height = gbxScenario.Height - 36
        agdSegment.Height = gbxSegment.Height - 36
        agdTable.Height = gbxTable.Height - 36
        agdValues.Height = gbxValues.Height - 36
    End Sub

    Sub RefreshScenario()

        Dim lCrit As String = ""
        With agdScenario.Source
            .Rows = 1
            For lWatRow As Integer = 1 To agdWatershed.Source.Rows
                If agdWatershed.Source.CellSelected(lWatRow, 0) Then 'list scenarios for this watershed
                    lCrit = "WatershedID = " & agdWatershed.Source.CellValue(lWatRow, 0)
                    Dim lStr As String = "SELECT DISTINCTROW ScenarioData.Name, " & _
                                                            "ScenarioData.ID " & _
                                                            "From ScenarioData " & _
                                                            "WHERE (" & lCrit & ")"
                    Dim lTable As DataTable = Database.GetTable(lStr)
                    For lRow As Integer = 0 To lTable.Rows.Count - 1
                        .Rows += 1
                        .CellValue(.Rows - 1, 0) = lTable.Rows(lRow).Item(0).ToString
                        .CellValue(.Rows - 1, 1) = agdWatershed.Source.CellValue(lWatRow, 1)
                        .CellValue(.Rows - 1, 2) = lTable.Rows(lRow).Item(1).ToString
                        .CellValue(.Rows - 1, 3) = agdWatershed.Source.CellValue(lWatRow, 0)
                    Next
                End If
            Next
        End With

        agdScenario.SizeAllColumnsToContents()
        agdScenario.Refresh()

        RefreshSegment()
    End Sub

    Sub RefreshSegment()

        Dim lCrit As String = ""
        With agdSegment.Source
            .Rows = 1
            For lScenRow As Integer = 1 To agdScenario.Source.Rows
                If agdScenario.Source.CellSelected(lScenRow, 0) Then 'list segments for this scenario
                    lCrit = "ScenarioID = " & agdScenario.Source.CellValue(lScenRow, 3)
                    '                If Len(Filt(2).txt) > 0 Then  'add filter criteria
                    '                    crit = crit & " AND " & Filt(2).txt
                    '                End If
                    Dim lStr As String = "SELECT DISTINCTROW SegData.Name, " & _
                                                            "SegData.Description " & _
                                                            "From SegData " & _
                                                            "WHERE (" & lCrit & ")"
                    Dim lTable As DataTable = Database.GetTable(lStr)
                    For lRow As Integer = 0 To lTable.Rows.Count - 1
                        .Rows += 1
                        .CellValue(.Rows - 1, 0) = lTable.Rows(lRow).Item(0).ToString
                        .CellValue(.Rows - 1, 1) = lTable.Rows(lRow).Item(1).ToString
                        .CellValue(.Rows - 1, 2) = agdScenario.Source.CellValue(lScenRow, 0)
                        .CellValue(.Rows - 1, 3) = agdScenario.Source.CellValue(lScenRow, 1)
                    Next
                End If
            Next
        End With

        agdSegment.SizeAllColumnsToContents()
        agdSegment.Refresh()

        RefreshTable()
        RefreshParm()

    End Sub

    Sub RefreshTable()

        '        Dim selstr$, i&, j&, crit$, dispfg%, lwid&, nrow&, OpTypStr$(3)

        '        MousePointer = vbHourglass

        '        OpTypStr(1) = "PERLND"
        '        OpTypStr(2) = "IMPLND"
        '        OpTypStr(3) = "RCHRES"

        '        'build query based on selected segments
        '        selstr = "SELECT DISTINCTROW ScenTableList.Name, " & _
        '                                    "ScenTableList.TabID, " & _
        '                                    "ScenTableList.OpnTypID " & _
        '                               "FROM ScenTableList WHERE ("
        '        For i = 1 To agdSeg.Rows 'look for selected segments
        '            If agdSeg.Selected(i, 0) Then 'list tables for this segment
        '                dispfg = 1
        '                selstr = selstr & "SegID = " & agdSeg.ItemData(i) & " OR "
        '            End If
        '        Next i

        '        If dispfg = 1 Then 'display tables for selected segments
        '            selstr = Left(selstr, Len(selstr) - 3) & ")"
        '            If Len(Filt(3).txt) > 0 Then  'add filter criteria
        '                selstr = selstr & " AND " & Filt(3).txt
        '            End If
        '            myTab = myDB.OpenRecordset(selstr, dbOpenDynaset)
        '            'set column headers
        '            agdTab.ColTitle(0) = "Name"
        '            agdTab.ColTitle(1) = "Seg Type"
        '            agdTab.ColSelectable(0) = True

        '            agdTab.ClearData()
        '            nrow = 0
        '            With myTab
        '                Do Until .EOF '.NoMatch
        '                    nrow = nrow + 1
        '                    agdTab.Rows = nrow
        '                    agdTab.TextMatrix(nrow, 0) = !Name
        '                    'set width of name field to display all info
        '                    lwid = TextWidth(!Name)
        '                    If lwid > agdTab.ColWidth(0) Then
        '                        agdTab.ColWidth(0) = lwid
        '                    End If
        '                    agdTab.TextMatrix(nrow, 1) = OpTypStr(!OpnTypID)
        '                    'update itemdata, not sure what goes here yet!!!
        '                    agdTab.ItemData(agdTab.Rows) = !TabID
        '                    .MoveNext()
        '                Loop
        '            End With

        '            myTab.Close()
        '            fraTab.Visible = True
        '        Else 'no segments selected for which to display tables
        '            fraTab.Visible = False
        '            fraView.Visible = False
        '            'mnuMain(5).Enabled = False
        '        End If

        '        MousePointer = vbDefault

    End Sub

    Sub RefreshParm()

        '        Dim selstr$, i&, j&, crit$, dispfg%, lwid&, nrow&, OpTypStr$(3)

        '        MousePointer = vbHourglass

        '        'build query based on selected segments
        '        selstr = "SELECT DISTINCTROW ParmTableData.ParmID, " & _
        '                                    "ParmTableData.Name, " & _
        '                                    "ParmTableData.Table, " & _
        '                                    "ParmTableData.TabID " & _
        '                               "FROM ParmTableData WHERE ("
        '        For i = 1 To agdSeg.Rows 'look for selected segments
        '            If agdSeg.Selected(i, 0) Then 'list tables for this segment
        '                dispfg = 1
        '                selstr = selstr & "SegID = " & agdSeg.ItemData(i) & " OR "
        '            End If
        '        Next i

        '        If dispfg = 1 Then 'display parms for selected segments
        '            selstr = RTrim(Left(selstr, Len(selstr) - 3)) & ")"
        '            If Len(Filt(3).txt) > 0 Then  'add filter criteria
        '                selstr = selstr & " AND " & Filt(3).txt
        '            End If
        '            myTab = myDB.OpenRecordset(selstr, dbOpenDynaset)
        '            'set column headers
        '            agdParm.ColTitle(0) = "Name"
        '            agdParm.ColTitle(1) = "Table"
        '            agdParm.ColSelectable(0) = True

        '            agdParm.ClearData()
        '            nrow = 0
        '            With myTab
        '                Do Until .EOF '.NoMatch
        '                    nrow = nrow + 1
        '                    agdParm.Rows = nrow
        '                    agdParm.TextMatrix(nrow, 0) = !Name
        '                    'set width of name field to display all info
        '                    lwid = TextWidth(!Name)
        '                    If lwid > agdParm.ColWidth(0) Then
        '                        agdParm.ColWidth(0) = lwid
        '                    End If
        '                    agdParm.TextMatrix(nrow, 1) = !Table
        '                    'update itemdata
        '                    agdParm.ItemData(nrow) = !ParmID
        '                    .MoveNext()
        '                Loop
        '            End With

        '            myTab.Close()
        '            fraTab.Visible = True
        '        Else 'no segments selected for which to display tables
        '            fraTab.Visible = False
        '            fraView.Visible = False
        '            'mnuMain(5).Enabled = False
        '        End If

        '        MousePointer = vbDefault

    End Sub

    Sub ViewParms(ByVal Pid&)

        '        Dim i&, nrow&, selstr$, lwid&, Alias$, ColHeader$

        '        agdView.Cols = 6
        '        agdView.ColTitle(0) = "Name"
        '        agdView.ColTitle(1) = "Value"
        '        agdView.ColTitle(2) = "Segment"
        '        agdView.ColTitle(3) = "Scenario"
        '        agdView.ColTitle(4) = "Occur"
        '        agdView.ColTitle(5) = "Alias"
        '        agdView.ClearData()
        '        nrow = 0

        '        'build query based on selected segments
        '        selstr = "SELECT DISTINCTROW ParmTableData.SegID, " & _
        '                                    "ParmTableData.OpnTypID, " & _
        '                                    "ParmTableData.Name, " & _
        '                                    "ParmTableData.ParmID, " & _
        '                                    "ParmTableData.Value, " & _
        '                                    "ParmTableData.Table, " & _
        '                                    "ParmTableData.Occur, " & _
        '                                    "ParmTabledata.AliasInfo " & _
        '                 "From ParmTableData " & _
        '                 "WHERE (ParmID = " & Pid & " OR AssocID = " & Pid & ") AND ("
        '        For i = 1 To agdSeg.Rows 'look for selected segments
        '            If agdSeg.Selected(i, 0) Then 'list tables for this segment
        '                selstr = selstr & "SegID = " & agdSeg.ItemData(i) & " OR "
        '            End If
        '        Next i
        '        selstr = RTrim(Left(selstr, Len(selstr) - 3)) & ")"
        '        myTab = myDB.OpenRecordset(selstr, dbOpenDynaset)

        '        With myTab
        '            agdView.Header = "    Parameter " & !Name
        '            fraView.ToolTipText = "HSPF Parameter help from Message File will go here."
        '            Do Until .EOF
        '                nrow = nrow + 1
        '                agdView.Rows = nrow
        '                agdView.TextMatrix(nrow, 0) = !Name
        '                agdView.TextMatrix(nrow, 1) = !Value
        '                agdView.TextMatrix(nrow, 4) = !Occur
        '                If Len(Trim(!AliasInfo)) > 0 Then
        '          Call FillInAlias(!Table, !Occur, !OpnTypID, !SegID, Alias, ColHeader)
        '                    agdView.ColTitle(5) = ColHeader
        '                Else
        '          Alias = ""
        '                End If
        '        agdView.TextMatrix(nrow, 5) = Alias
        '                i = 1
        '                Do While i <= agdSeg.Rows
        '                    If agdSeg.ItemData(i) = !SegID Then
        '                        agdView.TextMatrix(nrow, 2) = agdSeg.TextMatrix(i, 0)
        '                        agdView.TextMatrix(nrow, 3) = agdSeg.TextMatrix(i, 2)
        '                        i = agdSeg.Rows
        '                    End If
        '                    i = i + 1
        '                Loop
        '                .MoveNext()
        '            Loop
        '        End With

        '        myTab.Close()

        '        Call HideLikeCol(agdView, 4)

        '        fraView.Visible = True
        '        mnuMain(5).Enabled = True

    End Sub

    '    Private Sub HideLikeCol(ByVal agd As Object, ByVal scol&)
    '        Dim i&, c1Same As Boolean, c2Same As Boolean

    '        c1Same = True
    '        c2Same = True

    '        If Len(Trim(agd.TextMatrix(1, scol + 1))) > 0 Then
    '            c2Same = False
    '        End If

    '        If agd.Rows > 1 Then
    '            For i = 2 To agd.Rows
    '                If agd.TextMatrix(i, scol) <> agd.TextMatrix(1, scol) Then
    '                    c1Same = False
    '                End If
    '                If Trim(agd.TextMatrix(i, scol + 1)) <> "" Then
    '                    c2Same = False
    '                End If
    '            Next i
    '        End If

    '        If c1Same Then
    '            agd.ColWidth(scol) = 0
    '        End If
    '        If c2Same Then
    '            agd.ColWidth(scol + 1) = 0
    '        End If
    '    End Sub

    Sub ViewTable(ByVal Tid&)

        '        Dim i&, j&, ncol&, nrow&, brow&, crit$, selstr$, PrmID&()
        '        Dim Alias$, ColHeader$

        '        myTab = myDB.OpenRecordset("ParmTableList", dbOpenDynaset)

        '        agdView.ClearData()
        '        agdView.Rows = 1
        '        'build headers for table view
        '        ncol = 3
        '        agdView.ColTitle(0) = "Op Num"
        '        agdView.ColTitle(1) = "Scen"
        '        'set to no width if not applicable???
        '        agdView.ColTitle(2) = "Occur"
        '        agdView.ColTitle(3) = "Alias"
        '        crit = "TabID = " & Tid
        '        With myTab
        '            .FindFirst(crit)
        '            agdView.Header = "    Table " & myTab!TabName
        '            Do Until .NoMatch
        '                ncol = ncol + 1
        '                agdView.ColTitle(ncol) = !Name
        '                ReDim Preserve PrmID(ncol)
        '                PrmID(ncol) = !id
        '                .FindNext(crit)
        '            Loop
        '            .Close()
        '        End With
        '        agdView.Cols = ncol + 1

        '        'build query based on selected table
        '        selstr = "SELECT DISTINCTROW ParmTableData.SegID, " & _
        '                                    "ParmTableData.OpnTypID, " & _
        '                                    "ParmTableData.Name, " & _
        '                                    "ParmTableData.Value, " & _
        '                                    "ParmTableData.ParmID, " & _
        '                                    "ParmTableData.Table, " & _
        '                                    "ParmTableData.Occur, " & _
        '                                    "ParmTabledata.AliasInfo " & _
        '                 "From ParmTableData " & _
        '                 "WHERE (TabID = " & Tid & ")"
        '        myTab = myDB.OpenRecordset(selstr, dbOpenDynaset)

        '        nrow = 0
        '        For i = 1 To agdSeg.Rows 'look for selected segments
        '            If agdSeg.Selected(i, 0) Then 'list table values for this segment
        '                crit = "SegID = " & agdSeg.ItemData(i)
        '                brow = nrow
        '                With myTab
        '                    .FindFirst(crit)
        '                    Do Until .NoMatch
        '                        nrow = brow + !Occur
        '                        If nrow > agdView.Rows Or nrow = 1 Then
        '                            If nrow > 1 Then agdView.Rows = nrow
        '                            agdView.TextMatrix(nrow, 0) = Right(agdSeg.TextMatrix(i, 0), Len(agdSeg.TextMatrix(i, 0)) - 6)
        '                            agdView.TextMatrix(nrow, 1) = agdSeg.TextMatrix(i, 2)
        '                            agdView.TextMatrix(nrow, 2) = !Occur
        '                            If Len(Trim(!AliasInfo)) > 0 Then
        '                Call FillInAlias(!Table, !Occur, !OpnTypID, !SegID, Alias, ColHeader)
        '                                agdView.ColTitle(3) = ColHeader
        '                            Else
        '                Alias = ""
        '                            End If
        '              agdView.TextMatrix(nrow, 3) = Alias
        '                        End If
        '                        For j = 2 To ncol
        '                            If PrmID(j) = !ParmID Then
        '                                agdView.TextMatrix(nrow, j) = !Value
        '                                Exit For
        '                            End If
        '                        Next j
        '                        .FindNext(crit)
        '                    Loop
        '                End With
        '            End If
        '        Next i

        '        myTab.Close()
        '        Call HideLikeCol(agdView, 2)
        '        fraView.Visible = True
        '        mnuMain(5).Enabled = True

    End Sub

    

    '    Private Sub FillInAlias(ByVal Table$, ByVal Occur&, ByVal OpnTypID&, ByVal SegID&, ByVal Alias$, ByVal ColHeader$)
    '        Dim crit$
    '        Dim myParm As Recordset

    '    Alias = ""
    '        crit = "Name = '" & Table & "' AND Occur = " & Occur & " AND OpnTypID = " & OpnTypID
    '        myRec = myDB.OpenRecordset("TableAliasDefn", dbOpenDynaset)
    '        With myRec
    '            .FindFirst(crit)
    '            If .NoMatch And Occur > 1 Then
    '                crit = "Name = '" & Table & "' AND Occur = 1 AND OpnTypID = " & OpnTypID
    '                .FindFirst(crit)
    '            End If
    '            If .NoMatch Then
    '                MsgBox("NO ALIAS")
    '        Alias = "?"
    '            Else
    '                If Len(Trim(!IDVarName)) > 0 Then
    '                    myParm = myDB.OpenRecordset("ParmData", dbOpenDynaset)
    '                    crit = "ParmID = " & myRec!IDVar & " AND SegID = " & SegID & " AND Occur = " & Occur
    '                    myParm.FindFirst(crit)
    '                    If Not myParm.NoMatch Then
    '            Alias = myParm!Value & " "
    '                    End If
    '                    myParm.Close()
    '                    ColHeader = !IDVarName
    '                Else
    '                    ColHeader = "Alias"
    '                End If
    '                If Len(Trim(!AppearName)) > 0 Then
    '          Alias = Alias & !AppearName
    '                End If
    '                If Len(Trim(!SubsKeyName)) > 0 Then
    '          Alias = Alias & " (see " & !SubsKeyName & " for this seg)"
    '                End If
    '            End If
    '            .Close()
    '        End With
    '    End Sub

    '    Private Sub agdParm_Click()
    '        Dim i&, Pid&

    '        MousePointer = vbHourglass

    '        i = 1
    '        Do While i <= agdParm.Rows
    '            If agdParm.Selected(i, 0) Then
    '                Pid = agdParm.ItemData(i)
    '                i = agdParm.Rows
    '            End If
    '            i = i + 1
    '        Loop

    '        Call ViewParms(Pid)

    '        MousePointer = vbDefault

    '    End Sub

    '    Private Sub agdScen_Click()

    '        If agdScen.Col = 0 Then 'update segment list
    '            Call RefreshSegment()
    '        End If

    '    End Sub

    '    Private Sub agdSeg_Click()

    '        If agdSeg.Col = 0 Then 'update table list
    '            Call RefreshTable()
    '            Call RefreshParm()
    '        End If

    '    End Sub

    '    Private Sub agdTab_Click()

    '        Dim i&, Tid&

    '        MousePointer = vbHourglass

    '        i = 1
    '        Do While i <= agdTab.Rows
    '            If agdTab.Selected(i, 0) Then
    '                Tid = agdTab.ItemData(i)
    '                i = agdTab.Rows
    '            End If
    '            i = i + 1
    '        Loop

    '        Call ViewTable(Tid)

    '        MousePointer = vbDefault

    '    End Sub

    '    Private Sub cmdBigReg_Click()
    '        Static sashVOrigLeft&

    '        sashView_Click()
    '        SashVdragging = True
    '        If sashView.Top = 0 Then
    '            sashVOrigLeft = sashV.Left + 180
    '            Call sashV_MouseMove(0, 0, -5000.0#, 0.0#)
    '            cmdBigReg.Caption = "-"
    '        Else
    '            Call sashV_MouseMove(0, 0, sashVOrigLeft - sashV.Left, 0.0#)
    '            cmdBigReg.Caption = "+"
    '        End If
    '        SashVdragging = False
    '        'cmdBigReg.Left = fraView.Width - cmdBigReg.Width - 50

    '    End Sub

    '    Private Sub cmdScen_Click(ByVal Index As Integer) '0:add,1:delete,2:view details,3:filter

    '        Dim i&, crit$, WatName$, UCIFile$, ScenExist As Boolean
    '        Dim WatKeys$()

    '        If Index = 0 Then 'add (hidden on form, avail thru menu, use project add instead
    '            Call Map1.GetSelectedKeys(WatKeys())
    '            If UBound(WatKeys) = 1 Then 'only one project select, continue
    '                On Error GoTo skip
    '                With cmdUCIFile
    '                    .flags = &H1000& 'file must exist
    '                    .Filter = "UCI files (*.uci)|*.uci"
    '                    .ShowOpen()
    '                    UCIFile = FilenameOnly(.Filename)
    '                End With
    '                ScenExist = False
    '                If agdScen.Rows > 1 Then
    '                    i = 1
    '                    Do While Not ScenExist
    '                        If UCase(UCIFile) = UCase(agdScen.TextMatrix(i, 1)) Then
    '                            ScenExist = True
    '                        Else
    '                            i = i + 1
    '                        End If
    '                        If i >= agdScen.Rows Then Exit Do
    '                    Loop
    '                End If
    '                If Not ScenExist Then
    '                    'get project name from database (use DBF in future?)
    '                    myWat = myDB.OpenRecordset("WatershedData", dbOpenDynaset)
    '                    crit = "ID = " & WatKeys(0)
    '                    myWat.FindFirst(crit)
    '                    WatName = myWat!Name
    '                    myWat.Close()
    '                    Call frmScn.BuildScn(WatName, " ", " ", UCIFile, " ", " ", " ", " ", " ", " ", _
    '                                         "11.0", " ", " ", " ", " ", " ")
    '                Else
    '                    MsgBox("This Scenario exists in this project. Delete it first!")
    '                End If
    '            Else
    '                MsgBox("Select one and only one Project to which this Scenario will be added.")
    '            End If
    'skip:       On Error GoTo 0
    '        ElseIf Index = 1 Then 'delete
    '            For i = 1 To agdScen.Rows
    '                If agdScen.Selected(i, 0) Then
    '                    Call frmScn.DeleteScenario(agdScen.ItemData(i))
    '                End If
    '            Next i
    '        ElseIf Index = 2 Then 'view
    '            Load(frmScn)
    '        Else
    '            MsgBox("Cant filter Scenarios yet!")
    '        End If
    '        Call RefreshScenario()

    '    End Sub

    '    Private Sub cmdSeg_Click(ByVal Index As Integer)
    '        Dim i%, b As Boolean

    '        If Index = 0 Then 'filter
    '            If FiltInd <> 2 Then Call frmFilt.ClearFilters()
    '            FiltInd = 2
    '            Call frmFilt.InitFilters(agdSeg)
    '            frmFilt.Show(1)
    '        ElseIf Index = 1 Then 'view
    '            MsgBox("Not Currently Implemented")
    '        Else 'all or none
    '            If Index = 2 Then 'all
    '                b = True
    '            Else
    '                b = False
    '            End If
    '            For i = 1 To agdSeg.Rows
    '                agdSeg.Selected(i, 1) = b
    '            Next i
    '            agdSeg_Click()
    '        End If

    '    End Sub

    '    Private Sub cmdTab_Click()

    '        If FiltInd <> 3 Then Call frmFilt.ClearFilters()
    '        FiltInd = 3
    '        Call frmFilt.InitFilters(agdTab)
    '        frmFilt.Show(1)

    '    End Sub

    '    Private Sub Form_Terminate()

    '        myDB.Close()

    '    End Sub

    '    Private Sub Map1_SelectionChange(ByVal FeatureID As String, ByVal layer As Long, ByVal state As Boolean)

    '        '    Debug.Print "picked point " & FeatureID, layer, state

    '        MousePointer = vbHourglass

    '        Call RefreshScenario()

    '        MousePointer = vbDefault

    '    End Sub

    '    Private Sub mnufile_Click(ByVal Index As Integer)
    '        If Index = 0 Then
    '            Unload(Me)
    '        End If
    '    End Sub

    '    Private Sub mnuHelp_Click(ByVal Index As Integer)
    '        If Index = 0 Then 'about
    '            frmAbout.Show(vbModal)
    '        ElseIf Index = 1 Then 'contents
    '            'WinHelp Me.hwnd, App.HelpFile, HELP_FINDER, CLng(0)
    '            OpenFile(App.HelpFile)
    '        ElseIf Index = 2 Then 'debug
    '            dbg.Show()
    '        End If
    '    End Sub

    '    Private Sub mnuMain_Click(ByVal Index As Integer)
    '        If Index = 5 Then
    '            frmReport.Show()
    '        End If
    '    End Sub

    '    Private Sub mnuProject_Click(ByVal Index As Integer)
    '        With cmdProject
    '            .DialogTitle = "HSPFParm File Open Project"
    '            .CancelError = True
    '            .flags = &H1804& 'not read only
    '            .Filter = "Project files (*.Wat)|*.Wat"
    '            On Error GoTo err
    '            .ShowOpen()
    '        End With
    '        Call frmWat.AddWatFromFile(cmdProject.Filename, False)
    'err:
    '    End Sub

    '    Private Sub mnuScen_Click(ByVal Index As Integer)
    '        Call cmdScen_Click(Index)
    '    End Sub

    '    Private Sub mnuSeg_Click(ByVal Index As Integer)
    '        Call cmdSeg_Click(Index)
    '    End Sub

    '    Private Sub mnuView_Click(ByVal Index As Integer)
    '        If Index = 0 Then
    '            frmView.Show()
    '        Else
    '            MsgBox("View Summary Not Yet Implemented", vbOKOnly, "View")
    '        End If
    '    End Sub

    Private Sub agdWatershed_MouseDownCell(ByVal aGrid As atcControls.atcGrid, ByVal aRow As Integer, ByVal aColumn As Integer) Handles agdWatershed.MouseDownCell
        For lCol As Integer = 0 To agdWatershed.Source.Columns - 1
            If Not agdWatershed.Source.CellSelected(aRow, lCol) Then
                agdWatershed.Source.CellSelected(aRow, lCol) = True
            Else
                agdWatershed.Source.CellSelected(aRow, lCol) = False
            End If
        Next
        Refresh()
        RefreshScenario()
    End Sub

    Private Sub agdScenario_MouseDownCell(ByVal aGrid As atcControls.atcGrid, ByVal aRow As Integer, ByVal aColumn As Integer) Handles agdScenario.MouseDownCell
        For lCol As Integer = 0 To agdScenario.Source.Columns - 1
            If Not agdScenario.Source.CellSelected(aRow, lCol) Then
                agdScenario.Source.CellSelected(aRow, lCol) = True
            Else
                agdScenario.Source.CellSelected(aRow, lCol) = False
            End If
        Next
        Refresh()
        RefreshSegment()
    End Sub
End Class