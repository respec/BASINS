Imports atcUtility
Imports MapWinUtility
Imports System.Data

Public Class frmReport

    Friend pParentObj As frmHSPFParm
    <CLSCompliant(False)> Public Database As atcUtility.atcMDB

    Friend Class ParmDetail
        Public Name As String
        Public Width As Integer
        Public StartCol As Integer
        Public Min As Single
        Public Max As Single
        Public Sum As Single
    End Class

    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Public Sub InitializeUI(ByVal aParentObj As frmHSPFParm, ByVal aDatabase As atcUtility.atcMDB)
        pParentObj = aParentObj
        Database = aDatabase
    End Sub

    Private Sub cmdAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAll.Click
        If pParentObj.agdSegment.Source.Rows > 0 Then
            Dim f As String = lblFile.Text
            If f = "<none>" Then
                Logger.Msg("The Report file must be set using the Set File button.", vbOKOnly, "Write Problem")
            Else
                Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
                For lRow As Integer = 1 To pParentObj.agdSegment.Source.Rows - 1
                    For lCol As Integer = 0 To pParentObj.agdSegment.Source.Columns - 1
                        pParentObj.agdSegment.Source.CellSelected(lRow, lCol) = True
                    Next
                Next
                pParentObj.Refresh()
                pParentObj.RefreshTable()
                For i As Integer = 1 To pParentObj.agdTable.Source.Rows - 1
                    pParentObj.agdTable.Source.CellSelected(i, 0) = True
                    Dim Tid As Integer = pParentObj.pTableGridIDs(i - 1)
                    Dim lTableName As String = pParentObj.agdTable.Source.CellValue(i, 0)
                    pParentObj.ViewTable(Tid, lTableName)
                    pParentObj.Refresh()
                    PrintSave()
                    pParentObj.agdTable.Source.CellSelected(i, 0) = False
                Next i
                'clear main by selecting scenario again
                pParentObj.RefreshSegment()
                Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
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
        Dim lFileName As String = lblFile.Text
        If pParentObj.agdValues.Source.Rows > 0 Then
            If lFileName = "<none>" Then
                Logger.Msg("The Report file must be set using the Set File button.", vbOKOnly, "Write Problem")
            Else
                Dim lTxt As String = "    " & pParentObj.lblTableParmName.Text & vbCrLf
                If Mid(lTxt, 5, 1) <> "T" Or (Mid(lTxt, 5, 1) = "T" And Not cbxUCI.Checked) Then
                    'output header if not doing uci tables
                    AppendFileString(lFileName, lTxt)
                End If

                'is this by table or parameter?
                If Mid(lTxt, 5, 1) = "T" Then
                    'table
                    Dim lPDs As New atcCollection

                    'get field widths
                    Dim lTabNam As String = ""
                    Dim lOpTypNam As String = ""
                    Dim lOpTyp As Integer = 0
                    For lRow As Integer = 1 To pParentObj.agdTable.Source.Rows
                        If pParentObj.agdTable.Source.CellSelected(lRow, 0) Then
                            lTabNam = pParentObj.agdTable.Source.CellValue(lRow, 0)
                            lOpTypNam = pParentObj.agdTable.Source.CellValue(lRow, 1)
                            If lOpTypNam = "PERLND" Then
                                lOpTyp = 1
                            ElseIf lOpTypNam = "IMPLND" Then
                                lOpTyp = 2
                            ElseIf lOpTypNam = "RCHRES" Then
                                lOpTyp = 3
                            End If
                            Exit For
                        End If
                    Next lRow

                    If Not cbxUCI.Checked Then
                        lTxt = "  Op Type " & lOpTypNam & vbCrLf
                        AppendFileString(lFileName, lTxt)
                        AppendFileString(lFileName, vbCrLf)
                    End If

                    Dim lCrit As String = "TabName = '" & lTabNam & "' AND OpnTypID = " & CStr(lOpTyp)

                    Dim lStr As String = "SELECT DISTINCTROW ParmTableList.Name, " & _
                                                            "ParmTableList.StartCol, " & _
                                                            "ParmTableList.Width " & _
                                                            "From ParmTableList " & _
                                                            "WHERE (" & lCrit & ")"
                    Dim lTable As DataTable = Database.GetTable(lStr)
                    For lRow As Integer = 0 To lTable.Rows.Count - 1
                        Dim lPD As New ParmDetail
                        lPD.Name = lTable.Rows(lRow).Item(0).ToString()
                        lPD.StartCol = lTable.Rows(lRow).Item(1).ToString()
                        lPD.Width = lTable.Rows(lRow).Item(2).ToString()
                        lPD.Min = 999999
                        lPD.Max = -999999
                        lPD.Sum = 0.0#
                        lPDs.Add(lPD)
                    Next
                    Dim lParmCount As Integer = lTable.Rows.Count

                    If Not cbxUCI.Checked Then
                        lTxt = "Op Num    Scenario  "
                        Dim lExtraCols As Integer = 0
                        If pParentObj.agdValues.Source.Columns = lParmCount + 4 Then
                            'occur column is present 
                            lExtraCols += 1
                            lTxt = lTxt & pParentObj.agdValues.Source.CellValue(0, 2)
                            Dim k As Integer = Len(lTxt)
                            For i As Integer = k + 1 To 30
                                lTxt = lTxt & " "
                            Next i
                        End If
                        Dim lAliasLen As Integer = 0
                        If pParentObj.agdValues.Source.Columns > lParmCount + 2 Then
                            'alias column is present 
                            lExtraCols += 1
                            lTxt = lTxt & pParentObj.agdValues.Source.CellValue(0, 1 + lExtraCols)
                            Dim k As Integer = Len(lTxt)
                            If k > 30 Then
                                For i As Integer = k + 1 To 40
                                    lTxt = lTxt & " "
                                Next i
                            Else
                                For i As Integer = k + 1 To 30
                                    lTxt = lTxt & " "
                                Next i
                            End If
                            lAliasLen = Len(lTxt)
                        End If
                        lTxt = lTxt & "Desc                "
                        For lParm As Integer = 0 To lPDs.Count - 1
                            Dim k As Integer = Len(lPDs(lParm).Name)
                            Dim w As Integer = lPDs(lParm).Width
                            If w > k Then
                                lTxt = lTxt & lPDs(lParm).Name
                                For n As Integer = 1 To (w - k)
                                    lTxt = lTxt & " "
                                Next n
                            ElseIf w <= k Then
                                lTxt = lTxt & Mid(lPDs(lParm).Name, 1, w)
                            End If
                        Next lParm
                        AppendFileString(lFileName, lTxt & vbCrLf)
                        'fill in values from table
                        For lRow As Integer = 1 To pParentObj.agdValues.Source.Rows - 1
                            lTxt = Trim(pParentObj.agdValues.Source.CellValue(lRow, 0))
                            Dim lCurSeg As String = lOpTypNam & pParentObj.agdValues.Source.CellValue(lRow, 0)
                            Dim k As Integer = Len(lTxt)
                            For i As Integer = k To 9
                                lTxt = lTxt & " "
                            Next i
                            lTxt = lTxt & Mid(pParentObj.agdValues.Source.CellValue(lRow, 1), 1, 10)
                            k = Len(lTxt)
                            For i As Integer = k To 19
                                lTxt = lTxt & " "
                            Next i
                            If pParentObj.agdValues.Source.Columns = lParmCount + 4 Then
                                'occur column is present 
                                lTxt = lTxt & pParentObj.agdValues.Source.CellValue(lRow, 2)
                                k = Len(lTxt)
                                For i As Integer = k To 29
                                    lTxt = lTxt & " "
                                Next i
                            End If
                            If pParentObj.agdValues.Source.Columns > lParmCount + 2 Then
                                'alias column is present 
                                lTxt = lTxt & pParentObj.agdValues.Source.CellValue(lRow, 1 + lExtraCols)
                                k = Len(lTxt)
                                If k > 30 Then
                                    For i As Integer = k To 39
                                        lTxt = lTxt & " "
                                    Next i
                                Else
                                    For i As Integer = k To 29
                                        lTxt = lTxt & " "
                                    Next i
                                End If
                                If Len(lTxt) > lAliasLen Then
                                    lTxt = Mid(lTxt, 1, lAliasLen)
                                End If
                            End If
                            Dim lDesc As String = ""
                            For i As Integer = 1 To pParentObj.agdSegment.Source.Rows
                                If pParentObj.agdSegment.Source.CellValue(i, 0) = lCurSeg Then
                                    lDesc = pParentObj.agdSegment.Source.CellValue(i, 1)  'add description
                                    Exit For
                                End If
                            Next i
                            lTxt = lTxt & lDesc
                            k = Len(lDesc)
                            If k < 20 Then
                                For i As Integer = k To 19
                                    lTxt = lTxt & " "
                                Next i
                            End If
                            For k = 0 To lPDs.Count - 1
                                Dim n As Integer = Len(pParentObj.agdValues.Source.CellValue(lRow, k + lExtraCols + 2))
                                lTxt = lTxt & pParentObj.agdValues.Source.CellValue(lRow, k + lExtraCols + 2)
                                'keep sum, min, max
                                Dim lRTemp As Single = Val(pParentObj.agdValues.Source.CellValue(lRow, k + lExtraCols + 2))
                                lPDs(k).sum = lPDs(k).sum + lRTemp
                                If lRTemp > lPDs(k).Max Then
                                    lPDs(k).Max = lRTemp
                                End If
                                If lRTemp < lPDs(k).Min Then
                                    lPDs(k).Min = lRTemp
                                End If
                                For i As Integer = n + 1 To lPDs(k).Width
                                    lTxt = lTxt & " "
                                Next i
                            Next k
                            AppendFileString(lFileName, lTxt & vbCrLf)
                        Next lRow
                        AppendFileString(lFileName, vbCrLf)
                        'add lines for min, max, mean
                        lTxt = "Min                "
                        lTxt = lTxt & "                    "
                        If pParentObj.agdValues.Source.Columns = lParmCount + 4 Then
                            'occur column is present 
                            lTxt = lTxt & "          "
                        End If
                        If pParentObj.agdValues.Source.Columns > lParmCount + 2 Then
                            'alias column is present 
                            lTxt = lTxt & "          "
                        End If
                        For k As Integer = 0 To lPDs.Count - 1
                            Dim txtval As String = Str(lPDs(k).Min)
                            Dim n As Integer = Len(txtval)
                            lTxt = lTxt & txtval
                            For i As Integer = n + 1 To lPDs(k).Width
                                lTxt = lTxt & " "
                            Next i
                        Next k
                        AppendFileString(lFileName, lTxt & vbCrLf)
                        'output max
                        lTxt = "Max                "
                        lTxt = lTxt & "                    "
                        If pParentObj.agdValues.Source.Columns = lParmCount + 4 Then
                            'occur column is present 
                            lTxt = lTxt & "          "
                        End If
                        If pParentObj.agdValues.Source.Columns > lParmCount + 2 Then
                            'alias column is present 
                            lTxt = lTxt & "          "
                        End If
                        For k As Integer = 0 To lPDs.Count - 1
                            Dim txtval As String = Str(lPDs(k).Max)
                            Dim n As Integer = Len(txtval)
                            lTxt = lTxt & txtval
                            For i As Integer = n + 1 To lPDs(k).Width
                                lTxt = lTxt & " "
                            Next i
                        Next k
                        AppendFileString(lFileName, lTxt & vbCrLf)
                        lTxt = "Mean               "
                        lTxt = lTxt & "                    "
                        If pParentObj.agdValues.Source.Columns = lParmCount + 4 Then
                            'occur column is present 
                            lTxt = lTxt & "          "
                        End If
                        If pParentObj.agdValues.Source.Columns > lParmCount + 2 Then
                            'alias column is present 
                            lTxt = lTxt & "          "
                        End If
                        For k As Integer = 0 To lPDs.Count - 1
                            Dim rtemp As Single = lPDs(k).sum / (pParentObj.agdValues.Source.Rows - 1)
                            Dim wid As Integer = lPDs(k).Width
                            Dim txtval As String = Str(rtemp)
                            Dim n As Integer = Len(txtval)
                            lTxt = lTxt & txtval
                            For i As Integer = n + 1 To lPDs(k).Width
                                lTxt = lTxt & " "
                            Next i
                        Next k
                        AppendFileString(lFileName, lTxt & vbCrLf)
                        AppendFileString(lFileName, vbCrLf)
                        AppendFileString(lFileName, vbCrLf)
                    Else 'want table in uci form
                        AppendFileString(lFileName, lTabNam & vbCrLf)
                        lTxt = lOpTypNam & " ***"
                        For j As Integer = 1 To 80
                            lTxt = lTxt & " "
                        Next j
                        For j As Integer = 0 To lPDs.Count - 1
                            Dim k As Integer = lPDs(j).StartCol
                            Mid(lTxt, k) = lPDs(j).Name
                        Next j
                        AppendFileString(lFileName, lTxt & vbCrLf)
                        'figure out if the grid has the extra columns for occur and alias
                        Dim lExtraCols As Integer = 0
                        If pParentObj.agdValues.Source.Columns = lParmCount + 4 Then
                            'occur column is present 
                            lExtraCols += 1
                        End If
                        If pParentObj.agdValues.Source.Columns > lParmCount + 2 Then
                            'alias column is present 
                            lExtraCols += 1
                        End If
                        'fill in each row
                        For lRow As Integer = 1 To pParentObj.agdValues.Source.Rows - 1
                            lTxt = Trim(pParentObj.agdValues.Source.CellValue(lRow, 0))
                            For i As Integer = 1 To 80
                                lTxt = lTxt & " "
                            Next i
                            For k As Integer = 0 To lPDs.Count - 1
                                Dim i As Integer = lPDs(k).StartCol
                                Mid(lTxt, i) = Trim(pParentObj.agdValues.Source.CellValue(lRow, k + 2 + lExtraCols))
                            Next k
                            AppendFileString(lFileName, lTxt & vbCrLf)
                        Next lRow
                        AppendFileString(lFileName, "END " & lTabNam & vbCrLf)
                        AppendFileString(lFileName, vbCrLf)
                        AppendFileString(lFileName, vbCrLf)
                    End If
                Else
                    'parameter
                    lTxt = "Name      Value     Segment             Scenario            Desc"
                    If pParentObj.agdValues.Source.Columns > 4 Then   'need to know if occur field is present
                        Dim k As Integer = Len(lTxt)
                        For i As Integer = k + 1 To 80
                            lTxt = lTxt & " "
                        Next i
                        lTxt = lTxt & pParentObj.agdValues.Source.CellValue(0, 4)
                    End If
                    If pParentObj.agdValues.Source.Columns > 5 Then   'need to know if alias field is present
                        Dim k As Integer = Len(lTxt)
                        For i As Integer = k + 1 To 90
                            lTxt = lTxt & " "
                        Next i
                        lTxt = lTxt & pParentObj.agdValues.Source.CellValue(0, 5)
                    End If
                    AppendFileString(lFileName, vbCrLf)
                    AppendFileString(lFileName, lTxt & vbCrLf)
                    For lRow As Integer = 1 To pParentObj.agdValues.Source.Rows
                        lTxt = ""
                        lTxt = lTxt & pParentObj.agdValues.Source.CellValue(lRow, 0)
                        Dim k As Integer = Len(lTxt)
                        For i As Integer = k + 1 To 10
                            lTxt = lTxt & " "
                        Next i
                        lTxt = lTxt & pParentObj.agdValues.Source.CellValue(lRow, 1)
                        k = Len(lTxt)
                        For i As Integer = k + 1 To 20
                            lTxt = lTxt & " "
                        Next i
                        lTxt = lTxt & pParentObj.agdValues.Source.CellValue(lRow, 2)
                        Dim lCurSeg As String = pParentObj.agdValues.Source.CellValue(lRow, 2)
                        k = Len(lTxt)
                        For i As Integer = k + 1 To 40
                            lTxt = lTxt & " "
                        Next i
                        lTxt = lTxt & pParentObj.agdValues.Source.CellValue(lRow, 3)  'scenario
                        k = Len(lTxt)
                        For i As Integer = k + 1 To 60
                            lTxt = lTxt & " "
                        Next i
                        If Len(lTxt) > 60 Then
                            lTxt = Mid(lTxt, 1, 60)
                        End If
                        Dim lDesc As String = ""
                        For lSegRow As Integer = 1 To pParentObj.agdSegment.Source.Rows
                            If pParentObj.agdSegment.Source.CellValue(lSegRow, 0) = lCurSeg Then
                                lDesc = pParentObj.agdSegment.Source.CellValue(lSegRow, 1)  'add description
                                Exit For
                            End If
                        Next lSegRow
                        lTxt = lTxt & lDesc
                        k = Len(lDesc)
                        If k < 20 Then
                            For i As Integer = k To 19
                                lTxt = lTxt & " "
                            Next i
                        End If
                        'optional fields
                        If pParentObj.agdValues.Source.Columns > 4 Then   'need to know if occur field is present
                            lTxt = lTxt & pParentObj.agdValues.Source.CellValue(lRow, 4)
                            k = Len(lTxt)
                            For i As Integer = k + 1 To 90
                                lTxt = lTxt & " "
                            Next i
                        End If
                        If pParentObj.agdValues.Source.Columns > 5 Then   'need to know if alias field is present
                            lTxt = lTxt & pParentObj.agdValues.Source.CellValue(lRow, 5)
                            k = Len(lTxt)
                            For i As Integer = k + 1 To 100
                                lTxt = lTxt & " "
                            Next i
                        End If
                        AppendFileString(lFileName, lTxt & vbCrLf)
                    Next lRow
                    AppendFileString(lFileName, vbCrLf)
                    AppendFileString(lFileName, vbCrLf)
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