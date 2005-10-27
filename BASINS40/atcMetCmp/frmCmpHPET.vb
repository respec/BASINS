Imports atcData

Public Class frmCmpHPET
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
  Friend WithEvents lblLatitude As System.Windows.Forms.Label
  Friend WithEvents txtLatitude As System.Windows.Forms.TextBox
  Friend WithEvents lblCloudCover As System.Windows.Forms.Label
  Friend WithEvents btnTMin As System.Windows.Forms.Button
  Friend WithEvents txtTMin As System.Windows.Forms.TextBox
  Friend WithEvents lblTMin As System.Windows.Forms.Label
  Friend WithEvents lblTMax As System.Windows.Forms.Label
  Friend WithEvents btnTMax As System.Windows.Forms.Button
  Friend WithEvents txtTMax As System.Windows.Forms.TextBox
  Friend WithEvents rdoDegF As System.Windows.Forms.RadioButton
  Friend WithEvents rdoDegC As System.Windows.Forms.RadioButton
  Friend WithEvents lblMonCoeff As System.Windows.Forms.Label
  Friend WithEvents lblJan As System.Windows.Forms.Label
  Friend WithEvents lblFeb As System.Windows.Forms.Label
  Friend WithEvents lblMar As System.Windows.Forms.Label
  Friend WithEvents lblApr As System.Windows.Forms.Label
  Friend WithEvents lblMay As System.Windows.Forms.Label
  Friend WithEvents lblJun As System.Windows.Forms.Label
  Friend WithEvents lblJul As System.Windows.Forms.Label
  Friend WithEvents lblAug As System.Windows.Forms.Label
  Friend WithEvents lblSep As System.Windows.Forms.Label
  Friend WithEvents lblOct As System.Windows.Forms.Label
  Friend WithEvents lblNov As System.Windows.Forms.Label
  Friend WithEvents lblDec As System.Windows.Forms.Label
  Friend WithEvents txtJan As System.Windows.Forms.TextBox
  Friend WithEvents txtFeb As System.Windows.Forms.TextBox
  Friend WithEvents txtMar As System.Windows.Forms.TextBox
  Friend WithEvents txtApr As System.Windows.Forms.TextBox
  Friend WithEvents txtMay As System.Windows.Forms.TextBox
  Friend WithEvents txtJun As System.Windows.Forms.TextBox
  Friend WithEvents txtJul As System.Windows.Forms.TextBox
  Friend WithEvents txtAug As System.Windows.Forms.TextBox
  Friend WithEvents txtSep As System.Windows.Forms.TextBox
  Friend WithEvents txtOct As System.Windows.Forms.TextBox
  Friend WithEvents txtNov As System.Windows.Forms.TextBox
  Friend WithEvents txtDec As System.Windows.Forms.TextBox
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
    Me.lblCloudCover = New System.Windows.Forms.Label
    Me.lblLatitude = New System.Windows.Forms.Label
    Me.txtLatitude = New System.Windows.Forms.TextBox
    Me.panelBottom = New System.Windows.Forms.Panel
    Me.btnCancel = New System.Windows.Forms.Button
    Me.btnOk = New System.Windows.Forms.Button
    Me.btnTMin = New System.Windows.Forms.Button
    Me.txtTMin = New System.Windows.Forms.TextBox
    Me.lblTMin = New System.Windows.Forms.Label
    Me.lblTMax = New System.Windows.Forms.Label
    Me.btnTMax = New System.Windows.Forms.Button
    Me.txtTMax = New System.Windows.Forms.TextBox
    Me.rdoDegF = New System.Windows.Forms.RadioButton
    Me.rdoDegC = New System.Windows.Forms.RadioButton
    Me.lblMonCoeff = New System.Windows.Forms.Label
    Me.lblJan = New System.Windows.Forms.Label
    Me.lblFeb = New System.Windows.Forms.Label
    Me.lblMar = New System.Windows.Forms.Label
    Me.lblApr = New System.Windows.Forms.Label
    Me.lblMay = New System.Windows.Forms.Label
    Me.lblJun = New System.Windows.Forms.Label
    Me.lblJul = New System.Windows.Forms.Label
    Me.lblAug = New System.Windows.Forms.Label
    Me.lblSep = New System.Windows.Forms.Label
    Me.lblOct = New System.Windows.Forms.Label
    Me.lblNov = New System.Windows.Forms.Label
    Me.lblDec = New System.Windows.Forms.Label
    Me.txtJan = New System.Windows.Forms.TextBox
    Me.txtFeb = New System.Windows.Forms.TextBox
    Me.txtMar = New System.Windows.Forms.TextBox
    Me.txtApr = New System.Windows.Forms.TextBox
    Me.txtMay = New System.Windows.Forms.TextBox
    Me.txtJun = New System.Windows.Forms.TextBox
    Me.txtJul = New System.Windows.Forms.TextBox
    Me.txtAug = New System.Windows.Forms.TextBox
    Me.txtSep = New System.Windows.Forms.TextBox
    Me.txtOct = New System.Windows.Forms.TextBox
    Me.txtNov = New System.Windows.Forms.TextBox
    Me.txtDec = New System.Windows.Forms.TextBox
    Me.panelBottom.SuspendLayout()
    Me.SuspendLayout()
    '
    'lblCloudCover
    '
    Me.lblCloudCover.Location = New System.Drawing.Point(16, 16)
    Me.lblCloudCover.Name = "lblCloudCover"
    Me.lblCloudCover.Size = New System.Drawing.Size(144, 16)
    Me.lblCloudCover.TabIndex = 2
    Me.lblCloudCover.Text = "Specify Input Timeseries"
    '
    'lblLatitude
    '
    Me.lblLatitude.Location = New System.Drawing.Point(8, 104)
    Me.lblLatitude.Name = "lblLatitude"
    Me.lblLatitude.Size = New System.Drawing.Size(144, 16)
    Me.lblLatitude.TabIndex = 3
    Me.lblLatitude.Text = "Latitude (decimal degress):"
    '
    'txtLatitude
    '
    Me.txtLatitude.Location = New System.Drawing.Point(152, 104)
    Me.txtLatitude.Name = "txtLatitude"
    Me.txtLatitude.Size = New System.Drawing.Size(72, 20)
    Me.txtLatitude.TabIndex = 4
    Me.txtLatitude.Text = ""
    '
    'panelBottom
    '
    Me.panelBottom.Controls.Add(Me.btnCancel)
    Me.panelBottom.Controls.Add(Me.btnOk)
    Me.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom
    Me.panelBottom.Location = New System.Drawing.Point(0, 205)
    Me.panelBottom.Name = "panelBottom"
    Me.panelBottom.Size = New System.Drawing.Size(496, 32)
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
    Me.txtTMin.Size = New System.Drawing.Size(360, 20)
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
    Me.txtTMax.Size = New System.Drawing.Size(360, 20)
    Me.txtTMax.TabIndex = 23
    Me.txtTMax.Text = ""
    '
    'rdoDegF
    '
    Me.rdoDegF.Checked = True
    Me.rdoDegF.Location = New System.Drawing.Point(272, 112)
    Me.rdoDegF.Name = "rdoDegF"
    Me.rdoDegF.Size = New System.Drawing.Size(96, 16)
    Me.rdoDegF.TabIndex = 24
    Me.rdoDegF.TabStop = True
    Me.rdoDegF.Text = "Degrees F"
    '
    'rdoDegC
    '
    Me.rdoDegC.Location = New System.Drawing.Point(360, 112)
    Me.rdoDegC.Name = "rdoDegC"
    Me.rdoDegC.Size = New System.Drawing.Size(96, 16)
    Me.rdoDegC.TabIndex = 25
    Me.rdoDegC.Text = "Degrees C"
    '
    'lblMonCoeff
    '
    Me.lblMonCoeff.Location = New System.Drawing.Point(8, 128)
    Me.lblMonCoeff.Name = "lblMonCoeff"
    Me.lblMonCoeff.Size = New System.Drawing.Size(160, 16)
    Me.lblMonCoeff.TabIndex = 26
    Me.lblMonCoeff.Text = "Specify Monthly Coefficients"
    '
    'lblJan
    '
    Me.lblJan.Location = New System.Drawing.Point(16, 152)
    Me.lblJan.Name = "lblJan"
    Me.lblJan.Size = New System.Drawing.Size(24, 16)
    Me.lblJan.TabIndex = 27
    Me.lblJan.Text = "Jan"
    '
    'lblFeb
    '
    Me.lblFeb.Location = New System.Drawing.Point(56, 152)
    Me.lblFeb.Name = "lblFeb"
    Me.lblFeb.Size = New System.Drawing.Size(24, 16)
    Me.lblFeb.TabIndex = 28
    Me.lblFeb.Text = "Feb"
    '
    'lblMar
    '
    Me.lblMar.Location = New System.Drawing.Point(96, 152)
    Me.lblMar.Name = "lblMar"
    Me.lblMar.Size = New System.Drawing.Size(24, 16)
    Me.lblMar.TabIndex = 29
    Me.lblMar.Text = "Mar"
    '
    'lblApr
    '
    Me.lblApr.Location = New System.Drawing.Point(136, 152)
    Me.lblApr.Name = "lblApr"
    Me.lblApr.Size = New System.Drawing.Size(24, 16)
    Me.lblApr.TabIndex = 30
    Me.lblApr.Text = "Apr"
    '
    'lblMay
    '
    Me.lblMay.Location = New System.Drawing.Point(176, 152)
    Me.lblMay.Name = "lblMay"
    Me.lblMay.Size = New System.Drawing.Size(32, 16)
    Me.lblMay.TabIndex = 31
    Me.lblMay.Text = "May"
    '
    'lblJun
    '
    Me.lblJun.Location = New System.Drawing.Point(216, 152)
    Me.lblJun.Name = "lblJun"
    Me.lblJun.Size = New System.Drawing.Size(24, 16)
    Me.lblJun.TabIndex = 32
    Me.lblJun.Text = "Jun"
    '
    'lblJul
    '
    Me.lblJul.Location = New System.Drawing.Point(256, 152)
    Me.lblJul.Name = "lblJul"
    Me.lblJul.Size = New System.Drawing.Size(24, 16)
    Me.lblJul.TabIndex = 33
    Me.lblJul.Text = "Jul"
    '
    'lblAug
    '
    Me.lblAug.Location = New System.Drawing.Point(296, 152)
    Me.lblAug.Name = "lblAug"
    Me.lblAug.Size = New System.Drawing.Size(24, 16)
    Me.lblAug.TabIndex = 34
    Me.lblAug.Text = "Aug"
    '
    'lblSep
    '
    Me.lblSep.Location = New System.Drawing.Point(336, 152)
    Me.lblSep.Name = "lblSep"
    Me.lblSep.Size = New System.Drawing.Size(24, 16)
    Me.lblSep.TabIndex = 35
    Me.lblSep.Text = "Sep"
    '
    'lblOct
    '
    Me.lblOct.Location = New System.Drawing.Point(376, 152)
    Me.lblOct.Name = "lblOct"
    Me.lblOct.Size = New System.Drawing.Size(24, 16)
    Me.lblOct.TabIndex = 36
    Me.lblOct.Text = "Oct"
    '
    'lblNov
    '
    Me.lblNov.Location = New System.Drawing.Point(416, 152)
    Me.lblNov.Name = "lblNov"
    Me.lblNov.Size = New System.Drawing.Size(24, 16)
    Me.lblNov.TabIndex = 37
    Me.lblNov.Text = "Nov"
    '
    'lblDec
    '
    Me.lblDec.Location = New System.Drawing.Point(456, 152)
    Me.lblDec.Name = "lblDec"
    Me.lblDec.Size = New System.Drawing.Size(24, 16)
    Me.lblDec.TabIndex = 38
    Me.lblDec.Text = "Dec"
    '
    'txtJan
    '
    Me.txtJan.Location = New System.Drawing.Point(8, 168)
    Me.txtJan.Name = "txtJan"
    Me.txtJan.Size = New System.Drawing.Size(40, 20)
    Me.txtJan.TabIndex = 39
    Me.txtJan.Text = "0.0055"
    '
    'txtFeb
    '
    Me.txtFeb.Location = New System.Drawing.Point(48, 168)
    Me.txtFeb.Name = "txtFeb"
    Me.txtFeb.Size = New System.Drawing.Size(40, 20)
    Me.txtFeb.TabIndex = 40
    Me.txtFeb.Text = "0.0055"
    '
    'txtMar
    '
    Me.txtMar.Location = New System.Drawing.Point(88, 168)
    Me.txtMar.Name = "txtMar"
    Me.txtMar.Size = New System.Drawing.Size(40, 20)
    Me.txtMar.TabIndex = 41
    Me.txtMar.Text = "0.0055"
    '
    'txtApr
    '
    Me.txtApr.Location = New System.Drawing.Point(128, 168)
    Me.txtApr.Name = "txtApr"
    Me.txtApr.Size = New System.Drawing.Size(40, 20)
    Me.txtApr.TabIndex = 42
    Me.txtApr.Text = "0.0055"
    '
    'txtMay
    '
    Me.txtMay.Location = New System.Drawing.Point(168, 168)
    Me.txtMay.Name = "txtMay"
    Me.txtMay.Size = New System.Drawing.Size(40, 20)
    Me.txtMay.TabIndex = 43
    Me.txtMay.Text = "0.0055"
    '
    'txtJun
    '
    Me.txtJun.Location = New System.Drawing.Point(208, 168)
    Me.txtJun.Name = "txtJun"
    Me.txtJun.Size = New System.Drawing.Size(40, 20)
    Me.txtJun.TabIndex = 44
    Me.txtJun.Text = "0.0055"
    '
    'txtJul
    '
    Me.txtJul.Location = New System.Drawing.Point(248, 168)
    Me.txtJul.Name = "txtJul"
    Me.txtJul.Size = New System.Drawing.Size(40, 20)
    Me.txtJul.TabIndex = 45
    Me.txtJul.Text = "0.0055"
    '
    'txtAug
    '
    Me.txtAug.Location = New System.Drawing.Point(288, 168)
    Me.txtAug.Name = "txtAug"
    Me.txtAug.Size = New System.Drawing.Size(40, 20)
    Me.txtAug.TabIndex = 46
    Me.txtAug.Text = "0.0055"
    '
    'txtSep
    '
    Me.txtSep.Location = New System.Drawing.Point(328, 168)
    Me.txtSep.Name = "txtSep"
    Me.txtSep.Size = New System.Drawing.Size(40, 20)
    Me.txtSep.TabIndex = 47
    Me.txtSep.Text = "0.0055"
    '
    'txtOct
    '
    Me.txtOct.Location = New System.Drawing.Point(368, 168)
    Me.txtOct.Name = "txtOct"
    Me.txtOct.Size = New System.Drawing.Size(40, 20)
    Me.txtOct.TabIndex = 48
    Me.txtOct.Text = "0.0055"
    '
    'txtNov
    '
    Me.txtNov.Location = New System.Drawing.Point(408, 168)
    Me.txtNov.Name = "txtNov"
    Me.txtNov.Size = New System.Drawing.Size(40, 20)
    Me.txtNov.TabIndex = 49
    Me.txtNov.Text = "0.0055"
    '
    'txtDec
    '
    Me.txtDec.Location = New System.Drawing.Point(448, 168)
    Me.txtDec.Name = "txtDec"
    Me.txtDec.Size = New System.Drawing.Size(40, 20)
    Me.txtDec.TabIndex = 50
    Me.txtDec.Text = "0.0055"
    '
    'frmCmpHPET
    '
    Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
    Me.ClientSize = New System.Drawing.Size(496, 237)
    Me.Controls.Add(Me.txtDec)
    Me.Controls.Add(Me.txtNov)
    Me.Controls.Add(Me.txtOct)
    Me.Controls.Add(Me.txtSep)
    Me.Controls.Add(Me.txtAug)
    Me.Controls.Add(Me.txtJul)
    Me.Controls.Add(Me.txtJun)
    Me.Controls.Add(Me.txtMay)
    Me.Controls.Add(Me.txtApr)
    Me.Controls.Add(Me.txtMar)
    Me.Controls.Add(Me.txtFeb)
    Me.Controls.Add(Me.txtJan)
    Me.Controls.Add(Me.lblDec)
    Me.Controls.Add(Me.lblNov)
    Me.Controls.Add(Me.lblOct)
    Me.Controls.Add(Me.lblSep)
    Me.Controls.Add(Me.lblAug)
    Me.Controls.Add(Me.lblJul)
    Me.Controls.Add(Me.lblJun)
    Me.Controls.Add(Me.lblMay)
    Me.Controls.Add(Me.lblApr)
    Me.Controls.Add(Me.lblMar)
    Me.Controls.Add(Me.lblFeb)
    Me.Controls.Add(Me.lblJan)
    Me.Controls.Add(Me.lblMonCoeff)
    Me.Controls.Add(Me.rdoDegC)
    Me.Controls.Add(Me.rdoDegF)
    Me.Controls.Add(Me.txtTMax)
    Me.Controls.Add(Me.btnTMax)
    Me.Controls.Add(Me.lblTMax)
    Me.Controls.Add(Me.lblTMin)
    Me.Controls.Add(Me.txtTMin)
    Me.Controls.Add(Me.btnTMin)
    Me.Controls.Add(Me.panelBottom)
    Me.Controls.Add(Me.txtLatitude)
    Me.Controls.Add(Me.lblLatitude)
    Me.Controls.Add(Me.lblCloudCover)
    Me.Name = "frmCmpHPET"
    Me.Text = "Specify Hamon PET Inputs"
    Me.panelBottom.ResumeLayout(False)
    Me.ResumeLayout(False)

  End Sub

#End Region
  Public Function AskUser(ByVal aDataManager As atcDataManager, ByRef aTMinTS As atcTimeseries, ByRef aTMaxTS As atcTimeseries, ByRef aDegF As Boolean, ByRef aLatitude As Double, ByRef aCTS() As Double) As Boolean
    pDataManager = aDataManager
    Me.ShowDialog()
    If pOk Then
      aTMinTS = pTMinTS
      aTMaxTS = pTMaxTS
      aDegF = rdoDegF.Checked
      aLatitude = CDbl(txtLatitude.Text)
      aCTS(1) = CDbl(txtJan.Text)
      aCTS(2) = CDbl(txtFeb.Text)
      aCTS(3) = CDbl(txtMar.Text)
      aCTS(4) = CDbl(txtApr.Text)
      aCTS(5) = CDbl(txtMay.Text)
      aCTS(6) = CDbl(txtJun.Text)
      aCTS(7) = CDbl(txtJul.Text)
      aCTS(8) = CDbl(txtAug.Text)
      aCTS(9) = CDbl(txtSep.Text)
      aCTS(10) = CDbl(txtOct.Text)
      aCTS(11) = CDbl(txtNov.Text)
      aCTS(12) = CDbl(txtDec.Text)
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
