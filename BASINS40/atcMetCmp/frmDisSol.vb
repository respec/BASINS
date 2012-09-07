Imports atcData
Imports atcUtility
Imports MapWinUtility

Public Class frmDisSol
    Inherits System.Windows.Forms.Form

    Private pOk As Boolean
    Private pTSGroup As atcTimeseriesGroup
    Private cLat As Double

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
    Friend WithEvents btnSolar As System.Windows.Forms.Button
    Friend WithEvents txtSolar As System.Windows.Forms.TextBox
    Friend WithEvents lblTSer As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDisSol))
        Me.lblTSer = New System.Windows.Forms.Label
        Me.lblLatitude = New System.Windows.Forms.Label
        Me.txtLatitude = New System.Windows.Forms.TextBox
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnOk = New System.Windows.Forms.Button
        Me.btnSolar = New System.Windows.Forms.Button
        Me.txtSolar = New System.Windows.Forms.TextBox
        Me.SuspendLayout()
        '
        'lblTSer
        '
        Me.lblTSer.AutoSize = True
        Me.lblTSer.Location = New System.Drawing.Point(12, 9)
        Me.lblTSer.Name = "lblTSer"
        Me.lblTSer.Size = New System.Drawing.Size(196, 13)
        Me.lblTSer.TabIndex = 0
        Me.lblTSer.Text = "Specify Daily Solar Radiation Timeseries"
        '
        'lblLatitude
        '
        Me.lblLatitude.AutoSize = True
        Me.lblLatitude.Location = New System.Drawing.Point(12, 88)
        Me.lblLatitude.Name = "lblLatitude"
        Me.lblLatitude.Size = New System.Drawing.Size(130, 13)
        Me.lblLatitude.TabIndex = 3
        Me.lblLatitude.Text = "Latitude (decimal degress)"
        '
        'txtLatitude
        '
        Me.txtLatitude.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtLatitude.Location = New System.Drawing.Point(148, 85)
        Me.txtLatitude.Name = "txtLatitude"
        Me.txtLatitude.Size = New System.Drawing.Size(238, 20)
        Me.txtLatitude.TabIndex = 4
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(322, 125)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(64, 24)
        Me.btnCancel.TabIndex = 6
        Me.btnCancel.Text = "Cancel"
        '
        'btnOk
        '
        Me.btnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOk.Location = New System.Drawing.Point(252, 125)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(64, 24)
        Me.btnOk.TabIndex = 5
        Me.btnOk.Text = "Ok"
        '
        'btnSolar
        '
        Me.btnSolar.Location = New System.Drawing.Point(15, 39)
        Me.btnSolar.Name = "btnSolar"
        Me.btnSolar.Size = New System.Drawing.Size(48, 20)
        Me.btnSolar.TabIndex = 1
        Me.btnSolar.Text = "Select"
        '
        'txtSolar
        '
        Me.txtSolar.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtSolar.Location = New System.Drawing.Point(69, 40)
        Me.txtSolar.Name = "txtSolar"
        Me.txtSolar.ReadOnly = True
        Me.txtSolar.Size = New System.Drawing.Size(317, 20)
        Me.txtSolar.TabIndex = 2
        '
        'frmDisSol
        '
        Me.AcceptButton = Me.btnOk
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(398, 161)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.txtSolar)
        Me.Controls.Add(Me.btnOk)
        Me.Controls.Add(Me.txtLatitude)
        Me.Controls.Add(Me.btnSolar)
        Me.Controls.Add(Me.lblLatitude)
        Me.Controls.Add(Me.lblTSer)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Name = "frmDisSol"
        Me.Text = "Disaggregate Solar Radiation"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region
    Public Function AskUser(ByRef aSRadTSer As atcTimeseries, ByRef aLatitude As Double) As Boolean
        Me.ShowDialog()
        If pOk Then
            aLatitude = cLat
            aSRadTSer = pTSGroup(0)
        End If
        Return pOk
    End Function

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        If Not pTSGroup Is Nothing AndAlso pTSGroup.Count > 0 Then
            If Double.TryParse(txtLatitude.Text.Trim(), cLat) Then
                If cLat >= MetComputeLatitudeMin And cLat <= MetComputeLatitudeMax Then
                    pOk = True
                    Close()
                Else
                    Logger.Msg("Value for 'Latitude' must be between " & DoubleToString(MetComputeLatitudeMin) & " and " & DoubleToString(MetComputeLatitudeMax) & vbCrLf & _
                               "Value specified is '" & DoubleToString(cLat) & "'", Me.Text & " Problem")
                End If
            Else
                Logger.Msg("Numeric value must be specified for 'Latitude'." & vbCrLf & _
                           "This value is currently not numeric.", Me.Text & " Problem")
            End If
        Else
            Logger.Msg("No 'Daily Solar Radiation Timeseries' selected." & vbCrLf & _
                       "Use 'Select' buttons to specify the timeseries", Me.Text & " Problem")
        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Close()
    End Sub

    Private Sub btnSolar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSolar.Click
        pTSGroup = atcDataManager.UserSelectData("Select Daily Solar Radiation Timeseries")
        If pTSGroup.Count > 0 Then
            txtSolar.Text = pTSGroup.ItemByIndex(0).ToString
        End If
    End Sub

    Private Sub frmDisSol_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyValue = Windows.Forms.Keys.F1 Then
            ShowHelp("BASINS Details\Compute\Disaggregations.html")
        End If
    End Sub
End Class
