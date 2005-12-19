Imports atcControls
Imports atcUtility

Imports System.Windows.Forms

Friend Class frmSelectData
  Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

  Public Sub New()
    MyBase.New()

    InitializeComponent()

    pMatchingGrid.AllowHorizontalScrolling = False
    pSelectedGrid.AllowHorizontalScrolling = False

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
  Friend WithEvents groupTop As System.Windows.Forms.GroupBox
  Friend WithEvents pnlButtons As System.Windows.Forms.Panel
  Friend WithEvents btnOk As System.Windows.Forms.Button
  Friend WithEvents btnCancel As System.Windows.Forms.Button
  Friend WithEvents splitAboveSelected As System.Windows.Forms.Splitter
  Friend WithEvents groupSelected As System.Windows.Forms.GroupBox
  Friend WithEvents panelCriteria As System.Windows.Forms.Panel
  Friend WithEvents splitAboveMatching As System.Windows.Forms.Splitter
  Friend WithEvents lblMatching As System.Windows.Forms.Label
  Friend WithEvents pMatchingGrid As atcControls.atcGrid
  Friend WithEvents pSelectedGrid As atcControls.atcGrid
  Friend WithEvents MainMenu1 As System.Windows.Forms.MainMenu
  Friend WithEvents mnuAttributes As System.Windows.Forms.MenuItem
  Friend WithEvents mnuSelect As System.Windows.Forms.MenuItem
  Friend WithEvents mnuAttributesAdd As System.Windows.Forms.MenuItem
  Friend WithEvents mnuAttributesRemove As System.Windows.Forms.MenuItem
  Friend WithEvents mnuAttributesMove As System.Windows.Forms.MenuItem
  Friend WithEvents mnuSelectAllMatching As System.Windows.Forms.MenuItem
  Friend WithEvents mnuSelectClear As System.Windows.Forms.MenuItem
  Friend WithEvents mnuSelectNoMatching As System.Windows.Forms.MenuItem
  Friend WithEvents mnuFileManage As System.Windows.Forms.MenuItem
  Friend WithEvents mnuAddData As System.Windows.Forms.MenuItem
  Friend WithEvents mnuSelectAll As System.Windows.Forms.MenuItem
  Friend WithEvents mnuFile As System.Windows.Forms.MenuItem
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
    Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmSelectData))
    Me.groupTop = New System.Windows.Forms.GroupBox
    Me.pMatchingGrid = New atcControls.atcGrid
    Me.lblMatching = New System.Windows.Forms.Label
    Me.splitAboveMatching = New System.Windows.Forms.Splitter
    Me.panelCriteria = New System.Windows.Forms.Panel
    Me.pnlButtons = New System.Windows.Forms.Panel
    Me.btnCancel = New System.Windows.Forms.Button
    Me.btnOk = New System.Windows.Forms.Button
    Me.splitAboveSelected = New System.Windows.Forms.Splitter
    Me.groupSelected = New System.Windows.Forms.GroupBox
    Me.pSelectedGrid = New atcControls.atcGrid
    Me.MainMenu1 = New System.Windows.Forms.MainMenu
    Me.mnuFile = New System.Windows.Forms.MenuItem
    Me.mnuAddData = New System.Windows.Forms.MenuItem
    Me.mnuFileManage = New System.Windows.Forms.MenuItem
    Me.mnuAttributes = New System.Windows.Forms.MenuItem
    Me.mnuAttributesAdd = New System.Windows.Forms.MenuItem
    Me.mnuAttributesRemove = New System.Windows.Forms.MenuItem
    Me.mnuAttributesMove = New System.Windows.Forms.MenuItem
    Me.mnuSelect = New System.Windows.Forms.MenuItem
    Me.mnuSelectAll = New System.Windows.Forms.MenuItem
    Me.mnuSelectClear = New System.Windows.Forms.MenuItem
    Me.mnuSelectAllMatching = New System.Windows.Forms.MenuItem
    Me.mnuSelectNoMatching = New System.Windows.Forms.MenuItem
    Me.groupTop.SuspendLayout()
    Me.pnlButtons.SuspendLayout()
    Me.groupSelected.SuspendLayout()
    Me.SuspendLayout()
    '
    'groupTop
    '
    Me.groupTop.AccessibleDescription = resources.GetString("groupTop.AccessibleDescription")
    Me.groupTop.AccessibleName = resources.GetString("groupTop.AccessibleName")
    Me.groupTop.Anchor = CType(resources.GetObject("groupTop.Anchor"), System.Windows.Forms.AnchorStyles)
    Me.groupTop.BackgroundImage = CType(resources.GetObject("groupTop.BackgroundImage"), System.Drawing.Image)
    Me.groupTop.Controls.Add(Me.pMatchingGrid)
    Me.groupTop.Controls.Add(Me.lblMatching)
    Me.groupTop.Controls.Add(Me.splitAboveMatching)
    Me.groupTop.Controls.Add(Me.panelCriteria)
    Me.groupTop.Dock = CType(resources.GetObject("groupTop.Dock"), System.Windows.Forms.DockStyle)
    Me.groupTop.Enabled = CType(resources.GetObject("groupTop.Enabled"), Boolean)
    Me.groupTop.Font = CType(resources.GetObject("groupTop.Font"), System.Drawing.Font)
    Me.groupTop.ImeMode = CType(resources.GetObject("groupTop.ImeMode"), System.Windows.Forms.ImeMode)
    Me.groupTop.Location = CType(resources.GetObject("groupTop.Location"), System.Drawing.Point)
    Me.groupTop.Name = "groupTop"
    Me.groupTop.RightToLeft = CType(resources.GetObject("groupTop.RightToLeft"), System.Windows.Forms.RightToLeft)
    Me.groupTop.Size = CType(resources.GetObject("groupTop.Size"), System.Drawing.Size)
    Me.groupTop.TabIndex = CType(resources.GetObject("groupTop.TabIndex"), Integer)
    Me.groupTop.TabStop = False
    Me.groupTop.Text = resources.GetString("groupTop.Text")
    Me.groupTop.Visible = CType(resources.GetObject("groupTop.Visible"), Boolean)
    '
    'pMatchingGrid
    '
    Me.pMatchingGrid.AccessibleDescription = resources.GetString("pMatchingGrid.AccessibleDescription")
    Me.pMatchingGrid.AccessibleName = resources.GetString("pMatchingGrid.AccessibleName")
    Me.pMatchingGrid.AllowHorizontalScrolling = True
    Me.pMatchingGrid.Anchor = CType(resources.GetObject("pMatchingGrid.Anchor"), System.Windows.Forms.AnchorStyles)
    Me.pMatchingGrid.AutoScroll = CType(resources.GetObject("pMatchingGrid.AutoScroll"), Boolean)
    Me.pMatchingGrid.AutoScrollMargin = CType(resources.GetObject("pMatchingGrid.AutoScrollMargin"), System.Drawing.Size)
    Me.pMatchingGrid.AutoScrollMinSize = CType(resources.GetObject("pMatchingGrid.AutoScrollMinSize"), System.Drawing.Size)
    Me.pMatchingGrid.BackgroundImage = CType(resources.GetObject("pMatchingGrid.BackgroundImage"), System.Drawing.Image)
    Me.pMatchingGrid.Dock = CType(resources.GetObject("pMatchingGrid.Dock"), System.Windows.Forms.DockStyle)
    Me.pMatchingGrid.Enabled = CType(resources.GetObject("pMatchingGrid.Enabled"), Boolean)
    Me.pMatchingGrid.Font = CType(resources.GetObject("pMatchingGrid.Font"), System.Drawing.Font)
    Me.pMatchingGrid.ImeMode = CType(resources.GetObject("pMatchingGrid.ImeMode"), System.Windows.Forms.ImeMode)
    Me.pMatchingGrid.LineColor = System.Drawing.Color.Empty
    Me.pMatchingGrid.LineWidth = 0.0!
    Me.pMatchingGrid.Location = CType(resources.GetObject("pMatchingGrid.Location"), System.Drawing.Point)
    Me.pMatchingGrid.Name = "pMatchingGrid"
    Me.pMatchingGrid.RightToLeft = CType(resources.GetObject("pMatchingGrid.RightToLeft"), System.Windows.Forms.RightToLeft)
    Me.pMatchingGrid.Size = CType(resources.GetObject("pMatchingGrid.Size"), System.Drawing.Size)
    Me.pMatchingGrid.Source = Nothing
    Me.pMatchingGrid.TabIndex = CType(resources.GetObject("pMatchingGrid.TabIndex"), Integer)
    Me.pMatchingGrid.Visible = CType(resources.GetObject("pMatchingGrid.Visible"), Boolean)
    '
    'lblMatching
    '
    Me.lblMatching.AccessibleDescription = resources.GetString("lblMatching.AccessibleDescription")
    Me.lblMatching.AccessibleName = resources.GetString("lblMatching.AccessibleName")
    Me.lblMatching.Anchor = CType(resources.GetObject("lblMatching.Anchor"), System.Windows.Forms.AnchorStyles)
    Me.lblMatching.AutoSize = CType(resources.GetObject("lblMatching.AutoSize"), Boolean)
    Me.lblMatching.Dock = CType(resources.GetObject("lblMatching.Dock"), System.Windows.Forms.DockStyle)
    Me.lblMatching.Enabled = CType(resources.GetObject("lblMatching.Enabled"), Boolean)
    Me.lblMatching.Font = CType(resources.GetObject("lblMatching.Font"), System.Drawing.Font)
    Me.lblMatching.Image = CType(resources.GetObject("lblMatching.Image"), System.Drawing.Image)
    Me.lblMatching.ImageAlign = CType(resources.GetObject("lblMatching.ImageAlign"), System.Drawing.ContentAlignment)
    Me.lblMatching.ImageIndex = CType(resources.GetObject("lblMatching.ImageIndex"), Integer)
    Me.lblMatching.ImeMode = CType(resources.GetObject("lblMatching.ImeMode"), System.Windows.Forms.ImeMode)
    Me.lblMatching.Location = CType(resources.GetObject("lblMatching.Location"), System.Drawing.Point)
    Me.lblMatching.Name = "lblMatching"
    Me.lblMatching.RightToLeft = CType(resources.GetObject("lblMatching.RightToLeft"), System.Windows.Forms.RightToLeft)
    Me.lblMatching.Size = CType(resources.GetObject("lblMatching.Size"), System.Drawing.Size)
    Me.lblMatching.TabIndex = CType(resources.GetObject("lblMatching.TabIndex"), Integer)
    Me.lblMatching.Text = resources.GetString("lblMatching.Text")
    Me.lblMatching.TextAlign = CType(resources.GetObject("lblMatching.TextAlign"), System.Drawing.ContentAlignment)
    Me.lblMatching.Visible = CType(resources.GetObject("lblMatching.Visible"), Boolean)
    '
    'splitAboveMatching
    '
    Me.splitAboveMatching.AccessibleDescription = resources.GetString("splitAboveMatching.AccessibleDescription")
    Me.splitAboveMatching.AccessibleName = resources.GetString("splitAboveMatching.AccessibleName")
    Me.splitAboveMatching.Anchor = CType(resources.GetObject("splitAboveMatching.Anchor"), System.Windows.Forms.AnchorStyles)
    Me.splitAboveMatching.BackgroundImage = CType(resources.GetObject("splitAboveMatching.BackgroundImage"), System.Drawing.Image)
    Me.splitAboveMatching.Dock = CType(resources.GetObject("splitAboveMatching.Dock"), System.Windows.Forms.DockStyle)
    Me.splitAboveMatching.Enabled = CType(resources.GetObject("splitAboveMatching.Enabled"), Boolean)
    Me.splitAboveMatching.Font = CType(resources.GetObject("splitAboveMatching.Font"), System.Drawing.Font)
    Me.splitAboveMatching.ImeMode = CType(resources.GetObject("splitAboveMatching.ImeMode"), System.Windows.Forms.ImeMode)
    Me.splitAboveMatching.Location = CType(resources.GetObject("splitAboveMatching.Location"), System.Drawing.Point)
    Me.splitAboveMatching.MinExtra = CType(resources.GetObject("splitAboveMatching.MinExtra"), Integer)
    Me.splitAboveMatching.MinSize = CType(resources.GetObject("splitAboveMatching.MinSize"), Integer)
    Me.splitAboveMatching.Name = "splitAboveMatching"
    Me.splitAboveMatching.RightToLeft = CType(resources.GetObject("splitAboveMatching.RightToLeft"), System.Windows.Forms.RightToLeft)
    Me.splitAboveMatching.Size = CType(resources.GetObject("splitAboveMatching.Size"), System.Drawing.Size)
    Me.splitAboveMatching.TabIndex = CType(resources.GetObject("splitAboveMatching.TabIndex"), Integer)
    Me.splitAboveMatching.TabStop = False
    Me.splitAboveMatching.Visible = CType(resources.GetObject("splitAboveMatching.Visible"), Boolean)
    '
    'panelCriteria
    '
    Me.panelCriteria.AccessibleDescription = resources.GetString("panelCriteria.AccessibleDescription")
    Me.panelCriteria.AccessibleName = resources.GetString("panelCriteria.AccessibleName")
    Me.panelCriteria.Anchor = CType(resources.GetObject("panelCriteria.Anchor"), System.Windows.Forms.AnchorStyles)
    Me.panelCriteria.AutoScroll = CType(resources.GetObject("panelCriteria.AutoScroll"), Boolean)
    Me.panelCriteria.AutoScrollMargin = CType(resources.GetObject("panelCriteria.AutoScrollMargin"), System.Drawing.Size)
    Me.panelCriteria.AutoScrollMinSize = CType(resources.GetObject("panelCriteria.AutoScrollMinSize"), System.Drawing.Size)
    Me.panelCriteria.BackgroundImage = CType(resources.GetObject("panelCriteria.BackgroundImage"), System.Drawing.Image)
    Me.panelCriteria.Dock = CType(resources.GetObject("panelCriteria.Dock"), System.Windows.Forms.DockStyle)
    Me.panelCriteria.Enabled = CType(resources.GetObject("panelCriteria.Enabled"), Boolean)
    Me.panelCriteria.Font = CType(resources.GetObject("panelCriteria.Font"), System.Drawing.Font)
    Me.panelCriteria.ImeMode = CType(resources.GetObject("panelCriteria.ImeMode"), System.Windows.Forms.ImeMode)
    Me.panelCriteria.Location = CType(resources.GetObject("panelCriteria.Location"), System.Drawing.Point)
    Me.panelCriteria.Name = "panelCriteria"
    Me.panelCriteria.RightToLeft = CType(resources.GetObject("panelCriteria.RightToLeft"), System.Windows.Forms.RightToLeft)
    Me.panelCriteria.Size = CType(resources.GetObject("panelCriteria.Size"), System.Drawing.Size)
    Me.panelCriteria.TabIndex = CType(resources.GetObject("panelCriteria.TabIndex"), Integer)
    Me.panelCriteria.Text = resources.GetString("panelCriteria.Text")
    Me.panelCriteria.Visible = CType(resources.GetObject("panelCriteria.Visible"), Boolean)
    '
    'pnlButtons
    '
    Me.pnlButtons.AccessibleDescription = resources.GetString("pnlButtons.AccessibleDescription")
    Me.pnlButtons.AccessibleName = resources.GetString("pnlButtons.AccessibleName")
    Me.pnlButtons.Anchor = CType(resources.GetObject("pnlButtons.Anchor"), System.Windows.Forms.AnchorStyles)
    Me.pnlButtons.AutoScroll = CType(resources.GetObject("pnlButtons.AutoScroll"), Boolean)
    Me.pnlButtons.AutoScrollMargin = CType(resources.GetObject("pnlButtons.AutoScrollMargin"), System.Drawing.Size)
    Me.pnlButtons.AutoScrollMinSize = CType(resources.GetObject("pnlButtons.AutoScrollMinSize"), System.Drawing.Size)
    Me.pnlButtons.BackgroundImage = CType(resources.GetObject("pnlButtons.BackgroundImage"), System.Drawing.Image)
    Me.pnlButtons.Controls.Add(Me.btnCancel)
    Me.pnlButtons.Controls.Add(Me.btnOk)
    Me.pnlButtons.Dock = CType(resources.GetObject("pnlButtons.Dock"), System.Windows.Forms.DockStyle)
    Me.pnlButtons.Enabled = CType(resources.GetObject("pnlButtons.Enabled"), Boolean)
    Me.pnlButtons.Font = CType(resources.GetObject("pnlButtons.Font"), System.Drawing.Font)
    Me.pnlButtons.ImeMode = CType(resources.GetObject("pnlButtons.ImeMode"), System.Windows.Forms.ImeMode)
    Me.pnlButtons.Location = CType(resources.GetObject("pnlButtons.Location"), System.Drawing.Point)
    Me.pnlButtons.Name = "pnlButtons"
    Me.pnlButtons.RightToLeft = CType(resources.GetObject("pnlButtons.RightToLeft"), System.Windows.Forms.RightToLeft)
    Me.pnlButtons.Size = CType(resources.GetObject("pnlButtons.Size"), System.Drawing.Size)
    Me.pnlButtons.TabIndex = CType(resources.GetObject("pnlButtons.TabIndex"), Integer)
    Me.pnlButtons.Text = resources.GetString("pnlButtons.Text")
    Me.pnlButtons.Visible = CType(resources.GetObject("pnlButtons.Visible"), Boolean)
    '
    'btnCancel
    '
    Me.btnCancel.AccessibleDescription = resources.GetString("btnCancel.AccessibleDescription")
    Me.btnCancel.AccessibleName = resources.GetString("btnCancel.AccessibleName")
    Me.btnCancel.Anchor = CType(resources.GetObject("btnCancel.Anchor"), System.Windows.Forms.AnchorStyles)
    Me.btnCancel.BackgroundImage = CType(resources.GetObject("btnCancel.BackgroundImage"), System.Drawing.Image)
    Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
    Me.btnCancel.Dock = CType(resources.GetObject("btnCancel.Dock"), System.Windows.Forms.DockStyle)
    Me.btnCancel.Enabled = CType(resources.GetObject("btnCancel.Enabled"), Boolean)
    Me.btnCancel.FlatStyle = CType(resources.GetObject("btnCancel.FlatStyle"), System.Windows.Forms.FlatStyle)
    Me.btnCancel.Font = CType(resources.GetObject("btnCancel.Font"), System.Drawing.Font)
    Me.btnCancel.Image = CType(resources.GetObject("btnCancel.Image"), System.Drawing.Image)
    Me.btnCancel.ImageAlign = CType(resources.GetObject("btnCancel.ImageAlign"), System.Drawing.ContentAlignment)
    Me.btnCancel.ImageIndex = CType(resources.GetObject("btnCancel.ImageIndex"), Integer)
    Me.btnCancel.ImeMode = CType(resources.GetObject("btnCancel.ImeMode"), System.Windows.Forms.ImeMode)
    Me.btnCancel.Location = CType(resources.GetObject("btnCancel.Location"), System.Drawing.Point)
    Me.btnCancel.Name = "btnCancel"
    Me.btnCancel.RightToLeft = CType(resources.GetObject("btnCancel.RightToLeft"), System.Windows.Forms.RightToLeft)
    Me.btnCancel.Size = CType(resources.GetObject("btnCancel.Size"), System.Drawing.Size)
    Me.btnCancel.TabIndex = CType(resources.GetObject("btnCancel.TabIndex"), Integer)
    Me.btnCancel.Text = resources.GetString("btnCancel.Text")
    Me.btnCancel.TextAlign = CType(resources.GetObject("btnCancel.TextAlign"), System.Drawing.ContentAlignment)
    Me.btnCancel.Visible = CType(resources.GetObject("btnCancel.Visible"), Boolean)
    '
    'btnOk
    '
    Me.btnOk.AccessibleDescription = resources.GetString("btnOk.AccessibleDescription")
    Me.btnOk.AccessibleName = resources.GetString("btnOk.AccessibleName")
    Me.btnOk.Anchor = CType(resources.GetObject("btnOk.Anchor"), System.Windows.Forms.AnchorStyles)
    Me.btnOk.BackgroundImage = CType(resources.GetObject("btnOk.BackgroundImage"), System.Drawing.Image)
    Me.btnOk.Dock = CType(resources.GetObject("btnOk.Dock"), System.Windows.Forms.DockStyle)
    Me.btnOk.Enabled = CType(resources.GetObject("btnOk.Enabled"), Boolean)
    Me.btnOk.FlatStyle = CType(resources.GetObject("btnOk.FlatStyle"), System.Windows.Forms.FlatStyle)
    Me.btnOk.Font = CType(resources.GetObject("btnOk.Font"), System.Drawing.Font)
    Me.btnOk.Image = CType(resources.GetObject("btnOk.Image"), System.Drawing.Image)
    Me.btnOk.ImageAlign = CType(resources.GetObject("btnOk.ImageAlign"), System.Drawing.ContentAlignment)
    Me.btnOk.ImageIndex = CType(resources.GetObject("btnOk.ImageIndex"), Integer)
    Me.btnOk.ImeMode = CType(resources.GetObject("btnOk.ImeMode"), System.Windows.Forms.ImeMode)
    Me.btnOk.Location = CType(resources.GetObject("btnOk.Location"), System.Drawing.Point)
    Me.btnOk.Name = "btnOk"
    Me.btnOk.RightToLeft = CType(resources.GetObject("btnOk.RightToLeft"), System.Windows.Forms.RightToLeft)
    Me.btnOk.Size = CType(resources.GetObject("btnOk.Size"), System.Drawing.Size)
    Me.btnOk.TabIndex = CType(resources.GetObject("btnOk.TabIndex"), Integer)
    Me.btnOk.Text = resources.GetString("btnOk.Text")
    Me.btnOk.TextAlign = CType(resources.GetObject("btnOk.TextAlign"), System.Drawing.ContentAlignment)
    Me.btnOk.Visible = CType(resources.GetObject("btnOk.Visible"), Boolean)
    '
    'splitAboveSelected
    '
    Me.splitAboveSelected.AccessibleDescription = resources.GetString("splitAboveSelected.AccessibleDescription")
    Me.splitAboveSelected.AccessibleName = resources.GetString("splitAboveSelected.AccessibleName")
    Me.splitAboveSelected.Anchor = CType(resources.GetObject("splitAboveSelected.Anchor"), System.Windows.Forms.AnchorStyles)
    Me.splitAboveSelected.BackgroundImage = CType(resources.GetObject("splitAboveSelected.BackgroundImage"), System.Drawing.Image)
    Me.splitAboveSelected.Dock = CType(resources.GetObject("splitAboveSelected.Dock"), System.Windows.Forms.DockStyle)
    Me.splitAboveSelected.Enabled = CType(resources.GetObject("splitAboveSelected.Enabled"), Boolean)
    Me.splitAboveSelected.Font = CType(resources.GetObject("splitAboveSelected.Font"), System.Drawing.Font)
    Me.splitAboveSelected.ImeMode = CType(resources.GetObject("splitAboveSelected.ImeMode"), System.Windows.Forms.ImeMode)
    Me.splitAboveSelected.Location = CType(resources.GetObject("splitAboveSelected.Location"), System.Drawing.Point)
    Me.splitAboveSelected.MinExtra = CType(resources.GetObject("splitAboveSelected.MinExtra"), Integer)
    Me.splitAboveSelected.MinSize = CType(resources.GetObject("splitAboveSelected.MinSize"), Integer)
    Me.splitAboveSelected.Name = "splitAboveSelected"
    Me.splitAboveSelected.RightToLeft = CType(resources.GetObject("splitAboveSelected.RightToLeft"), System.Windows.Forms.RightToLeft)
    Me.splitAboveSelected.Size = CType(resources.GetObject("splitAboveSelected.Size"), System.Drawing.Size)
    Me.splitAboveSelected.TabIndex = CType(resources.GetObject("splitAboveSelected.TabIndex"), Integer)
    Me.splitAboveSelected.TabStop = False
    Me.splitAboveSelected.Visible = CType(resources.GetObject("splitAboveSelected.Visible"), Boolean)
    '
    'groupSelected
    '
    Me.groupSelected.AccessibleDescription = resources.GetString("groupSelected.AccessibleDescription")
    Me.groupSelected.AccessibleName = resources.GetString("groupSelected.AccessibleName")
    Me.groupSelected.Anchor = CType(resources.GetObject("groupSelected.Anchor"), System.Windows.Forms.AnchorStyles)
    Me.groupSelected.BackgroundImage = CType(resources.GetObject("groupSelected.BackgroundImage"), System.Drawing.Image)
    Me.groupSelected.Controls.Add(Me.pSelectedGrid)
    Me.groupSelected.Dock = CType(resources.GetObject("groupSelected.Dock"), System.Windows.Forms.DockStyle)
    Me.groupSelected.Enabled = CType(resources.GetObject("groupSelected.Enabled"), Boolean)
    Me.groupSelected.Font = CType(resources.GetObject("groupSelected.Font"), System.Drawing.Font)
    Me.groupSelected.ImeMode = CType(resources.GetObject("groupSelected.ImeMode"), System.Windows.Forms.ImeMode)
    Me.groupSelected.Location = CType(resources.GetObject("groupSelected.Location"), System.Drawing.Point)
    Me.groupSelected.Name = "groupSelected"
    Me.groupSelected.RightToLeft = CType(resources.GetObject("groupSelected.RightToLeft"), System.Windows.Forms.RightToLeft)
    Me.groupSelected.Size = CType(resources.GetObject("groupSelected.Size"), System.Drawing.Size)
    Me.groupSelected.TabIndex = CType(resources.GetObject("groupSelected.TabIndex"), Integer)
    Me.groupSelected.TabStop = False
    Me.groupSelected.Text = resources.GetString("groupSelected.Text")
    Me.groupSelected.Visible = CType(resources.GetObject("groupSelected.Visible"), Boolean)
    '
    'pSelectedGrid
    '
    Me.pSelectedGrid.AccessibleDescription = resources.GetString("pSelectedGrid.AccessibleDescription")
    Me.pSelectedGrid.AccessibleName = resources.GetString("pSelectedGrid.AccessibleName")
    Me.pSelectedGrid.AllowHorizontalScrolling = True
    Me.pSelectedGrid.Anchor = CType(resources.GetObject("pSelectedGrid.Anchor"), System.Windows.Forms.AnchorStyles)
    Me.pSelectedGrid.AutoScroll = CType(resources.GetObject("pSelectedGrid.AutoScroll"), Boolean)
    Me.pSelectedGrid.AutoScrollMargin = CType(resources.GetObject("pSelectedGrid.AutoScrollMargin"), System.Drawing.Size)
    Me.pSelectedGrid.AutoScrollMinSize = CType(resources.GetObject("pSelectedGrid.AutoScrollMinSize"), System.Drawing.Size)
    Me.pSelectedGrid.BackgroundImage = CType(resources.GetObject("pSelectedGrid.BackgroundImage"), System.Drawing.Image)
    Me.pSelectedGrid.Dock = CType(resources.GetObject("pSelectedGrid.Dock"), System.Windows.Forms.DockStyle)
    Me.pSelectedGrid.Enabled = CType(resources.GetObject("pSelectedGrid.Enabled"), Boolean)
    Me.pSelectedGrid.Font = CType(resources.GetObject("pSelectedGrid.Font"), System.Drawing.Font)
    Me.pSelectedGrid.ImeMode = CType(resources.GetObject("pSelectedGrid.ImeMode"), System.Windows.Forms.ImeMode)
    Me.pSelectedGrid.LineColor = System.Drawing.Color.Empty
    Me.pSelectedGrid.LineWidth = 0.0!
    Me.pSelectedGrid.Location = CType(resources.GetObject("pSelectedGrid.Location"), System.Drawing.Point)
    Me.pSelectedGrid.Name = "pSelectedGrid"
    Me.pSelectedGrid.RightToLeft = CType(resources.GetObject("pSelectedGrid.RightToLeft"), System.Windows.Forms.RightToLeft)
    Me.pSelectedGrid.Size = CType(resources.GetObject("pSelectedGrid.Size"), System.Drawing.Size)
    Me.pSelectedGrid.Source = Nothing
    Me.pSelectedGrid.TabIndex = CType(resources.GetObject("pSelectedGrid.TabIndex"), Integer)
    Me.pSelectedGrid.Visible = CType(resources.GetObject("pSelectedGrid.Visible"), Boolean)
    '
    'MainMenu1
    '
    Me.MainMenu1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuFile, Me.mnuAttributes, Me.mnuSelect})
    Me.MainMenu1.RightToLeft = CType(resources.GetObject("MainMenu1.RightToLeft"), System.Windows.Forms.RightToLeft)
    '
    'mnuFile
    '
    Me.mnuFile.Enabled = CType(resources.GetObject("mnuFile.Enabled"), Boolean)
    Me.mnuFile.Index = 0
    Me.mnuFile.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuAddData, Me.mnuFileManage})
    Me.mnuFile.Shortcut = CType(resources.GetObject("mnuFile.Shortcut"), System.Windows.Forms.Shortcut)
    Me.mnuFile.ShowShortcut = CType(resources.GetObject("mnuFile.ShowShortcut"), Boolean)
    Me.mnuFile.Text = resources.GetString("mnuFile.Text")
    Me.mnuFile.Visible = CType(resources.GetObject("mnuFile.Visible"), Boolean)
    '
    'mnuAddData
    '
    Me.mnuAddData.Enabled = CType(resources.GetObject("mnuAddData.Enabled"), Boolean)
    Me.mnuAddData.Index = 0
    Me.mnuAddData.Shortcut = CType(resources.GetObject("mnuAddData.Shortcut"), System.Windows.Forms.Shortcut)
    Me.mnuAddData.ShowShortcut = CType(resources.GetObject("mnuAddData.ShowShortcut"), Boolean)
    Me.mnuAddData.Text = resources.GetString("mnuAddData.Text")
    Me.mnuAddData.Visible = CType(resources.GetObject("mnuAddData.Visible"), Boolean)
    '
    'mnuFileManage
    '
    Me.mnuFileManage.Enabled = CType(resources.GetObject("mnuFileManage.Enabled"), Boolean)
    Me.mnuFileManage.Index = 1
    Me.mnuFileManage.Shortcut = CType(resources.GetObject("mnuFileManage.Shortcut"), System.Windows.Forms.Shortcut)
    Me.mnuFileManage.ShowShortcut = CType(resources.GetObject("mnuFileManage.ShowShortcut"), Boolean)
    Me.mnuFileManage.Text = resources.GetString("mnuFileManage.Text")
    Me.mnuFileManage.Visible = CType(resources.GetObject("mnuFileManage.Visible"), Boolean)
    '
    'mnuAttributes
    '
    Me.mnuAttributes.Enabled = CType(resources.GetObject("mnuAttributes.Enabled"), Boolean)
    Me.mnuAttributes.Index = 1
    Me.mnuAttributes.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuAttributesAdd, Me.mnuAttributesRemove, Me.mnuAttributesMove})
    Me.mnuAttributes.Shortcut = CType(resources.GetObject("mnuAttributes.Shortcut"), System.Windows.Forms.Shortcut)
    Me.mnuAttributes.ShowShortcut = CType(resources.GetObject("mnuAttributes.ShowShortcut"), Boolean)
    Me.mnuAttributes.Text = resources.GetString("mnuAttributes.Text")
    Me.mnuAttributes.Visible = CType(resources.GetObject("mnuAttributes.Visible"), Boolean)
    '
    'mnuAttributesAdd
    '
    Me.mnuAttributesAdd.Enabled = CType(resources.GetObject("mnuAttributesAdd.Enabled"), Boolean)
    Me.mnuAttributesAdd.Index = 0
    Me.mnuAttributesAdd.Shortcut = CType(resources.GetObject("mnuAttributesAdd.Shortcut"), System.Windows.Forms.Shortcut)
    Me.mnuAttributesAdd.ShowShortcut = CType(resources.GetObject("mnuAttributesAdd.ShowShortcut"), Boolean)
    Me.mnuAttributesAdd.Text = resources.GetString("mnuAttributesAdd.Text")
    Me.mnuAttributesAdd.Visible = CType(resources.GetObject("mnuAttributesAdd.Visible"), Boolean)
    '
    'mnuAttributesRemove
    '
    Me.mnuAttributesRemove.Enabled = CType(resources.GetObject("mnuAttributesRemove.Enabled"), Boolean)
    Me.mnuAttributesRemove.Index = 1
    Me.mnuAttributesRemove.Shortcut = CType(resources.GetObject("mnuAttributesRemove.Shortcut"), System.Windows.Forms.Shortcut)
    Me.mnuAttributesRemove.ShowShortcut = CType(resources.GetObject("mnuAttributesRemove.ShowShortcut"), Boolean)
    Me.mnuAttributesRemove.Text = resources.GetString("mnuAttributesRemove.Text")
    Me.mnuAttributesRemove.Visible = CType(resources.GetObject("mnuAttributesRemove.Visible"), Boolean)
    '
    'mnuAttributesMove
    '
    Me.mnuAttributesMove.Enabled = CType(resources.GetObject("mnuAttributesMove.Enabled"), Boolean)
    Me.mnuAttributesMove.Index = 2
    Me.mnuAttributesMove.Shortcut = CType(resources.GetObject("mnuAttributesMove.Shortcut"), System.Windows.Forms.Shortcut)
    Me.mnuAttributesMove.ShowShortcut = CType(resources.GetObject("mnuAttributesMove.ShowShortcut"), Boolean)
    Me.mnuAttributesMove.Text = resources.GetString("mnuAttributesMove.Text")
    Me.mnuAttributesMove.Visible = CType(resources.GetObject("mnuAttributesMove.Visible"), Boolean)
    '
    'mnuSelect
    '
    Me.mnuSelect.Enabled = CType(resources.GetObject("mnuSelect.Enabled"), Boolean)
    Me.mnuSelect.Index = 2
    Me.mnuSelect.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuSelectAll, Me.mnuSelectClear, Me.mnuSelectAllMatching, Me.mnuSelectNoMatching})
    Me.mnuSelect.Shortcut = CType(resources.GetObject("mnuSelect.Shortcut"), System.Windows.Forms.Shortcut)
    Me.mnuSelect.ShowShortcut = CType(resources.GetObject("mnuSelect.ShowShortcut"), Boolean)
    Me.mnuSelect.Text = resources.GetString("mnuSelect.Text")
    Me.mnuSelect.Visible = CType(resources.GetObject("mnuSelect.Visible"), Boolean)
    '
    'mnuSelectAll
    '
    Me.mnuSelectAll.Enabled = CType(resources.GetObject("mnuSelectAll.Enabled"), Boolean)
    Me.mnuSelectAll.Index = 0
    Me.mnuSelectAll.Shortcut = CType(resources.GetObject("mnuSelectAll.Shortcut"), System.Windows.Forms.Shortcut)
    Me.mnuSelectAll.ShowShortcut = CType(resources.GetObject("mnuSelectAll.ShowShortcut"), Boolean)
    Me.mnuSelectAll.Text = resources.GetString("mnuSelectAll.Text")
    Me.mnuSelectAll.Visible = CType(resources.GetObject("mnuSelectAll.Visible"), Boolean)
    '
    'mnuSelectClear
    '
    Me.mnuSelectClear.Enabled = CType(resources.GetObject("mnuSelectClear.Enabled"), Boolean)
    Me.mnuSelectClear.Index = 1
    Me.mnuSelectClear.Shortcut = CType(resources.GetObject("mnuSelectClear.Shortcut"), System.Windows.Forms.Shortcut)
    Me.mnuSelectClear.ShowShortcut = CType(resources.GetObject("mnuSelectClear.ShowShortcut"), Boolean)
    Me.mnuSelectClear.Text = resources.GetString("mnuSelectClear.Text")
    Me.mnuSelectClear.Visible = CType(resources.GetObject("mnuSelectClear.Visible"), Boolean)
    '
    'mnuSelectAllMatching
    '
    Me.mnuSelectAllMatching.Enabled = CType(resources.GetObject("mnuSelectAllMatching.Enabled"), Boolean)
    Me.mnuSelectAllMatching.Index = 2
    Me.mnuSelectAllMatching.Shortcut = CType(resources.GetObject("mnuSelectAllMatching.Shortcut"), System.Windows.Forms.Shortcut)
    Me.mnuSelectAllMatching.ShowShortcut = CType(resources.GetObject("mnuSelectAllMatching.ShowShortcut"), Boolean)
    Me.mnuSelectAllMatching.Text = resources.GetString("mnuSelectAllMatching.Text")
    Me.mnuSelectAllMatching.Visible = CType(resources.GetObject("mnuSelectAllMatching.Visible"), Boolean)
    '
    'mnuSelectNoMatching
    '
    Me.mnuSelectNoMatching.Enabled = CType(resources.GetObject("mnuSelectNoMatching.Enabled"), Boolean)
    Me.mnuSelectNoMatching.Index = 3
    Me.mnuSelectNoMatching.Shortcut = CType(resources.GetObject("mnuSelectNoMatching.Shortcut"), System.Windows.Forms.Shortcut)
    Me.mnuSelectNoMatching.ShowShortcut = CType(resources.GetObject("mnuSelectNoMatching.ShowShortcut"), Boolean)
    Me.mnuSelectNoMatching.Text = resources.GetString("mnuSelectNoMatching.Text")
    Me.mnuSelectNoMatching.Visible = CType(resources.GetObject("mnuSelectNoMatching.Visible"), Boolean)
    '
    'frmSelectData
    '
    Me.AcceptButton = Me.btnOk
    Me.AccessibleDescription = resources.GetString("$this.AccessibleDescription")
    Me.AccessibleName = resources.GetString("$this.AccessibleName")
    Me.AutoScaleBaseSize = CType(resources.GetObject("$this.AutoScaleBaseSize"), System.Drawing.Size)
    Me.AutoScroll = CType(resources.GetObject("$this.AutoScroll"), Boolean)
    Me.AutoScrollMargin = CType(resources.GetObject("$this.AutoScrollMargin"), System.Drawing.Size)
    Me.AutoScrollMinSize = CType(resources.GetObject("$this.AutoScrollMinSize"), System.Drawing.Size)
    Me.BackgroundImage = CType(resources.GetObject("$this.BackgroundImage"), System.Drawing.Image)
    Me.CancelButton = Me.btnCancel
    Me.ClientSize = CType(resources.GetObject("$this.ClientSize"), System.Drawing.Size)
    Me.Controls.Add(Me.groupSelected)
    Me.Controls.Add(Me.splitAboveSelected)
    Me.Controls.Add(Me.pnlButtons)
    Me.Controls.Add(Me.groupTop)
    Me.Enabled = CType(resources.GetObject("$this.Enabled"), Boolean)
    Me.Font = CType(resources.GetObject("$this.Font"), System.Drawing.Font)
    Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
    Me.ImeMode = CType(resources.GetObject("$this.ImeMode"), System.Windows.Forms.ImeMode)
    Me.Location = CType(resources.GetObject("$this.Location"), System.Drawing.Point)
    Me.MaximumSize = CType(resources.GetObject("$this.MaximumSize"), System.Drawing.Size)
    Me.Menu = Me.MainMenu1
    Me.MinimumSize = CType(resources.GetObject("$this.MinimumSize"), System.Drawing.Size)
    Me.Name = "frmSelectData"
    Me.RightToLeft = CType(resources.GetObject("$this.RightToLeft"), System.Windows.Forms.RightToLeft)
    Me.StartPosition = CType(resources.GetObject("$this.StartPosition"), System.Windows.Forms.FormStartPosition)
    Me.Text = resources.GetString("$this.Text")
    Me.groupTop.ResumeLayout(False)
    Me.pnlButtons.ResumeLayout(False)
    Me.groupSelected.ResumeLayout(False)
    Me.ResumeLayout(False)

  End Sub

