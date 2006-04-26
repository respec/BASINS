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
  Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
  Friend WithEvents btnOk As System.Windows.Forms.Button
  Friend WithEvents picMapWindow As System.Windows.Forms.PictureBox
  Friend WithEvents lblProjFile As System.Windows.Forms.Label
  Friend WithEvents lblConfigFile As System.Windows.Forms.Label
  Friend WithEvents lblMapWindowURL As System.Windows.Forms.LinkLabel
  Friend WithEvents lblMapWindow As System.Windows.Forms.Label
  Friend WithEvents lblMapwinVersion As System.Windows.Forms.Label
  Friend WithEvents lblProjFileLabel As System.Windows.Forms.Label
  Friend WithEvents lblConfigFileLabel As System.Windows.Forms.Label
  Friend WithEvents lblBASINS As System.Windows.Forms.Label
  Friend WithEvents lblBasinsVersion As System.Windows.Forms.Label
  Friend WithEvents lblBasinsURL As System.Windows.Forms.LinkLabel
  Friend WithEvents picBASINS As System.Windows.Forms.PictureBox
  Friend WithEvents picATC As System.Windows.Forms.PictureBox
  Friend WithEvents lblAquaTerraURL As System.Windows.Forms.LinkLabel
  Friend WithEvents lblATC As System.Windows.Forms.Label
  Friend WithEvents lblZedGraphURL As System.Windows.Forms.LinkLabel
  Friend WithEvents picZedGraph As System.Windows.Forms.PictureBox
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
    Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmAbout))
    Me.GroupBox1 = New System.Windows.Forms.GroupBox
    Me.lblMapWindow = New System.Windows.Forms.Label
    Me.lblMapWindowURL = New System.Windows.Forms.LinkLabel
    Me.lblMapwinVersion = New System.Windows.Forms.Label
    Me.picMapWindow = New System.Windows.Forms.PictureBox
    Me.btnOk = New System.Windows.Forms.Button
    Me.lblProjFileLabel = New System.Windows.Forms.Label
    Me.lblConfigFileLabel = New System.Windows.Forms.Label
    Me.lblProjFile = New System.Windows.Forms.Label
    Me.lblConfigFile = New System.Windows.Forms.Label
    Me.picBASINS = New System.Windows.Forms.PictureBox
    Me.lblBASINS = New System.Windows.Forms.Label
    Me.lblBasinsVersion = New System.Windows.Forms.Label
    Me.lblBasinsURL = New System.Windows.Forms.LinkLabel
    Me.picATC = New System.Windows.Forms.PictureBox
    Me.lblAquaTerraURL = New System.Windows.Forms.LinkLabel
    Me.lblATC = New System.Windows.Forms.Label
    Me.lblZedGraphURL = New System.Windows.Forms.LinkLabel
    Me.picZedGraph = New System.Windows.Forms.PictureBox
    Me.GroupBox1.SuspendLayout()
    Me.SuspendLayout()
    '
    'GroupBox1
    '
    Me.GroupBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.GroupBox1.Controls.Add(Me.lblMapWindow)
    Me.GroupBox1.Controls.Add(Me.lblMapWindowURL)
    Me.GroupBox1.Controls.Add(Me.lblMapwinVersion)
    Me.GroupBox1.Controls.Add(Me.picMapWindow)
    Me.GroupBox1.Location = New System.Drawing.Point(16, 304)
    Me.GroupBox1.Name = "GroupBox1"
    Me.GroupBox1.Size = New System.Drawing.Size(584, 96)
    Me.GroupBox1.TabIndex = 7
    Me.GroupBox1.TabStop = False
    Me.GroupBox1.Text = "Powered by MapWindow"
    '
    'lblMapWindow
    '
    Me.lblMapWindow.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lblMapWindow.AutoSize = True
    Me.lblMapWindow.ImeMode = System.Windows.Forms.ImeMode.NoControl
    Me.lblMapWindow.Location = New System.Drawing.Point(262, 11)
    Me.lblMapWindow.Name = "lblMapWindow"
    Me.lblMapWindow.Size = New System.Drawing.Size(306, 16)
    Me.lblMapWindow.TabIndex = 1
    Me.lblMapWindow.Text = "MapWindow Programmable Geographic Information System"
    Me.lblMapWindow.TextAlign = System.Drawing.ContentAlignment.MiddleRight
    '
    'lblMapWindowURL
    '
    Me.lblMapWindowURL.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lblMapWindowURL.AutoSize = True
    Me.lblMapWindowURL.ImeMode = System.Windows.Forms.ImeMode.NoControl
    Me.lblMapWindowURL.LinkArea = New System.Windows.Forms.LinkArea(0, 24)
    Me.lblMapWindowURL.Location = New System.Drawing.Point(421, 67)
    Me.lblMapWindowURL.Name = "lblMapWindowURL"
    Me.lblMapWindowURL.Size = New System.Drawing.Size(142, 16)
    Me.lblMapWindowURL.TabIndex = 13
    Me.lblMapWindowURL.TabStop = True
    Me.lblMapWindowURL.Text = "http://www.MapWindow.org"
    '
    'lblMapwinVersion
    '
    Me.lblMapwinVersion.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lblMapwinVersion.ImeMode = System.Windows.Forms.ImeMode.NoControl
    Me.lblMapwinVersion.Location = New System.Drawing.Point(329, 39)
    Me.lblMapwinVersion.Name = "lblMapwinVersion"
    Me.lblMapwinVersion.Size = New System.Drawing.Size(239, 16)
    Me.lblMapwinVersion.TabIndex = 12
    Me.lblMapwinVersion.TextAlign = System.Drawing.ContentAlignment.MiddleRight
    '
    'picMapWindow
    '
    Me.picMapWindow.Cursor = System.Windows.Forms.Cursors.Hand
    Me.picMapWindow.Image = CType(resources.GetObject("picMapWindow.Image"), System.Drawing.Image)
    Me.picMapWindow.ImeMode = System.Windows.Forms.ImeMode.NoControl
    Me.picMapWindow.Location = New System.Drawing.Point(9, 32)
    Me.picMapWindow.Name = "picMapWindow"
    Me.picMapWindow.Size = New System.Drawing.Size(204, 54)
    Me.picMapWindow.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
    Me.picMapWindow.TabIndex = 10
    Me.picMapWindow.TabStop = False
    '
    'btnOk
    '
    Me.btnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.btnOk.ImeMode = System.Windows.Forms.ImeMode.NoControl
    Me.btnOk.Location = New System.Drawing.Point(520, 536)
    Me.btnOk.Name = "btnOk"
    Me.btnOk.Size = New System.Drawing.Size(72, 24)
    Me.btnOk.TabIndex = 8
    Me.btnOk.Text = "Close"
    '
    'lblProjFileLabel
    '
    Me.lblProjFileLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
    Me.lblProjFileLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl
    Me.lblProjFileLabel.Location = New System.Drawing.Point(24, 488)
    Me.lblProjFileLabel.Name = "lblProjFileLabel"
    Me.lblProjFileLabel.Size = New System.Drawing.Size(133, 15)
    Me.lblProjFileLabel.TabIndex = 11
    Me.lblProjFileLabel.Text = "Current Project File: "
    '
    'lblConfigFileLabel
    '
    Me.lblConfigFileLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
    Me.lblConfigFileLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl
    Me.lblConfigFileLabel.Location = New System.Drawing.Point(24, 512)
    Me.lblConfigFileLabel.Name = "lblConfigFileLabel"
    Me.lblConfigFileLabel.Size = New System.Drawing.Size(124, 15)
    Me.lblConfigFileLabel.TabIndex = 15
    Me.lblConfigFileLabel.Text = "Current Config File:"
    '
    'lblProjFile
    '
    Me.lblProjFile.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lblProjFile.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
    Me.lblProjFile.ImeMode = System.Windows.Forms.ImeMode.NoControl
    Me.lblProjFile.Location = New System.Drawing.Point(128, 488)
    Me.lblProjFile.Name = "lblProjFile"
    Me.lblProjFile.Size = New System.Drawing.Size(462, 16)
    Me.lblProjFile.TabIndex = 16
    '
    'lblConfigFile
    '
    Me.lblConfigFile.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lblConfigFile.BackColor = System.Drawing.SystemColors.Control
    Me.lblConfigFile.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
    Me.lblConfigFile.ImeMode = System.Windows.Forms.ImeMode.NoControl
    Me.lblConfigFile.Location = New System.Drawing.Point(128, 512)
    Me.lblConfigFile.Name = "lblConfigFile"
    Me.lblConfigFile.Size = New System.Drawing.Size(462, 16)
    Me.lblConfigFile.TabIndex = 17
    '
    'picBASINS
    '
    Me.picBASINS.Cursor = System.Windows.Forms.Cursors.Hand
    Me.picBASINS.Image = CType(resources.GetObject("picBASINS.Image"), System.Drawing.Image)
    Me.picBASINS.Location = New System.Drawing.Point(8, 8)
    Me.picBASINS.Name = "picBASINS"
    Me.picBASINS.Size = New System.Drawing.Size(136, 128)
    Me.picBASINS.TabIndex = 18
    Me.picBASINS.TabStop = False
    '
    'lblBASINS
    '
    Me.lblBASINS.Font = New System.Drawing.Font("Microsoft Sans Serif", 27.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblBASINS.Location = New System.Drawing.Point(152, 16)
    Me.lblBASINS.Name = "lblBASINS"
    Me.lblBASINS.Size = New System.Drawing.Size(184, 48)
    Me.lblBASINS.TabIndex = 19
    Me.lblBASINS.Text = "BASINS"
    '
    'lblBasinsVersion
    '
    Me.lblBasinsVersion.Location = New System.Drawing.Point(160, 64)
    Me.lblBasinsVersion.Name = "lblBasinsVersion"
    Me.lblBasinsVersion.Size = New System.Drawing.Size(176, 23)
    Me.lblBasinsVersion.TabIndex = 20
    Me.lblBasinsVersion.Text = "Version 4.0 Beta "
    '
    'lblBasinsURL
    '
    Me.lblBasinsURL.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lblBasinsURL.AutoSize = True
    Me.lblBasinsURL.ImeMode = System.Windows.Forms.ImeMode.NoControl
    Me.lblBasinsURL.LinkArea = New System.Windows.Forms.LinkArea(0, 39)
    Me.lblBasinsURL.Location = New System.Drawing.Point(360, 96)
    Me.lblBasinsURL.Name = "lblBasinsURL"
    Me.lblBasinsURL.Size = New System.Drawing.Size(218, 16)
    Me.lblBasinsURL.TabIndex = 21
    Me.lblBasinsURL.TabStop = True
    Me.lblBasinsURL.Text = "http://www.epa.gov/waterscience/BASINS/"
    '
    'picATC
    '
    Me.picATC.Cursor = System.Windows.Forms.Cursors.Hand
    Me.picATC.Image = CType(resources.GetObject("picATC.Image"), System.Drawing.Image)
    Me.picATC.Location = New System.Drawing.Point(8, 152)
    Me.picATC.Name = "picATC"
    Me.picATC.Size = New System.Drawing.Size(408, 136)
    Me.picATC.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
    Me.picATC.TabIndex = 22
    Me.picATC.TabStop = False
    '
    'lblAquaTerraURL
    '
    Me.lblAquaTerraURL.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lblAquaTerraURL.AutoSize = True
    Me.lblAquaTerraURL.ImeMode = System.Windows.Forms.ImeMode.NoControl
    Me.lblAquaTerraURL.LinkArea = New System.Windows.Forms.LinkArea(0, 25)
    Me.lblAquaTerraURL.Location = New System.Drawing.Point(440, 264)
    Me.lblAquaTerraURL.Name = "lblAquaTerraURL"
    Me.lblAquaTerraURL.Size = New System.Drawing.Size(136, 16)
    Me.lblAquaTerraURL.TabIndex = 23
    Me.lblAquaTerraURL.TabStop = True
    Me.lblAquaTerraURL.Text = "http://www.aquaterra.com/"
    '
    'lblATC
    '
    Me.lblATC.Location = New System.Drawing.Point(424, 160)
    Me.lblATC.Name = "lblATC"
    Me.lblATC.Size = New System.Drawing.Size(176, 23)
    Me.lblATC.TabIndex = 24
    '
    'lblZedGraphURL
    '
    Me.lblZedGraphURL.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lblZedGraphURL.LinkArea = New System.Windows.Forms.LinkArea(23, 20)
    Me.lblZedGraphURL.Location = New System.Drawing.Point(320, 432)
    Me.lblZedGraphURL.Name = "lblZedGraphURL"
    Me.lblZedGraphURL.Size = New System.Drawing.Size(248, 16)
    Me.lblZedGraphURL.TabIndex = 25
    Me.lblZedGraphURL.TabStop = True
    Me.lblZedGraphURL.Text = "Graphing by ZedGraph   http://zedgraph.org/"
    '
    'picZedGraph
    '
    Me.picZedGraph.Cursor = System.Windows.Forms.Cursors.Hand
    Me.picZedGraph.Image = CType(resources.GetObject("picZedGraph.Image"), System.Drawing.Image)
    Me.picZedGraph.Location = New System.Drawing.Point(24, 416)
    Me.picZedGraph.Name = "picZedGraph"
    Me.picZedGraph.Size = New System.Drawing.Size(176, 48)
    Me.picZedGraph.TabIndex = 26
    Me.picZedGraph.TabStop = False
    '
    'frmAbout
    '
    Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
    Me.ClientSize = New System.Drawing.Size(608, 573)
    Me.Controls.Add(Me.picZedGraph)
    Me.Controls.Add(Me.lblZedGraphURL)
    Me.Controls.Add(Me.lblATC)
    Me.Controls.Add(Me.lblAquaTerraURL)
    Me.Controls.Add(Me.picATC)
    Me.Controls.Add(Me.lblBasinsURL)
    Me.Controls.Add(Me.lblBasinsVersion)
    Me.Controls.Add(Me.lblBASINS)
    Me.Controls.Add(Me.picBASINS)
    Me.Controls.Add(Me.lblConfigFile)
    Me.Controls.Add(Me.lblProjFile)
    Me.Controls.Add(Me.lblConfigFileLabel)
    Me.Controls.Add(Me.lblProjFileLabel)
    Me.Controls.Add(Me.btnOk)
    Me.Controls.Add(Me.GroupBox1)
    Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
    Me.KeyPreview = True
    Me.MaximizeBox = False
    Me.MinimizeBox = False
    Me.Name = "frmAbout"
    Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
    Me.Text = "About"
    Me.GroupBox1.ResumeLayout(False)
    Me.ResumeLayout(False)

  End Sub

#End Region

  Public Sub ShowAbout()
    Try
      'Me.Icon = g_MapWin.ApplicationInfo.FormIcon
      'TODO: Get MapWindow version for label
      'lblMapwinVersion.Text = "Version:  " & g_MapWin.ApplicationInfo.VersionString
      lblProjFile.Text = g_MapWin.Project.FileName
      lblConfigFile.Text = g_MapWin.Project.ConfigFileName

      Me.Show()
    Catch ex As System.Exception
      g_MapWin.ShowErrorDialog(ex)
    End Try
  End Sub

  Private Sub OpenLinkURL(ByVal aLink as Windows.Forms.LinkLabel)
    Try      
      Diagnostics.Process.Start(aLink.Text.Substring(aLink.LinkArea.Start, aLink.LinkArea.Length))
    Catch ex As System.Exception
      g_MapWin.ShowErrorDialog(ex)
    End Try
  End Sub

  Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
    Me.Hide()
  End Sub

  Private Sub lblMapWindowURL_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lblMapWindowURL.LinkClicked
    OpenLinkURL(lblMapWindowURL)
  End Sub

  Private Sub picMapWindow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picMapWindow.Click
    OpenLinkURL(lblMapWindowURL)
  End Sub

  Private Sub picATC_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picATC.Click
    OpenLinkURL(lblAquaTerraURL)
  End Sub

  Private Sub lblAquaTerraURL_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lblAquaTerraURL.LinkClicked
    OpenLinkURL(lblAquaTerraURL)
  End Sub

  Private Sub lblBasinsURL_LinkClicked(ByVal sender As Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lblBasinsURL.LinkClicked
    OpenLinkURL(lblBasinsURL)
  End Sub

  Private Sub picBASINS_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picBASINS.Click
    OpenLinkURL(lblBasinsURL)
  End Sub

  Private Sub lblZedGraphURL_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lblZedGraphURL.LinkClicked
    OpenLinkURL(lblZedGraphURL)
  End Sub

  Private Sub picZedGraph_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picZedGraph.Click
    OpenLinkURL(lblZedGraphURL)
  End Sub

  Private Sub frmAbout_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
    If e.KeyCode = Windows.Forms.Keys.F1 Then atcUtility.ShowHelp("")
  End Sub
End Class
