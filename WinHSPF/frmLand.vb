Imports System.Drawing
Imports MapWinUtility
Imports atcUCI
Imports atcControls
Imports atcUCIForms
Imports System.ComponentModel

Public Class frmLand
    Dim pOrigTotal As Double = 0
    Dim pAreaCalc As Double = 0
    Dim ListSources As New CustomCheckedListBox
    Dim ListTargets As New CustomCheckedListBox
    Dim pVScrollColumnOffset As Integer = 16
    Dim pChanged As Boolean = False

    Public Sub New()
        Dim lHspfOperator As HspfOperation
        Dim lHspfConnection As HspfConnection
        Dim lOper1, lOper2, lRow As Integer
        Dim lSchemFound, lNetFound As Boolean
        Dim lAddSourceNameAndId As String

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        Me.grpSources.Controls.Add(Me.ListSources)
        ListSources.FormattingEnabled = True
        ListSources.Location = New Point(9, 42)
        ListSources.Name = "ListSources"
        ListSources.Size = New Size(190, 184)
        ListSources.Visible = True


        Me.grpTargets.Controls.Add(Me.ListTargets)
        ListTargets.FormattingEnabled = True
        ListTargets.Location = New Point(9, 42)
        ListTargets.Name = "ListTargets"
        ListTargets.Size = New Size(190, 184)
        ListTargets.Visible = True

        Me.Text = "WinHSPF - LandUse Editor"
        Me.Icon = pIcon
        Me.MinimumSize = New Size(970, 635)

        CheckSchematic(lSchemFound, lNetFound)
        If lNetFound And Not lSchemFound Then
            If Logger.Msg("The Land Use Editor requires a Schematic Block." & vbCrLf & "Would you like to convert the Network Block to a Schematic Block?", MsgBoxStyle.YesNo, "WinHSPF - Land Use Editor") = 1 Then
                'ConvertNetworkToSchematic()
                WinHSPF.pUCI.MaxAreaByLand2Stream = 0
            End If
        End If


        With grdLand
            .Source = New atcControls.atcGridSource
            .Clear()
            .AllowHorizontalScrolling = False
            .AllowNewValidValues = True
            .Visible = True
        End With

        With grdLand.Source
            .Rows = 1
            .Columns = 5
            .FixedRows = 1
            .CellValue(0, 0) = "Source ID"
            .CellValue(0, 1) = "Source Description"
            .CellValue(0, 2) = "Target ID"
            .CellValue(0, 3) = "Target Description"
            If WinHSPF.pUCI.GlobalBlock.EmFg = 1 Then
                .CellValue(0, 4) = "Area (Acres)"
            Else
                .CellValue(0, 4) = "Area (Hectares)"
            End If

            If .Rows > 1 Then
                For lRow = 1 To .Rows - 1
                    .CellEditable(lRow, 4) = True
                Next
            End If

            For lOper1 = 1 To WinHSPF.pUCI.OpnSeqBlock.Opns.Count - 1
                lHspfOperator = WinHSPF.pUCI.OpnSeqBlock.Opn(lOper1)
                If lHspfOperator.Name = "RCHRES" Then
                    ListTargets.Items.Add(lHspfOperator.Name & " " & lHspfOperator.Id, True)

                    For lOper2 = 0 To lHspfOperator.Sources.Count - 1
                        lHspfConnection = lHspfOperator.Sources(lOper2)
                        lAddSourceNameAndId = lHspfConnection.Source.VolName & " " & lHspfConnection.Source.VolId
                        If (lHspfConnection.Source.VolName = "PERLND" Or lHspfConnection.Source.VolName = "IMPLND") And Not ListSources.Items.Contains(lAddSourceNameAndId) Then
                            ListSources.Items.Add(lHspfConnection.Source.VolName & " " & lHspfConnection.Source.VolId, True)
                        End If
                    Next
                End If
            Next lOper1

        End With

        ListSources.SelectionMode = SelectionMode.One
        ListTargets.SelectionMode = SelectionMode.One
        ListSources.CheckOnClick = True
        ListTargets.CheckOnClick = True

        'Manually add events to handle clicking on Targets/Sources and their check-all boxes.
        AddHandler chkAllSources.CheckStateChanged, AddressOf chkAllSources_CheckedChanged
        AddHandler ListSources.ItemCheck, AddressOf ListSources_IndividualCheckChanged
        AddHandler ListTargets.ItemCheck, AddressOf ListTargets_IndividualCheckChanged
        AddHandler chkAllTargets.CheckStateChanged, AddressOf chkAllTargets_CheckedChanged

        RefreshGrid()

    End Sub

    Private Function GetOperation(ByVal volname$, ByVal volid As Integer) As HspfOperation
        Dim lHspfOpnBlk As HspfOpnBlk
        Dim lHspfOperation As HspfOperation
        Dim lConnectionObject As Object
        GetOperation = Nothing

        lHspfOpnBlk = WinHSPF.pUCI.OpnBlks(volname)
        For Each lConnectionObject In lHspfOpnBlk.Ids
            lHspfOperation = lConnectionObject
            If lHspfOperation.Id = volid Then
                GetOperation = lHspfOperation
                Exit For
            End If
        Next

    End Function

    Private Sub RefreshGrid()
        Dim lOper1, lRow As Integer
        Dim lHspfOperator As HspfOperation
        Dim lHspfConnection As HspfConnection
        Dim lConnectionObject As Object
        Dim lSourceID, lTargetID As String

        With grdLand
            .Clear()
            .Source.Rows = 1
        End With

        For lOper1 = 1 To WinHSPF.pUCI.OpnSeqBlock.Opns.Count - 1
            lHspfOperator = WinHSPF.pUCI.OpnSeqBlock.Opn(lOper1)

            For Each lConnectionObject In lHspfOperator.Sources
                lHspfConnection = lConnectionObject
                lSourceID = lHspfConnection.Source.VolName & " " & lHspfConnection.Source.VolId
                lTargetID = lHspfOperator.Name & " " & lHspfOperator.Id
                lRow = grdLand.Source.Rows
                With ListSources
                    If lHspfOperator.Name <> "COPY" AndAlso .Items.Contains(lSourceID) AndAlso .GetItemChecked(.FindStringExact(lSourceID)) AndAlso ListTargets.GetItemChecked(ListTargets.FindStringExact(lTargetID)) Then
                        grdLand.Source.CellValue(lRow, 0) = lSourceID
                        grdLand.Source.CellValue(lRow, 1) = lHspfConnection.Source.Opn.Description
                        grdLand.Source.CellValue(lRow, 2) = lTargetID
                        grdLand.Source.CellValue(lRow, 3) = WinHSPF.pUCI.OpnSeqBlock.Opns(lOper1).Description
                        grdLand.Source.CellValue(lRow, 4) = lHspfConnection.MFact
                    End If
                End With
            Next
        Next

        With grdLand

            If .Source.Rows > 1 Then
                For lRow = 0 To .Source.Rows - 1
                    .Source.Alignment(lRow, 4) = atcAlignment.HAlignRight
                Next
            End If

            grdLand.SizeAllColumnsToContents(grdLand.Width - pVScrollColumnOffset, True)
            .Refresh()

            pAreaCalc = 0

            If grdLand.Source.Rows > 1 Then
                For lRow = 1 To grdLand.Source.Rows - 1
                    pAreaCalc = pAreaCalc + grdLand.Source.CellValue(lRow, 4)
                    grdLand.Source.CellEditable(lRow, 4) = True
                Next
            End If

        End With
        pOrigTotal = pAreaCalc
        CalculateAreaSums()

    End Sub

    Public Sub CheckSchematic(ByVal schemfound As Boolean, ByVal netfound As Boolean)
        Dim lConnectionObject As Object
        Dim lHspfConnection As HspfConnection

        schemfound = False
        netfound = False
        For Each lConnectionObject In WinHSPF.pUCI.Connections
            lHspfConnection = lConnectionObject
            If lHspfConnection.Typ = 3 Then
                schemfound = True
            ElseIf lHspfConnection.Typ = 2 Then
                netfound = True
            End If
        Next lConnectionObject
    End Sub

    Private Sub ListSources_IndividualCheckChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckEventArgs)

        RemoveHandler chkAllSources.CheckStateChanged, AddressOf chkAllSources_CheckedChanged

        chkAllSources.Checked = False

        AddHandler chkAllSources.CheckStateChanged, AddressOf chkAllSources_CheckedChanged

        RemoveHandler ListSources.ItemCheck, AddressOf ListSources_IndividualCheckChanged
        ListSources.SetItemChecked(e.Index, e.NewValue)
        AddHandler ListSources.ItemCheck, AddressOf ListSources_IndividualCheckChanged

        RefreshGrid()
    End Sub

    Private Sub chkAllSources_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim lRow As Integer
        RemoveHandler ListSources.ItemCheck, AddressOf ListSources_IndividualCheckChanged

        For lRow = 0 To ListSources.Items.Count - 1
            ListSources.SetItemChecked(lRow, chkAllSources.Checked)
        Next

        AddHandler ListSources.ItemCheck, AddressOf ListSources_IndividualCheckChanged

        RefreshGrid()
    End Sub

    Private Sub ListTargets_IndividualCheckChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckEventArgs)

        RemoveHandler chkAllTargets.CheckStateChanged, AddressOf chkAllTargets_CheckedChanged

        chkAllTargets.Checked = False

        AddHandler chkAllTargets.CheckStateChanged, AddressOf chkAllTargets_CheckedChanged

        RemoveHandler ListTargets.ItemCheck, AddressOf ListTargets_IndividualCheckChanged
        ListTargets.SetItemChecked(e.Index, e.NewValue)
        AddHandler ListTargets.ItemCheck, AddressOf ListTargets_IndividualCheckChanged

        RefreshGrid()
    End Sub

    Private Sub chkAllTargets_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim lRow As Integer
        RemoveHandler ListTargets.ItemCheck, AddressOf ListTargets_IndividualCheckChanged

        For lRow = 0 To ListTargets.Items.Count - 1
            ListTargets.SetItemChecked(lRow, chkAllTargets.Checked)
        Next

        AddHandler ListTargets.ItemCheck, AddressOf ListTargets_IndividualCheckChanged

        RefreshGrid()
    End Sub

    Private Sub grdTable_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles grdLand.Resize
        grdLand.SizeAllColumnsToContents(grdLand.Width - pVScrollColumnOffset, True)
    End Sub

    Private Sub CellEdited(ByVal aGrid As atcGrid, ByVal aRow As Integer, ByVal aColumn As Integer) Handles grdLand.CellEdited
        Dim lRow As Integer

        Me.Text = "WinHSPF - LandUse Editor *"
        pChanged = True
        pAreaCalc = 0

        If grdLand.Source.Rows > 1 Then
            For lRow = 1 To grdLand.Source.Rows - 1
                pAreaCalc = pAreaCalc + grdLand.Source.CellValue(lRow, 4)
                grdLand.Source.CellEditable(lRow, 4) = True
            Next
        End If

        SaveChanges()
        CalculateAreaSums()

    End Sub

    'This is a modified class written by JohnH posted at http://www.vbdotnetforums.com/. It is a checked list box that does not fill the select rectangle with the default blue.

    Public Class CustomCheckedListBox
        Inherits CheckedListBox

        Private _CheckedBackColor As Color = SystemColors.Window

        <Category("Appearance"), Description("The background color of checked items.")> _
        Public Property CheckedBackColor() As Color
            Get
                Return _CheckedBackColor
            End Get
            Set(ByVal value As Color)
                _CheckedBackColor = value
            End Set
        End Property

        Protected Overrides Sub OnDrawItem(ByVal e As System.Windows.Forms.DrawItemEventArgs)
            Dim lBoxLocation As Point
            Dim lChecked As Boolean = Me.CheckedIndices.Contains(e.Index)
            Dim lTextRectangle As Rectangle
            Dim lCheckState As VisualStyles.CheckBoxState
            Dim lText As String
            Dim lFormat As New StringFormat
            Dim lFocused As Boolean

            lTextRectangle = e.Bounds
            lTextRectangle.X += lTextRectangle.Height
            lTextRectangle.Width -= lTextRectangle.Height + 1

            'back color
            If lChecked Then
                Using sb As New SolidBrush(Me._CheckedBackColor)
                    e.Graphics.FillRectangle(sb, lTextRectangle)
                End Using
            Else
                Using sb As New SolidBrush(Me.BackColor)
                    e.Graphics.FillRectangle(sb, lTextRectangle)
                End Using
            End If

            'checkbox            
            lBoxLocation = e.Bounds.Location
            lBoxLocation.Offset(1, 1)

            lCheckState = VisualStyles.CheckBoxState.UncheckedNormal
            If lChecked Then
                lCheckState = VisualStyles.CheckBoxState.CheckedNormal
            End If

            CheckBoxRenderer.DrawCheckBox(e.Graphics, lBoxLocation, lCheckState)

            'text    
            Using sb As New SolidBrush(Me.ForeColor)
                lText = Me.Name
                If Me.Items.Count > 0 Then
                    lText = Me.GetItemText(Me.Items(e.Index))
                End If
                lFormat.LineAlignment = StringAlignment.Center
                lTextRectangle.Inflate(-3, 0)
                e.Graphics.DrawString(lText, Me.Font, sb, lTextRectangle, lFormat)
            End Using

            'focus rectangle
            lFocused = (e.State And DrawItemState.Selected) = DrawItemState.Selected
            If lFocused Then
                Using p As New Pen(Color.Black)
                    p.DashStyle = Drawing2D.DashStyle.Dot
                    lTextRectangle.Inflate(2, -1)
                    e.Graphics.DrawRectangle(p, lTextRectangle)
                End Using
            End If
        End Sub

        Public Sub New()

        End Sub
    End Class

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click

        If pChanged Then
            If Logger.Message("Do you want to save the changes to the Land Use Table?", "WinHSPF - LandUse Editor", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, Windows.Forms.DialogResult.Yes) = Microsoft.VisualBasic.MsgBoxResult.Yes Then
                SaveChanges()
                Me.Dispose()
            Else
                Me.Dispose()
            End If
        Else
            Me.Dispose()
        End If


    End Sub

    Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click

        If pChanged Then
            SaveChanges()
        End If

        Me.Dispose()

    End Sub
    Private Sub CalculateAreaSums()
        If pAreaCalc = pOrigTotal Then
            txtTotal.Text = CStr(Format(pAreaCalc, "#####0.00")).PadLeft(18)

            txtLabelOrigTotal.Visible = False
            txtLabelDifference.Visible = False
            txtOrigTotal.Visible = False
            txtDifference.Visible = False

            '.NET conversion: Switch Differnece label font to ControlText (un-do Red coloring)
            txtDifference.ForeColor = System.Drawing.SystemColors.ControlText
            txtLabelDifference.ForeColor = System.Drawing.SystemColors.ControlText
        Else
            txtLabelOrigTotal.Visible = True
            txtLabelDifference.Visible = True
            txtOrigTotal.Visible = True
            txtDifference.Visible = True

            txtTotal.Text = CStr(Format(pAreaCalc, "#####0.00")).PadLeft(18)
            txtOrigTotal.Text = CStr(Format(pOrigTotal, "#####0.00")).PadLeft(18)
            txtDifference.Text = CStr(Format(pAreaCalc - pOrigTotal, "#####0.00")).PadLeft(18)

            txtDifference.ForeColor = Color.Red
            txtLabelDifference.ForeColor = Color.Red
        End If
    End Sub

    Private Sub SaveChanges()
        Dim lHspfOperation As HspfOperation
        Dim lConnectionObject As Object
        Dim lHspfConnection As HspfConnection
        Dim i&, rows&, lName$, lId&
        Dim s() As String

        rows = 0
        With grdLand
            For i = 0 To ListTargets.CheckedItems.Count - 1
                s = ListTargets.CheckedItems.Item(i).ToString.Split(" "c)
                lName = s(0)
                lId = s(1)
                lHspfOperation = GetOperation(lName, lId)
                For Each lConnectionObject In lHspfOperation.Sources
                    lHspfConnection = lConnectionObject
                    If lHspfConnection.Typ = 3 Then
                        If ListSources.Items.Contains(lHspfConnection.Source.VolName & " " & lHspfConnection.Source.VolId) AndAlso ListSources.GetItemChecked(ListSources.FindStringExact(lHspfConnection.Source.VolName & " " & lHspfConnection.Source.VolId)) Then
                            rows += 1
                            lHspfConnection.MFact = grdLand.Source.CellValue(rows, 4)
                        End If
                    End If
                Next lConnectionObject
            Next i
        End With
        WinHSPF.pUCI.Edited = True
    End Sub

End Class
