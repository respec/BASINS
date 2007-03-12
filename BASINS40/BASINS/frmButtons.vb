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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmButtons))
        Me.SuspendLayout()
        '
        'frmButtons
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(292, 273)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmButtons"
        Me.Text = "frmButtons"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private pLabelClicked As String

    Public Function AskUser(ByVal aTitle As String, ByVal aLabels As IEnumerable) As String
        Dim lTop As Integer = 0
        Text = aTitle
        For Each curLabel As String In aLabels

            Dim btn As Windows.Forms.Button = New Windows.Forms.Button
            btn.Text = curLabel
            btn.Top = lTop
            btn.Left = 0
            btn.Width = Me.ClientSize.Width
            btn.Anchor = Windows.Forms.AnchorStyles.Left Or Windows.Forms.AnchorStyles.Right Or Windows.Forms.AnchorStyles.Top
            lTop += btn.Height

            Me.Controls.Add(btn)
            AddHandler btn.Click, AddressOf btnClick

        Next
        Me.Height = lTop + Me.Height - Me.ClientSize.Height
        pLabelClicked = "Cancel" 'If form closes without user clicking a button, default to "Cancel"
        Me.ShowDialog() 'Block until form closes
        Return pLabelClicked
    End Function

    Private Sub btnClick(ByVal sender As Object, ByVal e As System.EventArgs)
        pLabelClicked = sender.text
        Me.Hide()
    End Sub

End Class
