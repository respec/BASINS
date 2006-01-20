Imports atcData
Imports MapWinUtility

Public Class frmCmpPenman
  Inherits System.Windows.Forms.Form

  Private pOk As Boolean
  Private pTMinTS As atcTimeseries
  Private pTMaxTS As atcTimeseries
  Private pSRadTS As atcTimeseries
  Private pDewPTS As atcTimeseries
  Private pWindTS As atcTimeseries
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
  Friend WithEvents btnTMin As System.Windows.Forms.Button
  Friend WithEvents txtTMin As System.Windows.Forms.TextBox
  Friend WithEvents lblTMin As System.Windows.Forms.Label
  Friend WithEvents lblTMax As System.Windows.Forms.Label
  Friend WithEvents btnTMax As System.Windows.Forms.Button
  Friend WithEvents txtTMax As System.Windows.Forms.TextBox
  Friend WithEvents lblJensenPET As System.Windows.Forms.Label
  Friend WithEvents lblSRad As System.Windows.Forms.Label
  Friend WithEvents btnSRad As System.Windows.Forms.Button
  Friend WithEvents txtSRad As System.Windows.Forms.TextBox
  Friend WithEvents lblDewP As System.Windows.Forms.Label
  Friend WithEvents lblWind As System.Windows.Forms.Label
  Friend WithEvents btnDewP As System.Windows.Forms.Button
  Friend WithEvents btnWind As System.Windows.Forms.Button
  Friend WithEvents txtDewP As System.Windows.Forms.TextBox
  Friend WithEvents txtWind As System.Windows.Forms.TextBox
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
    Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmCmpPenman))
    Me.lblJensenPET = New System.Windows.Forms.Label
    Me.panelBottom = New System.Windows.Forms.Panel
    Me.btnCancel = New System.Windows.Forms.Button
    Me.btnOk = New System.Windows.Forms.Button
    Me.btnTMin = New System.Windows.Forms.Button
    Me.txtTMin = New System.Windows.Forms.TextBox
    Me.lblTMin = New System.Windows.Forms.Label
    Me.lblTMax = New System.Windows.Forms.Label
    Me.btnTMax = New System.Windows.Forms.Button
    Me.txtTMax = New System.Windows.Forms.TextBox
    Me.lblSRad = New System.Windows.Forms.Label
    Me.btnSRad = New System.Windows.Forms.Button
    Me.txtSRad = New System.Windows.Forms.TextBox
    Me.lblDewP = New System.Windows.Forms.Label
    Me.lblWind = New System.Windows.Forms.Label
    Me.btnDewP = New System.Windows.Forms.Button
    Me.btnWind = New System.Windows.Forms.Button
    Me.txtDewP = New System.Windows.Forms.TextBox
    Me.txtWind = New System.Windows.Forms.TextBox
    Me.panelBottom.SuspendLayout()
    Me.SuspendLayout()
    '
    'lblJensenPET
    '
    Me.lblJensenPET.Location = New System.Drawing.Point(16, 16)
    Me.lblJensenPET.Name = "lblJensenPET"
    Me.lblJensenPET.Size = New System.Drawing.Size(144, 16)
    Me.lblJensenPET.TabIndex = 2
    Me.lblJensenPET.Text = "Specify Input Timeseries"
    '
    'panelBottom
    '
    Me.panelBottom.Controls.Add(Me.btnCancel)
    Me.panelBottom.Controls.Add(Me.btnOk)
    Me.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom
    Me.panelBottom.Location = New System.Drawing.Point(0, 205)
    Me.panelBottom.Name = "panelBottom"
    Me.panelBottom.Size = New System.Drawing.Size(504, 32)
    Me.panelBottom.TabIndex = 16
    '
    'btnCancel
    '
    Me.btnCancel.Location = New System.Drawing.Point(256, 0)
    Me.btnCancel.Name = "btnCancel"
    Me.btnCancel.Size = New System.Drawing.Size(64, 24)
    Me.btnCancel.TabIndex = 1
    Me.btnCancel.Text = "Cancel"
    '
    'btnOk
    '
    Me.btnOk.Location = New System.Drawing.Point(160, 0)
    Me.btnOk.Name = "btnOk"
    Me.btnOk.Size = New System.Drawing.Size(64, 24)
    Me.btnOk.TabIndex = 0
    Me.btnOk.Text = "Ok"
    '
    'btnTMin
    '
    Me.btnTMin.Location = New System.Drawing.Point(104, 40)
    Me.btnTMin.Name = "btnTMin"
    Me.btnTMin.Size = New System.Drawing.Size(48, 20)
    Me.btnTMin.TabIndex = 18
    Me.btnTMin.Text = "Select"
    '
    'txtTMin
    '
    Me.txtTMin.Location = New System.Drawing.Point(160, 40)
    Me.txtTMin.Name = "txtTMin"
    Me.txtTMin.ReadOnly = True
    Me.txtTMin.Size = New System.Drawing.Size(344, 20)
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
    Me.btnTMax.Location = New System.Drawing.Point(104, 72)
    Me.btnTMax.Name = "btnTMax"
    Me.btnTMax.Size = New System.Drawing.Size(48, 20)
    Me.btnTMax.TabIndex = 22
    Me.btnTMax.Text = "Select"
    '
    'txtTMax
    '
    Me.txtTMax.Location = New System.Drawing.Point(160, 72)
    Me.txtTMax.Name = "txtTMax"
    Me.txtTMax.ReadOnly = True
    Me.txtTMax.Size = New System.Drawing.Size(344, 20)
    Me.txtTMax.TabIndex = 23
    Me.txtTMax.Text = ""
    '
    'lblSRad
    '
    Me.lblSRad.Location = New System.Drawing.Point(8, 104)
    Me.lblSRad.Name = "lblSRad"
    Me.lblSRad.Size = New System.Drawing.Size(96, 16)
    Me.lblSRad.TabIndex = 51
    Me.lblSRad.Text = "Solar Radiation:"
    '
    'btnSRad
    '
    Me.btnSRad.Location = New System.Drawing.Point(104, 104)
    Me.btnSRad.Name = "btnSRad"
    Me.btnSRad.Size = New System.Drawing.Size(48, 20)
    Me.btnSRad.TabIndex = 52
    Me.btnSRad.Text = "Select"
    '
    'txtSRad
    '
    Me.txtSRad.Location = New System.Drawing.Point(160, 104)
    Me.txtSRad.Name = "txtSRad"
    Me.txtSRad.ReadOnly = True
    Me.txtSRad.Size = New System.Drawing.Size(344, 20)
    Me.txtSRad.TabIndex = 53
    Me.txtSRad.Text = ""
    '
    'lblDewP
    '
    Me.lblDewP.Location = New System.Drawing.Point(8, 136)
    Me.lblDewP.Name = "lblDewP"
    Me.lblDewP.Size = New System.Drawing.Size(96, 16)
    Me.lblDewP.TabIndex = 54
    Me.lblDewP.Text = "Dewpoint Temp:"
    '
    'lblWind
    '
    Me.lblWind.Location = New System.Drawing.Point(8, 168)
    Me.lblWind.Name = "lblWind"
    Me.lblWind.Size = New System.Drawing.Size(96, 16)
    Me.lblWind.TabIndex = 55
    Me.lblWind.Text = "Wind Movement:"
    '
    'btnDewP
    '
    Me.btnDewP.Location = New System.Drawing.Point(104, 136)
    Me.btnDewP.Name = "btnDewP"
    Me.btnDewP.Size = New System.Drawing.Size(48, 20)
    Me.btnDewP.TabIndex = 56
    Me.btnDewP.Text = "Select"
    '
    'btnWind
    '
    Me.btnWind.Location = New System.Drawing.Point(104, 168)
    Me.btnWind.Name = "btnWind"
    Me.btnWind.Size = New System.Drawing.Size(48, 20)
    Me.btnWind.TabIndex = 57
    Me.btnWind.Text = "Select"
    '
    'txtDewP
    '
    Me.txtDewP.Location = New System.Drawing.Point(160, 136)
    Me.txtDewP.Name = "txtDewP"
    Me.txtDewP.ReadOnly = True
    Me.txtDewP.Size = New System.Drawing.Size(344, 20)
    Me.txtDewP.TabIndex = 58
    Me.txtDewP.Text = ""
    '
    'txtWind
    '
    Me.txtWind.Location = New System.Drawing.Point(160, 168)
    Me.txtWind.Name = "txtWind"
    Me.txtWind.ReadOnly = True
    Me.txtWind.Size = New System.Drawing.Size(344, 20)
    Me.txtWind.TabIndex = 59
    Me.txtWind.Text = ""
    '
    'frmCmpPenman
    '
    Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
    Me.ClientSize = New System.Drawing.Size(504, 237)
    Me.Controls.Add(Me.txtWind)
    Me.Controls.Add(Me.txtDewP)
    Me.Controls.Add(Me.btnWind)
    Me.Controls.Add(Me.btnDewP)
    Me.Controls.Add(Me.lblWind)
    Me.Controls.Add(Me.lblDewP)
    Me.Controls.Add(Me.txtSRad)
    Me.Controls.Add(Me.btnSRad)
    Me.Controls.Add(Me.lblSRad)
    Me.Controls.Add(Me.txtTMax)
    Me.Controls.Add(Me.btnTMax)
    Me.Controls.Add(Me.lblTMax)
    Me.Controls.Add(Me.lblTMin)
    Me.Controls.Add(Me.txtTMin)
    Me.Controls.Add(Me.btnTMin)
    Me.Controls.Add(Me.panelBottom)
    Me.Controls.Add(Me.lblJensenPET)
    Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
    Me.Name = "frmCmpPenman"
    Me.Text = "Compute Penman Pan Evaporation"
    Me.panelBottom.ResumeLayout(False)
    Me.ResumeLayout(False)

  End Sub

