Imports atcUtility

Public Class frmScript
  Inherits System.Windows.Forms.Form

  Private pBasinsPlugin As PlugIn
  Private pDefaultScript = "Imports Microsoft.VisualBasic" & vbCrLf _
                       & "Public Class Sample" & vbCrLf _
                       & "  Public Shared Function ScriptMain(ByVal Arg As String) As String" & vbCrLf _
                       & "    MsgBox(""Hello!"", MsgBoxStyle.Exclamation, ""Message Title"")" & vbCrLf _
                       & "    Return Arg.ToUpper()" & vbCrLf _
                       & "  End Function" & vbCrLf _
                       & "End Class"
  Private pFilter As String = "VB.net (*.vb)|*.vb|All files|*.*"
  Private pFileName As String = ""

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
  Friend WithEvents tools As System.Windows.Forms.ToolBar
  Friend WithEvents tbbRun As System.Windows.Forms.ToolBarButton
  Friend WithEvents tbbsep As System.Windows.Forms.ToolBarButton
  Friend WithEvents tbbSave As System.Windows.Forms.ToolBarButton
  Friend WithEvents txtScript As System.Windows.Forms.RichTextBox
  Friend WithEvents tbbOpen As System.Windows.Forms.ToolBarButton
  Friend WithEvents tbbNew As System.Windows.Forms.ToolBarButton
  Friend WithEvents tbbCompile As System.Windows.Forms.ToolBarButton
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
    Me.txtScript = New System.Windows.Forms.RichTextBox
    Me.tools = New System.Windows.Forms.ToolBar
    Me.tbbNew = New System.Windows.Forms.ToolBarButton
    Me.tbbOpen = New System.Windows.Forms.ToolBarButton
    Me.tbbSave = New System.Windows.Forms.ToolBarButton
    Me.tbbsep = New System.Windows.Forms.ToolBarButton
    Me.tbbRun = New System.Windows.Forms.ToolBarButton
    Me.tbbCompile = New System.Windows.Forms.ToolBarButton
    Me.SuspendLayout()
    '
    'txtScript
    '
    Me.txtScript.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtScript.Font = New System.Drawing.Font("Courier New", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.txtScript.Location = New System.Drawing.Point(8, 48)
    Me.txtScript.Name = "txtScript"
    Me.txtScript.Size = New System.Drawing.Size(400, 280)
    Me.txtScript.TabIndex = 0
    Me.txtScript.Text = "RichTextBox1"
    '
    'tools
    '
    Me.tools.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.tools.Buttons.AddRange(New System.Windows.Forms.ToolBarButton() {Me.tbbNew, Me.tbbOpen, Me.tbbSave, Me.tbbsep, Me.tbbRun, Me.tbbCompile})
    Me.tools.Dock = System.Windows.Forms.DockStyle.None
    Me.tools.DropDownArrows = True
    Me.tools.Location = New System.Drawing.Point(0, 0)
    Me.tools.Name = "tools"
    Me.tools.ShowToolTips = True
    Me.tools.Size = New System.Drawing.Size(412, 42)
    Me.tools.TabIndex = 1
    '
    'tbbNew
    '
    Me.tbbNew.Text = "New"
    '
    'tbbOpen
    '
    Me.tbbOpen.Text = "Open"
    '
    'tbbSave
    '
    Me.tbbSave.Text = "Save"
    '
    'tbbsep
    '
    Me.tbbsep.Style = System.Windows.Forms.ToolBarButtonStyle.Separator
    '
    'tbbRun
    '
    Me.tbbRun.Text = "Run"
    '
    'tbbCompile
    '
    Me.tbbCompile.Text = "Plugin"
    Me.tbbCompile.ToolTipText = "Compile as a plugin"
    '
    'frmScript
    '
    Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
    Me.ClientSize = New System.Drawing.Size(416, 333)
    Me.Controls.Add(Me.tools)
    Me.Controls.Add(Me.txtScript)
    Me.Name = "frmScript"
    Me.Text = "frmScript"
    Me.ResumeLayout(False)

  End Sub

#End Region

  Private Sub tools_ButtonClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles tools.ButtonClick
    Select Case e.Button.Text
      Case "New"
        txtScript.Text = pDefaultScript
      Case "Open"
        Dim cdOpen As New Windows.Forms.OpenFileDialog
        cdOpen.Filter = pFilter
        cdOpen.FileName = pFileName
        If cdOpen.ShowDialog() = Windows.Forms.DialogResult.OK Then
          pFileName = cdOpen.FileName
          txtScript.Text = WholeFileString(pFileName)
        End If

      Case "Save"
        Dim cdSave As New Windows.Forms.SaveFileDialog
        cdSave.Filter = pFilter
        cdSave.FileName = pFileName
        If cdSave.ShowDialog() = Windows.Forms.DialogResult.OK Then
          pFileName = cdSave.FileName
          SaveFileString(pFileName, txtScript.Text)
        End If

      Case "Run"
        Dim args(0) As Object
        args(0) = "Test Argument"
        pBasinsPlugin.RunBasinsScript(txtScript.Text, args)

      Case "Plugin"
        Dim cdSave As New Windows.Forms.SaveFileDialog
        Dim errors As String = ""
        cdSave.Filter = "DLL files (*.dll)|*.dll"
        cdSave.FileName = g_MapWin.Plugins.PluginFolder _
                        & System.IO.Path.GetFileNameWithoutExtension(pFileName) & ".dll"
        If cdSave.ShowDialog() = Windows.Forms.DialogResult.OK Then
          CompileScript(txtScript.Text, _
                        errors, _
                        Split("System.dll,Microsoft.VisualBasic.dll,atcData.dll,atcUtility.dll,MapWinInterfaces.dll", ","), _
                        cdSave.FileName)
          If errors.Length = 0 Then
            If Not g_MapWin.Plugins.AddFromFile(cdSave.FileName) Then
              MsgBox("Could not add plugin" & vbCr & cdsave.FileName, , "Plugins.AddFromFile")
            Else
              If Not g_MapWin.Plugins.StartPlugin(System.IO.Path.GetFileNameWithoutExtension(cdsave.FileName)) Then
                g_MapWin.Plugins.ShowPluginDialog()
                'MsgBox("Could not start plugin" & vbCr & cdsave.FileName, , "Plugins.StartPlugin")
              End If
            End If
          Else
              MsgBox(errors, , "Compile errors")
            End If
          End If
    End Select
  End Sub

  Public WriteOnly Property BasinsPlugin() As PlugIn
    Set(ByVal newValue As PlugIn)
      pBasinsPlugin = newValue
    End Set
  End Property

  Private Sub frmScript_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
    txtScript.Text = ""
  End Sub
End Class
