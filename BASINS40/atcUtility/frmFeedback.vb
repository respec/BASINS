Imports MapWinUtility

Public Class frmFeedback
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
    Friend WithEvents lblName As System.Windows.Forms.Label
    Friend WithEvents txtName As System.Windows.Forms.TextBox
    Friend WithEvents lblEmail As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnSend As System.Windows.Forms.Button
    Friend WithEvents btnCopy As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents txtEmail As System.Windows.Forms.TextBox
    Friend WithEvents txtMessage As System.Windows.Forms.TextBox
    Friend WithEvents txtSystemInformation As System.Windows.Forms.TextBox
    Friend WithEvents chkSendSystemInformation As System.Windows.Forms.CheckBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmFeedback))
        Me.lblName = New System.Windows.Forms.Label
        Me.txtName = New System.Windows.Forms.TextBox
        Me.txtEmail = New System.Windows.Forms.TextBox
        Me.lblEmail = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtMessage = New System.Windows.Forms.TextBox
        Me.txtSystemInformation = New System.Windows.Forms.TextBox
        Me.btnSend = New System.Windows.Forms.Button
        Me.btnCopy = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.chkSendSystemInformation = New System.Windows.Forms.CheckBox
        Me.SuspendLayout()
        '
        'lblName
        '
        Me.lblName.Location = New System.Drawing.Point(16, 24)
        Me.lblName.Name = "lblName"
        Me.lblName.Size = New System.Drawing.Size(72, 16)
        Me.lblName.TabIndex = 0
        Me.lblName.Text = "Your &Name:"
        '
        'txtName
        '
        Me.txtName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtName.Location = New System.Drawing.Point(128, 16)
        Me.txtName.Name = "txtName"
        Me.txtName.Size = New System.Drawing.Size(264, 20)
        Me.txtName.TabIndex = 1
        Me.txtName.Text = ""
        '
        'txtEmail
        '
        Me.txtEmail.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtEmail.Location = New System.Drawing.Point(128, 40)
        Me.txtEmail.Name = "txtEmail"
        Me.txtEmail.Size = New System.Drawing.Size(264, 20)
        Me.txtEmail.TabIndex = 3
        Me.txtEmail.Text = ""
        '
        'lblEmail
        '
        Me.lblEmail.Location = New System.Drawing.Point(16, 48)
        Me.lblEmail.Name = "lblEmail"
        Me.lblEmail.Size = New System.Drawing.Size(128, 16)
        Me.lblEmail.TabIndex = 2
        Me.lblEmail.Text = "Your &Email Address:"
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(16, 80)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(192, 16)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Enter a &message to the developers:"
        '
        'txtMessage
        '
        Me.txtMessage.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtMessage.Location = New System.Drawing.Point(16, 104)
        Me.txtMessage.Multiline = True
        Me.txtMessage.Name = "txtMessage"
        Me.txtMessage.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtMessage.Size = New System.Drawing.Size(376, 96)
        Me.txtMessage.TabIndex = 5
        Me.txtMessage.Text = ""
        '
        'txtSystemInformation
        '
        Me.txtSystemInformation.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtSystemInformation.BackColor = System.Drawing.SystemColors.Control
        Me.txtSystemInformation.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSystemInformation.Location = New System.Drawing.Point(16, 232)
        Me.txtSystemInformation.Multiline = True
        Me.txtSystemInformation.Name = "txtSystemInformation"
        Me.txtSystemInformation.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtSystemInformation.Size = New System.Drawing.Size(376, 96)
        Me.txtSystemInformation.TabIndex = 6
        Me.txtSystemInformation.Text = ""
        '
        'btnSend
        '
        Me.btnSend.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSend.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnSend.Location = New System.Drawing.Point(144, 336)
        Me.btnSend.Name = "btnSend"
        Me.btnSend.Size = New System.Drawing.Size(72, 24)
        Me.btnSend.TabIndex = 8
        Me.btnSend.Text = "&Send"
        '
        'btnCopy
        '
        Me.btnCopy.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCopy.Location = New System.Drawing.Point(232, 336)
        Me.btnCopy.Name = "btnCopy"
        Me.btnCopy.Size = New System.Drawing.Size(72, 24)
        Me.btnCopy.TabIndex = 9
        Me.btnCopy.Text = "&Copy"
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(320, 336)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(72, 24)
        Me.btnCancel.TabIndex = 10
        Me.btnCancel.Text = "&Cancel"
        '
        'chkSendSystemInformation
        '
        Me.chkSendSystemInformation.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkSendSystemInformation.Checked = True
        Me.chkSendSystemInformation.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkSendSystemInformation.Location = New System.Drawing.Point(16, 208)
        Me.chkSendSystemInformation.Name = "chkSendSystemInformation"
        Me.chkSendSystemInformation.Size = New System.Drawing.Size(368, 16)
        Me.chkSendSystemInformation.TabIndex = 11
        Me.chkSendSystemInformation.Text = "Send the following system information with your message:"
        '
        'frmFeedback
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(400, 373)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnCopy)
        Me.Controls.Add(Me.btnSend)
        Me.Controls.Add(Me.txtSystemInformation)
        Me.Controls.Add(Me.txtMessage)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtEmail)
        Me.Controls.Add(Me.txtName)
        Me.Controls.Add(Me.lblName)
        Me.Controls.Add(Me.lblEmail)
        Me.Controls.Add(Me.chkSendSystemInformation)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmFeedback"
        Me.Text = "Feedback"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private pSend As Boolean = False

    Public Function ShowFeedback(ByRef aName As String, ByRef aEmail As String, ByRef aMessage As String, ByRef aSystemInformation As String) As Boolean
        txtName.Text = aName
        txtEmail.Text = aEmail
        txtMessage.Text = aMessage
        txtSystemInformation.Text = aSystemInformation
        Me.Show()
        Me.Refresh()

        If aSystemInformation.Length = 0 Then
            txtSystemInformation.Text = FeedbackGenericSystemInformation()
        End If

        While Me.Visible
            Windows.Forms.Application.DoEvents()
            System.Threading.Thread.Sleep(100)
        End While

        If pSend Then
            aName = txtName.Text
            aEmail = txtEmail.Text
            aMessage = txtMessage.Text
            If chkSendSystemInformation.Checked Then
                aSystemInformation = txtSystemInformation.Text
            Else
                aSystemInformation = "System information not sent"
            End If
        End If
        Me.Close()
        Return pSend
    End Function

    Private Sub btnCopy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCopy.Click
        Dim lFeedback As String = "Name: " & txtName.Text & vbCrLf
        lFeedback &= "Email: " & txtEmail.Text & vbCrLf
        lFeedback &= "Message: " & txtMessage.Text & vbCrLf
        lFeedback &= vbCrLf & txtSystemInformation.Text & vbCrLf
        Windows.Forms.Clipboard.SetDataObject(lFeedback)
    End Sub

    Private Sub btnSend_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSend.Click
        SaveSetting("BASINS4", "Feedback", "Name", txtName.Text)
        SaveSetting("BASINS4", "Feedback", "Email", txtEmail.Text)
        pSend = True
        Me.Hide()
    End Sub

    Private Sub frmFeedback_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        txtName.Text = GetSetting("BASINS4", "Feedback", "Name", "")
        txtEmail.Text = GetSetting("BASINS4", "Feedback", "Email", "")
    End Sub

    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        pSend = False
        Me.Hide()
    End Sub

    Public Function FeedbackGenericSystemInformation() As String
        'TODO: format as an html document?
        Dim lSectionFooter As String = "___________________________" & vbCrLf
        Dim lFeedback As String = "Feedback at " & Now.ToString("u") & vbCrLf
        lFeedback &= "CommandLine: " & Environment.CommandLine & vbCrLf
        lFeedback &= "User: " & Environment.UserName & vbCrLf
        lFeedback &= "Machine: " & Environment.MachineName & vbCrLf
        lFeedback &= "OSVersion: " & Environment.OSVersion.ToString & vbCrLf
        lFeedback &= "CLRVersion: " & Environment.Version.ToString & vbCrLf
        lFeedback &= "LogFile: " & Logger.FileName & vbCrLf
        If IO.File.Exists(Logger.FileName) Then
            Try
                lFeedback &= WholeFileString(Logger.FileName) & vbCrLf
            Catch e As Exception
                lFeedback &= vbCrLf & "Logger file read failed, exception message:" & _
                             vbCrLf & e.ToString & vbCrLf & vbCrLf
            End Try
        End If
        lFeedback &= lSectionFooter

        Return lFeedback
    End Function
End Class
