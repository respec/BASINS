Imports atcUtility

Public Class frmCliGenParmList
    Inherits System.Windows.Forms.Form
    Dim pOk As Boolean

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
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents chklstParms As System.Windows.Forms.CheckedListBox
    Friend WithEvents btnSelNone As System.Windows.Forms.Button
    Friend WithEvents btnSelAll As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCliGenParmList))
        Me.chklstParms = New System.Windows.Forms.CheckedListBox
        Me.btnOK = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnSelNone = New System.Windows.Forms.Button
        Me.btnSelAll = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'chklstParms
        '
        Me.chklstParms.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chklstParms.IntegralHeight = False
        Me.chklstParms.Location = New System.Drawing.Point(8, 8)
        Me.chklstParms.Name = "chklstParms"
        Me.chklstParms.Size = New System.Drawing.Size(300, 253)
        Me.chklstParms.TabIndex = 0
        '
        'btnOK
        '
        Me.btnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnOK.Location = New System.Drawing.Point(12, 297)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(64, 24)
        Me.btnOK.TabIndex = 3
        Me.btnOK.Text = "OK"
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(240, 297)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(64, 24)
        Me.btnCancel.TabIndex = 4
        Me.btnCancel.Text = "Cancel"
        '
        'btnSelNone
        '
        Me.btnSelNone.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSelNone.Location = New System.Drawing.Point(240, 267)
        Me.btnSelNone.Name = "btnSelNone"
        Me.btnSelNone.Size = New System.Drawing.Size(64, 24)
        Me.btnSelNone.TabIndex = 2
        Me.btnSelNone.Text = "None"
        '
        'btnSelAll
        '
        Me.btnSelAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnSelAll.Location = New System.Drawing.Point(12, 267)
        Me.btnSelAll.Name = "btnSelAll"
        Me.btnSelAll.Size = New System.Drawing.Size(64, 24)
        Me.btnSelAll.TabIndex = 1
        Me.btnSelAll.Text = "All"
        '
        'frmCliGenParmList
        '
        Me.AcceptButton = Me.btnOK
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(316, 333)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.btnSelAll)
        Me.Controls.Add(Me.btnSelNone)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.chklstParms)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Name = "frmCliGenParmList"
        Me.Text = "Select CliGen Parameters to Edit"
        Me.ResumeLayout(False)

    End Sub

#End Region
    Public Function AskUser(ByRef aParms As atcCollection, ByVal aParmDescs As atcCollection) As Boolean
        Dim i As Integer
        Dim lStr As String
        For i = 0 To aParms.Count - 1
            If aParmDescs.Count = aParms.Count Then
                lStr = aParmDescs.ItemByIndex(i)
            Else
                lStr = aParms.Keys.Item(i)
            End If
            If aParms.ItemByIndex(i) Then
                chklstParms.Items.Add(lStr, True)
            Else
                chklstParms.Items.Add(lStr, False)
            End If
        Next
        Me.ShowDialog()
        If pOk Then
            With chklstParms
                For i = 0 To .Items.Count - 1
                    If .GetItemChecked(i) <> aParms.ItemByIndex(i) Then 'turned parm on/off
                        lStr = aParms.Keys.Item(i)
                        aParms.RemoveAt(i)
                        aParms.Insert(i, lStr, .GetItemChecked(i))
                    End If
                Next
            End With
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Close()
    End Sub

    Private Sub btnOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOK.Click
        pOk = True
        Close()
    End Sub

    Private Sub btnSelNone_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelNone.Click
        With chklstParms
            For i As Integer = 0 To .Items.Count - 1
                .SetItemChecked(i, False)
            Next
        End With
    End Sub

    Private Sub btnSelAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelAll.Click
        With chklstParms
            For i As Integer = 0 To .Items.Count - 1
                .SetItemChecked(i, True)
            Next
        End With
    End Sub

    Private Sub frmCliGenParmList_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyValue = Windows.Forms.Keys.F1 Then
            ShowHelp("BASINS Details\Compute\Cligen.html")
        End If
    End Sub
End Class
