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

    Public LabelAccept As String = "Ok"
    Public LabelCancel As String = "Cancel"
    Public LabelTimeout As String = "Cancel"
    Public TimeoutSeconds As Integer = 0

    Public RegistryAppName As String = ""
    Public RegistrySection As String = ""
    Public RegistryKey As String = ""
    Public RegistryCheckboxText As String = "Always Use This Answer"

    Private pLabelClicked As String
    Private pMargin As Integer = 12

    Public Function AskUser(ByVal aTitle As String, _
                            ByVal aMessage As String, _
                            ByVal ParamArray aButtonLabels() As String) As String
        Return AskUser(aTitle, aMessage, Array.AsReadOnly(aButtonLabels))
    End Function

    Public Function AskUser(ByVal aTitle As String, _
                            ByVal aMessage As String, _
                            ByVal aButtonLabels As IEnumerable) As String
        Dim lButtonLeft As Integer = pMargin
        Dim lButtonTop As Integer = pMargin
        Dim lChkAlways As CheckBox = Nothing

        If RegistryAppName.Length > 0 AndAlso RegistrySection.Length > 0 AndAlso RegistryKey.Length > 0 Then
            Dim lRegistryLabel As String = GetSetting(RegistryAppName, RegistrySection, RegistryKey, "")
            If lRegistryLabel.Length > 0 Then
                Return lRegistryLabel
            Else
                lChkAlways = New CheckBox
                lChkAlways.Text = RegistryCheckboxText
                Me.Controls.Add(lChkAlways)
                lChkAlways.Left = pMargin
            End If
        End If

        Text = aTitle
        Dim lButtonAnchor As System.Windows.Forms.AnchorStyles = AnchorStyles.Bottom + AnchorStyles.Left
        Dim lHorizontal As Boolean = True
        If aMessage.StartsWith("VERTICAL") Then
            aMessage = aMessage.Substring(8)
            lHorizontal = False
            lButtonAnchor = AnchorStyles.Top + AnchorStyles.Left
        End If

        Dim lNoMessage As Boolean = String.IsNullOrEmpty(aMessage)
        If lNoMessage Then
            lblMessage.Text = ""
        Else
            lblMessage.Text = aMessage
        End If

        Dim lButtons As New Generic.List(Of System.Windows.Forms.Button)
        Dim lSetHeight As Boolean = False
        Dim lMaxButtonWidth As Integer = 0

        For Each curLabel As String In aButtonLabels
            Dim btn As System.Windows.Forms.Button = New System.Windows.Forms.Button
            btn.Anchor = lButtonAnchor
            btn.AutoSize = True
            btn.Left = lButtonLeft

            Dim lLabel As String = curLabel
            btn.Tag = lLabel
            While lLabel.StartsWith("+") OrElse lLabel.StartsWith("-")
                If lLabel.StartsWith("+") Then
                    LabelAccept = btn.Tag
                Else
                    LabelCancel = btn.Tag
                End If
                lLabel = lLabel.Substring(1)
            End While

            If btn.Tag.ToLower = LabelAccept.ToLower Then Me.AcceptButton = btn
            If btn.Tag.ToLower = LabelCancel.ToLower Then Me.CancelButton = btn

            btn.Text = lLabel

            If Not lSetHeight Then
                If lNoMessage Then
                    Me.Height = lblMessage.Top + btn.Height + pMargin + Me.Height - Me.ClientSize.Height
                Else
                    Me.Height = lblMessage.Top + lblMessage.Height + pMargin + btn.Height + pMargin + Me.Height - Me.ClientSize.Height
                End If
                If lChkAlways IsNot Nothing Then
                    Me.Height += lChkAlways.Height + pMargin
                    lChkAlways.Top = Me.ClientSize.Height - btn.Height - pMargin - lChkAlways.Height - pMargin
                    lChkAlways.Anchor = AnchorStyles.Bottom + AnchorStyles.Left
                End If
                lSetHeight = True
                lButtonTop = Me.ClientSize.Height - btn.Height - pMargin
            End If

            btn.Top = lButtonTop

            lButtons.Add(btn)
            Me.Controls.Add(btn)
            If lHorizontal Then
                lButtonLeft += btn.Width + pMargin
            Else
                Me.Height += btn.Height + pMargin
                lButtonTop += btn.Height + pMargin
                If btn.Width > lMaxButtonWidth Then lMaxButtonWidth = btn.Width
            End If

            AddHandler btn.Click, AddressOf btnClick

        Next
        If lHorizontal Then
            Dim lMoveButtons As Integer = (lblMessage.Left + lblMessage.Width + pMargin - lButtonLeft) / 2
            If lMoveButtons < 0 Then
                Me.Width = lButtonLeft
            Else
                Me.Width = lblMessage.Left + lblMessage.Width + pMargin

                For Each lButton As System.Windows.Forms.Button In lButtons
                    lButton.Left += lMoveButtons
                Next
            End If
        Else
            For Each lButton As System.Windows.Forms.Button In lButtons
                lButton.Width = lMaxButtonWidth
            Next
            If lblMessage.Width > lMaxButtonWidth Then lMaxButtonWidth = lblMessage.Width
            lMaxButtonWidth += pMargin * 2
            If lMaxButtonWidth > Me.Width Then Me.Width = lMaxButtonWidth
        End If

        Me.Show()
        Me.BringToFront()

        Dim lTimeLimit As Double = Date.Now.AddSeconds(TimeoutSeconds).ToOADate
        pLabelClicked = ""
        While pLabelClicked.Length = 0
            Application.DoEvents()
            System.Threading.Thread.Sleep(100)
            If TimeoutSeconds > 0 AndAlso Date.Now.ToOADate > lTimeLimit Then
                pLabelClicked = LabelCancel
            End If
        End While

        Me.Visible = False

        For Each lButton As System.Windows.Forms.Button In lButtons
            Me.Controls.Remove(lButton)
        Next

        If lChkAlways IsNot Nothing Then
            If lChkAlways.Checked Then
                SaveSetting(RegistryAppName, RegistrySection, RegistryKey, pLabelClicked)
            End If
            Me.Controls.Remove(lChkAlways)
        End If

        Return pLabelClicked
    End Function

    Private Sub btnClick(ByVal sender As Object, ByVal e As System.EventArgs)
        pLabelClicked = sender.Tag
    End Sub

    Protected Overrides Sub OnClosing(ByVal e As System.ComponentModel.CancelEventArgs)
        If pLabelClicked.Length = 0 Then
            pLabelClicked = LabelCancel
        End If
    End Sub
End Class
