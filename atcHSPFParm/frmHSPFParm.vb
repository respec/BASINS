Imports atcUtility
Imports atcMwGisUtility
Imports MapWinUtility
Imports System.Drawing
Imports System.Data

Public Class frmHSPFParm
    Inherits System.Windows.Forms.Form

    Friend pTableGridIDs As atcCollection
    Friend pParmGridIDs As atcCollection
    Friend pSegmentGridIDs As atcCollection
    Friend pSelectedSegmentFilters As atcCollection
    Friend pSelectedTableFilters As atcCollection
    Friend WithEvents cmdDeleteWatershed As System.Windows.Forms.Button
    Friend WithEvents cmdAddWatershed As System.Windows.Forms.Button
    Friend WithEvents cmdDeleteScenario As System.Windows.Forms.Button
    Friend WithEvents cmdAddScenario As System.Windows.Forms.Button
    <CLSCompliant(False)> Public Database As atcUtility.atcMDB

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
    Friend WithEvents cmdMap As System.Windows.Forms.Button
    Friend WithEvents cmdHelp As System.Windows.Forms.Button
    Friend WithEvents cmdWrite As System.Windows.Forms.Button
    Friend WithEvents gbxWatershed As System.Windows.Forms.GroupBox
    Friend WithEvents gbxScenario As System.Windows.Forms.GroupBox
    Friend WithEvents gbxSegment As System.Windows.Forms.GroupBox
    Friend WithEvents gbxTable As System.Windows.Forms.GroupBox
    Friend WithEvents gbxValues As System.Windows.Forms.GroupBox
    Friend WithEvents cmdWatershedDetails As System.Windows.Forms.Button
    Friend WithEvents agdWatershed As atcControls.atcGrid
    Friend WithEvents agdScenario As atcControls.atcGrid
    Friend WithEvents agdSegment As atcControls.atcGrid
    Friend WithEvents agdValues As atcControls.atcGrid
    Friend WithEvents cmdScenarioDetails As System.Windows.Forms.Button
    Friend WithEvents cmdNoneSegments As System.Windows.Forms.Button
    Friend WithEvents cmdAllSegments As System.Windows.Forms.Button
    Friend WithEvents cmdFilterSegments As System.Windows.Forms.Button
    Friend WithEvents cmdTableFilter As System.Windows.Forms.Button
    Friend WithEvents rbnParameters As System.Windows.Forms.RadioButton
    Friend WithEvents rbnTables As System.Windows.Forms.RadioButton
    Friend WithEvents agdTable As atcControls.atcGrid
    Friend WithEvents agdParameter As atcControls.atcGrid
    Friend WithEvents cmdAbout As System.Windows.Forms.Button
    Friend WithEvents lblTableParmName As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmHSPFParm))
        Me.cmdMap = New System.Windows.Forms.Button
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.cmdAbout = New System.Windows.Forms.Button
        Me.cmdWrite = New System.Windows.Forms.Button
        Me.gbxWatershed = New System.Windows.Forms.GroupBox
        Me.agdWatershed = New atcControls.atcGrid
        Me.cmdWatershedDetails = New System.Windows.Forms.Button
        Me.gbxScenario = New System.Windows.Forms.GroupBox
        Me.cmdScenarioDetails = New System.Windows.Forms.Button
        Me.agdScenario = New atcControls.atcGrid
        Me.gbxSegment = New System.Windows.Forms.GroupBox
        Me.cmdNoneSegments = New System.Windows.Forms.Button
        Me.cmdAllSegments = New System.Windows.Forms.Button
        Me.cmdFilterSegments = New System.Windows.Forms.Button
        Me.agdSegment = New atcControls.atcGrid
        Me.gbxTable = New System.Windows.Forms.GroupBox
        Me.agdParameter = New atcControls.atcGrid
        Me.rbnParameters = New System.Windows.Forms.RadioButton
        Me.rbnTables = New System.Windows.Forms.RadioButton
        Me.agdTable = New atcControls.atcGrid
        Me.cmdTableFilter = New System.Windows.Forms.Button
        Me.gbxValues = New System.Windows.Forms.GroupBox
        Me.lblTableParmName = New System.Windows.Forms.Label
        Me.agdValues = New atcControls.atcGrid
        Me.cmdDeleteWatershed = New System.Windows.Forms.Button
        Me.cmdAddWatershed = New System.Windows.Forms.Button
        Me.cmdDeleteScenario = New System.Windows.Forms.Button
        Me.cmdAddScenario = New System.Windows.Forms.Button
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
        Me.gbxWatershed.Controls.Add(Me.cmdDeleteWatershed)
        Me.gbxWatershed.Controls.Add(Me.cmdAddWatershed)
        Me.gbxWatershed.Controls.Add(Me.agdWatershed)
        Me.gbxWatershed.Controls.Add(Me.cmdWatershedDetails)
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
        'cmdWatershedDetails
        '
        Me.cmdWatershedDetails.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdWatershedDetails.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdWatershedDetails.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdWatershedDetails.Location = New System.Drawing.Point(461, 19)
        Me.cmdWatershedDetails.Name = "cmdWatershedDetails"
        Me.cmdWatershedDetails.Size = New System.Drawing.Size(59, 22)
        Me.cmdWatershedDetails.TabIndex = 9
        Me.cmdWatershedDetails.Text = "Details"
        '
        'gbxScenario
        '
        Me.gbxScenario.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbxScenario.Controls.Add(Me.cmdDeleteScenario)
        Me.gbxScenario.Controls.Add(Me.cmdAddScenario)
        Me.gbxScenario.Controls.Add(Me.cmdScenarioDetails)
        Me.gbxScenario.Controls.Add(Me.agdScenario)
        Me.gbxScenario.Location = New System.Drawing.Point(12, 145)
        Me.gbxScenario.Name = "gbxScenario"
        Me.gbxScenario.Size = New System.Drawing.Size(536, 93)
        Me.gbxScenario.TabIndex = 10
        Me.gbxScenario.TabStop = False
        Me.gbxScenario.Text = "Scenario"
        '
        'cmdScenarioDetails
        '
        Me.cmdScenarioDetails.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdScenarioDetails.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdScenarioDetails.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdScenarioDetails.Location = New System.Drawing.Point(461, 19)
        Me.cmdScenarioDetails.Name = "cmdScenarioDetails"
        Me.cmdScenarioDetails.Size = New System.Drawing.Size(59, 22)
        Me.cmdScenarioDetails.TabIndex = 12
        Me.cmdScenarioDetails.Text = "Details"
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
        Me.agdScenario.Size = New System.Drawing.Size(437, 57)
        Me.agdScenario.Source = Nothing
        Me.agdScenario.TabIndex = 11
        '
        'gbxSegment
        '
        Me.gbxSegment.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbxSegment.Controls.Add(Me.cmdNoneSegments)
        Me.gbxSegment.Controls.Add(Me.cmdAllSegments)
        Me.gbxSegment.Controls.Add(Me.cmdFilterSegments)
        Me.gbxSegment.Controls.Add(Me.agdSegment)
        Me.gbxSegment.Location = New System.Drawing.Point(12, 244)
        Me.gbxSegment.Name = "gbxSegment"
        Me.gbxSegment.Size = New System.Drawing.Size(536, 93)
        Me.gbxSegment.TabIndex = 11
        Me.gbxSegment.TabStop = False
        Me.gbxSegment.Text = "Segment"
        '
        'cmdNoneSegments
        '
        Me.cmdNoneSegments.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdNoneSegments.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdNoneSegments.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNoneSegments.Location = New System.Drawing.Point(461, 68)
        Me.cmdNoneSegments.Name = "cmdNoneSegments"
        Me.cmdNoneSegments.Size = New System.Drawing.Size(59, 22)
        Me.cmdNoneSegments.TabIndex = 15
        Me.cmdNoneSegments.Text = "None"
        '
        'cmdAllSegments
        '
        Me.cmdAllSegments.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdAllSegments.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdAllSegments.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAllSegments.Location = New System.Drawing.Point(461, 43)
        Me.cmdAllSegments.Name = "cmdAllSegments"
        Me.cmdAllSegments.Size = New System.Drawing.Size(59, 22)
        Me.cmdAllSegments.TabIndex = 14
        Me.cmdAllSegments.Text = "All"
        '
        'cmdFilterSegments
        '
        Me.cmdFilterSegments.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdFilterSegments.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdFilterSegments.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdFilterSegments.Location = New System.Drawing.Point(461, 18)
        Me.cmdFilterSegments.Name = "cmdFilterSegments"
        Me.cmdFilterSegments.Size = New System.Drawing.Size(59, 22)
        Me.cmdFilterSegments.TabIndex = 13
        Me.cmdFilterSegments.Text = "Filter"
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
        Me.agdSegment.Size = New System.Drawing.Size(437, 57)
        Me.agdSegment.Source = Nothing
        Me.agdSegment.TabIndex = 12
        '
        'gbxTable
        '
        Me.gbxTable.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbxTable.Controls.Add(Me.agdParameter)
        Me.gbxTable.Controls.Add(Me.rbnParameters)
        Me.gbxTable.Controls.Add(Me.rbnTables)
        Me.gbxTable.Controls.Add(Me.agdTable)
        Me.gbxTable.Controls.Add(Me.cmdTableFilter)
        Me.gbxTable.Location = New System.Drawing.Point(12, 343)
        Me.gbxTable.Name = "gbxTable"
        Me.gbxTable.Size = New System.Drawing.Size(536, 93)
        Me.gbxTable.TabIndex = 12
        Me.gbxTable.TabStop = False
        Me.gbxTable.Text = "Table/Parameter"
        '
        'agdParameter
        '
        Me.agdParameter.AllowHorizontalScrolling = True
        Me.agdParameter.AllowNewValidValues = False
        Me.agdParameter.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.agdParameter.CellBackColor = System.Drawing.SystemColors.Window
        Me.agdParameter.Fixed3D = False
        Me.agdParameter.LineColor = System.Drawing.SystemColors.Control
        Me.agdParameter.LineWidth = 1.0!
        Me.agdParameter.Location = New System.Drawing.Point(16, 40)
        Me.agdParameter.Name = "agdParameter"
        Me.agdParameter.Size = New System.Drawing.Size(433, 35)
        Me.agdParameter.Source = Nothing
        Me.agdParameter.TabIndex = 19
        '
        'rbnParameters
        '
        Me.rbnParameters.AutoSize = True
        Me.rbnParameters.Location = New System.Drawing.Point(79, 19)
        Me.rbnParameters.Name = "rbnParameters"
        Me.rbnParameters.Size = New System.Drawing.Size(78, 17)
        Me.rbnParameters.TabIndex = 18
        Me.rbnParameters.Text = "Parameters"
        Me.rbnParameters.UseVisualStyleBackColor = True
        '
        'rbnTables
        '
        Me.rbnTables.AutoSize = True
        Me.rbnTables.Checked = True
        Me.rbnTables.Location = New System.Drawing.Point(16, 19)
        Me.rbnTables.Name = "rbnTables"
        Me.rbnTables.Size = New System.Drawing.Size(57, 17)
        Me.rbnTables.TabIndex = 17
        Me.rbnTables.TabStop = True
        Me.rbnTables.Text = "Tables"
        Me.rbnTables.UseVisualStyleBackColor = True
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
        Me.agdTable.Location = New System.Drawing.Point(16, 40)
        Me.agdTable.Name = "agdTable"
        Me.agdTable.Size = New System.Drawing.Size(433, 35)
        Me.agdTable.Source = Nothing
        Me.agdTable.TabIndex = 16
        '
        'cmdTableFilter
        '
        Me.cmdTableFilter.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdTableFilter.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdTableFilter.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdTableFilter.Location = New System.Drawing.Point(461, 18)
        Me.cmdTableFilter.Name = "cmdTableFilter"
        Me.cmdTableFilter.Size = New System.Drawing.Size(59, 22)
        Me.cmdTableFilter.TabIndex = 14
        Me.cmdTableFilter.Text = "Filter"
        '
        'gbxValues
        '
        Me.gbxValues.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbxValues.Controls.Add(Me.lblTableParmName)
        Me.gbxValues.Controls.Add(Me.agdValues)
        Me.gbxValues.Location = New System.Drawing.Point(12, 442)
        Me.gbxValues.Name = "gbxValues"
        Me.gbxValues.Size = New System.Drawing.Size(536, 93)
        Me.gbxValues.TabIndex = 13
        Me.gbxValues.TabStop = False
        Me.gbxValues.Text = "Values"
        '
        'lblTableParmName
        '
        Me.lblTableParmName.AutoSize = True
        Me.lblTableParmName.Location = New System.Drawing.Point(13, 16)
        Me.lblTableParmName.Name = "lblTableParmName"
        Me.lblTableParmName.Size = New System.Drawing.Size(0, 13)
        Me.lblTableParmName.TabIndex = 21
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
        Me.agdValues.Location = New System.Drawing.Point(16, 35)
        Me.agdValues.Name = "agdValues"
        Me.agdValues.Size = New System.Drawing.Size(504, 40)
        Me.agdValues.Source = Nothing
        Me.agdValues.TabIndex = 14
        '
        'cmdDeleteWatershed
        '
        Me.cmdDeleteWatershed.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdDeleteWatershed.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdDeleteWatershed.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDeleteWatershed.Location = New System.Drawing.Point(461, 68)
        Me.cmdDeleteWatershed.Name = "cmdDeleteWatershed"
        Me.cmdDeleteWatershed.Size = New System.Drawing.Size(59, 22)
        Me.cmdDeleteWatershed.TabIndex = 17
        Me.cmdDeleteWatershed.Text = "Delete"
        '
        'cmdAddWatershed
        '
        Me.cmdAddWatershed.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdAddWatershed.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdAddWatershed.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddWatershed.Location = New System.Drawing.Point(461, 43)
        Me.cmdAddWatershed.Name = "cmdAddWatershed"
        Me.cmdAddWatershed.Size = New System.Drawing.Size(59, 22)
        Me.cmdAddWatershed.TabIndex = 16
        Me.cmdAddWatershed.Text = "Add"
        '
        'cmdDeleteScenario
        '
        Me.cmdDeleteScenario.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdDeleteScenario.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdDeleteScenario.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDeleteScenario.Location = New System.Drawing.Point(461, 68)
        Me.cmdDeleteScenario.Name = "cmdDeleteScenario"
        Me.cmdDeleteScenario.Size = New System.Drawing.Size(59, 22)
        Me.cmdDeleteScenario.TabIndex = 17
        Me.cmdDeleteScenario.Text = "Delete"
        '
        'cmdAddScenario
        '
        Me.cmdAddScenario.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdAddScenario.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdAddScenario.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddScenario.Location = New System.Drawing.Point(461, 43)
        Me.cmdAddScenario.Name = "cmdAddScenario"
        Me.cmdAddScenario.Size = New System.Drawing.Size(59, 22)
        Me.cmdAddScenario.TabIndex = 16
        Me.cmdAddScenario.Text = "Add"
        '
        'frmHSPFParm
        '
        Me.AcceptButton = Me.cmdMap
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(560, 584)
        Me.Controls.Add(Me.gbxValues)
        Me.Controls.Add(Me.gbxTable)
        Me.Controls.Add(Me.gbxSegment)
        Me.Controls.Add(Me.gbxScenario)
        Me.Controls.Add(Me.gbxWatershed)
        Me.Controls.Add(Me.cmdWrite)
        Me.Controls.Add(Me.cmdAbout)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdMap)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Name = "frmHSPFParm"
        Me.Text = "BASINS HSPFParm - Parameter Database for HSPF"
        Me.gbxWatershed.ResumeLayout(False)
        Me.gbxScenario.ResumeLayout(False)
        Me.gbxSegment.ResumeLayout(False)
        Me.gbxTable.ResumeLayout(False)
        Me.gbxTable.PerformLayout()
        Me.gbxValues.ResumeLayout(False)
        Me.gbxValues.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub cmdAbout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAbout.Click
        Logger.Msg("BASINS HSPFParm" & vbCrLf & vbCrLf & "Version 2.0", MsgBoxStyle.OkOnly, "BASINS HSPFParm")
    End Sub

    Private Sub cmdHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdHelp.Click
        ShowHelp("BASINS Details\Watershed and Instream Model Setup\HSPF.html")
    End Sub

    Private Sub cmdMap_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdMap.Click
        Logger.Msg("Map option is not yet implemented.  This option will refresh the map with the HSPFParm project locations.", MsgBoxStyle.OkOnly, "BASINS HSPFParm")
        'save the current project and then open the HSPFParm layers on the map
    End Sub

    Private Sub cmdWrite_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdWrite.Click
        Dim lfrmReport As New frmReport
        lfrmReport.Show()
    End Sub

    Public Sub InitializeUI(ByVal aPath As String, ByVal aDBName As String)
        pSelectedSegmentFilters = New atcCollection
        pSelectedTableFilters = New atcCollection

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
                    If lPointTable.FieldName(lColIndex + 1) = "WEATHER" Then
                        .CellValue(0, lColIndex) = "Weather Regime"
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
            .Columns = 3
            .ColorCells = True
            .FixedRows = 1
            .FixedColumns = 0
            .CellValue(0, 0) = "Name"
            .CellValue(0, 1) = "Project Name"
            .CellValue(0, 2) = "ID"
            .Rows = 1
        End With
        agdScenario.SizeAllColumnsToContents()
        agdScenario.Refresh()

        With agdSegment
            .Source = New atcControls.atcGridSource
            .AllowHorizontalScrolling = False
        End With
        With agdSegment.Source
            .Columns = 3
            .ColorCells = True
            .FixedRows = 1
            .FixedColumns = 0
            .CellValue(0, 0) = "Name"
            .CellValue(0, 1) = "Description"
            .CellValue(0, 2) = "Scenario Name"
            .CellValue(0, 3) = "Project Name"
            .Rows = 1
        End With
        agdSegment.SizeAllColumnsToContents()
        agdSegment.Refresh()
        pSegmentGridIDs = New atcCollection

        With agdTable
            .Source = New atcControls.atcGridSource
            .AllowHorizontalScrolling = False
        End With
        With agdTable.Source
            .Columns = 2
            .ColorCells = True
            .FixedRows = 1
            .FixedColumns = 0
            .CellValue(0, 0) = "Name"
            .CellValue(0, 1) = "Segment Type"
            .Rows = 1
        End With
        agdTable.SizeAllColumnsToContents()
        agdTable.Refresh()
        pTableGridIDs = New atcCollection

        With agdParameter
            .Source = New atcControls.atcGridSource
            .AllowHorizontalScrolling = False
        End With
        With agdParameter.Source
            .Columns = 2
            .ColorCells = True
            .FixedRows = 1
            .FixedColumns = 0
            .CellValue(0, 0) = "Name"
            .CellValue(0, 1) = "Table"
            .Rows = 1
        End With
        agdParameter.SizeAllColumnsToContents()
        agdParameter.Refresh()
        pParmGridIDs = New atcCollection

        With agdValues
            .Source = New atcControls.atcGridSource
            .AllowHorizontalScrolling = False
        End With
        With agdValues.Source
            .Rows = 0
            .Columns = 0
        End With
        agdValues.SizeAllColumnsToContents()
        agdValues.Refresh()

        'open the database here
        Database = New atcUtility.atcMDB(aDBName)
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
        agdTable.Height = gbxTable.Height - 58
        agdParameter.Height = gbxTable.Height - 58
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
        pSegmentGridIDs.Clear()
        With agdSegment.Source
            .Rows = 1
            For lScenRow As Integer = 1 To agdScenario.Source.Rows
                If agdScenario.Source.CellSelected(lScenRow, 0) Then 'list segments for this scenario
                    lCrit = "ScenarioID = " & agdScenario.Source.CellValue(lScenRow, 2)
                    If pSelectedSegmentFilters.Count > 0 Then  'add filter criteria
                        Dim lSelectedSegmentFiltersAsIds As New atcCollection
                        For Each lSeg As String In pSelectedSegmentFilters
                            If lSeg = "PERLND" Then
                                lSelectedSegmentFiltersAsIds.Add(1)
                            ElseIf lSeg = "IMPLND" Then
                                lSelectedSegmentFiltersAsIds.Add(2)
                            ElseIf lSeg = "RCHRES" Then
                                lSelectedSegmentFiltersAsIds.Add(3)
                            End If
                        Next
                        Dim lFiltText As String = "OpnTypID = "
                        Dim lFilt As String = ""
                        lFilt = "(" & lFiltText & lSelectedSegmentFiltersAsIds(0)
                        For lIndex As Integer = 1 To lSelectedSegmentFiltersAsIds.Count - 1
                            lFilt = lFilt & " OR " & lFiltText & lSelectedSegmentFiltersAsIds(lIndex)
                        Next
                        lCrit = lCrit & " AND " & lFilt & ")"
                    End If
                    Dim lStr As String = "SELECT DISTINCTROW SegData.ID, " & _
                                                            "SegData.Name, " & _
                                                            "SegData.Description " & _
                                                            "From SegData " & _
                                                            "WHERE (" & lCrit & ")"
                    Dim lTable As DataTable = Database.GetTable(lStr)
                    For lRow As Integer = 0 To lTable.Rows.Count - 1
                        .Rows += 1
                        .CellValue(.Rows - 1, 0) = lTable.Rows(lRow).Item(1).ToString
                        .CellValue(.Rows - 1, 1) = lTable.Rows(lRow).Item(2).ToString
                        .CellValue(.Rows - 1, 2) = agdScenario.Source.CellValue(lScenRow, 0)
                        .CellValue(.Rows - 1, 3) = agdScenario.Source.CellValue(lScenRow, 1)
                        pSegmentGridIDs.Add(lTable.Rows(lRow).Item(0))
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

        Dim lCrit As String = ""
        Dim lOpnTyp As Integer = 0
        pTableGridIDs.Clear()
        With agdTable.Source
            .Rows = 1
            Dim lSelectedCount As Integer = 0
            For lSegRow As Integer = 1 To agdSegment.Source.Rows - 1
                If agdSegment.Source.CellSelected(lSegRow, 0) Then 'list tables for this segment
                    lSelectedCount += 1
                    If lSelectedCount = 1 Then
                        lCrit = "SegID = " & pSegmentGridIDs(lSegRow - 1)
                    Else
                        lCrit = lCrit & " OR SegID = " & pSegmentGridIDs(lSegRow - 1)
                    End If
                End If
            Next
            If lSelectedCount > 0 Then
                If pSelectedTableFilters.Count > 0 Then  'add filter criteria
                    Dim lFiltText As String = "Name = '"
                    Dim lFilt As String = ""
                    lFilt = "(" & lFiltText & Mid(pSelectedTableFilters(0), 1, pSelectedTableFilters(0).length - 2)
                    For lIndex As Integer = 1 To pSelectedTableFilters.Count - 1
                        lFilt = lFilt & "' OR " & lFiltText & Mid(pSelectedTableFilters(lIndex), 1, pSelectedTableFilters(lIndex).length - 2)
                    Next
                    lCrit = lCrit & " AND " & lFilt & "')"
                End If
                Dim lStr As String = "SELECT DISTINCTROW ScenTableList.Name, " & _
                                                        "ScenTableList.TabID, " & _
                                                        "ScenTableList.OpnTypID " & _
                                                        "From ScenTableList " & _
                                                        "WHERE (" & lCrit & ")"
                Dim lTable As DataTable = Database.GetTable(lStr)
                For lRow As Integer = 0 To lTable.Rows.Count - 1
                    .Rows += 1
                    .CellValue(.Rows - 1, 0) = lTable.Rows(lRow).Item(0).ToString
                    lOpnTyp = lTable.Rows(lRow).Item(2).ToString
                    If lOpnTyp = 1 Then
                        .CellValue(.Rows - 1, 1) = "PERLND"
                    ElseIf lOpnTyp = 2 Then
                        .CellValue(.Rows - 1, 1) = "IMPLND"
                    ElseIf lOpnTyp = 3 Then
                        .CellValue(.Rows - 1, 1) = "RCHRES"
                    End If
                    pTableGridIDs.Add(lTable.Rows(lRow).Item(1))
                Next
            End If
        End With

        agdTable.SizeAllColumnsToContents()
        agdTable.Refresh()
        ClearValues()

    End Sub

    Sub RefreshParm()

        Dim lCrit As String = ""
        pParmGridIDs.Clear()
        With agdParameter.Source
            .Rows = 1
            Dim lSelectedCount As Integer = 0
            For lSegRow As Integer = 1 To agdSegment.Source.Rows - 1
                If agdSegment.Source.CellSelected(lSegRow, 0) Then 'list parameters for this segment
                    lSelectedCount += 1
                    If lSelectedCount = 1 Then
                        lCrit = "SegID = " & pSegmentGridIDs(lSegRow - 1)
                    Else
                        lCrit = lCrit & " OR SegID = " & pSegmentGridIDs(lSegRow - 1)
                    End If
                End If
            Next
            If lSelectedCount > 0 Then
                If pSelectedTableFilters.Count > 0 Then  'add filter criteria
                    Dim lFiltText As String = "Table = '"
                    Dim lFilt As String = ""
                    lFilt = "(" & lFiltText & Mid(pSelectedTableFilters(0), 1, pSelectedTableFilters(0).length - 2)
                    For lIndex As Integer = 1 To pSelectedTableFilters.Count - 1
                        lFilt = lFilt & "' OR " & lFiltText & Mid(pSelectedTableFilters(lIndex), 1, pSelectedTableFilters(lIndex).length - 2)
                    Next
                    lCrit = lCrit & " AND " & lFilt & "')"
                End If
                Dim lStr As String = "SELECT DISTINCTROW ParmTableData.ParmID, " & _
                                                        "ParmTableData.Name, " & _
                                                        "ParmTableData.Table, " & _
                                                        "ParmTableData.TabID " & _
                                                        "From ParmTableData " & _
                                                        "WHERE (" & lCrit & ")"
                Dim lTable As DataTable = Database.GetTable(lStr)
                For lRow As Integer = 0 To lTable.Rows.Count - 1
                    .Rows += 1
                    .CellValue(.Rows - 1, 0) = lTable.Rows(lRow).Item(1).ToString
                    .CellValue(.Rows - 1, 1) = lTable.Rows(lRow).Item(2).ToString
                    pParmGridIDs.Add(lTable.Rows(lRow).Item(0))
                Next
            End If
        End With

        agdParameter.SizeAllColumnsToContents()
        agdParameter.Refresh()
        ClearValues()

    End Sub

    Sub ViewParms(ByVal aParmId As Integer, ByVal aParmName As String)

        Dim lParmCrit As String = ""
        Dim lCrit As String = ""
        Dim lOpnTyp As Integer = 0
        With agdValues.Source
            .Columns = 6
            .Rows = 1
            .ColorCells = True
            .FixedRows = 1
            .FixedColumns = 0
            'build headers for parameter view
            .CellValue(0, 0) = "Name"
            .CellValue(0, 1) = "Value"
            .CellValue(0, 2) = "Segment"
            .CellValue(0, 3) = "Scenario"
            .CellValue(0, 4) = "Occur"
            .CellValue(0, 5) = "Alias"

            lParmCrit = "(ParmID = " & aParmId & " OR AssocID = " & aParmId & ")"
            lblTableParmName.Text = "Parameter " & aParmName

            Dim lSegCrit As String = ""
            For lSegRow As Integer = 1 To agdSegment.Source.Rows
                If agdSegment.Source.CellSelected(lSegRow, 0) Then 'list tables for this segment
                    lSegCrit = " SegID = " & pSegmentGridIDs(lSegRow - 1)
                    lCrit = lParmCrit & " AND " & lSegCrit
                    'now populate table values
                    Dim lStr As String = "SELECT DISTINCTROW ParmTableData.SegID, " & _
                                                            "ParmTableData.OpnTypID, " & _
                                                            "ParmTableData.Name, " & _
                                                            "ParmTableData.ParmID, " & _
                                                            "ParmTableData.Value, " & _
                                                            "ParmTableData.Table, " & _
                                                            "ParmTableData.Occur, " & _
                                                            "ParmTabledata.AliasInfo " & _
                                                            "From ParmTableData " & _
                                                            "WHERE (" & lCrit & ")"
                    Dim lTable As DataTable = Database.GetTable(lStr)
                    If lTable.Rows.Count > 0 Then
                        For lRow As Integer = 0 To lTable.Rows.Count - 1
                            .Rows += 1
                            .CellValue(.Rows - 1, 0) = lTable.Rows(lRow).Item(2).ToString
                            .CellValue(.Rows - 1, 1) = lTable.Rows(lRow).Item(4).ToString
                            .CellValue(.Rows - 1, 2) = agdSegment.Source.CellValue(lSegRow, 0)
                            .CellValue(.Rows - 1, 3) = agdSegment.Source.CellValue(lSegRow, 2)
                            .CellValue(.Rows - 1, 4) = lTable.Rows(lRow).Item(6).ToString
                            Dim lAlias As String = ""
                            Dim lColHeader As String = ""
                            If Len(Trim(lTable.Rows(lRow).Item(7).ToString)) > 0 Then
                                FillInAlias(lTable.Rows(lRow).Item(5).ToString, lTable.Rows(lRow).Item(6).ToString, lTable.Rows(lRow).Item(1).ToString, lTable.Rows(lRow).Item(0).ToString, lAlias, lColHeader)
                                .CellValue(0, 5) = lColHeader
                            Else
                                lAlias = ""
                            End If
                            .CellValue(.Rows - 1, 5) = lAlias
                        Next
                    End If
                End If
            Next
        End With

        HideLikeCol(4)

        agdValues.SizeAllColumnsToContents()
        agdValues.Refresh()

    End Sub

    Private Sub HideLikeCol(ByVal aSCol As Integer)

        Dim lCol1Same As Boolean = True
        Dim lCol2Same As Boolean = True

        With agdValues.Source
            If Len(Trim(.CellValue(1, aSCol + 1))) > 0 And .CellValue(1, aSCol + 1) IsNot Nothing Then
                lCol2Same = False
            End If

            If .Rows > 1 Then
                For i As Integer = 2 To .Rows - 1
                    If .CellValue(i, aSCol) <> .CellValue(1, aSCol) Then
                        lCol1Same = False
                    End If
                    If Trim(.CellValue(i, aSCol + 1)) <> "" And .CellValue(i, aSCol + 1) IsNot Nothing Then
                        lCol2Same = False
                    End If
                Next i
            End If

            If lCol1Same Then
                'need to remove column 
                For lCol As Integer = aSCol To .Columns - 1
                    For lRow As Integer = 0 To .Rows - 1
                        .CellValue(lRow, lCol) = .CellValue(lRow, lCol + 1)
                    Next
                Next
                .Columns = .Columns - 1
            End If
            If lCol2Same Then
                'need to remove column 
                For lCol As Integer = aSCol To .Columns - 1
                    For lRow As Integer = 0 To .Rows - 1
                        .CellValue(lRow, lCol) = .CellValue(lRow, lCol + 1)
                    Next
                Next
                .Columns = .Columns - 1
            End If
        End With
    End Sub

    Sub ViewTable(ByVal aTableId As Integer, ByVal aTableName As String)

        Dim lTableCrit As String = ""
        Dim lCrit As String = ""
        Dim lOpnTyp As Integer = 0
        With agdValues.Source
            .Columns = 4
            .Rows = 1
            .ColorCells = True
            .FixedRows = 1
            .FixedColumns = 0
            'build headers for table view
            .CellValue(0, 0) = "Op Num"
            .CellValue(0, 1) = "Scen"
            'set to no width if not applicable?
            .CellValue(0, 2) = "Occur"
            .CellValue(0, 3) = "Alias"

            lTableCrit = "TabID = " & aTableId
            lblTableParmName.Text = "Table " & aTableName
            Dim lStr As String = "SELECT DISTINCTROW ParmTableList.Name, " & _
                                                    "ParmTableList.id " & _
                                                    "From ParmTableList " & _
                                                    "WHERE (" & lTableCrit & ")"
            Dim lTable As DataTable = Database.GetTable(lStr)
            For lRow As Integer = 0 To lTable.Rows.Count - 1
                .Columns += 1
                .CellValue(0, .Columns - 1) = lTable.Rows(lRow).Item(0).ToString()
            Next

            Dim lSegCrit As String = ""
            For lSegRow As Integer = 1 To agdSegment.Source.Rows
                If agdSegment.Source.CellSelected(lSegRow, 0) Then 'list tables for this segment
                    lSegCrit = " SegID = " & pSegmentGridIDs(lSegRow - 1)
                    lCrit = lTableCrit & " AND " & lSegCrit
                    'find max occurrances 
                    lStr = "SELECT DISTINCTROW ParmTableData.SegID, " & _
                                              "ParmTableData.Occur " & _
                                              "From ParmTableData " & _
                                              "WHERE (" & lCrit & ")"
                    Dim lMaxOccur As Integer = 1
                    lTable = Database.GetTable(lStr)
                    For lRow As Integer = 0 To lTable.Rows.Count - 1
                        If lTable.Rows(lRow).Item(1) > lMaxOccur Then
                            lMaxOccur = lTable.Rows(lRow).Item(1)
                        End If
                    Next
                    For lOccur As Integer = 1 To lMaxOccur
                        Dim lOccurCrit As String = " Occur = " & lOccur.ToString
                        lCrit = lTableCrit & " AND " & lSegCrit & " AND " & lOccurCrit
                        'now populate table values
                        lStr = "SELECT DISTINCTROW ParmTableData.SegID, " & _
                                                  "ParmTableData.OpnTypID, " & _
                                                  "ParmTableData.Name, " & _
                                                  "ParmTableData.Value, " & _
                                                  "ParmTableData.ParmID, " & _
                                                  "ParmTableData.Table, " & _
                                                  "ParmTableData.Occur, " & _
                                                  "ParmTabledata.AliasInfo " & _
                                                  "From ParmTableData " & _
                                                  "WHERE (" & lCrit & ")"
                        lTable = Database.GetTable(lStr)
                        If lTable.Rows.Count > 0 Then
                            .Rows += 1
                            For lRow As Integer = 0 To lTable.Rows.Count - 1
                                .CellValue(.Rows - 1, 0) = Mid(agdSegment.Source.CellValue(lSegRow, 0), 7)
                                .CellValue(.Rows - 1, 1) = agdSegment.Source.CellValue(lSegRow, 2)
                                .CellValue(.Rows - 1, 2) = lTable.Rows(lRow).Item(6).ToString
                                Dim lAlias As String = ""
                                Dim lColHeader As String = ""
                                If Len(Trim(lTable.Rows(lRow).Item(7).ToString)) > 0 Then
                                    FillInAlias(lTable.Rows(lRow).Item(5).ToString, lTable.Rows(lRow).Item(6).ToString, lTable.Rows(lRow).Item(1).ToString, lTable.Rows(lRow).Item(0).ToString, lAlias, lColHeader)
                                    .CellValue(0, 3) = lColHeader
                                Else
                                    lAlias = ""
                                End If
                                .CellValue(.Rows - 1, 3) = lAlias
                                .CellValue(.Rows - 1, 4 + lRow) = lTable.Rows(lRow).Item(3).ToString   'problem here, need to add another row instead of more columns
                            Next
                        End If
                    Next
                End If
            Next
        End With

        HideLikeCol(2)

        agdValues.SizeAllColumnsToContents()
        agdValues.Refresh()

    End Sub

    Private Sub FillInAlias(ByVal aTableName As String, ByVal aOccur As Integer, ByVal aOpnTypID As Integer, ByVal aSegID As Integer, ByRef aAlias As String, ByRef aColHeader As String)

        aAlias = ""
        Dim lCrit As String = "Name = '" & aTableName & "' AND Occur = " & aOccur & " AND OpnTypID = " & aOpnTypID
        Dim lStr As String = "SELECT DISTINCTROW TableAliasDefn.IDVar, " & _
                                                "TableAliasDefn.IDVarName, " & _
                                                "TableAliasDefn.AppearName, " & _
                                                "TableAliasDefn.SubsKeyName " & _
                                                "From TableAliasDefn " & _
                                                "WHERE (" & lCrit & ")"
        Dim lTable As DataTable = Database.GetTable(lStr)
        If lTable.Rows.Count = 0 And aOccur > 1 Then
            lCrit = "Name = '" & aTableName & "' AND Occur = 1 AND OpnTypID = " & aOpnTypID
            lStr = "SELECT DISTINCTROW TableAliasDefn.IDVar, " & _
                                      "TableAliasDefn.IDVarName, " & _
                                      "TableAliasDefn.AppearName, " & _
                                      "TableAliasDefn.SubsKeyName " & _
                                      "From TableAliasDefn " & _
                                      "WHERE (" & lCrit & ")"
            lTable = Database.GetTable(lStr)
        End If
        If lTable.Rows.Count = 0 Then
            MsgBox("NO ALIAS")
            aAlias = "?"
        Else
            Dim lIDVar As String = lTable.Rows(0).Item(0).ToString
            Dim lIDVarName As String = lTable.Rows(0).Item(1).ToString
            Dim lAppearName As String = lTable.Rows(0).Item(2).ToString
            Dim lSubsKeyName As String = lTable.Rows(0).Item(3).ToString

            If Len(Trim(lIDVarName)) > 0 Then
                lCrit = "ParmID = " & lIDVar & " AND SegID = " & aSegID & " AND Occur = " & aOccur
                lStr = "SELECT DISTINCTROW ParmData.ParmID, " & _
                                          "ParmData.Value " & _
                                          "From ParmData " & _
                                          "WHERE (" & lCrit & ")"
                Dim lParmTable As DataTable = Database.GetTable(lStr)
                If lParmTable.Rows.Count > 0 Then
                    aAlias = lParmTable.Rows(0).Item(1).ToString & " "
                End If
                aColHeader = lIDVarName
            Else
                aColHeader = "Alias"
            End If
            If Len(Trim(lAppearName)) > 0 Then
                aAlias = aAlias & lAppearName
            End If
            If Len(Trim(lSubsKeyName)) > 0 Then
                aAlias = aAlias & " (see " & lSubsKeyName & " for this seg)"
            End If
        End If
    End Sub

    '    Private Sub Map1_SelectionChange(ByVal FeatureID As String, ByVal layer As Long, ByVal state As Boolean)

    '        '    Debug.Print "picked point " & FeatureID, layer, state

    '        MousePointer = vbHourglass

    '        Call RefreshScenario()

    '        MousePointer = vbDefault

    '    End Sub

    Private Sub agdWatershed_MouseDownCell(ByVal aGrid As atcControls.atcGrid, ByVal aRow As Integer, ByVal aColumn As Integer) Handles agdWatershed.MouseDownCell
        If aRow > 0 Then
            For lCol As Integer = 0 To agdWatershed.Source.Columns - 1
                If Not agdWatershed.Source.CellSelected(aRow, lCol) Then
                    agdWatershed.Source.CellSelected(aRow, lCol) = True
                Else
                    agdWatershed.Source.CellSelected(aRow, lCol) = False
                End If
            Next
            Refresh()
            RefreshScenario()
        End If
    End Sub

    Private Sub agdScenario_MouseDownCell(ByVal aGrid As atcControls.atcGrid, ByVal aRow As Integer, ByVal aColumn As Integer) Handles agdScenario.MouseDownCell
        If aRow > 0 Then
            For lCol As Integer = 0 To agdScenario.Source.Columns - 1
                If Not agdScenario.Source.CellSelected(aRow, lCol) Then
                    agdScenario.Source.CellSelected(aRow, lCol) = True
                Else
                    agdScenario.Source.CellSelected(aRow, lCol) = False
                End If
            Next
            Refresh()
            RefreshSegment()
        End If
    End Sub

    Private Sub agdSegment_MouseDownCell(ByVal aGrid As atcControls.atcGrid, ByVal aRow As Integer, ByVal aColumn As Integer) Handles agdSegment.MouseDownCell
        If aRow > 0 Then
            For lCol As Integer = 0 To agdSegment.Source.Columns - 1
                If Not agdSegment.Source.CellSelected(aRow, lCol) Then
                    agdSegment.Source.CellSelected(aRow, lCol) = True
                Else
                    agdSegment.Source.CellSelected(aRow, lCol) = False
                End If
            Next
            Refresh()
            RefreshTable()
            RefreshParm()
        End If
    End Sub

    Private Sub rbnTables_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbnTables.CheckedChanged
        If rbnTables.Checked Then
            agdTable.Visible = True
            agdParameter.Visible = False
        Else
            agdTable.Visible = False
            agdParameter.Visible = True
        End If
    End Sub

    Private Sub rbnParameters_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbnParameters.CheckedChanged
        If rbnParameters.Checked Then
            agdTable.Visible = False
            agdParameter.Visible = True
        Else
            agdTable.Visible = True
            agdParameter.Visible = False
        End If
    End Sub

    Private Sub agdTable_MouseDownCell(ByVal aGrid As atcControls.atcGrid, ByVal aRow As Integer, ByVal aColumn As Integer) Handles agdTable.MouseDownCell
        If aRow > 0 Then
            'unselect everything first
            For lRow As Integer = 0 To agdTable.Source.Rows - 1
                For lCol As Integer = 0 To agdTable.Source.Columns - 1
                    agdTable.Source.CellSelected(lRow, lCol) = False
                Next
            Next
            For lCol As Integer = 0 To agdTable.Source.Columns - 1
                agdTable.Source.CellSelected(aRow, lCol) = True
            Next
            Dim lTableId As Integer = pTableGridIDs(aRow - 1)
            Dim lTableName As String = agdTable.Source.CellValue(aRow, 0)

            Refresh()
            ViewTable(lTableId, lTableName)
        End If
    End Sub

    Private Sub agdParameter_MouseDownCell(ByVal aGrid As atcControls.atcGrid, ByVal aRow As Integer, ByVal aColumn As Integer) Handles agdParameter.MouseDownCell
        If aRow > 0 Then
            'unselect everything first
            For lRow As Integer = 0 To agdParameter.Source.Rows - 1
                For lCol As Integer = 0 To agdParameter.Source.Columns - 1
                    agdParameter.Source.CellSelected(lRow, lCol) = False
                Next
            Next
            For lCol As Integer = 0 To agdParameter.Source.Columns - 1
                agdParameter.Source.CellSelected(aRow, lCol) = True
            Next
            Dim lParmId As Integer = pParmGridIDs(aRow - 1)
            Dim lParmName As String = agdParameter.Source.CellValue(aRow, 0)

            Refresh()
            ViewParms(lParmId, lParmName)
        End If
    End Sub

    Private Sub cmdWatershedDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdWatershedDetails.Click
        Dim lIDs As New atcCollection
        Dim lFirstIndex As Integer = -1
        Dim lCount As Integer = 0
        With agdWatershed.Source
            For lRow As Integer = 1 To .Rows - 1
                lCount = lCount + 1
                lIDs.Add(.CellValue(lRow, 0))
                If .CellSelected(lRow, 0) And lFirstIndex < 0 Then
                    lFirstIndex = lCount
                End If
            Next
        End With

        Dim lfrmWatershedDetails As New frmWatershedDetails
        lfrmWatershedDetails.InitializeUI(lIDs, lFirstIndex, agdWatershed.Source)
        lfrmWatershedDetails.Show()
    End Sub

    Private Sub cmdScenarioDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdScenarioDetails.Click
        If agdScenario.Source.Rows < 2 Then
            Logger.Msg("Select one or more watersheds." & vbCrLf & "Scenario Details are available for selected watersheds.", MsgBoxStyle.OkOnly, "BASINS HSPFParm")
        Else
            Dim lIDs As New atcCollection
            Dim lFirstIndex As Integer = -1
            Dim lCount As Integer = 0
            With agdScenario.Source
                For lRow As Integer = 1 To .Rows - 1
                    lCount = lCount + 1
                    lIDs.Add(.CellValue(lRow, 2))
                    If .CellSelected(lRow, 0) And lFirstIndex < 0 Then
                        lFirstIndex = lCount
                    End If
                Next
            End With

            Dim lfrmScenarioDetails As New frmScenarioDetails
            lfrmScenarioDetails.InitializeUI(lIDs, lFirstIndex, Database)
            lfrmScenarioDetails.Show()
        End If
    End Sub

    Private Sub cmdFilterSegments_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdFilterSegments.Click
        Dim lSegments As New atcCollection
        Dim lTmpStr As String
        For lScenRow As Integer = 1 To agdScenario.Source.Rows
            If agdScenario.Source.CellSelected(lScenRow, 0) Then 'list segments for this scenario
                Dim lCrit As String = "ScenarioID = " & agdScenario.Source.CellValue(lScenRow, 2)
                Dim lStr As String = "SELECT DISTINCTROW SegData.ID, " & _
                                                        "SegData.Name, " & _
                                                        "SegData.Description " & _
                                                        "From SegData " & _
                                                        "WHERE (" & lCrit & ")"
                Dim lTable As DataTable = Database.GetTable(lStr)
                For lRow As Integer = 0 To lTable.Rows.Count - 1
                    lTmpStr = Mid(LTrim(lTable.Rows(lRow).Item(1)), 1, 6)
                    If Not lSegments.Contains(lTmpStr) Then
                        lSegments.Add(lTmpStr)
                    End If
                Next
            End If
        Next

        Dim lfrmSegmentFilter As New frmFilter
        lfrmSegmentFilter.InitializeUI("Segment", lSegments, pSelectedSegmentFilters, Me)
        If lfrmSegmentFilter.ShowDialog = Windows.Forms.DialogResult.OK Then
            RefreshSegment()
        End If
    End Sub

    Private Sub cmdAllSegments_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAllSegments.Click
        For lRow As Integer = 1 To agdSegment.Source.Rows - 1
            For lCol As Integer = 0 To agdSegment.Source.Columns - 1
                agdSegment.Source.CellSelected(lRow, lCol) = True
            Next
        Next
        Refresh()
        RefreshTable()
        RefreshParm()
    End Sub

    Private Sub cmdNoneSegments_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdNoneSegments.Click
        For lRow As Integer = 1 To agdSegment.Source.Rows - 1
            For lCol As Integer = 0 To agdSegment.Source.Columns - 1
                agdSegment.Source.CellSelected(lRow, lCol) = False
            Next
        Next
        Refresh()
        RefreshTable()
        RefreshParm()
    End Sub

    Private Sub cmdTableFilter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdTableFilter.Click
        Dim lTables As New atcCollection
        Dim lSelectedCount As Integer = 0
        Dim lCrit As String = ""
        For lSegRow As Integer = 1 To agdSegment.Source.Rows - 1
            If agdSegment.Source.CellSelected(lSegRow, 0) Then 'list tables for this segment
                lSelectedCount += 1
                If lSelectedCount = 1 Then
                    lCrit = "SegID = " & pSegmentGridIDs(lSegRow - 1)
                Else
                    lCrit = lCrit & " OR SegID = " & pSegmentGridIDs(lSegRow - 1)
                End If
            End If
        Next
        If lSelectedCount > 0 Then
            Dim lStr As String = "SELECT DISTINCTROW ScenTableList.Name, " & _
                                                    "ScenTableList.TabID, " & _
                                                    "ScenTableList.OpnTypID " & _
                                                    "From ScenTableList " & _
                                                    "WHERE (" & lCrit & ")"
            Dim lTable As DataTable = Database.GetTable(lStr)
            Dim lOpnTyp As Integer = 0
            Dim lTmp As String = ""
            For lRow As Integer = 0 To lTable.Rows.Count - 1
                lTmp = lTable.Rows(lRow).Item(0).ToString
                lOpnTyp = lTable.Rows(lRow).Item(2).ToString
                If lOpnTyp = 1 Then
                    lTmp = lTmp & ":P"
                ElseIf lOpnTyp = 2 Then
                    lTmp = lTmp & ":I"
                ElseIf lOpnTyp = 3 Then
                    lTmp = lTmp & ":R"
                End If
                lTables.Add(lTmp)
            Next
        End If

        Dim lfrmTableFilter As New frmFilter
        lfrmTableFilter.InitializeUI("Table", lTables, pSelectedTableFilters, Me)
        If lfrmTableFilter.ShowDialog = Windows.Forms.DialogResult.OK Then
            RefreshTable()
            RefreshParm()
        End If
    End Sub

    Private Sub ClearValues()
        With agdValues.Source
            .Rows = 0
            .Columns = 0
        End With
        agdValues.SizeAllColumnsToContents()
        agdValues.Refresh()
        lblTableParmName.Text = ""
    End Sub

    Private Sub cmdAddWatershed_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAddWatershed.Click
        'With cmdProject
        '    .DialogTitle = "HSPFParm File Open Project"
        '    .CancelError = True
        '    .flags = &H1804& 'not read only
        '    .Filter = "Project files (*.Wat)|*.Wat"
        '    On Error GoTo err
        '    .ShowOpen()
        'End With
        'Call frmWat.AddWatFromFile(cmdProject.Filename, False)

        ' Call Map1.GetSelectedKeys(WatKeys())
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
    End Sub

    Private Sub cmdDeleteWatershed_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDeleteWatershed.Click

    End Sub

    Private Sub cmdAddScenario_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAddScenario.Click

    End Sub

    Private Sub cmdDeleteScenario_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDeleteScenario.Click

    End Sub
End Class