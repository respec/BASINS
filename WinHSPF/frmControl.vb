Imports System.Drawing
Imports atcData
Imports atcUCI
Imports MapWinUtility
Imports atcUtility


Public Class frmControl

    Dim pPCheckBoxValues(11) As Integer
    Dim pICheckBoxValues(5) As Integer
    Dim pRCheckBoxValues(9) As Integer

    Dim pPChange As Boolean = False
    Dim pIChange As Boolean = False
    Dim pRChange As Boolean = False

    Dim defuci As HspfUci = pUCI



    Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.MinimumSize = Me.Size
        Me.Icon = pIcon

        MsgBox(defuci.Name)

        SSTabPIR.SelectTab(0)
        SSTabPIR.TabPages(0).Enabled = False
        SSTabPIR.TabPages(1).Enabled = False
        SSTabPIR.TabPages(2).Enabled = False


        If pUCI.OpnBlks("PERLND").Count > 0 Then
            SSTabPIR.TabPages(0).Enabled = True
            txtNoPERLND.Visible = False
            With pUCI.OpnBlks("PERLND").Tables("ACTIVITY")

                CheckBox1.Checked = .Parms(0).Value
                CheckBox2.Checked = .Parms(1).Value
                CheckBox3.Checked = .Parms(2).Value
                CheckBox4.Checked = .Parms(3).Value
                CheckBox5.Checked = .Parms(4).Value
                CheckBox6.Checked = .Parms(5).Value
                CheckBox7.Checked = .Parms(6).Value
                CheckBox8.Checked = .Parms(7).Value
                CheckBox9.Checked = .Parms(8).Value
                CheckBox10.Checked = .Parms(9).Value
                CheckBox11.Checked = .Parms(10).Value
                CheckBox12.Checked = .Parms(11).Value

            End With
        Else 'no perlnd
            SSTabPIR.TabPages(0).Enabled = False
            txtNoPERLND.Visible = True

            CheckBox1.Visible = False
            CheckBox2.Visible = False
            CheckBox3.Visible = False
            CheckBox4.Visible = False
            CheckBox5.Visible = False
            CheckBox6.Visible = False
            CheckBox7.Visible = False
            CheckBox8.Visible = False
            CheckBox9.Visible = False
            CheckBox10.Visible = False
            CheckBox11.Visible = False
            CheckBox12.Visible = False
            CheckBox13.Visible = False

        End If

        If pUCI.OpnBlks("IMPLND").Count > 0 Then
            SSTabPIR.TabPages(1).Enabled = True
            txtNoIMPLND.Visible = False
            With pUCI.OpnBlks("IMPLND").Tables("ACTIVITY")

                CheckBox13.Checked = .Parms(0).Value
                CheckBox14.Checked = .Parms(1).Value
                CheckBox15.Checked = .Parms(2).Value
                CheckBox16.Checked = .Parms(3).Value
                CheckBox17.Checked = .Parms(4).Value
                CheckBox18.Checked = .Parms(5).Value

            End With
        Else ' no implnd
            SSTabPIR.TabPages(1).Enabled = False
            txtNoIMPLND.Visible = True

            CheckBox13.Visible = False
            CheckBox14.Visible = False
            CheckBox15.Visible = False
            CheckBox16.Visible = False
            CheckBox17.Visible = False
            CheckBox18.Visible = False

        End If

        If pUCI.OpnBlks("RCHRES").Count > 0 Then
            SSTabPIR.TabPages(2).Enabled = True
            txtNoRCHRES.Visible = False
            With pUCI.OpnBlks("RCHRES").Tables("ACTIVITY")

                CheckBox19.Checked = .Parms(0).Value
                CheckBox20.Checked = .Parms(1).Value
                CheckBox21.Checked = .Parms(2).Value
                CheckBox22.Checked = .Parms(3).Value
                CheckBox23.Checked = .Parms(4).Value
                CheckBox24.Checked = .Parms(5).Value
                CheckBox25.Checked = .Parms(6).Value
                CheckBox26.Checked = .Parms(7).Value
                CheckBox27.Checked = .Parms(8).Value
                CheckBox28.Checked = .Parms(9).Value

            End With
        Else 'no rchres
            SSTabPIR.TabPages(2).Enabled = False
            txtNoIMPLND.Visible = True

            CheckBox19.Visible = False
            CheckBox20.Visible = False
            CheckBox21.Visible = False
            CheckBox22.Visible = False
            CheckBox23.Visible = False
            CheckBox24.Visible = False
            CheckBox25.Visible = False
            CheckBox26.Visible = False
            CheckBox27.Visible = False
            CheckBox28.Visible = False
        End If
        GenerateCheckValuesArray()


    End Sub


    Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click
        Dim lTable As HspfTable
        Dim lOpnBlk As HspfOperation
        Dim vOpnBlk As Object
        Dim lMsgResult As MsgBoxResult
        Dim i As Integer

        If (pIChange Or pRChange Or pPChange) Then
            'okay and something has changed
            For Each vOpnBlk In pUCI.OpnBlks("PERLND").Ids
                lOpnBlk = vOpnBlk
                lTable = lOpnBlk.Tables("ACTIVITY")
                For i = 0 To pPCheckBoxValues.Length - 1
                    lTable.Parms(i).Value = pPCheckBoxValues(i).ToString
                Next i
            Next vOpnBlk

            For Each vOpnBlk In pUCI.OpnBlks("IMPLND").Ids
                lOpnBlk = vOpnBlk
                lTable = lOpnBlk.Tables("ACTIVITY")

                For i = 0 To pICheckBoxValues.Length - 1
                    lTable.Parms(i).Value = pICheckBoxValues(i)
                Next i
            Next vOpnBlk

            For Each vOpnBlk In pUCI.OpnBlks("RCHRES").Ids
                lOpnBlk = vOpnBlk
                lTable = lOpnBlk.Tables("ACTIVITY")
                For i = 0 To pRCheckBoxValues.Length - 1
                    lTable.Parms(i).Value = pRCheckBoxValues(i)
                Next i
            Next vOpnBlk

            'query for updating tables
            If pPChange Then
                If AnyMissingTables("PERLND") Then
                    lMsgResult = Logger.Message("Changed have been made to the PERLND control cards, and additional tabled are required. Add the required tabled automatically?", "WinHSPF - Control Card Query", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, Windows.Forms.DialogResult.Yes)
                    If lMsgResult = MsgBoxResult.Yes Then
                        Call CheckAndAddMissingTables("PERLND")
                    End If
                End If
            End If
            If pIChange Then
                If AnyMissingTables("IMPLND") Then
                    lMsgResult = Logger.Message("Changed have been made to the IMPLND control cards, and additional tabled are required. Add the required tabled automatically?", "WinHSPF - Control Card Query", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, Windows.Forms.DialogResult.Yes)
                    If lMsgResult = MsgBoxResult.Yes Then
                        CheckAndAddMissingTables("IMPLND")
                    End If
                End If
            End If
            If pRChange Then
                If AnyMissingTables("RCHRES") Then
                    lMsgResult = Logger.Message("Changed have been made to the RCHRES control cards, and additional tabled are required. Add the required tabled automatically?", "WinHSPF - Control Card Query", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, Windows.Forms.DialogResult.Yes)
                    If lMsgResult = MsgBoxResult.Yes Then
                        CheckAndAddMissingTables("RCHRES")
                        UpdateFlagDependencies("RCHRES")
                    End If
                End If
            End If
            If pIChange Or pRChange Or pPChange Then
                Call SetMissingValuesToDefaults(pUCI, defuci)
            End If

            pUCI.Edited = True
        ElseIf pIChange Or pRChange Or pPChange Then
            lMsgResult = Logger.Message("Changed have been made to Control Cards. Disacrd Changes?", "WinHSPF - Control Card Query", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, Windows.Forms.DialogResult.No)
            If lMsgResult = MsgBoxResult.No Then
                Exit Sub
            End If
        End If
        Me.Dispose()
    End Sub

    Private Sub CheckIChange(ByVal Index)

    End Sub

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Me.Dispose()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        SSTabPIR.TabPages(1).Enabled = False
        txtNoIMPLND.Visible = True

        CheckBox13.Visible = False
        CheckBox14.Visible = False
        CheckBox15.Visible = False
        CheckBox16.Visible = False
        CheckBox17.Visible = False
        CheckBox18.Visible = False
    End Sub

    Private Sub GenerateCheckValuesArray()
        pPCheckBoxValues(0) = CheckBox1.CheckState
        pPCheckBoxValues(1) = CheckBox2.CheckState
        pPCheckBoxValues(2) = CheckBox3.CheckState
        pPCheckBoxValues(3) = CheckBox4.CheckState
        pPCheckBoxValues(4) = CheckBox5.CheckState
        pPCheckBoxValues(5) = CheckBox6.CheckState
        pPCheckBoxValues(6) = CheckBox7.CheckState
        pPCheckBoxValues(7) = CheckBox8.CheckState
        pPCheckBoxValues(8) = CheckBox9.CheckState
        pPCheckBoxValues(9) = CheckBox10.CheckState
        pPCheckBoxValues(10) = CheckBox11.CheckState
        pPCheckBoxValues(11) = CheckBox12.CheckState

        pICheckBoxValues(0) = CheckBox13.CheckState
        pICheckBoxValues(1) = CheckBox14.CheckState
        pICheckBoxValues(2) = CheckBox15.CheckState
        pICheckBoxValues(3) = CheckBox16.CheckState
        pICheckBoxValues(4) = CheckBox17.CheckState
        pICheckBoxValues(5) = CheckBox18.CheckState

        pRCheckBoxValues(0) = CheckBox19.CheckState
        pRCheckBoxValues(1) = CheckBox20.CheckState
        pRCheckBoxValues(2) = CheckBox21.CheckState
        pRCheckBoxValues(3) = CheckBox22.CheckState
        pRCheckBoxValues(4) = CheckBox23.CheckState
        pRCheckBoxValues(5) = CheckBox24.CheckState
        pRCheckBoxValues(6) = CheckBox25.CheckState
        pRCheckBoxValues(7) = CheckBox26.CheckState
        pRCheckBoxValues(8) = CheckBox27.CheckState
        pRCheckBoxValues(9) = CheckBox28.CheckState

    End Sub
    Private Sub ChecksChanged_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged, CheckBox2.CheckedChanged, CheckBox3.CheckedChanged, CheckBox4.CheckedChanged, CheckBox5.CheckedChanged, CheckBox6.CheckedChanged, CheckBox7.CheckedChanged, CheckBox8.CheckedChanged, CheckBox9.CheckedChanged, CheckBox10.CheckedChanged, CheckBox11.CheckedChanged, CheckBox12.CheckedChanged, CheckBox13.CheckedChanged, CheckBox14.CheckedChanged, CheckBox15.CheckedChanged, CheckBox16.CheckedChanged, CheckBox17.CheckedChanged, CheckBox18.CheckedChanged, CheckBox19.CheckedChanged, CheckBox20.CheckedChanged, CheckBox21.CheckedChanged, CheckBox22.CheckedChanged, CheckBox23.CheckedChanged, CheckBox24.CheckedChanged, CheckBox25.CheckedChanged, CheckBox26.CheckedChanged, CheckBox27.CheckedChanged, CheckBox28.CheckedChanged
        Dim lClickedCheckBox As Windows.Forms.CheckBox = sender

        If lClickedCheckBox.Checked = True Then
            Select Case lClickedCheckBox.Name

                'PERLND Checkboxes
                Case "CheckBox2"
                    CheckBox1.Checked = True
                    pPChange = True
                Case "CheckBox4"
                    CheckBox3.Checked = True
                    pPChange = True
                Case "CheckBox5"
                    CheckBox1.Checked = True
                    pPChange = True
                Case "CheckBox6"
                    CheckBox1.Checked = True
                    CheckBox3.Checked = True
                    CheckBox5.Checked = True
                    pPChange = True
                Case "CheckBox7"
                    CheckBox3.Checked = True
                    CheckBox4.Checked = True
                    pPChange = True
                Case "CheckBox9"
                    CheckBox3.Checked = True
                    CheckBox4.Checked = True
                    CheckBox8.Checked = True
                    pPChange = True
                Case "CheckBox10"
                    CheckBox1.Checked = True
                    CheckBox3.Checked = True
                    CheckBox4.Checked = True
                    CheckBox5.Checked = True
                    CheckBox8.Checked = True
                    pPChange = True
                Case "CheckBox11"
                    CheckBox1.Checked = True
                    CheckBox3.Checked = True
                    CheckBox4.Checked = True
                    CheckBox5.Checked = True
                    CheckBox8.Checked = True
                    pPChange = True
                Case "CheckBox12"
                    CheckBox3.Checked = True
                    CheckBox8.Checked = True
                    pPChange = True

                    'IMPLND Checkboxes
                Case "CheckBox14"
                    CheckBox13.Checked = True
                    pIChange = True
                Case "CheckBox16"
                    CheckBox15.Checked = True
                    pIChange = True
                Case "CheckBox17"
                    CheckBox13.Checked = True
                    CheckBox15.Checked = True
                    pIChange = True
                Case "CheckBox18"
                    CheckBox15.Checked = True
                    CheckBox16.Checked = True
                    pIChange = True

                    'RCHRES Checkboxes
                Case "CheckBox20"
                    CheckBox19.Checked = True
                    pRChange = True
                Case "CheckBox21"
                    CheckBox19.Checked = True
                    CheckBox20.Checked = True
                    pRChange = True
                Case "CheckBox22"
                    CheckBox19.Checked = True
                    CheckBox20.Checked = True
                    pRChange = True
                Case "CheckBox23"
                    CheckBox19.Checked = True
                    CheckBox20.Checked = True
                    CheckBox22.Checked = True
                    pRChange = True
                Case "CheckBox24"
                    CheckBox19.Checked = True
                    CheckBox20.Checked = True
                    CheckBox22.Checked = True
                    CheckBox23.Checked = True
                    pRChange = True
                Case "CheckBox25"
                    CheckBox19.Checked = True
                    CheckBox20.Checked = True
                    CheckBox22.Checked = True
                    pRChange = True
                Case "CheckBox26"
                    CheckBox19.Checked = True
                    CheckBox20.Checked = True
                    CheckBox22.Checked = True
                    CheckBox23.Checked = True
                    CheckBox25.Checked = True
                    pRChange = True
                Case "CheckBox27"
                    CheckBox19.Checked = True
                    CheckBox20.Checked = True
                    CheckBox22.Checked = True
                    CheckBox23.Checked = True
                    CheckBox25.Checked = True
                    CheckBox26.Checked = True
                    pRChange = True
                Case "CheckBox28"
                    CheckBox19.Checked = True
                    CheckBox20.Checked = True
                    CheckBox21.Checked = True
                    CheckBox22.Checked = True
                    CheckBox23.Checked = True
                    CheckBox25.Checked = True
                    CheckBox26.Checked = True
                    CheckBox27.Checked = True
                    pRChange = True
            End Select

            GenerateCheckValuesArray()

        End If

    End Sub

    Public Function AnyMissingTables(ByVal aOpName As String)
        Dim cTablesRequiredMissing As System.Collections.ObjectModel.Collection(Of HspfStatusType)
        Dim lOpnBlk As HspfOpnBlk
        Dim lOper As HspfOperation
        Dim vOper As Object

        AnyMissingTables = False
        lOpnBlk = pUCI.OpnBlks(aOpName)

        For Each vOper In lOpnBlk.Ids
            lOper = vOper  'setting the collection forces build of tablestatus
            cTablesRequiredMissing = lOper.TableStatus.GetInfo(1, False)
            lOper.TableStatus.Update() 'need to update in case we just changed flags
        Next vOper

        For Each vOper In lOpnBlk.Ids
            lOper = vOper
            cTablesRequiredMissing = lOper.TableStatus.GetInfo(1, False)
            If cTablesRequiredMissing.Count > 0 Then
                AnyMissingTables = True
            End If
        Next vOper

    End Function

    Public Sub CheckAndAddMissingTables(ByVal opname$)
        Dim cTablesRequiredMissing As System.Collections.ObjectModel.Collection(Of HspfStatusType)
        Dim lOpnBlk As HspfOpnBlk, lOper As HspfOperation, vOper As Object
        Dim vTableStatus As Object
        Dim lStatus As HspfStatusType
        Dim tabname$

        lOpnBlk = pUCI.OpnBlks(opname)

        For Each vOper In lOpnBlk.Ids
            lOper = vOper  'setting the collection forces build of tablestatus
            cTablesRequiredMissing = lOper.TableStatus.GetInfo(1, False)
            lOper.TableStatus.Update() 'need to update in case we just changed flags
        Next vOper

        For Each vOper In lOpnBlk.Ids
            lOper = vOper
            cTablesRequiredMissing = lOper.TableStatus.GetInfo(1, False)

            For Each vTableStatus In cTablesRequiredMissing
                lStatus = vTableStatus
                If lStatus.occur > 1 Then
                    tabname = lStatus.Name & ":" & lStatus.occur
                Else
                    tabname = lStatus.Name
                End If
                If lOpnBlk.Count > 0 Then
                    'double check to see if this table exists
                    If Not lOpnBlk.TableExists(tabname) Then
                        lOpnBlk.AddTableForAll(tabname, opname)
                        setDefaultsForTable(pUCI, defUci, opname, tabname)
                    End If
                End If
            Next vTableStatus
        Next vOper
        For Each vOper In lOpnBlk.Ids
            lOper = vOper
            lOper.TableStatus.Update()
        Next vOper
    End Sub

    Public Sub setDefaultsForTable(ByVal myUci As HspfUci, ByVal defUci As HspfUci, ByVal opname$, ByVal TableName$)
        Dim loptyp As HspfOpnBlk
        Dim vOpn As Object, lOpn As HspfOperation, dOpn As HspfOperation
        Dim lTab As HspfTable, dTab As HspfTable
        Dim vPar As Object
        Dim Id&

        If myUci.OpnBlks(opname).Count > 0 Then
            loptyp = myUci.OpnBlks(opname)
            For Each vOpn In loptyp.Ids
                lOpn = vOpn
                Id = DefaultOpnId(lOpn, defUci)
                If Id > 0 Then
                    dOpn = defUci.OpnBlks(lOpn.Name).operfromid(Id)
                    If Not dOpn Is Nothing Then
                        If lOpn.TableExists(TableName) Then
                            lTab = lOpn.tables(TableName)
                            If DefaultThisTable(loptyp.Name, lTab.Name) Then
                                If dOpn.TableExists(lTab.Name) Then
                                    dTab = dOpn.tables(lTab.Name)
                                    For Each vPar In lTab.Parms
                                        If DefaultThisParameter(loptyp.Name, lTab.Name, vPar.Name) Then
                                            If vPar.Value <> vPar.Name Then
                                                vPar.Value = dTab.Parms(vPar.Name).Value
                                            End If
                                        End If
                                    Next vPar
                                End If
                            End If
                        End If
                    End If
                End If
            Next vOpn
        End If

    End Sub

    Public Function DefaultOpnId(ByVal lOpn As HspfOperation, ByVal defUci As HspfUci) As Long
        Dim dOpn As HspfOperation
        If lOpn.DefOpnId <> 0 Then
            DefaultOpnId = lOpn.DefOpnId
        Else
            dOpn = matchOperWithDefault(lOpn.Name, lOpn.Description, defUci)
            If dOpn Is Nothing Then
                DefaultOpnId = 0
            Else
                DefaultOpnId = dOpn.Id
            End If
        End If
    End Function

    Private Function DefaultThisTable(ByVal OperName$, ByVal TableName$) As Boolean
        If OperName = "PERLND" Or OperName = "IMPLND" Then
            If TableName = "ACTIVITY" Or _
               TableName = "PRINT-INFO" Or _
               TableName = "GEN-INFO" Or _
               TableName = "PWAT-PARM5" Then
                DefaultThisTable = False
            ElseIf Microsoft.VisualBasic.Left(TableName, 4) = "QUAL" Then
                DefaultThisTable = False
            Else
                DefaultThisTable = True
            End If
        ElseIf OperName = "RCHRES" Then
            If TableName = "ACTIVITY" Or _
               TableName = "PRINT-INFO" Or _
               TableName = "GEN-INFO" Or _
               TableName = "HYDR-PARM1" Then
                DefaultThisTable = False
            ElseIf Microsoft.VisualBasic.Left(TableName, 3) = "GQ-" Then
                DefaultThisTable = False
            Else
                DefaultThisTable = True
            End If
        Else
            DefaultThisTable = False
        End If
    End Function

    Private Function DefaultThisParameter(ByVal OperName$, ByVal TableName$, ByVal ParmName$) As Boolean
        DefaultThisParameter = True
        If OperName = "PERLND" Then
            If TableName = "PWAT-PARM2" Then
                If ParmName = "SLSUR" Or ParmName = "LSUR" Then
                    DefaultThisParameter = False
                End If
            ElseIf TableName = "NQUALS" Then
                If ParmName = "NQUAL" Then
                    DefaultThisParameter = False
                End If
            End If
        ElseIf OperName = "IMPLND" Then
            If TableName = "IWAT-PARM2" Then
                If ParmName = "SLSUR" Or ParmName = "LSUR" Then
                    DefaultThisParameter = False
                End If
            ElseIf TableName = "NQUALS" Then
                If ParmName = "NQUAL" Then
                    DefaultThisParameter = False
                End If
            End If
        ElseIf OperName = "RCHRES" Then
            If TableName = "HYDR-PARM2" Then
                If ParmName = "LEN" Or _
                   ParmName = "DELTH" Or _
                   ParmName = "FTBUCI" Then
                    DefaultThisParameter = False
                End If
            ElseIf TableName = "GQ-GENDATA" Then
                If ParmName = "NGQUAL" Then
                    DefaultThisParameter = False
                End If
            End If
        End If
    End Function

    Public Function matchOperWithDefault(ByVal OpTypName As String, ByVal OpnDesc As String, ByVal defUci As HspfUci) As HspfOperation
        Dim vOpn As Object
        Dim lOpn As HspfOperation
        Dim ctemp As String

        For Each vOpn In defUci.OpnBlks(OpTypName).Ids
            lOpn = vOpn
            If lOpn.Description = OpnDesc Then
                matchOperWithDefault = lOpn
                Exit Function
            End If
        Next vOpn
        'a complete match not found, look for partial
        For Each vOpn In defUci.OpnBlks(OpTypName).Ids
            lOpn = vOpn
            If Len(lOpn.Description) > Len(OpnDesc) Then
                ctemp = Microsoft.VisualBasic.Left(lOpn.Description, Len(OpnDesc))
                If ctemp = OpnDesc Then
                    matchOperWithDefault = lOpn
                    Exit Function
                End If
            ElseIf Len(lOpn.Description) < Len(OpnDesc) Then
                ctemp = Microsoft.VisualBasic.Left(OpnDesc, Len(lOpn.Description))
                If lOpn.Description = ctemp Then
                    matchOperWithDefault = lOpn
                    Exit Function
                End If
            End If
            If Len(OpnDesc) > 4 And Len(lOpn.Description) > 4 Then
                ctemp = Microsoft.VisualBasic.Left(OpnDesc, 4)
                If Microsoft.VisualBasic.Left(lOpn.Description, 4) = ctemp Then
                    matchOperWithDefault = lOpn
                    Exit Function
                End If
            End If
        Next vOpn
        'not found, use first one
        If defUci.OpnBlks(OpTypName).Count > 0 Then
            matchOperWithDefault = defUci.OpnBlks(OpTypName).Ids(1)
        Else
            matchOperWithDefault = Nothing
        End If
    End Function

    Public Sub UpdateFlagDependencies(ByVal opname$)
        Dim lOpnBlk As HspfOpnBlk
        Dim lOper As HspfOperation, vOper As Object

        lOpnBlk = pUCI.OpnBlks(opname)
        For Each vOper In lOpnBlk.Ids
            lOper = vOper
            If lOper.TableExists("ACTIVITY") Then
                If lOper.tables("ACTIVITY").Parms("SEDFG").Value = 1 Then
                    If lOper.TableExists("HYDR-PARM1") Then
                        'change aux flags
                        lOper.tables("HYDR-PARM1").Parms("AUX1FG").Value = 1
                        lOper.tables("HYDR-PARM1").Parms("AUX2FG").Value = 1
                        lOper.tables("HYDR-PARM1").Parms("AUX3FG").Value = 1
                    End If
                End If
                If lOper.tables("ACTIVITY").Parms("PLKFG").Value = 1 Then
                    If lOper.TableExists("NUT-FLAGS") Then
                        'change po4 flag
                        lOper.tables("NUT-FLAGS").Parms("PO4FG").Value = 1
                    End If
                End If
            End If
        Next vOper
    End Sub

    Public Sub SetMissingValuesToDefaults(ByVal myUci As HspfUci, ByVal defUci As HspfUci)
        Dim vOpTyp As Object, loptyp As HspfOpnBlk
        Dim vOpn As Object, lOpn As HspfOperation, dOpn As HspfOperation
        Dim vTab As Object, lTab As HspfTable, dTab As HspfTable
        Dim vPar As Object
        Dim Id&
        Dim OpTyps() As String = {"PERLND", "IMPLND", "RCHRES"}

        For Each vOpTyp In OpTyps
            If myUci.OpnBlks(vOpTyp).Count > 0 Then
                loptyp = myUci.OpnBlks(vOpTyp)
                For Each vOpn In loptyp.Ids
                    lOpn = vOpn
                    Id = DefaultOpnId(lOpn, defUci)
                    If Id > 0 Then
                        dOpn = defUci.OpnBlks(lOpn.Name).OperFromID(Id)
                        If Not dOpn Is Nothing Then
                            For Each vTab In lOpn.Tables
                                lTab = vTab
                                If dOpn.TableExists(lTab.Name) Then
                                    dTab = dOpn.Tables(lTab.Name)
                                    For Each vPar In lTab.Parms
                                        If vPar.Value.GetType.Name = "Double" AndAlso vPar.Value = -999.0# Then
                                            vPar.Value = dTab.Parms(vPar.Name).Value
                                        End If
                                    Next vPar
                                End If
                            Next vTab
                        End If
                    End If
                Next vOpn
            End If
        Next vOpTyp
    End Sub

End Class