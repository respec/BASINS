Imports atcControls
Imports atcData
Imports atcSeasons
Imports atcUtility
Imports MapWinUtility

Public Class frmVariationCligen
  Inherits System.Windows.Forms.Form

  Private Const AllSeasons As String = "All Seasons"

  Private pVariation As VariationCligen
  Private pSeasonsAvailable As New atcCollection
  Private pSeasons As atcSeasonBase
  Private pParmDescs As New atcCollection

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
  Friend WithEvents lblFunction As System.Windows.Forms.Label
  Friend WithEvents txtFunction As System.Windows.Forms.TextBox
  Friend WithEvents txtIncrement As System.Windows.Forms.TextBox
  Friend WithEvents lblIncrement As System.Windows.Forms.Label
  Friend WithEvents txtMax As System.Windows.Forms.TextBox
  Friend WithEvents txtMin As System.Windows.Forms.TextBox
  Friend WithEvents lblMaximum As System.Windows.Forms.Label
  Friend WithEvents lblMinimum As System.Windows.Forms.Label
  Friend WithEvents grpSeasons As System.Windows.Forms.GroupBox
  Friend WithEvents cboSeasons As System.Windows.Forms.ComboBox
  Friend WithEvents lstSeasons As System.Windows.Forms.ListBox
  Friend WithEvents btnSeasonsAll As System.Windows.Forms.Button
  Friend WithEvents btnSeasonsNone As System.Windows.Forms.Button
  Friend WithEvents btnOk As System.Windows.Forms.Button
  Friend WithEvents btnCancel As System.Windows.Forms.Button
  Friend WithEvents lblName As System.Windows.Forms.Label
  Friend WithEvents txtName As System.Windows.Forms.TextBox
  Friend WithEvents cboParameter As System.Windows.Forms.ComboBox
  Friend WithEvents agdCligenHSPF As atcControls.atcGrid
  Friend WithEvents lblParameterFilename As System.Windows.Forms.Label
  Friend WithEvents txtParameterFilename As System.Windows.Forms.TextBox
  Friend WithEvents Label2 As System.Windows.Forms.Label
  Friend WithEvents lblStartYear As System.Windows.Forms.Label
  Friend WithEvents txtStartYear As System.Windows.Forms.TextBox
  Friend WithEvents txtParameter As System.Windows.Forms.Label
  Friend WithEvents txtNumYears As System.Windows.Forms.TextBox
  Friend WithEvents lblParameterDescription As System.Windows.Forms.Label
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
    Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmVariationCligen))
    Me.lblFunction = New System.Windows.Forms.Label
    Me.txtFunction = New System.Windows.Forms.TextBox
    Me.txtIncrement = New System.Windows.Forms.TextBox
    Me.lblIncrement = New System.Windows.Forms.Label
    Me.txtMax = New System.Windows.Forms.TextBox
    Me.txtMin = New System.Windows.Forms.TextBox
    Me.lblMaximum = New System.Windows.Forms.Label
    Me.lblMinimum = New System.Windows.Forms.Label
    Me.grpSeasons = New System.Windows.Forms.GroupBox
    Me.cboSeasons = New System.Windows.Forms.ComboBox
    Me.lstSeasons = New System.Windows.Forms.ListBox
    Me.btnSeasonsAll = New System.Windows.Forms.Button
    Me.btnSeasonsNone = New System.Windows.Forms.Button
    Me.btnOk = New System.Windows.Forms.Button
    Me.btnCancel = New System.Windows.Forms.Button
    Me.lblName = New System.Windows.Forms.Label
    Me.txtName = New System.Windows.Forms.TextBox
    Me.txtParameter = New System.Windows.Forms.Label
    Me.cboParameter = New System.Windows.Forms.ComboBox
    Me.agdCligenHSPF = New atcControls.atcGrid
    Me.lblParameterFilename = New System.Windows.Forms.Label
    Me.txtParameterFilename = New System.Windows.Forms.TextBox
    Me.txtNumYears = New System.Windows.Forms.TextBox
    Me.txtStartYear = New System.Windows.Forms.TextBox
    Me.Label2 = New System.Windows.Forms.Label
    Me.lblStartYear = New System.Windows.Forms.Label
    Me.lblParameterDescription = New System.Windows.Forms.Label
    Me.grpSeasons.SuspendLayout()
    Me.SuspendLayout()
    '
    'lblFunction
    '
    Me.lblFunction.BackColor = System.Drawing.Color.Transparent
    Me.lblFunction.Location = New System.Drawing.Point(24, 112)
    Me.lblFunction.Name = "lblFunction"
    Me.lblFunction.Size = New System.Drawing.Size(64, 18)
    Me.lblFunction.TabIndex = 5
    Me.lblFunction.Text = "Function:"
    Me.lblFunction.TextAlign = System.Drawing.ContentAlignment.MiddleRight
    '
    'txtFunction
    '
    Me.txtFunction.Location = New System.Drawing.Point(96, 112)
    Me.txtFunction.Name = "txtFunction"
    Me.txtFunction.Size = New System.Drawing.Size(71, 20)
    Me.txtFunction.TabIndex = 6
    Me.txtFunction.Text = "Multiply"
    '
    'txtIncrement
    '
    Me.txtIncrement.Location = New System.Drawing.Point(96, 184)
    Me.txtIncrement.Name = "txtIncrement"
    Me.txtIncrement.Size = New System.Drawing.Size(71, 20)
    Me.txtIncrement.TabIndex = 12
    Me.txtIncrement.Text = "0.05"
    '
    'lblIncrement
    '
    Me.lblIncrement.BackColor = System.Drawing.Color.Transparent
    Me.lblIncrement.ImageAlign = System.Drawing.ContentAlignment.BottomRight
    Me.lblIncrement.Location = New System.Drawing.Point(24, 184)
    Me.lblIncrement.Name = "lblIncrement"
    Me.lblIncrement.Size = New System.Drawing.Size(64, 17)
    Me.lblIncrement.TabIndex = 11
    Me.lblIncrement.Text = "Increment:"
    Me.lblIncrement.TextAlign = System.Drawing.ContentAlignment.MiddleRight
    '
    'txtMax
    '
    Me.txtMax.Location = New System.Drawing.Point(96, 160)
    Me.txtMax.Name = "txtMax"
    Me.txtMax.Size = New System.Drawing.Size(71, 20)
    Me.txtMax.TabIndex = 10
    Me.txtMax.Text = "1.1"
    '
    'txtMin
    '
    Me.txtMin.Location = New System.Drawing.Point(96, 136)
    Me.txtMin.Name = "txtMin"
    Me.txtMin.Size = New System.Drawing.Size(71, 20)
    Me.txtMin.TabIndex = 8
    Me.txtMin.Text = "0.9"
    '
    'lblMaximum
    '
    Me.lblMaximum.BackColor = System.Drawing.Color.Transparent
    Me.lblMaximum.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
    Me.lblMaximum.Location = New System.Drawing.Point(24, 160)
    Me.lblMaximum.Name = "lblMaximum"
    Me.lblMaximum.Size = New System.Drawing.Size(64, 18)
    Me.lblMaximum.TabIndex = 9
    Me.lblMaximum.Text = "Maximum:"
    Me.lblMaximum.TextAlign = System.Drawing.ContentAlignment.MiddleRight
    '
    'lblMinimum
    '
    Me.lblMinimum.BackColor = System.Drawing.Color.Transparent
    Me.lblMinimum.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
    Me.lblMinimum.Location = New System.Drawing.Point(24, 136)
    Me.lblMinimum.Name = "lblMinimum"
    Me.lblMinimum.Size = New System.Drawing.Size(64, 18)
    Me.lblMinimum.TabIndex = 7
    Me.lblMinimum.Text = "Minimum:"
    Me.lblMinimum.TextAlign = System.Drawing.ContentAlignment.MiddleRight
    '
    'grpSeasons
    '
    Me.grpSeasons.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.grpSeasons.Controls.Add(Me.cboSeasons)
    Me.grpSeasons.Controls.Add(Me.lstSeasons)
    Me.grpSeasons.Controls.Add(Me.btnSeasonsAll)
    Me.grpSeasons.Controls.Add(Me.btnSeasonsNone)
    Me.grpSeasons.Location = New System.Drawing.Point(176, 104)
    Me.grpSeasons.Name = "grpSeasons"
    Me.grpSeasons.Size = New System.Drawing.Size(162, 168)
    Me.grpSeasons.TabIndex = 13
    Me.grpSeasons.TabStop = False
    Me.grpSeasons.Text = "Vary Seasonally"
    '
    'cboSeasons
    '
    Me.cboSeasons.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.cboSeasons.ItemHeight = 13
    Me.cboSeasons.Location = New System.Drawing.Point(16, 24)
    Me.cboSeasons.MaxDropDownItems = 20
    Me.cboSeasons.Name = "cboSeasons"
    Me.cboSeasons.Size = New System.Drawing.Size(138, 21)
    Me.cboSeasons.TabIndex = 14
    '
    'lstSeasons
    '
    Me.lstSeasons.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lstSeasons.IntegralHeight = False
    Me.lstSeasons.Location = New System.Drawing.Point(16, 48)
    Me.lstSeasons.Name = "lstSeasons"
    Me.lstSeasons.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple
    Me.lstSeasons.Size = New System.Drawing.Size(138, 72)
    Me.lstSeasons.TabIndex = 15
    '
    'btnSeasonsAll
    '
    Me.btnSeasonsAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
    Me.btnSeasonsAll.Location = New System.Drawing.Point(16, 128)
    Me.btnSeasonsAll.Name = "btnSeasonsAll"
    Me.btnSeasonsAll.Size = New System.Drawing.Size(63, 23)
    Me.btnSeasonsAll.TabIndex = 16
    Me.btnSeasonsAll.Text = "All"
    '
    'btnSeasonsNone
    '
    Me.btnSeasonsNone.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.btnSeasonsNone.Location = New System.Drawing.Point(90, 128)
    Me.btnSeasonsNone.Name = "btnSeasonsNone"
    Me.btnSeasonsNone.Size = New System.Drawing.Size(64, 22)
    Me.btnSeasonsNone.TabIndex = 17
    Me.btnSeasonsNone.Text = "None"
    '
    'btnOk
    '
    Me.btnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
    Me.btnOk.Location = New System.Drawing.Point(96, 460)
    Me.btnOk.Name = "btnOk"
    Me.btnOk.Size = New System.Drawing.Size(72, 24)
    Me.btnOk.TabIndex = 18
    Me.btnOk.Text = "Ok"
    '
    'btnCancel
    '
    Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
    Me.btnCancel.Location = New System.Drawing.Point(184, 460)
    Me.btnCancel.Name = "btnCancel"
    Me.btnCancel.Size = New System.Drawing.Size(72, 24)
    Me.btnCancel.TabIndex = 19
    Me.btnCancel.Text = "Cancel"
    '
    'lblName
    '
    Me.lblName.BackColor = System.Drawing.Color.Transparent
    Me.lblName.Location = New System.Drawing.Point(0, 12)
    Me.lblName.Name = "lblName"
    Me.lblName.Size = New System.Drawing.Size(112, 18)
    Me.lblName.TabIndex = 1
    Me.lblName.Text = "Input Name:"
    Me.lblName.TextAlign = System.Drawing.ContentAlignment.TopRight
    '
    'txtName
    '
    Me.txtName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtName.Location = New System.Drawing.Point(120, 8)
    Me.txtName.Name = "txtName"
    Me.txtName.Size = New System.Drawing.Size(215, 20)
    Me.txtName.TabIndex = 2
    Me.txtName.Text = ""
    '
    'txtParameter
    '
    Me.txtParameter.BackColor = System.Drawing.Color.Transparent
    Me.txtParameter.Location = New System.Drawing.Point(0, 60)
    Me.txtParameter.Name = "txtParameter"
    Me.txtParameter.Size = New System.Drawing.Size(112, 18)
    Me.txtParameter.TabIndex = 21
    Me.txtParameter.Text = "Parameter to Vary:"
    Me.txtParameter.TextAlign = System.Drawing.ContentAlignment.TopRight
    '
    'cboParameter
    '
    Me.cboParameter.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.cboParameter.ItemHeight = 13
    Me.cboParameter.Location = New System.Drawing.Point(120, 56)
    Me.cboParameter.MaxDropDownItems = 20
    Me.cboParameter.Name = "cboParameter"
    Me.cboParameter.Size = New System.Drawing.Size(215, 21)
    Me.cboParameter.TabIndex = 22
    '
    'agdCligenHSPF
    '
    Me.agdCligenHSPF.AllowHorizontalScrolling = False
    Me.agdCligenHSPF.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.agdCligenHSPF.CellBackColor = System.Drawing.Color.Empty
    Me.agdCligenHSPF.LineColor = System.Drawing.Color.Empty
    Me.agdCligenHSPF.LineWidth = 0.0!
    Me.agdCligenHSPF.Location = New System.Drawing.Point(8, 288)
    Me.agdCligenHSPF.Name = "agdCligenHSPF"
    Me.agdCligenHSPF.Size = New System.Drawing.Size(335, 160)
    Me.agdCligenHSPF.Source = Nothing
    Me.agdCligenHSPF.TabIndex = 26
    '
    'lblParameterFilename
    '
    Me.lblParameterFilename.BackColor = System.Drawing.Color.Transparent
    Me.lblParameterFilename.Location = New System.Drawing.Point(16, 36)
    Me.lblParameterFilename.Name = "lblParameterFilename"
    Me.lblParameterFilename.Size = New System.Drawing.Size(96, 18)
    Me.lblParameterFilename.TabIndex = 27
    Me.lblParameterFilename.Text = "Parameter File:"
    Me.lblParameterFilename.TextAlign = System.Drawing.ContentAlignment.TopRight
    '
    'txtParameterFilename
    '
    Me.txtParameterFilename.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtParameterFilename.Location = New System.Drawing.Point(120, 32)
    Me.txtParameterFilename.Name = "txtParameterFilename"
    Me.txtParameterFilename.Size = New System.Drawing.Size(215, 20)
    Me.txtParameterFilename.TabIndex = 28
    Me.txtParameterFilename.Text = "<click to select>"
    '
    'txtNumYears
    '
    Me.txtNumYears.Location = New System.Drawing.Point(96, 232)
    Me.txtNumYears.Name = "txtNumYears"
    Me.txtNumYears.Size = New System.Drawing.Size(71, 20)
    Me.txtNumYears.TabIndex = 32
    Me.txtNumYears.Text = "1"
    '
    'txtStartYear
    '
    Me.txtStartYear.Location = New System.Drawing.Point(96, 208)
    Me.txtStartYear.Name = "txtStartYear"
    Me.txtStartYear.Size = New System.Drawing.Size(71, 20)
    Me.txtStartYear.TabIndex = 30
    Me.txtStartYear.Text = "1985"
    '
    'Label2
    '
    Me.Label2.BackColor = System.Drawing.Color.Transparent
    Me.Label2.ImageAlign = System.Drawing.ContentAlignment.BottomRight
    Me.Label2.Location = New System.Drawing.Point(24, 232)
    Me.Label2.Name = "Label2"
    Me.Label2.Size = New System.Drawing.Size(64, 17)
    Me.Label2.TabIndex = 31
    Me.Label2.Text = "Num Years:"
    Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
    '
    'lblStartYear
    '
    Me.lblStartYear.BackColor = System.Drawing.Color.Transparent
    Me.lblStartYear.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
    Me.lblStartYear.Location = New System.Drawing.Point(24, 208)
    Me.lblStartYear.Name = "lblStartYear"
    Me.lblStartYear.Size = New System.Drawing.Size(64, 18)
    Me.lblStartYear.TabIndex = 29
    Me.lblStartYear.Text = "Start Year:"
    Me.lblStartYear.TextAlign = System.Drawing.ContentAlignment.MiddleRight
    '
    'lblParameterDescription
    '
    Me.lblParameterDescription.BackColor = System.Drawing.Color.Transparent
    Me.lblParameterDescription.Location = New System.Drawing.Point(121, 80)
    Me.lblParameterDescription.Name = "lblParameterDescription"
    Me.lblParameterDescription.Size = New System.Drawing.Size(215, 16)
    Me.lblParameterDescription.TabIndex = 33
    Me.lblParameterDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
    '
    'frmVariationCligen
    '
    Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
    Me.ClientSize = New System.Drawing.Size(352, 493)
    Me.Controls.Add(Me.txtNumYears)
    Me.Controls.Add(Me.txtStartYear)
    Me.Controls.Add(Me.txtParameterFilename)
    Me.Controls.Add(Me.txtName)
    Me.Controls.Add(Me.txtFunction)
    Me.Controls.Add(Me.txtIncrement)
    Me.Controls.Add(Me.txtMax)
    Me.Controls.Add(Me.txtMin)
    Me.Controls.Add(Me.Label2)
    Me.Controls.Add(Me.lblStartYear)
    Me.Controls.Add(Me.lblParameterFilename)
    Me.Controls.Add(Me.cboParameter)
    Me.Controls.Add(Me.txtParameter)
    Me.Controls.Add(Me.lblName)
    Me.Controls.Add(Me.btnCancel)
    Me.Controls.Add(Me.btnOk)
    Me.Controls.Add(Me.grpSeasons)
    Me.Controls.Add(Me.lblFunction)
    Me.Controls.Add(Me.lblIncrement)
    Me.Controls.Add(Me.lblMaximum)
    Me.Controls.Add(Me.lblMinimum)
    Me.Controls.Add(Me.agdCligenHSPF)
    Me.Controls.Add(Me.lblParameterDescription)
    Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
    Me.Name = "frmVariationCligen"
    Me.Text = "Generate Input With Cligen"
    Me.grpSeasons.ResumeLayout(False)
    Me.ResumeLayout(False)

  End Sub