#End Region
  Public Function AskUser(ByVal aDataManager As atcDataManager, ByRef aTMinTS As atcTimeseries, ByRef aTMaxTS As atcTimeseries, ByRef aSRadTS As atcTimeseries, ByRef aDewPTS As atcTimeseries, ByRef aWindTS As atcTimeseries) As Boolean
    pDataManager = aDataManager
    Me.ShowDialog()
    If pOk Then
      aTMinTS = pTMinTS
      aTMaxTS = pTMaxTS
      aSRadTS = pSRadTS
      aDewPTS = pDewPTS
      aWindTS = pWindTS
    End If
    Return pOk
  End Function

  Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
    If Not pTMinTS Is Nothing And Not pTMaxTS Is Nothing And Not pSRadTS Is Nothing And _
       Not pDewPTS Is Nothing And Not pWindTS Is Nothing Then
      pOk = True
      Close()
    Else
      Logger.Msg("No Timeseries selected for 'Min Temp', 'Max Temp'" & _
                 "'Solar Radiation', 'Dewpoint Temp', or 'Wind Movement'." & vbCrLf & _
                 "Use 'Select' buttons to specify the timeseries", Me.Text & " Problem")
    End If
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

  Private Sub btnSRad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSRad.Click
    Dim lTSGroup As atcDataGroup = pDataManager.UserSelectData("Select data for Solar Radiation")
    If lTSGroup.Count > 0 Then
      pSRadTS = lTSGroup(0)
      txtSRad.Text = pSRadTS.ToString
    End If
  End Sub

  Private Sub btnDewP_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDewP.Click
    Dim lTSGroup As atcDataGroup = pDataManager.UserSelectData("Select data for Daily Dewpoint Temperature")
    If lTSGroup.Count > 0 Then
      pDewPTS = lTSGroup(0)
      txtDewP.Text = pDewPTS.ToString
    End If
  End Sub

  Private Sub btnWind_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnWind.Click
    Dim lTSGroup As atcDataGroup = pDataManager.UserSelectData("Select data for Daily Wind Movement (miles)")
    If lTSGroup.Count > 0 Then
      pWindTS = lTSGroup(0)
      txtWind.Text = pWindTS.ToString
    End If
  End Sub
End Class
