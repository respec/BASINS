Imports MapWinUtility

Public Class frmSpecifyYearSubset
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
    Friend WithEvents cboStartMonth As System.Windows.Forms.ComboBox
    Friend WithEvents txtStartDay As System.Windows.Forms.TextBox
    Friend WithEvents txtEndDay As System.Windows.Forms.TextBox
    Friend WithEvents cboEndMonth As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOk As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSpecifyYearSubset))
        Me.Label1 = New System.Windows.Forms.Label
        Me.cboStartMonth = New System.Windows.Forms.ComboBox
        Me.txtStartDay = New System.Windows.Forms.TextBox
        Me.txtEndDay = New System.Windows.Forms.TextBox
        Me.cboEndMonth = New System.Windows.Forms.ComboBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.btnOk = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 19)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(55, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Start Date"
        '
        'cboStartMonth
        '
        Me.cboStartMonth.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboStartMonth.Items.AddRange(New Object() {"January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"})
        Me.cboStartMonth.Location = New System.Drawing.Point(102, 16)
        Me.cboStartMonth.Name = "cboStartMonth"
        Me.cboStartMonth.Size = New System.Drawing.Size(88, 21)
        Me.cboStartMonth.TabIndex = 1
        Me.cboStartMonth.Text = "January"
        '
        'txtStartDay
        '
        Me.txtStartDay.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtStartDay.Location = New System.Drawing.Point(196, 16)
        Me.txtStartDay.Name = "txtStartDay"
        Me.txtStartDay.Size = New System.Drawing.Size(40, 20)
        Me.txtStartDay.TabIndex = 2
        Me.txtStartDay.Text = "1"
        Me.txtStartDay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtEndDay
        '
        Me.txtEndDay.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtEndDay.Location = New System.Drawing.Point(196, 56)
        Me.txtEndDay.Name = "txtEndDay"
        Me.txtEndDay.Size = New System.Drawing.Size(40, 20)
        Me.txtEndDay.TabIndex = 5
        Me.txtEndDay.Text = "31"
        Me.txtEndDay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'cboEndMonth
        '
        Me.cboEndMonth.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboEndMonth.Items.AddRange(New Object() {"January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"})
        Me.cboEndMonth.Location = New System.Drawing.Point(102, 56)
        Me.cboEndMonth.Name = "cboEndMonth"
        Me.cboEndMonth.Size = New System.Drawing.Size(88, 21)
        Me.cboEndMonth.TabIndex = 4
        Me.cboEndMonth.Text = "December"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 59)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(52, 13)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "End Date"
        '
        'btnOk
        '
        Me.btnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOk.Location = New System.Drawing.Point(102, 97)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(64, 24)
        Me.btnOk.TabIndex = 6
        Me.btnOk.Text = "Ok"
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(172, 97)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(64, 24)
        Me.btnCancel.TabIndex = 7
        Me.btnCancel.Text = "Cancel"
        '
        'frmSpecifyYearSubset
        '
        Me.AcceptButton = Me.btnOk
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(248, 133)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOk)
        Me.Controls.Add(Me.txtEndDay)
        Me.Controls.Add(Me.cboEndMonth)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtStartDay)
        Me.Controls.Add(Me.cboStartMonth)
        Me.Controls.Add(Me.Label1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmSpecifyYearSubset"
        Me.Text = "Specify Season"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private pOk As Boolean = False

    Public Function AskUser(ByRef aStartMonth As Integer, ByRef aStartDay As Integer, _
                            ByRef aEndMonth As Integer, ByRef aEndDay As Integer) As Boolean
        If aStartMonth >= 1 And aStartMonth <= 12 Then
            cboStartMonth.Text = cboStartMonth.Items.Item(aStartMonth - 1)
        End If
        If aEndMonth >= 1 And aEndMonth <= 12 Then
            cboEndMonth.Text = cboEndMonth.Items.Item(aEndMonth - 1)
        End If
        If aStartDay >= 1 And aStartDay <= 31 Then
            txtStartDay.Text = aStartDay
        End If
        If aEndDay >= 1 And aEndDay <= 31 Then
            txtEndDay.Text = aEndDay
        End If

        Me.ShowDialog()

        If pOk Then
            aStartMonth = cboStartMonth.SelectedIndex + 1
            aEndMonth = cboEndMonth.SelectedIndex + 1
            aStartDay = CInt(txtStartDay.Text)
            aEndDay = CInt(txtEndDay.Text)
        End If
        Return pOk
    End Function

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        If Not IsNumeric(txtStartDay.Text) Then
            Logger.Msg("Enter a number for the starting day")
        ElseIf Not IsNumeric(txtEndDay.Text) Then
            Logger.Msg("Enter a number for the ending day")
        Else
            Dim lStartDay As Integer = CInt(txtStartDay.Text)
            Dim lEndDay As Integer = CInt(txtEndDay.Text)
            If lStartDay < 1 Or lStartDay > 31 Then
                Logger.Msg("Enter a number between 1 and 31 for the starting day")
            ElseIf lEndDay < 1 Or lStartDay > 31 Then
                Logger.Msg("Enter a number between 1 and 31 for the ending day")
            Else
                pOk = True
                Me.Close()
            End If
        End If
    End Sub
End Class
