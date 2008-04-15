Imports System.Windows.Forms
Imports atcUtility
Imports MapWinUtility

Friend Class frmManager
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()
        InitializeComponent()
        AddHandler atcDataManager.OpenedData, AddressOf OpenedData
    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            RemoveHandler atcDataManager.OpenedData, AddressOf OpenedData
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
    Friend WithEvents MainMenu1 As System.Windows.Forms.MainMenu
    Friend WithEvents txtDetails As System.Windows.Forms.TextBox
    Friend WithEvents btnOpen As System.Windows.Forms.Button
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents btnDisplay As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents lstFiles As System.Windows.Forms.ListBox
    'Friend WithEvents panelOpening As System.Windows.Forms.Panel
    'Friend WithEvents lstDataSourceType As System.Windows.Forms.ListBox
    'Friend WithEvents lblDataSourceType As System.Windows.Forms.Label
    'Friend WithEvents btnOpen As System.Windows.Forms.Button
    'Friend WithEvents btnCancel As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmManager))
        Me.MainMenu1 = New System.Windows.Forms.MainMenu(Me.components)
        Me.txtDetails = New System.Windows.Forms.TextBox
        Me.lstFiles = New System.Windows.Forms.ListBox
        Me.btnOpen = New System.Windows.Forms.Button
        Me.btnClose = New System.Windows.Forms.Button
        Me.btnDisplay = New System.Windows.Forms.Button
        Me.btnNew = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'txtDetails
        '
        Me.txtDetails.AllowDrop = True
        Me.txtDetails.BackColor = System.Drawing.SystemColors.ControlLight
        Me.txtDetails.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.txtDetails.Location = New System.Drawing.Point(0, 225)
        Me.txtDetails.Multiline = True
        Me.txtDetails.Name = "txtDetails"
        Me.txtDetails.Size = New System.Drawing.Size(504, 84)
        Me.txtDetails.TabIndex = 2
        '
        'lstFiles
        '
        Me.lstFiles.AllowDrop = True
        Me.lstFiles.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstFiles.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lstFiles.IntegralHeight = False
        Me.lstFiles.ItemHeight = 17
        Me.lstFiles.Location = New System.Drawing.Point(0, 40)
        Me.lstFiles.Name = "lstFiles"
        Me.lstFiles.Size = New System.Drawing.Size(504, 180)
        Me.lstFiles.TabIndex = 3
        '
        'btnOpen
        '
        Me.btnOpen.Location = New System.Drawing.Point(7, 7)
        Me.btnOpen.Name = "btnOpen"
        Me.btnOpen.Size = New System.Drawing.Size(90, 26)
        Me.btnOpen.TabIndex = 4
        Me.btnOpen.Text = "Open File..."
        Me.btnOpen.UseVisualStyleBackColor = True
        '
        'btnClose
        '
        Me.btnClose.Location = New System.Drawing.Point(199, 8)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(106, 26)
        Me.btnClose.TabIndex = 5
        Me.btnClose.Text = "Close Selected"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'btnDisplay
        '
        Me.btnDisplay.Location = New System.Drawing.Point(311, 7)
        Me.btnDisplay.Name = "btnDisplay"
        Me.btnDisplay.Size = New System.Drawing.Size(119, 26)
        Me.btnDisplay.TabIndex = 6
        Me.btnDisplay.Text = "Display Selected"
        Me.btnDisplay.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(103, 8)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(90, 26)
        Me.btnNew.TabIndex = 7
        Me.btnNew.Text = "New File..."
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'frmManager
        '
        Me.AllowDrop = True
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
        Me.ClientSize = New System.Drawing.Size(504, 309)
        Me.Controls.Add(Me.btnNew)
        Me.Controls.Add(Me.btnDisplay)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnOpen)
        Me.Controls.Add(Me.lstFiles)
        Me.Controls.Add(Me.txtDetails)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmManager"
        Me.Text = "Data Sources"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Public Sub Edit()
        lstFiles.Items.Clear()
        For Each lDataSource As atcDataSource In atcDataManager.DataSources
            lstFiles.Items.Add(lDataSource.Name & " " & lDataSource.Specification & " (" & lDataSource.DataSets.Count & ")")
        Next
        'lstFiles.SelectedIndex = lstFiles.Items.Count - 1
        Me.Show()
    End Sub

    Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpen.Click
        Dim lCollection As New ArrayList
        lCollection.Add("File")
        Dim lNewSource As atcDataSource = atcDataManager.UserSelectDataSource(lCollection)
        If Not lNewSource Is Nothing Then
            If (atcDataManager.OpenDataSource(lNewSource, lNewSource.Specification, Nothing)) Then
                lstFiles.SelectedIndex = lstFiles.Items.Count - 1
            Else
                If Logger.LastDbgText.Length > 0 Then
                    Logger.Msg(Logger.LastDbgText, "Data Open Problem")
                End If
            End If
        End If
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        Dim lFilesOnly As New ArrayList(1)
        lFilesOnly.Add("File")
        Dim lNewSource As atcDataSource = atcDataManager.UserSelectDataSource(lFilesOnly, "Select a File Type", False, True)
        If Not lNewSource Is Nothing Then 'user did not cancel
            If Not atcDataManager.OpenDataSource(lNewSource, lNewSource.Specification, Nothing) Then
                If Logger.LastDbgText.Length > 0 Then
                    Logger.Msg(Logger.LastDbgText, "Data New Problem")
                End If
            End If
        End If
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        'Skip In Memory data source at index zero
        Dim iSourceSelectedNext As Integer = 0
        For iSource As Integer = atcDataManager.DataSources.Count - 1 To 1 Step -1
            If lstFiles.SelectedIndices.Contains(iSource) Then
                Logger.Dbg("Close:" & CType(atcDataManager.DataSources.Item(iSource), atcDataSource).Specification)
                atcDataManager.DataSources.RemoveAt(iSource)
                lstFiles.Items.RemoveAt(iSource)
                iSourceSelectedNext = iSource - 1
            End If
        Next
        lstFiles.SelectedIndex = iSourceSelectedNext
    End Sub

    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        For iSource As Integer = atcDataManager.DataSources.Count - 1 To 0 Step -1
            If lstFiles.SelectedIndices.Contains(iSource) Then
                Dim lDataSource As atcDataSource = atcDataManager.DataSources.Item(iSource)
                Logger.Dbg("Display:" & lDataSource.Specification)
                atcDataManager.UserSelectDisplay(lDataSource.Specification, lDataSource.DataSets.Clone)
            End If
        Next
    End Sub

    Private Sub OpenedData(ByVal aDataSource As atcDataSource)
        Edit()
    End Sub

    Private Sub lstFiles_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstFiles.SelectedIndexChanged
        Dim selectedItem As String = lstFiles.SelectedItem

        For Each source As atcDataSource In atcDataManager.DataSources
            If selectedItem = source.Name & " " & source.Specification & " (" & source.DataSets.Count & ")" Then
                txtDetails.Text = source.Name
                If Not source.Specification Is Nothing AndAlso source.Specification.Length > 0 Then txtDetails.Text &= vbCrLf & source.Specification
                If source.DataSets.Count > 0 Then
                    txtDetails.Text &= vbCrLf & Format(source.DataSets.Count, "#,###") & " Timeseries"
                End If
                If FileExists(source.Specification) Then
                    txtDetails.Text &= vbCrLf & Format(FileLen(source.Specification), "#,###") & " bytes"
                    txtDetails.Text &= vbCrLf & "Modified " & System.IO.File.GetLastWriteTime(source.Specification)
                End If
                Exit For
            End If
        Next
    End Sub

    Protected Overrides Sub OnClosing(ByVal e As System.ComponentModel.CancelEventArgs)
        Debug.Write("Closing frmManager")
    End Sub

    Private Sub Form_DragEnter( _
        ByVal sender As Object, ByVal e As Windows.Forms.DragEventArgs) _
        Handles Me.DragEnter, lstFiles.DragEnter, txtDetails.DragEnter

        If e.Data.GetDataPresent(Windows.Forms.DataFormats.FileDrop) Then
            e.Effect = Windows.Forms.DragDropEffects.All
        End If
    End Sub

    Private Sub Form_DragDrop( _
        ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) _
        Handles Me.DragDrop, lstFiles.DragDrop, txtDetails.DragDrop

        If e.Data.GetDataPresent(Windows.Forms.DataFormats.FileDrop) Then
            Dim lFileNames() As String = e.Data.GetData(Windows.Forms.DataFormats.FileDrop)
            For Each lFileName As String In lFileNames
                Logger.Dbg("DroppedFile:" & lFileName)
                atcDataManager.OpenDataSource(lFileName)
            Next
        End If
    End Sub
End Class


