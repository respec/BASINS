Public Class frmSelectDisplay
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
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmSelectDisplay))
        '
        'frmSelectDisplay
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(168, 273)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmSelectDisplay"
        Me.Text = "Display Data"

    End Sub

#End Region

    Private Const pPADDING As Integer = 5
    Private pArgButton() As Windows.Forms.Button
    Private pDataManager As atcDataManager
    Private pDataGroup As atcDataGroup

    Public Sub AskUser(ByVal aDataManager As atcDataManager, ByVal aDataGroup As atcDataGroup)
        pDataManager = aDataManager
        pDataGroup = aDataGroup
        Dim lButtonWidth As Integer = Me.ClientRectangle.Width - pPADDING * 2
        Dim iArg As Integer = 0

        Dim DisplayPlugins As ICollection = pDataManager.GetPlugins(GetType(atcDataDisplay))
        For Each lDisp As atcDataDisplay In DisplayPlugins

            ReDim Preserve pArgButton(iArg)
            pArgButton(iArg) = New Windows.Forms.Button
            With pArgButton(iArg)
                If iArg = 0 Then
                    .Top = pPADDING
                Else
                    .Top = pArgButton(iArg - 1).Top + pArgButton(iArg - 1).Height + pPADDING
                End If
                .Width = lButtonWidth
                .Left = pPADDING

                .Tag = lDisp.Name
                Dim iColon As Integer = lDisp.Name.IndexOf("::")
                If iColon > 0 Then
                    .Text = lDisp.Name.Substring(iColon + 2)
                Else
                    .Text = lDisp.Name
                End If
                .Anchor = Windows.Forms.AnchorStyles.Right Or Windows.Forms.AnchorStyles.Left Or Windows.Forms.AnchorStyles.Top

                AddHandler pArgButton(iArg).Click, AddressOf ArgButton_Click
            End With
            Controls.Add(pArgButton(iArg))
            iArg += 1
        Next
        iArg -= 1
        If iArg >= 0 Then
            Me.Height = pArgButton(iArg).Top + pArgButton(iArg).Height + pPADDING + (Me.Height - Me.ClientRectangle.Height)
            Me.ShowDialog()
        Else
            Me.Close()
        End If
    End Sub

    Private Sub ArgButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim newDisplay As atcDataDisplay
        Dim DisplayPlugins As ICollection = pDataManager.GetPlugins(GetType(atcDataDisplay))
        For Each atf As atcDataDisplay In DisplayPlugins
            If atf.Name = sender.Tag Then
                Dim typ As System.Type = atf.GetType()
                Dim asm As System.Reflection.Assembly = System.Reflection.Assembly.GetAssembly(typ)
                newDisplay = asm.CreateInstance(typ.FullName)
                newDisplay.Show(pDataManager, pDataGroup)
                Me.Close()
                Exit Sub
            End If
        Next
    End Sub

End Class
