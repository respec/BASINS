Imports atcData
Imports atcUtility

Public Class frmSpecifySeasonalAttributes
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
  Friend WithEvents grpAttributes As System.Windows.Forms.GroupBox
  Friend WithEvents btnAttributesNone As System.Windows.Forms.Button
  Friend WithEvents btnAttributesAll As System.Windows.Forms.Button
  Friend WithEvents lstAttributes As System.Windows.Forms.ListBox
  Friend WithEvents Splitter1 As System.Windows.Forms.Splitter
  Friend WithEvents grpSeasons As System.Windows.Forms.GroupBox
  Friend WithEvents lstSeasons As System.Windows.Forms.ListBox
  Friend WithEvents cboSeasons As System.Windows.Forms.ComboBox
  Friend WithEvents btnSeasonsNone As System.Windows.Forms.Button
  Friend WithEvents btnSeasonsAll As System.Windows.Forms.Button
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
    Me.grpAttributes = New System.Windows.Forms.GroupBox
    Me.btnAttributesNone = New System.Windows.Forms.Button
    Me.btnAttributesAll = New System.Windows.Forms.Button
    Me.lstAttributes = New System.Windows.Forms.ListBox
    Me.Splitter1 = New System.Windows.Forms.Splitter
    Me.grpSeasons = New System.Windows.Forms.GroupBox
    Me.btnSeasonsNone = New System.Windows.Forms.Button
    Me.btnSeasonsAll = New System.Windows.Forms.Button
    Me.lstSeasons = New System.Windows.Forms.ListBox
    Me.cboSeasons = New System.Windows.Forms.ComboBox
    Me.grpAttributes.SuspendLayout()
    Me.grpSeasons.SuspendLayout()
    Me.SuspendLayout()
    '
    'grpAttributes
    '
    Me.grpAttributes.Controls.Add(Me.btnAttributesNone)
    Me.grpAttributes.Controls.Add(Me.btnAttributesAll)
    Me.grpAttributes.Controls.Add(Me.lstAttributes)
    Me.grpAttributes.Dock = System.Windows.Forms.DockStyle.Left
    Me.grpAttributes.Location = New System.Drawing.Point(0, 0)
    Me.grpAttributes.Name = "grpAttributes"
    Me.grpAttributes.Size = New System.Drawing.Size(200, 301)
    Me.grpAttributes.TabIndex = 9
    Me.grpAttributes.TabStop = False
    Me.grpAttributes.Text = "Attributes"
    '
    'btnAttributesNone
    '
    Me.btnAttributesNone.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.btnAttributesNone.Location = New System.Drawing.Point(128, 264)
    Me.btnAttributesNone.Name = "btnAttributesNone"
    Me.btnAttributesNone.Size = New System.Drawing.Size(64, 23)
    Me.btnAttributesNone.TabIndex = 10
    Me.btnAttributesNone.Text = "None"
    '
    'btnAttributesAll
    '
    Me.btnAttributesAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
    Me.btnAttributesAll.Location = New System.Drawing.Point(8, 264)
    Me.btnAttributesAll.Name = "btnAttributesAll"
    Me.btnAttributesAll.Size = New System.Drawing.Size(64, 24)
    Me.btnAttributesAll.TabIndex = 9
    Me.btnAttributesAll.Text = "All"
    '
    'lstAttributes
    '
    Me.lstAttributes.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lstAttributes.IntegralHeight = False
    Me.lstAttributes.Location = New System.Drawing.Point(8, 16)
    Me.lstAttributes.Name = "lstAttributes"
    Me.lstAttributes.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
    Me.lstAttributes.Size = New System.Drawing.Size(184, 240)
    Me.lstAttributes.TabIndex = 7
    '
    'Splitter1
    '
    Me.Splitter1.Location = New System.Drawing.Point(200, 0)
    Me.Splitter1.Name = "Splitter1"
    Me.Splitter1.Size = New System.Drawing.Size(8, 301)
    Me.Splitter1.TabIndex = 10
    Me.Splitter1.TabStop = False
    '
    'grpSeasons
    '
    Me.grpSeasons.Controls.Add(Me.btnSeasonsNone)
    Me.grpSeasons.Controls.Add(Me.btnSeasonsAll)
    Me.grpSeasons.Controls.Add(Me.lstSeasons)
    Me.grpSeasons.Controls.Add(Me.cboSeasons)
    Me.grpSeasons.Dock = System.Windows.Forms.DockStyle.Fill
    Me.grpSeasons.Location = New System.Drawing.Point(208, 0)
    Me.grpSeasons.Name = "grpSeasons"
    Me.grpSeasons.Size = New System.Drawing.Size(200, 301)
    Me.grpSeasons.TabIndex = 11
    Me.grpSeasons.TabStop = False
    Me.grpSeasons.Text = "Seasons"
    '
    'btnSeasonsNone
    '
    Me.btnSeasonsNone.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.btnSeasonsNone.Location = New System.Drawing.Point(128, 264)
    Me.btnSeasonsNone.Name = "btnSeasonsNone"
    Me.btnSeasonsNone.Size = New System.Drawing.Size(64, 23)
    Me.btnSeasonsNone.TabIndex = 12
    Me.btnSeasonsNone.Text = "None"
    '
    'btnSeasonsAll
    '
    Me.btnSeasonsAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
    Me.btnSeasonsAll.Location = New System.Drawing.Point(8, 264)
    Me.btnSeasonsAll.Name = "btnSeasonsAll"
    Me.btnSeasonsAll.Size = New System.Drawing.Size(64, 24)
    Me.btnSeasonsAll.TabIndex = 11
    Me.btnSeasonsAll.Text = "All"
    '
    'lstSeasons
    '
    Me.lstSeasons.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lstSeasons.IntegralHeight = False
    Me.lstSeasons.Location = New System.Drawing.Point(8, 40)
    Me.lstSeasons.Name = "lstSeasons"
    Me.lstSeasons.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
    Me.lstSeasons.Size = New System.Drawing.Size(184, 216)
    Me.lstSeasons.TabIndex = 7
    '
    'cboSeasons
    '
    Me.cboSeasons.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.cboSeasons.Enabled = False
    Me.cboSeasons.Location = New System.Drawing.Point(8, 16)
    Me.cboSeasons.Name = "cboSeasons"
    Me.cboSeasons.Size = New System.Drawing.Size(184, 21)
    Me.cboSeasons.TabIndex = 6
    '
    'frmSpecifySeasonalAttributes
    '
    Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
    Me.ClientSize = New System.Drawing.Size(408, 301)
    Me.Controls.Add(Me.grpSeasons)
    Me.Controls.Add(Me.Splitter1)
    Me.Controls.Add(Me.grpAttributes)
    Me.Name = "frmSpecifySeasonalAttributes"
    Me.Text = "Seasonal Attributes"
    Me.grpAttributes.ResumeLayout(False)
    Me.grpSeasons.ResumeLayout(False)
    Me.ResumeLayout(False)

  End Sub

