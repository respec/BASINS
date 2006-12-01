Friend Class frmAbout
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
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents picMapWindow As System.Windows.Forms.PictureBox
    Friend WithEvents lblProjFile As System.Windows.Forms.Label
    Friend WithEvents lblConfigFile As System.Windows.Forms.Label
    Friend WithEvents lblMapWindowURL As System.Windows.Forms.LinkLabel
    Friend WithEvents lblMapwinVersion As System.Windows.Forms.Label
    Friend WithEvents lblProjFileLabel As System.Windows.Forms.Label
    Friend WithEvents lblConfigFileLabel As System.Windows.Forms.Label
    Friend WithEvents lblBASINS As System.Windows.Forms.Label
    Friend WithEvents lblBasinsVersion As System.Windows.Forms.Label
    Friend WithEvents lblBasinsURL As System.Windows.Forms.LinkLabel
    Friend WithEvents picBASINS As System.Windows.Forms.PictureBox
    Friend WithEvents lblAquaTerraURL As System.Windows.Forms.LinkLabel
    Friend WithEvents picATC As System.Windows.Forms.PictureBox
    Friend WithEvents picZedGraph As System.Windows.Forms.PictureBox
    Friend WithEvents lblZedGraphURL As System.Windows.Forms.LinkLabel
    Friend WithEvents grpMapWindow As System.Windows.Forms.GroupBox
    Friend WithEvents grpATC As System.Windows.Forms.GroupBox
    Friend WithEvents grpZedGraph As System.Windows.Forms.GroupBox
    Friend WithEvents lblDisclaimer As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAbout))
        Me.grpMapWindow = New System.Windows.Forms.GroupBox
        Me.lblMapwinVersion = New System.Windows.Forms.Label
        Me.picMapWindow = New System.Windows.Forms.PictureBox
        Me.lblMapWindowURL = New System.Windows.Forms.LinkLabel
        Me.btnOk = New System.Windows.Forms.Button
        Me.lblProjFileLabel = New System.Windows.Forms.Label
        Me.lblConfigFileLabel = New System.Windows.Forms.Label
        Me.lblProjFile = New System.Windows.Forms.Label
        Me.lblConfigFile = New System.Windows.Forms.Label
        Me.picBASINS = New System.Windows.Forms.PictureBox
        Me.lblBASINS = New System.Windows.Forms.Label
        Me.lblBasinsVersion = New System.Windows.Forms.Label
        Me.lblBasinsURL = New System.Windows.Forms.LinkLabel
        Me.grpATC = New System.Windows.Forms.GroupBox
        Me.picATC = New System.Windows.Forms.PictureBox
        Me.lblAquaTerraURL = New System.Windows.Forms.LinkLabel
        Me.grpZedGraph = New System.Windows.Forms.GroupBox
        Me.picZedGraph = New System.Windows.Forms.PictureBox
        Me.lblZedGraphURL = New System.Windows.Forms.LinkLabel
        Me.lblDisclaimer = New System.Windows.Forms.Label
        Me.grpMapWindow.SuspendLayout()
        CType(Me.picMapWindow, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picBASINS, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpATC.SuspendLayout()
        CType(Me.picATC, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpZedGraph.SuspendLayout()
        CType(Me.picZedGraph, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'grpMapWindow
        '
        Me.grpMapWindow.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpMapWindow.Controls.Add(Me.lblMapwinVersion)
        Me.grpMapWindow.Controls.Add(Me.picMapWindow)
        Me.grpMapWindow.Controls.Add(Me.lblMapWindowURL)
        Me.grpMapWindow.Location = New System.Drawing.Point(19, 368)
        Me.grpMapWindow.Name = "grpMapWindow"
        Me.grpMapWindow.Size = New System.Drawing.Size(733, 102)
        Me.grpMapWindow.TabIndex = 7
        Me.grpMapWindow.TabStop = False
        Me.grpMapWindow.Text = "Powered by MapWindow Programmable Geographic Information System"
        '
        'lblMapwinVersion
        '
        Me.lblMapwinVersion.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblMapwinVersion.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lblMapwinVersion.Location = New System.Drawing.Point(532, 28)
        Me.lblMapwinVersion.Name = "lblMapwinVersion"
        Me.lblMapwinVersion.Size = New System.Drawing.Size(177, 18)
        Me.lblMapwinVersion.TabIndex = 12
        Me.lblMapwinVersion.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'picMapWindow
        '
        Me.picMapWindow.Cursor = System.Windows.Forms.Cursors.Hand
        Me.picMapWindow.Image = CType(resources.GetObject("picMapWindow.Image"), System.Drawing.Image)
        Me.picMapWindow.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.picMapWindow.Location = New System.Drawing.Point(11, 28)
        Me.picMapWindow.Name = "picMapWindow"
        Me.picMapWindow.Size = New System.Drawing.Size(245, 62)
        Me.picMapWindow.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.picMapWindow.TabIndex = 10
        Me.picMapWindow.TabStop = False
        '
        'lblMapWindowURL
        '
        Me.lblMapWindowURL.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblMapWindowURL.AutoSize = True
        Me.lblMapWindowURL.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lblMapWindowURL.LinkArea = New System.Windows.Forms.LinkArea(0, 25)
        Me.lblMapWindowURL.Location = New System.Drawing.Point(541, 73)
        Me.lblMapWindowURL.Name = "lblMapWindowURL"
        Me.lblMapWindowURL.Size = New System.Drawing.Size(180, 17)
        Me.lblMapWindowURL.TabIndex = 13
        Me.lblMapWindowURL.TabStop = True
        Me.lblMapWindowURL.Text = "http://www.MapWindow.org/"
        '
        'btnOk
        '
        Me.btnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOk.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnOk.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.btnOk.Location = New System.Drawing.Point(654, 639)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(86, 27)
        Me.btnOk.TabIndex = 8
        Me.btnOk.Text = "Close"
        '
        'lblProjFileLabel
        '
        Me.lblProjFileLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblProjFileLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lblProjFileLabel.Location = New System.Drawing.Point(29, 580)
        Me.lblProjFileLabel.Name = "lblProjFileLabel"
        Me.lblProjFileLabel.Size = New System.Drawing.Size(159, 18)
        Me.lblProjFileLabel.TabIndex = 11
        Me.lblProjFileLabel.Text = "Current Project File: "
        '
        'lblConfigFileLabel
        '
        Me.lblConfigFileLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblConfigFileLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lblConfigFileLabel.Location = New System.Drawing.Point(29, 608)
        Me.lblConfigFileLabel.Name = "lblConfigFileLabel"
        Me.lblConfigFileLabel.Size = New System.Drawing.Size(149, 17)
        Me.lblConfigFileLabel.TabIndex = 15
        Me.lblConfigFileLabel.Text = "Current Config File:"
        '
        'lblProjFile
        '
        Me.lblProjFile.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblProjFile.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblProjFile.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lblProjFile.Location = New System.Drawing.Point(154, 580)
        Me.lblProjFile.Name = "lblProjFile"
        Me.lblProjFile.Size = New System.Drawing.Size(586, 19)
        Me.lblProjFile.TabIndex = 16
        '
        'lblConfigFile
        '
        Me.lblConfigFile.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblConfigFile.BackColor = System.Drawing.SystemColors.Control
        Me.lblConfigFile.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblConfigFile.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lblConfigFile.Location = New System.Drawing.Point(154, 608)
        Me.lblConfigFile.Name = "lblConfigFile"
        Me.lblConfigFile.Size = New System.Drawing.Size(586, 18)
        Me.lblConfigFile.TabIndex = 17
        '
        'picBASINS
        '
        Me.picBASINS.Cursor = System.Windows.Forms.Cursors.Hand
        Me.picBASINS.Image = CType(resources.GetObject("picBASINS.Image"), System.Drawing.Image)
        Me.picBASINS.Location = New System.Drawing.Point(10, 9)
        Me.picBASINS.Name = "picBASINS"
        Me.picBASINS.Size = New System.Drawing.Size(163, 148)
        Me.picBASINS.TabIndex = 18
        Me.picBASINS.TabStop = False
        '
        'lblBASINS
        '
        Me.lblBASINS.Font = New System.Drawing.Font("Microsoft Sans Serif", 27.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBASINS.Location = New System.Drawing.Point(182, 18)
        Me.lblBASINS.Name = "lblBASINS"
        Me.lblBASINS.Size = New System.Drawing.Size(221, 56)
        Me.lblBASINS.TabIndex = 19
        Me.lblBASINS.Text = "BASINS"
        '
        'lblBasinsVersion
        '
        Me.lblBasinsVersion.Location = New System.Drawing.Point(192, 74)
        Me.lblBasinsVersion.Name = "lblBasinsVersion"
        Me.lblBasinsVersion.Size = New System.Drawing.Size(211, 18)
        Me.lblBasinsVersion.TabIndex = 20
        Me.lblBasinsVersion.Text = "Version 4.0"
        '
        'lblBasinsURL
        '
        Me.lblBasinsURL.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblBasinsURL.AutoSize = True
        Me.lblBasinsURL.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lblBasinsURL.LinkArea = New System.Windows.Forms.LinkArea(0, 39)
        Me.lblBasinsURL.Location = New System.Drawing.Point(471, 120)
        Me.lblBasinsURL.Name = "lblBasinsURL"
        Me.lblBasinsURL.Size = New System.Drawing.Size(269, 17)
        Me.lblBasinsURL.TabIndex = 21
        Me.lblBasinsURL.TabStop = True
        Me.lblBasinsURL.Text = "http://www.epa.gov/waterscience/BASINS/"
        '
        'grpATC
        '
        Me.grpATC.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpATC.Controls.Add(Me.picATC)
        Me.grpATC.Controls.Add(Me.lblAquaTerraURL)
        Me.grpATC.Location = New System.Drawing.Point(19, 174)
        Me.grpATC.Name = "grpATC"
        Me.grpATC.Size = New System.Drawing.Size(733, 185)
        Me.grpATC.TabIndex = 27
        Me.grpATC.TabStop = False
        Me.grpATC.Text = "Developed by AQUA TERRA Consultants"
        '
        'picATC
        '
        Me.picATC.Cursor = System.Windows.Forms.Cursors.Hand
        Me.picATC.Image = CType(resources.GetObject("picATC.Image"), System.Drawing.Image)
        Me.picATC.Location = New System.Drawing.Point(10, 18)
        Me.picATC.Name = "picATC"
        Me.picATC.Size = New System.Drawing.Size(489, 157)
        Me.picATC.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.picATC.TabIndex = 25
        Me.picATC.TabStop = False
        '
        'lblAquaTerraURL
        '
        Me.lblAquaTerraURL.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblAquaTerraURL.AutoSize = True
        Me.lblAquaTerraURL.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lblAquaTerraURL.LinkArea = New System.Windows.Forms.LinkArea(0, 25)
        Me.lblAquaTerraURL.Location = New System.Drawing.Point(550, 158)
        Me.lblAquaTerraURL.Name = "lblAquaTerraURL"
        Me.lblAquaTerraURL.Size = New System.Drawing.Size(171, 17)
        Me.lblAquaTerraURL.TabIndex = 26
        Me.lblAquaTerraURL.TabStop = True
        Me.lblAquaTerraURL.Text = "http://www.aquaterra.com/"
        '
        'grpZedGraph
        '
        Me.grpZedGraph.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpZedGraph.Controls.Add(Me.picZedGraph)
        Me.grpZedGraph.Controls.Add(Me.lblZedGraphURL)
        Me.grpZedGraph.Location = New System.Drawing.Point(19, 479)
        Me.grpZedGraph.Name = "grpZedGraph"
        Me.grpZedGraph.Size = New System.Drawing.Size(733, 83)
        Me.grpZedGraph.TabIndex = 28
        Me.grpZedGraph.TabStop = False
        Me.grpZedGraph.Text = "Graphing by ZedGraph"
        '
        'picZedGraph
        '
        Me.picZedGraph.Cursor = System.Windows.Forms.Cursors.Hand
        Me.picZedGraph.Image = CType(resources.GetObject("picZedGraph.Image"), System.Drawing.Image)
        Me.picZedGraph.Location = New System.Drawing.Point(19, 18)
        Me.picZedGraph.Name = "picZedGraph"
        Me.picZedGraph.Size = New System.Drawing.Size(211, 56)
        Me.picZedGraph.TabIndex = 28
        Me.picZedGraph.TabStop = False
        '
        'lblZedGraphURL
        '
        Me.lblZedGraphURL.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblZedGraphURL.LinkArea = New System.Windows.Forms.LinkArea(0, 20)
        Me.lblZedGraphURL.Location = New System.Drawing.Point(586, 55)
        Me.lblZedGraphURL.Name = "lblZedGraphURL"
        Me.lblZedGraphURL.Size = New System.Drawing.Size(135, 19)
        Me.lblZedGraphURL.TabIndex = 27
        Me.lblZedGraphURL.TabStop = True
        Me.lblZedGraphURL.Text = "http://zedgraph.org/"
        '
        'lblDisclaimer
        '
        Me.lblDisclaimer.Location = New System.Drawing.Point(192, 102)
        Me.lblDisclaimer.Name = "lblDisclaimer"
        Me.lblDisclaimer.Size = New System.Drawing.Size(192, 35)
        Me.lblDisclaimer.TabIndex = 29
        Me.lblDisclaimer.Text = "Pre-Release Version For Testing Only"
        '
        'frmAbout
        '
        Me.AcceptButton = Me.btnOk
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
        Me.CancelButton = Me.btnOk
        Me.ClientSize = New System.Drawing.Size(761, 678)
        Me.Controls.Add(Me.picBASINS)
        Me.Controls.Add(Me.lblDisclaimer)
        Me.Controls.Add(Me.lblBasinsVersion)
        Me.Controls.Add(Me.lblBASINS)
        Me.Controls.Add(Me.lblConfigFile)
        Me.Controls.Add(Me.lblProjFile)
        Me.Controls.Add(Me.lblConfigFileLabel)
        Me.Controls.Add(Me.lblProjFileLabel)
        Me.Controls.Add(Me.btnOk)
        Me.Controls.Add(Me.grpMapWindow)
        Me.Controls.Add(Me.grpATC)
        Me.Controls.Add(Me.grpZedGraph)
        Me.Controls.Add(Me.lblBasinsURL)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmAbout"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "About"
        Me.grpMapWindow.ResumeLayout(False)
        Me.grpMapWindow.PerformLayout()
        CType(Me.picMapWindow, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picBASINS, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpATC.ResumeLayout(False)
        Me.grpATC.PerformLayout()
        CType(Me.picATC, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpZedGraph.ResumeLayout(False)
        CType(Me.picZedGraph, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Public Sub ShowAbout()
        Try
            Me.Icon = g_MapWin.ApplicationInfo.FormIcon
            lblBasinsVersion.Text = "Version Dated:  " & IO.File.GetLastWriteTime(Me.GetType().Assembly.Location).ToShortDateString
            lblMapwinVersion.Text = "Version:  " & System.Reflection.Assembly.GetEntryAssembly.GetName.Version.ToString
            lblProjFile.Text = g_MapWin.Project.FileName
            lblConfigFile.Text = g_MapWin.Project.ConfigFileName
            Me.Show()
        Catch ex As System.Exception
            g_MapWin.ShowErrorDialog(ex)
        End Try
    End Sub

    Private Sub OpenLinkURL(ByVal aLink As Windows.Forms.LinkLabel)
        Try
            Diagnostics.Process.Start(aLink.Text.Substring(aLink.LinkArea.Start, aLink.LinkArea.Length))
        Catch ex As System.Exception
            g_MapWin.ShowErrorDialog(ex)
        End Try
    End Sub

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
        Handles btnOk.Click
        Me.Hide()
    End Sub

    Private Sub lblMapWindowURL_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) _
        Handles lblMapWindowURL.LinkClicked
        OpenLinkURL(lblMapWindowURL)
    End Sub

    Private Sub picMapWindow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
        Handles picMapWindow.Click
        OpenLinkURL(lblMapWindowURL)
    End Sub

    Private Sub picATC_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
        Handles picATC.Click
        OpenLinkURL(lblAquaTerraURL)
    End Sub

    Private Sub lblAquaTerraURL_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) _
        Handles lblAquaTerraURL.LinkClicked
        OpenLinkURL(lblAquaTerraURL)
    End Sub

    Private Sub lblBasinsURL_LinkClicked(ByVal sender As Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) _
        Handles lblBasinsURL.LinkClicked
        OpenLinkURL(lblBasinsURL)
    End Sub

    Private Sub picBASINS_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
        Handles picBASINS.Click
        OpenLinkURL(lblBasinsURL)
    End Sub

    Private Sub lblZedGraphURL_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) _
        Handles lblZedGraphURL.LinkClicked
        OpenLinkURL(lblZedGraphURL)
    End Sub

    Private Sub picZedGraph_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
        Handles picZedGraph.Click
        OpenLinkURL(lblZedGraphURL)
    End Sub

    Private Sub frmAbout_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) _
        Handles MyBase.KeyDown
        If e.KeyCode = Windows.Forms.Keys.F1 Then
            atcUtility.ShowHelp("")
        End If
    End Sub

End Class
