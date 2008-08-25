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
        Dim i, j, lMassLinkRow, lMassLinkCol, rows As Integer
        Dim lMassLink As HspfMassLink
        Dim complete As Boolean

        'remove mass link records with this id
        lMassLinkRow = 1
        Do While lMassLinkRow <= pMassLink.Uci.MassLinks.Count - 1
            'remove mass links from collection
            lMassLink = pMassLink.Uci.MassLinks(lMassLinkRow)
            If lMassLink.MassLinkId = cboID.Items.Item(cboID.SelectedIndex) Then
                pMassLink.Uci.MassLinks.RemoveAt(lMassLinkRow)
            Else
                lMassLinkRow += 1
            End If
        Loop
        grdMassLink.Refresh()
        'put back mass links with this id
        With grdMassLink.Source
            For lMassLinkRow = i To .Rows - 1
                complete = True
                For lMassLinkCol = 1 To .Columns - 1
                    If Len(.CellValue(lMassLinkRow, lMassLinkCol - 1)) < 1 And j <> 3 And j <> 9 Then
                        'no data entered for this field, dont do this row
                        complete = False
                    End If
                Next
                If Not complete Then
                    Me.Hide()
                    Logger.Msg("Some required fields on row " & lMassLinkRow & " are empty." & vbCrLf & "This row will be ignored.", Microsoft.VisualBasic.MsgBoxStyle.OkOnly)
                    Me.Show()
                Else
                    lMassLink = New HspfMassLink
                    lMassLink.Uci = pMassLink.Uci
                    lMassLink.MassLinkId = cboID.Items.Item(cboID.SelectedIndex)
                    lMassLink.Source.VolName = .CellValue(lMassLinkRow, 0)
                    lMassLink.Source.Group = .CellValue(lMassLinkRow, 1)
                    lMassLink.Source.Member = .CellValue(lMassLinkRow, 2)
                    lMassLink.Source.MemSub1 = .CellValue(lMassLinkRow, 3)
                    lMassLink.Source.MemSub2 = .CellValue(lMassLinkRow, 4)
                    lMassLink.MFact = .CellValue(lMassLinkRow, 5)
                    lMassLink.Target.VolName = .CellValue(lMassLinkRow, 6)
                    lMassLink.Target.Group = .CellValue(lMassLinkRow, 7)
                    lMassLink.Target.Member = .CellValue(lMassLinkRow, 8)
                    lMassLink.Target.MemSub1 = .CellValue(lMassLinkRow, 9)
                    lMassLink.Target.MemSub2 = .CellValue(lMassLinkRow, 10)
                    lMassLink.Comment = .CellValue(lMassLinkRow, 11)
                    pMassLink.Uci.MassLinks.Add(lMassLink)
                End If
            Next
        End With
        pChanged = False
    End Sub
    Public Sub Add() Implements ctlEdit.Add
        Changed = True
    End Sub

    Public Property Data() As Object Implements ctlEdit.Data
        Get
            Return pMassLink
        End Get

        Set(ByVal aMassLink As Object)
            Dim found As Boolean
            Dim lMassLink As HspfMassLink, mlcnt, mlno&()
            pMassLink = aMassLink
            'build list of masslinks
            mlcnt = 0
            For lMassLinkNumber As Integer = 1 To pMassLink.Uci.MassLinks.Count - 1 '<<<< .NET Conversion: Changed for Debug
                lMassLink = pMassLink.Uci.MassLinks(lMassLinkNumber)
                found = False
                For k As Integer = 0 To mlcnt - 1
                    If lMassLink.MassLinkId = mlno(k) Then
                        found = True
                    End If
                Next k
                If found = False Then
                    mlcnt = mlcnt + 1
                    ReDim Preserve mlno(mlcnt)
                    mlno(mlcnt - 1) = lMassLink.MassLinkId
                End If
            Next

            cboID.Items.Clear()
            For lcboIDitem As Integer = 1 To mlcnt
                cboID.Items.Add(mlno(lcboIDitem - 1))
            Next
            cboID.SelectedIndex = 0
            prevMLid = cboID.SelectedIndex '<<<<<<< .NET Conversion: Not clear on what this was

            With grdMassLink
                .Source = New atcControls.atcGridSource
                .Clear()
                .AllowHorizontalScrolling = False
                .AllowNewValidValues = True
                .Visible = True
            End With

            RefreshGrid()
        End Set
    End Property
    Private Sub RefreshGrid()
        Dim lMassLink As HspfMassLink
        grdMassLink.Clear()
        With grdMassLink.Source
            .Rows = 0
            .Columns = 12

            .CellValue(0, 0) = "VolName"
            .CellValue(0, 1) = "Group"
            .CellValue(0, 2) = "MemName"
            .CellValue(0, 3) = "MemSub1"
            .CellValue(0, 4) = "MemSub2"
            .CellValue(0, 5) = "MultFactor"
            .CellValue(0, 6) = "VolName"
            .CellValue(0, 7) = "Group"
            .CellValue(0, 8) = "MemName"
            .CellValue(0, 9) = "MemSub1"
            .CellValue(0, 10) = "MemSub2"
            .CellValue(0, 11) = "HIDE"
            Dim lRowFillerIndex As Integer = 1
            For lRow As Integer = 1 To pMassLink.Uci.MassLinks.Count - 1
                lMassLink = pMassLink.Uci.MassLinks(lRow)
                If lMassLink.MassLinkId = cboID.Items.Item(cboID.SelectedIndex) Then
                    .CellValue(lRowFillerIndex, 0) = lMassLink.Source.VolName
                    .CellValue(lRowFillerIndex, 1) = lMassLink.Source.Group
                    .CellValue(lRowFillerIndex, 2) = lMassLink.Source.Member
                    .CellValue(lRowFillerIndex, 3) = lMassLink.Source.MemSub1
                    .CellValue(lRowFillerIndex, 4) = lMassLink.Source.MemSub2
                    .CellValue(lRowFillerIndex, 5) = lMassLink.MFact
                    .CellValue(lRowFillerIndex, 6) = lMassLink.Target.VolName
                    .CellValue(lRowFillerIndex, 7) = lMassLink.Target.Group
                    .CellValue(lRowFillerIndex, 8) = lMassLink.Target.Member
                    .CellValue(lRowFillerIndex, 9) = lMassLink.Target.MemSub1
                    .CellValue(lRowFillerIndex, 10) = lMassLink.Target.MemSub2
                    .CellValue(lRowFillerIndex, 11) = lMassLink.Comment
                    lRowFillerIndex += 1
                End If
            Next

            For lCol As Integer = 0 To .Columns - 1
                For lRow As Integer = 1 To .Rows - 1
                    .CellEditable(lRow, lCol) = True
                Next
            Next

            For lCol As Integer = 0 To 11
                .CellColor(0, lCol) = SystemColors.ControlLight
            Next

        End With
        grdMassLink.SizeAllColumnsToContents()
        grdMassLink.Refresh()
        pChanged = False
    End Sub

    Public Sub New(ByVal aHspfMassLink As Object, ByVal aParent As Windows.Forms.Form, ByVal aTag As String)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        grdMassLink.Source = New atcGridSource
        Data = aHspfMassLink
    End Sub

    Private Sub cboID_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboID.SelectedIndexChanged
        Dim discard As Integer
        If prevMLid <> cboID.SelectedIndex Then
            If Changed Then
                Me.Hide()
                discard = Logger.Msg("Changes to current MassLink have not been saved. Discard them?", Microsoft.VisualBasic.MsgBoxStyle.OkOnly)
                Me.Show()
            Else
                discard = 1 'no changes
            End If
            If discard = 1 Then 'discard
                pChanged = False
                RefreshGrid()
            Else 'no discard
                cboID.SelectedIndex = prevMLid
            End If
        End If
        prevMLid = cboID.SelectedIndex
    End Sub

    Private Sub cmdAddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAddNew.Click
        MsgBox(cboID.Items.Item(cboID.SelectedIndex))
    End Sub
End Class
