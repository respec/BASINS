Imports atcUCI
Imports atcUtility
Imports MapWinUtility
Imports System.Drawing
Imports System.Windows.Forms

Public Class ctlEditSpecialAction
    Implements ctlEdit

    Dim pVScrollColumnOffset As Integer = 16
    Dim pSpecialActionBlk As HspfSpecialActionBlk
    Dim pChanged As Boolean
    Dim PreviousTab As Integer = 0
    Dim pfrmAgPrac As frmAgPrac
    Dim pCurrentSelectedRow As Integer
    Public Event Change(ByVal aChange As Boolean) Implements ctlEdit.Change

    Public ReadOnly Property Caption() As String Implements ctlEdit.Caption
        Get
            Return "Edit Special Actions Block"
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
    Private Sub Display()
        DisplayRecords()
        DisplayCounts()
    End Sub
    Private Sub DisplayCounts()
        Dim ac, dc, unc, uqc, cc, lRow As Integer

        ac = 0
        dc = 0
        unc = 0
        uqc = 0
        cc = 0

        With atcgrid0
            For lRow = 1 To .Source.Rows - 1   'top header row was being counted so subtracted 1
                Select Case .Source.CellValue(lRow, 0)
                    Case "Action" : ac += 1
                    Case "Distribute" : dc += 1
                    Case "User Defn Name" : unc += 1
                    Case "User Defn Quan" : uqc += 1
                    Case "Condition" : cc += 1
                End Select
            Next
            lblcounts.Text = "Records: " & (.Source.Rows - 1) & ", Actions: " & (ac) & ", Distributes: " & (dc) & ", User Define Names: " & (unc) & ", User Define Quans: " & (uqc) & ", Conditions: " & (cc)
        End With
    End Sub
    Private Sub DisplayRecords()

        Dim lRecordType As String
        With pSpecialActionBlk.Records
            For lOper As Integer = 0 To .Count - 1
                lRecordType = pSpecialActionBlk.HspfSpecialRecordName(.Item(lOper).SpecType)
                atcgrid0.Source.CellValue(lOper + 1, 0) = lRecordType
                atcgrid0.Source.CellValue(lOper + 1, 1) = .Item(lOper).Text
            Next
        End With

        atcgrid0.SizeAllColumnsToContents(atcgrid0.Width - pVScrollColumnOffset, True)
        atcgrid0.Refresh()

    End Sub

    Private Sub Allatcgrid_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles atcgrid0.Resize, atcgrid1.Resize, atcgrid2.Resize, atcgrid3.Resize, atcgrid4.Resize, atcgrid5.Resize
        atcgrid0.SizeAllColumnsToContents(atcgrid0.Width - pVScrollColumnOffset, True)
        atcgrid1.SizeAllColumnsToContents(atcgrid1.Width - pVScrollColumnOffset, True)
        atcgrid2.SizeAllColumnsToContents(atcgrid2.Width - pVScrollColumnOffset, True)
        atcgrid3.SizeAllColumnsToContents(atcgrid3.Width - pVScrollColumnOffset, True)
        atcgrid4.SizeAllColumnsToContents(atcgrid4.Width - pVScrollColumnOffset, True)
        atcgrid5.SizeAllColumnsToContents(atcgrid5.Width - pVScrollColumnOffset, True)
    End Sub

    Private Sub PutRecsToFrontTab(ByVal itab As Integer)
        Dim lRow, lCol, rowcount As Integer
        Dim newText, ctemp As String

        If itab = 1 Then
            'action type records
            rowcount = 0
            With atcgrid1.Source
                For lRow = 1 To atcgrid0.Source.Rows
                    If atcgrid0.Source.CellValue(lRow, 0) = "Action" Then
                        'get next record from this tab
                        rowcount = rowcount + 1
                        newText = "  "
                        newText = BlankPad(newText & .CellValue(rowcount, 0), 8)
                        ctemp = "   "
                        ctemp = RSet(.CellValue(rowcount, 1), Len(ctemp))
                        newText = newText & ctemp
                        If Len(.CellValue(rowcount, 2)) = 0 Then
                            newText = newText & "    "
                        ElseIf .CellValue(rowcount, 2) = 0 Then
                            newText = newText & "    "
                        Else
                            ctemp = "   "
                            ctemp = RSet(.CellValue(rowcount, 2), Len(ctemp))
                            newText = newText & ctemp
                        End If
                        newText = BlankPad(newText & .CellValue(rowcount, 3), 17)
                        If Len(.CellValue(rowcount, 4)) = 0 Then
                            newText = newText & "   "
                        ElseIf .CellValue(rowcount, 4) = 0 Then
                            newText = newText & "   "
                        Else
                            ctemp = "   "
                            ctemp = RSet(.CellValue(rowcount, 4), Len(ctemp))
                            newText = newText & ctemp
                        End If
                        If Len(.CellValue(rowcount, 5)) = 0 Then
                            newText = newText & "    "
                        ElseIf .CellValue(rowcount, 5) = 0 Then
                            newText = newText & "    "
                        Else
                            newText = BlankPad(newText & .CellValue(rowcount, 5), 24)
                        End If
                        If Len(.CellValue(rowcount, 6)) = 0 Then
                            newText = newText & "   "
                        ElseIf .CellValue(rowcount, 6) = 0 Then
                            newText = newText & "   "
                        Else
                            ctemp = "   "
                            ctemp = RSet(.CellValue(rowcount, 6), Len(ctemp))
                            newText = newText & ctemp
                        End If
                        If Len(.CellValue(rowcount, 7)) = 0 Then
                            newText = newText & "   "
                        ElseIf .CellValue(rowcount, 7) = 0 Then
                            newText = newText & "   "
                        Else
                            ctemp = "   "
                            ctemp = RSet(.CellValue(rowcount, 7), Len(ctemp))
                            newText = newText & ctemp
                        End If
                        If Len(.CellValue(rowcount, 8)) = 0 Then
                            newText = newText & "   "
                        ElseIf .CellValue(rowcount, 8) = 0 Then
                            newText = newText & "   "
                        Else
                            ctemp = "   "
                            ctemp = RSet(.CellValue(rowcount, 8), Len(ctemp))
                            newText = newText & ctemp
                        End If
                        If Len(.CellValue(rowcount, 9)) = 0 Then
                            newText = newText & "   "
                        ElseIf .CellValue(rowcount, 9) = 0 Then
                            newText = newText & "   "
                        Else
                            ctemp = "   "
                            ctemp = RSet(.CellValue(rowcount, 9), Len(ctemp))
                            newText = newText & ctemp
                        End If
                        If Len(.CellValue(rowcount, 10)) = 0 Then
                            newText = newText & "  "
                        ElseIf .CellValue(rowcount, 10) = 0 Then
                            newText = newText & "  "
                        Else
                            ctemp = "  "
                            ctemp = RSet(.CellValue(rowcount, 10), Len(ctemp))
                            newText = newText & ctemp
                        End If
                        ctemp = "  "
                        ctemp = RSet(.CellValue(rowcount, 11), Len(ctemp))
                        newText = newText & ctemp & "  "
                        If IsNumeric(.CellValue(rowcount, 12)) Then
                            newText = BlankPad(newText & .CellValue(rowcount, 12), 57) 'addr
                        Else
                            newText = BlankPad(newText & .CellValue(rowcount, 12), 48) 'vname
                            newText = BlankPad(newText & .CellValue(rowcount, 13), 51)
                            newText = BlankPad(newText & .CellValue(rowcount, 14), 54)
                            newText = BlankPad(newText & .CellValue(rowcount, 15), 57)
                        End If
                        newText = BlankPad(newText & .CellValue(rowcount, 16), 60)
                        If IsNumeric(.CellValue(rowcount, 17)) Then
                            ctemp = "          "
                            ctemp = RSet(.CellValue(rowcount, 17), Len(ctemp))
                            newText = newText & ctemp 'value
                        Else
                            newText = BlankPad(newText & .CellValue(rowcount, 17), 70) 'quan
                        End If
                        newText = newText & " "
                        newText = BlankPad(newText & .CellValue(rowcount, 18), 73)
                        If .CellValue(rowcount, 19) = 0 Then
                            newText = newText & "    "
                        Else
                            ctemp = "    "
                            ctemp = RSet(.CellValue(rowcount, 19), Len(ctemp))
                            newText = newText & ctemp
                        End If
                        If .CellValue(rowcount, 20) = 0 Then
                            newText = newText & "   "
                        Else
                            ctemp = "   "
                            ctemp = RSet(.CellValue(rowcount, 20), Len(ctemp))
                            newText = newText & ctemp
                        End If
                        atcgrid0.Source.CellValue(lRow, 1) = newText
                        atcgrid0.Source.CellEditable(lRow, 0) = True
                    End If
                Next
            End With
        ElseIf itab = 2 Then
            'distribute records
            rowcount = 0
            With atcgrid2.Source
                For lRow = 1 To .Rows
                    If atcgrid0.Source.CellValue(lRow, 0) = "Distribute" Then
                        'get next record from this tab
                        rowcount = rowcount + 1
                        newText = "  DISTRB"
                        ctemp = "   "
                        ctemp = RSet(.CellValue(rowcount, 0), Len(ctemp))
                        newText = newText & ctemp
                        ctemp = "    "
                        ctemp = RSet(.CellValue(rowcount, 1), Len(ctemp))
                        newText = newText & ctemp & " "
                        newText = BlankPad(newText & .CellValue(rowcount, 2), 18)
                        ctemp = "    "
                        ctemp = RSet(.CellValue(rowcount, 3), Len(ctemp))
                        newText = newText & ctemp & " "
                        newText = BlankPad(newText & .CellValue(rowcount, 4), 30)
                        For lCol = 1 To 10
                            ctemp = "     "
                            ctemp = RSet(.CellValue(rowcount, 4 + lCol), Len(ctemp))
                            newText = newText & ctemp
                        Next
                        atcgrid0.Source.CellValue(lRow, 1) = newText
                        atcgrid0.Source.CellEditable(lRow, 0) = True
                    End If
                Next
            End With
        ElseIf itab = 3 Then
            'User Defn Name records
            rowcount = 0
            With atcgrid3.Source
                For lRow = 1 To atcgrid0.Source.Rows
                    If atcgrid0.Source.CellValue(lRow, 0) = "User Defn Name" Then
                        'get next record from this tab
                        rowcount = rowcount + 1
                        newText = "  UVNAME  "
                        newText = BlankPad(newText & .CellValue(rowcount, 0), 16)
                        ctemp = "   "
                        ctemp = RSet(.CellValue(rowcount, 1), Len(ctemp))
                        newText = newText & ctemp & " "
                        If IsNumeric(.CellValue(rowcount, 2)) Then
                            newText = BlankPad(newText & .CellValue(rowcount, 2), 35) 'addr
                        Else
                            newText = BlankPad(newText & .CellValue(rowcount, 2), 26) 'vname
                            newText = BlankPad(newText & .CellValue(rowcount, 3), 29)
                            newText = BlankPad(newText & .CellValue(rowcount, 4), 32)
                            newText = BlankPad(newText & .CellValue(rowcount, 5), 35)
                        End If
                        newText = newText & " "
                        ctemp = "     "
                        ctemp = RSet(.CellValue(rowcount, 6), Len(ctemp))
                        newText = newText & ctemp & " "
                        newText = BlankPad(newText & .CellValue(rowcount, 7), 46)
                        newText = newText & "    "
                        If IsNumeric(.CellValue(rowcount, 8)) Then
                            newText = BlankPad(newText & .CellValue(rowcount, 8), 65) 'addr
                        Else
                            newText = BlankPad(newText & .CellValue(rowcount, 8), 56) 'vname
                            newText = BlankPad(newText & .CellValue(rowcount, 9), 59)
                            newText = BlankPad(newText & .CellValue(rowcount, 10), 62)
                            newText = BlankPad(newText & .CellValue(rowcount, 11), 65)
                        End If
                        newText = newText & " "
                        ctemp = "     "
                        ctemp = RSet(.CellValue(rowcount, 12), Len(ctemp))
                        newText = newText & ctemp & " "
                        newText = BlankPad(newText & .CellValue(rowcount, 13), 76)
                        atcgrid0.Source.CellValue(lRow, 1) = newText
                        atcgrid0.Source.CellEditable(lRow, 0) = True
                    End If
                Next
            End With
        ElseIf itab = 4 Then
            'User Defn Quan records
            rowcount = 0
            With atcgrid4.Source
                For lRow = 1 To atcgrid0.Source.Rows
                    If atcgrid0.Source.CellValue(lRow, 0) = "User Defn Quan" Then
                        'get next record from this tab
                        rowcount = rowcount + 1
                        newText = "  UVQUAN "
                        newText = BlankPad(newText & .CellValue(rowcount, 0), 16)
                        newText = BlankPad(newText & .CellValue(rowcount, 1), 23)
                        ctemp = "   "
                        ctemp = RSet(.CellValue(rowcount, 2), Len(ctemp))
                        newText = newText & ctemp & " "
                        If IsNumeric(.CellValue(rowcount, 3)) Then
                            newText = BlankPad(newText & .CellValue(rowcount, 3), 42) 'addr
                        Else
                            newText = BlankPad(newText & .CellValue(rowcount, 3), 33) 'vname
                            newText = BlankPad(newText & .CellValue(rowcount, 4), 36)
                            newText = BlankPad(newText & .CellValue(rowcount, 5), 39)
                            newText = BlankPad(newText & .CellValue(rowcount, 6), 42)
                        End If
                        ctemp = "   "
                        ctemp = RSet(.CellValue(rowcount, 7), Len(ctemp))
                        newText = newText & ctemp
                        ctemp = "          "
                        ctemp = RSet(.CellValue(rowcount, 8), Len(ctemp))
                        newText = newText & ctemp & " "
                        newText = BlankPad(newText & .CellValue(rowcount, 9), 58)
                        ctemp = "   "
                        ctemp = RSet(.CellValue(rowcount, 10), Len(ctemp))
                        newText = newText & ctemp & " "
                        newText = BlankPad(newText & .CellValue(rowcount, 11), 64)
                        ctemp = "   "
                        ctemp = RSet(.CellValue(rowcount, 12), Len(ctemp))
                        newText = newText & ctemp & " "
                        newText = newText & .CellValue(rowcount, 13)
                        atcgrid0.Source.CellValue(lRow, 1) = newText
                        atcgrid0.Source.CellEditable(lRow, 0) = True
                    End If
                Next
            End With
        ElseIf itab = 5 Then
            'conditional records
            rowcount = 0
            With atcgrid5.Source
                For lRow = 1 To atcgrid0.Source.Rows
                    If atcgrid0.Source.CellValue(lRow, 0) = "Condition" Then
                        'get next record from this tab
                        rowcount = rowcount + 1
                        atcgrid0.Source.CellValue(lRow, 1) = .CellValue(rowcount, 0)
                        atcgrid0.Source.CellEditable(lRow, 0) = True
                    End If
                Next
            End With
        End If
    End Sub
    Private Function BlankPad(ByVal ctxt As String, ByVal ilen As Integer)
        'pad a string to be the desired length
        Dim lOper1, lOper2 As Integer
        If Len(ctxt) > ilen Then
            BlankPad = Mid(ctxt, 1, ilen)
        ElseIf Len(ctxt) < ilen Then
            lOper2 = ilen - Len(ctxt)
            BlankPad = ctxt
            For lOper1 = 1 To lOper2
                BlankPad = BlankPad & " "
            Next
        Else
            BlankPad = ctxt
        End If
    End Function

    Public Sub Add() Implements ctlEdit.Add
        Changed = True

        Dim lCaption As String = "Special Action Add Problem"

        With tabSpecial
            If .TabIndex = 0 Then 'add a record
                With atcgrid0.Source
                    If pCurrentSelectedRow > 0 Then
                        .Rows += 1
                        For lRow As Integer = .Rows To pCurrentSelectedRow - 1
                            For lColumn As Integer = 0 To .Columns - 1
                                .CellValue(lRow, lColumn) = .CellValue(lRow - 1, lColumn)
                            Next
                        Next
                        .CellValue(pCurrentSelectedRow, 0) = "Comment"
                        .CellValue(pCurrentSelectedRow, 1) = ""
                        DisplayCounts()
                    Else
                        Logger.Msg("Select a Row to Add after ", MsgBoxStyle.OkOnly, lCaption)
                    End If
                End With
            Else
                Logger.Msg("Add " & .TabPages(.TabIndex).Text & " records using the 'Records' tab.", MsgBoxStyle.OkOnly, lCaption)
            End If
        End With
    End Sub

    Public Sub Help() Implements ctlEdit.Help
        'TODO: add this code
    End Sub

    Public Sub Remove() Implements ctlEdit.Remove
        Changed = True
        Dim lCaption As String = "Special Action Remove Problem"

        With tabSpecial
            If .TabIndex = 0 Then 'remove a record
                With atcgrid0.Source
                    If pCurrentSelectedRow > 0 Then
                        For lRow As Integer = pCurrentSelectedRow To .Rows - 1
                            For lColumn As Integer = 0 To .Columns - 1
                                .CellValue(lRow, lColumn) = .CellValue(lRow + 1, lColumn)
                            Next
                        Next
                        .Rows -= 1
                    Else
                        Logger.Msg("No Special Action available to Remove", MsgBoxStyle.OkOnly, lCaption)
                    End If
                End With
            Else
                Logger.Msg("Remove " & .TabPages(.TabIndex).Text & " records using the 'Records' tab.", MsgBoxStyle.OkOnly, lCaption)
            End If
        End With
    End Sub

    Public Property Data() As Object Implements ctlEdit.Data
        Get
            Return pSpecialActionBlk
        End Get


        Set(ByVal aHspfSpecialAction As Object)
            pSpecialActionBlk = aHspfSpecialAction
            atcgrid0.Source = New atcControls.atcGridSource

            With atcgrid0
                .Clear()
                .AllowHorizontalScrolling = False
                .AllowNewValidValues = True
                .AutoScroll = True
                .Visible = True
            End With

            With atcgrid0.Source
                .CellValue(0, 0) = "Type"
                .CellValue(0, 1) = "Text"
                .FixedRows = 1
            End With

            atcgrid0.SizeAllColumnsToContents(atcgrid0.Width - pVScrollColumnOffset, True)
            atcgrid0.Refresh()
            'action records

            atcgrid1.Source = New atcControls.atcGridSource
            With atcgrid1
                .Clear()
                .AllowHorizontalScrolling = False
                .AllowNewValidValues = True
                .Visible = True
            End With

            With atcgrid1.Source
                .CellValue(0, 0) = "OpTyp"
                .CellValue(0, 1) = "OpFst"
                .CellValue(0, 2) = "OpLst"
                .CellValue(0, 3) = "Dc"
                .CellValue(0, 4) = "Ds"
                .CellValue(0, 5) = "Yr"
                .CellValue(0, 6) = "Mo"
                .CellValue(0, 7) = "Dy"
                .CellValue(0, 8) = "Hr"
                .CellValue(0, 9) = "Mn"
                .CellValue(0, 10) = "DsInd"
                .CellValue(0, 11) = "Typ"
                .CellValue(0, 12) = "Vname/Addr"
                .CellValue(0, 13) = "Sub1"
                .CellValue(0, 14) = "Sub2"
                .CellValue(0, 15) = "Sub3"
                .CellValue(0, 16) = "ActCod"
                .CellValue(0, 17) = "Value/Uvquan"
                .CellValue(0, 18) = "Tc"
                .CellValue(0, 19) = "Ts"
                .CellValue(0, 20) = "Num"

                .FixedRows = 1
            End With

            atcgrid1.SizeAllColumnsToContents(atcgrid1.Width - pVScrollColumnOffset, True)
            atcgrid1.Refresh()

            'distributes
            atcgrid2.Source = New atcControls.atcGridSource
            With atcgrid2
                .Clear()
                .AllowHorizontalScrolling = False
                .AllowNewValidValues = True
                .Visible = True
            End With

            With atcgrid2.Source
                .CellValue(0, 0) = "DSInd"
                .CellValue(0, 1) = "Count"
                .CellValue(0, 2) = "CTCode"
                .CellValue(0, 3) = "TStep"
                .CellValue(0, 4) = "DefFg"
                .CellValue(0, 5) = "Frac1"
                .CellValue(0, 6) = "Frac2"
                .CellValue(0, 7) = "Frac3"
                .CellValue(0, 8) = "Frac4"
                .CellValue(0, 9) = "Frac5"
                .CellValue(0, 10) = "Frac6"
                .CellValue(0, 11) = "Frac7"
                .CellValue(0, 12) = "Frac8"
                .CellValue(0, 13) = "Frac9"
                .CellValue(0, 14) = "Frac10"

                .FixedRows = 1
            End With

            atcgrid2.SizeAllColumnsToContents(atcgrid2.Width - pVScrollColumnOffset, True)
            atcgrid2.Refresh()

            'uvnames

            atcgrid3.Source = New atcControls.atcGridSource
            With atcgrid3
                .Clear()
                .AllowHorizontalScrolling = False
                .AllowNewValidValues = True
                .Visible = True
            End With

            With atcgrid3.Source
                .CellValue(0, 0) = "UVName"
                .CellValue(0, 1) = "VCount"
                .CellValue(0, 2) = "VName/Addr"
                .CellValue(0, 3) = "Sub1"
                .CellValue(0, 4) = "Sub2"
                .CellValue(0, 5) = "Sub3"
                .CellValue(0, 6) = "Frac"
                .CellValue(0, 7) = "ActCd"
                .CellValue(0, 8) = "VName/Addr"
                .CellValue(0, 9) = "Sub1"
                .CellValue(0, 10) = "Sub2"
                .CellValue(0, 11) = "Sub3"
                .CellValue(0, 12) = "Frac"
                .CellValue(0, 13) = "ActCd"

                .FixedRows = 1
            End With

            atcgrid3.SizeAllColumnsToContents(atcgrid3.Width - pVScrollColumnOffset, True)
            atcgrid3.Refresh()

            'User Defn Quan

            atcgrid4.Source = New atcControls.atcGridSource
            With atcgrid4
                .Clear()
                .AllowHorizontalScrolling = False
                .AllowNewValidValues = True
                .Visible = True
            End With

            With atcgrid4.Source
                .CellValue(0, 0) = "UVQNam"
                .CellValue(0, 1) = "OpTyp"
                .CellValue(0, 2) = "OpNo"
                .CellValue(0, 3) = "VName/Addr"
                .CellValue(0, 4) = "Sub1"
                .CellValue(0, 5) = "Sub2"
                .CellValue(0, 6) = "Sub3"
                .CellValue(0, 7) = "Typ"
                .CellValue(0, 8) = "Mult"
                .CellValue(0, 9) = "LagCode"
                .CellValue(0, 10) = "LagStep"
                .CellValue(0, 11) = "AgCode"
                .CellValue(0, 12) = "AgStep"
                .CellValue(0, 13) = "Tran"

                .FixedRows = 1
            End With

            atcgrid4.SizeAllColumnsToContents(atcgrid4.Width - pVScrollColumnOffset, True)
            atcgrid4.Refresh()

            'Conditionals

            atcgrid5.Source = New atcControls.atcGridSource
            With atcgrid5
                .Clear()
                .AllowHorizontalScrolling = False
                .AllowNewValidValues = True
                .Visible = True
            End With

            With atcgrid5.Source
                .CellValue(0, 0) = "Text"

                .FixedRows = 1
            End With

            atcgrid5.Refresh()

            Display()
        End Set
    End Property

    Public Sub New(ByVal aHspfSpecialAction As Object, ByVal aParent As Windows.Forms.Form, ByVal aTag As String)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Data = aHspfSpecialAction
    End Sub

    Public Sub tabSpecial_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tabSpecial.SelectedIndexChanged
        Dim lRow As Integer
        Dim newText As String

        If tabSpecial.SelectedIndex <> PreviousTab And PreviousTab <> 0 Then
            'changed tab, put previous tab recs back to first tab
            PutRecsToFrontTab(PreviousTab)
            atcgrid0.Refresh()
            atcgrid0.SizeAllColumnsToContents(atcgrid0.Width - pVScrollColumnOffset, True)
        End If
        If tabSpecial.SelectedIndex <> PreviousTab Then
            'now load records for this tab
            If tabSpecial.SelectedIndex = 1 Then
                'action type records
                With atcgrid1.Source
                    .Columns = 21
                    .Rows = 1
                    For lRow = 1 To atcgrid0.Source.Rows
                        If atcgrid0.Source.CellValue(lRow, 0) = "Action" Then
                            .Rows = .Rows + 1
                            newText = Mid(atcgrid0.Source.CellValue(lRow, 1), 3)
                            .CellValue(.Rows - 1, 0) = Trim(Mid(newText, 1, 6))
                            .CellValue(.Rows - 1, 1) = Mid(newText, 7, 3)
                            If Mid(newText, 10, 4) = "    " Then
                                .CellValue(.Rows - 1, 2) = 0
                            Else
                                .CellValue(.Rows - 1, 2) = Mid(newText, 10, 4)
                            End If
                            .CellValue(.Rows - 1, 3) = Mid(newText, 14, 2)
                            If Mid(newText, 16, 3) = "   " Then
                                .CellValue(.Rows - 1, 4) = 0
                            Else
                                .CellValue(.Rows - 1, 4) = Mid(newText, 16, 3)
                            End If
                            If Mid(newText, 19, 4) = "    " Then
                                .CellValue(.Rows - 1, 5) = 0
                            Else
                                .CellValue(.Rows - 1, 5) = Mid(newText, 19, 4)
                            End If
                            If Mid(newText, 23, 3) = "   " Then
                                .CellValue(.Rows - 1, 6) = 0
                            Else
                                .CellValue(.Rows - 1, 6) = Mid(newText, 23, 3)
                            End If
                            If Mid(newText, 26, 3) = "   " Then
                                .CellValue(.Rows - 1, 7) = 0
                            Else
                                .CellValue(.Rows - 1, 7) = Mid(newText, 26, 3)
                            End If
                            If Mid(newText, 29, 3) = "   " Then
                                .CellValue(.Rows - 1, 8) = 0
                            Else
                                .CellValue(.Rows - 1, 8) = Mid(newText, 29, 3)
                            End If
                            If Mid(newText, 32, 3) = "   " Then
                                .CellValue(.Rows - 1, 9) = 0
                            Else
                                .CellValue(.Rows - 1, 9) = Mid(newText, 32, 3)
                            End If
                            If Mid(newText, 35, 2) = "  " Then
                                .CellValue(.Rows - 1, 10) = 0
                            Else
                                .CellValue(.Rows - 1, 10) = Mid(newText, 35, 2)
                            End If
                            .CellValue(.Rows - 1, 11) = Mid(newText, 37, 2)
                            'determine if vname or addr
                            If IsNumeric(Mid(newText, 41, 8)) Then
                                .CellValue(.Rows - 1, 12) = Mid(newText, 41, 8) 'addr
                                .CellValue(.Rows - 1, 13) = ""
                                .CellValue(.Rows - 1, 14) = ""
                                .CellValue(.Rows - 1, 15) = ""
                            Else
                                .CellValue(.Rows - 1, 12) = Mid(newText, 41, 6)
                                .CellValue(.Rows - 1, 13) = Mid(newText, 47, 3)
                                .CellValue(.Rows - 1, 14) = Mid(newText, 50, 3)
                                .CellValue(.Rows - 1, 15) = Mid(newText, 53, 3)
                            End If
                            .CellValue(.Rows - 1, 16) = Mid(newText, 56, 3)
                            'determine if value or uvquan
                            If IsNumeric(Mid(newText, 59, 10)) Then
                                .CellValue(.Rows - 1, 17) = Trim(Mid(newText, 59, 10)) 'value
                            Else
                                .CellValue(.Rows - 1, 17) = Mid(newText, 63, 6) 'quan
                            End If
                            .CellValue(.Rows - 1, 18) = Mid(newText, 70, 2)
                            If Len(Trim(Mid(newText, 73, 3))) = 0 Or Len(newText) < 73 Then
                                .CellValue(.Rows - 1, 19) = 0
                            Else
                                .CellValue(.Rows - 1, 19) = Mid(newText, 73, 3)
                            End If
                            If Mid(newText, 76, 3) = "   " Or Len(newText) < 76 Then
                                .CellValue(.Rows - 1, 20) = 0
                            Else
                                .CellValue(.Rows - 1, 20) = Mid(newText, 76, 3)
                            End If
                        End If
                    Next

                    For j As Integer = 0 To .Columns - 1
                        For k As Integer = 1 To .Rows - 1
                            .CellEditable(k, j) = True
                        Next
                    Next

                    atcgrid1.SizeAllColumnsToContents(atcgrid1.Width - pVScrollColumnOffset, True)
                    atcgrid1.Refresh()
                End With
            ElseIf tabSpecial.SelectedIndex = 2 Then
                'distributes
                With atcgrid2.Source
                    .Columns = 15
                    .Rows = 1
                    For lRow = 1 To atcgrid0.Source.Rows
                        If atcgrid0.Source.CellValue(lRow, 0) = "Distribute" Then
                            .Rows = .Rows + 1
                            newText = Mid(atcgrid0.Source.CellValue(lRow, 1), 3)
                            .CellValue(.Rows - 1, 0) = Mid(newText, 7, 3)
                            .CellValue(.Rows - 1, 1) = Mid(newText, 11, 3)
                            .CellValue(.Rows - 1, 2) = Mid(newText, 15, 2)
                            .CellValue(.Rows - 1, 3) = Mid(newText, 18, 3)
                            .CellValue(.Rows - 1, 4) = Mid(newText, 22, 5)
                            .CellValue(.Rows - 1, 5) = Mid(newText, 29, 5)
                            .CellValue(.Rows - 1, 6) = Mid(newText, 34, 5)
                            .CellValue(.Rows - 1, 7) = Mid(newText, 39, 5)
                            .CellValue(.Rows - 1, 8) = Mid(newText, 44, 5)
                            .CellValue(.Rows - 1, 9) = Mid(newText, 49, 5)
                            .CellValue(.Rows - 1, 10) = Mid(newText, 54, 5)
                            .CellValue(.Rows - 1, 11) = Mid(newText, 59, 5)
                            .CellValue(.Rows - 1, 12) = Mid(newText, 64, 5)
                            .CellValue(.Rows - 1, 13) = Mid(newText, 69, 5)
                            .CellValue(.Rows - 1, 14) = Mid(newText, 74, 5)
                        End If
                    Next
                    For j As Integer = 0 To .Columns - 1
                        For k As Integer = 1 To .Rows - 1
                            .CellEditable(k, j) = True
                        Next
                    Next
                    atcgrid2.SizeAllColumnsToContents(atcgrid2.Width - pVScrollColumnOffset, True)
                    atcgrid2.Refresh()
                End With
            ElseIf tabSpecial.SelectedIndex = 3 Then
                'uvname
                With atcgrid3.Source
                    .Columns = 14
                    .Rows = 1
                    For lRow = 1 To atcgrid0.Source.Rows
                        If atcgrid0.Source.CellValue(lRow, 0) = "User Defn Name" Then
                            .Rows = .Rows + 1
                            newText = Mid(atcgrid0.Source.CellValue(lRow, 1), 3)
                            .CellValue(.Rows - 1, 0) = Mid(newText, 9, 6)
                            .CellValue(.Rows - 1, 1) = Mid(newText, 15, 3)
                            .CellValue(.Rows - 1, 2) = Mid(newText, 19, 6)
                            .CellValue(.Rows - 1, 3) = Mid(newText, 25, 3)
                            .CellValue(.Rows - 1, 4) = Mid(newText, 28, 3)
                            .CellValue(.Rows - 1, 5) = Mid(newText, 31, 3)
                            .CellValue(.Rows - 1, 6) = Mid(newText, 35, 5)
                            .CellValue(.Rows - 1, 7) = Mid(newText, 41, 4)
                            .CellValue(.Rows - 1, 8) = Mid(newText, 49, 6)
                            .CellValue(.Rows - 1, 9) = Mid(newText, 55, 3)
                            .CellValue(.Rows - 1, 10) = Mid(newText, 58, 3)
                            .CellValue(.Rows - 1, 11) = Mid(newText, 61, 3)
                            If Mid(newText, 65, 5) = "     " Then
                                .CellValue(.Rows - 1, 12) = 1
                            Else
                                .CellValue(.Rows - 1, 12) = Mid(newText, 65, 5)
                            End If
                            .CellValue(.Rows - 1, 13) = Mid(newText, 71, 4)
                        End If
                    Next
                    For j As Integer = 0 To .Columns - 1
                        For k As Integer = 1 To .Rows - 1
                            .CellEditable(k, j) = True
                        Next
                    Next
                    atcgrid3.SizeAllColumnsToContents(atcgrid3.Width - pVScrollColumnOffset, True)
                    atcgrid3.Refresh()
                End With
            ElseIf tabSpecial.SelectedIndex = 4 Then
                'User Defn Quan
                With atcgrid4.Source
                    .Columns = 14
                    .Rows = 1
                    For lRow = 1 To atcgrid0.Source.Rows
                        If atcgrid0.Source.CellValue(lRow, 0) = "User Defn Quan" Then
                            .Rows = .Rows + 1
                            newText = Mid(atcgrid0.Source.CellValue(lRow, 1), 3)
                            .CellValue(.Rows - 1, 0) = Mid(newText, 8, 6)
                            .CellValue(.Rows - 1, 1) = Mid(newText, 15, 6)
                            .CellValue(.Rows - 1, 2) = Mid(newText, 22, 3)
                            .CellValue(.Rows - 1, 3) = Mid(newText, 26, 6)
                            .CellValue(.Rows - 1, 4) = Mid(newText, 33, 3)
                            .CellValue(.Rows - 1, 5) = Mid(newText, 36, 3)
                            .CellValue(.Rows - 1, 6) = Mid(newText, 39, 3)
                            .CellValue(.Rows - 1, 7) = Mid(newText, 41, 3)
                            If Mid(newText, 44, 10) = "          " Then
                                .CellValue(.Rows - 1, 8) = 1.0#
                            Else
                                .CellValue(.Rows - 1, 8) = Mid(newText, 44, 10)
                            End If
                            .CellValue(.Rows - 1, 9) = Mid(newText, 55, 2)
                            If Mid(newText, 57, 3) = "   " Then
                                .CellValue(.Rows - 1, 10) = 1
                            Else
                                .CellValue(.Rows - 1, 10) = Mid(newText, 57, 3)
                            End If
                            .CellValue(.Rows - 1, 11) = Mid(newText, 61, 2)
                            If Mid(newText, 63, 3) = "   " Then
                                .CellValue(.Rows - 1, 12) = 1
                            Else
                                .CellValue(.Rows - 1, 12) = Mid(newText, 63, 3)
                            End If
                            .CellValue(.Rows - 1, 13) = Mid(newText, 67, 4)
                        End If
                    Next
                    For j As Integer = 0 To .Columns - 1
                        For k As Integer = 1 To .Rows - 1
                            .CellEditable(k, j) = True
                        Next
                    Next
                    atcgrid4.SizeAllColumnsToContents(atcgrid4.Width - pVScrollColumnOffset, True)
                    atcgrid4.Refresh()
                End With
            ElseIf tabSpecial.SelectedIndex = 5 Then
                'conditionals
                With atcgrid5.Source
                    .Columns = 1
                    .Rows = 1
                    For lRow = 1 To atcgrid0.Source.Rows
                        If atcgrid0.Source.CellValue(lRow, 0) = "Condition" Then
                            .Rows = .Rows + 1
                            .CellValue(.Rows - 1, 0) = atcgrid0.Source.CellValue(lRow, 1)
                        End If
                    Next
                    For j As Integer = 0 To .Columns - 1
                        For k As Integer = 1 To .Rows - 1
                            .CellEditable(k, j) = True
                        Next
                    Next
                    atcgrid5.SizeAllColumnsToContents(atcgrid5.Width - pVScrollColumnOffset, True)
                    atcgrid5.Refresh()
                End With
            End If
        End If
        PreviousTab = tabSpecial.SelectedIndex
    End Sub

    Public Sub Save() Implements ctlEdit.Save
        Dim mySpecialRecord As HspfSpecialRecord
        Dim lOper As Integer

        PutRecsToFrontTab(tabSpecial.SelectedIndex)
        atcgrid0.Refresh()

        With pSpecialActionBlk.Records
            Do Until .Count = 0
                .RemoveAt(1)
            Loop
            For lOper = 1 To atcgrid0.Source.Rows - 1
                mySpecialRecord = New HspfSpecialRecord
                mySpecialRecord.Text = atcgrid0.Source.CellValue(lOper, 1)
                If atcgrid0.Source.CellValue(lOper, 0) = "Comment" Then
                    mySpecialRecord.SpecType = 6
                ElseIf atcgrid0.Source.CellValue(lOper, 0) = "Condition" Then
                    mySpecialRecord.SpecType = 5
                ElseIf atcgrid0.Source.CellValue(lOper, 0) = "Distribute" Then
                    mySpecialRecord.SpecType = 2
                ElseIf atcgrid0.Source.CellValue(lOper, 0) = "User Defn Name" Then
                    mySpecialRecord.SpecType = 3
                ElseIf atcgrid0.Source.CellValue(lOper, 0) = "User Defn Quan" Then
                    mySpecialRecord.SpecType = 4
                Else
                    mySpecialRecord.SpecType = 1
                End If
                pSpecialActionBlk.Records.Add(mySpecialRecord)
            Next
        End With

    End Sub

    Public Function UVNameInUse(ByVal Name$) As Boolean
        Dim ctmp As String
        Dim lIndex As Integer

        UVNameInUse = False
        'check front tab
        For lIndex = 1 To atcgrid0.Source.Rows
            If atcgrid0.Source.CellValue(lIndex, 0) = "User Defn Name" Then
                ctmp = atcgrid0.Source.CellValue(lIndex, 1)
                If Trim(Mid(ctmp, 11, 6)) = Name Then
                    UVNameInUse = True
                End If
            End If
        Next lIndex
        'check uvname tab as well
        For lIndex = 1 To atcgrid3.Source.Rows
            ctmp = atcgrid3.Source.CellValue(lIndex, 0)
            If Trim(ctmp) = Name Then
                UVNameInUse = True
            End If
        Next
    End Function

    Public Function NextDistribNumber() As Long
        Dim ctmp As String
        Dim lIndex As Integer
        Dim ifound As Boolean

        NextDistribNumber = 1

        ifound = True
        Do Until ifound = False
            ifound = False
            'check front tab
            For lIndex = 1 To atcgrid0.Source.Rows
                If atcgrid0.Source.CellValue(lIndex, 0) = "Distrib" Then
                    ctmp = atcgrid0.Source.CellValue(lIndex, 1)
                    If CInt(Mid(ctmp, 9, 3)) = NextDistribNumber Then
                        ifound = True
                        Exit For
                    End If
                End If
            Next lIndex
            If Not ifound Then
                'check distrib tab as well
                For lIndex = 1 To atcgrid2.Source.Rows
                    ctmp = atcgrid2.Source.CellValue(lIndex, 0)
                    If IsNumeric(ctmp) Then
                        If CInt(ctmp) = NextDistribNumber Then
                            ifound = True
                            Exit For
                        End If
                    End If
                Next
            End If
            If ifound Then
                NextDistribNumber = NextDistribNumber + 1
            End If
        Loop

    End Function

    Public Function UVQuanInUse(ByVal Name$, ByVal Id&) As String
        Dim ctmp As String
        Dim lIndex As Integer

        UVQuanInUse = ""
        'check front tab
        For lIndex = 1 To atcgrid0.Source.Rows
            If atcgrid0.Source.CellValue(lIndex, 0) = "User Defn Quan" Then
                ctmp = atcgrid0.Source.CellValue(lIndex, 1)
                If Trim(Mid(ctmp, 17, 6)) = Name And CInt(Mid(ctmp, 24, 3)) = Id Then
                    UVQuanInUse = Mid(ctmp, 10, 6)
                End If
            End If
        Next
        'check uvquan tab as well
        For lIndex = 1 To atcgrid4.Source.Rows
            ctmp = atcgrid4.Source.CellValue(lIndex, 1)
            If Trim(ctmp) = Name Then
                ctmp = atcgrid4.Source.CellValue(lIndex, 2)
                If IsNumeric(ctmp) Then
                    If CInt(ctmp) = Id Then
                        UVQuanInUse = Mid(ctmp, 10, 6)
                    End If
                End If
            End If
        Next
    End Function

    Public Function NextUVQuanName(ByVal firstfour$) As String
        Dim lctmp As String
        Dim lIndex As Integer
        Dim lnextnumber As Integer
        Dim lfound As Boolean

        lnextnumber = 1
        NextUVQuanName = ""
        lfound = True
        Do Until lfound = False
            lfound = False
            NextUVQuanName = firstfour & CStr(lnextnumber)
            'check front tab
            For lIndex = 1 To atcgrid0.Source.Rows
                If atcgrid0.Source.CellValue(lIndex, 0) = "User Defn Quan" Then
                    lctmp = atcgrid0.Source.CellValue(lIndex, 1)
                    If Trim(Mid(lctmp, 10, 6)) = NextUVQuanName Then
                        lfound = True
                        Exit For
                    End If
                End If
            Next
            If Not lfound Then
                'check user defn quan tab as well
                For lIndex = 1 To atcgrid4.Source.Rows
                    lctmp = atcgrid4.Source.CellValue(lIndex, 1)
                    If Trim(lctmp) = NextUVQuanName Then
                        lfound = True
                        Exit For
                    End If
                Next
            End If
            If lfound Then
                lnextnumber = lnextnumber + 1
            End If
        Loop

    End Function

    Public Sub AddToBeginning(ByVal cbuff$, ByVal itype&)

        With atcgrid0.Source
            If (.Rows = 1 And Len(.CellValue(1, 0)) > 0) Or .Rows > 1 Then
                .Rows += 1
                'shifts table down one row starting from the bottom
                For lRow As Integer = 1 To .Rows - 1
                    .CellValue(.Rows - lRow, 0) = .CellValue(.Rows - lRow - 1, 0)
                    .CellValue(.Rows - lRow, 1) = .CellValue(.Rows - lRow - 1, 1)
                Next
            End If
            .CellValue(1, 0) = pSpecialActionBlk.HspfSpecialRecordName(itype)
            .CellValue(1, 1) = cbuff
            Changed = True
        End With
        atcgrid0.SizeAllColumnsToContents(atcgrid0.Width - pVScrollColumnOffset, True)
        atcgrid0.Refresh()
    End Sub

    Public Sub AddToEnd(ByVal cbuff$, ByVal itype&)
        With atcgrid0.Source
            .CellValue(.Rows, 0) = pSpecialActionBlk.HspfSpecialRecordName(itype)
            .CellValue(.Rows - 1, 1) = cbuff
            Changed = True
        End With
        atcgrid0.SizeAllColumnsToContents(atcgrid0.Width - pVScrollColumnOffset, True)
        atcgrid0.Refresh()
    End Sub

    Private Sub cmdAgPrac_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAgPrac.Click

        If IsNothing(pfrmAgPrac) Then
            pfrmAgPrac = New frmAgPrac
            pfrmAgPrac.Init(pSpecialActionBlk.Uci, Me)
            pfrmAgPrac.Show()

        Else
            If pfrmAgPrac.IsDisposed Then
                pfrmAgPrac = New frmAgPrac
                pfrmAgPrac.Init(pSpecialActionBlk.Uci, Me)
                pfrmAgPrac.Show()
            Else
                pfrmAgPrac.WindowState = FormWindowState.Normal
                pfrmAgPrac.BringToFront()
            End If
        End If
        
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        MsgBox(tabSpecial.Width)
    End Sub

    Public Sub DoLimitsSpecialActions(ByVal aTypeIndex As Integer, ByVal aSelectedColumn As Integer, ByVal aSelectedRow As Integer)
        Dim vOpnBlk As Object, lopnblk As HspfOpnBlk

        If aTypeIndex = 0 Then
            If aSelectedColumn = 0 Then
                With atcgrid0
                    Dim lValidValues As New Collection
                    lValidValues.Add("Comment")
                    lValidValues.Add("Action")
                    lValidValues.Add("Distribute")
                    lValidValues.Add("User Defn Name")
                    lValidValues.Add("User Defn Quan")
                    lValidValues.Add("Condition")
                    .ValidValues = lValidValues
                    .AllowNewValidValues = False
                    .Refresh()
                    atcgrid0.Source.CellEditable(aSelectedRow, aSelectedColumn) = True
                End With
            End If
        ElseIf aTypeIndex = 1 Then
            'action type record
            With atcgrid1
                Dim lValidValues As New Collection
                If aSelectedColumn = 0 Then
                    'valid operation types
                    For Each vOpnBlk In pSpecialActionBlk.Uci.OpnBlks
                        lopnblk = vOpnBlk
                        If lopnblk.Count > 0 Then
                            If lopnblk.Name = "PERLND" Or lopnblk.Name = "IMPLND" Or _
                               lopnblk.Name = "RCHRES" Or lopnblk.Name = "PLTGEN" Or _
                               lopnblk.Name = "COPY" Or lopnblk.Name = "GENER" Then
                                lValidValues.Add(lopnblk.Name)
                            End If
                        End If
                    Next vOpnBlk
                ElseIf aSelectedColumn = 1 Then
                    SetOperationMinMax(lValidValues, pSpecialActionBlk.Uci, .Source.CellValue(aSelectedRow, aSelectedColumn - 1))
                ElseIf aSelectedColumn = 2 Then
                    SetOperationMinMax(lValidValues, pSpecialActionBlk.Uci, .Source.CellValue(aSelectedRow, aSelectedColumn - 2))
                ElseIf aSelectedColumn = 3 Then
                    lValidValues.Add("MI")
                    lValidValues.Add("HR")
                    lValidValues.Add("DY")
                    lValidValues.Add("MO")
                    lValidValues.Add("YR")
                ElseIf aSelectedColumn = 18 Then
                    lValidValues.Add("MI")
                    lValidValues.Add("HR")
                    lValidValues.Add("DY")
                    lValidValues.Add("MO")
                    lValidValues.Add("YR")
                End If
                .ValidValues = lValidValues
                .AllowNewValidValues = False
                .Refresh()
            End With
        ElseIf aTypeIndex = 2 Then
            'distributes
            With atcgrid2
                Dim lValidValues As New Collection
                If aSelectedColumn = 2 Then
                    lValidValues.Add("MI")
                    lValidValues.Add("HR")
                    lValidValues.Add("DY")
                    lValidValues.Add("MO")
                    lValidValues.Add("YR")
                ElseIf aSelectedColumn = 4 Then
                    lValidValues.Add("SKIP")
                    lValidValues.Add("SHIFT")
                    lValidValues.Add("ACCUM")
                End If
                .ValidValues = lValidValues
                .AllowNewValidValues = False
                .Refresh()
            End With
        ElseIf aTypeIndex = 3 Then
            'uvname
            With atcgrid3
                Dim lValidValues As New Collection
                If aSelectedColumn = 7 Then
                    lValidValues.Add("QUAN")
                    lValidValues.Add("MOVT")
                    lValidValues.Add("MOV1")
                    lValidValues.Add("MOV2")
                ElseIf aSelectedColumn = 13 Then
                    lValidValues.Add("")
                    lValidValues.Add("QUAN")
                    lValidValues.Add("MOVT")
                    lValidValues.Add("MOV1")
                    lValidValues.Add("MOV2")
                End If
                .ValidValues = lValidValues
                .AllowNewValidValues = False
                .Refresh()
            End With
        ElseIf aTypeIndex = 4 Then
            'User Defn Quan
            With atcgrid4
                Dim lValidValues As New Collection
                If aSelectedColumn = 1 Then
                    'valid operation types
                    For Each vOpnBlk In pSpecialActionBlk.Uci.OpnBlks
                        lopnblk = vOpnBlk
                        If lopnblk.Count > 0 Then
                            If lopnblk.Name = "PERLND" Or lopnblk.Name = "IMPLND" Or _
                               lopnblk.Name = "RCHRES" Or lopnblk.Name = "PLTGEN" Or _
                               lopnblk.Name = "COPY" Or lopnblk.Name = "GENER" Then
                                lValidValues.Add(lopnblk.Name)
                            End If
                        End If
                    Next vOpnBlk
                ElseIf aSelectedColumn = 2 Then
                    SetOperationMinMax(lValidValues, pSpecialActionBlk.Uci, .Source.CellValue(aSelectedRow, aSelectedColumn - 1))
                ElseIf aSelectedColumn = 9 Then
                    lValidValues.Add("MI")
                    lValidValues.Add("HR")
                    lValidValues.Add("DY")
                    lValidValues.Add("MO")
                    lValidValues.Add("YR")
                ElseIf aSelectedColumn = 11 Then
                    lValidValues.Add("MI")
                    lValidValues.Add("HR")
                    lValidValues.Add("DY")
                    lValidValues.Add("MO")
                    lValidValues.Add("YR")
                ElseIf aSelectedColumn = 13 Then
                    lValidValues.Add("SUM")
                    lValidValues.Add("AVER")
                    lValidValues.Add("MAX")
                    lValidValues.Add("MIN")
                End If
                .ValidValues = lValidValues
                .AllowNewValidValues = False
                .Refresh()
            End With
        ElseIf aTypeIndex = 5 Then
            'conditional
        End If
    End Sub

    Private Sub atcgrid0_MouseDownCell(ByVal aGrid As atcControls.atcGrid, ByVal aRow As Integer, ByVal aColumn As Integer) Handles atcgrid0.MouseDownCell
        pCurrentSelectedRow = aRow
        DoLimitsSpecialActions(0, aColumn, aRow)
    End Sub

    Private Sub atcgrid1_MouseDownCell(ByVal aGrid As atcControls.atcGrid, ByVal aRow As Integer, ByVal aColumn As Integer) Handles atcgrid1.MouseDownCell
        pCurrentSelectedRow = aRow
        DoLimitsSpecialActions(1, aColumn, aRow)
    End Sub

    Private Sub atcgrid2_MouseDownCell(ByVal aGrid As atcControls.atcGrid, ByVal aRow As Integer, ByVal aColumn As Integer) Handles atcgrid2.MouseDownCell
        pCurrentSelectedRow = aRow
        DoLimitsSpecialActions(2, aColumn, aRow)
    End Sub

    Private Sub atcgrid3_MouseDownCell(ByVal aGrid As atcControls.atcGrid, ByVal aRow As Integer, ByVal aColumn As Integer) Handles atcgrid3.MouseDownCell
        pCurrentSelectedRow = aRow
        DoLimitsSpecialActions(3, aColumn, aRow)
    End Sub

    Private Sub atcgrid4_MouseDownCell(ByVal aGrid As atcControls.atcGrid, ByVal aRow As Integer, ByVal aColumn As Integer) Handles atcgrid4.MouseDownCell
        pCurrentSelectedRow = aRow
        DoLimitsSpecialActions(4, aColumn, aRow)
    End Sub

    Private Sub atcgrid1_CellEdited(ByVal aGrid As atcControls.atcGrid, ByVal aRow As Integer, ByVal aColumn As Integer) Handles atcgrid1.CellEdited
        
        Dim lMinValue As Integer = -999
        Dim lMaxValue As Integer = -999
        If aColumn = 4 Then
            lMinValue = 0
        ElseIf aColumn = 5 Then
            lMinValue = 0
        ElseIf aColumn = 6 Then
            lMinValue = 0
        ElseIf aColumn = 7 Then
            lMinValue = 0
        ElseIf aColumn = 8 Then
            lMinValue = 0
        ElseIf aColumn = 9 Then
            lMinValue = 0
        ElseIf aColumn = 10 Then
            lMinValue = 0
        ElseIf aColumn = 11 Then
            lMaxValue = 4
            lMinValue = 2
        ElseIf aColumn = 19 Then
            lMinValue = 0
        ElseIf aColumn = 20 Then
            lMinValue = 0
        End If

        If lMaxValue <> -999 Or lMinValue <> -999 Then
            Dim lNewValue As String = aGrid.Source.CellValue(aRow, aColumn)
            Dim lNewValueNumeric As Double = -999
            If IsNumeric(lNewValue) Then lNewValueNumeric = CDbl(lNewValue)
            Dim lNewColor As Color = aGrid.Source.CellColor(aRow, aColumn)
            If ((lNewValueNumeric >= lMinValue And lMinValue <> -999) Or lMinValue = -999) AndAlso _
               ((lNewValueNumeric <= lMaxValue And lMaxValue <> -999) Or lMaxValue = -999) Then
                lNewColor = aGrid.CellBackColor
            Else
                lNewColor = Color.Pink
            End If
            If Not lNewColor.Equals(aGrid.Source.CellColor(aRow, aColumn)) Then
                aGrid.Source.CellColor(aRow, aColumn) = lNewColor
            End If
        End If
    End Sub

    Private Sub atcgrid2_CellEdited(ByVal aGrid As atcControls.atcGrid, ByVal aRow As Integer, ByVal aColumn As Integer) Handles atcgrid2.CellEdited
        Dim lMinValue As Integer = -999
        Dim lMaxValue As Integer = -999
        If aColumn = 0 Then
            lMinValue = 0
        ElseIf aColumn = 1 Then
            lMaxValue = 10
            lMinValue = 1
        ElseIf aColumn = 3 Then
            lMinValue = 0
        Else
            lMinValue = 0
        End If

        If lMaxValue <> -999 Or lMinValue <> -999 Then
            Dim lNewValue As String = aGrid.Source.CellValue(aRow, aColumn)
            Dim lNewValueNumeric As Double = -999
            If IsNumeric(lNewValue) Then lNewValueNumeric = CDbl(lNewValue)
            Dim lNewColor As Color = aGrid.Source.CellColor(aRow, aColumn)
            If ((lNewValueNumeric >= lMinValue And lMinValue <> -999) Or lMinValue = -999) AndAlso _
               ((lNewValueNumeric <= lMaxValue And lMaxValue <> -999) Or lMaxValue = -999) Then
                lNewColor = aGrid.CellBackColor
            Else
                lNewColor = Color.Pink
            End If
            If Not lNewColor.Equals(aGrid.Source.CellColor(aRow, aColumn)) Then
                aGrid.Source.CellColor(aRow, aColumn) = lNewColor
            End If
        End If
    End Sub

    Private Sub atcgrid3_CellEdited(ByVal aGrid As atcControls.atcGrid, ByVal aRow As Integer, ByVal aColumn As Integer) Handles atcgrid3.CellEdited
        Dim lMinValue As Integer = -999
        Dim lMaxValue As Integer = -999
        If aColumn = 1 Then
            lMinValue = 1
        End If

        If lMaxValue <> -999 Or lMinValue <> -999 Then
            Dim lNewValue As String = aGrid.Source.CellValue(aRow, aColumn)
            Dim lNewValueNumeric As Double = -999
            If IsNumeric(lNewValue) Then lNewValueNumeric = CDbl(lNewValue)
            Dim lNewColor As Color = aGrid.Source.CellColor(aRow, aColumn)
            If ((lNewValueNumeric >= lMinValue And lMinValue <> -999) Or lMinValue = -999) AndAlso _
               ((lNewValueNumeric <= lMaxValue And lMaxValue <> -999) Or lMaxValue = -999) Then
                lNewColor = aGrid.CellBackColor
            Else
                lNewColor = Color.Pink
            End If
            If Not lNewColor.Equals(aGrid.Source.CellColor(aRow, aColumn)) Then
                aGrid.Source.CellColor(aRow, aColumn) = lNewColor
            End If
        End If
    End Sub

    Private Sub atcgrid4_CellEdited(ByVal aGrid As atcControls.atcGrid, ByVal aRow As Integer, ByVal aColumn As Integer) Handles atcgrid4.CellEdited
        Dim lMinValue As Integer = -999
        Dim lMaxValue As Integer = -999
        If aColumn = 7 Then
            lMaxValue = 4
            lMinValue = 2
        ElseIf aColumn = 10 Then
            lMinValue = 0
        ElseIf aColumn = 12 Then
            lMinValue = 0
        End If

        If lMaxValue <> -999 Or lMinValue <> -999 Then
            Dim lNewValue As String = aGrid.Source.CellValue(aRow, aColumn)
            Dim lNewValueNumeric As Double = -999
            If IsNumeric(lNewValue) Then lNewValueNumeric = CDbl(lNewValue)
            Dim lNewColor As Color = aGrid.Source.CellColor(aRow, aColumn)
            If ((lNewValueNumeric >= lMinValue And lMinValue <> -999) Or lMinValue = -999) AndAlso _
               ((lNewValueNumeric <= lMaxValue And lMaxValue <> -999) Or lMaxValue = -999) Then
                lNewColor = aGrid.CellBackColor
            Else
                lNewColor = Color.Pink
            End If
            If Not lNewColor.Equals(aGrid.Source.CellColor(aRow, aColumn)) Then
                aGrid.Source.CellColor(aRow, aColumn) = lNewColor
            End If
        End If
    End Sub

End Class