#End Region

  'Private WithEvents pDataManager As atcDataManager
  Private WithEvents pGroup As atcDataGroup
  Private pSeasonsAvailable As atcDataAttributes

  Public Function AskUser(ByVal aGroup As atcDataGroup, ByVal aSeasonsAvailable As atcDataAttributes) As atcDataAttributes
    'pDataManager = aDataManager
    pGroup = aGroup
    pSeasonsAvailable = aSeasonsAvailable
    Clear()
    Me.ShowDialog()
  End Function

  Private Sub Clear()
    cboSeasons.Items.Clear()
    lstSeasons.Items.Clear()
    lstAttributes.Items.Clear()
    For Each lSeason As atcDefinedValue In pSeasonsAvailable
      cboSeasons.Items.Add(lSeason.Definition.Name.Substring(0, lSeason.Definition.Name.IndexOf("::")))
    Next
    For Each lDef As atcAttributeDefinition In atcDataAttributes.AllDefinitions()
      If lDef.Calculated AndAlso atcDataAttributes.IsSimple(lDef) Then
        lstAttributes.Items.Add(lDef.Name)
      End If
    Next
  End Sub

  Private Sub cboSeasons_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboSeasons.SelectedIndexChanged
    lstSeasons.Items.Clear()
    Dim lSeasonSource As atcDataSource = CurrentSeason()
    If Not lSeasonSource Is Nothing Then
      Dim lArguments As New atcDataAttributes
      Dim lAttributes As New atcDataAttributes
      Dim lCalculatedAttributes As New atcDataAttributes

      lAttributes.SetValue(lstAttributes.SelectedItems(0), 0)

      lArguments.Add("Attributes", lAttributes)
      lArguments.SetValue("Timeseries", pGroup)
      lArguments.SetValue("CalculatedAttributes", lCalculatedAttributes)

      lSeasonSource.Open(cboSeasons.Text & "::SeasonalAttributes", lArguments)
      For Each lSeasonalAttribute As atcDefinedValue In lCalculatedAttributes
        Dim lSeasonName As String = lSeasonalAttribute.Arguments.GetValue("SeasonName") 'Definition.Name
        If Not lstSeasons.Items.Contains(lSeasonName) Then
          lstSeasons.Items.Add(lSeasonName)
          lstSeasons.SetSelected(lstSeasons.Items.Count - 1, True)
        End If
      Next
    End If
  End Sub

  Private Function CurrentSeason() As atcDataSource
    For Each lSeason As atcDefinedValue In pSeasonsAvailable
      If lSeason.Definition.Name.Equals(cboSeasons.Text & "::SeasonalAttributes") Then
        Return lSeason.Definition.Calculator
      End If
    Next
  End Function

  Private Sub btnAttributesAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAttributesAll.Click
    For index As Integer = 0 To lstAttributes.Items.Count - 1
      lstAttributes.SetSelected(index, True)
    Next
  End Sub

  Private Sub btnAttributesNone_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAttributesNone.Click
    For index As Integer = 0 To lstAttributes.Items.Count - 1
      lstAttributes.SetSelected(index, False)
    Next
  End Sub

  Private Sub lstAttributes_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstAttributes.SelectedIndexChanged
    If lstAttributes.SelectedIndices.Count > 0 Then
      cboSeasons.Enabled = True
    Else
      cboSeasons.Enabled = False
    End If
  End Sub

  Private Sub btnSeasonsAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSeasonsAll.Click
    For index As Integer = 0 To lstSeasons.Items.Count - 1
      lstSeasons.SetSelected(index, True)
    Next
  End Sub

  Private Sub btnSeasonsNone_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSeasonsNone.Click
    For index As Integer = 0 To lstSeasons.Items.Count - 1
      lstSeasons.SetSelected(index, False)
    Next
  End Sub
End Class
