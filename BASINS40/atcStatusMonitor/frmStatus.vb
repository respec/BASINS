Public Class frmStatus
    Inherits System.Windows.Forms.Form

    Private pLevels As New Generic.List(Of ctlStatus)
    Private pMargin As Integer = 10

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

        Me.Level = 1
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
    Friend WithEvents txtLog As System.Windows.Forms.TextBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmStatus))
        Me.txtLog = New System.Windows.Forms.TextBox
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnPause = New System.Windows.Forms.Button
        Me.btnDetails = New System.Windows.Forms.Button
        Me.SuspendLayout()
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
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancel.Location = New System.Drawing.Point(12, 14)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(65, 26)
        Me.btnCancel.TabIndex = 7
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnPause
        '
        Me.btnPause.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnPause.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPause.Location = New System.Drawing.Point(83, 14)
        Me.btnPause.Name = "btnPause"
        Me.btnPause.Size = New System.Drawing.Size(65, 26)
        Me.btnPause.TabIndex = 8
        Me.btnPause.Text = "Pause"
        Me.btnPause.UseVisualStyleBackColor = True
        '
        'btnDetails
        '
        Me.btnDetails.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnDetails.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDetails.Location = New System.Drawing.Point(154, 14)
        Me.btnDetails.Name = "btnDetails"
        Me.btnDetails.Size = New System.Drawing.Size(65, 26)
        Me.btnDetails.TabIndex = 9
        Me.btnDetails.Text = "Details"
        Me.btnDetails.UseVisualStyleBackColor = True
        '
        'frmStatus
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(514, 52)
        Me.Controls.Add(Me.btnDetails)
        Me.Controls.Add(Me.btnPause)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.txtLog)
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

    Public Property Level() As Integer
        Get
            Return pLevels.Count
        End Get
        Set(ByVal value As Integer)
            If value > 0 AndAlso value < 10 Then
                While value > pLevels.Count
                    Dim newLevel As New ctlStatus
                    pLevels.Add(newLevel)
                    Me.Controls.Add(newLevel)
                    With newLevel
                        .Top = pMargin + (pLevels.Count - 1) * (.Height + pMargin)
                        .Left = pMargin
                        .Width = Me.Width - pMargin * 2
                        .Anchor = AnchorStyles.Top + AnchorStyles.Left + AnchorStyles.Right
                        Me.Height += .Height + pMargin
                    End With
                End While
                If Level < pLevels.Count Then
                    pLevels.RemoveRange(Level, pLevels.Count - Level)
                    Me.Height -= (pLevels(0).Height + pMargin) * (pLevels.Count - Level)
                End If
            End If
        End Set
    End Property

    Public Property Label(ByVal aIndex As Integer) As String
        Get
            If pLevels.Count < 1 Then
                Return ""
            Else
                Return pLevels(pLevels.Count - 1).Label(aIndex)
            End If
        End Get
        Set(ByVal aNewValue As String)
            If pLevels.Count > 0 Then pLevels(pLevels.Count - 1).Label(aIndex) = aNewValue
        End Set
    End Property

    Public ReadOnly Property Progress() As Windows.Forms.ProgressBar
        Get
            If pLevels.Count < 1 Then
                Return Nothing
            Else
                Return pLevels(pLevels.Count - 1).Progress
            End If
        End Get
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
            If Me.Height > txtLog.Top + txtLog.Left * 3 Then
                txtLog.Width = Me.ClientRectangle.Width - pMargin * 2
                txtLog.Height = Me.ClientRectangle.Height - txtLog.Top - txtLog.Left
                txtLog.Visible = True
            Else
                txtLog.Visible = False
            End If
        Catch
        End Try
    End Sub
End Class
