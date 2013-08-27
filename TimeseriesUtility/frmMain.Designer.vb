<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing Then
                RemoveHandler atcData.atcDataManager.OpenedData, AddressOf FileOpenedOrClosed
                RemoveHandler atcData.atcDataManager.ClosedData, AddressOf FileOpenedOrClosed
                If components IsNot Nothing Then
                    components.Dispose()
                End If
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
        Me.lblFile = New System.Windows.Forms.Label()
        Me.btnManageFiles = New System.Windows.Forms.Button()
        Me.btnSelectData = New System.Windows.Forms.Button()
        Me.lblDatasets = New System.Windows.Forms.Label()
        Me.btnList = New System.Windows.Forms.Button()
        Me.btnGraph = New System.Windows.Forms.Button()
        Me.btnOpen = New System.Windows.Forms.Button()
        Me.btnSaveWDM = New System.Windows.Forms.Button()
        Me.btnImportWDM = New System.Windows.Forms.Button()
        Me.btnDump = New System.Windows.Forms.Button()
        Me.btnCompare = New System.Windows.Forms.Button()
        Me.btnTree = New System.Windows.Forms.Button()
        Me.btnSaveList = New System.Windows.Forms.Button()
        Me.grpView = New System.Windows.Forms.GroupBox()
        Me.grpSave = New System.Windows.Forms.GroupBox()
        Me.btnGenerateMet = New System.Windows.Forms.Button()
        Me.grpGenerate = New System.Windows.Forms.GroupBox()
        Me.btnGenerateMath = New System.Windows.Forms.Button()
        Me.btnHelp = New System.Windows.Forms.Button()
        Me.pictureLogo = New System.Windows.Forms.PictureBox()
        Me.grpView.SuspendLayout()
        Me.grpSave.SuspendLayout()
        Me.grpGenerate.SuspendLayout()
        CType(Me.pictureLogo, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblFile
        '
        Me.lblFile.AutoSize = True
        Me.lblFile.Location = New System.Drawing.Point(228, 17)
        Me.lblFile.Name = "lblFile"
        Me.lblFile.Size = New System.Drawing.Size(87, 13)
        Me.lblFile.TabIndex = 2
        Me.lblFile.Text = "No files are open"
        '
        'btnManageFiles
        '
        Me.btnManageFiles.Location = New System.Drawing.Point(120, 12)
        Me.btnManageFiles.Name = "btnManageFiles"
        Me.btnManageFiles.Size = New System.Drawing.Size(102, 23)
        Me.btnManageFiles.TabIndex = 1
        Me.btnManageFiles.Text = "Manage Files"
        Me.btnManageFiles.UseVisualStyleBackColor = True
        '
        'btnSelectData
        '
        Me.btnSelectData.Location = New System.Drawing.Point(12, 41)
        Me.btnSelectData.Name = "btnSelectData"
        Me.btnSelectData.Size = New System.Drawing.Size(102, 23)
        Me.btnSelectData.TabIndex = 3
        Me.btnSelectData.Text = "Select Timeseries"
        Me.btnSelectData.UseVisualStyleBackColor = True
        '
        'lblDatasets
        '
        Me.lblDatasets.AutoSize = True
        Me.lblDatasets.Location = New System.Drawing.Point(120, 46)
        Me.lblDatasets.Name = "lblDatasets"
        Me.lblDatasets.Size = New System.Drawing.Size(135, 13)
        Me.lblDatasets.TabIndex = 4
        Me.lblDatasets.Text = "No Timeseries are selected"
        '
        'btnList
        '
        Me.btnList.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnList.Location = New System.Drawing.Point(6, 18)
        Me.btnList.Name = "btnList"
        Me.btnList.Size = New System.Drawing.Size(75, 23)
        Me.btnList.TabIndex = 5
        Me.btnList.Text = "List"
        Me.btnList.UseVisualStyleBackColor = True
        '
        'btnGraph
        '
        Me.btnGraph.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnGraph.Location = New System.Drawing.Point(6, 47)
        Me.btnGraph.Name = "btnGraph"
        Me.btnGraph.Size = New System.Drawing.Size(75, 23)
        Me.btnGraph.TabIndex = 6
        Me.btnGraph.Text = "Graph"
        Me.btnGraph.UseVisualStyleBackColor = True
        '
        'btnOpen
        '
        Me.btnOpen.Location = New System.Drawing.Point(12, 12)
        Me.btnOpen.Name = "btnOpen"
        Me.btnOpen.Size = New System.Drawing.Size(102, 23)
        Me.btnOpen.TabIndex = 0
        Me.btnOpen.Text = "Open File"
        Me.btnOpen.UseVisualStyleBackColor = True
        '
        'btnSaveWDM
        '
        Me.btnSaveWDM.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnSaveWDM.Location = New System.Drawing.Point(6, 47)
        Me.btnSaveWDM.Name = "btnSaveWDM"
        Me.btnSaveWDM.Size = New System.Drawing.Size(126, 23)
        Me.btnSaveWDM.TabIndex = 7
        Me.btnSaveWDM.Text = "Save to WDM"
        Me.btnSaveWDM.UseVisualStyleBackColor = True
        '
        'btnImportWDM
        '
        Me.btnImportWDM.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnImportWDM.Location = New System.Drawing.Point(6, 76)
        Me.btnImportWDM.Name = "btnImportWDM"
        Me.btnImportWDM.Size = New System.Drawing.Size(126, 23)
        Me.btnImportWDM.TabIndex = 8
        Me.btnImportWDM.Text = "Import Text to WDM"
        Me.btnImportWDM.UseVisualStyleBackColor = True
        '
        'btnDump
        '
        Me.btnDump.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnDump.Location = New System.Drawing.Point(377, 104)
        Me.btnDump.Name = "btnDump"
        Me.btnDump.Size = New System.Drawing.Size(119, 23)
        Me.btnDump.TabIndex = 9
        Me.btnDump.Text = "Dump All To Text"
        Me.btnDump.UseVisualStyleBackColor = True
        Me.btnDump.Visible = False
        '
        'btnCompare
        '
        Me.btnCompare.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnCompare.Location = New System.Drawing.Point(377, 133)
        Me.btnCompare.Name = "btnCompare"
        Me.btnCompare.Size = New System.Drawing.Size(119, 23)
        Me.btnCompare.TabIndex = 10
        Me.btnCompare.Text = "Compare Text"
        Me.btnCompare.UseVisualStyleBackColor = True
        Me.btnCompare.Visible = False
        '
        'btnTree
        '
        Me.btnTree.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnTree.Location = New System.Drawing.Point(6, 76)
        Me.btnTree.Name = "btnTree"
        Me.btnTree.Size = New System.Drawing.Size(75, 23)
        Me.btnTree.TabIndex = 11
        Me.btnTree.Text = "Tree"
        Me.btnTree.UseVisualStyleBackColor = True
        '
        'btnSaveList
        '
        Me.btnSaveList.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnSaveList.Location = New System.Drawing.Point(6, 19)
        Me.btnSaveList.Name = "btnSaveList"
        Me.btnSaveList.Size = New System.Drawing.Size(126, 23)
        Me.btnSaveList.TabIndex = 12
        Me.btnSaveList.Text = "Save List As Text"
        Me.btnSaveList.UseVisualStyleBackColor = True
        '
        'grpView
        '
        Me.grpView.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.grpView.Controls.Add(Me.btnList)
        Me.grpView.Controls.Add(Me.btnGraph)
        Me.grpView.Controls.Add(Me.btnTree)
        Me.grpView.Location = New System.Drawing.Point(12, 86)
        Me.grpView.Name = "grpView"
        Me.grpView.Size = New System.Drawing.Size(86, 105)
        Me.grpView.TabIndex = 13
        Me.grpView.TabStop = False
        Me.grpView.Text = "View"
        '
        'grpSave
        '
        Me.grpSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.grpSave.Controls.Add(Me.btnSaveList)
        Me.grpSave.Controls.Add(Me.btnSaveWDM)
        Me.grpSave.Controls.Add(Me.btnImportWDM)
        Me.grpSave.Location = New System.Drawing.Point(104, 86)
        Me.grpSave.Name = "grpSave"
        Me.grpSave.Size = New System.Drawing.Size(138, 105)
        Me.grpSave.TabIndex = 14
        Me.grpSave.TabStop = False
        Me.grpSave.Text = "Save"
        '
        'btnGenerateMet
        '
        Me.btnGenerateMet.Location = New System.Drawing.Point(6, 19)
        Me.btnGenerateMet.Name = "btnGenerateMet"
        Me.btnGenerateMet.Size = New System.Drawing.Size(111, 23)
        Me.btnGenerateMet.TabIndex = 15
        Me.btnGenerateMet.Text = "Meteorologic"
        Me.btnGenerateMet.UseVisualStyleBackColor = True
        '
        'grpGenerate
        '
        Me.grpGenerate.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.grpGenerate.Controls.Add(Me.btnGenerateMath)
        Me.grpGenerate.Controls.Add(Me.btnGenerateMet)
        Me.grpGenerate.Location = New System.Drawing.Point(248, 86)
        Me.grpGenerate.Name = "grpGenerate"
        Me.grpGenerate.Size = New System.Drawing.Size(123, 79)
        Me.grpGenerate.TabIndex = 16
        Me.grpGenerate.TabStop = False
        Me.grpGenerate.Text = "Compute"
        '
        'btnGenerateMath
        '
        Me.btnGenerateMath.Location = New System.Drawing.Point(6, 48)
        Me.btnGenerateMath.Name = "btnGenerateMath"
        Me.btnGenerateMath.Size = New System.Drawing.Size(111, 23)
        Me.btnGenerateMath.TabIndex = 16
        Me.btnGenerateMath.Text = "Math"
        Me.btnGenerateMath.UseVisualStyleBackColor = True
        '
        'btnHelp
        '
        Me.btnHelp.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnHelp.Location = New System.Drawing.Point(424, 168)
        Me.btnHelp.Name = "btnHelp"
        Me.btnHelp.Size = New System.Drawing.Size(70, 23)
        Me.btnHelp.TabIndex = 17
        Me.btnHelp.Text = "Help"
        Me.btnHelp.UseVisualStyleBackColor = True
        '
        'pictureLogo
        '
        Me.pictureLogo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pictureLogo.Image = Global.TimeseriesUtility.My.Resources.Resources._177x121transparent
        Me.pictureLogo.Location = New System.Drawing.Point(375, 12)
        Me.pictureLogo.Name = "pictureLogo"
        Me.pictureLogo.Size = New System.Drawing.Size(119, 81)
        Me.pictureLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pictureLogo.TabIndex = 18
        Me.pictureLogo.TabStop = False
        '
        'frmMain
        '
        Me.AllowDrop = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(506, 203)
        Me.Controls.Add(Me.btnHelp)
        Me.Controls.Add(Me.grpGenerate)
        Me.Controls.Add(Me.grpSave)
        Me.Controls.Add(Me.grpView)
        Me.Controls.Add(Me.btnDump)
        Me.Controls.Add(Me.btnCompare)
        Me.Controls.Add(Me.btnOpen)
        Me.Controls.Add(Me.btnSelectData)
        Me.Controls.Add(Me.lblDatasets)
        Me.Controls.Add(Me.btnManageFiles)
        Me.Controls.Add(Me.lblFile)
        Me.Controls.Add(Me.pictureLogo)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Name = "frmMain"
        Me.Text = "SARA Timeseries Utility"
        Me.grpView.ResumeLayout(False)
        Me.grpSave.ResumeLayout(False)
        Me.grpGenerate.ResumeLayout(False)
        CType(Me.pictureLogo, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblFile As System.Windows.Forms.Label
    Friend WithEvents btnManageFiles As System.Windows.Forms.Button
    Friend WithEvents btnSelectData As System.Windows.Forms.Button
    Friend WithEvents lblDatasets As System.Windows.Forms.Label
    Friend WithEvents btnList As System.Windows.Forms.Button
    Friend WithEvents btnGraph As System.Windows.Forms.Button
    Friend WithEvents btnOpen As System.Windows.Forms.Button
    Friend WithEvents btnSaveWDM As System.Windows.Forms.Button
    Friend WithEvents btnImportWDM As System.Windows.Forms.Button
    Friend WithEvents btnDump As System.Windows.Forms.Button
    Friend WithEvents btnCompare As System.Windows.Forms.Button
    Friend WithEvents btnTree As System.Windows.Forms.Button
    Friend WithEvents btnSaveList As System.Windows.Forms.Button
    Friend WithEvents grpView As System.Windows.Forms.GroupBox
    Friend WithEvents grpSave As System.Windows.Forms.GroupBox
    Friend WithEvents btnGenerateMet As System.Windows.Forms.Button
    Friend WithEvents grpGenerate As System.Windows.Forms.GroupBox
    Friend WithEvents btnGenerateMath As System.Windows.Forms.Button
    Friend WithEvents btnHelp As System.Windows.Forms.Button
    Friend WithEvents pictureLogo As System.Windows.Forms.PictureBox

End Class
