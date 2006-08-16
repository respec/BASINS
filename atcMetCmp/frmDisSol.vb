Imports atcData
Imports atcUtility
Imports MapWinUtility

Public Class frmDisSol
    Inherits System.Windows.Forms.Form

    Private pOk As Boolean
    Private pTSGroup As atcDataGroup
    Private pDataManager As atcDataManager
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
    Friend WithEvents panelBottom As System.Windows.Forms.Panel
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
        Me.panelBottom = New System.Windows.Forms.Panel
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnOk = New System.Windows.Forms.Button
        Me.btnSolar = New System.Windows.Forms.Button
        Me.txtSolar = New System.Windows.Forms.TextBox
        Me.panelBottom.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblTSer
        '
        Me.lblTSer.Location = New System.Drawing.Point(19, 18)
        Me.lblTSer.Name = "lblTSer"
        Me.lblTSer.Size = New System.Drawing.Size(269, 19)
        Me.lblTSer.TabIndex = 2
        Me.lblTSer.Text = "Specify Daily Solar Radiation Timeseries"
        '
        'lblLatitude
        '
        Me.lblLatitude.Location = New System.Drawing.Point(19, 102)
        Me.lblLatitude.Name = "lblLatitude"
        Me.lblLatitude.Size = New System.Drawing.Size(183, 18)
        Me.lblLatitude.TabIndex = 3
        Me.lblLatitude.Text = "Latitude (in decimal degress)"
        '
        'txtLatitude
        '
        Me.txtLatitude.Location = New System.Drawing.Point(211, 102)
        Me.txtLatitude.Name = "txtLatitude"
        Me.txtLatitude.Size = New System.Drawing.Size(87, 22)
        Me.txtLatitude.TabIndex = 4
        '
        'panelBottom
        '
        Me.panelBottom.Controls.Add(Me.btnCancel)
        Me.panelBottom.Controls.Add(Me.btnOk)
        Me.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.panelBottom.Location = New System.Drawing.Point(0, 144)
        Me.panelBottom.Name = "panelBottom"
        Me.panelBottom.Size = New System.Drawing.Size(355, 37)
        Me.panelBottom.TabIndex = 16
        '
        'btnCancel
        '
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(211, 0)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(77, 28)
        Me.btnCancel.TabIndex = 1
        Me.btnCancel.Text = "Cancel"
        '
        'btnOk
        '
        Me.btnOk.Location = New System.Drawing.Point(96, 0)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(77, 28)
        Me.btnOk.TabIndex = 0
        Me.btnOk.Text = "Ok"
        '
        'btnSolar
        '
        Me.btnSolar.Location = New System.Drawing.Point(19, 46)
        Me.btnSolar.Name = "btnSolar"
        Me.btnSolar.Size = New System.Drawing.Size(58, 23)
        Me.btnSolar.TabIndex = 18
        Me.btnSolar.Text = "Select"
        '
        'txtSolar
        '
        Me.txtSolar.Location = New System.Drawing.Point(86, 46)
        Me.txtSolar.Name = "txtSolar"
        Me.txtSolar.ReadOnly = True
        Me.txtSolar.Size = New System.Drawing.Size(260, 22)
        Me.txtSolar.TabIndex = 19
        '
        'frmDisSol
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(355, 181)
        Me.Controls.Add(Me.txtSolar)
        Me.Controls.Add(Me.txtLatitude)
        Me.Controls.Add(Me.btnSolar)
        Me.Controls.Add(Me.panelBottom)
        Me.Controls.Add(Me.lblLatitude)
        Me.Controls.Add(Me.lblTSer)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Name = "frmDisSol"
        Me.Text = "Disaggregate Solar Radiation"
        Me.panelBottom.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region
    Public Function AskUser(ByVal aDataManager As atcDataManager, ByRef aSRadTSer As atcTimeseries, ByRef aLatitude As Double) As Boolean
        pDataManager = aDataManager
        Me.ShowDialog()
        If pOk Then
            aLatitude = cLat
            aSRadTSer = pTSGroup(0)
        End If
        Return pOk
    End Function

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        If Not pTSGroup Is Nothing AndAlso pTSGroup.Count > 0 Then
            If IsNumeric(txtLatitude.Text) Then
                cLat = CDbl(txtLatitude.Text)
                If cLat >= 25 And cLat <= 51 Then
                    pOk = True
                    Close()
                Else
                    Logger.Msg("Value for 'Latitude' must be between 25 and 51" & vbCrLf & _
                               "Value specified is '" & cLat & "'", Me.Text & " Problem")
                End If
            Else
                Logger.Msg("Value must be specified for 'Latitude'." & vbCrLf & _
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
        pTSGroup = pDataManager.UserSelectData("Select Daily Solar Radiation Timeseries")
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
