Imports atcData
Imports atcUtility

Imports System.Windows.Forms

Friend Class frmDisplayFrequencyGrid
    Inherits System.Windows.Forms.Form
    Private pInitializing As Boolean
    Friend WithEvents mnuGraph As System.Windows.Forms.MenuItem
    Private WithEvents pFormSpecify As frmSWSTAT

    'The group of atcTimeseries displayed
    Private WithEvents pDataGroup As atcTimeseriesGroup

    Private pSource As atcFrequencyGridSource
    Private pSwapperSource As atcControls.atcGridSourceRowColumnSwapper
    Private pNday() As Double
    Friend WithEvents mnuFileExportResults As System.Windows.Forms.MenuItem
    Private pReturns() As Double

#Region " Windows Form Designer generated code "

    Public Sub New(ByVal aDataGroup As atcData.atcTimeseriesGroup, ByVal aHigh As Boolean, ByVal aNday() As Double, ByVal aReturns() As Double)
        MyBase.New()
        pInitializing = True
        Me.Visible = False
        pDataGroup = aDataGroup
        pNday = aNday
        pReturns = aReturns

        InitializeComponent() 'required by Windows Form Designer

        Dim DisplayPlugins As ICollection = atcDataManager.GetPlugins(GetType(atcDataDisplay))
        For Each lDisp As atcDataDisplay In DisplayPlugins
            Dim lMenuText As String = lDisp.Name
            If lMenuText.StartsWith("Analysis::") Then lMenuText = lMenuText.Substring(10)
            mnuAnalysis.MenuItems.Add(lMenuText, New EventHandler(AddressOf mnuAnalysis_Click))
        Next

        pSource = Nothing 'Get rid of obsolete source before changing HighDisplay to avoid refresh trouble
        Me.HighDisplay = aHigh
        If pInitializing Then
            pInitializing = False
            Me.Show()
        End If
        PopulateGrid()
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
    Friend WithEvents MainMenu1 As System.Windows.Forms.MainMenu
    Friend WithEvents mnuAnalysis As System.Windows.Forms.MenuItem
    Friend WithEvents mnuFile As System.Windows.Forms.MenuItem
    Friend WithEvents agdMain As atcControls.atcGrid
    Friend WithEvents mnuView As System.Windows.Forms.MenuItem
    Friend WithEvents mnuViewColumns As System.Windows.Forms.MenuItem
    Friend WithEvents mnuViewRows As System.Windows.Forms.MenuItem
    Friend WithEvents mnuViewHigh As System.Windows.Forms.MenuItem
    Friend WithEvents mnuViewLow As System.Windows.Forms.MenuItem
    Friend WithEvents mnuEdit As System.Windows.Forms.MenuItem
    Friend WithEvents mnuEditCopy As System.Windows.Forms.MenuItem
    Friend WithEvents mnuFileSaveGrid As System.Windows.Forms.MenuItem
    Friend WithEvents mnuSizeColumnsToContents As System.Windows.Forms.MenuItem
    Friend WithEvents mnuViewSep1 As System.Windows.Forms.MenuItem
    Friend WithEvents mnuViewSep2 As System.Windows.Forms.MenuItem
    Friend WithEvents mnuFileSaveReport As System.Windows.Forms.MenuItem
    Friend WithEvents mnuFileSaveViewNDay As System.Windows.Forms.MenuItem
    Friend WithEvents mnuHelp As System.Windows.Forms.MenuItem
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDisplayFrequencyGrid))
        Me.MainMenu1 = New System.Windows.Forms.MainMenu(Me.components)
        Me.mnuFile = New System.Windows.Forms.MenuItem
        Me.mnuFileSaveGrid = New System.Windows.Forms.MenuItem
        Me.mnuFileSaveReport = New System.Windows.Forms.MenuItem
        Me.mnuFileSaveViewNDay = New System.Windows.Forms.MenuItem
        Me.mnuFileExportResults = New System.Windows.Forms.MenuItem
        Me.mnuEdit = New System.Windows.Forms.MenuItem
        Me.mnuEditCopy = New System.Windows.Forms.MenuItem
        Me.mnuView = New System.Windows.Forms.MenuItem
        Me.mnuViewColumns = New System.Windows.Forms.MenuItem
        Me.mnuViewRows = New System.Windows.Forms.MenuItem
        Me.mnuViewSep1 = New System.Windows.Forms.MenuItem
        Me.mnuViewHigh = New System.Windows.Forms.MenuItem
        Me.mnuViewLow = New System.Windows.Forms.MenuItem
        Me.mnuViewSep2 = New System.Windows.Forms.MenuItem
        Me.mnuSizeColumnsToContents = New System.Windows.Forms.MenuItem
        Me.mnuAnalysis = New System.Windows.Forms.MenuItem
        Me.mnuHelp = New System.Windows.Forms.MenuItem
        Me.mnuGraph = New System.Windows.Forms.MenuItem
        Me.agdMain = New atcControls.atcGrid
        Me.SuspendLayout()
        '
        'MainMenu1
        '
        Me.MainMenu1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuFile, Me.mnuEdit, Me.mnuView, Me.mnuAnalysis, Me.mnuHelp, Me.mnuGraph})
        '
        'mnuFile
        '
        Me.mnuFile.Index = 0
        Me.mnuFile.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuFileSaveGrid, Me.mnuFileSaveReport, Me.mnuFileSaveViewNDay, Me.mnuFileExportResults})
        Me.mnuFile.Text = "File"
        '
        'mnuFileSaveGrid
        '
        Me.mnuFileSaveGrid.Index = 0
        Me.mnuFileSaveGrid.Shortcut = System.Windows.Forms.Shortcut.CtrlS
        Me.mnuFileSaveGrid.Text = "Save Grid"
        '
        'mnuFileSaveReport
        '
        Me.mnuFileSaveReport.Index = 1
        Me.mnuFileSaveReport.Text = "Save Report"
        '
        'mnuFileSaveViewNDay
        '
        Me.mnuFileSaveViewNDay.Index = 2
        Me.mnuFileSaveViewNDay.Text = "Save/View N-Day"
        '
        'mnuFileExportResults
        '
        Me.mnuFileExportResults.Index = 3
        Me.mnuFileExportResults.Text = "Export Results"
        '
        'mnuEdit
        '
        Me.mnuEdit.Index = 1
        Me.mnuEdit.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuEditCopy})
        Me.mnuEdit.Text = "Edit"
        '
        'mnuEditCopy
        '
        Me.mnuEditCopy.Index = 0
        Me.mnuEditCopy.Shortcut = System.Windows.Forms.Shortcut.CtrlC
        Me.mnuEditCopy.Text = "Copy"
        '
        'mnuView
        '
        Me.mnuView.Index = 2
        Me.mnuView.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuViewColumns, Me.mnuViewRows, Me.mnuViewSep1, Me.mnuViewHigh, Me.mnuViewLow, Me.mnuViewSep2, Me.mnuSizeColumnsToContents})
        Me.mnuView.Text = "View"
        '
        'mnuViewColumns
        '
        Me.mnuViewColumns.Checked = True
        Me.mnuViewColumns.Index = 0
        Me.mnuViewColumns.Text = "Columns"
        '
        'mnuViewRows
        '
        Me.mnuViewRows.Index = 1
        Me.mnuViewRows.Text = "Rows"
        '
        'mnuViewSep1
        '
        Me.mnuViewSep1.Index = 2
        Me.mnuViewSep1.Text = "-"
        '
        'mnuViewHigh
        '
        Me.mnuViewHigh.Checked = True
        Me.mnuViewHigh.Index = 3
        Me.mnuViewHigh.Text = "High"
        '
        'mnuViewLow
        '
        Me.mnuViewLow.Index = 4
        Me.mnuViewLow.Text = "Low"
        '
        'mnuViewSep2
        '
        Me.mnuViewSep2.Index = 5
        Me.mnuViewSep2.Text = "-"
        '
        'mnuSizeColumnsToContents
        '
        Me.mnuSizeColumnsToContents.Index = 6
        Me.mnuSizeColumnsToContents.Text = "Size Columns To Contents"
        '
        'mnuAnalysis
        '
        Me.mnuAnalysis.Index = 3
        Me.mnuAnalysis.Text = "Analysis"
        '
        'mnuHelp
        '
        Me.mnuHelp.Index = 4
        Me.mnuHelp.Shortcut = System.Windows.Forms.Shortcut.F1
        Me.mnuHelp.ShowShortcut = False
        Me.mnuHelp.Text = "Help"
        '
        'mnuGraph
        '
        Me.mnuGraph.Index = 5
        Me.mnuGraph.Text = "Graph"
        '
        'agdMain
        '
        Me.agdMain.AllowHorizontalScrolling = True
        Me.agdMain.AllowNewValidValues = False
        Me.agdMain.CellBackColor = System.Drawing.Color.Empty
        Me.agdMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.agdMain.Fixed3D = False
        Me.agdMain.LineColor = System.Drawing.Color.Empty
        Me.agdMain.LineWidth = 0.0!
        Me.agdMain.Location = New System.Drawing.Point(0, 0)
        Me.agdMain.Name = "agdMain"
        Me.agdMain.Size = New System.Drawing.Size(720, 545)
        Me.agdMain.Source = Nothing
        Me.agdMain.TabIndex = 0
        '
        'frmDisplayFrequencyGrid
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(720, 545)
        Me.Controls.Add(Me.agdMain)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Menu = Me.MainMenu1
        Me.Name = "frmDisplayFrequencyGrid"
        Me.Text = "Frequency Statistics"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub PopulateGrid()
        If Not pInitializing Then
            Dim lContinue As Boolean = True
            pSource = New atcFrequencyGridSource(pDataGroup, pNday, pReturns)
            'If pSource.Columns < 3 Then
            '    lContinue = UserSpecifyAttributes()
            '    If lContinue Then
            '        pSource = New atcFrequencyGridSource(pDataGroup)
            '    End If
            'End If

            If lContinue Then
                pSource.High = mnuViewHigh.Checked

                pSwapperSource = New atcControls.atcGridSourceRowColumnSwapper(pSource)
                pSwapperSource.SwapRowsColumns = mnuViewRows.Checked

                agdMain.Initialize(pSwapperSource)
                agdMain.SizeAllColumnsToContents()

                SizeToGrid()

                agdMain.Refresh()
            Else 'user cancelled Frequency Grid specs form
                Me.Close()
            End If
        End If
    End Sub

    Private Sub mnuAnalysis_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAnalysis.Click
        atcDataManager.ShowDisplay(sender.Text, pDataGroup)
    End Sub

    Private Sub mnuEditCopy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuEditCopy.Click
        Clipboard.SetDataObject(Me.ToString)
    End Sub

    Private Sub mnuFileSaveGrid_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFileSaveGrid.Click
        Dim lSaveDialog As New System.Windows.Forms.SaveFileDialog
        With lSaveDialog
            .Title = "Save Grid As"
            .DefaultExt = ".txt"
            .FileName = ReplaceString(Me.Text, " ", "_") & "_grid.txt"
            If .ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                SaveFileString(.FileName, Me.ToString)
            End If
        End With
    End Sub

    Private Sub mnuFileSaveReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFileSaveReport.Click
        Dim lSaveDialog As New System.Windows.Forms.SaveFileDialog
        With lSaveDialog
            .Title = "Save Frequency Report As"
            .DefaultExt = ".txt"
            .FileName = ReplaceString(Me.Text, " ", "_") & "_report.txt"
            If .ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                SaveFileString(.FileName, pSource.CreateReport)
                OpenFile(.FileName)
            End If
        End With
    End Sub

    Private Sub pDataGroup_Added(ByVal aAdded As atcCollection) Handles pDataGroup.Added
        PopulateGrid()
        'TODO: could efficiently insert newly added item(s)
    End Sub

    Private Sub pDataGroup_Removed(ByVal aRemoved As atcCollection) Handles pDataGroup.Removed
        PopulateGrid()
        'TODO: could efficiently remove by serial number
    End Sub

    Private Sub mnuViewColumns_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuViewColumns.Click
        SwapRowsColumns = False
    End Sub

    Private Sub mnuViewRows_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuViewRows.Click
        SwapRowsColumns = True
    End Sub

    Private Sub mnuViewHigh_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuViewHigh.Click
        HighDisplay = True
    End Sub

    Private Sub mnuViewLow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuViewLow.Click
        HighDisplay = False
    End Sub

    Public Overrides Function ToString() As String
        Return Me.Text & vbCrLf & agdMain.ToString
    End Function

    'True for rows and columns to be swapped, false for normal orientation
    Public Property SwapRowsColumns() As Boolean
        Get
            Return pSwapperSource.SwapRowsColumns
        End Get
        Set(ByVal newValue As Boolean)
            If pSwapperSource.SwapRowsColumns <> newValue Then
                pSwapperSource.SwapRowsColumns = newValue
                SizeToGrid()
                agdMain.Refresh()
            End If
            mnuViewRows.Checked = newValue
            mnuViewColumns.Checked = Not newValue
        End Set
    End Property

    Public Property HighDisplay() As Boolean
        Get
            Return pSource.High
        End Get
        Set(ByVal newValue As Boolean)
            mnuViewHigh.Checked = newValue
            mnuViewLow.Checked = Not newValue
            Me.Text = "Frequency Statistics"
            If Not pSource Is Nothing AndAlso pSource.High <> newValue Then
                pSource.High = newValue
                agdMain.SizeAllColumnsToContents()
                SizeToGrid()
                agdMain.Refresh()
            End If
        End Set
    End Property

    Protected Overrides Sub OnClosing(ByVal e As System.ComponentModel.CancelEventArgs)
        pDataGroup = Nothing
        pSource = Nothing
    End Sub

    Private Sub mnuSizeColumnsToContents_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuSizeColumnsToContents.Click
        agdMain.SizeAllColumnsToContents()
        SizeToGrid()
        agdMain.Refresh()
    End Sub

    Private Sub mnuHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuHelp.Click
        ShowHelp("BASINS Details\Analysis\Time Series Functions.html")
    End Sub

    Private Sub mnuFileSaveViewNDay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFileSaveViewNDay.Click
        atcDataManager.UserSelectDisplay("N-Day timeseries", pSource.AllNday)
    End Sub

    Public Sub SizeToGrid()
        Try
            Dim lRequestedHeight As Integer = Me.Height - agdMain.Height + pSwapperSource.Rows * agdMain.RowHeight(0)
            Dim lRequestedWidth As Integer = Me.Width - agdMain.Width
            For lColumn As Integer = 0 To pSwapperSource.Columns - 1
                lRequestedWidth += agdMain.ColumnWidth(lColumn)
            Next
            Dim lScreenArea As System.Drawing.Rectangle = My.Computer.Screen.WorkingArea

            Width = Math.Min(lScreenArea.Width - 100, lRequestedWidth + 20)
            Height = Math.Min(lScreenArea.Height - 100, lRequestedHeight + 20)

        Catch 'Ignore error if we can't tell how large to make it, or can't rezise
        End Try
    End Sub

    Private Sub mnuGraph_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuGraph.Click
        Dim lGraphPlugin As New atcGraph.atcGraphPlugin
        Dim lGraphForm As atcGraph.atcGraphForm = lGraphPlugin.Show(pDataGroup, "Frequency")
    End Sub

    Private Sub mnuFileExportResults_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuFileExportResults.Click
        Dim lSaveDialog As New System.Windows.Forms.SaveFileDialog
        With lSaveDialog
            .Title = "Export Frequency Results As"
            .DefaultExt = ".txt"
            .FileName = ReplaceString(Me.Text, " ", "_") & "_export.txt"
            If .ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                SaveFileString(.FileName, pSource.CreateReport(True))
                OpenFile(.FileName)
            End If
        End With

    End Sub
End Class
