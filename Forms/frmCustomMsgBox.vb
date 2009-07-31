Imports System.Windows.Forms

Public Class frmCustomMsgBox
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
        'frmCustomMsgBox
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(365, 152)
        Me.Controls.Add(Me.lblMessage)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmCustomMsgBox"
        Me.ShowIcon = False
        Me.Text = "frmCustomMsgBox"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    ''' <summary>
    ''' Label of the default button of the message box
    ''' </summary>
    Public LabelAccept As String = "Ok"

    ''' <summary>
    ''' Label of the button activated by the "Esc" key
    ''' </summary>
    Public LabelCancel As String = "Cancel"

    ''' <summary>
    ''' Label to return when the timeout expires without the user pressing a button
    ''' </summary>
    Public LabelTimeout As String = "Cancel"

    ''' <summary>
    ''' Number of seconds to wait before returning LabelTimeout (not used if not greater than zero)
    ''' </summary>
    Public TimeoutSeconds As Integer = 0

    ''' <summary>
    ''' Application Name to get and set registry value for checkbox
    ''' </summary>
    Public RegistryAppName As String = ""

    ''' <summary>
    ''' Registry Section to get and set registry value for checkbox
    ''' </summary>
    Public RegistrySection As String = ""

    ''' <summary>
    ''' Registry Key to get and set registry value for checkbox
    ''' </summary>
    Public RegistryKey As String = ""

    ''' <summary>
    ''' Text displayed for checkbox that offers to never again show this message
    ''' </summary>
    ''' <remarks>Only used if RegistryAppName, RegistrySection, and RegistryKey are set</remarks>
    Public RegistryCheckboxText As String = "Always Use This Answer"

    ''' <summary>
    ''' Number of pixels left empty around edges and between controls
    ''' </summary>
    Public LayoutMargin As Integer = 12

    ''' <summary>
    ''' The label of the button that was clicked
    ''' </summary>
    Private pLabelClicked As String

    ''' <summary>
    ''' Display aTitle and aMessage and ask the user to choose a button from aButtonLabels
    ''' </summary>
    ''' <param name="aMessage">Text in main area of message box</param>
    ''' <param name="aTitle">Text for title area of message box</param>
    ''' <param name="aButtonLabels">Labels for buttons</param>
    ''' <returns>Label of button that was clicked</returns>
    Public Function AskUser(ByVal aMessage As String, _
                            ByVal aTitle As String, _
                            ByVal ParamArray aButtonLabels() As String) As String
        Return AskUser(aMessage, aTitle, Array.AsReadOnly(aButtonLabels))
    End Function

    ''' <summary>
    ''' Display aTitle and aMessage and ask the user to choose a button from aButtonLabels
    ''' </summary>
    ''' <param name="aMessage">Text in main area of message box</param>
    ''' <param name="aTitle">Text for title area of message box</param>
    ''' <param name="aButtonLabels">Labels for buttons</param>
    ''' <returns>Label of button that was clicked</returns>
    Public Function AskUser(ByVal aMessage As String, _
                            ByVal aTitle As String, _
                            ByVal aButtonLabels As IEnumerable) As String
        Dim lButtonLeft As Integer = LayoutMargin
        Dim lChkAlways As CheckBox = Nothing

        If RegistryAppName.Length > 0 AndAlso RegistrySection.Length > 0 AndAlso RegistryKey.Length > 0 Then
            Dim lRegistryLabel As String = GetSetting(RegistryAppName, RegistrySection, RegistryKey, "")
            If lRegistryLabel.Length > 0 Then
                Return lRegistryLabel
            Else
                lChkAlways = New CheckBox
                lChkAlways.AutoSize = True
                lChkAlways.Text = RegistryCheckboxText
                Me.Controls.Add(lChkAlways)
                lChkAlways.Left = LayoutMargin
            End If
        End If

        Text = aTitle
        lblMessage.Text = aMessage
        Dim lButtons As New Generic.List(Of Windows.Forms.Button)
        Dim lSetHeight As Boolean = False

        For Each curLabel As String In aButtonLabels
            Dim btn As Windows.Forms.Button = New Windows.Forms.Button
            btn.Anchor = AnchorStyles.Bottom + AnchorStyles.Left
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
                Me.Height = lblMessage.Top + lblMessage.Height + LayoutMargin + btn.Height + LayoutMargin + Me.Height - Me.ClientSize.Height
                If lChkAlways IsNot Nothing Then
                    Me.Height += lChkAlways.Height + LayoutMargin
                    lChkAlways.Top = Me.ClientSize.Height - btn.Height - LayoutMargin - lChkAlways.Height - LayoutMargin
                    lChkAlways.Anchor = AnchorStyles.Bottom + AnchorStyles.Left
                End If
                lSetHeight = True
            End If

            btn.Top = Me.ClientSize.Height - btn.Height - LayoutMargin

            lButtons.Add(btn)
            Me.Controls.Add(btn)
            lButtonLeft += btn.Width + LayoutMargin

            AddHandler btn.Click, AddressOf btnClick

        Next

        Dim lWidest As Integer = Math.Max(lButtonLeft, lblMessage.Left + lblMessage.Width + LayoutMargin)
        If lChkAlways IsNot Nothing Then
            lWidest = Math.Max(lWidest, LayoutMargin + lChkAlways.Width + LayoutMargin)
        End If
        Me.Width = lWidest

        lblMessage.Left = (Me.Width - lblMessage.Width) / 2
        If lChkAlways IsNot Nothing Then lChkAlways.Left = (Me.Width - lChkAlways.Width) / 2

        If lWidest > lButtonLeft Then
            Dim lMoveButtons As Integer = (lWidest - lButtonLeft) / 2
            For Each lButton As Windows.Forms.Button In lButtons
                lButton.Left += lMoveButtons
            Next
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

        For Each lButton As Windows.Forms.Button In lButtons
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
        'If the window is being closed before a buttons was clicked, pretend the Cancel button was clicked.
        If pLabelClicked.Length = 0 Then
            pLabelClicked = LabelCancel
        End If
    End Sub
End Class
