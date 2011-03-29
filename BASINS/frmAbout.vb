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
    Friend WithEvents lblProgramName As System.Windows.Forms.Label
    Friend WithEvents lblPluginVersion As System.Windows.Forms.Label
    Friend WithEvents lblProgramURL As System.Windows.Forms.LinkLabel
    Friend WithEvents picProgramLogo As System.Windows.Forms.PictureBox
    Friend WithEvents lblAquaTerraURL As System.Windows.Forms.LinkLabel
    Friend WithEvents picATC As System.Windows.Forms.PictureBox
    Friend WithEvents picZedGraph As System.Windows.Forms.PictureBox
    Friend WithEvents lblZedGraphURL As System.Windows.Forms.LinkLabel
    Friend WithEvents grpMapWindow As System.Windows.Forms.GroupBox
    Friend WithEvents grpATC As System.Windows.Forms.GroupBox
    Friend WithEvents grpZedGraph As System.Windows.Forms.GroupBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAbout))
        Me.grpMapWindow = New System.Windows.Forms.GroupBox()
        Me.lblMapwinVersion = New System.Windows.Forms.Label()
        Me.picMapWindow = New System.Windows.Forms.PictureBox()
        Me.lblMapWindowURL = New System.Windows.Forms.LinkLabel()
        Me.btnOk = New System.Windows.Forms.Button()
        Me.lblProjFileLabel = New System.Windows.Forms.Label()
        Me.lblConfigFileLabel = New System.Windows.Forms.Label()
        Me.lblProjFile = New System.Windows.Forms.Label()
        Me.lblConfigFile = New System.Windows.Forms.Label()
        Me.picProgramLogo = New System.Windows.Forms.PictureBox()
        Me.lblProgramName = New System.Windows.Forms.Label()
        Me.lblPluginVersion = New System.Windows.Forms.Label()
        Me.lblProgramURL = New System.Windows.Forms.LinkLabel()
        Me.grpATC = New System.Windows.Forms.GroupBox()
        Me.picATC = New System.Windows.Forms.PictureBox()
        Me.lblAquaTerraURL = New System.Windows.Forms.LinkLabel()
        Me.grpZedGraph = New System.Windows.Forms.GroupBox()
        Me.picZedGraph = New System.Windows.Forms.PictureBox()
        Me.lblZedGraphURL = New System.Windows.Forms.LinkLabel()
        Me.grpMapWindow.SuspendLayout()
        CType(Me.picMapWindow, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picProgramLogo, System.ComponentModel.ISupportInitialize).BeginInit()
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
        Me.grpMapWindow.Location = New System.Drawing.Point(12, 308)
        Me.grpMapWindow.Name = "grpMapWindow"
        Me.grpMapWindow.Size = New System.Drawing.Size(588, 88)
        Me.grpMapWindow.TabIndex = 5
        Me.grpMapWindow.TabStop = False
        Me.grpMapWindow.Text = "Powered by MapWindow Programmable Geographic Information System"
        '
        'lblMapwinVersion
        '
        Me.lblMapwinVersion.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblMapwinVersion.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lblMapwinVersion.Location = New System.Drawing.Point(420, 24)
        Me.lblMapwinVersion.Name = "lblMapwinVersion"
        Me.lblMapwinVersion.Size = New System.Drawing.Size(148, 16)
        Me.lblMapwinVersion.TabIndex = 12
        Me.lblMapwinVersion.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'picMapWindow
        '
        Me.picMapWindow.Cursor = System.Windows.Forms.Cursors.Hand
        Me.picMapWindow.Image = CType(resources.GetObject("picMapWindow.Image"), System.Drawing.Image)
        Me.picMapWindow.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.picMapWindow.Location = New System.Drawing.Point(9, 24)
        Me.picMapWindow.Name = "picMapWindow"
        Me.picMapWindow.Size = New System.Drawing.Size(204, 54)
        Me.picMapWindow.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.picMapWindow.TabIndex = 10
        Me.picMapWindow.TabStop = False
        '
        'lblMapWindowURL
        '
        Me.lblMapWindowURL.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblMapWindowURL.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lblMapWindowURL.LinkArea = New System.Windows.Forms.LinkArea(0, 25)
        Me.lblMapWindowURL.Location = New System.Drawing.Point(387, 65)
        Me.lblMapWindowURL.Name = "lblMapWindowURL"
        Me.lblMapWindowURL.Size = New System.Drawing.Size(186, 13)
        Me.lblMapWindowURL.TabIndex = 6
        Me.lblMapWindowURL.TabStop = True
        Me.lblMapWindowURL.Text = "http://www.MapWindow.org/"
        Me.lblMapWindowURL.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'btnOk
        '
        Me.btnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOk.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnOk.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.btnOk.Location = New System.Drawing.Point(528, 545)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(72, 23)
        Me.btnOk.TabIndex = 1
        Me.btnOk.Text = "Close"
        '
        'lblProjFileLabel
        '
        Me.lblProjFileLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblProjFileLabel.AutoSize = True
        Me.lblProjFileLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lblProjFileLabel.Location = New System.Drawing.Point(24, 495)
        Me.lblProjFileLabel.Name = "lblProjFileLabel"
        Me.lblProjFileLabel.Size = New System.Drawing.Size(65, 13)
        Me.lblProjFileLabel.TabIndex = 11
        Me.lblProjFileLabel.Text = "Project File: "
        '
        'lblConfigFileLabel
        '
        Me.lblConfigFileLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblConfigFileLabel.AutoSize = True
        Me.lblConfigFileLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lblConfigFileLabel.Location = New System.Drawing.Point(24, 519)
        Me.lblConfigFileLabel.Name = "lblConfigFileLabel"
        Me.lblConfigFileLabel.Size = New System.Drawing.Size(59, 13)
        Me.lblConfigFileLabel.TabIndex = 15
        Me.lblConfigFileLabel.Text = "Config File:"
        '
        'lblProjFile
        '
        Me.lblProjFile.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblProjFile.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblProjFile.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lblProjFile.Location = New System.Drawing.Point(101, 495)
        Me.lblProjFile.Name = "lblProjFile"
        Me.lblProjFile.Size = New System.Drawing.Size(499, 16)
        Me.lblProjFile.TabIndex = 16
        '
        'lblConfigFile
        '
        Me.lblConfigFile.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblConfigFile.BackColor = System.Drawing.SystemColors.Control
        Me.lblConfigFile.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblConfigFile.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lblConfigFile.Location = New System.Drawing.Point(101, 519)
        Me.lblConfigFile.Name = "lblConfigFile"
        Me.lblConfigFile.Size = New System.Drawing.Size(499, 16)
        Me.lblConfigFile.TabIndex = 17
        '
        'picProgramLogo
        '
        Me.picProgramLogo.Cursor = System.Windows.Forms.Cursors.Hand
        Me.picProgramLogo.Location = New System.Drawing.Point(8, 8)
        Me.picProgramLogo.Name = "picProgramLogo"
        Me.picProgramLogo.Size = New System.Drawing.Size(136, 128)
        Me.picProgramLogo.TabIndex = 18
        Me.picProgramLogo.TabStop = False
        '
        'lblProgramName
        '
        Me.lblProgramName.AutoSize = True
        Me.lblProgramName.Font = New System.Drawing.Font("Microsoft Sans Serif", 27.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblProgramName.Location = New System.Drawing.Point(152, 16)
        Me.lblProgramName.Name = "lblProgramName"
        Me.lblProgramName.Size = New System.Drawing.Size(185, 42)
        Me.lblProgramName.TabIndex = 19
        Me.lblProgramName.Text = "BASINS 4"
        '
        'lblPluginVersion
        '
        Me.lblPluginVersion.Location = New System.Drawing.Point(160, 64)
        Me.lblPluginVersion.Name = "lblPluginVersion"
        Me.lblPluginVersion.Size = New System.Drawing.Size(176, 16)
        Me.lblPluginVersion.TabIndex = 20
        '
        'lblProgramURL
        '
        Me.lblProgramURL.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblProgramURL.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lblProgramURL.LinkArea = New System.Windows.Forms.LinkArea(0, 39)
        Me.lblProgramURL.Location = New System.Drawing.Point(315, 67)
        Me.lblProgramURL.Name = "lblProgramURL"
        Me.lblProgramURL.Size = New System.Drawing.Size(270, 13)
        Me.lblProgramURL.TabIndex = 2
        Me.lblProgramURL.TabStop = True
        Me.lblProgramURL.Text = "http://www.epa.gov/waterscience/BASINS/"
        Me.lblProgramURL.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'grpATC
        '
        Me.grpATC.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpATC.Controls.Add(Me.picATC)
        Me.grpATC.Controls.Add(Me.lblAquaTerraURL)
        Me.grpATC.Location = New System.Drawing.Point(12, 142)
        Me.grpATC.Name = "grpATC"
        Me.grpATC.Size = New System.Drawing.Size(588, 160)
        Me.grpATC.TabIndex = 3
        Me.grpATC.TabStop = False
        Me.grpATC.Text = "Developed by AQUA TERRA Consultants"
        '
        'picATC
        '
        Me.picATC.Cursor = System.Windows.Forms.Cursors.Hand
        Me.picATC.Image = CType(resources.GetObject("picATC.Image"), System.Drawing.Image)
        Me.picATC.Location = New System.Drawing.Point(8, 16)
        Me.picATC.Name = "picATC"
        Me.picATC.Size = New System.Drawing.Size(408, 136)
        Me.picATC.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.picATC.TabIndex = 25
        Me.picATC.TabStop = False
        '
        'lblAquaTerraURL
        '
        Me.lblAquaTerraURL.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblAquaTerraURL.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lblAquaTerraURL.LinkArea = New System.Windows.Forms.LinkArea(0, 25)
        Me.lblAquaTerraURL.Location = New System.Drawing.Point(422, 137)
        Me.lblAquaTerraURL.Name = "lblAquaTerraURL"
        Me.lblAquaTerraURL.Size = New System.Drawing.Size(151, 13)
        Me.lblAquaTerraURL.TabIndex = 4
        Me.lblAquaTerraURL.TabStop = True
        Me.lblAquaTerraURL.Text = "http://www.aquaterra.com/"
        Me.lblAquaTerraURL.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'grpZedGraph
        '
        Me.grpZedGraph.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpZedGraph.Controls.Add(Me.picZedGraph)
        Me.grpZedGraph.Controls.Add(Me.lblZedGraphURL)
        Me.grpZedGraph.Location = New System.Drawing.Point(12, 402)
        Me.grpZedGraph.Name = "grpZedGraph"
        Me.grpZedGraph.Size = New System.Drawing.Size(588, 72)
        Me.grpZedGraph.TabIndex = 7
        Me.grpZedGraph.TabStop = False
        Me.grpZedGraph.Text = "Graphing by ZedGraph"
        '
        'picZedGraph
        '
        Me.picZedGraph.Cursor = System.Windows.Forms.Cursors.Hand
        Me.picZedGraph.Image = CType(resources.GetObject("picZedGraph.Image"), System.Drawing.Image)
        Me.picZedGraph.Location = New System.Drawing.Point(16, 16)
        Me.picZedGraph.Name = "picZedGraph"
        Me.picZedGraph.Size = New System.Drawing.Size(176, 48)
        Me.picZedGraph.TabIndex = 28
        Me.picZedGraph.TabStop = False
        '
        'lblZedGraphURL
        '
        Me.lblZedGraphURL.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblZedGraphURL.LinkArea = New System.Windows.Forms.LinkArea(0, 41)
        Me.lblZedGraphURL.Location = New System.Drawing.Point(345, 47)
        Me.lblZedGraphURL.Name = "lblZedGraphURL"
        Me.lblZedGraphURL.Size = New System.Drawing.Size(228, 16)
        Me.lblZedGraphURL.TabIndex = 8
        Me.lblZedGraphURL.TabStop = True
        Me.lblZedGraphURL.Text = "http://sourceforge.net/projects/zedgraph/"
        Me.lblZedGraphURL.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'frmAbout
        '
        Me.AcceptButton = Me.btnOk
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.CancelButton = Me.btnOk
        Me.ClientSize = New System.Drawing.Size(612, 580)
        Me.Controls.Add(Me.lblProgramURL)
        Me.Controls.Add(Me.picProgramLogo)
        Me.Controls.Add(Me.lblPluginVersion)
        Me.Controls.Add(Me.lblProgramName)
        Me.Controls.Add(Me.lblConfigFile)
        Me.Controls.Add(Me.lblProjFile)
        Me.Controls.Add(Me.lblConfigFileLabel)
        Me.Controls.Add(Me.lblProjFileLabel)
        Me.Controls.Add(Me.btnOk)
        Me.Controls.Add(Me.grpMapWindow)
        Me.Controls.Add(Me.grpATC)
        Me.Controls.Add(Me.grpZedGraph)
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmAbout"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "About"
        Me.grpMapWindow.ResumeLayout(False)
        CType(Me.picMapWindow, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picProgramLogo, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpATC.ResumeLayout(False)
        CType(Me.picATC, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpZedGraph.ResumeLayout(False)
        CType(Me.picZedGraph, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Public Sub ShowAbout()
        Try
            Try
                Me.Icon = g_MapWin.ApplicationInfo.FormIcon
            Catch
            End Try
            Try
                picProgramLogo.Width = g_MapWin.ApplicationInfo.SplashPicture.Width
                picProgramLogo.Image = g_MapWin.ApplicationInfo.SplashPicture
            Catch
            End Try

            lblProgramName.Text = g_AppNameLong
            lblProgramName.Left = picProgramLogo.Left + picProgramLogo.Width + 8
            lblPluginVersion.Left = lblProgramName.Left

            If lblProgramName.Text.Length < 10 Then
                lblProgramName.Font = New System.Drawing.Font("Microsoft Sans Serif", 27.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Else
                lblProgramName.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            End If

            lblProgramURL.Text = g_URL_Home
            lblProgramURL.LinkArea = New LinkArea(0, g_URL_Home.Length)
            lblProgramURL.Left = Me.Width - lblProgramURL.Width - 20

            Try
                lblPluginVersion.Text = "Build Date:  " & IO.File.GetLastWriteTime(Me.GetType().Assembly.Location).ToShortDateString
            Catch
            End Try
            Try
                lblMapwinVersion.Text = System.Reflection.Assembly.GetEntryAssembly.GetName.Version.ToString
            Catch
            End Try
            Try
                lblProjFile.Text = g_Project.FileName
                lblConfigFile.Text = g_Project.ConfigFileName
            Catch
            End Try
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
        Handles lblProgramURL.LinkClicked
        OpenLinkURL(lblProgramURL)
    End Sub

    Private Sub picBASINS_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
        Handles picProgramLogo.Click
        OpenLinkURL(lblProgramURL)
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
