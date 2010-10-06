Imports atcControls
Imports atcUtility
Imports MapWinUtility.Strings

Public Class frmResult
    Inherits System.Windows.Forms.Form

    Dim pReportFilename As String
    Dim pTitle As String

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
    Friend WithEvents lblHeader As System.Windows.Forms.Label
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents FileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents EditToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CopyToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OpenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SaveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents agdResult As atcControls.atcGrid
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmResult))
        Me.lblHeader = New System.Windows.Forms.Label
        Me.agdResult = New atcControls.atcGrid
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.OpenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.SaveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.EditToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.CopyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblHeader
        '
        Me.lblHeader.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblHeader.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblHeader.Location = New System.Drawing.Point(12, 24)
        Me.lblHeader.Name = "lblHeader"
        Me.lblHeader.Size = New System.Drawing.Size(743, 14)
        Me.lblHeader.TabIndex = 0
        Me.lblHeader.Text = "Label1"
        '
        'agdResult
        '
        Me.agdResult.AllowHorizontalScrolling = True
        Me.agdResult.AllowNewValidValues = False
        Me.agdResult.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.agdResult.CellBackColor = System.Drawing.Color.Empty
        Me.agdResult.LineColor = System.Drawing.Color.Empty
        Me.agdResult.LineWidth = 0.0!
        Me.agdResult.Location = New System.Drawing.Point(15, 41)
        Me.agdResult.Name = "agdResult"
        Me.agdResult.Size = New System.Drawing.Size(740, 314)
        Me.agdResult.Source = Nothing
        Me.agdResult.TabIndex = 2
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.EditToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(767, 24)
        Me.MenuStrip1.TabIndex = 3
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.OpenToolStripMenuItem, Me.SaveToolStripMenuItem})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(35, 20)
        Me.FileToolStripMenuItem.Text = "File"
        '
        'OpenToolStripMenuItem
        '
        Me.OpenToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.OpenToolStripMenuItem.Name = "OpenToolStripMenuItem"
        Me.OpenToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.OpenToolStripMenuItem.Text = "Open..."
        '
        'SaveToolStripMenuItem
        '
        Me.SaveToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem"
        Me.SaveToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.SaveToolStripMenuItem.Text = "Save As..."
        '
        'EditToolStripMenuItem
        '
        Me.EditToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.EditToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CopyToolStripMenuItem})
        Me.EditToolStripMenuItem.Name = "EditToolStripMenuItem"
        Me.EditToolStripMenuItem.Size = New System.Drawing.Size(37, 20)
        Me.EditToolStripMenuItem.Text = "Edit"
        '
        'CopyToolStripMenuItem
        '
        Me.CopyToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.CopyToolStripMenuItem.Name = "CopyToolStripMenuItem"
        Me.CopyToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.C), System.Windows.Forms.Keys)
        Me.CopyToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.CopyToolStripMenuItem.Text = "Copy"
        '
        'frmResult
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(767, 367)
        Me.Controls.Add(Me.agdResult)
        Me.Controls.Add(Me.lblHeader)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "frmResult"
        Me.Text = "Form1"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Public Sub InitializeResults(ByVal aTitle As String, ByVal aHeader As String, ByVal aFilename As String, ByVal aGridSource As atcGridSource)
        pReportFilename = aFilename
        SetTitleHeader(aTitle, aHeader)
        agdResult.Clear()
        agdResult.Source = aGridSource
        ResizeGrid
    End Sub

    Private Sub SetTitleHeader(ByVal aTitle As String, ByVal aHeader As String)
        pTitle = aTitle
        Me.Text = pTitle & " - " & pReportFilename
        lblHeader.Text = aHeader
    End Sub

    Private Sub frmResult_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyValue = Windows.Forms.Keys.F1 Then
            ShowHelp("BASINS Details\Watershed Characterization Reports.html")
        End If
    End Sub

    Private Sub CopyToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CopyToolStripMenuItem.Click
        Clipboard.SetText(agdResult.ToString)
    End Sub

    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        Dim cdlg As New Windows.Forms.SaveFileDialog
        cdlg.Filter = "Text Files|*.txt|All Files|*.*"
        cdlg.FilterIndex = 0
        cdlg.FileName = pReportFilename
        If cdlg.ShowDialog = Windows.Forms.DialogResult.OK Then
            pReportFilename = cdlg.FileName
            Me.Text = pTitle & " - " & cdlg.FileName
            SaveFileString(cdlg.FileName, pTitle & vbCrLf & "  " & lblHeader.Text & vbCrLf & vbCrLf & agdResult.ToString)
        End If
    End Sub

    Private Sub OpenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenToolStripMenuItem.Click
        Dim cdlg As New Windows.Forms.OpenFileDialog
        cdlg.Filter = "Text Files|*.txt|All Files|*.*"
        cdlg.FilterIndex = 0
        cdlg.FileName = Me.Text.Substring(pTitle.Length + 3)
        If cdlg.ShowDialog = Windows.Forms.DialogResult.OK Then
            Me.Text = pTitle & " - " & cdlg.FileName
            Dim lReportText As String = WholeFileString(cdlg.FileName)
            pTitle = StrSplit(lReportText, vbCrLf, "")
            lblHeader.Text = StrSplit(lReportText, vbCrLf, "")
            SetTitleHeader(pTitle, lblHeader.Text)
            StrSplit(lReportText, vbCrLf, "")
            agdResult.Source.FromString(lReportText)
            ResizeGrid()
        End If
    End Sub

    Private Sub ResizeGrid()
        Try
            'Start with boundary within window around grid
            Dim lDesiredWidth As Integer = Me.Width - agdResult.Width + 10
            Dim lDesiredHeight As Integer = Me.Height - agdResult.Height + 10

            Dim lScreenWidth As Integer = Screen.PrimaryScreen.WorkingArea.Width
            Dim lScreenHeight As Integer = Screen.PrimaryScreen.WorkingArea.Height

            agdResult.SizeAllColumnsToContents()
            For lColumn As Integer = 1 To agdResult.Source.Columns
                lDesiredWidth += agdResult.ColumnWidth(lColumn)
            Next
            If lDesiredWidth < lScreenWidth Then
                Me.Width = lDesiredWidth
                'If this makes the right of this window fall off the screen, scoot it left
                If Me.Left + lDesiredWidth > lScreenWidth Then
                    Me.Left = lScreenWidth - Me.Width
                End If
            ElseIf lDesiredWidth < Screen.PrimaryScreen.WorkingArea.Width Then
            End If

            For lRow As Integer = 1 To agdResult.Source.Rows
                lDesiredHeight += agdResult.RowHeight(lRow)
            Next
            If lDesiredHeight < lScreenHeight Then
                Me.Height = lDesiredHeight
                'If this makes the right of this window fall off the screen, scoot it up
                If Me.Top + lDesiredHeight > lScreenHeight Then
                    Me.Top = lScreenHeight - Me.Height
                End If
            End If
        Catch e As Exception
            'Ignore errors resizing, it is nice but not necessary to resize the window
        End Try
        agdResult.Refresh()
    End Sub
End Class
