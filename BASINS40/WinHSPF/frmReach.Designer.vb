<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmReach
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.grdReach = New atcControls.atcGrid
        Me.AtcGridBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.HspfFtableBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.cmdOK = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.FTables = New System.Windows.Forms.Button
        CType(Me.AtcGridBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.HspfFtableBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'grdReach
        '
        Me.grdReach.AllowHorizontalScrolling = True
        Me.grdReach.AllowNewValidValues = False
        Me.grdReach.CellBackColor = System.Drawing.Color.Empty
        Me.grdReach.LineColor = System.Drawing.Color.Empty
        Me.grdReach.LineWidth = 0.0!
        Me.grdReach.Location = New System.Drawing.Point(10, 13)
        Me.grdReach.Margin = New System.Windows.Forms.Padding(2)
        Me.grdReach.Name = "grdReach"
        Me.grdReach.Size = New System.Drawing.Size(750, 376)
        Me.grdReach.Source = Nothing
        Me.grdReach.TabIndex = 0
        '
        'AtcGridBindingSource
        '
        Me.AtcGridBindingSource.DataSource = GetType(atcControls.atcGrid)
        '
        'HspfFtableBindingSource
        '
        Me.HspfFtableBindingSource.DataSource = GetType(atcUCI.HspfFtable)
        '
        'DataGridViewTextBoxColumn1
        '
        Me.DataGridViewTextBoxColumn1.DataPropertyName = "Tag"
        Me.DataGridViewTextBoxColumn1.HeaderText = "Tag"
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        '
        'cmdOK
        '
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.Location = New System.Drawing.Point(243, 433)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.Size = New System.Drawing.Size(99, 36)
        Me.cmdOK.TabIndex = 1
        Me.cmdOK.Text = "OK"
        Me.cmdOK.UseVisualStyleBackColor = True
        '
        'cmdCancel
        '
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.Location = New System.Drawing.Point(348, 433)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(99, 36)
        Me.cmdCancel.TabIndex = 2
        Me.cmdCancel.Text = "Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = True
        '
        'FTables
        '
        Me.FTables.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FTables.Location = New System.Drawing.Point(660, 433)
        Me.FTables.Name = "FTables"
        Me.FTables.Size = New System.Drawing.Size(99, 36)
        Me.FTables.TabIndex = 3
        Me.FTables.Text = "FTables"
        Me.FTables.UseVisualStyleBackColor = True
        '
        'frmReach
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(771, 481)
        Me.Controls.Add(Me.FTables)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.grdReach)
        Me.Margin = New System.Windows.Forms.Padding(2)
        Me.Name = "frmReach"
        Me.Text = "WinHSPF - Reach Editor"
        CType(Me.AtcGridBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.HspfFtableBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grdReach As atcControls.atcGrid
    Friend WithEvents AtcGridBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents DataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents HspfFtableBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents cmdOK As System.Windows.Forms.Button
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
    Friend WithEvents FTables As System.Windows.Forms.Button
End Class
