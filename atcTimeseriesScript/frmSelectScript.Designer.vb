<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmSelectScript
#Region "Windows Form Designer generated code "
    <System.Diagnostics.DebuggerNonUserCode()> Public Sub New()
        MyBase.New()
        'This call is required by the Windows Form Designer.
        InitializeComponent()
    End Sub
    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> Protected Overloads Overrides Sub Dispose(ByVal Disposing As Boolean)
        If Disposing Then
            If Not components Is Nothing Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(Disposing)
    End Sub
    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer
    Public ToolTip1 As System.Windows.Forms.ToolTip
    Public WithEvents cmdRun As System.Windows.Forms.Button
    Public WithEvents cmdWizard As System.Windows.Forms.Button
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Public WithEvents cmdFind As System.Windows.Forms.Button
    Public WithEvents cmdDelete As System.Windows.Forms.Button
    Public WithEvents cmdTest As System.Windows.Forms.Button
    Public WithEvents fraButtons As System.Windows.Forms.Panel
    Public dlgOpenFileOpen As System.Windows.Forms.OpenFileDialog
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.fraButtons = New System.Windows.Forms.Panel
        Me.cmdRun = New System.Windows.Forms.Button
        Me.cmdWizard = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdFind = New System.Windows.Forms.Button
        Me.cmdDelete = New System.Windows.Forms.Button
        Me.cmdTest = New System.Windows.Forms.Button
        Me.dlgOpenFileOpen = New System.Windows.Forms.OpenFileDialog
        Me.agdScripts = New atcControls.atcGrid
        Me.fraButtons.SuspendLayout()
        Me.SuspendLayout()
        '
        'fraButtons
        '
        Me.fraButtons.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.fraButtons.BackColor = System.Drawing.SystemColors.Control
        Me.fraButtons.Controls.Add(Me.cmdRun)
        Me.fraButtons.Controls.Add(Me.cmdWizard)
        Me.fraButtons.Controls.Add(Me.cmdCancel)
        Me.fraButtons.Controls.Add(Me.cmdFind)
        Me.fraButtons.Controls.Add(Me.cmdDelete)
        Me.fraButtons.Controls.Add(Me.cmdTest)
        Me.fraButtons.Cursor = System.Windows.Forms.Cursors.Default
        Me.fraButtons.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraButtons.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraButtons.Location = New System.Drawing.Point(560, 8)
        Me.fraButtons.Name = "fraButtons"
        Me.fraButtons.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraButtons.Size = New System.Drawing.Size(89, 185)
        Me.fraButtons.TabIndex = 7
        Me.fraButtons.Text = "Frame1"
        '
        'cmdRun
        '
        Me.cmdRun.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdRun.BackColor = System.Drawing.SystemColors.Control
        Me.cmdRun.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdRun.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdRun.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdRun.Location = New System.Drawing.Point(0, 0)
        Me.cmdRun.Name = "cmdRun"
        Me.cmdRun.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdRun.Size = New System.Drawing.Size(89, 25)
        Me.cmdRun.TabIndex = 1
        Me.cmdRun.Text = "&Run"
        Me.cmdRun.UseVisualStyleBackColor = False
        '
        'cmdWizard
        '
        Me.cmdWizard.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdWizard.BackColor = System.Drawing.SystemColors.Control
        Me.cmdWizard.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdWizard.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdWizard.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdWizard.Location = New System.Drawing.Point(0, 32)
        Me.cmdWizard.Name = "cmdWizard"
        Me.cmdWizard.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdWizard.Size = New System.Drawing.Size(89, 25)
        Me.cmdWizard.TabIndex = 2
        Me.cmdWizard.Text = "&Edit"
        Me.cmdWizard.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(0, 160)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(89, 25)
        Me.cmdCancel.TabIndex = 6
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdFind
        '
        Me.cmdFind.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdFind.BackColor = System.Drawing.SystemColors.Control
        Me.cmdFind.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdFind.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdFind.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdFind.Location = New System.Drawing.Point(0, 64)
        Me.cmdFind.Name = "cmdFind"
        Me.cmdFind.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdFind.Size = New System.Drawing.Size(89, 25)
        Me.cmdFind.TabIndex = 3
        Me.cmdFind.Text = "&Find..."
        Me.cmdFind.UseVisualStyleBackColor = False
        '
        'cmdDelete
        '
        Me.cmdDelete.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdDelete.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDelete.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDelete.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDelete.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDelete.Location = New System.Drawing.Point(0, 96)
        Me.cmdDelete.Name = "cmdDelete"
        Me.cmdDelete.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDelete.Size = New System.Drawing.Size(89, 25)
        Me.cmdDelete.TabIndex = 4
        Me.cmdDelete.Text = "For&get"
        Me.cmdDelete.UseVisualStyleBackColor = False
        '
        'cmdTest
        '
        Me.cmdTest.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdTest.BackColor = System.Drawing.SystemColors.Control
        Me.cmdTest.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdTest.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdTest.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdTest.Location = New System.Drawing.Point(0, 128)
        Me.cmdTest.Name = "cmdTest"
        Me.cmdTest.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdTest.Size = New System.Drawing.Size(89, 25)
        Me.cmdTest.TabIndex = 5
        Me.cmdTest.Text = "&Debug"
        Me.cmdTest.UseVisualStyleBackColor = False
        '
        'agdScripts
        '
        Me.agdScripts.AllowHorizontalScrolling = True
        Me.agdScripts.AllowNewValidValues = False
        Me.agdScripts.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.agdScripts.CellBackColor = System.Drawing.SystemColors.Window
        Me.agdScripts.Fixed3D = False
        Me.agdScripts.LineColor = System.Drawing.SystemColors.Control
        Me.agdScripts.LineWidth = 1.0!
        Me.agdScripts.Location = New System.Drawing.Point(8, 8)
        Me.agdScripts.Name = "agdScripts"
        Me.agdScripts.Size = New System.Drawing.Size(545, 217)
        Me.agdScripts.Source = Nothing
        Me.agdScripts.TabIndex = 0
        '
        'frmSelectScript
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdCancel
        Me.ClientSize = New System.Drawing.Size(656, 231)
        Me.Controls.Add(Me.fraButtons)
        Me.Controls.Add(Me.agdScripts)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Location = New System.Drawing.Point(3, 18)
        Me.Name = "frmSelectScript"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text = "Script Selection for Import"
        Me.fraButtons.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Public WithEvents agdScripts As atcControls.atcGrid
#End Region
End Class