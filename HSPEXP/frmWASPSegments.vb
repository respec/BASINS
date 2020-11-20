Imports MapWinUtility

Public Class frmWASPSegments
    Inherits System.Windows.Forms.Form

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
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtNumber As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents rbnYes As RadioButton
    Friend WithEvents rbnNo As RadioButton
    Friend WithEvents btnOk As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmWASPSegments))
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtNumber = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.btnOk = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.rbnYes = New System.Windows.Forms.RadioButton()
        Me.rbnNo = New System.Windows.Forms.RadioButton()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 19)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(206, 15)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Number of WASP Surface Segments"
        '
        'txtNumber
        '
        Me.txtNumber.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtNumber.Location = New System.Drawing.Point(247, 16)
        Me.txtNumber.Name = "txtNumber"
        Me.txtNumber.Size = New System.Drawing.Size(40, 20)
        Me.txtNumber.TabIndex = 2
        Me.txtNumber.Text = "1"
        Me.txtNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 59)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(150, 15)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Include Benthic Segments"
        '
        'btnOk
        '
        Me.btnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOk.Location = New System.Drawing.Point(152, 98)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(65, 24)
        Me.btnOk.TabIndex = 6
        Me.btnOk.Text = "OK"
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(222, 98)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(65, 24)
        Me.btnCancel.TabIndex = 7
        Me.btnCancel.Text = "Cancel"
        '
        'rbnYes
        '
        Me.rbnYes.AutoSize = True
        Me.rbnYes.Location = New System.Drawing.Point(191, 59)
        Me.rbnYes.Name = "rbnYes"
        Me.rbnYes.Size = New System.Drawing.Size(45, 19)
        Me.rbnYes.TabIndex = 8
        Me.rbnYes.Text = "Yes"
        Me.rbnYes.UseVisualStyleBackColor = True
        '
        'rbnNo
        '
        Me.rbnNo.AutoSize = True
        Me.rbnNo.Checked = True
        Me.rbnNo.Location = New System.Drawing.Point(242, 59)
        Me.rbnNo.Name = "rbnNo"
        Me.rbnNo.Size = New System.Drawing.Size(41, 19)
        Me.rbnNo.TabIndex = 9
        Me.rbnNo.TabStop = True
        Me.rbnNo.Text = "No"
        Me.rbnNo.UseVisualStyleBackColor = True
        '
        'frmWASPSegments
        '
        Me.AcceptButton = Me.btnOk
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(299, 133)
        Me.Controls.Add(Me.rbnNo)
        Me.Controls.Add(Me.rbnYes)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOk)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtNumber)
        Me.Controls.Add(Me.Label1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmWASPSegments"
        Me.Text = "WASP Segments"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private pOk As Boolean = False

    Public Function AskUser(ByRef aNumSegments As Integer, ByRef aBenthicSegments As Boolean) As Boolean

        Me.BringToFront()
        Me.ShowDialog()

        If pOk Then
            aNumSegments = CInt(txtNumber.Text)
            If rbnYes.Checked Then
                aBenthicSegments = True
            Else
                aBenthicSegments = False
            End If
        End If
        Return pOk
    End Function

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        If Not IsNumeric(txtNumber.Text) Then
            Logger.Msg("Enter the number of surface segments")
        Else
            pOk = True
            Me.Close()
        End If
    End Sub
End Class
