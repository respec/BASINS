Public Class frmStatus
    Inherits System.Windows.Forms.Form

    Private pLevels As New Generic.List(Of ctlStatus)

    Delegate Sub SetLevelCallback(ByVal aNewLevel As Integer)
    Private pSetLevelCallback As New SetLevelCallback(AddressOf SetLevel)

    Private pMargin As Integer = 10
    Private pDetailsVisible As Boolean = False

    Friend Shared ReadOnly LastLabel As Integer = 4 'bottom label has index 5

    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnPause As System.Windows.Forms.Button
    Friend WithEvents btnLog As System.Windows.Forms.Button
    Friend WithEvents lblBottom As System.Windows.Forms.Label
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
    Friend WithEvents txtDetails As System.Windows.Forms.TextBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmStatus))
        Me.txtDetails = New System.Windows.Forms.TextBox
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnPause = New System.Windows.Forms.Button
        Me.btnLog = New System.Windows.Forms.Button
        Me.lblBottom = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'txtDetails
        '
        Me.txtDetails.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDetails.Location = New System.Drawing.Point(14, 62)
        Me.txtDetails.Multiline = True
        Me.txtDetails.Name = "txtDetails"
        Me.txtDetails.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtDetails.Size = New System.Drawing.Size(485, 68)
        Me.txtDetails.TabIndex = 2
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.AutoSize = True
        Me.btnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancel.Location = New System.Drawing.Point(14, 7)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(81, 31)
        Me.btnCancel.TabIndex = 7
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnPause
        '
        Me.btnPause.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnPause.AutoSize = True
        Me.btnPause.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPause.Location = New System.Drawing.Point(100, 7)
        Me.btnPause.Name = "btnPause"
        Me.btnPause.Size = New System.Drawing.Size(78, 31)
        Me.btnPause.TabIndex = 8
        Me.btnPause.Text = "Pause"
        Me.btnPause.UseVisualStyleBackColor = True
        '
        'btnLog
        '
        Me.btnLog.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnLog.AutoSize = True
        Me.btnLog.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLog.Location = New System.Drawing.Point(185, 7)
        Me.btnLog.Name = "btnLog"
        Me.btnLog.Size = New System.Drawing.Size(78, 31)
        Me.btnLog.TabIndex = 9
        Me.btnLog.Text = "Log"
        Me.btnLog.UseVisualStyleBackColor = True
        '
        'lblBottom
        '
        Me.lblBottom.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblBottom.AutoSize = True
        Me.lblBottom.BackColor = System.Drawing.Color.Transparent
        Me.lblBottom.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBottom.Location = New System.Drawing.Point(270, 16)
        Me.lblBottom.Name = "lblBottom"
        Me.lblBottom.Size = New System.Drawing.Size(66, 17)
        Me.lblBottom.TabIndex = 13
        Me.lblBottom.Text = "lblBottom"
        Me.lblBottom.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'frmStatus
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
        Me.ClientSize = New System.Drawing.Size(514, 52)
        Me.Controls.Add(Me.lblBottom)
        Me.Controls.Add(Me.btnLog)
        Me.Controls.Add(Me.btnPause)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.txtDetails)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmStatus"
        Me.Text = "Status "
        Me.TopMost = True
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Public Property Level() As Integer
        Get
            Return pLevels.Count
        End Get
        Set(ByVal aNewLevel As Integer)
            If aNewLevel > 0 AndAlso aNewLevel < 10 AndAlso aNewLevel <> pLevels.Count Then
                If Me.InvokeRequired Then
                    Me.Invoke(pSetLevelCallback, aNewLevel)
                Else
                    SetLevel(aNewLevel)
                End If
            End If
        End Set
    End Property

    Private Sub SetLevel(ByVal aNewLevel As Integer)
        Dim lHeightChange As Integer = 0
        If aNewLevel < pLevels.Count Then
            lHeightChange = -(pLevels(0).Height + pMargin) * (pLevels.Count - aNewLevel)
            txtDetails.Top += lHeightChange
            For lRemoveIndex As Integer = pLevels.Count - 1 To aNewLevel Step -1
                Dim lRemoveControl As ctlStatus = pLevels(lRemoveIndex)
                Me.Controls.Remove(lRemoveControl)
                pLevels.RemoveAt(lRemoveIndex)
                lRemoveControl.Dispose()
            Next
        Else
            While aNewLevel > pLevels.Count
                Dim newLevel As New ctlStatus
                pLevels.Add(newLevel)
                Me.Controls.Add(newLevel)
                With newLevel
                    .Height = pLevels(0).Height
                    .Top = pMargin + (pLevels.Count - 1) * (.Height + pMargin)
                    .Left = pMargin
                    .Width = Me.ClientSize.Width - (pMargin * 2)
                    .Anchor = AnchorStyles.Top + AnchorStyles.Left + AnchorStyles.Right
                    lHeightChange += .Height + pMargin
                    txtDetails.Top += lHeightChange
                End With
            End While
        End If
        If lHeightChange <> 0 Then Me.Height += lHeightChange
    End Sub

    Public Property Label(ByVal aIndex As Integer, Optional ByVal aLevel As Integer = 0) As String
        Get
            Select Case aIndex
                Case 0 : Return Me.Text
                Case 5 : Return lblBottom.Text
                Case Else
                    If aIndex < 1 OrElse aIndex > LastLabel Then
                        Return ""
                    Else
                        If aLevel < 1 Then aLevel = pLevels.Count
                        If aLevel > 0 AndAlso aLevel <= pLevels.Count Then
                            Return pLevels(aLevel - 1).Label(aIndex)
                        Else
                            Return ""
                        End If
                    End If
            End Select
        End Get
        Set(ByVal aNewValue As String)
            Select Case aIndex
                Case 0 : Me.Text = aNewValue
                Case 5 : lblBottom.Text = aNewValue
                Case Else
                    If aLevel < 1 Then aLevel = pLevels.Count
                    If aLevel > 0 AndAlso aLevel <= pLevels.Count Then
                        pLevels(aLevel - 1).Label(aIndex) = aNewValue
                    End If
            End Select
        End Set
    End Property

    Public ReadOnly Property Progress(Optional ByVal aLevel As Integer = 0) As Windows.Forms.ProgressBar
        Get
            If aLevel < 1 Then aLevel = pLevels.Count
            Return pLevels(aLevel - 1).Progress
        End Get
    End Property

    Public Property LogVisible() As Boolean
        Get
            Return pDetailsVisible
        End Get
        Set(ByVal aNewValue As Boolean)
            If aNewValue <> pDetailsVisible Then
                pDetailsVisible = aNewValue
                txtDetails.Visible = pDetailsVisible
                If pDetailsVisible Then
                    ShowLog()
                Else
                    txtDetails.Visible = False

                    Dim lDetailsTop As Integer = pMargin
                    If pLevels.Count > 0 Then
                        Dim lLastLevel As ctlStatus = pLevels(pLevels.Count - 1)
                        lDetailsTop = lLastLevel.Top + lLastLevel.Height + pMargin
                    End If

                    Me.Height -= (btnLog.Top - lDetailsTop)
                End If
            End If
        End Set
    End Property

    Public Sub Clear()
        For lLabelIndex As Integer = 1 To LastLabel
            Label(lLabelIndex) = ""
        Next
        txtDetails.Clear()
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
        Label(1) = "Canceling..."
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

    Private Sub btnLog_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLog.Click
        LogVisible = Not LogVisible
    End Sub

    Private Sub frmStatus_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        Try
            If LogVisible Then
                ShowLog()
            Else
                txtDetails.Visible = False
            End If
        Catch
        End Try
    End Sub

    Private Sub ShowLog()
        If Me.WindowState = FormWindowState.Normal Then
            Dim lDetailsTop As Integer = pMargin
            If pLevels.Count > 0 Then
                Dim lLastLevel As ctlStatus = pLevels(pLevels.Count - 1)
                lDetailsTop = lLastLevel.Top + lLastLevel.Height + pMargin
            End If
            Dim lDetailsHeight As Integer = btnLog.Top - pMargin - lDetailsTop
            If lDetailsHeight < pMargin * 8 Then
                Me.Height += pMargin * 10 - lDetailsHeight
            ElseIf ClientRectangle.Width < pMargin * 10 Then
                Me.Width += pMargin * 10
            Else
                txtDetails.Top = lDetailsTop
                txtDetails.Height = lDetailsHeight
                txtDetails.Width = Me.ClientRectangle.Width - pMargin * 2
                txtDetails.Visible = True
            End If
        End If
    End Sub
End Class
