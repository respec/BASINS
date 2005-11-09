Imports atcData

Public Class frmCmpJPET
  Inherits System.Windows.Forms.Form

  Private pOk As Boolean
  Private pTMinTS As atcTimeseries
  Private pTMaxTS As atcTimeseries
  Private pSRadTS As atcTimeseries
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
  Friend WithEvents lblJensenPET As System.Windows.Forms.Label
  Friend WithEvents lblConstant As System.Windows.Forms.Label
  Friend WithEvents txtConstant As System.Windows.Forms.TextBox
  Friend WithEvents lblSRad As System.Windows.Forms.Label
  Friend WithEvents btnSRad As System.Windows.Forms.Button
  Friend WithEvents txtSRad As System.Windows.Forms.TextBox
  Friend WithEvents lblTempUnits As System.Windows.Forms.Label
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
    Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmCmpJPET))
    Me.lblJensenPET = New System.Windows.Forms.Label
    Me.lblConstant = New System.Windows.Forms.Label
    Me.txtConstant = New System.Windows.Forms.TextBox
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
    Me.lblSRad = New System.Windows.Forms.Label
    Me.btnSRad = New System.Windows.Forms.Button
    Me.txtSRad = New System.Windows.Forms.TextBox
    Me.lblTempUnits = New System.Windows.Forms.Label
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
    'lblConstant
    '
    Me.lblConstant.Location = New System.Drawing.Point(8, 144)
    Me.lblConstant.Name = "lblConstant"
    Me.lblConstant.Size = New System.Drawing.Size(128, 16)
    Me.lblConstant.TabIndex = 3
    Me.lblConstant.Text = "Constant Coefficient:"
    '
    'txtConstant
    '
    Me.txtConstant.Location = New System.Drawing.Point(144, 144)
    Me.txtConstant.Name = "txtConstant"
    Me.txtConstant.Size = New System.Drawing.Size(72, 20)
    Me.txtConstant.TabIndex = 4
    Me.txtConstant.Text = ""
    '
    'panelBottom
    '
    Me.panelBottom.Controls.Add(Me.btnCancel)
    Me.panelBottom.Controls.Add(Me.btnOk)
    Me.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom
    Me.panelBottom.Location = New System.Drawing.Point(0, 253)
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
    Me.btnOk.Location = New System.Drawing.Point(144, 0)
    Me.btnOk.Name = "btnOk"
    Me.btnOk.Size = New System.Drawing.Size(64, 24)
    Me.btnOk.TabIndex = 0
    Me.btnOk.Text = "Ok"
    '
    'btnTMin
    '
    Me.btnTMin.Location = New System.Drawing.Point(88, 40)
    Me.btnTMin.Name = "btnTMin"
    Me.btnTMin.Size = New System.Drawing.Size(48, 24)
    Me.btnTMin.TabIndex = 18
    Me.btnTMin.Text = "Select"
    '
    'txtTMin
    '
    Me.txtTMin.Location = New System.Drawing.Point(144, 40)
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
    Me.btnTMax.Location = New System.Drawing.Point(88, 72)
    Me.btnTMax.Name = "btnTMax"
    Me.btnTMax.Size = New System.Drawing.Size(48, 23)
    Me.btnTMax.TabIndex = 22
    Me.btnTMax.Text = "Select"
    '
    'txtTMax
    '
    Me.txtTMax.Location = New System.Drawing.Point(144, 72)
    Me.txtTMax.Name = "txtTMax"
    Me.txtTMax.ReadOnly = True
    Me.txtTMax.Size = New System.Drawing.Size(344, 20)
    Me.txtTMax.TabIndex = 23
    Me.txtTMax.Text = ""
    '
    'rdoDegF
    '
    Me.rdoDegF.Checked = True
    Me.rdoDegF.Location = New System.Drawing.Point(360, 144)
    Me.rdoDegF.Name = "rdoDegF"
    Me.rdoDegF.Size = New System.Drawing.Size(96, 16)
    Me.rdoDegF.TabIndex = 24
    Me.rdoDegF.TabStop = True
    Me.rdoDegF.Text = "Degrees F"
    '
    'rdoDegC
    '
    Me.rdoDegC.Location = New System.Drawing.Point(360, 168)
    Me.rdoDegC.Name = "rdoDegC"
    Me.rdoDegC.Size = New System.Drawing.Size(96, 16)
    Me.rdoDegC.TabIndex = 25
    Me.rdoDegC.Text = "Degrees C"
    '
    'lblMonCoeff
    '
    Me.lblMonCoeff.Location = New System.Drawing.Point(8, 176)
    Me.lblMonCoeff.Name = "lblMonCoeff"
    Me.lblMonCoeff.Size = New System.Drawing.Size(160, 16)
    Me.lblMonCoeff.TabIndex = 26
    Me.lblMonCoeff.Text = "Specify Monthly Coefficients"
    '
    'lblJan
    '
    Me.lblJan.Location = New System.Drawing.Point(16, 200)
    Me.lblJan.Name = "lblJan"
    Me.lblJan.Size = New System.Drawing.Size(24, 16)
    Me.lblJan.TabIndex = 27
    Me.lblJan.Text = "Jan"
    '
    'lblFeb
    '
    Me.lblFeb.Location = New System.Drawing.Point(56, 200)
    Me.lblFeb.Name = "lblFeb"
    Me.lblFeb.Size = New System.Drawing.Size(24, 16)
    Me.lblFeb.TabIndex = 28
    Me.lblFeb.Text = "Feb"
    '
    'lblMar
    '
    Me.lblMar.Location = New System.Drawing.Point(96, 200)
    Me.lblMar.Name = "lblMar"
    Me.lblMar.Size = New System.Drawing.Size(24, 16)
    Me.lblMar.TabIndex = 29
    Me.lblMar.Text = "Mar"
    '
    'lblApr
    '
    Me.lblApr.Location = New System.Drawing.Point(136, 200)
    Me.lblApr.Name = "lblApr"
    Me.lblApr.Size = New System.Drawing.Size(24, 16)
    Me.lblApr.TabIndex = 30
    Me.lblApr.Text = "Apr"
    '
    'lblMay
    '
    Me.lblMay.Location = New System.Drawing.Point(176, 200)
    Me.lblMay.Name = "lblMay"
    Me.lblMay.Size = New System.Drawing.Size(32, 16)
    Me.lblMay.TabIndex = 31
    Me.lblMay.Text = "May"
    '
    'lblJun
    '
    Me.lblJun.Location = New System.Drawing.Point(216, 200)
    Me.lblJun.Name = "lblJun"
    Me.lblJun.Size = New System.Drawing.Size(24, 16)
    Me.lblJun.TabIndex = 32
    Me.lblJun.Text = "Jun"
    '
    'lblJul
    '
    Me.lblJul.Location = New System.Drawing.Point(256, 200)
    Me.lblJul.Name = "lblJul"
    Me.lblJul.Size = New System.Drawing.Size(24, 16)
    Me.lblJul.TabIndex = 33
    Me.lblJul.Text = "Jul"
    '
    'lblAug
    '
    Me.lblAug.Location = New System.Drawing.Point(296, 200)
    Me.lblAug.Name = "lblAug"
    Me.lblAug.Size = New System.Drawing.Size(24, 16)
    Me.lblAug.TabIndex = 34
    Me.lblAug.Text = "Aug"
    '
    'lblSep
    '
    Me.lblSep.Location = New System.Drawing.Point(336, 200)
    Me.lblSep.Name = "lblSep"
    Me.lblSep.Size = New System.Drawing.Size(24, 16)
    Me.lblSep.TabIndex = 35
    Me.lblSep.Text = "Sep"
    '
    'lblOct
    '
    Me.lblOct.Location = New System.Drawing.Point(376, 200)
    Me.lblOct.Name = "lblOct"
    Me.lblOct.Size = New System.Drawing.Size(24, 16)
    Me.lblOct.TabIndex = 36
    Me.lblOct.Text = "Oct"
    '
    'lblNov
    '
    Me.lblNov.Location = New System.Drawing.Point(416, 200)
    Me.lblNov.Name = "lblNov"
    Me.lblNov.Size = New System.Drawing.Size(24, 16)
    Me.lblNov.TabIndex = 37
    Me.lblNov.Text = "Nov"
    '
    'lblDec
    '
    Me.lblDec.Location = New System.Drawing.Point(456, 200)
    Me.lblDec.Name = "lblDec"
    Me.lblDec.Size = New System.Drawing.Size(24, 16)
    Me.lblDec.TabIndex = 38
    Me.lblDec.Text = "Dec"
    '
    'txtJan
    '
    Me.txtJan.Location = New System.Drawing.Point(8, 216)
    Me.txtJan.Name = "txtJan"
    Me.txtJan.Size = New System.Drawing.Size(40, 20)
    Me.txtJan.TabIndex = 39
    Me.txtJan.Text = "0.012"
    '
    'txtFeb
    '
    Me.txtFeb.Location = New System.Drawing.Point(48, 216)
    Me.txtFeb.Name = "txtFeb"
    Me.txtFeb.Size = New System.Drawing.Size(40, 20)
    Me.txtFeb.TabIndex = 40
    Me.txtFeb.Text = "0.012"
    '
    'txtMar
    '
    Me.txtMar.Location = New System.Drawing.Point(88, 216)
    Me.txtMar.Name = "txtMar"
    Me.txtMar.Size = New System.Drawing.Size(40, 20)
    Me.txtMar.TabIndex = 41
    Me.txtMar.Text = "0.012"
    '
    'txtApr
    '
    Me.txtApr.Location = New System.Drawing.Point(128, 216)
    Me.txtApr.Name = "txtApr"
    Me.txtApr.Size = New System.Drawing.Size(40, 20)
    Me.txtApr.TabIndex = 42
    Me.txtApr.Text = "0.012"
    '
    'txtMay
    '
    Me.txtMay.Location = New System.Drawing.Point(168, 216)
    Me.txtMay.Name = "txtMay"
    Me.txtMay.Size = New System.Drawing.Size(40, 20)
    Me.txtMay.TabIndex = 43
    Me.txtMay.Text = "0.012"
    '
    'txtJun
    '
    Me.txtJun.Location = New System.Drawing.Point(208, 216)
    Me.txtJun.Name = "txtJun"
    Me.txtJun.Size = New System.Drawing.Size(40, 20)
    Me.txtJun.TabIndex = 44
    Me.txtJun.Text = "0.012"
    '
    'txtJul
    '
    Me.txtJul.Location = New System.Drawing.Point(248, 216)
    Me.txtJul.Name = "txtJul"
    Me.txtJul.Size = New System.Drawing.Size(40, 20)
    Me.txtJul.TabIndex = 45
    Me.txtJul.Text = "0.012"
    '
    'txtAug
    '
    Me.txtAug.Location = New System.Drawing.Point(288, 216)
    Me.txtAug.Name = "txtAug"
    Me.txtAug.Size = New System.Drawing.Size(40, 20)
    Me.txtAug.TabIndex = 46
    Me.txtAug.Text = "0.012"
    '
    'txtSep
    '
    Me.txtSep.Location = New System.Drawing.Point(328, 216)
    Me.txtSep.Name = "txtSep"
    Me.txtSep.Size = New System.Drawing.Size(40, 20)
    Me.txtSep.TabIndex = 47
    Me.txtSep.Text = "0.012"
    '
    'txtOct
    '
    Me.txtOct.Location = New System.Drawing.Point(368, 216)
    Me.txtOct.Name = "txtOct"
    Me.txtOct.Size = New System.Drawing.Size(40, 20)
    Me.txtOct.TabIndex = 48
    Me.txtOct.Text = "0.012"
    '
    'txtNov
    '
    Me.txtNov.Location = New System.Drawing.Point(408, 216)
    Me.txtNov.Name = "txtNov"
    Me.txtNov.Size = New System.Drawing.Size(40, 20)
    Me.txtNov.TabIndex = 49
    Me.txtNov.Text = "0.012"
    '
    'txtDec
    '
    Me.txtDec.Location = New System.Drawing.Point(448, 216)
    Me.txtDec.Name = "txtDec"
    Me.txtDec.Size = New System.Drawing.Size(40, 20)
    Me.txtDec.TabIndex = 50
    Me.txtDec.Text = "0.012"
    '
    'lblSRad
    '
    Me.lblSRad.Location = New System.Drawing.Point(8, 104)
    Me.lblSRad.Name = "lblSRad"
    Me.lblSRad.Size = New System.Drawing.Size(88, 16)
    Me.lblSRad.TabIndex = 51
    Me.lblSRad.Text = "Solar Radiation:"
    '
    'btnSRad
    '
    Me.btnSRad.Location = New System.Drawing.Point(88, 104)
    Me.btnSRad.Name = "btnSRad"
    Me.btnSRad.Size = New System.Drawing.Size(48, 23)
    Me.btnSRad.TabIndex = 52
    Me.btnSRad.Text = "Select"
    '
    'txtSRad
    '
    Me.txtSRad.Location = New System.Drawing.Point(144, 104)
    Me.txtSRad.Name = "txtSRad"
    Me.txtSRad.ReadOnly = True
    Me.txtSRad.Size = New System.Drawing.Size(344, 20)
    Me.txtSRad.TabIndex = 53
    Me.txtSRad.Text = ""
    '
    'lblTempUnits
    '
    Me.lblTempUnits.Location = New System.Drawing.Point(256, 152)
    Me.lblTempUnits.Name = "lblTempUnits"
    Me.lblTempUnits.Size = New System.Drawing.Size(104, 16)
    Me.lblTempUnits.TabIndex = 54
    Me.lblTempUnits.Text = "Temperature Units:"
    '
    'frmCmpJPET
    '
    Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
    Me.ClientSize = New System.Drawing.Size(496, 285)
    Me.Controls.Add(Me.lblTempUnits)
    Me.Controls.Add(Me.txtSRad)
    Me.Controls.Add(Me.btnSRad)
    Me.Controls.Add(Me.lblSRad)
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
    Me.Controls.Add(Me.txtConstant)
    Me.Controls.Add(Me.lblConstant)
    Me.Controls.Add(Me.lblJensenPET)
    Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
    Me.Name = "frmCmpJPET"
    Me.Text = "Specify Jensen PET Inputs"
    Me.panelBottom.ResumeLayout(False)
    Me.ResumeLayout(False)

  End Sub

#End Region
  Public Function AskUser(ByVal aDataManager As atcDataManager, ByRef aTMinTS As atcTimeseries, ByRef aTMaxTS As atcTimeseries, ByRef aSRadTS As atcTimeseries, ByRef aDegF As Boolean, ByRef aCTX As Double, ByRef aCTS() As Double) As Boolean
    pDataManager = aDataManager
    Me.ShowDialog()
    If pOk Then
      aTMinTS = pTMinTS
      aTMaxTS = pTMaxTS
      aSRadTS = pSRadTS
      aDegF = rdoDegF.Checked
      aCTX = CDbl(txtConstant.Text)
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

  Private Sub btnSRad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSRad.Click
    Dim lTSGroup As atcDataGroup = pDataManager.UserSelectData("Select data for Solar Radiation")
    If lTSGroup.Count > 0 Then
      pSRadTS = lTSGroup(0)
      txtSRad.Text = pSRadTS.ToString
    End If
  End Sub
End Class
