Imports atcUtility

Public Class atcSelectList
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
  Friend WithEvents panelTop As System.Windows.Forms.Panel
  Friend WithEvents panelBottom As System.Windows.Forms.Panel
  Friend WithEvents btnOk As System.Windows.Forms.Button
  Friend WithEvents btnCancel As System.Windows.Forms.Button
  Friend WithEvents btnAttributesNone As System.Windows.Forms.Button
  Friend WithEvents btnAttributesAll As System.Windows.Forms.Button
  Friend WithEvents lstAvailable As System.Windows.Forms.ListBox
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
    Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(atcSelectList))
    Me.panelTop = New System.Windows.Forms.Panel
    Me.btnAttributesNone = New System.Windows.Forms.Button
    Me.btnAttributesAll = New System.Windows.Forms.Button
    Me.lstAvailable = New System.Windows.Forms.ListBox
    Me.panelBottom = New System.Windows.Forms.Panel
    Me.btnCancel = New System.Windows.Forms.Button
    Me.btnOk = New System.Windows.Forms.Button
    Me.panelTop.SuspendLayout()
    Me.panelBottom.SuspendLayout()
    Me.SuspendLayout()
    '
    'panelTop
    '
    Me.panelTop.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.panelTop.Controls.Add(Me.btnAttributesNone)
    Me.panelTop.Controls.Add(Me.btnAttributesAll)
    Me.panelTop.Controls.Add(Me.lstAvailable)
    Me.panelTop.Location = New System.Drawing.Point(0, 0)
    Me.panelTop.Name = "panelTop"
    Me.panelTop.Size = New System.Drawing.Size(192, 328)
    Me.panelTop.TabIndex = 14
    '
    'btnAttributesNone
    '
    Me.btnAttributesNone.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.btnAttributesNone.Location = New System.Drawing.Point(120, 290)
    Me.btnAttributesNone.Name = "btnAttributesNone"
    Me.btnAttributesNone.Size = New System.Drawing.Size(64, 23)
    Me.btnAttributesNone.TabIndex = 13
    Me.btnAttributesNone.Text = "None"
    '
    'btnAttributesAll
    '
    Me.btnAttributesAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
    Me.btnAttributesAll.Location = New System.Drawing.Point(8, 290)
    Me.btnAttributesAll.Name = "btnAttributesAll"
    Me.btnAttributesAll.Size = New System.Drawing.Size(64, 24)
    Me.btnAttributesAll.TabIndex = 12
    Me.btnAttributesAll.Text = "All"
    '
    'lstAvailable
    '
    Me.lstAvailable.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lstAvailable.IntegralHeight = False
    Me.lstAvailable.Location = New System.Drawing.Point(8, 15)
    Me.lstAvailable.Name = "lstAvailable"
    Me.lstAvailable.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple
    Me.lstAvailable.Size = New System.Drawing.Size(176, 267)
    Me.lstAvailable.TabIndex = 11
    '
    'panelBottom
    '
    Me.panelBottom.Controls.Add(Me.btnCancel)
    Me.panelBottom.Controls.Add(Me.btnOk)
    Me.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom
    Me.panelBottom.Location = New System.Drawing.Point(0, 341)
    Me.panelBottom.Name = "panelBottom"
    Me.panelBottom.Size = New System.Drawing.Size(192, 32)
    Me.panelBottom.TabIndex = 15
    '
    'btnCancel
    '
    Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.btnCancel.Location = New System.Drawing.Point(120, 0)
    Me.btnCancel.Name = "btnCancel"
    Me.btnCancel.Size = New System.Drawing.Size(64, 24)
    Me.btnCancel.TabIndex = 1
    Me.btnCancel.Text = "Cancel"
    '
    'btnOk
    '
    Me.btnOk.Location = New System.Drawing.Point(8, 0)
    Me.btnOk.Name = "btnOk"
    Me.btnOk.Size = New System.Drawing.Size(64, 24)
    Me.btnOk.TabIndex = 0
    Me.btnOk.Text = "Ok"
    '
    'atcSelectList
    '
    Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
    Me.ClientSize = New System.Drawing.Size(192, 373)
    Me.Controls.Add(Me.panelTop)
    Me.Controls.Add(Me.panelBottom)
    Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
    Me.Name = "atcSelectList"
    Me.Text = "Select"
    Me.panelTop.ResumeLayout(False)
    Me.panelBottom.ResumeLayout(False)
    Me.ResumeLayout(False)

  End Sub

#End Region

  Private pAvailable As ArrayList
  Private pSelected As ArrayList
  Private pOk As Boolean

  Public Function AskUser(ByVal aAvailable As ArrayList, ByVal aSelected As ArrayList) As Boolean
    If aAvailable Is Nothing Then
      Throw New Exception("Nothing Available in atcSelectList::AskUser")
    ElseIf aAvailable.Count < 1 Then
      Throw New Exception("Zero items Available in atcSelectList::AskUser")
    ElseIf aSelected Is Nothing Then
      Throw New Exception("Selected = Nothing - Must be assigned before calling atcSelectList::AskUser")
    End If
    pAvailable = aAvailable
    pSelected = aSelected
    Clear()
    Me.ShowDialog()

    If pOk Then
      aSelected.Clear()
      For Each lName As String In lstAvailable.SelectedItems
        aSelected.Add(lName)
      Next
    End If
    Return pOk
  End Function

  Private Sub Clear()
    Dim lIndex As Integer = 0
    pOk = False
    lstAvailable.Items.Clear()
    For Each lAttrName As String In pAvailable
      lstAvailable.Items.Add(lAttrName)
      If pSelected.Contains(lAttrName) Then
        lstAvailable.SetSelected(lIndex, True)
        'Checking GetSelected seems to be needed to actually make the above SetSelected work
        LogDbg("atcSelectList: Selected(" & lIndex & ") = " & lstAvailable.GetSelected(lIndex) & " """ & lAttrName & """")
      End If
      lIndex += 1
    Next
    lstAvailable.Refresh()
  End Sub

  Private Sub btnAttributesAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAttributesAll.Click
    For index As Integer = 0 To lstAvailable.Items.Count - 1
      lstAvailable.SetSelected(index, True)
    Next
  End Sub

  Private Sub btnAttributesNone_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAttributesNone.Click
    For index As Integer = 0 To lstAvailable.Items.Count - 1
      lstAvailable.SetSelected(index, False)
    Next
  End Sub

  Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
    pOk = True
    Close()
  End Sub

  Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
    Close()
  End Sub

End Class
