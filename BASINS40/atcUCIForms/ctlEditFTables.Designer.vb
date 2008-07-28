<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ctlEditFTables
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Me.grdEdit = New atcControls.atcGrid
        Me.cmdImport = New System.Windows.Forms.Button
        Me.cmdCompute = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'grdEdit
        '
        Me.grdEdit.AllowHorizontalScrolling = True
        Me.grdEdit.AllowNewValidValues = False
        Me.grdEdit.CellBackColor = System.Drawing.Color.Empty
        Me.grdEdit.LineColor = System.Drawing.Color.Empty
        Me.grdEdit.LineWidth = 0.0!
        Me.grdEdit.Location = New System.Drawing.Point(0, 0)
        Me.grdEdit.Name = "grdEdit"
        Me.grdEdit.Size = New System.Drawing.Size(470, 264)
        Me.grdEdit.Source = Nothing
        Me.grdEdit.TabIndex = 0
        '
        'cmdImport
        '
        Me.cmdImport.Location = New System.Drawing.Point(476, 120)
        Me.cmdImport.Name = "cmdImport"
        Me.cmdImport.Size = New System.Drawing.Size(130, 47)
        Me.cmdImport.TabIndex = 1
        Me.cmdImport.Text = "Import From Cross Section"
        Me.cmdImport.UseVisualStyleBackColor = True
        '
        'cmdCompute
        '
        Me.cmdCompute.Location = New System.Drawing.Point(475, 173)
        Me.cmdCompute.Name = "cmdCompute"
        Me.cmdCompute.Size = New System.Drawing.Size(129, 47)
        Me.cmdCompute.TabIndex = 2
        Me.cmdCompute.Text = "Compute New"
        Me.cmdCompute.UseVisualStyleBackColor = True
        '
        'ctlEditFTables
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.cmdCompute)
        Me.Controls.Add(Me.cmdImport)
        Me.Controls.Add(Me.grdEdit)
        Me.Name = "ctlEditFTables"
        Me.Size = New System.Drawing.Size(607, 264)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grdEdit As atcControls.atcGrid
    Friend WithEvents cmdImport As System.Windows.Forms.Button
    Friend WithEvents cmdCompute As System.Windows.Forms.Button

End Class
