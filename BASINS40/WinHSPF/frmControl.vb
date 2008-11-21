Imports atcUCI
Imports MapWinUtility

Public Class frmControl

    Dim pCheckBoxValues(2)() As Integer
    Dim pInitialCheckBoxValues(2)() As Integer

    Dim pMissingTables(1)() As String

    Dim pPChange As Boolean = False
    Dim pIChange As Boolean = False
    Dim pRChange As Boolean = False

    Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.MinimumSize = Me.Size
        Me.Icon = pIcon

        SSTabPIR.SelectTab(0)
        SSTabPIR.TabPages(0).Enabled = False
        SSTabPIR.TabPages(1).Enabled = False
        SSTabPIR.TabPages(2).Enabled = False

        ReDim pCheckBoxValues(0)(11)
        ReDim pCheckBoxValues(1)(5)
        ReDim pCheckBoxValues(2)(9)
        ReDim pInitialCheckBoxValues(0)(11)
        ReDim pInitialCheckBoxValues(1)(5)
        ReDim pInitialCheckBoxValues(2)(9)

        If pUCI.OpnBlks("PERLND").Count > 0 Then
            SSTabPIR.TabPages(0).Enabled = True
            txtNoPERLND.Visible = False
            With pUCI.OpnBlks("PERLND").Tables("ACTIVITY")

                pCheckBoxValues(0)(0) = .Parms(0).Value
                pCheckBoxValues(0)(1) = .Parms(1).Value
                pCheckBoxValues(0)(2) = .Parms(2).Value
                pCheckBoxValues(0)(3) = .Parms(3).Value
                pCheckBoxValues(0)(4) = .Parms(4).Value
                pCheckBoxValues(0)(5) = .Parms(5).Value
                pCheckBoxValues(0)(6) = .Parms(6).Value
                pCheckBoxValues(0)(7) = .Parms(7).Value
                pCheckBoxValues(0)(8) = .Parms(8).Value
                pCheckBoxValues(0)(9) = .Parms(9).Value
                pCheckBoxValues(0)(10) = .Parms(10).Value
                pCheckBoxValues(0)(11) = .Parms(11).Value

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

                pCheckBoxValues(1)(0) = .Parms(0).Value
                pCheckBoxValues(1)(1) = .Parms(1).Value
                pCheckBoxValues(1)(2) = .Parms(2).Value
                pCheckBoxValues(1)(3) = .Parms(3).Value
                pCheckBoxValues(1)(4) = .Parms(4).Value
                pCheckBoxValues(1)(5) = .Parms(5).Value

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

                pCheckBoxValues(2)(0) = .Parms(0).Value
                pCheckBoxValues(2)(1) = .Parms(1).Value
                pCheckBoxValues(2)(2) = .Parms(2).Value
                pCheckBoxValues(2)(3) = .Parms(3).Value
                pCheckBoxValues(2)(4) = .Parms(4).Value
                pCheckBoxValues(2)(5) = .Parms(5).Value
                pCheckBoxValues(2)(6) = .Parms(6).Value
                pCheckBoxValues(2)(7) = .Parms(7).Value
                pCheckBoxValues(2)(8) = .Parms(8).Value
                pCheckBoxValues(2)(9) = .Parms(9).Value

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

        pCheckBoxValues(0).CopyTo(pInitialCheckBoxValues(0), 0)
        pCheckBoxValues(1).CopyTo(pInitialCheckBoxValues(1), 0)
        pCheckBoxValues(2).CopyTo(pInitialCheckBoxValues(2), 0)

        CheckBox1.Checked = pCheckBoxValues(0)(0)
        CheckBox2.Checked = pCheckBoxValues(0)(1)
        CheckBox3.Checked = pCheckBoxValues(0)(2)
        CheckBox4.Checked = pCheckBoxValues(0)(3)
        CheckBox5.Checked = pCheckBoxValues(0)(4)
        CheckBox6.Checked = pCheckBoxValues(0)(5)
        CheckBox7.Checked = pCheckBoxValues(0)(6)
        CheckBox8.Checked = pCheckBoxValues(0)(7)
        CheckBox9.Checked = pCheckBoxValues(0)(8)
        CheckBox10.Checked = pCheckBoxValues(0)(9)
        CheckBox11.Checked = pCheckBoxValues(0)(10)
        CheckBox12.Checked = pCheckBoxValues(0)(11)
        CheckBox13.Checked = pCheckBoxValues(1)(0)
        CheckBox14.Checked = pCheckBoxValues(1)(1)
        CheckBox15.Checked = pCheckBoxValues(1)(2)
        CheckBox16.Checked = pCheckBoxValues(1)(3)
        CheckBox17.Checked = pCheckBoxValues(1)(4)
        CheckBox18.Checked = pCheckBoxValues(1)(5)
        CheckBox19.Checked = pCheckBoxValues(2)(0)
        CheckBox20.Checked = pCheckBoxValues(2)(1)
        CheckBox21.Checked = pCheckBoxValues(2)(2)
        CheckBox22.Checked = pCheckBoxValues(2)(3)
        CheckBox23.Checked = pCheckBoxValues(2)(4)
        CheckBox24.Checked = pCheckBoxValues(2)(5)
        CheckBox25.Checked = pCheckBoxValues(2)(6)
        CheckBox26.Checked = pCheckBoxValues(2)(7)
        CheckBox27.Checked = pCheckBoxValues(2)(8)
        CheckBox28.Checked = pCheckBoxValues(2)(9)

        AddHandler CheckBox1.CheckedChanged, AddressOf CheckLogic
        AddHandler CheckBox2.CheckedChanged, AddressOf CheckLogic
        AddHandler CheckBox3.CheckedChanged, AddressOf CheckLogic
        AddHandler CheckBox4.CheckedChanged, AddressOf CheckLogic
        AddHandler CheckBox5.CheckedChanged, AddressOf CheckLogic
        AddHandler CheckBox6.CheckedChanged, AddressOf CheckLogic
        AddHandler CheckBox7.CheckedChanged, AddressOf CheckLogic
        AddHandler CheckBox8.CheckedChanged, AddressOf CheckLogic
        AddHandler CheckBox9.CheckedChanged, AddressOf CheckLogic
        AddHandler CheckBox10.CheckedChanged, AddressOf CheckLogic
        AddHandler CheckBox11.CheckedChanged, AddressOf CheckLogic
        AddHandler CheckBox12.CheckedChanged, AddressOf CheckLogic
        AddHandler CheckBox13.CheckedChanged, AddressOf CheckLogic
        AddHandler CheckBox14.CheckedChanged, AddressOf CheckLogic
        AddHandler CheckBox15.CheckedChanged, AddressOf CheckLogic
        AddHandler CheckBox16.CheckedChanged, AddressOf CheckLogic
        AddHandler CheckBox17.CheckedChanged, AddressOf CheckLogic
        AddHandler CheckBox18.CheckedChanged, AddressOf CheckLogic
        AddHandler CheckBox19.CheckedChanged, AddressOf CheckLogic
        AddHandler CheckBox20.CheckedChanged, AddressOf CheckLogic
        AddHandler CheckBox21.CheckedChanged, AddressOf CheckLogic
        AddHandler CheckBox22.CheckedChanged, AddressOf CheckLogic
        AddHandler CheckBox23.CheckedChanged, AddressOf CheckLogic
        AddHandler CheckBox24.CheckedChanged, AddressOf CheckLogic
        AddHandler CheckBox25.CheckedChanged, AddressOf CheckLogic
        AddHandler CheckBox26.CheckedChanged, AddressOf CheckLogic
        AddHandler CheckBox27.CheckedChanged, AddressOf CheckLogic
        AddHandler CheckBox28.CheckedChanged, AddressOf CheckLogic

    End Sub

    Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click
        Dim lTable As HspfTable
        Dim lOpnBlk As HspfOperation
        Dim lVOpnBlk As Object
        Dim lMsgResult As MsgBoxResult
        Dim lMissingTablesString As String = Nothing
        Dim lOper, lOper2 As Integer


        CompareChecksToLoadState()

        If (pIChange Or pRChange Or pPChange) Then
            'okay and something has changed
            For Each lVOpnBlk In pUCI.OpnBlks("PERLND").Ids
                lOpnBlk = lVOpnBlk
                lTable = lOpnBlk.Tables("ACTIVITY")
                For lOper2 = 0 To pCheckBoxValues(0).Length - 1
                    lTable.Parms(lOper2).Value = pCheckBoxValues(0)(lOper2).ToString
                Next lOper2
            Next lVOpnBlk

            For Each lVOpnBlk In pUCI.OpnBlks("IMPLND").Ids
                lOpnBlk = lVOpnBlk
                lTable = lOpnBlk.Tables("ACTIVITY")

                For lOper2 = 0 To pCheckBoxValues(1).Length - 1
                    lTable.Parms(lOper2).Value = pCheckBoxValues(1)(lOper2)
                Next lOper2
            Next lVOpnBlk

            For Each lVOpnBlk In pUCI.OpnBlks("RCHRES").Ids
                lOpnBlk = lVOpnBlk
                lTable = lOpnBlk.Tables("ACTIVITY")
                For lOper2 = 0 To pCheckBoxValues(2).Length - 1
                    lTable.Parms(lOper2).Value = pCheckBoxValues(2)(lOper2)
                Next lOper2
            Next lVOpnBlk

            'query for updating tables
            If pPChange Then
                If AnyMissingTables("PERLND") Then

                    For lOper = 0 To pMissingTables(0).Length - 1
                        If pMissingTables(1)(lOper) > 1 Then
                            lMissingTablesString = String.Concat(lMissingTablesString, pMissingTables(0)(lOper) & " (" & pMissingTables(1)(lOper) & " Occurances)" & vbCrLf)
                        Else
                            lMissingTablesString = String.Concat(lMissingTablesString, pMissingTables(0)(lOper) & vbCrLf)
                        End If
                    Next

                    lMsgResult = Logger.Message("Changed have been made to the PERLND control cards. The Following Tables are Required:" & vbCrLf & vbCrLf & lMissingTablesString & vbCrLf & "Add the required tables automatically?", "WinHSPF - Control Card Query", MessageBoxButtons.YesNo, MessageBoxIcon.Question, Windows.Forms.DialogResult.Yes)
                    If lMsgResult = MsgBoxResult.Yes Then
                        Call CheckAndAddMissingTables("PERLND")
                    End If
                End If
            End If
            If pIChange Then
                If AnyMissingTables("IMPLND") Then

                    For lOper = 0 To pMissingTables(0).Length - 1
                        If pMissingTables(1)(lOper) > 1 Then
                            lMissingTablesString = String.Concat(lMissingTablesString, pMissingTables(0)(lOper) & " (" & pMissingTables(1)(lOper) & " Occurances)" & vbCrLf)
                        Else
                            lMissingTablesString = String.Concat(lMissingTablesString, pMissingTables(0)(lOper) & vbCrLf)
                        End If
                    Next

                    lMsgResult = Logger.Message("Changed have been made to the IMPLND control cards. The Following Tables are Required:" & vbCrLf & vbCrLf & lMissingTablesString & vbCrLf & "Add the required tables automatically?", "WinHSPF - Control Card Query", MessageBoxButtons.YesNo, MessageBoxIcon.Question, Windows.Forms.DialogResult.Yes)
                    If lMsgResult = MsgBoxResult.Yes Then
                        CheckAndAddMissingTables("IMPLND")
                    End If
                End If
            End If
            If pRChange Then
                If AnyMissingTables("RCHRES") Then

                    For lOper = 0 To pMissingTables(0).Length - 1
                        If pMissingTables(1)(lOper) > 1 Then
                            lMissingTablesString = String.Concat(lMissingTablesString, pMissingTables(0)(lOper) & " (" & pMissingTables(1)(lOper) & " Occurances)" & vbCrLf)
                        Else
                            lMissingTablesString = String.Concat(lMissingTablesString, pMissingTables(0)(lOper) & vbCrLf)
                        End If
                    Next

                    lMsgResult = Logger.Message("Changed have been made to the RCHRES control cards. The Following Tables are Required:" & vbCrLf & vbCrLf & lMissingTablesString & vbCrLf & "Add the required tables automatically?", "WinHSPF - Control Card Query", MessageBoxButtons.YesNo, MessageBoxIcon.Question, Windows.Forms.DialogResult.Yes)
                    If lMsgResult = MsgBoxResult.Yes Then
                        CheckAndAddMissingTables("RCHRES")
                        UpdateFlagDependencies("RCHRES")
                    End If
                End If
            End If
            If pIChange Or pRChange Or pPChange Then
                Call SetMissingValuesToDefaults(pUCI, pDefUCI)
            End If

            pUCI.Edited = True

        End If
        Me.Dispose()
    End Sub

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Dim lMsgResult As Windows.Forms.DialogResult = Nothing

        CompareChecksToLoadState()

        If pIChange Or pRChange Or pPChange Then
            lMsgResult = Logger.Message("Changed have been made to Control Cards. Disacrd Changes?", "WinHSPF - Control Card Query", MessageBoxButtons.YesNo, MessageBoxIcon.Question, Windows.Forms.DialogResult.No)
            If lMsgResult = MsgBoxResult.Yes Then
                Me.Dispose()
            End If
        Else
            Me.Dispose()
        End If
    End Sub

    Public Sub CompareChecksToLoadState()
        Dim lOper As Integer

        For lOper = 0 To 11
            If pCheckBoxValues(0)(lOper) <> pInitialCheckBoxValues(0)(lOper) Then
                pPChange = True
            End If
        Next
        For lOper = 0 To 5
            If pCheckBoxValues(1)(lOper) <> pInitialCheckBoxValues(1)(lOper) Then
                pIChange = True
            End If
        Next
        For lOper = 0 To 9
            If pCheckBoxValues(2)(lOper) <> pInitialCheckBoxValues(2)(lOper) Then
                pRChange = True
            End If
        Next

    End Sub

    Private Sub CheckLogic(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim lClickedCheckBox As Windows.Forms.CheckBox = sender

        Select Case lClickedCheckBox.Name

            'PERLND Checkboxes
            Case "CheckBox1"
                pCheckBoxValues(0)(0) = CheckBox1.CheckState
            Case "CheckBox2"
                pCheckBoxValues(0)(1) = CheckBox2.CheckState
                If lClickedCheckBox.Checked = True Then
                    CheckBox1.Checked = True
                End If
            Case "CheckBox3"
                pCheckBoxValues(0)(2) = CheckBox3.CheckState
            Case "CheckBox4"
                pCheckBoxValues(0)(3) = CheckBox4.CheckState
                If lClickedCheckBox.Checked = True Then
                    CheckBox3.Checked = True
                End If
            Case "CheckBox5"
                pCheckBoxValues(0)(4) = CheckBox5.CheckState
                If lClickedCheckBox.Checked = True Then
                    CheckBox1.Checked = True
                End If
            Case "CheckBox6"
                pCheckBoxValues(0)(5) = CheckBox6.CheckState
                If lClickedCheckBox.Checked = True Then
                    CheckBox1.Checked = True
                    CheckBox3.Checked = True
                    CheckBox5.Checked = True
                End If
            Case "CheckBox7"
                pCheckBoxValues(0)(6) = CheckBox7.CheckState
                If lClickedCheckBox.Checked = True Then
                    CheckBox3.Checked = True
                    CheckBox4.Checked = True
                End If
            Case "CheckBox8"
                pCheckBoxValues(0)(7) = CheckBox8.CheckState
            Case "CheckBox9"
                pCheckBoxValues(0)(8) = CheckBox9.CheckState
                If lClickedCheckBox.Checked = True Then
                    CheckBox3.Checked = True
                    CheckBox4.Checked = True
                    CheckBox8.Checked = True
                End If
            Case "CheckBox10"
                pCheckBoxValues(0)(9) = CheckBox10.CheckState
                If lClickedCheckBox.Checked = True Then
                    CheckBox1.Checked = True
                    CheckBox3.Checked = True
                    CheckBox4.Checked = True
                    CheckBox5.Checked = True
                    CheckBox8.Checked = True
                End If
            Case "CheckBox11"
                pCheckBoxValues(0)(10) = CheckBox11.CheckState
                If lClickedCheckBox.Checked = True Then
                    CheckBox1.Checked = True
                    CheckBox3.Checked = True
                    CheckBox4.Checked = True
                    CheckBox5.Checked = True
                    CheckBox8.Checked = True
                End If
            Case "CheckBox12"
                pCheckBoxValues(0)(11) = CheckBox12.CheckState
                If lClickedCheckBox.Checked = True Then
                    CheckBox3.Checked = True
                    CheckBox8.Checked = True
                End If

                'IMPLND Checkboxes
            Case "CheckBox13"
                pCheckBoxValues(1)(0) = CheckBox13.CheckState
            Case "CheckBox14"
                pCheckBoxValues(1)(1) = CheckBox14.CheckState
                If lClickedCheckBox.Checked = True Then
                    CheckBox13.Checked = True
                End If
            Case "CheckBox15"
                pCheckBoxValues(1)(2) = CheckBox15.CheckState
            Case "CheckBox16"
                pCheckBoxValues(1)(3) = CheckBox16.CheckState
                If lClickedCheckBox.Checked = True Then
                    CheckBox15.Checked = True
                End If
            Case "CheckBox17"
                pCheckBoxValues(1)(4) = CheckBox17.CheckState
                If lClickedCheckBox.Checked = True Then
                    CheckBox13.Checked = True
                    CheckBox15.Checked = True
                End If
            Case "CheckBox18"
                pCheckBoxValues(1)(5) = CheckBox18.CheckState
                If lClickedCheckBox.Checked = True Then
                    CheckBox15.Checked = True
                    CheckBox16.Checked = True
                End If

                'RCHRES Checkboxes
            Case "CheckBox19"
                pCheckBoxValues(2)(0) = CheckBox19.CheckState
            Case "CheckBox20"
                pCheckBoxValues(2)(1) = CheckBox20.CheckState
                If lClickedCheckBox.Checked = True Then
                    CheckBox19.Checked = True
                End If
            Case "CheckBox21"
                pCheckBoxValues(2)(2) = CheckBox21.CheckState
                If lClickedCheckBox.Checked = True Then
                    CheckBox19.Checked = True
                    CheckBox20.Checked = True
                End If
            Case "CheckBox22"
                pCheckBoxValues(2)(3) = CheckBox22.CheckState
                If lClickedCheckBox.Checked = True Then
                    CheckBox19.Checked = True
                    CheckBox20.Checked = True
                End If
            Case "CheckBox23"
                pCheckBoxValues(2)(4) = CheckBox23.CheckState
                If lClickedCheckBox.Checked = True Then
                    CheckBox19.Checked = True
                    CheckBox20.Checked = True
                    CheckBox22.Checked = True
                End If
            Case "CheckBox24"
                pCheckBoxValues(2)(5) = CheckBox24.CheckState
                If lClickedCheckBox.Checked = True Then
                    CheckBox19.Checked = True
                    CheckBox20.Checked = True
                    CheckBox22.Checked = True
                    CheckBox23.Checked = True
                End If
            Case "CheckBox25"
                pCheckBoxValues(2)(6) = CheckBox25.CheckState
                If lClickedCheckBox.Checked = True Then
                    CheckBox19.Checked = True
                    CheckBox20.Checked = True
                    CheckBox22.Checked = True
                End If
            Case "CheckBox26"
                pCheckBoxValues(2)(7) = CheckBox26.CheckState
                If lClickedCheckBox.Checked = True Then
                    CheckBox19.Checked = True
                    CheckBox20.Checked = True
                    CheckBox22.Checked = True
                    CheckBox23.Checked = True
                    CheckBox25.Checked = True
                End If
            Case "CheckBox27"
                pCheckBoxValues(2)(8) = CheckBox27.CheckState
                If lClickedCheckBox.Checked = True Then
                    CheckBox19.Checked = True
                    CheckBox20.Checked = True
                    CheckBox22.Checked = True
                    CheckBox23.Checked = True
                    CheckBox25.Checked = True
                    CheckBox26.Checked = True
                End If
            Case "CheckBox28"
                pCheckBoxValues(2)(8) = CheckBox28.CheckState
                If lClickedCheckBox.Checked = True Then
                    CheckBox19.Checked = True
                    CheckBox20.Checked = True
                    CheckBox21.Checked = True
                    CheckBox22.Checked = True
                    CheckBox23.Checked = True
                    CheckBox25.Checked = True
                    CheckBox26.Checked = True
                    CheckBox27.Checked = True
                End If
        End Select

    End Sub

    Public Function AnyMissingTables(ByVal aOpName As String) As Boolean
        Dim lTablesRequiredMissing As System.Collections.ObjectModel.Collection(Of HspfStatusType)
        Dim lOpnBlk As HspfOpnBlk
        Dim lOper As HspfOperation
        Dim lVOper As Object
        Dim lMissingTableIndex As Integer

        AnyMissingTables = Nothing
        lOpnBlk = pUCI.OpnBlks(aOpName)

        For Each lVOper In lOpnBlk.Ids
            lOper = lVOper  'setting the collection forces build of tablestatus
            lTablesRequiredMissing = lOper.TableStatus.GetInfo(1, False)
            lOper.TableStatus.Update() 'need to update in case we just changed flags
        Next lVOper

        For Each lVOper In lOpnBlk.Ids
            lOper = lVOper
            lTablesRequiredMissing = lOper.TableStatus.GetInfo(1, False)
            If lTablesRequiredMissing.Count > 0 Then
                For Each lMissingTable As Object In lTablesRequiredMissing
                    If pMissingTables(0) Is Nothing Then
                        ReDim pMissingTables(0)(0)
                        ReDim pMissingTables(1)(0)
                        pMissingTables(0)(0) = lMissingTable.Name
                        pMissingTables(1)(0) = lMissingTable.Occur
                    Else
                        lMissingTableIndex = Array.IndexOf(pMissingTables(0), lMissingTable.Name)
                        If lMissingTableIndex = -1 Then
                            ReDim Preserve pMissingTables(0)(UBound(pMissingTables(0)) + 1)
                            ReDim Preserve pMissingTables(1)(UBound(pMissingTables(1)) + 1)
                            pMissingTables(0)(UBound(pMissingTables(0))) = lMissingTable.Name
                            pMissingTables(1)(UBound(pMissingTables(1))) = lMissingTable.Occur
                        ElseIf lMissingTable.Occur > pMissingTables(1)(lMissingTableIndex) Then
                            pMissingTables(1)(lMissingTableIndex) = lMissingTable.Occur
                        End If
                    End If
                Next
                Return True
            End If
        Next lVOper

    End Function


    Public Sub CheckAndAddMissingTables(ByVal opname As String)
        Dim lTablesRequiredMissing As System.Collections.ObjectModel.Collection(Of HspfStatusType)
        Dim lOpnBlk As HspfOpnBlk
        Dim lOper As HspfOperation
        Dim lVOper As Object
        Dim lVTableStatus As Object
        Dim lStatus As HspfStatusType
        Dim lTabname As String

        lOpnBlk = pUCI.OpnBlks(opname)

        For Each lVOper In lOpnBlk.Ids
            lOper = lVOper  'setting the collection forces build of tablestatus
            lTablesRequiredMissing = lOper.TableStatus.GetInfo(1, False)
            lOper.TableStatus.Update() 'need to update in case we just changed flags
        Next lVOper

        For Each lVOper In lOpnBlk.Ids
            lOper = lVOper
            lTablesRequiredMissing = lOper.TableStatus.GetInfo(1, False)

            For Each lVTableStatus In lTablesRequiredMissing
                lStatus = lVTableStatus
                If lStatus.Occur > 1 Then
                    lTabname = lStatus.Name & ":" & lStatus.Occur
                Else
                    lTabname = lStatus.Name
                End If
                If lOpnBlk.Count > 0 Then
                    'double check to see if this table exists
                    If Not lOpnBlk.TableExists(lTabname) Then
                        lOpnBlk.AddTableForAll(lTabname, opname)
                        SetDefaultsForTable(pUCI, pDefUCI, opname, lTabname)
                    End If
                End If
            Next lVTableStatus
        Next lVOper
        For Each lVOper In lOpnBlk.Ids
            lOper = lVOper
            lOper.TableStatus.Update()
        Next lVOper
    End Sub

    Public Sub SetDefaultsForTable(ByVal pUCI As HspfUci, ByVal aDefUCI As HspfUci, ByVal opname As String, ByVal TableName As String)
        Dim lOptyp As HspfOpnBlk
        Dim lVOpn As Object
        Dim lOpn As HspfOperation
        Dim lDOpn As HspfOperation
        Dim lTab As HspfTable
        Dim lDTab As HspfTable
        Dim lVPar As Object
        Dim Id As Integer

        If pUCI.OpnBlks(opname).Count > 0 Then
            lOptyp = pUCI.OpnBlks(opname)
            For Each lVOpn In lOptyp.Ids
                lOpn = lVOpn
                Id = DefaultOpnId(lOpn, aDefUCI)
                If Id > 0 Then
                    lDOpn = aDefUCI.OpnBlks(lOpn.Name).OperFromID(Id)
                    If Not lDOpn Is Nothing Then
                        If lOpn.TableExists(TableName) Then
                            lTab = lOpn.Tables(TableName)
                            If DefaultThisTable(lOptyp.Name, lTab.Name) Then
                                If lDOpn.TableExists(lTab.Name) Then
                                    lDTab = lDOpn.Tables(lTab.Name)
                                    For Each lVPar In lTab.Parms
                                        If DefaultThisParameter(lOptyp.Name, lTab.Name, lVPar.Name) Then
                                            If lVPar.Value <> lVPar.Name Then
                                                lVPar.Value = lDTab.Parms(lVPar.Name).Value
                                            End If
                                        End If
                                    Next lVPar
                                End If
                            End If
                        End If
                    End If
                End If
            Next lVOpn
        End If

    End Sub

    Public Function DefaultOpnId(ByVal lOpn As HspfOperation, ByVal aDefUCI As HspfUci) As Long
        Dim lDOpn As HspfOperation

        If lOpn.DefOpnId <> 0 Then
            DefaultOpnId = lOpn.DefOpnId
        Else
            lDOpn = matchOperWithDefault(lOpn.Name, lOpn.Description, aDefUCI)
            If lDOpn Is Nothing Then
                DefaultOpnId = 0
            Else
                DefaultOpnId = lDOpn.Id
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

    Public Function matchOperWithDefault(ByVal OpTypName As String, ByVal OpnDesc As String, ByVal aDefUCI As HspfUci) As HspfOperation
        Dim lVOpn As Object
        Dim lOpn As HspfOperation
        Dim lTempString As String

        For Each lVOpn In aDefUCI.OpnBlks(OpTypName).Ids
            lOpn = lVOpn
            If lOpn.Description = OpnDesc Then
                matchOperWithDefault = lOpn
                Exit Function
            End If
        Next lVOpn
        'a complete match not found, look for partial
        For Each lVOpn In aDefUCI.OpnBlks(OpTypName).Ids
            lOpn = lVOpn
            If Len(lOpn.Description) > Len(OpnDesc) Then
                lTempString = Microsoft.VisualBasic.Left(lOpn.Description, Len(OpnDesc))
                If lTempString = OpnDesc Then
                    matchOperWithDefault = lOpn
                    Exit Function
                End If
            ElseIf Len(lOpn.Description) < Len(OpnDesc) Then
                lTempString = Microsoft.VisualBasic.Left(OpnDesc, Len(lOpn.Description))
                If lOpn.Description = lTempString Then
                    matchOperWithDefault = lOpn
                    Exit Function
                End If
            End If
            If Len(OpnDesc) > 4 And Len(lOpn.Description) > 4 Then
                lTempString = Microsoft.VisualBasic.Left(OpnDesc, 4)
                If Microsoft.VisualBasic.Left(lOpn.Description, 4) = lTempString Then
                    matchOperWithDefault = lOpn
                    Exit Function
                End If
            End If
        Next lVOpn
        'not found, use first one
        If aDefUCI.OpnBlks(OpTypName).Count > 0 Then
            matchOperWithDefault = aDefUCI.OpnBlks(OpTypName).Ids(1)
        Else
            matchOperWithDefault = Nothing
        End If
    End Function

    Public Sub UpdateFlagDependencies(ByVal opname$)
        Dim lOpnBlk As HspfOpnBlk
        Dim lOper As HspfOperation
        Dim lVOper As Object

        lOpnBlk = pUCI.OpnBlks(opname)
        For Each lVOper In lOpnBlk.Ids
            lOper = lVOper
            If lOper.TableExists("ACTIVITY") Then
                If lOper.Tables("ACTIVITY").Parms("SEDFG").Value = 1 Then
                    If lOper.TableExists("HYDR-PARM1") Then
                        'change aux flags
                        lOper.Tables("HYDR-PARM1").Parms("AUX1FG").Value = 1
                        lOper.Tables("HYDR-PARM1").Parms("AUX2FG").Value = 1
                        lOper.Tables("HYDR-PARM1").Parms("AUX3FG").Value = 1
                    End If
                End If
                If lOper.Tables("ACTIVITY").Parms("PLKFG").Value = 1 Then
                    If lOper.TableExists("NUT-FLAGS") Then
                        'change po4 flag
                        lOper.Tables("NUT-FLAGS").Parms("PO4FG").Value = 1
                    End If
                End If
            End If
        Next lVOper
    End Sub

    Public Sub SetMissingValuesToDefaults(ByVal pUCI As HspfUci, ByVal aDefUCI As HspfUci)
        Dim lVOpTyp As Object
        Dim lOptyp As HspfOpnBlk
        Dim lVOpn As Object
        Dim lOpn As HspfOperation
        Dim lDOpn As HspfOperation
        Dim lVTab As Object
        Dim lTab As HspfTable
        Dim lDTab As HspfTable
        Dim lVPar As Object
        Dim Id As Integer
        Dim lOpTyps() As String = {"PERLND", "IMPLND", "RCHRES"}

        For Each lVOpTyp In lOpTyps
            If pUCI.OpnBlks(lVOpTyp).Count > 0 Then
                lOptyp = pUCI.OpnBlks(lVOpTyp)
                For Each lVOpn In lOptyp.Ids
                    lOpn = lVOpn
                    Id = DefaultOpnId(lOpn, aDefUCI)
                    If Id > 0 Then
                        lDOpn = aDefUCI.OpnBlks(lOpn.Name).OperFromID(Id)
                        If Not lDOpn Is Nothing Then
                            For Each lVTab In lOpn.Tables
                                lTab = lVTab
                                If lDOpn.TableExists(lTab.Name) Then
                                    lDTab = lDOpn.Tables(lTab.Name)
                                    For Each lVPar In lTab.Parms
                                        If lVPar.Value.GetType.Name = "Double" AndAlso lVPar.Value = -999.0# Then
                                            lVPar.Value = lDTab.Parms(lVPar.Name).Value
                                        End If
                                    Next lVPar
                                End If
                            Next lVTab
                        End If
                    End If
                Next lVOpn
            End If
        Next lVOpTyp
    End Sub

    'Private Function MatchString(ByVal aString As String) As Boolean
    '    If aString = pMatchString Then
    '        Return True
    '    Else
    '        Return False
    '    End If
    'End Function
End Class