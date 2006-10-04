Public Class frmStatus
    Inherits System.Windows.Forms.Form

    Private Const pLastLabel As Integer = 5
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
    Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmStatus))
    Me.Progress = New System.Windows.Forms.ProgressBar
    Me.lblTop = New System.Windows.Forms.Label
    Me.txtLog = New System.Windows.Forms.TextBox
    Me.lblLeft = New System.Windows.Forms.Label
    Me.lblMiddle = New System.Windows.Forms.Label
    Me.lblRight = New System.Windows.Forms.Label
    Me.lblBottom = New System.Windows.Forms.Label
    Me.SuspendLayout()
    '
    'Progress
    '
    Me.Progress.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.Progress.Location = New System.Drawing.Point(8, 72)
    Me.Progress.Name = "Progress"
    Me.Progress.Size = New System.Drawing.Size(424, 24)
    Me.Progress.TabIndex = 0
    '
    'lblTop
    '
    Me.lblTop.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lblTop.BackColor = System.Drawing.Color.Transparent
    Me.lblTop.Location = New System.Drawing.Point(8, 8)
    Me.lblTop.Name = "lblTop"
    Me.lblTop.Size = New System.Drawing.Size(424, 16)
    Me.lblTop.TabIndex = 1
    Me.lblTop.Text = "lblTop"
    '
    'txtLog
    '
    Me.txtLog.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtLog.Location = New System.Drawing.Point(8, 176)
    Me.txtLog.Multiline = True
    Me.txtLog.Name = "txtLog"
    Me.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Both
    Me.txtLog.Size = New System.Drawing.Size(424, 160)
    Me.txtLog.TabIndex = 2
    Me.txtLog.Text = ""
    '
    'lblLeft
    '
    Me.lblLeft.AutoSize = True
    Me.lblLeft.BackColor = System.Drawing.Color.Transparent
    Me.lblLeft.Location = New System.Drawing.Point(8, 40)
    Me.lblLeft.Name = "lblLeft"
    Me.lblLeft.Size = New System.Drawing.Size(35, 16)
    Me.lblLeft.TabIndex = 3
    Me.lblLeft.Text = "lblLeft"
    '
    'lblMiddle
    '
    Me.lblMiddle.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lblMiddle.BackColor = System.Drawing.Color.Transparent
    Me.lblMiddle.Location = New System.Drawing.Point(8, 40)
    Me.lblMiddle.Name = "lblMiddle"
    Me.lblMiddle.Size = New System.Drawing.Size(424, 16)
    Me.lblMiddle.TabIndex = 4
    Me.lblMiddle.Text = "lblMiddle"
    Me.lblMiddle.TextAlign = System.Drawing.ContentAlignment.TopCenter
    '
    'lblRight
    '
    Me.lblRight.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lblRight.AutoSize = True
    Me.lblRight.BackColor = System.Drawing.Color.Transparent
    Me.lblRight.Location = New System.Drawing.Point(392, 40)
    Me.lblRight.Name = "lblRight"
    Me.lblRight.Size = New System.Drawing.Size(42, 16)
    Me.lblRight.TabIndex = 5
    Me.lblRight.Text = "lblRight"
    Me.lblRight.TextAlign = System.Drawing.ContentAlignment.TopRight
    '
    'lblBottom
    '
    Me.lblBottom.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lblBottom.BackColor = System.Drawing.Color.Transparent
    Me.lblBottom.Location = New System.Drawing.Point(8, 112)
    Me.lblBottom.Name = "lblBottom"
    Me.lblBottom.Size = New System.Drawing.Size(424, 16)
    Me.lblBottom.TabIndex = 6
    Me.lblBottom.Text = "lblBottom"
    Me.lblBottom.TextAlign = System.Drawing.ContentAlignment.TopCenter
    '
    'frmStatus
    '
    Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
    Me.ClientSize = New System.Drawing.Size(440, 333)
    Me.Controls.Add(Me.lblBottom)
    Me.Controls.Add(Me.lblRight)
    Me.Controls.Add(Me.lblLeft)
    Me.Controls.Add(Me.txtLog)
    Me.Controls.Add(Me.lblTop)
    Me.Controls.Add(Me.Progress)
    Me.Controls.Add(Me.lblMiddle)
    Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
    Me.Name = "frmStatus"
    Me.Text = "Status"
    Me.TopMost = True
    Me.WindowState = System.Windows.Forms.FormWindowState.Minimized
    Me.ResumeLayout(False)

  End Sub

#End Region

    Public ReadOnly Property LastLabel() As Integer
        Get
            Return pLastLabel
        End Get
    End Property

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
                Case 4 : lblRight.Text = aNewValue
                Case 5 : lblBottom.Text = aNewValue
            End Select
        End Set
    End Property

    Public Sub Clear()
        For iLabel As Integer = 0 To LastLabel
            Label(iLabel) = ""
        Next
        txtLog.Clear()
    End Sub

  'Private Sub frmStatus_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
  '  MsgBox(New StackTrace(True).ToString, , "frmStatus_Closing")
  'End Sub

  'Private Sub frmStatus_VisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.VisibleChanged
  '  If Not Me.Visible Then
  '    MsgBox(New StackTrace(True).ToString, , "frmStatus_VisibleChanged = " & Me.Visible)
  '  End If
  'End Sub

  'Private Sub frmStatus_StyleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.StyleChanged
  '  MsgBox(New StackTrace(True).ToString, , "frmStatus_StyleChanged")
  'End Sub

  'Private Sub frmStatus_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Disposed
  '  MsgBox(New StackTrace(True).ToString, , "frmStatus_Disposed")
  'End Sub

    Private Sub frmStatus_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        If Not Exiting Then
            e.Cancel = True
            Me.Hide()
        End If
    End Sub
End Class
