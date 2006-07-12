Imports atcControls
Imports atcUtility

Public Class frmResult
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
    Friend WithEvents lblHeader As System.Windows.Forms.Label
    Friend WithEvents agdResult As atcControls.atcGrid
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmResult))
        Me.lblHeader = New System.Windows.Forms.Label
        Me.agdResult = New atcControls.atcGrid
        Me.SuspendLayout()
        '
        'lblHeader
        '
        Me.lblHeader.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblHeader.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblHeader.Location = New System.Drawing.Point(8, 8)
        Me.lblHeader.Name = "lblHeader"
        Me.lblHeader.Size = New System.Drawing.Size(436, 16)
        Me.lblHeader.TabIndex = 0
        Me.lblHeader.Text = "Label1"
        '
        'agdResult
        '
        Me.agdResult.AllowHorizontalScrolling = True
        Me.agdResult.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.agdResult.CellBackColor = System.Drawing.Color.Empty
        Me.agdResult.LineColor = System.Drawing.Color.Empty
        Me.agdResult.LineWidth = 0.0!
        Me.agdResult.Location = New System.Drawing.Point(8, 32)
        Me.agdResult.Name = "agdResult"
        Me.agdResult.Size = New System.Drawing.Size(432, 224)
        Me.agdResult.Source = Nothing
        Me.agdResult.TabIndex = 2
        '
        'frmResult
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
        Me.ClientSize = New System.Drawing.Size(448, 264)
        Me.Controls.Add(Me.agdResult)
        Me.Controls.Add(Me.lblHeader)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Name = "frmResult"
        Me.Text = "Form1"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Public Sub InitializeResults(ByVal aTitle1 As String, ByVal aTitle2 As String, ByVal aGridSource As atcGridSource)
        Me.Text = aTitle1
        lblHeader.Text = aTitle2
        agdResult.Clear()
        agdResult.Source = aGridSource
        agdResult.SizeAllColumnsToContents()
        agdResult.Refresh()
    End Sub

    Private Sub frmResult_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyValue = Windows.Forms.Keys.F1 Then
            ShowHelp("BASINS Details\Watershed Characterization Reports.html")
        End If
    End Sub
End Class
