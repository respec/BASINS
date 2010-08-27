Imports System.Drawing
Imports MapWinUtility
Imports atcUCI
Imports atcUtility

'.net conversion issue:************************
'Check definition of pCtl 
'Check starter path - hardcoded for now!!!
'**********************************************


Public Class frmAgPrac

    Dim pUci As HspfUci
    Dim pCtl As ctlEditSpecialAction

    Dim cPracIDs As New Collection
    Dim cPracRecs As New Collection

    Private Sub Form_Load()

        Dim vOper As Object
        Dim lOper As HspfOperation
        Dim lFreeFile, pcount As Integer
        Dim lTempString, lFileLength, lTrimmedlStr As String
        Dim lCend As Boolean
        Dim lTname As String = Nothing
        Dim lStr As String = Nothing


        txtNA.Visible = False
        txtNA.Enabled = False
        GroupLayers.Visible = True
        GroupPar.Visible = True
        GroupProps.Visible = True
        'Height = 555

        comboRep.Items.Clear()
        comboRep.Items.Add("None")
        comboRep.Items.Add("YR")
        comboRep.Items.Add("MO")
        comboRep.Items.Add("DY")
        comboRep.Items.Add("HR")
        comboRep.Items.Add("MI")
        Try
            lFreeFile = FreeFile()

            Dim lBasinsFolder As String = My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\AQUA TERRA Consultants\BASINS", "Base Directory", "C:\Basins")
            lTname = lBasinsFolder & "\models\HSPF\bin\starter" & "\" & "agpractice.txt"
            FileOpen(lFreeFile, lTname, OpenMode.Input)
            pcount = 0

            Do Until EOF(lFreeFile)
                lStr = LineInput(lFreeFile)
                lFileLength = Len(lStr)
                If lFileLength > 6 Then
                    If Microsoft.VisualBasic.Left(lStr, 7) = "PRACTIC" Then
                        'found start of a practice
                        lTempString = StrRetRem(lStr)
                        lstPrac.Items.Add(lStr)
                        pcount = pcount + 1
                        lCend = False
                        Do While Not lCend
                            lStr = LineInput(lFreeFile)
                            lTrimmedlStr = Trim(lStr)
                            lFileLength = Len(lTrimmedlStr)
                            If Microsoft.VisualBasic.Left(lTrimmedlStr, lFileLength) = "END PRACTICE" Then
                                'found end of practice
                                lCend = True
                            Else
                                cPracIDs.Add(pcount)
                                cPracRecs.Add(lStr)
                            End If
                        Loop
                    End If
                End If
            Loop
            FileClose()
            'formerly goto FillLists
            For Each vOper In pUci.OpnSeqBlock.Opns
                lOper = vOper
                If lOper.Name = "PERLND" Then
                    lstSeg.Items.Add(lOper.Name & " " & lOper.Id & " (" & lOper.Description & ")")
                End If
            Next

            atxYear.HardMax = pUci.GlobalBlock.EDate(0)
            atxYear.HardMin = pUci.GlobalBlock.SDate(0)

            lstPrac.SetSelected(0, True)
            lstSeg.SetSelected(0, True)
            ShowDetails()
        Catch e As Exception
            If Err.Number = 53 Then
                MsgBox("File " & lTname & " not found.", vbOKOnly, "Read Ag Practices Problem")
            Else
                MsgBox(Err.Description & vbCrLf & vbCrLf & lStr, vbOKOnly, "Read Ag Practices Problem")
            End If
        End Try

        

    End Sub

    Private Sub Form_Unload(ByVal Cancel As Integer)
        Do Until cPracIDs.Count = 0
            cPracIDs.Remove(1)
        Loop
        Do Until cPracRecs.Count = 0
            cPracRecs.Remove(1)
        Loop
    End Sub

    Private Sub frmAgPrac_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Form_Load()
    End Sub

    Friend Sub Init(ByVal aUci As HspfUci, ByVal aCtl As ctlEditSpecialAction)
        pUci = aUci
        pCtl = aCtl
        Me.Icon = aCtl.ParentForm.Icon
    End Sub



    Private Sub ShowDetails()
        Dim lOper, lOperType As Integer
        Dim SDate(5), RDate(5), lBufferString, lTempString As String
        Dim lActionCount As Integer
        Dim lLayerFlag As Boolean
        Dim lId As Long


        ParGrid.Source = New atcControls.atcGridSource

        With ParGrid
            .Clear()
            .AllowHorizontalScrolling = False
            .AllowNewValidValues = True
            .Visible = True
            .Source.Columns = 2
        End With

        lId = lstPrac.SelectedIndex + 1

        lLayerFlag = False
        For lOper = 1 To cPracIDs.Count
            If cPracIDs(lOper) = lId Then
                If Mid(cPracRecs(lOper), 3, 6) = "UVNAME" Then
                    lLayerFlag = True
                End If
            End If
        Next

        If GroupProps.Visible = False Then
            GroupProps.Visible = True
        End If
        If lLayerFlag Then
            atxSurface.Visible = True
            atxUpper.Visible = True
            txtSurface.Visible = True
            txtUpper.Visible = True
            txtNA.Visible = False
        Else
            atxSurface.Visible = False
            atxUpper.Visible = False
            txtSurface.Visible = False
            txtUpper.Visible = False
            txtNA.Visible = True

        End If

        lActionCount = 0

        For lOper = 1 To cPracIDs.Count
            If cPracIDs(lOper) = lId Then
                'identify the type of record
                lBufferString = cPracRecs(lOper)
                If Len(lBufferString) = 0 Or InStr(lBufferString, "***") > 0 Then
                    lOperType = 6 ' hComment
                ElseIf Microsoft.VisualBasic.Left(Trim(lBufferString), 3) = "IF " Or _
                       Microsoft.VisualBasic.Left(Trim(lBufferString), 4) = "ELSE" Or _
                       Microsoft.VisualBasic.Left(Trim(lBufferString), 6) = "END IF" Then
                    lOperType = 5 'hCondition
                ElseIf Mid(lBufferString, 3, 6) = "DISTRB" Then
                    lOperType = 2 'hDistribute
                ElseIf Mid(lBufferString, 3, 6) = "UVNAME" Then
                    lOperType = 3 'hUserDefineName
                ElseIf Mid(lBufferString, 3, 6) = "UVQUAN" Then
                    lOperType = 4 'hUserDefineQuan
                Else
                    lOperType = 1 'hAction
                End If
                'extract info as needed
                If lOperType = 3 Then 'hUserDefineName
                    'assume that the uvname will have surface/upper distrib
                    atxSurface.ValueDouble = Mid(lBufferString, 37, 5)
                    atxUpper.ValueDouble = Mid(lBufferString, 67, 5)
                ElseIf lOperType = 1 Then
                    lActionCount = lActionCount + 1
                    'check if repeating
                    lTempString = Mid(lBufferString, 72, 2)
                    If lTempString = "YR" Then
                        comboRep.SelectedIndex = 1
                    ElseIf lTempString = "MO" Then
                        comboRep.SelectedIndex = 2
                    ElseIf lTempString = "DY" Then
                        comboRep.SelectedIndex = 3
                    ElseIf lTempString = "HR" Then
                        comboRep.SelectedIndex = 4
                    ElseIf lTempString = "MI" Then
                        comboRep.SelectedIndex = 5
                    Else
                        comboRep.SelectedIndex = 0
                    End If
                    'check for dated action
                    If Len(Trim(Mid(lBufferString, 21, 4))) > 0 Then
                        RDate(0) = CInt(Mid(lBufferString, 21, 4))
                        If Mid(lBufferString, 25, 3) = "   " Then
                            RDate(1) = 0
                        Else
                            RDate(1) = CInt(Mid(lBufferString, 25, 3))
                        End If
                        If Mid(lBufferString, 28, 3) = "   " Then
                            RDate(2) = 0
                        Else
                            RDate(2) = CInt(Mid(lBufferString, 28, 3))
                        End If
                        If Mid(lBufferString, 31, 3) = "   " Then
                            RDate(3) = 0
                        Else
                            RDate(3) = CInt(Mid(lBufferString, 31, 3))
                        End If
                        If Mid(lBufferString, 34, 3) = "   " Then
                            RDate(4) = 0
                        Else
                            RDate(4) = CInt(Mid(lBufferString, 34, 3))
                        End If
                        SDate(0) = pUci.GlobalBlock.SDate(0)
                        SDate(1) = pUci.GlobalBlock.SDate(1)
                        SDate(2) = pUci.GlobalBlock.SDate(2)
                        SDate(3) = pUci.GlobalBlock.SDate(3)
                        SDate(4) = pUci.GlobalBlock.SDate(4)
                        If Mid(lBufferString, 72, 2) = "DY" Then
                            'set date to start of run
                            atxYear.ValueInteger = SDate(0)
                            atxMo.ValueInteger = SDate(1)
                            atxDay.ValueInteger = SDate(2)
                            atxHr.ValueInteger = SDate(3)
                            atxMin.ValueInteger = SDate(4)
                        Else
                            'yearly repeating or other
                            RDate(0) = SDate(0)
                            'If Date2J(rDate) < Date2J(SDate) Then
                            '    'change to following year
                            '    rDate(0) = rDate(0) + 1
                            'End If
                            atxYear.ValueInteger = RDate(0)
                            atxMo.ValueInteger = RDate(1)
                            atxDay.ValueInteger = RDate(2)
                            atxHr.ValueInteger = RDate(3)
                            atxMin.ValueInteger = RDate(4)
                        End If
                    End If
                    'fill names and values
                    If lLayerFlag Then
                        ParGrid.Source.CellValue(lActionCount - 1, 0) = Mid(lBufferString, 43, 3)
                    Else
                        ParGrid.Source.CellValue(lActionCount - 1, 0) = Mid(lBufferString, 43, 6)
                    End If
                    ParGrid.Source.CellValue(lActionCount - 1, 1) = Mid(lBufferString, 61, 10)
                    If Len(Mid(lBufferString, 16, 2)) > 0 Then
                        'defer by default
                        chkDelay.Checked = True
                    End If
                End If
            End If
        Next

        For lRow As Integer = 0 To ParGrid.Source.Rows - 1
            ParGrid.Source.CellColor(lRow, 0) = SystemColors.ControlLight
        Next

        For lCol As Integer = 0 To ParGrid.Source.Rows - 1
            ParGrid.Source.CellEditable(lCol, 1) = True
        Next

        ParGrid.SizeAllColumnsToContents()
        ParGrid.Refresh()

    End Sub

    Private Sub lstPrac_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstPrac.SelectedIndexChanged
        If lstPrac.SelectedItems.Count = 1 And lstSeg.SelectedItems.Count = 1 Then
            ShowDetails()
        End If
    End Sub

    Private Sub lstSeg_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstSeg.SelectedIndexChanged
        If lstPrac.SelectedItems.Count = 1 And lstSeg.SelectedItems.Count = 1 Then
            ShowDetails()
        End If
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click
        Dim lnrepeat, lRecordCount As Integer
        Dim lLayerFlag As Boolean
        Dim lFirstAction As Boolean, lWroteAction As Boolean
        Dim rDate(5), SDate(5), EDate(5), lID, lLoopVar1, lLoopVar2, lDistribID, lPerLndId, lType As Integer
        Dim lUvq, lBufferString, lTempString, lSub1, lSub2 As String
        lUvq = ""    '.net conversion: Silences 'used before assigned' error
        lSub1 = ""   '.net conversion: Silences 'used before assigned' error
        lSub2 = ""   '.net conversion: Silences 'used before assigned' error

        Dim strid As String

        'if one practice and one land use selected, apply
        If lstPrac.SelectedItems.Count = 1 And lstSeg.SelectedItems.Count = 1 Then

            lTempString = Mid(lstSeg.Items.Item(lstSeg.SelectedIndex), 8)
            lPerLndId = StrRetRem(lTempString)

            If chkDelay.Checked = True Then
                'deferring, add uvquan and conditional
                lUvq = pCtl.UVQuanInUse("PERLND", lPerLndId)
                If Len(lUvq) = 0 Then
                    'find an unused uvquan name
                    lUvq = pCtl.NextUVQuanName("prec")
                    If Len(lUvq) = 5 Then
                        lUvq = lUvq & " "
                    End If
                    'write this one to spec act records
                    strid = RSet(CStr(lPerLndId), 3)
                    lBufferString = "  UVQUAN " & lUvq & " PERLND " & strid & " PREC             3                 DY  1 SUM"
                    pCtl.AddToBeginning(lBufferString, 4)
                End If
            End If

            lLayerFlag = False
            lFirstAction = True
            lWroteAction = False
            lDistribID = 0
            lID = lstPrac.Items.Count
            lRecordCount = 0
            For lLoopVar1 = 1 To cPracIDs.Count
                If cPracIDs(lLoopVar1) = lID Then
                    'identify the type of record
                    lBufferString = cPracRecs(lLoopVar1)
                    If Len(lBufferString) = 0 Or InStr(lBufferString, "***") > 0 Then
                        lType = 6
                    ElseIf Microsoft.VisualBasic.Left(Trim(lBufferString), 3) = "IF " Or _
                           Microsoft.VisualBasic.Left(Trim(lBufferString), 4) = "ELSE" Or _
                           Microsoft.VisualBasic.Left(Trim(lBufferString), 6) = "END IF" Then
                        lType = 5 'hcondition
                    ElseIf Mid(lBufferString, 3, 6) = "DISTRB" Then
                        lType = 2 'hDistribute
                        lDistribID = pCtl.NextDistribNumber
                    ElseIf Mid(lBufferString, 3, 6) = "UVNAME" Then
                        lType = 3 'hUserDefineName
                    ElseIf Mid(lBufferString, 3, 6) = "UVQUAN" Then
                        lType = 4 'hUserDefineQuan
                    Else
                        lType = 1 'hAction
                    End If
                    'modify this record as appropriate
                    If lType = 4 Then
                        'set operation number
                        strid = RSet(CStr(lPerLndId), 3)
                        lBufferString = Mid(lBufferString, 1, 23) & strid & Mid(lBufferString, 27)
                    ElseIf lType = 3 Then
                        'create var name from upper/surface split
                        lLayerFlag = True
                        strid = CStr(atxSurface.ValueDouble)
                        lLoopVar2 = InStr(1, strid, ".")
                        lSub1 = " "
                        If lLoopVar2 > 0 And Len(strid) >= lLoopVar2 + 1 Then
                            lSub1 = Mid(strid, lLoopVar2 + 1, 1)
                        End If
                        strid = CStr(atxUpper.ValueDouble)
                        lLoopVar2 = InStr(1, strid, ".")
                        lSub2 = " "
                        If lLoopVar2 > 0 And Len(strid) >= lLoopVar2 + 1 Then
                            lSub2 = Mid(strid, lLoopVar2 + 1, 1)
                        End If
                        lBufferString = Mid(lBufferString, 1, 13) & lSub1 & lSub2 & Mid(lBufferString, 16)
                        strid = RSet(CStr(atxSurface.ValueDouble), 5)
                        lBufferString = Mid(lBufferString, 1, 36) & strid & Mid(lBufferString, 42)
                        strid = RSet(CStr(atxUpper.ValueDouble), 5)
                        lBufferString = Mid(lBufferString, 1, 66) & strid & Mid(lBufferString, 72)
                    ElseIf lType = 2 Then
                        'change distrib id
                        strid = RSet(CStr(lDistribID), 3)
                        lBufferString = Mid(lBufferString, 1, 8) & strid & Mid(lBufferString, 12)
                    ElseIf lType = 1 Then
                        lRecordCount = lRecordCount + 1

                        If lFirstAction And chkDelay.Checked = True Then
                            'need to open conditional
                            pCtl.AddToEnd("IF (" & Trim(lUvq) & " < 0.11) THEN", 5)
                            lFirstAction = False
                        End If

                        'set operation number
                        strid = RSet(CStr(lPerLndId), 3)
                        lBufferString = Mid(lBufferString, 1, 8) & strid & Mid(lBufferString, 12)
                        'check if defering
                        If chkDelay.Checked = True Then
                            lBufferString = Mid(lBufferString, 1, 15) & "DY  1" & Mid(lBufferString, 21)
                        Else
                            lBufferString = Mid(lBufferString, 1, 15) & "     " & Mid(lBufferString, 21)
                        End If
                        'start year
                        RSet(CStr(atxYear.ValueInteger), 4)
                        lBufferString = Mid(lBufferString, 1, 20) & strid & Mid(lBufferString, 25)
                        'start mon
                        strid = RSet(CStr(atxMo.ValueInteger), 3)
                        If atxMo.ValueInteger <> 0 Then
                            lBufferString = Mid(lBufferString, 1, 24) & strid & Mid(lBufferString, 28)
                        Else
                            lBufferString = Mid(lBufferString, 1, 24) & "   " & Mid(lBufferString, 28)
                        End If
                        'start day
                        strid = RSet(CStr(atxDay.ValueInteger), 3)
                        If atxDay.ValueInteger <> 0 Then
                            lBufferString = Mid(lBufferString, 1, 27) & strid & Mid(lBufferString, 31)
                        Else
                            lBufferString = Mid(lBufferString, 1, 27) & "   " & Mid(lBufferString, 31)
                        End If
                        'start hr
                        strid = RSet(CStr(atxHr.ValueInteger), 3)
                        If atxMin.ValueInteger <> 0 Then
                            lBufferString = Mid(lBufferString, 1, 30) & strid & Mid(lBufferString, 34)
                        Else
                            lBufferString = Mid(lBufferString, 1, 30) & "   " & Mid(lBufferString, 34)
                        End If
                        'start min
                        strid = RSet(CStr(atxMin.ValueInteger), 3)
                        If atxMin.ValueInteger <> 0 Then
                            lBufferString = Mid(lBufferString, 1, 33) & strid & Mid(lBufferString, 37)
                        Else
                            lBufferString = Mid(lBufferString, 1, 33) & "   " & Mid(lBufferString, 37)
                        End If
                        'distrib id
                        If IsNumeric(Mid(lBufferString, 39, 3)) And lDistribID > 0 Then
                            RSet(CStr(lDistribID), 3)
                            lBufferString = Mid(lBufferString, 1, 35) & strid & Mid(lBufferString, 39)
                        End If
                        If lLayerFlag Then
                            'update uvname
                            lBufferString = Mid(lBufferString, 1, 45) & lSub1 & lSub2 & Mid(lBufferString, 48)
                        End If
                        'value
                        strid = RSet(CStr(ParGrid.Source.CellValue(lRecordCount - 1, 1)), 10)
                        lBufferString = Mid(lBufferString, 1, 60) & strid & Mid(lBufferString, 71)
                        'check if repeating
                        If comboRep.Items.Item(comboRep.SelectedIndex) <> "None" Then
                            'repeating
                            If Len(Mid(lBufferString, 75, 3)) = 0 Then
                                lBufferString = Mid(lBufferString, 1, 71) & comboRep.Items.Item(comboRep.SelectedIndex) & "   1" & Mid(lBufferString, 78)
                            Else
                                lBufferString = Mid(lBufferString, 1, 71) & comboRep.Items.Item(comboRep.SelectedIndex) & Mid(lBufferString, 74)
                            End If
                        Else
                            lBufferString = Mid(lBufferString, 1, 71) & "      " & Mid(lBufferString, 77)
                        End If
                        If Mid(lBufferString, 72, 2) = "YR" Then
                            'yearly repeating
                            'check number of repeats
                            EDate(0) = pUci.GlobalBlock.EDate(0)
                            EDate(1) = pUci.GlobalBlock.EDate(1)
                            EDate(2) = pUci.GlobalBlock.EDate(2)
                            EDate(3) = pUci.GlobalBlock.EDate(3)
                            EDate(4) = pUci.GlobalBlock.EDate(4)
                            SDate(0) = atxYear.ValueInteger
                            SDate(1) = atxMo.ValueInteger
                            SDate(2) = atxDay.ValueInteger
                            SDate(3) = atxHr.ValueInteger
                            SDate(4) = atxMin.ValueInteger
                            lnrepeat = Int((Date2J(EDate) - Date2J(SDate)) / 365)
                            strid = RSet(CStr(lnrepeat), 3)
                            lBufferString = Mid(lBufferString, 1, 77) & strid
                        End If
                        lWroteAction = True
                    End If
                    'add to spec act list
                    If lType = 3 Then
                        'check to make sure this name hasn't already been used
                        lTempString = Mid(lBufferString, 11, 5)
                        If Not pCtl.UVNameInUse(lTempString) Then
                            pCtl.AddToEnd(lBufferString, lType)
                        End If
                    Else
                        pCtl.AddToEnd(lBufferString, lType)
                    End If
                End If
            Next
            If lWroteAction And chkDelay.Checked = True Then
                'need to close conditional
                pCtl.AddToEnd("END IF", 5)
            End If
        Else
            Logger.Msg("One Practice and One Land Segment must be selected.", Microsoft.VisualBasic.MsgBoxStyle.OkOnly, "Add Ag Practice Problem")
        End If
        Me.Close()
    End Sub

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Me.Dispose()
    End Sub
End Class