Imports atcData
Imports atcSeasons
Imports atcUtility
Imports MapWinUtility

Public Class frmVariation
    Inherits System.Windows.Forms.Form

    Private Const AllSeasons As String = "All Seasons"
    Private Const pClickMe As String = "<click to specify>"

    Private pVariation As Variation
    Private pSeasonsAvailable As New atcCollection
    Private pSeasons As atcSeasonBase
    Friend WithEvents txtVaryPET As System.Windows.Forms.TextBox
    Friend WithEvents lblPET As System.Windows.Forms.Label
    Friend WithEvents lblAirTemp As System.Windows.Forms.Label

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
    Friend WithEvents lblData As System.Windows.Forms.Label
    Friend WithEvents txtVaryData As System.Windows.Forms.TextBox
    Friend WithEvents grpSeasons As System.Windows.Forms.GroupBox
    Friend WithEvents cboSeasons As System.Windows.Forms.ComboBox
    Friend WithEvents lstSeasons As System.Windows.Forms.ListBox
    Friend WithEvents btnSeasonsAll As System.Windows.Forms.Button
    Friend WithEvents btnSeasonsNone As System.Windows.Forms.Button
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents lblName As System.Windows.Forms.Label
    Friend WithEvents txtName As System.Windows.Forms.TextBox
    Friend WithEvents btnScript As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmVariation))
        Me.lblFunction = New System.Windows.Forms.Label
        Me.txtFunction = New System.Windows.Forms.TextBox
        Me.txtIncrement = New System.Windows.Forms.TextBox
        Me.lblIncrement = New System.Windows.Forms.Label
        Me.txtMax = New System.Windows.Forms.TextBox
        Me.txtMin = New System.Windows.Forms.TextBox
        Me.lblMaximum = New System.Windows.Forms.Label
        Me.lblMinimum = New System.Windows.Forms.Label
        Me.lblData = New System.Windows.Forms.Label
        Me.txtVaryData = New System.Windows.Forms.TextBox
        Me.grpSeasons = New System.Windows.Forms.GroupBox
        Me.cboSeasons = New System.Windows.Forms.ComboBox
        Me.lstSeasons = New System.Windows.Forms.ListBox
        Me.btnSeasonsAll = New System.Windows.Forms.Button
        Me.btnSeasonsNone = New System.Windows.Forms.Button
        Me.btnOk = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.lblName = New System.Windows.Forms.Label
        Me.txtName = New System.Windows.Forms.TextBox
        Me.btnScript = New System.Windows.Forms.Button
        Me.txtVaryPET = New System.Windows.Forms.TextBox
        Me.lblPET = New System.Windows.Forms.Label
        Me.lblAirTemp = New System.Windows.Forms.Label
        Me.grpSeasons.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblFunction
        '
        Me.lblFunction.AutoSize = True
        Me.lblFunction.BackColor = System.Drawing.Color.Transparent
        Me.lblFunction.Location = New System.Drawing.Point(43, 138)
        Me.lblFunction.Name = "lblFunction"
        Me.lblFunction.Size = New System.Drawing.Size(51, 13)
        Me.lblFunction.TabIndex = 5
        Me.lblFunction.Text = "Function:"
        Me.lblFunction.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtFunction
        '
        Me.txtFunction.Location = New System.Drawing.Point(100, 135)
        Me.txtFunction.Name = "txtFunction"
        Me.txtFunction.Size = New System.Drawing.Size(71, 20)
        Me.txtFunction.TabIndex = 6
        Me.txtFunction.Text = "Multiply"
        '
        'txtIncrement
        '
        Me.txtIncrement.Location = New System.Drawing.Point(100, 207)
        Me.txtIncrement.Name = "txtIncrement"
        Me.txtIncrement.Size = New System.Drawing.Size(71, 20)
        Me.txtIncrement.TabIndex = 12
        Me.txtIncrement.Text = "0.05"
        '
        'lblIncrement
        '
        Me.lblIncrement.AutoSize = True
        Me.lblIncrement.BackColor = System.Drawing.Color.Transparent
        Me.lblIncrement.ImageAlign = System.Drawing.ContentAlignment.BottomRight
        Me.lblIncrement.Location = New System.Drawing.Point(37, 210)
        Me.lblIncrement.Name = "lblIncrement"
        Me.lblIncrement.Size = New System.Drawing.Size(57, 13)
        Me.lblIncrement.TabIndex = 11
        Me.lblIncrement.Text = "Increment:"
        Me.lblIncrement.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtMax
        '
        Me.txtMax.Location = New System.Drawing.Point(100, 183)
        Me.txtMax.Name = "txtMax"
        Me.txtMax.Size = New System.Drawing.Size(71, 20)
        Me.txtMax.TabIndex = 10
        Me.txtMax.Text = "1.1"
        '
        'txtMin
        '
        Me.txtMin.Location = New System.Drawing.Point(100, 159)
        Me.txtMin.Name = "txtMin"
        Me.txtMin.Size = New System.Drawing.Size(71, 20)
        Me.txtMin.TabIndex = 8
        Me.txtMin.Text = "0.9"
        '
        'lblMaximum
        '
        Me.lblMaximum.AutoSize = True
        Me.lblMaximum.BackColor = System.Drawing.Color.Transparent
        Me.lblMaximum.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblMaximum.Location = New System.Drawing.Point(40, 186)
        Me.lblMaximum.Name = "lblMaximum"
        Me.lblMaximum.Size = New System.Drawing.Size(54, 13)
        Me.lblMaximum.TabIndex = 9
        Me.lblMaximum.Text = "Maximum:"
        Me.lblMaximum.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblMinimum
        '
        Me.lblMinimum.AutoSize = True
        Me.lblMinimum.BackColor = System.Drawing.Color.Transparent
        Me.lblMinimum.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblMinimum.Location = New System.Drawing.Point(43, 162)
        Me.lblMinimum.Name = "lblMinimum"
        Me.lblMinimum.Size = New System.Drawing.Size(51, 13)
        Me.lblMinimum.TabIndex = 7
        Me.lblMinimum.Text = "Minimum:"
        Me.lblMinimum.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblData
        '
        Me.lblData.AutoSize = True
        Me.lblData.BackColor = System.Drawing.Color.Transparent
        Me.lblData.Location = New System.Drawing.Point(25, 41)
        Me.lblData.Name = "lblData"
        Me.lblData.Size = New System.Drawing.Size(69, 13)
        Me.lblData.TabIndex = 3
        Me.lblData.Text = "Data to Vary:"
        Me.lblData.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'txtVaryData
        '
        Me.txtVaryData.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtVaryData.Location = New System.Drawing.Point(100, 38)
        Me.txtVaryData.Name = "txtVaryData"
        Me.txtVaryData.Size = New System.Drawing.Size(330, 20)
        Me.txtVaryData.TabIndex = 4
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
        Me.grpSeasons.Location = New System.Drawing.Point(177, 115)
        Me.grpSeasons.Name = "grpSeasons"
        Me.grpSeasons.Size = New System.Drawing.Size(253, 216)
        Me.grpSeasons.TabIndex = 13
        Me.grpSeasons.TabStop = False
        Me.grpSeasons.Text = "Vary Seasonally"
        '
        'cboSeasons
        '
        Me.cboSeasons.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboSeasons.ItemHeight = 13
        Me.cboSeasons.Location = New System.Drawing.Point(6, 19)
        Me.cboSeasons.MaxDropDownItems = 20
        Me.cboSeasons.Name = "cboSeasons"
        Me.cboSeasons.Size = New System.Drawing.Size(241, 21)
        Me.cboSeasons.TabIndex = 14
        '
        'lstSeasons
        '
        Me.lstSeasons.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstSeasons.IntegralHeight = False
        Me.lstSeasons.Location = New System.Drawing.Point(6, 46)
        Me.lstSeasons.Name = "lstSeasons"
        Me.lstSeasons.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple
        Me.lstSeasons.Size = New System.Drawing.Size(241, 135)
        Me.lstSeasons.TabIndex = 15
        '
        'btnSeasonsAll
        '
        Me.btnSeasonsAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnSeasonsAll.Location = New System.Drawing.Point(6, 187)
        Me.btnSeasonsAll.Name = "btnSeasonsAll"
        Me.btnSeasonsAll.Size = New System.Drawing.Size(63, 23)
        Me.btnSeasonsAll.TabIndex = 16
        Me.btnSeasonsAll.Text = "All"
        '
        'btnSeasonsNone
        '
        Me.btnSeasonsNone.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSeasonsNone.Location = New System.Drawing.Point(184, 187)
        Me.btnSeasonsNone.Name = "btnSeasonsNone"
        Me.btnSeasonsNone.Size = New System.Drawing.Size(63, 23)
        Me.btnSeasonsNone.TabIndex = 17
        Me.btnSeasonsNone.Text = "None"
        '
        'btnOk
        '
        Me.btnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOk.Location = New System.Drawing.Point(280, 347)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(72, 24)
        Me.btnOk.TabIndex = 19
        Me.btnOk.Text = "Ok"
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(358, 347)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(72, 24)
        Me.btnCancel.TabIndex = 20
        Me.btnCancel.Text = "Cancel"
        '
        'lblName
        '
        Me.lblName.AutoSize = True
        Me.lblName.BackColor = System.Drawing.Color.Transparent
        Me.lblName.Location = New System.Drawing.Point(29, 15)
        Me.lblName.Name = "lblName"
        Me.lblName.Size = New System.Drawing.Size(65, 13)
        Me.lblName.TabIndex = 1
        Me.lblName.Text = "Input Name:"
        Me.lblName.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'txtName
        '
        Me.txtName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtName.Location = New System.Drawing.Point(100, 12)
        Me.txtName.Name = "txtName"
        Me.txtName.Size = New System.Drawing.Size(330, 20)
        Me.txtName.TabIndex = 2
        '
        'btnScript
        '
        Me.btnScript.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnScript.Location = New System.Drawing.Point(178, 347)
        Me.btnScript.Name = "btnScript"
        Me.btnScript.Size = New System.Drawing.Size(96, 24)
        Me.btnScript.TabIndex = 18
        Me.btnScript.Text = "Open Script..."
        '
        'txtVaryPET
        '
        Me.txtVaryPET.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtVaryPET.Location = New System.Drawing.Point(100, 83)
        Me.txtVaryPET.Name = "txtVaryPET"
        Me.txtVaryPET.Size = New System.Drawing.Size(330, 20)
        Me.txtVaryPET.TabIndex = 5
        '
        'lblPET
        '
        Me.lblPET.AutoSize = True
        Me.lblPET.BackColor = System.Drawing.Color.Transparent
        Me.lblPET.Location = New System.Drawing.Point(27, 86)
        Me.lblPET.Name = "lblPET"
        Me.lblPET.Size = New System.Drawing.Size(67, 13)
        Me.lblPET.TabIndex = 21
        Me.lblPET.Text = "PET to Vary:"
        Me.lblPET.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblAirTemp
        '
        Me.lblAirTemp.AutoSize = True
        Me.lblAirTemp.BackColor = System.Drawing.Color.Transparent
        Me.lblAirTemp.Location = New System.Drawing.Point(27, 67)
        Me.lblAirTemp.Name = "lblAirTemp"
        Me.lblAirTemp.Size = New System.Drawing.Size(346, 13)
        Me.lblAirTemp.TabIndex = 22
        Me.lblAirTemp.Text = "If the data being varied is air temperature, PET can be computed from it:"
        Me.lblAirTemp.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'frmVariation
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(442, 383)
        Me.Controls.Add(Me.txtVaryPET)
        Me.Controls.Add(Me.lblPET)
        Me.Controls.Add(Me.btnScript)
        Me.Controls.Add(Me.lblName)
        Me.Controls.Add(Me.txtName)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOk)
        Me.Controls.Add(Me.grpSeasons)
        Me.Controls.Add(Me.lblFunction)
        Me.Controls.Add(Me.txtFunction)
        Me.Controls.Add(Me.txtIncrement)
        Me.Controls.Add(Me.txtMax)
        Me.Controls.Add(Me.txtMin)
        Me.Controls.Add(Me.txtVaryData)
        Me.Controls.Add(Me.lblIncrement)
        Me.Controls.Add(Me.lblMaximum)
        Me.Controls.Add(Me.lblMinimum)
        Me.Controls.Add(Me.lblData)
        Me.Controls.Add(Me.lblAirTemp)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Name = "frmVariation"
        Me.Text = "Input"
        Me.grpSeasons.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Public Function AskUser(Optional ByVal aVariation As Variation = Nothing) As Variation
        If aVariation Is Nothing Then
            pVariation = New Variation
        Else
            pVariation = aVariation.Clone
        End If
        If pVariation.DataSets Is Nothing Then pVariation.DataSets = New atcDataGroup

        cboSeasons.Items.Add(AllSeasons)
        pSeasonsAvailable = atcSeasonPlugin.AllSeasonTypes
        For Each lSeasonType As Type In pSeasonsAvailable
            Dim lSeasonTypeShortName As String = atcSeasonPlugin.SeasonClassNameToLabel(lSeasonType.Name)
            Select Case lSeasonTypeShortName 'TODO: handle difficult seasons
                Case "Calendar Year"
                Case "Water Year"
                Case "Year Subset"
                Case Else
                    cboSeasons.Items.Add(lSeasonTypeShortName)
            End Select
        Next

        FormFromVariation()

        If Me.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Return pVariation
        Else
            Return Nothing
        End If
    End Function

    Private Sub txtVaryData_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtVaryData.Click
        UserSelectData()
    End Sub

    Private Sub txtVaryData_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtVaryData.KeyPress
        UserSelectData()
    End Sub

    Private Sub txtVaryPET_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtVaryPET.Click
        UserSelectPET()
    End Sub

    Private Sub txtVaryPET_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtVaryPET.KeyPress
        UserSelectPET()
    End Sub

    Private Sub UserSelectData()
        Dim lData As atcDataGroup = g_DataManager.UserSelectData("Select data to vary", pVariation.DataSets)
        If Not lData Is Nothing Then
            pVariation.DataSets = lData
            UpdateDataText(txtVaryData, lData)
        End If
    End Sub

    Private Sub UserSelectPET()
        Dim lData As atcDataGroup = g_DataManager.UserSelectData("Select PET data to compute from air temperature", pVariation.DataSets)
        If Not lData Is Nothing Then
            pVariation.PETdata = lData
            UpdateDataText(txtVaryPET, lData)
        End If
    End Sub

    Private Sub txtFunction_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtFunction.Click
        Dim aCategory As New ArrayList(1)
        aCategory.Add("Generate Timeseries")
        pVariation.ComputationSource = g_DataManager.UserSelectDataSource(aCategory, "Select Function for Varying Input Data")
        If pVariation.ComputationSource Is Nothing Then
            txtFunction.Text = pClickMe
        Else
            txtFunction.Text = pVariation.ComputationSource.Specification
        End If
    End Sub

    Private Sub UpdateDataText(ByVal aTextBox As Windows.Forms.TextBox, _
                               ByVal aGroup As atcDataGroup)
        If Not aGroup Is Nothing AndAlso aGroup.Count > 0 Then
            aTextBox.Text = aGroup.ItemByIndex(0).ToString
            If aGroup.Count > 1 Then aTextBox.Text &= " (and " & aGroup.Count - 1 & " more)"
        Else
            aTextBox.Text = pClickMe
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

    Private Function VariationFromForm(ByVal aVariation As Variation) As Boolean
        With aVariation
            .Name = txtName.Text
            If txtVaryData.Text.Equals(pClickMe) Then
                Logger.Msg("No data was selected", "Need Data To Vary")
                Return False
            End If
            If txtFunction.Text.Equals(pClickMe) Then
                Logger.Msg("No function was selected", "Need Function")
                Return False
            End If
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

    Private Sub btnScript_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnScript.Click
        Dim lOpenDialog As New System.Windows.Forms.OpenFileDialog
        Dim lVariationTemplate As New Variation

        If VariationFromForm(lVariationTemplate) Then
            With lOpenDialog
                .Title = "Select Script"
                .Filter = "VB.Net *.vb|*.vb|C Sharp *.cs|*.cs|All Files|*.*"
                Try
                    .FilterIndex = CInt(GetSetting("BASINS4", "Scenario", "ScriptExtIndex", 1))
                Catch
                    .FilterIndex = 1
                End Try
                .FileName = GetSetting("BASINS4", "Scenario", "ScriptFilename", ReplaceString(Me.Text, " ", "_") & ".vb")
                If .ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                    SaveSetting("BASINS4", "Scenario", "ScriptExtIndex", .FilterIndex)
                    SaveSetting("BASINS4", "Scenario", "ScriptFilename", .FileName)
                    Dim lErrors As String = ""
                    Try
                        pVariation = Scripting.Run(FileExt(.FileName), "", .FileName, lErrors, False, (g_MapWin), lVariationTemplate)
                    Catch ex As Exception
                        If lErrors.Length > 0 Then lErrors &= vbCrLf & vbCrLf
                        lErrors &= ex.Message
                    End Try
                    If lErrors.Length > 0 Then
                        Logger.Msg("Error running variation script" & vbCrLf & vbCrLf & lErrors)
                    Else 'ok scriped variation - update form as appropriate
                        FormFromVariation()
                    End If
                End If
            End With
        End If
    End Sub

    Private Sub FormFromVariation()
        With pVariation
            txtName.Text = .Name
            txtFunction.Text = .Operation
            If Not Double.IsNaN(.Min) Then txtMin.Text = .Min
            If Not Double.IsNaN(.Max) Then txtMax.Text = .Max
            If Not Double.IsNaN(.Increment) Then txtIncrement.Text = .Increment
            UpdateDataText(txtVaryData, pVariation.DataSets)
            UpdateDataText(txtVaryPET, pVariation.PETdata)
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

    Private Sub frmVariation_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyValue = Windows.Forms.Keys.F1 Then
            ShowHelp("BASINS Details\Analysis\Climate Assessment Tool.html")
        End If
    End Sub

End Class
