Imports atcData
Imports atcUtility
Imports MapWinUtility

Public Class frmCmpSol
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
    Friend WithEvents lblCloudCover As System.Windows.Forms.Label
    Friend WithEvents btnCloudCover As System.Windows.Forms.Button
    Friend WithEvents txtCloudCover As System.Windows.Forms.TextBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCmpSol))
        Me.lblCloudCover = New System.Windows.Forms.Label
        Me.lblLatitude = New System.Windows.Forms.Label
        Me.txtLatitude = New System.Windows.Forms.TextBox
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnOk = New System.Windows.Forms.Button
        Me.btnCloudCover = New System.Windows.Forms.Button
        Me.txtCloudCover = New System.Windows.Forms.TextBox
        Me.SuspendLayout()
        '
        'lblCloudCover
        '
        Me.lblCloudCover.AutoSize = True
        Me.lblCloudCover.Location = New System.Drawing.Point(12, 9)
        Me.lblCloudCover.Name = "lblCloudCover"
        Me.lblCloudCover.Size = New System.Drawing.Size(156, 13)
        Me.lblCloudCover.TabIndex = 0
        Me.lblCloudCover.Text = "Specify Cloud Cover Timeseries"
        '
        'lblLatitude
        '
        Me.lblLatitude.AutoSize = True
        Me.lblLatitude.Location = New System.Drawing.Point(12, 72)
        Me.lblLatitude.Name = "lblLatitude"
        Me.lblLatitude.Size = New System.Drawing.Size(130, 13)
        Me.lblLatitude.TabIndex = 3
        Me.lblLatitude.Text = "Latitude (decimal degress)"
        '
        'txtLatitude
        '
        Me.txtLatitude.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtLatitude.Location = New System.Drawing.Point(159, 69)
        Me.txtLatitude.Name = "txtLatitude"
        Me.txtLatitude.Size = New System.Drawing.Size(148, 20)
        Me.txtLatitude.TabIndex = 4
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(243, 109)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(64, 24)
        Me.btnCancel.TabIndex = 6
        Me.btnCancel.Text = "Cancel"
        '
        'btnOk
        '
        Me.btnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOk.Location = New System.Drawing.Point(173, 109)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(64, 24)
        Me.btnOk.TabIndex = 5
        Me.btnOk.Text = "Ok"
        '
        'btnCloudCover
        '
        Me.btnCloudCover.Location = New System.Drawing.Point(15, 39)
        Me.btnCloudCover.Name = "btnCloudCover"
        Me.btnCloudCover.Size = New System.Drawing.Size(48, 20)
        Me.btnCloudCover.TabIndex = 1
        Me.btnCloudCover.Text = "Select"
        '
        'txtCloudCover
        '
        Me.txtCloudCover.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtCloudCover.Location = New System.Drawing.Point(72, 40)
        Me.txtCloudCover.Name = "txtCloudCover"
        Me.txtCloudCover.ReadOnly = True
        Me.txtCloudCover.Size = New System.Drawing.Size(235, 20)
        Me.txtCloudCover.TabIndex = 2
        '
        'frmCmpSol
        '
        Me.AcceptButton = Me.btnOk
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(319, 145)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOk)
        Me.Controls.Add(Me.txtCloudCover)
        Me.Controls.Add(Me.btnCloudCover)
        Me.Controls.Add(Me.txtLatitude)
        Me.Controls.Add(Me.lblLatitude)
        Me.Controls.Add(Me.lblCloudCover)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmCmpSol"
        Me.Text = "Compute Solar Radiation"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region
    Public Function AskUser(ByRef aCldTSer As atcTimeseries, ByRef aLatitude As Double) As Boolean
        Me.ShowDialog()
        If pOk Then
            aLatitude = cLat
            aCldTSer = pTSGroup(0)
        End If
        Return pOk
    End Function

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        If Not pTSGroup Is Nothing AndAlso pTSGroup.Count > 0 Then
            If IsNumeric(txtLatitude.Text) Then
                cLat = CDbl(txtLatitude.Text)
                If cLat >= MetComputeLatitudeMin AndAlso cLat <= MetComputeLatitudeMax Then
                    pOk = True
                    Close()
                Else
                    Logger.Msg("Value for 'Latitude' must be between " & MetComputeLatitudeMin & " and " & MetComputeLatitudeMax & vbCrLf & _
                               "Value specified is '" & cLat & "'", Me.Text & " Problem")
                End If
            Else
                Logger.Msg("Value must be specified for 'Latitude'." & vbCrLf & _
                           "This value is currently not numeric.", Me.Text & " Problem")
            End If
        Else
            Logger.Msg("No Timeseries selected for 'Cloud Cover'" & vbCrLf & _
                       "Use 'Select' button to specify the timeseries", Me.Text & " Problem")
        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Close()
    End Sub

    Private Sub btnCloudCover_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCloudCover.Click
        pTSGroup = atcDataManager.UserSelectData("Select data for Cloud Cover")
        If pTSGroup.Count > 0 Then
            Dim lTimser As atcTimeseries = pTSGroup.ItemByIndex(0)
            txtCloudCover.Text = lTimser.ToString
            txtLatitude.Text = lTimser.Attributes.GetValue("Latitude", txtLatitude.Text)
        End If
    End Sub

    Private Sub frmCmpSol_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyValue = Windows.Forms.Keys.F1 Then
            ShowHelp("BASINS Details\Compute\Computations.html")
        End If
    End Sub
End Class
