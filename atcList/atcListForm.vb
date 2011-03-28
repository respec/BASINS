Imports atcData
Imports atcUtility

Imports System.Windows.Forms

Public Class atcListForm
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()
        InitializeComponent() 'required by Windows Form Designer
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
    Friend WithEvents mnuAttributeRows As System.Windows.Forms.MenuItem
    Friend WithEvents mnuAttributeColumns As System.Windows.Forms.MenuItem
    Friend WithEvents mnuView As System.Windows.Forms.MenuItem
    Friend WithEvents mnuSizeColumnsToContents As System.Windows.Forms.MenuItem
    Friend WithEvents mnuFileSep1 As System.Windows.Forms.MenuItem
    Friend WithEvents mnuFileSave As System.Windows.Forms.MenuItem
    Friend WithEvents mnuEdit As System.Windows.Forms.MenuItem
    Friend WithEvents mnuEditCopy As System.Windows.Forms.MenuItem
    Friend WithEvents mnuViewSep1 As System.Windows.Forms.MenuItem
    Friend WithEvents mnuFileSelectAttributes As System.Windows.Forms.MenuItem
    Friend WithEvents mnuFileSelectData As System.Windows.Forms.MenuItem
    Friend WithEvents mnuViewValues As System.Windows.Forms.MenuItem
    Friend WithEvents mnuFilterNoData As System.Windows.Forms.MenuItem
    Friend WithEvents mnuDateValueFormats As System.Windows.Forms.MenuItem
    Friend WithEvents mnuViewValueAttributes As System.Windows.Forms.MenuItem
    Friend WithEvents mnuHelp As System.Windows.Forms.MenuItem
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(atcListForm))
        Me.MainMenu1 = New System.Windows.Forms.MainMenu(Me.components)
        Me.mnuFile = New System.Windows.Forms.MenuItem
        Me.mnuFileSelectData = New System.Windows.Forms.MenuItem
        Me.mnuFileSelectAttributes = New System.Windows.Forms.MenuItem
        Me.mnuFileSep1 = New System.Windows.Forms.MenuItem
        Me.mnuFileSave = New System.Windows.Forms.MenuItem
        Me.mnuEdit = New System.Windows.Forms.MenuItem
        Me.mnuEditCopy = New System.Windows.Forms.MenuItem
        Me.mnuView = New System.Windows.Forms.MenuItem
        Me.mnuAttributeRows = New System.Windows.Forms.MenuItem
        Me.mnuAttributeColumns = New System.Windows.Forms.MenuItem
        Me.mnuViewSep1 = New System.Windows.Forms.MenuItem
        Me.mnuSizeColumnsToContents = New System.Windows.Forms.MenuItem
        Me.mnuViewValues = New System.Windows.Forms.MenuItem
        Me.mnuFilterNoData = New System.Windows.Forms.MenuItem
        Me.mnuDateValueFormats = New System.Windows.Forms.MenuItem
        Me.mnuAnalysis = New System.Windows.Forms.MenuItem
        Me.mnuHelp = New System.Windows.Forms.MenuItem
        Me.agdMain = New atcControls.atcGrid
        Me.mnuViewValueAttributes = New System.Windows.Forms.MenuItem
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
        Me.mnuView.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuAttributeRows, Me.mnuAttributeColumns, Me.mnuViewSep1, Me.mnuSizeColumnsToContents, Me.mnuViewValues, Me.mnuViewValueAttributes, Me.mnuFilterNoData, Me.mnuDateValueFormats})
        Me.mnuView.Text = "View"
        '
        'mnuAttributeRows
        '
        Me.mnuAttributeRows.Checked = True
        Me.mnuAttributeRows.Index = 0
        Me.mnuAttributeRows.Text = "Attribute Rows"
        '
        'mnuAttributeColumns
        '
        Me.mnuAttributeColumns.Index = 1
        Me.mnuAttributeColumns.Text = "Attribute Columns"
        '
        'mnuViewSep1
        '
        Me.mnuViewSep1.Index = 2
        Me.mnuViewSep1.Text = "-"
        '
        'mnuSizeColumnsToContents
        '
        Me.mnuSizeColumnsToContents.Index = 3
        Me.mnuSizeColumnsToContents.Text = "Size Columns To Contents"
        '
        'mnuViewValues
        '
        Me.mnuViewValues.Checked = True
        Me.mnuViewValues.Index = 4
        Me.mnuViewValues.Text = "Timeseries Values"
        '
        'mnuFilterNoData
        '
        Me.mnuFilterNoData.Checked = True
        Me.mnuFilterNoData.Index = 6
        Me.mnuFilterNoData.Text = "Filter NoData"
        '
        'mnuDateValueFormats
        '
        Me.mnuDateValueFormats.Index = 7
        Me.mnuDateValueFormats.Text = "Date and Value Formats..."
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
        Me.agdMain.Fixed3D = False
        Me.agdMain.LineColor = System.Drawing.Color.Empty
        Me.agdMain.LineWidth = 0.0!
        Me.agdMain.Location = New System.Drawing.Point(0, 0)
        Me.agdMain.Name = "agdMain"
        Me.agdMain.Size = New System.Drawing.Size(528, 545)
        Me.agdMain.Source = Nothing
        Me.agdMain.TabIndex = 0
        '
        'mnuViewValueAttributes
        '
        Me.mnuViewValueAttributes.Index = 5
        Me.mnuViewValueAttributes.Text = "Value Attributes"
        '
        'atcListForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(528, 545)
        Me.Controls.Add(Me.agdMain)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Menu = Me.MainMenu1
        Me.Name = "atcListForm"
        Me.Text = "Timeseries List"
        Me.ResumeLayout(False)

    End Sub

