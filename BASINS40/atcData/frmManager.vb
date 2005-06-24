Imports System.Windows.Forms

Imports atcUtility

Friend Class frmManager
  Inherits System.Windows.Forms.Form

  Private WithEvents pManager As atcDataManager

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
  Friend WithEvents MainMenu1 As System.Windows.Forms.MainMenu
  Friend WithEvents tbbOpen As System.Windows.Forms.ToolBarButton
  Friend WithEvents txtDetails As System.Windows.Forms.TextBox
  Friend WithEvents toolbarTop As System.Windows.Forms.ToolBar
  Friend WithEvents lstFiles As System.Windows.Forms.ListBox
  Friend WithEvents panelOpening As System.Windows.Forms.Panel
  Friend WithEvents lblOpening As System.Windows.Forms.Label
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
    Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmManager))
    Me.MainMenu1 = New System.Windows.Forms.MainMenu
    Me.toolbarTop = New System.Windows.Forms.ToolBar
    Me.tbbOpen = New System.Windows.Forms.ToolBarButton
    Me.txtDetails = New System.Windows.Forms.TextBox
    Me.lstFiles = New System.Windows.Forms.ListBox
    Me.panelOpening = New System.Windows.Forms.Panel
    Me.lblOpening = New System.Windows.Forms.Label
    Me.panelOpening.SuspendLayout()
    Me.SuspendLayout()
    '
    'toolbarTop
    '
    Me.toolbarTop.Buttons.AddRange(New System.Windows.Forms.ToolBarButton() {Me.tbbOpen})
    Me.toolbarTop.DropDownArrows = True
    Me.toolbarTop.Location = New System.Drawing.Point(0, 0)
    Me.toolbarTop.Name = "toolbarTop"
    Me.toolbarTop.ShowToolTips = True
    Me.toolbarTop.Size = New System.Drawing.Size(504, 42)
    Me.toolbarTop.TabIndex = 1
    '
    'tbbOpen
    '
    Me.tbbOpen.Text = "Open"
    Me.tbbOpen.ToolTipText = "Find new data sources"
    '
    'txtDetails
    '
    Me.txtDetails.BackColor = System.Drawing.SystemColors.ControlLight
    Me.txtDetails.Dock = System.Windows.Forms.DockStyle.Bottom
    Me.txtDetails.Location = New System.Drawing.Point(0, 237)
    Me.txtDetails.Multiline = True
    Me.txtDetails.Name = "txtDetails"
    Me.txtDetails.Size = New System.Drawing.Size(504, 72)
    Me.txtDetails.TabIndex = 2
    Me.txtDetails.Text = ""
    '
    'lstFiles
    '
    Me.lstFiles.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lstFiles.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lstFiles.IntegralHeight = False
    Me.lstFiles.ItemHeight = 20
    Me.lstFiles.Location = New System.Drawing.Point(0, 40)
    Me.lstFiles.Name = "lstFiles"
    Me.lstFiles.Size = New System.Drawing.Size(504, 192)
    Me.lstFiles.TabIndex = 3
    '
    'panelOpening
    '
    Me.panelOpening.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.panelOpening.Controls.Add(Me.lblOpening)
    Me.panelOpening.Location = New System.Drawing.Point(0, 0)
    Me.panelOpening.Name = "panelOpening"
    Me.panelOpening.Size = New System.Drawing.Size(504, 312)
    Me.panelOpening.TabIndex = 4
    Me.panelOpening.Visible = False
    '
    'lblOpening
    '
    Me.lblOpening.Location = New System.Drawing.Point(16, 40)
    Me.lblOpening.Name = "lblOpening"
    Me.lblOpening.Size = New System.Drawing.Size(480, 64)
    Me.lblOpening.TabIndex = 0
    Me.lblOpening.Text = "Opening..."
    '
    'frmManager
    '
    Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
    Me.ClientSize = New System.Drawing.Size(504, 309)
    Me.Controls.Add(Me.panelOpening)
    Me.Controls.Add(Me.lstFiles)
    Me.Controls.Add(Me.txtDetails)
    Me.Controls.Add(Me.toolbarTop)
    Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
    Me.Name = "frmManager"
    Me.Text = "Data Sources"
    Me.panelOpening.ResumeLayout(False)
    Me.ResumeLayout(False)

  End Sub

#End Region

  Public Sub Edit(ByVal aManager As atcDataManager)
    pManager = aManager
    lstFiles.Items.Clear()
    For Each source As atcDataSource In pManager.DataSources
      lstFiles.Items.Add(source.Name & " " & source.FileName & " (" & source.Timeseries.Count & ")")
    Next
    Me.Show()
  End Sub

  Private Sub toolbarTop_ButtonClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles toolbarTop.ButtonClick
    Select Case e.Button.Text
      Case "Open"
        panelOpening.Visible = True
        pManager.OpenData("")
        panelOpening.Visible = False
      Case "Close"
        'TODO: how do we remove a file from pManager.Files?
    End Select
  End Sub

  Private Sub pManager_OpenedData(ByVal aDataSource As atcDataSource) Handles pManager.OpenedData
    Edit(pManager)
  End Sub

  Private Sub lstFiles_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstFiles.SelectedIndexChanged
    Dim selectedItem As String = lstFiles.SelectedItem

    For Each source As atcDataSource In pManager.DataSources
      If selectedItem = source.Name & " " & source.FileName & " (" & source.Timeseries.Count & ")" Then
        txtDetails.Text = source.Name
        If source.FileName.Length > 0 Then txtDetails.Text &= vbCrLf & "File '" & source.FileName & "'"
        If source.Timeseries.Count > 0 Then
          txtDetails.Text &= vbCrLf & Format(source.Timeseries.Count, "#,###") & " Timeseries"
        End If
        If FileExists(source.FileName) Then
          txtDetails.Text &= vbCrLf & Format(FileLen(source.FileName), "#,###") & " bytes"
          txtDetails.Text &= vbCrLf & "Modified " & System.IO.File.GetLastWriteTime(source.FileName)
        End If
        Exit For
      End If
    Next
  End Sub
End Class
