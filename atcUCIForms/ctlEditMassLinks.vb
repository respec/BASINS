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
    Dim lMassLinkIdStarter As New ArrayList()

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
        Dim lMassLinkRow As Integer
        Dim lMassLink, lMassLinkToAdd As HspfMassLink
        Dim complete As Boolean
        Dim lMassLinkTableRow, lMassLinkTableCol As Integer
        lMassLinkTableRow = 1


        '.net converstion: The following For-loop cycles through all MassLinks. If the ID of a row matches the ID of the displayed table. 
        'Then the rows in the reocrd are replaced with the rows of the table.

        For lMassLinkRow = 1 To pMassLink.Uci.MassLinks.Count - 1
            lMassLink = pMassLink.Uci.MassLinks(lMassLinkRow)
            If lMassLink.MassLinkId = cboID.Items.Item(cboID.SelectedIndex) Then

                With grdMassLink.Source
                    complete = True
                    For lMassLinkTableCol = 1 To .Columns - 1
                        If Len(.CellValue(lMassLinkTableRow, lMassLinkTableCol - 1)) < 1 And lMassLinkTableCol <> 3 And lMassLinkTableCol <> 9 Then
                            'no data entered for this field, dont do this row
                            complete = False
                        End If
                    Next
                    If Not complete Then
                        Me.Hide()
                        Logger.Msg("Some required fields on row " & lMassLinkTableRow & " are empty." & vbCrLf & "This row will be ignored.", Microsoft.VisualBasic.MsgBoxStyle.OkOnly)
                        Me.Show()
                    Else
                        lMassLinkToAdd = New HspfMassLink
                        lMassLinkToAdd.Uci = pMassLink.Uci
                        lMassLinkToAdd.MassLinkId = cboID.Items.Item(cboID.SelectedIndex)
                        lMassLinkToAdd.Source.VolName = .CellValue(lMassLinkTableRow, 0)
                        lMassLinkToAdd.Source.Group = .CellValue(lMassLinkTableRow, 1)
                        lMassLinkToAdd.Source.Member = .CellValue(lMassLinkTableRow, 2)
                        lMassLinkToAdd.Source.MemSub1 = .CellValue(lMassLinkTableRow, 3)
                        lMassLinkToAdd.Source.MemSub2 = .CellValue(lMassLinkTableRow, 4)
                        lMassLinkToAdd.MFact = .CellValue(lMassLinkTableRow, 5)
                        lMassLinkToAdd.Target.VolName = .CellValue(lMassLinkTableRow, 6)
                        lMassLinkToAdd.Target.Group = .CellValue(lMassLinkTableRow, 7)
                        lMassLinkToAdd.Target.Member = .CellValue(lMassLinkTableRow, 8)
                        lMassLinkToAdd.Target.MemSub1 = .CellValue(lMassLinkTableRow, 9)
                        lMassLinkToAdd.Target.MemSub2 = .CellValue(lMassLinkTableRow, 10)
                        lMassLinkToAdd.Comment = .CellValue(lMassLinkTableRow, 11)
                        pMassLink.Uci.MassLinks.RemoveAt(lMassLinkRow)
                        pMassLink.Uci.MassLinks.Insert(lMassLinkRow, lMassLinkToAdd)
                        lMassLinkTableRow += 1
                    End If
                End With
            End If
        Next
        RefreshGrid()
        grdMassLink.Refresh()
        pChanged = False
    End Sub
    Public Sub Add() Implements ctlEdit.Add

        '.net conversion:
        'Add uses the array.insert call to place the entry in order in the MassLink.Uci.MassLinks array. The lMassLinkIdStarter
        'indexes the rows where each ID starts - i.e. the second entry in ID lMassLinkIdStarter corresponds to the row
        'where the second ID in the combobox (from the top) starts. 

        With grdMassLink.Source
            .Rows += 1
            Dim lMassLinkToAddRow As Integer = lMassLinkIdStarter.Item(cboID.SelectedIndex) + .Rows - 2
            Dim lMassLinkToAdd As HspfMassLink

            lMassLinkToAdd = New HspfMassLink
            lMassLinkToAdd.Uci = pMassLink.Uci
            lMassLinkToAdd.MassLinkId = cboID.Items.Item(cboID.SelectedIndex)

            pMassLink.Uci.MassLinks.Insert(lMassLinkToAddRow, lMassLinkToAdd)
        End With
        Changed = True
    End Sub

    Public Property Data() As Object Implements ctlEdit.Data
        Get
            Return pMassLink
        End Get

        Set(ByVal aMassLink As Object)
            Dim found As Boolean
            Dim lMassLink As HspfMassLink
            Dim lMassLinkLooperCount
            Dim lMassLinkLooperID() As Integer
            lMassLinkLooperID = New Integer() {} '.net conversion: Silences 'used before assigned' error
            pMassLink = aMassLink

            'build list of masslinks
            lMassLinkLooperCount = 0
            For lMassLinkNumber As Integer = 1 To pMassLink.Uci.MassLinks.Count - 1 '<<<< .NET Conversion: Changed for Debug
                lMassLink = pMassLink.Uci.MassLinks(lMassLinkNumber)
                found = False
                For lMassLinkRowEntry As Integer = 0 To lMassLinkLooperCount - 1
                    If lMassLink.MassLinkId = lMassLinkLooperID(lMassLinkRowEntry) Then
                        found = True
                    End If
                Next

                If found = False Then
                    lMassLinkLooperCount += 1
                    ReDim Preserve lMassLinkLooperID(lMassLinkLooperCount)
                    lMassLinkLooperID(lMassLinkLooperCount - 1) = lMassLink.MassLinkId
                    lMassLinkIdStarter.Add(lMassLinkNumber)
                End If
            Next
            lMassLinkIdStarter(0) -= 1

            cboID.Items.Clear()
            For lcboIDitem As Integer = 1 To lMassLinkLooperCount
                cboID.Items.Add(lMassLinkLooperID(lcboIDitem - 1))
            Next
            cboID.SelectedIndex = 0
            prevMLid = cboID.SelectedIndex

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
                discard = Logger.Msg("Changes to current MassLink have not been saved. Discard them?", Microsoft.VisualBasic.MsgBoxStyle.YesNo)
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
    Private Sub grdMassLink_CellEdited(ByVal aGrid As atcControls.atcGrid, ByVal aRow As Integer, ByVal aColumn As Integer) Handles grdMassLink.CellEdited
        Changed = True
    End Sub
    Private Sub cmdAddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAddNew.Click
        Dim lOper, lCheckThisId, lIdExistsAtIndex As Integer
        Dim lExists As Boolean
        Dim lcboIdItems() As Integer
        lcboIdItems = New Integer() {}

        For lOper = 0 To cboID.Items.Count - 1
            ReDim Preserve lcboIdItems(lcboIdItems.Length)
            lcboIdItems(lOper) = cboID.Items.Item(lOper)
        Next

        lExists = True
        lCheckThisId = 0
        Do Until lExists = False
            lCheckThisId += 1
            lIdExistsAtIndex = Array.IndexOf(lcboIdItems, lCheckThisId)
            If lIdExistsAtIndex = -1 Then
                cboID.Items.Insert(lCheckThisId - 1, lCheckThisId)
                Exit Do
            End If
        Loop

        lMassLinkIdStarter.Insert(lCheckThisId - 1, lMassLinkIdStarter.Item(lCheckThisId - 1))
        For lOper = 1 To (lMassLinkIdStarter.Count - lCheckThisId)
            lMassLinkIdStarter.Item(lMassLinkIdStarter.Count - lOper) += 1
        Next

        cboID.SelectedIndex = lCheckThisId - 1
        Add()


    End Sub

End Class
