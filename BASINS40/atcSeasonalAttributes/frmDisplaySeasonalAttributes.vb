Imports atcData
Imports atcUtility
'Imports MapWinUtility

Imports System.Windows.Forms

Friend Class frmDisplaySeasonalAttributes
    Inherits System.Windows.Forms.Form
    Private pInitializing As Boolean

#Region " Windows Form Designer generated code "

    Public Sub New(Optional ByVal aDataGroup As atcData.atcDataGroup = Nothing)
        MyBase.New()
        pInitializing = True
        If aDataGroup Is Nothing Then
            pDataGroup = New atcDataGroup
        Else
            pDataGroup = aDataGroup
        End If
        InitializeComponent() 'required by Windows Form Designer

        Dim DisplayPlugins As ICollection = atcDataManager.GetPlugins(GetType(atcDataDisplay))
        For Each ldisp As atcDataDisplay In DisplayPlugins
            Dim lMenuText As String = ldisp.Name
            If lMenuText.StartsWith("Analysis::") Then lMenuText = lMenuText.Substring(10)
            mnuAnalysis.MenuItems.Add(lMenuText, New EventHandler(AddressOf mnuAnalysis_Click))
        Next

        If pDataGroup.Count = 0 Then 'ask user to specify some Data
            atcDataManager.UserSelectData(, pDataGroup)
        End If

        pInitializing = False
        If pDataGroup.Count > 0 Then
            PopulateGrid()
        Else 'user declined to specify Data
            Me.Close()
        End If
        'agdMain.AllowHorizontalScrolling = False
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
    Friend WithEvents mnuViewSeasonColumns As System.Windows.Forms.MenuItem
    Friend WithEvents mnuViewSeasonRows As System.Windows.Forms.MenuItem
    Friend WithEvents mnuFileSave As System.Windows.Forms.MenuItem
    Friend WithEvents mnuEdit As System.Windows.Forms.MenuItem
    Friend WithEvents mnuEditCopy As System.Windows.Forms.MenuItem
    Friend WithEvents mnuSizeColumnsToContents As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem1 As System.Windows.Forms.MenuItem
    Friend WithEvents mnuFileSep1 As System.Windows.Forms.MenuItem
    Friend WithEvents mnuFileSelectData As System.Windows.Forms.MenuItem
    Friend WithEvents mnuFileSelectAttributes As System.Windows.Forms.MenuItem
    Friend WithEvents mnuFileForgetAttributes As System.Windows.Forms.MenuItem
    Friend WithEvents mnuHelp As System.Windows.Forms.MenuItem
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDisplaySeasonalAttributes))
        Me.MainMenu1 = New System.Windows.Forms.MainMenu(Me.components)
        Me.mnuFile = New System.Windows.Forms.MenuItem
        Me.mnuFileSelectData = New System.Windows.Forms.MenuItem
        Me.mnuFileSelectAttributes = New System.Windows.Forms.MenuItem
        Me.mnuFileForgetAttributes = New System.Windows.Forms.MenuItem
        Me.mnuFileSep1 = New System.Windows.Forms.MenuItem
        Me.mnuFileSave = New System.Windows.Forms.MenuItem
        Me.mnuEdit = New System.Windows.Forms.MenuItem
        Me.mnuEditCopy = New System.Windows.Forms.MenuItem
        Me.mnuView = New System.Windows.Forms.MenuItem
        Me.mnuViewSeasonColumns = New System.Windows.Forms.MenuItem
        Me.mnuViewSeasonRows = New System.Windows.Forms.MenuItem
        Me.MenuItem1 = New System.Windows.Forms.MenuItem
        Me.mnuSizeColumnsToContents = New System.Windows.Forms.MenuItem
        Me.mnuAnalysis = New System.Windows.Forms.MenuItem
        Me.mnuHelp = New System.Windows.Forms.MenuItem
        Me.agdMain = New atcControls.atcGrid
        Me.SuspendLayout()
        '
        'MainMenu1
        '
        Me.MainMenu1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuFile, Me.mnuEdit, Me.mnuView, Me.mnuAnalysis, Me.mnuHelp})
        '
        'mnuFile
        '
        Me.mnuFile.Index = 0
        Me.mnuFile.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuFileSelectData, Me.mnuFileSelectAttributes, Me.mnuFileForgetAttributes, Me.mnuFileSep1, Me.mnuFileSave})
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
        Me.mnuFileSelectAttributes.Text = "Select &Additional Attributes"
        '
        'mnuFileForgetAttributes
        '
        Me.mnuFileForgetAttributes.Index = 2
        Me.mnuFileForgetAttributes.Text = "Select Di&fferent Attributes"
        '
        'mnuFileSep1
        '
        Me.mnuFileSep1.Index = 3
        Me.mnuFileSep1.Text = "-"
        '
        'mnuFileSave
        '
        Me.mnuFileSave.Index = 4
        Me.mnuFileSave.Shortcut = System.Windows.Forms.Shortcut.CtrlS
        Me.mnuFileSave.Text = "Save Grid"
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
        Me.mnuView.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuViewSeasonColumns, Me.mnuViewSeasonRows, Me.MenuItem1, Me.mnuSizeColumnsToContents})
        Me.mnuView.Text = "View"
        '
        'mnuViewSeasonColumns
        '
        Me.mnuViewSeasonColumns.Checked = True
        Me.mnuViewSeasonColumns.Index = 0
        Me.mnuViewSeasonColumns.Text = "Season Columns"
        '
        'mnuViewSeasonRows
        '
        Me.mnuViewSeasonRows.Index = 1
        Me.mnuViewSeasonRows.Text = "Season Rows"
        '
        'MenuItem1
        '
        Me.MenuItem1.Index = 2
        Me.MenuItem1.Text = "-"
        '
        'mnuSizeColumnsToContents
        '
        Me.mnuSizeColumnsToContents.Index = 3
        Me.mnuSizeColumnsToContents.Text = "Size ColumnsTo Contents"
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
        Me.mnuHelp.Text = "Help"
        '
        'agdMain
        '
        Me.agdMain.AllowHorizontalScrolling = True
        Me.agdMain.AllowNewValidValues = False
        Me.agdMain.CellBackColor = System.Drawing.Color.Empty
        Me.agdMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.agdMain.LineColor = System.Drawing.Color.Empty
        Me.agdMain.LineWidth = 0.0!
        Me.agdMain.Location = New System.Drawing.Point(0, 0)
        Me.agdMain.Name = "agdMain"
        Me.agdMain.Size = New System.Drawing.Size(497, 318)
        Me.agdMain.Source = Nothing
        Me.agdMain.TabIndex = 0
        '
        'frmDisplaySeasonalAttributes
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(497, 318)
        Me.Controls.Add(Me.agdMain)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Menu = Me.MainMenu1
        Me.Name = "frmDisplaySeasonalAttributes"
        Me.Text = "Seasonal Attributes"
        Me.ResumeLayout(False)

    End Sub

