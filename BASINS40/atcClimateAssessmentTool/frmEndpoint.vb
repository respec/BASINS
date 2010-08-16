Imports atcData
Imports atcSeasons
Imports atcUtility
Imports MapWinUtility

Public Class frmEndpoint
    Inherits System.Windows.Forms.Form

    Private pNotNumberString As String = "<none>"
    Private pVariation As atcVariation
    Private pSeasonsAvailable As New atcCollection
    Private pSeasons As atcSeasonBase
    Private pAllSeasons As Integer()

    Friend WithEvents lblOperation As System.Windows.Forms.Label
    Friend WithEvents txtOperation As System.Windows.Forms.TextBox
    Friend WithEvents panelOperation As System.Windows.Forms.Panel
    Friend WithEvents chkSeasons As System.Windows.Forms.CheckBox

    Private pSettingFormSeason As Boolean = False

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
    Friend WithEvents txtName As System.Windows.Forms.TextBox
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents lblAttribute As System.Windows.Forms.Label
    Friend WithEvents lblData As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents txtMin As System.Windows.Forms.TextBox
    Friend WithEvents lblMinimum As System.Windows.Forms.Label
    Friend WithEvents txtLowColor As System.Windows.Forms.TextBox
    Friend WithEvents lblLowColor As System.Windows.Forms.Label
    Friend WithEvents txtHighColor As System.Windows.Forms.TextBox
    Friend WithEvents lblName As System.Windows.Forms.Label
    Friend WithEvents txtData As System.Windows.Forms.TextBox
    Friend WithEvents lblHighColor As System.Windows.Forms.Label
    Friend WithEvents cboAttribute As System.Windows.Forms.ComboBox
    Friend WithEvents txtMax As System.Windows.Forms.TextBox
    Friend WithEvents lblMaximum As System.Windows.Forms.Label
    Friend WithEvents txtDefaultColor As System.Windows.Forms.TextBox
    Friend WithEvents lblDefaultColor As System.Windows.Forms.Label
    Friend WithEvents grpSeasons As System.Windows.Forms.GroupBox
    Friend WithEvents cboSeasons As System.Windows.Forms.ComboBox
    Friend WithEvents lstSeasons As System.Windows.Forms.ListBox
    Friend WithEvents btnSeasonsAll As System.Windows.Forms.Button
    Friend WithEvents btnSeasonsNone As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmEndpoint))
        Me.lblName = New System.Windows.Forms.Label
        Me.txtName = New System.Windows.Forms.TextBox
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnOk = New System.Windows.Forms.Button
        Me.lblAttribute = New System.Windows.Forms.Label
        Me.lblData = New System.Windows.Forms.Label
        Me.txtData = New System.Windows.Forms.TextBox
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.txtHighColor = New System.Windows.Forms.TextBox
        Me.lblHighColor = New System.Windows.Forms.Label
        Me.txtLowColor = New System.Windows.Forms.TextBox
        Me.lblLowColor = New System.Windows.Forms.Label
        Me.txtMax = New System.Windows.Forms.TextBox
        Me.txtMin = New System.Windows.Forms.TextBox
        Me.lblMaximum = New System.Windows.Forms.Label
        Me.lblMinimum = New System.Windows.Forms.Label
        Me.txtDefaultColor = New System.Windows.Forms.TextBox
        Me.lblDefaultColor = New System.Windows.Forms.Label
        Me.cboAttribute = New System.Windows.Forms.ComboBox
        Me.grpSeasons = New System.Windows.Forms.GroupBox
        Me.cboSeasons = New System.Windows.Forms.ComboBox
        Me.lstSeasons = New System.Windows.Forms.ListBox
        Me.btnSeasonsAll = New System.Windows.Forms.Button
        Me.btnSeasonsNone = New System.Windows.Forms.Button
        Me.lblOperation = New System.Windows.Forms.Label
        Me.txtOperation = New System.Windows.Forms.TextBox
        Me.panelOperation = New System.Windows.Forms.Panel
        Me.chkSeasons = New System.Windows.Forms.CheckBox
        Me.GroupBox1.SuspendLayout()
        Me.grpSeasons.SuspendLayout()
        Me.panelOperation.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblName
        '
        Me.lblName.AutoSize = True
        Me.lblName.BackColor = System.Drawing.Color.Transparent
        Me.lblName.Location = New System.Drawing.Point(12, 19)
        Me.lblName.Name = "lblName"
        Me.lblName.Size = New System.Drawing.Size(83, 13)
        Me.lblName.TabIndex = 0
        Me.lblName.Text = "Endpoint Name:"
        Me.lblName.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'txtName
        '
        Me.txtName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtName.Location = New System.Drawing.Point(101, 16)
        Me.txtName.Name = "txtName"
        Me.txtName.Size = New System.Drawing.Size(217, 20)
        Me.txtName.TabIndex = 1
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(252, 528)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(72, 24)
        Me.btnCancel.TabIndex = 23
        Me.btnCancel.Text = "Cancel"
        '
        'btnOk
        '
        Me.btnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOk.Location = New System.Drawing.Point(174, 528)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(72, 24)
        Me.btnOk.TabIndex = 22
        Me.btnOk.Text = "Ok"
        '
        'lblAttribute
        '
        Me.lblAttribute.AutoSize = True
        Me.lblAttribute.BackColor = System.Drawing.Color.Transparent
        Me.lblAttribute.Location = New System.Drawing.Point(46, 67)
        Me.lblAttribute.Name = "lblAttribute"
        Me.lblAttribute.Size = New System.Drawing.Size(49, 13)
        Me.lblAttribute.TabIndex = 4
        Me.lblAttribute.Text = "Attribute:"
        Me.lblAttribute.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblData
        '
        Me.lblData.AutoSize = True
        Me.lblData.BackColor = System.Drawing.Color.Transparent
        Me.lblData.Location = New System.Drawing.Point(45, 43)
        Me.lblData.Name = "lblData"
        Me.lblData.Size = New System.Drawing.Size(50, 13)
        Me.lblData.TabIndex = 2
        Me.lblData.Text = "Data set:"
        Me.lblData.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'txtData
        '
        Me.txtData.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtData.Location = New System.Drawing.Point(101, 40)
        Me.txtData.Name = "txtData"
        Me.txtData.Size = New System.Drawing.Size(217, 20)
        Me.txtData.TabIndex = 3
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.txtHighColor)
        Me.GroupBox1.Controls.Add(Me.lblHighColor)
        Me.GroupBox1.Controls.Add(Me.txtLowColor)
        Me.GroupBox1.Controls.Add(Me.lblLowColor)
        Me.GroupBox1.Controls.Add(Me.txtMax)
        Me.GroupBox1.Controls.Add(Me.txtMin)
        Me.GroupBox1.Controls.Add(Me.lblMaximum)
        Me.GroupBox1.Controls.Add(Me.lblMinimum)
        Me.GroupBox1.Controls.Add(Me.txtDefaultColor)
        Me.GroupBox1.Controls.Add(Me.lblDefaultColor)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 96)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(312, 160)
        Me.GroupBox1.TabIndex = 6
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Highlight Values"
        '
        'txtHighColor
        '
        Me.txtHighColor.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtHighColor.BackColor = System.Drawing.Color.OrangeRed
        Me.txtHighColor.Location = New System.Drawing.Point(144, 120)
        Me.txtHighColor.Name = "txtHighColor"
        Me.txtHighColor.Size = New System.Drawing.Size(162, 20)
        Me.txtHighColor.TabIndex = 16
        '
        'lblHighColor
        '
        Me.lblHighColor.BackColor = System.Drawing.Color.Transparent
        Me.lblHighColor.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblHighColor.Location = New System.Drawing.Point(16, 120)
        Me.lblHighColor.Name = "lblHighColor"
        Me.lblHighColor.Size = New System.Drawing.Size(120, 17)
        Me.lblHighColor.TabIndex = 15
        Me.lblHighColor.Text = "Color Higher Values:"
        Me.lblHighColor.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtLowColor
        '
        Me.txtLowColor.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtLowColor.BackColor = System.Drawing.Color.DeepSkyBlue
        Me.txtLowColor.Location = New System.Drawing.Point(144, 72)
        Me.txtLowColor.Name = "txtLowColor"
        Me.txtLowColor.Size = New System.Drawing.Size(162, 20)
        Me.txtLowColor.TabIndex = 12
        '
        'lblLowColor
        '
        Me.lblLowColor.BackColor = System.Drawing.Color.Transparent
        Me.lblLowColor.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblLowColor.Location = New System.Drawing.Point(16, 72)
        Me.lblLowColor.Name = "lblLowColor"
        Me.lblLowColor.Size = New System.Drawing.Size(120, 17)
        Me.lblLowColor.TabIndex = 11
        Me.lblLowColor.Text = "Color Lower Values:"
        Me.lblLowColor.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtMax
        '
        Me.txtMax.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtMax.Location = New System.Drawing.Point(144, 96)
        Me.txtMax.Name = "txtMax"
        Me.txtMax.Size = New System.Drawing.Size(162, 20)
        Me.txtMax.TabIndex = 14
        Me.txtMax.Text = "<none>"
        '
        'txtMin
        '
        Me.txtMin.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtMin.Location = New System.Drawing.Point(144, 48)
        Me.txtMin.Name = "txtMin"
        Me.txtMin.Size = New System.Drawing.Size(162, 20)
        Me.txtMin.TabIndex = 10
        Me.txtMin.Text = "<none>"
        '
        'lblMaximum
        '
        Me.lblMaximum.BackColor = System.Drawing.Color.Transparent
        Me.lblMaximum.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblMaximum.Location = New System.Drawing.Point(16, 96)
        Me.lblMaximum.Name = "lblMaximum"
        Me.lblMaximum.Size = New System.Drawing.Size(120, 17)
        Me.lblMaximum.TabIndex = 13
        Me.lblMaximum.Text = "Maximum Value:"
        Me.lblMaximum.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblMinimum
        '
        Me.lblMinimum.BackColor = System.Drawing.Color.Transparent
        Me.lblMinimum.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblMinimum.Location = New System.Drawing.Point(16, 48)
        Me.lblMinimum.Name = "lblMinimum"
        Me.lblMinimum.Size = New System.Drawing.Size(120, 17)
        Me.lblMinimum.TabIndex = 9
        Me.lblMinimum.Text = "Minimum Value:"
        Me.lblMinimum.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtDefaultColor
        '
        Me.txtDefaultColor.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDefaultColor.BackColor = System.Drawing.Color.White
        Me.txtDefaultColor.Location = New System.Drawing.Point(144, 24)
        Me.txtDefaultColor.Name = "txtDefaultColor"
        Me.txtDefaultColor.Size = New System.Drawing.Size(162, 20)
        Me.txtDefaultColor.TabIndex = 8
        '
        'lblDefaultColor
        '
        Me.lblDefaultColor.BackColor = System.Drawing.Color.Transparent
        Me.lblDefaultColor.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblDefaultColor.Location = New System.Drawing.Point(16, 24)
        Me.lblDefaultColor.Name = "lblDefaultColor"
        Me.lblDefaultColor.Size = New System.Drawing.Size(120, 17)
        Me.lblDefaultColor.TabIndex = 7
        Me.lblDefaultColor.Text = "Default Color:"
        Me.lblDefaultColor.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cboAttribute
        '
        Me.cboAttribute.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboAttribute.Location = New System.Drawing.Point(101, 64)
        Me.cboAttribute.MaxDropDownItems = 20
        Me.cboAttribute.Name = "cboAttribute"
        Me.cboAttribute.Size = New System.Drawing.Size(217, 21)
        Me.cboAttribute.TabIndex = 5
        Me.cboAttribute.Text = "ComboBox1"
        '
        'grpSeasons
        '
        Me.grpSeasons.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpSeasons.Controls.Add(Me.chkSeasons)
        Me.grpSeasons.Controls.Add(Me.cboSeasons)
        Me.grpSeasons.Controls.Add(Me.lstSeasons)
        Me.grpSeasons.Controls.Add(Me.btnSeasonsAll)
        Me.grpSeasons.Controls.Add(Me.btnSeasonsNone)
        Me.grpSeasons.Location = New System.Drawing.Point(12, 262)
        Me.grpSeasons.Name = "grpSeasons"
        Me.grpSeasons.Size = New System.Drawing.Size(312, 260)
        Me.grpSeasons.TabIndex = 17
        Me.grpSeasons.TabStop = False
        Me.grpSeasons.Text = "Seasons"
        '
        'cboSeasons
        '
        Me.cboSeasons.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboSeasons.ItemHeight = 13
        Me.cboSeasons.Location = New System.Drawing.Point(184, 24)
        Me.cboSeasons.MaxDropDownItems = 20
        Me.cboSeasons.Name = "cboSeasons"
        Me.cboSeasons.Size = New System.Drawing.Size(122, 21)
        Me.cboSeasons.TabIndex = 18
        '
        'lstSeasons
        '
        Me.lstSeasons.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstSeasons.IntegralHeight = False
        Me.lstSeasons.Location = New System.Drawing.Point(6, 48)
        Me.lstSeasons.MultiColumn = True
        Me.lstSeasons.Name = "lstSeasons"
        Me.lstSeasons.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple
        Me.lstSeasons.Size = New System.Drawing.Size(300, 177)
        Me.lstSeasons.TabIndex = 19
        '
        'btnSeasonsAll
        '
        Me.btnSeasonsAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnSeasonsAll.Location = New System.Drawing.Point(6, 231)
        Me.btnSeasonsAll.Name = "btnSeasonsAll"
        Me.btnSeasonsAll.Size = New System.Drawing.Size(63, 23)
        Me.btnSeasonsAll.TabIndex = 20
        Me.btnSeasonsAll.Text = "All"
        '
        'btnSeasonsNone
        '
        Me.btnSeasonsNone.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSeasonsNone.Location = New System.Drawing.Point(243, 231)
        Me.btnSeasonsNone.Name = "btnSeasonsNone"
        Me.btnSeasonsNone.Size = New System.Drawing.Size(63, 23)
        Me.btnSeasonsNone.TabIndex = 21
        Me.btnSeasonsNone.Text = "None"
        '
        'lblOperation
        '
        Me.lblOperation.AutoSize = True
        Me.lblOperation.BackColor = System.Drawing.Color.Transparent
        Me.lblOperation.Location = New System.Drawing.Point(27, 9)
        Me.lblOperation.Name = "lblOperation"
        Me.lblOperation.Size = New System.Drawing.Size(56, 13)
        Me.lblOperation.TabIndex = 5
        Me.lblOperation.Text = "Operation:"
        Me.lblOperation.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtOperation
        '
        Me.txtOperation.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtOperation.Location = New System.Drawing.Point(89, 6)
        Me.txtOperation.Name = "txtOperation"
        Me.txtOperation.Size = New System.Drawing.Size(217, 20)
        Me.txtOperation.TabIndex = 6
        '
        'panelOperation
        '
        Me.panelOperation.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.panelOperation.Controls.Add(Me.txtOperation)
        Me.panelOperation.Controls.Add(Me.lblOperation)
        Me.panelOperation.Location = New System.Drawing.Point(12, 60)
        Me.panelOperation.Name = "panelOperation"
        Me.panelOperation.Size = New System.Drawing.Size(312, 30)
        Me.panelOperation.TabIndex = 24
        Me.panelOperation.Visible = False
        '
        'chkSeasons
        '
        Me.chkSeasons.AutoSize = True
        Me.chkSeasons.Location = New System.Drawing.Point(6, 26)
        Me.chkSeasons.Name = "chkSeasons"
        Me.chkSeasons.Size = New System.Drawing.Size(172, 17)
        Me.chkSeasons.TabIndex = 40
        Me.chkSeasons.Text = "Only include values in selected"
        Me.chkSeasons.UseVisualStyleBackColor = True
        '
        'frmEndpoint
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(336, 564)
        Me.Controls.Add(Me.panelOperation)
        Me.Controls.Add(Me.lblAttribute)
        Me.Controls.Add(Me.grpSeasons)
        Me.Controls.Add(Me.cboAttribute)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.lblName)
        Me.Controls.Add(Me.txtName)
        Me.Controls.Add(Me.txtData)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOk)
        Me.Controls.Add(Me.lblData)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Name = "frmEndpoint"
        Me.Text = "Endpoint"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.grpSeasons.ResumeLayout(False)
        Me.grpSeasons.PerformLayout()
        Me.panelOperation.ResumeLayout(False)
        Me.panelOperation.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Public Function AskUser(Optional ByVal aVariation As atcVariation = Nothing) As Boolean
        pVariation = aVariation.Clone

        If pVariation.DataSets Is Nothing Then pVariation.DataSets = New atcTimeseriesGroup

        If pVariation.IsInput Then
            panelOperation.Visible = True
            grpSeasons.Visible = False
        Else
            For Each lAttribute As atcAttributeDefinition In atcDataAttributes.AllDefinitions
                If lAttribute.TypeString.ToLower.Equals("double") AndAlso atcDataAttributes.IsSimple(lAttribute) Then
                    cboAttribute.Items.Add(lAttribute.Name)
                End If
            Next

            'cboSeasons.Items.Add(AllSeasons)
            pSeasonsAvailable = atcSeasonPlugin.AllSeasonTypes
            For Each lSeasonType As Type In pSeasonsAvailable
                Dim lSeasonTypeShortName As String = atcSeasonPlugin.SeasonClassNameToLabel(lSeasonType.Name)
                Select Case lSeasonTypeShortName
                    Case "Calendar Year", "Water Year", "Month"
                        cboSeasons.Items.Add(lSeasonTypeShortName & "s")
                End Select
            Next
        End If

        FormFromVariation()

        If Me.ShowDialog() = Windows.Forms.DialogResult.OK Then
            pVariation.CopyTo(aVariation)
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub txtData_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtData.Click
        UserSelectData()
    End Sub

    Private Sub txtData_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtData.KeyPress
        UserSelectData()
    End Sub

    Private Sub UserSelectData()
        Dim lData As atcTimeseriesGroup = atcDataManager.UserSelectData("Select data for endpoint", pVariation.DataSets)
        If Not lData Is Nothing Then
            pVariation.DataSets = lData
            UpdateDataText(txtData, lData)
        End If
    End Sub

    Private Sub UpdateDataText(ByVal aTextBox As Windows.Forms.TextBox, _
                               ByVal aGroup As atcTimeseriesGroup)
        If Not aGroup Is Nothing AndAlso aGroup.Count > 0 Then
            aTextBox.Text = aGroup.ItemByIndex(0).ToString
            If aGroup.Count > 1 Then aTextBox.Text &= " (and " & aGroup.Count - 1 & " more)"
        Else
            aTextBox.Text = "<click to select data>"
        End If
    End Sub

    Private Sub SetAllSeasons()
        If pSeasons.AllSeasons.Length = 0 Then
            Dim lTimeseries As atcTimeseries = pVariation.DataSets.ItemByIndex(0)
            pAllSeasons = pSeasons.AllSeasonsInDates(lTimeseries.Dates.Values)
        Else
            pAllSeasons = pSeasons.AllSeasons
        End If
    End Sub

    Private Sub cboSeasons_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboSeasons.SelectedIndexChanged
        If Not pSettingFormSeason Then
            pSeasons = Nothing
            pAllSeasons = Nothing
            lstSeasons.Items.Clear()
            'If cboSeasons.Text <> AllSeasons Then
            Try
                pSeasons = SelectedSeasonType.InvokeMember(Nothing, Reflection.BindingFlags.CreateInstance, Nothing, Nothing, New Object() {})
                SetAllSeasons()
                'For Each lSeasonIndex As Integer In pSeasons.AllSeasons
                '    pSeasons.SeasonSelected(lSeasonIndex) = True
                'Next
                RefreshSeasonsList()
            Catch ex As Exception
                Logger.Dbg("Could not create new seasons for '" & cboSeasons.Text & "': " & ex.ToString)
            End Try
            'End If
        End If
    End Sub

    Private Sub RefreshSeasonsList()
        Try
            Dim lMaxWidth As Integer = 0
            Dim lSeasonName As String
            lstSeasons.Items.Clear()
            For Each lSeasonIndex As Integer In pAllSeasons
                lSeasonName = pSeasons.SeasonName(lSeasonIndex)
                If lSeasonName.Length > lMaxWidth Then lMaxWidth = lSeasonName.Length
                lstSeasons.Items.Add(lSeasonName)
                lstSeasons.SetSelected(lstSeasons.Items.Count - 1, pSeasons.SeasonSelected(lSeasonIndex))
            Next

            lstSeasons.ColumnWidth = lstSeasons.CreateGraphics().MeasureString("X", lstSeasons.Font).Width * (lMaxWidth + 1)
            lstSeasons.TopIndex = 0
            'Loop to check what was selected - removing this reveals a bug in the list control and it forgets what was selected
            For Each lSelectedIndex As Integer In lstSeasons.SelectedIndices
                Logger.Dbg("Selected " & lSelectedIndex & " = " & lstSeasons.Items(lSelectedIndex))
            Next
            lstSeasons.Refresh()
        Catch ex As Exception
            Logger.Dbg("Could not populate season list for '" & cboSeasons.Text & "': " & ex.ToString)
        End Try
    End Sub

    Private Function SelectedSeasonType() As Type
        Dim lSeasonPlugin As New atcSeasonPlugin
        For Each lSeasonType As Type In pSeasonsAvailable
            Dim lSeasonName As String = atcSeasonPlugin.SeasonClassNameToLabel(lSeasonType.Name)
            If lSeasonName.Equals(cboSeasons.Text) OrElse lSeasonName.Equals(cboSeasons.Text & "s") OrElse cboSeasons.Text.Equals(lSeasonName & "s") Then
                Return lSeasonType
            End If
        Next
        Return Nothing
    End Function

    Private Sub btnSeasonsAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSeasonsAll.Click
        For iItem As Integer = lstSeasons.Items.Count - 1 To 0 Step -1
            lstSeasons.SetSelected(iItem, True)
        Next
    End Sub

    Private Sub btnSeasonsNone_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSeasonsNone.Click
        For iItem As Integer = lstSeasons.Items.Count - 1 To 0 Step -1
            lstSeasons.SetSelected(iItem, False)
        Next
    End Sub

    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
        Try
            If VariationFromForm(pVariation) Then
                Me.DialogResult = Windows.Forms.DialogResult.OK
                Me.Close()
            End If
        Catch ex As Exception
            Logger.Msg(ex.Message, "Could not create variation")
        End Try
    End Sub

    Private Function VariationFromForm(ByVal aVariation As atcVariation) As Boolean
        Try
            If txtName.Text.Trim.Length = 0 Then
                Logger.Msg("Name was not entered", "Name is required")
                Return False
            End If
            With aVariation
                .Name = txtName.Text.Trim
                .Operation = cboAttribute.Text
                If chkSeasons.Checked Then
                    .Seasons = pSeasons
                    If Not pSeasons Is Nothing Then
                        For Each lSeasonIndex As Integer In pAllSeasons
                            pSeasons.SeasonSelected(lSeasonIndex) = lstSeasons.SelectedItems.Contains(pSeasons.SeasonName(lSeasonIndex))
                        Next
                    End If
                Else
                    .Seasons = Nothing
                End If

                If Not Double.TryParse(txtMin.Text, .Min) Then
                    .Min = GetNaN()
                End If

                If Not Double.TryParse(txtMax.Text, .Max) Then
                    .Max = GetNaN()
                End If

                .ColorAboveMax = txtHighColor.BackColor
                .ColorBelowMin = txtLowColor.BackColor
                .ColorDefault = txtDefaultColor.BackColor
            End With
        Catch ex As Exception
            Logger.Msg(ex.Message, "Could not create endpoint")
            Return False
        End Try
        Return True
    End Function

    Private Sub UserSelectColor(ByVal txt As Windows.Forms.TextBox)
        Dim cdlg As New Windows.Forms.ColorDialog
        cdlg.Color = txt.BackColor
        If cdlg.ShowDialog = Windows.Forms.DialogResult.OK Then
            txt.BackColor = cdlg.Color
            txt.Text = cdlg.Color.Name
        End If
    End Sub

    Private Sub txtDefaultColor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDefaultColor.Click
        UserSelectColor(txtDefaultColor)
    End Sub

    Private Sub txtDefaultColor_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtDefaultColor.KeyPress
        UserSelectColor(txtDefaultColor)
    End Sub

    Private Sub txtHighColor_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtHighColor.Click
        UserSelectColor(txtHighColor)
    End Sub

    Private Sub txtHighColor_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtHighColor.KeyPress
        UserSelectColor(txtHighColor)
    End Sub

    Private Sub txtLowColor_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtLowColor.Click
        UserSelectColor(txtLowColor)
    End Sub

    Private Sub txtLowColor_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtLowColor.KeyPress
        UserSelectColor(txtLowColor)
    End Sub

    Private Sub FormFromVariation()
        With pVariation
            txtName.Text = .Name
            txtOperation.Text = .Operation
            cboAttribute.Text = .Operation
            If Double.IsNaN(.Min) Then
                txtMin.Text = pNotNumberString
            Else
                txtMin.Text = CStr(.Min)
            End If
            If Double.IsNaN(.Max) Then
                txtMax.Text = pNotNumberString
            Else
                txtMax.Text = CStr(.Max)
            End If
            txtHighColor.BackColor = .ColorAboveMax
            txtHighColor.Text = .ColorAboveMax.Name

            txtLowColor.BackColor = .ColorBelowMin
            txtLowColor.Text = .ColorBelowMin.Name

            txtDefaultColor.BackColor = .ColorDefault
            txtDefaultColor.Text = .ColorDefault.Name

            UpdateDataText(txtData, pVariation.DataSets)
            If Not .IsInput Then
                If .Seasons Is Nothing Then
                    EnableSeasons(False)
                Else
                    pSettingFormSeason = True
                    EnableSeasons(True)
                    cboSeasons.Text = atcSeasonPlugin.SeasonClassNameToLabel(.Seasons.GetType.Name)
                    pSeasons = .Seasons
                    SetAllSeasons()
                    RefreshSeasonsList()
                    pSettingFormSeason = False
                End If
            End If
        End With
    End Sub

    Private Sub frmEndpoint_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyValue = Windows.Forms.Keys.F1 Then
            ShowHelp("BASINS Details\Analysis\Climate Assessment Tool.html")
        End If
    End Sub

    Private Sub chkSeasons_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkSeasons.CheckedChanged
        EnableSeasons(chkSeasons.Checked)
    End Sub

    Private Sub EnableSeasons(ByVal aEnable As Boolean)

        If chkSeasons.Checked <> aEnable Then chkSeasons.Checked = aEnable

        cboSeasons.Visible = aEnable
        lstSeasons.Visible = aEnable
        btnSeasonsAll.Visible = aEnable
        btnSeasonsNone.Visible = aEnable

        If aEnable Then
            cboSeasons_SelectedIndexChanged(Nothing, Nothing)
        Else
            pSeasons = Nothing
        End If
    End Sub

End Class
