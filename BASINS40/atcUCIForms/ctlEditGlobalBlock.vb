Imports System.Drawing
Imports MapWinUtility
Imports atcUCI
Imports atcControls

Public Class ctlEditGlobalBlock
    Implements ctlEdit

    Dim pHspfGlobalBlk As HspfGlobalBlk
    Dim pChanged As Boolean
    Public Event Change(ByVal aChange As Boolean) Implements ctlEdit.Change

    Public ReadOnly Property Caption() As String Implements ctlEdit.Caption
        Get
            Return "Global Block"
        End Get
    End Property

    Public Property Changed() As Boolean Implements ctlEdit.Changed
        Get
            Return pChanged
        End Get
        Set(ByVal aChanged As Boolean)
            If aChanged <> pChanged Then
                pChanged = aChanged
                RaiseEvent Change(aChanged)
            End If
        End Set
    End Property

    Public Sub Add() Implements ctlEdit.Add
        'not needed 
    End Sub

    Public Property Data() As Object Implements ctlEdit.Data
        Get
            Return pHspfGlobalBlk
        End Get
        Set(ByVal aHspfGlobalBlk As Object)
            pHspfGlobalBlk = aHspfGlobalBlk
            txtRunInfo.Text = pHspfGlobalBlk.RunInf.Value
        End Set
    End Property

    Public Sub Help() Implements ctlEdit.Help
        'TODO: add this code
    End Sub

    Public Sub Remove() Implements ctlEdit.Remove
        'not be needed
    End Sub

    Public Sub Save() Implements ctlEdit.Save
        pHspfGlobalBlk.RunInf.Value = txtRunInfo.Text
    End Sub

    Public Sub New(ByVal aHspfFilesBlk As Object, ByVal aParent As Windows.Forms.Form)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Data = aHspfFilesBlk
    End Sub
End Class
