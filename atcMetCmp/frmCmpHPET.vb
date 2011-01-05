Imports atcData
Imports atcUtility
Imports MapWinUtility

Public Class frmCmpHPET
    Inherits System.Windows.Forms.Form

    Private pOk As Boolean
    Private pTMinTS As atcTimeseries
    Private pTMaxTS As atcTimeseries
    Private cLat As Double
    Private cCTS(12) As Double

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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCmpHPET))
        Me.lblCloudCover = New System.Windows.Forms.Label
        Me.lblLatitude = New System.Windows.Forms.Label
        Me.txtLatitude = New System.Windows.Forms.TextBox
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
        Me.SuspendLayout()
        '
        'lblCloudCover
        '
        Me.lblCloudCover.Location = New System.Drawing.Point(12, 9)
        Me.lblCloudCover.Name = "lblCloudCover"
        Me.lblCloudCover.Size = New System.Drawing.Size(144, 16)
        Me.lblCloudCover.TabIndex = 0
        Me.lblCloudCover.Text = "Specify Input Timeseries"
        '
        'lblLatitude
        '
        Me.lblLatitude.AutoSize = True
        Me.lblLatitude.Location = New System.Drawing.Point(12, 107)
        Me.lblLatitude.Name = "lblLatitude"
        Me.lblLatitude.Size = New System.Drawing.Size(133, 13)
        Me.lblLatitude.TabIndex = 7
        Me.lblLatitude.Text = "Latitude (decimal degress):"
        '
        'txtLatitude
        '
        Me.txtLatitude.Location = New System.Drawing.Point(152, 104)
        Me.txtLatitude.Name = "txtLatitude"
        Me.txtLatitude.Size = New System.Drawing.Size(72, 20)
        Me.txtLatitude.TabIndex = 8
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(429, 208)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(64, 24)
        Me.btnCancel.TabIndex = 25
        Me.btnCancel.Text = "Cancel"
        '
        'btnOk
        '
        Me.btnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOk.Location = New System.Drawing.Point(359, 208)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(64, 24)
        Me.btnOk.TabIndex = 24
        Me.btnOk.Text = "Ok"
        '
        'btnTMin
        '
        Me.btnTMin.Location = New System.Drawing.Point(74, 36)
        Me.btnTMin.Name = "btnTMin"
        Me.btnTMin.Size = New System.Drawing.Size(48, 20)
        Me.btnTMin.TabIndex = 2
        Me.btnTMin.Text = "Select"
        '
        'txtTMin
        '
        Me.txtTMin.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtTMin.Location = New System.Drawing.Point(128, 36)
        Me.txtTMin.Name = "txtTMin"
        Me.txtTMin.ReadOnly = True
        Me.txtTMin.Size = New System.Drawing.Size(363, 20)
        Me.txtTMin.TabIndex = 3
        '
        'lblTMin
        '
        Me.lblTMin.AutoSize = True
        Me.lblTMin.Location = New System.Drawing.Point(12, 40)
        Me.lblTMin.Name = "lblTMin"
        Me.lblTMin.Size = New System.Drawing.Size(57, 13)
        Me.lblTMin.TabIndex = 1
        Me.lblTMin.Text = "Min Temp:"
        '
        'lblTMax
        '
        Me.lblTMax.AutoSize = True
        Me.lblTMax.Location = New System.Drawing.Point(12, 74)
        Me.lblTMax.Name = "lblTMax"
        Me.lblTMax.Size = New System.Drawing.Size(60, 13)
        Me.lblTMax.TabIndex = 4
        Me.lblTMax.Text = "Max Temp:"
        '
        'btnTMax
        '
        Me.btnTMax.Location = New System.Drawing.Point(74, 71)
        Me.btnTMax.Name = "btnTMax"
        Me.btnTMax.Size = New System.Drawing.Size(48, 20)
        Me.btnTMax.TabIndex = 5
        Me.btnTMax.Text = "Select"
        '
        'txtTMax
        '
        Me.txtTMax.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtTMax.Location = New System.Drawing.Point(128, 71)
        Me.txtTMax.Name = "txtTMax"
        Me.txtTMax.ReadOnly = True
        Me.txtTMax.Size = New System.Drawing.Size(363, 20)
        Me.txtTMax.TabIndex = 6
        '
        'rdoDegF
        '
        Me.rdoDegF.Checked = True
        Me.rdoDegF.Location = New System.Drawing.Point(270, 104)
        Me.rdoDegF.Name = "rdoDegF"
        Me.rdoDegF.Size = New System.Drawing.Size(96, 20)
        Me.rdoDegF.TabIndex = 9
        Me.rdoDegF.TabStop = True
        Me.rdoDegF.Text = "Fahrenheit"
        '
        'rdoDegC
        '
        Me.rdoDegC.Location = New System.Drawing.Point(270, 124)
        Me.rdoDegC.Name = "rdoDegC"
        Me.rdoDegC.Size = New System.Drawing.Size(96, 20)
        Me.rdoDegC.TabIndex = 10
        Me.rdoDegC.Text = "Celsius"
        '
        'lblMonCoeff
        '
        Me.lblMonCoeff.AutoSize = True
        Me.lblMonCoeff.Location = New System.Drawing.Point(12, 142)
        Me.lblMonCoeff.Name = "lblMonCoeff"
        Me.lblMonCoeff.Size = New System.Drawing.Size(140, 13)
        Me.lblMonCoeff.TabIndex = 11
        Me.lblMonCoeff.Text = "Specify Monthly Coefficients"
        '
        'lblJan
        '
        Me.lblJan.AutoSize = True
        Me.lblJan.Location = New System.Drawing.Point(23, 161)
        Me.lblJan.Name = "lblJan"
        Me.lblJan.Size = New System.Drawing.Size(24, 13)
        Me.lblJan.TabIndex = 27
        Me.lblJan.Text = "Jan"
        '
        'lblFeb
        '
        Me.lblFeb.AutoSize = True
        Me.lblFeb.Location = New System.Drawing.Point(63, 161)
        Me.lblFeb.Name = "lblFeb"
        Me.lblFeb.Size = New System.Drawing.Size(25, 13)
        Me.lblFeb.TabIndex = 28
        Me.lblFeb.Text = "Feb"
        '
        'lblMar
        '
        Me.lblMar.AutoSize = True
        Me.lblMar.Location = New System.Drawing.Point(103, 161)
        Me.lblMar.Name = "lblMar"
        Me.lblMar.Size = New System.Drawing.Size(25, 13)
        Me.lblMar.TabIndex = 29
        Me.lblMar.Text = "Mar"
        '
        'lblApr
        '
        Me.lblApr.AutoSize = True
        Me.lblApr.Location = New System.Drawing.Point(143, 161)
        Me.lblApr.Name = "lblApr"
        Me.lblApr.Size = New System.Drawing.Size(23, 13)
        Me.lblApr.TabIndex = 30
        Me.lblApr.Text = "Apr"
        '
        'lblMay
        '
        Me.lblMay.AutoSize = True
        Me.lblMay.Location = New System.Drawing.Point(183, 161)
        Me.lblMay.Name = "lblMay"
        Me.lblMay.Size = New System.Drawing.Size(27, 13)
        Me.lblMay.TabIndex = 31
        Me.lblMay.Text = "May"
        '
        'lblJun
        '
        Me.lblJun.AutoSize = True
        Me.lblJun.Location = New System.Drawing.Point(223, 161)
        Me.lblJun.Name = "lblJun"
        Me.lblJun.Size = New System.Drawing.Size(24, 13)
        Me.lblJun.TabIndex = 32
        Me.lblJun.Text = "Jun"
        '
        'lblJul
        '
        Me.lblJul.AutoSize = True
        Me.lblJul.Location = New System.Drawing.Point(263, 161)
        Me.lblJul.Name = "lblJul"
        Me.lblJul.Size = New System.Drawing.Size(20, 13)
        Me.lblJul.TabIndex = 33
        Me.lblJul.Text = "Jul"
        '
        'lblAug
        '
        Me.lblAug.AutoSize = True
        Me.lblAug.Location = New System.Drawing.Point(303, 161)
        Me.lblAug.Name = "lblAug"
        Me.lblAug.Size = New System.Drawing.Size(26, 13)
        Me.lblAug.TabIndex = 34
        Me.lblAug.Text = "Aug"
        '
        'lblSep
        '
        Me.lblSep.AutoSize = True
        Me.lblSep.Location = New System.Drawing.Point(343, 161)
        Me.lblSep.Name = "lblSep"
        Me.lblSep.Size = New System.Drawing.Size(26, 13)
        Me.lblSep.TabIndex = 35
        Me.lblSep.Text = "Sep"
        '
        'lblOct
        '
        Me.lblOct.AutoSize = True
        Me.lblOct.Location = New System.Drawing.Point(383, 161)
        Me.lblOct.Name = "lblOct"
        Me.lblOct.Size = New System.Drawing.Size(24, 13)
        Me.lblOct.TabIndex = 36
        Me.lblOct.Text = "Oct"
        '
        'lblNov
        '
        Me.lblNov.AutoSize = True
        Me.lblNov.Location = New System.Drawing.Point(423, 161)
        Me.lblNov.Name = "lblNov"
        Me.lblNov.Size = New System.Drawing.Size(27, 13)
        Me.lblNov.TabIndex = 37
        Me.lblNov.Text = "Nov"
        '
        'lblDec
        '
        Me.lblDec.AutoSize = True
        Me.lblDec.Location = New System.Drawing.Point(463, 161)
        Me.lblDec.Name = "lblDec"
        Me.lblDec.Size = New System.Drawing.Size(27, 13)
        Me.lblDec.TabIndex = 38
        Me.lblDec.Text = "Dec"
        '
        'txtJan
        '
        Me.txtJan.Location = New System.Drawing.Point(15, 177)
        Me.txtJan.Name = "txtJan"
        Me.txtJan.Size = New System.Drawing.Size(40, 20)
        Me.txtJan.TabIndex = 12
        Me.txtJan.Text = "0.0055"
        '
        'txtFeb
        '
        Me.txtFeb.Location = New System.Drawing.Point(55, 177)
        Me.txtFeb.Name = "txtFeb"
        Me.txtFeb.Size = New System.Drawing.Size(40, 20)
        Me.txtFeb.TabIndex = 13
        Me.txtFeb.Text = "0.0055"
        '
        'txtMar
        '
        Me.txtMar.Location = New System.Drawing.Point(95, 177)
        Me.txtMar.Name = "txtMar"
        Me.txtMar.Size = New System.Drawing.Size(40, 20)
        Me.txtMar.TabIndex = 14
        Me.txtMar.Text = "0.0055"
        '
        'txtApr
        '
        Me.txtApr.Location = New System.Drawing.Point(135, 177)
        Me.txtApr.Name = "txtApr"
        Me.txtApr.Size = New System.Drawing.Size(40, 20)
        Me.txtApr.TabIndex = 15
        Me.txtApr.Text = "0.0055"
        '
        'txtMay
        '
        Me.txtMay.Location = New System.Drawing.Point(175, 177)
        Me.txtMay.Name = "txtMay"
        Me.txtMay.Size = New System.Drawing.Size(40, 20)
        Me.txtMay.TabIndex = 16
        Me.txtMay.Text = "0.0055"
        '
        'txtJun
        '
        Me.txtJun.Location = New System.Drawing.Point(215, 177)
        Me.txtJun.Name = "txtJun"
        Me.txtJun.Size = New System.Drawing.Size(40, 20)
        Me.txtJun.TabIndex = 17
        Me.txtJun.Text = "0.0055"
        '
        'txtJul
        '
        Me.txtJul.Location = New System.Drawing.Point(255, 177)
        Me.txtJul.Name = "txtJul"
        Me.txtJul.Size = New System.Drawing.Size(40, 20)
        Me.txtJul.TabIndex = 18
        Me.txtJul.Text = "0.0055"
        '
        'txtAug
        '
        Me.txtAug.Location = New System.Drawing.Point(295, 177)
        Me.txtAug.Name = "txtAug"
        Me.txtAug.Size = New System.Drawing.Size(40, 20)
        Me.txtAug.TabIndex = 19
        Me.txtAug.Text = "0.0055"
        '
        'txtSep
        '
        Me.txtSep.Location = New System.Drawing.Point(335, 177)
        Me.txtSep.Name = "txtSep"
        Me.txtSep.Size = New System.Drawing.Size(40, 20)
        Me.txtSep.TabIndex = 20
        Me.txtSep.Text = "0.0055"
        '
        'txtOct
        '
        Me.txtOct.Location = New System.Drawing.Point(375, 177)
        Me.txtOct.Name = "txtOct"
        Me.txtOct.Size = New System.Drawing.Size(40, 20)
        Me.txtOct.TabIndex = 21
        Me.txtOct.Text = "0.0055"
        '
        'txtNov
        '
        Me.txtNov.Location = New System.Drawing.Point(415, 177)
        Me.txtNov.Name = "txtNov"
        Me.txtNov.Size = New System.Drawing.Size(40, 20)
        Me.txtNov.TabIndex = 22
        Me.txtNov.Text = "0.0055"
        '
        'txtDec
        '
        Me.txtDec.Location = New System.Drawing.Point(455, 177)
        Me.txtDec.Name = "txtDec"
        Me.txtDec.Size = New System.Drawing.Size(40, 20)
        Me.txtDec.TabIndex = 23
        Me.txtDec.Text = "0.0055"
        '
        'frmCmpHPET
        '
        Me.AcceptButton = Me.btnOk
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(505, 244)
        Me.Controls.Add(Me.btnOk)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnTMax)
        Me.Controls.Add(Me.btnTMin)
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
        Me.Controls.Add(Me.txtTMax)
        Me.Controls.Add(Me.txtTMin)
        Me.Controls.Add(Me.txtLatitude)
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
        Me.Controls.Add(Me.lblTMax)
        Me.Controls.Add(Me.lblTMin)
        Me.Controls.Add(Me.lblLatitude)
        Me.Controls.Add(Me.lblCloudCover)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Name = "frmCmpHPET"
        Me.Text = "Compute Hamon PET"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region
    Public Function AskUser(ByRef aTMinTS As atcTimeseries, ByRef aTMaxTS As atcTimeseries, ByRef aDegF As Boolean, ByRef aLatitude As Double, ByRef aCTS() As Double) As Boolean
        Me.ShowDialog()
        If pOk Then
            aTMinTS = pTMinTS
            aTMaxTS = pTMaxTS
            aDegF = rdoDegF.Checked
            aLatitude = cLat
            For i As Integer = 1 To 12
                aCTS(i) = cCTS(i)
            Next
        End If
        Return pOk
    End Function

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        If Not pTMinTS Is Nothing And Not pTMaxTS Is Nothing Then
            If IsNumeric(txtLatitude.Text) Then
                cLat = CDbl(txtLatitude.Text)
                If cLat >= MetComputeLatitudeMin AndAlso cLat <= MetComputeLatitudeMax Then
                    On Error GoTo BadCoeff
                    cCTS(1) = CDbl(txtJan.Text)
                    cCTS(2) = CDbl(txtFeb.Text)
                    cCTS(3) = CDbl(txtMar.Text)
                    cCTS(4) = CDbl(txtApr.Text)
                    cCTS(5) = CDbl(txtMay.Text)
                    cCTS(6) = CDbl(txtJun.Text)
                    cCTS(7) = CDbl(txtJul.Text)
                    cCTS(8) = CDbl(txtAug.Text)
                    cCTS(9) = CDbl(txtSep.Text)
                    cCTS(10) = CDbl(txtOct.Text)
                    cCTS(11) = CDbl(txtNov.Text)
                    cCTS(12) = CDbl(txtDec.Text)
                    pOk = True
                    For i As Integer = 1 To 12
                        If cCTS(i) < 0 Or cCTS(i) > 1 Then
                            pOk = False
                            Logger.Msg("Values for 'Monthly Coefficients' must be between 0 and 1" & vbCrLf & _
                                       "Value for month " & i & " is '" & cCTS(i) & "'", Me.Text & " Problem")
                            Exit For
                        End If
                    Next
                    If pOk Then Close()
                Else
                    Logger.Msg("Value for 'Latitude' must be between " & MetComputeLatitudeMin & " and " & MetComputeLatitudeMax & vbCrLf & _
                               "Value specified is '" & cLat & "'", Me.Text & " Problem")
                End If
            Else
                Logger.Msg("Value must be specified for 'Latitude'." & vbCrLf & _
                           "This value is currently not numeric.", Me.Text & " Problem")
            End If
        Else
            Logger.Msg("No Timeseries selected for 'Min Temp' or 'Max Temp'" & vbCrLf & _
                         "Use 'Select' buttons to specify the timeseries", Me.Text & " Problem")
        End If
        Exit Sub
BadCoeff:
        Logger.Msg("Values must be specified for 'Monthly Coefficients'." & vbCrLf & _
                   "At least one value is currently not numeric.", Me.Text & " Problem")

    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Close()
    End Sub

    Private Sub btnTMin_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTMin.Click
        Dim lTSGroup As atcTimeseriesGroup = atcDataManager.UserSelectData("Select data for Daily Min Temperature")
        If lTSGroup.Count > 0 Then
            pTMinTS = lTSGroup(0)
            txtTMin.Text = pTMinTS.ToString
        End If
    End Sub

    Private Sub btnTMax_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTMax.Click
        Dim lTSGroup As atcTimeseriesGroup = atcDataManager.UserSelectData("Select data for Daily Max Temperature")
        If lTSGroup.Count > 0 Then
            pTMaxTS = lTSGroup(0)
            txtTMax.Text = pTMaxTS.ToString
        End If
    End Sub

    Private Sub frmCmpHPET_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyValue = Windows.Forms.Keys.F1 Then
            ShowHelp("BASINS Details\Compute\Computations.html")
        End If
    End Sub
End Class
