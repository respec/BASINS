Public Class frmSpecifyEventAttributes
    Inherits System.Windows.Forms.Form

    Private pOk As Boolean

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
    Friend WithEvents rdoHigh As System.Windows.Forms.RadioButton
    Friend WithEvents rdoLow As System.Windows.Forms.RadioButton
    Friend WithEvents lblHighLow As System.Windows.Forms.Label
    Friend WithEvents lblThreshold As System.Windows.Forms.Label
    Friend WithEvents txtThreshold As System.Windows.Forms.TextBox
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents txtDaysGapAllowed As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lblMinMax As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSpecifyEventAttributes))
        Me.rdoHigh = New System.Windows.Forms.RadioButton
        Me.rdoLow = New System.Windows.Forms.RadioButton
        Me.lblHighLow = New System.Windows.Forms.Label
        Me.lblThreshold = New System.Windows.Forms.Label
        Me.txtThreshold = New System.Windows.Forms.TextBox
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnOk = New System.Windows.Forms.Button
        Me.lblMinMax = New System.Windows.Forms.Label
        Me.txtDaysGapAllowed = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'rdoHigh
        '
        Me.rdoHigh.Checked = True
        Me.rdoHigh.Location = New System.Drawing.Point(32, 28)
        Me.rdoHigh.Name = "rdoHigh"
        Me.rdoHigh.Size = New System.Drawing.Size(128, 16)
        Me.rdoHigh.TabIndex = 2
        Me.rdoHigh.TabStop = True
        Me.rdoHigh.Text = "Above"
        '
        'rdoLow
        '
        Me.rdoLow.Location = New System.Drawing.Point(32, 50)
        Me.rdoLow.Name = "rdoLow"
        Me.rdoLow.Size = New System.Drawing.Size(128, 16)
        Me.rdoLow.TabIndex = 3
        Me.rdoLow.Text = "Below"
        '
        'lblHighLow
        '
        Me.lblHighLow.AutoSize = True
        Me.lblHighLow.Location = New System.Drawing.Point(12, 12)
        Me.lblHighLow.Name = "lblHighLow"
        Me.lblHighLow.Size = New System.Drawing.Size(205, 13)
        Me.lblHighLow.TabIndex = 1
        Me.lblHighLow.Text = "Select values Above or Below Threshold?"
        '
        'lblThreshold
        '
        Me.lblThreshold.AutoSize = True
        Me.lblThreshold.Location = New System.Drawing.Point(12, 80)
        Me.lblThreshold.Name = "lblThreshold"
        Me.lblThreshold.Size = New System.Drawing.Size(86, 13)
        Me.lblThreshold.TabIndex = 4
        Me.lblThreshold.Text = "Threshold value:"
        '
        'txtThreshold
        '
        Me.txtThreshold.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtThreshold.Location = New System.Drawing.Point(150, 77)
        Me.txtThreshold.Name = "txtThreshold"
        Me.txtThreshold.Size = New System.Drawing.Size(134, 20)
        Me.txtThreshold.TabIndex = 5
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(220, 160)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(64, 24)
        Me.btnCancel.TabIndex = 8
        Me.btnCancel.Text = "Cancel"
        '
        'btnOk
        '
        Me.btnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOk.Location = New System.Drawing.Point(150, 160)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(64, 24)
        Me.btnOk.TabIndex = 7
        Me.btnOk.Text = "Ok"
        '
        'lblMinMax
        '
        Me.lblMinMax.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblMinMax.AutoSize = True
        Me.lblMinMax.Location = New System.Drawing.Point(12, 132)
        Me.lblMinMax.Name = "lblMinMax"
        Me.lblMinMax.Size = New System.Drawing.Size(88, 13)
        Me.lblMinMax.TabIndex = 6
        Me.lblMinMax.Text = "Range of values:"
        '
        'txtDaysGapAllowed
        '
        Me.txtDaysGapAllowed.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDaysGapAllowed.Location = New System.Drawing.Point(150, 103)
        Me.txtDaysGapAllowed.Name = "txtDaysGapAllowed"
        Me.txtDaysGapAllowed.Size = New System.Drawing.Size(134, 20)
        Me.txtDaysGapAllowed.TabIndex = 10
        Me.txtDaysGapAllowed.Text = "0"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 106)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(106, 13)
        Me.Label1.TabIndex = 9
        Me.Label1.Text = "Days of gap allowed:"
        '
        'frmSpecifyEventAttributes
        '
        Me.AcceptButton = Me.btnOk
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(296, 196)
        Me.Controls.Add(Me.txtDaysGapAllowed)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOk)
        Me.Controls.Add(Me.lblMinMax)
        Me.Controls.Add(Me.txtThreshold)
        Me.Controls.Add(Me.lblThreshold)
        Me.Controls.Add(Me.lblHighLow)
        Me.Controls.Add(Me.rdoLow)
        Me.Controls.Add(Me.rdoHigh)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmSpecifyEventAttributes"
        Me.Text = "Specify Event Attributes"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region
    Public Function AskUser(ByRef aHigh As Boolean, ByRef aThreshold As Double, ByRef aDaysGapAllowed As Double, ByVal aMinMax As String) As Boolean
        lblMinMax.Text = "Range of values: " & aMinMax
        Me.ShowDialog()
        If pOk Then
            aHigh = rdoHigh.Checked
            aThreshold = CDbl(txtThreshold.Text)
            aDaysGapAllowed = CDbl(txtDaysGapAllowed.Text)
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
End Class