#End Region

  Private Const PADDING As Integer = 5
  Private Const NOTHING_VALUE = "~Missing~"

  Private pcboCriteria() As Windows.Forms.ComboBox
  Private plstCriteria() As atcGrid
  Private pCriteriaFraction() As Single

  Private WithEvents pDataManager As atcDataManager

  Private pMatchingGroup As atcDataGroup
  Private pSelectedGroup As atcDataGroup
  Private pSaveGroup As atcDataGroup = Nothing

  Private pMatchingSource As GridSource
  Private pSelectedSource As GridSource

  Private pInitializing As Boolean
  Private pSelectedOK As Boolean = False
  Private pRevertedToSaved As Boolean = False

  Private pTotalTS As Integer

  Public Function AskUser(ByVal aDataManager As atcDataManager, Optional ByVal aGroup As atcDataGroup = Nothing, Optional ByVal aModal As Boolean = True) As atcDataGroup
    If aGroup Is Nothing Then
      pSelectedGroup = New atcDataGroup
    Else
      pSaveGroup = aGroup.Clone
      pSelectedGroup = aGroup
    End If

    pDataManager = aDataManager

    'If pDataManager.DataSources.Count = 1 Then
    '  pDataManager.UserManage()
    '  While pDataManager.DataSources.Count = 1
    '    Application.DoEvents()
    '  End While
    'End If

    pMatchingGroup = New atcDataGroup
    pMatchingSource = New GridSource(pDataManager, pMatchingGroup)
    pMatchingSource.SelectedItems = pSelectedGroup
    pSelectedSource = New GridSource(pDataManager, pSelectedGroup)

    pMatchingGrid.Initialize(pMatchingSource)
    pSelectedGrid.Initialize(pSelectedSource)

    Populate()
    If aModal Then
      Me.ShowDialog()
      If Not pSelectedOK Then 'User clicked Cancel or closed dialog
        If Not pRevertedToSaved Then pSelectedGroup.ChangeTo(pSaveGroup)
      End If
      Return pSelectedGroup
    Else
      Me.Show()
    End If
  End Function

  Private Sub Populate()
    pInitializing = True

    Try
      For iCriteria As Integer = pcboCriteria.GetUpperBound(0) To 0 Step -1
        RemoveCriteria(pcboCriteria(iCriteria), plstCriteria(iCriteria))
      Next
    Catch ex As Exception
      'first time through there is nothing to remove, error is normal
    End Try

    ReDim pcboCriteria(0)
    ReDim plstCriteria(0)
    ReDim pCriteriaFraction(0)

    For Each lAttribName As String In pDataManager.SelectionAttributes
      AddCriteria(lAttribName)
    Next

    PopulateMatching()
    pInitializing = False
    UpdatedCriteria()
    SizeCriteria()
  End Sub

  Private Sub PopulateCriteriaCombos()
    Dim i As Integer
    For i = 0 To pcboCriteria.GetUpperBound(0)
      pcboCriteria(i).Items.Clear()
    Next
    For Each def As atcAttributeDefinition In atcDataAttributes.AllDefinitions
      If Not pcboCriteria(0).Items.Contains(def.Name) _
       AndAlso atcDataAttributes.IsSimple(def) Then
        For i = 0 To pcboCriteria.GetUpperBound(0)
          pcboCriteria(i).Items.Add(def.Name)
        Next
      End If
    Next
  End Sub

  Private Sub PopulateCriteriaList(ByVal aAttributeName As String, ByVal aList As atcGrid)
    Dim lNumeric As Boolean = False
    Dim lSortedItems As New atcCollection
    Dim lAttributeDef As atcAttributeDefinition = atcDataAttributes.GetDefinition(aAttributeName)
    Dim lTsIndex As Integer = 0
    Dim lTsLastIndex As Integer = pDataManager.DataSets.Count - 1

    LogDbg("Start PopulateCriteriaList(" & aAttributeName & ")")

    If Not lAttributeDef Is Nothing Then
      Select Case lAttributeDef.TypeString.ToLower
        Case "integer", "single", "double"
          lNumeric = True
      End Select
    End If

    With aList
      .Visible = False
      For Each ts As atcDataSet In pDataManager.DataSets
        Dim lVal As String = ts.Attributes.GetFormattedValue(aAttributeName, NOTHING_VALUE)
        Dim lIndex As Integer = 0
        If Not lSortedItems.Contains(lVal) Then
          If lNumeric Then
            Dim lKey As Double = ts.Attributes.GetValue(aAttributeName, Double.NegativeInfinity)
            lIndex = BinarySearchNumeric(lKey, lSortedItems.Keys)
            lSortedItems.Insert(lIndex, lKey, lVal)
          Else
            Dim lKey As String = ts.Attributes.GetValue(aAttributeName, NOTHING_VALUE)
            lIndex = BinarySearchString(lKey, lSortedItems.Keys)
            lSortedItems.Insert(lIndex, lKey, lVal)
          End If
        End If
        lTsIndex += 1
        LogProgress("PopulateCriteriaList ", lTsIndex, lTsLastIndex)
      Next
      .Initialize(New ListSource(lSortedItems))
      If lNumeric Then
        .Source.Alignment(0, 0) = atcAlignment.HAlignDecimal
      Else
        .Source.Alignment(0, 0) = atcAlignment.HAlignLeft
      End If
      .Visible = True
      .Refresh()
    End With

    LogDbg("Finished PopulateCriteriaList(" & aAttributeName & ")")
  End Sub

  'Returns first index of a key equal to or higher than aKey
  Private Function BinarySearchString(ByVal aKey As String, ByVal aKeys As ArrayList) As Integer
    Dim lHigher As Integer = aKeys.Count
    Dim lLower As Integer = -1
    Dim lProbe As Integer
    While (lHigher - lLower > 1)
      lProbe = (lHigher + lLower) / 2
      If (aKeys.Item(lProbe) < aKey) Then
        lLower = lProbe
      Else
        lHigher = lProbe
      End If
    End While
    Return lHigher
  End Function

  'Returns first index of a key equal to or higher than aKey
  Private Function BinarySearchNumeric(ByVal aKey As Double, ByVal aKeys As ArrayList) As Integer
    Dim lHigher As Integer = aKeys.Count
    Dim lLower As Integer = -1
    Dim lProbe As Integer
    While (lHigher - lLower > 1)
      lProbe = (lHigher + lLower) / 2
      If (CDbl(aKeys.Item(lProbe)) < aKey) Then
        lLower = lProbe
      Else
        lHigher = lProbe
      End If
    End While
    Return lHigher
  End Function

  Private Sub PopulateMatching()
    Dim iLastCriteria As Integer = pcboCriteria.GetUpperBound(0)
    pMatchingGroup.Clear()
    pTotalTS = 0
    For Each ts As atcDataSet In pDataManager.DataSets
      pTotalTS += 1
      For iCriteria As Integer = 0 To iLastCriteria
        Dim attrName As String = pcboCriteria(iCriteria).SelectedItem
        If Not attrName Is Nothing Then
          Dim selectedValues As atcCollection = CType(plstCriteria(iCriteria).Source, ListSource).SelectedItems
          If selectedValues.Count > 0 Then 'none selected = all selected
            Dim attrValue As String = ts.Attributes.GetFormattedValue(attrName, NOTHING_VALUE)
            If Not selectedValues.Contains(attrValue) Then 'Does not match this criteria
              GoTo NextTS
            End If
          End If
        End If
      Next
      'Matched all criteria, add to matching table
      pMatchingGroup.Add(ts)
      SelectMatchingRow(pMatchingGroup.Count, pSelectedGroup.Contains(ts))
