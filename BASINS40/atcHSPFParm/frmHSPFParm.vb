Imports atcUtility
Imports atcMwGisUtility
Imports MapWinUtility
Imports System.Drawing

Public Class frmHSPFParm
    Inherits System.Windows.Forms.Form

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

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        Me.Close()
    End Sub

    Private Sub cmdAbout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAbout.Click
        Logger.Msg("BASINS HSPFParm" & vbCrLf & vbCrLf & "Version 2.0", MsgBoxStyle.OkOnly, "BASINS HSPFParm")
    End Sub

    Private Sub cmdExisting_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub cmdHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdHelp.Click
        ShowHelp("BASINS Details\Watershed and Instream Model Setup\HSPF.html")
    End Sub

    Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdMap.Click

    End Sub

    Public Sub InitializeUI()
        With agdWatershed
            .Source = New atcControls.atcGridSource
            .AllowHorizontalScrolling = False
        End With
        With agdWatershed.Source
            .Columns = 2
            .ColorCells = True
            .FixedRows = 1
            .FixedColumns = 1
            .CellColor(0, 0) = SystemColors.ControlDark
            .CellColor(0, 1) = SystemColors.ControlDark
            .Rows = 25
            .CellValue(0, 0) = "Land Cover"
            .CellValue(0, 1) = "Mannings (Velocity) Coefficient"
            .CellValue(1, 0) = "Urban and Built-Up Land"
            .CellValue(2, 0) = "Dryland Cropland and Pasture"
            .CellValue(3, 0) = "Irrigated Cropland and Pasture"
            .CellValue(4, 0) = "Mixed Dryland/Irrigated Cropland and Pasture"
            .CellValue(5, 0) = "Cropland/Grassland Mosaic"
            .CellValue(6, 0) = "Cropland/Woodland Mosaic"
            .CellValue(7, 0) = "Grassland"
            .CellValue(8, 0) = "Shrubland"
            .CellValue(9, 0) = "Mixed Shrubland/Grassland"
            .CellValue(10, 0) = "Savanna"
            .CellValue(11, 0) = "Deciduous Broadleaf Forest"
            .CellValue(12, 0) = "Deciduous Needleleaf Forest"
            .CellValue(13, 0) = "Evergreen Broadleaf Forest"
            .CellValue(14, 0) = "Evergreen Needleleaf Forest"
            .CellValue(15, 0) = "Mixed Forest"
            .CellValue(16, 0) = "Water Bodies"
            .CellValue(17, 0) = "Herbaceous Wetland"
            .CellValue(18, 0) = "Wooded Wetland"
            .CellValue(19, 0) = "Barren or Sparsely Vegetated"
            .CellValue(20, 0) = "Herbaceous Tundra"
            .CellValue(21, 0) = "Wooded Tundra"
            .CellValue(22, 0) = "Mixed Tundra"
            .CellValue(23, 0) = "Bare Ground Tundra"
            .CellValue(24, 0) = "Snow or Ice"
            .CellValue(1, 1) = "0.03"
            .CellValue(2, 1) = "0.03"
            .CellValue(3, 1) = "0.035"
            .CellValue(4, 1) = "0.033"
            .CellValue(5, 1) = "0.035"
            .CellValue(6, 1) = "0.04"
            .CellValue(7, 1) = "0.05"
            .CellValue(8, 1) = "0.05"
            .CellValue(9, 1) = "0.05"
            .CellValue(10, 1) = "0.06"
            .CellValue(11, 1) = "0.1"
            .CellValue(12, 1) = "0.1"
            .CellValue(13, 1) = "0.12"
            .CellValue(14, 1) = "0.12"
            .CellValue(15, 1) = "0.1"
            .CellValue(16, 1) = "0.035"
            .CellValue(17, 1) = "0.05"
            .CellValue(18, 1) = "0.05"
            .CellValue(19, 1) = "0.03"
            .CellValue(20, 1) = "0.05"
            .CellValue(21, 1) = "0.05"
            .CellValue(22, 1) = "0.05"
            .CellValue(23, 1) = "0.04"
            .CellValue(24, 1) = "0.04"
            For lIndex As Integer = 1 To 24
                .CellColor(lIndex, 0) = SystemColors.ControlDark
                .CellEditable(lIndex, 1) = True
            Next
        End With
        agdWatershed.SizeAllColumnsToContents()
        agdWatershed.Refresh()

        With agdScenario
            .Source = New atcControls.atcGridSource
            .AllowHorizontalScrolling = False
        End With
        With agdScenario.Source
            .Columns = 2
            .ColorCells = True
            .FixedRows = 1
            .FixedColumns = 1
            .CellColor(0, 0) = SystemColors.ControlDark
            .CellColor(0, 1) = SystemColors.ControlDark
            .Rows = 25
            .CellValue(0, 0) = "Land Cover"
            .CellValue(0, 1) = "Mannings (Velocity) Coefficient"
            .CellValue(1, 0) = "Urban and Built-Up Land"
            .CellValue(2, 0) = "Dryland Cropland and Pasture"
            .CellValue(3, 0) = "Irrigated Cropland and Pasture"
            .CellValue(4, 0) = "Mixed Dryland/Irrigated Cropland and Pasture"
            .CellValue(5, 0) = "Cropland/Grassland Mosaic"
            .CellValue(6, 0) = "Cropland/Woodland Mosaic"
            .CellValue(7, 0) = "Grassland"
            .CellValue(8, 0) = "Shrubland"
            .CellValue(9, 0) = "Mixed Shrubland/Grassland"
            .CellValue(10, 0) = "Savanna"
            .CellValue(11, 0) = "Deciduous Broadleaf Forest"
            .CellValue(12, 0) = "Deciduous Needleleaf Forest"
            .CellValue(13, 0) = "Evergreen Broadleaf Forest"
            .CellValue(14, 0) = "Evergreen Needleleaf Forest"
            .CellValue(15, 0) = "Mixed Forest"
            .CellValue(16, 0) = "Water Bodies"
            .CellValue(17, 0) = "Herbaceous Wetland"
            .CellValue(18, 0) = "Wooded Wetland"
            .CellValue(19, 0) = "Barren or Sparsely Vegetated"
            .CellValue(20, 0) = "Herbaceous Tundra"
            .CellValue(21, 0) = "Wooded Tundra"
            .CellValue(22, 0) = "Mixed Tundra"
            .CellValue(23, 0) = "Bare Ground Tundra"
            .CellValue(24, 0) = "Snow or Ice"
            .CellValue(1, 1) = "0.03"
            .CellValue(2, 1) = "0.03"
            .CellValue(3, 1) = "0.035"
            .CellValue(4, 1) = "0.033"
            .CellValue(5, 1) = "0.035"
            .CellValue(6, 1) = "0.04"
            .CellValue(7, 1) = "0.05"
            .CellValue(8, 1) = "0.05"
            .CellValue(9, 1) = "0.05"
            .CellValue(10, 1) = "0.06"
            .CellValue(11, 1) = "0.1"
            .CellValue(12, 1) = "0.1"
            .CellValue(13, 1) = "0.12"
            .CellValue(14, 1) = "0.12"
            .CellValue(15, 1) = "0.1"
            .CellValue(16, 1) = "0.035"
            .CellValue(17, 1) = "0.05"
            .CellValue(18, 1) = "0.05"
            .CellValue(19, 1) = "0.03"
            .CellValue(20, 1) = "0.05"
            .CellValue(21, 1) = "0.05"
            .CellValue(22, 1) = "0.05"
            .CellValue(23, 1) = "0.04"
            .CellValue(24, 1) = "0.04"
            For lIndex As Integer = 1 To 24
                .CellColor(lIndex, 0) = SystemColors.ControlDark
                .CellEditable(lIndex, 1) = True
            Next
        End With
        agdScenario.SizeAllColumnsToContents()
        agdScenario.Refresh()
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
End Class