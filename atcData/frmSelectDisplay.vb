Imports atcUtility
Imports MapWinUtility

Public Class frmSelectDisplay
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
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents lblDescribeDatasets As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents btnDiscard As System.Windows.Forms.Button
    Friend WithEvents btnFrequency As System.Windows.Forms.Button
    Friend WithEvents btnSeasonal As System.Windows.Forms.Button
    Friend WithEvents btnTree As System.Windows.Forms.Button
    Friend WithEvents btnGraph As System.Windows.Forms.Button
    Friend WithEvents btnList As System.Windows.Forms.Button
    Friend WithEvents btnSelect As System.Windows.Forms.Button

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSelectDisplay))
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.btnFrequency = New System.Windows.Forms.Button
        Me.btnSeasonal = New System.Windows.Forms.Button
        Me.btnTree = New System.Windows.Forms.Button
        Me.btnGraph = New System.Windows.Forms.Button
        Me.btnList = New System.Windows.Forms.Button
        Me.lblDescribeDatasets = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.btnSave = New System.Windows.Forms.Button
        Me.btnDiscard = New System.Windows.Forms.Button
        Me.btnSelect = New System.Windows.Forms.Button
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.btnFrequency)
        Me.GroupBox1.Controls.Add(Me.btnSeasonal)
        Me.GroupBox1.Controls.Add(Me.btnTree)
        Me.GroupBox1.Controls.Add(Me.btnGraph)
        Me.GroupBox1.Controls.Add(Me.btnList)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 144)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(211, 165)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Display"
        '
        'btnFrequency
        '
        Me.btnFrequency.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnFrequency.Location = New System.Drawing.Point(6, 135)
        Me.btnFrequency.Name = "btnFrequency"
        Me.btnFrequency.Size = New System.Drawing.Size(197, 23)
        Me.btnFrequency.TabIndex = 8
        Me.btnFrequency.Tag = "Analysis::Frequency Grid"
        Me.btnFrequency.Text = "Frequency Grid"
        Me.btnFrequency.UseVisualStyleBackColor = True
        '
        'btnSeasonal
        '
        Me.btnSeasonal.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSeasonal.Location = New System.Drawing.Point(6, 106)
        Me.btnSeasonal.Name = "btnSeasonal"
        Me.btnSeasonal.Size = New System.Drawing.Size(197, 23)
        Me.btnSeasonal.TabIndex = 7
        Me.btnSeasonal.Tag = "Analysis::Seasonal Attributes"
        Me.btnSeasonal.Text = "Seasonal Attributes"
        Me.btnSeasonal.UseVisualStyleBackColor = True
        '
        'btnTree
        '
        Me.btnTree.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnTree.Location = New System.Drawing.Point(6, 77)
        Me.btnTree.Name = "btnTree"
        Me.btnTree.Size = New System.Drawing.Size(197, 23)
        Me.btnTree.TabIndex = 6
        Me.btnTree.Tag = "Analysis::Data Tree"
        Me.btnTree.Text = "Data Tree"
        Me.btnTree.UseVisualStyleBackColor = True
        '
        'btnGraph
        '
        Me.btnGraph.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGraph.Location = New System.Drawing.Point(6, 48)
        Me.btnGraph.Name = "btnGraph"
        Me.btnGraph.Size = New System.Drawing.Size(197, 23)
        Me.btnGraph.TabIndex = 5
        Me.btnGraph.Tag = "Analysis::Graph"
        Me.btnGraph.Text = "Graph"
        Me.btnGraph.UseVisualStyleBackColor = True
        '
        'btnList
        '
        Me.btnList.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnList.Location = New System.Drawing.Point(6, 19)
        Me.btnList.Name = "btnList"
        Me.btnList.Size = New System.Drawing.Size(197, 23)
        Me.btnList.TabIndex = 5
        Me.btnList.Tag = "Analysis::List"
        Me.btnList.Text = "List"
        Me.btnList.UseVisualStyleBackColor = True
        '
        'lblDescribeDatasets
        '
        Me.lblDescribeDatasets.AutoSize = True
        Me.lblDescribeDatasets.Location = New System.Drawing.Point(12, 9)
        Me.lblDescribeDatasets.Name = "lblDescribeDatasets"
        Me.lblDescribeDatasets.Size = New System.Drawing.Size(152, 13)
        Me.lblDescribeDatasets.TabIndex = 1
        Me.lblDescribeDatasets.Text = "New Data has been computed"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 31)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(158, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Select what to do with this data:"
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.Location = New System.Drawing.Point(18, 57)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(197, 23)
        Me.btnSave.TabIndex = 3
        Me.btnSave.Text = "Save to file"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnDiscard
        '
        Me.btnDiscard.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDiscard.Location = New System.Drawing.Point(18, 86)
        Me.btnDiscard.Name = "btnDiscard"
        Me.btnDiscard.Size = New System.Drawing.Size(197, 23)
        Me.btnDiscard.TabIndex = 4
        Me.btnDiscard.Text = "Discard"
        Me.btnDiscard.UseVisualStyleBackColor = True
        '
        'btnSelect
        '
        Me.btnSelect.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSelect.Location = New System.Drawing.Point(18, 115)
        Me.btnSelect.Name = "btnSelect"
        Me.btnSelect.Size = New System.Drawing.Size(197, 23)
        Me.btnSelect.TabIndex = 5
        Me.btnSelect.Text = "Add/Remove Datasets"
        Me.btnSelect.UseVisualStyleBackColor = True
        '
        'frmSelectDisplay
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(235, 321)
        Me.Controls.Add(Me.btnSelect)
        Me.Controls.Add(Me.btnDiscard)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.lblDescribeDatasets)
        Me.Controls.Add(Me.GroupBox1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Name = "frmSelectDisplay"
        Me.Text = "Display Data"
        Me.GroupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private Const pPADDING As Integer = 5
    'Private pArgButton() As Windows.Forms.Button
    Private pTimeseriesGroup As atcTimeseriesGroup

    Public Sub AskUser(ByVal aTimeseriesGroup As atcTimeseriesGroup)
        pTimeseriesGroup = aTimeseriesGroup
        FormFromGroup()
        'iArg -= 1
        'If iArg >= 0 Then
        '    Me.Height = pArgButton(iArg).Top + pArgButton(iArg).Height + pPADDING + (Me.Height - Me.ClientRectangle.Height)
        Me.Show() 'Dialog()
        'Else
        '    Me.Close()
        'End If
    End Sub

    Private Sub FormFromGroup()
        lblDescribeDatasets.Text = pTimeseriesGroup.Count & " dataset"
        If pTimeseriesGroup.Count <> 1 Then lblDescribeDatasets.Text &= "s"
    End Sub

    Private Sub btn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnList.Click, btnGraph.Click, btnTree.Click, btnSeasonal.Click, btnFrequency.Click
        atcDataManager.ShowDisplay(sender.tag, pTimeseriesGroup)
    End Sub

    Private Sub btnDiscard_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDiscard.Click
        Dim lDataSource As atcDataSource
        For iDataSource As Integer = 0 To atcDataManager.DataSources.Count - 1
            lDataSource = atcDataManager.DataSources.Item(iDataSource)
            If lDataSource.DataSets.Equals(pTimeseriesGroup) Then
                If Logger.Msg("Discard " & lDataSource.ToString, MsgBoxStyle.YesNo, "Discard Data") = MsgBoxResult.Yes Then
                    pTimeseriesGroup.Dispose()
                    atcDataManager.RemoveDataSource(lDataSource)
                    Me.Close()
                Else
                    Exit Sub
                End If
            End If
        Next
    End Sub

    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        'If SaveData() Then Me.Close()
        SaveData()
    End Sub

    Private Function SaveData() As Boolean
        Dim lSave As New frmSaveData
        Dim lSaveSource As atcDataSource = lSave.AskUser(pTimeseriesGroup)
        '    Dim lSaveIn As atcDataSource = UserOpenDataFile(False, True)
        If Not lSaveSource Is Nothing AndAlso lSaveSource.Specification.Length > 0 Then
            Return lSaveSource.AddDataSets(pTimeseriesGroup)
        End If
        '    End If
        '    Me.Close()
        Return False
    End Function

    Private Function UserOpenDataFile(Optional ByVal aNeedToOpen As Boolean = True, _
                                      Optional ByVal aNeedToSave As Boolean = False) As atcTimeseriesSource
        Dim lFilesOnly As New ArrayList(1)
        lFilesOnly.Add("File")
        Dim lNewSource As atcDataSource = atcDataManager.UserSelectDataSource(lFilesOnly, "Select a File Type", aNeedToOpen, aNeedToSave)
        If Not lNewSource Is Nothing Then 'user did not cancel
            atcDataManager.OpenDataSource(lNewSource, lNewSource.Specification, Nothing)
        End If
        Return lNewSource
    End Function

    Private Sub btnSelect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelect.Click
        pTimeseriesGroup = atcDataManager.UserSelectData("Select Data", pTimeseriesGroup)
        FormFromGroup()
    End Sub

    Private Sub frmSelectDisplay_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyValue = Windows.Forms.Keys.F1 Then
            ShowHelp("BASINS Details\Project Creation and Management\GIS and Time-Series Data\Time-Series Management.html")
        End If
    End Sub
End Class
