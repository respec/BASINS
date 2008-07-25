Imports MapWinUtility

Public Class frmEdit
    Private WithEvents pEditControl As ctlEdit
    Private pParent As Windows.Forms.Form
    Private pAddRemoveFlag As Boolean = True
    Private pEditFlag As Boolean = True

    Friend Property EditControl() As Windows.Forms.Control
        Get
            Return pEditControl
        End Get
        Set(ByVal aControl As Windows.Forms.Control)
            'ensure minimum width and height to be able to see buttons
            If aControl.Width < 640 Then
                aControl.Width = 640
            End If
            Width = aControl.Width
            Height = aControl.Height + 80
            panelEdit.Controls.Add(aControl)
            panelEdit.Width = aControl.Width
            panelEdit.Height = aControl.Height
            aControl.Dock = Windows.Forms.DockStyle.Fill
            pEditControl = aControl
        End Set
    End Property

    Friend Property AddRemoveFlag() As Boolean
        Get
            Return pAddRemoveFlag
        End Get
        Set(ByVal aAddRemoveFlag As Boolean)
            pAddRemoveFlag = aAddRemoveFlag
            If pAddRemoveFlag = False Then
                pEditFlag = False
            End If
        End Set
    End Property

    Friend Property EditFlag() As Boolean
        Get
            Return pEditFlag
        End Get
        Set(ByVal aEditFlag As Boolean)
            pEditFlag = aEditFlag
        End Set
    End Property


    Private Sub cmdOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdOk.Click
        pEditControl.Save()
        Me.Dispose()
    End Sub

    Private Sub cmdApply_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdApply.Click
        pEditControl.Save()
    End Sub

    Private Sub cmdCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        If pEditControl.Changed Then
            If Logger.Msg("Changes have been made.  Discard them?", _
                        Microsoft.VisualBasic.MsgBoxStyle.YesNo, _
                        "Discard Changes") = Microsoft.VisualBasic.MsgBoxResult.Yes Then
                Me.Dispose()
            End If
        Else
            Me.Dispose()
        End If
    End Sub

    Private Sub cmdAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        pEditControl.Add()
    End Sub

    Public Sub New(ByVal aParent As Windows.Forms.Form)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        pParent = aParent
    End Sub

    Private Sub pEditControl_Change(ByVal aChange As Boolean) Handles pEditControl.Change
        Me.Text = pEditControl.Caption
        If aChange Then
            Me.Text &= " *"
        End If
    End Sub

    Private Sub frmEdit_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not pEditFlag Then
            cmdEdit.Visible = False
        End If
        If Not pAddRemoveFlag Then
            cmdAdd.Visible = False
            cmdRemove.Visible = False
        End If
    End Sub
End Class