Imports atcData
Imports MapWinUtility

Public Class frmEndpoint
  Inherits System.Windows.Forms.Form

  Private pNotNumberString As String = "<none>"
  Private pVariation As Variation

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
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
    Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmEndpoint))
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
    Me.GroupBox1.SuspendLayout()
    Me.SuspendLayout()
    '
    'lblName
    '
    Me.lblName.BackColor = System.Drawing.Color.Transparent
    Me.lblName.Location = New System.Drawing.Point(8, 16)
    Me.lblName.Name = "lblName"
    Me.lblName.Size = New System.Drawing.Size(96, 17)
    Me.lblName.TabIndex = 1
    Me.lblName.Text = "Endpoint Name:"
    Me.lblName.TextAlign = System.Drawing.ContentAlignment.TopRight
    '
    'txtName
    '
    Me.txtName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtName.Location = New System.Drawing.Point(112, 16)
    Me.txtName.Name = "txtName"
    Me.txtName.Size = New System.Drawing.Size(161, 20)
    Me.txtName.TabIndex = 2
    Me.txtName.Text = ""
    '
    'btnCancel
    '
    Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
    Me.btnCancel.Location = New System.Drawing.Point(160, 280)
    Me.btnCancel.Name = "btnCancel"
    Me.btnCancel.Size = New System.Drawing.Size(72, 24)
    Me.btnCancel.TabIndex = 19
    Me.btnCancel.Text = "Cancel"
    '
    'btnOk
    '
    Me.btnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
    Me.btnOk.Location = New System.Drawing.Point(56, 280)
    Me.btnOk.Name = "btnOk"
    Me.btnOk.Size = New System.Drawing.Size(72, 24)
    Me.btnOk.TabIndex = 18
    Me.btnOk.Text = "Ok"
    '
    'lblAttribute
    '
    Me.lblAttribute.BackColor = System.Drawing.Color.Transparent
    Me.lblAttribute.Location = New System.Drawing.Point(8, 64)
    Me.lblAttribute.Name = "lblAttribute"
    Me.lblAttribute.Size = New System.Drawing.Size(96, 17)
    Me.lblAttribute.TabIndex = 5
    Me.lblAttribute.Text = "Attribute:"
    Me.lblAttribute.TextAlign = System.Drawing.ContentAlignment.MiddleRight
    '
    'lblData
    '
    Me.lblData.BackColor = System.Drawing.Color.Transparent
    Me.lblData.Location = New System.Drawing.Point(8, 40)
    Me.lblData.Name = "lblData"
    Me.lblData.Size = New System.Drawing.Size(96, 17)
    Me.lblData.TabIndex = 3
    Me.lblData.Text = "Data set:"
    Me.lblData.TextAlign = System.Drawing.ContentAlignment.TopRight
    '
    'txtData
    '
    Me.txtData.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtData.Location = New System.Drawing.Point(112, 40)
    Me.txtData.Name = "txtData"
    Me.txtData.Size = New System.Drawing.Size(161, 20)
    Me.txtData.TabIndex = 4
    Me.txtData.Text = ""
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
    Me.GroupBox1.Location = New System.Drawing.Point(16, 96)
    Me.GroupBox1.Name = "GroupBox1"
    Me.GroupBox1.Size = New System.Drawing.Size(256, 160)
    Me.GroupBox1.TabIndex = 7
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
    Me.txtHighColor.Size = New System.Drawing.Size(96, 20)
    Me.txtHighColor.TabIndex = 17
    Me.txtHighColor.Text = ""
    '
    'lblHighColor
    '
    Me.lblHighColor.BackColor = System.Drawing.Color.Transparent
    Me.lblHighColor.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
    Me.lblHighColor.Location = New System.Drawing.Point(16, 120)
    Me.lblHighColor.Name = "lblHighColor"
    Me.lblHighColor.Size = New System.Drawing.Size(120, 17)
    Me.lblHighColor.TabIndex = 16
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
    Me.txtLowColor.Size = New System.Drawing.Size(96, 20)
    Me.txtLowColor.TabIndex = 13
    Me.txtLowColor.Text = ""
    '
    'lblLowColor
    '
    Me.lblLowColor.BackColor = System.Drawing.Color.Transparent
    Me.lblLowColor.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
    Me.lblLowColor.Location = New System.Drawing.Point(16, 72)
    Me.lblLowColor.Name = "lblLowColor"
    Me.lblLowColor.Size = New System.Drawing.Size(120, 17)
    Me.lblLowColor.TabIndex = 12
    Me.lblLowColor.Text = "Color Lower Values:"
    Me.lblLowColor.TextAlign = System.Drawing.ContentAlignment.MiddleRight
    '
    'txtMax
    '
    Me.txtMax.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtMax.Location = New System.Drawing.Point(144, 96)
    Me.txtMax.Name = "txtMax"
    Me.txtMax.Size = New System.Drawing.Size(96, 20)
    Me.txtMax.TabIndex = 11
    Me.txtMax.Text = "<none>"
    '
    'txtMin
    '
    Me.txtMin.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtMin.Location = New System.Drawing.Point(144, 48)
    Me.txtMin.Name = "txtMin"
    Me.txtMin.Size = New System.Drawing.Size(96, 20)
    Me.txtMin.TabIndex = 9
    Me.txtMin.Text = "<none>"
    '
    'lblMaximum
    '
    Me.lblMaximum.BackColor = System.Drawing.Color.Transparent
    Me.lblMaximum.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
    Me.lblMaximum.Location = New System.Drawing.Point(16, 96)
    Me.lblMaximum.Name = "lblMaximum"
    Me.lblMaximum.Size = New System.Drawing.Size(120, 17)
    Me.lblMaximum.TabIndex = 10
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
    Me.lblMinimum.TabIndex = 8
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
    Me.txtDefaultColor.Size = New System.Drawing.Size(96, 20)
    Me.txtDefaultColor.TabIndex = 15
    Me.txtDefaultColor.Text = ""
    '
    'lblDefaultColor
    '
    Me.lblDefaultColor.BackColor = System.Drawing.Color.Transparent
    Me.lblDefaultColor.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
    Me.lblDefaultColor.Location = New System.Drawing.Point(16, 24)
    Me.lblDefaultColor.Name = "lblDefaultColor"
    Me.lblDefaultColor.Size = New System.Drawing.Size(120, 17)
    Me.lblDefaultColor.TabIndex = 14
    Me.lblDefaultColor.Text = "Default Color:"
    Me.lblDefaultColor.TextAlign = System.Drawing.ContentAlignment.MiddleRight
    '
    'cboAttribute
    '
    Me.cboAttribute.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.cboAttribute.Location = New System.Drawing.Point(112, 64)
    Me.cboAttribute.MaxDropDownItems = 20
    Me.cboAttribute.Name = "cboAttribute"
    Me.cboAttribute.Size = New System.Drawing.Size(161, 21)
    Me.cboAttribute.TabIndex = 6
    Me.cboAttribute.Text = "ComboBox1"
    '
    'frmEndpoint
    '
    Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
    Me.ClientSize = New System.Drawing.Size(296, 325)
    Me.Controls.Add(Me.cboAttribute)
    Me.Controls.Add(Me.GroupBox1)
    Me.Controls.Add(Me.lblName)
    Me.Controls.Add(Me.txtName)
    Me.Controls.Add(Me.txtData)
    Me.Controls.Add(Me.btnCancel)
    Me.Controls.Add(Me.btnOk)
    Me.Controls.Add(Me.lblAttribute)
    Me.Controls.Add(Me.lblData)
    Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
    Me.Name = "frmEndpoint"
    Me.Text = "Endpoint"
    Me.GroupBox1.ResumeLayout(False)
    Me.ResumeLayout(False)

  End Sub

