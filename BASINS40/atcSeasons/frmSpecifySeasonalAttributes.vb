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
  Friend WithEvents grpSeasons As System.Windows.Forms.GroupBox
  Friend WithEvents lstSeasons As System.Windows.Forms.ListBox
  Friend WithEvents cboSeasons As System.Windows.Forms.ComboBox
  Friend WithEvents Splitter1 As System.Windows.Forms.Splitter
  Friend WithEvents grpAttributes As System.Windows.Forms.GroupBox
  Friend WithEvents btnAttributesNone As System.Windows.Forms.Button
  Friend WithEvents btnAttributesAll As System.Windows.Forms.Button
  Friend WithEvents lstAttributes As System.Windows.Forms.ListBox
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
    Me.grpSeasons = New System.Windows.Forms.GroupBox
    Me.lstSeasons = New System.Windows.Forms.ListBox
    Me.cboSeasons = New System.Windows.Forms.ComboBox
    Me.Splitter1 = New System.Windows.Forms.Splitter
    Me.grpAttributes = New System.Windows.Forms.GroupBox
    Me.btnAttributesNone = New System.Windows.Forms.Button
    Me.btnAttributesAll = New System.Windows.Forms.Button
    Me.lstAttributes = New System.Windows.Forms.ListBox
    Me.grpSeasons.SuspendLayout()
    Me.grpAttributes.SuspendLayout()
    Me.SuspendLayout()
    '
    'grpSeasons
    '
    Me.grpSeasons.Controls.Add(Me.lstSeasons)
    Me.grpSeasons.Controls.Add(Me.cboSeasons)
    Me.grpSeasons.Dock = System.Windows.Forms.DockStyle.Left
    Me.grpSeasons.Location = New System.Drawing.Point(0, 0)
    Me.grpSeasons.Name = "grpSeasons"
    Me.grpSeasons.Size = New System.Drawing.Size(200, 301)
    Me.grpSeasons.TabIndex = 7
    Me.grpSeasons.TabStop = False
    Me.grpSeasons.Text = "Seasons"
    '
    'lstSeasons
    '
    Me.lstSeasons.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lstSeasons.IntegralHeight = False
    Me.lstSeasons.Location = New System.Drawing.Point(8, 56)
    Me.lstSeasons.Name = "lstSeasons"
    Me.lstSeasons.Size = New System.Drawing.Size(184, 236)
    Me.lstSeasons.TabIndex = 7
    '
    'cboSeasons
    '
    Me.cboSeasons.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.cboSeasons.Location = New System.Drawing.Point(8, 24)
    Me.cboSeasons.Name = "cboSeasons"
    Me.cboSeasons.Size = New System.Drawing.Size(184, 21)
    Me.cboSeasons.TabIndex = 6
    '
    'Splitter1
    '
    Me.Splitter1.Location = New System.Drawing.Point(200, 0)
    Me.Splitter1.Name = "Splitter1"
    Me.Splitter1.Size = New System.Drawing.Size(8, 301)
    Me.Splitter1.TabIndex = 8
    Me.Splitter1.TabStop = False
    '
    'grpAttributes
    '
    Me.grpAttributes.Controls.Add(Me.btnAttributesNone)
    Me.grpAttributes.Controls.Add(Me.btnAttributesAll)
    Me.grpAttributes.Controls.Add(Me.lstAttributes)
    Me.grpAttributes.Dock = System.Windows.Forms.DockStyle.Fill
    Me.grpAttributes.Location = New System.Drawing.Point(208, 0)
    Me.grpAttributes.Name = "grpAttributes"
    Me.grpAttributes.Size = New System.Drawing.Size(200, 301)
    Me.grpAttributes.TabIndex = 9
    Me.grpAttributes.TabStop = False
    Me.grpAttributes.Text = "Attributes"
    '
    'btnAttributesNone
    '
    Me.btnAttributesNone.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.btnAttributesNone.Location = New System.Drawing.Point(128, 24)
    Me.btnAttributesNone.Name = "btnAttributesNone"
    Me.btnAttributesNone.Size = New System.Drawing.Size(64, 23)
    Me.btnAttributesNone.TabIndex = 10
    Me.btnAttributesNone.Text = "None"
    '
    'btnAttributesAll
    '
    Me.btnAttributesAll.Location = New System.Drawing.Point(8, 24)
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
    Me.lstAttributes.Location = New System.Drawing.Point(8, 56)
    Me.lstAttributes.Name = "lstAttributes"
    Me.lstAttributes.Size = New System.Drawing.Size(184, 236)
    Me.lstAttributes.TabIndex = 7
    '
    'frmSeasonalAttributes
    '
    Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
    Me.ClientSize = New System.Drawing.Size(408, 301)
    Me.Controls.Add(Me.grpAttributes)
    Me.Controls.Add(Me.Splitter1)
    Me.Controls.Add(Me.grpSeasons)
    Me.Name = "frmSeasonalAttributes"
    Me.Text = "Seasonal Attributes"
    Me.grpSeasons.ResumeLayout(False)
    Me.grpAttributes.ResumeLayout(False)
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
  End Function

  Private Sub Clear()
    cboSeasons.Items.Clear()
    lstSeasons.Items.Clear()
    lstAttributes.Items.Clear()
    For Each lSeason As atcDefinedValue In pSeasonsAvailable
      cboSeasons.Items.Add(lSeason.Definition.Name)
    Next
    For Each lDef As atcAttributeDefinition In atcDataAttributes.AllDefinitions()
      If lDef.Calculated Then lstAttributes.Items.Add(lDef.Name)
    Next
  End Sub

  Private Sub cboSeasons_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboSeasons.SelectedIndexChanged
    lstSeasons.Items.Clear()
    Dim lSeasonSource As atcDataSource = CurrentSeason()
    If Not lSeasonSource Is Nothing Then
      Dim lArguments As New atcDataAttributes
      Dim lAttributes As New atcDataAttributes
      Dim lCalculatedAttributes As New atcDataAttributes

      lAttributes.SetValue("Min", 0)

      lArguments.Add("Attributes", lAttributes)
      lArguments.SetValue("Timeseries", pGroup)
      lArguments.SetValue("CalculatedAttributes", lCalculatedAttributes)

      lSeasonSource.Open(cboSeasons.Text & "::SeasonalAttributes", lArguments)
      For Each lSeasonalAttribute As atcDefinedValue In lCalculatedAttributes
        If Not lstSeasons.Items.Contains(lSeasonalAttribute.Definition.Name) Then
          lstSeasons.Items.Add(lSeasonalAttribute.Definition.Name)
        End If
      Next
    End If
  End Sub

  Private Function CurrentSeason() As atcDataSource
    For Each lSeason As atcDefinedValue In pSeasonsAvailable
      If lSeason.Definition.Name.Equals(cboSeasons.Text) Then
        Return lSeason.Definition.Calculator
      End If
    Next
  End Function
End Class