NextTS:
    Next
    lblMatching.Text = "Matching Data (" & pMatchingGroup.Count & " of " & pTotalTS & ")"
    pMatchingGrid.Refresh()
  End Sub

  Private Function GetIndex(ByVal aName As String) As Integer
    Return CInt(Mid(aName, InStr(aName, "#") + 1))
  End Function

  Private Sub cboCriteria_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
    If Not sender.SelectedItem Is Nothing Then
      PopulateCriteriaList(sender.SelectedItem, plstCriteria(GetIndex(sender.name)))
      UpdatedCriteria()
    End If
  End Sub

  Private Sub lstCriteria_MouseDownCell(ByVal aGrid As atcGrid, ByVal aRow As Integer, ByVal aColumn As Integer)
    Dim lSource As ListSource = aGrid.Source
    Dim lIndex As Integer = lSource.SelectedItems.IndexFromKey(aRow)
    If lIndex >= 0 Then
      lSource.SelectedItems.RemoveAt(lIndex)
    Else
      lSource.SelectedItems.Add(aRow, lSource.CellValue(aRow, aColumn))
    End If
    aGrid.Refresh()
    PopulateMatching()
  End Sub

  Private Sub UpdatedCriteria()
    If Not pInitializing Then
      Dim mnu As MenuItem
      Dim iLastCriteria As Integer = pcboCriteria.GetUpperBound(0)

      UpdateManagerSelectionAttributes()
      PopulateMatching()
      RefreshSelected()

      For Each mnu In mnuAttributesRemove.MenuItems
        RemoveHandler mnu.Click, AddressOf mnuRemove_Click
      Next
      For Each mnu In mnuAttributesMove.MenuItems
        RemoveHandler mnu.Click, AddressOf mnuMove_Click
      Next

      mnuAttributesRemove.MenuItems.Clear()
      mnuAttributesMove.MenuItems.Clear()

      If iLastCriteria > 0 Then 'Only allow moving/removing if more than one exists
        For iCriteria As Integer = 0 To iLastCriteria
          mnu = mnuAttributesRemove.MenuItems.Add("&" & iCriteria + 1 & " " & pcboCriteria(iCriteria).SelectedItem)
          AddHandler mnu.Click, AddressOf mnuRemove_Click
          mnu = mnuAttributesMove.MenuItems.Add("&" & iCriteria + 1 & " " & pcboCriteria(iCriteria).SelectedItem)
          AddHandler mnu.Click, AddressOf mnuMove_Click
        Next
      End If
    End If
  End Sub

  Private Sub RemoveCriteria(ByVal cbo As Windows.Forms.ComboBox, ByVal lst As atcGrid)
    Dim iRemoving As Integer = GetIndex(cbo.Name)
    Dim newLastCriteria As Integer = pcboCriteria.GetUpperBound(0) - 1
    Dim OldToNew As Single = 1 / (1 - pCriteriaFraction(iRemoving))
    Dim mnu As MenuItem
    RemoveHandler cbo.SelectedValueChanged, AddressOf cboCriteria_SelectedIndexChanged
    RemoveHandler lst.MouseDownCell, AddressOf lstCriteria_MouseDownCell
    panelCriteria.Controls.Remove(cbo)
    panelCriteria.Controls.Remove(lst)

    For iMoving As Integer = iRemoving To pcboCriteria.GetUpperBound(0) - 1
      pcboCriteria(iMoving) = pcboCriteria(iMoving + 1)
      plstCriteria(iMoving) = plstCriteria(iMoving + 1)
      pcboCriteria(iMoving).Name = "cboCriteria#" & iMoving
      plstCriteria(iMoving).Name = "lstCriteria#" & iMoving
      pCriteriaFraction(iMoving) = pCriteriaFraction(iMoving + 1)
    Next

    ReDim Preserve pcboCriteria(newLastCriteria)
    ReDim Preserve plstCriteria(newLastCriteria)
    ReDim Preserve pCriteriaFraction(newLastCriteria)

    'Expand remaining criteria proportionally to fill space
    For iScanCriteria As Integer = 0 To newLastCriteria
      pCriteriaFraction(iScanCriteria) *= OldToNew
    Next

    SizeCriteria()
    UpdatedCriteria()
  End Sub

  Private Sub AddCriteria(Optional ByVal aText As String = "")
    Dim iCriteria As Integer = pcboCriteria.GetUpperBound(0)

    If Not pcboCriteria(iCriteria) Is Nothing Then 'If we already populated this index, move to next one
      iCriteria += 1                               'This happens every time except for the first one
      ReDim Preserve pcboCriteria(iCriteria)
      ReDim Preserve plstCriteria(iCriteria)
      ReDim Preserve pCriteriaFraction(iCriteria)
    End If

    Dim fractionInUse As Single = 0
    For iScanCriteria As Integer = 0 To iCriteria - 1
      fractionInUse += pCriteriaFraction(iScanCriteria)
    Next

    Dim newEqualPortion As Single = 1 / (iCriteria + 1)
    Dim totalShrinkingNeeded As Single = fractionInUse + newEqualPortion - 1

    'Default to give new one an equal portion of the width
    pCriteriaFraction(iCriteria) = newEqualPortion

    If totalShrinkingNeeded > 0 Then 'Not enough extra unused space
      'Shrink existing criteria proportionally to fit the new one in
      For iScanCriteria As Integer = 0 To iCriteria - 1
        pCriteriaFraction(iScanCriteria) *= (1 - totalShrinkingNeeded)
      Next
    End If

    pcboCriteria(iCriteria) = New Windows.Forms.ComboBox
    plstCriteria(iCriteria) = New atcGrid

    panelCriteria.Controls.Add(pcboCriteria(iCriteria))
    panelCriteria.Controls.Add(plstCriteria(iCriteria))

    AddHandler pcboCriteria(iCriteria).SelectedValueChanged, AddressOf cboCriteria_SelectedIndexChanged
    AddHandler plstCriteria(iCriteria).MouseDownCell, AddressOf lstCriteria_MouseDownCell

    With pcboCriteria(iCriteria)
      .Name = "cboCriteria#" & iCriteria
      .DropDownStyle = Windows.Forms.ComboBoxStyle.DropDownList
      .MaxDropDownItems = 40
      .Sorted = True
    End With

    With plstCriteria(iCriteria)
      .Name = "lstCriteria#" & iCriteria
      .AllowHorizontalScrolling = False
    End With

    If iCriteria = 0 Then
      PopulateCriteriaCombos()
    Else 'populate from first combo box
      For iItem As Integer = 0 To pcboCriteria(0).Items.Count - 1
        pcboCriteria(iCriteria).Items.Add(pcboCriteria(0).Items.Item(iItem))
      Next
    End If
    If aText.Length > 0 Then
      pcboCriteria(iCriteria).Text = aText
    Else 'Find next criteria that is not yet in use
      For Each curName As String In pcboCriteria(iCriteria).Items
        For iOtherCriteria As Integer = 0 To iCriteria - 1
          If curName.Equals(pcboCriteria(iOtherCriteria).SelectedItem) Then GoTo NextName
        Next
        If atcDataAttributes.GetDefinition(curName).Calculated Then GoTo NextName
        pcboCriteria(iCriteria).Text = curName
        Exit For
