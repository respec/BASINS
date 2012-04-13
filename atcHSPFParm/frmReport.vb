Imports atcUtility
Imports MapWinUtility
Imports System.Data

Public Class frmReport

    Friend pParentObj As frmHSPFParm
    <CLSCompliant(False)> Public Database As atcUtility.atcMDB

    Friend Class ParmDetail
        Friend name As String
        Friend width As Integer
        Friend startcol As Integer
    End Class

    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Public Sub InitializeUI(ByVal aParentObj As frmHSPFParm, ByVal aDatabase As atcUtility.atcMDB)
        pParentObj = aParentObj
    End Sub

    Private Sub cmdAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAll.Click
        If pParentObj.agdSegment.Source.Rows > 0 Then
            Dim f As String = lblFile.Text
            If f = "<none>" Then
                Logger.Msg("The Report file must be set using the Set File button.", vbOKOnly, "Write Problem")
            Else
                'MousePointer = vbHourglass
                'can write either tables or parms
                'suggest writing tables since less space req
                For i As Integer = 1 To pParentObj.agdSegment.Source.Rows
                    pParentObj.agdSegment.Source.CellSelected(i, 1) = True
                Next i
                For i As Integer = 1 To pParentObj.agdTable.Source.Rows
                    Dim ctmp As String = pParentObj.agdTable.Source.CellValue(i, 0)
                    If ctmp <> "NQUALS" And Mid(ctmp, 1, 5) <> "QUAL-" Then 'kludge for performance
                        pParentObj.agdTable.Source.CellSelected(i, 0) = True
                        Dim Tid As Integer = pParentObj.pTableGridIDs(i)
                        Dim lTableName As String = ""
                        pParentObj.ViewTable(Tid, lTableName)
                        PrintSave()
                    End If
                Next i
                'clear main by selecting scenario again
                pParentObj.RefreshSegment()
                'MousePointer = vbDefault
            End If
        Else
            Logger.Msg("One scenario must be selected on the main form.", vbOKOnly, "Write Problem")
        End If
    End Sub

    Private Sub cmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        Me.Dispose()
    End Sub

    Private Sub cbxSelect_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxUCI.CheckedChanged
        
    End Sub

    Private Sub frmReport_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyValue = Windows.Forms.Keys.F1 Then
            ShowHelp("BASINS Details\Watershed and Instream Model Setup\HSPFParm.html")
        End If
    End Sub

    Private Sub cmdWrite_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdWrite.Click
        PrintSave()
    End Sub

    Sub PrintSave()
        Dim Min(23) As Single
        Dim Max(23) As Single
        Dim sum(23) As Single
        Dim lPDs As New atcCollection

        Dim f As String = lblFile.Text
        If pParentObj.agdValues.Source.Rows > 0 Then
            If f = "<none>" Then
                Logger.Msg("The Report file must be set using the Set File button.", vbOKOnly, "Write Problem")
            Else
                Dim ltxt As String = pParentObj.lblTableParmName.Text
                If Mid(ltxt, 5, 1) <> "T" Or (Mid(ltxt, 5, 1) = "T" And Not cbxUCI.Checked) Then
                    'output header if not doing uci tables
                    AppendFileString(f, ltxt)
                End If

                'is this by table or parameter?
                If Mid(ltxt, 5, 1) = "T" Then
                    'table
                    'get field widths
                    Dim TabNam As String = ""
                    Dim OpTypNam As String = ""
                    Dim OpTyp As Integer = 0
                    For i As Integer = 1 To pParentObj.agdTable.Source.Rows
                        If pParentObj.agdTable.Source.CellSelected(i, 0) Then
                            TabNam = pParentObj.agdTable.Source.CellValue(i, 0)
                            OpTypNam = pParentObj.agdTable.Source.CellValue(i, 1)
                            If OpTypNam = "PERLND" Then
                                OpTyp = 1
                            ElseIf OpTypNam = "IMPLND" Then
                                OpTyp = 2
                            ElseIf OpTypNam = "RCHRES" Then
                                OpTyp = 3
                            End If
                            Exit For
                        End If
                    Next i

                    If Not cbxUCI.Checked Then
                        ltxt = "  Op Type " & OpTypNam
                        AppendFileString(f, ltxt)
                        AppendFileString(f, "")
                    End If

                    Dim crit As String = "TabName = '" & TabNam & "' AND OpnTypID = " & CStr(OpTyp)

                    Dim lStr As String = "SELECT DISTINCTROW ParmTableList.Name, " & _
                                                            "ParmTableList.StartCol " & _
                                                            "ParmTableList.Width " & _
                                                            "From ParmTableList " & _
                                                            "WHERE (" & crit & ")"
                    Dim lTable As DataTable = Database.GetTable(lStr)
                    For lRow As Integer = 0 To lTable.Rows.Count - 1
                        Dim pd As New ParmDetail
                        pd.Name = lTable.Rows(lRow).Item(0).ToString()
                        pd.StartCol = lTable.Rows(lRow).Item(1).ToString()
                        pd.Width = lTable.Rows(lRow).Item(2).ToString()
                        lPDs.Add(pd)
                    Next
                    Dim lParmCount As Integer = lTable.Rows.Count

                    If Not cbxUCI.Checked Then
                        ltxt = "Op Num    Scenario  "
                        If pParentObj.agdTable.Source.Columns = lParmCount + 4 Then
                            'occur column is present 
                            ltxt = ltxt & pParentObj.agdTable.Source.CellValue(0, 2)
                            Dim k As Integer = Len(ltxt)
                            For i As Integer = k + 1 To 30
                                ltxt = ltxt & " "
                            Next i
                        End If
                        If pParentObj.agdTable.Source.Columns = lParmCount + 4 Then
                            'alias column is present 
                            ltxt = ltxt & pParentObj.agdTable.Source.CellValue(0, 3)
                            Dim k As Integer = Len(ltxt)
                            If k > 30 Then
                                For i As Integer = k + 1 To 40
                                    ltxt = ltxt & " "
                                Next i
                            Else
                                For i As Integer = k + 1 To 30
                                    ltxt = ltxt & " "
                                Next i
                            End If
                        End If
                        ltxt = ltxt & "Desc                "
                        For j As Integer = 3 To pParentObj.agdTable.Source.Columns
                            Dim k As Integer = Len(lPDs(j - 2).Name)
                            Dim w As Integer = lPDs(j - 2).Width
                            If w > k Then
                                ltxt = ltxt & lPDs(j - 2).Name
                                For n As Integer = 1 To (w - k)
                                    ltxt = ltxt & " "
                                Next n
                            ElseIf w <= k Then
                                ltxt = ltxt & Mid(lPDs(j - 2).Name, 1, w)
                            End If
                        Next j
                        AppendFileString(f, ltxt)
                        'initialize min, max, sum for each column
                        For j As Integer = 1 To pParentObj.agdTable.Source.Columns
                            Min(j) = 999999
                            Max(j) = -999999
                            sum(j) = 0.0#
                        Next j
                        'fill in values from table
                        For j As Integer = 1 To pParentObj.agdTable.Source.Rows
                            ltxt = Trim(pParentObj.agdTable.Source.CellValue(j, 0))
                            Dim curop As Integer = CInt(ltxt)
                            Dim curseg As String = OpTypNam & pParentObj.agdTable.Source.CellValue(j, 0)
                            Dim k As Integer = Len(ltxt)
                            For i As Integer = k To 9
                                ltxt = ltxt & " "
                            Next i
                            ltxt = ltxt & Mid(pParentObj.agdTable.Source.CellValue(j, 1), 1, 10)
                            k = Len(ltxt)
                            For i As Integer = k To 19
                                ltxt = ltxt & " "
                            Next i
                            If pParentObj.agdTable.Source.Columns = lParmCount + 4 Then
                                'occur column is present 
                                ltxt = ltxt & pParentObj.agdTable.Source.CellValue(j, 2)
                                k = Len(ltxt)
                                For i As Integer = k To 29
                                    ltxt = ltxt & " "
                                Next i
                            End If
                            If pParentObj.agdTable.Source.Columns = lParmCount + 4 Then
                                'alias column is present 
                                ltxt = ltxt & pParentObj.agdTable.Source.CellValue(j, 3)
                                k = Len(ltxt)
                                If k > 30 Then
                                    For i As Integer = k To 39
                                        ltxt = ltxt & " "
                                    Next i
                                Else
                                    For i As Integer = k To 29
                                        ltxt = ltxt & " "
                                    Next i
                                End If
                            End If
                            Dim desc As String = ""
                            For i As Integer = 1 To pParentObj.agdSegment.Source.Rows
                                If pParentObj.agdSegment.Source.CellValue(i, 0) = curseg Then
                                    desc = pParentObj.agdSegment.Source.CellValue(i, 1)  'add description
                                    Exit For
                                End If
                            Next i
                            ltxt = ltxt & desc
                            k = Len(desc)
                            If k < 20 Then
                                For i As Integer = k To 19
                                    ltxt = ltxt & " "
                                Next i
                            End If
                            For k = 5 To pParentObj.agdValues.Source.Columns
                                Dim n As Integer = Len(pParentObj.agdValues.Source.CellValue(j, k - 1))
                                ltxt = ltxt & pParentObj.agdValues.Source.CellValue(j, k - 1)
                                'keep sum, min, max
                                Dim rtemp As Single = Val(pParentObj.agdValues.Source.CellValue(j, k - 1))
                                sum(k) = sum(k) + rtemp
                                If rtemp > Max(k) Then
                                    Max(k) = rtemp
                                End If
                                If rtemp < Min(k) Then
                                    Min(k) = rtemp
                                End If
                                For i As Integer = n + 1 To lPDs(k - 4).Width
                                    ltxt = ltxt & " "
                                Next i
                            Next k
                            AppendFileString(f, ltxt)
                        Next j
                        AppendFileString(f, "")
                        'add lines for min, max, mean
                        ltxt = "Min                "
                        ltxt = ltxt & "                    "
                        If pParentObj.agdTable.Source.Columns = lParmCount + 4 Then
                            'occur column is present 
                            ltxt = ltxt & "          "
                        End If
                        If pParentObj.agdTable.Source.Columns = lParmCount + 4 Then
                            'alias column is present 
                            ltxt = ltxt & "          "
                        End If
                        For k As Integer = 5 To pParentObj.agdTable.Source.Columns
                            Dim txtval As String = Str(Min(k))
                            Dim n As Integer = Len(txtval)
                            ltxt = ltxt & txtval
                            For i As Integer = n + 1 To lPDs(k - 4).Width
                                ltxt = ltxt & " "
                            Next i
                        Next k
                        AppendFileString(f, ltxt)
                        'output max
                        ltxt = "Max                "
                        ltxt = ltxt & "                    "
                        If pParentObj.agdTable.Source.Columns = lParmCount + 4 Then
                            'occur column is present 
                            ltxt = ltxt & "          "
                        End If
                        If pParentObj.agdTable.Source.Columns = lParmCount + 4 Then
                            'alias column is present 
                            ltxt = ltxt & "          "
                        End If
                        For k As Integer = 5 To pParentObj.agdTable.Source.Columns
                            Dim txtval As String = Str(Max(k))
                            Dim n As Integer = Len(txtval)
                            ltxt = ltxt & txtval
                            For i As Integer = n + 1 To lPDs(k - 4).Width
                                ltxt = ltxt & " "
                            Next i
                        Next k
                        AppendFileString(f, ltxt)
                        ltxt = "Mean               "
                        ltxt = ltxt & "                    "
                        If pParentObj.agdTable.Source.Columns = lParmCount + 4 Then
                            'occur column is present 
                            ltxt = ltxt & "          "
                        End If
                        If pParentObj.agdTable.Source.Columns = lParmCount + 4 Then
                            'alias column is present 
                            ltxt = ltxt & "          "
                        End If
                        For k As Integer = 5 To pParentObj.agdValues.Source.Columns
                            Dim rtemp As Single = sum(k) / pParentObj.agdValues.Source.Rows
                            Dim wid As Integer = lPDs(k - 4).Width
                            Dim txtval As String = Str(rtemp)
                            Dim n As Integer = Len(txtval)
                            ltxt = ltxt & txtval
                            For i As Integer = n + 1 To lPDs(k - 4).Width
                                ltxt = ltxt & " "
                            Next i
                        Next k
                        AppendFileString(f, ltxt)
                        AppendFileString(f, "")
                        AppendFileString(f, "")
                    Else 'want table in uci form
                        AppendFileString(f, TabNam)
                        ltxt = OpTypNam & " ***"
                        For j As Integer = 1 To 80
                            ltxt = ltxt & " "
                        Next j
                        For j As Integer = 5 To pParentObj.agdValues.Source.Columns
                            Dim k As Integer = lPDs(j - 4).StartCol
                            Mid(ltxt, k) = lPDs(j - 4).Name
                        Next j
                        AppendFileString(f, ltxt)
                        'fill in each row
                        For j As Integer = 1 To pParentObj.agdValues.Source.Rows
                            ltxt = Trim(pParentObj.agdValues.Source.CellValue(j, 0))
                            For i As Integer = 1 To 80
                                ltxt = ltxt & " "
                            Next i
                            For k As Integer = 5 To pParentObj.agdValues.Source.Columns
                                Dim i As Integer = lPDs(k - 4).StartCol
                                Mid(ltxt, i) = Trim(pParentObj.agdValues.Source.CellValue(j, k - 1))
                            Next k
                            AppendFileString(f, ltxt)
                        Next j
                        AppendFileString(f, "END " & TabNam)
                        AppendFileString(f, "")
                        AppendFileString(f, "")
                    End If
                Else
                    'parameter
                    ltxt = "Name      Value     Segment             Scenario            Desc"
                    If pParentObj.agdValues.Source.Columns > 3 Then   'need to know if occur field is present
                        ltxt = ltxt & pParentObj.agdValues.Source.CellValue(0, 4)
                        Dim k As Integer = Len(ltxt)
                        For i As Integer = k + 1 To 60
                            ltxt = ltxt & " "
                        Next i
                    End If
                    If pParentObj.agdValues.Source.Columns > 4 Then   'need to know if alias field is present
                        ltxt = ltxt & pParentObj.agdValues.Source.CellValue(0, 5)
                        Dim k As Integer = Len(ltxt)
                        For i As Integer = k + 1 To 70
                            ltxt = ltxt & " "
                        Next i
                    End If
                    AppendFileString(f, "")
                    AppendFileString(f, ltxt)
                    For j As Integer = 1 To pParentObj.agdValues.Source.Rows
                        ltxt = ""
                        ltxt = ltxt & pParentObj.agdValues.Source.CellValue(j, 0)
                        Dim k As Integer = Len(ltxt)
                        For i As Integer = k + 1 To 10
                            ltxt = ltxt & " "
                        Next i
                        ltxt = ltxt & pParentObj.agdValues.Source.CellValue(j, 1)
                        k = Len(ltxt)
                        For i As Integer = k + 1 To 20
                            ltxt = ltxt & " "
                        Next i
                        ltxt = ltxt & pParentObj.agdValues.Source.CellValue(j, 2)
                        Dim curseg As String = pParentObj.lblTableParmName.Text
                        k = Len(ltxt)
                        For i As Integer = k + 1 To 40
                            ltxt = ltxt & " "
                        Next i
                        ltxt = ltxt & pParentObj.agdValues.Source.CellValue(j, 2)  'scenario
                        k = Len(ltxt)
                        For i As Integer = k + 1 To 60
                            ltxt = ltxt & " "
                        Next i
                        If Len(ltxt) > 60 Then
                            ltxt = Mid(ltxt, 1, 60)
                        End If
                        Dim desc As String = ""
                        For i As Integer = 1 To pParentObj.agdSegment.Source.Rows
                            If pParentObj.agdSegment.Source.CellValue(i, 0) = curseg Then
                                desc = pParentObj.agdSegment.Source.CellValue(i, 1)  'add description
                                Exit For
                            End If
                        Next i
                        ltxt = ltxt & desc
                        k = Len(desc)
                        If k < 20 Then
                            For i As Integer = k To 19
                                ltxt = ltxt & " "
                            Next i
                        End If
                        'optional fields
                        If pParentObj.agdValues.Source.Columns > 3 Then   'need to know if occur field is present
                            ltxt = ltxt & pParentObj.agdValues.Source.CellValue(j, 4)
                            k = Len(ltxt)
                            For i As Integer = k + 1 To 90
                                ltxt = ltxt & " "
                            Next i
                        End If
                        If pParentObj.agdValues.Source.Columns > 4 Then   'need to know if alias field is present
                            ltxt = ltxt & pParentObj.agdValues.Source.CellValue(j, 5)
                            k = Len(ltxt)
                            For i As Integer = k + 1 To 100
                                ltxt = ltxt & " "
                            Next i
                        End If
                        AppendFileString(f, ltxt)
                    Next j
                    AppendFileString(f, "")
                    AppendFileString(f, "")
                End If
            End If
        Else
            Logger.Msg("Some tables or parameters must be selected on the main form.", vbOKOnly, "Write Problem")
        End If
    End Sub

    Private Sub cmdSet_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSet.Click
        If cdSetOut.ShowDialog() = Windows.Forms.DialogResult.OK Then
            lblFile.Text = cdSetOut.FileName
        End If
    End Sub
End Class