Imports atcUtility
Imports MapWinUtility

Public Class frmBuildNew
  Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

  Public Sub New()
    MyBase.New()

    'This call is required by the Windows Form Designer.
    InitializeComponent()

    'Add any initialization after the InitializeComponent() call
    txtInstructions.Text = "To Build a New BASINS Project, " & _
       "zoom/pan to your geographic area of interest, select (highlight) it, " & _
       "and then click 'Build'.  " & _
       "If your area is outside the US, then click 'Build' " & _
       "with no features selected to create an international project."

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
  Friend WithEvents cmdBuild As System.Windows.Forms.Button
  Friend WithEvents txtInstructions As System.Windows.Forms.TextBox
  Friend WithEvents txtSelected As System.Windows.Forms.TextBox
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
    Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmBuildNew))
    Me.cmdBuild = New System.Windows.Forms.Button
    Me.txtInstructions = New System.Windows.Forms.TextBox
    Me.txtSelected = New System.Windows.Forms.TextBox
    Me.SuspendLayout()
    '
    'cmdBuild
    '
    Me.cmdBuild.Anchor = System.Windows.Forms.AnchorStyles.Bottom
    Me.cmdBuild.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.cmdBuild.Location = New System.Drawing.Point(156, 192)
    Me.cmdBuild.Name = "cmdBuild"
    Me.cmdBuild.Size = New System.Drawing.Size(96, 32)
    Me.cmdBuild.TabIndex = 1
    Me.cmdBuild.Text = "Build"
    '
    'txtInstructions
    '
    Me.txtInstructions.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtInstructions.BackColor = System.Drawing.SystemColors.InactiveCaptionText
    Me.txtInstructions.BorderStyle = System.Windows.Forms.BorderStyle.None
    Me.txtInstructions.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.txtInstructions.Location = New System.Drawing.Point(16, 16)
    Me.txtInstructions.Multiline = True
    Me.txtInstructions.Name = "txtInstructions"
    Me.txtInstructions.Size = New System.Drawing.Size(392, 96)
    Me.txtInstructions.TabIndex = 2
    Me.txtInstructions.Text = ""
    '
    'txtSelected
    '
    Me.txtSelected.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtSelected.BackColor = System.Drawing.SystemColors.Menu
    Me.txtSelected.Location = New System.Drawing.Point(16, 112)
    Me.txtSelected.Multiline = True
    Me.txtSelected.Name = "txtSelected"
    Me.txtSelected.Size = New System.Drawing.Size(392, 65)
    Me.txtSelected.TabIndex = 3
    Me.txtSelected.Text = "Selected Features:"
    '
    'frmBuildNew
    '
    Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
    Me.ClientSize = New System.Drawing.Size(423, 239)
    Me.Controls.Add(Me.txtSelected)
    Me.Controls.Add(Me.txtInstructions)
    Me.Controls.Add(Me.cmdBuild)
    Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
    Me.KeyPreview = True
    Me.Name = "frmBuildNew"
    Me.Text = "Build New BASINS 4 Project"
    Me.TopMost = True
    Me.ResumeLayout(False)

  End Sub

#End Region

  Private Sub cmdBuild_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBuild.Click
    SaveSetting("BASINS4", "Window Positions", "BuildTop", Me.Top)
    SaveSetting("BASINS4", "Window Positions", "BuildLeft", Me.Left)
    Me.Close()
    SpecifyAndCreateNewProject()
  End Sub

  Private Sub frmBuildNew_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
    If e.KeyValue = Windows.Forms.Keys.F1 Then
      ShowHelp("BASINS Details\Welcome to BASINS 4 Window\Build BASINS Project.html")
    End If
  End Sub
End Class
