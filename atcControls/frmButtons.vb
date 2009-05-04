Public Class frmButtons
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
    Friend WithEvents lblMessage As System.Windows.Forms.Label

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.lblMessage = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'lblMessage
        '
        Me.lblMessage.AutoSize = True
        Me.lblMessage.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMessage.Location = New System.Drawing.Point(12, 21)
        Me.lblMessage.Name = "lblMessage"
        Me.lblMessage.Size = New System.Drawing.Size(89, 20)
        Me.lblMessage.TabIndex = 0
        Me.lblMessage.Text = "lblMessage"
        '
        'frmButtons
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(365, 152)
        Me.Controls.Add(Me.lblMessage)
        Me.Name = "frmButtons"
        Me.Text = "frmButtons"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private pLabelClicked As String
    Private pLabelCancel As String = "Cancel"
    Private pMargin As Integer = 12

    Public Function AskUser(ByVal aTitle As String, _
                            ByVal aMessage As String, _
                            ByVal aLabels As IEnumerable, _
                   Optional ByVal aTimeoutSeconds As Integer = 0, _
                   Optional ByVal aTimeoutLabel As String = "Cancel") As String
        Text = aTitle
        lblMessage.Text = aMessage
        Dim lButtonLeft As Integer = pMargin
        Dim lButtons As New Generic.List(Of Windows.Forms.Button)
        Dim lSetHeight As Boolean = False

        For Each curLabel As String In aLabels
            Dim btn As Windows.Forms.Button = New Windows.Forms.Button
            btn.Anchor = AnchorStyles.Bottom
            btn.AutoSize = True
            btn.Left = lButtonLeft

            Dim lLabel As String = curLabel
            btn.Tag = lLabel
            While lLabel.StartsWith("+") OrElse lLabel.StartsWith("-")
                If lLabel.StartsWith("+") Then
                    Me.AcceptButton = btn
                    btn.DialogResult = Windows.Forms.DialogResult.OK
                    lLabel = lLabel.Substring(1)
                End If
                If lLabel.StartsWith("-") Then
                    Me.CancelButton = btn
                    btn.DialogResult = Windows.Forms.DialogResult.Cancel
                    lLabel = lLabel.Substring(1)
                    pLabelCancel = btn.Tag
                End If
            End While

            btn.Text = lLabel

            If Not lSetHeight Then
                Me.Height = lblMessage.Top + lblMessage.Height + pMargin + btn.Height + pMargin + Me.Height - Me.ClientSize.Height
                lSetHeight = True
            End If

            btn.Top = Me.ClientSize.Height - btn.Height - pMargin

            lButtons.Add(btn)
            Me.Controls.Add(btn)
            lButtonLeft += btn.Width + pMargin

            AddHandler btn.Click, AddressOf btnClick

        Next

        If lButtonLeft > lblMessage.Width + pMargin Then
            Me.Width = lButtonLeft
        Else
            Me.Width = lblMessage.Width + pMargin
        End If

        Dim lStartTime As Integer = Date.Now.ToOADate

        Me.Show()
        Me.BringToFront()

        pLabelClicked = ""
        While pLabelClicked.Length = 0
            Application.DoEvents()
            System.Threading.Thread.Sleep(100)
        End While

        Me.Visible = False

        For Each lButton As Windows.Forms.Button In lButtons
            Me.Controls.Remove(lButton)
        Next

        Return pLabelClicked
    End Function

    Private Sub btnClick(ByVal sender As Object, ByVal e As System.EventArgs)
        pLabelClicked = sender.Tag
    End Sub

    Protected Overrides Sub OnClosing(ByVal e As System.ComponentModel.CancelEventArgs)
        If pLabelClicked.Length = 0 Then
            pLabelClicked = pLabelCancel
        End If
    End Sub
End Class
