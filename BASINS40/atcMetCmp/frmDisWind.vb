Imports atcData
Imports atcUtility
Imports MapWinUtility

Public Class frmDisWind
    Inherits System.Windows.Forms.Form

    Private pOk As Boolean
    Private pWindTS As atcTimeseries
    Private pDataManager As atcDataManager
    Private cHrDist(24) As Double

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
    Friend WithEvents btnWind As System.Windows.Forms.Button
    Friend WithEvents txtWind As System.Windows.Forms.TextBox
    Friend WithEvents lblWind As System.Windows.Forms.Label
    Friend WithEvents lblHrDist As System.Windows.Forms.Label
    Friend WithEvents txtHr1 As System.Windows.Forms.TextBox
    Friend WithEvents txtHr2 As System.Windows.Forms.TextBox
    Friend WithEvents txtHr3 As System.Windows.Forms.TextBox
    Friend WithEvents txtHr4 As System.Windows.Forms.TextBox
    Friend WithEvents txtHr5 As System.Windows.Forms.TextBox
    Friend WithEvents txtHr6 As System.Windows.Forms.TextBox
    Friend WithEvents txtHr7 As System.Windows.Forms.TextBox
    Friend WithEvents txtHr8 As System.Windows.Forms.TextBox
    Friend WithEvents txtHr9 As System.Windows.Forms.TextBox
    Friend WithEvents txtHr10 As System.Windows.Forms.TextBox
    Friend WithEvents txtHr11 As System.Windows.Forms.TextBox
    Friend WithEvents txtHr12 As System.Windows.Forms.TextBox
    Friend WithEvents lbl1To12 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtHr24 As System.Windows.Forms.TextBox
    Friend WithEvents txtHr23 As System.Windows.Forms.TextBox
    Friend WithEvents txtHr22 As System.Windows.Forms.TextBox
    Friend WithEvents txtHr21 As System.Windows.Forms.TextBox
    Friend WithEvents txtHr20 As System.Windows.Forms.TextBox
    Friend WithEvents txtHr19 As System.Windows.Forms.TextBox
    Friend WithEvents txtHr18 As System.Windows.Forms.TextBox
    Friend WithEvents txtHr17 As System.Windows.Forms.TextBox
    Friend WithEvents txtHr16 As System.Windows.Forms.TextBox
    Friend WithEvents txtHr15 As System.Windows.Forms.TextBox
    Friend WithEvents txtHr14 As System.Windows.Forms.TextBox
    Friend WithEvents txtHr13 As System.Windows.Forms.TextBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDisWind))
        Me.lblCloudCover = New System.Windows.Forms.Label
        Me.panelBottom = New System.Windows.Forms.Panel
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnOk = New System.Windows.Forms.Button
        Me.btnWind = New System.Windows.Forms.Button
        Me.txtWind = New System.Windows.Forms.TextBox
        Me.lblWind = New System.Windows.Forms.Label
        Me.lblHrDist = New System.Windows.Forms.Label
        Me.txtHr1 = New System.Windows.Forms.TextBox
        Me.txtHr2 = New System.Windows.Forms.TextBox
        Me.txtHr3 = New System.Windows.Forms.TextBox
        Me.txtHr4 = New System.Windows.Forms.TextBox
        Me.txtHr5 = New System.Windows.Forms.TextBox
        Me.txtHr6 = New System.Windows.Forms.TextBox
        Me.txtHr7 = New System.Windows.Forms.TextBox
        Me.txtHr8 = New System.Windows.Forms.TextBox
        Me.txtHr9 = New System.Windows.Forms.TextBox
        Me.txtHr10 = New System.Windows.Forms.TextBox
        Me.txtHr11 = New System.Windows.Forms.TextBox
        Me.txtHr12 = New System.Windows.Forms.TextBox
        Me.lbl1To12 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtHr24 = New System.Windows.Forms.TextBox
        Me.txtHr23 = New System.Windows.Forms.TextBox
        Me.txtHr22 = New System.Windows.Forms.TextBox
        Me.txtHr21 = New System.Windows.Forms.TextBox
        Me.txtHr20 = New System.Windows.Forms.TextBox
        Me.txtHr19 = New System.Windows.Forms.TextBox
        Me.txtHr18 = New System.Windows.Forms.TextBox
        Me.txtHr17 = New System.Windows.Forms.TextBox
        Me.txtHr16 = New System.Windows.Forms.TextBox
        Me.txtHr15 = New System.Windows.Forms.TextBox
        Me.txtHr14 = New System.Windows.Forms.TextBox
        Me.txtHr13 = New System.Windows.Forms.TextBox
        Me.panelBottom.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblCloudCover
        '
        Me.lblCloudCover.AutoSize = True
        Me.lblCloudCover.Location = New System.Drawing.Point(12, 9)
        Me.lblCloudCover.Name = "lblCloudCover"
        Me.lblCloudCover.Size = New System.Drawing.Size(149, 13)
        Me.lblCloudCover.TabIndex = 0
        Me.lblCloudCover.Text = "Specify Daily Wind Timeseries"
        '
        'panelBottom
        '
        Me.panelBottom.Controls.Add(Me.btnCancel)
        Me.panelBottom.Controls.Add(Me.btnOk)
        Me.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.panelBottom.Location = New System.Drawing.Point(0, 169)
        Me.panelBottom.Name = "panelBottom"
        Me.panelBottom.Size = New System.Drawing.Size(555, 38)
        Me.panelBottom.TabIndex = 31
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(479, 2)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(64, 24)
        Me.btnCancel.TabIndex = 33
        Me.btnCancel.Text = "Cancel"
        '
        'btnOk
        '
        Me.btnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOk.Location = New System.Drawing.Point(409, 2)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(64, 24)
        Me.btnOk.TabIndex = 32
        Me.btnOk.Text = "Ok"
        '
        'btnWind
        '
        Me.btnWind.Location = New System.Drawing.Point(63, 36)
        Me.btnWind.Name = "btnWind"
        Me.btnWind.Size = New System.Drawing.Size(48, 20)
        Me.btnWind.TabIndex = 2
        Me.btnWind.Text = "Select"
        '
        'txtWind
        '
        Me.txtWind.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtWind.Location = New System.Drawing.Point(117, 36)
        Me.txtWind.Name = "txtWind"
        Me.txtWind.ReadOnly = True
        Me.txtWind.Size = New System.Drawing.Size(426, 20)
        Me.txtWind.TabIndex = 3
        '
        'lblWind
        '
        Me.lblWind.AutoSize = True
        Me.lblWind.Location = New System.Drawing.Point(22, 39)
        Me.lblWind.Name = "lblWind"
        Me.lblWind.Size = New System.Drawing.Size(35, 13)
        Me.lblWind.TabIndex = 1
        Me.lblWind.Text = "Wind:"
        '
        'lblHrDist
        '
        Me.lblHrDist.AutoSize = True
        Me.lblHrDist.Location = New System.Drawing.Point(12, 72)
        Me.lblHrDist.Name = "lblHrDist"
        Me.lblHrDist.Size = New System.Drawing.Size(130, 13)
        Me.lblHrDist.TabIndex = 4
        Me.lblHrDist.Text = "Specify Hourly Distribution"
        '
        'txtHr1
        '
        Me.txtHr1.Location = New System.Drawing.Point(63, 104)
        Me.txtHr1.Name = "txtHr1"
        Me.txtHr1.Size = New System.Drawing.Size(40, 20)
        Me.txtHr1.TabIndex = 6
        Me.txtHr1.Text = "0.034"
        '
        'txtHr2
        '
        Me.txtHr2.Location = New System.Drawing.Point(103, 104)
        Me.txtHr2.Name = "txtHr2"
        Me.txtHr2.Size = New System.Drawing.Size(40, 20)
        Me.txtHr2.TabIndex = 7
        Me.txtHr2.Text = "0.034"
        '
        'txtHr3
        '
        Me.txtHr3.Location = New System.Drawing.Point(143, 104)
        Me.txtHr3.Name = "txtHr3"
        Me.txtHr3.Size = New System.Drawing.Size(40, 20)
        Me.txtHr3.TabIndex = 8
        Me.txtHr3.Text = "0.034"
        '
        'txtHr4
        '
        Me.txtHr4.Location = New System.Drawing.Point(183, 104)
        Me.txtHr4.Name = "txtHr4"
        Me.txtHr4.Size = New System.Drawing.Size(40, 20)
        Me.txtHr4.TabIndex = 9
        Me.txtHr4.Text = "0.034"
        '
        'txtHr5
        '
        Me.txtHr5.Location = New System.Drawing.Point(223, 104)
        Me.txtHr5.Name = "txtHr5"
        Me.txtHr5.Size = New System.Drawing.Size(40, 20)
        Me.txtHr5.TabIndex = 10
        Me.txtHr5.Text = "0.034"
        '
        'txtHr6
        '
        Me.txtHr6.Location = New System.Drawing.Point(263, 104)
        Me.txtHr6.Name = "txtHr6"
        Me.txtHr6.Size = New System.Drawing.Size(40, 20)
        Me.txtHr6.TabIndex = 11
        Me.txtHr6.Text = "0.034"
        '
        'txtHr7
        '
        Me.txtHr7.Location = New System.Drawing.Point(303, 104)
        Me.txtHr7.Name = "txtHr7"
        Me.txtHr7.Size = New System.Drawing.Size(40, 20)
        Me.txtHr7.TabIndex = 12
        Me.txtHr7.Text = "0.034"
        '
        'txtHr8
        '
        Me.txtHr8.Location = New System.Drawing.Point(343, 104)
        Me.txtHr8.Name = "txtHr8"
        Me.txtHr8.Size = New System.Drawing.Size(40, 20)
        Me.txtHr8.TabIndex = 13
        Me.txtHr8.Text = "0.035"
        '
        'txtHr9
        '
        Me.txtHr9.Location = New System.Drawing.Point(383, 104)
        Me.txtHr9.Name = "txtHr9"
        Me.txtHr9.Size = New System.Drawing.Size(40, 20)
        Me.txtHr9.TabIndex = 14
        Me.txtHr9.Text = "0.037"
        '
        'txtHr10
        '
        Me.txtHr10.Location = New System.Drawing.Point(423, 104)
        Me.txtHr10.Name = "txtHr10"
        Me.txtHr10.Size = New System.Drawing.Size(40, 20)
        Me.txtHr10.TabIndex = 15
        Me.txtHr10.Text = "0.041"
        '
        'txtHr11
        '
        Me.txtHr11.Location = New System.Drawing.Point(463, 104)
        Me.txtHr11.Name = "txtHr11"
        Me.txtHr11.Size = New System.Drawing.Size(40, 20)
        Me.txtHr11.TabIndex = 16
        Me.txtHr11.Text = "0.046"
        '
        'txtHr12
        '
        Me.txtHr12.Location = New System.Drawing.Point(503, 104)
        Me.txtHr12.Name = "txtHr12"
        Me.txtHr12.Size = New System.Drawing.Size(40, 20)
        Me.txtHr12.TabIndex = 17
        Me.txtHr12.Text = "0.05"
        '
        'lbl1To12
        '
        Me.lbl1To12.AutoSize = True
        Me.lbl1To12.Location = New System.Drawing.Point(22, 107)
        Me.lbl1To12.Name = "lbl1To12"
        Me.lbl1To12.Size = New System.Drawing.Size(31, 13)
        Me.lbl1To12.TabIndex = 5
        Me.lbl1To12.Text = "1-12:"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(22, 131)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(37, 13)
        Me.Label1.TabIndex = 18
        Me.Label1.Text = "12-24:"
        '
        'txtHr24
        '
        Me.txtHr24.Location = New System.Drawing.Point(503, 128)
        Me.txtHr24.Name = "txtHr24"
        Me.txtHr24.Size = New System.Drawing.Size(40, 20)
        Me.txtHr24.TabIndex = 30
        Me.txtHr24.Text = "0.034"
        '
        'txtHr23
        '
        Me.txtHr23.Location = New System.Drawing.Point(463, 128)
        Me.txtHr23.Name = "txtHr23"
        Me.txtHr23.Size = New System.Drawing.Size(40, 20)
        Me.txtHr23.TabIndex = 29
        Me.txtHr23.Text = "0.035"
        '
        'txtHr22
        '
        Me.txtHr22.Location = New System.Drawing.Point(423, 128)
        Me.txtHr22.Name = "txtHr22"
        Me.txtHr22.Size = New System.Drawing.Size(40, 20)
        Me.txtHr22.TabIndex = 28
        Me.txtHr22.Text = "0.035"
        '
        'txtHr21
        '
        Me.txtHr21.Location = New System.Drawing.Point(383, 128)
        Me.txtHr21.Name = "txtHr21"
        Me.txtHr21.Size = New System.Drawing.Size(40, 20)
        Me.txtHr21.TabIndex = 27
        Me.txtHr21.Text = "0.038"
        '
        'txtHr20
        '
        Me.txtHr20.Location = New System.Drawing.Point(343, 128)
        Me.txtHr20.Name = "txtHr20"
        Me.txtHr20.Size = New System.Drawing.Size(40, 20)
        Me.txtHr20.TabIndex = 26
        Me.txtHr20.Text = "0.04"
        '
        'txtHr19
        '
        Me.txtHr19.Location = New System.Drawing.Point(303, 128)
        Me.txtHr19.Name = "txtHr19"
        Me.txtHr19.Size = New System.Drawing.Size(40, 20)
        Me.txtHr19.TabIndex = 25
        Me.txtHr19.Text = "0.043"
        '
        'txtHr18
        '
        Me.txtHr18.Location = New System.Drawing.Point(263, 128)
        Me.txtHr18.Name = "txtHr18"
        Me.txtHr18.Size = New System.Drawing.Size(40, 20)
        Me.txtHr18.TabIndex = 24
        Me.txtHr18.Text = "0.05"
        '
        'txtHr17
        '
        Me.txtHr17.Location = New System.Drawing.Point(223, 128)
        Me.txtHr17.Name = "txtHr17"
        Me.txtHr17.Size = New System.Drawing.Size(40, 20)
        Me.txtHr17.TabIndex = 23
        Me.txtHr17.Text = "0.056"
        '
        'txtHr16
        '
        Me.txtHr16.Location = New System.Drawing.Point(183, 128)
        Me.txtHr16.Name = "txtHr16"
        Me.txtHr16.Size = New System.Drawing.Size(40, 20)
        Me.txtHr16.TabIndex = 22
        Me.txtHr16.Text = "0.057"
        '
        'txtHr15
        '
        Me.txtHr15.Location = New System.Drawing.Point(143, 128)
        Me.txtHr15.Name = "txtHr15"
        Me.txtHr15.Size = New System.Drawing.Size(40, 20)
        Me.txtHr15.TabIndex = 21
        Me.txtHr15.Text = "0.058"
        '
        'txtHr14
        '
        Me.txtHr14.Location = New System.Drawing.Point(103, 128)
        Me.txtHr14.Name = "txtHr14"
        Me.txtHr14.Size = New System.Drawing.Size(40, 20)
        Me.txtHr14.TabIndex = 20
        Me.txtHr14.Text = "0.054"
        '
        'txtHr13
        '
        Me.txtHr13.Location = New System.Drawing.Point(63, 128)
        Me.txtHr13.Name = "txtHr13"
        Me.txtHr13.Size = New System.Drawing.Size(40, 20)
        Me.txtHr13.TabIndex = 19
        Me.txtHr13.Text = "0.053"
        '
        'frmDisWind
        '
        Me.AcceptButton = Me.btnOk
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(555, 207)
        Me.Controls.Add(Me.txtHr24)
        Me.Controls.Add(Me.txtHr23)
        Me.Controls.Add(Me.txtHr22)
        Me.Controls.Add(Me.txtHr21)
        Me.Controls.Add(Me.txtHr20)
        Me.Controls.Add(Me.txtHr19)
        Me.Controls.Add(Me.txtHr18)
        Me.Controls.Add(Me.txtHr17)
        Me.Controls.Add(Me.txtHr16)
        Me.Controls.Add(Me.txtHr15)
        Me.Controls.Add(Me.txtHr14)
        Me.Controls.Add(Me.txtHr13)
        Me.Controls.Add(Me.txtHr12)
        Me.Controls.Add(Me.txtHr11)
        Me.Controls.Add(Me.txtHr10)
        Me.Controls.Add(Me.txtHr9)
        Me.Controls.Add(Me.txtHr8)
        Me.Controls.Add(Me.txtHr7)
        Me.Controls.Add(Me.txtHr6)
        Me.Controls.Add(Me.txtHr5)
        Me.Controls.Add(Me.txtHr4)
        Me.Controls.Add(Me.txtHr3)
        Me.Controls.Add(Me.txtHr2)
        Me.Controls.Add(Me.txtHr1)
        Me.Controls.Add(Me.txtWind)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lbl1To12)
        Me.Controls.Add(Me.lblHrDist)
        Me.Controls.Add(Me.lblWind)
        Me.Controls.Add(Me.btnWind)
        Me.Controls.Add(Me.panelBottom)
        Me.Controls.Add(Me.lblCloudCover)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Name = "frmDisWind"
        Me.Text = "Disaggregate Wind"
        Me.panelBottom.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region
    Public Function AskUser(ByVal aDataManager As atcDataManager, ByRef aWindTS As atcTimeseries, ByRef aHrDist() As Double) As Boolean
        pDataManager = aDataManager
        Me.ShowDialog()
        If pOk Then
            aWindTS = pWindTS
            For i As Integer = 1 To 24
                aHrDist(i) = cHrDist(i)
            Next
        End If
        Return pOk
    End Function

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        If Not pWindTS Is Nothing Then
            On Error GoTo BadHourDist
            cHrDist(1) = CDbl(txtHr1.Text)
            cHrDist(2) = CDbl(txtHr2.Text)
            cHrDist(3) = CDbl(txtHr3.Text)
            cHrDist(4) = CDbl(txtHr4.Text)
            cHrDist(5) = CDbl(txtHr5.Text)
            cHrDist(6) = CDbl(txtHr6.Text)
            cHrDist(7) = CDbl(txtHr7.Text)
            cHrDist(8) = CDbl(txtHr8.Text)
            cHrDist(9) = CDbl(txtHr9.Text)
            cHrDist(10) = CDbl(txtHr10.Text)
            cHrDist(11) = CDbl(txtHr11.Text)
            cHrDist(12) = CDbl(txtHr12.Text)
            cHrDist(13) = CDbl(txtHr13.Text)
            cHrDist(14) = CDbl(txtHr14.Text)
            cHrDist(15) = CDbl(txtHr15.Text)
            cHrDist(16) = CDbl(txtHr16.Text)
            cHrDist(17) = CDbl(txtHr17.Text)
            cHrDist(18) = CDbl(txtHr18.Text)
            cHrDist(19) = CDbl(txtHr19.Text)
            cHrDist(20) = CDbl(txtHr20.Text)
            cHrDist(21) = CDbl(txtHr21.Text)
            cHrDist(22) = CDbl(txtHr22.Text)
            cHrDist(23) = CDbl(txtHr23.Text)
            cHrDist(24) = CDbl(txtHr24.Text)
            pOk = True
            Dim lTotDist As Double = 0.0
            For i As Integer = 1 To 24
                If cHrDist(i) < 0 Or cHrDist(i) > 1 Then
                    pOk = False
                    Logger.Msg("Values for 'Hourly Distribution' must be between 0 and 1" & vbCrLf & _
                               "Value for month " & i & " is '" & cHrDist(i) & "'", Me.Text & " Problem")
                    Exit For
                Else
                    lTotDist += cHrDist(i)
                End If
            Next
            If pOk Then 'make sure hourly distributions add up to 1
                If Math.Abs(lTotDist) - 1 < 0.0001 Then
                    Close()
                Else
                    pOk = False
                    Logger.Msg("The total for all 'Hourly Distribution' values must summ to 1." & vbCrLf & _
                               "Currently all values sum to " & "'" & lTotDist & "'", Me.Text & " Problem")
                End If
            End If
        Else
            Logger.Msg("No 'Daily Wind Timeseries' selected." & vbCrLf & _
                       "Use 'Select' buttons to specify the timeseries", Me.Text & " Problem")
        End If
        Exit Sub
BadHourDist:
        Logger.Msg("Values must be specified for 'Hourly Distribution'." & vbCrLf & _
                   "At least one value is currently not numeric.", Me.Text & " Problem")
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Close()
    End Sub

    Private Sub btnWind_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnWind.Click
        Dim lTSGroup As atcDataGroup = pDataManager.UserSelectData("Select data for Daily Min Temperature")
        If lTSGroup.Count > 0 Then
            pWindTS = lTSGroup(0)
            txtWind.Text = pWindTS.ToString
        End If
    End Sub

    Private Sub frmDisWind_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyValue = Windows.Forms.Keys.F1 Then
            ShowHelp("BASINS Details\Compute\Disaggregations.html")
        End If
    End Sub
End Class
