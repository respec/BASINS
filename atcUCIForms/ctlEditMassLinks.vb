Imports System.Drawing
Imports MapWinUtility
Imports atcUCI
Imports atcControls
Imports System.Collections.ObjectModel

Public Class ctlEditMassLinks
    Implements ctlEdit

    Dim pMassLink As HspfMassLink
    Dim pChanged As Boolean
    Public Event Change(ByVal aChange As Boolean) Implements ctlEdit.Change
    Dim prevMLid As Integer
    Public ReadOnly Property Caption() As String Implements ctlEdit.Caption
        Get
            Return "Edit Mass Link Block"
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
    Public Sub Help() Implements ctlEdit.Help
        'TODO: add this code
    End Sub
    Public Sub Remove() Implements ctlEdit.Remove
        'TODO: add this code
    End Sub

    Public Sub Save() Implements ctlEdit.Save
        'TODO: add this code
    End Sub
    Public Sub Add() Implements ctlEdit.Add
        Changed = True
    End Sub

    Public Property Data() As Object Implements ctlEdit.Data
        Get
            Return pMassLink
        End Get

        Set(ByVal aMassLink As Object)
            Dim i, k As Integer
            Dim found As Boolean
            Dim lMassLink As HspfMassLink, mlcnt, mlno&()
            pMassLink = aMassLink
            'build list of masslinks
            mlcnt = 0
            For i = 1 To pMassLink.Uci.MassLinks.Count - 1 '<<<< .NET Conversion: Changed for Debug
                lMassLink = pMassLink.Uci.MassLinks(i)
                found = False
                For k = 0 To mlcnt - 1
                    If lMassLink.MassLinkId = mlno(k) Then
                        found = True
                    End If
                Next k
                If found = False Then
                    mlcnt = mlcnt + 1
                    ReDim Preserve mlno(mlcnt)
                    mlno(mlcnt - 1) = lMassLink.MassLinkId
                End If
            Next i

            cboID.Items.Clear()
            For i = 1 To mlcnt
                cboID.Items.Add(mlno(i - 1))
            Next i
            cboID.SelectedIndex = 0
            prevMLid = cboID.SelectedIndex '<<<<<<< .NET Conversion: Not clear on what this was
            Refresh()
        End Set
    End Property
    Public Sub New(ByVal aHspfMassLink As Object, ByVal aParent As Windows.Forms.Form, ByVal aTag As String)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        grdMassLink.Source = New atcGridSource
        Data = aHspfMassLink
    End Sub
End Class
