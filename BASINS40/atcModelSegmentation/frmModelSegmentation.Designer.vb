<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmModelSegmentation
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmModelSegmentation))
        Me.cmdAssign = New System.Windows.Forms.Button
        Me.cmdDisplay = New System.Windows.Forms.Button
        Me.cmdInput = New System.Windows.Forms.Button
        Me.fraSubbasins = New System.Windows.Forms.GroupBox
        Me.cboSubbasins = New System.Windows.Forms.ComboBox
        Me.fraSubbasins.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdAssign
        '
        Me.cmdAssign.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdAssign.Location = New System.Drawing.Point(12, 103)
        Me.cmdAssign.Name = "cmdAssign"
        Me.cmdAssign.Size = New System.Drawing.Size(324, 34)
        Me.cmdAssign.TabIndex = 0
        Me.cmdAssign.Text = "Assign Met Stations By Proximity"
        Me.cmdAssign.UseVisualStyleBackColor = True
        '
        'cmdDisplay
        '
        Me.cmdDisplay.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdDisplay.Location = New System.Drawing.Point(12, 203)
        Me.cmdDisplay.Name = "cmdDisplay"
        Me.cmdDisplay.Size = New System.Drawing.Size(324, 34)
        Me.cmdDisplay.TabIndex = 1
        Me.cmdDisplay.Text = "Display Segments Thematically"
        Me.cmdDisplay.UseVisualStyleBackColor = True
        '
        'cmdInput
        '
        Me.cmdInput.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdInput.Location = New System.Drawing.Point(12, 152)
        Me.cmdInput.Name = "cmdInput"
        Me.cmdInput.Size = New System.Drawing.Size(323, 34)
        Me.cmdInput.TabIndex = 2
        Me.cmdInput.Text = "Input Segment IDs for Selected"
        Me.cmdInput.UseVisualStyleBackColor = True
        '
        'fraSubbasins
        '
        Me.fraSubbasins.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.fraSubbasins.Controls.Add(Me.cboSubbasins)
        Me.fraSubbasins.Location = New System.Drawing.Point(12, 12)
        Me.fraSubbasins.Name = "fraSubbasins"
        Me.fraSubbasins.Size = New System.Drawing.Size(323, 72)
        Me.fraSubbasins.TabIndex = 11
        Me.fraSubbasins.TabStop = False
        Me.fraSubbasins.Text = "Subbasins Layer"
        '
        'cboSubbasins
        '
        Me.cboSubbasins.AllowDrop = True
        Me.cboSubbasins.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboSubbasins.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSubbasins.Location = New System.Drawing.Point(18, 30)
        Me.cboSubbasins.Name = "cboSubbasins"
        Me.cboSubbasins.Size = New System.Drawing.Size(285, 24)
        Me.cboSubbasins.TabIndex = 11
        '
        'frmModelSegmentation
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(353, 259)
        Me.Controls.Add(Me.fraSubbasins)
        Me.Controls.Add(Me.cmdInput)
        Me.Controls.Add(Me.cmdDisplay)
        Me.Controls.Add(Me.cmdAssign)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmModelSegmentation"
        Me.Text = "BASINS Model Segmentation"
        Me.fraSubbasins.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents cmdAssign As System.Windows.Forms.Button
    Friend WithEvents cmdDisplay As System.Windows.Forms.Button
    Friend WithEvents cmdInput As System.Windows.Forms.Button
    Friend WithEvents fraSubbasins As System.Windows.Forms.GroupBox
    Friend WithEvents cboSubbasins As System.Windows.Forms.ComboBox
End Class
