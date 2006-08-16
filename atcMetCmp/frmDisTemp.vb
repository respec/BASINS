Imports atcData
Imports atcUtility
Imports MapWinUtility

Public Class frmDisTemp
    Inherits System.Windows.Forms.Form

    Private pOk As Boolean
    Private pTMinTS As atcTimeseries
    Private pTMaxTS As atcTimeseries
    Private pDataManager As atcDataManager
    Private cObsTime As Integer

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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDisTemp))
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
        Me.lblCloudCover.Location = New System.Drawing.Point(19, 18)
        Me.lblCloudCover.Name = "lblCloudCover"
        Me.lblCloudCover.Size = New System.Drawing.Size(250, 19)
        Me.lblCloudCover.TabIndex = 2
        Me.lblCloudCover.Text = "Specify Daily Temperature Timeseries"
        '
        'lblObsTime
        '
        Me.lblObsTime.Location = New System.Drawing.Point(10, 120)
        Me.lblObsTime.Name = "lblObsTime"
        Me.lblObsTime.Size = New System.Drawing.Size(134, 18)
        Me.lblObsTime.TabIndex = 3
        Me.lblObsTime.Text = "Observation Hour:"
        '
        'txtObsTime
        '
        Me.txtObsTime.Location = New System.Drawing.Point(154, 120)
        Me.txtObsTime.Name = "txtObsTime"
        Me.txtObsTime.Size = New System.Drawing.Size(57, 22)
        Me.txtObsTime.TabIndex = 4
        '
        'panelBottom
        '
        Me.panelBottom.Controls.Add(Me.btnCancel)
        Me.panelBottom.Controls.Add(Me.btnOk)
        Me.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.panelBottom.Location = New System.Drawing.Point(0, 162)
        Me.panelBottom.Name = "panelBottom"
        Me.panelBottom.Size = New System.Drawing.Size(556, 37)
        Me.panelBottom.TabIndex = 16
        '
        'btnCancel
        '
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(288, 0)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(77, 28)
        Me.btnCancel.TabIndex = 1
        Me.btnCancel.Text = "Cancel"
        '
        'btnOk
        '
        Me.btnOk.Location = New System.Drawing.Point(182, 0)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(77, 28)
        Me.btnOk.TabIndex = 0
        Me.btnOk.Text = "Ok"
        '
        'btnTMin
        '
        Me.btnTMin.Location = New System.Drawing.Point(86, 46)
        Me.btnTMin.Name = "btnTMin"
        Me.btnTMin.Size = New System.Drawing.Size(58, 23)
        Me.btnTMin.TabIndex = 18
        Me.btnTMin.Text = "Select"
        '
        'txtTMin
        '
        Me.txtTMin.Location = New System.Drawing.Point(154, 46)
        Me.txtTMin.Name = "txtTMin"
        Me.txtTMin.ReadOnly = True
        Me.txtTMin.Size = New System.Drawing.Size(393, 22)
        Me.txtTMin.TabIndex = 19
        '
        'lblTMin
        '
        Me.lblTMin.Location = New System.Drawing.Point(10, 46)
        Me.lblTMin.Name = "lblTMin"
        Me.lblTMin.Size = New System.Drawing.Size(76, 19)
        Me.lblTMin.TabIndex = 20
        Me.lblTMin.Text = "Min Temp:"
        '
        'lblTMax
        '
        Me.lblTMax.Location = New System.Drawing.Point(10, 83)
        Me.lblTMax.Name = "lblTMax"
        Me.lblTMax.Size = New System.Drawing.Size(76, 19)
        Me.lblTMax.TabIndex = 21
        Me.lblTMax.Text = "Max Temp:"
        '
        'btnTMax
        '
        Me.btnTMax.Location = New System.Drawing.Point(86, 82)
        Me.btnTMax.Name = "btnTMax"
        Me.btnTMax.Size = New System.Drawing.Size(58, 23)
        Me.btnTMax.TabIndex = 22
        Me.btnTMax.Text = "Select"
        '
        'txtTMax
        '
        Me.txtTMax.Location = New System.Drawing.Point(154, 83)
        Me.txtTMax.Name = "txtTMax"
        Me.txtTMax.ReadOnly = True
        Me.txtTMax.Size = New System.Drawing.Size(393, 22)
        Me.txtTMax.TabIndex = 23
        '
        'frmDisTemp
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(556, 199)
        Me.Controls.Add(Me.txtTMax)
        Me.Controls.Add(Me.txtTMin)
        Me.Controls.Add(Me.txtObsTime)
        Me.Controls.Add(Me.btnTMax)
        Me.Controls.Add(Me.lblTMax)
        Me.Controls.Add(Me.lblTMin)
        Me.Controls.Add(Me.btnTMin)
        Me.Controls.Add(Me.panelBottom)
        Me.Controls.Add(Me.lblObsTime)
        Me.Controls.Add(Me.lblCloudCover)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Name = "frmDisTemp"
        Me.Text = "Disaggregate Temperature"
        Me.panelBottom.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region
    Public Function AskUser(ByVal aDataManager As atcDataManager, ByRef aTMinTS As atcTimeseries, ByRef aTMaxTS As atcTimeseries, ByRef aObsTime As Integer) As Boolean
        pDataManager = aDataManager
        Me.ShowDialog()
        If pOk Then
            aTMinTS = pTMinTS
            aTMaxTS = pTMaxTS
            aObsTime = cObsTime
        End If
        Return pOk
    End Function

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        If Not pTMinTS Is Nothing And Not pTMaxTS Is Nothing Then
            If IsNumeric(txtObsTime.Text) Then
                cObsTime = CDbl(txtObsTime.Text)
                If cObsTime >= 1 And cObsTime <= 24 Then
                    pOk = True
                    Close()
                Else
                    Logger.Msg("Value for 'Observation Hour' must be between 1 and 24" & vbCrLf & _
                               "Value specified is '" & cObsTime & "'", Me.Text & " Problem")
                End If
            Else
                Logger.Msg("Value must be specified for 'Observation Hour'." & vbCrLf & _
                           "This value is currently not numeric.", Me.Text & " Problem")
            End If
        Else
            Logger.Msg("No Timeseries selected for 'Min Temp' or 'Max Temp'" & vbCrLf & _
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

    Private Sub frmDisTemp_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyValue = Windows.Forms.Keys.F1 Then
            ShowHelp("BASINS Details\Compute\Disaggregations.html")
        End If
    End Sub
End Class
