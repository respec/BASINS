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
  Friend WithEvents panelBottom As System.Windows.Forms.Panel
  Friend WithEvents btnCancel As System.Windows.Forms.Button
  Friend WithEvents btnOk As System.Windows.Forms.Button
  Friend WithEvents lblMinMax As System.Windows.Forms.Label
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
    Me.rdoHigh = New System.Windows.Forms.RadioButton
    Me.rdoLow = New System.Windows.Forms.RadioButton
    Me.lblHighLow = New System.Windows.Forms.Label
    Me.lblThreshold = New System.Windows.Forms.Label
    Me.txtThreshold = New System.Windows.Forms.TextBox
    Me.panelBottom = New System.Windows.Forms.Panel
    Me.btnCancel = New System.Windows.Forms.Button
    Me.btnOk = New System.Windows.Forms.Button
    Me.lblMinMax = New System.Windows.Forms.Label
    Me.panelBottom.SuspendLayout()
    Me.SuspendLayout()
    '
    'rdoHigh
    '
    Me.rdoHigh.Checked = True
    Me.rdoHigh.Location = New System.Drawing.Point(32, 40)
    Me.rdoHigh.Name = "rdoHigh"
    Me.rdoHigh.Size = New System.Drawing.Size(128, 16)
    Me.rdoHigh.TabIndex = 0
    Me.rdoHigh.TabStop = True
    Me.rdoHigh.Text = "Above"
    '
    'rdoLow
    '
    Me.rdoLow.Location = New System.Drawing.Point(32, 64)
    Me.rdoLow.Name = "rdoLow"
    Me.rdoLow.Size = New System.Drawing.Size(128, 16)
    Me.rdoLow.TabIndex = 1
    Me.rdoLow.Text = "Below"
    '
    'lblHighLow
    '
    Me.lblHighLow.Location = New System.Drawing.Point(16, 16)
    Me.lblHighLow.Name = "lblHighLow"
    Me.lblHighLow.Size = New System.Drawing.Size(224, 16)
    Me.lblHighLow.TabIndex = 2
    Me.lblHighLow.Text = "Select values Above or Below Threshold?"
    '
    'lblThreshold
    '
    Me.lblThreshold.Location = New System.Drawing.Point(16, 96)
    Me.lblThreshold.Name = "lblThreshold"
    Me.lblThreshold.Size = New System.Drawing.Size(128, 16)
    Me.lblThreshold.TabIndex = 3
    Me.lblThreshold.Text = "Specify Threshold value:"
    '
    'txtThreshold
    '
    Me.txtThreshold.Location = New System.Drawing.Point(144, 96)
    Me.txtThreshold.Name = "txtThreshold"
    Me.txtThreshold.TabIndex = 4
    Me.txtThreshold.Text = ""
    '
    'panelBottom
    '
    Me.panelBottom.Controls.Add(Me.btnCancel)
    Me.panelBottom.Controls.Add(Me.btnOk)
    Me.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom
    Me.panelBottom.Location = New System.Drawing.Point(0, 157)
    Me.panelBottom.Name = "panelBottom"
    Me.panelBottom.Size = New System.Drawing.Size(256, 32)
    Me.panelBottom.TabIndex = 16
    '
    'btnCancel
    '
    Me.btnCancel.Location = New System.Drawing.Point(128, 0)
    Me.btnCancel.Name = "btnCancel"
    Me.btnCancel.Size = New System.Drawing.Size(64, 24)
    Me.btnCancel.TabIndex = 1
    Me.btnCancel.Text = "Cancel"
    '
    'btnOk
    '
    Me.btnOk.Location = New System.Drawing.Point(40, 0)
    Me.btnOk.Name = "btnOk"
    Me.btnOk.Size = New System.Drawing.Size(64, 24)
    Me.btnOk.TabIndex = 0
    Me.btnOk.Text = "Ok"
    '
    'lblMinMax
    '
    Me.lblMinMax.Location = New System.Drawing.Point(16, 120)
    Me.lblMinMax.Name = "lblMinMax"
    Me.lblMinMax.Size = New System.Drawing.Size(232, 16)
    Me.lblMinMax.TabIndex = 17
    Me.lblMinMax.Text = "Range of values:"
    '
    'frmSpecifyEventAttributes
    '
    Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
    Me.ClientSize = New System.Drawing.Size(256, 189)
    Me.Controls.Add(Me.lblMinMax)
    Me.Controls.Add(Me.panelBottom)
    Me.Controls.Add(Me.txtThreshold)
    Me.Controls.Add(Me.lblThreshold)
    Me.Controls.Add(Me.lblHighLow)
    Me.Controls.Add(Me.rdoLow)
    Me.Controls.Add(Me.rdoHigh)
    Me.Name = "frmSpecifyEventAttributes"
    Me.Text = "Specify Event Attributes"
    Me.panelBottom.ResumeLayout(False)
    Me.ResumeLayout(False)

  End Sub

#End Region
  Public Function AskUser(ByRef aHigh As Boolean, ByRef aThreshold As Double, ByVal aMinMax As String) As Boolean
    'pDataManager = aDataManager
    lblMinMax.Text = "Range of values: " & aMinMax
    Me.ShowDialog()
    If pOk Then
      aHigh = rdoHigh.Checked
      aThreshold = CDbl(txtThreshold.Text)
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
