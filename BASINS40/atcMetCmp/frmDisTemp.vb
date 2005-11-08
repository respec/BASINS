Imports atcData

Public Class frmDisTemp
  Inherits System.Windows.Forms.Form

  Private pOk As Boolean
  Private pTMinTS As atcTimeseries
  Private pTMaxTS As atcTimeseries
  Private pDataManager As atcDataManager

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
  Friend WithEvents panelBottom As System.Windows.Forms.Panel
  Friend WithEvents btnCancel As System.Windows.Forms.Button
  Friend WithEvents btnOk As System.Windows.Forms.Button
  Friend WithEvents lblCloudCover As System.Windows.Forms.Label
  Friend WithEvents btnTMin As System.Windows.Forms.Button
  Friend WithEvents txtTMin As System.Windows.Forms.TextBox
  Friend WithEvents lblTMin As System.Windows.Forms.Label
  Friend WithEvents lblTMax As System.Windows.Forms.Label
  Friend WithEvents btnTMax As System.Windows.Forms.Button
  Friend WithEvents txtTMax As System.Windows.Forms.TextBox
  Friend WithEvents lblObsTime As System.Windows.Forms.Label
  Friend WithEvents txtObsTime As System.Windows.Forms.TextBox
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
    Me.lblCloudCover = New System.Windows.Forms.Label
    Me.lblObsTime = New System.Windows.Forms.Label
    Me.txtObsTime = New System.Windows.Forms.TextBox
    Me.panelBottom = New System.Windows.Forms.Panel
    Me.btnCancel = New System.Windows.Forms.Button
    Me.btnOk = New System.Windows.Forms.Button
    Me.btnTMin = New System.Windows.Forms.Button
    Me.txtTMin = New System.Windows.Forms.TextBox
    Me.lblTMin = New System.Windows.Forms.Label
    Me.lblTMax = New System.Windows.Forms.Label
    Me.btnTMax = New System.Windows.Forms.Button
    Me.txtTMax = New System.Windows.Forms.TextBox
    Me.panelBottom.SuspendLayout()
    Me.SuspendLayout()
    '
    'lblCloudCover
    '
    Me.lblCloudCover.Location = New System.Drawing.Point(16, 16)
    Me.lblCloudCover.Name = "lblCloudCover"
    Me.lblCloudCover.Size = New System.Drawing.Size(208, 16)
    Me.lblCloudCover.TabIndex = 2
    Me.lblCloudCover.Text = "Specify Daily Temperature Timeseries"
    '
    'lblObsTime
    '
    Me.lblObsTime.Location = New System.Drawing.Point(8, 104)
    Me.lblObsTime.Name = "lblObsTime"
    Me.lblObsTime.Size = New System.Drawing.Size(112, 16)
    Me.lblObsTime.TabIndex = 3
    Me.lblObsTime.Text = "Observation Hour:"
    '
    'txtObsTime
    '
    Me.txtObsTime.Location = New System.Drawing.Point(128, 104)
    Me.txtObsTime.Name = "txtObsTime"
    Me.txtObsTime.Size = New System.Drawing.Size(48, 20)
    Me.txtObsTime.TabIndex = 4
    Me.txtObsTime.Text = ""
    '
    'panelBottom
    '
    Me.panelBottom.Controls.Add(Me.btnCancel)
    Me.panelBottom.Controls.Add(Me.btnOk)
    Me.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom
    Me.panelBottom.Location = New System.Drawing.Point(0, 141)
    Me.panelBottom.Name = "panelBottom"
    Me.panelBottom.Size = New System.Drawing.Size(464, 32)
    Me.panelBottom.TabIndex = 16
    '
    'btnCancel
    '
    Me.btnCancel.Location = New System.Drawing.Point(240, 0)
    Me.btnCancel.Name = "btnCancel"
    Me.btnCancel.Size = New System.Drawing.Size(64, 24)
    Me.btnCancel.TabIndex = 1
    Me.btnCancel.Text = "Cancel"
    '
    'btnOk
    '
    Me.btnOk.Location = New System.Drawing.Point(152, 0)
    Me.btnOk.Name = "btnOk"
    Me.btnOk.Size = New System.Drawing.Size(64, 24)
    Me.btnOk.TabIndex = 0
    Me.btnOk.Text = "Ok"
    '
    'btnTMin
    '
    Me.btnTMin.Location = New System.Drawing.Point(72, 40)
    Me.btnTMin.Name = "btnTMin"
    Me.btnTMin.Size = New System.Drawing.Size(48, 24)
    Me.btnTMin.TabIndex = 18
    Me.btnTMin.Text = "Select"
    '
    'txtTMin
    '
    Me.txtTMin.Location = New System.Drawing.Point(128, 40)
    Me.txtTMin.Name = "txtTMin"
    Me.txtTMin.ReadOnly = True
    Me.txtTMin.Size = New System.Drawing.Size(328, 20)
    Me.txtTMin.TabIndex = 19
    Me.txtTMin.Text = ""
    '
    'lblTMin
    '
    Me.lblTMin.Location = New System.Drawing.Point(8, 40)
    Me.lblTMin.Name = "lblTMin"
    Me.lblTMin.Size = New System.Drawing.Size(64, 16)
    Me.lblTMin.TabIndex = 20
    Me.lblTMin.Text = "Min Temp:"
    '
    'lblTMax
    '
    Me.lblTMax.Location = New System.Drawing.Point(8, 72)
    Me.lblTMax.Name = "lblTMax"
    Me.lblTMax.Size = New System.Drawing.Size(64, 16)
    Me.lblTMax.TabIndex = 21
    Me.lblTMax.Text = "Max Temp:"
    '
    'btnTMax
    '
    Me.btnTMax.Location = New System.Drawing.Point(72, 71)
    Me.btnTMax.Name = "btnTMax"
    Me.btnTMax.Size = New System.Drawing.Size(48, 23)
    Me.btnTMax.TabIndex = 22
    Me.btnTMax.Text = "Select"
    '
    'txtTMax
    '
    Me.txtTMax.Location = New System.Drawing.Point(128, 72)
    Me.txtTMax.Name = "txtTMax"
    Me.txtTMax.ReadOnly = True
    Me.txtTMax.Size = New System.Drawing.Size(328, 20)
    Me.txtTMax.TabIndex = 23
    Me.txtTMax.Text = ""
    '
    'frmDisTemp
    '
    Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
    Me.ClientSize = New System.Drawing.Size(464, 173)
    Me.Controls.Add(Me.txtTMax)
    Me.Controls.Add(Me.btnTMax)
    Me.Controls.Add(Me.lblTMax)
    Me.Controls.Add(Me.lblTMin)
    Me.Controls.Add(Me.txtTMin)
    Me.Controls.Add(Me.btnTMin)
    Me.Controls.Add(Me.panelBottom)
    Me.Controls.Add(Me.txtObsTime)
    Me.Controls.Add(Me.lblObsTime)
    Me.Controls.Add(Me.lblCloudCover)
    Me.Name = "frmDisTemp"
    Me.Text = "Specify Temperature Disaggregation Inputs"
    Me.panelBottom.ResumeLayout(False)
    Me.ResumeLayout(False)

  End Sub

#End Region
  Public Function AskUser(ByVal aDataManager As atcDataManager, ByRef aTMinTS As atcTimeseries, ByRef aTMaxTS As atcTimeseries, ByRef aObsTime As Integer) As Boolean
    pDataManager = aDataManager
    Me.ShowDialog()
    If pOk Then
      aTMinTS = pTMinTS
      aTMaxTS = pTMaxTS
      aObsTime = CInt(txtObsTime.Text)
    End If
    Return pOk
  End Function

  Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
    pOk = True
    Close()
  End Sub

  Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
    Close()
  End Sub

  Private Sub btnTMin_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTMin.Click
    Dim lTSGroup As atcDataGroup = pDataManager.UserSelectData("Select data for Daily Min Temperature")
    If lTSGroup.Count > 0 Then
      pTMinTS = lTSGroup(0)
      txtTMin.Text = pTMinTS.ToString
    End If
  End Sub

  Private Sub btnTMax_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTMax.Click
    Dim lTSGroup As atcDataGroup = pDataManager.UserSelectData("Select data for Daily Max Temperature")
    If lTSGroup.Count > 0 Then
      pTMaxTS = lTSGroup(0)
      txtTMax.Text = pTMaxTS.ToString
    End If
  End Sub

End Class