NextName:
      Next
    End If
  End Sub

  Private Sub panelCriteria_SizeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles panelCriteria.SizeChanged
    SizeCriteria()
  End Sub

  Private Sub ResizeOneCriteria(ByVal aCriteria As Integer, ByVal aWidth As Integer)
    Dim iLastCriteria As Integer = pcboCriteria.GetUpperBound(0)
    Dim lWidth As Integer = aWidth - PADDING
    pcboCriteria(aCriteria).Width = lWidth
    pCriteriaFraction(aCriteria) = lWidth / (panelCriteria.Width - PADDING)
    plstCriteria(aCriteria).Width = lWidth
    plstCriteria(aCriteria).ColumnWidth(0) = lWidth
    While aCriteria < iLastCriteria
      aCriteria += 1
      pcboCriteria(aCriteria).Left = pcboCriteria(aCriteria - 1).Left + pcboCriteria(aCriteria - 1).Width + PADDING
      plstCriteria(aCriteria).Left = pcboCriteria(aCriteria).Left
    End While

    'Fit rightmost criteria to fill remaining space
    Dim availableWidth As Integer = panelCriteria.Width - PADDING * 2
    If pcboCriteria(iLastCriteria).Left < availableWidth Then
      lWidth = availableWidth - pcboCriteria(iLastCriteria).Left
      pcboCriteria(iLastCriteria).Width = lWidth
      plstCriteria(iLastCriteria).Width = lWidth
      plstCriteria(iLastCriteria).ColumnWidth(0) = lWidth
    End If
  End Sub

  Private Sub SizeCriteria()
    If Visible AndAlso Not pcboCriteria Is Nothing Then
      Dim iLastCriteria As Integer = pcboCriteria.GetUpperBound(0)
      If iLastCriteria >= 0 Then
        Dim availableWidth As Integer = panelCriteria.Width
        'Dim perCriteriaWidth As Integer = (panelCriteria.Width - PADDING) / (iLastCriteria + 1)
        Dim curLeft As Integer = 0

        pMatchingGrid.ColumnWidth(0) = 0
        pSelectedGrid.ColumnWidth(0) = 0

        For iCriteria As Integer = 0 To iLastCriteria
          pcboCriteria(iCriteria).Top = PADDING
          pcboCriteria(iCriteria).Left = curLeft
          If iCriteria = iLastCriteria AndAlso curLeft < availableWidth Then
            pcboCriteria(iCriteria).Width = availableWidth - curLeft 'Rightmost criteria fills remaining space
          Else
            If availableWidth * pCriteriaFraction(iCriteria) > PADDING * 2 Then
              pcboCriteria(iCriteria).Width = availableWidth * pCriteriaFraction(iCriteria) - PADDING
            Else
              pcboCriteria(iCriteria).Width = PADDING
            End If
          End If

          With plstCriteria(iCriteria)
            .Top = pcboCriteria(iCriteria).Top + pcboCriteria(iCriteria).Height + PADDING
            .Left = curLeft
            .Width = pcboCriteria(iCriteria).Width
            .ColumnWidth(0) = .Width
            .Height = panelCriteria.Height - .Top - PADDING
            .Visible = True
            .BringToFront()
            .Refresh()
          End With

          curLeft = pcboCriteria(iCriteria).Left + pcboCriteria(iCriteria).Width + PADDING

          pMatchingGrid.ColumnWidth(iCriteria + 1) = pcboCriteria(iCriteria).Width + PADDING
          pSelectedGrid.ColumnWidth(iCriteria + 1) = pMatchingGrid.ColumnWidth(iCriteria + 1)
        Next
        pMatchingGrid.Refresh()
        pSelectedGrid.Refresh()
      End If
    End If
  End Sub

  Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
    'If user didn't select anything, 
    ' but either narrowed the matching group or there are not more than 10 datasets,
    ' assume they meant to select all the matching datasets
    If pSelectedGroup.Count = 0 AndAlso _
      (pMatchingGroup.Count < pDataManager.DataSets.Count OrElse pMatchingGroup.Count < 11) Then
      pSelectedGroup.ChangeTo(pMatchingGroup)
    End If
    pSelectedOK = True
    Me.Close()
  End Sub

  Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
    pSelectedOK = False
    pRevertedToSaved = True
    pSelectedGroup.ChangeTo(pSaveGroup)
    Me.Close()
  End Sub

  'Update SelectionAttributes from current set of pcboCriteria
  Private Sub UpdateManagerSelectionAttributes()
    Dim curAttributes As New ArrayList
    For iCriteria As Integer = 0 To pcboCriteria.GetUpperBound(0)
      Dim attrName As String = pcboCriteria(iCriteria).SelectedItem
      If Not attrName Is Nothing Then
        curAttributes.Add(attrName)
      End If
    Next
    If curAttributes.Count > 0 Then
      pDataManager.SelectionAttributes.Clear()
      pDataManager.SelectionAttributes.AddRange(curAttributes)
    End If
  End Sub

  Private Sub SelectMatchingRow(ByVal aRow As Integer, ByVal aSelect As Boolean)
    For iColumn As Integer = 0 To pMatchingSource.Columns - 1
      pMatchingSource.CellSelected(aRow, iColumn) = aSelect
    Next
  End Sub

  Private Sub pMatchingGrid_MouseDownCell(ByVal aGrid As atcGrid, ByVal aRow As Integer, ByVal aColumn As Integer) Handles pMatchingGrid.MouseDownCell
    If IsNumeric(pMatchingSource.CellValue(aRow, 0)) Then 'clicked a row containing a serial number
      Dim lSerial As Integer = CInt(pMatchingSource.CellValue(aRow, 0)) 'Serial number in clicked row
      Dim iTS As Integer = pSelectedGroup.IndexOfSerial(lSerial)
      If iTS >= 0 Then 'Already selected, unselect
        pSelectedGroup.RemoveAt(iTS)
        SelectMatchingRow(aRow, False)
      Else 'Not already selected, select it now
        iTS = pMatchingGroup.IndexOfSerial(lSerial)
        If iTS >= 0 Then 'Found matching serial number in pMatchingGroup
          Dim selTS As atcData.atcDataSet = pMatchingGroup(iTS)
          pSelectedGroup.Add(selTS)
          SelectMatchingRow(aRow, True)
        End If
      End If
    End If
    RefreshSelected()
  End Sub

  Private Sub RefreshSelected()
    pMatchingGrid.Refresh()
    pSelectedGrid.Refresh()
    groupSelected.Text = "Selected Data (" & pSelectedGroup.Count & " of " & pTotalTS & ")"
  End Sub

  Private Sub pSelectedGrid_MouseDownCell(ByVal aGrid As atcGrid, ByVal aRow As Integer, ByVal aColumn As Integer) Handles pSelectedGrid.MouseDownCell
    If IsNumeric(pSelectedSource.CellValue(aRow, 0)) Then 'clicked a row containing a serial number
      Dim lSerial As Integer = CInt(pSelectedSource.CellValue(aRow, 0)) 'Serial number in row to be removed
      Dim iTS As Integer = pSelectedGroup.IndexOfSerial(lSerial)
      If iTS >= 0 Then 'Found matching serial number in pSelectedGroup
        pSelectedGroup.RemoveAt(iTS)
        RefreshSelected()
      Else
        'TODO: should never reach this line
      End If
    End If
  End Sub

  Private Sub pDataManager_OpenedData(ByVal aDataSource As atcDataSource) Handles pDataManager.OpenedData
    Populate()
  End Sub

  Private Sub pMatchingGrid_UserResizedColumn(ByVal aGrid As atcGrid, ByVal aColumn As Integer, ByVal aWidth As Integer) Handles pMatchingGrid.UserResizedColumn
    pSelectedGrid.ColumnWidth(aColumn) = aWidth
    pSelectedGrid.Refresh()
    ResizeOneCriteria(aColumn - 1, aWidth)
  End Sub

  Private Sub pSelectedGrid_UserResizedColumn(ByVal aGrid As atcGrid, ByVal aColumn As Integer, ByVal aWidth As Integer) Handles pSelectedGrid.UserResizedColumn
    pMatchingGrid.ColumnWidth(aColumn) = aWidth
    pMatchingGrid.Refresh()
    ResizeOneCriteria(aColumn - 1, aWidth)
  End Sub

  Private Sub mnuAttributesAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAttributesAdd.Click
    AddCriteria()
    UpdatedCriteria()
    SizeCriteria()
  End Sub

  Private Sub mnuSelectClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuSelectClear.Click
    If pSelectedGroup.Count > 0 Then
      pSelectedGroup.Clear()
      RefreshSelected()
    End If
  End Sub

  Private Sub mnuSelectAllMatching_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuSelectAllMatching.Click
    'For Each ts As atcDataSet In pMatchingGroup
    '  If Not pSelectedGroup.Contains(ts) Then pSelectedGroup.Add(ts)
    'Next
    'RefreshSelected()
    Dim lAdd As New atcCollection
    For Each ts As atcDataSet In pMatchingGroup
      If Not pSelectedGroup.Contains(ts) Then lAdd.Add(ts)
    Next
    If lAdd.Count > 0 Then
      pSelectedGroup.Add(lAdd)
      RefreshSelected()
    End If
  End Sub

  Private Sub mnuSelectNoMatching_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuSelectNoMatching.Click
    Dim lRemove As New atcCollection
    For Each ts As atcDataSet In pMatchingGroup
      If pSelectedGroup.Contains(ts) Then lRemove.Add(ts)
    Next
    If lRemove.Count > 0 Then
      pSelectedGroup.Remove(lRemove)
      RefreshSelected()
    End If
  End Sub

  Private Sub mnuFileManage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFileManage.Click
    pDataManager.UserManage() ' .OpenData("")
  End Sub

  Private Sub mnuRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    Dim mnu As MenuItem = sender
    Dim index As Integer = mnu.Index
    RemoveCriteria(pcboCriteria(index), plstCriteria(index))
  End Sub

  Private Sub mnuMove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    'TODO: re-order criteria
  End Sub

  Private Sub mnuAddData_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAddData.Click
    Dim lNewSource As atcDataSource = pDataManager.UserSelectDataSource
    If Not lNewSource Is Nothing Then
      pDataManager.OpenDataSource(lNewSource, lNewSource.Specification, Nothing)
    End If
  End Sub

  Private Sub mnuSelectAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuSelectAll.Click
    Dim lAdd As New atcCollection
    For Each ts As atcDataSet In pDataManager.DataSets
      If Not pSelectedGroup.Contains(ts) Then lAdd.Add(ts)
    Next
    If lAdd.Count > 0 Then
      pSelectedGroup.Add(lAdd) 'TODO: add with IDs as keys
      RefreshSelected()
    End If
  End Sub

  Private Sub frmSelectData_VisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.VisibleChanged
    If Visible Then SizeCriteria()
  End Sub

  Private Sub frmSelectData_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Closed
    pDataManager = Nothing
    pMatchingGroup = Nothing
    pSaveGroup = Nothing
    pMatchingSource = Nothing
    pSelectedSource = Nothing
  End Sub
