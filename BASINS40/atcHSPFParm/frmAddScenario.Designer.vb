<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAddScenario
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAddScenario))
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.Label11 = New System.Windows.Forms.Label
        Me.Label12 = New System.Windows.Forms.Label
        Me.Label13 = New System.Windows.Forms.Label
        Me.Label14 = New System.Windows.Forms.Label
        Me.Label15 = New System.Windows.Forms.Label
        Me.Label16 = New System.Windows.Forms.Label
        Me.Label17 = New System.Windows.Forms.Label
        Me.Label18 = New System.Windows.Forms.Label
        Me.Label19 = New System.Windows.Forms.Label
        Me.Label20 = New System.Windows.Forms.Label
        Me.Label21 = New System.Windows.Forms.Label
        Me.txtUCIName = New System.Windows.Forms.TextBox
        Me.txtName = New System.Windows.Forms.TextBox
        Me.txtType = New System.Windows.Forms.TextBox
        Me.txtWatershedId = New System.Windows.Forms.TextBox
        Me.txtStartDate = New System.Windows.Forms.TextBox
        Me.txtEndDate = New System.Windows.Forms.TextBox
        Me.txtUnits = New System.Windows.Forms.TextBox
        Me.txtNumSegments = New System.Windows.Forms.TextBox
        Me.txtNumReaches = New System.Windows.Forms.TextBox
        Me.txtLandUseType = New System.Windows.Forms.TextBox
        Me.txtChannels = New System.Windows.Forms.TextBox
        Me.txtWQConstituents = New System.Windows.Forms.TextBox
        Me.txtSources = New System.Windows.Forms.TextBox
        Me.txtPurpose = New System.Windows.Forms.TextBox
        Me.txtVersion = New System.Windows.Forms.TextBox
        Me.txtReference = New System.Windows.Forms.TextBox
        Me.txtContactName = New System.Windows.Forms.TextBox
        Me.txtOrganization = New System.Windows.Forms.TextBox
        Me.txtPhone = New System.Windows.Forms.TextBox
        Me.txtComments = New System.Windows.Forms.TextBox
        Me.cmdClose = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.cdUCI = New System.Windows.Forms.OpenFileDialog
        Me.cmdSet = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Label1.Location = New System.Drawing.Point(87, 24)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(47, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "UCI File:"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(97, 50)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(38, 13)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Name:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(101, 76)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(34, 13)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Type:"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(59, 102)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(76, 13)
        Me.Label5.TabIndex = 4
        Me.Label5.Text = "Watershed ID:"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(76, 128)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(58, 13)
        Me.Label6.TabIndex = 5
        Me.Label6.Text = "Start Date:"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(79, 154)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(55, 13)
        Me.Label7.TabIndex = 6
        Me.Label7.Text = "End Date:"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(79, 180)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(55, 13)
        Me.Label8.TabIndex = 7
        Me.Label8.Text = "UCI Units:"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(52, 206)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(82, 13)
        Me.Label9.TabIndex = 8
        Me.Label9.Text = "Num Segments:"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(56, 232)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(78, 13)
        Me.Label10.TabIndex = 9
        Me.Label10.Text = "Num Reaches:"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(52, 258)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(83, 13)
        Me.Label11.TabIndex = 10
        Me.Label11.Text = "Land Use Type:"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(81, 284)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(54, 13)
        Me.Label12.TabIndex = 11
        Me.Label12.Text = "Channels:"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(45, 310)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(90, 13)
        Me.Label13.TabIndex = 12
        Me.Label13.Text = "WQ Constituents:"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(40, 336)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(95, 13)
        Me.Label14.TabIndex = 13
        Me.Label14.Text = "Chemical Sources:"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(55, 362)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(79, 13)
        Me.Label15.TabIndex = 14
        Me.Label15.Text = "Study Purpose:"
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(89, 388)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(45, 13)
        Me.Label16.TabIndex = 15
        Me.Label16.Text = "Version:"
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Location = New System.Drawing.Point(19, 414)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(115, 13)
        Me.Label17.TabIndex = 16
        Me.Label17.Text = "Application Reference:"
        Me.Label17.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Location = New System.Drawing.Point(57, 440)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(78, 13)
        Me.Label18.TabIndex = 17
        Me.Label18.Text = "Contact Name:"
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Location = New System.Drawing.Point(26, 466)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(109, 13)
        Me.Label19.TabIndex = 18
        Me.Label19.Text = "Contact Organization:"
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Location = New System.Drawing.Point(26, 492)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(109, 13)
        Me.Label20.TabIndex = 19
        Me.Label20.Text = "Contact Phone Email:"
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.Location = New System.Drawing.Point(75, 518)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(59, 13)
        Me.Label21.TabIndex = 20
        Me.Label21.Text = "Comments:"
        '
        'txtUCIName
        '
        Me.txtUCIName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtUCIName.Location = New System.Drawing.Point(140, 21)
        Me.txtUCIName.Name = "txtUCIName"
        Me.txtUCIName.Size = New System.Drawing.Size(417, 20)
        Me.txtUCIName.TabIndex = 21
        '
        'txtName
        '
        Me.txtName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtName.Location = New System.Drawing.Point(140, 47)
        Me.txtName.Name = "txtName"
        Me.txtName.Size = New System.Drawing.Size(417, 20)
        Me.txtName.TabIndex = 22
        '
        'txtType
        '
        Me.txtType.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtType.Location = New System.Drawing.Point(140, 73)
        Me.txtType.Name = "txtType"
        Me.txtType.Size = New System.Drawing.Size(417, 20)
        Me.txtType.TabIndex = 23
        '
        'txtWatershedId
        '
        Me.txtWatershedId.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtWatershedId.Location = New System.Drawing.Point(140, 99)
        Me.txtWatershedId.Name = "txtWatershedId"
        Me.txtWatershedId.Size = New System.Drawing.Size(417, 20)
        Me.txtWatershedId.TabIndex = 25
        '
        'txtStartDate
        '
        Me.txtStartDate.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtStartDate.Location = New System.Drawing.Point(140, 125)
        Me.txtStartDate.Name = "txtStartDate"
        Me.txtStartDate.Size = New System.Drawing.Size(417, 20)
        Me.txtStartDate.TabIndex = 26
        '
        'txtEndDate
        '
        Me.txtEndDate.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtEndDate.Location = New System.Drawing.Point(140, 151)
        Me.txtEndDate.Name = "txtEndDate"
        Me.txtEndDate.Size = New System.Drawing.Size(417, 20)
        Me.txtEndDate.TabIndex = 27
        '
        'txtUnits
        '
        Me.txtUnits.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtUnits.Location = New System.Drawing.Point(140, 177)
        Me.txtUnits.Name = "txtUnits"
        Me.txtUnits.Size = New System.Drawing.Size(417, 20)
        Me.txtUnits.TabIndex = 28
        '
        'txtNumSegments
        '
        Me.txtNumSegments.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtNumSegments.Location = New System.Drawing.Point(140, 203)
        Me.txtNumSegments.Name = "txtNumSegments"
        Me.txtNumSegments.Size = New System.Drawing.Size(417, 20)
        Me.txtNumSegments.TabIndex = 29
        '
        'txtNumReaches
        '
        Me.txtNumReaches.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtNumReaches.Location = New System.Drawing.Point(140, 229)
        Me.txtNumReaches.Name = "txtNumReaches"
        Me.txtNumReaches.Size = New System.Drawing.Size(417, 20)
        Me.txtNumReaches.TabIndex = 30
        '
        'txtLandUseType
        '
        Me.txtLandUseType.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtLandUseType.Location = New System.Drawing.Point(140, 255)
        Me.txtLandUseType.Name = "txtLandUseType"
        Me.txtLandUseType.Size = New System.Drawing.Size(417, 20)
        Me.txtLandUseType.TabIndex = 31
        '
        'txtChannels
        '
        Me.txtChannels.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtChannels.Location = New System.Drawing.Point(140, 281)
        Me.txtChannels.Name = "txtChannels"
        Me.txtChannels.Size = New System.Drawing.Size(417, 20)
        Me.txtChannels.TabIndex = 32
        '
        'txtWQConstituents
        '
        Me.txtWQConstituents.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtWQConstituents.Location = New System.Drawing.Point(140, 307)
        Me.txtWQConstituents.Name = "txtWQConstituents"
        Me.txtWQConstituents.Size = New System.Drawing.Size(417, 20)
        Me.txtWQConstituents.TabIndex = 33
        '
        'txtSources
        '
        Me.txtSources.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtSources.Location = New System.Drawing.Point(140, 333)
        Me.txtSources.Name = "txtSources"
        Me.txtSources.Size = New System.Drawing.Size(417, 20)
        Me.txtSources.TabIndex = 34
        '
        'txtPurpose
        '
        Me.txtPurpose.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtPurpose.Location = New System.Drawing.Point(140, 359)
        Me.txtPurpose.Name = "txtPurpose"
        Me.txtPurpose.Size = New System.Drawing.Size(417, 20)
        Me.txtPurpose.TabIndex = 35
        '
        'txtVersion
        '
        Me.txtVersion.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtVersion.Location = New System.Drawing.Point(140, 385)
        Me.txtVersion.Name = "txtVersion"
        Me.txtVersion.Size = New System.Drawing.Size(417, 20)
        Me.txtVersion.TabIndex = 36
        '
        'txtReference
        '
        Me.txtReference.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtReference.Location = New System.Drawing.Point(140, 411)
        Me.txtReference.Name = "txtReference"
        Me.txtReference.Size = New System.Drawing.Size(417, 20)
        Me.txtReference.TabIndex = 37
        '
        'txtContactName
        '
        Me.txtContactName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtContactName.Location = New System.Drawing.Point(140, 437)
        Me.txtContactName.Name = "txtContactName"
        Me.txtContactName.Size = New System.Drawing.Size(417, 20)
        Me.txtContactName.TabIndex = 38
        '
        'txtOrganization
        '
        Me.txtOrganization.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtOrganization.Location = New System.Drawing.Point(140, 463)
        Me.txtOrganization.Name = "txtOrganization"
        Me.txtOrganization.Size = New System.Drawing.Size(417, 20)
        Me.txtOrganization.TabIndex = 39
        '
        'txtPhone
        '
        Me.txtPhone.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtPhone.Location = New System.Drawing.Point(140, 489)
        Me.txtPhone.Name = "txtPhone"
        Me.txtPhone.Size = New System.Drawing.Size(417, 20)
        Me.txtPhone.TabIndex = 40
        '
        'txtComments
        '
        Me.txtComments.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtComments.Location = New System.Drawing.Point(140, 515)
        Me.txtComments.Name = "txtComments"
        Me.txtComments.Size = New System.Drawing.Size(417, 20)
        Me.txtComments.TabIndex = 41
        '
        'cmdClose
        '
        Me.cmdClose.Location = New System.Drawing.Point(301, 582)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.Size = New System.Drawing.Size(64, 25)
        Me.cmdClose.TabIndex = 42
        Me.cmdClose.Text = "Cancel"
        Me.cmdClose.UseVisualStyleBackColor = True
        '
        'cmdOK
        '
        Me.cmdOK.Location = New System.Drawing.Point(220, 582)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.Size = New System.Drawing.Size(64, 25)
        Me.cmdOK.TabIndex = 43
        Me.cmdOK.Text = "OK"
        Me.cmdOK.UseVisualStyleBackColor = True
        '
        'cdUCI
        '
        Me.cdUCI.Filter = "UCI files (*.uci)|*.uci"
        '
        'cmdSet
        '
        Me.cmdSet.Location = New System.Drawing.Point(12, 21)
        Me.cmdSet.Name = "cmdSet"
        Me.cmdSet.Size = New System.Drawing.Size(55, 20)
        Me.cmdSet.TabIndex = 44
        Me.cmdSet.Text = "Set"
        Me.cmdSet.UseVisualStyleBackColor = True
        '
        'frmAddScenario
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(582, 619)
        Me.Controls.Add(Me.cmdSet)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.cmdClose)
        Me.Controls.Add(Me.txtComments)
        Me.Controls.Add(Me.txtPhone)
        Me.Controls.Add(Me.txtOrganization)
        Me.Controls.Add(Me.txtContactName)
        Me.Controls.Add(Me.txtReference)
        Me.Controls.Add(Me.txtVersion)
        Me.Controls.Add(Me.txtPurpose)
        Me.Controls.Add(Me.txtSources)
        Me.Controls.Add(Me.txtWQConstituents)
        Me.Controls.Add(Me.txtChannels)
        Me.Controls.Add(Me.txtLandUseType)
        Me.Controls.Add(Me.txtNumReaches)
        Me.Controls.Add(Me.txtNumSegments)
        Me.Controls.Add(Me.txtUnits)
        Me.Controls.Add(Me.txtEndDate)
        Me.Controls.Add(Me.txtStartDate)
        Me.Controls.Add(Me.txtWatershedId)
        Me.Controls.Add(Me.txtType)
        Me.Controls.Add(Me.txtName)
        Me.Controls.Add(Me.txtUCIName)
        Me.Controls.Add(Me.Label21)
        Me.Controls.Add(Me.Label20)
        Me.Controls.Add(Me.Label19)
        Me.Controls.Add(Me.Label18)
        Me.Controls.Add(Me.Label17)
        Me.Controls.Add(Me.Label16)
        Me.Controls.Add(Me.Label15)
        Me.Controls.Add(Me.Label14)
        Me.Controls.Add(Me.Label13)
        Me.Controls.Add(Me.Label12)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmAddScenario"
        Me.Text = "Add Scenario"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents txtUCIName As System.Windows.Forms.TextBox
    Friend WithEvents txtName As System.Windows.Forms.TextBox
    Friend WithEvents txtType As System.Windows.Forms.TextBox
    Friend WithEvents txtWatershedId As System.Windows.Forms.TextBox
    Friend WithEvents txtStartDate As System.Windows.Forms.TextBox
    Friend WithEvents txtEndDate As System.Windows.Forms.TextBox
    Friend WithEvents txtUnits As System.Windows.Forms.TextBox
    Friend WithEvents txtNumSegments As System.Windows.Forms.TextBox
    Friend WithEvents txtNumReaches As System.Windows.Forms.TextBox
    Friend WithEvents txtLandUseType As System.Windows.Forms.TextBox
    Friend WithEvents txtChannels As System.Windows.Forms.TextBox
    Friend WithEvents txtWQConstituents As System.Windows.Forms.TextBox
    Friend WithEvents txtSources As System.Windows.Forms.TextBox
    Friend WithEvents txtPurpose As System.Windows.Forms.TextBox
    Friend WithEvents txtVersion As System.Windows.Forms.TextBox
    Friend WithEvents txtReference As System.Windows.Forms.TextBox
    Friend WithEvents txtContactName As System.Windows.Forms.TextBox
    Friend WithEvents txtOrganization As System.Windows.Forms.TextBox
    Friend WithEvents txtPhone As System.Windows.Forms.TextBox
    Friend WithEvents txtComments As System.Windows.Forms.TextBox
    Friend WithEvents cmdClose As System.Windows.Forms.Button
    Friend WithEvents cmdOK As System.Windows.Forms.Button
    Friend WithEvents cdUCI As System.Windows.Forms.OpenFileDialog
    Friend WithEvents cmdSet As System.Windows.Forms.Button
End Class
