<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAddWatershed
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAddWatershed))
        Me.lblProject = New System.Windows.Forms.Label
        Me.lblHuc = New System.Windows.Forms.Label
        Me.lblLocation = New System.Windows.Forms.Label
        Me.lblArea = New System.Windows.Forms.Label
        Me.lblComments = New System.Windows.Forms.Label
        Me.lblSetting = New System.Windows.Forms.Label
        Me.lblWeather = New System.Windows.Forms.Label
        Me.txtProject = New System.Windows.Forms.TextBox
        Me.txtHuc = New System.Windows.Forms.TextBox
        Me.txtLocation = New System.Windows.Forms.TextBox
        Me.txtArea = New System.Windows.Forms.TextBox
        Me.txtComments = New System.Windows.Forms.TextBox
        Me.txtSetting = New System.Windows.Forms.TextBox
        Me.txtWeather = New System.Windows.Forms.TextBox
        Me.cmdClose = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.lblLatitude = New System.Windows.Forms.Label
        Me.lblLongitude = New System.Windows.Forms.Label
        Me.txtLatitude = New System.Windows.Forms.TextBox
        Me.txtLongitude = New System.Windows.Forms.TextBox
        Me.SuspendLayout()
        '
        'lblProject
        '
        Me.lblProject.AutoSize = True
        Me.lblProject.Location = New System.Drawing.Point(56, 27)
        Me.lblProject.Name = "lblProject"
        Me.lblProject.Size = New System.Drawing.Size(74, 13)
        Me.lblProject.TabIndex = 1
        Me.lblProject.Text = "Project Name:"
        '
        'lblHuc
        '
        Me.lblHuc.AutoSize = True
        Me.lblHuc.Location = New System.Drawing.Point(98, 53)
        Me.lblHuc.Name = "lblHuc"
        Me.lblHuc.Size = New System.Drawing.Size(33, 13)
        Me.lblHuc.TabIndex = 2
        Me.lblHuc.Text = "HUC:"
        '
        'lblLocation
        '
        Me.lblLocation.AutoSize = True
        Me.lblLocation.Location = New System.Drawing.Point(81, 79)
        Me.lblLocation.Name = "lblLocation"
        Me.lblLocation.Size = New System.Drawing.Size(51, 13)
        Me.lblLocation.TabIndex = 3
        Me.lblLocation.Text = "Location:"
        '
        'lblArea
        '
        Me.lblArea.AutoSize = True
        Me.lblArea.Location = New System.Drawing.Point(52, 105)
        Me.lblArea.Name = "lblArea"
        Me.lblArea.Size = New System.Drawing.Size(78, 13)
        Me.lblArea.TabIndex = 4
        Me.lblArea.Text = "Drainage Area:"
        '
        'lblComments
        '
        Me.lblComments.AutoSize = True
        Me.lblComments.Location = New System.Drawing.Point(71, 131)
        Me.lblComments.Name = "lblComments"
        Me.lblComments.Size = New System.Drawing.Size(59, 13)
        Me.lblComments.TabIndex = 5
        Me.lblComments.Text = "Comments:"
        '
        'lblSetting
        '
        Me.lblSetting.AutoSize = True
        Me.lblSetting.Location = New System.Drawing.Point(20, 157)
        Me.lblSetting.Name = "lblSetting"
        Me.lblSetting.Size = New System.Drawing.Size(112, 13)
        Me.lblSetting.TabIndex = 6
        Me.lblSetting.Text = "Physiographic Setting:"
        '
        'lblWeather
        '
        Me.lblWeather.AutoSize = True
        Me.lblWeather.Location = New System.Drawing.Point(42, 183)
        Me.lblWeather.Name = "lblWeather"
        Me.lblWeather.Size = New System.Drawing.Size(90, 13)
        Me.lblWeather.TabIndex = 7
        Me.lblWeather.Text = "Weather Regime:"
        '
        'txtProject
        '
        Me.txtProject.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtProject.Location = New System.Drawing.Point(137, 24)
        Me.txtProject.Name = "txtProject"
        Me.txtProject.Size = New System.Drawing.Size(417, 20)
        Me.txtProject.TabIndex = 22
        '
        'txtHuc
        '
        Me.txtHuc.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtHuc.Location = New System.Drawing.Point(137, 50)
        Me.txtHuc.Name = "txtHuc"
        Me.txtHuc.Size = New System.Drawing.Size(417, 20)
        Me.txtHuc.TabIndex = 23
        '
        'txtLocation
        '
        Me.txtLocation.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtLocation.Location = New System.Drawing.Point(137, 76)
        Me.txtLocation.Name = "txtLocation"
        Me.txtLocation.Size = New System.Drawing.Size(417, 20)
        Me.txtLocation.TabIndex = 24
        '
        'txtArea
        '
        Me.txtArea.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtArea.Location = New System.Drawing.Point(137, 102)
        Me.txtArea.Name = "txtArea"
        Me.txtArea.Size = New System.Drawing.Size(417, 20)
        Me.txtArea.TabIndex = 25
        '
        'txtComments
        '
        Me.txtComments.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtComments.Location = New System.Drawing.Point(137, 128)
        Me.txtComments.Name = "txtComments"
        Me.txtComments.Size = New System.Drawing.Size(417, 20)
        Me.txtComments.TabIndex = 26
        '
        'txtSetting
        '
        Me.txtSetting.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtSetting.Location = New System.Drawing.Point(137, 154)
        Me.txtSetting.Name = "txtSetting"
        Me.txtSetting.Size = New System.Drawing.Size(417, 20)
        Me.txtSetting.TabIndex = 27
        '
        'txtWeather
        '
        Me.txtWeather.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtWeather.Location = New System.Drawing.Point(137, 180)
        Me.txtWeather.Name = "txtWeather"
        Me.txtWeather.Size = New System.Drawing.Size(417, 20)
        Me.txtWeather.TabIndex = 28
        '
        'cmdClose
        '
        Me.cmdClose.Location = New System.Drawing.Point(297, 267)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.Size = New System.Drawing.Size(64, 25)
        Me.cmdClose.TabIndex = 42
        Me.cmdClose.Text = "Cancel"
        Me.cmdClose.UseVisualStyleBackColor = True
        '
        'cmdOK
        '
        Me.cmdOK.Location = New System.Drawing.Point(218, 267)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.Size = New System.Drawing.Size(64, 25)
        Me.cmdOK.TabIndex = 43
        Me.cmdOK.Text = "OK"
        Me.cmdOK.UseVisualStyleBackColor = True
        '
        'lblLatitude
        '
        Me.lblLatitude.AutoSize = True
        Me.lblLatitude.Location = New System.Drawing.Point(84, 210)
        Me.lblLatitude.Name = "lblLatitude"
        Me.lblLatitude.Size = New System.Drawing.Size(48, 13)
        Me.lblLatitude.TabIndex = 44
        Me.lblLatitude.Text = "Latitude:"
        '
        'lblLongitude
        '
        Me.lblLongitude.AutoSize = True
        Me.lblLongitude.Location = New System.Drawing.Point(75, 234)
        Me.lblLongitude.Name = "lblLongitude"
        Me.lblLongitude.Size = New System.Drawing.Size(57, 13)
        Me.lblLongitude.TabIndex = 45
        Me.lblLongitude.Text = "Longitude:"
        '
        'txtLatitude
        '
        Me.txtLatitude.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtLatitude.Location = New System.Drawing.Point(137, 207)
        Me.txtLatitude.Name = "txtLatitude"
        Me.txtLatitude.Size = New System.Drawing.Size(417, 20)
        Me.txtLatitude.TabIndex = 46
        '
        'txtLongitude
        '
        Me.txtLongitude.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtLongitude.Location = New System.Drawing.Point(137, 234)
        Me.txtLongitude.Name = "txtLongitude"
        Me.txtLongitude.Size = New System.Drawing.Size(417, 20)
        Me.txtLongitude.TabIndex = 47
        '
        'frmAddWatershed
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(582, 303)
        Me.Controls.Add(Me.txtLongitude)
        Me.Controls.Add(Me.txtLatitude)
        Me.Controls.Add(Me.lblLongitude)
        Me.Controls.Add(Me.lblLatitude)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.cmdClose)
        Me.Controls.Add(Me.txtWeather)
        Me.Controls.Add(Me.txtSetting)
        Me.Controls.Add(Me.txtComments)
        Me.Controls.Add(Me.txtArea)
        Me.Controls.Add(Me.txtLocation)
        Me.Controls.Add(Me.txtHuc)
        Me.Controls.Add(Me.txtProject)
        Me.Controls.Add(Me.lblWeather)
        Me.Controls.Add(Me.lblSetting)
        Me.Controls.Add(Me.lblComments)
        Me.Controls.Add(Me.lblArea)
        Me.Controls.Add(Me.lblLocation)
        Me.Controls.Add(Me.lblHuc)
        Me.Controls.Add(Me.lblProject)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmAddWatershed"
        Me.Text = "Add Watershed"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblProject As System.Windows.Forms.Label
    Friend WithEvents lblHuc As System.Windows.Forms.Label
    Friend WithEvents lblLocation As System.Windows.Forms.Label
    Friend WithEvents lblArea As System.Windows.Forms.Label
    Friend WithEvents lblComments As System.Windows.Forms.Label
    Friend WithEvents lblSetting As System.Windows.Forms.Label
    Friend WithEvents lblWeather As System.Windows.Forms.Label
    Friend WithEvents txtProject As System.Windows.Forms.TextBox
    Friend WithEvents txtHuc As System.Windows.Forms.TextBox
    Friend WithEvents txtLocation As System.Windows.Forms.TextBox
    Friend WithEvents txtArea As System.Windows.Forms.TextBox
    Friend WithEvents txtComments As System.Windows.Forms.TextBox
    Friend WithEvents txtSetting As System.Windows.Forms.TextBox
    Friend WithEvents txtWeather As System.Windows.Forms.TextBox
    Friend WithEvents cmdClose As System.Windows.Forms.Button
    Friend WithEvents cmdOK As System.Windows.Forms.Button
    Friend WithEvents lblLatitude As System.Windows.Forms.Label
    Friend WithEvents lblLongitude As System.Windows.Forms.Label
    Friend WithEvents txtLatitude As System.Windows.Forms.TextBox
    Friend WithEvents txtLongitude As System.Windows.Forms.TextBox
End Class