End Class

Friend Class GridSource
  Inherits atcControls.atcGridSource

  ' 0 to label the columns in row 0
  '-1 to not label columns
  Private Const LabelRow As Integer = -1

  Private pDataManager As atcDataManager
  Private pDataGroup As atcDataGroup
  Private pSelected As atcCollection

  Public Property SelectedItems() As atcCollection
    Get
      Return pSelected
    End Get
    Set(ByVal newValue As atcCollection)
      pSelected = newValue
    End Set
  End Property

  Sub New(ByVal aDataManager As atcData.atcDataManager, _
          ByVal aDataGroup As atcData.atcDataGroup)
    pDataManager = aDataManager
    pDataGroup = aDataGroup
  End Sub

  Overrides Property Columns() As Integer
    Get
      Return pDataManager.SelectionAttributes.Count() + 1
    End Get
    Set(ByVal Value As Integer)
    End Set
  End Property

  Overrides Property Rows() As Integer
    Get
      Return pDataGroup.Count + LabelRow + 1
    End Get
    Set(ByVal Value As Integer)
    End Set
  End Property

  Overrides Property CellValue(ByVal aRow As Integer, ByVal aColumn As Integer) As String
    Get
      If aRow = LabelRow Then
        If aColumn = 0 Then
          Return ""
        Else
          Return pDataManager.SelectionAttributes(aColumn - 1)
        End If
      ElseIf aColumn = 0 Then
        Return pDataGroup(aRow - (LabelRow + 1)).Serial()
      Else
        Return pDataGroup(aRow - (LabelRow + 1)).Attributes.GetFormattedValue(pDataManager.SelectionAttributes(aColumn - 1))
      End If
    End Get
    Set(ByVal Value As String)
    End Set
  End Property

  Overrides Property Alignment(ByVal aRow As Integer, ByVal aColumn As Integer) As atcControls.atcAlignment
    Get
      If aRow > LabelRow AndAlso aColumn > 0 Then
        Dim lAttributeDef As atcAttributeDefinition = atcDataAttributes.GetDefinition(pDataManager.SelectionAttributes(aColumn - 1))
        If Not lAttributeDef Is Nothing Then
          Select Case lAttributeDef.TypeString.ToLower
            Case "integer", "single", "double"
              Return atcAlignment.HAlignDecimal
          End Select
        End If
      End If
      Return atcControls.atcAlignment.HAlignLeft
    End Get
    Set(ByVal Value As atcControls.atcAlignment)
    End Set
  End Property

  Overrides Property CellSelected(ByVal aRow As Integer, ByVal aColumn As Integer) As Boolean
    Get
      If Not pSelected Is Nothing Then
        If aRow = LabelRow Then
          Return False
        Else
          Return pSelected.Contains(pDataGroup(aRow - (LabelRow + 1)))
        End If
      End If
      Return False
    End Get
    Set(ByVal newValue As Boolean)
    End Set
  End Property

