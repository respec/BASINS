Imports System.Drawing
Imports System.Reflection
Imports System.Windows.Forms

Public Class atcGraphEdit
    Inherits System.Windows.Forms.Form

    Private pEditMe As Object
    Private pTypeName As String

    Event Apply()

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
    Friend WithEvents grpEdit As System.Windows.Forms.GroupBox
    Friend WithEvents Splitter1 As System.Windows.Forms.Splitter
    Friend WithEvents btnApply As System.Windows.Forms.Button
    Friend WithEvents treeProperties As System.Windows.Forms.TreeView
    Friend WithEvents pnlProperty As System.Windows.Forms.Panel
    Friend WithEvents chkProperty As System.Windows.Forms.CheckBox
    Friend WithEvents txtProperty As System.Windows.Forms.TextBox
    Friend WithEvents lblProperty As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(atcGraphEdit))
        Me.treeProperties = New System.Windows.Forms.TreeView
        Me.grpEdit = New System.Windows.Forms.GroupBox
        Me.chkProperty = New System.Windows.Forms.CheckBox
        Me.lblProperty = New System.Windows.Forms.Label
        Me.btnApply = New System.Windows.Forms.Button
        Me.pnlProperty = New System.Windows.Forms.Panel
        Me.txtProperty = New System.Windows.Forms.TextBox
        Me.Splitter1 = New System.Windows.Forms.Splitter
        Me.grpEdit.SuspendLayout()
        Me.SuspendLayout()
        '
        'treeProperties
        '
        Me.treeProperties.Dock = System.Windows.Forms.DockStyle.Left
        Me.treeProperties.Location = New System.Drawing.Point(0, 0)
        Me.treeProperties.Name = "treeProperties"
        Me.treeProperties.Size = New System.Drawing.Size(440, 485)
        Me.treeProperties.TabIndex = 0
        '
        'grpEdit
        '
        Me.grpEdit.Controls.Add(Me.chkProperty)
        Me.grpEdit.Controls.Add(Me.lblProperty)
        Me.grpEdit.Controls.Add(Me.btnApply)
        Me.grpEdit.Controls.Add(Me.pnlProperty)
        Me.grpEdit.Controls.Add(Me.txtProperty)
        Me.grpEdit.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grpEdit.Location = New System.Drawing.Point(448, 0)
        Me.grpEdit.Name = "grpEdit"
        Me.grpEdit.Size = New System.Drawing.Size(248, 485)
        Me.grpEdit.TabIndex = 1
        Me.grpEdit.TabStop = False
        '
        'chkProperty
        '
        Me.chkProperty.Location = New System.Drawing.Point(16, 200)
        Me.chkProperty.Name = "chkProperty"
        Me.chkProperty.Size = New System.Drawing.Size(152, 24)
        Me.chkProperty.TabIndex = 3
        Me.chkProperty.Visible = False
        '
        'lblProperty
        '
        Me.lblProperty.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblProperty.Location = New System.Drawing.Point(16, 32)
        Me.lblProperty.Name = "lblProperty"
        Me.lblProperty.Size = New System.Drawing.Size(216, 160)
        Me.lblProperty.TabIndex = 1
        '
        'btnApply
        '
        Me.btnApply.Location = New System.Drawing.Point(88, 336)
        Me.btnApply.Name = "btnApply"
        Me.btnApply.Size = New System.Drawing.Size(64, 32)
        Me.btnApply.TabIndex = 0
        Me.btnApply.Text = "Apply"
        '
        'pnlProperty
        '
        Me.pnlProperty.BackColor = System.Drawing.SystemColors.Control
        Me.pnlProperty.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlProperty.Location = New System.Drawing.Point(16, 200)
        Me.pnlProperty.Name = "pnlProperty"
        Me.pnlProperty.Size = New System.Drawing.Size(216, 120)
        Me.pnlProperty.TabIndex = 4
        Me.pnlProperty.Visible = False
        '
        'txtProperty
        '
        Me.txtProperty.Location = New System.Drawing.Point(16, 200)
        Me.txtProperty.Multiline = True
        Me.txtProperty.Name = "txtProperty"
        Me.txtProperty.Size = New System.Drawing.Size(216, 120)
        Me.txtProperty.TabIndex = 2
        Me.txtProperty.Visible = False
        '
        'Splitter1
        '
        Me.Splitter1.Location = New System.Drawing.Point(440, 0)
        Me.Splitter1.Name = "Splitter1"
        Me.Splitter1.Size = New System.Drawing.Size(8, 485)
        Me.Splitter1.TabIndex = 2
        Me.Splitter1.TabStop = False
        '
        'atcGraphEdit
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(696, 485)
        Me.Controls.Add(Me.grpEdit)
        Me.Controls.Add(Me.Splitter1)
        Me.Controls.Add(Me.treeProperties)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "atcGraphEdit"
        Me.Text = "Edit Graph"
        Me.grpEdit.ResumeLayout(False)
        Me.grpEdit.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Public Sub Edit(ByVal aEditMe As Object)
        pEditMe = aEditMe
        treeProperties.Nodes.Clear()
        Me.Show()
        AddPropertiesSubtree(treeProperties.Nodes, pEditMe)
    End Sub

    Private Sub AddPropertiesSubtree(ByVal aAddTo As TreeNodeCollection, ByVal aObj As Object)
        Dim lPropInfo As PropertyInfo() = aObj.GetType.GetProperties()
        Dim lValue As Object
        Dim lPropertyTypeString As String
        Dim lSystem As Boolean
        For i As Integer = 0 To lPropInfo.Length - 1
            With lPropInfo(i)
                If .PropertyType.IsPublic() And .CanWrite Then
                    lPropertyTypeString = .PropertyType.ToString
                    If lPropertyTypeString.StartsWith("System.") Then
                        lSystem = True
                        lPropertyTypeString = lPropertyTypeString.Substring(7)
                    Else
                        lSystem = False
                    End If
                    Dim newNode As TreeNode = aAddTo.Add(.Name & " as " & lPropertyTypeString)
                    newNode.Expand()
                    lValue = Nothing
                    Select Case .GetIndexParameters().Length
                        Case 0
                            Try
                                lValue = .GetValue(aObj, Nothing)
                            Catch ex As Exception
                            End Try
                        Case 1
                            newNode.Text &= "()"
                        Case Else
                            newNode.ForeColor = Color.Gray
                    End Select
                    If Not lValue Is Nothing Then
                        If lValue.ToString <> lPropertyTypeString Then
                            newNode.Text &= " = " & lValue.ToString
                        End If
                        If Not lSystem Then
                            AddPropertiesSubtree(newNode.Nodes, lValue)
                        End If
                    End If
                End If
            End With
        Next
    End Sub

    Private Sub treeProperties_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles treeProperties.AfterSelect
        Dim lNodeText As String = treeProperties.SelectedNode.FullPath
        Dim iAs As Integer = lNodeText.LastIndexOf(" as ")

        If iAs < 0 Then

        Else
            Dim iEquals As Integer = lNodeText.LastIndexOf(" = ")
            Dim lValue As String
            If iEquals < 0 Then
                pTypeName = lNodeText.Substring(iAs + 4)
                lValue = ""
            Else
                pTypeName = lNodeText.Substring(iAs + 4, iEquals - iAs - 4)
                lValue = lNodeText.Substring(iEquals + 3)
            End If

            Dim iBackslash As Integer = lNodeText.LastIndexOf("\")
            lblProperty.Text = lNodeText.Substring(iBackslash + 1, iAs - iBackslash)

            chkProperty.Visible = False
            pnlProperty.Visible = False
            txtProperty.Visible = False
            Select Case pTypeName
                Case "Boolean"
                    chkProperty.Visible = True
                    chkProperty.Checked = CBool(lValue)
                Case "Drawing.Color"
                    pnlProperty.Visible = True
                    pnlProperty.BackColor = Color.FromName(lValue.Substring(7, lValue.Length - 8))
                Case ""
                    'if no type name, don't display edit control
                Case Else
                    txtProperty.Visible = True
                    txtProperty.Text = lValue
            End Select
        End If
    End Sub

    Private Sub pnlProperty_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pnlProperty.Click
        Dim cDlg As New ColorDialog
        cDlg.Color = pnlProperty.BackColor
        cDlg.AnyColor = True
        cDlg.FullOpen = True
        If (cDlg.ShowDialog() = Windows.Forms.DialogResult.OK) Then
            pnlProperty.BackColor = cDlg.Color
        End If
    End Sub

    Private Sub chkProperty_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkProperty.CheckedChanged
        chkProperty.Text = chkProperty.Checked
    End Sub

    Private Sub btnApply_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnApply.Click
        Select Case pTypeName
            Case "Boolean"
                SetValueOfNode(treeProperties.SelectedNode, chkProperty.Checked)
            Case "Drawing.Color"
                SetValueOfNode(treeProperties.SelectedNode, pnlProperty.BackColor)
            Case "String"
                SetValueOfNode(treeProperties.SelectedNode, txtProperty.Text)
            Case "Single"
                SetValueOfNode(treeProperties.SelectedNode, CSng(txtProperty.Text))
            Case "Double"
                SetValueOfNode(treeProperties.SelectedNode, CDbl(txtProperty.Text))
        End Select
        RaiseEvent Apply()
    End Sub

    Private Sub SetValueOfNode(ByVal aNode As TreeNode, ByVal aValue As Object)
        Dim lObj As Object = GetObjectForNode(aNode.Parent)
        GetPropInfoForNode(aNode, lObj).SetValue(lObj, aValue, Nothing)
        aNode.Text = aNode.Text.Substring(0, aNode.Text.IndexOf(" = ") + 3) & aValue.ToString
    End Sub

    Private Function GetObjectForNode(ByVal aNode As TreeNode) As Object
        If aNode Is Nothing Then 'at top of tree
            Return pEditMe
        Else
            Dim lObj As Object = GetObjectForNode(aNode.Parent)
            Return GetPropInfoForNode(aNode, lObj).GetValue(lObj, Nothing)
        End If
    End Function

    Private Function GetPropInfoForNode(ByVal aNode As TreeNode, ByVal aParentObj As Object) As PropertyInfo
        Dim lPropInfo As PropertyInfo()
        Dim iAs As Integer = aNode.Text.LastIndexOf(" as ")
        Dim lPropName As String = aNode.Text.Substring(0, iAs)

        lPropInfo = aParentObj.GetType.GetProperties()
        For i As Integer = 0 To lPropInfo.GetUpperBound(0)
            If lPropInfo(i).Name = lPropName Then
                Return lPropInfo(i)
            End If
        Next
        Return Nothing
    End Function

End Class
