<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmOutput
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmOutput))
        Me.txtOutputPath = New System.Windows.Forms.TextBox
        Me.gboOutputSelect = New System.Windows.Forms.GroupBox
        Me.rdoRoraSum = New System.Windows.Forms.RadioButton
        Me.rdoRoraWY = New System.Windows.Forms.RadioButton
        Me.rdoRoraQrt = New System.Windows.Forms.RadioButton
        Me.rdoRoraPek = New System.Windows.Forms.RadioButton
        Me.rdoRoraMon = New System.Windows.Forms.RadioButton
        Me.txtResultFileContent = New System.Windows.Forms.TextBox
        Me.gboOutputSelect.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtOutputPath
        '
        Me.txtOutputPath.Dock = System.Windows.Forms.DockStyle.Top
        Me.txtOutputPath.Location = New System.Drawing.Point(0, 0)
        Me.txtOutputPath.Name = "txtOutputPath"
        Me.txtOutputPath.Size = New System.Drawing.Size(532, 20)
        Me.txtOutputPath.TabIndex = 0
        '
        'gboOutputSelect
        '
        Me.gboOutputSelect.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gboOutputSelect.Controls.Add(Me.rdoRoraSum)
        Me.gboOutputSelect.Controls.Add(Me.rdoRoraWY)
        Me.gboOutputSelect.Controls.Add(Me.rdoRoraQrt)
        Me.gboOutputSelect.Controls.Add(Me.rdoRoraPek)
        Me.gboOutputSelect.Controls.Add(Me.rdoRoraMon)
        Me.gboOutputSelect.Location = New System.Drawing.Point(12, 27)
        Me.gboOutputSelect.Name = "gboOutputSelect"
        Me.gboOutputSelect.Size = New System.Drawing.Size(508, 48)
        Me.gboOutputSelect.TabIndex = 1
        Me.gboOutputSelect.TabStop = False
        Me.gboOutputSelect.Text = "Select Output File Type"
        '
        'rdoRoraSum
        '
        Me.rdoRoraSum.AutoSize = True
        Me.rdoRoraSum.Location = New System.Drawing.Point(270, 20)
        Me.rdoRoraSum.Name = "rdoRoraSum"
        Me.rdoRoraSum.Size = New System.Drawing.Size(62, 17)
        Me.rdoRoraSum.TabIndex = 4
        Me.rdoRoraSum.TabStop = True
        Me.rdoRoraSum.Text = "rorasum"
        Me.rdoRoraSum.UseVisualStyleBackColor = True
        '
        'rdoRoraWY
        '
        Me.rdoRoraWY.AutoSize = True
        Me.rdoRoraWY.Location = New System.Drawing.Point(207, 20)
        Me.rdoRoraWY.Name = "rdoRoraWY"
        Me.rdoRoraWY.Size = New System.Drawing.Size(56, 17)
        Me.rdoRoraWY.TabIndex = 3
        Me.rdoRoraWY.TabStop = True
        Me.rdoRoraWY.Text = "rorawy"
        Me.rdoRoraWY.UseVisualStyleBackColor = True
        '
        'rdoRoraQrt
        '
        Me.rdoRoraQrt.AutoSize = True
        Me.rdoRoraQrt.Location = New System.Drawing.Point(145, 20)
        Me.rdoRoraQrt.Name = "rdoRoraQrt"
        Me.rdoRoraQrt.Size = New System.Drawing.Size(55, 17)
        Me.rdoRoraQrt.TabIndex = 2
        Me.rdoRoraQrt.TabStop = True
        Me.rdoRoraQrt.Text = "roraqrt"
        Me.rdoRoraQrt.UseVisualStyleBackColor = True
        '
        'rdoRoraPek
        '
        Me.rdoRoraPek.AutoSize = True
        Me.rdoRoraPek.Location = New System.Drawing.Point(77, 20)
        Me.rdoRoraPek.Name = "rdoRoraPek"
        Me.rdoRoraPek.Size = New System.Drawing.Size(61, 17)
        Me.rdoRoraPek.TabIndex = 1
        Me.rdoRoraPek.TabStop = True
        Me.rdoRoraPek.Text = "rorapek"
        Me.rdoRoraPek.UseVisualStyleBackColor = True
        '
        'rdoRoraMon
        '
        Me.rdoRoraMon.AutoSize = True
        Me.rdoRoraMon.Location = New System.Drawing.Point(7, 20)
        Me.rdoRoraMon.Name = "rdoRoraMon"
        Me.rdoRoraMon.Size = New System.Drawing.Size(63, 17)
        Me.rdoRoraMon.TabIndex = 0
        Me.rdoRoraMon.TabStop = True
        Me.rdoRoraMon.Text = "roramon"
        Me.rdoRoraMon.UseVisualStyleBackColor = True
        '
        'txtResultFileContent
        '
        Me.txtResultFileContent.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtResultFileContent.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtResultFileContent.Location = New System.Drawing.Point(13, 81)
        Me.txtResultFileContent.Multiline = True
        Me.txtResultFileContent.Name = "txtResultFileContent"
        Me.txtResultFileContent.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtResultFileContent.Size = New System.Drawing.Size(507, 384)
        Me.txtResultFileContent.TabIndex = 2
        '
        'frmOutput
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(532, 477)
        Me.Controls.Add(Me.txtResultFileContent)
        Me.Controls.Add(Me.gboOutputSelect)
        Me.Controls.Add(Me.txtOutputPath)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmOutput"
        Me.Text = "RORA Output"
        Me.gboOutputSelect.ResumeLayout(False)
        Me.gboOutputSelect.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtOutputPath As System.Windows.Forms.TextBox
    Friend WithEvents gboOutputSelect As System.Windows.Forms.GroupBox
    Friend WithEvents txtResultFileContent As System.Windows.Forms.TextBox
    Friend WithEvents rdoRoraSum As System.Windows.Forms.RadioButton
    Friend WithEvents rdoRoraWY As System.Windows.Forms.RadioButton
    Friend WithEvents rdoRoraQrt As System.Windows.Forms.RadioButton
    Friend WithEvents rdoRoraPek As System.Windows.Forms.RadioButton
    Friend WithEvents rdoRoraMon As System.Windows.Forms.RadioButton
End Class