#End Region

    'The group of atcTimeseries displayed
    Private WithEvents pDataGroup As atcTimeseriesGroup

    Private pDateFormat As New atcDateFormat

    'Value formatting options, can be overridden by timeseries attributes
    Private pMaxWidth As Integer = 10
    Private pFormat As String = "#,##0.########"
    Private pExpFormat As String = ""
    Private pCantFit As String = "#"
    Private pSignificantDigits As Integer = 5

    'Translator class between pDataGroup and agdMain
    Private pSource As atcTimeseriesGridSource
    Private pDisplayAttributes As Generic.List(Of String)
    Private pSwapperSource As atcControls.atcGridSourceRowColumnSwapper

    Public Sub Initialize(Optional ByVal aTimeseriesGroup As atcData.atcTimeseriesGroup = Nothing, _
                          Optional ByVal aDisplayAttributes As Generic.List(Of String) = Nothing, _
                          Optional ByVal aShowValues As Boolean = True, _
                          Optional ByVal aFilterNoData As Boolean = False, _
                          Optional ByVal aShowForm As Boolean = True)
        If aTimeseriesGroup Is Nothing Then
            pDataGroup = New atcTimeseriesGroup
        Else
            pDataGroup = aTimeseriesGroup
        End If

        If aDisplayAttributes Is Nothing Then
            pDisplayAttributes = atcDataManager.DisplayAttributes
        Else
            pDisplayAttributes = aDisplayAttributes
        End If

        If aShowForm Then
            Dim DisplayPlugins As ICollection = atcDataManager.GetPlugins(GetType(atcDataDisplay))
            For Each lDisp As atcDataDisplay In DisplayPlugins
                Dim lMenuText As String = lDisp.Name
                If lMenuText.StartsWith("Analysis::") Then lMenuText = lMenuText.Substring(10)
                mnuAnalysis.MenuItems.Add(lMenuText, New EventHandler(AddressOf mnuAnalysis_Click))
            Next
        End If

        If pDataGroup.Count = 0 Then 'ask user to specify some timeseries
            pDataGroup = atcDataManager.UserSelectData(, pDataGroup)
        End If

        If pDataGroup.Count > 0 Then
            If aShowForm Then Me.Show()
            mnuViewValues.Checked = aShowValues
            mnuFilterNoData.Checked = aFilterNoData
            PopulateGrid()
        Else 'user declined to specify timeseries
            Me.Close()
        End If

    End Sub

    Public Property DateFormat() As atcDateFormat
        Get
            Return pDateFormat
        End Get
        Set(ByVal newValue As atcDateFormat)
            pDateFormat = newValue
            If pSource IsNot Nothing Then
                pSource.DateFormat = pDateFormat
                If agdMain IsNot Nothing Then agdMain.Refresh()
            End If
        End Set
    End Property

    Public Sub ValueFormat(Optional ByVal aMaxWidth As Integer = 10, _
                           Optional ByVal aFormat As String = "#,##0.########", _
                           Optional ByVal aExpFormat As String = "#.#e#", _
                           Optional ByVal aCantFit As String = "#", _
                           Optional ByVal aSignificantDigits As Integer = 5)
        pMaxWidth = aMaxWidth
        pFormat = aFormat
        pExpFormat = aExpFormat
        pCantFit = aCantFit
        pSignificantDigits = aSignificantDigits
        If pSource IsNot Nothing Then
            pSource.ValueFormat(pMaxWidth, pFormat, pExpFormat, pCantFit, pSignificantDigits)
            If agdMain IsNot Nothing Then agdMain.Refresh()
        End If
    End Sub

    Private Sub PopulateGrid()
        'with timeseries data, a list of attributes and options define a timeseries grid source
        pSource = New atcTimeseriesGridSource(pDataGroup, pDisplayAttributes, _
                                              mnuViewValues.Checked, _
                                              mnuFilterNoData.Checked)
        pSource.DisplayValueAttributes = mnuViewValueAttributes.Checked
        With pSource
            .DateFormat = pDateFormat
            .ValueFormat(pMaxWidth, pFormat, pExpFormat, pCantFit, pSignificantDigits)
        End With

        pSwapperSource = New atcControls.atcGridSourceRowColumnSwapper(pSource)
        pSwapperSource.SwapRowsColumns = mnuAttributeColumns.Checked

        agdMain.Initialize(pSwapperSource)
        'TODO: could SizeAllColumnsToContents return total width?
        agdMain.SizeAllColumnsToContents()

        SizeToGrid()
        agdMain.Refresh()
    End Sub

    Private Function GetIndex(ByVal aName As String) As Integer
        Return CInt(Mid(aName, InStr(aName, "#") + 1))
    End Function

    Private Sub mnuAnalysis_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAnalysis.Click
        atcDataManager.ShowDisplay(sender.Text, pDataGroup)
    End Sub

    Private Sub pDataGroup_Added(ByVal aAdded As atcCollection) Handles pDataGroup.Added
        If Me.Visible Then PopulateGrid()
        'TODO: could efficiently insert newly added item(s)
    End Sub

    Private Sub pDataGroup_Removed(ByVal aRemoved As atcCollection) Handles pDataGroup.Removed
        If Me.Visible Then PopulateGrid()
        'TODO: could efficiently remove by serial number
    End Sub

    Protected Overrides Sub OnClosing(ByVal e As System.ComponentModel.CancelEventArgs)
        pDataGroup = Nothing
        pSource = Nothing
    End Sub

    Private Sub mnuAttributeRows_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAttributeRows.Click
        SwapRowsColumns = False
    End Sub

    Private Sub mnuAttributeColumns_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAttributeColumns.Click
        SwapRowsColumns = True
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
        Dim lst As New atcControls.atcSelectList
        Dim lAvailable As New Generic.List(Of String)
        For Each lAttrDef As atcAttributeDefinition In atcDataAttributes.AllDefinitions
            Select Case lAttrDef.TypeString.ToLower
                Case "double", "integer", "boolean", "string", "atctimeunit"
                    Select Case lAttrDef.Name.ToLower
                        Case "attributes", "bins", "compfg", "constant coefficient", "degrees f", "headercomplete", "highflag", "kendall tau", "n-day high value", "n-day low value", "n-day high attribute", "n-day low attribute", "number", "return period", "summary file", "vbtime", "%*", "%sum*"
                            'Skip displaying some things in the list
                        Case Else
                            lAvailable.Add(lAttrDef.Name)
                    End Select
            End Select
        Next
        lAvailable.Sort()
        If lst.AskUser(lAvailable, pDisplayAttributes) Then
            'TODO: set project modified flag
            PopulateGrid()
        End If
    End Sub

    Private Sub mnuFileSelectData_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFileSelectData.Click
        atcDataManager.UserSelectData(, pDataGroup, , False)
    End Sub

    Private Sub mnuSizeColumnsToContents_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuSizeColumnsToContents.Click
        agdMain.SizeAllColumnsToContents()
        SizeToGrid()
        agdMain.Refresh()
    End Sub

    Private Sub mnuViewValues_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuViewValues.Click
        mnuViewValues.Checked = Not mnuViewValues.Checked
        PopulateGrid()
    End Sub

    Private Sub mnuFilterNoData_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFilterNoData.Click
        mnuFilterNoData.Checked = Not mnuFilterNoData.Checked
        PopulateGrid()
    End Sub

    Private Sub mnuViewValueAttributes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuViewValueAttributes.Click
        mnuViewValueAttributes.Checked = Not mnuViewValueAttributes.Checked
        DisplayValueAttributes = mnuViewValueAttributes.Checked
    End Sub

    Public Property DisplayValueAttributes() As Boolean
        Get
            Return pSource.DisplayValueAttributes
        End Get
        Set(ByVal newValue As Boolean)
            mnuViewValueAttributes.Checked = newValue
            pSource.DisplayValueAttributes = newValue
            agdMain.SizeAllColumnsToContents()
            SizeToGrid()
            agdMain.Refresh()
        End Set
    End Property

    'True for attributes in columns, False for attributes in rows
    Public Property SwapRowsColumns() As Boolean
        Get
            Return pSwapperSource.SwapRowsColumns
        End Get
        Set(ByVal newValue As Boolean)
            If pSwapperSource.SwapRowsColumns <> newValue Then
                pSwapperSource.SwapRowsColumns = newValue
                agdMain.SizeAllColumnsToContents()
                SizeToGrid()
                agdMain.Refresh()
            End If
            mnuAttributeColumns.Checked = newValue
            mnuAttributeRows.Checked = Not newValue
        End Set
    End Property

    Public Overrides Function ToString() As String
        Return Me.Text & vbCrLf & agdMain.ToString
    End Function

    Private pHelpLocation As String = "BASINS Details\Analysis\Time Series Functions\List.html"
    Private Sub mnuHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuHelp.Click
        ShowHelp(pHelpLocation)
    End Sub
    Private Sub atcListForm_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyValue = Windows.Forms.Keys.F1 Then
            ShowHelp(pHelpLocation)
        End If
    End Sub

    Private Sub mnuOptions_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuDateValueFormats.Click
        Dim lFrmOptions As New frmOptions
        With lFrmOptions
            .List = Me
            Select Case pDateFormat.DateOrder
                Case atcDateFormat.DateOrderEnum.DayMonthYear : .radioOrderDMY.Checked = True
                Case atcDateFormat.DateOrderEnum.JulianDate : .radioOrderJulian.Checked = True
                Case atcDateFormat.DateOrderEnum.MonthDayYear : .radioOrderMDY.Checked = True
                Case atcDateFormat.DateOrderEnum.YearMonthDay : .radioOrderYMD.Checked = True
            End Select
            .chkYears.Checked = pDateFormat.IncludeYears
            .chkMonths.Checked = pDateFormat.IncludeMonths
            .chkDays.Checked = pDateFormat.IncludeDays
            .chkHours.Checked = pDateFormat.IncludeHours
            .chkMinutes.Checked = pDateFormat.IncludeMinutes
            .chkSeconds.Checked = pDateFormat.IncludeSeconds

            .chk2digitYears.Checked = pDateFormat.TwoDigitYears
            .chkMidnight24.Checked = pDateFormat.Midnight24
            .chkMonthNames.Checked = pDateFormat.MonthNames

            .txtDateSeparator.Text = pDateFormat.DateSeparator
            .txtTimeSeparator.Text = pDateFormat.TimeSeparator

            .txtFormat.Text = pFormat
            .txtExpFormat.Text = pExpFormat
            .txtSignificantDigits.Text = pSignificantDigits
            .txtMaxWidth.Text = pMaxWidth
            .txtCantFit.Text = pCantFit

            .SaveState()
            .Show(Me)
        End With
    End Sub

    Friend Sub SetOptions(ByVal aFrmOptions As frmOptions)
        If aFrmOptions IsNot Nothing Then
            With aFrmOptions
                If .radioOrderDMY.Checked Then pDateFormat.DateOrder = atcDateFormat.DateOrderEnum.DayMonthYear
                If .radioOrderJulian.Checked Then pDateFormat.DateOrder = atcDateFormat.DateOrderEnum.JulianDate
                If .radioOrderMDY.Checked Then pDateFormat.DateOrder = atcDateFormat.DateOrderEnum.MonthDayYear
                If .radioOrderYMD.Checked Then pDateFormat.DateOrder = atcDateFormat.DateOrderEnum.YearMonthDay

                pDateFormat.IncludeYears = .chkYears.Checked
                pDateFormat.IncludeMonths = .chkMonths.Checked
                pDateFormat.IncludeHours = .chkHours.Checked
                pDateFormat.IncludeMinutes = .chkMinutes.Checked
                pDateFormat.IncludeSeconds = .chkSeconds.Checked
                pDateFormat.IncludeDays = .chkDays.Checked

                pDateFormat.TwoDigitYears = .chk2digitYears.Checked
                pDateFormat.Midnight24 = .chkMidnight24.Checked
                pDateFormat.MonthNames = .chkMonthNames.Checked

                pDateFormat.DateSeparator = .txtDateSeparator.Text
                pDateFormat.TimeSeparator = .txtTimeSeparator.Text

                pFormat = .txtFormat.Text
                pExpFormat = .txtExpFormat.Text
                Integer.TryParse(.txtSignificantDigits.Text, pSignificantDigits)
                Integer.TryParse(.txtMaxWidth.Text, pMaxWidth)
                pCantFit = .txtCantFit.Text
                PopulateGrid()
            End With
        End If
    End Sub

    Public Sub SizeToGrid()
        Try
            Dim lRequestedHeight As Integer = (Me.Height - agdMain.Height) + pSwapperSource.Rows * agdMain.RowHeight(0)
            Dim lRequestedWidth As Integer = (Me.Width - agdMain.Width)
            For lColumn As Integer = 0 To pSwapperSource.Columns - 1
                lRequestedWidth += agdMain.ColumnWidth(lColumn)
            Next
            Dim lScreenArea As System.Drawing.Rectangle = My.Computer.Screen.WorkingArea

            Width = Math.Min(lScreenArea.Width - 100, lRequestedWidth + 20)
            Height = Math.Min(lScreenArea.Height - 100, lRequestedHeight + 20)
        Catch 'Ignore error if we can't tell how large to make it, or can't rezise
        End Try
    End Sub

End Class