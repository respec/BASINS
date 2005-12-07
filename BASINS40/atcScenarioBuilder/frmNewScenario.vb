Public Class frmScenarioEditor
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
  Friend WithEvents lblName As System.Windows.Forms.Label
  Friend WithEvents txtName As System.Windows.Forms.TextBox
  Friend WithEvents cboBase As System.Windows.Forms.ComboBox
  Friend WithEvents lblBase As System.Windows.Forms.Label
  Friend WithEvents lstOperations As System.Windows.Forms.ListBox
  Friend WithEvents lblOperations As System.Windows.Forms.Label
  Friend WithEvents btnOk As System.Windows.Forms.Button
  Friend WithEvents btnAddOperation As System.Windows.Forms.Button
  Friend WithEvents btnRemoveOperation As System.Windows.Forms.Button
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
    Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmScenarioEditor))
    Me.lblName = New System.Windows.Forms.Label
    Me.txtName = New System.Windows.Forms.TextBox
    Me.cboBase = New System.Windows.Forms.ComboBox
    Me.lblBase = New System.Windows.Forms.Label
    Me.lstOperations = New System.Windows.Forms.ListBox
    Me.lblOperations = New System.Windows.Forms.Label
    Me.btnOk = New System.Windows.Forms.Button
    Me.btnAddOperation = New System.Windows.Forms.Button
    Me.btnRemoveOperation = New System.Windows.Forms.Button
    Me.SuspendLayout()
    '
    'lblName
    '
    Me.lblName.Location = New System.Drawing.Point(8, 20)
    Me.lblName.Name = "lblName"
    Me.lblName.Size = New System.Drawing.Size(96, 16)
    Me.lblName.TabIndex = 0
    Me.lblName.Text = "Scenario Name"
    '
    'txtName
    '
    Me.txtName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtName.Location = New System.Drawing.Point(112, 16)
    Me.txtName.Name = "txtName"
    Me.txtName.Size = New System.Drawing.Size(176, 20)
    Me.txtName.TabIndex = 1
    Me.txtName.Text = ""
    '
    'cboBase
    '
    Me.cboBase.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.cboBase.Location = New System.Drawing.Point(112, 56)
    Me.cboBase.Name = "cboBase"
    Me.cboBase.Size = New System.Drawing.Size(176, 21)
    Me.cboBase.TabIndex = 2
    '
    'lblBase
    '
    Me.lblBase.Location = New System.Drawing.Point(8, 60)
    Me.lblBase.Name = "lblBase"
    Me.lblBase.Size = New System.Drawing.Size(96, 16)
    Me.lblBase.TabIndex = 3
    Me.lblBase.Text = "Base Scenario"
    '
    'lstOperations
    '
    Me.lstOperations.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lstOperations.Location = New System.Drawing.Point(8, 120)
    Me.lstOperations.Name = "lstOperations"
    Me.lstOperations.Size = New System.Drawing.Size(280, 147)
    Me.lstOperations.TabIndex = 4
    '
    'lblOperations
    '
    Me.lblOperations.Location = New System.Drawing.Point(8, 96)
    Me.lblOperations.Name = "lblOperations"
    Me.lblOperations.Size = New System.Drawing.Size(88, 16)
    Me.lblOperations.TabIndex = 5
    Me.lblOperations.Text = "Operations"
    '
    'btnOk
    '
    Me.btnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
    Me.btnOk.Location = New System.Drawing.Point(120, 320)
    Me.btnOk.Name = "btnOk"
    Me.btnOk.Size = New System.Drawing.Size(56, 24)
    Me.btnOk.TabIndex = 6
    Me.btnOk.Text = "Ok"
    '
    'btnAddOperation
    '
    Me.btnAddOperation.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
    Me.btnAddOperation.Location = New System.Drawing.Point(64, 280)
    Me.btnAddOperation.Name = "btnAddOperation"
    Me.btnAddOperation.Size = New System.Drawing.Size(56, 24)
    Me.btnAddOperation.TabIndex = 7
    Me.btnAddOperation.Text = "Add"
    '
    'btnRemoveOperation
    '
    Me.btnRemoveOperation.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
    Me.btnRemoveOperation.Location = New System.Drawing.Point(176, 280)
    Me.btnRemoveOperation.Name = "btnRemoveOperation"
    Me.btnRemoveOperation.Size = New System.Drawing.Size(56, 24)
    Me.btnRemoveOperation.TabIndex = 8
    Me.btnRemoveOperation.Text = "Remove"
    '
    'frmScenarioEditor
    '
    Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
    Me.ClientSize = New System.Drawing.Size(296, 357)
    Me.Controls.Add(Me.btnRemoveOperation)
    Me.Controls.Add(Me.btnAddOperation)
    Me.Controls.Add(Me.btnOk)
    Me.Controls.Add(Me.lblOperations)
    Me.Controls.Add(Me.lstOperations)
    Me.Controls.Add(Me.lblBase)
    Me.Controls.Add(Me.cboBase)
    Me.Controls.Add(Me.txtName)
    Me.Controls.Add(Me.lblName)
    Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
    Me.Name = "frmScenarioEditor"
    Me.Text = "Scenario Editor"
    Me.ResumeLayout(False)

  End Sub

#End Region

  Private Sub btnAddOperation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddOperation.Click

  End Sub

  Private Sub btnRemoveOperation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemoveOperation.Click

  End Sub

  Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click

  End Sub

  Protected Overrides Sub OnClosing(ByVal e As System.ComponentModel.CancelEventArgs)
    'pDataManager = Nothing
    'pSource = Nothing
  End Sub

End Class