#End Region

  Public Function AskUser(Optional ByVal aVariation As Variation = Nothing) As Variation
    pVariation = aVariation.Clone
    If pVariation Is Nothing Then pVariation = New Variation
    If pVariation.DataSets Is Nothing Then pVariation.DataSets = New atcDataGroup

    For Each lAttribute As atcAttributeDefinition In atcDataAttributes.AllDefinitions
      If atcDataAttributes.IsSimple(lAttribute) Then
        cboAttribute.Items.Add(lAttribute.Name)
      End If
    Next

    With pVariation
      txtName.Text = .Name
      UpdateDataText(txtData, pVariation.DataSets)
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
    End With

    'pSeasonsAvailable = pSeasonalPlugin.AvailableOperations(True, False)
    'cboSeasons.Items.Add("No seasons")
    'For Each lSeason As atcDefinedValue In pSeasonsAvailable
    '  cboSeasons.Items.Add(lSeason.Definition.Name.Substring(0, lSeason.Definition.Name.IndexOf("::")))
    'Next

    Me.ShowDialog()

    Return pVariation

  End Function

  Private Sub txtData_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtData.Click
    UserSelectDataToVary()
  End Sub

  Private Sub txtData_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtData.KeyPress
    UserSelectDataToVary()
  End Sub

  Private Sub UserSelectDataToVary()
    Dim lData As atcDataGroup = g_DataManager.UserSelectData("Select data to vary", pVariation.DataSets)
    If Not lData Is Nothing Then
      pVariation.DataSets = lData
      UpdateDataText(txtData, lData)
    End If
  End Sub

  Private Sub UpdateDataText(ByVal aTextBox As Windows.Forms.TextBox, _
                             ByVal aGroup As atcDataGroup)
    If aGroup.Count > 0 Then
      aTextBox.Text = aGroup.ItemByIndex(0).ToString
      If aGroup.Count > 1 Then aTextBox.Text &= " (and " & aGroup.Count - 1 & " more)"
    Else
      aTextBox.Text = "<click to select data>"
    End If
  End Sub

  Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
    pVariation = Nothing
    Me.Close()
  End Sub

  Private Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
    Try
      With pVariation
        .Name = txtName.Text
        .Operation = cboAttribute.Text
        Try
          .Min = CDbl(txtMin.Text)
        Catch
          .Min = Double.NaN
        End Try
        Try
          .Max = CDbl(txtMax.Text)
        Catch
          .Max = Double.NaN
        End Try
        .ColorAboveMax = txtHighColor.BackColor
        .ColorBelowMin = txtLowColor.BackColor
        .ColorDefault = txtDefaultColor.BackColor
      End With
      Me.Close()
    Catch ex As Exception
      Logger.Msg(ex.Message, "Could not create endpoint")
    End Try
  End Sub

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
End Class
