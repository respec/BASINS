Imports atcData
Imports atcUtility
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCmpPenman))
        Me.lblJensenPET = New System.Windows.Forms.Label
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
        Me.SuspendLayout()
        '
        'lblJensenPET
        '
        Me.lblJensenPET.AutoSize = True
        Me.lblJensenPET.Location = New System.Drawing.Point(12, 9)
        Me.lblJensenPET.Name = "lblJensenPET"
        Me.lblJensenPET.Size = New System.Drawing.Size(122, 13)
        Me.lblJensenPET.TabIndex = 0
        Me.lblJensenPET.Text = "Specify Input Timeseries"
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(447, 198)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(64, 24)
        Me.btnCancel.TabIndex = 17
        Me.btnCancel.Text = "Cancel"
        '
        'btnOk
        '
        Me.btnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOk.Location = New System.Drawing.Point(377, 198)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(64, 24)
        Me.btnOk.TabIndex = 16
        Me.btnOk.Text = "Ok"
        '
        'btnTMin
        '
        Me.btnTMin.Location = New System.Drawing.Point(104, 40)
        Me.btnTMin.Name = "btnTMin"
        Me.btnTMin.Size = New System.Drawing.Size(48, 20)
        Me.btnTMin.TabIndex = 2
        Me.btnTMin.Text = "Select"
        '
        'txtTMin
        '
        Me.txtTMin.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtTMin.Location = New System.Drawing.Point(158, 40)
        Me.txtTMin.Name = "txtTMin"
        Me.txtTMin.ReadOnly = True
        Me.txtTMin.Size = New System.Drawing.Size(353, 20)
        Me.txtTMin.TabIndex = 3
        '
        'lblTMin
        '
        Me.lblTMin.AutoSize = True
        Me.lblTMin.Location = New System.Drawing.Point(12, 43)
        Me.lblTMin.Name = "lblTMin"
        Me.lblTMin.Size = New System.Drawing.Size(57, 13)
        Me.lblTMin.TabIndex = 1
        Me.lblTMin.Text = "Min Temp:"
        '
        'lblTMax
        '
        Me.lblTMax.AutoSize = True
        Me.lblTMax.Location = New System.Drawing.Point(12, 75)
        Me.lblTMax.Name = "lblTMax"
        Me.lblTMax.Size = New System.Drawing.Size(60, 13)
        Me.lblTMax.TabIndex = 4
        Me.lblTMax.Text = "Max Temp:"
        '
        'btnTMax
        '
        Me.btnTMax.Location = New System.Drawing.Point(104, 72)
        Me.btnTMax.Name = "btnTMax"
        Me.btnTMax.Size = New System.Drawing.Size(48, 20)
        Me.btnTMax.TabIndex = 5
        Me.btnTMax.Text = "Select"
        '
        'txtTMax
        '
        Me.txtTMax.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtTMax.Location = New System.Drawing.Point(158, 72)
        Me.txtTMax.Name = "txtTMax"
        Me.txtTMax.ReadOnly = True
        Me.txtTMax.Size = New System.Drawing.Size(353, 20)
        Me.txtTMax.TabIndex = 6
        '
        'lblSRad
        '
        Me.lblSRad.AutoSize = True
        Me.lblSRad.Location = New System.Drawing.Point(12, 107)
        Me.lblSRad.Name = "lblSRad"
        Me.lblSRad.Size = New System.Drawing.Size(82, 13)
        Me.lblSRad.TabIndex = 7
        Me.lblSRad.Text = "Solar Radiation:"
        '
        'btnSRad
        '
        Me.btnSRad.Location = New System.Drawing.Point(104, 104)
        Me.btnSRad.Name = "btnSRad"
        Me.btnSRad.Size = New System.Drawing.Size(48, 20)
        Me.btnSRad.TabIndex = 8
        Me.btnSRad.Text = "Select"
        '
        'txtSRad
        '
        Me.txtSRad.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtSRad.Location = New System.Drawing.Point(158, 104)
        Me.txtSRad.Name = "txtSRad"
        Me.txtSRad.ReadOnly = True
        Me.txtSRad.Size = New System.Drawing.Size(353, 20)
        Me.txtSRad.TabIndex = 9
        '
        'lblDewP
        '
        Me.lblDewP.AutoSize = True
        Me.lblDewP.Location = New System.Drawing.Point(12, 139)
        Me.lblDewP.Name = "lblDewP"
        Me.lblDewP.Size = New System.Drawing.Size(85, 13)
        Me.lblDewP.TabIndex = 10
        Me.lblDewP.Text = "Dewpoint Temp:"
        '
        'lblWind
        '
        Me.lblWind.AutoSize = True
        Me.lblWind.Location = New System.Drawing.Point(12, 171)
        Me.lblWind.Name = "lblWind"
        Me.lblWind.Size = New System.Drawing.Size(88, 13)
        Me.lblWind.TabIndex = 13
        Me.lblWind.Text = "Wind Movement:"
        '
        'btnDewP
        '
        Me.btnDewP.Location = New System.Drawing.Point(104, 136)
        Me.btnDewP.Name = "btnDewP"
        Me.btnDewP.Size = New System.Drawing.Size(48, 20)
        Me.btnDewP.TabIndex = 11
        Me.btnDewP.Text = "Select"
        '
        'btnWind
        '
        Me.btnWind.Location = New System.Drawing.Point(104, 168)
        Me.btnWind.Name = "btnWind"
        Me.btnWind.Size = New System.Drawing.Size(48, 20)
        Me.btnWind.TabIndex = 14
        Me.btnWind.Text = "Select"
        '
        'txtDewP
        '
        Me.txtDewP.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDewP.Location = New System.Drawing.Point(158, 136)
        Me.txtDewP.Name = "txtDewP"
        Me.txtDewP.ReadOnly = True
        Me.txtDewP.Size = New System.Drawing.Size(353, 20)
        Me.txtDewP.TabIndex = 12
        '
        'txtWind
        '
        Me.txtWind.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtWind.Location = New System.Drawing.Point(158, 168)
        Me.txtWind.Name = "txtWind"
        Me.txtWind.ReadOnly = True
        Me.txtWind.Size = New System.Drawing.Size(353, 20)
        Me.txtWind.TabIndex = 15
        '
        'frmCmpPenman
        '
        Me.AcceptButton = Me.btnOk
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(523, 234)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOk)
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
        Me.Controls.Add(Me.lblJensenPET)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Name = "frmCmpPenman"
        Me.Text = "Compute Penman Pan Evaporation"
        Me.ResumeLayout(False)
        Me.PerformLayout()

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

    Private Sub frmCmpPenman_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyValue = Windows.Forms.Keys.F1 Then
            ShowHelp("BASINS Details\Compute\Computations.html")
        End If
    End Sub
End Class
