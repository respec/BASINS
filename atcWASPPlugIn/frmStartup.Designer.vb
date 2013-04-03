<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmStartup
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
        Me.lnkCreateNew = New System.Windows.Forms.LinkLabel()
        Me.lnkOpenExisting = New System.Windows.Forms.LinkLabel()
        Me.lnkOpenLast = New System.Windows.Forms.LinkLabel()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'lnkCreateNew
        '
        Me.lnkCreateNew.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lnkCreateNew.AutoSize = True
        Me.lnkCreateNew.Font = New System.Drawing.Font("Tahoma", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lnkCreateNew.Location = New System.Drawing.Point(3, 8)
        Me.lnkCreateNew.Name = "lnkCreateNew"
        Me.lnkCreateNew.Size = New System.Drawing.Size(367, 18)
        Me.lnkCreateNew.TabIndex = 0
        Me.lnkCreateNew.TabStop = True
        Me.lnkCreateNew.Text = "1. Select stream reaches and build new WASP model..."
        '
        'lnkOpenExisting
        '
        Me.lnkOpenExisting.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lnkOpenExisting.AutoSize = True
        Me.lnkOpenExisting.Font = New System.Drawing.Font("Tahoma", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lnkOpenExisting.Location = New System.Drawing.Point(3, 43)
        Me.lnkOpenExisting.Name = "lnkOpenExisting"
        Me.lnkOpenExisting.Size = New System.Drawing.Size(334, 18)
        Me.lnkOpenExisting.TabIndex = 0
        Me.lnkOpenExisting.TabStop = True
        Me.lnkOpenExisting.Text = "2. Open an existing WASP Model Builder project..."
        '
        'lnkOpenLast
        '
        Me.lnkOpenLast.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lnkOpenLast.AutoSize = True
        Me.lnkOpenLast.Font = New System.Drawing.Font("Tahoma", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lnkOpenLast.Location = New System.Drawing.Point(3, 78)
        Me.lnkOpenLast.Name = "lnkOpenLast"
        Me.lnkOpenLast.Size = New System.Drawing.Size(320, 18)
        Me.lnkOpenLast.TabIndex = 0
        Me.lnkOpenLast.TabStop = True
        Me.lnkOpenLast.Text = "3. Reopen your last WASP Model Builder project"
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 1
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.lnkCreateNew, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.lnkOpenLast, 0, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.lnkOpenExisting, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.btnClose, 0, 4)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(12, 12)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 5
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 15.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(387, 150)
        Me.TableLayoutPanel1.TabIndex = 1
        '
        'btnClose
        '
        Me.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnClose.Location = New System.Drawing.Point(309, 123)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(75, 23)
        Me.btnClose.TabIndex = 2
        Me.btnClose.Text = "Close"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'frmStartup
        '
        Me.AcceptButton = Me.lnkOpenExisting
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnClose
        Me.ClientSize = New System.Drawing.Size(411, 174)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmStartup"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Welcome to BASINS WASP Model Builder"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lnkCreateNew As System.Windows.Forms.LinkLabel
    Friend WithEvents lnkOpenExisting As System.Windows.Forms.LinkLabel
    Friend WithEvents lnkOpenLast As System.Windows.Forms.LinkLabel
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents btnClose As System.Windows.Forms.Button

End Class