#End Region

  Public Function AskUser(Optional ByVal aVariation As VariationCligen = Nothing) As VariationCligen
    If aVariation Is Nothing Then
      pVariation = New VariationCligen
    Else
      pVariation = aVariation.Clone
    End If

    FindAllCligenParameters()

    cboSeasons.Items.Add(AllSeasons)
    pSeasonsAvailable = atcSeasonPlugin.AllSeasonTypes
    For Each lSeasonType As Type In pSeasonsAvailable
      Dim lSeasonTypeShortName As String = atcSeasonPlugin.SeasonClassNameToLabel(lSeasonType.Name)
      If lSeasonTypeShortName.Equals("Month") Then
        cboSeasons.Items.Add(lSeasonTypeShortName)
      End If
    Next

    FormFromVariation()

    If Me.ShowDialog() = Windows.Forms.DialogResult.OK Then
      Return pVariation
    Else
      Return Nothing
    End If
  End Function

  Private Sub UserSelectData(ByVal aCligenConstituent As String)
    Dim lReplaceIndex As Integer = pVariation.DataSets.IndexFromKey(aCligenConstituent)
    Dim lDataSet As atcDataSet = pVariation.DataSets.Item(lReplaceIndex)
    Dim lData As New atcDataGroup
    If Not lDataSet Is Nothing Then lData.Add(lDataSet)
    lData = g_DataManager.UserSelectData("Select data to replace with Cligen " & aCligenConstituent, lData)
    If Not lData Is Nothing AndAlso lData.Count > 0 Then
      pVariation.DataSets.Item(lReplaceIndex) = lData.Item(0)
      UpdateGrid()
    End If
  End Sub

  Private Sub txtFunction_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtFunction.Click
    Dim aCategory As New ArrayList(1)
    aCategory.Add("Compute")
    pVariation.ComputationSource = g_DataManager.UserSelectDataSource(aCategory, "Select Function for Varying Input Data")
    If pVariation.ComputationSource Is Nothing Then
      txtFunction.Text = "<click to specify>"
    Else
      txtFunction.Text = pVariation.ComputationSource.ToString
    End If
  End Sub

  Private Sub cboSeasons_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboSeasons.SelectedIndexChanged
    If Not pSettingFormSeason Then
      pSeasons = Nothing
      lstSeasons.Items.Clear()
      If cboSeasons.Text <> AllSeasons Then
        Try
          pSeasons = SelectedSeasonType.InvokeMember(Nothing, Reflection.BindingFlags.CreateInstance, Nothing, Nothing, New Object() {})
          For Each lSeasonIndex As Integer In pSeasons.AllSeasons
            pSeasons.SeasonSelected(lSeasonIndex) = True
          Next
          RefreshSeasonsList()
        Catch ex As Exception
          Logger.Dbg("Could not create new seasons for '" & cboSeasons.Text & "': " & ex.ToString)
        End Try
      End If
    End If
  End Sub

  Private Sub RefreshSeasonsList()
    Try
      lstSeasons.Items.Clear()
      For Each lSeasonIndex As Integer In pSeasons.AllSeasons
        lstSeasons.Items.Add(pSeasons.SeasonName(lSeasonIndex))
        lstSeasons.SetSelected(lstSeasons.Items.Count - 1, pSeasons.SeasonSelected(lSeasonIndex))
      Next
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
      If atcSeasonPlugin.SeasonClassNameToLabel(lSeasonType.Name).Equals(cboSeasons.Text) Then
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

  Private Function VariationFromForm(ByVal aVariation As VariationCligen) As Boolean
    With aVariation
      .ParmToVary = cboParameter.Text
      .BaseParmFileName = txtParameterFilename.Text
      .StartYear = txtStartYear.Text
      .NumYears = txtNumYears.Text

      .Name = txtName.Text
      .Operation = txtFunction.Text
      .Seasons = pSeasons
      If Not pSeasons Is Nothing Then
        For lListIndex As Integer = 0 To lstSeasons.Items.Count - 1
          Dim lSeasonName As String = lstSeasons.Items(lListIndex)
          For Each lSeasonIndex As Integer In pSeasons.AllSeasons
            If pSeasons.SeasonName(lSeasonIndex) = lSeasonName Then
              pSeasons.SeasonSelected(lSeasonIndex) = lstSeasons.SelectedIndices.Contains(lListIndex)
              Exit For
            End If
          Next
        Next
      End If

      Try
        .Min = CDbl(txtMin.Text)
      Catch
        Logger.Msg("Minimum value must be a number", "Non-numeric value")
        Return False
      End Try
      Try
        .Max = CDbl(txtMax.Text)
      Catch
        Logger.Msg("Maximum value must be a number", "Non-numeric value")
        Return False
      End Try
      Try
        .Increment = CDbl(txtIncrement.Text)
      Catch
        Logger.Msg("Increment must be a number", "Non-numeric value")
        Return False
      End Try
    End With
    Return True
  End Function

  Private Sub FormFromVariation()
    With pVariation
      txtParameterFilename.Text = .BaseParmFileName
      cboParameter.Text = .ParmToVary
      txtStartYear.Text = .StartYear
      txtNumYears.Text = .NumYears

      txtName.Text = .Name
      txtFunction.Text = .Operation
      If Not Double.IsNaN(.Min) Then txtMin.Text = .Min
      If Not Double.IsNaN(.Max) Then txtMax.Text = .Max
      If Not Double.IsNaN(.Increment) Then txtIncrement.Text = .Increment
      UpdateGrid()
      If .Seasons Is Nothing Then
        cboSeasons.SelectedIndex = 0
      Else
        pSettingFormSeason = True
        cboSeasons.Text = atcSeasonPlugin.SeasonClassNameToLabel(.Seasons.GetType.Name)
        pSeasons = .Seasons
        RefreshSeasonsList()
        pSettingFormSeason = False
      End If
    End With
  End Sub

  Private Sub UpdateGrid()
    Dim lSource As New atcGridSource
    With lSource
      .FixedRows = 1
      .Rows = VariationCligen.CligenConstituents.Length + 1
      .Columns = 2
      .CellValue(0, 0) = "Cligen Parameter"
      .CellValue(0, 1) = "HSPF Input to Replace"
      .CellColor(0, 0) = System.Drawing.SystemColors.Control
      .CellColor(0, 1) = System.Drawing.SystemColors.Control
      .ColorCells = True
      For lRow As Integer = 1 To .Rows - 1
        Dim lCligenParm As String = VariationCligen.CligenConstituents(lRow - 1)
        Dim lDataset As atcDataSet = pVariation.DataSets.ItemByKey(lCligenParm)
        .CellValue(lRow, 0) = lCligenParm
        If lDataset Is Nothing Then
          .CellValue(lRow, 1) = "<click to select data>"
        Else
          .CellValue(lRow, 1) = lDataset.ToString
        End If
      Next
    End With
    agdCligenHSPF.Clear()
    agdCligenHSPF.Source = lSource
    agdCligenHSPF.SizeAllColumnsToContents()
    agdCligenHSPF.Refresh()
  End Sub

  Private Sub agdCligenHSPF_MouseDownCell(ByVal aGrid As atcControls.atcGrid, ByVal aRow As Integer, ByVal aColumn As Integer) Handles agdCligenHSPF.MouseDownCell
    If aRow > 0 Then UserSelectData(agdCligenHSPF.Source.CellValue(aRow, 0))
  End Sub

  Private Sub FindAllCligenParameters()
    Dim lFileName As String = FindFile("Locate Cligen parameter list", "CliGenEdit.prm", "*.prm", "Cligen Parameter List (*.prm)|*.prm|All Files|*.*")
    If lFileName.Length > 0 Then
      Dim lStr As String = WholeFileString(lFileName)
      Dim lParm As String
      Dim lDesc As String = ""
      Dim lPos As Integer
      cboParameter.Items.Clear()
      While lStr.Length > 0
        lParm = StrSplit(lStr, vbCrLf, "")
        lPos = lParm.IndexOf("'")
        If lPos > 0 Then 'description exists for this parm
          lDesc = Trim(lParm.Substring(lPos + 1))
          lParm = Trim(lParm.Substring(0, lPos))
        End If
        If lParm.Chars(0) = "#" Then 'not currently editing this parm
          lParm = lParm.TrimStart("#")
        Else 'TODO: move this out of Else to allow varying hidden parameters
          cboParameter.Items.Add(lParm)
        End If
        If lDesc.Length > 0 Then pParmDescs.Add(lParm, lDesc)
      End While
    End If
  End Sub

  Private Sub SelectCligenParameterFile()
    Dim lFilename As String = FindFile("Select Cligen Parameter file to open", , , _
                                       "Cligen Parameter Files (*.par)|*.par|All Files|*.*", True, , 1)
    If Len(lFilename) > 0 Then
      txtParameterFilename.Text = lFilename
    End If
    UpdateGrid()
  End Sub

  Private Sub txtParameterFilename_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles txtParameterFilename.MouseDown
    SelectCligenParameterFile()
  End Sub

  Private Sub txtParameterFilename_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtParameterFilename.KeyPress
    SelectCligenParameterFile()
  End Sub

  Private Sub cboParameter_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboParameter.SelectedIndexChanged
    Dim lParmDescIndex As Integer = pParmDescs.IndexFromKey(cboParameter.Text)
    If lParmDescIndex < 0 Then
      lblParameterDescription.Text = ""
    Else
      lblParameterDescription.Text = pParmDescs.Item(lParmDescIndex)
    End If
  End Sub

End Class