#End Region

    'The group of atcTimeseries displayed
    Private WithEvents pDataGroup As atcDataGroup

    'Translator class between pDataGroup and agdMain
    Private pSource As atcSeasonalAttributesGridSource
    Private pSwapperSource As atcControls.atcGridSourceRowColumnSwapper

    Private Sub PopulateGrid()
        Dim lWasSwapped As Boolean = Not pSwapperSource Is Nothing AndAlso pSwapperSource.SwapRowsColumns
        pSource = New atcSeasonalAttributesGridSource(pDataGroup)
        If pSource.Columns < 3 Then
            UserSpecifyAttributes()
            pSource = New atcSeasonalAttributesGridSource(pDataGroup)
        End If
        pSwapperSource = New atcControls.atcGridSourceRowColumnSwapper(pSource)
        If lWasSwapped Then pSwapperSource.SwapRowsColumns = True
        agdMain.Initialize(pSwapperSource)
        agdMain.SizeAllColumnsToContents()
        agdMain.Refresh()
    End Sub

    Private Sub mnuAnalysis_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAnalysis.Click
        atcDataManager.ShowDisplay(sender.Text, pDataGroup)
    End Sub

    Private Sub mnuFileSelectData_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFileSelectData.Click
        atcDataManager.UserSelectData(, pDataGroup, , False)
    End Sub

    Private Sub pDataGroup_Added(ByVal aAdded As atcCollection) Handles pDataGroup.Added
        If Not pInitializing Then PopulateGrid()
        'TODO: could efficiently insert newly added item(s)
    End Sub

    Private Sub pDataGroup_Removed(ByVal aRemoved As atcCollection) Handles pDataGroup.Removed
        If Not pInitializing Then PopulateGrid()
        'TODO: could efficiently remove by serial number
    End Sub

    Private Sub mnuViewSeasonColumns_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuViewSeasonColumns.Click
        SwapRowsColumns = False
    End Sub

    Private Sub mnuViewSeasonRows_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuViewSeasonRows.Click
        SwapRowsColumns = True
    End Sub

    Private Sub mnuFileSelectAttributes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFileSelectAttributes.Click
        UserSpecifyAttributes()
        PopulateGrid()
    End Sub

    Private Sub mnuFileForgetAttributes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFileForgetAttributes.Click
        For Each lData As atcDataSet In pDataGroup
            Dim lRemoveThese As New ArrayList
            For Each lAttribute As atcDefinedValue In lData.Attributes
                If Not lAttribute.Arguments Is Nothing AndAlso lAttribute.Arguments.ContainsAttribute("SeasonIndex") Then
                    lRemoveThese.Add(lAttribute)
                End If
            Next
            For Each lAttribute As atcDefinedValue In lRemoveThese
                lData.Attributes.Remove(lAttribute)
            Next
        Next
        agdMain.Visible = False
        PopulateGrid()
        agdMain.Visible = True
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

    Private Sub UserSpecifyAttributes()
        For Each lPlugin As atcDataPlugin In atcDataManager.GetPlugins(GetType(atcDataSource))
            If (lPlugin.Name = "Timeseries::Seasons") Then
                Dim typ As System.Type = lPlugin.GetType()
                Dim asm As System.Reflection.Assembly = System.Reflection.Assembly.GetAssembly(typ)
                Dim newSource As atcDataSource = asm.CreateInstance(typ.FullName)
                Dim newArguments As New atcDataAttributes
                newArguments.SetValue("Timeseries", pDataGroup)
                newSource.Open("SeasonalAttributes", newArguments)
                Exit For
            End If
        Next
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
                agdMain.SizeAllColumnsToContents()
                agdMain.Refresh()
            End If
            mnuViewSeasonRows.Checked = newValue
            mnuViewSeasonColumns.Checked = Not newValue
        End Set
    End Property

    Protected Overrides Sub OnClosing(ByVal e As System.ComponentModel.CancelEventArgs)
        pDataGroup = Nothing
        pSource = Nothing
    End Sub

    Private Sub mnuSizeColumnsToContents_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuSizeColumnsToContents.Click
        agdMain.SizeAllColumnsToContents()
        agdMain.Refresh()
    End Sub

    Private Sub mnuHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuHelp.Click
        ShowHelp("BASINS Details\Analysis\Time Series Functions\Seasonal Attributes.html")
    End Sub

End Class
