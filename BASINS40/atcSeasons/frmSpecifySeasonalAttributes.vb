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
    Friend WithEvents panelTop As System.Windows.Forms.Panel
    Friend WithEvents panelBottom As System.Windows.Forms.Panel
    Friend WithEvents grpSeasons As System.Windows.Forms.GroupBox
    Friend WithEvents btnSeasonsNone As System.Windows.Forms.Button
    Friend WithEvents btnSeasonsAll As System.Windows.Forms.Button
    Friend WithEvents lstSeasons As System.Windows.Forms.ListBox
    Friend WithEvents cboSeasons As System.Windows.Forms.ComboBox
    Friend WithEvents Splitter1 As System.Windows.Forms.Splitter
    Friend WithEvents grpAttributes As System.Windows.Forms.GroupBox
    Friend WithEvents btnAttributesNone As System.Windows.Forms.Button
    Friend WithEvents btnAttributesAll As System.Windows.Forms.Button
    Friend WithEvents lstAttributes As System.Windows.Forms.ListBox
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSpecifySeasonalAttributes))
        Me.panelTop = New System.Windows.Forms.Panel
        Me.grpSeasons = New System.Windows.Forms.GroupBox
        Me.btnSeasonsNone = New System.Windows.Forms.Button
        Me.btnSeasonsAll = New System.Windows.Forms.Button
        Me.lstSeasons = New System.Windows.Forms.ListBox
        Me.cboSeasons = New System.Windows.Forms.ComboBox
        Me.Splitter1 = New System.Windows.Forms.Splitter
        Me.grpAttributes = New System.Windows.Forms.GroupBox
        Me.btnAttributesNone = New System.Windows.Forms.Button
        Me.btnAttributesAll = New System.Windows.Forms.Button
        Me.lstAttributes = New System.Windows.Forms.ListBox
        Me.panelBottom = New System.Windows.Forms.Panel
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnOk = New System.Windows.Forms.Button
        Me.panelTop.SuspendLayout()
        Me.grpSeasons.SuspendLayout()
        Me.grpAttributes.SuspendLayout()
        Me.panelBottom.SuspendLayout()
        Me.SuspendLayout()
        '
        'panelTop
        '
        Me.panelTop.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.panelTop.Controls.Add(Me.grpSeasons)
        Me.panelTop.Controls.Add(Me.Splitter1)
        Me.panelTop.Controls.Add(Me.grpAttributes)
        Me.panelTop.Location = New System.Drawing.Point(0, 0)
        Me.panelTop.Name = "panelTop"
        Me.panelTop.Size = New System.Drawing.Size(408, 328)
        Me.panelTop.TabIndex = 14
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
        Me.grpSeasons.Size = New System.Drawing.Size(200, 328)
        Me.grpSeasons.TabIndex = 14
        Me.grpSeasons.TabStop = False
        Me.grpSeasons.Text = "Seasons"
        '
        'btnSeasonsNone
        '
        Me.btnSeasonsNone.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSeasonsNone.Location = New System.Drawing.Point(128, 291)
        Me.btnSeasonsNone.Name = "btnSeasonsNone"
        Me.btnSeasonsNone.Size = New System.Drawing.Size(64, 23)
        Me.btnSeasonsNone.TabIndex = 12
        Me.btnSeasonsNone.Text = "None"
        '
        'btnSeasonsAll
        '
        Me.btnSeasonsAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnSeasonsAll.Location = New System.Drawing.Point(8, 291)
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
        Me.lstSeasons.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple
        Me.lstSeasons.Size = New System.Drawing.Size(184, 243)
        Me.lstSeasons.TabIndex = 7
        Me.lstSeasons.Tag = "Seasons"
        '
        'cboSeasons
        '
        Me.cboSeasons.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboSeasons.Enabled = False
        Me.cboSeasons.Location = New System.Drawing.Point(8, 16)
        Me.cboSeasons.MaxDropDownItems = 20
        Me.cboSeasons.Name = "cboSeasons"
        Me.cboSeasons.Size = New System.Drawing.Size(184, 21)
        Me.cboSeasons.TabIndex = 6
        Me.cboSeasons.Tag = "SeasonType"
        '
        'Splitter1
        '
        Me.Splitter1.Location = New System.Drawing.Point(200, 0)
        Me.Splitter1.Name = "Splitter1"
        Me.Splitter1.Size = New System.Drawing.Size(8, 328)
        Me.Splitter1.TabIndex = 13
        Me.Splitter1.TabStop = False
        '
        'grpAttributes
        '
        Me.grpAttributes.Controls.Add(Me.btnAttributesNone)
        Me.grpAttributes.Controls.Add(Me.btnAttributesAll)
        Me.grpAttributes.Controls.Add(Me.lstAttributes)
        Me.grpAttributes.Dock = System.Windows.Forms.DockStyle.Left
        Me.grpAttributes.Location = New System.Drawing.Point(0, 0)
        Me.grpAttributes.Name = "grpAttributes"
        Me.grpAttributes.Size = New System.Drawing.Size(200, 328)
        Me.grpAttributes.TabIndex = 12
        Me.grpAttributes.TabStop = False
        Me.grpAttributes.Text = "Attributes"
        '
        'btnAttributesNone
        '
        Me.btnAttributesNone.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnAttributesNone.Location = New System.Drawing.Point(128, 291)
        Me.btnAttributesNone.Name = "btnAttributesNone"
        Me.btnAttributesNone.Size = New System.Drawing.Size(64, 23)
        Me.btnAttributesNone.TabIndex = 10
        Me.btnAttributesNone.Text = "None"
        '
        'btnAttributesAll
        '
        Me.btnAttributesAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnAttributesAll.Location = New System.Drawing.Point(8, 291)
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
        Me.lstAttributes.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple
        Me.lstAttributes.Size = New System.Drawing.Size(184, 267)
        Me.lstAttributes.TabIndex = 7
        Me.lstAttributes.Tag = "Attributes"
        '
        'panelBottom
        '
        Me.panelBottom.Controls.Add(Me.btnCancel)
        Me.panelBottom.Controls.Add(Me.btnOk)
        Me.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.panelBottom.Location = New System.Drawing.Point(0, 334)
        Me.panelBottom.Name = "panelBottom"
        Me.panelBottom.Size = New System.Drawing.Size(408, 39)
        Me.panelBottom.TabIndex = 15
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(336, 3)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(64, 24)
        Me.btnCancel.TabIndex = 1
        Me.btnCancel.Text = "Cancel"
        '
        'btnOk
        '
        Me.btnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOk.Location = New System.Drawing.Point(266, 3)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(64, 24)
        Me.btnOk.TabIndex = 0
        Me.btnOk.Text = "Ok"
        '
        'frmSpecifySeasonalAttributes
        '
        Me.AcceptButton = Me.btnOk
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(408, 373)
        Me.Controls.Add(Me.panelTop)
        Me.Controls.Add(Me.panelBottom)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmSpecifySeasonalAttributes"
        Me.Text = "Seasonal Attributes"
        Me.panelTop.ResumeLayout(False)
        Me.grpSeasons.ResumeLayout(False)
        Me.grpAttributes.ResumeLayout(False)
        Me.panelBottom.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private WithEvents pGroup As atcDataGroup
    Private pSeasonsAvailable As atcDataAttributes
    Private pOk As Boolean

    Public Function AskUser(ByVal aGroup As atcDataGroup, ByVal aSeasonsAvailable As atcDataAttributes) As Boolean
        pGroup = aGroup
        pSeasonsAvailable = aSeasonsAvailable
        Clear()
        Me.ShowDialog()
        If pOk Then
            Dim lAttributesToCalculate As New atcDataAttributes
            For Each lAttrName As String In lstAttributes.SelectedItems
                lAttributesToCalculate.SetValue(lAttrName, Nothing)
            Next
            CalculateAttributes(lAttributesToCalculate, True)
        End If
        Return pOk
    End Function

    Private Sub Clear()
        pOk = False
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
        LoadListSelected(lstAttributes)
        cboSeasons.SelectedItem = GetSetting("atcSeasons", "SeasonType", "combobox", "")
        LoadListSelected(lstSeasons)
    End Sub

    Private Sub SaveListSelected(ByVal lst As Windows.Forms.ListBox)
        SaveSetting("atcSeasons", lst.Tag, "dummy", "")
        DeleteSetting("atcSeasons", lst.Tag)
        For Each lItem As String In lst.SelectedItems
            SaveSetting("atcSeasons", lst.Tag, lItem, lItem)
        Next
    End Sub

    Private Sub LoadListSelected(ByVal lst As Windows.Forms.ListBox)
        Dim lSelectedArray As String(,) = GetAllSettings("atcSeasons", lst.Tag)
        Dim lItemIndex As Integer
        Try
            lst.ClearSelected()
            For lIndex As Integer = lSelectedArray.GetUpperBound(0) To 0 Step -1
                Dim lSelectedItem As String = lSelectedArray(lIndex, 1)
                For lItemIndex = lst.Items.Count - 1 To 0 Step -1
                    If lst.Items(lItemIndex) = lSelectedItem Then
                        lst.SetSelected(lItemIndex, True)
                        Exit For
                    End If
                Next
            Next
        Catch e As Exception
            MapWinUtility.Logger.Dbg("Error retrieving saved settings: " & e.Message)
        End Try
    End Sub

    Private Sub cboSeasons_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboSeasons.SelectedIndexChanged
        lstSeasons.Items.Clear()
        Dim lSeasonSource As atcDataSource = CurrentSeason()
        If Not lSeasonSource Is Nothing AndAlso lstAttributes.SelectedItems.Count > 0 Then
            Dim lArguments As New atcDataAttributes
            Dim lAttributes As New atcDataAttributes
            lAttributes.SetValue(lstAttributes.SelectedItems(0), 0)
            lArguments.Add("Attributes", lAttributes)

            For Each lSeasonalAttribute As atcDefinedValue In CalculateAttributes(lAttributes, False)
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
        Return Nothing
    End Function

    Private Function CalculateAttributes(ByVal aAttributes As atcDataAttributes, _
                                         ByVal aSetInTimeseries As Boolean) As atcDataAttributes
        Dim lSeasonSource As atcDataSource = CurrentSeason()
        Dim lCalculatedAttributes As New atcDataAttributes
        Dim lAllCalculatedAttributes As atcDataAttributes

        If aSetInTimeseries Then
            lAllCalculatedAttributes = New atcDataAttributes
        Else
            lAllCalculatedAttributes = lCalculatedAttributes
        End If
        If Not lSeasonSource Is Nothing Then
            Dim lArguments As New atcDataAttributes

            lArguments.SetValue("Attributes", aAttributes)
            lArguments.SetValue("CalculatedAttributes", lCalculatedAttributes)

            For Each lTimeseries As atcTimeseries In pGroup
                lArguments.SetValue("Timeseries", lTimeseries)
                lSeasonSource.Open(cboSeasons.Text & "::SeasonalAttributes", lArguments)
                If aSetInTimeseries Then
                    For Each lAtt As atcDefinedValue In lCalculatedAttributes 'Seasonal Attribute
                        Dim lSeasonName As String = lAtt.Arguments.GetValue("SeasonName")
                        If lstSeasons.SelectedItems.Contains(lSeasonName) Then 'This season is selected, set the attribute
                            lTimeseries.Attributes.SetValue(lAtt.Definition, lAtt.Value, lAtt.Arguments)
                            lAllCalculatedAttributes.SetValue(lAtt.Definition, lAtt.Value, lAtt.Arguments)
                        End If
                    Next
                    lCalculatedAttributes.Clear()
                End If
            Next
        End If
        Return lAllCalculatedAttributes

    End Function

    Private Sub lstAttributes_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstAttributes.SelectedIndexChanged
        If lstAttributes.SelectedIndices.Count > 0 Then
            cboSeasons.Enabled = True
        Else
            cboSeasons.Enabled = False
        End If
    End Sub

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

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        pOk = True
        SaveListSelected(lstAttributes)
        SaveSetting("atcSeasons", "SeasonType", "combobox", cboSeasons.SelectedItem)
        SaveListSelected(lstSeasons)
        Close()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Close()
    End Sub

End Class
