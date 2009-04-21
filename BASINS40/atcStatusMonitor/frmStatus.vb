Public Class frmStatus
    Inherits System.Windows.Forms.Form

    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnPause As System.Windows.Forms.Button
    Friend WithEvents btnDetails As System.Windows.Forms.Button
    Public Exiting As Boolean = False

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
    Friend WithEvents lblTop As System.Windows.Forms.Label
    Friend WithEvents txtLog As System.Windows.Forms.TextBox
    Friend WithEvents lblRight As System.Windows.Forms.Label
    Friend WithEvents Progress As System.Windows.Forms.ProgressBar
    Friend WithEvents lblLeft As System.Windows.Forms.Label
    Friend WithEvents lblMiddle As System.Windows.Forms.Label
    Friend WithEvents lblBottom As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmStatus))
        Me.Progress = New System.Windows.Forms.ProgressBar
        Me.lblTop = New System.Windows.Forms.Label
        Me.txtLog = New System.Windows.Forms.TextBox
        Me.lblLeft = New System.Windows.Forms.Label
        Me.lblMiddle = New System.Windows.Forms.Label
        Me.lblRight = New System.Windows.Forms.Label
        Me.lblBottom = New System.Windows.Forms.Label
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnPause = New System.Windows.Forms.Button
        Me.btnDetails = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'Progress
        '
        Me.Progress.Location = New System.Drawing.Point(8, 72)
        Me.Progress.Name = "Progress"
        Me.Progress.Size = New System.Drawing.Size(497, 24)
        Me.Progress.TabIndex = 0
        '
        'lblTop
        '
        Me.lblTop.AutoSize = True
        Me.lblTop.BackColor = System.Drawing.Color.Transparent
        Me.lblTop.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTop.Location = New System.Drawing.Point(8, 8)
        Me.lblTop.Name = "lblTop"
        Me.lblTop.Size = New System.Drawing.Size(42, 13)
        Me.lblTop.TabIndex = 1
        Me.lblTop.Text = "lblTop"
        '
        'txtLog
        '
        Me.txtLog.Location = New System.Drawing.Point(8, 182)
        Me.txtLog.Multiline = True
        Me.txtLog.Name = "txtLog"
        Me.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtLog.Size = New System.Drawing.Size(497, 244)
        Me.txtLog.TabIndex = 2
        '
        'lblLeft
        '
        Me.lblLeft.AutoSize = True
        Me.lblLeft.BackColor = System.Drawing.Color.Transparent
        Me.lblLeft.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLeft.Location = New System.Drawing.Point(8, 40)
        Me.lblLeft.Name = "lblLeft"
        Me.lblLeft.Size = New System.Drawing.Size(42, 13)
        Me.lblLeft.TabIndex = 3
        Me.lblLeft.Text = "lblLeft"
        '
        'lblMiddle
        '
        Me.lblMiddle.BackColor = System.Drawing.Color.Transparent
        Me.lblMiddle.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMiddle.Location = New System.Drawing.Point(8, 40)
        Me.lblMiddle.Name = "lblMiddle"
        Me.lblMiddle.Size = New System.Drawing.Size(497, 16)
        Me.lblMiddle.TabIndex = 4
        Me.lblMiddle.Text = "lblMiddle"
        Me.lblMiddle.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblRight
        '
        Me.lblRight.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblRight.AutoSize = True
        Me.lblRight.BackColor = System.Drawing.Color.Transparent
        Me.lblRight.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRight.Location = New System.Drawing.Point(458, 40)
        Me.lblRight.Name = "lblRight"
        Me.lblRight.Size = New System.Drawing.Size(50, 13)
        Me.lblRight.TabIndex = 5
        Me.lblRight.Text = "lblRight"
        Me.lblRight.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblBottom
        '
        Me.lblBottom.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblBottom.BackColor = System.Drawing.Color.Transparent
        Me.lblBottom.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBottom.Location = New System.Drawing.Point(8, 112)
        Me.lblBottom.Name = "lblBottom"
        Me.lblBottom.Size = New System.Drawing.Size(497, 16)
        Me.lblBottom.TabIndex = 6
        Me.lblBottom.Text = "lblBottom"
        Me.lblBottom.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'btnCancel
        '
        Me.btnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancel.Location = New System.Drawing.Point(11, 131)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(65, 26)
        Me.btnCancel.TabIndex = 7
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnPause
        '
        Me.btnPause.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPause.Location = New System.Drawing.Point(82, 131)
        Me.btnPause.Name = "btnPause"
        Me.btnPause.Size = New System.Drawing.Size(65, 26)
        Me.btnPause.TabIndex = 8
        Me.btnPause.Text = "Pause"
        Me.btnPause.UseVisualStyleBackColor = True
        '
        'btnDetails
        '
        Me.btnDetails.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDetails.Location = New System.Drawing.Point(153, 131)
        Me.btnDetails.Name = "btnDetails"
        Me.btnDetails.Size = New System.Drawing.Size(65, 26)
        Me.btnDetails.TabIndex = 9
        Me.btnDetails.Text = "Details"
        Me.btnDetails.UseVisualStyleBackColor = True
        '
        'frmStatus
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(514, 169)
        Me.Controls.Add(Me.btnDetails)
        Me.Controls.Add(Me.btnPause)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.lblBottom)
        Me.Controls.Add(Me.lblRight)
        Me.Controls.Add(Me.lblLeft)
        Me.Controls.Add(Me.txtLog)
        Me.Controls.Add(Me.lblTop)
        Me.Controls.Add(Me.Progress)
        Me.Controls.Add(Me.lblMiddle)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmStatus"
        Me.Text = "Status "
        Me.TopMost = True
        Me.WindowState = System.Windows.Forms.FormWindowState.Minimized
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Friend ReadOnly LastLabel As Integer = 5

    Public Property Label(ByVal aIndex As Integer) As String
        Get
            Select Case aIndex
                Case 0 : Return Me.Text
                Case 1 : Return lblTop.Text
                Case 2 : Return lblLeft.Text
                Case 3 : Return lblMiddle.Text
                Case 4 : Return lblRight.Text
                Case 5 : Return lblBottom.Text
                Case Else : Return ""
            End Select
        End Get
        Set(ByVal aNewValue As String)
            Select Case aIndex
                Case 0 : Me.Text = aNewValue
                Case 1 : lblTop.Text = aNewValue
                Case 2 : lblLeft.Text = aNewValue
                Case 3 : lblMiddle.Text = aNewValue
                Case 4 : lblRight.Text = aNewValue : lblRight.Left = Me.ClientRectangle.Width - lblRight.Width - lblLeft.Left
                Case 5 : lblBottom.Text = aNewValue
            End Select
        End Set
    End Property

    Public Sub Clear()
        For lLabelIndex As Integer = 0 To LastLabel
            Label(lLabelIndex) = ""
        Next
        txtLog.Clear()
        Progress.Visible = False
    End Sub

    Private Sub frmStatus_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        If Not Exiting Then
            e.Cancel = True
        End If
        Me.Visible = False
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Clear()
        Label(1) = "Cancelling"
        Console.WriteLine("C")
    End Sub

    Private Sub btnPause_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPause.Click
        If btnPause.Text = "Pause" Then
            Console.WriteLine("P")
            btnPause.Text = "Run"
        Else
            btnPause.Text = "Pause"
            Console.WriteLine("R")
        End If
    End Sub

    Private Sub btnDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDetails.Click
        Dim lHeightHidingDetails As Integer = btnDetails.Top + btnDetails.Height + 46
        If Me.Height > lHeightHidingDetails Then
            Me.Height = lHeightHidingDetails
        Else
            Me.Height = 700
        End If
    End Sub

    Private Sub frmStatus_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        Try
            lblRight.Left = Me.ClientRectangle.Width - lblRight.Width - lblLeft.Left
            Dim lControlWidth As Integer = Me.ClientRectangle.Width - Progress.Left * 2
            lblMiddle.Width = lControlWidth
            Progress.Width = lControlWidth
            If Me.Height > txtLog.Top + txtLog.Left * 3 Then
                txtLog.Width = lControlWidth
                txtLog.Height = Me.ClientRectangle.Height - txtLog.Top - txtLog.Left
                txtLog.Visible = True
            Else
                txtLog.Visible = False
            End If
        Catch
        End Try
    End Sub

End Class
