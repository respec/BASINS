Public Class frmSpecifyNdayHighLow
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
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtNDays As System.Windows.Forms.TextBox
    Friend WithEvents txtStartDay As System.Windows.Forms.TextBox
    Friend WithEvents cboStartMonth As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSpecifyNdayHighLow))
        Me.btnOk = New System.Windows.Forms.Button
        Me.txtNDays = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtStartDay = New System.Windows.Forms.TextBox
        Me.cboStartMonth = New System.Windows.Forms.ComboBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'btnOk
        '
        Me.btnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOk.Location = New System.Drawing.Point(154, 89)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(80, 24)
        Me.btnOk.TabIndex = 7
        Me.btnOk.Text = "Ok"
        '
        'txtNDays
        '
        Me.txtNDays.Location = New System.Drawing.Point(108, 19)
        Me.txtNDays.Name = "txtNDays"
        Me.txtNDays.Size = New System.Drawing.Size(40, 20)
        Me.txtNDays.TabIndex = 9
        Me.txtNDays.Text = "1"
        Me.txtNDays.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 22)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(83, 13)
        Me.Label1.TabIndex = 8
        Me.Label1.Text = "Number of Days"
        '
        'txtStartDay
        '
        Me.txtStartDay.Location = New System.Drawing.Point(194, 46)
        Me.txtStartDay.Name = "txtStartDay"
        Me.txtStartDay.Size = New System.Drawing.Size(40, 20)
        Me.txtStartDay.TabIndex = 14
        Me.txtStartDay.Text = "1"
        Me.txtStartDay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'cboStartMonth
        '
        Me.cboStartMonth.Items.AddRange(New Object() {"January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"})
        Me.cboStartMonth.Location = New System.Drawing.Point(108, 45)
        Me.cboStartMonth.Name = "cboStartMonth"
        Me.cboStartMonth.Size = New System.Drawing.Size(80, 21)
        Me.cboStartMonth.TabIndex = 13
        Me.cboStartMonth.Text = "January"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 48)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(59, 13)
        Me.Label2.TabIndex = 12
        Me.Label2.Text = "Year Starts"
        '
        'frmSpecifyNdayHighLow
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(246, 125)
        Me.Controls.Add(Me.txtStartDay)
        Me.Controls.Add(Me.txtNDays)
        Me.Controls.Add(Me.cboStartMonth)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnOk)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmSpecifyNdayHighLow"
        Me.Text = "Specify N-day"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private pOk As Boolean = False

    Public Function AskUser(ByRef aStartMonth As Integer, ByRef aStartDay As Integer, _
                            ByRef aNumDays As Integer) As Boolean
        If aStartMonth >= 1 And aStartMonth <= 12 Then
            cboStartMonth.Text = cboStartMonth.Items.Item(aStartMonth - 1)
        End If
        If aStartDay >= 1 And aStartDay <= 31 Then
            txtStartDay.Text = aStartDay
        End If

        If aNumDays >= 1 Then
            txtNDays.Text = CStr(aNumDays)
        End If

        'If aHigh Then
        '  radioHigh.Checked = True
        'Else
        '  radioLow.Checked = True
        'End If

        Me.ShowDialog()

        If pOk Then
            aStartMonth = cboStartMonth.SelectedIndex + 1
            aStartDay = CInt(txtStartDay.Text)
            aNumDays = CInt(txtNDays.Text)
            'aHigh = radioHigh.Checked
        End If
        Return pOk
    End Function

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        If Not IsNumeric(txtNDays.Text) Then
            MsgBox("Enter a number for the number of days")
        ElseIf Not IsNumeric(txtStartDay.Text) Then
            MsgBox("Enter a number for the starting day")
        Else
            Dim lNDays As Integer = CInt(txtNDays.Text)
            Dim lStartDay As Integer = CInt(txtStartDay.Text)
            If lStartDay < 1 Or lStartDay > 31 Then
                MsgBox("Enter a number between 1 and 31 for the starting day")
            ElseIf lNDays < 1 Or lNDays > 366 Then
                MsgBox("Enter a number between 1 and 365 for the starting day")
            Else
                pOk = True
                Me.Close()
            End If
        End If
    End Sub

End Class
