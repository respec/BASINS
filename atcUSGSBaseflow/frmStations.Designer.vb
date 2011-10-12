<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmStations
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmStations))
        Me.txtHeader = New System.Windows.Forms.TextBox
        Me.dgvStationEntries = New System.Windows.Forms.DataGridView
        Me.fld1RDBFilename = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.fld2DrainageArea = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.fld3StationID = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.fld4StationInfo = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.btnOK = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        CType(Me.dgvStationEntries, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtHeader
        '
        Me.txtHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtHeader.Dock = System.Windows.Forms.DockStyle.Top
        Me.txtHeader.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtHeader.Location = New System.Drawing.Point(0, 0)
        Me.txtHeader.Multiline = True
        Me.txtHeader.Name = "txtHeader"
        Me.txtHeader.ReadOnly = True
        Me.txtHeader.Size = New System.Drawing.Size(584, 75)
        Me.txtHeader.TabIndex = 0
        '
        'dgvStationEntries
        '
        Me.dgvStationEntries.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvStationEntries.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvStationEntries.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.fld1RDBFilename, Me.fld2DrainageArea, Me.fld3StationID, Me.fld4StationInfo})
        Me.dgvStationEntries.Location = New System.Drawing.Point(0, 81)
        Me.dgvStationEntries.MultiSelect = False
        Me.dgvStationEntries.Name = "dgvStationEntries"
        Me.dgvStationEntries.Size = New System.Drawing.Size(584, 199)
        Me.dgvStationEntries.TabIndex = 1
        '
        'fld1RDBFilename
        '
        Me.fld1RDBFilename.HeaderText = "RDB Filename"
        Me.fld1RDBFilename.Name = "fld1RDBFilename"
        '
        'fld2DrainageArea
        '
        Me.fld2DrainageArea.HeaderText = "Drainage Area (sq mi)"
        Me.fld2DrainageArea.Name = "fld2DrainageArea"
        '
        'fld3StationID
        '
        Me.fld3StationID.HeaderText = "Station ID (optional)"
        Me.fld3StationID.Name = "fld3StationID"
        '
        'fld4StationInfo
        '
        Me.fld4StationInfo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.fld4StationInfo.HeaderText = "Station Info (optional)"
        Me.fld4StationInfo.Name = "fld4StationInfo"
        Me.fld4StationInfo.Width = 121
        '
        'btnOK
        '
        Me.btnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnOK.Location = New System.Drawing.Point(416, 299)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(75, 23)
        Me.btnOK.TabIndex = 2
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(497, 299)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 3
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'frmStations
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(584, 334)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.dgvStationEntries)
        Me.Controls.Add(Me.txtHeader)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmStations"
        Me.Text = "USGS Baseflow Separation Stations"
        CType(Me.dgvStationEntries, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtHeader As System.Windows.Forms.TextBox
    Friend WithEvents dgvStationEntries As System.Windows.Forms.DataGridView
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents fld1RDBFilename As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents fld2DrainageArea As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents fld3StationID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents fld4StationInfo As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