End Class

Friend Class ListSource
  Inherits atcControls.atcGridSource

  Private pAlignment As atcAlignment = atcAlignment.HAlignDecimal
  Private pValues As atcCollection
  Private pSelected As atcCollection

  Public Property SelectedItems() As atcCollection
    Get
      Return pSelected
    End Get
    Set(ByVal newValue As atcCollection)
      pSelected = newValue
    End Set
  End Property

  Sub New(ByVal aValues As atcCollection, Optional ByVal aSelected As atcCollection = Nothing)
    pValues = aValues
    If aSelected Is Nothing Then
      pSelected = New atcCollection
    Else
      pSelected = aSelected
    End If
  End Sub

  Overrides Property Columns() As Integer
    Get
      Return 1
    End Get
    Set(ByVal Value As Integer)
    End Set
  End Property

  Overrides Property Rows() As Integer
    Get
      If pValues Is Nothing Then Return 1
      Return pValues.Count
    End Get
    Set(ByVal Value As Integer)
    End Set
  End Property

  Overrides Property CellValue(ByVal aRow As Integer, ByVal aColumn As Integer) As String
    Get
      Try
        Return pValues.ItemByIndex(aRow)
      Catch
        Return ""
      End Try
    End Get
    Set(ByVal Value As String)
    End Set
  End Property

  Overrides Property Alignment(ByVal aRow As Integer, ByVal aColumn As Integer) As atcControls.atcAlignment
    Get
      Return pAlignment
    End Get
    Set(ByVal newValue As atcControls.atcAlignment)
      pAlignment = newValue
    End Set
  End Property

  'Overrides Property CellColor(ByVal aRow As Integer, ByVal aColumn As Integer) As System.Drawing.Color
  '  Get
  '    If pSelected.Contains(CellValue(aRow, aColumn)) Then
  '      Return System.Drawing.SystemColors.Highlight
  '    Else
  '      Return System.Drawing.SystemColors.Window 'TODO: use grid's CellBackColor
  '    End If
  '  End Get
  '  Set(ByVal Value As System.Drawing.Color)
  '  End Set
  'End Property

  Overrides Property CellSelected(ByVal aRow As Integer, ByVal aColumn As Integer) As Boolean
    Get
      Return pSelected.Keys.Contains(aRow) ' & "," & aColumn)
    End Get
    Set(ByVal newValue As Boolean)
      If newValue Then
        If Not pSelected.Keys.Contains(aRow) Then
          pSelected.Add(aRow, CellValue(aRow, aColumn))
        End If
      Else
        pSelected.RemoveByKey(aRow)
      End If
    End Set
  End Property
End Class