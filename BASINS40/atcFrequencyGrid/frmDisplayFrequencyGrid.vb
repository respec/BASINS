Imports atcData
Imports atcUtility

Imports System.Windows.Forms

Friend Class frmDisplayFrequencyGrid
    Inherits System.Windows.Forms.Form
    Private pInitializing As Boolean

#Region " Windows Form Designer generated code "

    Public Sub New(ByVal aDataManager As atcData.atcDataManager, _
          Optional ByVal aDataGroup As atcData.atcDataGroup = Nothing)
        MyBase.New()
        pInitializing = True
        pDataManager = aDataManager
        If aDataGroup Is Nothing Then
            pDataGroup = New atcDataGroup
        Else
            pDataGroup = aDataGroup
        End If
        InitializeComponent() 'required by Windows Form Designer

        Dim DisplayPlugins As ICollection = pDataManager.GetPlugins(GetType(atcDataDisplay))
        For Each lDisp As atcDataDisplay In DisplayPlugins
            Dim lMenuText As String = lDisp.Name
            If lMenuText.StartsWith("Analysis::") Then lMenuText = lMenuText.Substring(10)
            mnuAnalysis.MenuItems.Add(lMenuText, New EventHandler(AddressOf mnuAnalysis_Click))
        Next

        If pDataGroup.Count = 0 Then 'ask user to specify some Data
            pDataManager.UserSelectData(, pDataGroup, True)
        End If

        If pDataGroup.Count > 0 AndAlso UserSpecifyAttributes() Then
            pInitializing = False
            PopulateGrid()
        Else 'user declined to specify Data
            Me.Close()
        End If
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
    Friend WithEvents mnuFileSave As System.Windows.Forms.MenuItem
    Friend WithEvents mnuSizeColumnsToContents As System.Windows.Forms.MenuItem
    Friend WithEvents mnuViewSep1 As System.Windows.Forms.MenuItem
    Friend WithEvents mnuViewSep2 As System.Windows.Forms.MenuItem
    Friend WithEvents mnuFileSep1 As System.Windows.Forms.MenuItem
    Friend WithEvents mnuFileSelectAttributes As System.Windows.Forms.MenuItem
    Friend WithEvents mnuFileSelectData As System.Windows.Forms.MenuItem
    Friend WithEvents mnuHelp As System.Windows.Forms.MenuItem
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmDisplayFrequencyGrid))
        Me.MainMenu1 = New System.Windows.Forms.MainMenu
        Me.mnuFile = New System.Windows.Forms.MenuItem
        Me.mnuFileSelectData = New System.Windows.Forms.MenuItem
        Me.mnuFileSelectAttributes = New System.Windows.Forms.MenuItem
        Me.mnuFileSep1 = New System.Windows.Forms.MenuItem
        Me.mnuFileSave = New System.Windows.Forms.MenuItem
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
        Me.agdMain = New atcControls.atcGrid
        Me.mnuHelp = New System.Windows.Forms.MenuItem
        Me.SuspendLayout()
        '
        'MainMenu1
        '
        Me.MainMenu1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuFile, Me.mnuEdit, Me.mnuView, Me.mnuAnalysis, Me.mnuHelp})
        '
        'mnuFile
        '
        Me.mnuFile.Index = 0
        Me.mnuFile.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuFileSelectData, Me.mnuFileSelectAttributes, Me.mnuFileSep1, Me.mnuFileSave})
        Me.mnuFile.Text = "File"
        '
        'mnuFileSelectData
        '
        Me.mnuFileSelectData.Index = 0
        Me.mnuFileSelectData.Text = "Select &Data"
        '
        'mnuFileSelectAttributes
        '
        Me.mnuFileSelectAttributes.Index = 1
        Me.mnuFileSelectAttributes.Text = "Select &Attributes"
        '
        'mnuFileSep1
        '
        Me.mnuFileSep1.Index = 2
        Me.mnuFileSep1.Text = "-"
        '
        'mnuFileSave
        '
        Me.mnuFileSave.Index = 3
        Me.mnuFileSave.Shortcut = System.Windows.Forms.Shortcut.CtrlS
        Me.mnuFileSave.Text = "Save"
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
        'agdMain
        '
        Me.agdMain.AllowHorizontalScrolling = True
        Me.agdMain.CellBackColor = System.Drawing.Color.Empty
        Me.agdMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.agdMain.LineColor = System.Drawing.Color.Empty
        Me.agdMain.LineWidth = 0.0!
        Me.agdMain.Location = New System.Drawing.Point(0, 0)
        Me.agdMain.Name = "agdMain"
        Me.agdMain.Size = New System.Drawing.Size(720, 545)
        Me.agdMain.Source = Nothing
        Me.agdMain.TabIndex = 0
        '
        'mnuHelp
        '
        Me.mnuHelp.Index = 4
        Me.mnuHelp.Shortcut = System.Windows.Forms.Shortcut.F1
        Me.mnuHelp.ShowShortcut = False
        Me.mnuHelp.Text = "Help"
        '
        'frmDisplayFrequencyGrid
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(720, 545)
        Me.Controls.Add(Me.agdMain)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Menu = Me.MainMenu1
        Me.Name = "frmDisplayFrequencyGrid"
        Me.Text = "High Values"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private pDataManager As atcDataManager

    'The group of atcTimeseries displayed
    Private WithEvents pDataGroup As atcDataGroup

    Private pSource As atcFrequencyGridSource
    Private pSwapperSource As atcControls.atcGridSourceRowColumnSwapper

    Private Sub PopulateGrid()
        If Not pInitializing Then
            Dim lContinue As Boolean = True
            pSource = New atcFrequencyGridSource(pDataGroup)
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

                Dim lRequestedHeight As Single = Me.Height - agdMain.Top - agdMain.Height + pSource.Rows * agdMain.RowHeight(0)
                Dim lRequestedWidth As Single = Me.Width - agdMain.Left - agdMain.Width
                For lColumn As Integer = 0 To pSource.Columns - 1
                    lRequestedWidth += agdMain.ColumnWidth(lColumn)
                Next
                Me.Height = lRequestedHeight
                Me.Width = lRequestedWidth
                agdMain.Refresh()
            Else 'user cancelled Frequency Grid specs form
                Me.Close()
            End If
        End If
    End Sub

    Private Sub mnuAnalysis_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAnalysis.Click
        pDataManager.ShowDisplay(sender.Text, pDataGroup)
    End Sub

    Private Sub mnuEditCopy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuEditCopy.Click
        Clipboard.SetDataObject(Me.ToString)
    End Sub

    Private Sub mnuFileSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFileSave.Click
        Dim lSaveDialog As New System.Windows.Forms.SaveFileDialog
        With lSaveDialog
            .Title = "Save Grid As"
            .DefaultExt = ".txt"
            .FileName = ReplaceString(Me.Text, " ", "_") & ".txt"
            If .ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                SaveFileString(.FileName, Me.ToString)
            End If
        End With
    End Sub

    Private Sub mnuFileSelectAttributes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFileSelectAttributes.Click
        If UserSpecifyAttributes() Then PopulateGrid()
    End Sub

    Private Sub mnuFileSelectData_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFileSelectData.Click
        pInitializing = True
        pDataGroup = pDataManager.UserSelectData(, pDataGroup, False)
        pInitializing = False
        PopulateGrid()
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

    Private Function UserSpecifyAttributes() As Boolean
        Dim lForm As New frmSpecifyFrequency
        Dim lChoseHigh As Boolean
        If lForm.AskUser(pDataManager, pDataGroup, lChoseHigh) Then
            pSource = Nothing 'Get rid of obsolete source before changing HighDisplay to avoid refresh trouble
            Me.HighDisplay = lChoseHigh
            Return True
        Else
            Return False
        End If
    End Function

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
            If newValue Then
                Me.Text = "High Values"
            Else
                Me.Text = "Low Values"
            End If
            If Not pSource Is Nothing AndAlso pSource.High <> newValue Then
                pSource.High = newValue
                agdMain.Refresh()
            End If
        End Set
    End Property

    Protected Overrides Sub OnClosing(ByVal e As System.ComponentModel.CancelEventArgs)
        pDataManager = Nothing
        pDataGroup = Nothing
        pSource = Nothing
    End Sub

    Private Sub mnuSizeColumnsToContents_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuSizeColumnsToContents.Click
        agdMain.SizeAllColumnsToContents()
        agdMain.Refresh()
    End Sub

    Private Sub mnuHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuHelp.Click
        ShowHelp("BASINS Details\Analysis\Time Series Functions.html")
    End Sub
End Class
